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
    public partial class CustomContextMenuStrip_Select : ContextMenuStrip
    {
        public event GlobalObject.DelegateCollection.NonArgumentHandle _选中;
        public event GlobalObject.DelegateCollection.NonArgumentHandle _取消;
        public event GlobalObject.DelegateCollection.NonArgumentHandle _全部选中;
        public event GlobalObject.DelegateCollection.NonArgumentHandle _全部取消;
        public event GlobalObject.DelegateCollection.NonArgumentHandle _选中导入项;
        public event GlobalObject.DelegateCollection.NonArgumentHandle _取消导入项;

        public CustomContextMenuStrip_Select()
        {
            InitializeComponent();
        }

        public CustomContextMenuStrip_Select(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        private void toolStripMenuItem_选中_Click(object sender, EventArgs e)
        {
            Control cltr = this.SourceControl;

            if (cltr == null)
            {
                return;
            }

            if (cltr is DataGridView)
            {
                DataGridView dgvTemp = cltr as DataGridView;

                int columnsIndex = -1;

                foreach (DataGridViewColumn cl in dgvTemp.Columns)
                {
                    if (cl.CellType == typeof(DataGridViewCheckBoxColumn)
                        || cl.CellType == typeof(DataGridViewCheckBoxCell))
                    {
                        columnsIndex = cl.Index;
                        break;
                    }
                }

                if (columnsIndex != -1)
                {
                    foreach (DataGridViewRow dgvr in dgvTemp.SelectedRows)
                    {
                        dgvr.Cells[columnsIndex].Value = true;
                    }
                }
            }
            else
            {
                if (_选中 != null)
                {
                    _选中();
                }
            }
        }

        private void toolStripMenuItem_取消_Click(object sender, EventArgs e)
        {
            Control cltr = this.SourceControl;

            if (cltr == null)
            {
                return;
            }

            if (cltr is DataGridView)
            {
                DataGridView dgvTemp = cltr as DataGridView;

                int columnsIndex = -1;

                foreach (DataGridViewColumn cl in dgvTemp.Columns)
                {
                    if (cl.CellType == typeof(DataGridViewCheckBoxColumn)
                        || cl.CellType == typeof(DataGridViewCheckBoxCell))
                    {
                        columnsIndex = cl.Index;
                        break;
                    }
                }

                if (columnsIndex != -1)
                {
                    foreach (DataGridViewRow dgvr in dgvTemp.SelectedRows)
                    {
                        dgvr.Cells[columnsIndex].Value = false;
                    }
                }
            }
            else
            {
                if (_取消 != null)
                {
                    _取消();
                }
            }
        }

        private void toolStripMenuItem_全部取消_Click(object sender, EventArgs e)
        {
            Control cltr = this.SourceControl;

            if (cltr == null)
            {
                return;
            }

            if (cltr is DataGridView)
            {
                DataGridView dgvTemp = cltr as DataGridView;

                int columnsIndex = -1;

                foreach (DataGridViewColumn cl in dgvTemp.Columns)
                {
                    if (cl.CellType == typeof(DataGridViewCheckBoxColumn)
                        || cl.CellType == typeof(DataGridViewCheckBoxCell))
                    {
                        columnsIndex = cl.Index;
                        break;
                    }
                }

                if (columnsIndex != -1)
                {
                    foreach (DataGridViewRow dgvr in dgvTemp.Rows)
                    {
                        dgvr.Cells[columnsIndex].Value = false;
                    }
                }
            }
            else
            {
                if (_全部取消 != null)
                {
                    _全部取消();
                }
            }
        }

        private void toolStripMenuItem_全部选中_Click(object sender, EventArgs e)
        {
            Control cltr = this.SourceControl;

            if (cltr == null)
            {
                return;
            }

            if (cltr is DataGridView)
            {
                DataGridView dgvTemp = cltr as DataGridView;

                int columnsIndex = -1;

                foreach (DataGridViewColumn cl in dgvTemp.Columns)
                {
                    if (cl.CellType == typeof(DataGridViewCheckBoxColumn)
                        || cl.CellType == typeof(DataGridViewCheckBoxCell))
                    {
                        columnsIndex = cl.Index;
                        break;
                    }
                }

                if (columnsIndex != -1)
                {
                    foreach (DataGridViewRow dgvr in dgvTemp.Rows)
                    {
                        dgvr.Cells[columnsIndex].Value = true;
                    }
                }
            }
            else
            {
                if (_全部选中 != null)
                {
                    _全部选中();
                }
            }
        }

        private void toolStripMenuItem_选中导入项_Click(object sender, EventArgs e)
        {
            Control cltr = this.SourceControl;

            if (cltr == null)
            {
                return;
            }

            if (cltr is DataGridView)
            {
                
                DataGridView dgvTemp = cltr as DataGridView;

                int columnsIndex = -1;

                foreach (DataGridViewColumn cl in dgvTemp.Columns)
                {
                    if (cl.CellType == typeof(DataGridViewCheckBoxColumn)
                        || cl.CellType == typeof(DataGridViewCheckBoxCell))
                    {
                        columnsIndex = cl.Index;
                        break;
                    }
                }

                if (columnsIndex != -1)
                {
                    DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

                    if (dtTemp == null)
                    {
                        return;
                    }

                    foreach (DataGridViewColumn cl in dgvTemp.Columns)
                    {
                        if (cl.Index != columnsIndex)
                        {
                            if (!dtTemp.Columns.Contains(cl.Name))
                            {
                                MessageDialog.ShowPromptMessage("文件无【" + cl.Name + "】列名");
                                return;
                            }
                        }
                    }

                    foreach (DataGridViewRow dgvr in dgvTemp.Rows)
                    {
                        foreach (DataRow dr in dtTemp.Rows)
                        {
                            foreach (DataGridViewColumn dgvc in dgvTemp.Columns)
                            {
                                if (dgvc.Index != columnsIndex)
                                {
                                    dgvr.Cells[dgvc.Name].Value = dgvr.Cells[dgvc.Name].Value == null ? "" : dgvr.Cells[dgvc.Name].Value.ToString();
                                    dr[dgvc.Name] = dr[dgvc.Name] == null ? "" : dr[dgvc.Name].ToString();

                                    if (dgvr.Cells[dgvc.Name].Value.ToString() == dr[dgvc.Name].ToString())
                                    {
                                        dgvr.Cells[columnsIndex].Value = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (_选中导入项 != null)
                {
                    _选中导入项();
                }
            }
        }

        private void toolStripMenuItem_取消导入项_Click(object sender, EventArgs e)
        {
            Control cltr = this.SourceControl;

            if (cltr == null)
            {
                return;
            }

            if (cltr is DataGridView)
            {

                DataGridView dgvTemp = cltr as DataGridView;

                int columnsIndex = -1;

                foreach (DataGridViewColumn cl in dgvTemp.Columns)
                {
                    if (cl.CellType == typeof(DataGridViewCheckBoxColumn)
                        || cl.CellType == typeof(DataGridViewCheckBoxCell))
                    {
                        columnsIndex = cl.Index;
                        break;
                    }
                }

                if (columnsIndex != -1)
                {
                    DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

                    if (dtTemp == null)
                    {
                        return;
                    }

                    foreach (DataGridViewColumn cl in dgvTemp.Columns)
                    {
                        if (cl.Index != columnsIndex)
                        {
                            if (!dtTemp.Columns.Contains(cl.Name))
                            {
                                MessageDialog.ShowPromptMessage("文件无【" + cl.Name + "】列名");
                                return;
                            }
                        }
                    }

                    foreach (DataGridViewRow dgvr in dgvTemp.Rows)
                    {
                        foreach (DataRow dr in dtTemp.Rows)
                        {
                            foreach (DataGridViewColumn dgvc in dgvTemp.Columns)
                            {
                                if (dgvc.Index != columnsIndex)
                                {
                                    dgvr.Cells[dgvc.Name].Value = dgvr.Cells[dgvc.Name].Value == null ? "" : dgvr.Cells[dgvc.Name].Value.ToString();
                                    dr[dgvc.Name] = dr[dgvc.Name] == null ? "" : dr[dgvc.Name].ToString();

                                    if (dgvr.Cells[dgvc.Name].Value.ToString() == dr[dgvc.Name].ToString())
                                    {
                                        dgvr.Cells[columnsIndex].Value = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (_取消导入项 != null)
                {
                    _取消导入项();
                }
            }
        }
    }
}
