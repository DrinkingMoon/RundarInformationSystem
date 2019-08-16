using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlobalObject;
using System.Data;

namespace ServerModule
{
    class GaugeManage : IGaugeManage
    {
        /// <summary>
        /// 获得量检具台帐信息
        /// </summary>
        /// <returns>返回量检具台帐信息</returns>
        public DataTable GetGaugeAllInfo(bool lyFlag, bool bfFlag)
        {
            string strSql = "select * from View_S_GaugeStandingBook where 1=1";

            if (!lyFlag)
            {
                strSql += " and 在库 = 1 ";
            }

            if (!bfFlag)
            {
                strSql += " and 报废 = 0 ";
            }

            strSql += " order by 物品ID,入库时间 ";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public S_GaugeStandingBook GetSingleInfo(string code)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                var varData = from a in ctx.S_GaugeStandingBook
                              where a.GaugeCoding == code
                              select a;

                if (varData.Count() == 0)
                {
                    return null;
                }
                else
                {
                    return varData.Single();
                }
            }
        }

        /// <summary>
        /// 根据单据号获得某个物品的量检具编号数据集
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回一条工装台帐的数据集</returns>
        public DataTable GetGaugeCodingFromBillNo(string billNo, int goodsID)
        {
            string strSql = "select GaugeCoding from S_GaugeOperation where BillID = '"
                + billNo + "' and GoodsID = " + goodsID;

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public void OperationGaugeStandingBook(DepotManagementDataContext ctx, string billNo, CE_MarketingType type, int operationType)
        {
            try
            {
                CE_SubsidiaryOperationType subType =
                    (CE_SubsidiaryOperationType)Enum.ToObject(typeof(CE_SubsidiaryOperationType), operationType);

                var varData = from a in ctx.S_GaugeOperation
                              where a.BillID == billNo
                              select a;

                foreach (S_GaugeOperation operationInfo in varData)
                {
                    S_GaugeStandingBook lnqBook = new S_GaugeStandingBook();

                    var varbook = from a in ctx.S_GaugeStandingBook
                                  where a.GaugeCoding == operationInfo.GaugeCoding
                                  select a;

                    if (varbook.Count() > 1)
                    {
                        throw new Exception("【物品ID】:" + operationInfo.GoodsID.ToString() + " 【量检具编号】:" + operationInfo.GaugeCoding
                            + "，在【量检具台账】中存在多个，无法进行业务操作");
                    }

                    if (type == CE_MarketingType.入库)
                    {
                        if (varbook.Count() == 0)
                        {
                            lnqBook = new S_GaugeStandingBook();
                            lnqBook.GaugeCoding = operationInfo.GaugeCoding;
                            lnqBook.GoodsID = operationInfo.GoodsID;
                            lnqBook.InputDate = ServerTime.Time;
                            lnqBook.ScrapFlag = false;
                            lnqBook.Validity = 0;
                        }
                        else
                        {
                            lnqBook = varbook.Single();
                            lnqBook.MaterialDate = null;
                            lnqBook.DutyUser = null;
                            lnqBook.ScrapFlag = false;
                        }
                    }
                    else if(type == CE_MarketingType.出库)
                    {
                        if (varbook.Count() == 0)
                        {
                            throw new Exception("【物品ID】:" + operationInfo.GoodsID.ToString() + " 【量检具编号】:" + operationInfo.GaugeCoding
                            + "，不存在【量检具台账】中，无法进行【出库】业务操作");
                        }

                        lnqBook = varbook.Single();
                        lnqBook.DutyUser = null;
                        lnqBook.ScrapFlag = false;
                        lnqBook.MaterialDate = ServerTime.Time;

                        if (subType == CE_SubsidiaryOperationType.领料)
                        {
                            lnqBook.MaterialDate = ServerTime.Time;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更新量检具信息
        /// </summary>
        /// <param name="gaugeStandingBook">量检具信息</param>
        /// <param name="mode">操作类型</param>
        public void SaveInfo(S_GaugeStandingBook gaugeStandingBook, CE_OperatorMode mode)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                try
                {
                    var varGauge = from a in ctx.S_GaugeStandingBook
                                   where a.GaugeCoding == gaugeStandingBook.GaugeCoding
                                   select a;

                    if (varGauge.Count() == 1)
                    {
                        if (mode == CE_OperatorMode.添加)
                        {
                            throw new Exception("已存在【量检具编号】：" + gaugeStandingBook.GaugeCoding + "， 无法添加");
                        }

                        S_GaugeStandingBook lnqGauge = varGauge.Single();

                        lnqGauge.Manufacturer = gaugeStandingBook.Manufacturer;
                        lnqGauge.Remark = gaugeStandingBook.Remark;
                        lnqGauge.Validity = gaugeStandingBook.Validity;
                        lnqGauge.EffectiveDate = gaugeStandingBook.EffectiveDate;
                        lnqGauge.DutyUser = gaugeStandingBook.DutyUser;
                        lnqGauge.InputDate = gaugeStandingBook.InputDate;
                        lnqGauge.MaterialDate = gaugeStandingBook.MaterialDate;
                        lnqGauge.CheckType = gaugeStandingBook.CheckType;
                        lnqGauge.GaugeType = gaugeStandingBook.GaugeType;
                        lnqGauge.FactoryNo = gaugeStandingBook.FactoryNo;
                    }
                    else if (varGauge.Count() == 0)
                    {
                        if (mode == CE_OperatorMode.修改)
                        {
                            throw new Exception("不存在【量检具编号】：" + gaugeStandingBook.GaugeCoding + "， 无法修改");
                        }

                        gaugeStandingBook.F_Id = Guid.NewGuid().ToString();
                        ctx.S_GaugeStandingBook.InsertOnSubmit(gaugeStandingBook);
                    }
                    else
                    {
                        throw new Exception("【量检具编号】重复");
                    }

                    ctx.SubmitChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void DeleteInfo(string gaugeCoding)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                var varData = from a in ctx.S_GaugeStandingBook
                              where a.GaugeCoding == gaugeCoding
                              select a;

                ctx.S_GaugeStandingBook.DeleteAllOnSubmit(varData);
                ctx.SubmitChanges();
            }
        }

        /// <summary>
        /// 批量插入量检具业务表
        /// </summary>
        /// <param name="billNo">业务单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="gaugeCodingTable">单据业务的量检具编号数据集</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public void UpdateGaugeOperation(string billNo, int goodsID, DataTable gaugeCodingTable)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                try
                {
                    var varData = from a in ctx.S_GaugeOperation
                                  where a.BillID == billNo
                                  && a.GoodsID == goodsID
                                  select a;

                    ctx.S_GaugeOperation.DeleteAllOnSubmit(varData);

                    for (int i = 0; i < gaugeCodingTable.Rows.Count; i++)
                    {
                        S_GaugeOperation lnqOperation = new S_GaugeOperation();

                        lnqOperation.BillID = billNo;
                        lnqOperation.GaugeCoding = gaugeCodingTable.Rows[i]["GaugeCoding"].ToString();
                        lnqOperation.GoodsID = goodsID;

                        ctx.S_GaugeOperation.InsertOnSubmit(lnqOperation);
                    }

                    ctx.SubmitChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void UpLoadFileInfo(Bus_Gauge_Files fileInfo)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                try
                {
                    ctx.Connection.Open();
                    ctx.Transaction = ctx.Connection.BeginTransaction();

                    var varData = from a in ctx.Bus_Gauge_Files
                                  where a.FileName == fileInfo.FileName
                                  && a.GaugeCode == fileInfo.GaugeCode
                                  && a.FileType == fileInfo.FileType
                                  select a;

                    if (varData.Count() > 0)
                    {
                        throw new Exception("【"+ fileInfo.FileName +"】已存在，无法重复上传，如需重新上传，请先删除此【文件记录】");
                    }

                    fileInfo.F_Id = Guid.NewGuid().ToString();
                    fileInfo.F_CreateTime = ServerTime.Time;
                    fileInfo.F_CreateUser = BasicInfo.LoginID;
                    ctx.Bus_Gauge_Files.InsertOnSubmit(fileInfo);
                    ctx.SubmitChanges();

                    StatusChange(ctx, fileInfo.GaugeCode);
                    ctx.SubmitChanges();

                    ctx.Transaction.Commit();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        void StatusChange(DepotManagementDataContext ctx, string code)
        {
            var varBook = from a in ctx.S_GaugeStandingBook
                          where a.GaugeCoding == code
                          select a;

            S_GaugeStandingBook bookInfo = varBook.FirstOrDefault();

            var varFile = from a in ctx.Bus_Gauge_Files
                          where a.GaugeCode == code
                          && a.FileType == "校准证书"
                          select a;

            if (varFile.Count() > 0)
            {
                DateTime tempDate = (DateTime)varFile.Select(k => k.FileDate).Max();
                bookInfo.EffectiveDate = tempDate.AddMonths((int)bookInfo.Validity);

                IBillMessagePromulgatorServer billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();
                billMessageServer.BillType = "量检具台账";
                billMessageServer.EndFlowMessage(code, "已提交了校准证书", null, null);
            }
            else
            {
                bookInfo.EffectiveDate = null;
            }

            var varScrapt = from a in ctx.Bus_Gauge_Files
                            where a.GaugeCode == code
                            && a.FileType == "报废记录"
                            select a;

            if (varScrapt.Count() > 0)
            {
                bookInfo.ScrapFlag = true;
            }
            else
            {
                bookInfo.ScrapFlag = false;
            }
        }

        public void DeleteFileInfo(string keyValue)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                try
                {
                    ctx.Connection.Open();
                    ctx.Transaction = ctx.Connection.BeginTransaction();

                    var varData = from a in ctx.Bus_Gauge_Files
                                  where a.F_Id == keyValue
                                  select a;

                    string code = varData.First().GaugeCode;

                    ctx.Bus_Gauge_Files.DeleteAllOnSubmit(varData);
                    ctx.SubmitChanges();

                    StatusChange(ctx, code);
                    ctx.SubmitChanges();

                    ctx.Transaction.Commit();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public DataTable GetTable_FilesInfo(string gaugeCode)
        {
            string strSql = "select * from View_Bus_Gauge_Files where GaugeCode = '" + gaugeCode + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }
    }
}
