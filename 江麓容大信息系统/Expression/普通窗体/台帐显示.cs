using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using UniversalControlLibrary;
using Service_Economic_Financial;

namespace Expression
{
    /// <summary>
    /// 台帐显示界面
    /// </summary>
    public partial class 台帐显示 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 财务服务组件
        /// </summary>
        IGatherBillAndDetailBillServer m_findEstrade = Service_Economic_Financial.ServerModuleFactory.GetServerModule<IGatherBillAndDetailBillServer>();

        public 台帐显示(int intGoodsID,DateTime dtStart,DateTime dtEnd)
        {
            InitializeComponent();

            try
            {
                DataTable dt = new DataTable();

                if (!m_findEstrade.GetAllGather("Estrade", intGoodsID, dtStart, dtEnd, "", "", out dt, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }

                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
                return;
            }
        }
    }
}
