using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using ServerModule;
using GlobalObject;

namespace UniversalControlLibrary
{
    public partial class FrmShowList : Form
    {
        bool _IsMultiSelect = false;
        public bool IsMultiSelect
        {
            get { return _IsMultiSelect; }
            set { _IsMultiSelect = value; }
        }

        DataTable _DrRowList = new DataTable();
        public DataTable DrRowList
        {
            get { return _DrRowList; }
            set { _DrRowList = value; }
        }

        private DataRow _DrShowlist = null;    //返回结果集
        public DataRow DrShowlist
        {
            get { return _DrShowlist; }
            set { _DrShowlist = value; }
        }

        private string _SlistSql = "";      //sql语句
        private string _pyColumn = "";      //拼音字段
        private string _wbColumn = "";      //五笔字段
        private string _codeColumn = "";	 //数字字段
        private string _sFind = "";         //检索字符串
        private string[] _sGrdMappingName;   //字段名称

        private Point _ptLt = new Point();

        public FrmShowList()
        {
            InitializeComponent();
        }

        /// <summary>
		/// 
		/// </summary>
		/// <param name="sql">查询的sql</param>
		/// <param name="pyColumn">拼音字段</param>
		/// <param name="wbColumn">五笔字段</param>
		/// <param name="sFind">检索的文本内容</param>
		/// <param name="grdMappingName">显示的字段名称</param>
		/// <param name="grdWidth">字段的宽度</param>
		public FrmShowList(string sql, string pyColumn, string wbColumn, string codeColumn, string sFind,
            string[] grdMappingName, Point ptLocation)
		{
			InitializeComponent();

			_SlistSql = sql;
			_pyColumn = pyColumn;
			_wbColumn = wbColumn;
			_codeColumn = codeColumn;
			_sFind = sFind;
			_sGrdMappingName = grdMappingName;

            _ptLt = ptLocation;
            if (_ptLt.X + this.Width > Screen.PrimaryScreen.Bounds.Width)
            {
                _ptLt.X = Screen.PrimaryScreen.Bounds.Width - this.Width;
            }

            if (_ptLt.Y + this.Height > Screen.PrimaryScreen.Bounds.Height)
            {
                _ptLt.Y = Screen.PrimaryScreen.Bounds.Height - this.Height;
            }
		}

        #region 自定义方法区

        /// <summary>
        /// 查询sql
        /// </summary>
        private void selectSql()
        {
            string sSql = "";
            sSql = _SlistSql + " and (";

            string strSql = "";

            if (_pyColumn.Trim() != "")
            {
                strSql += " or " + _pyColumn + " like '%" + this.txtShowlist.Text.Trim() + "%' ";
            }

            if (_wbColumn.Trim() != "")
            {
                strSql += " or " + _wbColumn + " like '%" + this.txtShowlist.Text.Trim() + "%' ";
            }

            if (_codeColumn.Trim() != "")
            {
                strSql += " or " + _codeColumn + " like '%" + this.txtShowlist.Text.Trim() + "%' ";
            }


            if (strSql.Length > 0)
            {
                strSql = strSql.Substring(3, strSql.Length - 3);
            }

            sSql += strSql + ") ";

            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = GlobalObject.GlobalParameter.StorehouseConnectionString;
                conn.Open();

                SqlCommand command = new SqlCommand(sSql);
                command.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
            }

            DataView dv = new DataView(dt);
            this.dGShowlist.DataSource = dv;

            if (_DrRowList == null || _DrRowList.Rows.Count == 0)
            {
                _DrRowList = dt.Clone(); 
            }

            if (dt.Rows.Count <= 0) return;

            if (_IsMultiSelect)
            {
                foreach (DataRow dr in _DrRowList.Rows)
                {
                    foreach (DataGridViewRow dgvr in dGShowlist.Rows)
                    {
                        if (CompareDataGridViewRow(dr, dgvr))
                        {
                            dgvr.Cells["选"].Value = true;
                        }
                    }
                }
            }
        }

        DataRow CopyDataGridViewRow(DataGridViewRow row)
        {
            DataRow resultRow = _DrRowList.NewRow();

            foreach (DataGridViewColumn dgvc in dGShowlist.Columns)
            {
                if (dgvc.Name == "选")
                {
                    continue;
                }

                resultRow[dgvc.Name] = row.Cells[dgvc.Name].Value;
            }

            return resultRow;
        }

        bool CompareDataGridViewRow(DataRow row1, DataGridViewRow row2)
        {
            foreach (DataGridViewColumn dgvc in dGShowlist.Columns)
            {
                if (dgvc.Name == "选")
                {
                    continue;
                }

                if (row1[dgvc.Name].ToString() != row2.Cells[dgvc.Name].Value.ToString())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 返回选中记录
        /// </summary>
        private void returnRow()
        {
            DataView dv = (DataView)this.dGShowlist.DataSource;

            if (dv.Table.Rows.Count <= 0 || dGShowlist.CurrentCell.RowIndex > dGShowlist.Rows.Count)
                return;

            _DrShowlist = dv[dGShowlist.CurrentRow.Index].Row;
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 创建DataGrid结构
        /// </summary>
        private void createDataGrd()
        {
            for (int i = 0; i <= _sGrdMappingName.Length - 1; i++)
            {
                DataGridViewTextBoxColumn dgtbCoumn = new DataGridViewTextBoxColumn();

                dgtbCoumn.HeaderText = _sGrdMappingName[i].Trim();
                dgtbCoumn.DataPropertyName = _sGrdMappingName[i].Trim();
            }
        }

        #endregion

        private void FrmShowlist_Load(object sender, System.EventArgs e)
        {
            if (_IsMultiSelect)
            {
                this.btnOK.Visible = true;
                this.选.Visible = true;
            }
            else
            {
                this.btnOK.Visible = false;
                this.选.Visible = false;
            }

            this.createDataGrd();  //创建DataGrid结构
            this.selectSql();
            Location = _ptLt;
            this.txtShowlist.Text = _sFind;

            txtShowlist.Focus();
            this.txtShowlist.SelectAll();
        }

        private void dGShowlist_CurrentCellChanged(object sender, System.EventArgs e)
        {
           DataView dv = (DataView)this.dGShowlist.DataSource;

            if (dv.Table.Rows.Count <= 0)
                return;
        }

        private void dGShowlist_DoubleClick(object sender, System.EventArgs e)
        {
            if (!_IsMultiSelect)
            {
                this.returnRow();  //返回选中记录
                this.Close();
            }
        }

        private void txtShowlist_TextChanged(object sender, EventArgs e)
        {
            txtShowlist.Text = txtShowlist.Text.Trim();
            this.selectSql();
        }

        private void txtShowlist_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!_IsMultiSelect)
                {
                    this.returnRow();  //返回选中记录
                    this.Close();
                }
            }

            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.No;
                this.Close();
            }

            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                dGShowlist.Focus();

                if (e.KeyCode == Keys.Up)
                {
                    SendKeys.Send("{UP}");
                }

                if (e.KeyCode == Keys.Down)
                {
                    SendKeys.Send("{DOWN}");
                }
            }
        }

        private void txtShowlist_DoubleClick(object sender, EventArgs e)
        {
            this.txtShowlist.SelectAll();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void dGShowlist_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!_IsMultiSelect && dGShowlist.CurrentRow != null)
                {
                    this.returnRow();  //返回选中记录
                    this.Close();
                }
            }
        }

        private void dGShowlist_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!_IsMultiSelect || dGShowlist.CurrentRow == null)
            {
                return;
            }

            if (dGShowlist.Columns[e.ColumnIndex].Name == "选")
            {
                dGShowlist.CurrentRow.Cells[e.ColumnIndex].Value = !Convert.ToBoolean(dGShowlist.CurrentRow.Cells[e.ColumnIndex].Value);
                bool blSel = Convert.ToBoolean(dGShowlist.CurrentRow.Cells[e.ColumnIndex].Value);

                if (blSel)
                {
                    _DrRowList.Rows.Add(CopyDataGridViewRow(dGShowlist.CurrentRow));
                }
                else
                {
                    foreach (DataRow dr in _DrRowList.Rows)
                    {
                        if (CompareDataGridViewRow(dr, dGShowlist.CurrentRow))
                        {
                            _DrRowList.Rows.Remove(dr);
                            return;
                        }
                    }
                }
            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
    }
}
