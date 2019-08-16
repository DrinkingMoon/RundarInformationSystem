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
    public partial class FormConditionDataTableFind : Form, IConditionForm
    {
        /// <summary>
        /// 查询参数服务接口
        /// </summary>
        ISearchParamsServer m_searchParamsServer = BasicServerFactory.GetServerModule<ISearchParamsServer>();

        /// <summary>
        /// 查询结果表
        /// </summary>
        DataTable m_quertResultTable;

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
        /// 存储标志
        /// </summary>
        bool m_saveFlag;

        /// <summary>
        /// 是否保存查询,True表示保存查询,False表示不保存查询
        /// </summary>
        public bool SaveFlag
        {
            get { return m_saveFlag; }
        }

        string _KeyName = null;

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw 
                | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        public event GlobalObject.DelegateCollection.ShowDataGirdViewInfo FormShow;

        public FormConditionDataTableFind(DataTable dataSource, string keyName)
        {
            List<string> tempList = new List<string>();

            foreach (DataColumn col in dataSource.Columns)
            {
                tempList.Add(col.ColumnName);
            }

            m_arrayFindFild = tempList.ToArray();
            m_quertResultTable = dataSource;
            _KeyName = keyName;

            Init();
        }

        public FormConditionDataTableFind(DataTable dataSource)
        {
            List<string> tempList = new List<string>();

            foreach (DataColumn col in dataSource.Columns)
            {
                tempList.Add(col.ColumnName);
            }

            m_arrayFindFild = tempList.ToArray();
            m_quertResultTable = dataSource;

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
            string m_sentenceSQL = null;

            try
            {
                for (int i = 0; i < panelParameter.Controls.Count; i++)
                {
                    UserControlFindCondition newControl = panelParameter.Controls[i] as UserControlFindCondition;

                    if (i != panelParameter.Controls.Count - 1)
                    {
                        m_sentenceSQL = m_sentenceSQL + newControl.BuildSQLSentence("报表",false);
                    }
                    else
                    {
                        m_sentenceSQL = m_sentenceSQL + newControl.BuildSQLSentence("报表", true);
                    }
                }
            }
            catch (Exception exce)
            {
                Console.WriteLine(exce.Message);
                return;
            }

            if (m_sentenceSQL != null)
            {
                string error = null;
                dataGridView1.DataSource = 
                    DataSetHelper.SiftDataTable(m_quertResultTable, m_sentenceSQL, out error);
            }
            else
            {
                dataGridView1.DataSource = m_quertResultTable;
            }

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

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
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            string strColName = "";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            dataGridView1.ClearSelection();
            dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[strColName];
        }

        private void 数据展示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选中数据行后再进行此操作");
                return;
            }

            FormViewData form = new FormViewData(dataGridView1.Columns, dataGridView1.SelectedRows[0]);
            form.ShowDialog();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (_KeyName != null)
            {
                if (dataGridView1.Columns.Contains(_KeyName))
                {
                    if (FormShow != null)
                    {
                        FormShow(dataGridView1, _KeyName);
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("查询功能获取失败");
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("查询结果集中无关键字段【"+ _KeyName +"】");
                }

            }

        }
    }
}
