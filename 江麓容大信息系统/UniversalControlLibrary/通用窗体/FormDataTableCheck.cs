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

namespace UniversalControlLibrary
{
    public partial class FormDataTableCheck : Form
    {
        public event GlobalObject.DelegateCollection.FormDataTableCheckFindDelegate OnFormDataTableCheckFind;

        public event GlobalObject.DelegateCollection.FormDataTableCheckDelegate OnFormDataTableCheck;

        private DataTable m_dtResult = new DataTable();

        public DataTable _DtResult
        {
            get { return m_dtResult; }
            set { m_dtResult = value; }
        }

        DataTable m_dtSource = new DataTable();

        private bool m_blDateTimeControlShow = false;

        public bool _BlDateTimeControlShow
        {
            get { return m_blDateTimeControlShow; }
            set { m_blDateTimeControlShow = value; }
        }

        private bool m_blIsCheckBox = true;

        public bool _BlIsCheckBox
        {
            get { return m_blIsCheckBox; }
            set { m_blIsCheckBox = value; }
        }

        DataTable _CheckSource = new DataTable();

        List<string> _ListKeys = new List<string>();

        /// <summary>
        /// TABLE类型的选择窗体
        /// </summary>
        /// <param name="source">数据源</param>
        public FormDataTableCheck(DataTable source)
        {
            InitializeComponent();
            m_dtSource = source;
        }

        public FormDataTableCheck(DataTable source, DataTable checkSource, List<string> lstKeys)
        {
            InitializeComponent();
            m_dtSource = source;
            _CheckSource = checkSource;
            _ListKeys = lstKeys;
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="source">数据集</param>
        void RefrshData(DataTable source)
        {
            m_dtResult = source.Clone();
            dataGridView1.DataSourceBinding.DataSource = source;

            if (m_blIsCheckBox && _CheckSource != null && _ListKeys != null)
            {
                foreach (DataRow dr in _CheckSource.Rows)
                {
                    foreach (DataGridViewRow dgvr in dataGridView1.Rows)
                    {
                        bool flag = true;

                        foreach (string keyName in _ListKeys)
                        {
                            if (dgvr.Cells[keyName].Value.ToString() != dr[keyName].ToString())
                            {
                                flag = false;
                            }
                        }

                        if (flag)
                        {
                            dgvr.Cells["选"].Value = true;
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (m_blIsCheckBox)
            {
                foreach (DataRow dr in ((DataView)dataGridView1.DataSource).Table.Rows)
                {
                    if (Convert.ToBoolean(dr["选"]))
                    {
                        DataRow drTemp = m_dtResult.NewRow();

                        foreach (DataGridViewColumn dgvc in dataGridView1.Columns)
                        {
                            if (m_dtResult.Columns.Contains(dgvc.Name))
                            {
                                drTemp[dgvc.Name] = dr[dgvc.Name];
                            }
                        }

                        m_dtResult.Rows.Add(drTemp);
                    }
                }

                if (m_dtResult == null || m_dtResult.Rows.Count == 0)
                {
                    if (MessageDialog.ShowEnquiryMessage("您未勾选任何项/记录,是否继续？") == DialogResult.No)
                    {
                        return;
                    }
                }

                if (OnFormDataTableCheck != null)
                {
                    if (OnFormDataTableCheck(m_dtResult) == DialogResult.OK)
                    {
                        this.Close();
                    }
                    else
                    {
                        return;
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                if (dataGridView1.CurrentRow == null)
                {
                    return;
                }

                DataRow dr = m_dtResult.NewRow();

                foreach (DataGridViewColumn dgvc in dataGridView1.Columns)
                {
                    if (m_dtResult.Columns.Contains(dgvc.Name))
                    {
                        dr[dgvc.Name] = dataGridView1.CurrentRow.Cells[dgvc.Name].Value;
                    }
                }

                m_dtResult.Rows.Add(dr);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                dgvr.Cells["选"].Value = true;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                dgvr.Cells["选"].Value = false;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0)
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "选")
                {
                    dataGridView1.Rows[e.RowIndex].Cells["选"].Value = !(Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells["选"].Value));

                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = true;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (OnFormDataTableCheckFind != null)
            {
                RefrshData(OnFormDataTableCheckFind(dtpStart.Value, dtpEnd.Value));
            }
        }

        private void FormDataTableCheck_Load(object sender, EventArgs e)
        {
            if (m_blIsCheckBox)
            {
                if (!m_dtSource.Columns.Contains("选"))
                {
                    m_dtSource.Columns.Add("选");
                }

                foreach (DataRow dr in m_dtSource.Rows)
                {
                    dr["选"] = false;
                }
            }

            customContextMenuStrip_Select1.Visible = m_blIsCheckBox;
            radioButton1.Enabled = m_blIsCheckBox;
            radioButton2.Enabled = m_blIsCheckBox;
            dataGridView1.Columns["选"].Visible = m_blIsCheckBox;
            groupBox3.Visible = m_blIsCheckBox;

            dtpEnd.Value = DateTime.Now;
            dtpStart.Value = DateTime.Now.AddMonths(-1);
            gbDate.Visible = m_blDateTimeControlShow;

            RefrshData(m_dtSource);
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (!m_blIsCheckBox)
            {
                if (dataGridView1.CurrentRow == null)
                {
                    return;
                }

                DataRow dr = m_dtResult.NewRow();

                foreach (DataGridViewColumn dgvc in dataGridView1.Columns)
                {
                    if (m_dtResult.Columns.Contains(dgvc.Name))
                    {
                        dr[dgvc.Name] = dataGridView1.CurrentRow.Cells[dgvc.Name].Value;
                    }
                }

                m_dtResult.Rows.Add(dr);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
