using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServerModule;

namespace Service_Economic_Financial
{
    class LowestSellPriceServer : ILowestSellPriceServer
    {
        /// <summary>
        /// 获取所有物品的最低定价
        /// </summary>
        /// <returns>返回数据集</returns>
        public DataTable GetAllInfo()
        {
            string sql = "select * from View_YX_LowestMarketPrice";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 获取主机厂与容大相匹配的ID
        /// </summary>
        /// <param name="clientCode">主机厂编号</param>
        /// <param name="communicateGoodsCode">主机厂的零件图号</param>
        /// <param name="communicateGoodsName">主机厂的零件名称</param>
        /// <param name="goodsID">容大的物品ID</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回数据集</returns>
        public int? GetCommunicateID(string clientCode, string communicateGoodsCode, string communicateGoodsName, int goodsID,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.YX_GoodsSystemMatchingCommunicate
                             where a.GoodsID == goodsID && a.Communicate == clientCode && a.CommunicateGoodsCode == communicateGoodsCode
                             && a.CommunicateGoodsName == communicateGoodsName
                             select a;

                if (result.Count() > 0)
                {
                    return result.Single().ID;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 添加或修改数据
        /// </summary>
        /// <param name="lowestMarketProce">YX_LowestMarketPrice数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回false</returns>
        public bool InsertAndUpdateData(YX_LowestMarketPrice lowestMarketProce,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.YX_LowestMarketPrice
                             where a.GoodsID == lowestMarketProce.GoodsID && a.ClientID == lowestMarketProce.ClientID
                             select a;

                if (result.Count() > 0)
                {
                    YX_LowestMarketPrice yx = result.Single();

                    yx.Date = lowestMarketProce.Date;
                    yx.Price = lowestMarketProce.Price;
                    yx.ClientID = lowestMarketProce.ClientID;
                    yx.Rater = lowestMarketProce.Rater;
                    yx.CommunicateID = lowestMarketProce.CommunicateID;
                    yx.Remark = lowestMarketProce.Remark;
                    yx.TerminalPrice = lowestMarketProce.TerminalPrice;
                }
                else
                {
                    dataContxt.YX_LowestMarketPrice.InsertOnSubmit(lowestMarketProce);
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
        /// 通过物品编号删除最低定价
        /// </summary>
        /// <param name="goodsID">物品编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回false</returns>
        public bool DeleteData(int goodsID,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.YX_LowestMarketPrice
                             where a.GoodsID == goodsID
                             select a;

                if (result.Count() > 0)
                {
                    dataContxt.YX_LowestMarketPrice.DeleteAllOnSubmit(result);
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
        /// 通过客户编码和物品ID获得最低定价
        /// </summary>
        /// <param name="clientCode">客户编码</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回YX_LowestMarketPrice数据集，失败返回Null</returns>
        public YX_LowestMarketPrice GetDataByClientCode(string clientCode,int goodsID, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.YX_LowestMarketPrice
                             where a.ClientID == clientCode && a.GoodsID == goodsID
                             select a;

                if (result.Count() > 0)
                {
                    return result.Single();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 获取主机厂的零件信息
        /// </summary>
        /// <param name="communicateID">编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回数据集</returns>
        public YX_GoodsSystemMatchingCommunicate GetCommunicateInfo(string communicateID,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.YX_GoodsSystemMatchingCommunicate
                             where a.ID == Convert.ToInt32(communicateID)
                             select a;

                if (result.Count() > 0)
                {
                    return result.Single();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }
    }
}
