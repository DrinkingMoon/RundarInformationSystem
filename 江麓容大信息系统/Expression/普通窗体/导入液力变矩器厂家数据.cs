using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 导入液力变矩器厂家数据界面
    /// </summary>
    public partial class 导入液力变矩器厂家数据 : Form
    {
        /// <summary>
        /// 导入数据是否完成的标志
        /// </summary>
        public bool Complete
        {
            get;
            set;
        }

        /// <summary>
        /// 导入的数据集
        /// </summary>
        public DataTable Data
        {
            get;
            set;
        }

        public 导入液力变矩器厂家数据()
        {
            InitializeComponent();
        }

        private void btnGetFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFileName.Text = openFileDialog1.FileName;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (txtFileName.Text.Trim().Length > 0)
            {
                if (System.IO.File.Exists(txtFileName.Text))
                {
                    openFileDialog1.FileName = txtFileName.Text;
                }
                else
                {
                    MessageDialog.ShowPromptMessage("不存在的数据文件：" + txtFileName.Text);
                    return;
                }
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(openFileDialog1.FileName))
            {
                MessageDialog.ShowPromptMessage("请选择要导入的文件后再进行此操作！");
                return;
            }

            try
            {
                Data = ExcelHelperP.RenderFromExcel(openFileDialog1.OpenFile());

                if (Data != null || Data.Rows.Count == 0 ||
                    Data.Columns[0].ColumnName != "液力变矩器型号" || Data.Columns[4].ColumnName != "变矩器出厂编号")
                {
                    MessageDialog.ShowPromptMessage(string.Format("{0} 中没有包含所需的信息，无法导入！", openFileDialog1.FileName));
                    return;
                }

                Complete = true;

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception exce)
            {
                MessageDialog.ShowErrorMessage(exce.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
