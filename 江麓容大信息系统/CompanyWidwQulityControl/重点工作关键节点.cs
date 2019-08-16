using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniversalControlLibrary;
using ServerModule;
using GlobalObject;
using Service_Peripheral_CompanyQuality;
using FlowControlService;

namespace Form_Peripheral_CompanyQuality
{
    public partial class 重点工作关键节点 : Form
    {
        public List<Bus_FocalWork_MonthlyProgress_KeyPoint> _List_KeyPoint = new List<Bus_FocalWork_MonthlyProgress_KeyPoint>();

        DataTable _Table_Source = new DataTable();

        /// <summary>
        /// 服务组件
        /// </summary>
        IFocalWork _Service_FocalWork = Service_Peripheral_CompanyQuality.ServerModuleFactory.GetServerModule<IFocalWork>();

        public 重点工作关键节点(string keyValue)
        {
            InitializeComponent();
            _Table_Source = _Service_FocalWork.GetTable_KeyPoint(keyValue);
        }

        public 重点工作关键节点(List<Bus_FocalWork_MonthlyProgress_KeyPoint> lstKeyPoint)
        {
            InitializeComponent();
            _List_KeyPoint = lstKeyPoint;
        }

        private void 重点工作关键节点_Load(object sender, EventArgs e)
        {
            if (_Table_Source != null && _Table_Source.Rows.Count > 0)
            {
                customDataGridView1.DataSource = _Table_Source;
                customDataGridView1.ReadOnly = true;
            }
            else if (_List_KeyPoint != null && _List_KeyPoint.Count() > 0)
            {
                this.状态.Items.Remove("待启动");
                this.状态.Items.Remove("进行中");

                foreach (Bus_FocalWork_MonthlyProgress_KeyPoint item in _List_KeyPoint)
                {
                    Bus_FocalWork_KeyPoint keyPoint = _Service_FocalWork.GetSingle_KeyPoint(item.KeyPointId);

                    View_HR_Personnel personnel = UniversalFunction.GetPersonnelInfo(keyPoint.DutyUser);

                    customDataGridView1.Rows.Add(new object[] { keyPoint.KeyPointName, item.Evaluate, keyPoint.StartDate, 
                        keyPoint.EndDate, personnel.姓名, item.F_Id});
                }
            }
        }

        private void 重点工作关键节点_FormClosing(object sender, FormClosingEventArgs e)
        {
            customDataGridView1.CurrentCell = null;

            if (_List_KeyPoint != null && _List_KeyPoint.Count() > 0)
            {
                foreach (Bus_FocalWork_MonthlyProgress_KeyPoint item in _List_KeyPoint)
                {
                    foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                    {
                        if (item.F_Id == dgvr.Cells["F_Id"].Value.ToString())
                        {
                            item.Evaluate = dgvr.Cells["状态"].Value == null ? null : dgvr.Cells["状态"].Value.ToString();
                            break;
                        }
                    }
                }
            }
        }

        private void customDataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dgvr in (sender as CustomDataGridView).Rows)
            {
                if (dgvr.Cells["状态"].Value.ToString() == "未完成")
                {
                    dgvr.DefaultCellStyle.BackColor = Color.Red;
                }
                else if (dgvr.Cells["状态"].Value.ToString() == "延期")
                {
                    dgvr.DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
        }
    }
}
