namespace MainMenu
{
    partial class f4Precio
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(f4Precio));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.baseDeDatosDataSet = new MainMenu.BaseDeDatosDataSet();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.productosTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.ProductosTableAdapter();
            this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.basculaTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.BasculaTableAdapter();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dtG = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbxcaducidad = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxprecio = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.tbxPLU = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxdescripcion = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbxCodigo = new System.Windows.Forms.Label();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLoad = new System.Windows.Forms.ToolStripButton();
            this.toolStripRango = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripEnviar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripCerrar = new System.Windows.Forms.ToolStripButton();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.pLU_detalleTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.PLU_detalleTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.baseDeDatosDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtG)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseDeDatosDataSet
            // 
            this.baseDeDatosDataSet.DataSetName = "BaseDeDatosDataSet";
            this.baseDeDatosDataSet.EnforceConstraints = false;
            this.baseDeDatosDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataSource = this.baseDeDatosDataSet;
            this.bindingSource1.Position = 0;
            // 
            // bindingSource2
            // 
            this.bindingSource2.DataMember = "PLU_detalle";
            this.bindingSource2.DataSource = this.baseDeDatosDataSet;
            // 
            // productosTableAdapter
            // 
            this.productosTableAdapter.ClearBeforeFill = true;
            // 
            // dlgOpenFile
            // 
            this.dlgOpenFile.FileName = "dlgOpenFile";
            // 
            // basculaTableAdapter
            // 
            this.basculaTableAdapter.ClearBeforeFill = true;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.dtG, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // dtG
            // 
            resources.ApplyResources(this.dtG, "dtG");
            this.dtG.AutoGenerateColumns = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtG.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dtG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtG.DataSource = this.bindingSource1;
            this.dtG.Name = "dtG";
            this.dtG.ReadOnly = true;
            this.dtG.ReadOnlyChanged += new System.EventHandler(this.dtG_ReadOnlyChanged);
            this.dtG.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dtG_CellBeginEdit);
            this.dtG.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtG_CellClick);
            this.dtG.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtG_CellEnter);
            this.dtG.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtG_CellValueChanged);
            this.dtG.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dtG_DataError);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbxcaducidad);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbxprecio);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.tbxPLU);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbxdescripcion);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbxCodigo);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // tbxcaducidad
            // 
            this.tbxcaducidad.BackColor = System.Drawing.Color.AliceBlue;
            this.tbxcaducidad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxcaducidad.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.tbxcaducidad, "tbxcaducidad");
            this.tbxcaducidad.Name = "tbxcaducidad";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Name = "label5";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Name = "label3";
            // 
            // tbxprecio
            // 
            this.tbxprecio.BackColor = System.Drawing.Color.AliceBlue;
            this.tbxprecio.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxprecio.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.tbxprecio, "tbxprecio");
            this.tbxprecio.Name = "tbxprecio";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.ForeColor = System.Drawing.Color.Black;
            this.label17.Name = "label17";
            // 
            // tbxPLU
            // 
            this.tbxPLU.BackColor = System.Drawing.Color.AliceBlue;
            this.tbxPLU.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxPLU.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.tbxPLU, "tbxPLU");
            this.tbxPLU.Name = "tbxPLU";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Name = "label2";
            // 
            // tbxdescripcion
            // 
            this.tbxdescripcion.BackColor = System.Drawing.Color.AliceBlue;
            this.tbxdescripcion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxdescripcion.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.tbxdescripcion, "tbxdescripcion");
            this.tbxdescripcion.Name = "tbxdescripcion";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Name = "label1";
            // 
            // tbxCodigo
            // 
            this.tbxCodigo.BackColor = System.Drawing.Color.AliceBlue;
            this.tbxCodigo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxCodigo.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.tbxCodigo, "tbxCodigo");
            this.tbxCodigo.Name = "tbxCodigo";
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.BackColor = System.Drawing.Color.AliceBlue;
            this.bindingNavigator1.BackgroundImage = global::MainMenu.Properties.Resources.fondo2;
            this.bindingNavigator1.BindingSource = this.bindingSource1;
            this.bindingNavigator1.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigator1.CountItemFormat = "of {0}";
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.toolStripLoad,
            this.toolStripRango,
            this.toolStripSeparator1,
            this.toolStripEnviar,
            this.toolStripSeparator2,
            this.toolStripCerrar});
            resources.ApplyResources(this.bindingNavigator1, "bindingNavigator1");
            this.bindingNavigator1.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigator1.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigator1.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigator1.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigator1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.bindingNavigator1_ItemClicked);
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.ForeColor = System.Drawing.Color.MidnightBlue;
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            resources.ApplyResources(this.bindingNavigatorCountItem, "bindingNavigatorCountItem");
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.bindingNavigatorMoveFirstItem, "bindingNavigatorMoveFirstItem");
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.bindingNavigatorMovePreviousItem, "bindingNavigatorMovePreviousItem");
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            resources.ApplyResources(this.bindingNavigatorSeparator, "bindingNavigatorSeparator");
            // 
            // bindingNavigatorPositionItem
            // 
            resources.ApplyResources(this.bindingNavigatorPositionItem, "bindingNavigatorPositionItem");
            this.bindingNavigatorPositionItem.ForeColor = System.Drawing.Color.MidnightBlue;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            resources.ApplyResources(this.bindingNavigatorSeparator1, "bindingNavigatorSeparator1");
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.bindingNavigatorMoveNextItem, "bindingNavigatorMoveNextItem");
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.bindingNavigatorMoveLastItem, "bindingNavigatorMoveLastItem");
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            resources.ApplyResources(this.bindingNavigatorSeparator2, "bindingNavigatorSeparator2");
            // 
            // toolStripLoad
            // 
            this.toolStripLoad.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripLoad.Image = global::MainMenu.Properties.Resources.impprecio;
            resources.ApplyResources(this.toolStripLoad, "toolStripLoad");
            this.toolStripLoad.Name = "toolStripLoad";
            this.toolStripLoad.Click += new System.EventHandler(this.toolStripLoad_Click);
            // 
            // toolStripRango
            // 
            this.toolStripRango.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripRango.Image = global::MainMenu.Properties.Resources.precio;
            resources.ApplyResources(this.toolStripRango, "toolStripRango");
            this.toolStripRango.Name = "toolStripRango";
            this.toolStripRango.Click += new System.EventHandler(this.toolStripRango_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // toolStripEnviar
            // 
            this.toolStripEnviar.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripEnviar.Image = global::MainMenu.Properties.Resources.connect;
            resources.ApplyResources(this.toolStripEnviar, "toolStripEnviar");
            this.toolStripEnviar.Name = "toolStripEnviar";
            this.toolStripEnviar.Click += new System.EventHandler(this.toolStripEnviar_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // toolStripCerrar
            // 
            this.toolStripCerrar.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripCerrar.Image = global::MainMenu.Properties.Resources.cancelar;
            resources.ApplyResources(this.toolStripCerrar, "toolStripCerrar");
            this.toolStripCerrar.Name = "toolStripCerrar";
            this.toolStripCerrar.Click += new System.EventHandler(this.toolStripCerrar_Click);
            // 
            // pLU_detalleTableAdapter
            // 
            this.pLU_detalleTableAdapter.ClearBeforeFill = true;
            // 
            // f4Precio
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.bindingNavigator1);
            this.Name = "f4Precio";
            this.Tag = "";
            this.Load += new System.EventHandler(this.f4Precio_Load);
            ((System.ComponentModel.ISupportInitialize)(this.baseDeDatosDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtG)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripLoad;
        private System.Windows.Forms.ToolStripButton toolStripEnviar;
        private System.Windows.Forms.ToolStripButton toolStripCerrar;
        private BaseDeDatosDataSet baseDeDatosDataSet;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.DataGridView dtG;
        private System.Windows.Forms.BindingSource bindingSource2;
        private BaseDeDatosDataSetTableAdapters.ProductosTableAdapter productosTableAdapter;
        private System.Windows.Forms.ToolStripButton toolStripRango;
        private System.Windows.Forms.OpenFileDialog dlgOpenFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label tbxCodigo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label tbxdescripcion;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label tbxPLU;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label tbxprecio;
        private System.Windows.Forms.Label tbxcaducidad;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private BaseDeDatosDataSetTableAdapters.BasculaTableAdapter basculaTableAdapter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.IO.Ports.SerialPort serialPort1;
        private BaseDeDatosDataSetTableAdapters.PLU_detalleTableAdapter pLU_detalleTableAdapter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }

}