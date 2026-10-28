
namespace ImageHeaven
{
    partial class frmDelivery
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDelivery));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbBundle = new nControls.deComboBox();
            this.deLabel2 = new nControls.deLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.deTextBox1 = new nControls.deTextBox();
            this.deLabel4 = new nControls.deLabel();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.deLabel3 = new nControls.deLabel();
            this.deLabel1 = new nControls.deLabel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.deLabel6 = new nControls.deLabel();
            this.deLabel7 = new nControls.deLabel();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.deTextBox2 = new nControls.deTextBox();
            this.deLabel5 = new nControls.deLabel();
            this.deLabel8 = new nControls.deLabel();
            this.deLabel9 = new nControls.deLabel();
            this.dateTimePicker3 = new System.Windows.Forms.DateTimePicker();
            this.button2 = new nControls.deButton();
            this.button3 = new nControls.deButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbBundle);
            this.groupBox1.Controls.Add(this.deLabel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(800, 57);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // cmbBundle
            // 
            this.cmbBundle.BackColor = System.Drawing.Color.White;
            this.cmbBundle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBundle.ForeColor = System.Drawing.Color.Black;
            this.cmbBundle.FormattingEnabled = true;
            this.cmbBundle.Location = new System.Drawing.Point(324, 19);
            this.cmbBundle.Mandatory = false;
            this.cmbBundle.Name = "cmbBundle";
            this.cmbBundle.Size = new System.Drawing.Size(196, 21);
            this.cmbBundle.TabIndex = 1;
            this.cmbBundle.TabStop = false;
            this.cmbBundle.Leave += new System.EventHandler(this.cmbBundle_Leave);
            // 
            // deLabel2
            // 
            this.deLabel2.AutoSize = true;
            this.deLabel2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel2.Location = new System.Drawing.Point(229, 21);
            this.deLabel2.Name = "deLabel2";
            this.deLabel2.Size = new System.Drawing.Size(85, 15);
            this.deLabel2.TabIndex = 17;
            this.deLabel2.Text = "Bundle Name :";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox3);
            this.groupBox2.Controls.Add(this.deTextBox2);
            this.groupBox2.Controls.Add(this.deLabel5);
            this.groupBox2.Controls.Add(this.dateTimePicker3);
            this.groupBox2.Controls.Add(this.deLabel8);
            this.groupBox2.Controls.Add(this.deLabel9);
            this.groupBox2.Controls.Add(this.checkBox2);
            this.groupBox2.Controls.Add(this.dateTimePicker2);
            this.groupBox2.Controls.Add(this.deLabel6);
            this.groupBox2.Controls.Add(this.deLabel7);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.deTextBox1);
            this.groupBox2.Controls.Add(this.deLabel4);
            this.groupBox2.Controls.Add(this.dateTimePicker1);
            this.groupBox2.Controls.Add(this.deLabel3);
            this.groupBox2.Controls.Add(this.deLabel1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 57);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(800, 293);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // deTextBox1
            // 
            this.deTextBox1.BackColor = System.Drawing.Color.White;
            this.deTextBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deTextBox1.ForeColor = System.Drawing.Color.Black;
            this.deTextBox1.Location = new System.Drawing.Point(324, 78);
            this.deTextBox1.Mandatory = true;
            this.deTextBox1.Name = "deTextBox1";
            this.deTextBox1.Size = new System.Drawing.Size(196, 23);
            this.deTextBox1.TabIndex = 23;
            // 
            // deLabel4
            // 
            this.deLabel4.AutoSize = true;
            this.deLabel4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel4.Location = new System.Drawing.Point(237, 83);
            this.deLabel4.Name = "deLabel4";
            this.deLabel4.Size = new System.Drawing.Size(77, 15);
            this.deLabel4.TabIndex = 22;
            this.deLabel4.Text = "Delivered To :";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = " ";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(324, 48);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(196, 20);
            this.dateTimePicker1.TabIndex = 21;
            // 
            // deLabel3
            // 
            this.deLabel3.AutoSize = true;
            this.deLabel3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel3.Location = new System.Drawing.Point(232, 52);
            this.deLabel3.Name = "deLabel3";
            this.deLabel3.Size = new System.Drawing.Size(82, 15);
            this.deLabel3.TabIndex = 20;
            this.deLabel3.Text = "Delivery Date :";
            // 
            // deLabel1
            // 
            this.deLabel1.AutoSize = true;
            this.deLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel1.Location = new System.Drawing.Point(124, 21);
            this.deLabel1.Name = "deLabel1";
            this.deLabel1.Size = new System.Drawing.Size(192, 15);
            this.deLabel1.TabIndex = 17;
            this.deLabel1.Text = "Delivered to CHC for DMS upload ?";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(324, 19);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(44, 17);
            this.checkBox1.TabIndex = 18;
            this.checkBox1.Text = "Yes";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Click += new System.EventHandler(this.checkBox1_Click_1);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(324, 125);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(44, 17);
            this.checkBox2.TabIndex = 25;
            this.checkBox2.Text = "Yes";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.Click += new System.EventHandler(this.checkBox2_Click_1);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = " ";
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(324, 154);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(196, 20);
            this.dateTimePicker2.TabIndex = 27;
            // 
            // deLabel6
            // 
            this.deLabel6.AutoSize = true;
            this.deLabel6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel6.Location = new System.Drawing.Point(223, 156);
            this.deLabel6.Name = "deLabel6";
            this.deLabel6.Size = new System.Drawing.Size(91, 15);
            this.deLabel6.TabIndex = 26;
            this.deLabel6.Text = "Uploaded Date :";
            // 
            // deLabel7
            // 
            this.deLabel7.AutoSize = true;
            this.deLabel7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel7.Location = new System.Drawing.Point(227, 125);
            this.deLabel7.Name = "deLabel7";
            this.deLabel7.Size = new System.Drawing.Size(87, 15);
            this.deLabel7.TabIndex = 24;
            this.deLabel7.Text = "Uploaded File ?";
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(324, 195);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(44, 17);
            this.checkBox3.TabIndex = 29;
            this.checkBox3.Text = "Yes";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.Click += new System.EventHandler(this.checkBox3_Click);
            // 
            // deTextBox2
            // 
            this.deTextBox2.BackColor = System.Drawing.Color.White;
            this.deTextBox2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deTextBox2.ForeColor = System.Drawing.Color.Black;
            this.deTextBox2.Location = new System.Drawing.Point(324, 222);
            this.deTextBox2.Mandatory = true;
            this.deTextBox2.Name = "deTextBox2";
            this.deTextBox2.Size = new System.Drawing.Size(196, 23);
            this.deTextBox2.TabIndex = 31;
            // 
            // deLabel5
            // 
            this.deLabel5.AutoSize = true;
            this.deLabel5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel5.Location = new System.Drawing.Point(240, 225);
            this.deLabel5.Name = "deLabel5";
            this.deLabel5.Size = new System.Drawing.Size(72, 15);
            this.deLabel5.TabIndex = 32;
            this.deLabel5.Text = "Challan No :";
            // 
            // deLabel8
            // 
            this.deLabel8.AutoSize = true;
            this.deLabel8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel8.Location = new System.Drawing.Point(232, 258);
            this.deLabel8.Name = "deLabel8";
            this.deLabel8.Size = new System.Drawing.Size(80, 15);
            this.deLabel8.TabIndex = 30;
            this.deLabel8.Text = "Challan Date :";
            // 
            // deLabel9
            // 
            this.deLabel9.AutoSize = true;
            this.deLabel9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel9.Location = new System.Drawing.Point(229, 197);
            this.deLabel9.Name = "deLabel9";
            this.deLabel9.Size = new System.Drawing.Size(83, 15);
            this.deLabel9.TabIndex = 28;
            this.deLabel9.Text = "Raise invoice ?";
            // 
            // dateTimePicker3
            // 
            this.dateTimePicker3.CustomFormat = " ";
            this.dateTimePicker3.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker3.Location = new System.Drawing.Point(324, 256);
            this.dateTimePicker3.Name = "dateTimePicker3";
            this.dateTimePicker3.Size = new System.Drawing.Size(196, 20);
            this.dateTimePicker3.TabIndex = 33;
            // 
            // button2
            // 
            this.button2.AutoSize = true;
            this.button2.BackColor = System.Drawing.SystemColors.Control;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.button2.Location = new System.Drawing.Point(540, 353);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(104, 40);
            this.button2.TabIndex = 17;
            this.button2.Text = "      &Save";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button2.UseCompatibleTextRendering = true;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.AutoSize = true;
            this.button3.BackColor = System.Drawing.SystemColors.Control;
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(670, 352);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(118, 40);
            this.button3.TabIndex = 18;
            this.button3.Text = "     C&lose";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button3.UseCompatibleTextRendering = true;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // frmDelivery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 396);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDelivery";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmDelivery_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private nControls.deComboBox cmbBundle;
        private nControls.deLabel deLabel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private nControls.deLabel deLabel1;
        private nControls.deLabel deLabel3;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private nControls.deLabel deLabel4;
        private nControls.deTextBox deTextBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private nControls.deRadioButton rdoMotherName;
        private nControls.deRadioButton rdoFatherName;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private nControls.deLabel deLabel6;
        private nControls.deLabel deLabel7;
        private System.Windows.Forms.CheckBox checkBox3;
        private nControls.deTextBox deTextBox2;
        private nControls.deLabel deLabel5;
        private nControls.deLabel deLabel8;
        private nControls.deLabel deLabel9;
        private System.Windows.Forms.DateTimePicker dateTimePicker3;
        private nControls.deButton button2;
        private nControls.deButton button3;
    }
}