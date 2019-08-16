using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using Expression;
using GlobalObject;
using PlatformManagement;
using Service_Quality_File;
using UniversalControlLibrary;

namespace Form_Quality_File
{
    public partial class 文件销毁记录表 : Form
    {
        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 部门服务组件
        /// </summary>
        IDepartmentServer m_serverDepartment = ServerModule.ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 人员服务组件
        /// </summary>
        IPersonnelInfoServer m_serverPersonnel = ServerModule.ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        /// <summary>
        /// 文件发放回收登记表服务组件
        /// </summary>
        ISystemFileDestroyLogList m_serverDestroyLog =
            Service_Quality_File.ServerModuleFactory.GetServerModule<ISystemFileDestroyLogList>();

        /// <summary>
        /// LNQ数据集
        /// </summary>
        FM_DestroyLogList m_lnqDestroyLog = new FM_DestroyLogList();

        public 文件销毁记录表()
        {
            InitializeComponent();
        }

        private void 文件销毁记录表_Load(object sender, EventArgs e)
        {
            ClearData();
            RefreshData();
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的信息</param>
        void PositioningRecord(string id)
        {
            string strColName = "";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["文件ID"].Value.ToString() == id)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        void ClearData()
        {
            txtFileNo.Text = "";
            txtFileNo.Tag = null;
            txtFileName.Text = "";
            txtVersion.Text = "";
            txtCoverFile.Text = "";
            numCopies.Value = 0;
            txtDestroyWay.Text = "";

            lbProposer.Text = "";
            lbProposerTime.Text = "";
            lbApprover.Text = "";
            lbApproverTime.Text = "";
            lbDestroyPersonnel.Text = "";
            lbDestroyTime.Text = "";
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefreshData()
        {
            dataGridView1.DataSource = m_serverDestroyLog.GetTableInfo();

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        /// <summary>
        /// 检测数据
        /// </summary>
        /// <returns></returns>
        void CheckData()
        {
            if (txtFileNo.Tag == null)
            {
                throw new Exception("请选择文件");
            }
        }

        /// <summary>
        /// 获得数据
        /// </summary>
        void GetData()
        {
            m_lnqDestroyLog.Copies = Convert.ToInt32( numCopies.Value);
            m_lnqDestroyLog.CoverFile = txtCoverFile.Text;
            m_lnqDestroyLog.DestroyWay = txtDestroyWay.Text;
        }

        /// <summary>
        /// 操作流程
        /// </summary>
        /// <param name="dorr">操作流程枚举</param>
        void OperatorFlow(SystemFileDestroyLog enumDestroyLog)
        {
            try
            {
                if (txtFileNo.Text.Trim().Length == 0)
                {
                    return;
                }

                m_lnqDestroyLog.FileID = Convert.ToInt32(txtFileNo.Tag);

                if ((enumDestroyLog == SystemFileDestroyLog.修改 || enumDestroyLog == SystemFileDestroyLog.删除) 
                    && lbProposer.Text != BasicInfo.LoginName)
                {
                    MessageDialog.ShowPromptMessage("不是创建人本人不能操作,请重新核对");
                    return;
                }

                switch (enumDestroyLog)
                {
                    case SystemFileDestroyLog.添加:
                        CheckData();
                        GetData();
                        m_serverDestroyLog.Add(m_lnqDestroyLog);
                        break;
                    case SystemFileDestroyLog.修改:
                        CheckData();
                        GetData();
                        m_serverDestroyLog.Update(m_lnqDestroyLog);
                        break;
                    case SystemFileDestroyLog.删除:
                        m_serverDestroyLog.Delete(m_lnqDestroyLog.FileID);
                        break;
                    case SystemFileDestroyLog.批准:
                        m_serverDestroyLog.Approve(m_lnqDestroyLog.FileID);
                        break;
                    case SystemFileDestroyLog.销毁:
                        m_serverDestroyLog.Destroy(m_lnqDestroyLog.FileID);
                        break;
                    default:
                        break;
                }

                MessageDialog.ShowPromptMessage(enumDestroyLog.ToString() + "成功");
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }

            RefreshData();
            PositioningRecord(m_lnqDestroyLog.FileID.ToString());
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OperatorFlow(SystemFileDestroyLog.添加);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            OperatorFlow(SystemFileDestroyLog.修改);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            OperatorFlow(SystemFileDestroyLog.删除);
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            OperatorFlow(SystemFileDestroyLog.批准);
        }

        private void btnDestroy_Click(object sender, EventArgs e)
        {
            OperatorFlow(SystemFileDestroyLog.销毁);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            txtFileName.Text = dataGridView1.CurrentRow.Cells["文件名称"].Value.ToString();
            txtFileNo.Text = dataGridView1.CurrentRow.Cells["文件编号"].Value.ToString();
            txtFileNo.Tag = dataGridView1.CurrentRow.Cells["文件ID"].Value;
            txtVersion.Text = dataGridView1.CurrentRow.Cells["版本号"].Value.ToString();
            txtDestroyWay.Text = dataGridView1.CurrentRow.Cells["销毁办法"].Value.ToString();
            txtCoverFile.Text = dataGridView1.CurrentRow.Cells["文件载体"].Value.ToString();
            numCopies.Value = Convert.ToDecimal( dataGridView1.CurrentRow.Cells["份数"].Value);

            lbProposer.Text = dataGridView1.CurrentRow.Cells["申请人"].Value.ToString();
            lbProposerTime.Text = dataGridView1.CurrentRow.Cells["申请日期"].Value.ToString();
            lbApprover.Text = dataGridView1.CurrentRow.Cells["批准人"].Value.ToString();
            lbApproverTime.Text = dataGridView1.CurrentRow.Cells["批准日期"].Value.ToString();
            lbDestroyPersonnel.Text = dataGridView1.CurrentRow.Cells["销毁人"].Value.ToString();
            lbDestroyTime.Text = dataGridView1.CurrentRow.Cells["销毁日期"].Value.ToString();

            if (BasicInfo.ListRoles.Contains(
                m_billMessageServer.GetDeptDirectorRoleName(
                m_serverPersonnel.GetPersonnelInfo(lbProposer.Text).部门编码)[0]))
            {
                btnApprove.Visible = true;
            }
            else
            {
                btnApprove.Visible = false;
            }

            if (lbApprover.Text.Trim().Length != 0 && lbDestroyPersonnel.Text.Trim().Length == 0 )
            {
                btnDestroy.Visible = true;
            }
            else
            {
                btnDestroy.Visible = false;
            }
        }

        private void txtFileNo_OnCompleteSearch()
        {
            txtFileNo.Tag = txtFileNo.DataResult["文件ID"];
            txtFileName.Text = txtFileNo.DataResult["文件名称"].ToString();
            txtVersion.Text = txtFileNo.DataResult["版本号"].ToString();
        }

        private void txtFileNo_Enter(object sender, EventArgs e)
        {
            txtFileNo.StrEndSql = " and 类别ID in (11)";
        }
    }
}
