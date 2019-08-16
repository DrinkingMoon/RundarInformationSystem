/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  阀块总成检测数据.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2013/12/09
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 供显示及查询阀块总成检测数据，数据来源从阀块下线试验台
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2013/12/09 16:02:08 作者: 夏石友 当前版本: V1.00
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
using GlobalObject;
using PlatformManagement;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 阀块总成检测数据 : Form
    {
        /// <summary>
        /// 阀块检测数据服务接口
        /// </summary>
        IValveCheckDataService m_valveDataService = PMS_ServerFactory.GetServerModule<IValveCheckDataService>();

        /// <summary>
        /// 授权标志
        /// </summary>
        AuthorityFlag m_authFlag;

        public 阀块总成检测数据(FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            this.dateTimePickerST.Value = DateTime.Now.AddDays(-7).Date;
            this.dateTimePickerET.Value = DateTime.Now.Date;

            btnSearch_Click(null, null);

            this.userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        private void 阀块总成检测数据_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择一条数据后再进行此操作");
                return;
            }

            报表_阀块总成检测数据 report = new 报表_阀块总成检测数据(m_authFlag, (Int64)dataGridView1.SelectedRows[0].Cells["序号"].Value);
            report.ShowDialog();
        }

        /// <summary>
        /// 日期检索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = m_valveDataService.GetData(this.dateTimePickerST.Value, this.dateTimePickerET.Value);
        }

        private void btnSearchProduct_Click(object sender, EventArgs e)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtNumber.Text.Trim()))
            {
                MessageDialog.ShowPromptMessage("请输入阀块编号后再进行此操作");
                return;
            }

            this.dataGridView1.DataSource = m_valveDataService.GetData(this.txtNumber.Text);
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            btnPrint_Click(sender, e);
        }
    }
}
