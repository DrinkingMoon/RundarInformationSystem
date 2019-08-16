/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ObjectPropertyCompare.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2014/05/21
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 用于解决DataGridView绑定List后不能点击列头排序的问题
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace GlobalObject
{
    /// <summary>
    /// 实现对象属性比较的类，List排序可用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPropertyCompare<T> : IComparer<T>
    {
        private PropertyDescriptor property;
        private ListSortDirection direction;

        // 构造函数
        public ObjectPropertyCompare(PropertyDescriptor property, ListSortDirection direction)
        {
            this.property = property;
            this.direction = direction;
        }

        // 实现IComparer中方法
        public int Compare(T x, T y)
        {
            object xValue = x.GetType().GetProperty(property.Name).GetValue(x, null);
            object yValue = y.GetType().GetProperty(property.Name).GetValue(y, null);

            int returnValue;

            if (xValue == null || yValue == null) return 1;

            if (xValue is IComparable)
            {
                returnValue = ((IComparable)xValue).CompareTo(yValue);
            }
            else if (xValue.Equals(yValue))
            {
                returnValue = 0;
            }
            else
            {
                returnValue = xValue.ToString().CompareTo(yValue.ToString());
            }

            if (direction == ListSortDirection.Ascending)
            {
                return returnValue;
            }
            else
            {
                return returnValue * -1;
            }
        }
    }
}
