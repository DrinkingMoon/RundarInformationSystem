using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniversalControlLibrary;
using ServerModule;

namespace Expression
{
    public partial class Form改变临时档案分总成编号 : Form
    {
        public Form改变临时档案分总成编号()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查数据是否录入正确
        /// </summary>
        /// <returns>正确返回true</returns>
        private bool CheckData()
        {
            if (txtOldNumber.Text.Length < 7)
            {
                MessageDialog.ShowPromptMessage("分总成编号不正确，请重新录入");

                txtOldNumber.SelectAll();
                txtOldNumber.Focus();
                return false;
            }

            if (txtNewNumber.Text.Length < 7)
            {
                MessageDialog.ShowPromptMessage("分总成编号不正确，请重新录入");

                txtNewNumber.SelectAll();
                txtNewNumber.Focus();
                return false;
            }

            if (txtOldNumber.Text.Substring(0, 3) != txtNewNumber.Text.Substring(0, 3))
            {
                MessageDialog.ShowPromptMessage("分总成编号不正确，或者新旧编号不是同一类型，请重新录入或者咨询信息化系统管理员");

                txtNewNumber.SelectAll();
                txtNewNumber.Focus();
                return false;
            }

            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!CheckData())
            {
                return;
            }

            // 临时电子档案服务
            ITempElectronFileServer server = ServerModuleFactory.GetServerModule<ITempElectronFileServer>();
            string error = null;

            if (server.UpdateParentScanCode(txtOldNumber.Text, txtNewNumber.Text, out error))
            {
                MessageDialog.ShowPromptMessage("更新成功");
                this.Close();
            }
            else
            {
                MessageDialog.ShowErrorMessage(error);
            }
        }
    }
}
