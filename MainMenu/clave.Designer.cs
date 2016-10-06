namespace MainMenu
{
    partial class clave
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(clave));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.inicio_password = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.inicio_user = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cancelar = new System.Windows.Forms.Button();
            this.aceptar = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lbxTit = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.inicio_password);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.inicio_user);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // inicio_password
            // 
            resources.ApplyResources(this.inicio_password, "inicio_password");
            this.inicio_password.Name = "inicio_password";
            this.inicio_password.Enter += new System.EventHandler(this.inicio_password_Enter);
            this.inicio_password.KeyDown += new System.Windows.Forms.KeyEventHandler(this.inicio_password_KeyDown);
            this.inicio_password.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.inicio_password_KeyPress);
            this.inicio_password.Leave += new System.EventHandler(this.inicio_password_Leave);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // inicio_user
            // 
            resources.ApplyResources(this.inicio_user, "inicio_user");
            this.inicio_user.Name = "inicio_user";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cancelar
            // 
            resources.ApplyResources(this.cancelar, "cancelar");
            this.cancelar.BackColor = System.Drawing.Color.Teal;
            this.cancelar.ForeColor = System.Drawing.Color.White;
            this.cancelar.Name = "cancelar";
            this.cancelar.UseVisualStyleBackColor = false;
            this.cancelar.Click += new System.EventHandler(this.cancelar_Click);
            // 
            // aceptar
            // 
            resources.ApplyResources(this.aceptar, "aceptar");
            this.aceptar.BackColor = System.Drawing.Color.Teal;
            this.aceptar.ForeColor = System.Drawing.Color.White;
            this.aceptar.Name = "aceptar";
            this.aceptar.UseVisualStyleBackColor = false;
            this.aceptar.Click += new System.EventHandler(this.aceptar_Click);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.AliceBlue;
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.cancelar);
            this.panel2.Controls.Add(this.aceptar);
            this.panel2.Controls.Add(this.groupBox1);
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
            // clave
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "clave";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Activated += new System.EventHandler(this.clave_Activated);
            this.Load += new System.EventHandler(this.clave_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox inicio_user;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cancelar;
        private System.Windows.Forms.Button aceptar;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lbxTit;
        public System.Windows.Forms.TextBox inicio_password;

    }
}