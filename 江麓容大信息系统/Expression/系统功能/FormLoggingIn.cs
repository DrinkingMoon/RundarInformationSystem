/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormLoggingIn.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/15
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 系统登录界面
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
using System.Data.Linq;
using System.Linq;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using GlobalObject;
using AsynSocketService;
using SocketCommDefiniens;
using Service_Peripheral_HR;
using UniversalControlLibrary;
using System.Runtime.InteropServices;

namespace Expression
{
    /// <summary>
    /// 系统登录界面
    /// </summary>
    public partial class FormLoggingIn : Form
    {
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        #region variants

        /// <summary>
        /// 登录系统标志
        /// </summary>
        bool m_loggingFlag = false;

        #endregion variants

        #region properties

        /// <summary>
        /// 登录系统标志
        /// </summary>
        public bool LoggingFlag
        {
            get { return m_loggingFlag; }
            set { m_loggingFlag = value; }
        }

        #endregion properties

        /// <summary>
        /// 构造函数
        /// </summary>
        public FormLoggingIn()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormLoggingIn_Load(object sender, EventArgs e)
        {
            txtUserCode.Text = GlobalParameter.UserCode;
            cmbSystem.Text = GlobalParameter.SystemName.ToString();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbSystem.Text == "")
                {
                    MessageDialog.ShowPromptMessage("请选择【系统平台】");
                    return;
                }

                if (FindWindow(null, cmbSystem.Text + "信息化系统") != IntPtr.Zero)
                {
                    MessageDialog.ShowPromptMessage("您已经运行了一个【"+ cmbSystem.Text +"信息化】程序，无法运行多个");
                    return;
                }

                if (txtUserCode.Text == "" || txtPwd.Text == "")
                {
                    MessageDialog.ShowPromptMessage("请填写完整用户信息!");
                    return;
                }

                GlobalParameter.Init(cmbSystem.Text);
                DBOperateERP.AccessDB.InitDBOperate();

                GlobalObject.DatabaseServer.IsConnection();

                string loginNotice = UniversalFunction.GetSwitchKey()[13];

                string workID = UniversalFunction.GetWorkID(txtUserCode.Text);

                if (AuthenticationManager.IdentifyAuthority(workID, txtPwd.Text))
                {
                    LoggingFlag = true;

                    //if ((loginNotice != null && loginNotice.Length > 0)
                    //    && loginNotice != GlobalParameter.LoginNotice)
                    //{
                    //    登陆界面温馨提示 frm = new 登陆界面温馨提示(loginNotice);
                    //    frm.ShowDialog();
                    //}

                    IAuthentication authentication = AuthenticationManager.Authentication;
                    List<string> listRoles = new List<string>();
                    string[] roleCodes = new string[authentication.Roles.Count];

                    for (int i = 0; i < authentication.Roles.Count; i++)
                    {
                        listRoles.Add(authentication.Roles[i].角色名称);
                        roleCodes[i] = authentication.Roles[i].角色编码;
                    }

                    View_HR_Personnel personnelInfo = UniversalFunction.GetPersonnelInfo(workID);

                    if (personnelInfo == null || personnelInfo.工号 == null)
                    {
                        throw new Exception("【工号】：" + workID + " 不存在,请咨询人力资源");
                    }
                    else if (!personnelInfo.是否在职)
                    {
                        throw new Exception("【工号】：" + workID + " 已【离职】,请咨询人力资源");
                    }

                    BasicInfo.LoginID = personnelInfo.工号;
                    BasicInfo.LoginName = personnelInfo.姓名;
                    BasicInfo.DeptCode = personnelInfo.部门编码;
                    BasicInfo.DeptName = personnelInfo.部门名称;
                    BasicInfo.BaseSwitchInfo = UniversalFunction.GetSwitchKey();

                    GlobalParameter.UserCode = txtUserCode.Text;
                    GlobalParameter.LoginNotice = loginNotice;

                    GlobalParameter.Save();

                    BasicInfo.ListRoles = listRoles;
                    BasicInfo.RoleCodes = roleCodes;

                    if (BasicInfo.ListRoles.Contains("物流公司") 
                        && BasicInfo.ListRoles.Contains("物流公司_库管理员") 
                        && BasicInfo.ListRoles.Count() == 2
                        && cmbSystem.SelectedIndex == 0)
                    {
                        throw new Exception("物流公司，无法登陆【湖南容大】信息化系统");
                    }

                    this.Close();
                }
            }
            catch (Exception err)
            {
                MessageDialog.ShowErrorMessage(err.Message);
                return;
            }
        }

        /// <summary>
        /// 信息重置
        /// </summary>
        void Reset()
        {
            txtUserCode.Text = "";
            txtPwd.Text = "";
            txtUserCode.Focus();
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        /// <summary>
        /// 用户编号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPwd.Focus();
                txtPwd.SelectAll();
            }
        }

        /// <summary>
        /// 用户密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOk.PerformClick();
            }
        }

        private void cmbSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtUserCode.Focus();
            txtUserCode.SelectAll();
        }
    }
}