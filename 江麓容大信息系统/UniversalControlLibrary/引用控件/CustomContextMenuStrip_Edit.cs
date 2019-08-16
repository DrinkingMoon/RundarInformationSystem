using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace UniversalControlLibrary
{
    public partial class CustomContextMenuStrip_Edit : ContextMenuStrip
    {
        bool _IsAddFirstRow = false;

        public bool IsAddFirstRow
        {
            get { return _IsAddFirstRow; }
            set { _IsAddFirstRow = value; }
        }

        public event GlobalObject.DelegateCollection.DataTableHandle _InputEvent;
        public event GlobalObject.DelegateCollection.DataGridViewEditRow _AddEvent;
        public event GlobalObject.DelegateCollection.DataGridViewEditRow _DeleteEvent;
        public event GlobalObject.DelegateCollection.NonArgumentHandle _导入;
        public event GlobalObject.DelegateCollection.NonArgumentHandle _删除;
        public event GlobalObject.DelegateCollection.NonArgumentHandle _添加;
        public event GlobalObject.DelegateCollection.NonArgumentHandle _导出;

        public CustomContextMenuStrip_Edit()
        {
            InitializeComponent();
        }

        public void toolStripMenuItem_导入_Click(object sender, EventArgs e)
        {
            Control cltr = this.SourceControl;

            if (cltr == null)
            {
                return;
            }

            if (cltr is DataGridView)
            {
                DataGridView dgvTemp = cltr as DataGridView;

                DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

                if (dtTemp == null)
                {
                    return;
                }

                foreach (DataGridViewColumn cl in dgvTemp.Columns)
                {
                    if (!dtTemp.Columns.Contains(cl.HeaderText))
                    {
                        MessageDialog.ShowPromptMessage("文件无【" + cl.HeaderText + "】列名");
                        return;
                    }
                }

                if (_InputEvent != null)
                {
                    _InputEvent(dtTemp);
                }

                MessageDialog.ShowPromptMessage("导入成功");
            }
            else
            {
                if (_导入 != null)
                {
                    _导入();
                }
            }
        }

        public void toolStripMenuItem_删除_Click(object sender, EventArgs e)
        {
            Control cltr = this.SourceControl;

            if (cltr == null)
            {
                return;
            }

            if (cltr is DataGridView)
            {
                DataGridView dgvTemp = cltr as DataGridView;

                if (dgvTemp.Rows.Count > 0)
                {
                    foreach (DataGridViewRow dr in dgvTemp.SelectedRows)
                    {
                        DataGridViewRow deleteRow = dr;

                        dgvTemp.Rows.Remove(dr);

                        if (_DeleteEvent != null)
                        {
                            _DeleteEvent(ref deleteRow);
                        }
                    }
                }
            }
            else
            {
                if (_删除 != null)
                {
                    _删除();
                }
            }
        }

        public void toolStripMenuItem_添加_Click(object sender, EventArgs e)
        {
            Control cltr = this.SourceControl;

            if (cltr == null)
            {
                return;
            }

            if (cltr is DataGridView)
            {
                DataGridView dgvTemp = cltr as DataGridView;

                DataGridViewRow dgvr = new DataGridViewRow();
                dgvr.CreateCells(dgvTemp);

                foreach (DataGridViewColumn cl in dgvTemp.Columns)
                {
                    if (cl is DataGridViewNumericUpDownColumn)
                    {
                        dgvr.Cells[cl.Index].Value = 0;
                    }
                    else
                    {
                        dgvr.Cells[cl.Index].Value = "";
                    }
                }

                if (_AddEvent != null)
                {
                    _AddEvent(ref dgvr);
                }

                int index = 0;
                if (_IsAddFirstRow)
                {
                    dgvTemp.Rows.Insert(0, dgvr);
                }
                else
                {
                    dgvTemp.Rows.Add(dgvr);
                    index = dgvTemp.Rows.Count - 1;
                }

                int columnIndex = 0;
                foreach (DataGridViewColumn cl in dgvTemp.Columns)
                {
                    if (cl.Visible)
                    {
                        columnIndex = cl.Index;
                        break;
                    }
                }

                dgvTemp.CurrentCell = dgvTemp.Rows[index].Cells[columnIndex];
                dgvTemp.FirstDisplayedScrollingRowIndex = index;
            }
            else
            {
                if (_添加 != null)
                {
                    _添加();
                }
            }

        }

        public void toolStripMenuItem_导出_Click(object sender, EventArgs e)
        {
            Control cltr = this.SourceControl;

            if (cltr == null)
            {
                return;
            }

            if (cltr is DataGridView)
            {
                DataGridView dgvTemp = cltr as DataGridView;
                ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dgvTemp);
            }
            else
            {
                if (_导出 != null)
                {
                    _导出();
                }
            }
        }

        public CustomContextMenuStrip_Edit(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
