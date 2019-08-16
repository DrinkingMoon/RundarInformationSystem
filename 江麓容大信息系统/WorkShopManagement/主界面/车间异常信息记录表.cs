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
using System.Reflection;
using UniversalControlLibrary;
using ServerModule;

namespace Form_Manufacture_WorkShop
{
    public partial class 车间异常信息记录表 : Form
    {
        /// <summary>
        /// 车间异常处理服务组件
        /// </summary>
        Service_Manufacture_WorkShop.IMaterialsTransferException m_serverException =
            Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<IMaterialsTransferException>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strError = "";

        /// <summary>
        /// 查找条件字段列表
        /// </summary>
        List<string> m_lstFindField = new List<string>();

        /// <summary>
        /// 查询信息集合
        /// </summary>
        DataTable m_dtSource = new DataTable();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        public 车间异常信息记录表()
        {
            InitializeComponent();

            checkBillDateAndStatus1.InsertComBox(typeof(MaterialsTransferExceptionStatus));
            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.车间异常信息记录表.ToString(), m_serverException);
            m_billMessageServer.BillType = CE_BillTypeEnum.车间异常信息记录表.ToString();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefrshData()
        {
            m_dtSource = m_serverException.GetBillInfo();

            DataColumnCollection columns = m_dtSource.Columns;

            if (columns.Count > 0)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    m_lstFindField.Add(columns[i].ColumnName);
                }
            }

            dataGridView1.DataSource = m_dtSource;

            userControlDataLocalizer1.Init(this.dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, this.dataGridView1.Name, BasicInfo.LoginID));
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="msg">定位信息</param>
        void PositioningRecord(string msg)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((string)dataGridView1.Rows[i].Cells["单据号"].Value == msg)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells["单据号"];
                }
            }
        }

        /// <summary>
        /// 重新窗体消息处理函数
        /// </summary>
        /// <param name="m">窗体消息</param>
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                //接收自定义消息,放弃未提交的单据号
                case WndMsgSender.CloseMsg:
                    // 放弃未使用的单据号
                    m_billNoControl.CancelBill();
                    break;

                case WndMsgSender.PositioningMsg:
                    WndMsgData msg = new WndMsgData();      //这是创建自定义信息的结构
                    Type dataType = msg.GetType();

                    msg = (WndMsgData)m.GetLParam(dataType);//这里获取的就是作为LParam参数发送来的信息的结构

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, CE_BillTypeEnum.车间异常信息记录表.ToString());

                    if (dtMessage.Rows.Count == 0)
                    {
                        m_billMessageServer.DestroyMessage(msg.MessageContent);
                        MessageDialog.ShowPromptMessage("未找到相关记录");
                    }
                    else
                    {
                        dataGridView1.DataSource = dtMessage;
                        dataGridView1.Rows[0].Selected = true;
                    }

                    break;

                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            FormFindCondition formFindCondition = new FormFindCondition(m_lstFindField.ToArray());

            if (formFindCondition.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            DataTable dtTemp = GlobalObject.DataSetHelper.SiftDataTable(m_dtSource,
                formFindCondition.SearchSQL, out m_strError);

            if (dtTemp == null)
            {
                MessageDialog.ShowPromptMessage(m_strError);
            }
            else
            {
                dataGridView1.DataSource = dtTemp;
            }
        }

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            RefrshData();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            RefrshData();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void 车间异常信息记录表_Load(object sender, EventArgs e)
        {
            RefrshData();

            dataGridView1.Columns["单据号"].Width = 120;
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            string billNo = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();

            车间异常明细信息处理 frm = new 车间异常明细信息处理(billNo, false);
            frm.ShowDialog();
            RefrshData();
            PositioningRecord(billNo);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            string billNo = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();

            if (MessageDialog.ShowEnquiryMessage("是否要删除【" + billNo + "】号单据?")
                == DialogResult.Yes)
            {
                if (m_serverException.DeleteBill(billNo, out m_strError))
                {
                    MessageDialog.ShowPromptMessage("删除成功");
                    m_billMessageServer.DestroyMessage(billNo);
                    m_billNoControl.CancelBill(billNo);
                    RefrshData();
                }
                else
                {
                    MessageDialog.ShowPromptMessage("删除失败：" + m_strError);
                }
            }
        }

        private void btnDispose_Click(object sender, EventArgs e)
        {
            string billNo = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();

            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == MaterialsTransferExceptionStatus.已处理.ToString())
            {
                MessageDialog.ShowPromptMessage("此异常信息已处理");
                return;
            }
            else
            {
                if (!m_serverException.InsertExceptionInfo(billNo, out m_strError))
                {
                    MessageDialog.ShowPromptMessage(m_strError);
                    return;
                }
            }

            车间异常明细信息处理 frm = new 车间异常明细信息处理(billNo, true);
            frm.ShowDialog();
            RefrshData();
            PositioningRecord(billNo);
        }
    }
}
