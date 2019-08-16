/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  Quarantine.cs
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
using PlatformManagement;

namespace ServerModule
{
    /// <summary>
    /// 产品隔离单管理类
    /// </summary>
    class Quarantine:BasicServer, ServerModule.IQuarantine
    {
        /// <summary>
        /// 过滤查询
        /// </summary>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="djzt">单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回表信息</returns>
        public DataTable GetAllBill(string startTime, string endTime, string djzt, out string error)
        {
            error = "";

            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = authorization.Query("不合格隔离查询", null);
            DataRow[] dr;

            if (djzt == "全  部")
            {
                dr = qr.DataCollection.Tables[0].Select("编制日期 >= '" + startTime + " 00:00:00' and 编制日期 <='" + endTime + " 00:00:00' ");
            }
            else
            {
                dr = qr.DataCollection.Tables[0].Select("编制日期 >= '" + startTime + " 00:00:00' and 编制日期 <='" + endTime + " 00:00:00' and 单据状态 = '" + djzt + "' ");
            }

            DataTable dt = qr.DataCollection.Tables[0].Clone();

            for (int i = 0; i < dr.Length; i++)
            {
                dt.ImportRow(dr[i]);
            }

            return dt;
        }

        #region 获取单号

        /// <summary>
        /// 获得单号
        /// </summary>
        /// <returns>返回获取的单号</returns>
        public string GetBillID()
        {
            string strNewDJH = "";
           
            try
            {

                string strDJH = "YXGL" + ServerTime.Time.Year.ToString();
                string strSql = "select max(substring(Bill_ID,11,5)) from S_QuarantineBill where Bill_ID like '" + strDJH + "%'";

                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (dt.Rows[0][0].ToString() != "")
                {
                    string strValue = (Convert.ToInt32(dt.Rows[0][0].ToString()) + 1).ToString("D5");
                    strNewDJH = strDJH + ServerTime.GetMouth() + strValue;
                }
                else
                {
                    strNewDJH = strDJH + ServerTime.GetMouth() + "00001";
                }

                return strNewDJH;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion 

        /// <summary>
        /// 根据单据号删除(改变删除状态)
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功</returns>
        public bool DeleteBill(string djh, out string error)
        {
            error = "";

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var delete = from a in dataContxt.S_QuarantineBill
                         where a.Bill_ID == djh
                         select a;

            if (delete.Count() < 1)
            {
                error = "没有此单据！";
                return false;
            }
            else
            {
                S_QuarantineBill quarantinebill = delete.Single();

                quarantinebill.DeleteFlag = true;
                dataContxt.SubmitChanges();

                return true;
            }
        }

        /// <summary>
        /// 添加主表信息
        /// </summary>
        /// <param name="quarantine">S_QuarantineBill对象</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AddBill(S_QuarantineBill quarantine,out string error)
        {
             error = "";

             try
             {
                 DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                 var dataMain = from a in dataContxt.S_QuarantineBill
                                where a.Bill_ID == quarantine.Bill_ID
                                select a;

                 if (dataMain.Count() > 0)
                 {
                     S_QuarantineBill q_bill = dataMain.Single();

                     if (q_bill.Status == "已保存" && q_bill.Status != "已解封")
                     {
                         q_bill.Storage = quarantine.Storage;
                         q_bill.Remark = quarantine.Remark;
                         q_bill.CheckMan = quarantine.CheckMan;
                         q_bill.CheckTime = quarantine.CheckTime;
                         q_bill.Description = quarantine.Description;
                         q_bill.Department = quarantine.Department;
                         q_bill.DeleteFlag = false;

                         dataContxt.SubmitChanges();
                        
                         return true;
                     }
                     else
                     {
                         error = "单据已经解封，不能进行此操作！";
                         return false;
                     }
                 }
                 else
                 {
                     S_QuarantineBill lnqQuarantineAdd = new S_QuarantineBill();

                     lnqQuarantineAdd.Bill_ID = quarantine.Bill_ID;
                     lnqQuarantineAdd.Storage = quarantine.Storage;
                     lnqQuarantineAdd.Remark = quarantine.Remark;
                     lnqQuarantineAdd.LRRY = quarantine.LRRY;
                     lnqQuarantineAdd.LRRQ = quarantine.LRRQ;
                     lnqQuarantineAdd.Description = quarantine.Description;
                     lnqQuarantineAdd.Department = quarantine.Department;
                     lnqQuarantineAdd.DeleteFlag = false;
                     lnqQuarantineAdd.Status = "已保存";
                     lnqQuarantineAdd.IsHandle = quarantine.IsHandle;

                     dataContxt.S_QuarantineBill.InsertOnSubmit(lnqQuarantineAdd);
                     dataContxt.SubmitChanges();
                     return true;
                 }
             }
             catch(Exception ex)
             {
                 error = ex.Message;
                 return false;
             }
        }

        /// <summary>
        /// 修改ProductStock表的状态
        /// </summary>
        /// <param name="stockCode">箱体编号</param>
        /// <param name="goodID">物品编号</param>
        /// <param name="flag">是否为正常状态 True 是 False 不是</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True</returns>
        public bool UpdateProductStockStatus(string stockCode, string goodID, bool flag, out string error)
        {
            error = "";

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var updateProduct = from b in dataContxt.ProductStock
                                where b.ProductCode == stockCode 
                                && b.GoodsID == int.Parse(goodID)
                                select b;

            if (updateProduct.Count() != 1)
            {
                error = "数据错误！";
                return false;
            }
            else
            {
                ProductStock stock = updateProduct.Single();

                stock.IsNatural = flag;

                dataContxt.SubmitChanges();

                return true;
            }
        }

        /// <summary>
        /// 仓管审核，修改主表状态
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="handle">是否处理标志 字符串</param>
        /// <param name="status">单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AuditingBill(string billID,string handle,string status,out string error)
        {
            error="";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var verify = from a in dataContxt.S_QuarantineBill
                             where a.Bill_ID == billID
                             select a;

                if (verify.Count() != 1)
                {
                    error = "数据信息有误";
                    return false;
                }
                else
                {
                    S_QuarantineBill dataMain = verify.Single();

                    dataMain.Status = status;
                    dataMain.IsHandle = handle;
                    dataMain.CheckMan = BasicInfo.LoginName;
                    dataMain.CheckTime = ServerTime.Time;
                   
                    dataContxt.SubmitChanges();

                    return true;
                }
            }
            catch(Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 根据单据号查询明细
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回表数据</returns>
        public DataTable GetList(string billID,out string error)
        {
            error = "";

            string sql = @"select a.bill_id as Bill_ID,a.storage as storage,b.物品名称 as GoodsName, "+
                " ProductStockCode as ProductCode,b.图号型号 as GoodsCode,b.规格 as Spec,a.Batchcode  "+
                " as BatchNo,Price as UnitPrice,b.物品类别 as Depot,a.Remark as Remark,goodid as "+
                " GoodsID,IsHandle as IsHandle from S_Quarantinelist as a "+
                " left join View_F_GoodsPlanCost as b on a.goodid = b.序号 where a.bill_id='" + billID + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt == null)
            {
                error = "没有明细信息！";
                return dt;
            }

            return dt;
        }

        /// <summary>
        /// 修改明细
        /// </summary>
        /// <param name="quarantineList">产品明细对象</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true</returns>
        public bool UpdateList(S_QuarantineList quarantineList, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var update = from a in dataContxt.S_QuarantineList
                             where a.Bill_ID == quarantineList.Bill_ID && a.ProductStockCode == quarantineList.ProductStockCode 
                             && a.GoodID == quarantineList.GoodID
                             select a;

                if (update.Count() > 0)
                {
                    S_QuarantineList quarantine = update.Single();

                    quarantine.IsHandle = true;

                    dataContxt.SubmitChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 通过单据号查到处理状态
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="code">箱体编号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回数据集</returns>
        public DataTable GetOperationStatus(string billID,string code, out string error)
        {
            error="";

            try
            {
                string sql = "select ishandle from dbo.S_QuarantineList where bill_id = '"+billID+"'";

                if (code != null)
                {
                    sql += " and productstockcode = '" + code + "'";
                }

                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

                return dt;
            }
            catch(Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 删除单据明细
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回TRUE</returns>
        private bool DeleteSellInList(string billID, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var varData = from a in dataContxt.S_QuarantineList
                              where a.Bill_ID == billID
                              select a;

                foreach (var item in varData)
                {
                    //还原原有数据的状态
                    if (!UpdateProductStockStatus(item.ProductStockCode,item.GoodID.ToString(),true, out error))
                    {
                        return false;
                    }
                }

                if (varData.Count() != 0)
                {
                    dataContxt.S_QuarantineList.DeleteAllOnSubmit(varData);
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
        /// 单据明细的数据库操作
        /// </summary>
        /// <param name="listInfo">需要操作的数据集</param>
        /// <param name="billID">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true</returns>
        public bool AddList(DataTable listInfo, string billID, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            if (DeleteSellInList(billID, out error))
            {
                try
                {
                    for (int i = 0; i < listInfo.Rows.Count; i++)
                    {
                        S_QuarantineList lnqList = new S_QuarantineList();

                        lnqList.Bill_ID = billID;
                        lnqList.GoodID = int.Parse(listInfo.Rows[i]["GoodsID"].ToString());
                        lnqList.ProductStockCode = listInfo.Rows[i]["ProductCode"].ToString();
                        lnqList.BatchCode = listInfo.Rows[i]["BatchNo"].ToString();
                        lnqList.Price = decimal.Parse(listInfo.Rows[i]["UnitPrice"].ToString());
                        lnqList.Remark = listInfo.Rows[i]["Remark"].ToString();
                        lnqList.Storage = listInfo.Rows[i]["Storage"].ToString();
                        lnqList.IsHandle = Convert.ToBoolean("False");
                        
                        dataContxt.S_QuarantineList.InsertOnSubmit(lnqList);
                        //现在TABLE的数据的状态
                        UpdateProductStockStatus(listInfo.Rows[i]["ProductCode"].ToString(), 
                            listInfo.Rows[i]["GoodsID"].ToString(),false, out error);
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
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 查询入库商品信息
        /// </summary>
        /// <param name="goodsID">物品编号</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回满足条件的数据集</returns>
        public DataTable GetProductStockInfo(string goodsID,string storageID, out string error)
        {
            error = "";

            string sql = " select b.图号型号,ProductCode 箱体编号,b.物品名称,ProductStatus 总成状态,GoodsID " +
                         " as GoodsID from ProductStock as a inner join View_F_GoodsPlanCost as b " +
                         " on a.GoodsID = b.序号 where (ProductStatus in ('入库','退库','领料退库','调入')) " +
                         " and isnatural='1' and storageID = '" + storageID + "'";
                
            if (goodsID != null)
            {
                sql += " and a.GoodsID='" + goodsID + "'";
            }

            sql += " order by 箱体编号";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 查找产品编号查找信息
        /// </summary>
        /// <param name="goodsID">产品编号</param>
        /// <returns>返回满足条件的数据集</returns>
        public DataTable GetProductCodeInfo(string goodsID)
        {
            string sql = "select * from F_GoodsPlanCost where id='"+goodsID+"' and IsDisable = 0";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 查找已经隔离了的产品
        /// </summary>
        /// <returns>返回满足条件的数据集</returns>
        public DataTable GetInsulateGoodsInfo()
        {
            string sql = " select ProductCode as 箱体编号, b.图号型号,b.物品名称,"+
                         " ProductStatus as 总成状态,StorageName as 库房名称,GoodsID" +
                         " from ProductStock as a inner join View_F_GoodsPlanCost as b on a.GoodsID = b.序号"+
                         " inner join BASE_Storage as c on a.StorageID = c.StorageID  where isnatural='0' "+
                         " and ProductStatus in ('入库','退库','领料退库','调入')";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 通过箱体编号查看单据号
        /// </summary>
        /// <param name="stockCode">箱体编号</param>
        /// <returns>返回单据号</returns>
        public DataRow GetBillID(string stockCode)
        {
            string sql = @"select Bill_ID from S_QuarantineList where ProductStockCode='" + stockCode + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt.Rows[0];
        }

        /// <summary>
        /// 质管处理，修改表信息
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="disposeName">处理人</param>
        /// <param name="dispose">处理方案</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True</returns>
        public bool HandleBill(string djh,string disposeName,string dispose,out string error)
        {
            error = "";

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var vardata = from a in dataContxt.S_QuarantineBill
                          where a.Bill_ID == djh
                          select a;

            if (vardata.Count() != 1)
            {
                error = "数据不正确！";
                return false;
            }
            else
            {
                S_QuarantineBill list = vardata.Single();

                list.DisposeName = disposeName;
                list.DisposeTime = ServerTime.Time;
                list.DisposeSol = dispose;
                dataContxt.SubmitChanges();

                return true;
            }
        }
    }
}
