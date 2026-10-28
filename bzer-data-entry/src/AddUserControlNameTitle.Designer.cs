namespace bzer_data_entry
{
    partial class AddUserControlNameTitle
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
            this.addNameTitle = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // addNameTitle
            // 
            this.addNameTitle.BackColor = System.Drawing.Color.NavajoWhite;
            this.addNameTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addNameTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addNameTitle.Image = global::bzer_data_entry.Properties.Resources.plus;
            this.addNameTitle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addNameTitle.Location = new System.Drawing.Point(0, 0);
            this.addNameTitle.Name = "addNameTitle";
            this.addNameTitle.Size = new System.Drawing.Size(207, 40);
            this.addNameTitle.TabIndex = 0;
            this.addNameTitle.Text = "Doyel";
            this.addNameTitle.UseVisualStyleBackColor = false;
            this.addNameTitle.Click += new System.EventHandler(this.addNameTitle_Click);
            // 
            // AddUserControlNameTitle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.addNameTitle);
            this.Name = "AddUserControlNameTitle";
            this.Size = new System.Drawing.Size(207, 40);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button addNameTitle;
    }
}
