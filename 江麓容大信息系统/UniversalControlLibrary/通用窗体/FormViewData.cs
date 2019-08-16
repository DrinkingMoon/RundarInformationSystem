using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;

namespace UniversalControlLibrary
{
    public partial class FormViewData : Form
    {
        public FormViewData(DataGridViewColumnCollection columns, DataGridViewRow row)
        {
            InitializeComponent();

            int visibleColumnAmount = 0;

            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].Visible)
                {
                    visibleColumnAmount++;
                }
            }

            visibleColumnAmount += visibleColumnAmount % 2;

            dataGridView1.Rows.Clear();
            for (int i = 0; i < visibleColumnAmount / 2; i++)
            {
                DataGridViewRow newRow = new DataGridViewRow();
                dataGridView1.Rows.Add(new object[] {"", "", "", "" });
            }

            int rowIndex = 0;
            int colIndex = 0;

            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].Visible)
                {
                    dataGridView1.Rows[rowIndex++ / 4].Cells[colIndex++].Value = columns[i].Name;
                    dataGridView1.Rows[rowIndex++ / 4].Cells[colIndex++].Value = row.Cells[i].Value;                    
                    colIndex = colIndex % 4;
                }
            }

            dataGridView1.Columns[0].DefaultCellStyle.ForeColor = Color.Green;
            dataGridView1.Columns[1].Width = 160;
            dataGridView1.Columns[2].DefaultCellStyle.ForeColor = Color.Green;
            dataGridView1.Columns[3].Width = 160;

            this.Width = dataGridView1.Columns[0].Width * 2 + dataGridView1.Columns[1].Width * 2 + 30;
            this.Height = (dataGridView1.Rows.Count + 2) * dataGridView1.Rows[0].Height + 20;
        }

        public FormViewData(IQueryable<View_HR_PersonnelArchive> query)
        {
            InitializeComponent();

            dataGridView1.Columns.Clear();

            dataGridView1.DataSource = query;

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns["外部职称"].Visible = false;
                dataGridView1.Columns["内部级别"].Visible = false;
                dataGridView1.Columns["离职时间"].Visible = false;
                dataGridView1.Columns["参加工作的时间"].Visible = false;
                //dataGridView1.Columns["是否核心员工"].Visible = false;
                //dataGridView1.Columns["类别"].Visible = false;
                //dataGridView1.Columns["合同类型"].Visible = false;
                //dataGridView1.Columns["合同起始时间"].Visible = false;
                dataGridView1.Columns["合同到期日"].Visible = false;
                dataGridView1.Columns["出生日期"].Visible = false;
                dataGridView1.Columns["国籍"].Visible = false;
                dataGridView1.Columns["民族"].Visible = false;
                dataGridView1.Columns["籍贯"].Visible = false;
                dataGridView1.Columns["政治面貌"].Visible = false;
                dataGridView1.Columns["婚姻状况"].Visible = false;
                dataGridView1.Columns["身份证"].Visible = false;
                dataGridView1.Columns["毕业院校"].Visible = false;
                dataGridView1.Columns["文化程度"].Visible = false;
                dataGridView1.Columns["专业"].Visible = false;
                dataGridView1.Columns["学制"].Visible = false;
                dataGridView1.Columns["毕业年份"].Visible = false;
                dataGridView1.Columns["家庭地址"].Visible = false;
                //dataGridView1.Columns["邮编"].Visible = false;
                dataGridView1.Columns["电话"].Visible = false;
                dataGridView1.Columns["手机"].Visible = false;
                //dataGridView1.Columns["开户银行"].Visible = false;
                //dataGridView1.Columns["银行账号"].Visible = false;
                //dataGridView1.Columns["社会保障号"].Visible = false;
                dataGridView1.Columns["QQ号码"].Visible = false;
                dataGridView1.Columns["电子邮箱"].Visible = false;
                dataGridView1.Columns["爱好"].Visible = false;
                dataGridView1.Columns["特长"].Visible = false;
                dataGridView1.Columns["性别"].Visible = false;
                //dataGridView1.Columns["工作经历"].Visible = false;
                //dataGridView1.Columns["教育培训经历"].Visible = false;
                //dataGridView1.Columns["家庭成员"].Visible = false;
                dataGridView1.Columns["关联编号"].Visible = false;
                //dataGridView1.Columns["个人档案所在地"].Visible = false;
                //dataGridView1.Columns["照片"].Visible = false;
                //dataGridView1.Columns["附件"].Visible = false;
                //dataGridView1.Columns["附件名"].Visible = false;
                //dataGridView1.Columns["调动次数"].Visible = false;
                //dataGridView1.Columns["培训次数"].Visible = false;
                dataGridView1.Columns["备注"].Visible = false;
                dataGridView1.Columns["记录人员"].Visible = false;
                dataGridView1.Columns["记录时间"].Visible = false;
                //dataGridView1.Columns["变更次数"].Visible = false;
                //dataGridView1.Columns["拼音"].Visible = false;
                //dataGridView1.Columns["五笔"].Visible = false;
                //dataGridView1.Columns["部门编号"].Visible = false;
                dataGridView1.Columns["人员状态"].Visible = false;

                this.Height = (dataGridView1.Rows.Count + 2) * dataGridView1.Rows[0].Height + 30;
                dataGridView1.Columns[0].Width = 60;
                dataGridView1.Columns[1].Width = 80;
                dataGridView1.Columns[2].Width = 80;
            }
        }
    }
}
