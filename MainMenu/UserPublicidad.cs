using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace MainMenu
{
    public partial class UserPublicidad : UserControl
    {
        #region Declaracion Class
         ADOutil Conec = new ADOutil();
        #endregion

        #region Declaracion Constantes y Variables
        string idpubl;
        private int iActionToSave = 0;
        Color colorbefore;
        #endregion

        public UserPublicidad()
        {
            InitializeComponent();
            this.tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + string.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);

            treListadoM.TabStop = true;
            tbxFind.TabStop = true;
            button1.TabStop = true;
            panel1.TabStop = true;

            tbxFind.Focus();
        }

        #region Procesos treeview
        private void treeListado_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView nodo = ((TreeView)sender);
            nodo.Select();
            nodo.Focus();
            activarDesactivarEdicion(false, Color.WhiteSmoke);
            Consulta_EnBD(nodo.SelectedNode.Name);

            tbxFind.Enabled = true;
            button1.Enabled = true;
        }
                   
        #endregion

        #region Captura, validacion y eventos de control
       
        private void tbxPubli_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.tbxTituloPubli.Focus();
        }
        private void tbxPubli_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxPubli.BackColor;
            tbxPubli.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxPubli_Leave(object sender, EventArgs e)
        {
            tbxPubli.BackColor = colorbefore;
        }
        private void tbxPubli_Validated(object sender, EventArgs e)
        {
            string dat = Variable.validar_salida(tbxPubli.Text, 0);
            if (dat != "")
            {
                tbxPubli.Text = dat;
            }
            else tbxPubli.Focus();
        }

        private void tbxDescripcion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.tbxPubli.Focus();
        }
        private void tbxDescripcion_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxDescripcion.BackColor;
            tbxDescripcion.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxDescripcion_Leave(object sender, EventArgs e)
        {
            tbxDescripcion.BackColor = colorbefore;
        }
        private void tbxDescripcion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (e.KeyChar == 'Ñ' || e.KeyChar == 'ñ')
            {
                e.Handled = true;
            }
            else if (Char.IsLetter(e.KeyChar) || Char.IsPunctuation(e.KeyChar) || Char.IsSymbol(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (e.KeyChar == 8 || e.KeyChar == 32)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        private void tbxDescripcion_Validated(object sender, EventArgs e)
        {
            string dat = Variable.validar_salida(tbxDescripcion.Text, 2);
            if (dat != "")
            {
                tbxDescripcion.Text = dat;
            }
        }

        private void tbxTituloPubli_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.tbxDescripcion.Focus();               
        }       
        private void tbxTituloPubli_Validated(object sender, EventArgs e)
        {
            string dat = Variable.validar_salida(tbxTituloPubli.Text, 8);
            if (dat != "")
            {
                tbxTituloPubli.Text = dat;
            }
            else tbxTituloPubli.Focus();
        }
        private void tbxTituloPubli_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxTituloPubli.BackColor;
            tbxTituloPubli.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxTituloPubli_Leave(object sender, EventArgs e)
        {
            tbxTituloPubli.BackColor = colorbefore;
        }
        private void tbxTituloPubli_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (e.KeyChar == 'Ñ' || e.KeyChar == 'ñ')
            {
                e.Handled = true;
            }
            else if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (e.KeyChar == 8 || e.KeyChar == 32)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
       
        #endregion

        #region evento de botones
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            limpiezaTextBoxes();
            Form1.statRegistro = ESTADO.EstadoRegistro.PKNOTRATADO;
            btnEditar.Enabled = false;
            btnBorrar.Enabled = false;
            btnGuardar.Enabled = true;
            btnNuevo.Enabled = false;
            iActionToSave = 0;

            treListadoM.TabStop = false;
            tbxFind.TabStop = false;
            button1.TabStop = false;
            panel1.TabStop = false;

            tbxFind.Enabled = false;
            button1.Enabled = false;

            tbxTituloPubli.Focus();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            iActionToSave = 1;
            activarDesactivarEdicion(true, Color.White);
            tbxTituloPubli.Focus();
            Form1.statRegistro = ESTADO.EstadoRegistro.PKPARCIAL;
            btnEditar.Enabled = false;
            btnGuardar.Enabled = true;
            btnBorrar.Enabled = false;

            treListadoM.TabStop = false;
            tbxFind.TabStop = false;
            button1.TabStop = false;
            panel1.TabStop = false;

            tbxFind.Enabled = false;
            button1.Enabled = false;

            tbxTituloPubli.Focus();
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            bool error = false;
            if (tbxPubli.Text == "" || tbxPubli.Text == "0")
            {
                MessageBox.Show(this, Variable.SYS_MSJ[238, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbxPubli.Focus();
                error = true;
            }
            if (tbxTituloPubli.Text == "" || tbxTituloPubli.Text == "0")
            {
                MessageBox.Show(this, Variable.SYS_MSJ[77, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbxTituloPubli.Focus();
                error = true;
            }            
            if (Form1.statRegistro == ESTADO.EstadoRegistro.PKNOTRATADO)
            {
                OleDbDataReader olplu = Conec.Obtiene_Dato("Select Titulo  From Publicidad Where Titulo = '" + tbxTituloPubli.Text.Trim() + "'" + " AND borrado = " + false, Conec.CadenaConexion);
                if (olplu.Read())
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[277, Variable.idioma]);
                    this.tbxTituloPubli.Focus();
                    error = true;
                }
                olplu.Close();
            }
            if (!error)
            {
                OleDbDataReader olplu = Conec.Obtiene_Dato("Select id_publicidad  From Publicidad Where id_publicidad = " + Convert.ToInt32(tbxPubli.Text), Conec.CadenaConexion);
                if (olplu.Read())
                {
                    iActionToSave = 1;
                }
                else iActionToSave = 0;
                olplu.Close();

                Guardar(iActionToSave);
                //if (Form1.statRegistro == ESTADO.EstadoRegistro.PKNOTRATADO) limpiezaTextBoxes();

                //if (Form1.statRegistro == ESTADO.EstadoRegistro.PKPARCIAL) activarDesactivarEdicion(false, Color.WhiteSmoke);
                activarDesactivarEdicion(false, Color.WhiteSmoke);
                listadoId_EnBD("borrado = " + false);
                Form1.statRegistro = ESTADO.EstadoRegistro.PKTRATADO;

                treListadoM.TabStop = true;
                tbxFind.TabStop = true;
                button1.TabStop = true;
                panel1.TabStop = true;

                MessageBox.Show(this, Variable.SYS_MSJ[356, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbxFind.Focus();

                tbxFind.Enabled = true;
                button1.Enabled = true;
            }
        }
        private void btnBorrar_Click(object sender, EventArgs e)
        {
            Eliminar();
            Form1.statRegistro = ESTADO.EstadoRegistro.PKTRATADO;
            listadoId_EnBD("borrado = false");
            limpiezaTextBoxes();
            activarDesactivarEdicion(false, Color.WhiteSmoke);

            tbxFind.Enabled = true;
            button1.Enabled = true;
        }
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            ToolStripManager.RevertMerge("toolStrip2");
            this.Dispose();
        }
        private void btnDepurar_Click(object sender, EventArgs e)
        {
            UserDepurar uspurge = new UserDepurar((int)ESTADO.FileSource.fMensajes);
            uspurge.ShowDialog(this);
            listadoId_EnBD("borrado = " + false);
            limpiezaTextBoxes();
            activarDesactivarEdicion(false, Color.WhiteSmoke);
        }
        private void btnExportar_Click(object sender, EventArgs e)
        {
            FileStream MyFileExp = null;
            try
            {
                SaveFileDialog dlgOpenFile = new SaveFileDialog();
                dlgOpenFile.InitialDirectory = Variable.appPath;

                dlgOpenFile.DefaultExt = ".txt";
                dlgOpenFile.Filter = "Data files (*.txt)|*.txt|All files(*.*)|*.*";

                dlgOpenFile.FilterIndex = 1;
                dlgOpenFile.RestoreDirectory = true;

                if (dlgOpenFile.ShowDialog() == DialogResult.OK)
                {
                    if (dlgOpenFile.CreatePrompt == true)
                    {
                        string path = dlgOpenFile.FileName;
                        MyFileExp = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
                    }
                    if (dlgOpenFile.OverwritePrompt == true)
                    {
                        string path = dlgOpenFile.FileName;
                        MyFileExp = new FileStream(path, FileMode.Create, FileAccess.Write);
                    }

                    ImpExp UsExport = new ImpExp();

                    UsExport.exportar(4, ref MyFileExp);

                    MyFileExp.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnImportar_Click(object sender, EventArgs e)
        {
            string nombre_archivo;
            FileStream MyFile = null;

            OpenFileDialog dlgOpenFile = new OpenFileDialog();

            try
            {
                dlgOpenFile.ShowReadOnly = true;
                dlgOpenFile.InitialDirectory = Variable.appPath;

                dlgOpenFile.DefaultExt = "*.txt";
                dlgOpenFile.Filter = "Data files (*.txt)|*.txt|All files(*.*)|*.*";

                if (dlgOpenFile.ShowDialog() == DialogResult.OK)
                {
                    nombre_archivo = dlgOpenFile.FileName;

                    if (nombre_archivo != "")
                    {
                        if (dlgOpenFile.ReadOnlyChecked == true)
                        {
                            MyFile = (FileStream)dlgOpenFile.OpenFile();
                        }
                        else
                        {
                            MyFile = new FileStream(nombre_archivo, FileMode.Open, FileAccess.Read);
                        }
                        ImpExp IMP = new ImpExp();
                        IMP.importar((int)ESTADO.FileSource.fMensajes, ref MyFile, nombre_archivo);

                        listadoId_EnBD("borrado = " + false);
                        limpiezaTextBoxes();
                        activarDesactivarEdicion(false, Color.WhiteSmoke);
                    }
                    else MessageBox.Show(this, Variable.SYS_MSJ[29, Variable.idioma]);  //"No hay archivo seleccionado");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Activacion de controles en la vista
        void activarDesactivarEdicion(bool pbActivar, Color pbColor)
        {
           // tbxPubli.Enabled = pbActivar;
            tbxTituloPubli.Enabled = pbActivar;
            tbxTituloPubli.BackColor = pbColor;
            tbxDescripcion.Enabled = pbActivar;
            tbxDescripcion.BackColor = pbColor;
        }

        private void limpiezaTextBoxes()
        {
            tbxPubli.Clear();
            tbxTituloPubli.Clear();
            tbxDescripcion.Clear();
            tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);
            activarDesactivarEdicion(true, Color.White);
            tbxPubli.Text = muestraAutoincrementoId();
            idpubl = tbxPubli.Text;
            tbxTituloPubli.Focus();
        }
        #endregion

        #region Registro de Base de Datos y DataSet
        private void Consulta_EnBD(string ncod)
        {
            DataRow dr = baseDeDatosDataSet.Publicidad.Rows.Find(Convert.ToInt32(ncod));
            if (dr != null)
            {
                Mostrar_Dato(ref dr);
                if (!Convert.ToBoolean(dr["borrado"]))
                {
                    btnEditar.Enabled = true;
                    btnBorrar.Enabled = true;
                    btnGuardar.Enabled = false;
                    btnNuevo.Enabled = true;
                }
                else
                {
                    btnEditar.Enabled = false;
                    btnBorrar.Enabled = false;
                    btnGuardar.Enabled = false;
                    btnNuevo.Enabled = true;
                }
            }

        }
        private void Mostrar_Dato(ref DataRow dr)
        {
            idpubl = dr["id_publicidad"].ToString();
            tbxPubli.Text = dr["id_publicidad"].ToString();
            tbxTituloPubli.Text = dr["Titulo"].ToString();
            tbxDescripcion.Text = dr["Mensaje"].ToString();
            tbxTituloPubli.Focus();
        }
            
        private void listadoId_EnBD(string condicion)
        {
            try
            {
                treListadoM.Nodes.Clear();
                              
                publicidadTableAdapter.Fill(baseDeDatosDataSet.Publicidad);

                DataRow[] DRP = baseDeDatosDataSet.Publicidad.Select(condicion, "id_publicidad ASC");

                foreach (DataRow dr in DRP)
                {
                    treListadoM.Nodes.Add(dr["id_publicidad"].ToString(), dr["Titulo"].ToString());
                }
                treListadoM.ExpandAll();

                if (treListadoM.Nodes.Count == 0)
                {
                    btnEditar.Enabled = false;
                    btnBorrar.Enabled = false;
                    btnGuardar.Enabled = false;
                    btnNuevo.Enabled = true;
                    btnImportar.Enabled = true;
                    btnExportar.Enabled = false;
                    btnDepurar.Enabled = false;
                }
                else
                {
                    btnEditar.Enabled = false;
                    btnBorrar.Enabled = false;
                    btnGuardar.Enabled = false;
                    btnNuevo.Enabled = true;
                    btnImportar.Enabled = false;
                    btnExportar.Enabled = true;
                    btnDepurar.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Excepcion " + ex);
            }
        }
        private string muestraAutoincrementoId()
        {
            int cod = 0;

            OleDbDataReader LP = Conec.Obtiene_Dato("SELECT id_publicidad FROM PUBLICIDAD ORDER BY id_publicidad desc", Conec.CadenaConexion);
            if (LP.Read()) cod = Convert.ToInt32(LP.GetValue(0));
            LP.Close();
            return Convert.ToString(cod + 1);

        }
        private void Guardar(int iActionToSave)
        {            
            string fecha_act = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);
            tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);

            if (iActionToSave == 0)
            {
                DataRow dr = baseDeDatosDataSet.Publicidad.NewRow();

                dr.BeginEdit();
                dr["id_publicidad"] = tbxPubli.Text;
                dr["Titulo"] = Variable.validar_salida(tbxTituloPubli.Text,8);
                dr["Mensaje"] = Variable.validar_salida(tbxDescripcion.Text,2);
                dr["pendiente"] = false;
                dr.EndEdit();

                publicidadTableAdapter.Update(dr);
                baseDeDatosDataSet.Publicidad.AcceptChanges();

                Conec.CadenaSelect = "INSERT INTO Publicidad " +
                "(id_publicidad,Titulo,Mensaje,pendiente, actualizado)" +
               "VALUES (" + Convert.ToInt32(tbxPubli.Text) + ",'" + dr["Titulo"].ToString() + "','" + dr["Mensaje"].ToString() + "'," + false + ",'" + fecha_act + "')";

                Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Publicidad.TableName);
            }
            if (iActionToSave == 1)
            {
                DataRow dr = baseDeDatosDataSet.Publicidad.Rows.Find(Convert.ToInt32(idpubl));
                if (dr != null)
                {
                    dr.BeginEdit();
                    dr["Titulo"] = Variable.validar_salida(tbxTituloPubli.Text,8);
                    dr["Mensaje"] = Variable.validar_salida(tbxDescripcion.Text,2);
                    dr["pendiente"] = true;
                    dr.EndEdit();

                    publicidadTableAdapter.Update(dr);
                    baseDeDatosDataSet.Publicidad.AcceptChanges();

                    Conec.Condicion = "id_publicidad = " + Convert.ToInt32(idpubl);
                    Conec.CadenaSelect = "UPDATE Publicidad " +
                    "SET Titulo = '" + dr["Titulo"].ToString() +
                    "', Mensaje = '" + dr["Mensaje"].ToString() +
                    "', pendiente = " + true +
                    ", actualizado = '" + fecha_act +
                    "' WHERE (" + Conec.Condicion + ")";

                    Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Publicidad.TableName);
                }
            }
        }

        private void Eliminar()
        {
            bool borrar = false;

            DataRow dr = baseDeDatosDataSet.Publicidad.Rows.Find(Convert.ToInt32(idpubl));

            if (dr != null && baseDeDatosDataSet.Publicidad.Rows.Count > 0)
            {
                if (MessageBox.Show(this, Variable.SYS_MSJ[87, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    borrar = true;
                    OleDbDataReader BPub = Conec.Obtiene_Dato("SELECT * FROM Public_Detalle WHERE id_publicidad = " + idpubl,Conec.CadenaConexion );
                    if (BPub.Read())
                    {
                        borrar = true;
                        MessageBox.Show(this, Variable.SYS_MSJ[88, Variable.idioma]);  //"El mensaje esta asignado a una bascula");                        
                    }
                    BPub.Close();

                    OleDbDataReader BPub2 = Conec.Obtiene_Dato("SELECT * FROM Productos WHERE publicidad1 = " + idpubl + " or publicidad2 = " + idpubl + " or publicidad3 = " + idpubl + " or publicidad4 = " + idpubl , Conec.CadenaConexion);
                    if (BPub2.Read())
                    {
                        borrar = true;
                        MessageBox.Show(this, Variable.SYS_MSJ[89, Variable.idioma]);  //"El mensaje esta asignado a un producto");
                    }
                    BPub2.Close();

                    if (borrar)
                    {
                        dr.Delete();
                        baseDeDatosDataSet.AcceptChanges();

                        Conec.Condicion = "id_publicidad = " + Convert.ToInt32(idpubl);
                        Conec.CadenaSelect = "UPDATE Publicidad  SET borrado = " + true + " WHERE (" + Conec.Condicion + ")";

                        Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Publicidad.TableName);

                        Conec.Condicion = "publicidad1 = " + Convert.ToInt32(idpubl);
                        Conec.CadenaSelect = "UPDATE Productos  SET publicidad1 = 0, pendiente = " + true + " WHERE (" + Conec.Condicion + ")";

                        Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Publicidad.TableName);

                        Conec.Condicion = "publicidad2 = " + Convert.ToInt32(idpubl);
                        Conec.CadenaSelect = "UPDATE Productos  SET publicidad2 = 0, pendiente = " + true + " WHERE (" + Conec.Condicion + ")";

                        Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Publicidad.TableName);

                        Conec.Condicion = "publicidad3 = " + Convert.ToInt32(idpubl);
                        Conec.CadenaSelect = "UPDATE Productos  SET publicidad3 = 0, pendiente = " + true + " WHERE (" + Conec.Condicion + ")";

                        Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Publicidad.TableName);

                        Conec.Condicion = "publicidad4 = " + Convert.ToInt32(idpubl);
                        Conec.CadenaSelect = "UPDATE Productos  SET publicidad4 = 0, pendiente = " + true + " WHERE (" + Conec.Condicion + ")";

                        Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Publicidad.TableName);
                    }
                }
            }
            else
            {
                MessageBox.Show(this, Variable.SYS_MSJ[91, Variable.idioma], "", MessageBoxButtons.OK);
            }
        }
        #endregion

        private void UserPublicidad_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Publicidad' Puede moverla o quitarla según sea necesario.
            this.publicidadTableAdapter.Fill(this.baseDeDatosDataSet.Publicidad);
            listadoId_EnBD("borrado = " + false);
            //if (treListadoM.Nodes.Count > 0)
            //{
                activarDesactivarEdicion(false, Color.WhiteSmoke);
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            publicidadTableAdapter.Fill(baseDeDatosDataSet.Publicidad);
            
            listadoId_EnBD("Titulo Like '*" + tbxFind.Text + "*'");
        }

        private void tbxPubli_KeyPress(object sender, KeyPressEventArgs e)
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

        private void tbxFind_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxFind.BackColor;
            tbxFind.BackColor = Color.FromArgb(255, 255, 174);
        }

        private void tbxFind_Leave(object sender, EventArgs e)
        {
            tbxFind.BackColor = colorbefore;
        }

        private void tbxFind_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(button1, null);
            }
        }        
                       
    }
}
