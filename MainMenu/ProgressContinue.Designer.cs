namespace MainMenu
{
    partial class ProgressContinue
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressContinue));
            this.lbxMsj = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressBarDirectRender1 = new MainMenu.ProgressBarDirectRender();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lbxTit = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbxMsj
            // 
            resources.ApplyResources(this.lbxMsj, "lbxMsj");
            this.lbxMsj.BackColor = System.Drawing.Color.Transparent;
            this.lbxMsj.ForeColor = System.Drawing.Color.Black;
            this.lbxMsj.Name = "lbxMsj";
            this.lbxMsj.UseWaitCursor = true;
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.AliceBlue;
            this.panel2.Controls.Add(this.progressBar1);
            this.panel2.Controls.Add(this.progressBarDirectRender1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.lbxMsj);
            this.panel2.Name = "panel2";
            this.panel2.UseWaitCursor = true;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // progressBar1
            // 
            resources.ApplyResources(this.progressBar1, "progressBar1");
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.UseWaitCursor = true;
            // 
            // progressBarDirectRender1
            // 
            resources.ApplyResources(this.progressBarDirectRender1, "progressBarDirectRender1");
            this.progressBarDirectRender1.iValueMax = 0;
            this.progressBarDirectRender1.Name = "progressBarDirectRender1";
            this.progressBarDirectRender1.UseWaitCursor = true;
            this.progressBarDirectRender1.Value = 0;
            // 
            // panel3
            // 
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panel3.BackgroundImage = global::MainMenu.Properties.Resources.fondo2;
            this.panel3.Controls.Add(this.lbxTit);
            this.panel3.Name = "panel3";
            this.panel3.UseWaitCursor = true;
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
            // 
            // lbxTit
            // 
            resources.ApplyResources(this.lbxTit, "lbxTit");
            this.lbxTit.BackColor = System.Drawing.Color.Transparent;
            this.lbxTit.ForeColor = System.Drawing.Color.Black;
            this.lbxTit.Name = "lbxTit";
            this.lbxTit.UseWaitCursor = true;
            // 
            // ProgressContinue
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ProgressContinue";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.TransparencyKey = System.Drawing.Color.Lime;
            this.UseWaitCursor = true;
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbxTit;
        public System.Windows.Forms.Label lbxMsj;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private ProgressBarDirectRender progressBarDirectRender1;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}