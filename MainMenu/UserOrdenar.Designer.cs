namespace MainMenu
{
    partial class UserOrdenar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserOrdenar));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvwCarpetas = new TreeViewBound.TreeViewBound();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.listViewProductos = new System.Windows.Forms.ListView();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip31 = new System.Windows.Forms.ToolStrip();
            this.StripGuardar = new System.Windows.Forms.ToolStripButton();
            this.StripEnviar = new System.Windows.Forms.ToolStripButton();
            this.StripCerrar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.baseDeDatosDataSet = new MainMenu.BaseDeDatosDataSet();
            this.basculaTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.BasculaTableAdapter();
            this.grupoTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.GrupoTableAdapter();
            this.productosTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.ProductosTableAdapter();
            this.prod_detalleTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.Prod_detalleTableAdapter();
            this.carpeta_detalleTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.carpeta_detalleTableAdapter();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.toolStrip31.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseDeDatosDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvwCarpetas);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel3);
            // 
            // tvwCarpetas
            // 
            this.tvwCarpetas.DisplayMember = "";
            resources.ApplyResources(this.tvwCarpetas, "tvwCarpetas");
            this.tvwCarpetas.HideSelection = false;
            this.tvwCarpetas.ImageList = this.imageList1;
            this.tvwCarpetas.Name = "tvwCarpetas";
            this.tvwCarpetas.ParentMember = "";
            this.tvwCarpetas.PathSeparator = "/";
            this.tvwCarpetas.RootParentValue = null;
            this.tvwCarpetas.ValueMember = null;
            this.tvwCarpetas.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvwCarpetas_AfterLabelEdit);
            this.tvwCarpetas.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvwCarpetas_ItemDrag);
            this.tvwCarpetas.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwCarpetas_AfterSelect);
            this.tvwCarpetas.Click += new System.EventHandler(this.tvwCarpetas_Click);
            this.tvwCarpetas.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvwCarpetas_DragDrop);
            this.tvwCarpetas.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvwCarpetas_DragEnter);
            this.tvwCarpetas.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvwCarpetas_KeyDown);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folderSelect.png");
            this.imageList1.Images.SetKeyName(1, "folderOpen.png");
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.listViewProductos);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // listViewProductos
            // 
            this.listViewProductos.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this.listViewProductos, "listViewProductos");
            this.listViewProductos.FullRowSelect = true;
            this.listViewProductos.Name = "listViewProductos";
            this.listViewProductos.SmallImageList = this.imageList2;
            this.listViewProductos.UseCompatibleStateImageBehavior = false;
            this.listViewProductos.View = System.Windows.Forms.View.Details;
            this.listViewProductos.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listViewProductos_AfterLabelEdit);
            this.listViewProductos.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listViewProductos_ItemDrag);
            this.listViewProductos.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewProductos_DragDrop);
            this.listViewProductos.DragEnter += new System.Windows.Forms.DragEventHandler(this.listViewProductos_DragEnter);
            this.listViewProductos.DragLeave += new System.EventHandler(this.listViewProductos_DragLeave);
            this.listViewProductos.DoubleClick += new System.EventHandler(this.listViewProductos_DoubleClick);
            this.listViewProductos.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewProductos_KeyDown);
            this.listViewProductos.Leave += new System.EventHandler(this.listViewProductos_Leave);
            this.listViewProductos.MouseLeave += new System.EventHandler(this.listViewProductos_MouseLeave);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "carpetas.png");
            this.imageList2.Images.SetKeyName(1, "producto2.png");
            // 
            // toolStrip31
            // 
            this.toolStrip31.BackColor = System.Drawing.Color.LightSteelBlue;
            resources.ApplyResources(this.toolStrip31, "toolStrip31");
            this.toolStrip31.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip31.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip31.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StripGuardar,
            this.StripEnviar,
            this.StripCerrar,
            this.toolStripSeparator3,
            this.toolStripLabel1,
            this.toolStripLabel2,
            this.toolStripSeparator1,
            this.toolStripLabel3});
            this.toolStrip31.Name = "toolStrip31";
            // 
            // StripGuardar
            // 
            this.StripGuardar.ForeColor = System.Drawing.Color.MidnightBlue;
            this.StripGuardar.Image = global::MainMenu.Properties.Resources.save3;
            resources.ApplyResources(this.StripGuardar, "StripGuardar");
            this.StripGuardar.Name = "StripGuardar";
            this.StripGuardar.Click += new System.EventHandler(this.StripGuardar_Click);
            // 
            // StripEnviar
            // 
            resources.ApplyResources(this.StripEnviar, "StripEnviar");
            this.StripEnviar.ForeColor = System.Drawing.Color.MidnightBlue;
            this.StripEnviar.Image = global::MainMenu.Properties.Resources.connect;
            this.StripEnviar.Name = "StripEnviar";
            this.StripEnviar.Click += new System.EventHandler(this.soloCarpetasToolStripMenuItem_Click);
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
            // toolStripLabel1
            // 
            resources.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
            this.toolStripLabel1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripLabel1.Name = "toolStripLabel1";
            // 
            // toolStripLabel2
            // 
            resources.ApplyResources(this.toolStripLabel2, "toolStripLabel2");
            this.toolStripLabel2.ForeColor = System.Drawing.Color.Blue;
            this.toolStripLabel2.Name = "toolStripLabel2";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // toolStripLabel3
            // 
            resources.ApplyResources(this.toolStripLabel3, "toolStripLabel3");
            this.toolStripLabel3.ForeColor = System.Drawing.Color.Blue;
            this.toolStripLabel3.Name = "toolStripLabel3";
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataSource = this.baseDeDatosDataSet;
            this.bindingSource1.Position = 0;
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
            // UserOrdenar
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip31);
            this.Name = "UserOrdenar";
            this.Load += new System.EventHandler(this.UserOrdenar_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
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
        private BaseDeDatosDataSetTableAdapters.Prod_detalleTableAdapter prod_detalleTableAdapter;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private TreeViewBound.TreeViewBound tvwCarpetas;
        private System.Windows.Forms.ListView listViewProductos;
        private BaseDeDatosDataSetTableAdapters.carpeta_detalleTableAdapter carpeta_detalleTableAdapter;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        public System.Windows.Forms.ToolStrip toolStrip31;
        private System.Windows.Forms.ToolStripButton StripCerrar;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.ToolStripButton StripEnviar;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.ToolStripButton StripGuardar;
       
    }
}