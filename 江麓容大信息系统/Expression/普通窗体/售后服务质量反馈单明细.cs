using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using WebServerModule2;
using Service_Peripheral_HR;
using Service_Peripheral_External;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 售后服务质量反馈明细界面
    /// </summary>
    public partial class 售后服务质量反馈单明细 : BasicFormTool
    {
        #region 声明变量
        /// <summary>
        /// 部门服务组件
        /// </summary>
        IDepartmentServer m_serverDepartment = ServerModule.ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 调运单服务类
        /// </summary>
        IManeuverServer m_maneuverServer = Service_Peripheral_External.ServerModuleFactory.GetServerModule<IManeuverServer>();

        /// <summary>
        /// 调运单实体
        /// </summary>
        Out_ManeuverBill maneuverBillInfo = new Out_ManeuverBill();

        /// <summary>
        /// 库存服务
        /// </summary>
        IStoreServer m_storeServer = ServerModule.ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 服务类
        /// </summary>
        IServiceFeedBack2 m_serverFeedBack = WebServerModule2.ServerModuleFactory2.GetServerModule<IServiceFeedBack2>();

        /// <summary>
        /// 反馈单的实体
        /// </summary>
        S_ServiceFeedBack m_lnqServerFeedBack = new S_ServiceFeedBack();

        /// <summary>
        /// 部门信息服务组件
        /// </summary>
        IOrganizationServer m_departmentServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IOrganizationServer>();

        /// <summary>
        /// 反馈单号
        /// </summary>
        public string m_strFkBillID = null;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 人员信息服务
        /// </summary>
        IPersonnelInfoServer m_personnelInfoServer = ServerModule.ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        /// <summary>
        /// 权限组件
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 营销产品服务组件
        /// </summary>
        IProductListServer m_serverProductList = ServerModule.ServerModuleFactory.GetServerModule<IProductListServer>();

        /// <summary>
        /// 整车的主观故障
        /// </summary>
        string m_strCarMainBug;

        /// <summary>
        /// 整车客观故障
        /// </summary>
        string m_strCarSecondBug;

        /// <summary>
        /// CVT信息
        /// </summary>
        DataRow m_drCVTInfo;

        /// <summary>
        /// 基础物品服务
        /// </summary>
        IBasicGoodsServer m_basicGoodsServer = ServerModule.ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 初始化行
        /// </summary>
        DataRow m_drInit;

        /// <summary>
        /// 初始化dtMxCK
        /// </summary>
        DataTable m_dtMxCK = new DataTable();

        /// <summary>
        /// 初始化dtManeuverList
        /// </summary>
        DataTable dtManeuverList = new DataTable();

        /// <summary>
        /// 初始化dtProductCodes
        /// </summary>
        DataTable m_dtProductCodes = new DataTable();

        /// <summary>
        /// 关联单号
        /// </summary>
        string m_strServiceBill = "";

        /// <summary>
        /// 单据消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgator = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 人员档案服务组件
        /// </summary>
        IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        /// <summary>
        /// 单据编号服务
        /// </summary>
        IAssignBillNoServer m_serverBillNo = ServerModule.ServerModuleFactory.GetServerModule<IAssignBillNoServer>();

        /// <summary>
        /// 领料退库单
        /// </summary>
        S_MaterialReturnedInTheDepot m_inTheDepotBill = new S_MaterialReturnedInTheDepot();

        /// <summary>
        /// 领料退库明细
        /// </summary>
        S_MaterialListReturnedInTheDepot m_inTheDepotGoods = new S_MaterialListReturnedInTheDepot();

        /// <summary>
        /// 领料单
        /// </summary>
        S_MaterialRequisition m_requisitionBill = new S_MaterialRequisition();

        /// <summary>
        /// 领料明细
        /// </summary>
        S_MaterialRequisitionGoods m_lnqGoods = new S_MaterialRequisitionGoods();

        /// <summary>
        /// 是否从售后发出
        /// </summary>
        bool m_isServiceStock = false;

        #endregion

        List<Out_ManeuverBill> listManeuverBill = new List<Out_ManeuverBill>();

        public 售后服务质量反馈单明细(AuthorityFlag m_authFlag1, string strDJH, string serviceID)
        {
            InitializeComponent();

            m_msgPromulgator.BillType = "售后服务质量反馈单";
            m_authFlag = m_authFlag1;
        }

        public 售后服务质量反馈单明细(AuthorityFlag m_authFlag1)
        {
            InitializeComponent();
            m_authFlag = m_authFlag1;
            m_msgPromulgator.BillType = "售后服务质量反馈单";
        }

        public override void LoadFormInfo()
        {
            if (this.BusinessView_Row != null)
            {
                base.LoadFormInfo();
                m_strFkBillID = this.BusinessView_Row.Cells["反馈单号"].Value.ToString();
                m_strServiceBill = this.BusinessView_Row.Cells["关联号"].Value.ToString();

                FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);
                FaceAuthoritySetting.SetVisibly(this.toolStrip1, m_authFlag);

                toolStrip1.Visible = true;

                if (m_strFkBillID.Contains("SHFK"))
                {
                    GetMessage();
                    BindControl();
                    ControlNotUsable();

                    if ((txtStatus.Text.Equals("等待确认返回时间") && BasicInfo.LoginID == "0025") || BasicInfo.ListRoles.Contains(CE_RoleEnum.业务系统管理员.ToString()))
                    {
                        btnUpdateReplace.Visible = true;
                        cbIsBack.Enabled = true;
                    }

                    if (txtStatus.Text.Equals("等待主管审核"))
                    {
                        txtYXChargeSuggestion.ReadOnly = false;
                        txtYXChargeSuggestion.Enabled = true;
                        cbIsBack.Enabled = true;
                        txtYXSignature.Text = BasicInfo.LoginName;
                        txtYXSignature.Tag = BasicInfo.LoginID;
                    }

                    if (txtStatus.Text.Equals("等待责任部门确认"))
                    {
                        txtDutyDeptCharge.Text = BasicInfo.LoginName;
                        txtDutyDeptCharge.Tag = BasicInfo.LoginID;
                    }

                    if (txtStatus.Text.Equals("等待质管检查"))
                    {
                        txtZGCheckName.Text = BasicInfo.LoginName;
                        txtZGCheckName.Tag = BasicInfo.LoginID;
                    }

                    if (txtProcessName.Text == BasicInfo.LoginName && !txtStatus.Text.Equals("单据完成"))
                    {
                        修改toolStripButton7.Enabled = true;
                    }

                    txtBackID.Enabled = true;
                    GetMessageLoad();
                    txtFeedBackID.ForeColor = Color.Red;

                    DataTable replaceDt = m_serverFeedBack.GetReplace(txtServiceID.Text, out m_strErr);

                    dataGridView1.DataSource = replaceDt;

                    if (dataGridView1.Rows.Count > 0)
                    {
                        rbAccesoryBack.Checked = true;
                    }

                    if (txtServiceID.Text != "")
                    {
                        DataTable dtServier = m_serverFeedBack.GetAfterServiceByBillID(txtServiceID.Text);

                        txtDiagnoseSituation.Text = dtServier.Rows[0]["诊断及测试情况"].ToString();
                        m_isServiceStock = Convert.ToBoolean(dtServier.Rows[0]["是否由售后库房发出"]);

                        if (Convert.ToBoolean(dtServier.Rows[0]["是否CVT故障"]))
                        {
                            cmbCVTBug.Text = "是";
                        }
                        else
                        {
                            cmbCVTBug.Text = "否";
                        }
                    }
                }
                else
                {
                    txtStatus.Text = "新建单据";
                    txtStatus.ReadOnly = true;
                    txtFeedBackID.ReadOnly = true;
                    txtBackID.ReadOnly = true;
                    txtProcessName.Text = BasicInfo.LoginName;
                    txtProcessName.Tag = BasicInfo.LoginID;

                    GetMessage();

                    txtFeedBackID.Text = m_serverFeedBack.GetNextBillID(2);
                    txtFeedBackID.ForeColor = Color.Red;
                    txtBackID.Text = "系统自动生成";
                    txtBackID.ForeColor = Color.Red;

                    cmbStatus.SelectedIndex = 0;
                    cbFrequency.SelectedIndex = 0;

                    if (txtServiceID.Text != "")
                    {
                        DataTable dtServier = m_serverFeedBack.GetAfterServiceByBillID(txtServiceID.Text);

                        cmbMessageSource.Text = dtServier.Rows[0]["信息来源"].ToString();
                        txtUserName.Text = dtServier.Rows[0]["用户姓名"].ToString();
                        txtCVTCode.Text = dtServier.Rows[0]["变速箱型号"].ToString();
                        txtCVTID.Text = dtServier.Rows[0]["变速箱编号"].ToString();
                        txtDiagnoseSituation.Text = dtServier.Rows[0]["诊断及测试情况"].ToString();

                        if (dtServier.Rows[0]["新三包"] != null)
                        {
                            cbSanBao.Checked = dtServier.Rows[0]["新三包"].ToString() == "是" ? true : false;
                        }
                        else
                        {
                            cbSanBao.Checked = false;
                        }

                        if (dtServier.Rows[0]["行驶里程"].ToString() == "")
                        {
                            txtRunMileage.Value = 0;
                        }
                        else
                            txtRunMileage.Value = Convert.ToDecimal(dtServier.Rows[0]["行驶里程"].ToString());

                        txtSiteName.Text = dtServier.Rows[0]["服务站名称"].ToString();
                        txtSolution.Text = "处理方案:" + dtServier.Rows[0]["处理方案"].ToString() + "  处理结果：" + dtServier.Rows[0]["处理结果"].ToString();
                        txtCarModel.Text = dtServier.Rows[0]["车型"].ToString();

                        if (dtServier.Rows[0]["购车时间"].ToString() == "")
                        {
                            dtpBuyCarTime.Value = Convert.ToDateTime(ServerTime.Time.ToShortDateString());
                        }
                        else
                        {
                            dtpBuyCarTime.Value = Convert.ToDateTime(dtServier.Rows[0]["购车时间"].ToString());
                        }

                        txtLinkTel.Text = dtServier.Rows[0]["用户电话"].ToString();
                        txtChassisNum.Text = dtServier.Rows[0]["车架号"].ToString();

                        DataTable replaceDt = m_serverFeedBack.GetReplace(txtServiceID.Text, out m_strErr);

                        dataGridView1.DataSource = replaceDt;

                        if (dataGridView1.Rows.Count > 0)
                        {
                            rbAccesoryBack.Checked = true;
                        }

                        GetMessageLoad();
                    }
                }
            }
            else
            {
                FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);
                FaceAuthoritySetting.SetVisibly(this.toolStrip1, m_authFlag);

                toolStrip1.Visible = true;

                ControlUsable();

                txtStatus.Text = "新建单据";
                txtStatus.ReadOnly = true;
                txtFeedBackID.ReadOnly = true;
                txtBackID.ReadOnly = true;
                txtProcessName.Text = BasicInfo.LoginName;
                txtProcessName.Tag = BasicInfo.LoginID;

                GetMessage();

                txtFeedBackID.Text = m_serverFeedBack.GetNextBillID(2);

                txtFeedBackID.ForeColor = Color.Red;
                txtBackID.Text = "系统自动生成";
                txtBackID.ForeColor = Color.Red;

                cmbStatus.SelectedIndex = 0;
                cbFrequency.SelectedIndex = 0;
            }
        }

        private void 售后服务质量反馈单明细_Load(object sender, EventArgs e)
        {
            LoadFormInfo();
        }

        /// <summary>
        /// 重新窗体消息处理函数
        /// </summary>
        /// <param name="m">窗体消息</param>
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WndMsgSender.PositioningMsg:
                    WndMsgData msg = new WndMsgData();      //这是创建自定义信息的结构
                    Type dataType = msg.GetType();

                    msg = (WndMsgData)m.GetLParam(dataType);//这里获取的就是作为LParam参数发送来的信息的结构

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "售后服务质量反馈单");

                    if (dtMessage.Rows.Count == 0)
                    {
                        m_msgPromulgator.DestroyMessage(msg.MessageContent);
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
        /// 获得信息来源
        /// </summary>
        private void GetMessage()
        {
            DataTable MessageDt = m_serverFeedBack.GetMessageSource();

            for (int i = 0; i < MessageDt.Rows.Count; i++)
            {
                cmbMessageSource.Items.Add(MessageDt.Rows[i]["来源"].ToString());
            }

            txtBackID.Text = "系统自动生成";
            txtBackID.ForeColor = Color.Red;
        }

        /// <summary>
        /// 绑定控件
        /// </summary>
        private void BindControl()
        {
            DataTable dt = m_serverFeedBack.GetTableByBill(m_strFkBillID, out m_strErr);

            if (dt.Rows.Count > 0)
            {
                txtFeedBackID.Text = dt.Rows[0]["FK_Bill_ID"].ToString();
                txtFeedBackID.ForeColor = Color.Red;
                txtAnalyse.Text = dt.Rows[0]["Analyse"].ToString();

                if (dt.Rows[0]["AppearCount"].ToString() == "")
                {
                    txtAppearCount.Value = 0;
                }
                else
                {
                    txtAppearCount.Value = Convert.ToDecimal(dt.Rows[0]["AppearCount"].ToString());
                }

                if (dt.Rows[0]["BatchNumber"].ToString() == "")
                {
                    txtBatchNumber.Value = 0;
                    txtBatchNumber.ReadOnly = true;
                }
                else
                    txtBatchNumber.Value = Convert.ToDecimal(dt.Rows[0]["BatchNumber"].ToString());

                if (dt.Rows[0]["BugNumber"].ToString() == "")
                {
                    txtBugCount.Value = 0;
                    txtBugCount.ReadOnly = true;
                }
                else
                {
                    txtBugCount.Value = Convert.ToDecimal(dt.Rows[0]["BugNumber"].ToString());
                }

                if (dt.Rows[0]["BugCode"].ToString() != "否")
                {
                    txtBackID.Text = dt.Rows[0]["BugCode"].ToString();
                    cbIsBack.Checked = true;
                }
                else
                {
                    cbIsBack.Checked = false;
                    txtBackID.Visible = false;
                    label25.Visible = false;
                }

                txtCarModel.Text = dt.Rows[0]["CarModel"].ToString();
                txtChassisNum.Text = dt.Rows[0]["ChassisNum"].ToString();
                txtCVTCode.Text = dt.Rows[0]["CVTCode"].ToString();
                txtCVTID.Text = dt.Rows[0]["CVTID"].ToString();
                txtCVTStatus.Text = dt.Rows[0]["CVTCondition"].ToString();
                txtDutyDept.Tag = dt.Rows[0]["DutyDept"].ToString();

                txtSameBill.Text = dt.Rows[0]["SameBillNo"].ToString();

                if (txtDutyDept.Tag.ToString() != "")
                {
                    txtDutyDept.Text = m_departmentServer.GetDeptName(txtDutyDept.Tag.ToString());
                }

                txtDutyDeptCharge.Tag = dt.Rows[0]["DutyDeptCharge"].ToString();

                if (txtDutyDeptCharge.Tag.ToString() != "")
                {
                    txtDutyDeptCharge.Text = m_personnerServer.GetPersonnelInfo(txtDutyDeptCharge.Tag.ToString()).Name;
                }

                txtDutyPerson.Tag = dt.Rows[0]["DutyPerson"].ToString();

                if (txtDutyPerson.Tag.ToString() != "")
                {
                    txtDutyPerson.Text = m_personnerServer.GetPersonnelInfo(txtDutyPerson.Tag.ToString()).Name;
                }

                txtFinishClaim.Text = dt.Rows[0]["FinishClaim"].ToString();
                txtforeverImplement.Text = dt.Rows[0]["foreverImplement"].ToString();
                txtLinkTel.Text = dt.Rows[0]["LinkTel"].ToString();
                txtNewSoftware.Text = dt.Rows[0]["NewSoftware"].ToString();
                txtPracticable.Text = dt.Rows[0]["Practicable"].ToString();

                if (dt.Rows[0]["IsBack"] != null || dt.Rows[0]["IsBack"].ToString() != "")
                {
                    cmbTKFS.Text = dt.Rows[0]["IsBack"].ToString();
                }

                if (dt.Rows[0]["ProcessMode"].ToString().Equals("已处理"))
                {
                    cmbStatus.SelectedIndex = 1;
                }
                else
                {
                    cmbStatus.SelectedIndex = 0;
                }

                cmbStatus.Enabled = false;

                if (dt.Rows[0]["RunMileage"].ToString() == "")
                {
                    txtRunMileage.Value = 0;
                }
                else
                    txtRunMileage.Value = Convert.ToDecimal(dt.Rows[0]["RunMileage"].ToString());

                txtProcessName.Tag = dt.Rows[0]["ProcessName"].ToString();

                if (txtProcessName.Tag.ToString() != "")
                {
                    txtProcessName.Text = m_personnerServer.GetPersonnelInfo(txtProcessName.Tag.ToString()).Name;
                }

                txtServiceID.Text = dt.Rows[0]["ServiceID"].ToString();
                txtSiteName.Text = m_serverFeedBack.GetClient(dt.Rows[0]["SiteName"].ToString())["ClientName"].ToString();
                txtSiteName.Tag = dt.Rows[0]["SiteName"].ToString();
                txtSolution.Text = dt.Rows[0]["Solution"].ToString();
                txtStatus.Text = dt.Rows[0]["Status"].ToString();
                txtStockSuggestion.Text = dt.Rows[0]["StockSuggestion"].ToString();
                txtTCUCode.Text = dt.Rows[0]["TCUCode"].ToString();
                txtTemporary.Text = dt.Rows[0]["Temporary"].ToString();
                txtUserName.Text = dt.Rows[0]["UserName"].ToString();
                txtYXChargeSuggestion.Text = dt.Rows[0]["YXChargeSuggestion"].ToString();
                txtZGChargeSuggestion.Text = dt.Rows[0]["ZGChargeSuggestion"].ToString();

                txtZGCheckName.Tag = dt.Rows[0]["ZGCheckName"].ToString();

                if (txtZGCheckName.Tag.ToString() != "")
                {
                    txtZGCheckName.Text = m_personnerServer.GetPersonnelInfo(txtZGCheckName.Tag.ToString()).Name;
                }

                txtSignature.Tag = dt.Rows[0]["Signature"].ToString();

                if (txtSignature.Tag.ToString() != "")
                {
                    txtSignature.Text = m_personnerServer.GetPersonnelInfo(txtSignature.Tag.ToString()).Name;
                }

                txtYXSignature.Tag = dt.Rows[0]["YXSignature"].ToString();

                if (txtYXSignature.Tag.ToString() != "")
                {
                    txtYXSignature.Text = m_personnerServer.GetPersonnelInfo(txtYXSignature.Tag.ToString()).Name;
                }

                txtDutyPersonName.Tag = txtDutyPerson.Tag;
                txtDutyPersonName.Text = txtDutyPerson.Text;

                if (dt.Rows[0]["DutyPersonDate"].ToString() == "")
                {
                    dtpDutyPersonDate.Value = ServerTime.Time;
                }
                else
                {
                    dtpDutyPersonDate.Value = Convert.ToDateTime(dt.Rows[0]["DutyPersonDate"].ToString());
                }

                if (dt.Rows[0]["DutyDeptChargeDate"].ToString() == "")
                {
                    dtpDutyDeptChargeDate.Value = ServerTime.Time;
                }
                else
                {
                    dtpDutyDeptChargeDate.Value = Convert.ToDateTime(dt.Rows[0]["DutyDeptChargeDate"].ToString());
                }

                if (dt.Rows[0]["YXSignatureDate"].ToString() == "")
                {
                    dtpYXSignatureDate.Value = ServerTime.Time;
                }
                else
                {
                    dtpYXSignatureDate.Value = Convert.ToDateTime(dt.Rows[0]["YXSignatureDate"].ToString());
                }

                if (dt.Rows[0]["SignatureDate"].ToString() == "")
                {
                    dtpSignatureDate.Value = ServerTime.Time;
                }
                else
                {
                    dtpSignatureDate.Value = Convert.ToDateTime(dt.Rows[0]["SignatureDate"].ToString());
                }

                if (dt.Rows[0]["BuyCarTime"].ToString() == "")
                {
                    dtpBuyCarTime.Value = Convert.ToDateTime(ServerTime.Time.ToShortDateString());
                }
                else
                {
                    dtpBuyCarTime.Value = Convert.ToDateTime(dt.Rows[0]["BuyCarTime"].ToString());
                }

                if (dt.Rows[0]["ProcessTime"].ToString() == "" || dt.Rows[0]["ProcessTime"].ToString() == null)
                {
                    dtpProcessTime.Value = ServerTime.Time;
                }
                else
                {
                    dtpProcessTime.Value = Convert.ToDateTime(dt.Rows[0]["ProcessTime"].ToString());
                }

                if (dt.Rows[0]["ZGCheckDate"].ToString() == "" || dt.Rows[0]["ZGCheckDate"].ToString() == null)
                {
                    dtpZGCheckDate.Value = ServerTime.Time;
                }
                else
                {
                    dtpZGCheckDate.Value = Convert.ToDateTime(dt.Rows[0]["ProcessTime"].ToString());
                }

                if (dt.Rows[0]["ReplyTime"].ToString() == "" || dt.Rows[0]["ReplyTime"].ToString() == null)
                {
                    dtpReplyTime.Value = ServerTime.Time;
                }
                else
                {
                    dtpReplyTime.Value = Convert.ToDateTime(dt.Rows[0]["ReplyTime"].ToString());
                }

                cmbMessageSource.Text = dt.Rows[0]["MessageSource"].ToString();

                if (dt.Rows[0]["IsOpen"].ToString() == "否" || dt.Rows[0]["IsOpen"].ToString() == "")
                {
                    cmbIsOpen.SelectedIndex = 0;
                }
                else
                {
                    cmbIsOpen.SelectedIndex = 1;
                }

                if (dt.Rows[0]["IsFMEAfile"].ToString() == "否" || dt.Rows[0]["IsFMEAfile"].ToString() == "")
                {
                    cmbIsFMEA.SelectedIndex = 0;
                }
                else
                {
                    cmbIsFMEA.SelectedIndex = 1;
                }

                if (dt.Rows[0]["IsClose"].ToString() == "否" || dt.Rows[0]["IsClose"].ToString() == "")
                {
                    cmbIsClose.SelectedIndex = 0;
                }
                else
                {
                    cmbIsClose.SelectedIndex = 1;
                }

                if (dt.Rows[0]["IsBack"].ToString().Equals("有"))
                {
                    rbAccesoryBack.Checked = true;

                    DataTable replaceDt;

                    if (txtServiceID.Text != "" || txtServiceID.Text != null)
                    {
                        replaceDt = m_serverFeedBack.GetReplace(txtServiceID.Text, out m_strErr);
                    }
                    else
                    {
                        replaceDt = m_serverFeedBack.GetReplace(txtFeedBackID.Text, out m_strErr);
                    }

                    if (replaceDt.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = replaceDt;
                    }
                }
                else
                {
                    rbAccesoryNotBack.Checked = true;
                }
            }
        }

        /// <summary>
        /// 是否返回零固件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbAccesoryBack_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text.Equals("等待确认返回时间") || txtStatus.Text.Equals("新建单据"))
            {
                if (rbAccesoryBack.Checked)
                {
                    if (txtServiceID.Text.Trim() != "")
                    {
                        FormReplaceAccessory frm = new FormReplaceAccessory(txtServiceID.Text);
                        frm.ShowDialog();

                        if (frm.BlIsUpdate)
                        {
                            //if (frm.OldGoodsID != 0)
                            //{
                            //    lbCvtID.Text = frm.OldGoodsID.ToString();
                            //}

                            if (frm.Flag)
                            {
                                if (frm.NewGoodsID != "")
                                {
                                    txtCVTID.Text = frm.NewGoodsID;
                                    txtCVTCode.Text = frm.NewGoodsCode1;
                                }
                            }

                            txtStatus.Text = "等待主管审核";

                            GetControlValue();

                            DataTable ReplaceDt;

                            if (txtServiceID.Text != "" || txtServiceID.Text != null)
                            {
                                ReplaceDt = m_serverFeedBack.GetReplace(txtServiceID.Text, out m_strErr);
                            }
                            else
                            {
                                ReplaceDt = m_serverFeedBack.GetReplace(txtFeedBackID.Text, out m_strErr);
                            }

                            if (ReplaceDt.Rows.Count > 0)
                            {
                                dataGridView1.DataSource = ReplaceDt;
                            }
                        }
                    }
                    else
                    {
                        FormReplaceAccessory frm = new FormReplaceAccessory(txtFeedBackID.Text);
                        frm.ShowDialog();

                        if (frm.BlIsUpdate)
                        {
                            txtStatus.Text = "等待主管审核";

                            GetControlValue();

                            DataTable ReplaceDt;

                            if (txtServiceID.Text != "" || txtServiceID.Text != null)
                            {
                                ReplaceDt = m_serverFeedBack.GetReplace(txtServiceID.Text, out m_strErr);
                            }
                            else
                            {
                                ReplaceDt = m_serverFeedBack.GetReplace(txtFeedBackID.Text, out m_strErr);
                            }

                            if (ReplaceDt.Rows.Count > 0)
                            {
                                dataGridView1.DataSource = ReplaceDt;
                            }
                        }
                    }

                    if (txtStatus.Text.Equals("新建单据"))
                    {
                        bool isbasic = m_serverFeedBack.InsertFeedBack(m_lnqServerFeedBack, GetMessageBug(), out m_strErr);
                    }
                    else
                    {
                        bool isbasic = m_serverFeedBack.UpdateFeedBackTime(txtFeedBackID.Text,txtCVTID.Text,txtCVTCode.Text, out m_strErr);
                    }
                }
            }
        }

        private void txtUserName_OnCompleteSearch()
        {
            txtCVTCode.Text = txtUserName.DataResult["CVT型号"].ToString();
            txtCVTID.Text = txtUserName.DataResult["CVT编号"].ToString();
            txtChassisNum.Text = txtUserName.DataResult["车架号"].ToString();
            dtpBuyCarTime.Value = Convert.ToDateTime(txtUserName.DataResult["销售日期"].ToString());
            txtLinkTel.Text = txtUserName.DataResult["联系电话"].ToString();
            txtUserName.Text = txtUserName.DataResult["客户名称"].ToString();
            txtCarModel.Text = txtUserName.DataResult["车型"].ToString();
        }

        private void txtDutyPerson_OnCompleteSearch()
        {
            txtDutyPerson.Text = txtDutyPerson.DataResult["姓名"].ToString();
            txtDutyPerson.Tag = txtDutyPerson.DataResult["工号"].ToString();
        }

        private void txtDutyPerson_Enter(object sender, EventArgs e)
        {
            string sql = "";

            if (txtDutyDept.Text.Trim() == "" || txtDutyDept.Text.Trim() == null)
            {
                MessageDialog.ShowPromptMessage("未选择责任部门！");
                return;
            }

            sql += " and Dept like '" + m_serverDepartment.GetDepartmentCode(txtDutyDept.Text) + "%'";

            txtDutyPerson.StrEndSql = sql;
        }

        private void txtDutyDept_OnCompleteSearch()
        {
            txtDutyDept.Text = txtDutyDept.DataResult["部门名称"].ToString();
            txtDutyDept.Tag = txtDutyDept.DataResult["部门编码"].ToString();
        }

        private void btnUpdateReplace_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text.Equals("等待确认返回时间"))
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    if (txtServiceID.Text.Trim() != "")
                    {
                        FormReplaceAccessory frm = new FormReplaceAccessory(txtServiceID.Text);
                        frm.ShowDialog();

                        if (frm.BlIsUpdate)
                        {
                            if (frm.Flag)
                            {
                                if (frm.NewGoodsID != "")
                                {
                                    txtCVTID.Text = frm.NewGoodsID;
                                    txtCVTCode.Text = frm.NewGoodsCode1;
                                    txtTCUCode.Text = frm.NewGoodsID1;
                                }
                            }

                            txtStatus.Text = "等待主管审核";

                            GetControlValue();

                            DataTable ReplaceDt;

                            if (txtServiceID.Text != "" || txtServiceID.Text != null)
                            {
                                ReplaceDt = m_serverFeedBack.GetReplace(txtServiceID.Text, out m_strErr);
                            }
                            else
                            {
                                ReplaceDt = m_serverFeedBack.GetReplace(txtFeedBackID.Text, out m_strErr);
                            }

                            if (ReplaceDt.Rows.Count > 0)
                            {
                                dataGridView1.DataSource = ReplaceDt;
                            }

                            bool isbasic = m_serverFeedBack.UpdateFeedBackTime(txtFeedBackID.Text,txtCVTID.Text,txtCVTCode.Text, out m_strErr);
                        }
                    }
                    else
                    {
                        FormReplaceAccessory frm = new FormReplaceAccessory(txtFeedBackID.Text);
                        frm.ShowDialog();

                        if (frm.BlIsUpdate)
                        {
                            txtStatus.Text = "等待主管审核";

                            GetControlValue();

                            DataTable replaceDt;

                            if (txtServiceID.Text != "" || txtServiceID.Text != null)
                            {
                                replaceDt = m_serverFeedBack.GetReplace(txtServiceID.Text, out m_strErr);
                            }
                            else
                            {
                                replaceDt = m_serverFeedBack.GetReplace(txtFeedBackID.Text, out m_strErr);
                            }

                            if (replaceDt.Rows.Count > 0)
                            {
                                dataGridView1.DataSource = replaceDt;
                            }

                            bool isbasic = m_serverFeedBack.UpdateFeedBackTime(txtFeedBackID.Text, txtCVTID.Text,txtCVTCode.Text, out m_strErr);
                        }
                    }

                }
                else
                {
                    MessageDialog.ShowPromptMessage("此单据没有返回件，若要添加返回件，鼠标点击“有”！");
                }
            }
        }

        /// <summary>
        /// 获取每个信息
        /// </summary>
        void GetControlValue()
        {
            m_lnqServerFeedBack.FK_Bill_ID = txtFeedBackID.Text;
            m_lnqServerFeedBack.ServiceID = txtServiceID.Text;
            m_lnqServerFeedBack.MessageSource = cmbMessageSource.Text;
            m_lnqServerFeedBack.SiteName = txtSiteName.Tag.ToString();
            m_lnqServerFeedBack.CarModel = txtCarModel.Text;
            m_lnqServerFeedBack.CVTCode = txtCVTCode.Text;
            m_lnqServerFeedBack.CVTID = txtCVTID.Text;
            m_lnqServerFeedBack.ChassisNum = txtChassisNum.Text;
            m_lnqServerFeedBack.TCUCode = txtTCUCode.Text;
            m_lnqServerFeedBack.NewSoftware = txtNewSoftware.Text;
            m_lnqServerFeedBack.CVTCondition = txtCVTStatus.Text;
            m_lnqServerFeedBack.UserName = txtUserName.Text;
            m_lnqServerFeedBack.LinkTel = txtLinkTel.Text;
            m_lnqServerFeedBack.BugNumber = txtBugCount.Value.ToString();
            m_lnqServerFeedBack.BatchNumber = txtBatchNumber.Value.ToString();
            m_lnqServerFeedBack.BuyCarTime = dtpBuyCarTime.Value;
            m_lnqServerFeedBack.RunMileage = txtRunMileage.Value.ToString();
            m_lnqServerFeedBack.ProcessMode = cmbStatus.Text;

            if (cbIsBack.Checked)
            {
                m_lnqServerFeedBack.BugCode = "是";
            }
            else
            {
                m_lnqServerFeedBack.BugCode = "否";
            }

            if (txtProcessName.Text.Trim() == "")
            {
                m_lnqServerFeedBack.ProcessName = BasicInfo.LoginID;
            }
            else
            {
                m_lnqServerFeedBack.ProcessName = BasicInfo.LoginID;
            }

            m_lnqServerFeedBack.ProcessTime = dtpProcessTime.Value;
            m_lnqServerFeedBack.Solution = txtSolution.Text;

            if (rbAccesoryBack.Checked)
            {
                m_lnqServerFeedBack.IsBack = "有";
            }
            else
            {
                m_lnqServerFeedBack.IsBack = "无";
            }
        }

        /// <summary>
        /// 检查故障信息是否完整
        /// </summary>
        /// <returns></returns>
        private bool CheckControl()
        {
            if (txtCVTID.Text.Trim()=="" || txtCVTID.Text.Trim().Length < 9)
            {
                MessageDialog.ShowPromptMessage("请填写正确的总成编号 ！");
                return false;
            }

            if (cmbMessageSource.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择信息来源 ！");
                return false;
            }

            if (txtCarModel.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择车型 ！");
                return false;
            }

            if (txtSiteName.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择服务站 ！");
                return false;
            }

            if (cmbStatus.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择是否处理 ！");
                return false;
            }

            if (txtSolution.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写处理措施 ！");
                return false;
            }

            if (cbFrequency.SelectedIndex == 4)
            {
                MessageDialog.ShowPromptMessage("故障频次不能是‘ 未知’ ！");
                return false;
            }

            if (cbCVTOil.SelectedIndex == 3)
            {
                MessageDialog.ShowPromptMessage("CVT油量检测不能是‘ 未知’ ！");
                return false;
            }

            if (cbCondition.SelectedIndex == 5 && txtOther.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("故障出现的特定条件是‘其他’，请填明条件 ！");
                return false;
            }

            if (cbPKey.Checked == false && txtPKey.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填明P档的压力值 ！");
                return false;
            }

            if (cbRKey.Checked == false && txtRKey.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填明R档的压力值 ！");
                return false;
            }

            if (cbNKey.Checked == false && txtNKey.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填明N档的压力值 ！");
                return false;
            }

            if (cbDKey.Checked == false && txtDKey.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填明D档的压力值 ！");
                return false;
            }

            if (cbSKey.Checked == false && txtSKey.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填明S档的压力值 ！");
                return false;
            }

            if (cbPressure.SelectedIndex == 1 && txtPressure.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请说明压力传感器不合格的具体情况 ！");
                return false;
            }

            if (cbActive.SelectedIndex == 1 && txtActive.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请说明主动转速传感器不合格的具体情况 ！");
                return false;
            }

            if (cbPassivity.SelectedIndex == 1 && txtPassivity.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请说明被动转速传感器不合格的具体情况 ！");
                return false;
            }

            if (cbKnob.SelectedIndex == 1 && txtKnob.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请说明档位开关不合格的具体情况 ！");
                return false;
            }

            if (cbOverLink.SelectedIndex == 1 && txtOverLink.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请说明线速连接不合格的具体情况 ！");
                return false;
            }

            if (cbOilSump.SelectedIndex == 1 && txtOilSump.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请说明油底壳、壳体不合格的具体情况 ！");
                return false;
            }

            if ((checkBox11.Checked || checkBox13.Checked) && txtBugDescribe.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("“加速不起”请注明最高可达到的车速；“车速表掉转速”请注明什么车速下掉转速，发动机转速上升到多少转！");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 提交单据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text.Equals("新建单据"))
            {
                if (CheckControl())
                {
                    GetControlValue();

                    bool isbasic = m_serverFeedBack.InsertFeedBack(m_lnqServerFeedBack, GetMessageBug(), out m_strErr);

                    if (isbasic)
                    {
                        m_serverFeedBack.DeleteBillStatus();
                        MessageDialog.ShowPromptMessage("【" + txtFeedBackID.Text + "】反馈单保存成功！");
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage(m_strErr);
                    }

                    m_strFkBillID = m_serverFeedBack.GetFeedBackID()["FK_Bill_ID"].ToString();
                    this.Close();
                }
            }
            else
            {
                if (MessageDialog.ShowEnquiryMessage("单据将重新审核, 是否继续？") == DialogResult.Yes)
                {
                    if (CheckControl())
                    {
                        GetControlValue();

                        bool isbasic = m_serverFeedBack.InsertFeedBack(m_lnqServerFeedBack, GetMessageBug(), out m_strErr);

                        if (isbasic)
                        {
                            m_serverFeedBack.DeleteBillStatus();

                            m_msgPromulgator.PassFlowMessage(txtFeedBackID.Text,
                                    string.Format("【车型】：{0} 【CVT型号】: {1} 【CVT箱号】：{2} 【车架号】：{3}   ※※※ 等待【{4}】处理",
                                    txtCarModel.Text, txtCVTCode.Text, txtCVTID.Text, txtChassisNum.Text, UniversalFunction.GetPersonnelName("0025")),
                                    BillFlowMessage_ReceivedUserType.用户, "0025");

                            MessageDialog.ShowPromptMessage("【" + txtFeedBackID.Text + "】反馈单保存成功！");
                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage(m_strErr);
                        }

                        m_strFkBillID = m_serverFeedBack.GetFeedBackID()["FK_Bill_ID"].ToString();
                        this.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 创建dtProductCodes
        /// </summary>
        private void CreateProductCodesDt()
        {
            m_dtProductCodes.Columns.Add("ProductCode");
            m_dtProductCodes.Columns.Add("GoodsID");
            m_dtProductCodes.Columns.Add("GoodsCode");
            m_dtProductCodes.Columns.Add("GoodsName");
            m_dtProductCodes.Columns.Add("Spec");
            m_dtProductCodes.Columns.Add("BoxNo");
            m_dtProductCodes.Columns.Add("IsUse");
        }

        /// <summary>
        /// 新建一天营销退库单，保存在dt中
        /// </summary>
        private void CreateYXStorage(string goodsCode, string storageID)
        {
            BillNumberControl m_billNoControl = new BillNumberControl(CE_BillTypeEnum.营销退库单, ServerModule.ServerModuleFactory.GetServerModule<ISellIn>());

            string BackID = m_billNoControl.GetNewBillNo();

            DataTable dt = new DataTable();

            dt.Columns.Add("ID");
            dt.Columns.Add("DJH");
            dt.Columns.Add("ObjectDept");
            dt.Columns.Add("LRRY");
            dt.Columns.Add("Date");
            dt.Columns.Add("YWFS");
            dt.Columns.Add("Price");
            dt.Columns.Add("SHRY");
            dt.Columns.Add("StorageID");
            dt.Columns.Add("LRKS");
            dt.Columns.Add("ShDate");
            dt.Columns.Add("KFRY");
            dt.Columns.Add("JYRY");
            dt.Columns.Add("Remark");

            m_drInit = dt.NewRow();
            m_drInit["ID"] = 0;
            m_drInit["DJH"] = BackID;

            if (txtSiteName.Tag == null)
            {
                m_drInit["ObjectDept"] = m_serverFeedBack.GetClient(txtSiteName.Text)["ClientCode"].ToString();
            }
            else
            {
                m_drInit["ObjectDept"] = txtSiteName.Tag.ToString();
            }

            if (txtProcessName.Tag == null)
            {
                m_drInit["LRRY"] = m_personnelInfoServer.GetPersonnelInfoFromName(
                        BasicInfo.DeptCode, txtProcessName.Text).工号;
            }
            else
            {
                m_drInit["LRRY"] = txtProcessName.Tag.ToString();
            }

            m_drInit["Date"] = DateTime.Now.ToString();
            m_drInit["YWFS"] = "销售退库";
            m_drInit["StorageID"] = storageID;

            m_drInit["LRKS"] = BasicInfo.DeptCode;
            m_drInit["SHRY"] = BasicInfo.LoginID;
            m_drInit["ShDate"] = ServerTime.Time;
            m_drInit["KFRY"] = "";
            m_drInit["JYRY"] = "";

            m_dtMxCK = new DataTable();

            m_dtMxCK.Columns.Add("CPID");
            m_dtMxCK.Columns.Add("GoodsCode");
            m_dtMxCK.Columns.Add("GoodsName");
            m_dtMxCK.Columns.Add("Spec");
            m_dtMxCK.Columns.Add("Depot");
            m_dtMxCK.Columns.Add("BatchNo");
            m_dtMxCK.Columns.Add("UnitPrice");
            m_dtMxCK.Columns.Add("Count");
            m_dtMxCK.Columns.Add("Unit");
            m_dtMxCK.Columns.Add("Price");
            m_dtMxCK.Columns.Add("Provider");
            m_dtMxCK.Columns.Add("Remark");

            DataTable dtCvt = m_basicGoodsServer.GetCVTInfo(goodsCode, storageID);

            if (dtCvt != null && dtCvt.Rows.Count > 0)
            {
                m_drCVTInfo = m_dtMxCK.NewRow();
                m_drCVTInfo["CPID"] = dtCvt.Rows[0]["序号"].ToString();
                m_drCVTInfo["GoodsCode"] = dtCvt.Rows[0]["图号型号"].ToString();
                m_drCVTInfo["GoodsName"] = dtCvt.Rows[0]["物品名称"].ToString();
                m_drCVTInfo["Spec"] = dtCvt.Rows[0]["规格"].ToString();
                m_drCVTInfo["Depot"] = dtCvt.Rows[0]["物品类别"].ToString();
                m_drCVTInfo["BatchNo"] = dtCvt.Rows[0]["批次"].ToString();
                m_drCVTInfo["UnitPrice"] = dtCvt.Rows[0]["单价"].ToString();
                m_drCVTInfo["Count"] = txtBugCount.Value;
                m_drCVTInfo["Unit"] = dtCvt.Rows[0]["单位"].ToString();
                m_drCVTInfo["Price"] = Convert.ToDecimal(txtBugCount.Value) * Convert.ToDecimal(dtCvt.Rows[0]["单价"].ToString());
                m_drCVTInfo["Provider"] = dtCvt.Rows[0]["供应商"].ToString();
                m_drCVTInfo["Remark"] = "";

                m_drInit["Price"] = m_drCVTInfo["Price"].ToString();
                m_drInit["Remark"] = "由售后反馈单【" + txtFeedBackID.Text + "】自动生成";

                dt.Rows.Add(m_drInit);
                m_dtMxCK.Rows.Add(m_drCVTInfo);
            }
            else
            {
                MessageDialog.ShowPromptMessage("零件信息有误，请查证！");
            }
        }

        /// <summary>
        /// 新建一张调运单，保存在dt中
        /// </summary>
        private bool CreateManeuverBill()
        {
            int m_intGoodsID = 0;

            try
            {
                string BackID = m_serverBillNo.AssignNewNo(m_maneuverServer, "调运单");

                maneuverBillInfo.AssociatedBillNo = txtFeedBackID.Text;
                maneuverBillInfo.Bill_ID = BackID;
                maneuverBillInfo.BillStatus = "等待入库";

                if (UniversalFunction.IsProduct(m_intGoodsID))
                {
                    maneuverBillInfo.InStorageID = "05";
                }
                else
                {
                    maneuverBillInfo.InStorageID = "09";
                }

                maneuverBillInfo.ProposerTime = dtpProcessTime.Value;
                maneuverBillInfo.Remark = "由售后反馈单【" + txtFeedBackID.Text + "】自动生成";
                maneuverBillInfo.Verify = BasicInfo.LoginName;
                maneuverBillInfo.VerifyTime = ServerTime.Time;
                maneuverBillInfo.OutStorageID = txtSiteName.Tag.ToString();
                maneuverBillInfo.Proposer = txtProcessName.Text;

                if (IntegrativeQuery.GetStorageOrStationPrincipal(maneuverBillInfo.OutStorageID) != null)
                {
                    maneuverBillInfo.Shipper = m_personnerServer.GetPersonnelViewInfo(IntegrativeQuery.GetStorageOrStationPrincipal(maneuverBillInfo.OutStorageID)[0]).员工姓名;
                    maneuverBillInfo.ExcShipper = m_personnerServer.GetPersonnelViewInfo(IntegrativeQuery.GetStorageOrStationPrincipal(maneuverBillInfo.OutStorageID)[0]).员工姓名;
                }
                else
                {
                    return false;
                }

                maneuverBillInfo.ShipperTime = Convert.ToDateTime(dataGridView1.Rows[0].Cells["旧件发出时间"].Value);
                maneuverBillInfo.ExcConfirmorTime = Convert.ToDateTime(dataGridView1.Rows[0].Cells["返回时间"].Value);
                maneuverBillInfo.ExcConfirmor = "彭何威";

                listManeuverBill.Add(maneuverBillInfo);

                dtManeuverList = new DataTable();

                dtManeuverList.Columns.Add("Unique");
                dtManeuverList.Columns.Add("物品ID");
                dtManeuverList.Columns.Add("申请数量");
                dtManeuverList.Columns.Add("发货数量");
                dtManeuverList.Columns.Add("收货数量");
                dtManeuverList.Columns.Add("备注");
                dtManeuverList.Columns.Add("账务库房ID");

                View_S_Stock S_StockInfo = m_storeServer.GetGoodsStore(Convert.ToInt32(m_intGoodsID));

                m_drCVTInfo = dtManeuverList.NewRow();
                m_drCVTInfo["Unique"] = txtCVTID.Text;
                m_drCVTInfo["物品ID"] = m_intGoodsID;
                m_drCVTInfo["申请数量"] = 1;
                m_drCVTInfo["发货数量"] = 1;
                m_drCVTInfo["收货数量"] = 1;
                m_drCVTInfo["备注"] = "";
                m_drCVTInfo["账务库房ID"] = maneuverBillInfo.InStorageID;

                dtManeuverList.Rows.Add(m_drCVTInfo);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 质管部意见
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text.Equals("等待质管确认"))
            {
                if (txtDutyDept.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请选择责任部门！");
                    return;
                }

                if (dtpReplyTime.Value <= dtpSignatureDate.Value)
                {
                    MessageDialog.ShowPromptMessage("要求完成时间应大于处理时间，请重新选择！");
                    return;
                }

                int count = 0;

                if (txtAppearCount.Value != 0)
                {
                    count = Convert.ToInt32(txtAppearCount.Value);
                }
                
                bool isZGCheck =false;

                if (txtSameBill.Text.Trim() == "")
                {
                    isZGCheck = m_serverFeedBack.UpdateZGAffirm(txtFeedBackID.Text, txtZGChargeSuggestion.Text,
                        txtDutyDept.Tag.ToString(), dtpReplyTime.Value, count,null, out m_strErr);
                }
                else
                {
                    isZGCheck = m_serverFeedBack.UpdateZGAffirm(txtFeedBackID.Text, txtZGChargeSuggestion.Text,
                        txtDutyDept.Tag.ToString(), dtpReplyTime.Value, count, txtSameBill.Text, out m_strErr);
                }

                if (isZGCheck && txtSameBill.Text.Trim() == "")
                {
                    m_msgPromulgator.PassFlowMessage(txtFeedBackID.Text,
                                    string.Format("【车型】：{0} 【CVT型号】: {1} 【CVT箱号】：{2} 【车架号】：{3}   ※※※ 等待【{4}部门主管】处理",
                                    txtCarModel.Text, txtCVTCode.Text, txtCVTID.Text, txtChassisNum.Text, txtDutyDept.Text),
                      BillFlowMessage_ReceivedUserType.角色,m_msgPromulgator.GetDeptPrincipalRoleName(txtDutyDept.Text).ToList());

                    MessageDialog.ShowPromptMessage("【" + txtFeedBackID.Text + "】号反馈单质管确认成功！");
                }
                else if (isZGCheck && txtSameBill.Text.Trim() != "")
                {
                    List<string> noticeRoles = new List<string>();

                    noticeRoles.Add(CE_RoleEnum.营销普通人员.ToString());
                    noticeRoles.Add(CE_RoleEnum.营销主管.ToString());
                    noticeRoles.Add(CE_RoleEnum.质量工程师.ToString());
                    noticeRoles.Add(CE_RoleEnum.质控主管.ToString());

                    m_msgPromulgator.EndFlowMessage(txtFeedBackID.Text,
                        string.Format("{0} 号售后服务反馈单已经处理完毕", txtFeedBackID.Text),
                        noticeRoles, null);

                    MessageDialog.ShowPromptMessage("【" + txtFeedBackID.Text + "】号反馈单处理完成！");
                }
                else
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                }
            }
            else if (txtStatus.Text.Trim().Equals("等待责任部门确认"))
            {
                MessageDialog.ShowPromptMessage("单据已经过质管确认,不能重复确认！");
            }
            else
            {
                MessageDialog.ShowPromptMessage("请等待营销部门确认单据后，再进行此操作！");
            }

            this.Close();
        }

        /// <summary>
        /// 责任部门确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text.Equals("等待责任部门确认"))
            {
                if (txtDutyPerson.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请选择责任人");
                    return;
                }

                if (txtDutyDeptCharge.Text.Trim() == "")
                {
                    txtDutyDeptCharge.Text = BasicInfo.LoginName;
                    txtDutyDeptCharge.Tag = BasicInfo.LoginID;
                }

                if (m_serverDepartment.IsPersonByDept(txtDutyDeptCharge.Tag.ToString()))
                {
                    bool isDuty = m_serverFeedBack.UpdateDutyDept(txtFeedBackID.Text,
                            txtDutyPerson.Tag.ToString(), txtFinishClaim.Text, txtStockSuggestion.Text, out m_strErr);                    

                    if (isDuty)
                    {
                        if (txtSameBill.Text.Trim() != "")
                        {
                            m_msgPromulgator.PassFlowMessage(txtFeedBackID.Text,
                                    string.Format("【车型】：{0} 【CVT型号】: {1} 【CVT箱号】：{2} 【车架号】：{3}   ※※※ 等待【质量工程师】处理",
                                    txtCarModel.Text, txtCVTCode.Text, txtCVTID.Text, txtChassisNum.Text),
                                    BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.质量工程师.ToString());
                        }
                        else
                        {
                            m_msgPromulgator.PassFlowMessage(txtFeedBackID.Text,
                                    string.Format("【车型】：{0} 【CVT型号】: {1} 【CVT箱号】：{2} 【车架号】：{3}   ※※※ 等待【{4}】处理",
                                    txtCarModel.Text, txtCVTCode.Text, txtCVTID.Text, txtChassisNum.Text, txtDutyPerson.Text), BillFlowMessage_ReceivedUserType.用户, txtDutyPerson.Tag.ToString());
                        }

                        MessageDialog.ShowPromptMessage("【" + txtFeedBackID.Text + "】号反馈单责任部门主管确认成功！");
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage(m_strErr);
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("只能是" + txtDutyDept.Text + "的主管才能进行此操作！");
                }
            }
            else if (txtStatus.Text.Trim().Equals("等待责任人确认"))
            {
                MessageDialog.ShowPromptMessage("单据已经过确认，不能重复确认");
            }
            else
            {
                MessageDialog.ShowPromptMessage("请等待质管部门确认单据后，再进行此操作！");
            }

            this.Close();
        }

        /// <summary>
        /// 责任人确认单据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text.Equals("等待责任人确认"))
            {
                if (BasicInfo.LoginID == txtDutyPerson.Tag.ToString())
                {
                    if (txtTemporary.Text.Trim() == "" || txtAnalyse.Text.Trim() == "")
                    {
                        MessageDialog.ShowPromptMessage("请将信息填写完整填写");
                        return;
                    }

                    S_ServiceFeedBack list = new S_ServiceFeedBack();

                    list.FK_Bill_ID = txtFeedBackID.Text;
                    list.Temporary = txtTemporary.Text;
                    list.Analyse = txtAnalyse.Text;
                    list.foreverImplement = txtforeverImplement.Text;
                    list.IsFMEAfile = cmbIsFMEA.Text;
                    list.IsOpen = cmbIsOpen.Text;

                    bool isdutyPerson = m_serverFeedBack.UpdateDutyPerson(list, out m_strErr);

                    if (isdutyPerson)
                    {
                        m_msgPromulgator.PassFlowMessage(txtFeedBackID.Text,
                                    string.Format("【车型】：{0} 【CVT型号】: {1} 【CVT箱号】：{2} 【车架号】：{3}   ※※※ 等待【质量工程师】处理",
                                    txtCarModel.Text, txtCVTCode.Text, txtCVTID.Text, txtChassisNum.Text),
                     CE_RoleEnum.质量工程师.ToString(), true);

                        MessageDialog.ShowPromptMessage("【" + txtFeedBackID.Text + "】号反馈单责任人确认成功！");
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage(m_strErr);
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("您没有权限进行此操作！");
                }
            }
            else if (txtStatus.Text.Equals("等待质管检查"))
            {
                MessageDialog.ShowPromptMessage("负责人已经确认过，不能重复确认！");
            }
            else
            {
                MessageDialog.ShowPromptMessage("请等待责任部门确认单据后，再进行此操作！");
            }

            this.Close();
        }

        /// <summary>
        /// 质管检查
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text.Equals("等待质管检查"))
            {
                bool isZgCheck = m_serverFeedBack.UpdateZGCheck(txtFeedBackID.Text, cmbIsClose.Text, txtPracticable.Text, out m_strErr);

                if (isZgCheck)
                {
                    List<string> noticeRoles = new List<string>();

                    noticeRoles.Add(CE_RoleEnum.营销普通人员.ToString());
                    noticeRoles.Add(CE_RoleEnum.营销主管.ToString());
                    noticeRoles.Add(CE_RoleEnum.质量工程师.ToString());
                    noticeRoles.Add(CE_RoleEnum.质控主管.ToString());

                    m_msgPromulgator.EndFlowMessage(txtFeedBackID.Text,
                        string.Format("{0} 号售后服务反馈单已经处理完毕", txtFeedBackID.Text),
                        noticeRoles, null);

                    MessageDialog.ShowPromptMessage("【" + txtFeedBackID.Text + "】号反馈单处理完成！");
                }
                else
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                }
            }
            else if (txtStatus.Text.Equals("单据完成"))
            {
                MessageDialog.ShowPromptMessage("单据已处理完成，不能重复确认！");
            }
            else
            {
                MessageDialog.ShowPromptMessage("请等待负责人确认单据后，再进行此操作！");
            }
            this.Close();
        }

        /// <summary>
        /// 通过函电获得顾客及故障信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtServiceID_Click(object sender, EventArgs e)
        {
            FormQueryInfo frm = QueryInfoDialog.GetAfterServiceBill("",""); 
            frm.ShowDialog();

            txtServiceID.Text = (string)frm.GetDataItem("单据号");

            if (txtServiceID.Text != "")
            {
                DataTable dtServier = m_serverFeedBack.GetAfterServiceByBillID(txtServiceID.Text);

                cmbMessageSource.Text = dtServier.Rows[0]["信息来源"].ToString();
                txtUserName.Text = dtServier.Rows[0]["用户姓名"].ToString();
                txtCVTCode.Text = dtServier.Rows[0]["变速箱型号"].ToString();
                txtCVTID.Text = dtServier.Rows[0]["变速箱编号"].ToString();

                if (dtServier.Rows[0]["行驶里程"].ToString() == "")
                {
                    txtRunMileage.Value = 0;
                }
                else
                    txtRunMileage.Value = Convert.ToDecimal(dtServier.Rows[0]["行驶里程"].ToString());

                txtSiteName.Text = dtServier.Rows[0]["服务站名称"].ToString();
                txtSolution.Text = dtServier.Rows[0]["处理方案"].ToString();
                txtCarModel.Text = dtServier.Rows[0]["车型"].ToString();

                if (dtServier.Rows[0]["购车时间"].ToString() == "")
                {
                    dtpBuyCarTime.Value = Convert.ToDateTime(ServerTime.Time.ToShortDateString());
                }
                else
                {
                    dtpBuyCarTime.Value = Convert.ToDateTime(dtServier.Rows[0]["购车时间"].ToString());
                }

                txtLinkTel.Text = dtServier.Rows[0]["用户电话"].ToString();
                txtChassisNum.Text = dtServier.Rows[0]["车架号"].ToString();

                DataTable replaceDt = m_serverFeedBack.GetReplace(txtServiceID.Text, out m_strErr);

                dataGridView1.DataSource = replaceDt;

                if (dataGridView1.Rows.Count > 0)
                {
                    rbAccesoryBack.Checked = true;
                }

                rbAccesoryBack.Enabled = false;
                rbAccesoryNotBack.Enabled = false;

                GetMessageLoad();
            }
        }

        private void 修改toolStripButton7_Click(object sender, EventArgs e)
        {
            if (txtProcessName.Tag.ToString() == BasicInfo.LoginID)
            {
                ControlUsable();

                txtFeedBackID.ReadOnly = true;
                txtServiceID.ReadOnly = true;
                txtStatus.ReadOnly = true;
                txtBackID.ReadOnly = true;
            }
            else
            {
                MessageDialog.ShowPromptMessage("对不起！只有反馈人自己才能进行此操作！");
            }
        }

        private void cbOilSump_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbOilSump.SelectedIndex == 1)
            {
                txtOilSump.Visible = true;
            }
            else
            {
                txtOilSump.Visible = false;
            }
        }

        private void cbPressure_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPressure.SelectedIndex == 1)
            {
                txtPressure.Visible = true;
            }
            else
            {
                txtPressure.Visible = false;
            }
        }

        private void cbActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbActive.SelectedIndex == 1)
            {
                txtActive.Visible = true;
            }
            else
            {
                txtActive.Visible = false;
            }
        }

        private void cbPassivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPassivity.SelectedIndex == 1)
            {
                txtPassivity.Visible = true;
            }
            else
                txtPassivity.Visible = false;
        }

        private void cbKnob_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbKnob.SelectedIndex == 1)
                txtKnob.Visible = true;
            else
                txtKnob.Visible = false;
        }

        private void cbOverLink_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbOverLink.SelectedIndex == 1)
                txtOverLink.Visible = true;
            else
                txtOverLink.Visible = false;
        }

        private void cbPKey_CheckedChanged(object sender, EventArgs e)
        {
            if (cbPKey.Checked)
            {
                txtPKey.ReadOnly = true;
            }
            else
                txtPKey.ReadOnly = false;
        }

        private void cbRKey_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRKey.Checked)
                txtRKey.ReadOnly = true;
            else
                txtRKey.ReadOnly = false;
        }

        private void cbNKey_CheckedChanged(object sender, EventArgs e)
        {
            if (cbNKey.Checked)
                txtNKey.ReadOnly = true;
            else
                txtNKey.ReadOnly = false;
        }

        private void cbDKey_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDKey.Checked)
                txtDKey.ReadOnly = true;
            else
                txtDKey.ReadOnly = false;
        }

        private void cbSKey_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSKey.Checked)
                txtSKey.ReadOnly = true;
            else
                txtSKey.ReadOnly = false;
        }

        private void cbCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbCondition.SelectedIndex == 5)
            {
                txtOther.Visible = true;
            }
            else
                txtOther.Visible = false;
        }

        private void txtBackID_DoubleClick(object sender, EventArgs e)
        {
            if (txtBackID.Text == "系统自动生成")
            {
                营销退库明细单 form = new 营销退库明细单(0, m_authFlag);
                form.ShowDialog();
            }
            else
            {
                DataRow row = m_serverFeedBack.IsExist(txtBackID.Text);

                营销退库明细单 form = new 营销退库明细单(Convert.ToInt32(row["ID"].ToString()), m_authFlag);
                form.ShowDialog();
            }
        }

        private void txtSiteName_OnCompleteSearch()
        {
            txtSiteName.Tag = txtSiteName.DataResult["客户编码"].ToString();
            txtSiteName.Text = txtSiteName.DataResult["客户名称"].ToString();
        }

        private void cmbMessageSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMessageSource.SelectedIndex == 0)
            {
                txtRunMileage.Value = 0;
                txtRunMileage.Enabled = false;
            }
            else
                txtRunMileage.Enabled = true;
        }

        /// <summary>
        /// 获得故障信息
        /// </summary>
        /// <returns></returns>
        private OF_BugMessageInfo GetMessageBug()
        {
            OF_BugMessageInfo LnqBugInfo = new OF_BugMessageInfo();

            LnqBugInfo.BugCode = GetBugCode();
            LnqBugInfo.CarMainBug = GetCarMainBug();
            LnqBugInfo.CarSecendBug = GetCarSecondBug();
            LnqBugInfo.Frequency = cbFrequency.Text;
            LnqBugInfo.Condition = cbCondition.Text;
            LnqBugInfo.BugDeclare = txtBugDescribe.Text;
            LnqBugInfo.CVTOilDetection = cbCVTOil.Text;

            if (cbPressure.SelectedIndex == 1)
            {
                LnqBugInfo.PressureSensor = txtPressure.Text;
            }
            else
            {
                LnqBugInfo.PressureSensor = cbPressure.Text;
            }

            if (cbActive.SelectedIndex == 1)
            {
                LnqBugInfo.ActiveSensor = txtActive.Text;
            }
            else
            {
                LnqBugInfo.ActiveSensor = cbActive.Text;
            }

            if (cbPassivity.SelectedIndex == 1)
            {
                LnqBugInfo.PassivitySensor = txtPassivity.Text;
            }
            else
            {
                LnqBugInfo.PassivitySensor = cbPassivity.Text;
            }

            if (cbKnob.SelectedIndex == 1)
            {
                LnqBugInfo.ShiftKnob = txtKnob.Text;
            }
            else
            {
                LnqBugInfo.ShiftKnob = cbKnob.Text;
            }

            if (cbOverLink.SelectedIndex == 1)
            {
                LnqBugInfo.OverLinkStatus = txtOverLink.Text;
            }
            else
            {
                LnqBugInfo.OverLinkStatus = cbOverLink.Text;
            }

            if (cbOilSump.SelectedIndex == 1)
            {
                LnqBugInfo.OilSump = txtOilSump.Text;
            }
            else
            {
                LnqBugInfo.OilSump = cbOilSump.Text;
            }

            if (cbPKey.Checked)
            {
                LnqBugInfo.PKey = cbPKey.Text;
            }
            else
            {
                LnqBugInfo.PKey = txtPKey.Text;
            }

            if (cbRKey.Checked)
            {
                LnqBugInfo.RKey = cbRKey.Text;
            }
            else
                LnqBugInfo.RKey = txtRKey.Text;

            if (cbNKey.Checked)
            {
                LnqBugInfo.NKey = cbNKey.Text;
            }
            else
                LnqBugInfo.NKey = txtNKey.Text;

            if (cbDKey.Checked)
            {
                LnqBugInfo.DKey = cbDKey.Text;
            }
            else
                LnqBugInfo.DKey = txtDKey.Text;

            if (cbSKey.Checked)
            {
                LnqBugInfo.SKey = cbSKey.Text;
            }
            else
                LnqBugInfo.SKey = txtSKey.Text;

            return LnqBugInfo;
        }

        /// <summary>
        /// 获得客观故障
        /// </summary>
        /// <returns>返回字符串形式的故障信息</returns>
        private string GetCarSecondBug()
        {
            foreach (Control cl in this.panel3.Controls)
            {
                if (cl is CheckBox)
                {
                    if (((CheckBox)cl).Checked)
                    {
                        m_strCarSecondBug = cl.Text + ";";
                    }
                }
            }

            if (m_strCarMainBug == "" || m_strCarMainBug == null)
            {
                return m_strCarMainBug;
            }
            else
                return m_strCarMainBug.Substring(0, m_strCarMainBug.Length - 1);
        }

        /// <summary>
        /// 获得主观故障
        /// </summary>
        /// <returns>返回字符串形式的主观故障信息</returns>
        private string GetCarMainBug()
        {
            foreach (Control cl in this.panel2.Controls)
            {
                if (cl is CheckBox)
                {
                    if (((CheckBox)cl).Checked)
                    {
                        m_strCarMainBug = cl.Text + ";";
                    }
                }
            }


            if (m_strCarMainBug == "" || m_strCarMainBug == null)
            {
                return m_strCarMainBug;
            }
            else
                return m_strCarMainBug.Substring(0, m_strCarMainBug.Length - 1);
        }

        /// <summary>
        /// 获得故障代码
        /// </summary>
        /// <returns>返回故障代码</returns>
        private string GetBugCode()
        {
            string bugCode = "";

            foreach (Control cl in this.panel5.Controls)
            {
                if (cl is CheckBox)
                {
                    if (((CheckBox)cl).Checked)
                    {
                        bugCode = cl.Text + ";";
                    }
                }
            }

            if (bugCode == "")
            {
                return bugCode;
            }
            else
                return bugCode.Substring(0, bugCode.Length - 1);
        }

        /// <summary>
        /// 获得初始化故障信息
        /// </summary>
        private void GetMessageLoad()
        {
            DataTable fkBugDt;

            if (m_strServiceBill != "")
            {
                fkBugDt = m_serverFeedBack.GetBugMessageByServiceID(txtServiceID.Text);
            }
            else
                fkBugDt = m_serverFeedBack.GetBugMessageByServiceID(m_strFkBillID);

            if (fkBugDt.Rows.Count > 0)
            {
                //ddlBugCode.Text = FeedBack.GetBugCodeByName(Convert.ToInt32(FKBugDt.Rows[0]["BugCode"].ToString()))["BugName"].ToString();
                cbFrequency.Text = fkBugDt.Rows[0]["Frequency"].ToString();
                cbCVTOil.Text = fkBugDt.Rows[0]["CVTOilDetection"].ToString();
                txtBugDescribe.Text = fkBugDt.Rows[0]["BugDeclare"].ToString();

                if (fkBugDt.Rows[0]["Condition"].ToString().Equals("冷车"))
                {
                    cbCondition.SelectedIndex = 0;
                    txtOther.Visible = false;
                }
                else if (fkBugDt.Rows[0]["Condition"].ToString().Equals("热车"))
                {
                    cbCondition.SelectedIndex = 1;
                    txtOther.Visible = false;
                }
                else if (fkBugDt.Rows[0]["Condition"].ToString().Equals("低速（60码以下）"))
                {
                    cbCondition.SelectedIndex = 2;
                    txtOther.Visible = false;
                }
                else if (fkBugDt.Rows[0]["Condition"].ToString().Equals("高速（60码以上）"))
                {
                    cbCondition.SelectedIndex = 3;
                    txtOther.Visible = false;
                }
                else if (fkBugDt.Rows[0]["Condition"].ToString().Equals("行驶过程中突然出现"))
                {
                    cbCondition.SelectedIndex = 4;
                    txtOther.Visible = false;
                }
                else
                {
                    cbCondition.SelectedIndex = 5;
                    txtOther.Text = fkBugDt.Rows[0]["Condition"].ToString();
                    txtOther.Visible = true;
                }

                if (fkBugDt.Rows[0]["PressureSensor"].ToString().Equals("合格"))
                {
                    cbPressure.SelectedIndex = 0;
                    txtPressure.Visible = false;
                }
                else if (fkBugDt.Rows[0]["PressureSensor"].ToString().Equals("未知"))
                {
                    cbPressure.SelectedIndex = 2;
                    txtPressure.Visible = false;
                }
                else
                {
                    cbPressure.SelectedIndex = 1;
                    txtPressure.Text = fkBugDt.Rows[0]["PressureSensor"].ToString();
                    txtPressure.Visible = true;
                }

                if (fkBugDt.Rows[0]["ActiveSensor"].ToString().Equals("合格"))
                {
                    cbActive.SelectedIndex = 0;
                    txtActive.Visible = false;
                }
                else if (fkBugDt.Rows[0]["ActiveSensor"].ToString().Equals("未知"))
                {
                    cbActive.SelectedIndex = 2;
                    txtActive.Visible = false;
                }
                else
                {
                    cbActive.SelectedIndex = 1;
                    txtActive.Text = fkBugDt.Rows[0]["ActiveSensor"].ToString();
                    txtActive.Visible = true;
                }

                if (fkBugDt.Rows[0]["PassivitySensor"].ToString().Equals("合格"))
                {
                    cbPassivity.SelectedIndex = 0;
                    txtPassivity.Visible = false;
                }
                else if (fkBugDt.Rows[0]["PassivitySensor"].ToString().Equals("未知"))
                {
                    cbPassivity.SelectedIndex = 2;
                    txtPassivity.Visible = false;
                }
                else
                {
                    cbPassivity.SelectedIndex = 1;
                    txtPassivity.Text = fkBugDt.Rows[0]["PassivitySensor"].ToString();
                    txtPassivity.Visible = true;
                }

                if (fkBugDt.Rows[0]["ShiftKnob"].ToString().Equals("合格"))
                {
                    cbKnob.SelectedIndex = 0;
                    txtKnob.Visible = false;
                }
                else if (fkBugDt.Rows[0]["ShiftKnob"].ToString().Equals("未知"))
                {
                    cbKnob.SelectedIndex = 2;
                    txtKnob.Visible = false;
                }
                else
                {
                    cbKnob.SelectedIndex = 1;
                    txtKnob.Text = fkBugDt.Rows[0]["ShiftKnob"].ToString();
                    txtKnob.Visible = true;
                }

                if (fkBugDt.Rows[0]["OverLinkStatus"].ToString().Equals("合格"))
                {
                    cbOverLink.SelectedIndex = 0;
                    txtOverLink.Visible = false;
                }
                else if (fkBugDt.Rows[0]["OverLinkStatus"].ToString().Equals("未知"))
                {
                    cbOverLink.SelectedIndex = 2;
                    txtOverLink.Visible = false;
                }
                else
                {
                    cbOverLink.SelectedIndex = 1;
                    txtOverLink.Text = fkBugDt.Rows[0]["OverLinkStatus"].ToString();
                    txtOverLink.Visible = true;
                }

                if (fkBugDt.Rows[0]["OilSump"].ToString().Equals("合格"))
                {
                    cbOilSump.SelectedIndex = 0;
                    txtOilSump.Visible = false;
                }
                else if (fkBugDt.Rows[0]["OilSump"].ToString().Equals("未知"))
                {
                    cbOilSump.SelectedIndex = 2;
                    txtOilSump.Visible = false;
                }
                else
                {
                    cbOilSump.SelectedIndex = 1;
                    txtOilSump.Text = fkBugDt.Rows[0]["OilSump"].ToString();
                    txtOilSump.Visible = true;
                }

                if (fkBugDt.Rows[0]["PKey"].ToString().Equals("服务站未检查"))
                {
                    cbPKey.Checked = true;
                    txtPKey.ReadOnly = true;
                }
                else
                {
                    txtPKey.Text = fkBugDt.Rows[0]["PKey"].ToString();
                    cbPKey.Enabled = false;
                }

                if (fkBugDt.Rows[0]["RKey"].ToString().Equals("服务站未检查"))
                {
                    cbRKey.Checked = true;
                    txtRKey.ReadOnly = true;
                }
                else
                {
                    txtRKey.Text = fkBugDt.Rows[0]["RKey"].ToString();
                    cbRKey.Enabled = false;
                }

                if (fkBugDt.Rows[0]["NKey"].ToString().Equals("服务站未检查"))
                {
                    cbNKey.Checked = true;
                    txtNKey.ReadOnly = true;
                }
                else
                {
                    txtNKey.Text = fkBugDt.Rows[0]["NKey"].ToString();
                    cbNKey.Enabled = false;
                }

                if (fkBugDt.Rows[0]["DKey"].ToString().Equals("服务站未检查"))
                {
                    cbDKey.Checked = true;
                    txtDKey.ReadOnly = true;
                }
                else
                {
                    txtDKey.Text = fkBugDt.Rows[0]["DKey"].ToString();
                    cbDKey.Enabled = false;
                }

                if (fkBugDt.Rows[0]["SKey"].ToString().Equals("服务站未检查"))
                {
                    cbSKey.Checked = true;
                    txtSKey.ReadOnly = true;
                }
                else
                {
                    txtSKey.Text = fkBugDt.Rows[0]["SKey"].ToString();
                    cbSKey.Enabled = false;
                }

                if (fkBugDt.Rows[0]["CarMainBug"].ToString().Contains("其他"))
                {
                    checkBox39.Checked = true;
                }

                if (fkBugDt.Rows[0]["CarMainBug"].ToString().Contains("抖动"))
                {
                    checkBox1.Checked = true;
                }

                if (fkBugDt.Rows[0]["CarMainBug"].ToString().Contains("异响"))
                {
                    checkBox2.Checked = true;
                }

                if (fkBugDt.Rows[0]["CarMainBug"].ToString().Contains("噪音大"))
                {
                    checkBox3.Checked = true;
                }

                if (fkBugDt.Rows[0]["CarMainBug"].ToString().Contains("油耗高"))
                {
                    checkBox4.Checked = true;
                }

                if (fkBugDt.Rows[0]["CarMainBug"].ToString().Contains("换挡冲击"))
                {
                    checkBox5.Checked = true;
                }

                if (fkBugDt.Rows[0]["CarMainBug"].ToString().Contains("加速慢"))
                {
                    checkBox6.Checked = true;
                }

                if (fkBugDt.Rows[0]["CarSecendBug"].ToString().Contains("前进挡无反应"))
                {
                    checkBox7.Checked = true;
                }

                if (fkBugDt.Rows[0]["CarSecendBug"].ToString().Contains("其他"))
                {
                    checkBox40.Checked = true;
                }

                if (fkBugDt.Rows[0]["CarSecendBug"].ToString().Contains("倒挡无反应"))
                {
                    checkBox8.Checked = true;
                }

                if (fkBugDt.Rows[0]["CarSecendBug"].ToString().Contains("挂档无反应"))
                {
                    checkBox9.Checked = true;
                }

                if (fkBugDt.Rows[0]["CarSecendBug"].ToString().Contains("亮故障灯"))
                {
                    checkBox10.Checked = true;
                }

                if (fkBugDt.Rows[0]["CarSecendBug"].ToString().Contains("加速不起（不变速）"))
                {
                    checkBox11.Checked = true;
                }

                if (fkBugDt.Rows[0]["CarSecendBug"].ToString().Contains("漏油"))
                {
                    checkBox12.Checked = true;
                }

                if (fkBugDt.Rows[0]["CarSecendBug"].ToString().Contains("车速表掉转速"))
                {
                    checkBox13.Checked = true;
                }

                if (fkBugDt.Rows[0]["CarSecendBug"].ToString().Contains("刹车熄火"))
                {
                    checkBox14.Checked = true;
                }

                if (fkBugDt.Rows[0]["CarSecendBug"].ToString().Contains("无挡位显示"))
                {
                    checkBox15.Checked = true;
                }

                if (fkBugDt.Rows[0]["CarSecendBug"].ToString().Contains("P挡拔不出"))
                {
                    checkBox16.Checked = true;
                }

                if (fkBugDt.Rows[0]["CarSecendBug"].ToString().Contains("P挡锁不住"))
                {
                    checkBox17.Checked = true;
                }

                if (fkBugDt.Rows[0]["BugCode"].ToString().Contains("无故障码"))
                {
                    checkBox18.Checked = true;
                }

                if (fkBugDt.Rows[0]["BugCode"].ToString().Contains("P0703"))
                {
                    checkBox19.Checked = true;
                }

                if (fkBugDt.Rows[0]["BugCode"].ToString().Contains("P0705"))
                {
                    checkBox20.Checked = true;
                }

                if (fkBugDt.Rows[0]["BugCode"].ToString().Contains("P0710"))
                {
                    checkBox21.Checked = true;
                }

                if (fkBugDt.Rows[0]["BugCode"].ToString().Contains("P0715"))
                {
                    checkBox22.Checked = true;
                }

                if (fkBugDt.Rows[0]["BugCode"].ToString().Contains("P0720"))
                {
                    checkBox23.Checked = true;
                }

                if (fkBugDt.Rows[0]["BugCode"].ToString().Contains("P0725"))
                {
                    checkBox24.Checked = true;
                }

                if (fkBugDt.Rows[0]["BugCode"].ToString().Contains("P0730"))
                {
                    checkBox32.Checked = true;
                }

                if (fkBugDt.Rows[0]["BugCode"].ToString().Contains("P0744"))
                {
                    checkBox33.Checked = true;
                }

                if (fkBugDt.Rows[0]["BugCode"].ToString().Contains("P0745"))
                {
                    checkBox34.Checked = true;
                }

                if (fkBugDt.Rows[0]["BugCode"].ToString().Contains("P0750"))
                {
                    checkBox35.Checked = true;
                }

                if (fkBugDt.Rows[0]["BugCode"].ToString().Contains("P0755"))
                {
                    checkBox36.Checked = true;
                }

                if (fkBugDt.Rows[0]["BugCode"].ToString().Contains("P0760"))
                {
                    checkBox37.Checked = true;
                }

                if (fkBugDt.Rows[0]["BugCode"].ToString().Contains("P1706"))
                {
                    checkBox38.Checked = true;
                }

                if (fkBugDt.Rows[0]["BugCode"].ToString().Contains("P1707"))
                {
                    checkBox31.Checked = true;
                }

                if (fkBugDt.Rows[0]["BugCode"].ToString().Contains("P1708"))
                {
                    checkBox30.Checked = true;
                }

                if (fkBugDt.Rows[0]["BugCode"].ToString().Contains("P1709"))
                {
                    checkBox29.Checked = true;
                }

                if (fkBugDt.Rows[0]["BugCode"].ToString().Contains("P1710"))
                {
                    checkBox28.Checked = true;
                }

                if (fkBugDt.Rows[0]["BugCode"].ToString().Contains("P1711"))
                {
                    checkBox27.Checked = true;
                }

                if (fkBugDt.Rows[0]["BugCode"].ToString().Contains("P1712"))
                {
                    checkBox26.Checked = true;
                }

                if (fkBugDt.Rows[0]["BugCode"].ToString().Contains("无法检查"))
                {
                    checkBox25.Checked = true;
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 禁用控件
        /// </summary>
        private void ControlNotUsable()
        {
            foreach (Control cl in this.groupBox1.Controls)
            {
                if (cl is TextBox)
                {
                    ((TextBox)cl).ReadOnly = true;

                    if (cl.GetType() == typeof(TextBoxShow))
                    {
                        ((TextBox)cl).Enabled = false;
                    }
                    cl.BackColor = Color.White;
                }

                if (cl is ComboBox)
                {
                    ((ComboBox)cl).Enabled = false;
                    cl.BackColor = Color.White;
                }

                if (cl is CheckBox)
                {
                    ((CheckBox)cl).Enabled = false;
                    cl.BackColor = Color.White;
                }

                if (cl is NumericUpDown)
                {
                    ((NumericUpDown)cl).ReadOnly = true;
                    cl.BackColor = Color.White;
                }

                if (cl is DateTimePicker)
                {
                    ((DateTimePicker)cl).Enabled = false;
                    cl.BackColor = Color.White;
                }
            }

            foreach (Control clPanel in this.groupBox7.Controls)
            {
                if (clPanel is TextBox)
                {
                    ((TextBox)clPanel).ReadOnly = true;

                    if (clPanel.GetType() == typeof(TextBoxShow))
                    {
                        ((TextBox)clPanel).Enabled = false;
                    }
                    
                    clPanel.BackColor = Color.White;
                }

                if (clPanel is ComboBox)
                {
                    ((ComboBox)clPanel).Enabled = false;
                    clPanel.BackColor = Color.White;
                }

                if (clPanel is CheckBox)
                {
                    ((CheckBox)clPanel).Enabled = false;
                    clPanel.BackColor = Color.White;
                }
            }

            foreach (Control clPanel in this.panel2.Controls)
            {
                if (clPanel is CheckBox)
                {
                    ((CheckBox)clPanel).Enabled = false;
                    clPanel.BackColor = Color.White;
                }
            }

            foreach (Control clPanel in this.panel3.Controls)
            {
                if (clPanel is CheckBox)
                {
                    ((CheckBox)clPanel).Enabled = false;
                    clPanel.BackColor = Color.White;
                }
            }

            foreach (Control clPanel in this.panel4.Controls)
            {
                if (clPanel is CheckBox)
                {
                    ((CheckBox)clPanel).Enabled = false;
                    clPanel.BackColor = Color.White;
                }

                if (clPanel is TextBox)
                {
                    ((TextBox)clPanel).ReadOnly = true;

                    if (clPanel.GetType() == typeof(TextBoxShow))
                    {
                        ((TextBox)clPanel).Enabled = false;
                    }

                    clPanel.BackColor = Color.White;
                }
            }

            cmbTKFS.Enabled = true;
        }

        /// <summary>
        /// 控件可以使用
        /// </summary>
        private void ControlUsable()
        {
            foreach (Control cl in this.groupBox1.Controls)
            {
                if (cl is TextBox)
                {
                    ((TextBox)cl).ReadOnly = false;
                    ((TextBox)cl).Enabled = true;

                    cl.BackColor = Color.White;
                }

                if (cl is ComboBox)
                {
                    ((ComboBox)cl).Enabled = true;
                    cl.BackColor = Color.White;
                }

                if (cl is CheckBox)
                {
                    ((CheckBox)cl).Enabled = true;
                    cl.BackColor = Color.White;
                }

                if (cl is NumericUpDown)
                {
                    ((NumericUpDown)cl).ReadOnly = false;
                    cl.BackColor = Color.White;
                }

                if (cl is DateTimePicker)
                {
                    ((DateTimePicker)cl).Enabled = true;
                    cl.BackColor = Color.White;
                }
            }

            foreach (Control cl in this.groupBox7.Controls)
            {
                if (cl is TextBox)
                {
                    ((TextBox)cl).ReadOnly = false;
                    ((TextBox)cl).Enabled = true;
                    cl.BackColor = Color.White;
                }

                if (cl is ComboBox)
                {
                    ((ComboBox)cl).Enabled = true;
                    cl.BackColor = Color.White;
                }

                if (cl is CheckBox)
                {
                    ((CheckBox)cl).Enabled = true;
                    cl.BackColor = Color.White;
                }
            }

            foreach (Control cl in this.panel2.Controls)
            {
                if (cl is CheckBox)
                {
                    ((CheckBox)cl).Enabled = true;
                    cl.BackColor = Color.White;
                }
            }

            foreach (Control cl in this.panel3.Controls)
            {
                if (cl is CheckBox)
                {
                    ((CheckBox)cl).Enabled = true;
                    cl.BackColor = Color.White;
                }
            }

            foreach (Control cl in this.panel4.Controls)
            {
                if (cl is CheckBox)
                {
                    ((CheckBox)cl).Enabled = true;
                    cl.BackColor = Color.White;
                }

                if (cl is TextBox)
                {
                    ((TextBox)cl).ReadOnly = false;
                    ((TextBox)cl).Enabled = true;

                    cl.BackColor = Color.White;
                }
            }
        }

        private void txtChassisNum_OnCompleteSearch()
        {
            txtCVTCode.Text = txtChassisNum.DataResult["CVT型号"].ToString();
            txtCVTID.Text = txtChassisNum.DataResult["CVT编号"].ToString();
            txtChassisNum.Text = txtChassisNum.DataResult["车架号"].ToString();
            txtCarModel.Text = txtChassisNum.DataResult["车型号"].ToString();
        }

        private void checkBox39_Click(object sender, EventArgs e)
        {
            if (checkBox39.Checked)
            {
                MessageDialog.ShowPromptMessage("请在故障补充说明中填写整车主观故障!");
            }
        }

        private void checkBox40_Click(object sender, EventArgs e)
        {
            if (checkBox40.Checked)
            {
                MessageDialog.ShowPromptMessage("请在故障补充说明中填写整车客观故障!");
            }
        }

        private void 回退toolStripButton_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text.Trim() != "单据完成")
            {
                回退单据 form = new 回退单据(CE_BillTypeEnum.售后服务质量反馈单, txtFeedBackID.Text, txtStatus.Text);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (MessageBox.Show("您确定要回退此单据吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        bool b = false;

                        if (cbIsBack.Checked)
                        {
                            b = m_serverFeedBack.ReturnFeedBackBill(form.StrBillID,
                            form.StrBillStatus, form.Reason, txtBackID.Text, out m_strErr);
                        }
                        else
                        {
                            b = m_serverFeedBack.ReturnFeedBackBill(form.StrBillID,
                            form.StrBillStatus, form.Reason, txtBackID.Text, out m_strErr);
                        }

                        if (b)
                        {
                            MessageDialog.ShowPromptMessage("回退成功！");
                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage(m_strErr);
                        }
                    }

                    this.Close();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }

        private void txtSameBill_Click(object sender, EventArgs e)
        {
            FormQueryInfo frm = QueryInfoDialog.GetFeedBackBill("", "");
            frm.ShowDialog();

            txtSameBill.Text = (string)frm.GetDataItem("反馈单号");

            if (txtSameBill.Text != "")
            {
                txtFinishClaim.Text = (string)frm.GetDataItem("完成要求");
                txtStockSuggestion.Text = (string)frm.GetDataItem("库存产品处理意见");
                txtTemporary.Text = (string)frm.GetDataItem("临时措施");
                cmbIsFMEA.Text = (string)frm.GetDataItem("是否列入FMEA文件");
                cmbIsOpen.Text = (string)frm.GetDataItem("是否水平展开");
                txtforeverImplement.Text = (string)frm.GetDataItem("永久性措施");
                txtAnalyse.Text = (string)frm.GetDataItem("原因分析");
                txtDutyDept.Text = (string)frm.GetDataItem("责任部门");
                txtDutyDept.Tag = (string)frm.GetDataItem("部门编码");
                txtDutyPerson.Text = (string)frm.GetDataItem("责任人");
                txtDutyPerson.Tag = UniversalFunction.GetPersonnelCode((string)frm.GetDataItem("责任人"));
                txtPracticable.Text = (string)frm.GetDataItem("落实情况");
                cmbIsClose.SelectedIndex = 1;


                txtFinishClaim.ReadOnly = true;
                txtStockSuggestion.ReadOnly = true;
                txtTemporary.ReadOnly = true;
                cmbIsOpen.Enabled = false;
                cmbIsFMEA.Enabled = false;
                txtforeverImplement.ReadOnly = true;
                txtAnalyse.ReadOnly = true;
            }
            else
            {
                txtFinishClaim.ReadOnly = false;
                txtStockSuggestion.ReadOnly = false;
                txtTemporary.ReadOnly = false;
                cmbIsOpen.Enabled = true;
                cmbIsFMEA.Enabled = true;
                txtforeverImplement.ReadOnly = false;
                txtAnalyse.ReadOnly = false;
            }
        }

        /// <summary>
        /// 营销部意见
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 主管审核toolStripButton_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text.Equals("等待主管审核"))
            {
                DataTable replaceDt;

                if (!m_isServiceStock)//判断是否从售后库房发出
                {
                    cbIsBack.Checked = false;
                    cbIsBack.Enabled = false;
                }

                //if (cbIsBack.Checked)
                //{
                //    if (MessageBox.Show("是否生成关联单？", "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                //    {
                //        cbIsBack.Checked = false;
                //    }
                //}

                if (txtServiceID.Text != "" || txtServiceID.Text != null)
                {
                    replaceDt = m_serverFeedBack.GetReplace(txtServiceID.Text, out m_strErr);
                }
                else
                {
                    replaceDt = m_serverFeedBack.GetReplace(txtFeedBackID.Text, out m_strErr);
                }

                if (txtYXChargeSuggestion.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写营销部意见！");
                    return;
                }

                if (cbIsBack.Checked && cmbTKFS.Text == "")
                {
                    MessageBox.Show("请选择退库方式", "提示");
                    cmbTKFS.Focus();
                    return;
                }

                bool isYXCheck = false;
                //int GoodsID = 0;
                //string billNo = "";
                //string TKFS = "";
                //bool CreateBill = false;

                //if (cbIsBack.Checked)
                //{
                //    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                //    {
                        //int newGoodsID = m_basicGoodsServer.GetGoodsIDByGoodsCode(dataGridView1.Rows[i].Cells["图号"].Value.ToString(),
                        //    dataGridView1.Rows[i].Cells["更新件"].Value.ToString(), dataGridView1.Rows[i].Cells["规格1"].Value.ToString());

                        //GoodsID = m_basicGoodsServer.GetGoodsIDByGoodsCode(dataGridView1.Rows[i].Cells["返回件图号"].Value.ToString(),
                        //    dataGridView1.Rows[i].Cells["返回件"].Value.ToString(), dataGridView1.Rows[i].Cells["规格"].Value.ToString());
                        ////DataTable Dt = m_serverProductList.GetOneProductListTable(GoodsID);

                        ////if (Dt != null && Dt.Rows.Count > 0)
                        ////{
                        //m_intGoodsID = GoodsID;
                        ////}

                        //if (m_intGoodsID == 0)
                        //{
                        //    MessageDialog.ShowPromptMessage("没有返回零件，不能生成关联单！");
                        //    return;
                        //}

                        //if (cmbTKFS.SelectedIndex == 0)
                        //{
                        //    DataTable Dt = m_serverProductList.GetOneProductListTable(GoodsID);
                        //    string stoctID = "01";

                        //    if (Dt != null && Dt.Rows.Count > 0)
                        //    {
                        //        m_intGoodsID = GoodsID;
                        //        stoctID = "02";
                        //        CreateProductCodesDt();

                        //        View_F_GoodsPlanCost goodsInfo = m_basicGoodsServer.GetGoodsInfoView(GoodsID);

                        //        DataRow dr = DtProductCodes.NewRow();

                        //        dr["ProductCode"] = txtCVTID.Text;
                        //        dr["GoodsID"] = GoodsID;
                        //        dr["GoodsCode"] = goodsInfo.图号型号.ToString();
                        //        dr["GoodsName"] = goodsInfo.物品名称.ToString();
                        //        dr["Spec"] = goodsInfo.规格.ToString();
                        //        dr["BoxNo"] = "";
                        //        DtProductCodes.Rows.Add(dr);
                        //    }

                        //    if (m_intGoodsID == 0)
                        //    {
                        //        continue;
                        //    }

                        //    TKFS = cmbTKFS.Text;                           

                        //    CreateYXStorage(dataGridView1.Rows[i].Cells["返回件图号"].Value.ToString(), stoctID); 

                        //    CreateBill = m_serverFeedBack.UpdateYXCheckCreateManeuverBill(DtProductCodes, m_dtMxCK,
                        //        m_drInit, MarketingType.退库.ToString(), null, null, m_drInit["DJH"].ToString(), out m_strErr);

                        //    if (CreateBill)
                        //    {
                        //        billNo += m_drInit["DJH"].ToString();
                        //    }
                        //}
                        //else
                        //{
                            //TKFS = cmbTKFS.Text;

                            //if (CreateManeuverBill())
                            //{
                            //    string code = "";

                            //    if (GoodsID != newGoodsID)
                            //    {
                            //        //if (CreateRequisitionBill(newGoodsID) && CreateReturnedInTheDepotBill(GoodsID))
                            //        //{
                            //        //    DataTable dtTemp = m_serverProductList.GetOneProductListTable(newGoodsID);

                            //        //    if (dtTemp.Rows.Count > 0)
                            //        //    {
                            //        //        code = dtTemp.Rows[0]["Code"].ToString();
                            //        //    }

                            //        //    ProductsCodes lnqList = new ProductsCodes();

                            //        //    lnqList.ProductCode = dataGridView1.Rows[i].Cells["更新件编号"].Value.ToString();
                            //        //    lnqList.GoodsName = dataGridView1.Rows[i].Cells["更新件"].Value.ToString();
                            //        //    lnqList.GoodsCode = dataGridView1.Rows[i].Cells["图号"].Value.ToString();
                            //        //    lnqList.Spec = dataGridView1.Rows[i].Cells["规格1"].Value.ToString();
                            //        //    lnqList.GoodsID = newGoodsID;
                            //        //    lnqList.Code = code;
                            //        //    lnqList.ZcCode = "";
                            //        //    lnqList.DJH = m_requisitionBill.Bill_ID;
                            //        //    lnqList.BoxNo = "";

                            //        //    CreateBill = m_serverFeedBack.UpdateYXCheckCreateManeuverBill(m_inTheDepotBill, m_inTheDepotGoods, m_requisitionBill,
                            //        //        m_lnqGoods, lnqList, maneuverBillInfo, dtManeuverList,
                            //        //        m_serverFeedBack.GetClient(txtSiteName.Text)["ClientCode"].ToString(), out m_strErr);
                            //        //}
                            //    }
                            //    else
                            //    {
                            //        CreateBill = m_serverFeedBack.UpdateYXCheckCreateManeuverBill(null, null, null, null, maneuverBillInfo,
                            //            dtManeuverList, maneuverBillInfo.Bill_ID, out m_strErr);

                                    
                            //    }
                            //}
                            //else
                            //{
                            //    MessageDialog.ShowPromptMessage("外部库房信息有误");
                            //    return;
                            //}
                //        }

                //        if (CreateBill)
                //        {
                //            billNo += maneuverBillInfo.Bill_ID;
                //        }
                //        else
                //        {
                //            MessageDialog.ShowPromptMessage(m_strErr);
                //            return;
                //        }
                //    }

                //    if (CreateBill)
                //    {
                //        isYXCheck = m_serverFeedBack.UpdateYXCheck(txtFeedBackID.Text,
                //            txtYXChargeSuggestion.Text, billNo, TKFS, out m_strErr);
                //    }
                //    else
                //    {
                //        MessageDialog.ShowPromptMessage(m_strErr);
                //        return;
                //    }
                //}
                //else
                //{
                    isYXCheck = m_serverFeedBack.UpdateYXCheck(txtFeedBackID.Text,
                        txtYXChargeSuggestion.Text, "",cmbTKFS.Text, out m_strErr);
                //}

                if (isYXCheck)
                {
                    m_msgPromulgator.PassFlowMessage(txtFeedBackID.Text,
                                    string.Format("【车型】：{0} 【CVT型号】: {1} 【CVT箱号】：{2} 【车架号】：{3}   ※※※ 等待【质量工程师】处理",
                                    txtCarModel.Text, txtCVTCode.Text, txtCVTID.Text, txtChassisNum.Text),
                      CE_RoleEnum.质量工程师.ToString(), true);

                    MessageDialog.ShowPromptMessage("【" + txtFeedBackID.Text + "】反馈单审核成功！");
                }
                else
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                }
            }
            else if (txtStatus.Text.Trim().Equals("等待确认返回时间"))
            {
                MessageDialog.ShowPromptMessage("请您的下属人员确认全部返回件的返回时间后，再进行此操作！");
            }
            else if (txtStatus.Text.Trim().Equals("等待质管确认"))
            {
                MessageDialog.ShowPromptMessage("单据已经审核！");
            }
            else
            {
                MessageDialog.ShowPromptMessage("单据未填写！");
            }

            this.Close();
        }
    }
}
