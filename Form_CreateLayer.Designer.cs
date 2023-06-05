namespace SimpleGIS
{
    partial class Form_CreateLayer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_CreateLayer));
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_LayerType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_SavePath = new System.Windows.Forms.TextBox();
            this.button_SavePath = new System.Windows.Forms.Button();
            this.button_OK = new System.Windows.Forms.Button();
            this.button_NO = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(74, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "图层类型：";
            // 
            // comboBox_LayerType
            // 
            this.comboBox_LayerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_LayerType.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_LayerType.FormattingEnabled = true;
            this.comboBox_LayerType.Location = new System.Drawing.Point(167, 71);
            this.comboBox_LayerType.Name = "comboBox_LayerType";
            this.comboBox_LayerType.Size = new System.Drawing.Size(200, 24);
            this.comboBox_LayerType.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(74, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "存储路径：";
            // 
            // textBox_SavePath
            // 
            this.textBox_SavePath.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_SavePath.Location = new System.Drawing.Point(167, 128);
            this.textBox_SavePath.Name = "textBox_SavePath";
            this.textBox_SavePath.Size = new System.Drawing.Size(200, 26);
            this.textBox_SavePath.TabIndex = 3;
            // 
            // button_SavePath
            // 
            this.button_SavePath.BackColor = System.Drawing.SystemColors.Control;
            this.button_SavePath.Image = ((System.Drawing.Image)(resources.GetObject("button_SavePath.Image")));
            this.button_SavePath.Location = new System.Drawing.Point(382, 128);
            this.button_SavePath.Name = "button_SavePath";
            this.button_SavePath.Size = new System.Drawing.Size(26, 26);
            this.button_SavePath.TabIndex = 4;
            this.button_SavePath.UseVisualStyleBackColor = false;
            this.button_SavePath.Click += new System.EventHandler(this.button_SavePath_Click);
            // 
            // button_OK
            // 
            this.button_OK.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_OK.Location = new System.Drawing.Point(145, 223);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(63, 27);
            this.button_OK.TabIndex = 5;
            this.button_OK.Text = "确认";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_NO
            // 
            this.button_NO.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_NO.Location = new System.Drawing.Point(283, 223);
            this.button_NO.Name = "button_NO";
            this.button_NO.Size = new System.Drawing.Size(63, 27);
            this.button_NO.TabIndex = 6;
            this.button_NO.Text = "取消";
            this.button_NO.UseVisualStyleBackColor = true;
            this.button_NO.Click += new System.EventHandler(this.button_NO_Click);
            // 
            // Form_CreateLayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 311);
            this.Controls.Add(this.button_NO);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.button_SavePath);
            this.Controls.Add(this.textBox_SavePath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox_LayerType);
            this.Controls.Add(this.label1);
            this.Name = "Form_CreateLayer";
            this.Text = "新建图层";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_LayerType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_SavePath;
        private System.Windows.Forms.Button button_SavePath;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_NO;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}