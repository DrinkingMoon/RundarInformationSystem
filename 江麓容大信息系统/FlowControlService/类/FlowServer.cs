/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FlowServer.cs
 * 作者    :  曹津彬    版本: v1.00    日期: 2014/04/2
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
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using GlobalObject;
using PlatformManagement;
using ServerModule;

namespace FlowControlService
{
    enum OrderFlagEnum
    {
        Now,

        Next
    }

    /// <summary>
    /// 流程服务组件
    /// </summary>
    class FlowServer : IFlowServer
    {
        #region GetInfo

        public List<string> GetBusinessInfoVersion(CE_BillTypeEnum billType)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Flow_BusinessInfo
                          where a.BusinessTypeName == billType.ToString()
                          orderby a.Version descending
                          select a.Version;

            return varData.ToList();
        }

        /// <summary>
        /// 获得流程信息
        /// </summary>
        /// <param name="billType">单据类型</param>
        /// <returns>返回流程信息</returns>
        Flow_BusinessInfo GetBusinessInfo(DepotManagementDataContext ctx, CE_BillTypeEnum billType)
        {
            var varData = from a in ctx.Flow_BusinessInfo
                          where a.BusinessTypeName == billType.ToString()
                          orderby a.Version descending
                          select a;

            if (varData == null || varData.Count() == 0)
            {
                return null;
            }
            else
            {
                return varData.First();
            }
        }

        /// <summary>
        /// 获得流程信息
        /// </summary>
        /// <param name="billType">单据类型</param>
        /// <param name="version">版本号</param>
        /// <returns>返回流程信息</returns>
        public Flow_BusinessInfo GetBusinessInfo(CE_BillTypeEnum billType, string version)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            return GetBusinessInfo(ctx, billType, version);
        }

        /// <summary>
        /// 获得流程信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billType">单据类型</param>
        /// <param name="version">版本号</param>
        /// <returns>返回流程信息</returns>
        Flow_BusinessInfo GetBusinessInfo(DepotManagementDataContext ctx, CE_BillTypeEnum billType, string version)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(version))
            {
                return GetBusinessInfo(ctx, billType);
            }

            var varData = from a in ctx.Flow_BusinessInfo
                          where a.BusinessTypeName == billType.ToString()
                          && a.Version == version
                          select a;

            if (varData == null || varData.Count() == 0)
            {
                return null;
            }
            else if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else
            {
                throw new Exception("流程业务清单有误");
            }
        }

        /// <summary>
        /// 获得每个流程节点的信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回信息列表</returns>
        public List<Flow_FlowData> GetBusinessOperationInfo(string billNo)
        {
            List<Flow_FlowInfo> listTemp = GetListFlowInfo(billNo).OrderBy(k => k.FlowOrder).ToList();
            List<Flow_FlowData> listTemp1 = new List<Flow_FlowData>();
            List<Flow_FlowData> listResult = new List<Flow_FlowData>();

            foreach (Flow_FlowInfo item in listTemp)
            {
                listTemp1 = GetBusinessOperationInfo(billNo, item.FlowID).OrderByDescending(k => k.OperationTime).ToList(); 

                if (listTemp == null || listTemp.Count == 0)
                {
                    throw new Exception("获取流程节点信息失败");
                }
                else
                {
                    if (listTemp1.Count > 0)
                    {
                        listResult.Add(listTemp1.First());
                    }
                }
            }

            return listResult;
        }

        /// <summary>
        /// 获得当前业务流程信息对象
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回对象</returns>
        public Flow_FlowInfo GetNowFlowInfo(string billNo)
        {
            int? businessTypeID = GetBusinessTypeID(billNo);

            if (businessTypeID == null)
            {
                return null;
            }
            else
            {
                return GetNowFlowInfo((int)businessTypeID, billNo);
            }
        }

        /// <summary>
        /// 获得当前业务流程信息对象
        /// </summary>
        /// <param name="businessTypeID">业务ID</param>
        /// <param name="billNo">单据号</param>
        /// <returns>返回对象</returns>
        public Flow_FlowInfo GetNowFlowInfo(int businessTypeID, string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Flow_FlowBillData
                          join b in ctx.Flow_FlowInfo
                          on a.FlowID equals b.FlowID
                          where a.BusinessTypeID == businessTypeID
                          && a.BillNo == billNo
                          select b;

            if (varData == null || varData.Count() == 0)
            {
                List<Flow_FlowInfo> listInfo = GetListFlowInfo(businessTypeID);

                if (listInfo != null && listInfo.Count > 0)
                {
                    return listInfo.First();
                }
                else
                {
                    throw new Exception("请设置此单据类型的业务流程后再操作");
                }
            }
            else
            {
                return varData.Single();
            }
        }

        /// <summary>
        /// 获得某个操作节点的操作信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="businessStatus">业务状态</param>
        /// <returns>返回List</returns>
        public List<Flow_FlowData> GetBusinessOperationInfo(string billNo, string businessStatus)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            List<string> result = new List<string>();

            List<Flow_FlowData> listData = GetFlowDataInfo(ctx, billNo);

            if (listData == null)
            {
                return null;
            }
            else
            {
                var varData = from a in listData
                              join b in ctx.Flow_FlowInfo
                              on a.FlowID equals b.FlowID
                              where b.BusinessStatus == businessStatus
                              select a;

                if (varData == null)
                {
                    return null;
                }
                else
                {
                    return varData.ToList();
                }
            }
        }

        /// <summary>
        /// 获得某个操作节点的操作信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="flowID">流程ID</param>
        /// <returns>返回List</returns>
        List<Flow_FlowData> GetBusinessOperationInfo(DepotManagementDataContext ctx, string billNo, int flowID)
        {
            List<string> result = new List<string>();
            List<Flow_FlowData> listData = GetFlowDataInfo(ctx, billNo);

            if (listData == null)
            {
                return null;
            }
            else
            {
                var varData = from a in listData
                              join b in ctx.Flow_FlowInfo
                              on a.FlowID equals b.FlowID
                              where b.FlowID == flowID
                              select a;

                if (varData == null)
                {
                    return null;
                }
                else
                {
                    return varData.ToList();
                }
            }
        }
        
        /// <summary>
        /// 获得某个操作节点的操作信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="flowID">流程ID</param>
        /// <returns>返回List</returns>
        public List<Flow_FlowData> GetBusinessOperationInfo(string billNo, int flowID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            return GetBusinessOperationInfo(ctx, billNo, flowID);
        }

        /// <summary>
        /// 判断单据是否存在
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回True,不存在返回False</returns>
        public bool IsExist(string billNo)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                CheckData(ctx, billNo);
                Flow_FlowBillData billInfo = GetBillData(ctx, billNo);

                if (billInfo == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 获得单据状态
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回字符串</returns>
        public string GetNowBillStatus(string billNo)
        {
            int? businessTypeID = GetBusinessTypeID(billNo);

            if (businessTypeID == null)
            {
                return null;
            }
            else
            {
                return GetBillStatus((int)businessTypeID, billNo);
            }
        }

        /// <summary>
        /// 获得下一个流程的状态
        /// </summary>
        /// <param name="billNo"></param>
        /// <returns></returns>
        public string GetNextBillStatus(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            int? businessTypeID = GetBusinessTypeID(billNo);

            if (businessTypeID == null)
            {
                return null;
            }
            else
            {
                Flow_FlowInfo tempFlow = GetNextFlowInfo(ctx, (int)businessTypeID, billNo);

                if (tempFlow == null)
                {
                    return null;
                }
                else
                {
                    return tempFlow.BusinessStatus;
                }
            }
        }

        /// <summary>
        /// 获得业务流程信息对象列表
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回对象列表</returns>
        public List<Flow_FlowInfo> GetListFlowInfo(string billNo)
        {
            int? businessTypeID = GetBusinessTypeID(billNo);

            if (businessTypeID != null)
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
                string error = "";

                Hashtable tempHash = new Hashtable();
                tempHash.Add("@BillNo", billNo == null ? "" : billNo);
                tempHash.Add("@BusinessTypeID", businessTypeID);

                DataTable tempTable = GlobalObject.DatabaseServer.QueryInfoPro("BASE_Flow_GetFlowInfo", tempHash, out error);

                var varData = from a in ctx.Flow_FlowInfo
                              join b in ctx.Flow_FlowExecuteTemp
                              on a.FlowID equals b.FlowID
                              select a;

                return varData.ToList();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 是否要指定人或者角色
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="isParallel">是否并行</param>
        /// <returns>需要返回True,不需要返回False</returns>
        public bool IsPointPersonnel(string billNo, out bool isParallel)
        {
            isParallel = false;
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            int? businessTypeID = GetBusinessTypeID(billNo);

            if (businessTypeID == null)
            {
                return false;
            }
            else
            {
                Flow_FlowInfo nowFlowInfo = GetNowFlowInfo(ctx, (int)businessTypeID, billNo);
                Flow_FlowInfo nextFlowInfo = GetNextFlowInfo(ctx, (int)businessTypeID, billNo);

                if (nowFlowInfo.FlowID == nextFlowInfo.FlowID)
                {
                    return false;
                }

                List<Flow_FlowExecuteInfo> lstFlowExecuteInfo = GetBusinessTypeExecuteList((int)businessTypeID);

                var varData = from a in lstFlowExecuteInfo
                              where a.FlowID == nextFlowInfo.FlowID
                              select a;

                if (varData == null || varData.Count() == 0)
                {
                    return false;
                }
                else
                {
                    foreach (Flow_FlowExecuteInfo item in varData)
                    {
                        if ((item.RoleName == null || item.RoleName.ToString().Trim().Length == 0) 
                            &&(item.RoleStyleName == null || item.RoleStyleName.ToString().Trim().Length == 0))
                        {
                            isParallel = (bool)item.IsParallel;
                            return true;
                        }
                    }

                    return false;
                }
            }
        }

        /// <summary>
        /// 获得执行流程
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="businessTypeID"></param>
        /// <returns>返回字典：流程顺序，流程名，是否已执行标志</returns>
        public Dictionary<int, Dictionary<string, bool>> GetExcuteFlowInfo(string billNo, int businessTypeID)
        {
            string error = "";
            Dictionary<int, Dictionary<string, bool>> tempResult = new Dictionary<int, Dictionary<string, bool>>();
            Hashtable tempHash = new Hashtable();

            tempHash.Add("@BillNo", billNo == null ? "" : billNo);
            tempHash.Add("@BusinessTypeID", businessTypeID);

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfoPro("BASE_Flow_GetExcuteInfo", tempHash, out error);

            var varData = (from a in tempTable.AsEnumerable()
                           select new
                           {
                               FlowOrder = a.Field<int>("FlowOrder")
                           }).Distinct();

            foreach (var item in varData)
            {
                Dictionary<string, bool> tempDicList = new Dictionary<string, bool>();

                var varTempData = from a in
                                      (from a in tempTable.AsEnumerable()
                                       select new
                                       {
                                           FlowOrder = a.Field<int>("FlowOrder"),
                                           WorkName = a.Field<string>("WorkName"),
                                           FlowName = a.Field<string>("FlowName"),
                                           ExecuteFlag = a.Field<bool>("ExecuteFlag")
                                       })
                                  where a.FlowOrder == item.FlowOrder
                                  select a;

                foreach (var item1 in varTempData)
                {
                    if (item.FlowOrder == 1 && item1.WorkName == null)
                    {
                        tempDicList.Add(BasicInfo.LoginName + "\r\n(" + item1.FlowName.ToString() + ")", true);
                    }
                    else if (item1.WorkName == "0" || item1.WorkName == "1")
                    {
                        tempDicList.Add("待指定" + "\r\n(" + item1.FlowName.ToString() + ")", false);
                    }
                    else
                    {
                        string workName = "";

                        if (item1.WorkName == null)
                        {
                            workName = "完成";
                        }
                        else
                        {
                            workName = item1.WorkName.ToString();

                            if (workName.Contains("{") && workName.Contains("}"))
                            {
                                workName = workName.Replace("{", "");
                                workName = workName.Replace("}", "");

                                workName = UniversalFunction.GetDeptName(workName);
                            }
                        }

                        string temp = string.Format("{0}\r\n({1})", workName, item1.FlowName.ToString());

                        tempDicList.Add(temp, Convert.ToBoolean(item1.ExecuteFlag));
                    }
                }

                tempResult.Add(item.FlowOrder, tempDicList);
            }

            return tempResult;
        }

        /// <summary>
        /// 获得业务ID
        /// </summary>
        /// <param name="billType">业务类型</param>
        /// <param name="version">版本号，可为空</param>
        /// <returns>返回业务ID</returns>
        public int GetBusinessTypeID(CE_BillTypeEnum billType, string version)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return GetBusinessTypeID(ctx, billType, version);
        }

        /// <summary>
        /// 获得业务ID
        /// </summary>
        /// <param name="billType">业务类型</param>
        /// <param name="version">版本号，可为空</param>
        /// <returns>返回业务ID</returns>
        int GetBusinessTypeID(DepotManagementDataContext ctx, CE_BillTypeEnum billType, string version)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(version))
            {
                var varData = from a in ctx.Flow_BusinessInfo
                              where a.BusinessTypeName == billType.ToString()
                              orderby a.Version descending
                              select a;

                if (varData.Count() > 0)
                {
                    return varData.First().BusinessTypeID;
                }
            }
            else
            {
                var varData = from a in ctx.Flow_BusinessInfo
                              where a.BusinessTypeName == billType.ToString()
                              && a.Version == version
                              select a;

                if (varData.Count() > 0)
                {
                    return varData.First().BusinessTypeID;
                }
            }

            return 0;
        }

        /// <summary>
        /// 获得历史操作信息对象列表
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回对象列表</returns>
        public List<CommonProcessInfo> GetFlowData(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            List<CommonProcessInfo> lstResult = new List<CommonProcessInfo>();

            var varData = from a in ctx.Flow_FlowData
                          join b in ctx.Flow_FlowInfo
                          on a.FlowID equals b.FlowID
                          join c in ctx.Flow_FlowBillData
                          on a.FlowBillID equals c.FlowBillID
                          where c.BillNo == billNo
                          orderby a.OperationTime descending
                          select a;

            if (varData != null)
            {
                foreach (Flow_FlowData item in varData)
                {
                    CommonProcessInfo tempLnq = new CommonProcessInfo();

                    Flow_FlowInfo tempFlowInfo = GetFlowInfo(ctx, item.FlowID);

                    if (tempFlowInfo != null)
                    {
                        tempLnq.操作节点 = tempFlowInfo.FlowName;
                    }

                    tempLnq.人员 = UniversalFunction.GetPersonnelName(item.OperationPersonnel);
                    tempLnq.时间 = item.OperationTime.ToString();
                    tempLnq.意见 = item.Advise;
                    tempLnq.工号 = item.OperationPersonnel;

                    lstResult.Add(tempLnq);
                }
            }

            return lstResult;
        }

        /// <summary>
        /// 获得业务状态列表
        /// </summary>
        /// <param name="billType">业务类型</param>
        /// <param name="version">版本号 可为空</param>
        /// <returns>返回列表</returns>
        public List<string> GetBusinessStatus(CE_BillTypeEnum billType, string version)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            List<string> lstResult = new List<string>();

            lstResult.Add("全部");

            int businessTypeID = GetBusinessTypeID(ctx, billType, version);

            var varFlowInfo = from a in ctx.Flow_FlowInfo
                              where a.BusinessTypeID == businessTypeID
                              orderby a.FlowOrder
                              select a;

            if (varFlowInfo != null)
            {
                foreach (Flow_FlowInfo item in varFlowInfo)
                {
                    lstResult.Add(item.BusinessStatus);
                }
            }

            return lstResult;
        }

        /// <summary>
        /// 获得对应业务的单据信息
        /// </summary>
        /// <param name="billType">业务类型</param>
        /// <param name="version">版本号</param>
        /// <param name="lstbusinessStatus"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="findType"></param>
        /// <param name="billNo"></param>
        /// <returns>返回Table</returns>
        public DataTable ShowBusinessAllInfo(CE_BillTypeEnum billType, string version, 
            string[] lstbusinessStatus, DateTime startDate, DateTime endDate, string findType, string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            DataTable resultTable = new DataTable();

            int businessTypeID = GetBusinessTypeID(ctx, billType, version);

            var varFlowInfo = from a in ctx.Flow_FlowInfo
                              where a.BusinessTypeID == businessTypeID
                              orderby a.FlowOrder
                              select a;

            string businessStatus = "";

            if (lstbusinessStatus == null || lstbusinessStatus.Count() == 0)
            {
                throw new Exception("获取业务状态失败");
            }

            foreach (string status in lstbusinessStatus)
            {
                businessStatus += status + ",";
            }

            if (varFlowInfo != null && varFlowInfo.Count() > 0)
            {
                Hashtable tempHash = new Hashtable();
                tempHash.Add("@BusinessTypeID", businessTypeID);
                tempHash.Add("@StartDate", startDate.Date);
                tempHash.Add("@EndDate", Convert.ToDateTime(endDate.Date.ToShortDateString() + " 23:59:59"));
                tempHash.Add("@BusinessStatus", businessStatus);
                tempHash.Add("@WorkID", BasicInfo.LoginID);
                tempHash.Add("@FindType", findType);
                tempHash.Add("@BillNo", billNo == null ? DBNull.Value : (object)billNo);

                string error = "";
                resultTable = GlobalObject.DatabaseServer.QueryInfoPro("BASE_Flow_GetAllBusinessInfo", tempHash, out error);
            }

            return resultTable;
        }

        /// <summary>
        /// 判断流程是否为业务的最后流程
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="flowInfo">流程对象</param>
        /// <returns>是 返回True,否 返回 False</returns>
        bool IsFinalStep(DepotManagementDataContext ctx, Flow_FlowInfo flowInfo)
        {
            if (flowInfo == null)
            {
                return false;
            }

            var varData = from a in ctx.Flow_FlowInfo
                          where a.BusinessTypeID == flowInfo.BusinessTypeID
                          select a;

            if (varData.Count() == 0)
            {
                return false;
            }
            else
            {
                varData = from a in varData
                          where a.FlowOrder > flowInfo.FlowOrder
                          select a;

                if (varData.Count() == 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获得并行知会人员
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="operatorFlowInfo">执行流程对象</param>
        /// <param name="billNo">单据号</param>
        /// <param name="operationType">操作类型</param>
        /// <returns>返回工号列表</returns>
        List<string> GetParallelNotifyPersonnel(DepotManagementDataContext ctx, Flow_FlowInfo operatorFlowInfo, 
            string billNo, CE_FlowOperationType operationType)
        {
            List<string> listResult = new List<string>();

            if (operatorFlowInfo == null || billNo == null)
            {
                return listResult;
            }

            List<Flow_FlowExecuteInfo> listTempNowExceuteInfo = GetBillAuthority(ctx, OrderFlagEnum.Now, operatorFlowInfo.BusinessTypeID, operatorFlowInfo.FlowOrder, billNo);

            if (listTempNowExceuteInfo != null)
            {
                foreach (Flow_FlowExecuteInfo nowAuthority in listTempNowExceuteInfo)
                {
                    if (nowAuthority.IsParallel != null)
                    {
                        var varExecuteInfoPersonnel = from a in ctx.Flow_FlowExecuteInfoPersonnel
                                                      where a.BillNo == billNo && a.FlowID == nowAuthority.FlowID
                                                      select a;

                        foreach (Flow_FlowExecuteInfoPersonnel personnel in varExecuteInfoPersonnel)
                        {
                            if (!(bool)nowAuthority.IsParallel)
                            {
                                var varNotifyPersonnel = from a in ctx.Flow_FlowData
                                                         join b in ctx.Flow_FlowBillData
                                                         on a.FlowBillID equals b.FlowBillID
                                                         where !a.IsBack && a.OperationPersonnel == personnel.WorkID
                                                         && (a.Advise.Substring(0, 2) == "已阅" || a.Advise.Substring(0, 2) == "同意")
                                                         && a.FlowID == nowAuthority.FlowID && b.BillNo == billNo
                                                         select a;

                                if (varNotifyPersonnel.Count() == 0)
                                {
                                    listResult.Add(personnel.WorkID.ToString());
                                }
                            }
                            else
                            {
                                listResult.Add(personnel.WorkID.ToString());
                            }
                        }
                    }
                }
            }

            if (listResult != null && listResult.Count() > 0 && operationType == CE_FlowOperationType.提交)
            {
                listResult.Remove(BasicInfo.LoginID);
            }

            return listResult;
        }

        /// <summary>
        /// 获得单据状态
        /// </summary>
        /// <param name="businessTypeID">业务ID</param>
        /// <param name="billNo">单据号</param>
        /// <returns>返回字符串</returns>
        string GetBillStatus(int businessTypeID, string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            Flow_FlowInfo operatorFlowInfo = GetNowFlowInfo(ctx, businessTypeID, billNo);

            if (operatorFlowInfo == null)
            {
                return "";
            }

            return operatorFlowInfo.BusinessStatus;
        }

        /// <summary>
        /// 获得业务ID
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回可为空的INT型业务ID</returns>
        int? GetBusinessTypeID(DepotManagementDataContext ctx, string billNo)
        {
            if (billNo == null || billNo.Trim().Length == 0)
            {
                return null;
            }

            int? businessTypeID = null;

            string billPrix = GlobalObject.GeneralFunction.ScreenString(CE_ScreenType.字母, billNo);

            var varData = from a in ctx.BASE_BillType
                          join b in ctx.Flow_BusinessInfo
                          on a.TypeName equals b.BusinessTypeName
                          where a.TypeCode == billPrix
                          orderby b.Version descending
                          select b.BusinessTypeID;

            if (varData.Count() > 0)
            {
                businessTypeID = Convert.ToInt32(varData.ToList().First());
            }

            return businessTypeID;
        }

        /// <summary>
        /// 获得业务ID
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回可为空的INT型业务ID</returns>
        int? GetBusinessTypeID(string billNo)
        {
            if (billNo == null || billNo.Trim().Length == 0)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return GetBusinessTypeID(ctx, billNo);
        }

        /// <summary>
        /// 检测业务
        /// </summary>
        /// <param name="businessTypeID">业务ID</param>
        void CheckData(int businessTypeID)
        {
            if (businessTypeID == 0)
            {
                throw new Exception("不存在此单据类型");
            }
            else
            {
                List<Flow_FlowInfo> listTemp = GetListFlowInfo(businessTypeID);

                if (listTemp == null || listTemp.Count == 0)
                {
                    throw new Exception("请设置此单据类型的业务流程后再操作");
                }
            }
        }

        /// <summary>
        /// 检测单据号
        /// </summary>
        /// <param name="billNo">单据号</param>
        void CheckData(DepotManagementDataContext ctx, string billNo)
        {
            int? businessTypeID = GetBusinessTypeID(ctx, billNo);

            if (businessTypeID == null)
            {
                throw new Exception("不存在此单据类型");
            }
            else
            {
                List<Flow_FlowInfo> listTemp = GetListFlowInfo(ctx, (int)businessTypeID);

                if (listTemp == null || listTemp.Count == 0)
                {
                    throw new Exception("请设置此单据类型的业务流程后再操作");
                }
            }
        }

        /// <summary>
        /// 获得流程单据信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回流程单据信息对象</returns>
        public Flow_FlowBillData GetBillData(string billNo)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                return GetBillData(ctx, billNo);
            }
        }

        /// <summary>
        /// 获得流程单据信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <returns>返回流程单据信息对象</returns>
        public Flow_FlowBillData GetBillData(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.Flow_FlowBillData
                          where a.BillNo == billNo
                          select a;

            if (varData == null || varData.Count() == 0)
            {
                return null;
            }
            else
            {
                return varData.Single();
            }
        }

        /// <summary>
        /// 获得业务对象
        /// </summary>
        /// <param name="businessTypeID">业务ID</param>
        /// <returns>返回业务对象</returns>
        Flow_BusinessInfo GetBusinessInfo(int businessTypeID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Flow_BusinessInfo
                          where a.BusinessTypeID == businessTypeID
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else
            {
                throw new Exception("无此业务类型");
            }
        }

        /// <summary>
        /// 获得操作信息对象列表
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <returns>返回对象列表</returns>
        List<Flow_FlowData> GetFlowDataInfo(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.Flow_FlowData
                          join b in ctx.Flow_FlowBillData
                          on a.FlowBillID equals b.FlowBillID
                          where b.BillNo == billNo
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 获得逻辑运算对象列表
        /// </summary>
        /// <param name="flowID">流程ID</param>
        /// <param name="isParallel">是否并行标志</param>
        /// <returns>返回对象列表</returns>
        List<Flow_FlowLogicalAssociation> GetFlowIDLogicalListInfo(int flowID, bool isParallel)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varLogicalAssociation = from a in ctx.Flow_FlowLogicalAssociation
                                        where a.FlowID == flowID
                                        && a.IsParallel == isParallel
                                        select a;

            return varLogicalAssociation.ToList();
        }

        /// <summary>
        /// 获得业务流程信息对象列表
        /// </summary>
        /// <param name="businessTypeID">业务ID</param>
        /// <returns>返回对象列表</returns>
        List<Flow_FlowInfo> GetListFlowInfo(int businessTypeID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return GetListFlowInfo(ctx, businessTypeID);
        }

        /// <summary>
        /// 获得业务流程信息对象列表
        /// </summary>
        /// <param name="businessTypeID">业务ID</param>
        /// <returns>返回对象列表</returns>
        List<Flow_FlowInfo> GetListFlowInfo(DepotManagementDataContext ctx, int businessTypeID)
        {
            var varFlowInfo = from a in ctx.Flow_FlowInfo
                              where a.BusinessTypeID == businessTypeID
                              orderby a.FlowOrder
                              select a;

            if (varFlowInfo == null)
            {
                return null;
            }
            else
            {
                return varFlowInfo.ToList();
            }
        }
        
        /// <summary>
        /// 获得业务流程信息对象
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="businessTypeID">业务ID</param>
        /// <param name="orderID">流程顺序ID</param>
        /// <returns>返回对象</returns>
        Flow_FlowInfo GetFlowInfo(DepotManagementDataContext ctx, int businessTypeID, int orderID)
        {
            var varData = from a in ctx.Flow_FlowInfo
                          where a.FlowOrder == orderID
                          && a.BusinessTypeID == businessTypeID
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
        /// 获得业务流程信息对象
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="flowID">流程ID</param>
        /// <returns>返回对象</returns>
        Flow_FlowInfo GetFlowInfo(DepotManagementDataContext ctx, int flowID)
        {
            var varData = from a in ctx.Flow_FlowInfo
                          where a.FlowID == flowID
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
        /// 获得当前业务流程信息对象
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="businessTypeID">业务ID</param>
        /// <param name="billNo">单据号</param>
        /// <returns>返回对象</returns>
        Flow_FlowInfo GetNowFlowInfo(DepotManagementDataContext ctx, int businessTypeID, string billNo)
        {
            var varData = from a in ctx.Flow_FlowBillData
                          join b in ctx.Flow_FlowInfo
                          on a.FlowID equals b.FlowID
                          where a.BusinessTypeID == businessTypeID
                          && a.BillNo == billNo
                          select b;

            if (varData == null || varData.Count() == 0)
            {
                List<Flow_FlowInfo> listInfo = GetListFlowInfo(businessTypeID);

                if (listInfo != null && listInfo.Count > 0)
                {
                    return listInfo.First();
                }
                else
                {
                    throw new Exception("请设置此单据类型的业务流程后再操作");
                }
            }
            else
            {
                return varData.Single();
            }
        }

        /// <summary>
        /// 获得指定人员或者角色信息列表
        /// </summary>
        /// <param name="notifyPersonnel"></param>
        /// <returns></returns>
        List<string> GetListNotifyPersonnel(NotifyPersonnelInfo notifyPersonnel)
        {
            List<string> tempNotifyList = new List<string>();

            if (notifyPersonnel != null)
            {
                BillFlowMessage_ReceivedUserType userType = notifyPersonnel.UserType == null ? BillFlowMessage_ReceivedUserType.角色 :
                    GlobalObject.GeneralFunction.StringConvertToEnum<BillFlowMessage_ReceivedUserType>(notifyPersonnel.UserType);

                if (notifyPersonnel.PersonnelBasicInfoList != null)
                {
                    switch (userType)
                    {
                        case BillFlowMessage_ReceivedUserType.用户:
                            foreach (PersonnelBasicInfo pbi in notifyPersonnel.PersonnelBasicInfoList)
                            {
                                if (pbi.工号 != null && pbi.工号.Length > 0)
                                {
                                    tempNotifyList.Add(pbi.工号);
                                }
                            }
                            break;

                        case BillFlowMessage_ReceivedUserType.角色:
                            foreach (PersonnelBasicInfo pbi in notifyPersonnel.PersonnelBasicInfoList)
                            {
                                if (pbi.角色 != null && pbi.角色.Length > 0)
                                {
                                    //tempNotifyList.Add(pbi.角色);

                                    string roleCode = PlatformManagement.PlatformFactory.GetRoleManagement().GetRoleViewFromRoleName(
                                        new List<string>(new string[] { pbi.角色}))[0].角色编码;

                                    IQueryable<PlatformManagement.View_Auth_User> lnqUsers =
                                        PlatformManagement.PlatformFactory.GetUserManagement().GetUsers(roleCode);

                                    foreach (View_Auth_User user in lnqUsers)
                                    {
                                        tempNotifyList.Add(user.登录名);
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            return tempNotifyList.Distinct().ToList();
        }

        /// <summary>
        /// 判断是否为逻辑处理流程传递
        /// </summary>
        /// <param name="fla"></param>
        /// <param name="primaryKeyValue"></param>
        /// <returns></returns>
        bool IsLogicalPass(Flow_FlowLogicalAssociation fla, string primaryKeyValue)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Flow_FlowLogicalInfo
                          where a.FlowAssociationID == fla.FlowAssociationID
                          select a;

            if (varData.Count() == 0)
            {
                return false;
            }
            else
            {
                if (fla.IsParallel)
                {
                    foreach (Flow_FlowLogicalInfo fli in varData)
                    {
                        if (IsLogicalInfoPass(fli, primaryKeyValue))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    foreach (Flow_FlowLogicalInfo fli in varData)
                    {
                        if (!IsLogicalInfoPass(fli, primaryKeyValue))
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 判断是否有逻辑信息
        /// </summary>
        /// <param name="fli"></param>
        /// <param name="primaryKeyValue"></param>
        /// <returns></returns>
        bool IsLogicalInfoPass(Flow_FlowLogicalInfo fli, string primaryKeyValue)
        {
            string strSql = "select * from "+ fli.AssociationTable 
                + " where "+ fli.PrimaryKeyName + " = '" + primaryKeyValue + "' and (" + fli.LogicalCondition + ")";

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (tempTable == null || tempTable.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 获得下一个流程逻辑流程信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="businessTypeID">业务类型ID</param>
        /// <param name="flowInfo">当前流程信息</param>
        /// <param name="billNo">单据号</param>
        /// <returns>返回流程信息</returns>
        Flow_FlowInfo GetNextLogicalFlowInfo(DepotManagementDataContext ctx, int businessTypeID, Flow_FlowInfo flowInfo, string billNo)
        {
            Flow_FlowInfo result = GetFlowInfo(ctx, businessTypeID, flowInfo.FlowOrder + 1);

            List<Flow_FlowLogicalAssociation> tempLogicalList = GetFlowIDLogicalListInfo(result.FlowID, false);

            if (tempLogicalList == null || tempLogicalList.Count == 0)
            {
                return result;
            }
            else
            {
                foreach (Flow_FlowLogicalAssociation fla in tempLogicalList)
                {
                    if (IsLogicalPass(fla, billNo))
                    {
                        result = GetNextLogicalFlowInfo(ctx, businessTypeID, result, billNo);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// 获得下一业务流程信息对象
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="businessTypeID">业务ID</param>
        /// <param name="nowOrder">当前业务流程顺序ID</param>
        /// <param name="billNo">业务编号</param>
        /// <returns>返回对象</returns>
        Flow_FlowInfo GetNextFlowInfo(DepotManagementDataContext ctx, int businessTypeID, int nowOrder, string billNo)
        {
            Flow_BusinessInfo tempBusinessInfo = GetBusinessInfo(businessTypeID);
            Flow_FlowInfo nowFlowInfo = GetFlowInfo(ctx, businessTypeID, nowOrder);

            if (nowOrder == 0)
            {
                Flow_FlowInfo tempFirst = GetFlowInfo(ctx, businessTypeID, 1);

                if (tempFirst != null)
                {
                    return tempFirst;
                }
                else
                {
                    throw new Exception("【" + tempBusinessInfo.BusinessTypeName + "】业务未设置流程起始步骤");
                }
            }
            else
            {
                List<Flow_FlowInfo> tempList = GetListFlowInfo(businessTypeID).OrderByDescending(t => t.FlowOrder).ToList();

                if (nowOrder != tempList.First().FlowOrder)
                {
                    //实际业务逻辑判断流程
                    List<Flow_FlowLogicalAssociation> tempLogicalList = 
                        GetFlowIDLogicalListInfo(GetFlowInfo(ctx, businessTypeID, nowFlowInfo.FlowOrder + 1).FlowID, false);

                    if (tempLogicalList != null && tempLogicalList.Count > 0)
                    {
                        return GetNextLogicalFlowInfo(ctx, businessTypeID, nowFlowInfo, billNo);
                    }

                    //多角色多指定人_串行流程
                    List<Flow_FlowExecuteInfo> listTempNowExceuteInfo = 
                        GetBillAuthority(ctx, OrderFlagEnum.Now, nowFlowInfo.BusinessTypeID, nowFlowInfo.FlowOrder, billNo);

                    if (listTempNowExceuteInfo != null && listTempNowExceuteInfo.Count > 0 
                        && listTempNowExceuteInfo.Where(k => k.RoleName == null && k.RoleStyleName == null && k.IsParallel == true).Count() > 0)
                    {
                        List<string> lstParallelPersonnel = GetParallelNotifyPersonnel(ctx, nowFlowInfo, billNo, CE_FlowOperationType.提交);

                        if (lstParallelPersonnel != null && lstParallelPersonnel.Count > 0)
                        {
                            return nowFlowInfo;
                        }
                    }

                    //正常单一流程
                    Flow_FlowInfo tempFlowInfo = GetFlowInfo(ctx, businessTypeID, nowFlowInfo.FlowOrder + 1);

                    if (tempFlowInfo != null)
                    {
                        return tempFlowInfo;
                    }
                    else
                    {
                        throw new Exception("此业务流程异常");
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获得下一业务流程信息对象
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="businessTypeID">业务ID</param>
        /// <param name="billNo">单据号</param>
        /// <returns>返回对象</returns>
        Flow_FlowInfo GetNextFlowInfo(DepotManagementDataContext ctx, int businessTypeID, string billNo)
        {
            Flow_FlowInfo tempNowInfo = GetNowFlowInfo(ctx, businessTypeID, billNo);

            if (tempNowInfo == null)
            {
                return null;
            }
            else
            {
                return GetNextFlowInfo(ctx, businessTypeID, tempNowInfo.FlowOrder, billNo);
            }
        }

        /// <summary>
        /// 获得业务流程执行人员信息对象列表
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="businessTypeID">业务ID</param>
        /// <param name="billNo">单据号</param>
        /// <returns>返回对象</returns>
        List<Flow_FlowExecuteInfoPersonnel> GetBusinessTypeExecutePersonnelList(DepotManagementDataContext ctx, string billNo, Flow_FlowInfo flowInfo)
        {
            var varData = from a in ctx.Flow_FlowInfo
                          join b in ctx.Flow_FlowExecuteInfoPersonnel
                          on a.FlowID equals b.FlowID
                          where a.FlowID == flowInfo.FlowID
                          && b.BillNo == billNo
                          select b;

            return varData.ToList();
        }

        /// <summary>
        /// 获得业务流程执行信息对象列表
        /// </summary>
        /// <param name="businessTypeID">业务ID</param>
        /// <returns>返回对象</returns>
        List<Flow_FlowExecuteInfo> GetBusinessTypeExecuteList(int businessTypeID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Flow_FlowInfo
                          join b in ctx.Flow_FlowExecuteInfo
                          on a.FlowID equals b.FlowID
                          where a.BusinessTypeID == businessTypeID
                          select b;

            return varData.ToList();
        }

        /// <summary>
        /// 获得业务流程执行信息对象列表
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="businessTypeID">业务ID</param>
        /// <returns>返回对象</returns>
        List<Flow_FlowExecuteInfo> GetBusinessTypeExecuteList(DepotManagementDataContext ctx, int businessTypeID)
        {
            var varData = from a in ctx.Flow_FlowInfo
                          join b in ctx.Flow_FlowExecuteInfo
                          on a.FlowID equals b.FlowID
                          where a.BusinessTypeID == businessTypeID
                          select b;

            return varData.ToList();
        }

        /// <summary>
        /// 获得业务流程执行人员信息对象列表(权限)
        /// </summary>
        /// <param name="orderFlag">流程顺序简单枚举</param>
        /// <param name="businessTypeID">业务ID</param>
        /// <param name="orderID">当前流程顺序ID</param>
        /// <param name="billNo">业务编号</param>
        /// <returns>返回对象列表</returns>
        List<Flow_FlowExecuteInfoPersonnel> GetBillAuthorityPersonnel(OrderFlagEnum orderFlag, int businessTypeID, int orderID, string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            Flow_FlowInfo tempInfo = new Flow_FlowInfo();

            switch (orderFlag)
            {
                case OrderFlagEnum.Now:
                    tempInfo = GetFlowInfo(ctx, businessTypeID, orderID);
                    break;
                case OrderFlagEnum.Next:
                    tempInfo = GetNextFlowInfo(ctx, businessTypeID, orderID, billNo);
                    break;
                default:
                    break;
            }

            if (tempInfo != null)
            {
                var varData = from a in ctx.Flow_FlowExecuteInfoPersonnel
                              where a.FlowID == tempInfo.FlowID
                              select a;

                if (varData.Count() > 0)
                {
                    return varData.ToList();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得业务流程执行信息对象列表(权限)
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="orderFlag">流程顺序简单枚举</param>
        /// <param name="businessTypeID">业务ID</param>
        /// <param name="orderID">流程顺序ID</param>
        /// <param name="billNo">业务编号</param>
        /// <returns>返回对象列表</returns>
        List<Flow_FlowExecuteInfo> GetBillAuthority(DepotManagementDataContext ctx, OrderFlagEnum orderFlag, int businessTypeID, int orderID, string billNo)
        {
            Flow_FlowInfo tempInfo = new Flow_FlowInfo();

            switch (orderFlag)
            {
                case OrderFlagEnum.Now:
                    tempInfo = GetFlowInfo(ctx, businessTypeID, orderID);
                    break;
                case OrderFlagEnum.Next:
                    tempInfo = GetNextFlowInfo(ctx, businessTypeID, orderID, billNo);
                    break;
                default:
                    break;
            }


            if (tempInfo != null)
            {
                var varData = from a in ctx.Flow_FlowExecuteInfo
                              where a.FlowID == tempInfo.FlowID
                              select a;

                if (varData.Count() > 0)
                {
                    return varData.ToList();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得业务流程执行人员列表
        /// </summary>
        /// <param name="orderFlag">流程顺序简单枚举</param>
        /// <param name="businessTypeID">业务ID</param>
        /// <param name="orderID">当前流程顺序ID</param>
        /// <param name="billNo">业务编号</param>
        /// <returns>返回列表</returns>
        List<string> GetPersonnelList(OrderFlagEnum orderFlag, int businessTypeID, int orderID, string billNo)
        {
            List<Flow_FlowExecuteInfoPersonnel> tempPersonnel = GetBillAuthorityPersonnel(orderFlag, businessTypeID, orderID, billNo);
            List<string> result = new List<string>();

            if (tempPersonnel != null)
            {
                foreach (Flow_FlowExecuteInfoPersonnel item in tempPersonnel)
                {
                    result.Add(item.WorkID);
                }
            }

            return result;
        }

        /// <summary>
        /// 获得业务流程执行权限角色列表
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="orderFlag">流程顺序简单枚举</param>
        /// <param name="businessTypeID">业务ID</param>
        /// <param name="orderID">当前流程顺序ID</param>
        /// <param name="storageIDOrWorkShopCode">库房/车间编码</param>
        /// <param name="billNo">业务编号</param>
        /// <returns>返回列表</returns>
        List<string> GetRoleList(DepotManagementDataContext ctx, OrderFlagEnum orderFlag, int businessTypeID, int orderID, 
            string storageIDOrWorkShopCode, string billNo)
        {
            IBillMessagePromulgatorServer billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();
            List<string> result = new List<string>();

            List<Flow_FlowExecuteInfo> tempExcList = GetBillAuthority(ctx, orderFlag, businessTypeID, orderID, billNo);

            if (tempExcList == null)
            {
                return result;
            }
            else
            {
                foreach (Flow_FlowExecuteInfo item in tempExcList)
                {
                    if (item.RoleName != null)
                    {
                        result.Add(item.RoleName);
                    }
                    else if (item.RoleStyleName != null)
                    {
                        CE_RoleStyleType roleType = GlobalObject.GeneralFunction.StringConvertToEnum<CE_RoleStyleType>(item.RoleStyleName);

                        if (roleType == CE_RoleStyleType.仓管)
                        {
                            result.AddRange(billMessageServer.GetSuperior(ctx, roleType, storageIDOrWorkShopCode));
                        }
                        else
                        {
                            if (item.RoleStyleName_ExceFlowID == null)
                            {
                                throw new Exception("流程控制器指定流程节点无效");
                            }

                            List<Flow_FlowData> lstData = GetBusinessOperationInfo(ctx, billNo, 
                                (int)item.RoleStyleName_ExceFlowID).OrderByDescending(k => k.OperationTime).ToList();

                            if (lstData == null || lstData.Count() == 0)
                            {
                                throw new Exception("当前流程节点操作记录为空");
                            }

                            View_HR_Personnel personnel = UniversalFunction.GetPersonnelInfo(ctx, lstData[0].OperationPersonnel);

                            if (personnel == null)
                            {
                                throw new Exception("无法获取操作人信息");
                            }

                            if (roleType == CE_RoleStyleType.节点人)
                            {
                                result.Add(personnel.工号);
                            }
                            else
                            {
                                result.AddRange(billMessageServer.GetSuperior(ctx, roleType, personnel.部门编码));
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 转换Table
        /// </summary>
        /// <param name="infoTable">数据信息集</param>
        /// <param name="businessTable">业务信息集</param>
        /// <param name="primaryKeyName">关联业务编号关键字段</param>
        /// <returns>返回Table</returns>
        DataTable ConvertTable(DataTable infoTable, DataTable businessTable, string primaryKeyName)
        {
            List<string> columnsName = new List<string>();

            foreach (DataColumn col in infoTable.Columns)
            {
                if (col.ColumnName != primaryKeyName)
                {
                    columnsName.Add(col.ColumnName);
                }
            }

            DataTable result = new DataTable();

            result.Columns.Add("业务编号");
            result.Columns.Add("业务状态");

            foreach (string clName in columnsName)
            {
                result.Columns.Add(clName);
            }

            result.Columns.Add("申请人");
            result.Columns.Add("申请时间");

            if (businessTable != null && businessTable.Rows.Count > 0)
            {
                var varData = from a in businessTable.AsEnumerable()
                              join b in infoTable.AsEnumerable()
                              on a.Field<string>("业务编号") equals b.Field<string>(primaryKeyName)
                              select new
                              {
                                  a,
                                  b
                              };

                foreach (var item1 in varData)
                {
                    DataRow resultRow = result.NewRow();

                    resultRow["业务编号"] = item1.a.Field<string>("业务编号");
                    resultRow["业务状态"] = item1.a.Field<string>("业务状态");
                    resultRow["申请人"] = item1.a.Field<string>("申请人");
                    resultRow["申请时间"] = item1.a.Field<DateTime>("申请时间");

                    foreach (string clName in columnsName)
                    {
                        resultRow[clName] = item1.b.Field<string>(clName);
                    }

                    result.Rows.Add(resultRow);
                }
            }

            return result;
        }

        /// <summary>
        /// 获取部门人员
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="lstDepartment">部门列表</param>
        /// <returns>返回人员列表</returns>
        List<string> GetPersonnelList_Department(DepotManagementDataContext ctx, List<string> lstDepartment)
        {
            List<string> result = new List<string>();

            foreach (string dept in lstDepartment)
            {
                string temp = dept;

                temp = temp.Replace("{", "");
                temp = temp.Replace("}", "");

                var varData = from a in ctx.HR_PersonnelArchive
                              where a.Dept.Length >= temp.Length
                              && a.Dept.Substring(0, temp.Length) == temp
                              && a.PersonnelStatus == 1
                              select a.WorkID;

                result.AddRange(varData.ToList());
            }

            result = result.Distinct().ToList();
            return result;
        }

        /// <summary>
        /// 获取单据当前执行权限
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="workID">工号</param>
        /// <param name="lstRoles">权限列表</param>
        /// <param name="storageIDOrWorkShopCode">库房ID</param>
        /// <returns>具有执行权限返回TRUE , 否则返回FALSE</returns>
        bool IsExecUser_Now(DepotManagementDataContext ctx, string billNo, Flow_FlowInfo flowInfo, string workID, 
            List<string> lstRoles, string storageIDOrWorkShopCode)
        {
            try
            {
                if (flowInfo.FlowOrder == 1)
                {
                    return true;
                }

                var varData1 = from a in ctx.Flow_FlowExecuteInfo
                               where a.FlowID == flowInfo.FlowID
                               select a;

                var varData2 = from a in ctx.Flow_FlowExecuteInfoPersonnel
                               where a.FlowID == flowInfo.FlowID
                               && a.BillNo == billNo
                               select a;

                if (varData1.Count() == 0 && varData2.Count() == 0)
                {
                    return false;
                }

                List<string> lstTempRoles = GetRoleList(ctx, OrderFlagEnum.Now, flowInfo.BusinessTypeID, 
                    flowInfo.FlowOrder, storageIDOrWorkShopCode, billNo);

                foreach (string role in lstTempRoles)
                {
                    if (lstRoles.Contains(role))
                    {
                        return true;
                    }

                    if (role.Contains("{") && role.Contains("}"))
                    {
                        string tempDept = role;

                        tempDept = tempDept.Replace("{", "");
                        tempDept = tempDept.Replace("}", "");

                        var varData3 = from a in ctx.HR_PersonnelArchive
                                       join b in ctx.Fun_get_BelongDept(tempDept)
                                       on a.Dept equals b.DeptCode
                                       where a.WorkID == workID
                                       select a;

                        if (varData3.Count() > 0)
                        {
                            return true;
                        }
                    }
                }

                if (lstTempRoles.Contains(workID))
                {
                    return true;
                }

                if (varData2.Where(k => k.WorkID == workID).Count() > 0)
                {
                    return true;
                }

                return false;

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Control

        /// <summary>
        /// 编辑业务流程知会执行人员
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="flowID">流程ID</param>
        /// <param name="notifyPersonnel">知会人员列表</param>
        void EditExecuteInfoPersonnel(DepotManagementDataContext ctx, string billNo, int flowID, List<string> notifyPersonnel)
        {
            var varData = from a in ctx.Flow_FlowExecuteInfoPersonnel
                          where a.BillNo == billNo
                          && a.FlowID == flowID
                          select a;

            ctx.Flow_FlowExecuteInfoPersonnel.DeleteAllOnSubmit(varData);

            foreach (string item in notifyPersonnel)
            {
                Flow_FlowExecuteInfoPersonnel tempLnq = new Flow_FlowExecuteInfoPersonnel();

                tempLnq.BillNo = billNo;
                tempLnq.FlowID = flowID;
                tempLnq.WorkID = item;

                ctx.Flow_FlowExecuteInfoPersonnel.InsertOnSubmit(tempLnq);
            }

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 回退单据处理单据操作记录
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="nowFlowInfo"></param>
        /// <param name="nextFlowInfo"></param>
        /// <param name="billNo"></param>
        void RebackFlowData(DepotManagementDataContext ctx, Flow_FlowInfo nowFlowInfo, Flow_FlowInfo nextFlowInfo, string billNo)
        {
            var varData = from a in ctx.Flow_FlowData
                          join b in ctx.Flow_FlowInfo on a.FlowID equals b.FlowID
                          join c in ctx.Flow_FlowBillData on a.FlowBillID equals c.FlowBillID
                          where b.BusinessTypeID == nowFlowInfo.BusinessTypeID 
                          && b.FlowOrder <= nowFlowInfo.FlowOrder && b.FlowOrder >= nextFlowInfo.FlowOrder
                          && c.BillNo == billNo
                          select new{ FlowDataID = a.FlowDataID , BillNo = c.BillNo, FlowID = a.FlowID};

            foreach (var item in varData)
            {
                var tempData = from a in ctx.Flow_FlowData
                               where a.FlowDataID == item.FlowDataID
                               select a;
                if (tempData.Count() == 1)
                {
                    tempData.Single().IsBack = true;
                }

                var varPersonnel = from a in ctx.Flow_FlowExecuteInfoPersonnel
                                   where a.BillNo == item.BillNo && a.FlowID == item.FlowID
                                   && a.FlowID != nextFlowInfo.FlowID
                                   select a;

                ctx.Flow_FlowExecuteInfoPersonnel.DeleteAllOnSubmit(varPersonnel);
            }

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billMessageServer">消息服务组件接口对象</param>
        /// <param name="flowInfo">发送消息的流程信息对象</param>
        /// <param name="billNo">单据号</param>
        /// <param name="receiverdType">接收类型</param>
        /// <param name="passInfo">接收消息角色/人员列表</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="keyWords">关键字</param>
        void SendMessage(DepotManagementDataContext ctx, IBillMessagePromulgatorServer billMessageServer, 
            Flow_FlowInfo flowInfo, string billNo, 
            PlatformManagement.BillFlowMessage_ReceivedUserType receiverdType, List<string> passInfo, string keyWords)
        {
            if (keyWords == null || keyWords.Trim().Length == 0)
            {
                keyWords = "";
            }
            else
            {
                keyWords += "  ※※※ ";
            }

            Flow_BusinessInfo tempBusinessInfo = GetBusinessInfo(flowInfo.BusinessTypeID);
            BillNumberControl billNoControl = new BillNumberControl(tempBusinessInfo.BusinessTypeName);

            if (IsFinalStep(ctx, flowInfo))
            {
                List<string> tempPersonnelList = new List<string>();

                List<Flow_FlowData> tempListData = GetFlowDataInfo(ctx, billNo);

                if (tempListData != null)
                {
                    foreach (Flow_FlowData item in tempListData)
                    {
                        tempPersonnelList.Add(item.OperationPersonnel);
                    }
                }

                billMessageServer.EndFlowMessage(billNo,
                    string.Format(keyWords + "{0}号{1}已完成", billNo, tempBusinessInfo.BusinessTypeName),
                    null, tempPersonnelList);
                billNoControl.UseBill(billNo);
            }
            else
            {
                if (passInfo == null || passInfo.Count == 0)
                {
                    throw new Exception("请给下步流程指定相关处理人员或者角色后，再试");
                }

                string msg = "";

                foreach (string item in passInfo)
                {
                    if (receiverdType == BillFlowMessage_ReceivedUserType.用户)
                    {
                        msg += "【" + UniversalFunction.GetPersonnelName(item) + "】,";
                    }
                    else
                    {
                        msg += "【" + item + "】,";
                    }
                }

                msg = msg.Substring(0, msg.Length - 1);

                if (!billMessageServer.IsExist(billNo))
                {
                    billMessageServer.SendNewFlowMessage(billNo, keyWords + "等待" + msg + "处理", receiverdType, passInfo);
                }
                else
                {
                    billMessageServer.PassFlowMessage(billNo, keyWords + "等待" + msg + "处理", receiverdType, passInfo);
                }
            }
        }

        /// <summary>
        /// 自定义发送消息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="nextFlowInfo"></param>
        /// <param name="billNo"></param>
        /// <param name="notifyPersonnel"></param>
        /// <returns></returns>
        void CustomPointPersonnel(DepotManagementDataContext ctx, Flow_FlowInfo nextFlowInfo, 
            string billNo, NotifyPersonnelInfo notifyPersonnel)
        {
            var varData = from a in ctx.Flow_FlowExecuteInfo
                          where a.FlowID == nextFlowInfo.FlowID
                          && (a.RoleName == null || a.RoleName.ToString().Trim().Length == 0)
                          && (a.RoleStyleName == null || a.RoleStyleName.ToString().Trim().Length == 0)
                          select a;

            if (varData != null && varData.Count() == 1)
            {
                if (notifyPersonnel.PersonnelBasicInfoList != null && notifyPersonnel.PersonnelBasicInfoList.Count > 0)
                {
                    EditExecuteInfoPersonnel(ctx, billNo, nextFlowInfo.FlowID, GetListNotifyPersonnel(notifyPersonnel));
                }
                else
                {
                    throw new Exception("流程出错, 请为下一个流程指定操作人或者操作角色");
                }
            }
        }

        /// <summary>
        /// 逻辑判断进行发送消息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="nextFlowInfo">下一业务流程信息对象</param>
        /// <param name="billNo">单据号</param>
        /// <param name="storageIDOrWorkShopCode">库房/车间编码</param>
        /// <param name="notifyPersonnel">知会人员列表</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="keyWords">关键字</param>
        void ConditionSendMessage(DepotManagementDataContext ctx,  Flow_FlowInfo nextFlowInfo, 
            string billNo, string storageIDOrWorkShopCode,
            NotifyPersonnelInfo notifyPersonnel, CE_FlowOperationType operationType, string keyWords)
        {
            IBillMessagePromulgatorServer billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();
            Flow_BusinessInfo tempBusinessInfo = GetBusinessInfo(nextFlowInfo.BusinessTypeID);
            billMessageServer.BillType = tempBusinessInfo.BusinessTypeName;
            List<Flow_FlowExecuteInfo> listTempNextExceuteInfo = GetBillAuthority(ctx, OrderFlagEnum.Now, nextFlowInfo.BusinessTypeID, 
                nextFlowInfo.FlowOrder, billNo);

            List<Flow_FlowData> listFlowData = GetFlowDataInfo(ctx, billNo);
            List<Flow_FlowExecuteInfoPersonnel> lstPersonnel = GetBusinessTypeExecutePersonnelList(ctx, billNo, nextFlowInfo);
            List<string> lstNotifyPersonnel = new List<string>();

            PlatformManagement.BillFlowMessage_ReceivedUserType userType = BillFlowMessage_ReceivedUserType.用户;

            //有执行节点、并行
            if (listTempNextExceuteInfo != null && listTempNextExceuteInfo.Where(k => k.IsParallel != null).Count() > 0)
            {
                userType = BillFlowMessage_ReceivedUserType.用户;
                lstNotifyPersonnel = GetParallelNotifyPersonnel(ctx, nextFlowInfo, billNo, operationType);
            }//有执行节点、串行
            else if (listTempNextExceuteInfo != null && listTempNextExceuteInfo.Count() > 0)
            {
                lstNotifyPersonnel =
                    GetRoleList(ctx, OrderFlagEnum.Now, nextFlowInfo.BusinessTypeID,
                    nextFlowInfo.FlowOrder, storageIDOrWorkShopCode, billNo);

                if (lstNotifyPersonnel == null || lstNotifyPersonnel.Count() == 0)
                {
                    throw new Exception("请给下步流程指定相关处理人员或者角色后，再试");
                }

                List<string> lstPass = new List<string>();

                foreach (string str in lstNotifyPersonnel)
                {
                    if (str.Contains("{") && str.Contains("}"))
                    {
                        lstPass.Add(str);
                    }
                }

                if (lstPass.Count() == 0)
                {
                    int k = 0;

                    if (Int32.TryParse(lstNotifyPersonnel[0], out k))
                    {
                        userType = BillFlowMessage_ReceivedUserType.用户;
                    }
                    else
                    {
                        userType = BillFlowMessage_ReceivedUserType.角色;
                    }
                }
                else
                {
                    userType = BillFlowMessage_ReceivedUserType.用户;
                    lstNotifyPersonnel = GetPersonnelList_Department(ctx, lstPass);
                }
            }
            else if (notifyPersonnel != null 
                && notifyPersonnel.PersonnelBasicInfoList != null 
                && notifyPersonnel.PersonnelBasicInfoList.Count() > 0)
            {
                userType = GeneralFunction.StringConvertToEnum<BillFlowMessage_ReceivedUserType>(notifyPersonnel.UserType);

                foreach (PersonnelBasicInfo item in notifyPersonnel.PersonnelBasicInfoList)
                {
                    switch (userType)
                    {
                        case BillFlowMessage_ReceivedUserType.用户:
                            lstNotifyPersonnel.Add(item.工号);
                            break;
                        case BillFlowMessage_ReceivedUserType.角色:
                            lstNotifyPersonnel.Add(item.角色);
                            break;
                        default:
                            break;
                    }
                }

            }//无执行节点、知会指定执行人员
            else if (lstPersonnel != null && lstPersonnel.Count() > 0)
            {
                userType = BillFlowMessage_ReceivedUserType.用户;
                lstNotifyPersonnel = lstPersonnel.Select(k => k.WorkID).ToList();
            }//无执行节点、无指定执行人员、知会历史操作人
            else if (listFlowData != null && listFlowData.Count() > 0)
            {
                userType = BillFlowMessage_ReceivedUserType.用户;
                lstNotifyPersonnel = 
                    listFlowData.Where(k => k.FlowID == nextFlowInfo.FlowID).Select(t => t.OperationPersonnel).Distinct().ToList();
            }
            else
            {
                throw new Exception("此流程无设置，无历史操作人，流程出错");
            }

            SendMessage(ctx, billMessageServer, nextFlowInfo, billNo, userType, lstNotifyPersonnel, keyWords);
        }
        
        /// <summary>
        /// 处理单据主要信息以及操作记录
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="nowFlowInfo"></param>
        /// <param name="nextFlowInfo"></param>
        /// <param name="billNo"></param>
        /// <param name="advise"></param>
        /// <param name="notifyPersonnel"></param>
        /// <param name="operationType"></param>
        void OperationBillMainInfo_RecordFlowData(DepotManagementDataContext ctx, Flow_FlowInfo nowFlowInfo,
            Flow_FlowInfo nextFlowInfo, string billNo, string advise, 
            NotifyPersonnelInfo notifyPersonnel, CE_FlowOperationType operationType)
        {
            Flow_BusinessInfo tempBusinessInfo = GetBusinessInfo(nextFlowInfo.BusinessTypeID);
            Flow_FlowBillData tempBillData = GetBillData(ctx, billNo);

            if (tempBillData == null)
            {
                Flow_FlowBillData tempData = new Flow_FlowBillData();

                tempData.BillNo = billNo;
                tempData.BusinessTypeID = tempBusinessInfo.BusinessTypeID;
                tempData.FlowID = nextFlowInfo.FlowID;
                tempData.CreateDate = ServerTime.Time;
                tempData.CreatePersonnel = BasicInfo.LoginID;

                ctx.Flow_FlowBillData.InsertOnSubmit(tempData);
                ctx.SubmitChanges();

                InsertFlowData(ctx, nowFlowInfo.FlowID, billNo, advise);
                ctx.SubmitChanges();
            }
            else
            {
                InsertFlowData(ctx, nowFlowInfo.FlowID, billNo, advise);
                ctx.SubmitChanges();

                if (operationType == CE_FlowOperationType.提交)
                {
                    nextFlowInfo = GetNextFlowInfo(ctx, (int)GetBusinessTypeID(billNo), billNo);
                }

                tempBillData.FlowID = nextFlowInfo.FlowID;
                ctx.SubmitChanges();
            }

            switch (operationType)
            {
                case CE_FlowOperationType.提交:
                    if (nextFlowInfo != nowFlowInfo)
                    {
                        CustomPointPersonnel(ctx, nextFlowInfo, billNo, notifyPersonnel);
                        ctx.SubmitChanges();
                    }
                    break;
                case CE_FlowOperationType.回退:
                    RebackFlowData(ctx, nowFlowInfo, nextFlowInfo, billNo);
                    ctx.SubmitChanges();
                    break;
                case CE_FlowOperationType.暂存:
                case CE_FlowOperationType.未知:
                    break;
                default:
                    break;
            }

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 添加业务流程操作记录
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="flowID">流程ID</param>
        /// <param name="billNo">单据号</param>
        /// <param name="advise">操作人建议</param>
        void InsertFlowData(DepotManagementDataContext ctx, int flowID, string billNo, string advise)
        {
            try
            {
                CheckData(ctx, billNo);
                Flow_FlowBillData tempBill = GetBillData(ctx, billNo);

                if (tempBill == null)
                {
                    throw new Exception("【"+ billNo +"】业务单号不存在");
                }
                else
                {
                    Flow_FlowData tempData = new Flow_FlowData();

                    tempData.FlowBillID = tempBill.FlowBillID;
                    tempData.FlowID = flowID;
                    tempData.OperationPersonnel = BasicInfo.LoginID;
                    tempData.OperationTime = ServerTime.Time;
                    tempData.Advise = advise;

                    ctx.Flow_FlowData.InsertOnSubmit(tempData);
                }

                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        void DeleteMessage(string billNo)
        {
            string strSql = "delete from PlatformService.dbo.Flow_BillFlowMessage where 单据号 = '"+ billNo +"'";

            GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 流程传输主方法(默认)
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="advise">操作人员建议</param>
        /// <param name="storageIDOrWorkShopCode">库房/车间编码</param>
        /// <param name="notifyPersonnel">指定知会人员列表</param>
        /// <param name="keyWords">关键字</param>
        public void FlowPass(string billNo, string advise, string storageIDOrWorkShopCode, 
            NotifyPersonnelInfo notifyPersonnel, string keyWords)
        {

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                if (advise == null || advise.Trim().Length == 0)
                {
                    advise = "通过 ";
                }

                CheckData(ctx, billNo);

                int? businessTypeID = GetBusinessTypeID(ctx, billNo);

                if (businessTypeID == null)
                {
                    throw new Exception("不存在此业务");
                }

                Flow_FlowInfo nextFlowInfo = GetNextFlowInfo(ctx, (int)businessTypeID, billNo);
                Flow_FlowInfo nowFlowInfo = GetNowFlowInfo(ctx, (int)businessTypeID, billNo);

                if (nextFlowInfo != null)
                {
                    if (!IsExecUser_Now(ctx, billNo, nowFlowInfo, BasicInfo.LoginID, BasicInfo.ListRoles, storageIDOrWorkShopCode))
                    {
                        throw new Exception("您不具有此单据的执行权限，请重新刷新单据后再试");
                    }

                    OperationBillMainInfo_RecordFlowData(ctx, nowFlowInfo, nextFlowInfo, billNo, advise, notifyPersonnel,
                        CE_FlowOperationType.提交);

                    ConditionSendMessage(ctx, nextFlowInfo, billNo, storageIDOrWorkShopCode, notifyPersonnel,
                        CE_FlowOperationType.提交, keyWords);
                }

                ctx.SubmitChanges();
                ctx.Transaction.Commit();
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 强制删除业务单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        public void FlowForceDelete(string billNo)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                CheckData(ctx, billNo);

                var varData = from a in ctx.Flow_FlowBillData
                              where a.BillNo == billNo
                              select a;

                var varTemp = from a in ctx.Flow_FlowExecuteInfoPersonnel
                              where a.BillNo == billNo
                              select a;

                ctx.Flow_FlowExecuteInfoPersonnel.DeleteAllOnSubmit(varTemp);
                ctx.Flow_FlowBillData.DeleteAllOnSubmit(varData);

                IAssignBillNoServer service = ServerModule.BasicServerFactory.GetServerModule<IAssignBillNoServer>();
                service.CancelBillNo(ctx, billNo);

                ctx.SubmitChanges();
            }

            DeleteMessage(billNo);
        }

        /// <summary>
        /// 删除业务单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        public void FlowDelete(DepotManagementDataContext ctx, string billNo)
        {
            CheckData(ctx, billNo);

            if (IsFinalStep(ctx, GetNowFlowInfo(ctx, (int)GetBusinessTypeID(billNo), billNo)))
            {
                throw new Exception("此单据已完成，无法删除");
            }

            var varTemp = from a in ctx.Flow_FlowExecuteInfoPersonnel
                          where a.BillNo == billNo
                          select a;

            var varData = from a in ctx.Flow_FlowBillData
                          where a.BillNo == billNo
                          select a;

            if (varData.Count() > 0)
            {
                var varData1 = from a in ctx.Flow_FlowData
                               where a.FlowBillID == varData.First().FlowBillID
                               select a;

                if (varData1.OrderBy(k => k.OperationTime).First().OperationPersonnel != BasicInfo.LoginID)
                {
                    throw new Exception("无法删除非本人创建的单据");
                }

                ctx.Flow_FlowData.DeleteAllOnSubmit(varData1);
            }

            ctx.Flow_FlowExecuteInfoPersonnel.DeleteAllOnSubmit(varTemp);
            ctx.Flow_FlowBillData.DeleteAllOnSubmit(varData);

            IAssignBillNoServer service = ServerModule.BasicServerFactory.GetServerModule<IAssignBillNoServer>();
            service.CancelBillNo(ctx, billNo);

            ctx.SubmitChanges();

            DeleteMessage(billNo);
        }

        /// <summary>
        /// 流程暂存
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="storageIDOrWorkShopCode">库房/车间编号</param>
        /// <param name="advise">他人意见</param>
        /// <param name="keyWords">关键字</param>
        public void FlowHold(string billNo, string storageIDOrWorkShopCode, string advise, string keyWords)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                if (advise == null || advise.Trim().Length == 0)
                {
                    advise = "暂存 ";
                }

                CheckData(ctx, billNo);

                int? businessTypeID = GetBusinessTypeID(billNo);

                if (businessTypeID == null)
                {
                    throw new Exception("不存在此业务");
                }

                Flow_FlowInfo flowInfo = GetNowFlowInfo(ctx, (int)businessTypeID, billNo);

                if (flowInfo != null)
                {
                    if (!IsExecUser_Now(ctx, billNo, flowInfo, BasicInfo.LoginID, BasicInfo.ListRoles, storageIDOrWorkShopCode))
                    {
                        throw new Exception("您不具有此单据的执行权限，请重新刷新单据后再试");
                    }

                    OperationBillMainInfo_RecordFlowData(ctx, flowInfo, flowInfo, billNo, advise, null, 
                        CE_FlowOperationType.暂存);
                }

                ConditionSendMessage(ctx, GetNowFlowInfo(ctx, (int)businessTypeID, billNo), billNo, storageIDOrWorkShopCode, null,
                    CE_FlowOperationType.暂存, keyWords);

                ctx.SubmitChanges();
                ctx.Transaction.Commit();
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 流程回退
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="advise">他人意见</param>
        /// <param name="storageIDOrWorkShopCode">库房/车间编号</param>
        /// <param name="flowID">流程ID</param>
        /// <param name="keyWords">关键字</param>
        public void FlowReback(string billNo, string advise, string storageIDOrWorkShopCode, int flowID, string keyWords)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                if (advise == null || advise.Trim().Length == 0)
                {
                    advise = "回退 ";
                }

                CheckData(ctx, billNo);

                int? businessTypeID = GetBusinessTypeID(billNo);

                if (businessTypeID == null)
                {
                    throw new Exception("不存在此业务");
                }

                Flow_FlowInfo nextFlowInfo = GetFlowInfo(ctx, flowID);
                Flow_FlowInfo nowFlowInfo = GetNowFlowInfo(ctx, (int)businessTypeID, billNo);

                if (nextFlowInfo != null)
                {
                    if (!IsExecUser_Now(ctx, billNo, nowFlowInfo, BasicInfo.LoginID, BasicInfo.ListRoles, storageIDOrWorkShopCode))
                    {
                        throw new Exception("您不具有此单据的执行权限，请重新刷新单据后再试");
                    }

                    OperationBillMainInfo_RecordFlowData(ctx, nowFlowInfo, nextFlowInfo, billNo, advise, null,
                        CE_FlowOperationType.回退);
                }

                ConditionSendMessage(ctx, GetNowFlowInfo(ctx, (int)businessTypeID, billNo), billNo, storageIDOrWorkShopCode, null,
                    CE_FlowOperationType.回退, keyWords);

                ctx.SubmitChanges();
                ctx.Transaction.Commit();
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 完成引用单据的消息传递
        /// </summary>
        /// <param name="billNo"></param>
        /// <param name="lstReferenceBillNo"></param>
        public void FlowFinishReferenceSendMessage(string billNo, List<string> lstReferenceBillNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                Flow_FlowInfo flowInfo = GetNowFlowInfo(ctx, (int)GetBusinessTypeID(billNo), billNo);

                if (IsFinalStep(ctx, flowInfo))
                {
                    IBillMessagePromulgatorServer billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

                    foreach (string tempBillNo in lstReferenceBillNo)
                    {
                        Flow_BusinessInfo tempBusinessInfo = GetBusinessInfo((int)GetBusinessTypeID(tempBillNo));
                        billMessageServer.BillType = tempBusinessInfo.BusinessTypeName;

                        billMessageServer.EndFlowMessage(tempBillNo, "此流程已完成", null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 回退已完成的单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="advise">他人意见</param>
        /// <param name="storageIDOrWorkShopCode">库房/车间编号</param>
        /// <param name="flowID">流程ID</param>
        /// <param name="keyWords">关键字</param>
        public void FlowReback_Finish(string billNo, string advise, string storageIDOrWorkShopCode, int flowID, string keyWords)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                if (advise == null || advise.Trim().Length == 0)
                {
                    advise = "回退 ";
                }

                CheckData(ctx, billNo);

                int? businessTypeID = GetBusinessTypeID(billNo);

                if (businessTypeID == null)
                {
                    throw new Exception("不存在此业务");
                }

                Flow_FlowInfo nextFlowInfo = GetFlowInfo(ctx, flowID);
                Flow_FlowInfo nowFlowInfo = GetNowFlowInfo(ctx, (int)businessTypeID, billNo);

                if (nextFlowInfo != null)
                {
                    OperationBillMainInfo_RecordFlowData(ctx, nowFlowInfo, nextFlowInfo, billNo, advise, null,
                        CE_FlowOperationType.回退);
                }

                ConditionSendMessage(ctx, GetNowFlowInfo(ctx, (int)businessTypeID, billNo), billNo, storageIDOrWorkShopCode, null,
                    CE_FlowOperationType.回退, keyWords);

                var varData = from a in ctx.BASE_AssignBillNo
                              where a.Bill_No == billNo
                              select a;

                if (varData.Count() == 1)
                {
                    BASE_AssignBillNo assignBillInfo = varData.Single();

                    assignBillInfo.IsAbandoned = false;
                    assignBillInfo.AlreadyUse = false;
                }

                ctx.SubmitChanges();
                ctx.Transaction.Commit();
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
 
        }

        #endregion

        //#region 流程传递(未使用) AuthorityControl(待使用 已停用)

        ///// <summary>
        ///// 流程传输主方法
        ///// </summary>
        ///// <param name="billNo">单据号</param>
        ///// <param name="storageIDOrWorkShopCode">库房/车间编码</param>
        ///// <param name="notifyPersonnel">知会人员列表</param>
        ///// <param name="error">错误信息</param>
        //void FlowPass(string billNo, string storageIDOrWorkShopCode, NotifyPersonnelInfo notifyPersonnel, out string error)
        //{
        //    DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
        //    error = null;

        //    try
        //    {
        //        CheckData(billNo);

        //        int? businessTypeID = GetBusinessTypeID(billNo);

        //        if (businessTypeID == null)
        //        {
        //            throw new Exception("不存在此业务");
        //        }

        //        Flow_FlowInfo nextFlowInfo = GetNextFlowInfo((int)businessTypeID, billNo);

        //        if (nextFlowInfo != null)
        //        {
        //            UpdateBillInfo(ctx, nextFlowInfo, billNo);
        //            ConditionSendMessage(ctx, nextFlowInfo, billNo, storageIDOrWorkShopCode, notifyPersonnel, FlowOperationType.提交);
        //        }

        //        ctx.SubmitChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        error = ex.Message;
        //    }
        //}

        ///// <summary>
        ///// 流程传输主方法(指定业务ID，对于相同业务多版本)
        ///// </summary>
        ///// <param name="businessTypeID">业务ID</param>
        ///// <param name="billNo">单据号</param>
        ///// <param name="storageIDOrWorkShopCode">库房/车间编码</param>
        ///// <param name="notifyPersonnel">知会人员列表</param>
        ///// <param name="error">错误信息</param>
        //void FlowPass(int businessTypeID, string billNo, string storageIDOrWorkShopCode, NotifyPersonnelInfo notifyPersonnel, out string error)
        //{
        //    DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
        //    error = null;

        //    try
        //    {
        //        CheckData(billNo);
        //        var varData = from a in ctx.Flow_BusinessInfo
        //                      where a.BusinessTypeID == businessTypeID
        //                      select a;

        //        if (varData == null || varData.Count() == 0)
        //        {
        //            throw new Exception("不存在此业务");
        //        }

        //        Flow_FlowInfo nextFlowInfo = GetNextFlowInfo((int)businessTypeID, billNo);

        //        if (nextFlowInfo != null)
        //        {
        //            UpdateBillInfo(ctx, nextFlowInfo, billNo);
        //            ConditionSendMessage(ctx, nextFlowInfo, billNo, storageIDOrWorkShopCode, notifyPersonnel, FlowOperationType.提交);
        //        }

        //        ctx.SubmitChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        error = ex.Message;
        //        throw new Exception(error);
        //    }
        //}

        //#endregion

        //#region AuthorityControl(待使用 已停用)

        ///// <summary>
        ///// 判断当前用户是否允许操作
        ///// </summary>
        ///// <param name="billNo">单据号</param>
        ///// <param name="businessTypeID">业务ID</param>
        ///// <param name="storageIDOrWorkShopCode">库房/车间编码</param>
        ///// <returns>返回True 表示允许，返回False 表示不允许</returns>
        //bool IsAllowable(string billNo, int businessTypeID, string storageIDOrWorkShopCode)
        //{
        //    Flow_FlowInfo tempNowInfo = GetNowFlowInfo(businessTypeID, billNo);

        //    if (tempNowInfo == null)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        List<string> listPersonnel = GetPersonnelList(OrderFlagEnum.Now, businessTypeID, tempNowInfo.FlowOrder, billNo);

        //        if (listPersonnel != null)
        //        {
        //            if (listPersonnel.Contains(BasicInfo.LoginName))
        //            {
        //                return true;
        //            }
        //        }
        //        else
        //        {
        //            List<string> listRole = GetRoleList(OrderFlagEnum.Next, businessTypeID, tempNowInfo.FlowOrder, storageIDOrWorkShopCode, billNo);

        //            if (listRole != null)
        //            {
        //                foreach (string item in listRole)
        //                {
        //                    if (BasicInfo.ListRoles.Contains(item))
        //                    {
        //                        return true;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return false;
        //}

        ///// <summary>
        ///// 智能获得流程名称
        ///// </summary>
        ///// <param name="nameAndVersion">业务名+版本信息</param>
        ///// <param name="billNo">单据号</param>
        ///// <returns>返回流程名称字符串</returns>
        //public string GetBillNoAuthority(string nameAndVersion, string billNo)
        //{
        //    string result = null;
        //    Flow_BusinessInfo businessInfo = GetBusinessInfo(nameAndVersion);

        //    if (businessInfo != null)
        //    {
        //        Flow_FlowInfo nowFlowInfo = GetNowFlowInfo(businessInfo.BusinessTypeID, billNo);

        //        if (nowFlowInfo != null)
        //        {
        //            Flow_FlowInfo nextFlowInfo = GetNextFlowInfo(businessInfo.BusinessTypeID, nowFlowInfo.FlowOrder, billNo);

        //            if (nextFlowInfo != null)
        //            {
        //                result = nextFlowInfo.FlowName;
        //            }
        //        }
        //        else
        //        {
        //            List<Flow_FlowInfo> listFlowInfo = GetListFlowInfo(businessInfo.BusinessTypeID);

        //            if (listFlowInfo != null)
        //            {
        //                result = listFlowInfo.OrderBy(k => k.FlowOrder).First().FlowName;
        //            }
        //        }
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// 获得业务流程名列表
        ///// </summary>
        ///// <param name="nameAndVersion">业务名+版本信息</param>
        ///// <returns>返回列表</returns>
        //public List<string> GetBusinessAllAuthority(string nameAndVersion)
        //{
        //    List<string> result = new List<string>();

        //    Flow_BusinessInfo businessInfo = GetBusinessInfo(nameAndVersion);

        //    if (businessInfo != null)
        //    {
        //        List<Flow_FlowInfo> listFlowInfo = GetListFlowInfo(businessInfo.BusinessTypeID);

        //        if (listFlowInfo != null && listFlowInfo.Count > 0)
        //        {
        //            foreach (Flow_FlowInfo item in listFlowInfo)
        //            {
        //                result.Add(item.FlowName);
        //            }
        //        }
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// 获得业务信息对象
        ///// </summary>
        ///// <param name="nameAndVersion">业务名+版本信息</param>
        ///// <returns>返回对象</returns>
        //public Flow_BusinessInfo GetBusinessInfo(string nameAndVersion)
        //{
        //    DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

        //    var varData = from a in ctx.Flow_BusinessInfo
        //                  where (a.BusinessTypeName + a.Version) == nameAndVersion
        //                  select a;

        //    if (varData.Count() == 0)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return varData.First();
        //    }
        //}

        //#endregion
    }
}
