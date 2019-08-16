using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 用模糊查询方式给DataGridView中的数据进行定位
    /// </summary>
    public partial class UserControlDataLocalizer : UserControl
    {
        /// <summary>
        /// 起始索引
        /// </summary>
        private int m_startIndex = 0;

        /// <summary>
        /// 获取或设置起始索引
        /// </summary>
        public int StartIndex
        {
            get { return m_startIndex; }
            set { m_startIndex = value; }
        }

        /// <summary>
        /// 要定位的数据控件
        /// </summary>
        private DataGridView m_dataComponent;

        /// <summary>
        /// 基础数据源
        /// </summary>
        private DataTable m_baseDataTable;

        /// <summary>
        /// 数据源
        /// </summary>
        private DataTable m_dataTable;

        /// <summary>
        /// 数据源
        /// </summary>
        private object m_dataSource;

        /// <summary>
        /// 窗体名称
        /// </summary>
        string m_formName = "";

        /// <summary>
        /// DataGridView名称
        /// </summary>
        string m_dataGridViewName = "";

        /// <summary>
        /// 字段集
        /// </summary>
        string[] m_arrayField = null;

        /// <summary>
        /// 系统隐藏字段
        /// </summary>
        string[] m_sysHideFields = null;

        /// <summary>
        /// 是否仅定位的标志，不用过滤
        /// </summary>
        [
            Category("Data"),
            Description("是否仅定位的标志，不用过滤"),
            RefreshProperties(RefreshProperties.All)
        ]
        public bool OnlyLocalize
        {
            get;
            set;
        }

        /// <summary>
        /// 是否立即定位的标志
        /// </summary>
        //private bool m_localizeRightnow;

        /// <summary>
        /// 上一次查询的数据内容
        /// </summary>
        //private string m_preFindContext = "";

        /// <summary>
        /// 是否初始化的标志
        /// </summary>
        private bool m_bInit = false;

        /// <summary>
        /// 是否允许数据绑定标志
        /// </summary>
        private bool m_bAllowDataBindingComplete = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserControlDataLocalizer()
        {
            InitializeComponent();
        }
               
        /// <summary>
        /// 获取或设置控件字体
        /// </summary>
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;

                foreach (var item in this.Controls)
                {
                    if ((item as ToolStrip) != null)
                    {
                        (item as ToolStrip).Font = value;
                    }
                }
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataComponent">关联的数据显示组件</param>
        /// <param name="formName">界面名称</param>
        /// <param name="dtField">需隐藏的字段数据表</param>
        public UserControlDataLocalizer(DataGridView dataComponent, string formName, DataTable dtField)
        {            
            System.Diagnostics.Debug.Assert(dataComponent != null);

            m_formName = formName;

            m_dataGridViewName = dataComponent.Name;

            GetSystemHideField(dataComponent);

            InitializeComponent();

            Init(dataComponent, formName, dtField);
        }

        /// <summary>
        /// 获取系统隐藏字段
        /// </summary>
        /// <param name="dataComponent">组件</param>
        void GetSystemHideField(DataGridView dataComponent)
        {
            #region 获取系统隐藏字段

            List<string> sysHideFields = new List<string>();

            for (int i = 0; i < dataComponent.Columns.Count; i++)
            {
                if (!dataComponent.Columns[i].Visible)
                {
                    sysHideFields.Add(dataComponent.Columns[i].Name);
                }
            }

            m_sysHideFields = sysHideFields.ToArray();

            #endregion
        }

        void dataComponent_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            m_startIndex = e.RowIndex;
        }

        void dataComponent_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (m_bAllowDataBindingComplete)
            {
                m_dataSource = m_dataComponent.DataSource;
                m_dataTable = m_dataSource as DataTable;
                m_baseDataTable = m_dataComponent.DataSource as DataTable;
            }

            if (m_dataComponent.RowHeadersVisible)
            {
                Graphics g = m_dataComponent.CreateGraphics();
                SizeF size = g.MeasureString(string.Format("000{0}", m_dataComponent.Rows.Count), m_dataComponent.Font);

                m_dataComponent.RowHeadersWidth = (int)size.Width;
            }
        }

        /// <summary>
        /// 获得字段集
        /// </summary>
        /// <param name="dataComponent"></param>
        void GetFields(DataGridView dataComponent)
        {
            if (m_sysHideFields == null)
            {
                GetSystemHideField(dataComponent);
            }

            List<string> strList = new List<string>();

            for (int i = 0; i < dataComponent.Columns.Count; i++)
            {
                if (!m_sysHideFields.Contains(dataComponent.Columns[i].Name))
                    strList.Add(dataComponent.Columns[i].Name.ToString());
            }

            m_arrayField = strList.ToArray();
        }

        /// <summary>
        /// 控件初始化
        /// </summary>
        /// <param name="dataComponent">关联的数据显示组件, 不允许为null</param>
        /// <param name="formName">界面名称, 不允许为null</param>
        /// <param name="dtField">需隐藏的字段数据表, 不允许为null</param>
        public void Init(DataGridView dataComponent, string formName, DataTable dtField)
        {
            string strComboxText = cmbFindItem.Text.Trim();

            if (dataComponent == null)
            {
                return;
            }
            else
            {
                GetFields(dataComponent);
            }

            cmbFindItem.Items.Clear();

            m_formName = formName;

            m_dataGridViewName = dataComponent.Name;

            for (int i = 0; i < dataComponent.Columns.Count; i++)
            {
                for (int k = 0; k < m_arrayField.Length; k++)
                {
                    if (dataComponent.Columns[i].Name.ToString() == m_arrayField[k].ToString())
                    {
                        dataComponent.Columns[i].Visible = true;
                    }
                }
            }

            for (int i = 0; i < dataComponent.Columns.Count; i++)
            {
                if (dtField != null)
                {
                    for (int k = 0; k < dtField.Rows.Count; k++)
                    {
                        if (dataComponent.Columns[i].Name.ToString() == dtField.Rows[k][0].ToString())
                        {
                            dataComponent.Columns[i].Visible = false;
                        }
                    }

                }
            }

            m_dataComponent = dataComponent;

            // 显示行头
            m_dataComponent.RowHeadersVisible = true;

            Delegate[] invokeList = GlobalObject.GeneralFunction.GetObjectEventList(m_dataComponent, "RowPostPaint");

            if (invokeList == null)
            {
                m_dataComponent.RowPostPaint -= new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView_RowPostPaint);
                m_dataComponent.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView_RowPostPaint);
            }

            m_baseDataTable = m_dataComponent.DataSource as DataTable;
            m_dataSource = m_dataComponent.DataSource;
            m_dataTable = m_dataSource as DataTable;

            if (cmbFindItem.Items.Count == 0)
            {
                for (int i = 0; i < m_dataComponent.Columns.Count; i++)
                {
                    if (m_dataComponent.Columns[i].Visible)// && m_dataComponent.Columns[i].Name != "序号")
                    {
                        cmbFindItem.Items.Add(m_dataComponent.Columns[i].Name);
                    }
                }

                if (cmbFindItem.Items.Count > 0)
                {
                    if (cmbFindItem.Items[0].ToString() != "序号" && cmbFindItem.Items[0].ToString() != "行号")
                    {
                        if (cmbFindItem.Items.Contains(strComboxText))
                        {
                            cmbFindItem.SelectedItem = strComboxText;
                        }
                        else
                        {
                            cmbFindItem.SelectedIndex = 0;
                        }
                    }
                    else if (cmbFindItem.Items.Count > 1)
                    {
                        cmbFindItem.SelectedIndex = 1;
                    }
                }
            }

            if (!m_bInit)
            {
                m_bInit = true;
                m_bAllowDataBindingComplete = true;

                dataComponent.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataComponent_DataBindingComplete);
                dataComponent.CellEnter += new DataGridViewCellEventHandler(dataComponent_CellEnter);
                this.cmbFindItem.SelectedIndexChanged += new System.EventHandler(this.cmbFindItem_SelectedIndexChanged);
            }
        }

        private void dataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                m_dataComponent.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                m_dataComponent.RowHeadersDefaultCellStyle.Font,
                rectangle,
                m_dataComponent.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public void RefreshData()
        {
            if (m_dataComponent != null)
            {
                m_dataComponent.Refresh();
            }
        }

        /// <summary>
        /// 数据定位
        /// </summary>
        public void LocalizeData()
        {
            if (m_dataComponent.Rows.Count == 0)
            {
                return;
            }

            string findData = txtContext.Text.ToUpper();

            if (!OnlyLocalize)
            {
                // 非仅定位模式（即过滤模式）
                m_dataComponent.ClearSelection();

                CurrencyManager cm = null;

                if (m_dataComponent.DataSource != null)
                {
                    cm = (CurrencyManager)BindingContext[m_dataComponent.DataSource];

                    cm.SuspendBinding(); // 挂起数据绑定
                }

                for (int i = 0; i < m_dataComponent.Rows.Count; i++)
                {
                    if (txtContext.Text.Length == 0)
                    {
                        m_dataComponent.Rows[i].Visible = true;
                    }
                    else
                    {
                        if (m_dataComponent.Rows[i].Cells[cmbFindItem.Text].Value == null ||
                            !m_dataComponent.Rows[i].Cells[cmbFindItem.Text].Value.ToString().ToUpper().Contains(findData))
                        {
                            m_dataComponent.Rows[i].Visible = false;
                        }
                        else
                        {
                            m_dataComponent.Rows[i].Visible = true;
                        }
                    }
                }

                if (m_dataComponent.DataSource != null)
                {
                    cm.ResumeBinding(); // 恢复数据绑定
                }
            }
            else
            {
                // 仅定位模式
                bool find = false;

                if (m_startIndex >= m_dataComponent.Rows.Count)
                {
                    m_startIndex = 0;
                }

                if (m_dataComponent.Rows[m_startIndex].Selected)
                {
                    m_startIndex++;

                    if (m_startIndex >= m_dataComponent.Rows.Count)
                    {
                        m_startIndex = 0;
                    }
                }

                for (int i = m_startIndex; i < m_dataComponent.Rows.Count; i++)
                {
                    if (m_dataComponent.Rows[i].Cells[cmbFindItem.Text].Value != null
                        && m_dataComponent.Rows[i].Cells[cmbFindItem.Text].Value.ToString().ToUpper().Contains(findData))
                    {
                        m_dataComponent.ClearSelection();

                        if (m_dataComponent.Rows[i].Visible)
                        {
                            m_dataComponent.FirstDisplayedScrollingRowIndex = i;
                            m_dataComponent.Rows[i].Selected = true;
                            m_dataComponent.CurrentCell = m_dataComponent.Rows[i].Cells[cmbFindItem.Text];

                            m_startIndex = i + 1;
                            find = true;
                        }

                        break;
                    }
                }

                // 从头开始查找
                if (!find)
                {
                    m_startIndex = 0;

                    if (DialogResult.Yes == MessageDialog.ShowEnquiryMessage("已经检索到记录尾部，没有找到符合条件的信息，是否从头开始？"))
                    {
                        m_dataComponent.ClearSelection();
                        LocalizeData();
                    }
                }
            }
        }

        /// <summary>
        /// 刷新数据源
        /// </summary>
        private void RefreshDataSource()
        {
            if (OnlyLocalize)
            {
                return;
            }

            try
            {
                m_bAllowDataBindingComplete = false;

                if (m_dataTable != null)
                {
                    m_dataComponent.DataSource = 
                        FuzzyFindDataTableRecord.FindRecord(m_baseDataTable, cmbFindItem.Text, txtContext.Text);
                }
                else if (m_dataSource != null)
                {
                    m_dataComponent.DataSource = m_dataSource;
                }
                else
                {
                    for (int i = 0; i < m_dataComponent.Rows.Count; i++)
                    {
                        m_dataComponent.Rows[i].Visible = true;
                    }
                }

                m_dataComponent.Refresh();

                m_dataComponent.ClearSelection();
            }
            finally
            {
                m_bAllowDataBindingComplete = true;
            }
        }

        private void cmbFindItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_startIndex = 0;

            RefreshDataSource();
        }

        private void txtContext_TextChanged(object sender, EventArgs e)
        {
            if (OnlyLocalize)
            {
                m_startIndex = 0;

                LocalizeData();
            }
            else
            {
                if (m_dataTable != null)
                {
                    RefreshDataSource();
                }
                else
                {
                    m_startIndex = 0;
                    LocalizeData();
                }
            }
        }

        /// <summary>
        /// 点击数据定位按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindData_Click(object sender, EventArgs e)
        {
            LocalizeData();
        }

        private void txtContext_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LocalizeData();
            }
        }

        private void btnSetHideFields_Click(object sender, EventArgs e)
        {
            隐藏字段窗体 form = new 隐藏字段窗体(m_formName, m_dataGridViewName, m_arrayField);

            form.ShowDialog();

            if (m_dataTable != null)
            {
                m_dataComponent.DataSource = m_baseDataTable;
            }
            else if (m_dataSource != null)
            {
                m_dataComponent.DataSource = m_dataSource;
            }

            Init(m_dataComponent, m_formName, form.DtReturn);

            RefreshDataSource();
        }
    }
}
