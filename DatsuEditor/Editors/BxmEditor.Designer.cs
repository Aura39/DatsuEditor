namespace DatsuEditor.Editors
{
    partial class BxmEditor
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
            this.bxmView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // bxmView
            // 
            this.bxmView.BackColor = System.Drawing.SystemColors.Window;
            this.bxmView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.bxmView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bxmView.Location = new System.Drawing.Point(0, 0);
            this.bxmView.Name = "bxmView";
            this.bxmView.Size = new System.Drawing.Size(439, 444);
            this.bxmView.TabIndex = 0;
            // 
            // BxmEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bxmView);
            this.Name = "BxmEditor";
            this.Size = new System.Drawing.Size(439, 444);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView bxmView;
    }
}
