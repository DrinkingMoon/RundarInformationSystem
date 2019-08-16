/******************************************************************************
 *
 * 文件名称:  下线返修记录管理.cs
 * 作者    :  邱瑶       日期: 2013/11/04
 * 开发平台:  vs2008(c#)
 * 用于    :  生产线管理信息系统
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using Form_Peripheral_HR;
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 下线返修记录管理 : Form
    {
        /// <summary>
        /// 下线试验结果信息服务
        /// </summary>
        ICVTRepairInfoServer m_testServer = PMS_ServerFactory.GetServerModule<ICVTRepairInfoServer>();

        /// <summary>
        /// 产品信息服务
        /// </summary>
        private IProductInfoServer m_productInfoServer = PMS_ServerFactory.GetServerModule<IProductInfoServer>();

        /// <summary>
        /// 变速箱箱号
        /// </summary>
        string m_productNumber;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 查询列名
        /// </summary>
        List<string> m_lstFindField = new List<string>();

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        public 下线返修记录管理(FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            IQueryable<View_P_ProductInfo> productInfo = null;

            if (!m_productInfoServer.GetAllProductInfo(out productInfo, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }
            else
            {
                productInfo = from r in productInfo
                              where !r.产品类型名称.Contains("返修")
                              select r;

                cmbProductCode.DataSource = productInfo;
                cmbProductCode.DisplayMember = "产品类型编码";
                cmbProductCode.ValueMember = "产品类型编码";
            }

            SearchData();
        }

        private void 下线返修记录管理_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authFlag);
        }

        /// <summary>
        /// 选择返修人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFind_Click(object sender, EventArgs e)
        {
            FormSelectPersonnel form = new FormSelectPersonnel("员工");

            form.DeptCode = BasicInfo.DeptCode;

            if (form.ShowDialog() == DialogResult.OK)
            {
                txtAssembler.Text = "";

                for (int i = 0; i < form.SelectedUser.Count; i++)
                {
                    txtAssembler.Text += form.SelectedUser[i].员工姓名 + ";";
                }
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            // 校验箱号录入是否正确
            m_productNumber = cmbProductCode.Text + " " + txtOldProductCode.Text.Trim();

            if (m_testServer.CanOffLineTest(m_productNumber, out m_error))
            {
                MessageDialog.ShowPromptMessage("旧箱检测通过");
            }
            else
            {
                MessageDialog.ShowPromptMessage("旧箱" + m_error);
            }

            // 校验箱号录入是否正确
            m_productNumber = cmbProductCode.Text + " " + txtNewProductCode.Text.Trim();

            if (m_testServer.CanOffLineTest(m_productNumber, out m_error))
            {
                MessageDialog.ShowPromptMessage("新箱检测通过");
            }
            else
            {
                MessageDialog.ShowPromptMessage("新箱" + m_error);
            }
        }

        /// <summary>
        /// 检索实体数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchProduct_Click(object sender, EventArgs e)
        {
            View_ZPX_CVTRepairInfo data = new View_ZPX_CVTRepairInfo();

            data.产品型号 = cmbProductCode.Text;
            data.新箱号 = GlobalObject.GeneralFunction.IsNullOrEmpty(txtProductNumber.Text) ? null : txtProductNumber.Text;
            data.旧箱号 = GlobalObject.GeneralFunction.IsNullOrEmpty(txtProductNumber.Text) ? null : txtProductNumber.Text;

            RefreshDataGridView(m_testServer.GetViewData(data));
        }

        /// <summary>
        /// 刷新数据控件
        /// </summary>
        /// <param name="data">数据集</param>
        private void RefreshDataGridView(IEnumerable<View_ZPX_CVTRepairInfo> data)
        {
            dataGridView1.DataSource = data.ToList();

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
               UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

            dataGridView1.Refresh();
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        /// <summary>
        /// 新建
        /// </summary>
        private void btnNew_Click(object sender, EventArgs e)
        {
            cmbProductCode.SelectedIndex = -1;
            txtProductNumber.Text = "";
            txtFault.Text = "";
            txtAssociateBillNoBF.Text = "";
            txtAssociaBillNoTK.Text = "";
            txtAssembler.Text = "";
            txtBonnetCode.Text = "";
            txtDuty.Text = "";
            txtNewProductCode.Text = "";
            txtOldProductCode.Text = "";
            txtScheme.Text = "";
            numTaskTime.Value = 0;
            cmbRepairType.SelectedIndex = -1;
            cmbAssociateType.SelectedIndex = -1;
            lbStatus.Text = "新建单据";
        }

        /// <summary>
        /// 按日期检索数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchData();
        }

        /// <summary>
        /// 检索数据
        /// </summary>
        void SearchData()
        {
            List<View_ZPX_CVTRepairInfo> result =
                m_testServer.GetViewData(dateTimePickerST.Value.Date, dateTimePickerET.Value.Date.AddDays(1)).ToList();

            RefreshDataGridView(result);
        }

        private void 下线返修记录管理_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void 删除toolStripButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (MessageDialog.ShowEnquiryMessage("您确定删除选中的记录吗？") == DialogResult.Yes)
                {
                    if (!m_testServer.DeleteRepairInfo(Convert.ToInt32(dataGridView1.CurrentRow.Cells["序号"].Value)))
                    {
                        MessageDialog.ShowPromptMessage("删除失败！");
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("删除成功！");

                        SearchData();
                    }
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选中一行记录后再进行此操作！");
            }
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(this.labelTitle.Text, e.Column);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtAssembler.Text = dataGridView1.CurrentRow.Cells["返修人员"].Value.ToString();
            txtBonnetCode.Text = dataGridView1.CurrentRow.Cells["阀块编号"].Value
                == DBNull.Value ? "" : (string)dataGridView1.CurrentRow.Cells["阀块编号"].Value;
            txtDuty.Text = dataGridView1.CurrentRow.Cells["责任判定"].Value 
                == DBNull.Value ? "" : (string)dataGridView1.CurrentRow.Cells["责任判定"].Value;
            txtFault.Text = dataGridView1.CurrentRow.Cells["故障现象"].Value.ToString();
            txtNewProductCode.Text = dataGridView1.CurrentRow.Cells["新箱号"].Value 
                == DBNull.Value ? "" : (string)dataGridView1.CurrentRow.Cells["新箱号"].Value;
            txtOldProductCode.Text = dataGridView1.CurrentRow.Cells["旧箱号"].Value
                == DBNull.Value ? "" : (string)dataGridView1.CurrentRow.Cells["旧箱号"].Value;
            txtScheme.Text = dataGridView1.CurrentRow.Cells["返修方案及明细"].Value.ToString();
            cmbProductCode.Text = dataGridView1.CurrentRow.Cells["产品型号"].Value.ToString();
            cmbRepairType.Text = dataGridView1.CurrentRow.Cells["返修类型"].Value.ToString();
            cbTest.Checked = (string)dataGridView1.CurrentRow.Cells["是否需要重新跑试验"].Value == "需要" ? true : false;
            numTaskTime.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["工时"].Value);
            lbStatus.Text = dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString();
            cmbAssociateType.Text = dataGridView1.CurrentRow.Cells["关联单据类别"].Value
                == DBNull.Value ? "" : (string)dataGridView1.CurrentRow.Cells["关联单据类别"].Value;

            if (cmbAssociateType.Text.Contains("报废单"))
            {
                txtAssociateBillNoBF.Text = (string)dataGridView1.CurrentRow.Cells["关联单号"].Value;
                txtAssociateBillNoBF.Visible = true;
                txtAssociaBillNoTK.Visible = false;
            }
            else
            {
                txtAssociaBillNoTK.Text = (string)dataGridView1.CurrentRow.Cells["关联单号"].Value;
                txtAssociaBillNoTK.Visible = true;
                txtAssociateBillNoBF.Visible = false;
            }

            if (cmbRepairType.SelectedIndex != 3)
            {
                txtProductNumber.Text = dataGridView1.CurrentRow.Cells["新箱号"].Value
                == DBNull.Value ? "" : (string)dataGridView1.CurrentRow.Cells["新箱号"].Value;
                txtNewProductCode.Text = "";
            }
        }

        private void 查看关联单据的信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cmbAssociateType.Text == "报废单")
            {
                ((IMainForm)StapleInfo.MainForm).ShowForm("报废单", WndMsgSender.PositioningMsg, txtAssociateBillNoBF.Text);
            }
            else
            {
                ((IMainForm)StapleInfo.MainForm).ShowForm("营销退库单", WndMsgSender.PositioningMsg, txtAssociaBillNoTK.Text);
            }            
        }

        /// <summary>
        /// 检查界面上的数据是否有效，为添加、更新、删除操作作准备
        /// </summary>
        /// <returns>有效返回true</returns>
        private bool CheckData()
        {
            m_productNumber = "";

            if (cmbProductCode.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择产品型号后再进行此操作！");

                cmbProductCode.Focus();
                return false;
            }

            if (numTaskTime.Value <= 0)
            {
                MessageDialog.ShowPromptMessage("请填写工时！");
                return false;
            }

            if (cmbRepairType.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择返修类型后再进行此操作！");
                return false;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtProductNumber.Text.Trim())
                && GlobalObject.GeneralFunction.IsNullOrEmpty(txtOldProductCode.Text.Trim())
                && GlobalObject.GeneralFunction.IsNullOrEmpty(txtNewProductCode.Text.Trim()))
            {
                MessageDialog.ShowPromptMessage("请录入产品编号后再进行此操作！");

                txtProductNumber.Focus();

                return false;
            }

            // 校验箱号录入是否正确
           
            if (cmbRepairType.SelectedIndex == 3)
            {
                if (txtOldProductCode.Text.Trim() == "" || txtNewProductCode.Text.Trim() == "" || txtBonnetCode.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写旧箱编号、新箱编号、阀块编号！");
                    return false;
                }

                //m_productNumber = cmbProductCode.Text + " " + txtOldProductCode.Text.Trim();

                //if (!m_testServer.CanOffLineTest(m_productNumber, out m_error))
                //{
                //    MessageDialog.ShowPromptMessage(m_error);
                //    return false;
                //}

                //m_productNumber = cmbProductCode.Text + " " + txtOldProductCode.Text.Trim();

                //if (!m_testServer.CanOffLineTest(m_productNumber, out m_error))
                //{
                //    MessageDialog.ShowPromptMessage(m_error);
                //    return false;
                //}
            }
            else
            {
                //m_productNumber = cmbProductCode.Text + " " + txtProductNumber.Text.Trim();

                //if (!m_testServer.CanOffLineTest(m_productNumber, out m_error))
                //{
                //    MessageDialog.ShowPromptMessage(m_error);
                //    return false;
                //}

                if (!SCM_Level02_ServerFactory.GetServerModule<IProductCodeServer>().VerifyProductCodesInfo(
                cmbProductCode.Text, txtProductNumber.Text, GlobalObject.CE_BarCodeType.内部钢印码, out m_error))
                {
                    MessageDialog.ShowErrorMessage(m_error);

                    txtProductNumber.Focus();

                    return false;
                }
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtAssembler.Text))
            {
                MessageDialog.ShowPromptMessage("请选择预装员后再进行此操作");

                return false;
            }

            txtFault.Text = txtFault.Text.Trim();

            if (cmbAssociateType.SelectedIndex == 0)
            {
                if (txtAssociateBillNoBF.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请选择关联单号后再进行此操作");
                    return false;
                }
            }

            if (cmbAssociateType.SelectedIndex == 1 && txtAssociaBillNoTK.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择关联单号后再进行此操作");
                return false;
            }

            if (txtFault.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写故障现象后再进行此操作");
                return false;
            }

            if (txtScheme.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写返修方案后再进行此操作");
                return false;
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckData())
            {
                return;
            }

            ZPX_CVTRepairInfo data = new ZPX_CVTRepairInfo();

            data.ProductType = cmbProductCode.Text;

            if (cmbRepairType.SelectedIndex != 3)
            {
                data.ProductNewNumber = txtProductNumber.Text;
            }
            else
            {
                data.ProductOldNumber = txtOldProductCode.Text;
                data.ProductNewNumber = txtNewProductCode.Text;
                data.BonnetNumber = txtBonnetCode.Text;
            }

            data.Assembler = txtAssembler.Text.ToString();
            data.Fault = txtFault.Text;
            data.Scheme = txtScheme.Text.Trim();
            data.IsTest = cbTest.Checked;
            data.Hours = numTaskTime.Value;
            data.Recorder = BasicInfo.LoginID;
            data.RecordTime = ServerTime.Time;
            data.Status = "等待审核";
            data.RepairType = cmbRepairType.Text;

            if (cmbAssociateType.SelectedIndex != -1)
            {
                data.AssociateType = cmbAssociateType.Text;

                if (cmbAssociateType.SelectedIndex == 0)
                {
                    data.AssociateBillNo = txtAssociateBillNoBF.Text;
                }
                else
                {
                    data.AssociateBillNo = txtAssociaBillNoTK.Text;
                }
            }

            try
            {
                m_testServer.Insert(data);

                RefreshDataGridView(m_testServer.GetViewData(new View_ZPX_CVTRepairInfo() { 序号 = data.ID }));
            }
            catch (Exception exce)
            {
                MessageDialog.ShowErrorMessage(exce.Message);
            }
        }

        private void cmbAssociateType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAssociateType.SelectedIndex == 0)
            {
                txtAssociateBillNoBF.Visible = true;
                txtAssociaBillNoTK.Visible = false;
            }
            else
            {
                txtAssociaBillNoTK.Visible = true;
                txtAssociateBillNoBF.Visible = false;
            }
        }

        /// <summary>
        /// 从dataGridView中获取选择的数据
        /// </summary>
        /// <returns>返回获取到的数据</returns>
        private View_ZPX_CVTRepairInfo GetSelectedData()
        {
            List<View_ZPX_CVTRepairInfo> result =
             dataGridView1.DataSource as List<View_ZPX_CVTRepairInfo>;

            DataGridViewCellCollection cells = dataGridView1.SelectedRows[0].Cells;
            View_ZPX_CVTRepairInfo data = null;

            if (result == null)
            {
                data = new View_ZPX_CVTRepairInfo();

                data.序号 = (int)cells["序号"].Value;
                data.产品型号 = (string)cells["产品型号"].Value;
                data.旧箱号 = (string)cells["旧箱号"].Value;
                data.新箱号 = (string)cells["新箱号"].Value;
                data.返修人员 = (string)cells["返修人员"].Value;
                data.故障现象 = (string)cells["故障现象"].Value;
                data.返修方案及明细 = (string)cells["返修方案及明细"].Value;
                data.是否需要重新跑试验 = (string)cells["是否需要重新跑试验"].Value;
                data.记录时间 = (DateTime)cells["记录时间"].Value;
                data.记录人员 = (string)cells["记录人员"].Value;
                data.返修类型 = (string)cells["返修类型"].Value;
                data.单据状态 = (string)cells["单据状态"].Value;
                data.阀块编号 = (string)cells["阀块编号"].Value;
                data.工时 = (decimal)cells["工时"].Value;
                data.关联单号 = (string)cells["关联单号"].Value;
                data.关联单据类别 = (string)cells["关联单据类别"].Value;
                data.试验状态 = (string)cells["试验状态"].Value;
                data.责任判定 = (string)cells["责任判定"].Value;
            }
            else
            {
                data = result.Single(p => p.序号 == (int)cells["序号"].Value);
            }

            return data;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lbStatus.Text == "等待审核")
            {
                if (!CheckData())
                    return;

                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageDialog.ShowPromptMessage("请选择要操作的记录后再进行此操作");
                    return;
                }

                View_ZPX_CVTRepairInfo data = GetSelectedData();

                data.产品型号 = cmbProductCode.Text;

                if (cmbRepairType.SelectedIndex != 3)
                {
                    data.旧箱号 = txtProductNumber.Text;
                }
                else
                {
                    data.旧箱号 = txtOldProductCode.Text;
                    data.新箱号 = txtNewProductCode.Text;
                    data.阀块编号 = txtBonnetCode.Text;
                }

                data.返修人员 = txtAssembler.Text.ToString();
                data.故障现象 = txtFault.Text;
                data.返修方案及明细 = txtScheme.Text.Trim();
                data.是否需要重新跑试验 = cbTest.Checked == true ? "需要" : "不需要";
                data.工时 = numTaskTime.Value;
                data.返修类型 = cmbRepairType.Text;
                data.单据状态 = "等待审核";

                if (cmbAssociateType.SelectedIndex != -1)
                {
                    data.关联单据类别 = cmbAssociateType.Text;

                    if (cmbAssociateType.SelectedIndex == 0)
                    {
                        data.关联单号 = txtAssociateBillNoBF.Text;
                    }
                    else
                    {
                        data.关联单号 = txtAssociaBillNoTK.Text;
                    }
                }

                try
                {
                    m_testServer.Update(data);

                    MessageDialog.ShowPromptMessage("执行成功");

                    RefreshDataGridView(m_testServer.GetViewData(new View_ZPX_CVTRepairInfo() { 序号 = data.序号 }));
                }
                catch (Exception exce)
                {
                    MessageDialog.ShowErrorMessage(exce.Message);
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
            }
        }

        private void btnAuditing_Click(object sender, EventArgs e)
        {
            if (lbStatus.Text == "等待质检判定")
            {
                if (txtDuty.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写责任判定！");
                    return;
                }

                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageDialog.ShowPromptMessage("请选择要确认判定的行后再进行此操作");
                    return;
                }

                try
                {
                    for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                    {
                        m_testServer.Auditing((int)dataGridView1.SelectedRows[i].Cells["序号"].Value, txtDuty.Text);
                    }

                    SearchData();
                }
                catch (Exception exce)
                {
                    MessageDialog.ShowErrorMessage(exce.Message);
                }
            }
            else 
            {
                MessageDialog.ShowPromptMessage("请确认单据状态");
            }
        }

        private void btnAdvSearch_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "下线返修信息管理";
            IQueryResult qr = authorization.Query(businessID, null, null, 0);
            List<string> lstFindField = new List<string>();
            DataColumnCollection columns = qr.DataCollection.Tables[0].Columns;

            if (qr.Succeeded && columns.Count > 0)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    lstFindField.Add(columns[i].ColumnName);
                }
            }

            FormConditionFind formFindCondition = new FormConditionFind(this, lstFindField.ToArray(), businessID, labelTitle.Text);
            formFindCondition.ShowDialog();
        }

        private void 刷新toolStripButton_Click(object sender, EventArgs e)
        {
            SearchData();
        }

        private void 审核toolStripButton_Click(object sender, EventArgs e)
        {
            if (lbStatus.Text.Trim() == "等待审核")
            {
                if (BasicInfo.LoginName == dataGridView1.CurrentRow.Cells["记录人员"].Value.ToString())
                {
                    if (!CheckData())
                        return;

                    if (dataGridView1.SelectedRows.Count == 0)
                    {
                        MessageDialog.ShowPromptMessage("请选择要操作的记录后再进行此操作");
                        return;
                    }

                    View_ZPX_CVTRepairInfo data = GetSelectedData();

                    data.产品型号 = cmbProductCode.Text;

                    if (cmbRepairType.SelectedIndex != 3)
                    {
                        data.旧箱号 = txtProductNumber.Text;
                    }
                    else
                    {
                        data.旧箱号 = txtOldProductCode.Text;
                        data.新箱号 = txtNewProductCode.Text;
                        data.阀块编号 = txtBonnetCode.Text;
                    }

                    data.返修人员 = txtAssembler.Text.ToString();
                    data.故障现象 = txtFault.Text;
                    data.返修方案及明细 = txtScheme.Text.Trim();
                    data.是否需要重新跑试验 = cbTest.Checked == true ? "需要" : "不需要";
                    data.工时 = numTaskTime.Value;
                    data.返修类型 = cmbRepairType.Text;
                    data.单据状态 = "等待质检判定";

                    if (cmbAssociateType.SelectedIndex != -1)
                    {
                        data.关联单据类别 = cmbAssociateType.Text;

                        if (cmbAssociateType.SelectedIndex == 0)
                        {
                            data.关联单号 = txtAssociateBillNoBF.Text;
                        }
                        else
                        {
                            data.关联单号 = txtAssociaBillNoTK.Text;
                        }
                    }

                    try
                    {
                        m_testServer.Update(data);

                        MessageDialog.ShowPromptMessage("执行成功");

                        RefreshDataGridView(m_testServer.GetViewData(new View_ZPX_CVTRepairInfo() { 序号 = data.序号 }));
                    }
                    catch (Exception exce)
                    {
                        MessageDialog.ShowErrorMessage(exce.Message);
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("您不是记录人员不能审核！");
                    return;
                }
            }
        }
    }
}
