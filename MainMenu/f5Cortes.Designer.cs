namespace MainMenu
{
    partial class f5Cortes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(f5Cortes));
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tvwBascula = new TreeViewBound.TreeViewBound();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.lvwVentas = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.baseDeDatosDataSet = new MainMenu.BaseDeDatosDataSet();
            this.basculaTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.BasculaTableAdapter();
            this.grupoTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.GrupoTableAdapter();
            this.productosTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.ProductosTableAdapter();
            this.prod_detalleTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.Prod_detalleTableAdapter();
            this.carpeta_detalleTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.carpeta_detalleTableAdapter();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.ventasTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.VentasTableAdapter();
            this.ventas_DetalleTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.Ventas_DetalleTableAdapter();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip6 = new System.Windows.Forms.ToolStrip();
            this.ribConsulta = new System.Windows.Forms.ToolStripButton();
            this.ribCorte = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ribCerrar = new System.Windows.Forms.ToolStripButton();
            this.vendedorTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.VendedorTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseDeDatosDataSet)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStrip6.SuspendLayout();
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
            this.splitContainer2.Panel2.Controls.Add(this.lvwVentas);
            // 
            // tvwBascula
            // 
            resources.ApplyResources(this.tvwBascula, "tvwBascula");
            this.tvwBascula.DisplayMember = null;
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
            // 
            // lvwVentas
            // 
            resources.ApplyResources(this.lvwVentas, "lvwVentas");
            this.lvwVentas.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lvwVentas.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader8,
            this.columnHeader9});
            this.lvwVentas.ForeColor = System.Drawing.Color.MidnightBlue;
            this.lvwVentas.Name = "lvwVentas";
            this.lvwVentas.UseCompatibleStateImageBehavior = false;
            this.lvwVentas.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // columnHeader5
            // 
            resources.ApplyResources(this.columnHeader5, "columnHeader5");
            // 
            // columnHeader6
            // 
            resources.ApplyResources(this.columnHeader6, "columnHeader6");
            // 
            // columnHeader8
            // 
            resources.ApplyResources(this.columnHeader8, "columnHeader8");
            // 
            // columnHeader9
            // 
            resources.ApplyResources(this.columnHeader9, "columnHeader9");
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "Vendedor";
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
            // ventasTableAdapter
            // 
            this.ventasTableAdapter.ClearBeforeFill = true;
            // 
            // ventas_DetalleTableAdapter
            // 
            this.ventas_DetalleTableAdapter.ClearBeforeFill = true;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.toolStrip6, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.splitContainer2, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // toolStrip6
            // 
            resources.ApplyResources(this.toolStrip6, "toolStrip6");
            this.toolStrip6.BackgroundImage = global::MainMenu.Properties.Resources.fondo2;
            this.toolStrip6.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip6.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ribConsulta,
            this.ribCorte,
            this.toolStripSeparator2,
            this.ribCerrar});
            this.toolStrip6.Name = "toolStrip6";
            // 
            // ribConsulta
            // 
            resources.ApplyResources(this.ribConsulta, "ribConsulta");
            this.ribConsulta.BackColor = System.Drawing.Color.AliceBlue;
            this.ribConsulta.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ribConsulta.Image = global::MainMenu.Properties.Resources.consultavta;
            this.ribConsulta.Name = "ribConsulta";
            this.ribConsulta.Click += new System.EventHandler(this.ribConsulta_Click);
            // 
            // ribCorte
            // 
            resources.ApplyResources(this.ribCorte, "ribCorte");
            this.ribCorte.BackColor = System.Drawing.Color.AliceBlue;
            this.ribCorte.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ribCorte.Image = global::MainMenu.Properties.Resources.cortecaja;
            this.ribCorte.Name = "ribCorte";
            this.ribCorte.Click += new System.EventHandler(this.ribCorte_Click);
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // ribCerrar
            // 
            resources.ApplyResources(this.ribCerrar, "ribCerrar");
            this.ribCerrar.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ribCerrar.Image = global::MainMenu.Properties.Resources.cancelar;
            this.ribCerrar.Name = "ribCerrar";
            this.ribCerrar.Click += new System.EventHandler(this.ribCerrar_Click);
            // 
            // vendedorTableAdapter
            // 
            this.vendedorTableAdapter.ClearBeforeFill = true;
            // 
            // f5Cortes
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "f5Cortes";
            this.Activated += new System.EventHandler(this.f5Cortes_Activated);
            this.Load += new System.EventHandler(this.f5Cortes_Load);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseDeDatosDataSet)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStrip6.ResumeLayout(false);
            this.toolStrip6.PerformLayout();
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
        private System.Windows.Forms.ToolStrip toolStrip6;
        private System.Windows.Forms.ToolStripButton ribConsulta;
        private System.Windows.Forms.ToolStripButton ribCorte;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton ribCerrar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListView lvwVentas;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.IO.Ports.SerialPort serialPort1;
        private BaseDeDatosDataSetTableAdapters.VentasTableAdapter ventasTableAdapter;
        private BaseDeDatosDataSetTableAdapters.Ventas_DetalleTableAdapter ventas_DetalleTableAdapter;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private BaseDeDatosDataSetTableAdapters.VendedorTableAdapter vendedorTableAdapter;
    }
}