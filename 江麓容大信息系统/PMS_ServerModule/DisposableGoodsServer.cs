using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerModule;
using System.Data.Linq;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 一次性物料清单服务类
    /// </summary>
    public class DisposableGoodsServer : ServerModule.IDisposableGoodsServer
    {
        /// <summary>
        /// 获取一次性物料信息
        /// </summary>
        /// <param name="returnInfo">操作后查询返回的产品信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>获取成功返回True，获取失败返回False</returns>
        public bool GetAllDataInfo(out IQueryable<View_ZPX_DisposableGoods> returnInfo, out string error)
        {
            error = "";

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                Table<View_ZPX_DisposableGoods> table = dataContxt.GetTable<View_ZPX_DisposableGoods>();

                returnInfo = from c in table
                                    select c;

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                returnInfo = null;
                return false;
            }
        }

        /// <summary>
        /// 通过产品型号返回物料信息
        /// </summary>
        /// <param name="productType">产品型号</param>
        /// <returns>成功返回View_ZPX_DisposableGoods数据集，否则返回NULL</returns>
        public IQueryable<View_ZPX_DisposableGoods> GetDataByProductType(string productType)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from a in dataContxt.View_ZPX_DisposableGoods
                             where a.产品型号 == productType
                             select a;

                if (result.Count() > 0)
                {
                    return result;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 添加一条信息
        /// </summary>
        /// <param name="disposeGoods">一次性物料数据</param>
        /// <param name="error">错误信息</param>
        /// <returns>添加成功返回True，失败返回False</returns>
        public bool InsertData(ZPX_DisposableGoods disposeGoods,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.ZPX_DisposableGoods
                             where a.ProductType == disposeGoods.ProductType && a.GoodsCode == disposeGoods.GoodsCode
                             && a.GoodsName == disposeGoods.GoodsName && a.Spec == disposeGoods.Spec
                             select a;

                if (result.Count() > 0)
                {
                    error = disposeGoods.ProductType + "产品的" + disposeGoods.GoodsCode + "零件已经存在。";
                    return false;
                }
                else
                {
                    dataContxt.ZPX_DisposableGoods.InsertOnSubmit(disposeGoods);
                    dataContxt.SubmitChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 批量添加物料信息
        /// </summary>
        /// <param name="copyProductType">复制的产品型号</param>
        /// <param name="productType">复制给该产品的产品型号</param>
        /// <param name="error">错误信息</param>
        /// <returns>添加成功返回True，失败返回False</returns>
        public bool InsertBatchData(string copyProductType, string productType, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.ZPX_DisposableGoods
                             where a.ProductType == productType
                             select a;

                if (result.Count() > 0)
                {
                    dataContxt.ZPX_DisposableGoods.DeleteAllOnSubmit(result);
                }

                var resultCopy = from a in dataContxt.ZPX_DisposableGoods
                             where a.ProductType == copyProductType
                             select a;

                if (resultCopy.Count() > 0)
                {
                    foreach (ZPX_DisposableGoods item in resultCopy)
                    {
                        ZPX_DisposableGoods dispozeGoods = new ZPX_DisposableGoods();

                        dispozeGoods.ProductType = productType;
                        dispozeGoods.Count = item.Count;
                        dispozeGoods.Date = ServerTime.Time;
                        dispozeGoods.GoodsCode = item.GoodsCode;
                        dispozeGoods.GoodsName = item.GoodsName;
                        dispozeGoods.Spec = item.Spec;
                        dispozeGoods.UserCode = BasicInfo.LoginID;

                        dataContxt.ZPX_DisposableGoods.InsertOnSubmit(dispozeGoods);
                    }
                }
                else
                {
                    error = "复制的产品中没有物料信息！";
                    return false;
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除一条信息
        /// </summary>
        /// <param name="disposeGoods">一次性物料数据</param>
        /// <param name="error">错误信息</param>
        /// <returns>添加成功返回True，失败返回False</returns>
        public bool DeleteData(ZPX_DisposableGoods disposeGoods, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.ZPX_DisposableGoods
                             where a.ProductType == disposeGoods.ProductType && a.GoodsCode == disposeGoods.GoodsCode
                             && a.GoodsName == disposeGoods.GoodsName && a.Spec == disposeGoods.Spec
                             select a;

                if (result.Count() > 0)
                {
                    dataContxt.ZPX_DisposableGoods.DeleteAllOnSubmit(result);
                    dataContxt.SubmitChanges();
                }
                else
                {
                    error = "数据信息有误！";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }
}
