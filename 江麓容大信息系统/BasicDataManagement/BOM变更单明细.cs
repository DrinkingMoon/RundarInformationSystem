using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;
using PlatformManagement;
using UniversalControlLibrary;
using Service_Project_Design;
using System.IO;
using FlowControlService;

namespace Form_Project_Design
{
    public partial class BOM变更单明细 : CustomFlowForm
    {
        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据号
        /// </summary>
        private Business_Base_BomChange m_lnqBillInfo = new Business_Base_BomChange();

        public Business_Base_BomChange LnqBillInfo
        {
            get { return m_lnqBillInfo; }
            set { m_lnqBillInfo = value; }
        }

        /// <summary>
        /// 结构信息数据集合
        /// </summary>
        List<View_Business_Base_BomChange_Struct> m_listStruct = new List<View_Business_Base_BomChange_Struct>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IBOMChangeService m_serviceStatement = Service_Project_Design.ServerModuleFactory.GetServerModule<IBOMChangeService>();

        /// <summary>
        /// 流程服务组件
        /// </summary>
        IFlowServer m_serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        /// <summary>
        /// 二进制
        /// </summary>
        Byte[] m_byteData = null;

        /// <summary>
        /// 零件选择SQL语句
        /// </summary>
        string m_strShowTextBoxSql = "";

        /// <summary>
        /// BOM服务组件
        /// </summary>
        IBOMInfoService m_serviceBOMInfo = Service_Project_Design.ServerModuleFactory.GetServerModule<IBOMInfoService>();

        public BOM变更单明细()
        {
            InitializeComponent();
        }

        void 图号型号_m_OnCompleteSearch()
        {            
            DataRow drTemp = this.图号型号.DataResult;

            if (drTemp != null)
            {
                foreach (DataGridViewRow dr in dataGridViewStruct.Rows)
                {
                    if (dr.Cells["物品ID"].Value.ToString() == drTemp["物品ID"].ToString())
                    {
                        MessageDialog.ShowPromptMessage("存在相同项，不能添加重复零件");
                        dataGridViewStruct.CurrentRow.Cells["图号型号"].Value = "";
                        dataGridViewStruct.CurrentRow.Cells["物品名称"].Value = "";
                        dataGridViewStruct.CurrentRow.Cells["规格"].Value = "";
                        dataGridViewStruct.CurrentRow.Cells["物品ID"].Value = 0;
                        return;
                    }
                }

                dataGridViewStruct.CurrentRow.Cells["图号型号"].Value = drTemp["零部件编码"];
                dataGridViewStruct.CurrentRow.Cells["物品名称"].Value = drTemp["零部件名称"];
                dataGridViewStruct.CurrentRow.Cells["规格"].Value = drTemp["规格"];
                dataGridViewStruct.CurrentRow.Cells["物品ID"].Value = drTemp["物品ID"];
            }
        }

        void SetInfo()
        {
            if (m_lnqBillInfo != null)
            {
                lbBillStatus.Text = m_serverFlow.GetNowBillStatus(m_lnqBillInfo.BillNo);

                txtBillNo.Text = m_lnqBillInfo.BillNo;
                txtFileCode.Text = m_lnqBillInfo.FileCode;
                txtReason.Text = m_lnqBillInfo.Reason;
                m_byteData = m_lnqBillInfo.FileInfo.ToArray();
            }
            else
            {
                lbBillStatus.Text = CE_CommonBillStatus.新建单据.ToString();

                m_lnqBillInfo = new Business_Base_BomChange();

                txtBillNo.Text = this.FlowInfo_BillNo;
                m_lnqBillInfo.BillNo = txtBillNo.Text;
                this.FlowInfo_BillNo = txtBillNo.Text;
            }

            m_listStruct = m_serviceStatement.GetListStructInfo(m_lnqBillInfo.BillNo);
            List<View_Business_Base_BomChange_PartsLibrary> listLibrary = 
                m_serviceStatement.GetListLibraryInfo(m_lnqBillInfo.BillNo);

            RefreshDataGridView(listLibrary, m_listStruct);
            SetStructShowTextBoxSql();
        }

        List<View_Business_Base_BomChange_Struct> SingleParentGoodsStruct(int goodsID)
        {
            var varData = from a in m_listStruct
                          where a.父级物品ID == goodsID
                          select a;

            return varData.ToList();
        }

        void RefreshDataGridView(List<View_Business_Base_BomChange_PartsLibrary> lstlibrary,
            List<View_Business_Base_BomChange_Struct> lstStruct)
        {
            if (lstStruct != null)
            {
                dataGridViewParent.Rows.Clear();
                dataGridViewStruct.Rows.Clear();

                var varData = (from a in m_listStruct
                              select new { a.父级图号, a.父级物品ID }).Distinct();

                foreach (var item in varData)
                {
                    dataGridViewParent.Rows.Add(new object[] { item.父级图号, item.父级物品ID });
                }
            }

            if (lstlibrary != null)
            {
                dataGridViewLibrary.DataSource = new BindingCollection<View_Business_Base_BomChange_PartsLibrary>(lstlibrary);
            }
        }

        public override void LoadFormInfo()
        {
            try
            {
                m_billNoControl = new BillNumberControl(CE_BillTypeEnum.BOM变更单.ToString(), m_serviceStatement);
                m_lnqBillInfo = m_serviceStatement.GetSingleBillInfo(this.FlowInfo_BillNo);
                this.图号型号.m_OnCompleteSearch += new DelegateCollection.NonArgumentHandle(图号型号_m_OnCompleteSearch);
                SetInfo();

                switch (GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommonBillStatus>(lbBillStatus.Text))
                {
                    case CE_CommonBillStatus.新建单据:
                        break;
                    case CE_CommonBillStatus.等待审核:
                    case CE_CommonBillStatus.单据完成:

                        txtFileCode.Enabled = false;
                        lbUpFile.Enabled = false;
                        txtReason.Enabled = false;

                        btnLibraryAdd.Enabled = false;
                        btnLibraryDelete.Enabled = false;
                        btnParentAdd.Enabled = false;
                        btnParentDelete.Enabled = false;

                        dataGridViewStruct.ContextMenuStrip = null;

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
            }
        }

        private void btnStructInput_Click(object sender, EventArgs e)
        {
            FormGoodsSelect frm = new FormGoodsSelect(m_serviceStatement.GetAssemblyInfo());

            if (frm.ShowDialog() == DialogResult.OK)
            {
                List<View_BASE_BomStruct> listTemp = m_serviceBOMInfo.GetBOMList_Design(frm.GoodsInfo.序号);

                string goodsCode = dataGridViewParent.CurrentRow.Cells["父级图号_2"].Value.ToString();
                int goodsID = Convert.ToInt32( dataGridViewParent.CurrentRow.Cells["父级物品ID_2"].Value);

                foreach (View_BASE_BomStruct item in listTemp)
                {
                    View_Business_Base_BomChange_Struct tempLnq = new View_Business_Base_BomChange_Struct();

                    tempLnq.单据号 = txtBillNo.Text;
                    tempLnq.父级图号 = goodsCode;
                    tempLnq.父级物品ID = goodsID;
                    tempLnq.规格 = item.Spec;
                    tempLnq.基数 = item.Usage;
                    tempLnq.图号型号 = item.GoodsCode;
                    tempLnq.物品ID = item.GoodsID;
                    tempLnq.物品名称 = item.GoodsName;

                    m_listStruct.Add(tempLnq);
                }

                List<View_Business_Base_BomChange_Struct> lstTemp = SingleParentGoodsStruct(goodsID);

                foreach (View_Business_Base_BomChange_Struct item in lstTemp)
                {
                    dataGridViewStruct.Rows.Add(new object[] { item.单据号, item.父级图号, item.父级物品ID, item.物品ID, 
                    item.图号型号, item.物品名称, item.规格, item.基数 });
                }

                string strTemp = GetEndSql();
                foreach (DataGridViewRow dr in dataGridViewStruct.Rows)
                {
                    SetStructShowTextBox(dr, strTemp);
                }

                userControlDataLocalizer1.Init(dataGridViewStruct, this.Name,
                    UniversalFunction.SelectHideFields(this.Name, dataGridViewStruct.Name, BasicInfo.LoginID));
            }
        }

        private void lbUpFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdPic = new OpenFileDialog();
            ofdPic.Filter = "doc files (*.doc)|*.doc|pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";

            if (ofdPic.ShowDialog() == DialogResult.OK)
            {
                string sPicPaht = ofdPic.FileName.ToString();
                FileStream file = new FileStream(sPicPaht, FileMode.Open);

                if (file.Length / 1024 / 1024 > 2)
                {
                    MessageDialog.ShowPromptMessage("文件不能超过2M");
                }
                else
                {
                    BinaryReader br = new BinaryReader(file);
                    m_byteData = br.ReadBytes((int)file.Length);
                    if (file != null)
                        file.Close();
                }
            }
        }

        private void lbDownFile_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "pdf files (*.pdf)|*.pdf|doc files (*.doc)|*.doc|All files (*.*)|*.*";

            if (Convert.IsDBNull( m_lnqBillInfo.FileInfo))
            {
                MessageDialog.ShowPromptMessage("该条变更单无文件");
                return;
            }

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                m_byteData = (byte[]) m_lnqBillInfo.FileInfo.ToArray();

                if (m_byteData != null || m_byteData.ToString() != "")
                {
                    FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(m_byteData);
                    bw.Close();
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// 检测零件库添加信息是否完整
        /// </summary>
        /// <returns>通过返回True否则返回False</returns>
        bool CheckLibraryAddInfo()
        {
            foreach (DataGridViewRow dr in dataGridViewLibrary.Rows)
            {
                if (dr.Cells["物品ID_1"].Value.ToString() == txtGoodsCode.Tag.ToString())
                {
                    MessageDialog.ShowPromptMessage("不能添加重复项，请重新选择物品");
                    return false;
                }
            }

            if (txtPivotalPart.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写【关键件】信息");
                return false;
            }

            if (txtMaterial.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写【材质】信息");
                return false;
            }

            if (txtVersion.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写【版次号】信息");
                return false;
            }

            if (cmbPartType.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【零件类型】信息");
                return false;
            }

            return true;
        }

        void ClearLibraryInputInfo()
        {
            txtGoodsCode.Text = "";
            txtGoodsCode.Tag = null;
            txtGoodsName.Text = "";
            txtSpec.Text = "";
            txtMaterial.Text = "";
            txtVersion.Text = "";
            txtPivotalPart.Text = "";
            txtRemark.Text = "";
            cmbPartType.SelectedIndex = -1;
        }

        private void btnLibraryAdd_Click(object sender, EventArgs e)
        {
            if (!CheckLibraryAddInfo())
            {
                return;
            }

            List<View_Business_Base_BomChange_PartsLibrary> listLibrary =
                (dataGridViewLibrary.DataSource as BindingCollection<View_Business_Base_BomChange_PartsLibrary>).ToList();

            View_Business_Base_BomChange_PartsLibrary tempLibrary = new View_Business_Base_BomChange_PartsLibrary();

            View_F_GoodsPlanCost goodsInfo = UniversalFunction.GetGoodsInfo((int)txtGoodsCode.Tag);

            tempLibrary.版次号 = txtVersion.Text;
            tempLibrary.备注 = txtRemark.Text;
            tempLibrary.材质 = txtMaterial.Text;
            tempLibrary.单据号 = m_lnqBillInfo.BillNo;
            tempLibrary.关键件 = txtPivotalPart.Text;
            tempLibrary.规格 = goodsInfo.规格;
            tempLibrary.零件类型 = cmbPartType.Text;
            tempLibrary.图号型号 = goodsInfo.图号型号;
            tempLibrary.物品ID = (int)txtGoodsCode.Tag;
            tempLibrary.物品名称 = goodsInfo.物品名称;
            
            foreach (Control cl in groupBox2.Controls)
            {
                if (cl is RadioButton && ((RadioButton)cl).Checked)
                {
                    tempLibrary.操作类型 = ((RadioButton)cl).Text;
                }
            }

            listLibrary.Add(tempLibrary);

            dataGridViewLibrary.DataSource = new BindingCollection<View_Business_Base_BomChange_PartsLibrary>(listLibrary);
            ClearLibraryInputInfo();
        }

        private void btnLibraryDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewLibrary.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in dataGridViewLibrary.SelectedRows)
                {
                    dataGridViewLibrary.Rows.Remove(dr);
                }
            }
        }

        private void btnStructAdd_Click(object sender, EventArgs e)
        {
            dataGridViewStruct.Rows.Add(new object[] { txtBillNo.Text, dataGridViewParent.CurrentRow.Cells["父级图号_2"].Value.ToString(), 
                (int)dataGridViewParent.CurrentRow.Cells["父级物品ID_2"].Value, 0, "", "", "", 0 });

            SetStructShowTextBox(dataGridViewStruct.Rows[dataGridViewStruct.Rows.Count - 1], GetEndSql());

            dataGridViewStruct.CurrentCell = dataGridViewStruct.Rows[dataGridViewStruct.Rows.Count - 1].Cells[4];
            dataGridViewStruct.FirstDisplayedScrollingRowIndex = dataGridViewStruct.Rows.Count - 1;
        }

        private void btnStructDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewStruct.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in dataGridViewStruct.SelectedRows)
                {
                    dataGridViewStruct.Rows.Remove(dr);
                }
            }
        }

        private void btnParentAdd_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridViewParent.Rows)
            {
                if (dr.Cells["父级物品ID_2"].Value.ToString() == txtParentGoodsCode.Tag.ToString())
                {
                    MessageDialog.ShowPromptMessage("存在相同项，不能添加重复父级图号");
                    return;
                }
            }

            List<View_BASE_BomStruct> listTemp = m_serviceBOMInfo.GetBOMList_Design(Convert.ToInt32(txtParentGoodsCode.Tag));

            foreach (View_BASE_BomStruct item in listTemp)
            {
                View_Business_Base_BomChange_Struct tempLnq = new View_Business_Base_BomChange_Struct();

                tempLnq.单据号 = txtBillNo.Text;
                tempLnq.父级图号 = item.ParentGoodsCode;
                tempLnq.父级物品ID = item.ParentID;
                tempLnq.规格 = item.Spec;
                tempLnq.基数 = item.Usage;
                tempLnq.图号型号 = item.GoodsCode;
                tempLnq.物品ID = item.GoodsID;
                tempLnq.物品名称 = item.GoodsName;

                m_listStruct.Add(tempLnq);
            }

            dataGridViewParent.Rows.Add(new object[] { txtParentGoodsCode.Text, (int)txtParentGoodsCode.Tag });

            dataGridViewParent.CurrentCell = dataGridViewParent.Rows[dataGridViewParent.Rows.Count - 1].Cells[0];
            dataGridViewParent.FirstDisplayedScrollingRowIndex = dataGridViewParent.Rows.Count - 1;
            dataGridViewParent_CellEnter(sender, new DataGridViewCellEventArgs(0, dataGridViewParent.Rows.Count - 1));
        }

        private void btnParentDelete_Click(object sender, EventArgs e)
        {
            m_listStruct.RemoveAll(r => r.父级物品ID == (int)dataGridViewParent.CurrentRow.Cells["父级物品ID_2"].Value);
            dataGridViewParent.Rows.Remove(dataGridViewParent.CurrentRow);

            if (dataGridViewParent.Rows.Count != 0)
            {
                dataGridViewParent.CurrentCell = dataGridViewParent.Rows[dataGridViewParent.Rows.Count - 1].Cells[0];
                dataGridViewParent.FirstDisplayedScrollingRowIndex = dataGridViewParent.Rows.Count - 1;
                dataGridViewParent_CellEnter(sender, new DataGridViewCellEventArgs(0, dataGridViewParent.Rows.Count - 1));
            }
            else
            {
                dataGridViewStruct.Rows.Clear();
            }

        }

        private void txtParentGoodsCode_OnCompleteSearch()
        {
            if (txtParentGoodsCode.DataResult != null)
            {
                txtParentGoodsCode.Tag = txtParentGoodsCode.DataResult["物品ID"];
                txtParentGoodsCode.Text = txtParentGoodsCode.DataResult["零部件编码"].ToString();
            }
            else
            {
                txtParentGoodsCode.Tag = null;
                txtParentGoodsCode.Text = "";
            }
        }

        private void txtGoodsCode_OnCompleteSearch()
        {
            if (txtGoodsCode.FindItem == TextBoxShow.FindType.BOM表零件)
            {
                txtGoodsCode.Tag = txtGoodsCode.DataResult["物品ID"];

                txtSpec.Text = txtGoodsCode.DataResult["规格"].ToString();
                txtGoodsCode.Text = txtGoodsCode.DataResult["零部件编码"].ToString();
                txtGoodsName.Text = txtGoodsCode.DataResult["零部件名称"].ToString();

                BASE_BomPartsLibrary tempLnq = m_serviceBOMInfo.GetLibrarySingle(Convert.ToInt32(txtGoodsCode.Tag));

                if (tempLnq != null)
                {
                    txtMaterial.Text = tempLnq.Material;
                    txtVersion.Text = tempLnq.Version;
                    txtPivotalPart.Text = tempLnq.PivotalPart;
                    cmbPartType.Text = tempLnq.PartType;
                }

            }
            else if (txtGoodsCode.FindItem == TextBoxShow.FindType.所有物品)
            {
                txtGoodsCode.Tag = txtGoodsCode.DataResult["序号"];

                txtSpec.Text = txtGoodsCode.DataResult["规格"].ToString();
                txtGoodsCode.Text = txtGoodsCode.DataResult["图号型号"].ToString();
                txtGoodsName.Text = txtGoodsCode.DataResult["物品名称"].ToString();
            }
        }

        private void dataGridViewParent_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewParent == null || dataGridViewParent.Rows.Count == 0)
            {
                return;
            }

            dataGridViewStruct.Rows.Clear();

            List<View_Business_Base_BomChange_Struct> lstTemp = 
                SingleParentGoodsStruct(Convert.ToInt32(dataGridViewParent.CurrentRow.Cells["父级物品ID_2"].Value));

            foreach (View_Business_Base_BomChange_Struct item in lstTemp)
            {
                if (item.物品ID == null)
                {
                    continue;
                }

                dataGridViewStruct.Rows.Add(new object[] { item.单据号, item.父级图号, item.父级物品ID, item.物品ID, 
                    item.图号型号, item.物品名称, item.规格, item.基数 });
            }

            string strTemp = GetEndSql();
            foreach (DataGridViewRow dr in dataGridViewStruct.Rows)
            {
                SetStructShowTextBox(dr, strTemp);
            }

            userControlDataLocalizer1.Init(dataGridViewStruct, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridViewStruct.Name, BasicInfo.LoginID));
        }

        private void dataGridViewLibrary_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewLibrary == null || dataGridViewLibrary.Rows.Count == 0)
            {
                return;
            }

            txtGoodsCode.Text = dataGridViewLibrary.CurrentRow.Cells["图号型号_1"].Value.ToString();
            txtGoodsCode.Tag = (int)dataGridViewLibrary.CurrentRow.Cells["物品ID_1"].Value;
            txtGoodsName.Text = dataGridViewLibrary.CurrentRow.Cells["物品名称_1"].Value.ToString();
            txtSpec.Text = dataGridViewLibrary.CurrentRow.Cells["规格_1"].Value.ToString();
            txtVersion.Text = dataGridViewLibrary.CurrentRow.Cells["版次号"].Value.ToString();
            txtMaterial.Text = dataGridViewLibrary.CurrentRow.Cells["材质"].Value.ToString();
            txtPivotalPart.Text = dataGridViewLibrary.CurrentRow.Cells["关键件"].Value.ToString();
            cmbPartType.Text = dataGridViewLibrary.CurrentRow.Cells["零件类型"].Value.ToString();
            txtRemark.Text = dataGridViewLibrary.CurrentRow.Cells["备注"].Value.ToString();

            foreach (Control cl in groupBox2.Controls)
            {
                if (cl is RadioButton)
                {
                    if (((RadioButton)cl).Text == dataGridViewLibrary.CurrentRow.Cells["操作类型"].Value.ToString())
                    {
                        ((RadioButton)cl).Checked = true;
                    }
                    else
                    {
                        ((RadioButton)cl).Checked = false;
                    }
                }
            }
        }

        private void dataGridViewStruct_Leave(object sender, EventArgs e)
        {
            m_listStruct.RemoveAll(r => r.父级物品ID == (int)dataGridViewParent.CurrentRow.Cells["父级物品ID_2"].Value);

            if (dataGridViewStruct.Rows.Count == 0)
            {
                View_Business_Base_BomChange_Struct tempLnq = new View_Business_Base_BomChange_Struct();

                tempLnq.单据号 = txtBillNo.Text;
                tempLnq.父级图号 = dataGridViewParent.CurrentRow.Cells["父级图号_2"].Value.ToString();
                tempLnq.父级物品ID = Convert.ToInt32(dataGridViewParent.CurrentRow.Cells["父级物品ID_2"].Value);

                m_listStruct.Add(tempLnq);
                return;
            }

            foreach (DataGridViewRow dr in dataGridViewStruct.Rows)
            {
                if (dr.Cells["物品ID"].Value == null || dr.Cells["物品ID"].Value.ToString() == "0")
                {
                    continue;
                }

                View_Business_Base_BomChange_Struct tempLnq = new View_Business_Base_BomChange_Struct();

                tempLnq.单据号 = dr.Cells["单据号"].Value.ToString();
                tempLnq.父级图号 = dr.Cells["父级图号"].Value.ToString();
                tempLnq.父级物品ID = Convert.ToInt32(dr.Cells["父级物品ID"].Value);
                tempLnq.规格 = dr.Cells["规格"].Value.ToString();
                tempLnq.基数 = Convert.ToDecimal(dr.Cells["基数"].Value);
                tempLnq.图号型号 = dr.Cells["图号型号"].Value.ToString();
                tempLnq.物品名称 = dr.Cells["物品名称"].Value.ToString();
                tempLnq.物品ID = Convert.ToInt32(dr.Cells["物品ID"].Value);

                m_listStruct.Add(tempLnq);
            }
        }

        private void rb_Add_CheckedChanged(object sender, EventArgs e)
        {
            txtGoodsCode.FindItem = rb_Add.Checked ? TextBoxShow.FindType.所有物品 : TextBoxShow.FindType.BOM表零件;
        }

        private void txtGoodsCode_Enter(object sender, EventArgs e)
        {
            foreach (Control cl in groupBox2.Controls)
            {
                if (cl is RadioButton)
                {
                    if (((RadioButton)cl).Checked)
                    {
                        if (((RadioButton)cl).Text == CE_OperatorMode.添加.ToString())
                        {
                            txtGoodsCode.StrEndSql = " and 序号 not in (select GoodsID from BASE_BomPartsLibrary) and 禁用 = 0";
                        }
                        else
                        {
                            txtGoodsCode.StrEndSql = "";
                        }

                        txtGoodsCode.ShowResultForm = true;
                        return;
                    }
                }
            }

            txtGoodsCode.ShowResultForm = false;
            MessageDialog.ShowPromptMessage("请先选择变更方式");
        }

        bool CheckSubmitInfo()
        {
            if (txtFileCode.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写技术单号");
                return false;
            }

            if (txtReason.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写变更原因");
                return false;
            }

            if (m_byteData == null || m_byteData.ToString() == "")
            {
                MessageDialog.ShowPromptMessage("请上传文件!");
                return false;
            }

            if (m_listStruct.Count == 0 && dataGridViewLibrary.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("无任何变更内容，请重新确认");
                return false;
            }

            return true;
        }

        private bool customPanel1_PanelGetDataInfo(CE_FlowOperationType flowOperationType)
        {
            try
            {
                if (!CheckSubmitInfo())
                {
                    return false;
                }

                m_lnqBillInfo.BillNo = txtBillNo.Text;
                m_lnqBillInfo.FileCode = txtFileCode.Text;
                m_lnqBillInfo.FileInfo = m_byteData;
                m_lnqBillInfo.Reason = txtReason.Text;

                List<View_Business_Base_BomChange_PartsLibrary> listLibrary =
                    (dataGridViewLibrary.DataSource as BindingCollection<View_Business_Base_BomChange_PartsLibrary>).ToList();

                ResultInfo = m_lnqBillInfo;
                FlowInfo_BillNo = m_lnqBillInfo.BillNo;
                FlowOperationType = flowOperationType;

                this.ResultList = new List<object>();

                ResultList.Add(m_listStruct);
                ResultList.Add(listLibrary);
                KeyWords = "【技术变更单号】:" + m_lnqBillInfo.FileCode;

                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return false;
            }
        }

        void SetStructShowTextBoxSql()
        {
            m_strShowTextBoxSql = " select distinct * from (select GoodsID as 物品ID, b.图号型号 as 零部件编码,b.物品名称 as 零部件名称, " +
                                  " b.规格, Version as 版次号, b.拼音码 as PY, b.五笔码 as WB from BASE_BomPartsLibrary as a inner join " +
                                  " View_F_GoodsPlanCost as b on a.GoodsID = b.序号 ";

            foreach (DataGridViewRow dr in dataGridViewLibrary.Rows)
            {
                if (dr.Cells["操作类型"].Value.ToString() == CE_OperatorMode.添加.ToString())
                {
                    m_strShowTextBoxSql += " Union all select " + dr.Cells["物品ID_1"].Value.ToString() + ","
                                                    + "'" + dr.Cells["图号型号_1"].Value.ToString() + "',"
                                                    + "'" + dr.Cells["物品名称_1"].Value.ToString() + "',"
                                                    + "'" + dr.Cells["规格_1"].Value.ToString() + "',"
                                                    + "'" + dr.Cells["版次号"].Value.ToString() + "',"
                                                    + "dbo.fun_get_bm('" + dr.Cells["物品名称_1"].Value.ToString() + "',1),"
                                                    + "dbo.fun_get_bm('" + dr.Cells["物品名称_1"].Value.ToString() + "',0)";
                }
            }

            m_strShowTextBoxSql += ") as a where 1 = 1 ";

            txtParentGoodsCode.strSql = m_strShowTextBoxSql;

            string strTemp = GetEndSql();
            foreach (DataGridViewRow dr in dataGridViewStruct.Rows)
            {
                SetStructShowTextBox(dr, strTemp);
            }
        }

        string GetEndSql()
        {
            Dictionary<CE_GoodsAttributeName, string> dic = new Dictionary<CE_GoodsAttributeName, string>();

            dic.Add(CE_GoodsAttributeName.CVT, "True");
            dic.Add(CE_GoodsAttributeName.TCU, "True");

            List<int> listInfo = UniversalFunction.GetGoodsInfoList_Attribute(dic).Select(k => k.ID).ToList();

            if (dataGridViewParent.CurrentRow != null)
            {
                listInfo.Add((int)dataGridViewParent.CurrentRow.Cells["父级物品ID_2"].Value);
            }

            string strGoods = UniversalFunction.GetListString<int>(listInfo, ',');

            if (strGoods == null)
            {
                return "";
            }
            else
            {
                return " and 物品ID not in (" + strGoods + ")";
            }
        }

        void SetStructShowTextBox(DataGridViewRow dr, string endSql)
        {
            ((DataGridViewTextBoxShowCell)dr.Cells["图号型号"]).m_StrSql = m_strShowTextBoxSql + endSql;
            ((DataGridViewTextBoxShowCell)dr.Cells["图号型号"]).m_SZstring = new string[] { "物品ID", "零部件编码", "零部件名称", "规格", "版次号" };
            ((DataGridViewTextBoxShowCell)dr.Cells["图号型号"]).m_strPyColunm = "PY";
            ((DataGridViewTextBoxShowCell)dr.Cells["图号型号"]).m_strWbColunm = "零部件编码";
            ((DataGridViewTextBoxShowCell)dr.Cells["图号型号"]).m_strCodeColunm = "零部件名称";
            ((DataGridViewTextBoxShowCell)dr.Cells["图号型号"]).m_strShowMessage = "零部件编码";
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                SetStructShowTextBoxSql();
            }
        }

        private void txtParentGoodsCode_Enter(object sender, EventArgs e)
        {
            txtParentGoodsCode.StrEndSql = " and 物品ID in (select GoodsID from F_GoodsAttributeRecord " +
                " where AttributeID in (" + (int)CE_GoodsAttributeName.CVT + ","
                                       + (int)CE_GoodsAttributeName.TCU + ","
                                       + (int)CE_GoodsAttributeName.部件 + ") and AttributeValue = '" + bool.TrueString + "')";
        }
    }
}
