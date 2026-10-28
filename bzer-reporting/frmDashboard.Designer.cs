
namespace Reporting
{
    partial class frmDashboard
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
            this.mainPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lvwBatchDetails = new System.Windows.Forms.ListView();
            this.lvwSummary = new System.Windows.Forms.ListView();
            this.State = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.count = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.flowLayoutSearch = new System.Windows.Forms.FlowLayoutPanel();
            this.txtSearch = new nControls.deTextBox();
            this.btnSearch = new nControls.deButton();
            this.btnClearSearch = new nControls.deButton();
            this.flowLayoutFilters = new System.Windows.Forms.FlowLayoutPanel();
            this.pbiCmbDistrict = new PhoneticBengaliInput.PBIComboBox();
            this.pbiCmbBlock = new PhoneticBengaliInput.PBIComboBox();
            this.pbiCmbMouza = new PhoneticBengaliInput.PBIComboBox();
            this.btnApplyFilter = new nControls.deButton();
            this.btnClearFilter = new nControls.deButton();
            this.stsDetails = new System.Windows.Forms.StatusStrip();
            this.filterField = new System.Windows.Forms.ToolStripStatusLabel();
            this.batchCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainPanel.SuspendLayout();
            this.flowLayoutSearch.SuspendLayout();
            this.flowLayoutFilters.SuspendLayout();
            this.stsDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mainPanel.ColumnCount = 2;
            this.mainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.70434F));
            this.mainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 77.29566F));
            this.mainPanel.Controls.Add(this.lvwBatchDetails, 1, 1);
            this.mainPanel.Controls.Add(this.lvwSummary, 0, 1);
            this.mainPanel.Controls.Add(this.flowLayoutSearch, 0, 0);
            this.mainPanel.Controls.Add(this.flowLayoutFilters, 1, 0);
            this.mainPanel.Controls.Add(this.stsDetails, 1, 2);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.RowCount = 3;
            this.mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 86.38838F));
            this.mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.61161F));
            this.mainPanel.Size = new System.Drawing.Size(1229, 334);
            this.mainPanel.TabIndex = 0;
            // 
            // lvwBatchDetails
            // 
            this.lvwBatchDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwBatchDetails.FullRowSelect = true;
            this.lvwBatchDetails.GridLines = true;
            this.lvwBatchDetails.HideSelection = false;
            this.lvwBatchDetails.Location = new System.Drawing.Point(282, 77);
            this.lvwBatchDetails.Name = "lvwBatchDetails";
            this.lvwBatchDetails.Size = new System.Drawing.Size(944, 218);
            this.lvwBatchDetails.TabIndex = 4;
            this.lvwBatchDetails.UseCompatibleStateImageBehavior = false;
            this.lvwBatchDetails.View = System.Windows.Forms.View.Details;
            // 
            // lvwSummary
            // 
            this.lvwSummary.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.State,
            this.count});
            this.lvwSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwSummary.GridLines = true;
            this.lvwSummary.HideSelection = false;
            this.lvwSummary.Location = new System.Drawing.Point(3, 77);
            this.lvwSummary.Name = "lvwSummary";
            this.lvwSummary.Size = new System.Drawing.Size(273, 218);
            this.lvwSummary.TabIndex = 3;
            this.lvwSummary.UseCompatibleStateImageBehavior = false;
            this.lvwSummary.View = System.Windows.Forms.View.Details;
            // 
            // State
            // 
            this.State.Text = "States";
            // 
            // count
            // 
            this.count.Text = "Count";
            // 
            // flowLayoutSearch
            // 
            this.flowLayoutSearch.Controls.Add(this.txtSearch);
            this.flowLayoutSearch.Controls.Add(this.btnSearch);
            this.flowLayoutSearch.Controls.Add(this.btnClearSearch);
            this.flowLayoutSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutSearch.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutSearch.Name = "flowLayoutSearch";
            this.flowLayoutSearch.Size = new System.Drawing.Size(273, 68);
            this.flowLayoutSearch.TabIndex = 6;
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = System.Drawing.Color.White;
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.ForeColor = System.Drawing.Color.Black;
            this.txtSearch.Location = new System.Drawing.Point(3, 3);
            this.txtSearch.Mandatory = true;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(102, 23);
            this.txtSearch.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSearch.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(111, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Search";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSearch.UseCompatibleTextRendering = true;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClearSearch
            // 
            this.btnClearSearch.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnClearSearch.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnClearSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearSearch.Location = new System.Drawing.Point(192, 3);
            this.btnClearSearch.Name = "btnClearSearch";
            this.btnClearSearch.Size = new System.Drawing.Size(75, 23);
            this.btnClearSearch.TabIndex = 3;
            this.btnClearSearch.Text = "Clear";
            this.btnClearSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClearSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClearSearch.UseCompatibleTextRendering = true;
            this.btnClearSearch.UseVisualStyleBackColor = true;
            this.btnClearSearch.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // flowLayoutFilters
            // 
            this.flowLayoutFilters.Controls.Add(this.pbiCmbDistrict);
            this.flowLayoutFilters.Controls.Add(this.pbiCmbBlock);
            this.flowLayoutFilters.Controls.Add(this.pbiCmbMouza);
            this.flowLayoutFilters.Controls.Add(this.btnApplyFilter);
            this.flowLayoutFilters.Controls.Add(this.btnClearFilter);
            this.flowLayoutFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutFilters.Location = new System.Drawing.Point(282, 3);
            this.flowLayoutFilters.Name = "flowLayoutFilters";
            this.flowLayoutFilters.Size = new System.Drawing.Size(944, 68);
            this.flowLayoutFilters.TabIndex = 7;
            // 
            // pbiCmbDistrict
            // 
            this.pbiCmbDistrict.BengaliPhoneticText = "";
            this.pbiCmbDistrict.CurrentLanguage = PhoneticBengaliInput.Language.Bn;
            this.pbiCmbDistrict.Dock = System.Windows.Forms.DockStyle.Top;
            this.pbiCmbDistrict.Enabled = false;
            this.pbiCmbDistrict.EnglishText = "";
            this.pbiCmbDistrict.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.pbiCmbDistrict.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.pbiCmbDistrict.Location = new System.Drawing.Point(3, 3);
            this.pbiCmbDistrict.Name = "pbiCmbDistrict";
            this.pbiCmbDistrict.SelectedIndex = -1;
            this.pbiCmbDistrict.ShowLanguageSwitcherButton = false;
            this.pbiCmbDistrict.Size = new System.Drawing.Size(252, 55);
            this.pbiCmbDistrict.TabIndex = 0;
            this.pbiCmbDistrict.Leave += new System.EventHandler(this.pbiCmbDistrict_Leave);
            // 
            // pbiCmbBlock
            // 
            this.pbiCmbBlock.BengaliPhoneticText = "";
            this.pbiCmbBlock.CurrentLanguage = PhoneticBengaliInput.Language.Bn;
            this.pbiCmbBlock.Dock = System.Windows.Forms.DockStyle.Top;
            this.pbiCmbBlock.Enabled = false;
            this.pbiCmbBlock.EnglishText = "";
            this.pbiCmbBlock.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.pbiCmbBlock.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.pbiCmbBlock.Location = new System.Drawing.Point(261, 3);
            this.pbiCmbBlock.Name = "pbiCmbBlock";
            this.pbiCmbBlock.SelectedIndex = -1;
            this.pbiCmbBlock.ShowLanguageSwitcherButton = false;
            this.pbiCmbBlock.Size = new System.Drawing.Size(252, 55);
            this.pbiCmbBlock.TabIndex = 2;
            this.pbiCmbBlock.Leave += new System.EventHandler(this.pbiCmbBlock_Leave);
            // 
            // pbiCmbMouza
            // 
            this.pbiCmbMouza.BengaliPhoneticText = "";
            this.pbiCmbMouza.CurrentLanguage = PhoneticBengaliInput.Language.Bn;
            this.pbiCmbMouza.Enabled = false;
            this.pbiCmbMouza.EnglishText = "";
            this.pbiCmbMouza.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.pbiCmbMouza.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.pbiCmbMouza.Location = new System.Drawing.Point(519, 3);
            this.pbiCmbMouza.Name = "pbiCmbMouza";
            this.pbiCmbMouza.SelectedIndex = -1;
            this.pbiCmbMouza.ShowLanguageSwitcherButton = false;
            this.pbiCmbMouza.Size = new System.Drawing.Size(252, 55);
            this.pbiCmbMouza.TabIndex = 3;
            this.pbiCmbMouza.Leave += new System.EventHandler(this.pbiCmbMouza_Leave);
            // 
            // btnApplyFilter
            // 
            this.btnApplyFilter.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnApplyFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApplyFilter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApplyFilter.Location = new System.Drawing.Point(777, 3);
            this.btnApplyFilter.Name = "btnApplyFilter";
            this.btnApplyFilter.Size = new System.Drawing.Size(75, 23);
            this.btnApplyFilter.TabIndex = 1;
            this.btnApplyFilter.Text = "Apply";
            this.btnApplyFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnApplyFilter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnApplyFilter.UseCompatibleTextRendering = true;
            this.btnApplyFilter.UseVisualStyleBackColor = true;
            this.btnApplyFilter.Click += new System.EventHandler(this.btnApplyFilter_Click);
            // 
            // btnClearFilter
            // 
            this.btnClearFilter.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnClearFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearFilter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearFilter.Location = new System.Drawing.Point(858, 3);
            this.btnClearFilter.Name = "btnClearFilter";
            this.btnClearFilter.Size = new System.Drawing.Size(75, 23);
            this.btnClearFilter.TabIndex = 4;
            this.btnClearFilter.Text = "Clear";
            this.btnClearFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClearFilter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClearFilter.UseCompatibleTextRendering = true;
            this.btnClearFilter.UseVisualStyleBackColor = true;
            this.btnClearFilter.Click += new System.EventHandler(this.btnClearFilter_Click);
            // 
            // stsDetails
            // 
            this.stsDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.stsDetails.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.batchCount,
            this.filterField});
            this.stsDetails.Location = new System.Drawing.Point(279, 298);
            this.stsDetails.Name = "stsDetails";
            this.stsDetails.Size = new System.Drawing.Size(950, 22);
            this.stsDetails.TabIndex = 8;
            // 
            // filterField
            // 
            this.filterField.Name = "filterField";
            this.filterField.Size = new System.Drawing.Size(0, 17);
            // 
            // batchCount
            // 
            this.batchCount.Name = "batchCount";
            this.batchCount.Size = new System.Drawing.Size(70, 17);
            this.batchCount.Text = "batchCount";
            // 
            // frmDashboard
            // 
            this.ClientSize = new System.Drawing.Size(1229, 334);
            this.Controls.Add(this.mainPanel);
            this.Name = "frmDashboard";
            this.Text = "Dashboard";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.flowLayoutSearch.ResumeLayout(false);
            this.flowLayoutSearch.PerformLayout();
            this.flowLayoutFilters.ResumeLayout(false);
            this.stsDetails.ResumeLayout(false);
            this.stsDetails.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.TableLayoutPanel mainPanel;
        #endregion
        private System.Windows.Forms.ListView lvwBatchDetails;
        private nControls.deTextBox txtSearch;
        private nControls.deButton btnSearch;
        private nControls.deButton btnClearSearch;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutSearch;
        private System.Windows.Forms.ListView lvwSummary;
        private System.Windows.Forms.ColumnHeader State;
        private System.Windows.Forms.ColumnHeader count;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutFilters;
        private PhoneticBengaliInput.PBIComboBox pbiCmbDistrict;
        private nControls.deButton btnApplyFilter;
        private PhoneticBengaliInput.PBIComboBox pbiCmbBlock;
        private PhoneticBengaliInput.PBIComboBox pbiCmbMouza;
        private nControls.deButton btnClearFilter;
        private System.Windows.Forms.StatusStrip stsDetails;
        private System.Windows.Forms.ToolStripStatusLabel filterField;
        private System.Windows.Forms.ToolStripStatusLabel batchCount;
    }
}