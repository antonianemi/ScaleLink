namespace MainMenu
{
    partial class f10Mantenimiento
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(f10Mantenimiento));
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.baseDeDatosDataSet1 = new MainMenu.BaseDeDatosDataSet();
            this.usuariosTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.UsuariosTableAdapter();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip5 = new System.Windows.Forms.ToolStrip();
            this.ribConfig = new System.Windows.Forms.ToolStripButton();
            this.ribUsuario = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ribRespaldar = new System.Windows.Forms.ToolStripButton();
            this.ribCompactar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ribPurgeScale = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.basculaTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.BasculaTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseDeDatosDataSet1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStrip5.SuspendLayout();
            this.SuspendLayout();
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "Bascula";
            this.bindingSource1.DataSource = this.baseDeDatosDataSet1;
            // 
            // baseDeDatosDataSet1
            // 
            this.baseDeDatosDataSet1.DataSetName = "BaseDeDatosDataSet";
            this.baseDeDatosDataSet1.EnforceConstraints = false;
            this.baseDeDatosDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // usuariosTableAdapter
            // 
            this.usuariosTableAdapter.ClearBeforeFill = true;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.toolStrip5, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // toolStrip5
            // 
            this.toolStrip5.BackgroundImage = global::MainMenu.Properties.Resources.fondo2;
            resources.ApplyResources(this.toolStrip5, "toolStrip5");
            this.toolStrip5.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip5.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ribConfig,
            this.ribUsuario,
            this.toolStripSeparator3,
            this.ribRespaldar,
            this.ribCompactar,
            this.toolStripSeparator1,
            this.ribPurgeScale});
            this.toolStrip5.Name = "toolStrip5";
            // 
            // ribConfig
            // 
            this.ribConfig.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ribConfig.Image = global::MainMenu.Properties.Resources.Utilities32;
            resources.ApplyResources(this.ribConfig, "ribConfig");
            this.ribConfig.Name = "ribConfig";
            this.ribConfig.Click += new System.EventHandler(this.ribConfig_Click);
            // 
            // ribUsuario
            // 
            this.ribUsuario.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ribUsuario.Image = global::MainMenu.Properties.Resources.Vendedor;
            resources.ApplyResources(this.ribUsuario, "ribUsuario");
            this.ribUsuario.Name = "ribUsuario";
            this.ribUsuario.Click += new System.EventHandler(this.ribUsuario_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // ribRespaldar
            // 
            this.ribRespaldar.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ribRespaldar.Image = global::MainMenu.Properties.Resources.backup;
            resources.ApplyResources(this.ribRespaldar, "ribRespaldar");
            this.ribRespaldar.Name = "ribRespaldar";
            this.ribRespaldar.Click += new System.EventHandler(this.ribRespaldar_Click);
            // 
            // ribCompactar
            // 
            this.ribCompactar.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ribCompactar.Image = global::MainMenu.Properties.Resources.compactar;
            resources.ApplyResources(this.ribCompactar, "ribCompactar");
            this.ribCompactar.Name = "ribCompactar";
            this.ribCompactar.Click += new System.EventHandler(this.ribCompactar_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // ribPurgeScale
            // 
            this.ribPurgeScale.ForeColor = System.Drawing.Color.MidnightBlue;
            resources.ApplyResources(this.ribPurgeScale, "ribPurgeScale");
            this.ribPurgeScale.Name = "ribPurgeScale";
            this.ribPurgeScale.Click += new System.EventHandler(this.ribPurgeScale_Click);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // basculaTableAdapter
            // 
            this.basculaTableAdapter.ClearBeforeFill = true;
            // 
            // f10Mantenimiento
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "f10Mantenimiento";
            this.Activated += new System.EventHandler(this.f10Mantenimiento_Activated);
            this.Load += new System.EventHandler(this.f10Mantenimiento_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseDeDatosDataSet1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStrip5.ResumeLayout(false);
            this.toolStrip5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bindingSource1;
        private BaseDeDatosDataSet baseDeDatosDataSet1;
        private BaseDeDatosDataSetTableAdapters.UsuariosTableAdapter usuariosTableAdapter;
        public System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStrip toolStrip5;
        private System.Windows.Forms.ToolStripButton ribConfig;
        private System.Windows.Forms.ToolStripButton ribUsuario;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton ribRespaldar;
        private System.Windows.Forms.ToolStripButton ribCompactar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton ribPurgeScale;
        private BaseDeDatosDataSetTableAdapters.BasculaTableAdapter basculaTableAdapter;



    }
}