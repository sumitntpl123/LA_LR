namespace PhoneticBengaliInput
{
    partial class PBIKeyboardLayoutHelp
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.kbPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.kbPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // kbPictureBox
            // 
            this.kbPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kbPictureBox.Image = global::PhoneticBengaliInput.Properties.Resources.keyboardmanual;
            this.kbPictureBox.Location = new System.Drawing.Point(0, 0);
            this.kbPictureBox.Name = "kbPictureBox";
            this.kbPictureBox.Size = new System.Drawing.Size(594, 222);
            this.kbPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.kbPictureBox.TabIndex = 0;
            this.kbPictureBox.TabStop = false;
            // 
            // PBIKeyboardLayoutHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 222);
            this.Controls.Add(this.kbPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "PBIKeyboardLayoutHelp";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Keyboard Layout for phonetic Bengali typing";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PBIKeyboardLayoutHelp_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.kbPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox kbPictureBox;
    }
}