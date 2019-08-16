using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Service_Project_Project;

namespace Form_Project_Project
{
    public partial class 项目工时表填写人员设置 : Form
    {
        ITimesheets _ServiceTime = ServerModuleFactory.GetServerModule<ITimesheets>();

        public 项目工时表填写人员设置()
        {
            InitializeComponent();
        }

        private void 项目工时表填写人员设置_Load(object sender, EventArgs e)
        {
            dataGridView.DataSource = _ServiceTime.GetSetPersonnelInfo();
        }

        private void txtName_OnCompleteSearch()
        {
            txtWorkID.Text = txtName.DataResult["工号"].ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _ServiceTime.AddPersonnel(txtWorkID.Text);
            MessageBox.Show("添加成功");
            dataGridView.DataSource = _ServiceTime.GetSetPersonnelInfo();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.CurrentRow == null)
            {
                return;
            }

            _ServiceTime.DeletePeronnnel(
                dataGridView.CurrentRow.Cells["工号"].Value.ToString());
            MessageBox.Show("删除成功");
            dataGridView.DataSource = _ServiceTime.GetSetPersonnelInfo();
        }
    }
}
