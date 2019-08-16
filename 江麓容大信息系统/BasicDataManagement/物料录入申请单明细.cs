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
using UniversalControlLibrary;
using Service_Project_Design;
using FlowControlService;
using System.Text.RegularExpressions;

namespace Form_Project_Design
{
    public partial class 物料录入申请单明细 : CustomFlowForm
    {
        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 物料录入申请服务组件
        /// </summary>
        IGoodsEnteringBill m_serverGoodsEntering = Service_Project_Design.ServerModuleFactory.GetServerModule<IGoodsEnteringBill>();

        /// <summary>
        /// 数据列表
        /// </summary>
        List<View_S_GoodsEnteringBill> m_listGoodsEntering = new List<View_S_GoodsEnteringBill>();

        /// <summary>
        /// 单据号
        /// </summary>
        private S_GoodsEnteringBill m_lnqBillInfo = new S_GoodsEnteringBill();

        public S_GoodsEnteringBill LnqBillInfo
        {
            get { return m_lnqBillInfo; }
            set { m_lnqBillInfo = value; }
        }

        /// <summary>
        /// 流程服务组件
        /// </summary>
        IFlowServer m_serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        public 物料录入申请单明细()
        {
            InitializeComponent(); 
        }

        /// <summary>
        /// 数据检测
        /// </summary>
        /// <returns>通过返回True,未通过返回False</returns>
        bool DataCheck()
        {
            if (lbSDBStatus.Text == GoodsEnteringBillStatus.等待校对.ToString())
            {
                BindingCollection<View_S_GoodsEnteringBill> tempList = dataGridView1.DataSource as BindingCollection<View_S_GoodsEnteringBill>;

                foreach (View_S_GoodsEnteringBill item in tempList)
                {
                    if ((item.单位 == null || item.单位.ToString().Trim().Length == 0)
                        || (item.材料类别编码 == null || item.材料类别编码.ToString().Trim().Length == 0))
                    {
                        string msg = "【图号】:" + item.图号型号 + " 【名称】:" + item.物品名称 + "【规格】:" + item.规格;
                        MessageDialog.ShowPromptMessage(msg + "单位/材料类别 未指定");
                        return false;
                    }
                }
            }

            return true;
        }
   
        /// <summary>
        /// 清空信息
        /// </summary>
        void ClearInfo()
        {
            txtCode.Text = "";
            txtName.Text = "";
            txtSpec.Text = "";
            txtDepot.Text = "";
            txtDepot.Tag = "";
            cmbUnit.SelectedIndex = -1;
            txtRemark.Text = "";
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        void ShowInfo()
        {
            string status = m_serverFlow.GetNowBillStatus(m_lnqBillInfo.BillNo);

            if (status == null || status.Trim().Length == 0)
            {
                lbSDBStatus.Text = CE_CommonBillStatus.新建单据.ToString();
                txtSDBNo.Text = this.FlowInfo_BillNo;
                m_lnqBillInfo.BillNo = txtSDBNo.Text;
            }
            else
            {
                txtSDBNo.Text = m_lnqBillInfo.BillNo;
                lbSDBStatus.Text = status;
            }

            m_lnqBillInfo.BillNo = txtSDBNo.Text;
            RefreshDataGridView(m_listGoodsEntering);
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="listInfo">数据集合</param>
        void RefreshDataGridView(List<View_S_GoodsEnteringBill> listInfo)
        {
            if (listInfo != null)
            {
                dataGridView1.DataSource = new BindingCollection<View_S_GoodsEnteringBill>(listInfo);
                dataGridView1.Columns["单据号"].Visible = false;
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                txtCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
                txtName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
                txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();

                if (dataGridView1.CurrentRow.Cells["材料类别编码"].Value != null)
	            {
                    txtDepot.Text = dataGridView1.CurrentRow.Cells["材料类别"].Value.ToString();
                    txtDepot.Tag = dataGridView1.CurrentRow.Cells["材料类别编码"].Value.ToString();
	            }
                else
                {
                    txtDepot.Text = "";
                    txtDepot.Tag = "";
                }

                if (dataGridView1.CurrentRow.Cells["单位"].Value != null)
                {
                    cmbUnit.Text = dataGridView1.CurrentRow.Cells["单位"].Value.ToString();
                }
                else
                {
                    cmbUnit.SelectedIndex = -1;
                }

                txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            }
        }

        private bool customForm_PanelGetDateInfo(CE_FlowOperationType flowOperationType)
        {
            if (flowOperationType == CE_FlowOperationType.提交 && !DataCheck())
            {
                return false;
            }

            BindingCollection<View_S_GoodsEnteringBill> tempList = 
                dataGridView1.DataSource as BindingCollection<View_S_GoodsEnteringBill>;

            ResultInfo = tempList;
            FlowInfo_BillNo = txtSDBNo.Text;

            List<object> listobj = new List<object>();

            listobj.Add((object)txtSDBNo.Text);
            listobj.Add((object)flowOperationType);

            ResultList = listobj;

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            BindingCollection<View_S_GoodsEnteringBill> tempList = dataGridView1.DataSource as BindingCollection<View_S_GoodsEnteringBill>;
            View_S_GoodsEnteringBill tempLnq = new View_S_GoodsEnteringBill();

            if (txtName.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("物品名称不能为空");
                return;
            }

            tempLnq.单据号 = txtSDBNo.Text;

            tempLnq.图号型号 = txtCode.Text.ToUpper().Trim();
            tempLnq.图号型号 = StapleFunction.DeleteSpace(tempLnq.图号型号);
            tempLnq.物品名称 = txtName.Text.Trim();
            tempLnq.物品名称 = tempLnq.物品名称.Replace('（', '(');
            tempLnq.物品名称 = tempLnq.物品名称.Replace('）', ')');
            tempLnq.规格 = txtSpec.Text.Trim();
            tempLnq.单位 = cmbUnit.Text;
            tempLnq.材料类别 = txtDepot.Text;
            tempLnq.材料类别编码 = txtDepot.Tag.ToString();
            tempLnq.备注 = txtRemark.Text.Trim();

            try
            {
                foreach (View_S_GoodsEnteringBill item in tempList)
                {
                    if (item.图号型号 == tempLnq.图号型号 && item.物品名称 == tempLnq.物品名称 && item.规格 == tempLnq.规格)
                    {
                        throw new Exception("已存在于当前列表中，无法重复添加");
                    }
                }

                CheckString(tempLnq.物品名称);
                m_serverGoodsEntering.CheckGoodsInfo(tempLnq);
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return;
            }

            tempList.Add(tempLnq);
            RefreshDataGridView(m_listGoodsEntering);
            ClearInfo();
        }

        void CheckString(string str)
        {
            char[] cc = str.ToCharArray();

            for (int i = 0; i < cc.Length; i++)
            {
                string checkString = cc[i].ToString();
                if (2 * checkString.Length == Encoding.Default.GetByteCount(checkString))
                {
                    if (!(cc[i] >= 0x4e00 && cc[i] <= 0x9fbb))
                    {
                        throw new Exception("存在全角字符");
                    }
                }

                if (i - 1 >= 0 && i + 1 < cc.Length)
                {
                    if (checkString == "x")
                    {
                        if (Regex.IsMatch(cc[i - 1].ToString(), "[0-9]")
                            && Regex.IsMatch(cc[i + 1].ToString(), "[0-9]"))
                        {
                            throw new Exception("x运用错误");
                        }
                    }

                    if (checkString == " ")
                    {
                        if ((cc[i - 1] >= 0x4e00 && cc[i - 1] <= 0x9fbb)
                            && (Regex.IsMatch(cc[i + 1].ToString(), "[0-9]")
                            || Regex.IsMatch(cc[i + 1].ToString(), "[a-zA-Z]")))
                        {
                            throw new Exception("中文与字母、中文与数字之间不允许存在空格");
                        }
                    }
                }
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            View_S_GoodsEnteringBill tempLnq = new View_S_GoodsEnteringBill();

            if (txtName.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("物品名称不能为空");
                return;
            }

            tempLnq.单据号 = txtSDBNo.Text;

            tempLnq.图号型号 = txtCode.Text.ToUpper().Trim();
            tempLnq.图号型号 = StapleFunction.DeleteSpace(tempLnq.图号型号);
            tempLnq.物品名称 = txtName.Text.Trim();
            tempLnq.物品名称 = tempLnq.物品名称.Replace('（', '(');
            tempLnq.物品名称 = tempLnq.物品名称.Replace('）', ')');
            tempLnq.规格 = txtSpec.Text.Trim();
            tempLnq.单位 = cmbUnit.Text;
            tempLnq.材料类别 = txtDepot.Text;
            tempLnq.材料类别编码 = txtDepot.Tag.ToString();
            tempLnq.备注 = txtRemark.Text.Trim();

            dataGridView1.CurrentRow.Cells["图号型号"].Value = txtCode.Text.Trim();

            #region 2016-11-22, 夏石友, 增加了去除全角、半角空格的功能
            dataGridView1.CurrentRow.Cells["物品名称"].Value = StapleFunction.DeleteSpace(txtName.Text.Trim());
            dataGridView1.CurrentRow.Cells["规格"].Value = StapleFunction.DeleteSpace(txtSpec.Text.Trim());
            #endregion

            dataGridView1.CurrentRow.Cells["材料类别"].Value = txtDepot.Text;
            dataGridView1.CurrentRow.Cells["材料类别编码"].Value = txtDepot.Tag;
            dataGridView1.CurrentRow.Cells["单位"].Value = cmbUnit.Text;
            dataGridView1.CurrentRow.Cells["备注"].Value = txtRemark.Text.Trim();


            try
            {
                CheckString(tempLnq.物品名称);
                m_serverGoodsEntering.CheckGoodsInfo(tempLnq);

                dataGridView1.CurrentRow.Cells["图号型号"].Value = tempLnq.图号型号;
                dataGridView1.CurrentRow.Cells["物品名称"].Value = tempLnq.物品名称;
                dataGridView1.CurrentRow.Cells["规格"].Value = tempLnq.规格;
                dataGridView1.CurrentRow.Cells["材料类别"].Value = tempLnq.材料类别;
                dataGridView1.CurrentRow.Cells["材料类别编码"].Value = tempLnq.材料类别编码;
                dataGridView1.CurrentRow.Cells["单位"].Value = tempLnq.单位;
                dataGridView1.CurrentRow.Cells["备注"].Value = tempLnq.备注;

            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return;
            }

            ClearInfo();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
            ClearInfo();
        }

        private void btnDepot_Click(object sender, EventArgs e)
        {
            FormDepotType form = new FormDepotType();

            if (form.ShowDialog() == DialogResult.OK)
            {
                txtDepot.Tag = form.SelectedDepotType.仓库编码;
                txtDepot.Text = form.SelectedDepotType.仓库名称;
            }
        }

        public override void LoadFormInfo()
        {
            try
            {
                m_lnqBillInfo.BillNo = this.FlowInfo_BillNo;
                m_billNoControl = new BillNumberControl(CE_BillTypeEnum.物料录入申请单.ToString(), m_serverGoodsEntering);
                m_billMessageServer.BillType = CE_BillTypeEnum.物料录入申请单.ToString();
                m_listGoodsEntering = m_serverGoodsEntering.GetListInfo(FlowInfo_BillNo);
                StapleInfo.InitUnitComboBox(cmbUnit);

                ClearInfo();
                ShowInfo();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
