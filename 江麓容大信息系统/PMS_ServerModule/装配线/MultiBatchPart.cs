using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using DBOperate;
using PlatformManagement;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 多批次数据服务
    /// </summary>
    class MultiBatchPartServer : BasicServer, ServerModule.IMultiBatchPartServer
    {
        /// <summary>
        /// 获取当前登录人员在多批次管理中许可操作的用途信息
        /// </summary>
        /// <returns>返回获取到的用途名称信息</returns>
        public IQueryable<View_ZPX_PersonnelAuthority> GetPersonnelPurpose()
        {
            DepotManagementDataContext dc = CommentParameter.DepotDataContext;

            return from r in dc.View_ZPX_PersonnelAuthority
                   where GlobalObject.BasicInfo.LoginID == r.工号
                   select r;
        }

        /// <summary>
        /// 获取用户指定权限范围内的所有数据
        /// </summary>
        /// <returns>返回获取到的数据</returns>
        public IQueryable<View_ZPX_MultiBatchPart> GetData()
        {
            DepotManagementDataContext dc = CommentParameter.DepotDataContext;

            IQueryable<View_ZPX_MultiBatchPart> result = null;

            if (GlobalObject.BasicInfo.IsFuzzyContainsRoleName("业务系统管理员"))
            {
                result = from r in dc.View_ZPX_MultiBatchPart
                         select r;
            }
            else
            {
                result = from r in dc.View_ZPX_MultiBatchPart
                         join s in dc.View_ZPX_PersonnelAuthority
                             on r.用途编号 equals s.装配用途编号
                         where s.工号 == GlobalObject.BasicInfo.LoginID
                         select r;
            }

            return result;
        }

        /// <summary>
        /// 获取指定日期范围内的数据（用户指定权限范围内）
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>返回获取到的数据</returns>
        public IQueryable<View_ZPX_MultiBatchPart> GetData(DateTime beginDate, DateTime endDate)
        {
            beginDate = beginDate.Date;
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            DepotManagementDataContext dc = CommentParameter.DepotDataContext;

            IQueryable<View_ZPX_MultiBatchPart> result = null;

            if (GlobalObject.BasicInfo.IsFuzzyContainsRoleName("业务系统管理员"))
            {
                result = from r in dc.View_ZPX_MultiBatchPart
                         where r.更新日期 >= beginDate && r.更新日期 <= endDate
                         select r;
            }
            else
            {
                result = from r in dc.View_ZPX_MultiBatchPart
                         join s in dc.View_ZPX_PersonnelAuthority
                             on r.用途编号 equals s.装配用途编号
                         where s.工号 == GlobalObject.BasicInfo.LoginID && r.更新日期 >= beginDate && r.更新日期 <= endDate
                         select r;
            }

            return result;
        }

        /// <summary>
        /// 获取指定条形码的数据
        /// </summary>
        /// <param name="barCode">要获取数据的条形码</param>
        /// <returns>返回获取到的数据</returns>
        public IQueryable<View_ZPX_MultiBatchPart> GetData(int barCode)
        {
            DepotManagementDataContext dc = CommentParameter.DepotDataContext;

            IQueryable<View_ZPX_MultiBatchPart> result = null;

            if (GlobalObject.BasicInfo.IsFuzzyContainsRoleName("业务系统管理员"))
            {
                result = from r in dc.View_ZPX_MultiBatchPart
                         where r.条形码 == barCode
                         select r;
            }
            else
            {
                result = from r in dc.View_ZPX_MultiBatchPart 
                         join s in dc.View_ZPX_PersonnelAuthority
                         on r.用途编号 equals s.装配用途编号
                         where r.条形码 == barCode && s.工号 == GlobalObject.BasicInfo.LoginID
                         select r;
            }

            return result;
        }

        /// <summary>
        /// 检查多批次中指定领料单是否已经导入（防止多次导入）
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <returns>存在返回true</returns>
        public bool IsExistMRBill(string billNo)
        {
            DepotManagementDataContext dc = CommentParameter.DepotDataContext;

            var result = (from r in dc.ZPX_MultiBatchPart
                         where r.Remark.Contains(billNo)
                         select r).Take(1);

            return result.Count() > 0;
        }

        /// <summary>
        /// 根据单据物品明细添加多批次信息（从领料单、营销出库单)
        /// </summary>
        /// <param name="userCode">操作用户</param>
        /// <param name="purposeID">多批次用途编号</param>
        /// <param name="cvtNumber">变速箱号</param>
        /// <param name="billNo">单据号</param>
        /// <param name="lstGoods">领料物品明细列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回true</returns>
        public bool AddFromBill(string userCode, int purposeID, string cvtNumber, string billNo, List<StorageGoods> lstGoods, out string error)
        {
            error = null;

            if (lstGoods == null || lstGoods.Count == 0)
            {
                error = "物品明细为空，无法添加多批次信息";
                return false;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                IBarCodeServer barCodeServer = PMS_ServerFactory.GetServerModule<IBarCodeServer>();
                DateTime dt = ServerTime.Time;
                List<P_AssemblingBom> bomDatas = null;
                string productType = cvtNumber.Trim();
                string[] arrayCvtNumber = null; // 存放分隔后的箱号

                // 下线车间再制造
                if (purposeID == 5)
                {
                    if (GlobalObject.GeneralFunction.IsNullOrEmpty(productType))
                    {
                        error = "产品类型不允许为空";
                        return false;
                    }

                    arrayCvtNumber = productType.Split(new char[] { ' ' });

                    productType = arrayCvtNumber[0] + " FX";

                    bomDatas = (from r in ctx.P_AssemblingBom
                                where r.ProductCode == productType
                                select r).ToList();

                    if (bomDatas.Count < 100)
                    {
                        error = string.Format("还未配置 [{0}] 型号装配BOM的数据请配置后再进行此操作", productType);
                        return false;
                    }
                }

                // 是否向多批次中添加了数据的标志
                bool addDataFlag = false;
                
                for (int i = 0; i < lstGoods.Count; i++)
                {
                    string tempCVTNumber = cvtNumber;

                    // 下线车间再制造
                    if (purposeID == 5)
                    {
                        IProductParts productParts = BasicServerFactory.GetServerModule<IProductParts>();

                        // 如果不是一次性物品, 而且在装配BOM中找不到则报错
                        if (!productParts.IsDisposableParts(productType, lstGoods[i].GoodsCode))
                        {
                            if (arrayCvtNumber.Length != 2)
                            {
                                error = "您导入的单据中包含非一次性物料，缺少产品箱号，无法进行此操作";
                                return false;
                            }

                            if (!bomDatas.Exists(p => p.PartCode == lstGoods[i].GoodsCode && p.PartName == lstGoods[i].GoodsName))
                            {
                                error = string.Format("无法在装配BOM的 {0} 的数据中找到{1}, {2} 的物品信息，无法进行此操作！",
                                    productType, lstGoods[i].GoodsCode, lstGoods[i].GoodsName);

                                return false;
                            }
                        }
                        else
                        {
                            tempCVTNumber = "";
                        }
                    }

                    View_S_InDepotGoodsBarCodeTable barCode = barCodeServer.GetBarCodeInfo(
                        lstGoods[i].GoodsCode, lstGoods[i].GoodsName, lstGoods[i].Spec,
                        lstGoods[i].Provider, lstGoods[i].BatchNo, lstGoods[i].StorageID.ToString());

                    if (barCode == null)
                    {
                        error = string.Format("无法找到 {0}, {1}, 规格：{2}, 供应商：{3}, 批次号：{4}, 库存编码：{5} 物品的条形码信息，无法进行此操作！",
                            lstGoods[i].GoodsCode, lstGoods[i].GoodsName, lstGoods[i].Spec,
                            lstGoods[i].Provider, lstGoods[i].BatchNo, lstGoods[i].StorageID);

                        return false;
                    }

                    var result = from r in ctx.ZPX_MultiBatchPart
                                 where r.BarCode == barCode.条形码 && r.PurposeID == purposeID && r.CVTNumber == tempCVTNumber
                                 select r;

                    ZPX_MultiBatchPart mbp = null;

                    if (result.Count() > 0)
                    {
                        mbp = result.Single();
                        mbp.Counts += Convert.ToInt32(lstGoods[i].Quantity);

                        if (mbp.Remark.Length > 0)
                            mbp.Remark += "\r\n";

                        mbp.UpdateDate = dt;
                        mbp.Remark += ServerTime.Time.ToString("yyyy-MM-dd HH:mm:ss") + ", 用户 " + userCode + ", 单据号：" + billNo + ", 增加数量：" + lstGoods[i].Quantity.ToString() + "；";
                    }
                    else
                    {
                        mbp = new ZPX_MultiBatchPart();

                        mbp.BarCode = barCode.条形码;
                        mbp.PurposeID = purposeID;
                        mbp.CVTNumber = tempCVTNumber;
                        mbp.Counts = Convert.ToInt32(lstGoods[i].Quantity);
                        mbp.Remark = dt.ToString("yyyy-MM-dd HH:mm:ss") + ", 用户 " + userCode + ", 单据号：" + billNo + "，数量：" + mbp.Counts.ToString() + "；";
                        mbp.CreateDate = dt;
                        mbp.UpdateDate = dt;
                        mbp.InputDate = barCode.入库时间;
                        mbp.Msrepl_tran_version = Guid.NewGuid();

                        ctx.ZPX_MultiBatchPart.InsertOnSubmit(mbp);
                    }

                    if (mbp.Counts < 0)
                    {
                        throw new Exception("【图号型号】：" + lstGoods[i].GoodsCode 
                            + "【物品名称】：" + lstGoods[i].GoodsName + "【规格】：" + lstGoods[i].Spec 
                            + "【批次号】：" + lstGoods[i].BatchNo + " 物品数量不能小于0");
                    }

                    addDataFlag = true;
                    ctx.SubmitChanges();
                }

                if (addDataFlag)
                {
                    ZPX_MultiBatchPart_LLD lld = new ZPX_MultiBatchPart_LLD();

                    lld.Bill_ID = billNo;

                    ctx.ZPX_MultiBatchPart_LLD.InsertOnSubmit(lld);
                    ctx.SubmitChanges();
                }
                else
                {
                    throw new Exception("没有添加任何数据，可能是单据中的零件不属于装配BOM");
                }

                ctx.Transaction.Commit();
                return true;
            }
            catch (Exception err)
            {
                ctx.Transaction.Rollback();
                error = err.Message;
                return false;
            }
        }

        /// <summary>
        /// 由用户直接添加多批次信息
        /// </summary>
        /// <param name="userCode">用户编号</param>
        /// <param name="purposeID">用途编号</param>
        /// <param name="barCodeId">条形码ID</param>
        /// <param name="cvtNumber">变速箱号</param>
        /// <param name="count">装配数量</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回true</returns>
        public bool Add(string userCode, int purposeID, int barCodeId, string cvtNumber, int count, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
                IBarCodeServer barCodeServer = BasicServerFactory.GetServerModule<IBarCodeServer>();
                View_S_InDepotGoodsBarCodeTable barCode = null;

                if (!barCodeServer.GetData(barCodeId, out barCode, out error))
                    return false;

                var result = from r in ctx.ZPX_MultiBatchPart                             
                             where r.BarCode == barCodeId && r.PurposeID == purposeID && r.CVTNumber == cvtNumber
                             select r;

                if (result.Count() > 0)
                {
                    error = string.Format("条形码 [{0}] 的物品已经存在！", barCode.条形码);
                    return false;
                }

                ZPX_MultiBatchPart mbp = new ZPX_MultiBatchPart();
                DateTime dt = ServerTime.Time;

                // 下线车间再制造
                if (purposeID == 5)
                {
                    string productType = cvtNumber.Split(new char[] { ' ' })[0];

                    IProductParts productParts = BasicServerFactory.GetServerModule<IProductParts>();

                    // 如果是一次性物品, 则不用CVT编号
                    if (productParts.IsDisposableParts(productType, barCode.图号型号))
                    {
                        cvtNumber = "";
                    }
                }

                mbp.BarCode = barCode.条形码;
                mbp.PurposeID = purposeID;
                mbp.CVTNumber = cvtNumber;
                mbp.Counts = count;
                mbp.Remark = dt.ToString("yyyy-MM-dd HH:mm:ss") + ", 用户 " + userCode +  " 增加数量 " + count.ToString() + " ；";
                mbp.CreateDate = dt;
                mbp.UpdateDate = dt;
                mbp.InputDate = barCode.入库时间;
                mbp.Msrepl_tran_version = Guid.NewGuid();

                ctx.ZPX_MultiBatchPart.InsertOnSubmit(mbp);

                ctx.SubmitChanges();
                return true;
            }
            catch (Exception err)
            {
                error = err.Message;
                return false;
            }
        }

        /// <summary>
        /// 根据返修零件增加条形码
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <param name="purposeID">用途编号</param>
        /// <param name="dicBarcode">条形码字典</param>
        /// <param name="error">出错时的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool AddFromReparativePartList(string userCode, int purposeID, Dictionary<int, int> dicBarcode, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
                IBarCodeServer barCodeServer = BasicServerFactory.GetServerModule<IBarCodeServer>();
                View_S_InDepotGoodsBarCodeTable barCode = null;
                DateTime dt = ServerTime.Time;

                foreach (KeyValuePair<int,int> item in dicBarcode)
                {
                    var result = from r in ctx.ZPX_MultiBatchPart
                                 where r.BarCode == item.Key && r.PurposeID == purposeID
                                 select r;

                    ZPX_MultiBatchPart mbp = null;

                    if (result.Count() == 0)
                    {
                        mbp = new ZPX_MultiBatchPart();

                        mbp.BarCode = item.Key;
                        mbp.PurposeID = purposeID;
                        mbp.CVTNumber = "";
                        mbp.Counts = item.Value;
                        mbp.CreateDate = dt;
                        mbp.UpdateDate = dt;
                        mbp.Remark += dt.ToString("yyyy-MM-dd HH:mm:ss") + ", 用户 " + userCode + " 增加数量为：" + mbp.Counts.ToString() + " ；";

                        if (!barCodeServer.GetData(item.Key, out barCode, out error))
                            return false;

                        mbp.InputDate = barCode.入库时间;
                        mbp.Msrepl_tran_version = Guid.NewGuid();

                        ctx.ZPX_MultiBatchPart.InsertOnSubmit(mbp);
                    }
                    else
                    {
                        mbp = result.Single();

                        mbp.Counts += item.Value;
                        mbp.UpdateDate = dt;

                        if (mbp.Remark.Length > 0)
                            mbp.Remark += "\r\n";

                        mbp.Remark += dt.ToString("yyyy-MM-dd HH:mm:ss") + ", 用户 " + userCode + " 增加数量：" + item.Value.ToString() + " ；";
                    }
                }

                ctx.SubmitChanges();
                return true;
            }
            catch (Exception err)
            {
                error = err.Message;
                return false;
            }
        }

        /// <summary>
        /// 由用户更新多批次信息
        /// </summary>
        /// <param name="userCode">用户编号</param>
        /// <param name="purposeID">用途编号</param>
        /// <param name="barCodeId">条形码ID</param>
        /// <param name="cvtNumber">变速箱号</param>
        /// <param name="count">装配数量</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回true</returns>
        public bool Update(string userCode, int purposeID, int barCodeId, string cvtNumber, int count, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
                IBarCodeServer barCodeServer = BasicServerFactory.GetServerModule<IBarCodeServer>();

                S_InDepotGoodsBarCodeTable barCode = null;

                if (!barCodeServer.GetData(barCodeId, out barCode, out error))
                    return false;

                var result = from r in ctx.ZPX_MultiBatchPart
                             where r.BarCode == barCodeId && r.PurposeID == purposeID && r.CVTNumber == cvtNumber
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("您所属部门对应的多批次条形码 [{0}] 的物品不存在！", barCode.ID);
                    return false;
                }

                ZPX_MultiBatchPart mbp = result.Single();

                if (mbp.Counts == count)
                {
                    error = string.Format("条形码 [{0}] 的物品中原有数量与您当前设置的数量一致，不需要更改！", barCode.ID);
                    return false;
                }

                mbp.Counts = count;

                if (mbp.Remark.Length > 0)
                    mbp.Remark += "\r\n";

                mbp.Remark += ServerTime.Time.ToString("yyyy-MM-dd HH:mm:ss") + ", 用户 " + userCode + " 更新数量为：" + count.ToString() + " ；";

                ctx.SubmitChanges();
                return true;
            }
            catch (Exception err)
            {
                error = err.Message;
                return false;
            }
        }

        /// <summary>
        /// 用户删除指定条形码多批次信息
        /// </summary>
        /// <param name="purposeID">用途编号</param>
        /// <param name="barCodeId">条形码编号</param>
        /// <param name="cvtNumber">变速箱号</param>       
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回true</returns>
        public bool Delete(int purposeID, int barCodeId, string cvtNumber, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
                IBarCodeServer barCodeServer = BasicServerFactory.GetServerModule<IBarCodeServer>();
                S_InDepotGoodsBarCodeTable barCode = null;

                if (!barCodeServer.GetData(barCodeId, out barCode, out error))
                    return false;

                var result = from r in ctx.ZPX_MultiBatchPart
                             where r.BarCode == barCodeId && r.PurposeID == purposeID && r.CVTNumber == cvtNumber
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("您所属部门对应的多批次条形码 [{0}] 的物品不存在！", barCode.ID);
                    return false;
                }

                ctx.ZPX_MultiBatchPart.DeleteAllOnSubmit(result);
                ctx.SubmitChanges();
                return true;
            }
            catch (Exception err)
            {
                error = err.Message;
                return false;
            }
        }

        /// <summary>
        /// 用户删除指定用途的所有多批次信息
        /// </summary>
        /// <param name="purposeID">用途编号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回true</returns>
        public bool Delete(int purposeID, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var result = from r in ctx.ZPX_MultiBatchPart
                             where r.PurposeID == purposeID
                             select r;

                ctx.ZPX_MultiBatchPart.DeleteAllOnSubmit(result);
                ctx.SubmitChanges();
                return true;
            }
            catch (Exception err)
            {
                error = err.Message;
                return false;
            }
        }
    }
}
