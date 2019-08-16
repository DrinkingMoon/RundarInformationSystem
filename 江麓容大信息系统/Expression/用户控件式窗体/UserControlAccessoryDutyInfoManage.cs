/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UserControlAccessoryDutyInfoManage.cs
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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 零件责任归属管理组件
    /// </summary>
    public partial class UserControlAccessoryDutyInfoManage : Form
    {
        /// <summary>
        /// 权限组件
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 查找到的符合条件的零部件SQE分工明细表
        /// </summary>
        IQueryable<View_B_AccessoryDutyInfo> m_findAccessoryDutyInfo;

        /// <summary>
        /// 供应商服务组件
        /// </summary>
        IAccessoryDutyInfoManageServer m_accessoryDutyInfoManageServer = 
                        ServerModuleFactory.GetServerModule<IAccessoryDutyInfoManageServer>();

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        public UserControlAccessoryDutyInfoManage(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        private void UserControlAccessoryDutyInfoManage_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlAccessoryDutyInfoManage_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(this.toolStrip1, m_authFlag);

            if (!m_accessoryDutyInfoManageServer.GetAllAccessoryDutyInfo(
                BasicInfo.ListRoles, BasicInfo.LoginName, out m_findAccessoryDutyInfo, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView(m_findAccessoryDutyInfo);
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="findAccessoryInfo">数据集合</param>
        void RefreshDataGridView(IQueryable findAccessoryInfo)
        {
            dataGridView1.DataSource = findAccessoryInfo;

            // 添加数据定位控件
            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
                m_dataLocalizer.OnlyLocalize = true;

                panelPara.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
                m_dataLocalizer.Visible = true;
            }

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            dataGridView1.Refresh();
        }

        /// <summary>
        /// 检测是否允许添加/修改供应商
        /// </summary> 
        /// <returns>检测通过返回True，未通过返回False</returns>
        bool CheckDataItem()
        {
            if (txtSort.Text == "")
            {
                MessageDialog.ShowPromptMessage("类别不能为空!");
                return false;
            }

            if (txtCode.Text == "")
            {
                MessageDialog.ShowPromptMessage("图号/型号不能为空!");
                return false;
            }

            if (txtName.Text == "")
            {
                MessageDialog.ShowPromptMessage("物品名称不能为空!");
                return false;
            }

            if (txtProviderA.Text == "")
            {
                MessageDialog.ShowPromptMessage("A级供应商不能为空!");
                return false;
            }

            if (cmbGrade.SelectedItem == null)
            {
                MessageDialog.ShowPromptMessage("难度分级不能为空!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 重置右面板参数
        /// </summary>
        void ClearControl()
        {
            txtSort.Text = "";
            txtCode.Text = "";
            txtName.Text = "";
            txtSpec.Text = "";
            txtProviderA.Text = "";
            txtProviderB.Text = "";
            txtProviderC.Text = "";
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void btnOutExcel_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!m_accessoryDutyInfoManageServer.GetAllAccessoryDutyInfo
                (BasicInfo.ListRoles, BasicInfo.LoginName, out m_findAccessoryDutyInfo, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView(m_findAccessoryDutyInfo);
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

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            ClearControl();

            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            txtSort.Text = dataGridView1.CurrentRow.Cells["类别"].Value.ToString();

            txtName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
            txtCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
            txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();

            txtProviderA.Text = dataGridView1.CurrentRow.Cells["A级供应商"].Value == null ?
                "" : dataGridView1.CurrentRow.Cells["A级供应商"].Value.ToString();
            txtProviderB.Text = dataGridView1.CurrentRow.Cells["B级供应商"].Value == null ?
                "" : dataGridView1.CurrentRow.Cells["B级供应商"].Value.ToString();
            txtProviderC.Text = dataGridView1.CurrentRow.Cells["C级供应商"].Value == null ?
                "" : dataGridView1.CurrentRow.Cells["C级供应商"].Value.ToString();
            cmbGrade.Text = dataGridView1.CurrentRow.Cells["难度等级"].Value.ToString();
            txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value == null ?
                "" : dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
        }
    }
}
