using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using GlobalObject;
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// 领料单用途服务接口
    /// </summary>
    class MaterialRequisitionPurposeServer : BasicServer, IMaterialRequisitionPurposeServer
    {
        /// <summary>
        /// 获取所有用途
        /// </summary>
        /// <returns>返回获取到的单据用途信息</returns>
        public IQueryable<BASE_MaterialRequisitionPurpose> GetAllPurpose()
        {
            string error = "";
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.BASE_MaterialRequisitionPurpose
                         where r.IsDisable == true
                         select r;

            Hashtable hs = new Hashtable();

            hs.Add("@DeptCode", BasicInfo.DeptCode);
            DataTable dtTemp = 
                GlobalObject.DatabaseServer.QueryInfoPro("Business_Finance_GetPurpose_Department", hs, out error);

            List<BASE_MaterialRequisitionPurpose> lstTemp = new List<BASE_MaterialRequisitionPurpose>();

            foreach (BASE_MaterialRequisitionPurpose item in result.ToList())
            {
                if (DataSetHelper.SiftDataTable(dtTemp, "Code = '"+ item.Code +"'").Rows.Count > 0)
                {
                    lstTemp.Add(item);
                }
            }

            return lstTemp.AsQueryable<BASE_MaterialRequisitionPurpose>();
        }

        /// <summary>
        /// 根据用途编码获取用途信息
        /// </summary>
        /// <param name="purpose">用途编码/用途名称</param>
        /// <returns>返回获取到的用途信息, 失败返回null</returns>
        public BASE_MaterialRequisitionPurpose GetBillPurpose(string purpose)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return GetBillPurpose(dataContxt, purpose);
        }

        /// <summary>
        /// 根据用途编码获取用途信息
        /// </summary>
        /// <param name="dataContxt">数据上下文</param>
        /// <param name="purpose">用途编码/用途名称</param>
        /// <returns>返回获取到的用途信息, 失败返回null</returns>
        public BASE_MaterialRequisitionPurpose GetBillPurpose(DepotManagementDataContext dataContxt, string purpose)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(purpose))
            {
                return null;
            }

            var result = from r in dataContxt.BASE_MaterialRequisitionPurpose
                         where (r.Code == purpose || r.Purpose == purpose)
                         && r.IsDisable == true
                         select r;


            if (result.Count() == 1)
            {
                return result.Single();
            }

            throw new Exception("获取用途：【"+ purpose +"】失败");
        }
    }
}
