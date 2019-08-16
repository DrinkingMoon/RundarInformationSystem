/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  快速返修变速箱.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2014/01/06
 * 开发平台:  Visual C# 2005
 * 用于    :  ERP软件
 *----------------------------------------------------------------------------
 * 描述 : 用于快速返修变速箱，记录返修信息及返修次数
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2014/01/06 16:28 作者: 夏石友 当前版本: V1.1
 *        修改说明: 增加了批量返修的功能
 ******************************************************************************/
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
using System.Diagnostics;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 快速返修变速箱 : Form
    {
        #region 成员变量

        /// <summary>
        /// 产品名称
        /// </summary>
        string m_productName;

        /// <summary>
        /// 箱号
        /// </summary>
        string m_productNumber;

        /// <summary>
        /// 产品类型编码
        /// </summary>
        string m_productTypeCode;

        /// <summary>
        /// 电子档案服务组件
        /// </summary>
        IElectronFileServer m_electronFilesServer = ServerModuleFactory.GetServerModule<IElectronFileServer>();

        /// <summary>
        /// 条形码服务组件
        /// </summary>
        IBarCodeServer m_barCodeServer = ServerModuleFactory.GetServerModule<IBarCodeServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 获取指定变速箱原有电子档案信息
        /// </summary>
        IQueryable<View_P_ElectronFile> m_electronFileInfo = null;

        /// <summary>
        /// 需要录入标识码的零件信息
        /// </summary>
        IQueryable<View_ZPX_PartNameWithUniqueCode> m_uniqueCodeParts;

        /// <summary>
        /// 可选配的零件图号型号信息
        /// </summary>
        IQueryable<string> m_optionPartCodes;

        /// <summary>
        /// 获取指定变速箱装配BOM信息
        /// </summary>
        List<View_P_AssemblingBom> m_assemblingBomInfo = null;

        /// <summary>
        /// 父总成零件图号与零件装配虚拟编码之间的映射
        /// </summary>
        Dictionary<string, string> m_virtualPartMapping = null;

        /// <summary>
        /// 产品信息服务
        /// </summary>
        private IProductInfoServer m_productInfoServer = PMS_ServerFactory.GetServerModule<IProductInfoServer>();

        #endregion

        public 快速返修变速箱()
        {
            InitializeComponent();

            IQueryable<View_P_ProductInfo> productInfo = null;

            if (!m_productInfoServer.GetAllProductInfo(out productInfo, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }
            else
            {
                productInfo = from r in productInfo
                              where !r.产品类型名称.Contains("返修")
                              select r;

                cmbProductCode.DataSource = productInfo;
                cmbProductCode.DisplayMember = "产品类型编码";
                cmbProductCode.ValueMember = "产品类型编码";
            }
        }

        /// <summary>
        /// 检查CVT编号
        /// </summary>
        /// <returns>成功返回true</returns>
        bool CheckCVTNumber()
        {
            if (cmbProductCode.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择产品类型后再进行此操作");
                cmbProductCode.Focus();
                return false;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtProductNumber.Text))
            {
                MessageDialog.ShowPromptMessage("请输入箱号后再进行此操作");
                txtProductNumber.Focus();
                return false;
            }

            if (!ServerModuleFactory.GetServerModule<IProductCodeServer>().VerifyProductCodesInfo(
                cmbProductCode.Text, txtProductNumber.Text, GlobalObject.CE_BarCodeType.内部钢印码, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                txtProductNumber.Focus();
                return false;
            }

            if (!GlobalObject.GeneralFunction.IsNullOrEmpty(txtProductNumber2.Text))
            {
                if (!ServerModuleFactory.GetServerModule<IProductCodeServer>().VerifyProductCodesInfo(
                    cmbProductCode.Text, txtProductNumber2.Text, GlobalObject.CE_BarCodeType.内部钢印码, out m_error))
                {
                    MessageDialog.ShowErrorMessage(m_error);
                    txtProductNumber2.Focus();
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 获取指定箱号的分总成零件信息及电子档案信息
        /// </summary>
        /// <param name="productNumber">要获取信息的箱号</param>
        /// <param name="repairCount">返回指定箱号的变速箱的返修次数</param>
        /// <returns>返回获取到的分总成零件信息</returns>
        private Dictionary<string, object> GetParentInfo(string productNumber, out int repairCount)
        {
            Dictionary<string, object> dicParentInfo = new Dictionary<string, object>();
            IQueryable<View_P_ElectronFile> electronFileInfo = null;
            List<string> parentNames = null;

            repairCount = 0;

            // 获取指定变速箱箱号的信息
            if (m_electronFilesServer.GetElectronFile(productNumber, out electronFileInfo, out m_error))
            {
                repairCount = (from r in electronFileInfo
                                        select r.返修次数).Max();
               
                parentNames = (from r in electronFileInfo
                               where r.父总成名称 != ""
                               orderby r.父总成名称
                               select r.父总成名称).Distinct().ToList();

                IQueryable<View_P_ElectronFile> info = from r in electronFileInfo
                                                       where parentNames.Contains(r.零部件名称)
                                                       orderby r.零部件名称, r.装配时间 descending
                                                       select r;

                foreach (var item in info)
                {
                    if (!dicParentInfo.ContainsKey(item.零部件名称))
                        dicParentInfo.Add(item.零部件名称, item);
                }
            }

            parentNames = (from r in m_assemblingBomInfo
                           where r.父总成名称 != "" //!Nullable.Equals(r.父总成名称, null)
                           orderby r.父总成名称
                           select r.父总成名称).Distinct().ToList();

            IEnumerable<View_P_AssemblingBom> infos = from r in m_assemblingBomInfo
                                                      where parentNames.Contains(r.零件名称) || r.是否总成
                                                      orderby r.零件名称
                                                      select r;

            foreach (var item in infos)
            {
                if (!dicParentInfo.ContainsKey(item.零件名称))
                {
                    if (item.零件名称 == m_productName)
                        item.备注 = productNumber;
                    else
                        item.备注 = CreateVirtualCode(item.零件编码);

                    dicParentInfo.Add(item.零件名称, item);
                }
            }

            return dicParentInfo;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        void Init()
        {
            if (!CheckCVTNumber())
                return;

            txtProductNumber.Text = txtProductNumber.Text.Trim().ToUpper();
            txtProductNumber2.Text = txtProductNumber2.Text.Trim().ToUpper();

            if (m_productNumber == cmbProductCode.Text + " " + txtProductNumber.Text)
                return;

            m_productNumber = cmbProductCode.Text + " " + txtProductNumber.Text;

            List<string> parentNames = null;

            string productType = cmbProductCode.Text;

            Dictionary<string, string> mapping = m_electronFilesServer.GetVirtualPartMapping(productType);

            Debug.Assert(mapping != null, "获取不到总成虚拟装配码");

            m_virtualPartMapping = new Dictionary<string, string>();

            foreach (var item in mapping)
            {
                m_virtualPartMapping.Add(item.Value, item.Key);
            }

            m_assemblingBomInfo = ServerModuleFactory.GetServerModule<IAssemblingBom>().GetAssemblingBom(productType);

            foreach (var item in m_assemblingBomInfo)
            {
                if (item.父总成名称 == null)
                    item.父总成名称 = "";
            }

            m_productTypeCode = productType;

            // 获取指定变速箱箱号的信息
            m_electronFilesServer.GetElectronFile(m_productNumber, out m_electronFileInfo, out m_error);

            m_productName = (from r in m_assemblingBomInfo
                             where r.父总成名称 == "" //Nullable.Equals(r.父总成名称, null)
                             select r.零件名称).Single();

            parentNames = (from r in m_assemblingBomInfo
                           where r.父总成名称 != "" //!Nullable.Equals(r.父总成名称, null)
                           orderby r.父总成名称
                           select r.父总成名称).Distinct().ToList();

            parentNames.Remove(m_productName);
            parentNames.Insert(0, m_productName);

            chkLstParentName.Items.Clear();
            chkLstParentName.Items.AddRange(parentNames.ToArray());

            #region 让分总成下拉框中可以包含（箱体线束总成等零件，返修时会更换一些BOM表中不存在的子零件）
            parentNames = (from r in m_assemblingBomInfo
                           where r.是否总成
                           orderby r.零件编码
                           select r.零件名称).Distinct().ToList();

            cmbParentName.Items.Clear();
            cmbParentName.Items.AddRange(parentNames.ToArray());
            #endregion

            // 获取需要录入标识码的零件信息
            m_uniqueCodeParts = m_electronFilesServer.GetPartNameWithUniqueCode();

            // 获取可选配的零件图号型号信息
            m_optionPartCodes = ServerModuleFactory.GetServerModule<IChoseConfectServer>().GetOptionPartCode();
        }

        /// <summary>
        /// 创建总成虚拟装配码
        /// </summary>
        /// <param name="parentCode">总成图号</param>
        /// <returns>返回获取到的装配码</returns>
        private string CreateVirtualCode(string parentCode)
        {
            try
            {
                string code = string.Format("{0} FX {1}{2}", m_virtualPartMapping[parentCode], ServerTime.Time.ToString("yyyyMM"),
                        m_productNumber.Substring(m_productNumber.Length - 5));

                return code;

            }
            catch (Exception)
            {
                return parentCode;
                //Exception exce = new Exception("获取不到总成虚拟装配码，总成图号：" + parentCode + "，方法名：CreateVirtualCode");

                //throw exce;
            }
        }

        /// <summary>
        /// 获取变速箱信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetCVTInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckCVTNumber())
                {
                    return;
                }

                Init();
            }
            catch (Exception exce)
            {
                m_productNumber = "";

                MessageDialog.ShowErrorMessage(exce.Message);
            }
        }

        /// <summary>
        /// 查看变速箱维修记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindRepairedInfo_Click(object sender, EventArgs e)
        {
            if (m_electronFileInfo == null)
            {
                MessageDialog.ShowPromptMessage("没有维修记录");
                return;
            }

            var result = from r in m_electronFileInfo
                         where r.返修次数 > 0
                         select r;

            FormQueryInfo form = new FormQueryInfo(result);
            form.ShowDialog();
        }

        /// <summary>
        /// 检查父总成零件信息是否正确
        /// </summary>
        /// <returns>正确返回true</returns>
        private bool CheckParentPartData()
        {
            if (chkLstParentName.CheckedItems.Count == 0)
            {
                MessageDialog.ShowPromptMessage("至少勾选一个总成名称后再进行此操作");
                return false;
            }

            if (txtParentPartReason.Text.Trim() == "")
            {
                txtParentPartReason.Focus();
                MessageDialog.ShowPromptMessage("请录入总成调整原因后再进行此操作");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检查录入的信息是否正确
        /// </summary>
        /// <returns>正确返回true</returns>
        private bool CheckData()
        {
            string parentName = cmbParentName.Text;
            string goodsCode = txtGoodsCode.Text;
            string goodsName = txtGoodsName.Text;
            string spec = txtSpec.Text;

            if (!chkIsOnlyAdjustPart.Checked && txtBatchNo.Text.Length == 0)
            {
                numBarcode.Focus();
                MessageDialog.ShowPromptMessage("请获取返修件的条形码后再进行此操作");
                return false;
            }

            if (numAssemblyAmount.Value == 0m)
            {
                numAssemblyAmount.Focus();
                MessageDialog.ShowPromptMessage("请录入返修的装配数量后再进行此操作");
                return false;
            }

            // 调整模式下必须录入备注
            if (chkIsOnlyAdjustPart.Checked && txtRemark.Text.Trim().Length == 0)
            {
                txtRemark.Focus();
                MessageDialog.ShowPromptMessage("请录入备注信息后再进行此操作");
                return false;
            }

            // 是否需要录入标识码的零件
            bool needUniqueCode = (from r in m_uniqueCodeParts
                                   where r.零件名称 == goodsName
                                   select r).Count() > 0;

            if (needUniqueCode && txtPartID.Text.Trim() == "")
            {
                txtPartID.Focus();
                MessageDialog.ShowPromptMessage("请录入零件标识码后再进行此操作");
                return false;
            }

            // 是否选配零件
            //bool isOptionPartCode = false;

            if (m_optionPartCodes.Contains(goodsCode))
            {
               //isOptionPartCode = true;

                if (txtCheckData.Text.Trim() == "")
                {
                    txtCheckData.Focus();
                    MessageDialog.ShowPromptMessage("请录入正确的检测数据后再进行此操作（效仿原有检测数据录入格式）");
                    return false;
                }

                if (txtResultData.Text.Trim() == "")
                {
                    txtResultData.Focus();
                    MessageDialog.ShowPromptMessage("请录入正确的测量数据后再进行此操作（效仿原有测量数据录入格式）");
                    return false;
                }
            }
            else
            {
                if (txtCheckData.Text != "")
                {
                    txtCheckData.Focus();

                    MessageDialog.ShowPromptMessage("非选配件不允许录入检测数据,需要记录有关信息请在备注中录入数据");
                    return false;
                }

                if (txtResultData.Text != "")
                {
                    txtResultData.Focus();

                    MessageDialog.ShowPromptMessage("非选配件不允许录入测量数据,需要记录有关信息请在备注中录入数据");
                    return false;
                }

                try
                {
                    if (m_productName.Contains("力帆") && (goodsName == "中间轮轴" || goodsName == "中间轮"))
                    {
                        if (numAssemblyAmount.Value != 1m)
                        {
                            MessageDialog.ShowPromptMessage("录入的装配数量不正确，请核实后再进行此操作");
                            return false;
                        }
                    }
                    else
                    {
                        // 对于“众泰”的“后端盖密封垫”是选配件
                        if (m_productName.Contains("众泰") && goodsName == "后端盖密封垫")
                        {
                            //isOptionPartCode = true;
                        }
                            

                        //if (!isOptionPartCode)
                        //{
                        //    int amount = 0;

                        //    IQueryable<View_P_ElectronFile> partInfo = null;

                        //    if (m_electronFileInfo != null)
                        //    {
                        //        partInfo = from r in m_electronFileInfo
                        //                   where r.父总成名称 == parentName &&
                        //                   r.零部件编码 == goodsCode && r.零部件名称 == goodsName && r.规格 == spec
                        //                   select r;
                        //    }

                        //    if (partInfo == null || partInfo.Count() == 0)
                        //    {
                        //        IEnumerable<View_P_AssemblingBom> partInfo2 = null;

                        //        partInfo2 = from r in m_assemblingBomInfo
                        //                    where r.父总成名称 == parentName &&
                        //                          r.零件编码 == goodsCode && r.零件名称 == goodsName && r.规格 == spec
                        //                    select r;

                        //        if (partInfo2.Count() == 0)
                        //        {
                        //            MessageDialog.ShowErrorMessage(string.Format("图号型号[{0}], 物品名称[{1}], 规格[{2}] 的物品并不属于当前产品", goodsCode, goodsName, spec));
                        //            return false;
                        //        }
                        //        else
                        //        {
                        //            amount = partInfo2.First().装配数量;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        amount = (from r in partInfo
                        //                  select r.数量).Max();
                        //    }

                        //    if (Convert.ToInt32(numAssemblyAmount.Value) > amount)
                        //    {
                        //        MessageDialog.ShowPromptMessage("录入的装配数量不正确，请核实后再进行此操作，如果确实是正确的请联系管理员");
                        //        return false;
                        //    }
                        //}
                    }
                }
                catch (Exception exce)
                {
                    MessageDialog.ShowErrorMessage(exce.Message);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 检查录入行数据是否正确
        /// </summary>
        /// <param name="row">录入的数据行</param>
        /// <param name="ef">录入数据正确修改工位信息</param>
        /// <returns>正确返回true</returns>
        private bool CheckData(DataGridViewRow row, P_ElectronFile ef)
        {
            string parentName = row.Cells["总成名称"].Value.ToString();
            string goodsCode = row.Cells["图号型号"].Value.ToString();
            string goodsName = row.Cells["零件名称"].Value.ToString();
            string spec = row.Cells["规格"].Value.ToString();

            if (parentName == "" && row.Cells["零件名称"].Value.ToString() != m_productName)
            {
                MessageDialog.ShowPromptMessage(string.Format("零件名称[{0}],图号型号[{1}],规格[{2}]的物品没有指定总成名称, 无法继续。行号：{3}",
                    goodsName, goodsCode, spec, row.Index + 1));

                dataGridView1.CurrentCell = row.Cells[0];
                return false;
            }

            // 是否需要录入标识码的零件
            bool needUniqueCode = (from r in m_uniqueCodeParts
                                   where r.零件名称 == goodsName
                                   select r).Count() > 0;

            if (needUniqueCode && !GlobalObject.GeneralFunction.IsNullOrEmpty(txtProductNumber2.Text) && txtProductNumber.Text != txtProductNumber2.Text)
            {
                MessageDialog.ShowErrorMessage("批量返修时不支持有标识码的零件：" + goodsName);
                return false;
            }

            if (needUniqueCode && row.Cells["标识码"].Value.ToString() == "")
            {
                dataGridView1.CurrentCell = row.Cells[0];

                MessageDialog.ShowPromptMessage("请录入[" + goodsName + "] 零件的标识码后再进行此操作");
                return false;
            }

            // 是否选配零件
            //bool isOptionPartCode = false;

            if (m_optionPartCodes.Contains(goodsCode))
            {
                //isOptionPartCode = true;

                if (!GlobalObject.GeneralFunction.IsNullOrEmpty(txtProductNumber2.Text) && txtProductNumber.Text != txtProductNumber2.Text)
                {
                    MessageDialog.ShowErrorMessage("批量返修时不支持选配零件：" + goodsName);
                    return false;
                }

                if (row.Cells["检测数据"].Value.ToString() == "")
                {
                    dataGridView1.CurrentCell = row.Cells[0];

                    MessageDialog.ShowPromptMessage("请录入正确的检测数据后再进行此操作（效仿原有检测数据录入格式）");
                    return false;
                }

                if (row.Cells["测量数据"].Value.ToString() == "")
                {
                    dataGridView1.CurrentCell = row.Cells[0];

                    MessageDialog.ShowPromptMessage("请录入正确的测量数据后再进行此操作（效仿原有测量数据录入格式）");
                    return false;
                }
            }

            try
            {
                //int amount = 0;

                if (m_productName.Contains("力帆") && (goodsName == "中间轮轴" || goodsName == "中间轮"))
                {
                    if (Convert.ToInt32(row.Cells["装配数量"].Value) != 1)
                    {
                        dataGridView1.CurrentCell = row.Cells[0];

                        MessageDialog.ShowPromptMessage("录入的装配数量不正确，请核实后再进行此操作");
                        return false;
                    }
                }
                else
                {
                    //IQueryable<View_P_ElectronFile> partInfo = null;

                    //// 对于“众泰”的“后端盖密封垫”是选配件
                    //if (m_productName.Contains("众泰") && goodsName == "后端盖密封垫")
                    //    isOptionPartCode = true;

                    //if (m_electronFileInfo != null)
                    //{
                    //    if (!isOptionPartCode)
                    //    {
                    //        partInfo = from r in m_electronFileInfo
                    //                   where r.父总成名称 == parentName &&
                    //                   r.零部件编码 == goodsCode && r.零部件名称 == goodsName && r.规格 == spec
                    //                   select r;
                    //    }
                    //    else
                    //    {
                    //        partInfo = from r in m_electronFileInfo
                    //                   where r.父总成名称 == parentName &&
                    //                   r.零部件编码 == goodsCode && r.零部件名称 == goodsName
                    //                   select r;
                    //    }
                    //}

                    //if (partInfo == null || partInfo.Count() == 0)
                    //{
                    //    IEnumerable<View_P_AssemblingBom> partInfo2 = null;

                    //    if (!isOptionPartCode)
                    //    {
                    //        partInfo2 = from r in m_assemblingBomInfo
                    //                    where r.父总成名称 == parentName &&
                    //                          r.零件编码 == goodsCode && r.零件名称 == goodsName && r.规格 == spec
                    //                    select r;
                    //    }
                    //    else
                    //    {
                    //        partInfo2 = from r in m_assemblingBomInfo
                    //                    where r.父总成名称 == parentName &&
                    //                          r.零件编码 == goodsCode && r.零件名称 == goodsName
                    //                    select r;
                    //    }

                    //    if (partInfo2.Count() == 0)
                    //    {
                    //        dataGridView1.CurrentCell = row.Cells[0];

                    //        MessageDialog.ShowErrorMessage(string.Format("图号型号[{0}], 物品名称[{1}], 规格[{2}] 的物品并不属于当前产品", goodsCode, goodsName, spec));
                    //        return false;
                    //    }
                    //    else
                    //    {
                    //        amount = partInfo2.First().装配数量;
                    //        ef.WorkBench = partInfo2.First().工位;
                    //    }
                    //}
                    //else
                    //{
                    //    amount = (from r in partInfo
                    //              select r.数量).Max();

                    //    ef.WorkBench = partInfo.First().工位;
                    //}

                    //if (!isOptionPartCode && Convert.ToInt32(row.Cells["装配数量"].Value) > amount)
                    //{
                    //    dataGridView1.CurrentCell = row.Cells[0];

                    //    MessageDialog.ShowPromptMessage("录入的装配数量不正确，请核实后再进行此操作，如果确实是正确的请联系管理员");
                    //    return false;
                    //}
                }
            }
            catch (Exception exce)
            {
                dataGridView1.CurrentCell = row.Cells[0];

                MessageDialog.ShowErrorMessage(exce.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检查是否选中行
        /// </summary>
        /// <returns>正确返回true</returns>
        private bool CheckSelectRow()
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                return true;
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选中好一条记录后再进行此操作");
                return false;
            }
        }

        /// <summary>
        /// 检查总成零件信息是否已经重复
        /// </summary>
        /// <param name="currentRow">当前行</param>
        /// <param name="parentName">总成名称</param>
        /// <returns>重复返回true</returns>
        private bool CheckRepeatedParentPart(int currentRow, string parentName)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (currentRow != i && dataGridView1.Rows[i].Cells["零件名称"].Value.ToString() == parentName)
                {
                    MessageDialog.ShowPromptMessage("已经存在：" + parentName);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 检查零件信息是否已经重复
        /// </summary>
        /// <param name="currentRow">当前行</param>
        /// <returns>重复返回true</returns>
        private bool CheckRepeatedPart(int currentRow)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (currentRow != i &&
                    dataGridView1.Rows[i].Cells["总成名称"].Value.ToString() == cmbParentName.Text &&
                    dataGridView1.Rows[i].Cells["零件名称"].Value.ToString() == txtGoodsName.Text &&
                    dataGridView1.Rows[i].Cells["图号型号"].Value.ToString() == txtGoodsCode.Text &&
                    dataGridView1.Rows[i].Cells["规格"].Value.ToString() == txtSpec.Text)
                {
                    MessageDialog.ShowPromptMessage(string.Format("已经存在总成名称[{3}]，图号型号【{0}】，物品名称【{1}】，规格【{2}】的物品", 
                        txtGoodsCode.Text, txtGoodsName.Text, txtSpec.Text, cmbParentName.Text));

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 绘制数据显示控件行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 新建按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chkLstParentName.Items.Count; i++)
            {
                chkLstParentName.SetItemChecked(i, false);
            }

            chkLstParentName.ClearSelected();
            chkIsOnlyAdjustPart.Checked = false;

            txtParentPartReason.Text = "";
            
            numBarcode.Value = 0;
            numAssemblyAmount.Value = 0;

            cmbParentName.SelectedIndex = -1;

            txtGoodsCode.Text = "";
            txtGoodsName.Text = "";
            txtSpec.Text = "";
            txtPartID.Text = "";
            txtProvider.Text = "";
            txtBatchNo.Text = "";
            txtCheckData.Text = "";
            txtResultData.Text = "";
            txtRemark.Text = "";
            txtFittingPersonnel.Text = "";            
        }

        /// <summary>
        /// 添加总成调整信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddParentPart_Click(object sender, EventArgs e)
        {
            if (!CheckParentPartData())
                return;

            for (int i = 0; i < chkLstParentName.CheckedItems.Count; i++)
            {
                string name = chkLstParentName.CheckedItems[i].ToString();

                if (CheckRepeatedParentPart(-1, name))
                {
                    return;
                }

                if (m_electronFileInfo != null)
                {
                    var data = from r in m_electronFileInfo
                               where r.零部件名称 == name
                               select r;

                    if (data.Count() > 0)
                    {
                        var result = data.First();

                        dataGridView1.Rows.Add(new object[] { "是", "局部调整", result.父总成名称, name, result.零部件编码, result.规格,
                            "", 1, "", "", txtParentPartReason.Text, "", ""});

                        continue;
                    }
                }

                var resultInfo = (from r in m_assemblingBomInfo
                              where r.零件名称 == name
                              select r).First();


                dataGridView1.Rows.Add(new object[] { "是", "局部调整", resultInfo.父总成名称, name, resultInfo.零件编码, resultInfo.规格,
                    "", 1, "", "", txtParentPartReason.Text, "", ""});
            }
        }

        /// <summary>
        /// 修改总成调整信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateParentPart_Click(object sender, EventArgs e)
        {
            if (!CheckSelectRow())
                return;

            if (dataGridView1.SelectedRows[0].Cells["是否父总成"].Value.ToString() == "否")
            {
                MessageDialog.ShowPromptMessage("请选择一条父总成记录后再进行此操作");
                return;
            }

            if (!CheckParentPartData())
                return;

            if (chkLstParentName.CheckedItems.Count != 1)
            {
                MessageDialog.ShowPromptMessage("修改操作只允许选择一个总成名称");
                return;
            }

            string name = chkLstParentName.CheckedItems[0].ToString();

            if (CheckRepeatedParentPart(dataGridView1.SelectedRows[0].Index, name))
            {
                return;
            }

            var result = (from r in m_electronFileInfo
                          where r.零部件名称 == name
                          select r).First();

            DataGridViewRow row = dataGridView1.SelectedRows[0];

            row.Cells["总成名称"].Value = result.父总成名称;
            row.Cells["零件名称"].Value = name;
            row.Cells["图号型号"].Value = result.零部件编码;
            row.Cells["规格"].Value = result.规格;
            row.Cells["备注"].Value = txtParentPartReason.Text;
        }

        /// <summary>
        /// 添加返修零件信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPart_Click(object sender, EventArgs e)
        {
            if (!CheckData())
                return;

            if (CheckRepeatedPart(-1))
                return;

            dataGridView1.Rows.Add(new object[] { "否", chkIsOnlyAdjustPart.Checked ? "局部调整" : "局部返修", 
                cmbParentName.Text, txtGoodsName.Text, txtGoodsCode.Text, txtSpec.Text,
                txtPartID.Text, numAssemblyAmount.Value, txtProvider.Text, txtBatchNo.Text, txtRemark.Text,
                txtCheckData.Text, txtResultData.Text });
        }

        /// <summary>
        /// 修改返修零件信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdatePart_Click(object sender, EventArgs e)
        {
            if (!CheckSelectRow())
                return;

            if (dataGridView1.SelectedRows[0].Cells["是否父总成"].Value.ToString() == "是")
            {
                MessageDialog.ShowPromptMessage("请选择一条零件记录后再进行此操作");
                return;
            }

            //if (!CheckData())
            //    return;

            if (CheckRepeatedPart(dataGridView1.SelectedRows[0].Index))
                return;

            DataGridViewRow row = dataGridView1.SelectedRows[0];

            row.Cells["返修模式"].Value = chkIsOnlyAdjustPart.Checked ? "局部调整" : "局部返修";
            
            row.Cells["总成名称"].Value = cmbParentName.Text;
            row.Cells["零件名称"].Value = txtGoodsName.Text;
            row.Cells["图号型号"].Value = txtGoodsCode.Text;
            row.Cells["规格"].Value = txtSpec.Text;

            row.Cells["标识码"].Value = txtPartID.Text;
            row.Cells["装配数量"].Value = numAssemblyAmount.Value;
            row.Cells["供应商"].Value = txtProvider.Text;
            row.Cells["批次号"].Value = txtBatchNo.Text;
            row.Cells["备注"].Value = txtRemark.Text;
            row.Cells["检测数据"].Value = txtCheckData.Text;
            row.Cells["测量数据"].Value = txtResultData.Text;
        }

        /// <summary>
        /// 获取条形码信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetBarCodeInfo_Click(object sender, EventArgs e)
        {
            View_S_InDepotGoodsBarCodeTable data;
            
            if (!m_barCodeServer.GetData(Convert.ToInt32(numBarcode.Value), out data, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
            }
            else
	        {
                txtGoodsCode.Text = data.图号型号;
                txtGoodsName.Text = data.物品名称;
                txtSpec.Text = data.规格;
                txtProvider.Text = data.供货单位;
                txtBatchNo.Text = data.批次号;

                var info = from r in m_assemblingBomInfo
                           where r.零件编码 == data.图号型号 && r.零件名称 == data.物品名称
                           select r;

                if (info.Count() > 0)
                    cmbParentName.Text = info.First().父总成名称;
            }
        }

        /// <summary>
        /// 从dataGridView中删除当前选中行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!CheckSelectRow())
                return;

            dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
        }

        /// <summary>
        /// 点击单元格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
                return;

            DataGridViewRow row = dataGridView1.SelectedRows[0];

            if (row.Cells["是否父总成"].Value.ToString() == "是")
            {
                chkLstParentName.ClearSelected();

                for (int i = 0; i <chkLstParentName.Items.Count; i++)
                {
                    if (chkLstParentName.Items[i].ToString() == row.Cells["零件名称"].Value.ToString())
                    {
                        chkLstParentName.SetItemChecked(i, true);
                        chkLstParentName.SetSelected(i, true);
                    }
                    else
                    {
                        chkLstParentName.SetItemChecked(i, false);
                    }
                }

                txtParentPartReason.Text = row.Cells["备注"].Value.ToString();
            }
            else
            {
                chkIsOnlyAdjustPart.Checked = row.Cells["返修模式"].Value.ToString() == "局部调整";

                cmbParentName.Text = row.Cells["总成名称"].Value.ToString();
                txtGoodsName.Text = row.Cells["零件名称"].Value.ToString();
                txtGoodsCode.Text = row.Cells["图号型号"].Value.ToString();
                txtSpec.Text = row.Cells["规格"].Value.ToString();

                txtPartID.Text = row.Cells["标识码"].Value.ToString();
                numAssemblyAmount.Value = Convert.ToDecimal(row.Cells["装配数量"].Value);
                txtProvider.Text = row.Cells["供应商"].Value.ToString();
                txtBatchNo.Text = row.Cells["批次号"].Value.ToString();
                txtRemark.Text = row.Cells["备注"].Value.ToString();
                txtCheckData.Text = row.Cells["检测数据"].Value.ToString();
                txtResultData.Text = row.Cells["测量数据"].Value.ToString();
            }
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("没有记录需要保存");
                return;
            }

            dataGridView1.Sort(new RowComparer(SortOrder.Ascending));

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow row1 = dataGridView1.Rows[i];

                if (i + 1 >= dataGridView1.Rows.Count)
                {
                    break;
                }
                else
                {
                    DataGridViewRow row2 = dataGridView1.Rows[i + 1];

                    if (row1.Cells["总成名称"].Value.ToString() == row2.Cells["总成名称"].Value.ToString() &&
                        row1.Cells["零件名称"].Value.ToString() == row2.Cells["零件名称"].Value.ToString() &&
                        row1.Cells["图号型号"].Value.ToString() == row2.Cells["图号型号"].Value.ToString() &&
                        row1.Cells["规格"].Value.ToString() == row2.Cells["规格"].Value.ToString() &&
                        row1.Cells["批次号"].Value.ToString() == row2.Cells["批次号"].Value.ToString())
                    {
                        dataGridView1.CurrentCell = row2.Cells[0];
                        MessageDialog.ShowPromptMessage(string.Format("第[{0}]行数据与第[{1}]行数据一样，请核对后再进行此操作", i + 1, i + 2));
                        return;
                    }
                }
            }

            // 返修次数
            int repairCount = 0;

            List<string> lstProductNumber = m_electronFilesServer.GetProductNumber(
                cmbProductCode.Text, txtProductNumber.Text, txtProductNumber2.Text);

            if (lstProductNumber.Count > 1)
            {
                string showInfo = "当批量返修时，返修次数由系统根据电子档案中的返修次数自动加1，";
                showInfo += "对于返修过但没有在电子档案中有记录的返修次数会出错，您确定继续此操作吗？";

                if (MessageDialog.ShowEnquiryMessage(showInfo) != DialogResult.Yes)
                {
                    return;
                }
            }

            for (int k = 0; k < lstProductNumber.Count; k++)
            {
                m_productNumber = lstProductNumber[k];

                List<P_ElectronFile> lstEF = new List<P_ElectronFile>(dataGridView1.Rows.Count);
                Dictionary<string, object> dicParentInfo = GetParentInfo(m_productNumber, out repairCount);

                if (lstProductNumber.Count == 1)
                {                    
                    // 维持上次的返修次数
                    if (!chkPreRepairCount.Checked)
                    {
                        if (numRepairCount.Value == 0 || numRepairCount.Value < repairCount)
                        {
                            numRepairCount.Focus();
                            MessageDialog.ShowPromptMessage("返修次数不能为0 或 小于最新返修次数");
                            numRepairCount.Value = 1;
                            numRepairCount.Select(0, 10);
                            return;
                        }
                        else
                        {
                            repairCount = Convert.ToInt32(numRepairCount.Value);
                        }
                    }
                    else
                    {
                        if (repairCount == 0)
                        {
                            numRepairCount.Focus();
                            MessageDialog.ShowPromptMessage("当前变速箱未发现返修记录，返修次数为0时不允许选择【维持上次的返修次数】");
                            numRepairCount.Value = 1;
                            numRepairCount.Select(0, 10);
                            chkPreRepairCount.Checked = false;
                            return;
                        }
                    }
                }
                else
                {
                    // 维持上次的返修次数
                    if (chkPreRepairCount.Checked)
                    {
                        if (repairCount == 0)
                        {
                            chkPreRepairCount.Focus();
                            MessageDialog.ShowErrorMessage(string.Format("【{0}】变速箱没有返修记录，无法选择【维持上次的返修次数】选项", m_productNumber));
                            return;
                        }
                    }
                    else
                    {
                        repairCount++;
                    }
                }

                try
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        DataGridViewRow row = dataGridView1.Rows[i];
                        P_ElectronFile ef = m_electronFilesServer.CreateElectronFile(m_productNumber);

                        if (!CheckData(row, ef))
                            return;

                        ef.ParentName = row.Cells["总成名称"].Value.ToString();
                        ef.GoodsOnlyCode = row.Cells["标识码"].Value.ToString();
                        ef.GoodsCode = row.Cells["图号型号"].Value.ToString();
                        ef.GoodsName = row.Cells["零件名称"].Value.ToString();

                        if (GlobalObject.GeneralFunction.IsNullOrEmpty(ef.ParentName))
                        {
                            ef.GoodsOnlyCode = m_productNumber;
                        }
                        else
                        {
                            if (!dicParentInfo.ContainsKey(ef.ParentName))
                            {
                                MessageDialog.ShowErrorMessage("错误的父总成名称：" + ef.ParentName);

                                dataGridView1.CurrentCell = row.Cells[0];

                                return;
                            }

                            if (dicParentInfo[ef.ParentName] is View_P_ElectronFile)
                            {
                                ef.ParentCode = (dicParentInfo[ef.ParentName] as View_P_ElectronFile).零部件编码;
                                ef.ParentScanCode = (dicParentInfo[ef.ParentName] as View_P_ElectronFile).零件标识码;
                            }
                            else
                            {
                                ef.ParentCode = (dicParentInfo[ef.ParentName] as View_P_AssemblingBom).零件编码;
                                ef.ParentScanCode = (dicParentInfo[ef.ParentName] as View_P_AssemblingBom).备注;

                                if (dicParentInfo.ContainsKey(ef.GoodsName))
                                    ef.GoodsOnlyCode = (dicParentInfo[ef.GoodsName] as View_P_AssemblingBom).备注;
                            }
                        }

                        ef.Spec = row.Cells["规格"].Value.ToString();

                        ef.Counts = Convert.ToInt32(row.Cells["装配数量"].Value);
                        ef.BatchNo = row.Cells["批次号"].Value.ToString();
                        ef.Provider = row.Cells["供应商"].Value.ToString();

                        ef.CheckDatas = row.Cells["检测数据"].Value.ToString();
                        ef.FactDatas = row.Cells["测量数据"].Value.ToString();
                        ef.AssemblingMode = row.Cells["返修模式"].Value.ToString();

                        if (ef.AssemblingMode == "正常装配")
                        {
                            MessageDialog.ShowErrorMessage("装配模式不能是【正常装配】，请与管理员联系");
                            return;
                        }

                        ef.Remark = "快速返修：" + row.Cells["备注"].Value.ToString();
                        ef.FillInPersonnel = BasicInfo.LoginID;
                        ef.FittingPersonnel = (row.Cells["维修人员工号"].Value == null) 
                            ? BasicInfo.LoginID : row.Cells["维修人员工号"].Value.ToString();

                        lstEF.Add(ef);
                    }

                    if (!m_electronFilesServer.AddRepairInfo(m_productTypeCode, m_productName, repairCount, lstEF, out m_error))
                    {
                        MessageDialog.ShowErrorMessage(m_error);
                        return;
                    }
                }
                catch (Exception exce)
                {
                    MessageDialog.ShowErrorMessage(string.Format("出错时箱号为：{0}, 出错信息：{1}", lstProductNumber[k], exce.Message));

                    if (lstProductNumber.Count > 1)
                    {
                        MessageDialog.ShowPromptMessage("批量返修出错时修复错误后需从出错箱号开始继续后续操作");
                        txtProductNumber.Text = lstProductNumber[k].Substring(cmbProductCode.Text.Length + 1);
                        txtProductNumber.Focus();
                        txtProductNumber.SelectAll();
                    }

                    return;
                }
            }

            MessageDialog.ShowPromptMessage("操作成功");

            dataGridView1.Rows.Clear();

            btnNew_Click(sender, e);
        }

        /// <summary>
        /// 从领料单导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadDataFromMRB_Click(object sender, EventArgs e)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(m_productNumber))
            {
                MessageDialog.ShowPromptMessage("请获取变速箱信息后再进行此操作");
                return;
            }

            FormQueryInfo dialog = QueryInfoDialog.GetMaterialRequisitionBillDialog(null, CE_BillTypeEnum.领料单);

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string billNo = dialog.GetStringDataItem("领料单号");

                if (MessageDialog.ShowEnquiryMessage("您确定要导入 " + billNo +
                    " 领料单的信息吗？批量导入无法确定通用件的父总成信息，请导入完成后进行检查父总成名称、装配数量等信息。此过程需要一段时间，是否继续？") == DialogResult.No)
                {
                    return;
                }

                // 领料单服务
                IMaterialRequisitionServer billServer = ServerModuleFactory.GetServerModule<IMaterialRequisitionServer>();

                // 领料单物品清单服务
                IMaterialRequisitionGoodsServer goodsServer = ServerModuleFactory.GetServerModule<IMaterialRequisitionGoodsServer>();

                View_S_MaterialRequisition bill = billServer.GetBillView(billNo);

                List<View_S_MaterialRequisitionGoods> lstGoods =
                    (from r in goodsServer.GetGoods(billNo)
                     orderby r.显示位置
                     select r).ToList();

                foreach (var item in lstGoods)
                {
                    IEnumerable<View_P_AssemblingBom> resultInfo = null;

                    if (m_optionPartCodes.Contains(item.图号型号))
                    {
                        resultInfo = from r in m_assemblingBomInfo
                                     where r.零件名称 == item.物品名称 && r.零件编码 == item.图号型号
                                     select r;
                    }
                    else
                    {
                        if (m_productName.Contains("力帆") && (item.物品名称 == "中间轮轴" || item.物品名称 == "中间轮"))
                        {
                            resultInfo = from r in m_assemblingBomInfo
                                         where r.零件名称 == item.物品名称
                                         select r;
                        }
                        else
                        {
                            resultInfo = from r in m_assemblingBomInfo
                                         where r.零件名称 == item.物品名称 && r.零件编码 == item.图号型号 && r.规格 == item.规格
                                         select r;
                        }
                    }

                    if (resultInfo.Count() > 0)
                    {
                        View_P_AssemblingBom data = resultInfo.First();

                        if (m_optionPartCodes.Contains(item.图号型号))
                        {
                            View_P_ElectronFile optionPart = m_electronFilesServer.GetOptionPartInfo(
                                m_productTypeCode, item.图号型号, out m_error);

                            dataGridView1.Rows.Add(new object[] { "否", chkIsOnlyAdjustPart.Checked ? "局部调整" : "局部返修", 
                                data.父总成名称, item.物品名称, item.图号型号, item.规格,
                                "", item.实领数 > 2 ? data.装配数量 : item.实领数, item.供应商编码, item.批次号, "", 
                                StapleFunction.GetNonNumericString(optionPart.检测数据), 
                                StapleFunction.GetNonNumericString(optionPart.实际数据)
                                });
                        }
                        else
                        {
                            decimal count = item.实领数 < data.装配数量 ? item.实领数 : data.装配数量;

                            // 对于“众泰”的“后端盖密封垫”是选配件
                            if (m_productName.Contains("众泰") && item.物品名称 == "后端盖密封垫" && item.实领数 <= 2)
                                count = item.实领数;

                            dataGridView1.Rows.Add(new object[] { "否", chkIsOnlyAdjustPart.Checked ? "局部调整" : "局部返修", 
                                data.父总成名称, item.物品名称, item.图号型号, item.规格,
                                "", count, item.供应商编码, item.批次号, "", "", ""
                                });
                        }
                    }
                    else
                    {
                        dataGridView1.Rows.Add(new object[] { "否", chkIsOnlyAdjustPart.Checked ? "局部调整" : "局部返修", 
                                "未知父总成, 请修改总成", item.物品名称, item.图号型号, item.规格,
                                "", item.实领数, item.供应商编码, item.批次号, "", "", ""
                                });

                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Red;

                        //MessageDialog.ShowErrorMessage(string.Format("图号型号[{0}], 物品名称[{1}], 规格[{2}] 的物品并不属于当前产品",
                        //    item.图号型号, item.物品名称, item.规格));
                    }
                }
            }
        }

        /// <summary>
        /// 从营销出库单导入数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadDataFromOutboundOrder_Click(object sender, EventArgs e)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(m_productNumber))
            {
                MessageDialog.ShowPromptMessage("请获取变速箱信息后再进行此操作");
                return;
            }

            FormQueryInfo dialog = QueryInfoDialog.GetOutboundBillDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string billNo = dialog.GetStringDataItem("单据号");

                if (MessageDialog.ShowEnquiryMessage("您确定要导入 " + billNo
                    + " 营销出库单的信息吗？此过程需要一段时间，是否继续？") == DialogResult.No)
                {
                    return;
                }

                // 营销出库单服务
                ISellIn billServer = ServerModuleFactory.GetServerModule<ISellIn>();

                DataTable bill = billServer.GetBill(billNo, 0);

                string storageID = bill.Rows[0]["StorageID"].ToString();

                DataTable list = billServer.GetList((int)bill.Rows[0]["ID"]);

                if (list.Rows.Count > 0)
                {
                    List<StorageGoods> lstGoods = new List<StorageGoods>(list.Rows.Count);

                    for (int i = 0; i < list.Rows.Count; i++)
                    {
                        StorageGoods item = new StorageGoods();

                        item.GoodsCode = list.Rows[i]["GoodsCode"].ToString();
                        item.GoodsName = list.Rows[i]["GoodsName"].ToString();
                        item.Spec = list.Rows[i]["Spec"].ToString();
                        item.Provider = list.Rows[i]["Provider"].ToString();
                        item.BatchNo = list.Rows[i]["BatchNo"].ToString();
                        item.Quantity = (decimal)list.Rows[i]["Count"];
                        item.StorageID = storageID;


                        IEnumerable<View_P_AssemblingBom> resultInfo = null;

                        if (m_optionPartCodes.Contains(item.GoodsCode))
                        {
                            resultInfo = from r in m_assemblingBomInfo
                                         where r.零件名称 == item.GoodsName && r.零件编码 == item.GoodsCode
                                         select r;
                        }
                        else
                        {
                            if (m_productName.Contains("力帆") && (item.GoodsName == "中间轮轴" || item.GoodsName == "中间轮"))
                            {
                                resultInfo = from r in m_assemblingBomInfo
                                             where r.零件名称 == item.GoodsName
                                             select r;
                            }
                            else
                            {
                                resultInfo = from r in m_assemblingBomInfo
                                             where r.零件名称 == item.GoodsName && r.零件编码 == item.GoodsCode && r.规格 == item.Spec
                                             select r;
                            }
                        }

                        if (resultInfo.Count() > 0)
                        {
                            View_P_AssemblingBom data = resultInfo.First();

                            if (m_optionPartCodes.Contains(item.GoodsCode))
                            {
                                View_P_ElectronFile optionPart = m_electronFilesServer.GetOptionPartInfo(
                                    m_productTypeCode, item.GoodsCode, out m_error);

                                dataGridView1.Rows.Add(new object[] { "否", chkIsOnlyAdjustPart.Checked ? "局部调整" : "局部返修", 
                                data.父总成名称, item.GoodsName, item.GoodsCode, item.Spec,
                                "", item.Quantity > 2 ? data.装配数量 : item.Quantity, item.Provider, item.BatchNo, "", 
                                StapleFunction.GetNonNumericString(optionPart.检测数据), 
                                StapleFunction.GetNonNumericString(optionPart.实际数据)
                                });
                            }
                            else
                            {
                                decimal count = item.Quantity < data.装配数量 ? item.Quantity : data.装配数量;

                                // 对于“众泰”的“后端盖密封垫”是选配件
                                if (m_productName.Contains("众泰") && item.GoodsName == "后端盖密封垫" && item.Quantity <= 2)
                                    count = item.Quantity;

                                dataGridView1.Rows.Add(new object[] { "否", chkIsOnlyAdjustPart.Checked ? "局部调整" : "局部返修", 
                                data.父总成名称, item.GoodsName, item.GoodsCode, item.Spec,
                                "", count, item.Provider, item.BatchNo, "", "", ""
                                });
                            }
                        }
                        else
                        {
                            dataGridView1.Rows.Add(new object[] { "否", chkIsOnlyAdjustPart.Checked ? "局部调整" : "局部返修", 
                                "未知父总成, 请修改总成", item.GoodsName, item.GoodsCode, item.Spec,
                                "", item.Quantity, item.Provider, item.BatchNo, "", "", ""
                                });

                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Red;

                            //MessageDialog.ShowErrorMessage(string.Format("图号型号[{0}], 物品名称[{1}], 规格[{2}] 的物品并不属于当前产品",
                            //    item.GoodsCode, item.GoodsName, item.Spec));
                        }
                    }
                }
            }
        }

        private void 查看数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择一行数据后再进行此操作");
                return;
            }

            FormViewData form = new FormViewData(dataGridView1.Columns, dataGridView1.Rows[dataGridView1.SelectedRows[0].Index]);
            form.ShowDialog();
        }

        /// <summary>
        /// 维持上次的返修次数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkPreRepairCount_CheckedChanged(object sender, EventArgs e)
        {
            numRepairCount.Enabled = !chkPreRepairCount.Checked;
        }

        private void 快速返修变速箱_Load(object sender, EventArgs e)
        {

        }

        private void txtProductNumber2_TextChanged(object sender, EventArgs e)
        {
            btnLoadDataFromMRB.Enabled = txtProductNumber2.Text.Length == 0;
            btnLoadDataFromOutboundOrder.Enabled = txtProductNumber2.Text.Length == 0;
        }

        private void btnSetRepairedPersonnel_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请至少选择一行数据后再进行此操作");
                return;
            }

            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                dataGridView1.SelectedRows[i].Cells["维修人员工号"].Value = txtFittingPersonnel.DataResult["工号"].ToString();
                dataGridView1.SelectedRows[i].Cells["维修人员"].Value = txtFittingPersonnel.DataResult["姓名"].ToString();
            }
        }

        private void 设置气密性_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckCVTNumber())
                {
                    return;
                }

                Init();
                
                List<string> lstProductNumber = m_electronFilesServer.GetProductNumber(cmbProductCode.Text, txtProductNumber.Text, txtProductNumber2.Text);

                Form设置气密性 form = new Form设置气密性(m_productTypeCode, m_productName, lstProductNumber);

                form.ShowDialog();
            }
            catch (Exception exe)
            {
                m_productNumber = "";

                MessageDialog.ShowErrorMessage(exe.Message);
            }
        }
    }

    /// <summary>
    /// 用于DataGridView行排序的比较类
    /// </summary>
    class RowComparer : System.Collections.IComparer
    {
        private static int sortOrderModifier = 1;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sortOrder">排序方向，递增还是递减</param>
        public RowComparer(SortOrder sortOrder)
        {
            if (sortOrder == SortOrder.Descending)
            {
                sortOrderModifier = -1;
            }
            else if (sortOrder == SortOrder.Ascending)
            { 
                sortOrderModifier = 1;
            }
        }

        /// <summary>
        /// 比较方法
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            DataGridViewRow DataGridViewRow1 = (DataGridViewRow)x;
            DataGridViewRow DataGridViewRow2 = (DataGridViewRow)y;

            // Try to sort based on the Last Name column.
            int CompareResult = System.String.Compare(
                DataGridViewRow1.Cells[2].Value.ToString(),
                DataGridViewRow2.Cells[2].Value.ToString());

            // If the Last Names are equal, sort based on the First Name.
            if (CompareResult == 0)
            {
                CompareResult = System.String.Compare(
                    DataGridViewRow1.Cells[3].Value.ToString(),
                    DataGridViewRow2.Cells[3].Value.ToString());

                // If the Last Names are equal, sort based on the First Name.
                if (CompareResult == 0)
                {
                    CompareResult = System.String.Compare(
                        DataGridViewRow1.Cells[4].Value.ToString(),
                        DataGridViewRow2.Cells[4].Value.ToString());

                    // If the Last Names are equal, sort based on the First Name.
                    if (CompareResult == 0)
                    {
                        CompareResult = System.String.Compare(
                            DataGridViewRow1.Cells[5].Value.ToString(),
                            DataGridViewRow2.Cells[5].Value.ToString());

                        // If the Last Names are equal, sort based on the First Name.
                        if (CompareResult == 0)
                        {
                            CompareResult = System.String.Compare(
                                DataGridViewRow1.Cells[9].Value.ToString(),
                                DataGridViewRow2.Cells[9].Value.ToString());
                        }
                    }
                }
            }

            return CompareResult * sortOrderModifier;
        }
    }
}
