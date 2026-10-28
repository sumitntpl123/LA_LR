namespace PhoneticBengaliInput
{
    partial class PBIComboBox
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
            this.languageSwitcherBtn = new System.Windows.Forms.Button();
            this.phoneticPreviewTxt = new System.Windows.Forms.Label();
            this.mainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.mainComboBox = new System.Windows.Forms.ComboBox();
            this.mainLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // languageSwitcherBtn
            // 
            this.languageSwitcherBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.languageSwitcherBtn.Location = new System.Drawing.Point(221, 3);
            this.languageSwitcherBtn.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.languageSwitcherBtn.Name = "languageSwitcherBtn";
            this.languageSwitcherBtn.Size = new System.Drawing.Size(28, 23);
            this.languageSwitcherBtn.TabIndex = 2;
            this.languageSwitcherBtn.Text = "En";
            this.languageSwitcherBtn.UseVisualStyleBackColor = true;
            this.languageSwitcherBtn.Click += new System.EventHandler(this.languageSwitcherBtn_Click);
            // 
            // phoneticPreviewTxt
            // 
            this.phoneticPreviewTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.phoneticPreviewTxt.AutoSize = true;
            this.mainLayoutPanel.SetColumnSpan(this.phoneticPreviewTxt, 2);
            this.phoneticPreviewTxt.Location = new System.Drawing.Point(3, 29);
            this.phoneticPreviewTxt.Name = "phoneticPreviewTxt";
            this.phoneticPreviewTxt.Size = new System.Drawing.Size(246, 13);
            this.phoneticPreviewTxt.TabIndex = 3;
            // 
            // mainLayoutPanel
            // 
            this.mainLayoutPanel.ColumnCount = 2;
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mainLayoutPanel.Controls.Add(this.phoneticPreviewTxt, 0, 1);
            this.mainLayoutPanel.Controls.Add(this.languageSwitcherBtn, 1, 0);
            this.mainLayoutPanel.Controls.Add(this.mainComboBox, 0, 0);
            this.mainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainLayoutPanel.Name = "mainLayoutPanel";
            this.mainLayoutPanel.RowCount = 2;
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainLayoutPanel.Size = new System.Drawing.Size(252, 55);
            this.mainLayoutPanel.TabIndex = 0;
            // 
            // mainComboBox
            // 
            this.mainComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.mainComboBox.FormattingEnabled = true;
            this.mainComboBox.Location = new System.Drawing.Point(3, 4);
            this.mainComboBox.Name = "mainComboBox";
            this.mainComboBox.Size = new System.Drawing.Size(214, 21);
            this.mainComboBox.TabIndex = 0;
            this.mainComboBox.DropDown += new System.EventHandler(this.mainComboBox_DropDown);
            this.mainComboBox.SelectionChangeCommitted += new System.EventHandler(this.mainComboBox_SelectionChangeCommitted);
            this.mainComboBox.TextUpdate += new System.EventHandler(this.mainComboBox_TextUpdate);
            this.mainComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mainComboBox_KeyDown);
            this.mainComboBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mainComboBox_KeyUp);
            // 
            // PBIComboBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainLayoutPanel);
            this.Name = "PBIComboBox";
            this.Size = new System.Drawing.Size(252, 55);
            this.Load += new System.EventHandler(this.PBIComboBox_Load);
            this.mainLayoutPanel.ResumeLayout(false);
            this.mainLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button languageSwitcherBtn;
        private System.Windows.Forms.Label phoneticPreviewTxt;
        private System.Windows.Forms.TableLayoutPanel mainLayoutPanel;
        private System.Windows.Forms.ComboBox mainComboBox;
    }
}
