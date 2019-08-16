using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using UniversalControlLibrary;
using Service_Manufacture_Storage;
using GlobalObject;

namespace Expression
{
    public partial class 整台整包发料 : Form
    {
        string m_storageID = "";

        string m_billNo = "";

        private List<View_S_MaterialRequisitionGoods> m_resultList = new List<View_S_MaterialRequisitionGoods>();

        public List<View_S_MaterialRequisitionGoods> ResultList
        {
            get { return m_resultList; }
            set { m_resultList = value; }
        }

        IStoreServer m_storeServer = Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<IStoreServer>();

        ServerModule.IBasicGoodsServer m_basicGoodsServer = ServerModule.ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        Service_Manufacture_Storage.IProductOrder m_productOrderServer = Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<Service_Manufacture_Storage.IProductOrder>();

        ServerModule.IBomServer m_BomService = ServerModule.ServerModuleFactory.GetServerModule<IBomServer>();

        public 整台整包发料(string billNo, string storageID)
        {
            InitializeComponent();

            m_storageID = storageID;

            m_billNo = billNo;

            cmbProductType.DataSource = m_BomService.GetAssemblyTypeList();
            cmbProductType.SelectedIndex = -1;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            List<BASE_ProductOrder> listOrder = m_productOrderServer.GetPackGoodsList(cmbProductType.Text, CE_DebitScheduleApplicable.正常装配);
            double showPosition = 0;

            foreach (BASE_ProductOrder curItem in listOrder)
            {
                View_F_GoodsPlanCost tempGoodsInfo = UniversalFunction.GetGoodsInfo(curItem.GoodsID);
                List<View_S_Stock> listStock = m_storeServer.GetGoodsStoreOnlyForAssembly(curItem.GoodsID, m_storageID).ToList();

                decimal dcCount = curItem.Redices * numFetchCount.Value;
                decimal dcPack = m_basicGoodsServer.GetPackCount(curItem.GoodsID, null);
                decimal dcRealCount = 0;

                if (dcCount % dcPack == 0)
                {
                    dcRealCount = (decimal)((int)(dcCount / dcPack));
                }
                else
                {
                    dcRealCount = (decimal)((int)(dcCount / dcPack)) + 1;
                }

                decimal requestCount = dcRealCount * dcPack;
                decimal requestCountTemp = requestCount;
                int stockIndex = 0;

                foreach (View_S_Stock stockItem in listStock)
                {

                    if (stockItem.库存数量 == 0)
                    {
                        continue;
                    }

                    stockIndex++;

                    View_S_MaterialRequisitionGoods goods = new View_S_MaterialRequisitionGoods();

                    goods.领料单号 = m_billNo;

                    if (stockIndex == 1)
                    {
                        goods.基数 = curItem.Redices;
                    }
                    else
                    {
                        goods.基数 = 0;
                    }

                    goods.物品ID = stockItem.物品ID;
                    goods.实领数 = 0;
                    goods.图号型号 = tempGoodsInfo.图号型号;
                    goods.物品名称 = tempGoodsInfo.物品名称;
                    goods.规格 = tempGoodsInfo.规格;

                    if (GlobalObject.GeneralFunction.IsNullOrEmpty(tempGoodsInfo.规格) && !GlobalObject.GeneralFunction.IsNullOrEmpty(stockItem.规格))
                    {
                        goods.规格 = stockItem.规格;
                    }

                    goods.显示位置 = showPosition++;
                    goods.备注 = "";
                    
                    goods.物品类别 = stockItem.材料类别编码;
                    goods.库存数 = (decimal)stockItem.库存数量;
                    goods.供应商编码 = stockItem.供货单位;
                    goods.批次号 = stockItem.批次号;
                    goods.实领数 = (decimal)(requestCount > stockItem.库存数量 ? (decimal)((int)(stockItem.库存数量 / dcPack)) * dcPack : requestCount);
                    goods.单位 = stockItem.单位;
                    goods.货架 = stockItem.货架;
                    goods.层 = stockItem.层;
                    goods.列 = stockItem.列;
                    goods.请领数 = requestCountTemp;

                    if (stockItem.库存数量 >= requestCount)
                    {
                        m_resultList.Add(goods);
                        break;
                    }
                    else
                    {
                        if (listStock.Count != 1 || (stockIndex != listStock.Count))
                        {
                            requestCount -= (decimal)goods.实领数;
                        }

                        m_resultList.Add(goods);
                    }
                }
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}
