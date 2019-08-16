using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using System.Data;
using System.Data.Linq;

namespace ProvidersServerModule
{
    public class ProvidersBaseServer : IProvidersBaseServer
    {
        /// <summary>
        /// 查询结果过滤器
        /// </summary>
        string m_queryResultFilter = null;

        /// <summary>
        /// 获取或设置查询结果过滤器
        /// </summary>
        public string QueryResultFilter
        {
            get { return m_queryResultFilter; }
            set { m_queryResultFilter = value; }
        }

        /// <summary>
        /// 获取供应商档案管理
        /// </summary>
        /// <param name="returnInfo">供应商档案信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllBill(out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            IAuthorization serverAuthorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = serverAuthorization.Query("供应商档案管理", null);
            }
            else
            {
                qr = serverAuthorization.Query("供应商档案管理", null, QueryResultFilter);
            }

            if (!qr.Succeeded)
            {
                error = qr.Error;
                return false;
            }

            returnInfo = qr;
            return true;
        }

        /// <summary>
        /// 添加供应商档案
        /// </summary>
        /// <param name="providers">供应商档案数据集</param>
        /// <param name="dtLinkMan">供应商联系人数据集</param>
        /// <param name="dtPersonnel">供应商责任人数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AddProvidersInfo(P_ProvidersBaseInfo providers,List<ProviderPrincipal> dtPersonnel,
            List<P_ProviderLinkMan> dtLinkMan, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.P_ProvidersBaseInfo
                             where a.ProviderCode == providers.ProviderCode
                             && a.ProviderName == providers.ProviderName
                             select a;

                if (result.Count() == 0)
                {
                    dataContxt.P_ProvidersBaseInfo.InsertOnSubmit(providers);
                }
                else if (result.Count() == 1)
                {
                    P_ProvidersBaseInfo lnqInfo = result.Single();

                    lnqInfo.Address = providers.Address;
                    lnqInfo.BankCode = providers.BankCode;
                    lnqInfo.CompanyWeb = providers.CompanyWeb;
                    lnqInfo.Email = providers.Email;
                    lnqInfo.FaxNo = providers.FaxNo;
                    lnqInfo.IsPapers = providers.IsPapers;
                    lnqInfo.LegalRepresenta = providers.LegalRepresenta;
                    lnqInfo.OpenBank = providers.OpenBank;
                    lnqInfo.OpenInvoiceNumber = providers.OpenInvoiceNumber;
                    lnqInfo.Postcode = providers.Postcode;
                    lnqInfo.Property = providers.Property;
                    lnqInfo.ProviderType = providers.ProviderType;
                    lnqInfo.Record = BasicInfo.LoginID;
                    lnqInfo.RecordDate = ServerTime.Time;
                    lnqInfo.Remark = providers.Remark;
                    lnqInfo.Status = "潜在供应商";
                    lnqInfo.TaxpayerNumber = providers.TaxpayerNumber;
                    lnqInfo.Annex = providers.Annex;
                    lnqInfo.AnnexName = providers.AnnexName;
                }

                //添加责任人
                if (DeletePrincipal(providers.ProviderCode, out error))
                {
                    foreach (ProviderPrincipal item in dtPersonnel)
                    {
                        dataContxt.ProviderPrincipal.InsertOnSubmit(item);
                    }

                    dataContxt.SubmitChanges();
                }

                //添加联系人
                if (DeleteLinkMan(providers.ProviderCode, out error))
                {
                    foreach (P_ProviderLinkMan item in dtLinkMan)
                    {
                        dataContxt.P_ProviderLinkMan.InsertOnSubmit(item);
                    }
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除供应商联系人
        /// </summary>
        /// <param name="providerCode">供应商编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        private bool DeleteLinkMan(string providerCode, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.P_ProviderLinkMan
                             where a.ProviderCode == providerCode
                             select a;

                dataContxt.P_ProviderLinkMan.DeleteAllOnSubmit(result);

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除供应商责任人
        /// </summary>
        /// <param name="providerCode">供应商编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        private bool DeletePrincipal(string providerCode,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.ProviderPrincipal
                             where a.Provider == providerCode
                             select a;

                dataContxt.ProviderPrincipal.DeleteAllOnSubmit(result);
                
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 添加供应商所供零件
        /// </summary>
        /// <param name="oldListGoods">旧零件数据集</param>
        /// <param name="listGoods">更新后的零件数据集</param>
        /// <param name="providerCode">供应商编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool AddGoodsInfo(List<P_ProviderGoods> oldListGoods,List<P_ProviderGoods> listGoods,
            string providerCode,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
                
                var resultGoods = from a in dataContxt.P_ProviderGoods
                             where a.ProviderCode == providerCode
                             select a;

                if (DeleteGoodsInfo(providerCode, out error))
                {
                    foreach (P_ProviderGoods item in listGoods)
                    {
                        dataContxt.P_ProviderGoods.InsertOnSubmit(item);
                    }
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 通过供应商编号获取供应商档案信息
        /// </summary>
        /// <param name="providerCode">供应商编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回数据集，失败返回null</returns>
        public P_ProvidersBaseInfo GetBaseInfoByCode(string providerCode,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.P_ProvidersBaseInfo
                             where a.ProviderCode == providerCode
                             select a;

                if (result.Count() > 0)
                {
                    return result.Single();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 通过供应商编号获取供应商所供零件
        /// </summary>
        /// <param name="providerCode">供应商编号</param>
        /// <returns>成功返回数据集，失败返回null</returns>
        public DataTable GetGoodsInfoByCode(string providerCode)
        {
            string sql = "select * from View_P_ProviderGoods where 供应商='" + providerCode + "'";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        /// <summary>
        /// 通过供应商编号获取供应商联系人
        /// </summary>
        /// <param name="providerCode">供应商编号</param>
        /// <returns>成功返回数据集，失败返回null</returns>
        public DataTable GetLinkManByCode(string providerCode)
        {
            string sql = "select * from View_P_ProviderLinkMan where 供应商简码='" + providerCode + "'";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        /// <summary>
        /// 获得责任人信息
        /// </summary>
        /// <param name="providerCode">责任人工号</param>
        /// <returns>返回责任人信息</returns>
        public DataTable GetProviderPrincipal(string providerCode)
        {
            string strSql = "select Name as 责任人,PrincipalWorkId as 责任人工号, IsMainDuty as 主要责任人,ID as 序号 " +
                            " from ProviderPrincipal as a " +
                            " inner join HR_Personnel as b on a.PrincipalWorkId = b.WorkID " +
                            " where a.Provider = '" + providerCode + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 删除供应商所供零件
        /// </summary>
        /// <param name="providerCode">供应商编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回false</returns>
        private bool DeleteGoodsInfo(string providerCode, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.P_ProviderGoods
                             where a.ProviderCode == providerCode
                             select a;

                dataContxt.P_ProviderGoods.DeleteAllOnSubmit(result);

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 检测供应商是否在库存表已存在
        /// </summary>
        /// <param name="providercode">供应商编码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>不存在返回True，存在返回False</returns>
        public bool CheckStockPrivuderIsIn(string providercode, out string error)
        {
            error = null;

            try
            {
                string strSql = "select * from S_Stock where Provider = '" + providercode + "'";
                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (dt.Rows.Count == 0)
                {
                    return true;
                }
                else
                {
                    error = "库存表里已经存在此供应商的供货信息，不能删除";
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 通过类别名称或编号，获取零件类别
        /// </summary>
        /// <param name="type">零件类别名称或编号</param>
        /// <returns>有数据返回数据集，没有数据返回Null</returns>
        public P_ProviderGoodsType GetGoodsType(string type)
        {
            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.P_ProviderGoodsType
                             where a.TypeName == type || a.ID == Convert.ToInt32(type)
                             select a;

                if (result.Count() > 0)
                {
                    return result.Single();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 批量插入供应商档案
        /// </summary>
        /// <param name="providers">供应商档案列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        public bool InsertProvidersInfo(DataTable providers,out string error)
        {
            error = null;

            string strTemp = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                for (int i = 0; i < providers.Rows.Count; i++)
                {
                    if (providers.Rows[i]["供应商简码"].ToString().Trim() != "")
                    {
                        strTemp = providers.Rows[i]["供应商简码"].ToString().Trim();
                        string code = strTemp;

                        var result = from a in dataContxt.P_ProvidersBaseInfo
                                     where a.ProviderCode == code
                                     && a.ProviderName == providers.Rows[i]["供应商全称"].ToString().Trim()
                                     select a;

                        if (result.Count() == 0)
                        {
                            P_ProvidersBaseInfo providersList = new P_ProvidersBaseInfo();

                            providersList.ProviderCode = code;
                            providersList.ProviderName = providers.Rows[i]["供应商全称"] == DBNull.Value ? ""
                               : providers.Rows[i]["供应商全称"].ToString().Trim();
                            providersList.Address = providers.Rows[i]["地址"] == DBNull.Value ? ""
                               : providers.Rows[i]["地址"].ToString().Trim();
                            providersList.BankCode = providers.Rows[i]["帐号"] == DBNull.Value ? ""
                               : providers.Rows[i]["帐号"].ToString().Trim();
                            providersList.Email = providers.Rows[i]["电子邮件"] == DBNull.Value ? ""
                               : providers.Rows[i]["电子邮件"].ToString().Trim();
                            providersList.FaxNo = providers.Rows[i]["传真"] == DBNull.Value ? ""
                               : providers.Rows[i]["传真"].ToString().Trim();
                            providersList.LegalRepresenta = "";//法人代表
                            providersList.OpenBank = providers.Rows[i]["开户行"] == DBNull.Value ? ""
                               : providers.Rows[i]["开户行"].ToString().Trim();
                            providersList.OpenInvoiceNumber = providers.Rows[i]["开具发票电话号码"] == DBNull.Value ? ""
                               : providers.Rows[i]["开具发票电话号码"].ToString().Trim();
                            providersList.Postcode = providers.Rows[i]["邮编"] == DBNull.Value ? ""
                               : providers.Rows[i]["邮编"].ToString().Trim();
                            providersList.Property = "";//公司性质
                            providersList.Record = BasicInfo.LoginID;
                            providersList.RecordDate = ServerTime.Time;
                            providersList.Remark = "";//备注
                            providersList.ShortName = "";//简称
                            providersList.Status = "合格供应商";//供应商状态
                            providersList.TaxpayerNumber = providers.Rows[i]["纳税人识别号"] == DBNull.Value ? ""
                               : providers.Rows[i]["纳税人识别号"].ToString().Trim();

                            providersList.PY = UniversalFunction.GetPYWBCode(providers.Rows[i]["供应商全称"].ToString().Trim(), "PY");
                            providersList.WB = UniversalFunction.GetPYWBCode(providers.Rows[i]["供应商全称"].ToString().Trim(), "WB");

                            dataContxt.P_ProvidersBaseInfo.InsertOnSubmit(providersList);
                            dataContxt.SubmitChanges();
                        }
                    }

                    if (providers.Rows[i]["零件图号"].ToString().Trim() != "")
                    {
                        var resultList = from a in dataContxt.P_ProviderGoods
                                         where a.GoodsCode == providers.Rows[i]["零件图号"].ToString().Trim()
                                         && a.GoodsName == providers.Rows[i]["零件名称"].ToString().Trim()
                                         && a.Spec == providers.Rows[i]["规格"].ToString().Trim()
                                         && a.ProviderCode == strTemp
                                         select a;

                        if (resultList.Count() == 0)
                        {
                            P_ProviderGoods goods = new P_ProviderGoods();

                            goods.GoodsCode = providers.Rows[i]["零件图号"] == DBNull.Value ? ""
                                : providers.Rows[i]["零件图号"].ToString().Trim();
                            goods.GoodsName = providers.Rows[i]["零件名称"] == DBNull.Value ? ""
                                : providers.Rows[i]["零件名称"].ToString().Trim();
                            goods.Spec = providers.Rows[i]["规格"] == DBNull.Value ? ""
                                : providers.Rows[i]["规格"].ToString().Trim();
                            goods.ProviderCode = strTemp;

                            dataContxt.P_ProviderGoods.InsertOnSubmit(goods);
                            dataContxt.SubmitChanges();
                        }
                    }

                    if (providers.Rows[i]["联系人"].ToString().Trim() != "")
                    {
                        var resultLinkMan = from a in dataContxt.P_ProviderLinkMan
                                            where a.Name == providers.Rows[i]["联系人"].ToString().Trim()
                                            && a.ProviderCode == strTemp
                                            select a;

                        if (resultLinkMan.Count() == 0)
                        {
                            P_ProviderLinkMan linkMan = new P_ProviderLinkMan();

                            linkMan.Mobilephone = providers.Rows[i]["手机"] == DBNull.Value ? ""
                                : providers.Rows[i]["手机"].ToString().Trim();
                            linkMan.Name = providers.Rows[i]["联系人"] == DBNull.Value ? ""
                                : providers.Rows[i]["联系人"].ToString().Trim();
                            linkMan.Position = providers.Rows[i]["职务"] == DBNull.Value ? ""
                               : providers.Rows[i]["职务"].ToString().Trim();
                            linkMan.ProviderCode = strTemp;
                            linkMan.Telephone = providers.Rows[i]["联系电话"] == DBNull.Value ? ""
                               : providers.Rows[i]["联系电话"].ToString().Trim();

                            dataContxt.P_ProviderLinkMan.InsertOnSubmit(linkMan);
                            dataContxt.SubmitChanges();
                        }
                    }
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message + "供应商简码" + strTemp;
                return false;
            }
        }
    }
}
