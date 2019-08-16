 /******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  AccessoryDutyInfoManageServer.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/07/17
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/07/17 8:54:12 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using DBOperate;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using GlobalObject;


namespace ServerModule
{
    /// <summary>
    /// 零件责任归属管理类
    /// </summary>
    class AccessoryDutyInfoManageServer : BasicServer, IAccessoryDutyInfoManageServer
    {
        /// <summary>
        /// 获得供应商的主要责任人
        /// </summary>
        /// <param name="providerCode">供应商编码</param>
        /// <returns>负责人姓名</returns>
        public string GetProviderPrincipal(string providerCode)
        {
            string strSql = "select * from View_ProviderPrincipal where ProviderCode = '" + providerCode + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return "";
            }
            else
            {
                return dtTemp.Rows[0]["PrincipalWorkId"].ToString();
            }
        }

        /// <summary>
        /// 获取零件责任归属信息
        /// </summary>
        /// <param name="listRole">角色列表</param>
        /// <param name="logionName">登录名</param>
        /// <param name="returnAccessory">返回查询到的零件责任归属信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllAccessoryDutyInfo(List<string> listRole, string logionName,
            out IQueryable<View_B_AccessoryDutyInfo> returnAccessory, out string error)
        {
            returnAccessory = null;

            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                Table<View_B_AccessoryDutyInfo> table = dataContxt.GetTable<View_B_AccessoryDutyInfo>();
                returnAccessory = from c in table
                                  orderby c.类别, c.图号型号
                                  select c;
                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 获取指定用户指定权限的零件责任归属信息
        /// </summary>
        /// <param name="userCode">用户名</param>
        /// <param name="userPower">
        /// 0     ：获取记录中包含指定用户名的信息（如：SQE、采购员、供应商开发人员中有任何一个是指定用户的记录；
        /// 其他值：所有记录
        /// </param>
        /// <param name="returnAccessory">返回查询到的零件信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllAccessoryCode(string userCode, int userPower,
            out DataTable returnAccessory, out string error)
        {
            returnAccessory = null;
            error = null;

            Hashtable paramTable = new Hashtable();

            paramTable.Add("@UserCode", userCode);
            paramTable.Add("@Power", userPower);

            DataSet AccessoryInfoDataSet = new DataSet();
            Dictionary<OperateCMD, object> dicOperateCMD =
                m_dbOperate.RunProc_CMD("SelAllView_B_AccessoryDutyInfo", AccessoryInfoDataSet, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            returnAccessory = AccessoryInfoDataSet.Tables[0];
            return true;
        }

        #region 夏石友，2012-07-18，将报检入库单中的此功能移动到此，原方法名：CheckSafeProvider

        /// <summary>
        /// 判断此物品与供应商是否通过审核
        /// </summary>
        /// <param name="AccessoryDutyInfo">零件责任归属信息</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>供应商通过审核返回true，否则返回false</returns>
        public bool IsSafeProvider(B_AccessoryDutyInfo AccessoryDutyInfo, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.B_AccessoryDutyInfo
                              where a.GoodsID == AccessoryDutyInfo.GoodsID
                              select a;

                if (varData.Count() != 1)
                {
                    throw new Exception("数据为空或者不唯一");
                }
                else
                {
                    B_AccessoryDutyInfo tempInfo = varData.Single();

                    if (tempInfo.ProviderA != null
                        && tempInfo.ProviderA.Split(',').ToList().Contains(AccessoryDutyInfo.ProviderA))
                    {
                        return true;
                    }

                    if (tempInfo.ProviderB != null
                        && tempInfo.ProviderB.Split(',').ToList().Contains(AccessoryDutyInfo.ProviderA))
                    {
                        return true;
                    }

                    if (tempInfo.ProviderC != null
                        && tempInfo.ProviderC.Split(',').ToList().Contains(AccessoryDutyInfo.ProviderA))
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        #endregion
    }
}
