using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;

namespace MainMenu
{
	/// <summary>
	/// Descripción breve de rcarpeta.
	/// </summary>
	public class RangoCarpeta : System.Windows.Forms.Form
	{
		private Button button1;
		private Button button2;
		private Label label1;
        private Label label2;
		private Label label3;
		private Label label4;		
		private ComboBox cbxcarpetas;
		public int codigo1;
		public int codigo2;
		public int n_folder;
		ADOutil Conec = new ADOutil();
        private GroupBox groupBox1;
        private TextBox tbxcode2;
        private TextBox tbxcode1;
		/// <summary>
		/// Variable del diseñador requerida.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public RangoCarpeta()
		{
			//
			// Necesario para admitir el Diseñador de Windows Forms
			//
			InitializeComponent();
			tbxcode1.KeyDown+=new System.Windows.Forms.KeyEventHandler(this.code1_KeyDown);
		    tbxcode1.Validating+=new System.ComponentModel.CancelEventHandler(this.code1_Validating);
		    tbxcode2.KeyDown+=new System.Windows.Forms.KeyEventHandler(this.tbxcode2_KeyDown);
		    tbxcode2.Validating+=new System.ComponentModel.CancelEventHandler(this.tbxcode2_Validating);
			this.cbxcarpetas.SelectedIndexChanged+=new System.EventHandler(this.combocarpeta_SelectedIndexChanged);

			Conec.CadenaSelect = "SELECT * FROM carpeta ORDER BY ID";
          
			Conec.ConectarDB(Conec.ArchivoDatos,Conec.CadenaSelect,"carpeta",Conec.CadenaConexion);
			Conec.dbDataSet.Tables["carpeta"].PrimaryKey = new DataColumn[] {Conec.dbDataSet.Tables["carpeta"].Columns["ID"]};

			if (Conec.dbDataSet.Tables["carpeta"].Rows.Count  > 0)
			{
				this.cbxcarpetas.DataSource = Conec.dbDataSet.Tables["carpeta"];
				this.cbxcarpetas.DisplayMember = "descripcion";
				this.cbxcarpetas.ValueMember = "ID";
			}

			//
			// TODO: Agregar código de constructor después de llamar a InitializeComponent
			//
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RangoCarpeta));
            this.cbxcarpetas = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbxcode2 = new System.Windows.Forms.TextBox();
            this.tbxcode1 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbxcarpetas
            // 
            resources.ApplyResources(this.cbxcarpetas, "cbxcarpetas");
            this.cbxcarpetas.Name = "cbxcarpetas";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.BackColor = System.Drawing.Color.Teal;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.BackColor = System.Drawing.Color.Teal;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.tbxcode2);
            this.groupBox1.Controls.Add(this.tbxcode1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cbxcarpetas);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // tbxcode2
            // 
            resources.ApplyResources(this.tbxcode2, "tbxcode2");
            this.tbxcode2.Name = "tbxcode2";
            // 
            // tbxcode1
            // 
            resources.ApplyResources(this.tbxcode1, "tbxcode1");
            this.tbxcode1.Name = "tbxcode1";
            this.tbxcode1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxcode1_KeyDown);
            // 
            // RangoCarpeta
            // 
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "RangoCarpeta";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion
		private void code1_KeyDown(object sender,System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter) tbxcode2.Focus();
		}
		private void tbxcode2_KeyDown(object sender,System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter) this.cbxcarpetas.Focus();
		}
		private void code1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (tbxcode2.Text != "")
			{
				if ( tbxcode1.Text == "") 
				{
					MessageBox.Show(this.label1.Text + Variable.SYS_MSJ[201,Variable.idioma]);  //" No puede estar vacia");
					tbxcode1.Focus();
				}
				else 
				{
					if (Convert.ToInt32(this.tbxcode1.Text) > Convert.ToInt32(this.tbxcode2.Text))
					{
						MessageBox.Show(this.label1.Text + Variable.SYS_MSJ[203,Variable.idioma]); //" No puede ser menor que la incial ");
						this.tbxcode1.Focus();
					}
				}
			}
			if (this.tbxcode1.Text != "" )this.codigo1= Convert.ToInt32(this.tbxcode1.Text);
		}
		private void tbxcode2_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (this.tbxcode1.Text != "")
			{
				if (this.tbxcode2.Text == "") 
				{
					MessageBox.Show(this.label1.Text + Variable.SYS_MSJ[201,Variable.idioma]); // " No puede estar vacia");
					this.tbxcode2.Focus();
				}
				else 
				{
					if (Convert.ToInt32(this.tbxcode2.Text) < Convert.ToInt32(this.tbxcode1.Text))
					{
						MessageBox.Show(this.label1.Text + Variable.SYS_MSJ[203,Variable.idioma]);  //" No puede ser menor que la inicial" );
						this.tbxcode2.Focus();
					}
				}
			}
			if (this.tbxcode2.Text != "" )this.codigo2= Convert.ToInt32(this.tbxcode2.Text);
		}

		private void combocarpeta_SelectedIndexChanged(object sender,System.EventArgs e)
		{
			System.Windows.Forms.ComboBox f = (System.Windows.Forms.ComboBox)sender;
			f.Select();
    	}

		private void button2_Click(object sender, System.EventArgs e)
		{
			this.Dispose();
			this.Close();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.codigo1=Convert.ToInt32(this.tbxcode1.Text);
			this.codigo2=Convert.ToInt32(this.tbxcode2.Text);
			this.n_folder=Convert.ToInt32(this.cbxcarpetas.SelectedValue);
			this.Close();
		}

        private void tbxcode1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        
	}
}
