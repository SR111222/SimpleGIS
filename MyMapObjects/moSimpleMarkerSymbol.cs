using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing; // 绘图
using System.Security.Cryptography; // 安全相关，这里用来生成质量比较高的随机数


namespace MyMapObjects
{
    /// <summary>
    /// 简单点符号的封装
    /// </summary>
    public class moSimpleMarkerSymbol : moSymbol
    {
        #region 字段

        private string _Label = ""; // 符号标签
        private bool _Visible = true; // 该符号是否显示
        private moSimpleMarkerSymbolStyleConstant _Style = moSimpleMarkerSymbolStyleConstant.SolidCircle;
        private Color _Color = Color.LightPink; // 符号的颜色，尽量偏浅
        /// <summary>
        /// 尺寸，单位：*毫米*
        /// </summary>
        private double _Size = 3;

        #endregion


        #region 构造函数
        /// <summary>
        /// 创造一个随机的偏浅色的符号
        /// </summary>
        public moSimpleMarkerSymbol()
        {
            //生成随机颜色
            CreateRandomColor();
        }

        public moSimpleMarkerSymbol(string label)
        {
            _Label = label;
            //生成随机颜色
            CreateRandomColor();
        }

        #endregion


        #region 属性
        /// <summary>
        /// 获取符号类型（点、线、面）
        /// </summary>
        public override moSymbolTypeConstant SymbolType
        {
            get
            {
                return moSymbolTypeConstant.SimpleMarkerSymbol;
            }
        }

        public string Label
        {
            get { return _Label; }
            set { _Label = value; }
        }
        public bool Visible
        {
            get { return _Visible; }
            set { _Visible = value; }
        }

        public moSimpleMarkerSymbolStyleConstant Style
        {
            get { return _Style; }
            set { _Style = value; }
        }

        public Color Color
        {
            get { return _Color; }
            set { _Color = value; }
        }

        public double Size
        {
            get { return _Size; }
            set { _Size = value; }
        }

        #endregion


        #region 方法
        /// <summary>
        /// 克隆一个点符号元素
        /// </summary>
        /// <returns></returns>
        public override moSymbol Clone()
        {
            moSimpleMarkerSymbol sSymbol = new moSimpleMarkerSymbol();
            sSymbol._Label = _Label;
            sSymbol._Visible = _Visible;
            sSymbol._Style = _Style;
            sSymbol._Color = _Color;
            sSymbol._Size = _Size;
            return sSymbol;
        }

        #endregion


        #region 私有函数
        //为本符号生成随机的颜色
        private void CreateRandomColor()
        {
            //总体思想：每个随机颜色RGB中总有一个为252，其他两个值的取值范围为179-245，这样取值的目的在于让地图颜色偏浅，美观
            //生成4个元素的字节数组，第一个值决定哪个通道取252，另外三个中的两个值决定另外两个通道的值
            byte[] sBytes = new byte[4];
            RNGCryptoServiceProvider sChanelRng = new RNGCryptoServiceProvider();
            sChanelRng.GetBytes(sBytes); // 获得随机的四个数
            Int32 sChanelValue = sBytes[0];
            byte A = 255, R, G, B;
            if (sChanelValue <= 85)
            {
                R = 252;
                G = (byte)(179 + 66 * sBytes[2] / 255);
                B = (byte)(179 + 66 * sBytes[3] / 255);
            }
            else if (sChanelValue <= 170)
            {
                G = 252;
                R = (byte)(179 + 66 * sBytes[1] / 255);
                B = (byte)(179 + 66 * sBytes[3] / 255);
            }
            else
            {
                B = 252;
                R = (byte)(179 + 66 * sBytes[1] / 255);
                G = (byte)(179 + 66 * sBytes[2] / 255);
            }
            _Color = Color.FromArgb(A, R, G, B);
        }

        #endregion
    }
}
