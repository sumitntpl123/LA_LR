namespace Reporting
{
    partial class frmSite
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSite));
            this.sfdUAT = new System.Windows.Forms.SaveFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.deComboBox1 = new nControls.deComboBox();
            this.deLabel1 = new nControls.deLabel();
            this.deButton1 = new nControls.deButton();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.deLabel2 = new nControls.deLabel();
            this.deLabel3 = new nControls.deLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvBundle = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.grdStatus = new System.Windows.Forms.DataGridView();
            this.cmsDeeds = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.updateDeedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBundle)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdStatus)).BeginInit();
            this.cmsDeeds.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1023, 69);
            this.panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.deComboBox1);
            this.groupBox1.Controls.Add(this.deLabel1);
            this.groupBox1.Controls.Add(this.deButton1);
            this.groupBox1.Controls.Add(this.dateTimePicker2);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.deLabel2);
            this.groupBox1.Controls.Add(this.deLabel3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1023, 69);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Criteria :";
            // 
            // deComboBox1
            // 
            this.deComboBox1.BackColor = System.Drawing.Color.White;
            this.deComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.deComboBox1.ForeColor = System.Drawing.Color.Black;
            this.deComboBox1.FormattingEnabled = true;
            this.deComboBox1.Items.AddRange(new object[] {
            "Data Entry",
            "Scan",
            "QC",
            "Doc Type Association",
            "FQC",
            "Submission",
            "Certification"});
            this.deComboBox1.Location = new System.Drawing.Point(106, 26);
            this.deComboBox1.Mandatory = true;
            this.deComboBox1.Name = "deComboBox1";
            this.deComboBox1.Size = new System.Drawing.Size(196, 23);
            this.deComboBox1.TabIndex = 1;
            // 
            // deLabel1
            // 
            this.deLabel1.AutoSize = true;
            this.deLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel1.Location = new System.Drawing.Point(61, 30);
            this.deLabel1.Name = "deLabel1";
            this.deLabel1.Size = new System.Drawing.Size(39, 15);
            this.deLabel1.TabIndex = 8;
            this.deLabel1.Text = "Filter :";
            // 
            // deButton1
            // 
            this.deButton1.BackColor = System.Drawing.SystemColors.Control;
            this.deButton1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("deButton1.BackgroundImage")));
            this.deButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.deButton1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.deButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deButton1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deButton1.Location = new System.Drawing.Point(905, 24);
            this.deButton1.Name = "deButton1";
            this.deButton1.Size = new System.Drawing.Size(92, 29);
            this.deButton1.TabIndex = 7;
            this.deButton1.Text = "&Search";
            this.deButton1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deButton1.UseCompatibleTextRendering = true;
            this.deButton1.UseVisualStyleBackColor = false;
            this.deButton1.Click += new System.EventHandler(this.deButton1_Click);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = " ";
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(682, 27);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(196, 23);
            this.dateTimePicker2.TabIndex = 5;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = " ";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(406, 27);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(196, 23);
            this.dateTimePicker1.TabIndex = 3;
            // 
            // deLabel2
            // 
            this.deLabel2.AutoSize = true;
            this.deLabel2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel2.Location = new System.Drawing.Point(332, 29);
            this.deLabel2.Name = "deLabel2";
            this.deLabel2.Size = new System.Drawing.Size(68, 15);
            this.deLabel2.TabIndex = 4;
            this.deLabel2.Text = "Date From :";
            // 
            // deLabel3
            // 
            this.deLabel3.AutoSize = true;
            this.deLabel3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel3.Location = new System.Drawing.Point(624, 29);
            this.deLabel3.Name = "deLabel3";
            this.deLabel3.Size = new System.Drawing.Size(52, 15);
            this.deLabel3.TabIndex = 6;
            this.deLabel3.Text = "Date To :";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 69);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1023, 503);
            this.panel2.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgvBundle);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(0, 91);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1023, 412);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "File Summary : ";
            // 
            // dgvBundle
            // 
            this.dgvBundle.AllowUserToAddRows = false;
            this.dgvBundle.AllowUserToDeleteRows = false;
            this.dgvBundle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBundle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBundle.Location = new System.Drawing.Point(3, 18);
            this.dgvBundle.MultiSelect = false;
            this.dgvBundle.Name = "dgvBundle";
            this.dgvBundle.ReadOnly = true;
            this.dgvBundle.RowHeadersWidth = 62;
            this.dgvBundle.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBundle.Size = new System.Drawing.Size(1017, 391);
            this.dgvBundle.TabIndex = 8;
            this.dgvBundle.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvBundle_MouseClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.grdStatus);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1023, 91);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Report Summary : ";
            // 
            // grdStatus
            // 
            this.grdStatus.AllowUserToAddRows = false;
            this.grdStatus.AllowUserToDeleteRows = false;
            this.grdStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdStatus.Location = new System.Drawing.Point(3, 18);
            this.grdStatus.MultiSelect = false;
            this.grdStatus.Name = "grdStatus";
            this.grdStatus.ReadOnly = true;
            this.grdStatus.RowHeadersWidth = 62;
            this.grdStatus.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdStatus.Size = new System.Drawing.Size(1017, 70);
            this.grdStatus.TabIndex = 8;
            this.grdStatus.DoubleClick += new System.EventHandler(this.grdStatus_DoubleClick);
            this.grdStatus.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdStatus_MouseClick);
            // 
            // cmsDeeds
            // 
            this.cmsDeeds.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateDeedToolStripMenuItem});
            this.cmsDeeds.Name = "cmsDeeds";
            this.cmsDeeds.Size = new System.Drawing.Size(153, 26);
            // 
            // updateDeedToolStripMenuItem
            // 
            this.updateDeedToolStripMenuItem.Name = "updateDeedToolStripMenuItem";
            this.updateDeedToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.updateDeedToolStripMenuItem.Text = "&Export to Excel";
            this.updateDeedToolStripMenuItem.Click += new System.EventHandler(this.updateDeedToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.contextMenuStrip1.Name = "cmsDeeds";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem1.Text = "&Export to Excel";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // frmSite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1023, 572);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmSite";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Site Report ";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmSite_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBundle)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdStatus)).EndInit();
            this.cmsDeeds.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SaveFileDialog sfdUAT;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private nControls.deLabel deLabel2;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private nControls.deLabel deLabel3;
        private nControls.deButton deButton1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView grdStatus;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dgvBundle;
        private nControls.deLabel deLabel1;
        private nControls.deComboBox deComboBox1;
        private System.Windows.Forms.ContextMenuStrip cmsDeeds;
        private System.Windows.Forms.ToolStripMenuItem updateDeedToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}