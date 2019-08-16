/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  BomMappingServer.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/09/06
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : BomMapping服务组件类, 用于设计BOM与装配BOM之间的映射
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/09/06 19:49:08 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using DBOperate;
using System.Linq;

namespace ServerModule
{
    /// <summary>
    /// BomMapping类
    /// </summary>
    class BomMappingServer : BasicServer, IBomMappingServer
    {
        /// <summary>
        /// 获取某一版本的BomMapping信息
        /// </summary>
        /// <param name="productName">产品名称</param>
        /// <returns>操作成功返回获取到的映射表数据</returns>
        public IQueryable<View_P_ProductBomMapping> GetBomMapping(string productName)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_P_ProductBomMapping 
                         where r.产品名称 == productName 
                         select r;

            return result;
        }

        /// <summary>
        /// 获取指定工位的BomMapping信息
        /// </summary>
        /// <param name="workBench">工位号</param>
        /// <returns>操作成功返回获取到的映射表数据</returns>
        public IQueryable<View_P_ProductBomMapping> GetBomMappingOfWorkBench(string workBench)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_P_ProductBomMapping 
                         where r.工位 == workBench 
                         select r;

            return result;
        }

        /// <summary>
        /// 获取指定父总成名称的BomMapping信息
        /// </summary>
        /// <param name="partName">父总成名称</param>
        /// <returns>操作成功返回获取到的映射表数据</returns>
        public IQueryable<View_P_ProductBomMapping> GetChildrenPart(string partName)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_P_ProductBomMapping 
                         where r.父总成名称 == partName 
                         select r;

            return result;
        }

        /// <summary>
        /// 获取工位信息
        /// </summary>
        /// <param name="productName">产品名称</param>
        /// <param name="parentCode">零件父总成编码</param>
        /// <param name="partCode">零件编码</param>
        /// <returns>返回获得的工位信息</returns>
        public List<View_P_ProductBomMapping>GetMappingInfo(string productName, string parentCode, string partCode)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_P_ProductBomMapping
                              where r.产品名称 == productName && r.父总成编码 == parentCode && r.零件编码 == partCode
                              select r;

            return result.ToList();
        }

        /// <summary>
        /// 检测设计与装配基数是否吻合
        /// </summary>
        /// <param name="designedBasicCount">设计BOM中此零件基数</param>
        /// <param name="productName">产品名称</param>
        /// <param name="parentCode">零件父总成编码</param>
        /// <param name="partCode">零件编码</param>
        /// <param name="assemblyAmount">此零件在BOM映射表中的装配数量之和</param>
        /// <returns>返回是否一致的标志</returns>
        public bool IsCoincideBasicCount(int designedBasicCount, string productName, string parentCode, string partCode, out int assemblyAmount)
        {
            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                assemblyAmount = (from r in dataContxt.View_P_ProductBomMapping
                                  where r.产品名称 == productName && r.父总成编码 == parentCode && r.零件编码 == partCode
                                  select r.装配数).Sum();

            }
            catch (Exception err)
            {
                string errMsg = err.Message;
                assemblyAmount = 0;
            }

            return assemblyAmount == designedBasicCount;
        }

        /// <summary>
        /// 添加映射表信息
        /// </summary>
        /// <param name="bomMapping">要添加的信息</param>
        /// <returns>返回是否成功的标志</returns> 
        public bool AddBomMapping(P_ProductBomMapping bomMapping)
        {
            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                dataContxt.P_ProductBomMapping.InsertOnSubmit(bomMapping);
                dataContxt.SubmitChanges(System.Data.Linq.ConflictMode.ContinueOnConflict);

                return true;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                foreach (System.Data.Linq.ObjectChangeConflict occ in dataContxt.ChangeConflicts)
                {
                    // 使用Linq缓存中实体对象的值，覆盖当前数据库中的值  
                    occ.Resolve(System.Data.Linq.RefreshMode.KeepCurrentValues);
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception err)
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                try
                {
                    string error = err.Message;

                    dataContxt.Dispose();

                    dataContxt = CommentParameter.DepotDataContext;
                    dataContxt.P_ProductBomMapping.InsertOnSubmit(bomMapping);

                    dataContxt.SubmitChanges();

                    return true;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
        }

        /// <summary>
        /// 修改指定映射表信息
        /// </summary>
        /// <param name="id">要更新的数据 ID</param>
        /// <param name="bomMapping">修改后的值</param>
        /// <returns>返回是否成功的标志</returns> 
        public bool UpdateBomMapping(int id, P_ProductBomMapping bomMapping)
        {
            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.P_ProductBomMapping 
                             where r.ID == id 
                             select r;

                if (result.Count() == 0)
                {
                    throw new Exception("没有找到要更新的记录无法进行此操作！");
                }

                P_ProductBomMapping updateRecord = result.Single();

                updateRecord.ProductName = bomMapping.ProductName;
                updateRecord.ParentCode = bomMapping.ParentCode;
                updateRecord.ParentName = bomMapping.ParentName;
                updateRecord.PartCode = bomMapping.PartCode;
                updateRecord.PartName = bomMapping.PartName;
                updateRecord.Spec = bomMapping.Spec;
                updateRecord.PartCounts = bomMapping.PartCounts;
                updateRecord.FittingParentName = bomMapping.FittingParentName;
                updateRecord.FittingCounts = bomMapping.FittingCounts;
                updateRecord.Workbench = bomMapping.Workbench;
                updateRecord.NeedToClean = bomMapping.NeedToClean;
                updateRecord.UserCode = bomMapping.UserCode;
                updateRecord.Date = bomMapping.Date;
                updateRecord.Remarks = bomMapping.Remarks;

                dataContxt.SubmitChanges(System.Data.Linq.ConflictMode.ContinueOnConflict);
                return true;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                foreach (System.Data.Linq.ObjectChangeConflict occ in dataContxt.ChangeConflicts)
                {
                    // 使用Linq缓存中实体对象的值，覆盖当前数据库中的值  
                    occ.Resolve(System.Data.Linq.RefreshMode.KeepCurrentValues);
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception err)
            {
                try
                {
                    DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
                    string error = err.Message;

                    dataContxt.Dispose();

                    dataContxt = CommentParameter.DepotDataContext;
                    dataContxt.P_ProductBomMapping.Attach(bomMapping);
                    dataContxt.SubmitChanges();

                    return true;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
        }

        /// <summary>
        /// 更新映射表中某一产品名称
        /// </summary>
        /// <param name="oldProductName">老产品名称</param>
        /// <param name="newProductName">新产品名称</param>
        /// <returns>返回是否成功更新BomMapping中某一版本的版本号</returns>
        public bool UpdateProductName(string oldProductName, string newProductName)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.P_ProductBomMapping 
                         where r.ProductName == oldProductName 
                         select r;

            try
            {
                if (result.Count() == 0)
                {
                    throw new Exception("没有找到要更新的记录无法进行此操作！");
                }

                foreach (var record in result)
                {
                    P_ProductBomMapping updateRecord = record;

                    updateRecord.ProductName = newProductName;
                }

                dataContxt.SubmitChanges(System.Data.Linq.ConflictMode.ContinueOnConflict);
                return true;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                foreach (System.Data.Linq.ObjectChangeConflict occ in dataContxt.ChangeConflicts)
                {
                    // 使用Linq缓存中实体对象的值，覆盖当前数据库中的值  
                    occ.Resolve(System.Data.Linq.RefreshMode.KeepCurrentValues);
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception err)
            {
                try
                {
                    string error = err.Message;

                    dataContxt.Dispose();
                    dataContxt = CommentParameter.DepotDataContext;

                    foreach (var record in result)
                    {
                        P_ProductBomMapping updateRecord = record;

                        updateRecord.ProductName = newProductName;
                        dataContxt.P_ProductBomMapping.Attach(updateRecord);
                    }

                    dataContxt.SubmitChanges();
                    return true;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
        }

        /// <summary>
        /// 删除指定映射表信息
        /// </summary>
        /// <param name="id">要删除的数据 ID</param>
        /// <returns>返回是否成功的标志</returns> 
        public bool DeleteBomMapping(int id)
        {
            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.P_ProductBomMapping 
                             where r.ID == id 
                             select r;

                if (result.Count() == 0)
                {
                    throw new Exception("没有找到要删除的记录无法进行此操作！");
                }

                dataContxt.P_ProductBomMapping.DeleteOnSubmit(result.Single());
                dataContxt.SubmitChanges(System.Data.Linq.ConflictMode.ContinueOnConflict);

                return true;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                foreach (System.Data.Linq.ObjectChangeConflict occ in dataContxt.ChangeConflicts)
                {
                    // 使用Linq缓存中实体对象的值，覆盖当前数据库中的值  
                    occ.Resolve(System.Data.Linq.RefreshMode.KeepCurrentValues);
                }

                dataContxt.SubmitChanges();
                return true;
            }
        }
    }
}
