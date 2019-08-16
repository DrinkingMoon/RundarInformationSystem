/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UserControlProductType.cs
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
using PlatformManagement;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 产品信息组件
    /// </summary>
    public partial class UserControlProductInfo : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 查找到的符合条件的单位信息
        /// </summary>
        IQueryable<View_P_ProductInfo> m_returnProductType;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 服务组件
        /// </summary>
        IProductInfoServer m_productInfoServer = ServerModuleFactory.GetServerModule<IProductInfoServer>();

        public UserControlProductInfo(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authorityFlag = nodeInfo.Authority;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();            
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlProductType_Resize(object sender, EventArgs e)
        {
            panelLeft.Width = (this.Width - panelCenter.Width) / 2;
            panelRight.Width = this.Width - panelCenter.Width - panelLeft.Width;
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlProductType_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authorityFlag);

            if (!m_productInfoServer.GetAllProductInfo(out m_returnProductType, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView();
            RefreshControl();
            txtProductTypeCode.Focus();
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        void RefreshDataGridView()
        {
            dataGridView1.DataSource = m_returnProductType;

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            dataGridView1.Refresh();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        /// <summary>
        /// 根据界面信息生成产品信息对象
        /// </summary>
        /// <returns>返回生成的产品信息对象</returns>
        private P_ProductInfo GenerateProductInfo()
        {
            P_ProductInfo productInfo = new P_ProductInfo();

            productInfo.ProductType = txtProductTypeCode.Text;
            productInfo.ProductCode = txtShortCode.Text;
            productInfo.ProductName = txtProductTypeName.Text;
            productInfo.IsReturn = chkIsReturn.Checked;
            productInfo.Remark = txtRemark.Text;

            return productInfo;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtProductTypeCode.Text == "")
            {
                txtProductTypeCode.Focus();
                MessageDialog.ShowErrorMessage("产品类型编码不允许为空");
                return;
            }

            if (txtShortCode.Text == "")
            {
                txtShortCode.Focus();
                MessageDialog.ShowErrorMessage("产品装配简码不允许为空");
                return;
            }

            if (txtProductTypeName.Text == "")
            {
                txtProductTypeName.Focus();
                MessageDialog.ShowErrorMessage("产品类型名称不允许为空");
                return;
            }

            if (!m_productInfoServer.AddProductInfo(GenerateProductInfo(), out m_returnProductType, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView();
            RefreshControl();
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
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("您是否确定要删除该行信息?") == DialogResult.No)
            {
                return;
            }

            for (int i = 0; i < n; i++)
            {
                int id = Convert.ToInt32(dataGridView1.SelectedRows[i].Cells[0].Value);

                if (!m_productInfoServer.DeleteProductInfo(id, out m_returnProductType, out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                }
            }

            RefreshDataGridView();
            RefreshControl();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择需要修改的数据行!");
                return;
            }
            else if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("系统不允许同时修改多行数据!");
                return;
            }

            if (!m_productInfoServer.UpdateProductInfo(GenerateProductInfo(), out m_returnProductType, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView();
            RefreshControl();
        }

        /// <summary>
        /// 点击DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            RefreshControl();
        }

        /// <summary>
        /// 清空控件值
        /// </summary>
        void ClearControl()
        {
            txtProductTypeCode.Text = "";
            txtShortCode.Text = "";
            txtProductTypeName.Text = "";
            chkIsReturn.Checked = false;
            txtRemark.Text = "";
        }

        /// <summary>
        /// 刷新控件
        /// </summary>
        void RefreshControl()
        {
            ClearControl();

            DataGridViewRow row = null;

            if (dataGridView1.CurrentRow != null)
            {
                row = dataGridView1.CurrentRow;
            }
            else if (dataGridView1.Rows.Count > 0)
            {
                row = dataGridView1.Rows[0];
            }

            if (row != null)
            {
                txtProductTypeCode.Text = row.Cells[1].Value.ToString();
                txtShortCode.Text = row.Cells[2].Value.ToString();
                txtProductTypeName.Text = row.Cells[3].Value.ToString();
                chkIsReturn.Checked = (bool)row.Cells[4].Value;
                txtRemark.Text = row.Cells[5].Value.ToString();
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
    }
}
