using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;
using Service_Economic_Financial;
using ServerModule;

namespace Form_Economic_Financial
{
    public partial class 最低销售定价 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 最低定价服务组件
        /// </summary>
        ILowestSellPriceServer m_LowestPriceServer = Service_Economic_Financial.ServerModuleFactory.GetServerModule<ILowestSellPriceServer>();

        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_basicGoodsServer = ServerModule.ServerModuleFactory.GetServerModule<ServerModule.IBasicGoodsServer>();

        /// <summary>
        /// 客户信息服务类
        /// </summary>
        IClientServer m_clientServer = ServerModule.ServerModuleFactory.GetServerModule<ServerModule.IClientServer>();

        public 最低销售定价(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            RefreshDataGridView();
            txtRecorder.Text = BasicInfo.LoginName;
            txtRecordTime.Text = ServerTime.Time.ToString();
        }

        private void 最低销售定价_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, authorityFlag);
        }

         /// <summary>
        /// 刷新
        /// </summary>
        private void RefreshDataGridView()
        {
            DataTable dt = m_LowestPriceServer.GetAllInfo();

            if (dt != null)
            {
                dataGridView1.DataSource = dt;

                dataGridView1.Columns["GoodsID"].Visible = false;
            }

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
               UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

            dataGridView1.Columns["ClientID"].Visible = false;
            dataGridView1.Columns["CommunicateID"].Visible = false;
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtGoodsCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
            txtGoodsCode.Tag = dataGridView1.CurrentRow.Cells["GoodsID"].Value.ToString();
            txtRecorder.Text = dataGridView1.CurrentRow.Cells["定价人"].Value.ToString();
            txtRecordTime.Text = dataGridView1.CurrentRow.Cells["定价时间"].Value.ToString();
            txtSpce.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
            tbsGoods.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
            numLowestPrice.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["最低定价"].Value.ToString());
            numUnitPrice.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["单价"].Value);
            lbUnit.Text = dataGridView1.CurrentRow.Cells["单位"].Value.ToString();
            txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            txtClient.Text = dataGridView1.CurrentRow.Cells["主机厂"].Value.ToString();
            txtClient.Tag = dataGridView1.CurrentRow.Cells["ClientID"].Value.ToString();
            tbsClientGoodsCode.Tag = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["CommunicateID"].Value);
            tbsClientGoodsCode.Text = dataGridView1.CurrentRow.Cells["主机厂图号型号"].Value.ToString();
            txtClientGoodsName.Text = dataGridView1.CurrentRow.Cells["主机厂物品名称"].Value.ToString();
            numTerminalPrice.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["终端价格"].Value.ToString());
        }

        private void tbsGoods_OnCompleteSearch()
        {
            txtGoodsCode.Text = tbsGoods.DataResult["图号型号"].ToString();
            txtGoodsCode.Tag = tbsGoods.DataResult["序号"].ToString();
            tbsGoods.Text = tbsGoods.DataResult["物品名称"].ToString();
            txtSpce.Text = tbsGoods.DataResult["规格"].ToString();
            lbUnit.Text = tbsGoods.DataResult["单位"].ToString();
            numUnitPrice.Value = Convert.ToDecimal(tbsGoods.DataResult["单价"].ToString());
            numLowestPrice.Value = Convert.ToDecimal(tbsGoods.DataResult["单价"].ToString());
        }

        /// <summary>
        /// 检测控件的正确性
        /// </summary>
        /// <returns>正确返回true，否则返货false</returns>
        bool CheckControl() 
        {
            if (tbsClientGoodsCode.Text.Trim() == "" || txtGoodsCode.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择物品及主机厂的零件！");
                return false;
            }

            if (numLowestPrice.Value == 0)
            {
                if (MessageDialog.ShowEnquiryMessage("确定物品的最低定价为 0 吗？") == DialogResult.No)
                {
                    return false;
                }
            }

            return true;
        }

        private void 保存toolStripButton_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            YX_LowestMarketPrice lowest = new YX_LowestMarketPrice();

            lowest.Date = ServerTime.Time;
            lowest.GoodsID = Convert.ToInt32(txtGoodsCode.Tag.ToString());
            lowest.Price = numLowestPrice.Value;
            lowest.Rater = BasicInfo.LoginID;
            lowest.Remark = txtRemark.Text;
            lowest.ClientID = txtClient.Tag.ToString();
            lowest.CommunicateID = Convert.ToInt32( tbsClientGoodsCode.Tag);
            lowest.TerminalPrice = numTerminalPrice.Value;

            if (!m_LowestPriceServer.InsertAndUpdateData(lowest, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }

            RefreshDataGridView();
        }

        private void 删除toolStripButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (MessageDialog.ShowEnquiryMessage("确定删除选中数据吗？") == DialogResult.No)
                {
                    return;
                }
                else
                {
                    if (!m_LowestPriceServer.DeleteData(Convert.ToInt32(dataGridView1.CurrentRow.Cells["GoodsID"].Value), out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的一行数据！");
                return;
            }

            RefreshDataGridView();
        }

        private void 导入toolStripButton_Click(object sender, EventArgs e)
        {
            DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

            if (dtTemp == null)
            {
                //MessageDialog.ShowPromptMessage(m_error);
                return;
            }

            if (!CheckTable(dtTemp))
            {
                return;
            }

            DataTable dtClone = dtTemp.Clone();

            foreach (DataRow dr in dtTemp.Rows)
            {
                YX_LowestMarketPrice lowest = new YX_LowestMarketPrice();

                int GoodsID = m_basicGoodsServer.GetGoodsIDByGoodsCode(dr["容大代码"].ToString(), 
                    dr["零件名称"].ToString(), dr["规格"].ToString());

                if (GoodsID != 0 && GoodsID != 7126)
                {
                    lowest.GoodsID = GoodsID;
                }
                else
                {
                    AddErrorInfo(ref dtClone, dr);
                    continue;
                }

                string clientCode = m_clientServer.GetClientCode(dr["客户名称"].ToString());

                if (clientCode != "")
                {
                    lowest.ClientID = clientCode;
                }
                else
                {
                    AddErrorInfo(ref dtClone, dr);
                    continue;
                }

                int? communicateID = m_LowestPriceServer.GetCommunicateID(lowest.ClientID, dr["主机厂图号型号"].ToString(),
                    dr["主机厂物品名称"].ToString(), lowest.GoodsID, out m_error);

                if (communicateID != null)
                {
                    lowest.CommunicateID = communicateID;
                }
                else
                {
                    AddErrorInfo(ref dtClone, dr);
                    continue;
                }

                lowest.Date = ServerTime.Time;
                lowest.Price = Convert.ToDecimal(dr["配件含税单价（元）"].ToString());
                lowest.TerminalPrice = Convert.ToDecimal(dr["配件终端最低价（元）"].ToString());
                lowest.Rater = BasicInfo.LoginID;
                lowest.Remark = dr["备注"].ToString();

                if (!m_LowestPriceServer.InsertAndUpdateData(lowest, out m_error))
                {
                    AddErrorInfo(ref dtClone, dr);
                    continue;
                }
            }

            if (dtClone.Rows.Count > 0)
            {
                FormShowYXLowestPriceError form = new FormShowYXLowestPriceError(dtTemp,null);
                form.ShowDialog();
            }
            else
            {
                MessageDialog.ShowPromptMessage("导入成功");
            }

            #region 逻辑代码修改 Modify by cjb on 2015.11.18
            //bool flag = false;
            ////string GoodsName = "";

            //for (int i = 0; i < dtTemp.Rows.Count; i++)
            //{
            //    YX_LowestMarketPrice lowest = new YX_LowestMarketPrice();

            //    lowest.Date = ServerTime.Time;

            //    int GoodsID = m_basicGoodsServer.GetGoodsIDByGoodsCode(
            //        dtTemp.Rows[i]["容大代码"].ToString(), dtTemp.Rows[i]["零件名称"].ToString(), dtTemp.Rows[i]["规格"].ToString());

            //    if (GoodsID != 0 && GoodsID != 7126)
            //    {
            //        lowest.GoodsID = GoodsID;
            //    }
            //    else if (GoodsID == 7126)
            //    {
            //        dtTemp.Rows.RemoveAt(i);
            //        i--;
            //        continue;
            //    }
            //    else
            //    {
            //        //GoodsName += "容大代码为:" + dtTemp.Rows[i]["容大代码"].ToString() + ";零件名称:" +
            //        //    dtTemp.Rows[i]["零件名称"].ToString() + "的零件有误,系统没有该零件;\r\n";
            //        flag = true;

            //        continue;
            //    }

            //    lowest.Price = Convert.ToDecimal(dtTemp.Rows[i]["配件含税单价（元）"].ToString());
            //    lowest.TerminalPrice = Convert.ToDecimal(dtTemp.Rows[i]["配件终端最低价（元）"].ToString());
            //    lowest.Rater = BasicInfo.LoginID;
            //    lowest.Remark = dtTemp.Rows[i]["备注"].ToString();
            //    string clientCode = m_clientServer.GetClientCode(dtTemp.Rows[i]["客户名称"].ToString());

            //    if (clientCode != "")
            //    {
            //        lowest.ClientID = clientCode;

            //        string communicateID = m_LowestPriceServer.GetCommunicateID(clientCode, dtTemp.Rows[i]["主机厂图号型号"].ToString(),
            //            dtTemp.Rows[i]["主机厂物品名称"].ToString(), GoodsID, out m_error);

            //        if (communicateID != "")
            //        {
            //            lowest.CommunicateID = communicateID;
            //        }
            //        else
            //        {
            //            flag = true;
            //            continue;
            //        }
            //    }
            //    else
            //    {
            //        flag = true;
            //        continue;
            //    }

            //    if (!m_LowestPriceServer.InsertAndUpdateData(lowest, out m_error))
            //    {
            //        flag = true;
            //    }
            //    else
            //    {
            //        dtTemp.Rows.RemoveAt(i);
            //        i--;
            //    }
            //}

            //if (flag)
            //{
            //    FormShowYXLowestPriceError form = new FormShowYXLowestPriceError(dtTemp,null);
            //    form.ShowDialog();
            //}
            //else
            //{
            //    MessageDialog.ShowPromptMessage("导入成功");
            //}

            #endregion
        }

        void AddErrorInfo(ref DataTable table, DataRow tempRow)
        {
            DataRow newRow = table.NewRow();

            foreach (DataColumn cl in table.Columns)
            {
                newRow[cl.ColumnName] = tempRow[cl.ColumnName];
            }

            table.Rows.Add(newRow);
        }

        /// <summary>
        /// 检查Excel表的数据
        /// </summary>
        /// <param name="dtcheck">表</param>
        /// <returns>返回是否正确</returns>
        bool CheckTable(DataTable dtcheck)
        {
            if (!dtcheck.Columns.Contains("容大代码"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【容大代码】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("零件名称"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【零件名称】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("配件终端最低价（元）"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【配件终端最低价（元）】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("规格"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【规格】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("客户名称"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【客户名称】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("配件含税单价（元）"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【配件含税单价（元）】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("备注"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【备注】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("主机厂图号型号"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【主机厂图号型号】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("主机厂物品名称"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【主机厂物品名称】信息");
                return false;
            }

            return true;
        }

        private void 主机厂与系统零件toolStripButton_Click(object sender, EventArgs e)
        {
            主机厂与系统零件匹配设置 form = new 主机厂与系统零件匹配设置();
            form.ShowDialog();
        }

        private void 刷新toolStripButton_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void tbsClientGoodsCode_OnCompleteSearch()
        {
            tbsClientGoodsCode.Text = tbsClientGoodsCode.DataResult["主机厂图号型号"].ToString();
            txtClientGoodsName.Text = tbsClientGoodsCode.DataResult["主机厂物品名称"].ToString();
            tbsClientGoodsCode.Tag = Convert.ToInt32(tbsClientGoodsCode.DataResult["ID"]);

            txtClient.Text = tbsClientGoodsCode.DataResult["主机厂"].ToString();
            txtClient.Tag = tbsClientGoodsCode.DataResult["主机厂编码"].ToString();
        }

        private void txtClient_OnCompleteSearch()
        {
            txtClient.Tag = txtClient.DataResult["客户编码"].ToString();
            txtClient.Text = txtClient.DataResult["客户名称"].ToString();
        }

        private void 最低销售定价_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void 导出toolStripButton_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }
    }
}
