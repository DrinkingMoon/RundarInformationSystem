using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoUpgradeSystem
{
    /// <summary>
    /// 没有文件内容的升级文件信息
    /// </summary>
    class UpgradeFileInfoNoFileContent
    {
        public int 序号;
        public string 软件系统名称;
        public double 版本号;
        public string 文件名称;
        public string 文件大小;
        public DateTime 日期;

        public UpgradeFileInfoNoFileContent()
        {
        }

        public UpgradeFileInfoNoFileContent(int id, string sysName, double version, string fileName, string fileSize, DateTime date)
        {
            序号 = id;
            软件系统名称 = sysName;
            版本号 = version;
            文件名称 = fileName;
            文件大小 = fileSize;
            日期 = date;
        }
    }

    /// <summary>
    /// 升级管理数据库操作类
    /// </summary>
    class UpgradeManagement
    {
        /// <summary>
        /// 权限管理数据库操作器
        /// </summary>
        PlatfromDatabaseDataContext m_ctx;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UpgradeManagement()
        {
            m_ctx = new PlatfromDatabaseDataContext(GlobalParameter.PlatformServiceConnectionString);
            m_ctx.DeferredLoadingEnabled = false;
        }

        /// <summary>
        /// 获取所有升级信息
        /// </summary>
        /// <returns>成功返回获取到的升级信息，失败返回null</returns>
        public IQueryable<Sys_AutoUpgrade> GetUpgradeInfo()
        {
            var result = from r in m_ctx.Sys_AutoUpgrade select r;
            return result;
        }

        /// <summary>
        /// 获取不含文件内容的升级信息
        /// </summary>
        /// <returns></returns>
        public IQueryable<UpgradeFileInfoNoFileContent> GetUpgradeInfoNoFileContent()
        {
            var result = from r in m_ctx.Sys_AutoUpgrade select 
                             new UpgradeFileInfoNoFileContent { 序号 = r.序号, 版本号 = r.版本号, 日期 = r.日期, 
                                 软件系统名称 = r.软件系统名称, 文件大小 = r.文件大小, 文件名称 = r.文件名称 };
            return result;
        }

        /// <summary>
        /// 获取指定软件系统名称下的所有升级信息集
        /// </summary>
        /// <param name="softSystemName">软件系统名称</param>
        /// <returns>成功返回获取到的升级信息集,失败返回null</returns>
        public IQueryable<Sys_AutoUpgrade> GetUpgradeInfo(string softSystemName)
        {
            var result = from r in m_ctx.Sys_AutoUpgrade
                         where r.软件系统名称 == softSystemName
                         select r;

            if (result.Count() == 0)
            {
                return null;
            }
            return result;
        }

        /// <summary>
        /// 获取指定纪录编号下的升级信息
        /// </summary>
        /// <param name="id">纪录编号</param>
        /// <returns>成功返回获取到的升级信息,失败返回null</returns>
        public Sys_AutoUpgrade GetUpgradeInfo(int id)
        {
            int oldCommandTimeout = m_ctx.CommandTimeout;

            try
            {
                m_ctx.CommandTimeout = 300;

                var result = from r in m_ctx.Sys_AutoUpgrade where r.序号 == id select r;

                if (result.Count() == 0)
                {
                    return null;
                }

                return result.First();
            }
            finally
            {
                m_ctx.CommandTimeout = oldCommandTimeout;
            }
        }

        /// <summary>
        /// 添加新升级信息
        /// </summary>
        /// <param name="record">要添加的新升级信息</param>
        /// <returns>成功返回true,其他返回false</returns>
        public bool AddUpgradeInfo(Sys_AutoUpgrade record)
        {
            m_ctx.Sys_AutoUpgrade.InsertOnSubmit(record);
            m_ctx.SubmitChanges();

            return true;
        }

        /// <summary>
        /// 删除升级信息
        /// </summary>
        /// <param name="record">要删除的升级信息</param>
        /// <returns>成功返回true,其他返回false</returns>
        public bool DeleteUpgradeInfo(Sys_AutoUpgrade record)
        {
            var result = m_ctx.Sys_AutoUpgrade.Single(r => r.序号 == record.序号);
            m_ctx.Sys_AutoUpgrade.DeleteOnSubmit(result);
            m_ctx.SubmitChanges();

            return true;
        }

        /// <summary>
        /// 更新升级信息
        /// </summary>
        /// <param name="business">要更新的升级信息</param>
        /// <returns>成功返回true,其他返回false</returns>
        public bool UpdateUpgradeInfo(Sys_AutoUpgrade record)
        {
            Sys_AutoUpgrade result = m_ctx.Sys_AutoUpgrade.Single(r => r.序号 == record.序号);

            result.软件系统名称 = record.软件系统名称;
            result.文件名称 = record.文件名称;
            result.版本号 = record.版本号;
            result.文件内容 = record.文件内容;
            result.文件大小 = record.文件大小;

            m_ctx.SubmitChanges();

            return true;
        }
    }
}
