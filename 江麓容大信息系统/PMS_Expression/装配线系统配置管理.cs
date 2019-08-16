/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  装配线系统配置管理.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/15
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 供应商信息界面
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

namespace Expression
{
    /// <summary>
    /// 装配线系统配置管理
    /// </summary>
    public partial class 装配线系统配置管理 : Form
    {
        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 配置参数服务
        /// </summary>
        ServerModule.IZPXProductionParams _serviceProductionParams = ServerModule.PMS_ServerFactory.GetServerModule<IZPXProductionParams>();

        public 装配线系统配置管理(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            m_authFlag = nodeInfo.Authority;

        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 装配线系统配置管理_Resize(object sender, EventArgs e)
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
        private void 装配线系统配置管理_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authFlag);
            RefreshControl();
        }

        /// <summary>
        /// 工位管辖划分(装配用途、用途权限分配、工位用途设置)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 工位管辖划分_Click(object sender, EventArgs e)
        {
            工位管辖划分 dialog = new 工位管辖划分();

            dialog.ShowDialog();
        }

        /// <summary>
        /// 点击DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        /// <summary>
        /// 清空控件值
        /// </summary>
        void ClearControl()
        {
        }

        /// <summary>
        /// 刷新控件
        /// </summary>
        void RefreshControl()
        {
            customDataGridView1.DataSource = _serviceProductionParams.GetParamsList();
        }

        private void customDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (customDataGridView1.CurrentRow == null)
            {
                return;
            }

            装配线系统参数配置 frm = new 装配线系统参数配置(Convert.ToInt32(customDataGridView1.CurrentRow.Cells["ID"].Value));
            frm.ShowDialog();
            RefreshControl();
        }
    }
}
