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
    public partial class BOM表零件库 : Form
    {
        /// <summary>
        /// BOM服务组件
        /// </summary>
        IBomServer m_serverBom = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// LINQ 数据集
        /// </summary>
        BASE_BomPartsLibrary m_lnqLibrary = new BASE_BomPartsLibrary();

        public BOM表零件库(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            AuthorityControl(nodeInfo.Authority);
            RefreshData();
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        void ClearData()
        {
            txtGoodsCode.Text = "";
            txtGoodsName.Text = "";
            txtMaterial.Text = "";
            txtPivotalPart.Text = "";
            txtRemark.Text = "";
            txtSpec.Text = "";
            txtVersion.Text = "";
            txtGoodsName.Tag = -1;
            cmbPartType.SelectedIndex = -1;
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="msg">定位信息</param>
        void PositioningRecord(string msg)
        {
            string strColName = "";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((string)dataGridView1.Rows[i].Cells["物品ID"].Value.ToString() == msg)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefreshData()
        {
            dataGridView1.DataSource = m_serverBom.GetBOMPartsLibrary();

            userControlDataLocalizer1.Init(this.dataGridView1,this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                ClearData();

                txtGoodsCode.Text = dataGridView1.CurrentRow.Cells["零件图号"].Value.ToString();
                txtGoodsName.Text = dataGridView1.CurrentRow.Cells["零件名称"].Value.ToString();
                txtGoodsName.Tag = Convert.ToInt32( dataGridView1.CurrentRow.Cells["物品ID"].Value);
                txtSpec.Text = dataGridView1.CurrentRow.Cells["零件规格"].Value.ToString();
                txtVersion.Text = dataGridView1.CurrentRow.Cells["版次号"].Value.ToString();
                txtMaterial.Text = dataGridView1.CurrentRow.Cells["材质"].Value.ToString();
                txtPivotalPart.Text = dataGridView1.CurrentRow.Cells["关键件"].Value.ToString();
                txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
                cmbPartType.Text = dataGridView1.CurrentRow.Cells["零件类型"].Value.ToString();
                
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string error = "";

            m_lnqLibrary.GoodsID = Convert.ToInt32(txtGoodsName.Tag);
            m_lnqLibrary.Version = txtVersion.Text;
            m_lnqLibrary.Remark = txtRemark.Text;
            m_lnqLibrary.PivotalPart = txtPivotalPart.Text;
            m_lnqLibrary.PartType = cmbPartType.Text;
            m_lnqLibrary.Material = txtMaterial.Text;
            m_lnqLibrary.CreatePersonnel = BasicInfo.LoginID;

            if (!m_serverBom.UpdateBOMPartsLibrary(m_lnqLibrary,out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("更新成功");
            }

            RefreshData();
            PositioningRecord(m_lnqLibrary.GoodsID.ToString());
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
