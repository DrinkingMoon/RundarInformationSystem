using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GlobalObject;
using ServerModule;
using FlowControlService;

namespace UniversalControlLibrary
{
    public partial class FormValueSet : Form
    {
        List<BASE_IntegrationReportList> _Result = new List<BASE_IntegrationReportList>();

        public List<BASE_IntegrationReportList> Result
        {
            get { return _Result; }
            set { _Result = value; }
        }

        public FormValueSet()
        {
            InitializeComponent();
        }

        public FormValueSet(List<BASE_IntegrationReportList> lstCondition)
        {
            InitializeComponent();
            _Result = lstCondition;

            if (lstCondition == null)
            {
                return;
            }

            Point ptFieldName = new Point(25, 67);
            Point ptFieldFormat = new Point(145, 67);
            Point ptValue = new Point(267, 67);
            Size sz = new Size(200, 21);

            foreach (BASE_IntegrationReportList item in lstCondition)
            {
                Label lbFieldName = new Label();
                lbFieldName.AutoSize = true;
                lbFieldName.Location = ptFieldName;
                lbFieldName.Text = item.FieldName;

                panel1.Controls.Add(lbFieldName);
                ptFieldName.Y += 30;

                Label lbFieldFormat = new Label();
                lbFieldFormat.AutoSize = true;
                lbFieldFormat.Location = ptFieldFormat;
                lbFieldFormat.Text = item.FieldFormat;

                panel1.Controls.Add(lbFieldFormat);
                ptFieldFormat.Y += 30;

                switch (item.ParameterType.ToLower())
                {
                    case "int":
                        NumericUpDown numInt = new NumericUpDown();
                        numInt.Maximum = 10000000;
                        numInt.Location = ptValue;
                        numInt.Size = sz;
                        numInt.Tag = item.ParameterName;
                        numInt.DecimalPlaces = 0;

                        panel1.Controls.Add(numInt);
                        break;
                    case "decimal":
                        NumericUpDown numDic = new NumericUpDown();
                        numDic.Maximum = 10000000;
                        numDic.Location = ptValue;
                        numDic.Size = sz;
                        numDic.Tag = item.ParameterName;
                        numDic.DecimalPlaces = 2;

                        panel1.Controls.Add(numDic);
                        break;
                    case "datetime":
                        DateTimePicker dtpTime = new DateTimePicker();
                        dtpTime.Location = ptValue;
                        dtpTime.Size = sz;
                        dtpTime.Tag = item.ParameterName;

                        panel1.Controls.Add(dtpTime);
                        break;
                    case "string":
                        TextBox txtStr = new TextBox();
                        txtStr.Location = ptValue;
                        txtStr.Size = sz;
                        txtStr.Tag = item.ParameterName;

                        panel1.Controls.Add(txtStr);
                        break;
                    default:
                        break;
                }

                ptValue.Y += 30;
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            foreach (Control cl in panel1.Controls)
            {
                if (cl.Tag == null)
                {
                    continue;
                }

                foreach (BASE_IntegrationReportList item in _Result)
                {
                    if (cl.Tag.ToString() == item.ParameterName)
                    {
                        if (cl is NumericUpDown)
                        {
                            item.ReportCode = (cl as NumericUpDown).Value.ToString();
                        }
                        else if (cl is DateTimePicker)
                        {
                            item.ReportCode = (cl as DateTimePicker).Value.ToString();
                        }
                        else if (cl is TextBox)
                        {
                            item.ReportCode = (cl as TextBox).Text.ToString();
                        }
                        else
                        {
                            item.ReportCode = null;
                        }
                    }
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
