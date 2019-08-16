using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GlobalObject;
using ServerModule;
using FlowControlService;
using System.Reflection;

namespace UniversalControlLibrary
{
    public partial class CustomMainForm : Form
    {
        CustomDataGridView m_selectDataGridView = new CustomDataGridView();

        public CustomDataGridView SelectDataGridView
        {
            get { return m_selectDataGridView; }
            set { m_selectDataGridView = value; }
        }

        /// <summary>
        /// 流程服务组件
        /// </summary>
        IFlowServer _ServiceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = null;

        /// <summary>
        /// 操作类型
        /// </summary>
        CE_FlowOperationType m_operationType = CE_FlowOperationType.未知;

        public CE_FlowOperationType OperationType
        {
            get { return m_operationType; }
            set { m_operationType = value; }
        }

        /// <summary>
        /// 单据号
        /// </summary>
        string m_billNo = null;

        public string BillNo
        {
            get { return m_billNo; }
            set { m_billNo = value; }
        }

        /// <summary>
        /// 知会人员
        /// </summary>
        NotifyPersonnelInfo m_notifyPersonnel = new NotifyPersonnelInfo();

        public NotifyPersonnelInfo NotifyPersonnel
        {
            get { return m_notifyPersonnel; }
            set { m_notifyPersonnel = value; }
        }

        /// <summary>
        /// 明细界面类型
        /// </summary>
        Type _Type_DetailForm;

        /// <summary>
        /// 业务类型
        /// </summary>
        CE_BillTypeEnum _Enum_BillType = CE_BillTypeEnum.未知单据;

        /// <summary>
        /// 业务相关接口
        /// </summary>
        IFlowBusinessService _BusinessService = FlowControlService.ServerModuleFactory.GetServerModule<IFlowBusinessService>();

        public event UniversalControlLibrary.FormCommonProcess.FormSubmit Form_CommonProcessSubmit;

        public event GlobalObject.DelegateCollection.NonArgumentHandle Form_SendMessage;

        public event GlobalObject.DelegateCollection.ButtonClick Form_btnPrint;

        public CustomMainForm()
        {
            InitializeComponent();
        }

        public CustomMainForm(Type detailForm, CE_BillTypeEnum billType, IFlowBusinessService businessService)
        {
            InitializeComponent();
            _Type_DetailForm = detailForm;
            _Enum_BillType = billType;
            _BusinessService = businessService;
        }

        private void CustomMainForm_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                labelTitle.Text = _Enum_BillType.ToString();
                cmbVersion.DataSource = _ServiceFlow.GetBusinessInfoVersion(_Enum_BillType);
                cmbVersion.SelectedIndex = 0;
                m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

                checkBillDateAndStatus1.cmbBillStatus.Items.Clear();
                checkBillDateAndStatus1.InsertComBox(_ServiceFlow.GetBusinessStatus(_Enum_BillType, cmbVersion.Text));
                checkBillDateAndStatus1.InitDateTime();
                m_billNoControl = new BillNumberControl(_Enum_BillType.ToString(), _BusinessService as IBasicBillServer);
                m_billMessageServer.BillType = _Enum_BillType.ToString();

                foreach (TabPage tp in tabControl1.TabPages)
                {
                    foreach (Control cl in tp.Controls)
                    {
                        if (cl is CustomDataGridView)
                        {
                            ((CustomDataGridView)cl).DoubleClick += new EventHandler(dataGridView_DoubleClick);
                            ((CustomDataGridView)cl).CellEnter += new DataGridViewCellEventHandler(CustomMainForm_CellEnter);
                        }
                    }
                }

                RefreshData(tabControl1.SelectedTab);
            }
        }

        void CustomMainForm_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            CustomDataGridView dgv = sender as CustomDataGridView;

            if (dgv != null && dgv.CurrentRow != null)
            {
                this.m_billNo = dgv.CurrentRow.Cells["业务编号"].Value.ToString();
            }
        }

        public void ToolStripSeparator_ShowStatus(PlatformManagement.AuthorityFlag authorityFlag)
        {
            if (authorityFlag.ToString().Length == 0)
            {
                return;
            }

            List<String> lstAuthority = authorityFlag.ToString().Split(',').ToList();

            for (int i = 0; i < lstAuthority.Count; i++)
            {
                lstAuthority[i] = lstAuthority[i].Trim().ToUpper();
            }

            foreach (ToolStripItem tsi in toolStrip1.Items)
            {
                if (tsi.Tag != null)
                {
                    if (lstAuthority.Contains(tsi.Tag.ToString().Trim().ToUpper()))
                    {
                        tsi.Visible = true;
                    }
                    else
                    {
                        tsi.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public void RefreshData(TabPage tp)
        {
            try
            {
                DataGridView dgv = GetDataGridView(tp);

                dgv.DataSource = _ServiceFlow.ShowBusinessAllInfo(_Enum_BillType, cmbVersion.Text,
                    checkBillDateAndStatus1.ListBillStatus == null || checkBillDateAndStatus1.ListBillStatus.Count() == 0 ?
                    new string[] { "全部" } : checkBillDateAndStatus1.ListBillStatus.ToArray(),
                    checkBillDateAndStatus1.dtpStartTime.Value, checkBillDateAndStatus1.dtpEndTime.Value, tp.Text, null);

                Flow_BusinessInfo bussinessInfo = _ServiceFlow.GetBusinessInfo(_Enum_BillType, cmbVersion.Text);

                foreach (DataGridViewColumn dgvc in dgv.Columns)
                {
                    if (dgvc.HeaderText == bussinessInfo.KeysName)
                    {
                        dgvc.Visible = false;
                    }
                }

                userControlDataLocalizer1.Init(dgv, this.Name,
                    UniversalFunction.SelectHideFields(this.Name, dgv.Name, BasicInfo.LoginID));
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
            }
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, _Enum_BillType.ToString());

                    if (dtMessage == null || dtMessage.Rows.Count == 0)
                    {
                        //m_billMessageServer.DestroyMessage(msg.MessageContent);
                        MessageDialog.ShowPromptMessage("未找到相关记录");
                    }
                    else
                    {
                        tabControl1.SelectedTab = tabControl1.TabPages[0];

                        CustomDataGridView dgv = GetDataGridView(tabControl1.SelectedTab);

                        dgv.DataSource = _ServiceFlow.ShowBusinessAllInfo(_Enum_BillType, cmbVersion.Text, new string[] { "全部" },
                            checkBillDateAndStatus1.dtpStartTime.Value, checkBillDateAndStatus1.dtpEndTime.Value, tabControl1.SelectedTab.Text, msg.MessageContent);

                        Flow_BusinessInfo bussinessInfo = _ServiceFlow.GetBusinessInfo(_Enum_BillType, cmbVersion.Text);

                        foreach (DataGridViewColumn dgvc in dgv.Columns)
                        {
                            if (dgvc.HeaderText == bussinessInfo.KeysName)
                            {
                                dgvc.Visible = false;
                            }
                        }

                        dgv.Rows[0].Selected = true;
                    }

                    break;

                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        public void PositioningRecord(string billNo)
        {
            string strColName = "";
            CustomDataGridView dgv = GetDataGridView(tabControl1.SelectedTab);
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if ((string)dgv.Rows[i].Cells["业务编号"].Value == billNo)
                {
                    dgv.FirstDisplayedScrollingRowIndex = i;
                    dgv.CurrentCell = dgv.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 获得当前CustomDataGridView
        /// </summary>
        /// <param name="tp">tabPage</param>
        /// <returns>返回DataGridView</returns>
        CustomDataGridView GetDataGridView(TabPage tp)
        {
            CustomDataGridView dgv = new CustomDataGridView();

            foreach (Control cl in tp.Controls)
            {
                if (cl is DataGridView)
                {
                    dgv = cl as CustomDataGridView;
                }
            }

            m_selectDataGridView = dgv;

            return dgv;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        string GetFlowInfo_BillNo(object obj)
        {
            PropertyInfo pi1 = _Type_DetailForm.GetProperty("FlowInfo_BillNo",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            string billNo = pi1.GetValue(obj, null).ToString();

            return billNo;
        }

        object SetFlowInfo_BillNo(DataGridView dgv, string keyName)
        {
            object obj = Activator.CreateInstance(_Type_DetailForm);

            PropertyInfo pi = _Type_DetailForm.GetProperty("FlowInfo_BillNo",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            pi.SetValue(obj, dgv.CurrentRow.Cells[keyName].Value.ToString(), null);

            return obj;
        }

        void SetFormBusinessInfo(DataGridView dgvr, CE_OperatorMode operatorMode,
            CE_BillTypeEnum billType, string keyName, ref FormCommonProcess frm)
        {
            List<object> resultList = new List<object>();

            foreach (DataGridViewRow dr in dgvr.Rows)
            {
                Entity_BusinessOperationInfo info = new Entity_BusinessOperationInfo();

                info.BillType = billType;
                info.BusinessNo = dr.Cells[keyName].Value.ToString();
                info.OperatorMode = operatorMode;

                info.FlowInfoList = null;
                info.FlowMagicDic = null;

                if (dgvr.CurrentRow == dr)
                {
                    frm.BusinessList_Object = info;
                }

                resultList.Add(info);
            }

            frm.BusinessList = resultList;
        }

        private void dataGridView_DoubleClick(object sender, EventArgs e)
        {
            btnFind_Click(null, null);
        }

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            RefreshData(tabControl1.SelectedTab);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.m_billNo = null;
            RefreshData(tabControl1.SelectedTab);
        }

        public void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData(tabControl1.SelectedTab);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                CustomDataGridView dgv = GetDataGridView(tabControl1.SelectedTab);
                string tempStr = dgv.CurrentRow.Cells["业务编号"].Value.ToString();

                if (MessageDialog.ShowEnquiryMessage("您确定要删除【" + tempStr + "】号业务单据吗？") == DialogResult.Yes)
                {
                    _BusinessService.DeleteInfo(tempStr);
                    MessageDialog.ShowPromptMessage("删除成功");
                    m_billMessageServer.DestroyMessage(tempStr);
                }

                RefreshData(tabControl1.SelectedTab);
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            CustomDataGridView dgv = GetDataGridView(tabControl1.SelectedTab);

            if (dgv.CurrentRow == null)
            {
                return;
            }

            object obj = SetFlowInfo_BillNo(dgv as DataGridView, "业务编号");

            if (tabControl1.SelectedTab.Text == "待处理")
            {
                FormCommonProcess frm = new FormCommonProcess(_Enum_BillType, cmbVersion.Text, obj as CustomFlowForm, CE_OperatorMode.编辑);
                SetFormBusinessInfo(dgv as DataGridView, CE_OperatorMode.编辑, _Enum_BillType, "业务编号", ref frm);
                frm.CommonProcessSubmit += new FormCommonProcess.FormSubmit(frm_CommonProcessSubmit);
                m_operationType = CE_FlowOperationType.未知;

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    SendMessage();
                }
            }
            else
            {
                FormCommonProcess frm = new FormCommonProcess(_Enum_BillType, cmbVersion.Text, obj as CustomFlowForm, CE_OperatorMode.查看);
                SetFormBusinessInfo(dgv as DataGridView, CE_OperatorMode.查看, _Enum_BillType, "业务编号", ref frm);
                frm.ShowDialog();
            }

            RefreshData(tabControl1.SelectedTab);
            PositioningRecord(GetFlowInfo_BillNo(obj));
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            object obj = Activator.CreateInstance(_Type_DetailForm);
            CustomFlowForm customFrm = obj as CustomFlowForm;

            customFrm.FlowInfo_BillNo = m_billNoControl.GetNewBillNo();

            FormCommonProcess frm = new FormCommonProcess(_Enum_BillType, cmbVersion.Text, customFrm, CE_OperatorMode.添加);
            frm.CommonProcessSubmit += new FormCommonProcess.FormSubmit(frm_CommonProcessSubmit);
            m_operationType = CE_FlowOperationType.未知;

            if (frm.ShowDialog() != DialogResult.OK)
            {
                m_billNoControl.CancelBill(GetFlowInfo_BillNo(obj));
            }
            else
            {
                SendMessage();
            }

            RefreshData(tabControl1.SelectedTab);
            PositioningRecord(GetFlowInfo_BillNo(obj));
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, GetDataGridView(tabControl1.SelectedTab) as DataGridView);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            UniversalControlLibrary.FormPluginMethodCollection.BusinessDataSelect(_Enum_BillType,
                "单据号", ShowDetailInfo);
        }

        void ShowDetailInfo(DataGridView dgv, string keyName)
        {
            object obj = SetFlowInfo_BillNo(dgv as DataGridView, keyName);

            FormCommonProcess frm = new FormCommonProcess(_Enum_BillType, cmbVersion.Text, obj as CustomFlowForm, CE_OperatorMode.查看);
            SetFormBusinessInfo(dgv, CE_OperatorMode.查看, _Enum_BillType, keyName, ref frm);
            frm.ShowDialog();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (Form_btnPrint != null)
            {
                Form_btnPrint(sender, e);
            }
        }

        public void SendMessage()
        {
            if (Form_SendMessage != null)
            {
                Form_SendMessage();
            }
        }

        bool frm_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            if (Form_CommonProcessSubmit != null)
            {
                return Form_CommonProcessSubmit(form, advise);
            }

            return true;
        }

        private void cmbVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkBillDateAndStatus1.cmbBillStatus.Items.Clear();
            checkBillDateAndStatus1.InsertComBox(_ServiceFlow.GetBusinessStatus(_Enum_BillType, cmbVersion.Text));
            RefreshData(tabControl1.SelectedTab);
        }
    }
}
