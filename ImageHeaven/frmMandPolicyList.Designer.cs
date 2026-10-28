
namespace ImageHeaven
{
    partial class frmMandPolicyList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMandPolicyList));
            this.cmdExpList = new System.Windows.Forms.Button();
            this.cmdClose = new System.Windows.Forms.Button();
            this.grdPolicy = new System.Windows.Forms.DataGridView();
            this.svFile = new System.Windows.Forms.SaveFileDialog();
            this.lblBatchName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdPolicy)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdExpList
            // 
            this.cmdExpList.Location = new System.Drawing.Point(443, 267);
            this.cmdExpList.Name = "cmdExpList";
            this.cmdExpList.Size = new System.Drawing.Size(75, 23);
            this.cmdExpList.TabIndex = 7;
            this.cmdExpList.Text = "Save";
            this.cmdExpList.UseVisualStyleBackColor = true;
            this.cmdExpList.Click += new System.EventHandler(this.cmdExpList_Click);
            // 
            // cmdClose
            // 
            this.cmdClose.Location = new System.Drawing.Point(524, 267);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(75, 23);
            this.cmdClose.TabIndex = 6;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // grdPolicy
            // 
            this.grdPolicy.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdPolicy.Location = new System.Drawing.Point(3, 28);
            this.grdPolicy.Name = "grdPolicy";
            this.grdPolicy.Size = new System.Drawing.Size(598, 233);
            this.grdPolicy.TabIndex = 5;
            // 
            // lblBatchName
            // 
            this.lblBatchName.Location = new System.Drawing.Point(86, 9);
            this.lblBatchName.Name = "lblBatchName";
            this.lblBatchName.Size = new System.Drawing.Size(150, 16);
            this.lblBatchName.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Bundle name:";
            // 
            // frmMandPolicyList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 292);
            this.Controls.Add(this.cmdExpList);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.grdPolicy);
            this.Controls.Add(this.lblBatchName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMandPolicyList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "File with mandatory document missing";
            this.Load += new System.EventHandler(this.frmMandPolicyList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdPolicy)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdExpList;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.DataGridView grdPolicy;
        private System.Windows.Forms.SaveFileDialog svFile;
        private System.Windows.Forms.Label lblBatchName;
        private System.Windows.Forms.Label label1;
    }
}