using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;
using PlatformManagement;
using Service_Quality_File;
using Expression;
using UniversalControlLibrary;
using Form_Quality_File.Properties;

namespace Form_Quality_File
{
    public partial class 体系文件结构 : Form
    {
        /// <summary>
        /// 文件基础服务组件
        /// </summary>
        ISystemFileBasicInfo m_serverFileBasic = Service_Quality_File.ServerModuleFactory.GetServerModule<ISystemFileBasicInfo>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr = "";

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 体系文件基础信息服务组件
        /// </summary>
        ISystemFileBasicInfo m_serverBasicInfo = Service_Quality_File.ServerModuleFactory.GetServerModule<ISystemFileBasicInfo>();

        /// <summary>
        /// FTP服务组件
        /// </summary>
        FileServiceSocket m_serverFTP = new FileServiceSocket(GlobalObject.GlobalParameter.FTPServerIP,
            GlobalObject.GlobalParameter.FTPServerAdvancedUser,
            GlobalObject.GlobalParameter.FTPServerAdvancedPassword);

        public 体系文件结构(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent(); 

            m_authFlag = nodeInfo.Authority;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, authorityFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);
        }

        /// <summary>
        /// 获得FTP错误信息
        /// </summary>
        bool GetError()
        {
            if (m_serverFTP.Errormessage.Length != 0)
            {
                MessageDialog.ShowPromptMessage(m_serverFTP.Errormessage);
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 递归获得子节点的信息
        /// </summary>
        /// <param name="node">父级节点</param>
        /// <param name="listFileID">节点信息列表</param>
        void GetChildNodeInfo(TreeNode node, ref List<int> listFileID)
        {
            string strNodeTag = node.Tag.ToString();

            if (strNodeTag.Contains("File"))
            {
                listFileID.Add(Convert.ToInt32(strNodeTag.Substring(4)));
                return;
            }
            else
            {
                foreach (TreeNode childNode in node.Nodes)
                {
                    strNodeTag = childNode.Tag.ToString();

                    if (strNodeTag.Contains("File"))
                    {
                        listFileID.Add(Convert.ToInt32(strNodeTag.Substring(4)));
                    }
                    else
                    {
                        GetChildNodeInfo(childNode, ref listFileID);
                    }
                }
            }
        }

        private void 文件体系结构_Load(object sender, EventArgs e)
        {
            GlobalObject.GeneralFunction.LoadTreeViewDt(treeView1, m_serverBasicInfo.GetTreeInfo(CE_FileType.体系文件), 
                "Name", "ID", "ParentID", "ParentID = '0'");
            treeView1.Nodes[0].Expand();
            AuthorityControl(m_authFlag);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            List<int> listFileID = new List<int>();

            GetChildNodeInfo(treeView1.SelectedNode, ref listFileID);

            dataGridView1.DataSource = m_serverBasicInfo.GetFilesInfo(listFileID);
            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            txtDepartment.Text = dataGridView1.CurrentRow.Cells["归口部门"].Value.ToString();
            txtDepartment.Tag = dataGridView1.CurrentRow.Cells["部门编号"].Value.ToString();
            txtSort.Tag = Convert.ToInt32(dataGridView1.CurrentRow.Cells["类别ID"].Value);
            txtFileName.Text = dataGridView1.CurrentRow.Cells["文件名称"].Value.ToString();
            txtFileNo.Text = dataGridView1.CurrentRow.Cells["文件编号"].Value.ToString();
            txtSort.Text = dataGridView1.CurrentRow.Cells["文件类别"].Value.ToString();
            txtVersion.Text = dataGridView1.CurrentRow.Cells["版本号"].Value.ToString();
            txtFileNo.Tag = dataGridView1.CurrentRow.Cells["文件ID"].Value;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();

            GlobalObject.GeneralFunction.LoadTreeViewDt(treeView1, m_serverBasicInfo.GetTreeInfo(CE_FileType.体系文件), 
                "Name", "ID", "ParentID", "ParentID = '0'");
            treeView1.Nodes[0].Expand();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            string path = dataGridView1.CurrentRow.Cells["路径"].Value.ToString();
            string tempPath = path.Substring(path.LastIndexOf('/') + 1, path.Length - path.LastIndexOf('/') - 1);

            FileOperationService.File_Look(new Guid(tempPath),
                    GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
        }

        private void btnDownLoad_Click(object sender, EventArgs e)
        {
            string path = dataGridView1.CurrentRow.Cells["路径"].Value.ToString();

            saveFileDialog1.Filter = "All files (*.*)|*.*";
            saveFileDialog1.FileName = txtFileName.Text;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                m_serverFTP.Download(path, saveFileDialog1.FileName);

                if (GetError())
                {
                    MessageDialog.ShowPromptMessage("下载成功");
                }
            }
        }

        private void btnUpLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (MessageDialog.ShowEnquiryMessage("您是否要上传【文件编号】：" + txtFileNo.Text
                        + " 【文件名称】：" + txtFileName.Text + "？") == DialogResult.No)
                    {
                        return;
                    }

                    Guid guid = new Guid();
                    string fileType = openFileDialog1.FileName.Substring(openFileDialog1.FileName.LastIndexOf("."));
                    string strFtpServerPath = "/" + ServerTime.Time.Year.ToString() + "/" + ServerTime.Time.Month.ToString() + "/";

                    if (!GlobalObject.FileTypeRecognition.IsWordDocument(openFileDialog1.FileName))
                    {
                        throw new Exception("此文件非正常WORD文件，可能由于文件格式无法识别或者文件加密造成无法上传");
                    }

                    guid = Guid.NewGuid();
                    m_serverFileBasic.AddFile(guid, strFtpServerPath + guid.ToString(), fileType);

                    CursorControl.SetWaitCursor(this);
                    m_serverFileBasic.FileUpLoad(openFileDialog1.FileName, strFtpServerPath, guid.ToString(), fileType);
                    this.Cursor = System.Windows.Forms.Cursors.Arrow;

                    if (GetError())
                    {
                        FM_FileList fileInfo = new FM_FileList();

                        fileInfo.DeleteFlag = false;
                        fileInfo.Department = txtDepartment.Tag.ToString();
                        fileInfo.FileName = txtFileName.Text;
                        fileInfo.FileNo = txtFileNo.Text;
                        fileInfo.FileUnique = guid;
                        fileInfo.SortID = Convert.ToInt32(txtSort.Tag);
                        fileInfo.Version = txtVersion.Text;

                        if (m_serverFileBasic.AddFileList(fileInfo, out m_strErr))
                        {
                            treeView1_AfterSelect(null, null);
                            MessageDialog.ShowPromptMessage("上传成功");
                        }
                        else
                        {
                            throw new Exception(m_strErr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
                this.Cursor = System.Windows.Forms.Cursors.Arrow;
            }
        }

        private void txtSort_OnCompleteSearch()
        {
            txtSort.Tag = Convert.ToInt32(txtSort.DataResult["文件类别ID"]);
        }

        private void txtSort_Enter(object sender, EventArgs e)
        {
            txtSort.StrEndSql = " and 文件类型ID = " + (int)CE_FileType.体系文件 + " and 文件类别ID not in (select ParentID from FM_FileSort)";
        }

        private void txtDepartment_OnCompleteSearch()
        {
            txtDepartment.Tag = txtDepartment.DataResult["部门编码"].ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null)
                {
                    throw new Exception("请在【文件列表】中，选中需要删除的文件");
                }

                if (MessageDialog.ShowEnquiryMessage("您是否要删除【文件编号】：" 
                    + txtFileNo.Text + " 【文件名称】：" + txtFileName.Text + "？") == DialogResult.No)
                {
                    return;
                }

                m_serverBasicInfo.DeleteFile(Convert.ToInt32(dataGridView1.CurrentRow.Cells["文件ID"].Value));
                treeView1_AfterSelect(null, null);
                throw new Exception("删除成功");
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }
    }
}
