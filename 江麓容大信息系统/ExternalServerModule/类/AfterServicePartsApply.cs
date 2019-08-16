/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  AfterServicePartsApply.cs
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
using ServerModule;
using DBOperate;
using PlatformManagement;
using GlobalObject;

namespace Service_Peripheral_External
{
    /// <summary>
    /// 售后配件申请服务
    /// </summary>
    class AfterServicePartsApply : IAfterServicePartsApply
    {
        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 调运单服务组件
        /// </summary>
        IManeuverServer m_serverManeuver = Service_Peripheral_External.ServerModuleFactory.GetServerModule<IManeuverServer>();

        /// <summary>
        /// 业务操作服务
        /// </summary>
        IBusinessOperation m_serverBusiness = Service_Peripheral_External.ServerModuleFactory.GetServerModule<IBusinessOperation>();

        /// <summary>
        /// 单据编号服务
        /// </summary>
        IAssignBillNoServer m_serverBillNo = ServerModule.ServerModuleFactory.GetServerModule<IAssignBillNoServer>();

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.Out_AfterServicePartsApplyBill
                          where a.Bill_ID == billNo
                          select a;

            if (varData == null || varData.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 检查某单据是否存在
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(string billNo)
        {
            string strSql = "SELECT * FROM [DepotManagement].[dbo].[Out_AfterServicePartsApplyBill] where Bill_ID = '" + billNo + "'";

            System.Data.DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp != null && dtTemp.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得所有单据信息
        /// </summary>
        /// <param name="billStatus">单据状态</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回Table</returns>
        public DataTable GetAllBillInfo(string billStatus, DateTime startTime, DateTime endTime)
        {
            string strSql = "select * from View_Out_AfterServicePartsApplyBill  where 申请日期 >= '" + startTime + "' and 申请日期 <= '" + endTime + "' ";


            if (billStatus != "全部")
            {
                strSql += " and 单据状态 = '" + billStatus + "'";
            }

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得某一单据明细信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回Table</returns>
        public DataTable GetListInfo(string billNo)
        {
            string strSql = "select * from View_Out_AfterServicePartsApplyList  where Bill_ID = '" + billNo + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得一条单据信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回数据集</returns>
        public Out_AfterServicePartsApplyBill GetBillInfo(string billNo)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varData = from a in dataContext.Out_AfterServicePartsApplyBill
                          where a.Bill_ID == billNo
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
        /// 插入数据
        /// </summary>
        /// <param name="afterService">数据集</param>
        /// <param name="listInfo">明细表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool InsertBill(Out_AfterServicePartsApplyBill afterService, DataTable listInfo, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.Out_AfterServicePartsApplyBill
                              where a.Bill_ID == afterService.Bill_ID
                              select a;
                if (varData.Count() == 0)
                {
                    afterService.Bill_ID = m_serverBillNo.AssignNewNo(this, "售后配件申请单");
                    afterService.BillStatus = "等待主管审核";
                    afterService.Proposer = BasicInfo.LoginName;
                    afterService.ProposerTime = ServerTime.Time;

                    if (!InsertListInfo(dataContext, afterService.Bill_ID, listInfo, out error))
                    {
                        return false;
                    }

                    dataContext.Out_AfterServicePartsApplyBill.InsertOnSubmit(afterService);

                    m_billMessageServer.DestroyMessage(afterService.Bill_ID);
                    m_billMessageServer.SendNewFlowMessage(afterService.Bill_ID,
                        string.Format("{0}号售后配件申请单已提交，请营销主管审核", afterService.Bill_ID), CE_RoleEnum.营销主管);

                }
                else if (varData.Count() == 1)
                {
                    Out_AfterServicePartsApplyBill lnqBill = varData.Single();

                    lnqBill._4SLinkMan = afterService._4SLinkMan;
                    lnqBill._4SPhone = afterService._4SPhone;
                    lnqBill.Address = afterService.Address;
                    lnqBill.ApplyState = afterService.ApplyState;
                    lnqBill.CVTType = afterService.CVTType;
                    lnqBill.InStorageID = afterService.InStorageID;
                    lnqBill.OutStorageID = afterService.OutStorageID;
                    lnqBill.ProductCode = afterService.ProductCode;
                    lnqBill.Proposer = BasicInfo.LoginName;
                    lnqBill.ProposerTime = ServerTime.Time;
                    lnqBill.Remark = afterService.Remark;
                    lnqBill.ServiceErea = afterService.ServiceErea;

                    if (!DeleteListInfo(dataContext,lnqBill.Bill_ID,out error))
                    {
                        return false;
                    }

                    if (!InsertListInfo(dataContext,lnqBill.Bill_ID,listInfo,out error))
                    {
                        return false;
                    }


                    m_billMessageServer.DestroyMessage(afterService.Bill_ID);
                    m_billMessageServer.SendNewFlowMessage(afterService.Bill_ID,
                        string.Format("{0}号售后配件申请单已提交，请营销主管审核", afterService.Bill_ID), CE_RoleEnum.营销主管);

                }
                else
                {
                    error = "数据重复";
                    return false;
                }

                dataContext.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }

        }

        /// <summary>
        /// 审核单据
        /// </summary>
        /// <param name="afterService">数据集</param>
        /// <param name="listInfo">明细表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool VerifyBill(Out_AfterServicePartsApplyBill afterService, DataTable listInfo, out string error)
        {
            error = null;

            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;
            dataContext.Connection.Open();
            dataContext.Transaction = dataContext.Connection.BeginTransaction();

            try
            {

                var varData = from a in dataContext.Out_AfterServicePartsApplyBill
                              where a.Bill_ID == afterService.Bill_ID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                    throw new Exception(error);
                }
                else
                {
                    Out_AfterServicePartsApplyBill lnqBill = varData.Single();

                    lnqBill.BillStatus = "已完成";
                    lnqBill.Verify = BasicInfo.LoginName;
                    lnqBill.VerifyTime = ServerTime.Time;

                    if (!DeleteListInfo(dataContext, lnqBill.Bill_ID, out error))
                    {
                        throw new Exception(error);
                    }

                    if (!InsertListInfo(dataContext, lnqBill.Bill_ID, listInfo, out error))
                    {
                        throw new Exception(error);
                    }

                    if (!CreateManeuver(dataContext, lnqBill, listInfo, out error))
                    {
                        throw new Exception(error);
                    }
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
        /// 创建调运单
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="afterService">数据集</param>
        /// <param name="listInfo">明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool CreateManeuver(DepotManagementDataContext dataContext ,Out_AfterServicePartsApplyBill afterService,DataTable listInfo,out string error)
        {
            error = null;

            try
            {
                Out_ManeuverBill lnqBill = new Out_ManeuverBill();

                lnqBill.AssociatedBillNo = afterService.Bill_ID;
                lnqBill.Bill_ID = "由系统自动生成";
                lnqBill.BillStatus = "等待出库";
                lnqBill.InStorageID = afterService.InStorageID;
                lnqBill.OutStorageID = afterService.OutStorageID;
                lnqBill.Proposer = afterService.Proposer;
                lnqBill.ProposerTime = afterService.ProposerTime;
                lnqBill.Remark = afterService.Remark;

                listInfo.Columns.Add("收货数量");
                listInfo.Columns.Add("发货数量");

                for (int i = 0; i < listInfo.Rows.Count; i++)
                {
                    listInfo.Rows[i]["申请数量"] = Convert.ToDecimal(listInfo.Rows[i]["审核数量"]);
                }

                if (!m_serverManeuver.InsertBill(lnqBill, listInfo, out error))
                {
                    return false;
                }

                if (!m_serverManeuver.OperationInfo(lnqBill, listInfo, out error))
                {
                    return false;
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
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteBill(string billNo, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.Out_AfterServicePartsApplyBill
                              where a.Bill_ID == billNo
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据为空或者不唯一";
                    return false;
                }
                else
                {
                    dataContext.Out_AfterServicePartsApplyBill.DeleteAllOnSubmit(varData);
                    dataContext.SubmitChanges();
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
        /// 删除明细数据
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteListInfo(DepotManagementDataContext dataContext,string billNo,out string error)
        {
            error = null;

            try
            {
                var varData = from a in dataContext.Out_AfterServicePartsApplyList
                              where a.Bill_ID == billNo
                              select a;

                dataContext.Out_AfterServicePartsApplyList.DeleteAllOnSubmit(varData);

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 插入明细数据
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="listInfo">明细表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool InsertListInfo(DepotManagementDataContext dataContext, string billNo,DataTable listInfo,out string error)
        {
            error = null;

            try
            {
                for (int i = 0; i < listInfo.Rows.Count; i++)
                {
                    Out_AfterServicePartsApplyList lnqList = new Out_AfterServicePartsApplyList();

                    lnqList.Bill_ID = billNo;
                    lnqList.GoodsID = Convert.ToInt32(listInfo.Rows[i]["物品ID"]);
                    lnqList.ApplyCount = Convert.ToDecimal(listInfo.Rows[i]["申请数量"]);
                    lnqList.AuditingCount = Convert.ToDecimal(listInfo.Rows[i]["审核数量"]);
                    lnqList.Remark = listInfo.Rows[i]["备注"].ToString();
                    lnqList.StorageID = listInfo.Rows[i]["账务库房ID"].ToString();

                    //if (lnqList.AuditingCount == 0)
                    //{
                    //    error = "审核数不能为0";
                    //    return false;
                    //}

                    dataContext.Out_AfterServicePartsApplyList.InsertOnSubmit(lnqList);
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }
}
