/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormFindCondition.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/15
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 关于界面
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2009/07/03 08:02:08 作者: 夏石友 当前版本: V1.00
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
using GlobalObject;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 条件查询窗体
    /// </summary>
    public partial class FormConditionFind : Form, IConditionForm
    {
        /// <summary>
        /// 查询参数服务接口
        /// </summary>
        ISearchParamsServer m_searchParamsServer = BasicServerFactory.GetServerModule<ISearchParamsServer>();

        /// <summary>
        /// 窗体消息发布器
        /// </summary>
        WndMsgSender m_wndMsgSender = new WndMsgSender();

        /// <summary>
        /// 查询结果表
        /// </summary>
        DataTable m_quertResultTable;

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
        /// 查询条件字典
        /// </summary>
        Dictionary<string, UserControlFindCondition> m_controlDic = new Dictionary<string, UserControlFindCondition>();

        /// <summary>
        /// 查询条件临时字典
        /// </summary>
        Dictionary<string, UserControlFindCondition> m_tempControlDic = new Dictionary<string, UserControlFindCondition>();

        /// <summary>
        /// 查询的业务名称
        /// </summary>
        string m_findBusiness;

        /// <summary>
        /// 回调窗体
        /// </summary>
        Control m_callbackForm;

        /// <summary>
        /// 存储标志
        /// </summary>
        bool m_saveFlag;

        /// <summary>
        /// 调用本功能的上级界面标题
        /// </summary>
        string m_parentTitle;

        /// <summary>
        /// 带参的业务查询的参数集合
        /// </summary>
        string[] m_proParameter;

        /// <summary>
        /// 是否保存查询,True表示保存查询,False表示不保存查询
        /// </summary>
        public bool SaveFlag
        {
            get { return m_saveFlag; }
        }

        /// <summary>
        /// 查询的业务名称
        /// </summary>
        public string Business
        {
            get { return m_findBusiness; }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            if (!GlobalObject.GeneralFunction.IsNullOrEmpty(m_parentTitle))
            {
                string[] paramsName = m_searchParamsServer.GetSearchName(m_parentTitle, Business);

                if (paramsName != null)
                {
                    panelSelectSearch.Visible = true;

                    cmbSearchName.Items.Clear();
                    cmbSearchName.Items.Add("");
                    cmbSearchName.Items.AddRange(paramsName);
                }
            }
        }

        public FormConditionFind(string[] arrayFindFild, string findBusiness)
        {
            m_arrayFindFild = arrayFindFild;
            m_findBusiness = findBusiness;

            Init();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="callbackForm">回调窗体</param>
        /// <param name="arrayFindFild">查找用的字段</param>
        /// <param name="findBusiness">要查询的业务</param>
        public FormConditionFind(Control callbackForm, string[] arrayFindFild, string findBusiness)
        {
            m_wndMsgSender = new WndMsgSender();
            m_callbackForm = callbackForm;
            m_arrayFindFild = arrayFindFild;
            m_findBusiness = findBusiness;

            Init();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="callbackForm">回调窗体</param>
        /// <param name="arrayFindFild">查找用的字段</param>
        /// <param name="findBusiness">要查询的业务</param>
        /// <param name="parentTitle">调用本功能的上级界面标题</param>
        public FormConditionFind(Control callbackForm, string[] arrayFindFild, string findBusiness, string parentTitle)
        {
            m_wndMsgSender = new WndMsgSender();
            m_callbackForm = callbackForm;
            m_arrayFindFild = arrayFindFild;
            m_findBusiness = findBusiness;
            m_parentTitle = parentTitle;

            Init();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="callbackForm">回调窗体</param>
        /// <param name="arrayFindFild">查找用的字段</param>
        /// <param name="findBusiness">要查询的业务</param>
        /// <param name="parentTitle">调用本功能的上级界面标题</param>
        public FormConditionFind(Control callbackForm, string[] arrayFindFild, string findBusiness, string parentTitle, string[] proParameter)
        {
            m_wndMsgSender = new WndMsgSender();
            m_callbackForm = callbackForm;
            m_arrayFindFild = arrayFindFild;
            m_findBusiness = findBusiness;
            m_parentTitle = parentTitle;
            m_proParameter = proParameter;

            Init();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="arrayFindFild">查找用的字段</param>
        /// <param name="findBusiness">要查询的业务</param>
        /// <param name="parentTitle">调用本功能的上级界面标题</param>
        public FormConditionFind(string[] arrayFindFild, string findBusiness, string parentTitle)
        {
            m_arrayFindFild = arrayFindFild;
            m_findBusiness = findBusiness;
            m_parentTitle = parentTitle;

            Init();
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormFindCondition_Load(object sender, EventArgs e)
        {
            if (panelParameter.Controls.Count > 0 || dataGridView1.Rows.Count > 0)
                return;

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(m_parentTitle))
            {
                btnSaveCondition.Visible = false;
                btnFind.Left -= btnSaveCondition.Width;
                btnSave.Left -= btnSaveCondition.Width;
            }

            panelParameter.Controls.Clear();
            panelTop.Height = 120;

            foreach (var item in m_controlDic)
            {
                panelParameter.Controls.Add(item.Value);
                item.Value.Dock = DockStyle.Bottom;
                panelTop.Height = panelTop.Height + item.Value.Height;
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

            tmpControl.Name = tmpControl.Name + m_count.ToString();
            m_tempControlDic.Add(tmpControl.Name, tmpControl);
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormFindCondition_Resize(object sender, EventArgs e)
        {
            panelLeft.Width = (this.Width - panelCenter.Width) / 2;
            panelRight.Width = this.Width - panelCenter.Width - panelLeft.Width;
        }

        /// <summary>
        /// 保存我的查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (m_quertResultTable == null || m_quertResultTable.Columns.Count == 0 || m_quertResultTable.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("没有数据无法保存查询结果");
                return;
            }

            m_saveFlag = true;

            CreateExcelFile();

            //m_controlDic.Clear();

            //for (int i = 0; i < panelParameter.Controls.Count; i++)
            //{
            //    UserControlFindCondition newControl = panelParameter.Controls[i] as UserControlFindCondition;
            //    newControl.SaveFlag = true;
            //    m_controlDic.Add(newControl.Name, newControl);
            //}
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

            m_sentenceSQL = null;
            string pareTemp = "";

            try
            {
                for (int i = 0; i < panelParameter.Controls.Count; i++)
                {
                    UserControlFindCondition newControl = panelParameter.Controls[i] as UserControlFindCondition;

                    if (i != panelParameter.Controls.Count - 1)
                    {
                        m_sentenceSQL = m_sentenceSQL + newControl.BuildSQLSentence("",false);
                    }
                    else
                    {
                        m_sentenceSQL = m_sentenceSQL + newControl.BuildSQLSentence("",true);

                        if (newControl.FieldName == "单据号")
                        {
                            pareTemp = newControl.DataValue;
                        }
                    }
                }
            }
            catch (Exception exce)
            {
                Console.WriteLine(exce.Message);
                return;
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
                case "报废单":
                    businessID = "报废单查询";
                    break;
                case "报废单综合查询":
                    businessID = m_findBusiness;
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
                case "采购退货单综合查询":
                    businessID = m_findBusiness;
                    break;
                case "领料单综合查询":
                    businessID = m_findBusiness;
                    break;
                case "样品确认申请单":
                    businessID = "样品确认申请单综合查询";
                    break;
                case "不合格品隔离处置单":
                    businessID = "不合格品隔离处置单查询";
                    break;
                case "挑选返工返修单":
                    businessID = "挑选返工返修单查询";
                    break;
                case "供应质量信息反馈单":
                    businessID = "供应商质量信息反馈单查询";
                    break;
                case "委外报检入库单":
                    businessID = "委外报检入库单查询";
                    break;
                case "产品型号变更单":
                    businessID = "产品型号变更单查询";
                    break;
                default:
                    businessID = m_findBusiness;
                    break;
            }

            string sql = m_sentenceSQL.ToLower();

            if (sql.LastIndexOf(" union ") != -1)
            {
                MessageDialog.ShowPromptMessage("存在非法字符，无法进行此操作！");
                return;
            }

            if (businessID == "培训综合统计查询" &&
                (m_sentenceSQL.Contains("培训开始时间") || m_sentenceSQL.Contains("培训终止时间")))
            {
                m_sentenceSQL = m_sentenceSQL + " or 培训开始时间 is null";                
            }

            IQueryResult qr = authorization.Query(businessID, null, m_sentenceSQL);

            if (m_findBusiness == "零星采购申请财务综合查询")
            {
                string[] pare = { pareTemp};
                qr = authorization.QueryMultParam(businessID, m_sentenceSQL, pare);
            }

            if (m_findBusiness == "出差综合查询")
            {
                string[] pare = { BasicInfo.DeptCode };
                qr = authorization.QueryMultParam(businessID,m_sentenceSQL, pare);
            }

            if (m_findBusiness == "出差人员查看")
            {
                string[] pare = { BasicInfo.LoginID };
                qr = authorization.QueryMultParam(businessID, m_sentenceSQL, pare);
            }

            if (m_findBusiness == "订单入库单综合查询")
            {
                qr = authorization.QueryMultParam(businessID, m_sentenceSQL, m_proParameter);
            }

            if (!qr.Succeeded)
            {
                m_quertResultTable = null;
                MessageDialog.ShowErrorMessage(qr.Error);
            }
            else
            {
                m_quertResultTable = qr.DataCollection.Tables[0];
                lblAmount.Text = m_quertResultTable.Rows.Count.ToString();
            }

            dataGridView1.DataSource = m_quertResultTable;

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if (dataGridView1.Columns[i].Name.Contains("标志") || dataGridView1.Columns[i].Name.Contains("领料类型") || 
                    dataGridView1.Columns[i].Name.Contains("人编码") || //dataGridView1.Columns[i].Name.Contains("签名")  ||
                    dataGridView1.Columns[i].Name.Contains("权限控制") || dataGridView1.Columns[i].Name.Contains("序号"))
                {
                    dataGridView1.Columns[i].Visible = false;
                }
            }

            #region 隐藏不允许查看的列

            if (qr.HideFields != null && qr.HideFields.Length > 0)
            {
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (qr.HideFields.Contains(dataGridView1.Columns[i].Name))
                    {
                        dataGridView1.Columns[i].Visible = false;
                    }
                }
            }

            #endregion

            if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.采购账务管理员.ToString())
                && !BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString()))
            {
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (dataGridView1.Columns[i].Name.Contains("单价") || dataGridView1.Columns[i].Name.Contains("金额"))
                    {
                        dataGridView1.Columns[i].Visible = false;
                    }
                }
            }

            dataGridView1.Refresh();
        }

        /// <summary>
        /// 删除条件
        /// </summary>
        /// <param name="tmpControl"></param>
        public void DeleteCondition(UserControlFindCondition tmpControl)
        {
            panelTop.Height = panelTop.Height - tmpControl.Height;
            panelParameter.Controls.Remove(tmpControl);
            m_tempControlDic.Remove(tmpControl.Name);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                lblCurRow.Text = (e.RowIndex+1).ToString();
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Modify by cjb on 2016.11.8 适用性不强，暂时禁用
            //if (m_callbackForm == null || e.RowIndex < 0 || e.ColumnIndex < 0)
            //{
            //    return;
            //}

            //Modify by cjb on 2016.11.8 适用性不强，暂时禁用
            //string billNo = null;

            //for (int i = 0; i < dataGridView1.Columns.Count; i++)
            //{
            //    if (dataGridView1.Columns[i].Name.Contains("单号"))
            //    {
            //        billNo = dataGridView1.Rows[e.RowIndex].Cells[i].Value.ToString();
            //        break;
            //    }
            //    else if (dataGridView1.Columns[i].Name.Contains("单据号"))
            //    {
            //        billNo = dataGridView1.Rows[e.RowIndex].Cells[i].Value.ToString();
            //        break;
            //    }
            //}

            //if (billNo == null)
            //{
            //    return;
            //}

            //WndMsgData sendData = new WndMsgData();

            //sendData.MessageContent = billNo;
            //m_wndMsgSender.SendMessage(m_callbackForm.Handle, WndMsgSender.PositioningMsg, sendData);

            //this.Close();
        }

        /// <summary>
        /// 是否必须为文本模式
        /// </summary>
        /// <param name="columnName">要判断的列名</param>
        /// <returns>是返回true</returns>
        private bool MustTextMode(string columnName)
        {
            // 文本列, 有些数字列必须为文本模式，否则在EXCEL中显示会不正确，如电话073188123456如果不为文本列则最开头的0会抹掉 
            string[] textColumn = { "联系方式", "电话", "手机", "传真", "批次" };

            foreach (var item in textColumn)
            {
                if (columnName.ToLower().Contains(item.ToLower()))
                {
                    return true;
                }
            }

            return false;
        }

        private void CreateExcelFile()
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);

            //Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            //if (excel == null)
            //{
            //    MessageDialog.ShowPromptMessage("无法打开EXCEL, 不能进行此操作");
            //    return;
            //}

            //Cursor preCursor = Cursor.Current;
            //try
            //{
            //    Microsoft.Office.Interop.Excel.Workbook workbook = excel.Application.Workbooks.Add(true);

            //    char beginColHead = 'A';//A-Z, AA-AZ, BA-BZ
            //    int colWidth = 0;
            //    string[] colHeadArray = { "", "A", "B", "C", "D", "E" };

            //    // 从数据库中获取列宽信息
            //    List<Show_FieldWidth> lstFieldWidth = StapleInfo.FieldWidthServer.GetFieldWidth();

            //    // 获取A-Z列
            //    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)excel.Columns["A:Z", Type.Missing];

            //    // 每行高度
            //    range.RowHeight = 25;

            //    // 每列宽度
            //    //range.ColumnWidth = 15;

            //    // 每行水平居中对齐
            //    range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            //    // 每行垂直居中对齐
            //    range.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

            //    range.WrapText = true;          // 自动换行
            //    //range.EntireColumn.AutoFit(); // 列高根据内容自动调整
            //    //range.EntireRow.AutoFit();      // 行高根据内容自动调整

            //    for (int i = 0; i < m_quertResultTable.Columns.Count; i++)
            //    {
            //        if (!dataGridView1.Columns[i].Visible)
            //        {
            //            m_quertResultTable.Columns.RemoveAt(i);
            //            i--;
            //            continue;
            //        }

            //        #region 设置EXCEL列宽
            //        string colHead = colHeadArray[i / 26] + Convert.ToChar(Convert.ToByte(beginColHead) + i % 26).ToString();

            //        List<Show_FieldWidth> fwInfo = StapleInfo.FieldWidthServer.GetFieldWidth(lstFieldWidth, m_quertResultTable.Columns[i].ColumnName);

            //        if (fwInfo.Count > 0)
            //        {
            //            colWidth = fwInfo[0].ExcelColumnWidth;
            //        }
            //        else
            //        {
            //            colWidth = 12;
            //        }

            //        ((Microsoft.Office.Interop.Excel.Range)excel.Columns[colHead, Type.Missing]).ColumnWidth = colWidth;
            //        #endregion

            //        excel.Cells[1, i + 1] = m_quertResultTable.Columns[i].ColumnName;
            //    }

            //    int rowIndex = 2;

            //    for (int i = 0; i < m_quertResultTable.Rows.Count; i++)
            //    {
            //        for (int col = 0; col < m_quertResultTable.Columns.Count; col++)
            //        {
            //            if (MustTextMode(m_quertResultTable.Columns[col].ColumnName))
            //            {
            //                excel.Cells[rowIndex, col + 1] = "'" + m_quertResultTable.Rows[i][col];
            //            }
            //            else
            //            {
            //                excel.Cells[rowIndex, col + 1] = m_quertResultTable.Rows[i][col];
            //            }
            //        }

            //        rowIndex++;
            //    }
            //    //excel.Cells[1, 1] = "hello";
            //    //excel.Cells[1, 2] = "world";
            //    //excel.Cells[2, 1] = "hello2";
            //    //excel.Cells[2, 2] = "world2";

            //    //excel.Visible = true;

            //    Cursor.Current = Cursors.WaitCursor;
            //    workbook.SaveAs(saveFileDialog1.FileName, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
            //        Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            //    excel.Quit();
            //    excel = null;
            //}
            //finally
            //{
            //    Cursor.Current = preCursor;

            //    if (excel != null)
            //        excel.Quit();
            //}
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Modify by cjb on 2016.11.8 影响选中行功能
            //if (e.RowIndex < 0 || e.ColumnIndex < 0)
            //{
            //    return;
            //}

            //string strColName = "";

            //foreach (DataGridViewColumn col in dataGridView1.Columns)
            //{
            //    if (col.Visible)
            //    {
            //        strColName = col.Name;
            //        break;
            //    }
            //}

            //dataGridView1.ClearSelection();
            //dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[strColName];
        }

        private void 数据展示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            FormViewData form = new FormViewData(dataGridView1.Columns, dataGridView1.CurrentRow);
            form.ShowDialog();
        }

        /// <summary>
        /// 选择原来保存了的检索名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSearchName_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelParameter.Controls.Clear();
            panelTop.Height = 120;

            if (cmbSearchName.Text != "")
            {
                string error = null;

                IQueryable<SYS_SearchParams> searchParams = m_searchParamsServer.GetParams(
                    m_parentTitle, Business, cmbSearchName.Text, out error);

                if (searchParams == null)
                {
                    return;
                }

                if (!GlobalObject.GeneralFunction.IsNullOrEmpty(error))
                {
                    MessageDialog.ShowErrorMessage(error);
                    return;
                }

                m_count = 0;

                foreach (var item in searchParams)
                {
                    UserControlFindCondition tmpControl = new UserControlFindCondition(m_arrayFindFild, this);

                    tmpControl.LeftParentheses = item.LeftParentheses.ToString();
                    tmpControl.FieldName = item.FieldName;
                    tmpControl.Operator = item.Operator;
                    tmpControl.DataType = item.DataType;
                    tmpControl.DataValue = item.DataValue;
                    tmpControl.RightParentheses = item.RightParentheses.ToString();
                    tmpControl.LogicSymbol = item.Logic;

                    tmpControl.Parent = panelParameter;
                    tmpControl.Dock = DockStyle.Bottom;
                    panelTop.Height = panelTop.Height + tmpControl.Height;
                    m_count++;
                    tmpControl.Name = m_count.ToString();
                }
            }
        }

        /// <summary>
        /// 保存检索条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveCondition_Click(object sender, EventArgs e)
        {
            if (panelParameter.Controls.Count == 0)
            {
                MessageDialog.ShowPromptMessage("还没有设置查询条件无法进行保存");
                return;
            }

            string searchName = InputBox.ShowDialog("保存检索条件", "检索条件名称：", cmbSearchName.Text);

            if (!GlobalObject.GeneralFunction.IsNullOrEmpty(searchName))
            {
                List<SYS_SearchParams> lstParam = new List<SYS_SearchParams>();

                int orderNo = 0;
                string error = null;

                foreach (var item in panelParameter.Controls)
                {
                    SYS_SearchParams param = new SYS_SearchParams();

                    param.BusinessName = m_parentTitle;
                    param.ItemName = Business;
                    param.SearchName = searchName;

                    UserControlFindCondition tmpControl = (UserControlFindCondition)item;

                    //if (tmpControl.LeftParentheses.Length > 0)
                    //    param.LeftParentheses = tmpControl.LeftParentheses[0].ToString();

                    //if (tmpControl.RightParentheses.Length > 0)
                    //    param.RightParentheses = tmpControl.RightParentheses[0].ToString();

                    param.LeftParentheses = tmpControl.LeftParentheses.Trim();
                    param.RightParentheses = tmpControl.RightParentheses.Trim();
                    param.FieldName = tmpControl.FieldName;
                    param.Operator = tmpControl.Operator;
                    param.OrderNo = orderNo++;
                    param.Logic = tmpControl.LogicSymbol;
                    param.DataType = tmpControl.DataType;
                    param.DataValue = tmpControl.DataValue;
                    param.UserCode = GlobalObject.BasicInfo.LoginID;

                    lstParam.Add(param);
                }

                if (!m_searchParamsServer.AddParam(lstParam, out error))
                {
                    MessageDialog.ShowErrorMessage(error);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("成功保存查询条件");

                    if (!cmbSearchName.Items.Contains(searchName))
                    {
                        cmbSearchName.Items.Add("");
                        cmbSearchName.Items.Add(searchName);
                        cmbSearchName.Text = searchName;

                        if (!panelSelectSearch.Visible)
                        {
                            panelSelectSearch.Visible = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 删除我的查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteSearch_Click(object sender, EventArgs e)
        {
            if (cmbSearchName.Text == "")
            {
                MessageDialog.ShowPromptMessage("不允许删除空查询");
                return;
            }
            else
            {
                if (MessageDialog.ShowEnquiryMessage("您真的要删除我的查询“" + cmbSearchName.Text + "”？") == DialogResult.Yes)
                {
                    string error = null;

                    if (!m_searchParamsServer.DeleteParam(m_parentTitle, Business, cmbSearchName.Text, out error))
                    {
                        MessageDialog.ShowErrorMessage(error);
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("操作成功");

                        cmbSearchName.Items.Remove(cmbSearchName.Text);

                        if (cmbSearchName.Items.Count <= 1)
                        {
                            panelSelectSearch.Visible = false;
                        }
                    }
                }
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            数据展示ToolStripMenuItem_Click(sender, e);
        }
    }
}
