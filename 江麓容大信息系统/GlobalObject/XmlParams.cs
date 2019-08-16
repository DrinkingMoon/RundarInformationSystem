/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  XmlParams.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/06/22
 * 开发平台:  Visual C# 2005
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 从XML配置文件中读取系统所需的参数
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/06/22 8:55:07 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Diagnostics;

namespace GlobalObject
{
    // 示例
    //<?xml version="1.0"?>
    //<parms>
    //  <parm name="电机通信端口">
    //    <parmitem value="9600">波特率</parmitem>
    //    <parmitem value="1">端口号</parmitem>
    //    <parmitem value="8">数据位数</parmitem>
    //    <parmitem value="1">停止位</parmitem>
    //  </parm>
    //  <parm name="TCU通信端口">
    //    <parmitem value="115200">波特率</parmitem>
    //    <parmitem value="2">端口号</parmitem>
    //  </parm>
    //  <parm name="空载磨合前进档试验参数">
    //    <parmitem value="2.432">速比上限值</parmitem>
    //    <parmitem value="0.432">速比下限值</parmitem>
    //    <parmitem value="D">目标档位</parmitem>
    //  </parm>
    //</parms>

    /// <summary>
    /// 从XML文件中读取配置参数及保存参数到配置文件
    /// </summary>
    public class XmlParams
    {
        /// <summary>
        /// 根节点名称
        /// </summary>
        /// <remarks>注意：△▲XML文件中的各节点名称是区分大小写的▲△</remarks>
        const string RootNodeName = "params";

        /// <summary>
        /// 参数节点名称
        /// </summary>
        const string ParamNodeName = "param";

        /// <summary>
        /// 参数项节点名称
        /// </summary>
        const string ParamItemNodeName = "paramitem";

        /// <summary>
        /// 进行操作的XML配置文件的名称
        /// </summary>
        string m_xmlFileName;

        /// <summary>
        /// 获取或设置进行操作的XML配置文件的名称
        /// </summary>
        public string FileName
        {
            get { return m_xmlFileName; }
            set
            {
                Debug.Assert(!string.IsNullOrEmpty(value), "文件名不能为空或空串");
                m_xmlFileName = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xmlFileName">进行操作的XML配置文件的名称</param>
        public XmlParams(string xmlFileName)
        {
            FileName = xmlFileName;
        }

        /// <summary>
        /// 从XML配置文件中读取参数
        /// </summary>
        /// <returns>成功返回获取到的参数表, 失败返回null</returns>
        public Dictionary<string, Object> GetParams()
        {
            if (!File.Exists(FileName))
            {
                throw new Exception("找不到文件：" + FileName);
            }

            XmlDocument doc = new XmlDocument();

            doc.Load(FileName);

            XmlNode parentNode = doc.SelectSingleNode(RootNodeName);

            if (parentNode == null)
            {
                throw new Exception(string.Format("找不到对应的参数节点 '{0}'", RootNodeName));
            }
            else if (!parentNode.HasChildNodes)
            {
                return null;
            }

            Dictionary<string, Object> result = new Dictionary<string, Object>();

            foreach (XmlNode childNode in parentNode.ChildNodes)
            {
                result.Add(childNode.Attributes[0].Value, GetParam(childNode));
            }

            return result;
        }

        /// <summary>
        /// 获取指定节点下所有一级节点的内容
        /// </summary>
        /// <param name="parentNode">父节点</param>
        /// <returns>返回以字典方式保存的参数表</returns>
        Dictionary<string, string> GetParam(XmlNode parentNode)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (XmlNode childNode in parentNode.ChildNodes)
            {
                XmlAttribute attr = childNode.Attributes[0];

                result.Add(childNode.InnerText, attr.Value);
            }

            return result;
        }

        /// <summary>
        /// 备份XML文件
        /// </summary>
        /// <param name="fileName">要备份的XML文件</param>
        /// <returns>备份后的XML文件名称</returns>
        private string BackupXmlFile(string fileName)
        {
            string bakFile = fileName.Replace(".xml", ".xmlbak");

            // 以覆盖方式备份文件
            File.Copy(fileName, bakFile, true);

            return bakFile;
        }

        /// <summary>
        /// 还原XML文件
        /// </summary>
        /// <param name="fileName">要还原的XML文件</param>
        private void RestoreXmlFile(string bakFileName)
        {
            // 备份前的文件名
            string originalFile = bakFileName.Replace(".xmlbak", ".xml");

            File.Copy(bakFileName, originalFile, true);

            File.Delete(bakFileName);
        }

        /// <summary>
        /// 存储试验参数
        /// </summary>
        /// <param name="dicParams">参数列表</param>
        /// <param name="firenew">是否以全新的方式存储试验参数，如果此参数为真则原文件中的所有旧信息被清除</param>
        public void SaveParams(Dictionary<string, Object> dicParams, bool firenew)
        {
            if (firenew)
            {
                SaveNewParams(dicParams);
            }
            else
            {
                SaveParams(dicParams);
            }
        }

        /// <summary>
        /// 以全新的方式存储字典中包含的试验参数，原文件中的所有旧信息被清除
        /// </summary>
        /// <param name="dicParams">参数列表</param>
        void SaveNewParams(Dictionary<string, Object> dicParams)
        {
            XmlDocument doc = new XmlDocument();

            // 配置文件不存在则新建节点
            XmlNode declearNode = doc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            doc.AppendChild(declearNode);

            XmlNode parentNode = doc.CreateNode(XmlNodeType.Element, RootNodeName, "");
            doc.AppendChild(parentNode);
            
            foreach (KeyValuePair<string, Object> param in dicParams)
            {
                XmlNode dataNode = doc.CreateNode(XmlNodeType.Element, ParamNodeName, "");
                Dictionary<string, string> dicChildParam = param.Value as Dictionary<string, string>;

                parentNode.AppendChild(dataNode);

                XmlNode attr = doc.CreateNode(XmlNodeType.Attribute, "name", null);
                attr.Value = param.Key;
                dataNode.Attributes.SetNamedItem(attr);

                SaveParam(doc, dataNode, dicChildParam);
            }

            doc.Save(FileName);
        }

        /// <summary>
        /// 存储试验参数(如果字典中没有更新的参数将予以保留)
        /// </summary>
        /// <param name="dicParams">参数列表</param>
        void SaveParams(Dictionary<string, Object> dicParams)
        {
            XmlDocument doc = new XmlDocument();

            #region 2017-09-30 夏石友, 保存前先备份原文件，防止操作失败的现象
            string bakFileName = "";

            if (File.Exists(FileName))
            {
                bakFileName = BackupXmlFile(FileName);
            }

            #endregion

            try
            {
                // 加载配置文件
                doc.Load(FileName);
            }
            catch (Exception)
            {
                // 配置文件不存在则新建节点
                XmlNode declearNode = doc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
                doc.AppendChild(declearNode);
            }

            XmlNode parentNode = doc.SelectSingleNode(RootNodeName);

            if (parentNode == null)
            {
                parentNode = doc.CreateNode(XmlNodeType.Element, RootNodeName, "");
                doc.AppendChild(parentNode);
            }

            foreach (KeyValuePair<string, Object> param in dicParams)
            {
                XmlNode dataNode = null;

                foreach (XmlNode node in parentNode.ChildNodes)
                {
                    if (node.Attributes.Count > 0)
                    {
                        if (node.Attributes[0].Value == param.Key.ToString())
                        {
                            dataNode = node;
                            break;
                        }
                    }
                }

                Dictionary<string, string> dicChildParam = param.Value as Dictionary<string, string>;

                if (dataNode == null)
                {
                    // 节点不存在 新建节点
                    dataNode = doc.CreateNode(XmlNodeType.Element, ParamNodeName, "");

                    parentNode.AppendChild(dataNode);

                    XmlNode attr = doc.CreateNode(XmlNodeType.Attribute, "name", null);
                    attr.Value = param.Key;
                    dataNode.Attributes.SetNamedItem(attr);

                    SaveParam(doc, dataNode, dicChildParam);
                }
                else
                {
                    dataNode.Attributes[0].Value = param.Key;

                    bool bFind;

                    foreach (KeyValuePair<string, string> paramItem in dicChildParam)
                    {
                        bFind = false;

                        foreach (XmlNode node in dataNode.ChildNodes)
                        {
                            if (paramItem.Key.CompareTo(node.InnerText) == 0)
                            {
                                bFind = true;
                                node.Attributes[0].Value = paramItem.Value;
                                break;
                            }
                        }

                        if (!bFind)
                        {
                            SaveParam(doc, dataNode, paramItem.Key, paramItem.Value);
                        }
                    }

                }
            }

            doc.Save(FileName);

            #region 2017-09-30 夏石友, 保存前先备份原文件，防止操作失败的现象

            try
            {
                doc.Load(FileName);
            }
            catch (Exception exce)
            {
                RestoreXmlFile(bakFileName);
                throw exce;
            }

            #endregion
        }

        /// <summary>
        /// 将参数字典中的所有项以子节点的方式添加到父节点中
        /// </summary>
        /// <param name="doc">XML文档对象</param>
        /// <param name="parentNode">XML父节点对象</param>
        /// <param name="paramItems">参数字典</param>
        void SaveParam(XmlDocument doc, XmlNode parentNode, Dictionary<string, string> paramItems)
        {
            foreach (KeyValuePair<string, string> item in paramItems)
            {
                SaveParam(doc, parentNode, item.Key, item.Value);
            }
        }

        /// <summary>
        /// 在父节点中添加一子节点
        /// </summary>
        /// <param name="doc">XML文档对象</param>
        /// <param name="parentNode">XML父节点对象</param>
        /// <param name="paramName">参数名</param>
        /// <param name="paramValue">参数值</param>
        void SaveParam(XmlDocument doc, XmlNode parentNode, string paramName, string paramValue)
        {
            XmlNode childNode = doc.CreateNode(XmlNodeType.Element, ParamItemNodeName, "");

            childNode.InnerText = paramName;

            XmlNode attr = doc.CreateNode(XmlNodeType.Attribute, "value", null);
            attr.Value = paramValue;
            childNode.Attributes.SetNamedItem(attr);

            parentNode.AppendChild(childNode);
        }
    }
}


