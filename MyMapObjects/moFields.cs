using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMapObjects
{
    public class moFields
    {
        #region 字段

        private List<moField> _Fields;  //字段集合
        private string _PrimaryField = "";  //主字段
        private bool _ShowAlias = false;    //是否显示别名

        #endregion

        #region 构造函数

        public moFields()
        {
            _Fields = new List<moField>();

        }

        #endregion


        #region 属性
        /// <summary>
        /// 获取元素数目
        /// </summary>
        public Int32 Count
        {
            get { return _Fields.Count; }
        }
        /// <summary>
        /// 获取或设置主字段
        /// </summary>
        public string PrimaryField
        {
            get { return _PrimaryField; }
            set { _PrimaryField = value; }
        }
        /// <summary>
        /// 获取或设置是否显示别名
        /// </summary>
        public bool ShowAlias
        {
            get { return _ShowAlias; }
            set { _ShowAlias = value; }
        }
        #endregion


        #region 方法
        /// <summary>
        /// 获取指定索引号的元素（字段）
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public moField GetItem(Int32 index)
        {
            return _Fields[index];
        }
        /// <summary>
        /// 获取指定名称的字段
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public moField GetItem(string name)
        {
            Int32 sIndex = FindField(name);
            if(sIndex >= 0)
            {
                return _Fields[sIndex];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 追加一个字段，不能重复，若重复则throw异常
        /// </summary>
        /// <param name="field"></param>
        public void Append(moField field)
        {
            if(FindField(field.Name) >= 0)
            {
                throw new Exception("Fields对象不能存在重名的字段！");//直接中断
            }

            _Fields.Add(field);
            //有字段添加后，需要触发事件
            if(FieldAppended != null)
            {
                FieldAppended(this, field);
            }
        }
        /// <summary>
        /// 删除指定索引号的元素（字段）
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(Int32 index)
        {
            moField sField = _Fields[index]; // 看有没有该索引的元素，如果索引不存在则底层报错

            _Fields.RemoveAt(index);

            //删除元素后触发事件
            if(FieldRemoved != null)
            {
                FieldRemoved(this, index, sField);
            }
        }

        /// <summary>
        /// 根据名称查找字段，返回索引号，如果没有返回-1
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Int32 FindField(string name)
        {
            for (Int32 i = 0; i < _Fields.Count; i++)
            {
                if (_Fields[i].Name.ToLower() == name.ToLower()) // 转换为小写后比较，即大小写不敏感
                {
                    return i;
                }
            }
            return -1;
        }

        #endregion


        #region 事件
        /// <summary>
        /// 有字段被加入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="fieldAppended"></param>
        internal delegate void FieldAppendedHandle(object sender, moField fieldAppended);
        internal event FieldAppendedHandle FieldAppended;

        /// <summary>
        /// 有字段被删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="fieldIndex"></param>
        /// <param name="fieldRemoved"></param>
        internal delegate void FieldRemovedHandle(object sender, Int32 fieldIndex, moField fieldRemoved);
        internal event FieldRemovedHandle FieldRemoved;

        #endregion


        #region 私有函数
        
        #endregion
    }
}
