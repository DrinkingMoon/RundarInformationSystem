/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormModificationPassWord.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/15
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 修改密码界面
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
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    public partial class FormModificationPassWord : Form
    {
        /// <summary>
        /// 用户管理
        /// </summary>
        IUserManagement m_userManagement = PlatformFactory.GetUserManagement();

        public FormModificationPassWord()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            string oldPwd = txtOldPwd.Text;
            string newPwd = txtNewPwd.Text;
            string affirmPwd = txtAffirmPwd.Text;

            if (oldPwd == "" || newPwd == "" || affirmPwd == "")
            {
                MessageBox.Show("请填写完整用户信息!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (newPwd != affirmPwd)
            {
                MessageBox.Show("新密码与确认密码不一致,请重新填写!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtNewPwd.Text = "";
                txtAffirmPwd.Text = "";
                return;
            }
            else
            {
                try
                {
                    if (m_userManagement.UpdatePassword(BasicInfo.LoginID, oldPwd, newPwd))
                    {
                        MessageDialog.ShowPromptMessage("成功修改密码，下次登录请用新密码登录！");
                        this.Close();
                    }
                }
                catch (Exception error)
                {
                    MessageDialog.ShowErrorMessage(error.Message);
                }
            }
        }

        /// <summary>
        /// 信息重置
        /// </summary>
        void Reset()
        {
            txtOldPwd.Text = "";
            txtNewPwd.Text = "";
            txtAffirmPwd.Text = "";
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
    }
}