﻿/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormAfreshFitting.cs
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
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 重新装配界面类
    /// </summary>
    public partial class FormAfreshFitting : Form
    {
        /// <summary>
        /// 通信组件
        /// </summary>
        CommResponseServer m_commResponseServer;

        public FormAfreshFitting(CommResponseServer commServer)
        {
            InitializeComponent();
            m_commResponseServer = commServer;
        }
        
        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (cmbAfreshFittingFashion.SelectedIndex == 1)
            {
                string msg = "在[零部件重装]方式下,请进入电子档案直接更改零件信息";
                MessageDialog.ShowPromptMessage(msg);
            }
            else if (cmbAfreshFittingFashion.SelectedIndex == 0)
            {
                string msg = "在[总成重装]方式下,请在相应总成工位选择重装模式进行重装";
                MessageDialog.ShowPromptMessage(msg);

                m_commResponseServer.StartServer();
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormAfreshFitting_Load(object sender, EventArgs e)
        {
            cmbAfreshFittingFashion.SelectedIndex = 0;
        }
    }
}
