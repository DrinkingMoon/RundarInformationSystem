using System;
using System.Collections.Generic;
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// 质量数据库接口
    /// </summary>
    public interface IQualitySystemDatabase : IBasicBillServer
    {
        /// <summary>
        /// 批量导入数据
        /// </summary>
        /// <param name="listRecord">数据集</param>
        void BatchInportInfo(List<ZL_Database_Record> listRecord);

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        void DeleteInfo(string billNo);

        /// <summary>
        /// 删除文件结构
        /// </summary>
        /// <param name="guid">唯一编码</param>
        void DeleteFilesStruct(string guid);

        /// <summary>
        /// 更新一条文件信息
        /// </summary>
        /// <param name="fileStruct">文件信息数据集</param>
        void UpdateFileStruct(ZL_Database_FileStruct fileStruct);

        /// <summary>
        /// 插入一条文件信息
        /// </summary>
        /// <param name="fileStruct">文件信息数据集</param>
        void InsertFileStruct(ZL_Database_FileStruct fileStruct);

        /// <summary>
        /// 获得单条文件信息
        /// </summary>
        /// <param name="guid"> 唯一编码</param>
        /// <returns>返回数据集</returns>
        ZL_Database_FileStruct GetFileStructInfo(string guid);

        /// <summary>
        /// 获得文件结构
        /// </summary>
        /// <param name="guid">GUID</param>
        /// <param name="billNo">单据号</param>
        /// <returns>返回Table</returns>
        DataTable GetAllFileStruct(string guid, string billNo);

        /// <summary>
        /// 编辑文件结构信息
        /// </summary>
        /// <param name="fileStruct">LNQ文件结构信息集合</param>
        void EditFileStruct(ServerModule.ZL_Database_FileStruct fileStruct);

        /// <summary>
        /// 编辑业务信息
        /// </summary>
        /// <param name="record">LNQ业务信息集合</param>
        void EditInfo(ServerModule.ZL_Database_Record record);

        /// <summary>
        /// 获得业务信息列表
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回LIST列表信息</returns>
        List<View_ZL_Database_Record> GetListInfo(DateTime startTime, DateTime endTime);

        /// <summary>
        /// 获得设置信息
        /// </summary>
        /// <param name="settingName">设置类型名称</param>
        /// <returns>返回List集合</returns>
        System.Collections.Generic.List<ServerModule.ZL_Database_Settings> GetSettingInfo(string settingName);

        /// <summary>
        /// 获得单条业务信息
        /// </summary>
        /// <param name="billNo">业务编号</param>
        /// <returns>返回LNQ数据集</returns>
        ServerModule.ZL_Database_Record GetSingleInfo(string billNo);
    }
}
