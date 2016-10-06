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
    public partial class UserGrupos : UserControl
    {
        #region Declaracion Class
        ADOutil Conec = new ADOutil();
        #endregion

        #region Declaracion Constantes y Variables
        public string idgrup;
        Color colorbefore;
        #endregion

        #region Inicio
        public UserGrupos()
        {
            InitializeComponent();        
            
        }
        #endregion 
               
       #region Activacion de controles en la vista
        void activarDesactivarEdicion(bool pbActivar, Color pbColor)
        {
            tbxNombre.Enabled = pbActivar;
            tbxNombre.BackColor = pbColor;
            tbxDireccion.Enabled = pbActivar;
            tbxDireccion.BackColor = pbColor;
            tbxDescripcion.Enabled = pbActivar;
            tbxDescripcion.BackColor = pbColor;       
        }
        
        private void limpiezaTextBoxes()
        {
            tbxGrupo.Clear();
            tbxNombre.Clear();
            tbxDireccion.Clear();
            tbxDescripcion.Clear();            
            activarDesactivarEdicion(true,Color.White);
            tbxGrupo.Text = muestraAutoincrementoId();
            tbxNombre.Focus();

        }
        #endregion

        #region Validacion y captura
        private void tbxGrupo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { this.tbxNombre.Focus(); }
        }

        private void tbxDescripcion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { this.tbxDireccion.Focus(); }
        }
        private void tbxNombre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { this.tbxDescripcion.Focus(); }
        }

        private void tbxDireccion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { }
        }


        private void tbxNombre_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) //KeyPressEventArgs e)
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

        private void tbxNombre_Enter(object sender, EventArgs e) //KeyPressEventArgs e)
        {
            colorbefore = tbxNombre.BackColor;
            tbxNombre.BackColor = Color.FromArgb(255, 255, 174);
        }

        private void tbxNombre_Leave(object sender, EventArgs e) //KeyPressEventArgs e)
        {
            tbxNombre.BackColor = colorbefore;
        }

        private void tbxDescrip_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) //KeyPressEventArgs e)
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

        private void tbxDescrip_Enter(object sender, EventArgs e) //KeyPressEventArgs e)
        {
            colorbefore = tbxDescripcion.BackColor;
            tbxDescripcion.BackColor = Color.FromArgb(255, 255, 174);
        }

        private void tbxDescrip_Leave(object sender, EventArgs e) //KeyPressEventArgs e)
        {
            tbxDescripcion.BackColor = colorbefore;
        }

        #endregion

        #region Registro de Base de Datos y DataSet
        private void Consulta_EnBD(string ncod)
        {
            DataRow dr = baseDeDatosDataSet.Grupo.Rows.Find(Convert.ToInt64(ncod));
            if (dr != null)
            {
                Mostrar_Dato(ref dr);
            }
        }

        private void Mostrar_Dato(ref DataRow dr)
        {
            idgrup = dr["id_grupo"].ToString();
            tbxGrupo.Text = dr["id_grupo"].ToString();
            tbxNombre.Text = dr["nombre_gpo"].ToString();
            tbxDescripcion.Text = dr["descripcion"].ToString();
            tbxDireccion.Text = dr["direccion"].ToString();
            tbxNombre.Focus();
        }

        private void listadoId_EnBD()
        {
            try
            {
             
                Conec.CadenaSelect = "SELECT * FROM Grupo ORDER BY id_grupo";
              
                grupoTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                grupoTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                grupoTableAdapter.Fill(baseDeDatosDataSet.Grupo);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Excepcion " + ex);
            }
        }

        private string muestraAutoincrementoId()
        {           
            int cod = 0;

            OleDbDataReader LP = Conec.Obtiene_Dato("SELECT id_grupo FROM GRUPO ORDER BY id_grupo desc", Conec.CadenaConexion);
            if (LP.Read()) cod = Convert.ToInt32(LP.GetValue(0));
            LP.Close();
            return Convert.ToString(cod + 1);
        }

        private int Guardar_Grupo(int iActionToSave)
        {
            string Fecha_act = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);

            if (tbxNombre.Text.Length == 0)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[59, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 2;
            }

            grupoTableAdapter.Fill(baseDeDatosDataSet.Grupo);


            if (iActionToSave == 0)        //Nuevo grupo
            {
                Conec.CadenaSelect = "INSERT INTO Grupo " +
                "(id_grupo,nombre_gpo, descripcion, direccion, actualizado)" +
                "VALUES (" + Convert.ToInt32(tbxGrupo.Text) + ",'" +
                tbxNombre.Text + "','" +
                tbxDescripcion.Text + "','" +
                tbxDireccion.Text + "','" +
                Fecha_act + "')";

                Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Productos.TableName);

                //bool Existe = true;
                string fecha_act = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);

                Variable.user_contrastepapel = 6;
                Variable.user_contrasteetiqueta = 6;
                Variable.user_formato.medio_imp = 0;
                Variable.user_formato.for_papel_tipoimpre = 0;
                Variable.user_formato.for_ecsep_tipoimpre = 0;
                Variable.user_EAN_UPCxProd = 0;
                Variable.user_EAN_UPCxTicket = 0;
                Variable.user_formato.ncodigobar_xprod = 0;
                Variable.user_formato.ncodigobar_xticket = 0;
                Variable.user_codigoxticket = ""; 
                Variable.user_codigoxprod = "";
                Variable.user_corrimientopieza = 1; 
                Variable.user_prefijo = "0";
                Variable.user_depto = "0";
                Variable.user_retardoimpresion = 0;
                Variable.user_nutri = 0; 
                Variable.user_Nformato_ticket = 0;
                Variable.user_Nformato_producto = 0;

                //if (!Existe)
                //{
                    Conec.CadenaSelect = "INSERT INTO Impresor " +
                    "(id_bascula, id_grupo, c_papel, c_etiqueta, tipoimp, formato_papel, formato_etiq, EAN_etiq,EAN_papel,barcode_prod,barcode_ticket, barcode_personal_ticket," +
                    "barcode_personal_prod,cero_pieza,prefijo,departamento,retardo,nutrientes,f_personalizado_papel, f_personalizado_etiq,actualizado,pendiente)" +
                    "VALUES (" + "0," +
                        Convert.ToInt32(tbxGrupo.Text) + "," +
                        Variable.user_contrastepapel + "," +
                        Variable.user_contrasteetiqueta + "," +
                        Variable.user_formato.medio_imp + "," +
                        Variable.user_formato.for_papel_tipoimpre + "," +
                        Variable.user_formato.for_ecsep_tipoimpre + "," +
                        Variable.user_EAN_UPCxProd + "," +
                        Variable.user_EAN_UPCxTicket + "," +
                        Variable.user_formato.ncodigobar_xprod + "," +
                        Variable.user_formato.ncodigobar_xticket + ",'" +
                        Variable.user_codigoxticket + "','" +
                        Variable.user_codigoxprod + "'," +
                        Variable.user_corrimientopieza + "," +
                        Convert.ToInt16(Variable.user_prefijo) + "," +
                        Convert.ToInt16(Variable.user_depto) + "," +
                        Variable.user_retardoimpresion + "," +
                        Variable.user_nutri + "," +
                        Variable.user_Nformato_ticket + "," +
                        Variable.user_Nformato_producto + ",'" +
                        fecha_act + "'," +
                        false + ")";

                    Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Impresor.TableName);

                /*}
                if (Existe)
                {
                    Conec.Condicion = "id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo;
                    Conec.CadenaSelect = "UPDATE Impresor " +
                    "SET c_papel = " + Variable.user_contrastepapel +
                    ", c_etiqueta = " + Variable.user_contrasteetiqueta +
                    ", tipoimp = " + Variable.user_formato.medio_imp +
                    ", formato_papel = " + Variable.user_formato.for_papel_tipoimpre +
                    ", formato_etiq = " + Variable.user_formato.for_ecsep_tipoimpre +
                    ", EAN_etiq = " + Variable.user_EAN_UPCxProd +
                    ", EAN_papel = " + Variable.user_EAN_UPCxTicket +
                    ", barcode_prod = " + Variable.user_formato.ncodigobar_xprod +
                    ", barcode_ticket= " + Variable.user_formato.ncodigobar_xticket +
                    ", barcode_personal_ticket = '" + Variable.user_codigoxticket +
                    "',barcode_personal_prod = '" + Variable.user_codigoxprod +
                    "',cero_pieza = " + Variable.user_corrimientopieza +
                    ", prefijo =" + Convert.ToInt16(Variable.user_prefijo) +
                    ", departamento = " + Convert.ToInt16(Variable.user_depto) +
                    ", retardo = " + Variable.user_retardoimpresion +
                    ", nutrientes = " + Variable.user_nutri +
                    ",f_personalizado_papel = " + Variable.user_Nformato_ticket +
                    ",f_personalizado_etiq = " + Variable.user_Nformato_producto +
                    ", pendiente = " + true +
                    ", actualizado = '" + fecha_act +
                    "' WHERE (" + Conec.Condicion + ")";

                    Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Impresor.TableName);
                    impresorTableAdapter.Fill(baseDeDatosDataSet.Impresor);
                }*/

                    MessageBox.Show(this, Variable.SYS_MSJ[60, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else        //Grupo existente
            {
                DataRow dr = baseDeDatosDataSet.Grupo.Rows.Find(Convert.ToInt32(idgrup));

                dr.BeginEdit();
                dr["nombre_gpo"] = tbxNombre.Text;
                dr["descripcion"] = tbxDescripcion.Text;
                dr["actualizado"] = Fecha_act;
                dr.EndEdit();

                grupoTableAdapter.Update(dr);
                baseDeDatosDataSet.AcceptChanges();

                Conec.Condicion = "id_grupo = " + Convert.ToInt32(idgrup);
                Conec.CadenaSelect = "UPDATE Grupo " +
                "SET nombre_gpo = '" + tbxNombre.Text +
                "', descripcion = '" + tbxDescripcion.Text +
                "', actualizado = '" + Fecha_act +
                "' WHERE ( " + Conec.Condicion + ")";

                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Productos.TableName);

                MessageBox.Show(this, Variable.SYS_MSJ[61, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);

                return 1;
            }

            return 0;
        }

        private void Eliminar()
        {
            bool borrar = false;

            DataRow dr = baseDeDatosDataSet.Grupo.Rows.Find(Convert.ToInt32(idgrup));

            if (dr != null && baseDeDatosDataSet.Grupo.Rows.Count > 0)
            {
                if (MessageBox.Show(this, Variable.SYS_MSJ[239, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    borrar = true;
                    if (Conec.Obtiene_Dato("SELECT * FROM Bascula WHERE (id_grupo = " + idgrup + ")", Conec.CadenaConexion).Read())
                    {
                        MessageBox.Show(this, Variable.SYS_MSJ[240, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                        borrar = false;
                    }
                    else
                    {

                        if (Conec.Obtiene_Dato("SELECT * FROM Prod_detalle WHERE (id_grupo = " + idgrup + ")", Conec.CadenaConexion).Read())
                        {
                            MessageBox.Show(this, Variable.SYS_MSJ[341, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);  //la bascula tiene productos asignado no es posible borrarla
                            borrar = false;
                        }
                    }

                    if (borrar)
                    {
                        dr.Delete();
                        baseDeDatosDataSet.AcceptChanges();

                        Conec.Condicion = "id_grupo = " + Convert.ToInt32(idgrup);
                        Conec.CadenaSelect = "DELETE * FROM Grupo WHERE (" + Conec.Condicion + ")";

                        Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Grupo.TableName);

                        Conec.Condicion = "id_grupo = " + Convert.ToInt32(idgrup);
                        Conec.CadenaSelect = "DELETE * FROM Impresor WHERE (" + Conec.Condicion + ")";

                        Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Impresor.TableName);

                        string sele = "DELETE * FROM carpeta_detalle WHERE ( id_grupo = " + Convert.ToInt32(idgrup) + ")";
                        Conec.EliminarReader(Conec.CadenaConexion, sele, "carpeta_detalle");

                        bool existe;
                        OleDbDataReader OlHead = Conec.Obtiene_Dato("Select * From Encabezado Where id_bascula = 0 and id_grupo = " + Convert.ToInt32(idgrup), Conec.CadenaConexion);
                        if (OlHead.Read()) existe = true;
                        else existe = false;
                        OlHead.Close();

                        if (existe)
                        {
                            Conec.Condicion = "id_bascula = 0 and id_grupo = " + Convert.ToInt32(idgrup);

                            Conec.CadenaSelect = "DELETE * FROM Encabezado WHERE (" + Conec.Condicion + ")";
                            Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Configuracion.TableName);
                        }

                        OleDbDataReader OlTex = Conec.Obtiene_Dato("Select * From Textos Where id_bascula = 0 and id_grupo = " + Convert.ToInt32(idgrup), Conec.CadenaConexion);
                        if (OlTex.Read()) existe = true;
                        else existe = false;
                        OlTex.Close();

                        if (existe)
                        {
                            Conec.Condicion = "id_bascula = 0 and id_grupo = " + Convert.ToInt32(idgrup);

                            Conec.CadenaSelect = "DELETE * FROM Textos WHERE (" + Conec.Condicion + ")";
                            Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Textos.TableName);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(this, Variable.SYS_MSJ[62, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region Eventos de Botones

        public bool comando(int opcion, int iActionToSave)
        {
            int irtaFunc = 0;

            switch (opcion)
            {
                case 1:  //crear nuevo
                    limpiezaTextBoxes();
                    Form1.statRegistro = ESTADO.EstadoRegistro.PKNOTRATADO;
                    break;
                case 2: //editar o mostrar registro
                    activarDesactivarEdicion(true,Color.White);
                    Consulta_EnBD(idgrup);
                    Form1.statRegistro = ESTADO.EstadoRegistro.PKPARCIAL;
                    break;
                case 3: //guardar registro

                    bool bNombre = false;

                    grupoTableAdapter.Fill(baseDeDatosDataSet.Grupo);

                    DataRow[] drr = baseDeDatosDataSet.Grupo.Select("nombre_gpo = '" + tbxNombre.Text + "'");

                    foreach (DataRow drbas in drr)
                    {
                        if (drbas["id_grupo"].ToString() != idgrup && drbas["nombre_gpo"].ToString() == tbxNombre.Text)
                        {
                            bNombre = true;
                            break;
                        }
                    }

                    if (bNombre == true)
                    {
                        if (iActionToSave == 0)
                        {
                            MessageBox.Show(this, Variable.SYS_MSJ[63, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        else
                        {
                            MessageBox.Show(this, Variable.SYS_MSJ[340, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        
                        return false;
                    }

                    irtaFunc = Guardar_Grupo(iActionToSave);

                    if (irtaFunc > 1)
                    {
                        return false;
                    }
                    //if (Form1.statRegistro == ESTADO.EstadoRegistro.PKNOTRATADO) limpiezaTextBoxes();
                    //if (Form1.statRegistro == ESTADO.EstadoRegistro.PKPARCIAL) activarDesactivarEdicion(false,Color.WhiteSmoke);
                    //Form1.statRegistro = ESTADO.EstadoRegistro.PKTRATADO;
                    listadoId_EnBD();
                    break;
                case 4: // eliminar registro
                    Eliminar();
                   // Form1.statRegistro = ESTADO.EstadoRegistro.PKTRATADO;
                    listadoId_EnBD();
                    //limpiezaTextBoxes();
                    break;
                case 5:
                    this.Dispose();
                    break;
                case 6:
                    activarDesactivarEdicion(false,Color.WhiteSmoke);
                    Consulta_EnBD(idgrup);
                    break;
            }

            return true;
        }
               
        #endregion        

        private void UserGrupos_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet1.Grupo' Puede moverla o quitarla según sea necesario.
            this.grupoTableAdapter.Fill(this.baseDeDatosDataSet.Grupo);

            listadoId_EnBD();
        }

        private void tbxNombre_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.tbxDescripcion.Focus();
        }      
    }
}
