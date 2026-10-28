namespace ControlTest
{
    partial class TestDialogForm
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
            this.pbiTextBox1 = new PhoneticBengaliInput.PBITextBox();
            this.pbiComboBox1 = new PhoneticBengaliInput.PBIComboBox();
            this.SuspendLayout();
            // 
            // pbiTextBox1
            // 
            this.pbiTextBox1.BengaliPhoneticText = "";
            this.pbiTextBox1.CurrentLanguage = PhoneticBengaliInput.Language.Bn;
            this.pbiTextBox1.Enabled = false;
            this.pbiTextBox1.EnglishText = "";
            this.pbiTextBox1.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.pbiTextBox1.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.pbiTextBox1.Location = new System.Drawing.Point(27, 51);
            this.pbiTextBox1.Name = "pbiTextBox1";
            this.pbiTextBox1.ShowLanguageSwitcherButton = true;
            this.pbiTextBox1.Size = new System.Drawing.Size(291, 54);
            this.pbiTextBox1.TabIndex = 0;
            // 
            // pbiComboBox1
            // 
            this.pbiComboBox1.BengaliPhoneticText = "";
            this.pbiComboBox1.CurrentLanguage = PhoneticBengaliInput.Language.Bn;
            this.pbiComboBox1.Enabled = false;
            this.pbiComboBox1.EnglishText = "";
            this.pbiComboBox1.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.pbiComboBox1.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.pbiComboBox1.Location = new System.Drawing.Point(27, 111);
            this.pbiComboBox1.Name = "pbiComboBox1";
            this.pbiComboBox1.SelectedIndex = -1;
            this.pbiComboBox1.ShowLanguageSwitcherButton = true;
            this.pbiComboBox1.Size = new System.Drawing.Size(291, 55);
            this.pbiComboBox1.TabIndex = 1;
            // 
            // TestDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 237);
            this.Controls.Add(this.pbiComboBox1);
            this.Controls.Add(this.pbiTextBox1);
            this.Name = "TestDialogForm";
            this.Text = "TestDialogForm";
            this.ResumeLayout(false);

        }

        #endregion

        private PhoneticBengaliInput.PBITextBox pbiTextBox1;
        private PhoneticBengaliInput.PBIComboBox pbiComboBox1;
    }
}