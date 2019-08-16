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
    public partial class FormGoodsSelect : Form
    {
        View_F_GoodsPlanCost m_GoodsInfo = new View_F_GoodsPlanCost();

        public View_F_GoodsPlanCost GoodsInfo
        {
            get { return m_GoodsInfo; }
            set { m_GoodsInfo = value; }
        }

        string _strSql;

        DataTable _goodsSelectTable;

        public FormGoodsSelect(string strSql)
        {
            InitializeComponent();
            _strSql = strSql;
        }

        public FormGoodsSelect(DataTable goodsSelectTable)
        {
            InitializeComponent();
            _goodsSelectTable = goodsSelectTable;
        }

        private void btnFindCode_Click(object sender, EventArgs e)
        {
            DataTable tempTable = new DataTable();

            if (_strSql != null && _strSql.Trim().Length == 0)
            {
                tempTable = GlobalObject.DatabaseServer.QueryInfo(_strSql);
            }
            else if (_goodsSelectTable != null && _goodsSelectTable.Rows.Count != 0)
            {
                tempTable = _goodsSelectTable;
            }

            if (tempTable != null && tempTable.Rows.Count != 0)
            {
                FormQueryInfo frm = new FormQueryInfo(tempTable);

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    txtCode.Text = frm.GetDataItem("图号型号").ToString();
                    txtCode.Tag = frm.GetDataItem("物品ID");
                    txtName.Text = frm.GetDataItem("物品名称").ToString();
                    txtSpec.Text = frm.GetDataItem("规格").ToString();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim().Length == 0)
            {
                MessageBox.Show("请选择物品");
                return;
            }

            m_GoodsInfo = UniversalFunction.GetGoodsInfo((int)txtCode.Tag);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
