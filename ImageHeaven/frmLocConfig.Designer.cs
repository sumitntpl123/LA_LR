
namespace ImageHeaven
{
    partial class frmLocConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLocConfig));
            this.deLabel1 = new nControls.deLabel();
            this.deTextBox1 = new nControls.deTextBox();
            this.deButton20 = new nControls.deButton();
            this.deButton21 = new nControls.deButton();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // deLabel1
            // 
            this.deLabel1.AutoSize = true;
            this.deLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deLabel1.Location = new System.Drawing.Point(42, 20);
            this.deLabel1.Name = "deLabel1";
            this.deLabel1.Size = new System.Drawing.Size(59, 15);
            this.deLabel1.TabIndex = 0;
            this.deLabel1.Text = "Location :";
            // 
            // deTextBox1
            // 
            this.deTextBox1.BackColor = System.Drawing.Color.White;
            this.deTextBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deTextBox1.ForeColor = System.Drawing.Color.Black;
            this.deTextBox1.Location = new System.Drawing.Point(116, 17);
            this.deTextBox1.Mandatory = true;
            this.deTextBox1.Name = "deTextBox1";
            this.deTextBox1.Size = new System.Drawing.Size(317, 23);
            this.deTextBox1.TabIndex = 1;
            this.deTextBox1.Leave += new System.EventHandler(this.deTextBox1_Leave);
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
            this.deButton20.Location = new System.Drawing.Point(296, 58);
            this.deButton20.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.deButton20.Name = "deButton20";
            this.deButton20.Size = new System.Drawing.Size(87, 30);
            this.deButton20.TabIndex = 85;
            this.deButton20.Text = "&Save";
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
            this.deButton21.Location = new System.Drawing.Point(407, 58);
            this.deButton21.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.deButton21.Name = "deButton21";
            this.deButton21.Size = new System.Drawing.Size(75, 30);
            this.deButton21.TabIndex = 86;
            this.deButton21.Text = "C&lose";
            this.deButton21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.deButton21.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.deButton21.UseCompatibleTextRendering = true;
            this.deButton21.UseVisualStyleBackColor = false;
            this.deButton21.Click += new System.EventHandler(this.deButton21_Click);
            // 
            // frmLocConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 97);
            this.Controls.Add(this.deButton20);
            this.Controls.Add(this.deButton21);
            this.Controls.Add(this.deTextBox1);
            this.Controls.Add(this.deLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLocConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Path Configuration Window";
            this.Load += new System.EventHandler(this.frmPathConfig_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private nControls.deLabel deLabel1;
        private nControls.deTextBox deTextBox1;
        private nControls.deButton deButton20;
        private nControls.deButton deButton21;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}