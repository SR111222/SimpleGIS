﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace MyMapObjects
{
    /// <summary>
    /// 矢量图层
    /// </summary>
    public class moMapLayer
    {
        #region 字段

        private moGeometryTypeConstant _ShapeType = moGeometryTypeConstant.Point;   //图层的几何类型，点图层线图层面图层
        private string _Name = "Untitled";  //图层名称
        private bool _Visible = true;   //图层是否可见
        private bool _Selectable = true;    //图层是否参与基于地图的选择
        private string _Description = "";   //描述
        private bool _IsDirty = false;  //图层是否被修改过，方便外部程序使用
        private moFields _AttributeFields = new moFields(); //字段集合对象
        private moFeatures _Features = new moFeatures();    //要素集合对象
        private moFeatures _SelectedFeatures = new moFeatures();    //选择要素集合
        private moRectangle _Extent = new moRectangle(double.MaxValue, double.MinValue, double.MaxValue, double.MinValue);  //图层范围
        private moRenderer _Renderer;   //符号渲染对象
        private moLabelRenderer _LabelRenderer; //注记渲染对象

        #endregion


        #region 构造函数

        public moMapLayer()
        {
            Initialize();
        }

        public moMapLayer(string name, moGeometryTypeConstant shapeType)
        {
            _Name = name;
            _ShapeType = shapeType;
            Initialize();
        }

        public moMapLayer(string name, moGeometryTypeConstant shapeType, moFields attributeFields)
        {
            _Name = name;
            _ShapeType = shapeType;
            _AttributeFields = attributeFields;
            Initialize();
        }

        #endregion


        #region 属性
        /// <summary>
        /// 获取选择的index列表
        /// </summary>
        public List<Int32> SelectIndex
        {
            get { return GetSelectIndex(); }
        }

        /// <summary>
        /// 获取图层的要素几何类型
        /// </summary>
        public moGeometryTypeConstant ShapeType
        {
            get { return _ShapeType; }
        }

        /// <summary>
        /// 获取或设置图层名称
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        /// <summary>
        /// 指示图层是否可见
        /// </summary>
        public bool Visible
        {
            get { return _Visible; }
            set { _Visible = value; }
        }

        /// <summary>
        /// 指示图层是否可以进行选择操作
        /// </summary>
        public bool Selectable
        {
            get { return _Selectable; }
            set { _Selectable = value; }
        }

        /// <summary>
        /// 获取或设置描述
        /// </summary>
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        /// <summary>
        /// 指示图层是否被修改过
        /// </summary>
        public bool IsDirty
        {
            get { return _IsDirty; }
            set { _IsDirty = value; }
        }

        /// <summary>
        /// 获取图层范围
        /// </summary>
        public moRectangle Extent
        {
            get { return _Extent; }
        }


        /// <summary>
        /// 获取或设置要素集合
        /// </summary>
        public moFeatures Features
        {
            get { return _Features; }
            set
            {
                _Features = value;
                CalExtent();
            }
        }

        /// <summary>
        /// 获取或设置选择要素集合
        /// </summary>
        public moFeatures SelectedFeatures
        {
            get { return _SelectedFeatures; }
            set { _SelectedFeatures = value; }
        }

        /// <summary>
        /// 获取属性字段集合
        /// </summary>
        public moFields AttributeFields
        {
            get { return _AttributeFields; }
        }

        /// <summary>
        /// 获取或设置图层渲染
        /// </summary>
        public moRenderer Renderer
        {
            get { return _Renderer; }
            set { _Renderer = value; }
        }

        /// <summary>
        /// 获取或设置图层注记渲染
        /// </summary>
        public moLabelRenderer LabelRenderer
        {
            get { return _LabelRenderer; }
            set { _LabelRenderer = value; }
        }

        #endregion


        #region 方法
        /// <summary>
        /// 移除选择
        /// </summary>
        public void RemoveSelection()
        {
            MyMapObjects.moFeatures sFeatures = new MyMapObjects.moFeatures();
            for (Int32 i = 0; i < _Features.Count; ++i)
            {
                if (SelectIndex.Contains(i) == false)
                {
                    sFeatures.Add(_Features.GetItem(i));
                }
            }
            _Features = sFeatures;
            _SelectedFeatures.Clear();
            UpdateExtent();
        }
        /// <summary>
        /// 更新范围
        /// </summary>
        public void UpdateExtent()
        {
            CalExtent();
        }
        /// <summary>
        /// 获取被选中的要素集合的范围
        /// </summary>
        /// <returns></returns>
        public moRectangle GetSelectionExtent()
        {
            throw new NotImplementedException(); // 课堂不再实现
        }
        /// <summary>
        /// 清除所选择的要素集合
        /// </summary>
        public void ClearSelection()
        {
            _SelectedFeatures.Clear();
        }
        /// <summary>
        /// 根据矩形盒执行搜索，（仅实现最常用的选择模式，若要扩展，需在形参中多传入一个选择模式参数，根据不同模式来进行不同操作）
        /// </summary>
        /// <param name="selectingBox"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public moFeatures SearchByBox(moRectangle selectingBox, double tolerance)
        {
            // 最常用的选择模式，选择点/框和要素矩形框有交集
            moFeatures sSelection = null; 
            if(selectingBox.Width == 0 && selectingBox.Height == 0)
            {
                //按照点选算法
                moPoint sSelectingPoint = new moPoint(selectingBox.MinX, selectingBox.MinY);
                sSelection = SearchFeaturesByPoint(sSelectingPoint, tolerance);
            }
            else if(selectingBox.Width >= 0 && selectingBox.Height >= 0)
            {
                //按照框选算法
                sSelection = SearchFeaturesByBox(selectingBox);
            }
            else
            {
                throw new Exception("Invalid selecting box!");
            }

            //


            return sSelection;
        }
        /// <summary>
        /// 采用指定的方法对要素执行选择，即所有要素集合和选择要素集合之间进行某种集合运算（同样只实现最简单的方法，若要扩展建议selectMethod参数使用枚举）
        /// </summary>
        /// <param name="features">已选择的要素集合</param>
        /// <param name="selectMethod"></param>
        public void ExecuteSelect(moFeatures features, Int32 selectMethod)
        {
            // 此处仅新建集合
            if(selectMethod == 0)
            {
                _SelectedFeatures.Clear();
                Int32 sFeatureCount = features.Count;
                for(Int32 i = 0; i < sFeatureCount; i++)
                {
                    _SelectedFeatures.Add(features.GetItem(i));
                }
            }
            else // 其他方式未实现
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// 获取一个新要素的框架
        /// </summary>
        /// <returns></returns>
        public moFeature GetNewFeature()
        {
            moFeature sFeature = CreateNewFeature();
            return sFeature;
        }
        /// <summary>
        /// 绘制该图层的所有要素
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="extent">当前视域范围</param>
        /// <param name="mapScale">比例尺倒数</param>
        /// <param name="dpm">每米代表的像素数</param>
        /// <param name="mpu">每坐标单位代表的米数</param>
        internal void DrawFeatures(Graphics g, moRectangle extent, double mapScale, double dpm, double mpu)
        {
            //（1）为所有要素配置符号
            SetFeatureSymbols();
            //（2）判断是否位于绘制范围内，如是，则绘制
            Int32 sFeatureCount = _Features.Count;
            for(Int32 i = 0; i < sFeatureCount; i++)
            {
                moFeature sFeature = _Features.GetItem(i);
                if(IsFeatureInExtent(sFeature, extent))
                {
                    moGeometry sGeometry = _Features.GetItem(i).Geometry;
                    moSymbol sSymbol = _Features.GetItem(i).Symbol;
                    moMapDrawingTools.DrawGeometry(g, extent, mapScale, dpm, mpu, sGeometry, sSymbol);
                }
            }
        }
        /// <summary>
        /// 绘制选中的要素
        /// </summary>
        /// <param name="g"></param>
        /// <param name="extent"></param>
        /// <param name="mapScale"></param>
        /// <param name="dpm"></param>
        /// <param name="mpu"></param>
        /// <param name="symbol"></param>
        internal void DrawSelectedFeatures(Graphics g, moRectangle extent, double mapScale, double dpm, double mpu, moSymbol symbol)
        {
            Int32 sFeatureCount = _SelectedFeatures.Count;
            for(Int32 i = 0; i < sFeatureCount; i++)
            {
                moFeature sFeature = _SelectedFeatures.GetItem(i);
                if(IsFeatureInExtent(sFeature, extent))
                {
                    moGeometry sGeometry = sFeature.Geometry;
                    moMapDrawingTools.DrawGeometry(g, extent, mapScale, dpm, mpu, sGeometry, symbol);
                }
            }
        }
        /// <summary>
        /// 绘制注记
        /// </summary>
        /// <param name="g"></param>
        /// <param name="extent"></param>
        /// <param name="mapScale"></param>
        /// <param name="dpm"></param>
        /// <param name="mpu"></param>
        /// <param name="placedLabelExtents">已经放置的注记的集合，用来进行冲突检测（简化，有冲突直接放弃，不再寻找其他位置）</param>
        internal void DrawLabels(Graphics g, moRectangle extent, double mapScale, double dpm, double mpu, List<RectangleF> placedLabelExtents)
        {
            if (_LabelRenderer == null)
                return;
            if (_LabelRenderer.LabelFeatures == false)
                return;
            Int32 sFieldIndex = _AttributeFields.FindField(_LabelRenderer.Field);
            if (sFieldIndex < 0)
                return;
            Int32 sFeatureCount = _Features.Count;
            for (Int32 i = 0; i <= sFeatureCount - 1; i++)
            {
                moFeature sFeature = _Features.GetItem(i);
                if (IsFeatureInExtent(sFeature, extent) == false)
                {   //要素不位于显示范围内，不显示注记
                    continue;
                }
                if (sFeature.Symbol == null)
                {   //要素没有配置符号，不显示注记
                    continue;
                }
                if (IsFeatureSymbolVisible(sFeature) == false)
                {   //要素符号不可见，自然就不显示注记
                    continue;
                }
                string sLabelText = GetValueString(sFeature.Attributes.GetItem(sFieldIndex));
                if (sLabelText == string.Empty)
                {   //注记文本为空，不显示注记
                    continue;
                }
                //根据要素几何类型采用相应的配置方案
                if (sFeature.ShapeType == moGeometryTypeConstant.Point)
                {   //点要素，取点的右上为定位点，但要考虑点符号的大小
                    //（1）复制符号
                    moTextSymbol sTextSymbol;  //最终绘制注记所采用的符号
                    sTextSymbol = _LabelRenderer.TextSymbol.Clone();    //复制符号
                    //（2）计算定位点并设置符号
                    PointF sSrcLabelPoint;   //定位点的屏幕坐标
                    moPoint sPoint = (moPoint)sFeature.Geometry;
                    PointF sSrcPoint = FromMapPoint(extent, mapScale, dpm, mpu, sPoint);    //点要素的屏幕坐标
                    moSimpleMarkerSymbol sMarkerSymbol = (moSimpleMarkerSymbol)sFeature.Symbol;
                    float sSymbolSize = (float)(sMarkerSymbol.Size / 1000 * dpm);        //符号的屏幕尺寸
                    //右上方并设置符号
                    sSrcLabelPoint = new PointF(sSrcPoint.X + sSymbolSize / 2, sSrcPoint.Y - sSymbolSize / 2);
                    sTextSymbol.Alignment = moTextSymbolAlignmentConstant.BottomLeft;
                    //（3）计算注记的屏幕范围矩形
                    RectangleF sLabelExtent = GetLabelExtent(g, dpm, sSrcLabelPoint, sLabelText, sTextSymbol);
                    //（4）冲突检测
                    if (HasConflict(sLabelExtent, placedLabelExtents) == false)
                    {   //没有冲突，则绘制并将当前注记范围矩形加入placedLabelExtents
                        moMapDrawingTools.DrawLabel(g, dpm, sLabelExtent.Location, sLabelText, sTextSymbol);
                        placedLabelExtents.Add(sLabelExtent);
                    }
                }
                else if (sFeature.ShapeType == moGeometryTypeConstant.MultiPolyline)
                {   //线要素，为每个部分的中点配置一个注记
                    //（1）获取符号，线要素无需复制符号
                    moTextSymbol sTextSymbol = _LabelRenderer.TextSymbol;
                    //（2）对每个部分进行配置
                    moMultiPolyline sMultiPolyline = (moMultiPolyline)sFeature.Geometry;
                    Int32 sPartCount = sMultiPolyline.Parts.Count;
                    for (Int32 j = 0; j <= sPartCount - 1; j++)
                    {
                        //获取注记
                        moPoint sMapLabelPoint = moMapTools.GetMidPointOfPolyline(sMultiPolyline.Parts.GetItem(j));
                        PointF sSrcLabelPoint = FromMapPoint(extent, mapScale, dpm, mpu, sMapLabelPoint);
                        //计算注记的屏幕范围矩形
                        RectangleF sLabelExtent = GetLabelExtent(g, dpm, sSrcLabelPoint, sLabelText, _LabelRenderer.TextSymbol);
                        //冲突检测
                        if (HasConflict(sLabelExtent, placedLabelExtents) == false)
                        {   //没有冲突，则绘制并将当前注记范围矩形加入placedLabelExtents
                            moMapDrawingTools.DrawLabel(g, dpm, sLabelExtent.Location, sLabelText, sTextSymbol);
                            placedLabelExtents.Add(sLabelExtent);
                        }
                    }
                }
                else if (sFeature.ShapeType == moGeometryTypeConstant.MultiPolygon)
                {   //面要素，为面积最大的外环及其包含的内环所构成的多边形配置一个注记
                    //（1）获取符号，面要素无需复制符号
                    moTextSymbol sTextSymbol = _LabelRenderer.TextSymbol;
                    //（2）获取注记点
                    moMultiPolygon sMultiPolygon = (moMultiPolygon)sFeature.Geometry;
                    moPoint sMapLabelPoint = moMapTools.GetLabelPointOfMultiPolygon(sMultiPolygon);
                    PointF sSrcLabelPoint = FromMapPoint(extent, mapScale, dpm, mpu, sMapLabelPoint);
                    //（3）计算注记的屏幕范围矩形
                    RectangleF sLabelExtent = GetLabelExtent(g, dpm, sSrcLabelPoint, sLabelText, _LabelRenderer.TextSymbol);
                    //（4）冲突检测
                    if (HasConflict(sLabelExtent, placedLabelExtents) == false)
                    {   //没有冲突，则绘制并将当前注记范围矩形加入placedLabelExtents
                        moMapDrawingTools.DrawLabel(g, dpm, sLabelExtent.Location, sLabelText, sTextSymbol);
                        placedLabelExtents.Add(sLabelExtent);
                    }
                }
                else
                { throw new Exception("Invalid shape type!"); }
            }
        }

        #endregion


        #region 私有函数

        // 初始化
        private void Initialize()
        {
            // (1) 加入_AttributeFields对象的 事件event
            _AttributeFields.FieldAppended += _AttributeFields_FieldAppended;
            _AttributeFields.FieldRemoved += _AttributeFields_FieldRemoved;
            // (2) 初始化符号渲染对象
            InitializeRenderer();
        }

        //有字段被移除时，给所有字段删除一个属性值（监听到有字段被删除）
        private void _AttributeFields_FieldRemoved(object sender, int fieldIndex, moField fieldRemoved)
        {
            //throw new NotImplementedException();
            Int32 sFeatureCount = _Features.Count;
            for(Int32 i = 0; i < sFeatureCount; i++)
            {
                moFeature sFeature = _Features.GetItem(i);
                sFeature.Attributes.RemoveAt(fieldIndex);
            }
        }

        //有字段被加入时，给所有字段增加一个属性值（监听到有字段被加入）
        private void _AttributeFields_FieldAppended(object sender, moField fieldAppended)
        {
            //throw new NotImplementedException();
            //给所有要素增加一个属性值
            Int32 sFeatureCount = _Features.Count;
            for (Int32 i = 0; i <= sFeatureCount - 1; i++)
            {
                moFeature sFeature = _Features.GetItem(i);
                if (fieldAppended.ValueType == moValueTypeConstant.dInt16)
                {
                    Int16 sValue = 0;
                    sFeature.Attributes.Append(sValue);
                }
                else if (fieldAppended.ValueType == moValueTypeConstant.dInt32)
                {
                    Int32 sValue = 0;
                    sFeature.Attributes.Append(sValue);
                }
                else if (fieldAppended.ValueType == moValueTypeConstant.dInt64)
                {
                    Int64 sValue = 0;
                    sFeature.Attributes.Append(sValue);
                }
                else if (fieldAppended.ValueType == moValueTypeConstant.dSingle)
                {
                    float sValue = 0;
                    sFeature.Attributes.Append(sValue);
                }
                else if (fieldAppended.ValueType == moValueTypeConstant.dDouble)
                {
                    double sValue = 0;
                    sFeature.Attributes.Append(sValue);
                }
                else if (fieldAppended.ValueType == moValueTypeConstant.dText)
                {
                    string sValue = string.Empty;
                    sFeature.Attributes.Append(sValue);
                }
                else
                {
                    throw new Exception("Invalid field value type!");
                }
            }
        }

        //初始化符号渲染对象
        private void InitializeRenderer()
        {
            moSimpleRenderer sRenderer = new moSimpleRenderer();
            if (_ShapeType == moGeometryTypeConstant.Point)
            {
                sRenderer.Symbol = new moSimpleMarkerSymbol();
                _Renderer = sRenderer;
            }
            else if (_ShapeType == moGeometryTypeConstant.MultiPolyline)
            {
                sRenderer.Symbol = new moSimpleLineSymbol();
                _Renderer = sRenderer;
            }
            else if (_ShapeType == moGeometryTypeConstant.MultiPolygon)
            {
                sRenderer.Symbol = new moSimpleFillSymbol();
                _Renderer = sRenderer;
            }
            else
            {
                throw new Exception("Invalid shape type!");
            }
        }

        //计算图层范围
        private void CalExtent()
        {
            double sMinX = Double.MaxValue;
            double sMaxX = Double.MinValue;
            double sMinY = Double.MaxValue;
            double sMaxY = Double.MinValue;
            Int32 sFeatureCount = _Features.Count;
            for (Int32 i = 0; i < sFeatureCount; i++)
            {
                moRectangle sExtent = _Features.GetItem(i).GetEnvelope();
                if (sExtent.MinX < sMinX)
                    sMinX = sExtent.MinX;
                if (sExtent.MaxX > sMaxX)
                    sMaxX = sExtent.MaxX;
                if (sExtent.MinY < sMinY)
                    sMinY = sExtent.MinY;
                if (sExtent.MaxY > sMaxY)
                    sMaxY = sExtent.MaxY;
            }
            _Extent = new moRectangle(sMinX, sMaxX, sMinY, sMaxY);
        }

        //根据点搜索要素
        private moFeatures SearchFeaturesByPoint(moPoint point, double tolerance)
        {
            moFeatures sSelectedFeatures = new moFeatures();
            Int32 sFeatureCount = _Features.Count;
            for (Int32 i = 0; i < sFeatureCount; i++)
            {
                if (_ShapeType == moGeometryTypeConstant.Point)
                {
                    moPoint sPoint = (moPoint)_Features.GetItem(i).Geometry;
                    if (moMapTools.IsPointOnPoint(point, sPoint, tolerance) == true)
                    {
                        sSelectedFeatures.Add(_Features.GetItem(i));
                    }
                }
                else if (_ShapeType == moGeometryTypeConstant.MultiPolyline)
                {
                    moMultiPolyline sMultiPolyline = (moMultiPolyline)_Features.GetItem(i).Geometry;
                    if (moMapTools.IsPointOnMultiPolyline(point, sMultiPolyline, tolerance) == true)
                    {
                        sSelectedFeatures.Add(_Features.GetItem(i));
                    }
                }
                else if (_ShapeType == moGeometryTypeConstant.MultiPolygon)
                {
                    moMultiPolygon sMultiPolygon = (moMultiPolygon)_Features.GetItem(i).Geometry;
                    if (moMapTools.IsPointWithinMultiPolygon(point, sMultiPolygon) == true)
                    {
                        sSelectedFeatures.Add(_Features.GetItem(i));
                    }
                }
            }
            return sSelectedFeatures;
        }

        //根据矩形盒搜索要素
        public moFeatures SearchFeaturesByBox(moRectangle selectingBox)
        {
            moFeatures sSelectedFeatures = new moFeatures();
            Int32 sFeatureCount = _Features.Count;
            for (Int32 i = 0; i < sFeatureCount; i++)
            {
                if (_ShapeType == moGeometryTypeConstant.Point)
                {
                    moPoint sPoint = (moPoint)_Features.GetItem(i).Geometry;
                    if (moMapTools.IsPointWithinBox(sPoint, selectingBox) == true)
                    {
                        sSelectedFeatures.Add(_Features.GetItem(i));
                    }
                }
                else if (_ShapeType == moGeometryTypeConstant.MultiPolyline)
                {
                    moMultiPolyline sMultiPolyline = (moMultiPolyline)_Features.GetItem(i).Geometry;
                    if (moMapTools.IsMultiPolylinePartiallyWithinBox(sMultiPolyline, selectingBox) == true)
                    {
                        sSelectedFeatures.Add(_Features.GetItem(i));
                    }
                }
                else if (_ShapeType == moGeometryTypeConstant.MultiPolygon)
                {
                    moMultiPolygon sMultiPolygon = (moMultiPolygon)_Features.GetItem(i).Geometry;
                    if (moMapTools.IsMultiPolygonPartiallyWithinBox(sMultiPolygon, selectingBox) == true)
                    {
                        sSelectedFeatures.Add(_Features.GetItem(i));
                    }
                }
            }
            return sSelectedFeatures;
        }

        //新建一个新要素框架
        private moFeature CreateNewFeature()
        {
            moAttributes sAttributes = new moAttributes();
            Int32 sFieldCount = _AttributeFields.Count;
            for (Int32 i = 0; i < sFieldCount; i++)
            {
                moField sField = _AttributeFields.GetItem(i);
                if (sField.ValueType == moValueTypeConstant.dInt16)
                {
                    Int16 sValue = 0;
                    sAttributes.Append(sValue);
                }
                else if (sField.ValueType == moValueTypeConstant.dInt32)
                {
                    Int32 sValue = 0;
                    sAttributes.Append(sValue);
                }
                else if (sField.ValueType == moValueTypeConstant.dInt64)
                {
                    Int64 sValue = 0;
                    sAttributes.Append(sValue);
                }
                else if (sField.ValueType == moValueTypeConstant.dSingle)
                {
                    float sValue = 0;
                    sAttributes.Append(sValue);
                }
                else if (sField.ValueType == moValueTypeConstant.dDouble)
                {
                    double sValue = 0;
                    sAttributes.Append(sValue);
                }
                else if (sField.ValueType == moValueTypeConstant.dText)
                {
                    String sValue = "";
                    sAttributes.Append(sValue);
                }
                else
                {
                    throw new Exception("Invalid value type!");
                }
            }
            moFeature sFeature = new moFeature(_ShapeType, null, sAttributes);
            return sFeature;
        }

        //为所有要素配置符号
        private void SetFeatureSymbols()
        {
            Int32 sFeatureCount = _Features.Count;
            if (_Renderer.RendererType == moRendererTypeConstant.Simple)
            {
                moSimpleRenderer sRenderer = (moSimpleRenderer)_Renderer;
                for (Int32 i = 0; i < sFeatureCount; i++)
                {
                    _Features.GetItem(i).Symbol = sRenderer.Symbol;
                }
            }
            else if (_Renderer.RendererType == moRendererTypeConstant.UniqueValue)
            {
                moUniqueValueRenderer sRenderer = (moUniqueValueRenderer)_Renderer;
                string sFieldName = sRenderer.Field;
                Int32 sFieldIndex = _AttributeFields.FindField(sFieldName);
                if (sFieldIndex >= 0)
                {
                    for (Int32 i = 0; i < sFeatureCount; i++)
                    {
                        string sValueString = GetValueString(_Features.GetItem(i).Attributes.GetItem(sFieldIndex));
                        _Features.GetItem(i).Symbol = sRenderer.FindSymbol(sValueString);
                    }
                }
                else
                {
                    for (Int32 i = 0; i < sFeatureCount; i++)
                        _Features.GetItem(i).Symbol = null;
                }
            }
            else if (_Renderer.RendererType == moRendererTypeConstant.ClassBreaks)
            {
                moClassBreaksRenderer sRenderer = (moClassBreaksRenderer)_Renderer;
                string sFieldName = sRenderer.Field;
                Int32 sFieldIndex = _AttributeFields.FindField(sFieldName);
                moValueTypeConstant sValueType = _AttributeFields.GetItem(sFieldIndex).ValueType;
                if (sFieldIndex >= 0)
                {
                    for (Int32 i = 0; i < sFeatureCount; i++)
                    {
                        double sValue = 0;
                        if (sValueType == moValueTypeConstant.dInt16)
                            sValue = (Int16)_Features.GetItem(i).Attributes.GetItem(sFieldIndex);
                        else if (sValueType == moValueTypeConstant.dInt32)
                            sValue = (Int32)_Features.GetItem(i).Attributes.GetItem(sFieldIndex);
                        else if (sValueType == moValueTypeConstant.dInt64)
                            sValue = (Int64)_Features.GetItem(i).Attributes.GetItem(sFieldIndex);
                        else if (sValueType == moValueTypeConstant.dSingle)
                            sValue = (float)_Features.GetItem(i).Attributes.GetItem(sFieldIndex);
                        else if (sValueType == moValueTypeConstant.dDouble)
                            sValue = (double)_Features.GetItem(i).Attributes.GetItem(sFieldIndex);
                        else
                            throw new Exception("Invalid value type of field " + sFieldName);

                        _Features.GetItem(i).Symbol = sRenderer.FindSymbol(sValue);
                    }
                }
                else
                {
                    for (Int32 i = 0; i < sFeatureCount; i++)
                        _Features.GetItem(i).Symbol = null;
                }
            }
            else
            {
                throw new Exception("Invalid renderer type!");
            }
        }

        //获取值的字符串形式
        private string GetValueString(object value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.ToString();
        }

        //指定要素是否位于指定范围内，这里仅计算要素外包矩形和范围矩形是否相交
        private bool IsFeatureInExtent(moFeature feature, moRectangle extent)
        {
            moRectangle sRect = feature.GetEnvelope();
            if (sRect.MaxX < extent.MinX || sRect.MinX > extent.MaxX)
            { 
                return false;
            }
            else if (sRect.MaxY < extent.MinY || sRect.MinY > extent.MaxY)
            { 
                return false;
            }
            else
            { 
                return true;
            }
        }

        //要素符号是否可见
        private bool IsFeatureSymbolVisible(moFeature feature)
        {
            moSymbol sSymbol = feature.Symbol;
            if (sSymbol.SymbolType == moSymbolTypeConstant.SimpleMarkerSymbol)
            {
                moSimpleMarkerSymbol sMarkerSymbol = (moSimpleMarkerSymbol)sSymbol;
                return sMarkerSymbol.Visible;
            }
            else if (sSymbol.SymbolType == moSymbolTypeConstant.SimpleLineSymbol)
            {
                moSimpleLineSymbol sLineSymbol = (moSimpleLineSymbol)sSymbol;
                return sLineSymbol.Visible;
            }
            else if (sSymbol.SymbolType == moSymbolTypeConstant.SimpleFillSymbol)
            {
                moSimpleFillSymbol sFillSymbol = (moSimpleFillSymbol)sSymbol;
                return sFillSymbol.Visible;
            }
            else
            { 
                throw new Exception("Invalid symbol type!"); 
            }
        }

        //将地图坐标转换为屏幕坐标
        private PointF FromMapPoint(moRectangle extent, double mapScale, double dpm, double mpu, moPoint point)
        {
            double sOffsetX = extent.MinX, sOffsetY = extent.MaxY;  //获取地图坐标系相对屏幕坐标系的平移量
            PointF sPoint = new PointF();          //屏幕坐标
            sPoint.X = (float)((point.X - sOffsetX) * mpu / mapScale * dpm);
            sPoint.Y = (float)((sOffsetY - point.Y) * mpu / mapScale * dpm);
            return sPoint;
        }
        
        //获取注记的屏幕范围
        private RectangleF GetLabelExtent(Graphics g, double dpm, PointF labelPoint, string labelText, moTextSymbol textSymbol)
        {
            //（1）测量注记大小
            SizeF sLabelSize = g.MeasureString(labelText, textSymbol.Font);     //注记的尺寸
            //（2）计算偏移量
            float sLabelOffsetX, sLabelOffsetY;       //注记偏移量（屏幕坐标），向右、向上位正
            sLabelOffsetX = (float)(textSymbol.OffsetX / 1000 * dpm);
            sLabelOffsetY = (float)(textSymbol.OffsetY / 1000 * dpm);
            //（3）根据布局计算左上点
            PointF sTopLeftPoint = new PointF();        //注记左上点坐标（屏幕坐标）
            if (textSymbol.Alignment == moTextSymbolAlignmentConstant.TopLeft)
            {
                sTopLeftPoint.X = labelPoint.X + sLabelOffsetX;
                sTopLeftPoint.Y = labelPoint.Y - sLabelOffsetY;
            }
            else if (textSymbol.Alignment == moTextSymbolAlignmentConstant.TopCenter)
            {
                sTopLeftPoint.X = labelPoint.X - sLabelSize.Width / 2 + sLabelOffsetX;
                sTopLeftPoint.Y = labelPoint.Y - sLabelOffsetY;
            }
            else if (textSymbol.Alignment == moTextSymbolAlignmentConstant.TopRight)
            {
                sTopLeftPoint.X = labelPoint.X - sLabelSize.Width + sLabelOffsetX;
                sTopLeftPoint.Y = labelPoint.Y - sLabelOffsetY;
            }
            else if (textSymbol.Alignment == moTextSymbolAlignmentConstant.CenterLeft)
            {
                sTopLeftPoint.X = labelPoint.X + sLabelOffsetX;
                sTopLeftPoint.Y = labelPoint.Y - sLabelSize.Height / 2 - sLabelOffsetY;
            }
            else if (textSymbol.Alignment == moTextSymbolAlignmentConstant.CenterCenter)
            {
                sTopLeftPoint.X = labelPoint.X - sLabelSize.Width / 2 + sLabelOffsetX;
                sTopLeftPoint.Y = labelPoint.Y - sLabelSize.Height / 2 - sLabelOffsetY;
            }
            else if (textSymbol.Alignment == moTextSymbolAlignmentConstant.CenterRight)
            {
                sTopLeftPoint.X = labelPoint.X - sLabelSize.Width + sLabelOffsetX;
                sTopLeftPoint.Y = labelPoint.Y - sLabelSize.Height / 2 - sLabelOffsetY;
            }
            else if (textSymbol.Alignment == moTextSymbolAlignmentConstant.BottomLeft)
            {
                sTopLeftPoint.X = labelPoint.X + sLabelOffsetX;
                sTopLeftPoint.Y = labelPoint.Y - sLabelSize.Height - sLabelOffsetY;
            }
            else if (textSymbol.Alignment == moTextSymbolAlignmentConstant.BottomCenter)
            {
                sTopLeftPoint.X = labelPoint.X - sLabelSize.Width / 2 + sLabelOffsetX;
                sTopLeftPoint.Y = labelPoint.Y - sLabelSize.Height - sLabelOffsetY;
            }
            else if (textSymbol.Alignment == moTextSymbolAlignmentConstant.BottomRight)
            {
                sTopLeftPoint.X = labelPoint.X - sLabelSize.Width + sLabelOffsetX;
                sTopLeftPoint.Y = labelPoint.Y - sLabelSize.Height - sLabelOffsetY;
            }
            else
            { 
                throw new Exception("Invalid text symbol alignment!");
            }
            //（4）返回注记范围矩形
            RectangleF sRect = new RectangleF(sTopLeftPoint, sLabelSize);
            return sRect;
        }

        //指定的矩形是否与指定矩形集合内所有矩形存在相交
        private bool HasConflict(RectangleF labelExtent, List<RectangleF> placedLabelExtents)
        {
            Int32 sCount = placedLabelExtents.Count;
            float sMinX1 = labelExtent.X, sMaxX1 = labelExtent.X + labelExtent.Width;
            float sMinY1 = labelExtent.Y, sMaxY1 = labelExtent.Y + labelExtent.Height;
            for (Int32 i = 0; i <= sCount - 1; i++)
            {
                RectangleF sCurExtent = placedLabelExtents[i];
                float sMinX2 = sCurExtent.X, sMaxX2 = sCurExtent.X + sCurExtent.Width;
                float sMinY2 = sCurExtent.Y, sMaxY2 = sCurExtent.Y + sCurExtent.Height;
                if (sMinX1 > sMaxX2 || sMaxX1 < sMinX2)
                { }
                else if (sMinY1 > sMaxY2 || sMaxY1 < sMinY2)
                { }
                else
                { return true; }
            }
            return false;
        }

        private List<Int32> GetSelectIndex()
        {
            List<Int32> selectIndex = new List<Int32>();
            for (Int32 i = 0; i < _SelectedFeatures.Count; ++i)
            {
                for (Int32 j = 0; j < _Features.Count; ++j)
                {
                    if (_Features.GetItem(j) == _SelectedFeatures.GetItem(i))
                    {
                        selectIndex.Add(j);
                        break;
                    }
                }
            }
            return selectIndex;
        }

        #endregion
    }
}
