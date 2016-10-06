namespace MainMenu
{
    partial class UserDepurar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserDepurar));
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.gbxFiltro = new System.Windows.Forms.GroupBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.tbxProdfin = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.tbxProdini = new System.Windows.Forms.TextBox();
            this.aceptar = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lbxTit = new System.Windows.Forms.Label();
            this.cancelar = new System.Windows.Forms.Button();
            this.gbxFiltro.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveFileDialog1
            // 
            resources.ApplyResources(this.saveFileDialog1, "saveFileDialog1");
            // 
            // folderBrowserDialog1
            // 
            resources.ApplyResources(this.folderBrowserDialog1, "folderBrowserDialog1");
            // 
            // dlgOpenFile
            // 
            this.dlgOpenFile.FileName = "*.*";
            resources.ApplyResources(this.dlgOpenFile, "dlgOpenFile");
            // 
            // gbxFiltro
            // 
            resources.ApplyResources(this.gbxFiltro, "gbxFiltro");
            this.gbxFiltro.Controls.Add(this.Label1);
            this.gbxFiltro.Controls.Add(this.Label5);
            this.gbxFiltro.Controls.Add(this.tbxProdfin);
            this.gbxFiltro.Controls.Add(this.Label2);
            this.gbxFiltro.Controls.Add(this.tbxProdini);
            this.gbxFiltro.Name = "gbxFiltro";
            this.gbxFiltro.TabStop = false;
            // 
            // Label1
            // 
            resources.ApplyResources(this.Label1, "Label1");
            this.Label1.BackColor = System.Drawing.Color.Transparent;
            this.Label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label1.Name = "Label1";
            // 
            // Label5
            // 
            resources.ApplyResources(this.Label5, "Label5");
            this.Label5.BackColor = System.Drawing.Color.Transparent;
            this.Label5.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label5.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label5.Name = "Label5";
            // 
            // tbxProdfin
            // 
            resources.ApplyResources(this.tbxProdfin, "tbxProdfin");
            this.tbxProdfin.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tbxProdfin.Name = "tbxProdfin";
            this.tbxProdfin.Enter += new System.EventHandler(this.tbxProdfin_Enter);
            this.tbxProdfin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxProdfin_KeyDown);
            this.tbxProdfin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxProdfin_KeyPress);
            this.tbxProdfin.Leave += new System.EventHandler(this.tbxProdfin_Leave);
            // 
            // Label2
            // 
            resources.ApplyResources(this.Label2, "Label2");
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label2.Name = "Label2";
            // 
            // tbxProdini
            // 
            resources.ApplyResources(this.tbxProdini, "tbxProdini");
            this.tbxProdini.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tbxProdini.Name = "tbxProdini";
            this.tbxProdini.Enter += new System.EventHandler(this.tbxProdini_Enter);
            this.tbxProdini.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxProdini_KeyDown);
            this.tbxProdini.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxProdini_KeyPress);
            this.tbxProdini.Leave += new System.EventHandler(this.tbxProdini_Leave);
            // 
            // aceptar
            // 
            resources.ApplyResources(this.aceptar, "aceptar");
            this.aceptar.BackColor = System.Drawing.Color.Teal;
            this.aceptar.ForeColor = System.Drawing.Color.White;
            this.aceptar.Name = "aceptar";
            this.aceptar.UseVisualStyleBackColor = false;
            this.aceptar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.AliceBlue;
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.gbxFiltro);
            this.panel2.Controls.Add(this.aceptar);
            this.panel2.Controls.Add(this.cancelar);
            this.panel2.Name = "panel2";
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // panel3
            // 
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panel3.BackgroundImage = global::MainMenu.Properties.Resources.fondo2;
            this.panel3.Controls.Add(this.lbxTit);
            this.panel3.Name = "panel3";
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
            // 
            // lbxTit
            // 
            resources.ApplyResources(this.lbxTit, "lbxTit");
            this.lbxTit.BackColor = System.Drawing.Color.Transparent;
            this.lbxTit.ForeColor = System.Drawing.Color.Black;
            this.lbxTit.Name = "lbxTit";
            // 
            // cancelar
            // 
            resources.ApplyResources(this.cancelar, "cancelar");
            this.cancelar.BackColor = System.Drawing.Color.Teal;
            this.cancelar.ForeColor = System.Drawing.Color.White;
            this.cancelar.Name = "cancelar";
            this.cancelar.UseVisualStyleBackColor = false;
            this.cancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // UserDepurar
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "UserDepurar";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Activated += new System.EventHandler(this.UserDepurar_Activated);
            this.Load += new System.EventHandler(this.UserDepurar_Load);
            this.gbxFiltro.ResumeLayout(false);
            this.gbxFiltro.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxFiltro;
        public System.Windows.Forms.Label Label1;
        public System.Windows.Forms.Label Label5;
        private System.Windows.Forms.TextBox tbxProdfin;
        public System.Windows.Forms.Label Label2;
        private System.Windows.Forms.TextBox tbxProdini;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog dlgOpenFile;
        private System.Windows.Forms.Button aceptar;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lbxTit;
        private System.Windows.Forms.Button cancelar;



    }
}