using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMapObjects
{
    /// <summary>
    /// 标签渲染工具的封装
    /// </summary>
    public class moLabelRenderer
    {
        #region 字段

        private bool _LabelFeatures = false;    //是否显示注记
        private moTextSymbol _TextSymbol = new moTextSymbol(); //文本符号
        private string _Field = "";    //绑定字段的名称

        #endregion


        #region 属性
        /// <summary>
        /// 获取或设置是否显示注记
        /// </summary>
        public bool LabelFeatures
        {
            get { return _LabelFeatures; }
            set { _LabelFeatures = value; }
        }
        /// <summary>
        /// 获取或设置文本符号
        /// </summary>
        public moTextSymbol TextSymbol
        {
            get { return _TextSymbol; }
            set { _TextSymbol = value; }
        }
        /// <summary>
        /// 获取或设置绑定字段的名称
        /// </summary>
        public string Field
        {
            get { return _Field; }
            set { _Field = value; }
        }

        #endregion
    }
}
