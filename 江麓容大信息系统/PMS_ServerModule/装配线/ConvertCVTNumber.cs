using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlobalObject;
using System.Collections;
using DBOperate;
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// 转换变速箱箱号检测方式枚举
    /// </summary>
    public enum ConvertCVTNumber_CheckEnum
    {
        /// <summary>
        /// 检查新箱变更信息
        /// </summary>
        检查新箱信息,

        /// <summary>
        /// 检查新箱档案信息
        /// </summary>
        检查新箱档案信息,

        /// <summary>
        /// 检查旧箱变更信息
        /// </summary>
        检查旧箱信息,

        /// <summary>
        /// 检查旧箱档案信息
        /// </summary>
        检查旧箱档案信息,

        /// <summary>
        /// 检查新旧箱变更信息
        /// </summary>
        检查新旧箱信息

    }

    /// <summary>
    /// 转换变速箱箱号信息操作类
    /// </summary>
    class ConvertCVTNumber : ServerModule.IConvertCVTNumber
    {
        /// <summary>
        /// 电子档案服务
        /// </summary>
        IElectronFileServer m_electronFileServer = PMS_ServerFactory.GetServerModule<IElectronFileServer>();

        /// <summary>
        /// 装配BOM服务
        /// </summary>
        IAssemblingBom m_assemblingBom = PMS_ServerFactory.GetServerModule<IAssemblingBom>();

        /// <summary>
        /// 检查箱号是否在变更表中存在（存在于旧箱号或新箱号）
        /// </summary>
        /// <param name="checkMode">检测方式</param>
        /// <param name="productType">产品类型编号</param>
        /// <param name="productNumber">箱号</param>
        /// <returns>存在返回true</returns>
        public bool IsExists(ConvertCVTNumber_CheckEnum checkMode, string productType, string productNumber)
        {
            DepotManagementDataContext context = CommentParameter.DepotDataContext;

            IQueryable<View_ZPX_ConvertedCVTNumber> result = null;
            IQueryable<P_ElectronFile> result2 = null;

            switch (checkMode)
            {
                case ConvertCVTNumber_CheckEnum.检查新箱信息:
                    result = from r in context.View_ZPX_ConvertedCVTNumber
                             where (r.新箱型号 == productType && r.新箱编号 == productNumber)
                             select r;
                    break;

                case ConvertCVTNumber_CheckEnum.检查旧箱信息:
                    result = from r in context.View_ZPX_ConvertedCVTNumber
                             where (r.旧箱型号 == productType && r.旧箱编号 == productNumber)
                             select r;
                    break;

                case ConvertCVTNumber_CheckEnum.检查新旧箱信息:
                    result = from r in context.View_ZPX_ConvertedCVTNumber
                             where (r.旧箱型号 == productType && r.旧箱编号 == productNumber) || (
                                    r.新箱型号 == productType && r.新箱编号 == productNumber)
                             select r;
                    break;

                case ConvertCVTNumber_CheckEnum.检查旧箱档案信息:
                case ConvertCVTNumber_CheckEnum.检查新箱档案信息:
                    result2 = from r in context.P_ElectronFile
                             where r.ProductCode == productType + " " + productNumber
                             select r;

                    break;
            }

            if (result != null)
                return result.Count() > 0;
            else
                return result2.Count() > 0;
        }

        /// <summary>
        /// 判断变速箱是否新箱
        /// </summary>
        /// <param name="productType">产品类型编号</param>
        /// <param name="productNumber">箱号</param>
        /// <returns>是新箱返回true</returns>
        public bool IsNewCVT(string productType, string productNumber)
        {
            DepotManagementDataContext context = CommentParameter.DepotDataContext;

            var result = from r in context.ProductsCodes
                         where r.GoodsCode == productType && r.ProductCode == productNumber
                         select r;

            return result.Count() == 0;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="startDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>返回获取到的数据集</returns>
        public IQueryable<View_ZPX_ConvertedCVTNumber> GetData(DateTime startDate, DateTime endDate)
        {
            DepotManagementDataContext context = CommentParameter.DepotDataContext;

            return from r in context.View_ZPX_ConvertedCVTNumber
                   where r.日期 >= startDate.Date && r.日期 < endDate.Date
                   select r;
        }

        /// <summary>
        /// 添加维修记录(不仅变更箱号还根据旧箱电子档案生成新箱电子档案)
        /// </summary>
        /// <param name="data">要添加的数据</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool Add(ZPX_ConvertedCVTNumber data, out string error)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            if (Add(ctx, data, out error))
            {
                ctx.SubmitChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 添加维修记录(不仅变更箱号还根据旧箱电子档案生成新箱电子档案)
        /// </summary>
        /// <param name="data">要添加的数据</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool Add(DepotManagementDataContext ctx, ZPX_ConvertedCVTNumber data, out string error)
        {
            error = null;

            try
            {
                string oldCVTNumber = data.OldProductType + " " + data.OldProductNumber;
                string newCVTNumber = data.NewProductType + " " + data.NewProductNumber;

                if (!m_electronFileServer.IsExists(oldCVTNumber))
                {
                    error = "电子档案中找不到旧箱信息，检查操作是否有误，无法继续";
                    return false;
                }

                P_AssemblingBom oldBom = m_assemblingBom.GetRootNode(data.OldProductType);
                P_AssemblingBom newBom = m_assemblingBom.GetRootNode(data.NewProductType);

                #region Modify by cjb on 2016.10.9, reason:由于LINQ无法调用存储过程ZPX_ConvertElectronFile，为保证操作在同一事务里，故用代码实现存储过程中的功能

                string OldCode = oldCVTNumber;
                string OldName = oldBom.PartName;
                string NewCode = newCVTNumber;
                string NewName = newBom.PartName;

                var varData = from a in ctx.P_ElectronFile
                              where a.ProductCode == OldCode
                              select a;

                if (varData.Count() == 0)
                {
                    throw new Exception(string.Format("箱号{0}的变速箱电子档案已经存在，无法进行此操作!", OldCode));
                }

                foreach (P_ElectronFile item in varData)
                {
                    P_ElectronFile tempFile = item;
                    string NewType = NewCode.Substring(NewCode.IndexOf(' ')).Trim();

                    P_ElectronFile lnqInsert = new P_ElectronFile();

                    lnqInsert.ProductCode = NewCode;
                    lnqInsert.ParentCode = tempFile.ParentName == OldName ? NewType : tempFile.ParentCode;
                    lnqInsert.ParentName = tempFile.ParentName == OldName ? NewName : tempFile.ParentName;
                    lnqInsert.ParentScanCode = tempFile.ParentScanCode == OldCode ? NewCode : tempFile.ParentScanCode;
                    lnqInsert.GoodsCode = tempFile.GoodsName == OldName ? NewType : tempFile.GoodsCode;
                    lnqInsert.GoodsName = tempFile.GoodsName == OldName ? NewName : tempFile.GoodsName;
                    lnqInsert.Spec = tempFile.Spec;
                    lnqInsert.GoodsOnlyCode = tempFile.GoodsOnlyCode == OldCode ? NewCode : tempFile.GoodsOnlyCode;
                    lnqInsert.Counts = tempFile.Counts;
                    lnqInsert.Provider = tempFile.Provider;
                    lnqInsert.BatchNo = tempFile.BatchNo;
                    lnqInsert.WorkBench = tempFile.WorkBench;
                    lnqInsert.CheckDatas = tempFile.CheckDatas;
                    lnqInsert.FactDatas = tempFile.FactDatas;
                    lnqInsert.FittingPersonnel = tempFile.FittingPersonnel;
                    lnqInsert.FittingTime = ServerTime.Time.ToString("yyyy-MM-dd HH:mm:ss");
                    lnqInsert.FinishTime = ServerTime.Time.ToString("yyyy-MM-dd HH:mm:ss");
                    lnqInsert.FillInPersonnel = tempFile.FillInPersonnel;
                    lnqInsert.FillInDate = tempFile.FillInDate;
                    lnqInsert.AssemblingMode = tempFile.AssemblingMode;

                    if (lnqInsert.ParentCode == "")
                    {
                        lnqInsert.Remark = string.Format("[旧箱箱号：{0},{1}]; {2}", OldCode, ServerTime.Time.ToString("yyyy-MM-dd HH:mm:ss"), tempFile.Remark);
                    }
                    else
                    {
                        lnqInsert.Remark = string.Format("旧箱箱号：{0}; {1}", OldCode, tempFile.Remark);
                    }

                    ctx.P_ElectronFile.InsertOnSubmit(lnqInsert);
                }


                var varUpdate1 = from a in ctx.P_ElectronFile
                                 where a.ProductCode == OldCode && a.ParentCode == ""
                                 select a;

                foreach (P_ElectronFile item in varUpdate1)
                {
                    item.Remark += string.Format("[新箱箱号：{0},{1}]", NewCode, ServerTime.Time.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                #endregion

                //GlobalObject.DatabaseServer.QueryInfoPro("ZPX_ConvertElectronFile", hsTable, out error);

                //ctx.ZPX_ConvertElectronFile(oldCVTNumber, oldBom.PartName, newCVTNumber, newBom.PartName);

                //IDBOperate dbOperate = CommentParameter.GetDBOperate();

                //Hashtable paramTable = new Hashtable();

                //paramTable.Add("@OldCode", oldCVTNumber);
                //paramTable.Add("@OldName", oldBom.PartName);
                //paramTable.Add("@NewCode", newCVTNumber);
                //paramTable.Add("@NewName", newBom.PartName);

                //Dictionary<OperateCMD, object> dicOperateCMD = dbOperate.RunProc_CMD("ZPX_ConvertElectronFile", paramTable);

                //if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
                //{
                //    error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                //    return false;
                //}

                //DateTime serverTime = ServerTime.Time;

                //P_ElectronFile item = m_electronFileServer.CreateElectronFile(newCVTNumber);

                //item.GoodsCode = rootBom.PartCode;
                //item.GoodsName = rootBom.PartName;
                //item.GoodsOnlyCode = item.ProductCode;
                //item.WorkBench = rootBom.Workbench;
                //item.Remark = string.Format("旧箱箱号 {0}；", oldCVTNumber);

                //m_electronFileServer.AddElectronFile(ctx, item);

                //P_ElectronFile oldRoot = m_electronFileServer.GetRootNode(ctx, oldCVTNumber);

                //oldRoot.Remark += string.Format("新箱箱号 {0}；", newCVTNumber);

                ctx.ZPX_ConvertedCVTNumber.InsertOnSubmit(data);

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 批量变更变速箱箱号
        /// </summary>
        /// <param name="convertMode">变更模式, 手动模式：新箱箱号是操作人手工录入的；自动模式：新箱箱号由系统自动生成</param>
        /// <param name="data">要添加的数据</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool BatchConvertCVTNumber(string convertMode, List<ZPX_ConvertedCVTNumber> data, out string error)
        {
            error = null;

            if (data == null || data.Count == 0)
            {
                return true;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                DateTime serverTime = ServerTime.Time;

                if (convertMode == "手动模式")
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        data[i].NewProductType = data[i].NewProductType.Replace(" FX", "");
                    }

                    foreach (var item in data)
                    {
                        if (!Add(ctx, item, out error))
                        {
                            return false;
                        }
                    }
                    //ctx.ZPX_ConvertedCVTNumber.InsertAllOnSubmit(data);
                }
                else
                {
                    string productName = (from r in ctx.P_ProductInfo
                                          where r.ProductCode == data[0].NewProductType
                                          select r.ProductName).First();

                    BatchBarcodeInf batchBarcodeInf;

                    if (!PMS_ServerFactory.GetServerModule<IBarcodeForAssemblyLine>().BatchGenerateBarcode(
                        data[0].NewProductType, productName, data.Count, "[批量变更箱号]    ", out batchBarcodeInf, out error))
                    {
                        return false;
                    }

                    string[] arrayNewProductNumber = null;

                    foreach (var item in batchBarcodeInf.Barcodes)
                    {
                        arrayNewProductNumber = item.Value.Barcode;
                        break;
                    }

                    char[] separator = new char[] { ' ' };

                    for (int i = 0; i < data.Count; i++)
                    {
                        data[i].NewProductNumber = arrayNewProductNumber[i].Split(separator)[1];
                        data[i].NewProductType = data[i].NewProductType.Replace(" FX", "");

                        if (!Add(ctx, data[i], out error))
                        {
                            return false;
                        }
                    }

                    //ctx.ZPX_ConvertedCVTNumber.InsertAllOnSubmit(data);
                }

                // 删除移交记录
                for (int i = 0; i < data.Count; i++)
                {
                    var result = from r in ctx.ZPX_CVTHandoverInfo
                                 where r.ProductType == data[i].OldProductType && r.ProductNumber == data[i].OldProductNumber
                                 select r;

                    if (result.Count() > 0)
                    {
                        ctx.ZPX_CVTHandoverInfo.DeleteAllOnSubmit(result);
                    }
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

        /// <summary>
        /// 删除维修记录
        /// </summary>
        /// <param name="id">要删除数据的ID</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool Delete(int id, out string error)
        {
            error = null;

            DepotManagementDataContext context = CommentParameter.DepotDataContext;
            var data = from r in context.ZPX_ConvertedCVTNumber
                       where r.ID == id
                       select r;

            try
            {
                if (data.Count() == 0)
                {
                    error = "没有找到要删除的维修记录";
                    return false;
                }

                // 删除由本人刚才误操作所生成的电子档案数据
                var ef = from r in context.P_ElectronFile
                         where r.FillInPersonnel == BasicInfo.LoginID &&
                            r.FillInDate.Value.Date == ServerTime.Time.Date
                         select r;

                if (ef.Count() > 0)
                    context.P_ElectronFile.DeleteAllOnSubmit(ef);

                context.ZPX_ConvertedCVTNumber.DeleteAllOnSubmit(data);

                context.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }
    }
}
