using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMapObjects
{
    /// <summary>
    /// 对 要素(feature) 的封装
    /// </summary>
    public class moFeature
    {
        #region 字段

        private moGeometryTypeConstant _ShapeType = moGeometryTypeConstant.MultiPolygon; // 几何类型
        /// <summary>
        /// 几何图形,注意这里是抽象类，使用时需显示转换为非抽象类
        /// </summary>
        private moGeometry _Geometry; // 几何图形,注意这里是抽象类，使用时需显示转换为非抽象类
        private moAttributes _Attributes; // 属性集合
        private moSymbol _Symbol;   //符号

        #endregion


        #region 构造函数

        public moFeature(moGeometryTypeConstant shapeType, moGeometry geometry, moAttributes attributes)
        {
            _ShapeType = shapeType;
            _Geometry = geometry;
            _Attributes = attributes;
        }

        #endregion


        #region 属性
        /// <summary>
        /// 获取或设置几何类型（set要不要？）
        /// </summary>
        public moGeometryTypeConstant ShapeType
        {
            get { return _ShapeType; }
            set { _ShapeType = value; }
        }
        /// <summary>
        /// 获取或设置几何图形
        /// </summary>
        public moGeometry Geometry
        {
            get { return _Geometry; }
            set { _Geometry = value; }
        }
        /// <summary>
        /// 获取或设置属性值集合
        /// </summary>
        public moAttributes Attributes
        {
            get { return _Attributes; }
            set { _Attributes = value; }
        }
        /// <summary>
        /// 获取或设置符号，internal，只供程序集访问
        /// </summary>
        internal moSymbol Symbol
        {
            get { return _Symbol; }
            set { _Symbol = value; }
        }

        #endregion


        #region 方法
        /// <summary>
        /// 获取要素的最小外包矩形，如果是点则返回长度为0的矩形
        /// </summary>
        /// <returns></returns>
        public moRectangle GetEnvelope()
        {
            moRectangle sRect = null;
            
            if(_ShapeType == moGeometryTypeConstant.Point)
            {
                moPoint sPoint = (moPoint)_Geometry; //？？？？？
                sRect = new moRectangle(sPoint.X, sPoint.X, sPoint.Y, sPoint.Y);
            }
            else if(_ShapeType == moGeometryTypeConstant.MultiPolyline)
            {
                moMultiPolyline sMultiPolyline = (moMultiPolyline)_Geometry;
                sRect = sMultiPolyline.GetEnvelope();
            }
            else
            {
                moMultiPolygon sMultiPolygon = (moMultiPolygon)_Geometry;
                sRect = sMultiPolygon.GetEnvelope();
            }

            return sRect;
        }
        /// <summary>
        /// 克隆一个要素对象
        /// </summary>
        /// <returns></returns>
        public moFeature Clone()
        {
            moGeometryTypeConstant sShapeType = _ShapeType;
            moGeometry sGeometry = null;
            moAttributes sAttributes = _Attributes.Clone();
            if (_ShapeType == moGeometryTypeConstant.Point)
            {
                moPoint sPoint = (moPoint)_Geometry;
                sGeometry = sPoint.Clone();
            }
            else if (_ShapeType == moGeometryTypeConstant.MultiPolyline)
            {
                moMultiPolyline sMultiPolyline = (moMultiPolyline)_Geometry;
                sGeometry = sMultiPolyline.Clone();
            }
            else if (_ShapeType == moGeometryTypeConstant.MultiPolygon)
            {
                moMultiPolygon sMultiPolygon = (moMultiPolygon)_Geometry;
                sGeometry = sMultiPolygon.Clone();
            }
            moFeature sFeature = new moFeature(sShapeType, sGeometry, sAttributes);
            return sFeature;
        }

        #endregion

    }
}
