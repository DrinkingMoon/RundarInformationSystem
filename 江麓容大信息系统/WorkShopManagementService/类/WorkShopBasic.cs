/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  WorkShopBasic.cs
 * 作者    :  曹津彬    版本: v1.00    日期: 2014/01/22
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 ******************************************************************************/
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
using System.Text.RegularExpressions;
using ServerModule;
using System.Reflection;

namespace Service_Manufacture_WorkShop
{
    /// <summary>
    /// 车间基础信息服务
    /// </summary>
    public class WorkShopBasic : IWorkShopBasic
    {
        /// <summary>
        /// 由权限获得所管辖的车间代码
        /// </summary>
        /// <returns>返回车间代码列表</returns>
        public List<string> GetWorkShopCodeRole()
        {
            List<string> result = new List<string>();

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.装配车间管理员.ToString()))
            {
                result.Add(CE_WorkShopCode.ZPCJ.ToString());
            }

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.下线车间管理员.ToString()))
            {
                result.Add(CE_WorkShopCode.XXCJ.ToString());
            }

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.TCU车间管理员.ToString()))
            {
                result.Add(CE_WorkShopCode.TCUCJ.ToString());
            }

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.机加车间管理员.ToString()))
            {
                result.Add(CE_WorkShopCode.JJCJ.ToString());
            }

            return result;
        }

        /// <summary>
        /// 获得单条车间信息
        /// </summary>
        /// <param name="msg">车间编码或者车间名称</param>
        /// <returns>返回结果集</returns>
        public WS_WorkShopCode GetWorkShopCodeInfo(string msg)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.WS_WorkShopCode
                          where a.WSCode == msg || a.WSName == msg || a.DeptCode == msg
                          select a;

            if (varData.Count() != 1)
            {
                return null;
            }
            else
            {
                return varData.Single();
            }
        }

        /// <summary>
        /// 获得车间基础信息
        /// </summary>
        /// <returns>返回Table</returns>
        public DataTable GetWorkShopBasicInfo()
        {
            string strSql = "select * from View_WS_WorkShopCode";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 根据用途编码获取用途名称
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns>返回结果集</returns>
        public WS_ConsumptionPurpose GetConsumptionPurpose(string code)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.WS_ConsumptionPurpose
                          where a.PurposeCode == code
                          select a;

            if (varData.Count() != 1)
            {
                return null;
            }
            else
            {
                return varData.Single();
            }
        }

        /// <summary>
        /// 获得人员对应的车间信息
        /// </summary>
        /// <param name="info">人员工号或者人员名称</param>
        /// <returns>返回数据集</returns>
        public WS_WorkShopCode GetPersonnelWorkShop(DepotManagementDataContext ctx, string info)
        {
            var varData = from a in ctx.HR_PersonnelArchive
                          where a.WorkID == info || a.Name == info
                          select a;

            if (varData.Count() > 0)
            {
                HR_PersonnelArchive temp = varData.First();

                var varData1 = (from a in ctx.WS_WorkShopCode
                                select a).Where(k => k.DeptCode.Length <= temp.Dept.Length
                                   && k.DeptCode == temp.Dept.Substring(0, k.DeptCode.Length));

                if (varData1.Count() > 0)
                {
                    return varData1.First();
                }
            }

            return null;
        }

        /// <summary>
        /// 获得人员对应的车间信息
        /// </summary>
        /// <param name="info">人员工号或者人员名称</param>
        /// <returns>返回数据集</returns>
        public WS_WorkShopCode GetPersonnelWorkShop(string info)
        {
            WS_WorkShopCode tempLnq = new WS_WorkShopCode();

            string strSql = " select DeptCode,WSCode,WSName from HR_PersonnelArchive as a inner join WS_WorkShopCode as b on " +
                            " LEN(a.Dept) >= LEN(b.DeptCode) and SUBSTRING(a.Dept,1,LEN(b.DeptCode)) = b.DeptCode " +
                            " where WorkID = '"+ info +"' or Name = '"+ info +"'";

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (tempTable != null && tempTable.Rows.Count > 0)
            {
                tempLnq.DeptCode = tempTable.Rows[0]["DeptCode"].ToString();
                tempLnq.WSCode = tempTable.Rows[0]["WSCode"].ToString();
                tempLnq.WSName = tempTable.Rows[0]["WSName"].ToString();

                return tempLnq;
            }
            else
            {
                return null;
            }
        }
    }
}
