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
    /// 下线不合格品放行单服务组件
    /// </summary>
    class ProductReleases : BasicServer, IProductReleases
    {
        /// <summary>
        /// 单据编号分配服务
        /// </summary>
        IAssignBillNoServer m_serverAssginBillNo = BasicServerFactory.GetServerModule<IAssignBillNoServer>();


        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.ZL_ProductReleases
                          where a.BillNo == billNo
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[ZL_ProductReleases] where billNo = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得全部单据
        /// </summary>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="billStatus">单据状态</param>
        /// <param name="flagIsNew">是否显示最新信息 True :是 False :否</param>
        /// <returns>返回获得的全部单据信息</returns>
        public DataTable GetAllBill(DateTime startTime, DateTime endTime, string billStatus, bool flagIsNew)
        {
            string strSql = "select * from View_ZL_ProductReleases as a   where 1=1  ";

            if (flagIsNew)
            {
                strSql += " and 申请时间 = (select MAX(申请时间) from View_ZL_ProductReleases where 产品箱号 = a.产品箱号 and 产品型号 = a.产品型号)";
            }

            strSql += " and 申请时间 >= '" + startTime + "' and 申请时间 <= '" + endTime + "'";

            if (billStatus != "全  部")
            {
                strSql += " and 单据状态 = '" + billStatus + "'";
            }

            strSql += " order by 申请时间 desc";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        public void DeleteBill(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var vardata = from a in ctx.ZL_ProductReleases
                          where a.BillNo == billNo
                          select a;

            ctx.ZL_ProductReleases.DeleteAllOnSubmit(vardata);
            ctx.SubmitChanges();
        }

        /// <summary>
        /// 提交单据
        /// </summary>
        /// <param name="produtctReleases">LINQ实体集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool SubmitBill(ZL_ProductReleases produtctReleases, out string error)
        {
            error = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in ctx.ZL_ProductReleases
                              where a.BillNo == produtctReleases.BillNo
                              select a;

                ZL_ProductReleases lnqProductReleases = new ZL_ProductReleases();

                if (varData.Count() == 0)
                {
                    lnqProductReleases.BillNo = m_serverAssginBillNo.AssignNewNo(this, CE_BillTypeEnum.下线不合格品放行单.ToString());
                    lnqProductReleases.BillStatus = "等待审核";
                    lnqProductReleases.FaultPhenomenon = produtctReleases.FaultPhenomenon;
                    lnqProductReleases.ProductCode = produtctReleases.ProductCode;
                    lnqProductReleases.ProductModel = produtctReleases.ProductModel;
                    lnqProductReleases.Proposer = BasicInfo.LoginName;
                    lnqProductReleases.ProposerDate = ServerTime.Time;
                    lnqProductReleases.Remark = produtctReleases.Remark;

                    ctx.ZL_ProductReleases.InsertOnSubmit(lnqProductReleases);
                }
                else if (varData.Count() == 1)
                {
                    lnqProductReleases = varData.Single();

                    switch (lnqProductReleases.BillStatus)
                    {
                        case "新建单据":
                            lnqProductReleases.BillStatus = "等待审核";
                            lnqProductReleases.FaultPhenomenon = produtctReleases.FaultPhenomenon;
                            lnqProductReleases.ProductCode = produtctReleases.ProductCode;
                            lnqProductReleases.ProductModel = produtctReleases.ProductModel;
                            lnqProductReleases.Proposer = BasicInfo.LoginName;
                            lnqProductReleases.ProposerDate = ServerTime.Time;
                            lnqProductReleases.Remark = produtctReleases.Remark;
                            break;
                        case "等待审核":
                            lnqProductReleases.BillStatus = "等待批准";
                            lnqProductReleases.Auditing = BasicInfo.LoginName;
                            lnqProductReleases.AuditingDate = ServerTime.Time;
                            break;
                        case "等待批准":
                            lnqProductReleases.BillStatus = "已完成";
                            lnqProductReleases.Authorize = BasicInfo.LoginName;
                            lnqProductReleases.AuthorizeDate = ServerTime.Time;
                            break;
                            ;
                        default:
                            break;
                    }
                }

                ctx.SubmitChanges();

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
