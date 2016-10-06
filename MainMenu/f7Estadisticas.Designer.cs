namespace MainMenu
{
    partial class f7Estadisticas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(f7Estadisticas));
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.treListadoR = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.tbxFecha = new System.Windows.Forms.Label();
            this.pnldetalle = new System.Windows.Forms.Panel();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxDatofin = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbxDatoini = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DesgloseImpuestos = new System.Windows.Forms.CheckBox();
            this.cBxNserie = new System.Windows.Forms.ComboBox();
            this.tbxFeinicio = new System.Windows.Forms.DateTimePicker();
            this.Label7 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.tbxVendfin = new System.Windows.Forms.TextBox();
            this.tbxProdfin = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.tbxFefinal = new System.Windows.Forms.DateTimePicker();
            this.tbxVendini = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbxProdini = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip4 = new System.Windows.Forms.ToolStrip();
            this.ribGeneral = new System.Windows.Forms.ToolStripButton();
            this.ribEstadistico = new System.Windows.Forms.ToolStripButton();
            this.ribVentas = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.BaseDeDatosDataSet = new MainMenu.BaseDeDatosDataSet();
            this.Ventas_DetalleBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.Ventas_DetalleTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.Ventas_DetalleTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.pnldetalle.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.toolStrip4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BaseDeDatosDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Ventas_DetalleBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer2
            // 
            resources.ApplyResources(this.splitContainer2, "splitContainer2");
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            resources.ApplyResources(this.splitContainer2.Panel1, "splitContainer2.Panel1");
            this.splitContainer2.Panel1.Controls.Add(this.treListadoR);
            // 
            // splitContainer2.Panel2
            // 
            resources.ApplyResources(this.splitContainer2.Panel2, "splitContainer2.Panel2");
            this.splitContainer2.Panel2.Controls.Add(this.tableLayoutPanel1);
            // 
            // treListadoR
            // 
            resources.ApplyResources(this.treListadoR, "treListadoR");
            this.treListadoR.ImageList = this.imageList1;
            this.treListadoR.Name = "treListadoR";
            this.treListadoR.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeListado_AfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Document Text.png");
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.panel4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnldetalle, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel4.Controls.Add(this.label14);
            this.panel4.Controls.Add(this.tbxFecha);
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label14.Name = "label14";
            // 
            // tbxFecha
            // 
            resources.ApplyResources(this.tbxFecha, "tbxFecha");
            this.tbxFecha.ForeColor = System.Drawing.Color.Blue;
            this.tbxFecha.Name = "tbxFecha";
            // 
            // pnldetalle
            // 
            this.pnldetalle.Controls.Add(this.btnImprimir);
            this.pnldetalle.Controls.Add(this.groupBox2);
            this.pnldetalle.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.pnldetalle, "pnldetalle");
            this.pnldetalle.Name = "pnldetalle";
            // 
            // btnImprimir
            // 
            this.btnImprimir.Image = global::MainMenu.Properties.Resources.print;
            resources.ApplyResources(this.btnImprimir, "btnImprimir");
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.UseVisualStyleBackColor = true;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.tbxDatofin);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.tbxDatoini);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Cursor = System.Windows.Forms.Cursors.Default;
            this.label9.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label9.Name = "label9";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label3.Name = "label3";
            // 
            // tbxDatofin
            // 
            this.tbxDatofin.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tbxDatofin, "tbxDatofin");
            this.tbxDatofin.Name = "tbxDatofin";
            this.tbxDatofin.Enter += new System.EventHandler(this.tbxDatofin_Enter);
            this.tbxDatofin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxDatofin_KeyDown);
            this.tbxDatofin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbx_KeyPress);
            this.tbxDatofin.Leave += new System.EventHandler(this.tbxDatofin_Leave);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label4.Name = "label4";
            // 
            // tbxDatoini
            // 
            this.tbxDatoini.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tbxDatoini, "tbxDatoini");
            this.tbxDatoini.Name = "tbxDatoini";
            this.tbxDatoini.Enter += new System.EventHandler(this.tbxDatoini_Enter);
            this.tbxDatoini.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxDatoini_KeyDown);
            this.tbxDatoini.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbx_KeyPress);
            this.tbxDatoini.Leave += new System.EventHandler(this.tbxDatoini_Leave);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DesgloseImpuestos);
            this.groupBox1.Controls.Add(this.cBxNserie);
            this.groupBox1.Controls.Add(this.tbxFeinicio);
            this.groupBox1.Controls.Add(this.Label7);
            this.groupBox1.Controls.Add(this.Label6);
            this.groupBox1.Controls.Add(this.Label5);
            this.groupBox1.Controls.Add(this.tbxVendfin);
            this.groupBox1.Controls.Add(this.tbxProdfin);
            this.groupBox1.Controls.Add(this.Label2);
            this.groupBox1.Controls.Add(this.Label1);
            this.groupBox1.Controls.Add(this.tbxFefinal);
            this.groupBox1.Controls.Add(this.tbxVendini);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.tbxProdini);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // DesgloseImpuestos
            // 
            resources.ApplyResources(this.DesgloseImpuestos, "DesgloseImpuestos");
            this.DesgloseImpuestos.Name = "DesgloseImpuestos";
            this.DesgloseImpuestos.UseVisualStyleBackColor = true;
            this.DesgloseImpuestos.CheckedChanged += new System.EventHandler(this.DesgloseImpuestos_CheckedChanged);
            // 
            // cBxNserie
            // 
            this.cBxNserie.FormattingEnabled = true;
            resources.ApplyResources(this.cBxNserie, "cBxNserie");
            this.cBxNserie.Name = "cBxNserie";
            // 
            // tbxFeinicio
            // 
            resources.ApplyResources(this.tbxFeinicio, "tbxFeinicio");
            this.tbxFeinicio.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.tbxFeinicio.Name = "tbxFeinicio";
            this.tbxFeinicio.Value = new System.DateTime(2013, 1, 26, 0, 0, 0, 0);
            this.tbxFeinicio.ValueChanged += new System.EventHandler(this.tbxFeinicio_ValueChanged);
            // 
            // Label7
            // 
            resources.ApplyResources(this.Label7, "Label7");
            this.Label7.BackColor = System.Drawing.Color.Transparent;
            this.Label7.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label7.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label7.Name = "Label7";
            // 
            // Label6
            // 
            resources.ApplyResources(this.Label6, "Label6");
            this.Label6.BackColor = System.Drawing.Color.Transparent;
            this.Label6.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label6.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label6.Name = "Label6";
            // 
            // Label5
            // 
            resources.ApplyResources(this.Label5, "Label5");
            this.Label5.BackColor = System.Drawing.Color.Transparent;
            this.Label5.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label5.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label5.Name = "Label5";
            // 
            // tbxVendfin
            // 
            this.tbxVendfin.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tbxVendfin, "tbxVendfin");
            this.tbxVendfin.Name = "tbxVendfin";
            this.tbxVendfin.Enter += new System.EventHandler(this.tbxVendfin_Enter);
            this.tbxVendfin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxVendfin_KeyDown);
            this.tbxVendfin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbx_KeyPress);
            this.tbxVendfin.Leave += new System.EventHandler(this.tbxVendfin_Leave);
            // 
            // tbxProdfin
            // 
            this.tbxProdfin.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tbxProdfin, "tbxProdfin");
            this.tbxProdfin.Name = "tbxProdfin";
            this.tbxProdfin.Enter += new System.EventHandler(this.tbxProdfin_Enter);
            this.tbxProdfin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxProdfin_KeyDown);
            this.tbxProdfin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbx_KeyPress);
            this.tbxProdfin.Leave += new System.EventHandler(this.tbxProdfin_Leave);
            // 
            // Label2
            // 
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.Label2, "Label2");
            this.Label2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label2.Name = "Label2";
            // 
            // Label1
            // 
            this.Label1.BackColor = System.Drawing.Color.Transparent;
            this.Label1.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.Label1, "Label1");
            this.Label1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label1.Name = "Label1";
            // 
            // tbxFefinal
            // 
            resources.ApplyResources(this.tbxFefinal, "tbxFefinal");
            this.tbxFefinal.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.tbxFefinal.Name = "tbxFefinal";
            this.tbxFefinal.Value = new System.DateTime(2013, 1, 26, 0, 0, 0, 0);
            this.tbxFefinal.ValueChanged += new System.EventHandler(this.tbxFefinal_ValueChanged);
            // 
            // tbxVendini
            // 
            this.tbxVendini.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tbxVendini, "tbxVendini");
            this.tbxVendini.Name = "tbxVendini";
            this.tbxVendini.Enter += new System.EventHandler(this.tbxVendini_Enter);
            this.tbxVendini.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxVendini_KeyDown);
            this.tbxVendini.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbx_KeyPress);
            this.tbxVendini.Leave += new System.EventHandler(this.tbxVendini_Leave);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Cursor = System.Windows.Forms.Cursors.Default;
            this.label8.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label8.Name = "label8";
            // 
            // tbxProdini
            // 
            this.tbxProdini.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tbxProdini, "tbxProdini");
            this.tbxProdini.Name = "tbxProdini";
            this.tbxProdini.Enter += new System.EventHandler(this.tbxProdini_Enter);
            this.tbxProdini.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxProdini_KeyDown);
            this.tbxProdini.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbx_KeyPress);
            this.tbxProdini.Leave += new System.EventHandler(this.tbxProdini_Leave);
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.splitContainer2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.toolStrip4, 0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // toolStrip4
            // 
            this.toolStrip4.BackgroundImage = global::MainMenu.Properties.Resources.fondo2;
            resources.ApplyResources(this.toolStrip4, "toolStrip4");
            this.toolStrip4.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ribGeneral,
            this.ribEstadistico,
            this.ribVentas,
            this.toolStripSeparator1,
            this.toolStripButton1});
            this.toolStrip4.Name = "toolStrip4";
            // 
            // ribGeneral
            // 
            this.ribGeneral.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ribGeneral.Image = global::MainMenu.Properties.Resources.rgeneral;
            resources.ApplyResources(this.ribGeneral, "ribGeneral");
            this.ribGeneral.Name = "ribGeneral";
            this.ribGeneral.Click += new System.EventHandler(this.ribGeneral_Click);
            // 
            // ribEstadistico
            // 
            this.ribEstadistico.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ribEstadistico.Image = global::MainMenu.Properties.Resources.rstadistica;
            resources.ApplyResources(this.ribEstadistico, "ribEstadistico");
            this.ribEstadistico.Name = "ribEstadistico";
            this.ribEstadistico.Click += new System.EventHandler(this.ribEstadistica_Click);
            // 
            // ribVentas
            // 
            this.ribVentas.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ribVentas.Image = global::MainMenu.Properties.Resources.rventas;
            resources.ApplyResources(this.ribVentas, "ribVentas");
            this.ribVentas.Name = "ribVentas";
            this.ribVentas.Click += new System.EventHandler(this.ribVentas_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripButton1.Image = global::MainMenu.Properties.Resources.cancelar;
            resources.ApplyResources(this.toolStripButton1, "toolStripButton1");
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // BaseDeDatosDataSet
            // 
            this.BaseDeDatosDataSet.DataSetName = "BaseDeDatosDataSet";
            this.BaseDeDatosDataSet.EnforceConstraints = false;
            this.BaseDeDatosDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Ventas_DetalleBindingSource
            // 
            this.Ventas_DetalleBindingSource.DataMember = "Ventas_Detalle";
            this.Ventas_DetalleBindingSource.DataSource = this.BaseDeDatosDataSet;
            // 
            // Ventas_DetalleTableAdapter
            // 
            this.Ventas_DetalleTableAdapter.ClearBeforeFill = true;
            // 
            // f7Estadisticas
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "f7Estadisticas";
            this.Load += new System.EventHandler(this.f7Estadisticas_Load);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.pnldetalle.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.toolStrip4.ResumeLayout(false);
            this.toolStrip4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BaseDeDatosDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Ventas_DetalleBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnldetalle;
        public System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker tbxFeinicio;
        private System.Windows.Forms.DateTimePicker tbxFefinal;
        public System.Windows.Forms.Label Label1;
        public System.Windows.Forms.Label Label2;
        public System.Windows.Forms.Label Label5;
        public System.Windows.Forms.Label Label6;
        public System.Windows.Forms.Label Label7;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TreeView treListadoR;
        private System.Windows.Forms.TextBox tbxVendfin;
        private System.Windows.Forms.TextBox tbxProdfin;
        private System.Windows.Forms.TextBox tbxVendini;
        private System.Windows.Forms.TextBox tbxProdini;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label tbxFecha;   
        private System.Windows.Forms.ToolStrip toolStrip4;
        private System.Windows.Forms.ToolStripButton ribGeneral;
        private System.Windows.Forms.ToolStripButton ribEstadistico;
        private System.Windows.Forms.ToolStripButton ribVentas;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbxDatofin;
        private System.Windows.Forms.TextBox tbxDatoini;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.Label label9;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label label4;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btnImprimir;
        private System.Windows.Forms.ComboBox cBxNserie;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.BindingSource Ventas_DetalleBindingSource;
        private BaseDeDatosDataSet BaseDeDatosDataSet;
        private BaseDeDatosDataSetTableAdapters.Ventas_DetalleTableAdapter Ventas_DetalleTableAdapter;
        private System.Windows.Forms.CheckBox DesgloseImpuestos;

    }
}