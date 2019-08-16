/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  DepotServer.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/07/17
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/07/17 8:54:12 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Collections;
using DBOperate;
using System.Data.Linq;

namespace ServerModule
{
    /// <summary>
    /// 材料类别管理类
    /// </summary>
    class MaterialTypeServer : IMaterialTypeServer
    {
        /// <summary>
        /// 数据库操作接口
        /// </summary>
        IDBOperate m_dbOperate = CommentParameter.GetDBOperatorOfDepotManagement();

        /// <summary>
        /// 获取所有材料类别信息
        /// </summary>
        /// <param name="materialTypes">查询到的材料类别信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllMaterialType(out IQueryable<View_S_Depot> materialTypes, out string error)
        {
            materialTypes = null;
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                Table<View_S_Depot> depotTable = dataContxt.GetTable<View_S_Depot>();

                materialTypes = from c in depotTable orderby c.仓库编码 select c;

                return true;
            }
            catch (Exception err)
            {
                error = err.ToString();
                return false;
            }
        }

        /// <summary>
        /// 获取材料类别信息表
        /// </summary>
        /// <param name="lstMaterialType">材料类别信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取材料类别信息表</returns>
        public bool GetAllMaterialType(out List<MaterialTypeData> lstMaterialType, out string error)
        {
            lstMaterialType = null;
            error = null;

            DataSet ds = new DataSet();

            if (!AccessDB.ExecuteDbProcedure("SelAllS_Depot", ds, out error))
            {
                return false;
            }

            DataTable dt = ds.Tables[0];

            if (dt == null || dt.Rows.Count == 0)
            {
                error = "没有获取到材料类别信息";
                return false;
            }

            lstMaterialType = new List<MaterialTypeData>(dt.Rows.Count);

            foreach (DataRow row in dt.Rows)
            {
                MaterialTypeData info = GetMaterialTypeInfo(row, dt.Columns);
                lstMaterialType.Add(info);
            }

            return true;
        }

        /// <summary>
        /// 从数据行中获取材料类别信息
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="columns">列集合</param>
        /// <returns>从数据行获取到的材料类别信息</returns>
        private MaterialTypeData GetMaterialTypeInfo(DataRow row, DataColumnCollection columns)
        {
            Dictionary<string, string> data = new Dictionary<string, string>(columns.Count);

            foreach (DataColumn column in columns)
            {
                data.Add(column.ColumnName, row[column].ToString());
            }

            MaterialTypeData lnqInfo = new MaterialTypeData();

            lnqInfo.MaterialTypeCode = data["仓库编码"];
            lnqInfo.MaterialTypeName = data["仓库名称"];
            lnqInfo.MaterialTypeGrade = Convert.ToInt32(data["级数"]);
            lnqInfo.IsEnd = Convert.ToBoolean(data["是否末级"]);

            return lnqInfo;
        }

        /// <summary>
        /// 克隆材料类别信息列表
        /// </summary>
        /// <param name="lstSurInfo">要克隆的源数据</param>
        /// <returns>克隆后的功能树节点信息列表</returns>
        public List<MaterialTypeData> Clone(List<MaterialTypeData> lstSurInfo)
        {
            if (lstSurInfo == null)
            {
                return null;
            }

            MaterialTypeData[] arrayInf = new MaterialTypeData[lstSurInfo.Count];

            int index = 0;

            foreach (MaterialTypeData node in lstSurInfo)
            {
                arrayInf[index++] = node.Clone() as MaterialTypeData;
            }

            List<MaterialTypeData> lstClone = new List<MaterialTypeData>(arrayInf);

            return lstClone;
        }

        /// <summary>
        /// 更新材料类别
        /// </summary>
        /// <param name="materialType">要修改的材料类别信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true</returns>
        public bool UpdataMaterialType(MaterialTypeData materialType, out string error)
        {
            error = null;

            Hashtable paramTable = GetParamTable(materialType);

            if (!AccessDB.ExecuteDbProcedure("UpdateMaterialType", paramTable, out error))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取由材料类别信息产生的存储过程参数表
        /// </summary>
        /// <param name="materialTypeInfo">材料类别信息</param>
        /// <returns>产生的参数表</returns>
        private Hashtable GetParamTable(MaterialTypeData materialTypeInfo)
        {
            Dictionary<string, object> dicParam = new Dictionary<string, object>();

            dicParam.Add("@MaterialTypeCode", materialTypeInfo.MaterialTypeCode);
            dicParam.Add("@MaterialTypeName", materialTypeInfo.MaterialTypeName);
            dicParam.Add("@MaterialTypeGrade", materialTypeInfo.MaterialTypeGrade);
            dicParam.Add("@IsEnd", materialTypeInfo.IsEnd);

            return AccessDB.SetSPParam(dicParam);
        }

        /// <summary>
        /// 添加材料类别 并更新父材料类别信息
        /// </summary>
        /// <param name="newMaterialType">要增加的材料类别信息</param>
        /// <param name="parentMaterialType">要更新的父材料类别信息, 如果此信息不为null则需修改父材料类别树信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true</returns>
        public bool AddMaterialType(MaterialTypeData newMaterialType, MaterialTypeData parentMaterialType, out string error)
        {
            error = null;

            Hashtable[] inParamTables = new Hashtable[2];
            Hashtable[] outParamTables = new Hashtable[2];

            inParamTables[0] = GetParamTable(newMaterialType);
            inParamTables[1] = GetParamTable(parentMaterialType);

            string[] spCmds = new string[2];
            spCmds[0] = "AddMaterialType";
            spCmds[1] = "UpdateMaterialType";

            Dictionary<OperateCMD, object> dicOperateCMD =
                AccessDB.GetIDBOperate().Transaction_CMD(spCmds, inParamTables, ref outParamTables);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 添加材料类别
        /// </summary>
        /// <param name="materialType">要增加的材料类别信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true</returns>
        public bool AddMaterialType(MaterialTypeData materialType, out string error)
        {
            error = null;

            Hashtable paramTable = GetParamTable(materialType);

            if (!AccessDB.ExecuteDbProcedure("AddMaterialType", paramTable, out error))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 删除某一材料类别信息
        /// </summary>
        /// <param name="materialTypeCode">材料类别编码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除某一材料类别信息</returns>
        public bool DeleteMaterialType(string materialTypeCode, out string error)
        {
            error = null;

            Hashtable paramTable = new Hashtable();

            paramTable.Add("@MaterialTypeCode", materialTypeCode);

            if (!AccessDB.ExecuteDbProcedure("DelMaterialType", paramTable, out error))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 删除指定材料类别信息（父材料类别参数存在数据时更新父材料类别信息）
        /// </summary>
        /// <param name="materialTypeCode">要删除的材料类别编码</param>
        /// <param name="parentMaterialType">父材料类别信息, 如果此信息不为null则需修改父材料类别树信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true</returns>
        public bool DeleteMaterialType(string materialTypeCode, MaterialTypeData parentMaterialType, out string error)
        {
            error = null;

            Hashtable[] inParamTables = new Hashtable[2];
            Hashtable[] outParamTables = new Hashtable[2];

            inParamTables[0] = new Hashtable();
            inParamTables[0].Add("@MaterialTypeCode", materialTypeCode);

            inParamTables[1] = GetParamTable(parentMaterialType);

            string[] spCmds = new string[2];
            spCmds[0] = "DelMaterialType";
            spCmds[1] = "UpdateMaterialType";

            Dictionary<OperateCMD, object> dicOperateCMD =
                AccessDB.GetIDBOperate().Transaction_CMD(spCmds, inParamTables, ref outParamTables);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            return true;
        }
    }
}
