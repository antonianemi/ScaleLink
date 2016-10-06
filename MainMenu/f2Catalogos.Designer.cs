namespace MainMenu
{
    partial class f2Catalogos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(f2Catalogos));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.ribProductos = new System.Windows.Forms.ToolStripButton();
            this.ribIngredientes = new System.Windows.Forms.ToolStripButton();
            this.ribOfertas = new System.Windows.Forms.ToolStripButton();
            this.ribPublicidad = new System.Windows.Forms.ToolStripButton();
            this.ribVendedores = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip2, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // toolStrip2
            // 
            resources.ApplyResources(this.toolStrip2, "toolStrip2");
            this.toolStrip2.BackgroundImage = global::MainMenu.Properties.Resources.fondo2;
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ribProductos,
            this.ribIngredientes,
            this.ribOfertas,
            this.ribPublicidad,
            this.ribVendedores,
            this.toolStripSeparator1});
            this.toolStrip2.Name = "toolStrip2";
            // 
            // ribProductos
            // 
            resources.ApplyResources(this.ribProductos, "ribProductos");
            this.ribProductos.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ribProductos.Image = global::MainMenu.Properties.Resources.producto2;
            this.ribProductos.Name = "ribProductos";
            this.ribProductos.Click += new System.EventHandler(this.ribProductos_Click);
            // 
            // ribIngredientes
            // 
            resources.ApplyResources(this.ribIngredientes, "ribIngredientes");
            this.ribIngredientes.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ribIngredientes.Image = global::MainMenu.Properties.Resources.BT_Texto;
            this.ribIngredientes.Name = "ribIngredientes";
            this.ribIngredientes.Click += new System.EventHandler(this.ribIngredientes_Click);
            // 
            // ribOfertas
            // 
            resources.ApplyResources(this.ribOfertas, "ribOfertas");
            this.ribOfertas.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ribOfertas.Image = global::MainMenu.Properties.Resources.precio;
            this.ribOfertas.Name = "ribOfertas";
            this.ribOfertas.Click += new System.EventHandler(this.ribOfertas_Click);
            // 
            // ribPublicidad
            // 
            resources.ApplyResources(this.ribPublicidad, "ribPublicidad");
            this.ribPublicidad.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ribPublicidad.Image = global::MainMenu.Properties.Resources.Publicidad2;
            this.ribPublicidad.Name = "ribPublicidad";
            this.ribPublicidad.Click += new System.EventHandler(this.ribPublicidad_Click);
            // 
            // ribVendedores
            // 
            resources.ApplyResources(this.ribVendedores, "ribVendedores");
            this.ribVendedores.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ribVendedores.Image = global::MainMenu.Properties.Resources.BO_3vendedor;
            this.ribVendedores.Name = "ribVendedores";
            this.ribVendedores.Click += new System.EventHandler(this.ribVendedor_Click);
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // f2Catalogos
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "f2Catalogos";
            this.Activated += new System.EventHandler(this.f2Catalogos_Activated);
            this.Load += new System.EventHandler(this.f2Catalogos_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton ribProductos;
        private System.Windows.Forms.ToolStripButton ribIngredientes;
        private System.Windows.Forms.ToolStripButton ribOfertas;
        private System.Windows.Forms.ToolStripButton ribPublicidad;
        private System.Windows.Forms.ToolStripButton ribVendedores;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.ToolStrip toolStrip2;












    }
}