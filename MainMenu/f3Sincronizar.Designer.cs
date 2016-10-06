namespace MainMenu
{
    partial class f3Sincronizar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(f3Sincronizar));
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tvwBascula = new TreeViewBound.TreeViewBound();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.pnldetalle = new System.Windows.Forms.Panel();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.baseDeDatosDataSet = new MainMenu.BaseDeDatosDataSet();
            this.basculaTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.BasculaTableAdapter();
            this.grupoTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.GrupoTableAdapter();
            this.productosTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.ProductosTableAdapter();
            this.prod_detalleTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.Prod_detalleTableAdapter();
            this.carpeta_detalleTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.carpeta_detalleTableAdapter();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.ribOrganizar = new System.Windows.Forms.ToolStripButton();
            this.ribAsignar = new System.Windows.Forms.ToolStripButton();
            this.ribOrdenar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ribDtosGnral = new System.Windows.Forms.ToolStripButton();
            this.ribCodebar = new System.Windows.Forms.ToolStripButton();
            this.ribTextos = new System.Windows.Forms.ToolStripButton();
            this.ribHora = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ribBascula = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseDeDatosDataSet)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer2
            // 
            resources.ApplyResources(this.splitContainer2, "splitContainer2");
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            resources.ApplyResources(this.splitContainer2.Panel1, "splitContainer2.Panel1");
            this.splitContainer2.Panel1.Controls.Add(this.tvwBascula);
            // 
            // splitContainer2.Panel2
            // 
            resources.ApplyResources(this.splitContainer2.Panel2, "splitContainer2.Panel2");
            this.splitContainer2.Panel2.Controls.Add(this.pnldetalle);
            // 
            // tvwBascula
            // 
            this.tvwBascula.DisplayMember = null;
            resources.ApplyResources(this.tvwBascula, "tvwBascula");
            this.tvwBascula.HideSelection = false;
            this.tvwBascula.ImageList = this.imageList2;
            this.tvwBascula.Name = "tvwBascula";
            this.tvwBascula.ParentMember = null;
            this.tvwBascula.RootParentValue = null;
            this.tvwBascula.ValueMember = null;
            this.tvwBascula.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwBascula_AfterSelect);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "grupo2.ico");
            this.imageList2.Images.SetKeyName(1, "Scale.ico");
            this.imageList2.Images.SetKeyName(2, "ico_wlabel.png");
            this.imageList2.Images.SetKeyName(3, "ico_wlsd.png");
            // 
            // pnldetalle
            // 
            resources.ApplyResources(this.pnldetalle, "pnldetalle");
            this.pnldetalle.BackColor = System.Drawing.Color.AliceBlue;
            this.pnldetalle.Name = "pnldetalle";
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "carpeta_detalle";
            this.bindingSource1.DataSource = this.baseDeDatosDataSet;
            // 
            // baseDeDatosDataSet
            // 
            this.baseDeDatosDataSet.DataSetName = "BaseDeDatosDataSet";
            this.baseDeDatosDataSet.EnforceConstraints = false;
            this.baseDeDatosDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // basculaTableAdapter
            // 
            this.basculaTableAdapter.ClearBeforeFill = true;
            // 
            // grupoTableAdapter
            // 
            this.grupoTableAdapter.ClearBeforeFill = true;
            // 
            // productosTableAdapter
            // 
            this.productosTableAdapter.ClearBeforeFill = true;
            // 
            // prod_detalleTableAdapter
            // 
            this.prod_detalleTableAdapter.ClearBeforeFill = true;
            // 
            // carpeta_detalleTableAdapter
            // 
            this.carpeta_detalleTableAdapter.ClearBeforeFill = true;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.toolStrip3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.splitContainer2, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // toolStrip3
            // 
            this.toolStrip3.BackgroundImage = global::MainMenu.Properties.Resources.fondo2;
            resources.ApplyResources(this.toolStrip3, "toolStrip3");
            this.toolStrip3.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ribOrganizar,
            this.ribAsignar,
            this.ribOrdenar,
            this.toolStripSeparator2,
            this.ribDtosGnral,
            this.ribCodebar,
            this.ribTextos,
            this.ribHora,
            this.toolStripSeparator1,
            this.ribBascula,
            this.toolStripSeparator3});
            this.toolStrip3.Name = "toolStrip3";
            // 
            // ribOrganizar
            // 
            this.ribOrganizar.BackColor = System.Drawing.Color.AliceBlue;
            this.ribOrganizar.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ribOrganizar.Image = global::MainMenu.Properties.Resources.archivos;
            resources.ApplyResources(this.ribOrganizar, "ribOrganizar");
            this.ribOrganizar.Name = "ribOrganizar";
            this.ribOrganizar.Click += new System.EventHandler(this.ribOrganizar_Click);
            // 
            // ribAsignar
            // 
            this.ribAsignar.BackColor = System.Drawing.Color.AliceBlue;
            this.ribAsignar.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ribAsignar.Image = global::MainMenu.Properties.Resources.distribuir;
            resources.ApplyResources(this.ribAsignar, "ribAsignar");
            this.ribAsignar.Name = "ribAsignar";
            this.ribAsignar.Click += new System.EventHandler(this.ribAsignar_Click);
            // 
            // ribOrdenar
            // 
            this.ribOrdenar.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ribOrdenar.Image = global::MainMenu.Properties.Resources.Ordenar;
            resources.ApplyResources(this.ribOrdenar, "ribOrdenar");
            this.ribOrdenar.Name = "ribOrdenar";
            this.ribOrdenar.Click += new System.EventHandler(this.ribOrdenar_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // ribDtosGnral
            // 
            this.ribDtosGnral.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ribDtosGnral.Image = global::MainMenu.Properties.Resources.BT_configBas;
            resources.ApplyResources(this.ribDtosGnral, "ribDtosGnral");
            this.ribDtosGnral.Name = "ribDtosGnral";
            this.ribDtosGnral.Click += new System.EventHandler(this.ribDtosGrnal_Click);
            // 
            // ribCodebar
            // 
            this.ribCodebar.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ribCodebar.Image = global::MainMenu.Properties.Resources.BT_etiqueta;
            resources.ApplyResources(this.ribCodebar, "ribCodebar");
            this.ribCodebar.Name = "ribCodebar";
            this.ribCodebar.Click += new System.EventHandler(this.ribCodeBar_Click);
            // 
            // ribTextos
            // 
            this.ribTextos.BackColor = System.Drawing.Color.AliceBlue;
            this.ribTextos.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ribTextos.Image = global::MainMenu.Properties.Resources.BT_Encabezado;
            resources.ApplyResources(this.ribTextos, "ribTextos");
            this.ribTextos.Name = "ribTextos";
            this.ribTextos.Click += new System.EventHandler(this.ribTexto_Click);
            // 
            // ribHora
            // 
            this.ribHora.Image = global::MainMenu.Properties.Resources.Reloj;
            resources.ApplyResources(this.ribHora, "ribHora");
            this.ribHora.Name = "ribHora";
            this.ribHora.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // ribBascula
            // 
            this.ribBascula.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ribBascula.Image = global::MainMenu.Properties.Resources.BT_sincroniza;
            resources.ApplyResources(this.ribBascula, "ribBascula");
            this.ribBascula.Name = "ribBascula";
            this.ribBascula.Click += new System.EventHandler(this.ribBascula_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // f3Sincronizar
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "f3Sincronizar";
            this.Activated += new System.EventHandler(this.f3Sincronizar_Activated);
            this.Load += new System.EventHandler(this.f3Sincronizar_Load);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseDeDatosDataSet)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bindingSource1;
        private BaseDeDatosDataSet baseDeDatosDataSet;
        private BaseDeDatosDataSetTableAdapters.BasculaTableAdapter basculaTableAdapter;
        private BaseDeDatosDataSetTableAdapters.GrupoTableAdapter grupoTableAdapter;
        private BaseDeDatosDataSetTableAdapters.ProductosTableAdapter productosTableAdapter;
        private BaseDeDatosDataSetTableAdapters.Prod_detalleTableAdapter prod_detalleTableAdapter;
        // private BaseDeDatosDataSetTableAdapters.carpetaTableAdapter carpetaTableAdapter;
        private BaseDeDatosDataSetTableAdapters.carpeta_detalleTableAdapter carpeta_detalleTableAdapter;
        private TreeViewBound.TreeViewBound tvwBascula;
        public System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton ribAsignar;
        private System.Windows.Forms.ToolStripButton ribOrganizar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton ribDtosGnral;
        private System.Windows.Forms.ToolStripButton ribCodebar;
        private System.Windows.Forms.ToolStripButton ribTextos;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton ribBascula;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.Panel pnldetalle;
        private System.Windows.Forms.ToolStripButton ribOrdenar;
        private System.Windows.Forms.ToolStripButton ribHora;
    }
}