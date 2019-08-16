using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using DBOperate;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 下线返修服务类
    /// </summary>
    class CVTRepairInfoServer : ServerModule.ICVTRepairInfoServer
    {
        /// <summary>
        /// 根据日期范围查询数据
        /// </summary>
        /// <param name="begin">起始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns>返回查询到的数据</returns>
        public IEnumerable<View_ZPX_CVTRepairInfo> GetViewData(DateTime begin, DateTime end)
        {
            DepotManagementDataContext context = CommentParameter.DepotDataContext;

            var result = from r in context.View_ZPX_CVTRepairInfo
                         where r.记录时间 >= begin.Date && r.记录时间 <= end.Date
                         select r;

            return result;
        }

        /// <summary>
        /// 根据参数实体查询数据
        /// </summary>
        /// <param name="data">检索不为空的数据</param>
        /// <returns>返回查询到的数据</returns>
        public IEnumerable<View_ZPX_CVTRepairInfo> GetViewData(View_ZPX_CVTRepairInfo data)
        {
            DepotManagementDataContext context = CommentParameter.DepotDataContext;

            IEnumerable<View_ZPX_CVTRepairInfo> result = null;

            if (data.序号 > 0)
            {
                result = from r in context.View_ZPX_CVTRepairInfo
                         where r.序号 == data.序号
                         select r;
            }
            else
            {
                result = from r in context.View_ZPX_CVTRepairInfo
                         where r.产品型号 == data.产品型号
                         select r;

                if (!GlobalObject.GeneralFunction.IsNullOrEmpty(data.新箱号) || !GlobalObject.GeneralFunction.IsNullOrEmpty(data.旧箱号))
                {
                    result = from r in result
                             where r.新箱号.Contains(data.新箱号)
                             select r;
                }
            }

            return result;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="data">要添加的数据</param>
        /// <returns>操作是否成功的标志</returns>
        public bool Insert(ZPX_CVTRepairInfo data)
        {
            DepotManagementDataContext context = CommentParameter.DepotDataContext;

            var result = from r in context.ZPX_CVTRepairInfo
                         where r.ProductType == data.ProductType && r.ProductOldNumber == data.ProductOldNumber
                         && r.RepairType == data.RepairType && r.ProductNewNumber == r.ProductNewNumber
                         && r.Hours == data.Hours
                         select r;

            if (result.Count() > 0)
            {
                throw new Exception("已经存在此记录，不允许重复添加");
            }

            context.ZPX_CVTRepairInfo.InsertOnSubmit(data);
            context.SubmitChanges();

            return true;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="data">要更新的数据</param>
        /// <returns>操作是否成功的标志</returns>
        public bool Update(View_ZPX_CVTRepairInfo data)
        {
            DepotManagementDataContext context = CommentParameter.DepotDataContext;

            var result = from r in context.ZPX_CVTRepairInfo
                         where r.ID == data.序号
                         select r;

            if (result.Count() == 0)
            {
                throw new Exception("找不到要更新的记录，无法进行此操作");
            }

            ZPX_CVTRepairInfo updateData = result.Single();

            if (updateData.Recorder != GlobalObject.BasicInfo.LoginID)
            {
                throw new Exception("不是记录创建人员不允许进行此操作");
            }

            if (updateData.Status == "已完成")
            {
                throw new Exception("返修已完成，不允许进行此操作");
            }

            View_ZPX_CVTRepairInfo old = (from r in context.View_ZPX_CVTRepairInfo
                                                 where r.序号 == data.序号
                                                 select r).Single();

            if (GlobalObject.StapleFunction.SimpleEqual<View_ZPX_CVTRepairInfo>(old, data))
            {
                throw new Exception("数据没有任何变化，不需要进行此操作");
            }

            updateData.ProductType = data.产品型号;
            updateData.Assembler = data.返修人员;
            updateData.AssociateBillNo = data.关联单号;
            updateData.AssociateType = data.关联单据类别;
            updateData.BonnetNumber = data.阀块编号;
            updateData.Fault = data.故障现象;
            updateData.Duty = data.责任判定;
            updateData.Hours = data.工时;
            updateData.IsTest = data.是否需要重新跑试验 == "需要" ? true : false;
            updateData.ProductNewNumber = data.新箱号;
            updateData.ProductOldNumber = data.旧箱号;
            updateData.RepairType = data.返修类型;
            updateData.Scheme = data.返修方案及明细;
            updateData.Status = data.单据状态;

            context.SubmitChanges();

            return true;
        }

        /// <summary>
        /// 删除下线返修信息
        /// </summary>
        /// <param name="id">要删除的数据ID</param>
        public bool DeleteRepairInfo(int id)
        {
            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var result = from r in ctx.ZPX_CVTRepairInfo
                             where r.ID == id
                             select r;

                ctx.ZPX_CVTRepairInfo.DeleteAllOnSubmit(result);

                ctx.SubmitChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }           
        }

        /// <summary>
        /// 检测是否能进行试验
        /// </summary>
        /// <param name="productCode">箱号</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>允许试验返回true</returns>
        public bool CanOffLineTest(string productCode, out string error)
        {
            error = null;

            Hashtable paramTable = new Hashtable();
            DataSet ds = new DataSet();

            paramTable.Add("@ProductNumber", productCode);

            IDBOperate dbOperate = CommentParameter.GetDBOperatorOfDepotManagement();

            Dictionary<OperateCMD, object> dicOperateCMD = dbOperate.RunProc_CMD("ZPX_AllowOffLineTest", ds, paramTable);

            DataTable dataTable = ds.Tables[0];

            if (dataTable.Rows[0][0].ToString() == "允许")
            {
                return true;
            }

            error = dataTable.Rows[0][0].ToString();

            if (dataTable.Columns.Count == 1)
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// 质检判定
        /// </summary>
        /// <param name="dataID">要判定的数据编号</param>
        /// <param name="duty">判定内容</param>
        /// <returns>操作是否成功的标志</returns>
        public bool Auditing(int dataID,string duty)
        {
            DepotManagementDataContext context = CommentParameter.DepotDataContext;

            var result = from r in context.ZPX_CVTRepairInfo
                         where r.ID == dataID
                         select r;

            if (result.Count() == 0)
            {
                throw new Exception("找不到要审核的记录，无法进行此操作");
            }

            ZPX_CVTRepairInfo updateData = result.Single();

            updateData.Duty = BasicInfo.LoginName + ServerTime.Time.ToString() + duty;
            updateData.Status = "已完成";

            context.SubmitChanges();

            return true;
        }
    }
}
