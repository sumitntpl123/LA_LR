
namespace ImageHeaven
{
    partial class frmTargetIndexForMove
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
            this.deIndex = new nControls.deTextBox();
            this.deOK = new nControls.deButton();
            this.SuspendLayout();
            // 
            // deIndex
            // 
            this.deIndex.BackColor = System.Drawing.Color.White;
            this.deIndex.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deIndex.ForeColor = System.Drawing.Color.Black;
            this.deIndex.Location = new System.Drawing.Point(19, 20);
            this.deIndex.Mandatory = true;
            this.deIndex.Name = "deIndex";
            this.deIndex.Size = new System.Drawing.Size(69, 23);
            this.deIndex.TabIndex = 0;
            // 
            // deOK
            // 
            this.deOK.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.deOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deOK.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deOK.Location = new System.Drawing.Point(136, 12);
            this.deOK.Name = "deOK";
            this.deOK.Size = new System.Drawing.Size(46, 39);
            this.deOK.TabIndex = 1;
            this.deOK.Text = "OK";
            this.deOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deOK.UseCompatibleTextRendering = true;
            this.deOK.UseVisualStyleBackColor = true;
            this.deOK.Click += new System.EventHandler(this.deButton1_Click);
            // 
            // frmTargetIndexForMove
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(201, 65);
            this.ControlBox = false;
            this.Controls.Add(this.deOK);
            this.Controls.Add(this.deIndex);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTargetIndexForMove";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Target Position (ESC to Cancel)";
            this.TopMost = true;
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmTargetIndexForMove_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private nControls.deTextBox deIndex;
        private nControls.deButton deOK;
    }
}