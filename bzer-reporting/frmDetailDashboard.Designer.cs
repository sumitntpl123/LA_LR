
namespace Reporting
{
    partial class frmDetailDashboard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDetailDashboard));
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.deLabel2 = new nControls.deLabel();
            this.deTextBox1 = new nControls.deTextBox();
            this.deTextBox29 = new nControls.deTextBox();
            this.deLabel1 = new nControls.deLabel();
            this.deLabel29 = new nControls.deLabel();
            this.deButton1 = new nControls.deButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.grdStatus = new System.Windows.Forms.DataGridView();
            this.updateDeedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsDeeds = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportToCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sfdUAT = new System.Windows.Forms.SaveFileDialog();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdStatus)).BeginInit();
            this.cmsDeeds.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 65);
            this.panel1.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.deLabel2);
            this.groupBox1.Controls.Add(this.deTextBox1);
            this.groupBox1.Controls.Add(this.deTextBox29);
            this.groupBox1.Controls.Add(this.deLabel1);
            this.groupBox1.Controls.Add(this.deLabel29);
            this.groupBox1.Controls.Add(this.deButton1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(800, 65);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Criteria";
            // 
            // deLabel2
            // 
            this.deLabel2.AutoSize = true;
            this.deLabel2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel2.ForeColor = System.Drawing.Color.Red;
            this.deLabel2.Location = new System.Drawing.Point(8, 40);
            this.deLabel2.Name = "deLabel2";
            this.deLabel2.Size = new System.Drawing.Size(54, 15);
            this.deLabel2.TabIndex = 15;
            this.deLabel2.Text = "deLabel2";
            // 
            // deTextBox1
            // 
            this.deTextBox1.BackColor = System.Drawing.Color.White;
            this.deTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.deTextBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deTextBox1.ForeColor = System.Drawing.Color.Black;
            this.deTextBox1.Location = new System.Drawing.Point(445, 19);
            this.deTextBox1.Mandatory = true;
            this.deTextBox1.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.deTextBox1.MaxLength = 99999999;
            this.deTextBox1.Name = "deTextBox1";
            this.deTextBox1.Size = new System.Drawing.Size(98, 22);
            this.deTextBox1.TabIndex = 2;
            this.deTextBox1.Leave += new System.EventHandler(this.deTextBox1_Leave);
            // 
            // deTextBox29
            // 
            this.deTextBox29.BackColor = System.Drawing.Color.White;
            this.deTextBox29.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.deTextBox29.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deTextBox29.ForeColor = System.Drawing.Color.Black;
            this.deTextBox29.Location = new System.Drawing.Point(213, 19);
            this.deTextBox29.Mandatory = true;
            this.deTextBox29.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.deTextBox29.MaxLength = 99999999;
            this.deTextBox29.Name = "deTextBox29";
            this.deTextBox29.Size = new System.Drawing.Size(98, 22);
            this.deTextBox29.TabIndex = 1;
            this.deTextBox29.Leave += new System.EventHandler(this.deTextBox29_Leave);
            // 
            // deLabel1
            // 
            this.deLabel1.AutoSize = true;
            this.deLabel1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel1.Location = new System.Drawing.Point(367, 23);
            this.deLabel1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.deLabel1.Name = "deLabel1";
            this.deLabel1.Size = new System.Drawing.Size(74, 13);
            this.deLabel1.TabIndex = 14;
            this.deLabel1.Text = "To Serial No :";
            // 
            // deLabel29
            // 
            this.deLabel29.AutoSize = true;
            this.deLabel29.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel29.Location = new System.Drawing.Point(121, 23);
            this.deLabel29.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.deLabel29.Name = "deLabel29";
            this.deLabel29.Size = new System.Drawing.Size(88, 13);
            this.deLabel29.TabIndex = 13;
            this.deLabel29.Text = "From Serial No :";
            // 
            // deButton1
            // 
            this.deButton1.BackColor = System.Drawing.SystemColors.Control;
            this.deButton1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("deButton1.BackgroundImage")));
            this.deButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.deButton1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.deButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deButton1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deButton1.Location = new System.Drawing.Point(682, 15);
            this.deButton1.Name = "deButton1";
            this.deButton1.Size = new System.Drawing.Size(92, 29);
            this.deButton1.TabIndex = 12;
            this.deButton1.Text = "&Search";
            this.deButton1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deButton1.UseCompatibleTextRendering = true;
            this.deButton1.UseVisualStyleBackColor = false;
            this.deButton1.Click += new System.EventHandler(this.deButton1_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(800, 450);
            this.panel3.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.grdStatus);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(800, 450);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Summary :";
            // 
            // grdStatus
            // 
            this.grdStatus.AllowUserToAddRows = false;
            this.grdStatus.AllowUserToDeleteRows = false;
            this.grdStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdStatus.Location = new System.Drawing.Point(3, 71);
            this.grdStatus.MultiSelect = false;
            this.grdStatus.Name = "grdStatus";
            this.grdStatus.ReadOnly = true;
            this.grdStatus.RowHeadersWidth = 62;
            this.grdStatus.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdStatus.Size = new System.Drawing.Size(794, 369);
            this.grdStatus.TabIndex = 7;
            this.grdStatus.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdStatus_MouseClick);
            // 
            // updateDeedToolStripMenuItem
            // 
            this.updateDeedToolStripMenuItem.Name = "updateDeedToolStripMenuItem";
            this.updateDeedToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.updateDeedToolStripMenuItem.Text = "&Export to Excel";
            this.updateDeedToolStripMenuItem.Click += new System.EventHandler(this.updateDeedToolStripMenuItem_Click);
            // 
            // cmsDeeds
            // 
            this.cmsDeeds.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateDeedToolStripMenuItem,
            this.exportToCSVToolStripMenuItem});
            this.cmsDeeds.Name = "cmsDeeds";
            this.cmsDeeds.Size = new System.Drawing.Size(153, 48);
            // 
            // exportToCSVToolStripMenuItem
            // 
            this.exportToCSVToolStripMenuItem.Name = "exportToCSVToolStripMenuItem";
            this.exportToCSVToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exportToCSVToolStripMenuItem.Text = "&Export to CSV";
            this.exportToCSVToolStripMenuItem.Click += new System.EventHandler(this.exportToCSVToolStripMenuItem_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 450);
            this.panel2.TabIndex = 11;
            // 
            // frmDetailDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDetailDashboard";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Detailed Dashboard Report";
            this.Load += new System.EventHandler(this.frmDetailDashboard_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdStatus)).EndInit();
            this.cmsDeeds.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private nControls.deButton deButton1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView grdStatus;
        private System.Windows.Forms.ToolStripMenuItem updateDeedToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmsDeeds;
        private System.Windows.Forms.SaveFileDialog sfdUAT;
        private System.Windows.Forms.Panel panel2;
        private nControls.deLabel deLabel29;
        private nControls.deLabel deLabel1;
        private nControls.deTextBox deTextBox29;
        private nControls.deTextBox deTextBox1;
        private nControls.deLabel deLabel2;
        private System.Windows.Forms.ToolStripMenuItem exportToCSVToolStripMenuItem;
    }
}