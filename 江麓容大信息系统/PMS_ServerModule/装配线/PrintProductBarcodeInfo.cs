using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 记录装配用打印产品条形码的信息类，防止装配车间与下线车间箱号相同的现象
    /// </summary>
    class PrintProductBarcodeInfo : ServerModule.IPrintProductBarcodeInfo
    {
        /// <summary>
        /// 检查箱号是否存在
        /// </summary>
        /// <param name="productNumber">箱号</param>
        /// <returns>存在返回true</returns>
        public bool IsExists(string productNumber)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.View_ZPX_PrintProductBarcode
                         where r.变速箱号 == productNumber
                         select r;

            return result.Count() > 0;
        }

        /// <summary>
        /// 检查指定的变速箱号是否允许打印
        /// </summary>
        /// <param name="productNumber">变速箱号</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>操作是否成功的标志，成功返回true，失败返回false</returns>
        public bool AllowPrint(string productNumber, out string error)
        {
            error = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                var result = from r in ctx.View_ZPX_PrintProductBarcode
                             where r.变速箱号 == productNumber
                             select r;

                if (result.Count() > 0)
                {
                    View_ZPX_PrintProductBarcode info = result.Single();

                    //int len = BasicInfo.DeptCode.Length > info.部门编码.Length ? info.部门编码.Length : BasicInfo.DeptCode.Length;
                    int len = BasicInfo.DeptCode.Length > 4 ? 4 : BasicInfo.DeptCode.Length;

                    if (len > info.部门编码.Length)
                        len = info.部门编码.Length;

                    if (BasicInfo.DeptCode.Substring(0, len) != info.部门编码.Substring(0, len))
                    {
                        error = string.Format("{0} 已经由 {1} 的 {2} 于 {3} 完成打印，不允许跨车间重复打印", info.变速箱号, info.部门名称, info.姓名, info.日期);

                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 添加产品条形码打印信息
        /// </summary>
        /// <param name="productNumber">变速箱号</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>操作是否成功的标志，成功返回true，失败返回false</returns>
        public bool Add(string productNumber, out string error)
        {
            error = null;
            //if (!AllowPrint(productNumber, out error))
            //{
            //    return false;
            //}

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var result = from r in ctx.ZPX_PrintProductBarcode
                             where r.ProductBarcode == productNumber
                             select r;

                if (result.Count() == 0)
                {
                    ZPX_PrintProductBarcode data = new ZPX_PrintProductBarcode();

                    data.ProductBarcode = productNumber;
                    data.UserCode = BasicInfo.LoginID;
                    data.Date = ServerTime.Time;
                    data.Remark = "[打印装配条形码]    ";

                    ctx.ZPX_PrintProductBarcode.InsertOnSubmit(data);
                }
                else
                {
                    result.Single().Remark += string.Format("[{0} 于 {1} 打印]    ", BasicInfo.LoginID, ServerTime.Time.ToString("yyyy-MM-dd HH:mm:ss"));
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
    }
}
