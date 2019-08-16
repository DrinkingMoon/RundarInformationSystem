using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GlobalObject;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 数据显示控件列宽控制类
    /// </summary>
    public class ColumnWidthControl
    {
        /// <summary>
        /// 参数对象
        /// </summary>
        static Dictionary<string, object> m_dicFormParams;

        /// <summary>
        /// XML配置文件操作类
        /// </summary>
        static MultiLevelXmlParams m_xmlParams;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns>初始化成功返回true</returns>
        static public bool Init()
        {
            try
            {
                m_xmlParams = new MultiLevelXmlParams(Environment.CurrentDirectory + "\\FaceParams.xml");
                m_dicFormParams = m_xmlParams.GetParams();

                return true;
            }
            catch //(Exception err)
            {
                //MessageDialog.ShowErrorMessage(err.Message);
            }

            return false;
        }

        /// <summary>
        /// 保存数据到配置文件
        /// </summary>
        static public void Save()
        {
            if (m_xmlParams != null)
            {
                m_xmlParams.SaveParams(m_dicFormParams, false);
            }
        }

        /// <summary>
        /// 控制DataGridView控件列宽、列是否显示
        /// </summary>
        /// <param name="formName">窗体名称</param>
        /// <param name="dataGridView">要控制的控件</param>
        static public void SetDataGridView(string formName, DataGridView dataGridView)
        {
            DataGridViewColumnCollection columns = dataGridView.Columns;
            Dictionary<string, object> columnParams = new Dictionary<string, object>();

            if (m_dicFormParams == null)
            {
                return;
            }

            if (m_dicFormParams.ContainsKey(formName))
            {
                columnParams = m_dicFormParams[formName] as Dictionary<string, object>;
            }
            else
            {
                columnParams = m_dicFormParams["数据显示控件列宽"] as Dictionary<string, object>;
            }

            int totalWidth = 0;

            try
            {
                for (int i = 0; i < dataGridView.Columns.Count; i++)
                {
                    string colName = dataGridView.Columns[i].Name;

                    if (columnParams.ContainsKey(colName))
                    {
                        Dictionary<string, string> paramItem = columnParams[colName] as Dictionary<string, string>;

                        dataGridView.Columns[i].Width = Int32.Parse(paramItem["width"]);
                        dataGridView.Columns[i].Visible = bool.Parse(paramItem["visible"]);
                    }
                    else
                    {
                        dataGridView.Columns[i].Width = 120;
                    }

                    totalWidth += dataGridView.Columns[i].Width;
                }

                if (totalWidth < dataGridView.Width && dataGridView.Width > 100)
                {
                    double factor = 1.0 * (dataGridView.Width - 80) / totalWidth;

                    for (int i = 0; i < dataGridView.Columns.Count; i++)
                    {
                        dataGridView.Columns[i].Width = (int)(dataGridView.Columns[i].Width * factor);
                    }
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("GeneralControl.SetDataGridView：{0}", err.Message);
            }

        }

        /// <summary>
        /// 保存列宽等参数
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="columnName"></param>
        static public void SaveColumnParams(string formName, DataGridViewColumn column)
        {
            Dictionary<string, object> formParams;
            Dictionary<string, string> columnParams;

            if (m_dicFormParams != null)
            {
                if (!m_dicFormParams.ContainsKey(formName))
                {
                    formParams = new Dictionary<string, object>();
                    m_dicFormParams.Add(formName, formParams);
                }
                else
                {
                    formParams = m_dicFormParams[formName] as Dictionary<string, object>;
                }

                if (!formParams.ContainsKey(column.Name))
                {
                    columnParams = new Dictionary<string, string>();
                    formParams.Add(column.Name, columnParams);
                }
                else
                {
                    columnParams = formParams[column.Name] as Dictionary<string, string>;
                }

                columnParams["width"] = column.Width.ToString();
                columnParams["visible"] = column.Visible.ToString();
            }
        }
    }
}
