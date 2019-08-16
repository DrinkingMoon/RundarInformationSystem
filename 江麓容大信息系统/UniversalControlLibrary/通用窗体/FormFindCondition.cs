/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormFindCondition.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/15
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 关于界面
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2009/07/03 08:02:08 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
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
using System.Reflection;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 单据查询界面类
    /// </summary>
    public partial class FormFindCondition : Form, IConditionForm
    {
        /// <summary>
        /// SQL查询where语句字段
        /// </summary>
        string m_sentenceSQL = null;

        /// <summary>
        /// 生成的查询条件SQL语句(仅包含where语句部分)
        /// </summary>
        public string SearchSQL
        {
            get { return m_sentenceSQL; }
        }

        /// <summary>
        /// 查询字段
        /// </summary>
        string[] m_arrayFindFild;

        /// <summary>
        /// 新增条件次数
        /// </summary>
        int m_count;

        /// <summary>
        /// 查询条件字典
        /// </summary>
        Dictionary<string, UserControlFindCondition> m_controlDic = new Dictionary<string, UserControlFindCondition>();

        /// <summary>
        /// 查询条件临时字典
        /// </summary>
        Dictionary<string, UserControlFindCondition> m_tempControlDic = new Dictionary<string, UserControlFindCondition>();

        /// <summary>
        /// 查询的业务名称
        /// </summary>
        string m_findBusiness = null;

        /// <summary>
        /// 查询的业务名称
        /// </summary>
        public string Business
        {
            get { return m_findBusiness; }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        public FormFindCondition(string[] arrayFindFild)
        {
            Init();
            m_arrayFindFild = arrayFindFild;
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormFindCondition_Load(object sender, EventArgs e)
        {
            panelParameter.Controls.Clear();
            panelTop.Height = 120;

            foreach (var item in m_controlDic)
            {
                panelParameter.Controls.Add(item.Value);
                item.Value.Dock = DockStyle.Bottom;
                panelTop.Height = panelTop.Height + item.Value.Height;
            }
        }

        private void btnAddCondition_MouseEnter(object sender, EventArgs e)
        {
            btnAddCondition.ForeColor = Color.Red;
        }

        private void btnAddCondition_MouseLeave(object sender, EventArgs e)
        {
            btnAddCondition.ForeColor = Color.Blue;
        }

        private void btnAddCondition_MouseDown(object sender, MouseEventArgs e)
        {
            btnAddCondition.ForeColor = Color.Gold;
        }

        private void btnAddCondition_MouseUp(object sender, MouseEventArgs e)
        {
            btnAddCondition.ForeColor = Color.Blue;
        }

        /// <summary>
        /// 新增条件
        /// </summary>
        private void btnAddCondition_Click(object sender, EventArgs e)
        {
            UserControlFindCondition tmpControl = new UserControlFindCondition(m_arrayFindFild, this);

            tmpControl.Parent = panelParameter;
            tmpControl.Dock = DockStyle.Bottom;
            panelTop.Height = panelTop.Height + tmpControl.Height;

            m_count++;

            tmpControl.Name = tmpControl.Name + m_count.ToString();
            m_tempControlDic.Add(tmpControl.Name, tmpControl);

            int height  = 152 + (m_tempControlDic.Count - 1) * tmpControl.Height;
            this.MaximumSize = new Size(this.Width, height);
            this.Height = height;
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormFindCondition_Resize(object sender, EventArgs e)
        {
            panelLeft.Width = (this.Width - panelCenter.Width) / 2;
            panelRight.Width = this.Width - panelCenter.Width - panelLeft.Width;
        }

        /// <summary>
        /// 提交查询条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSumbitFindCondition_Click(object sender, EventArgs e)
        {
            if (panelParameter.Controls.Count == 0)
            {
                MessageDialog.ShowPromptMessage("您还没有建立查询条件无法进行此操作！");
                return;
            }

            m_sentenceSQL = null;

            try
            {
                for (int i = 0; i < panelParameter.Controls.Count; i++)
                {
                    UserControlFindCondition newControl = panelParameter.Controls[i] as UserControlFindCondition;

                    if (i != panelParameter.Controls.Count - 1)
                    {
                        m_sentenceSQL = m_sentenceSQL + newControl.BuildSQLSentence("",false);
                    }
                    else
                    {
                        m_sentenceSQL = m_sentenceSQL + newControl.BuildSQLSentence("",true);
                    }
                }
             
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception exce)
            {
                Console.WriteLine(exce.Message);
                return;
            }
        }

        /// <summary>
        /// 删除条件
        /// </summary>
        /// <param name="tmpControl"></param>
        public void DeleteCondition(UserControlFindCondition tmpControl)
        {
            panelTop.Height = panelTop.Height - tmpControl.Height;
            panelParameter.Controls.Remove(tmpControl);
            m_tempControlDic.Remove(tmpControl.Name);

            this.Height = 152 + (m_tempControlDic.Count - 1) * tmpControl.Height;
            this.MaximumSize = new Size(this.Width, this.Height);
        }
    }
}
