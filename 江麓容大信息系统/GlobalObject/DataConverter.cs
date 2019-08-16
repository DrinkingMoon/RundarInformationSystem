/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  DataConverter.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2014/05/22
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 数据转换工具
 * 其它 : 本转换工具提供多种数据转换，包括DataTable与对象，全角半角转换等
 * 注意: 对象列表转换为DataTable或DataTable转换为对象列表.
 *       字段参照由对象的PropertyName决定.
 *       数据模型类的属性名必需与字段名一致, 包括大小写一致.
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 ******************************************************************************/

using System;
using System.Reflection;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml;
using System.Web.UI;

namespace GlobalObject
{
    /// <summary>
    /// 数据转换工具，将DataTable与对象之间进行互相转换
    /// </summary>
    public static class DataConverter
    {
        public static List<T> ToList<T>(this DataTable dt) where T : new()
        {
            var list = new List<T>();

            if (dt == null) return list;

            var len = dt.Rows.Count;

            for (var i = 0; i < len; i++)
            {
                var info = new T();

                foreach (DataColumn dc in dt.Rows[i].Table.Columns)
                {
                    var field = dc.ColumnName;

                    var value = dt.Rows[i][field].ToString();

                    if (GlobalObject.GeneralFunction.IsNullOrEmpty(value))
                        continue;

                    if (IsDate(value))
                    {
                        value = DateTime.Parse(value).ToString();
                    }

                    var p = info.GetType().GetProperty(field);

                    try
                    {
                        if (p.PropertyType == typeof(string))
                        {
                            p.SetValue(info, value, null);
                        }
                        else if (p.PropertyType == typeof(int))
                        {
                            p.SetValue(info, int.Parse(value), null);
                        }
                        else if (p.PropertyType == typeof(bool))
                        {
                            p.SetValue(info, bool.Parse(value), null);
                        }
                        else if (p.PropertyType == typeof(DateTime))
                        {
                            p.SetValue(info, DateTime.Parse(value), null);
                        }
                        else if (p.PropertyType == typeof(decimal))
                        {
                            p.SetValue(info, decimal.Parse(value), null);
                        }
                        else if (p.PropertyType == typeof(float))
                        {
                            p.SetValue(info, float.Parse(value), null);
                        }
                        else if (p.PropertyType == typeof(double))
                        {
                            p.SetValue(info, double.Parse(value), null);
                        }
                        else
                        {
                            p.SetValue(info, value, null);
                        }
                    }

                    catch (Exception)
                    {
                        //p.SetValue(info, ex.Message, null);
                    }
                }

                list.Add(info);
            }

            dt.Dispose(); dt = null;

            return list;
        }

        /// <summary>
        /// 按照属性顺序的列名集合
        /// </summary>
        public static IList<string> GetColumnNames(this DataTable dt)
        {
            DataColumnCollection dcc = dt.Columns;

            //由于集合中的元素是确定的，所以可以指定元素的个数，系统就不会分配多余的空间，效率会高点

            IList<string> list = new List<string>(dcc.Count);

            foreach (DataColumn dc in dcc)
            {
                list.Add(dc.ColumnName);
            }

            return list;
        }

        /// <summary>
        /// 判断参数是否是日期型数据
        /// </summary>
        /// <param name="d">要判断的数据</param>
        /// <returns>参数是日期型返回true</returns>
        private static bool IsDate(string d)
        {
            DateTime d1;

            double d2;

            return !double.TryParse(d, out d2) && DateTime.TryParse(d, out d1);
        }

        /// <summary>
        /// 根据类创建表结构
        /// </summary>
        /// <param name="t">要构建表结构的数据类型</param>
        /// <returns>构建的表</returns>
        public static DataTable CreateTable(Type t)
        {
            return BuiltTable(t.GetProperties());
        }

        /// <summary>
        /// 根据对象的属性创建数据表
        /// </summary>
        /// <param name="pinfo">属性数组</param>
        /// <returns>根据属性数组创建的表</returns>
        private static DataTable BuiltTable(PropertyInfo[] pinfo)
        {
            try
            {
                if (pinfo == null)
                    return null;

                DataTable table = new DataTable();

                foreach (PropertyInfo info in pinfo)
                {
                    Type type = info.PropertyType;

                    if (info.PropertyType.IsGenericType)
                        type = info.PropertyType.GetGenericArguments()[0];

                    DataColumn column = new DataColumn(info.Name, type);

                    column.AllowDBNull = true;
                    table.Columns.Add(column);
                }

                return table;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Object to Object. 将一个对象转换为指定类型的对象.
        /// 注意: destination内的Property必需在source内存在.
        /// </summary>
        /// <param name="source">要转换的源数据</param>
        /// <param name="destination">目标对象类型</param>
        /// <returns>转换后的对象</returns>
        public static object CopyProperties(object source, Type destination)
        {
            try
            {
                if (source == null)
                    return null;

                object destObj = destination.Assembly.CreateInstance(destination.FullName);

                PropertyInfo[] propsDest = destObj.GetType().GetProperties();

                foreach (PropertyInfo infoDest in propsDest)
                {
                    object value = GetValueOfObject(source, infoDest.Name);

                    if (CanShallowCopyProperty(value))
                        SetPropertyValue(destObj, infoDest, value);
                }

                return destObj;
            }
            catch { return null; }
        }

        /// <summary>
        /// 指定参数是否可用于浅拷贝
        /// </summary>
        /// <param name="propValue">需判断能否用于浅拷贝的对象</param>
        /// <returns>可用于浅拷贝返回true</returns>
        private static bool CanShallowCopyProperty(object propValue)
        {
            if (propValue == null)
                return true;

            if (propValue.GetType().IsValueType || propValue is string)
                return true;

            return false;
        }

        /// <summary>
        /// 复制对象属性.
        /// </summary>
        /// <param name="source">要转换的源数据</param>
        /// <param name="destination">目标对象类型</param>
        /// <returns>转换后的对象</returns>
        public static void CopyProperties(object source, object destObj)
        {
            try
            {
                if (source == null || destObj == null)
                    return;

                PropertyInfo[] propsDest = destObj.GetType().GetProperties();

                foreach (PropertyInfo infoDest in propsDest)
                {
                    object value = GetValueOfObject(source, infoDest.Name);

                    if (CanShallowCopyProperty(value))
                        SetPropertyValue(destObj, infoDest, value);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 复制对象. 浅拷贝.
        /// </summary>
        /// <param name="source">要克隆的源对象</param>
        /// <returns>复制后的源对象</returns>
        public static object CloneObject(object source)
        {
            try
            {
                if (source == null)
                    return null;

                Type objType = source.GetType();

                object destObj = objType.Assembly.CreateInstance(objType.FullName);

                PropertyInfo[] propsSource = objType.GetProperties();

                foreach (PropertyInfo infoSource in propsSource)
                {
                    object value = GetValueOfObject(source, infoSource.Name);

                    if (CanShallowCopyProperty(value))
                        SetPropertyValue(destObj, infoSource, value);
                }

                return destObj;
            }
            catch 
            {
                return null;
            }
        }

        /// <summary>
        /// 复制一个对象数组.
        /// </summary>
        /// <param name="source">要克隆的源对象列表</param>
        /// <returns>复制后的源对象列表</returns>
        public static ArrayList CloneObjects(IList source)
        {
            if (source == null)
                return null;

            ArrayList ret = new ArrayList();

            foreach (object o in source)
                ret.Add(CloneObject(o));

            return ret;
        }

        /// <summary>
        /// 获取对象指定属性的值
        /// </summary>
        /// <param name="obj">要获取数据的对象</param>
        /// <param name="property">要获取数据的属性名称</param>
        /// <returns>获取到返回数据值，失败返回null</returns>
        public static object GetValueOfObject(object obj, string property)
        {
            try
            {
                if (obj == null)
                    return null;

                Type type = obj.GetType();

                PropertyInfo[] pinfo = type.GetProperties();

                foreach (PropertyInfo info in pinfo)
                {
                    if (info.Name.ToUpper() == property.ToUpper())
                        return info.GetValue(obj, null);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 从对象数据组取出某一个对象. 返回对象指定属性名称(returnPropName)的值
        /// </summary>
        /// <param name="objects">要取出对象的列表</param>
        /// <param name="keyPropName">需判断的属性名称</param>
        /// <param name="keyValue">需判断的属性值</param>
        /// <param name="returnPropName">从满足要求的到对象中获取值所对应的属性名称</param>
        /// <returns>成功则返回从满足要求的到对象中获取指定属性名称对应的数值，失败返回null</returns>
        public static object GetObjectValueByKey(IList objects, string keyPropName, object keyValue, string returnPropName)
        {
            object o = GetObjectByKey(objects, keyPropName, keyValue);

            if (o != null)
                return GetValueOfObject(o, returnPropName);
            else
                return null;
        }

        /// <summary>
        /// 纵对象数据组取出某一个对象. 参数指定关键字段名称(keyPropName)及值(keyValue).
        /// </summary>
        public static object GetObjectByKey(IList objects, string keyPropName, object keyValue)
        {
            foreach (object o in objects)
            {
                object value = GetValueOfObject(o, keyPropName);

                if (value == null)
                    continue;

                if (value.ToString().ToLower() == keyValue.ToString().ToLower())
                {
                    return o;
                }
            }

            return null;
        }

        /// <summary>
        /// 查找对象包含指定属性.
        /// </summary>
        public static bool FindProperty(object obj, string property)
        {
            try
            {
                if (obj == null)
                    return false;

                Type type = obj.GetType();
                PropertyInfo[] pinfo = type.GetProperties();

                foreach (PropertyInfo info in pinfo)
                {
                    if (info.Name.ToUpper() == property.ToUpper())
                        return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public static void SetValueofDataRow(DataRow dr, string field, object value)
        {
            try
            {
                if (dr == null)
                    return;

                dr[field] = value;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 设置对象某个属性的值
        /// </summary>
        public static void SetValueOfObject(object obj, string property, object value)
        {
            try
            {
                if (obj == null)
                {
                    return;
                }

                Type type = obj.GetType();
                PropertyInfo[] pinfo = type.GetProperties();

                foreach (PropertyInfo info in pinfo)
                {
                    if (info.Name.ToUpper() == property.ToUpper())
                    {
                        SetPropertyValue(obj, info, value);
                        break;
                    }
                }
            }
            catch
            {
            }
        }

        public static void SetPropertyValue(object instance, PropertyInfo prop, object value)
        {
            try
            {
                if (prop == null)
                {
                    return;
                }

                if (prop.PropertyType.ToString() == "System.String")
                {
                }
                else if (prop.PropertyType.ToString() == "System.Decimal")
                {
                    value = Decimal.Parse(value.ToString());
                }
                else if (prop.PropertyType.ToString() == "System.Boolean")
                {
                    value = Boolean.Parse(value.ToString());
                }
                else if (prop.PropertyType.ToString() == "System.Int32")
                {
                    value = int.Parse(value.ToString());
                }
                else if (prop.PropertyType.ToString() == "System.Single")
                {
                    value = Single.Parse(value.ToString());
                }
                else if (prop.PropertyType.ToString() == "System.Double")
                {                    
                    value = Single.Parse(value.ToString());
                }
                else if (prop.PropertyType.ToString() == "System.DateTime")
                {
                    value = DateTime.Parse(value.ToString());
                }

                prop.SetValue(instance, value, null);
            }
            catch
            {
            }
        }

        public static IList CSharpDataTypes()
        {
            ArrayList list = new ArrayList();

            list.Add(typeof(System.DateTime));
            list.Add(typeof(System.Byte));
            list.Add(typeof(System.SByte));
            list.Add(typeof(System.Int16));
            list.Add(typeof(System.Int32));
            list.Add(typeof(System.Int64));
            list.Add(typeof(System.IntPtr));
            list.Add(typeof(System.UInt16));
            list.Add(typeof(System.UInt32));
            list.Add(typeof(System.UInt64));
            list.Add(typeof(System.UIntPtr));
            list.Add(typeof(System.Single));
            list.Add(typeof(System.Double));
            list.Add(typeof(System.Decimal));
            list.Add(typeof(System.Boolean));
            list.Add(typeof(System.Char));
            list.Add(typeof(System.String));

            return list;
        }

        /// <summary>
        /// 根据IList对象创建数据表
        /// </summary>
        public static DataTable IListToDataTable(IList list)
        {
            try
            {
                if (list == null)
                    return null;

                if (list.Count <= 0)
                    return null;

                Type type = list[0].GetType();
                PropertyInfo[] pinfo = type.GetProperties();

                DataTable table = BuiltTable(pinfo);//创建表
                DataRow row = null;

                foreach (object o in list)
                {
                    row = table.NewRow();

                    foreach (PropertyInfo info in pinfo)
                    {
                        object v = info.GetValue(o, null);

                        if (!ColumnExists(table, info.Name))
                            continue;

                        if (null == v)
                            row[info.Name] = DBNull.Value;
                        else
                            row[info.Name] = v;
                    }

                    table.Rows.Add(row);
                }

                return table;
            }
            catch
            {
                return null;
            }
        }

        public static bool ColumnExists(DataTable dt, string columnName)
        {
            if (dt == null)
                return false;

            foreach (DataColumn col in dt.Columns)
            {
                if (col.ColumnName.ToLower() == columnName.ToLower())
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 数据表转换为List对象
        /// </summary>
        public static List<T> DataTableToList<T>(DataTable table) where T : new()
        {
            try
            {
                if (table == null)
                    return null;

                List<T> list = new List<T>(table.Rows.Count);
                T o = default(T);
                Type type = typeof(T);
                PropertyInfo[] pinfo = type.GetProperties();

                foreach (DataRow row in table.Rows)
                {
                    o = new T();

                    foreach (PropertyInfo info in pinfo)
                    {
                        SetPropertyValue(o, info, GetFieldValue(row, info.Name));
                    }

                    list.Add(o);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 根据对象的属性取字段的值
        /// </summary>
        private static object GetFieldValue(DataRow row, string propertyName)
        {
            if (row == null)
                return null;

            if (row.Table.Columns.IndexOf(propertyName) >= 0)
            {
                object value = row[propertyName];

                if (value != null && value is DateTime)
                {
                    if ((DateTime)value <= DateTime.MinValue.AddDays(1))
                        value = null;
                }

                return value;
            }

            return null;
        }

        public static DataRow UpdateDataRowFromObject(DataRow row, object o)
        {
            PropertyInfo[] pinfo = o.GetType().GetProperties();

            foreach (PropertyInfo info in pinfo)
            {
                if (row.Table.Columns.IndexOf(info.Name) >= 0)
                    row[info.Name] = info.GetValue(o, null);
            }

            return row;
        }

        public static DataRow AddDataRowFromObject(DataTable dt, object o)
        {
            DataRow row = dt.NewRow();
            PropertyInfo[] pinfo = o.GetType().GetProperties();

            foreach (PropertyInfo info in pinfo)
            {
                if (dt.Columns.IndexOf(info.Name) >= 0)
                    row[info.Name] = info.GetValue(o, null);
            }

            dt.Rows.Add(row);
            return row;
        }

        public static void SetTwoRowValues(DataRow rowFrom, DataRow rowTo)
        {
            for (int i = 0; i < rowFrom.Table.Columns.Count; i++)
            {
                rowTo[i] = rowFrom[i];
            }
        }

        /// <summary>
        /// 从源行中对相同字段名的列付值
        /// </summary>
        /// <param name="drSouce"></param>
        /// <param name="drTo"></param>
        public static void SetTwoRowSameColValue(DataRow drSource, DataRow drTo)
        {
            for (int i = 0; i < drSource.Table.Columns.Count; i++)
            {
                string fieldname = drSource.Table.Columns[i].ColumnName;

                DataColumn col = drTo.Table.Columns[fieldname];

                if (col != null)
                {
                    drTo[fieldname] = drSource[fieldname];
                }
            }
        }

        /// <summary>
        /// 数据行(DataRow)转换为对象,对象的Type由type参数决定.
        /// </summary>
        public static object DataRowToObject(DataRow row, Type type)
        {
            if (null == row)
                return null;

            try
            {
                object o = type.Assembly.CreateInstance(type.FullName);

                PropertyInfo[] pinfo = type.GetProperties();

                foreach (PropertyInfo info in pinfo)
                {
                    //字段名称与对象属性相符才赋值
                    if (row.Table.Columns.IndexOf(info.Name) >= 0)
                    {
                        object v = GetFieldValue(row, info.Name);

                        SetPropertyValue(o, info, v);
                    }
                }

                return o;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// ArrayList转换为对象数组.
        /// </summary>
        public static object[] ToObjects(IList source)
        {
            if (null == source)
                return null;

            object[] ret = new object[source.Count];

            for (int i = 0; i < source.Count; i++)
                ret[i] = source[i];

            return ret;
        }

        /// <summary>
        /// 对象数组转换为ArrayList.
        /// </summary>
        public static ArrayList ToArrayList(IList list)
        {
            if (list == null)
                return null;

            ArrayList arrlist = new ArrayList();

            foreach (object o in list)
                arrlist.Add(o);

            return arrlist;
        }

        /// <summary>
        /// 对象数组转换为ArrayList.
        /// </summary>
        public static ArrayList ToArrayList(object[] source)
        {
            if (null != source)
                return new ArrayList((ICollection)source);
            else //如果来源数据为null,返回一个空的ArrayList.
                return new ArrayList();
        }

        /// <summary>
        /// 把字符串以逗号分格，转换成数据库格式in('a','b') 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSQLInDataFormat(string input)
        {
            string hql = string.Empty;

            if (input == string.Empty)
                return hql;

            string[] sArray = input.Split(',');
            
            foreach (string str in sArray)
            {
                if (str.Length == 0)
                    continue;

                hql += "'" + str + "',";
            }

            if (hql.Substring(hql.Length - 1, 1) == ",")
                hql = hql.Substring(0, hql.Length - 1);

            return hql;
        }

        /// <summary>
        /// 把字符串以逗号分格，转换成数据库格式''a'',''b'' 
        /// </summary>
        public static string ToSQLInDataFormatTwo(string input)
        {
            string hql = string.Empty;

            if (input == string.Empty)
                return hql;

            string[] sArray = input.Split(',');

            foreach (string str in sArray)
            {
                if (str.Length == 0) continue;
                hql += "''" + str + "'',";
            }

            if (hql.Substring(hql.Length - 1, 1) == ",")
                hql = hql.Substring(0, hql.Length - 1);

            return hql;
        }

        public static string ToSQLInDataFormat(string[] input)
        {
            string hql = string.Empty;
            
            if (input.Length == 0)
                return hql;

            foreach (string str in input)
            {
                hql += "'" + str + "',";
            }

            if (hql.Substring(hql.Length - 1, 1) == ",")
                hql = hql.Substring(0, hql.Length - 1);

            return hql;
        }

        /// <summary>
        /// 从table转成dataset
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataSet TableToDataSet(DataTable dt)
        {
            DataSet ds = new DataSet();

            ds.Tables.Add(dt);

            return ds;
        }

        public static DataSet TableToDataSet(DataTable[] dtArr)
        {
            DataSet ds = new DataSet();

            for (int i = 0; i < dtArr.Length; i++)
            {
                ds.Tables.Add(dtArr[i]);
            }

            return ds;
        }

        /// <summary>
        /// 修改对表的某列的值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="value"></param>
        public static bool UpdateTableCol(DataTable dt, string fieldName, object value)
        {
            try
            {
                if (dt.Columns.IndexOf(fieldName) < 0)
                    throw new Exception("表没有" + fieldName + "列！");

                foreach (DataRow dr in dt.Rows)
                {
                    dr[fieldName] = value;
                }

                return true;
            }
            catch
            {
                return false;
            }


        }

        /// <summary>
        /// 以逗号分格字符串，返回数组
        /// 如果第一个和最后一个字符为, 去掉
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] ToStringSplit(string str)
        {
            if (str.Length > 0)
            {
                if (str[0] == ',')
                    str = str.Substring(1, str.Length - 1);

                if (str[str.Length - 1] == ',')
                    str = str.Substring(0, str.Length - 1);
            }

            string[] sArray = str.Split(',');
            return sArray;
        }


        /// <summary>
        /// 把一个行的数据新增一个表中
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static DataTable AddTableRowByRow(DataTable dt, DataRow dr)
        {
            bool b = false;
            DataRow drNew = dt.NewRow();

            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                string colname = dr.Table.Columns[i].ColumnName;

                if (dt.Columns.IndexOf(colname) >= 0)
                {
                    drNew[colname] = dr[colname];
                    b = true;
                }
            }

            if (b)
            {
                dt.Rows.Add(drNew);
            }

            return dt;
        }
    }

    /// <summary>
    /// 对字符或字符串的处理
    /// </summary>
    public class DOString
    {
        /// <summary>
        /// 把字符转成大写并去两边空格
        /// </summary>
        public static string ToTrimAndUpper(string str)
        {
            return str.Trim().ToUpper();
        }

        public static string ToTrimAndUpper(object o)
        {
            return o.ToString().Trim().ToUpper();
        }

        public static string ToTrimAndBigToSmall(object o)
        {
            return BigToSmall(o.ToString().Trim(), CodeWidth.HalfWidth);
        }

        /// <summary>
        /// 返回一个半角 去左右空格 大写的字符串
        /// </summary>
        public static string ToTUBS(object o)
        {
            return BigToSmall(o.ToString().Trim().ToUpper(), CodeWidth.HalfWidth );
        }

        /// <summary>
        /// 判断字符是否是数字。是返回true 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool NuNumber(char c)
        {
            if ((int)c >= 48 && (int)c <= 57)
                return true;
            else
                return false;

        }

        /// <summary>
        /// 去除千分号
        /// </summary>
        public static object OffkiloSign(object obj)
        {
            if (obj == null)
                return obj;

            string s = obj.ToString();

            return s.Replace(",", string.Empty);
        }

        /// <summary>
        /// 全角、半角类别枚举
        /// </summary>
        public enum CodeWidth
        {
            /// <summary>
            /// 全角
            /// </summary>
            FullWidth,

            /// <summary>
            /// 半角
            /// </summary>
            HalfWidth
        }

        /// <summary>
        /// 全角半角间转换
        /// </summary>
        /// <param name="content">要转换的内容</param>
        /// <param name="desCodeWidth">目标编码格式，FullWidth则表示将源内容转为全角</param>
        /// <returns>返回转换后的值</returns>
        public static string BigToSmall(string content, CodeWidth desCodeWidth)
        {
            string strBig, to_strBig;

            strBig = "ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ０１２３４５６７８９　＇﹃﹄『』＄／ａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚ．＊";

            to_strBig = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 '“”“”$/abcdefghijklmnopqrstuvwxyz.*";
            
            for (int i = 0; i < strBig.Length; i++)
            {
                if (desCodeWidth == CodeWidth.HalfWidth)
                {
                    content = content.Replace(strBig[i], to_strBig[i]);
                }
                else
                {
                    content = content.Replace(to_strBig[i], strBig[i]);
                }
            }

            return content;
        }

        /// <summary>
        /// 对比两个字符串的ASCII值大小，要是X1>X2,将X1,X2交换        
        /// </summary>
        public static void CompareStringASCII(ref string x1, ref string x2)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(x1) || GlobalObject.GeneralFunction.IsNullOrEmpty(x2))
                return;

            string y1 = x1.ToUpper();
            string y2 = x2.ToUpper();
            int iLength = y1.Length;

            if (y2.Length < iLength)
                iLength = y2.Length;

            for (int i = 0; i < iLength; i++)
            {
                int iASC1 = (int)y1[i];
                int iASC2 = (int)y2[i];

                if (iASC1 > iASC2)
                {
                    string tmp = x1;

                    x1 = x2;
                    x2 = tmp;
                    break;
                }

                if (iASC1 < iASC2)
                    break;
            }
        }

        /// <summary>
        /// 查找出text中含有spilt字符串从第几个字符结束的位置数组
        /// </summary>
        /// <param name="text">"12345678901234567890"</param>
        /// <param name="spilt">"12"</param>
        /// <returns>2，13</returns>
        public static int[] DoStringIndexArray(string text, string spilt)
        {
            int[] ret = null;

            try
            {
                int iStart = 0;
                int iEnd = text.Length - 1;
                int spiltLength = spilt.Length;
                ArrayList list = new ArrayList();

                while (iStart <= iEnd)
                {
                    int index = text.IndexOf(spilt, iStart);
                    iStart = index + spiltLength;
                    if (iStart <= iEnd)
                    {
                        list.Add(iStart);
                    }
                }
                ret = new int[list.Count];
                for (int i = 0; i < ret.Length; i++)
                {
                    ret[i] = Convert.ToInt32(list[i]);
                }
            }
            catch
            {
                ret = null;
            }
            return ret;
        }


    }
}