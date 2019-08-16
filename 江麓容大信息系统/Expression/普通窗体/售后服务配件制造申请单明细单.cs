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
using Service_Manufacture_Storage;

namespace Expression
{
    /// <summary>
    /// 售后服务配件制造申请明细界面
    /// </summary>
    public partial class 售后服务配件制造申请单明细单 : Form
    {
        /// <summary>
        /// 发料清单服务组件
        /// </summary>
        Service_Manufacture_Storage.IProductOrder m_serverProductOrder = Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<Service_Manufacture_Storage.IProductOrder>();

        /// <summary>
        /// 部门信息服务组件
        /// </summary>
        ServerModule.IDepartmentServer m_serverDepartment = ServerModule.ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer =
            BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgator = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 售后服务配件制造申请单服务接口
        /// </summary>
        ServerModule.IAfterServiceMakePartsBill m_billServer = ServerModule.ServerModuleFactory.GetServerModule<IAfterServiceMakePartsBill>();

        /// <summary>
        /// 主表信息
        /// </summary>
        YX_AfterServiceMakePartsBill m_masterInfo = new YX_AfterServiceMakePartsBill();

        /// <summary>
        /// 整台领料时显示用的信息
        /// </summary>
        StringBuilder m_sb;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 单据号
        /// </summary>
        private string m_bill;

        public string Bill
        {
            get { return m_bill; }
            set { m_bill = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="billID">单据号</param>
        public 售后服务配件制造申请单明细单(string billID, AuthorityFlag m_authFlag)
        {

            InitializeComponent();

            m_msgPromulgator.BillType = "售后服务配件制造申请单";

            FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);
            FaceAuthoritySetting.SetVisibly(this.toolStrip, m_authFlag);
            this.toolStrip.Visible = true;

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.售后服务备件制造申请单, m_billServer);

            DataTable dtStorageName = UniversalFunction.GetStorageTb();

            for (int i = 0; i < dtStorageName.Rows.Count; i++)
            {
                if (dtStorageName.Rows[i]["StorageName"].ToString() == "制造库房" 
                    || dtStorageName.Rows[i]["StorageName"].ToString() == "自制半成品库")
                {
                    cmbStorageID.Items.Add(dtStorageName.Rows[i]["StorageName"].ToString());
                }
            }

            cmbStorageID.SelectedIndex = -1;

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(billID))
            {
                lbUserName.Text = BasicInfo.LoginName;
                txtSellID.Text = m_billNoControl.GetNewBillNo();
                txtSellID.Tag = "New";
                lbKS.Text = m_serverDepartment.GetDeptInfoFromPersonnelInfo(BasicInfo.LoginName).Rows[0]["DepartmentName"].ToString();
            }
            else
            {
                m_masterInfo = m_billServer.GetBill(billID);
                cmbStorageID.Text = UniversalFunction.GetStorageName(m_masterInfo.StorageID);
                txtSellID.Text = m_masterInfo.BillID;
                txtBillRemark.Text = m_masterInfo.Remark;
                lbUserName.Text = m_masterInfo.Applicant;
                lbKS.Text = m_serverDepartment.GetDeptInfoFromPersonnelInfo(m_masterInfo.Applicant).Rows[0]["DepartmentName"].ToString();               
            }

            m_bill = txtSellID.Text;

            RefershDataGridView(m_bill);
        }

        void txtProductCode_OnCompleteSearch()
        {
            txtProductCode.Text = txtProductCode.DataResult["图号型号"].ToString();
        }

        void txtGoodsName_OnCompleteSearch()
        {
            txtGoodsCode.Text = txtGoodsName.DataResult["图号型号"].ToString();
            txtGoodsName.Text = txtGoodsName.DataResult["物品名称"].ToString();
            txtGoodsName.Tag = Convert.ToInt32(txtGoodsName.DataResult["序号"]);
            txtSpce.Text = txtGoodsName.DataResult["规格"].ToString();
            lbUnit.Text = txtGoodsName.DataResult["单位"].ToString();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dataGridView1.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView1.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        void ClearDate()
        {
            txtGoodsCode.Text = "";
            txtGoodsName.Text = "";
            txtGoodsName.Tag = -1;
            txtRemark.Text = "";
            txtSpce.Text = "";
        }

        /// <summary>
        /// 刷新datagirdview
        /// </summary>
        /// <param name="strbillid">单据号</param>
        void RefershDataGridView(string strbillid)
        {
            dataGridView1.DataSource = m_billServer.GetList(strbillid) ;
            dataGridView1.Columns["单据号"].Visible = false;
            dataGridView1.Columns["序号"].Visible = false;
        }

        /// <summary>
        /// 获得主表信息
        /// </summary>
        void GetBillMessage()
        {
            m_masterInfo.BillID = txtSellID.Text;
            m_masterInfo.Remark = txtBillRemark.Text.Trim();
            m_masterInfo.RequestDate = ServerTime.Time;
            m_masterInfo.Applicant = lbUserName.Text;
            m_masterInfo.StorageID = UniversalFunction.GetStorageID(cmbStorageID.Text);
        }

        /// <summary>
        /// 检查信息是否合法
        /// </summary>
        /// <returns>检测通过返回True，否则返回False</returns>
        bool CheckForm()
        {
            if (txtGoodsName.Text.Trim() == "" && Convert.ToInt32( txtGoodsName.Tag) == -1)
            {
                MessageDialog.ShowPromptMessage("请选择物品");
                return false;
            }

            if (nudCount.Value == 0)
            {
                MessageDialog.ShowPromptMessage("数量不可为0，请填写数量");
                return false;
            }

            if (cmbStorageID.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择所属库房");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检查是否有相同物品
        /// </summary>
        /// <returns>返回TRUE为无相同物品，返回FALSE为存在相同物品</returns>
        bool CheckSameGoods()
        {
            int intGoodsID = Convert.ToInt32(txtGoodsName.Tag);

            DataTable dtTemp = (DataTable)dataGridView1.DataSource;

            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                if (intGoodsID == Convert.ToInt32(dtTemp.Rows[i]["物品ID"]))
                {
                    return false;
                }
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            GetBillMessage();

            if (!m_billServer.UpdateBill(m_masterInfo,(DataTable)dataGridView1.DataSource,
                AfterServiceMakePartsBillStatus.营销保存,out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
            }
            else
            {
                MessageDialog.ShowPromptMessage("保存成功");
                m_msgPromulgator.DestroyMessage(txtSellID.Text);
                m_msgPromulgator.SendNewFlowMessage(txtSellID.Text,
                    string.Format("{0} 号售后服务配件制造申请单，请主管审核", txtSellID.Text), BillFlowMessage_ReceivedUserType.角色,
                    m_billMessageServer.GetSuperior(CE_RoleStyleType.上级领导, BasicInfo.LoginID));

                txtSellID.Tag = null;
                this.Close();
            }

            RefershDataGridView(txtSellID.Text);
        }

        private void btnSh_Click(object sender, EventArgs e)
        {
            GetBillMessage();

            if (!m_billServer.UpdateBill(m_masterInfo, (DataTable)dataGridView1.DataSource,
                AfterServiceMakePartsBillStatus.等待主管审核, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
            }
            else
            {
                MessageDialog.ShowPromptMessage("审核成功");

                List<string> strList = new List<string>();
                strList.Add(CE_RoleEnum.制造负责人.ToString());
                strList.Add(CE_RoleEnum.下线主管.ToString());
                strList.Add(CE_RoleEnum.制造部办公室文员.ToString());
                strList.Add(CE_RoleEnum.下线信息员.ToString());

                m_msgPromulgator.PassFlowMessage(m_masterInfo.BillID,
                    string.Format("{0} 号售后服务配件制造申请单，请车间确认", m_masterInfo.BillID), 
                    BillFlowMessage_ReceivedUserType.角色, strList);

                this.Close();
            }

            RefershDataGridView(txtSellID.Text);
        }

        private void btnAffirm_Click(object sender, EventArgs e)
        {
            GetBillMessage();

            if (!m_billServer.UpdateBill(m_masterInfo, (DataTable)dataGridView1.DataSource,
                AfterServiceMakePartsBillStatus.等待车间确认, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
            }
            else
            {
                m_billNoControl.UseBill(m_masterInfo.BillID);
                MessageDialog.ShowPromptMessage("确认成功");

                #region 发送知会消息

                List<string> noticeRoles = new List<string>();

                string strDept = m_serverDepartment.GetDeptInfoFromPersonnelInfo(
                    m_billServer.GetBill(txtSellID.Text).Applicant).Rows[0]["DepartmentCode"].ToString();
                noticeRoles.AddRange(m_billMessageServer.GetDeptDirectorRoleName(strDept));

                m_msgPromulgator.EndFlowMessage(m_masterInfo.BillID,
                    string.Format("{0} 号售后服务配件制造申请单已经处理完毕", m_masterInfo.BillID),
                    noticeRoles, null);

                #endregion 发送知会消息

                this.Close();
            }

            RefershDataGridView(txtSellID.Text);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteRow(dataGridView1.CurrentRow.Index);
            ClearDate();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (CheckForm())
            {
                if (CheckSameGoods())
                {
                    DataTable dtTemp = (DataTable)dataGridView1.DataSource;

                    DataRow drTemp = dtTemp.NewRow();

                    drTemp["物品ID"] = Convert.ToInt32(txtGoodsName.Tag);
                    drTemp["图号型号"] = txtGoodsCode.Text;
                    drTemp["物品名称"] = txtGoodsName.Text;
                    drTemp["规格"] = txtSpce.Text;
                    drTemp["总成型号"] = txtProductCode.Text;
                    drTemp["数量"] = nudCount.Value;
                    drTemp["单位"] = lbUnit.Text;
                    drTemp["备注"] = txtRemark.Text;
                    drTemp["单据号"] = txtSellID.Text;
                    drTemp["序号"] = 0;

                    dtTemp.Rows.Add(drTemp);

                    dataGridView1.DataSource = dtTemp;

                    ClearDate();
                }
            }
        }

        /// <summary>
        /// 删除DataGridView中的指定行索引的记录
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        private void DeleteRow(int rowIndex)
        {
            DataTable dtTemp = (DataTable)dataGridView1.DataSource;

            if (dtTemp == null || rowIndex >= dtTemp.Rows.Count)
            {
                MessageDialog.ShowErrorMessage("无法删除不存在的记录");
                return;
            }

            dtTemp.Rows.RemoveAt(rowIndex);
            dataGridView1.DataSource = dtTemp;
            dataGridView1.Refresh();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DeleteRow(dataGridView1.CurrentRow.Index);

            btnAdd_Click(sender, e);
        }

        private void 售后服务配件制造申请单明细单_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (txtSellID.Tag != null)
            {
                m_billNoControl.CancelBill(txtSellID.Text);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            txtGoodsName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
            txtGoodsName.Tag = Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value);
            txtGoodsCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
            txtSpce.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
            txtProductCode.Text = dataGridView1.CurrentRow.Cells["总成型号"].Value.ToString();
            txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            nudCount.Value = Convert.ToInt32(dataGridView1.CurrentRow.Cells["数量"].Value);
            lbUnit.Text = dataGridView1.CurrentRow.Cells["单位"].Value.ToString();
        }

        private void btnCreateMaterialRequisition_Click(object sender, EventArgs e)
        {
            if (m_masterInfo.Status.ToString() == "已完成")
            {
                if (!m_billServer.AutogenerationMaterialRequisition(m_masterInfo.BillID,
                    m_masterInfo.StorageID, out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                    return;
                }
                else
                {
                    MessageDialog.ShowPromptMessage("自动生成成功！");

                    string strOutMessage = "";

                    if (m_err != null && m_err != "")
                    {
                        strOutMessage += "\n" + m_err + "\n 库存不足";
                        m_sb = new StringBuilder();
                        m_sb.AppendLine();
                        m_sb.AppendLine(strOutMessage);
                        m_sb.AppendLine();
                        m_sb.AppendLine("  请自行在所关联的领料单中进行调整");

                        FormLargeMessage form = new FormLargeMessage(m_sb.ToString(), Color.Red);
                        form.ShowDialog();
                    }

                    this.Close();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }
        }
    }
}
