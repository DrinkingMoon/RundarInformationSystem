using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace ServerModule
{
    /// <summary>
    /// 报废单物品信息服务
    /// </summary>
    class ScrapGoodsServer :BasicServer, IScrapGoodsServer
    {
        /// <summary>
        /// 箱体编码服务组件
        /// </summary>
        IProductCodeServer m_serverProductCode = ServerModuleFactory.GetServerModule<IProductCodeServer>();

        /// <summary>
        /// 物品比较器
        /// </summary>
        class GoodsComparer : IEqualityComparer<View_S_ScrapGoods>
        {
            public bool Equals(View_S_ScrapGoods x, View_S_ScrapGoods y)
            {
                if (x.序号 == y.序号)
                    return true;
                else
                    return false;
            }

            public int GetHashCode(View_S_ScrapGoods obj)
            {
                return obj.序号.GetHashCode();
            }
        }

        /// <summary>
        /// 检测是否与之前的数据一致
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="lstGoods">要检查的数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>True 一致 False 不一致</returns>
        public bool IsInfoAccordance(string djh, List<View_S_ScrapGoods> lstGoods, out string error)
        {
            error = null;

            #region 夏石友，2012-07-13 17:00 改写

            try
            {
                if (!ServerModuleFactory.GetServerModule<IMaterialRequisitionGoodsServer>().IsExist(djh))
                {
                    return true;
                }

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                // 获取报废单中物品明细，并用物品ID排序
                List<View_S_ScrapGoods> checkGoods = (from r in ctx.View_S_ScrapGoods
                                                      where r.报废单号 == djh
                                                      orderby r.物品ID
                                                      select r).ToList();

                if (checkGoods.Count != lstGoods.Count)
                {
                    return false;
                }

                // 将要检查的数据集用物品ID排序
                List<View_S_ScrapGoods> surGoods = (from r in lstGoods.AsEnumerable()
                                                    orderby r.物品ID
                                                    select r).ToList();

                // 逐一比较，有不一致则返回
                for (int i = 0; i < surGoods.Count; i++)
                {
                    if (surGoods[i].物品ID != checkGoods[i].物品ID || surGoods[i].报废数量 != checkGoods[i].报废数量)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }

            #endregion

            //try
            //{
            //    string strSql = "select * from S_MaterialRequisition where AssociatedBillNo = '" + djh + "'";

            //    DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            //    if (dt == null || dt.Rows.Count == 0)
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        bool blflag = false;

            //        strSql = "select * from S_ScrapGoods where Bill_ID = '" + djh + "'";

            //        dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            blflag = false;

            //            foreach (var item in goodsdate)
            //            {
            //                if (Convert.ToInt32(dt.Rows[i]["GoodsID"]) == Convert.ToInt32(item.物品ID)
            //                    && Convert.ToDecimal(dt.Rows[i]["Quantity"]) == Convert.ToDecimal(item.报废数量))
            //                {
            //                    blflag = true;
            //                }
            //            }

            //            if (!blflag)
            //            {
            //                return false;
            //            }
            //        }
            //    }

            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    error = ex.Message;
            //    return false;                
            //}
        }

        /// <summary>
        /// 检查是否存在某单据物品清单
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(string billNo)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return (from r in dataContxt.S_ScrapGoods
                    where r.Bill_ID == billNo
                    select r).Count() > 0;
        }

        /// <summary>
        /// 获取指定报废单的物品信息
        /// </summary>
        /// <param name="billNo">报废单号</param>
        /// <returns>返回获取的物品信息</returns>
        public IEnumerable<View_S_ScrapGoods> GetGoods(string billNo)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return (from r in dataContxt.View_S_ScrapGoods
                    where r.报废单号 == billNo
                    select r);//.AsEnumerable().Distinct(new GoodsComparer());
        }

        /// <summary>
        /// 获取指定报废单的物品分组信息
        /// </summary>
        /// <param name="billNo">报废单号</param>
        /// <returns>返回获取的物品信息</returns>
        public IEnumerable<GoodsGroup> GetGoodsByGroup(string billNo)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            IEnumerable<View_S_ScrapGoods> billGoods = from r in dataContxt.View_S_ScrapGoods
                                                       where r.报废单号 == billNo
                                                       select r;

            return (from r in billGoods
                    group r by new { r.物品ID, r.图号型号, r.物品名称, r.规格 } into g
                    select new GoodsGroup { 物品ID = g.Key.物品ID, 图号型号 = g.Key.图号型号, 
                        物品名称 = g.Key.物品名称, 规格 = g.Key.规格, 数量 = g.Sum(p => p.报废数量) });
        }

        /// <summary>
        /// 保存单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="bill">要保存的单据对象</param>
        /// <param name="goodsList">要保存的报废物品清单</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool SaveGoods(string billNo, S_ScrapBill bill, List<View_S_ScrapGoods> goodsList, 
            out string error)
        {
            error = null;
            ScrapBillServer serverScrapBill = new ScrapBillServer();
            try
            {
                DepotManagementDataContext contxt = CommentParameter.DepotDataContext;

                if (IsExist(billNo))
                {
                    #region 夏石友，2012-07-13 10:20

                    S_ScrapBill surBill = serverScrapBill.GetBill(billNo);

                    // 判断当前保存人员是否是建单人
                    if (surBill.FillInPersonnelCode != bill.FillInPersonnelCode)
                    {
                        error = string.Format("您不是建单人 [{0}, {1}] 无法进行此操作", surBill.FillInPersonnelCode, surBill.FillInPersonnel);
                        return false;
                    }

                    #endregion

                    //检查总成报废数量是否都设置了流水码
                    foreach (var item in goodsList)
                    {
                        if (!m_serverProductCode.IsFitCount(item.物品ID, Convert.ToInt32(item.报废数量), bill.Bill_ID))
                        {
                            error = "请对产品设置流水号,并保证产品数量与流水号数一致";
                            return false;
                        }
                    }


                    if (!serverScrapBill.DeleteBill(contxt, billNo, false, out error))
                    {
                        return false;
                    }

                    DeleteGoods(contxt, billNo);
                }

                if (!serverScrapBill.AddBill(contxt,bill, out error))
                {
                    return false;
                }

                AddGoods(contxt, goodsList);

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
        /// 添加物品信息
        /// </summary>
        /// <param name="goods">物品信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        public bool AddGoods(S_ScrapGoods goods, out string error)
        {
            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                error = null;
                dataContxt.S_ScrapGoods.InsertOnSubmit(goods);
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
        /// 批量添加物品
        /// </summary>
        /// <param name="dataContxt">LINQ 数据上下文</param>
        /// <param name="lstGoods">要添加的物品信息列表</param>
        public void AddGoods(DepotManagementDataContext dataContxt, List<View_S_ScrapGoods> lstGoods)
        {
            foreach (var item in lstGoods)
            {
                S_ScrapGoods goods = new S_ScrapGoods();

                goods.Bill_ID = item.报废单号;
                goods.GoodsID = item.物品ID;
                goods.BatchNo = item.批次号;
                goods.CVTNumber = item.CVT编号;
                goods.Provider = item.供应商;
                goods.ResponsibilityProvider = item.责任供应商;
                goods.Quantity = item.报废数量;
                goods.Reason = item.报废原因;
                goods.WastrelType = item.报废类别;
                goods.ResponsibilityDepartment = item.责任部门;
                goods.UnitPrice = item.单价;
                goods.LosingMoney = item.损失金额;
                goods.WorkingHours = item.工时;
                goods.Remark = item.备注;
                goods.ResponsibilityType = item.责任类型;
                goods.Version = item.版次号;
                goods.ClaimType = item.索赔类别;

                dataContxt.S_ScrapGoods.InsertOnSubmit(goods);
            }
        }

        /// <summary>
        /// 批量更新工时
        /// </summary>
        /// <param name="lstGoods">要更新的物品信息列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        public bool UpdateGoods(List<View_S_ScrapGoods> lstGoods, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                foreach (var item in lstGoods)
                {
                    S_ScrapGoods goods = (from r in dataContxt.S_ScrapGoods 
                                          where r.ID == item.序号 
                                          select r).Single();

                    goods.WorkingHours = item.工时;
                    goods.Provider = item.供应商;
                    goods.Reason = item.报废原因;
                    goods.ResponsibilityType = item.责任类型;
                    goods.ResponsibilityProvider = item.责任供应商;
                    goods.ClaimType = item.索赔类别;
                }

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
        /// <returns>返回操作是否成功的标志</returns>
        public bool DeleteGoods(List<long> id, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_ScrapGoods
                             where id.Contains(r.ID)
                             select r;

                if (result.Count() > 0)
                {
                    dataContxt.S_ScrapGoods.DeleteAllOnSubmit(result);
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
        /// 删除某报废单下的所有物品信息
        /// </summary>
        /// <param name="dataContxt">LINQ 数据上下文</param>
        /// <param name="billNo">报废单号</param>
        public void DeleteGoods(DepotManagementDataContext dataContxt, string billNo)
        {
            var result = from r in dataContxt.S_ScrapGoods
                         where r.Bill_ID == billNo
                         select r;

            dataContxt.S_ScrapGoods.DeleteAllOnSubmit(result);
        }

        /// <summary>
        /// 更新物品信息
        /// </summary>
        /// <param name="goods">物品信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        public bool UpdateGoods(S_ScrapGoods goods, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_ScrapGoods
                             where r.ID == goods.ID
                             select r;

                if (result.Count() > 0)
                {
                    S_ScrapGoods updateGoods = result.Single();

                    updateGoods.Bill_ID = goods.Bill_ID;
                    updateGoods.BatchNo = goods.BatchNo;
                    updateGoods.CVTNumber = goods.CVTNumber;
                    updateGoods.GoodsID = goods.GoodsID;
                    updateGoods.Provider = goods.Provider;
                    updateGoods.ResponsibilityProvider = goods.ResponsibilityProvider;
                    updateGoods.Reason = goods.Reason;
                    updateGoods.ResponsibilityDepartment = goods.ResponsibilityDepartment;
                    updateGoods.Quantity = goods.Quantity;
                    updateGoods.UnitPrice = goods.UnitPrice;
                    updateGoods.LosingMoney = goods.LosingMoney;
                    updateGoods.WorkingHours = goods.WorkingHours;
                    updateGoods.Remark = goods.Remark;

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
