namespace SimpleGIS
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            MyMapObjects.moLayers moLayers1 = new MyMapObjects.moLayers();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新建ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出地图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.编辑ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.视图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据视图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.布局视图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.插入ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.标题ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.文本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.内图廓线ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.图例ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.比例尺ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.指北针ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.选择ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.按属性选择ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清除所选要素ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.AddLayer = new System.Windows.Forms.ToolStripButton();
            this.Save = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ZoomIn = new System.Windows.Forms.ToolStripButton();
            this.ZoomOut = new System.Windows.Forms.ToolStripButton();
            this.Pan = new System.Windows.Forms.ToolStripButton();
            this.FullExtent = new System.Windows.Forms.ToolStripButton();
            this.SelectFeatures = new System.Windows.Forms.ToolStripButton();
            this.ClearSelectedFeatures = new System.Windows.Forms.ToolStripButton();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.EditSpBtn = new System.Windows.Forms.ToolStripSplitButton();
            this.BeginEditItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EndEditItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MoveFeatureBtn = new System.Windows.Forms.ToolStripButton();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.moMapControl1 = new MyMapObjects.moMapControl();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tssCoordinate = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssMapScale = new System.Windows.Forms.ToolStripStatusLabel();
            this.treeView_RigthMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.移除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.上移至顶层ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.上移一层ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.下一一层ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.下移至底层ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.缩放至图层ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开属性表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.另存为SPGLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditPointBtn = new System.Windows.Forms.ToolStripButton();
            this.CreateFeatureBtn = new System.Windows.Forms.ToolStripButton();
            this.SelectLayer = new System.Windows.Forms.ToolStripComboBox();
            this.btnIdentify = new System.Windows.Forms.ToolStripButton();
            this.SaveEditItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moMapRightMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.图层渲染ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.显示注记ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.treeView_RigthMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.menuStrip1.AutoSize = false;
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.编辑ToolStripMenuItem,
            this.视图ToolStripMenuItem,
            this.插入ToolStripMenuItem,
            this.选择ToolStripMenuItem,
            this.帮助ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 1);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1246, 30);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "菜单栏";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建ToolStripMenuItem,
            this.添加数据ToolStripMenuItem,
            this.导出地图ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(44, 26);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 新建ToolStripMenuItem
            // 
            this.新建ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("新建ToolStripMenuItem.Image")));
            this.新建ToolStripMenuItem.Name = "新建ToolStripMenuItem";
            this.新建ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.新建ToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.新建ToolStripMenuItem.Text = "新建";
            this.新建ToolStripMenuItem.Click += new System.EventHandler(this.新建ToolStripMenuItem_Click);
            // 
            // 添加数据ToolStripMenuItem
            // 
            this.添加数据ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("添加数据ToolStripMenuItem.Image")));
            this.添加数据ToolStripMenuItem.Name = "添加数据ToolStripMenuItem";
            this.添加数据ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.添加数据ToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.添加数据ToolStripMenuItem.Text = "添加数据";
            this.添加数据ToolStripMenuItem.Click += new System.EventHandler(this.添加数据ToolStripMenuItem_Click);
            // 
            // 导出地图ToolStripMenuItem
            // 
            this.导出地图ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("导出地图ToolStripMenuItem.Image")));
            this.导出地图ToolStripMenuItem.Name = "导出地图ToolStripMenuItem";
            this.导出地图ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.导出地图ToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.导出地图ToolStripMenuItem.Text = "导出地图";
            this.导出地图ToolStripMenuItem.Click += new System.EventHandler(this.导出地图ToolStripMenuItem_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // 编辑ToolStripMenuItem
            // 
            this.编辑ToolStripMenuItem.Name = "编辑ToolStripMenuItem";
            this.编辑ToolStripMenuItem.Size = new System.Drawing.Size(44, 26);
            this.编辑ToolStripMenuItem.Text = "编辑";
            // 
            // 视图ToolStripMenuItem
            // 
            this.视图ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.数据视图ToolStripMenuItem,
            this.布局视图ToolStripMenuItem});
            this.视图ToolStripMenuItem.Name = "视图ToolStripMenuItem";
            this.视图ToolStripMenuItem.Size = new System.Drawing.Size(44, 26);
            this.视图ToolStripMenuItem.Text = "视图";
            // 
            // 数据视图ToolStripMenuItem
            // 
            this.数据视图ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("数据视图ToolStripMenuItem.Image")));
            this.数据视图ToolStripMenuItem.Name = "数据视图ToolStripMenuItem";
            this.数据视图ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.数据视图ToolStripMenuItem.Text = "数据视图";
            // 
            // 布局视图ToolStripMenuItem
            // 
            this.布局视图ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("布局视图ToolStripMenuItem.Image")));
            this.布局视图ToolStripMenuItem.Name = "布局视图ToolStripMenuItem";
            this.布局视图ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.布局视图ToolStripMenuItem.Text = "布局视图";
            // 
            // 插入ToolStripMenuItem
            // 
            this.插入ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.标题ToolStripMenuItem,
            this.文本ToolStripMenuItem,
            this.内图廓线ToolStripMenuItem,
            this.图例ToolStripMenuItem,
            this.比例尺ToolStripMenuItem,
            this.指北针ToolStripMenuItem});
            this.插入ToolStripMenuItem.Name = "插入ToolStripMenuItem";
            this.插入ToolStripMenuItem.Size = new System.Drawing.Size(44, 26);
            this.插入ToolStripMenuItem.Text = "插入";
            // 
            // 标题ToolStripMenuItem
            // 
            this.标题ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("标题ToolStripMenuItem.Image")));
            this.标题ToolStripMenuItem.Name = "标题ToolStripMenuItem";
            this.标题ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.标题ToolStripMenuItem.Text = "标题";
            // 
            // 文本ToolStripMenuItem
            // 
            this.文本ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("文本ToolStripMenuItem.Image")));
            this.文本ToolStripMenuItem.Name = "文本ToolStripMenuItem";
            this.文本ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.文本ToolStripMenuItem.Text = "文本";
            // 
            // 内图廓线ToolStripMenuItem
            // 
            this.内图廓线ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("内图廓线ToolStripMenuItem.Image")));
            this.内图廓线ToolStripMenuItem.Name = "内图廓线ToolStripMenuItem";
            this.内图廓线ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.内图廓线ToolStripMenuItem.Text = "内图廓线";
            // 
            // 图例ToolStripMenuItem
            // 
            this.图例ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("图例ToolStripMenuItem.Image")));
            this.图例ToolStripMenuItem.Name = "图例ToolStripMenuItem";
            this.图例ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.图例ToolStripMenuItem.Text = "图例";
            // 
            // 比例尺ToolStripMenuItem
            // 
            this.比例尺ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("比例尺ToolStripMenuItem.Image")));
            this.比例尺ToolStripMenuItem.Name = "比例尺ToolStripMenuItem";
            this.比例尺ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.比例尺ToolStripMenuItem.Text = "比例尺";
            // 
            // 指北针ToolStripMenuItem
            // 
            this.指北针ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("指北针ToolStripMenuItem.Image")));
            this.指北针ToolStripMenuItem.Name = "指北针ToolStripMenuItem";
            this.指北针ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.指北针ToolStripMenuItem.Text = "指北针";
            // 
            // 选择ToolStripMenuItem
            // 
            this.选择ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.按属性选择ToolStripMenuItem,
            this.清除所选要素ToolStripMenuItem});
            this.选择ToolStripMenuItem.Name = "选择ToolStripMenuItem";
            this.选择ToolStripMenuItem.Size = new System.Drawing.Size(44, 26);
            this.选择ToolStripMenuItem.Text = "选择";
            // 
            // 按属性选择ToolStripMenuItem
            // 
            this.按属性选择ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("按属性选择ToolStripMenuItem.Image")));
            this.按属性选择ToolStripMenuItem.Name = "按属性选择ToolStripMenuItem";
            this.按属性选择ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.按属性选择ToolStripMenuItem.Text = "按属性选择";
            // 
            // 清除所选要素ToolStripMenuItem
            // 
            this.清除所选要素ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("清除所选要素ToolStripMenuItem.Image")));
            this.清除所选要素ToolStripMenuItem.Name = "清除所选要素ToolStripMenuItem";
            this.清除所选要素ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.清除所选要素ToolStripMenuItem.Text = "清除所选要素";
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(44, 26);
            this.帮助ToolStripMenuItem.Text = "帮助";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddLayer,
            this.Save,
            this.toolStripSeparator1,
            this.ZoomIn,
            this.ZoomOut,
            this.Pan,
            this.FullExtent,
            this.SelectFeatures,
            this.ClearSelectedFeatures});
            this.toolStrip1.Location = new System.Drawing.Point(0, 31);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(355, 30);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // AddLayer
            // 
            this.AddLayer.AutoSize = false;
            this.AddLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddLayer.Image = ((System.Drawing.Image)(resources.GetObject("AddLayer.Image")));
            this.AddLayer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddLayer.Name = "AddLayer";
            this.AddLayer.Size = new System.Drawing.Size(26, 20);
            this.AddLayer.Text = "打开（Crtl+O）";
            this.AddLayer.Click += new System.EventHandler(this.AddLayer_Click);
            // 
            // Save
            // 
            this.Save.AutoSize = false;
            this.Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Save.Image = ((System.Drawing.Image)(resources.GetObject("Save.Image")));
            this.Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(26, 20);
            this.Save.Text = "导出地图（Ctrl+S）";
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 30);
            // 
            // ZoomIn
            // 
            this.ZoomIn.AutoSize = false;
            this.ZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("ZoomIn.Image")));
            this.ZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ZoomIn.Name = "ZoomIn";
            this.ZoomIn.Size = new System.Drawing.Size(26, 20);
            this.ZoomIn.Text = "放大";
            this.ZoomIn.Click += new System.EventHandler(this.ZoomIn_Click);
            // 
            // ZoomOut
            // 
            this.ZoomOut.AutoSize = false;
            this.ZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("ZoomOut.Image")));
            this.ZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ZoomOut.Name = "ZoomOut";
            this.ZoomOut.Size = new System.Drawing.Size(26, 20);
            this.ZoomOut.Text = "缩小";
            this.ZoomOut.Click += new System.EventHandler(this.ZoomOut_Click);
            // 
            // Pan
            // 
            this.Pan.AutoSize = false;
            this.Pan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Pan.Image = ((System.Drawing.Image)(resources.GetObject("Pan.Image")));
            this.Pan.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Pan.Name = "Pan";
            this.Pan.Size = new System.Drawing.Size(26, 20);
            this.Pan.Text = "漫游";
            // 
            // FullExtent
            // 
            this.FullExtent.AutoSize = false;
            this.FullExtent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.FullExtent.Image = ((System.Drawing.Image)(resources.GetObject("FullExtent.Image")));
            this.FullExtent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FullExtent.Name = "FullExtent";
            this.FullExtent.Size = new System.Drawing.Size(26, 20);
            this.FullExtent.Text = "toolStripButton1";
            this.FullExtent.Click += new System.EventHandler(this.FullExtent_Click);
            // 
            // SelectFeatures
            // 
            this.SelectFeatures.AutoSize = false;
            this.SelectFeatures.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SelectFeatures.Image = ((System.Drawing.Image)(resources.GetObject("SelectFeatures.Image")));
            this.SelectFeatures.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SelectFeatures.Name = "SelectFeatures";
            this.SelectFeatures.Size = new System.Drawing.Size(26, 20);
            this.SelectFeatures.Text = "选择要素";
            // 
            // ClearSelectedFeatures
            // 
            this.ClearSelectedFeatures.AutoSize = false;
            this.ClearSelectedFeatures.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ClearSelectedFeatures.Image = ((System.Drawing.Image)(resources.GetObject("ClearSelectedFeatures.Image")));
            this.ClearSelectedFeatures.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ClearSelectedFeatures.Name = "ClearSelectedFeatures";
            this.ClearSelectedFeatures.Size = new System.Drawing.Size(26, 20);
            this.ClearSelectedFeatures.Text = "清除所选要素";
            // 
            // toolStrip2
            // 
            this.toolStrip2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip2.AutoSize = false;
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EditSpBtn,
            this.MoveFeatureBtn,
            this.EditPointBtn,
            this.CreateFeatureBtn,
            this.SelectLayer,
            this.btnIdentify});
            this.toolStrip2.Location = new System.Drawing.Point(275, 31);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(971, 30);
            this.toolStrip2.TabIndex = 4;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // EditSpBtn
            // 
            this.EditSpBtn.AutoSize = false;
            this.EditSpBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BeginEditItem,
            this.EndEditItem,
            this.SaveEditItem});
            this.EditSpBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EditSpBtn.Name = "EditSpBtn";
            this.EditSpBtn.Size = new System.Drawing.Size(66, 27);
            this.EditSpBtn.Text = "编辑器";
            // 
            // BeginEditItem
            // 
            this.BeginEditItem.Name = "BeginEditItem";
            this.BeginEditItem.Size = new System.Drawing.Size(180, 22);
            this.BeginEditItem.Text = "开始编辑";
            // 
            // EndEditItem
            // 
            this.EndEditItem.Name = "EndEditItem";
            this.EndEditItem.Size = new System.Drawing.Size(180, 22);
            this.EndEditItem.Text = "停止编辑";
            // 
            // MoveFeatureBtn
            // 
            this.MoveFeatureBtn.AutoSize = false;
            this.MoveFeatureBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MoveFeatureBtn.Image = ((System.Drawing.Image)(resources.GetObject("MoveFeatureBtn.Image")));
            this.MoveFeatureBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MoveFeatureBtn.Name = "MoveFeatureBtn";
            this.MoveFeatureBtn.Size = new System.Drawing.Size(26, 20);
            this.MoveFeatureBtn.Text = "编辑工具";
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.CheckBoxes = true;
            this.treeView1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeView1.Location = new System.Drawing.Point(9, 64);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(259, 635);
            this.treeView1.TabIndex = 5;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // moMapControl1
            // 
            this.moMapControl1.BackColor = System.Drawing.Color.White;
            this.moMapControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.moMapControl1.FlashColor = System.Drawing.Color.Green;
            this.moMapControl1.Layers = moLayers1;
            this.moMapControl1.Location = new System.Drawing.Point(275, 64);
            this.moMapControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.moMapControl1.Name = "moMapControl1";
            this.moMapControl1.SelectionColor = System.Drawing.Color.Cyan;
            this.moMapControl1.Size = new System.Drawing.Size(971, 635);
            this.moMapControl1.TabIndex = 6;
            this.moMapControl1.MapScaleChanged += new MyMapObjects.moMapControl.MapScaleChangedHandle(this.moMapControl1_MapScaleChanged);
            this.moMapControl1.AfterTrackingLayerDraw += new MyMapObjects.moMapControl.AfterTrackingLayerDrawHandle(this.moMapControl1_AfterTrackingLayerDraw);
            this.moMapControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.moMapControl1_MouseClick);
            this.moMapControl1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.moMapControl1_MouseDoubleClick);
            this.moMapControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.moMapControl1_MouseDown);
            this.moMapControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.moMapControl1_MouseMove);
            this.moMapControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.moMapControl1_MouseUp);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssCoordinate,
            this.tssMapScale});
            this.statusStrip1.Location = new System.Drawing.Point(0, 707);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1264, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tssCoordinate
            // 
            this.tssCoordinate.AutoSize = false;
            this.tssCoordinate.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssCoordinate.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tssCoordinate.Name = "tssCoordinate";
            this.tssCoordinate.Size = new System.Drawing.Size(200, 17);
            // 
            // tssMapScale
            // 
            this.tssMapScale.AutoSize = false;
            this.tssMapScale.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssMapScale.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tssMapScale.Name = "tssMapScale";
            this.tssMapScale.Size = new System.Drawing.Size(200, 17);
            // 
            // treeView_RigthMenu
            // 
            this.treeView_RigthMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.移除ToolStripMenuItem,
            this.上移至顶层ToolStripMenuItem,
            this.上移一层ToolStripMenuItem,
            this.下一一层ToolStripMenuItem,
            this.下移至底层ToolStripMenuItem,
            this.缩放至图层ToolStripMenuItem,
            this.打开属性表ToolStripMenuItem,
            this.另存为SPGLToolStripMenuItem,
            this.图层渲染ToolStripMenuItem,
            this.显示注记ToolStripMenuItem});
            this.treeView_RigthMenu.Name = "contextMenuStrip1";
            this.treeView_RigthMenu.Size = new System.Drawing.Size(181, 246);
            // 
            // 移除ToolStripMenuItem
            // 
            this.移除ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("移除ToolStripMenuItem.Image")));
            this.移除ToolStripMenuItem.Name = "移除ToolStripMenuItem";
            this.移除ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.移除ToolStripMenuItem.Text = "移除";
            // 
            // 上移至顶层ToolStripMenuItem
            // 
            this.上移至顶层ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("上移至顶层ToolStripMenuItem.Image")));
            this.上移至顶层ToolStripMenuItem.Name = "上移至顶层ToolStripMenuItem";
            this.上移至顶层ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.上移至顶层ToolStripMenuItem.Text = "上移至顶层";
            // 
            // 上移一层ToolStripMenuItem
            // 
            this.上移一层ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("上移一层ToolStripMenuItem.Image")));
            this.上移一层ToolStripMenuItem.Name = "上移一层ToolStripMenuItem";
            this.上移一层ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.上移一层ToolStripMenuItem.Text = "上移一层";
            // 
            // 下一一层ToolStripMenuItem
            // 
            this.下一一层ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("下一一层ToolStripMenuItem.Image")));
            this.下一一层ToolStripMenuItem.Name = "下一一层ToolStripMenuItem";
            this.下一一层ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.下一一层ToolStripMenuItem.Text = "下移一层";
            // 
            // 下移至底层ToolStripMenuItem
            // 
            this.下移至底层ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("下移至底层ToolStripMenuItem.Image")));
            this.下移至底层ToolStripMenuItem.Name = "下移至底层ToolStripMenuItem";
            this.下移至底层ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.下移至底层ToolStripMenuItem.Text = "下移至底层";
            // 
            // 缩放至图层ToolStripMenuItem
            // 
            this.缩放至图层ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("缩放至图层ToolStripMenuItem.Image")));
            this.缩放至图层ToolStripMenuItem.Name = "缩放至图层ToolStripMenuItem";
            this.缩放至图层ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.缩放至图层ToolStripMenuItem.Text = "缩放至图层";
            // 
            // 打开属性表ToolStripMenuItem
            // 
            this.打开属性表ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("打开属性表ToolStripMenuItem.Image")));
            this.打开属性表ToolStripMenuItem.Name = "打开属性表ToolStripMenuItem";
            this.打开属性表ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.打开属性表ToolStripMenuItem.Text = "打开属性表";
            // 
            // 另存为SPGLToolStripMenuItem
            // 
            this.另存为SPGLToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("另存为SPGLToolStripMenuItem.Image")));
            this.另存为SPGLToolStripMenuItem.Name = "另存为SPGLToolStripMenuItem";
            this.另存为SPGLToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.另存为SPGLToolStripMenuItem.Text = "另存为SPGL";
            // 
            // EditPointBtn
            // 
            this.EditPointBtn.AutoSize = false;
            this.EditPointBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.EditPointBtn.Image = ((System.Drawing.Image)(resources.GetObject("EditPointBtn.Image")));
            this.EditPointBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EditPointBtn.Name = "EditPointBtn";
            this.EditPointBtn.Size = new System.Drawing.Size(26, 20);
            this.EditPointBtn.Text = "toolStripButton1";
            // 
            // CreateFeatureBtn
            // 
            this.CreateFeatureBtn.AutoSize = false;
            this.CreateFeatureBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CreateFeatureBtn.Image = ((System.Drawing.Image)(resources.GetObject("CreateFeatureBtn.Image")));
            this.CreateFeatureBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CreateFeatureBtn.Name = "CreateFeatureBtn";
            this.CreateFeatureBtn.Size = new System.Drawing.Size(26, 20);
            this.CreateFeatureBtn.Text = "toolStripButton2";
            // 
            // SelectLayer
            // 
            this.SelectLayer.Name = "SelectLayer";
            this.SelectLayer.Size = new System.Drawing.Size(121, 30);
            // 
            // btnIdentify
            // 
            this.btnIdentify.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnIdentify.Image = ((System.Drawing.Image)(resources.GetObject("btnIdentify.Image")));
            this.btnIdentify.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnIdentify.Name = "btnIdentify";
            this.btnIdentify.Size = new System.Drawing.Size(23, 27);
            this.btnIdentify.Text = "toolStripButton3";
            // 
            // SaveEditItem
            // 
            this.SaveEditItem.Name = "SaveEditItem";
            this.SaveEditItem.Size = new System.Drawing.Size(180, 22);
            this.SaveEditItem.Text = "保存编辑";
            // 
            // moMapRightMenu
            // 
            this.moMapRightMenu.Name = "moMapRightMenu";
            this.moMapRightMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // 图层渲染ToolStripMenuItem
            // 
            this.图层渲染ToolStripMenuItem.Name = "图层渲染ToolStripMenuItem";
            this.图层渲染ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.图层渲染ToolStripMenuItem.Text = "图层渲染";
            // 
            // 显示注记ToolStripMenuItem
            // 
            this.显示注记ToolStripMenuItem.Name = "显示注记ToolStripMenuItem";
            this.显示注记ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.显示注记ToolStripMenuItem.Text = "显示注记";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 729);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.moMapControl1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "SimpleGIS";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.treeView_RigthMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton AddLayer;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton Save;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton ZoomIn;
        private System.Windows.Forms.ToolStripButton ZoomOut;
        private System.Windows.Forms.ToolStripButton Pan;
        private System.Windows.Forms.ToolStripButton FullExtent;
        private System.Windows.Forms.ToolStripButton SelectFeatures;
        private System.Windows.Forms.ToolStripButton ClearSelectedFeatures;
        private System.Windows.Forms.ToolStripSplitButton EditSpBtn;
        private System.Windows.Forms.ToolStripButton MoveFeatureBtn;
        private System.Windows.Forms.ToolStripMenuItem 新建ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导出地图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 编辑ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 视图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 数据视图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 布局视图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 插入ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 选择ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 标题ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 文本ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 内图廓线ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 图例ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 比例尺ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 指北针ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 按属性选择ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清除所选要素ToolStripMenuItem;
        private System.Windows.Forms.TreeView treeView1;
        private MyMapObjects.moMapControl moMapControl1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tssCoordinate;
        private System.Windows.Forms.ToolStripStatusLabel tssMapScale;
        private System.Windows.Forms.ContextMenuStrip treeView_RigthMenu;
        private System.Windows.Forms.ToolStripMenuItem 移除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 上移至顶层ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 上移一层ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 下一一层ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 下移至底层ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 缩放至图层ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开属性表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 另存为SPGLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem BeginEditItem;
        private System.Windows.Forms.ToolStripMenuItem EndEditItem;
        private System.Windows.Forms.ToolStripButton EditPointBtn;
        private System.Windows.Forms.ToolStripButton CreateFeatureBtn;
        private System.Windows.Forms.ToolStripComboBox SelectLayer;
        private System.Windows.Forms.ToolStripButton btnIdentify;
        private System.Windows.Forms.ToolStripMenuItem SaveEditItem;
        private System.Windows.Forms.ToolStripMenuItem 图层渲染ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 显示注记ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip moMapRightMenu;
    }
}

