using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;






namespace gis_test1
{
    //static 
    class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.Engine);


            //#region 初始化许可
            //ESRI.ArcGIS.esriSystem.IAoInitialize m_AoInitialize = new ESRI.ArcGIS.esriSystem.AoInitializeClass();
            //ESRI.ArcGIS.esriSystem.esriLicenseStatus licenseStatus = ESRI.ArcGIS.esriSystem.esriLicenseStatus.esriLicenseUnavailable;
            //licenseStatus = m_AoInitialize.Initialize(ESRI.ArcGIS.esriSystem.esriLicenseProductCode.esriLicenseProductCodeAdvanced);
            //if (licenseStatus == ESRI.ArcGIS.esriSystem.esriLicenseStatus.esriLicenseNotInitialized)
            //{
            //    MessageBox.Show("没有esriLicenseProductCodeArcInfo许可！");
            //    Application.Exit();
            //}
            //#endregion

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }



    }
}
