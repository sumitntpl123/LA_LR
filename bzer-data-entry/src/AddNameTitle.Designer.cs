namespace bzer_data_entry
{
    partial class AddNameTitle
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.labelKhatian = new System.Windows.Forms.Label();
            this.buttonDel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.pbiTextBoxName = new PhoneticBengaliInput.PBITextBox();
            this.pbiTextBoxTitle = new PhoneticBengaliInput.PBITextBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.pbiTextBoxKhatian = new PhoneticBengaliInput.PBITextBox();
            this.labelBataKhatian = new System.Windows.Forms.Label();
            this.pbiTextBoxBataKhatian = new PhoneticBengaliInput.PBITextBox();
            this.listViewNameTitleKhatianBata = new System.Windows.Forms.ListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderKhatian = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSlash = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderBataKhatian = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOk = new System.Windows.Forms.Button();
            this.labelLoadingSuggestions = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.listViewNameTitleKhatianBata, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 42.47787F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 57.52213F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(541, 541);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 83F));
            this.tableLayoutPanel2.Controls.Add(this.labelKhatian, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.buttonDel, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelName, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.pbiTextBoxName, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.pbiTextBoxTitle, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.buttonAdd, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.pbiTextBoxKhatian, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.labelBataKhatian, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.pbiTextBoxBataKhatian, 1, 3);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(38, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(465, 205);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // labelKhatian
            // 
            this.labelKhatian.AutoSize = true;
            this.labelKhatian.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelKhatian.Location = new System.Drawing.Point(3, 112);
            this.labelKhatian.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.labelKhatian.Name = "labelKhatian";
            this.labelKhatian.Size = new System.Drawing.Size(72, 13);
            this.labelKhatian.TabIndex = 5;
            this.labelKhatian.Text = "খতিয়ান:";
            // 
            // buttonDel
            // 
            this.buttonDel.BackColor = System.Drawing.SystemColors.MenuBar;
            this.buttonDel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDel.ForeColor = System.Drawing.Color.Red;
            this.buttonDel.Location = new System.Drawing.Point(385, 54);
            this.buttonDel.Name = "buttonDel";
            this.buttonDel.Size = new System.Drawing.Size(75, 40);
            this.buttonDel.TabIndex = 5;
            this.buttonDel.Text = "Delete";
            this.buttonDel.UseVisualStyleBackColor = false;
            this.buttonDel.Click += new System.EventHandler(this.buttonDel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(3, 61);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "পদবি:";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelName.Location = new System.Drawing.Point(3, 10);
            this.labelName.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(72, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "নাম:";
            // 
            // pbiTextBoxName
            // 
            this.pbiTextBoxName.BengaliPhoneticText = "";
            this.pbiTextBoxName.CurrentLanguage = PhoneticBengaliInput.Language.En;
            this.pbiTextBoxName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbiTextBoxName.Enabled = false;
            this.pbiTextBoxName.EnglishText = "";
            this.pbiTextBoxName.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.pbiTextBoxName.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.pbiTextBoxName.Location = new System.Drawing.Point(81, 3);
            this.pbiTextBoxName.Name = "pbiTextBoxName";
            this.pbiTextBoxName.ShowLanguageSwitcherButton = false;
            this.pbiTextBoxName.ShowSuggestions = true;
            this.pbiTextBoxName.Size = new System.Drawing.Size(298, 45);
            this.pbiTextBoxName.TabIndex = 0;
            // 
            // pbiTextBoxTitle
            // 
            this.pbiTextBoxTitle.BengaliPhoneticText = "";
            this.pbiTextBoxTitle.CurrentLanguage = PhoneticBengaliInput.Language.En;
            this.pbiTextBoxTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbiTextBoxTitle.Enabled = false;
            this.pbiTextBoxTitle.EnglishText = "";
            this.pbiTextBoxTitle.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.pbiTextBoxTitle.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.pbiTextBoxTitle.Location = new System.Drawing.Point(81, 54);
            this.pbiTextBoxTitle.Name = "pbiTextBoxTitle";
            this.pbiTextBoxTitle.ShowLanguageSwitcherButton = false;
            this.pbiTextBoxTitle.ShowSuggestions = true;
            this.pbiTextBoxTitle.Size = new System.Drawing.Size(298, 45);
            this.pbiTextBoxTitle.TabIndex = 1;
            // 
            // buttonAdd
            // 
            this.buttonAdd.BackColor = System.Drawing.SystemColors.MenuBar;
            this.buttonAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAdd.Location = new System.Drawing.Point(385, 3);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 40);
            this.buttonAdd.TabIndex = 4;
            this.buttonAdd.Text = "&Add";
            this.buttonAdd.UseVisualStyleBackColor = false;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // pbiTextBoxKhatian
            // 
            this.pbiTextBoxKhatian.BengaliPhoneticText = "";
            this.pbiTextBoxKhatian.CurrentLanguage = PhoneticBengaliInput.Language.En;
            this.pbiTextBoxKhatian.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbiTextBoxKhatian.Enabled = false;
            this.pbiTextBoxKhatian.EnglishText = "";
            this.pbiTextBoxKhatian.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.pbiTextBoxKhatian.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.pbiTextBoxKhatian.Location = new System.Drawing.Point(81, 105);
            this.pbiTextBoxKhatian.Name = "pbiTextBoxKhatian";
            this.pbiTextBoxKhatian.ShowLanguageSwitcherButton = false;
            this.pbiTextBoxKhatian.ShowSuggestions = false;
            this.pbiTextBoxKhatian.Size = new System.Drawing.Size(298, 45);
            this.pbiTextBoxKhatian.TabIndex = 2;
            // 
            // labelBataKhatian
            // 
            this.labelBataKhatian.AutoSize = true;
            this.labelBataKhatian.Location = new System.Drawing.Point(3, 163);
            this.labelBataKhatian.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.labelBataKhatian.Name = "labelBataKhatian";
            this.labelBataKhatian.Size = new System.Drawing.Size(72, 13);
            this.labelBataKhatian.TabIndex = 6;
            this.labelBataKhatian.Text = "বাটা খতিয়ান:";
            // 
            // pbiTextBoxBataKhatian
            // 
            this.pbiTextBoxBataKhatian.BengaliPhoneticText = "";
            this.pbiTextBoxBataKhatian.CurrentLanguage = PhoneticBengaliInput.Language.En;
            this.pbiTextBoxBataKhatian.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbiTextBoxBataKhatian.Enabled = false;
            this.pbiTextBoxBataKhatian.EnglishText = "";
            this.pbiTextBoxBataKhatian.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.pbiTextBoxBataKhatian.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.pbiTextBoxBataKhatian.Location = new System.Drawing.Point(81, 156);
            this.pbiTextBoxBataKhatian.Name = "pbiTextBoxBataKhatian";
            this.pbiTextBoxBataKhatian.ShowLanguageSwitcherButton = false;
            this.pbiTextBoxBataKhatian.ShowSuggestions = false;
            this.pbiTextBoxBataKhatian.Size = new System.Drawing.Size(298, 46);
            this.pbiTextBoxBataKhatian.TabIndex = 3;
            // 
            // listViewNameTitleKhatianBata
            // 
            this.listViewNameTitleKhatianBata.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderTitle,
            this.columnHeaderKhatian,
            this.columnHeaderSlash,
            this.columnHeaderBataKhatian});
            this.listViewNameTitleKhatianBata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewNameTitleKhatianBata.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewNameTitleKhatianBata.FullRowSelect = true;
            this.listViewNameTitleKhatianBata.GridLines = true;
            this.listViewNameTitleKhatianBata.HideSelection = false;
            this.listViewNameTitleKhatianBata.LabelEdit = true;
            this.listViewNameTitleKhatianBata.Location = new System.Drawing.Point(3, 214);
            this.listViewNameTitleKhatianBata.Name = "listViewNameTitleKhatianBata";
            this.listViewNameTitleKhatianBata.Size = new System.Drawing.Size(535, 280);
            this.listViewNameTitleKhatianBata.TabIndex = 6;
            this.listViewNameTitleKhatianBata.UseCompatibleStateImageBehavior = false;
            this.listViewNameTitleKhatianBata.View = System.Windows.Forms.View.Details;
            this.listViewNameTitleKhatianBata.SelectedIndexChanged += new System.EventHandler(this.listViewNameTitleKhatianBata_SelectedIndexChanged);
            this.listViewNameTitleKhatianBata.Resize += new System.EventHandler(this.listViewNameTitleKhatianBata_Resize);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "নাম";
            this.columnHeaderName.Width = 87;
            // 
            // columnHeaderTitle
            // 
            this.columnHeaderTitle.Text = "পদবি";
            this.columnHeaderTitle.Width = 150;
            // 
            // columnHeaderKhatian
            // 
            this.columnHeaderKhatian.Text = "খতিয়ান";
            this.columnHeaderKhatian.Width = 150;
            // 
            // columnHeaderSlash
            // 
            this.columnHeaderSlash.Text = "/";
            this.columnHeaderSlash.Width = 122;
            // 
            // columnHeaderBataKhatian
            // 
            this.columnHeaderBataKhatian.Text = "বাটা খতিয়ান";
            this.columnHeaderBataKhatian.Width = 150;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 103F));
            this.tableLayoutPanel3.Controls.Add(this.buttonOk, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.labelLoadingSuggestions, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 500);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(535, 38);
            this.tableLayoutPanel3.TabIndex = 7;
            // 
            // buttonOk
            // 
            this.buttonOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOk.Location = new System.Drawing.Point(435, 3);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(97, 32);
            this.buttonOk.TabIndex = 7;
            this.buttonOk.Text = "O&k";
            this.buttonOk.UseVisualStyleBackColor = false;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // labelLoadingSuggestions
            // 
            this.labelLoadingSuggestions.AutoSize = true;
            this.labelLoadingSuggestions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelLoadingSuggestions.Location = new System.Drawing.Point(3, 25);
            this.labelLoadingSuggestions.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelLoadingSuggestions.Name = "labelLoadingSuggestions";
            this.labelLoadingSuggestions.Size = new System.Drawing.Size(426, 13);
            this.labelLoadingSuggestions.TabIndex = 8;
            this.labelLoadingSuggestions.Text = "Loading Suggestions...";
            this.labelLoadingSuggestions.Visible = false;
            // 
            // AddNameTitle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 541);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(443, 535);
            this.Name = "AddNameTitle";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Add Name and Title";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label label2;
        private PhoneticBengaliInput.PBITextBox pbiTextBoxName;
        private PhoneticBengaliInput.PBITextBox pbiTextBoxTitle;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonDel;
        private System.Windows.Forms.Label labelKhatian;
        private PhoneticBengaliInput.PBITextBox pbiTextBoxKhatian;
        private System.Windows.Forms.ListView listViewNameTitleKhatianBata;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderTitle;
        private System.Windows.Forms.ColumnHeader columnHeaderKhatian;
        private System.Windows.Forms.Label labelBataKhatian;
        private PhoneticBengaliInput.PBITextBox pbiTextBoxBataKhatian;
        private System.Windows.Forms.ColumnHeader columnHeaderSlash;
        private System.Windows.Forms.ColumnHeader columnHeaderBataKhatian;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label labelLoadingSuggestions;
    }
}