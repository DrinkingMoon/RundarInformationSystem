using System;

namespace ServerModule
{
    public interface IZPXProductionParams
    {
        System.Collections.Generic.List<ServerModule.ZPX_ProductionParams> GetParamsList();
        ZPX_ProductionParams GetSingleInfo(int paramsID);
        void SaveInfo(ZPX_ProductionParams productionParams);
    }
}
