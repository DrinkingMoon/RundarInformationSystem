using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerModule;
using System.Diagnostics;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 单据编号控制类
    /// </summary>
    public class BillNumberControl
    {
        /// <summary>
        /// 单据类型, 表示是领料单、普通入库单等
        /// </summary>
        CE_BillTypeEnum m_billType;

        /// <summary>
        /// 单据服务接口
        /// </summary>
        IBasicBillServer m_billServer;

        /// <summary>
        /// 新建单据而被分配的单据号列表
        /// </summary>
        List<string> m_lstNewBillId = new List<string>();

        /// <summary>
        /// 单据编号分配服务
        /// </summary>
        IAssignBillNoServer m_assignBill = BasicServerFactory.GetServerModule<IAssignBillNoServer>();

        /// <summary>
        /// 信息
        /// </summary>
        public string strMessge = "";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="billType">单据类型</param>
        /// <param name="billServer">单据服务接口</param>
        public BillNumberControl(CE_BillTypeEnum billType, IBasicBillServer billServer)
        {
            Debug.Assert(billServer != null);
            m_billType = billType;
            m_billServer = billServer;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="billType">单据类型</param>
        /// <param name="billServer">单据服务接口</param>
        public BillNumberControl(string billType, IBasicBillServer billServer)
        {
            Debug.Assert(!GlobalObject.GeneralFunction.IsNullOrEmpty(billType));
            Debug.Assert(billServer != null);

            m_billType = (CE_BillTypeEnum)Enum.Parse(typeof(CE_BillTypeEnum), billType);
            m_billServer = billServer;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="billType">单据类型</param>
        /// <param name="billServer">单据服务接口</param>
        public BillNumberControl(string billType)
        {
            Debug.Assert(!GlobalObject.GeneralFunction.IsNullOrEmpty(billType));

            m_billType = (CE_BillTypeEnum)Enum.Parse(typeof(CE_BillTypeEnum), billType);
        }

        /// <summary>
        /// 获取新建单据号
        /// </summary>
        /// <returns>返回获取到的单据号</returns>
        public String GetNewBillNo(DepotManagementDataContext dataContxt)
        {
            string billNo = "";
            bool bFind = false;

            for (int i = 0; i < m_lstNewBillId.Count; i++)
            {
                if (!m_billServer.IsExist(dataContxt, m_lstNewBillId[i]))
                {
                    // 使用旧单据
                    bFind = true;
                    billNo = m_lstNewBillId[i];
                }
                else
                {
                    m_lstNewBillId.RemoveAt(i);
                    i--;
                }
            }

            if (!bFind)
            {
                billNo = m_assignBill.AssignNewNo(dataContxt, m_billServer, m_billType.ToString());
                m_lstNewBillId.Add(billNo);
            }

            return billNo;
        }

        /// <summary>
        /// 获取新建单据号
        /// </summary>
        /// <returns>返回获取到的单据号</returns>
        public String GetNewBillNo()
        {
            string billNo = "";
            bool bFind = false;

            for (int i = 0; i < m_lstNewBillId.Count; i++)
            {
                if (!m_billServer.IsExist(m_lstNewBillId[i]))
                {
                    // 使用旧单据
                    bFind = true;
                    billNo = m_lstNewBillId[i];
                }
                else
                {
                    m_lstNewBillId.RemoveAt(i);
                    i--;
                    
                }
            }

            if (!bFind)
            {
                billNo = m_assignBill.AssignNewNo(m_billServer, m_billType.ToString());
                m_lstNewBillId.Add(billNo);
            }

            return billNo;
        }

        /// <summary>
        /// 取消分配了又没有用到的单据
        /// </summary>
        public void CancelBill()
        {
            foreach (var number in m_lstNewBillId)
            {
                CancelBill(number);
            }
        }

        /// <summary>
        /// 取消分配了又没有用到的单据
        /// </summary>
        /// <param name="billNo">要取消的单据号</param>
        public void CancelBill(string billNo)
        {
            m_assignBill.CancelBillNo(m_billType.ToString(), billNo);
        }

        /// <summary>
        /// 使用单据号
        /// </summary>
        /// <param name="billNo">要使用的单据号</param>
        public void UseBill(string billNo)
        {
            m_assignBill.UseBillNo(m_billType.ToString(), billNo);
        }
    }
}
