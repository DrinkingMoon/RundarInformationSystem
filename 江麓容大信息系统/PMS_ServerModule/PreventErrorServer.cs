using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data;
using PMS_ServerModule;

namespace ServerModule
{
    class PreventErrorServer : IPreventErrorServer
    {
        #region 打扭矩防错

        /// <summary>
        /// 获得所有零件的打扭矩信息
        /// </summary>
        /// <returns>获取成功返回dt，获取失败返回null</returns>
        public DataTable GetAllTorqueSpannerInfo()
        {
            string sql = "select * from View_ZPX_TorqueSpanner";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 修改打扭矩零件的屏蔽时间
        /// </summary>
        /// <param name="workBench">工位</param>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="startTime">屏蔽开始时间</param>
        /// <param name="endTime">屏蔽终止时间</param>
        /// <param name="error">错误信息</param>
        /// <returns>修改成功返回true，失败返回False</returns>
        public bool UpdateShieldTime(string workBench,string goodsCode,string goodsName,
                                     string spec,DateTime startTime, DateTime endTime, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.ZPX_TorqueSpanner
                             where a.WorkBench == workBench && a.GoodsCode == goodsCode
                             && a.GoodsName == goodsName && a.Spec == spec
                             select a;

                if (result.Count() == 1)
                {
                    ZPX_TorqueSpanner torque = result.Single();

                    torque.StartTime = startTime;
                    torque.EndTime = endTime;
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
        /// 添加零件打扭矩的信息
        /// </summary>
        /// <param name="torqueSpanner">打扭矩数据对象</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        public bool InsertTorqueSpanner(ZPX_TorqueSpanner torqueSpanner,out string error)
        {
            error = "";

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from a in dataContxt.ZPX_TorqueSpanner
                             where a.WorkBench == torqueSpanner.WorkBench
                             && a.GoodsCode == torqueSpanner.GoodsCode
                             && a.GoodsName == torqueSpanner.GoodsName
                             && a.Spec == torqueSpanner.Spec
                             select a;

                if (result.Count() == 0)
                {
                    dataContxt.ZPX_TorqueSpanner.InsertOnSubmit(torqueSpanner);
                }
                else
                {
                    error = "数据重复，请检查！";
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
        /// 修改零件打扭矩的信息
        /// </summary>
        /// <param name="torqueSpanner">打扭矩数据对象</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        public bool UpdateTorqueSpanner(ZPX_TorqueSpanner torqueSpanner, out string error)
        {
            error = "";

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from a in dataContxt.ZPX_TorqueSpanner
                             where a.WorkBench == torqueSpanner.WorkBench
                             && a.GoodsCode == torqueSpanner.GoodsCode
                             && a.GoodsName == torqueSpanner.GoodsName
                             && a.Spec == torqueSpanner.Spec
                             select a;

                if (result.Count() != 1)
                {
                    error = "数据错误，请检查！";
                    return false;
                }
                else
                {
                    ZPX_TorqueSpanner lnqTorque = result.Single();

                    lnqTorque.ProductType = torqueSpanner.ProductType;
                    lnqTorque.Seconds = torqueSpanner.Seconds;
                    lnqTorque.CommPort = torqueSpanner.CommPort;
                    lnqTorque.GoodsWorkBench = torqueSpanner.GoodsWorkBench;
                    lnqTorque.IsOtherWorkBenchPart = torqueSpanner.IsOtherWorkBenchPart;
                    lnqTorque.CommMode = torqueSpanner.CommMode;
                    lnqTorque.TorqueCount = torqueSpanner.TorqueCount;
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
        /// 删除零件打扭矩的信息
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        public bool DeleteTorqueSpanner(int id, out string error)
        {
            error = "";

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from a in dataContxt.ZPX_TorqueSpanner
                             where a.ID == id
                             select a;

                if (result.Count() != 1)
                {
                    error = "数据错误，请检查！";
                    return false;
                }
                else
                {
                    dataContxt.ZPX_TorqueSpanner.DeleteAllOnSubmit(result);
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
        /// 判断零件是否在打扭矩的工位上
        /// </summary>
        /// <param name="workBench">工位</param>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <returns>在返回true不在返回false</returns>
        public bool IsWorkBenchGoods(string workBench,string goodsCode,string goodsName,string spec)
        {
            string strSql = "select count(*) from DepotManagement.dbo.P_AssemblingBom " +
                            " where Workbench = '" + workBench + "' and PartCode = '" + goodsCode + "'" +
                            " and PartName = '" + goodsName + "' and Spec = '" + spec + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region 电子称

        /// <summary>
        /// 获得所有零件的电子称防错信息
        /// </summary>
        /// <returns>获取成功返回dt，获取失败返回null</returns>
        public DataTable GetAllLeakproofPartsInfo()
        {
            string sql = "select * from PlatformService.dbo.ZPX_LeakproofParts";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        /// <summary>
        /// 修改电子称零件的屏蔽时间
        /// </summary>
        /// <param name="workBench">工位</param>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="starTime">屏蔽开始时间</param>
        /// <param name="endTime">屏蔽终止时间</param>
        /// <param name="error">错误信息</param>
        /// <returns>修改成功返回true，失败返回False</returns>
        public bool UpdateLeakproofTime(string workBench, string goodsCode, string goodsName,
                                    string spec, DateTime starTime, DateTime endTime,  out string error)
        {
            error = "";

            try
            {
                PlatformServiceDataContext dataContxt = ParameterFactory.PlatformDataContext;

                var result = from a in dataContxt.ZPX_LeakproofParts
                             where a.工位 == workBench && a.防漏装零件图号 == goodsCode
                             && a.防漏装零件名称 == goodsName && a.防漏装零件规格 == spec
                             select a;

                if (result.Count() == 1)
                {
                    ZPX_LeakproofParts leakproof = result.Single();

                    leakproof.屏蔽开始时间 = starTime;
                    leakproof.屏蔽终止时间 = endTime;
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
        /// 添加零件电子称防错的信息
        /// </summary>
        /// <param name="leakproofParts">电子称防错数据对象</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        public bool InsertLeakproofParts(ZPX_LeakproofParts leakproofParts, out string error)
        {
            error = "";

            PlatformServiceDataContext dataContxt = ParameterFactory.PlatformDataContext;

            try
            {
                var result = from a in dataContxt.ZPX_LeakproofParts
                             where a.工位 == leakproofParts.工位
                             && a.防漏装零件图号 == leakproofParts.防漏装零件图号
                             && a.防漏装零件名称 == leakproofParts.防漏装零件名称
                             && a.防漏装零件规格 == leakproofParts.防漏装零件规格
                             select a;

                if (result.Count() == 0)
                {
                    dataContxt.ZPX_LeakproofParts.InsertOnSubmit(leakproofParts);
                }
                else
                {
                    error = "数据重复，请检查！";
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
        /// 修改零件打扭矩的信息
        /// </summary>
        /// <param name="leakproofParts">电子称防错数据对象</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        public bool UpdateLeakproofParts(ZPX_LeakproofParts leakproofParts, out string error)
        {
            error = "";

            PlatformServiceDataContext dataContxt = ParameterFactory.PlatformDataContext;

            try
            {
                var result = from a in dataContxt.ZPX_LeakproofParts
                             where a.工位 == leakproofParts.工位
                             && a.防漏装零件图号 == leakproofParts.防漏装零件图号
                             && a.防漏装零件名称 == leakproofParts.防漏装零件名称
                             && a.防漏装零件规格 == leakproofParts.防漏装零件规格
                             select a;

                if (result.Count() != 1)
                {
                    error = "数据错误，请检查！";
                    return false;
                }
                else
                {
                    ZPX_LeakproofParts lnqLeakproof = result.Single();

                    lnqLeakproof.电子秤端口号 = leakproofParts.电子秤端口号;
                    lnqLeakproof.防错模式 = leakproofParts.防错模式;
                    lnqLeakproof.零件单重 = leakproofParts.零件单重;
                    lnqLeakproof.公差 = leakproofParts.公差;
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
        /// 删除零件电子称防错的信息
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        public bool DeleteLeakproofParts(int id, out string error)
        {
            error = "";

            PlatformServiceDataContext dataContxt = ParameterFactory.PlatformDataContext;

            try
            {
                var result = from a in dataContxt.ZPX_LeakproofParts
                             where a.ID == id
                             select a;

                if (result.Count() != 1)
                {
                    error = "数据错误，请检查！";
                    return false;
                }
                else
                {
                    dataContxt.ZPX_LeakproofParts.DeleteAllOnSubmit(result);
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
        #endregion

        #region 装配顺序及工位间装配顺序

        /// <summary>
        /// 获得所有零件的装配顺序防错信息
        /// </summary>
        /// <returns>获取成功返回dt，获取失败返回null</returns>
        public DataTable GetAllWorkbenchConfig()
        {
            string sql = "select * from PlatformService.dbo.ZPX_ProductWorkbenchConfig";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 修改装配顺序及工位间装配顺序的屏蔽时间
        /// </summary>
        /// <param name="workBench">当前工位</param>
        /// <param name="upWorkBench">上道工位</param>
        /// <param name="productCode">产品类型</param>
        /// <param name="name">分总成名称</param>
        /// <param name="starTime">屏蔽开始时间</param>
        /// <param name="endTime">屏蔽终止时间</param>
        /// <param name="error">错误信息</param>
        /// <returns>修改成功返回true，失败返回False</returns>
        public bool UpdateProductWorkbenchTime(string workBench, string upWorkBench, string productCode,
                                     string name,DateTime starTime, DateTime endTime,  out string error)
        {
            error = "";

            try
            {
                PlatformServiceDataContext dataContxt = ParameterFactory.PlatformDataContext;

                var result = from a in dataContxt.ZPX_ProductWorkbenchConfig
                             where a.当前工位 == workBench && a.上道工位 == upWorkBench
                             && a.产品类型 == productCode && a.分总成名称 == name
                             select a;

                if (result.Count() == 1)
                {
                    ZPX_ProductWorkbenchConfig product = result.Single();

                    product.屏蔽开始时间 = starTime;
                    product.屏蔽终止时间 = endTime;
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
        /// 获得分工位总成
        /// </summary>
        /// <param name="queryAssemblingBom">操作后查询返回的产品信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>获取成功返回True，获取失败返回False</returns>
        public bool GetAssemblingBom(
            out IQueryable<P_AssemblingBom> queryAssemblingBom, out string error)
        {
            error = "";

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                Table<P_AssemblingBom> table = dataContxt.GetTable<P_AssemblingBom>();

                queryAssemblingBom = (from c in table
                                      where c.ParentName != null
                                      orderby c.ParentName
                                      select c).Distinct();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                queryAssemblingBom = null;

                return false;
            }
        }

        /// <summary>
        /// 添加装配顺序防错的信息
        /// </summary>
        /// <param name="workbenchConfig">装配顺序防错数据对象</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        public bool InsertWorkbenchConfig(ZPX_ProductWorkbenchConfig workbenchConfig, out string error)
        {
            error = "";

            PlatformServiceDataContext dataContxt = ParameterFactory.PlatformDataContext;

            try
            {
                var result = from a in dataContxt.ZPX_ProductWorkbenchConfig
                             where a.产品类型 == workbenchConfig.产品类型
                             && a.当前工位 == workbenchConfig.当前工位
                             select a;

                if (result.Count() == 0)
                {
                    dataContxt.ZPX_ProductWorkbenchConfig.InsertOnSubmit(workbenchConfig);
                }
                else
                {
                    error = "数据重复，请检查！";
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
        /// 修改装配顺序防错的信息
        /// </summary>
        /// <param name="workbenchConfig">装配顺序防错数据对象</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        public bool UpdateWorkbenchConfig(ZPX_ProductWorkbenchConfig workbenchConfig, out string error)
        {
            error = "";

            PlatformServiceDataContext dataContxt = ParameterFactory.PlatformDataContext;

            try
            {
                var result = from a in dataContxt.ZPX_ProductWorkbenchConfig
                             where a.当前工位 == workbenchConfig.当前工位
                             && a.产品类型 == workbenchConfig.产品类型
                             select a;

                if (result.Count() != 1)
                {
                    error = "数据错误，请检查！";
                    return false;
                }
                else
                {
                    ZPX_ProductWorkbenchConfig lnqWorkbenchConfig = result.Single();

                    lnqWorkbenchConfig.按序装配末道工序值 = workbenchConfig.按序装配末道工序值;
                    lnqWorkbenchConfig.按序装配端口号 = workbenchConfig.按序装配端口号;
                    lnqWorkbenchConfig.上道工位 = workbenchConfig.上道工位;
                    lnqWorkbenchConfig.是按顺序流水号装配 = workbenchConfig.是按顺序流水号装配;
                    lnqWorkbenchConfig.是本阶段末工位 = workbenchConfig.是本阶段末工位;
                    lnqWorkbenchConfig.是本阶段起始工位 = workbenchConfig.是本阶段起始工位;
                    lnqWorkbenchConfig.是气密性检测工位 = workbenchConfig.是气密性检测工位;
                    lnqWorkbenchConfig.是所有产品通用工位 = workbenchConfig.是所有产品通用工位;
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
        /// 删除装配顺序防错的信息
        /// </summary>
        /// <param name="product">产品类型</param>
        /// <param name="bench">当前工位</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        public bool DeleteWorkbenchConfig(string product, string bench, out string error)
        {
            error = "";

            PlatformServiceDataContext dataContxt = ParameterFactory.PlatformDataContext;

            try
            {
                var result = from a in dataContxt.ZPX_ProductWorkbenchConfig
                             where a.产品类型 == product && a.当前工位 == bench
                             select a;

                if (result.Count() != 1)
                {
                    error = "数据错误，请检查！";
                    return false;
                }
                else
                {
                    dataContxt.ZPX_ProductWorkbenchConfig.DeleteAllOnSubmit(result);
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

        #endregion

        #region CCD拍照

        /// <summary>
        /// 获得所有零件的CCD拍照防错信息
        /// </summary>
        /// <returns>获取成功返回dt，获取失败返回null</returns>
        public DataTable GetAllCCDConfig()
        {
            string sql = "select * from PlatformService.dbo.ZPX_CCDConfig";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        /// <summary>
        /// 添加CCD拍照防错的信息
        /// </summary>
        /// <param name="config">装配顺序防错数据对象</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        public bool InsertCCDConfig(ZPX_CCDConfig config, out string error)
        {
            error = "";

            PlatformServiceDataContext dataContxt = ParameterFactory.PlatformDataContext;

            try
            {
                var result = from a in dataContxt.ZPX_CCDConfig
                             where a.工位 == config.工位
                             && a.零件图号 == config.零件图号 && a.零件名称 == config.零件名称
                             && a.零件规格 == config.零件规格
                             select a;

                if (result.Count() == 0)
                {
                    dataContxt.ZPX_CCDConfig.InsertOnSubmit(config);
                }
                else
                {
                    error = "数据重复，请检查！";
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
        /// 修改CCD拍照防错的信息
        /// </summary>
        /// <param name="config">CCD拍照防错数据对象</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        public bool UpdateCCDConfig(ZPX_CCDConfig config, out string error)
        {
            error = "";

            PlatformServiceDataContext dataContxt = ParameterFactory.PlatformDataContext;

            try
            {
                var result = from a in dataContxt.ZPX_CCDConfig
                             where a.工位 == config.工位
                             && a.零件图号 == config.零件图号 && a.零件名称 == config.零件名称
                             && a.零件规格 == config.零件规格
                             select a;

                if (result.Count() != 1)
                {
                    error = "数据错误，请检查！";
                    return false;
                }
                else
                {
                    ZPX_CCDConfig lnqCCD = result.Single();

                    lnqCCD.测点号 = config.测点号;
                    lnqCCD.适用产品类型 = config.适用产品类型;
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
        /// 删除CCD拍照防错的信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="bench">工位</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        public bool DeleteCCDConfig(string goodsCode,string goodsName,string spec, string bench, out string error)
        {
            error = "";

            PlatformServiceDataContext dataContxt = ParameterFactory.PlatformDataContext;

            try
            {
                var result = from a in dataContxt.ZPX_CCDConfig
                             where a.工位 == bench && a.零件图号 == goodsCode
                             && a.零件名称 == goodsName && a.零件规格 == spec
                             select a;

                if (result.Count() != 1)
                {
                    error = "数据错误，请检查！";
                    return false;
                }
                else
                {
                    dataContxt.ZPX_CCDConfig.DeleteAllOnSubmit(result);
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
        /// 修改CCD零件的屏蔽时间
        /// </summary>
        /// <param name="workBench">工位</param>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="starTime">屏蔽开始时间</param>
        /// <param name="endTime">屏蔽终止时间</param>
        /// <param name="error">错误信息</param>
        /// <returns>修改成功返回true，失败返回False</returns>
        public bool UpdateCCDConfigTime(string workBench, string goodsCode, string goodsName,
                                    string spec, DateTime starTime, DateTime endTime, out string error)
        {
            error = "";

            try
            {
                PlatformServiceDataContext dataContxt = ParameterFactory.PlatformDataContext;

                var result = from a in dataContxt.ZPX_CCDConfig
                             where a.工位 == workBench && a.零件图号 == goodsCode
                             && a.零件名称 == goodsName && a.零件规格 == spec
                             select a;

                if (result.Count() == 1)
                {
                    ZPX_CCDConfig CCDConfig = result.Single();

                    CCDConfig.屏蔽开始时间 = starTime;
                    CCDConfig.屏蔽终止时间 = endTime;
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

        #endregion

        /// <summary>
        /// 获取分总成信息
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns>获取成功返回True，获取失败返回False</returns>
        public DataTable GetAllAssemblingBom(string productType)
        {
            string sql = "select distinct 父总成名称 分总成名称 from View_P_AssemblingBom" +
                         " where (父总成编码 is not null) AND (父总成编码 <> '') AND 父总成编码 not in (" +
                         " select ProductType from dbo.P_ProductInfo)";

            if (productType != "")
            {
                sql += " and 产品编码 in ('" + productType + "')";
            }

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 判断分总成是否在该总成之下
        /// </summary>
        /// <param name="assemble">分总成</param>
        /// <param name="productType">产品类型</param>
        /// <returns>获取成功返回True，获取失败返回False</returns>
        public bool IsAssemblingBom(string assemble, string productType)
        {
            string sql = "select * from View_P_AssemblingBom where 父总成名称='" + assemble + "'" +
                         " and 产品编码='" + productType + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
