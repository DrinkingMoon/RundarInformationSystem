using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;
using PlatformManagement;

namespace UniversalControlLibrary
{
    public partial class CheckBillDateAndStatus : UserControl
    {
        /// <summary>
        /// 单据状态
        /// </summary>
        private List<string> m_listBillStatus = new List<string>();

        public List<string> ListBillStatus
        {
            get { return m_listBillStatus; }
            set { m_listBillStatus = value; }
        }

        public string error = "";

        /// <summary>
        /// 是否多选 True 多选；False 单选
        /// </summary>
        private bool m_blCheck = false;

        /// <summary>
        /// 显示状态查询选项
        /// </summary>
        private bool m_blStatusVisible = true;

        /// <summary>
        /// 显示多选选项
        /// </summary>
        private bool m_blMultiVisible = true;

        #region 属性

        /// <summary>
        /// 显示状态查询选项
        /// </summary>
        public bool StatusVisible
        {
            get { return m_blStatusVisible; }
            set { m_blStatusVisible = value; }
        }

        /// <summary>
        /// 属性
        /// </summary>
        public bool MultiVisible
        {
            get { return m_blMultiVisible; }
            set { m_blMultiVisible = value; }
        }

        #endregion

        /// <summary>
        /// 完成检索后触发的事件
        /// </summary>
        public event GlobalObject.DelegateCollection.NonArgumentHandle OnCompleteSearch;

        public CheckBillDateAndStatus()
        {
            InitializeComponent();
        }

        private void CheckBillDateAndStatus_Load(object sender, EventArgs e)
        {
            if (!m_blStatusVisible)
            {
                rbMultiple.Visible = m_blStatusVisible;
                radioButton1.Visible = m_blStatusVisible;
                label25.Visible = m_blStatusVisible;
                cmbBillStatus.Visible = m_blStatusVisible;
            }
            else
            {
                rbMultiple.Visible = m_blMultiVisible;
            }

            //只能修改控件时间，但是无法进行第一次界面显示时的查询功能，如要实现 必须在界面构造函数或者LOAD中运行 InitDateTime()方法；
            dtpStartTime.Value = DateTime.Now.AddDays(1).AddMonths(-1);
            dtpEndTime.Value = DateTime.Now.AddDays(1);
        }

        public void InitDateTime()
        {
            dtpStartTime.Value = DateTime.Now.AddDays(1).AddMonths(-1);
            dtpEndTime.Value = DateTime.Now.AddDays(1);

        }

        /// <summary>
        /// 插入单据状态
        /// </summary>
        /// <param name="billstatus"></param>
        public void InsertComBox(string[] billstatus)
        {
            for (int i = 0; i < billstatus.Length; i++)
            {
                cmbBillStatus.Items.Add(billstatus[i].ToString());
            }

            cmbBillStatus.SelectedIndex = 0;
        }

        /// <summary>
        /// 插入单据状态
        /// </summary>
        /// <param name="billstatus"></param>
        public void InsertComBox(List<string> listStatus)
        {
            foreach (string item in listStatus)
            {
                cmbBillStatus.Items.Add(item);
            }

            cmbBillStatus.SelectedIndex = 0;
        }

        /// <summary>
        /// 插入单据状态
        /// </summary>
        /// <param name="enumType"></param>
        public void InsertComBox(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                return;
            }
            string[] strBillStatus = new string[Enum.GetValues(enumType).GetLength(0) + 1];

            int i = 1;
            strBillStatus[0] = "全部";
            foreach (var item in Enum.GetValues(enumType))
            {
                strBillStatus[i] = ((Enum)item).ToString();
                i += 1;
            }

            InsertComBox(strBillStatus);
        }

        /// <summary>
        /// 获得查询字符串
        /// </summary>
        /// <param name="fielddatename"></param>
        /// <param name="fieldstatusname"></param>
        /// <returns></returns>
        public string GetSqlString(string fielddatename, string fieldstatusname)
        {
            string result = fielddatename + " between  Convert(dateTime, '" + dtpStartTime.Value.ToShortDateString() + "') and Convert(dateTime, '" + dtpEndTime.Value.ToShortDateString() + "') ";

            if (m_blCheck)
            {
                if (!m_listBillStatus.Contains("全部"))
                {
                    string tempFind = "";

                    foreach (string item in m_listBillStatus)
                    {
                        tempFind += "'" + item + "',";
                    }

                    tempFind = tempFind.Substring(0, tempFind.Length - 1);

                    result += " and " + fieldstatusname + " in (" + tempFind + ")";
                }
            }
            else
            {
                if (cmbBillStatus.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        if (!cmbBillStatus.Text.Contains("全") && !cmbBillStatus.Text.Contains("部"))
                        {
                            result += " and " + fieldstatusname + " = '" + cmbBillStatus.Text + "'";
                        }
                    });
                }

            }

            return result;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (OnCompleteSearch != null)
            {
                if (!rbMultiple.Checked)
                {
                    m_listBillStatus = new List<string>();
                    m_listBillStatus.Add(cmbBillStatus.Text);
                }

                OnCompleteSearch();
            }
        }

        void frm_ExecuteTask(List<string> listCheck)
        {
            m_listBillStatus = listCheck;
            m_blCheck = true;
            btnFind_Click(null, null);
        }

        private void RadioButton_Click(object sender, EventArgs e)
        {
            if (rbMultiple.Checked)
            {
                cmbBillStatus.Enabled = false;
                List<string> tempList = new List<string>();

                foreach (object item in cmbBillStatus.Items)
                {
                    tempList.Add(item.ToString());
                }

                FormDataCheck frm = new FormDataCheck(tempList, m_listBillStatus);
                frm.ExecuteTask += new GlobalObject.DelegateCollection.DelegateTask(frm_ExecuteTask);
                frm.ShowDialog();
            }
            else
            {
                cmbBillStatus.Enabled = true;
                m_listBillStatus = null;
                m_blCheck = false;
            }
        }
    }
}