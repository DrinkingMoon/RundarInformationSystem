using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PlatformManagement;
using GlobalObject;

namespace ServerModule
{
    class MinorPurchaseBillServer : ServerModule.IMinorPurchaseBillServer
    {
        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 查询结果过滤器
        /// </summary>
        string m_queryResultFilter = null;

        /// <summary>
        /// 人员档案服务组件
        /// </summary>
        Service_Peripheral_HR.IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<Service_Peripheral_HR.IPersonnelArchiveServer>();

        /// <summary>
        /// 获取或设置查询结果过滤器
        /// </summary>
        public string QueryResultFilter
        {
            get { return m_queryResultFilter; }
            set { m_queryResultFilter = value; }
        }

        /// <summary>
        /// 获得引用的合同信息
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns>返回Table</returns>
        public DataTable GetBargainRelate(DateTime startDate, DateTime endDate)
        {
            string strSql = " select distinct a.BillNo as 单据号, a.PurchaseType as 采购类别, dbo.fun_get_Name(a.Applicant) as 申请人, "+
                            " dbo.fun_get_Department(a.ApplicantDeptCode) as 申请部门, a.ApplicantDate as 申请时间, a.Remark as 备注 "+
                            " from B_MinorPurchaseBill as a inner join B_MinorPurchaseList as b on a.BillNo = b.BillNo "+
                            " left join (select a.MinorPurchaseBillNo, b.GoodsID from B_BargainInfo as a inner join B_BargainGoods as b "+
                            " on a.BargainNumber = b.BargainNumber where a.MinorPurchaseBillNo <> '' and a.MinorPurchaseBillNo is not null) as c "+
                            " on a.BillNo = c.MinorPurchaseBillNo and b.GoodsID = c.GoodsID "+
                            " where b.GoodsStatus = '已到货' and c.MinorPurchaseBillNo is null and a.ProcurementEngineer = '" + BasicInfo.LoginID + "' " +
                            " and a.ApplicantDate >= '" + startDate.ToShortDateString() + "' and a.ApplicantDate <= '" + endDate.ToShortDateString() + "' " +
                            " order by a.ApplicantDate desc";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单据列表
        /// </summary>
        /// <param name="returnInfo">零星采购信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllBillInfo(out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            IAuthorization serverAuthorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = serverAuthorization.Query("零星采购申请单", null);
            }
            else
            {
                qr = serverAuthorization.Query("零星采购申请单", null, QueryResultFilter);
            }

            if (!qr.Succeeded)
            {
                error = qr.Error;
                return false;
            }

            returnInfo = qr;
            return true;
        }

        /// <summary>
        /// 获得单条信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回LINQ数据集</returns>
        public B_MinorPurchaseBill GetBillInfo(string billID)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varData = from a in dataContext.B_MinorPurchaseBill
                          where a.BillNo == billID
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
        /// 获得明细信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回Table</returns>
        public DataTable GetListInfo(string billID)
        {
            string strSql = "select * from View_B_MinorPurchaseList where 编号 = '" + billID + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得已到货的明细信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回Table</returns>
        public DataTable GetFinishListInfo(string billID)
        {
            string strSql = " select c.序号 as 物品ID, c.图号型号,c.物品名称,c.规格,供应商,预算价格,申请数量,c.单位" +
                            " from View_B_MinorPurchaseList as a inner join View_F_GoodsPlanCost as c on a.GoodsID = c.序号" +
                            " left join (select a.MinorPurchaseBillNo, b.GoodsID from B_BargainInfo as a inner join B_BargainGoods as b " +
                            " on a.BargainNumber = b.BargainNumber where a.MinorPurchaseBillNo <> '' and a.MinorPurchaseBillNo is not null) as b " +
                            " on a.编号 = b.MinorPurchaseBillNo and a.GoodsID = b.GoodsID " +
                            " where a.物品状态 = '已到货' and b.MinorPurchaseBillNo is null and a.编号 = '" + billID + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 删除明细
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteList(DepotManagementDataContext dataContext, string billNo, out string error)
        {
            error = null;

            try
            {
                var varData = from a in dataContext.B_MinorPurchaseList
                              where a.BillNo == billNo
                              select a;

                dataContext.B_MinorPurchaseList.DeleteAllOnSubmit(varData);

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;

            }
        }

        /// <summary>
        /// 删除零星采购单
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteInfo(string billNo, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.B_MinorPurchaseBill
                              where a.BillNo == billNo
                              select a;

                if (!DeleteList(dataContext, billNo, out error))
                {
                    throw new Exception(error);
                }

                DeleteChangeBill(billNo,out error);
                dataContext.B_MinorPurchaseBill.DeleteAllOnSubmit(varData);
                dataContext.SubmitChanges();

                m_billMessageServer.BillType = "零星采购申请单";
                m_billMessageServer.DestroyMessage(billNo);

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;

            }
        }

        /// <summary>
        /// 插入明细
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="listInfo">明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool InsertList(DepotManagementDataContext dataContext, string billNo, DataTable listInfo, out string error)
        {
            error = null;

            try
            {
                for (int i = 0; i < listInfo.Rows.Count; i++)
                {
                    B_MinorPurchaseList lnqList = new B_MinorPurchaseList();

                    lnqList.BillNo = billNo;
                    lnqList.GoodsCode = listInfo.Rows[i]["图号型号"].ToString();
                    lnqList.Count = Convert.ToInt32(listInfo.Rows[i]["申请数量"]);
                    lnqList.GoodsName = listInfo.Rows[i]["物品名称"].ToString();
                    lnqList.Remark = listInfo.Rows[i]["备注"].ToString();
                    lnqList.GoodsStatus = listInfo.Rows[i]["物品状态"].ToString();
                    lnqList.Price = Convert.ToDecimal(listInfo.Rows[i]["预算价格"]);
                    lnqList.Provider = listInfo.Rows[i]["供应商"].ToString();
                    lnqList.FinishDate = Convert.ToDateTime(listInfo.Rows[i]["最终到货日期"]);
                    lnqList.RequireArriveDate = Convert.ToDateTime(listInfo.Rows[i]["要求到货日期"]);
                    lnqList.Spec = listInfo.Rows[i]["规格"].ToString();
                    lnqList.Unit = UniversalFunction.GetUnitID(listInfo.Rows[i]["单位"].ToString()) 
                        == "" ? "1" : UniversalFunction.GetUnitID(listInfo.Rows[i]["单位"].ToString());

                    if (listInfo.Rows[i]["是否有图纸发品保部"] == null)
                    {
                        lnqList.IsDrawing = false;
                    }
                    else
                    {
                        lnqList.IsDrawing = Convert.ToBoolean(listInfo.Rows[i]["是否有图纸发品保部"]);
                    }

                    if (listInfo.Rows[i]["GoodsID"].ToString() != "")
                    {
                        lnqList.GoodsID =  Convert.ToInt32(listInfo.Rows[i]["GoodsID"]);
                    }

                    dataContext.B_MinorPurchaseList.InsertOnSubmit(lnqList);
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获取新的单据号
        /// </summary>
        /// <returns>成功返回单号，失败抛出异常</returns>
        private string GetNextBillID()
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            long maxValue = 1;

            DateTime dt = ServerTime.Time;

            if (dataContxt.B_MinorPurchaseBill.Count() > 0)
            {
                var result = from c in dataContxt.B_MinorPurchaseBill
                             where c.BillNo.Substring(0, 4) == dt.Year.ToString()
                             && c.BillNo.Substring(4, 2) == string.Format("{0:D2}",dt.Month)
                             select c;

                if (result.Count() > 0)
                {
                    maxValue += (from c in result select Convert.ToInt32(c.BillNo.Substring(6))).Max();
                }
            }

            return string.Format("{0:D4}{1:D2}{2:D3}", dt.Year, dt.Month, maxValue);
        }

        /// <summary>
        /// 提交申请
        /// </summary>
        /// <param name="minorBill">单据信息数据集</param>
        /// <param name="listInfo">明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool InsertBill(B_MinorPurchaseBill minorBill, DataTable listInfo, out string error)
        {
            m_billMessageServer.BillType = "零星采购申请单";
            error = null;

            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            try
            {
                dataContext.Connection.Open();
                dataContext.Transaction = dataContext.Connection.BeginTransaction();

                var varData = from a in dataContext.B_MinorPurchaseBill
                              where a.Applicant == minorBill.Applicant && a.ApplicantDate == minorBill.ApplicantDate
                              && a.PurchaseType == minorBill.PurchaseType
                              select a;

                if (varData.Count() == 0)
                {
                    if (minorBill.PurchaseType != "零星加工")
                    {
                        minorBill.BillStatus = MinorPurchaseBillStatus.等待仓管确认.ToString();
                    }
                    else
                    {
                        minorBill.BillStatus = MinorPurchaseBillStatus.等待部门负责人审核.ToString();
                    }

                    minorBill.BillNo = GetNextBillID();

                    dataContext.B_MinorPurchaseBill.InsertOnSubmit(minorBill);
                    dataContext.SubmitChanges();

                    if (!InsertList(dataContext, minorBill.BillNo, listInfo, out error))
                    {
                        throw new Exception(error);
                    }

                    if (minorBill.PurchaseType == "零星采购" || minorBill.PurchaseType == "油品采购")
                    {
                        m_billMessageServer.SendNewFlowMessage(minorBill.BillNo.ToString(),
                            string.Format("【采购类别】：{0} 【申请部门】：{1} 【用途】：{2}   ※※※ 等待【备件库管理员】处理",
                            minorBill.PurchaseType, UniversalFunction.GetDeptName( minorBill.ApplicantDeptCode), 
                            UniversalFunction.GetPurposeInfo(minorBill.PurposeCode).Purpose),
                            BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.备件仓库管理员.ToString());
                    }
                    else if (minorBill.PurchaseType == "量具采购" || minorBill.PurchaseType == "检具加工")
                    {
                        m_billMessageServer.SendNewFlowMessage(minorBill.BillNo.ToString(),
                            string.Format("【采购类别】：{0} 【申请部门】：{1} 【用途】：{2}   ※※※ 等待【量检具库管理员】处理",
                            minorBill.PurchaseType, UniversalFunction.GetDeptName(minorBill.ApplicantDeptCode),
                            UniversalFunction.GetPurposeInfo(minorBill.PurposeCode).Purpose),
                            BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.量检具库管理员.ToString());
                    }
                    else
                    {
                        m_billMessageServer.SendNewFlowMessage(minorBill.BillNo.ToString(),
                            string.Format("【采购类别】：{0} 【申请部门】：{1} 【用途】：{2}   ※※※ 等待【部门负责人】处理",
                            minorBill.PurchaseType, UniversalFunction.GetDeptName(minorBill.ApplicantDeptCode),
                            UniversalFunction.GetPurposeInfo(minorBill.PurposeCode).Purpose),
                            BillFlowMessage_ReceivedUserType.角色,
                            m_billMessageServer.GetDeptPrincipalRoleName(UniversalFunction.GetPersonnelInfo(minorBill.Applicant).部门编码).ToList());
                    }
                }
                else if (varData.Count() == 1)
                {
                    B_MinorPurchaseBill lnqBill = varData.Single();

                    if (lnqBill.Compiler != BasicInfo.LoginID)
                    {
                        throw new Exception("不是编制人本人，不能重新提交单据");
                    }

                    if (minorBill.PurchaseType != "零星加工")
                    {
                        lnqBill.BillStatus = MinorPurchaseBillStatus.等待仓管确认.ToString();
                    }
                    else
                    {
                        lnqBill.BillStatus = MinorPurchaseBillStatus.等待部门负责人审核.ToString();
                    }

                    lnqBill.Remark = minorBill.Remark;
                    lnqBill.Applicant = minorBill.Applicant;
                    lnqBill.ApplicantDate = minorBill.ApplicantDate;
                    lnqBill.CompileDate = ServerTime.Time;
                    lnqBill.ApplicantDeptCode = minorBill.ApplicantDeptCode;
                    lnqBill.PurchaseType = minorBill.PurchaseType;
                    lnqBill.Exigence = minorBill.Exigence;
                    lnqBill.PurposeCode = minorBill.PurposeCode;
                    lnqBill.DeptDirector = "";
                    lnqBill.DeptDirectorIdea = "";
                    lnqBill.DeptDirectorRatify = false;
                    lnqBill.LeaderIdea = "";
                    lnqBill.LeaderRatify = false;
                    lnqBill.Leader = "";
                    lnqBill.CWIdea = "";
                    lnqBill.CWLeader = "";
                    lnqBill.CWRatify = false;
                    lnqBill.GeneralManager = "";
                    lnqBill.GMIdrea = "";
                    lnqBill.GMRatify = false;

                    if (!DeleteList(dataContext, minorBill.BillNo, out error))
                    {
                        throw new Exception(error);
                    }

                    if (!InsertList(dataContext, minorBill.BillNo, listInfo, out error))
                    {
                        throw new Exception(error);
                    }

                    if (minorBill.PurchaseType == "零星采购" || minorBill.PurchaseType == "油品采购")
                    {
                        m_billMessageServer.PassFlowMessage(minorBill.BillNo.ToString(),
                            string.Format("【采购类别】：{0} 【申请部门】：{1} 【用途】：{2}   ※※※ 等待【备件库管理员】处理",
                            minorBill.PurchaseType, UniversalFunction.GetDeptName(minorBill.ApplicantDeptCode),
                            UniversalFunction.GetPurposeInfo(minorBill.PurposeCode).Purpose),
                            BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.备件仓库管理员.ToString());
                    }
                    else if (minorBill.PurchaseType == "量具采购" || minorBill.PurchaseType == "检具加工")
                    {
                        m_billMessageServer.PassFlowMessage(minorBill.BillNo.ToString(),
                            string.Format("【采购类别】：{0} 【申请部门】：{1} 【用途】：{2}   ※※※ 等待【量检具库管理员】处理",
                            minorBill.PurchaseType, UniversalFunction.GetDeptName(minorBill.ApplicantDeptCode),
                            UniversalFunction.GetPurposeInfo(minorBill.PurposeCode).Purpose),
                            BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.量检具库管理员.ToString());
                    }
                    else
                    {
                        m_billMessageServer.PassFlowMessage(minorBill.BillNo.ToString(),
                            string.Format("【采购类别】：{0} 【申请部门】：{1} 【用途】：{2}   ※※※ 等待【部门负责人】处理",
                            minorBill.PurchaseType, UniversalFunction.GetDeptName(minorBill.ApplicantDeptCode),
                            UniversalFunction.GetPurposeInfo(minorBill.PurposeCode).Purpose),
                            BillFlowMessage_ReceivedUserType.角色,
                            m_billMessageServer.GetDeptPrincipalRoleName(UniversalFunction.GetPersonnelInfo(minorBill.Applicant).部门编码).ToList());
                    }
                }
                else
                {
                    error = "数据重复";
                    throw new Exception(error);
                }

                dataContext.SubmitChanges();

                dataContext.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                dataContext.Transaction.Rollback();
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获得合计金额
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回金额</returns>
        public decimal GetSumPrice(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return GetSumPrice(ctx, billNo);
        }

        decimal GetSumPrice(DepotManagementDataContext ctx, string billNo)
        {
            decimal result = 0;

            var varData1 = from a in ctx.B_MinorPurchaseList
                           where a.BillNo == billNo
                           select new { SumPrice = a.Count * a.Price };

            if (varData1.Count() > 0)
            {
                result = Math.Round( varData1.Sum(k => k.SumPrice).Value, 2);
            }

            return result;
        }

        /// <summary>
        /// 操作业务
        /// </summary>
        /// <param name="minorBill">单据信息数据集</param>
        /// <param name="listInfo">单据明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool OperationInfo(B_MinorPurchaseBill minorBill, DataTable listInfo, out string error)
        {
            m_billMessageServer.BillType = "零星采购申请单";
            error = null;

            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            dataContext.Connection.Open();
            dataContext.Transaction = dataContext.Connection.BeginTransaction();

            try
            {
                var varData = from a in dataContext.B_MinorPurchaseBill
                              where a.BillNo == minorBill.BillNo
                              select a;

                decimal resultList = GetSumPrice(dataContext, minorBill.BillNo);

                if (varData.Count() == 1)
                {
                    B_MinorPurchaseBill lnqBill = varData.Single();

                    //将string转换成枚举类型
                    MinorPurchaseBillStatus billStatus =
                        GlobalObject.GeneralFunction.StringConvertToEnum<MinorPurchaseBillStatus>(lnqBill.BillStatus);

                    switch (billStatus)
                    {
                        case MinorPurchaseBillStatus.新建单据:
                            break;
                        case MinorPurchaseBillStatus.等待仓管确认:
                            lnqBill.BillStatus = MinorPurchaseBillStatus.等待部门负责人审核.ToString();
                            lnqBill.KFRY = BasicInfo.LoginID;
                            lnqBill.KFDate = ServerTime.Time;

                            if (!DeleteList(dataContext, minorBill.BillNo, out error))
                            {
                                throw new Exception(error);
                            }

                            if (!InsertList(dataContext, minorBill.BillNo, listInfo, out error))
                            {
                                throw new Exception(error);
                            }

                            m_billMessageServer.PassFlowMessage(minorBill.BillNo.ToString(),
                                string.Format("【采购类别】：{0} 【申请部门】：{1} 【用途】：{2}   ※※※ 等待【部门负责人】处理",
                                minorBill.PurchaseType, UniversalFunction.GetDeptName(minorBill.ApplicantDeptCode),
                                UniversalFunction.GetPurposeInfo(minorBill.PurposeCode).Purpose),
                                BillFlowMessage_ReceivedUserType.角色,
                                m_billMessageServer.GetDeptPrincipalRoleName(UniversalFunction.GetPersonnelInfo(lnqBill.Applicant).部门编码).ToList());
                            break;
                        case MinorPurchaseBillStatus.等待部门负责人审核:

                            lnqBill.DeptDirector = BasicInfo.LoginID;
                            lnqBill.DeptDirectorDate = ServerTime.Time;
                            lnqBill.DeptDirectorIdea = minorBill.DeptDirectorIdea;
                            lnqBill.DeptDirectorRatify = minorBill.DeptDirectorRatify;

                            if (Convert.ToBoolean(lnqBill.DeptDirectorRatify))
                            {
                                lnqBill.BillStatus = MinorPurchaseBillStatus.等待分管领导审核.ToString();

                                m_billMessageServer.PassFlowMessage(minorBill.BillNo.ToString(),
                                    string.Format("【采购类别】：{0} 【申请部门】：{1} 【用途】：{2}   ※※※ 等待【分管领导】处理",
                                    minorBill.PurchaseType, UniversalFunction.GetDeptName(minorBill.ApplicantDeptCode),
                                    UniversalFunction.GetPurposeInfo(minorBill.PurposeCode).Purpose),
                                    BillFlowMessage_ReceivedUserType.角色, 
                                    m_billMessageServer.GetDeptLeaderRoleName(UniversalFunction.GetPersonnelInfo(lnqBill.Applicant).部门编码).ToList());
                            }
                            else
                            {
                                lnqBill.BillStatus = "未通过";

                                List<string> noticeUser = new List<string>();
                                noticeUser.Add(lnqBill.Applicant);
                                noticeUser.Add(lnqBill.Compiler);

                                m_billMessageServer.EndFlowMessage(minorBill.BillNo.ToString(), 
                                    string.Format("{0}号零星采购申请单终止", minorBill.BillNo.ToString()), null, noticeUser);
                            }
                            break;
                        case MinorPurchaseBillStatus.等待确认日期:

                            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.采购员.ToString()))
                            {
                                lnqBill.ProcurementEngineer = BasicInfo.LoginID;

                                if (!DeleteList(dataContext, minorBill.BillNo, out error))
                                {
                                    throw new Exception(error);
                                }

                                if (!InsertList(dataContext, minorBill.BillNo, listInfo, out error))
                                {
                                    throw new Exception(error);
                                }

                                m_billMessageServer.PassFlowMessage(minorBill.BillNo.ToString(),
                                    string.Format("{0}号零星采购申请单修改了日期，请编制人确认", minorBill.BillNo.ToString()),
                                    BillFlowMessage_ReceivedUserType.用户, lnqBill.Compiler);
                            }
                            else
                            {
                                if (!DeleteList(dataContext, minorBill.BillNo, out error))
                                {
                                    throw new Exception(error);
                                }

                                if (!InsertList(dataContext, minorBill.BillNo, listInfo, out error))
                                {
                                    throw new Exception(error);
                                }

                                lnqBill.BillStatus = MinorPurchaseBillStatus.等待分管领导审核.ToString();

                                m_billMessageServer.PassFlowMessage(minorBill.BillNo.ToString(),
                                    string.Format("【采购类别】：{0} 【申请部门】：{1} 【用途】：{2}   ※※※ 等待【分管领导】处理",
                                    minorBill.PurchaseType, UniversalFunction.GetDeptName(minorBill.ApplicantDeptCode),
                                    UniversalFunction.GetPurposeInfo(minorBill.PurposeCode).Purpose),
                                    BillFlowMessage_ReceivedUserType.角色,
                                    m_billMessageServer.GetDeptLeaderRoleName(UniversalFunction.GetPersonnelInfo(lnqBill.Applicant).部门编码).ToList());
                            }

                            break;

                        case  MinorPurchaseBillStatus.等待高级负责人审批:

                            lnqBill.DeptDirector += "," + BasicInfo.LoginID;
                            lnqBill.DeptDirectorDate = ServerTime.Time;
                            lnqBill.DeptDirectorIdea = minorBill.DeptDirectorIdea;
                            lnqBill.DeptDirectorRatify = minorBill.DeptDirectorRatify;                            

                            if (Convert.ToBoolean(lnqBill.DeptDirectorRatify))
                            {
                                lnqBill.BillStatus = MinorPurchaseBillStatus.等待分管领导审核.ToString();

                                m_billMessageServer.PassFlowMessage(minorBill.BillNo.ToString(),
                                    string.Format("【采购类别】：{0} 【申请部门】：{1} 【用途】：{2}   ※※※ 等待【分管领导】处理",
                                    minorBill.PurchaseType, UniversalFunction.GetDeptName(minorBill.ApplicantDeptCode),
                                    UniversalFunction.GetPurposeInfo(minorBill.PurposeCode).Purpose),
                                    BillFlowMessage_ReceivedUserType.角色, 
                                    m_billMessageServer.GetDeptLeaderRoleName(UniversalFunction.GetPersonnelInfo(lnqBill.Applicant).部门编码).ToList());
                            }
                            else
                            {
                                lnqBill.BillStatus = "未通过";

                                List<string> noticeUser = new List<string>();

                                noticeUser.Add(lnqBill.Compiler);
                                noticeUser.Add(lnqBill.Applicant);

                                m_billMessageServer.EndFlowMessage(minorBill.BillNo.ToString(),
                                    string.Format("{0}号零星采购申请单终止", minorBill.BillNo.ToString()), null, noticeUser);
                            }

                            break;

                        case MinorPurchaseBillStatus.等待分管领导审核:

                            lnqBill.Leader = BasicInfo.LoginID;
                            lnqBill.LeaderDate = ServerTime.Time;
                            lnqBill.LeaderIdea = minorBill.LeaderIdea;
                            lnqBill.LeaderRatify = minorBill.LeaderRatify;

                            if (Convert.ToBoolean(lnqBill.LeaderRatify))
                            {
                                #region 2017-10-19 夏石友按照新的经济权限修订（容大公司经营业务活动审批权限）
                                //if (resultList >= 2000)
                                #endregion
                                {
                                    lnqBill.BillStatus = MinorPurchaseBillStatus.等待财务审核.ToString();
                                    m_billMessageServer.PassFlowMessage(minorBill.BillNo.ToString(),
                                        string.Format("【采购类别】：{0} 【申请部门】：{1} 【用途】：{2}   ※※※ 等待【财务分管领导】处理",
                                        minorBill.PurchaseType, UniversalFunction.GetDeptName(minorBill.ApplicantDeptCode),
                                        UniversalFunction.GetPurposeInfo(minorBill.PurposeCode).Purpose),
                                        BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.财务分管领导.ToString());
                                }
                                #region 2017-10-19 夏石友按照新的经济权限修订（容大公司经营业务活动审批权限）
                                //else
                                //{
                                //    lnqBill.BillStatus = MinorPurchaseBillStatus.等待采购部调配人员.ToString();

                                //    m_billMessageServer.PassFlowMessage(minorBill.BillNo.ToString(),
                                //        string.Format("【采购类别】：{0} 【申请部门】：{1} 【用途】：{2}   ※※※ 等待【采购部账务管理员】处理",
                                //        minorBill.PurchaseType, UniversalFunction.GetDeptName(minorBill.ApplicantDeptCode),
                                //        UniversalFunction.GetPurposeInfo(minorBill.PurposeCode).Purpose),
                                //        BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.采购账务管理员.ToString());
                                //}
                                #endregion
                            }
                            else
                            {
                                lnqBill.BillStatus = "未通过";

                                List<string> noticeUser = new List<string>();
                                noticeUser.Add(lnqBill.Applicant);
                                noticeUser.Add(lnqBill.Compiler);

                                m_billMessageServer.EndFlowMessage(minorBill.BillNo.ToString(),
                                    string.Format("{0}号零星采购申请单终止", minorBill.BillNo.ToString()), null, noticeUser);
                            }

                            break;
                        case MinorPurchaseBillStatus.等待财务审核:

                            lnqBill.BillStatus = MinorPurchaseBillStatus.等待总经理审核.ToString();
                            lnqBill.CWLeader = BasicInfo.LoginID;
                            lnqBill.CWLeaderDate = ServerTime.Time;
                            lnqBill.CWIdea = minorBill.CWIdea;
                            lnqBill.CWRatify = minorBill.CWRatify;                            

                            if (Convert.ToBoolean(lnqBill.CWRatify))
                            {
                                #region 2017-10-19 夏石友按照新的经济权限修订（容大公司经营业务活动审批权限）
                                //if (resultList <= 5000)
                                //{
                                //    lnqBill.BillStatus = MinorPurchaseBillStatus.等待采购部调配人员.ToString();

                                //    m_billMessageServer.PassFlowMessage(minorBill.BillNo.ToString(),
                                //        string.Format("【采购类别】：{0} 【申请部门】：{1} 【用途】：{2}   ※※※ 等待【采购部账务管理员】处理",
                                //        minorBill.PurchaseType, UniversalFunction.GetDeptName(minorBill.ApplicantDeptCode),
                                //        UniversalFunction.GetPurposeInfo(minorBill.PurposeCode).Purpose), BillFlowMessage_ReceivedUserType.用户,
                                //        CE_RoleEnum.采购账务管理员.ToString());
                                //}
                                //else
                                #endregion
                                {
                                    lnqBill.BillStatus = MinorPurchaseBillStatus.等待总经理审核.ToString();

                                    m_billMessageServer.PassFlowMessage(minorBill.BillNo.ToString(),
                                        string.Format("【采购类别】：{0} 【申请部门】：{1} 【用途】：{2}   ※※※ 等待【总经理】处理",
                                        minorBill.PurchaseType, UniversalFunction.GetDeptName(minorBill.ApplicantDeptCode),
                                        UniversalFunction.GetPurposeInfo(minorBill.PurposeCode).Purpose),
                                        BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.总经理.ToString());
                                }
                            }
                            else
                            {
                                lnqBill.BillStatus = "未通过";

                                List<string> noticeUser = new List<string>();
                                noticeUser.Add(lnqBill.Applicant);
                                noticeUser.Add(lnqBill.Compiler);

                                m_billMessageServer.EndFlowMessage(minorBill.BillNo.ToString(),
                                    string.Format("{0}号零星采购申请单终止", minorBill.BillNo.ToString()), null, noticeUser);
                            }
                            break;
                        case MinorPurchaseBillStatus.等待总经理审核:

                            lnqBill.BillStatus = MinorPurchaseBillStatus.等待采购部调配人员.ToString();
                            lnqBill.GeneralManager = BasicInfo.LoginID;
                            lnqBill.GMDate = ServerTime.Time;
                            lnqBill.GMIdrea = minorBill.GMIdrea;
                            lnqBill.GMRatify = minorBill.GMRatify;

                            if (Convert.ToBoolean(lnqBill.GMRatify))
                            {
                                m_billMessageServer.PassFlowMessage(minorBill.BillNo.ToString(),
                                    string.Format("【采购类别】：{0} 【申请部门】：{1} 【用途】：{2}   ※※※ 等待【采购部账务管理员】处理",
                                    minorBill.PurchaseType, UniversalFunction.GetDeptName(minorBill.ApplicantDeptCode),
                                    UniversalFunction.GetPurposeInfo(minorBill.PurposeCode).Purpose), BillFlowMessage_ReceivedUserType.用户, 
                                    CE_RoleEnum.采购账务管理员.ToString());
                            }
                            else
                            {
                                lnqBill.BillStatus = MinorPurchaseBillStatus.已完成.ToString();

                                List<string> noticeUser = new List<string>();
                                noticeUser.Add(lnqBill.Applicant);
                                noticeUser.Add(lnqBill.Compiler);

                                m_billMessageServer.EndFlowMessage(minorBill.BillNo.ToString(),
                                    string.Format("{0}号零星采购申请单终止", minorBill.BillNo.ToString()), null, noticeUser);
                            }
                            break;
                        case MinorPurchaseBillStatus.等待采购部调配人员:
                            break;
                        case MinorPurchaseBillStatus.等待采购工程师确认采购:
                            lnqBill.ProcurementEngineer = BasicInfo.LoginID;

                            for (int i = 0; i < listInfo.Rows.Count; i++)
                            {
                                var result = from e in dataContext.B_MinorPurchaseList
                                             where e.GoodsCode == listInfo.Rows[i]["图号型号"].ToString()
                                             && e.GoodsName == listInfo.Rows[i]["物品名称"].ToString()
                                             && e.Spec == listInfo.Rows[i]["规格"].ToString()
                                             && e.BillNo == lnqBill.BillNo
                                             select e;

                                B_MinorPurchaseList list = result.Single();

                                list.GoodsStatus = "等待到货"; //listInfo.Rows[i]["物品状态"].ToString();
                                list.Provider = listInfo.Rows[i]["供应商"].ToString();
                                list.Price = Convert.ToDecimal(listInfo.Rows[i]["预算价格"]);
                                list.FinishDate = ServerTime.Time;

                                dataContext.SubmitChanges();
                            }

                            lnqBill.BillStatus = MinorPurchaseBillStatus.等待确认到货.ToString();

                            m_billMessageServer.PassFlowMessage(minorBill.BillNo.ToString(),
                                string.Format("【采购类别】：{0} 【申请部门】：{1} 【用途】：{2}   ※※※ 等待【{3}】处理",
                                minorBill.PurchaseType, UniversalFunction.GetDeptName(minorBill.ApplicantDeptCode),
                                UniversalFunction.GetPurposeInfo(minorBill.PurposeCode).Purpose, UniversalFunction.GetPersonnelName(BasicInfo.LoginID)),
                                BillFlowMessage_ReceivedUserType.用户, BasicInfo.LoginID.ToString());
                            break;
                        case MinorPurchaseBillStatus.等待确认到货:

                            lnqBill.ProcurementEngineer = BasicInfo.LoginID;
                            bool flagReach = false;

                            for (int i = 0; i < listInfo.Rows.Count; i++)
                            {
                                var result = from e in dataContext.B_MinorPurchaseList
                                             where e.GoodsCode == listInfo.Rows[i]["图号型号"].ToString()
                                             && e.GoodsName == listInfo.Rows[i]["物品名称"].ToString()
                                             && e.Spec == listInfo.Rows[i]["规格"].ToString()
                                             && e.BillNo == lnqBill.BillNo
                                             select e;

                                B_MinorPurchaseList list = result.Single();

                                list.GoodsStatus = listInfo.Rows[i]["物品状态"].ToString();
                                list.Provider = listInfo.Rows[i]["供应商"].ToString();
                                list.Price = Convert.ToDecimal(listInfo.Rows[i]["预算价格"]);
                                list.FinishDate = ServerTime.Time;

                                if (list.GoodsStatus != "已到货")
                                {
                                    flagReach = true;
                                }

                                dataContext.SubmitChanges();
                            }

                            if (flagReach)
                            {
                                lnqBill.BillStatus = MinorPurchaseBillStatus.等待确认到货.ToString();
                                m_billMessageServer.PassFlowMessage(minorBill.BillNo.ToString(),
                                    string.Format("【采购类别】：{0} 【申请部门】：{1} 【用途】：{2}   ※※※ 等待【{3}】处理",
                                    minorBill.PurchaseType, UniversalFunction.GetDeptName(minorBill.ApplicantDeptCode),
                                    UniversalFunction.GetPurposeInfo(minorBill.PurposeCode).Purpose, UniversalFunction.GetPersonnelName(BasicInfo.LoginID)),
                                    BillFlowMessage_ReceivedUserType.用户, BasicInfo.LoginID.ToString());
                            }
                            else
                            {
                                lnqBill.BillStatus = MinorPurchaseBillStatus.已完成.ToString();
                                lnqBill.FinishDate = ServerTime.Time;

                                List<string> noticeUser = new List<string>();
                                noticeUser.Add(lnqBill.Applicant);
                                noticeUser.Add(lnqBill.Compiler);

                                m_billMessageServer.EndFlowMessage(minorBill.BillNo.ToString(),
                                    string.Format("{0}号零星采购申请单已经完成", minorBill.BillNo.ToString()), null, noticeUser);
                            }
                            break;
                        case MinorPurchaseBillStatus.已完成:
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    error = "数据重复或者为空";
                    throw new Exception(error);
                }

                dataContext.SubmitChanges();
                dataContext.Transaction.Commit();

                return true;
            }
            catch (Exception ex)
            {
                dataContext.Transaction.Rollback();
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 在审批通过后，采购部文员分配采购工程师
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="workID">采购工程师工号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true失败返回false</returns>
        public bool UpdateMinorPurchaseBill(string billNo,string workID, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.B_MinorPurchaseBill
                             where a.BillNo == billNo
                             select a;

                if (result.Count() == 1)
                {
                    B_MinorPurchaseBill bill = result.Single();

                    bill.BillStatus = MinorPurchaseBillStatus.等待采购工程师确认采购.ToString();
                    bill.ProcurementEngineer = workID;
                    bill.DeployMan = BasicInfo.LoginID;
                    bill.DeployDate = ServerTime.Time;

                    dataContxt.SubmitChanges();

                    m_billMessageServer.BillType = "零星采购申请单";
                    m_billMessageServer.PassFlowMessage(billNo,
                                    string.Format("【采购类别】：{0} 【申请部门】：{1} 【用途】：{2}   ※※※ 等待【{3}】处理",
                                    bill.PurchaseType, UniversalFunction.GetDeptName(bill.ApplicantDeptCode),
                                    UniversalFunction.GetPurposeInfo(bill.PurposeCode).Purpose, CE_RoleEnum.采购员.ToString()),
                              BillFlowMessage_ReceivedUserType.用户, workID);
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="strDJH">单据号</param>
        /// <param name="strBillStatus">单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="strRebackReason">回退原因</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool ReturnBill(string strDJH, string strBillStatus, out string error, string strRebackReason)
        {
            error = null;

            try
            {
                m_billMessageServer.BillType = "零星采购申请单";
                DepotManagementDataContext dateContxt = CommentParameter.DepotDataContext;

                var varData = from a in dateContxt.B_MinorPurchaseBill
                              where a.BillNo == strDJH
                              select a;

                string strMsg = "";

                if (varData.Count() == 1)
                {
                    B_MinorPurchaseBill minorPurchase = varData.Single();
                    MinorPurchaseBillStatus billStatus =
                        GlobalObject.GeneralFunction.StringConvertToEnum<MinorPurchaseBillStatus>(strBillStatus);

                    switch (billStatus)
                    {
                        case MinorPurchaseBillStatus.新建单据:
                            strMsg = string.Format("{0}号零星采购申请单已回退，请您重新处理单据; 回退原因为:" + strRebackReason, strDJH);
                            m_billMessageServer.PassFlowMessage(strDJH, strMsg,BillFlowMessage_ReceivedUserType.用户,minorPurchase.Compiler);

                            minorPurchase.BillStatus = billStatus.ToString();
                            minorPurchase.CWLeader = "";
                            //minorPurchase.CWIdea = "";
                            //minorPurchase.CWRatify = false;
                            minorPurchase.DeptDirector = "";
                            //minorPurchase.DeptDirectorIdea = "";
                            //minorPurchase.DeptDirectorRatify = false;
                            minorPurchase.GeneralManager = "";
                            //minorPurchase.GMIdrea = "";
                            //minorPurchase.GMRatify = false;
                            minorPurchase.KFRY = "";
                            minorPurchase.Leader = "";
                            //minorPurchase.LeaderIdea = "";
                            //minorPurchase.LeaderRatify = false;
                            minorPurchase.ProcurementEngineer = "";
                           
                            break;

                        case MinorPurchaseBillStatus.等待确认日期:
                            strMsg = string.Format("{0}号零星采购申请单已回退，请您重新处理单据; 回退原因为:" + strRebackReason, strDJH);

                            m_billMessageServer.PassFlowMessage(strDJH, strMsg,
                              BillFlowMessage_ReceivedUserType.角色,CE_RoleEnum.采购员.ToString());

                            minorPurchase.BillStatus = billStatus.ToString();
                            minorPurchase.CWLeader = "";
                            //minorPurchase.CWIdea = "";
                            //minorPurchase.CWRatify = false;
                            minorPurchase.DeptDirector = "";
                            //minorPurchase.DeptDirectorIdea = "";
                            //minorPurchase.DeptDirectorRatify = false;
                            minorPurchase.GeneralManager = "";
                            //minorPurchase.GMIdrea = "";
                            //minorPurchase.GMRatify = false;
                            minorPurchase.KFRY = "";
                            minorPurchase.Leader = "";
                            //minorPurchase.LeaderIdea = "";
                            //minorPurchase.LeaderRatify = false;
                            minorPurchase.ProcurementEngineer = "";

                            break;

                        case MinorPurchaseBillStatus.等待部门负责人审核:
                            strMsg = string.Format("{0}号零星采购申请单已回退，请您重新处理单据; 回退原因为:" + strRebackReason, strDJH);

                            m_billMessageServer.PassFlowMessage(strDJH, strMsg,
                              BillFlowMessage_ReceivedUserType.角色,
                              m_billMessageServer.GetDeptPrincipalRoleName(UniversalFunction.GetPersonnelInfo(minorPurchase.Compiler).部门编码).ToList());

                            minorPurchase.BillStatus = billStatus.ToString();

                            minorPurchase.CWLeader = "";
                            //minorPurchase.CWIdea = "";
                            //minorPurchase.CWRatify = false;
                            minorPurchase.DeptDirector = "";
                            //minorPurchase.DeptDirectorIdea = "";
                            //minorPurchase.DeptDirectorRatify = false;
                            minorPurchase.GeneralManager = "";
                            //minorPurchase.GMIdrea = "";
                            //minorPurchase.GMRatify = false;
                            minorPurchase.Leader = "";
                            //minorPurchase.LeaderIdea = "";
                            //minorPurchase.LeaderRatify = false;

                            break;

                        case MinorPurchaseBillStatus.等待采购部调配人员:

                            strMsg = string.Format("{0}号零星采购申请单已回退，请您重新处理单据; 回退原因为:" + strRebackReason, strDJH);

                            m_billMessageServer.PassFlowMessage(minorPurchase.BillNo.ToString(), strMsg, 
                                BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.采购账务管理员.ToString());

                            minorPurchase.BillStatus = billStatus.ToString();

                            minorPurchase.ProcurementEngineer = "";
                            minorPurchase.DeployMan = "";
                            minorPurchase.DeployDate = null;
                            break;
                        default:
                            break;
                    }

                    dateContxt.SubmitChanges();

                    return true;
                }
                else
                {
                    error = "数据不唯一或者为空";

                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;

                return false;
            }
        }

        /// <summary>
        /// 通过物品ID获得物品库存信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回库存信息</returns>
        public string GetGoodsStock(int goodsID)
        {
            string sql = "SELECT SUM(库存数量) FROM [dbo].[View_S_Stock] " +
                         " where 物品ID = " + goodsID;

            sql += "group by 物品ID";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 获得时间范围内的采购变更处置单的所有信息
        /// </summary>
        /// <param name="starTime">起始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <param name="status">单据状态</param>
        /// <returns>返回数据集</returns>
        public DataTable GetMinorPurchaseChangeBill(DateTime starTime,DateTime endTime,string status)
        {
            string sql = "select * from View_B_MinorPurchaseChangeBill where 申请日期>='" + starTime + "'" +
                         " and 申请日期<='" + endTime + "'";

            if (status != "全部")
            {
                sql += " and 单据状态 = '" + status + "' order by 关联单号";
            }

            return GlobalObject.DatabaseServer.QueryInfo(sql);
        }

        /// <summary>
        /// 新增采购变更处置单
        /// </summary>
        /// <param name="changBill">采购变更处置信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>新增成功返回true，失败返回false</returns>
        public bool InsertChangeBill(B_MinorPurchaseChangeBill changBill, out string error)
        {
            error = "";
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                dataContxt.Connection.Open();
                dataContxt.Transaction = dataContxt.Connection.BeginTransaction();

                var result = from a in dataContxt.B_MinorPurchaseChangeBill
                             where a.AssociateBillNo == changBill.AssociateBillNo
                             && a.OldGoodsCode == changBill.OldGoodsCode
                             && a.OldGoodsName == changBill.OldGoodsName
                             && a.OldGoodsSpec == changBill.OldGoodsSpec
                             select a;

                if (result.Count() > 0)
                {
                    error = "已经提交过此零件的变更,请重新选择旧件";
                    return false;
                }

                dataContxt.B_MinorPurchaseChangeBill.InsertOnSubmit(changBill);
                dataContxt.SubmitChanges();

                result = from a in dataContxt.B_MinorPurchaseChangeBill
                             where a.AssociateBillNo == changBill.AssociateBillNo
                             && a.OldGoodsCode == changBill.OldGoodsCode
                             && a.OldGoodsName == changBill.OldGoodsName
                             && a.OldGoodsSpec == changBill.OldGoodsSpec
                             select a;

                var resultMinor = from a in dataContxt.B_MinorPurchaseBill
                                  where a.BillNo == changBill.AssociateBillNo
                                  select a;

                m_billMessageServer.BillType = "零星采购变更处置单";

                m_billMessageServer.SendNewFlowMessage(result.Single().ID.ToString(),
                            string.Format("{0}号零星采购变更处置单已提交，等待请购人确认", result.Single().ID.ToString()),
                            BillFlowMessage_ReceivedUserType.用户, resultMinor.Single().Applicant);

                dataContxt.SubmitChanges();
                dataContxt.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                dataContxt.Transaction.Rollback();
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改采购变更处置单
        /// </summary>
        /// <param name="changBill">采购变更处置信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>新增成功返回true，失败返回false</returns>
        public bool UpdateChangeBill(B_MinorPurchaseChangeBill changBill, out string error)
        {
            error = "";
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            m_billMessageServer.BillType = "零星采购变更处置单";

            try
            {
                dataContxt.Connection.Open();
                dataContxt.Transaction = dataContxt.Connection.BeginTransaction();

                var result = from a in dataContxt.B_MinorPurchaseChangeBill
                             where a.AssociateBillNo == changBill.AssociateBillNo
                             && a.OldGoodsCode == changBill.OldGoodsCode
                             && a.OldGoodsName == changBill.OldGoodsName
                             && a.OldGoodsSpec == changBill.OldGoodsSpec
                             select a;

                var resultMinor = from a in dataContxt.B_MinorPurchaseBill
                                  where a.BillNo == changBill.AssociateBillNo
                                  select a;

                if (result.Count() == 1)
                {
                    B_MinorPurchaseChangeBill minor = result.Single();
                    
                    if (changBill.BillStatus == "等待请购人确认")
                    {
                        minor.NewGoodsCode = changBill.NewGoodsCode;
                        minor.NewGoodsName = changBill.NewGoodsName;
                        minor.NewGoodsSpec = changBill.NewGoodsSpec;
                        minor.ChangeReason = changBill.ChangeReason;
                        minor.BillStatus = changBill.BillStatus;

                        dataContxt.SubmitChanges();

                        m_billMessageServer.PassFlowMessage(minor.ID.ToString(),
                              string.Format("{0}号零星采购变更处置单已提交，等待请购人确认", minor.ID.ToString()),
                              BillFlowMessage_ReceivedUserType.用户, resultMinor.Single().Applicant);
                    }
                    else if (changBill.BillStatus == "等待负责人审核")
                    {
                        minor.Confirmor = changBill.Confirmor;
                        minor.ConfirmDate = changBill.ConfirmDate;
                        minor.BillStatus = changBill.BillStatus;

                        dataContxt.SubmitChanges();

                        m_billMessageServer.PassFlowMessage(minor.ID.ToString(),
                              string.Format("{0}号零星采购变更处置单已确认，请部门领导审核", minor.ID.ToString()),
                              BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.采购负责人.ToString());
                    }
                    else if (changBill.BillStatus == "已完成")
                    {
                        minor.DeptDirectDate = changBill.DeptDirectDate;
                        minor.DeptDirector = changBill.DeptDirector;
                        minor.BillStatus = changBill.BillStatus;

                        var resultList = from c in dataContxt.B_MinorPurchaseList
                                         where c.BillNo == minor.AssociateBillNo
                                         select c;

                        foreach (B_MinorPurchaseList item in resultList)
                        {
                            if (item.GoodsCode == minor.OldGoodsCode && item.GoodsName == minor.OldGoodsName
                                && item.Spec == minor.OldGoodsSpec)
                            {
                                item.GoodsCode = changBill.NewGoodsCode;
                                item.GoodsName = changBill.NewGoodsName;
                                item.Spec = changBill.NewGoodsSpec;

                                break;
                            }                            
                        }

                        dataContxt.SubmitChanges();

                        List<string> noticeUser = new List<string>();

                        noticeUser.Add(minor.Applicant);
                        noticeUser.Add(resultMinor.Single().Applicant);

                        m_billMessageServer.EndFlowMessage(minor.ID.ToString(),
                      string.Format("{0}号零星采购变更处置单已完成", minor.ID.ToString()), null, noticeUser);
                    }
                }
                else
                {
                    error = "数据有误！请重新核对";
                    return false;
                }

                dataContxt.SubmitChanges();
                dataContxt.Transaction.Commit();

                return true;
            }
            catch (Exception ex)
            {
                dataContxt.Transaction.Rollback();
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除采购变更处置单
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>删除成功返回true，失败返回false</returns>
        public bool DeleteChangeBill(int id, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.B_MinorPurchaseChangeBill
                             where a.ID == id
                             select a;

                if (result.Count() > 1)
                {
                    error = "数据信息有误！";
                    return false;
                }

                dataContxt.B_MinorPurchaseChangeBill.DeleteAllOnSubmit(result);
                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除采购变更处置单
        /// </summary>
        /// <param name="associateBillNo">关联单号</param>
        /// <param name="error">错误信息</param>
        /// <returns>删除成功返回true，失败返回false</returns>
        public bool DeleteChangeBill(string associateBillNo, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.B_MinorPurchaseChangeBill
                             where a.AssociateBillNo == associateBillNo
                             select a;

                if (result.Count() > 0)
                {
                    dataContxt.B_MinorPurchaseChangeBill.DeleteAllOnSubmit(result);
                    dataContxt.SubmitChanges();
                }                

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 通过关联号查询变更单是否走完
        /// </summary>
        /// <param name="associateBillNo">关联单号</param>
        /// <returns>完成返回true，未完成返回false</returns>
        public bool IsFinishMinorPur(string associateBillNo)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from a in dataContxt.B_MinorPurchaseChangeBill
                         where a.AssociateBillNo == associateBillNo
                         && a.BillStatus != "已完成"
                         select a;

            if (result.Count() > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 获得采购申请单的统计报表
        /// </summary>
        /// <param name="startYear">查询起始日期的年</param>
        /// <param name="startMonth">查询起始日期的月</param>
        /// <param name="endYear">查询截止日期的年</param>
        /// <param name="endMonth">查询截止日期的月</param>
        /// <param name="dept">需要查询的部门</param>
        /// <returns>返回统计报表</returns>
        public DataTable GetStatisticsTable(string startYear,string startMonth,string endYear,string endMonth,string dept)
        {
            string sql = "SELECT TOP (100) PERCENT a.申请部门, COUNT(*) AS 总申请零件数, " +
                         " CASE WHEN SUM(c.单价) IS NULL THEN '0' ELSE SUM(c.单价) END AS 金额" +
                         " FROM dbo.View_B_MinorPurchaseBill AS a INNER JOIN" +
                         " dbo.B_MinorPurchaseList AS b ON a.单据号 = b.BillNo LEFT OUTER JOIN" +
                         " dbo.View_B_OrderFormGoods AS c ON c.图号型号 = b.GoodsCode AND " +
                         " c.物品名称 = b.GoodsName AND c.规格 = b.Spec AND " +
                         " c.合同号 = (select BargainNumber from dbo.B_BargainInfo where MinorPurchaseBillNo=a.单据号)"+
                         " where a.编制日期 >= '" + startYear + "-" + startMonth + "-01'" +
                         " and a.编制日期<='" + endYear + "-" + endMonth + "-25'";

            if (dept == "全部" || dept == " " || dept == "")
            {
                sql += " group by 申请部门 order by 总申请零件数";
            }
            else
            {
                sql += " and a.申请部门 = '" + dept + "' group by 申请部门 order by 总申请零件数";
            }

            return GlobalObject.DatabaseServer.QueryInfo(sql);
        }

        /// <summary>
        /// 修改单据中已打印的状态
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>修改成功返回true失败返回false</returns>
        public bool UpdatePrintStatus(string billNo,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.B_MinorPurchaseBill
                             where a.BillNo == billNo
                             select a;

                if (result.Count() == 1)
                {
                    B_MinorPurchaseBill bill = result.Single();

                    bill.Isprint = true;
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获得明细单条记录
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <returns>返回单条LNQ记录</returns>
        public B_MinorPurchaseList GetListSingle(string billNo, string goodsCode,string goodsName,string spec)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.B_MinorPurchaseList
                          where a.BillNo == billNo
                          && a.GoodsCode == goodsCode && a.GoodsName == goodsName
                          && a.Spec == spec
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
        /// 通过用途编号获得用途名称
        /// </summary>
        /// <param name="purposeCode">用途编号</param>
        /// <returns>成功返回用途名称，失败返回Null</returns>
        public string GetPurposeNameByCode(string purposeCode)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.BASE_MaterialRequisitionPurpose
                          where a.Code == purposeCode
                          select a;

            if (varData.Count() != 1)
            {
                return null;
            }
            else
            {
                return varData.Single().Purpose;
            }
        }

        /// <summary>
        /// 同一零件近期是否购买过
        /// </summary>
        /// <param name="goodsID">零件ID</param>
        /// <param name="billNo">单据号</param>
        /// <returns>返回数据集</returns>
        public DataTable GetPart(int goodsID,string billNo)
        {
            string sql = "select * from B_MinorPurchaseList where BillNo in (" +
                       " select BillNo from B_MinorPurchaseBill " +
                       " where GETDATE()- B_MinorPurchaseBill.CompileDate <=91" +
                       " and BillStatus<>'已完成' ) and BillNo <>'" + billNo + "' and GoodsID=" + goodsID;

            return GlobalObject.DatabaseServer.QueryInfo(sql);
        }
    }
}
