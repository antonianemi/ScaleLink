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
    public partial class UserVendedores : UserControl
    {
        #region Declaracion Class
        ADOutil Conec = new ADOutil();
        #endregion

        #region Declaracion Constantes y Variables
        string idvend;
        int iActionToSave = 0;
        Color colorbefore;
        #endregion

        #region Inicio
        public UserVendedores()
        {
            InitializeComponent();
            this.tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + string.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);
            listadoId_EnBD("borrado = " + false);

            treListadoV.TabStop = true;
            tbxFind.TabStop = true;
            button1.TabStop = true;
            panel1.TabStop = true;
            tbxFind.Focus();
        }
        #endregion

        #region Procesos pushBotones
        private void treeListado_AfterSelect(object sender, TreeViewEventArgs e)
        {            
            TreeView nodo = ((TreeView)sender);
            nodo.Select();
            nodo.Focus();            
            Consulta_EnBD(nodo.SelectedNode.Name);
            activarDesactivarEdicion(false, Color.WhiteSmoke);
            tbxFind.Enabled = true;
            button1.Enabled = true;
        }
                    
        #endregion

        #region Activacion de controles en la vista
        void activarDesactivarEdicion(bool pbActivar, Color pbColor)
        {
            tbxvendedor.Enabled = pbActivar;
            tbxvendedor.BackColor = pbColor;
            tbxdescripcion.Enabled = pbActivar;
            tbxdescripcion.BackColor = pbColor;
        }
              
        private void limpiezaTextBoxes()
        {
            activarDesactivarEdicion(true, Color.White);
            tbxvendedor.Clear();
            tbxdescripcion.Clear();
            tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);
            tbxvendedor.Text = muestraAutoincrementoId();
            idvend = tbxvendedor.Text;
            tbxvendedor.Focus();
        }
        #endregion

        #region Captura y validacion

        #region evento de vendedores
        private void tbxvendedor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { this.tbxdescripcion.Focus(); }
        }
        private void tbxvendedor_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxvendedor.BackColor;
            tbxvendedor.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxvendedor_Leave(object sender, EventArgs e)
        {
            tbxvendedor.BackColor = colorbefore;
        }
        private void tbxvendedor_Validated(object sender, EventArgs e)
        {

        }
        #endregion

        #region evento de descripcion

        private void tbxdescripcion_Validated(object sender, EventArgs e)
        {

        }
        private void tbxdescripcion_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxdescripcion.BackColor;
            tbxdescripcion.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxdescripcion_Leave(object sender, EventArgs e)
        {
            OleDbDataReader olplu = Conec.Obtiene_Dato("Select Nombre  From Oferta Where TRIM(Nombre) = '" + tbxdescripcion.Text.Trim() +"'", Conec.CadenaConexion);
            if (olplu.Read())
            {
                MessageBox.Show(this, Variable.SYS_MSJ[277, Variable.idioma]);
                this.tbxdescripcion.Focus();
            }
            else tbxdescripcion.BackColor = colorbefore;
            olplu.Close();
        }
        private void tbxdescripcion_KeyPress(object sender, KeyPressEventArgs e)
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
        #endregion

        #endregion

        #region Evento de botones toolbar.
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
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            bool error = false;
            if (tbxvendedor.Text == "" || tbxvendedor.Text == "0")
            {
                MessageBox.Show(this, Variable.SYS_MSJ[238, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbxvendedor.Focus();
                error = true;
            }
            if (tbxdescripcion.Text == "" || tbxdescripcion.Text == "0")
            {
                MessageBox.Show(this, Variable.SYS_MSJ[77, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbxdescripcion.Focus();
                error = true;
            }

            idvend = tbxvendedor.Text.Trim();

            if (Form1.statRegistro == ESTADO.EstadoRegistro.PKNOTRATADO)
            {
                OleDbDataReader olplu2 = Conec.Obtiene_Dato("Select id_vendedor  From Vendedor Where id_vendedor = " + Convert.ToInt32(tbxvendedor.Text) + " AND borrado = " + false, Conec.CadenaConexion);
                if (olplu2.Read())
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[375, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    error = true;
                }
                else
                {
                    OleDbDataReader olplu4 = Conec.Obtiene_Dato("Select id_vendedor  From Vendedor Where id_vendedor = " + Convert.ToInt32(tbxvendedor.Text) + " AND borrado = " + true, Conec.CadenaConexion);
                    if (olplu4.Read())
                    {
                        iActionToSave = 1;
                    }
                    olplu4.Close();

                    OleDbDataReader olplu = Conec.Obtiene_Dato("Select Nombre  From Vendedor Where Nombre = '" + tbxdescripcion.Text.Trim() + "' AND borrado = " + false + " AND id_vendedor <> " + Convert.ToInt32(tbxvendedor.Text), Conec.CadenaConexion);
                    if (olplu.Read())
                    {
                        MessageBox.Show(this, Variable.SYS_MSJ[277, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.tbxdescripcion.Focus();
                        error = true;
                    }
                    olplu.Close();
                }                
                olplu2.Close();
            }
            else
            {
                OleDbDataReader olplu = Conec.Obtiene_Dato("Select Nombre  From Vendedor Where Nombre = '" + tbxdescripcion.Text.Trim() + "' AND borrado = " + false + " AND id_vendedor <> " + Convert.ToInt32(tbxvendedor.Text), Conec.CadenaConexion);
                if (olplu.Read())
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[277, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.tbxdescripcion.Focus();
                    error = true;
                }
                else
                {
                    iActionToSave = 1;
                }
                olplu.Close();
            
            }

            if (!error)
            {
                //OleDbDataReader olplu = Conec.Obtiene_Dato("Select id_vendedor  From Vendedor Where id_vendedor = " + Convert.ToInt32(tbxvendedor.Text), Conec.CadenaConexion);
                //if (olplu.Read())
                //{
                //    iActionToSave = 1;
                //}
                //else iActionToSave = 0;
                //olplu.Close();

                Guardar(iActionToSave);

                activarDesactivarEdicion(false, Color.WhiteSmoke);

                listadoId_EnBD("borrado = " + false);
                Form1.statRegistro = ESTADO.EstadoRegistro.PKTRATADO;

                treListadoV.TabStop = true;
                tbxFind.TabStop = true;
                button1.TabStop = true;
                panel1.TabStop = true;

                MessageBox.Show(this, Variable.SYS_MSJ[347, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbxFind.Focus();

                tbxFind.Enabled = true;
                button1.Enabled = true;
            }
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            iActionToSave = 1;
            activarDesactivarEdicion(true, Color.White);
            tbxvendedor.Enabled = false;
            tbxdescripcion.Focus();
            Form1.statRegistro = ESTADO.EstadoRegistro.PKPARCIAL;
            btnEditar.Enabled = false;
            btnGuardar.Enabled = true;
            btnBorrar.Enabled = false;

            treListadoV.TabStop = false;
            tbxFind.TabStop = false;
            button1.TabStop = false;
            panel1.TabStop = false;
            tbxFind.Enabled = false;
            button1.Enabled = false;
            tbxdescripcion.Focus();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            limpiezaTextBoxes();
            Form1.statRegistro = ESTADO.EstadoRegistro.PKNOTRATADO;
            btnEditar.Enabled = false;
            btnBorrar.Enabled = false;
            btnGuardar.Enabled = true;
            btnNuevo.Enabled = false;
            iActionToSave = 0;

            treListadoV.TabStop = false;
            tbxFind.TabStop = false;
            button1.TabStop = false;
            panel1.TabStop = false;
            tbxFind.Enabled = false;
            button1.Enabled = false;
            tbxvendedor.Focus();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            ToolStripManager.RevertMerge("toolStrip2");
            this.Dispose();
        }

        private void btnDepurar_Click(object sender, EventArgs e)
        {
            UserDepurar uspurge = new UserDepurar((int)ESTADO.FileSource.fVendedores);
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

                    UsExport.exportar((int)ESTADO.FileSource.fVendedores, ref MyFileExp);

                    MyFileExp.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Registro de Base de Datos y DataSet
        private void Consulta_EnBD(string ncod)
        {
            DataRow dr = baseDeDatosDataSet.Vendedor.Rows.Find(Convert.ToInt64(ncod));
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
            idvend = dr["id_vendedor"].ToString();
            tbxvendedor.Text = dr["id_vendedor"].ToString();
            tbxdescripcion.Text = dr["Nombre"].ToString();
            tbxFecha.Text = String.Format(Variable.F_Hora, Convert.ToDateTime(dr["actualizado"].ToString())) + " " + String.Format(Variable.FOR_FECHAS[Variable.ffecha], Convert.ToDateTime(dr["actualizado"].ToString())); ;
            tbxdescripcion.Focus();
        }
        private void listadoId_EnBD(string condicion)
        {
            try
            {
                treListadoV.Nodes.Clear();
              
                vendedorTableAdapter.Fill(baseDeDatosDataSet.Vendedor);
                DataRow[] DRV = baseDeDatosDataSet.Vendedor.Select(condicion,"id_vendedor ASC");

                foreach (DataRow dr in DRV)  //baseDeDatosDataSet.Vendedor.Rows)
                {
                    treListadoV.Nodes.Add(dr["id_vendedor"].ToString(), dr["Nombre"].ToString());
                }               
                treListadoV.ExpandAll();

                if (treListadoV.Nodes.Count == 0)
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

            OleDbDataReader LP = Conec.Obtiene_Dato("SELECT id_vendedor FROM Vendedor ORDER BY id_vendedor desc", Conec.CadenaConexion);
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
                DataRow dr = baseDeDatosDataSet.Vendedor.NewRow();

                dr.BeginEdit();
                dr["id_vendedor"] = tbxvendedor.Text;
                dr["Nombre"] = tbxdescripcion.Text;
                dr["pendiente"] = false;
                dr.EndEdit();

                vendedorTableAdapter.Update(dr);
                baseDeDatosDataSet.AcceptChanges();

                Conec.CadenaSelect = "INSERT INTO Vendedor " +
                "(id_vendedor, Nombre, actualizado,pendiente)" +
                "VALUES (" + Convert.ToInt32(tbxvendedor.Text) + ",'" + tbxdescripcion.Text + "','" + fecha_act + "'," + false + ")";

                Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Vendedor.TableName);

            }
            if (iActionToSave == 1) 
            {
                DataRow dr = baseDeDatosDataSet.Vendedor.Rows.Find(Convert.ToInt32(idvend));

                dr.BeginEdit();
                dr["Nombre"] = tbxdescripcion.Text;                             
                dr["actualizado"] = fecha_act;
                dr["borrado"] = false;
                dr["pendiente"] = true;
                dr.EndEdit();

                vendedorTableAdapter.Update(dr);
                baseDeDatosDataSet.AcceptChanges();

                Conec.Condicion = "id_vendedor = " + Convert.ToInt32(idvend);
                Conec.CadenaSelect = "UPDATE Vendedor " +
                "SET Nombre = '" + tbxdescripcion.Text + "', actualizado = '" + fecha_act + "', pendiente = " + true + " WHERE ( " + Conec.Condicion + ")";

                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Vendedor.TableName);
            }
        }
        private void Eliminar()
        {
            bool borrar = false;

            DataRow dr = baseDeDatosDataSet.Vendedor.Rows.Find(Convert.ToInt32(idvend));

            if (dr != null && baseDeDatosDataSet.Vendedor.Rows.Count > 0)
            {
                if (MessageBox.Show(this, Variable.SYS_MSJ[93, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    borrar = true;
                    if (Conec.Obtiene_Dato("SELECT * FROM Ventas WHERE (id_vendedor = " + idvend + ")", Conec.CadenaConexion).Read())
                    {
                        if (MessageBox.Show(this, Variable.SYS_MSJ[242, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            borrar = true;
                        }
                    }
                    
                    if (borrar)
                    {
                        dr.Delete();
                        baseDeDatosDataSet.AcceptChanges();

                        Conec.Condicion = "id_vendedor = " + Convert.ToInt32(idvend);
                        Conec.CadenaSelect = "UPDATE Vendedor " +
                        "SET borrado = " + true + " WHERE ( " + Conec.Condicion + ")";

                        Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Vendedor.TableName);
                    }
                }
            }
            else
            {
                MessageBox.Show(this, Variable.SYS_MSJ[94, Variable.idioma], "", MessageBoxButtons.OK);
            }
        }
        #endregion

        private void UserVendedores_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Publicidad' Puede moverla o quitarla según sea necesario.
            this.publicidadTableAdapter.Fill(this.baseDeDatosDataSet.Publicidad);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Vendedor' Puede moverla o quitarla según sea necesario.
            this.vendedorTableAdapter.Fill(this.baseDeDatosDataSet.Vendedor);

           
            activarDesactivarEdicion(false, Color.WhiteSmoke);
        }
        private void UserVendedores_Activated(object sender, EventArgs e)
        {           
            if (treListadoV.Nodes.Count > 0)
            {
                activarDesactivarEdicion(true, Color.White);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            vendedorTableAdapter.Fill(baseDeDatosDataSet.Vendedor);

            listadoId_EnBD("Nombre Like '*" + tbxFind.Text + "*'");
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
                        IMP.importar((int)ESTADO.FileSource.fVendedores, ref MyFile, nombre_archivo);

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

        private void tbxvendedor_KeyPress(object sender, KeyPressEventArgs e)
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
