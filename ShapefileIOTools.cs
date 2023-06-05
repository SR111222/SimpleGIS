using MyMapObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace SimpleGIS
{
    internal class ShapefileIOTools
    {
        #region 程序集方法

        /// <summary>
        /// 从shp文件中读取地图图层
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        internal static MyMapObjects.moMapLayer LoadMapLayer(string filePath)
        {
            try
            {
                //获取文件名称
                string shpname = System.IO.Path.GetFileNameWithoutExtension(filePath);
                //坐标文件中的对应记录的起始位置相对于坐标文件起始位置的位移量
                int[] feaBias;
                //记录段长度
                int[] feaLength;

                //读取shx文件
                using(FileStream shx = new FileStream(filePath.Substring(0,filePath.IndexOf(".shp"))+".shx",FileMode.Open,FileAccess.Read))
                {
                    using (BinaryReader br = new BinaryReader(shx))
                    {
                        //文件头长度固定100字节
                        br.BaseStream.Seek(100,SeekOrigin.Begin);
                        long count = (br.BaseStream.Length - 100) / 8;
                        //读取位移量
                        feaBias = new int[count];
                        //读取记录段长度
                        feaLength = new int[count];
                        byte[] data;
                        for(long i = 0; i<count;i++)
                        {
                            data = br.ReadBytes(8).Reverse().ToArray();
                            feaLength[i] = 2 * BitConverter.ToInt32(data, 0);
                            feaBias[i] = 2 * BitConverter.ToInt32(data, 4);
                        }
                    }
                }
                //读取shp文件
                using (FileStream shp = new FileStream(filePath.Substring(0,filePath.IndexOf(".shp"))+ ".shp",FileMode.Open,FileAccess.Read))
                {
                    using (BinaryReader br = new BinaryReader(shp))
                    {
                        //读取dbf文件
                        using (FileStream dbf = new FileStream(filePath.Substring(0,filePath.IndexOf(".shp"))+".dbf",FileMode.Open,FileAccess.Read))
                        {
                            using (BinaryReader br_dbf = new BinaryReader(dbf))
                            {
                                //第32位记录shapeType
                                br.BaseStream.Seek(32,SeekOrigin.Begin);
                                int typeInt = br.ReadInt32();
                                MyMapObjects.moGeometryTypeConstant sGeometryType = new MyMapObjects.moGeometryTypeConstant();
                                switch(typeInt)
                                {
                                    case 3:
                                    case 13:
                                        sGeometryType = MyMapObjects.moGeometryTypeConstant.MultiPolyline;
                                        break;
                                    case 5:
                                        sGeometryType = MyMapObjects.moGeometryTypeConstant.MultiPolygon;
                                        break;
                                    default:
                                        sGeometryType = MyMapObjects.moGeometryTypeConstant.Point;
                                        break;
                                }

                                //读取最大外包矩形
                                double[] Envelope = new double[4];
                                Envelope[0] = br.ReadDouble();  //Xmin
                                Envelope[1] = br.ReadDouble();  //Ymin
                                Envelope[2] = br.ReadDouble();  //Xmax
                                Envelope[3] = br.ReadDouble();  //Xmin

                                MyMapObjects.moFields sFields = new MyMapObjects.moFields();

                                br_dbf.BaseStream.Seek(4, SeekOrigin.Begin);
                                //第4-7位，文件中的记录条数
                                int rowCount = br_dbf.ReadInt32();
                                //第8-9位，文件中的字节数
                                short firstRow = br_dbf.ReadInt16();
                                //第10-11位，一条记录中的字节数
                                short rowLength = br_dbf.ReadInt16();
                                //记录个数，每条记录32个字节
                                int attributeCount = (firstRow - 33) / 32;
                                byte[] fieldLength = new byte[attributeCount];
                                string fieldName;
                                char ch;

                                //依次读取dbf
                                for(int i=0;i<attributeCount; i++)
                                {
                                    //从第32位开始读取，每次间隔32位
                                    br_dbf.BaseStream.Seek(i * 32 + 32, SeekOrigin.Begin);
                                    //0-10为位，记录项名称
                                    fieldName = Encoding.UTF8.GetString(br_dbf.ReadBytes(11)).Trim('\0');
                                    MyMapObjects.moValueTypeConstant sValueType = MyMapObjects.moValueTypeConstant.dInt16;
                                    //第11位，记录项的数据类型
                                    switch (ch = br_dbf.ReadChar())
                                    {
                                        case 'I':
                                            sValueType = MyMapObjects.moValueTypeConstant.dInt16;
                                            break;
                                        case 'L':
                                            sValueType = MyMapObjects.moValueTypeConstant.dInt32;
                                            break;
                                        case 'N':
                                        case 'F':
                                        case 'B':
                                            sValueType = MyMapObjects.moValueTypeConstant.dDouble;
                                            break;
                                        default:
                                            sValueType = MyMapObjects.moValueTypeConstant.dText;
                                            break;
                                    }

                                    //12-15,保留字节,无用跳过
                                    br_dbf.BaseStream.Seek(4, SeekOrigin.Current);
                                    //第16位，字段长度
                                    fieldLength[i] = br_dbf.ReadByte();

                                    MyMapObjects.moField sField = new MyMapObjects.moField(fieldName, sValueType);
                                    sFields.Append(sField);
                                }

                                //读取实体信息
                                br_dbf.BaseStream.Seek(firstRow, SeekOrigin.Begin);
                                MyMapObjects.moFeatures sFeatures = new MyMapObjects.moFeatures();
                                for (int i = 0; i < rowCount; i++) 
                                {
                                    //跳至第一条记录内容
                                    br.BaseStream.Seek(feaBias[i], SeekOrigin.Begin);
                                    MyMapObjects.moGeometry sGeometry = LoadGeometry(sGeometryType, br);
                                    MyMapObjects.moAttributes sAttributes = new MyMapObjects.moAttributes();

                                    string temp;
                                    br_dbf.BaseStream.Seek(1, SeekOrigin.Current);

                                    //依次读入每一条记录
                                    for (int j = 0; j < attributeCount; j++) 
                                    {
                                        temp = Encoding.GetEncoding("GBK").GetString(br_dbf.ReadBytes(fieldLength[j])).Trim((char)0x20);
                                        //根据ValueType加入属性表
                                        if(sFields.GetItem(j).ValueType == MyMapObjects.moValueTypeConstant.dInt32)
                                        {
                                            if(temp.Equals(""))
                                            {
                                                sAttributes.Append(0);
                                            }
                                            else
                                            {
                                                sAttributes.Append(int.Parse(temp));
                                            }
                                        }
                                        if(sFields.GetItem(j).ValueType == MyMapObjects.moValueTypeConstant.dDouble)
                                        {
                                            double val;
                                            if(temp.Equals("")||(!double.TryParse(temp,out val)))
                                            {
                                                sAttributes.Append(0);
                                            }
                                            else
                                            {
                                                sAttributes.Append(double.Parse(temp));
                                            }
                                        }
                                        if(sFields.GetItem(j).ValueType == MyMapObjects.moValueTypeConstant.dText)
                                        {
                                            sAttributes.Append(temp);
                                        }
                                    }
                                    //生成要素
                                    MyMapObjects.moFeature sFeature = new MyMapObjects.moFeature(sGeometryType, sGeometry, sAttributes);
                                    sFeatures.Add(sFeature);
                                }
                                //生成图层
                                MyMapObjects.moMapLayer sMapLayer = new MyMapObjects.moMapLayer(shpname, sGeometryType, sFields);
                                sMapLayer.Features = sFeatures;
                                return sMapLayer;
                            }
                        }
                    }    
                }
            }
            catch(Exception)
            {
                return null;
            }
        }

        #endregion

        #region 私有函数

        /// <summary>
        /// 读取要素
        /// </summary>
        /// <param name="sGeometryType"></param>
        /// <param name="sr"></param>
        /// <returns></returns>
        private static MyMapObjects.moGeometry LoadGeometry(MyMapObjects.moGeometryTypeConstant sGeometryType, BinaryReader sr)
        {
            if(sGeometryType == MyMapObjects.moGeometryTypeConstant.Point)
            {
                MyMapObjects.moPoint sPoint = LoadPoint(sr);
                return sPoint;
            }
            else if(sGeometryType == MyMapObjects.moGeometryTypeConstant.MultiPolyline)
            {
                MyMapObjects.moMultiPolyline sMultiPolyline = LoadMultiPolyline(sr);
                return sMultiPolyline;
            }    
            else if(sGeometryType == MyMapObjects.moGeometryTypeConstant.MultiPolygon)
            {
                MyMapObjects.moMultiPolygon sMultiPolygon = LoadMultiPolygon(sr);
                return sMultiPolygon;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 读取复合多边形
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        private static moMultiPolygon LoadMultiPolygon(BinaryReader sr)
        {
            MyMapObjects.moMultiPolygon sMultiPolygon = new MyMapObjects.moMultiPolygon();
            //8个字节头和int型4位shapeType
            sr.BaseStream.Seek(12, SeekOrigin.Current);
            //读取外包矩形
            double[] envelope = new double[4];
            envelope[0] = sr.ReadDouble();
            envelope[1] = sr.ReadDouble();
            envelope[2] = sr.ReadDouble();
            envelope[3] = sr.ReadDouble();
            //子环个数
            int partnum = sr.ReadInt32();
            //点总数
            int pointnum = sr.ReadInt32();

            //每个子环在点数组中的起始位置
            int[] parts = new int[partnum + 1];
            for (int i = 0; i < partnum; i++)
                parts[i] = sr.ReadInt32();
            parts[partnum] = pointnum;

            double x, y;

            //依次读入每一个子环
            for(int i=0;i<partnum;i++)
            {
                MyMapObjects.moPoints sPoints = new MyMapObjects.moPoints();
                //每一个子环的点数是parts[i+1]-parts[i]
                for(int j = 0; j < parts[i+1] - parts[i];j++)
                {
                    //依次读入x,y坐标
                    x = sr.ReadDouble();
                    y = sr.ReadDouble();
                    MyMapObjects.moPoint sPoint = new MyMapObjects.moPoint(x, y);
                    //将点加入该子环
                    sPoints.Add(sPoint);
                }
                //将子环加入该复合多边形
                sMultiPolygon.Parts.Add(sPoints);
            }
            sMultiPolygon.UpdateExtent();
            return sMultiPolygon;
        }

        /// <summary>
        /// 读取复合折线
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        private static moMultiPolyline LoadMultiPolyline(BinaryReader sr)
        {
            MyMapObjects.moMultiPolyline sMultiPolyline = new MyMapObjects.moMultiPolyline();
            //8个字节头和int型4位shapeType
            sr.BaseStream.Seek(12, SeekOrigin.Current);
            //读取外包矩形
            double[] envelope = new double[4];
            envelope[0] = sr.ReadDouble();
            envelope[1] = sr.ReadDouble();
            envelope[2] = sr.ReadDouble();
            envelope[3] = sr.ReadDouble();
            //子线段数量
            int partnum = sr.ReadInt32();
            //总点数
            int pointnum = sr.ReadInt32();
            int[] parts = new int[partnum + 1];

            double x, y;
            //依次读入每个子线段在所有点中的起始位置
            for (int i = 0; i < partnum; i++)
                parts[i] = sr.ReadInt32();

            //最后一位存储点数
            parts[partnum] = pointnum;

            //依次读入每一条线段
            for(int i=0; i < partnum; i++)
            {
                MyMapObjects.moPoints sPoints = new MyMapObjects.moPoints();
                //每一条线段的点数是parts[i+1]-parts[i]
                for(int j = 0; j < parts[i + 1] - parts[i];j++)
                {
                    //依次读取x，y坐标
                    x = sr.ReadDouble();
                    y = sr.ReadDouble();
                    MyMapObjects.moPoint sPoint = new MyMapObjects.moPoint(x, y);
                    //将点加入该折线
                    sPoints.Add(sPoint);
                }
                //将折线加入该复合折线
                sMultiPolyline.Parts.Add(sPoints);
            }
            sMultiPolyline.UpdateExtent();
            return sMultiPolyline;
        }

        /// <summary>
        /// 读取点要素
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        private static MyMapObjects.moPoint LoadPoint(BinaryReader sr)
        {
            //8个字节头和int型4位shapeType
            sr.BaseStream.Seek(12, SeekOrigin.Current);
            //读取x，y坐标
            double x, y;
            x = sr.ReadDouble();
            y = sr.ReadDouble();
            //生成点
            MyMapObjects.moPoint sPoint = new MyMapObjects.moPoint(x, y);
            return sPoint;
        }

        #endregion
    }
}
