using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GlobalObject;

namespace UniversalControlLibrary
{
    public partial class CustomFlowForm : Form
    {
        public CustomFlowForm()
        {
            InitializeComponent();
        }

        Dictionary<int, Dictionary<string, bool>> _custom_flowMagic = null;
        public Dictionary<int, Dictionary<string, bool>> Custom_FlowMagic
        {
            get { return _custom_flowMagic; }
            set { _custom_flowMagic = value; }
        }

        List<CommonProcessInfo> _custom_flowInfo = null;
        public List<CommonProcessInfo> Custom_FlowInfo
        {
            get { return _custom_flowInfo; }
            set { _custom_flowInfo = value; }
        }

        private string _keyWords = null;
        public string KeyWords
        {
            get { return _keyWords; }
            set { _keyWords = value; }
        }

        private string flowInfo_BillNo = null;
        public string FlowInfo_BillNo
        {
            get { return flowInfo_BillNo; }
            set { flowInfo_BillNo = value; }
        }

        private string flowInfo_StorageIDOrWorkShopCode = null;
        public string FlowInfo_StorageIDOrWorkShopCode
        {
            get { return flowInfo_StorageIDOrWorkShopCode; }
            set { flowInfo_StorageIDOrWorkShopCode = value; }
        }

        private NotifyPersonnelInfo flowInfo_NotifyInfo = new NotifyPersonnelInfo();
        public NotifyPersonnelInfo FlowInfo_NotifyInfo
        {
            get { return flowInfo_NotifyInfo; }
            set { flowInfo_NotifyInfo = value; }
        }

        private CE_FlowOperationType _flowOperationType;
        public CE_FlowOperationType FlowOperationType
        {
            get { return _flowOperationType; }
            set { _flowOperationType = value; }
        }

        private object resultInfo = null;
        public object ResultInfo
        {
            get { return resultInfo; }
            set { resultInfo = value; }
        }

        private List<object> resultList = new List<object>();
        public List<object> ResultList
        {
            get { return resultList; }
            set { resultList = value; }
        }

        private DataTable resultTable = new DataTable();
        public DataTable ResultTable
        {
            get { return resultTable; }
            set { resultTable = value; }
        }

        CE_FormFlowType _流程控制类型;
        public CE_FormFlowType 流程控制类型
        {
            get { return _流程控制类型; }
            set { _流程控制类型 = value; }
        }

        public event GlobalObject.DelegateCollection.GetDataInfo PanelGetDataInfo;
        public virtual void LoadFormInfo() { }

        /// <summary>
        /// 收集数据
        /// </summary>
        /// <param name="isIgnoreResult">是否忽略结果的布尔值</param>
        /// <returns>收集成功返回True, 收集失败返回False</returns>
        public bool GetDateInfo(GlobalObject.CE_FlowOperationType flowOperationType)
        {
            if (PanelGetDataInfo != null)
            {
                this.resultList.Clear();
                this.resultTable.Rows.Clear();
                this.resultInfo = null;
                this.flowInfo_StorageIDOrWorkShopCode = null;

                return PanelGetDataInfo(flowOperationType);
            }
            else
            {
                return false;
            }
        }

        public bool GetNotifyPersonnel(bool isMultiSelect)
        {
            flowInfo_NotifyInfo = new NotifyPersonnelInfo();
            FormSelectPersonnel2 frm = new FormSelectPersonnel2();
            frm.IsMultiSelect = isMultiSelect;

            if (frm.ShowDialog() != DialogResult.OK)
            {
                return false;
            }

            flowInfo_NotifyInfo = frm.SelectedNotifyPersonnelInfo;
            return true;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
