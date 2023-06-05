using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMapObjects
{
    public class moPoint : moGeometry
    {
        #region 字段 Items

        private double _X;
        private double _Y;

        #endregion


        #region 构造函数 Constructor

        public moPoint() {
        
        }

        public moPoint(double x, double y)
        {
            _X = x;
            _Y = y;
        }

        #endregion


        #region 属性 attribute

        /// <summary>
        /// 获取或设置X坐标
        /// </summary>
        public double X
        {
            get { return _X; }
            set { _X = value; }
        }
        /// <summary>
        /// 获取或设置Y坐标
        /// </summary>
        public double Y
        {
            get { return _Y; }
            set { _Y = value; }
        }

        #endregion

        #region 成员函数
        /// <summary>
        /// 克隆一个对象 clone
        /// </summary>
        /// <returns></returns>
        public moPoint Clone()
        {
            moPoint sPoint = new moPoint(_X, _Y); // 这里new的对象不会销毁，不是局部变量，应该是开在堆区的变量？
            return sPoint; //返回这个内部声明的变量也是可以的
        }
        #endregion
    }
}
