using System;
using System.Collections.Generic;
/******************************************************************************
 *
 * 文件名称:  下线返修扭矩防错设置.cs
 * 作者    :  邱瑶       日期: 2014/07/7
 * 开发平台:  vs2008(c#)
 * 用于    :  装配线管理信息系统
 ******************************************************************************/
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniversalControlLibrary;
using ServerModule;
using GlobalObject;

namespace Expression
{
    public partial class 设置阶段 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 下线防错数据服务类
        /// </summary>
        IOfflineFailSafeServer m_offlineFailServer = ServerModuleFactory.GetServerModule<IOfflineFailSafeServer>();

        public 设置阶段()
        {
            InitializeComponent();

            dataGridView1.DataSource = new BindingCollection<ZPX_OfflinePhaseSet>(m_offlineFailServer.GetAllPhase());
        }

        private void btnFindCode_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = QueryInfoDialog.GetAllGoodsInfo();

            if (form != null && form.ShowDialog() == DialogResult.OK)
            {
                //txtGoodsCode.Text = form.GetDataItem("图号型号").ToString();
                //txtGoodsName.Text = form.GetDataItem("物品名称").ToString();
                //txtSpec.Text = form.GetDataItem("规格").ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ZPX_OfflinePhaseSet phase = new ZPX_OfflinePhaseSet();

            phase.Phase = txtPhase.Text;
            phase.Contain = txtContain.Text;

            if (!m_offlineFailServer.InsertPhase(phase, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }

            dataGridView1.DataSource = new BindingCollection<ZPX_OfflinePhaseSet>(m_offlineFailServer.GetAllPhase());
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (MessageDialog.ShowEnquiryMessage("确认删除选中信息吗？") == DialogResult.Yes)
                {
                    if (!m_offlineFailServer.DeletePhase(dataGridView1.CurrentRow.Cells["阶段"].Value.ToString(), out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }
                }
            }

            dataGridView1.DataSource = new BindingCollection<ZPX_OfflinePhaseSet>(m_offlineFailServer.GetAllPhase());
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtPhase.Text = dataGridView1.CurrentRow.Cells["阶段"].Value.ToString();
            txtContain.Text = dataGridView1.CurrentRow.Cells["包含阶段"].Value.ToString();
        }
    }
}
