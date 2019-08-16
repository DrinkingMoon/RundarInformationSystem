using System;

namespace ServerModule
{
    /// <summary>
    /// 字段宽度信息服务接口
    /// </summary>
    public interface IFieldWidthServer
    {
        /// <summary>
        /// 从数据库中获取字段宽度信息
        /// </summary>
        /// <returns>返回获取到的信息</returns>
        System.Collections.Generic.List<Show_FieldWidth> GetFieldWidth(System.Collections.Generic.List<Show_FieldWidth> source, 
            string fuzzyField);

        /// <summary>
        /// 从数据源中获取满足被参数字段名包含的字段宽度信息
        /// </summary>
        /// <returns>返回查询到的信息</returns>
        System.Collections.Generic.List<Show_FieldWidth> GetFieldWidth();
    }
}
