using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WebServerModule2;
using ServerModule;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 返回件与更新件确认界面
    /// </summary>
    public partial class FormReplaceAccessory : Form
    {
        /// <summary>
        /// CVT客户信息服务组件
        /// </summary>
        ICVTCustomerInformationServer m_serverCVTCustomerInfo = ServerModuleFactory.GetServerModule<ICVTCustomerInformationServer>();

        /// <summary>
        /// 服务类
        /// </summary>
        IServiceFeedBack2 m_serverFeedBack = WebServerModule2.ServerModuleFactory2.GetServerModule<IServiceFeedBack2>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 基础物品服务
        /// </summary>
        IBasicGoodsServer m_basicGoodsServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 数据集
        /// </summary>
        DataTable m_dtList = new DataTable();

        /// <summary>
        /// 信息是否修改
        /// </summary>
        private bool m_blIsUpdate = false;

        public bool BlIsUpdate
        {
            get { return m_blIsUpdate; }
            set { m_blIsUpdate = value; }
        }

        /// <summary>
        /// 关联单据编号
        /// </summary>
        string m_strServiceID;

        /// <summary>
        /// 返回件物品编号
        /// </summary>
        private int m_intOldGoodsID;

        public int IntOldGoodsID
        {
            get { return m_intOldGoodsID; }
            set { m_intOldGoodsID = value; }
        }

        /// <summary>
        /// 更新件物品编号
        /// </summary>
        private int m_intNewGoodsID;

        public int IntNewGoodsID
        {
            get { return m_intNewGoodsID; }
            set { m_intNewGoodsID = value; }
        }

        /// <summary>
        /// 返回件总成编号
        /// </summary>
        private string m_strBackCvtID;

        public string StrBackCvtID
        {
            get { return m_strBackCvtID; }
            set { m_strBackCvtID = value; }
        }

        /// <summary>
        /// 更新件总成编号
        /// </summary>
        private string m_NewGoodsID = "";

        public string NewGoodsID
        {
            get { return m_NewGoodsID; }
            set { m_NewGoodsID = value; }
        }

        /// <summary>
        /// 更新件图号
        /// </summary>
        private string m_NewGoodsCode;

        public string NewGoodsCode1
        {
            get { return m_NewGoodsCode; }
            set { m_NewGoodsCode = value; }
        }

        /// <summary>
        /// 判断是否为总成
        /// </summary>
        private bool m_flag;

        public bool Flag
        {
            get { return m_flag; }
            set { m_flag = value; }
        }

        /// <summary>
        /// 更新件TCU物品ID
        /// </summary>
        private string m_newGoodsID;

        public string NewGoodsID1
        {
            get { return m_newGoodsID; }
            set { m_newGoodsID = value; }
        }

        /// <summary>
        /// 产品信息服务组件
        /// </summary>
        IProductCodeServer m_serverProductCode = ServerModule.ServerModuleFactory.GetServerModule<IProductCodeServer>();

        public FormReplaceAccessory(string FeedBackBill_ID)
        {
            InitializeComponent();
            
            m_strServiceID = FeedBackBill_ID;
            m_dtList = m_serverFeedBack.GetReplaceByID(m_strServiceID);

            dataGridView1.DataSource = m_dtList;
            dataGridView1.Columns["Remark"].Visible = false;
            dataGridView1.Columns["ID"].Visible = false;
            
        }

        /// <summary>
        /// 创建样式
        /// </summary>
        private void CreateDateTableStyle()
        {
            m_dtList.Columns.Add("ServiceID");
            m_dtList.Columns.Add("OldGoodsName");
            m_dtList.Columns.Add("OldGoodsCode");
            m_dtList.Columns.Add("OldSpec");
            m_dtList.Columns.Add("OldGoodsID");
            m_dtList.Columns.Add("OldCvtID");
            m_dtList.Columns.Add("BackTime");
            m_dtList.Columns.Add("NewGoodsName");
            m_dtList.Columns.Add("NewGoodsCode");
            m_dtList.Columns.Add("NewSpec");
            m_dtList.Columns.Add("NewGoodsID");
            m_dtList.Columns.Add("NewCvtID");
            m_dtList.Columns.Add("Count");
            m_dtList.Columns.Add("Unit");
            m_dtList.Columns.Add("Remark");
            m_dtList.Columns.Add("GiveOutDate");
        }

        /// <summary>
        /// 点击确认，把信息添加到DataTable中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtOldCode.Text.Trim() == "" && txtGoodsSpec.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择返回件及返回日期！");
                return;
            }

            if ((txtOldCvtID.Text != "" && txtOldCvtID.Text.Trim().Length < 9) 
                || (txtNewCvtID.Text != "" && txtNewCvtID.Text.Trim().Length < 9))
            {
                MessageDialog.ShowPromptMessage("请填写正确的总成编号！");
                return;
            }

            if (dtpGiveOutDate.Checked == false)
            {
                MessageDialog.ShowPromptMessage("请填写旧件发出日期！");
                return;
            }

            if (dtpGiveOutDate.Value > dtpBackTime.Value)
            {
                MessageDialog.ShowPromptMessage("旧件发出日期不能大于旧件返回日期！");
                return;
            }

            if (txtOldCode.Text.Trim() != "")
            {
                if (UniversalFunction.IsProduct(Convert.ToInt32(txtOldCode.Tag.ToString())))
                {
                    m_flag = true;

                    if (!m_serverProductCode.VerifyProductCodesInfo(Convert.ToInt32(txtOldCode.Tag.ToString()), 
                        txtOldGoodsID.Text.Trim(), GlobalObject.CE_BarCodeType.内部钢印码, out m_strErr))
                    {
                        MessageDialog.ShowPromptMessage(m_strErr);
                        return;
                    }

                    IProductListServer serverProductList = ServerModule.ServerModuleFactory.GetServerModule<IProductListServer>();

                    if (Convert.ToBoolean(UniversalFunction.GetGoodsAttributeInfo(Convert.ToInt32(txtOldCode.Tag.ToString()), 
                        GlobalObject.CE_GoodsAttributeName.CVT)))
                    {
                        m_NewGoodsCode = txtOldCode.Text;
                        m_NewGoodsID = txtOldCvtID.Text;

                        if (txtOldGoodsID.Text.Trim() != txtOldCvtID.Text.Trim())
                        {
                            MessageDialog.ShowPromptMessage("当返回件是总成时，返回件编号和总成编号必须一致");
                            return;
                        }
                    }
                }
            }

            if (txtNewCode.Text.Trim() != "")
            {
                if (UniversalFunction.IsProduct(Convert.ToInt32(txtNewCode.Tag.ToString())))
                {
                    if (!m_serverProductCode.VerifyProductCodesInfo(Convert.ToInt32(txtNewCode.Tag.ToString()), 
                        txtNewGoodsID.Text.Trim(), GlobalObject.CE_BarCodeType.内部钢印码, out m_strErr))
                    {
                        MessageDialog.ShowPromptMessage(m_strErr);
                        return;
                    }

                    IProductListServer serverProductList = ServerModule.ServerModuleFactory.GetServerModule<IProductListServer>();

                    if (Convert.ToBoolean(UniversalFunction.GetGoodsAttributeInfo(Convert.ToInt32(txtNewCode.Tag), 
                        GlobalObject.CE_GoodsAttributeName.CVT)))
                    {
                        if (txtNewCvtID.Text.Trim() != txtNewGoodsID.Text.Trim())
                        {
                            MessageDialog.ShowPromptMessage("当更新件是总成时，更新件编号和总成编号必须一致");
                            return;
                        }
                    }
                    else
                    {
                        m_newGoodsID = txtNewGoodsID.Text;
                    }
                }
            }

            if (CheckSameGoods())
            {
                DataRow dr = m_dtList.NewRow();
                dr["ServiceID"] = m_strServiceID;
                dr["OldGoodsName"] = txtGoodsName.Text;
                dr["OldGoodsCode"] = txtOldCode.Text;
                dr["OldSpec"] = txtGoodsSpec.Text;
                dr["OldCvtID"] = txtOldCvtID.Text;
                dr["BackTime"] = dtpBackTime.Value;
                dr["NewGoodsName"] = textBox2.Text;
                dr["NewGoodsCode"] = txtNewCode.Text;
                dr["NewSpec"] = textBox1.Text;
                dr["NewCvtID"] = txtNewCvtID.Text;
                dr["NewGoodsID"] = txtNewGoodsID.Text;
                dr["OldGoodsID"] = txtOldGoodsID.Text;
                dr["Count"] = txtCount.Value.ToString();
                dr["Unit"] = txtUnit.Text;
                dr["GiveOutDate"] = dtpGiveOutDate.Value;

                m_dtList.Rows.Add(dr);

                m_strBackCvtID = dr["OldCvtID"].ToString();
                dataGridView1.DataSource = m_dtList;
                txtOldCode.Text = "";
                txtGoodsName.Text = "";
                txtGoodsSpec.Text = "";
                txtOldCvtID.Text = "";
                txtNewCvtID.Text = "";
                txtNewCode.Text = "";
                txtCount.Text = "0";
                textBox1.Text = "";
                textBox2.Text = "";
                txtUnit.Text = "";
            }
        }

        /// <summary>
        /// 检查同种物品
        /// </summary>
        /// <param name="intGoodsID"></param>
        /// <returns></returns>
        public bool CheckSameGoods()
        {
            if (m_dtList == null || m_dtList.Rows.Count == 0)
            {
                return true;
            }
            m_dtList = (DataTable)dataGridView1.DataSource;

            for (int i = 0; i < m_dtList.Rows.Count; i++)
            {

                if (m_dtList.Rows[i]["OldGoodsName"].ToString().Trim() == txtGoodsName.Text.Trim()
                    && m_dtList.Rows[i]["OldGoodsCode"].ToString().Trim() == txtOldCode.Text.Trim()
                    && m_dtList.Rows[i]["OldSpec"].ToString().Trim() == txtGoodsSpec.Text.Trim())
                {
                    m_dtList.Rows.RemoveAt(i);
                }
            }
            return true;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtGoodsName.Text = dataGridView1.CurrentRow.Cells["OldGoodsName"].Value.ToString();
            txtGoodsSpec.Text = dataGridView1.CurrentRow.Cells["OldSpec"].Value.ToString();
            txtOldCode.Text = dataGridView1.CurrentRow.Cells["OldGoodsCode"].Value.ToString();
            txtOldCode.Tag = m_basicGoodsServer.GetGoodsIDByGoodsCode(txtOldCode.Text, txtGoodsName.Text, txtGoodsSpec.Text).ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells["NewSpec"].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells["NewGoodsName"].Value.ToString();
            txtNewCode.Text = dataGridView1.CurrentRow.Cells["NewGoodsCode"].Value.ToString();
            txtNewCode.Tag = m_basicGoodsServer.GetGoodsIDByGoodsCode(txtNewCode.Text, textBox2.Text, textBox1.Text).ToString();
            txtOldCvtID.Text = dataGridView1.CurrentRow.Cells["OldCvtID"].Value.ToString();
            txtNewCvtID.Text = dataGridView1.CurrentRow.Cells["NewCvtID"].Value.ToString();
            txtNewGoodsID.Text = dataGridView1.CurrentRow.Cells["NewGoods"].Value.ToString();
            txtOldGoodsID.Text = dataGridView1.CurrentRow.Cells["OldGoods"].Value.ToString();

            if (dataGridView1.CurrentRow.Cells["Count"].Value.ToString().Trim() == ""
                || dataGridView1.CurrentRow.Cells["Count"].Value.ToString() == null)
            {
                txtCount.Value = 0;
            }
            else
                txtCount.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["Count"].Value.ToString());

            txtUnit.Text = dataGridView1.CurrentRow.Cells["Unit"].Value.ToString();

            m_intOldGoodsID = m_basicGoodsServer.GetGoodsID(txtOldCode.Text, txtGoodsName.Text, txtOldCode.Text);
        }

        private void txtNewCode_OnCompleteSearch()
        {
            txtNewCode.Text = txtNewCode.DataResult["图号型号"].ToString();
            txtNewCode.Tag = txtNewCode.DataResult["序号"].ToString();
            textBox1.Text = txtNewCode.DataResult["规格"].ToString();
            textBox2.Text = txtNewCode.DataResult["物品名称"].ToString();
            txtUnit.Text = txtNewCode.DataResult["单位"].ToString();
            m_intNewGoodsID = Convert.ToInt32(txtNewCode.DataResult["序号"].ToString());
        }

        private void txtOldCode_OnCompleteSearch()
        {
            txtOldCode.Text = txtOldCode.DataResult["图号型号"].ToString();
            txtOldCode.Tag = txtOldCode.DataResult["序号"].ToString();
            txtGoodsName.Text = txtOldCode.DataResult["物品名称"].ToString();
            txtGoodsSpec.Text = txtOldCode.DataResult["规格"].ToString();
            txtUnit.Text = txtOldCode.DataResult["单位"].ToString();
            m_intOldGoodsID = Convert.ToInt32(txtOldCode.DataResult["序号"].ToString());
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            m_dtList = (DataTable)dataGridView1.DataSource;

            if (m_dtList.Rows.Count != 0)
            {
                m_dtList.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                dataGridView1.DataSource = m_dtList;
                m_intOldGoodsID = 0;
                txtOldCode.Text = "";
                txtGoodsName.Text = "";
                txtGoodsSpec.Text = "";
                txtOldCvtID.Text = "";
                txtNewCvtID.Text = "";
                txtNewCode.Text = "";
                txtCount.Text = "0";
                textBox1.Text = "";
                textBox2.Text = "";
                txtUnit.Text = "";
                txtOldGoodsID.Text = "";
                txtNewGoodsID.Text = "";
            }
        }

        /// <summary>
        /// 保存，提交数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dtNew = (DataTable)dataGridView1.DataSource;

            if (dtNew.Rows.Count > 0)
            {
                DataRow drServiceFeedBack = m_serverFeedBack.GetServiceFeedBackBill(m_strServiceID);

                if (drServiceFeedBack == null)
                {
                    MessageDialog.ShowPromptMessage("反馈单号不唯一或者不存在");
                }

                bool b = m_serverFeedBack.InsertReplace(dtNew, m_strServiceID, out m_strErr);

                if (b)
                {
                    if (m_serverCVTCustomerInfo.InsertCustomerHistoryInfo(m_strServiceID, drServiceFeedBack["ChassisNum"].ToString(),
                        drServiceFeedBack["CVTCode"].ToString(),
                        drServiceFeedBack["CarModel"].ToString(),
                        drServiceFeedBack["UserName"].ToString(),
                        drServiceFeedBack["SiteName"].ToString(),
                        dtNew, out m_strErr))
                    {
                        MessageDialog.ShowPromptMessage("返回件保存成功！");

                        m_dtList = m_serverFeedBack.GetReplaceByID(m_strServiceID);
                        dataGridView1.DataSource = m_dtList;
                        dataGridView1.Columns["Remark"].Visible = false;
                        dataGridView1.Columns["ID"].Visible = false;
                        m_blIsUpdate = true;
                    }
                }
            }
            else
            {
                if (!m_serverFeedBack.DeleteReplace(m_strServiceID, out m_strErr))
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                }

                m_blIsUpdate = true;
            }
            
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
