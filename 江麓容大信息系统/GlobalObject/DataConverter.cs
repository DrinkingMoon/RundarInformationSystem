/******************************************************************************
 * ��Ȩ���� (c) 2006-2010, С����ҵ�����ݴ��������ι�˾
 *
 * �ļ�����:  DataConverter.cs
 * ����    :  ��ʯ��    �汾: v1.00    ����: 2014/05/22
 * ����ƽ̨:  Visual C# 2008
 * ����    :  �ֿ�������
 *----------------------------------------------------------------------------
 * ���� : ����ת������
 * ���� : ��ת�������ṩ��������ת��������DataTable�����ȫ�ǰ��ת����
 * ע��: �����б�ת��ΪDataTable��DataTableת��Ϊ�����б�.
 *       �ֶβ����ɶ����PropertyName����.
 *       ����ģ������������������ֶ���һ��, ������Сдһ��.
 *----------------------------------------------------------------------------
 * ������Ϣ: �μ�ϵͳ'������ĵ�'
 *----------------------------------------------------------------------------
 * ��ʷ��¼:
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
    /// ����ת�����ߣ���DataTable�����֮����л���ת��
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
        /// ��������˳�����������
        /// </summary>
        public static IList<string> GetColumnNames(this DataTable dt)
        {
            DataColumnCollection dcc = dt.Columns;

            //���ڼ����е�Ԫ����ȷ���ģ����Կ���ָ��Ԫ�صĸ�����ϵͳ�Ͳ���������Ŀռ䣬Ч�ʻ�ߵ�

            IList<string> list = new List<string>(dcc.Count);

            foreach (DataColumn dc in dcc)
            {
                list.Add(dc.ColumnName);
            }

            return list;
        }

        /// <summary>
        /// �жϲ����Ƿ�������������
        /// </summary>
        /// <param name="d">Ҫ�жϵ�����</param>
        /// <returns>�����������ͷ���true</returns>
        private static bool IsDate(string d)
        {
            DateTime d1;

            double d2;

            return !double.TryParse(d, out d2) && DateTime.TryParse(d, out d1);
        }

        /// <summary>
        /// �����ഴ����ṹ
        /// </summary>
        /// <param name="t">Ҫ������ṹ����������</param>
        /// <returns>�����ı�</returns>
        public static DataTable CreateTable(Type t)
        {
            return BuiltTable(t.GetProperties());
        }

        /// <summary>
        /// ���ݶ�������Դ������ݱ�
        /// </summary>
        /// <param name="pinfo">��������</param>
        /// <returns>�����������鴴���ı�</returns>
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
        /// Object to Object. ��һ������ת��Ϊָ�����͵Ķ���.
        /// ע��: destination�ڵ�Property������source�ڴ���.
        /// </summary>
        /// <param name="source">Ҫת����Դ����</param>
        /// <param name="destination">Ŀ���������</param>
        /// <returns>ת����Ķ���</returns>
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
        /// ָ�������Ƿ������ǳ����
        /// </summary>
        /// <param name="propValue">���ж��ܷ�����ǳ�����Ķ���</param>
        /// <returns>������ǳ��������true</returns>
        private static bool CanShallowCopyProperty(object propValue)
        {
            if (propValue == null)
                return true;

            if (propValue.GetType().IsValueType || propValue is string)
                return true;

            return false;
        }

        /// <summary>
        /// ���ƶ�������.
        /// </summary>
        /// <param name="source">Ҫת����Դ����</param>
        /// <param name="destination">Ŀ���������</param>
        /// <returns>ת����Ķ���</returns>
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
        /// ���ƶ���. ǳ����.
        /// </summary>
        /// <param name="source">Ҫ��¡��Դ����</param>
        /// <returns>���ƺ��Դ����</returns>
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
        /// ����һ����������.
        /// </summary>
        /// <param name="source">Ҫ��¡��Դ�����б�</param>
        /// <returns>���ƺ��Դ�����б�</returns>
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
        /// ��ȡ����ָ�����Ե�ֵ
        /// </summary>
        /// <param name="obj">Ҫ��ȡ���ݵĶ���</param>
        /// <param name="property">Ҫ��ȡ���ݵ���������</param>
        /// <returns>��ȡ����������ֵ��ʧ�ܷ���null</returns>
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
        /// �Ӷ���������ȡ��ĳһ������. ���ض���ָ����������(returnPropName)��ֵ
        /// </summary>
        /// <param name="objects">Ҫȡ��������б�</param>
        /// <param name="keyPropName">���жϵ���������</param>
        /// <param name="keyValue">���жϵ�����ֵ</param>
        /// <param name="returnPropName">������Ҫ��ĵ������л�ȡֵ����Ӧ����������</param>
        /// <returns>�ɹ��򷵻ش�����Ҫ��ĵ������л�ȡָ���������ƶ�Ӧ����ֵ��ʧ�ܷ���null</returns>
        public static object GetObjectValueByKey(IList objects, string keyPropName, object keyValue, string returnPropName)
        {
            object o = GetObjectByKey(objects, keyPropName, keyValue);

            if (o != null)
                return GetValueOfObject(o, returnPropName);
            else
                return null;
        }

        /// <summary>
        /// �ݶ���������ȡ��ĳһ������. ����ָ���ؼ��ֶ�����(keyPropName)��ֵ(keyValue).
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
        /// ���Ҷ������ָ������.
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
        /// ���ö���ĳ�����Ե�ֵ
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
        /// ����IList���󴴽����ݱ�
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

                DataTable table = BuiltTable(pinfo);//������
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
        /// ���ݱ�ת��ΪList����
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
        /// ���ݶ��������ȡ�ֶε�ֵ
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
        /// ��Դ���ж���ͬ�ֶ������и�ֵ
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
        /// ������(DataRow)ת��Ϊ����,�����Type��type��������.
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
                    //�ֶ������������������Ÿ�ֵ
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
        /// ArrayListת��Ϊ��������.
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
        /// ��������ת��ΪArrayList.
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
        /// ��������ת��ΪArrayList.
        /// </summary>
        public static ArrayList ToArrayList(object[] source)
        {
            if (null != source)
                return new ArrayList((ICollection)source);
            else //�����Դ����Ϊnull,����һ���յ�ArrayList.
                return new ArrayList();
        }

        /// <summary>
        /// ���ַ����Զ��ŷָ�ת�������ݿ��ʽin('a','b') 
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
        /// ���ַ����Զ��ŷָ�ת�������ݿ��ʽ''a'',''b'' 
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
        /// ��tableת��dataset
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
        /// �޸ĶԱ��ĳ�е�ֵ
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="value"></param>
        public static bool UpdateTableCol(DataTable dt, string fieldName, object value)
        {
            try
            {
                if (dt.Columns.IndexOf(fieldName) < 0)
                    throw new Exception("��û��" + fieldName + "�У�");

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
        /// �Զ��ŷָ��ַ�������������
        /// �����һ�������һ���ַ�Ϊ, ȥ��
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
        /// ��һ���е���������һ������
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
    /// ���ַ����ַ����Ĵ���
    /// </summary>
    public class DOString
    {
        /// <summary>
        /// ���ַ�ת�ɴ�д��ȥ���߿ո�
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
        /// ����һ����� ȥ���ҿո� ��д���ַ���
        /// </summary>
        public static string ToTUBS(object o)
        {
            return BigToSmall(o.ToString().Trim().ToUpper(), CodeWidth.HalfWidth );
        }

        /// <summary>
        /// �ж��ַ��Ƿ������֡��Ƿ���true 
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
        /// ȥ��ǧ�ֺ�
        /// </summary>
        public static object OffkiloSign(object obj)
        {
            if (obj == null)
                return obj;

            string s = obj.ToString();

            return s.Replace(",", string.Empty);
        }

        /// <summary>
        /// ȫ�ǡ�������ö��
        /// </summary>
        public enum CodeWidth
        {
            /// <summary>
            /// ȫ��
            /// </summary>
            FullWidth,

            /// <summary>
            /// ���
            /// </summary>
            HalfWidth
        }

        /// <summary>
        /// ȫ�ǰ�Ǽ�ת��
        /// </summary>
        /// <param name="content">Ҫת��������</param>
        /// <param name="desCodeWidth">Ŀ������ʽ��FullWidth���ʾ��Դ����תΪȫ��</param>
        /// <returns>����ת�����ֵ</returns>
        public static string BigToSmall(string content, CodeWidth desCodeWidth)
        {
            string strBig, to_strBig;

            strBig = "���£ãģţƣǣȣɣʣˣ̣ͣΣϣУѣңӣԣգ֣ףأ٣ڣ�������������������������롺���磯�������������������������������������";

            to_strBig = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 '��������$/abcdefghijklmnopqrstuvwxyz.*";
            
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
        /// �Ա������ַ�����ASCIIֵ��С��Ҫ��X1>X2,��X1,X2����        
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
        /// ���ҳ�text�к���spilt�ַ����ӵڼ����ַ�������λ������
        /// </summary>
        /// <param name="text">"12345678901234567890"</param>
        /// <param name="spilt">"12"</param>
        /// <returns>2��13</returns>
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