using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.GISClient;



namespace gis_test1
{
    public partial class OnlineSelfSet : Form
    {
        private IActiveView pActiveView = null;
        private IMap map =null;
        public OnlineSelfSet(IHookHelper hookHelper)
        {
            InitializeComponent();
            pActiveView = hookHelper.ActiveView;
            map = hookHelper.FocusMap;
        }

        private void button1_Click(object sender, EventArgs e)
        {   
            string url = textBox1.Text;
            IPropertySet pPropertyset = new PropertySetClass();
            pPropertyset.SetProperty("url", url);
            IWMTSConnectionFactory pWMTSConnectionfactory = new WMTSConnectionFactory();
            IWMTSConnection pWMTSConnection = pWMTSConnectionfactory.Open(pPropertyset, 0, null);
            IWMTSLayer pWMTSLayer = new WMTSLayer();
            IName pName = pWMTSConnection.FullName;
            pWMTSLayer.Connect(pName);
            //return pWMTSLayer as ILayer;
            map.AddLayer(pWMTSLayer as ILayer);
            pActiveView.Refresh();
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
