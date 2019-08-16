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
using System.IO;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 质量信息反馈信息界面
    /// </summary>
    public partial class 质量信息反馈单 : Form
    {
        /// <summary>
        /// 库存信息服务组件
        /// </summary>
        IStoreServer m_serverStore = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 供应商服务组件
        /// </summary>
        IProviderServer m_serverProvider = ServerModuleFactory.GetServerModule<IProviderServer>();

        /// <summary>
        /// 报检入库服务组件
        /// </summary>
        ICheckOutInDepotServer m_serverCheckOutInDepot = ServerModuleFactory.GetServerModule<ICheckOutInDepotServer>();

        /// <summary>
        /// 质量信息反馈单组件
        /// </summary>
        IMessMessageFeedback m_serverMess = ServerModuleFactory.GetServerModule<IMessMessageFeedback>();

        /// <summary>
        /// 质量信息反馈单LINQ数据集
        /// </summary>
        private S_MessMessageFeedback m_lnqMess = new S_MessMessageFeedback();

        public S_MessMessageFeedback LnqMess
        {
            get { return m_lnqMess; }
            set { m_lnqMess = value; }
        }

        /// <summary>
        /// 报检入库单视图LINQ数据集
        /// </summary>
        View_S_CheckOutInDepotBill m_lnqViewCheck = new View_S_CheckOutInDepotBill();

        /// <summary>
        /// 供货状态
        /// </summary>
        string m_strGHZT;

        /// <summary>
        /// 信息来源
        /// </summary>
        string m_strXXLY;

        /// <summary>
        /// SQE意见
        /// </summary>
        string m_strSQEYJ;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 标志
        /// </summary>
        bool m_blFlag = false;

        public 质量信息反馈单(string strDJH)
        {
            InitializeComponent();

            if (strDJH != "")
            {
                m_lnqMess = m_serverMess.GetData(strDJH);

                m_lnqViewCheck = m_serverMess.GetCheckInDepotBill(m_lnqMess.InDepotBillID);
            }
            else
            {
                m_blFlag = true;
                txtDJH.Text = "系统自动生成";
                txtName.Enabled = true;
                txtBatchNo.Enabled = true;
                groupBox4.Text = groupBox6.Text;
                groupBox6.Text = "";
            }
        }

        void txtBatchNo_OnCompleteSearch()
        {
            if (txtName.Tag == null)
            {
                MessageDialog.ShowPromptMessage("请选择物品名称");
                return;
            }

            string strDJH = m_serverCheckOutInDepot.GetBillNo(
                Convert.ToInt32(txtName.Tag), txtBatchNo.Text);

            if (strDJH != "")
            {
                txtBatchNo.Text = txtBatchNo.DataResult["批次号"].ToString();
                m_lnqMess.InDepotBillID = strDJH;
                m_lnqViewCheck = m_serverMess.GetCheckInDepotBill(m_lnqMess.InDepotBillID);

                ShowAllMessage();
                ShowElesMessage();
            }

            txtDJZT.Text = "等待STA验证";
            txtDJH.Text = m_serverMess.GetBillID();
        }

        void txtName_OnCompleteSearch()
        {
            txtName.Text = txtName.DataResult["物品名称"].ToString();
            txtCode.Text = txtName.DataResult["图号型号"].ToString();
            txtSpec.Text = txtName.DataResult["规格"].ToString();
            txtDepot.Text = txtName.DataResult["物品类别"].ToString();
            txtName.Tag = Convert.ToInt32(txtName.DataResult["序号"]);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            GetAllMessage();

            MessageBox.Show("保存成功！","提示");
            this.Close();
        }

        /// <summary>
        /// 获得全部信息
        /// </summary>
        private void GetAllMessage()
        {
            CheckGHZT();
            CheckSQEJY();
            CheckXXLY();
            m_lnqMess.DJH = txtDJH.Text;
            m_lnqMess.DJZT = txtDJZT.Text;
            m_lnqMess.InDepotBillID = txtInDepotBillID.Text;
            m_lnqMess.LinkMan = txtLinkMan.Text;
            m_lnqMess.LinkPhone = txtLinkPhone.Text;
            m_lnqMess.LinkEmail = txtLinkEmail.Text;
            m_lnqMess.RevertTime = dtpRevertTime.Value;
            m_lnqMess.JLRD_LinkEmail = txtJLRD_LinkEmail.Text;
            m_lnqMess.JLRD_LinkMan = txtJLRD_LinkMan.Text;
            m_lnqMess.JLRD_LinkPhone = txtJLRD_LinkPhone.Text;
            m_lnqMess.ForGoodsStatus = m_strGHZT;
            m_lnqMess.MessageFromStatus = m_strXXLY;
            m_lnqMess.DisqualificationDepict = txtBHGPMS.Text;
            m_lnqMess.SQEMindStatus = m_strSQEYJ;
            m_lnqMess.SQEElseMindMessage = txtSQEQTYJ.Text;
            m_lnqMess.SQEvalidateMessage = txtYZJG.Text;
            m_lnqMess.QEAffirmMessage = txtQRJG.Text;

            if (pcbPic.Image != null)
            {
                m_lnqMess.Picture = GetPicToBinary(pcbPic.Image);
            }
            
            m_lnqMess.AllCount = numAllCount.Value;
            m_lnqMess.DefectiveRate = numDefectiveRate.Value;

        }

        /// <summary>
        /// 展示主要信息
        /// </summary>
        private void ShowAllMessage()
        {
            
            txtDJH.Text = m_lnqMess.DJH;
            txtDJZT.Text = m_lnqMess.DJZT;
            txtProviderName.Text = m_serverProvider.GetPrivderName(m_lnqViewCheck.供货单位);
            txtInDepotBillID.Text = m_lnqMess.InDepotBillID;

            if (m_lnqMess.LinkMan == null && m_lnqMess.LinkEmail == null && m_lnqMess.LinkPhone == null
                && m_lnqMess.JLRD_LinkEmail == null && m_lnqMess.JLRD_LinkMan == null && m_lnqMess.JLRD_LinkPhone == null)
            {
                DataTable dt = m_serverMess.GetNearestLinkManInfo(txtProviderName.Text.Trim());

                if (dt != null && dt.Rows.Count > 0)
                {
                    txtLinkMan.Text = dt.Rows[0]["LinkMan"].ToString();
                    txtLinkPhone.Text = dt.Rows[0]["LinkPhone"].ToString();
                    txtLinkEmail.Text = dt.Rows[0]["LinkEmail"].ToString();
                    txtJLRD_LinkMan.Text = dt.Rows[0]["JLRD_LinkMan"].ToString();
                    txtJLRD_LinkEmail.Text = dt.Rows[0]["JLRD_LinkEmail"].ToString();
                    txtJLRD_LinkPhone.Text = dt.Rows[0]["JLRD_LinkPhone"].ToString();
                }
            }
            else
            {
                txtLinkMan.Text = m_lnqMess.LinkMan;
                txtLinkPhone.Text = m_lnqMess.LinkPhone;
                txtLinkEmail.Text = m_lnqMess.LinkEmail;
                txtJLRD_LinkMan.Text = m_lnqMess.JLRD_LinkMan;
                txtJLRD_LinkEmail.Text = m_lnqMess.JLRD_LinkEmail;
                txtJLRD_LinkPhone.Text = m_lnqMess.JLRD_LinkPhone;
            }

            dtpRevertTime.Value = m_lnqMess.RevertTime == null?
                ServerTime.Time : m_lnqMess.RevertTime.Value;
            m_strGHZT = m_lnqMess.ForGoodsStatus;
            m_strXXLY = m_lnqMess.MessageFromStatus;
            txtBHGPMS.Text = m_lnqMess.DisqualificationDepict;
            m_strSQEYJ = m_lnqMess.SQEMindStatus;
            txtSQEQTYJ.Text = m_lnqMess.SQEElseMindMessage;
            txtYZJG.Text = m_lnqMess.SQEvalidateMessage;
            txtQRJG.Text = m_lnqMess.QEAffirmMessage;
            pcbPic.Image = m_lnqMess.Picture == null ? 
                null :GetPicture(m_lnqMess.Picture.ToArray());
            numDefectiveRate.Value = m_lnqMess.DefectiveRate == null ?
                0 : (decimal)m_lnqMess.DefectiveRate;

            SetGHZT();
            SetXXLY();
            SetSQEYJ();
        }

        /// <summary>
        /// 展示次要信息
        /// </summary>
        public void ShowElesMessage()
        {
            txtName.Text = m_lnqViewCheck.物品名称;
            txtCode.Text = m_lnqViewCheck.图号型号;
            txtDepot.Text = m_lnqViewCheck.仓库;
            txtSpec.Text = m_lnqViewCheck.规格;
            dtpDHRQ.Value = m_lnqViewCheck.到货日期;
            txtOrderNumber.Text = m_lnqViewCheck.订单号;
            txtBatchNo.Text = m_lnqViewCheck.批次号;
            lbJYR.Text = UniversalFunction.GetPersonnelName(m_lnqMess.QCRY);
            lbJYRQ.Text = m_lnqMess.QCRQ.ToString();
            lbSQEYJ.Text = UniversalFunction.GetPersonnelName(m_lnqMess.SQERY);
            lbSQERQ.Text = m_lnqMess.SQERQ.ToString();
            lbSQEYZ.Text = UniversalFunction.GetPersonnelName(m_lnqMess.SQEYZRY);
            lbSQEYZRQ.Text = m_lnqMess.SQEYZRQ.ToString();
            lbzlgcs.Text = UniversalFunction.GetPersonnelName(m_lnqMess.QEQRRY);
            lbzlgcsrq.Text = m_lnqMess.QEQRRQ.ToString();

            if (m_lnqMess.AllCount == null)
            {
                SetAllCount();
            }
            else
            {
                numAllCount.Value = (decimal)m_lnqMess.AllCount;
            }
        }

        /// <summary>
        /// 设置本批总数
        /// </summary>
        public void SetAllCount()
        {
            decimal dcCount = m_serverMess.GetAllCount(Convert.ToInt32(m_lnqViewCheck.物品ID), 
                m_lnqViewCheck.批次号,
                m_lnqViewCheck.库房代码, out m_err);

            if (dcCount == 0)
            {

                DataTable dt = m_serverStore.GetGoodsStockInfo(Convert.ToInt32(m_lnqViewCheck.物品ID),
                    m_lnqViewCheck.批次号, "", m_lnqViewCheck.库房代码);

                if (dt == null || dt.Rows.Count == 0)
                {
                    numAllCount.Value = 0;
                }
                else
                {
                    numAllCount.Value = Convert.ToDecimal(dt.Rows[0]["ExistCount"]);
                }
            }
            else
            {
                numAllCount.Value = dcCount;
            }

        }

        /// <summary>
        /// 获得供货状态
        /// </summary>
        private void CheckGHZT()
        {
            if (rbGHZTPL.Checked)
            {
                m_strGHZT = rbGHZTPL.Text;
                return;
            }

            if (rbGHZTXPL.Checked)
            {
                m_strGHZT = rbGHZTXPL.Text;
                return;
            }

            if (rbGHZTYJ.Checked)
            {
                m_strGHZT = rbGHZTYJ.Text;
                return;
            }
        }

        /// <summary>
        /// 获得信息来源
        /// </summary>
        private void CheckXXLY()
        {
            if (rbXXLYGK.Checked)
            {
                m_strXXLY = rbXXLYGK.Text;
                return;
            }

            if (rbXXLYJHJY.Checked)
            {
                m_strXXLY = rbXXLYJHJY.Text;
                return;
            }

            if (rbXXLYSCGC.Checked)
            {
                m_strXXLY = rbXXLYSCGC.Text;
                return;
            }
        }

        /// <summary>
        /// 获得SQE意见
        /// </summary>
        private void CheckSQEJY()
        {
            if (rbSQEQT.Checked)
            {
                m_strSQEYJ = rbSQEQT.Text;
                return;
            }

            if (rbSQESP.Checked)
            {
                m_strSQEYJ = rbSQESP.Text;
                return;
            }

            if (rbSQETCZD.Checked)
            {
                m_strSQEYJ = rbSQETCZD.Text;
                return;
            }

            if (rbSQEPRCL.Checked)
            {
                m_strSQEYJ = rbSQEPRCL.Text;
                return;
            }
        }

        /// <summary>
        /// 设置供货状态
        /// </summary>
        private void SetGHZT()
        {
            if (rbGHZTPL.Text == m_strGHZT)
            {
                rbGHZTPL.Checked = true;
                return;
            }

            if (rbGHZTXPL.Text == m_strGHZT)
            {
                rbGHZTXPL.Checked = true;
                return;
            }

            if (rbGHZTYJ.Text == m_strGHZT)
            {
                rbGHZTYJ.Checked = true;
                return;
            }
        }

        /// <summary>
        /// 设置信息来源
        /// </summary>
        private void SetXXLY()
        {
            if (rbXXLYGK.Text == m_strXXLY)
            {
                rbXXLYGK.Checked = true;
                return;
            }

            if (rbXXLYJHJY.Text == m_strXXLY)
            {
                rbXXLYJHJY.Checked = true;
                return;
            }

            if (rbXXLYSCGC.Text == m_strXXLY)
            {
                rbXXLYSCGC.Checked = true;
                return;
            }
        }

        /// <summary>
        /// 设置SQE意见
        /// </summary>
        private void SetSQEYJ()
        {
            if (rbSQEQT.Text == m_strSQEYJ)
            {
                rbSQEQT.Checked = true;
                return;
            }

            if (rbSQESP.Text == m_strSQEYJ)
            {
                rbSQESP.Checked = true;
                return;
            }

            if (rbSQETCZD.Text == m_strSQEYJ)
            {
                rbSQETCZD.Checked = true;
                return;
            }

            if (rbSQEPRCL.Text == m_strSQEYJ)
            {
                rbSQEPRCL.Checked = true;
                return;
            }
        }

        private void 质量信息反馈单_Load(object sender, EventArgs e)
        {
            if (!m_blFlag)
            {
                ShowAllMessage();
                ShowElesMessage();
            }
        }

        private void btnOutExcel_Click(object sender, EventArgs e)
        {

            IBillReportInfo report = new 报表_质量信息反馈单(m_lnqMess.DJH, "");
            PrintReportBill print = new PrintReportBill(21, 29.7, report);
            (report as 报表_质量信息反馈单).ShowDialog();
        }

        private void lbUpPicture_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdPic = new OpenFileDialog();    
            //取得或設定目前的檔名擴展名，以決定出現在對話方塊中[檔案類型] 的選項。    
            ofdPic.Filter = "JPG(*.JPG;*.JPEG);gif文件(*.GIF)|*.jpg;*.jpeg;*.gif";    
            //取得或設定檔案對話方塊中目前所選取之篩選條件的索引    
            ofdPic.FilterIndex = 1;    
            //關閉對話框，還原當前的目錄    
            ofdPic.RestoreDirectory = true;    
            //取得或設定含有檔案對話方塊中所選文件的名稱。    
            ofdPic.FileName = "";

            if (ofdPic.ShowDialog() == DialogResult.OK)
            {
                if (pcbPic.Image != null)
                {
                    pcbPic.Image.Dispose();
                    pcbPic.Image = null;
                }
                //得到文件名及路徑    
                string sPicPaht = ofdPic.FileName.ToString();

                //FileInfo：提供建立、複製、刪除、移動和開啟檔案的執行個體 (Instance) 方法    
                FileInfo fiPicInfo = new FileInfo(sPicPaht);
                //Length：取得目前檔案的大小。以字節為單位    
                long lPicLong = fiPicInfo.Length / 1024;
                //得到文名    
                string sPicName = fiPicInfo.Name;
                //取得父目錄    
                string sPicDirectory = fiPicInfo.Directory.ToString();
                //DirectoryName ：取得表示目錄完整路徑。    
                string sPicDirectoryPath = fiPicInfo.DirectoryName;



                //封裝GDI+點陣圖像，是用來處理像素資料所定義影像的物件。    
                //Bitmap類：封裝GDI+ 點陣圖，這個點陣圖是由圖形影像的像素資料及其屬性所組成。Bitmap 是用來處理像素資料所定義影像的物件。   

                
                //如果文件大於500KB，警告    
                if (lPicLong > 4000)
                {
                    MessageBox.Show("此文件大小為" + lPicLong + "K；已超過最大限制的400K范圍！");
                }
                else
                {
                    pcbPic.Image = Bitmap.FromFile(sPicPaht);
                }
            }

        }

        private static byte[] GetPicToBinary(Image image)
        {
            MemoryStream ms = new MemoryStream();
            Bitmap bitmap = new Bitmap(image);
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            byte[] b = ms.ToArray();
            return b;
        }

        private static Image GetPicture(byte[] byPct)
        {
            MemoryStream ms = new MemoryStream(byPct);
            ms.Write(byPct, 0, byPct.Length);
            Image im = Bitmap.FromStream(ms);

            ms.Close();
            return im;
        }

        private void txtBatchNo_Enter(object sender, EventArgs e)
        {
            if (txtName.Tag == null || txtName.Tag.ToString() == "")
            {
                return;
            }
            else
            {
                txtBatchNo.StrEndSql = " and 物品ID = " + Convert.ToInt32(txtName.Tag);
            }
        }
    }
}
