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
    public partial class Form_CreateLayer : Form
    {
        #region 字段
        private string _layerName;
        private MyMapObjects.moGeometryTypeConstant _layerType;
        private string _savePath;
        #endregion

        #region 构造函数
        public Form_CreateLayer()
        {
            InitializeComponent();
            comboBox_LayerType.Items.Add("Point");
            comboBox_LayerType.Items.Add("MultiPoint");
            comboBox_LayerType.Items.Add("MultiPolyline");
            comboBox_LayerType.Items.Add("MultiPolygon");
            comboBox_LayerType.SelectedIndex = 0;
        }
        #endregion

        private void button_SavePath_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = @"自定义图层(*.spgl)|*.spgl";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                _savePath = saveFileDialog.FileName;
                textBox_SavePath.Text = _savePath;
            }
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            if(comboBox_LayerType.SelectedIndex == 0)
            {
                _layerType = MyMapObjects.moGeometryTypeConstant.Point;
            }    
            else if(comboBox_LayerType.SelectedIndex == 1)
            {
                _layerType = MyMapObjects.moGeometryTypeConstant.MultiPoint;
            }
            else if (comboBox_LayerType.SelectedIndex == 2)
            {
                _layerType = MyMapObjects.moGeometryTypeConstant.MultiPolyline;
            }
            else if(comboBox_LayerType.SelectedIndex == 3)
            {
                _layerType = MyMapObjects.moGeometryTypeConstant.MultiPolygon;
            }

            _savePath = textBox_SavePath.Text;
            if(_savePath == String.Empty)
            {
                MessageBox.Show(@"请先选择保存路径");
            }
            else
            {
                _layerName = Path.GetFileNameWithoutExtension(_savePath);
                frmMain main = (frmMain)Owner;
                main.GetCreateLayerInfo(_layerName, _layerType, _savePath);
                //if (comboBox_LayerType.Enabled == false) main.PasteToNew();
                this.Close();
            }
        }

        private void button_NO_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
