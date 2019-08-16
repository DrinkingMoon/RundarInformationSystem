/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormSanctionCount.cs
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
using System.Drawing;
using System.Linq;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using PlatformManagement;

namespace Expression
{
    /// <summary>
    /// 报废流程数量审批界面
    /// </summary>
    public partial class FormSanctionCount : Form
    {
        public FormSanctionCount()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormSanctionCount_Load(object sender, EventArgs e)
        {
            int sanctionCount1 = 0;
            int sanctionCount2 = 0;
            int sanctionCount3 = 0;

            numSanctionCount1.Value = Convert.ToDecimal(sanctionCount1);
            numSanctionCount2.Value = Convert.ToDecimal(sanctionCount2);
            numSanctionCount3.Value = Convert.ToDecimal(sanctionCount3);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int sanctionCount1 = Convert.ToInt32(numSanctionCount1.Value);
            int sanctionCount2 = Convert.ToInt32(numSanctionCount2.Value);
            int sanctionCount3 = Convert.ToInt32(numSanctionCount3.Value);

            this.Close();
        }
    }
}
