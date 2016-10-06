using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MainMenu
{
    public partial class UserUsuarios : UserControl
    {
        #region Declaracion Class
        ADOutil Conec = new ADOutil();
        CheckBox[] chk_Atributos;
        Color colorbefore;
        #endregion

        public UserUsuarios()
        {
            InitializeComponent();
            this.toolStripLabel2.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + string.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);
            chk_Atributos = new CheckBox[24] { atributo0, atributo1, atributo2, atributo3,
                                            atributo4, atributo5, atributo6, atributo7,
                                            atributo8, atributo9, atributo10, atributo11,
                                            atributo12, atributo13, atributo14, atributo15,
                                            atributo16, atributo17, atributo18, atributo19,
                                            atributo20, atributo21, atributo22, atributo23};
            asignarEventos();
            listadoId_EnBD("");    
        }
       
        #region Activacion de controles en la vista
        void activarDesactivarEdicion(bool pbActivar, Color pbColor)
        {
            tbxUser.Enabled = pbActivar;
            tbxPass.Enabled = pbActivar;
            tbxDescripcion.Enabled = pbActivar;
            cbxTipo.Enabled = pbActivar;
            activadesactiva_check(chk_Atributos, pbActivar);
           
        }       
        private void limpiezaTextBoxes()
        {
            activarDesactivarEdicion(true,Color.White);
            tbxUser.Clear();
            tbxPass.Clear();
            tbxDescripcion.Clear();
            limpia_check(chk_Atributos,false);
            cbxTipo.SelectedIndex = 0;            
            tbxUser.Focus();
        }
        private string asigna_privilegio(CheckBox[] atribb)
        {
            string atributos = "";

            for (int i = 0; i < atribb.Length; i++)
            {
                if (atribb[i].Checked == true)
                    atributos = atributos + "1";
                else
                    atributos = atributos + "0";
            }
            return atributos;
        }
        private void asigna_check(string elemento)
        {
            for (int i = 0; i < elemento.Length; i++)
            {
                if (elemento.Substring(i, 1) == "1")
                { chk_Atributos[i].Checked = true; }
                else
                { chk_Atributos[i].Checked = false; }
            }
        }
        private void limpia_check(CheckBox[] atribb, bool chkactive)
        {
            for (int i = 0; i < atribb.Length; i++)
            {
                chk_Atributos[i].Checked = chkactive;
            }
        }
        private void activadesactiva_check(CheckBox[] atribb, bool chkactive)
        {
            for (int i = 0; i < atribb.Length; i++)
            {
                chk_Atributos[i].Enabled = chkactive;
            }
        }
        private void asignarEventos()
        {
            foreach (CheckBox permiso in chk_Atributos)
            {
                permiso.CheckedChanged += new EventHandler(permiso_CheckedChanged);
            }
        }
        #endregion

        #region Registro de Base de Datos y DataSet
        private void treeListado_AfterSelect(object sender, TreeViewEventArgs e)
        {            
            TreeView nodo = ((TreeView)sender);
            nodo.Select();
            nodo.Focus();
            activarDesactivarEdicion(false, Color.WhiteSmoke);
            Consulta_EnBD(nodo.SelectedNode.Name);

            if (nodo.SelectedNode.Name == "admin")
            {
                btnBorrar.Enabled = false;
            }
        }
        private void Consulta_EnBD(string ncod)
        {
            usuariosTableAdapter.Fill(baseDeDatosDataSet1.Usuarios);
            baseDeDatosDataSet1.Usuarios.PrimaryKey = new DataColumn[] { baseDeDatosDataSet1.Usuarios.id_userColumn };

            DataRow dr = baseDeDatosDataSet1.Usuarios.Rows.Find(ncod.Trim());
            if (dr != null)
            {
                Form1.statRegistro = ESTADO.EstadoRegistro.PKTRATADO;
                Mostrar_Dato(ref dr);
                btnEditar.Enabled = true;
                btnBorrar.Enabled = true;
                btnGuardar.Enabled = false;
                btnNuevo.Enabled = true;
            }
        }
        private void Mostrar_Dato(ref DataRow dr)
        {
            tbxUser.Text = dr["id_user"].ToString();
            tbxPass.Text = dr["contrasena"].ToString();
            tbxDescripcion.Text = dr["descripcion"].ToString();
            cbxTipo.SelectedIndex = Convert.ToInt16(dr["nivel"].ToString());
            activadesactiva_check(chk_Atributos, false);
            asigna_check(dr["privilegios"].ToString());           
            this.tbxUser.Focus();
        }
        private void listadoId_EnBD(string condicion)
        {
            try
            {
                treListadoU.Nodes.Clear();

                usuariosTableAdapter.Fill(baseDeDatosDataSet1.Usuarios);
                DataRow[] DRV = baseDeDatosDataSet1.Usuarios.Select(condicion, "id_user ASC");

                foreach (DataRow dr in DRV)  //baseDeDatosDataSet.Vendedor.Rows)
                {
                    treListadoU.Nodes.Add(dr["id_user"].ToString(), dr["id_user"].ToString());
                }
                treListadoU.ExpandAll();

                btnEditar.Enabled = false;
                btnBorrar.Enabled = false;
                btnGuardar.Enabled = false;
                btnNuevo.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Excepcion " + ex);
            }
        }
        private bool muestraAutoincrementoId()
        {
            bool cod;

            OleDbDataReader LP = Conec.Obtiene_Dato("SELECT id_user FROM USUARIOS WHERE id_user = '" + tbxUser.Text + "' ORDER BY id_user desc", Conec.CadenaConexion);
            if (LP.Read()) cod = true;
            else cod = false;
            LP.Close();

            return cod;
        }
        private void Guardar(bool Existe)
        {
            string cade_Atribb = asigna_privilegio(chk_Atributos);

            if (!Existe)
            { 
                Conec.CadenaSelect = "INSERT INTO Usuarios " +
                "(id_user, contrasena, descripcion, privilegios, nivel)" +
               "VALUES ('" + tbxUser.Text + "','" + 
               tbxPass.Text + "','" + 
               tbxDescripcion.Text + "','" +
               cade_Atribb + "'," + 
               cbxTipo.SelectedIndex + ")";

                Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet1.Usuarios.TableName);

            }
            else
            {
                Conec.Condicion = "id_user = '" + tbxUser.Text +"'";
                Conec.CadenaSelect = "UPDATE Usuarios SET " +
                "contrasena = '" + tbxPass.Text + "'," +
                "descripcion = '" + tbxDescripcion.Text + "'," +
                "nivel = " + cbxTipo.SelectedIndex + "," +
                "privilegios = '" + cade_Atribb + "' " +
                "WHERE ( " + Conec.Condicion + " )";

                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet1.Usuarios.TableName);
                
            }          
        }
        private void Eliminar()
        {            

            DataRow dr = baseDeDatosDataSet1.Usuarios.Rows.Find(tbxUser.Text.Trim());

            if (dr != null && baseDeDatosDataSet1.Usuarios.Rows.Count > 0)            
            {
                if (tbxUser.Text.Trim() != "admin")
                {
                    if (MessageBox.Show(this, Variable.SYS_MSJ[371, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        dr.Delete();
                        baseDeDatosDataSet1.AcceptChanges();

                        Conec.Condicion = "id_user = '" + tbxUser.Text.Trim() + "' AND id_user <> 'admin'";
                        Conec.CadenaSelect = "DELETE FROM Usuarios " +
                            " WHERE ( " + Conec.Condicion + ")";

                        Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet1.Usuarios.TableName);
                    }
                }
                else
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[373, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(this, Variable.SYS_MSJ[372, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion
                
        private void UserUsuarios_Load(object sender, EventArgs e)
        {
            limpiezaTextBoxes();
            btnEditar.Enabled = false;
            btnGuardar.Enabled = false;
            btnNuevo.Enabled = true;
            cbxTipo.Items[0] = Variable.SYS_MSJ[397, Variable.idioma];
            cbxTipo.Items[1] = Variable.SYS_MSJ[398, Variable.idioma];
            activarDesactivarEdicion(false, Color.WhiteSmoke);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            ToolStripManager.RevertMerge("toolStrip5");
            this.Dispose();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (tbxUser.Text != "")
            {
                Form1.statRegistro = ESTADO.EstadoRegistro.PKTRATADO;
                bool Existe = muestraAutoincrementoId();
                Guardar(Existe);
                MessageBox.Show(this, Variable.SYS_MSJ[241, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                activarDesactivarEdicion(false, Color.WhiteSmoke);
                listadoId_EnBD("");
            }
            else
            {
                MessageBox.Show(this, Variable.SYS_MSJ[370, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbxUser.Focus();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Form1.statRegistro = ESTADO.EstadoRegistro.PKPARCIAL;
            activarDesactivarEdicion(true, Color.White);
            if (cbxTipo.SelectedIndex == 0)
            {
                activadesactiva_check(chk_Atributos, true);
            }
            else
            {
                activadesactiva_check(chk_Atributos, true);
                this.atributo1.Enabled = false;
                this.atributo17.Enabled = false;
                this.atributo18.Enabled = false;
                this.atributo19.Enabled = false;
                this.atributo20.Enabled = false;
                this.atributo21.Enabled = false;
                this.atributo22.Enabled = false;
                this.atributo23.Enabled = false;
            }
            btnEditar.Enabled = false;
            btnGuardar.Enabled = true;
            btnBorrar.Enabled = false;
            tbxUser.Enabled = false;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Form1.statRegistro = ESTADO.EstadoRegistro.PKNOTRATADO;
            limpiezaTextBoxes();
            btnEditar.Enabled = false;
            btnBorrar.Enabled = false;
            btnGuardar.Enabled = true;
            btnNuevo.Enabled = false;
        }
        private void btnBorrar_Click(object sender, EventArgs e)
        {
            Eliminar();
            Form1.statRegistro = ESTADO.EstadoRegistro.PKTRATADO;
            listadoId_EnBD("");

            limpiezaTextBoxes();
            activarDesactivarEdicion(false, Color.WhiteSmoke);
        }
        private void tbxUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Consulta_EnBD(tbxUser.Text);
                this.tbxPass.Focus();
            }
        }

        private void tbxPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.cbxTipo.Focus();
        }

        private void tbxDescripcion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.atributo0.Focus();
        }

        private void permiso_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox permiso = (CheckBox)sender;

            GetNextControl(permiso, true).Focus();
        }

        private void tbxUser_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= 'a' && e.KeyChar <= 'z' || e.KeyChar >= 'A' && e.KeyChar <= 'Z' || e.KeyChar == 8 || e.KeyChar >= '0' && e.KeyChar <= '9')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void tbxPass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= 'a' && e.KeyChar <= 'z' || e.KeyChar >= 'A' && e.KeyChar <= 'Z' || e.KeyChar == 8 || e.KeyChar >= '0' && e.KeyChar <= '9')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void tbxDescripcion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= 'a' && e.KeyChar <= 'z' || e.KeyChar >= 'A' && e.KeyChar <= 'Z' || e.KeyChar == 8 || e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 32)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void tbxUser_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxUser.BackColor;
            tbxUser.BackColor = Color.FromArgb(255, 255, 174);
        }

        private void tbxPass_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxPass.BackColor;
            tbxPass.BackColor = Color.FromArgb(255, 255, 174);
        }

        private void tbxDescripcion_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxDescripcion.BackColor;
            tbxDescripcion.BackColor = Color.FromArgb(255, 255, 174);
        }

        private void tbxUser_Leave(object sender, EventArgs e)
        {
            tbxUser.BackColor = colorbefore;
        }

        private void tbxPass_Leave(object sender, EventArgs e)
        {
            tbxPass.BackColor = colorbefore;
        }

        private void tbxDescripcion_Leave(object sender, EventArgs e)
        {
            tbxDescripcion.BackColor = colorbefore;
        }

        private void cbxTipo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.tbxDescripcion.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            usuariosTableAdapter.Fill(baseDeDatosDataSet1.Usuarios);

            listadoId_EnBD("descripcion Like '*" + tbxFind.Text + "*'");
        }

        private void cbxTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Form1.statRegistro != ESTADO.EstadoRegistro.PKTRATADO)
            {
                if (cbxTipo.SelectedIndex == 0)
                {
                    activadesactiva_check(chk_Atributos, true);
                    limpia_check(chk_Atributos, true);
                }
                else
                {
                    activadesactiva_check(chk_Atributos, true);
                    limpia_check(chk_Atributos, false);
                    this.atributo1.Enabled = false;
                    this.atributo17.Enabled = false;
                    this.atributo18.Enabled = false;
                    this.atributo19.Enabled = false;
                    this.atributo20.Enabled = false;
                    this.atributo21.Enabled = false;
                    this.atributo22.Enabled = false;
                    this.atributo23.Enabled = false;
                }
            }
        }
    }
}
