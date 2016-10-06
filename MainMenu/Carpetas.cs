using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace MainMenu
{
    /// <summary>
    /// Descripción breve de carpetas.
    /// </summary>
    public class Carpetas : System.Windows.Forms.Form
	{
        private Label label1;
        private Label label3;
        private Button aceptar;
        private Button cancelar;	
        private ComboBox cbxFolder;
        private TextBox tbxNombre;
        private TextBox tbxID;
        private TextBox tbxPadre;
        private GroupBox groupBox1;
        private Label label5;
        private int parent;
		public static string nomfold;
		public static int numfold;
        public static int numparent;
		public static int numsubfold;
        public static int opcion;
        public static int nbascula;
        public static int ngrupo;
        public static int iAction;  //0-> Crear Carpeta, 1->Editar Carpeta
		private int n_reg;

        Color colorbefore;

        ADOutil Conec = new ADOutil();

        BaseDeDatosDataSet basededatosDataset = new BaseDeDatosDataSet();
        BaseDeDatosDataSetTableAdapters.Prod_detalleTableAdapter prod_detalleTableAdapter = new BaseDeDatosDataSetTableAdapters.Prod_detalleTableAdapter();
        BaseDeDatosDataSetTableAdapters.carpeta_detalleTableAdapter carpeta_DetalleTableAdapter = new BaseDeDatosDataSetTableAdapters.carpeta_detalleTableAdapter();
        //carpeta_detalleTableAdapter carpeta_detalleTableAdapter = new carpeta_detalleTableAdapter();
        private Button btnExam;
        private TextBox tbxImagen;
        private Label label2;
        private OpenFileDialog openFileDialog1;
        private Panel panel2;
        private Panel panel3;
        private Label lbxTit;
        
		/// <summary>
		/// Variable del diseñador requerida.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Carpetas()
		{
			InitializeComponent();
            this.CancelButton = cancelar;
		}

        public Carpetas(Point position)
        {
            InitializeComponent();
            this.CancelButton = cancelar;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(position.X, position.Y);
        }

		/// <summary>
		/// Limpiar los recursos que se estén utilizando.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Método necesario para admitir el Diseñador, no se puede modificar
		/// el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Carpetas));
            this.label1 = new System.Windows.Forms.Label();
            this.aceptar = new System.Windows.Forms.Button();
            this.cancelar = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cbxFolder = new System.Windows.Forms.ComboBox();
            this.tbxID = new System.Windows.Forms.TextBox();
            this.tbxNombre = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnExam = new System.Windows.Forms.Button();
            this.tbxImagen = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxPadre = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lbxTit = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // aceptar
            // 
            this.aceptar.BackColor = System.Drawing.Color.Teal;
            resources.ApplyResources(this.aceptar, "aceptar");
            this.aceptar.ForeColor = System.Drawing.Color.White;
            this.aceptar.Name = "aceptar";
            this.aceptar.UseVisualStyleBackColor = false;
            this.aceptar.Click += new System.EventHandler(this.aceptar_Click);
            // 
            // cancelar
            // 
            this.cancelar.BackColor = System.Drawing.Color.Teal;
            resources.ApplyResources(this.cancelar, "cancelar");
            this.cancelar.ForeColor = System.Drawing.Color.White;
            this.cancelar.Name = "cancelar";
            this.cancelar.UseVisualStyleBackColor = false;
            this.cancelar.Click += new System.EventHandler(this.cancelar_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cbxFolder
            // 
            this.cbxFolder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cbxFolder, "cbxFolder");
            this.cbxFolder.Name = "cbxFolder";
            this.cbxFolder.SelectedIndexChanged += new System.EventHandler(this.cbxFolder_SelectedIndexChanged_1);
            this.cbxFolder.Enter += new System.EventHandler(this.cbxFolder_Enter);
            this.cbxFolder.Leave += new System.EventHandler(this.cbxFolder_Leave);
            // 
            // tbxID
            // 
            resources.ApplyResources(this.tbxID, "tbxID");
            this.tbxID.Name = "tbxID";
            // 
            // tbxNombre
            // 
            resources.ApplyResources(this.tbxNombre, "tbxNombre");
            this.tbxNombre.Name = "tbxNombre";
            this.tbxNombre.Enter += new System.EventHandler(this.tbxNombre_Enter);
            this.tbxNombre.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxNombre_KeyDown);
            this.tbxNombre.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxNombre_KeyPress);
            this.tbxNombre.Leave += new System.EventHandler(this.tbxNombre_Leave);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnExam);
            this.groupBox1.Controls.Add(this.tbxImagen);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbxPadre);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tbxNombre);
            this.groupBox1.Controls.Add(this.cbxFolder);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbxID);
            this.groupBox1.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // btnExam
            // 
            this.btnExam.BackColor = System.Drawing.Color.Teal;
            resources.ApplyResources(this.btnExam, "btnExam");
            this.btnExam.ForeColor = System.Drawing.Color.White;
            this.btnExam.Name = "btnExam";
            this.btnExam.UseVisualStyleBackColor = false;
            this.btnExam.Click += new System.EventHandler(this.btnExam_Click);
            // 
            // tbxImagen
            // 
            resources.ApplyResources(this.tbxImagen, "tbxImagen");
            this.tbxImagen.Name = "tbxImagen";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tbxPadre
            // 
            resources.ApplyResources(this.tbxPadre, "tbxPadre");
            this.tbxPadre.Name = "tbxPadre";
            this.tbxPadre.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxPadre_KeyDown);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.AliceBlue;
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.cancelar);
            this.panel2.Controls.Add(this.aceptar);
            this.panel2.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panel3.BackgroundImage = global::MainMenu.Properties.Resources.fondo2;
            this.panel3.Controls.Add(this.lbxTit);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
            // 
            // lbxTit
            // 
            resources.ApplyResources(this.lbxTit, "lbxTit");
            this.lbxTit.BackColor = System.Drawing.Color.Transparent;
            this.lbxTit.ForeColor = System.Drawing.Color.Black;
            this.lbxTit.Name = "lbxTit";
            // 
            // Carpetas
            // 
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Carpetas";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Activated += new System.EventHandler(this.Carpetas_Activated);
            this.Load += new System.EventHandler(this.Carpetas_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		public bool Buscar_Folder(string numfold)
		{			
			bool existe = false;
			OleDbDataReader BF = Conec.Obtiene_Dato("SELECT ID,Nombre FROM carpeta_detalle WHERE (ID = " + Convert.ToInt32(numfold) + " and id_bascula = " + nbascula + " and id_grupo = " + ngrupo  + ")",Conec.CadenaConexion);
			if (BF.Read())
			{				
				Carpetas.numfold = BF.GetInt32(0);
				Carpetas.nomfold = BF.GetString(1);
				existe = true;
			}
			BF.Close();
			return existe;
		}

        public bool Buscar_Nombre_Folder(string nomfold,string numfold)
        {
            bool existe = false;
            OleDbDataReader BF = Conec.Obtiene_Dato("SELECT ID,Nombre FROM carpeta_detalle WHERE (Nombre = '" + nomfold +"' and ID_padre = " + Convert.ToInt32(numfold) + " and id_bascula = " + nbascula + " and id_grupo = " + ngrupo + ")", Conec.CadenaConexion);
            if (BF.Read())
            {
                Carpetas.numfold = BF.GetInt32(0);
                Carpetas.nomfold = BF.GetString(1);               
                existe = true;
            }
            BF.Close();
            return existe;
        }
        public bool Buscar_Nombre_Folder(string nomfold, int numparent)
        {
            bool existe = false;
            OleDbDataReader BF = Conec.Obtiene_Dato("SELECT ID,Nombre FROM carpeta_detalle WHERE (Nombre = '" + nomfold + "' and ID = " + Convert.ToInt32(numparent) + " and id_bascula = " + nbascula + " and id_grupo = " + ngrupo + ")", Conec.CadenaConexion);
            if (BF.Read())
            {
                //Carpetas.numfold = BF.GetInt32(0);
                //Carpetas.nomfold = BF.GetString(1);
                existe = true;
            }
            BF.Close();
            return existe;
        }
        public bool Buscar_Nombre_Folder(string nomfold, int numfold, int numparent)
        {
            bool existe = false;
            OleDbDataReader BF = Conec.Obtiene_Dato("SELECT ID,Nombre FROM carpeta_detalle WHERE (Nombre = '" + nomfold + "' and ID_padre = " + numparent + " and id_bascula = " + nbascula + " and id_grupo = " + ngrupo + "and  ID <> " + numfold + ")", Conec.CadenaConexion);
            if (BF.Read())
            {
                existe = true;
            }
            BF.Close();
            return existe;
        }



		public bool Buscar_Articulo(int id_carpeta, int id_bascula, int id_grupo)
		{			
			OleDbDataReader BA = Conec.Obtiene_Dato("SELECT * FROM Prod_detalle WHERE (id_carpeta = " + id_carpeta + "and id_bascula = " + id_bascula + "and id_grupo = " + id_grupo + ")",Conec.CadenaConexion);
			bool existe = false;
            if (BA.Read()) { existe = true; }
			BA.Close();
			return existe;
		}	

		public bool Buscar_SubFolder(string numfold)
		{			
			OleDbDataReader BS = Conec.Obtiene_Dato("SELECT ID,Nombre FROM carpeta_detalle WHERE (ID_padre = " + Convert.ToInt32(numfold) + "and id_bascula = " + nbascula + " and id_grupo = " + ngrupo  + ")",Conec.CadenaConexion);
			bool existe = false;
            if (BS.Read()) { existe = true; }
			BS.Close();
			return existe;
		}

        public int Buscar_Numeros_Folders(string numfold)
        {
            carpeta_DetalleTableAdapter.Fill(basededatosDataset.carpeta_detalle);
            DataRow[] DR_FOLDER = basededatosDataset.carpeta_detalle.Select("ID_padre = " + Convert.ToInt32(numfold) + "and id_bascula = " + nbascula + " and id_grupo = " + ngrupo);
            int num_folder = 0;
            if (DR_FOLDER != null)
            {
                num_folder = DR_FOLDER.Length;
            }
            
            return num_folder;
        }

        public string muestraAutoincrementoId()
        {
            int cod = 0;

            OleDbDataReader LC = Conec.Obtiene_Dato("SELECT ID FROM carpeta_detalle WHERE id_bascula = " + nbascula + " and id_grupo = " + ngrupo  + " ORDER BY ID desc", Conec.CadenaConexion);
            if (LC.Read()) cod = Convert.ToInt32(LC.GetValue(0));
            LC.Close();
            return Convert.ToString(cod + 1);
        }

		private void Nuevo_Folder(ref int numfold, string nomfold, int parent, string archivo)
		{	
			try
			{
                if (parent >= 0 && n_reg > 0)
                {
                    Conec.CadenaSelect = "INSERT INTO carpeta_detalle (id_bascula, id_grupo, ID,ID_padre,Nombre,Imagen,tabla) VALUES (" + 
                        nbascula + "," + 
                        ngrupo + "," + 
                        numfold + "," + 
                        Convert.ToInt32(parent) + ",'" + 
                        nomfold + "','" + 
                        archivo + "','C'"+ ")";
                }
                else
                {
                    Conec.CadenaSelect = "INSERT INTO carpeta_detalle (id_grupo, id_bascula,ID,ID_padre,Nombre,Imagen) VALUES (" +
                    nbascula + "," +
                    ngrupo + ",0,null,'" +
                    nomfold + "','" +
                    archivo + "')";
                }

                if (nomfold != "")
                {
                    Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, "carpeta_detalle");                   
                }
			}
			catch (Exception ex)
			{
                MessageBox.Show(this, ex.Message, Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error);              
			}
		}
        public bool Nuevo_Folder(int numfold, string nomfold, int parent,int npos)
        {
            try
            {
                Conec.CadenaSelect = "INSERT INTO carpeta_detalle (id_bascula,id_grupo,ID,ID_padre,Nombre,tabla,posicion) VALUES (" + 
                    nbascula +"," + 
                    ngrupo + "," + 
                    numfold + "," + 
                    Convert.ToInt32(parent) + ",'" + 
                    nomfold + "','C'," +
                    npos + ")";

                if (nomfold != "")
                {
                    Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, "carpeta_detalle");                    
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void Actualiza_Folder(int numfold, string nomfold, string archivo)
        {
            try
            {
                Conec.CadenaSelect = "UPDATE carpeta_detalle SET " +
                    "Nombre = '" + nomfold + "'," +
                    "Imagen = '" + archivo + "'" +
                    " WHERE ( ID = " + numfold + " and id_bascula = " + nbascula + " and id_grupo = " + ngrupo  + ")";

                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "carpeta_detalle");                
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

		public bool Del_Folder(string nomfold,int numfold)
		{
			try
			{					
				string sele = "DELETE * FROM carpeta_detalle WHERE (ID = " + numfold + " and id_bascula = " + nbascula + " and id_grupo = " + ngrupo  + ")";
				Conec.EliminarReader(Conec.CadenaConexion,sele,"carpeta_detalle");
				return true;
			}
			catch (Exception ex)
			{
				string str=ex.Message;
                MessageBox.Show(this, str, Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		
		}
		private void aceptar_Click(object sender, System.EventArgs e)
		{
            int iError = 0;

			try
			{
                if (Buscar_Numeros_Folders(this.tbxPadre.Text) < 500)
                {
                    if (this.tbxNombre.Text == "")
                    {
                        MessageBox.Show(this, Variable.SYS_MSJ[77, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.tbxNombre.Focus();
                        iError = 1;
                    }
                    else
                    {
                        if (iAction == 0)
                        {
                            //Crea Carpeta
                            if (!Buscar_Folder(this.tbxID.Text) && !Buscar_Nombre_Folder(this.tbxNombre.Text, this.tbxPadre.Text))
                            {
                                if (!Buscar_Nombre_Folder(this.tbxNombre.Text, Convert.ToInt32(this.tbxPadre.Text)))
                                {
                                    Nuevo_Folder(ref numfold, this.tbxNombre.Text, Convert.ToInt32(this.tbxPadre.Text), this.tbxImagen.Text);
                                }
                                else
                                {
                                    MessageBox.Show(this, Variable.SYS_MSJ[277, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    this.tbxNombre.Focus();
                                    iError = 1;
                                }
                            }
                            else
                            {
                                MessageBox.Show(this, Variable.SYS_MSJ[277, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                this.tbxNombre.Focus();
                                iError = 1;
                            }
                        }
                        else
                        {
                            //Edita Carpeta
                            if (!Buscar_Nombre_Folder(this.tbxNombre.Text, numfold, numparent))
                            {
                                Actualiza_Folder(numfold, this.tbxNombre.Text, this.tbxImagen.Text);
                            }
                            else
                            {
                                MessageBox.Show(this, Variable.SYS_MSJ[277, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                this.tbxNombre.Focus();
                                iError = 1;
                            }
                        }

                        if (iError == 0)
                        {
                            numfold = Convert.ToInt32(this.tbxID.Text);
                            nomfold = this.tbxNombre.Text;
                            parent = Convert.ToInt32(this.tbxPadre.Text);
                            this.cbxFolder.SelectedValue = parent;
                            this.Close();
                            this.Dispose();
                        }
                    }
                }
                else MessageBox.Show(this, Variable.SYS_MSJ[279, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma]);
			}
			catch(Exception ex)
			{
                MessageBox.Show(this, ex.Message, Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error);
			}						
		}

		private void cancelar_Click(object sender, System.EventArgs e)
		{
			this.Close();
			this.Dispose();
		}

		private void cbxFolder_SelectedIndexChanged(object sender, System.EventArgs e)
		{			
			this.tbxPadre.Text = this.cbxFolder.SelectedValue.ToString();
			parent = Convert.ToInt32(this.tbxPadre.Text);
            this.btnExam.Focus();
		}

		private void txt_padre_TextChanged(object sender, System.EventArgs e)
		{
			if (this.tbxPadre.Text == "" ) this.tbxPadre.Text = "0";
		}

        private void Carpetas_Load(object sender, EventArgs e)
        {
            Conec.CadenaSelect2 = "SELECT ID, Nombre FROM carpeta_detalle WHERE id_bascula = " + nbascula + " and id_grupo = " + ngrupo + " ORDER BY ID";
            OleDbDataAdapter myDataAdapter4 = new OleDbDataAdapter(Conec.CadenaSelect2, Conec.CadenaConexion);
            DataSet ds_carpeta = new DataSet();
            
            myDataAdapter4.Fill(ds_carpeta, "carpeta_detalle");
          
            this.cbxFolder.DisplayMember = "Nombre";
            this.cbxFolder.ValueMember = "ID";
            this.cbxFolder.DataSource =ds_carpeta.Tables["carpeta_detalle"];
            this.cbxFolder.SelectedValue = Carpetas.numparent;

            this.n_reg = ds_carpeta.Tables["carpeta_detalle"].Rows.Count;
           
            if (Carpetas.opcion == 0) //CREACION
            {
                this.tbxID.Text = muestraAutoincrementoId();
                this.tbxNombre.Text = "";
                this.tbxPadre.Text = Carpetas.numparent.ToString(); // "0";
                Carpetas.numfold = Convert.ToInt32(this.tbxID.Text);
                this.cbxFolder.SelectedValue = Carpetas.numparent;
                //this.tbxPos.Text = "0";
                this.tbxImagen.Text = "";                
            }
            if (Carpetas.opcion == 1)  //MODIFICACION
            {
                carpeta_DetalleTableAdapter.Fill(basededatosDataset.carpeta_detalle);
                DataRow[] DA = basededatosDataset.carpeta_detalle.Select("id_bascula = " + Carpetas.nbascula + " and id_grupo = " + Carpetas.ngrupo + " and ID = " + Carpetas.numfold);
                
                if (DA.Length == 1)
                {
                    this.tbxNombre.Text = DA[0]["Nombre"].ToString();
                    this.tbxID.Text = Carpetas.numfold.ToString();
                    this.tbxPadre.Text = DA[0]["ID_padre"].ToString();
                    this.cbxFolder.SelectedValue = Convert.ToInt32(this.tbxPadre.Text);
                    this.tbxImagen.Text = DA[0]["imagen"].ToString();
                }


                /*OleDbDataReader dc = Conec.Obtiene_Dato("Select ID, ID_padre, Nombre, imagen FROM carpeta_detalle WHERE id_bascula = " + Carpetas.nbascula + " and id_grupo = " + Carpetas.ngrupo + " and ID = " + Carpetas.numfold, Conec.CadenaConexion);
                if (dc.Read())
                {
                    this.tbxNombre.Text = dc.GetString(2);
                    this.tbxID.Text = Carpetas.numfold.ToString();
                    this.tbxPadre.Text = dc.GetInt32(1).ToString();
                    this.cbxFolder.SelectedValue = Convert.ToInt32(this.tbxPadre.Text);
                    this.tbxImagen.Text = dc.GetString(3);
                }
                dc.Close();*/
            }            
        }

        private void Carpetas_Activated(object sender, EventArgs e)
        {
            this.tbxNombre.Focus();         
        }       

        private void btnExam_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.InitialDirectory = Variable.appPath + "\\images\\";           
            this.openFileDialog1.DefaultExt = "JPG";
            this.openFileDialog1.Filter = "Image Files(*.JPG)|*.JPG|All files (*.*)|*.*";
            DialogResult result = this.openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                tbxImagen.Text = this.openFileDialog1.FileName;               
                opcion = 2;
                if (!File.Exists(tbxImagen.Text))
                {
                    tbxImagen.Text = "";
                }
            }
        }

        private void tbxNombre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) cbxFolder.Focus();
        }

        private void tbxPadre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) cbxFolder.Focus();
        }

        private void tbxPos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.aceptar.Focus();
        }

        private void tbxPos_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                if (e.KeyChar != (char)Keys.Enter && e.KeyChar != (char)Keys.Tab && e.KeyChar != 8)
                {
                    e.Handled = true;
                }
                else
                {
                    e.Handled = false;
                }
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel2.ClientRectangle,
                Color.LightSteelBlue, 1, ButtonBorderStyle.Solid,
                Color.LightSteelBlue, 1, ButtonBorderStyle.Solid,
                Color.LightSteelBlue, 1, ButtonBorderStyle.Solid,
                Color.LightSteelBlue, 1, ButtonBorderStyle.Solid);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel3.ClientRectangle,
                Color.LightSteelBlue, 1, ButtonBorderStyle.Solid,
                Color.LightSteelBlue, 1, ButtonBorderStyle.Solid,
                Color.LightSteelBlue, 1, ButtonBorderStyle.Solid,
                Color.LightSteelBlue, 1, ButtonBorderStyle.None);
        }

        #region tbxNombre
        private void tbxNombre_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxNombre.BackColor;
            tbxNombre.BackColor = Color.FromArgb(255, 255, 174);
        }

        private void tbxNombre_Leave(object sender, EventArgs e)
        {
            tbxNombre.BackColor = colorbefore;
        }

        private void tbxNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= 'a' && e.KeyChar <= 'z' || e.KeyChar >= 'A' && e.KeyChar <= 'Z' || e.KeyChar == 8 || e.KeyChar == 32 || e.KeyChar >= '0' && e.KeyChar <= '9')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        #endregion     

        #region cbxFolder
        private void cbxFolder_Enter(object sender, EventArgs e)
        {
            colorbefore = cbxFolder.BackColor;
            cbxFolder.BackColor = Color.FromArgb(255, 255, 174);
        }

        private void cbxFolder_Leave(object sender, EventArgs e)
        {
            cbxFolder.BackColor = colorbefore;
        }
        #endregion

        private void cbxFolder_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}
