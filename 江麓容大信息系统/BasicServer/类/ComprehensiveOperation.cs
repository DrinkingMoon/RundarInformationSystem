using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using DBOperate;
using PlatformManagement;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 综合性操作类
    /// </summary>
    class ComprehensiveOperation : BasicServer, ServerModule.IComprehensiveOperation
    {
        /// <summary>
        /// 操作隐藏字段数据集
        /// </summary>
        /// <param name="formName">窗体名称</param>
        /// <param name="dataGridViewName">数据显示控件名称</param>
        /// <param name="userCode">用户编码</param>
        /// <param name="dtHideFields">是否隐藏设置表</param>
        /// <param name="error">出错时，输出错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool OperationHideFields(string formName, string dataGridViewName,
            string userCode, DataTable dtHideFields, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.SYS_HideFields
                              where a.FormName == formName
                              && a.DataGridViewName == dataGridViewName
                              && a.UserCode == userCode
                              select a;

                dataContext.SYS_HideFields.DeleteAllOnSubmit(varData);

                for (int i = 0; i < dtHideFields.Rows.Count; i++)
                {
                    if (dtHideFields.Rows[i]["选"].ToString() != "True")
                    {
                        SYS_HideFields lnqHideFields = new SYS_HideFields();

                        lnqHideFields.DataGridViewName = dataGridViewName;
                        lnqHideFields.FieldName = dtHideFields.Rows[i]["FieldName"].ToString();
                        lnqHideFields.FormName = formName;
                        lnqHideFields.UserCode = userCode;

                        dataContext.SYS_HideFields.InsertOnSubmit(lnqHideFields);
                    }

                }

                dataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }

            return true;
        }
    }
}
