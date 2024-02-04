namespace Rina.Client
{
    partial class pForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(pForm));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.currentProgress = new System.Windows.Forms.ProgressBar();
            this.completeProgressText = new System.Windows.Forms.Label();
            this.currentProgressText = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.completeProgress = new System.Windows.Forms.ProgressBar();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Status});
            this.statusStrip.Location = new System.Drawing.Point(0, 187);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(476, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "Status";
            // 
            // Status
            // 
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(0, 17);
            // 
            // currentProgress
            // 
            this.currentProgress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.currentProgress.Location = new System.Drawing.Point(27, 84);
            this.currentProgress.Name = "currentProgress";
            this.currentProgress.Size = new System.Drawing.Size(416, 21);
            this.currentProgress.TabIndex = 2;
            this.currentProgress.Click += new System.EventHandler(this.currentProgress_Click);
            // 
            // completeProgressText
            // 
            this.completeProgressText.AutoSize = true;
            this.completeProgressText.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.completeProgressText.Location = new System.Drawing.Point(24, 68);
            this.completeProgressText.Name = "completeProgressText";
            this.completeProgressText.Size = new System.Drawing.Size(101, 13);
            this.completeProgressText.TabIndex = 3;
            this.completeProgressText.Text = "Toplam İlerleme: 0%";
            this.completeProgressText.Click += new System.EventHandler(this.completeProgressText_Click);
            // 
            // currentProgressText
            // 
            this.currentProgressText.AutoSize = true;
            this.currentProgressText.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.currentProgressText.Location = new System.Drawing.Point(24, 118);
            this.currentProgressText.Name = "currentProgressText";
            this.currentProgressText.Size = new System.Drawing.Size(163, 13);
            this.currentProgressText.TabIndex = 4;
            this.currentProgressText.Text = "Dosya İlerlemesi: 0%  |  0.00 kb/s";
            this.currentProgressText.Click += new System.EventHandler(this.currentProgressText_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(151, -21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Dosya Yükleme Aracı";
            // 
            // completeProgress
            // 
            this.completeProgress.Location = new System.Drawing.Point(27, 134);
            this.completeProgress.Name = "completeProgress";
            this.completeProgress.Size = new System.Drawing.Size(416, 21);
            this.completeProgress.TabIndex = 7;
            // 
            // pForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 209);
            this.Controls.Add(this.completeProgress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.currentProgressText);
            this.Controls.Add(this.completeProgressText);
            this.Controls.Add(this.currentProgress);
            this.Controls.Add(this.statusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(476, 209);
            this.MinimumSize = new System.Drawing.Size(476, 209);
            this.Movable = false;
            this.Name = "pForm";
            this.Padding = new System.Windows.Forms.Padding(0, 60, 0, 0);
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.AeroShadow;
            this.Style = MetroFramework.MetroColorStyle.Silver;
            this.Text = "Patcher";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.Load += new System.EventHandler(this.pForm_Load);
            this.Shown += new System.EventHandler(this.pForm_Shown);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        public  System.Windows.Forms.ToolStripStatusLabel Status;
        public  System.Windows.Forms.ProgressBar currentProgress;
        public  System.Windows.Forms.Label completeProgressText;
        public  System.Windows.Forms.Label currentProgressText;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ProgressBar completeProgress;
    }
}

