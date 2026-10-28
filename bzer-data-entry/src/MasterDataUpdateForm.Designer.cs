namespace bzer_data_entry
{
    partial class MasterDataUpdateForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeViewMasterData = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelDistrict = new System.Windows.Forms.TableLayoutPanel();
            this.labelDistrict = new System.Windows.Forms.Label();
            this.buttonAddDistrict = new System.Windows.Forms.Button();
            this.pbiTextBoxDistrict = new PhoneticBengaliInput.PBITextBox();
            this.tableLayoutPanelBlock = new System.Windows.Forms.TableLayoutPanel();
            this.labelBlock = new System.Windows.Forms.Label();
            this.buttonAddBlock = new System.Windows.Forms.Button();
            this.pbiTextBoxBlock = new PhoneticBengaliInput.PBITextBox();
            this.tableLayoutPanelMouza = new System.Windows.Forms.TableLayoutPanel();
            this.labelMouza = new System.Windows.Forms.Label();
            this.buttonAddMouza = new System.Windows.Forms.Button();
            this.pbiTextBoxMouza = new PhoneticBengaliInput.PBITextBox();
            this.tableLayoutPanelCode = new System.Windows.Forms.TableLayoutPanel();
            this.pbiTextBoxCode = new PhoneticBengaliInput.PBITextBox();
            this.labelCode = new System.Windows.Forms.Label();
            this.tableLayoutPanelRemove = new System.Windows.Forms.TableLayoutPanel();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.tableLayoutPanelProjaSt = new System.Windows.Forms.TableLayoutPanel();
            this.listViewProjaSt = new System.Windows.Forms.ListView();
            this.columnHeaderProjaSt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanelDistrict.SuspendLayout();
            this.tableLayoutPanelBlock.SuspendLayout();
            this.tableLayoutPanelMouza.SuspendLayout();
            this.tableLayoutPanelCode.SuspendLayout();
            this.tableLayoutPanelRemove.SuspendLayout();
            this.tableLayoutPanelProjaSt.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeViewMasterData);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel3);
            this.splitContainer1.Size = new System.Drawing.Size(919, 450);
            this.splitContainer1.SplitterDistance = 305;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeViewMasterData
            // 
            this.treeViewMasterData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewMasterData.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeViewMasterData.Location = new System.Drawing.Point(0, 0);
            this.treeViewMasterData.Name = "treeViewMasterData";
            this.treeViewMasterData.Size = new System.Drawing.Size(305, 450);
            this.treeViewMasterData.TabIndex = 9;
            this.treeViewMasterData.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewMasterData_AfterSelect);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanelDistrict, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanelBlock, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanelMouza, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanelCode, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanelRemove, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanelProjaSt, 0, 5);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 6;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(610, 450);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // tableLayoutPanelDistrict
            // 
            this.tableLayoutPanelDistrict.ColumnCount = 3;
            this.tableLayoutPanelDistrict.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelDistrict.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDistrict.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelDistrict.Controls.Add(this.labelDistrict, 0, 0);
            this.tableLayoutPanelDistrict.Controls.Add(this.buttonAddDistrict, 1, 0);
            this.tableLayoutPanelDistrict.Controls.Add(this.pbiTextBoxDistrict, 1, 0);
            this.tableLayoutPanelDistrict.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDistrict.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelDistrict.Name = "tableLayoutPanelDistrict";
            this.tableLayoutPanelDistrict.RowCount = 1;
            this.tableLayoutPanelDistrict.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDistrict.Size = new System.Drawing.Size(604, 54);
            this.tableLayoutPanelDistrict.TabIndex = 1;
            // 
            // labelDistrict
            // 
            this.labelDistrict.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelDistrict.AutoSize = true;
            this.labelDistrict.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDistrict.Location = new System.Drawing.Point(103, 12);
            this.labelDistrict.Margin = new System.Windows.Forms.Padding(3, 0, 3, 15);
            this.labelDistrict.Name = "labelDistrict";
            this.labelDistrict.Size = new System.Drawing.Size(45, 15);
            this.labelDistrict.TabIndex = 4;
            this.labelDistrict.Text = "Name";
            this.labelDistrict.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonAddDistrict
            // 
            this.buttonAddDistrict.BackColor = System.Drawing.SystemColors.Control;
            this.buttonAddDistrict.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonAddDistrict.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddDistrict.ForeColor = System.Drawing.Color.Black;
            this.buttonAddDistrict.Image = global::bzer_data_entry.Properties.Resources.plus;
            this.buttonAddDistrict.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonAddDistrict.Location = new System.Drawing.Point(456, 5);
            this.buttonAddDistrict.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.buttonAddDistrict.Name = "buttonAddDistrict";
            this.buttonAddDistrict.Size = new System.Drawing.Size(145, 46);
            this.buttonAddDistrict.TabIndex = 3;
            this.buttonAddDistrict.Text = "&Add District";
            this.buttonAddDistrict.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonAddDistrict.UseVisualStyleBackColor = false;
            this.buttonAddDistrict.Click += new System.EventHandler(this.buttonAddDistrict_Click);
            // 
            // pbiTextBoxDistrict
            // 
            this.pbiTextBoxDistrict.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pbiTextBoxDistrict.BengaliPhoneticText = "";
            this.pbiTextBoxDistrict.CurrentLanguage = PhoneticBengaliInput.Language.En;
            this.pbiTextBoxDistrict.Enabled = false;
            this.pbiTextBoxDistrict.EnglishText = "";
            this.pbiTextBoxDistrict.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.pbiTextBoxDistrict.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.pbiTextBoxDistrict.Location = new System.Drawing.Point(154, 3);
            this.pbiTextBoxDistrict.Name = "pbiTextBoxDistrict";
            this.pbiTextBoxDistrict.ShowLanguageSwitcherButton = false;
            this.pbiTextBoxDistrict.ShowSuggestions = true;
            this.pbiTextBoxDistrict.Size = new System.Drawing.Size(296, 48);
            this.pbiTextBoxDistrict.TabIndex = 2;
            this.pbiTextBoxDistrict.PhoneticChanged += new PhoneticBengaliInput.PBIControl.OnPhoneticChanged(this.pbiTextBoxDistrict_PhoneticChanged);
            // 
            // tableLayoutPanelBlock
            // 
            this.tableLayoutPanelBlock.ColumnCount = 3;
            this.tableLayoutPanelBlock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelBlock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelBlock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelBlock.Controls.Add(this.labelBlock, 0, 0);
            this.tableLayoutPanelBlock.Controls.Add(this.buttonAddBlock, 1, 0);
            this.tableLayoutPanelBlock.Controls.Add(this.pbiTextBoxBlock, 1, 0);
            this.tableLayoutPanelBlock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBlock.Location = new System.Drawing.Point(3, 63);
            this.tableLayoutPanelBlock.Name = "tableLayoutPanelBlock";
            this.tableLayoutPanelBlock.RowCount = 1;
            this.tableLayoutPanelBlock.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelBlock.Size = new System.Drawing.Size(604, 52);
            this.tableLayoutPanelBlock.TabIndex = 2;
            // 
            // labelBlock
            // 
            this.labelBlock.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelBlock.AutoSize = true;
            this.labelBlock.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBlock.Location = new System.Drawing.Point(103, 11);
            this.labelBlock.Margin = new System.Windows.Forms.Padding(3, 0, 3, 15);
            this.labelBlock.Name = "labelBlock";
            this.labelBlock.Size = new System.Drawing.Size(45, 15);
            this.labelBlock.TabIndex = 7;
            this.labelBlock.Text = "Name";
            this.labelBlock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonAddBlock
            // 
            this.buttonAddBlock.BackColor = System.Drawing.SystemColors.Control;
            this.buttonAddBlock.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonAddBlock.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddBlock.Image = global::bzer_data_entry.Properties.Resources.plus;
            this.buttonAddBlock.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonAddBlock.Location = new System.Drawing.Point(456, 5);
            this.buttonAddBlock.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.buttonAddBlock.Name = "buttonAddBlock";
            this.buttonAddBlock.Size = new System.Drawing.Size(145, 44);
            this.buttonAddBlock.TabIndex = 6;
            this.buttonAddBlock.Text = "&Add Block";
            this.buttonAddBlock.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonAddBlock.UseVisualStyleBackColor = false;
            this.buttonAddBlock.Click += new System.EventHandler(this.buttonAddBlock_Click);
            // 
            // pbiTextBoxBlock
            // 
            this.pbiTextBoxBlock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pbiTextBoxBlock.BengaliPhoneticText = "";
            this.pbiTextBoxBlock.CurrentLanguage = PhoneticBengaliInput.Language.En;
            this.pbiTextBoxBlock.Enabled = false;
            this.pbiTextBoxBlock.EnglishText = "";
            this.pbiTextBoxBlock.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.pbiTextBoxBlock.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.pbiTextBoxBlock.Location = new System.Drawing.Point(154, 3);
            this.pbiTextBoxBlock.Name = "pbiTextBoxBlock";
            this.pbiTextBoxBlock.ShowLanguageSwitcherButton = false;
            this.pbiTextBoxBlock.ShowSuggestions = false;
            this.pbiTextBoxBlock.Size = new System.Drawing.Size(296, 46);
            this.pbiTextBoxBlock.TabIndex = 4;
            this.pbiTextBoxBlock.PhoneticChanged += new PhoneticBengaliInput.PBIControl.OnPhoneticChanged(this.pbiTextBoxBlock_PhoneticChanged);
            // 
            // tableLayoutPanelMouza
            // 
            this.tableLayoutPanelMouza.ColumnCount = 3;
            this.tableLayoutPanelMouza.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelMouza.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMouza.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelMouza.Controls.Add(this.labelMouza, 0, 0);
            this.tableLayoutPanelMouza.Controls.Add(this.buttonAddMouza, 1, 0);
            this.tableLayoutPanelMouza.Controls.Add(this.pbiTextBoxMouza, 1, 0);
            this.tableLayoutPanelMouza.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMouza.Location = new System.Drawing.Point(3, 121);
            this.tableLayoutPanelMouza.Name = "tableLayoutPanelMouza";
            this.tableLayoutPanelMouza.RowCount = 1;
            this.tableLayoutPanelMouza.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMouza.Size = new System.Drawing.Size(604, 54);
            this.tableLayoutPanelMouza.TabIndex = 3;
            // 
            // labelMouza
            // 
            this.labelMouza.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelMouza.AutoSize = true;
            this.labelMouza.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMouza.Location = new System.Drawing.Point(103, 12);
            this.labelMouza.Margin = new System.Windows.Forms.Padding(3, 0, 3, 15);
            this.labelMouza.Name = "labelMouza";
            this.labelMouza.Size = new System.Drawing.Size(45, 15);
            this.labelMouza.TabIndex = 8;
            this.labelMouza.Text = "Name";
            this.labelMouza.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonAddMouza
            // 
            this.buttonAddMouza.BackColor = System.Drawing.SystemColors.Control;
            this.buttonAddMouza.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonAddMouza.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddMouza.Image = global::bzer_data_entry.Properties.Resources.plus;
            this.buttonAddMouza.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonAddMouza.Location = new System.Drawing.Point(456, 5);
            this.buttonAddMouza.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.buttonAddMouza.Name = "buttonAddMouza";
            this.buttonAddMouza.Size = new System.Drawing.Size(145, 46);
            this.buttonAddMouza.TabIndex = 7;
            this.buttonAddMouza.Text = "&Add Mouza";
            this.buttonAddMouza.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonAddMouza.UseVisualStyleBackColor = false;
            this.buttonAddMouza.Click += new System.EventHandler(this.buttonAddMouza_Click);
            // 
            // pbiTextBoxMouza
            // 
            this.pbiTextBoxMouza.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pbiTextBoxMouza.BengaliPhoneticText = "";
            this.pbiTextBoxMouza.CurrentLanguage = PhoneticBengaliInput.Language.En;
            this.pbiTextBoxMouza.Enabled = false;
            this.pbiTextBoxMouza.EnglishText = "";
            this.pbiTextBoxMouza.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.pbiTextBoxMouza.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.pbiTextBoxMouza.Location = new System.Drawing.Point(154, 3);
            this.pbiTextBoxMouza.Name = "pbiTextBoxMouza";
            this.pbiTextBoxMouza.ShowLanguageSwitcherButton = false;
            this.pbiTextBoxMouza.ShowSuggestions = false;
            this.pbiTextBoxMouza.Size = new System.Drawing.Size(296, 48);
            this.pbiTextBoxMouza.TabIndex = 6;
            this.pbiTextBoxMouza.PhoneticChanged += new PhoneticBengaliInput.PBIControl.OnPhoneticChanged(this.pbiTextBoxMouza_PhoneticChanged);
            // 
            // tableLayoutPanelCode
            // 
            this.tableLayoutPanelCode.ColumnCount = 3;
            this.tableLayoutPanelCode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelCode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelCode.Controls.Add(this.pbiTextBoxCode, 1, 0);
            this.tableLayoutPanelCode.Controls.Add(this.labelCode, 0, 0);
            this.tableLayoutPanelCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelCode.Location = new System.Drawing.Point(3, 181);
            this.tableLayoutPanelCode.Name = "tableLayoutPanelCode";
            this.tableLayoutPanelCode.RowCount = 1;
            this.tableLayoutPanelCode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelCode.Size = new System.Drawing.Size(604, 45);
            this.tableLayoutPanelCode.TabIndex = 11;
            // 
            // pbiTextBoxCode
            // 
            this.pbiTextBoxCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pbiTextBoxCode.BengaliPhoneticText = "";
            this.pbiTextBoxCode.CurrentLanguage = PhoneticBengaliInput.Language.En;
            this.pbiTextBoxCode.Enabled = false;
            this.pbiTextBoxCode.EnglishText = "";
            this.pbiTextBoxCode.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.pbiTextBoxCode.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.pbiTextBoxCode.Location = new System.Drawing.Point(154, 3);
            this.pbiTextBoxCode.Name = "pbiTextBoxCode";
            this.pbiTextBoxCode.ShowLanguageSwitcherButton = false;
            this.pbiTextBoxCode.ShowSuggestions = false;
            this.pbiTextBoxCode.Size = new System.Drawing.Size(296, 39);
            this.pbiTextBoxCode.TabIndex = 0;
            // 
            // labelCode
            // 
            this.labelCode.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelCode.AutoSize = true;
            this.labelCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCode.Location = new System.Drawing.Point(108, 7);
            this.labelCode.Margin = new System.Windows.Forms.Padding(3, 0, 3, 15);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(40, 15);
            this.labelCode.TabIndex = 1;
            this.labelCode.Text = "Code";
            this.labelCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanelRemove
            // 
            this.tableLayoutPanelRemove.ColumnCount = 3;
            this.tableLayoutPanelRemove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelRemove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRemove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelRemove.Controls.Add(this.buttonRemove, 2, 0);
            this.tableLayoutPanelRemove.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelRemove.Location = new System.Drawing.Point(3, 232);
            this.tableLayoutPanelRemove.Name = "tableLayoutPanelRemove";
            this.tableLayoutPanelRemove.RowCount = 1;
            this.tableLayoutPanelRemove.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRemove.Size = new System.Drawing.Size(604, 39);
            this.tableLayoutPanelRemove.TabIndex = 12;
            // 
            // buttonRemove
            // 
            this.buttonRemove.BackColor = System.Drawing.SystemColors.Control;
            this.buttonRemove.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRemove.ForeColor = System.Drawing.Color.Red;
            this.buttonRemove.Image = global::bzer_data_entry.Properties.Resources.minus;
            this.buttonRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRemove.Location = new System.Drawing.Point(456, 5);
            this.buttonRemove.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(145, 32);
            this.buttonRemove.TabIndex = 10;
            this.buttonRemove.Text = "&Remove";
            this.buttonRemove.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonRemove.UseVisualStyleBackColor = false;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // tableLayoutPanelProjaSt
            // 
            this.tableLayoutPanelProjaSt.ColumnCount = 3;
            this.tableLayoutPanelProjaSt.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelProjaSt.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelProjaSt.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelProjaSt.Controls.Add(this.listViewProjaSt, 0, 2);
            this.tableLayoutPanelProjaSt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelProjaSt.Location = new System.Drawing.Point(3, 277);
            this.tableLayoutPanelProjaSt.Name = "tableLayoutPanelProjaSt";
            this.tableLayoutPanelProjaSt.RowCount = 3;
            this.tableLayoutPanelProjaSt.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProjaSt.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProjaSt.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelProjaSt.Size = new System.Drawing.Size(604, 170);
            this.tableLayoutPanelProjaSt.TabIndex = 13;
            this.tableLayoutPanelProjaSt.Visible = false;
            // 
            // listViewProjaSt
            // 
            this.listViewProjaSt.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderProjaSt});
            this.tableLayoutPanelProjaSt.SetColumnSpan(this.listViewProjaSt, 3);
            this.listViewProjaSt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewProjaSt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewProjaSt.GridLines = true;
            this.listViewProjaSt.HideSelection = false;
            this.listViewProjaSt.Location = new System.Drawing.Point(3, 3);
            this.listViewProjaSt.Name = "listViewProjaSt";
            this.listViewProjaSt.Size = new System.Drawing.Size(598, 164);
            this.listViewProjaSt.TabIndex = 12;
            this.listViewProjaSt.UseCompatibleStateImageBehavior = false;
            this.listViewProjaSt.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderProjaSt
            // 
            this.columnHeaderProjaSt.Text = "প্রজাস্বত্বের ধরণ";
            this.columnHeaderProjaSt.Width = 594;
            // 
            // MasterDataUpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(919, 450);
            this.Controls.Add(this.splitContainer1);
            this.MinimumSize = new System.Drawing.Size(816, 489);
            this.Name = "MasterDataUpdateForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Master Data Update";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanelDistrict.ResumeLayout(false);
            this.tableLayoutPanelDistrict.PerformLayout();
            this.tableLayoutPanelBlock.ResumeLayout(false);
            this.tableLayoutPanelBlock.PerformLayout();
            this.tableLayoutPanelMouza.ResumeLayout(false);
            this.tableLayoutPanelMouza.PerformLayout();
            this.tableLayoutPanelCode.ResumeLayout(false);
            this.tableLayoutPanelCode.PerformLayout();
            this.tableLayoutPanelRemove.ResumeLayout(false);
            this.tableLayoutPanelProjaSt.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeViewMasterData;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDistrict;
        private System.Windows.Forms.Button buttonAddDistrict;
        private PhoneticBengaliInput.PBITextBox pbiTextBoxDistrict;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBlock;
        private System.Windows.Forms.Button buttonAddBlock;
        private PhoneticBengaliInput.PBITextBox pbiTextBoxBlock;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMouza;
        private System.Windows.Forms.Button buttonAddMouza;
        private PhoneticBengaliInput.PBITextBox pbiTextBoxMouza;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelCode;
        private PhoneticBengaliInput.PBITextBox pbiTextBoxCode;
        private System.Windows.Forms.Label labelCode;
        private System.Windows.Forms.Label labelDistrict;
        private System.Windows.Forms.Label labelBlock;
        private System.Windows.Forms.Label labelMouza;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRemove;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelProjaSt;
        private System.Windows.Forms.ListView listViewProjaSt;
        private System.Windows.Forms.ColumnHeader columnHeaderProjaSt;
    }
}