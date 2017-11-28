namespace SendMedia
{
    partial class MainForm
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
            this.gridImages = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // gridImages
            // 
            this.gridImages.AutoScroll = true;
            this.gridImages.BackColor = System.Drawing.Color.White;
            this.gridImages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gridImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridImages.Location = new System.Drawing.Point(0, 0);
            this.gridImages.Name = "gridImages";
            this.gridImages.Size = new System.Drawing.Size(637, 345);
            this.gridImages.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 345);
            this.Controls.Add(this.gridImages);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HelpButton = true;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Insert Media";
            this.TopMost = true;
            this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Leave += new System.EventHandler(this.MainForm_Leave);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel gridImages;
    }
}

