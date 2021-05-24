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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.GISClient;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geoprocessing;

using ESRI.ArcGIS.Geodatabase;
using System.Text.RegularExpressions;
using ESRI.ArcGIS.ConversionTools;

using OSGeo.OGR;
using OSGeo.GDAL;
using ESRI.ArcGIS.GlobeCore;
using ESRI.ArcGIS.DataSourcesFile;
using System.Data.OleDb;

namespace gis_test1
{
    public partial class Form1 : Form
    {
        public static Form1 frm1 = null;
        public Form1()
        {
            InitializeComponent();
            axTOCControl1.SetBuddyControl(axMapControl1);
            frm1 = this;
        }
        public static string usr = null;
        public static int pipe = -2;//管线编号
        public static int segment = -2;//分段
        public static int manage = 0;//管理权限
        DataTable dt;
        private void button1_Click_1(object sender, EventArgs e)
        {
            IPropertySet pPropertyset = new PropertySetClass();
            //美国地图 https://services.arcgisonline.com/arcgis/rest/services/Demographics/USA_Population_Density/MapServer/WMTS/
            //中国地图 http://cache1.arcgisonline.cn/arcgis/rest/services/ChinaOnlineCommunity/MapServer
            //http://t0.tianditu.gov.cn/img_w/wmts?request=GetCapabilities&service=wmts&tk=d5b82c3720349ff7cb0e6bbfeb437324
            pPropertyset.SetProperty("url", "http://rt{s}.map.gtimg.com/realtimerender?z={z}&x={x}&y={y}&type=vector&style=0");//https://restapi.amap.com/v3/staticmap?location=116.481485,39.990464&zoom=10&key=37245e538a959ede062cbaacac69c00c//瓦片http://t0.tianditu.gov.cn/img_w/wmts?SERVICE=WMTS&REQUEST=GetTile&VERSION=1.0.0&LAYER=img&STYLE=default&TILEMATRIXSET=w&FORMAT=tiles&TILEMATRIX={z}&TILEROW={x}&TILECOL={y}&tk=d5b82c3720349ff7cb0e6bbfeb437324//http://t0.tianditu.gov.cn/vec_c/wmts?tk=d5b82c3720349ff7cb0e6bbfeb437324
            IWMTSConnectionFactory pWMTSConnectionfactory = new WMTSConnectionFactory();
            IWMTSConnection pWMTSConnection = pWMTSConnectionfactory.Open(pPropertyset, 0, null);
            IWMTSLayer pWMTSLayer = new WMTSLayer();
            IName pName = pWMTSConnection.FullName;
            pWMTSLayer.Connect(pName);

            axMapControl1.AddLayer(pWMTSLayer as ILayer);
            axMapControl1.Refresh();
          
        }


        private void 城市地图ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //http://cache1.arcgisonline.cn/arcgis/rest/services/ChinaOnlineCommunity/MapServer
            IPropertySet pPropertyset = new PropertySetClass();
            pPropertyset.SetProperty("url", "http://cache1.arcgisonline.cn/arcgis/rest/services/ChinaOnlineCommunity/MapServer/WMTS/");
            IWMTSConnectionFactory pWMTSConnectionfactory = new WMTSConnectionFactory();
            IWMTSConnection pWMTSConnection = pWMTSConnectionfactory.Open(pPropertyset, 0, null);
            IWMTSLayer pWMTSLayer = new WMTSLayer();
            IName pName = pWMTSConnection.FullName;
            pWMTSLayer.Connect(pName);


            axMapControl1.AddLayer(pWMTSLayer as ILayer);
            axMapControl1.Refresh();

        }

        private void 街道地图ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            IPropertySet pPropertyset = new PropertySetClass();
            pPropertyset.SetProperty("url", "http://cache1.arcgisonline.cn/arcgis/rest/services/ChinaOnlineStreetGray/MapServer/WMTS/");
            IWMTSConnectionFactory pWMTSConnectionfactory = new WMTSConnectionFactory();
            IWMTSConnection pWMTSConnection = pWMTSConnectionfactory.Open(pPropertyset, 0, null);
            IWMTSLayer pWMTSLayer = new WMTSLayer();
            IName pName = pWMTSConnection.FullName;
            pWMTSLayer.Connect(pName);

            axMapControl1.AddLayer(pWMTSLayer as ILayer);
            axMapControl1.Refresh();
        }

        private void 遥感地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {


            //axMapControl1.Refresh();
            IPropertySet pPropertyset = new PropertySetClass();
            pPropertyset.SetProperty("url", "https://services.arcgisonline.com/arcgis/rest/services/World_Imagery/MapServer/WMTS/");
            IWMTSConnectionFactory pWMTSConnectionfactory = new WMTSConnectionFactory();
            IWMTSConnection pWMTSConnection = pWMTSConnectionfactory.Open(pPropertyset, 0, null);
            IWMTSLayer pWMTSLayer = new WMTSLayer();
            IName pName = pWMTSConnection.FullName;
            pWMTSLayer.Connect(pName);

            axMapControl1.AddLayer(pWMTSLayer as ILayer,0);
            axMapControl1.Refresh();
            //axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll,null,null);        
        }


        /// <summary>
        /// 右键操作lsx
        /// </summary>
        ITOCControlEvents_OnMouseDownEvent ted;
        public ITOCControlEvents_OnMouseDownEvent Ted
        {
            get { return ted; }
            set { ted = value; }
        }
        ITOCControlEvents_OnMouseUpEvent teu;
        public ITOCControlEvents_OnMouseUpEvent Teu
        {
            get { return teu; }
            set { teu = value; }
        }
        ILayer pMovelayer;
        int toIndex;
        public void AdjLayMd()
        {
            esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
            if (ted.button == 1)
            {
                IBasicMap map = null;
                ILayer layer = null;
                object other = null;
                object index = null;
                axTOCControl1.HitTest(ted.x, ted.y, ref item, ref map, ref layer, ref other, ref index);
                if (item == esriTOCControlItem.esriTOCControlItemLayer)
                {
                    if (layer is IAnnotationSublayer)
                        return;
                    else
                    {
                        pMovelayer = layer;
                    }
                }
            }
            //鼠标右键按下
            else if (ted.button == 2)
            {
                if (axMapControl1.LayerCount > 0) //主视图中有地理数据
                {
                    return;
                }
            }
        }
        public void AdjLayMu()
        {
            if (teu.button == 1)
            {
                esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
                IBasicMap map = null;
                ILayer layer = null;
                object other = null;
                object index = null;
                axTOCControl1.HitTest(teu.x, teu.y, ref item, ref map, ref layer, ref other, ref index);
                IMap pMap = axMapControl1.ActiveView.FocusMap;
                if (item == esriTOCControlItem.esriTOCControlItemLayer || layer != null)
                {
                    if (pMovelayer != null)//!=null??
                    {
                        ILayer pTempLayer;
                        for (int i = 0; i < pMap.LayerCount; i++)
                        {
                            pTempLayer = pMap.get_Layer(i);
                            if (pTempLayer == layer)
                                toIndex = i;//获取鼠标点击位置的图层索引号
                        }
                        pMap.MoveLayer(pMovelayer, toIndex);
                        axMapControl1.ActiveView.Refresh();
                        axTOCControl1.Update();
                    }
                }
            }
        }
        ILayer SelectedLayer_TOC;
        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            ted = e;
            AdjLayMd();
            ESRI.ArcGIS.Controls.esriTOCControlItem Item = ESRI.ArcGIS.Controls.esriTOCControlItem.esriTOCControlItemNone;
            if (e.button == 1)
            {
                IBasicMap map = null;
                ILayer layer = null;
                object other = null;
                object index = null;

                axTOCControl1.HitTest(e.x, e.y, ref Item, ref map, ref layer, ref other, ref index);
                if (Item == esriTOCControlItem.esriTOCControlItemLayer)
                {
                    if (layer is IAnnotationSublayer)
                    {
                        return;
                    }
                    else
                    {
                        SelectedLayer_TOC = layer;
                    }
                }

                //if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A) ;
            }
            if (e.button == 2)
            {

                IBasicMap pBasicMap = null;
                ILayer pLayer = null;
                object other = null;
                object index = null;
                axTOCControl1.HitTest(e.x, e.y, ref Item, ref pBasicMap, ref pLayer, ref other, ref index);          //实现赋值
                if (Item == esriTOCControlItem.esriTOCControlItemLayer)//点击的是图层的话，就显示右键菜单
                {
                    SelectedLayer_TOC = pLayer;
                    contextMenuStrip1.Show(axTOCControl1, new System.Drawing.Point(e.x, e.y));
                    //显示右键菜单，并定义其相对控件的位置，正好在鼠标出显示
                }
            }
        }

        private void 删除图层ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (SelectedLayer_TOC != null)
            {
                axMapControl1.Map.DeleteLayer(SelectedLayer_TOC);
                SelectedLayer_TOC = null;
                axMapControl1.Refresh();
            }
        }
        private void 删除所有图层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (SelectedLayer_TOC != null)
            //{
            //    axMapControl1.Map.ClearLayers();
            //    SelectedLayer_TOC = null;
            //    axMapControl1.Refresh();
            //}
        }
        private void 移动到顶部ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedLayer_TOC != null)
            {
                axMapControl1.Map.MoveLayer(SelectedLayer_TOC, 0);
                SelectedLayer_TOC = null;
            }
        }

        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Form frm = new Form2();
            //frm.Show();


            IHookHelper map_hookHelper = new HookHelperClass();
            //参数赋值  
            map_hookHelper.Hook = axMapControl1.Object;
            Form frmExportDlg = new Form2(map_hookHelper);
            frmExportDlg.Show();


        }

        

        private void shp文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog OpenFdlg = new OpenFileDialog();
            OpenFdlg.Title = "选择shp文件";
            OpenFdlg.Filter = "Shape格式文件（*.shp）|*.shp";
            OpenFdlg.Multiselect = true; // 允许同时导入多个文件
            if (OpenFdlg.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in OpenFdlg.FileNames)
                {
                    if (file == null) continue;
                    //将Shp文件的路径分割为路径和不带格式后缀的文件名
                    string pathName = System.IO.Path.GetDirectoryName(file);
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(file);
                    axMapControl1.AddShapeFile(pathName, fileName);
                    IFeatureLayer pFeatureLayer = axMapControl1.Map.get_Layer(0) as IFeatureLayer;
                    IFeatureClass fClass = pFeatureLayer.FeatureClass;
                    if (fClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                    {
                        pFeatureLayer.Name = fileName + "(Line)";
                    }
                    else if (fClass.ShapeType == esriGeometryType.esriGeometryPoint)
                    {
                        pFeatureLayer.Name = fileName + "(Point)";
                    }
                    else if (fClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        pFeatureLayer.Name = fileName + "(Polygon)";
                    }
                }
            }
            if (axMapControl1.LayerCount == 1)
            {
                axMapControl1.Extent = axMapControl1.Extent;

            }
            axMapControl1.Refresh();
            axTOCControl1.Update();
        }

        private void 栅格数据文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.axMapControl1.ActiveView.Clear();//清空当前
            OpenFileDialog xjRasterOpenFileDialog = new OpenFileDialog();
            xjRasterOpenFileDialog.Title = "打开栅格数据";
            xjRasterOpenFileDialog.Filter = "栅格数据(*.tiff;*.tif;*.jpep;*.jpg;*.png;*.bmp)| *.tiff; *.tif; *.jpep; *.jpg; *.png; *.bmp";


            if (xjRasterOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                string xjRasterPath = xjRasterOpenFileDialog.FileName;
                string xjRasterFolder = System.IO.Path.GetDirectoryName(xjRasterPath);
                string xjRasterFileName = System.IO.Path.GetFileName(xjRasterPath);
                //工作空间（实例化）
                IWorkspaceFactory xjRasterWsF = new RasterWorkspaceFactory();
                IWorkspace xjRasterWsn = xjRasterWsF.OpenFromFile(xjRasterFolder, 0);
                IRasterWorkspace xjRasterWs = xjRasterWsn as IRasterWorkspace;//强制转换
                IRasterDataset xjRasterDS = xjRasterWs.OpenRasterDataset(xjRasterFileName);
                //影像金字塔的判断和创建（可以没有）
                IRasterPyramid xjRasterPyramid = xjRasterDS as IRasterPyramid;
                if (xjRasterPyramid != null)
                {
                    if (!(xjRasterPyramid.Present))
                    {
                        xjRasterPyramid.Create();
                    }
                }
                //新建栅格图层
                IRasterLayer xjRasterLayer = new RasterLayer();//引用Carto
                xjRasterLayer.CreateFromRaster(xjRasterDS.CreateDefaultRaster());
                //加载显示
                this.axMapControl1.AddLayer(xjRasterLayer, 0);
                this.axMapControl1.ActiveView.Refresh();
            }
        }

        private void kml文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ///
            /// 先转换为lyr再打开lyr，调用arctool转换时，需要先去arcmap看参数设置顺序
            ///
            OpenFileDialog OpenFdlg = new OpenFileDialog();
            OpenFdlg.Title = "选择kml文件";
            OpenFdlg.Filter = "kml格式文件（*.kml;*.kmz）|*.kml;*.kmz";
            OpenFdlg.ShowDialog();
            string strFileName = OpenFdlg.FileName;
            if (strFileName == string.Empty)
            {
                return;
            }

            GeoProcessor pGeoprocessor = new GeoProcessor();
            KMLToLayer pKMLToLayer = new ESRI.ArcGIS.ConversionTools.KMLToLayer();
            IVariantArray parameters = new VarArrayClass();
            parameters.Add(strFileName);//pKMLToLayer.in_kml_file = strFileName;
            parameters.Add(@"D:\");//pKMLToLayer.output_layer = strFileName;
            parameters.Add(@"tmp");//pKMLToLayer.output_folder = @"tmp\";
            //pKMLToLayer.output_data = @"E:\Data\测试二.lyr";
            pGeoprocessor.OverwriteOutput = true;
            ///lsx注解，Execute第一个参数时执行的类的名字，第二个是参数的集合需要注意参数的顺序
            IGeoProcessorResult resultGeo = pGeoprocessor.Execute("KMLToLayer", parameters, null);//(IGeoProcessorResult)

            if (resultGeo.Status == esriJobStatus.esriJobSucceeded)
            {
                String layerpath = @"D:\tmp.lyr";
                MapDocument mapDoc = new MapDocument();
                mapDoc.Open(layerpath);
                //获取第一个地图的第一个图层
                ILayer pLyr = mapDoc.get_Layer(0, 0);
                axMapControl1.AddLayer(pLyr);
                MessageBox.Show("执行成功！", "输出Layer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }             
            else
                MessageBox.Show("执行Error！", "输出Layer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

        private void geoJson文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharpMap.GdalConfiguration.ConfigureGdal();
            SharpMap.GdalConfiguration.ConfigureOgr();

            // 注册所有的驱动
            Ogr.RegisterAll();
            OSGeo.GDAL.Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "YES");
            // 为了使属性表字段支持中文，请添加下面这句
            OSGeo.GDAL.Gdal.SetConfigOption("SHAPE_ENCODING", "CP936");
            //string strVectorFile = @"d:\gdal.shp";
            //string strVectorFile = @"d:\gdalnew.shp";
            //string strVectorFile = @"d:\ogr.gdb";


            OpenFileDialog OpenFdlg = new OpenFileDialog();
            OpenFdlg.Title = "选择GeoJson文件";
            OpenFdlg.Filter = "geojson格式文件（*.geojson）|*.geojson";
            OpenFdlg.ShowDialog();
            string strFileName = OpenFdlg.FileName;
            if (strFileName == string.Empty)
            {
                return;
            }

            //打开数据
            DataSource ds = Ogr.Open(strFileName, 0);
            if (ds == null)
            {
                MessageBox.Show("打开文件失败！");
                return;
            }
            //MessageBox.Show("打开文件成功！");
            OSGeo.OGR.Driver dv = Ogr.GetDriverByName("ESRI Shapefile");
            if (dv == null)
            {
                MessageBox.Show("打开驱动失败！");
                return;
            }
            //MessageBox.Show("打开驱动成功！");
            string svpath = @"D://tmp.shp";
            dv.CopyDataSource(ds, svpath, null);
            MessageBox.Show("数据转换成功！");

            string pathName = System.IO.Path.GetDirectoryName(svpath);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(svpath);
            axMapControl1.AddShapeFile(pathName, fileName);
            axMapControl1.Extent = axMapControl1.FullExtent;

        }

        private void 连接服务器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IHookHelper map_hookHelper = new HookHelperClass();
            //参数赋值  
            map_hookHelper.Hook = axMapControl1.Object;
            Form frmExportDlg = new OnlineSelfSet(map_hookHelper);
            frmExportDlg.Show();
        }

        private void 文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 显示图层属性ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GeoMapAttribute newMapAttribute = new GeoMapAttribute(SelectedLayer_TOC);
            newMapAttribute.Text = "图层属性信息";       
            newMapAttribute.Show();
        }

        private void axTOCControl1_OnMouseUp(object sender, ITOCControlEvents_OnMouseUpEvent e)
        {
            if (e.button == 1)
            {
                esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
                IBasicMap map = null;
                ILayer layer = null;
                object other = null;
                object index = null;

                axTOCControl1.HitTest(e.x, e.y, ref item, ref map, ref layer, ref other, ref index);
                IMap pMap = this.axMapControl1.ActiveView.FocusMap;
                if (item == esriTOCControlItem.esriTOCControlItemLayer || layer != null )
                {
                    if (SelectedLayer_TOC != null)
                    {
                        ILayer pTempLayer;
                        for (int i = 0; i < pMap.LayerCount; i++)
                        {
                            pTempLayer = pMap.get_Layer(i);
                            if (pTempLayer == layer)
                            {
                                toIndex = i;
                            }
                        }
                        
                        pMap.MoveLayer(SelectedLayer_TOC, toIndex);
                        axMapControl1.ActiveView.Refresh();
                        axTOCControl1.Update();
                    }
                }
            }
        }


        private void axMapControl1_OnMouseMove_1(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            
            IProjectedCoordinateSystem pcs = this.axMapControl1.SpatialReference as IProjectedCoordinateSystem;
            WKSPoint pt = new WKSPoint(); //不能用IPoint pt = new PointClass();因为后面的方法只支持WKSPoint

            pt.X = e.mapX;
            pt.Y = e.mapY;

            if (pcs == null)
                return;
            pcs.Inverse(1, ref pt); //将平面坐标转换为地理坐标                 
           
            StatusLabel.Text = String.Format("东经：{0}°, 北纬：{1}°", pt.X.ToString(), pt.Y.ToString());

        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {

        }

        public void AddPointByBuffer(List<IPoint> lst, int layerIndex = 0)

        {
            //得到要添加地物的图层
            IFeatureLayer layer = axMapControl1.Map.get_Layer(layerIndex) as IFeatureLayer;
            //定义一个地物类,把要编辑的图层转化为定义的地物类
            IFeatureClass fc = layer.FeatureClass;
            //先定义一个编辑的工作空间,然后把转化为数据集,最后转化为编辑工作空间,
            IWorkspaceEdit w = (fc as IDataset).Workspace as IWorkspaceEdit;
            //开始事务操作
            w.StartEditing(true);
            //开始编辑
            w.StartEditOperation();
            IPoint p;
            IFeatureBuffer f;
            IFeatureCursor cur = fc.Insert(true);

            for (int i = 0; i < lst.Count; i++)
            {
                f = fc.CreateFeatureBuffer();
                //p = new ESRI.ArcGIS.Geometry.Point();               
                //p.PutCoords(lst[i].X,lst[i].Y);                
                f.Shape = lst[i];
                cur.InsertFeature(f);
            }
            //结束编辑
            w.StopEditOperation();
            //结束事务操作
            w.StopEditing(true);
        }
        //创建IPoint 对象
        private IPoint ConstructPoint(double x, double y)
        {
            IPoint pPoint = new ESRI.ArcGIS.Geometry.Point();
            pPoint.PutCoords(x, y);
            return pPoint;
        }
        private void 画点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            List<IPoint> lst = new List<IPoint>();

            int row = dt.Rows.Count;
            try
            {
                for (int i = 1; i < row; i++)
                {
                    IPoint point = ConstructPoint(Convert.ToDouble(dt.Rows[i][2]), Convert.ToDouble(dt.Rows[i][1]));
                    lst.Add(point);
                }                
            }
            catch ( Exception err)
            {
                //MessageBox.Show(e.ToString());
            }
            AddPointByBuffer(lst);
        }

        public void AddLineByWrite()

        {
            IFeatureLayer layer = axMapControl1.Map.get_Layer(0) as IFeatureLayer;
            IFeatureClass fc = layer.FeatureClass;
            IFeatureClassWrite fr = fc as IFeatureClassWrite;

            IWorkspaceEdit w = (fc as IDataset).Workspace as IWorkspaceEdit;
            IFeature f;
            //可选参数的设置

            object Missing = Type.Missing;
            IPoint p = new PointClass();
            w.StartEditing(true);

            w.StartEditOperation();
            IFeatureCursor cur = fc.Insert(true);
            for (int i = 0; i < 100; i++)
            {
                f = fc.CreateFeature();
                //定义一个多义线对象
                IPolyline PlyLine = new PolylineClass();
                //定义一个点的集合
                IPointCollection ptclo = PlyLine as IPointCollection;
                //定义一系列要添加到多义线上的点对象，并赋初始值
                for (int j = 0; j < 4; j++)
                {
                    p.PutCoords(i, j);
                    ptclo.AddPoint(p, ref Missing, ref Missing);
                }
                
                f.Shape = PlyLine;
                //fr.WriteFeature(f);
                cur.InsertFeature(f as IFeatureBuffer);
            }
                      
            w.StopEditOperation();
            w.StopEditing(true);
        }
        private void 画线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IGeometry polyline = axMapControl1.TrackLine();
            ILineElement pLineElement = new LineElementClass();
            IElement pElement = pLineElement as IElement;
            pElement.Geometry = polyline;
            IMap pMap = axMapControl1.Map;
            IGraphicsContainer pGraphicsContainer = pMap as IGraphicsContainer;
            pGraphicsContainer.AddElement((IElement)pLineElement, 0);
            axMapControl1.ActiveView.Refresh();          
            axTOCControl1.Update();

            AddLineByWrite();
        }

        public static void CreatShpFile(out string ErrorMsg, string shpFullFilePath, 
            ISpatialReference spatialReference, esriGeometryType pGeometryType)
        {
            ErrorMsg = "";
            try
            {
                //将Shp文件的路径分割为路径和不带格式后缀的文件名
                string shpFolder = System.IO.Path.GetDirectoryName(shpFullFilePath);
                string shpFileName = System.IO.Path.GetFileName(shpFullFilePath);
                IWorkspaceFactory pWorkspaceFac = new ShapefileWorkspaceFactoryClass();
                IWorkspace pWorkSpace = pWorkspaceFac.OpenFromFile(shpFolder, 0);
                IFeatureWorkspace pFeatureWorkSpace = pWorkSpace as IFeatureWorkspace;
                //如果文件已存在               
                if (System.IO.File.Exists(shpFullFilePath))
                {
                    if (MessageBox.Show("文件已存在，是否覆盖？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                    {
                        IFeatureClass pFCChecker = pFeatureWorkSpace.OpenFeatureClass(shpFileName);
                        if (pFCChecker != null)
                        {
                            IDataset pds = pFCChecker as IDataset;
                            pds.Delete();
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                IFeatureClassDescription fcDescription = new FeatureClassDescriptionClass();
                IObjectClassDescription pObjectDescription = (IObjectClassDescription)fcDescription;
                IFields fields = pObjectDescription.RequiredFields;
                int shapeFieldIndex = fields.FindField(fcDescription.ShapeFieldName);
                IField field = fields.get_Field(shapeFieldIndex);
                IGeometryDef geometryDef = field.GeometryDef;
                IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;
                //点
                geometryDefEdit.GeometryType_2 = pGeometryType; //geometry类型
                ISpatialReferenceFactory pSpatialRefFac = new SpatialReferenceEnvironmentClass(); //坐标系
                                                                                                  //IProjectedCoordinateSystem pcsSys = pSpatialRefFac.CreateProjectedCoordinateSystem(pcsType);//投影坐标系
                                                                                                  //geometryDefEdit.SpatialReference_2 = pcsSys;
                int pcsType = (int)esriSRGeoCSType.esriSRGeoCS_WGS1984;
                ISpatialReference spatialReference1 = pSpatialRefFac.CreateGeographicCoordinateSystem(pcsType);
                geometryDefEdit.SpatialReference_2 = spatialReference1;

                IFieldChecker fieldChecker = new FieldCheckerClass();
                IEnumFieldError enumFieldError = null;
                IFields validatedFields = null; //将传入字段 转成 validatedFields
                fieldChecker.ValidateWorkspace = pWorkSpace;
                fieldChecker.Validate(fields, out enumFieldError, out validatedFields);

                pFeatureWorkSpace.CreateFeatureClass(shpFileName, validatedFields, pObjectDescription.InstanceCLSID, pObjectDescription.ClassExtensionCLSID, esriFeatureType.esriFTSimple, fcDescription.ShapeFieldName, "");
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
            }
        }
        private void 新建shp文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFdlg = new OpenFileDialog();
            OpenFdlg.CheckFileExists = false; //设置不弹出警告
            //OpenFdlg.FileOk += openFileDialog1_FileOk;//关联事件方法
            OpenFdlg.Title = "保存shp文件";
            OpenFdlg.Filter = "Shape格式文件（*.shp）|*.shp";
            OpenFdlg.ShowDialog();
            string strFileName = OpenFdlg.FileName;
            if (strFileName == string.Empty)
            {
                return;
            }
            
            string str;
            CreatShpFile(out str, strFileName, null, esriGeometryType.esriGeometryPoint);

            //随即打开新建的图层在最顶层
            string pathName = System.IO.Path.GetDirectoryName(strFileName);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(strFileName);         
            axMapControl1.AddShapeFile(pathName, fileName);
            axMapControl1.Extent = axMapControl1.FullExtent;

        }

        public static void GenerateSHPFile(IGeometry pResultGeometry, string filename)
        {
            IWorkspaceFactory wsf = new ShapefileWorkspaceFactory();
            IFeatureWorkspace fwp;
            IFeatureLayer flay = new FeatureLayer();

            fwp = (IFeatureWorkspace)wsf.OpenFromFile(System.IO.Path.GetDirectoryName(filename), 0);
            IFeatureClass fc = fwp.OpenFeatureClass(System.IO.Path.GetFileName(filename));
            flay.FeatureClass = fc;
            flay.Name = flay.FeatureClass.AliasName;

            IDataset pDataSet = flay.FeatureClass as IDataset;
            IWorkspaceEdit m_WorkSpaceEdit = (IWorkspaceEdit)pDataSet.Workspace;
            if (!m_WorkSpaceEdit.IsBeingEdited())
            {
                m_WorkSpaceEdit.StartEditing(true);
                m_WorkSpaceEdit.EnableUndoRedo();
            }

            ITopologicalOperator pTop = pResultGeometry as ITopologicalOperator;
            pTop.Simplify();

            m_WorkSpaceEdit.StartEditOperation();
            ITable pTable = flay.FeatureClass as ITable;
            pTable.DeleteSearchedRows(null);

            IFeature pFeature = fc.CreateFeature();
            pFeature.Shape = ModifyGeomtryZMValue(fc, pResultGeometry);
            pFeature.Store();

            m_WorkSpaceEdit.StopEditOperation();
            m_WorkSpaceEdit.StopEditing(true);
        }

        private static IGeometry ModifyGeomtryZMValue(IObjectClass featureClass, IGeometry modifiedGeo)
        {
            try
            {
                IFeatureClass trgFtCls = featureClass as IFeatureClass;
                if (trgFtCls == null) return null;
                string shapeFieldName = trgFtCls.ShapeFieldName;
                IFields fields = trgFtCls.Fields;
                int geometryIndex = fields.FindField(shapeFieldName);
                IField field = fields.get_Field(geometryIndex);
                IGeometryDef pGeometryDef = field.GeometryDef;
                IPointCollection pPointCollection = modifiedGeo as IPointCollection;
                if (pGeometryDef.HasZ)
                {
                    IZAware pZAware = modifiedGeo as IZAware;
                    pZAware.ZAware = true;
                    IZ iz1 = modifiedGeo as IZ; //若报iz1为空的错误，则将设置Z值的这两句改成IPoint point = (IPoint)pGeo;  point.Z = 0;
                    iz1.SetConstantZ((modifiedGeo as IPoint).Z);//如果此处报错，说明该几何体的点本身都没有Z值，在此处可以自己手动设置Z值,比如0，也就算将此句改成iz1.SetConstantZ(0);
                }
                else
                {
                    IZAware pZAware = modifiedGeo as IZAware;
                    pZAware.ZAware = false;
                }
                if (pGeometryDef.HasM)
                {
                    IMAware pMAware = modifiedGeo as IMAware;
                    pMAware.MAware = true;
                }
                else
                {
                    IMAware pMAware = modifiedGeo as IMAware;
                    pMAware.MAware = false;
                }
                return modifiedGeo;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "ModifyGeomtryZMValue error");
                return modifiedGeo;
            }
        }


        private void axMapControl1_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
          
        }

        private void axMapControl2_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
        }

        private void axMapControl2_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {

        }

        private void axMapControl1_OnViewRefreshed(object sender, IMapControlEvents2_OnViewRefreshedEvent e)
        {

        }
     
        private void axMapControl1_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {

        }


        private void 导入地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void 显示照片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            Form frmExportDlg = new 显示图片();
            frmExportDlg.Show();
        }

        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        //打开excel 返回 DataGridView
        public void ReadExcel(string sExcelFile,ref DataGridView dgBom)
        {
            //try
            //{
                DataTable ExcelTable;
                DataSet ds = new DataSet();
                //Excel的连接
                string sExt = System.IO.Path.GetExtension(sExcelFile);
                string sConn = null;
                if (sExt == ".xlsx")
                {
                    sConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + sExcelFile + ";" + "Extended Properties='Excel 12.0;HDR=YES'";
                }
                else if (sExt == ".xls")
                {
                    sConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + sExcelFile + ";" + "Extended Properties=Excel 8.0";
                }

                OleDbConnection objConn = new OleDbConnection(sConn);
                objConn.Open();
                DataTable schemaTable = objConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
                string tableName = schemaTable.Rows[0][2].ToString().Trim();//获取Excel的表名，默认值是sheet1
                Console.WriteLine(tableName);
                // string strSql = "SELECT * FROM [" + tableName + "]";
                string strSql = "SELECT * FROM [Sheet1$]";
                OleDbCommand objCmd = new OleDbCommand(strSql, objConn);
                OleDbDataAdapter myData = new OleDbDataAdapter(strSql, objConn);
                myData.Fill(ds, tableName);//填充数据

                dgBom.DataSource = ds;
                //dgBom.DataBind();
                dgBom.Show();
                Console.WriteLine("dgBom 展示完毕！");
                objConn.Close();


                ExcelTable = ds.Tables[tableName];
                int iColums = ExcelTable.Columns.Count;//列数
                int iRows = ExcelTable.Rows.Count;//行数

                //定义二维数组存储Excel表中读取的数据
                string[,] storedata = new string[iRows, iColums];

                for (int i = 0; i < ExcelTable.Rows.Count; i++)
                    for (int j = 0; j < ExcelTable.Columns.Count; j++)
                    {
                        //将Excel表中的数据存储到数组
                        storedata[i, j] = ExcelTable.Rows[i][j].ToString();

                    }
                int excelBom = 0;//记录表中有用信息的行数，有用信息是指除去表的标题和表的栏目，本例中表的用用信息是从第三行开始
                                 //确定有用的行数
                for (int k = 2; k < ExcelTable.Rows.Count; k++)
                    if (storedata[k, 1] != "")
                        excelBom++;
                if (excelBom == 0)
                {
                    //Response.Write("<script language=javascript>alert('您导入的表格不合格式！')</script>");
                    MessageBox.Show("表中有用信息的行数为0 ,导入的表格不合格式！");
                }
                else
                {
                    //LoadDataToDataBase(storedata，excelBom)//该函数主要负责将storedata中有用的数据写入到数据库中，在此不是问题的关键省略 
                }


        }
        //打开excel 返回 DataTable
        private DataTable ReadFromExcel(string excelpath)
        {
            string sExt = System.IO.Path.GetExtension(excelpath);
            string sConn = null;
            if (sExt == ".xlsx"  )
            {
                sConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + excelpath + ";" + "Extended Properties='Excel 12.0;HDR=YES'";
            }
            else if (sExt == ".xls")
            {
                sConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + excelpath + ";" + "Extended Properties=Excel 8.0";
            }
           
            else
            {
                throw new Exception("文件格式有误");
            }
            OleDbConnection oledbConn = new OleDbConnection(sConn);
            oledbConn.Open();
            OleDbDataAdapter command = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", oledbConn);
            DataSet ds = new DataSet();
            command.Fill(ds);
            oledbConn.Close();
            return ds.Tables[0];
        }

        //打开excel 返回 DataTable
        private void ReadFromExcel(string excelpath,ref DataTable dt)
        {
            string sExt = System.IO.Path.GetExtension(excelpath);
            string sConn = null;
            if (sExt == ".xlsx")
            {
                sConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + excelpath + ";" + "Extended Properties='Excel 12.0;HDR=NO'";
            }
            else if (sExt == ".xls")
            {
                sConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + excelpath + ";" + "Extended Properties=Excel 8.0";
            }
            else
            {
                throw new Exception("文件格式有误");
            }
            OleDbConnection oledbConn = new OleDbConnection(sConn);
            oledbConn.Open();
            OleDbDataAdapter command = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", oledbConn);
            //DataSet ds = new DataSet();
            command.Fill(dt);
            oledbConn.Close();
            //return ds.Tables[0];
        }


        
        private void 导入CSV数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFdlg = new OpenFileDialog();
            OpenFdlg.CheckFileExists = false; //设置不弹出警告            
            OpenFdlg.Title = "导入excel文件";
            OpenFdlg.Filter = "excel格式文件（*.xls）|*.xls;*.xlsx;*.csv";
            OpenFdlg.ShowDialog();
            string filePath = OpenFdlg.FileName;
            if (filePath == string.Empty)
            {
                return;
            }

            //DataGridView dgBom = new DataGridView();
            //ReadExcel(filePath, ref dgBom);
            ////dgBom.Show();

           
            dt = ReadFromExcel(filePath);
            MessageBox.Show("dt 读取成功！\ndt.Rows.Count = "+dt.Rows.Count);
            Console.WriteLine("dt.Rows.Count = " + dt.Rows.Count +
                "\n dt.Columns.Count = " + dt.Columns.Count);
            for(int i = 5; i >0; i--)
            {
                string item = dt.Rows[i][i].ToString();
                Console.WriteLine("dt.Rows[" + i + "][" + i + "] = " + item);
            }
            Console.WriteLine("dt.Rows[0][0] = " + dt.Rows[0][0].ToString());
            
        }
        private void 登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 未登录状态
            if ((pipe == -2) && (segment == -2))
            {
                Form frmExportDlg = new launch();
                frmExportDlg.Show();
            }
            else
            {
                Form frmExportDlg = new launchexit();
                frmExportDlg.Show();
            }
        }
                    public static bool check_launch()
        {
            if (usr != null)
                return true;
            else return false;
        }

        /// <summary>
        /// 登录时用来刷新主窗口状态
        /// </summary>
        public void refresh_label_launch()
        {
            label1.Text = "已登录用户：" + usr;
            label1.Update();
        }

        /// <summary>
        /// 退出登录时用来刷新主窗口状态
        /// </summary>
        public void refresh_label_launchout()
        {
            label1.Text = "用户未登录";
            label1.Update();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void axToolbarControl1_OnMouseDown(object sender, IToolbarControlEvents_OnMouseDownEvent e)
        {

        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 高后果区识别ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IHookHelper map_hookHelper = new HookHelperClass();
            map_hookHelper.Hook = axMapControl1.Object;
            Form bufferAnalysis = new BufferAnalysis(map_hookHelper);
            bufferAnalysis.Show();
        }

        private void 向数据库中导入管道ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frmExportDlg = new ImportPipeToMySQL();
            frmExportDlg.Show();
        }
    }
}
