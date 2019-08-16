using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMS_ServerModule;

namespace ServerModule
{
    /// <summary>
    /// 工位用途
    /// </summary>
    public class WorkbenchPurpose
    {
        /// <summary>
        /// 工位号
        /// </summary>
        private string m_workbench;

        public string 工位
        {
            get { return m_workbench; }
            set { m_workbench = value; }
        }

        /// <summary>
        /// 用途编号
        /// </summary>
        public int m_purposeID;

        public int 装配用途编号
        {
            get { return m_purposeID; }
            set { m_purposeID = value; }
        }

        /// <summary>
        /// 用途名称
        /// </summary>
        public string m_purpose;

        public string 装配用途名称
        {
            get { return m_purpose; }
            set { m_purpose = value; }
        }
    }

    /// <summary>
    /// 生产模块配置管理服务(装配用途、用途权限分配、工位用途设置)
    /// </summary>
    class ConfigManagement : IConfigManagement
    {
        #region 装配用途
        
        /// <summary>
        /// 获取所有装配用途
        /// </summary>
        /// <returns>获取到的装配用途信息</returns>
        public IQueryable<View_ZPX_MultiBatchPartPurpose> GetAssemblingPurpose()
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_ZPX_MultiBatchPartPurpose
                         orderby r.装配用途编号
                         select r;

            return result;
        }

        /// <summary>
        /// 添加装配用途
        /// </summary>
        /// <param name="purpose">用途名称</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true, 失败返回false</returns>
        public bool AddAssemblingPurpose(string purpose, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.View_ZPX_MultiBatchPartPurpose
                             where r.装配用途名称 == purpose
                             select r;

                if (result.Count() > 0)
                {
                    error = "装配用途已经存在不允许重复添加";
                    return false;
                }

                ZPX_AssemblingPurpose purposeObj = new ZPX_AssemblingPurpose();

                purposeObj.Purpose = purpose;

                dataContxt.ZPX_AssemblingPurpose.InsertOnSubmit(purposeObj);

                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改装配用途
        /// </summary>
        /// <param name="oldPurpose">旧用途名称</param>
        /// <param name="newPurpose">新用途名称</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true, 失败返回false</returns>
        public bool UpdateAssemblingPurpose(string oldPurpose, string newPurpose, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.ZPX_AssemblingPurpose
                             where r.Purpose == oldPurpose
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("“{0}”装配用途不存在，无法修改", oldPurpose);
                    return false;
                }

                ZPX_AssemblingPurpose purposeObj = result.Single();

                purposeObj.Purpose = newPurpose;

                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        #endregion 装配用途

        #region 给人员分配装配用途权限, 每个工位都会分配装配用途，只有分配了权限的人员才可以配置工位、装配多批次信息等

        /// <summary>
        /// 获取所有装配用途权限
        /// </summary>
        /// <returns>获取到的装配用途权限信息</returns>
        public IQueryable<View_ZPX_PersonnelAuthority> GetPurposeAuthority()
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_ZPX_PersonnelAuthority
                         orderby r.工号, r.装配用途编号
                         select r;

            return result;
        }

        /// <summary>
        /// 获取指定人员具有的装配用途权限信息
        /// </summary>
        /// <param name="workID">人员工号</param>
        /// <returns>获取到的装配用途权限信息</returns>
        public IQueryable<View_ZPX_PersonnelAuthority> GetPurposeAuthority(string workID)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_ZPX_PersonnelAuthority
                         where r.工号 == workID
                         select r;

            return result;
        }

        /// <summary>
        /// 给人员添加装配用途权限
        /// </summary>
        /// <param name="workID">人员工号</param>
        /// <param name="purposeID">用途编号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true, 失败返回false</returns>
        public bool AddPurposeAuthority(string workID, int purposeID, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.View_ZPX_PersonnelAuthority
                             where r.工号 == workID && r.装配用途编号 == purposeID
                             select r;

                if (result.Count() > 0)
                {
                    error = string.Format("{0} 人员已经分配了 {1} 装配用途权限不允许重复分配", workID, purposeID);
                    return false;
                }

                ZPX_PersonnelAuthority obj = new ZPX_PersonnelAuthority();

                obj.WorkID = workID;
                obj.PurposeID = purposeID;

                dataContxt.ZPX_PersonnelAuthority.InsertOnSubmit(obj);

                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除指定用户装配用途权限
        /// </summary>
        /// <param name="workID">工号</param>
        /// <param name="purposeID">用途编号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true, 失败返回false</returns>
        public bool DeletePurposeAuthority(string workID, int purposeID, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.ZPX_PersonnelAuthority
                             where r.WorkID == workID && r.PurposeID == purposeID
                             select r;

                if (result.Count() == 0)
                {
                    return true;
                }

                ZPX_PersonnelAuthority obj = result.Single();

                dataContxt.ZPX_PersonnelAuthority.DeleteOnSubmit(obj);

                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        #endregion 给人员分配装配用途权限

        #region 给工位分配装配用途, 只有分配了权限的人员才可以配置工位信息

        /// <summary>
        /// 获取所有工位用途信息
        /// </summary>
        /// <returns>获取到的工位用途信息</returns>
        public IQueryable<WorkbenchPurpose> GetWorkbenchPurpose()
        {
            PlatformServiceDataContext dataContxt = ParameterFactory.PlatformDataContext;

            var result = from r in dataContxt.View_ZPX_WorkBenchConfig
                         orderby r.工位
                         select new WorkbenchPurpose { 工位 = r.工位, 装配用途编号 = r.用途编号, 装配用途名称 = r.用途 };

            return result;
        }

        /// <summary>
        /// 获取指定人员具有权限的工位用途信息
        /// </summary>
        /// <param name="workID">人员工号</param>
        /// <returns>获取到的工位用途信息</returns>
        public IQueryable<WorkbenchPurpose> GetWorkbenchPurpose(string workID)
        {
            IQueryable<View_ZPX_PersonnelAuthority> purposeAuthority = GetPurposeAuthority(workID);

            if (purposeAuthority.Count() == 0)
            {
                return null;
            }

            List<int> lstPurposeID = (from r in purposeAuthority
                                     select r.装配用途编号).ToList();

            PlatformServiceDataContext dataContxt = ParameterFactory.PlatformDataContext;

            var result = from r in dataContxt.View_ZPX_WorkBenchConfig
                         where lstPurposeID.Contains(r.用途编号)
                         select new WorkbenchPurpose { 工位 = r.工位, 装配用途编号 = r.用途编号, 装配用途名称 = r.用途 };

            return result;
        }

        /// <summary>
        /// 给工位设置装配用途
        /// </summary>
        /// <param name="workbench">工位号</param>
        /// <param name="purposeID">用途编号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true, 失败返回false</returns>
        public bool UpdateWorkbenchPurpose(string workbench, int purposeID, out string error)
        {
            error = null;

            try
            {
                PlatformServiceDataContext dataContxt = ParameterFactory.PlatformDataContext;

                var result = from r in dataContxt.ZPX_WorkBenchConfig
                             where r.工位 == workbench
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("{0} 工位配置信息不存在，无法进行此设置", workbench);
                    return false;
                }

                ZPX_WorkBenchConfig obj = result.Single();

                obj.用途编号 = purposeID;

                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }
 
        #endregion 给人员分配装配用途权限
    }
}
