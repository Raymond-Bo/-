using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System;
using System.Drawing;
using System.Runtime.InteropServices;
//using ESRI.ArcGIS.ADF.BaseClasses;
//using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace gis_test1
{
    public partial class Form2 : Form
    {
        private double pWidth, pHeight;
        private IActiveView pActiveView = null;
        public Form2(IHookHelper hookHelper)
        {
            InitializeComponent();
            pActiveView = hookHelper.ActiveView;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "(*.tif)|*.tif|(*.jpeg)|*.jpeg|(*.pdf)|*.pdf|(*.bmp)|*.bmp";

            //sfd.FilterIndex = 1;
            //sfd.RestoreDirectory = true;
            //if (sfd.ShowDialog() == DialogResult.OK)
            //{
            //    MessageBox.Show(sfd.FileName.ToString());
            //}
            //else
            //{
            //    MessageBox.Show("取消保存");
            //    return;
            //}

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.saveMapFileDialog.ShowDialog();
            //this.txtFileName.Text = saveMapFileDialog.FileName;

            //SaveFileDialog saveMapFileDialog = new SaveFileDialog();
            this.saveMapFileDialog.Filter = "JPEG(*.jpg)|*.jpg|BMP(*.BMP)|*.bmp|PNG(*.png)|*.png|TIFF(*.tif)|*.tif|GIF(*.gif)|*.gif|SVG(*.svg)|*.svg|AI(*.ai)|*.ai|EMF(*.emf)|*.emf|PDF(*.pdf)|*.pdf|EPS(*.eps)|*.eps";
            this.saveMapFileDialog.Title = "导出地图";
            this.saveMapFileDialog.RestoreDirectory = true;  //对话框在关闭前是否还原到当前目录下
            this.saveMapFileDialog.ShowDialog();
            this.txtFileName.Text = saveMapFileDialog.FileName;

        }


        private void ExportTool()
        {
            IExport pExport = null;
            //           IExportJPEG pExportFormat;  
            IWorldFileSettings pWorldFile = null;
            IExportImage pExportType;
            IEnvelope pDriverBounds = null;
            //         int lScreenResolution ;  
            tagRECT userRECT = new tagRECT();
            IEnvelope pEnv = new EnvelopeClass();
            //          double dWidth;  
            //          double dHeight;  
            int lResolution;
            lResolution = Convert.ToInt32(this.txtResolution.Value);
            switch (this.saveMapFileDialog.Filter.ToString().Trim().Substring(0, 3))
            {
                case "jpg":
                    pExport = new ExportJPEGClass();
                    break;
                case "bmp":
                    pExport = new ExportBMPClass();
                    break;
                case "gif":
                    pExport = new ExportGIFClass();
                    break;
                case "tif":
                    pExport = new ExportTIFFClass();
                    break;
                case "png":
                    pExport = new ExportPNGClass();
                    break;
                case "emf":
                    pExport = new ExportEMFClass();
                    break;
                case "pdf":
                    pExport = new ExportPDFClass();
                    break;
                case ".ai":
                    pExport = new ExportAIClass();
                    break;
                case "svg":
                    pExport = new ExportSVGClass();
                    break;
                default:
                    pExport = new ExportJPEGClass();
                    break;
            }

            if (this.txtFileName.Text.ToString().Trim() != "")
            {
                if (System.IO.File.Exists(this.txtFileName.Text.ToString()) == true)
                {
                    MessageBox.Show("该文件已经存在，请重新命名！");
                    this.txtFileName.Text = "";
                    this.txtFileName.Focus();
                }
                else
                {
                    pExport.ExportFileName = this.txtFileName.Text;
                    pExport.Resolution = lResolution;
                    pExportType = pExport as IExportImage;
                    pExportType.ImageType = esriExportImageType.esriExportImageTypeTrueColor;
                    pEnv = pActiveView.Extent;
                    pWorldFile = (IWorldFileSettings)pExport;
                    pWorldFile.MapExtent = pEnv;
                    pWorldFile.OutputWorldFile = false;
                    userRECT.top = 0;
                    userRECT.left = 0;
                    userRECT.right = Convert.ToInt32(pWidth);
                    userRECT.bottom = Convert.ToInt32(pHeight);
                    pDriverBounds = new EnvelopeClass();
                    pDriverBounds.PutCoords(userRECT.top, userRECT.bottom, userRECT.right, userRECT.top);
                    pExport.PixelBounds = pDriverBounds;

                    ITrackCancel pTrackCancel = new TrackCancelClass();
                    pActiveView.Output(pExport.StartExporting(), lResolution, ref userRECT, pActiveView.Extent, pTrackCancel);
                    pExport.FinishExporting();
                    MessageBox.Show("图片导出成功!", "保存", MessageBoxButtons.OK);
                    this.Close();

                }

            }
            else
            {
                MessageBox.Show("请保存文件!");
            }

        }

        public IActiveView ResActiveView
        {
            get
            {
                return pActiveView;
            }
            set
            {
                pActiveView = value;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            comboBox1_SelectedIndexChanged(null, null);
            if (this.radioButton2.Checked == true)
            {
                this.txtWidth.Text = pWidth.ToString(".00");
                this.txtHeight.Text = pHeight.ToString(".00");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbPageSize.Text)
            {
                case "自定义":
                    txtWidth.Focus();
                    break;
                case "A4":
                    showWH(21, 29.7);
                    break;
                case "A3":
                    showWH(29.7, 42);
                    break;
                case "A2":
                    showWH(42, 59.4);
                    break;
                case "A1":
                    showWH(59.4, 84.1);
                    break;
                case "A0":
                    showWH(84.1, 118.9);
                    break;
            }
        }

        private void showWH(double pW, double pH)
        {
            pWidth = Convert.ToDouble((pW / 2.54) * Convert.ToDouble(txtResolution.Value));
            pHeight = Convert.ToDouble((pH / 2.54) * Convert.ToDouble(txtResolution.Value));
            //if (this.radioButton1.Checked == true)
            //{
            //    this.txtWidth.Text = Convert.ToDouble(pW / 2.54).ToString(".00");
            //    this.txtHeight.Text = Convert.ToDouble(pH / 2.54).ToString(".00");
            //}
            if (this.radioButton1.Checked == true)
            {
                this.txtWidth.Text = pW.ToString(".00");
                this.txtHeight.Text = pH.ToString(".00");
            }
            else
            {
                this.txtWidth.Text = pW.ToString(".00");
                this.txtHeight.Text = pH.ToString(".00");
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //if (this.radioButton1.Checked == true)
            //{
            //    pWidth = Convert.ToDouble(txtWidth.Text) * Convert.ToDouble(txtResolution.Value);

            //}
            if (this.radioButton1.Checked == true)
            {
                pWidth = Convert.ToDouble(Convert.ToDouble(txtWidth.Text) / 2.54) * Convert.ToDouble(txtResolution.Value);

            }
            else
            {
                pWidth = Convert.ToDouble(txtWidth.Text);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //if (this.radioButton1.Checked == true)
            //{
            //    pHeight = Convert.ToDouble(txtHeight.Text) * Convert.ToDouble(txtResolution.Value);

            //}
            if (this.radioButton1.Checked == true)
            {
                pHeight = Convert.ToDouble(Convert.ToDouble(txtHeight.Text) / 2.54) * Convert.ToDouble(txtResolution.Value);

            }
            else
            {
                pHeight = Convert.ToDouble(txtHeight.Text);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.txtWidth.Text = pWidth.ToString(".00");
            this.txtHeight.Text = pHeight.ToString(".00");
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.txtWidth.Text = Convert.ToDouble(2.54 * pWidth / Convert.ToDouble(txtResolution.Value)).ToString(".00");
            this.txtHeight.Text = Convert.ToDouble(2.54 * pHeight / Convert.ToDouble(txtResolution.Value)).ToString(".00");
        }


        /// <summary>  
        /// 是否大于０的数字  
        /// </summary>  
        /// <param name="v"></param>  
        /// <returns></returns>  
        private bool IsNumbericA(string v)
        {
            return ((this.IsIntegerA(v)) || (this.IsFloatA(v)));
        }
        /// <summary>  
        /// 是否正浮点数  
        /// </summary>  
        /// <param name="v"></param>  
        /// <returns></returns>  
        private bool IsFloatA(string v)
        {
            string pattern = @"^[1-9]\d*\.\d*|0\.\d*[1-9]\d*$";
            Regex reg = new Regex(pattern);
            return reg.IsMatch(v);
        }
        /// <summary>  
        /// 是否正整数  


        /// </summary>  
        /// <param name="v"></param>  
        /// <returns></returns>  
        private bool IsIntegerA(string v)
        {
            string pattern = @"^[0-9]*[1-9][0-9]*$";
            Regex reg = new Regex(pattern);
            return reg.IsMatch(v);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (!IsNumbericA(txtWidth.Text))
            {
                MessageBox.Show("请输入数字！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtWidth.Focus();
                return;
            }
            if (!IsNumbericA(txtHeight.Text))
            {
                MessageBox.Show("请输入数字！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtHeight.Focus();
                return;
            }
            if (txtResolution.Value > 1)
            {
                ExportTool();
            }
            this.Cursor = Cursors.Default;
        }

        private void frmResource_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }
    }
}
