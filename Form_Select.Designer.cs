namespace SimpleGIS
{
    partial class Form_Select
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
            this.or = new System.Windows.Forms.Button();
            this.and = new System.Windows.Forms.Button();
            this.like = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnLessOrMore = new System.Windows.Forms.Button();
            this.btnLess = new System.Windows.Forms.Button();
            this.btnMoreOrEqual = new System.Windows.Forms.Button();
            this.btnMore = new System.Windows.Forms.Button();
            this.btnNotEqual = new System.Windows.Forms.Button();
            this.btnEqual = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.SQLTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.UniqueValueList = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.FieldsList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LayerList = new System.Windows.Forms.ComboBox();
            this.bt_cancel = new System.Windows.Forms.Button();
            this.bt_OK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // or
            // 
            this.or.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.or.Location = new System.Drawing.Point(313, 172);
            this.or.Margin = new System.Windows.Forms.Padding(2);
            this.or.Name = "or";
            this.or.Size = new System.Drawing.Size(39, 22);
            this.or.TabIndex = 47;
            this.or.Text = "or";
            this.or.UseVisualStyleBackColor = true;
            // 
            // and
            // 
            this.and.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.and.Location = new System.Drawing.Point(367, 172);
            this.and.Margin = new System.Windows.Forms.Padding(2);
            this.and.Name = "and";
            this.and.Size = new System.Drawing.Size(39, 22);
            this.and.TabIndex = 46;
            this.and.Text = "and";
            this.and.UseVisualStyleBackColor = true;
            // 
            // like
            // 
            this.like.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.like.Location = new System.Drawing.Point(341, 199);
            this.like.Margin = new System.Windows.Forms.Padding(2);
            this.like.Name = "like";
            this.like.Size = new System.Drawing.Size(39, 22);
            this.like.TabIndex = 45;
            this.like.Text = "like";
            this.like.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(261, 297);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(67, 27);
            this.button1.TabIndex = 44;
            this.button1.Text = "应用";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnLessOrMore
            // 
            this.btnLessOrMore.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLessOrMore.Location = new System.Drawing.Point(367, 146);
            this.btnLessOrMore.Margin = new System.Windows.Forms.Padding(2);
            this.btnLessOrMore.Name = "btnLessOrMore";
            this.btnLessOrMore.Size = new System.Drawing.Size(39, 22);
            this.btnLessOrMore.TabIndex = 43;
            this.btnLessOrMore.Text = "<=";
            this.btnLessOrMore.UseVisualStyleBackColor = true;
            // 
            // btnLess
            // 
            this.btnLess.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLess.Location = new System.Drawing.Point(313, 146);
            this.btnLess.Margin = new System.Windows.Forms.Padding(2);
            this.btnLess.Name = "btnLess";
            this.btnLess.Size = new System.Drawing.Size(39, 22);
            this.btnLess.TabIndex = 42;
            this.btnLess.Text = "<";
            this.btnLess.UseVisualStyleBackColor = true;
            // 
            // btnMoreOrEqual
            // 
            this.btnMoreOrEqual.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMoreOrEqual.Location = new System.Drawing.Point(367, 119);
            this.btnMoreOrEqual.Margin = new System.Windows.Forms.Padding(2);
            this.btnMoreOrEqual.Name = "btnMoreOrEqual";
            this.btnMoreOrEqual.Size = new System.Drawing.Size(39, 22);
            this.btnMoreOrEqual.TabIndex = 41;
            this.btnMoreOrEqual.Text = ">=";
            this.btnMoreOrEqual.UseVisualStyleBackColor = true;
            // 
            // btnMore
            // 
            this.btnMore.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMore.Location = new System.Drawing.Point(313, 119);
            this.btnMore.Margin = new System.Windows.Forms.Padding(2);
            this.btnMore.Name = "btnMore";
            this.btnMore.Size = new System.Drawing.Size(39, 22);
            this.btnMore.TabIndex = 40;
            this.btnMore.Text = ">";
            this.btnMore.UseVisualStyleBackColor = true;
            // 
            // btnNotEqual
            // 
            this.btnNotEqual.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnNotEqual.Location = new System.Drawing.Point(367, 93);
            this.btnNotEqual.Margin = new System.Windows.Forms.Padding(2);
            this.btnNotEqual.Name = "btnNotEqual";
            this.btnNotEqual.Size = new System.Drawing.Size(39, 22);
            this.btnNotEqual.TabIndex = 39;
            this.btnNotEqual.Text = "≠";
            this.btnNotEqual.UseVisualStyleBackColor = true;
            // 
            // btnEqual
            // 
            this.btnEqual.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnEqual.Location = new System.Drawing.Point(313, 93);
            this.btnEqual.Margin = new System.Windows.Forms.Padding(2);
            this.btnEqual.Name = "btnEqual";
            this.btnEqual.Size = new System.Drawing.Size(39, 22);
            this.btnEqual.TabIndex = 38;
            this.btnEqual.Text = "=";
            this.btnEqual.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 219);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 12);
            this.label4.TabIndex = 37;
            this.label4.Text = "Select * From Where";
            // 
            // SQLTextBox
            // 
            this.SQLTextBox.Location = new System.Drawing.Point(37, 234);
            this.SQLTextBox.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.SQLTextBox.Multiline = true;
            this.SQLTextBox.Name = "SQLTextBox";
            this.SQLTextBox.Size = new System.Drawing.Size(373, 50);
            this.SQLTextBox.TabIndex = 36;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(165, 50);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 35;
            this.label3.Text = "唯一值";
            // 
            // UniqueValueList
            // 
            this.UniqueValueList.FormattingEnabled = true;
            this.UniqueValueList.ItemHeight = 12;
            this.UniqueValueList.Location = new System.Drawing.Point(165, 66);
            this.UniqueValueList.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.UniqueValueList.Name = "UniqueValueList";
            this.UniqueValueList.Size = new System.Drawing.Size(119, 136);
            this.UniqueValueList.TabIndex = 34;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 50);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 33;
            this.label2.Text = "字段";
            // 
            // FieldsList
            // 
            this.FieldsList.FormattingEnabled = true;
            this.FieldsList.ItemHeight = 12;
            this.FieldsList.Location = new System.Drawing.Point(35, 66);
            this.FieldsList.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.FieldsList.Name = "FieldsList";
            this.FieldsList.Size = new System.Drawing.Size(123, 136);
            this.FieldsList.TabIndex = 32;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 31;
            this.label1.Text = "图层：";
            // 
            // LayerList
            // 
            this.LayerList.FormattingEnabled = true;
            this.LayerList.Location = new System.Drawing.Point(81, 15);
            this.LayerList.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.LayerList.Name = "LayerList";
            this.LayerList.Size = new System.Drawing.Size(330, 20);
            this.LayerList.TabIndex = 30;
            // 
            // bt_cancel
            // 
            this.bt_cancel.Location = new System.Drawing.Point(341, 297);
            this.bt_cancel.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.bt_cancel.Name = "bt_cancel";
            this.bt_cancel.Size = new System.Drawing.Size(67, 27);
            this.bt_cancel.TabIndex = 29;
            this.bt_cancel.Text = "取消";
            this.bt_cancel.UseVisualStyleBackColor = true;
            // 
            // bt_OK
            // 
            this.bt_OK.Location = new System.Drawing.Point(179, 297);
            this.bt_OK.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.bt_OK.Name = "bt_OK";
            this.bt_OK.Size = new System.Drawing.Size(67, 27);
            this.bt_OK.TabIndex = 28;
            this.bt_OK.Text = "确定";
            this.bt_OK.UseVisualStyleBackColor = true;
            // 
            // Form_Select
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 338);
            this.Controls.Add(this.or);
            this.Controls.Add(this.and);
            this.Controls.Add(this.like);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnLessOrMore);
            this.Controls.Add(this.btnLess);
            this.Controls.Add(this.btnMoreOrEqual);
            this.Controls.Add(this.btnMore);
            this.Controls.Add(this.btnNotEqual);
            this.Controls.Add(this.btnEqual);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.SQLTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.UniqueValueList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FieldsList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LayerList);
            this.Controls.Add(this.bt_cancel);
            this.Controls.Add(this.bt_OK);
            this.Name = "Form_Select";
            this.Text = "查找";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button or;
        private System.Windows.Forms.Button and;
        private System.Windows.Forms.Button like;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnLessOrMore;
        private System.Windows.Forms.Button btnLess;
        private System.Windows.Forms.Button btnMoreOrEqual;
        private System.Windows.Forms.Button btnMore;
        private System.Windows.Forms.Button btnNotEqual;
        private System.Windows.Forms.Button btnEqual;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox SQLTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox UniqueValueList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox FieldsList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox LayerList;
        private System.Windows.Forms.Button bt_cancel;
        private System.Windows.Forms.Button bt_OK;
    }
}