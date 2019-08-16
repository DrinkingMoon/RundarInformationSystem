using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 与BOM比较界面
    /// </summary>
    public partial class FormCompareBom : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 查询参数服务接口
        /// </summary>
        ISearchParamsServer m_searchParamsServer = BasicServerFactory.GetServerModule<ISearchParamsServer>();

        /// <summary>
        /// 不合格产品隔离服务类（只要通过GoodsID获得产品信息）
        /// </summary>
        IQuarantine m_serverQuarantine = ServerModuleFactory.GetServerModule<IQuarantine>();

        /// <summary>
        /// 产品信息管理服务组件
        /// </summary>
        IProductInfoServer m_productInfoServer = ServerModuleFactory.GetServerModule<IProductInfoServer>();

        /// <summary>
        /// 供应商配额服务组件
        /// </summary>
        IGoodsLeastPackAndStock m_GoodsLeast = ServerModuleFactory.GetServerModule<IGoodsLeastPackAndStock>();

        /// <summary>
        /// Bom服务组件
        /// </summary>
        IBomServer m_bomServer = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 与Bom相比较的表
        /// </summary>
        string m_type;

        public FormCompareBom(string type)
        {
            InitializeComponent();

            m_type = type;

            if (!GlobalObject.GeneralFunction.IsNullOrEmpty("比较Bom"))
            {
                string[] paramsName = m_searchParamsServer.GetSearchName("比较Bom");

                if (paramsName != null)
                {
                    cmbSearchName.Items.Clear();
                    cmbSearchName.Items.Add("");
                    cmbSearchName.Items.AddRange(paramsName);
                }
            }

            IQueryable<View_P_ProductInfo> productInfo = null;

            if (!m_productInfoServer.GetAllProductInfo(out productInfo, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
            }
            else
            {
                foreach (var item in productInfo)
                {
                    if (!item.是否返修专用)
                    {
                        clbEdition.Items.Add(item.产品类型编码);
                    }
                }
            }
        }

        private void 添加物品toolStripButton_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = new DataGridViewRow();
            dgvBomReject.Rows.Add(dr);
        }

        private void dgvBomReject_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            DataGridViewColumnCollection columns = this.dgvBomReject.Columns;

            switch (columns[e.ColumnIndex].Name)
            {
                case "选择物品":
                    btnFindCode(dgvBomReject.Rows[e.RowIndex],false);
                    break;
            }
        }

        /// <summary>
        /// 查找物品
        /// </summary>
        /// <param name="row">选择行</param>
        /// <param name="flag">是否是本身表的零件</param>
        private void btnFindCode(DataGridViewRow row,bool flag)
        {
            FormQueryInfo form = QueryInfoDialog.GetPlanCostGoodsDialog();

            if (flag)
            {
                if (m_type == "安全库存")
                {
                    form = QueryInfoDialog.GetSafeStock();
                }
                else if (m_type == "供应商配额")
                {
                    form = QueryInfoDialog.GetGoodsLeastPackAndStock();
                }
            }

            if (form != null && form.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < dgvBomReject.Rows.Count; i++)
                {
                    if (dgvBomReject.Rows[i].Cells["物品ID"].Value != null &&
                        Convert.ToInt32(dgvBomReject.Rows[i].Cells["物品ID"].Value) == (int)form.GetDataItem("序号"))
                    {
                        if (MessageBox.Show("此物品有重复记录是否继续？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }
                    }
                }

                if (flag)
                {
                    row.Cells["Self图号型号"].Value = (string)form.GetDataItem("图号型号");
                    row.Cells["Self物品ID"].Value = (int)form.GetDataItem("序号");
                    row.Cells["Self物品名称"].Value = (string)form.GetDataItem("物品名称");
                    row.Cells["Self规格"].Value = (string)form.GetDataItem("规格");

                    DataGridViewRow dr = new DataGridViewRow();
                    dgvSelfReject.Rows.Add(dr);
                }
                else
                {
                    row.Cells["图号型号"].Value = (string)form.GetDataItem("图号型号");
                    row.Cells["物品ID"].Value = (int)form.GetDataItem("序号");
                    row.Cells["物品名称"].Value = (string)form.GetDataItem("物品名称");
                    row.Cells["规格"].Value = (string)form.GetDataItem("规格");

                    DataGridViewRow dr = new DataGridViewRow();
                    dgvBomReject.Rows.Add(dr);
                }                
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvBomReject.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("还没有设置不比较的零件无法进行保存");
                return;
            }
            else if (CheckDataGridView())
            {
                string searchName = InputBox.ShowDialog("保存检索条件", "检索条件名称：", cmbSearchName.Text);

                if (!GlobalObject.GeneralFunction.IsNullOrEmpty(searchName))
                {
                    List<SYS_SearchParams> lstParam = new List<SYS_SearchParams>();

                    int orderNo = 0;
                    string error = null;


                    for (int j = 0; j < dgvBomReject.Rows.Count; j++)
                    {
                            SYS_SearchParams param = new SYS_SearchParams();

                            param.BusinessName = "比较Bom";
                            param.ItemName = dgvBomReject.Rows[j].Cells[dgvBomReject.Columns.Count - 1].Value.ToString();
                            param.SearchName = searchName;
                            param.FieldName = dgvBomReject.Columns[1].HeaderText;
                            param.Operator = "<>";
                            param.OrderNo = orderNo++;
                            param.Logic = "and";
                            param.DataType = "String";
                            param.DataValue = dgvBomReject.Rows[j].Cells[1].Value.ToString();
                            param.UserCode = GlobalObject.BasicInfo.LoginID;

                            lstParam.Add(param);
                    }

                    if (!m_searchParamsServer.AddParam(lstParam, out error))
                    {
                        MessageDialog.ShowErrorMessage(error);
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("成功保存查询条件");

                        if (!cmbSearchName.Items.Contains(searchName))
                        {
                            cmbSearchName.Items.Add("");
                            cmbSearchName.Items.Add(searchName);
                            cmbSearchName.Text = searchName;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 检测datagridview
        /// </summary>
        /// <returns>数据正确返回True，没添加零件或数据不正确返回False</returns>
        bool CheckDataGridView()
        {
            if (dgvBomReject.Rows.Count > 0)
            {
                for (int i = 0; i < dgvBomReject.Rows.Count; i++)
                {
                    if (dgvBomReject.Rows[i].Cells[1].Value.ToString() == "" || dgvBomReject.Rows[i].Cells[5].Value.ToString() == "")
                    {
                        MessageDialog.ShowPromptMessage("请选择零件并选择剔除模式！");
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        private void btnDeleteSearch_Click(object sender, EventArgs e)
        {
            if (cmbSearchName.Text == "")
            {
                MessageDialog.ShowPromptMessage("不允许删除空查询");
                return;
            }
            else
            {
                if (MessageDialog.ShowEnquiryMessage("您真的要删除我的查询“" + cmbSearchName.Text + "”？") == DialogResult.Yes)
                {
                    string error = null;

                    if (!m_searchParamsServer.DeleteParamCompareBom("比较Bom", cmbSearchName.Text, out error))
                    {
                        MessageDialog.ShowErrorMessage(error);
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("操作成功");

                        cmbSearchName.Items.Remove(cmbSearchName.Text);
                    }
                }
            }
        }

        private void cmbSearchName_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvBomReject.Rows.Clear();

            if (cmbSearchName.Text != "")
            {
                string error = null;

                IQueryable<SYS_SearchParams> searchParams = m_searchParamsServer.GetParams(
                    "比较Bom", cmbSearchName.Text, out error);

                if (searchParams == null)
                {
                    return;
                }

                if (!GlobalObject.GeneralFunction.IsNullOrEmpty(error))
                {
                    MessageDialog.ShowErrorMessage(error);
                    return;
                }

                dgvBomReject.Rows.Clear();
                foreach (var item in searchParams)
                {
                    DataGridViewRow dr = new DataGridViewRow();

                    DataTable dtGoodPlan = m_serverQuarantine.GetProductCodeInfo(item.DataValue);

                    dgvBomReject.Rows.Add(new object[] { "", item.DataValue, dtGoodPlan.Rows[0]["GoodsCode"].ToString(), dtGoodPlan.Rows[0]["GoodsName"].ToString(), dtGoodPlan.Rows[0]["Spec"].ToString(), item.ItemName });
                }
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            dgvBomShow.DataSource = null;
            dgvSelfShow.DataSource = null;

            string whereSql = "";
            string editionSql = "";
            string selfRejectSql = "";

            if (clbEdition.CheckedItems.Count > 0)
            {
                editionSql = "版本 in (";

                for (int i = 0; i < clbEdition.CheckedItems.Count; i++)
                {
                    if (i < clbEdition.CheckedItems.Count -1)
                    {
                        editionSql += "'" + clbEdition.CheckedItems[i].ToString() + "',";
                    }
                    else
                    {
                        editionSql += "'" + clbEdition.CheckedItems[i].ToString() + "')";
                    }
                }

                if (dgvSelfReject.Rows.Count > 0)
                {
                    selfRejectSql = " and 图号型号 not in(";

                    for (int i = 0; i < dgvSelfReject.Rows.Count; i++)
                    {
                        if (i < dgvSelfReject.Rows.Count - 1)
                        {
                            selfRejectSql += "'" + dgvSelfReject.Rows[i].Cells["Self图号型号"].Value.ToString() + "',";
                        }
                        else
                        {
                            selfRejectSql += "'" + dgvSelfReject.Rows[i].Cells["Self图号型号"].Value.ToString() + "')";
                        }
                    }
                }

                if (CheckDataGridView())
                {
                    for (int i = 0; i < dgvBomReject.Rows.Count; i++)
                    {
                        if (dgvBomReject.Rows[i].Cells["剔除模式"].Value.ToString() == "仅零件本身")
                        {
                            whereSql += " and 零部件编码 <> '" + dgvBomReject.Rows[i].Cells["图号型号"].Value.ToString() + "'";
                        }
                        else if (dgvBomReject.Rows[i].Cells["剔除模式"].Value.ToString() == "子零件")
                        {
                            whereSql += " and 父总成编码 <> '" + dgvBomReject.Rows[i].Cells["图号型号"].Value.ToString() + "'";
                        }
                        else
                        {
                            whereSql += " and 零部件编码 <> '" + dgvBomReject.Rows[i].Cells["图号型号"].Value.ToString() + "'" +
                                        " and 父总成编码 <> '" + dgvBomReject.Rows[i].Cells["图号型号"].Value.ToString() + "'";
                        }
                    }

                    string dataTableName;

                    if (m_type == "安全库存")
                    {
                        dataTableName = "View_S_SafeStock";
                    }
                    else
                    {
                        dataTableName = "View_B_GoodsLeastPackAndStock";
                    }

                    DataTable dtUseBom = m_GoodsLeast.GetGoodsUseBomTable(editionSql,whereSql, dataTableName);

                    if (dtUseBom != null && dtUseBom.Rows.Count > 0)
                    {
                        dgvBomShow.DataSource = dtUseBom;
                    }

                    DataTable dtBomNotUse = m_GoodsLeast.GetGoodsNotUseBomTable(selfRejectSql, dataTableName);

                    if (dtBomNotUse != null && dtBomNotUse.Rows.Count > 0)
                    {
                        dgvSelfShow.DataSource = dtBomNotUse;
                    }
                }
                else if (dgvBomReject.Rows.Count == 0)
                {
                    string dataTableName;

                    if (m_type == "安全库存")
                    {
                        dataTableName = "View_S_SafeStock";
                    }
                    else
                    {
                        dataTableName = "View_B_GoodsLeastPackAndStock";
                    }

                    DataTable dt = m_GoodsLeast.GetGoodsUseBomTable(editionSql,whereSql, dataTableName);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dgvBomShow.DataSource = dt;
                    }

                    DataTable dtBomNotUse = m_GoodsLeast.GetGoodsNotUseBomTable(selfRejectSql, dataTableName);

                    if (dtBomNotUse != null && dtBomNotUse.Rows.Count > 0)
                    {
                        dgvSelfShow.DataSource = dtBomNotUse;
                    }
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请先选择产品类型！");
                return;
            }
        }

        private void 添加剔除本身表物品toolStripButton_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = new DataGridViewRow();
            dgvSelfReject.Rows.Add(dr);
        }

        private void 删除选中剔除Bom表的零件行toolStripButton_Click(object sender, EventArgs e)
        {
            if (dgvBomReject.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择至少一条记录后再进行此操作");
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("您确定要删除选择行？") == DialogResult.No)
            {
                return;
            }

            for (int i = dgvBomReject.SelectedRows.Count; i > 0; i--)
            {
                dgvBomReject.Rows.Remove(dgvBomReject.SelectedRows[i - 1]);
            }
        }

        private void 删除选中剔除本身表的零件行toolStripButton_Click(object sender, EventArgs e)
        {
            if (dgvSelfReject.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择至少一条记录后再进行此操作");
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("您确定要删除选择行？") == DialogResult.No)
            {
                return;
            }

            for (int i = dgvSelfReject.SelectedRows.Count; i > 0; i--)
            {
                dgvSelfReject.Rows.Remove(dgvSelfReject.SelectedRows[i - 1]);
            }
        }

        private void dgvSelfReject_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            DataGridViewColumnCollection columns = this.dgvBomReject.Columns;

            switch (columns[e.ColumnIndex].Name)
            {
                case "选择物品":
                    btnFindCode(dgvSelfReject.Rows[e.RowIndex],true);
                    break;
            }
        }

        private void dgvBomShow_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
               e.RowBounds.Location.Y,
               dgvBomShow.RowHeadersWidth - 4,
               e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dgvBomShow.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dgvBomShow.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void dgvSelfShow_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
               e.RowBounds.Location.Y,
               dgvSelfShow.RowHeadersWidth - 4,
               e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dgvSelfShow.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dgvSelfShow.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void 导出Bom_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dgvBomReject);
        }

        private void 导出本身_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog2, dgvSelfReject);
        }
    }
}
