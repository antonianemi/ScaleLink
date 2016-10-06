namespace MainMenu
{
    partial class UserParametro
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserParametro));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbxDireccion = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbxRazon = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbxCiudad = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.DOMINGO = new System.Windows.Forms.CheckBox();
            this.SABADO = new System.Windows.Forms.CheckBox();
            this.VIERNES = new System.Windows.Forms.CheckBox();
            this.JUEVES = new System.Windows.Forms.CheckBox();
            this.MIERCOLES = new System.Windows.Forms.CheckBox();
            this.MARTES = new System.Windows.Forms.CheckBox();
            this.LUNES = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbxHorainicio = new System.Windows.Forms.DateTimePicker();
            this.tbxHorafin = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.cbxFrecuencia = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.toolStrip51 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnEditar = new System.Windows.Forms.ToolStripButton();
            this.btnGuardar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCerrar = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tbxFecha = new System.Windows.Forms.ToolStripLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkEnvioProgramado = new System.Windows.Forms.CheckBox();
            this.chkEnvio = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rBxFechaForm2 = new System.Windows.Forms.RadioButton();
            this.rBxFechaForm1 = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rBxMonedaForm4 = new System.Windows.Forms.RadioButton();
            this.rBxMonedaForm3 = new System.Windows.Forms.RadioButton();
            this.rBxMonedaForm2 = new System.Windows.Forms.RadioButton();
            this.rBxMonedaForm1 = new System.Windows.Forms.RadioButton();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.rBxUMForm2 = new System.Windows.Forms.RadioButton();
            this.rBxUMForm1 = new System.Windows.Forms.RadioButton();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.rBxIdiomaForm2 = new System.Windows.Forms.RadioButton();
            this.rBxIdiomaForm1 = new System.Windows.Forms.RadioButton();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.baseDeDatosDataSet = new MainMenu.BaseDeDatosDataSet();
            this.datosGralTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.DatosGralTableAdapter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkEnableAds = new System.Windows.Forms.CheckBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.toolStrip51.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseDeDatosDataSet)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbxDireccion);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.tbxRazon);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.tbxCiudad);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // tbxDireccion
            // 
            this.tbxDireccion.AcceptsReturn = true;
            resources.ApplyResources(this.tbxDireccion, "tbxDireccion");
            this.tbxDireccion.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbxDireccion.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tbxDireccion.Name = "tbxDireccion";
            this.tbxDireccion.Enter += new System.EventHandler(this.tbxDireccion_Enter);
            this.tbxDireccion.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxDireccion_KeyDown);
            this.tbxDireccion.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxDireccion_KeyPress);
            this.tbxDireccion.Leave += new System.EventHandler(this.tbxDireccion_Leave);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Cursor = System.Windows.Forms.Cursors.Default;
            this.label6.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label6.Name = "label6";
            // 
            // tbxRazon
            // 
            this.tbxRazon.AcceptsReturn = true;
            resources.ApplyResources(this.tbxRazon, "tbxRazon");
            this.tbxRazon.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbxRazon.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tbxRazon.Name = "tbxRazon";
            this.tbxRazon.Enter += new System.EventHandler(this.tbxRazon_Enter);
            this.tbxRazon.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxRazon_KeyDown);
            this.tbxRazon.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxRazon_KeyPress);
            this.tbxRazon.Leave += new System.EventHandler(this.tbxRazon_Leave);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Cursor = System.Windows.Forms.Cursors.Default;
            this.label7.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label7.Name = "label7";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Cursor = System.Windows.Forms.Cursors.Default;
            this.label8.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label8.Name = "label8";
            // 
            // tbxCiudad
            // 
            this.tbxCiudad.AcceptsReturn = true;
            resources.ApplyResources(this.tbxCiudad, "tbxCiudad");
            this.tbxCiudad.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbxCiudad.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tbxCiudad.Name = "tbxCiudad";
            this.tbxCiudad.Enter += new System.EventHandler(this.tbxCiudad_Enter);
            this.tbxCiudad.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxCiudad_KeyPress);
            this.tbxCiudad.Leave += new System.EventHandler(this.tbxCiudad_Leave);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.DOMINGO);
            this.groupBox5.Controls.Add(this.SABADO);
            this.groupBox5.Controls.Add(this.VIERNES);
            this.groupBox5.Controls.Add(this.JUEVES);
            this.groupBox5.Controls.Add(this.MIERCOLES);
            this.groupBox5.Controls.Add(this.MARTES);
            this.groupBox5.Controls.Add(this.LUNES);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // DOMINGO
            // 
            resources.ApplyResources(this.DOMINGO, "DOMINGO");
            this.DOMINGO.Checked = true;
            this.DOMINGO.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DOMINGO.Name = "DOMINGO";
            this.DOMINGO.UseVisualStyleBackColor = true;
            this.DOMINGO.CheckStateChanged += new System.EventHandler(this.DOMINGO_CheckStateChanged);
            // 
            // SABADO
            // 
            resources.ApplyResources(this.SABADO, "SABADO");
            this.SABADO.Checked = true;
            this.SABADO.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SABADO.Name = "SABADO";
            this.SABADO.UseVisualStyleBackColor = true;
            this.SABADO.CheckStateChanged += new System.EventHandler(this.SABADO_CheckStateChanged);
            // 
            // VIERNES
            // 
            resources.ApplyResources(this.VIERNES, "VIERNES");
            this.VIERNES.Checked = true;
            this.VIERNES.CheckState = System.Windows.Forms.CheckState.Checked;
            this.VIERNES.Name = "VIERNES";
            this.VIERNES.UseVisualStyleBackColor = true;
            this.VIERNES.CheckStateChanged += new System.EventHandler(this.VIERNES_CheckStateChanged);
            // 
            // JUEVES
            // 
            resources.ApplyResources(this.JUEVES, "JUEVES");
            this.JUEVES.Checked = true;
            this.JUEVES.CheckState = System.Windows.Forms.CheckState.Checked;
            this.JUEVES.Name = "JUEVES";
            this.JUEVES.UseVisualStyleBackColor = true;
            this.JUEVES.CheckStateChanged += new System.EventHandler(this.JUEVES_CheckStateChanged);
            // 
            // MIERCOLES
            // 
            resources.ApplyResources(this.MIERCOLES, "MIERCOLES");
            this.MIERCOLES.Checked = true;
            this.MIERCOLES.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MIERCOLES.Name = "MIERCOLES";
            this.MIERCOLES.UseVisualStyleBackColor = true;
            this.MIERCOLES.CheckStateChanged += new System.EventHandler(this.MIERCOLES_CheckStateChanged);
            // 
            // MARTES
            // 
            resources.ApplyResources(this.MARTES, "MARTES");
            this.MARTES.Checked = true;
            this.MARTES.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MARTES.Name = "MARTES";
            this.MARTES.UseVisualStyleBackColor = true;
            this.MARTES.CheckStateChanged += new System.EventHandler(this.MARTES_CheckStateChanged);
            // 
            // LUNES
            // 
            resources.ApplyResources(this.LUNES, "LUNES");
            this.LUNES.Checked = true;
            this.LUNES.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LUNES.Name = "LUNES";
            this.LUNES.UseVisualStyleBackColor = true;
            this.LUNES.CheckStateChanged += new System.EventHandler(this.LUNES_CheckStateChanged);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // tbxHorainicio
            // 
            resources.ApplyResources(this.tbxHorainicio, "tbxHorainicio");
            this.tbxHorainicio.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.tbxHorainicio.Name = "tbxHorainicio";
            this.tbxHorainicio.ShowUpDown = true;
            this.tbxHorainicio.Value = new System.DateTime(2012, 1, 1, 0, 0, 0, 0);
            this.tbxHorainicio.ValueChanged += new System.EventHandler(this.tbxHorainicio_ValueChanged);
            // 
            // tbxHorafin
            // 
            resources.ApplyResources(this.tbxHorafin, "tbxHorafin");
            this.tbxHorafin.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.tbxHorafin.Name = "tbxHorafin";
            this.tbxHorafin.ShowUpDown = true;
            this.tbxHorafin.Value = new System.DateTime(2011, 12, 31, 0, 0, 0, 0);
            this.tbxHorafin.ValueChanged += new System.EventHandler(this.tbxHorafin_ValueChanged);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // cbxFrecuencia
            // 
            resources.ApplyResources(this.cbxFrecuencia, "cbxFrecuencia");
            this.cbxFrecuencia.FormattingEnabled = true;
            this.cbxFrecuencia.Items.AddRange(new object[] {
            resources.GetString("cbxFrecuencia.Items"),
            resources.GetString("cbxFrecuencia.Items1"),
            resources.GetString("cbxFrecuencia.Items2"),
            resources.GetString("cbxFrecuencia.Items3"),
            resources.GetString("cbxFrecuencia.Items4"),
            resources.GetString("cbxFrecuencia.Items5"),
            resources.GetString("cbxFrecuencia.Items6"),
            resources.GetString("cbxFrecuencia.Items7"),
            resources.GetString("cbxFrecuencia.Items8"),
            resources.GetString("cbxFrecuencia.Items9"),
            resources.GetString("cbxFrecuencia.Items10")});
            this.cbxFrecuencia.Name = "cbxFrecuencia";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // toolStrip51
            // 
            this.toolStrip51.BackColor = System.Drawing.Color.LightSteelBlue;
            resources.ApplyResources(this.toolStrip51, "toolStrip51");
            this.toolStrip51.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip51.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip51.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.btnEditar,
            this.btnGuardar,
            this.toolStripSeparator2,
            this.btnCerrar,
            this.toolStripLabel1,
            this.tbxFecha});
            this.toolStrip51.Name = "toolStrip51";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // btnEditar
            // 
            this.btnEditar.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btnEditar.Image = global::MainMenu.Properties.Resources.edit3;
            resources.ApplyResources(this.btnEditar, "btnEditar");
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btnGuardar.Image = global::MainMenu.Properties.Resources.save3;
            resources.ApplyResources(this.btnGuardar, "btnGuardar");
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // btnCerrar
            // 
            this.btnCerrar.ForeColor = System.Drawing.Color.MidnightBlue;
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkEnvioProgramado);
            this.groupBox1.Controls.Add(this.chkEnvio);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.tbxHorainicio);
            this.groupBox1.Controls.Add(this.cbxFrecuencia);
            this.groupBox1.Controls.Add(this.tbxHorafin);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label9);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // chkEnvioProgramado
            // 
            this.chkEnvioProgramado.Checked = true;
            this.chkEnvioProgramado.CheckState = System.Windows.Forms.CheckState.Checked;
            resources.ApplyResources(this.chkEnvioProgramado, "chkEnvioProgramado");
            this.chkEnvioProgramado.Name = "chkEnvioProgramado";
            this.chkEnvioProgramado.UseVisualStyleBackColor = true;
            // 
            // chkEnvio
            // 
            resources.ApplyResources(this.chkEnvio, "chkEnvio");
            this.chkEnvio.Checked = true;
            this.chkEnvio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnvio.Name = "chkEnvio";
            this.chkEnvio.UseVisualStyleBackColor = true;
            this.chkEnvio.CheckedChanged += new System.EventHandler(this.chkEnvio_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rBxFechaForm2);
            this.groupBox3.Controls.Add(this.rBxFechaForm1);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // rBxFechaForm2
            // 
            resources.ApplyResources(this.rBxFechaForm2, "rBxFechaForm2");
            this.rBxFechaForm2.Name = "rBxFechaForm2";
            this.rBxFechaForm2.TabStop = true;
            this.rBxFechaForm2.UseVisualStyleBackColor = true;
            this.rBxFechaForm2.CheckedChanged += new System.EventHandler(this.rBxFechaForm2_CheckedChanged);
            // 
            // rBxFechaForm1
            // 
            resources.ApplyResources(this.rBxFechaForm1, "rBxFechaForm1");
            this.rBxFechaForm1.Name = "rBxFechaForm1";
            this.rBxFechaForm1.TabStop = true;
            this.rBxFechaForm1.UseVisualStyleBackColor = true;
            this.rBxFechaForm1.CheckedChanged += new System.EventHandler(this.rBxFechaForm1_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rBxMonedaForm4);
            this.groupBox4.Controls.Add(this.rBxMonedaForm3);
            this.groupBox4.Controls.Add(this.rBxMonedaForm2);
            this.groupBox4.Controls.Add(this.rBxMonedaForm1);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // rBxMonedaForm4
            // 
            resources.ApplyResources(this.rBxMonedaForm4, "rBxMonedaForm4");
            this.rBxMonedaForm4.Name = "rBxMonedaForm4";
            this.rBxMonedaForm4.TabStop = true;
            this.rBxMonedaForm4.UseVisualStyleBackColor = true;
            this.rBxMonedaForm4.CheckedChanged += new System.EventHandler(this.rBxMonedaForm4_CheckedChanged);
            // 
            // rBxMonedaForm3
            // 
            resources.ApplyResources(this.rBxMonedaForm3, "rBxMonedaForm3");
            this.rBxMonedaForm3.Name = "rBxMonedaForm3";
            this.rBxMonedaForm3.TabStop = true;
            this.rBxMonedaForm3.UseVisualStyleBackColor = true;
            this.rBxMonedaForm3.CheckedChanged += new System.EventHandler(this.rBxMonedaForm3_CheckedChanged);
            // 
            // rBxMonedaForm2
            // 
            resources.ApplyResources(this.rBxMonedaForm2, "rBxMonedaForm2");
            this.rBxMonedaForm2.Name = "rBxMonedaForm2";
            this.rBxMonedaForm2.TabStop = true;
            this.rBxMonedaForm2.UseVisualStyleBackColor = true;
            this.rBxMonedaForm2.CheckedChanged += new System.EventHandler(this.rBxMonedaForm2_CheckedChanged);
            // 
            // rBxMonedaForm1
            // 
            resources.ApplyResources(this.rBxMonedaForm1, "rBxMonedaForm1");
            this.rBxMonedaForm1.Name = "rBxMonedaForm1";
            this.rBxMonedaForm1.TabStop = true;
            this.rBxMonedaForm1.UseVisualStyleBackColor = true;
            this.rBxMonedaForm1.CheckedChanged += new System.EventHandler(this.rBxMonedaForm1_CheckedChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.rBxUMForm2);
            this.groupBox6.Controls.Add(this.rBxUMForm1);
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // rBxUMForm2
            // 
            resources.ApplyResources(this.rBxUMForm2, "rBxUMForm2");
            this.rBxUMForm2.Name = "rBxUMForm2";
            this.rBxUMForm2.TabStop = true;
            this.rBxUMForm2.UseVisualStyleBackColor = true;
            this.rBxUMForm2.CheckedChanged += new System.EventHandler(this.rBxUMForm2_CheckedChanged);
            // 
            // rBxUMForm1
            // 
            resources.ApplyResources(this.rBxUMForm1, "rBxUMForm1");
            this.rBxUMForm1.Name = "rBxUMForm1";
            this.rBxUMForm1.TabStop = true;
            this.rBxUMForm1.UseVisualStyleBackColor = true;
            this.rBxUMForm1.CheckedChanged += new System.EventHandler(this.rBxUMForm1_CheckedChanged);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.rBxIdiomaForm2);
            this.groupBox7.Controls.Add(this.rBxIdiomaForm1);
            resources.ApplyResources(this.groupBox7, "groupBox7");
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.TabStop = false;
            // 
            // rBxIdiomaForm2
            // 
            resources.ApplyResources(this.rBxIdiomaForm2, "rBxIdiomaForm2");
            this.rBxIdiomaForm2.Name = "rBxIdiomaForm2";
            this.rBxIdiomaForm2.TabStop = true;
            this.rBxIdiomaForm2.UseVisualStyleBackColor = true;
            this.rBxIdiomaForm2.CheckedChanged += new System.EventHandler(this.rBxIdiomaForm2_CheckedChanged);
            // 
            // rBxIdiomaForm1
            // 
            resources.ApplyResources(this.rBxIdiomaForm1, "rBxIdiomaForm1");
            this.rBxIdiomaForm1.Name = "rBxIdiomaForm1";
            this.rBxIdiomaForm1.TabStop = true;
            this.rBxIdiomaForm1.UseVisualStyleBackColor = true;
            this.rBxIdiomaForm1.CheckedChanged += new System.EventHandler(this.rBxIdiomaForm1_CheckedChanged);
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "DatosGral";
            this.bindingSource1.DataSource = this.baseDeDatosDataSet;
            // 
            // baseDeDatosDataSet
            // 
            this.baseDeDatosDataSet.DataSetName = "BaseDeDatosDataSet";
            this.baseDeDatosDataSet.EnforceConstraints = false;
            this.baseDeDatosDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // datosGralTableAdapter
            // 
            this.datosGralTableAdapter.ClearBeforeFill = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox7);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.groupBox6);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // chkEnableAds
            // 
            this.chkEnableAds.Checked = true;
            this.chkEnableAds.CheckState = System.Windows.Forms.CheckState.Checked;
            resources.ApplyResources(this.chkEnableAds, "chkEnableAds");
            this.chkEnableAds.Name = "chkEnableAds";
            this.chkEnableAds.UseVisualStyleBackColor = true;
            this.chkEnableAds.CheckedChanged += new System.EventHandler(this.chkEnableAds_CheckedChanged);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.chkEnableAds);
            resources.ApplyResources(this.groupBox8, "groupBox8");
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.TabStop = false;
            // 
            // UserParametro
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.toolStrip51);
            this.Name = "UserParametro";
            this.Load += new System.EventHandler(this.UserParametro_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.toolStrip51.ResumeLayout(false);
            this.toolStrip51.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseDeDatosDataSet)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DateTimePicker tbxHorainicio;
        private System.Windows.Forms.DateTimePicker tbxHorafin;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbxFrecuencia;
        private System.Windows.Forms.Label label11;
        public System.Windows.Forms.TextBox tbxDireccion;
        public System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox tbxRazon;
        public System.Windows.Forms.Label label7;
        public System.Windows.Forms.Label label8;
        public System.Windows.Forms.TextBox tbxCiudad;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox DOMINGO;
        private System.Windows.Forms.CheckBox SABADO;
        private System.Windows.Forms.CheckBox VIERNES;
        private System.Windows.Forms.CheckBox JUEVES;
        private System.Windows.Forms.CheckBox MIERCOLES;
        private System.Windows.Forms.CheckBox MARTES;
        private System.Windows.Forms.CheckBox LUNES;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel tbxFecha;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnEditar;
        private System.Windows.Forms.ToolStripButton btnGuardar;
        private System.Windows.Forms.BindingSource bindingSource1;
        private BaseDeDatosDataSet baseDeDatosDataSet;
        private BaseDeDatosDataSetTableAdapters.DatosGralTableAdapter datosGralTableAdapter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnCerrar;
        public System.Windows.Forms.ToolStrip toolStrip51;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkEnvio;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rBxFechaForm2;
        private System.Windows.Forms.RadioButton rBxFechaForm1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rBxMonedaForm2;
        private System.Windows.Forms.RadioButton rBxMonedaForm1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.RadioButton rBxUMForm2;
        private System.Windows.Forms.RadioButton rBxUMForm1;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.RadioButton rBxIdiomaForm2;
        private System.Windows.Forms.RadioButton rBxIdiomaForm1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rBxMonedaForm3;
        private System.Windows.Forms.RadioButton rBxMonedaForm4;
        private System.Windows.Forms.CheckBox chkEnvioProgramado;
        private System.Windows.Forms.CheckBox chkEnableAds;
        private System.Windows.Forms.GroupBox groupBox8;
    }
}