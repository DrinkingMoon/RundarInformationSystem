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
using WebServerModule2;
using UniversalControlLibrary;


namespace Expression
{
    /// <summary>
    /// 量检具台帐界面
    /// </summary>
    public partial class 量检具台帐 : Form
    {
        /// <summary>
        /// 数据集
        /// </summary>
        DataTable m_dtDateSource = new DataTable();

        /// <summary>
        /// 查找条件字段列表
        /// </summary>
        List<string> m_lstFindField = new List<string>();

        /// <summary>
        /// 量检具服务组件
        /// </summary>
        IGaugeManage _Servivc_Gauge = ServerModuleFactory.GetServerModule<IGaugeManage>();

        /// <summary>
        /// 权限组件
        /// </summary>
        AuthorityFlag m_authFlag;

        public 量检具台帐(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();
            m_authFlag = nodeInfo.Authority;
            RefreshDataGridView();
        }

        private void 量检具台帐_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="dataSource">数据集</param>
        void RefreshDataGridView()
        {
            dataGridView1.DataSource = _Servivc_Gauge.GetGaugeAllInfo(chbYLY.Checked, chbYBF.Checked);
            userControlDataLocalizer1.Init(this.dataGridView1, this.Name, null);

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((string)dataGridView1.Rows[i].Cells["量检具编号"].Value == txtGaugeCoding.Text)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[1];
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1_CellClick(null, null);
                }
            }
        }

        S_GaugeStandingBook GetMessage()
        {
            if (txtCode.Tag == null)
            {
                throw new Exception("请选择【物品】");
            }

            if (GeneralFunction.IsNullOrEmpty(txtGaugeCoding.Text))
            {
                throw new Exception("请填写【量检具编号】");
            }

            if (GeneralFunction.IsNullOrEmpty(txtManufacturer.Text))
            {
                throw new Exception("请填写【制造商】");
            }

            if (dtpInputDate.Value.Date > ServerTime.Time.Date)
            {
                throw new Exception("【入库日期】不能大于当前时间");
            }

            if (dtpMaterialDate.Value.Date > ServerTime.Time.Date)
            {
                throw new Exception("【领料日期】不能大于当前时间");
            }

            if (GeneralFunction.IsNullOrEmpty(cmbGaugeType.Text))
            {
                throw new Exception("请选择【量检具类别】");
            }

            if (cmbGaugeType.Text != "C类" && GeneralFunction.IsNullOrEmpty(cmbCheckType.Text))
            {
                throw new Exception("请选择【校准类别】");
            }

            S_GaugeStandingBook book = new S_GaugeStandingBook();

            book.F_Id = txtName.Tag == null ? Guid.NewGuid().ToString() : txtName.Tag.ToString();
            book.DutyUser = txtDutyUser.Tag.ToString();
            book.GaugeCoding = txtGaugeCoding.Text;
            book.GoodsID = Convert.ToInt32(txtCode.Tag);
            book.InputDate = dtpInputDate.Value;
            book.Manufacturer = txtManufacturer.Text;
            book.MaterialDate = chbZK.Checked ? null : (DateTime?)dtpMaterialDate.Value;
            book.Remark = txtRemark.Text;
            book.Validity = (int)numValidity.Value;
            book.ScrapFlag = chbBF.Checked;
            book.FactoryNo = txtFactoryNo.Text;
            book.GaugeType = cmbGaugeType.Text;
            book.CheckType = cmbCheckType.Text;

            return book;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            txtName.Focus();

            try
            {
                if (txtName.Tag == null || txtName.Tag.ToString() == "")
                {
                    throw new Exception("请选择需要修改的记录");
                }

                _Servivc_Gauge.SaveInfo(GetMessage(), CE_OperatorMode.修改);
                throw new Exception("修改成功");
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
            finally
            {
                RefreshDataGridView();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtName.Focus();
            try
            {
                _Servivc_Gauge.SaveInfo(GetMessage(), CE_OperatorMode.添加);
                throw new Exception("添加成功");
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
            finally
            {
                RefreshDataGridView();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null)
                {
                    return;
                }

                _Servivc_Gauge.DeleteInfo(dataGridView1.CurrentRow.Cells["量检具编号"].Value.ToString());
                throw new Exception("删除成功");
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
            finally
            {
                RefreshDataGridView();
            }
        }

        private void btnRelFiles_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            try
            {
                if (_Servivc_Gauge.GetSingleInfo(dataGridView1.CurrentRow.Cells["量检具编号"].Value.ToString()) == null)
                {
                    throw new Exception("请先添加【量检具台账】,再上传相关文件");
                }

                量检具关联文件 frm = new 量检具关联文件(dataGridView1.CurrentRow.Cells["量检具编号"].Value.ToString());
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
            finally
            {
                RefreshDataGridView();
            }
        }

        private void txtCode_OnCompleteSearch()
        {
            if (txtCode.DataResult == null)
            {
                return;
            }

            txtCode.Text = txtCode.DataResult["图号型号"].ToString();
            txtName.Text = txtCode.DataResult["物品名称"].ToString();
            txtSpce.Text = txtCode.DataResult["规格"].ToString();

            txtCode.Tag = txtCode.DataResult["序号"];
        }

        private void txtDutyUser_OnCompleteSearch()
        {
            if (txtDutyUser.DataResult == null)
            {
                return;
            }

            txtDutyUser.Text = txtDutyUser.DataResult["姓名"].ToString();
            txtDutyUser.Tag = txtDutyUser.DataResult["工号"].ToString();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            txtName.Tag = dataGridView1.CurrentRow.Cells["F_Id"].Value.ToString();

            txtName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
            txtSpce.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
            txtCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
            txtCode.Tag = Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value);

            txtGaugeCoding.Text = dataGridView1.CurrentRow.Cells["量检具编号"].Value.ToString();
            txtManufacturer.Text = dataGridView1.CurrentRow.Cells["制造商"].Value.ToString();

            object workID = dataGridView1.CurrentRow.Cells["DutyUser"].Value;

            if (workID != null)
            {
                View_HR_Personnel personnel = UniversalFunction.GetPersonnelInfo(workID.ToString());

                txtDutyUser.Text = personnel.姓名;
                txtDutyUser.Tag = personnel.工号;
                txtDept.Text = personnel.部门名称;
            }
            else
            {
                txtDutyUser.Text = "";
                txtDutyUser.Tag = null;
                txtDept.Text = null;
            }

            txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            numValidity.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["校准周期"].Value.ToString());

            dtpMaterialDate.Value = GeneralFunction.IsNullOrEmpty(dataGridView1.CurrentRow.Cells["领用日期"].Value.ToString()) ?
                ServerTime.Time : Convert.ToDateTime(dataGridView1.CurrentRow.Cells["领用日期"].Value);
            txtEffectiveDate.Text = dataGridView1.CurrentRow.Cells["有效日期"].Value.ToString();

            dtpInputDate.Value = dataGridView1.CurrentRow.Cells["入库时间"].Value == DBNull.Value ?
                ServerTime.Time : Convert.ToDateTime(dataGridView1.CurrentRow.Cells["入库时间"].Value);

            chbZK.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["在库"].Value);

            chbBF.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["报废"].Value);

            cmbGaugeType.Text = dataGridView1.CurrentRow.Cells["量检具类别"].Value.ToString();
            txtFactoryNo.Text = dataGridView1.CurrentRow.Cells["出厂编号"].Value.ToString();
            cmbCheckType.Text = dataGridView1.CurrentRow.Cells["校准类别"].Value.ToString();
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                if (dgvr.Cells["有效日期"].Value == null 
                    || GeneralFunction.IsNullOrEmpty(dgvr.Cells["有效日期"].Value.ToString()))
                {
                    continue;
                }

                DateTime tempTime = Convert.ToDateTime(dgvr.Cells["有效日期"].Value);

                if (tempTime.Date.AddDays(-15) <= ServerTime.Time.Date)
                {
                    dgvr.DefaultCellStyle.BackColor = Color.Yellow;
                }

                if (tempTime.Date < ServerTime.Time.Date)
                {
                    dgvr.DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        private void chbZK_CheckedChanged(object sender, EventArgs e)
        {
            if (chbZK.Checked)
            {
                dtpMaterialDate.Visible = false;
                label13.Visible = false;
            }
            else
            {
                dtpMaterialDate.Visible = true;
                label13.Visible = true;
            }
        }

        private void cmbGaugeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGaugeType.Text != "C类")
            {
                cmbCheckType.Enabled = true;
            }
            else
            {
                cmbCheckType.SelectedIndex = -1;
                cmbCheckType.Enabled = false;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnRelFiles_Click(null, null);
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            string strSql = "select *,  b.员工姓名 AS 责任人, b.部门 AS 责任部门 from View_S_GaugeStandingBook as a "+
                " left join dbo.View_HR_PersonnelArchive AS b ON a.DutyUser = b.员工编号";
            ExcelHelperP.DataTableToExcel(saveFileDialog1, GlobalObject.DatabaseServer.QueryInfo(strSql), null);
        }
    }
}
