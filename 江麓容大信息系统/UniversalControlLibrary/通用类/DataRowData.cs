using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 数据行数据提取器
    /// </summary>
    public class DataRowData
    {
        /// <summary>
        /// 数据行
        /// </summary>
        private DataRow m_data;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data">数据行</param>
        public DataRowData(DataRow data)
        {
            m_data = data;
        }

        /// <summary>
        /// 直接提取返回值为字符串类型的单元数据
        /// </summary>
        /// <param name="colName">列名</param>
        /// <returns>获取到的值</returns>
        public string this[string colName]
        {
            get
            {
                if (m_data[colName] == null || m_data[colName] == DBNull.Value)
                    return "";
                else
                    return m_data[colName].ToString();
            }
        }

        /// <summary>
        /// 直接提取返回值为日期类型的单元数据
        /// </summary>
        /// <param name="colName">列名</param>
        /// <returns>获取到的值</returns>
        public object GetData(string colName)
        {
            if (m_data[colName] == null || m_data[colName] == DBNull.Value)
                return null;
            else
                return m_data[colName];
        }
        
        /// <summary>
        /// 直接提取返回值为日期类型的单元数据
        /// </summary>
        /// <param name="colName">列名</param>
        /// <returns>获取到的值</returns>
        public DateTime GetDate(string colName)
        {
            if (m_data[colName] == null || m_data[colName] == DBNull.Value)
                return DateTime.MinValue;
            else
                return (DateTime)m_data[colName];
        }

        /// <summary>
        /// 直接提取返回值为日期类型的单元数据
        /// </summary>
        /// <param name="colName">列名</param>
        /// <returns>获取到的值</returns>
        public System.Nullable<DateTime> GetDate2(string colName)
        {
            if (m_data[colName] == null || m_data[colName] == DBNull.Value)
                return null;
            else
                return (DateTime)m_data[colName];
        }

        /// <summary>
        /// 直接提取返回值为整型类型的单元数据
        /// </summary>
        /// <param name="colName">列名</param>
        /// <returns>获取到的值</returns>
        public int GetIntData(string colName)
        {
            if (m_data[colName] == null || m_data[colName] == DBNull.Value)
                return Int32.MinValue;
            else
                return (int)m_data[colName];
        }

        /// <summary>
        /// 直接提取返回值为整型类型的单元数据
        /// </summary>
        /// <param name="colName">列名</param>
        /// <returns>获取到的值</returns>
        public System.Nullable<int> GetIntData2(string colName)
        {
            if (m_data[colName] == null || m_data[colName] == DBNull.Value)
                return null;
            else
                return (int)m_data[colName];
        }

        /// <summary>
        /// 直接提取返回值为整型类型的单元数据
        /// </summary>
        /// <param name="colName">列名</param>
        /// <returns>获取到的值</returns>
        public decimal GetDecimalData(string colName)
        {
            if (m_data[colName] == null || m_data[colName] == DBNull.Value)
                return Decimal.Zero;
            else
                return (decimal)m_data[colName];
        }

        /// <summary>
        /// 直接提取返回值为整型类型的单元数据
        /// </summary>
        /// <param name="colName">列名</param>
        /// <returns>获取到的值</returns>
        public System.Nullable<decimal> GetDecimalData2(string colName)
        {
            if (m_data[colName] == null || m_data[colName] == DBNull.Value)
                return null;
            else
                return (decimal)m_data[colName];
        }
    }
}
