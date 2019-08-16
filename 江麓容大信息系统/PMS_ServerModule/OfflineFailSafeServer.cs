using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data;
using GlobalObject;

namespace ServerModule
{
    public class OfflineFailSafeServer : ServerModule.IOfflineFailSafeServer
    {
        /// <summary>
        /// 设置出错返回值
        /// </summary>
        /// <param name="err">传入的错误信息</param>
        /// <param name="returnProductInfo">返回的信息</param>
        /// <param name="error">返回的错误信息</param>
        /// <returns>始终返回False</returns>
        bool SetReturnError(object err, out IQueryable<View_ZPX_OfflineFailsafe> returnProductInfo, out string error)
        {
            returnProductInfo = null;
            error = err.ToString();

            return false;
        }

        /// <summary>
        /// 获取全部单据信息
        /// </summary>
        /// <returns>返回下线返修扭矩防错信息</returns>
        public List<View_ZPX_OfflineFailsafe> GetAllBill()
        {
            string strSql = "select * from View_ZPX_OfflineFailsafe order by 产品型号,返修类型,分装总成,阶段,防错顺序";
             
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return dataContxt.ExecuteQuery<View_ZPX_OfflineFailsafe>(strSql, new object[] { }).ToList();
        }

        /// <summary>
        /// 通过产品型号获取单据信息
        /// </summary>
        /// <param name="productType">产品型号</param>
        /// <returns>返回下线返修扭矩防错信息</returns>
        public List<View_ZPX_OfflineFailsafe> GetBillByProductType(string productType)
        {
            string strSql = "select * from View_ZPX_OfflineFailsafe ";

            if (productType != "全部")
            {
                strSql += " where 产品型号='" + productType + "'";
            }

            strSql += " order by 产品型号,返修类型,分装总成,阶段,防错顺序";
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return dataContxt.ExecuteQuery<View_ZPX_OfflineFailsafe>(strSql, new object[] { }).ToList();
        }

        /// <summary>
        /// 获得最大的防错顺序
        /// </summary>
        /// <param name="type">返修类型</param>
        /// <param name="product">产品型号</param>
        /// <param name="partName">分装总成</param>
        /// <param name="phase">阶段</param>
        /// <returns>返回最大的防错顺序整形值</returns>
        public int GetOrderNoMax(string type,string product,string partName,string phase)
        {
            string sql = "select max(防错顺序) as 防错顺序 from View_ZPX_OfflineFailsafe where 返修类型='" + type + "'" +
                         " and 产品型号 ='" + product + "' and 分装总成='" + partName + "' and 阶段='" + phase + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt == null)
            {
                return -1;
            }
            else
            {
                return Convert.ToInt32(dt.Rows[0]["防错顺序"] == DBNull.Value ? "0" : dt.Rows[0]["防错顺序"]);
            }
        }

        /// <summary>
        /// 添加扭矩防错信息
        /// </summary>
        /// <param name="failSafe">扭矩防错</param>
        /// <param name="error">错误信息</param>
        /// <returns>添加成功返回True失败返回False</returns>
        public bool InsertFailsafe(ZPX_OfflineFailsafe failSafe, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.ZPX_OfflineFailsafe
                             where a.ProductType == failSafe.ProductType
                             && a.GoodsCode == failSafe.GoodsCode && a.GoodsName == a.GoodsName
                             && a.Spec == failSafe.Spec && a.PositionMessage == failSafe.PositionMessage
                             && a.SourceProductType == failSafe.SourceProductType
                             && a.Phase == failSafe.Phase
                             select a;

                if (result.Count() == 0)
                {
                    dataContxt.ZPX_OfflineFailsafe.InsertOnSubmit(failSafe);
                }
                else
                {
                    dataContxt.ZPX_OfflineFailsafe.DeleteAllOnSubmit(result);
                    dataContxt.ZPX_OfflineFailsafe.InsertOnSubmit(failSafe);
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
        /// 修改扭矩防错信息
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="failSafe">扭矩防错</param>
        /// <param name="error">错误信息</param>
        /// <returns>添加成功返回True失败返回False</returns>
        public bool UpdateFailsafe(int id,ZPX_OfflineFailsafe failSafe, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.ZPX_OfflineFailsafe
                             where a.ID == id
                             select a;

                dataContxt.ZPX_OfflineFailsafe.DeleteAllOnSubmit(result);
                dataContxt.ZPX_OfflineFailsafe.InsertOnSubmit(failSafe);

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
        /// 删除扭矩防错信息
        /// </summary>
        /// <param name="productType">产品型号</param>
        /// <param name="parentName">分总成名称</param>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="phase">阶段</param>
        /// <param name="positionMessage">位置信息</param>
        /// <param name="error">错误信息呢</param>
        /// <returns>删除成功返回true失败返回False</returns>
        public bool DeleteFailSafe(string productType,string parentName,string goodsCode,
            string goodsName,string spec,string phase,string positionMessage,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.ZPX_OfflineFailsafe
                             where a.ProductType == productType && a.SourceProductType == parentName
                             && a.GoodsCode == goodsCode && a.GoodsName == goodsName
                             && a.Spec == spec && a.Phase == phase && a.PositionMessage == positionMessage
                             select a;

                if (result.Count() != 1)
                {
                    error = "数据有误，请检查";
                    return false;
                }
                else
                {
                    dataContxt.ZPX_OfflineFailsafe.DeleteAllOnSubmit(result);
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
        /// 保存防错顺序
        /// </summary>
        /// <param name="lstData">要保存的列表数据</param>
        public void SaveOrderNo(List<View_ZPX_OfflineFailsafe> lstData)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            int index = 0;
            string[] workbenchs = (from r in lstData select r.返修类型).Distinct().ToArray();
            string partName = "";
            string phase = "";

            foreach (var item in lstData)
            {
                var result = from r in dataContxt.ZPX_OfflineFailsafe
                             where r.ProductType == item.产品型号 && r.Type == item.返修类型
                             && r.GoodsCode == item.图号型号 && r.GoodsName == item.物品名称
                             && r.PositionMessage == item.位置信息
                             && r.Spec == item.规格 && r.Phase == item.阶段 && r.SourceProductType == item.分装总成
                             select r;

                if (partName != item.分装总成 || phase != item.阶段)
                {
                    index = 0;
                    index++;
                    partName = item.分装总成;
                    phase = item.阶段;
                    result.Single().OrderNo = index;
                }
                else
                {
                    index++;
                    result.Single().OrderNo = index;
                }
            }

            dataContxt.SubmitChanges();
        }

        /// <summary>
        /// 获取阶段信息
        /// </summary>
        /// <returns>返回阶段信息</returns>
        public DataTable GetPhase()
        {
            string strSql = "select distinct Phase from dbo.ZPX_OfflinePhaseSet";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获取全部阶段信息
        /// </summary>
        /// <returns>返回阶段信息</returns>
        public List<ZPX_OfflinePhaseSet> GetAllPhase()
        {
            string strSql = "select * from dbo.ZPX_OfflinePhaseSet";

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return dataContxt.ExecuteQuery<ZPX_OfflinePhaseSet>(strSql, new object[] { }).ToList();
        }

        /// <summary>
        /// 添加阶段信息
        /// </summary>
        /// <param name="phaseSet">阶段信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>添加成功返回True失败返回False</returns>
        public bool InsertPhase(ZPX_OfflinePhaseSet phaseSet,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.ZPX_OfflinePhaseSet
                             where a.Phase == phaseSet.Phase && a.Contain == phaseSet.Contain
                             select a;

                if (result.Count() == 0)
                {                    
                    dataContxt.ZPX_OfflinePhaseSet.InsertOnSubmit(phaseSet);
                }
                else
                {
                    ZPX_OfflinePhaseSet phase = result.Single();

                    phase.Contain = phaseSet.Contain;
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
        /// 删除阶段信息
        /// </summary>
        /// <param name="phase">阶段</param>
        /// <param name="error">错误信息</param>
        /// <returns>删除成功返回true失败返回false</returns>
        public bool DeletePhase(string phase,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.ZPX_OfflinePhaseSet
                             where a.Phase == phase
                             select a;

                if (result.Count() > 0)
                {
                    var resultList = from e in dataContxt.ZPX_OfflinePhaseSet
                                     where e.Contain.Contains(phase)
                                     select e;

                    if (resultList.Count() > 0)
                    {
                        error = "其他阶段包含此阶段，不能删除！";
                        return false;
                    }

                    dataContxt.ZPX_OfflinePhaseSet.DeleteAllOnSubmit(result);
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
        /// 复制指定的产品型号信息到目标产品
        /// </summary>
        /// <param name="surProductType">源产品类型</param>
        /// <param name="tarProductType">目标产品类型</param>
        /// <param name="phase">阶段</param>
        /// <param name="error">操作错误时返回的错误信息</param>
        /// <returns>复制成功返回True，复制失败返回False</returns>
        public bool CopyFailsafe(string surProductType, string tarProductType, string phase, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var source = from r in ctx.ZPX_OfflineFailsafe
                             where r.ProductType == surProductType
                             && (r.Phase == phase || r.SourceProductType == phase)
                             select r;

                if (source.Count() == 0)
                {
                    error = string.Format("{0} 的 {1} 阶段不存在扭矩防错零件信息，无法进行此操作", surProductType, phase);
                    return false;
                }

                var target = from r in ctx.ZPX_OfflineFailsafe
                             where r.ProductType == tarProductType
                             && (r.Phase == phase || r.SourceProductType == phase)
                             select r;

                if (target.Count() > 0)
                {
                    error = string.Format("{0} 的 {1} 阶段存在扭矩防错零件信息，无法进行此操作", tarProductType, phase);
                    return false;
                }

                string login = BasicInfo.LoginID;
                DateTime time = ServerTime.Time;

                foreach (var item in source)
                {
                    ZPX_OfflineFailsafe targetFailsafe = new ZPX_OfflineFailsafe();

                    targetFailsafe.OrderNo = item.OrderNo;
                    targetFailsafe.GoodsCode = item.GoodsCode;
                    targetFailsafe.GoodsName = item.GoodsName;
                    targetFailsafe.IsDistinguish = item.IsDistinguish;
                    targetFailsafe.Message = item.Message;
                    targetFailsafe.NumCount = item.NumCount;
                    targetFailsafe.Phase = item.Phase;
                    targetFailsafe.PositionMessage = item.PositionMessage;
                    targetFailsafe.ProductType = tarProductType;
                    targetFailsafe.Recorder = login;
                    targetFailsafe.RecordTime = time;
                    targetFailsafe.SourceProductType = item.SourceProductType;
                    targetFailsafe.Spec = item.Spec;
                    targetFailsafe.Torque = item.Torque;
                    targetFailsafe.Type = item.Type;

                    ctx.ZPX_OfflineFailsafe.InsertOnSubmit(targetFailsafe);
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
