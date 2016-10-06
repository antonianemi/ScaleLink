namespace MainMenu
{
    partial class RangoPrecios
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RangoPrecios));
            this.label3 = new System.Windows.Forms.Label();
            this.tbxDescuento = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbxCodigo2 = new System.Windows.Forms.TextBox();
            this.tbxCodigo1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnCan = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lbxTit = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Name = "label3";
            // 
            // tbxDescuento
            // 
            this.tbxDescuento.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this.tbxDescuento, "tbxDescuento");
            this.tbxDescuento.Name = "tbxDescuento";
            this.tbxDescuento.Enter += new System.EventHandler(this.tbxDescuento_Enter);
            this.tbxDescuento.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxDescuento_KeyDown);
            this.tbxDescuento.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxDescuento_KeyPress);
            this.tbxDescuento.Leave += new System.EventHandler(this.tbxDescuento_Leave);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Name = "label5";
            // 
            // tbxCodigo2
            // 
            this.tbxCodigo2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tbxCodigo2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(this.tbxCodigo2, "tbxCodigo2");
            this.tbxCodigo2.Name = "tbxCodigo2";
            this.tbxCodigo2.Enter += new System.EventHandler(this.tbxCodigo2_Enter);
            this.tbxCodigo2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxCodigo2_KeyDown);
            this.tbxCodigo2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxCodigo2_KeyPress);
            this.tbxCodigo2.Leave += new System.EventHandler(this.tbxCodigo2_Leave);
            // 
            // tbxCodigo1
            // 
            this.tbxCodigo1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tbxCodigo1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(this.tbxCodigo1, "tbxCodigo1");
            this.tbxCodigo1.Name = "tbxCodigo1";
            this.tbxCodigo1.Enter += new System.EventHandler(this.tbxCodigo1_Enter);
            this.tbxCodigo1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxCodigo1_KeyDown);
            this.tbxCodigo1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxCodigo1_KeyPress);
            this.tbxCodigo1.Leave += new System.EventHandler(this.tbxCodigo1_Leave);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Name = "label1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.tbxCodigo2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbxDescuento);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbxCodigo1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // btnCan
            // 
            this.btnCan.BackColor = System.Drawing.Color.Teal;
            resources.ApplyResources(this.btnCan, "btnCan");
            this.btnCan.ForeColor = System.Drawing.Color.White;
            this.btnCan.Name = "btnCan";
            this.btnCan.UseVisualStyleBackColor = false;
            this.btnCan.Click += new System.EventHandler(this.btnCan_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Teal;
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.AliceBlue;
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.btnOk);
            this.panel2.Controls.Add(this.btnCan);
            this.panel2.Controls.Add(this.groupBox1);
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
            // RangoPrecios
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RangoPrecios";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Activated += new System.EventHandler(this.RangoPrecios_Activated);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxDescuento;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbxCodigo2;
        private System.Windows.Forms.TextBox tbxCodigo1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCan;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lbxTit;
    }
}