using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 需要包含条件用户控件的主窗体所需实现的条件接口
    /// </summary>
    /// <remarks>查询条件窗体和过滤条件窗体需要实现</remarks>
    public interface IConditionForm
    {
        /// <summary>
        /// 删除条件
        /// </summary>
        /// <param name="control">要删除的控件</param>
        void DeleteCondition(UserControlFindCondition control);

        //// <summary>
        //// 获取此条件窗体涉及的业务名称
        //// </summary>
        //string Business
        //{
        //    get;
        //}
    }

    /// <summary>
    /// 过滤信息
    /// </summary>
    public class FilterInfo : IComparable
    {
        private int _orderNo;

        public int OrderNo
        {
            get { return _orderNo; }
            set { _orderNo = value; }
        }
        private string _leftParentheses;

        public string LeftParentheses
        {
            get { return _leftParentheses; }
            set { _leftParentheses = value; }
        }
        private string _rghtParentheses;

        public string RghtParentheses
        {
            get { return _rghtParentheses; }
            set { _rghtParentheses = value; }
        }
        private string _fieldName;

        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }
        private string _operator;

        public string _operator1
        {
            get { return _operator; }
            set { _operator = value; }
        }
        private string _dataType;

        public string DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }
        private string _dataValue;

        public string DataValue
        {
            get { return _dataValue; }
            set { _dataValue = value; }
        }
        private string _logic;

        public string Logic
        {
            get { return _logic; }
            set { _logic = value; }
        }

        #region IComparable 成员

        public int CompareTo(object obj)
        {
            if (obj is FilterInfo)
            {
                FilterInfo another = obj as FilterInfo;
                return this._orderNo.CompareTo(another._orderNo);
            }
            throw new ArgumentException("object is not a FilterInfo");
        }

        #endregion
    }
}
