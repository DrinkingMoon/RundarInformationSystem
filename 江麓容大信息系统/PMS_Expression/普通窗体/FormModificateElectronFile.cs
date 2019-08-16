/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormModificateElectronFile.cs
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
using GlobalObject;
using ServerModule;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 修改电子档案界面
    /// </summary>
    public partial class FormModificateElectronFile : Form
    {
        /// <summary>
        /// 电子档案ID
        /// </summary>
        long m_id;

        public FormModificateElectronFile(long id)
        {
            InitializeComponent();

            m_id = id;
        }

        /// <summary>
        /// 修改电子档案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            txtCheckData.Text = txtCheckData.Text.Trim();
            txtfactData.Text = txtfactData.Text.Trim();
            txtRemark.Text = txtRemark.Text.Trim();

            if (txtCheckData.Text == "" && txtfactData.Text == "" && txtRemark.Text == "")
            {
                MessageDialog.ShowErrorMessage("请录入附加数据后再进行此操作");
                return;
            }

            string error = null;

            if (!ServerModuleFactory.GetServerModule<IElectronFileServer>().ModificateElectronFile(
                m_id, txtCheckData.Text, txtfactData.Text, txtRemark.Text, out error))
            {
                MessageDialog.ShowErrorMessage(error);
                return;
            }

            MessageDialog.ShowPromptMessage("修改成功");
            this.Close();
        }
    }
}
