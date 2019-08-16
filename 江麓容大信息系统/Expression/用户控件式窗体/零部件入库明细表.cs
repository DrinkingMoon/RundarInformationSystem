/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UserControlInDepotBillDetailBill.cs
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
using ServerModule;
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 入库明细表组件
    /// </summary>
    public partial class 零部件入库明细表 : Form
    {
        /// <summary>
        /// 数据集
        /// </summary>
        DataTable m_dtFindBill = new DataTable();

        /// <summary>
        /// 产品汇总
        /// </summary>
        readonly string[] m_procuctType = new string[] { "按图号汇总", "按名称汇总", "按入库部门汇总" };

        /// <summary>
        /// 零部件汇总
        /// </summary>
        readonly string[] m_accessoryType = new string[] { "按日期汇总", "按单据编号汇总", "按图号汇总", "按名称汇总", "按供应商汇总", "按材料类别汇总", "按库房汇总" };

        /// <summary>
        /// 入库、出库明细汇总服务组件
        /// </summary>
        IDetailSummaryInfo m_detailSummaryInfo = ServerModuleFactory.GetServerModule<IDetailSummaryInfo>();

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authorityFlag;

        public 零部件入库明细表(FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            dtpEndTime.Value = ServerTime.Time.AddDays(1);

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            UpdateStyles();

            m_authorityFlag = nodeInfo.Authority;

            cmbStorage.Items.Add("全部库房");

            DataTable dt = UniversalFunction.GetStorageTb();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbStorage.Items.Add(dt.Rows[i]["StorageName"].ToString());
            }

            cmbFDBName.Text = "新账套";
            cmbStorage.Text = "全部库房";
            cmbWhere.SelectedIndex = 0;
        }

        private void 零部件入库明细表_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authorityFlag);
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlInDepotBillDetailBill_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            List<object> lstWhere = new List<object>();

            if (cmbFDBName.Text.Trim().Length == 0
                || cmbStorage.Text.Trim().Length == 0
                || cmbWhere.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("查询信息选择不完整");
                return;
            }

            lstWhere.Add(cmbFDBName.Text);
            lstWhere.Add(UniversalFunction.GetStorageID(cmbStorage.Text));
            lstWhere.Add(cmbWhere.Text);
            lstWhere.Add(dtpStartTime.Value.Date);
            lstWhere.Add(dtpEndTime.Value.Date);

            m_dtFindBill = m_detailSummaryInfo.GetInDepotDetailInfoForTable(lstWhere);

            RefreshDataGridView();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefreshDataGridView()
        {
            dataGridView1.DataSource = m_dtFindBill;

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
            dataGridView1.Refresh();

            if ((m_authorityFlag & AuthorityFlag.StockIn) == AuthorityFlag.StockIn)
            {
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (dataGridView1.Columns[i].Name.Contains("金额") || dataGridView1.Columns[i].Name.Contains("单价"))
                    {
                        dataGridView1.Columns[i].Visible = false;
                    }
                }
            }
        }

        private void btnOutExcel_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dataGridView1.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView1.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
    }
}
