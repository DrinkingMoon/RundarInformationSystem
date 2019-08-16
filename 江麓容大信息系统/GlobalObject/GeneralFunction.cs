
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq.Expressions;
using System.Data.OleDb;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GlobalObject
{
    /// <summary> 
    /// 通用功能以及通用方法类
    /// </summary>
    public static class GeneralFunction
    {
        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="tpConvertsionType">类型</param>
        /// <returns>返回值</returns>
        public static object ChangeType(this object value, Type tpConvertsionType)
        {

            if (tpConvertsionType.IsGenericType 
                && tpConvertsionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null || value.ToString().Length == 0)
                {
                    return null;
                }

                NullableConverter nullableConverter = new NullableConverter(tpConvertsionType);

                tpConvertsionType = nullableConverter.UnderlyingType;
            }

            return Convert.ChangeType(value, tpConvertsionType);
        }

        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="tpConvertsionType">类型名称</param>
        /// <returns>返回值</returns>
        public static object ChangeType(this object value, string typeName)
        {

            Type tpConvertsionType;

            if (typeName == "Int")
            {
                tpConvertsionType = System.Type.GetType("System.Int32");
            }
            else if (typeName.Contains("Int32"))
            {
                tpConvertsionType = System.Type.GetType("System.Int32");
            }
            else if (typeName.Contains("Int16"))
            {
                tpConvertsionType = System.Type.GetType("System.Int16");
            }
            else if (typeName.Contains("Int64"))
            {
                tpConvertsionType = System.Type.GetType("System.Int64");
            }
            else if (typeName.Contains("Boolean"))
            {
                tpConvertsionType = System.Type.GetType("System.Boolean");
            }
            else if (typeName.Contains("String"))
            {
                tpConvertsionType = System.Type.GetType("System.String");
            }
            else if (typeName.Contains("Decimal"))
            {
                tpConvertsionType = System.Type.GetType("System.Decimal");
            }
            else if (typeName.Contains("DateTime"))
            {
                tpConvertsionType = System.Type.GetType("System.DateTime");
            }
            else if (typeName.Contains("Byte"))
            {
                tpConvertsionType = System.Type.GetType("System.Byte");
            }
            else if (typeName.Contains("Double"))
            {
                tpConvertsionType = System.Type.GetType("System.Double");
            }
            else if (typeName.Contains("Guid"))
            {
                tpConvertsionType = System.Type.GetType("System.Guid");
            }
            else if (typeName.Contains("Single"))
            {
                tpConvertsionType = System.Type.GetType("System.Single");
            }
            else
            {
                tpConvertsionType = System.Type.GetType("System.Object");
            }

            return ChangeType(value, tpConvertsionType);
        }

        #region LINQ 结果集转 DataTable

        /// <summary>
        /// 将LINQ的查询结果转换为DataTable最简单的实现方法
        /// </summary>
        /// <param name="queryable">要转换的结果集</param>
        /// <returns>转换后的表</returns>
        public static DataTable ConvertToDataTable(System.Linq.IQueryable queryable)
        {
            DataTable dt = new DataTable();

            var props = queryable.ElementType.GetProperties();

            foreach (PropertyInfo pi in props)
            {
                Type colType = pi.PropertyType;

                if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    colType = colType.GetGenericArguments()[0];
                }

                dt.Columns.Add(new DataColumn(pi.Name, colType));
            }

            foreach (var item in queryable)
            {
                DataRow dr = dt.NewRow();

                foreach (System.Reflection.PropertyInfo pi in props)
                {
                    if (pi.GetValue(item, null) != null)
                    {
                        if (pi.GetValue(item, null) != null)
                        {
                            dr[pi.Name] = pi.GetValue(item, null);
                        }
                    }
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }

        /// <summary>
        /// 将LINQ的查询结果转换为DataTable最简单的实现方法
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="enumerable">要转换的结果集</param>
        /// <returns>转换后的表</returns>
        public static DataTable ConvertToDataTable<T>(IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return null;
            }

            DataTable dtReturn = new DataTable();

            // 列名 
            PropertyInfo[] oProps = enumerable.AsQueryable().ElementType.GetProperties();

            foreach (PropertyInfo pi in oProps)
            {
                Type colType = pi.PropertyType;

                if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    colType = colType.GetGenericArguments()[0];
                }

                dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
            }

            foreach (T rec in enumerable)
            {
                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }

            return dtReturn;
        }

        #endregion

        /// <summary>
        /// DataGridView转DataTable
        /// </summary>
        /// <param name="dgv">DataGridView</param>
        /// <returns>返回Table</returns>
        public static DataTable ConvertToDataTable(DataGridView dgv)
        {
            if (dgv == null)
            {
                return null;
            }

            DataTable resultTable = new DataTable();

            foreach (DataGridViewColumn dgvc in dgv.Columns)
            {
                if (dgvc.Visible)
                {
                    resultTable.Columns.Add(dgvc.Name);
                }
            }

            foreach (DataGridViewRow dgvr in dgv.Rows)
            {
                DataRow dr = resultTable.NewRow();

                foreach (DataColumn dc in resultTable.Columns)
                {
                    dr[dc.ColumnName] = dgvr.Cells[dc.ColumnName].Value;
                }

                resultTable.Rows.Add(dr);
            }

            return resultTable;
        }

        /// <summary>
        /// 克隆DataGridView
        /// </summary>
        /// <param name="dgv"></param>
        /// <returns></returns>
        public static DataGridView CloneDataGridView_Grid(DataGridView dgv)
        {
            try
            {
                DataGridView ResultDGV = new DataGridView();
                ResultDGV.ColumnHeadersDefaultCellStyle = dgv.ColumnHeadersDefaultCellStyle.Clone();
                DataGridViewCellStyle dtgvdcs = dgv.RowsDefaultCellStyle.Clone();
                dtgvdcs.BackColor = dgv.DefaultCellStyle.BackColor;
                dtgvdcs.ForeColor = dgv.DefaultCellStyle.ForeColor;
                dtgvdcs.Font = dgv.DefaultCellStyle.Font;
                ResultDGV.RowsDefaultCellStyle = dtgvdcs;
                ResultDGV.AllowUserToAddRows = false;
                ResultDGV.AllowUserToDeleteRows = dgv.AllowUserToDeleteRows;
                ResultDGV.AllowUserToOrderColumns = dgv.AllowUserToOrderColumns;
                ResultDGV.AllowUserToResizeColumns = dgv.AllowUserToResizeColumns;
                ResultDGV.AllowUserToResizeRows = dgv.AllowUserToResizeRows;
                ResultDGV.AlternatingRowsDefaultCellStyle = dgv.AlternatingRowsDefaultCellStyle.Clone();

                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    DataGridViewColumn DTGVC = dgv.Columns[i].Clone() as DataGridViewColumn;
                    DTGVC.ValueType = dgv.Columns[i].ValueType;
                    DTGVC.DisplayIndex = dgv.Columns[i].DisplayIndex;

                    if (DTGVC.CellType == null)
                    {
                        DTGVC.CellTemplate = new DataGridViewTextBoxCell();
                        ResultDGV.Columns.Add(DTGVC);
                    }
                    else
                    {
                        ResultDGV.Columns.Add(DTGVC);
                    }
                }

                return ResultDGV;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// 克隆DataGridView
        /// </summary>
        /// <param name="dgv"></param>
        /// <returns></returns>
        public static DataGridView CloneDataGridView_All(DataGridView dgv)
        {
            try
            {
                DataGridView ResultDGV = new DataGridView();
                ResultDGV.ColumnHeadersDefaultCellStyle = dgv.ColumnHeadersDefaultCellStyle.Clone();
                DataGridViewCellStyle dtgvdcs = dgv.RowsDefaultCellStyle.Clone();
                dtgvdcs.BackColor = dgv.DefaultCellStyle.BackColor;
                dtgvdcs.ForeColor = dgv.DefaultCellStyle.ForeColor;
                dtgvdcs.Font = dgv.DefaultCellStyle.Font;
                ResultDGV.RowsDefaultCellStyle = dtgvdcs;
                ResultDGV.AllowUserToAddRows = false;
                ResultDGV.AllowUserToDeleteRows = dgv.AllowUserToDeleteRows;
                ResultDGV.AllowUserToOrderColumns = dgv.AllowUserToOrderColumns;
                ResultDGV.AllowUserToResizeColumns = dgv.AllowUserToResizeColumns;
                ResultDGV.AllowUserToResizeRows = dgv.AllowUserToResizeRows;
                ResultDGV.AlternatingRowsDefaultCellStyle = dgv.AlternatingRowsDefaultCellStyle.Clone();

                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    DataGridViewColumn DTGVC = dgv.Columns[i].Clone() as DataGridViewColumn;
                    DTGVC.ValueType = dgv.Columns[i].ValueType;
                    DTGVC.DisplayIndex = dgv.Columns[i].DisplayIndex;

                    if (DTGVC.CellType == null)
                    {
                        DTGVC.CellTemplate = new DataGridViewTextBoxCell();
                        ResultDGV.Columns.Add(DTGVC);
                    }
                    else
                    {
                        ResultDGV.Columns.Add(DTGVC);
                    }
                }

                foreach (DataGridViewRow var in dgv.Rows)
                {
                    DataGridViewRow Dtgvr = var.Clone() as DataGridViewRow;
                    Dtgvr.DefaultCellStyle = var.DefaultCellStyle.Clone();
                    Dtgvr.Cells.Clear();

                    foreach (DataGridViewCell cell in var.Cells)
                    {
                        DataGridViewCell Dtgvcell = cell.Clone() as DataGridViewCell;
                        Dtgvcell.Value = cell.Value;
                        Dtgvr.Cells.Add(Dtgvcell);
                    }

                    if (var.Index % 2 == 0)
                        Dtgvr.DefaultCellStyle.BackColor = ResultDGV.RowsDefaultCellStyle.BackColor;
                    ResultDGV.Rows.Add(Dtgvr);
                }

                return ResultDGV;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }

        #region 序列化

        /// <summary>
        /// 二进制序列化数据对象
        /// </summary>
        /// <param name="obj">未序列化的数据对象</param>
        /// <returns>返回序列化后的byte[]数组</returns>
        static public byte[] SerializeObject(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("null input");
            }

            BinaryFormatter serialzer = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();

            serialzer.Serialize(memoryStream, obj);

            return memoryStream.ToArray();
        }

        /// <summary>
        /// 二进制反序列化数据
        /// </summary>
        /// <param name="input">序列化后的数据</param>
        /// <returns>返回反序列化后的object数据</returns>
        static public object DeserializeObject(byte[] input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("null input");
            }

            MemoryStream menoryStream = new MemoryStream(input);

            menoryStream.Position = 0;

            BinaryFormatter deserialzer = new BinaryFormatter();
            Object obj = deserialzer.Deserialize(menoryStream);

            menoryStream.Close();
            return obj;
        }

        /// <summary>
        /// 对象转指针
        /// </summary>
        /// <param name="arg">要转指针的对象</param>
        /// <param name="length">对象进行二进制序列化后的字节数</param>
        /// <returns>返回数据指针</returns>
        static public IntPtr ClassToIntPtr(object arg, out int length)
        {
            byte[] temp = SerializeObject(arg);

            length = temp.Length;

            IntPtr ptr = Marshal.AllocHGlobal(length);
            Marshal.Copy(temp, 0, ptr, length);

            return ptr;
        }

        /// <summary>
        /// 指针转对象
        /// </summary>
        /// <param name="ptr">要转对象的指针</param>
        /// <param name="length">指针指向的对象进行二进制序列化后的字节数</param>
        /// <returns>返回转换后的对象</returns>
        static public object IntPtrToClass(IntPtr ptr, int length)
        {
            byte[] temp = new byte[length];

            Marshal.Copy(ptr, temp, 0, length);

            return DeserializeObject(temp);
        }

        #endregion 序列化

        #region 根据反射设置对象属性值

        /// <summary>
        /// 设置相应属性的值(只支持基础类型，如：String、Boolean、Int32、Decimal、DateTime)
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="fieldName">属性名</param>
        /// <param name="fieldValue">属性值</param>
        public static void SetValue(object entity, string fieldName, string fieldValue)
        {
            Type entityType = entity.GetType();

            PropertyInfo propertyInfo = entityType.GetProperty(fieldName);

            if (IsType(propertyInfo.PropertyType, "System.String"))
            {
                propertyInfo.SetValue(entity, fieldValue, null);
            }
            else if (IsType(propertyInfo.PropertyType, "System.Boolean"))
            {
                propertyInfo.SetValue(entity, Boolean.Parse(fieldValue), null);
            }
            else if (IsType(propertyInfo.PropertyType, "System.Int32"))
            {
                if (fieldValue != "")
                    propertyInfo.SetValue(entity, int.Parse(fieldValue), null);
                else
                    propertyInfo.SetValue(entity, 0, null);
            }
            else if (IsType(propertyInfo.PropertyType, "System.Decimal"))
            {
                if (fieldValue != "")
                    propertyInfo.SetValue(entity, Decimal.Parse(fieldValue), null);
                else
                    propertyInfo.SetValue(entity, new Decimal(0), null);
            }
            else if (IsType(propertyInfo.PropertyType, "System.Nullable`1[System.DateTime]"))
            {
                if (fieldValue != "")
                {
                    try
                    {
                        propertyInfo.SetValue(
                            entity,
                            (DateTime?)DateTime.ParseExact(fieldValue, "yyyy-MM-dd HH:mm:ss", null), null);
                    }
                    catch
                    {
                        propertyInfo.SetValue(entity, (DateTime?)DateTime.ParseExact(fieldValue, "yyyy-MM-dd", null), null);
                    }
                }
                else
                {
                    propertyInfo.SetValue(entity, null, null);
                }
            }
            else
            {
                throw new Exception("采用反射设置对象属性值时遇到不支持的数据类型");
            }
        }

        /// <summary>
        /// 类型匹配(检查数据类型与给定的类型名称是否一致)
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <param name="typeName">类型名称</param>
        /// <returns>类型匹配返回true, 否则返回false</returns>
        public static bool IsType(Type type, string typeName)
        {
            if (type.ToString() == typeName)
                return true;

            if (type.ToString() == "System.Object")
                return false;

            return IsType(type.BaseType, typeName);
        }

        #endregion 根据反射设置对象属性值

        /// <summary>
        /// 获取对象指定的属性中的值
        /// </summary>
        /// <typeparam name="T">要获取数据的类型</typeparam>
        /// <param name="model">实体集</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>返回object</returns>
        public static object GetItemValue<T>(T model, string propertyName)
        {
            if (model == null)
            {
                return null;
            }

            PropertyInfo[] listPi = model.GetType().GetProperties();

            foreach (PropertyInfo pi in listPi)
            {
                object obj = pi.GetValue(model, null);

                if (pi.Name == propertyName)
                {
                    return obj;
                }
            }

            return null;
        }

        /// <summary>
        /// 从数据集获取实体对象列表
        /// </summary>
        /// <typeparam name="T">要获取数据的类型</typeparam>
        /// <param name="model">实体集</param>
        /// <returns>返回LIST</returns>
        public static List<string> GetItemPropertyName(object model)
        {
            PropertyInfo[] listPi = model.GetType().GetProperties();

            List<string> resultList = new List<string>();

            foreach (PropertyInfo pi in listPi)
            {
                if (pi.PropertyType.Namespace == "System")
                {
                    resultList.Add(pi.Name);
                }
            }

            return resultList;
        }

        public static List<string> GetFeildList<T>()
        {
            List<string> result = new List<string>();

            System.Reflection.PropertyInfo[] listPi = typeof(T).GetProperties();

            foreach (System.Reflection.PropertyInfo pi in listPi)
            {
                if (pi.PropertyType.Namespace == "System")
                {
                    result.Add(pi.Name);
                }
            }

            return result;
        }

        /// <summary>
        /// 根据实体集获取SQL where语句
        /// </summary>
        /// <typeparam name="T">要获取数据的类型</typeparam>
        /// <param name="model">实体集</param>
        /// <returns>成功返回获取到的实体对象列表，失败则抛出异常</returns>
        public static string GetItem<T>(T model)
        {
            PropertyInfo[] listPi = model.GetType().GetProperties();

            string strWhere = "";

            foreach (PropertyInfo pi in listPi)
            {
                object obj = pi.GetValue(model, null);

                if (obj != null && !pi.PropertyType.Name.Contains("List"))
                {
                    strWhere = strWhere + " and " + pi.Name + " = '" + obj.ToString() + "'";
                }
            }

            if (strWhere.Length == 0)
            {
                return null;
            }
            else
            {
                return strWhere.Substring(4);
            }
        }

        /// <summary>
        /// 反射获得实体集
        /// </summary>
        /// <typeparam name="T">实体集</typeparam>
        /// <param name="InputRow">输入DataRow</param>
        /// <returns>返回实体集</returns>
        public static T ReflectiveEntity<T>(DataRow InputRow)
        {
            if (InputRow == null)
            {
                return default(T);
            }

            T model = Activator.CreateInstance<T>();

            foreach (DataColumn dc in InputRow.Table.Columns)
            {
                PropertyInfo pi = model.GetType().GetProperty(dc.ColumnName);

                if (pi != null)
                {
                    if (InputRow[dc.ColumnName] != DBNull.Value)
                    {
                        pi.SetValue(model, InputRow[dc.ColumnName], null);
                    }
                    else
                    {
                        pi.SetValue(model, null, null);
                    }
                }
            }

            return model;
        }

        /// <summary>
        /// 类型转换字符串列表
        /// </summary>
        /// <param name="listT">类型列表</param>
        /// <returns>返回字符串列表</returns>
        public static List<string> ConvertListTypeToStringList<T> (List<T> listT)
        {
            List<string> result = new List<string>();

            foreach (T item in listT)
            {
                result.Add(item.ToString());
            }

            return result;
        }

        /// <summary>
        /// 获得枚举的列表
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns>返回List</returns>
        public static List<string> GetEumnList(Type enumType)
        {
            List<string> result = new List<string>();

            if (enumType.IsEnum)
            {
                result.AddRange(Enum.GetNames(enumType).ToList());
            } 

            return result;
        }

        /// <summary>
        /// 字符串转枚举
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="info">字符串信息</param>
        /// <returns>返回枚举类型</returns>
        public static T StringConvertToEnum<T>(string info)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), info, true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static T ValueConvertToEnum<T>(int objValue)
        {
            try
            {
                List<string> lstTemp = GetEumnList(typeof(T));

                foreach (string item in lstTemp)
                {
                    T tempType = StringConvertToEnum<T>(item);

                    if (objValue == (int)typeof(T).InvokeMember(item, BindingFlags.GetField, null, null, null))
                    {
                        return tempType;
                    }
                }

                return default(T);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 剔除字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回字符串</returns>
        public static string ScreenString(CE_ScreenType type, string str)
        {
            string result = "";

            switch (type)
            {
                case CE_ScreenType.数字:
                    result = System.Text.RegularExpressions.Regex.Replace(str, @"[^\d\d]", "");//不包括小数点
                    //string result = System.Text.RegularExpressions.Regex.Replace(str, @"[^\d.\d]", "");//包括小数点
                    break;
                case CE_ScreenType.字母:
                    result = System.Text.RegularExpressions.Regex.Replace(str, @"[^a-zA-Z]+", "");
                    break;
                case CE_ScreenType.汉字:
                    result = System.Text.RegularExpressions.Regex.Replace(str, @"[^\u4e00-\u9fa5]+", "");
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// 判断是否为数字
        /// </summary>
        /// <param name="oText">字符串</param>
        /// <returns>True 是 False 否</returns>
        public static bool IsNumberic(string oText)
        {
            try
            {
                decimal var1 = Convert.ToDecimal(oText);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static List<TreeNode> GetListNode(DataTable dt, string NodeText,
            string NodeValue, string RootSign, string RootSelect)
        {
            List<TreeNode> lstResult = new List<TreeNode>();

            //得到根节点信息   
            DataRow[] DataRoot = dt.Select(RootSelect);

            //循环遍历根节点信息   
            foreach (DataRow dr in DataRoot)
            {
                //加载根节点   
                TreeNode tn = new TreeNode();
                tn.Text = dr[NodeText].ToString();
                tn.Tag = dr[NodeValue].ToString();
                lstResult.Add(tn);
                tn.Expand();

                //递归加载子节点   
                LoadNodes(tn, dr, NodeText, NodeValue, RootSign, dt);
            }

            return lstResult;
        }

        /// <summary>
        /// 功能树加载
        /// </summary> 
        /// <param name="tv">树形控件</param>   
        /// <param name="dt">数据源控件</param>   
        /// <param name="NodeText">节点显示的字段名</param>   
        /// <param name="NodeValue">表主键编码字段名</param>   
        /// <param name="RootSign">根编码字段名</param>   
        /// <param name="RootSelect">显示根的条件内容</param>   
        public static void LoadTreeViewDt(TreeView tv, DataTable dt, string NodeText, 
            string NodeValue, string RootSign, string RootSelect)
        {
            //得到根节点信息   
            DataRow[] DataRoot = dt.Select(RootSelect);

            //循环遍历根节点信息   
            foreach (DataRow dr in DataRoot)
            {
                //加载根节点   
                TreeNode tn = new TreeNode();
                tn.Text = dr[NodeText].ToString();
                tn.Tag = dr[NodeValue].ToString();
                tv.Nodes.Add(tn);
                tn.Expand();

                //递归加载子节点   
                LoadNodes(tn, dr, NodeText, NodeValue, RootSign, dt);
            }
        }

        /// <summary>
        /// 无限递归添加节点
        /// </summary>
        /// <param name="tn">根节点</param>
        /// <param name="dr">根节点集合</param>
        /// <param name="nodeText">节点显示的字段名</param>
        /// <param name="nodeValue">表主键编码字段名</param>
        /// <param name="rootSign">根编码字段名</param>
        /// <param name="dt">数据源</param>
        static void LoadNodes(TreeNode tn, DataRow dr, string nodeText, string nodeValue, string rootSign, DataTable dt)
        {
            if (dr == null || tn == null) { return; }

            //得到子节点信息   
            DataRow[] DataChild = dt.Select(rootSign + "='" + dr[nodeValue].ToString() + "'");

            if (DataChild != null || DataChild.Length > 0)
            {
                //循环遍历子节点信息   
                foreach (DataRow drChild in DataChild)
                {
                    //加载子节点   
                    TreeNode tnChild = new TreeNode();
                    tnChild.Text = drChild[nodeText].ToString();
                    tnChild.Tag = drChild[nodeValue].ToString();
                    tn.Nodes.Add(tnChild);

                    //形成递归   
                    LoadNodes(tnChild, drChild, nodeText, nodeValue, rootSign, dt);
                }
            }
        }

        /// <summary>
        /// 获取文件类型的枚举
        /// </summary>
        /// <param name="fullDocumentName">文件名</param>
        /// <returns>返回文件类型的枚举</returns>
        public static CE_SystemFileType GetDocumentType(string fullDocumentName)
        {
            string documentType = System.IO.Path.GetExtension(fullDocumentName).Substring(1);
                //(fullDocumentName.Substring(fullDocumentName.LastIndexOf(".") + 1)).ToLower();

            if (documentType == "doc")
            {
                return CE_SystemFileType.Word;
            }
            else if (documentType == "xls")
            {
                return CE_SystemFileType.Excel;
            } 
            if (documentType == "xlsx")
            {
                return CE_SystemFileType.Excel2010;
            }
            else if (documentType == "ppt")
            {
                return CE_SystemFileType.PPT;
            }
            else if (documentType == "pdf")
            {
                return CE_SystemFileType.PDF;
            }
            else
            {
                return CE_SystemFileType.Miss;
            }
        }

        public static void ClearEvent(Control pControl, string pEventName)
        {
            if (pControl == null) return;
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(pEventName)) return;

            BindingFlags mPropertyFlags = BindingFlags.Instance | BindingFlags.Public
                | BindingFlags.Static | BindingFlags.NonPublic;//筛选
            BindingFlags mFieldFlags = BindingFlags.Static | BindingFlags.NonPublic;
            Type controlType = typeof(System.Windows.Forms.Control);
            PropertyInfo propertyInfo = controlType.GetProperty("Events", mPropertyFlags);
            EventHandlerList eventHandlerList = (EventHandlerList)propertyInfo.GetValue(pControl, null);//事件列表
            FieldInfo fieldInfo = (typeof(Control)).GetField("Event" + pEventName, mFieldFlags);

            if (fieldInfo == null)
            {
                return;
            }

            Delegate d = eventHandlerList[fieldInfo.GetValue(pControl)];

            if (d == null) return;
            EventInfo eventInfo = controlType.GetEvent(pEventName);

            foreach (Delegate dx in d.GetInvocationList())
                eventInfo.RemoveEventHandler(pControl, dx);//移除已订阅的pEventName类型事件

        }

        ///  <summary>     
        /// 获取对象事件 zgke@sina.com qq:116149     
        ///  </summary>     
        ///  <param name="p_Object">对象 </param>     
        ///  <param name="p_EventName">事件名 </param>     
        ///  <returns>委托列 </returns>     
        public static Delegate[] GetObjectEventList(object p_Object, string p_EventName)
        {
            FieldInfo _Field = p_Object.GetType().GetField(p_EventName, 
                BindingFlags.NonPublic | BindingFlags.Instance | 
                BindingFlags.Public | BindingFlags.Static);
            if (_Field == null)
            {
                return null;
            }
            object _FieldValue = _Field.GetValue(p_Object);
            if (_FieldValue != null && _FieldValue is Delegate)
            {
                Delegate _ObjectDelegate = (Delegate)_FieldValue;
                return _ObjectDelegate.GetInvocationList();
            }
            return null;
        }

        public static double GetDecimalPointAfter(object obj)
        {
            string str = obj.ToString();
            int count = str.LastIndexOf(".");

            if (count <= 0)
            {
                return 0;
            }

            double d1 = Double.Parse( "0" + str.Substring(count, str.Length - count));
            return d1;
        }

        public static int GetDecimalPointBefore(object obj)
        {
            string str = obj.ToString();
            int count = str.LastIndexOf(".");

            if (count <= 0)
            {
                return Convert.ToInt32(obj);
            }

            int d1 = Int32.Parse(str.Substring(0, count));
            return d1;
        }

        public static bool ParentControlIsExist<T>(Control cl, string parentControlName)
        {
            if (cl.Parent == null)
            {
                return false;
            }
            else
            {
                if (cl.Parent.GetType() == typeof(T))
                {
                    if (cl.Parent.Name == parentControlName)
                    {
                        return true;
                    }
                }

                return ParentControlIsExist<T>(cl.Parent, parentControlName);
            }
        }

        public static void SetRadioButton(object rbName, Control control)
        {
            if (control == null || rbName == null)
            {
                return;
            }

            foreach (Control cl in control.Controls)
            {
                if (cl is RadioButton)
                {
                    ((RadioButton)cl).Checked = ((RadioButton)cl).Text == rbName.ToString() ? true : false;
                }
            }
        }

        public static string GetRadioButton(Control control)
        {
            if (control == null)
            {
                return null;
            }

            foreach (Control cl in control.Controls)
            {
                if (cl is RadioButton)
                {
                    if (((RadioButton)cl).Checked)
                    {
                        return ((RadioButton)cl).Text;
                    }
                }
            }

            return null;
        }

        /// <summary>   
        /// 判断两个日期是否在同一周   
        /// </summary>   
        /// <param name="dtmS">开始日期</param>   
        /// <param name="dtmE">结束日期</param>  
        /// <returns></returns>   
        public static bool IsInSameWeek(DateTime dtmS, DateTime dtmE)
        {
            TimeSpan ts = dtmE - dtmS;
            double dbl = ts.TotalDays;
            int intDow = Convert.ToInt32(dtmE.DayOfWeek);
            if (intDow == 0) intDow = 7;
            if (dbl >= 7 || dbl >= intDow) return false;
            else return true;
        }

        public static string ListToString<T>(List<T> lstInfo, string split)
        {
            if (lstInfo == null)
            {
                return null;
            }

            string result = "";

            foreach (T item in lstInfo)
            {
                if (item != null)
                {
                    result += item.ToString() + split;
                }
            }

            if (result.Length > 0)
            {
                result = result.Substring(0, result.Length - split.Length);
            }

            return result;
        }

        public static string RepeatString(int n, string str)
        {
            string result = "";

            if (n > 0)
            {
                for (int i = 0; i < n; i++)
                {
                    result += str;
                }
            }

            return result;
        }

        public static bool IsNullOrEmpty(string str)
        {
            if (str == null || str.Trim().Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsNullOrEmptyObject(object obj)
        {
            if (obj == null || obj.ToString().Trim().Length == 0)
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
