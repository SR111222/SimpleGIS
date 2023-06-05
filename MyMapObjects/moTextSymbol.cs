using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace MyMapObjects
{
    /// <summary>
    /// 文本字体符号的封装
    /// </summary>
    public class moTextSymbol
    {
        #region 字段
        
        private Font _Font = new Font("微软雅黑", 8);   //字体
        private Color _FontColor = Color.Black;        //颜色
        private moTextSymbolAlignmentConstant _Alignment = moTextSymbolAlignmentConstant.CenterCenter;  //布局
        private double _OffsetX, _OffsetY;   //X,Y方向偏移量，单位毫米，向右为正，向上为正（反方向为负）
        private bool _UseMask = false;  //是否描边
        private double _MaskWidth = 0.5;    //描边宽度，单位毫米
        private Color _MaskColor = Color.White; //描边的字体

        #endregion


        #region 属性
        /// <summary>
        /// 获取或设置字体
        /// </summary>
        public Font Font
        {
            get { return _Font; }
            set { _Font = value; }
        }
        /// <summary>
        /// 获取或设置字体颜色
        /// </summary>
        public Color FontColor
        {
            get { return _FontColor; }
            set { _FontColor = value; }
        }
        /// <summary>
        /// 获取或设置布局
        /// </summary>
        public moTextSymbolAlignmentConstant Alignment
        {
            get { return _Alignment; }
            set { _Alignment = value; }
        }
        /// <summary>
        /// 获取或设置X偏移量，向右为正，向左为负
        /// </summary>
        public double OffsetX
        {
            get { return _OffsetX; }
            set { _OffsetX = value; }
        }
        /// <summary>
        /// 获取或设置Y偏移量，向上为正，向下为负
        /// </summary>
        public double OffsetY
        {
            get { return _OffsetY; }
            set { _OffsetY = value; }
        }
        /// <summary>
        /// 获取或设置是否描边
        /// </summary>
        public bool UseMask
        {
            get { return _UseMask; }
            set { _UseMask = value; }
        }
        /// <summary>
        /// 获取或设置描边的宽度，单位毫米
        /// </summary>
        public double MaskWidth
        {
            get { return _MaskWidth; }
            set { _MaskWidth = value; }
        }
        /// <summary>
        /// 获取或设置描边颜色
        /// </summary>
        public Color MaskColor
        {
            get { return _MaskColor; }
            set { _MaskColor = value; }
        }

        //旋转、横向拉伸就不再做了
        #endregion


        #region 方法
        /// <summary>
        /// 克隆一个文本字体对象
        /// </summary>
        /// <returns></returns>
        public moTextSymbol Clone()
        {
            moTextSymbol sTextSymbol = new moTextSymbol();
            sTextSymbol._Font = (Font)_Font.Clone(); // ***注意强制转换***
            sTextSymbol._FontColor = _FontColor;
            sTextSymbol._Alignment = _Alignment;
            sTextSymbol.OffsetX = _OffsetX;
            sTextSymbol.OffsetY = _OffsetY;
            sTextSymbol._UseMask = _UseMask;
            sTextSymbol.MaskColor = _MaskColor;
            sTextSymbol.MaskWidth = _MaskWidth;
            return sTextSymbol;
        }

        #endregion
    }
}
