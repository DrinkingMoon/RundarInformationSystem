using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using WebServerModule;
using WebServerModule2;
using GlobalObject;
using PlatformManagement;
using Service_Peripheral_HR;
using System.IO;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 售后函电明细界面
    /// </summary>
    public partial class FormAfterService : Form
    {
        /// <summary>
        /// 部门服务组件
        /// </summary>
        IDepartmentServer m_serverDepartment = ServerModule.ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 服务类
        /// </summary>
        IServiceFeedBack2 m_serverFeedBack = WebServerModule2.ServerModuleFactory2.GetServerModule<IServiceFeedBack2>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 文件名
        /// </summary>
        string m_fileName;

        /// <summary>
        /// 售后函电的基础类
        /// </summary>
        WebServerModule2.S_AfterService m_lnqAfterService = new WebServerModule2.S_AfterService();

        public WebServerModule2.S_AfterService LnqAfterService
        {
            get { return m_lnqAfterService; }
            set { m_lnqAfterService = value; }
        }

        /// <summary>
        /// 基础物品服务
        /// </summary>
        IBasicGoodsServer m_basicGoodsServer = ServerModuleFactory2.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 数据集
        /// </summary>
        DataTable m_dtList = new DataTable();

        /// <summary>
        /// 权限组件
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 人员档案管理类
        /// </summary>
        IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        /// <summary>
        /// 产品信息服务组件
        /// </summary>
        IProductCodeServer m_serverProductCode = ServerModule.ServerModuleFactory.GetServerModule<IProductCodeServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        public FormAfterService(string djh, AuthorityFlag nodeInfo)
        {
            InitializeComponent();
            m_authFlag = nodeInfo;
            AuthorityControl(m_authFlag);
            m_billMessageServer.BillType = "售后函电处理单";

            txtCheckName.Text = BasicInfo.LoginName;
            txtCheckName.Tag = BasicInfo.LoginID;

            txtReturnName.Text = BasicInfo.LoginName;
            txtReturnName.Tag = BasicInfo.LoginID;

            dtpReturnTime.Value = ServerTime.Time;
            toolStrip1.Visible = true;

            dataBindControl(djh);
            GetMessageLoad();
        }

        public FormAfterService(AuthorityFlag nodeInfo)
        {
            InitializeComponent();

            //获得信息源，绑定
            DataTable messageDt = m_serverFeedBack.GetMessageSource();

            for (int i = 0; i < messageDt.Rows.Count; i++)
            {
                cmbMessageSource.Items.Add(messageDt.Rows[i]["来源"].ToString());
            }

            //获得来电类型，绑定
            DataTable typeDt = m_serverFeedBack.GetMessageType();

            for (int i = 0; i < typeDt.Rows.Count; i++)
            {
                cmbType.Items.Add(typeDt.Rows[i]["Type"].ToString());
            }

            txtApplicant.Text = BasicInfo.LoginName;
            txtApplicant.Tag = BasicInfo.LoginID;
            dtpApplicantDate.Value = ServerTime.Time;
            txtStatus.Text = "新建单据";
            txtServiceID.Text = "系统自动生成";
            txtServiceID.ForeColor = Color.Red;
            cmbMessageSource.SelectedIndex = 2;
            cmbStatus.SelectedIndex = 0;
            cmbType.SelectedIndex = 0;
            cbPressure.SelectedIndex = 0;
            cbActive.SelectedIndex = 0;
            cbPassivity.SelectedIndex = 0;
            cbKnob.SelectedIndex = 0;
            cbOverLink.SelectedIndex = 0;
            cbOilSump.SelectedIndex = 0;
            cbCVTOil.SelectedIndex = 2;
            cbConType.SelectedIndex = 1;

            m_authFlag = nodeInfo;
            AuthorityControl(m_authFlag);

            toolStrip1.Visible = true;
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "售后函电处理单");

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
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, authorityFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);
        }

        /// <summary>
        /// 绑定控件
        /// </summary>
        /// <param name="djh">单据号</param>
        void dataBindControl(string djh)
        {
            CreateDateTableStyle();

            DataTable messageDt = m_serverFeedBack.GetMessageSource();

            for (int i = 0; i < messageDt.Rows.Count; i++)
            {
                cmbMessageSource.Items.Add(messageDt.Rows[i]["来源"].ToString());
            }

            DataTable typeDt = m_serverFeedBack.GetMessageType();

            for (int i = 0; i < typeDt.Rows.Count; i++)
            {
                cmbType.Items.Add(typeDt.Rows[i]["Type"].ToString());
            }

            DataTable dt = m_serverFeedBack.GetAfterServiceByBillID(djh);
            DataRow row = dt.Rows[0];

            txtServiceID.Text = row["单据号"].ToString();
            txtServiceIdea.Text = row["客服中心处理意见"].ToString();
            txtSiteName.Text = row["服务站名称"].ToString();
            txtSolution.Text = row["处理方案"].ToString();
            txtStatus.Text = row["单据状态"].ToString();
            txtUseProperty.Text = row["使用性质"].ToString();
            txtUserAddress.Text = row["用户住址"].ToString();
            txtUserAttitude.Text = row["用户态度"].ToString();
            txtUserName.Text = row["用户姓名"].ToString();
            txtUserTel.Text = row["用户电话"].ToString();
            txtAcceptName.Text = row["接函电人"].ToString();

            if (Convert.ToBoolean(row["是否CVT故障"]))
            {
                cmbCVTBug.Text = "是";
            }
            else
            {
                cmbCVTBug.Text = "否";
            }

            if (row["接函电人编号"] != null)
            {
                txtAcceptName.Tag = row["接函电人编号"].ToString();
            }
            else
            {
                txtAcceptName.Tag = m_personnerServer.GetPersonnelViewInfoByName(txtAcceptName.Text);
            }

            cmbStatus.Text = row["总成状态"].ToString();

            if (row["金额"].ToString() == "")
            {
                txtAmount.Text = "0";
            }
            else
            {
                txtAmount.Text = row["金额"].ToString();
            }

            txtBugAddress.Text = row["故障地点"].ToString();
            txtCarModel.Text = row["车型"].ToString();
            txtChassisNum.Text = row["车架号"].ToString();
            txtCheckName.Text = row["审核人"].ToString();

            if (row["审核人编号"] != null)
            {
                txtCheckName.Tag = row["审核人编号"].ToString();
            }
            else
            {
                txtCheckName.Tag = m_personnerServer.GetPersonnelViewInfoByName(txtCheckName.Text);
            }

            cbConType.Text = row["内容类别"].ToString();
            txtCVTCode.Text = row["变速箱型号"].ToString();
            txtCVTID.Text = row["变速箱编号"].ToString();
            txtFailureResults.Text = row["故障处理效果确认"].ToString();
            txtLinkTel.Text = row["联系电话"].ToString();
            txtProcessName.Text = row["接单处理人"].ToString();
            cbIsServiceStock.Checked = Convert.ToBoolean(row["是否由售后库房发出"]);

            if (row["新三包"] != null)
            {
                cbSanBao.Checked = row["新三包"].ToString() == "是" ? true : false;
            }
            else
            {
                cbSanBao.Checked = false;
            }

            if (row["是否三包内"].ToString() != "是")
            {
                cbIsThreeGuarantees.Checked = false;
            }
            else
            {
                cbIsThreeGuarantees.Checked = true;
            }

            if (row["处理人编号"] != null)
            {
                txtProcessName.Tag = row["处理人编号"].ToString();
            }
            else
            {
                txtProcessName.Tag = m_personnerServer.GetPersonnelViewInfoByName(txtProcessName.Text);
            }

            txtProcessResult.Text = row["处理结果"].ToString();
            txtApplicant.Text = row["函电录入人"].ToString();

            if (row["录入人编号"] != null)
            {
                txtApplicant.Tag = row["录入人编号"].ToString();
            }
            else
            {
                txtApplicant.Tag = m_personnerServer.GetPersonnelViewInfoByName(txtApplicant.Text);
            }

            if (row["接函电时间"].ToString() != "")
            {
                dtpAcceptTime.Value = Convert.ToDateTime(row["接函电时间"].ToString());
                dtpAcceptTime.Checked = true;
            }

            if (row["处理意见提出时间"].ToString() != "")
            {
                dtpCustomerDate.Value = Convert.ToDateTime(row["处理意见提出时间"].ToString());
                dtpCustomerDate.Checked = true;
            }
            else
            {
                dtpCustomerDate.Checked = false;
            }

            if (row["通报时间"].ToString() != "")
            {
                dtpNoticeDate.Value = Convert.ToDateTime(row["通报时间"].ToString());
                dtpNoticeDate.Checked = true;
            }
            else
            {
                dtpNoticeDate.Checked = false;
            }

            if (row["提出应对方案时间"].ToString() != "")
            {
                dtpStrategyDate.Value = Convert.ToDateTime(row["提出应对方案时间"].ToString());
                dtpStrategyDate.Checked = true;
            }
            else
            {
                dtpStrategyDate.Checked = false;
            }

            if (row["函电录入时间"].ToString() == "")
            {
                dtpApplicantDate.Checked = false;
            }
            else
            {
                dtpApplicantDate.Value = Convert.ToDateTime(row["函电录入时间"].ToString());
            }

            if (row["维修质量"].ToString() == "")
            {
                txtRepairQuality.Text = "好";
            }
            else
            {
                txtRepairQuality.Text = row["维修质量"].ToString();
            }

            txtReturnName.Text = row["回访人"].ToString();

            if (row["回访人编号"] != null)
            {
                txtReturnName.Tag = row["回访人编号"].ToString();
            }
            else
            {
                txtReturnName.Tag = m_personnerServer.GetPersonnelViewInfoByName(txtReturnName.Text);
            }

            txtRunMileage.Text = row["行驶里程"].ToString();

            if (row["服务态度"].ToString() == "")
            {
                txtServiceAttitude.Text = "好";
            }
            else
            {
                txtServiceAttitude.Text = row["服务态度"].ToString();
            }
            txtDiagnoseSituation.Text = row["诊断及测试情况"].ToString();

            GetMessageLoad();

            if (row["现场处理"].ToString() == "是")
            {
                rbLocaleY.Checked = true;
            }
            else
                rbLocaleN.Checked = true;

            if (row["救援金额"].ToString() != "否")
            {
                rbHelpY.Checked = true;
                txtHelMoney.Text = row["救援金额"].ToString();
            }
            else
            {
                rbHelpN.Checked = true;
                txtHelMoney.Text = "0";
            }

            if (row["购车时间"].ToString() == "")
            {
                dtpBuyCarTime.Value = Convert.ToDateTime(ServerTime.Time.ToShortDateString());
            }
            else
            {
                dtpBuyCarTime.Value = Convert.ToDateTime(row["购车时间"].ToString());
            }

            if (row["审核回访时间"].ToString() == "")
            {
                dtpCheckTime.Value = ServerTime.Time;
            }
            else
            {
                dtpCheckTime.Value = Convert.ToDateTime(row["审核回访时间"].ToString());
            }

            if (row["接单时间"].ToString() == "")
            {
                dtpProcessTime.Value = ServerTime.Time;
            }
            else
            {
                dtpProcessTime.Value = Convert.ToDateTime(row["接单时间"].ToString());
            }

            if (row["回访时间"].ToString() == "")
            {
                dtpReturnTime.Value = Convert.ToDateTime(ServerTime.Time);
            }
            else
            {
                dtpReturnTime.Value = Convert.ToDateTime(row["回访时间"].ToString());
            }

            if (row["反馈单号"].ToString().Equals("无") || row["反馈单号"].ToString() == "")
            {
                radioButton2.Checked = true;
                lbFKBillID.Text = "无";
            }
            else
            {
                radioButton1.Checked = true;
                lbFKBillID.Text = row["反馈单号"].ToString();
            }

            cmbMessageSource.Text = row["信息来源"].ToString();
            cmbType.Text = row["函电类别"].ToString();

            if (row["是否收费"].ToString().Equals("") || row["是否收费"].ToString().Equals("否"))
            {
                rbIsChargeNot.Checked = true;
                txtAmount.Text = "0";
            }
            else
            {
                rbIsCharge.Checked = true;
                txtAmount.Text = row["是否收费"].ToString();
            }

            DataTable Replaydt = m_serverFeedBack.GetReplaceByID(djh);

            dataGridView1.DataSource = Replaydt;
            dataGridView1.Columns["Remark"].Visible = false;
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["GiveOutDate"].Visible = false;

            if (txtStatus.Text.Equals("等待审核"))
            {
                txtCheckName.Text = BasicInfo.LoginName;
                txtCheckName.Tag = BasicInfo.LoginID;
            }

            if (txtStatus.Text.Equals("等待回访"))
            {
                txtReturnName.Text = BasicInfo.LoginName;
                txtReturnName.Tag = BasicInfo.LoginID;
            }

            DataTable dtFile = m_serverFeedBack.GetAfterServiceFile(djh);

            if (dtFile != null && dtFile.Rows.Count > 0)
            {
                linkFile.Visible = true;
                linkFile.Text = "下载附件：" + dtFile.Rows[0]["FileNames"].ToString();
                //m_picbyte = dtFile.Rows[0]["FileAddress"] as byte[];
                m_fileName = dtFile.Rows[0]["FileNames"].ToString();
            }

            if (txtStatus.Text == "等待接单")
            {
                cmbCVTBug.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// 创建样式
        /// </summary>
        private void CreateDateTableStyle()
        {
            m_dtList.Columns.Add("ServiceID");
            m_dtList.Columns.Add("OldGoodsName");
            m_dtList.Columns.Add("OldGoodsCode");
            m_dtList.Columns.Add("OldSpec");
            m_dtList.Columns.Add("OldGoodsID");
            m_dtList.Columns.Add("OldCvtID");
            m_dtList.Columns.Add("NewGoodsName");
            m_dtList.Columns.Add("NewGoodsCode");
            m_dtList.Columns.Add("NewSpec");
            m_dtList.Columns.Add("NewGoodsID");
            m_dtList.Columns.Add("NewCvtID");
            m_dtList.Columns.Add("BackTime");
            m_dtList.Columns.Add("Count");
            m_dtList.Columns.Add("Unit");
            m_dtList.Columns.Add("Remark");
            m_dtList.Columns.Add("GiveOutDate");
        }

        /// <summary>
        /// 检查同种物品
        /// </summary>
        /// <param name="intGoodsID"></param>
        /// <returns></returns>
        public bool CheckSameGoods()
        {
            if (m_dtList == null || m_dtList.Rows.Count == 0)
            {
                return true;
            }

            m_dtList = (DataTable)dataGridView1.DataSource;

            if (m_dtList.Rows.Count > 0)
            {
                for (int i = 0; i < m_dtList.Rows.Count; i++)
                {

                    if (m_dtList.Rows[i]["OldGoodsName"].ToString().Trim() == txtOldName.Text.Trim()
                        && m_dtList.Rows[i]["OldGoodsCode"].ToString().Trim() == txtOldCode.Text.Trim()
                        && m_dtList.Rows[i]["OldSpec"].ToString().Trim() == txtOldSpec.Text.Trim())
                    {
                        MessageDialog.ShowPromptMessage("不能添加同种物品！");
                        return false;
                    }
                }
            }
            else
            {
                if (MessageDialog.ShowEnquiryMessage("此单没有返回件和更新件吗？") == DialogResult.No)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 检查故障信息是否完整
        /// </summary>
        /// <returns></returns>
        private bool CheckControl()
        {
            if (dtpProcessTime.Value > ServerTime.Time)
            {
                MessageDialog.ShowPromptMessage("处理时间不能大于系统当前时间 ！");
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

            if (cbPressure.SelectedIndex == 2)
            {
                MessageDialog.ShowPromptMessage("压力传感器不能是‘ 未知’ ！");
                return false;
            }

            if (cbActive.SelectedIndex == 2)
            {
                MessageDialog.ShowPromptMessage("主动转速传感器不能是‘ 未知’ ！");
                return false;
            }

            if (cbPassivity.SelectedIndex == 2)
            {
                MessageDialog.ShowPromptMessage("被动转速传感器不能是‘ 未知’ ！");
                return false;
            }

            if (cbKnob.SelectedIndex == 2)
            {
                MessageDialog.ShowPromptMessage("档位开关不能是‘ 未知’ ！");
                return false;
            }

            if (cbOverLink.SelectedIndex == 2)
            {
                MessageDialog.ShowPromptMessage("线速连接状况不能是‘ 未知’ ！");
                return false;
            }

            if (cbOilSump.SelectedIndex == 2)
            {
                MessageDialog.ShowPromptMessage("油底壳、壳体不能是‘ 未知’ ！");
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

            if (rbLocaleY.Checked == false && rbLocaleN.Checked == false)
            {
                MessageDialog.ShowPromptMessage("请选择是否现场处理 ！");
                return false;
            }

            if (txtOldCode.Text.Trim() != "")
            {
                if (UniversalFunction.IsProduct(Convert.ToInt32(txtOldCode.Tag.ToString())))
                {
                    if (!m_serverProductCode.VerifyProductCodesInfo(Convert.ToInt32(txtOldCode.Tag.ToString()), 
                        txtOldGoodsID.Text.Trim(), GlobalObject.CE_BarCodeType.内部钢印码, out m_strErr))
                    {
                        MessageDialog.ShowPromptMessage(m_strErr);
                        return false;
                    }

                    if (txtOldGoodsID.Text.Trim() != txtOldCvtID.Text.Trim())
                    {
                        MessageDialog.ShowPromptMessage("当返回件是总成时，返回件编号和总成编号必须一致");
                        return false;
                    }
                }
            }

            if (txtNewCode.Text.Trim() != "")
            {
                if (UniversalFunction.IsProduct(Convert.ToInt32(txtNewCode.Tag.ToString())))
                {
                    if (!m_serverProductCode.VerifyProductCodesInfo(Convert.ToInt32(txtNewCode.Tag.ToString()), 
                        txtNewGoodsID.Text.Trim(), GlobalObject.CE_BarCodeType.内部钢印码, out m_strErr))
                    {
                        MessageDialog.ShowPromptMessage(m_strErr);
                        return false;
                    }

                    if (txtNewCvtID.Text.Trim() != txtNewGoodsID.Text.Trim())
                    {
                        MessageDialog.ShowPromptMessage("当更新件是总成时，更新件编号和总成编号必须一致");
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 为S_AfterService赋值
        /// </summary>
        /// <returns>返回S_AfterService数据集</returns>
        private WebServerModule2.S_AfterService GetServiceInfo()
        {
            if (txtServiceID.Text != "系统自动生成")
            {
                m_lnqAfterService.ServiceID = txtServiceID.Text;
            }
            else
                m_lnqAfterService.ServiceID = m_serverFeedBack.GetNextBillID(1);

            m_lnqAfterService.MessageSource = cmbMessageSource.Text;
            m_lnqAfterService.ServerType = cmbType.Text;
            m_lnqAfterService.ContentType = cbConType.Text;
            m_lnqAfterService.AcceptName = txtAcceptName.Tag.ToString();
            m_lnqAfterService.SiteName = txtSiteName.Text;
            m_lnqAfterService.LinkTel = txtLinkTel.Text;
            m_lnqAfterService.UserName = txtUserName.Text;
            m_lnqAfterService.UserTel = txtUserTel.Text;
            m_lnqAfterService.UserAddress = txtUserAddress.Text;
            m_lnqAfterService.CarModel = txtCarModel.Text;
            m_lnqAfterService.ChassisNum = txtChassisNum.Text;
            m_lnqAfterService.CVTCode = txtCVTCode.Text;
            m_lnqAfterService.CVTID = txtCVTID.Text;
            m_lnqAfterService.BugAddress = txtBugAddress.Text;
            m_lnqAfterService.BuyCarTime = dtpBuyCarTime.Text;
            m_lnqAfterService.UseProperty = txtUseProperty.Text;
            m_lnqAfterService.UserAttitude = txtUserAttitude.Text;
            m_lnqAfterService.ProcessName = txtProcessName.Tag.ToString();
            m_lnqAfterService.ProcessTime = dtpProcessTime.Value;
            m_lnqAfterService.ServiceIdea = txtServiceIdea.Text;
            m_lnqAfterService.CVTStatus = cmbStatus.Text;
            m_lnqAfterService.PY = UniversalFunction.GetPYWBCode(txtUserName.Text.Trim(), "PY");
            m_lnqAfterService.WB = UniversalFunction.GetPYWBCode(txtUserName.Text.Trim(), "WB");
            m_lnqAfterService.TCUCode = txtTCUCode.Text;
            m_lnqAfterService.NewSoftware = txtNewSoftware.Text;
            m_lnqAfterService.IsServiceStock = cbIsServiceStock.Checked;
            m_lnqAfterService.IsCVTBug = false;

            if (cbIsThreeGuarantees.Checked)
            {
                m_lnqAfterService.IsThreeGuarantees = "是";
            }
            else
            {
                m_lnqAfterService.IsThreeGuarantees = "否";
            }

            if (dtpAcceptTime.Checked)
            {
                m_lnqAfterService.AcceptTime = dtpAcceptTime.Value;
            }

            if (dtpCustomerDate.Checked)
            {
                m_lnqAfterService.CustomerDate = dtpCustomerDate.Value;
            }

            if (dtpNoticeDate.Checked)
            {
                m_lnqAfterService.NoticeDate = dtpNoticeDate.Value;
            }

            if (dtpStrategyDate.Checked)
            {
                m_lnqAfterService.StrategyDate = dtpStrategyDate.Value;
            }

            m_lnqAfterService.ApplicantDate = ServerTime.Time;
            m_lnqAfterService.Applicant = BasicInfo.LoginID;

            if (rbHelpY.Checked)
            {
                m_lnqAfterService.HelpMoney = txtHelMoney.Text;
            }
            else
                m_lnqAfterService.HelpMoney = "否";

            if (cmbMessageSource.Text != "主机厂")
            {
                m_lnqAfterService.RunMileage = txtRunMileage.Text;
            }
            else
                m_lnqAfterService.RunMileage = "0";

            return m_lnqAfterService;
        }

        /// <summary>
        /// 清空控件
        /// </summary>
        private void ClearControl()
        {
            txtOldCode.Text = "";
            txtOldName.Text = "";
            txtOldSpec.Text = "";
            txtOldCvtID.Text = "";
            txtNewCvtID.Text = "";
            txtNewCode.Text = "";
            txtNum.Value = 0;
            txtNewName.Text = "";
            txtNewSpec.Text = "";
            txtUnit.Text = "";
            txtOldGoodsID.Text = "";
            txtNewGoodsID.Text = "";
        }

        #region 获得故障信息
        /// <summary>
        /// 获得故障信息
        /// </summary>
        /// <returns></returns>
        private WebServerModule2.OF_BugMessageInfo GetMessageBug()
        {
            WebServerModule2.OF_BugMessageInfo LnqBugInfo = new WebServerModule2.OF_BugMessageInfo();

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
        /// <returns>返回字符串形式的客观故障信息</returns>
        private string GetCarSecondBug()
        {
            string CarSecondBug = "";

            foreach (Control cl in this.panel3.Controls)
            {
                if (cl is CheckBox)
                {
                    if (((CheckBox)cl).Checked)
                    {
                        CarSecondBug = cl.Text + ";";
                    }
                }
            }

            if (CarSecondBug == "")
            {
                return CarSecondBug;
            }
            else
                return CarSecondBug.Substring(0, CarSecondBug.Length - 1);
        }

        /// <summary>
        /// 获得主观故障信息
        /// </summary>
        /// <returns>返回字符串形式的主观故障信息</returns>
        private string GetCarMainBug()
        {
            string CarMainBug = "";

            foreach (Control cl in this.panel2.Controls)
            {
                if (cl is CheckBox)
                {
                    if (((CheckBox)cl).Checked)
                    {
                        CarMainBug = cl.Text + ";";
                    }
                }
            }

            if (CarMainBug == "")
            {
                return CarMainBug;
            }
            else
                return CarMainBug.Substring(0, CarMainBug.Length - 1);
        }

        /// <summary>
        /// 获得故障代码
        /// </summary>
        /// <returns>返回字符串形式的故障代码</returns>
        private string GetBugCode()
        {
            string BugCode = "";

            foreach (Control cl in this.panel5.Controls)
            {
                if (cl is CheckBox)
                {
                    if (((CheckBox)cl).Checked)
                    {
                        BugCode += cl.Text + ";";
                    }
                }
            }

            if (BugCode == "")
            {
                return BugCode;
            }
            else
                return BugCode.Substring(0, BugCode.Length - 1);
        }

        /// <summary>
        /// 初始化获得故障信息
        /// </summary>
        private void GetMessageLoad()
        {
            DataTable kfBugDt = m_serverFeedBack.GetBugMessageByServiceID(txtServiceID.Text);

            if (kfBugDt.Rows.Count > 0)
            {
                //ddlBugCode.Text = FeedBack.GetBugCodeByName(Convert.ToInt32(FKBugDt.Rows[0]["BugCode"].ToString()))["BugName"].ToString();

                cbFrequency.Text = kfBugDt.Rows[0]["Frequency"].ToString();
                cbCVTOil.Text = kfBugDt.Rows[0]["CVTOilDetection"].ToString();
                txtBugDescribe.Text = kfBugDt.Rows[0]["BugDeclare"].ToString();

                if (kfBugDt.Rows[0]["Condition"].ToString().Equals("冷车"))
                {
                    cbCondition.SelectedIndex = 0;
                    txtOther.Visible = false;
                }
                else if (kfBugDt.Rows[0]["Condition"].ToString().Equals("热车"))
                {
                    cbCondition.SelectedIndex = 1;
                    txtOther.Visible = false;
                }
                else if (kfBugDt.Rows[0]["Condition"].ToString().Equals("低速（60码以下）"))
                {
                    cbCondition.SelectedIndex = 2;
                    txtOther.Visible = false;
                }
                else if (kfBugDt.Rows[0]["Condition"].ToString().Equals("高速（60码以上）"))
                {
                    cbCondition.SelectedIndex = 3;
                    txtOther.Visible = false;
                }
                else if (kfBugDt.Rows[0]["Condition"].ToString().Equals("行驶过程中突然出现"))
                {
                    cbCondition.SelectedIndex = 4;
                    txtOther.Visible = false;
                }
                else
                {
                    cbCondition.SelectedIndex = 5;
                    txtOther.Text = kfBugDt.Rows[0]["Condition"].ToString();
                    txtOther.Visible = true;
                }

                if (kfBugDt.Rows[0]["PressureSensor"].ToString().Equals("合格"))
                {
                    cbPressure.SelectedIndex = 0;
                    txtPressure.Visible = false;
                }
                else if (kfBugDt.Rows[0]["PressureSensor"].ToString().Equals("未知"))
                {
                    cbPressure.SelectedIndex = 2;
                    txtPressure.Visible = false;
                }
                else
                {
                    cbPressure.SelectedIndex = 1;
                    txtPressure.Text = kfBugDt.Rows[0]["PressureSensor"].ToString();
                    txtPressure.Visible = true;
                }

                if (kfBugDt.Rows[0]["ActiveSensor"].ToString().Equals("合格"))
                {
                    cbActive.SelectedIndex = 0;
                    txtActive.Visible = false;
                }
                else if (kfBugDt.Rows[0]["ActiveSensor"].ToString().Equals("未知"))
                {
                    cbActive.SelectedIndex = 2;
                    txtActive.Visible = false;
                }
                else
                {
                    cbActive.SelectedIndex = 1;
                    txtActive.Text = kfBugDt.Rows[0]["ActiveSensor"].ToString();
                    txtActive.Visible = true;
                }

                if (kfBugDt.Rows[0]["PassivitySensor"].ToString().Equals("合格"))
                {
                    cbPassivity.SelectedIndex = 0;
                    txtPassivity.Visible = false;
                }
                else if (kfBugDt.Rows[0]["PassivitySensor"].ToString().Equals("未知"))
                {
                    cbPassivity.SelectedIndex = 2;
                    txtPassivity.Visible = false;
                }
                else
                {
                    cbPassivity.SelectedIndex = 1;
                    txtPassivity.Text = kfBugDt.Rows[0]["PassivitySensor"].ToString();
                    txtPassivity.Visible = true;
                }

                if (kfBugDt.Rows[0]["ShiftKnob"].ToString().Equals("合格"))
                {
                    cbKnob.SelectedIndex = 0;
                    txtKnob.Visible = false;
                }
                else if (kfBugDt.Rows[0]["ShiftKnob"].ToString().Equals("未知"))
                {
                    cbKnob.SelectedIndex = 2;
                    txtKnob.Visible = false;
                }
                else
                {
                    cbKnob.SelectedIndex = 1;
                    txtKnob.Text = kfBugDt.Rows[0]["ShiftKnob"].ToString();
                    txtKnob.Visible = true;
                }

                if (kfBugDt.Rows[0]["OverLinkStatus"].ToString().Equals("合格"))
                {
                    cbOverLink.SelectedIndex = 0;
                    txtOverLink.Visible = false;
                }
                else if (kfBugDt.Rows[0]["OverLinkStatus"].ToString().Equals("未知"))
                {
                    cbOverLink.SelectedIndex = 2;
                    txtOverLink.Visible = false;
                }
                else
                {
                    cbOverLink.SelectedIndex = 1;
                    txtOverLink.Text = kfBugDt.Rows[0]["OverLinkStatus"].ToString();
                    txtOverLink.Visible = true;
                }

                if (kfBugDt.Rows[0]["OilSump"].ToString().Equals("合格"))
                {
                    cbOilSump.SelectedIndex = 0;
                    txtOilSump.Visible = false;
                }
                else if (kfBugDt.Rows[0]["OilSump"].ToString().Equals("未知"))
                {
                    cbOilSump.SelectedIndex = 2;
                    txtOilSump.Visible = false;
                }
                else
                {
                    cbOilSump.SelectedIndex = 1;
                    txtOilSump.Text = kfBugDt.Rows[0]["OilSump"].ToString();
                    txtOilSump.Visible = true;
                }

                if (kfBugDt.Rows[0]["PKey"].ToString().Equals("服务站未检查"))
                {
                    cbPKey.Checked = true;
                    txtPKey.ReadOnly = true;
                }
                else
                {
                    txtPKey.Text = kfBugDt.Rows[0]["PKey"].ToString();
                    cbPKey.Enabled = false;
                }

                if (kfBugDt.Rows[0]["RKey"].ToString().Equals("服务站未检查"))
                {
                    cbRKey.Checked = true;
                    txtRKey.ReadOnly = true;
                }
                else
                {
                    txtRKey.Text = kfBugDt.Rows[0]["RKey"].ToString();
                    cbRKey.Enabled = false;
                }

                if (kfBugDt.Rows[0]["NKey"].ToString().Equals("服务站未检查"))
                {
                    cbNKey.Checked = true;
                    txtNKey.ReadOnly = true;
                }
                else
                {
                    txtNKey.Text = kfBugDt.Rows[0]["NKey"].ToString();
                    cbNKey.Enabled = false;
                }

                if (kfBugDt.Rows[0]["DKey"].ToString().Equals("服务站未检查"))
                {
                    cbDKey.Checked = true;
                    txtDKey.ReadOnly = true;
                }
                else
                {
                    txtDKey.Text = kfBugDt.Rows[0]["DKey"].ToString();
                    cbDKey.Enabled = false;
                }

                if (kfBugDt.Rows[0]["SKey"].ToString().Equals("服务站未检查"))
                {
                    cbSKey.Checked = true;
                    txtSKey.ReadOnly = true;
                }
                else
                {
                    txtSKey.Text = kfBugDt.Rows[0]["SKey"].ToString();
                    cbSKey.Enabled = false;
                }

                if (kfBugDt.Rows[0]["CarMainBug"].ToString().Contains("其他"))
                {
                    checkBox39.Checked = true;
                }

                if (kfBugDt.Rows[0]["CarMainBug"].ToString().Contains("抖动"))
                {
                    checkBox1.Checked = true;
                }

                if (kfBugDt.Rows[0]["CarMainBug"].ToString().Contains("异响"))
                {
                    checkBox2.Checked = true;
                }

                if (kfBugDt.Rows[0]["CarMainBug"].ToString().Contains("噪音大"))
                {
                    checkBox3.Checked = true;
                }

                if (kfBugDt.Rows[0]["CarMainBug"].ToString().Contains("油耗高"))
                {
                    checkBox4.Checked = true;
                }

                if (kfBugDt.Rows[0]["CarMainBug"].ToString().Contains("换挡冲击"))
                {
                    checkBox5.Checked = true;
                }

                if (kfBugDt.Rows[0]["CarMainBug"].ToString().Contains("加速慢"))
                {
                    checkBox6.Checked = true;
                }

                if (kfBugDt.Rows[0]["CarSecendBug"].ToString().Contains("其他"))
                {
                    checkBox40.Checked = true;
                }

                if (kfBugDt.Rows[0]["CarSecendBug"].ToString().Contains("前进挡无反应"))
                {
                    checkBox7.Checked = true;
                }

                if (kfBugDt.Rows[0]["CarSecendBug"].ToString().Contains("倒挡无反应"))
                {
                    checkBox8.Checked = true;
                }

                if (kfBugDt.Rows[0]["CarSecendBug"].ToString().Contains("挂档无反应"))
                {
                    checkBox9.Checked = true;
                }

                if (kfBugDt.Rows[0]["CarSecendBug"].ToString().Contains("亮故障灯"))
                {
                    checkBox10.Checked = true;
                }

                if (kfBugDt.Rows[0]["CarSecendBug"].ToString().Contains("加速不起（不变速）"))
                {
                    checkBox11.Checked = true;
                }

                if (kfBugDt.Rows[0]["CarSecendBug"].ToString().Contains("漏油"))
                {
                    checkBox12.Checked = true;
                }

                if (kfBugDt.Rows[0]["CarSecendBug"].ToString().Contains("车速表掉转速"))
                {
                    checkBox13.Checked = true;
                }

                if (kfBugDt.Rows[0]["CarSecendBug"].ToString().Contains("刹车熄火"))
                {
                    checkBox14.Checked = true;
                }

                if (kfBugDt.Rows[0]["CarSecendBug"].ToString().Contains("无挡位显示"))
                {
                    checkBox15.Checked = true;
                }

                if (kfBugDt.Rows[0]["CarSecendBug"].ToString().Contains("P挡拔不出"))
                {
                    checkBox16.Checked = true;
                }

                if (kfBugDt.Rows[0]["CarSecendBug"].ToString().Contains("P挡锁不住"))
                {
                    checkBox17.Checked = true;
                }

                if (kfBugDt.Rows[0]["BugCode"].ToString().Contains("无故障码"))
                {
                    checkBox18.Checked = true;
                }

                if (kfBugDt.Rows[0]["BugCode"].ToString().Contains("P0703"))
                {
                    checkBox19.Checked = true;
                }

                if (kfBugDt.Rows[0]["BugCode"].ToString().Contains("P0705"))
                {
                    checkBox20.Checked = true;
                }

                if (kfBugDt.Rows[0]["BugCode"].ToString().Contains("P0710"))
                {
                    checkBox21.Checked = true;
                }

                if (kfBugDt.Rows[0]["BugCode"].ToString().Contains("P0715"))
                {
                    checkBox22.Checked = true;
                }

                if (kfBugDt.Rows[0]["BugCode"].ToString().Contains("P0720"))
                {
                    checkBox23.Checked = true;
                }

                if (kfBugDt.Rows[0]["BugCode"].ToString().Contains("P0725"))
                {
                    checkBox24.Checked = true;
                }

                if (kfBugDt.Rows[0]["BugCode"].ToString().Contains("P0730"))
                {
                    checkBox38.Checked = true;
                }

                if (kfBugDt.Rows[0]["BugCode"].ToString().Contains("P0744"))
                {
                    checkBox37.Checked = true;
                }

                if (kfBugDt.Rows[0]["BugCode"].ToString().Contains("P0745"))
                {
                    checkBox36.Checked = true;
                }

                if (kfBugDt.Rows[0]["BugCode"].ToString().Contains("P0750"))
                {
                    checkBox35.Checked = true;
                }

                if (kfBugDt.Rows[0]["BugCode"].ToString().Contains("P0755"))
                {
                    checkBox34.Checked = true;
                }

                if (kfBugDt.Rows[0]["BugCode"].ToString().Contains("P0760"))
                {
                    checkBox33.Checked = true;
                }

                if (kfBugDt.Rows[0]["BugCode"].ToString().Contains("P1706"))
                {
                    checkBox32.Checked = true;
                }

                if (kfBugDt.Rows[0]["BugCode"].ToString().Contains("P1707"))
                {
                    checkBox31.Checked = true;
                }

                if (kfBugDt.Rows[0]["BugCode"].ToString().Contains("P1708"))
                {
                    checkBox30.Checked = true;
                }

                if (kfBugDt.Rows[0]["BugCode"].ToString().Contains("P1709"))
                {
                    checkBox29.Checked = true;
                }

                if (kfBugDt.Rows[0]["BugCode"].ToString().Contains("P1710"))
                {
                    checkBox28.Checked = true;
                }

                if (kfBugDt.Rows[0]["BugCode"].ToString().Contains("P1711"))
                {
                    checkBox27.Checked = true;
                }

                if (kfBugDt.Rows[0]["BugCode"].ToString().Contains("P1712"))
                {
                    checkBox26.Checked = true;
                }

                if (kfBugDt.Rows[0]["BugCode"].ToString().Contains("无法检查"))
                {
                    checkBox25.Checked = true;
                }

            }
            else
            {
                return;
            }
        }
        #endregion

        private void txtUserName_OnCompleteSearch()
        {
            if (txtUserName.DataResult != null)
            {
                txtCVTCode.Text = txtUserName.DataResult["CVT型号"].ToString();
                txtCVTID.Text = txtUserName.DataResult["CVT编号"].ToString();
                txtChassisNum.Text = txtUserName.DataResult["车架号"].ToString();
                dtpBuyCarTime.Value = Convert.ToDateTime(txtUserName.DataResult["销售日期"].ToString());
                txtUserTel.Text = txtUserName.DataResult["联系电话"].ToString();
                txtUserName.Text = txtUserName.DataResult["客户名称"].ToString();
                txtCarModel.Text = txtUserName.DataResult["车型"].ToString();
                txtUserAddress.Text = txtUserName.DataResult["详细地址"].ToString();
            }
            else
            {
                txtCVTCode.Text = "";
                txtCVTID.Text = "";
                txtChassisNum.Text = "";
                dtpBuyCarTime.Value = ServerTime.Time;
                txtUserTel.Text = "";
                txtUserName.Text = "";
                txtCarModel.Text = "";
                txtUserAddress.Text = "";
            }
        }

        private void txtProcessName_OnCompleteSearch()
        {
            txtProcessName.Text = txtProcessName.DataResult["姓名"].ToString();
            txtProcessName.Tag = txtProcessName.DataResult["工号"].ToString();
        }

        private void txtOldName_OnCompleteSearch()
        {
            txtOldCode.Text = txtOldName.DataResult["图号型号"].ToString();
            txtOldCode.Tag = txtOldName.DataResult["序号"].ToString();
            txtOldName.Text = txtOldName.DataResult["物品名称"].ToString();
            txtOldSpec.Text = txtOldName.DataResult["规格"].ToString();
            txtUnit.Text = txtOldName.DataResult["单位"].ToString();
        }

        private void txtNewName_OnCompleteSearch()
        {
            txtNewCode.Text = txtNewName.DataResult["图号型号"].ToString();
            txtNewCode.Tag = txtNewName.DataResult["序号"].ToString();
            txtNewSpec.Text = txtNewName.DataResult["规格"].ToString();
            txtNewName.Text = txtNewName.DataResult["物品名称"].ToString();
            txtUnit.Text = txtNewName.DataResult["单位"].ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtOldName.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择返回件！");
                return;
            }

            if (txtNum.Value == 0)
            {
                MessageDialog.ShowPromptMessage("数量不能为0！");
                return;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtOldCvtID.Text) || GlobalObject.GeneralFunction.IsNullOrEmpty(txtNewCvtID.Text))
            {
                MessageDialog.ShowPromptMessage("请填写【总成编号】！");
                return;
            }

            if (CheckSameGoods())
            {
                DataRow dr = m_dtList.NewRow();

                dr["ServiceID"] = txtServiceID.Text;
                dr["OldGoodsName"] = txtOldName.Text;
                dr["OldGoodsCode"] = txtOldCode.Text;
                dr["OldSpec"] = txtOldSpec.Text;
                dr["OldGoodsID"] = txtOldGoodsID.Text;
                dr["OldCvtID"] = txtOldCvtID.Text;
                dr["NewGoodsName"] = txtNewName.Text;
                dr["NewGoodsCode"] = txtNewCode.Text;
                dr["NewSpec"] = txtNewSpec.Text;
                dr["NewGoodsID"] = txtNewGoodsID.Text;
                dr["NewCvtID"] = txtNewCvtID.Text;
                dr["Count"] = txtNum.Value.ToString();
                dr["Unit"] = txtUnit.Text;
                m_dtList.Rows.Add(dr);

                dataGridView1.DataSource = m_dtList;

                IProductListServer serverProductList = ServerModule.ServerModuleFactory.GetServerModule<IProductListServer>();

                if (Convert.ToBoolean(UniversalFunction.GetGoodsAttributeInfo(Convert.ToInt32( txtOldCode.Tag.ToString()), 
                    CE_GoodsAttributeName.TCU)))
                {
                    txtNewSoftware.Text = txtOldGoodsID.Text;
                }

                ClearControl();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            m_dtList = (DataTable)dataGridView1.DataSource;

            if (m_dtList.Rows.Count != 0)
            {
                m_dtList.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                dataGridView1.DataSource = m_dtList;

                ClearControl();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtOldName.Text = dataGridView1.CurrentRow.Cells["OldGoodsName"].Value.ToString();
            txtOldSpec.Text = dataGridView1.CurrentRow.Cells["OldSpec"].Value.ToString();
            txtOldCode.Text = dataGridView1.CurrentRow.Cells["OldGoodsCode"].Value.ToString();
            txtOldCode.Tag = UniversalFunction.GetGoodsID(txtOldCode.Text, txtOldName.Text, txtOldSpec.Text);

            txtNewCode.Text = dataGridView1.CurrentRow.Cells["NewGoodsCode"].Value.ToString();
            txtNewSpec.Text = dataGridView1.CurrentRow.Cells["NewSpec"].Value.ToString();
            txtNewName.Text = dataGridView1.CurrentRow.Cells["NewGoodsName"].Value.ToString();
            txtNewCode.Tag = UniversalFunction.GetGoodsID(txtNewCode.Text, txtNewName.Text, txtNewSpec.Text);

            txtOldCvtID.Text = dataGridView1.CurrentRow.Cells["OldCvtID"].Value.ToString();
            txtNewCvtID.Text = dataGridView1.CurrentRow.Cells["NewCvtID"].Value.ToString();
            txtOldGoodsID.Text = dataGridView1.CurrentRow.Cells["OldGoodsID"].Value.ToString();
            txtNewGoodsID.Text = dataGridView1.CurrentRow.Cells["NewGoodsID"].Value.ToString();

            if (dataGridView1.CurrentRow.Cells["Count"].Value.ToString().Trim() == "" || dataGridView1.CurrentRow.Cells["Count"].Value.ToString() == null)
            {
                txtNum.Value = 0;
            }
            else
                txtNum.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["Count"].Value.ToString());

            txtUnit.Text = dataGridView1.CurrentRow.Cells["Unit"].Value.ToString();
        }

        private void 提交toolStripButton1_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text.Equals("新建单据") || txtStatus.Text.Equals("等待接单") || txtStatus.Text.Equals("等待结果确认"))
            {
                if (txtStatus.Text.Equals("等待接单"))
                {
                    if (BasicInfo.LoginID != txtApplicant.Tag.ToString())
                    {
                        MessageDialog.ShowPromptMessage("您没有权限进行此操作 ！");
                        return;
                    }
                }

                #region 检测控件的正确性

                if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtCVTID.Text))
                {
                    MessageDialog.ShowPromptMessage("请【总成编号】 ！");
                    return;
                }

                if (cmbMessageSource.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请选择信息来源 ！");
                    return;
                }

                if (txtCarModel.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请选择车型 ！");
                    return;
                }

                if (txtSiteName.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请选择服务站 ！");
                    return;
                }

                if (cmbMessageSource.SelectedIndex == 2)
                {
                    if (txtUserName.Text == "")
                    {
                        MessageDialog.ShowPromptMessage("请选择用户信息 ！");
                        return;
                    }
                }

                if (txtChassisNum.Text == "")
                {
                    MessageDialog.ShowPromptMessage("请选择车架号及变速箱信息！");
                    return;
                }

                if (txtProcessName.Text == "")
                {
                    MessageDialog.ShowPromptMessage("请选择接单人！");
                    return;
                }

                if (!dtpAcceptTime.Checked)
                {
                    MessageDialog.ShowPromptMessage("请选择接函电时间！");
                    return;
                }

                if (txtServiceIdea.Text == "")
                {
                    MessageDialog.ShowPromptMessage("请填写客户中心意见！");
                    return;
                }

                if (GetCarMainBug() == "" || GetCarSecondBug() == "")
                {
                    MessageDialog.ShowPromptMessage("请选择主观故障和客观故障！");
                    return;
                }

                if (GetBugCode() == "")
                {
                    MessageDialog.ShowPromptMessage("请选择故障代码！");
                    return;
                }

                if (cbFrequency.Text == "")
                {
                    MessageDialog.ShowPromptMessage("请选择故障频次！");
                    return;
                }

                if ((checkBox11.Checked || checkBox13.Checked) && txtBugDescribe.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("“加速不起”请注明最高可达到的车速；“车速表掉转速”请注明什么车速下掉转速，发动机转速上升到多少转！");
                    return;
                }

                if (rbHelpY.Checked && txtHelMoney.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写救援金额！");
                    return;
                }

                if (cbPKey.Checked == false && txtPKey.Text.ToString().Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写压力测试P档的结果");
                    return;
                }

                if (cbRKey.Checked == false && txtRKey.Text.ToString().Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写压力测试R档的结果");
                    return;
                }

                if (cbNKey.Checked == false && txtNKey.Text.ToString().Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写压力测试N档的结果");
                    return;
                }

                if (cbDKey.Checked == false && txtDKey.Text.ToString().Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写压力测试D档的结果");
                    return;
                }

                if (cbSKey.Checked == false && txtSKey.Text.ToString().Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写压力测试S档的结果");
                    return;
                }

                if (cmbStatus.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请选择总成状态！");
                    return;
                }

                if (!dtpCustomerDate.Checked && !dtpNoticeDate.Checked && !dtpStrategyDate.Checked)
                {
                    MessageDialog.ShowPromptMessage("请选择处理意见提出时间或者快速反应时间！");
                    return;
                }

                #endregion

                m_lnqAfterService.IsCVTBug = false;
                bool b = m_serverFeedBack.InsertService(GetServiceInfo(), GetMessageBug(), out m_strErr);

                if (b)
                {
                    m_billMessageServer.SendNewFlowMessage(m_lnqAfterService.ServiceID,
                        string.Format("{0} 号售后函电，请售后人员处理", m_lnqAfterService.ServiceID),BillFlowMessage_ReceivedUserType.用户,
                        m_personnerServer.GetPersonnelViewInfoByName(txtProcessName.Text));

                    MessageDialog.ShowPromptMessage("提交成功！");
                    this.Close();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
            }
        }

        private void txtProcessName_Enter(object sender, EventArgs e)
        {
            string sql = "";
            sql += " and Dept like '" + m_serverDepartment.GetDepartmentCode("营销服务部") + "%'";
            txtProcessName.StrEndSql = sql;
        }

        #region 控件的选择事件
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

        #endregion

        private void txtChassisNum_OnCompleteSearch()
        {
            txtCVTCode.Text = txtChassisNum.DataResult["CVT型号"].ToString();
            txtCVTID.Text = txtChassisNum.DataResult["CVT编号"].ToString();
            txtChassisNum.Text = txtChassisNum.DataResult["车架号"].ToString();
            txtCarModel.Text = txtChassisNum.DataResult["车型号"].ToString();
        }

        private void txtSiteName_OnCompleteSearch()
        {
            txtSiteName.Tag = txtSiteName.DataResult["客户编码"].ToString();
            txtSiteName.Text = txtSiteName.DataResult["客户名称"].ToString();
            txtLinkTel.Text = txtSiteName.DataResult["联系电话"].ToString();
        }

        private void 售后提交toolStripButton2_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text.Equals("等待接单"))
            {
                if (BasicInfo.LoginID == txtProcessName.Tag.ToString())
                {
                    if (CheckControl())
                    {
                        if (cmbCVTBug.SelectedIndex == -1)
                        {
                            MessageDialog.ShowPromptMessage("请选择“是否CVT故障”");
                            return;
                        }

                        WebServerModule2.S_AfterService list = new WebServerModule2.S_AfterService();

                        list.ServiceID = txtServiceID.Text;
                        list.ProcessName = txtProcessName.Text;
                        list.ProcessMode = txtProcessMode.Text.Trim() == "" ? "由客户中心直接派单" : txtProcessMode.Text;
                        list.ProcessTime = dtpProcessTime.Value;
                        list.DiagnoseSituation = txtDiagnoseSituation.Text;
                        list.NewSanB = cbSanBao.Checked == true ? "是" : "否";

                        if (cmbCVTBug.Text == "是")
                        {
                            list.IsCVTBug = true;
                        }
                        else
                        {
                            list.IsCVTBug = false;
                        }

                        if (rbLocaleY.Checked)
                        {
                            list.LocaleProcess = "是";
                        }
                        else
                            list.LocaleProcess = "否";


                        if (m_serverFeedBack.UpdateSaleTable(list, m_dtList, GetMessageBug(),true, out m_strErr))
                        {
                            m_billMessageServer.PassFlowMessage(txtServiceID.Text,
                       string.Format("{0} 号售后函电，等待处理结果", txtServiceID.Text),
                       BillFlowMessage_ReceivedUserType.用户, txtProcessName.Tag.ToString());

                            MessageDialog.ShowPromptMessage("故障处理完成,等待主管审核！");
                            this.Close();
                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage(m_strErr);
                        }
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("您不是指定的接单人，不能进行此操作");
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
            }
        }

        private void 审核toolStripButton3_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text.Equals("等待审核"))
            {
                if (txtStatus.Text.Equals("等待回访") || txtStatus.Text.Equals("处理完成"))
                {
                    MessageDialog.ShowPromptMessage("【" + txtServiceID.Text + "】号单据主管已经审核，不能进行此操作！)");
                    return;
                }

                string billNo = m_serverFeedBack.GetNextBillID(2);

                if (lbFKBillID.Text == "是")
                {
                    if (MessageBox.Show("是否生成售后反馈单？", "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        lbFKBillID.Text = "否";
                    }
                }
                else
                {
                    if (MessageBox.Show("是否生成售后反馈单吗？", "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        lbFKBillID.Text = "是";
                    }
                }

                bool b = m_serverFeedBack.UpdateCheckTable(txtServiceID.Text, BasicInfo.LoginID,
                    dtpCheckTime.Value.ToString(), lbFKBillID.Text, GetServiceInfo(), GetMessageBug(),
                    txtSolution.Text + " 结果：" + txtProcessResult.Text, txtDiagnoseSituation.Text, out m_strErr);

                if (b)
                {
                    m_billMessageServer.PassFlowMessage(m_lnqAfterService.ServiceID,
                        string.Format("{0} 号售后函电，请售后回访人员回访", m_lnqAfterService.ServiceID),
                        CE_RoleEnum.营销售后客户回访人员.ToString(), true);

                    if (lbFKBillID.Text.Equals("是"))
                    {
                        DataTable tempTable = m_serverFeedBack.GetReplaceByID(txtServiceID.Text);
                        m_billMessageServer.BillType = "售后质量信息反馈单";

                        if (tempTable != null && tempTable.Rows.Count > 0)
                        {
                            m_billMessageServer.SendNewFlowMessage(billNo,
                                    string.Format("【车型】：{0} 【CVT型号】: {1} 【CVT箱号】：{2} 【车架号】：{3}   ※※※ 等待【{4}】处理",
                                    txtCarModel.Text, txtCVTCode.Text, txtCVTID.Text, txtChassisNum.Text, UniversalFunction.GetPersonnelName("0025")),
                                    BillFlowMessage_ReceivedUserType.用户, "0025");
                        }
                        else
                        {
                            m_billMessageServer.SendNewFlowMessage(billNo,
                                    string.Format("【车型】：{0} 【CVT型号】: {1} 【CVT箱号】：{2} 【车架号】：{3}   ※※※ 等待【{4}】处理",
                                    txtCarModel.Text, txtCVTCode.Text, txtCVTID.Text, txtChassisNum.Text, CE_RoleEnum.营销主管.ToString()),
                                    BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.营销主管.ToString());
                        }
                    }
                    
                    MessageDialog.ShowPromptMessage("审核成功！");
                    this.Close();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
            }
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            lbFKBillID.Text = "是";
        }

        private void 回访确认toolStripButton4_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text.Equals("等待回访"))
            {
                if (txtRepairQuality.Text.Trim() == "" || txtServiceAttitude.Text.Trim() == ""
                   || txtFailureResults.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写完整的回访信息！");
                    return;
                }

                WebServerModule2.S_AfterService list = new WebServerModule2.S_AfterService();

                list.ServiceID = txtServiceID.Text;
                list.RepairQuality = txtRepairQuality.Text;
                list.ServiceAttitude = txtServiceAttitude.Text;

                if (rbIsCharge.Checked && txtAmount.Text.Equals("0"))
                {
                    MessageDialog.ShowPromptMessage("收取的费用不应该为0");
                    return;
                }

                if (rbIsCharge.Checked)
                {
                    list.IsCharge = rbIsCharge.Text;
                }
                else
                {
                    list.IsCharge = rbIsChargeNot.Text;
                }

                list.Amount = Convert.ToDecimal(txtAmount.Text);
                list.FailureResults = txtFailureResults.Text;
                list.ReturnName = BasicInfo.LoginID;
                list.ReturnTime = dtpReturnTime.Value;
                list.Status = "处理完成";

                if (m_serverFeedBack.UpdateResultTable(list, out m_strErr))
                {
                    List<string> noticeRoles = new List<string>();
                    noticeRoles.Add(CE_RoleEnum.营销普通人员.ToString());
                    noticeRoles.Add(CE_RoleEnum.营销主管.ToString());

                    m_billMessageServer.EndFlowMessage(list.ServiceID,
                        string.Format("{0} 号售后函电已经处理完毕", list.ServiceID),
                        noticeRoles, null);

                    MessageDialog.ShowPromptMessage("回访完成，单据处理完成！");
                    this.Close();
                }
                else
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
            }
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            lbFKBillID.Text = "否";
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (txtServiceID.Text != "" || txtServiceID.Text != null)
            {
                IBillReportInfo report = new 报表_售后函电基本信息(txtServiceID.Text, "售后函电基本信息单");
                (report as 报表_售后函电基本信息).ShowDialog();
            }
            else
            {
                MessageDialog.ShowPromptMessage("单据未保存！");
            }
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

        private void 回退单据toolStripButton2_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text.Trim() != "处理完成")
            {
                回退单据 form = new 回退单据(CE_BillTypeEnum.售后函电处理单, txtServiceID.Text, txtStatus.Text);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (MessageBox.Show("您确定要回退此单据吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (m_serverFeedBack.ReturnBill(form.StrBillID,
                            form.StrBillStatus, out m_strErr, form.Reason))
                        {
                            MessageDialog.ShowPromptMessage("回退成功");

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

        private void txtAcceptName_Enter(object sender, EventArgs e)
        {
            string sql = "";
            sql += " and Dept like '" + m_serverDepartment.GetDepartmentCode("营销服务部") + "%'";
            txtAcceptName.StrEndSql = sql;
        }

        private void txtAcceptName_OnCompleteSearch()
        {
            txtAcceptName.Text = txtAcceptName.DataResult["姓名"].ToString();
            txtAcceptName.Tag = txtAcceptName.DataResult["工号"].ToString();
        }

        private void linkFile_Click(object sender, EventArgs e)
        {
            try
            {
                FileServiceSocket ftpServer = new FileServiceSocket("192.168.0.7:8100", "After_Sale_Service", "~Qaz_1_2_3!");

                bool flag = ftpServer.Connect();

                if (flag)
                {
                    FolderBrowserDialog folder = new FolderBrowserDialog();

                    if (folder.ShowDialog() == DialogResult.OK)
                    {
                        string filepath = folder.SelectedPath + "\\";

                        ftpServer.Download(m_fileName, filepath);

                        if (ftpServer.Errormessage.Length != 0)
                        {
                            MessageDialog.ShowPromptMessage(ftpServer.Errormessage);
                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage("下载成功,路径：" + filepath);
                        }
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("服务器连接失败！");
                }
            }
            catch (Exception)
            {
                MessageDialog.ShowPromptMessage("文件不存在或已经移除！");
            }            
        }

        private void 结果toolStripButton_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text.Equals("等待结果确认"))
            {
                if (BasicInfo.LoginID == txtProcessName.Tag.ToString())
                {
                    if (CheckControl())
                    {
                        if (txtSolution.Text.Trim() == "")
                        {
                            MessageDialog.ShowPromptMessage("请填写处理方案 ！");
                            return;
                        }

                        if (txtProcessResult.Text.Trim() == "")
                        {
                            MessageDialog.ShowPromptMessage("请填写处理结果");
                            return;
                        }

                        string siteCode = m_serverFeedBack.GetClient(txtSiteName.Text)["ClientCode"].ToString();

                        if (cbIsServiceStock.Checked && cbIsThreeGuarantees.Checked)
                        {
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                int goodsID = UniversalFunction.GetGoodsID((string)dataGridView1.Rows[i].Cells["NewGoodsCode"].Value,
                                        (string)dataGridView1.Rows[i].Cells["NewGoodsName"].Value, (string)dataGridView1.Rows[i].Cells["NewSpec"].Value);

                                DataTable dt = m_serverFeedBack.GetStockNum(siteCode, goodsID.ToString());

                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    if (Convert.ToInt32(dt.Rows[0]["StockQty"]) <= 0)
                                    {
                                        MessageDialog.ShowPromptMessage((string)dataGridView1.Rows[i].Cells["NewGoodsCode"].Value + " "
                                            + (string)dataGridView1.Rows[i].Cells["NewGoodsName"].Value + "库存数为0，请先提交调货！");
                                        return;
                                    }
                                }
                                else
                                {
                                    MessageDialog.ShowPromptMessage((string)dataGridView1.Rows[i].Cells["NewGoodsCode"].Value + " "
                                            + (string)dataGridView1.Rows[i].Cells["NewGoodsName"].Value + "库存数为0，请先提交调货！");
                                    return;
                                }
                            }
                        }

                        WebServerModule2.S_AfterService list = new WebServerModule2.S_AfterService();

                        list.ServiceID = txtServiceID.Text;
                        list.ProcessMode = txtProcessMode.Text.Trim() == "" ? "由客户中心直接派单" : txtProcessMode.Text;
                        list.DiagnoseSituation = txtDiagnoseSituation.Text;
                        list.NewSanB = cbSanBao.Checked == true ? "是" : "否";
                        list.Solution = txtSolution.Text;
                        list.ProcessResult = txtProcessResult.Text;

                        if (cmbCVTBug.Text == "是")
                        {
                            list.IsCVTBug = true;
                        }
                        else
                        {
                            list.IsCVTBug = false;
                        }

                        if (rbLocaleY.Checked)
                        {
                            list.LocaleProcess = "是";
                        }
                        else
                            list.LocaleProcess = "否";

                        if (m_serverFeedBack.UpdateSaleTable(list, m_dtList, GetMessageBug(),false, out m_strErr))
                        {
                            m_billMessageServer.PassFlowMessage(txtServiceID.Text,
                                string.Format("{0} 号售后函电，请营销主管审核", txtServiceID.Text), CE_RoleEnum.营销主管.ToString(), true);

                            MessageDialog.ShowPromptMessage("故障处理完成,等待主管审核！");
                            this.Close();
                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage(m_strErr);
                        }
                    }
                }
            }
        }
    }
}
