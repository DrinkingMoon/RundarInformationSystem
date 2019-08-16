using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using UniversalControlLibrary;
using GlobalObject;

namespace Expression
{
    public partial class 还货单 : Form
    {
        /// <summary>
        /// 还货服务组件
        /// </summary>
        IProductReturnService m_serverReturn = ServerModuleFactory.GetServerModule<IProductReturnService>();

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

        public 还货单()
        {
            InitializeComponent();

            checkBillDateAndStatus1.InsertComBox(typeof(ProductLendReturnBillStatus));
            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.还货单.ToString(), m_serverReturn);
            m_billMessageServer.BillType = CE_BillTypeEnum.还货单.ToString();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefrshData()
        {
            m_dtSource = m_serverReturn.GetBillInfo();

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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, CE_BillTypeEnum.还货单.ToString());

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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefrshData();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
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
                if (m_serverReturn.DeleteBill(billNo, out m_strError))
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

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            string billNo = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();

            还货单明细 frm = new 还货单明细(billNo);
            frm.ShowDialog();
            RefrshData();
            PositioningRecord(billNo);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            还货单明细 frm = new 还货单明细(null);
            frm.ShowDialog();
            RefrshData();
            PositioningRecord(frm.StrBillNo);
        }

        private void 还货单_Load(object sender, EventArgs e)
        {
            RefrshData();

            dataGridView1.Columns["单据号"].Width = 120;
        }
    }
}
