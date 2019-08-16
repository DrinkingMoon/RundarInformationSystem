using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;


namespace Expression
{
    /// <summary>
    /// 自动生成入库单界面
    /// </summary>
    public partial class 自动生成入库单 : Form
    {
        /// <summary>
        /// BOM表信息服务组件
        /// </summary>
        IBomServer m_serverBom = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 自动生成入库单管理服务
        /// </summary>
        IGeneratesCheckOutInDepotServer m_serverGenerates = ServerModuleFactory.GetServerModule<IGeneratesCheckOutInDepotServer>();

        public 自动生成入库单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            DataTable dt = UniversalFunction.GetStorageTb();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbStorage.Items.Add(dt.Rows[i]["StorageName"].ToString());
            }

            cmbStorage.SelectedIndex = -1;
        }

        private void 自动生成入库单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="source">数据集</param>
        void RefreshDataGirdView(DataTable source)
        {
            dataGridView1.DataSource = source;

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name,
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

                m_dataLocalizer.OnlyLocalize = true;
                panelPara.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);
        }

        void txtProvider_OnCompleteSearch()
        {
            txtProvider.Text = txtProvider.DataResult["供应商名称"].ToString();
            txtProvider.Tag = txtProvider.DataResult["供应商编码"].ToString();
        }

        void txtName_OnCompleteSearch()
        {
            txtName.Text = txtName.DataResult["物品名称"].ToString();
            txtCode.Text = txtName.DataResult["图号型号"].ToString();
            txtSpec.Text = txtName.DataResult["规格"].ToString();

            DataRow dr = m_serverBom.GetBomInfo(txtCode.Text.Trim(), txtName.Text.Trim());

            if (dr == null)
            {
                txtVersion.Text = "";
            }
            else
            {
                txtVersion.Text = dr["Version"].ToString();
            }
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <returns>通过返回True，否则False</returns>
        bool CheckDate()
        {
            if (txtProvider.Tag == null || txtProvider.Tag.ToString().Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择供应商");
                return false;
            }

            if (txtCode.Text.Trim() == "" && txtName.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择物品");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 查询控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFind_Click(object sender, EventArgs e)
        {
            if (CheckDate())
            {

                DataTable dt = m_serverGenerates.GetAllInfo(txtProvider.Tag.ToString(), txtCode.Text,
                    txtName.Text, txtSpec.Text, out m_err);

                if (dt == null)
                {
                    MessageDialog.ShowPromptMessage(m_err);
                }
                else
                {
                    RefreshDataGirdView(dt);
                }
            }
        }

        private void btnGenerates_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            DataTable dt = (DataTable)dataGridView1.DataSource;

            DataTable dtDate = dt.Clone();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if ((bool)dt.Rows[i]["选"])
                {
                    dtDate.ImportRow(dt.Rows[i]);
                }
            }

            for (int i = 0; i < dtDate.Rows.Count; i++)
            {
                decimal dtNowCount = Convert.ToDecimal(dtDate.Rows[i]["已到货数"]) +
                    Convert.ToDecimal(dtDate.Rows[i]["到货数量"]);
                decimal dtOrderCount = Convert.ToDecimal(dtDate.Rows[i]["订货数量"]) * (decimal)1.1;

                if (dtOrderCount < dtNowCount)
                {
                    MessageDialog.ShowPromptMessage("物品名称为【" + dtDate.Rows[i]["物品名称"].ToString()
                        + "】，图号型号为【" +
                        dtDate.Rows[i]["图号型号"].ToString()
                        + "】的物品, 到货数量超出允许数量，请重新调整!");
                    return;
                }
            }

            if (cmbStorage.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择所属库房");
                return;
            }

            if (txtVersion.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写版次号");
                return;
            }

            string strStorageID = UniversalFunction.GetStorageID(cmbStorage.Text);


            if (!m_serverGenerates.AddCheckInDepotBill(dtDate, strStorageID, txtVersion.Text, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
            }
            else
            {
                MessageDialog.ShowPromptMessage("自动生成成功");
            }

            RefreshDataGirdView(m_serverGenerates.GetAllInfo(txtProvider.Tag.ToString(), txtCode.Text,
                    txtName.Text, txtSpec.Text, out m_err));
        }

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            if (dataGridView1.Rows.Count > 0 && e.ColumnIndex == 0)
            {
                if (!(bool)dataGridView1.Rows[e.RowIndex].Cells["选"].FormattedValue)
                {
                    dataGridView1.Rows[e.RowIndex].Cells["到货数量"].Value = Convert.ToDecimal(
                        dataGridView1.Rows[e.RowIndex].Cells["订货数量"].Value) -
                        Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells["已到货数"].Value);
                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].Cells["到货数量"].Value = 0;
                }

                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Formatting);
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            if (dataGridView1.Rows.Count > 0 && e.ColumnIndex == 8)
            {
                if (Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells["到货数量"].FormattedValue) > 0)
                {
                    dataGridView1.Rows[e.RowIndex].Cells["选"].Value = true;
                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].Cells["选"].Value = false;
                }

                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Formatting);
            }
        }
    }
}
