using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlobalObject;
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// 装配BOM数据库操作类
    /// </summary>
    class AssemblingBom : BasicServer, IAssemblingBom
    {
        #region IAssemblingBom 成员

        /// <summary>
        /// 获取指定产品的装配BOM
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <returns>返回指定产品的装配BOM</returns>
        public List<View_P_AssemblingBom> GetAssemblingBom(string productCode)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return (from r in dataContxt.View_P_AssemblingBom
                    where r.产品编码 == productCode
                    //orderby r.父总成编码, r.零件编码, r.工位, r.装配顺序
                    orderby r.工位, r.装配顺序,r.父总成编码, r.零件编码
                    select r).ToList();
        }

        /// <summary>
        /// 获取参数对应的装配BOM信息
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <param name="parentCode">父总成编码</param>
        /// <param name="partCode">零件编码</param>
        /// <param name="spec">规格</param>
        /// <returns>返回装配BOM信息</returns>
        public List<View_P_AssemblingBom> GetAssemblingBom(string productCode, string parentCode, string partCode, string spec)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return (from r in dataContxt.View_P_AssemblingBom
                    where r.产品编码 == productCode && r.父总成编码 == parentCode && r.零件编码 == partCode && r.规格 == spec
                    select r).ToList();
        }

        /// <summary>
        /// 获取参数对应的装配BOM信息
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <param name="partCode">零件编码</param>
        /// <param name="spec">规格</param>
        /// <returns>返回获取到的信息</returns>
        public List<View_P_AssemblingBom> GetAssemblingBom(string productCode, string partCode, string spec)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return (from r in dataContxt.View_P_AssemblingBom
                    where r.产品编码 == productCode && r.零件编码 == partCode && r.规格 == spec
                    select r).ToList();
        }

        /// <summary>
        /// 获取包含指定工位的装配BOM信息
        /// </summary>
        /// <param name="workBench">工位号</param>
        /// <returns>返回获取到的信息</returns>
        public IQueryable<View_P_AssemblingBom> GetInfoOfWorkBench(string workBench)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_P_AssemblingBom
                         where r.工位 == workBench
                         orderby r.装配顺序, r.零件编码
                         select r;

            return result;
        }

        /// <summary>
        /// 检查装配BOM中是否存在指定工位
        /// </summary>
        /// <param name="workbench">要检查的工位号</param>
        /// <returns>存在返回true，不存在返回false</returns>
        public bool IsExistsWorkbench(string workbench)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = (from r in dataContxt.P_AssemblingBom
                          where r.Workbench == workbench
                         select r).Take(1);

            return result.Count() > 0;
        }

        /// <summary>
        /// 获取指定父总成名称的子零件信息
        /// </summary>
        /// <param name="parentName">父总成名称</param>
        /// <returns>返回指定父总成名称的子零件信息</returns>
        public IQueryable<View_P_AssemblingBom> GetChildrenPart(string parentName)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_P_AssemblingBom
                         where r.父总成名称 == parentName
                         orderby r.装配顺序, r.零件编码
                         select r;

            return result;
        }

        /// <summary>
        /// 获取指定产品类型包含的总成名称(包含大总成)
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns>返回获取到的分总成名称列表</returns>
        public string[] GetParentNames(string productType)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            return (from r in dataContext.P_AssemblingBom
                    where r.ProductCode == productType && (r.AssemblyFlag == true)// && !Nullable.Equals(r.ParentName, null)
                    select r.PartName).ToArray();
        }

        /// <summary>
        /// 获取指定产品类型包含的总成名称(不包含大总成)
        /// </summary>
        /// <returns>返回获取到的分总成名称列表</returns>
        public string[] GetParentNames()
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            return (from r in dataContext.P_AssemblingBom
                    where (r.AssemblyFlag == true) && r.ParentName != null
                    select r.PartName).Distinct().ToArray();
        }

        /// <summary>
        /// 获取指定产品类型包含的工位
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns>返回获取到的工位列表</returns>
        public string[] GetWorkbenchs(string productType)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            return (from r in dataContext.P_AssemblingBom
                    where r.ProductCode == productType
                    orderby r.Workbench
                    select r.Workbench).Distinct().ToArray();
        }

        #region 夏石友，2012.07.12 16:00

        /// <summary>
        /// 获取指定产品类型根节点数据
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns>获取到的根节点数据</returns>
        public P_AssemblingBom GetRootNode(string productType)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            return (from r in dataContext.P_AssemblingBom
                    where r.ProductCode == productType && Nullable.Equals(r.ParentCode, null)
                    select r).Single();
        }

        #endregion

        /// <summary>
        /// 添加装配BOM零件信息
        /// </summary>
        /// <param name="info">装配BOM零件信息</param>
        /// <param name="error">False时返回错误信息</param>
        /// <returns>True 添加成功，False 添加失败</returns>
        public bool Add(P_AssemblingBom info, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                info.Date = ServerTime.Time;

                dataContxt.P_AssemblingBom.InsertOnSubmit(info);
                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 判断同一产品、同一零件在同一工位是否已经存在
        /// </summary>
        /// <param name="productCode">产品类型</param>
        /// <param name="goodsCode">零件编码</param>
        /// <param name="workbench">工位</param>
        /// <returns>存在返回true，不存在返回false</returns>
        public bool IsExistGoodsWorkbench(string productCode, string goodsCode, string workbench)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from a in dataContxt.P_AssemblingBom
                         where a.ProductCode == productCode
                         && a.PartCode == goodsCode
                         && a.Workbench == workbench
                         select a;

            if (result.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 删除装配BOM零件信息
        /// </summary>
        /// <param name="id">装配BOM零件信息ID</param>
        /// <param name="error">False时返回的错误信息</param>
        /// <returns>True 删除成功，False 删除失败</returns>
        public bool Delete(int id, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.P_AssemblingBom where r.ID == id select r;

                if (result.Count() == 0)
                {
                    error = string.Format("找不到ID为{0}的零件信息", id);
                    return false;
                }

                P_AssemblingBom lnqInfo = result.Single();

                var childrenPart = from r in dataContxt.P_AssemblingBom
                                   where r.ProductCode == lnqInfo.ProductCode && r.ParentCode == lnqInfo.PartCode
                                   select r;

                if (childrenPart.Count() > 0)
                {
                    error = "只有删除所有子零件后才能删除总成零件";
                    return false;
                }

                dataContxt.P_AssemblingBom.DeleteOnSubmit(lnqInfo);
                dataContxt.SubmitChanges(); 

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 更新装配BOM零件信息
        /// </summary>
        /// <param name="id">装配BOM零件信息ID</param>
        /// <param name="updateInfo">装配BOM零件更新后的信息</param>
        /// <param name="error">False时返回的错误信息</param>
        /// <returns>添加成功返回True 添加失败返回False</returns>
        public bool Update(int id, P_AssemblingBom updateInfo, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
                var result = from r in dataContxt.P_AssemblingBom where r.ID == id select r;

                if (result.Count() == 0)
                {
                    error = string.Format("找不到ID为{0}的零件信息", id);
                    return false;
                }

                P_AssemblingBom lnqInfo = result.Single();

                var childrenPart = from r in dataContxt.P_AssemblingBom
                                   where r.ProductCode == lnqInfo.ProductCode && r.ParentCode == lnqInfo.PartCode
                                   select r;

                DateTime serverTime = ServerTime.Time;

                if (childrenPart.Count() > 0)
                {
                    if (lnqInfo.AssemblyFlag && !updateInfo.AssemblyFlag)
                    {
                        error = "您要将总成零件修改成非总成零件时请确保该总成零件下无子零件";
                        return false;
                    }

                    // 如果分总成图号改变时其子零件的父总成图号也要改变
                    if (updateInfo.PartCode != lnqInfo.PartCode)
                    {
                        foreach (var child in childrenPart)
                        {
                            child.ParentCode = updateInfo.PartCode;
                            child.ParentName = updateInfo.PartName;
                            child.Date = serverTime;
                            child.UserCode = updateInfo.UserCode;
                            child.Remarks += "；修改父总成图号为：" + updateInfo.PartCode;
                        }
                    }
                }

                lnqInfo.ProductCode = updateInfo.ProductCode;
                lnqInfo.ParentCode = updateInfo.ParentCode;
                lnqInfo.ParentName = updateInfo.ParentName;
                lnqInfo.PartCode = updateInfo.PartCode;
                lnqInfo.PartName = updateInfo.PartName;
                lnqInfo.Spec = updateInfo.Spec;
                lnqInfo.IsAdaptingPart = updateInfo.IsAdaptingPart;
                lnqInfo.NeedToClean = updateInfo.NeedToClean;
                lnqInfo.AssemblyFlag = updateInfo.AssemblyFlag;

                #region 2013-09-03 夏石友
                lnqInfo.RasterProofing = updateInfo.RasterProofing;
                #endregion

                #region 2016-11-18 夏石友
                lnqInfo.IsMaterialShortage = updateInfo.IsMaterialShortage;
                #endregion

                lnqInfo.Date = serverTime;

                if (updateInfo.Remarks != "" && updateInfo.Remarks != lnqInfo.Remarks)
                    lnqInfo.Remarks += "；" + updateInfo.Remarks;

                lnqInfo.UserCode = updateInfo.UserCode;
                lnqInfo.Workbench = updateInfo.Workbench;
                lnqInfo.FittingCounts = updateInfo.FittingCounts;
                lnqInfo.OrderNo = updateInfo.OrderNo;

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 复制指定版本的装配BOM信息到目标版本
        /// </summary>
        /// <param name="surEdition">源装配BOM版本号</param>
        /// <param name="tarEdition">目标装配BOM版本号</param>
        /// <param name="error">操作错误时返回的错误信息</param>
        /// <returns>复制成功返回True，复制失败返回False</returns>
        public bool CopyBomData(string surEdition, string tarEdition, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                bool blIsExist = (from r in dataContxt.P_AssemblingBom
                                where r.ProductCode == tarEdition
                                select r).Count() > 0;

                if (blIsExist)
                {
                    error = string.Format("{0}的零件信息已经存在，不允许再次复制", tarEdition);
                    return false;
                }

                var result = from r in dataContxt.P_AssemblingBom 
                             where r.ProductCode == surEdition select r;

                if (result.Count() == 0)
                {
                    error = string.Format("找不到{0}的零件信息", surEdition);
                    return false;
                }

                View_P_ProductInfo lnqTargetProductInfo = (from r in dataContxt.View_P_ProductInfo
                                                  where r.产品类型编码 == tarEdition
                                                  select r).First();

                DateTime dtServerTime = ServerTime.Time;

                foreach (var item in result)
                {
                    P_AssemblingBom lnqInfo = new P_AssemblingBom();

                    lnqInfo.ProductCode = tarEdition;
                    lnqInfo.ParentCode = item.ParentCode;
                    lnqInfo.ParentName = item.ParentName;
                    lnqInfo.PartCode = item.PartCode;
                    lnqInfo.PartName = item.PartName;
                    lnqInfo.Spec = item.Spec;
                    lnqInfo.IsMaterialShortage = item.IsMaterialShortage;
                    lnqInfo.IsAdaptingPart = item.IsAdaptingPart;
                    lnqInfo.NeedToClean = item.NeedToClean;
                    lnqInfo.AssemblyFlag = item.AssemblyFlag;
                    lnqInfo.RasterProofing = item.RasterProofing;
                    lnqInfo.Date = dtServerTime;
                    lnqInfo.Remarks = item.Remarks;
                    lnqInfo.UserCode = GlobalObject.BasicInfo.LoginID;
                    lnqInfo.Workbench = item.Workbench;
                    lnqInfo.FittingCounts = item.FittingCounts;
                    lnqInfo.OrderNo = item.OrderNo;

                    if (lnqInfo.ParentCode == surEdition)
                    {
                        lnqInfo.ParentCode = tarEdition;
                        lnqInfo.ParentName = lnqTargetProductInfo.产品类型名称;
                    }
                    else if (lnqInfo.PartCode == surEdition)
                    {
                        lnqInfo.PartCode = tarEdition;
                        lnqInfo.PartName = lnqTargetProductInfo.产品类型名称;
                    }

                    dataContxt.P_AssemblingBom.InsertOnSubmit(lnqInfo);
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 复制子零件
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="surEdition">源装配BOM版本号</param>
        /// <param name="tarEdition">目标装配BOM产品信息</param>
        /// <param name="parentName">分总成名称</param>
        private void CopyChildPart(DepotManagementDataContext ctx, string surEdition, View_P_ProductInfo tarEdition, string parentName)
        {
            var result = from r in ctx.P_AssemblingBom
                         where r.ProductCode == surEdition && r.ParentName == parentName
                         select r;

            DateTime serverTime = ServerTime.Time;

            foreach (var item in result)
            {
                P_AssemblingBom info = new P_AssemblingBom();

                info.ProductCode = tarEdition.产品类型编码;
                info.ParentCode = item.ParentCode;
                info.ParentName = item.ParentName;
                info.PartCode = item.PartCode;
                info.PartName = item.PartName;
                info.Spec = item.Spec;
                info.IsMaterialShortage = item.IsMaterialShortage;
                info.IsAdaptingPart = item.IsAdaptingPart;
                info.NeedToClean = item.NeedToClean;
                info.AssemblyFlag = item.AssemblyFlag;
                info.RasterProofing = item.RasterProofing;
                info.Date = serverTime;
                info.Remarks = item.Remarks;
                info.UserCode = GlobalObject.BasicInfo.LoginID;
                info.Workbench = item.Workbench;
                info.FittingCounts = item.FittingCounts;
                info.OrderNo = item.OrderNo;

                if (info.ParentCode == surEdition)
                {
                    info.ParentCode = tarEdition.产品类型编码;
                    info.ParentName = tarEdition.产品类型名称;
                }
                else if (info.PartCode == surEdition)
                {
                    info.PartCode = tarEdition.产品类型编码;
                    info.PartName = tarEdition.产品类型名称;
                }

                ctx.P_AssemblingBom.InsertOnSubmit(info);

                if (item.AssemblyFlag)
                {
                    var result2 = from r in ctx.P_AssemblingBom
                                  where r.ProductCode == surEdition && r.ParentName == item.PartName
                                  select r;

                    if (result2.Count() > 0)
                    {
                        CopyChildPart(ctx, surEdition, tarEdition, item.PartName);
                    }
                }
            }
        }

        /// <summary>
        /// 更新分总成下所有零件工位号
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="surEdition">源装配BOM版本号</param>
        /// <param name="parentName">分总成名称</param>
        /// <param name="workBench">要设置的工位号</param>
        private void UpdateWorkBench(DepotManagementDataContext ctx, string surEdition, string parentName, string workBench)
        {
            List<P_AssemblingBom> result = (from r in ctx.P_AssemblingBom
                         where r.ProductCode == surEdition && r.ParentName == parentName
                         select r).ToList();

            DateTime serverTime = ServerTime.Time;

            for (int i = 0; i < result.Count; i++)
            {
                P_AssemblingBom item = result[i];

                List<P_AssemblingBom> result2 = (from r in ctx.P_AssemblingBom
                              where r.ProductCode == surEdition && r.ParentName == parentName &&
                                    r.PartCode == item.PartCode && r.PartName == item.PartName && r.Spec == item.Spec
                              select r).ToList();

                // 本分总成存在多个相同的零件（仅工位不同），此时需要合并
                if (result2.Count > 1)
                {
                    int amount = (from r in ctx.P_AssemblingBom
                                  where r.ProductCode == surEdition && r.ParentName == parentName &&
                                        r.PartCode == item.PartCode && r.PartName == item.PartName && r.Spec == item.Spec
                                  select r.FittingCounts).Sum();


                    item.FittingCounts = amount;

                    for (int index = 1; index < result2.Count; index++)
                    {
                        ctx.P_AssemblingBom.DeleteOnSubmit(result2[index]);
                    }

                    i += result2.Count - 1;
                }

                item.Workbench = workBench;
                item.UserCode = BasicInfo.LoginID;
                item.Date = serverTime;

                if (item.AssemblyFlag)
                {
                    var result3 = from r in ctx.P_AssemblingBom
                                  where r.ProductCode == surEdition && r.ParentName == item.PartName
                                  select r;

                    if (result3.Count() > 0)
                    {
                        UpdateWorkBench(ctx, surEdition, item.PartName, workBench);
                    }
                }
            }
        }

        /// <summary>
        /// 删除分总成下所有零件
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="surEdition">源装配BOM版本号</param>
        /// <param name="parentName">分总成名称</param>
        private void DeletePart(DepotManagementDataContext ctx, string surEdition, string parentName)
        {
            var result = from r in ctx.P_AssemblingBom
                         where r.ProductCode == surEdition && r.ParentName == parentName
                         select r;

            foreach (var item in result)
            {
                ctx.P_AssemblingBom.DeleteOnSubmit(item);

                if (item.AssemblyFlag)
                {
                    var result2 = from r in ctx.P_AssemblingBom
                                  where r.ProductCode == surEdition && r.ParentName == item.PartName
                                  select r;

                    if (result2.Count() > 0)
                    {
                        DeletePart(ctx, surEdition, item.PartName);
                    }
                }
            }
        }

        /// <summary>
        /// 复制指定版本的装配BOM的指定分总成信息到目标版本
        /// </summary>
        /// <param name="surEdition">源装配BOM版本号</param>
        /// <param name="tarEdition">目标装配BOM版本号</param>
        /// <param name="tarParentName">分总成名称</param>
        /// <param name="error">操作错误时返回的错误信息</param>
        /// <returns>复制成功返回True，复制失败返回False</returns>
        public bool CopyBomData(string surEdition, string tarEdition, string tarParentName, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                bool isExist = (from r in ctx.P_AssemblingBom
                                where r.ProductCode == tarEdition && r.ParentName == tarParentName
                                select r).Count() > 0;

                if (isExist)
                {
                    error = string.Format("{0} 的 {1} 已经存在下属零件信息，请删除 {0} {1} 下的所有原有子零件后再进行此操作", 
                        tarEdition, tarParentName);
                    return false;
                }

                var result2 = from r in ctx.P_AssemblingBom
                              where r.ProductCode == tarEdition && r.PartName == tarParentName
                              select r;

                if (result2.Count() == 0)
                {
                    error = string.Format("{0} 中不存在 {1} 零件，无法为不存在的 {1} 添加子零件", tarEdition, tarParentName);
                    return false;
                }
                else
                {
                    if (!result2.Single().AssemblyFlag)
                    {
                        error = string.Format("{0} 中的 {1} 零件被设置为不是分总成，无法为不是分总成的 {1} 添加子零件", 
                            tarEdition, tarParentName);
                        return false;
                    }
                }

                isExist = (from r in ctx.P_AssemblingBom
                           where r.ProductCode == surEdition && r.ParentName == tarParentName
                           select r).Count() > 0;

                if (!isExist)
                {
                    error = string.Format("找不到{0}的零件信息", surEdition);
                    return false;
                }

                View_P_ProductInfo targetProductInfo = (from r in ctx.View_P_ProductInfo
                                                        where r.产品类型编码 == tarEdition
                                                        select r).First();

                CopyChildPart(ctx, surEdition, targetProductInfo, tarParentName);

                ctx.SubmitChanges();
                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 更新分总成下所有零件工位号
        /// </summary>
        /// <param name="surEdition">源装配BOM版本号</param>
        /// <param name="parentName">分总成名称</param>
        /// <param name="workBench">要设置的工位号</param>
        /// <param name="updateParentPart">是否更新时一并更新分总成零件的工位号</param>
        /// <param name="error">操作错误时返回的错误信息</param>
        /// <returns>更新成功返回True，更新失败返回False</returns>
        public bool UpdateWorkBench(string surEdition, string parentName, 
            string workBench, bool updateParentPart, out string error)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            error = null;

            try
            {
                if (updateParentPart)
                {
                    P_AssemblingBom data = (from r in ctx.P_AssemblingBom
                                            where r.ProductCode == surEdition && r.PartName == parentName
                                            select r).Single();

                    data.UserCode = BasicInfo.LoginID;
                    data.Date = ServerTime.Time;
                    data.Workbench = workBench;
                }

                UpdateWorkBench(ctx, surEdition, parentName, workBench);

                ctx.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除分总成下所有零件
        /// </summary>
        /// <param name="surEdition">源装配BOM版本号</param>
        /// <param name="parentName">分总成名称</param>
        /// <param name="error">操作错误时返回的错误信息</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeletePart(string surEdition, string parentName, out string error)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            error = null;

            try
            {
                DeletePart(ctx, surEdition, parentName);

                ctx.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        #region 2013-09-05 夏石友

        /// <summary>
        /// 保存装配顺序
        /// </summary>
        /// <param name="lstData">要保存的列表数据</param>
        public void SaveOrderNo(List<View_P_AssemblingBom> lstData)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            int index = 0;
            string preWorkbench = lstData[0].工位;
            string[] workbenchs = (from r in lstData select r.工位).Distinct().ToArray();

            var clearWorkbenchData = from r in ctx.P_AssemblingBom
                                     where r.ProductCode == lstData[0].产品编码 && !r.RasterProofing && workbenchs.Contains(r.Workbench)
                                     select r;

            foreach (var item in clearWorkbenchData)
            {
                item.OrderNo = 0;
            }

            foreach (var item in lstData)
            {
                if (preWorkbench != item.工位)
                {
                    index = 0;
                    preWorkbench = item.工位;
                }

                var result = from r in ctx.P_AssemblingBom
                             where r.ID == item.序号
                             select r;

                result.Single().OrderNo = ++index;
            }

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 复制指定版本的装配BOM的指定分总成信息到目标版本
        /// </summary>
        /// <param name="surProductType">源装配BOM产品类型</param>
        /// <param name="tarProductType">目标装配BOM产品类型</param>
        /// <param name="workbench">工位</param>
        /// <param name="error">操作错误时返回的错误信息</param>
        /// <returns>复制成功返回True，复制失败返回False</returns>
        public bool CopyAssemblySequence(string surProductType, string tarProductType, string workbench, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var source = from r in ctx.P_AssemblingBom
                             where r.ProductCode == surProductType && r.Workbench == workbench && r.RasterProofing
                             select r;

                if (source.Count() == 0)
                {
                    error = string.Format("{0} 的 {1} 工位不存在光栅防错零件信息，无法进行此操作", surProductType, workbench);
                    return false;
                }

                var target = from r in ctx.P_AssemblingBom
                             where r.ProductCode == tarProductType && r.Workbench == workbench
                             select r;

                if (target.Count() == 0)
                {
                    error = string.Format("{0} 的 {1} 工位不存在任何零件信息，无法进行此操作", tarProductType, workbench);
                    return false;
                }

                bool find = false;

                foreach (var item in source)
                {
                    var data = from r in target
                               where item.PartCode == r.PartCode && item.PartName == r.PartName && item.Spec == r.Spec
                               select r;

                    if (data.Count() > 0)
                    {
                        find = true;

                        data.First().RasterProofing = true;
                        data.First().OrderNo = item.OrderNo;
                    }
                }

                if (!find)
                {
                    error = string.Format("{0} 的 {1} 工位没有找到任何符合要求的零件，无法进行此操作", tarProductType, workbench);
                    return false;
                }

                ctx.SubmitChanges();
                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        #endregion

        #endregion
    }
}
