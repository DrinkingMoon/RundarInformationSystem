using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;
namespace Expression
{
    /// <summary>
    /// CVT出厂检验记录界面
    /// </summary>
    public partial class CVT出厂检验记录 : Form
    {
        /// <summary>
        /// 服务组件
        /// </summary>
        IProductInfoServer m_productInfoServer = ServerModuleFactory.GetServerModule<IProductInfoServer>();

        /// <summary>
        /// 营销基础信息服务组件
        /// </summary>
        IProductDeliveryInspectionServer m_serverDeliveryInSpection = ServerModuleFactory.GetServerModule<IProductDeliveryInspectionServer>();

        /// <summary>
        /// 数据集
        /// </summary>
        P_DeliveryInspection m_lnqDelivery = new P_DeliveryInspection();

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        public CVT出厂检验记录(string strbillid,  AuthorityFlag authorFlag)
        {
            InitializeComponent();

            txtBill_ID.Text = strbillid;

            m_authFlag = authorFlag;

            P_DeliveryInspection lnqTemp = m_serverDeliveryInSpection.GetBill(strbillid);

            if (lnqTemp != null)
            {
                m_lnqDelivery = lnqTemp;
                ShowMessage();
            }
            else
            {
                //if (lnqTemp.Date <= Convert.ToDateTime("2013-10-29"))
                //{
                //    //原有的格式
                //    dataGridView1.DataSource = m_serverDeliveryInSpection.GetEmptyTable();
                //}
                //else
                //{
                //    //修改后的格式
                //    dataGridView1.DataSource = m_serverDeliveryInSpection.GetEmptyTable(lnqTemp.ProductType);
                //}

                dataGridView1.DataSource = m_serverDeliveryInSpection.GetEmptyTable(lnqTemp.ProductType);
                lbPersonnel.Text = BasicInfo.LoginName;
            }
        }

        private void CVT出厂检验记录_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);
            FaceAuthoritySetting.SetVisibly(this.toolStrip1, m_authFlag);
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        void ShowMessage()
        {
            txtProductType.Text = m_lnqDelivery.ProductType;
            txtProductCode.Text = m_lnqDelivery.ProductCode;

            if (m_lnqDelivery.Verdict != null 
                && m_lnqDelivery.Verdict != "" )
            {
                if (m_lnqDelivery.Verdict == rbYes.Text)
                {
                    rbYes.Checked = true;
                }
                else
                {
                    rbNo.Checked = true;
                }
            }

            if (m_lnqDelivery.FinalVerdict != null 
                && m_lnqDelivery.FinalVerdict != "" )
            {
                if (m_lnqDelivery.FinalVerdict == rbFinalYes.Text)
                {
                    rbFinalYes.Checked = true;
                }
                else
                {
                    rbFinalNo.Checked = true;
                }
            }

            lbFinalPersonnel.Text = m_lnqDelivery.FinalPersonnel;
            dtpFinalDate.Value =m_lnqDelivery.FinalDate == null? ServerTime.Time: Convert.ToDateTime( m_lnqDelivery.FinalDate);
            txtRemark.Text = m_lnqDelivery.Remark;
            lbPersonnel.Text = m_lnqDelivery.Surveyor;
            dtpCheck.Value = Convert.ToDateTime(m_lnqDelivery.Date);

            dataGridView1.DataSource = m_serverDeliveryInSpection.GetListInfo(txtBill_ID.Text);
        }

        private void btnOutExcel_Click(object sender, EventArgs e)
        {
            IBillReportInfo report = new 报表_CVT出厂检验记录(txtBill_ID.Text, "");
            PrintReportBill print = new PrintReportBill(19, 29.7, report);
            (report as 报表_CVT出厂检验记录).ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
