using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using Expression;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 强制试验记录 : Form
    {
        /// <summary>
        /// 下线试验结果信息服务
        /// </summary>
        IOffLineTest m_testServer = PMS_ServerFactory.GetServerModule<IOffLineTest>();

        public 强制试验记录()
        {
            InitializeComponent();

            RefreshData();

            userControlDataLocalizer1.Init(dataGridView1, this.Name, 
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, GlobalObject.BasicInfo.LoginID));
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshData()
        {
            dataGridView1.DataSource = m_testServer.GetForcedOffLineTestInfo();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtProductNumber.Text = "";
            txtReason.Text = "";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtProductNumber.Text) || GlobalObject.GeneralFunction.IsNullOrEmpty(txtReason.Text))
            {
                MessageDialog.ShowPromptMessage("请录入产品编号及原因后再进行此操作");
                return;
            }

            string error;
            string[] info = txtProductNumber.Text.Split(new char[] { ' ' });

            if (!SCM_Level02_ServerFactory.GetServerModule<IProductCodeServer>().VerifyProductCodesInfo(
                info[0], info[1], GlobalObject.CE_BarCodeType.内部钢印码, out error))
            {
                MessageDialog.ShowErrorMessage(error);

                txtProductNumber.Focus();

                return;
            }

            ZPX_ForcedOffLineTest data = new ZPX_ForcedOffLineTest();

            data.ProductNumber = txtProductNumber.Text;
            data.Reason = txtReason.Text;
            data.Recorder = GlobalObject.BasicInfo.LoginID;

            try
            {
                m_testServer.AddForcedOffLineTestInfo(data);

                RefreshData();

                MessageDialog.ShowPromptMessage("操作成功！");
            }
            catch (Exception exce)
            {
                MessageDialog.ShowErrorMessage(exce.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择要操作的数据行后再进行此操作");
                return;
            }

            try
            {
                m_testServer.DeleteForcedOffLineTestInfo((int)dataGridView1.SelectedRows[0].Cells["序号"].Value);

                RefreshData();

                MessageDialog.ShowPromptMessage("操作成功！");
            }
            catch (Exception exce)
            {
                MessageDialog.ShowErrorMessage(exce.Message);
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1 && dataGridView1.SelectedRows.Count > 0)
            {
                txtProductNumber.Text = dataGridView1.SelectedRows[0].Cells["产品编号"].Value.ToString();
                txtReason.Text = dataGridView1.SelectedRows[0].Cells["原因"].Value.ToString();
            }
        }
    }
}
