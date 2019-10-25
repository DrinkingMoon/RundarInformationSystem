using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerModule;

namespace Service_Project_Design
{
    public interface IPBOMChangeService : FlowControlService.IFlowBusinessService
    {
        Bus_PBOM_Change GetItem(string billNo);

        List<View_Bus_PBOM_Change_Detail> GetDetail(string billNo);

        void SaveInfo(Bus_PBOM_Change billInfo, List<View_Bus_PBOM_Change_Detail> detail);

        void OperatarUnFlowBusiness(string billNo);
    }
}
