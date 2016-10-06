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
    public partial class UserIngrediente : UserControl
    {
        #region Declaracion Class
        ADOutil Conec = new ADOutil();
        #endregion

        #region Declaracion Constantes y Variables
        string idingre;
        Color colorbefore;
        private int iActionToSave = 0;
        #endregion

        public UserIngrediente()
        {
            InitializeComponent();
            this.tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + string.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);

            treListadoI.TabStop = true;
            tbxFind.TabStop = true;
            button1.TabStop = true;
            panel1.TabStop = true;

            tbxFind.Focus();
        }

        #region Activacion de controles en la vista
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

        void activarDesactivarEdicion(bool pbActivar, Color pbColor)
        {
            //tbxNumero.Enabled = pbActivar;
            tbxNombre.Enabled = pbActivar;
            tbxNombre.BackColor = pbColor;
            tbxDescripcion.Enabled = pbActivar;
            tbxDescripcion.BackColor = pbColor;
        }                

        private void limpiezaTextBoxes()
        {           
            tbxNumero.Clear();
            tbxNombre.Clear();
            tbxDescripcion.Clear();
            tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);
            activarDesactivarEdicion(true, Color.White);
            tbxNumero.Text = muestraAutoincrementoId();
            idingre = tbxNumero.Text;
            tbxNombre.Focus();
        }
        #endregion

        #region Registro de Base de Datos y DataSet
        private void Consulta_EnBD(string ncod)
        {
            DataRow dr = baseDeDatosDataSet.Ingredientes.Rows.Find(Convert.ToInt64(ncod));
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
            idingre = dr["id_ingrediente"].ToString();
            tbxNumero.Text = dr["id_ingrediente"].ToString();
            tbxNombre.Text = dr["Nombre"].ToString();
            tbxDescripcion.Text = dr["Informacion"].ToString();
            tbxFecha.Text = String.Format(Variable.F_Hora, Convert.ToDateTime(dr["actualizado"].ToString())) + " " + String.Format(Variable.FOR_FECHAS[Variable.ffecha], Convert.ToDateTime(dr["actualizado"].ToString())); ;
            tbxNombre.Focus();
        }      
        private void listadoId_EnBD(string condicion)
        {
            try
            {
                treListadoI.Nodes.Clear();

                ingredientesTableAdapter.Fill(baseDeDatosDataSet.Ingredientes);

                DataRow[] DRI = baseDeDatosDataSet.Ingredientes.Select(condicion,"id_ingrediente ASC");
                foreach (DataRow dr in DRI)
                {
                    treListadoI.Nodes.Add(dr["id_ingrediente"].ToString(), dr["Nombre"].ToString());
                }
                treListadoI.ExpandAll();

                if (treListadoI.Nodes.Count == 0)
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

            OleDbDataReader LP = Conec.Obtiene_Dato("SELECT id_ingrediente FROM Ingredientes ORDER BY id_ingrediente desc", Conec.CadenaConexion);
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
                DataRow dr = baseDeDatosDataSet.Ingredientes.NewRow();

                dr.BeginEdit();
                dr["id_ingrediente"] = tbxNumero.Text;  // idingre;
                dr["Nombre"] = Variable.validar_salida(tbxNombre.Text,8);
                dr["Informacion"] = Variable.validar_salida(tbxDescripcion.Text,2);
                dr["actualizado"] = fecha_act;
                dr["pendiente"] = false;
                dr.EndEdit();

                ingredientesTableAdapter.Update(dr);
                baseDeDatosDataSet.AcceptChanges();

                Conec.CadenaSelect = "INSERT INTO Ingredientes " +
                "(id_ingrediente,Nombre,Informacion,pendiente,actualizado)" +
               "VALUES (" + Convert.ToInt32(dr["id_ingrediente"].ToString()) + ",'" + dr["Nombre"].ToString() + "','" + dr["Informacion"].ToString() + "'," + false + ",'" + fecha_act + "')";

                Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Ingredientes.TableName);
            }
            else
            {
                DataRow dr = baseDeDatosDataSet.Ingredientes.Rows.Find(Convert.ToInt32(idingre));
                if (dr != null)
                {
                    dr.BeginEdit();
                    dr["Nombre"] = Variable.validar_salida(tbxNombre.Text,8);
                    dr["Informacion"] = Variable.validar_salida(tbxDescripcion.Text,2);
                    dr["actualizado"] = fecha_act;
                    dr["pendiente"] = true;
                    dr.EndEdit();

                    ingredientesTableAdapter.Update(dr);
                    baseDeDatosDataSet.AcceptChanges();

                    Conec.Condicion = "id_ingrediente = " + Convert.ToInt32(idingre);
                    Conec.CadenaSelect = "UPDATE Ingredientes " +
                    "SET Nombre = '" + dr["Nombre"].ToString() +
                    "', Informacion = '" + dr["Informacion"].ToString() +
                    "', pendiente = " + true + 
                    ", actualizado = '" + dr["actualizado"].ToString() + 
                    "' WHERE ( " + Conec.Condicion + ")";

                    Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Ingredientes.TableName);
                }
            }
        }
        private void Eliminar()
        {
            bool borrar = false;

            DataRow dr = baseDeDatosDataSet.Ingredientes.Rows.Find(Convert.ToInt32(idingre));

            if (dr != null && baseDeDatosDataSet.Ingredientes.Rows.Count > 0)
            {
                if (MessageBox.Show(this, Variable.SYS_MSJ[64, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    borrar = true;
                    if (Conec.Obtiene_Dato("SELECT * FROM Ingre_detalle WHERE (id_ingrediente = " + idingre + ")", Conec.CadenaConexion).Read())
                    {
                        MessageBox.Show(this, Variable.SYS_MSJ[65, Variable.idioma]); //"El ingrediente esta asignado a una bascula");
                        borrar = true;
                        if (Conec.Obtiene_Dato("SELECT * FROM Productos WHERE (id_ingrediente = " + idingre + ")", Conec.CadenaConexion).Read())
                        {
                            MessageBox.Show(this, Variable.SYS_MSJ[66, Variable.idioma]);  //"El ingrediente esta asignado a un producto");
                            borrar = true;
                        }
                    }
                    if (borrar)
                    {
                        dr.Delete();
                        baseDeDatosDataSet.AcceptChanges();

                        Conec.Condicion = "id_ingrediente = " + Convert.ToInt32(idingre);
                        Conec.CadenaSelect = "UPDATE Ingredientes SET borrado = " + true + " WHERE (" + Conec.Condicion + ")";

                        Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Ingredientes.TableName);
                        Conec.Condicion = "id_ingrediente = " + Convert.ToInt32(idingre);
                        Conec.CadenaSelect = "UPDATE Productos SET id_ingrediente = 0, pendiente = " + true + " WHERE (" + Conec.Condicion + ")";

                        Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Productos.TableName);
                    }
                }
                else
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[67, Variable.idioma], "", MessageBoxButtons.OK); //No hay info adicional dado de alta
                }
            }
        }
        #endregion

        #region Eventos de botones
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            limpiezaTextBoxes();
            Form1.statRegistro = ESTADO.EstadoRegistro.PKNOTRATADO;
            btnEditar.Enabled = false;
            btnBorrar.Enabled = false;
            btnGuardar.Enabled = true;
            btnNuevo.Enabled = false;
            iActionToSave = 0;

            treListadoI.TabStop = false;
            tbxFind.TabStop = false;
            button1.TabStop = false;
            panel1.TabStop = false;
            tbxNombre.Focus();

            tbxFind.Enabled = false;
            button1.Enabled = false;

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            iActionToSave = 1;
            activarDesactivarEdicion(true, Color.White);            
            tbxNombre.Focus();
            Form1.statRegistro = ESTADO.EstadoRegistro.PKPARCIAL;
            btnEditar.Enabled = false;
            btnGuardar.Enabled = true;
            btnBorrar.Enabled = false;

            treListadoI.TabStop = false;
            tbxFind.TabStop = false;
            button1.TabStop = false;
            panel1.TabStop = false;

            tbxFind.Enabled = false;
            button1.Enabled = false;

            tbxNombre.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            bool error = false;
            if (tbxNumero.Text == "" || tbxNumero.Text == "0")
            {
                MessageBox.Show(this, Variable.SYS_MSJ[238, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbxNumero.Focus();
                error = true;
            }
            else if (tbxNombre.Text == "" || tbxNombre.Text == "0")
            {
                MessageBox.Show(this, Variable.SYS_MSJ[77, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbxNombre.Focus();
                error = true;
            }
            else if (tbxDescripcion.Text.Length <= 0)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[81, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbxDescripcion.Focus();
                error = true;
            }
            if (Form1.statRegistro == ESTADO.EstadoRegistro.PKNOTRATADO)
            {
                OleDbDataReader olplu = Conec.Obtiene_Dato("Select Nombre From Ingredientes Where Nombre = '" + tbxNombre.Text + "' AND borrado = " + false, Conec.CadenaConexion);
                if (olplu.Read())
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[264, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tbxNombre.Focus();
                    error = true;
                }
                olplu.Close();
            }
            if (!error)
            {
                OleDbDataReader olplu = Conec.Obtiene_Dato("Select id_ingrediente From Ingredientes Where id_ingrediente = " + Convert.ToInt32(tbxNumero.Text), Conec.CadenaConexion);
                if (olplu.Read())
                {                  
                    iActionToSave = 1;
                }
                else iActionToSave = 0;
                olplu.Close();

                Guardar(iActionToSave);
                //if (Form1.statRegistro == ESTADO.EstadoRegistro.PKNOTRATADO) limpiezaTextBoxes();

                //if (Form1.statRegistro == ESTADO.EstadoRegistro.PKPARCIAL) 
                activarDesactivarEdicion(false, Color.WhiteSmoke);

                listadoId_EnBD("borrado = " + false);

                treListadoI.TabStop = true;
                tbxFind.TabStop = true;
                button1.TabStop = true;
                panel1.TabStop = true;
                MessageBox.Show(this, Variable.SYS_MSJ[345, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbxFind.Focus();

                button1.Enabled = true;
                tbxFind.Enabled = true;
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            Eliminar();
            Form1.statRegistro = ESTADO.EstadoRegistro.PKTRATADO;
            listadoId_EnBD("borrado = " + false);
            limpiezaTextBoxes();
            activarDesactivarEdicion(false, Color.WhiteSmoke);

            tbxFind.Enabled = true;
            button1.Enabled = true;
        }

        private void UserIngrediente_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Ingredientes' Puede moverla o quitarla según sea necesario.
            this.ingredientesTableAdapter.Fill(this.baseDeDatosDataSet.Ingredientes);
            listadoId_EnBD("borrado = " + false);

           // if (treListadoI.Nodes.Count > 0)
            //{
                activarDesactivarEdicion(false, Color.WhiteSmoke);
            //}
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            ToolStripManager.RevertMerge("toolStrip2");
            this.Dispose();
        }

        private void btnDepurar_Click(object sender, EventArgs e)
        {
            UserDepurar uspurge = new UserDepurar((int)ESTADO.FileSource.fInfoAdicional);
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

                    UsExport.exportar(2, ref MyFileExp);

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
                        IMP.importar((int)ESTADO.FileSource.fInfoAdicional, ref MyFile, nombre_archivo);
                       
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

        private void button1_Click(object sender, EventArgs e)
        {
            ingredientesTableAdapter.Fill(baseDeDatosDataSet.Ingredientes);
            listadoId_EnBD("Nombre Like '*" + tbxFind.Text + "*'");
        }

        #endregion

        #region captura y validacion        

        private void tbxNombre_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxNombre.BackColor;
            tbxNombre.BackColor = Color.FromArgb(255, 255, 174);
        }

        private void tbxNombre_Leave(object sender, EventArgs e)
        {
            tbxNombre.BackColor = colorbefore;
        }

        private void tbxNombre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.tbxDescripcion.Focus();            
        }

        private void tbxNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) || e.KeyChar == '.')
            {
                e.Handled = false;
            }
            else if (e.KeyChar == 'Ñ' || e.KeyChar == 'ñ')
            {
                e.Handled = true;
            }
            else if (Char.IsLetter(e.KeyChar) )
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
        private void tbxNombre_Validated(object sender, EventArgs e)
        {

        }

        private void tbxNumero_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxNumero.BackColor;
            tbxNumero.BackColor = Color.FromArgb(255, 255, 174);
        }

        private void tbxNumero_Leave(object sender, EventArgs e)
        {
            tbxNumero.BackColor = colorbefore;
        }
        private void tbxNumero_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.tbxNombre.Focus();
        }

        private void tbxNumero_Validated(object sender, EventArgs e)
        {
            string dat = Variable.validar_salida(tbxNumero.Text, 1);
            if (dat != "")
            {
                tbxNumero.Text = dat;
            }
            else tbxNumero.Focus();
        }

        private void tbxDescripcion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.tbxNumero.Focus();
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
            if (Char.IsNumber(e.KeyChar) || e.KeyChar == '.' )
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

        }
       
        #endregion

        private void tbxNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8 || e.KeyChar >= '0' && e.KeyChar <= '9')
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
