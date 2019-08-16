/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  零部件入库汇总表.cs
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
    /// 入库汇总表组件
    /// </summary>
    public partial class 零部件入库汇总表 : Form
    {
        /// <summary>
        /// 产品汇总
        /// </summary>
        readonly string[] m_procuctType = new string[] { "按图号汇总", "按名称汇总", "按入库部门汇总" };

        /// <summary>
        /// 零部件汇总
        /// </summary>
        readonly string[] m_accessoryType = new string[] { "按图号汇总", "按名称汇总", "按供应商汇总", "按材料类别汇总" };

        /// <summary>
        /// 查询结果
        /// </summary>
        IQueryable<View_S_InDepotSummaryTable> m_findBill;

        /// <summary>
        /// 入库、出库明细汇总服务组件
        /// </summary>
        IDetailSummaryInfo m_detailSummaryInfo = ServerModuleFactory.GetServerModule<IDetailSummaryInfo>();

        /// <summary>
        /// 零部件入库汇总表
        /// </summary>
        报表_零部件入库汇总 m_reportAccessoryGatherBill;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authorityFlag;

        public 零部件入库汇总表(FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            DataTable dt = UniversalFunction.GetStorageTb();
            cmbStorage.Items.Add("所有库房");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbStorage.Items.Add(dt.Rows[i]["StorageName"].ToString());
            }

            cmbStorage.SelectedIndex = 0;

            dtpEndTime.Value = ServerTime.Time.AddDays(1);

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            
            UpdateStyles();

            m_authorityFlag = nodeInfo.Authority;
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 零部件入库汇总表_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 零部件入库汇总表_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authorityFlag);

            cmbType.SelectedIndex = 0;
            cmbOrder.SelectedIndex = 0;
        }

        /// <summary>
        /// 汇总类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbType.Text == "产品汇总")
            {
                cmbOrder.Items.Clear();
                cmbOrder.Items.AddRange(m_procuctType);
            }
            else //零部件汇总
            {
                cmbOrder.Items.Clear();
                cmbOrder.Items.AddRange(m_accessoryType);
            }

            cmbOrder.SelectedIndex = 0;
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            ExecuteGather();
            RefreshDataGridView();

            if (!toolStrip1.Visible && dataGridView1.Columns.Count > 0)
            {
                for (int i = 5; i < 9; i++)
                {
                    dataGridView1.Columns[i].Visible = false;
                }
            }
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns>查询成功返回True，否则返回False</returns>
        bool ExecuteGather()
        {
            if (!CheckItem())
            {
                return false;
            }

            if (cmbType.Text == "零部件汇总")
            {
                if (cmbOrder.Text == "按图号汇总")
                {
                    m_findBill = m_detailSummaryInfo.GetInDepotSummarInfo(dtpStartTime.Value.Date, dtpEndTime.Value.Date, SummaryMode.图号);
                }
                else if (cmbOrder.Text == "按名称汇总")
                {
                    m_findBill = m_detailSummaryInfo.GetInDepotSummarInfo(dtpStartTime.Value.Date, dtpEndTime.Value.Date, SummaryMode.名称);
                }
                else if (cmbOrder.Text == "按供应商汇总")
                {
                    m_findBill = m_detailSummaryInfo.GetInDepotSummarInfo(dtpStartTime.Value.Date, dtpEndTime.Value.Date, SummaryMode.供应商);
                }
                else if (cmbOrder.Text == "按材料类别汇总")
                {
                    m_findBill = m_detailSummaryInfo.GetInDepotSummarInfo(dtpStartTime.Value.Date, dtpEndTime.Value.Date, SummaryMode.材料类别);
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("抱歉, 暂时未实现此功能！");
            }

            return true;
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefreshDataGridView()
        {
            DataTable dt = new DataTable();

            if (cmbStorage.Text != "所有库房")
            {
                dt = m_detailSummaryInfo.GetInDepotSummarInfoForStorageID(dtpStartTime.Value.Date,
                    dtpEndTime.Value.Date,
                    UniversalFunction.GetStorageID(cmbStorage.Text));
            }
            else
            {
                dt = GlobalObject.GeneralFunction.ConvertToDataTable<View_S_InDepotSummaryTable>(m_findBill);
            }

            dataGridView1.DataSource = dt;
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

        /// <summary>
        /// 检测数据项
        /// </summary>
        /// <returns>通过返回True，否则返回False</returns>
        bool CheckItem()
        {
            if (cmbType.SelectedItem == null)
            {
                MessageDialog.ShowPromptMessage("汇总类型不允许为空!");
                return false;
            }

            if (cmbOrder.SelectedItem == null)
            {
                MessageDialog.ShowPromptMessage("分类汇总不允许为空!");
                return false;
            }
            
            return true;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ExecuteGather())
            {
                if (cmbType.SelectedItem.ToString() == "产品汇总")
                {
                    MessageDialog.ShowPromptMessage("抱歉, 暂时未实现此功能！");
                }
                else if (cmbType.SelectedItem.ToString() == "零部件汇总")
                {
                    m_reportAccessoryGatherBill = new 报表_零部件入库汇总(cmbOrder.SelectedItem.ToString(), dtpStartTime.Value.Date, dtpEndTime.Value.Date);
                    m_reportAccessoryGatherBill.ShowDialog();
                }
            }
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

        private void btnOutExcel_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }
    }
}
