using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 回退单据界面
    /// </summary>
    public partial class 回退单据 : Form
    {
        /// <summary>
        /// 领料单单据状态字符串数组
        /// </summary>
        string[] m_strLLD = { "新建单据", "等待主管审核", "等待出库" };

        /// <summary>
        /// 领料退库单单据状态字符串数组
        /// </summary>
        string[] m_strLTD = { "新建单据", "等待主管审核", "等待质检批准", "等待仓管退库" };

        /// <summary>
        /// 普通入库单单据状态字符串数组
        /// </summary>
        string[] m_strPRD = { "新建单据", "等待质检", "等待工装验证", "等待入库" };

        /// <summary>
        /// 采购退货单单据状态字符串数组
        /// </summary>
        string[] m_strCTD = { "新建单据", "等待财务审核", "等待仓管退货" };

        /// <summary>
        /// 报废单单据状态字符串数组
        /// </summary>
        string[] m_strBFD = { "新建单据", "等待主管审核", "等待质检批准", "等待仓管确认" };

        /// <summary>
        /// 挑返单单据状态字符串数组
        /// </summary>
        string[] m_strTFD = { "新建单据", "等待主管审核", "等待处理结果", "等待检验结果" };

        /// <summary>
        /// 隔离单单据状态字符串数组
        /// </summary>
        string[] m_strGLD = { "新建单据", "等待主管审核", "等待仓管调出", "等待处理结果", "等待质检结果", "等待质管主管确认" };

        /// <summary>
        /// 质量信息反馈单单据状态字符串数组
        /// </summary>
        string[] m_strFKD = { "等待QC质量信息提交", "等待STA意见", "等待质管部意见", "等待SQE验证" };

        /// <summary>
        /// 样品确认申请单单据状态字符串数组
        /// </summary>
        string[] m_strYPD = { "新建单据", "等待主管审核", "等待仓管确认到货", "等待检验", "等待确认检验信息", "等待工艺工程师评审", "等待零件工程师评审", "等待项目经理确认", "等待试验结果", "等待主管确认", "等待SQE处理" };

        /// <summary>
        /// 委外报检入库单单据状态字符串数组
        /// </summary>
        string[] m_strWJD = { "新建单据", "等待财务批准", "等待仓管确认到货", "等待质检电检验", "等待质检机检验", "等待入库" };

        /// <summary>
        /// 工装验证报告单单据状态字符串数组
        /// </summary>
        string[] m_strGYD = { "等待检验要求", "等待检验", "等待结论" };

        /// <summary>
        /// 售后函电处理单据状态字符串数组
        /// </summary>
        string[] m_strSHD = { "等待接单", "等待审核", "等待回访" };

        /// <summary>
        /// 售后质量反馈单单据状态字符串数组
        /// </summary>
        string[] m_strSHFK = { "等待确认返回时间", "等待主管审核", "等待质管确认", "等待责任部门确认", "等待责任人确认", "等待质管检查" };

        /// <summary>
        /// 营销入库单单据状态字符串数组
        /// </summary>
        string[] m_strYXRK = { "已保存", "已审核" };

        /// <summary>
        /// 营销出库单单据状态字符串数组
        /// </summary>
        string[] m_strYXCK = { "已保存" };

        /// <summary>
        /// 营销退库单单据状态字符串数组
        /// </summary>
        string[] m_strYXTK = { "已保存", "已审核" };

        /// <summary>
        /// 营销退货库单单据状态字符串数组
        /// </summary>
        string[] m_strYXTH = { "已保存", "已审核", "已检验" };

        /// <summary>
        /// 销售清单单据状态数组
        /// </summary>
        string[] m_strXSQD = { "等待销售人员确认" };

        /// <summary>
        /// 物料扣货单单据状态字符串数组
        /// </summary>
        string[] m_strWLKH = { "新建单据", "等待领导审核", "等待质管批准", "等待SQE确认", "等待采购确认" };

        /// <summary>
        /// 文件审查流程状态数组
        /// </summary>
        string[] m_strFMRV = { "新建流程", "等待主管审核" };

        /// <summary>
        /// 文件审查流程状态数组
        /// </summary>
        string[] m_strFMRL = { "新建流程", "等待审核", "等待批准" };

        /// <summary>
        /// 文件审查流程状态数组
        /// </summary>
        string[] m_strFMRA = { "新建单据", "等待审核", "等待批准" };

        /// <summary>
        /// 技术变更处置单
        /// </summary>
        string[] m_strChange = { "新建单据", "等待批准" };

        /// <summary>
        /// 零星采购申请单
        /// </summary>
        string[] m_strMinor = { "新建单据", "等待部门负责人审核", "等待采购部调配人员" };

        /// <summary>
        /// 车间物料转换单
        /// </summary>
        string[] m_strCJZH = { 
                                 Service_Manufacture_WorkShop.MaterialsTransferStatus.新建单据.ToString() ,
                                 Service_Manufacture_WorkShop.MaterialsTransferStatus.等待审核.ToString() ,
                                 Service_Manufacture_WorkShop.MaterialsTransferStatus.等待确认.ToString() ,
                             };

        /// <summary>
        /// 车间调运单
        /// </summary>
        string[] m_strCJDY = { 
                                 Service_Manufacture_WorkShop.CannibalizeBillStatus.新建单据.ToString() ,
                                 Service_Manufacture_WorkShop.CannibalizeBillStatus.等待审核.ToString() ,
                                 Service_Manufacture_WorkShop.CannibalizeBillStatus.等待确认.ToString() ,
                             };

        /// <summary>
        /// 车间耗用单
        /// </summary>
        string[] m_strCJHY = { 
                                 Service_Manufacture_WorkShop.ConsumptionBillStatus.新建单据.ToString() ,
                                 Service_Manufacture_WorkShop.ConsumptionBillStatus.等待审核.ToString() ,
                                 Service_Manufacture_WorkShop.ConsumptionBillStatus.等待确认.ToString() ,
                             };

        /// <summary>
        /// 单据号
        /// </summary>
        private string m_strBillID = "";

        public string StrBillID
        {
            get { return m_strBillID; }
            set { m_strBillID = value; }
        }

        /// <summary>
        /// 单据状态
        /// </summary>
        private string m_strBillStatus = "";

        public string StrBillStatus
        {
            get { return m_strBillStatus; }
            set { m_strBillStatus = value; }
        }

        /// <summary>
        /// 索引值
        /// </summary>
        private object m_objValue;

        public object ObjValue
        {
            get { return m_objValue; }
            set { m_objValue = value; }
        }

        /// <summary>
        /// 单据类型
        /// </summary>
        private CE_BillTypeEnum m_enumType;

        /// <summary>
        /// 获取单据流消息操作接口
        /// </summary>
        IBillFlowMessage m_billFlowMsg = PlatformFactory.GetObject<IBillFlowMessage>();

        /// <summary>
        /// 窗体消息发布器
        /// </summary>
        WndMsgSender m_wndMsgSender = new WndMsgSender();

        /// <summary>
        /// 回退原因
        /// </summary>
        public string Reason
        {
            get { return txtReason.Text; }
        }

        public 回退单据(CE_BillTypeEnum billType, string billNo, string billStatus)
        {
            InitializeComponent();
            lbDJZT.Text = billStatus;
            m_enumType = billType;
            txtDJH.Text = billNo;
            m_strBillID = billNo;
            GetBillType();
        }

        public 回退单据(CE_BillTypeEnum billType, string billNo, string billStatus , List<Flow_FlowInfo> listFlowInfo)
        {
            InitializeComponent();
            lbDJZT.Text = billStatus;
            m_enumType = billType;
            txtDJH.Text = billNo;
            m_strBillID = billNo;

            cmbDJZT.DataSource = listFlowInfo;

            cmbDJZT.DisplayMember = "BusinessStatus";
            cmbDJZT.ValueMember = "FlowID";

            cmbDJZT.SelectedIndex = -1;
        }

        /// <summary>
        /// 插入ComBox
        /// </summary>
        private void GetBillType()
        {
            switch (m_enumType)
            {
                case CE_BillTypeEnum.领料单:
                    InsertComBox(m_strLLD);
                    break;
                case CE_BillTypeEnum.领料退库单:
                    InsertComBox(m_strLTD);
                    break;
                case CE_BillTypeEnum.普通入库单:
                    InsertComBox(m_strPRD);
                    break;
                case CE_BillTypeEnum.采购退货单:
                    InsertComBox(m_strCTD);
                    break;
                case CE_BillTypeEnum.报废单:
                    InsertComBox(m_strBFD);
                    break;
                case CE_BillTypeEnum.挑选返工返修单:
                    InsertComBox(m_strTFD);
                    break;
                case CE_BillTypeEnum.不合格品隔离处置单:
                    InsertComBox(m_strGLD);
                    break;
                case CE_BillTypeEnum.供应质量信息反馈单:
                    InsertComBox(m_strFKD);
                    break;
                case CE_BillTypeEnum.样品确认申请单:
                    InsertComBox(m_strYPD);
                    break;
                case CE_BillTypeEnum.委外报检入库单:
                    InsertComBox(m_strWJD);
                    break;
                case CE_BillTypeEnum.自制件退货单:
                    InsertComBox(m_strCTD);
                    break;
                case CE_BillTypeEnum.售后函电处理单:
                    InsertComBox(m_strSHD);
                    break;
                case CE_BillTypeEnum.售后服务质量反馈单:
                    InsertComBox(m_strSHFK);
                    break;
                case CE_BillTypeEnum.工装验证报告单:
                    InsertComBox(m_strGYD);
                    break;
                case CE_BillTypeEnum.营销入库单:
                    InsertComBox(m_strYXRK);
                    break;
                case CE_BillTypeEnum.营销出库单:
                    InsertComBox(m_strYXCK);
                    break;
                case CE_BillTypeEnum.营销退库单:
                    InsertComBox(m_strYXTK);
                    break;
                case CE_BillTypeEnum.营销退货单:
                    InsertComBox(m_strYXTH);
                    break;
                case CE_BillTypeEnum.销售清单:
                    InsertComBox(m_strXSQD);
                    break;
                case CE_BillTypeEnum.物料扣货单:
                    InsertComBox(m_strWLKH);
                    break;
                case CE_BillTypeEnum.文件审查流程:
                    InsertComBox(m_strFMRV);
                    break;
                case CE_BillTypeEnum.文件发布流程:
                    InsertComBox(m_strFMRL);
                    break;
                case CE_BillTypeEnum.文件修订废止申请单:
                    InsertComBox(m_strFMRA);
                    break;
                case CE_BillTypeEnum.技术变更处置单:
                    InsertComBox(m_strChange);
                    break;
                case CE_BillTypeEnum.零星采购单:
                    InsertComBox(m_strMinor);
                    break;
                case CE_BillTypeEnum.车间物料转换单:
                    InsertComBox(m_strCJZH);
                    break;
                case CE_BillTypeEnum.车间调运单:
                    InsertComBox(m_strCJDY);
                    break;
                case CE_BillTypeEnum.车间耗用单:
                    InsertComBox(m_strCJHY);
                    break;
                default:
                    break;
            }

        }

        private void InsertComBox(string[] str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i].ToString() == lbDJZT.Text)
                {
                    return;
                }

                cmbDJZT.Items.Add(str[i]);
            }

            cmbDJZT.SelectedIndex = -1;
        }

        #region 按钮事件

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cmbDJZT.Text == "" || cmbDJZT.Text.Trim().Length == 0 || cmbDJZT.Text == null)
            {
                cmbDJZT.Focus();
                MessageDialog.ShowPromptMessage("请选择回退状态");
                return;
            }

            if (txtReason.Text.Trim() == "")
            {
                txtReason.Focus();
                MessageDialog.ShowPromptMessage("请填写回退原因");
                return;
            }

            m_strBillStatus = cmbDJZT.Text;
            m_objValue = cmbDJZT.SelectedValue;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            m_strBillStatus = "";
            this.Close();
        }

        #endregion
    }
}
