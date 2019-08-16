using System;
using System.Collections.Generic;
namespace ServerModule
{
    public interface IOfflineFailSafeServer
    {
        /// <summary>
        /// 获取全部单据信息
        /// </summary>
        /// <returns>返回下线返修扭矩防错信息</returns>
        System.Collections.Generic.List<View_ZPX_OfflineFailsafe> GetAllBill();

         /// <summary>
        /// 添加扭矩防错信息
        /// </summary>
        /// <param name="failSafe">扭矩防错</param>
        /// <param name="error">错误信息</param>
        /// <returns>添加成功返回True失败返回False</returns>
        bool InsertFailsafe(ZPX_OfflineFailsafe failSafe, out string error);

        /// <summary>
        /// 修改扭矩防错信息
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="failSafe">扭矩防错</param>
        /// <param name="error">错误信息</param>
        /// <returns>添加成功返回True失败返回False</returns>
        bool UpdateFailsafe(int id, ZPX_OfflineFailsafe failSafe, out string error);

        /// <summary>
        /// 删除扭矩防错信息
        /// </summary>
        /// <param name="productType">产品型号</param>
        /// <param name="parentName">分总成名称</param>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="phase">阶段</param>
        /// <param name="positionMessage">位置信息</param>
        /// <param name="error">错误信息呢</param>
        /// <returns>删除成功返回true失败返回False</returns>
        bool DeleteFailSafe(string productType, string parentName, string goodsCode,
            string goodsName, string spec, string phase, string positionMessage, out string error);

        /// <summary>
        /// 通过产品型号获取单据信息
        /// </summary>
        /// <param name="productType">产品型号</param>
        /// <returns>返回下线返修扭矩防错信息</returns>
        List<View_ZPX_OfflineFailsafe> GetBillByProductType(string productType);

        /// <summary>
        /// 保存防错顺序
        /// </summary>
        /// <param name="lstData">要保存的列表数据</param>
        void SaveOrderNo(System.Collections.Generic.List<View_ZPX_OfflineFailsafe> lstData);

        /// <summary>
        /// 获取阶段信息
        /// </summary>
        /// <returns>返回阶段信息</returns>
        System.Data.DataTable GetPhase();

        /// <summary>
        /// 获取全部阶段信息
        /// </summary>
        /// <returns>返回阶段信息</returns>
        System.Collections.Generic.List<ZPX_OfflinePhaseSet> GetAllPhase();

        /// <summary>
        /// 添加阶段信息
        /// </summary>
        /// <param name="phaseSet">阶段信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>添加成功返回True失败返回False</returns>
        bool InsertPhase(ZPX_OfflinePhaseSet phaseSet, out string error);

        /// <summary>
        /// 删除阶段信息
        /// </summary>
        /// <param name="phase">阶段</param>
        /// <param name="error">错误信息</param>
        /// <returns>删除成功返回true失败返回false</returns>
        bool DeletePhase(string phase, out string error);

        /// <summary>
        /// 获得最大的防错顺序
        /// </summary>
        /// <param name="type">返修类型</param>
        /// <param name="product">产品型号</param>
        /// <param name="partName">分装总成</param>
        /// <param name="phase">阶段</param>
        /// <returns>返回最大的防错顺序整形值</returns>
        int GetOrderNoMax(string type, string product, string partName, string phase);

        /// <summary>
        /// 复制指定的产品型号信息到目标产品
        /// </summary>
        /// <param name="surProductType">源产品类型</param>
        /// <param name="tarProductType">目标产品类型</param>
        /// <param name="phase">阶段或分总成名称</param>
        /// <param name="error">操作错误时返回的错误信息</param>
        /// <returns>复制成功返回True，复制失败返回False</returns>
        bool CopyFailsafe(string surProductType, string tarProductType, string phase, out string error);
    }
}
