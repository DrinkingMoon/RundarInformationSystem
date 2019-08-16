using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlobalObject;

namespace ServerModule
{
    class ZPXProductionParams : IZPXProductionParams
    {
        public List<ZPX_ProductionParams> GetParamsList()
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.ZPX_ProductionParams
                          select a;

            return varData.ToList();
        }

        public ZPX_ProductionParams GetSingleInfo(int paramsID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.ZPX_ProductionParams
                          where a.ID == paramsID
                          select a;

            if (varData.Count() != 1)
            {
                throw new Exception("数据不唯一或者为空");
            }
            else
            {
                return varData.Single();
            }
        }

        public void SaveInfo(ZPX_ProductionParams productionParams)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.ZPX_ProductionParams
                          where a.ID == productionParams.ID
                          select a;

            if (varData.Count() != 1)
            {
                throw new Exception("数据不唯一或者为空");
            }

            ZPX_ProductionParams tempParams = varData.Single();

            tempParams.DataType = productionParams.DataType;
            tempParams.RangeOfValues = productionParams.RangeOfValues;
            tempParams.Remark = productionParams.Remark;
            tempParams.Value1 = productionParams.Value1;
            tempParams.Value2 = productionParams.Value2;
            tempParams.Value3 = productionParams.Value3;
            tempParams.Value4 = productionParams.Value4;
            tempParams.Value5 = productionParams.Value5;
            tempParams.Value6 = productionParams.Value6;

            RecordLog(ctx, productionParams);

            ctx.SubmitChanges();
        }

        void RecordLog(DepotManagementDataContext ctx, ZPX_ProductionParams productionParams)
        {
            ZPX_ProductionParams_Log tempLnqLog = new ZPX_ProductionParams_Log();

            tempLnqLog.DataType = productionParams.DataType;
            tempLnqLog.RangeOfValues = productionParams.RangeOfValues;
            tempLnqLog.Remark = productionParams.Remark;
            tempLnqLog.Value1 = productionParams.Value1;
            tempLnqLog.Value2 = productionParams.Value2;
            tempLnqLog.Value3 = productionParams.Value3;
            tempLnqLog.Value4 = productionParams.Value4;
            tempLnqLog.Value5 = productionParams.Value5;
            tempLnqLog.Value6 = productionParams.Value6;

            tempLnqLog.ParamsID = productionParams.ID;
            tempLnqLog.OperationTime = ServerTime.Time;
            tempLnqLog.OperationMode = CE_OperatorMode.修改.ToString();
            tempLnqLog.OperationPersonnel = BasicInfo.LoginID;

            ctx.ZPX_ProductionParams_Log.InsertOnSubmit(tempLnqLog);
        }
    }
}
