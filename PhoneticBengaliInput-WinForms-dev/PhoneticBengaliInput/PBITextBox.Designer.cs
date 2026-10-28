namespace PhoneticBengaliInput
{
    partial class PBITextBox
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
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.phoneticPreviewTxt = new System.Windows.Forms.Label();
            this.mainTextBox = new System.Windows.Forms.TextBox();
            this.languageSwitcherBtn = new System.Windows.Forms.Button();
            this.mainLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayout
            // 
            this.mainLayout.ColumnCount = 2;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mainLayout.Controls.Add(this.phoneticPreviewTxt, 0, 1);
            this.mainLayout.Controls.Add(this.mainTextBox, 0, 0);
            this.mainLayout.Controls.Add(this.languageSwitcherBtn, 1, 0);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.RowCount = 2;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainLayout.Size = new System.Drawing.Size(291, 54);
            this.mainLayout.TabIndex = 0;
            // 
            // phoneticPreviewTxt
            // 
            this.phoneticPreviewTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.phoneticPreviewTxt.AutoSize = true;
            this.mainLayout.SetColumnSpan(this.phoneticPreviewTxt, 2);
            this.phoneticPreviewTxt.Location = new System.Drawing.Point(3, 29);
            this.phoneticPreviewTxt.Name = "phoneticPreviewTxt";
            this.phoneticPreviewTxt.Size = new System.Drawing.Size(285, 13);
            this.phoneticPreviewTxt.TabIndex = 1;
            // 
            // mainTextBox
            // 
            this.mainTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.mainTextBox.Location = new System.Drawing.Point(3, 4);
            this.mainTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 1, 3);
            this.mainTextBox.Name = "mainTextBox";
            this.mainTextBox.Size = new System.Drawing.Size(252, 20);
            this.mainTextBox.TabIndex = 0;
            this.mainTextBox.TextChanged += new System.EventHandler(this.mainTextBox_TextChanged);
            this.mainTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mainTextBox_KeyDown);
            this.mainTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mainTextBox_KeyUp);
            // 
            // languageSwitcherBtn
            // 
            this.languageSwitcherBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.languageSwitcherBtn.Location = new System.Drawing.Point(257, 3);
            this.languageSwitcherBtn.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.languageSwitcherBtn.Name = "languageSwitcherBtn";
            this.languageSwitcherBtn.Size = new System.Drawing.Size(31, 23);
            this.languageSwitcherBtn.TabIndex = 1;
            this.languageSwitcherBtn.Text = "En";
            this.languageSwitcherBtn.UseVisualStyleBackColor = true;
            this.languageSwitcherBtn.Click += new System.EventHandler(this.languageSwitcherBtn_Click);
            // 
            // PBITextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainLayout);
            this.Name = "PBITextBox";
            this.Size = new System.Drawing.Size(291, 54);
            this.Load += new System.EventHandler(this.PBITextBox_Load);
            this.mainLayout.ResumeLayout(false);
            this.mainLayout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.TextBox mainTextBox;
        private System.Windows.Forms.Button languageSwitcherBtn;
        private System.Windows.Forms.Label phoneticPreviewTxt;
    }
}
