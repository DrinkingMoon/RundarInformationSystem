using System;
using ServerModule;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data;

namespace Service_Manufacture_Storage
{
    public interface IWholeMachineRequisitionService : FlowControlService.IFlowBusinessService
    {
        void SetStatus(DataGridView dgv);

        /// <summary>
        /// 获得业务明细信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务明细列表</returns>
        System.Collections.Generic.List<ServerModule.View_Business_WarehouseOutPut_WholeMachineRequisitionDetail> GetListViewDetailInfo(string billNo);

        /// <summary>
        /// 获得库房顺序信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回库房顺序列表</returns>
        List<View_Business_WarehouseOutPut_WholeMachineRequisition_StorageID> GetListViewStorageIDInfo(string billNo);

        /// <summary>
        /// 获得单条业务总单信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务总单信息</returns>
        ServerModule.Business_WarehouseOutPut_WholeMachineRequisition GetSingleBillInfo(string billNo);

        /// <summary>
        /// 保存业务信息
        /// </summary>
        /// <param name="billInfo">业务总单信息</param>
        /// <param name="detailInfo">业务明细信息</param>
        /// <param name="listStorageID">库房顺序</param>
        void SaveInfo(ServerModule.Business_WarehouseOutPut_WholeMachineRequisition billInfo, 
            System.Collections.Generic.List<ServerModule.View_Business_WarehouseOutPut_WholeMachineRequisitionDetail> detailInfo, 
            System.Collections.Generic.List<ServerModule.View_Business_WarehouseOutPut_WholeMachineRequisition_StorageID> listStorageID);

        void AutoSupplementaryRequisition(string billNo, out List<string> listBillNo);

        void AutoFirstMaterialRequisition(string billNo, out List<string> listBillNo);

        void SignFinish(string billNo);
    }
}
