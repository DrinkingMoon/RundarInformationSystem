using System;
using FlowControlService;
using ServerModule;
using System.Collections.Generic;

namespace Service_Manufacture_WorkShop
{
    public interface IBatchNoChange : IFlowBusinessService
    {
        void SaveInfo(ServerModule.Business_WorkShop_BatchNoChange changeInfo, 
            System.Collections.Generic.List<ServerModule.View_Business_WorkShop_BatchNoChangeDetail> lstDetail);

        Business_WorkShop_BatchNoChange GetSingleInfo(string billNo);

        List<View_Business_WorkShop_BatchNoChangeDetail> GetListDetail(string billNo);

        void OperationBusiness(string billNo);
    }
}
