namespace MainMenu
{
    partial class UserCarpetas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserCarpetas));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvwCarpetas = new TreeViewBound.TreeViewBound();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.crearCarpetasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editarCarpetasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.borrarCarpetasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.listViewProductos = new System.Windows.Forms.ListView();
            this.toolStrip31 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.crearCarpetasToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.editarCarpetasToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.borrarCarpetasToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.StripProductos = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.StripEnviar = new System.Windows.Forms.ToolStripDropDownButton();
            this.TodaEstructuraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.soloCarpetasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.soloProductosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.contextMenuStrip1.SuspendLayout();
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
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.Controls.Add(this.tvwCarpetas);
            // 
            // splitContainer1.Panel2
            // 
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Panel2.Controls.Add(this.panel3);
            // 
            // tvwCarpetas
            // 
            resources.ApplyResources(this.tvwCarpetas, "tvwCarpetas");
            this.tvwCarpetas.ContextMenuStrip = this.contextMenuStrip1;
            this.tvwCarpetas.DisplayMember = "";
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
            this.tvwCarpetas.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvwCarpetas_NodeMouseClick);
            this.tvwCarpetas.Click += new System.EventHandler(this.tvwCarpetas_Click);
            this.tvwCarpetas.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvwCarpetas_DragDrop);
            this.tvwCarpetas.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvwCarpetas_DragEnter);
            this.tvwCarpetas.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvwCarpetas_KeyDown);
            // 
            // contextMenuStrip1
            // 
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            this.contextMenuStrip1.BackColor = System.Drawing.Color.AliceBlue;
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.crearCarpetasToolStripMenuItem,
            this.editarCarpetasToolStripMenuItem,
            this.borrarCarpetasToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            // 
            // crearCarpetasToolStripMenuItem
            // 
            resources.ApplyResources(this.crearCarpetasToolStripMenuItem, "crearCarpetasToolStripMenuItem");
            this.crearCarpetasToolStripMenuItem.Image = global::MainMenu.Properties.Resources.folderadd16;
            this.crearCarpetasToolStripMenuItem.Name = "crearCarpetasToolStripMenuItem";
            this.crearCarpetasToolStripMenuItem.Click += new System.EventHandler(this.crearCarpetasToolStripMenuItem_Click);
            // 
            // editarCarpetasToolStripMenuItem
            // 
            resources.ApplyResources(this.editarCarpetasToolStripMenuItem, "editarCarpetasToolStripMenuItem");
            this.editarCarpetasToolStripMenuItem.Image = global::MainMenu.Properties.Resources.folderedit16;
            this.editarCarpetasToolStripMenuItem.Name = "editarCarpetasToolStripMenuItem";
            this.editarCarpetasToolStripMenuItem.Click += new System.EventHandler(this.editarCarpetasToolStripMenuItem_Click);
            // 
            // borrarCarpetasToolStripMenuItem
            // 
            resources.ApplyResources(this.borrarCarpetasToolStripMenuItem, "borrarCarpetasToolStripMenuItem");
            this.borrarCarpetasToolStripMenuItem.Image = global::MainMenu.Properties.Resources.folderdel16;
            this.borrarCarpetasToolStripMenuItem.Name = "borrarCarpetasToolStripMenuItem";
            this.borrarCarpetasToolStripMenuItem.Click += new System.EventHandler(this.borrarCarpetasToolStripMenuItem_Click);
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
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Controls.Add(this.listViewProductos);
            this.panel3.Name = "panel3";
            // 
            // listViewProductos
            // 
            resources.ApplyResources(this.listViewProductos, "listViewProductos");
            this.listViewProductos.BackColor = System.Drawing.Color.WhiteSmoke;
            this.listViewProductos.FullRowSelect = true;
            this.listViewProductos.Name = "listViewProductos";
            this.listViewProductos.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewProductos.UseCompatibleStateImageBehavior = false;
            this.listViewProductos.View = System.Windows.Forms.View.Details;
            this.listViewProductos.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewProductos_ColumnClick);
            this.listViewProductos.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listViewProductos_ItemDrag);
            this.listViewProductos.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewProductos_DragDrop);
            this.listViewProductos.DragEnter += new System.Windows.Forms.DragEventHandler(this.listViewProductos_DragEnter);
            this.listViewProductos.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewProductos_KeyDown);
            // 
            // toolStrip31
            // 
            resources.ApplyResources(this.toolStrip31, "toolStrip31");
            this.toolStrip31.BackColor = System.Drawing.Color.LightSteelBlue;
            this.toolStrip31.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip31.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip31.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.StripProductos,
            this.toolStripSeparator2,
            this.StripEnviar,
            this.StripCerrar,
            this.toolStripSeparator3,
            this.toolStripLabel1,
            this.toolStripLabel2,
            this.toolStripSeparator1,
            this.toolStripLabel3});
            this.toolStrip31.Name = "toolStrip31";
            // 
            // toolStripButton1
            // 
            resources.ApplyResources(this.toolStripButton1, "toolStripButton1");
            this.toolStripButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.crearCarpetasToolStripMenuItem1,
            this.editarCarpetasToolStripMenuItem1,
            this.borrarCarpetasToolStripMenuItem1});
            this.toolStripButton1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripButton1.Image = global::MainMenu.Properties.Resources.grupo;
            this.toolStripButton1.Name = "toolStripButton1";
            // 
            // crearCarpetasToolStripMenuItem1
            // 
            resources.ApplyResources(this.crearCarpetasToolStripMenuItem1, "crearCarpetasToolStripMenuItem1");
            this.crearCarpetasToolStripMenuItem1.Name = "crearCarpetasToolStripMenuItem1";
            this.crearCarpetasToolStripMenuItem1.Click += new System.EventHandler(this.crearCarpetasToolStripMenuItem_Click);
            // 
            // editarCarpetasToolStripMenuItem1
            // 
            resources.ApplyResources(this.editarCarpetasToolStripMenuItem1, "editarCarpetasToolStripMenuItem1");
            this.editarCarpetasToolStripMenuItem1.Name = "editarCarpetasToolStripMenuItem1";
            this.editarCarpetasToolStripMenuItem1.Click += new System.EventHandler(this.editarCarpetasToolStripMenuItem_Click);
            // 
            // borrarCarpetasToolStripMenuItem1
            // 
            resources.ApplyResources(this.borrarCarpetasToolStripMenuItem1, "borrarCarpetasToolStripMenuItem1");
            this.borrarCarpetasToolStripMenuItem1.Name = "borrarCarpetasToolStripMenuItem1";
            this.borrarCarpetasToolStripMenuItem1.Click += new System.EventHandler(this.borrarCarpetasToolStripMenuItem_Click);
            // 
            // StripProductos
            // 
            resources.ApplyResources(this.StripProductos, "StripProductos");
            this.StripProductos.ForeColor = System.Drawing.Color.MidnightBlue;
            this.StripProductos.Image = global::MainMenu.Properties.Resources.producto2;
            this.StripProductos.Name = "StripProductos";
            this.StripProductos.Click += new System.EventHandler(this.StripProductos_Click);
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // StripEnviar
            // 
            resources.ApplyResources(this.StripEnviar, "StripEnviar");
            this.StripEnviar.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TodaEstructuraToolStripMenuItem,
            this.soloCarpetasToolStripMenuItem,
            this.soloProductosToolStripMenuItem});
            this.StripEnviar.ForeColor = System.Drawing.Color.MidnightBlue;
            this.StripEnviar.Image = global::MainMenu.Properties.Resources.connect;
            this.StripEnviar.Name = "StripEnviar";
            // 
            // TodaEstructuraToolStripMenuItem
            // 
            resources.ApplyResources(this.TodaEstructuraToolStripMenuItem, "TodaEstructuraToolStripMenuItem");
            this.TodaEstructuraToolStripMenuItem.Name = "TodaEstructuraToolStripMenuItem";
            this.TodaEstructuraToolStripMenuItem.Click += new System.EventHandler(this.TodaEstructuraToolStripMenuItem_Click);
            // 
            // soloCarpetasToolStripMenuItem
            // 
            resources.ApplyResources(this.soloCarpetasToolStripMenuItem, "soloCarpetasToolStripMenuItem");
            this.soloCarpetasToolStripMenuItem.Name = "soloCarpetasToolStripMenuItem";
            this.soloCarpetasToolStripMenuItem.Click += new System.EventHandler(this.soloCarpetasToolStripMenuItem_Click);
            // 
            // soloProductosToolStripMenuItem
            // 
            resources.ApplyResources(this.soloProductosToolStripMenuItem, "soloProductosToolStripMenuItem");
            this.soloProductosToolStripMenuItem.Name = "soloProductosToolStripMenuItem";
            this.soloProductosToolStripMenuItem.Click += new System.EventHandler(this.soloProductosToolStripMenuItem_Click);
            // 
            // StripCerrar
            // 
            resources.ApplyResources(this.StripCerrar, "StripCerrar");
            this.StripCerrar.ForeColor = System.Drawing.Color.MidnightBlue;
            this.StripCerrar.Image = global::MainMenu.Properties.Resources.cancelar;
            this.StripCerrar.Name = "StripCerrar";
            this.StripCerrar.Click += new System.EventHandler(this.StripCerrar_Click);
            // 
            // toolStripSeparator3
            // 
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // toolStripLabel1
            // 
            resources.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
            this.toolStripLabel1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
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
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
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
            // UserCarpetas
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip31);
            this.Name = "UserCarpetas";
            this.Load += new System.EventHandler(this.UserCarpetas_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
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
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem crearCarpetasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editarCarpetasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem borrarCarpetasToolStripMenuItem;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ToolStripButton StripProductos;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton StripEnviar;
        private System.Windows.Forms.ToolStripMenuItem soloCarpetasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem soloProductosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TodaEstructuraToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripButton1;
        private System.Windows.Forms.ToolStripMenuItem crearCarpetasToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem editarCarpetasToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem borrarCarpetasToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        public System.Windows.Forms.ToolStrip toolStrip31;
        private System.Windows.Forms.ToolStripButton StripCerrar;
        private System.IO.Ports.SerialPort serialPort1;
       
    }
}