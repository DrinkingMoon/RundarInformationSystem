using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;
using Expression.Properties;

namespace Expression
{
    public partial class 质量数据库主界面 : Form
    {
        string m_strError = "";

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 查找条件窗体
        /// </summary>
        FormConditionFind m_formFindCondition;

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 质量数据库服务接口
        /// </summary>
        IQualitySystemDatabase m_serverQylityDatabase = ServerModuleFactory.GetServerModule<IQualitySystemDatabase>();

        public 质量数据库主界面()
        {
            InitializeComponent();

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.质量数据库.ToString(), m_serverQylityDatabase);
            btnRefresh_Click(null,null);
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
                if ((string)dataGridView1.Rows[i].Cells["档案号"].Value == billNo)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string billNo = m_billNoControl.GetNewBillNo();

            质量数据库信息显示界面 frm = new 质量数据库信息显示界面(billNo);
            if (frm.ShowDialog() != DialogResult.OK)
            {
                m_billNoControl.CancelBill(billNo);
            }

            btnRefresh_Click(null, null);
            PositioningRecord(billNo);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = new BindingCollection<View_ZL_Database_Record>
                (m_serverQylityDatabase.GetListInfo(checkBillDateAndStatus1.dtpStartTime.Value, checkBillDateAndStatus1.dtpEndTime.Value));


            // 添加查询用的列
            if (m_findField == null)
            {
                List<string> lstColumnName = new List<string>();

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (dataGridView1.Columns[i].Visible)
                    {
                        lstColumnName.Add(dataGridView1.Columns[i].Name);
                    }
                }

                m_findField = lstColumnName.ToArray();
            }

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            btnRefresh_Click(null, null);
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            btnFind_Click(null, null);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            质量数据库信息显示界面 frm = new 质量数据库信息显示界面(dataGridView1.CurrentRow.Cells["档案号"].Value.ToString());
            frm.ShowDialog();

            btnRefresh_Click(null, null);
            PositioningRecord(dataGridView1.CurrentRow.Cells["档案号"].Value.ToString());
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string billNo = dataGridView1.CurrentRow.Cells["档案号"].Value.ToString();

            if (MessageDialog.ShowEnquiryMessage("您确定要删除档案号为【"+ billNo +"】的质量数据库档案?") == DialogResult.Yes)
            {
                m_serverQylityDatabase.DeleteInfo(billNo);
                MessageDialog.ShowPromptMessage("删除成功");
            }

            btnRefresh_Click(null, null);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (m_formFindCondition == null)
            {
                m_formFindCondition = new FormConditionFind(this, m_findField, labelTitle.Text, labelTitle.Text);
            }

            m_formFindCondition.ShowDialog();
        }

        /// <summary>
        /// 检测数据集
        /// </summary>
        /// <param name="checkList">需要检测的数据集</param>
        /// <returns>通过返回True，不通过返回False</returns>
        bool CheckTable(DataTable checkList)
        {
            if (!checkList.Columns.Contains("序列号"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【序列号】信息");
                return false;
            }

            if (!checkList.Columns.Contains("创建时间"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【创建时间】信息");
                return false;
            }

            if (!checkList.Columns.Contains("不良品类型"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【不良品类型】信息");
                return false;
            }

            if (!checkList.Columns.Contains("型号"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【型号】信息");
                return false;
            }

            if (!checkList.Columns.Contains("总成箱号"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【总成箱号】信息");
                return false;
            }

            if (!checkList.Columns.Contains("发生时间"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【发生时间】信息");
                return false;
            }

            if (!checkList.Columns.Contains("发现场所"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【发现场所】信息");
                return false;
            }

            if (!checkList.Columns.Contains("发现者"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【发现者】信息");
                return false;
            }

            if (!checkList.Columns.Contains("图号型号"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【图号型号】信息");
                return false;
            }

            if (!checkList.Columns.Contains("物品名称"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【物品名称】信息");
                return false;
            }

            if (!checkList.Columns.Contains("规格"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【规格】信息");
                return false;
            }

            if (!checkList.Columns.Contains("版次"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【版次】信息");
                return false;
            }

            if (!checkList.Columns.Contains("供应商"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【供应商】信息");
                return false;
            }

            if (!checkList.Columns.Contains("故障类型"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【故障类型】信息");
                return false;
            }

            if (!checkList.Columns.Contains("故障描述"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【故障描述】信息");
                return false;
            }

            if (!checkList.Columns.Contains("行驶里程"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【行驶里程】信息");
                return false;
            }

            if (!checkList.Columns.Contains("故障比例"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【故障比例】信息");
                return false;
            }

            if (!checkList.Columns.Contains("原因分析"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【原因分析】信息");
                return false;
            }

            if (!checkList.Columns.Contains("处理对策"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【处理对策】信息");
                return false;
            }

            if (!checkList.Columns.Contains("发现人员"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【发现人员】信息");
                return false;
            }

            return true;
        }

        private void txtInport_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                CursorControl.SetWaitCursor(this);
                DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1.OpenFile());

                if (dtTemp == null)
                {
                    MessageDialog.ShowPromptMessage(m_strError);
                    return;
                }

                if (CheckTable(dtTemp))
                {
                    List<ZL_Database_Record> listRecord = new List<ZL_Database_Record>();

                    foreach (DataRow dr in dtTemp.Rows)
                    {
                        if (dr["序列号"].ToString().Trim().Length == 0)
                        {
                            continue;
                        }

                        ZL_Database_Record m_lnqRecord = new ZL_Database_Record();

                        m_lnqRecord.BillNo = openFileDialog1.FileName.Substring(0, openFileDialog1.FileName.LastIndexOf("\\") + 1) + dr["序列号"].ToString().Trim();
                        m_lnqRecord.CreationTime = ServerTime.Time;
                        m_lnqRecord.AssemblyCartonNo = dr["总成箱号"].ToString().Trim();
                        m_lnqRecord.CauseAnalysis = dr["原因分析"].ToString().Trim();
                        m_lnqRecord.FaultRatio = dr["故障比例"].ToString().Trim();
                        m_lnqRecord.Provider = dr["供应商"].ToString().Trim();
                        m_lnqRecord.FaultDescription = dr["故障描述"].ToString().Trim();
                        m_lnqRecord.FaultType = dr["故障类型"].ToString().Trim();
                        m_lnqRecord.Finder = dr["发现人员"].ToString().Trim();
                        m_lnqRecord.FindPlaces = dr["发现场所"].ToString().Trim();
                        m_lnqRecord.FindRole = dr["发现者"].ToString().Trim();
                        m_lnqRecord.GoodsCode = dr["图号型号"].ToString().Trim();
                        m_lnqRecord.GoodsName = dr["物品名称"].ToString().Trim();
                        m_lnqRecord.Spec = dr["规格"].ToString().Trim();
                        m_lnqRecord.Mileage = dr["行驶里程"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(dr["行驶里程"].ToString().Trim());
                        m_lnqRecord.Model = dr["型号"].ToString().Trim();
                        m_lnqRecord.OccurrenceTime = dr["发生时间"].ToString().Trim() == ""? null : 
                            (DateTime?)Convert.ToDateTime(dr["发生时间"].ToString().Trim());
                        m_lnqRecord.TreatmentCountermeasures = dr["处理对策"].ToString().Trim();
                        m_lnqRecord.Type = dr["不良品类型"].ToString().Trim();
                        m_lnqRecord.Version = dr["版次"].ToString().Trim();

                        listRecord.Add(m_lnqRecord);
                    }

                    m_serverQylityDatabase.BatchInportInfo(listRecord);
                    MessageDialog.ShowPromptMessage("导入成功");
                }

                this.Cursor = System.Windows.Forms.Cursors.Arrow;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
                this.Cursor = System.Windows.Forms.Cursors.Arrow;
            }


            btnRefresh_Click(null, null);
        }
    }
}
