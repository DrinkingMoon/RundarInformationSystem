using System;
using System.Data;
using ServerModule;
namespace Service_Peripheral_HR
{
    /// <summary>
    /// 合同管理接口类
    /// </summary>
    public interface ILaborContractServer
    {
        /// <summary>
        /// 新增合同类别
        /// </summary>
        /// <param name="laborType">合同类别数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AddLaborType(ServerModule.HR_LaborContractType laborType, out string error);

        /// <summary>
        /// 通过合同类别的编号删除此合同类别
        /// </summary>
        /// <param name="typeCode">合同类别编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteLaborTypeByTypeCode(string typeCode, out string error);

        /// <summary>
        /// 获得所有合同类别
        /// </summary>
        /// <returns>返回数据集</returns>
        System.Data.DataTable GetLaborContracType();

        /// <summary>
        /// 通过合同类别的编号获得合同类别的名称
        /// </summary>
        /// <param name="typeCode">类别编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        string GetLaborTypeByTypeCode(string typeCode, out string error);

        /// <summary>
        /// 通过合同类别的名称获得合同类别的编号
        /// </summary>
        /// <param name="typeName">合同类别名称</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        string GetLaborTypeByTypeName(string typeName, out string error);

        /// <summary>
        /// 获得所有合同模版
        /// </summary>
        /// <returns>返回数据集</returns>
        System.Data.DataTable GetLaborContractTemplet();

        /// <summary>
        /// 通过合同类别的名称获得合同类别
        /// </summary>
        /// <param name="typeName">合同类别名称</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        string GetCategory(string typeName, out string error);

        /// <summary>
        /// 新增合同模板
        /// </summary>
        /// <param name="templet">合同模板数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AddContractTemplet(ServerModule.HR_LaborContractTemplet templet, out string error);

        /// <summary>
        /// 删除合同模板信息
        /// </summary>
        /// <param name="templetID">合同模板数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteContractTemplet(int templetID, out string error);

        /// <summary>
        /// 获得合同状态
        /// </summary>
        /// <returns>返回数据集</returns>
        System.Data.DataTable GetContractStatus();

        /// <summary>
        /// 通过状态编号删除合同状态
        /// </summary>
        /// <param name="statusID">状态编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        bool DeleteContractStatus(int statusID, out string error);

        /// <summary>
        /// 新增合同状态
        /// </summary>
        /// <param name="conStatus">合同状态数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败但会False</returns>
        bool AddContractStatus(ServerModule.HR_LaborContractStatus conStatus, out string error);

        /// <summary>
        /// 新增员工合同信息
        /// </summary>
        /// <param name="personnelContract">员工合同数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回false</returns>
        bool AddPersonnelContract(ServerModule.HR_PersonnelLaborContract personnelContract, out string error);

        /// <summary>
        /// 获取员工合同信息
        /// </summary>
        /// <param name="returnInfo">员工合同信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllPersonnelContarct(out PlatformManagement.IQueryResult returnInfo, out string error);

        /// <summary>
        /// 获取员工合同信息
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <returns>返回数据集</returns>
        DataTable GetPersonnelContarctByWorkID(string workID);

        /// <summary>
        /// 通过状态编号获得合同状态名
        /// </summary>
        /// <param name="statusID">状态编号</param>
        /// <returns>返回合同状态名</returns>
        string GetContractStatusByID(int statusID);

        /// <summary>
        /// 通过状态名获得合同状态编号
        /// </summary>
        /// <param name="statusName">状态名</param>
        /// <returns>返回合同状态编号</returns>
        int GetContractStatusByName(string statusName);

        /// <summary>
        /// 通过合同类别获得合同状态
        /// </summary>
        /// <param name="type">合同类型</param>
        /// <returns>返回数据集</returns>
        DataTable GetContractStatusByType(string type);

        /// <summary>
        /// 通过状态编号获得合同状态标志
        /// </summary>
        /// <param name="statusID">状态编号</param>
        /// <returns>返回合同状态标志</returns>
        bool GetContractStatusFlagByID(string statusID);

        /// <summary>
        /// 通过版本和类别获得合同模版编号
        /// </summary>
        /// <param name="type">类别</param>
        /// <param name="version">版本</param>
        /// <returns>合同模版编号</returns>
        int GetLaborContractTempletByTypeAndVersion(string type, string version);

        /// <summary>
        /// 修改员工合同信息
        /// </summary>
        /// <param name="personnelContractOld">员工原始合同数据集</param>
        /// <param name="personnelContractNew">员工新合同数据集</param>
        /// <param name="flag">状态标志</param>
        /// <param name="billNo">合同编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回false</returns>
        bool UpdatePersonnelContract(ServerModule.HR_PersonnelLaborContractHistory personnelContractOld,
            ServerModule.HR_PersonnelLaborContract personnelContractNew, bool flag, int billNo, out string error);

        /// <summary>
        /// 获取员工合同历史信息
        /// </summary>
        /// <param name="returnInfo">员工合同历史信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllContarctHistory(out PlatformManagement.IQueryResult returnInfo, out string error);

        /// <summary>
        /// 修改合同模板
        /// </summary>
        /// <param name="templet">合同模板数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool UpdateContractTemplet(HR_LaborContractTemplet templet, out string error);

        /// <summary>
        /// 获取员工合同信息
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        View_HR_PersonnelLaborContract GetPersonnelContarctByWorkID(string workID, out string error);
    }
}
