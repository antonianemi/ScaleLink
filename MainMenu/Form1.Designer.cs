namespace MainMenu
{
    partial class Form1
    {
       
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lblMsgStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.Inter_Timer = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.basculasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.catalogosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.IpadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.estadisticaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mantenimientoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.CambioUsuarioToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.CambiopasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ayudaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.wizardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ritInicio = new System.Windows.Forms.ToolStripMenuItem();
            this.ritCatalogo = new System.Windows.Forms.ToolStripMenuItem();
            this.ritCarpetas = new System.Windows.Forms.ToolStripMenuItem();
            this.ritPrecio = new System.Windows.Forms.ToolStripMenuItem();
            this.ritCorte = new System.Windows.Forms.ToolStripMenuItem();
            this.ritEstadistica = new System.Windows.Forms.ToolStripMenuItem();
            this.ritMentenimiento = new System.Windows.Forms.ToolStripMenuItem();
            this.ritAyuda = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMsgStatus
            // 
            this.lblMsgStatus.AutoToolTip = true;
            this.lblMsgStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblMsgStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Adjust;
            this.lblMsgStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblMsgStatus.Name = "lblMsgStatus";
            resources.ApplyResources(this.lblMsgStatus, "lblMsgStatus");
            // 
            // Inter_Timer
            // 
            this.Inter_Timer.Interval = 300000;
            this.Inter_Timer.Tick += new System.EventHandler(this.Inter_Timer_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.LightGray;
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.ShowItemToolTips = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.CornflowerBlue;
            this.menuStrip1.BackgroundImage = global::MainMenu.Properties.Resources.fondo2;
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.ritInicio,
            this.ritCatalogo,
            this.ritCarpetas,
            this.ritPrecio,
            this.ritCorte,
            this.ritEstadistica,
            this.ritMentenimiento,
            this.ritAyuda,
            this.salirToolStripMenuItem1});
            this.menuStrip1.Name = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.basculasToolStripMenuItem,
            this.catalogosToolStripMenuItem,
            this.IpadToolStripMenuItem,
            this.toolStripSeparator1,
            this.estadisticaToolStripMenuItem,
            this.mantenimientoToolStripMenuItem,
            this.toolStripSeparator3,
            this.CambioUsuarioToolStrip,
            this.CambiopasswordToolStripMenuItem,
            this.toolStripSeparator2,
            this.ayudaToolStripMenuItem,
            this.toolStripSeparator8,
            this.wizardToolStripMenuItem,
            this.salirToolStripMenuItem});
            this.toolStripMenuItem1.Image = global::MainMenu.Properties.Resources.BO_1_bc;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // basculasToolStripMenuItem
            // 
            this.basculasToolStripMenuItem.ForeColor = System.Drawing.Color.MidnightBlue;
            this.basculasToolStripMenuItem.Name = "basculasToolStripMenuItem";
            resources.ApplyResources(this.basculasToolStripMenuItem, "basculasToolStripMenuItem");
            this.basculasToolStripMenuItem.Click += new System.EventHandler(this.ritInicio_Click);
            // 
            // catalogosToolStripMenuItem
            // 
            this.catalogosToolStripMenuItem.ForeColor = System.Drawing.Color.MidnightBlue;
            this.catalogosToolStripMenuItem.Name = "catalogosToolStripMenuItem";
            resources.ApplyResources(this.catalogosToolStripMenuItem, "catalogosToolStripMenuItem");
            this.catalogosToolStripMenuItem.Click += new System.EventHandler(this.ritCatalogo_Click);
            // 
            // IpadToolStripMenuItem
            // 
            this.IpadToolStripMenuItem.ForeColor = System.Drawing.Color.MidnightBlue;
            this.IpadToolStripMenuItem.Name = "IpadToolStripMenuItem";
            resources.ApplyResources(this.IpadToolStripMenuItem, "IpadToolStripMenuItem");
            this.IpadToolStripMenuItem.Click += new System.EventHandler(this.ritSincronizacion_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // estadisticaToolStripMenuItem
            // 
            this.estadisticaToolStripMenuItem.ForeColor = System.Drawing.Color.MidnightBlue;
            this.estadisticaToolStripMenuItem.Name = "estadisticaToolStripMenuItem";
            resources.ApplyResources(this.estadisticaToolStripMenuItem, "estadisticaToolStripMenuItem");
            this.estadisticaToolStripMenuItem.Click += new System.EventHandler(this.ritEstadistica_Click);
            // 
            // mantenimientoToolStripMenuItem
            // 
            this.mantenimientoToolStripMenuItem.ForeColor = System.Drawing.Color.MidnightBlue;
            this.mantenimientoToolStripMenuItem.Name = "mantenimientoToolStripMenuItem";
            resources.ApplyResources(this.mantenimientoToolStripMenuItem, "mantenimientoToolStripMenuItem");
            this.mantenimientoToolStripMenuItem.Click += new System.EventHandler(this.ritMentenimiento_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // CambioUsuarioToolStrip
            // 
            this.CambioUsuarioToolStrip.ForeColor = System.Drawing.Color.MidnightBlue;
            this.CambioUsuarioToolStrip.Name = "CambioUsuarioToolStrip";
            resources.ApplyResources(this.CambioUsuarioToolStrip, "CambioUsuarioToolStrip");
            this.CambioUsuarioToolStrip.Click += new System.EventHandler(this.CambioUsuarioToolStrip_Click);
            // 
            // CambiopasswordToolStripMenuItem
            // 
            this.CambiopasswordToolStripMenuItem.ForeColor = System.Drawing.Color.MidnightBlue;
            this.CambiopasswordToolStripMenuItem.Name = "CambiopasswordToolStripMenuItem";
            resources.ApplyResources(this.CambiopasswordToolStripMenuItem, "CambiopasswordToolStripMenuItem");
            this.CambiopasswordToolStripMenuItem.Click += new System.EventHandler(this.CambiopasswordToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // ayudaToolStripMenuItem
            // 
            this.ayudaToolStripMenuItem.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ayudaToolStripMenuItem.Name = "ayudaToolStripMenuItem";
            resources.ApplyResources(this.ayudaToolStripMenuItem, "ayudaToolStripMenuItem");
            this.ayudaToolStripMenuItem.Click += new System.EventHandler(this.ritAyuda_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            // 
            // wizardToolStripMenuItem
            // 
            this.wizardToolStripMenuItem.ForeColor = System.Drawing.Color.MidnightBlue;
            this.wizardToolStripMenuItem.Name = "wizardToolStripMenuItem";
            resources.ApplyResources(this.wizardToolStripMenuItem, "wizardToolStripMenuItem");
            this.wizardToolStripMenuItem.Click += new System.EventHandler(this.wizardToolStripMenuItem_Click);
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.ForeColor = System.Drawing.Color.Navy;
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            resources.ApplyResources(this.salirToolStripMenuItem, "salirToolStripMenuItem");
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.rioSalir_Click);
            // 
            // ritInicio
            // 
            resources.ApplyResources(this.ritInicio, "ritInicio");
            this.ritInicio.BackColor = System.Drawing.Color.CornflowerBlue;
            this.ritInicio.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ritInicio.Name = "ritInicio";
            this.ritInicio.Click += new System.EventHandler(this.ritInicio_Click);
            // 
            // ritCatalogo
            // 
            resources.ApplyResources(this.ritCatalogo, "ritCatalogo");
            this.ritCatalogo.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ritCatalogo.Name = "ritCatalogo";
            this.ritCatalogo.Click += new System.EventHandler(this.ritCatalogo_Click);
            // 
            // ritCarpetas
            // 
            resources.ApplyResources(this.ritCarpetas, "ritCarpetas");
            this.ritCarpetas.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ritCarpetas.Name = "ritCarpetas";
            this.ritCarpetas.Click += new System.EventHandler(this.ritCarpetas_Click);
            // 
            // ritPrecio
            // 
            resources.ApplyResources(this.ritPrecio, "ritPrecio");
            this.ritPrecio.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ritPrecio.Name = "ritPrecio";
            this.ritPrecio.Click += new System.EventHandler(this.ritPrecio_Click);
            // 
            // ritCorte
            // 
            resources.ApplyResources(this.ritCorte, "ritCorte");
            this.ritCorte.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ritCorte.Name = "ritCorte";
            this.ritCorte.Click += new System.EventHandler(this.ritCorte_Click);
            // 
            // ritEstadistica
            // 
            resources.ApplyResources(this.ritEstadistica, "ritEstadistica");
            this.ritEstadistica.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ritEstadistica.Name = "ritEstadistica";
            this.ritEstadistica.Click += new System.EventHandler(this.ritEstadistica_Click);
            // 
            // ritMentenimiento
            // 
            resources.ApplyResources(this.ritMentenimiento, "ritMentenimiento");
            this.ritMentenimiento.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ritMentenimiento.Name = "ritMentenimiento";
            this.ritMentenimiento.Click += new System.EventHandler(this.ritMentenimiento_Click);
            // 
            // ritAyuda
            // 
            resources.ApplyResources(this.ritAyuda, "ritAyuda");
            this.ritAyuda.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ritAyuda.Name = "ritAyuda";
            this.ritAyuda.Click += new System.EventHandler(this.ritAyuda_Click);
            // 
            // salirToolStripMenuItem1
            // 
            resources.ApplyResources(this.salirToolStripMenuItem1, "salirToolStripMenuItem1");
            this.salirToolStripMenuItem1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.salirToolStripMenuItem1.Name = "salirToolStripMenuItem1";
            this.salirToolStripMenuItem1.Click += new System.EventHandler(this.rioSalir_Click);
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MdiChildActivate += new System.EventHandler(this.Form1_MdiChildActivate);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

       
        private System.Windows.Forms.ToolStripStatusLabel lblMsgStatus;       
        private System.Windows.Forms.Timer Inter_Timer;
        public System.Windows.Forms.StatusStrip statusStrip1;       
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem basculasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem catalogosToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem estadisticaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mantenimientoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem ayudaToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem wizardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ritCatalogo;
        private System.Windows.Forms.ToolStripMenuItem ritCarpetas;
        private System.Windows.Forms.ToolStripMenuItem ritPrecio;
        private System.Windows.Forms.ToolStripMenuItem ritCorte;
        private System.Windows.Forms.ToolStripMenuItem ritEstadistica;
        private System.Windows.Forms.ToolStripMenuItem ritMentenimiento;
        private System.Windows.Forms.ToolStripMenuItem ritAyuda;
        private System.Windows.Forms.ToolStripMenuItem ritInicio;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem CambioUsuarioToolStrip;
        private System.Windows.Forms.ToolStripMenuItem CambiopasswordToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem IpadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
      
    }
}

