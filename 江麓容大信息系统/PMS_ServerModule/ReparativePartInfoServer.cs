using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerModule
{
    /// <summary>
    /// 返修零件信息服务（打条形码用）
    /// </summary>
    class ReparativePartInfoServer : ServerModule.IReparativePartInfoServer
    {
        /// <summary>
        /// 获得指定产品图号的返修零件信息(打返修条形码用)
        /// </summary>
        /// <param name="productCode">产品图号</param>
        /// <returns>返回获取到的信息</returns>
        public IQueryable<View_ZPX_ReparativeBarcode> GetData(string productCode)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return from r in ctx.View_ZPX_ReparativeBarcode
                   where r.产品编码 == productCode
                   orderby r.位置
                   select r;
        }

        /// <summary>
        /// 更新返修零件信息
        /// </summary>
        /// <param name="lstInfo">要添加的条形码信息</param>
        /// <param name="error">出错时输出错误信息</param>
        /// <returns>返回操作是否成功的标志</returns>
        /// <remarks>打印条形码时如果找不到此物品的条形码时直接生成条形码用</remarks>
        public bool Update(List<StateData<View_ZPX_ReparativeBarcode>> lstInfo, out string error)
        {
            error = null;

            try
            {
                
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                IBasicGoodsServer serverBasicGoods = SCM_Level01_ServerFactory.GetServerModule<IBasicGoodsServer>();

                int intGoodsID = 0;

                foreach (var item in lstInfo)
                {
                    intGoodsID = serverBasicGoods.GetGoodsID(item.Data.零部件编码, item.Data.零部件名称, item.Data.规格);

                    if (intGoodsID == 0)
                    {
                        error = string.Format("获取不到 {0}, {1}, 规格：{2} 的物品的物品编号，请与管理员联系",
                            item.Data.零部件编码, item.Data.零部件名称, item.Data.规格);

                        return false;
                    }

                    ZPX_ReparativeBarcode data = new ZPX_ReparativeBarcode();

                    data.ID = item.Data.序号;
                    data.Edition = item.Data.产品编码;
                    data.GoodsID = intGoodsID;
                    data.Amount = item.Data.数量;
                    data.Provider = item.Data.供货单位;
                    data.Position = item.Data.位置;

                    IQueryable<ZPX_ReparativeBarcode> result = null;

                    if (data.ID != 0)
                    {
                        result = from r in ctx.ZPX_ReparativeBarcode
                                 where r.ID == data.ID
                                 select r;
                    }
                    else
                    {
                        result = from r in ctx.ZPX_ReparativeBarcode
                                 where r.Edition == data.Edition && r.GoodsID == intGoodsID
                                 select r;
                    }

                    switch (item.DataStatus)
                    {
                        case StateData<View_ZPX_ReparativeBarcode>.DataStatusEnum.Add:

                            if (result.Count() > 0)
                            {
                                error = string.Format("{0}, {1}, 规格：{2} 的物品已经存在不允许重复添加",
                                    item.Data.零部件编码, item.Data.零部件名称, item.Data.规格);

                                return false;
                            }

                            ctx.ZPX_ReparativeBarcode.InsertOnSubmit(data);
                            break;

                        case StateData<View_ZPX_ReparativeBarcode>.DataStatusEnum.Delete:

                            if (result.Count() == 0)
                            {
                                error = string.Format("{0}, {1}, 规格：{2} 的物品不存在无法删除",
                                    item.Data.零部件编码, item.Data.零部件名称, item.Data.规格);

                                return false;
                            }

                            ctx.ZPX_ReparativeBarcode.DeleteOnSubmit(result.Single());

                            break;
                        case StateData<View_ZPX_ReparativeBarcode>.DataStatusEnum.Update:

                            if (result.Count() == 0)
                            {
                                error = string.Format("{0}, {1}, 规格：{2} 的物品不存在无法更新",
                                    item.Data.零部件编码, item.Data.零部件名称, item.Data.规格);

                                return false;
                            }

                            var record = result.Single();

                            record.Amount = data.Amount;
                            record.Provider = data.Provider;
                            record.Position = data.Position;

                            break;
                    }
                }

                ctx.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }
    }
}
