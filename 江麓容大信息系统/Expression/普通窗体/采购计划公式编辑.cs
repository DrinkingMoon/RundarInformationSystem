using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;
using System.Runtime.InteropServices;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 采购计划公式编辑 : Form
    {
        string strTemp = "";

        bool flag = false;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 采购计划服务组件
        /// </summary>
        IPurcharsingPlan m_serverPurcharsingPlan = ServerModuleFactory.GetServerModule<IPurcharsingPlan>();

        List<string> list = new List<string>();

        public Point GetCursorCoordinates(TextBox textBox1)
        {
            Graphics gc = textBox1.CreateGraphics();

            int wd = (int)gc.MeasureString("x", textBox1.Font).Width;
            int ht = (int)gc.MeasureString("x", textBox1.Font).Height;

            Point p2 = textBox1.GetPositionFromCharIndex(textBox1.Text.Length - 1);
            Point p = textBox1.Location;

            p.X += p2.X + wd;
            p.Y += p2.Y + ht;

            return p;
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(string billNo)
        {
            string strColName = "";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["物品ID"].Value.ToString() == billNo)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        public 采购计划公式编辑(DataGridViewColumnCollection dataGirdViewColumns, CG_ProcurementSteps procurement)
        {
            InitializeComponent();

            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.ItemHeight = 15;

            numMathSteps.Value = procurement.CalculationSteps;
            numMathSteps.Tag = procurement.StepsID;
            txtStepsName.Text = procurement.StepsName;

            foreach (DataGridViewColumn item in dataGirdViewColumns)
            {
                cmbField.Items.Add(item.Name);
                list.Add("[" + item.Name + "]");
            }

            cmbField.SelectedIndex = -1;
            RefrshData();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefrshData()
        {
            dataGridView1.DataSource = m_serverPurcharsingPlan.GetProcurementPlanView(" and 步骤ID = " + Convert.ToInt32(numMathSteps.Tag));
            dataGridView1.Columns["公式名称"].Width = 200;
            dataGridView1.Columns["计算字段"].Width = 200;
            dataGridView1.Columns["序号"].Visible = false;
            dataGridView1.Columns["步骤ID"].Visible = false;
            dataGridView1.Columns["公式ID"].Visible = false;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            numMathSteps.Tag = dataGridView1.CurrentRow.Cells["步骤ID"].Value;
            txtCode.Tag = dataGridView1.CurrentRow.Cells["物品ID"].Value;
            txtCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
            txtName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
            txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
            cmbField.Text = dataGridView1.CurrentRow.Cells["计算字段"].Value.ToString();
            txtMathName.Tag = dataGridView1.CurrentRow.Cells["公式ID"].Value.ToString();
            txtMathName.Text = dataGridView1.CurrentRow.Cells["公式名称"].Value.ToString();
            txtFormula.Text = dataGridView1.CurrentRow.Cells["计算公式"].Value.ToString();
            txtFormula.Tag = dataGridView1.CurrentRow.Cells["序号"].Value;

            if (txtCode.Tag.ToString() == "0")
            {
                chbAll.Checked = true;
            }
            else
            {
                chbAll.Checked = false;
            }
        }

        /// <summary>
        /// 获得实体集信息
        /// </summary>
        /// <returns>返回LINQ</returns>
        CG_ProcurementPlan GetPlanMessage()
        {
            CG_ProcurementPlan lnqModel = new CG_ProcurementPlan();

            lnqModel.StepsID = Convert.ToInt32(numMathSteps.Tag);
            lnqModel.GoodsID = Convert.ToInt32(txtCode.Tag);
            lnqModel.ID = txtFormula.Tag == null ? 0 : Convert.ToInt32(txtFormula.Tag);
            lnqModel.MathID = txtMathName.Tag.ToString() == "" ? 0 : Convert.ToInt32(txtMathName.Tag);

            return lnqModel;
        }

        /// <summary>
        /// 获得实体集信息
        /// </summary>
        /// <returns>返回LINQ</returns>
        CG_ProcurementMath GetMathMessage()
        {
            CG_ProcurementMath lnqModel = new CG_ProcurementMath();

            lnqModel.MathID = txtMathName.Tag.ToString() == "" ? 0 : Convert.ToInt32(txtMathName.Tag);
            lnqModel.MathFormula = txtFormula.Text;
            lnqModel.MathName = txtMathName.Text;
            lnqModel.MathColumn = cmbField.Text;

            return lnqModel;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!m_serverPurcharsingPlan.OperatorProcuremnetPlanFormla(CE_OperatorMode.添加, GetPlanMessage(), GetMathMessage(), out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("添加成功");
            }

            string strGoodsID = txtCode.Tag.ToString();

            RefrshData();
            PositioningRecord(strGoodsID);
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (!m_serverPurcharsingPlan.OperatorProcuremnetPlanFormla(CE_OperatorMode.修改, GetPlanMessage(), GetMathMessage(), out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("修改成功");
            }

            string strGoodsID = txtCode.Tag.ToString();

            RefrshData();
            PositioningRecord(strGoodsID);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (!m_serverPurcharsingPlan.OperatorProcuremnetPlanFormla(CE_OperatorMode.删除, GetPlanMessage(), GetMathMessage(), out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("删除成功");
            }

            RefrshData();
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Set the DrawMode property to draw fixed sized items. 
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            // Draw the background of the ListBox control for each item. 
            e.DrawBackground();
            // Define the default color of the brush as black. 
            Brush myBrush = Brushes.Black;
            FontFamily fontFamily = new FontFamily("宋体");

            System.Drawing.Font myFont = new Font(fontFamily, 10);

            // Determine the color of the brush to draw each item based on the index of the item to draw . 

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                //e.Graphics.FillRectangle(Brushes.Blue, e.Bounds); 
                if (e.Index > -1)
                {
                    e.Graphics.DrawString(listBox1.Items[e.Index].ToString(), myFont, Brushes.White, e.Bounds, StringFormat.GenericDefault);
                }
            }
            else
            {
                //e.Graphics.FillRectangle(Brushes.White, e.Bounds); 
                if (e.Index > -1)
                {
                    e.Graphics.DrawString(listBox1.Items[e.Index].ToString(), myFont, myBrush, e.Bounds, StringFormat.GenericDefault);
                }
            }

            e.DrawFocusRectangle();
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.listBox1.Visible = false;
                this.txtFormula.Text = this.txtFormula.Text.Substring(0, txtFormula.Text.Length - 2);
                txtFormula.Text = txtFormula.Text.Trim();
                this.txtFormula.Focus();
            }

            if (e.KeyCode == Keys.Enter && this.listBox1.Visible && this.listBox1.SelectedItems.Count > 0)
            {
                e.Handled = true;

                this.txtFormula.Text = this.txtFormula.Text.Substring(0, txtFormula.Text.Length - 2) + this.listBox1.SelectedItems[0].ToString().Replace("[", "").Replace("]","");
                this.listBox1.Visible = false;
                txtFormula.Text = txtFormula.Text.Trim();
                this.txtFormula.Focus();
                this.txtFormula.Select(this.txtFormula.Text.Length, 0);
            }
        }

        private void txtFormula_KeyDown(object sender, KeyEventArgs e)
        {
            if (flag)
            {
                strTemp += e.KeyCode.ToString();
            }

            if (e.KeyCode == Keys.Down && this.listBox1.Visible)
            {
                this.listBox1.Focus();

                if (this.listBox1.SelectedItems.Count > 0)
                {
                    this.listBox1.SetSelected(this.listBox1.SelectedIndex, false);
                }

                if (this.listBox1.Items.Count > 0)
                {
                    this.listBox1.SetSelected(0, true);
                }
            }

            if (e.Control)
            {
                e.Handled = true;
            }

            if (e.KeyCode == Keys.J && e.Control)
            {
                e.Handled = true;

                List<string> listTemp = new List<string>();

                foreach (string item in list)
                {
                    if (item.Contains(strTemp))
                    {
                        listBox1.Items.Add(item);
                    }
                }

                listBox1.Location = GetCursorCoordinates(txtFormula);
                listBox1.Visible = true;
                listBox1.Focus();
            }

            if (e.KeyCode == Keys.K && e.Control)
            {
                string strSql = "select 序号,图号型号, 物品名称, 规格,单位,物品类别,拼音码,五笔码 from  View_F_GoodsPlanCost where  1= 1 ";
                string[] SZstring = new string[] { "图号型号", "物品名称", "物品类别", "规格", "单位" };
                string strPyColunm = "拼音码";
                string strWbColunm = "图号型号";
                string strCodeColunm = "物品名称";
                string strShowMessage = "序号";

                Point p = GetCursorCoordinates(txtFormula);


                FrmShowList show_list = new FrmShowList(strSql, strPyColunm, strWbColunm, strCodeColunm, "", SZstring,
                    Point.Add(Point.Add(this.Location, new Size(p)), new Size { Height = 30 }));
                show_list.Focus();

                if (show_list.ShowDialog() == DialogResult.OK)
                {
                    this.txtFormula.Text +=  show_list.DrShowlist[strShowMessage].ToString();
                }
                else
                {
                    this.txtFormula.Text += "";
                }

                txtFormula.Text = txtFormula.Text.Trim();
                this.txtFormula.Focus();
                this.txtFormula.Select(this.txtFormula.Text.Length, 0);
            }
        }

        private void listBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.listBox1.SelectedItems.Count > 0)
            {
                this.txtFormula.Text = this.listBox1.SelectedItems[0].ToString();

                this.listBox1.Visible = false;
                this.txtFormula.Focus();
            }
        }

        private void listBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Point m = new Point(e.X, e.Y);

            int index = GetItemAt(this.listBox1, e.X, e.Y);

            if (this.listBox1.SelectedItems.Count > 0 && this.listBox1.SelectedIndex != index)
            {

                this.listBox1.SetSelected(this.listBox1.SelectedIndex, false);
            }
            if (index != -1 && this.listBox1.SelectedIndex != index)
            {

                this.listBox1.SetSelected(index, true);
            }
        }

        private int GetItemAt(ListBox listBox, int X, int Y)
        {
            int index = -1;

            for (int i = 0; i < listBox.Items.Count; i++)
            {
                System.Drawing.Rectangle r = listBox.GetItemRectangle(i);
                if (r.Contains(new Point(X, Y)))
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        private void txtCode_OnCompleteSearch()
        {
            txtCode.Tag = txtCode.DataResult["序号"];
            txtCode.Text = txtCode.DataResult["图号型号"].ToString();
            txtName.Text = txtCode.DataResult["物品名称"].ToString();
            txtSpec.Text = txtCode.DataResult["规格"].ToString();
            txtMathName.Text = "";
            txtMathName.Tag = "";
        }

        private void btnFindCode_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = QueryInfoDialog.GetProcurementMath();

            if (DialogResult.OK == form.ShowDialog())
            {
                txtMathName.Text = form.GetDataItem("公式名称").ToString();
                txtMathName.Tag = form.GetDataItem("公式ID").ToString();
                cmbField.Text = form.GetDataItem("计算字段").ToString();
                txtFormula.Text = form.GetDataItem("计算公式").ToString();
            }
        }

        private void btnSaveFormula_Click(object sender, EventArgs e)
        {
            CG_ProcurementMath lnqMath = new CG_ProcurementMath();

            lnqMath.MathID = txtMathName.Tag.ToString() == "" ? 0 : Convert.ToInt32(txtMathName.Tag);
            lnqMath.MathName = txtMathName.Text;
            lnqMath.MathColumn = cmbField.Text;
            lnqMath.MathFormula = txtFormula.Text;

            if (!m_serverPurcharsingPlan.SaveProcurementMath(lnqMath,out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("公式保存成功");
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtMathName.Text = "";
            txtMathName.Tag = "";
        }

        private void chbAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chbAll.Checked)
            {
                txtCode.Enabled = false;
                txtCode.Tag = 0;
                txtName.Text = "全部物品";
            }
            else
            {
                txtCode.Enabled = true;
                txtCode.Tag = "";
            }
        }

        private void btnDeleteMath_Click(object sender, EventArgs e)
        {
            if (!m_serverPurcharsingPlan.DeleteProcurementMath(Convert.ToInt32(txtMathName.Tag),out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("公式删除成功");
            }
        }
    }
}
