using System;
using System.Data;
namespace ServerModule
{
    public interface IPreventErrorServer
    {
        #region 打扭矩
        /// <summary>
        /// 获得所有零件的打扭矩防错信息
        /// </summary>
        /// <returns>获取成功返回dt，获取失败返回null</returns>
        DataTable GetAllTorqueSpannerInfo();

        /// <summary>
        /// 修改打扭矩零件的屏蔽时间
        /// </summary>
        /// <param name="workBench">工位</param>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="startTime">屏蔽开始时间</param>
        /// <param name="endTime">屏蔽终止时间</param>
        /// <param name="error">错误信息</param>
        /// <returns>修改成功返回true，失败返回False</returns>
        bool UpdateShieldTime(string workBench, string goodsCode, string goodsName,
                                    string spec, DateTime startTime, DateTime endTime, out string error);

        /// <summary>
        /// 添加零件打扭矩防错的信息
        /// </summary>
        /// <param name="torqueSpanner">打扭矩防错数据对象</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        bool InsertTorqueSpanner(ZPX_TorqueSpanner torqueSpanner, out string error);

        /// <summary>
        /// 修改零件打扭矩防错的信息
        /// </summary>
        /// <param name="torqueSpanner">打扭矩防错数据对象</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        bool UpdateTorqueSpanner(ZPX_TorqueSpanner torqueSpanner, out string error);

        /// <summary>
        /// 删除零件打扭矩防错的信息
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        bool DeleteTorqueSpanner(int id, out string error);

        /// <summary>
        /// 判断零件是否在打扭矩的工位上
        /// </summary>
        /// <param name="workBench">工位</param>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <returns>在返回true不在返回false</returns>
        bool IsWorkBenchGoods(string workBench, string goodsCode, string goodsName, string spec);
        #endregion

        #region 电子称

        /// <summary>
        /// 获得所有零件的电子称防错信息
        /// </summary>
        /// <returns>获取成功返回dt，获取失败返回null</returns>
        DataTable GetAllLeakproofPartsInfo();

        /// <summary>
        /// 修改电子称零件的屏蔽时间
        /// </summary>
        /// <param name="workBench">工位</param>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="starTime">屏蔽开始时间</param>
        /// <param name="endTime">屏蔽终止时间</param>
        /// <param name="error">错误信息</param>
        /// <returns>修改成功返回true，失败返回False</returns>
        bool UpdateLeakproofTime(string workBench, string goodsCode, string goodsName,
                                    string spec, DateTime starTime, DateTime endTime, out string error);
        /// <summary>
        /// 添加零件电子称防错的信息
        /// </summary>
        /// <param name="leakproofParts">电子称防错数据对象</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        bool InsertLeakproofParts(ZPX_LeakproofParts leakproofParts, out string error);

        /// <summary>
        /// 修改零件电子称防错的信息
        /// </summary>
        /// <param name="leakproofParts">电子称防错数据对象</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        bool UpdateLeakproofParts(ZPX_LeakproofParts leakproofParts, out string error);

        /// <summary>
        /// 删除零件电子称防错的信息
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        bool DeleteLeakproofParts(int id, out string error);
        #endregion

        #region 装配顺序及工位间装配顺序

        /// <summary>
        /// 获得所有零件的装配顺序防错信息
        /// </summary>
        /// <returns>获取成功返回dt，获取失败返回null</returns>
        DataTable GetAllWorkbenchConfig();

        /// <summary>
        /// 修改装配顺序及工位间装配顺序的屏蔽时间
        /// </summary>
        /// <param name="workBench">当前工位</param>
        /// <param name="upWorkBench">上道工位</param>
        /// <param name="productCode">产品类型</param>
        /// <param name="name">分总成名称</param>
        /// <param name="starTime">屏蔽开始时间</param>
        /// <param name="endTime">屏蔽终止时间</param>
        /// <param name="error">错误信息</param>
        /// <returns>修改成功返回true，失败返回False</returns>
        bool UpdateProductWorkbenchTime(string workBench, string upWorkBench, string productCode,
                                     string name, DateTime starTime, DateTime endTime, out string error);
        /// <summary>
        /// 获得分工位总成
        /// </summary>
        /// <param name="queryAssemblingBom">操作后查询返回的产品信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>获取成功返回True，获取失败返回False</returns>
        bool GetAssemblingBom(
            out System.Linq.IQueryable<P_AssemblingBom> queryAssemblingBom, out string error);

        /// <summary>
        /// 添加零件装配顺序防错的信息
        /// </summary>
        /// <param name="workbenchConfig">装配顺序防错数据对象</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        bool InsertWorkbenchConfig(ZPX_ProductWorkbenchConfig workbenchConfig, out string error);

        /// <summary>
        /// 修改零件装配顺序防错的信息
        /// </summary>
        /// <param name="workbenchConfig">装配顺序防错数据对象</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        bool UpdateWorkbenchConfig(ZPX_ProductWorkbenchConfig workbenchConfig, out string error);

        /// <summary>
        /// 删除零件装配顺序防错的信息
        /// </summary>
        /// <param name="product">产品类型</param>
        /// <param name="bench">当前工位</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        bool DeleteWorkbenchConfig(string product, string bench, out string error);
        #endregion

        #region CCD拍照

        /// <summary>
        /// 获得所有零件的CCD拍照防错信息
        /// </summary>
        /// <returns>获取成功返回True，获取失败返回False</returns>
        DataTable GetAllCCDConfig();

        /// <summary>
        /// 修改CCD零件的屏蔽时间
        /// </summary>
        /// <param name="workBench">工位</param>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="starTime">屏蔽开始时间</param>
        /// <param name="endTime">屏蔽终止时间</param>
        /// <param name="error">错误信息</param>
        /// <returns>修改成功返回true，失败返回False</returns>
        bool UpdateCCDConfigTime(string workBench, string goodsCode, string goodsName,
                                    string spec, DateTime starTime, DateTime endTime, out string error);
        /// <summary>
        /// 添加CCD拍照防错的信息
        /// </summary>
        /// <param name="config">装配顺序防错数据对象</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        bool InsertCCDConfig(ZPX_CCDConfig config, out string error);

        /// <summary>
        /// 修改CCD拍照防错的信息
        /// </summary>
        /// <param name="config">CCD拍照防错数据对象</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        bool UpdateCCDConfig(ZPX_CCDConfig config, out string error);

        /// <summary>
        /// 删除CCD拍照防错的信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="bench">工位</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        bool DeleteCCDConfig(string goodsCode, string goodsName, string spec, string bench, out string error);
        #endregion

        /// <summary>
        /// 获取分总成信息
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns>获取成功返回True，获取失败返回False</returns>
        DataTable GetAllAssemblingBom(string productType);

        /// <summary>
        /// 判断分总成是否在该总成之下
        /// </summary>
        /// <param name="assemble">分总成</param>
        /// <param name="productType">产品类型</param>
        /// <returns>获取成功返回True，获取失败返回False</returns>
        bool IsAssemblingBom(string assemble, string productType);
    }
}
