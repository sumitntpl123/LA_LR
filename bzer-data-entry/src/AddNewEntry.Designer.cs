namespace bzer_data_entry
{
    partial class AddNewEntry
    {
        private System.ComponentModel.IContainer components = null;



        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.tableLayoutPanelPlot = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOk = new System.Windows.Forms.Button();
            this.listViewPlotBataPlotNumber = new System.Windows.Forms.ListView();
            this.district = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmPlotType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPlotNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSlash = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderBataPlotNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmPTSR = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanelPlotAddRemove = new System.Windows.Forms.TableLayoutPanel();
            this.pbiTextBoxMesurement = new PhoneticBengaliInput.PBITextBox();
            this.pbiComboBoxMouza = new PhoneticBengaliInput.PBIComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pbiComboBoxDistrict = new PhoneticBengaliInput.PBIComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPTSR = new PhoneticBengaliInput.PBITextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pbiTextBoxBataPlotNumber = new PhoneticBengaliInput.PBITextBox();
            this.labelPlotNumber = new System.Windows.Forms.Label();
            this.pbiTextBoxPlotNumber = new PhoneticBengaliInput.PBITextBox();
            this.labelSlash = new System.Windows.Forms.Label();
            this.cmbPlot = new PhoneticBengaliInput.PBIComboBox();
            this.cmbBlock = new PhoneticBengaliInput.PBIComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.cmdRemove = new System.Windows.Forms.Button();
            this.tableLayoutPanelPlot.SuspendLayout();
            this.tableLayoutPanelPlotAddRemove.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelPlot
            // 
            this.tableLayoutPanelPlot.AutoSize = true;
            this.tableLayoutPanelPlot.ColumnCount = 1;
            this.tableLayoutPanelPlot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPlot.Controls.Add(this.buttonOk, 0, 2);
            this.tableLayoutPanelPlot.Controls.Add(this.listViewPlotBataPlotNumber, 0, 1);
            this.tableLayoutPanelPlot.Controls.Add(this.tableLayoutPanelPlotAddRemove, 0, 0);
            this.tableLayoutPanelPlot.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelPlot.Name = "tableLayoutPanelPlot";
            this.tableLayoutPanelPlot.RowCount = 3;
            this.tableLayoutPanelPlot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPlot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPlot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPlot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelPlot.Size = new System.Drawing.Size(1386, 492);
            this.tableLayoutPanelPlot.TabIndex = 0;
            this.tableLayoutPanelPlot.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanelPlot_Paint);
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOk.Location = new System.Drawing.Point(1285, 454);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(98, 35);
            this.buttonOk.TabIndex = 4;
            this.buttonOk.Text = "O&k";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // listViewPlotBataPlotNumber
            // 
            this.listViewPlotBataPlotNumber.AllowColumnReorder = true;
            this.listViewPlotBataPlotNumber.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.district,
            this.columnHeader2,
            this.columnHeader3,
            this.clmPlotType,
            this.columnHeaderPlotNumber,
            this.columnHeaderSlash,
            this.columnHeaderBataPlotNumber,
            this.clmPTSR,
            this.columnHeader1});
            this.listViewPlotBataPlotNumber.FullRowSelect = true;
            this.listViewPlotBataPlotNumber.GridLines = true;
            this.listViewPlotBataPlotNumber.HideSelection = false;
            this.listViewPlotBataPlotNumber.Location = new System.Drawing.Point(3, 76);
            this.listViewPlotBataPlotNumber.Name = "listViewPlotBataPlotNumber";
            this.listViewPlotBataPlotNumber.Size = new System.Drawing.Size(1375, 354);
            this.listViewPlotBataPlotNumber.TabIndex = 3;
            this.listViewPlotBataPlotNumber.UseCompatibleStateImageBehavior = false;
            this.listViewPlotBataPlotNumber.View = System.Windows.Forms.View.Details;
            this.listViewPlotBataPlotNumber.SelectedIndexChanged += new System.EventHandler(this.listViewPlotBataPlotNumber_SelectedIndexChanged);
            this.listViewPlotBataPlotNumber.Resize += new System.EventHandler(this.listViewPlotBataPlotNumber_Resize);
            // 
            // district
            // 
            this.district.Text = "District";
            this.district.Width = 250;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Block";
            this.columnHeader2.Width = 250;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Mouza";
            this.columnHeader3.Width = 250;
            // 
            // clmPlotType
            // 
            this.clmPlotType.Text = "Plot Type";
            this.clmPlotType.Width = 150;
            // 
            // columnHeaderPlotNumber
            // 
            this.columnHeaderPlotNumber.Text = "Plot number";
            this.columnHeaderPlotNumber.Width = 120;
            // 
            // columnHeaderSlash
            // 
            this.columnHeaderSlash.Text = "/";
            this.columnHeaderSlash.Width = 90;
            // 
            // columnHeaderBataPlotNumber
            // 
            this.columnHeaderBataPlotNumber.Text = "Bata Plot Number";
            this.columnHeaderBataPlotNumber.Width = 160;
            // 
            // clmPTSR
            // 
            this.clmPTSR.Text = "PTSR";
            this.clmPTSR.Width = 160;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Measurement";
            this.columnHeader1.Width = 250;
            // 
            // tableLayoutPanelPlotAddRemove
            // 
            this.tableLayoutPanelPlotAddRemove.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tableLayoutPanelPlotAddRemove.ColumnCount = 18;
            this.tableLayoutPanelPlotAddRemove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.tableLayoutPanelPlotAddRemove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tableLayoutPanelPlotAddRemove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanelPlotAddRemove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.tableLayoutPanelPlotAddRemove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanelPlotAddRemove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.tableLayoutPanelPlotAddRemove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanelPlotAddRemove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 97F));
            this.tableLayoutPanelPlotAddRemove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPlotAddRemove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelPlotAddRemove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.303725F));
            this.tableLayoutPanelPlotAddRemove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.93983F));
            this.tableLayoutPanelPlotAddRemove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanelPlotAddRemove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanelPlotAddRemove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.82808F));
            this.tableLayoutPanelPlotAddRemove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 144F));
            this.tableLayoutPanelPlotAddRemove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            this.tableLayoutPanelPlotAddRemove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            this.tableLayoutPanelPlotAddRemove.Controls.Add(this.pbiTextBoxMesurement, 15, 0);
            this.tableLayoutPanelPlotAddRemove.Controls.Add(this.pbiComboBoxMouza, 5, 0);
            this.tableLayoutPanelPlotAddRemove.Controls.Add(this.label6, 4, 0);
            this.tableLayoutPanelPlotAddRemove.Controls.Add(this.label5, 2, 0);
            this.tableLayoutPanelPlotAddRemove.Controls.Add(this.pbiComboBoxDistrict, 1, 0);
            this.tableLayoutPanelPlotAddRemove.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanelPlotAddRemove.Controls.Add(this.txtPTSR, 13, 0);
            this.tableLayoutPanelPlotAddRemove.Controls.Add(this.label3, 12, 0);
            this.tableLayoutPanelPlotAddRemove.Controls.Add(this.label1, 14, 0);
            this.tableLayoutPanelPlotAddRemove.Controls.Add(this.pbiTextBoxBataPlotNumber, 11, 0);
            this.tableLayoutPanelPlotAddRemove.Controls.Add(this.labelPlotNumber, 8, 0);
            this.tableLayoutPanelPlotAddRemove.Controls.Add(this.pbiTextBoxPlotNumber, 9, 0);
            this.tableLayoutPanelPlotAddRemove.Controls.Add(this.labelSlash, 10, 0);
            this.tableLayoutPanelPlotAddRemove.Controls.Add(this.cmbPlot, 7, 0);
            this.tableLayoutPanelPlotAddRemove.Controls.Add(this.cmbBlock, 3, 0);
            this.tableLayoutPanelPlotAddRemove.Controls.Add(this.label2, 6, 0);
            this.tableLayoutPanelPlotAddRemove.Controls.Add(this.buttonAdd, 16, 0);
            this.tableLayoutPanelPlotAddRemove.Controls.Add(this.cmdRemove, 17, 0);
            this.tableLayoutPanelPlotAddRemove.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelPlotAddRemove.Name = "tableLayoutPanelPlotAddRemove";
            this.tableLayoutPanelPlotAddRemove.RowCount = 1;
            this.tableLayoutPanelPlotAddRemove.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 52.94118F));
            this.tableLayoutPanelPlotAddRemove.Size = new System.Drawing.Size(1375, 67);
            this.tableLayoutPanelPlotAddRemove.TabIndex = 2;
            this.tableLayoutPanelPlotAddRemove.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanelPlotAddRemove_Paint);
            // 
            // pbiTextBoxMesurement
            // 
            this.pbiTextBoxMesurement.BengaliPhoneticText = "";
            this.pbiTextBoxMesurement.CurrentLanguage = PhoneticBengaliInput.Language.En;
            this.pbiTextBoxMesurement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbiTextBoxMesurement.Enabled = false;
            this.pbiTextBoxMesurement.EnglishText = "";
            this.pbiTextBoxMesurement.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.pbiTextBoxMesurement.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.pbiTextBoxMesurement.Location = new System.Drawing.Point(1012, 3);
            this.pbiTextBoxMesurement.Name = "pbiTextBoxMesurement";
            this.pbiTextBoxMesurement.ShowLanguageSwitcherButton = false;
            this.pbiTextBoxMesurement.ShowSuggestions = false;
            this.pbiTextBoxMesurement.Size = new System.Drawing.Size(138, 61);
            this.pbiTextBoxMesurement.TabIndex = 21;
            // 
            // pbiComboBoxMouza
            // 
            this.pbiComboBoxMouza.BengaliPhoneticText = "";
            this.pbiComboBoxMouza.CurrentLanguage = PhoneticBengaliInput.Language.En;
            this.pbiComboBoxMouza.Enabled = false;
            this.pbiComboBoxMouza.EnglishText = "";
            this.pbiComboBoxMouza.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.pbiComboBoxMouza.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.pbiComboBoxMouza.Location = new System.Drawing.Point(340, 3);
            this.pbiComboBoxMouza.Name = "pbiComboBoxMouza";
            this.pbiComboBoxMouza.SelectedIndex = -1;
            this.pbiComboBoxMouza.ShowLanguageSwitcherButton = false;
            this.pbiComboBoxMouza.Size = new System.Drawing.Size(63, 34);
            this.pbiComboBoxMouza.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Location = new System.Drawing.Point(291, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Mouza";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label5.Location = new System.Drawing.Point(153, 6);
            this.label5.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 15);
            this.label5.TabIndex = 14;
            this.label5.Text = "Block";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pbiComboBoxDistrict
            // 
            this.pbiComboBoxDistrict.BengaliPhoneticText = "";
            this.pbiComboBoxDistrict.CurrentLanguage = PhoneticBengaliInput.Language.En;
            this.pbiComboBoxDistrict.Enabled = false;
            this.pbiComboBoxDistrict.EnglishText = "";
            this.pbiComboBoxDistrict.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.pbiComboBoxDistrict.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.pbiComboBoxDistrict.Location = new System.Drawing.Point(76, 3);
            this.pbiComboBoxDistrict.Name = "pbiComboBoxDistrict";
            this.pbiComboBoxDistrict.SelectedIndex = -1;
            this.pbiComboBoxDistrict.ShowLanguageSwitcherButton = false;
            this.pbiComboBoxDistrict.Size = new System.Drawing.Size(63, 34);
            this.pbiComboBoxDistrict.TabIndex = 13;
            this.pbiComboBoxDistrict.Leave += new System.EventHandler(this.pbiComboBoxDistrict_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label4.Location = new System.Drawing.Point(3, 6);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 12;
            this.label4.Text = "District";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPTSR
            // 
            this.txtPTSR.BengaliPhoneticText = "";
            this.txtPTSR.CurrentLanguage = PhoneticBengaliInput.Language.En;
            this.txtPTSR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPTSR.Enabled = false;
            this.txtPTSR.EnglishText = "";
            this.txtPTSR.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.txtPTSR.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.txtPTSR.Location = new System.Drawing.Point(871, 3);
            this.txtPTSR.Name = "txtPTSR";
            this.txtPTSR.ShowLanguageSwitcherButton = false;
            this.txtPTSR.ShowSuggestions = false;
            this.txtPTSR.Size = new System.Drawing.Size(2, 61);
            this.txtPTSR.TabIndex = 10;
            this.txtPTSR.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label3.Location = new System.Drawing.Point(863, 6);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(2, 60);
            this.label3.TabIndex = 9;
            this.label3.Text = "PSTR";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label3.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.Location = new System.Drawing.Point(879, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Measurement";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pbiTextBoxBataPlotNumber
            // 
            this.pbiTextBoxBataPlotNumber.BengaliPhoneticText = "";
            this.pbiTextBoxBataPlotNumber.CurrentLanguage = PhoneticBengaliInput.Language.En;
            this.pbiTextBoxBataPlotNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbiTextBoxBataPlotNumber.Enabled = false;
            this.pbiTextBoxBataPlotNumber.EnglishText = "";
            this.pbiTextBoxBataPlotNumber.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.pbiTextBoxBataPlotNumber.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.pbiTextBoxBataPlotNumber.Location = new System.Drawing.Point(766, 3);
            this.pbiTextBoxBataPlotNumber.Name = "pbiTextBoxBataPlotNumber";
            this.pbiTextBoxBataPlotNumber.ShowLanguageSwitcherButton = false;
            this.pbiTextBoxBataPlotNumber.ShowSuggestions = false;
            this.pbiTextBoxBataPlotNumber.Size = new System.Drawing.Size(91, 61);
            this.pbiTextBoxBataPlotNumber.TabIndex = 2;
            this.pbiTextBoxBataPlotNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.pbiTextBoxBataPlotNumber_KeyDown);
            this.pbiTextBoxBataPlotNumber.Leave += new System.EventHandler(this.pbiTextBoxBataPlotNumber_Leave);
            // 
            // labelPlotNumber
            // 
            this.labelPlotNumber.AutoSize = true;
            this.labelPlotNumber.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelPlotNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlotNumber.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelPlotNumber.Location = new System.Drawing.Point(577, 6);
            this.labelPlotNumber.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelPlotNumber.Name = "labelPlotNumber";
            this.labelPlotNumber.Size = new System.Drawing.Size(79, 15);
            this.labelPlotNumber.TabIndex = 0;
            this.labelPlotNumber.Text = "Plot Number:";
            this.labelPlotNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pbiTextBoxPlotNumber
            // 
            this.pbiTextBoxPlotNumber.BengaliPhoneticText = "";
            this.pbiTextBoxPlotNumber.CurrentLanguage = PhoneticBengaliInput.Language.En;
            this.pbiTextBoxPlotNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbiTextBoxPlotNumber.Enabled = false;
            this.pbiTextBoxPlotNumber.EnglishText = "";
            this.pbiTextBoxPlotNumber.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.pbiTextBoxPlotNumber.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.pbiTextBoxPlotNumber.Location = new System.Drawing.Point(662, 3);
            this.pbiTextBoxPlotNumber.Name = "pbiTextBoxPlotNumber";
            this.pbiTextBoxPlotNumber.ShowLanguageSwitcherButton = false;
            this.pbiTextBoxPlotNumber.ShowSuggestions = false;
            this.pbiTextBoxPlotNumber.Size = new System.Drawing.Size(77, 61);
            this.pbiTextBoxPlotNumber.TabIndex = 1;
            this.pbiTextBoxPlotNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.pbiTextBoxPlotNumber_KeyDown);
            this.pbiTextBoxPlotNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.pbiTextBoxPlotNumber_KeyPress);
            this.pbiTextBoxPlotNumber.Leave += new System.EventHandler(this.pbiTextBoxPlotNumber_Leave);
            // 
            // labelSlash
            // 
            this.labelSlash.AutoSize = true;
            this.labelSlash.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSlash.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSlash.Location = new System.Drawing.Point(745, 5);
            this.labelSlash.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.labelSlash.Name = "labelSlash";
            this.labelSlash.Size = new System.Drawing.Size(15, 24);
            this.labelSlash.TabIndex = 2;
            this.labelSlash.Text = "/";
            this.labelSlash.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cmbPlot
            // 
            this.cmbPlot.BengaliPhoneticText = "";
            this.cmbPlot.CurrentLanguage = PhoneticBengaliInput.Language.En;
            this.cmbPlot.Enabled = false;
            this.cmbPlot.EnglishText = "";
            this.cmbPlot.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.cmbPlot.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.cmbPlot.Location = new System.Drawing.Point(480, 3);
            this.cmbPlot.Name = "cmbPlot";
            this.cmbPlot.SelectedIndex = -1;
            this.cmbPlot.ShowLanguageSwitcherButton = false;
            this.cmbPlot.Size = new System.Drawing.Size(80, 34);
            this.cmbPlot.TabIndex = 11;
            // 
            // cmbBlock
            // 
            this.cmbBlock.BengaliPhoneticText = "";
            this.cmbBlock.CurrentLanguage = PhoneticBengaliInput.Language.En;
            this.cmbBlock.Enabled = false;
            this.cmbBlock.EnglishText = "";
            this.cmbBlock.KeyboardLayoutHelpShortcut = System.Windows.Forms.Shortcut.F1;
            this.cmbBlock.LanguageSwitchShortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.cmbBlock.Location = new System.Drawing.Point(207, 3);
            this.cmbBlock.Name = "cmbBlock";
            this.cmbBlock.SelectedIndex = -1;
            this.cmbBlock.ShowLanguageSwitcherButton = false;
            this.cmbBlock.Size = new System.Drawing.Size(63, 34);
            this.cmbBlock.TabIndex = 15;
            this.cmbBlock.Leave += new System.EventHandler(this.cmbBlock_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(416, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "PLot TYpe";
            // 
            // buttonAdd
            // 
            this.buttonAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAdd.Location = new System.Drawing.Point(1156, 3);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(106, 34);
            this.buttonAdd.TabIndex = 4;
            this.buttonAdd.Text = "&Add";
            this.buttonAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // cmdRemove
            // 
            this.cmdRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdRemove.ForeColor = System.Drawing.Color.Red;
            this.cmdRemove.Location = new System.Drawing.Point(1268, 3);
            this.cmdRemove.Name = "cmdRemove";
            this.cmdRemove.Size = new System.Drawing.Size(82, 34);
            this.cmdRemove.TabIndex = 20;
            this.cmdRemove.Text = "&Remove";
            this.cmdRemove.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdRemove.UseVisualStyleBackColor = true;
            // 
            // AddNewEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1360, 492);
            this.Controls.Add(this.tableLayoutPanelPlot);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(453, 531);
            this.Name = "AddNewEntry";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Plot Addition Form";
            this.Load += new System.EventHandler(this.AddNewEntry_Load);
            this.tableLayoutPanelPlot.ResumeLayout(false);
            this.tableLayoutPanelPlotAddRemove.ResumeLayout(false);
            this.tableLayoutPanelPlotAddRemove.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPlot;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPlotAddRemove;
    private System.Windows.Forms.Label labelPlotNumber;
    private PhoneticBengaliInput.PBITextBox pbiTextBoxPlotNumber;
    private System.Windows.Forms.ListView listViewPlotBataPlotNumber;
    private System.Windows.Forms.Button buttonOk;
    private System.Windows.Forms.Label labelSlash;
    private PhoneticBengaliInput.PBITextBox pbiTextBoxBataPlotNumber;
    private System.Windows.Forms.Button buttonAdd;
    private System.Windows.Forms.ColumnHeader columnHeaderPlotNumber;
    private System.Windows.Forms.ColumnHeader columnHeaderSlash;
    private System.Windows.Forms.ColumnHeader columnHeaderBataPlotNumber;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ColumnHeader columnHeaderMeasurement;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private PhoneticBengaliInput.PBITextBox txtPTSR;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ColumnHeader clmPlotType;
    private System.Windows.Forms.ColumnHeader clmPTSR;
    private PhoneticBengaliInput.PBIComboBox cmbPlot;
    private System.Windows.Forms.ColumnHeader district;
    private System.Windows.Forms.ColumnHeader Block;
    private System.Windows.Forms.ColumnHeader Mauza;
    private System.Windows.Forms.Label label5;
    private PhoneticBengaliInput.PBIComboBox pbiComboBoxDistrict;
    private System.Windows.Forms.Label label4;
    private PhoneticBengaliInput.PBIComboBox cmbBlock;
    private System.Windows.Forms.Label label2;
    private PhoneticBengaliInput.PBIComboBox pbiComboBoxMouza;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Button cmdRemove;
    private PhoneticBengaliInput.PBITextBox pbiTextBoxMesurement;
    private System.Windows.Forms.ColumnHeader clmDistrict;
    private System.Windows.Forms.ColumnHeader clmBlock;
    private System.Windows.Forms.ColumnHeader clmMauza;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}