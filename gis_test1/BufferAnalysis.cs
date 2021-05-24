using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geoprocessor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gis_test1
{
    public partial class BufferAnalysis : Form
    {
        private IActiveView pActiveView = null;
        private IMap pMap = null;
        int selectedLayer = -1;
        /// <summary>
        /// 选中的 管线名称
        /// </summary>
        string selectedLayerName = null;
        /// <summary>
        /// 选中 的 管线 中的点：从起点到终点的有序点集合
        /// </summary>
        List<IPoint> LinePoints = new List<IPoint>();
        /// <summary>
        /// 选中的 待缓冲分析的图层名称， 可能包含 点、线、面三种类型
        /// 即：多选菜单中 被勾选 的图层名
        /// </summary>
        List<string> SlectedLayerList = new List<string>();

        private AxMapControl axMapControl1 = null;

        private DataTable point_H = null;

        private IGraphicsContainer pIGraphicsContainer = null;
        private IGraphicsContainer pIGraphicsContainer_Slected = null;
        private IGeographicCoordinateSystem pGeographicCoordinateSystem;
        private IProjectedCoordinateSystem pProjectedCoordinateSystem;
        /// <summary>
        /// 线缓冲区集合：多个面
        /// </summary>
        List<IPolygon> lineBufferPolygons = new List<IPolygon>();

        /// <summary>
        /// 点缓冲区集合：多个面
        /// </summary>
        List<IPolygon> pointBufferPolygons = new List<IPolygon>();

        /// <summary>
        /// 点、线缓冲区的交集 集合：多个面
        /// </summary>
        List<IPolygon> IntersectBufferPolygons = new List<IPolygon>();

        /// <summary>
        /// 点缓冲区 和 线 的交集 集合：多段线
        /// </summary>
        List<IPolyline> IntersectBufferPolylines = new List<IPolyline>();

        /// <summary>
        /// 记录一条管线高后果区台账信息
        /// </summary>
        private Dictionary<int, List<HCA>> HCADic = new Dictionary<int, List<HCA>>();
        /// <summary>
        /// 以高后果区段为单位 返回用于填充 dataGrideView 的高后果区信息
        /// </summary>
        private List<ExportInfo> exportInfoList = new List<ExportInfo>();

        private double BufferDistance = 5000;

        private void dataInit(ref DataTable points)
        {
            point_H = new DataTable();
            points.Columns.Add("经度", typeof(Double));
            points.Columns.Add("纬度", typeof(Double));
            points.Columns.Add("风险类型", typeof(String));
            points.Columns.Add("描述", typeof(String));

            points.Rows.Add(82.6411, 41.9008, "人员密集型", "克孜尔检查站");
            points.Rows.Add(82.8705, 41.8233, "人员密集型", "盐水沟检查站");
            points.Rows.Add(83.1175, 41.7631, "环境影响型", "哈浪沟河");
            points.Rows.Add(83.1817, 41.7267, "人员密集型", "村庄");
            points.Rows.Add(83.3158, 41.6839, "人员密集型", "村庄");
            points.Rows.Add(83.3425, 41.6850, "人员密集型/火灾影响类", "牙哈处理厂，变电站");
        }
        /// <summary>
        /// 根据 管线 名称 获取 对应 有序点集合：LinePoints
        /// </summary>
        /// <param name="LineName">管线名称</param>
        /// <param name="points">构成管线的有序点集</param>
        private void LineInfoInit(string LineName, ref List<IPoint> points)
        {
            // mysql 中查找中文字段，需将中文字段放在 `` 中
            //string sql = @"select `纬度（小数）`,`经度（小数）`from gis0322";
            string sql = @"select `经度（小数）`,`纬度（小数）`from gis0322 order by `序号`";
            var dt = MyDataBase.MyDatabase.QueryFromDt(sql);
            points.Clear();
            if(dt != null)
            {
                
                for(int row = 0; row < dt.Rows.Count; row++)
                {
                    IPoint point = new PointClass();
                    point.X = Convert.ToDouble(dt.Rows[row][0]);
                    point.Y = Convert.ToDouble(dt.Rows[row][1]);
                    points.Add(point);
                }
            }
        }

        
        public BufferAnalysis(IHookHelper hookHelper)
        {
            InitializeComponent();
            pActiveView = hookHelper.ActiveView;
            IPageLayout iPageLayout = hookHelper.PageLayout;

            IntPtr pHandle = new IntPtr (hookHelper.ActiveView.ScreenDisplay.hWnd);
            axMapControl1 = System.Windows.Forms.Form.FromHandle(pHandle) as AxMapControl;
            pMap = axMapControl1.Map;
            pIGraphicsContainer = pMap as IGraphicsContainer;

            //坐标系初始化
            pGeographicCoordinateSystem = axMapControl1.SpatialReference as IGeographicCoordinateSystem;
            pProjectedCoordinateSystem = axMapControl1.SpatialReference as IProjectedCoordinateSystem;
            button2.Visible = false;
            //comboBox内容初始化
            comboBox1.Items.Add("--- 请选择 ---");
            comboBox1.SelectedIndex = 0;
            if (pMap == null) return;
            for (int i = 0; i < pMap.LayerCount; i++)
            {
                String layerName = pMap.get_Layer(i).Name;
                comboBox1.Items.Add(layerName);
                //checkedBox内容初始化
                checkedListBox1.Items.Add(layerName);
            }
            numericUpDown1.Value = 5000;
            //高后果点初始化
            dataInit(ref point_H);
            
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedLayer = comboBox1.SelectedIndex - 1;
            if(selectedLayer >= 0 && selectedLayer < pMap.LayerCount)
            {
                selectedLayerName = pMap.Layer[selectedLayer].Name;
                //MessageBox.Show("选中的图层编号为：" + selectedLayer + " : " + pMap.Layer[selectedLayer].Name);
            }
            //构成选中管线的点信息初始化
            LineInfoInit(selectedLayerName, ref LinePoints);
            //showPoints();
        }
        /// <summary>
        /// 长度(米)转换经纬度差
        /// </summary>
        /// <param name="bufferDistance"></param>
        /// <returns></returns>
        public static double convertedBufferDistanceToJWD(double bufferDistance)
        {
            IUnitConverter unitConverter = new UnitConverterClass();
            double convertedBufferDistance = unitConverter.ConvertUnits(bufferDistance, esriUnits.esriMeters, esriUnits.esriDecimalDegrees);
            return convertedBufferDistance;
        }
        /// <summary>
        ///经纬度差转换长度(米)
        /// </summary>
        /// <param name="bufferDistance"></param>
        /// <returns></returns>
        public static double convertedBufferDistanceToMeter(double bufferDistance)
        {
            IUnitConverter unitConverter = new UnitConverterClass();
            double convertedBufferDistance = unitConverter.ConvertUnits(bufferDistance, esriUnits.esriDecimalDegrees, esriUnits.esriMeters);
            return convertedBufferDistance;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (selectedLayer < 0 || point_H == null)
            {
                string msg = "selectedLayer < 0 || point_H == null\n\t重新选择！";
                MessageBox.Show(msg);
                return;
            }
            BufferDistance = Convert.ToDouble(numericUpDown1.Value);

            //记录 checkedListBox1 中被勾选的 图层名称 SlectedLayerList
            #region
            {
                string msg2 = "";
                SlectedLayerList.Clear();
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    //string itemText = checkedListBox1.GetItemText(checkedListBox1.Items[i]);
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        SlectedLayerList.Add(checkedListBox1.GetItemText(checkedListBox1.Items[i]));
                        msg2 += checkedListBox1.GetItemText(checkedListBox1.Items[i]) + "\n";
                    }
                }
                MessageBox.Show(msg2);
            }

            #endregion
            List<IPoint> path = new List<IPoint>();
            for (int i = 0; i < point_H.Rows.Count; i++)
            {
                IPoint p = new PointClass();
                p.X = Convert.ToDouble(point_H.Rows[i][0]);
                p.Y = Convert.ToDouble(point_H.Rows[i][1]);
                path.Add(p);
      
            }

            ILayer lineLayer = pMap.Layer[selectedLayer];
           
            { 
                //线缓冲区
                lineBufferPolygons = GetLineBufferArea(lineLayer, 10);
                // 展示 线 缓冲区
                #region
                foreach (IPolygon plgon in lineBufferPolygons)
                {
                    IPolygonElement PolygonElement = new PolygonElementClass();
                    //实例化要素以装载缓冲区
                    IElement element = PolygonElement as IElement;
                    //多边形填充颜色
                    ISimpleFillSymbol pSimpleFillSymbol = new SimpleFillSymbolClass();
                    IRgbColor pColor = GetColor(0, 60, 0);
                    pSimpleFillSymbol.Color = pColor;
                    pSimpleFillSymbol.Style = esriSimpleFillStyle.esriSFSDiagonalCross;
                    IFillShapeElement pFillEle = PolygonElement as IFillShapeElement;
                    pFillEle.Symbol = pSimpleFillSymbol;

                    //将几何要素赋值为多边形
                    element.Geometry = plgon;

                    pIGraphicsContainer.AddElement(element, 0);
                }
                #endregion
            }

            {
                // 线图层 和 点缓冲区 交集： 多线段---高后果管段
                IntersectBufferPolylines = GetLineIntersect(lineLayer, pointBufferPolygons);
                //将 多线段 展示
                #region
                foreach (IPolyline lineIntersect in IntersectBufferPolylines)
                {
                    //将线 缓冲 得到 高后果管道缓冲区
                    IPolygon lineIntersectBuffer = GetGeometryBufferArea(lineIntersect, 10);

                    //实例化要素以装载缓冲区
                    IPolygonElement PolygonElement = new PolygonElementClass();
                    IElement element = PolygonElement as IElement;
                    //多边形填充颜色
                    ISimpleFillSymbol pSimpleFillSymbol = new SimpleFillSymbolClass();
                    IRgbColor pColor = GetColor(255, 255, 0);
                    pSimpleFillSymbol.Color = pColor;
                    pSimpleFillSymbol.Style = esriSimpleFillStyle.esriSFSVertical;
                    IFillShapeElement pFillEle = PolygonElement as IFillShapeElement;
                    pFillEle.Symbol = pSimpleFillSymbol;

                    //将几何要素赋值为多边形
                    element.Geometry = lineIntersectBuffer;

                    pIGraphicsContainer.AddElement(element, 0);
                }
                #endregion
            }
            
            
            {
                // 高后果区查找功能测试
                button1.Enabled = false;
                button2.Enabled = false;
                button2.Visible = true;
                HCADic = HighConsequenceAera(ref LinePoints, SlectedLayerList, BufferDistance);
                button1.Enabled = true;
                if (HCADic.Count < 1)
                {
                    MessageBox.Show("未发现高后果区！");
                    button2.Enabled = false;
                    return;
                }
                
                //打印高后果区信息
                printHCAInfo(ref HCADic);
                //展示高后果区管段
                exportInfoList = showHCASegment(ref HCADic);
                //以高后果区段为单位 返回用于填充 dataGrideView 的高后果区信息，补全 exportInfoList 中缺失的 地物名称
                fillFeatureName(ref exportInfoList);
                //button2 点击时展示、导出高后果区台账
                button2.Enabled = true;

            }

            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            HCAExport hcaExport = new HCAExport(ref HCADic,ref exportInfoList);
            hcaExport.Show();
        }

        /// <summary>
        /// 返回 线layer 和 面polygon 的交集 System.Collections.ArrayList 线
        /// </summary>
        /// <param name="layer">线layer</param>
        /// <param name="polygon">面polygon</param>
        /// <returns></returns>
        /// 计算arcengine 线要素和面要素相交，每个面所截的线段长度
        ///    https://blog.csdn.net/tianxinzhe032/article/details/6660097
        public static System.Collections.ArrayList func(ref ILayer layer, ref IPolygon polygon)
        {
            System.Collections.ArrayList result = null;
            //IGeometry[] pIGeometry = null;
            //分别获取Polyline和Polygon的要素类
            IFeatureLayer pFeatureLayerPolyline = layer as IFeatureLayer;
            IFeatureLayer pFeatureLayerPolygon = polygon as IFeatureLayer;

            IFeatureClass pFeatureClassPolyline = pFeatureLayerPolyline.FeatureClass;
            IFeatureClass pFeatureClassPolygon = pFeatureLayerPolygon.FeatureClass;
            
            IFeatureCursor pPolyCursor = pFeatureClassPolygon.Search(null, false);
            IFeature pPolyFeature = pPolyCursor.NextFeature();

            while (pPolyFeature != null)
            {
                //IFeature pFeaturePolygon = pFeatureClassPolygon.GetFeature(18);
                IPolygon pPolygon = pPolyFeature.ShapeCopy as IPolygon;

                //获取线要素类的所有要素
                IFeatureCursor pFeatureCursorPolyline = pFeatureClassPolyline.Search(new QueryFilterClass(), false);
                IFeature pFeaturePolyline = pFeatureCursorPolyline.NextFeature();
                //遍历每一个线要素
                while (pFeaturePolyline != null)
                {
                    //求该线与某个面要素相交的几何线段
                    IPolyline pPolyline = pFeaturePolyline.ShapeCopy as IPolyline;
                    ITopologicalOperator pTopologicalOperator = pPolyline as ITopologicalOperator;
                    IPolyline pPolylineResult =
                        pTopologicalOperator.Intersect(pPolygon, esriGeometryDimension.esriGeometry1Dimension) as
                        IPolyline;
                    result.Add(pPolylineResult);
                    pFeaturePolyline = pFeatureCursorPolyline.NextFeature();
                }
                pPolyFeature = pPolyCursor.NextFeature();
            }
            return result;
        }
               
        /// <summary>
        /// 求 GeometryA 与 GeometryB 交集
        /// </summary>
        /// <param name="GeometryA"></param>
        /// <param name="GeometryB"></param>
        /// <returns></returns>
        public static IGeometry IntersectTwoGeometries(IGeometry GeometryA, IGeometry GeometryB)
        {
            if (GeometryA == null || GeometryB == null) return null;
            ITopologicalOperator pTopologicalOperator = GeometryA as ITopologicalOperator;
            IGeometry IntersectGeometry = pTopologicalOperator.Intersect(GeometryB, esriGeometryDimension.esriGeometry2Dimension);
            return IntersectGeometry;
        }

        /// <summary>
        /// 获取颜色对象，传入rgb值
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static IRgbColor GetColor(int r, int g, int b)
        { 
            IRgbColor pColor = new RgbColor();
            pColor.Red = r;
            pColor.Green = g;
            pColor.Blue = b;
            return pColor;
        }
        
        
        /// <summary>
        /// 求 线图层 缓冲区
        /// </summary>
        /// <param name="layer">线图层</param>
        /// <param name="BuffDistance">缓冲半径，单位：m</param>
        /// <returns></returns>
        ///   https://www.cnblogs.com/edcoder/p/11797487.html
        public static List<IPolygon> GetLineBufferArea(ILayer layer, double BuffDistance)
        {
            //以主地图为缓冲区添加对象
            //IGraphicsContainer graphicsContainer = axMapControl1.Map as IGraphicsContainer;
            List<IPolygon> PolygonList = new List<IPolygon>();
            //将图层名为LayerName的图层强转成要素选择集
            IFeatureSelection pFtSel = (IFeatureLayer)layer as IFeatureSelection;
            //将图层名为LayerName的图层中的所有要素加入选择集
            pFtSel.SelectFeatures(null, esriSelectionResultEnum.esriSelectionResultNew, false);

            ICursor pCursor;
            //获得遍历选择集中所有要素的游标
            pFtSel.SelectionSet.Search(null, false, out pCursor);
            IFeatureCursor pFtCursor = pCursor as IFeatureCursor;
            IFeature pFt = pFtCursor.NextFeature();
            //遍历所有选择集中的所有要素, 逐个要素地创建缓冲区
            while (pFt != null)
            {
                //将要素的几何对象(pFt.Shape)强转成ITopologicalOperator
                //pFt.Shape即为创建缓冲区的操作对象
                ITopologicalOperator topologicalOperator = pFt.Shape as ITopologicalOperator;
                //注意: BuffDIstance输入为正时向外缓冲, 为负时向内缓冲
                IPolygon polygon = topologicalOperator.Buffer(convertedBufferDistanceToJWD(BuffDistance)) as IPolygon;
                
                PolygonList.Add(polygon);
                //指向下一个
                pFt = pFtCursor.NextFeature();
            }
            //这里清除选择集, 以免高亮显示的要素与缓冲结果相互混淆
            pFtSel.Clear();
            return PolygonList;
        }

        /// <summary>
        /// 获取 Geometry对象：点、线、面 缓冲区
        /// </summary>
        /// <param name="p"></param>
        /// <param name="BuffDistance">缓冲半径，单位：m</param>
        /// <returns>点或线或面 的 缓冲区：面</returns>
        public static IPolygon GetGeometryBufferArea(IGeometry p, double BuffDistance)
        {
            ITopologicalOperator topologicalOperator = (ITopologicalOperator)p;
            //Create a polygon by buffering the point and get the IPolygon interface
            IGeometry pGeo = topologicalOperator.Buffer(convertedBufferDistanceToJWD(BuffDistance));
            return pGeo as IPolygon;
            IPolygon bufferPolygon = (IPolygon)topologicalOperator.Buffer(convertedBufferDistanceToJWD(BuffDistance));
            return bufferPolygon;
        }

        /// <summary>
        /// 求 线图层 和 多边形 交集，返回 线段集合
        /// </summary>
        /// <param name="layer">线图层</param>
        /// <param name="polygonList">多边形集合</param>
        /// <returns>线与多边形相交线段集合</returns>
        public static List<IPolyline> GetLineIntersect(ILayer layer, List<IPolygon> polygonList)
        {
            List<IPolyline> PolylineList = new List<IPolyline>();
            //将图层名为LayerName的图层强转成要素选择集
            IFeatureSelection pFtSel = (IFeatureLayer)layer as IFeatureSelection;
            //将图层名为LayerName的图层中的所有要素加入选择集
            pFtSel.SelectFeatures(null, esriSelectionResultEnum.esriSelectionResultNew, false);

            ICursor pCursor;
            //获得遍历选择集中所有要素的游标
            pFtSel.SelectionSet.Search(null, false, out pCursor);
            IFeatureCursor pFtCursor = pCursor as IFeatureCursor;
            IFeature pFt = pFtCursor.NextFeature();
            IFeature tmppFt = pFt;
            
            foreach (Polygon polygon in polygonList)
            {
                ITopologicalOperator topologicalOperator = polygon as ITopologicalOperator;
                pFt = tmppFt;
                while (pFt != null)
                {
                    IPolyline intersectLine = topologicalOperator.Intersect(pFt.Shape, esriGeometryDimension.esriGeometry1Dimension) as IPolyline;
                    if(intersectLine != null)
                    {
                        PolylineList.Add(intersectLine);
                    }
                    pFt = pFtCursor.NextFeature();
                }
            }
            //这里清除选择集, 以免高亮显示的要素与缓冲结果相互混淆
            pFtSel.Clear();
            return PolylineList;
        }

        /// <summary>
        /// 返回layer中距 路径path 一定距离范围内 所有要素 的 id编号
        /// </summary>
        /// <param name="Path">有序的点构成的路径</param>
        /// <param name="layer">待分析的图层</param>
        /// <param name="distance">缓冲距离，单位：米</param>
        /// <returns></returns>
        public static List<int> GetFeatureOIDAroundPath(List<IPoint> Path, ILayer layer, double distance)
        {
            if (Path.Count == 0 || layer == null)
            {
                return null;
            }
            //根据 Path 中 有序点 创建线段
            IPolygon buffer = null;
            IFeatureLayer currentFeatureLayer = layer as IFeatureLayer;
            List<IFeature> featureSelected = new List<IFeature>();
            List<int> featurnOID = new List<int>();
            IPoint from = Path[0];
            if(Path.Count == 1)
            {
                //Path 中 只有一个点,则按点的缓冲区处理
                buffer = GetGeometryBufferArea(Path[0], distance);
            }
            else
            {
                //Path 中包含的点不止一个，则顺序连接成线段，取线段的缓冲区
                IPointCollection m_PointCollection = new PolylineClass();
                for(int i = 0; i < Path.Count; i++)
                {
                    m_PointCollection.AddPoint(Path[i], Type.Missing, Type.Missing);
                }
                IPolyline m_Polyline = new PolylineClass();
                m_Polyline = m_PointCollection as IPolyline;

                buffer = GetGeometryBufferArea(m_Polyline, distance);
            }
            
            //将图层名为LayerName的图层强转成要素选择集
            IFeatureSelection pFtSel = (IFeatureLayer)layer as IFeatureSelection;
            //将图层名为LayerName的图层中的所有要素加入选择集
            pFtSel.SelectFeatures(null, esriSelectionResultEnum.esriSelectionResultNew, false);
            ICursor pCursor;
            //获得遍历选择集中所有要素的游标
            pFtSel.SelectionSet.Search(null, false, out pCursor);
            IFeatureCursor pFtCursor = pCursor as IFeatureCursor;
            IFeature pFt = pFtCursor.NextFeature();
            //对 buffer 建立 空间运算 分析接口
            IRelationalOperator relationalOperator = buffer as IRelationalOperator;
            //遍历所有选择集中的所有要素, 逐个 比较 是否与 buffer 有交集
            while (pFt != null)
            {
                if (relationalOperator.Contains(pFt.Shape))
                {
                    //layer 中 与 buffer 不相离 的要素，包含：相交，包含等，即 二者交集不为空
                    //将 与buffer 有 交集的要素 的OID 返回
                    featurnOID.Add(pFt.OID);
                }
                //指向下一个
                pFt = pFtCursor.NextFeature();
            }
            //这里清除选择集, 以免高亮显示的要素与缓冲结果相互混淆
            pFtSel.Clear();
            
            return featurnOID;
        }

        /// <summary>
        /// 管线 高后果区查找
        /// </summary>
        /// <param name="LinePoints">管线：由顺序点集组成</param>
        /// <param name="layers">周边地物图层集合</param>
        /// <param name="distance">缓冲分析距离，单位：米</param>
        /// <returns></returns>
        private Dictionary<int, List<HCA>> HighConsequenceAera(ref List<IPoint> LinePoints,List<string> layers, double distance)
        {
            Dictionary<int, List<HCA>> HCAFeatureID = new Dictionary<int, List<HCA>>();
            for (int i = 0; i < LinePoints.Count - 1; i++)
            {
                // 组成管线的点，每两个点组成一条 path
                List<IPoint> path = new List<IPoint>();
                path.Add(LinePoints[i]);
                path.Add(LinePoints[i + 1]);
                // 对 每条path： 与 周边地物图层 进行缓冲分析

                //一条 path 缓冲 到 多个 layer中，各 layer 中 要素 id 记录在 pathHCA[i] 中
                List<HCA> pathHCA = new List<HCA>();
                foreach (string layerName in layers)
                {
                    int layIdx = searchLayerByName(layerName);
                    if (layIdx == -1)
                    {
                        continue;
                    }
                    ILayer pLayer = axMapControl1.get_Layer(layIdx);
                    //一条 path 缓冲 到 某层 layer中的要素ID, 记录在 hca 中
                    HCA hca;
                    hca.layerName = layerName;
                    hca.featureOID = GetFeatureOIDAroundPath(path, pLayer, distance);
                    if(hca.featureOID.Count > 0)
                    {
                        pathHCA.Add(hca);
                        //MessageBox.Show(i.ToString() + "path\n" + layerName + hca.featureOID.Count.ToString());
                    }
                }
                if(pathHCA.Count > 0)
                {
                    HCAFeatureID.Add(i, pathHCA);
                }
            }
            return HCAFeatureID;
        }
        /// <summary>
        /// 记录 被 path 缓冲到 的 layer 中的 要素 ID
        /// </summary>
        public struct HCA
        {
            public string layerName;    //地物图层名称
            public List<int> featureOID;    // 地物图层中的被缓冲到的 要素 ID
        };
        /// <summary>
        /// 高后果区 台账 信息
        /// </summary>
        public struct ExportInfo
        {
            //public string lineLayerName;    // 管线图层名称
            public int startIdx;         // 高后果区管道起点索引
            public int endIdx;           // 高后果区管道终点索引
            public double length;           // 高后果区长度
            public Dictionary<string, Dictionary<int, string>> factor;    //识别因素（图层名称），管段描述（地物名称）
        };
        private int searchLayerByName(string name)
        {
            int ret = -1;
            for(int i = 0; i < axMapControl1.Map.LayerCount; i++)
            {
                if(axMapControl1.Map.Layer[i].Name == name)
                {
                    return i;
                }
            }
            return ret;
        }
        /// <summary>
        /// 打印 高后果区 信息
        /// </summary>
        /// <param name="HCADic"></param>
        private void printHCAInfo(ref Dictionary<int, List<HCA>> HCADic)
        {
            string msg1 = " ";
            foreach (var item in HCADic)
            {
                //每一段 path 的起点
                int start = item.Key;
                msg1 += start.ToString() + " 周边地物：\n";
                //每一段 path 对应在 各周边地物图层的高后果区要素
                List<HCA> hcaList = item.Value;
                foreach (var hca in hcaList)
                {
                    string lyrName = hca.layerName;
                    msg1 += "\t" + lyrName + "图层中涉及要素id ：";
                    foreach (int id in hca.featureOID)
                    {
                        msg1 += id.ToString() + " ";
                    }
                    msg1 += "\n";
                }
                msg1 += "\n\n";
            }
            //MessageBox.Show(msg1);`
            Console.WriteLine(msg1);
        }

        /// <summary>
        /// 展示高后果区管段
        /// </summary>
        /// <param name="HCADic">以 管线上相邻两点为单位的高后果区信息</param>
        /// <returns>以高后果区段为单位 返回用于填充 dataGrideView 的高后果区信息 </returns>
        private List<ExportInfo> showHCASegment(ref Dictionary<int, List<HCA>> HCADic)
        {
            List<ExportInfo> exportInfoList = new List<ExportInfo>();
            if (HCADic.Count < 1)
            {
                return exportInfoList;
            }
            int prv = HCADic.First().Key;
            IPointCollection hcaLinePoint = new PolylineClass();
            List<IPoint> hcaSegment = new List<IPoint>();
            
            for (int i = 0; i < HCADic.Count - 1; i++)
            {
                hcaSegment.Clear();
                ExportInfo exportInfo = new ExportInfo();
                Dictionary<string, Dictionary<int, string>> factor = new Dictionary<string, Dictionary<int, string>>();
                int start = HCADic.ElementAt(i).Key;
                hcaSegment.Add(LinePoints[start - 1]);
                exportInfo.startIdx = start - 1;   // 本段高后果区 起点 编号
                //exportInfo.factor.Add(HCADic[start])
                while (i < HCADic.Count - 1 && HCADic.ElementAt(i + 1).Key - start == 1)
                {
                    start++;
                    hcaSegment.Add(LinePoints[start - 1]);
                    List<HCA> Hca = HCADic.ElementAt(i).Value;
                    foreach(var hca in Hca)
                    {
                        string lyrName = hca.layerName;
                        if(factor.ContainsKey(lyrName) == false)
                        {
                            Dictionary<int, string> tmp = new Dictionary<int, string>();
                            foreach (int id in hca.featureOID)
                            {
                                tmp.Add(id, null);
                            }
                            factor.Add(lyrName,tmp);
                        }
                        else
                        {
                            foreach(int id in hca.featureOID)
                            {
                                if(factor[lyrName].Keys.Contains(id) == false)
                                {
                                       factor[lyrName].Add(id, null);
                                }
                            }
                        }
                        
                    }
                    i++;
                }
                //相同path编号的 高后果区 结束,可以画出来一段
                hcaSegment.Add(LinePoints[start]);
                exportInfo.endIdx = start;     // 本段高后果区 终点 编号
                
                //本段高后果区结束，画线
                IPointCollection pPointCollection = new PolylineClass();
                foreach (var pt in hcaSegment)
                {
                    pPointCollection.AddPoint(pt);
                }
                IPolyline pPolyline = new PolylineClass();
                pPolyline = pPointCollection as IPolyline;
                exportInfo.length = Math.Round(convertedBufferDistanceToMeter(pPolyline.Length),2);  // 本段高后果区 长度
                exportInfo.factor = factor;
                exportInfoList.Add(exportInfo);

                IPolygon lineIntersectBuffer = GetGeometryBufferArea(pPolyline, 12);

                //实例化要素以装载缓冲区
                IPolygonElement PolygonElement = new PolygonElementClass();
                IElement element = PolygonElement as IElement;
                //多边形填充颜色
                ISimpleFillSymbol pSimpleFillSymbol = new SimpleFillSymbolClass();
                IRgbColor pColor = GetColor(255, 255, 0);
                pSimpleFillSymbol.Color = pColor;
                pSimpleFillSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
                IFillShapeElement pFillEle = PolygonElement as IFillShapeElement;
                pFillEle.Symbol = pSimpleFillSymbol;

                //将几何要素赋值为多边形
                element.Geometry = lineIntersectBuffer;

                pIGraphicsContainer.AddElement(element, 0);
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                //新的一段高后果区开始
            }
            return exportInfoList;
        }

        /// <summary>
        /// 补全 exportInfoList 中缺失的 地物名称
        /// </summary>
        /// <param name="exportInfoList"></param>
        private void fillFeatureName(ref List<ExportInfo> exportInfoList)
        {
            foreach (var e in exportInfoList)
            {
                var factorDic = e.factor;
                string msg = "起点：" + e.startIdx.ToString() + " 终点：" + e.endIdx.ToString() + " 长度：" + 
                    e.length.ToString() + ":\n";
                foreach (var fact in factorDic)
                {
                    string lyrName = fact.Key;
                    msg += "图层名：" + lyrName;
                    var idDic = fact.Value;
                    for(int k = 0; k < idDic.Count; k++)
                    {
                        var id = idDic.ElementAt(k);
                        msg += id.Key.ToString() + " ";
                        string featureName = getFeatureNameByOID(lyrName, id.Key);
                        idDic[id.Key] = featureName;
                        msg += idDic[id.Key] + " ";
                    }
                    msg += "\n";
                }
                Console.WriteLine(msg);
            }
        }

        /// <summary>
        /// 获取指定图层中， 指定要素id某一列的属性
        /// </summary>
        /// <param name="lyrName"></param>
        /// <param name="id">要素OID</param>
        /// <param name="idx"></param>
        /// <returns></returns>
        private string getFeatureNameByOID(string lyrName, int id, int idx = 2)
        {
            string name = "";
            int lyrIdx = searchLayerByName(lyrName);
            if(lyrIdx < 0)
            {
                return name;
            }
            try
            {
                ILayer layer = axMapControl1.Map.get_Layer(lyrIdx);
                //将图层名为LayerName的图层强转成要素选择集
                IFeatureSelection pFtSel = (IFeatureLayer)layer as IFeatureSelection;
                //将图层名为LayerName的图层中的所有要素加入选择集
                pFtSel.SelectFeatures(null, esriSelectionResultEnum.esriSelectionResultNew, false);
                ICursor pCursor;
                //获得遍历选择集中所有要素的游标
                pFtSel.SelectionSet.Search(null, false, out pCursor);
                IFeatureCursor pFtCursor = pCursor as IFeatureCursor;
                IFeature pFt = pFtCursor.NextFeature();
                while (pFt != null)
                {
                    if(pFt.OID == id)
                    {
                        name = pFt.Value[idx].ToString();
                        break;
                    }
                    pFt = pFtCursor.NextFeature();
                }
                pFtSel.Clear();

            }
            catch
            {

            }
            return name;
        }

    }
}
