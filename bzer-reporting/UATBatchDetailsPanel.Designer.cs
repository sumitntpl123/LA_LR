namespace Reporting
{
    partial class UATBatchDetailsPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.twoColumnLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lblAvgKhSzVal = new System.Windows.Forms.Label();
            this.lblAvgKhSz = new System.Windows.Forms.Label();
            this.lblScanDPI = new System.Windows.Forms.Label();
            this.lblKhChecked = new System.Windows.Forms.Label();
            this.lblAvgImgSz = new System.Windows.Forms.Label();
            this.lblScanDPIVal = new System.Windows.Forms.Label();
            this.lblKhCheckedVal = new System.Windows.Forms.Label();
            this.lblAvgImgSzVal = new System.Windows.Forms.Label();
            this.lsMissingKhatians = new System.Windows.Forms.ListView();
            this.colMissingKhatian = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.twoColumnLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // twoColumnLayout
            // 
            this.twoColumnLayout.ColumnCount = 4;
            this.twoColumnLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.twoColumnLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.twoColumnLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.twoColumnLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.twoColumnLayout.Controls.Add(this.lblAvgKhSzVal, 0, 3);
            this.twoColumnLayout.Controls.Add(this.lblAvgKhSz, 0, 3);
            this.twoColumnLayout.Controls.Add(this.lblScanDPI, 2, 0);
            this.twoColumnLayout.Controls.Add(this.lblKhChecked, 2, 2);
            this.twoColumnLayout.Controls.Add(this.lblAvgImgSz, 2, 3);
            this.twoColumnLayout.Controls.Add(this.lblScanDPIVal, 3, 0);
            this.twoColumnLayout.Controls.Add(this.lblKhCheckedVal, 3, 2);
            this.twoColumnLayout.Controls.Add(this.lblAvgImgSzVal, 3, 3);
            this.twoColumnLayout.Controls.Add(this.lsMissingKhatians, 2, 1);
            this.twoColumnLayout.Location = new System.Drawing.Point(0, 0);
            this.twoColumnLayout.Name = "twoColumnLayout";
            this.twoColumnLayout.RowCount = 4;
            this.twoColumnLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.61187F));
            this.twoColumnLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.71875F));
            this.twoColumnLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.twoColumnLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.twoColumnLayout.Size = new System.Drawing.Size(681, 108);
            this.twoColumnLayout.TabIndex = 0;
            // 
            // lblAvgKhSzVal
            // 
            this.lblAvgKhSzVal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblAvgKhSzVal.AutoSize = true;
            this.lblAvgKhSzVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvgKhSzVal.Location = new System.Drawing.Point(173, 72);
            this.lblAvgKhSzVal.Name = "lblAvgKhSzVal";
            this.lblAvgKhSzVal.Size = new System.Drawing.Size(20, 17);
            this.lblAvgKhSzVal.TabIndex = 20;
            this.lblAvgKhSzVal.Text = "--";
            // 
            // lblAvgKhSz
            // 
            this.lblAvgKhSz.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblAvgKhSz.AutoSize = true;
            this.lblAvgKhSz.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvgKhSz.Location = new System.Drawing.Point(29, 72);
            this.lblAvgKhSz.Name = "lblAvgKhSz";
            this.lblAvgKhSz.Size = new System.Drawing.Size(138, 17);
            this.lblAvgKhSz.TabIndex = 6;
            this.lblAvgKhSz.Text = "Avg. Record Size:";
            // 
            // lblScanDPI
            // 
            this.lblScanDPI.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblScanDPI.AutoSize = true;
            this.lblScanDPI.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScanDPI.Location = new System.Drawing.Point(428, 0);
            this.lblScanDPI.Name = "lblScanDPI";
            this.lblScanDPI.Size = new System.Drawing.Size(79, 15);
            this.lblScanDPI.TabIndex = 7;
            this.lblScanDPI.Text = "Scan DPI:";
            // 
            // lblKhChecked
            // 
            this.lblKhChecked.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblKhChecked.AutoSize = true;
            this.lblKhChecked.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKhChecked.Location = new System.Drawing.Point(375, 31);
            this.lblKhChecked.Name = "lblKhChecked";
            this.lblKhChecked.Size = new System.Drawing.Size(132, 17);
            this.lblKhChecked.TabIndex = 9;
            this.lblKhChecked.Text = "Record Checked:";
            // 
            // lblAvgImgSz
            // 
            this.lblAvgImgSz.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblAvgImgSz.AutoSize = true;
            this.lblAvgImgSz.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvgImgSz.Location = new System.Drawing.Point(378, 72);
            this.lblAvgImgSz.Name = "lblAvgImgSz";
            this.lblAvgImgSz.Size = new System.Drawing.Size(129, 17);
            this.lblAvgImgSz.TabIndex = 10;
            this.lblAvgImgSz.Text = "Avg. Image Size:";
            // 
            // lblScanDPIVal
            // 
            this.lblScanDPIVal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblScanDPIVal.AutoSize = true;
            this.lblScanDPIVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScanDPIVal.Location = new System.Drawing.Point(513, 0);
            this.lblScanDPIVal.Name = "lblScanDPIVal";
            this.lblScanDPIVal.Size = new System.Drawing.Size(20, 15);
            this.lblScanDPIVal.TabIndex = 19;
            this.lblScanDPIVal.Text = "--";
            // 
            // lblKhCheckedVal
            // 
            this.lblKhCheckedVal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblKhCheckedVal.AutoSize = true;
            this.lblKhCheckedVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKhCheckedVal.Location = new System.Drawing.Point(513, 31);
            this.lblKhCheckedVal.Name = "lblKhCheckedVal";
            this.lblKhCheckedVal.Size = new System.Drawing.Size(20, 17);
            this.lblKhCheckedVal.TabIndex = 17;
            this.lblKhCheckedVal.Text = "--";
            // 
            // lblAvgImgSzVal
            // 
            this.lblAvgImgSzVal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblAvgImgSzVal.AutoSize = true;
            this.lblAvgImgSzVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvgImgSzVal.Location = new System.Drawing.Point(513, 72);
            this.lblAvgImgSzVal.Name = "lblAvgImgSzVal";
            this.lblAvgImgSzVal.Size = new System.Drawing.Size(20, 17);
            this.lblAvgImgSzVal.TabIndex = 21;
            this.lblAvgImgSzVal.Text = "--";
            // 
            // lsMissingKhatians
            // 
            this.lsMissingKhatians.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colMissingKhatian});
            this.twoColumnLayout.SetColumnSpan(this.lsMissingKhatians, 2);
            this.lsMissingKhatians.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsMissingKhatians.FullRowSelect = true;
            this.lsMissingKhatians.GridLines = true;
            this.lsMissingKhatians.HideSelection = false;
            this.lsMissingKhatians.Location = new System.Drawing.Point(343, 18);
            this.lsMissingKhatians.Name = "lsMissingKhatians";
            this.lsMissingKhatians.Size = new System.Drawing.Size(335, 6);
            this.lsMissingKhatians.TabIndex = 11;
            this.lsMissingKhatians.UseCompatibleStateImageBehavior = false;
            this.lsMissingKhatians.View = System.Windows.Forms.View.Details;
            this.lsMissingKhatians.Visible = false;
            // 
            // colMissingKhatian
            // 
            this.colMissingKhatian.Text = "Missing Khatian Numbers";
            this.colMissingKhatian.Width = 331;
            // 
            // UATBatchDetailsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.twoColumnLayout);
            this.Name = "UATBatchDetailsPanel";
            this.Size = new System.Drawing.Size(681, 219);
            this.twoColumnLayout.ResumeLayout(false);
            this.twoColumnLayout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel twoColumnLayout;
        private System.Windows.Forms.Label lblAvgKhSz;
        private System.Windows.Forms.Label lblScanDPI;
        private System.Windows.Forms.Label lblKhChecked;
        private System.Windows.Forms.Label lblAvgImgSz;
        private System.Windows.Forms.ListView lsMissingKhatians;
        private System.Windows.Forms.Label lblAvgKhSzVal;
        private System.Windows.Forms.Label lblScanDPIVal;
        private System.Windows.Forms.Label lblKhCheckedVal;
        private System.Windows.Forms.Label lblAvgImgSzVal;
        private System.Windows.Forms.ColumnHeader colMissingKhatian;
    }
}
