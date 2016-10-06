namespace MainMenu
{
    partial class UserUsuarios
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserUsuarios));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treListadoU = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.tbxFind = new System.Windows.Forms.TextBox();
            this.pnldetalle = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.atributo20 = new System.Windows.Forms.CheckBox();
            this.atributo9 = new System.Windows.Forms.CheckBox();
            this.atributo6 = new System.Windows.Forms.CheckBox();
            this.atributo5 = new System.Windows.Forms.CheckBox();
            this.atributo4 = new System.Windows.Forms.CheckBox();
            this.atributo3 = new System.Windows.Forms.CheckBox();
            this.atributo23 = new System.Windows.Forms.CheckBox();
            this.atributo22 = new System.Windows.Forms.CheckBox();
            this.atributo21 = new System.Windows.Forms.CheckBox();
            this.atributo19 = new System.Windows.Forms.CheckBox();
            this.atributo18 = new System.Windows.Forms.CheckBox();
            this.atributo17 = new System.Windows.Forms.CheckBox();
            this.atributo15 = new System.Windows.Forms.CheckBox();
            this.atributo14 = new System.Windows.Forms.CheckBox();
            this.atributo13 = new System.Windows.Forms.CheckBox();
            this.atributo12 = new System.Windows.Forms.CheckBox();
            this.atributo11 = new System.Windows.Forms.CheckBox();
            this.atributo10 = new System.Windows.Forms.CheckBox();
            this.atributo8 = new System.Windows.Forms.CheckBox();
            this.atributo16 = new System.Windows.Forms.CheckBox();
            this.atributo7 = new System.Windows.Forms.CheckBox();
            this.atributo2 = new System.Windows.Forms.CheckBox();
            this.atributo1 = new System.Windows.Forms.CheckBox();
            this.atributo0 = new System.Windows.Forms.CheckBox();
            this.tbxUser = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.cbxTipo = new System.Windows.Forms.ComboBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbxPass = new System.Windows.Forms.TextBox();
            this.tbxDescripcion = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.toolStrip51 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnNuevo = new System.Windows.Forms.ToolStripButton();
            this.btnEditar = new System.Windows.Forms.ToolStripButton();
            this.btnGuardar = new System.Windows.Forms.ToolStripButton();
            this.btnBorrar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCerrar = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.baseDeDatosDataSet1 = new MainMenu.BaseDeDatosDataSet();
            this.usuariosTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.UsuariosTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnldetalle.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.toolStrip51.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseDeDatosDataSet1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treListadoU);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pnldetalle);
            this.splitContainer1.Panel2.Controls.Add(this.panel4);
            // 
            // treListadoU
            // 
            resources.ApplyResources(this.treListadoU, "treListadoU");
            this.treListadoU.Name = "treListadoU";
            this.treListadoU.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeListado_AfterSelect);
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
            // 
            // pnldetalle
            // 
            resources.ApplyResources(this.pnldetalle, "pnldetalle");
            this.pnldetalle.Controls.Add(this.groupBox1);
            this.pnldetalle.Controls.Add(this.toolStrip51);
            this.pnldetalle.Name = "pnldetalle";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.tbxUser);
            this.groupBox1.Controls.Add(this.Label2);
            this.groupBox1.Controls.Add(this.cbxTipo);
            this.groupBox1.Controls.Add(this.Label1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tbxPass);
            this.groupBox1.Controls.Add(this.tbxDescripcion);
            this.groupBox1.Controls.Add(this.label4);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.atributo20);
            this.groupBox3.Controls.Add(this.atributo9);
            this.groupBox3.Controls.Add(this.atributo6);
            this.groupBox3.Controls.Add(this.atributo5);
            this.groupBox3.Controls.Add(this.atributo4);
            this.groupBox3.Controls.Add(this.atributo3);
            this.groupBox3.Controls.Add(this.atributo23);
            this.groupBox3.Controls.Add(this.atributo22);
            this.groupBox3.Controls.Add(this.atributo21);
            this.groupBox3.Controls.Add(this.atributo19);
            this.groupBox3.Controls.Add(this.atributo18);
            this.groupBox3.Controls.Add(this.atributo17);
            this.groupBox3.Controls.Add(this.atributo15);
            this.groupBox3.Controls.Add(this.atributo14);
            this.groupBox3.Controls.Add(this.atributo13);
            this.groupBox3.Controls.Add(this.atributo12);
            this.groupBox3.Controls.Add(this.atributo11);
            this.groupBox3.Controls.Add(this.atributo10);
            this.groupBox3.Controls.Add(this.atributo8);
            this.groupBox3.Controls.Add(this.atributo16);
            this.groupBox3.Controls.Add(this.atributo7);
            this.groupBox3.Controls.Add(this.atributo2);
            this.groupBox3.Controls.Add(this.atributo1);
            this.groupBox3.Controls.Add(this.atributo0);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // atributo20
            // 
            resources.ApplyResources(this.atributo20, "atributo20");
            this.atributo20.BackColor = System.Drawing.Color.AliceBlue;
            this.atributo20.Cursor = System.Windows.Forms.Cursors.Default;
            this.atributo20.ForeColor = System.Drawing.SystemColors.ControlText;
            this.atributo20.Name = "atributo20";
            this.atributo20.UseVisualStyleBackColor = false;
            // 
            // atributo9
            // 
            resources.ApplyResources(this.atributo9, "atributo9");
            this.atributo9.BackColor = System.Drawing.Color.AliceBlue;
            this.atributo9.Cursor = System.Windows.Forms.Cursors.Default;
            this.atributo9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.atributo9.Name = "atributo9";
            this.atributo9.UseVisualStyleBackColor = false;
            // 
            // atributo6
            // 
            resources.ApplyResources(this.atributo6, "atributo6");
            this.atributo6.BackColor = System.Drawing.Color.AliceBlue;
            this.atributo6.Cursor = System.Windows.Forms.Cursors.Default;
            this.atributo6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.atributo6.Name = "atributo6";
            this.atributo6.UseVisualStyleBackColor = false;
            // 
            // atributo5
            // 
            resources.ApplyResources(this.atributo5, "atributo5");
            this.atributo5.BackColor = System.Drawing.Color.AliceBlue;
            this.atributo5.Cursor = System.Windows.Forms.Cursors.Default;
            this.atributo5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.atributo5.Name = "atributo5";
            this.atributo5.UseVisualStyleBackColor = false;
            // 
            // atributo4
            // 
            resources.ApplyResources(this.atributo4, "atributo4");
            this.atributo4.BackColor = System.Drawing.Color.AliceBlue;
            this.atributo4.Cursor = System.Windows.Forms.Cursors.Default;
            this.atributo4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.atributo4.Name = "atributo4";
            this.atributo4.UseVisualStyleBackColor = false;
            // 
            // atributo3
            // 
            resources.ApplyResources(this.atributo3, "atributo3");
            this.atributo3.BackColor = System.Drawing.Color.AliceBlue;
            this.atributo3.Cursor = System.Windows.Forms.Cursors.Default;
            this.atributo3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.atributo3.Name = "atributo3";
            this.atributo3.UseVisualStyleBackColor = false;
            // 
            // atributo23
            // 
            resources.ApplyResources(this.atributo23, "atributo23");
            this.atributo23.BackColor = System.Drawing.Color.AliceBlue;
            this.atributo23.Cursor = System.Windows.Forms.Cursors.Default;
            this.atributo23.ForeColor = System.Drawing.SystemColors.ControlText;
            this.atributo23.Name = "atributo23";
            this.atributo23.UseVisualStyleBackColor = false;
            // 
            // atributo22
            // 
            resources.ApplyResources(this.atributo22, "atributo22");
            this.atributo22.BackColor = System.Drawing.Color.AliceBlue;
            this.atributo22.Cursor = System.Windows.Forms.Cursors.Default;
            this.atributo22.ForeColor = System.Drawing.SystemColors.ControlText;
            this.atributo22.Name = "atributo22";
            this.atributo22.UseVisualStyleBackColor = false;
            // 
            // atributo21
            // 
            resources.ApplyResources(this.atributo21, "atributo21");
            this.atributo21.BackColor = System.Drawing.Color.AliceBlue;
            this.atributo21.Cursor = System.Windows.Forms.Cursors.Default;
            this.atributo21.ForeColor = System.Drawing.SystemColors.ControlText;
            this.atributo21.Name = "atributo21";
            this.atributo21.UseVisualStyleBackColor = false;
            // 
            // atributo19
            // 
            resources.ApplyResources(this.atributo19, "atributo19");
            this.atributo19.BackColor = System.Drawing.Color.AliceBlue;
            this.atributo19.Cursor = System.Windows.Forms.Cursors.Default;
            this.atributo19.ForeColor = System.Drawing.SystemColors.ControlText;
            this.atributo19.Name = "atributo19";
            this.atributo19.UseVisualStyleBackColor = false;
            // 
            // atributo18
            // 
            resources.ApplyResources(this.atributo18, "atributo18");
            this.atributo18.BackColor = System.Drawing.Color.AliceBlue;
            this.atributo18.Cursor = System.Windows.Forms.Cursors.Default;
            this.atributo18.ForeColor = System.Drawing.SystemColors.ControlText;
            this.atributo18.Name = "atributo18";
            this.atributo18.UseVisualStyleBackColor = false;
            // 
            // atributo17
            // 
            resources.ApplyResources(this.atributo17, "atributo17");
            this.atributo17.BackColor = System.Drawing.Color.AliceBlue;
            this.atributo17.Cursor = System.Windows.Forms.Cursors.Default;
            this.atributo17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.atributo17.Name = "atributo17";
            this.atributo17.UseVisualStyleBackColor = false;
            // 
            // atributo15
            // 
            resources.ApplyResources(this.atributo15, "atributo15");
            this.atributo15.BackColor = System.Drawing.Color.AliceBlue;
            this.atributo15.Cursor = System.Windows.Forms.Cursors.Default;
            this.atributo15.ForeColor = System.Drawing.SystemColors.ControlText;
            this.atributo15.Name = "atributo15";
            this.atributo15.UseVisualStyleBackColor = false;
            // 
            // atributo14
            // 
            resources.ApplyResources(this.atributo14, "atributo14");
            this.atributo14.BackColor = System.Drawing.Color.AliceBlue;
            this.atributo14.Cursor = System.Windows.Forms.Cursors.Default;
            this.atributo14.ForeColor = System.Drawing.SystemColors.ControlText;
            this.atributo14.Name = "atributo14";
            this.atributo14.UseVisualStyleBackColor = false;
            // 
            // atributo13
            // 
            resources.ApplyResources(this.atributo13, "atributo13");
            this.atributo13.BackColor = System.Drawing.Color.AliceBlue;
            this.atributo13.Cursor = System.Windows.Forms.Cursors.Default;
            this.atributo13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.atributo13.Name = "atributo13";
            this.atributo13.UseVisualStyleBackColor = false;
            // 
            // atributo12
            // 
            resources.ApplyResources(this.atributo12, "atributo12");
            this.atributo12.BackColor = System.Drawing.Color.AliceBlue;
            this.atributo12.Cursor = System.Windows.Forms.Cursors.Default;
            this.atributo12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.atributo12.Name = "atributo12";
            this.atributo12.UseVisualStyleBackColor = false;
            // 
            // atributo11
            // 
            resources.ApplyResources(this.atributo11, "atributo11");
            this.atributo11.BackColor = System.Drawing.Color.AliceBlue;
            this.atributo11.Cursor = System.Windows.Forms.Cursors.Default;
            this.atributo11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.atributo11.Name = "atributo11";
            this.atributo11.UseVisualStyleBackColor = false;
            // 
            // atributo10
            // 
            resources.ApplyResources(this.atributo10, "atributo10");
            this.atributo10.BackColor = System.Drawing.Color.AliceBlue;
            this.atributo10.Cursor = System.Windows.Forms.Cursors.Default;
            this.atributo10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.atributo10.Name = "atributo10";
            this.atributo10.UseVisualStyleBackColor = false;
            // 
            // atributo8
            // 
            resources.ApplyResources(this.atributo8, "atributo8");
            this.atributo8.BackColor = System.Drawing.Color.AliceBlue;
            this.atributo8.Cursor = System.Windows.Forms.Cursors.Default;
            this.atributo8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.atributo8.Name = "atributo8";
            this.atributo8.UseVisualStyleBackColor = false;
            // 
            // atributo16
            // 
            resources.ApplyResources(this.atributo16, "atributo16");
            this.atributo16.BackColor = System.Drawing.Color.AliceBlue;
            this.atributo16.Cursor = System.Windows.Forms.Cursors.Default;
            this.atributo16.ForeColor = System.Drawing.SystemColors.ControlText;
            this.atributo16.Name = "atributo16";
            this.atributo16.UseVisualStyleBackColor = false;
            // 
            // atributo7
            // 
            resources.ApplyResources(this.atributo7, "atributo7");
            this.atributo7.BackColor = System.Drawing.Color.AliceBlue;
            this.atributo7.Cursor = System.Windows.Forms.Cursors.Default;
            this.atributo7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.atributo7.Name = "atributo7";
            this.atributo7.UseVisualStyleBackColor = false;
            // 
            // atributo2
            // 
            resources.ApplyResources(this.atributo2, "atributo2");
            this.atributo2.BackColor = System.Drawing.Color.AliceBlue;
            this.atributo2.Cursor = System.Windows.Forms.Cursors.Default;
            this.atributo2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.atributo2.Name = "atributo2";
            this.atributo2.UseVisualStyleBackColor = false;
            // 
            // atributo1
            // 
            resources.ApplyResources(this.atributo1, "atributo1");
            this.atributo1.BackColor = System.Drawing.Color.AliceBlue;
            this.atributo1.Cursor = System.Windows.Forms.Cursors.Default;
            this.atributo1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.atributo1.Name = "atributo1";
            this.atributo1.UseVisualStyleBackColor = false;
            // 
            // atributo0
            // 
            resources.ApplyResources(this.atributo0, "atributo0");
            this.atributo0.BackColor = System.Drawing.Color.AliceBlue;
            this.atributo0.Cursor = System.Windows.Forms.Cursors.Default;
            this.atributo0.ForeColor = System.Drawing.SystemColors.ControlText;
            this.atributo0.Name = "atributo0";
            this.atributo0.UseVisualStyleBackColor = false;
            // 
            // tbxUser
            // 
            resources.ApplyResources(this.tbxUser, "tbxUser");
            this.tbxUser.Name = "tbxUser";
            this.tbxUser.Enter += new System.EventHandler(this.tbxUser_Enter);
            this.tbxUser.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxUser_KeyDown);
            this.tbxUser.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxUser_KeyPress);
            this.tbxUser.Leave += new System.EventHandler(this.tbxUser_Leave);
            // 
            // Label2
            // 
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.Label2, "Label2");
            this.Label2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label2.Name = "Label2";
            // 
            // cbxTipo
            // 
            this.cbxTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cbxTipo, "cbxTipo");
            this.cbxTipo.FormattingEnabled = true;
            this.cbxTipo.Items.AddRange(new object[] {
            resources.GetString("cbxTipo.Items"),
            resources.GetString("cbxTipo.Items1")});
            this.cbxTipo.Name = "cbxTipo";
            this.cbxTipo.SelectedIndexChanged += new System.EventHandler(this.cbxTipo_SelectedIndexChanged);
            this.cbxTipo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbxTipo_KeyDown);
            // 
            // Label1
            // 
            this.Label1.BackColor = System.Drawing.Color.Transparent;
            this.Label1.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.Label1, "Label1");
            this.Label1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Label1.Name = "Label1";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // tbxPass
            // 
            resources.ApplyResources(this.tbxPass, "tbxPass");
            this.tbxPass.Name = "tbxPass";
            this.tbxPass.Enter += new System.EventHandler(this.tbxPass_Enter);
            this.tbxPass.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxPass_KeyDown);
            this.tbxPass.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxPass_KeyPress);
            this.tbxPass.Leave += new System.EventHandler(this.tbxPass_Leave);
            // 
            // tbxDescripcion
            // 
            resources.ApplyResources(this.tbxDescripcion, "tbxDescripcion");
            this.tbxDescripcion.Name = "tbxDescripcion";
            this.tbxDescripcion.Enter += new System.EventHandler(this.tbxDescripcion_Enter);
            this.tbxDescripcion.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxDescripcion_KeyDown);
            this.tbxDescripcion.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxDescripcion_KeyPress);
            this.tbxDescripcion.Leave += new System.EventHandler(this.tbxDescripcion_Leave);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Cursor = System.Windows.Forms.Cursors.Default;
            this.label4.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label4.Name = "label4";
            // 
            // toolStrip51
            // 
            this.toolStrip51.BackColor = System.Drawing.Color.LightSteelBlue;
            resources.ApplyResources(this.toolStrip51, "toolStrip51");
            this.toolStrip51.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip51.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip51.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.btnNuevo,
            this.btnEditar,
            this.btnGuardar,
            this.btnBorrar,
            this.toolStripSeparator2,
            this.btnCerrar,
            this.toolStripLabel1,
            this.toolStripLabel2});
            this.toolStrip51.Name = "toolStrip51";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // btnNuevo
            // 
            this.btnNuevo.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btnNuevo.Image = global::MainMenu.Properties.Resources.nuevo;
            resources.ApplyResources(this.btnNuevo, "btnNuevo");
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
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
            // btnBorrar
            // 
            this.btnBorrar.ForeColor = System.Drawing.Color.MidnightBlue;
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
            this.btnCerrar.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btnCerrar.Image = global::MainMenu.Properties.Resources.cancelar;
            resources.ApplyResources(this.btnCerrar, "btnCerrar");
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripLabel1.Name = "toolStripLabel1";
            resources.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
            // 
            // toolStripLabel2
            // 
            resources.ApplyResources(this.toolStripLabel2, "toolStripLabel2");
            this.toolStripLabel2.ForeColor = System.Drawing.Color.Blue;
            this.toolStripLabel2.Name = "toolStripLabel2";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.LightSteelBlue;
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "Usuarios";
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
            // UserUsuarios
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.splitContainer1);
            this.Name = "UserUsuarios";
            this.Load += new System.EventHandler(this.UserUsuarios_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnldetalle.ResumeLayout(false);
            this.pnldetalle.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.toolStrip51.ResumeLayout(false);
            this.toolStrip51.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseDeDatosDataSet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbxPass;
        private System.Windows.Forms.TextBox tbxUser;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Label Label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbxTipo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbxDescripcion;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.BindingSource bindingSource1;
        private BaseDeDatosDataSet baseDeDatosDataSet1;
        private System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.CheckBox atributo20;
        public System.Windows.Forms.CheckBox atributo9;
        public System.Windows.Forms.CheckBox atributo6;
        public System.Windows.Forms.CheckBox atributo5;
        public System.Windows.Forms.CheckBox atributo4;
        public System.Windows.Forms.CheckBox atributo3;
        public System.Windows.Forms.CheckBox atributo23;
        public System.Windows.Forms.CheckBox atributo22;
        public System.Windows.Forms.CheckBox atributo21;
        public System.Windows.Forms.CheckBox atributo19;
        public System.Windows.Forms.CheckBox atributo18;
        public System.Windows.Forms.CheckBox atributo17;
        public System.Windows.Forms.CheckBox atributo15;
        public System.Windows.Forms.CheckBox atributo14;
        public System.Windows.Forms.CheckBox atributo13;
        public System.Windows.Forms.CheckBox atributo12;
        public System.Windows.Forms.CheckBox atributo11;
        public System.Windows.Forms.CheckBox atributo10;
        public System.Windows.Forms.CheckBox atributo8;
        public System.Windows.Forms.CheckBox atributo16;
        public System.Windows.Forms.CheckBox atributo7;
        public System.Windows.Forms.CheckBox atributo2;
        public System.Windows.Forms.CheckBox atributo1;
        public System.Windows.Forms.CheckBox atributo0;
        private System.Windows.Forms.Label label3;
        private BaseDeDatosDataSetTableAdapters.UsuariosTableAdapter usuariosTableAdapter;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnNuevo;
        private System.Windows.Forms.ToolStripButton btnEditar;
        private System.Windows.Forms.ToolStripButton btnGuardar;
        private System.Windows.Forms.ToolStripButton btnBorrar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnCerrar;
        public System.Windows.Forms.ToolStrip toolStrip51;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbxFind;
        private System.Windows.Forms.TreeView treListadoU;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel pnldetalle;



    }
}