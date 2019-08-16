using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GlobalObject;

namespace UniversalControlLibrary
{
    public partial class FormDataComboBox : Form
    {
        private string m_Result;

        public string Result
        {
            get { return m_Result; }
            set { m_Result = value; }
        }

        private CE_DataGridViewItemsType m_ExportType;

        public CE_DataGridViewItemsType ExportType
        {
            get { return m_ExportType; }
            set { m_ExportType = value; }
        }

        public FormDataComboBox(List<string> dataSource, string headText)
        {
            InitializeComponent();

            this.Text = headText;
            comboBox1.DataSource = dataSource;
            comboBox1.SelectedIndex = -1;

            if (headText == "需要导出的数据集")
            {
                rbAll.Visible = true;
                rbSelect.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_Result = comboBox1.Text;
            m_ExportType = rbAll.Checked ? CE_DataGridViewItemsType.全部项 : CE_DataGridViewItemsType.选中项;

            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
