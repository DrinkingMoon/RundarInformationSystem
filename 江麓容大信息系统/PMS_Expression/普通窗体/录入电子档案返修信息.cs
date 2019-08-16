using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using System.Text.RegularExpressions;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 录入电子档案返修信息 : Form
    {
        /// <summary>
        /// 从外部传入的数据行（为当前选中的档案信息）
        /// </summary>
        private DataRowData m_dataRow;

        /// <summary>
        /// 条形码服务
        /// </summary>
        private IBarCodeServer m_barCodeServer = ServerModuleFactory.GetServerModule<IBarCodeServer>();

        /// <summary>
        /// 选配零件服务
        /// </summary>
        private IChoseConfectServer m_choseConfectServer = ServerModuleFactory.GetServerModule<IChoseConfectServer>();

        /// <summary>
        /// 电子档案服务
        /// </summary>
        private IElectronFileServer m_electronFileServer = ServerModuleFactory.GetServerModule<IElectronFileServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        private string m_error;

        /// <summary>
        /// 关闭窗体事件
        /// </summary>
        public GlobalObject.DelegateCollection.CloseFormDelegate CloseChildFormEvent;

        /// <summary>
        /// 更新模式枚举定义
        /// </summary>
        public enum UpdateModeEnum { 返修, 调整 };

        /// <summary>
        /// 更新模式
        /// </summary>
        private UpdateModeEnum m_updateMode;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="updateMode">更新模式</param>
        /// <param name="dataRow">当前选中的档案信息</param>
        public 录入电子档案返修信息(UpdateModeEnum updateMode, DataRow dataRow)
        {
            InitializeComponent();

            System.Diagnostics.Debug.Assert(dataRow != null);
            m_dataRow = new DataRowData(dataRow);

            m_updateMode = updateMode;

            InitFace(updateMode);
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        /// <param name="updateMode">更新模式</param>
        private void InitFace(UpdateModeEnum updateMode)
        {
            numOldAssemblyAmount.Value = Convert.ToInt32(m_dataRow.GetData("数量"));
            numAssemblyAmount.Maximum = numOldAssemblyAmount.Value;
            txtGoodsCode.Text = m_dataRow["零部件编码"];
            txtGoodsName.Text = m_dataRow["零部件名称"];
            txtOldBatchNo.Text = m_dataRow["批次号"];

            txtOldCheckData.Text = m_dataRow["检测数据"];
            txtOldResultData.Text = m_dataRow["实际数据"];

            CopyMeasureData();

            txtAssemblyDate.Text = m_dataRow["装配时间"];
            txtAssemblyPersonnel.Text = m_dataRow["装配人员"];

            if (updateMode == UpdateModeEnum.调整)
            {
                numBarcode.Enabled = false;
                btnGetBarCodeInfo.Enabled = false;

                txtPartID.Text = m_dataRow["零件标识码"];
                txtPartID.Enabled = false;

                numAssemblyAmount.Enabled = false;
                numAssemblyAmount.Value = numOldAssemblyAmount.Value;

                txtProvider.Text = m_dataRow["供应商"];
                txtSpec.Text = m_dataRow["规格"];
                txtBatchNo.Text = m_dataRow["批次号"];
            }

        }

        /// <summary>
        /// 从旧数据中拷贝检测数据、实现装配数据到新区域
        /// </summary>
        private void CopyMeasureData()
        {
            if (txtOldCheckData.Text == "")
            {
                return;
            }

            txtCheckData.Text = StapleFunction.GetNonNumericString(txtOldCheckData.Text);
            txtResultData.Text = StapleFunction.GetNonNumericString(txtOldResultData.Text);
        }

        /// <summary>
        /// 获取条形码信息
        /// </summary>
        /// <param name="barCodeID">条形码编号</param>
        /// <returns>成功返回获取到的条形码信息，失败返回null</returns>
        private View_S_InDepotGoodsBarCodeTable GetBarCodeInfo(int barCodeID)
        {
            View_S_InDepotGoodsBarCodeTable goodsInfo = null;
         
            if (!m_barCodeServer.GetData(barCodeID, out goodsInfo, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return null;
            }

            return goodsInfo;
        }

        /// <summary>
        /// 获取条形码信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetBarCodeInfo_Click(object sender, EventArgs e)
        {
            if (numBarcode.Value == 0m)
            {
                MessageDialog.ShowPromptMessage("条形码不能是0");
                return;
            }

            View_S_InDepotGoodsBarCodeTable goodsInfo = GetBarCodeInfo(Convert.ToInt32(numBarcode.Value));

            if (goodsInfo != null)
            {
                bool isValid = false;

                if (goodsInfo.图号型号 == txtGoodsCode.Text)
                {
                    // 是否选配零件，是选配零件则不要检查规格
                    if (m_choseConfectServer.IsExistChoseConfectInfo(goodsInfo.图号型号))
                    {
                        isValid = true;
                    }
                    else
                    {
                        if (goodsInfo.规格 == m_dataRow["规格"])
                            isValid = true;
                    }
                }

                if (isValid)
                {
                    txtBatchNo.Text = goodsInfo.批次号;
                    txtProvider.Text = goodsInfo.供货单位;
                    txtSpec.Text = goodsInfo.规格;
                }
                else
                {
                    MessageDialog.ShowErrorMessage(
                        string.Format("您录入的条形码【{0}】物品名称为【{1}】规格为【{2}】，与返修零件不符合", 
                        goodsInfo.条形码, goodsInfo.物品名称, goodsInfo.规格));
                }
            }
        }

        /// <summary>
        /// 检查数字录入是否正确
        /// </summary>
        /// <param name="strData">要检查的字符串</param>
        /// <returns>正确返回true</returns>
        private bool CheckNumericInfo(string strData)
        {
            int index = strData.IndexOf('=');
            int nextIndex = -1;
            string strValue;
            Regex r = new Regex(@"(^[+-]?(\d+)(\.\d+)?$)|(^[+-]?\d+(\.\d+)?[+]\d+(\.\d+)?$)");  // 同时匹配 +0.18 or +0.18+0.20

            while (index != -1)
            {
                nextIndex = strData.IndexOf(',', index);

                if (nextIndex == -1)
                {
                    nextIndex = strData.Length - 1;
                }

                if (nextIndex == index + 1)
                {
                    return false;
                }
                else
                {

                    if (StapleFunction.IsChineseCharacters(strData.Substring(nextIndex - 1, 1)))
                    {
                        strValue = strData.Substring(index + 1, nextIndex - index - 2);
                    }
                    else
                    {
                        strValue = strData.Substring(index + 1, nextIndex - index - 1);
                    }

                    //Regex r = new Regex(@"^[+-]?(\d+)(\.\d+)?$"); // 匹配浮点数
                    //Regex r = new Regex(@"^[+-]?\d+(\.\d+)?[+]\d+(\.\d+)?$"); // 匹配 +0.18+0.20

                    if (!r.IsMatch(strValue))
                        return false;
                }

                index = strData.IndexOf('=', nextIndex + 1);
            }

            index = strData.IndexOf(':');

            while (index != -1)
            {
                nextIndex = strData.IndexOf(',', index);

                if (nextIndex == -1)
                {
                    nextIndex = strData.Length;
                }

                if (nextIndex == index + 1)
                {
                    return false;
                }
                else
                {

                    if (StapleFunction.IsChineseCharacters(strData.Substring(nextIndex - 1, 1)))
                    {
                        strValue = strData.Substring(index + 1, nextIndex - index - 2);
                    }
                    else
                    {
                        strValue = strData.Substring(index + 1, nextIndex - index - 1);
                    }

                    if (!r.IsMatch(strValue))
                        return false;
                }

                index = nextIndex == strData.Length ? -1 : strData.IndexOf(':', nextIndex + 1);
            }

            return true;
        }

        /// <summary>
        /// 检查规格是否正确
        /// </summary>
        /// <param name="error">输出错误信息</param>
        /// <returns>正确返回true</returns>
        private bool CheckSpec(out string error)
        {
            double dblSpec = 0;

            error = null;

            if (m_dataRow["规格"].Length > 0)
                dblSpec = Convert.ToDouble(txtSpec.Text.Replace("mm", ""));

            int index = txtCheckData.Text.LastIndexOf(':');

            if (index + 1 == txtCheckData.Text.Length)
            {
                error = "没有选配信息";
                return false;
            }

            string[] strSpec = txtCheckData.Text.Substring(index + 1).Split(new char[] { '+' });

            if (strSpec.Length > 2)
            {
                error = "选配信息不正确";
                return false;
            }

            double value = 0;
            bool find = false;

            for (int i = 0; i < strSpec.Length; i++)
            {
                if (!Double.TryParse(strSpec[i], out value))
                {
                    error = "选配信息不正确";
                    return false;
                }

                if (value == dblSpec)
                {
                    find = true;
                }
            }

            if (!find)
            {
                error = "选配信息与当前规格不匹配，扫描零件的规格必须与选配结果一致才行";
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 检查录入的信息是否正确
        /// </summary>
        /// <returns>正确返回true</returns>
        private bool CheckData()
        {
            if (m_dataRow["父总成编码"] == "")
            {
                MessageDialog.ShowPromptMessage("一级总成不允许进行此操作");
                return false;
            }

            if (m_updateMode != UpdateModeEnum.调整 && txtBatchNo.Text.Length == 0)
            {
                numBarcode.Focus();
                MessageDialog.ShowPromptMessage("请获取返修件的条形码后再进行此操作");
                return false;
            }

            if (numAssemblyAmount.Value == 0m)
            {
                numAssemblyAmount.Focus();
                MessageDialog.ShowPromptMessage("请录入装配数量后再进行此操作");
                return false;
            }

            if (txtRemark.Text.Trim().Length == 0)
            {
                txtRemark.Focus();
                MessageDialog.ShowPromptMessage("请录入备注信息后再进行此操作");
                return false;
            }

            if (m_electronFileServer.IsAssemblyPart(m_dataRow["零部件编码"]))
            {
                return true;
            }

            string error;

            if (txtOldCheckData.Text.Length > 0)
            {
                if (StapleFunction.GetNonNumericString(txtOldCheckData.Text) != StapleFunction.GetNonNumericString(txtCheckData.Text) 
                    || !CheckNumericInfo(txtCheckData.Text))
                {
                    txtCheckData.Focus();
                    MessageDialog.ShowPromptMessage("请录入正确的检测数据后再进行此操作（效仿数据区的检测数据录入格式）");
                    return false;
                }

                if (!CheckSpec(out error))
                {
                    txtCheckData.Focus();
                    MessageDialog.ShowPromptMessage(error);
                    return false;
                }

                int index = txtResultData.Text.LastIndexOf(':');
                double value = 0;

                if (index + 1 == txtResultData.Text.Length
                    || StapleFunction.GetNonNumericString(txtResultData.Text) != StapleFunction.GetNonNumericString(txtResultData.Text) 
                    || !CheckNumericInfo(txtResultData.Text)
                    || !Double.TryParse(txtResultData.Text.Substring(index + 1), out value))
                {
                    txtResultData.Focus();
                    MessageDialog.ShowPromptMessage("请录入正确的测量数据后再进行此操作（效仿数据区的测量数据录入格式）");
                    return false;
                }
            }
            else
            {
                if (txtSpec.Text != m_dataRow["规格"])
                {
                    numBarcode.Focus();
                    MessageDialog.ShowPromptMessage("扫描零件的规格必须与原规格一致");
                    return false;
                }
            }

            if (m_dataRow["零件标识码"].Length > 0 && txtPartID.Text.Trim().Length == 0)
            {
                txtPartID.Focus();
                MessageDialog.ShowPromptMessage("请录入零件标识码后再进行此操作");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 添加返修信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckData())
            {
                return;
            }

            P_ElectronFile ef = m_electronFileServer.CreateElectronFile(m_dataRow["产品编码"]);
            
            ef.ParentCode = m_dataRow["父总成编码"];
            ef.ParentName = m_dataRow["父总成名称"];
            ef.ParentScanCode = m_dataRow["父总成扫描码"];
            ef.GoodsCode = txtGoodsCode.Text;
            ef.GoodsName = txtGoodsName.Text;
            ef.GoodsOnlyCode = txtPartID.Text;
            ef.Spec = txtSpec.Text;
            ef.Counts = Convert.ToInt32(numAssemblyAmount.Value);
            ef.Provider = txtProvider.Text;
            ef.BatchNo = txtBatchNo.Text;
            ef.CheckDatas = txtCheckData.Text;
            ef.FactDatas = txtResultData.Text;
            ef.AssemblingMode = m_updateMode.ToString();
            ef.Remark = txtRemark.Text;
            ef.WorkBench = m_dataRow["工位"];

            if (!m_electronFileServer.AddElectronFile(ef, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
            }
            else
            {
                MessageDialog.ShowPromptMessage("操作成功");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void 录入电子档案返修信息_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (CloseChildFormEvent != null)
                CloseChildFormEvent(this.DialogResult);
        }
    }
}
