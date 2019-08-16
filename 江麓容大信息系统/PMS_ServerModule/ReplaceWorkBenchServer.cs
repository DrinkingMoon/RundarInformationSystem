using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlobalObject;

namespace ServerModule
{
    class ReplaceWorkBenchServer : ServerModule.IReplaceWorkBenchServer
    {
        /// <summary>
        /// 获得所有替换工位的信息
        /// </summary>
        /// <returns>获取成功返回dt，获取失败返回null</returns>
        public List<ZPX_ReplaceWorkBench> GetAllData()
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from a in dataContxt.ZPX_ReplaceWorkBench
                         select a;
            return result.ToList();
        }

        /// <summary>
        /// 添加可替换工位的信息
        /// </summary>
        /// <param name="replace">可替换工位信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        public bool Insert(ZPX_ReplaceWorkBench replace, out string error)
        {
            error = "";

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from a in dataContxt.ZPX_ReplaceWorkBench
                             where a.主工位 == replace.主工位
                             && a.可替换工位 == replace.可替换工位 && a.适用产品类型 == replace.适用产品类型
                             select a;

                if (result.Count() == 0)
                {
                    dataContxt.ZPX_ReplaceWorkBench.InsertOnSubmit(replace);
                }
                else
                {
                    error = "数据重复，请检查！";
                    return false;
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
        /// 修改可替换工位的信息
        /// </summary>
        /// <param name="replace">可替换工位信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        public bool Update(ZPX_ReplaceWorkBench replace, out string error)
        {
            error = "";

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from a in dataContxt.ZPX_ReplaceWorkBench
                             where a.主工位 == replace.主工位 && a.可替换工位 == replace.可替换工位 
                             && a.适用产品类型 == replace.适用产品类型
                             select a;

                if (result.Count() == 1)
                {
                    error = "数据重复，请检查！";
                    return false;
                }
                else
                {
                    result = from a in dataContxt.ZPX_ReplaceWorkBench
                                 where a.ID == replace.ID
                                 select a;

                    ZPX_ReplaceWorkBench replaceWorkBench = result.Single();

                    replaceWorkBench.记录人员 = BasicInfo.LoginID;
                    replaceWorkBench.记录时间 = ServerTime.Time;
                    replaceWorkBench.可替换工位 = replace.可替换工位;
                    replaceWorkBench.适用产品类型 = replace.适用产品类型;
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
        /// 删除可替换工位的信息
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        public bool Delete(int id, out string error)
        {
            error = "";

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from a in dataContxt.ZPX_ReplaceWorkBench
                             where a.ID == id
                             select a;

                if (result.Count() != 1)
                {
                    error = "数据有误，请检查！";
                    return false;
                }
                else
                {
                    dataContxt.ZPX_ReplaceWorkBench.DeleteAllOnSubmit(result);
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
    }
}
