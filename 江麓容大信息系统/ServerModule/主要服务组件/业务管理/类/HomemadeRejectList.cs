/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  HomemadeRejectList.cs
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

namespace ServerModule
{
    /// <summary>
    /// 自制件退货明细服务类
    /// </summary>
    class HomemadeRejectList : BasicServer, ServerModule.IHomemadeRejectList
    {
        /// <summary>
        /// 基础物品信息服务
        /// </summary>
        IBasicGoodsServer m_basicGoodsServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 库存服务
        /// </summary>
        IStoreServer m_storeServer = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 检查是否存在某单据物品清单
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(string billNo)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return (from r in dataContxt.S_HomemadeRejectList
                    where r.Bill_ID == billNo
                    select r).Count() > 0;
        }

        /// <summary>
        /// 获取自制件退货单视图信息
        /// </summary>
        /// <param name="billNo">退货单号</param>
        /// <returns>成功返回获取领料出库信息, 失败返回null</returns>
        public DataTable GetBillView(string billNo)
        {
            string sql = "SELECT * FROM [DepotManagement].[dbo].[View_S_HomemadeRejectList] where 退货单号 = '" + billNo + "'";

            return GlobalObject.DatabaseServer.QueryInfo(sql);
        }

        /// <summary>
        /// 设置金额信息
        /// </summary>
        /// <param name="goods">物品信息</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        private bool SetPriceInfo(S_HomemadeRejectList goods, string storageID, out string error)
        {
            decimal factUnitPrice = 0;
            decimal planUnitPrice = 0;

            if (!m_basicGoodsServer.GetPlanUnitPrice(goods.GoodsID, out planUnitPrice, out error))
            {
                return false;
            }

            goods.PlanUnitPrice = planUnitPrice;
            goods.PlanPrice = planUnitPrice * goods.Amount;
            goods.UnitPrice = factUnitPrice;
            goods.Price = factUnitPrice * goods.Amount;
            goods.TotalPrice = CalculateClass.GetTotalPrice(goods.Price);

            return true;
        }

        /// <summary>
        /// 添加物品信息
        /// </summary>
        /// <param name="goods">物品信息</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool AddGoods(S_HomemadeRejectList goods, string storageID, out string error)
        {
            try
            {
                error = null;

                IsolationManageBill serverIsolation = new IsolationManageBill();

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                if (!SetPriceInfo(goods, storageID, out error))
                {
                    return false;
                }

                if (!serverIsolation.UpdateAssicotaeBillID(dataContxt, goods.Bill_ID, goods.GoodsID, goods.BatchNo, out error))
                {
                    return false;
                }

                dataContxt.S_HomemadeRejectList.InsertOnSubmit(goods);
                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除物品信息
        /// </summary>
        /// <param name="id">要删除的物品ID列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool DeleteGoods(List<long> id, out string error)
        {
            try
            {
                error = null;

                IsolationManageBill serverIsolation = new IsolationManageBill();

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_HomemadeRejectList
                             where id.Contains(r.ID)
                             select r;

                if (result.Count() > 0)
                {
                    foreach (var item in result)
                    {
                        if (!serverIsolation.ClearGoodsDate(dataContxt, item.Bill_ID, item.GoodsID, item.BatchNo, out error))
                        {
                            return false;
                        }
                    }

                    dataContxt.S_HomemadeRejectList.DeleteAllOnSubmit(result);
                    dataContxt.SubmitChanges();
                }

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除某自制件退货单下的所有物品信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="billNo">退货单号</param>
        public void DeleteGoods(DepotManagementDataContext context, string billNo)
        {
            var result = from r in context.S_HomemadeRejectList 
                         where r.Bill_ID == billNo 
                         select r;

            context.S_HomemadeRejectList.DeleteAllOnSubmit(result);
        }

        /// <summary>
        /// 删除某自制件退货单下的所有物品信息
        /// </summary>
        /// <param name="billNo">退货单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool DeleteGoods(string billNo, out string error)
        {
            try
            {
                error = null;

                IsolationManageBill serverIsolation = new IsolationManageBill();

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_HomemadeRejectList 
                             where r.Bill_ID == billNo 
                             select r;

                foreach (var item in result)
                {
                    if (!serverIsolation.ClearGoodsDate(dataContxt, item.Bill_ID, item.GoodsID, item.BatchNo, out error))
                    {
                        return false;
                    }
                }

                dataContxt.S_HomemadeRejectList.DeleteAllOnSubmit(result);
                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 更新物品信息
        /// </summary>
        /// <param name="goods">物品信息</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool UpdateGoods(S_HomemadeRejectList goods, string storageID, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_HomemadeRejectList
                             where r.ID == goods.ID
                             select r;

                if (result.Count() > 0)
                {
                    S_HomemadeRejectList updateGoods = result.Single();

                    updateGoods.Bill_ID = goods.Bill_ID;
                    updateGoods.GoodsID = goods.GoodsID;
                    updateGoods.Provider = goods.Provider;
                    updateGoods.BatchNo = goods.BatchNo;
                    updateGoods.ProviderBatchNo = goods.ProviderBatchNo;
                    updateGoods.Amount = goods.Amount;
                    updateGoods.Remark = goods.Remark;

                    if (!SetPriceInfo(updateGoods, storageID, out error))
                    {
                        return false;
                    }

                    dataContxt.SubmitChanges();
                }

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }
    }
}
