/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormFilterCondition.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/11/06
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 关于界面
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/11/06 11:14:08 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using System.Reflection;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 单据查询界面类
    /// </summary>
    public partial class FormFilterCondition : Form, IConditionForm
    {
        /// <summary>
        /// 标题
        /// </summary>
        string m_title;

        /// <summary>
        /// 查询结果
        /// </summary>
        IQueryResult m_queryResult = null;

        /// <summary>
        /// SQL查询where语句字段
        /// </summary>
        string m_sentenceSQL = null;

        /// <summary>
        /// 查询字段
        /// </summary>
        string[] m_arrayFindFild;

        /// <summary>
        /// 新增条件次数
        /// </summary>
        int m_count;

        /// <summary>
        /// 条件控件列表
        /// </summary>
        List<UserControlFindCondition> m_lstControl = new List<UserControlFindCondition>();

        /// <summary>
        /// 查询的业务名称
        /// </summary>
        string m_findBusiness;

        /// <summary>
        /// 查询的业务名称
        /// </summary>
        public string Business
        {
            get { return m_findBusiness; }
        }

        /// <summary>
        /// 过滤信息
        /// </summary>
        private List<FilterInfo> m_lstFilter;

        /// <summary>
        /// 获取查询过滤信息列表
        /// </summary>
        internal List<FilterInfo> QueryFilter
        {
            get { return m_lstFilter; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="arrayFindFild">查找用的字段</param>
        /// <param name="findBusiness">要查询的业务</param>
        public FormFilterCondition(string title, string[] arrayFindFild, string findBusiness)
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            m_title = title;
            this.Text = title + this.Text;

            m_arrayFindFild = arrayFindFild;
            m_findBusiness = findBusiness;

            m_lstFilter = QueryFilterControl.GetFilterInfo(title);

            panelParameter.Controls.Clear();
            panelTop.Height = 120;

            if (m_lstFilter != null)
            {
                foreach (var item in m_lstFilter)
                {
                    UserControlFindCondition tmpControl = new UserControlFindCondition(m_arrayFindFild, this);

                    tmpControl.LeftParentheses = item.LeftParentheses;
                    tmpControl.FieldName = item.FieldName;
                    tmpControl.Operator = item._operator1;
                    tmpControl.DataType = item.DataType;
                    tmpControl.DataValue = item.DataValue;
                    tmpControl.RightParentheses = item.RghtParentheses;
                    tmpControl.LogicSymbol = item.Logic;

                    tmpControl.Parent = panelParameter;
                    tmpControl.Dock = DockStyle.Bottom;
                    panelTop.Height = panelTop.Height + tmpControl.Height;
                    m_count++;

                    tmpControl.Name = m_count.ToString();
                    m_lstControl.Add(tmpControl);
                }
            }
        }

        private void btnAddCondition_MouseEnter(object sender, EventArgs e)
        {
            btnAddCondition.ForeColor = Color.Red;
        }

        private void btnAddCondition_MouseLeave(object sender, EventArgs e)
        {
            btnAddCondition.ForeColor = Color.Blue;
        }

        private void btnAddCondition_MouseDown(object sender, MouseEventArgs e)
        {
            btnAddCondition.ForeColor = Color.Gold;
        }

        private void btnAddCondition_MouseUp(object sender, MouseEventArgs e)
        {
            btnAddCondition.ForeColor = Color.Blue;
        }

        /// <summary>
        /// 新增条件
        /// </summary>
        private void btnAddCondition_Click(object sender, EventArgs e)
        {
            UserControlFindCondition tmpControl = new UserControlFindCondition(m_arrayFindFild, this);

            tmpControl.Parent = panelParameter;
            tmpControl.Dock = DockStyle.Bottom;
            panelTop.Height = panelTop.Height + tmpControl.Height;
            m_count++;

            tmpControl.Name = m_count.ToString();
            m_lstControl.Add(tmpControl);
        }

        /// <summary>
        /// 保存我的查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!TestFilterCondition())
                    return;

                List<FilterInfo> lstFilter = new List<FilterInfo>();

                for (int i = 0; i < panelParameter.Controls.Count; i++)
                {
                    UserControlFindCondition control = panelParameter.Controls[i] as UserControlFindCondition;

                    FilterInfo info = new FilterInfo();

                    info.OrderNo = i;
                    info.FieldName = control.FieldName;
                    info.DataValue = control.DataValue;

                    if (m_queryResult != null)
                        info.DataType = m_queryResult.DataCollection.Tables[0].Columns[info.FieldName].DataType.Name;
                    else
                        info.DataType = control.DataType;

                    info.Logic = control.LogicSymbol;
                    info._operator1 = control.Operator;
                    info.LeftParentheses = control.LeftParentheses;
                    info.RghtParentheses = control.RightParentheses;

                    lstFilter.Add(info);
                }

                QueryFilterControl.SaveFilter(m_title, lstFilter);
                QueryFilterControl.Save();

                MessageDialog.ShowPromptMessage("成功保存查询过滤条件！");
            }
            catch (Exception exce)
            {
                MessageDialog.ShowErrorMessage(exce.Message);
            }
        }

        /// <summary>
        /// 提交并查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFind_Click(object sender, EventArgs e)
        {
            if (panelParameter.Controls.Count == 0)
            {
                MessageDialog.ShowPromptMessage("您还没有建立查询条件无法进行此操作！");
                return;
            }

            if (!TestFilterCondition())
                return;

            dataGridView1.DataSource = m_queryResult.DataCollection.Tables[0];
            lblAmount.Text = dataGridView1.Rows.Count.ToString();

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if (dataGridView1.Columns[i].Name.Contains("标志") || dataGridView1.Columns[i].Name.Contains("领料类型") || 
                    dataGridView1.Columns[i].Name.Contains("人编码") || 
                    dataGridView1.Columns[i].Name.Contains("权限控制") || dataGridView1.Columns[i].Name.Contains("序号"))
                {
                    dataGridView1.Columns[i].Visible = false;
                }
            }

            #region 隐藏不允许查看的列

            if (m_queryResult.HideFields != null && m_queryResult.HideFields.Length > 0)
            {
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (m_queryResult.HideFields.Contains(dataGridView1.Columns[i].Name))
                    {
                        dataGridView1.Columns[i].Visible = false;
                    }
                }
            }

            #endregion

            dataGridView1.Refresh();
        }

        /// <summary>
        /// 测试过滤条件
        /// </summary>
        /// <returns>成功返回true</returns>
        private bool TestFilterCondition()
        {
            int leftParentheses = 0;
            int rightParentheses = 0;
            m_sentenceSQL = "";

            try
            {
                for (int i = 0; i < panelParameter.Controls.Count; i++)
                {
                    UserControlFindCondition control = panelParameter.Controls[i] as UserControlFindCondition;

                    leftParentheses += control.LeftParentheses.Length == 0 ? 0 : 1;
                    rightParentheses += control.RightParentheses.Length == 0 ? 0 : 1;

                    if (i != panelParameter.Controls.Count - 1)
                    {
                        m_sentenceSQL = m_sentenceSQL + control.BuildSQLSentence("",false);
                    }
                    else
                    {
                        m_sentenceSQL = m_sentenceSQL + control.BuildSQLSentence("",true);
                    }
                }

                if (leftParentheses != rightParentheses)
                {
                    MessageDialog.ShowPromptMessage("左右括号数量不匹配");
                    return false;
                }
            }
            catch (Exception exce)
            {
                Console.WriteLine(exce.Message);
                return false;
            }

            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "";

            switch (m_findBusiness)
            {
                case "报检入库单":
                    businessID = "报检入库单查询";
                    break;
                case "普通入库单":
                    businessID = "普通入库单查询";
                    break;
                case "采购退货单":
                    businessID = "采购退货单查询";
                    break;
                case "领料单":
                    businessID = "领料单查询";
                    break;
                case "领料退库单":
                    businessID = "领料退库单查询";
                    break;
                case "销售退库单":
                    businessID = "销售退库单查询";
                    break;
                case "销售出库单":
                    businessID = "销售出库单查询";
                    break;
                case "产品入库单":
                    businessID = "产品入库单查询";
                    break;
                case "还货单":
                    businessID = "还货单查询";
                    break;
                case "借货单":
                    businessID = "借货单查询";
                    break;
                case "报废单":
                    businessID = "报废单查询";
                    break;
                case "合同信息管理":
                    businessID = "合同信息综合查询";
                    break;
                case "订单信息管理":
                    businessID = "订单信息综合查询";
                    break;
                case "自制件入库单":
                    businessID = "自制件入库单查询";
                    break;
                case "产品型号变更单":
                    businessID = "产品型号变更单查询";
                    break;

            }

            m_queryResult = authorization.Query(businessID, null, m_sentenceSQL);

            if (!m_queryResult.Succeeded)
            {
                MessageDialog.ShowErrorMessage(m_queryResult.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 删除条件
        /// </summary>
        /// <param name="tmpControl"></param>
        public void DeleteCondition(UserControlFindCondition tmpControl)
        {
            panelTop.Height = panelTop.Height - tmpControl.Height;
            panelParameter.Controls.Remove(tmpControl);
            m_lstControl.RemoveAll(p => p.Name == tmpControl.Name);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                lblCurRow.Text = (e.RowIndex+1).ToString();
            }
        }
    }
}
