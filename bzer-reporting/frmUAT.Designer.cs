
namespace Reporting
{
    partial class frmUAT
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUAT));
            this.deLabel2 = new nControls.deLabel();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.deLabel3 = new nControls.deLabel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.deButton20 = new nControls.deButton();
            this.deButton21 = new nControls.deButton();
            this.deButton1 = new nControls.deButton();
            this.deLabel1 = new nControls.deLabel();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.deLabel4 = new nControls.deLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.grdStatus = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.deComboBox1 = new nControls.deComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportToExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sfdUAT = new System.Windows.Forms.SaveFileDialog();
            this.panel3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdStatus)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // deLabel2
            // 
            this.deLabel2.AutoSize = true;
            this.deLabel2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel2.Location = new System.Drawing.Point(470, 61);
            this.deLabel2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.deLabel2.Name = "deLabel2";
            this.deLabel2.Size = new System.Drawing.Size(44, 15);
            this.deLabel2.TabIndex = 2;
            this.deLabel2.Text = "User ID";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = " ";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(187, 20);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(2);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(206, 23);
            this.dateTimePicker1.TabIndex = 5;
            // 
            // deLabel3
            // 
            this.deLabel3.AutoSize = true;
            this.deLabel3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel3.Location = new System.Drawing.Point(122, 24);
            this.deLabel3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.deLabel3.Name = "deLabel3";
            this.deLabel3.Size = new System.Drawing.Size(62, 15);
            this.deLabel3.TabIndex = 4;
            this.deLabel3.Text = "Date From";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 513);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1000, 60);
            this.panel3.TabIndex = 17;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.deButton20);
            this.groupBox3.Controls.Add(this.deButton21);
            this.groupBox3.Location = new System.Drawing.Point(2, 4);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(994, 46);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            // 
            // deButton20
            // 
            this.deButton20.BackColor = System.Drawing.Color.White;
            this.deButton20.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("deButton20.BackgroundImage")));
            this.deButton20.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.deButton20.Enabled = false;
            this.deButton20.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.deButton20.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deButton20.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deButton20.Location = new System.Drawing.Point(724, 10);
            this.deButton20.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.deButton20.Name = "deButton20";
            this.deButton20.Size = new System.Drawing.Size(157, 30);
            this.deButton20.TabIndex = 12;
            this.deButton20.Text = "&Export to Excel";
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
            this.deButton21.Location = new System.Drawing.Point(896, 11);
            this.deButton21.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.deButton21.Name = "deButton21";
            this.deButton21.Size = new System.Drawing.Size(87, 30);
            this.deButton21.TabIndex = 10;
            this.deButton21.Text = "&Close";
            this.deButton21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deButton21.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.deButton21.UseCompatibleTextRendering = true;
            this.deButton21.UseVisualStyleBackColor = false;
            this.deButton21.Click += new System.EventHandler(this.deButton21_Click);
            // 
            // deButton1
            // 
            this.deButton1.BackColor = System.Drawing.SystemColors.Control;
            this.deButton1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("deButton1.BackgroundImage")));
            this.deButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.deButton1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.deButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deButton1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deButton1.Location = new System.Drawing.Point(755, 54);
            this.deButton1.Name = "deButton1";
            this.deButton1.Size = new System.Drawing.Size(92, 29);
            this.deButton1.TabIndex = 13;
            this.deButton1.Text = "&Search";
            this.deButton1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deButton1.UseCompatibleTextRendering = true;
            this.deButton1.UseVisualStyleBackColor = false;
            this.deButton1.Click += new System.EventHandler(this.deButton1_Click);
            // 
            // deLabel1
            // 
            this.deLabel1.AutoSize = true;
            this.deLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel1.Location = new System.Drawing.Point(457, 24);
            this.deLabel1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.deLabel1.Name = "deLabel1";
            this.deLabel1.Size = new System.Drawing.Size(57, 15);
            this.deLabel1.TabIndex = 0;
            this.deLabel1.Text = "User Type";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = " ";
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(187, 57);
            this.dateTimePicker2.Margin = new System.Windows.Forms.Padding(2);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(206, 23);
            this.dateTimePicker2.TabIndex = 7;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(516, 20);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(206, 23);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.Leave += new System.EventHandler(this.comboBox1_Leave);
            // 
            // deLabel4
            // 
            this.deLabel4.AutoSize = true;
            this.deLabel4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel4.Location = new System.Drawing.Point(138, 61);
            this.deLabel4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.deLabel4.Name = "deLabel4";
            this.deLabel4.Size = new System.Drawing.Size(46, 15);
            this.deLabel4.TabIndex = 6;
            this.deLabel4.Text = "Date To";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 87);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1000, 483);
            this.panel2.TabIndex = 16;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.grdStatus);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(2, 2);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(996, 479);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Report Summary";
            // 
            // grdStatus
            // 
            this.grdStatus.AllowUserToAddRows = false;
            this.grdStatus.AllowUserToDeleteRows = false;
            this.grdStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdStatus.Location = new System.Drawing.Point(2, 18);
            this.grdStatus.MultiSelect = false;
            this.grdStatus.Name = "grdStatus";
            this.grdStatus.ReadOnly = true;
            this.grdStatus.RowHeadersWidth = 62;
            this.grdStatus.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdStatus.Size = new System.Drawing.Size(992, 459);
            this.grdStatus.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.deButton1);
            this.groupBox1.Controls.Add(this.deLabel1);
            this.groupBox1.Controls.Add(this.dateTimePicker2);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.deLabel4);
            this.groupBox1.Controls.Add(this.deLabel2);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.deComboBox1);
            this.groupBox1.Controls.Add(this.deLabel3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(1000, 84);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Criteria";
            // 
            // deComboBox1
            // 
            this.deComboBox1.BackColor = System.Drawing.Color.White;
            this.deComboBox1.ForeColor = System.Drawing.Color.Black;
            this.deComboBox1.FormattingEnabled = true;
            this.deComboBox1.Location = new System.Drawing.Point(516, 57);
            this.deComboBox1.Mandatory = true;
            this.deComboBox1.Margin = new System.Windows.Forms.Padding(2);
            this.deComboBox1.Name = "deComboBox1";
            this.deComboBox1.Size = new System.Drawing.Size(206, 23);
            this.deComboBox1.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1000, 87);
            this.panel1.TabIndex = 15;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToExcelToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 26);
            // 
            // exportToExcelToolStripMenuItem
            // 
            this.exportToExcelToolStripMenuItem.Name = "exportToExcelToolStripMenuItem";
            this.exportToExcelToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exportToExcelToolStripMenuItem.Text = "Export to Excel";
            // 
            // frmUAT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 573);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUAT";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UAT Report";
            this.Load += new System.EventHandler(this.frmUAT_Load);
            this.panel3.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdStatus)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private nControls.deLabel deLabel2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private nControls.deLabel deLabel3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox3;
        private nControls.deButton deButton20;
        private nControls.deButton deButton21;
        private nControls.deButton deButton1;
        private nControls.deLabel deLabel1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.ComboBox comboBox1;
        private nControls.deLabel deLabel4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView grdStatus;
        private System.Windows.Forms.GroupBox groupBox1;
        private nControls.deComboBox deComboBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exportToExcelToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog sfdUAT;
    }
}