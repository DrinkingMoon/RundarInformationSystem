/*************************************************
 * 
 * 文件名称:  可替换工位.cs
 * 作者    :  邱瑶       日期: 2014/05/28
 * 开发平台:  vs2008(c#)
 * 用于    :  装配线管理信息系统
 ************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using UniversalControlLibrary;
using GlobalObject;

namespace Expression
{
    public partial class 可替换工位信息 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 服务组件
        /// </summary>
        IWorkbenchService m_workbenchServer = ServerModuleFactory.GetServerModule<IWorkbenchService>();

        /// <summary>
        ///可替换工位服务组件
        /// </summary>
        IReplaceWorkBenchServer m_replaceServer = PMS_ServerFactory.GetServerModule<IReplaceWorkBenchServer>();

        /// <summary>
        /// 产品信息
        /// </summary>
        List<View_P_ProductInfo> m_productInfo;

        public 可替换工位信息()
        {
            InitializeComponent();

            #region 获取工位
            IQueryable<View_P_Workbench> workbench = m_workbenchServer.Workbenchs;

            if (workbench.Count() > 0)
            {
                cmbWorkBench.Items.AddRange((from r in workbench select r.工位).ToArray());
            }
            else
            {
                btnSave.Visible = false;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;

                MessageDialog.ShowErrorMessage("没有获取到工位信息");
                return;
            }

            #endregion

            RefreshDataGridView();
        }

        private void 可替换工位信息_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public void RefreshDataGridView()
        {
            dataGridView1.DataSource = new BindingCollection<ZPX_ReplaceWorkBench>(m_replaceServer.GetAllData());

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns["ID"].Visible = false;
            }
        }

        private void btnFindProduct_Click(object sender, EventArgs e)
        {
            FormProductType form = new FormProductType();

            if (dataGridView1.Rows.Count > 0)
            {
                List<View_P_ProductInfo> list = new List<View_P_ProductInfo>();

                string[] productType = txtProductType.Text.Split(',');

                foreach (string item in productType)
                {
                    View_P_ProductInfo product = new View_P_ProductInfo();

                    product.产品类型编码 = item;
                    list.Add(product);
                }

                form.SelectedProduct = list;
            }

            string productStr = "";

            if (form.ShowDialog() == DialogResult.OK)
            {
                List<View_P_ProductInfo> productList = form.SelectedProduct;
                m_productInfo = productList;

                if (form.SelectedProduct.Count != form.ProductCount)
                {
                    foreach (View_P_ProductInfo item in productList)
                    {
                        productStr += item.产品类型编码 + ",";
                    }

                    productStr = productStr.Substring(0, productStr.Length - 1);
                }
                else
                {
                    productStr = "全部";
                }
            }

            txtProductType.Text = productStr;
        }

        private void cmbWorkBench_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtReplace.Text = cmbWorkBench.Text;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                cmbWorkBench.Text = dataGridView1.CurrentRow.Cells["主工位"].Value.ToString();
                txtReplace.Text = dataGridView1.CurrentRow.Cells["可替换工位"].Value.ToString();
                txtProductType.Text = dataGridView1.CurrentRow.Cells["适用产品类型"].Value.ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!txtReplace.Text.Substring(0, 3).Equals(cmbWorkBench.Text))
            {
                MessageDialog.ShowPromptMessage("可替换工位的格式应为：" + cmbWorkBench.Text + "X");
                return;
            }

            if (m_productInfo != null)
            {
                bool flag = false;

                foreach (View_P_ProductInfo item in m_productInfo)
                {
                    ZPX_ReplaceWorkBench replace = new ZPX_ReplaceWorkBench();

                    replace.主工位 = cmbWorkBench.Text;
                    replace.适用产品类型 = item.产品类型编码;
                    replace.可替换工位 = txtReplace.Text;
                    replace.记录时间 = ServerTime.Time;
                    replace.记录人员 = BasicInfo.LoginID;

                    if (!m_replaceServer.Insert(replace, out m_error))
                    {
                        flag = true;
                        continue;
                    }
                }

                if (!flag)
                {
                    MessageDialog.ShowPromptMessage("添加成功！");
                }
            }
            else
            {
                ZPX_ReplaceWorkBench replace = new ZPX_ReplaceWorkBench();

                replace.主工位 = cmbWorkBench.Text;
                replace.可替换工位 = txtReplace.Text;
                replace.记录时间 = ServerTime.Time;
                replace.记录人员 = BasicInfo.LoginName;

                if (!m_replaceServer.Insert(replace, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                    return;
                }

                MessageDialog.ShowPromptMessage("添加成功！");
            }

            RefreshDataGridView();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!txtReplace.Text.Substring(0, 3).Equals(cmbWorkBench.Text))
            {
                MessageDialog.ShowPromptMessage("可替换工位的格式应为：" + cmbWorkBench.Text + "X");
                return;
            }

            if (m_productInfo != null)
            {
                bool flag = false;

                foreach (View_P_ProductInfo item in m_productInfo)
                {
                    ZPX_ReplaceWorkBench replace = new ZPX_ReplaceWorkBench();

                    replace.ID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
                    replace.适用产品类型 = item.产品类型名称;
                    replace.可替换工位 = txtReplace.Text;
                    replace.记录时间 = ServerTime.Time;
                    replace.记录人员 = BasicInfo.LoginName;

                    if (!m_replaceServer.Update(replace, out m_error))
                    {
                        flag = true;
                        continue;
                    }
                }

                if (!flag)
                {
                    MessageDialog.ShowPromptMessage("修改成功！");
                }
            }
            else
            {
                ZPX_ReplaceWorkBench replace = new ZPX_ReplaceWorkBench();

                replace.ID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
                replace.可替换工位 = txtReplace.Text;
                replace.记录时间 = ServerTime.Time;
                replace.记录人员 = BasicInfo.LoginName;

                if (!m_replaceServer.Update(replace, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                    return;
                }

                MessageDialog.ShowPromptMessage("修改成功！");
            }

            RefreshDataGridView();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    if (m_replaceServer.Delete(Convert.ToInt32(dataGridView1.Rows[i].Cells["ID"].Value), out m_error))
                    {
                        continue;
                    }
                }

                MessageDialog.ShowPromptMessage("删除成功！");
                RefreshDataGridView();
            }
        }
    }
}
