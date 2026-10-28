namespace ImageHeaven
{
    partial class DocumentTypeListView
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
            this.mainListView = new System.Windows.Forms.ListView();
            this.docType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.shortcutKey = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.count = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // mainListView
            // 
            this.mainListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.docType,
            this.shortcutKey,
            this.count});
            this.mainListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainListView.FullRowSelect = true;
            this.mainListView.GridLines = true;
            this.mainListView.HideSelection = false;
            this.mainListView.Location = new System.Drawing.Point(0, 0);
            this.mainListView.MultiSelect = false;
            this.mainListView.Name = "mainListView";
            this.mainListView.Size = new System.Drawing.Size(197, 185);
            this.mainListView.TabIndex = 0;
            this.mainListView.UseCompatibleStateImageBehavior = false;
            this.mainListView.View = System.Windows.Forms.View.Details;
            // 
            // docType
            // 
            this.docType.Text = "Document Type";
            // 
            // shortcutKey
            // 
            this.shortcutKey.Text = "Keys";
            // 
            // count
            // 
            this.count.Text = "Count";
            // 
            // DocumentTypeListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainListView);
            this.Name = "DocumentTypeListView";
            this.Size = new System.Drawing.Size(197, 185);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView mainListView;
        private System.Windows.Forms.ColumnHeader docType;
        private System.Windows.Forms.ColumnHeader shortcutKey;
        private System.Windows.Forms.ColumnHeader count;
    }
}
