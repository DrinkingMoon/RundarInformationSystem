/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  GeneratesCheckOutInDepotServer.cs
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
    /// 自动生成入库单管理类
    /// </summary>
    class GeneratesCheckOutInDepotServer:BasicServer, ServerModule.IGeneratesCheckOutInDepotServer
    {
        /// <summary>
        /// 获得可以自动生成的入库单信息
        /// </summary>
        /// <param name="provider">供应商</param>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="gpec">规格</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回可以自动生成的入库单信息</returns>
        public DataTable GetAllInfo(string provider,
            string goodsCode, string goodsName, string gpec,out string error)
        {
            error = null;

            try
            {
                Hashtable paramTable = new Hashtable();

                paramTable.Add("@Provider", provider);
                paramTable.Add("@GoodsName", goodsName);
                paramTable.Add("@GoodsCode", goodsCode);
                paramTable.Add("@Spec", gpec);


                DataSet ds = new DataSet();
                Dictionary<OperateCMD, object> dicOperateCMD = 
                    m_dbOperate.RunProc_CMD("GetGeneratesBill", ds, paramTable);

                if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
                {
                    error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                    return null;
                }

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 自动插入报检入库单
        /// </summary>
        /// <param name="dateTable">需要插入的数据集</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="version">版次号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool AddCheckInDepotBill(DataTable dateTable, string storageID, string version, out string error)
        {
            error = null;

            BargainInfoServer serverBargainInfo = new BargainInfoServer();

            CheckOutInDepotServer serverCheckInDepot = new CheckOutInDepotServer();

            try
            {
                string strBatchNo = serverCheckInDepot.GetNextBillNo(1);

                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                for (int i = 0; i < dateTable.Rows.Count; i++)
                {
                    S_CheckOutInDepotBill bill = new S_CheckOutInDepotBill();

                    decimal dcUnitPrice = serverBargainInfo.GetBargainUnitPrice(dateTable.Rows[i]["订单号"].ToString(),
                        Convert.ToInt32(dateTable.Rows[i]["物品ID"]));

                    F_GoodsPlanCost lnqGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>().GetGoodsInfo(
                        Convert.ToInt32(dateTable.Rows[i]["物品ID"]));

                    bill.Bill_ID = GetBillID(strBatchNo, i);
                    bill.OrderFormNumber = dateTable.Rows[i]["订单号"].ToString();
                    bill.ArriveGoods_Time = ServerTime.Time;      // 到货日期
                    bill.Bill_Time = ServerTime.Time;                       // 报检日期
                    bill.BillStatus = CheckInDepotBillStatus.新建单据.ToString();
                    bill.Buyer = dateTable.Rows[i]["订货人"].ToString();                       // 采购员签名
                    bill.DeclarePersonnelCode = dateTable.Rows[i]["订货人编号"].ToString();          // 报检员编码
                    bill.DeclarePersonnel = dateTable.Rows[i]["订货人"].ToString();            // 报检员签名
                    bill.DeclareCount = Convert.ToInt32(dateTable.Rows[i]["到货数量"]);              // 报检数
                    bill.Provider = dateTable.Rows[i]["供应商"].ToString();                       // 供应商编码
                    bill.ProviderBatchNo = "";       // 供应商批次
                    bill.GoodsID = (int)dateTable.Rows[i]["物品ID"];
                    bill.BatchNo = strBatchNo + "Auto";                         // xsy, 没有废除OA前暂用
                    bill.Remark = "由自动订单自动生成的报检入库单";
                    bill.CheckOutGoodsType = 1;
                    bill.OnlyForRepairFlag = false;
                    bill.UnitPrice = dcUnitPrice;
                    bill.Price = decimal.Round(bill.UnitPrice * Convert.ToDecimal(dateTable.Rows[i]["到货数量"]), 2);
                    bill.PlanUnitPrice = Convert.ToDecimal(lnqGoods.GoodsUnitPrice);
                    bill.PlanPrice = decimal.Round(bill.PlanUnitPrice * Convert.ToDecimal(dateTable.Rows[i]["到货数量"]), 2);
                    bill.TotalPrice = CalculateClass.GetTotalPrice(bill.Price);
                    bill.StorageID = storageID;
                    bill.Version = version;
                    bill.IsExigenceCheck = false;
                    bill.UnitInvoicePrice = 0;
                    bill.InvoicePrice = 0;

                    if (UniversalFunction.GetStorageInfo_View(bill.StorageID).零成本控制)
                    {
                        throw new Exception("【零成本】库房，无法通过【报检入库单】入库");
                    }

                    dataContext.S_CheckOutInDepotBill.InsertOnSubmit(bill);
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
        /// 获得单据号
        /// </summary>
        /// <param name="djh">当前单据号</param>
        /// <param name="i">需要累加的值</param>
        /// <returns>返回新单据号</returns>
        private string GetBillID(string djh,int i)
        {
            string strNewDJH = djh;
            strNewDJH = djh.Substring(0,9) + 
                (Convert.ToInt32(djh.Substring(9, 6)) + i).ToString("D6");

            return strNewDJH;
        }
    }
}
