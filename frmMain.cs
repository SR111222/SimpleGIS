using MyMapObjects;
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
        private MyMapObjects.moSimpleFillSymbol mMovingPolygonSymbol;       //正在移动的多边形的符号
        private MyMapObjects.moSimpleFillSymbol mEditingPolygonSymbol;      //正在编辑的多边形的符号
        private MyMapObjects.moSimpleMarkerSymbol mEditingVertexSymbol;     //正在编辑的图形的顶点的符号
        private MyMapObjects.moSimpleLineSymbol mElasticSymbol;         //橡皮筋符号
        private bool mShowLngLat = false;       //指示是否显示经纬度

        //（2）与地图操作有关的变量
        /// <summary>
        /// 一般设置为枚举类型。0，无， 1，放大， 2，缩小， 3，漫游， 4，选择， 5，查询， 6，移动， 7，描绘， 8，编辑
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

        private Int32 mLastOpLayerIndex = -1;   //最近一次操作的图层索引

        //（3）与文件操作相关的变量
        public List<DataIOTools.spglShpFileManager> mSPGLShapeFiles = new List<DataIOTools.spglShpFileManager>();   //管理要素文件
        public List<DataIOTools.dbfFileManager> mDBFFiles = new List<DataIOTools.dbfFileManager>();     //管理属性文件

        
        #endregion

        #region 构造函数

        public frmMain()
        {
            InitializeComponent();
        }

        #endregion

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

        //将图层按点线面的方式依次排序
        private void SortLayers()
        {
            int sLayerCount = moMapControl1.Layers.Count;
            MyMapObjects.moLayers sLayers = new MyMapObjects.moLayers();
            if (sLayerCount == 1)
                return;
            for(int i=0;i<sLayerCount;i++)
            {
                if (moMapControl1.Layers.GetItem(i).ShapeType == MyMapObjects.moGeometryTypeConstant.Point)
                    sLayers.Add(moMapControl1.Layers.GetItem(i));
            }
            for (int i = 0; i < sLayerCount; i++)
            {
                if (moMapControl1.Layers.GetItem(i).ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPoint)
                    sLayers.Add(moMapControl1.Layers.GetItem(i));
            }
            for (int i = 0; i < sLayerCount; i++)
            {
                if (moMapControl1.Layers.GetItem(i).ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPolyline)
                    sLayers.Add(moMapControl1.Layers.GetItem(i));
            }
            for (int i = 0; i < sLayerCount; i++)
            {
                if (moMapControl1.Layers.GetItem(i).ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPolygon)
                    sLayers.Add(moMapControl1.Layers.GetItem(i));
            }
            moMapControl1.Layers.Clear();
            for (int i = 0; i < sLayerCount; i++) 
            {
                moMapControl1.Layers.Add(sLayers.GetItem(i));
            }
        }

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


        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            moMapControl1.Layers.GetItem(e.Node.Index).Visible = !moMapControl1.Layers.GetItem(e.Node.Index).Visible;
            moMapControl1.RedrawMap();
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                treeView_RigthMenu.Show(Control.MousePosition);
            }
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
                if(savefile.ShowDialog() == DialogResult.OK)
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

        private void AddLayer_Click(object sender, EventArgs e)
        {
            添加数据ToolStripMenuItem_Click(sender, e);
        }

        //
        private void Save_Click(object sender, EventArgs e)
        {
            导出地图ToolStripMenuItem_Click(sender, e);
        }

        //放大
        private void ZoomIn_Click(object sender, EventArgs e)
        {
            mMapOpStyle = 5;
        }

        //缩小
        private void ZoomOut_Click(object sender, EventArgs e)
        {
            mMapOpStyle = 6;
        }

        private void FullExtent_Click(object sender, EventArgs e)
        {
            moMapControl1.FullExtent();
        }

        private void moMapControl1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void moMapControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void moMapControl1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void moMapControl1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void moMapControl1_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void moMapControl1_AfterTrackingLayerDraw(object sender, moUserDrawingTool drawTool)
        {

        }

        private void moMapControl1_MapScaleChanged(object sender)
        {

        }
    }
}
