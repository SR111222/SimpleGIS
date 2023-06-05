using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMapObjects
{
    public abstract class moRenderer
    {
        public abstract moRendererTypeConstant RendererType { get; } // 抽象属性
        public abstract moRenderer Clone(); // 抽象方法
    }
}
