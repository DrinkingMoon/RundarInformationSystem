using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GlobalObject;
using UniversalControlLibrary;
using ServerModule;

namespace Expression
{
    public partial class 量检具关联文件 : Form
    {
        IGaugeManage _Service_Gauge = ServerModuleFactory.GetServerModule<IGaugeManage>();

        string _GaugeCode = null;

        public 量检具关联文件(string gaugeCode)
        {
            InitializeComponent();
            _GaugeCode = gaugeCode;
            DataGridViewShow();
        }

        void DataGridViewShow()
        {
            customDataGridView1.DataSource = _Service_Gauge.GetTable_FilesInfo(_GaugeCode);
        }

        private void btnUpLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (GeneralFunction.IsNullOrEmpty(txtFileName.Text))
                {
                    throw new Exception("请填写【文件名】");
                }

                if (GeneralFunction.IsNullOrEmpty(cmbFileType.Text))
                {
                    throw new Exception("请选择【文件类型】");
                }

                if (dtpFileDate.Value.Date > ServerTime.Time.Date)
                {
                    throw new Exception("【文件日期】不能大于当前时间");
                }

                string strFilePath = "";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    foreach (string filePath in openFileDialog1.FileNames)
                    {
                        Guid guid = Guid.NewGuid();
                        FileOperationService.File_UpLoad(guid, filePath,
                                GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)CE_SwitchName.文件传输方式]));
                        strFilePath += guid.ToString() + ",";
                    }

                    btnUpLoad.Tag = strFilePath.Substring(0, strFilePath.Length - 1);

                    Bus_Gauge_Files fileInfo = new Bus_Gauge_Files();

                    fileInfo.F_Id = Guid.NewGuid().ToString();
                    fileInfo.FileDate = dtpFileDate.Value;
                    fileInfo.FileName = txtFileName.Text;
                    fileInfo.FilePath = btnUpLoad.Tag.ToString();
                    fileInfo.FileType = cmbFileType.Text;
                    fileInfo.GaugeCode = _GaugeCode;

                    _Service_Gauge.UpLoadFileInfo(fileInfo);
                    throw new Exception("上传成功");
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
            finally
            {
                DataGridViewShow();
            }
        }

        private void btnDownLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (customDataGridView1.CurrentRow == null)
                {
                    return;
                }

                string filePath = customDataGridView1.CurrentRow.Cells["FilePath"].Value.ToString();

                if (GeneralFunction.IsNullOrEmpty(filePath))
                {
                    throw new Exception("无附件下载");
                }

                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    string[] tempArray = filePath.Split(',');

                    for (int i = 0; i < tempArray.Length; i++)
                    {
                        FileOperationService.File_DownLoad(new Guid(tempArray[i]),
                            folderBrowserDialog1.SelectedPath + "\\" + txtFileName.Text + "_" + i.ToString(),
                            GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (customDataGridView1.CurrentRow == null)
                {
                    return;
                }

                string keyValue = customDataGridView1.CurrentRow.Cells["F_Id"].Value.ToString();
                string filePath = customDataGridView1.CurrentRow.Cells["FilePath"].Value.ToString();

                _Service_Gauge.DeleteFileInfo(keyValue);

                if (!GeneralFunction.IsNullOrEmpty(filePath))
                {
                    foreach (string fileItem in filePath.Split(','))
                    {
                        UniversalControlLibrary.FileOperationService.File_Delete(new Guid(fileItem),
                            GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
                    }
                }

                throw new Exception("删除成功");
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
            finally
            {
                DataGridViewShow();
            }
        }

        private void customDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (customDataGridView1.CurrentRow == null)
            {
                return;
            }

            txtFileName.Tag = customDataGridView1.CurrentRow.Cells["F_Id"].Value.ToString();
            txtFileName.Text = customDataGridView1.CurrentRow.Cells["文件名"].Value.ToString();
            cmbFileType.Text = customDataGridView1.CurrentRow.Cells["文件类型"].Value.ToString();
            dtpFileDate.Value = Convert.ToDateTime( customDataGridView1.CurrentRow.Cells["文件日期"].Value);
            btnUpLoad.Tag = customDataGridView1.CurrentRow.Cells["FilePath"].Value.ToString();
        }

        private void customDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (customDataGridView1.CurrentRow == null)
            {
                return;
            }

            string[] tempArray = customDataGridView1.CurrentRow.Cells["FilePath"].Value.ToString().Split(',');

            for (int i = 0; i < tempArray.Length; i++)
            {
                FileOperationService.File_Look(new Guid(tempArray[i]),
                        GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)CE_SwitchName.文件传输方式]));
            }
        }
    }
}
