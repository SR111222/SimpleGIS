using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMapObjects
{
    public class moPoints:moGeometry
    {
        #region 字段 items

        private List<moPoint> _Points;
        private double _MinX = double.MaxValue, _MaxX = double.MinValue;//初始化设置，最大为最小，最小为最大
        private double _MinY = double.MaxValue, _MaxY = double.MinValue;

        #endregion

        #region 构造函数
        public moPoints()
        {
            _Points = new List<moPoint>();
        }
        public moPoints(moPoint[] points)
        {
            _Points = new List<moPoint>();
            _Points.AddRange(points);
        }
        #endregion


        #region 属性
        public Int32 Count
        {
            get { return _Points.Count; }
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
        /// 查找指定索引处的元素
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public moPoint GetItem(Int32 index)
        {
            return _Points[index];
        }
        /// <summary>
        /// 设置指定索引处的元素
        /// </summary>
        /// <param name="index"></param>
        /// <param name="part"></param>
        public void SetItem(Int32 index, moPoint point)
        {
            _Points[index] = point;
        }
        /// <summary>
        /// 添加元素到末尾
        /// </summary>
        /// <param name="point"></param>
        public void Add(moPoint point)
        {
            _Points.Add(point);
        }
        /// <summary>
        /// 添加数组中的元素到末尾
        /// </summary>
        /// <param name="points"></param>
        public void AddRange(moPoint[] points)
        {
            _Points.AddRange(points);
        }
        /// <summary>
        /// 将指定元素插入指定索引号处
        /// </summary>
        /// <param name="index"></param>
        /// <param name="point"></param>
        public void Insert(Int32 index, moPoint point)
        {
            _Points.Insert(index, point);
        }
        /// <summary>
        /// 将指定数组中的元素插入到指定索引处
        /// </summary>
        /// <param name="index"></param>
        /// <param name="points"></param>
        public void InsertRange(Int32 index, moPoint[] points)
        {
            _Points.InsertRange(index, points);
        }
        /// <summary>
        /// 删除指定索引号的元素
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(Int32 index)
        {
            _Points.RemoveAt(index);
        }
        /// <summary>
        /// 清空所有元素
        /// </summary>
        public void Clear()
        {
            _Points.Clear();
        }
        /// <summary>
        /// 将所有元素复制到一个数组里
        /// </summary>
        /// <returns></returns>
        public moPoint[] ToArray()
        {
            return _Points.ToArray();
        }
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
        /// 重新计算/更新范围（最大最小的XY值）
        /// </summary>
        public void UpdateExtent()
        {
            CalExtent();
        }
        /// <summary>
        /// 克隆一个对象
        /// </summary>
        /// <returns></returns>
        public moPoints Clone()
        {
            moPoints sPoints = new moPoints();
            Int32 sPointCount = _Points.Count;
            for (Int32 i = 0; i <= sPointCount - 1; i++)
            {
                moPoint sPoint = new moPoint(_Points[i].X, _Points[i].Y);
                sPoints.Add(sPoint);
            }
            sPoints._MinX = _MinX;
            sPoints._MaxX = _MaxX;
            sPoints._MinY = _MinY;
            sPoints._MaxY = _MaxY;
            return sPoints;
        }

        #endregion


        #region 私有函数
        //计算范围
        private void CalExtent()
        {
            double sMinX = double.MaxValue;
            double sMaxX = double.MinValue;
            double sMinY = double.MaxValue;
            double sMaxY = double.MinValue;
            Int32 sPointCount = _Points.Count;
            for (Int32 i = 0; i < sPointCount; i++)
            {
                if (_Points[i].X < sMinX)
                    sMinX = _Points[i].X;
                if (_Points[i].X > sMaxX)
                    sMaxX = _Points[i].X;
                if (_Points[i].Y < sMinY)
                    sMinY = _Points[i].Y;
                if (_Points[i].Y > sMaxY)
                    sMaxY = _Points[i].Y;
            }
            _MinX = sMinX;
            _MaxX = sMaxX;
            _MinY = sMinY;
            _MaxY = sMaxY;
        }
        #endregion
    }
}
