using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;
using System.IO;
using Form_Quality_File;
using Expression.Properties;

namespace Expression
{
    public partial class 质量数据库信息显示界面 : Form
    {
        Stack<string> m_back = new Stack<string>(50);//通过这两个栈来实现浏览文件时的前进与后退
        Stack<string> m_forward = new Stack<string>(50);

        string currentParentID = "";

        /// <summary>
        /// 旧文件信息
        /// </summary>
        ZL_Database_FileStruct m_oldFileStruct = new ZL_Database_FileStruct();

        /// <summary>
        /// 旧图标信息
        /// </summary>
        ListViewItem m_oldListViewItem = new ListViewItem();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 质量数据库服务接口
        /// </summary>
        IQualitySystemDatabase m_serverQylityDatabase = ServerModule.ServerModuleFactory.GetServerModule<IQualitySystemDatabase>();

        /// <summary>
        /// FTP服务组件
        /// </summary>
        FileServiceSocket m_serverFTP = new FileServiceSocket(GlobalObject.GlobalParameter.FTPServerIP,
            GlobalObject.GlobalParameter.FTPServerAdvancedUser,
            GlobalObject.GlobalParameter.FTPServerAdvancedPassword);

        /// <summary>
        /// 文件基础服务组件
        /// </summary>
        Service_Quality_File.ISystemFileBasicInfo m_serverFileBasic = 
            Service_Quality_File.ServerModuleFactory.GetServerModule<Service_Quality_File.ISystemFileBasicInfo>();

        ZL_Database_Record m_lnqRecord = new ZL_Database_Record();

        List<string> m_lstFaultType = new List<string>();

        public 质量数据库信息显示界面(string billNo)
        {
            InitializeComponent();

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.质量数据库.ToString(), m_serverQylityDatabase);
            FormInit();

            txtBillNo.Text = billNo;
        }

        private void 质量数据库信息显示界面_Load(object sender, EventArgs e)
        {
            m_lnqRecord = m_serverQylityDatabase.GetSingleInfo(txtBillNo.Text);
            ShowInfo();
        }

        /// <summary>
        /// 界面初始化
        /// </summary>
        void FormInit()
        {
            List<ZL_Database_Settings> listTemp = new List<ZL_Database_Settings>();
            ZL_Database_Settings tempItem = new ZL_Database_Settings();
            tempItem.SettingContent = "全部";

            listTemp = m_serverQylityDatabase.GetSettingInfo("不良品类型");

            cmbType.DataSource = listTemp;
            cmbType.DisplayMember = "SettingContent";
            cmbType.SelectedIndex = -1;

            listTemp = m_serverQylityDatabase.GetSettingInfo("型号");
            listTemp.Add(tempItem);

            cmbModel.DataSource = listTemp;
            cmbModel.DisplayMember = "SettingContent";
            cmbModel.SelectedIndex = -1;

            listTemp = m_serverQylityDatabase.GetSettingInfo("发现场所");
            listTemp.Add(tempItem);

            cmbFindPlaces.DataSource = listTemp;
            cmbFindPlaces.DisplayMember = "SettingContent";
            cmbFindPlaces.SelectedIndex = -1;

            listTemp = m_serverQylityDatabase.GetSettingInfo("发现者");

            cmbFindRole.DataSource = listTemp;
            cmbFindRole.DisplayMember = "SettingContent";
            cmbFindRole.SelectedIndex = -1;

            listTemp = m_serverQylityDatabase.GetSettingInfo("故障比例");

            cmbFaultRatio.DataSource = listTemp;
            cmbFaultRatio.DisplayMember = "SettingContent";
            cmbFaultRatio.SelectedIndex = -1;

            listTemp = m_serverQylityDatabase.GetSettingInfo("故障类型");
            foreach (ZL_Database_Settings item in listTemp)
            {
                m_lstFaultType.Add(item.Type + "_" + item.SettingContent);
            }
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        void ShowInfo()
        {
            if (m_lnqRecord == null)
            {
                return;
            }

            cmbFaultRatio.Text = m_lnqRecord.FaultRatio;
            cmbFindPlaces.Text = m_lnqRecord.FindPlaces;
            cmbFindRole.Text = m_lnqRecord.FindRole;
            cmbModel.Text = m_lnqRecord.Model;
            cmbType.Text = m_lnqRecord.Type;

            txtFaultDescription.Text = m_lnqRecord.FaultDescription;
            txtProvider.Text = m_lnqRecord.Provider;
            txtAssemblyCartonNo.Text = m_lnqRecord.AssemblyCartonNo;
            txtCauseAnalysis.Text = m_lnqRecord.CauseAnalysis;
            txtFaultType.Text = m_lnqRecord.FaultType;
            txtFinder.Text = m_lnqRecord.Finder;
            txtGoodsCode.Text = m_lnqRecord.GoodsCode;
            txtGoodsName.Text = m_lnqRecord.GoodsName;
            txtSpec.Text = m_lnqRecord.Spec;

            txtTreatmentCountermeasures.Text = m_lnqRecord.TreatmentCountermeasures;
            txtVersion.Text = m_lnqRecord.Version;

            numMileage.Value = m_lnqRecord.Mileage == null ? 0 : (decimal)m_lnqRecord.Mileage;
            dtpOccurrenceTime.Value = m_lnqRecord.OccurrenceTime == null ? ServerTime.Time : Convert.ToDateTime(m_lnqRecord.OccurrenceTime);

            RefreshListview();
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        void GetInfo()
        {
            m_lnqRecord = new ZL_Database_Record();

            m_lnqRecord.AssemblyCartonNo = txtAssemblyCartonNo.Text;
            m_lnqRecord.BillNo = txtBillNo.Text;
            m_lnqRecord.CauseAnalysis = txtCauseAnalysis.Text;
            m_lnqRecord.FaultRatio = cmbFaultRatio.Text;
            m_lnqRecord.Provider = txtProvider.Text;
            m_lnqRecord.FaultDescription = txtFaultDescription.Text;
            m_lnqRecord.FaultType = txtFaultType.Text;
            m_lnqRecord.Finder = txtFinder.Text;
            m_lnqRecord.FindPlaces = cmbFindPlaces.Text;
            m_lnqRecord.FindRole = cmbFindRole.Text;
            m_lnqRecord.GoodsCode = txtGoodsCode.Text;
            m_lnqRecord.GoodsName = txtGoodsName.Text;
            m_lnqRecord.Spec = txtSpec.Text;
            m_lnqRecord.Mileage = numMileage.Value;
            m_lnqRecord.Model = cmbModel.Text;
            m_lnqRecord.OccurrenceTime = Convert.ToDateTime( dtpOccurrenceTime.Value.ToShortDateString());
            m_lnqRecord.TreatmentCountermeasures = txtTreatmentCountermeasures.Text;
            m_lnqRecord.Type = cmbType.Text;
            m_lnqRecord.Version = txtVersion.Text;
        }

        /// <summary>
        /// 检测信息
        /// </summary>
        /// <returns>通过返回True,否则返回False</returns>
        bool CheckInfo()
        {
            if (cmbType.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择不良品【类型】");
                return false;
            }
            else if (cmbModel.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择【型号】");
                return false;
            }
            else if (cmbFindRole.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择【发现者】");
                return false;
            }
            else if (cmbFindPlaces.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择【发现场所】");
                return false;
            }
            else if (cmbFaultRatio.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择【故障比例】");
                return false;
            }
            else if (txtFaultType.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【故障类型】");
                return false;
            }
            else if (txtGoodsName.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择不良品【图型图号】");
                return false;
            }

            return true;
        }

        private void btnFaultType_Click(object sender, EventArgs e)
        {
            List<string> tempList = txtFaultType.Text.Split('、').ToList();

            FormDataCheck frm = new FormDataCheck(m_lstFaultType, tempList);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                string strFaultType = "";

                foreach (string item in frm.LstResult)
                {
                    strFaultType = strFaultType + item + "、";
                }

                txtFaultType.Text = strFaultType.Substring(0, strFaultType.Length - 1);
            }
        }

        private void txtGoodsCode_OnCompleteSearch()
        {
            txtGoodsCode.Text = txtGoodsCode.DataResult["图号型号"].ToString();
            txtGoodsName.Text = txtGoodsCode.DataResult["物品名称"].ToString();
            txtSpec.Text = txtGoodsCode.DataResult["规格"].ToString();

            txtGoodsCode.Tag = txtGoodsCode.DataResult["序号"];
        }

        private void txtProvider_OnCompleteSearch()
        {
            txtProvider.Text = txtProvider.DataResult["供应商名称"].ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckInfo())
                {
                    return;
                }

                GetInfo();
                m_serverQylityDatabase.EditInfo(m_lnqRecord);
                this.DialogResult = DialogResult.OK;
                MessageDialog.ShowPromptMessage("保存成功");
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 获得FTP错误信息
        /// </summary>
        bool GetError()
        {
            if (m_serverFTP.Errormessage.Length != 0)
            {
                MessageBox.Show(m_serverFTP.Errormessage);
                return false;
            }
            else
            {
                return true;
            }
        }

        #region 选择不同的显示方式

        void MenuitemIconClick(object sender, EventArgs e)
        {
            this.listView1.View = View.LargeIcon;
        }

        void MenuitemlistClick(object sender, EventArgs e)
        {
            this.listView1.View = View.List;
        }

        void DetailsClick(object sender, EventArgs e)
        {
            this.listView1.View = View.Details;
        }

        #endregion

        //创建文件夹
        private void createnewdir_Click(object sender, EventArgs e)
        {
            this.listView1.LabelEdit = true;

            ZL_Database_FileStruct tempLnq = new ZL_Database_FileStruct();

            tempLnq.BillNo = txtBillNo.Text;
            tempLnq.CreationTime = ServerTime.Time;
            tempLnq.FileName = "新建文件夹";
            tempLnq.ID = Guid.NewGuid();
            tempLnq.LastModifyTime = ServerTime.Time;
            tempLnq.ParentID = currentParentID == "" ? null : (Guid?)(new Guid(currentParentID));

            m_serverQylityDatabase.InsertFileStruct(tempLnq);

            ListViewItem item = new ListViewItem(new String[] { tempLnq.FileName, "", "文件夹", 
                            tempLnq.CreationTime.ToString(), tempLnq.LastModifyTime.ToString(), tempLnq.ID.ToString(), 
                            "", tempLnq.ParentID.ToString()}, 0);
            this.listView1.Items.Add(item);
            this.listView1.Items[this.listView1.Items.Count - 1].BeginEdit();
        }

        //创建文件
        private void createnewfile_Click(object sender, EventArgs e)
        {
            try
            {

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Guid guid = Guid.NewGuid();

                    string fileName = openFileDialog1.FileName.Substring(openFileDialog1.FileName.LastIndexOf("\\") + 1, 
                        openFileDialog1.FileName.LastIndexOf(".") - (openFileDialog1.FileName.LastIndexOf("\\") + 1));
                    string fileType = openFileDialog1.FileName.Substring(openFileDialog1.FileName.LastIndexOf("."));
                    string strFtpServerPath = "/" + ServerTime.Time.Year.ToString() + "/" + ServerTime.Time.Month.ToString() + "/";

                    CursorControl.SetWaitCursor(this);
                    m_serverFTP.Upload(openFileDialog1.FileName, strFtpServerPath + guid);
                    this.Cursor = System.Windows.Forms.Cursors.Arrow;

                    if (GetError())
                    {
                        m_serverFileBasic.AddFile(guid, strFtpServerPath + guid, fileType);

                        this.listView1.LabelEdit = true;

                        ZL_Database_FileStruct tempLnq = new ZL_Database_FileStruct();

                        tempLnq.BillNo = txtBillNo.Text;
                        tempLnq.CreationTime = ServerTime.Time;
                        tempLnq.FileUnique = guid;
                        tempLnq.FileName = fileName;
                        tempLnq.ID = Guid.NewGuid();
                        tempLnq.LastModifyTime = ServerTime.Time;
                        tempLnq.ParentID = currentParentID == "" ? null : (Guid?)(new Guid(currentParentID));

                        m_serverQylityDatabase.InsertFileStruct(tempLnq);

                        ListViewItem item = new ListViewItem(new String[] { tempLnq.FileName, 
                            m_serverFTP.GetFileSize(strFtpServerPath + guid).ToString(), fileType, 
                            tempLnq.CreationTime.ToString(), tempLnq.LastModifyTime.ToString(), tempLnq.ID.ToString(), 
                            strFtpServerPath + guid, tempLnq.ParentID.ToString()}, 0);

                        SetListViewItemExtention(fileType, ref item);

                        this.listView1.Items.Add(item);
                        this.listView1.Items[this.listView1.Items.Count - 1].BeginEdit();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
                this.Cursor = System.Windows.Forms.Cursors.Arrow;
            }
        }

        //刷新listView
        private void RefreshListview()
        {
            this.goback.Enabled = m_back.Count == 0 ? false : true;
            this.goforward.Enabled = m_forward.Count == 0 ? false : true;

            DataTable fileInfo = m_serverQylityDatabase.GetAllFileStruct(currentParentID, txtBillNo.Text);

            if (fileInfo != null)
            {
                foreach (DataRow dr in fileInfo.Rows)
                {
                    if (dr["FileType"].ToString().Trim().Length == 0)
                    {
                        ListViewItem item = new ListViewItem(new String[] { dr["FileName"].ToString(), "", "文件夹", 
                            dr["CreationTime"].ToString(), dr["LastModifyTime"].ToString(), dr["ID"].ToString(), 
                            dr["FilePath"].ToString(), dr["ParentID"].ToString()}, 0);
                        this.listView1.Items.Add(item);
                    }
                    else
                    {
                        ListViewItem item = new ListViewItem(new String[] { dr["FileName"].ToString(), 
                            m_serverFTP.GetFileSize(dr["FilePath"].ToString()).ToString(), 
                            dr["FileType"].ToString(), dr["CreationTime"].ToString(), 
                            dr["LastModifyTime"].ToString(), dr["ID"].ToString(), 
                            dr["FilePath"].ToString(), dr["ParentID"].ToString()}, 2);

                        SetListViewItemExtention(dr["FileType"].ToString(), ref item);
                        this.listView1.Items.Add(item);
                    }
                }
            }
        }

        //设置图标信息
        void SetListViewItemExtention(string extention, ref ListViewItem item)
        {
            if (extention == ".txt") { item.ImageIndex = 1; }
            else if (extention == ".log") { item.ImageIndex = 2; }
            else if (extention == ".doc") { item.ImageIndex = 3; }
            else if (extention == ".ppt") { item.ImageIndex = 4; }
            else if (extention == ".ini") { item.ImageIndex = 5; }
            else if (extention == ".xls") { item.ImageIndex = 7; }
            else if (extention == ".rar" || extention == ".zip") { item.ImageIndex = 8; }
            else if (extention == ".jpg" || extention == ".jpeg" || extention == ".png") { item.ImageIndex = 9; }
            else { item.ImageIndex = 6; }
        }

        //打开文件
        private void filediropen_Click(object sender, EventArgs e)
        {
            fileDirOpen();
        }

        //复制文件，主要是获得要复制的文件的完整路径，具体的复制操作将在粘贴时完成
        private void copydirorfile_Click(object sender, EventArgs e)
        {
            m_oldFileStruct = 
                m_serverQylityDatabase.GetFileStructInfo(this.listView1.SelectedItems[0].SubItems[5].Text.Trim());

            m_oldListViewItem = this.listView1.SelectedItems[0];

            paste.Enabled = true;
        }

        //粘贴文件
        private void paste_Click(object sender, EventArgs e)
        {
            if (m_oldFileStruct == null)
            {
                paste.Enabled = false;
                return;
            }
            else
            {
                if (m_oldFileStruct.ParentID.ToString() != currentParentID)
                {
                    m_oldFileStruct.ParentID = currentParentID == "" ? null : (Guid?)(new Guid(currentParentID));

                    m_oldListViewItem.SubItems[7].Text = m_oldFileStruct.ParentID.ToString();
                    m_serverQylityDatabase.UpdateFileStruct(m_oldFileStruct);
                    this.listView1.Items.Add(m_oldListViewItem);
                }
            }
        }

        //删除文件
        private void dirorfiledelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listView1.SelectedItems[0].SubItems[2].Text.Trim().Equals("文件夹"))
                {
                    if (MessageBox.Show("您确定要删除" + this.listView1.SelectedItems[0].Text.ToString() + "吗?", "删除文件夹",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.No)
                    {
                        return;
                    }
                }
                else
                {
                    if (MessageBox.Show("您确定要删除" + this.listView1.SelectedItems[0].Text.ToString() + "吗?", "删除文件",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.No)
                    {
                        return;
                    }
                }

                m_serverQylityDatabase.DeleteFilesStruct(this.listView1.SelectedItems[0].SubItems[5].Text.Trim());
                listView1.Items.Remove(listView1.SelectedItems[0]);
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return;
            }
        }

        //重命名文件
        private void filedirrename_Click(object sender, EventArgs e)
        {
            this.listView1.LabelEdit = true;
            this.listView1.SelectedItems[0].BeginEdit();
        }

        //打开文件或文件夹
        private void fileDirOpen()
        {
            string fileID = this.listView1.SelectedItems[0].SubItems[5].Text.Trim();

            if (!this.listView1.SelectedItems[0].SubItems[2].Text.Trim().Equals("文件夹"))
            {
                string filePath = this.listView1.SelectedItems[0].SubItems[6].Text.Trim();
                string fileType = this.listView1.SelectedItems[0].SubItems[2].Text.Trim();

                CE_SystemFileType documentType = GlobalObject.GeneralFunction.GetDocumentType(fileType);

                string winFilePath = GlobalObject.GlobalParameter.FileTempPath + filePath.Substring(filePath.LastIndexOf(@"/") + 1) + fileType;

                List<CE_FileOperatorType> listType = new List<CE_FileOperatorType>();

                listType.Add(CE_FileOperatorType.在线编辑);

                文件操作方式 frm = new 文件操作方式(listType, null, filePath, fileType);
                frm.ShowDialog();

                //m_serverFTP.Download(filePath, winFilePath);
                //GetError();

                //switch (documentType)
                //{
                //    case SystemFileType.Word:
                //    case SystemFileType.Excel:
                //    case SystemFileType.PPT:
                //        Office文件显示 frmOffice = new Office文件显示(winFilePath, documentType);
                //        frmOffice.Show();
                //        break;
                //    default:
                //        文件显示窗体 frmNormal = new 文件显示窗体(winFilePath);
                //        frmNormal.Show();
                //        break;
                //}
            }
            else
            {
                this.m_back.Push(this.listView1.SelectedItems[0].SubItems[5].Text.Trim() + "."
                    + this.listView1.SelectedItems[0].SubItems[7].Text.Trim());
                this.listView1.Items.Clear();
                currentParentID = fileID;
                RefreshListview();
            }
        }

        //后退浏览
        private void goback_Click(object sender, EventArgs e)
        {
            if (m_back.Count > 0)
            {
                string currentpath = this.m_back.Pop();
                this.m_forward.Push(currentpath);
                this.listView1.Items.Clear();

                currentParentID = currentpath.Substring(currentpath.IndexOf(".") + 1,
                    currentpath.Length - (currentpath.IndexOf(".") + 1));

                RefreshListview();

                if (m_back.Count == 0) { this.goback.Enabled = false; }
            }
        }

        //前进浏览
        private void goforward_Click(object sender, EventArgs e)
        {
            if (m_forward.Count > 0)
            {
                string currentpath = this.m_forward.Pop();
                this.m_back.Push(currentpath);
                this.listView1.Items.Clear();
                currentParentID = currentpath.Substring(0, currentpath.IndexOf("."));
                RefreshListview();

                if (m_forward.Count == 0) { this.goforward.Enabled = false; }
            }
        }

        private void listView1_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label == null)
            {
                return;
            }
            else
            {
                ZL_Database_FileStruct tempFileStruct =
                    m_serverQylityDatabase.GetFileStructInfo(this.listView1.SelectedItems[0].SubItems[5].Text.Trim());
                tempFileStruct.FileName = e.Label;

                if (this.listView1.SelectedItems[0].SubItems[2].Text.Trim().Equals("文件夹"))
                {
                    foreach (ListViewItem item in listView1.Items)
                    {
                        if (item.SubItems[2].Text.Trim().Equals("文件夹")
                            && item.SubItems[5].Text.Trim() != tempFileStruct.ID.ToString()
                            && item.SubItems[0].Text.Trim() == tempFileStruct.FileName)
                        {
                            MessageBox.Show("无法从命名文件夹" + e.Label + " " + "制定文件夹与现有文件夹同名，请另外指定文件夹名", "文件夹重命名错误", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                            e.CancelEdit = true; 
                            return;
                        }
                    }
                }
                else
                {
                    foreach (ListViewItem item in listView1.Items)
                    {
                        if (!item.SubItems[2].Text.Trim().Equals("文件夹")
                            && item.SubItems[5].Text.Trim() != tempFileStruct.ID.ToString()
                            && item.SubItems[0].Text.Trim() == tempFileStruct.FileName)
                        {
                            MessageBox.Show("无法从命名文件" + e.Label + " " + "制定文件与现有文件同名，请另外指定文件名", "文件重命名错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            e.CancelEdit = true;
                            return;
                        }
                    }
                }

                m_serverQylityDatabase.UpdateFileStruct(tempFileStruct);
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            fileDirOpen();
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listView1.SelectedItems.Count == 0)
                {
                    this.listView1.ContextMenuStrip = this.contextMenuStrip1;
                }
                else
                {
                    this.listView1.ContextMenuStrip = this.contextMenuStrip2;
                }
            }
        }

        private void 质量数据库信息显示界面_FormClosing(object sender, FormClosingEventArgs e)
        {
            string billNo = txtBillNo.Text;

            ZL_Database_Record record = m_serverQylityDatabase.GetSingleInfo(billNo);

            if (record == null)
            {
                DataTable tempTable = m_serverQylityDatabase.GetAllFileStruct(null, billNo);

                if (tempTable != null)
                {
                    foreach (DataRow dr in tempTable.Rows)
                    {
                        m_serverQylityDatabase.DeleteFilesStruct(dr["ID"].ToString());
                    }
                }
            }
        }
    }
}
