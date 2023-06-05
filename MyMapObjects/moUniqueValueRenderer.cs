﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMapObjects
{
    /// <summary>
    /// 唯一值渲染
    /// </summary>
    public class moUniqueValueRenderer : moRenderer
    {
        #region 字段

        private string _Field = ""; // 绑定字段的名称
        private string _HeadTitle = ""; // 图层显示控件 中的标题
        private bool _ShowHead = true; // 在图层显示控件中。是否显示标题

        private List<string> _Values = new List<string>(); // 唯一值集合，采用字符串
        private List<moSymbol> _Symbols = new List<moSymbol>(); // 符号集合

        private moSymbol _DefaultSymbol; // 默认符号
        private bool _ShowDefaultSymbol = true; // 在图层显示控件中是否显示默认符号

        #endregion


        #region 构造函数

        public moUniqueValueRenderer() { }

        #endregion


        #region 属性
        /// <summary>
        /// 获取渲染的类型
        /// </summary>
        public override moRendererTypeConstant RendererType
        {
            get { return moRendererTypeConstant.UniqueValue; }
        }
        /// <summary>
        /// 获取或设置 字段 、图层显示控件的标题 的名称
        /// </summary>
        public string Field
        {
            get { return _Field; }
            set 
            {
                _Field = value;
                _HeadTitle = value;
            }
        }
        /// <summary>
        /// 获取唯一值的数目
        /// </summary>
        public Int32 ValueCount
        {
            get { return _Values.Count; }
        }
        /// <summary>
        /// 获取或设置默认符号
        /// </summary>
        public moSymbol DefaultSymbol
        {
            get { return _DefaultSymbol; }
            set { _DefaultSymbol = value; }
        }
        /// <summary>
        /// 获取或设置是否显示字段标题的bool
        /// </summary>
        public bool ShowHead
        {
            get { return _ShowHead; }
            set { _ShowHead = value; }
        }
        /// <summary>
        /// 获取或设置是否显示默认符号的bool
        /// </summary>
        public bool ShowDefaultSymbol
        {
            get { return _ShowDefaultSymbol; }
            set { _ShowDefaultSymbol = value; }

        }

        #endregion


        #region 方法
        /// <summary>
        ///获取指定索引号的value
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetValue(Int32 index)
        {
            return _Values[index];
        }
        /// <summary>
        /// 设置指定索引号处的value
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetValue(Int32 index, string value)
        {
            _Values[index] = value;
        }
        /// <summary>
        /// 获取指定索引号处的symbol(符号)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public moSymbol GetSymbol(Int32 index)
        {
            return _Symbols[index];
        }
        /// <summary>
        /// 设置指定索引号处的symbol(符号)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="symbol"></param>
        public void SetSymbol(Int32 index, moSymbol symbol)
        {
            _Symbols[index] = symbol;
        }
        /// <summary>
        /// 添加唯一值和对应的符号
        /// </summary>
        /// <param name="value"></param>
        /// <param name="symbol"></param>
        public void AddUniqueValue(string value, moSymbol symbol)
        {
            _Values.Add(value);
            _Symbols.Add(symbol);
        }
        /// <summary>
        /// 增加唯一值数组和对应的符号数组，这里注意两个数组长度要一致
        /// </summary>
        /// <param name="values"></param>
        /// <param name="symbols"></param>
        public void AddUniqueValues(string[] values, moSymbol[] symbols)
        {
            if(values.Length != symbols.Length)
            {
                throw new Exception("the length of the two arrays is not equal!");
            }

            _Values.AddRange(values);
            _Symbols.AddRange(symbols);
        }
        /// <summary>
        /// 根据指定值获取相应的符号，如果该值不存在则返回默认符号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public moSymbol FindSymbol(string value)
        {
            Int32 sValueCount = _Values.Count;
            for(Int32 i=0; i < sValueCount; i++)
            {
                if(_Values[i] == value)
                {
                    return _Symbols[i];
                }
            }
            return _DefaultSymbol;
        }
        /// <summary>
        /// 克隆一个唯一值绘制对象
        /// </summary>
        /// <returns></returns>
        public override moRenderer Clone()
        {
            moUniqueValueRenderer sRenderer = new moUniqueValueRenderer();
            sRenderer._Field = _Field;
            sRenderer._HeadTitle = _HeadTitle;
            sRenderer._ShowHead = _ShowHead;
            Int32 sValueCount = _Values.Count;
            for (Int32 i = 0; i <= sValueCount - 1; i++)
            {
                string sValue = _Values[i];
                moSymbol sSymbol = null;
                if (_Symbols[i] != null)
                    sSymbol = _Symbols[i].Clone();
                sRenderer.AddUniqueValue(sValue, sSymbol);
            }
            if (_DefaultSymbol != null)
                sRenderer.DefaultSymbol = _DefaultSymbol.Clone();
            sRenderer._ShowDefaultSymbol = _ShowDefaultSymbol;
            return sRenderer;
        }

        #endregion
    }
}
