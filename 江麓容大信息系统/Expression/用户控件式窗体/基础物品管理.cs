/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UserControlPlanCostBill.cs
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
using System.Data.Linq;
using System.Data.Linq.Mapping;
using GlobalObject;
using PlatformManagement;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 零件计划价格组件
    /// </summary>
    public partial class UserControlPlanCostBill : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 查找到的符合条件的客户信息
        /// </summary>
        IQueryable<View_BASE_GoodsPlanCost> m_findGoodsPlanCost;

        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_basicGoodsServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        public UserControlPlanCostBill(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            StapleInfo.InitUnitComboBox(cmbUnit);
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void UserControlPlanCostBill_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlPlanCostBill_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);

            if (!m_basicGoodsServer.GetAllGoodsInfo(out m_findGoodsPlanCost, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView(m_findGoodsPlanCost);
            RefreshControl();
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="findPlanCostBill">计划金额信息</param>
        void RefreshDataGridView(IQueryable<View_BASE_GoodsPlanCost> findPlanCostBill)
        {

            lblAmount.Text = findPlanCostBill.Count().ToString();

            DataTable tempTable = GlobalObject.GeneralFunction.ConvertToDataTable<View_BASE_GoodsPlanCost>(findPlanCostBill);

            if (tempTable.Columns.Contains("单价"))
            {
                tempTable.Columns.Remove("单价");
            }

            if (tempTable.Columns.Contains("单位ID"))
            {
                tempTable.Columns.Remove("单位ID");
            }

            dataGridView1.DataSource = tempTable;
            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            dataGridView1.Refresh(); 

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                 UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
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

        /// <summary>
        /// 检测数据项内容
        /// </summary>
        /// <returns></returns>
        bool CheckDataItem()
        {
            if (txtGoodsType.Text == "")
            {
                txtGoodsType.Focus();
                MessageDialog.ShowPromptMessage("请选择物品类别!");
                return false;
            }

            if (txtName.Text == "")
            {
                MessageDialog.ShowPromptMessage("物品名称不允许为空!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int n = dataGridView1.SelectedRows.Count;

            if (n == 0)
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的数据行!");
            }
            else
            {
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    int id = Convert.ToInt32(dataGridView1.SelectedRows[i].Cells[0].Value);
                    string info = string.Format("您是否确定要删除 [{0}] 物品基础物品信息?", 
                        dataGridView1.SelectedRows[i].Cells[2].Value.ToString());

                    if (MessageDialog.ShowEnquiryMessage(info) == DialogResult.Yes)
                    {
                        if (!m_basicGoodsServer.DeleteGoods(id, out m_findGoodsPlanCost, out m_err))
                        {
                            MessageDialog.ShowPromptMessage(m_err);
                            return;
                        }
                    }
                }
            }

            RefreshDataGridView(m_findGoodsPlanCost);
            RefreshControl();
        }

        /// <summary>
        /// 清空控件值
        /// </summary>
        void ClearControl()
        {
            txtGoodsType.Tag = "";
            txtGoodsType.Text = "";
            txtCode.Text = "";
            txtName.Text = "";
            txtSpec.Text = "";
            txtRemark.Text = "";
            cmbUnit.SelectedIndex = -1;
            chkIsDisable.Checked = false;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            基础物品信息设置界面 frm = new 基础物品信息设置界面(Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value), btnUpdate.Visible);
            frm.ShowDialog();
            btnRefresh_Click(null, null);
            PositioningRecord((int)frm.GoodsID);
        }

        /// <summary>
        /// 点击DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            RefreshControl();
            //UserControl();
            lblCurRowNumber.Text = (e.RowIndex + 1).ToString();
        }

        /// <summary>
        /// 刷新控件
        /// </summary>
        void RefreshControl()
        {
            DataGridViewRow row = null;

            if (dataGridView1.CurrentRow != null)
            {
                row = dataGridView1.CurrentRow;
            }
            else
            {
                return;
            }

            txtCode.Text = row.Cells["图号型号"].Value.ToString();
            txtName.Text = row.Cells["物品名称"].Value.ToString();
            txtSpec.Text = row.Cells["规格"].Value.ToString();
            txtGoodsType.Tag = row.Cells["物品类别"].Value.ToString();
            txtGoodsType.Text = row.Cells["物品类别名称"].Value.ToString();
            chkIsDisable.Checked = Convert.ToBoolean(row.Cells["禁用"].Value);

            cmbUnit.Text = (string)row.Cells["单位"].Value;

            if (row.Cells["备注"].Value != null)
            {
                txtRemark.Text = row.Cells["备注"].Value.ToString();
            }
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        void PositioningRecord(int goodsID)
        {
            string strColName = "";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((int)dataGridView1.Rows[i].Cells["序号"].Value == goodsID)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!m_basicGoodsServer.GetAllGoodsInfo(out m_findGoodsPlanCost, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView(m_findGoodsPlanCost);
            RefreshControl();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            基础物品信息设置界面 frm = new 基础物品信息设置界面(null, true);
            frm.ShowDialog();
            btnRefresh_Click(null, null);

            if (frm.GoodsID != null)
            {
                PositioningRecord((int)frm.GoodsID);
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            btnUpdate_Click(null, null);
        }
    }
}
