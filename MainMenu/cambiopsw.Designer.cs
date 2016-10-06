namespace MainMenu
{
    partial class cambiopsw
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(cambiopsw));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbxUsuario = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbxContConfirm = new System.Windows.Forms.TextBox();
            this.tbxContNueva = new System.Windows.Forms.TextBox();
            this.tbxContActual = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
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
            this.groupBox1.Controls.Add(this.tbxUsuario);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbxContConfirm);
            this.groupBox1.Controls.Add(this.tbxContNueva);
            this.groupBox1.Controls.Add(this.tbxContActual);
            this.groupBox1.Controls.Add(this.Label1);
            this.groupBox1.Controls.Add(this.Label2);
            this.groupBox1.Controls.Add(this.Label3);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // tbxUsuario
            // 
            resources.ApplyResources(this.tbxUsuario, "tbxUsuario");
            this.tbxUsuario.Name = "tbxUsuario";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label4.Name = "label4";
            // 
            // tbxContConfirm
            // 
            resources.ApplyResources(this.tbxContConfirm, "tbxContConfirm");
            this.tbxContConfirm.Name = "tbxContConfirm";
            this.tbxContConfirm.Enter += new System.EventHandler(this.tbxContConfirm_Enter);
            this.tbxContConfirm.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxContConfirm_KeyDown);
            this.tbxContConfirm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxContConfirm_KeyPress);
            this.tbxContConfirm.Leave += new System.EventHandler(this.tbxContConfirm_Leave);
            // 
            // tbxContNueva
            // 
            resources.ApplyResources(this.tbxContNueva, "tbxContNueva");
            this.tbxContNueva.Name = "tbxContNueva";
            this.tbxContNueva.Enter += new System.EventHandler(this.tbxContNueva_Enter);
            this.tbxContNueva.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxContNueva_KeyDown);
            this.tbxContNueva.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxContNueva_KeyPress);
            this.tbxContNueva.Leave += new System.EventHandler(this.tbxContNueva_Leave);
            // 
            // tbxContActual
            // 
            resources.ApplyResources(this.tbxContActual, "tbxContActual");
            this.tbxContActual.Name = "tbxContActual";
            this.tbxContActual.Enter += new System.EventHandler(this.tbxContActual_Enter);
            this.tbxContActual.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxContActual_KeyDown);
            this.tbxContActual.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxContActual_KeyPress);
            this.tbxContActual.Leave += new System.EventHandler(this.tbxContActual_Leave);
            // 
            // Label1
            // 
            this.Label1.BackColor = System.Drawing.Color.Transparent;
            this.Label1.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.Label1, "Label1");
            this.Label1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label1.Name = "Label1";
            // 
            // Label2
            // 
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.Label2, "Label2");
            this.Label2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label2.Name = "Label2";
            // 
            // Label3
            // 
            this.Label3.BackColor = System.Drawing.Color.Transparent;
            this.Label3.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.Label3, "Label3");
            this.Label3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label3.Name = "Label3";
            // 
            // cancelar
            // 
            this.cancelar.BackColor = System.Drawing.Color.Teal;
            this.cancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancelar, "cancelar");
            this.cancelar.ForeColor = System.Drawing.Color.White;
            this.cancelar.Name = "cancelar";
            this.cancelar.UseVisualStyleBackColor = false;
            // 
            // aceptar
            // 
            this.aceptar.BackColor = System.Drawing.Color.Teal;
            resources.ApplyResources(this.aceptar, "aceptar");
            this.aceptar.ForeColor = System.Drawing.Color.White;
            this.aceptar.Name = "aceptar";
            this.aceptar.UseVisualStyleBackColor = false;
            this.aceptar.Click += new System.EventHandler(this.aceptar_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.AliceBlue;
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.cancelar);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.aceptar);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panel3.BackgroundImage = global::MainMenu.Properties.Resources.fondo2;
            this.panel3.Controls.Add(this.lbxTit);
            resources.ApplyResources(this.panel3, "panel3");
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
            // cambiopsw
            // 
            this.AcceptButton = this.aceptar;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.CancelButton = this.cancelar;
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "cambiopsw";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button cancelar;
        private System.Windows.Forms.Button aceptar;
        private System.Windows.Forms.TextBox tbxContConfirm;
        private System.Windows.Forms.TextBox tbxContNueva;
        private System.Windows.Forms.TextBox tbxContActual;
        public System.Windows.Forms.Label Label1;
        public System.Windows.Forms.Label Label2;
        public System.Windows.Forms.Label Label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lbxTit;
        private System.Windows.Forms.TextBox tbxUsuario;
        public System.Windows.Forms.Label label4;

    }
}