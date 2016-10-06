using System;
using System.Globalization;
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
    public partial class UserOfertas : UserControl
    {
        #region Declaracion Class
        ADOutil Conec = new ADOutil();
        #endregion

        #region Declaracion Constantes y Variables
        string idofer;
        int tipo_descuento = 0;
        private int iActionToSave = 0;
        Color colorbefore;
        #endregion

        #region Inicio
        public UserOfertas()
        {
            InitializeComponent();
            this.tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + string.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);
        }
        #endregion

        #region Procesos pushBotones
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

        #region Activacion de controles en la vista
        void activarDesactivarEdicion(bool pbActivar, Color pbColor)
        {
            //tbxOferta.Enabled = pbActivar;
            tbxNombre.Enabled = pbActivar;
            tbxNombre.BackColor = pbColor;
            dateTimeInicio.Enabled = pbActivar;
            dateTimeFin.Enabled = pbActivar;
            tbxDescuento.Enabled = pbActivar;
            tbxDescuento.BackColor = pbColor;
        }
        private void limpiezaTextBoxes()
        {
            tbxOferta.Clear();
            tbxNombre.Clear();
            dateTimeInicio.Value = DateTime.Now;
            dateTimeFin.Value = DateTime.Now;
            tipo_descuento = 0;
            tbxDescuento.Clear();
            tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);
            activarDesactivarEdicion(true, Color.White);
            tbxOferta.Text = muestraAutoincrementoId();
            idofer = tbxOferta.Text;
            tbxNombre.Focus();
        }
        #endregion

        #region Registro de Base de Datos y DataSet
        private void Consulta_EnBD(string ncod)
        {
            DataRow dr = baseDeDatosDataSet.Oferta.Rows.Find(Convert.ToInt32(ncod));
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
            DateTime fecha_inicio = Convert.ToDateTime(dr["fecha_inicio"].ToString());
            DateTime fecha_final = Convert.ToDateTime(dr["fecha_fin"].ToString());

            idofer = dr["id_oferta"].ToString();
            tbxOferta.Text = dr["id_oferta"].ToString();
            tbxNombre.Text = dr["nombre"].ToString();
            dateTimeInicio.Text = String.Format(Variable.FOR_FECHAS[Variable.ffecha],fecha_inicio);
            dateTimeFin.Text = String.Format(Variable.FOR_FECHAS[Variable.ffecha],fecha_final);
            tbxDescuento.Text = String.Format(Variable.F_Descuento, Convert.ToDouble(dr["Descuento"].ToString()));
            tbxFecha.Text = String.Format(Variable.F_Hora, Convert.ToDateTime(dr["actualizado"].ToString())) + " " + String.Format(Variable.FOR_FECHAS[Variable.ffecha], Convert.ToDateTime(dr["actualizado"].ToString()));
            tbxNombre.Focus();
        }
        private void listadoId_EnBD(string condicion)
        {
            try
            {
                treListadoO.Nodes.Clear();

                ofertaTableAdapter.Fill(baseDeDatosDataSet.Oferta);

                DataRow[] DRO = baseDeDatosDataSet.Oferta.Select(condicion, "id_oferta ASC");
                foreach (DataRow dr in DRO)
                {
                    treListadoO.Nodes.Add(dr["id_oferta"].ToString(), dr["nombre"].ToString());
                }

                treListadoO.ExpandAll();

                if (treListadoO.Nodes.Count == 0)
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

            OleDbDataReader LP = Conec.Obtiene_Dato("SELECT id_oferta FROM OFERTA ORDER BY id_oferta desc", Conec.CadenaConexion);
            if (LP.Read()) cod = Convert.ToInt32(LP.GetValue(0));
            LP.Close();
            return Convert.ToString(cod + 1);

            //hacer una consulta para saber el ultimo id registrado + aunmetar 
            //en 1 para sabe cual es e siguente en generar
        }
        private void Guardar(int iActionToSave)
        {
            string fecha_act = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);
            tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);
   
            if (iActionToSave == 0)
            {
                DataRow dr = baseDeDatosDataSet.Oferta.NewRow();

                dr.BeginEdit();
                dr["id_oferta"] = tbxOferta.Text;
                dr["nombre"] = Variable.validar_salida(tbxNombre.Text, 8);
                dr["fecha_inicio"] = String.Format(Variable.F_Fecha, dateTimeInicio.Value) + " 00:00:00";  //.ToShortDateString();
                dr["fecha_fin"] = String.Format(Variable.F_Fecha,dateTimeFin.Value) + " 23:59:59";  //.ToShortDateString();
                dr["tipo_desc"] = tipo_descuento.ToString();
                dr["Descuento"] = tbxDescuento.Text;
                dr["pendiente"] = false;
                dr.EndEdit();

                ofertaTableAdapter.Update(dr);
                baseDeDatosDataSet.AcceptChanges();

                Conec.CadenaSelect = "INSERT INTO Oferta " +
                "(id_oferta,nombre,fecha_inicio,fecha_fin,Descuento,pendiente,actualizado)" +
               "VALUES (" + Convert.ToInt64(tbxOferta.Text) + ",'" + dr["nombre"].ToString() + "','" + dr["fecha_inicio"].ToString() +
               "','" + dr["fecha_fin"].ToString() + "'," + Convert.ToDecimal(tbxDescuento.Text) + "," +
               false + ",'" + fecha_act + "')";

                Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Oferta.TableName);
            }
            if (iActionToSave == 1)
            {
                DataRow dr = baseDeDatosDataSet.Oferta.Rows.Find(Convert.ToInt32(idofer));

                dr.BeginEdit();
                dr["nombre"] = Variable.validar_salida(tbxNombre.Text, 8);
                dr["fecha_inicio"] = String.Format(Variable.F_Fecha, dateTimeInicio.Value) + " 00:00:00";
                dr["fecha_fin"] = String.Format(Variable.F_Fecha, dateTimeFin.Value) + " 23:59:59";
                dr["tipo_desc"] = tipo_descuento.ToString();
                dr["Descuento"] = tbxDescuento.Text;
                dr["pendiente"] = true;
                dr.EndEdit();

                ofertaTableAdapter.Update(dr);
                baseDeDatosDataSet.Oferta.AcceptChanges();

                Conec.Condicion = "id_oferta = " + Convert.ToInt32(idofer);
                Conec.CadenaSelect = "UPDATE Oferta " +
                "SET fecha_inicio = '" + dr["fecha_inicio"] +
                "', fecha_fin = '" + dr["fecha_fin"] +
                "', tipo_desc = " + tipo_descuento +
                ", Descuento = " + Convert.ToDecimal(tbxDescuento.Text) +
                ", pendiente = " + true +
                ", actualizado = '" + fecha_act +
                "', nombre = '" + dr["nombre"].ToString() + 
                "' WHERE ( " + Conec.Condicion + ")";

                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Oferta.TableName);
            }
        }
        private void Eliminar()
        {
            bool borrar = false;

            DataRow dr = baseDeDatosDataSet.Oferta.Rows.Find(Convert.ToInt32(idofer));

            if (dr != null && baseDeDatosDataSet.Oferta.Rows.Count > 0)
            {
                if (MessageBox.Show(this, Variable.SYS_MSJ[68, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    borrar = true;
                    if (Conec.Obtiene_Dato("SELECT * FROM Oferta_Detalle WHERE (id_oferta = " + Convert.ToInt32(idofer) + ")", Conec.CadenaConexion).Read())
                    {
                        if (MessageBox.Show(this, Variable.SYS_MSJ[69, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            borrar = true;
                        }
                    }
                    if (Conec.Obtiene_Dato("SELECT * FROM Productos WHERE (oferta = " + Convert.ToInt32(idofer) + ")", Conec.CadenaConexion).Read())
                    {
                        if (MessageBox.Show(this, Variable.SYS_MSJ[70, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            borrar = true;
                        }
                    }
                    if (borrar)
                    {
                        dr.Delete();
                        baseDeDatosDataSet.AcceptChanges();

                        Conec.Condicion = "id_oferta = " + Convert.ToInt32(idofer);
                        Conec.CadenaSelect = "UPDATE Oferta SET borrado = " + true + " WHERE (" + Conec.Condicion + ")";

                        Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Oferta.TableName);

                        Conec.Condicion = "oferta = " + Convert.ToInt32(idofer);
                        Conec.CadenaSelect = "UPDATE Productos SET oferta = 0, pendiente = " + true + " WHERE (" + Conec.Condicion + ")";

                        Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Productos.TableName);
                    }
                }
            }
            else
            {
                MessageBox.Show(this, Variable.SYS_MSJ[72, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region Captura y validacion

        #region evento de Oferta
        private void tbxOferta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.tbxNombre.Focus();
        }
        private void tbxOferta_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxOferta.BackColor;
            tbxOferta.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxOferta_Leave(object sender, EventArgs e)
        {
            tbxOferta.BackColor = colorbefore;
        }
        private void tbxOferta_Validated(object sender, EventArgs e)
        {
            string dat = Variable.validar_salida(tbxOferta.Text, 0);
            if (dat != "")
            {
                tbxOferta.Text = dat;
            }
            else tbxOferta.Focus();
        }
        #endregion

        #region evento de fecha de inicio y termino
        private void dateTimeInicio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.dateTimeFin.Focus();
        }
        private void dateTimeFin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.tbxDescuento.Focus();
        }
        private void dateTimeInicio_Enter(object sender, EventArgs e)
        {
            colorbefore = dateTimeInicio.BackColor;
            dateTimeInicio.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void dateTimeInicio_Leave(object sender, EventArgs e)
        {                       
            dateTimeInicio.BackColor = colorbefore;
        }
        private void dateTimeFin_Enter(object sender, EventArgs e)
        {
            colorbefore = dateTimeFin.BackColor;
            dateTimeFin.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void dateTimeFin_Leave(object sender, EventArgs e)
        {
            dateTimeFin.BackColor = colorbefore;
        }
        #endregion

        #region evento de nombre
        private void tbxNombre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.dateTimeInicio.Focus();
        }
        private void tbxNombre_Validated(object sender, EventArgs e)
        {

        }
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
            if (Char.IsNumber(e.KeyChar) || e.KeyChar == '.')
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

        #region Evento de descuento
        private void tbxDescuento_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && tbxDescuento.Text != "")
            {               
                tbxDescuento.Text = string.Format(Variable.F_Descuento, Convert.ToDecimal(tbxDescuento.Text));
                this.tbxNombre.Focus();
            }
        }
        private void tbxDescuento_Validated(object sender, EventArgs e)
        {
            int cad = Variable.iValidarImpuesto(tbxDescuento.Text);
            if (tbxDescuento.Text != "")
            {
                if (cad == 0)
                {
                    tbxDescuento.Text = string.Format(Variable.F_Descuento, Convert.ToDouble(tbxDescuento.Text));
                }
            }          
        }       
        private void tbxDescuento_KeyPress(object sender, KeyPressEventArgs e)
        {
            int iCountDot = tbxDescuento.Text.Count(c => c == '.');
            int iPosDot = tbxDescuento.Text.IndexOf('.');

            if (e.KeyChar == '.')
            {
                if (iCountDot > 0)
                {
                    e.Handled = true;
                }
                else if (tbxDescuento.Text.Count() > 0 && tbxDescuento.Text.Count() < 3)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            else if (Char.IsDigit(e.KeyChar))
            {
                if (tbxDescuento.Text.Count() < 2 && iCountDot == 0)
                {
                    e.Handled = false;
                }
                else if (iCountDot == 1)
                {
                    if (iPosDot == 1)
                    {
                        if (tbxDescuento.Text.Count() < 4)
                        {
                            e.Handled = false;
                        }
                        else
                        {
                            e.Handled = true;
                        }
                    }
                    else if (iPosDot == 2)
                    {
                        if (tbxDescuento.Text.Count() < 5)
                        {
                            e.Handled = false;
                        }
                        else
                        {
                            e.Handled = true;
                        }
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
                else
                {
                    e.Handled = true;
                } 
            }
            else if (e.KeyChar != (char)Keys.Enter && e.KeyChar != (char)Keys.Tab && e.KeyChar != 8)
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }
        private void tbxDescuento_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxDescuento.BackColor;
            tbxDescuento.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxDescuento_Leave(object sender, EventArgs e)
        {
            tbxDescuento.BackColor = colorbefore;
        }
        #endregion

        #region Eventos de Botones

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            limpiezaTextBoxes();
            Form1.statRegistro = ESTADO.EstadoRegistro.PKNOTRATADO;
            btnEditar.Enabled = false;
            btnBorrar.Enabled = false;
            btnGuardar.Enabled = true;
            btnNuevo.Enabled = false;
            iActionToSave = 0;

            treListadoO.TabStop = false;
            tbxFind.TabStop = false;
            button1.TabStop = false;
            panel1.TabStop = false;

            tbxFind.Enabled = false;
            button1.Enabled = false;

            tbxNombre.Focus();
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

            treListadoO.TabStop = false;
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
            if (tbxDescuento.Text == "0")//tbxOferta.Text == "" || tbxOferta.Text == "0")
            {
                MessageBox.Show(this, Variable.SYS_MSJ[394, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbxOferta.Focus();
                error = true;
            }
            else if (tbxNombre.Text == "" || tbxNombre.Text == "0")
            {
                MessageBox.Show(this, Variable.SYS_MSJ[77, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbxNombre.Focus();
                error = true;
            }
            else if (tbxDescuento.Text.Length <= 0)  // || Variable.iValidarImpuesto(tbxDescuento.Text) > 0)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[393, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbxDescuento.Focus();
                error = true;
            }
            else if (dateTimeInicio.Value.Date < DateTime.Now.Date)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[73, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //"La fecha inicial no menor que la actual", "Alerta!!!");
                dateTimeInicio.Focus();
                error = true;
            }
            else if (dateTimeFin.Value.Date < DateTime.Now.Date)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[75, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //"La fecha final no puede ser menor que la actual", "Alerta!!!");
                dateTimeFin.Focus();
                error = true;
            }
            else if (dateTimeFin.Value.Date < dateTimeInicio.Value.Date)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[76, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //"La fecha final no puede ser menor que la fecha inicial", "Alerta!!!");
                dateTimeFin.Focus();
                error = true;
            }
           
            if (Form1.statRegistro == ESTADO.EstadoRegistro.PKNOTRATADO)
            {
                OleDbDataReader olplu = Conec.Obtiene_Dato("Select nombre From Oferta Where nombre = '" + tbxNombre.Text.Trim() + "'" + " AND borrado = " + false, Conec.CadenaConexion);
                if (olplu.Read())
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[264, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    error = true;
                }
                olplu.Close();
            }
            if (!error)
            {
                OleDbDataReader olplu = Conec.Obtiene_Dato("Select id_oferta From Oferta Where id_oferta = " + Convert.ToInt32(tbxOferta.Text), Conec.CadenaConexion);
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

                treListadoO.TabStop = true;
                tbxFind.TabStop = true;
                button1.TabStop = true;
                panel1.TabStop = true;
                MessageBox.Show(this, Variable.SYS_MSJ[346, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            button1.Enabled = true;
            tbxFind.Enabled = true;

        }
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            ToolStripManager.RevertMerge("toolStrip2");
            this.Dispose();
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

                    UsExport.exportar(3, ref MyFileExp);

                    MyFileExp.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnDepurar_Click(object sender, EventArgs e)
        {
            UserDepurar uspurge = new UserDepurar((int)ESTADO.FileSource.fOfertas);
            uspurge.ShowDialog(this);

            listadoId_EnBD("borrado = " + false);
            if (treListadoO.Nodes.Count > 0)
            {
                activarDesactivarEdicion(true, Color.White);
            }
        }
        private void tbnImportar_Click(object sender, EventArgs e)
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
                        IMP.importar((int)ESTADO.FileSource.fOfertas, ref MyFile, nombre_archivo);

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

        #endregion

        private void UserOfertas_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Oferta' Puede moverla o quitarla según sea necesario.
            this.ofertaTableAdapter.Fill(this.baseDeDatosDataSet.Oferta);
            listadoId_EnBD("borrado = " + false);
            // if (treListadoO.Nodes.Count > 0)
            // {
            activarDesactivarEdicion(false, Color.WhiteSmoke);
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ofertaTableAdapter.Fill(baseDeDatosDataSet.Oferta);
            listadoId_EnBD("nombre Like '*" + tbxFind.Text + "*'");
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
