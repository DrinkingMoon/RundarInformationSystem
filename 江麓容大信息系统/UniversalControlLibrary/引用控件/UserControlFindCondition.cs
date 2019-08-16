/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UserControlFindCondition.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/15
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 电子档案界面
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2009/07/03 08:02:08 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 查询条件组件
    /// </summary>
    public partial class UserControlFindCondition : UserControl
    {
        /// <summary>
        /// 顶级控件接口
        /// </summary>
        IConditionForm m_parentForm;

        /// <summary>
        /// 当前查询字段
        /// </summary>
        string m_fieldName = "";

        /// <summary>
        /// SQL查询where语句字段
        /// </summary>
        string m_sentenceSQL = "";

        /// <summary>
        /// 获取或设置顺序编号(用于确定在父窗体中的位置)
        /// </summary>
        public int OrderNo
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置左括号
        /// </summary>
        public string LeftParentheses
        {
            get { return cmbLeft.Text; }

            set
            {
                if (cmbLeft.Items.Contains(value))
                {
                    cmbLeft.Text = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置右括号
        /// </summary>
        public string RightParentheses
        {
            get { return cmbRight.Text; }

            set
            {
                if (cmbRight.Items.Contains(value))
                {
                    cmbRight.Text = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置字段名称
        /// </summary>
        public string FieldName
        {
            get { return cmbFindField.Text; }

            set 
            {
                if (cmbFindField.Items.Contains(value))
                {
                    m_fieldName = value;
                    cmbFindField.Text = value;
                }
            }
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        string m_dataType;

        /// <summary>
        /// 获取或设置数据类型
        /// </summary>
        public string DataType
        {
            get { return m_dataType; }
            set { m_dataType = value; }
        }

        /// <summary>
        /// 数据值
        /// </summary>
        public string DataValue
        {
            get
            {
                if (txtFindData.Visible)
                    return txtFindData.Text;

                else if (cmbFindData.Visible)
                    return cmbFindData.Text;

                else if (numCount.Visible)
                    return numCount.Value.ToString();

                else if (numPrice.Visible)
                    return numPrice.Value.ToString();

                else if (dateTime.Visible)
                    return dateTime.Value.ToShortDateString();

                else
                    throw new Exception("条件中未知的数据值");
            }

            set
            {
                if (txtFindData.Visible)
                    txtFindData.Text = value;

                else if (cmbFindData.Visible)
                    cmbFindData.Text = value;

                else if (numCount.Visible)
                    numCount.Value = Convert.ToDecimal(value);

                else if (numPrice.Visible)
                    numPrice.Value = Convert.ToDecimal(value);

                else if (dateTime.Visible)
                    dateTime.Value = Convert.ToDateTime(value);

                else
                    throw new Exception("条件中未知的数据值");
            }
        }

        /// <summary>
        /// 获取或设置操作符
        /// </summary>
        public string Operator
        {
            get { return cmbOperator.Text; }

            set
            {
                if (cmbOperator.Items.Contains(value))
                {
                    cmbOperator.Text = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置关系符
        /// </summary>
        public string LogicSymbol
        {
            get { return cmbRelationSymbol.Text; }

            set
            {
                if (cmbRelationSymbol.Items.Contains(value))
                {
                    cmbRelationSymbol.Text = value;
                }
            }
        }

        public UserControlFindCondition(string[] arrayFindFild, IConditionForm parentForm)
        {
            InitializeComponent();

            m_parentForm = parentForm;

            cmbFindField.Items.AddRange(arrayFindFild);
            cmbFindField.SelectedIndex = 0;
            cmbOperator.SelectedIndex = 0;
            cmbRelationSymbol.SelectedIndex = 0;
            cmbLeft.SelectedIndex = 0;
            cmbRight.SelectedIndex = 0;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            m_parentForm.DeleteCondition(this);
        }

        /// <summary>
        /// 改变查询项
        /// </summary>
        void ChangeSelectFindItem()
        {
            m_dataType = typeof(string).Name;

            if (cmbFindField.SelectedItem.ToString().Contains("日期") || cmbFindField.SelectedItem.ToString().Contains("时间"))
            {
                m_dataType = typeof(DateTime).Name;

                txtFindData.Visible = false;
                cmbFindData.Visible = false;
                btnFind.Enabled = false;
                numCount.Visible = false;
                numPrice.Visible = false;
                dateTime.Visible = true;
            }
            else
            {
                dateTime.Visible = false;
                cmbFindData.Visible = false;
                btnFind.Enabled = false;
                numCount.Visible = false;
                numPrice.Visible = false;
                txtFindData.Visible = true;
            }
        }

        /// <summary>
        /// 改变下拉框选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comFindField_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSelectFindItem();

            if (cmbFindField.SelectedItem.ToString() != m_fieldName)
            {
                txtFindData.Text = "";
                dateTime.Value = DateTime.Now;

                if (cmbFindData.Items.Count > 0)
                {
                    cmbFindData.SelectedIndex = 0;
                }
                else
                {
                    cmbFindData.SelectedItem = null;
                }
            }

            if (txtFindData.Visible && cmbOperator.SelectedItem != null 
                && (cmbOperator.SelectedItem.ToString() == "是" || 
                cmbOperator.SelectedItem.ToString() == "不是"))
            {
                txtFindData.Text = "空";
            }
        }

        /// <summary>
        /// 按下鼠标键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comFindField_MouseDown(object sender, MouseEventArgs e)
        {
            m_fieldName = cmbFindField.SelectedItem.ToString();
        }

        /// <summary>
        /// 判断是否正确的条件
        /// </summary>
        /// <param name="strCondition">查询条件</param>
        /// <returns>正确返回true</returns>
        private bool IsValidCondition(string strCondition)
        {
            /*if (GlobalObject.GeneralFunction.IsNullOrEmpty(strCondition))
            {
                return false;
            }
            else */
            if (strCondition.Contains("'"))
            {
                return false;
            }
            else if (strCondition.Contains(';'))
            {
                return false;
            }
            else if (strCondition.Contains("'"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 构建SQL查询where语句字段
        /// </summary>
        /// <param name="lastFlag">是否为最后一个条件,True表示是最后一个条件,False表示不是最后一个条件</param>
        public string BuildSQLSentence(string typeName, bool lastFlag)
        {
            string findData = txtFindData.Text;

            m_sentenceSQL = null;

            if (dateTime.Visible)
            {
                findData = string.Format("{0}-{1}-{2}", dateTime.Value.Date.Year, dateTime.Value.Month, dateTime.Value.Day);
            }
            else if (cmbFindData.Visible)
            {
                findData = cmbFindData.SelectedItem.ToString();
            }
            else if (txtFindData.Visible)
            {
                findData = txtFindData.Text;
            }
            else if (numPrice.Visible)
            {
                findData = numPrice.Value.ToString();
            }
            else if (numCount.Visible)
            {
                findData = numCount.Value.ToString();
            }

            if (!IsValidCondition(findData))
            {
                throw new Exception("请输入正确的查询条件！");
            }

            string centerString = null;

            if (cmbOperator.Text == "包含")
            {
                centerString = cmbFindField.SelectedItem.ToString() + " like '%" + findData + "%'";
            }
            else if (cmbOperator.Text == "是" || cmbOperator.Text == "不是")
            {
                if (findData.Trim().ToLower() != "空")
                {
                    string error = "当操作符选择 “是”或“不是”时查找数据值只能为“空”";

                    MessageDialog.ShowPromptMessage(error);
                    
                    txtFindData.Text = "空";
                    throw new ArgumentException(error);
                }

                string operatorSymbol = "is";
                
                if (cmbOperator.Text == "不是")
                {
                    operatorSymbol = "is not";
                }

                centerString = string.Format("{0} {1} {2} ", cmbFindField.Text, operatorSymbol, "null"); //findData);
            }
            else
            {
                if (findData != "")
                {
                    if (dateTime.Visible)
                    {
                        if (cmbOperator.Text == "=")
                        {
                            if (typeName != "")
                            {
                                centerString = string.Format("({0} >= '{1} 00:00:00' " +
                                                            " and {0} <= '{1} 00:00:00')",
                                                            cmbFindField.SelectedItem.ToString(), findData);
                            }
                            else
                            {
                                centerString = string.Format("( CAST({0} as DateTime ) >= '{1} 00:00:00' " +
                                                            " and CAST({0} as DateTime) <= '{1} 00:00:00')",
                                                            cmbFindField.SelectedItem.ToString(), findData);
                            }
                        }
                        else
                        {
                            if (typeName != "")
                            {
                                centerString = string.Format("({0}{1} '{2} 00:00:00')",
                                    cmbFindField.SelectedItem.ToString(), cmbOperator.SelectedItem.ToString(), findData);
                            }
                            else
                            {
                                centerString = string.Format("( CAST({0} as DateTime ) {1} '{2} 00:00:00')",
                                    cmbFindField.SelectedItem.ToString(), cmbOperator.SelectedItem.ToString(), findData);
                            }
                        }
                    }
                    else
                    {
                        centerString = cmbFindField.SelectedItem.ToString() + cmbOperator.SelectedItem.ToString() + "'" + findData + "'";
                    }
                }
                else
                {
                    centerString = "(" + cmbFindField.SelectedItem.ToString() + cmbOperator.SelectedItem.ToString() + "'" 
                                       + findData + "'" + " or " + cmbFindField.SelectedItem.ToString() + " is null" + ")";
                }
            }

            if (cmbLeft.SelectedItem != null)
            {
                centerString = cmbLeft.SelectedItem.ToString() + centerString;
            }

            if (cmbRight.SelectedItem != null)
            {
                centerString = centerString + cmbRight.SelectedItem.ToString();
            }

            if (!lastFlag)
            {
                m_sentenceSQL = centerString + " " + cmbRelationSymbol.SelectedItem.ToString() + " ";

            }
            else
            {
                m_sentenceSQL = centerString;
            }

            return m_sentenceSQL;
        }

        private void cmbOperator_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbOperator.SelectedItem.ToString() == "是" || cmbOperator.SelectedItem.ToString() == "不是")
            {
                txtFindData.Text = "空";
            }
        }
    }
}
