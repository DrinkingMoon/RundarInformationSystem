using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UniversalControlLibrary
{
    public partial class FormDataTree : Form
    {

        public event GlobalObject.DelegateCollection.DelegateButtonAffrim ButtonAffrim;

        /// <summary>
        /// 树形展示窗体
        /// </summary>
        /// <param name="topText">窗体名称</param>
        /// <param name="info">树形信息</param>
        /// <param name="showInfo">树形节点显示字段名</param>
        /// <param name="childID">子节点ID字段名</param>
        /// <param name="parentID">父节点ID字段名</param>
        /// <param name="rootSelect">顶端ID筛选条件</param>
        public FormDataTree(string topText,DataTable info, string showInfo, string childID, string parentID, string rootSelect)
        {
            InitializeComponent();

            labelTitle.Text = topText;

            GlobalObject.GeneralFunction.LoadTreeViewDt(this.treeView1, info, showInfo, childID, parentID, rootSelect);

            treeView1.ExpandAll();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ButtonAffrim != null)
            {
                ButtonAffrim(treeView1);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
