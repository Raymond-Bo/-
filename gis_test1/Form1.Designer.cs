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


namespace gis_test1
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.在线地图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.中国地图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.影像地图ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.街道地图ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.遥感地图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.登录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导入地图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.向数据库中导入管道ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.漫游ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.高后果区识别ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.axTOCControl1 = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.删除图层ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.显示图层属性ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.移动到顶部ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除所有图层ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(266, 260);
            this.axLicenseControl1.Margin = new System.Windows.Forms.Padding(2);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 3;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.在线地图ToolStripMenuItem,
            this.登录ToolStripMenuItem,
            this.文件ToolStripMenuItem,
            this.漫游ToolStripMenuItem,
            this.高后果区识别ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(843, 25);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 在线地图ToolStripMenuItem
            // 
            this.在线地图ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.中国地图ToolStripMenuItem});
            this.在线地图ToolStripMenuItem.Name = "在线地图ToolStripMenuItem";
            this.在线地图ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.在线地图ToolStripMenuItem.Text = "在线地图";
            // 
            // 中国地图ToolStripMenuItem
            // 
            this.中国地图ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.影像地图ToolStripMenuItem1,
            this.街道地图ToolStripMenuItem1,
            this.遥感地图ToolStripMenuItem});
            this.中国地图ToolStripMenuItem.Name = "中国地图ToolStripMenuItem";
            this.中国地图ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.中国地图ToolStripMenuItem.Text = "预置地图";
            // 
            // 影像地图ToolStripMenuItem1
            // 
            this.影像地图ToolStripMenuItem1.Name = "影像地图ToolStripMenuItem1";
            this.影像地图ToolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.影像地图ToolStripMenuItem1.Text = "城市地图";
            this.影像地图ToolStripMenuItem1.Click += new System.EventHandler(this.城市地图ToolStripMenuItem1_Click);
            // 
            // 街道地图ToolStripMenuItem1
            // 
            this.街道地图ToolStripMenuItem1.Name = "街道地图ToolStripMenuItem1";
            this.街道地图ToolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.街道地图ToolStripMenuItem1.Text = "街道地图";
            this.街道地图ToolStripMenuItem1.Click += new System.EventHandler(this.街道地图ToolStripMenuItem1_Click);
            // 
            // 遥感地图ToolStripMenuItem
            // 
            this.遥感地图ToolStripMenuItem.Name = "遥感地图ToolStripMenuItem";
            this.遥感地图ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.遥感地图ToolStripMenuItem.Text = "遥感地图";
            this.遥感地图ToolStripMenuItem.Click += new System.EventHandler(this.遥感地图ToolStripMenuItem_Click);
            // 
            // 登录ToolStripMenuItem
            // 
            this.登录ToolStripMenuItem.Name = "登录ToolStripMenuItem";
            this.登录ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.登录ToolStripMenuItem.Text = "登录";
            this.登录ToolStripMenuItem.Click += new System.EventHandler(this.登录ToolStripMenuItem_Click);
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导入地图ToolStripMenuItem,
            this.向数据库中导入管道ToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.文件ToolStripMenuItem.Text = "文件";
            this.文件ToolStripMenuItem.Click += new System.EventHandler(this.文件ToolStripMenuItem_Click);
            // 
            // 导入地图ToolStripMenuItem
            // 
            this.导入地图ToolStripMenuItem.Name = "导入地图ToolStripMenuItem";
            this.导入地图ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.导入地图ToolStripMenuItem.Text = "导入shp文件";
            this.导入地图ToolStripMenuItem.Click += new System.EventHandler(this.shp文件ToolStripMenuItem_Click);
            // 
            // 向数据库中导入管道ToolStripMenuItem
            // 
            this.向数据库中导入管道ToolStripMenuItem.Name = "向数据库中导入管道ToolStripMenuItem";
            this.向数据库中导入管道ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.向数据库中导入管道ToolStripMenuItem.Text = "向数据库中导入管道";
            this.向数据库中导入管道ToolStripMenuItem.Click += new System.EventHandler(this.向数据库中导入管道ToolStripMenuItem_Click);
            // 
            // 漫游ToolStripMenuItem
            // 
            this.漫游ToolStripMenuItem.Name = "漫游ToolStripMenuItem";
            this.漫游ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.漫游ToolStripMenuItem.Text = "漫游";
            // 
            // 高后果区识别ToolStripMenuItem
            // 
            this.高后果区识别ToolStripMenuItem.Name = "高后果区识别ToolStripMenuItem";
            this.高后果区识别ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.高后果区识别ToolStripMenuItem.Text = "高后果区识别";
            this.高后果区识别ToolStripMenuItem.Click += new System.EventHandler(this.高后果区识别ToolStripMenuItem_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 412);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.ShowItemToolTips = true;
            this.statusStrip1.Size = new System.Drawing.Size(843, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 53);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.axMapControl1);
            this.splitContainer1.Size = new System.Drawing.Size(843, 359);
            this.splitContainer1.SplitterDistance = 150;
            this.splitContainer1.TabIndex = 8;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tabControl1);
            this.splitContainer2.Size = new System.Drawing.Size(150, 359);
            this.splitContainer2.SplitterDistance = 252;
            this.splitContainer2.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(150, 252);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.axTOCControl1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(142, 226);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "图层";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // axTOCControl1
            // 
            this.axTOCControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axTOCControl1.Location = new System.Drawing.Point(3, 3);
            this.axTOCControl1.Name = "axTOCControl1";
            this.axTOCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl1.OcxState")));
            this.axTOCControl1.Size = new System.Drawing.Size(136, 220);
            this.axTOCControl1.TabIndex = 0;
            this.axTOCControl1.OnMouseDown += new ESRI.ArcGIS.Controls.ITOCControlEvents_Ax_OnMouseDownEventHandler(this.axTOCControl1_OnMouseDown);
            this.axTOCControl1.OnMouseUp += new ESRI.ArcGIS.Controls.ITOCControlEvents_Ax_OnMouseUpEventHandler(this.axTOCControl1_OnMouseUp);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(142, 226);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "属性";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(142, 226);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "搜索";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // axMapControl1
            // 
            this.axMapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axMapControl1.Location = new System.Drawing.Point(0, 0);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(689, 359);
            this.axMapControl1.TabIndex = 0;
            this.axMapControl1.OnMouseMove += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseMoveEventHandler(this.axMapControl1_OnMouseMove_1);
            this.axMapControl1.OnViewRefreshed += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnViewRefreshedEventHandler(this.axMapControl1_OnViewRefreshed);
            this.axMapControl1.OnExtentUpdated += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnExtentUpdatedEventHandler(this.axMapControl1_OnExtentUpdated);
            this.axMapControl1.OnMapReplaced += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMapReplacedEventHandler(this.axMapControl1_OnMapReplaced);
            // 
            // 删除图层ToolStripMenuItem
            // 
            this.删除图层ToolStripMenuItem.Name = "删除图层ToolStripMenuItem";
            this.删除图层ToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.删除图层ToolStripMenuItem.Text = "删除图层";
            this.删除图层ToolStripMenuItem.Click += new System.EventHandler(this.删除图层ToolStripMenuItem_Click_1);
            // 
            // 显示图层属性ToolStripMenuItem
            // 
            this.显示图层属性ToolStripMenuItem.Name = "显示图层属性ToolStripMenuItem";
            this.显示图层属性ToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.显示图层属性ToolStripMenuItem.Text = "显示图层属性";
            this.显示图层属性ToolStripMenuItem.Click += new System.EventHandler(this.显示图层属性ToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除图层ToolStripMenuItem,
            this.显示图层属性ToolStripMenuItem,
            this.移动到顶部ToolStripMenuItem,
            this.删除所有图层ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(225, 92);
            // 
            // 移动到顶部ToolStripMenuItem
            // 
            this.移动到顶部ToolStripMenuItem.Name = "移动到顶部ToolStripMenuItem";
            this.移动到顶部ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Up)));
            this.移动到顶部ToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.移动到顶部ToolStripMenuItem.Text = "移动到顶部";
            this.移动到顶部ToolStripMenuItem.Click += new System.EventHandler(this.移动到顶部ToolStripMenuItem_Click);
            // 
            // 删除所有图层ToolStripMenuItem
            // 
            this.删除所有图层ToolStripMenuItem.Name = "删除所有图层ToolStripMenuItem";
            this.删除所有图层ToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.删除所有图层ToolStripMenuItem.Text = "删除所有图层";
            this.删除所有图层ToolStripMenuItem.Click += new System.EventHandler(this.删除所有图层ToolStripMenuItem_Click);
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.axToolbarControl1.Location = new System.Drawing.Point(0, 25);
            this.axToolbarControl1.Margin = new System.Windows.Forms.Padding(2);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(843, 28);
            this.axToolbarControl1.TabIndex = 0;
            this.axToolbarControl1.OnMouseDown += new ESRI.ArcGIS.Controls.IToolbarControlEvents_Ax_OnMouseDownEventHandler(this.axToolbarControl1_OnMouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(723, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "用户未登录";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 434);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.axToolbarControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "集输管道高后果区智能识别软件";
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        

        #endregion

        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem 在线地图ToolStripMenuItem;
        private ToolStripMenuItem 中国地图ToolStripMenuItem;
        private ToolStripMenuItem 影像地图ToolStripMenuItem1;
        private ToolStripMenuItem 街道地图ToolStripMenuItem1;
        private ToolStripMenuItem 遥感地图ToolStripMenuItem;
        private ToolStripMenuItem 文件ToolStripMenuItem;
        private ToolStripMenuItem 导入地图ToolStripMenuItem;
        private StatusStrip statusStrip1;
        private SplitContainer splitContainer1;
        private AxMapControl axMapControl1;
        private ToolStripStatusLabel StatusLabel;
        private ToolStripMenuItem 删除图层ToolStripMenuItem;
        private ToolStripMenuItem 显示图层属性ToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip1;
        private SplitContainer splitContainer2;
        private AxTOCControl axTOCControl1;
        private ToolStripMenuItem 移动到顶部ToolStripMenuItem;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private ToolStripMenuItem 删除所有图层ToolStripMenuItem;
        private ToolStripMenuItem 登录ToolStripMenuItem;
        private ToolStripMenuItem 漫游ToolStripMenuItem;
        private ToolStripMenuItem 高后果区识别ToolStripMenuItem;
        private ToolStripMenuItem 退出ToolStripMenuItem;
        private Label label1;
        private ToolStripMenuItem 向数据库中导入管道ToolStripMenuItem;
    }
    /// <summary>
    /// arcgis engine 调用arcgis server服务
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

}

