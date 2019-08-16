using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// 单据号分配服务类
    /// </summary>
    class AssignBillNoServer : BasicServer, IAssignBillNoServer
    {
        /// <summary>
        /// 单据类型服务
        /// </summary>
        IBillTypeServer m_billTypeServer = BasicServerFactory.GetServerModule<IBillTypeServer>();

        /// <summary>
        /// 单据服务
        /// </summary>
        IBasicBillServer m_billServer;

        #region IAssignBillNoServer 成员
        /// <summary>
        /// 为指定类别的单据分配新号
        /// </summary>
        /// <param name="dataContxt">数据上下文</param>
        /// <param name="billServer">单据服务</param>
        /// <param name="billTypeName">单据类别名称, 如：领料单</param>
        /// <returns>返回获取到的新单据号</returns>
        public string AssignNewNo(DepotManagementDataContext dataContxt, IBasicBillServer billServer, string billTypeName)
        {
            BASE_BillType billType = m_billTypeServer.GetBillTypeFromName(billTypeName);

            string billNo = null;

            if (billType == null)
            {
                throw new Exception("不存在的单据类型名称：" + billTypeName);
            }

            m_billServer = billServer;

            BASE_AssignBillNo billInfo = null;

            #region 检查是否有放弃的编号

            var result = from r in dataContxt.BASE_AssignBillNo
                         where r.Bill_TypeCode == billType.TypeCode && r.AlreadyUse == false
                         && r.IsAbandoned == true && r.AssignedDate.Year == ServerTime.Time.Year
                         select r;

            if (result.Count() > 0)
            {
                // 是否发现异常单据
                bool findExceptionBill = false;

                foreach (var item in result)
                {
                    if (!billServer.IsExist(dataContxt, item.Bill_No))
                    {
                        billInfo = item;
                        break;
                    }
                    else
                    {
                        findExceptionBill = true;
                        item.IsAbandoned = false;
                    }
                }

                if (findExceptionBill)
                    dataContxt.SubmitChanges();

                if (billInfo != null)
                {
                    Console.WriteLine("使用放弃的单据号：{0}", billInfo.Bill_No);

                    billInfo.IsAbandoned = false;
                    billInfo.AssignedDate = ServerModule.ServerTime.Time;

                    dataContxt.SubmitChanges();

                    return billInfo.Bill_No;
                }
            }

            #endregion

            // 生成新编号
            int maxValue = 1;

            result = from r in dataContxt.BASE_AssignBillNo
                     where r.Bill_TypeCode == billType.TypeCode && r.AssignedDate.Year == ServerTime.Time.Year
                     select r;

            if (result.Count() > 0)
            {
                maxValue += (from c in dataContxt.BASE_AssignBillNo
                             where c.Bill_TypeCode == billType.TypeCode && c.AssignedDate.Year == ServerTime.Time.Year
                             select Convert.ToInt32(c.Bill_No.Substring(billType.TypeCode.Length + 6))).Max();
            }

            //string prefix = GlobalObject.PinYin.GetFirstPYLetter(billTypeName).Substring(0, 3).ToUpper();

            DateTime dt = ServerTime.Time;

            billNo = string.Format("{0}{1:D4}{2:D2}{3:D6}", billType.TypeCode, dt.Year, dt.Month, maxValue);

            Console.WriteLine("使用新分配的单据号：{0}", billNo);

            billInfo = new BASE_AssignBillNo();

            billInfo.AlreadyUse = false;
            billInfo.AssignedDate = ServerModule.ServerTime.Time;
            billInfo.Bill_No = billNo;
            billInfo.Bill_TypeCode = billType.TypeCode;
            billInfo.IsAbandoned = false;

            dataContxt.BASE_AssignBillNo.InsertOnSubmit(billInfo);

            dataContxt.SubmitChanges();

            return billNo;
        }

        /// <summary>
        /// 为指定类别的单据分配新号
        /// </summary>
        /// <param name="billServer">单据服务</param>
        /// <param name="billTypeName">单据类别名称, 如：领料单</param>
        /// <returns>返回获取到的新单据号</returns>
        public string AssignNewNo(IBasicBillServer billServer, string billTypeName)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            BASE_BillType billType = m_billTypeServer.GetBillTypeFromName(billTypeName);

            string billNo = null;

            if (billType == null)
            {
                throw new Exception("不存在的单据类型名称：" + billTypeName);
            }

            m_billServer = billServer;

            BASE_AssignBillNo billInfo = null;

            #region 检查是否有放弃的编号

            var result = from r in dataContxt.BASE_AssignBillNo
                         where r.Bill_TypeCode == billType.TypeCode && r.AlreadyUse == false
                         && r.IsAbandoned == true && r.AssignedDate.Year == ServerTime.Time.Year
                         select r;

            if (result.Count() > 0)
            {
                // 是否发现异常单据
                bool findExceptionBill = false;

                foreach (var item in result)
                {
                    if (!billServer.IsExist(item.Bill_No))
                    {
                        billInfo = item;
                        break;
                    }
                    else
                    {
                        findExceptionBill = true;
                        item.IsAbandoned = false;
                    }
                }

                if (findExceptionBill)
                    dataContxt.SubmitChanges();

                if (billInfo != null)
                {
                    Console.WriteLine("使用放弃的单据号：{0}", billInfo.Bill_No);

                    billInfo.IsAbandoned = false;
                    billInfo.AssignedDate = ServerModule.ServerTime.Time;

                    dataContxt.SubmitChanges();

                    return billInfo.Bill_No;
                }
            }

            #endregion

            // 生成新编号
            int maxValue = 1;

            result = from r in dataContxt.BASE_AssignBillNo
                     where r.Bill_TypeCode == billType.TypeCode && r.AssignedDate.Year == ServerTime.Time.Year
                     select r;

            if (result.Count() > 0)
            {
                maxValue += (from c in dataContxt.BASE_AssignBillNo
                             where c.Bill_TypeCode == billType.TypeCode && c.AssignedDate.Year == ServerTime.Time.Year
                             select Convert.ToInt32(c.Bill_No.Substring(billType.TypeCode.Length + 6))).Max();
            }

            //string prefix = GlobalObject.PinYin.GetFirstPYLetter(billTypeName).Substring(0, 3).ToUpper();

            DateTime dt = ServerTime.Time;

            billNo = string.Format("{0}{1:D4}{2:D2}{3:D6}", billType.TypeCode, dt.Year, dt.Month, maxValue);

            Console.WriteLine("使用新分配的单据号：{0}", billNo);

            billInfo = new BASE_AssignBillNo();

            billInfo.AlreadyUse = false;
            billInfo.AssignedDate = ServerModule.ServerTime.Time;
            billInfo.Bill_No = billNo;
            billInfo.Bill_TypeCode = billType.TypeCode;
            billInfo.IsAbandoned = false;

            dataContxt.BASE_AssignBillNo.InsertOnSubmit(billInfo);

            dataContxt.SubmitChanges();

            return billNo;
        }

        /// <summary>
        /// 取消分配的单据号
        /// </summary>
        /// <param name="billTypeName">单据类型名称</param>
        /// <param name="billNo">单据号</param>
        /// <returns>成功则返回true, 失败返回false或抛出异常</returns>
        public bool CancelBillNo(string billTypeName, string billNo)
        {
            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                bool result = CancelBillNo(dataContxt, billTypeName, billNo);

                if (result)
                {
                    dataContxt.SubmitChanges();
                }

                return result;
            }
            catch (Exception exce)
            {
                Console.WriteLine("CancelBillNo：{0}", exce.Message);
                return false;
            }
        }

        /// <summary>
        /// 取消分配的单据号
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <returns>成功则返回true, 失败返回false或抛出异常</returns>
        public bool CancelBillNo(DepotManagementDataContext context, string billNo)
        {
            var result = from r in context.BASE_AssignBillNo
                         where r.Bill_No == billNo
                         select r;

            if (result.Count() > 0)
            {
                // 由外部调用时需要强制性删除
                if (result.Single().AlreadyUse)
                {
                    throw new Exception(string.Format("编号为 [{1}] 的单据已经正式使用不能取消！", billNo));
                }

                result.Single().IsAbandoned = true;
                result.Single().AlreadyUse = false;

                //删除CVT/TCU总成编号
                var varData = from a in context.ProductsCodes
                              where a.DJH == billNo
                              select a;

                context.ProductsCodes.DeleteAllOnSubmit(varData);

                return true;
            }

            return false;
        }

        /// <summary>
        /// 取消分配的单据号
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="billTypeName">单据类型名称</param>
        /// <param name="billNo">单据号</param>
        /// <returns>成功则返回true, 失败返回false或抛出异常</returns>
        public bool CancelBillNo(DepotManagementDataContext context, string billTypeName, string billNo)
        {
            BASE_BillType billType = m_billTypeServer.GetBillTypeFromName(billTypeName);

            if (billType != null)
            {
                var result = from r in context.BASE_AssignBillNo
                             where r.Bill_TypeCode == billType.TypeCode && r.Bill_No == billNo
                             select r;

                if (result.Count() > 0)
                {
                    // 如果单据已经存在则不能取消单据号
                    if (m_billServer != null)
                    {
                        if (m_billServer.IsExist(billNo))
                            return false;
                    }

                    // 由外部调用时需要强制性删除
                    if (result.Single().AlreadyUse)
                    {
                        throw new Exception(string.Format("{0}的编号为 [{1}] 的单据已经正式使用不能取消！", billTypeName, billNo));
                    }

                    result.Single().IsAbandoned = true;
                    result.Single().AlreadyUse = false;

                    //删除CVT/TCU总成编号
                    var varData = from a in context.ProductsCodes
                                  where a.DJH == billNo
                                  select a;

                    context.ProductsCodes.DeleteAllOnSubmit(varData);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 使用单据号(已经在某单据中正式使用，该单据已经完成)
        /// </summary>
        /// <param name="billTypeName">单据类型名称</param>
        /// <param name="billNo">单据号</param>
        /// <returns>成功则返回true, 失败返回false或抛出异常</returns>
        public bool UseBillNo(string billTypeName, string billNo)
        {
            DepotManagementDataContext dc = CommentParameter.DepotDataContext;

            BASE_BillType billType = m_billTypeServer.GetBillTypeFromName(billTypeName);

            if (billType != null)
            {
                var result = from r in dc.BASE_AssignBillNo
                             where r.Bill_TypeCode == billType.TypeCode && r.Bill_No == billNo
                             select r;

                if (result.Count() > 0)
                {
                    result.Single().IsAbandoned = false;
                    result.Single().AlreadyUse = true;

                    dc.SubmitChanges();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 使用单据号(已经在某单据中正式使用)
        /// </summary>
        /// <param name="dc">数据上下文</param>
        /// <param name="billTypeName">单据类型名称</param>
        /// <param name="billNo">单据号</param>
        /// <returns>成功则返回true, 失败返回false或抛出异常</returns>
        public bool UseBillNo(DepotManagementDataContext dc, string billTypeName, string billNo)
        {
            BASE_BillType billType = m_billTypeServer.GetBillTypeFromName(billTypeName);

            if (billType != null)
            {
                var result = from r in dc.BASE_AssignBillNo
                             where r.Bill_TypeCode == billType.TypeCode && r.Bill_No == billNo
                             select r;

                if (result.Count() > 0)
                {
                    if (result.Single().AlreadyUse)
                    {
                        throw new Exception(string.Format("{0}的编号为 [{1}] 的单据已经在正式使用！", billTypeName, billNo));
                    }

                    result.Single().IsAbandoned = false;
                    result.Single().AlreadyUse = true;

                    return true;
                }
            }

            return false;
        }

        #endregion

        /// <summary>
        /// 更新单据编号信息
        /// </summary>
        /// <param name="billNoInfo">更新后的信息</param>
        /// <returns>操作成功返回true</returns>
        private bool Update(BASE_AssignBillNo billNoInfo)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            var result = from r in dataContxt.BASE_AssignBillNo
                         where r.Bill_TypeCode == billNoInfo.Bill_TypeCode && r.Bill_No == billNoInfo.Bill_No
                         select r;

            if (result.Count() == 0)
            {
                return false;
            }

            BASE_AssignBillNo data = result.Single();

            data.AlreadyUse = billNoInfo.AlreadyUse;
            data.AssignedDate = ServerModule.ServerTime.Time;
            data.IsAbandoned = billNoInfo.IsAbandoned;

            dataContxt.SubmitChanges();
            return true;
        }

        /// <summary>
        /// 获得合同号
        /// </summary>
        /// <param name="strType">合同类型, 用于区分是哪个部门的合同</param>
        /// <returns>获取生成的合同号</returns>
        public string GetBargainNumber(string strType)
        {
            string newBargain = "AutoH";
            newBargain = newBargain + ServerTime.Time.Year.ToString();

            string sSql = "select max(BargainNumber) from B_BargainInfo where BargainNumber like '" + strType + newBargain + "%'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sSql);
            DateTime time = ServerTime.Time;

            if (dt.Rows[0][0].ToString() == "")
            {
                newBargain = strType + newBargain + time.Month.ToString("D2") + "000001";
            }
            else
            {
                string temp = dt.Rows[0][0].ToString();
                int len = temp.Length;
                string head = strType + newBargain + time.Month.ToString("D2");

                temp = (Convert.ToInt32(temp.Substring(len - 6, 6)) + 1).ToString("D6");
                newBargain = head + temp;
            }

            return newBargain;

        }

        /// <summary>
        /// 获得订单号
        /// </summary>
        /// <param name="strType">订单类型</param>
        /// <returns>返回获得的订单号</returns>
        public string GetOrderFormNumber(string strType)
        {
            string newOrderNumber = "AutoD";

            newOrderNumber = newOrderNumber + ServerTime.Time.Year.ToString();

            string sSql = "select max(OrderFormNumber) from B_OrderFormInfo where OrderFormNumber like '%" + strType + newOrderNumber + "%'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sSql);
            DateTime time = ServerTime.Time;

            if (dt.Rows[0][0].ToString() == "")
            {
                newOrderNumber = strType + newOrderNumber + time.Month.ToString("D2") + "000001";
            }
            else
            {
                string temp = dt.Rows[0][0].ToString();
                int len = temp.Length;
                string head = strType + newOrderNumber + time.Month.ToString("D2");

                temp = (Convert.ToInt32(temp.Substring(len - 6, 6)) + 1).ToString("D6");
                newOrderNumber = head + temp;
            }

            return newOrderNumber;
        }
    }
}
