using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Collections;

namespace DBOperate
{
    internal class XmlExpParameter
    {
        #region Private Fields

        /// <summary>
        /// XML文件名称
        /// </summary>
        string m_xmlFileName;

        /// <summary>
        /// 父节点名称
        /// </summary>
        string m_parentNodeName;

        #endregion

        #region Properties

        /// <summary>
        /// 获取或设置配置文件名称
        /// </summary>
        public string FileName
        {
            set
            {
                Debug.Assert(value != null && value.Length > 0);

                m_xmlFileName = Application.StartupPath + "//" + value + ".xml";
            }

            get { return m_xmlFileName; }
        }

        /// <summary>
        /// 获取或设置要获取参数的节点的父节点名称
        /// </summary>
        public string ParentNodeName
        {
            set
            {
                Debug.Assert(value != null && value.Length > 0);

                m_parentNodeName = value + '/';
            }

            get { return m_parentNodeName; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileName">要进行读写的配置文件的名称</param>
        public XmlExpParameter(string fileName)
        {
            FileName = fileName;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="nodeName">具有所需参数的节点名称</param>
        /// <returns>成功返回参数值, 失败抛出异常</returns>
        public string GetParameter(string nodeName)
        {
            if (!File.Exists(FileName))
            {
                throw new Exception("找不到配置文件" + FileName);
            }
            else
            {
                XmlDocument doc = new XmlDocument();

                doc.Load(FileName);

                XmlNode parameterNode = doc.SelectSingleNode(ParentNodeName + nodeName);

                if (parameterNode == null)
                {
                    throw new Exception("找不到对应的参数节点：" + nodeName);
                }

                return parameterNode.InnerText;
            }
        }

        /// <summary>
        /// 存储参数
        /// </summary>
        /// <param name="nodeName">参数名称</param>
        /// <param name="value">参数值</param>
        public void SaveParameter(string nodeName, string value)
        {
            //判断配置文件是否存在
            if (!File.Exists(FileName))
            {
                throw new Exception("找不到配置文件" + FileName);
            }

            XmlDocument doc = new XmlDocument();

            doc.Load(FileName);

            XmlNode parameterNode = doc.SelectSingleNode(ParentNodeName + nodeName);

            if (parameterNode == null)
            {
                throw new Exception("找不到对应的参数节点：" + nodeName);
            }

            parameterNode.InnerText = value;

            doc.Save(FileName);
        }

        /// <summary>
        /// 加载试验参数
        /// </summary>
        /// <returns>成功返回参数列表, 没有试验参数返回null, 其它抛出异常</returns>
        public Hashtable LoadParams()
        {
            if (!File.Exists(FileName))
            {
                return null;
            }
            else
            {
                XmlDocument doc = new XmlDocument();

                doc.Load(FileName);

                XmlNode parentNode = doc.SelectSingleNode("Params");

                if (parentNode == null)
                {
                    return null;
                }
                else if (!parentNode.HasChildNodes)
                {
                    return null;
                }

                Hashtable result = new Hashtable();

                for (int i = 0; i < parentNode.ChildNodes.Count; i++)
                {
                    if ((parentNode.ChildNodes[i].SelectSingleNode("Name") == null) ||
                        (parentNode.ChildNodes[i].SelectSingleNode("Value") == null))
                    {
                        continue;
                    }

                    result.Add(parentNode.ChildNodes[i].SelectSingleNode("Name").InnerText,
                                 parentNode.ChildNodes[i].SelectSingleNode("Value").InnerText);
                }

                return result;
            }
        }

        /// <summary>
        /// 存储实验参数
        /// </summary>
        /// <param name="table">参数列表</param>
        public void SaveParams(Hashtable table)
        {
            XmlDocument doc = new XmlDocument();

            //判断配置文件是否存在
            if (File.Exists(FileName))
            {
                //加载配置文件
                doc.Load(FileName);
            }
            else
            {
                //新建节点（配置文件不存在）
                XmlNode declearNode = doc.CreateNode(XmlNodeType.XmlDeclaration, "", "");

                doc.AppendChild(declearNode);
            }

            XmlNode parentNode = doc.SelectSingleNode("Params");

            if (parentNode == null)
            {
                parentNode = doc.CreateNode(XmlNodeType.Element, "Params", "");

                doc.AppendChild(parentNode);
            }

            foreach (DictionaryEntry dic in table)
            {
                XmlNode dataNode = null;

                foreach (XmlNode node in parentNode.ChildNodes)
                {
                    XmlNode nameNode = node.SelectSingleNode("Name");

                    if (nameNode == null)
                    {
                        continue;
                    }
                    else
                    {
                        if (nameNode.InnerText == dic.Key.ToString())
                        {
                            dataNode = node;

                            break;
                        }
                    }
                }

                if (dataNode == null)
                {
                    //节点不存在 新建节点
                    dataNode = doc.CreateNode(XmlNodeType.Element, "Param", "");

                    parentNode.AppendChild(dataNode);

                    XmlNode nameNode = doc.CreateNode(XmlNodeType.Element, "Name", "");

                    nameNode.InnerText = dic.Key.ToString();

                    dataNode.AppendChild(nameNode);

                    XmlNode valueNode = doc.CreateNode(XmlNodeType.Element, "Value", "");

                    valueNode.InnerText = dic.Value.ToString();

                    dataNode.AppendChild(valueNode);
                }
                else
                {
                    XmlNode nameNode = dataNode.SelectSingleNode("Name");

                    XmlNode valueNode = dataNode.SelectSingleNode("Value");

                    if (nameNode != null)
                    {
                        nameNode.InnerText = dic.Key.ToString();
                    }

                    if (valueNode != null)
                    {
                        valueNode.InnerText = dic.Value.ToString();
                    }
                }
            }

            doc.Save(FileName);
        }

        #endregion

    }
}
