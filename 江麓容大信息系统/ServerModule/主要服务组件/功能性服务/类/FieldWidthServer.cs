using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerModule
{
    /// <summary>
    /// 字段宽度信息服务类
    /// </summary>
    class FieldWidthServer: BasicServer, ServerModule.IFieldWidthServer
    {
        /// <summary>
        /// 从数据库中获取字段宽度信息
        /// </summary>
        /// <returns>返回获取到的信息</returns>
        public List<Show_FieldWidth> GetFieldWidth()
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            return (from r in dataContxt.Show_FieldWidth
                    select r).ToList();
        }

        /// <summary>
        /// 从数据源中获取满足被参数字段名包含的字段宽度信息
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="fieldName">字段名</param>
        /// <returns>返回查询到的信息</returns>
        public List<Show_FieldWidth> GetFieldWidth(List<Show_FieldWidth> source, string fieldName)
        {
            return (from r in source
                          where fieldName.ToLower().Contains(r.FieldName.ToLower())
                          select r).ToList();
        }
    }
}
