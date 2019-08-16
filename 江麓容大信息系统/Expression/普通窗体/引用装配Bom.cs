using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;
using Service_Manufacture_Storage;

namespace Expression
{
    /// <summary>
    /// 引用装配BOM信息界面
    /// </summary>
    public partial class 引用装配Bom : Form
    {
        /// <summary>
        /// BOM 服务组件
        /// </summary>
        ServerModule.IBomServer m_bomServer = ServerModule.ServerModuleFactory.GetServerModule<ServerModule.IBomServer>();

        /// <summary>
        /// 装配BOM 服务组件
        /// </summary>
        ServerModule.IAssemblingBom m_assemblingBom = ServerModule.ServerModuleFactory.GetServerModule<ServerModule.IAssemblingBom>();

        /// <summary>
        /// 服务组件
        /// </summary>
        ServerModule.IBomServer m_bomService = ServerModule.ServerModuleFactory.GetServerModule<ServerModule.IBomServer>();

        /// <summary>
        /// 发料清单服务组件m_scrapBillServer
        /// </summary>
        Service_Manufacture_Storage.IProductOrder m_billServer = Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<Service_Manufacture_Storage.IProductOrder>();

        /// <summary>
        /// 是否选择了产品类型
        /// </summary>
        private bool m_blIsSelectProductType = false;

        public bool BlIsSelectProductType
        {
            get { return m_blIsSelectProductType; }
            set { m_blIsSelectProductType = value; }
        }

        public 引用装配Bom()
        {
            InitializeComponent();

            this.cmbProductType.SelectedIndexChanged -= new System.EventHandler(this.cmbProductType_SelectedIndexChanged);
            cmbProductType.DataSource = m_bomService.GetAssemblyTypeList();
            this.cmbProductType.SelectedIndexChanged += new System.EventHandler(this.cmbProductType_SelectedIndexChanged);
        }

        private void cmbProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitViewData();
        }

        /// <summary>
        /// 初始化视图数据
        /// </summary>
        /// <param name="lstAssemblingBom">获取到的装配BOM信息列表</param>
        void InitViewData()
        {
            dataGridView1.DataSource = m_billServer.GetProductOrder(cmbProductType.Text, CE_DebitScheduleApplicable.正常装配);
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dataGridView1.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView1.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            m_blIsSelectProductType = true;
            this.Close();
        }
    }
}
