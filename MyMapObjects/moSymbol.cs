using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMapObjects
{
    /// <summary>
    /// 符号类（抽象类）
    /// </summary>
    public abstract class moSymbol
    {
        /// <summary>
        /// 抽象属性，获取符号类型，只有get
        /// </summary>
        public abstract moSymbolTypeConstant SymbolType { get; } // 注意语法

        /// <summary>
        /// 抽象方法，克隆符号
        /// </summary>
        /// <returns></returns>
        public abstract moSymbol Clone();
    }
}
