
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GlobalObject;
using PlatformManagement;

namespace ServerModule
{
    /// <summary>
    /// 技术变更单管理类
    /// </summary>
    class TechnologyAlteration : BasicServer, ServerModule.ITechnologyAlteration
    {
        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgator = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.S_TechnologyAlterationBill
                          where a.BillNo == billNo
                          select a;

            if (varData == null || varData.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 检查某单据是否存在
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(string billNo)
        {
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_TechnologyAlterationBill] where billNo = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得单条记录
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>返回获得的单据信息</returns>
        public DataRow GetBillInfo(string djh)
        {
            string strSql = "select * from S_TechnologyAlterationBill where BillNo = '" + djh + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 通过单号获得明细
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>返回获得的单据信息</returns>
        public DataTable GetListInfo(string djh)
        {
            string strSql = "select * from View_S_TechnologyAlterationList where 单据号 = '" + djh + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得全部单据
        /// </summary>
        /// <param name="billStatus">单据状态</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回获得的全部单据信息</returns>
        public DataTable GetAllBill(string billStatus, DateTime startTime, DateTime endTime)
        {
            string strSql = "select * from View_S_TechnologyAlterationBill " +
                " where 申请日期 >= '" + startTime + "' and 申请日期 <= '" + endTime + "'";

            if (billStatus != "全部")
            {
                strSql += " and 单据状态 = '" + billStatus + "'";
            }

            strSql += " ORDER BY 单据号 DESC";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得同名称同型号同规格在BOM表中的记录
        /// </summary>
        /// <param name="code">图号型号</param>
        /// <param name="name">物品名称</param>
        /// <param name="spec">规格</param>
        /// <returns>返回获取的BOM表信息</returns>
        public DataTable GetBomInfo(string code, string name, string spec)
        {
            string strSql = "select a.Edition as 产品型号,c.ProductName as 产品名称,a.ParentCode as 父总成编号, " +
                " b.PartName as 父总成名称, a.Version as 版次号,a.Counts as 基数 from  View_P_ProductBomImitate as a inner join " +
                " (select Edition,PartCode,PartName from View_P_ProductBomImitate where AssemblyFlag = 1) as b " +
                " on a.Edition = b.Edition and a.ParentCode = b.PartCode inner join P_ProductInfo as c " +
                " on a.Edition = c.ProductCode where a.PartCode = '" + code + "' and a.PartName = '" + name +
                "' and a.Spec = '" + spec + "' order by a.Edition,a.Counts,a.ParentCode";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得BOM表信息
        /// </summary>
        /// <param name="code">图号型号</param>
        /// <param name="name">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="edition">型号</param>
        /// <returns>返回DataRow</returns>
        public DataRow GetProductInfo(string code, string name, string spec, string edition)
        {
            string strSql = "select * FROM  View_P_ProductBomImitate where PartCode = '" + code + "' and PartName = '" + name
                + "' and Spec = '" + spec + "' and Edition = '" + edition + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return dtTemp.Rows[0];
            }
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteBill(string djh, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_TechnologyAlterationBill
                              where a.BillNo == djh
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
                else
                {
                    if (varData.Single().BillStatus == "已完成")
                    {
                        error = "不能删除已完成的单据";
                        return false;
                    }

                    var varList = from a in dataContext.S_TechnologyAlterationList
                                  where a.BillNo == djh
                                  select a;

                    dataContext.S_TechnologyAlterationList.DeleteAllOnSubmit(varList);
                    dataContext.S_TechnologyAlterationBill.DeleteAllOnSubmit(varData);
                }

                dataContext.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 提交申请
        /// </summary>
        /// <param name="technology">单据信息数据集</param>
        /// <param name="listInfo">明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool SubmitBill(S_TechnologyAlterationBill technology, DataTable listInfo, out string error)
        {
            error = null;

            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            try
            {
                dataContext.Connection.Open();
                dataContext.Transaction = dataContext.Connection.BeginTransaction();

                var varData = from a in dataContext.S_TechnologyAlterationBill
                              where a.BillNo == technology.BillNo
                              select a;

                if (varData.Count() == 0)
                {
                    technology.BillStatus = "等待批准";

                    if (!InsertList(dataContext, technology.BillNo, listInfo, out error))
                    {
                        throw new Exception(error);
                    }

                    dataContext.S_TechnologyAlterationBill.InsertOnSubmit(technology);
                }
                else if (varData.Count() == 1)
                {
                    S_TechnologyAlterationBill lnqBill = varData.Single();

                    lnqBill.BillStatus = "等待批准";
                    lnqBill.ChangeReason = technology.ChangeReason;
                    lnqBill.ChangeBillNo = technology.ChangeBillNo;
                    lnqBill.FileMessage = technology.FileMessage;

                    if (!DeleteList(dataContext, technology.BillNo, out error))
                    {
                        throw new Exception(error);
                    }

                    if (!InsertList(dataContext, technology.BillNo, listInfo, out error))
                    {
                        throw new Exception(error);
                    }
                }
                else
                {
                    error = "数据重复";
                    throw new Exception(error);
                }

                dataContext.SubmitChanges();

                dataContext.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                dataContext.Transaction.Rollback();
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除明细
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        private bool DeleteList(DepotManagementDataContext dataContext, string billNo, out string error)
        {
            error = null;

            try
            {
                var varData = from a in dataContext.S_TechnologyAlterationList
                              where a.BillNo == billNo
                              select a;

                dataContext.S_TechnologyAlterationList.DeleteAllOnSubmit(varData);

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;

            }
        }

        /// <summary>
        /// 插入明细
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="listInfo">明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        private bool InsertList(DepotManagementDataContext dataContext, string billNo, DataTable listInfo, out string error)
        {
            error = null;

            try
            {
                for (int i = 0; i < listInfo.Rows.Count; i++)
                {
                    S_TechnologyAlterationList lnqList = new S_TechnologyAlterationList();

                    lnqList.BillNo = billNo;
                    lnqList.ChangeReason = listInfo.Rows[i]["变更原因"].ToString();
                    lnqList.NewCounts = Convert.ToInt32(listInfo.Rows[i]["新零件基数"].ToString());
                    lnqList.NewGoodsID = Convert.ToInt32(listInfo.Rows[i]["NewGoodsID"].ToString());
                    lnqList.NewParentID = Convert.ToInt32(listInfo.Rows[i]["NewParentID"].ToString());
                    lnqList.NewVersion = listInfo.Rows[i]["新零件版次号"].ToString();
                    lnqList.NewGoodsCode = listInfo.Rows[i]["新零件编码"].ToString();
                    lnqList.NewGoodsName = listInfo.Rows[i]["新零件名称"].ToString();
                    lnqList.NewGoodsSpec = listInfo.Rows[i]["新零件规格"].ToString();
                    lnqList.ChangeMode = listInfo.Rows[i]["变更模式"].ToString();
                    lnqList.IsChangeGoodsInfo = listInfo.Rows[i]["是否修改零件本身"].ToString();

                    if (listInfo.Rows[i]["OldGoodsID"] != null && listInfo.Rows[i]["OldGoodsID"].ToString() != "")
                    {
                        lnqList.OldCounts = listInfo.Rows[i]["旧零件基数"].ToString() == "" ? 0 : Convert.ToInt32(listInfo.Rows[i]["旧零件基数"].ToString());
                        lnqList.OldGoodsID = Convert.ToInt32(listInfo.Rows[i]["OldGoodsID"].ToString());
                        lnqList.OldParentID = Convert.ToInt32(listInfo.Rows[i]["OldParentID"].ToString());
                        lnqList.OldVersion = listInfo.Rows[i]["旧零件版次号"].ToString();
                        lnqList.OldGoodsCode = listInfo.Rows[i]["旧零件编码"].ToString();
                        lnqList.OldGoodsName = listInfo.Rows[i]["旧零件名称"].ToString();
                        lnqList.OldGoodsSpec = listInfo.Rows[i]["旧零件规格"].ToString();
                    }

                    dataContext.S_TechnologyAlterationList.InsertOnSubmit(lnqList);
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
        /// 更改单据状态
        /// </summary>
        /// <param name="technology">变更单信息</param>
        /// <param name="listInfo">零件信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更改成功返回True，更改失败返回False</returns>
        public bool UpdateBill(S_TechnologyAlterationBill technology,DataTable listInfo, out string error)
        {
            error = null;

            try
            {
                bool flag = false;

                for (int i = 0; i < listInfo.Rows.Count; i++)
                {
                    if (listInfo.Rows[i]["是否修改零件本身"].ToString() == "是")
                    {
                        flag = true;
                        break;
                    }
                }

                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_TechnologyAlterationBill
                              where a.BillNo == technology.BillNo
                              select a;

                if (varData.Count() != 1)
                {
                    error = "单据不唯一或者为空";
                    return false;
                }
                else
                {
                    S_TechnologyAlterationBill lnqBill = varData.Single();

                    if (technology.BillStatus == "等待批准")
                    {
                        lnqBill.RatifyDate = ServerTime.Time;
                        lnqBill.Ratifier = BasicInfo.LoginID;

                        if (!flag)
                        {
                            lnqBill.BillStatus = "已完成";
                        }
                        else
                        {
                            lnqBill.BillStatus = "等待财务审核";

                            dataContext.SubmitChanges();
                            return true;
                        }
                    }
                    else if (technology.BillStatus == "等待财务审核")
                    {
                        lnqBill.BillStatus = "已完成";
                    }
                    else
                    {
                        lnqBill.BillStatus = technology.BillStatus;

                        dataContext.SubmitChanges();
                        return true;
                    }

                    if (!UpdateBOMDate(dataContext, lnqBill, listInfo, out error))
                    {
                        return false;
                    }
                }

                dataContext.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 对BOM表中数据进行变更
        /// </summary>
        /// <param name="datatContxt">数据上下文</param>
        /// <param name="technology">单据信息数据集</param>
        /// <param name="listInfo">变更单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>变更成功返回True，变更失败返回False</returns>
        public bool UpdateBOMDate(DepotManagementDataContext datatContxt,
            S_TechnologyAlterationBill technology, DataTable listInfo, out string error)
        {
            error = null;
            DateTime time = ServerTime.Time;

            try
            {
                for (int i = 0; i < listInfo.Rows.Count; i++)
                {
                    F_GoodsPlanCost lnqGoodsPlan = new F_GoodsPlanCost();
                    BASE_BomStruct lnqBomStruct = new BASE_BomStruct();
                    BASE_BomPartsLibrary lnqLibrary = new BASE_BomPartsLibrary();
                    DataRow row = listInfo.Rows[i];

                    if (row["变更模式"].ToString() == "新增")
                    {
                        if (row["NewGoodsID"] != null)
                        {
                            var resultGoods = from a in datatContxt.F_GoodsPlanCost
                                              where a.GoodsCode == row["新零件编码"].ToString()
                                              && a.GoodsName == row["新零件名称"].ToString() && a.Spec == row["新零件规格"].ToString()
                                              select a;

                            if (resultGoods.Count() == 0)
                            {
                                lnqGoodsPlan.UserCode = technology.Applicant;
                                lnqGoodsPlan.Date = time;
                                lnqGoodsPlan.GoodsCode = row["新零件编码"].ToString();
                                lnqGoodsPlan.GoodsName = row["新零件名称"].ToString();
                                lnqGoodsPlan.Spec = row["新零件规格"].ToString();
                                lnqGoodsPlan.GoodsType = "030101";
                                lnqGoodsPlan.GoodsUnitPrice = 0;
                                lnqGoodsPlan.IsDisable = false;
                                lnqGoodsPlan.PY = UniversalFunction.GetPYWBCode(row["新零件名称"].ToString(), "PY");
                                lnqGoodsPlan.WB = UniversalFunction.GetPYWBCode(row["新零件名称"].ToString(), "WB");
                                lnqGoodsPlan.Remark = "由技术变更单【" + technology.BillNo + "】新增";
                                lnqGoodsPlan.UnitID = 1;

                                datatContxt.F_GoodsPlanCost.InsertOnSubmit(lnqGoodsPlan);
                            }

                            var varBomPartsLibrary = from a in datatContxt.BASE_BomPartsLibrary
                                                     where a.GoodsID == Convert.ToInt32(row["NewGoodsID"])
                                                     select a;

                            if (varBomPartsLibrary.Count() == 0)
                            {
                                lnqLibrary.CreateDate = time;
                                lnqLibrary.CreatePersonnel = technology.Applicant;
                                lnqLibrary.GoodsID = Convert.ToInt32(row["NewGoodsID"]);
                                lnqLibrary.Material = "未知";
                                lnqLibrary.PartType = "产品";
                                lnqLibrary.PivotalPart = "A";
                                lnqLibrary.Remark = "由技术变更单【" + technology.BillNo + "】生成";
                                lnqLibrary.Version = row["新零件版次号"].ToString();

                                datatContxt.BASE_BomPartsLibrary.InsertOnSubmit(lnqLibrary);
                            }
                            else if (varBomPartsLibrary.Count() == 1)
                            {
                                lnqLibrary = varBomPartsLibrary.Single();

                                lnqLibrary.Version = row["新零件版次号"].ToString() == "" ? varBomPartsLibrary.Single().Version : row["新零件版次号"].ToString();
                                lnqLibrary.CreateDate = time;
                                lnqLibrary.CreatePersonnel = technology.Applicant;
                            }
                        }

                        if (row["NewParentID"] != null && row["NewGoodsID"] != null)
                        {
                            lnqBomStruct.CreateDate = time;
                            lnqBomStruct.CreatePersonnel = technology.Applicant;
                            lnqBomStruct.GoodsID = Convert.ToInt32(row["NewGoodsID"]);
                            lnqBomStruct.ParentID = Convert.ToInt32(row["NewParentID"]);
                            lnqBomStruct.SysVersion = 1;
                            lnqBomStruct.Usage = Convert.ToDecimal(row["新零件基数"]);

                            datatContxt.BASE_BomStruct.InsertOnSubmit(lnqBomStruct);
                        }
                    }
                    else if (row["变更模式"].ToString() == "修改" && row["是否修改零件本身"].ToString() == "否")
                    {
                        if (row["NewGoodsID"] != null)
                        {
                            var varBomPartsLibrary = from a in datatContxt.BASE_BomPartsLibrary
                                                     where a.GoodsID == Convert.ToInt32(row["NewGoodsID"])
                                                     select a;

                            if (varBomPartsLibrary.Count() == 0)
                            {
                                lnqLibrary.CreateDate = time;
                                lnqLibrary.CreatePersonnel = technology.Applicant;
                                lnqLibrary.GoodsID = Convert.ToInt32(row["NewGoodsID"]);
                                lnqLibrary.Material = "未知";
                                lnqLibrary.PartType = "产品";
                                lnqLibrary.PivotalPart = "A";
                                lnqLibrary.Remark = "由技术变更单【" + technology.BillNo + "】生成";
                                lnqLibrary.Version = row["新零件版次号"].ToString();

                                datatContxt.BASE_BomPartsLibrary.InsertOnSubmit(lnqLibrary);
                            }
                            else
                            {
                                lnqLibrary = varBomPartsLibrary.Single();

                                lnqLibrary.Version = row["新零件版次号"].ToString();
                            }
                        }

                        if (row["OldParentID"] != null && row["NewParentID"] != null
                            && row["OldGoodsID"] != null && row["NewGoodsID"] != null
                            && (int)row["OldParentID"] != 0 && (int)row["NewParentID"] != 0)
                        {
                            var varBomStruct = from a in datatContxt.BASE_BomStruct
                                               where a.ParentID == Convert.ToInt32(row["OldParentID"])
                                               && a.GoodsID == Convert.ToInt32(row["OldGoodsID"])
                                               select a;

                            if (varBomStruct.Count() < 1)
                            {
                                error = "BOM结构表中不存在此记录";
                                return false;
                            }
                            else
                            {
                                lnqBomStruct = varBomStruct.Single();

                                lnqBomStruct.CreateDate = time;
                                lnqBomStruct.CreatePersonnel = technology.Applicant;
                                lnqBomStruct.GoodsID = Convert.ToInt32(row["NewGoodsID"]);
                                lnqBomStruct.ParentID = Convert.ToInt32(row["NewParentID"]);
                                lnqBomStruct.SysVersion = lnqBomStruct.SysVersion + Convert.ToDecimal(0.01);
                                lnqBomStruct.Usage = Convert.ToDecimal(row["新零件基数"]);

                                var result = from a in datatContxt.BASE_BomStruct
                                             where a.ParentID == Convert.ToInt32(row["OldGoodsID"])
                                             select a;

                                foreach (BASE_BomStruct item in result)
                                {
                                    if (item.ParentID != Convert.ToInt32(row["NewGoodsID"]))
                                    {
                                        BASE_BomStruct baseBom = new BASE_BomStruct();

                                        baseBom.ParentID = Convert.ToInt32(row["NewGoodsID"]);
                                        baseBom.CreateDate = time;
                                        baseBom.CreatePersonnel = technology.Applicant;
                                        baseBom.GoodsID = item.GoodsID;
                                        baseBom.SysVersion = item.SysVersion;
                                        baseBom.Usage = item.Usage;

                                        datatContxt.BASE_BomStruct.InsertOnSubmit(baseBom);
                                    }
                                }
                            }
                        }
                        else
                        {
                            var varBomStruct = from a in datatContxt.BASE_BomStruct
                                               where a.GoodsID == Convert.ToInt32(row["OldGoodsID"])
                                               select a;

                            foreach (BASE_BomStruct item in varBomStruct)
                            {
                                item.CreateDate = ServerTime.Time;
                                item.CreatePersonnel = technology.Applicant;
                                item.GoodsID = Convert.ToInt32(row["NewGoodsID"]);
                                item.ParentID = item.ParentID;
                                item.SysVersion = lnqBomStruct.SysVersion + Convert.ToDecimal(0.01);
                                item.Usage = Convert.ToDecimal(row["新零件基数"]);
                            }
                        }
                    }
                    else if (row["变更模式"].ToString() == "修改" && row["是否修改零件本身"].ToString() == "是")
                    {
                        if (row["NewGoodsID"] != null && row["OldGoodsID"] != null)
                        {
                            var resultGoods = from a in datatContxt.F_GoodsPlanCost
                                              where a.ID == Convert.ToInt32(row["OldGoodsID"])
                                              select a;

                            if (resultGoods.Count() == 1)
                            {
                                lnqGoodsPlan = resultGoods.Single();

                                lnqGoodsPlan.UserCode = technology.Applicant;
                                lnqGoodsPlan.Date = ServerTime.Time;
                                lnqGoodsPlan.GoodsCode = row["新零件编码"].ToString();
                                lnqGoodsPlan.GoodsName = row["新零件名称"].ToString();
                                lnqGoodsPlan.Spec = row["新零件规格"].ToString();
                                lnqGoodsPlan.PY = UniversalFunction.GetPYWBCode(row["新零件名称"].ToString(), "PY");
                                lnqGoodsPlan.WB = UniversalFunction.GetPYWBCode(row["新零件名称"].ToString(), "WB");
                                lnqGoodsPlan.Remark = "由技术变更单【" + technology.BillNo + "】修改零件";
                            }
                            else
                            {
                                error = row["新零件名称"].ToString() + "修改失败";
                                return false;
                            }

                            var varBomPartsLibrary = from a in datatContxt.BASE_BomPartsLibrary
                                                     where a.GoodsID == Convert.ToInt32(row["NewGoodsID"])
                                                     select a;

                            if (varBomPartsLibrary.Count() != 1)
                            {
                                error = "新零件在零件库中不存在";
                                return false;
                            }
                            else
                            {
                                lnqLibrary = varBomPartsLibrary.Single();

                                lnqLibrary.Version = row["新零件版次号"].ToString();
                            }
                        }
                    }
                    else if (row["变更模式"].ToString() == "删除")
                    {
                        if (row["OldParentID"] == null)
                        {

                            var varStruct = from a in datatContxt.BASE_BomStruct
                                            where a.GoodsID == Convert.ToInt32(row["OldGoodsID"])
                                            select a;

                            datatContxt.BASE_BomStruct.DeleteAllOnSubmit(varStruct);

                            var varLibrary = from a in datatContxt.BASE_BomPartsLibrary
                                             where a.GoodsID == Convert.ToInt32(row["OldGoodsID"])
                                             select a;

                            datatContxt.BASE_BomPartsLibrary.DeleteAllOnSubmit(varLibrary);
                        }
                        else
                        {
                            var varStruct = from a in datatContxt.BASE_BomStruct
                                            where a.ParentID == Convert.ToInt32(row["OldParentID"])
                                            && a.GoodsID == Convert.ToInt32(row["OldGoodsID"])
                                            select a;

                            if (varStruct.Count() != 1)
                            {
                                error = "BOM结构表中不存在此记录";
                                return false;
                            }
                            else
                            {
                                lnqBomStruct = varStruct.Single();
                                datatContxt.BASE_BomStruct.DeleteOnSubmit(lnqBomStruct);
                            }
                        }
                    }

                    datatContxt.SubmitChanges();
                }

                #region 
                //switch (technology.ChangeMode)
                //{
                //    case "新增":

                //        #region 新增

                //        for (int i = 0; i < listInfo.Rows.Count; i++)
                //        {
                //            DataRow row = listInfo.Rows[i];

                //            if (row["NewGoodsID"] != null)
                //            {
                //                var resultGoods = from a in datatContxt.F_GoodsPlanCost
                //                                  where a.GoodsCode == row["新零件编码"].ToString()
                //                                  && a.GoodsName == row["新零件名称"].ToString() && a.Spec == row["新零件规格"].ToString()
                //                                  select a;

                //                if (resultGoods.Count() == 0)
                //                {
                //                    lnqGoodsPlan.UserCode = technology.Applicant;
                //                    lnqGoodsPlan.Date = ServerTime.Time;
                //                    lnqGoodsPlan.GoodsCode = row["新零件编码"].ToString();
                //                    lnqGoodsPlan.GoodsName = row["新零件名称"].ToString();
                //                    lnqGoodsPlan.Spec = row["新零件规格"].ToString();
                //                    lnqGoodsPlan.GoodsType = "ZZLJJXLFBZ";
                //                    lnqGoodsPlan.GoodsUnitPrice = 0;
                //                    lnqGoodsPlan.IsDisable = false;
                //                    lnqGoodsPlan.IsShelfLife = false;
                //                    lnqGoodsPlan.PY = UniversalFunction.GetPYWBCode(row["新零件名称"].ToString(), "PY");
                //                    lnqGoodsPlan.WB = UniversalFunction.GetPYWBCode(row["新零件名称"].ToString(), "WB");
                //                    lnqGoodsPlan.Remark = "由技术变更单【" + technology.BillNo + "】新增";
                //                    lnqGoodsPlan.UnitID = 1;

                //                    datatContxt.F_GoodsPlanCost.InsertOnSubmit(lnqGoodsPlan);
                //                }

                //                var varBomPartsLibrary = from a in datatContxt.BASE_BomPartsLibrary
                //                                         where a.GoodsID == Convert.ToInt32(row["NewGoodsID"])
                //                                         select a;

                //                if (varBomPartsLibrary.Count() == 0)
                //                {
                //                    lnqLibrary.CreateDate = ServerTime.Time;
                //                    lnqLibrary.CreatePersonnel = technology.Applicant;
                //                    lnqLibrary.GoodsID = Convert.ToInt32(row["NewGoodsID"]);
                //                    lnqLibrary.Material = "未知";
                //                    lnqLibrary.PartType = "产品";
                //                    lnqLibrary.PivotalPart = "A";
                //                    lnqLibrary.Remark = "由技术变更单【" + technology.BillNo + "】生成";
                //                    lnqLibrary.Version = row["新零件版次号"].ToString();

                //                    datatContxt.BASE_BomPartsLibrary.InsertOnSubmit(lnqLibrary);
                //                }
                //                else if (varBomPartsLibrary.Count() == 1)
                //                {
                //                    lnqLibrary = varBomPartsLibrary.Single();

                //                    lnqLibrary.Version = row["新零件版次号"].ToString();
                //                    lnqLibrary.CreateDate = ServerTime.Time;
                //                    lnqLibrary.CreatePersonnel = technology.Applicant;
                //                }
                //            }

                //            if (row["NewParentID"] != null && row["NewGoodsID"] != null)
                //            {
                //                lnqBomStruct.CreateDate = ServerTime.Time;
                //                lnqBomStruct.CreatePersonnel = technology.Applicant;
                //                lnqBomStruct.GoodsID = Convert.ToInt32(row["NewGoodsID"]);
                //                lnqBomStruct.ParentID = Convert.ToInt32(row["NewParentID"]);
                //                lnqBomStruct.SysVersion = 1;
                //                lnqBomStruct.Usage = Convert.ToDecimal(row["新零件基数"]);

                //                datatContxt.BASE_BomStruct.InsertOnSubmit(lnqBomStruct);
                //            }
                //        }
                //        #endregion

                //        break;
                //    case "修改":

                //        #region

                //        for (int i = 0; i < listInfo.Rows.Count; i++)
                //        {
                //            DataRow row = listInfo.Rows[i];

                //            if (row["NewGoodsID"] != null)
                //            {
                //                var varBomPartsLibrary = from a in datatContxt.BASE_BomPartsLibrary
                //                                         where a.GoodsID == Convert.ToInt32(row["NewGoodsID"])
                //                                         select a;

                //                if (varBomPartsLibrary.Count() != 1)
                //                {
                //                    error = "新零件在零件库中不存在";
                //                    return false;
                //                }
                //                else
                //                {
                //                    lnqLibrary = varBomPartsLibrary.Single();

                //                    lnqLibrary.Version = row["新零件版次号"].ToString();
                //                }
                //            }

                //            if (row["OldParentID"] != null && row["NewParentID"] != null
                //                && row["OldGoodsID"] != null && row["NewGoodsID"] != null)
                //            {
                //                var varBomStruct = from a in datatContxt.BASE_BomStruct
                //                                   where a.ParentID == Convert.ToInt32(row["OldParentID"])
                //                                   && a.GoodsID == Convert.ToInt32(row["OldGoodsID"])
                //                                   select a;

                //                if (varBomStruct.Count() != 1)
                //                {
                //                    error = "BOM结构表中不存在此记录";
                //                    return false;
                //                }
                //                else
                //                {
                //                    lnqBomStruct = varBomStruct.Single();

                //                    lnqBomStruct.CreateDate = ServerTime.Time;
                //                    lnqBomStruct.CreatePersonnel = technology.Applicant;
                //                    lnqBomStruct.GoodsID = Convert.ToInt32(row["NewGoodsID"]);
                //                    lnqBomStruct.ParentID = Convert.ToInt32(row["NewParentID"]);
                //                    lnqBomStruct.SysVersion = lnqBomStruct.SysVersion + Convert.ToDecimal(0.01);
                //                    lnqBomStruct.Usage = Convert.ToDecimal(row["新零件基数"]);
                //                }
                //            }
                //        }
                //        #endregion

                //        break;
                //    case "删除":

                //        #region 删除
                //        for (int i = 0; i < listInfo.Rows.Count; i++)
                //        {
                //            DataRow row = listInfo.Rows[i];

                //            if (row["OldParentID"] == null)
                //            {

                //                var varStruct = from a in datatContxt.BASE_BomStruct
                //                                where a.GoodsID == Convert.ToInt32(row["OldGoodsID"])
                //                                select a;

                //                datatContxt.BASE_BomStruct.DeleteAllOnSubmit(varStruct);

                //                var varLibrary = from a in datatContxt.BASE_BomPartsLibrary
                //                                 where a.GoodsID == Convert.ToInt32(row["OldGoodsID"])
                //                                 select a;

                //                datatContxt.BASE_BomPartsLibrary.DeleteAllOnSubmit(varLibrary);
                //            }
                //            else
                //            {
                //                var varStruct = from a in datatContxt.BASE_BomStruct
                //                                where a.ParentID == Convert.ToInt32(row["OldParentID"])
                //                                && a.GoodsID == Convert.ToInt32(row["OldGoodsID"])
                //                                select a;

                //                if (varStruct.Count() != 1)
                //                {
                //                    error = "BOM结构表中不存在此记录";
                //                    return false;
                //                }
                //                else
                //                {
                //                    lnqBomStruct = varStruct.Single();
                //                    datatContxt.BASE_BomStruct.DeleteOnSubmit(lnqBomStruct);
                //                }
                //            }
                //        }
                //        #endregion
                //        break;
                //    case "修改零件本身":

                //        for (int i = 0; i < listInfo.Rows.Count; i++)
                //        {
                //            DataRow row = listInfo.Rows[i];

                //            if (row["NewGoodsID"] != null && row["OldGoodsID"] != null)
                //            {
                //                var resultGoods = from a in datatContxt.F_GoodsPlanCost
                //                                  where a.ID == Convert.ToInt32(row["OldGoodsID"])
                //                                  select a;

                //                if (resultGoods.Count() == 1)
                //                {
                //                    lnqGoodsPlan = resultGoods.Single();

                //                    lnqGoodsPlan.UserCode = technology.Applicant;
                //                    lnqGoodsPlan.Date = ServerTime.Time;
                //                    lnqGoodsPlan.GoodsCode = row["新零件编码"].ToString();
                //                    lnqGoodsPlan.GoodsName = row["新零件名称"].ToString();
                //                    lnqGoodsPlan.Spec = row["新零件规格"].ToString();
                //                    lnqGoodsPlan.PY = UniversalFunction.GetPYWBCode(row["新零件名称"].ToString(), "PY");
                //                    lnqGoodsPlan.WB = UniversalFunction.GetPYWBCode(row["新零件名称"].ToString(), "WB");
                //                    lnqGoodsPlan.Remark = "由技术变更单【" + technology.BillNo + "】修改零件";
                //                }
                //                else
                //                {
                //                    error = row["新零件名称"].ToString() + "修改失败";
                //                    return false;
                //                }

                //                //var varBomPartsLibrary = from a in datatContxt.BASE_BomPartsLibrary
                //                //                         where a.GoodsID == Convert.ToInt32(row["NewGoodsID"])
                //                //                         select a;

                //                //if (varBomPartsLibrary.Count() == 0)
                //                //{
                //                //    lnqLibrary.CreateDate = ServerTime.Time;
                //                //    lnqLibrary.CreatePersonnel = technology.Applicant;
                //                //    lnqLibrary.GoodsID = Convert.ToInt32(row["NewGoodsID"]);
                //                //    lnqLibrary.Material = "未知";
                //                //    lnqLibrary.PartType = "产品";
                //                //    lnqLibrary.PivotalPart = "A";
                //                //    lnqLibrary.Remark = "由技术变更单【" + technology.BillNo + "】生成";
                //                //    lnqLibrary.Version = row["新零件版次号"].ToString();

                //                //    datatContxt.BASE_BomPartsLibrary.InsertOnSubmit(lnqLibrary);
                //                //}
                //                //else if (varBomPartsLibrary.Count() == 1)
                //                //{
                //                //    lnqLibrary = varBomPartsLibrary.Single();

                //                //    lnqLibrary.Version = row["新零件版次号"].ToString();
                //                //    lnqLibrary.CreateDate = ServerTime.Time;
                //                //    lnqLibrary.CreatePersonnel = technology.Applicant;
                //                //}
                //            }

                //            //if (row["OldParentID"] != null && row["NewParentID"] != null
                //            //    && row["OldGoodsID"] != null && row["NewGoodsID"] != null)
                //            //{
                //            //    var varBomStruct = from a in datatContxt.BASE_BomStruct
                //            //                       where a.ParentID == Convert.ToInt32(row["OldParentID"])
                //            //                       && a.GoodsID == Convert.ToInt32(row["OldGoodsID"])
                //            //                       select a;

                //            //    if (varBomStruct.Count() != 1)
                //            //    {
                //            //        error = "BOM结构表中不存在此记录";
                //            //        return false;
                //            //    }
                //            //    else
                //            //    {
                //            //        lnqBomStruct = varBomStruct.Single();

                //            //        lnqBomStruct.CreateDate = ServerTime.Time;
                //            //        lnqBomStruct.CreatePersonnel = technology.Applicant;
                //            //        lnqBomStruct.GoodsID = Convert.ToInt32(row["NewGoodsID"]);
                //            //        lnqBomStruct.ParentID = Convert.ToInt32(row["NewParentID"]);
                //            //        lnqBomStruct.SysVersion = lnqBomStruct.SysVersion + Convert.ToDecimal(0.01);
                //            //        lnqBomStruct.Usage = Convert.ToDecimal(row["新零件基数"]);
                //            //    }
                //            //}
                //        }
                //        break;
                //    default:
                //        break;
                //}
                #endregion

                datatContxt.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="strDJH">单据号</param>
        /// <param name="strBillStatus">单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="strRebackReason">回退原因</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool ReturnBill(string strDJH, string strBillStatus, out string error, string strRebackReason)
        {
            error = null;

            try
            {
                m_msgPromulgator.BillType = "技术变更处置单";
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.S_TechnologyAlterationBill
                              where a.BillNo == strDJH
                              select a;

                string strMsg = "";

                if (result.Count() == 1)
                {
                    S_TechnologyAlterationBill service = result.Single();

                    switch (strBillStatus)
                    {
                        case "等待批准":
                            strMsg = string.Format("{0}号技术变更处置单已回退，请您重新处理单据; 回退原因为" + strRebackReason, strDJH);
                            m_msgPromulgator.PassFlowMessage(strDJH, strMsg, 
                                BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.零件工程师.ToString());

                            service.BillStatus = "等待批准";
                            service.Ratifier = "";
                            service.RatifyDate = ServerTime.Time;
                            
                            break;
                        case "新建单据":
                            strMsg = string.Format("{0}号技术变更处置单已回退，请您重新处理单据; 回退原因为" + strRebackReason, strDJH);

                            m_msgPromulgator.PassFlowMessage(strDJH, strMsg,
                                BillFlowMessage_ReceivedUserType.用户,service.Applicant.ToString());

                            service.BillStatus = "新建单据";
                            service.Ratifier = "";
                            service.RatifyDate = ServerTime.Time;
                            break;
                        default:
                            break;
                    }

                    dataContxt.SubmitChanges();

                    return true;
                }
                else
                {
                    error = "数据不唯一或者为空";

                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;

                return false;
            }
        }

    }
}
