namespace MainMenu
{
    partial class UserOfertas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserOfertas));
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.treListadoO = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.tbxFind = new System.Windows.Forms.TextBox();
            this.pnldetalle = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnImportar = new System.Windows.Forms.ToolStripButton();
            this.btnExportar = new System.Windows.Forms.ToolStripButton();
            this.btnDepurar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnNuevo = new System.Windows.Forms.ToolStripButton();
            this.btnEditar = new System.Windows.Forms.ToolStripButton();
            this.btnGuardar = new System.Windows.Forms.ToolStripButton();
            this.btnBorrar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCerrar = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tbxFecha = new System.Windows.Forms.ToolStripLabel();
            this.label7 = new System.Windows.Forms.Label();
            this.tbxDescuento = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbxNombre = new System.Windows.Forms.TextBox();
            this.dateTimeFin = new System.Windows.Forms.DateTimePicker();
            this.dateTimeInicio = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbxOferta = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.baseDeDatosDataSet = new MainMenu.BaseDeDatosDataSet();
            this.ofertaTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.OfertaTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnldetalle.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseDeDatosDataSet)).BeginInit();
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
            this.splitContainer2.Panel1.Controls.Add(this.panel2);
            this.splitContainer2.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.pnldetalle);
            this.splitContainer2.Panel2.Controls.Add(this.panel4);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.treListadoO);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // treListadoO
            // 
            resources.ApplyResources(this.treListadoO, "treListadoO");
            this.treListadoO.ImageList = this.imageList1;
            this.treListadoO.Name = "treListadoO";
            this.treListadoO.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeListado_AfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Document Text.png");
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.tbxFind);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.button1, "button1");
            this.button1.Image = global::MainMenu.Properties.Resources.Search;
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbxFind
            // 
            resources.ApplyResources(this.tbxFind, "tbxFind");
            this.tbxFind.Name = "tbxFind";
            this.tbxFind.Enter += new System.EventHandler(this.tbxFind_Enter);
            this.tbxFind.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxFind_KeyDown);
            this.tbxFind.Leave += new System.EventHandler(this.tbxFind_Leave);
            // 
            // pnldetalle
            // 
            resources.ApplyResources(this.pnldetalle, "pnldetalle");
            this.pnldetalle.Controls.Add(this.label8);
            this.pnldetalle.Controls.Add(this.label6);
            this.pnldetalle.Controls.Add(this.toolStrip1);
            this.pnldetalle.Controls.Add(this.label7);
            this.pnldetalle.Controls.Add(this.tbxDescuento);
            this.pnldetalle.Controls.Add(this.label4);
            this.pnldetalle.Controls.Add(this.label5);
            this.pnldetalle.Controls.Add(this.tbxNombre);
            this.pnldetalle.Controls.Add(this.dateTimeFin);
            this.pnldetalle.Controls.Add(this.dateTimeInicio);
            this.pnldetalle.Controls.Add(this.label3);
            this.pnldetalle.Controls.Add(this.label1);
            this.pnldetalle.Controls.Add(this.tbxOferta);
            this.pnldetalle.Controls.Add(this.label2);
            this.pnldetalle.Name = "pnldetalle";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.LightSteelBlue;
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnImportar,
            this.btnExportar,
            this.btnDepurar,
            this.toolStripSeparator1,
            this.btnNuevo,
            this.btnEditar,
            this.btnGuardar,
            this.btnBorrar,
            this.toolStripSeparator2,
            this.btnCerrar,
            this.toolStripLabel1,
            this.tbxFecha});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnImportar
            // 
            this.btnImportar.Image = global::MainMenu.Properties.Resources.import;
            resources.ApplyResources(this.btnImportar, "btnImportar");
            this.btnImportar.Name = "btnImportar";
            this.btnImportar.Click += new System.EventHandler(this.tbnImportar_Click);
            // 
            // btnExportar
            // 
            this.btnExportar.Image = global::MainMenu.Properties.Resources.export;
            resources.ApplyResources(this.btnExportar, "btnExportar");
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Click += new System.EventHandler(this.btnExportar_Click);
            // 
            // btnDepurar
            // 
            this.btnDepurar.Image = global::MainMenu.Properties.Resources.purge;
            resources.ApplyResources(this.btnDepurar, "btnDepurar");
            this.btnDepurar.Name = "btnDepurar";
            this.btnDepurar.Click += new System.EventHandler(this.btnDepurar_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // btnNuevo
            // 
            this.btnNuevo.Image = global::MainMenu.Properties.Resources.nuevo;
            resources.ApplyResources(this.btnNuevo, "btnNuevo");
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            // 
            // btnEditar
            // 
            this.btnEditar.Image = global::MainMenu.Properties.Resources.edit3;
            resources.ApplyResources(this.btnEditar, "btnEditar");
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.Image = global::MainMenu.Properties.Resources.save3;
            resources.ApplyResources(this.btnGuardar, "btnGuardar");
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnBorrar
            // 
            this.btnBorrar.Image = global::MainMenu.Properties.Resources.eliminar;
            resources.ApplyResources(this.btnBorrar, "btnBorrar");
            this.btnBorrar.Name = "btnBorrar";
            this.btnBorrar.Click += new System.EventHandler(this.btnBorrar_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // btnCerrar
            // 
            this.btnCerrar.Image = global::MainMenu.Properties.Resources.cancelar;
            resources.ApplyResources(this.btnCerrar, "btnCerrar");
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.toolStripLabel1.Name = "toolStripLabel1";
            resources.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
            // 
            // tbxFecha
            // 
            resources.ApplyResources(this.tbxFecha, "tbxFecha");
            this.tbxFecha.ForeColor = System.Drawing.Color.Blue;
            this.tbxFecha.Name = "tbxFecha";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
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
            this.tbxDescuento.Validated += new System.EventHandler(this.tbxDescuento_Validated);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Name = "label5";
            // 
            // tbxNombre
            // 
            this.tbxNombre.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this.tbxNombre, "tbxNombre");
            this.tbxNombre.Name = "tbxNombre";
            this.tbxNombre.Enter += new System.EventHandler(this.tbxNombre_Enter);
            this.tbxNombre.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxNombre_KeyDown);
            this.tbxNombre.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxNombre_KeyPress);
            this.tbxNombre.Leave += new System.EventHandler(this.tbxNombre_Leave);
            // 
            // dateTimeFin
            // 
            resources.ApplyResources(this.dateTimeFin, "dateTimeFin");
            this.dateTimeFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimeFin.Name = "dateTimeFin";
            this.dateTimeFin.Value = new System.DateTime(2015, 6, 9, 12, 46, 46, 0);
            this.dateTimeFin.Enter += new System.EventHandler(this.dateTimeFin_Enter);
            this.dateTimeFin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dateTimeFin_KeyDown);
            this.dateTimeFin.Leave += new System.EventHandler(this.dateTimeFin_Leave);
            // 
            // dateTimeInicio
            // 
            resources.ApplyResources(this.dateTimeInicio, "dateTimeInicio");
            this.dateTimeInicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimeInicio.Name = "dateTimeInicio";
            this.dateTimeInicio.Value = new System.DateTime(2015, 6, 9, 0, 0, 0, 0);
            this.dateTimeInicio.Enter += new System.EventHandler(this.dateTimeInicio_Enter);
            this.dateTimeInicio.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dateTimeInicio_KeyDown);
            this.dateTimeInicio.Leave += new System.EventHandler(this.dateTimeInicio_Leave);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Name = "label3";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Name = "label1";
            // 
            // tbxOferta
            // 
            this.tbxOferta.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this.tbxOferta, "tbxOferta");
            this.tbxOferta.Name = "tbxOferta";
            this.tbxOferta.Enter += new System.EventHandler(this.tbxOferta_Enter);
            this.tbxOferta.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxOferta_KeyDown);
            this.tbxOferta.Leave += new System.EventHandler(this.tbxOferta_Leave);
            this.tbxOferta.Validated += new System.EventHandler(this.tbxOferta_Validated);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Name = "label2";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.LightSteelBlue;
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "Oferta";
            this.bindingSource1.DataSource = this.baseDeDatosDataSet;
            // 
            // baseDeDatosDataSet
            // 
            this.baseDeDatosDataSet.DataSetName = "BaseDeDatosDataSet";
            this.baseDeDatosDataSet.EnforceConstraints = false;
            this.baseDeDatosDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ofertaTableAdapter
            // 
            this.ofertaTableAdapter.ClearBeforeFill = true;
            // 
            // UserOfertas
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.splitContainer2);
            this.Name = "UserOfertas";
            this.Load += new System.EventHandler(this.UserOfertas_Load);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnldetalle.ResumeLayout(false);
            this.pnldetalle.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseDeDatosDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TreeView treListadoO;
        private System.Windows.Forms.Panel pnldetalle;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxOferta;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxDescuento;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dateTimeFin;
        private System.Windows.Forms.DateTimePicker dateTimeInicio;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.BindingSource bindingSource1;
        private BaseDeDatosDataSet baseDeDatosDataSet;
        private BaseDeDatosDataSetTableAdapters.OfertaTableAdapter ofertaTableAdapter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbxNombre;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbxFind;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnNuevo;
        private System.Windows.Forms.ToolStripButton btnEditar;
        private System.Windows.Forms.ToolStripButton btnGuardar;
        private System.Windows.Forms.ToolStripButton btnBorrar;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnCerrar;
        public System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel tbxFecha;
        private System.Windows.Forms.ToolStripButton btnDepurar;
        private System.Windows.Forms.ToolStripButton btnExportar;
        private System.Windows.Forms.ToolStripButton btnImportar;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;



    }
}