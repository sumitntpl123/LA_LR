namespace ControlTest
{
    partial class TestForm
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
            this.pbiComboBox1 = new PhoneticBengaliInput.PBIComboBox();
            this.pbiTextBox3 = new PhoneticBengaliInput.PBITextBox();
            this.pbiTextBox2 = new PhoneticBengaliInput.PBITextBox();
            this.pbiTextBox1 = new PhoneticBengaliInput.PBITextBox();
            this.pbiComboBox2 = new PhoneticBengaliInput.PBIComboBox();
            this.pbiComboBox3 = new PhoneticBengaliInput.PBIComboBox();
            this.btnClearBn = new System.Windows.Forms.Button();
            this.btnClearEn = new System.Windows.Forms.Button();
            this.txtBnFill = new System.Windows.Forms.TextBox();
            this.lblBnFill = new System.Windows.Forms.Label();
            this.panelBnFill = new System.Windows.Forms.Panel();
            this.panelEnFill = new System.Windows.Forms.Panel();
            this.txtEnFill = new System.Windows.Forms.TextBox();
            this.lblEnFill = new System.Windows.Forms.Label();
            this.btnDialog = new System.Windows.Forms.Button();
            this.panelBnFill.SuspendLayout();
            this.panelEnFill.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbiComboBox1
            // 
            this.pbiComboBox1.BengaliPhoneticText = "";
            this.pbiComboBox1.CurrentLanguage = PhoneticBengaliInput.Language.En;
            this.pbiComboBox1.Enabled = false;
            this.pbiComboBox1.EnglishText = "";
            this.pbiComboBox1.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.pbiComboBox1.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.pbiComboBox1.Location = new System.Drawing.Point(12, 141);
            this.pbiComboBox1.Name = "pbiComboBox1";
            this.pbiComboBox1.SelectedIndex = -1;
            this.pbiComboBox1.ShowLanguageSwitcherButton = true;
            this.pbiComboBox1.Size = new System.Drawing.Size(276, 55);
            this.pbiComboBox1.TabIndex = 3;
            // 
            // pbiTextBox3
            // 
            this.pbiTextBox3.BengaliPhoneticText = "";
            this.pbiTextBox3.CurrentLanguage = PhoneticBengaliInput.Language.En;
            this.pbiTextBox3.Enabled = false;
            this.pbiTextBox3.EnglishText = "";
            this.pbiTextBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pbiTextBox3.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.pbiTextBox3.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.pbiTextBox3.Location = new System.Drawing.Point(13, 80);
            this.pbiTextBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbiTextBox3.Name = "pbiTextBox3";
            this.pbiTextBox3.ShowLanguageSwitcherButton = false;
            this.pbiTextBox3.ShowSuggestions = false;
            this.pbiTextBox3.Size = new System.Drawing.Size(275, 54);
            this.pbiTextBox3.TabIndex = 2;
            // 
            // pbiTextBox2
            // 
            this.pbiTextBox2.BengaliPhoneticText = "";
            this.pbiTextBox2.CurrentLanguage = PhoneticBengaliInput.Language.Bn;
            this.pbiTextBox2.Enabled = false;
            this.pbiTextBox2.EnglishText = "";
            this.pbiTextBox2.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.pbiTextBox2.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.Alt0;
            this.pbiTextBox2.Location = new System.Drawing.Point(327, 12);
            this.pbiTextBox2.Name = "pbiTextBox2";
            this.pbiTextBox2.ShowLanguageSwitcherButton = true;
            this.pbiTextBox2.ShowSuggestions = true;
            this.pbiTextBox2.Size = new System.Drawing.Size(291, 54);
            this.pbiTextBox2.TabIndex = 1;
            // 
            // pbiTextBox1
            // 
            this.pbiTextBox1.BengaliPhoneticText = "";
            this.pbiTextBox1.CurrentLanguage = PhoneticBengaliInput.Language.En;
            this.pbiTextBox1.Enabled = false;
            this.pbiTextBox1.EnglishText = "";
            this.pbiTextBox1.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.pbiTextBox1.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.pbiTextBox1.Location = new System.Drawing.Point(12, 12);
            this.pbiTextBox1.Name = "pbiTextBox1";
            this.pbiTextBox1.ShowLanguageSwitcherButton = true;
            this.pbiTextBox1.ShowSuggestions = false;
            this.pbiTextBox1.Size = new System.Drawing.Size(276, 61);
            this.pbiTextBox1.TabIndex = 0;
            // 
            // pbiComboBox2
            // 
            this.pbiComboBox2.BengaliPhoneticText = "";
            this.pbiComboBox2.CurrentLanguage = PhoneticBengaliInput.Language.Bn;
            this.pbiComboBox2.Enabled = false;
            this.pbiComboBox2.EnglishText = "";
            this.pbiComboBox2.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.pbiComboBox2.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.Alt0;
            this.pbiComboBox2.Location = new System.Drawing.Point(327, 141);
            this.pbiComboBox2.Name = "pbiComboBox2";
            this.pbiComboBox2.SelectedIndex = -1;
            this.pbiComboBox2.ShowLanguageSwitcherButton = true;
            this.pbiComboBox2.Size = new System.Drawing.Size(291, 55);
            this.pbiComboBox2.TabIndex = 4;
            this.pbiComboBox2.InnerSelectedIndexChanged += new System.EventHandler(this.pbiComboBox2_InnerSelectedIndexChanged);
            // 
            // pbiComboBox3
            // 
            this.pbiComboBox3.BengaliPhoneticText = "";
            this.pbiComboBox3.CurrentLanguage = PhoneticBengaliInput.Language.En;
            this.pbiComboBox3.Enabled = false;
            this.pbiComboBox3.EnglishText = "";
            this.pbiComboBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pbiComboBox3.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.pbiComboBox3.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.pbiComboBox3.Location = new System.Drawing.Point(13, 214);
            this.pbiComboBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbiComboBox3.Name = "pbiComboBox3";
            this.pbiComboBox3.SelectedIndex = -1;
            this.pbiComboBox3.ShowLanguageSwitcherButton = false;
            this.pbiComboBox3.Size = new System.Drawing.Size(275, 68);
            this.pbiComboBox3.TabIndex = 5;
            // 
            // btnClearBn
            // 
            this.btnClearBn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearBn.Location = new System.Drawing.Point(580, 385);
            this.btnClearBn.Name = "btnClearBn";
            this.btnClearBn.Size = new System.Drawing.Size(95, 53);
            this.btnClearBn.TabIndex = 6;
            this.btnClearBn.Text = "Clear all Bengali";
            this.btnClearBn.UseVisualStyleBackColor = true;
            this.btnClearBn.Click += new System.EventHandler(this.btnClearBn_Click);
            // 
            // btnClearEn
            // 
            this.btnClearEn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearEn.Location = new System.Drawing.Point(693, 385);
            this.btnClearEn.Name = "btnClearEn";
            this.btnClearEn.Size = new System.Drawing.Size(95, 53);
            this.btnClearEn.TabIndex = 7;
            this.btnClearEn.Text = "Clear all English";
            this.btnClearEn.UseVisualStyleBackColor = true;
            this.btnClearEn.Click += new System.EventHandler(this.btnClearEn_Click);
            // 
            // txtBnFill
            // 
            this.txtBnFill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBnFill.Location = new System.Drawing.Point(109, 16);
            this.txtBnFill.Name = "txtBnFill";
            this.txtBnFill.Size = new System.Drawing.Size(180, 20);
            this.txtBnFill.TabIndex = 8;
            // 
            // lblBnFill
            // 
            this.lblBnFill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBnFill.AutoSize = true;
            this.lblBnFill.Location = new System.Drawing.Point(14, 19);
            this.lblBnFill.Name = "lblBnFill";
            this.lblBnFill.Size = new System.Drawing.Size(89, 13);
            this.lblBnFill.TabIndex = 9;
            this.lblBnFill.Text = "Set all Bengali to:";
            // 
            // panelBnFill
            // 
            this.panelBnFill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBnFill.AutoSize = true;
            this.panelBnFill.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelBnFill.Controls.Add(this.txtBnFill);
            this.panelBnFill.Controls.Add(this.lblBnFill);
            this.panelBnFill.Location = new System.Drawing.Point(481, 299);
            this.panelBnFill.Name = "panelBnFill";
            this.panelBnFill.Size = new System.Drawing.Size(307, 39);
            this.panelBnFill.TabIndex = 10;
            // 
            // panelEnFill
            // 
            this.panelEnFill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panelEnFill.AutoSize = true;
            this.panelEnFill.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelEnFill.Controls.Add(this.txtEnFill);
            this.panelEnFill.Controls.Add(this.lblEnFill);
            this.panelEnFill.Location = new System.Drawing.Point(481, 341);
            this.panelEnFill.Name = "panelEnFill";
            this.panelEnFill.Size = new System.Drawing.Size(307, 39);
            this.panelEnFill.TabIndex = 11;
            // 
            // txtEnFill
            // 
            this.txtEnFill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEnFill.Location = new System.Drawing.Point(109, 16);
            this.txtEnFill.Name = "txtEnFill";
            this.txtEnFill.Size = new System.Drawing.Size(180, 20);
            this.txtEnFill.TabIndex = 8;
            // 
            // lblEnFill
            // 
            this.lblEnFill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEnFill.AutoSize = true;
            this.lblEnFill.Location = new System.Drawing.Point(14, 19);
            this.lblEnFill.Name = "lblEnFill";
            this.lblEnFill.Size = new System.Drawing.Size(88, 13);
            this.lblEnFill.TabIndex = 9;
            this.lblEnFill.Text = "Set all English to:";
            // 
            // btnDialog
            // 
            this.btnDialog.Location = new System.Drawing.Point(13, 414);
            this.btnDialog.Name = "btnDialog";
            this.btnDialog.Size = new System.Drawing.Size(75, 23);
            this.btnDialog.TabIndex = 12;
            this.btnDialog.Text = "Open Dialog";
            this.btnDialog.UseVisualStyleBackColor = true;
            this.btnDialog.Click += new System.EventHandler(this.btnDialog_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnDialog);
            this.Controls.Add(this.panelEnFill);
            this.Controls.Add(this.panelBnFill);
            this.Controls.Add(this.btnClearEn);
            this.Controls.Add(this.btnClearBn);
            this.Controls.Add(this.pbiComboBox3);
            this.Controls.Add(this.pbiComboBox2);
            this.Controls.Add(this.pbiComboBox1);
            this.Controls.Add(this.pbiTextBox3);
            this.Controls.Add(this.pbiTextBox2);
            this.Controls.Add(this.pbiTextBox1);
            this.Name = "TestForm";
            this.ShowIcon = false;
            this.Text = "Control Test Form";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelBnFill.ResumeLayout(false);
            this.panelBnFill.PerformLayout();
            this.panelEnFill.ResumeLayout(false);
            this.panelEnFill.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PhoneticBengaliInput.PBITextBox pbiTextBox1;
        private PhoneticBengaliInput.PBITextBox pbiTextBox2;
        private PhoneticBengaliInput.PBITextBox pbiTextBox3;
        private PhoneticBengaliInput.PBIComboBox pbiComboBox1;
        private PhoneticBengaliInput.PBIComboBox pbiComboBox2;
        private PhoneticBengaliInput.PBIComboBox pbiComboBox3;
        private System.Windows.Forms.Button btnClearBn;
        private System.Windows.Forms.Button btnClearEn;
        private System.Windows.Forms.TextBox txtBnFill;
        private System.Windows.Forms.Label lblBnFill;
        private System.Windows.Forms.Panel panelBnFill;
        private System.Windows.Forms.Panel panelEnFill;
        private System.Windows.Forms.TextBox txtEnFill;
        private System.Windows.Forms.Label lblEnFill;
        private System.Windows.Forms.Button btnDialog;
    }
}

