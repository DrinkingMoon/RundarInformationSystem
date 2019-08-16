/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UniversalFunction.cs
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
using System.Linq;
using System.Text;
using System.Data;
using GlobalObject;
using System.Collections;
using DBOperate;

namespace ServerModule
{
    /// <summary>
    /// 通用功能类
    /// </summary>
    public class UniversalFunction : BasicServer
    {
        public static string GetWorkID(string workID)
        {
            string strSql = "select WorkID from HR_WorkID_No_Mapping where workNo = '" + workID + "'";

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (tempTable == null || tempTable.Rows.Count == 0)
            {
                return workID;
            }
            else
            {
                return tempTable.Rows[0][0].ToString();
            }
        }

        public static string GetLoginNotice()
        {
            string strSql = "select * from Base_Switch where ID = 13";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);
            return dt.Rows[0]["SwitchKey"].ToString();
        }

        /// <summary>
        /// 获得计划月
        /// </summary>
        /// <param name="month">当前月</param>
        /// <param name="planNode">计划点</param>
        /// <returns>返回计划月</returns>
        public static int GetPlanMonth(int month, int planNode)
        {
            if (month + planNode > 12)
            {
                return month + planNode - 12;
            }
            else
            {
                return month + planNode;
            }
        }

        /// <summary>
        /// 获取隐藏字段数据集
        /// </summary>
        /// <param name="formName">界面名</param>
        /// <param name="dataGridName">DataGrid空间名称</param>
        /// <param name="userCode">操作用户工号</param>
        /// <returns>Table</returns>
        public static DataTable SelectHideFields(string formName, string dataGridName, string userCode)
        {
            string strSql = "select FieldName from SYS_HideFields where FormName = '"
                + formName + "' and DataGridViewName = '" + dataGridName
                + "' and UserCode = '" + userCode + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得供应商信息
        /// </summary>
        /// <param name="providerCodeOrShortName">供应商编码/简称/全称</param>
        /// <returns>返回LINQ信息</returns>
        public static View_Provider GetProviderInfo(string providerCodeOrShortName)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_Provider
                          where a.供应商编码 == providerCodeOrShortName 
                          || a.简称 == providerCodeOrShortName 
                          || a.供应商名称 == providerCodeOrShortName
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else if (varData.Count() > 1)
            {
                return varData.Where(k => k.是否在用 == true).First();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 定位单个单据并显示在界面上
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="billType">单据类型</param>
        /// <returns>成功返回记录，不成功返回NULL</returns>
        public static DataTable PositioningOneRecord(string billID, string billType)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Flow_BusinessInfo
                          where a.BusinessTypeName == billType
                          orderby a.Version descending
                          select a;

            if (varData.Count() > 0 && varData.First().ViewShow != null)
            {
                string strSql = "select * from " + varData.First().ViewShow + " where 单据号 = '" + billID + "'";
                DataTable result1 = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (result1 != null && result1.Rows.Count > 0)
                {
                    return result1;
                }
            }


            DBOperate.IDBOperate dbOperate = CommentParameter.GetDBOperatorOfDepotManagement();

            System.Collections.Hashtable paramTable = new System.Collections.Hashtable();

            DataSet ds = new DataSet();

            paramTable.Add("@BillType", billType);
            paramTable.Add("@BillID", billID);

            Dictionary<DBOperate.OperateCMD, object> result = dbOperate.RunProc_CMD("BASE_GetMainBillFromMessage", ds, paramTable);

            if (!Convert.ToBoolean(result[DBOperate.OperateCMD.Return_OperateResult]))
            {
                string error = Convert.ToString(result[DBOperate.OperateCMD.Return_Errmsg]);

                if (error != "没有找到任何数据")
                {
                    return null;
                }
            }

            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 根据库房名称获得库房ID
        /// </summary>
        /// <param name="storageName">库房名称</param>
        /// <returns>返回库房ID</returns>
        public static string GetStorageID(string storageName)
        {
            string strSql = "select StorageID from (select StorageID,StorageName from Base_Storage Union all select SecStorageID, " +
                " SecStorageName from Out_StockInfo) as a  where StorageName = '" + storageName.Trim() + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return "";
            }

            return dtTemp.Rows[0][0].ToString();
        }

        /// <summary>
        /// 根据单位名称获得单位ID
        /// </summary>
        /// <param name="unitName">单位名称</param>
        /// <returns>返回单位ID</returns>
        public static string GetUnitID(string unitName)
        {
            string strSql = "select ID from S_Unit where UnitName = '" + unitName + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return "";
            }

            return dtTemp.Rows[0][0].ToString();
        }

        /// <summary>
        /// 根据单位ID获得单位名称
        /// </summary>
        /// <param name="unitID">单位ID</param>
        /// <returns>返回单位名称</returns>
        public static string GetUnitName(string unitID)
        {
            string strSql = "select UnitName from S_Unit where ID = '" + unitID + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return "";
            }

            return dtTemp.Rows[0][0].ToString();
        }

        /// <summary>
        /// 根据库房ID获得库房名称
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回库房名称</returns>
        public static string GetStorageName(DepotManagementDataContext ctx, string storageID)
        {
            var varData = from a in
                              (from a in ctx.BASE_Storage
                               select new { a.StorageID, a.StorageName }).
                                  Union(from b in ctx.Out_StockInfo
                                        select new { StorageID = b.SecStorageID, StorageName = b.SecStorageName })
                          where a.StorageID == storageID
                          select a.StorageName;

            if (varData.Count() == 0)
            {
                return "";
            }
            else
            {
                return varData.First();
            }
        }

        /// <summary>
        /// 根据库房ID获得库房名称
        /// </summary>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回库房名称</returns>
        public static string GetStorageName(string storageID)
        {
            string strSql = "select StorageName from (select StorageID,StorageName from Base_Storage Union all select SecStorageID," +
                " SecStorageName from Out_StockInfo) as a  where StorageID = '" + storageID.Trim() + "'";
            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return "";
            }

            return dtTemp.Rows[0][0].ToString();
        }

        /// <summary>
        /// 获得库房列表
        /// </summary>
        /// <returns>返回库房表</returns>
        public static DataTable GetStorageTb()
        {
            string strSql = "select * from Base_Storage";
            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return null;
            }

            return dtTemp;
        }

        public static List<BASE_Storage> GetListStorageInfo()
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return GetListStorageInfo(ctx);
        }

        public static List<BASE_Storage> GetListStorageInfo(DepotManagementDataContext ctx)
        {
            var varData = from a in ctx.BASE_Storage
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 获得车间列表
        /// </summary>
        /// <returns>返回车间表</returns>
        public static DataTable GetWorkShopTb()
        {
            string strSql = "select * from WS_WorkShopCode";
            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return null;
            }

            return dtTemp;
        }

        /// <summary>
        /// 获得公司实际库房列表
        /// </summary>
        /// <returns>返回库房表</returns>
        public static DataTable GetOutRealStorageInfo()
        {
            string strSql = "select * from Out_StockInfo as a inner join Base_Storage as b on a.SecStorageID = b.StorageID";
            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return null;
            }

            return dtTemp;
        }

        /// <summary>
        /// 获得服务站点列表
        /// </summary>
        /// <returns>返回库房表</returns>
        public static DataTable GetServiceStationInfo()
        {
            string strSql = "select * from Out_ServiceStationInfo";
            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return null;
            }

            return dtTemp;
        }

        /// <summary>
        /// 获得二级库房列表
        /// </summary>
        /// <returns>返回库房表</returns>
        public static DataTable GetSecStorageTb()
        {
            string strSql = "select * from Out_StockInfo";
            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return null;
            }

            return dtTemp;
        }

        /// <summary>
        /// 获得判断此库房是否为此人员所属库房
        /// </summary>
        /// <param name="storageID">库房ID</param>
        /// <returns>True 是，False 否</returns>
        public static bool CheckStorageAndPersonnel(string storageID)
        {
            string strSql = "select * from Base_StorageAndPersonnel where StorageID = '"
                + storageID + "' and WorkID = '" + BasicInfo.LoginID + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 获得人员名称
        /// </summary>
        /// <param name="personnelName">人员工号</param>
        /// <returns>人员姓名</returns>
        public static string GetPersonnelName(string personnelCode)
        {
            if (personnelCode == null || personnelCode == "")
            {
                return "";
            }
            else
            {
                string strSql = "select Name from HR_Personnel where WorkID = '" + personnelCode.ToString() + "'";
                DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

                return dtTemp.Rows[0][0].ToString();
            }
        }

        /// <summary>
        /// 获得人员ID
        /// </summary>
        /// <param name="personnelName">人员姓名</param>
        /// <returns>人员工号</returns>
        public static string GetPersonnelCode(string personnelName)
        {
            if (personnelName == null || personnelName == "")
            {
                return "";
            }
            else
            {
                string strSql = "select WorkID from HR_Personnel where Name = '" + personnelName.ToString() + "'";
                DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

                return dtTemp.Rows[0][0].ToString();
            }
        }

        /// <summary>
        /// 获得部门编码
        /// </summary>
        /// <param name="deptName">部门名称</param>
        /// <returns>部门编码</returns>
        public static string GetDeptCode(string deptName)
        {
            if (deptName == null || deptName == "")
            {
                return "";
            }
            else
            {
                string strSql = "select DeptCode from HR_Dept where DeptName = '" + deptName + "'";
                DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

                return dtTemp.Rows[0][0].ToString();
            }
        }

        /// <summary>
        /// 获得部门名称
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <returns>部门名称</returns>
        public static string GetDeptName(string deptCode)
        {
            if (deptCode == null || deptCode == "")
            {
                return "";
            }
            else
            {
                string strSql = "select DeptName from HR_Dept where DeptCode = '" + deptCode + "'";
                DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

                return dtTemp.Rows[0][0].ToString();
            }
        }

        public static HR_Dept GetDept_Belonge(string deptCode)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return GetDept_Belonge(ctx, deptCode);
        }

        public static HR_Dept GetDept_Belonge(DepotManagementDataContext ctx, string deptCode)
        {
            var varData = from a in ctx.HR_Dept
                          where a.DeptCode == ctx.Fun_get_BelongDept_Value(deptCode)
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得拼音五笔码
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="pinYinWuBi">返回方式("拼音","五笔")</param>
        /// <returns>返回拼音或者五笔码</returns>
        public static string GetPYWBCode(string name, string pinYinWuBi)
        {
            System.Nullable<System.Int32> intSurPYWY = 0;

            if (pinYinWuBi == "PY")
            {
                intSurPYWY = 1;
            }
            else
            {
                intSurPYWY = 2;
            }

            string strSql = "select dbo.fun_get_bm('" + name + "'," + intSurPYWY + ")";
            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dtTemp.Rows[0][0].ToString();
        }

        /// <summary>
        /// 获得省份表
        /// </summary>
        /// <returns>返回 Table</returns>
        public static DataTable GetProvinceTable()
        {
            string strSql = "select * from T_Province";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得某省份的下属城市
        /// </summary>
        /// <param name="province">省份名称</param>
        /// <returns>返回其下属的城市集合</returns>
        public static DataTable GetCityUnderlingProvince(string province)
        {
            string strSql = "select distinct 城市 from View_T_ProvinceCityDistrict " +
                " where 省份 = '" + province + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得指定表中的指定的单据状态
        /// </summary>
        /// <param name="tableName">数据库表名</param>
        /// <param name="billStatusName">表所对应的单据状态字段名</param>
        /// <param name="billName">表所对应的单据号字段名</param>
        /// <param name="billID">单据号</param>
        /// <returns>返回单据状态</returns>
        public static string GetBillStatus(string tableName, string billStatusName, string billName, string billID)
        {
            string strSql = "select * from " + tableName + " where " + billName + " = '" + billID + "'";
            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0 || dtTemp.Rows[0][billStatusName].ToString() == "")
            {
                return "";
            }
            else
            {
                return dtTemp.Rows[0][billStatusName].ToString();
            }
        }

        /// <summary>
        /// 判断是否属于Decimal类型
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>是返回True，不是返回False</returns>
        public static bool StringIsDecimal(string str)
        {
            bool isDec = true;

            try
            {
                Convert.ToDecimal(str);
            }
            catch
            {
                isDec = false;
            }

            return isDec;
        }

        /// <summary>
        /// 获得库房或服务站的负责人数据集
        /// </summary>
        /// <param name="secStorageID">编码</param>
        /// <returns>返回数据LIST</returns>
        public static List<string> GetStorageOrStationPrincipal(string secStorageID)
        {
            List<string> list = new List<string>();

            string strSql = "select distinct WorkID from Client as a inner join HR_Personnel as b on a.Principal = b.Name where ClientCode = '"
                + secStorageID + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                strSql = "select b.WorkID from BASE_Storage as a inner join BASE_StorageAndPersonnel as b on a.StorageID = b.StorageID " +
                    " inner join HR_Personnel as c on b.WorkID = c.WorkID where a.StorageID = '" + secStorageID + "'";

                dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (dtTemp.Rows.Count != 0)
                {
                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        list.Add(dtTemp.Rows[i]["WorkID"].ToString());
                    }
                }
            }
            else
            {
                list.Add(dtTemp.Rows[0]["WorkID"].ToString());
            }

            return list;
        }

        /// <summary>
        /// 判断是否为部门领导
        /// </summary>
        /// <param name="dept">部门编码或名称</param>
        /// <param name="personnel">人员信息</param>
        /// <returns>是返回True ，否返回False</returns>
        public static string GetIsManager(string dept, string personnel)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("@Dept", dept);
            parameters.Add("@Type", "");
            parameters.Add("@Personnel", personnel);

            DataSet ds = new DataSet();

            Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.RunProc_CMD("Config_GetDeptManager", ds, parameters);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return "";
            }
            else
            {
                return ds.Tables[0].Rows[0]["ManagerType"].ToString();
            }
        }

        /// <summary>
        /// 判断当前用户是否为对应单据的当前操作用户
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>是 返回True,不是 返回False</returns>
        public static bool IsOperator(string billNo)
        {
            string strSql = "select * from [PlatformService].[dbo].[Flow_BillFlowMessage] where 单据号 = '" + billNo + "'";

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (tempTable != null && tempTable.Rows.Count != 0)
            {
                DataRow dr = tempTable.Rows[0];

                if (dr["单据状态"].ToString() == "已完成")
                {
                    return false;
                }

                if (dr["接收方类型"].ToString() == "角色")
                {
                    List<string> listTemp = dr["接收方"].ToString().Split(',').ToList();

                    foreach (string item in listTemp)
                    {
                        if (BasicInfo.ListRoles.Contains(item))
                        {
                            return true;
                        }
                    }
                }
                else if (dr["接收方类型"].ToString() == "用户")
                {
                    if (dr["接收方"].ToString().Split(',').ToList().Contains(BasicInfo.LoginID))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 根据图号型号、物品名称、规格获得物品ID
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <returns>成功返回物品ID，失败返回0</returns>
        public static int GetGoodsID(string goodsCode, string goodsName, string spec)
        {
            string strSql = "select ID from dbo.F_GoodsPlanCost where " +
                            " GoodsCode ='" + goodsCode + "' and GoodsName='" + goodsName + "' and Spec='" + spec + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return 0;
            }

            return Convert.ToInt32(dtTemp.Rows[0][0].ToString());
        }

        /// <summary>
        /// 获得人员信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="info">员工工号或者姓名</param>
        /// <returns>返回人员信息</returns>
        public static View_HR_Personnel GetPersonnelInfo(DepotManagementDataContext ctx, string info)
        {
            var varData = from a in ctx.View_HR_Personnel
                          where a.工号 == info || a.姓名 == info
                          select a;

            if (varData.Count() == 0)
            {
                return null;
            }
            else
            {
                return varData.ToList<View_HR_Personnel>()[0];
            }
        }

        /// <summary>
        /// 获得人员信息
        /// </summary>
        /// <param name="info">员工工号或者姓名</param>
        /// <returns>返回人员信息</returns>
        public static View_HR_Personnel GetPersonnelInfo(string info)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_HR_Personnel
                          where a.工号 == info || a.姓名 == info
                          select a;

            if (varData.Count() != 1)
            {
                throw new Exception("【工号/姓名】： "+ info +" 不存在或者存在多条信息");
            }
            else
            {
                return varData.ToList<View_HR_Personnel>()[0];
            }
        }

        /// <summary>
        /// 获得人员信息
        /// </summary>
        /// <param name="info">员工工号或者姓名</param>
        /// <returns>返回人员信息</returns>
        public static View_HR_Personnel GetPersonnelInfo(string info, CE_HR_PersonnelStatus status)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_HR_Personnel
                          where a.工号 == info || a.姓名 == info
                          select a;

            if (status == CE_HR_PersonnelStatus.在职)
            {
                varData = from a in varData
                          where a.是否在职 == true
                          select a;

                return varData.ToList<View_HR_Personnel>()[0];
            }
            else
            {
                if (varData.Count() != 1)
                {
                    throw new Exception("获取人员信息不唯一");
                }
                else
                {
                    return varData.ToList<View_HR_Personnel>()[0];
                }
            }
        }

        /// <summary>
        /// 获得所有单位信息
        /// </summary>
        /// <returns>返回单位信息</returns>
        public static IQueryable<string> GetAllUnit()
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_S_Unit
                          where a.停用 == false
                          select a.单位;

            return varData;
        }

        /// <summary>
        /// 根据查询信息获取单条的所有对象库房信息
        /// </summary>
        /// <param name="msg">查询信息</param>
        /// <returns>返回单条LINQ信息</returns>
        public static View_BASE_AllStorageInfo GetAllStorageInfo(string msg)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_BASE_AllStorageInfo
                          where a.Code == msg || a.Name == msg
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
        /// 判断物品是否为产品
        /// （此方法可使为产品，
        /// 但不需设置流水号的物品，仍可以设置流水号，
        /// 并进行流水号的管理）
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>物品为产品返回True，不为产品返回False</returns>
        public static bool IsProduct(int goodsID)
        {
            if (Convert.ToBoolean(UniversalFunction.GetGoodsAttributeInfo(goodsID, CE_GoodsAttributeName.CVT))
                || Convert.ToBoolean(UniversalFunction.GetGoodsAttributeInfo(goodsID, CE_GoodsAttributeName.TCU)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 判断是否为分总成
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>True 是 False 否</returns>
        public static bool IsPointProduct(int goodsID)
        {
            if (Convert.ToBoolean(UniversalFunction.GetGoodsAttributeInfo(goodsID, CE_GoodsAttributeName.部件))
                && !UniversalFunction.IsProduct(goodsID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获得物料管理员权限列表
        /// </summary>
        /// <param name="storageCode">物存处代码</param>
        /// <returns>返回权限列表</returns>
        public static List<CE_RoleEnum> GetStoreroomKeeperRoleEnumList(string storageCode)
        {
            List<CE_RoleEnum> resultList = new List<CE_RoleEnum>();

            switch (storageCode)
            {
                case "01":
                    switch (GlobalParameter.SystemName)
                    {
                        case CE_SystemName.泸州容大:
                            resultList.Add(CE_RoleEnum.物流公司);
                            break;
                        case CE_SystemName.湖南容大:
                            resultList.Add(CE_RoleEnum.制造仓库管理员);
                            break;
                        default:
                            break;
                    }
                    break;
                case "02":
                    resultList.Add(CE_RoleEnum.成品仓库管理员);
                    break;
                case "03":
                    resultList.Add(CE_RoleEnum.电子元器件仓库管理员);
                    break;
                case "04":
                    resultList.Add(CE_RoleEnum.备件仓库管理员);
                    break;
                case "05":
                    resultList.Add(CE_RoleEnum.售后库房管理员);
                    break;
                case "06":
                    resultList.Add(CE_RoleEnum.油品库管理员);
                    break;
                case "07":
                    resultList.Add(CE_RoleEnum.量检具库管理员);
                    break;
                case "08":
                    resultList.Add(CE_RoleEnum.自制半成品库管理员);
                    break;
                case "09":
                    resultList.Add(CE_RoleEnum.售后配件库管理员);
                    break;
                case "10":
                    resultList.Add(CE_RoleEnum.受托品库房管理员);
                    break;
                case "11":
                    resultList.Add(CE_RoleEnum.材料库管理员);
                    break;
                case "12":
                    resultList.Add(CE_RoleEnum.样品库管理员);
                    break;
                case "13":
                    resultList.Add(CE_RoleEnum.样品库管理员);
                    break;
                case "14":
                    resultList.Add(CE_RoleEnum.成品仓库管理员);
                    break;
                case "15":
                    resultList.Add(CE_RoleEnum.制造仓库二管理员);
                    break;
                case "16":
                    resultList.Add(CE_RoleEnum.成品仓库二管理员);
                    break;
                case "17":
                    resultList.Add(CE_RoleEnum.自制半成品库二管理员);
                    break;
                case "JJCJ":
                    resultList.Add(CE_RoleEnum.机加车间管理员);
                    break;
                case "XXCJ":
                    resultList.Add(CE_RoleEnum.下线车间管理员);
                    break;
                case "ZPCJ":
                    resultList.Add(CE_RoleEnum.装配车间管理员);
                    break;
                case "TCUCJ":
                    resultList.Add(CE_RoleEnum.TCU车间管理员);
                    break;
                default:
                    break;
            }

            return resultList;
        }

        /// <summary>
        /// 根据GoodsID判断是否为量检具
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>是返回True，否返回False</returns>
        public static bool IsGauge(DepotManagementDataContext ctx, int goodsID, string storageID)
        {
            if (storageID == "07")
            {
                var varData = from a in ctx.S_GaugeStandingBook
                              where a.GoodsID == goodsID
                              select a;

                if (varData.Count() > 0)
                {
                    return true;
                }
                else
                {
                    var varGoodsInfo = from a in ctx.F_GoodsPlanCost
                                       where a.ID == goodsID && a.GoodsType == BasicInfo.BaseSwitchInfo[(int)CE_SwitchName.量检具类别编码]
                                       select a;

                    if (varGoodsInfo.Count() > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据GoodsID判断是否为量检具
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>是返回True，否返回False</returns>
        public static bool IsGauge(int goodsID, string storageID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return IsGauge(ctx, goodsID, storageID);
        }

        /// <summary>
        /// 检查物品是否在工装台帐中
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="isInStock">是否在库</param>
        /// <returns>True 在 False 不在</returns>
        public static bool IsInStandingBook(int goodsID, bool? isInStock)
        {
            string strSql = "select * from S_FrockStandingBook with(nolock) where GoodsID = " + goodsID;

            if (isInStock != null)
            {
                strSql += " and IsInStock = " + Convert.ToInt32(isInStock);
            }

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 获得某一个开关的值
        /// </summary>
        /// <returns>成功返回开关的值，失败返回""</returns>
        public static Dictionary<int,string> GetSwitchKey()
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            string strSql = "select * from BASE_Switch";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    result.Add(Convert.ToInt32(dr["ID"]), dr["SwitchKey"].ToString());
                }
            }

            return result;
        }

        /// <summary>
        /// 是否包含总成或分总成
        /// </summary>
        /// <param name="listInfo"></param>
        /// <returns></returns>
        public static bool IsContainAssembly(DataRow[] listRow)
        {
            string strSql = @"select Distinct GoodsID from ( select ParentID as GoodsID from dbo.BASE_BomStruct as a "+
                            " inner join View_F_GoodsPlanCost as b on a.ParentID = b.序号 "+
                            " where a.ParentID not in (select GoodsID from F_GoodsAttributeRecord "+
                            " where AttributeID in ("+ (int)GlobalObject.CE_GoodsAttributeName.CVT +","+
                            (int)GlobalObject.CE_GoodsAttributeName.CVT + ") and AttributeValue = '" + bool.TrueString + "') " +
                            " Union all select GoodsID from F_GoodsAttributeRecord  "+
                            " where AttributeID in ("+ (int)GlobalObject.CE_GoodsAttributeName.CVT +","+
                            (int)GlobalObject.CE_GoodsAttributeName.CVT + ") and AttributeValue = '" + bool.TrueString + "' " +
                            " Union all select GoodsID from S_HomemadePartInfo) as a";

            DataTable tempData = GlobalObject.DatabaseServer.QueryInfo(strSql);

            foreach (DataRow dr in listRow)
            {
                foreach (DataRow drCheck in tempData.Rows)
                {
                    if ((int)drCheck["GoodsID"] == (int)dr["物品ID"])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 判断是否为自制件
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>是返回True，否返回False</returns>
        public static bool IsHomemadePart(int goodsID)
        {
            string strSql = "select Count(*) from S_HomemadePartInfo where GoodsID = "+ goodsID;

            if (Convert.ToInt32( GlobalObject.DatabaseServer.QueryInfo(strSql).Rows[0][0]) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获得科室信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="goodsID">科室编码或科室名称</param>
        /// <returns>返回LINQ数据集</returns>
        public static View_Department GetDeptInfo(DepotManagementDataContext ctx, string info)
        {
            var varData = from a in ctx.View_Department
                          where a.部门代码 == info || a.部门名称 == info
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得物品类型
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回物品类型</returns>
        public static CE_GoodsType GetGoodsType(int goodsID, string storageID)
        {
            CE_GoodsType result;

            if (Convert.ToBoolean(UniversalFunction.GetGoodsAttributeInfo(goodsID, CE_GoodsAttributeName.TCU)))
            {
                result = CE_GoodsType.TCU;
            }
            else if (Convert.ToBoolean(UniversalFunction.GetGoodsAttributeInfo(goodsID, CE_GoodsAttributeName.CVT)))
            {
                result = CE_GoodsType.CVT;
            }
            else if (storageID != null && storageID.Trim().Length != 0 && IsGauge(goodsID, storageID))
            {
                result = CE_GoodsType.量检具;
            }
            else if (IsInStandingBook(goodsID, null))
            {
                result = CE_GoodsType.工装;
            }
            else
            {
                result = CE_GoodsType.未知物品;
            }

            return result;
        }

        /// <summary>
        /// 获得基础物品信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <returns>返回LINQ数据集</returns>
        public static View_F_GoodsPlanCost GetGoodsInfo(string goodsCode, string goodsName, string spec)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_F_GoodsPlanCost
                          where a.物品名称 == goodsName
                          && a.图号型号 == goodsCode
                          && a.规格 == spec
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得基础物品信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回LINQ数据集</returns>
        public static View_F_GoodsPlanCost GetGoodsInfo(int goodsID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_F_GoodsPlanCost
                          where a.序号 == goodsID
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得基础物品信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回LINQ数据集</returns>
        public static View_F_GoodsPlanCost GetGoodsInfo(DepotManagementDataContext ctx, int goodsID)
        {
            var varData = from a in ctx.View_F_GoodsPlanCost
                          where a.序号 == goodsID
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得字符串型的物品信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回字符串</returns>
        public static string GetGoodsMessage(DepotManagementDataContext ctx, int goodsID)
        {
            var varData = from a in ctx.F_GoodsPlanCost
                          where a.ID == goodsID
                          select a;

            if (varData.Count() == 0)
            {

                return "系统中无此物品信息";
            }
            else
            {
                return "【图号型号】:" + varData.First().GoodsCode
                    + " 【物品名称】:" + varData.First().GoodsName
                    + " 【规格】:" + varData.First().Spec;
            }
        }

        /// <summary>
        /// 获得字符串型的物品信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回字符串</returns>
        public static string GetGoodsMessage(int goodsID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return GetGoodsMessage(ctx, goodsID);
        }

        /// <summary>
        /// 获得总成或者分总成的BOM表合计零件数量
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="wsCode">车间代码</param>
        /// <returns>返回Tabel</returns>
        public static DataTable GetBomStruct(int goodsID, string wsCode)
        {
            string error = "";

            Hashtable parameters = new Hashtable();

            parameters.Add("@GoodsID", goodsID);
            parameters.Add("@WSCode", wsCode);

            return GlobalObject.DatabaseServer.QueryInfoPro("BASE_GetBomStruct", parameters, out error);
        }

        /// <summary>
        /// 获得库房信息
        /// </summary>
        /// <param name="stroageIDorName">库房ID或库房名称</param>
        /// <returns>返回Linq</returns>
        public static BASE_Storage GetStorageInfo(string stroageIDorName)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.BASE_Storage
                          where a.StorageID == stroageIDorName
                          || a.StorageName == stroageIDorName
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
        /// 获得库房信息
        /// </summary>
        /// <param name="stroageIDorName">库房ID或库房名称</param>
        /// <returns>返回Linq</returns>
        public static View_Base_Storage GetStorageInfo_View(string stroageIDorName)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_Base_Storage
                          where a.库房代码 == stroageIDorName
                          || a.库房名称 == stroageIDorName
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
        /// 根据单据不同获得单据前缀
        /// </summary>
        /// <param name="billType">单据类型</param>
        /// <returns>返回字符串</returns>
        public static string GetBillPrefix(CE_BillTypeEnum billType)
        {
            string strSql = "select * from BASE_BillType where TypeName = '"+ billType.ToString() +"'";

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (tempTable.Rows.Count == 0)
            {
                return "";
            }
            else
            {
                return tempTable.Rows[0]["TypeCode"].ToString();
            }
        }

        /// <summary>
        /// 获得所有部门信息
        /// </summary>
        /// <returns>返回Table</returns>
        public static DataTable GetAllDeptInfo()
        {
            string strSql = "select * from View_HR_Dept where 部门代码 <> '00'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获取明细账操作类型
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <returns>返回明细操作类型数据集</returns>
        public static BASE_SubsidiaryOperationType GetSubsidiaryOperationType(DepotManagementDataContext ctx, int typeID)
        {
            var varData = from a in ctx.BASE_SubsidiaryOperationType
                          where a.OperationType == typeID
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
        /// 获取明细账操作类型
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <returns>返回明细操作类型数据集</returns>
        public static BASE_SubsidiaryOperationType GetSubsidiaryOperationType(int typeID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.BASE_SubsidiaryOperationType
                          where a.OperationType == typeID
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
        /// 查找指定的库存信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="queryInfo">查询条件</param>
        /// <returns>返回查找到的库存信息</returns>
        public static S_Stock GetStockInfo(DepotManagementDataContext context, QueryCondition_Store queryInfo)
        {
            IQueryable<S_Stock> result = null;

            if (queryInfo.StorageID == null)
            {
                if (queryInfo.GoodsCode == null)
                {
                    result = from r in context.S_Stock
                             where r.GoodsID == queryInfo.GoodsID
                             && r.Provider == queryInfo.Provider
                             && r.BatchNo == queryInfo.BatchNo
                             select r;
                }
                else
                {
                    result = from r in context.S_Stock
                             where r.GoodsCode == queryInfo.GoodsCode
                             && r.GoodsName == queryInfo.GoodsName
                             && r.Spec == queryInfo.Spec
                             && r.Provider == queryInfo.Provider
                             && r.BatchNo == queryInfo.BatchNo
                             select r;

                }
            }
            else
            {
                if (queryInfo.GoodsCode == null)
                {
                    if (queryInfo.Provider == null || queryInfo.Provider.Trim() == "")
                    {
                        result = from r in context.S_Stock
                                 where r.GoodsID == queryInfo.GoodsID
                                 && r.BatchNo == queryInfo.BatchNo
                                 && r.StorageID == queryInfo.StorageID
                                 select r;
                    }
                    else
                    {
                        result = from r in context.S_Stock
                                 where r.GoodsID == queryInfo.GoodsID
                                 && r.Provider == queryInfo.Provider
                                 && r.BatchNo == queryInfo.BatchNo
                                 && r.StorageID == queryInfo.StorageID
                                 select r;
                    }
                }
                else
                {
                    if (queryInfo.Provider == null || queryInfo.Provider.Trim() == "")
                    {
                        result = from r in context.S_Stock
                                 where r.GoodsCode == queryInfo.GoodsCode
                                       && r.GoodsName == queryInfo.GoodsName
                                       && r.Spec == queryInfo.Spec
                                       && r.BatchNo == queryInfo.BatchNo
                                       && r.StorageID == queryInfo.StorageID
                                 select r;
                    }
                    else
                    {
                        result = from r in context.S_Stock
                                 where r.GoodsCode == queryInfo.GoodsCode
                                       && r.GoodsName == queryInfo.GoodsName
                                       && r.Spec == queryInfo.Spec
                                       && r.Provider == queryInfo.Provider
                                       && r.BatchNo == queryInfo.BatchNo
                                       && r.StorageID == queryInfo.StorageID
                                 select r;
                    }
                }
            }

            if (result.Count() > 0)
            {
                return result.First();
            }

            return null;
        }

        /// <summary>
        /// 查找指定的库存信息
        /// </summary>
        /// <param name="queryInfo">查询条件</param>
        /// <returns>返回查找到的库存信息</returns>
        public static S_Stock GetStockInfo(QueryCondition_Store queryInfo)
        {
            DepotManagementDataContext context = CommentParameter.DepotDataContext;
            IQueryable<S_Stock> result = null;

            if (queryInfo.StorageID == null)
            {
                if (queryInfo.GoodsCode == null)
                {
                    result = from r in context.S_Stock
                             where r.GoodsID == queryInfo.GoodsID
                             && r.Provider == queryInfo.Provider
                             && r.BatchNo == queryInfo.BatchNo
                             select r;
                }
                else
                {
                    result = from r in context.S_Stock
                             where r.GoodsCode == queryInfo.GoodsCode
                             && r.GoodsName == queryInfo.GoodsName
                             && r.Spec == queryInfo.Spec
                             && r.Provider == queryInfo.Provider
                             && r.BatchNo == queryInfo.BatchNo
                             select r;

                }
            }
            else
            {
                if (queryInfo.GoodsCode == null)
                {
                    if (queryInfo.Provider == null || queryInfo.Provider.Trim() == "")
                    {
                        result = from r in context.S_Stock
                                 where r.GoodsID == queryInfo.GoodsID
                                 && r.BatchNo == queryInfo.BatchNo
                                 && r.StorageID == queryInfo.StorageID
                                 select r;
                    }
                    else
                    {
                        result = from r in context.S_Stock
                                 where r.GoodsID == queryInfo.GoodsID
                                 && r.Provider == queryInfo.Provider
                                 && r.BatchNo == queryInfo.BatchNo
                                 && r.StorageID == queryInfo.StorageID
                                 select r;
                    }
                }
                else
                {
                    if (queryInfo.Provider == null || queryInfo.Provider.Trim() == "")
                    {
                        result = from r in context.S_Stock
                                 where r.GoodsCode == queryInfo.GoodsCode
                                       && r.GoodsName == queryInfo.GoodsName
                                       && r.Spec == queryInfo.Spec
                                       && r.BatchNo == queryInfo.BatchNo
                                       && r.StorageID == queryInfo.StorageID
                                 select r;
                    }
                    else
                    {
                        result = from r in context.S_Stock
                                 where r.GoodsCode == queryInfo.GoodsCode
                                       && r.GoodsName == queryInfo.GoodsName
                                       && r.Spec == queryInfo.Spec
                                       && r.Provider == queryInfo.Provider
                                       && r.BatchNo == queryInfo.BatchNo
                                       && r.StorageID == queryInfo.StorageID
                                 select r;
                    }
                }
            }

            if (result.Count() > 0)
            {
                return result.First();
            }

            return null;
        }

        /// <summary>
        /// 查找指定的库存信息
        /// </summary>
        /// <param name="queryInfo">查询条件</param>
        /// <returns>返回查找到的库存信息</returns>
        public static List<S_Stock> GetStockInfoList(QueryCondition_Store queryInfo)
        {
            DepotManagementDataContext context = CommentParameter.DepotDataContext;
            IQueryable<S_Stock> result = null;

            if (queryInfo.StorageID == null)
            {
                if (queryInfo.GoodsCode == null)
                {
                    result = from r in context.S_Stock
                             where r.GoodsID == queryInfo.GoodsID
                             && r.Provider == queryInfo.Provider
                             && r.BatchNo == queryInfo.BatchNo
                             select r;
                }
                else
                {
                    result = from r in context.S_Stock
                             where r.GoodsCode == queryInfo.GoodsCode
                             && r.GoodsName == queryInfo.GoodsName
                             && r.Spec == queryInfo.Spec
                             && r.Provider == queryInfo.Provider
                             && r.BatchNo == queryInfo.BatchNo
                             select r;

                }
            }
            else
            {
                if (queryInfo.GoodsCode == null)
                {
                    if (queryInfo.Provider == null || queryInfo.Provider.Trim() == "")
                    {
                        result = from r in context.S_Stock
                                 where r.GoodsID == queryInfo.GoodsID
                                 && r.BatchNo == queryInfo.BatchNo
                                 && r.StorageID == queryInfo.StorageID
                                 select r;
                    }
                    else
                    {
                        result = from r in context.S_Stock
                                 where r.GoodsID == queryInfo.GoodsID
                                 && r.Provider == queryInfo.Provider
                                 && r.BatchNo == queryInfo.BatchNo
                                 && r.StorageID == queryInfo.StorageID
                                 select r;
                    }
                }
                else
                {
                    if (queryInfo.Provider == null || queryInfo.Provider.Trim() == "")
                    {
                        result = from r in context.S_Stock
                                 where r.GoodsCode == queryInfo.GoodsCode
                                       && r.GoodsName == queryInfo.GoodsName
                                       && r.Spec == queryInfo.Spec
                                       && r.BatchNo == queryInfo.BatchNo
                                       && r.StorageID == queryInfo.StorageID
                                 select r;
                    }
                    else
                    {
                        result = from r in context.S_Stock
                                 where r.GoodsCode == queryInfo.GoodsCode
                                       && r.GoodsName == queryInfo.GoodsName
                                       && r.Spec == queryInfo.Spec
                                       && r.Provider == queryInfo.Provider
                                       && r.BatchNo == queryInfo.BatchNo
                                       && r.StorageID == queryInfo.StorageID
                                 select r;
                    }
                }
            }

            return result.ToList();
        }

        public static Dictionary<CE_GoodsAttributeName, object> GetGoodsInfList_Attribute_AttchedInfoList(int goodsID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return GetGoodsInfList_Attribute_AttchedInfoList(ctx, goodsID);
        }

        public static Dictionary<CE_GoodsAttributeName, object> GetGoodsInfList_Attribute_AttchedInfoList(DepotManagementDataContext ctx, int goodsID)
        {
            Dictionary<CE_GoodsAttributeName, object> result = new Dictionary<CE_GoodsAttributeName, object>();

            var varData = from a in ctx.F_GoodsPlanCost
                          join b in ctx.F_GoodsAttributeRecord
                          on a.ID equals b.GoodsID
                          where a.ID == goodsID
                          select b;

            foreach (var item in varData)
            {
                if (item.AttributeID == (int)CE_GoodsAttributeName.毛坯 && Convert.ToBoolean(item.AttributeValue))
                {
                    var varData1 = from a in ctx.F_GoodsBlankToProduct
                                   where a.AttributeRecordID == item.AttributeRecordID
                                   select a;

                    result.Add(CE_GoodsAttributeName.毛坯, varData1.ToList());
                }
                else if (item.AttributeID == (int)CE_GoodsAttributeName.替换件 && Convert.ToBoolean(item.AttributeValue))
                {
                    var varData2 = from a in ctx.F_GoodsReplaceInfo
                                   where a.AttributeRecordID == item.AttributeRecordID
                                   select a;

                    result.Add(CE_GoodsAttributeName.替换件, varData2.ToList());
                }
                else if (item.AttributeID == (int)CE_GoodsAttributeName.流水码 && Convert.ToBoolean(item.AttributeValue))
                {
                    var varData3 = from a in ctx.F_ProductWaterCode
                                   where a.AttributeRecordID == item.AttributeRecordID
                                   select a;

                    result.Add(CE_GoodsAttributeName.流水码, varData3.ToList());
                }
                else
                {
                    var varDataTemp = from a in ctx.F_GoodsAttribute
                                      where a.AttributeID == item.AttributeID
                                      select a;
                    if(varDataTemp.Count() == 1)
                    {
                        result.Add(GlobalObject.GeneralFunction.StringConvertToEnum<CE_GoodsAttributeName>(varDataTemp.Single().AttributeName), item.AttributeValue);
                    }
                    
                }
            }

            return result;
        }

        public static IEnumerable<F_GoodsPlanCost> GetGoodsInfoList_Attribute(DepotManagementDataContext ctx, Dictionary<CE_GoodsAttributeName, String> dicCondition)
        {
            IEnumerable<F_GoodsPlanCost> result = from a in ctx.F_GoodsPlanCost
                                                  where a.ID == 0
                                                  select a;

            foreach (KeyValuePair<CE_GoodsAttributeName, String> dic in dicCondition)
            {
                var varData = from a in ctx.F_GoodsAttributeRecord
                              join b in ctx.F_GoodsPlanCost
                              on a.GoodsID equals b.ID
                              where a.AttributeID == (int)dic.Key && a.AttributeValue == dic.Value
                              select b;

                result = result.Union(varData.ToList()).OrderBy(k => k.GoodsCode);
            }

            return result.Distinct();
        }

        public static IEnumerable<F_GoodsPlanCost> GetGoodsInfoList_Attribute(Dictionary<CE_GoodsAttributeName, String> dicCondition)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return GetGoodsInfoList_Attribute(ctx, dicCondition);
        }

        public static IEnumerable<F_GoodsPlanCost> GetGoodsInfoList_Attribute(DepotManagementDataContext ctx, CE_GoodsAttributeName goodsAttributeName, string attributeValue)
        {
            int attributeID = (int)goodsAttributeName;

            var varData = from a in ctx.F_GoodsAttributeRecord
                          join b in ctx.F_GoodsPlanCost
                          on a.GoodsID equals b.ID
                          where a.AttributeID == attributeID && a.AttributeValue == attributeValue
                          select b;


            return varData.AsEnumerable<F_GoodsPlanCost>();
        }

        public static IEnumerable<F_GoodsPlanCost> GetGoodsInfoList_Attribute(CE_GoodsAttributeName goodsAttributeName, string attributeValue)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return GetGoodsInfoList_Attribute(ctx, goodsAttributeName, attributeValue);
        }

        /// <summary>
        /// 获得物品属性信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="goodsAttributeName">属性名称</param>
        /// <returns>返回OBJECT</returns>
        public static object GetGoodsAttributeInfo(DepotManagementDataContext ctx, int goodsID, CE_GoodsAttributeName goodsAttributeName)
        {
            object result = new object();

            var varData = from a in ctx.F_GoodsPlanCost
                          where a.ID == goodsID
                          select a;

            if (varData.Count() != 1)
            {
                return null;
            }

            var varRecord = from a in ctx.F_GoodsAttributeRecord
                            where a.GoodsID == goodsID
                            && a.AttributeID == (int)goodsAttributeName
                            select a;

            if (varRecord.Count() == 1)
            {
                return varRecord.Single().AttributeValue;
            }
            else if (varRecord.Count() == 0)
            {
                return "False";
            }
            else
            {
                throw new Exception("物品属性记录不唯一" + UniversalFunction.GetGoodsMessage(goodsID));
            }
        }

        /// <summary>
        /// 获得物品属性信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="goodsAttributeName">属性名称</param>
        /// <returns>返回OBJECT</returns>
        public static object GetGoodsAttributeInfo(int goodsID, CE_GoodsAttributeName goodsAttributeName)
        {
            object result = new object();

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.F_GoodsPlanCost
                          where a.ID == goodsID
                          select a;

            if (varData.Count() != 1)
            {
                return null;
            }

            var varRecord = from a in ctx.F_GoodsAttributeRecord
                            where a.GoodsID == goodsID
                            && a.AttributeID == (int)goodsAttributeName
                            select a;

            if (varRecord.Count() == 1)
            {
                return varRecord.Single().AttributeValue;
            }
            else if(varRecord.Count() == 0)
            {
                return "False";
            }
            else
            {
                throw new Exception("物品属性记录不唯一" + UniversalFunction.GetGoodsMessage(goodsID));
            }
        }

        public static List<string> GetFromBillNo(CE_BillTypeEnum billType, string billNo)
        {
            List<string> result = new List<string>();
            DataTable tableTemp = new DataTable();
            string strSql = "";

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                if (billNo != null || billNo.Trim().Length > 0)
                {
                    if (ctx.Fun_get_BillNoPrefix_Type(billNo) == billType.ToString())
                    {
                        result.Add(billNo);
                    }
                    else
                    {
                        switch (billType)
                        {
                            case CE_BillTypeEnum.入库申请单:
                                strSql = " select distinct b.BillRelate as BillNo from Business_WarehouseInPut_Requisition as a "+
		                                 " inner join Business_WarehouseInPut_RequisitionDetail as b on a.BillNo = b.BillNo";
                                break;
                            case CE_BillTypeEnum.到货单:
                                break;
                            case CE_BillTypeEnum.入库单:
                                break;
                            case CE_BillTypeEnum.检验报告:
                                break;
                            case CE_BillTypeEnum.判定报告:
                                break;
                            default:
                                break;
                        }
                    }
                }

                tableTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (tableTemp != null)
                {
                    result = DataSetHelper.ColumnsToList_Distinct(tableTemp, "BillNo");
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static Base_MotorFactoryInfo GetMotoFactoryInfo(int factoryID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Base_MotorFactoryInfo
                          where a.ID == factoryID
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else
            {
                return null;
            }
        }

        public static BASE_MaterialRequisitionPurpose GetPurposeInfo(string codeOrName)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.BASE_MaterialRequisitionPurpose
                          where (a.Code == codeOrName || a.Purpose == codeOrName)
                          select a;

            if (varData.Count() > 0)
            {
                return varData.OrderByDescending(k => k.Code).First();
            }
            else
            {
                return null;
            }
        }

        public static HR_AttendanceScheme GetAttendanceSchemeRules(CE_HR_AttendanceScheme attendanceScheme)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.HR_AttendanceScheme
                          where a.SchemeName == attendanceScheme.ToString()
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else
            {
                throw new Exception("数据不唯一或者为空");
            }
        }

        public static string GetYearAndMonth(DateTime date)
        {
            if (date.Day >= Convert.ToInt32( BasicInfo.BaseSwitchInfo[(int)CE_SwitchName.月结日]))
            {
                date = date.AddMonths(1);
            }

            return date.Year.ToString() + date.Month.ToString("D2");
        }

        public static string GetListString<T>(List<T> listInfo, char chr)
        {
            string result = null;

            if (listInfo == null)
            {
                return result;
            }

            switch (typeof(T).Name.ToLower())
            {
                case "int32":

                    foreach (var item in listInfo)
                    {
                        result += item.ToString() + chr.ToString();
                    }

                    break;
                case "string":

                    foreach (var item in listInfo)
                    {
                        result += "'" + item.ToString() + "'" + chr.ToString();
                    }

                    break;
                default:
                    break;
            }

            if (result != null)
            {
                result = result.Substring(0, result.Length - chr.ToString().Length);
            }

            return result;
        }

        public static List<string> GetApplicableMode_CGBOM(int goodsID)
        {
            List<string> lstResult = new List<string>();

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return (from a in ctx.CG_CBOM
                    where a.GoodsID == goodsID
                    && a.Usage > 0
                    select a.Edition).ToList();
        }

        public static List<TCU_CarModelInfo> GetTCUCarModelInfo()
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.TCU_CarModelInfo
                          select a;

            return varData.ToList();
        }

        public static BASE_MaterialRequisitionPurpose GetPurpose(CE_PickingPurposeProperty pickType)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.BASE_MaterialRequisitionPurpose
                          select a;

            switch (pickType)
            {
                case CE_PickingPurposeProperty.破坏性检测:
                    varData = from a in varData
                              where a.DestructiveInspection == true
                              select a;
                    break;
                case CE_PickingPurposeProperty.三包外装配:
                    varData = from a in varData
                              where a.ThreeOutSideFit == true
                              select a;
                    break;
                case CE_PickingPurposeProperty.三包外维修:
                    varData = from a in varData
                              where a.ThreeOutSideRepair == true
                              select a;
                    break;
                case CE_PickingPurposeProperty.盘点:
                    varData = from a in varData
                              where a.Inventory == true
                              select a;
                    break;
                default:
                    break;
            }

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else
            {
                throw new Exception("无法获得【"+ pickType.ToString() +"】对应的用途");
            }
        }

        public static DataTable ClearTable_Price(DataTable dtSource)
        {
            DataTable tempTable = dtSource.Copy();

            List<string> lstColName = new List<string>();

            foreach (DataColumn dc in tempTable.Columns)
            {
                if (dc.ColumnName.Contains("金额") || dc.ColumnName.Contains("单价") || dc.ColumnName.Contains("税率"))
                {
                    lstColName.Add(dc.ColumnName);
                }
            }

            foreach (string dcName in lstColName)
            {
                tempTable.Columns.Remove(dcName);
            }

            return tempTable;
        }
    }
}
