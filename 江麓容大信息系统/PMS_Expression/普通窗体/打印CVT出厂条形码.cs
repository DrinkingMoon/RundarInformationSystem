using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;
using PlatformManagement;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 打印CVT出厂条形码 : Form
    {
        /// <summary>
        /// 产品条形码服务接口
        /// </summary>
        IProductBarcodeServer m_productBarcodeServer = ServerModuleFactory.GetServerModule<IProductBarcodeServer>();

        /// <summary>
        /// 产品条形码生成规则
        /// </summary>
        View_P_BuildRuleForVehicleBarcode m_buildRule = null;

        /// <summary>
        /// 打印设置单据信息
        /// </summary>
        View_P_PrintBillForVehicleBarcode m_printBill = null;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 授权标志
        /// </summary>
        AuthorityFlag m_authorityFlag;

        public 打印CVT出厂条形码(AuthorityFlag authFlag, View_P_PrintBillForVehicleBarcode bill)
        {
            InitializeComponent();

            m_authorityFlag = authFlag;

            cmbPrintMode.DataSource = m_productBarcodeServer.GetPrintMode();
            cmbPrintMode.DisplayMember = "PrintMode";

            InitDataGridView(bill);
        }

        /// <summary>
        /// 窗体加载
        /// 在构造函数中工具栏始终为不可见，需要在此处设置（原因不明）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 打印CVT出厂条形码_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip, m_authorityFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, m_authorityFlag);
        }

        /// <summary>
        /// 初始化数据显示控件
        /// </summary>
        /// <param name="bill">打印设置主表信息</param>
        private void InitDataGridView(View_P_PrintBillForVehicleBarcode bill)
        {
            m_printBill = bill;

            if (bill != null)
            {
                IQueryable<View_P_PrintListForVehicleBarcode> printList =
                    m_productBarcodeServer.GetPrintSettingList(m_printBill.打印编号);

                if (printList.Count() > 0)
                {
                    btnPrint.Enabled = true;
                    btnPrintSelectedList.Enabled = true;

                    txtPrintRemark.Text = m_printBill.打印说明;

                    dataGridView1.Rows.Clear();

                    foreach (var item in printList)
                    {
                        dataGridView1.Rows.Add(new object[] { 0, m_printBill.打印编号, item.打印规则编号, item.产品类型名称,
                            item.打印起始编号, item.打印结束编号, item.打印份数, item.打印模式, item.产品日期,
                            item.条形码构建规则, item.条形码示例 });

                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Tag = item;
                    }
                }
            }
        }

        /// <summary>
        /// 查找打印规则信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindRule_Click(object sender, EventArgs e)
        {
            FormQueryInfo dialog = QueryInfoDialog.GetBuildRuleForVehicleBarcode();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int buildRuleID = (int)dialog.GetDataItem("规则编号");

                txtBuildRuleID.Text = buildRuleID.ToString();
                txtProductType.Text = dialog.GetStringDataItem("产品类型名称");

                m_buildRule = m_productBarcodeServer.GetBuildRule(buildRuleID);
            }
        }

        /// <summary>
        /// 检查界面数据是否正确
        /// </summary>
        /// <returns></returns>
        private bool CheckData()
        {
            if (txtPrintRemark.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("打印说明不能为空");
                txtPrintRemark.Focus();
                return false;
            }

            if (txtBuildRuleID.Text == "")
            {
                MessageDialog.ShowPromptMessage("还没有设置条形码构建规则，不允许进行此操作");
                btnFindRule.Focus();
                return false;
            }

            if (numBeginNumber.Value == 0)
            {
                MessageDialog.ShowPromptMessage("打印起始编号不能为0");
                numBeginNumber.Focus();
                return false;
            }

            if (numEndNumber.Value == 0)
            {
                MessageDialog.ShowPromptMessage("打印结束编号不能为0");
                numEndNumber.Focus();
                return false;
            }

            if (numBeginNumber.Value > numEndNumber.Value)
            {
                MessageDialog.ShowPromptMessage("打印结束编号不能 < 打印起始编号");
                numEndNumber.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 添加打印项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckData())
            {
                return;
            }

            int printBillID = m_printBill == null ? 0 : m_printBill.打印编号;

            dataGridView1.Rows.Add(new object[] { 0, printBillID, Convert.ToInt32(txtBuildRuleID.Text), txtProductType.Text,
                numBeginNumber.Value, numEndNumber.Value, numPrintAmount.Value, cmbPrintMode.Text, dateTimePicker1.Value,
                m_buildRule.条形码构建规则, m_buildRule.条形码示例 });

            View_P_PrintListForVehicleBarcode listInfo = new View_P_PrintListForVehicleBarcode();

            listInfo.打印编号 = printBillID;
            listInfo.产品类型名称 = txtProductType.Text;
            listInfo.打印规则编号 = m_buildRule.规则编号;
            listInfo.打印起始编号 = Convert.ToInt32(numBeginNumber.Value);
            listInfo.打印结束编号 = Convert.ToInt32(numEndNumber.Value);
            listInfo.打印份数 = Convert.ToInt32(numPrintAmount.Value);
            listInfo.产品日期 = dateTimePicker1.Value;
            listInfo.打印模式编号 = ((P_PrintModeForVehicleBarcode)cmbPrintMode.SelectedItem).ID;
            listInfo.打印模式 = cmbPrintMode.Text;

            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Tag = listInfo;

            btnPrint.Enabled = false;
            btnPrintSelectedList.Enabled = false;
        }

        /// <summary>
        /// 检查是否正确选择操作的记录行
        /// </summary>
        /// <returns>正确返回true</returns>
        bool CheckSelectedRow()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择一条记录后再进行此操作");
                return false;
            }
            else if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("只允许选择一条记录进行处理！");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 修改明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
            {
                return;
            }

            View_P_PrintListForVehicleBarcode listInfo = dataGridView1.SelectedRows[0].Tag as View_P_PrintListForVehicleBarcode;

            int printBillID = m_printBill == null ? 0 : m_printBill.打印编号;
            int index = dataGridView1.SelectedRows[0].Index;

            dataGridView1.Rows.RemoveAt(index);

            dataGridView1.Rows.Insert(index, new object[] { 0, printBillID, Convert.ToInt32(txtBuildRuleID.Text), txtProductType.Text,
                numBeginNumber.Value, numEndNumber.Value, numPrintAmount.Value, cmbPrintMode.Text, dateTimePicker1.Value,
                m_buildRule.条形码构建规则, m_buildRule.条形码示例 });

            listInfo.打印编号 = printBillID;
            listInfo.产品类型名称 = txtProductType.Text;
            listInfo.打印规则编号 = m_buildRule.规则编号;
            listInfo.打印起始编号 = Convert.ToInt32(numBeginNumber.Value);
            listInfo.打印结束编号 = Convert.ToInt32(numEndNumber.Value);
            listInfo.打印份数 = Convert.ToInt32(numPrintAmount.Value);
            listInfo.产品日期 = dateTimePicker1.Value;
            listInfo.打印模式编号 = ((P_PrintModeForVehicleBarcode)cmbPrintMode.SelectedItem).ID;
            listInfo.打印模式 = cmbPrintMode.Text;

            dataGridView1.Rows[index].Tag = listInfo;

            btnPrint.Enabled = false;
            btnPrintSelectedList.Enabled = false;
        }

        /// <summary>
        /// 删除选择行数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
            {
                return;
            }

            dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);

            btnPrint.Enabled = false;
            btnPrintSelectedList.Enabled = false;
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckData())
            {
                return;
            }

            if (dataGridView1.Rows.Count == 0)
            {
                MessageDialog.ShowErrorMessage("还没有设置条形码列表，不允许进行此操作");
                return;
            }

            View_P_PrintBillForVehicleBarcode bill = m_printBill;

            if (bill == null)
            {
                bill = new View_P_PrintBillForVehicleBarcode();

                bill.工号 = BasicInfo.LoginID;
                bill.打印设置日期 = ServerTime.Time;
                bill.是否已经打印 = false;
                bill.打印说明 = txtPrintRemark.Text.Trim();
            }

            List<P_PrintListForVehicleBarcode> printList = new List<P_PrintListForVehicleBarcode>();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                View_P_PrintListForVehicleBarcode data = dataGridView1.Rows[i].Tag as View_P_PrintListForVehicleBarcode;

                P_PrintListForVehicleBarcode listInfo = new P_PrintListForVehicleBarcode();

                listInfo.BillID = data.打印编号;
                listInfo.BuildRuleID = data.打印规则编号;
                listInfo.BeginNumber = data.打印起始编号;
                listInfo.EndNumber = data.打印结束编号;
                listInfo.PrintDegree = data.打印份数;
                listInfo.Date = data.产品日期;
                listInfo.PrintMode = data.打印模式编号;

                printList.Add(listInfo);
            }
            
            if (m_productBarcodeServer.SavePrintSetting(bill, printList, out m_error))
            {
                m_printBill = m_productBarcodeServer.GetPrintSetting(bill.打印编号);

                InitDataGridView(m_printBill);

                btnPrint.Enabled = true;
                btnPrintSelectedList.Enabled = true;

                MessageDialog.ShowPromptMessage("操作成功");
            }
            else
            {
                MessageDialog.ShowErrorMessage(m_error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            View_P_PrintListForVehicleBarcode listInfo = dataGridView1.Rows[e.RowIndex].Tag as View_P_PrintListForVehicleBarcode;

            txtBuildRuleID.Text = listInfo.打印规则编号.ToString();
            txtProductType.Text = listInfo.产品类型名称;
            dateTimePicker1.Value = listInfo.产品日期;
            numBeginNumber.Value = listInfo.打印起始编号;
            numEndNumber.Value = listInfo.打印结束编号;
            numPrintAmount.Value = listInfo.打印份数;
            cmbPrintMode.Text = listInfo.打印模式;

            m_buildRule = m_productBarcodeServer.GetBuildRule(listInfo.打印规则编号);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 打印指定列表
        /// </summary>
        /// <param name="listPrint">要打印列表</param>
        private void Print(List<View_P_PrintListForVehicleBarcode> listPrint)
        {
            for (int i = 0; i < listPrint.Count; i++)
            {
                List<string> lstPrintInfo = new List<string>();

                View_P_PrintListForVehicleBarcode data = listPrint[i];

                if (m_productBarcodeServer.IsPrint(data))
                {
                    if (MessageDialog.ShowEnquiryMessage(string.Format("{0},产品日期:{1},打印起始编号【{2}】,打印结束编号【{3}】的条形码有部分或全部条形码已经打印过，是否继续打印?", 
                        data.产品类型名称, data.产品日期.ToString("yyyy年MM月"),
                        data.打印起始编号, data.打印结束编号)) == DialogResult.No)

                        return;
                }

                if (data.打印模式 == "按序打完所有条码后再按打印次数打下一遍")
                {
                    for (int k = 0; k < data.打印份数; k++)
                    {
                        for (int j = data.打印起始编号; j <= data.打印结束编号; j++)
                        {
                            lstPrintInfo.Add(m_productBarcodeServer.GetFormatStringOfBuildRule(data.打印规则编号, data.产品日期, j));
                        }
                    }
                }
                else
                {
                    for (int j = data.打印起始编号; j <= data.打印结束编号; j++)
                    {
                        for (int k = 0; k < data.打印份数; k++)
                        {
                            lstPrintInfo.Add(m_productBarcodeServer.GetFormatStringOfBuildRule(data.打印规则编号, data.产品日期, j));
                        }
                    }
                }

                foreach (var barcode in lstPrintInfo)
                {
                    if (!PrintPartBarcode.PrintBarcodeForVehicle(barcode))
                    {
                        MessageDialog.ShowErrorMessage(string.Format("打印 {0} 失败！", barcode));
                        return;
                    }
                }

                P_PrintLogForVehicleBarcode log = new P_PrintLogForVehicleBarcode();

                log.BeginNumber = data.打印起始编号;
                log.EndNumber = data.打印结束编号;
                log.BuildRuleID = data.打印规则编号;

                log.UserCode = BasicInfo.LoginID;
                log.Date = ServerTime.Time;

                log.ProductDate = data.产品日期;
                log.Reason = m_printBill.打印说明;
                log.PrintDegree = data.打印份数;
                log.PrintMode = data.打印模式编号;

                if (!m_productBarcodeServer.WritePrintLog(log, out m_error))
                {
                    MessageDialog.ShowErrorMessage(m_error);
                }
            }
        }

        /// <summary>
        /// 打印条形码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            List<View_P_PrintListForVehicleBarcode> listPrint = new List<View_P_PrintListForVehicleBarcode>();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                View_P_PrintListForVehicleBarcode data = dataGridView1.Rows[i].Tag as View_P_PrintListForVehicleBarcode;

                listPrint.Add(data);
            }

            Print(listPrint);

            if (!m_productBarcodeServer.SavePrintFlag(m_printBill.打印编号, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
            }
        }

        /// <summary>
        /// 打印选择的明细条形码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrintSelectedList_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
            {
                return;
            }

            List<View_P_PrintListForVehicleBarcode> listPrint = new List<View_P_PrintListForVehicleBarcode>();

            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                View_P_PrintListForVehicleBarcode data = dataGridView1.SelectedRows[i].Tag as View_P_PrintListForVehicleBarcode;

                listPrint.Add(data);
            }

            Print(listPrint);
        }
    }
}
