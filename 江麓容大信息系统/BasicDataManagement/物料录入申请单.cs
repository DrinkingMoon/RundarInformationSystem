using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using Expression;
using GlobalObject;
using PlatformManagement;
using FlowControlService;
using Service_Project_Design;
using UniversalControlLibrary;

namespace Form_Project_Design
{
    public partial class 物料录入申请单 : CustomMainForm
    {
        /// <summary>
        /// 操作权限
        /// </summary>
        AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 物料录入服务组件
        /// </summary>
        IGoodsEnteringBill m_serverGoodsEntering = Service_Project_Design.ServerModuleFactory.GetServerModule<IGoodsEnteringBill>();

        string m_strErr;

        public 物料录入申请单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
            : base(typeof(物料录入申请单明细), GlobalObject.CE_BillTypeEnum.物料录入申请单,
            Service_Project_Design.ServerModuleFactory.GetServerModule<IGoodsEnteringBill>())
        {
            InitializeComponent();
            m_authorityFlag = nodeInfo.Authority;
        }

        bool frm_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            CE_FlowOperationType tempType = GlobalObject.GeneralFunction.StringConvertToEnum<CE_FlowOperationType>(form.ResultList[1].ToString());

            switch (tempType)
            {
                case CE_FlowOperationType.提交:
                    if (!m_serverGoodsEntering.SubmitInfo(form.ResultList[0].ToString(), 
                        (form.ResultInfo as BindingCollection<View_S_GoodsEnteringBill>).ToList(), out m_strErr))
                    {
                        MessageDialog.ShowPromptMessage(m_strErr);
                        return false;
                    }
                    break;
                case CE_FlowOperationType.暂存:
                    if (!m_serverGoodsEntering.EditListInfo(form.ResultList[0].ToString(), 
                        (form.ResultInfo as BindingCollection<View_S_GoodsEnteringBill>).ToList(), out m_strErr))
                    {
                        MessageDialog.ShowPromptMessage(m_strErr);
                        return false;
                    }
                    break;
                case CE_FlowOperationType.回退:
                    break;
                case CE_FlowOperationType.未知:
                    break;
                default:
                    break;
            }

            if (!m_serverGoodsEntering.IsExist(form.FlowInfo_BillNo))
            {
                MessageDialog.ShowPromptMessage("数据为空，保存失败，如需退出，请直接X掉界面");
                return false;
            }

            return true;
        }

        private void 物料录入申请单_Load(object sender, EventArgs e)
        {
            this.ToolStripSeparator_ShowStatus(m_authorityFlag);
        }
    }
}
