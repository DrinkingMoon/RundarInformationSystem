using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GlobalObject;
using PlatformManagement;
using Service_Manufacture_WorkShop;
using Expression;
using UniversalControlLibrary;
using ServerModule;

namespace Form_Manufacture_WorkShop
{
    public partial class 批量修改 : Form
    {
        private CE_SubsidiaryOperationType m_enumOperationType;

        public CE_SubsidiaryOperationType EnumOperationType
        {
            get { return m_enumOperationType; }
            set { m_enumOperationType = value; }
        }

        private decimal m_decRatio;

        public decimal DecRatio
        {
            get { return m_decRatio; }
            set { m_decRatio = value; }
        }

        public 批量修改()
        {
            InitializeComponent();

            numBefore.Value = 0;
            numAfter.Value = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            m_enumOperationType = radioButton1.Checked ? CE_SubsidiaryOperationType.物料转换前 : CE_SubsidiaryOperationType.物料转换后;

            if (numAfter.Value == 0 || numBefore.Value == 0)
            {
                MessageDialog.ShowPromptMessage("修改前数量或者修改后数量不能为0");
                return;
            }

            m_decRatio = numAfter.Value / numBefore.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
