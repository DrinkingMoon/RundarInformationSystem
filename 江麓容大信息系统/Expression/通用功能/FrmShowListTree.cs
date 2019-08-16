using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    public partial class FrmShowTreeList : Form
    {
        /// <summary>
        /// 全局Table表
        /// </summary>
        DataTable dateStore;

        /// <summary>
        /// 错误string
        /// </summary>
        string m_err;

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 材料类型
        /// </summary>
        IQueryable<S_Depot> m_findDepotType;

        /// <summary>
        /// 库存视图
        /// </summary>
        IQueryable<View_S_Stock> m_findAllSock;

        /// <summary>
        /// 材料类型与管理人关系
        /// </summary>
        IDepotTypeForPersonnel m_DepotTypeForPersonnel = ServerModuleFactory.GetServerModule<IDepotTypeForPersonnel>();

        /// <summary>
        /// 库存表
        /// </summary>
        IStoreServer m_StoreServer = ServerModuleFactory.GetServerModule<IStoreServer>();

        public event GlobalObject.DelegateCollection.DelegateClick ClickEvent;
        private DataRow _DrShowlist = null;

        public DataRow DrShowlist
        {
            get { return _DrShowlist; }
            set { _DrShowlist = value; }
        }

        private int _isReturn = 0;         //是否有数据返回 0:无结果 1:有结果

        public int IsReturn
        {
            get { return _isReturn; }
            set { _isReturn = value; }
        }


        public FrmShowTreeList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FormFindGoods_Load(object sender, EventArgs e)
        {
            if (m_StoreServer.GetAllStore(null, false, out m_findAllSock, out m_err))
            {
                dateStore = GlobalObject.GeneralFunction.ConvertToDataTable<View_S_Stock>(m_findAllSock);
                dataGridView1.DataSource = dateStore;
            }


            m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
            panel3.Controls.Add(m_dataLocalizer);
            m_dataLocalizer.Dock = DockStyle.Bottom;
            
            m_findDepotType = m_DepotTypeForPersonnel.GetDepotTypeBill();
            GlobalObject.GeneralFunction.LoadTreeViewDt(treeView1,
                           m_DepotTypeForPersonnel.ChangeDataTable(GlobalObject.GeneralFunction.ConvertToDataTable<S_Depot>(m_findDepotType)),
                           "DepotName", "DepotCode", "RootSign", "RootSign = 'Root'");


        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode.Tag.ToString() != "0")
            {
                DataTable tbTemp = dateStore.Clone();

                DataRow[] dr = dateStore.Select("仓库 like '" + treeView1.SelectedNode.Tag.ToString() + "%'");

                for (int i = 0; i < dr.Length; i++)
                {
                    tbTemp.ImportRow(dr[i]);
                }

                dataGridView1.DataSource = tbTemp;
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (this.ClickEvent != null)
            {
                DataTable dt = (DataTable)this.dataGridView1.DataSource;
                DataView dv = dt.DefaultView;
                if (dt.Rows.Count <= 0)
                    return;
                _DrShowlist = dv[this.dataGridView1.CurrentCell.RowIndex].Row;
                this.ClickEvent(_DrShowlist);
            }
            else
            {
                DataTable dt = (DataTable)this.dataGridView1.DataSource;
                DataView dv = dt.DefaultView;
                if (dt.Rows.Count <= 0)
                    return;
                _isReturn = 1;
                _DrShowlist = dv[this.dataGridView1.CurrentCell.RowIndex].Row;
                this.Close();
            }
        }
    }
}
