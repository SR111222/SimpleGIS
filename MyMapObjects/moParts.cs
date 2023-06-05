using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMapObjects
{
    /// <summary>
    /// 单纯的 点的集合的集合 的数据结构，没有任何图形特性，就是单纯的集合,part:点集合，parts:点集合的集合
    /// </summary>
    public class moParts
    {
        #region 字段

        private List<moPoints> _Parts;

        #endregion

        #region 构造函数

        public moParts()
        {
            _Parts = new List<moPoints>();
        }
        public moParts(moPoints[] parts)
        {
            _Parts = new List<moPoints>();
            _Parts.AddRange(parts);
        }

        #endregion


        #region 属性
        /// <summary>
        /// 获取元素数目
        /// </summary>
        public Int32 Count
        {
            get { return _Parts.Count(); }
        }
        #endregion


        #region 方法

        /// <summary>
        /// 获取指定索引处的元素
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public moPoints GetItem(Int32 index)
        {
            return _Parts[index];
        }
        /// <summary>
        /// 设置指定索引处的元素
        /// </summary>
        /// <param name="index"></param>
        /// <param name="part"></param>
        public void SetItem(Int32 index, moPoints part)
        {
            _Parts[index] = part;
            
        }
        public void Add(moPoints part)
        {
            _Parts.Add(part);
        }
        public void AddRange(moPoints[] parts)
        {
            _Parts.AddRange(parts);
        }
        /// <summary>
        /// 将指定元素插入到指定索引处
        /// </summary>
        /// <param name="index"></param>
        /// <param name="part"></param>
        public void Insert(Int32 index, moPoints part)
        {
            _Parts.Insert(index, part);
        }
        /// <summary>
        /// 将指定数组中的元素插入到指定索引处
        /// </summary>
        /// <param name="index"></param>
        /// <param name="points"></param>
        public void InsertRange(Int32 index, moPoints[] parts)
        {
            _Parts.InsertRange(index, parts);
        }
        /// <summary>
        /// 删除指定索引号的part(points)
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(Int32 index)
        {
            _Parts.RemoveAt(index);
        }
        /// <summary>
        /// 清空所有元素
        /// </summary>
        public void Clear()
        {
            _Parts.Clear();
        }
        /// <summary>
        /// 将所有元素复制到一个数组里
        /// </summary>
        /// <returns></returns>
        public moPoints[] ToArray()
        {
            return _Parts.ToArray();
        }

        ///// <summary>
        ///// 获取外包矩形
        ///// </summary>
        ///// <returns></returns>
        //public moRectangle GetEnvelope()
        //{
        //    moRectangle sRectangle = new moRectangle(_MinX, _MaxX, _MinY, _MaxY);
        //    return sRectangle;
        //}

        /// <summary>
        /// 克隆一个对象
        /// </summary>
        /// <returns></returns>
        public moParts Clone()
        {
            moParts sParts = new moParts();
            Int32 sPartCount = _Parts.Count;
            for (Int32 i = 0; i < sPartCount; i++)
            {
                moPoints sPart = _Parts[i].Clone();
                sParts.Add(sPart);
            }
            return sParts;
        }

        #endregion

    }
}
