/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  GoodsAntirust.cs
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

namespace ServerModule
{
    /// <summary>
    /// 物品防锈管理类
    /// </summary>
    class GoodsAntirust : ServerModule.IGoodsAntirust
    {
        /// <summary>
        /// 单据编号分配服务
        /// </summary>
        IAssignBillNoServer m_assignBill = ServerModuleFactory.GetServerModule<IAssignBillNoServer>();

        /// <summary>
        /// 获得设置防锈期的表
        /// </summary>
        /// <returns>返回防锈期设置表</returns>
        public DataTable GetBaseGoodsAntirustSet()
        {
            string strSql = "select * from View_B_GoodsAntirust";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得最大ID
        /// </summary>
        /// <returns>返回string 最大ID</returns>
        public string GetMaxID()
        {
            string strSql = "select Max(ID) as ID from Base_GoodsAntirust";

            return GlobalObject.DatabaseServer.QueryInfo(strSql).Rows[0]["ID"].ToString();
        }

        /// <summary>
        /// 获得库存物品的防锈信息表
        /// </summary>
        /// <returns>返回库存物品的防锈信息表</returns>
        public DataTable GetStockAntirustCheck()
        {
            string strSql = "select 0 as 选,* from View_S_StockAntirust";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            dt.Columns.Add("物品质量状态");

            //完成第一次防锈且防锈期为12,24的物品防锈期缩短为6
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToInt32(dt.Rows[i]["防锈次数"]) > 0 
                    && (Convert.ToInt32(dt.Rows[i]["物品防锈期"]) == 12 || Convert.ToInt32(dt.Rows[i]["物品防锈期"]) == 24))
                {

                    dt.Rows[i]["物品防锈期"] = "6";
                }

                int intDay = 15;

                if (Convert.ToInt32(dt.Rows[i]["物品防锈期"]) == 12 || Convert.ToInt32(dt.Rows[i]["物品防锈期"]) == 24)
                {
                    intDay = 30;
                }

                #region 根据OA变更单CZ2013060000014进行修改 modify by cjb on 2013.6.18

                if ((Convert.ToInt32(dt.Rows[i]["物品防锈期"]) * 30 - Convert.ToInt32(dt.Rows[i]["防锈天数"]) > 0)
                    && (Convert.ToInt32(dt.Rows[i]["物品防锈期"]) * 30 - Convert.ToInt32(dt.Rows[i]["防锈天数"]) <= intDay))
                {
                    dt.Rows[i]["物品质量状态"] = "预过期";
                }
                else if (Convert.ToInt32(dt.Rows[i]["防锈天数"]) - Convert.ToInt32(dt.Rows[i]["物品防锈期"]) * 30 > 0)
                {
                    dt.Rows[i]["物品质量状态"] = "已过期";
                }
                //if ((Convert.ToInt32(dt.Rows[i]["防锈天数"]) - Convert.ToInt32(dt.Rows[i]["物品防锈期"]) * 30 > 0)
                //        && (Convert.ToInt32(dt.Rows[i]["防锈天数"]) - Convert.ToInt32(dt.Rows[i]["物品防锈期"]) * 30 <= intDay))
                //{
                //    dt.Rows[i]["物品质量状态"] = "预过期";
                //}
                //else if (Convert.ToInt32(dt.Rows[i]["防锈天数"]) - Convert.ToInt32(dt.Rows[i]["物品防锈期"]) * 30 > intDay)
                //{
                //    dt.Rows[i]["物品质量状态"] = "已过期";
                //}
                #endregion
                else if (Convert.ToInt32(dt.Rows[i]["防锈天数"]) - Convert.ToInt32(dt.Rows[i]["物品防锈期"]) * 30 <= 0)
                {
                    dt.Rows[i]["物品质量状态"] = "正常";
                }

            }

            return dt;
        }

        /// <summary>
        /// 添加防锈物品
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="antirustTime">防锈期</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool AddAntirustInfo(int goodsID,decimal antirustTime,out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext contxt = CommentParameter.DepotDataContext;

                var varData = from a in contxt.BASE_GoodsAntirust
                              where a.GoodsID == goodsID
                              select a;

                if (varData.Count() > 0)
                {
                    error = "数据不唯一或者为空";
                    return false;
                }

                BASE_GoodsAntirust lnqAntirust = new BASE_GoodsAntirust();

                lnqAntirust.GoodsID = goodsID;
                lnqAntirust.AntirustTime = antirustTime;

                contxt.BASE_GoodsAntirust.InsertOnSubmit(lnqAntirust);

                contxt.SubmitChanges();

                return true;   
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }       
        }

        /// <summary>
        /// 删除防锈物品
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool DeleteAntirustInfo(int goodsID,out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext contxt = CommentParameter.DepotDataContext;

                var varData = from a in contxt.BASE_GoodsAntirust
                              where a.GoodsID == goodsID
                              select a;

                contxt.BASE_GoodsAntirust.DeleteAllOnSubmit(varData);

                contxt.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 执行防锈
        /// </summary>
        /// <param name="goodsTable">需要执行防锈的物品数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool ExceAntirustInfo(DataTable goodsTable,out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext contxt = CommentParameter.DepotDataContext;

                for (int i = 0; i < goodsTable.Rows.Count; i++)
                {
                    var varStock = from a in contxt.S_Stock
                                  where a.GoodsID == (int)goodsTable.Rows[i]["物品ID"]
                                  && a.BatchNo == goodsTable.Rows[i]["批次号"].ToString()
                                  && a.Provider == goodsTable.Rows[i]["供应商"].ToString()
                                  && a.StorageID == goodsTable.Rows[i]["库房ID"].ToString()
                                  select a;

                    var varAntirust = from a in contxt.KF_GoodsAntirust
                                      where a.GoodsID == (int)goodsTable.Rows[i]["物品ID"]
                                      && a.BatchNo == goodsTable.Rows[i]["批次号"].ToString()
                                      && a.Provider == goodsTable.Rows[i]["供应商"].ToString()
                                      && a.StorageID == goodsTable.Rows[i]["库房ID"].ToString()
                                      select a;

                    if (varStock.Count() != 1)
                    {
                        error = "数据不唯一或者为空";
                        return false;
                    }
                    else
                    {
                        S_Stock lnqStock = varStock.Single();

                        KF_GoodsAntirust lnqAntirust = varAntirust.Single();

                        lnqAntirust.AntirustDate = ServerTime.Time;
                        lnqAntirust.AntirustState = "等待审核";
                        lnqAntirust.ExecutePersonnel = BasicInfo.LoginName;
                        lnqAntirust.ExecuteDate = ServerTime.Time;

                        if (Convert.ToInt32(goodsTable.Rows[i]["不合格数"]) > lnqStock.ExistCount)
                        {
                            error = "【" + lnqStock.GoodsCode.ToString() + "】的批次号[" + lnqAntirust.BatchNo
                                + "]的物品不合格数不能大于库存数";
                            return false;
                        }

                        lnqAntirust.AntirustUnqualifiedCount = Convert.ToInt32(goodsTable.Rows[i]["不合格数"]);

                        if (lnqAntirust.AntirustCount == null)
                        {
                            lnqAntirust.AntirustCount = 1;
                        }
                        else
                        {
                            lnqAntirust.AntirustCount = Convert.ToInt32(lnqAntirust.AntirustCount) + 1;
                        }
                    }
                }

                contxt.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 确认防锈
        /// </summary>
        /// <param name="goodsTable">需要执行的物品数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>True 成功 false  失败</returns>
        public bool AuthorizeAntirustInfo(DataTable goodsTable, out string error)
        {
            error = null;
            MaterialRequisitionServer serverMaterialBill = new MaterialRequisitionServer();
            DepotManagementDataContext contxt = CommentParameter.DepotDataContext;
            contxt.Connection.Open();
            contxt.Transaction = contxt.Connection.BeginTransaction();
            try
            {

                DataTable dtTemp = GlobalObject.DataSetHelper.SelectDistinct("", goodsTable, "库房ID");

                for (int k = 0; k < dtTemp.Rows.Count; k++)
                {
                    DataTable dtgoodsTable = 
                        GlobalObject.DataSetHelper.SiftDataTable(goodsTable, "库房ID = '" + dtTemp.Rows[k][0].ToString() + "'", out error);

                    if (error != null)
                    {
                        throw new Exception(error);
                    }

                    string billID = m_assignBill.AssignNewNo(serverMaterialBill, CE_BillTypeEnum.领料单.ToString());

                    for (int i = 0; i < dtgoodsTable.Rows.Count; i++)
                    {
                        var varStock = from a in contxt.S_Stock
                                       where a.GoodsID == (int)dtgoodsTable.Rows[i]["物品ID"]
                                       && a.BatchNo == dtgoodsTable.Rows[i]["批次号"].ToString()
                                       && a.StorageID == dtgoodsTable.Rows[i]["库房ID"].ToString()
                                       && a.Provider == dtgoodsTable.Rows[i]["供应商"].ToString()
                                       select a;

                        var varAntirust = from a in contxt.KF_GoodsAntirust
                                          where a.GoodsID == (int)dtgoodsTable.Rows[i]["物品ID"]
                                          && a.BatchNo == dtgoodsTable.Rows[i]["批次号"].ToString()
                                          && a.StorageID == dtgoodsTable.Rows[i]["库房ID"].ToString()
                                          && a.Provider == dtgoodsTable.Rows[i]["供应商"].ToString()
                                          select a;

                        if (varStock.Count() != 1)
                        {
                            error = "数据不唯一或者为空";
                            throw new Exception(error);
                        }
                        else
                        {
                            S_Stock lnqStock = varStock.Single();

                            KF_GoodsAntirust lnqAntirust = varAntirust.Single();

                            if (Convert.ToDecimal(lnqAntirust.AntirustUnqualifiedCount) > 0)
                            {
                                if (!CreateMaterialRequisitionGoods(contxt, lnqAntirust, billID, out error))
                                {
                                    throw new Exception(error);
                                }
                            }

                            lnqAntirust.AntirustUnqualifiedCount = 0;
                            lnqAntirust.AuthorizePersonnel = BasicInfo.LoginName;

                            if (lnqAntirust.AntirustState.ToString() == "等待确认")
                            {
                                lnqAntirust.AntirustDate = ServerTime.Time;
                                lnqAntirust.AntirustState = "未防锈";
                            }
                            else
                            {
                                error = "状态不符，请重新确认";
                                throw new Exception(error);
                            }
                        }
                    }

                    contxt.SubmitChanges();

                    var requistionVar = from a in contxt.S_MaterialRequisition
                                        where a.Bill_ID == billID
                                        select a;
                    
                    IMaterialRequisitionServer requisitionService = ServerModule.ServerModuleFactory.GetServerModule<IMaterialRequisitionServer>();

                    requisitionService.OpertaionDetailAndStock(contxt, requistionVar.Single());

                    m_assignBill.UseBillNo(CE_BillTypeEnum.领料单.ToString(), billID);
                }

                contxt.SubmitChanges();
                contxt.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                contxt.Transaction.Rollback();
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 生成领料单明细
        /// </summary>
        /// <param name="dataContxt">LINQ数据上下文</param>
        /// <param name="antirust">防锈物品信息</param>
        /// <param name="billID">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool CreateMaterialRequisitionGoods(DepotManagementDataContext dataContxt, 
            KF_GoodsAntirust antirust, string billID, out string error)
        {
            error = null;

            try
            {
                S_MaterialRequisitionGoods lnqMaterGoods = new S_MaterialRequisitionGoods();

                lnqMaterGoods.BasicCount = 0;
                lnqMaterGoods.BatchNo = antirust.BatchNo;
                lnqMaterGoods.Bill_ID = billID;
                lnqMaterGoods.GoodsID = (int)antirust.GoodsID;
                lnqMaterGoods.ProviderCode = antirust.Provider;
                lnqMaterGoods.RealCount = Convert.ToDecimal(antirust.AntirustUnqualifiedCount);
                lnqMaterGoods.Remark = "由防锈报废自动生成";
                lnqMaterGoods.RequestCount = Convert.ToDecimal(antirust.AntirustUnqualifiedCount);
                lnqMaterGoods.ShowPosition = 1;

                MaterialRequisitionGoodsServer serverMaterialGoods = new MaterialRequisitionGoodsServer();

                if (!serverMaterialGoods.AutoCreateGoods(dataContxt, lnqMaterGoods, out error))
                {
                    return false;
                }
                //dataContxt.S_MaterialRequisitionGoods.InsertOnSubmit(lnqMaterGoods);

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 审核防锈
        /// </summary>
        /// <param name="goodsTable">需要执行的物品数据表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool AuditingAntirustInfo(DataTable goodsTable,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext contxt = CommentParameter.DepotDataContext;

                for (int i = 0; i < goodsTable.Rows.Count; i++)
                {
                    var varData = from a in contxt.S_Stock
                                  where a.GoodsID == (int)goodsTable.Rows[i]["物品ID"]
                                  && a.BatchNo == goodsTable.Rows[i]["批次号"].ToString()
                                  && a.StorageID == goodsTable.Rows[i]["库房ID"].ToString()
                                  && a.Provider == goodsTable.Rows[i]["供应商"].ToString()
                                  select a;

                    var varAntirust = from a in contxt.KF_GoodsAntirust
                                  where a.GoodsID == (int)goodsTable.Rows[i]["物品ID"]
                                  && a.BatchNo == goodsTable.Rows[i]["批次号"].ToString()
                                  && a.StorageID == goodsTable.Rows[i]["库房ID"].ToString()
                                  && a.Provider == goodsTable.Rows[i]["供应商"].ToString()
                                  select a;

                    if (varData.Count() != 1)
                    {
                        error = "数据不唯一或者为空";
                        return false;
                    }
                    else
                    {
                        S_Stock lnqStock = varData.Single();

                        KF_GoodsAntirust lnqAntirust = varAntirust.Single();

                        lnqAntirust.AuditingDate = ServerTime.Time;
                        lnqAntirust.AuditingPersonnel = BasicInfo.LoginName;

                        if (Convert.ToInt32(goodsTable.Rows[i]["不合格数"]) > lnqStock.ExistCount)
                        {
                            error = "【" + lnqStock.GoodsCode.ToString() + "】的批次号[" + lnqStock.BatchNo
                                + "]的物品不合格数不能大于库存数";
                            return false;
                        }

                        lnqAntirust.AntirustUnqualifiedCount = Convert.ToInt32(goodsTable.Rows[i]["不合格数"]);

                        if (lnqAntirust.AntirustState.ToString() == "等待审核"
                            && Convert.ToDecimal(lnqAntirust.AntirustUnqualifiedCount) > 0)
                        {
                            lnqAntirust.AntirustState = "等待确认";
                        }
                        else if (lnqAntirust.AntirustState.ToString() == "等待审核"
                            && Convert.ToDecimal(lnqAntirust.AntirustUnqualifiedCount) == 0)
                        {
                            lnqAntirust.AntirustState = "未防锈";

                        }
                        else
                        {
                            error = "状态不符，请重新审核";
                            return false;
                        }
                    }
                }

                contxt.SubmitChanges();

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
