/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ISystemFileBasicInfo.cs
 * 作者    :  曹津彬    版本: v1.00    日期: 2014/01/22
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 ******************************************************************************/
using System;
using System.Data;
using System.Collections.Generic;
using ServerModule;
using GlobalObject;

namespace Service_Quality_File
{
    /// <summary>
    /// 体系文件分类服务接口
    /// </summary>
    public interface ISystemFileBasicInfo
    {

        void DeleteFile(int fileId);

        /// <summary>
        /// 修改文件信息
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        void ModifyFileInfo(FM_FileList fileInfo);

        /// <summary>
        /// 添加文件信息
        /// </summary>
        /// <param name="fileInfo">LINQ文件信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,否则返回False</returns>
        bool AddFileList(FM_FileList fileInfo, out string error);

        /// <summary>
        /// 文件上传业务
        /// </summary>
        /// <param name="localhostFilePath">本机文件路径</param>
        /// <param name="ftpFilePath">FTP文件路径</param>
        /// <param name="guid">文件唯一编码</param>
        /// <param name="fileType">文件类型</param>
        void FileUpLoad(string localhostFilePath, string ftpFilePath, string guid, string fileType);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="uniqueCode">唯一编码</param>
        /// <param name="fileType">文件类型</param>
        void UpdateFile(Guid uniqueCode, string fileType);

        /// <summary>
        /// 通过唯一编码获得文件路径
        /// </summary>
        /// <param name="uniqueCode">文件唯一编码</param>
        /// <returns>返回LNQ数据集</returns>
        FM_FilePath GetFilePathInfo(Guid uniqueCode);

        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="uniqueCode">文件唯一编码</param>
        /// <param name="path">文件路径</param>
        /// <param name="fileType">文件类型</param>
        void AddFile(Guid uniqueCode, string path, string fileType);

        /// <summary>
        /// 对相关文件进行FTP中的操作
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="fileID">文件ID</param>
        /// <param name="sortID">类别ID</param>
        /// <param name="error">错误信息</param>
        void OperatorFTPSystemFile(DepotManagementDataContext ctx, int fileID, int sortID);

        /// <summary>
        /// 获得单一文件信息
        /// </summary>
        /// <param name="fileID">文件ID</param>
        /// <returns>返回LNQ数据集</returns>
        FM_FileList GetFile(int fileID);

        /// <summary>
        /// 获得文件信息
        /// </summary>
        /// <param name="fileNo">文件编号</param>
        /// <param name="fileName">文件名称</param>
        /// <returns>返回Table</returns>
        DataTable GetFilesInfo(string fileNo, string fileName);

        /// <summary>
        /// 获得文件分类的文件路径
        /// </summary>
        /// <param name="sortID">类别ID</param>
        /// <returns>返回文件路径</returns>
        string SortPath(int sortID);

        /// <summary>
        /// 获得文件类别信息
        /// </summary>
        /// <param name="sortID">类别ID</param>
        /// <returns>返回类别信息</returns>
        FM_FileSort SortInfo(int sortID);

        /// <summary>
        /// 获得一定数量的文件信息
        /// </summary>
        /// <param name="listFileID">文件ID列表</param>
        /// <returns>返回Table</returns>
        DataTable GetFilesInfo(List<int> listFileID);

        /// <summary>
        /// 获得体系文件结构树
        /// </summary>
        /// <param name="fileType">文件类别</param>
        /// <returns></returns>
        DataTable GetTreeInfo(CE_FileType fileType);

        /// <summary>
        /// 获得所有
        /// </summary>
        /// <param name="fileType">文件类别</param>
        /// <returns></returns>
        System.Data.DataTable GetAllInfo(CE_FileType fileType);

        /// <summary>
        /// 对体系文件类别进行操作
        /// </summary>
        /// <param name="mode">操作类别</param>
        /// <param name="sort">LINQ数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool Operator(GlobalObject.CE_OperatorMode mode, ServerModule.FM_FileSort sort, out string error);
    }
}
