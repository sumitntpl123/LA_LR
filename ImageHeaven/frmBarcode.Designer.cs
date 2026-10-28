namespace ImageHeaven
{
    partial class frmBarcode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBarcode));
            this.lstImage = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.sfdUAT = new System.Windows.Forms.SaveFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.deLabel1 = new nControls.deLabel();
            this.deButton1 = new nControls.deButton();
            this.deComboBox2 = new nControls.deComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.deComboBox1 = new nControls.deComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.deCheckBox1 = new nControls.deCheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.grdStatus = new System.Windows.Forms.DataGridView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.deButton20 = new nControls.deButton();
            this.deButton21 = new nControls.deButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdStatus)).BeginInit();
            this.panel4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstImage
            // 
            this.lstImage.BackColor = System.Drawing.Color.Snow;
            this.lstImage.Enabled = false;
            this.lstImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstImage.FormattingEnabled = true;
            this.lstImage.HorizontalScrollbar = true;
            this.lstImage.Location = new System.Drawing.Point(12, 576);
            this.lstImage.Name = "lstImage";
            this.lstImage.Size = new System.Drawing.Size(130, 56);
            this.lstImage.TabIndex = 67;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(179, 592);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(124, 30);
            this.button1.TabIndex = 68;
            this.button1.Text = "&Generate Barcode";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.deButton1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(364, 582);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 69;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(388, 612);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 70;
            this.label2.Text = "label2";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.deCheckBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(913, 68);
            this.panel1.TabIndex = 71;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.deLabel1);
            this.panel2.Controls.Add(this.deButton1);
            this.panel2.Controls.Add(this.deComboBox2);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.deComboBox1);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(252, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(661, 68);
            this.panel2.TabIndex = 6;
            // 
            // deLabel1
            // 
            this.deLabel1.AutoSize = true;
            this.deLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.deLabel1.Location = new System.Drawing.Point(288, 43);
            this.deLabel1.Name = "deLabel1";
            this.deLabel1.Size = new System.Drawing.Size(54, 15);
            this.deLabel1.TabIndex = 11;
            this.deLabel1.Text = "deLabel1";
            // 
            // deButton1
            // 
            this.deButton1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.deButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deButton1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deButton1.Location = new System.Drawing.Point(566, 13);
            this.deButton1.Name = "deButton1";
            this.deButton1.Size = new System.Drawing.Size(75, 23);
            this.deButton1.TabIndex = 10;
            this.deButton1.Text = "&Show Files";
            this.deButton1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deButton1.UseCompatibleTextRendering = true;
            this.deButton1.UseVisualStyleBackColor = true;
            this.deButton1.Click += new System.EventHandler(this.deButton1_Click_1);
            // 
            // deComboBox2
            // 
            this.deComboBox2.BackColor = System.Drawing.Color.White;
            this.deComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.deComboBox2.ForeColor = System.Drawing.Color.Black;
            this.deComboBox2.FormattingEnabled = true;
            this.deComboBox2.Location = new System.Drawing.Point(338, 14);
            this.deComboBox2.Mandatory = true;
            this.deComboBox2.Name = "deComboBox2";
            this.deComboBox2.Size = new System.Drawing.Size(196, 21);
            this.deComboBox2.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(288, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Bundle :";
            // 
            // deComboBox1
            // 
            this.deComboBox1.BackColor = System.Drawing.Color.White;
            this.deComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.deComboBox1.ForeColor = System.Drawing.Color.Black;
            this.deComboBox1.FormattingEnabled = true;
            this.deComboBox1.Location = new System.Drawing.Point(70, 14);
            this.deComboBox1.Mandatory = true;
            this.deComboBox1.Name = "deComboBox1";
            this.deComboBox1.Size = new System.Drawing.Size(196, 21);
            this.deComboBox1.TabIndex = 7;
            this.deComboBox1.Leave += new System.EventHandler(this.deComboBox1_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(20, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Project :";
            // 
            // deCheckBox1
            // 
            this.deCheckBox1.AutoSize = true;
            this.deCheckBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deCheckBox1.Location = new System.Drawing.Point(9, 14);
            this.deCheckBox1.Name = "deCheckBox1";
            this.deCheckBox1.Size = new System.Drawing.Size(155, 21);
            this.deCheckBox1.TabIndex = 4;
            this.deCheckBox1.Text = "Re-Generate Barcode";
            this.deCheckBox1.UseVisualStyleBackColor = true;
            this.deCheckBox1.CheckedChanged += new System.EventHandler(this.deCheckBox1_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 68);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(913, 460);
            this.panel3.TabIndex = 72;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.grdStatus);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(913, 460);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "File Status :";
            // 
            // grdStatus
            // 
            this.grdStatus.AllowUserToAddRows = false;
            this.grdStatus.AllowUserToDeleteRows = false;
            this.grdStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdStatus.Location = new System.Drawing.Point(3, 19);
            this.grdStatus.MultiSelect = false;
            this.grdStatus.Name = "grdStatus";
            this.grdStatus.ReadOnly = true;
            this.grdStatus.RowHeadersWidth = 62;
            this.grdStatus.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdStatus.Size = new System.Drawing.Size(907, 348);
            this.grdStatus.TabIndex = 6;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.groupBox6);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 438);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(913, 49);
            this.panel4.TabIndex = 81;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.deButton20);
            this.groupBox6.Controls.Add(this.deButton21);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox6.Location = new System.Drawing.Point(612, 0);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(301, 49);
            this.groupBox6.TabIndex = 81;
            this.groupBox6.TabStop = false;
            // 
            // deButton20
            // 
            this.deButton20.BackColor = System.Drawing.Color.White;
            this.deButton20.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("deButton20.BackgroundImage")));
            this.deButton20.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.deButton20.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.deButton20.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deButton20.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deButton20.Location = new System.Drawing.Point(6, 11);
            this.deButton20.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.deButton20.Name = "deButton20";
            this.deButton20.Size = new System.Drawing.Size(172, 30);
            this.deButton20.TabIndex = 81;
            this.deButton20.Text = "&Generate Barcode";
            this.deButton20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deButton20.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.deButton20.UseCompatibleTextRendering = true;
            this.deButton20.UseVisualStyleBackColor = false;
            this.deButton20.Click += new System.EventHandler(this.deButton20_Click);
            // 
            // deButton21
            // 
            this.deButton21.BackColor = System.Drawing.Color.White;
            this.deButton21.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("deButton21.BackgroundImage")));
            this.deButton21.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.deButton21.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.deButton21.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deButton21.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deButton21.Location = new System.Drawing.Point(196, 11);
            this.deButton21.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.deButton21.Name = "deButton21";
            this.deButton21.Size = new System.Drawing.Size(87, 30);
            this.deButton21.TabIndex = 82;
            this.deButton21.Text = "A&bort";
            this.deButton21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deButton21.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.deButton21.UseCompatibleTextRendering = true;
            this.deButton21.UseVisualStyleBackColor = false;
            this.deButton21.Click += new System.EventHandler(this.deButton21_Click);
            // 
            // frmBarcode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 487);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lstImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBarcode";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmBarcode";
            this.Load += new System.EventHandler(this.frmBarcode_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdStatus)).EndInit();
            this.panel4.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox lstImage;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.SaveFileDialog sfdUAT;
        private System.Windows.Forms.Panel panel1;
        private nControls.deCheckBox deCheckBox1;
        private System.Windows.Forms.Panel panel2;
        private nControls.deButton deButton1;
        private nControls.deComboBox deComboBox2;
        private System.Windows.Forms.Label label4;
        private nControls.deComboBox deComboBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView grdStatus;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.GroupBox groupBox6;
        private nControls.deButton deButton20;
        private nControls.deButton deButton21;
        private nControls.deLabel deLabel1;
    }
}