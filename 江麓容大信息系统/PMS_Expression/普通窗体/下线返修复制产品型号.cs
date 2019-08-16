using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniversalControlLibrary;
using ServerModule;

namespace Expression
{
    public partial class 下线返修复制产品型号 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 产品信息服务组件
        /// </summary>
        IProductInfoServer m_productInfoServer = ServerModuleFactory.GetServerModule<IProductInfoServer>();

        /// <summary>
        /// 产品信息
        /// </summary>
        IQueryable<View_P_ProductInfo> m_productInfo;

        /// <summary>
        /// 下线防错数据服务类
        /// </summary>
        IOfflineFailSafeServer m_offlineFailServer = ServerModuleFactory.GetServerModule<IOfflineFailSafeServer>();


        public 下线返修复制产品型号()
        {
            InitializeComponent();

            #region 获取所有产品编码(产品类型)信息

            if (!m_productInfoServer.GetAllProductInfo(out m_productInfo, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                btnOK.Enabled = false;
                return;
            }

            if (m_productInfo != null)
            {
                foreach (var item in m_productInfo)
                {
                    cmbSourceProductType.Items.Add(item.产品类型编码);
                    cmbTargetProductType.Items.Add(item.产品类型编码);
                }

                cmbSourceProductType.SelectedIndex = 0;
                cmbTargetProductType.SelectedIndex = 1;
            }
            #endregion            
        }

        private void cmbSourceProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSourceProductName.Text = (from r in m_productInfo
                                         where r.产品类型编码 == cmbSourceProductType.Text
                                         select r.产品类型名称).First();
        }

        private void cmbTargetProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTargetProductName.Text = (from r in m_productInfo
                                         where r.产品类型编码 == cmbTargetProductType.Text
                                         select r.产品类型名称).First();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cmbSourceProductType.SelectedIndex == -1 || cmbTargetProductType.SelectedIndex == -1)
            {
                MessageDialog.ShowErrorMessage("产品类型不允许为空");
                return;
            }
            
            if (cmbSourceProductType.Text == cmbTargetProductType.Text)
            {
                cmbTargetProductType.Focus();
                MessageDialog.ShowErrorMessage("源产品类型不允许和目标产品类型相同");
                return;
            }

            if (cmbPhase.SelectedIndex == -1)
            {
                cmbPhase.Focus();
                MessageDialog.ShowErrorMessage("请选择需要复制的阶段！");
                return;
            }

            if (!m_offlineFailServer.CopyFailsafe(cmbSourceProductType.Text, cmbTargetProductType.Text, cmbPhase.Text, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
            }

            MessageDialog.ShowPromptMessage("复制成功！");
        }

        private void rbtnFinal_CheckedChanged(object sender, EventArgs e)
        {
            lblName.Text = "阶    段";

            DataTable dt = m_offlineFailServer.GetPhase();

            if (dt != null && dt.Rows.Count > 0)
            {
                cmbPhase.Items.Clear();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbPhase.Items.Add(dt.Rows[i]["Phase"].ToString());
                }
            }
        }

        private void rbtnSplit_CheckedChanged(object sender, EventArgs e)
        {
            lblName.Text = "分  总  成";

            cmbPhase.Items.Clear();
            cmbPhase.Items.AddRange(
                    ServerModuleFactory.GetServerModule<IAssemblingBom>().GetParentNames());
        }
    }
}
