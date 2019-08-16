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
    public partial class 文件发放回收登记表 : Form
    {
        /// <summary>
        /// 文件基础信息服务组件
        /// </summary>
        ISystemFileBasicInfo m_serverFileBasic = Service_Quality_File.ServerModuleFactory.GetServerModule<ISystemFileBasicInfo>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 人员服务组件
        /// </summary>
        IPersonnelInfoServer m_serverPersonnel = ServerModule.ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        /// <summary>
        /// 部门服务组件
        /// </summary>
        IDepartmentServer m_serverDepartment = ServerModule.ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 文件发放回收登记表服务组件
        /// </summary>
        ISystemFileDistributionOfRecyclingRegisterList m_serverDORRList =
            Service_Quality_File.ServerModuleFactory.GetServerModule<ISystemFileDistributionOfRecyclingRegisterList>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// LNQ数据集
        /// </summary>
        FM_DistributionOfRecyclingRegisterList m_lnqDORRList = new FM_DistributionOfRecyclingRegisterList();

        /// <summary>
        /// 指定人
        /// </summary>
        List<string> m_listPersonnel = new List<string>();

        public 文件发放回收登记表()
        {
            InitializeComponent();
            m_billMessageServer.BillType = CE_BillTypeEnum.文件发放回收登记表.ToString();
            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.文件审查流程.ToString(), m_serverDORRList);
        }

        private void 文件发放回收登记表_Load(object sender, EventArgs e)
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
                if (dataGridView1.Rows[i].Cells["序号"].Value.ToString() == id)
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
            txtGrantDepartment.Text = "";
            txtGrantDepartment.Tag = null;
            txtRecoverDepartment.Text = "";
            txtRecoverDepartment.Tag = null;

            lbGrantPersonnel.Text = "";
            lbGrantTime.Text = "";
            lbRecoverPersonnel.Text = "";
            lbRecoverTime.Text = "";
            lbSignPersonnel.Text = "";
            lbSignTime.Text = "";
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefreshData()
        {
            dataGridView1.DataSource = m_serverDORRList.GetTableInfo();

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
            else if (txtGrantDepartment.Tag == null)
            {
                throw new Exception("请选择发文单位");
            }
            else if (txtRecoverDepartment.Tag == null)
            {
                throw new Exception("请选择收文单位");
            }
        }

        /// <summary>
        /// 获得数据
        /// </summary>
        void GetData()
        {
            m_lnqDORRList.FileID = Convert.ToInt32(txtFileNo.Tag);
            m_lnqDORRList.GrantDepartment = txtGrantDepartment.Tag.ToString();
            m_lnqDORRList.RecoverDepartment = txtRecoverDepartment.Tag.ToString();
        }

        /// <summary>
        /// 重新窗体消息处理函数
        /// </summary>
        /// <param name="m">窗体消息</param>
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                //接收自定义消息,放弃未提交的单据号
                case WndMsgSender.CloseMsg:
                    // 放弃未使用的单据号
                    m_billNoControl.CancelBill();
                    break;

                case WndMsgSender.PositioningMsg:
                    WndMsgData msg = new WndMsgData();      //这是创建自定义信息的结构
                    Type dataType = msg.GetType();

                    msg = (WndMsgData)m.GetLParam(dataType);//这里获取的就是作为LParam参数发送来的信息的结构

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent.Substring(4), CE_BillTypeEnum.文件发放回收登记表.ToString());

                    if (dtMessage.Rows.Count == 0)
                    {
                        m_billMessageServer.DestroyMessage(msg.MessageContent);
                        MessageDialog.ShowPromptMessage("未找到相关记录");
                    }
                    else
                    {
                        dataGridView1.DataSource = dtMessage;
                        dataGridView1.Rows[0].Selected = true;
                    }

                    break;

                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }

        /// <summary>
        /// 操作流程
        /// </summary>
        /// <param name="dorr">操作流程枚举</param>
        void OperatorFlow(SystemFileDORRList dorr)
        {
            try
            {
                m_lnqDORRList.ID = txtFileName.Tag == null ? 0 : Convert.ToInt32(txtFileName.Tag);

                if ((dorr == SystemFileDORRList.修改 || dorr == SystemFileDORRList.删除) 
                    && lbGrantPersonnel.Text != BasicInfo.LoginName)
                {
                    MessageDialog.ShowPromptMessage("不是创建人本人不能操作,请重新核对");
                    return;
                }

                switch (dorr)
                {
                    case SystemFileDORRList.添加:
                        CheckData();
                        GetData();
                        m_serverDORRList.Add(ref m_lnqDORRList);
                        break;
                    case SystemFileDORRList.修改:
                        CheckData();
                        GetData();
                        m_serverDORRList.Update(m_lnqDORRList);
                        break;
                    case SystemFileDORRList.删除:
                        m_serverDORRList.Delete(m_lnqDORRList.ID);
                        break;
                    case SystemFileDORRList.签收:
                        m_serverDORRList.Sign(m_lnqDORRList.ID);
                        break;
                    case SystemFileDORRList.回收:
                        m_serverDORRList.Recover(m_lnqDORRList.ID);
                        break;
                    default:
                        break;
                }

                MessageDialog.ShowPromptMessage(dorr.ToString() + "成功");

                if (dorr == SystemFileDORRList.修改 || dorr == SystemFileDORRList.添加)
                {
                    MessageDialog.ShowPromptMessage("请指定签收人");

                    FormSelectPersonnel2 frm = new FormSelectPersonnel2();

                    if (frm.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    foreach (PersonnelBasicInfo pbi in frm.SelectedNotifyPersonnelInfo.PersonnelBasicInfoList)
                    {
                        if (pbi.工号 != null && pbi.工号.Length > 0)
                        {
                            m_listPersonnel.Add(pbi.工号);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }

            RefreshData();
            PositioningRecord(m_lnqDORRList.ID.ToString());
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OperatorFlow(SystemFileDORRList.添加);

            if (m_listPersonnel != null)
            {
                FM_FileList lnqTemp = m_serverFileBasic.GetFile(m_lnqDORRList.FileID);

                m_billMessageServer.DestroyMessage("DORR" + m_lnqDORRList.ID.ToString());
                m_billMessageServer.SendNewFlowMessage("DORR" + m_lnqDORRList.ID.ToString(),
                    string.Format("{0}文件已发放，请签收", lnqTemp.FileName + "[" + lnqTemp.FileNo + "]"),
                    BillFlowMessage_ReceivedUserType.用户, m_listPersonnel);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            OperatorFlow(SystemFileDORRList.修改);

            if (m_listPersonnel != null)
            {
                FM_FileList lnqTemp = m_serverFileBasic.GetFile(m_lnqDORRList.FileID);

                m_billMessageServer.PassFlowMessage("DORR" + m_lnqDORRList.ID.ToString(),
                    string.Format("{0}文件已发放，请签收", lnqTemp.FileName + "[" + lnqTemp.FileNo + "]"),
                    BillFlowMessage_ReceivedUserType.用户, m_listPersonnel);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            OperatorFlow(SystemFileDORRList.删除);
        }

        private void btnSign_Click(object sender, EventArgs e)
        {
            OperatorFlow(SystemFileDORRList.签收);
            
            FM_FileList lnqTemp = m_serverFileBasic.GetFile(m_lnqDORRList.FileID);
            m_billMessageServer.EndFlowMessage("DORR" + m_lnqDORRList.ID.ToString(),
                string.Format("{0}文件已签收", lnqTemp.FileName + "[" + lnqTemp.FileNo + "]"), null, null);
        }

        private void btnRecover_Click(object sender, EventArgs e)
        {
            OperatorFlow(SystemFileDORRList.回收);
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
            txtFileName.Tag = dataGridView1.CurrentRow.Cells["序号"].Value.ToString();
            txtFileNo.Text = dataGridView1.CurrentRow.Cells["文件编号"].Value.ToString();
            txtFileNo.Tag = dataGridView1.CurrentRow.Cells["文件ID"].Value;
            txtGrantDepartment.Text = dataGridView1.CurrentRow.Cells["发文单位"].Value.ToString();
            txtGrantDepartment.Tag = m_serverDepartment.GetDepartmentCode(dataGridView1.CurrentRow.Cells["发文单位"].Value.ToString());
            txtRecoverDepartment.Text = dataGridView1.CurrentRow.Cells["收文单位"].Value.ToString();
            txtRecoverDepartment.Tag = m_serverDepartment.GetDepartmentCode(dataGridView1.CurrentRow.Cells["收文单位"].Value.ToString());
            txtVersion.Text = dataGridView1.CurrentRow.Cells["版本号"].Value.ToString();

            lbGrantPersonnel.Text = dataGridView1.CurrentRow.Cells["发放人"].Value.ToString();
            lbGrantTime.Text = dataGridView1.CurrentRow.Cells["发放日期"].Value.ToString();
            lbRecoverPersonnel.Text = dataGridView1.CurrentRow.Cells["回收确认人"].Value.ToString();
            lbRecoverTime.Text = dataGridView1.CurrentRow.Cells["回收日期"].Value.ToString();
            lbSignPersonnel.Text = dataGridView1.CurrentRow.Cells["签收人"].Value.ToString();
            lbSignTime.Text = dataGridView1.CurrentRow.Cells["签收日期"].Value.ToString();

            if (lbSignPersonnel.Text.Length != 0
                && lbRecoverPersonnel.Text.Length == 0
                && BasicInfo.DeptCode.Substring(0, 2) == 
                m_serverPersonnel.GetPersonnelInfo(lbGrantPersonnel.Text).部门编码.Substring(0,2))
            {
                btnRecover.Visible = true;
            }
            else
            {
                btnRecover.Visible = false;
            }

            btnSign.Visible = UniversalFunction.IsOperator("DORR" + txtFileName.Tag.ToString());
        }

        private void txtFileNo_OnCompleteSearch()
        {
            txtFileNo.Tag = txtFileNo.DataResult["文件ID"];
            txtFileName.Text = txtFileNo.DataResult["文件名称"].ToString();
            txtVersion.Text = txtFileNo.DataResult["版本号"].ToString();
        }

        private void txtGrantDepartment_OnCompleteSearch()
        {
            txtGrantDepartment.Tag = txtGrantDepartment.DataResult["部门编码"];
        }

        private void txtRecoverDepartment_OnCompleteSearch()
        {
            txtRecoverDepartment.Tag = txtRecoverDepartment.DataResult["部门编码"];
        }

        private void txtFileNo_Enter(object sender, EventArgs e)
        {
            txtFileNo.StrEndSql = " and 类别ID not in (10,11) ";
        }
    }
}
