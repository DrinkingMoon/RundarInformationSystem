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
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 职位信息界面
    /// </summary>
    public partial class 职位信息 : Form
    {
        /// <summary>
        /// 人员服务组件
        /// </summary>
        IPersonnelInfoServer m_personnelInfo = ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 对数据类型已知的特定数据源
        /// </summary>
        IQueryable<HR_PositionType> m_findPositionType;

        /// <summary>
        /// 窗体加载
        /// </summary>
        public 职位信息()
        {
            InitializeComponent();
            
            m_findPositionType = m_personnelInfo.GetPositionType();
            dgvShowPosition.DataSource = m_findPositionType;           
        }

        /// <summary>
        /// 提交按钮，添加或修改职位信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPosition_Click(object sender, EventArgs e)
        {
            if (txtPositionID.Text == "" || txtPositionID.Text == null)
            {
                return;
            }

            if (txtPositionID.ReadOnly)
            {
                HR_PositionType type = new HR_PositionType();

                type.ID = int.Parse(txtPositionID.Text);
                type.PositionName = txtPositionName.Text;
                type.Remark = txtPositionRemark.Text;

                if (!m_personnelInfo.UpdatePositionType(type, out m_strErr))
                {
                    MessageDialog.ShowErrorMessage(m_strErr);
                    return;
                }
                else
                {
                    MessageBox.Show("职位修改成功！", "提示");
                }
            }
            else
            {
                HR_PositionType type = new HR_PositionType();

                type.ID = int.Parse(txtPositionID.Text);
                type.PositionName = txtPositionName.Text;
                type.Remark = txtPositionRemark.Text;

                if (!m_personnelInfo.AddPositionType(type, out m_strErr))
                {
                    MessageDialog.ShowErrorMessage(m_strErr);
                    return;
                }
                else
                {
                    MessageBox.Show("新职位添加成功！", "提示");
                }
            }

            m_findPositionType = m_personnelInfo.GetPositionType();
            dgvShowPosition.DataSource = m_findPositionType;
            //this.DialogResult = DialogResult.OK;

        }

        /// <summary>
        /// 职员编号只能输入数字
        /// </summary>
        private void txtPositionID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtPositionID.SelectionLength == 0)
            {
                if (e.KeyChar.CompareTo('0') < 0 || e.KeyChar.CompareTo('9') > 0)
                {
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// 新建按钮,使控件可用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInsert_Click(object sender, EventArgs e)
        {
            txtPositionID.ReadOnly = false;
            txtPositionID.Text = "";
            txtPositionName.Text = "";
            txtPositionRemark.Text = "";
        }

        /// <summary>
        /// datagridview的单元格点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvShowPosition_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            txtPositionID.Text = dgvShowPosition.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtPositionName.Text = dgvShowPosition.Rows[e.RowIndex].Cells[1].Value.ToString();

            txtPositionID.ReadOnly = true;
        }

        private void dgvShowPosition_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dgvShowPosition.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dgvShowPosition.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dgvShowPosition.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
    }

}
