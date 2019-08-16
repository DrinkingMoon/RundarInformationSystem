using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using ServerModule;
using System.Windows.Forms;

namespace UniversalControlLibrary
{
    public partial class FormDateTime : Form
    {
        /// <summary>
        /// 选择的时间
        /// </summary>
        public DateTime SelectedTime
        {
            get;
            set;
        }

        public event GlobalObject.DelegateCollection.FormInit DateTimeFormInit;

        /// <summary>
        /// 标题文本
        /// </summary>
        /// <param name="titleText"></param>
        public FormDateTime(string titleText)
        {
            InitializeComponent();

            this.Text = titleText;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SelectedTime = dateTimePicker1.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FormDateTime_Load(object sender, EventArgs e)
        {
            if (DateTimeFormInit != null)
            {
                DateTimeFormInit(this);
            }
        }
    }
}
