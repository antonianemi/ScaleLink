namespace MainMenu
{
    partial class UserGrupos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserGrupos));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.tbxDireccion = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxNombre = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbxGrupo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxDescripcion = new System.Windows.Forms.TextBox();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.baseDeDatosDataSet = new MainMenu.BaseDeDatosDataSet();
            this.grupoTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.GrupoTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseDeDatosDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Document Text.png");
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Name = "label4";
            // 
            // tbxDireccion
            // 
            resources.ApplyResources(this.tbxDireccion, "tbxDireccion");
            this.tbxDireccion.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tbxDireccion.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbxDireccion.Name = "tbxDireccion";
            this.tbxDireccion.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxDireccion_KeyDown);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Name = "label3";
            // 
            // tbxNombre
            // 
            resources.ApplyResources(this.tbxNombre, "tbxNombre");
            this.tbxNombre.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tbxNombre.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbxNombre.Name = "tbxNombre";
            this.tbxNombre.Enter += new System.EventHandler(this.tbxNombre_Enter);
            this.tbxNombre.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxNombre_KeyDown_1);
            this.tbxNombre.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxNombre_KeyPress);
            this.tbxNombre.Leave += new System.EventHandler(this.tbxNombre_Leave);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Name = "label1";
            // 
            // tbxGrupo
            // 
            resources.ApplyResources(this.tbxGrupo, "tbxGrupo");
            this.tbxGrupo.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tbxGrupo.Name = "tbxGrupo";
            this.tbxGrupo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxGrupo_KeyDown);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Name = "label2";
            // 
            // tbxDescripcion
            // 
            resources.ApplyResources(this.tbxDescripcion, "tbxDescripcion");
            this.tbxDescripcion.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tbxDescripcion.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbxDescripcion.Name = "tbxDescripcion";
            this.tbxDescripcion.Enter += new System.EventHandler(this.tbxDescrip_Enter);
            this.tbxDescripcion.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxDescrip_KeyPress);
            this.tbxDescripcion.Leave += new System.EventHandler(this.tbxDescrip_Leave);
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "Grupo";
            this.bindingSource1.DataSource = this.baseDeDatosDataSet;
            // 
            // baseDeDatosDataSet
            // 
            this.baseDeDatosDataSet.DataSetName = "BaseDeDatosDataSet";
            this.baseDeDatosDataSet.EnforceConstraints = false;
            this.baseDeDatosDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // grupoTableAdapter
            // 
            this.grupoTableAdapter.ClearBeforeFill = true;
            // 
            // UserGrupos
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbxDireccion);
            this.Controls.Add(this.tbxGrupo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbxDescripcion);
            this.Controls.Add(this.tbxNombre);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "UserGrupos";
            this.Load += new System.EventHandler(this.UserGrupos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseDeDatosDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxGrupo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxDescripcion;
        private System.Windows.Forms.BindingSource bindingSource1;
        private BaseDeDatosDataSet baseDeDatosDataSet;
        private BaseDeDatosDataSetTableAdapters.GrupoTableAdapter grupoTableAdapter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbxDireccion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxNombre;
        private System.Windows.Forms.ImageList imageList1;
    }
}