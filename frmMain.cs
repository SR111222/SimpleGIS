﻿using MyMapObjects;
using SimpleGIS.DataIOTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimpleGIS
{
    public partial class frmMain : Form
    {
        #region 字段

        //（1）选项变量
        private Color mZoomBoxColor = Color.DeepPink;   //缩放盒颜色
        private double mZoomBoxWidth = 0.5;     //缩放盒的边界宽度，单位毫米
        private Color mSelectBoxColor = Color.DarkGreen;        //选择盒颜色
        private double mSelectBoxWidth = 0.5;       //选择盒的边界宽度
        private double mZoomRatioFixed = 2;     //固定放大系数
        private double mZoomRatioMouseWheel = 1.2;      //滑轮放大系数
        private double mSelectingTolerance = 3;     //选择容限，单位像素
        private MyMapObjects.moSimpleFillSymbol mSelectingBoxSymbol;        //选择盒的符号
        private MyMapObjects.moSimpleFillSymbol mZoomBoxSymbol;     //缩放盒的符号
        private MyMapObjects.moSimpleLineSymbol mMovingPolylineSymbol;
        private MyMapObjects.moSimpleMarkerSymbol mMovingPointSymbol;
        private MyMapObjects.moSimpleFillSymbol mMovingPolygonSymbol;       //正在移动的多边形的符号
        private MyMapObjects.moSimpleFillSymbol mEditingPolygonSymbol;      //正在编辑的多边形的符号
        private MyMapObjects.moSimpleLineSymbol mEditingPolylineSymbol;
        private MyMapObjects.moSimpleMarkerSymbol mEditingVertexSymbol;     //正在编辑的图形的顶点的符号
        private MyMapObjects.moSimpleLineSymbol mElasticSymbol;         //橡皮筋符号
        private bool mShowLngLat = false;       //指示是否显示经纬度

        //（2）与地图操作有关的变量
        /// <summary>
        /// 一般设置为枚举类型。0，无， 1，放大， 2，缩小， 3，漫游， 4，选择， 5，识别， 6，编辑（选择+移动）， 7，描绘要素， 8，编辑节点
        /// </summary>
        private Int32 mMapOpStyle = 0;
        private PointF mStartMouseLocation;        //用户拉框时，首先按下的鼠标位置
        private bool mIsInZoom = false;     //当前是否正处于缩放的过程中
        private bool mIsInPan = false;      //当前是否处于漫游的过程中
        private bool mIsInSelect = false;       //当前是否处于选择中
        private bool mIsInIdentify = false;     //是否处于查询中
        private bool mIsInMovingShapes = false;     //是否处于移动图形的过程中
        private List<MyMapObjects.moGeometry> mMovingGeometries = new List<MyMapObjects.moGeometry>();  //正在移动的图形集合
        private MyMapObjects.moGeometry mEditingGeometry;       //正在编辑的图形
        private List<MyMapObjects.moPoints> mSketchingShape;        //正在描绘的图形，用一个多点集合存储


        
        /// ///////////////////////////////////////////////////////////////////
        

        private Int32 mLastOpLayerIndex = -1;   //最近一次操作的图层索引
        private Int32 mOperatingLayerIndex  //当前操作的图层的索引
        {
            get { return GetOpLayerIndex(); }
        }
        private bool mIsInMove = false;
        private Int32 mMouseOnPartIndex = -1;   //鼠标位于多边形部件的索引
        private bool mIsInEditPoint = false;    //编辑节点状态
        private Int32 mMouseOnPointIndex = -1;
        private bool mIsInMovePoint
        {
            get { return MovPointBtn.Checked; }
        }
        private bool mIsInAddPoint
        {
            get { return AddPointBtn.Checked; }
        }
        private bool mIsInDeletePoint
        {
            get { return DelPointBtn.Checked; }
        }
        //撤销节点编辑所需变量
        private List<Int32> mEditPointOperation = new List<Int32>();
        private List<MyMapObjects.moPoint> mEditPointRecord = new List<MyMapObjects.moPoint>();
        private List<Int32> mPastPartIndex = new List<Int32>();
        private List<Int32> mPastPointIndex = new List<Int32>();

        private bool mNeedToSave = false;   //是否需要保存数据
        private bool mSelectedIsMoved = false;  //选择的图形是否被移动
        private bool mPointEditNeedSave = false;//对节点的编辑是否被保存
        private List<MyMapObjects.moPoint> mSketchingPoint; //正在描绘的点
        private bool mEditMoMap = false;
        private List<Int32> mLayerIndex = new List<Int32>();
        private List<MyMapObjects.moGeometry> mCopyingGeometries = new List<MyMapObjects.moGeometry>();

        public List<Form_table> All_tables = new List<Form_table>();//所有属性表的集合
        public int Fid_table_windows = 0;
        private bool mIsInRenderer = false;     //当前是否处于图层渲染中


        //（3）与文件操作相关的变量
        public List<DataIOTools.spglShpFileManager> mSPGLShapeFiles = new List<DataIOTools.spglShpFileManager>();   //管理要素文件
        public List<DataIOTools.dbfFileManager> mDBFFiles = new List<DataIOTools.dbfFileManager>();     //管理属性文件

        //(4）与图层渲染有关的变量

        private Int32 mPointRendererMode = 0; //渲染方式,0:简单渲染,1:唯一值渲染,2:分级渲染
        private Int32 mPointSymbolStyle = 0; //样式索引
        private Color mPointSimpleRendererColor = Color.Red; //符号颜色
        private Double mPointSimpleRendererSize = 5; //符号尺寸
        private Int32 mPointUniqueFieldIndex = 0; //绑定字段索引
        private Double mPointUniqueRendererSize = 5; //符号尺寸
        private Int32 mPointClassBreaksFieldIndex = 0; //绑定字段索引
        private Int32 mPointClassBreaksNum = 5; //分类数
        private Color mPointClassBreaksRendererColor = Color.Red; //符号颜色
        private Double mPointClassBreaksRendererMinSize = 3; //符号起始尺寸,点图层采用符号尺寸进行分级表示
        private Double mPointClassBreaksRendererMaxSize = 6; //符号终止尺寸

        private Int32 mPolylineRendererMode = 0; //渲染方式,0:简单渲染,1:唯一值渲染,2:分级渲染
        private Int32 mPolylineSymbolStyle = 0; //样式索引
        private Color mPolylineSimpleRendererColor = Color.Red; //符号颜色
        private Double mPolylineSimpleRendererSize = 0.5; //符号尺寸
        private Int32 mPolylineUniqueFieldIndex = 0; //绑定字段索引
        private Double mPolylineUniqueRendererSize = 0.5; //符号尺寸
        private Int32 mPolylineClassBreaksFieldIndex = 0; //绑定字段索引
        private Int32 mPolylineClassBreaksNum = 5; //分类数
        private Color mPolylineClassBreaksRendererColor = Color.Red; //符号颜色
        private Double mPolylineClassBreaksRendererMinSize = 0.5; //符号起始尺寸,线图层采用符号尺寸进行分级表示
        private Double mPolylineClassBreaksRendererMaxSize = 1.5; //符号终止尺寸

        private Int32 mPolygonRendererMode = 0; //渲染方式,0:简单渲染,1:唯一值渲染,2:分级渲染
        private Color mPolygonSimpleRendererColor = Color.Red; //符号颜色
        private Color mOutlineColor = Color.Red;    //描边颜色
        private Int32 mPolygonUniqueFieldIndex = 0; //绑定字段索引
        private Int32 mPolygonClassBreaksFieldIndex = 0; //绑定字段索引
        private Int32 mPolygonClassBreaksNum = 5; //分类数
        private Color mPolygonClassBreaksRendererStartColor = Color.MistyRose; //符号起始颜色,面图层采用符号颜色进行分级表示
        private Color mPolygonClassBreaksRendererEndColor = Color.Red; //符号终止颜色

        //(5)与注记显示有关的变量
        private Color mLabelColor = Color.Black;
        private Font mLabelFont = new Font("宋体", 12);
        private Int32 mLabelFieldIndex = 0;
        private bool mLabelUseMask = false;
        private bool mLabelVisible = false;

        //初始化渲染窗口和注记窗口
        Form_PointRenderer mFormPointRenderer;
        Form_PolylineRenderer mFormPolylineRenderer;
        Form_PolygonRenderer mFormPolygonRenderer;
        Form_Label mFormLabel;

        #endregion

        #region 构造函数

        public frmMain()
        {
            InitializeComponent();
            moMapControl1.MouseWheel += MoMap_MouseWheel;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //(1)初始化符号
            InitializeSymbols();
            //(2)初始化描绘图形
            InitializeSketchingShape();
            //(3)显示比例尺
            ShowMapScale();
        }

        //显示当前比例尺
        private void ShowMapScale()
        {
            tssMapScale.Text = "1:" + moMapControl1.MapScale.ToString("0.00");
        }

        #endregion

        #region 菜单栏(menuStrip)

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_CreateLayer newlayer = new Form_CreateLayer();
            newlayer.Owner = this;
            newlayer.ShowDialog();
        }

        private void 添加数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog sDialog = new OpenFileDialog();
            sDialog.Filter = "(.shp)|*.shp|(*.spgl)|*.spgl";
            string sFileName = "";
            if (sDialog.ShowDialog(this) == DialogResult.OK)
            {
                sFileName = sDialog.FileName;
                sDialog.Dispose();
            }
            else
            {
                sDialog.Dispose();
                return;
            }
            try
            {
                //检查是否存在同名图层
                string layerName = Path.GetFileNameWithoutExtension(sFileName);
                for (int i = 0; i < moMapControl1.Layers.Count; i++)
                {
                    if (layerName == moMapControl1.Layers.GetItem(i).Name)
                    {
                        string errorMsg = "已存在同名图层！";
                        throw new Exception(errorMsg);
                    }
                }
                string extension = Path.GetExtension(sFileName);
                string dbfFilePath = "";
                DataIOTools.spglShpFileManager sSPGLShpFileManager;
                List<MyMapObjects.moGeometry> sGeometries;
                MyMapObjects.moGeometryTypeConstant sGeometryType;
                if (extension == ".shp")
                {
                    //读取shp文件
                    string shpFilePath = sFileName;
                    dbfFilePath = shpFilePath.Substring(0, shpFilePath.IndexOf(".shp")) + ".dbf";
                    //（1)读取shp文件，并以spgl文件进行管理
                    DataIOTools.shpFileReader sShpFileReader = new DataIOTools.shpFileReader(shpFilePath);
                    sGeometryType = sShpFileReader.ShapeType;
                    sGeometries = sShpFileReader.Geometries;
                    sSPGLShpFileManager = new DataIOTools.spglShpFileManager(sGeometryType);
                    sSPGLShpFileManager.SourceFileType = "shp";     //设置文件源
                    sSPGLShpFileManager.DefaultFilePath = shpFilePath;
                    sSPGLShpFileManager.UpdateGeometries(sGeometries);
                }
                else
                {
                    //读取spgl文件
                    string spglFilePath = sFileName;
                    dbfFilePath = spglFilePath.Substring(0, spglFilePath.IndexOf(".spgl")) + ".spgdbf";
                    //(1)读取spgl文件
                    sSPGLShpFileManager = new DataIOTools.spglShpFileManager(spglFilePath);
                    sSPGLShpFileManager.SourceFileType = "spgl";
                    sSPGLShpFileManager.DefaultFilePath = spglFilePath;
                    sGeometries = sSPGLShpFileManager.Geometries;
                    sGeometryType = sSPGLShpFileManager.GeometryType;
                }

                //(2)读取dbf文件
                DataIOTools.dbfFileManager sDBFFileManager = new DataIOTools.dbfFileManager(dbfFilePath);
                MyMapObjects.moFields sFields = sDBFFileManager.Fields;
                List<MyMapObjects.moAttributes> sAttributes = sDBFFileManager.AttributesList;
                //(3)判断要素数与属性数是否一致
                if (sGeometries.Count != sAttributes.Count)
                {
                    string errorMsg = "要素数与属性数不一致！";
                    throw new Exception(errorMsg);
                }
                //(4)添加至图层并加载
                MyMapObjects.moMapLayer sMapLayer = new MyMapObjects.moMapLayer(layerName, sGeometryType, sFields);
                //加载要素
                MyMapObjects.moFeatures sFeatures = new MyMapObjects.moFeatures();
                for (int i = 0; i < sGeometries.Count; i++)
                {
                    MyMapObjects.moFeature sFeature = new MyMapObjects.moFeature(sGeometryType, sGeometries[i], sAttributes[i]);
                    sFeatures.Add(sFeature);
                }
                sMapLayer.Features = sFeatures;
                ThingsAfterNewLayer(sMapLayer, sSPGLShpFileManager, sDBFFileManager);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
                return;
            }
        }

        private void 导出地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (moMapControl1.Layers.Count == 0) MessageBox.Show("请先导入图层");
            else
            {
                SaveFileDialog savefile = new SaveFileDialog();
                savefile.OverwritePrompt = true;
                savefile.RestoreDirectory = true;
                savefile.Title = "保存BMP图片";
                savefile.Filter = "BMP文件(*.bmp)|*.bmp";
                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    string filename = savefile.FileName;
                    moMapControl1.BmpMap.Save(filename, System.Drawing.Imaging.ImageFormat.Bmp);
                }
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 按属性选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Select newSelect = new Form_Select(this);
            newSelect.ShowDialog();
        }

        private void 清除所选要素ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < moMapControl1.Layers.Count; i++)
            {
                moMapControl1.Layers.GetItem(i).SelectedFeatures.Clear();
            }
            moMapControl1.RedrawTrackingShapes();
            for (int i = 0; i < All_tables.Count; i++) 
            {
                All_tables[i].Refresh_dataform_select();
            }
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = "";
            str += "GIS设计与应用第七组：\n\n";
            str += "黄儒豪\n";
            str += "秦昊\n";
            str += "宋瑞\n";
            str += "谢争彦\n";
            MessageBox.Show(str, "开发人员");
        }

        #endregion

        #region 工具栏(toolStrip)
        //添加图层
        private void AddLayer_Click(object sender, EventArgs e)
        {
            添加数据ToolStripMenuItem_Click(sender, e);
        }

        //导出地图
        private void Save_Click(object sender, EventArgs e)
        {
            导出地图ToolStripMenuItem_Click(sender, e);
        }

        //放大
        private void ZoomIn_Click(object sender, EventArgs e)
        {
            mMapOpStyle = 1;
            this.Cursor = new Cursor("ico/ZoomIn.ico");
        }

        //缩小
        private void ZoomOut_Click(object sender, EventArgs e)
        {
            mMapOpStyle = 2;
            this.Cursor = new Cursor("ico/ZoomOut.ico");
        }

        //漫游
        private void Pan_Click(object sender, EventArgs e)
        {
            mMapOpStyle = 3;
            this.Cursor = new Cursor("ico/PanUp.ico");
        }

        //缩放至全图
        private void FullExtent_Click(object sender, EventArgs e)
        {
            moMapControl1.FullExtent();
        }

        //选择
        private void SelectFeatures_Click(object sender, EventArgs e)
        {
            if (mOperatingLayerIndex == -1)
            {
                MessageBox.Show("请选中图层后,再进行选择要素操作!");
                return;
            }
            mMapOpStyle = 4;
            //this.Cursor = new Cursor("ico/EditSelect.ico");
        }

        //清除选择
        private void ClearSelectedFeatures_Click(object sender, EventArgs e)
        {
            清除所选要素ToolStripMenuItem_Click(sender, e);
        }

        #endregion

        #region 图层栏
        //点击后切换图层是否可显
        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            moMapControl1.Layers.GetItem(e.Node.Index).Visible = !moMapControl1.Layers.GetItem(e.Node.Index).Visible;
            moMapControl1.RedrawMap();
        }

        //右击显示功能菜单
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                treeView_RigthMenu.Show(Control.MousePosition);
            }
        }

        //单击选择图层
        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            Point clickPoint = new Point(e.X, e.Y);
            TreeNode currentNode = treeView1.GetNodeAt(clickPoint);
            if (currentNode != null)
            {
                treeView1.SelectedNode = currentNode;
                mLastOpLayerIndex = currentNode.Index;  //鼠标单击或右键菜单对应的图层索引
            }
            else
            {
                if (e.Button == MouseButtons.Right)
                {
                    mLastOpLayerIndex = -1;
                }
            }
        }

        //双击选择图层
        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            mLastOpLayerIndex = e.Node.Index;
        }

        #endregion 

        #region 读写、新建图层

        //接受新建图层的参数
        public void GetCreateLayerInfo(string layerName,moGeometryTypeConstant layerType,string savePath)
        {
            string sSPGLFilePath = savePath;    //用户输入的存储路径（包含文件名，以.spgl结尾）
            if (sSPGLFilePath.IndexOf(".spgl") == -1) sSPGLFilePath += ".spgl";
            MyMapObjects.moGeometryTypeConstant sGeometryType = layerType;  //用户指定的图层要素类型
            string sSPGDBFFilePath = sSPGLFilePath.Substring(0, sSPGLFilePath.IndexOf(".spgl")) + ".spgdbf";
            //初始化文件读取类
            DataIOTools.spglShpFileManager sSPGLShpFileManager = new DataIOTools.spglShpFileManager(sGeometryType);
            sSPGLShpFileManager.SourceFileType = "spgl";
            sSPGLShpFileManager.DefaultFilePath = sSPGLFilePath;
            sSPGLShpFileManager.SaveToFile(sSPGLShpFileManager.DefaultFilePath);
            DataIOTools.dbfFileManager sDBFFileManager = new DataIOTools.dbfFileManager();
            sDBFFileManager.DefaultPath = sSPGDBFFilePath;
            MyMapObjects.moField sField = new MyMapObjects.moField("id", MyMapObjects.moValueTypeConstant.dInt32);  //为用户添加id字段
            sDBFFileManager.CreateField(sField, new MyMapObjects.moAttributes());
            sDBFFileManager.SaveToFile(sDBFFileManager.DefaultPath);
            //添加至图层并加载
            MyMapObjects.moFields sFields = new MyMapObjects.moFields();
            sFields.Append(sField);
            sFields.PrimaryField = sField.Name;
            MyMapObjects.moMapLayer sMapLayer = new MyMapObjects.moMapLayer(layerName, sGeometryType, sFields);
            //加载要素
            MyMapObjects.moFeatures sFeatures = new MyMapObjects.moFeatures();
            sMapLayer.Features = sFeatures;

            ThingsAfterNewLayer(sMapLayer, sSPGLShpFileManager, sDBFFileManager);
        }


        

        //添加新图层后触发事件
        private void ThingsAfterNewLayer(MyMapObjects.moMapLayer sMapLayer, DataIOTools.spglShpFileManager sSPGLShpFileManager,DataIOTools.dbfFileManager sDBFFileManager)
        {
            //相关数据更新
            int index = SearchInsertIndex(sMapLayer.ShapeType);
            mLastOpLayerIndex = index;
            moMapControl1.Layers.Insert(index, sMapLayer);
            mSPGLShapeFiles.Insert(index,sSPGLShpFileManager);
            mDBFFiles.Insert(index, sDBFFileManager);
            RefreshLayersTree();
            if(moMapControl1.Layers.Count == 1)
            {
                moMapControl1.FullExtent();
            }
            else
            {
                moMapControl1.RedrawMap();
            }

            /*
            moMapControl1.Layers.Add(sMapLayer);
            SortLayers();
            treeView1.Nodes.Clear();
            for (int i = 0; i < moMapControl1.Layers.Count; i++)
            {
                TreeNode node = new TreeNode();
                node.Text = moMapControl1.Layers.GetItem(i).Name;
                node.Checked = moMapControl1.Layers.GetItem(i).Visible;
                treeView1.Nodes.Add(node);
            }

            if (moMapControl1.Layers.Count == 1)
            {
                moMapControl1.FullExtent();
            }
            else
            {
                moMapControl1.RedrawMap();
            }
            */
        }

        //寻找新打开图层的插入位置
        private Int32 SearchInsertIndex(MyMapObjects.moGeometryTypeConstant shapeType)
        {
            Int32 index = 0;
            for (; index < moMapControl1.Layers.Count; index++)
            {
                if (shapeType < moMapControl1.Layers.GetItem(index).ShapeType)
                {
                    return index;
                }
            }
            return index;
        }

        //图层列表刷新
        private void RefreshLayersTree()
        {
            treeView1.Nodes.Clear();
            for (Int32 i = 0; i < moMapControl1.Layers.Count; i++)
            {
                TreeNode layerNode = new TreeNode();
                layerNode.Text = moMapControl1.Layers.GetItem(i).Name;
                layerNode.Checked = moMapControl1.Layers.GetItem(i).Visible;
                layerNode.ContextMenuStrip = treeView_RigthMenu;
                treeView1.Nodes.Add(layerNode);
            }
            treeView1.Refresh();
        }

        #endregion



        #region 编辑相关

        //编辑
        private void EditSpBtn_Click(object sender, EventArgs e)
        {
            if (moMapControl1.Layers.Count > 0)
            {
                BeginEditItem.Enabled = true;
            }
        }

        //开始编辑
        private void BeginEditItem_Click(object sender, EventArgs e)
        {
            EndEditItem.Enabled = true;
            SaveEditItem.Enabled = true;
            MoveFeatureBtn.Enabled = true;
            EditPointBtn.Enabled = true;
            CreateFeatureBtn.Enabled = true;
            SelectLayer.Enabled = true;
            RefreshSelectLayer();
            MoveFeatureBtn_Click(sender, e); // 开始编辑后。先默认为移动操作
            mNeedToSave = false;
            mEditMoMap = true;
        }
        //结束编辑
        private void EndEditItem_Click(object sender, EventArgs e)
        {
            if (mPointEditNeedSave)
            {
                SavePointEdit();
            }
            if (mNeedToSave)
            {
                DialogResult dr = MessageBox.Show("是否保存编辑内容", "Saving", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Cancel)
                {
                    return;
                }
                else if (dr == DialogResult.Yes)
                {
                    SaveEditItem_Click(sender, e);
                }
                else
                {
                    CancelEdit();
                }
            }
            MoveFeatureBtn.Enabled = false;
            EditPointBtn.Enabled = false;
            CreateFeatureBtn.Enabled = false;
            MoveFeatureBtn.Checked = false;
            EditPointBtn.Checked = false;
            CreateFeatureBtn.Checked = false;
            SelectLayer.SelectedIndex = -1;
            SelectLayer.Enabled = false;
            EndEditItem.Enabled = false;
            SaveEditItem.Enabled = false;
            mNeedToSave = false;
            mMapOpStyle = 0;
            for (Int32 i = 0; i < moMapControl1.Layers.Count; i++)
            {
                moMapControl1.Layers.GetItem(i).SelectedFeatures.Clear();
                moMapControl1.Layers.GetItem(i).SelectIndex.Clear();
            }
            mMovingGeometries.Clear();
            moMapRightMenu.Items.Clear();
            InitializeSketchingShape();
            mEditingGeometry = null;
            mEditMoMap = false;
            moMapControl1.RedrawMap();
        }

        //编辑要素（移动+选择）
        private void MoveFeatureBtn_Click(object sender, EventArgs e)
        {
            if (mOperatingLayerIndex == -1) return;
            SavePointEdit();
            MoveFeatureBtn.Checked = true;
            EditPointBtn.Checked = false;
            CreateFeatureBtn.Checked = false;
            mMapOpStyle = 6;    //默认为编辑操作
            RightMenuInSelect();
        }
        //新建要素
        private void CreateFeatureBtn_Click(object sender, EventArgs e)
        {
            if (mOperatingLayerIndex == -1) return;
            SavePointEdit();
            MoveFeatureBtn.Checked = false;
            EditPointBtn.Checked = false;
            CreateFeatureBtn.Checked = true;
            mMapOpStyle = 7;
            RightMenuInSketch();
        }
        //保存编辑
        private void SaveEditItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (mPointEditNeedSave)
                {
                    SavePointEdit();
                }
                if (mNeedToSave)
                {
                    mNeedToSave = false;
                    SaveMapLayer(mOperatingLayerIndex);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
                return;
            }
        }

        private void SaveMapLayer(Int32 index)
        {
            //图形数据
            MyMapObjects.moMapLayer sLayer = moMapControl1.Layers.GetItem(index);
            mSPGLShapeFiles[index].Geometries.Clear();
            for (Int32 j = 0; j < sLayer.Features.Count; j++)
            {
                mSPGLShapeFiles[index].Geometries.Add(sLayer.Features.GetItem(j).Geometry);
            }
            mSPGLShapeFiles[index].UpdateGeometries(mSPGLShapeFiles[index].Geometries);
            string path = mSPGLShapeFiles[index].DefaultFilePath;
            mSPGLShapeFiles[index].SaveToFile(path);
            //属性数据
            mDBFFiles[index].Fields = sLayer.AttributeFields;
            mDBFFiles[index].AttributesList.Clear();
            for (Int32 j = 0; j < sLayer.Features.Count; j++)
            {
                mDBFFiles[index].AttributesList.Add(sLayer.Features.GetItem(j).Attributes);
            }
            mDBFFiles[index].UpdateAttributesList(mDBFFiles[index].AttributesList);
            path = mDBFFiles[index].DefaultPath;
            mDBFFiles[index].SaveToFile(path);
        }

        //编辑折点
        private void EditPointBtn_Click(object sender, EventArgs e)
        {
            if (EditPointBtn.Checked)
            {
                MoveFeatureBtn_Click(sender, e);
            }
            else
            {
                if (mOperatingLayerIndex == -1) return;
                MyMapObjects.moMapLayer sLayer = moMapControl1.Layers.GetItem(mOperatingLayerIndex);
                if (sLayer.SelectedFeatures.Count != 1)
                {
                    MessageBox.Show("请选择且仅选择一个可编辑的要素进行修改！");
                    return;
                }
                MoveFeatureBtn.Checked = false;
                EditPointBtn.Checked = true;
                CreateFeatureBtn.Checked = false;
                mMapOpStyle = 8;
                RightMenuInEdit();
                ShowEditStrip(sLayer.ShapeType);
                ShowEditGeometry();
            }
        }
        private void EditPointBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (EditPointBtn.Checked == false)
            {
                HideEditStrip();
            }
        }

        //移动节点
        private void MovePointBtn_Click(object sender, EventArgs e)
        {
            MovPointBtn.Checked = true;
            AddPointBtn.Checked = false;
            DelPointBtn.Checked = false;
        }
        //增加节点
        private void AddPointBtn_Click(object sender, EventArgs e)
        {
            MovPointBtn.Checked = false;
            AddPointBtn.Checked = true;
            DelPointBtn.Checked = false;
        }

        //删除节点
        private void DeletePointBtn_Click(object sender, EventArgs e)
        {
            MovPointBtn.Checked = false;
            AddPointBtn.Checked = false;
            DelPointBtn.Checked = true;
        }

        //选择操作图层
        private void SelectLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool needSave = false;
            if (mNeedToSave)
            {
                EndEditItem_Click(sender, e);
                needSave = true;
            }
            if (SelectLayer.SelectedIndex == -1)
            {
                SelectLayer.DropDownStyle = ComboBoxStyle.DropDown;
                SelectLayer.Text = "请选择图层";
            }
            else
            {
                SelectLayer.DropDownStyle = ComboBoxStyle.DropDownList;
            }
            if (mOperatingLayerIndex != -1 && mSPGLShapeFiles[mOperatingLayerIndex].SourceFileType == "shp")
            {
                Int32 goOn = 1;
                //Int32 goOn = (Int32)MessageBox.Show("当前图层为shapefile文件，对其进行编辑操作，会在同一目录下生成同名gvshp文件，操作只会保存在该gvshp文件中！若已有同名gvshp文件，会进行覆盖，请知悉！是否继续？（这里将保存注释掉了，所以这句话目前不准确）","Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                if (goOn == 1)
                {
                    mSPGLShapeFiles[mOperatingLayerIndex].SourceFileType = "spgl";
                    string filePath = mSPGLShapeFiles[mOperatingLayerIndex].DefaultFilePath;
                    string sPath = filePath.Substring(0, filePath.IndexOf(".shp")) + ".spgl";
                    mSPGLShapeFiles[mOperatingLayerIndex].SaveToFile(sPath);
                    mSPGLShapeFiles[mOperatingLayerIndex].DefaultFilePath = sPath;
                    filePath = mDBFFiles[mOperatingLayerIndex].DefaultPath;
                    sPath = filePath.Substring(0, filePath.IndexOf(".dbf")) + ".spgdbf";
                    mDBFFiles[mOperatingLayerIndex].SaveToFile(sPath);
                    mDBFFiles[mOperatingLayerIndex].DefaultPath = sPath;
                }
                else
                {
                    SelectLayer.SelectedIndex = -1;
                }
            }
            if (needSave && mOperatingLayerIndex != -1)
            {
                BeginEditItem_Click(sender, e);
            }
        }

        //识别
        private void btnIdentify_Click(object sender, EventArgs e)
        {
            mMapOpStyle = 5;
        }
        #endregion


        #region moMapControl右击菜单

        private void moMapRightMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (mMapOpStyle == 6)
            {
                RightOperateInSelect(e);
            }
            else if (mMapOpStyle == 7)
            {
                RightOperateInSketch(e);
            }
            else if (mMapOpStyle == 8)
            {
                RightOperateInEditPoint(e);
            }
        }

        private void moMapRightMenu_VisibleChanged(object sender, EventArgs e)
        {
            if (mOperatingLayerIndex == -1) return;
            if (mMapOpStyle == 6)
            {
                MyMapObjects.moMapLayer sLayer = moMapControl1.Layers.GetItem(mOperatingLayerIndex);
                if (sLayer.SelectedFeatures.Count == 0)
                {
                    moMapRightMenu.Items[0].Enabled = false;
                    moMapRightMenu.Items[2].Enabled = false;
                    moMapRightMenu.Items[3].Enabled = false;
                }
                else
                {
                    moMapRightMenu.Items[0].Enabled = true;
                    moMapRightMenu.Items[2].Enabled = true;
                    moMapRightMenu.Items[3].Enabled = true;
                }
                if (mCopyingGeometries.Count == 0) moMapRightMenu.Items[1].Enabled = false;
                else moMapRightMenu.Items[1].Enabled = true;
            }
            if (mMapOpStyle == 7)
            {
                if (mSketchingPoint.Count == 0 && mSketchingShape.Count == 1 && mSketchingShape[0].Count == 0)
                {
                    for (Int32 i = 0; i < 5; i++) moMapRightMenu.Items[i].Enabled = false;
                }
                else
                {
                    for (Int32 i = 0; i < 5; i++) moMapRightMenu.Items[i].Enabled = true;
                }
            }
            if (mMapOpStyle == 8)
            {
                if (mEditPointOperation.Count == 0)
                {
                    moMapRightMenu.Items[1].Enabled = false;
                }
                else moMapRightMenu.Items[1].Enabled = true;
            }
        }

        #endregion

        #region 鼠标操作

        #region MouseDown

        private void moMapControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (mMapOpStyle == 1)
                {
                    //放大
                    OnZoomIn_MouseDown(e);
                }
                else if (mMapOpStyle == 2)
                {
                    //缩小
                    ;
                }
                else if (mMapOpStyle == 3)
                {
                    //漫游
                    OnPan_MouseDown(e);
                }
                else if (mMapOpStyle == 4)
                {
                    OnNoEditSelect_MouseDown(e);
                }
                else if (mMapOpStyle == 5)
                {
                    OnIdentify_MouseDown(e);
                }
                else if (mMapOpStyle == 6 && mOperatingLayerIndex != -1)
                {
                    OnEdit_MouseDown(e);
                }
                else if (mMapOpStyle == 7)
                {
                    ;
                }
                else if (mMapOpStyle == 8 && mOperatingLayerIndex != -1)
                {
                    OnEditPoint_MouseDown(e);
                }
                
            }
        }

        private void OnZoomIn_MouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mStartMouseLocation = e.Location;
                mIsInZoom = true;
                this.Cursor = new Cursor("ico/ZoomIn.ico");
            }
        }

        private void OnPan_MouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mStartMouseLocation = e.Location;
                mIsInPan = true;
                this.Cursor = new Cursor("ico/PanUp.ico");
            }
        }

        private void OnEdit_MouseDown(MouseEventArgs e)
        {
            //判断应该是进行选择还是移动
            mIsInMove = false;
            mIsInSelect = false;
            MyMapObjects.moPoint sPoint = moMapControl1.ToMapPoint(e.Location.X, e.Location.Y);
            if (moMapControl1.Layers.GetItem(mOperatingLayerIndex).ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPolygon)
            {
                for (Int32 i = 0; i < moMapControl1.Layers.GetItem(mOperatingLayerIndex).SelectedFeatures.Count; i++)
                {
                    MyMapObjects.moFeature sFeature = moMapControl1.Layers.GetItem(mOperatingLayerIndex).SelectedFeatures.GetItem(i);
                    MyMapObjects.moMultiPolygon sMultiPolygon = (MyMapObjects.moMultiPolygon)sFeature.Geometry;
                    if (MyMapObjects.moMapTools.IsPointWithinMultiPolygon(sPoint, sMultiPolygon))
                    {
                        mIsInMove = true;
                        break;
                    }
                }
            }
            else if (moMapControl1.Layers.GetItem(mOperatingLayerIndex).ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPolyline)
            {
                for (Int32 i = 0; i < moMapControl1.Layers.GetItem(mOperatingLayerIndex).SelectedFeatures.Count; i++)
                {
                    MyMapObjects.moFeature sFeature = moMapControl1.Layers.GetItem(mOperatingLayerIndex).SelectedFeatures.GetItem(i);
                    MyMapObjects.moMultiPolyline sMultiPolyline = (MyMapObjects.moMultiPolyline)sFeature.Geometry;
                    double sTolerance = moMapControl1.ToMapDistance(mSelectingTolerance);
                    if (MyMapObjects.moMapTools.IsPointOnMultiPolyline(sPoint, sMultiPolyline, sTolerance))
                    {
                        mIsInMove = true;
                        break;
                    }
                }
            }
            else if (moMapControl1.Layers.GetItem(mOperatingLayerIndex).ShapeType == MyMapObjects.moGeometryTypeConstant.Point)
            {
                for (Int32 i = 0; i < moMapControl1.Layers.GetItem(mOperatingLayerIndex).SelectedFeatures.Count; i++)
                {
                    MyMapObjects.moFeature sFeature = moMapControl1.Layers.GetItem(mOperatingLayerIndex).SelectedFeatures.GetItem(i);
                    MyMapObjects.moPoint sFeaturePoint = (MyMapObjects.moPoint)sFeature.Geometry;
                    double sTolerance = moMapControl1.ToMapDistance(mSelectingTolerance);
                    if (MyMapObjects.moMapTools.IsPointOnPoint(sPoint, sFeaturePoint, sTolerance))
                    {
                        mIsInMove = true;
                        break;
                    }
                }
            }
            else if (moMapControl1.Layers.GetItem(mOperatingLayerIndex).ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPoint)
            {
                for (Int32 i = 0; i < moMapControl1.Layers.GetItem(mOperatingLayerIndex).SelectedFeatures.Count; i++)
                {
                    MyMapObjects.moFeature sFeature = moMapControl1.Layers.GetItem(mOperatingLayerIndex).SelectedFeatures.GetItem(i);
                    MyMapObjects.moPoints sPoints = (MyMapObjects.moPoints)sFeature.Geometry;
                    double sTolerance = moMapControl1.ToMapDistance(mSelectingTolerance);
                    if (MyMapObjects.moMapTools.IsPointOnPolyline(sPoint, sPoints, sTolerance))
                    {
                        mIsInMove = true;
                        break;
                    }
                }
            }
            if (mIsInMove)
            {
                OnMoveSelect_MouseDown(e);
            }
            else
            {
                mIsInSelect = true;
                OnSelect_MouseDown(e);
            }
        }

        private void OnIdentify_MouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mStartMouseLocation = e.Location;
                mIsInIdentify = true;
            }
        }

        private void OnSelect_MouseDown(MouseEventArgs e)
        {
            mStartMouseLocation = e.Location;
        }
        private void OnMoveSelect_MouseDown(MouseEventArgs e)
        {
            MyMapObjects.moMapLayer sLayer = moMapControl1.Layers.GetItem(mOperatingLayerIndex);
            Int32 sSelFeatureCount = sLayer.SelectedFeatures.Count;
            if (sSelFeatureCount == 0) return;
            //复制图层
            mMovingGeometries.Clear();
            if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPolygon)
            {
                for (Int32 i = 0; i < sSelFeatureCount; ++i)
                {
                    MyMapObjects.moMultiPolygon sOriPolygon = (MyMapObjects.moMultiPolygon)sLayer.SelectedFeatures.GetItem(i).Geometry;
                    MyMapObjects.moMultiPolygon sDesPolygon = sOriPolygon.Clone();
                    mMovingGeometries.Add(sDesPolygon);
                }
            }
            else if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPolyline)
            {
                for (Int32 i = 0; i < sSelFeatureCount; ++i)
                {
                    MyMapObjects.moMultiPolyline sOriPolygon = (MyMapObjects.moMultiPolyline)sLayer.SelectedFeatures.GetItem(i).Geometry;
                    MyMapObjects.moMultiPolyline sDesPolygon = sOriPolygon.Clone();
                    mMovingGeometries.Add(sDesPolygon);
                }
            }
            else if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.Point)
            {
                for (Int32 i = 0; i < sSelFeatureCount; ++i)
                {
                    MyMapObjects.moPoint sOriPolygon = (MyMapObjects.moPoint)sLayer.SelectedFeatures.GetItem(i).Geometry;
                    MyMapObjects.moPoint sDesPolygon = sOriPolygon.Clone();
                    mMovingGeometries.Add(sDesPolygon);
                }
            }
            else if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPoint)
            {
                for (Int32 i = 0; i < sSelFeatureCount; ++i)
                {
                    MyMapObjects.moPoints sOriPolygon = (MyMapObjects.moPoints)sLayer.SelectedFeatures.GetItem(i).Geometry;
                    MyMapObjects.moPoints sDesPolygon = sOriPolygon.Clone();
                    mMovingGeometries.Add(sDesPolygon);
                }
            }
            //设置变量
            mStartMouseLocation = e.Location;
        }

        private void OnNoEditSelect_MouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mStartMouseLocation = e.Location;
                mIsInSelect = true;
            }
        }

        private void OnEditPoint_MouseDown(MouseEventArgs e)
        {
            if (mMouseOnPartIndex != -1 && mMouseOnPointIndex != -1)
            {
                mIsInEditPoint = true;
                if (mIsInMovePoint)
                {
                    mEditPointOperation.Add(1);
                    mEditPointRecord.Add(moMapControl1.ToMapPoint(e.Location.X, e.Location.Y));
                    mPastPartIndex.Add(mMouseOnPartIndex);
                    mPastPointIndex.Add(mMouseOnPointIndex);
                }
            }
        }

        #endregion


        #region MouseMove

        private void moMapControl1_MouseMove(object sender, MouseEventArgs e)
        {
            ShowCoordinates(e.Location);
            if (mMapOpStyle == 1)
            {
                OnZoomIn_MouseMove(e);
            }
            else if (mMapOpStyle == 2)
            {
                ;
            }
            else if (mMapOpStyle == 3)
            {
                OnPan_MouseMove(e);
            }
            else if (mMapOpStyle == 4 && mOperatingLayerIndex != -1)
            {
                OnSelect_MouseMove(e);
            }
            else if (mMapOpStyle == 5)
            {
                OnIdentify_MouseMove(e);
            }
            else if (mMapOpStyle == 6)
            {
                OnEdit_MouseMove(e);
            }
            else if (mMapOpStyle == 7)
            {
                OnSketch_MouseMove(e);
            }
            else if (mMapOpStyle == 8)
            {
                OnEditPoint_MouseMove(e);
            }
            
        }

        private void OnZoomIn_MouseMove(MouseEventArgs e)
        {
            if (mIsInZoom == false)
            {
                return;
            }
            moMapControl1.Refresh();
            MyMapObjects.moRectangle sRect = GetMapRectByTwoPoints(mStartMouseLocation, e.Location);
            MyMapObjects.moUserDrawingTool sDrawingTool = moMapControl1.GetDrawingTool();
            sDrawingTool.DrawRectangle(sRect, mZoomBoxSymbol);
            this.Cursor = new Cursor("ico/ZoomIn.ico");
        }

        private void OnPan_MouseMove(MouseEventArgs e)
        {
            if (mIsInPan == false)
                return;
            moMapControl1.PanMapImageTo(e.Location.X - mStartMouseLocation.X, e.Location.Y - mStartMouseLocation.Y);
            this.Cursor = new Cursor("ico/PanDown.ico");
        }

        private void OnSelect_MouseMove(MouseEventArgs e)
        {
            if (mIsInSelect == false)
            {
                return;
            }
            moMapControl1.Refresh();
            MyMapObjects.moRectangle sRect = GetMapRectByTwoPoints(mStartMouseLocation, e.Location);
            MyMapObjects.moUserDrawingTool sDrawingTool = moMapControl1.GetDrawingTool();
            sDrawingTool.DrawRectangle(sRect, mSelectingBoxSymbol);
        }

        private void OnIdentify_MouseMove(MouseEventArgs e)
        {
            if (mIsInIdentify == false)
            {
                return;
            }
            moMapControl1.Refresh();
            MyMapObjects.moRectangle sRect = GetMapRectByTwoPoints(mStartMouseLocation, e.Location);
            MyMapObjects.moUserDrawingTool sDrawingTool = moMapControl1.GetDrawingTool();
            sDrawingTool.DrawRectangle(sRect, mSelectingBoxSymbol);
        }


        private void OnEdit_MouseMove(MouseEventArgs e)
        {
            if (mIsInMove)
            {
                OnMoveSelect_MouseMove(e);
            }
            else if (mIsInSelect)
            {
                OnSelect_MouseMove(e);
            }
        }
        private void OnMoveSelect_MouseMove(MouseEventArgs e)
        {
            if (mIsInMove == false) return;
            //修改移动图形的坐标
            double sDeltaX = moMapControl1.ToMapDistance(e.Location.X - mStartMouseLocation.X);
            double sDeltaY = moMapControl1.ToMapDistance(mStartMouseLocation.Y - e.Location.Y);
            mNeedToSave = true;
            ModifyMovingGeometries(sDeltaX, sDeltaY);
            //刷新地图并绘制移动图形
            moMapControl1.Refresh();
            DrawMovingShape();
            mSelectedIsMoved = true;
            //重新设置鼠标位置
            mStartMouseLocation = e.Location;
        }
        private void OnSketch_MouseMove(MouseEventArgs e)
        {
            MyMapObjects.moMapLayer sMapLayer = moMapControl1.Layers.GetItem(mOperatingLayerIndex);
            MyMapObjects.moPoint sCurPoint = moMapControl1.ToMapPoint(e.Location.X, e.Location.Y);
            if (sMapLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPolygon)
            {
                MyMapObjects.moPoints sLastPart = mSketchingShape.Last();
                Int32 sPointCount = sLastPart.Count;
                if (sPointCount == 0)
                { }
                else if (sPointCount == 1)
                {
                    moMapControl1.Refresh();
                    //只有一个顶点，则绘制一条橡皮筋
                    MyMapObjects.moPoint sFirstPoint = sLastPart.GetItem(0);
                    MyMapObjects.moUserDrawingTool sDrawingTool = moMapControl1.GetDrawingTool();
                    sDrawingTool.DrawLine(sFirstPoint, sCurPoint, mElasticSymbol);
                }
                else
                {
                    moMapControl1.Refresh();
                    //两个以上顶点，则绘制两条橡皮筋
                    MyMapObjects.moPoint sFirstPoint = sLastPart.GetItem(0);
                    MyMapObjects.moPoint sLastPoint = sLastPart.GetItem(sPointCount - 1);
                    MyMapObjects.moUserDrawingTool sDrawingTool = moMapControl1.GetDrawingTool();
                    sDrawingTool.DrawLine(sFirstPoint, sCurPoint, mElasticSymbol);
                    sDrawingTool.DrawLine(sLastPoint, sCurPoint, mElasticSymbol);
                }
            }
            else if (sMapLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPolyline)
            {
                MyMapObjects.moPoints sLastPart = mSketchingShape.Last();
                Int32 sPointCount = sLastPart.Count;
                if (sPointCount == 0)
                { }
                else
                {
                    moMapControl1.Refresh();
                    MyMapObjects.moPoint sLastPoint = sLastPart.GetItem(sPointCount - 1);
                    MyMapObjects.moUserDrawingTool sDrawingTool = moMapControl1.GetDrawingTool();
                    sDrawingTool.DrawLine(sLastPoint, sCurPoint, mElasticSymbol);
                }
            }
            else if (sMapLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.Point)
            {
                ;
            }
            else if (sMapLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPoint)
            {
                ;
            }
        }
        private void OnEditPoint_MouseMove(MouseEventArgs e)
        {
            MyMapObjects.moGeometryTypeConstant shapeType = moMapControl1.Layers.GetItem(mOperatingLayerIndex).ShapeType;
            if (shapeType == MyMapObjects.moGeometryTypeConstant.MultiPolygon)
            {
                MultiPolygonEditing_MouseMove(e);
            }
            else if (shapeType == MyMapObjects.moGeometryTypeConstant.MultiPolyline)
            {
                MultiPolylineEditing_MouseMove(e);
            }
            else if (shapeType == MyMapObjects.moGeometryTypeConstant.Point)
            {
                PointEditing_MouseMove(e);
            }
            else if (shapeType == MyMapObjects.moGeometryTypeConstant.MultiPoint)
            {
                MultiPointEditing_MouseMove(e);
            }
        }
        private void MultiPolygonEditing_MouseMove(MouseEventArgs e)
        {
            if (mIsInEditPoint == false && mEditingGeometry != null)
            {
                MyMapObjects.moMultiPolygon sMultiPolygon = (MyMapObjects.moMultiPolygon)mEditingGeometry;
                double sTolerance = moMapControl1.ToMapDistance(mSelectingTolerance);
                MyMapObjects.moPoint sPoint = moMapControl1.ToMapPoint(e.Location.X, e.Location.Y);
                if (PointCloseToMultiPolygonPoint(sPoint, sMultiPolygon, sTolerance))
                {
                    if (mIsInMovePoint) this.Cursor = new Cursor("ico/EditMoveVertex.ico");
                    if (mIsInAddPoint) this.Cursor = new Cursor("ico/AddPoint.ico");
                    if (mIsInDeletePoint) this.Cursor = new Cursor("ico/DeletePoint.ico");
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    mMouseOnPartIndex = -1;
                    mMouseOnPointIndex = -1;
                }
            }
            else
            {
                MyMapObjects.moMultiPolygon sMultiPolygon = (MyMapObjects.moMultiPolygon)mEditingGeometry;
                MyMapObjects.moPoint sPoint = moMapControl1.ToMapPoint(e.Location.X, e.Location.Y);
                if (mIsInMovePoint)
                {
                    mPointEditNeedSave = true;
                    MyMapObjects.moPoint newPoint = sMultiPolygon.Parts.GetItem(mMouseOnPartIndex).GetItem(mMouseOnPointIndex);
                    newPoint.X = sPoint.X; newPoint.Y = sPoint.Y;
                    sMultiPolygon.UpdateExtent();
                    mEditingGeometry = sMultiPolygon;
                    moMapControl1.RedrawTrackingShapes();
                }
                if (mIsInAddPoint) mIsInEditPoint = false;
                if (mIsInDeletePoint) mIsInEditPoint = false;
            }
        }
        private void MultiPolylineEditing_MouseMove(MouseEventArgs e)
        {
            if (mIsInEditPoint == false && mEditingGeometry != null)
            {
                MyMapObjects.moMultiPolyline sMultiPolyline = (MyMapObjects.moMultiPolyline)mEditingGeometry;
                double sTolerance = moMapControl1.ToMapDistance(mSelectingTolerance);
                MyMapObjects.moPoint sPoint = moMapControl1.ToMapPoint(e.Location.X, e.Location.Y);
                if (PointCloseToMultiPolylinePoint(sPoint, sMultiPolyline, sTolerance))
                {
                    if (mIsInMovePoint) this.Cursor = new Cursor("ico/EditMoveVertex.ico");
                    if (mIsInAddPoint) this.Cursor = new Cursor("ico/AddPoint.ico");
                    if (mIsInDeletePoint) this.Cursor = new Cursor("ico/DeletePoint.ico");
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    mMouseOnPartIndex = -1;
                    mMouseOnPointIndex = -1;
                }
            }
            else
            {
                MyMapObjects.moMultiPolyline sMultiPolyline = (MyMapObjects.moMultiPolyline)mEditingGeometry;
                MyMapObjects.moPoint sPoint = moMapControl1.ToMapPoint(e.Location.X, e.Location.Y);
                if (mIsInMovePoint)
                {
                    mPointEditNeedSave = true;
                    MyMapObjects.moPoint newPoint = sMultiPolyline.Parts.GetItem(mMouseOnPartIndex).GetItem(mMouseOnPointIndex);
                    newPoint.X = sPoint.X; newPoint.Y = sPoint.Y;
                    sMultiPolyline.UpdateExtent();
                    mEditingGeometry = sMultiPolyline;
                    moMapControl1.RedrawTrackingShapes();
                }
                if (mIsInAddPoint) mIsInEditPoint = false;
                if (mIsInDeletePoint) mIsInEditPoint = false;
            }
        }
        private void PointEditing_MouseMove(MouseEventArgs e)
        {
            if (mIsInAddPoint || mIsInDeletePoint) return;
            if (mIsInEditPoint == false && mEditingGeometry != null)
            {
                MyMapObjects.moPoint sEditingPoint = (MyMapObjects.moPoint)mEditingGeometry;
                double sTolerance = moMapControl1.ToMapDistance(mSelectingTolerance);
                MyMapObjects.moPoint sPoint = moMapControl1.ToMapPoint(e.Location.X, e.Location.Y);
                if (MyMapObjects.moMapTools.IsPointOnPoint(sPoint, sEditingPoint, sTolerance))
                {
                    mMouseOnPartIndex = 0;
                    mMouseOnPointIndex = 0;
                    if (mIsInMovePoint) this.Cursor = new Cursor("ico/EditMoveVertex.ico");
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    mMouseOnPartIndex = -1;
                    mMouseOnPointIndex = -1;
                }
            }
            else
            {
                MyMapObjects.moPoint sPoint = moMapControl1.ToMapPoint(e.Location.X, e.Location.Y);
                if (mIsInMovePoint)
                {
                    mPointEditNeedSave = true;
                    mEditingGeometry = sPoint;
                    moMapControl1.RedrawTrackingShapes();
                }
            }
        }
        private void MultiPointEditing_MouseMove(MouseEventArgs e)
        {
            if (mIsInEditPoint == false && mEditingGeometry != null)
            {
                MyMapObjects.moPoints sPoints = (MyMapObjects.moPoints)mEditingGeometry;
                double sTolerance = moMapControl1.ToMapDistance(mSelectingTolerance);
                MyMapObjects.moPoint sPoint = moMapControl1.ToMapPoint(e.Location.X, e.Location.Y);
                if (PointCloseToPoints(sPoint, sPoints, sTolerance))
                {
                    if (mIsInMovePoint) this.Cursor = new Cursor("ico/EditMoveVertex.ico");
                    if (mIsInAddPoint) this.Cursor = new Cursor("ico/AddPoint.ico");
                    if (mIsInDeletePoint) this.Cursor = new Cursor("ico/DeletePoint.ico");
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    mMouseOnPartIndex = -1;
                    mMouseOnPointIndex = -1;
                }
            }
            else
            {
                MyMapObjects.moPoints sPoints = (MyMapObjects.moPoints)mEditingGeometry;
                MyMapObjects.moPoint sPoint = moMapControl1.ToMapPoint(e.Location.X, e.Location.Y);
                if (mIsInMovePoint)
                {
                    mPointEditNeedSave = true;
                    MyMapObjects.moPoint newPoint = sPoints.GetItem(mMouseOnPointIndex);
                    newPoint.X = sPoint.X; newPoint.Y = sPoint.Y;
                    sPoints.UpdateExtent();
                    mEditingGeometry = sPoints;
                    moMapControl1.RedrawTrackingShapes();
                }
                //if (mIsInAddPoint) mIsInEditPoint = false;
                if (mIsInDeletePoint) mIsInEditPoint = false;
            }
        }


        #endregion



        #region MouseUp

        private void moMapControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (mMapOpStyle == 1)
                {
                    //放大
                    OnZoomIn_MouseUp(e);
                }
                else if (mMapOpStyle == 2)
                {
                    //缩小
                }
                else if (mMapOpStyle == 3)
                {
                    //漫游
                    OnPan_MouseUp(e);
                }
                else if (mMapOpStyle == 4 && mOperatingLayerIndex != -1)
                {
                    OnSelect_MouseUp(e);
                }
                else if (mMapOpStyle == 5)
                {
                    OnIdentify_MouseUp(e);
                }
                else if (mMapOpStyle == 6 && mOperatingLayerIndex != -1)
                {
                    OnEdit_MouseUp(e);
                }
                else if (mMapOpStyle == 7)
                {
                    ;
                }
                else if (mMapOpStyle == 8 && mOperatingLayerIndex != -1)
                {
                    OnEditPoint_MouseUp(e);
                }
                
            }
        }

        private void OnZoomIn_MouseUp(MouseEventArgs e)
        {
            if (mIsInZoom == false)
            {
                return;
            }
            mIsInZoom = false;
            if (mStartMouseLocation.X == e.Location.X && mStartMouseLocation.Y == e.Location.Y)
            {
                MyMapObjects.moPoint sPoint = moMapControl1.ToMapPoint(mStartMouseLocation.X, mStartMouseLocation.Y);
                moMapControl1.ZoomByCenter(sPoint, mZoomRatioFixed);
            }
            else
            {
                MyMapObjects.moRectangle sBox = GetMapRectByTwoPoints(mStartMouseLocation, e.Location);
                moMapControl1.ZoomToExtent(sBox);
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;

        }

        private void OnPan_MouseUp(MouseEventArgs e)
        {
            if (mIsInPan == false)
                return;
            mIsInPan = false;
            double sDeltaX = moMapControl1.ToMapDistance(e.Location.X - mStartMouseLocation.X);
            double sDeltaY = moMapControl1.ToMapDistance(mStartMouseLocation.Y - e.Location.Y);
            moMapControl1.PanDelta(sDeltaX, sDeltaY);
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void OnSelect_MouseUp(MouseEventArgs e)
        {
            if (mIsInSelect == false)
            {
                return;
            }
            mIsInSelect = false;
            MyMapObjects.moRectangle sBox = GetMapRectByTwoPoints(mStartMouseLocation, e.Location);
            double sTolerance = moMapControl1.ToMapDistance(mSelectingTolerance);
            moMapControl1.SelectLayerByBox(sBox, sTolerance, mOperatingLayerIndex); //该方法只在当前图层中选择，与demo中不同
            if(All_tables.Count != 0)
            {
                All_tables[mOperatingLayerIndex].Refresh_dataform_select();
            }
            moMapControl1.RedrawTrackingShapes();
         
          //  RedrawAttribute();

        }
  
        private void OnEdit_MouseUp(MouseEventArgs e)
        {
            if (mIsInMove)
            {
                OnMoveSelect_MouseUp(e);
            }
            else if (mIsInSelect)
            {
                OnSelect_MouseUp(e);
            }
        }

        private void OnIdentify_MouseUp(MouseEventArgs e)
        {
            if (mIsInIdentify == false)
            {
                return;
            }
            mIsInIdentify = false;
            moMapControl1.Refresh();
            MyMapObjects.moRectangle sBox = GetMapRectByTwoPoints(mStartMouseLocation, e.Location);
            double sTolerance = moMapControl1.ToMapDistance(mSelectingTolerance);
            moMapControl1.SelectLayerByBox(sBox, sTolerance, mOperatingLayerIndex); //该方法只在当前图层中选择，与demo中不同
            if (All_tables.Count != 0)
            {
                All_tables[mOperatingLayerIndex].Refresh_dataform_select();
            }
            moMapControl1.RedrawTrackingShapes();
            if (moMapControl1.Layers.Count == 0)
            {
                return;
            }
            else
            {
                MyMapObjects.moMapLayer sLayer = moMapControl1.Layers.GetItem(mOperatingLayerIndex);
                MyMapObjects.moFeatures sFeatures = sLayer.SearchByBox(sBox, sTolerance);
                Int32 sSelFeatureCount = sFeatures.Count;
                if (sSelFeatureCount > 0)
                {
                    MyMapObjects.moGeometry[] sGeometries = new MyMapObjects.moGeometry[sSelFeatureCount];
                    for (Int32 i = 0; i < sSelFeatureCount; i++)
                    {
                        sGeometries[i] = sFeatures.GetItem(i).Geometry;
                    }
                    moMapControl1.FlashShapes(sGeometries, 5, 800);

                }
            }
            Form_table datafram_windows = new Form_table(this, mLastOpLayerIndex);
            //需要在弹出的属性表中仅显示选中要素
            datafram_windows.Owner = this;
            datafram_windows.Name = moMapControl1.Layers.GetItem(mLastOpLayerIndex).Name;
            datafram_windows.Show();
        }

        private void OnMoveSelect_MouseUp(MouseEventArgs e)
        {
            if (mIsInMove == false) return;
            mIsInMove = false;
            if (mSelectedIsMoved)
            {
                //做相应的数据修改
                mSelectedIsMoved = false;
                mNeedToSave = true;
                MyMapObjects.moMapLayer sLayer = moMapControl1.Layers.GetItem(mOperatingLayerIndex);
                for (Int32 i = 0; i < sLayer.SelectedFeatures.Count; i++)
                {
                    MyMapObjects.moFeature sFeature = sLayer.SelectedFeatures.GetItem(i);
                    sFeature.Geometry = mMovingGeometries[i];
                }
                sLayer.UpdateExtent();
                //重构地图
                moMapControl1.RedrawMap();
            }
            else
            {
                MyMapObjects.moRectangle sBox = GetMapRectByTwoPoints(e.Location, e.Location);
                double sTolerance = moMapControl1.ToMapDistance(mSelectingTolerance);
                moMapControl1.SelectLayerByBox(sBox, sTolerance, mOperatingLayerIndex); //该方法只在当前图层中选择，与demo中不同
                moMapControl1.RedrawTrackingShapes();
            }
            //清除移动图形列表
            mMovingGeometries.Clear();
        }
        private void OnEditPoint_MouseUp(MouseEventArgs e)
        {
            if (mIsInEditPoint == false)
            {
                MoveFeatureBtn_Click(new object(), e);
            }
            if (mIsInAddPoint && moMapControl1.Layers.GetItem(mOperatingLayerIndex).ShapeType != MyMapObjects.moGeometryTypeConstant.Point)
            {
                mEditPointOperation.Add(2);
                mPastPartIndex.Add(mMouseOnPartIndex);
                mPastPointIndex.Add(mMouseOnPointIndex);
            }
            MyMapObjects.moGeometryTypeConstant shapeType = moMapControl1.Layers.GetItem(mOperatingLayerIndex).ShapeType;
            mIsInEditPoint = false;
            if (shapeType == MyMapObjects.moGeometryTypeConstant.MultiPolygon)
            {
                MultiPolygonEditing_MouseUp(e);
            }
            else if (shapeType == MyMapObjects.moGeometryTypeConstant.MultiPolyline)
            {
                MultiPolylineEditing_MouseUp(e);
            }
            else if (shapeType == MyMapObjects.moGeometryTypeConstant.Point)
            {
                PointEditing_MouseUp(e);
            }
            else if (shapeType == MyMapObjects.moGeometryTypeConstant.MultiPoint)
            {
                MultiPointEditing_MouseUp(e);
            }
            mMouseOnPartIndex = -1;
            mMouseOnPointIndex = -1;
        }
        private void MultiPolygonEditing_MouseUp(MouseEventArgs e)
        {
            MyMapObjects.moMultiPolygon sMultiPolygon = (MyMapObjects.moMultiPolygon)mEditingGeometry;
            MyMapObjects.moPoint sPoint = moMapControl1.ToMapPoint(e.Location.X, e.Location.Y);
            if (mIsInAddPoint)
            {
                mPointEditNeedSave = true;
                MyMapObjects.moPoints sPoints = sMultiPolygon.Parts.GetItem(mMouseOnPartIndex);
                sPoints.Insert(mMouseOnPointIndex + 1, sPoint);
                sMultiPolygon.UpdateExtent();
                mEditingGeometry = sMultiPolygon;
                moMapControl1.RedrawTrackingShapes();
                MovePointBtn_Click(new object(), e);
            }
            if (mIsInDeletePoint)
            {
                mPointEditNeedSave = true;
                MyMapObjects.moPoints sPoints = sMultiPolygon.Parts.GetItem(mMouseOnPartIndex);
                if (sPoints.Count > 3)
                {
                    mEditPointOperation.Add(3);
                    mEditPointRecord.Add(sPoints.GetItem(mMouseOnPointIndex).Clone());
                    mPastPartIndex.Add(mMouseOnPartIndex);
                    mPastPointIndex.Add(mMouseOnPointIndex);
                    sPoints.RemoveAt(mMouseOnPointIndex);
                }
                else
                {
                    MessageBox.Show("无法继续删减，否则会导致要素的几何无效！");
                    MovePointBtn_Click(new object(), e);
                    return;
                }
                sMultiPolygon.UpdateExtent();
                mEditingGeometry = sMultiPolygon;
                moMapControl1.RedrawTrackingShapes();
            }
        }
        private void MultiPolylineEditing_MouseUp(MouseEventArgs e)
        {
            MyMapObjects.moMultiPolyline sMultiPolyline = (MyMapObjects.moMultiPolyline)mEditingGeometry;
            MyMapObjects.moPoint sPoint = moMapControl1.ToMapPoint(e.Location.X, e.Location.Y);
            if (mIsInAddPoint)
            {
                mPointEditNeedSave = true;
                MyMapObjects.moPoints sPoints = sMultiPolyline.Parts.GetItem(mMouseOnPartIndex);
                sPoints.Insert(mMouseOnPointIndex + 1, sPoint);
                sMultiPolyline.UpdateExtent();
                mEditingGeometry = sMultiPolyline;
                moMapControl1.RedrawTrackingShapes();
                MovePointBtn_Click(new object(), e);
            }
            if (mIsInDeletePoint)
            {
                mPointEditNeedSave = true;
                MyMapObjects.moPoints sPoints = sMultiPolyline.Parts.GetItem(mMouseOnPartIndex);
                if (sPoints.Count > 2)
                {
                    mEditPointOperation.Add(3);
                    mEditPointRecord.Add(sPoints.GetItem(mMouseOnPointIndex).Clone());
                    mPastPartIndex.Add(mMouseOnPartIndex);
                    mPastPointIndex.Add(mMouseOnPointIndex);
                    sPoints.RemoveAt(mMouseOnPointIndex);
                    sPoints.RemoveAt(mMouseOnPointIndex);
                }
                else
                {
                    MessageBox.Show("无法继续删减，否则会导致要素的几何无效！");
                    MovePointBtn_Click(new object(), e);
                    return;
                }
                sMultiPolyline.UpdateExtent();
                mEditingGeometry = sMultiPolyline;
                moMapControl1.RedrawTrackingShapes();
            }
        }
        private void PointEditing_MouseUp(MouseEventArgs e)
        {
            if (mIsInAddPoint || mIsInDeletePoint)
            {
                MessageBox.Show("点要素无法执行当前操作！");
                MovePointBtn_Click(new object(), e);
            }
        }
        private void MultiPointEditing_MouseUp(MouseEventArgs e)
        {
            MyMapObjects.moPoints sPoints = (MyMapObjects.moPoints)mEditingGeometry;
            MyMapObjects.moPoint sPoint = moMapControl1.ToMapPoint(e.Location.X, e.Location.Y);
            if (mIsInAddPoint)
            {
                mPointEditNeedSave = true;
                sPoints.Insert(mMouseOnPointIndex, sPoint);
                sPoints.UpdateExtent();
                mEditingGeometry = sPoints;
                moMapControl1.RedrawTrackingShapes();
            }
            if (mIsInDeletePoint)
            {
                mPointEditNeedSave = true;
                if (sPoints.Count > 1)
                {
                    mEditPointOperation.Add(3);
                    mEditPointRecord.Add(sPoints.GetItem(mMouseOnPointIndex).Clone());
                    mPastPartIndex.Add(mMouseOnPartIndex);
                    mPastPointIndex.Add(mMouseOnPointIndex);
                    sPoints.RemoveAt(mMouseOnPointIndex);
                    sPoints.RemoveAt(mMouseOnPointIndex);
                }
                else
                {
                    MessageBox.Show("无法继续删减，否则会导致要素的几何无效！");
                    MovePointBtn_Click(new object(), e);
                    return;
                }
                sPoints.UpdateExtent();
                mEditingGeometry = sPoints;
                moMapControl1.RedrawTrackingShapes();
            }
        }



        #endregion



        #region MouseClick

        private void moMapControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (mMapOpStyle == 1)
                {
                    ;
                }
                else if (mMapOpStyle == 2)
                {
                    OnZoomOut_MouseClick(e);
                }
                else if (mMapOpStyle == 3)
                {
                    ;
                }
                else if (mMapOpStyle == 4)
                {
                    ;
                }
                else if (mMapOpStyle == 6)
                {
                    ;
                }
                else if (mMapOpStyle == 7 && mOperatingLayerIndex != -1)
                {
                    OnSketch_MouseClick(e);
                }
            }
        }

        private void OnZoomOut_MouseClick(MouseEventArgs e)
        {
            this.Cursor = new Cursor("ico/ZoomOut.ico");

            //单点缩小
            MyMapObjects.moPoint sPoint = moMapControl1.ToMapPoint(e.Location.X, e.Location.Y);
            moMapControl1.ZoomByCenter(sPoint, 1 / mZoomRatioFixed);
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }


        private void OnSketch_MouseClick(MouseEventArgs e)
        {
            MyMapObjects.moMapLayer sMapLayer = moMapControl1.Layers.GetItem(mOperatingLayerIndex);
            if (sMapLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.Point)
            {
                //将屏幕坐标转换为地图坐标并加入描绘图形
                MyMapObjects.moPoint sPoint = moMapControl1.ToMapPoint(e.Location.X, e.Location.Y);
                mSketchingPoint.Add(sPoint);
                EndSketchGeo(sMapLayer.ShapeType);
                //地图控件重绘跟踪图层
                moMapControl1.RedrawTrackingShapes();
            }
            else if (sMapLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPoint)
            {
                //将屏幕坐标转换为地图坐标并加入描绘图形
                MyMapObjects.moPoint sPoint = moMapControl1.ToMapPoint(e.Location.X, e.Location.Y);
                mSketchingPoint.Add(sPoint);
                EndSketchPart(sMapLayer.ShapeType);
                //地图控件重绘跟踪图层
                moMapControl1.RedrawTrackingShapes();
            }
            else
            {
                //将屏幕坐标转换为地图坐标并加入描绘图形
                MyMapObjects.moPoint sPoint = moMapControl1.ToMapPoint(e.Location.X, e.Location.Y);
                mSketchingShape.Last().Add(sPoint);
                //地图控件重绘跟踪图层
                moMapControl1.RedrawTrackingShapes();
            }
        }


        private void moMapControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (mOperatingLayerIndex != -1)
            {
                if (mMapOpStyle == 7)
                {
                    OnSketch_MouseDoubleClick(e);
                }
            }
        }
        private void OnSketch_MouseDoubleClick(MouseEventArgs e)
        {
            MyMapObjects.moMapLayer sMapLayer = moMapControl1.Layers.GetItem(mOperatingLayerIndex);
            EndSketchPart(sMapLayer.ShapeType);
            EndSketchGeo(sMapLayer.ShapeType);
        }

        #endregion

        #region MouseWheel
        //鼠标滑轮
        private void MoMap_MouseWheel(object sender, MouseEventArgs e)
        {
            //计算地图控件中心点的地图坐标
            double sX = moMapControl1.ClientRectangle.Width / 2;
            double sY = moMapControl1.ClientRectangle.Height / 2;
            MyMapObjects.moPoint sPoint = moMapControl1.ToMapPoint(sX, sY);
            if (e.Delta > 0)
            {
                moMapControl1.ZoomByCenter(sPoint, mZoomRatioMouseWheel);
            }
            else
            {
                moMapControl1.ZoomByCenter(sPoint, 1 / mZoomRatioMouseWheel);
            }
        }
        #endregion
        #endregion

        #region 图层渲染与注记
        public void GetPointRenderer(Int32 renderMode, Int32 symbolStyle, Color simpleRendererColor, Double simpleRendererSize,
            Int32 uniqueFieldIndex, Double uniqueRendererSize, Int32 classBreakFieldIndex, Int32 classNum,
            Color classBreakRendererColor, double classBreakRendererMinSize, double classBreakRendererMaxSize)
        {
            mPointRendererMode = renderMode;
            mPointSymbolStyle = symbolStyle;
            mPointSimpleRendererColor = simpleRendererColor;
            mPointSimpleRendererSize = simpleRendererSize;
            mPointUniqueFieldIndex = uniqueFieldIndex;
            mPointUniqueRendererSize = uniqueRendererSize;
            mPointClassBreaksFieldIndex = classBreakFieldIndex;
            mPointClassBreaksNum = classNum;
            mPointClassBreaksRendererColor = classBreakRendererColor;
            mPointClassBreaksRendererMinSize = classBreakRendererMinSize;
            mPointClassBreaksRendererMaxSize = classBreakRendererMaxSize;
            mIsInRenderer = true;
        }

        public void GetPolylineRenderer(Int32 renderMode, Int32 symbolStyle, Color simpleRendererColor, Double simpleRendererSize,
           Int32 uniqueFieldIndex, Double uniqueRendererSize, Int32 classBreakFieldIndex, Int32 classNum,
           Color classBreakRendererColor, double classBreakRendererMinSize, double classBreakRendererMaxSize)
        {
            mPolylineRendererMode = renderMode;
            mPolylineSymbolStyle = symbolStyle;
            mPolylineSimpleRendererColor = simpleRendererColor;
            mPolylineSimpleRendererSize = simpleRendererSize;
            mPolylineUniqueFieldIndex = uniqueFieldIndex;
            mPolylineUniqueRendererSize = uniqueRendererSize;
            mPolylineClassBreaksFieldIndex = classBreakFieldIndex;
            mPolylineClassBreaksNum = classNum;
            mPolylineClassBreaksRendererColor = classBreakRendererColor;
            mPolylineClassBreaksRendererMinSize = classBreakRendererMinSize;
            mPolylineClassBreaksRendererMaxSize = classBreakRendererMaxSize;
            mIsInRenderer = true;
        }

        public void GetPolygonRenderer(Int32 renderMode,Color OutlineRendererColor, Color simpleRendererColor,
            Int32 uniqueFieldIndex, Int32 classBreakFieldIndex, Int32 classNum,
            Color classBreakRendererStartColor, Color classBreakRendererEndColor)
        {
            mPolygonRendererMode = renderMode;
            mPolygonSimpleRendererColor = simpleRendererColor;
            mOutlineColor = OutlineRendererColor;
            mPolygonUniqueFieldIndex = uniqueFieldIndex;
            mPolygonClassBreaksFieldIndex = classBreakFieldIndex;
            mPolygonClassBreaksNum = classNum;
            mPolygonClassBreaksRendererStartColor = classBreakRendererStartColor;
            mPolygonClassBreaksRendererEndColor = classBreakRendererEndColor;
            mIsInRenderer = true;
        }

        public void GetLabel(bool visible, bool useMask, Int32 fieldIndex, Color color, Font font)
        {
            mLabelVisible = visible;
            mLabelUseMask = useMask;
            mLabelFieldIndex = fieldIndex;
            mLabelColor = color;
            mLabelFont = font;
        }
        #endregion 



        private void moMapControl1_AfterTrackingLayerDraw(object sender, moUserDrawingTool drawTool)
        {
            DrawSketchingShapes(drawTool);   //绘制描绘图形
            DrawEditingShapes(drawTool); //绘制正在编辑的图形
        }

        private void moMapControl1_MapScaleChanged(object sender)
        {
            ShowMapScale();
        }


        #region 私有函数 

        //获取当前操作图层索引
        private Int32 GetOpLayerIndex()
        {
            if (SelectLayer.SelectedIndex == -1)
            {
                if (mEditMoMap == true) return -1;
                else return mLastOpLayerIndex;
            }
            else
            {
                return mLayerIndex[SelectLayer.SelectedIndex];
            }
        }

        // 初始化描绘图形
        private void InitializeSketchingShape()
        {
            mSketchingPoint = new List<MyMapObjects.moPoint>();
            mSketchingShape = new List<MyMapObjects.moPoints>();
            MyMapObjects.moPoints sPoints = new MyMapObjects.moPoints();
            mSketchingShape.Add(sPoints);
        }

        //根据屏幕坐标显示地图坐标
        private void ShowCoordinates(PointF point)
        {
            MyMapObjects.moPoint sPoint = moMapControl1.ToMapPoint(point.X, point.Y);
            if (mShowLngLat == false)
            {
                double sX = Math.Round(sPoint.X, 2);
                double sY = Math.Round(sPoint.Y, 2);
                tssCoordinate.Text = "X: " + sX.ToString() + ", Y: " + sY.ToString();
            }
            else
            {
                MyMapObjects.moPoint sLngLat = moMapControl1.ProjectionCS.TransferToLngLat(sPoint);
                double sX = Math.Round(sLngLat.X, 4);
                double sY = Math.Round(sLngLat.Y, 4);
                tssCoordinate.Text = "X: " + sX.ToString() + ", Y: " + sY.ToString();
            }
        }
        //根据屏幕上两点获得一个地图坐标下的矩形
        private MyMapObjects.moRectangle GetMapRectByTwoPoints(PointF point1, PointF point2)
        {
            MyMapObjects.moPoint sPoint1 = moMapControl1.ToMapPoint(point1.X, point1.Y);
            MyMapObjects.moPoint sPoint2 = moMapControl1.ToMapPoint(point2.X, point2.Y);
            double sMinX = Math.Min(sPoint1.X, sPoint2.X);
            double sMaxX = Math.Max(sPoint1.X, sPoint2.X);
            double sMinY = Math.Min(sPoint1.Y, sPoint2.Y);
            double sMaxY = Math.Max(sPoint1.Y, sPoint2.Y);
            MyMapObjects.moRectangle sRect = new MyMapObjects.moRectangle(sMinX, sMaxX, sMinY, sMaxY);
            return sRect;
        }
        //根据指定的平移量修改移动图形的坐标
        private void ModifyMovingGeometries(double deltaX, double deltaY)
        {
            Int32 sCount = mMovingGeometries.Count;
            for (Int32 i = 0; i <= sCount - 1; i++)
            {
                if (mMovingGeometries[i].GetType() == typeof(MyMapObjects.moMultiPolygon))
                {
                    MyMapObjects.moMultiPolygon sMultiPolygon = (MyMapObjects.moMultiPolygon)mMovingGeometries[i];
                    Int32 sPartCount = sMultiPolygon.Parts.Count;
                    for (Int32 j = 0; j <= sPartCount - 1; j++)
                    {
                        MyMapObjects.moPoints sPoints = sMultiPolygon.Parts.GetItem(j);
                        Int32 sPointCount = sPoints.Count;
                        for (Int32 k = 0; k <= sPointCount - 1; k++)
                        {
                            MyMapObjects.moPoint sPoint = sPoints.GetItem(k);
                            sPoint.X = sPoint.X + deltaX;
                            sPoint.Y = sPoint.Y + deltaY;
                        }
                    }
                    sMultiPolygon.UpdateExtent();
                }
                else if (mMovingGeometries[i].GetType() == typeof(MyMapObjects.moMultiPolyline))
                {
                    MyMapObjects.moMultiPolyline sMultiPolyline = (MyMapObjects.moMultiPolyline)mMovingGeometries[i];
                    Int32 sPartCount = sMultiPolyline.Parts.Count;
                    for (Int32 j = 0; j <= sPartCount - 1; j++)
                    {
                        MyMapObjects.moPoints sPoints = sMultiPolyline.Parts.GetItem(j);
                        Int32 sPointCount = sPoints.Count;
                        for (Int32 k = 0; k <= sPointCount - 1; k++)
                        {
                            MyMapObjects.moPoint sPoint = sPoints.GetItem(k);
                            sPoint.X = sPoint.X + deltaX;
                            sPoint.Y = sPoint.Y + deltaY;
                        }
                    }
                    sMultiPolyline.UpdateExtent();
                }
                else if (mMovingGeometries[i].GetType() == typeof(MyMapObjects.moPoint))
                {
                    MyMapObjects.moPoint sPoint = (MyMapObjects.moPoint)mMovingGeometries[i];
                    sPoint.X = sPoint.X + deltaX;
                    sPoint.Y = sPoint.Y + deltaY;
                }
                else if (mMovingGeometries[i].GetType() == typeof(MyMapObjects.moPoints))
                {
                    MyMapObjects.moPoints sPoints = (MyMapObjects.moPoints)mMovingGeometries[i];
                    for (Int32 j = 0; j < sPoints.Count; j++)
                    {
                        MyMapObjects.moPoint sPoint = sPoints.GetItem(j);
                        sPoint.X = sPoint.X + deltaX;
                        sPoint.Y = sPoint.Y + deltaY;
                    }
                    sPoints.UpdateExtent();
                }
            }
        }
        //绘制正在移动的图形
        private void DrawMovingShape()
        {
            MyMapObjects.moUserDrawingTool sDrawingTool = moMapControl1.GetDrawingTool();
            Int32 sCount = mMovingGeometries.Count;
            for (Int32 i = 0; i <= sCount - 1; i++)
            {
                if (mMovingGeometries[i].GetType() == typeof(MyMapObjects.moMultiPolygon))
                {
                    MyMapObjects.moMultiPolygon sMultiPolygon = (MyMapObjects.moMultiPolygon)mMovingGeometries[i];
                    sDrawingTool.DrawMultiPolygon(sMultiPolygon, mMovingPolygonSymbol);
                }
                else if (mMovingGeometries[i].GetType() == typeof(MyMapObjects.moMultiPolyline))
                {
                    MyMapObjects.moMultiPolyline sMultiPolyline = (MyMapObjects.moMultiPolyline)mMovingGeometries[i];
                    sDrawingTool.DrawMultiPolyline(sMultiPolyline, mMovingPolylineSymbol);
                }
                else if (mMovingGeometries[i].GetType() == typeof(MyMapObjects.moPoint))
                {
                    MyMapObjects.moPoint sPoint = (MyMapObjects.moPoint)mMovingGeometries[i];
                    sDrawingTool.DrawPoint(sPoint, mMovingPointSymbol);
                }
                else if (mMovingGeometries[i].GetType() == typeof(MyMapObjects.moPoints))
                {
                    MyMapObjects.moPoints sPoints = (MyMapObjects.moPoints)mMovingGeometries[i];
                    sDrawingTool.DrawPoints(sPoints, mMovingPointSymbol);
                }
            }
        }
        //判断点是否靠近多边形
        private bool PointCloseToMultiPolygonPoint(MyMapObjects.moPoint sPoint, MyMapObjects.moMultiPolygon sMultiPolygon, double sTolerance)
        {
            if (mIsInMovePoint || mIsInDeletePoint)
            {
                for (Int32 i = 0; i < sMultiPolygon.Parts.Count; i++)
                {
                    MyMapObjects.moPoints sPoints = sMultiPolygon.Parts.GetItem(i);
                    Int32 j = 0;
                    for (; j < sPoints.Count; j++)
                    {
                        if (MyMapObjects.moMapTools.IsPointOnPoint(sPoint, sPoints.GetItem(j), sTolerance))
                        {
                            mMouseOnPartIndex = i;
                            mMouseOnPointIndex = j;
                            return true;
                        }
                    }
                }
            }
            if (mIsInAddPoint)
            {
                for (Int32 i = 0; i < sMultiPolygon.Parts.Count; i++)
                {
                    MyMapObjects.moPoints sPoints = sMultiPolygon.Parts.GetItem(i);
                    for (Int32 j = 0; j < sPoints.Count; j++)
                    {
                        MyMapObjects.moPoints tempPoints = new MyMapObjects.moPoints();
                        tempPoints.Add(sPoints.GetItem(j));
                        if (j < sPoints.Count - 1)
                        {
                            tempPoints.Add(sPoints.GetItem(j + 1));
                        }
                        else
                        {
                            tempPoints.Add(sPoints.GetItem(0));
                        }
                        tempPoints.UpdateExtent();
                        if (MyMapObjects.moMapTools.IsPointOnPolyline(sPoint, tempPoints, sTolerance))
                        {
                            mMouseOnPartIndex = i;
                            mMouseOnPointIndex = j;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        //判断点是否靠近折线
        private bool PointCloseToMultiPolylinePoint(MyMapObjects.moPoint sPoint, MyMapObjects.moMultiPolyline sMultiPolyline, double sTolerance)
        {
            if (mIsInMovePoint || mIsInDeletePoint)
            {
                for (Int32 i = 0; i < sMultiPolyline.Parts.Count; i++)
                {
                    MyMapObjects.moPoints sPoints = sMultiPolyline.Parts.GetItem(i);
                    Int32 j = 0;
                    for (; j < sPoints.Count; j++)
                    {
                        if (MyMapObjects.moMapTools.IsPointOnPoint(sPoint, sPoints.GetItem(j), sTolerance))
                        {
                            mMouseOnPartIndex = i;
                            mMouseOnPointIndex = j;
                            return true;
                        }
                    }
                }
            }
            if (mIsInAddPoint)
            {
                for (Int32 i = 0; i < sMultiPolyline.Parts.Count; i++)
                {
                    MyMapObjects.moPoints sPoints = sMultiPolyline.Parts.GetItem(i);
                    for (Int32 j = 0; j < sPoints.Count - 1; j++)
                    {
                        MyMapObjects.moPoints tempPoints = new MyMapObjects.moPoints();
                        tempPoints.Add(sPoints.GetItem(j));
                        tempPoints.Add(sPoints.GetItem(j + 1));
                        tempPoints.UpdateExtent();
                        if (MyMapObjects.moMapTools.IsPointOnPolyline(sPoint, tempPoints, sTolerance))
                        {
                            mMouseOnPartIndex = i;
                            mMouseOnPointIndex = j;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        //判断点是否靠近多点
        private bool PointCloseToPoints(MyMapObjects.moPoint sPoint, MyMapObjects.moPoints sPoints, double sTolerance)
        {
            if (mIsInMovePoint || mIsInDeletePoint)
            {
                for (Int32 i = 0; i < sPoints.Count; i++)
                {
                    if (MyMapObjects.moMapTools.IsPointOnPoint(sPoint, sPoints.GetItem(i), sTolerance))
                    {
                        mMouseOnPartIndex = 0;
                        mMouseOnPointIndex = i;
                        return true;
                    }
                }
            }
            if (mIsInAddPoint)
            {
                mMouseOnPartIndex = 0;
                mMouseOnPointIndex = sPoints.Count;
                return true;
            }
            return false;
        }
        //保存当前对节点的编辑
        private void SavePointEdit()
        {
            if (mPointEditNeedSave)
            {
                MyMapObjects.moMapLayer sLayer = moMapControl1.Layers.GetItem(mOperatingLayerIndex);
                sLayer.SelectedFeatures.GetItem(0).Geometry = mEditingGeometry;
                sLayer.UpdateExtent();
                mEditingGeometry = null;
                mMouseOnPartIndex = -1;
                mMouseOnPointIndex = -1;
                mPointEditNeedSave = false;
                mNeedToSave = true;
                DeleteEditPointRecord();
                moMapControl1.RedrawMap();
            }
            else
            {
                mEditingGeometry = null;
                mMouseOnPartIndex = -1;
                mMouseOnPointIndex = -1;
                moMapControl1.RedrawMap();
            }
            this.Cursor = Cursors.Default;
        }
        //删除编辑记录
        private void DeleteEditPointRecord()
        {
            mEditPointOperation.Clear();
            mEditPointRecord.Clear();
            mPastPartIndex.Clear();
            mPastPointIndex.Clear();
        }
        //选择状态右键菜单
        private void RightMenuInSelect()
        {
            if (mOperatingLayerIndex == -1) return;
            moMapRightMenu.Items.Clear();
            moMapRightMenu.Items.Add("复制");
            moMapRightMenu.Items.Add("粘贴");
            moMapRightMenu.Items.Add("删除");
            moMapRightMenu.Items.Add("复制并粘贴到新建图层");
        }

        //结束描绘图形
        private void EndSketchGeo(MyMapObjects.moGeometryTypeConstant shapeType)
        {
            mNeedToSave = true;
            if (shapeType == MyMapObjects.moGeometryTypeConstant.MultiPolygon)
            {
                if (mSketchingShape.Last().Count >= 1 && mSketchingShape.Last().Count < 3)
                    return;
                //如果最后一个部件的点数为0，则删除最后一个部件
                if (mSketchingShape.Last().Count == 0)
                {
                    mSketchingShape.Remove(mSketchingShape.Last());
                }
                //如果用户的确输入了，则加入多边形图层
                if (mSketchingShape.Count > 0)
                {
                    //查找多边形图层，如果有则加入该图层
                    MyMapObjects.moMapLayer sLayer = moMapControl1.Layers.GetItem(mOperatingLayerIndex);
                    //新建复合多边形
                    MyMapObjects.moMultiPolygon sMultiPolygon = new MyMapObjects.moMultiPolygon();
                    sMultiPolygon.Parts.AddRange(mSketchingShape.ToArray());
                    sMultiPolygon.UpdateExtent();
                    //生成要素并加入图层
                    MyMapObjects.moFeature sFeature = sLayer.GetNewFeature();
                    sFeature.Geometry = sMultiPolygon;
                    sLayer.Features.Add(sFeature);
                    sLayer.UpdateExtent();
                    sLayer.SelectedFeatures.Clear();
                    sLayer.SelectedFeatures.Add(sFeature);
                }
                //初始化描绘图形
                InitializeSketchingShape();
                //重绘地图
                moMapControl1.RedrawMap();
            }
            else if (shapeType == MyMapObjects.moGeometryTypeConstant.MultiPolyline)
            {
                if (mSketchingShape.Last().Count == 1)
                    return;
                //如果最后一个部件的点数为0，则删除最后一个部件
                if (mSketchingShape.Last().Count == 0)
                {
                    mSketchingShape.Remove(mSketchingShape.Last());
                }
                //如果用户的确输入了，则加入多边形图层
                if (mSketchingShape.Count > 0)
                {
                    //查找多边形图层，如果有则加入该图层
                    MyMapObjects.moMapLayer sLayer = moMapControl1.Layers.GetItem(mOperatingLayerIndex);
                    //新建复合多边形
                    MyMapObjects.moMultiPolyline sMultiPolyline = new MyMapObjects.moMultiPolyline();
                    sMultiPolyline.Parts.AddRange(mSketchingShape.ToArray());
                    sMultiPolyline.UpdateExtent();
                    //生成要素并加入图层
                    MyMapObjects.moFeature sFeature = sLayer.GetNewFeature();
                    sFeature.Geometry = sMultiPolyline;
                    sLayer.Features.Add(sFeature);
                    sLayer.UpdateExtent();
                    sLayer.SelectedFeatures.Clear();
                    sLayer.SelectedFeatures.Add(sFeature);
                }
                //初始化描绘图形
                InitializeSketchingShape();
                //重绘地图
                moMapControl1.RedrawMap();
            }
            else if (shapeType == MyMapObjects.moGeometryTypeConstant.Point)
            {
                if (mSketchingPoint.Count == 1)
                {
                    MyMapObjects.moMapLayer sLayer = moMapControl1.Layers.GetItem(mOperatingLayerIndex);
                    MyMapObjects.moFeature sFeature = sLayer.GetNewFeature();
                    sFeature.Geometry = mSketchingPoint[0];
                    sLayer.Features.Add(sFeature);
                    sLayer.UpdateExtent();
                    sLayer.SelectedFeatures.Clear();
                    sLayer.SelectedFeatures.Add(sFeature);
                }
                InitializeSketchingShape();
                moMapControl1.RedrawMap();
            }
            else if (shapeType == MyMapObjects.moGeometryTypeConstant.MultiPoint)
            {
                if (mSketchingPoint.Count > 0)
                {
                    MyMapObjects.moMapLayer sLayer = moMapControl1.Layers.GetItem(mOperatingLayerIndex);
                    //新建复合多边形
                    MyMapObjects.moPoints sPoints = new MyMapObjects.moPoints();
                    sPoints.AddRange(mSketchingPoint.ToArray());
                    sPoints.UpdateExtent();
                    //生成要素并加入图层
                    MyMapObjects.moFeature sFeature = sLayer.GetNewFeature();
                    sFeature.Geometry = sPoints;
                    sLayer.Features.Add(sFeature);
                    sLayer.UpdateExtent();
                    sLayer.SelectedFeatures.Clear();
                    sLayer.SelectedFeatures.Add(sFeature);
                }
                //初始化描绘图形
                InitializeSketchingShape();
                //重绘地图
                moMapControl1.RedrawMap();
            }
        }
        //结束描绘部件
        private void EndSketchPart(MyMapObjects.moGeometryTypeConstant shapeType)
        {
            mNeedToSave = true;
            if (shapeType == MyMapObjects.moGeometryTypeConstant.MultiPolygon)
            {
                //判断是否可以结束，即是否最少三个点
                if (mSketchingShape.Last().Count < 3)
                    return;
                //往描绘图形中增加一个多点对象
                MyMapObjects.moPoints sPoints = new MyMapObjects.moPoints();
                mSketchingShape.Add(sPoints);
                //重绘跟踪层
                moMapControl1.RedrawTrackingShapes();
            }
            else if (shapeType == MyMapObjects.moGeometryTypeConstant.MultiPolyline)
            {
                //判断是否可以结束，即是否最少两个点
                if (mSketchingShape.Last().Count < 2)
                    return;
                //往描绘图形中增加一个多点对象
                MyMapObjects.moPoints sPoints = new MyMapObjects.moPoints();
                mSketchingShape.Add(sPoints);
                //重绘跟踪层
                moMapControl1.RedrawTrackingShapes();
            }
            else if (shapeType == MyMapObjects.moGeometryTypeConstant.Point)
            {
                ;
            }
            else if (shapeType == MyMapObjects.moGeometryTypeConstant.MultiPoint)
            {
                //重绘跟踪层
                moMapControl1.RedrawTrackingShapes();
            }
        }

        //刷新图层下拉框
        private void RefreshSelectLayer()
        {
            SelectLayer.Items.Clear();
            mLayerIndex.Clear();
            for (Int32 i = 0; i < moMapControl1.Layers.Count; i++)
            {
                if (moMapControl1.Layers.GetItem(i).Visible == true)
                {
                    SelectLayer.Items.Add(moMapControl1.Layers.GetItem(i).Name);
                    mLayerIndex.Add(i);
                }
            }
            Int32 tempIndex = -1;
            for (Int32 i = 0; i < mLayerIndex.Count; i++)
            {
                if (mLayerIndex[i] == mLastOpLayerIndex)
                {
                    tempIndex = i;
                    break;
                }
            }
            if (tempIndex == -1 && mLayerIndex.Count > 0) tempIndex = 0;
            SelectLayer.SelectedIndex = tempIndex;
        }
        //取消修改
        private void CancelEdit()
        {
            try
            {
                if (mPointEditNeedSave)
                {
                    mEditingGeometry = null;
                    mPointEditNeedSave = false;
                    moMapControl1.RedrawMap();
                }
                for (Int32 i = 0; i < moMapControl1.Layers.Count; i++)
                {
                    MyMapObjects.moMapLayer sLayer = moMapControl1.Layers.GetItem(i);
                    MyMapObjects.moMapLayer sMapLayer = new MyMapObjects.moMapLayer(sLayer.Name, sLayer.ShapeType, mDBFFiles[i].Fields);
                    //加载要素
                    MyMapObjects.moFeatures sFeatures = new MyMapObjects.moFeatures();
                    for (Int32 j = 0; j < mSPGLShapeFiles[i].Geometries.Count; ++j)
                    {
                        MyMapObjects.moFeature sFeature = new MyMapObjects.moFeature(mSPGLShapeFiles[i].GeometryType,
                            mSPGLShapeFiles[i].Geometries[j], mDBFFiles[i].AttributesList[j]);
                        sFeatures.Add(sFeature);
                    }
                    sMapLayer.Features = sFeatures;
                    sMapLayer.Renderer = sLayer.Renderer;
                    sMapLayer.UpdateExtent();
                    moMapControl1.Layers.RemoveAt(i);
                    moMapControl1.Layers.Insert(i, sMapLayer);
                }
                mNeedToSave = false;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
                return;
            }
        }

        //编辑折点状态右键菜单
        private void RightMenuInEdit()
        {
            if (mOperatingLayerIndex == -1) return;
            moMapRightMenu.Items.Clear();
            moMapRightMenu.Items.Add("完成节点编辑");
            moMapRightMenu.Items.Add("撤销上一步操作");
        }
        //显示编辑节点工具栏
        private void ShowEditStrip(MyMapObjects.moGeometryTypeConstant shapeType)
        {
            EditPointStrip.Enabled = true;
            EditPointStrip.Visible = true;
            MovPointBtn.Enabled = true;
            MovPointBtn.Visible = true;
            MovPointBtn.Checked = true;
            if (shapeType == MyMapObjects.moGeometryTypeConstant.Point) return;
            AddPointBtn.Enabled = true;
            AddPointBtn.Visible = true;
            DelPointBtn.Enabled = true;
            DelPointBtn.Visible = true;
        }
        //显示编辑节点的图形
        private void ShowEditGeometry()
        {
            if (mOperatingLayerIndex == -1) return;
            MyMapObjects.moMapLayer sLayer = moMapControl1.Layers.GetItem(mOperatingLayerIndex);
            if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPolygon)
            {
                MyMapObjects.moMultiPolygon sOriMultiPolygon = (MyMapObjects.moMultiPolygon)sLayer.SelectedFeatures.GetItem(0).Geometry;
                MyMapObjects.moMultiPolygon sDesMultiPolygon = sOriMultiPolygon.Clone();
                mEditingGeometry = sDesMultiPolygon;
            }
            else if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPolyline)
            {
                MyMapObjects.moMultiPolyline sOriMultiPolyline = (MyMapObjects.moMultiPolyline)sLayer.SelectedFeatures.GetItem(0).Geometry;
                MyMapObjects.moMultiPolyline sDesMultiPolyline = sOriMultiPolyline.Clone();
                mEditingGeometry = sDesMultiPolyline;
            }
            else if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.Point)
            {
                MyMapObjects.moPoint sOriPoint = (MyMapObjects.moPoint)sLayer.SelectedFeatures.GetItem(0).Geometry;
                MyMapObjects.moPoint sDesPoint = sOriPoint.Clone();
                mEditingGeometry = sDesPoint;
            }
            else if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPoint)
            {
                MyMapObjects.moPoints sOriPoints = (MyMapObjects.moPoints)sLayer.SelectedFeatures.GetItem(0).Geometry;
                MyMapObjects.moPoints sDesPoints = sOriPoints.Clone();
                mEditingGeometry = sDesPoints;
            }
            moMapControl1.RedrawTrackingShapes();
        }

        //描绘状态右键菜单
        private void RightMenuInSketch()
        {
            if (mOperatingLayerIndex == -1) return;
            moMapRightMenu.Items.Clear();
            moMapRightMenu.Items.Add("结束部件");
            moMapRightMenu.Items.Add("完成草图");
            moMapRightMenu.Items.Add("撤销绘制中节点");
            moMapRightMenu.Items.Add("撤销绘制中部件");
            moMapRightMenu.Items.Add("撤销绘制中草图");
        }
        //隐藏编辑节点工具栏
        private void HideEditStrip()
        {
            EditPointStrip.Enabled = false;
            EditPointStrip.Visible = false;
            MovPointBtn.Enabled = false;
            MovPointBtn.Visible = false;
            MovPointBtn.Checked = false;
            AddPointBtn.Enabled = false;
            AddPointBtn.Visible = false;
            AddPointBtn.Checked = false;
            DelPointBtn.Enabled = false;
            DelPointBtn.Visible = false;
            DelPointBtn.Checked = false;
        }

        // 初始化符号
        private void InitializeSymbols()
        {
            mSelectingBoxSymbol = new MyMapObjects.moSimpleFillSymbol();
            mSelectingBoxSymbol.Color = Color.Transparent;
            mSelectingBoxSymbol.Outline.Color = mSelectBoxColor;
            mSelectingBoxSymbol.Outline.Size = mSelectBoxWidth;
            mZoomBoxSymbol = new MyMapObjects.moSimpleFillSymbol();
            mZoomBoxSymbol.Color = Color.Transparent;
            mZoomBoxSymbol.Outline.Color = mZoomBoxColor;
            mZoomBoxSymbol.Outline.Size = mZoomBoxWidth;
            mMovingPolygonSymbol = new MyMapObjects.moSimpleFillSymbol();
            mMovingPolygonSymbol.Color = Color.Transparent;
            mMovingPolygonSymbol.Outline.Color = Color.Black;
            mMovingPolylineSymbol = new MyMapObjects.moSimpleLineSymbol();
            mMovingPolylineSymbol.Color = Color.Black;
            mMovingPointSymbol = new MyMapObjects.moSimpleMarkerSymbol();
            mMovingPointSymbol.Color = Color.Black;
            mEditingPolygonSymbol = new MyMapObjects.moSimpleFillSymbol();
            mEditingPolygonSymbol.Color = Color.Transparent;
            mEditingPolygonSymbol.Outline.Color = Color.DarkGreen;
            mEditingPolygonSymbol.Outline.Size = 0.53;
            mEditingPolylineSymbol = new MyMapObjects.moSimpleLineSymbol();
            mEditingPolylineSymbol.Color = Color.DarkGreen;
            mEditingPolylineSymbol.Size = 0.53;
            mEditingVertexSymbol = new MyMapObjects.moSimpleMarkerSymbol();
            mEditingVertexSymbol.Color = Color.DarkGreen;
            mEditingVertexSymbol.Style = MyMapObjects.moSimpleMarkerSymbolStyleConstant.SolidSquare;
            mEditingVertexSymbol.Size = 2;
            mElasticSymbol = new MyMapObjects.moSimpleLineSymbol();
            mElasticSymbol.Color = Color.DarkGreen;
            mElasticSymbol.Size = 0.52;
            mElasticSymbol.Style = MyMapObjects.moSimpleLineSymbolStyleConstant.Dash;
        }

        //绘制正在描绘的图形
        private void DrawSketchingShapes(MyMapObjects.moUserDrawingTool drawingTool)
        {
            if (mOperatingLayerIndex == -1) return;
            MyMapObjects.moMapLayer sLayer = moMapControl1.Layers.GetItem(mOperatingLayerIndex);
            if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPolygon)
            {
                if (mSketchingShape == null)
                    return;
                Int32 sPartCount = mSketchingShape.Count;
                //绘制已经描绘完成的部分
                for (Int32 i = 0; i <= sPartCount - 2; i++)
                {
                    drawingTool.DrawPolygon(mSketchingShape[i], mEditingPolygonSymbol);
                }
                //正在描绘的部分（只有一个Part）
                MyMapObjects.moPoints sLastPart = mSketchingShape.Last();
                if (sLastPart.Count >= 2)
                    drawingTool.DrawPolyline(sLastPart, mEditingPolygonSymbol.Outline);
                //绘制所有顶点手柄
                for (Int32 i = 0; i <= sPartCount - 1; i++)
                {
                    MyMapObjects.moPoints sPoints = mSketchingShape[i];
                    drawingTool.DrawPoints(sPoints, mEditingVertexSymbol);
                }
            }
            else if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPolyline)
            {
                if (mSketchingShape == null)
                    return;
                Int32 sPartCount = mSketchingShape.Count;
                //绘制已经描绘完成的部分
                for (Int32 i = 0; i <= sPartCount - 2; i++)
                {
                    drawingTool.DrawPolyline(mSketchingShape[i], mEditingPolylineSymbol);
                }
                //正在描绘的部分（只有一个Part）
                MyMapObjects.moPoints sLastPart = mSketchingShape.Last();
                if (sLastPart.Count >= 2)
                {
                    drawingTool.DrawPolyline(sLastPart, mEditingPolylineSymbol);
                }
                //绘制所有顶点手柄
                for (Int32 i = 0; i <= sPartCount - 1; i++)
                {
                    MyMapObjects.moPoints sPoints = mSketchingShape[i];
                    drawingTool.DrawPoints(sPoints, mEditingVertexSymbol);
                }
            }
            else if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.Point)
            {
                if (mSketchingPoint == null || mSketchingPoint.Count == 0)
                    return;
                drawingTool.DrawPoint(mSketchingPoint[0], mEditingVertexSymbol);
            }
            else if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPoint)
            {
                if (mSketchingPoint == null || mSketchingPoint.Count == 0)
                    return;
                MyMapObjects.moPoints sPoints = new MyMapObjects.moPoints();
                sPoints.AddRange(mSketchingPoint.ToArray());
                drawingTool.DrawPoints(sPoints, mEditingVertexSymbol);
            }
        }

        //绘制正在编辑的图形
        private void DrawEditingShapes(MyMapObjects.moUserDrawingTool drawingTool)
        {
            if (mEditingGeometry == null)
                return;
            if (mEditingGeometry.GetType() == typeof(MyMapObjects.moMultiPolygon))
            {
                MyMapObjects.moMultiPolygon sMultiPolygon = (MyMapObjects.moMultiPolygon)mEditingGeometry;
                //绘制边界
                drawingTool.DrawMultiPolygon(sMultiPolygon, mEditingPolygonSymbol);
                //绘制顶点手柄
                Int32 sPartCount = sMultiPolygon.Parts.Count;
                for (Int32 i = 0; i <= sPartCount - 1; i++)
                {
                    MyMapObjects.moPoints sPoints = sMultiPolygon.Parts.GetItem(i);
                    drawingTool.DrawPoints(sPoints, mEditingVertexSymbol);
                }
            }
            else if (mEditingGeometry.GetType() == typeof(MyMapObjects.moMultiPolyline))
            {
                MyMapObjects.moMultiPolyline sMultiPolyline = (MyMapObjects.moMultiPolyline)mEditingGeometry;
                drawingTool.DrawMultiPolyline(sMultiPolyline, mEditingPolylineSymbol);
                Int32 sPartCount = sMultiPolyline.Parts.Count;
                for (Int32 i = 0; i <= sPartCount - 1; i++)
                {
                    MyMapObjects.moPoints sPoints = sMultiPolyline.Parts.GetItem(i);
                    drawingTool.DrawPoints(sPoints, mEditingVertexSymbol);
                }
            }
            else if (mEditingGeometry.GetType() == typeof(MyMapObjects.moPoint))
            {
                MyMapObjects.moPoint sPoint = (MyMapObjects.moPoint)mEditingGeometry;
                drawingTool.DrawPoint(sPoint, mEditingVertexSymbol);
            }
            else if (mEditingGeometry.GetType() == typeof(MyMapObjects.moPoints))
            {
                MyMapObjects.moPoints sPoints = (MyMapObjects.moPoints)mEditingGeometry;
                drawingTool.DrawPoints(sPoints, mEditingVertexSymbol);
            }
        }

        //选择状态右键菜单操作
        private void RightOperateInSelect(ToolStripItemClickedEventArgs e)
        {
            if (mOperatingLayerIndex == -1) return;
            MyMapObjects.moMapLayer sLayer = moMapControl1.Layers.GetItem(mOperatingLayerIndex);
            if (e.ClickedItem.Text == "删除")
            {
                mNeedToSave = true;
                sLayer.RemoveSelection();
                moMapControl1.RedrawMap();
            }
            if (e.ClickedItem.Text == "复制")
            {
                //CopySelectedFeatures();
                MessageBox.Show("未实现！");
            }
            if (e.ClickedItem.Text == "粘贴")
            {
                //PasteSelectedFeatures();
                MessageBox.Show("未实现！");
            }
            if (e.ClickedItem.Text == "复制并粘贴到新建图层")
            {
                //CopySelectedFeatures();
                //MyMapObjects.moGeometryTypeConstant shapeType = moMapControl1.Layers.GetItem(mOperatingLayerIndex).ShapeType;
                //CreateLayer newLayer = new CreateLayer();
                //newLayer.Owner = this;
                //newLayer.FixedType(shapeType);
                //newLayer.ShowDialog();
                MessageBox.Show("未实现！");
            }
        }

        //描绘状态右键菜单操作
        private void RightOperateInSketch(ToolStripItemClickedEventArgs e)
        {
            if (mOperatingLayerIndex == -1) return;
            MyMapObjects.moMapLayer sLayer = moMapControl1.Layers.GetItem(mOperatingLayerIndex);
            if (e.ClickedItem.Text == "结束部件")
            {
                EndSketchPart(sLayer.ShapeType);
            }
            else if (e.ClickedItem.Text == "完成草图")
            {
                EndSketchGeo(sLayer.ShapeType);
            }
            else if (e.ClickedItem.Text == "撤销绘制中节点")
            {
                DeleteLastSketchPoint(sLayer.ShapeType);
            }
            else if (e.ClickedItem.Text == "撤销绘制中部件")
            {
                DeleteLastSketchPart(sLayer.ShapeType);
            }
            else if (e.ClickedItem.Text == "撤销绘制中草图")
            {
                DeleteLastSketchGeo();
            }
        }

        //编辑节点状态下右键菜单操作
        private void RightOperateInEditPoint(ToolStripItemClickedEventArgs e)
        {
            if (mOperatingLayerIndex == -1) return;
            MyMapObjects.moMapLayer sLayer = moMapControl1.Layers.GetItem(mOperatingLayerIndex);
            if (e.ClickedItem.Text == "完成节点编辑")
            {
                MoveFeatureBtn_Click(new object(), e);
            }
            if (e.ClickedItem.Text == "撤销上一步操作")
            {
                Int32 editOp = mEditPointOperation.Last();
                if (editOp == 1)
                {
                    if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPolygon)
                    {
                        MyMapObjects.moMultiPolygon sMultiPolygon = (MyMapObjects.moMultiPolygon)mEditingGeometry;
                        MyMapObjects.moPoint sPoint = sMultiPolygon.Parts.GetItem(mPastPartIndex.Last()).GetItem(mPastPointIndex.Last());
                        sPoint.X = mEditPointRecord.Last().X;
                        sPoint.Y = mEditPointRecord.Last().Y;
                        sMultiPolygon.UpdateExtent();
                        mEditingGeometry = sMultiPolygon;
                    }
                    else if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPolyline)
                    {
                        MyMapObjects.moMultiPolyline sMultiPolyline = (MyMapObjects.moMultiPolyline)mEditingGeometry;
                        MyMapObjects.moPoint sPoint = sMultiPolyline.Parts.GetItem(mPastPartIndex.Last()).GetItem(mPastPointIndex.Last());
                        sPoint.X = mEditPointRecord.Last().X;
                        sPoint.Y = mEditPointRecord.Last().Y;
                        sMultiPolyline.UpdateExtent();
                        mEditingGeometry = sMultiPolyline;
                    }
                    else if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.Point)
                    {
                        MyMapObjects.moPoint sPoint = (MyMapObjects.moPoint)mEditingGeometry;
                        sPoint.X = mEditPointRecord.Last().X;
                        sPoint.Y = mEditPointRecord.Last().Y;
                        mEditingGeometry = sPoint;
                    }
                    else if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPoint)
                    {
                        MyMapObjects.moPoints sPoints = (MyMapObjects.moPoints)mEditingGeometry;
                        MyMapObjects.moPoint sPoint = sPoints.GetItem(mPastPointIndex.Last());
                        sPoint.X = mEditPointRecord.Last().X;
                        sPoint.Y = mEditPointRecord.Last().Y;
                        sPoints.UpdateExtent();
                        mEditingGeometry = sPoints;
                    }
                    mEditPointOperation.RemoveAt(mEditPointOperation.Count - 1);
                    mEditPointRecord.RemoveAt(mEditPointRecord.Count - 1);
                    mPastPartIndex.RemoveAt(mPastPartIndex.Count - 1);
                    mPastPointIndex.RemoveAt(mPastPointIndex.Count - 1);
                    moMapControl1.RedrawTrackingShapes();
                }
                else if (editOp == 2)
                {
                    if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPolygon)
                    {
                        MyMapObjects.moMultiPolygon sMultiPolygon = (MyMapObjects.moMultiPolygon)mEditingGeometry;
                        sMultiPolygon.Parts.GetItem(mPastPartIndex.Last()).RemoveAt(mPastPointIndex.Last() + 1);
                        sMultiPolygon.UpdateExtent();
                        mEditingGeometry = sMultiPolygon;
                    }
                    else if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPolyline)
                    {
                        MyMapObjects.moMultiPolyline sMultiPolyline = (MyMapObjects.moMultiPolyline)mEditingGeometry;
                        sMultiPolyline.Parts.GetItem(mPastPartIndex.Last()).RemoveAt(mPastPointIndex.Last() + 1);
                        sMultiPolyline.UpdateExtent();
                        mEditingGeometry = sMultiPolyline;
                    }
                    else if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.Point)
                    {
                        ;
                    }
                    else if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPoint)
                    {
                        MyMapObjects.moPoints sPoints = (MyMapObjects.moPoints)mEditingGeometry;
                        sPoints.RemoveAt(mPastPointIndex.Last());
                        sPoints.UpdateExtent();
                        mEditingGeometry = sPoints;
                    }
                    mEditPointOperation.RemoveAt(mEditPointOperation.Count - 1);
                    mPastPartIndex.RemoveAt(mPastPartIndex.Count - 1);
                    mPastPointIndex.RemoveAt(mPastPointIndex.Count - 1);
                    moMapControl1.RedrawTrackingShapes();
                }
                else if (editOp == 3)
                {
                    if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPolygon)
                    {
                        MyMapObjects.moMultiPolygon sMultiPolygon = (MyMapObjects.moMultiPolygon)mEditingGeometry;
                        MyMapObjects.moPoints sPoints = sMultiPolygon.Parts.GetItem(mPastPartIndex.Last());
                        sPoints.Insert(mPastPointIndex.Last(), mEditPointRecord.Last());
                        sMultiPolygon.UpdateExtent();
                        mEditingGeometry = sMultiPolygon;
                    }
                    else if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPolyline)
                    {
                        MyMapObjects.moMultiPolyline sMultiPolyline = (MyMapObjects.moMultiPolyline)mEditingGeometry;
                        MyMapObjects.moPoints sPoints = sMultiPolyline.Parts.GetItem(mPastPartIndex.Last());
                        sPoints.Insert(mPastPointIndex.Last(), mEditPointRecord.Last());
                        sMultiPolyline.UpdateExtent();
                        mEditingGeometry = sMultiPolyline;
                    }
                    else if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.Point)
                    {
                        ;
                    }
                    else if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPoint)
                    {
                        MyMapObjects.moPoints sPoints = (MyMapObjects.moPoints)mEditingGeometry;
                        sPoints.Insert(mPastPointIndex.Last(), mEditPointRecord.Last());
                        sPoints.UpdateExtent();
                        mEditingGeometry = sPoints;
                    }
                    mEditPointOperation.RemoveAt(mEditPointOperation.Count - 1);
                    mEditPointRecord.RemoveAt(mEditPointRecord.Count - 1);
                    mPastPartIndex.RemoveAt(mPastPartIndex.Count - 1);
                    mPastPointIndex.RemoveAt(mPastPointIndex.Count - 1);
                    moMapControl1.RedrawTrackingShapes();
                }
            }
        }

        //删除上一个节点
        private void DeleteLastSketchPoint(MyMapObjects.moGeometryTypeConstant shapeType)
        {
            mNeedToSave = true;
            if (shapeType == MyMapObjects.moGeometryTypeConstant.MultiPolygon)
            {
                Int32 pointsNum = mSketchingShape.Last().Count;
                if (pointsNum == 0)
                {
                    Int32 partsNum = mSketchingShape.Count;
                    if (partsNum > 1)
                    {
                        mSketchingShape.RemoveAt(partsNum - 1);
                        Int32 newPointsNum = mSketchingShape.Last().Count;
                        mSketchingShape.Last().RemoveAt(newPointsNum - 1);
                    }
                    else
                    {
                        return;
                    }
                }
                else mSketchingShape.Last().RemoveAt(pointsNum - 1);
                moMapControl1.RedrawTrackingShapes();
            }
            else if (shapeType == MyMapObjects.moGeometryTypeConstant.MultiPolyline)
            {
                Int32 pointsNum = mSketchingShape.Last().Count;
                if (pointsNum == 0)
                {
                    Int32 partsNum = mSketchingShape.Count;
                    if (partsNum > 1)
                    {
                        mSketchingShape.RemoveAt(partsNum - 1);
                        Int32 newPointsNum = mSketchingShape.Last().Count;
                        mSketchingShape.Last().RemoveAt(newPointsNum - 1);
                    }
                    else
                    {
                        return;
                    }
                }
                else mSketchingShape.Last().RemoveAt(pointsNum - 1);
                moMapControl1.RedrawTrackingShapes();
            }
            else if (shapeType == MyMapObjects.moGeometryTypeConstant.Point)
            {
                ;
            }
            else if (shapeType == MyMapObjects.moGeometryTypeConstant.MultiPoint)
            {
                Int32 pointsNum = mSketchingPoint.Count;
                if (pointsNum == 0) return;
                mSketchingPoint.RemoveAt(pointsNum - 1);
                moMapControl1.RedrawTrackingShapes();
            }
        }

        //撤销上一个部件
        private void DeleteLastSketchPart(MyMapObjects.moGeometryTypeConstant shapeType)
        {
            mNeedToSave = true;
            if (shapeType == MyMapObjects.moGeometryTypeConstant.MultiPolygon)
            {
                Int32 pointsNum = mSketchingShape.Last().Count;
                Int32 partsNum = mSketchingShape.Count;
                if (partsNum == 1) InitializeSketchingShape();
                else
                {
                    if (pointsNum == 0)
                    {
                        if (partsNum == 2) InitializeSketchingShape();
                        else mSketchingShape.RemoveAt(partsNum - 1);
                    }
                    else mSketchingShape.RemoveAt(partsNum - 1);
                }
                moMapControl1.RedrawTrackingShapes();
            }
            else if (shapeType == MyMapObjects.moGeometryTypeConstant.MultiPolyline)
            {
                Int32 pointsNum = mSketchingShape.Last().Count;
                Int32 partsNum = mSketchingShape.Count;
                if (partsNum == 1) InitializeSketchingShape();
                else
                {
                    if (pointsNum == 0)
                    {
                        if (partsNum == 2) InitializeSketchingShape();
                        else mSketchingShape.RemoveAt(partsNum - 1);
                    }
                    else mSketchingShape.RemoveAt(partsNum - 1);
                }
                moMapControl1.RedrawTrackingShapes();
            }
            else if (shapeType == MyMapObjects.moGeometryTypeConstant.Point)
            {
                ;
            }
            else if (shapeType == MyMapObjects.moGeometryTypeConstant.MultiPoint)
            {
                Int32 pointsNum = mSketchingPoint.Count;
                if (pointsNum == 0) return;
                mSketchingPoint.RemoveAt(pointsNum - 1);
                moMapControl1.RedrawTrackingShapes();
            }
        }

        //撤销上一个草图
        private void DeleteLastSketchGeo()
        {
            mNeedToSave = true;
            InitializeSketchingShape();
            moMapControl1.RedrawTrackingShapes();
        }







        #endregion

        #region treeView右击菜单

        private void 移除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mEditMoMap == true && mOperatingLayerIndex == mLastOpLayerIndex)
            {
                return;
            }
            else
            {
                moMapControl1.Layers.RemoveAt(mLastOpLayerIndex);
                mSPGLShapeFiles.RemoveAt(mLastOpLayerIndex);
                mDBFFiles.RemoveAt(mLastOpLayerIndex);
                mLastOpLayerIndex = -1;
                RefreshLayersTree();
                moMapControl1.RedrawMap();
            }
        }

        private void 上移至顶层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mLastOpLayerIndex == 0)
                return;

            MyMapObjects.moMapLayer tempLayer = moMapControl1.Layers.GetItem(mLastOpLayerIndex);
            moMapControl1.Layers.MoveTo(mLastOpLayerIndex, 0);

            DataIOTools.spglShpFileManager spglshp1 = mSPGLShapeFiles[mLastOpLayerIndex];
            mSPGLShapeFiles.RemoveAt(mLastOpLayerIndex);
            mSPGLShapeFiles.Insert(0, spglshp1);

            DataIOTools.dbfFileManager spgdbf1 = mDBFFiles[mLastOpLayerIndex];
            mDBFFiles.RemoveAt(mLastOpLayerIndex);
            mDBFFiles.Insert(0, spgdbf1);

            RefreshLayersTree();
            moMapControl1.RedrawMap();
        }

        private void 上移一层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mLastOpLayerIndex == 0)
                return;

            MyMapObjects.moMapLayer tempLayer = moMapControl1.Layers.GetItem(mLastOpLayerIndex);
            moMapControl1.Layers.MoveTo(mLastOpLayerIndex, mLastOpLayerIndex - 1);

            DataIOTools.spglShpFileManager spglshp1 = mSPGLShapeFiles[mLastOpLayerIndex];
            mSPGLShapeFiles.RemoveAt(mLastOpLayerIndex);
            mSPGLShapeFiles.Insert(mLastOpLayerIndex - 1, spglshp1);

            DataIOTools.dbfFileManager spgdbf1 = mDBFFiles[mLastOpLayerIndex];
            mDBFFiles.RemoveAt(mLastOpLayerIndex);
            mDBFFiles.Insert(mLastOpLayerIndex - 1, spgdbf1);

            RefreshLayersTree();
            moMapControl1.RedrawMap();
        }

        private void 下一一层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mLastOpLayerIndex == treeView1.Nodes.Count - 1)
                return;

            MyMapObjects.moMapLayer tempLayer = moMapControl1.Layers.GetItem(mLastOpLayerIndex);
            moMapControl1.Layers.MoveTo(mLastOpLayerIndex, mLastOpLayerIndex + 1);

            DataIOTools.spglShpFileManager spglshp1 = mSPGLShapeFiles[mLastOpLayerIndex];
            mSPGLShapeFiles.RemoveAt(mLastOpLayerIndex);
            mSPGLShapeFiles.Insert(mLastOpLayerIndex + 1, spglshp1);

            DataIOTools.dbfFileManager spgdbf1 = mDBFFiles[mLastOpLayerIndex];
            mDBFFiles.RemoveAt(mLastOpLayerIndex);
            mDBFFiles.Insert(mLastOpLayerIndex + 1, spgdbf1);

            RefreshLayersTree();
            moMapControl1.RedrawMap();
        }

        private void 下移至底层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mLastOpLayerIndex == treeView1.Nodes.Count - 1)
                return;

            MyMapObjects.moMapLayer tempLayer = moMapControl1.Layers.GetItem(mLastOpLayerIndex);
            moMapControl1.Layers.MoveTo(mLastOpLayerIndex, treeView1.Nodes.Count - 1);

            DataIOTools.spglShpFileManager spglshp1 = mSPGLShapeFiles[mLastOpLayerIndex];
            mSPGLShapeFiles.RemoveAt(mLastOpLayerIndex);
            mSPGLShapeFiles.Insert(treeView1.Nodes.Count - 1, spglshp1);

            DataIOTools.dbfFileManager spgdbf1 = mDBFFiles[mLastOpLayerIndex];
            mDBFFiles.RemoveAt(mLastOpLayerIndex);
            mDBFFiles.Insert(treeView1.Nodes.Count - 1, spgdbf1);

            RefreshLayersTree();
            moMapControl1.RedrawMap();
        }

        private void 缩放至图层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyMapObjects.moMapLayer tempLayer = moMapControl1.Layers.GetItem(mLastOpLayerIndex);
            MyMapObjects.moRectangle sExtent = tempLayer.Extent;
            moMapControl1.ZoomToExtent(sExtent);
        }

        private void 打开属性表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_table form_Table = new Form_table(this, mLastOpLayerIndex);
            form_Table.Owner = this;
            form_Table.Name = moMapControl1.Layers.GetItem(mLastOpLayerIndex).Name;
            form_Table.Show();
            form_Table.SetDesktopLocation(this.Location.X + (this.Width - form_Table.Width) / 2, this.Location.Y + (this.Height - form_Table.Height) / 2);
            form_Table.Refresh_dataform_select();
            All_tables.Add(form_Table);
            form_Table.Windows_index = Fid_table_windows;
            Fid_table_windows++;
        }

        private void 另存为SPGLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            string filepath, sPath;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = @"自定义图层(*.spgl)|*.spgl";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                filepath = saveFileDialog.FileName;
                mSPGLShapeFiles[mLastOpLayerIndex].SaveToFile(filepath);
                sPath = filepath.Substring(0, filepath.IndexOf(".spgl")) + ".spgdbf";
                mDBFFiles[mLastOpLayerIndex].SaveToFile(sPath);
            }
        }

        private void 图层渲染ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mIsInRenderer = false;
            MyMapObjects.moMapLayer sLayer = moMapControl1.Layers.GetItem(mLastOpLayerIndex);   //待渲染的图层
            if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.Point || sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPoint)
            {
                mFormPointRenderer = new Form_PointRenderer(moMapControl1.Layers.GetItem(mLastOpLayerIndex));
                mFormPointRenderer.Owner = this;
                mFormPointRenderer.ShowDialog();
                if (mIsInRenderer == false)
                    return;
                //简单渲染
                if (mPointRendererMode == 0)
                {
                    MyMapObjects.moSimpleRenderer sRenderer = new MyMapObjects.moSimpleRenderer();
                    MyMapObjects.moSimpleMarkerSymbol sSymbol = new MyMapObjects.moSimpleMarkerSymbol();
                    sSymbol.Style = (MyMapObjects.moSimpleMarkerSymbolStyleConstant)mPointSymbolStyle;
                    sSymbol.Color = mPointSimpleRendererColor;
                    sSymbol.Size = mPointSimpleRendererSize;
                    sRenderer.Symbol = sSymbol;
                    sLayer.Renderer = sRenderer;
                    moMapControl1.RedrawMap();
                }
                //唯一值渲染
                else if (mPointRendererMode == 1)
                {
                    MyMapObjects.moUniqueValueRenderer sRenderer = new MyMapObjects.moUniqueValueRenderer();
                    sRenderer.Field = sLayer.AttributeFields.GetItem(mPointUniqueFieldIndex).Name;
                    List<string> sValues = new List<string>();
                    Int32 sFeaturesCount = sLayer.Features.Count;
                    for (Int32 i = 0; i < sFeaturesCount; i++)
                    {
                        string sValue = Convert.ToString(sLayer.Features.GetItem(i).Attributes.GetItem(mPointUniqueFieldIndex));
                        sValues.Add(sValue);
                    }
                    //去除重复
                    sValues = sValues.Distinct().ToList();
                    //生成符号
                    Int32 sValueCount = sValues.Count;
                    for (Int32 i = 0; i < sValueCount; i++)
                    {
                        MyMapObjects.moSimpleMarkerSymbol sSymbol = new MyMapObjects.moSimpleMarkerSymbol();
                        sSymbol.Style = (MyMapObjects.moSimpleMarkerSymbolStyleConstant)mPointSymbolStyle;
                        sSymbol.Size = mPointSimpleRendererSize;
                        sRenderer.AddUniqueValue(sValues[i], sSymbol);
                    }
                    sRenderer.DefaultSymbol = new MyMapObjects.moSimpleMarkerSymbol();
                    sLayer.Renderer = sRenderer;
                    moMapControl1.RedrawMap();
                }
                //分级渲染
                else if (mPointRendererMode == 2)
                {
                    MyMapObjects.moClassBreaksRenderer sRenderer = new MyMapObjects.moClassBreaksRenderer();
                    sRenderer.Field = sLayer.AttributeFields.GetItem(mPointClassBreaksFieldIndex).Name;
                    List<double> sValues = new List<double>();
                    Int32 sFeatrueCount = sLayer.Features.Count;
                    Int32 sFieldIndex = sLayer.AttributeFields.FindField(sRenderer.Field);
                    MyMapObjects.moValueTypeConstant sValueType = sLayer.AttributeFields.GetItem(sFieldIndex).ValueType;
                    if (sValueType == MyMapObjects.moValueTypeConstant.dText)
                    {
                        MessageBox.Show("该字段不是数值字段，不支持分级渲染！");
                        return;
                    }
                    try
                    {
                        for (Int32 i = 0; i < sFeatrueCount; i++)
                        {
                            double sValue = Convert.ToDouble(sLayer.Features.GetItem(i).Attributes.GetItem(sFieldIndex));
                            sValues.Add(sValue);
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("该字段不是数值字段，不支持分级渲染！");
                        return;
                    }
                    double sMinValue = sValues.Min();
                    double sMaxValue = sValues.Max();
                    for (Int32 i = 0; i < mPointClassBreaksNum; i++)
                    {
                        double sValue = sMinValue + (sMaxValue - sMinValue) * (i + 1) / mPointClassBreaksNum;
                        MyMapObjects.moSimpleMarkerSymbol sSymbol = new MyMapObjects.moSimpleMarkerSymbol();
                        sSymbol.Color = mPointClassBreaksRendererColor;
                        sSymbol.Style = (MyMapObjects.moSimpleMarkerSymbolStyleConstant)mPointSymbolStyle;
                        sRenderer.AddBreakValue(sValue, sSymbol);
                    }
                    double sMinSize = mPointClassBreaksRendererMinSize;
                    double sMaxSize = mPointClassBreaksRendererMaxSize;
                    sRenderer.RampSize(sMinSize, sMaxSize);
                    sRenderer.DefaultSymbol = new MyMapObjects.moSimpleMarkerSymbol();
                    sLayer.Renderer = sRenderer;
                    moMapControl1.RedrawMap();
                }
            }
            else if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPolyline)
            {
                mFormPolylineRenderer = new Form_PolylineRenderer(moMapControl1.Layers.GetItem(mLastOpLayerIndex));
                mFormPolylineRenderer.Owner = this;
                mFormPolylineRenderer.ShowDialog();
                if (mIsInRenderer == false)
                    return;
                //简单渲染
                if (mPolylineRendererMode == 0)
                {
                    MyMapObjects.moSimpleRenderer sRenderer = new MyMapObjects.moSimpleRenderer();
                    MyMapObjects.moSimpleLineSymbol sSymbol = new MyMapObjects.moSimpleLineSymbol();
                    sSymbol.Style = (MyMapObjects.moSimpleLineSymbolStyleConstant)mPolylineSymbolStyle;//传参修改
                    sSymbol.Color = mPolylineSimpleRendererColor;//修改颜色
                    sSymbol.Size = mPolylineSimpleRendererSize;//修改尺寸
                    sRenderer.Symbol = sSymbol;
                    sLayer.Renderer = sRenderer;
                    moMapControl1.RedrawMap();
                }
                //唯一值渲染
                else if (mPolylineRendererMode == 1)
                {
                    MyMapObjects.moUniqueValueRenderer sRenderer = new MyMapObjects.moUniqueValueRenderer();
                    sRenderer.Field = sLayer.AttributeFields.GetItem(mPolylineUniqueFieldIndex).Name;
                    List<string> sValues = new List<string>();
                    Int32 sFeatrueCount = sLayer.Features.Count;
                    for (Int32 i = 0; i < sFeatrueCount; i++)
                    {
                        string sValue = Convert.ToString(sLayer.Features.GetItem(i).Attributes.GetItem(mPolylineUniqueFieldIndex));
                        sValues.Add(sValue);
                    }
                    //去除重复
                    sValues = sValues.Distinct().ToList();
                    //生成符号
                    Int32 sValueCount = sValues.Count;
                    for (Int32 i = 0; i < sValueCount; i++)
                    {
                        MyMapObjects.moSimpleLineSymbol sSymbol = new MyMapObjects.moSimpleLineSymbol();
                        sSymbol.Style = (MyMapObjects.moSimpleLineSymbolStyleConstant)mPolylineSymbolStyle;//修改样式
                        sSymbol.Size = mPolylineUniqueRendererSize;//修改尺寸
                        sRenderer.AddUniqueValue(sValues[i], sSymbol);
                    }
                    sRenderer.DefaultSymbol = new MyMapObjects.moSimpleLineSymbol();
                    sLayer.Renderer = sRenderer;
                    moMapControl1.RedrawMap();
                }
                //分级渲染
                else if (mPolylineRendererMode == 2)
                {
                    MyMapObjects.moClassBreaksRenderer sRenderer = new MyMapObjects.moClassBreaksRenderer();
                    sRenderer.Field = sLayer.AttributeFields.GetItem(mPolylineClassBreaksFieldIndex).Name;
                    List<double> sValues = new List<double>();
                    Int32 sFeatrueCount = sLayer.Features.Count;
                    Int32 sFieldIndex = sLayer.AttributeFields.FindField(sRenderer.Field);
                    MyMapObjects.moValueTypeConstant sValueType = sLayer.AttributeFields.GetItem(sFieldIndex).ValueType;
                    if (sValueType == MyMapObjects.moValueTypeConstant.dText)
                    {
                        MessageBox.Show("该字段不是数值字段，不支持分级渲染！");
                        return;
                    }
                    try
                    {
                        for (Int32 i = 0; i < sFeatrueCount; i++)
                        {
                            double sValue = Convert.ToDouble(sLayer.Features.GetItem(i).Attributes.GetItem(sFieldIndex));
                            sValues.Add(sValue);
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("该字段不是数值字段，不支持分级渲染！");
                        return;
                    }

                    double sMinValue = sValues.Min();
                    double sMaxValue = sValues.Max();
                    for (Int32 i = 0; i < mPolylineClassBreaksNum; i++)
                    {
                        double sValue = sMinValue + (sMaxValue - sMinValue) * (i + 1) / mPolylineClassBreaksNum;
                        MyMapObjects.moSimpleLineSymbol sSymbol = new MyMapObjects.moSimpleLineSymbol();
                        sSymbol.Color = mPolylineClassBreaksRendererColor;
                        sSymbol.Style = (MyMapObjects.moSimpleLineSymbolStyleConstant)mPolylineSymbolStyle;
                        sRenderer.AddBreakValue(sValue, sSymbol);
                    }
                    double sMinSize = mPolylineClassBreaksRendererMinSize;
                    double sMaxSize = mPolylineClassBreaksRendererMaxSize;
                    sRenderer.RampSize(sMinSize, sMaxSize);
                    sRenderer.DefaultSymbol = new MyMapObjects.moSimpleLineSymbol();
                    sLayer.Renderer = sRenderer;
                    moMapControl1.RedrawMap();
                }
            }
            else if (sLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPolygon)
            {
                mFormPolygonRenderer = new Form_PolygonRenderer(moMapControl1.Layers.GetItem(mLastOpLayerIndex));
                mFormPolygonRenderer.Owner = this;
                mFormPolygonRenderer.ShowDialog();
                if (mIsInRenderer == false)
                {
                    return;
                }
                //简单渲染
                if (mPolygonRendererMode == 0)
                {
                    MyMapObjects.moSimpleRenderer sRenderer = new MyMapObjects.moSimpleRenderer();
                    MyMapObjects.moSimpleFillSymbol sSymbol = new MyMapObjects.moSimpleFillSymbol();
                    sSymbol.Color = mPolygonSimpleRendererColor;
                    sSymbol.OutlineColor = mOutlineColor;
                    sSymbol.InitializeOutline();
                    sRenderer.Symbol = sSymbol;
                    sLayer.Renderer = sRenderer;
                    moMapControl1.RedrawMap();
                }
                //唯一值渲染
                else if (mPolygonRendererMode == 1)
                {
                    MyMapObjects.moUniqueValueRenderer sRenderer = new MyMapObjects.moUniqueValueRenderer();
                    sRenderer.Field = sLayer.AttributeFields.GetItem(mPolygonUniqueFieldIndex).Name;
                    List<string> sValues = new List<string>();
                    Int32 sFeatrueCount = sLayer.Features.Count;
                    for (Int32 i = 0; i < sFeatrueCount; i++)
                    {
                        string sValue = Convert.ToString(sLayer.Features.GetItem(i).Attributes.GetItem(mPolygonUniqueFieldIndex));
                        sValues.Add(sValue);
                    }
                    //去除重复
                    sValues = sValues.Distinct().ToList();
                    //生成符号
                    Int32 sValueCount = sValues.Count;
                    for (Int32 i = 0; i <= sValueCount - 1; i++)
                    {
                        MyMapObjects.moSimpleFillSymbol sSymbol = new MyMapObjects.moSimpleFillSymbol();
                        sSymbol.OutlineColor = mOutlineColor;
                        sSymbol.InitializeOutline();
                        sRenderer.AddUniqueValue(sValues[i], sSymbol);
                    }
                    sRenderer.DefaultSymbol = new MyMapObjects.moSimpleFillSymbol();
                    sLayer.Renderer = sRenderer;
                    moMapControl1.RedrawMap();
                }
                //分级渲染
                else if (mPolygonRendererMode == 2)
                {
                    MyMapObjects.moClassBreaksRenderer sRenderer = new MyMapObjects.moClassBreaksRenderer();
                    sRenderer.Field = sLayer.AttributeFields.GetItem(mPolygonClassBreaksFieldIndex).Name;
                    List<double> sValues = new List<double>();
                    Int32 sFeatrueCount = sLayer.Features.Count;
                    Int32 sFieldIndex = sLayer.AttributeFields.FindField(sRenderer.Field);
                    MyMapObjects.moValueTypeConstant sValueType = sLayer.AttributeFields.GetItem(sFieldIndex).ValueType;
                    if (sValueType == MyMapObjects.moValueTypeConstant.dText)
                    {
                        MessageBox.Show("该字段不是数值字段，不支持分级渲染！");
                        return;
                    }
                    try
                    {
                        for (Int32 i = 0; i < sFeatrueCount; i++)
                        {
                            double sValue = Convert.ToDouble(sLayer.Features.GetItem(i).Attributes.GetItem(sFieldIndex));
                            sValues.Add(sValue);
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("该字段不是数值字段，不支持分级渲染！");
                        return;
                    }
                    //获取最小最大值并分5级
                    double sMinValue = sValues.Min();
                    double sMaxValue = sValues.Max();
                    for (Int32 i = 0; i < mPolygonClassBreaksNum; i++)
                    {
                        double sValue = sMinValue + (sMaxValue - sMinValue) * (i + 1) / mPolygonClassBreaksNum;
                        MyMapObjects.moSimpleFillSymbol sSymbol = new MyMapObjects.moSimpleFillSymbol();
                        sSymbol.OutlineColor = mOutlineColor;
                        sSymbol.InitializeOutline();
                        sRenderer.AddBreakValue(sValue, sSymbol);
                    }
                    Color sStartColor = mPolygonClassBreaksRendererStartColor;
                    Color sEndColor = mPolygonClassBreaksRendererEndColor;
                    sRenderer.RampColor(sStartColor, sEndColor);
                    sRenderer.DefaultSymbol = new MyMapObjects.moSimpleFillSymbol();
                    sLayer.Renderer = sRenderer;
                    moMapControl1.RedrawMap();
                }
            }
        }

        private void 显示注记ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyMapObjects.moMapLayer sLayer = moMapControl1.Layers.GetItem(mLastOpLayerIndex);//待显示注记的图层
            mFormLabel = new Form_Label(sLayer);
            mFormLabel.Owner = this;
            mFormLabel.ShowDialog();
            MyMapObjects.moLabelRenderer sLabelRenderer = new MyMapObjects.moLabelRenderer();
            sLabelRenderer.Field = sLayer.AttributeFields.GetItem(mLabelFieldIndex).Name;
            sLabelRenderer.TextSymbol.Font = mLabelFont;
            sLabelRenderer.TextSymbol.FontColor = mLabelColor;
            sLabelRenderer.TextSymbol.UseMask = mLabelUseMask;
            sLabelRenderer.LabelFeatures = mLabelVisible;
            sLayer.LabelRenderer = sLabelRenderer;
            moMapControl1.RedrawMap();
        }

        #endregion
    }
}
