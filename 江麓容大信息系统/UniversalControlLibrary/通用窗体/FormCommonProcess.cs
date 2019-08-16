using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GlobalObject;
using PlatformManagement;
using ServerModule;
using System.Reflection;
using FlowControlService;

namespace UniversalControlLibrary
{
    public partial class FormCommonProcess : BasicFormTool
    {
        CE_BillTypeEnum m_billType;
        CustomFlowForm m_customForm = new CustomFlowForm();
        CE_OperatorMode m_operationMode;
        string m_FlowBusinessVersion;

        IFlowServer m_serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();
        List<CommonProcessInfo> m_flowInfo = new List<CommonProcessInfo>();
        Dictionary<int, Dictionary<string, bool>> m_flowMagic = new Dictionary<int, Dictionary<string, bool>>();
        Flow_FlowBillData m_flowBillInfo = null;

        int CONTENT_HIGH;
        int PARALLEL_LENTHG = 6;
        int CONNECT_HIGH = 45;
        int BRANCH_DIFFERENCE_HIGH = 14;
        int DIFFERENCE_HIGH = 39;

        Point START_POINT = new Point();
        string CONNECT_TEXT = "______";
        string BRANCH_TEXT = "|\r\n|\r\n|";


        public delegate bool FormSubmit(CustomFlowForm form, string advise);
        public event FormSubmit CommonProcessSubmit;

        /// <summary>
        /// 构造函数    
        /// </summary>
        /// <param name="billType">单据类型</param>
        /// <param name="billNo">单据号</param>
        /// <param name="loadPanel">界面对象</param>
        /// <param name="flowInfo">流程信息，仅对自定义</param>
        /// <param name="flowMagic">流程逻辑，仅对自定义</param>
        /// <param name="operationMode">操作类型</param>
        public FormCommonProcess(CE_BillTypeEnum billType, string version, CustomFlowForm form, CE_OperatorMode operationMode)
        {
            InitializeComponent();

            m_customForm = form;
            m_billType = billType;
            m_operationMode = operationMode;
            m_FlowBusinessVersion = version;

            m_customForm.FormBorderStyle = FormBorderStyle.None;
            m_customForm.TopLevel = false;
            m_customForm.Show();

            m_customForm.Parent = panel6;
            m_customForm.Dock = DockStyle.Fill;
            m_customForm.AutoScroll = true;
        }

        private void FormCommonProcess_Load(object sender, EventArgs e)
        {
            LoadFormInfo();

            Flow_BusinessInfo temp = m_serverFlow.GetBusinessInfo(m_billType, m_FlowBusinessVersion);

            if (temp != null)
            {
                m_flowMagic = m_serverFlow.GetExcuteFlowInfo(m_customForm.FlowInfo_BillNo, temp.BusinessTypeID);
                ShowFormFlowMagic();
            }

            if (m_customForm.FlowInfo_BillNo == null || m_customForm.FlowInfo_BillNo == "")
            {
                return;
            }

            m_flowBillInfo = m_serverFlow.GetBillData(m_customForm.FlowInfo_BillNo);

            if (m_customForm.流程控制类型 == CE_FormFlowType.默认)
            {
                m_flowInfo = m_serverFlow.GetFlowData(m_customForm.FlowInfo_BillNo);
                ShowFormFlowInfo();
            }
        }

        public override void LoadFormInfo()
        {
            if (BusinessList_Object != null)
            {
                if (BusinessList_Object.GetType() == typeof(Entity_BusinessOperationInfo))
                {
                    Entity_BusinessOperationInfo businessInfo = BusinessList_Object as Entity_BusinessOperationInfo;

                    m_customForm.FlowInfo_BillNo = businessInfo.BusinessNo;
                    m_customForm.Custom_FlowInfo = businessInfo.FlowInfoList;
                    m_customForm.Custom_FlowMagic = businessInfo.FlowMagicDic;

                    m_billType = businessInfo.BillType;
                    m_operationMode = businessInfo.OperatorMode;
                }
            }

            m_flowInfo = m_customForm.Custom_FlowInfo;
            m_flowMagic = m_customForm.Custom_FlowMagic;

            labelTitle.Text = m_customForm.Text;
            this.Text = m_customForm.Text;

            if (m_operationMode != CE_OperatorMode.查看)
            {
                bool visible = PanelVisible(m_customForm.FlowInfo_BillNo);

                panel1.Visible = visible;
                panel3.Visible = visible;
            }
            else
            {
                panel1.Visible = false;
                panel3.Visible = false;
            }

            base.LoadFormInfo();
            m_customForm.LoadFormInfo();
        }

        void ShowFormFlowInfo()
        {
            if (m_flowInfo != null)
            {
                panel5.Controls.Clear();

                foreach (CommonProcessInfo item in m_flowInfo)
                {
                    string tempStr = item.意见.Substring(0, item.意见.IndexOf(" "));
                    string msgStr = item.意见.Substring(item.意见.IndexOf(" "));


                    Label templb = new Label();

                    templb.Text = msgStr;
                    templb.Dock = DockStyle.Top;

                    int a = msgStr.Length / 126;
                    int k = msgStr.Length % 126;

                    if (k > 0)
                    {
                        a++;
                    }

                    templb.Height = a * 15;
                    templb.BackColor = Color.White;

                    int LblNum = templb.Text.Length;   //Label内容长度
                    int RowNum = 10;           //每行显示的字数
                    float FontWidth = templb.Width / templb.Text.Length;    //每个字符的宽度
                    int RowHeight = 15;           //每行的高度
                    int ColNum = (LblNum - (LblNum / RowNum) * RowNum) == 0 ? (LblNum / RowNum) : (LblNum / RowNum) + 1;   //列数
                    templb.AutoSize = false;    //设置AutoSize
                    templb.Width = (int)(FontWidth * 10.0);          //设置显示宽度
                    templb.Height = RowHeight * ColNum;           //设置显示高度

                    panel5.Controls.Add(templb);


                    templb = new Label();

                    string tempTopText = "";

                    if (item.操作节点 != null && item.操作节点.Trim().Length > 0)
                    {
                        tempTopText = "【" + item.操作节点 + "】" + item.人员 + " " + tempStr + " " + item.时间;
                    }
                    else
                    {
                        tempTopText = item.人员 + " " + tempStr + " " + item.时间;
                    }

                    templb.Text = tempTopText;
                    templb.Dock = DockStyle.Top;
                    templb.Height = 15;
                    templb.BackColor = Color.Gainsboro;

                    panel5.Controls.Add(templb);
                }
            }
        }

        void ShowFormFlowMagic()
        {
            panel7.Controls.Clear();

            CONTENT_HIGH = CONNECT_HIGH + 6;
            START_POINT = new Point(26, CONTENT_HIGH);
            int tempLength = 0;

            foreach (KeyValuePair<int, Dictionary<string, bool>> item in m_flowMagic)
            {
                Dictionary<string, bool> dicTemp = item.Value;

                if (item.Key == 1)
                {
                    Label templb = GetLabel(START_POINT, dicTemp.Keys.First(), Color.Black);
                    panel7.Controls.Add(templb);
                    tempLength = START_POINT.X + templb.Size.Width;
                }
                else
                {
                    if (dicTemp.Count == 1)
                    {
                        AddSerialLable(dicTemp.Keys.First(), dicTemp.Values.First() ? Color.Black : Color.Blue, ref tempLength);
                    }
                    else
                    {
                        AddParallelLable(dicTemp, ref tempLength);
                    }
                }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btnSubmit.Enabled = false;
            this.btnHold.Enabled = false;
            this.btnReback.Enabled = false;

            try
            {
                if (!m_customForm.GetDateInfo(CE_FlowOperationType.提交))
                {
                    return;
                }

                CheckData();
                string judge = "";

                if (rbAgree.Checked)
                {
                    judge = rbAgree.Text;
                }

                if (rbDisagree.Checked)
                {
                    DisagreeOperation();
                    return;
                }

                if (rbIsread.Checked)
                {
                    judge = rbIsread.Text;
                }

                string advise = judge + " " + textBox1.Text;
                if (CommonProcessSubmit != null && CommonProcessSubmit(m_customForm, advise))
                {
                    if (m_customForm.流程控制类型 == CE_FormFlowType.默认)
                    {
                        if (m_customForm.FlowInfo_BillNo == null || m_customForm.FlowInfo_BillNo.Trim().Length == 0)
                        {
                            throw new Exception("单据号不能为空");
                        }

                        bool isParallel = false;
                        if (m_serverFlow.IsPointPersonnel(m_customForm.FlowInfo_BillNo, out isParallel)
                            && (m_customForm.FlowInfo_NotifyInfo.PersonnelBasicInfoList == null 
                            || m_customForm.FlowInfo_NotifyInfo.PersonnelBasicInfoList.Count() == 0))
                        {
                            if (!m_customForm.GetNotifyPersonnel(true))
                            {
                                throw new Exception("请选择指定人或者角色点【确定】");
                            }
                        }

                        m_serverFlow.FlowPass(m_customForm.FlowInfo_BillNo, advise, m_customForm.FlowInfo_StorageIDOrWorkShopCode,
                            m_customForm.FlowInfo_NotifyInfo, m_customForm.KeyWords);
                    }

                    MessageDialog.ShowPromptMessage("提交成功");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
            finally
            {
                this.btnSubmit.Enabled = true;
                this.btnHold.Enabled = true;
                this.btnReback.Enabled = true;
                this.Cursor = System.Windows.Forms.Cursors.Arrow;
            }
        }

        private void btnHold_Click(object sender, EventArgs e)
        {
            try
            {
                if (!m_customForm.GetDateInfo(CE_FlowOperationType.暂存))
                {
                    return;
                }

                CheckData();
                string advise = "暂存 " + textBox1.Text;
                if (CommonProcessSubmit != null && CommonProcessSubmit(m_customForm, advise))
                {
                    if (m_customForm.流程控制类型 == CE_FormFlowType.默认)
                    {
                        if (m_customForm.FlowInfo_BillNo == null || m_customForm.FlowInfo_BillNo.Trim().Length == 0)
                        {
                            throw new Exception("单据号不能为空");
                        }

                        m_serverFlow.FlowHold(m_customForm.FlowInfo_BillNo, m_customForm.FlowInfo_StorageIDOrWorkShopCode, advise, m_customForm.KeyWords);

                        MessageDialog.ShowPromptMessage("暂存成功");
                    }

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btnReback_Click(object sender, EventArgs e)
        {
            try
            {
                if (!m_customForm.GetDateInfo(CE_FlowOperationType.回退))
                {
                    return;
                }

                CheckData();
                if (m_customForm.流程控制类型 == CE_FormFlowType.默认)
                {
                    if (m_serverFlow.IsExist(m_customForm.FlowInfo_BillNo))
                    {
                        Flow_FlowInfo lnqFlowInfo =
                            m_serverFlow.GetNowFlowInfo(m_serverFlow.GetBusinessTypeID(m_billType, m_FlowBusinessVersion), m_customForm.FlowInfo_BillNo);

                        List<Flow_FlowInfo> listFlowInfo = (from a in m_serverFlow.GetListFlowInfo(m_customForm.FlowInfo_BillNo)
                                                            where a.FlowOrder < lnqFlowInfo.FlowOrder
                                                            orderby a.FlowOrder
                                                            select a).ToList();

                        if (listFlowInfo == null || listFlowInfo.Count == 0)
                        {
                            throw new Exception("回退流程无记录");
                        }
                        else
                        {
                            回退单据 frm = new 回退单据(m_billType, m_customForm.FlowInfo_BillNo, lnqFlowInfo.BusinessStatus, listFlowInfo);
                            if (CommonProcessSubmit != null && CommonProcessSubmit(m_customForm, frm.Reason))
                            {
                                if (frm.ShowDialog() == DialogResult.OK)
                                {
                                    string advise = "回退 " + textBox1.Text + "原因：" + frm.Reason;

                                    m_serverFlow.FlowReback(m_customForm.FlowInfo_BillNo, advise, m_customForm.FlowInfo_StorageIDOrWorkShopCode,
                                        Convert.ToInt32(frm.ObjValue), m_customForm.KeyWords);

                                    MessageDialog.ShowPromptMessage("回退成功");
                                    this.DialogResult = DialogResult.OK;
                                    this.Close();
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("新建单据无法执行回退功能");
                    }
                }
                else
                {
                    回退单据 frm = new 回退单据(m_billType, m_customForm.FlowInfo_BillNo, null, null);
                    if (CommonProcessSubmit != null && CommonProcessSubmit(m_customForm, frm.Reason))
                    {
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            string advise = "回退 " + textBox1.Text + "原因：" + frm.Reason;

                            MessageDialog.ShowPromptMessage("回退成功");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btnPackUp_Click(object sender, EventArgs e)
        {
            if (btnPackUp.Text == ">>")
            {
                panel1.Visible = false;
                btnPackUp.Text = "<<";
            }
            else if (btnPackUp.Text == "<<")
            {
                panel1.Visible = true;
                btnPackUp.Text = ">>";
            }
        }

        void CheckData()
        {
            if (m_customForm.FlowInfo_BillNo == null || m_customForm.FlowInfo_BillNo.Trim().Length == 0)
            {
                throw new Exception("单据号不能为空");
            }

            Flow_FlowBillData tempBill = m_serverFlow.GetBillData(m_customForm.FlowInfo_BillNo);

            if (m_customForm.流程控制类型 == CE_FormFlowType.默认)
            {
                if ((m_flowBillInfo != null && tempBill == null) || (m_flowBillInfo == null && tempBill != null))
                {
                    throw new Exception("该流程业务有误，请关闭当前操作界面，刷新数据再试");
                }
                else if (m_flowBillInfo != null && tempBill != null && m_flowBillInfo.FlowID != tempBill.FlowID)
                {
                    throw new Exception("当前单据【业务状态】已被更改无法操作，请关闭当前操作界面，刷新数据再试");
                }
            }
        }

        void DisagreeOperation()
        {
            try
            {
                if (textBox1.Text.Trim().Length == 0)
                {
                    MessageDialog.ShowPromptMessage("请在【意见】栏中填写【不同意】的原因");
                    return;
                }

                if (!m_customForm.GetDateInfo(CE_FlowOperationType.回退))
                {
                    return;
                }

                if (m_customForm.FlowInfo_BillNo == null || m_customForm.FlowInfo_BillNo.Trim().Length == 0)
                {
                    throw new Exception("单据号不能为空");
                }

                if (m_serverFlow.IsExist(m_customForm.FlowInfo_BillNo))
                {
                    Flow_FlowInfo lnqFlowInfo = m_serverFlow.GetNowFlowInfo(m_serverFlow.GetBusinessTypeID(m_billType, m_FlowBusinessVersion), m_customForm.FlowInfo_BillNo);

                    List<Flow_FlowInfo> listFlowInfo = (from a in m_serverFlow.GetListFlowInfo(m_customForm.FlowInfo_BillNo)
                                                        where a.FlowOrder < lnqFlowInfo.FlowOrder
                                                        orderby a.FlowOrder
                                                        select a).ToList();

                    if (listFlowInfo == null || listFlowInfo.Count == 0)
                    {
                        throw new Exception("回退流程无记录");
                    }
                    else
                    {
                        string advise = "不同意 原因：" + textBox1.Text.Trim();

                        if (CommonProcessSubmit != null && CommonProcessSubmit(m_customForm, textBox1.Text.Trim()))
                        {
                            m_serverFlow.FlowReback(m_customForm.FlowInfo_BillNo, advise, m_customForm.FlowInfo_StorageIDOrWorkShopCode,
                                listFlowInfo[listFlowInfo.Count - 1].FlowID, m_customForm.KeyWords);
                            MessageDialog.ShowPromptMessage("提交成功");

                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                }
                else
                {
                    throw new Exception("新建单据无法执行【不同意】提交功能");
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        bool PanelVisible(string billNo)
        {
            string strSql = "select * from PlatformService.dbo.Flow_BillFlowMessage where 单据号 = '" + billNo + "'";

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (tempTable.Rows.Count == 0)
            {
                return true;
            }
            else
            {
                DataRow tempRow = tempTable.Rows[0];

                if (tempRow["单据状态"].ToString() == "已完成")
                {
                    return false;
                }
                else
                {
                    if (tempRow["接收方类型"].ToString() == BillFlowMessage_ReceivedUserType.用户.ToString())
                    {
                        if (tempRow["接收方"].ToString().Contains(","))
                        {
                            string[] tempAc = tempRow["接收方"].ToString().Split(',');

                            if (tempAc.Contains(BasicInfo.LoginID))
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (BasicInfo.LoginID == tempRow["接收方"].ToString())
                            {
                                return true;
                            }
                        }
                    }
                    else if (tempRow["接收方类型"].ToString() == BillFlowMessage_ReceivedUserType.角色.ToString())
                    {
                        if (tempRow["接收方"].ToString().Contains(","))
                        {
                            string[] tempAc = tempRow["接收方"].ToString().Split(',');

                            foreach (string item in tempAc)
                            {
                                if (BasicInfo.ListRoles.Contains(item))
                                {
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            if (BasicInfo.ListRoles.Contains(tempRow["接收方"].ToString()))
                            {
                                return true;
                            }
                        }
                    }

                    return false;
                }
            }
        }

        void AddSerialLable(string text, Color cl, ref int length)
        {
            Label templb = GetLabel(new Point(length, CONNECT_HIGH), CONNECT_TEXT, cl);

            panel7.Controls.Add(templb);

            Label templb1 = GetLabel(new Point(length + templb.Size.Width, CONTENT_HIGH), text, cl);
            panel7.Controls.Add(templb1);

            length = length + templb.Size.Width + templb1.Size.Width;
        }

        void AddParallelLable(Dictionary<string, bool> listInfo, ref int length)
        {
            length = length + PARALLEL_LENTHG;
            Label templb = new Label();
            Label templb1 = new Label();

            int iCount = 0;

            foreach (KeyValuePair<string, bool> item in listInfo)
            {
                if (iCount == 0)
                {
                    templb = GetLabel(new Point(length, CONNECT_HIGH), CONNECT_TEXT, item.Value ? Color.Black : Color.Blue);
                    panel7.Controls.Add(templb);

                    templb1 = GetLabel(new Point(length + templb.Size.Width, CONTENT_HIGH), item.Key, item.Value ? Color.Black : Color.Blue);
                    panel7.Controls.Add(templb1);
                }
                else
                {
                    panel7.Controls.Add(GetLabel(new Point(length - PARALLEL_LENTHG, CONNECT_HIGH + BRANCH_DIFFERENCE_HIGH + DIFFERENCE_HIGH * (iCount - 1)), BRANCH_TEXT, item.Value ? Color.Black : Color.Blue));
                    panel7.Controls.Add(GetLabel(new Point(length, CONNECT_HIGH + DIFFERENCE_HIGH * iCount), CONNECT_TEXT, item.Value ? Color.Black : Color.Blue));
                    panel7.Controls.Add(GetLabel(new Point(length + templb.Size.Width,
                        CONTENT_HIGH + DIFFERENCE_HIGH * iCount), item.Key, item.Value ? Color.Black : Color.Blue));
                }

                iCount = iCount + 1;
            }

            length = length + templb.Size.Width + templb1.Size.Width;
        }

        Label GetLabel(Point pt, string text, Color cl)
        {
            Label resultlb = new Label();

            resultlb.Text = text;
            resultlb.Location = pt;
            resultlb.AutoSize = true;
            resultlb.ForeColor = cl;

            return resultlb;
        }
    }
}
