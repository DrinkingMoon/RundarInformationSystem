using System;
using ServerModule;
namespace Service_Economic_Financial
{
    /// <summary>
    /// 最低定价服务组件
    /// </summary>
    public interface ILowestSellPriceServer
    {
        /// <summary>
        /// 通过物品编号删除最低定价
        /// </summary>
        /// <param name="goodsID">物品编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回false</returns>
        bool DeleteData(int goodsID, out string error);

        /// <summary>
        /// 获取所有物品的最低定价
        /// </summary>
        /// <returns>返回数据集</returns>
        System.Data.DataTable GetAllInfo();

        /// <summary>
        /// 添加或修改数据
        /// </summary>
        /// <param name="lowestMarketProce">YX_LowestMarketPrice数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回false</returns>
        bool InsertAndUpdateData(ServerModule.YX_LowestMarketPrice lowestMarketProce, out string error);

        /// <summary>
        /// 通过客户编码和物品ID获得最低定价
        /// </summary>
        /// <param name="clientCode">客户编码</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回YX_LowestMarketPrice数据集，失败返回Null</returns>
        YX_LowestMarketPrice GetDataByClientCode(string clientCode, int goodsID, out string error);

        /// <summary>
        /// 获取主机厂的零件信息
        /// </summary>
        /// <param name="communicateID">编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回数据集</returns>
        YX_GoodsSystemMatchingCommunicate GetCommunicateInfo(string communicateID, out string error);

        /// <summary>
        /// 获取主机厂与容大相匹配的ID
        /// </summary>
        /// <param name="clientCode">主机厂编号</param>
        /// <param name="communicateGoodsCode">主机厂的零件图号</param>
        /// <param name="communicateGoodsName">主机厂的零件名称</param>
        /// <param name="goodsID">容大的物品ID</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回数据集</returns>
        int? GetCommunicateID(string clientCode, string communicateGoodsCode, string communicateGoodsName, int goodsID, out string error);
    }
}
