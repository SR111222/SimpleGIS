using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMapObjects
{
    /// <summary>
    /// 多多边形，本质上是封装了图形特性的parts(点集合的集合)
    /// </summary>
    public class moMultiPolygon : moGeometry
    {
        #region 字段

        private moParts _Parts;
        double _MinX = double.MaxValue, _MaxX = double.MinValue;
        double _MinY = double.MaxValue, _MaxY = double.MinValue;

        #endregion

        #region 构造函数

        public moMultiPolygon()
        {
            _Parts = new moParts();
        }
        /// <summary>
        /// 单个点集合
        /// </summary>
        /// <param name="points"></param>
        public moMultiPolygon(moPoints points)
        {
            _Parts = new moParts();
            _Parts.Add(points);
        }
        public moMultiPolygon(moParts parts)
        {
            _Parts = parts;
        }
        #endregion


        #region 属性
        public moParts Parts
        {
            get { return _Parts; }
            set { _Parts = value; }
        }
        /// <summary>
        /// 获取最小X坐标 get Max X
        /// </summary>
        public double MinX
        {
            get { return _MinX; }
        }
        /// <summary>
        /// 获取最大X坐标
        /// </summary>
        public double MaxX
        {
            get { return _MaxX; }
        }
        /// <summary>
        /// 获取最小Y坐标
        /// </summary>
        public double MinY
        {
            get { return _MinY; }
        }
        /// <summary>
        /// 获取最大Y坐标
        /// </summary>
        public double MaxY
        {
            get { return _MaxY; }
        }
        #endregion


        #region 方法
        /// <summary>
        /// 获取外包矩形
        /// </summary>
        /// <returns></returns>
        public moRectangle GetEnvelope()
        {
            moRectangle sRectangle = new moRectangle(_MinX, _MaxX, _MinY, _MaxY);
            return sRectangle;
        }
        /// <summary>
        /// 重新计算/刷新范围（最大最小的XY）
        /// </summary>
        public void UpdateExtent()
        {
            CalExtent();
        }
        /// <summary>
        /// 克隆一个对象
        /// </summary>
        /// <returns></returns>
        public moMultiPolygon Clone()
        {
            moMultiPolygon sMultiPolygon = new moMultiPolygon();
            sMultiPolygon.Parts = _Parts.Clone();
            sMultiPolygon._MinX = _MinX;
            sMultiPolygon._MaxX = _MaxX;
            sMultiPolygon._MinY = _MinY;
            sMultiPolygon._MaxY = _MaxY;
            return sMultiPolygon;
        }

        #endregion


        #region 私有函数
        //计算范围
        private void CalExtent()
        {
            double sMinX = double.MaxValue, sMaxX = double.MinValue;
            double sMinY = double.MaxValue, sMaxY = double.MinValue;
            Int32 sPartCount = _Parts.Count;
            for (Int32 i = 0; i < sPartCount; i++)
            {
                _Parts.GetItem(i).UpdateExtent();
                if (_Parts.GetItem(i).MinX < sMinX)
                    sMinX = _Parts.GetItem(i).MinX;
                if (_Parts.GetItem(i).MaxX > sMaxX)
                    sMaxX = _Parts.GetItem(i).MaxX;
                if (_Parts.GetItem(i).MinY < sMinY)
                    sMinY = _Parts.GetItem(i).MinY;
                if (_Parts.GetItem(i).MaxY > sMaxY)
                    sMaxY = _Parts.GetItem(i).MaxY;
            }
            _MinX = sMinX;
            _MaxX = sMaxX;
            _MinY = sMinY;
            _MaxY = sMaxY;
        }
        #endregion
    }
}
