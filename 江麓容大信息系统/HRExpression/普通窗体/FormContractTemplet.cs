using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Expression;
using Service_Peripheral_HR;
using GlobalObject;
using ServerModule;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 获取合同模板界面
    /// </summary>
    public partial class FormContractTemplet : Form
    {
        /// <summary>
        /// 合同管理服务类
        /// </summary>
        ILaborContractServer m_laborServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ILaborContractServer>();

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 合同模板编号
        /// </summary>
        private int templetID;

        public int TempletID
        {
            get { return templetID; }
            set { templetID = value; }
        }

        /// <summary>
        /// 合同类别
        /// </summary>
        private string templetType;

        public string TempletType
        {
            get { return templetType; }
            set { templetType = value; }
        }

        public FormContractTemplet()
        {
            InitializeComponent();

            RefreshControl();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshControl()
        {
            dataGridView1.DataSource = m_laborServer.GetLaborContractTemplet();

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns["编号"].Visible = false;
                dataGridView1.Columns["合同附件"].Visible = false;
            }

            dataGridView1.Refresh();

            // 添加查询用的列
            if (m_findField == null)
            {
                List<string> lstColumnName = new List<string>();

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (dataGridView1.Columns[i].Visible)
                    {
                        lstColumnName.Add(dataGridView1.Columns[i].Name);
                    }
                }

                m_findField = lstColumnName.ToArray();
            }

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            templetID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["编号"].Value);

            templetType = dataGridView1.CurrentRow.Cells["合同类别"].Value.ToString();

            this.Close();
        }

        private void FormContractTemplet_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }
    }
}
