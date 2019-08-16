using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using AsynSocketService;
using System.Net;
using System.Net.Sockets;
using SocketCommDefiniens;
using ServerRequestProcessorModule;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 成品档案查询界面
    /// </summary>
    public partial class 成品档案查询 : Form
    {
        /// <summary>
        /// 电子档案服务组件
        /// </summary>
        IElectronFileServer m_serverElectronFile = ServerModuleFactory.GetServerModule<IElectronFileServer>();

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 营销服务组件
        /// </summary>
        ISellIn m_sell = ServerModuleFactory.GetServerModule<ISellIn>();

        public 成品档案查询(string strProductCode)
        {
            InitializeComponent();

            dataGridView1.DataSource = m_serverElectronFile.GetProductElectronFile(strProductCode);

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name,
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
                m_dataLocalizer.OnlyLocalize = true;
                panel1.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }
        }
    }
}
