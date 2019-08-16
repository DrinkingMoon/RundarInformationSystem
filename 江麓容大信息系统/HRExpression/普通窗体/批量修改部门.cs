using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Service_Peripheral_HR;
using ServerModule;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    public partial class 批量修改部门 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 部门信息服务组件
        /// </summary>
        IOrganizationServer m_departmentServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IOrganizationServer>();

        /// <summary>
        /// 人员档案管理类
        /// </summary>
        IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        public 批量修改部门()
        {
            InitializeComponent();

            IQueryable<View_HR_Dept> m_findDepartment;

            if (m_departmentServer.GetAllDeptInfo(out m_findDepartment, out m_error))
            {
                foreach (var item in m_findDepartment)
                {
                    cmbOldDept.Items.Add(item.部门名称);
                    cmbNewDept.Items.Add(item.部门名称);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbOldDept.SelectedIndex == -1 || cmbNewDept.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择原部门/科室或现部门/科室！");
                return;
            }

            if (cmbOldDept.Text == cmbNewDept.Text)
            {
                MessageDialog.ShowPromptMessage("两个部门一致，无需修改！");
                return;
            }

            string deptNew = m_departmentServer.GetDeptCode(cmbNewDept.Text);
            string deptOld = m_departmentServer.GetDeptCode(cmbOldDept.Text);

            if (MessageDialog.ShowEnquiryMessage("确定将【" + cmbOldDept.Text + "】换成【" + cmbNewDept.Text + "】吗？") == DialogResult.Yes)
            {
                if (!m_personnerServer.UpdateBatchDept(deptOld, deptNew, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                    return;
                }

                MessageDialog.ShowPromptMessage("批量修改成功！");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
