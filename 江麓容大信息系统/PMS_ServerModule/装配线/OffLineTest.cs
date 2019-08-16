using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerModule;
using System.Data;
using System.Reflection;
using System.Collections;
using DBOperate;

namespace ServerModule
{
    /// <summary>
    /// 下线试验结果信息操作类
    /// </summary>
    class OffLineTest : IOffLineTest
    {
        /// <summary>
        /// 根据参数实体查询数据
        /// </summary>
        /// <param name="data">检索不为空的数据</param>
        /// <returns>返回查询到的数据</returns>
        public IEnumerable<View_ZPX_CVTOffLineTestResult> GetViewData(View_ZPX_CVTOffLineTestResult data)
        {
            DepotManagementDataContext context = CommentParameter.DepotDataContext;

            IEnumerable<View_ZPX_CVTOffLineTestResult> result = null;

            if (data.编号 > 0)
            {
                result = from r in context.View_ZPX_CVTOffLineTestResult
                         where r.编号 == data.编号
                         select r;
            }
            else
            {
                result = from r in context.View_ZPX_CVTOffLineTestResult
                         where r.产品型号 == data.产品型号
                         select r;

                if (!GlobalObject.GeneralFunction.IsNullOrEmpty(data.产品箱号))
                {
                    result = from r in result
                             where r.产品箱号.Contains(data.产品箱号)
                             select r;
                }
            }

            return result;
        }

        /// <summary>
        /// 根据日期范围查询数据
        /// </summary>
        /// <param name="begin">起始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns>返回查询到的数据</returns>
        public IEnumerable<View_ZPX_CVTOffLineTestResult> GetViewData(DateTime begin, DateTime end)
        {
            DepotManagementDataContext context = CommentParameter.DepotDataContext;

            var result = from r in context.View_ZPX_CVTOffLineTestResult
                         where r.记录时间 >= begin.Date && r.记录时间 <= end.Date
                         select r;

            return result;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="data">要添加的数据</param>
        /// <returns>操作是否成功的标志</returns>
        public bool Add(ZPX_CVTOffLineTestResult data)
        {
            DepotManagementDataContext context = CommentParameter.DepotDataContext;

            var result = from r in context.ZPX_CVTOffLineTestResult
                         where r.ProductType == data.ProductType && r.ProductNumber == data.ProductNumber &&
                               !r.ReviewFlag
                         select r;

            if (result.Count() > 0)
            {
                throw new Exception("已经存在此记录，不允许重复添加");
            }

            data.Date = ServerTime.Time;

            var resultLog = from a in context.ZPX_CVTOffLineTestResultLog
                            where a.ProdutCode == data.ProductType + " " + data.ProductNumber
                            select a;

            context.ZPX_CVTOffLineTestResultLog.DeleteAllOnSubmit(resultLog);

            context.ZPX_CVTOffLineTestResult.InsertOnSubmit(data);
            context.SubmitChanges();

            return true;
        }

        public void Delete(View_ZPX_CVTOffLineTestResult data)
        {
            DepotManagementDataContext context = CommentParameter.DepotDataContext;

            var result = from r in context.ZPX_CVTOffLineTestResult
                         where r.ID == data.编号
                         select r;

            if (result.Count() == 0)
            {
                throw new Exception("不存在此记录");
            }

            if (result.First().ReviewFlag)
            {
                throw new Exception("记录已经审核，不允许进行此操作");
            }

            context.ZPX_CVTOffLineTestResult.DeleteAllOnSubmit(result);

            var resultLog = from a in context.ZPX_CVTOffLineTestResultLog
                            where a.ProdutCode == data.产品型号 + " " + data.产品箱号
                            select a;

            context.ZPX_CVTOffLineTestResultLog.DeleteAllOnSubmit(resultLog);
            context.SubmitChanges();
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="data">要更新的数据</param>
        /// <returns>操作是否成功的标志</returns>
        public bool Update(View_ZPX_CVTOffLineTestResult data)
        {
            DepotManagementDataContext context = CommentParameter.DepotDataContext;

            var result = from r in context.ZPX_CVTOffLineTestResult
                         where r.ID == data.编号
                         select r;

            if (result.Count() == 0)
            {
                throw new Exception("找不到要更新的记录，无法进行此操作");
            }

            ZPX_CVTOffLineTestResult updateData = result.Single();

            if (updateData.UserCode != GlobalObject.BasicInfo.LoginID)
            {
                throw new Exception("不是记录创建人员不允许进行此操作");
            }

            if (updateData.ReviewFlag)
            {
                throw new Exception("记录已经审核，不允许进行此操作");
            }

            View_ZPX_CVTOffLineTestResult old = (from r in context.View_ZPX_CVTOffLineTestResult
                                               where r.编号 == data.编号
                                               select r).Single();

            if (GlobalObject.StapleFunction.SimpleEqual<View_ZPX_CVTOffLineTestResult>(old, data))
            {
                throw new Exception("数据没有任何变化，不需要进行此操作");
            }

            PlatformManagement.ILogManagement log = 
                PlatformManagement.PlatformFactory.GetObject<PlatformManagement.ILogManagement>();

            log.WriteUpdateLog<View_ZPX_CVTOffLineTestResult>(GlobalObject.BasicInfo.LoginID, "下线试验信息管理",    
                new List<string>(new string[] { "编号" }), old, data);

            updateData.ProductType = data.产品型号;
            updateData.ProductNumber = data.产品箱号;
            updateData.Assembler = data.预装员工号;
            updateData.Date = ServerTime.Time;
            updateData.QualifiedFlag = data.合格标志;
            updateData.Fault = data.故障现象;
            updateData.Remark = data.备注;

            context.SubmitChanges();

            return true;
        }

        /// <summary>
        /// 更新说明
        /// </summary>
        /// <param name="data">要更新的数据</param>
        /// <returns>操作是否成功的标志</returns>
        public bool UpdateRemark(View_ZPX_CVTOffLineTestResult data)
        {
            DepotManagementDataContext context = CommentParameter.DepotDataContext;

            var result = from r in context.ZPX_CVTOffLineTestResult
                         where r.ID == data.编号
                         select r;

            if (result.Count() == 0)
            {
                throw new Exception("找不到要更新的记录，无法进行此操作");
            }

            ZPX_CVTOffLineTestResult updateData = result.Single();

            if (updateData.UserCode != GlobalObject.BasicInfo.LoginID)
            {
                throw new Exception("不是记录创建人员不允许进行此操作");
            }

            View_ZPX_CVTOffLineTestResult old = (from r in context.View_ZPX_CVTOffLineTestResult
                                                 where r.编号 == data.编号
                                                 select r).Single();

            PlatformManagement.ILogManagement log =
                PlatformManagement.PlatformFactory.GetObject<PlatformManagement.ILogManagement>();

            log.WriteUpdateLog<View_ZPX_CVTOffLineTestResult>(GlobalObject.BasicInfo.LoginID, "下线试验信息管理",
                new List<string>(new string[] { "编号" }), old, data);

            updateData.Remark = data.备注;

            context.SubmitChanges();

            return true;
        }

        /// <summary>
        /// 审核数据
        /// </summary>
        /// <param name="dataID">要审核的数据编号</param>
        /// <returns>操作是否成功的标志</returns>
        public bool Auditing(int dataID)
        {
            DepotManagementDataContext context = CommentParameter.DepotDataContext;

            var result = from r in context.ZPX_CVTOffLineTestResult
                         where r.ID == dataID
                         select r;

            if (result.Count() == 0)
            {
                throw new Exception("找不到要审核的记录，无法进行此操作");
            }

            ZPX_CVTOffLineTestResult updateData = result.Single();

            if (updateData.UserCode != GlobalObject.BasicInfo.LoginID)
            {
                throw new Exception("不是记录创建人员不允许进行此操作");
            }

            if (updateData.ReviewFlag)
            {
                throw new Exception("记录已经审核，不允许进行此操作");
            }

            updateData.ReviewFlag = true;
            updateData.ReviewDate = ServerTime.Time;

            context.SubmitChanges();

            return true;
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

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.ZPX_CVTOffLineTestResultLog
                          where a.ProdutCode == productCode
                          select a;

            if (varData.Count() == 0)
            {
                ZPX_CVTOffLineTestResultLog lnqLog = new ZPX_CVTOffLineTestResultLog();

                lnqLog.ProdutCode = productCode;
                lnqLog.Z08FinishTime = Convert.ToDateTime(dataTable.Rows[0][1]);

                ctx.ZPX_CVTOffLineTestResultLog.InsertOnSubmit(lnqLog);
            }
            else if (varData.Count() == 1)
            {
                ZPX_CVTOffLineTestResultLog lnqLog = varData.Single();

                lnqLog.Z08FinishTime = Convert.ToDateTime(dataTable.Rows[0][1]);
            }

            ctx.SubmitChanges();

            return false;
        }

        /// <summary>
        /// 获得记录表的数据
        /// </summary>
        /// <returns>返回Table</returns>
        public IEnumerable GetLogInfo()
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from a in ctx.ZPX_CVTOffLineTestResultLog
                          select new { 产品编码 = a.ProdutCode, Z08工位的结束时间 = a.Z08FinishTime };

            return result;
        }

        /// <summary>
        /// 获取强制下线试验信息
        /// </summary>
        /// <returns></returns>
        public IQueryable<View_ZPX_ForcedOffLineTest> GetForcedOffLineTestInfo()
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.View_ZPX_ForcedOffLineTest
                         select r;

            return result;
        }

        /// <summary>
        /// 添加强制下线试验信息
        /// </summary>
        /// <param name="data">要添加的数据</param>
        public void AddForcedOffLineTestInfo(ZPX_ForcedOffLineTest data)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            data.RecordTime = ServerTime.Time;

            ctx.ZPX_ForcedOffLineTest.InsertOnSubmit(data);

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 删除强制下线试验信息
        /// </summary>
        /// <param name="id">要删除的数据ID</param>
        public void DeleteForcedOffLineTestInfo(int id)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.ZPX_ForcedOffLineTest
                         where r.ID == id
                         select r;

            ctx.ZPX_ForcedOffLineTest.DeleteAllOnSubmit(result);

            ctx.SubmitChanges();
        }
    }
}
