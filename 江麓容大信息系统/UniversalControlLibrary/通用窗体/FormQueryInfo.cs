/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormQueryInfo.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/09/11
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 用于灵活获取信息的窗体
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/09/11 作者: 夏石友 当前版本: V1.00
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
using GlobalObject;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 零件信息界面类
    /// </summary>
    public partial class FormQueryInfo : Form
    {
        public event GlobalObject.DelegateCollection.DelegateFormQueryInfo OnDelegateFormQueryInfo;

        /// <summary>
        /// 数据字典
        /// </summary>
        private Dictionary<string, object> m_dicData = new Dictionary<string, object>();

        public Dictionary<string, object> DicData
        {
            get { return m_dicData; }
            set { m_dicData = value; }
        }

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 需要显示的列
        /// </summary>
        string[] m_showColumns;

        /// <summary>
        /// 需要隐藏的列
        /// </summary>
        string[] m_hideColumns;

        /// <summary>
        /// 获取数据项
        /// </summary>
        /// <param name="name">数据名称</param>
        /// <returns>返回获取到的数据值</returns>
        public Object GetDataItem(string name)
        {
            if (m_dicData.ContainsKey(name))
            {
                return m_dicData[name];
            }

            return null;
        }

        /// <summary>
        /// 获取值为字符串类型的数据项(数据类型不正确时会引发异常)
        /// </summary>
        /// <param name="name">数据名称</param>
        /// <returns>返回获取到的数据值</returns>
        public string GetStringDataItem(string name)
        {
            if (m_dicData.ContainsKey(name))
            {
                if (m_dicData[name] != System.DBNull.Value)
                    return m_dicData[name].ToString();
                else
                    return "";
            }

            return null;
        }

        /// <summary>
        /// 获取值为字符串类型的数据项(数据类型不正确时会引发异常)
        /// </summary>
        /// <param name="name">数据名称</param>
        /// <returns>返回获取到的数据值</returns>
        public string this[string name]
        {
            get
            {
                if (m_dicData.ContainsKey(name))
                {
                    if (m_dicData[name] != System.DBNull.Value)
                        return m_dicData[name].ToString();
                    else
                        return "";
                }

                return null;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        public FormQueryInfo(IQueryable dataSource)
        {
            Init();
            dataGridView1.DataSource = dataSource;
        }

        public FormQueryInfo(PlatformManagement.IQueryResult dataSource)
        {
            Init();
            dataGridView1.DataSource = dataSource.DataCollection.Tables[0];
        }

        public FormQueryInfo(DataTable dataSource)
        {
            Init();
            dataGridView1.DataSource = dataSource;
        }

        public FormQueryInfo(object dataSource)
        {
            Init();
            dataGridView1.DataSource = dataSource;
        }

        /// <summary>
        /// 设置隐藏列
        /// </summary>
        public string[] HideColumns
        {
            set
            {
                if (value == null)
                    return;

                m_hideColumns = value;
            }
        }

        /// <summary>
        /// 设置显示列
        /// </summary>
        public string[] ShowColumns
        {
            set
            {
                if (value == null)
                    return;

                m_showColumns = value;
            }
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormQueryInfo_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);

            if (dataGridView1.Columns.Count != 0)
            {
                if (dataGridView1.Columns.Contains("序号"))
                {
                    dataGridView1.Columns["序号"].Visible = false;
                }
                else if (dataGridView1.Columns.Contains("ID"))
                {
                    dataGridView1.Columns["ID"].Visible = false;
                }
            }
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormQueryInfo_Load(object sender, EventArgs e)
        {
            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);
        }

        /// <summary>
        /// 选定记录行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemSelect_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                m_dicData.Clear();

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    m_dicData.Add(dataGridView1.Columns[i].Name, row.Cells[i].Value);
                }

                if (OnDelegateFormQueryInfo != null)
                {
                    if (OnDelegateFormQueryInfo(dataGridView1.CurrentRow) == DialogResult.OK)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        return;
                    }
                }

                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageDialog.ShowPromptMessage("没有选定的记录行！");
            }
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

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            menuItemSelect.PerformClick();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void FormQueryInfo_Shown(object sender, EventArgs e)
        {
            if (m_hideColumns != null)
            {
                for (int col = 0; col < dataGridView1.Columns.Count; col++)
                {
                    dataGridView1.Columns[col].Visible = true;

                    if (m_hideColumns.Contains(dataGridView1.Columns[col].Name))
                    {
                        dataGridView1.Columns[col].Visible = false;
                    }
                }
            }
            else if (m_showColumns != null)
            {
                for (int col = 0; col < dataGridView1.Columns.Count; col++)
                {
                    dataGridView1.Columns[col].Visible = false;

                    if (m_showColumns.Contains(dataGridView1.Columns[col].Name))
                    {
                        dataGridView1.Columns[col].Visible = true;
                    }
                }
            }

            if (m_dataLocalizer == null)
            {
                // 添加数据定位控件
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                    ServerModule.UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

                panel1.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }
        }
    }
}
