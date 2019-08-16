using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformManagement;
using ServerModule;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 专家人才服务类
    /// </summary>
    class ExpertEmployeServer : Service_Peripheral_HR.IExpertEmployeServer
    {
        /// <summary>
        /// 查询结果过滤器
        /// </summary>
        string m_queryResultFilter = null;

        /// <summary>
        /// 获取或设置查询结果过滤器
        /// </summary>
        public string QueryResultFilter
        {
            get { return m_queryResultFilter; }
            set { m_queryResultFilter = value; }
        }

        /// <summary>
        /// 获取专家专业人才库的信息
        /// </summary>
        /// <param name="returnInfo">专家专业人才信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回获取到的信息</returns>
        public bool GetAllInfo(out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            IAuthorization serverAuthorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = serverAuthorization.Query("查看专家专业人才库", null);
            }
            else
            {
                qr = serverAuthorization.Query("查看专家专业人才库", null, QueryResultFilter);
            }

            if (!qr.Succeeded)
            {
                error = qr.Error;
                return false;
            }

            returnInfo = qr;
            return true;
        }

        /// <summary>
        /// 通过ID获取单个专家专业人才库的信息
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回获取到的信息，否则返回null</returns>
        public HR_ExpertEmploye GetInfoByID(int id, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_ExpertEmploye
                             where a.ID == id
                             select a;

                if (result.Count() == 1)
                {
                    return result.Single();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 添加专家专业人才信息
        /// </summary>
        /// <param name="expertEmploye">专家专业人才数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AddExpertEmploye(HR_ExpertEmploye expertEmploye, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_ExpertEmploye
                             where a.Name == expertEmploye.Name
                             select a;

                if (result.Count() == 0)
                {
                    dataContxt.HR_ExpertEmploye.InsertOnSubmit(expertEmploye);
                    dataContxt.SubmitChanges();
                }
                else
                {
                    error = "姓名叫【" + expertEmploye.Name + "】的人员已经存在！";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改专家专业人才信息
        /// </summary>
        /// <param name="expertEmploye">专家专业人才数据集</param>
        /// <param name="id">id</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true,失败返回False</returns>
        public bool UpdateExpertEmploye(HR_ExpertEmploye expertEmploye, int id, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_ExpertEmploye
                             where a.ID == id
                             select a;

                if (result.Count() != 1)
                {
                    error = "信息有误，请查证后再操作！";
                    return false;
                }
                else
                {

                    HR_ExpertEmploye expertEmployeList = result.Single();

                    expertEmployeList.Address = expertEmploye.Address;
                    expertEmployeList.Age = expertEmploye.Age;
                    expertEmployeList.Introduction = expertEmploye.Introduction;
                    expertEmployeList.MessageSource = expertEmploye.MessageSource;
                    expertEmployeList.Name = expertEmploye.Name;
                    expertEmployeList.OnAccess = expertEmploye.OnAccess;
                    expertEmployeList.Phone = expertEmploye.Phone;
                    expertEmployeList.Recorder = expertEmploye.Recorder;
                    expertEmployeList.Remark = expertEmploye.Remark;
                    expertEmployeList.Sex = expertEmploye.Sex;
                    expertEmployeList.Strong = expertEmploye.Strong;
                    expertEmployeList.TechnicalPost = expertEmploye.TechnicalPost;
                    expertEmployeList.Email = expertEmploye.Email;
                    expertEmployeList.Domain = expertEmploye.Domain;
                    expertEmployeList.Declaration = expertEmploye.Declaration;
                    expertEmployeList.CurrentCompany = expertEmploye.CurrentCompany;
                    expertEmployeList.Birthday = expertEmploye.Birthday;
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 通过id删除专家专业人才信息
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        public bool DeleteTrainEmploye(int id, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_ExpertEmploye
                             where a.ID == id
                             select a;

                dataContxt.HR_ExpertEmploye.DeleteAllOnSubmit(result);

                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }
}
