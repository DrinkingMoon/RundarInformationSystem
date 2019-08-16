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
using Expression;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 一次性物料清单 : Form
    {
        /// <summary>
        /// 产品信息管理服务组件
        /// </summary>
        IBomServer m_bomService = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 一次性物料清单
        /// </summary>
        IDisposableGoodsServer m_disposeGoodsServer = ServerModuleFactory.GetServerModule<IDisposableGoodsServer>();

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        public 一次性物料清单(FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;
            cmbProductType.DataSource = m_bomService.GetAssemblyTypeList();

            RefreshGridView();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshGridView()
        {
            IQueryable<View_ZPX_DisposableGoods> disposableGoods = null;

            if (!m_disposeGoodsServer.GetAllDataInfo(out disposableGoods, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
            }
            else 
            {
                dataGridView1.DataSource = disposableGoods;
            }

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        private void 一次性物料清单_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authFlag);
        }

        private void 一次性物料清单_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void txtGoodsName_OnCompleteSearch()
        {
            txtGoodsCode.Text = txtGoodsName.DataResult["图号型号"].ToString();
            txtGoodsCode.Tag = txtGoodsName.DataResult["序号"].ToString();
            txtGoodsName.Text = txtGoodsName.DataResult["物品名称"].ToString();
            txtSpec.Text = txtGoodsName.DataResult["规格"].ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbProductType.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择产品型号！");
                return;
            }

            if (txtGoodsCode.Text.Trim() == "" && txtGoodsName.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择物品名称！");
                return;
            }

            if (numCount.Value == 0)
            {
                MessageDialog.ShowPromptMessage("请填写数量！");
                return;
            }

            ZPX_DisposableGoods dis = new ZPX_DisposableGoods();

            dis.Count = Convert.ToInt32(numCount.Value);
            dis.Date = ServerTime.Time;
            dis.GoodsCode = txtGoodsCode.Text;
            dis.GoodsName = txtGoodsName.Text;
            dis.ProductType = cmbProductType.Text;
            dis.Spec = txtSpec.Text;
            dis.UserCode = BasicInfo.LoginID;

            if (!m_disposeGoodsServer.InsertData(dis, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
            }
            else
            {
                MessageDialog.ShowPromptMessage("添加成功！");
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            FormCopyProduct frm = new FormCopyProduct();

            frm.ShowDialog();

            RefreshGridView();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (MessageDialog.ShowEnquiryMessage("您确定删除选中的数据吗？") == DialogResult.Yes)
                {
                    string goodsInfo = "";

                    for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                    {
                        ZPX_DisposableGoods dis = new ZPX_DisposableGoods();

                        dis.GoodsCode = dataGridView1.SelectedRows[i].Cells["图号型号"].Value.ToString();
                        dis.GoodsName = dataGridView1.SelectedRows[i].Cells["物品名称"].Value.ToString();
                        dis.ProductType = dataGridView1.SelectedRows[i].Cells["产品型号"].Value.ToString();
                        dis.Spec = dataGridView1.SelectedRows[i].Cells["规格"].Value.ToString();

                        if (!m_disposeGoodsServer.DeleteData(dis,out m_error))
                        {
                            goodsInfo += "产品型号为" + dataGridView1.SelectedRows[i].Cells["产品型号"].Value.ToString() + "；物品名称：" + dataGridView1.SelectedRows[i].Cells["物品名称"].Value.ToString() + "；图号型号：" + dataGridView1.SelectedRows[i].Cells["图号型号"].Value.ToString() + "；规格：" + dataGridView1.SelectedRows[i].Cells["规格"].Value.ToString() + "\r\n";
                        }
                    }

                    if (goodsInfo == "")
                    {
                        MessageDialog.ShowPromptMessage("删除成功！");
                        RefreshGridView();
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("删除失败！\r\n" + goodsInfo);
                    }
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的数据行！");
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            cmbProductType.Text = dataGridView1.CurrentRow.Cells["产品型号"].Value.ToString();
            txtGoodsCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
            txtGoodsName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
            txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
            numCount.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["数量"].Value);
            cbNonzeroKilometer.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否0公里"].Value.ToString() == "是" ? true : false);
            txtUserCode.Text = dataGridView1.CurrentRow.Cells["录入人员"].Value.ToString();
            txtDate.Text = dataGridView1.CurrentRow.Cells["录入日期"].Value.ToString();
        }

        private void 刷新toolStripButton_Click(object sender, EventArgs e)
        {
            RefreshGridView();
        }
    }
}
