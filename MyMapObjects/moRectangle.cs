using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMapObjects
{
    /// <summary>
    /// 矩形 Rectangle
    /// </summary>
    public class moRectangle: moShape
    {
        #region 字段 items

        private double _MinX, _MaxX, _MinY, _MaxY;

        #endregion


        #region 构造函数 Constructor

        public moRectangle( double minX, double maxX, double minY, double maxY)
        {
            _MinX = minX;
            _MaxX = maxX;
            _MinY = minY;
            _MaxY = maxY;
        }

        #endregion


        #region 属性 attribute
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
        /// <summary>
        /// 获取宽度 get width
        /// </summary>
        public double Width
        {
            get { return _MaxX - _MinX; }
        }
        /// <summary>
        /// 获取高度 get height
        /// </summary>
        public double Height
        {
            get { return _MaxY - _MinY; }
        }
        /// <summary>
        /// 判断是否为空矩形 Is empty or not
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                if (_MaxX <= _MinX)
                    return true;
                else if (_MaxY <= _MinY)
                    return true;
                else
                    return false;
            }
        }

        #endregion


        #region 成员函数
        /// <summary>
        /// 克隆一个对象 clone
        /// </summary>
        /// <returns></returns> 
        public moRectangle Clone()
        {
            moRectangle sRectangle = new moRectangle(_MinX, _MaxX, _MinY, _MaxY); // 这里new的对象不会销毁，不是局部变量，应该是开在堆区的变量？
            return sRectangle; //返回这个内部声明的变量也是可以的
        }

        #endregion
    }
}
