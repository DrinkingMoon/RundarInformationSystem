using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 批量设置变速箱号的填写界面
    /// </summary>
    public partial class 填写变速箱编号 : Form
    {
        /// <summary>
        /// 营销产品服务组件
        /// </summary>
        IBomServer m_serviceBom = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 型号变更服务组件
        /// </summary>
        IProductChange m_product = ServerModuleFactory.GetServerModule<IProductChange>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 变速箱编号
        /// </summary>
        private string m_strCVTID;

        public string StrCVTID
        {
            get { return m_strCVTID; }
            set { m_strCVTID = value; }
        }

        public 填写变速箱编号()
        {
            InitializeComponent();
            cmbProductType.DataSource = m_serviceBom.GetAssemblyTypeList();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!ServerModuleFactory.GetServerModule<IProductCodeServer>().VerifyProductCodesInfo(
                cmbProductType.Text, txtCvtID.Text, GlobalObject.CE_BarCodeType.内部钢印码, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return;
            }

            string str = txtCvtID.Text;
            
            m_strCVTID = cmbProductType.Text.Trim() + " " + str.ToUpper();
            
            this.Close();
        }
    }
}
