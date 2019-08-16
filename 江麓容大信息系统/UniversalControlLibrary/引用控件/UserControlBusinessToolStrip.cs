using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UniversalControlLibrary
{
    public partial class UserControlBusinessToolStrip : UserControl
    {
        public event GlobalObject.DelegateCollection.NonArgumentHandle ButtonClick_Propose;

        public event GlobalObject.DelegateCollection.NonArgumentHandle ButtonClick_Audit;

        public event GlobalObject.DelegateCollection.NonArgumentHandle ButtonClick_Authorize;

        public event GlobalObject.DelegateCollection.NonArgumentHandle ButtonClick_Pass;

        public event GlobalObject.DelegateCollection.NonArgumentHandle ButtonClick_Print;

        public event GlobalObject.DelegateCollection.NonArgumentHandle ButtonClick_Refresh;

        public event GlobalObject.DelegateCollection.NonArgumentHandle ButtonClick_Affrim;

        public event GlobalObject.DelegateCollection.NonArgumentHandle ButtonClick_Reback;

        public event GlobalObject.DelegateCollection.NonArgumentHandle ButtonClick_Export;

        public void EnabledButton(string billNo)
        {
            //IFlowServer serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

            //string flowEnable = serverFlow.GetBillNoAuthority(this.ParentForm.Tag.ToString(), billNo);

            //if (flowEnable != null)
            //{
            //    for (int i = 0; i < toolStrip1.Items.Count; i++)
            //    {
            //        if (toolStrip1.Items[i] is ToolStripButton)
            //        {
            //            ToolStripButton tsb = (ToolStripButton)toolStrip1.Items[i];

            //            if (flowEnable == tsb.Tag.ToString())
            //            {
            //                tsb.Enabled = true;
            //            }
            //        }
            //    }
            //}
        }

        public void VisibleButton()
        {
            //IFlowServer serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

            //List<string> formAuthority = serverFlow.GetBusinessAllAuthority(this.ParentForm.Tag.ToString());

            //for (int i = 0; i < toolStrip1.Items.Count; i++)
            //{
            //    if (toolStrip1.Items[i] is ToolStripButton)
            //    {
            //        ToolStripButton tsb = (ToolStripButton)toolStrip1.Items[i];

            //        if (!formAuthority.Contains(tsb.Tag.ToString()))
            //        {
            //            tsb.Visible = false;
            //        }
            //    }
            //}
        }

        public UserControlBusinessToolStrip()
        {
            InitializeComponent();
            VisibleButton();
        }

        private void btnPropose_Click(object sender, EventArgs e)
        {
            if (ButtonClick_Propose != null)
            {
                ButtonClick_Propose();
            }
        }

        private void btnAudit_Click(object sender, EventArgs e)
        {
            if (ButtonClick_Audit != null)
            {
                ButtonClick_Audit();
            }
        }

        private void btnAuthorize_Click(object sender, EventArgs e)
        {
            if (ButtonClick_Authorize != null)
            {
                ButtonClick_Authorize();
            }
        }

        private void btnAffrim_Click(object sender, EventArgs e)
        {
            if (ButtonClick_Affrim != null)
            {
                ButtonClick_Affrim();
            }
        }

        private void btnReback_Click(object sender, EventArgs e)
        {
            if (ButtonClick_Reback != null)
            {
                ButtonClick_Reback();
            }
        }

        private void btnPass_Click(object sender, EventArgs e)
        {
            if (ButtonClick_Pass != null)
            {
                ButtonClick_Pass();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ButtonClick_Print != null)
            {
                ButtonClick_Print();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (ButtonClick_Refresh != null)
            {
                ButtonClick_Refresh();
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (ButtonClick_Export != null)
            {
                ButtonClick_Export();
            }
            else
            {
                Dictionary<string, DataGridView> tempDic = new Dictionary<string, DataGridView>();

                GetDataGridView(this.ParentForm, ref tempDic);

                if (tempDic.Count == 0)
                {
                    return;
                }
                else if (tempDic.Count == 1)
                {
                    OutPutExcel(tempDic.First().Value);
                }
                else
                {
                    FormDataRadio frm = new FormDataRadio(tempDic.Keys.ToList(), false);

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        OutPutExcel(tempDic[frm.m_strResult]);
                    }
                }
            }
        }

        void OutPutExcel(DataGridView dgrv)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dgrv);
        }

        void GetDataGridView(Control ctrl, ref Dictionary<string, DataGridView> dic)
        {
            foreach (Control item in ctrl.Controls)
            {
                if (item is DataGridView)
                {
                    dic.Add(((DataGridView)item).Name, (DataGridView)item);
                }

                GetDataGridView(item, ref dic);
            }
        }
    }
}
