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
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 库房信息界面
    /// </summary>
    public partial class 库房信息 : Form
    {
        /// <summary>
        /// 权限控制
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 库房服务
        /// </summary>
        IStorageInfo m_serverStorageInfo = ServerModuleFactory.GetServerModule<IStorageInfo>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        public 库房信息(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();
            m_authFlag = nodeInfo.Authority;
            AuthorityControl(m_authFlag);
            Refresh(2);
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        /// <param name="intFlag">0 库存界面，1 关系界面,2 两个界面同时</param>
        void Refresh(int intFlag)
        {
            if (intFlag == 0)
            {
                dataGridView1.DataSource = m_serverStorageInfo.GetStoreRoom();
            }
            else if (intFlag == 1)
            {
                dataGridView2.DataSource = m_serverStorageInfo.GetStoreRoomAndPerson();
            }
            else
            {
                dataGridView1.DataSource = m_serverStorageInfo.GetStoreRoom();
                dataGridView2.DataSource = m_serverStorageInfo.GetStoreRoomAndPerson();
            }
        }

        void tbs_StorageName_OnCompleteSearch()
        {
            tbs_StorageName.Tag = tbs_StorageName.DataResult["库房编码"].ToString();
        }

        void tbs_Personnel_OnCompleteSearch()
        {
            tbs_Personnel.Tag = tbs_Personnel.DataResult["工号"].ToString();
        }

        #region 控件事件

        private void dataGridView2_Click(object sender, EventArgs e)
        {
            tbs_StorageName.Text = dataGridView2.CurrentRow.Cells["库房名称"].Value.ToString();
            tbs_StorageName.Tag = dataGridView2.CurrentRow.Cells["库房编码"].Value.ToString();
            tbs_Personnel.Text = dataGridView2.CurrentRow.Cells["人员工号"].Value.ToString();
            tbs_Personnel.Tag = dataGridView2.CurrentRow.Cells["人员姓名"].Value.ToString();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            txtStorageName.Text = dataGridView1.CurrentRow.Cells["库房名称"].Value.ToString();
            txtStorageID.Text = dataGridView1.CurrentRow.Cells["库房编码"].Value.ToString();
        }

        private void btNewStorage_Click(object sender, EventArgs e)
        {
            txtStorageID.Text = "";
            txtStorageName.Text = "";
        }

        private void btNew_Click(object sender, EventArgs e)
        {
            tbs_Personnel.Text = "";
            tbs_Personnel.Tag = -1;
            tbs_StorageName.Text = "";
            tbs_StorageName.Tag = -1;
        }

        private void btAddStorage_Click(object sender, EventArgs e)
        {
            string strStorageID = txtStorageID.Text.Trim();
            string strName = txtStorageName.Text.Trim();

            if (m_serverStorageInfo.AddStorage(strStorageID,strName,out m_err))
            {
                MessageBox.Show("添加成功","提示");
            }
            else
            {
                MessageBox.Show("错误信息："+m_err,"提示");
            }

            Refresh(0);

            btNewStorage_Click(sender,e);
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            string strWorkID = tbs_Personnel.Tag.ToString();
            string strStorageID = tbs_StorageName.Tag.ToString();

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (dataGridView2.Rows[i].Cells["库房编码"].Value.ToString() == strStorageID
                    && dataGridView2.Rows[i].Cells["人员工号"].Value.ToString() == strWorkID)
                {
                    MessageDialog.ShowPromptMessage("不能重复添加相同信息！");
                    return;
                }
            }

            if (m_serverStorageInfo.AddStorageAndPersonnel(strWorkID,strStorageID,out m_err))
            {
                MessageBox.Show("添加成功","提示");
            }
            else
            {
                MessageBox.Show("错误信息："+ m_err,"提示");
            }

            Refresh(1);

            btNew_Click(sender, e);
        }

        private void btDeleteStorage_Click(object sender, EventArgs e)
        {
            string strStorageID = dataGridView1.CurrentRow.Cells["库房编码"].Value.ToString();

            string strName = dataGridView1.CurrentRow.Cells["库房名称"].Value.ToString();

            if (MessageBox.Show("如删除此库房，库房与人员关系也将被删除，是否要删除？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (m_serverStorageInfo.DeleteStorage(strStorageID, strName, out m_err))
                {
                    MessageBox.Show("删除成功", "提示");
                }
                else
                {
                    MessageBox.Show("错误信息：" + m_err, "提示");
                }
                Refresh(2);
            }

            btNewStorage_Click(sender, e);
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            string strWorkID = dataGridView2.CurrentRow.Cells["人员工号"].Value.ToString();
            string strStorageID = dataGridView2.CurrentRow.Cells["库房编码"].Value.ToString();

            if (m_serverStorageInfo.DeleteStorageAndPersonnel(strWorkID, strStorageID, out m_err))
            {
                MessageBox.Show("删除成功", "提示");
            }
            else
            {
                MessageBox.Show("错误信息：" + m_err, "提示");
            }

            Refresh(1);

            btNew_Click(sender, e);
        }
        #endregion 
    }
}
