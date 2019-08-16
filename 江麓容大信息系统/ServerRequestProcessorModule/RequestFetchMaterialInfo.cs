/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  RequestFetchMaterialProcessor.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/07/17
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/07/17 8:54:12 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using AsynSocketService;
using GlobalObject;
using SocketCommDefiniens;
using PlatformManagement;
using ServerModule;

namespace ServerRequestProcessorModule
{
    /// <summary>
    /// 响应领料请求处理器
    /// </summary>
    public class RequestFetchMaterialInfo
    {
        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_basicGoodsServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();
        
        /// <summary>
        /// 物品条形码服务组件
        /// </summary>
        IBarCodeServer m_barCodeServer = ServerModuleFactory.GetServerModule<IBarCodeServer>();

        /// <summary>
        /// 领料单物品明细服务组件
        /// </summary>
        IMaterialRequisitionGoodsServer m_requestMaterialServer = ServerModuleFactory.GetServerModule<IMaterialRequisitionGoodsServer>();

        /// <summary>
        /// 接收事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="re">事件参数</param>
        public Socket_FetchMaterial ReceiveReadBarCodeInfo(Socket_FetchMaterial fetchMaterialInfo)
        {
            string error;

            // 获取物品条形码信息
            S_InDepotGoodsBarCodeTable goodsInfo = null;

            if (!m_barCodeServer.GetData(Convert.ToInt32(fetchMaterialInfo.BarCode), out goodsInfo, out error))
            {
                fetchMaterialInfo.FetchState = Socket_FetchMaterial.FetchStateEnum.条形码有误;
                return fetchMaterialInfo;
            }

            // 检查领料物品清单中是否存在接收的单据号
            if (!m_requestMaterialServer.IsExist(fetchMaterialInfo.BillID))
            {
                fetchMaterialInfo.FetchState = Socket_FetchMaterial.FetchStateEnum.单据号有误;
                return fetchMaterialInfo;
            }

            // 获取领料单中的物品信息
            View_S_MaterialRequisitionGoods goods = m_requestMaterialServer.GetGoods(fetchMaterialInfo.BillID, goodsInfo);

            if (goods == null)
            {
                fetchMaterialInfo.DesireCount = 0;
                fetchMaterialInfo.FetchState = Socket_FetchMaterial.FetchStateEnum.领料单中无该零件的领料信息;
                return fetchMaterialInfo;
            }

            View_F_GoodsPlanCost basicGoods = m_basicGoodsServer.GetGoodsInfoView(goodsInfo.GoodsID);

            fetchMaterialInfo.GoodsCode = basicGoods.图号型号;
            fetchMaterialInfo.GoodsName = basicGoods.物品名称;
            fetchMaterialInfo.Spec = basicGoods.规格;
            fetchMaterialInfo.Provider = goodsInfo.Provider;
            fetchMaterialInfo.BatchNo = goodsInfo.BatchNo;
            fetchMaterialInfo.DesireCount = Convert.ToInt32(goods.请领数);
            fetchMaterialInfo.FetchState = Socket_FetchMaterial.FetchStateEnum.操作成功;

            return fetchMaterialInfo;
        }

        /// <summary>
        /// 接收事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="re">事件参数</param>
        public Socket_FetchMaterial ReceiveSaveBarCodeInfo(Socket_FetchMaterial fetchMaterialInfo)
        {
            string error;

            S_MaterialRequisitionGoods goods = new S_MaterialRequisitionGoods();

            View_F_GoodsPlanCost basicGoods = m_basicGoodsServer.GetGoodsInfo(
                fetchMaterialInfo.GoodsCode, fetchMaterialInfo.GoodsName, fetchMaterialInfo.Spec, out error);

            goods.Bill_ID = fetchMaterialInfo.BillID;
            goods.GoodsID = basicGoods.序号;
            goods.ProviderCode = fetchMaterialInfo.Provider;
            goods.BatchNo = fetchMaterialInfo.BatchNo;
            goods.RealCount = fetchMaterialInfo.FactCount;

            if (!m_requestMaterialServer.UpdateyGoodsFromWireless(goods, out error))
            {
                fetchMaterialInfo.FetchState = Socket_FetchMaterial.FetchStateEnum.更新领料清单失败;
            }
            else
            {
                fetchMaterialInfo.FetchState = Socket_FetchMaterial.FetchStateEnum.操作成功;
            }

            return fetchMaterialInfo;
        }
    }
}
