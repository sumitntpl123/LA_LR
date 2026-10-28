namespace ImageHeaven
{
    partial class frmExport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExport));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbBatch = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbProject = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tblExp = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgvbatch = new System.Windows.Forms.DataGridView();
            this.dgvexport = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblBatchSelected = new System.Windows.Forms.Label();
            this.lblSelected = new System.Windows.Forms.Label();
            this.lblFinalStatus = new System.Windows.Forms.Label();
            this.lblFileStatus = new System.Windows.Forms.Label();
            this.grpControl = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnValidatefiles = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblvolStatus = new System.Windows.Forms.Label();
            this.lblTotSta = new System.Windows.Forms.Label();
            this.progressBarFinal = new System.Windows.Forms.ProgressBar();
            this.label5 = new System.Windows.Forms.Label();
            this.lbldeedCount = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.progressBarFile = new System.Windows.Forms.ProgressBar();
            this.chkReExport = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.tblExp.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvbatch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvexport)).BeginInit();
            this.grpControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbBatch);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbProject);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(3, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(448, 80);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // cmbBatch
            // 
            this.cmbBatch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBatch.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBatch.FormattingEnabled = true;
            this.cmbBatch.Location = new System.Drawing.Point(124, 48);
            this.cmbBatch.Name = "cmbBatch";
            this.cmbBatch.Size = new System.Drawing.Size(266, 25);
            this.cmbBatch.TabIndex = 2;
            this.cmbBatch.Leave += new System.EventHandler(this.cmbBatch_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(62, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Bundle :";
            // 
            // cmbProject
            // 
            this.cmbProject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProject.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbProject.FormattingEnabled = true;
            this.cmbProject.Location = new System.Drawing.Point(124, 14);
            this.cmbProject.Name = "cmbProject";
            this.cmbProject.Size = new System.Drawing.Size(266, 25);
            this.cmbProject.TabIndex = 1;
            this.cmbProject.Leave += new System.EventHandler(this.cmbProject_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(61, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Project :";
            // 
            // tblExp
            // 
            this.tblExp.Controls.Add(this.tabPage1);
            this.tblExp.Controls.Add(this.tabPage2);
            this.tblExp.Location = new System.Drawing.Point(1, 104);
            this.tblExp.Name = "tblExp";
            this.tblExp.SelectedIndex = 0;
            this.tblExp.Size = new System.Drawing.Size(451, 303);
            this.tblExp.TabIndex = 41;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgvbatch);
            this.tabPage1.Controls.Add(this.dgvexport);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(443, 277);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "File Name";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvbatch
            // 
            this.dgvbatch.AllowUserToAddRows = false;
            this.dgvbatch.AllowUserToDeleteRows = false;
            this.dgvbatch.AllowUserToResizeColumns = false;
            this.dgvbatch.AllowUserToResizeRows = false;
            this.dgvbatch.BackgroundColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.dgvbatch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvbatch.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvbatch.Location = new System.Drawing.Point(9, 16);
            this.dgvbatch.Name = "dgvbatch";
            this.dgvbatch.RowHeadersVisible = false;
            this.dgvbatch.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvbatch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvbatch.Size = new System.Drawing.Size(194, 255);
            this.dgvbatch.TabIndex = 3;
            this.dgvbatch.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvbatch_CellClick_1);
            this.dgvbatch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvbatch_KeyDown);
            // 
            // dgvexport
            // 
            this.dgvexport.AllowUserToAddRows = false;
            this.dgvexport.AllowUserToDeleteRows = false;
            this.dgvexport.BackgroundColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.dgvexport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvexport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvexport.Location = new System.Drawing.Point(209, 15);
            this.dgvexport.Name = "dgvexport";
            this.dgvexport.RowHeadersVisible = false;
            this.dgvexport.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvexport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvexport.Size = new System.Drawing.Size(231, 256);
            this.dgvexport.TabIndex = 4;
            this.dgvexport.DataSourceChanged += new System.EventHandler(this.dgvexport_DataSourceOrSelectionChanged);
            this.dgvexport.SelectionChanged += new System.EventHandler(this.dgvexport_DataSourceOrSelectionChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(443, 277);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Result";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lblBatchSelected
            // 
            this.lblBatchSelected.AutoSize = true;
            this.lblBatchSelected.Location = new System.Drawing.Point(143, 419);
            this.lblBatchSelected.Name = "lblBatchSelected";
            this.lblBatchSelected.Size = new System.Drawing.Size(16, 13);
            this.lblBatchSelected.TabIndex = 55;
            this.lblBatchSelected.Text = "...";
            this.lblBatchSelected.Visible = false;
            // 
            // lblSelected
            // 
            this.lblSelected.AutoSize = true;
            this.lblSelected.Location = new System.Drawing.Point(89, 419);
            this.lblSelected.Name = "lblSelected";
            this.lblSelected.Size = new System.Drawing.Size(55, 13);
            this.lblSelected.TabIndex = 54;
            this.lblSelected.Text = "Selected: ";
            this.lblSelected.Visible = false;
            // 
            // lblFinalStatus
            // 
            this.lblFinalStatus.AutoSize = true;
            this.lblFinalStatus.Location = new System.Drawing.Point(90, 495);
            this.lblFinalStatus.Name = "lblFinalStatus";
            this.lblFinalStatus.Size = new System.Drawing.Size(16, 13);
            this.lblFinalStatus.TabIndex = 53;
            this.lblFinalStatus.Text = "...";
            // 
            // lblFileStatus
            // 
            this.lblFileStatus.AutoSize = true;
            this.lblFileStatus.Location = new System.Drawing.Point(91, 448);
            this.lblFileStatus.Name = "lblFileStatus";
            this.lblFileStatus.Size = new System.Drawing.Size(16, 13);
            this.lblFileStatus.TabIndex = 52;
            this.lblFileStatus.Text = "...";
            // 
            // grpControl
            // 
            this.grpControl.Controls.Add(this.btnClose);
            this.grpControl.Controls.Add(this.btnValidatefiles);
            this.grpControl.Controls.Add(this.btnExport);
            this.grpControl.Controls.Add(this.btnCancel);
            this.grpControl.Location = new System.Drawing.Point(175, 409);
            this.grpControl.Name = "grpControl";
            this.grpControl.Size = new System.Drawing.Size(271, 39);
            this.grpControl.TabIndex = 51;
            this.grpControl.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(207, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(61, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // btnValidatefiles
            // 
            this.btnValidatefiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnValidatefiles.Location = new System.Drawing.Point(6, 10);
            this.btnValidatefiles.Name = "btnValidatefiles";
            this.btnValidatefiles.Size = new System.Drawing.Size(61, 23);
            this.btnValidatefiles.TabIndex = 5;
            this.btnValidatefiles.Text = "&Validate";
            this.btnValidatefiles.UseVisualStyleBackColor = true;
            this.btnValidatefiles.Click += new System.EventHandler(this.cmdValidatefiles_Click);
            // 
            // btnExport
            // 
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Location = new System.Drawing.Point(73, 10);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(61, 23);
            this.btnExport.TabIndex = 6;
            this.btnExport.Text = "&Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click_1);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(140, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(61, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.cmdCancel_Click_1);
            // 
            // lblvolStatus
            // 
            this.lblvolStatus.AutoSize = true;
            this.lblvolStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblvolStatus.Location = new System.Drawing.Point(18, 472);
            this.lblvolStatus.Name = "lblvolStatus";
            this.lblvolStatus.Size = new System.Drawing.Size(56, 13);
            this.lblvolStatus.TabIndex = 50;
            this.lblvolStatus.Text = "File Status";
            // 
            // lblTotSta
            // 
            this.lblTotSta.AutoSize = true;
            this.lblTotSta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotSta.Location = new System.Drawing.Point(4, 512);
            this.lblTotSta.Name = "lblTotSta";
            this.lblTotSta.Size = new System.Drawing.Size(70, 13);
            this.lblTotSta.TabIndex = 49;
            this.lblTotSta.Text = "Export Status";
            // 
            // progressBarFinal
            // 
            this.progressBarFinal.ForeColor = System.Drawing.Color.Lime;
            this.progressBarFinal.Location = new System.Drawing.Point(83, 512);
            this.progressBarFinal.Name = "progressBarFinal";
            this.progressBarFinal.Size = new System.Drawing.Size(364, 23);
            this.progressBarFinal.TabIndex = 48;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(193, 432);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 13);
            this.label5.TabIndex = 46;
            this.label5.Visible = false;
            // 
            // lbldeedCount
            // 
            this.lbldeedCount.AutoSize = true;
            this.lbldeedCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldeedCount.Location = new System.Drawing.Point(44, 417);
            this.lbldeedCount.Name = "lbldeedCount";
            this.lbldeedCount.Size = new System.Drawing.Size(23, 15);
            this.lbldeedCount.TabIndex = 45;
            this.lbldeedCount.Text = "....";
            this.lbldeedCount.Visible = false;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(12, 418);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(34, 13);
            this.lblTotal.TabIndex = 43;
            this.lblTotal.Text = "Total:";
            this.lblTotal.Visible = false;
            // 
            // progressBarFile
            // 
            this.progressBarFile.ForeColor = System.Drawing.Color.Lime;
            this.progressBarFile.Location = new System.Drawing.Point(83, 466);
            this.progressBarFile.Name = "progressBarFile";
            this.progressBarFile.Size = new System.Drawing.Size(364, 23);
            this.progressBarFile.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarFile.TabIndex = 42;
            // 
            // chkReExport
            // 
            this.chkReExport.AutoSize = true;
            this.chkReExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkReExport.Location = new System.Drawing.Point(5, 5);
            this.chkReExport.Name = "chkReExport";
            this.chkReExport.Size = new System.Drawing.Size(70, 17);
            this.chkReExport.TabIndex = 0;
            this.chkReExport.Text = "Re-Export";
            this.chkReExport.UseVisualStyleBackColor = true;
            this.chkReExport.CheckedChanged += new System.EventHandler(this.chkReExport_CheckedChanged);
            // 
            // frmExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 540);
            this.Controls.Add(this.chkReExport);
            this.Controls.Add(this.lblBatchSelected);
            this.Controls.Add(this.lblSelected);
            this.Controls.Add(this.lblFinalStatus);
            this.Controls.Add(this.lblFileStatus);
            this.Controls.Add(this.grpControl);
            this.Controls.Add(this.lblvolStatus);
            this.Controls.Add(this.lblTotSta);
            this.Controls.Add(this.progressBarFinal);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbldeedCount);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.progressBarFile);
            this.Controls.Add(this.tblExp);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmExport";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Export";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmExport_FormClosing);
            this.Load += new System.EventHandler(this.frmExport_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tblExp.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvbatch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvexport)).EndInit();
            this.grpControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbBatch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbProject;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tblExp;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dgvbatch;
        private System.Windows.Forms.DataGridView dgvexport;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label lblBatchSelected;
        private System.Windows.Forms.Label lblSelected;
        private System.Windows.Forms.Label lblFinalStatus;
        private System.Windows.Forms.Label lblFileStatus;
        private System.Windows.Forms.GroupBox grpControl;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblvolStatus;
        private System.Windows.Forms.Label lblTotSta;
        private System.Windows.Forms.ProgressBar progressBarFinal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbldeedCount;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.ProgressBar progressBarFile;
        private System.Windows.Forms.CheckBox chkReExport;
        private System.Windows.Forms.Button btnValidatefiles;
        private System.Windows.Forms.Button btnClose;
    }
}