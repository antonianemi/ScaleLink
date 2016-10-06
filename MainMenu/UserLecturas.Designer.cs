namespace MainMenu
{
    partial class UserLecturas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserLecturas));
            this.eventLog1 = new System.Diagnostics.EventLog();
            this.listViewDatos = new System.Windows.Forms.ListView();
            this.toolStrip31 = new System.Windows.Forms.ToolStrip();
            this.StripProductos = new System.Windows.Forms.ToolStripButton();
            this.StripIngrediente = new System.Windows.Forms.ToolStripButton();
            this.StripPublicidad = new System.Windows.Forms.ToolStripButton();
            this.StripOferta = new System.Windows.Forms.ToolStripButton();
            this.StripVendedores = new System.Windows.Forms.ToolStripButton();
            this.StripCarpetas = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.StripCerrar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.baseDeDatosDataSet = new MainMenu.BaseDeDatosDataSet();
            this.basculaTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.BasculaTableAdapter();
            this.grupoTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.GrupoTableAdapter();
            this.productosTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.ProductosTableAdapter();
            this.publicidadTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.PublicidadTableAdapter();
            this.prod_detalleTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.Prod_detalleTableAdapter();
            this.ofertaTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.OfertaTableAdapter();
            this.oferta_DetalleTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.Oferta_DetalleTableAdapter();
            this.public_DetalleTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.Public_DetalleTableAdapter();
            this.vendedorTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.VendedorTableAdapter();
            this.ingredientesTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.IngredientesTableAdapter();
            this.ingre_detalleTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.Ingre_detalleTableAdapter();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).BeginInit();
            this.toolStrip31.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseDeDatosDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // eventLog1
            // 
            this.eventLog1.SynchronizingObject = this;
            // 
            // listViewDatos
            // 
            this.listViewDatos.AllowColumnReorder = true;
            this.listViewDatos.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this.listViewDatos, "listViewDatos");
            this.listViewDatos.FullRowSelect = true;
            this.listViewDatos.Name = "listViewDatos";
            this.listViewDatos.UseCompatibleStateImageBehavior = false;
            this.listViewDatos.View = System.Windows.Forms.View.Details;
            this.listViewDatos.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewDatos_ColumnClick);
            // 
            // toolStrip31
            // 
            this.toolStrip31.BackColor = System.Drawing.Color.LightSteelBlue;
            resources.ApplyResources(this.toolStrip31, "toolStrip31");
            this.toolStrip31.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip31.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip31.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StripProductos,
            this.StripIngrediente,
            this.StripPublicidad,
            this.StripOferta,
            this.StripVendedores,
            this.StripCarpetas,
            this.toolStripSeparator1,
            this.StripCerrar,
            this.toolStripSeparator3,
            this.toolStripLabel3});
            this.toolStrip31.Name = "toolStrip31";
            // 
            // StripProductos
            // 
            resources.ApplyResources(this.StripProductos, "StripProductos");
            this.StripProductos.ForeColor = System.Drawing.Color.MidnightBlue;
            this.StripProductos.Image = global::MainMenu.Properties.Resources.producto2;
            this.StripProductos.Name = "StripProductos";
            this.StripProductos.Click += new System.EventHandler(this.StripProductos_Click);
            // 
            // StripIngrediente
            // 
            this.StripIngrediente.ForeColor = System.Drawing.Color.MidnightBlue;
            this.StripIngrediente.Image = global::MainMenu.Properties.Resources.BT_Texto;
            resources.ApplyResources(this.StripIngrediente, "StripIngrediente");
            this.StripIngrediente.Name = "StripIngrediente";
            this.StripIngrediente.Click += new System.EventHandler(this.StripIngrediente_Click);
            // 
            // StripPublicidad
            // 
            resources.ApplyResources(this.StripPublicidad, "StripPublicidad");
            this.StripPublicidad.ForeColor = System.Drawing.Color.MidnightBlue;
            this.StripPublicidad.Image = global::MainMenu.Properties.Resources.Publicidad2;
            this.StripPublicidad.Name = "StripPublicidad";
            this.StripPublicidad.Click += new System.EventHandler(this.StripPublicidad_Click);
            // 
            // StripOferta
            // 
            resources.ApplyResources(this.StripOferta, "StripOferta");
            this.StripOferta.ForeColor = System.Drawing.Color.MidnightBlue;
            this.StripOferta.Image = global::MainMenu.Properties.Resources.precio;
            this.StripOferta.Name = "StripOferta";
            this.StripOferta.Click += new System.EventHandler(this.StripOferta_Click);
            // 
            // StripVendedores
            // 
            resources.ApplyResources(this.StripVendedores, "StripVendedores");
            this.StripVendedores.ForeColor = System.Drawing.Color.MidnightBlue;
            this.StripVendedores.Image = global::MainMenu.Properties.Resources.Usuario;
            this.StripVendedores.Name = "StripVendedores";
            this.StripVendedores.Click += new System.EventHandler(this.StripVendedores_Click);
            // 
            // StripCarpetas
            // 
            this.StripCarpetas.ForeColor = System.Drawing.Color.MidnightBlue;
            this.StripCarpetas.Image = global::MainMenu.Properties.Resources.grupo;
            resources.ApplyResources(this.StripCarpetas, "StripCarpetas");
            this.StripCarpetas.Name = "StripCarpetas";
            this.StripCarpetas.Click += new System.EventHandler(this.StripCarpetas_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // StripCerrar
            // 
            this.StripCerrar.ForeColor = System.Drawing.Color.MidnightBlue;
            this.StripCerrar.Image = global::MainMenu.Properties.Resources.cancelar;
            resources.ApplyResources(this.StripCerrar, "StripCerrar");
            this.StripCerrar.Name = "StripCerrar";
            this.StripCerrar.Click += new System.EventHandler(this.StripCerrar_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // toolStripLabel3
            // 
            resources.ApplyResources(this.toolStripLabel3, "toolStripLabel3");
            this.toolStripLabel3.ForeColor = System.Drawing.Color.Blue;
            this.toolStripLabel3.Name = "toolStripLabel3";
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "Ingre_detalle";
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
            // publicidadTableAdapter
            // 
            this.publicidadTableAdapter.ClearBeforeFill = true;
            // 
            // prod_detalleTableAdapter
            // 
            this.prod_detalleTableAdapter.ClearBeforeFill = true;
            // 
            // ofertaTableAdapter
            // 
            this.ofertaTableAdapter.ClearBeforeFill = true;
            // 
            // oferta_DetalleTableAdapter
            // 
            this.oferta_DetalleTableAdapter.ClearBeforeFill = true;
            // 
            // public_DetalleTableAdapter
            // 
            this.public_DetalleTableAdapter.ClearBeforeFill = true;
            // 
            // vendedorTableAdapter
            // 
            this.vendedorTableAdapter.ClearBeforeFill = true;
            // 
            // ingredientesTableAdapter
            // 
            this.ingredientesTableAdapter.ClearBeforeFill = true;
            // 
            // ingre_detalleTableAdapter
            // 
            this.ingre_detalleTableAdapter.ClearBeforeFill = true;
            // 
            // UserLecturas
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.listViewDatos);
            this.Controls.Add(this.toolStrip31);
            this.Name = "UserLecturas";
            this.Load += new System.EventHandler(this.UserLecturas_Load);
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).EndInit();
            this.toolStrip31.ResumeLayout(false);
            this.toolStrip31.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseDeDatosDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource bindingSource1;
        private BaseDeDatosDataSet baseDeDatosDataSet;
        private BaseDeDatosDataSetTableAdapters.BasculaTableAdapter basculaTableAdapter;
        private BaseDeDatosDataSetTableAdapters.GrupoTableAdapter grupoTableAdapter;
        private BaseDeDatosDataSetTableAdapters.ProductosTableAdapter productosTableAdapter;
        private BaseDeDatosDataSetTableAdapters.PublicidadTableAdapter publicidadTableAdapter;
        private BaseDeDatosDataSetTableAdapters.Prod_detalleTableAdapter prod_detalleTableAdapter;
        private BaseDeDatosDataSetTableAdapters.OfertaTableAdapter ofertaTableAdapter;
        private System.Diagnostics.EventLog eventLog1;
        private System.Windows.Forms.ListView listViewDatos;
        private BaseDeDatosDataSetTableAdapters.Oferta_DetalleTableAdapter oferta_DetalleTableAdapter;
        private BaseDeDatosDataSetTableAdapters.Public_DetalleTableAdapter public_DetalleTableAdapter;
        private BaseDeDatosDataSetTableAdapters.VendedorTableAdapter vendedorTableAdapter;
        private System.Windows.Forms.ToolStripButton StripProductos;
        private System.Windows.Forms.ToolStripButton StripIngrediente;
        private System.Windows.Forms.ToolStripButton StripPublicidad;
        private System.Windows.Forms.ToolStripButton StripOferta;
        private System.Windows.Forms.ToolStripButton StripVendedores;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private BaseDeDatosDataSetTableAdapters.IngredientesTableAdapter ingredientesTableAdapter;
        private BaseDeDatosDataSetTableAdapters.Ingre_detalleTableAdapter ingre_detalleTableAdapter;
        public System.Windows.Forms.ToolStrip toolStrip31;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton StripCerrar;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.ToolStripButton StripCarpetas;


    }
}