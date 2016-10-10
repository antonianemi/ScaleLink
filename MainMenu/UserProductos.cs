using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MainMenu
{
    public partial class UserProductos :UserControl
    {
        #region Declaracion Class
        ADOutil Conec = new ADOutil();
        int iTypeResolution = 0;    //0 Baja, 1 Alta
        #endregion

        #region Declaracion Constantes y Variables
        string idprod;
        int tipo_producto;
        int tipo_precio;
        Color colorbefore;
        private int iActionToSave = 0;      //0-> Grupo o bascula nueva; 1-> Grupo o bascula editada.
        #endregion

        #region Inicio
        public UserProductos()
        {
            InitializeComponent();
            this.tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + string.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);

            treListadoP.TabStop = true;
            tbxFind.TabStop = true;
            button1.TabStop = true;
            panel1.TabStop = true;
            tbxFind.Focus();

            if (Variable.TypeScale == Variable.TipoBascula.WLABEL) { funcionWLABEL(); }//Funcion para configurar la aplicacionpara WLABEL

        }                                                


        void funcionWLABEL()
        {
            //Ocultar Potho
            //groupBox5.Enabled = false;
            // groupBox5.Visible = false;
            groupBox5.Text = "";
            btnLimpia.Enabled = false;
            btnLimpia.Visible = false;
            btnImagen.Enabled = false;
            btnImagen.Visible = false;

            tbxImagenAsignada.Enabled = false;
            tbxImagenAsignada.Visible = false;
            label15.Enabled = false;
            label15.Visible = false;
            picImagen.Enabled = false;
            picImagen.Visible = false;



            //Ocultar Mensajes
            groupBox2.Enabled = false;
            groupBox2.Visible = false;
            //groupBox5.BackgroundImage = MainMenu.Properties.Resources.PRODUCT;

            Panel panel = new Panel();
            panel.Size = groupBox5.Size;
            panel.Location = groupBox5.Location;
            panel.BackgroundImage= MainMenu.Properties.Resources.PRODUCT;

            this.pnldetalle.Controls.Remove(this.groupBox5);
            this.pnldetalle.Controls.Add(panel);

        }
        #endregion

        #region Procesos pushBotones
        private void treeListado_AfterSelect(object sender, TreeViewEventArgs e)
        {           
            TreeView nodo = (TreeView)sender;
            nodo.Select();
            nodo.Focus();             
            Consulta_EnBD(nodo.SelectedNode.Name);
            activarDesactivarEdicion(false, Color.WhiteSmoke);
            tbxFind.Enabled = true;
            button1.Enabled = true;
            iActionToSave = 0;
        }
        
        #endregion
        
        #region Activacion de controles en la vista
        void activarDesactivarEdicion(bool pbActivar, Color clActivar)
        {
            tbxCodigo.Enabled = pbActivar;
            tbxCodigo.BackColor = clActivar;
            tbxdescripcion.Enabled = pbActivar;
            tbxdescripcion.BackColor = clActivar;
            tbxPLU.Enabled = pbActivar;
            tbxPLU.BackColor = clActivar;
            tbxprecio.Enabled = pbActivar;
            tbxprecio.BackColor = clActivar;
            tbximp.Enabled = pbActivar;
            tbximp.BackColor = clActivar;
            tbxMultiplo.Enabled = pbActivar;
            tbxMultiplo.BackColor = clActivar;
            tbxTara.Enabled = pbActivar;
            tbxTara.BackColor = clActivar;
            tbxcaducidad.Enabled = pbActivar;
            tbxcaducidad.BackColor = clActivar;
            rBxPesado.Enabled = pbActivar;           
            rBxNopesado.Enabled = pbActivar;
            rBxEditable.Enabled = pbActivar;            
            rBxNoeditable.Enabled = pbActivar;
            
            cbxOferta.Enabled = pbActivar;
            cbxOferta.BackColor = clActivar;            
            cbxInfoadicional.Enabled = pbActivar;
            cbxInfoadicional.BackColor = clActivar;
            cbxPubli1.Enabled = pbActivar;
            cbxPubli1.BackColor = clActivar;
            cbxPubli2.Enabled = pbActivar;
            cbxPubli2.BackColor = clActivar;
            cbxPubli3.Enabled = pbActivar;
            cbxPubli3.BackColor = clActivar;
            cbxPubli4.Enabled = pbActivar;
            cbxPubli4.BackColor = clActivar;
           
            btnImagen.Enabled = pbActivar;
            btnLimpia.Enabled = pbActivar;
        }
        private void ActivarDesactivarMenu()
        {
            if (treListadoP.Nodes.Count == 0)
            {
                btnEditar.Enabled = false;
                btnBorrar.Enabled = false;
                btnGuardar.Enabled = false;
                btnNuevo.Enabled = true;
                btnImport.Enabled = true;
                btnExportar.Enabled = false;
                btnDepurar.Enabled = false;
            }
            else
            {
                btnEditar.Enabled = false;
                btnBorrar.Enabled = false;
                btnGuardar.Enabled = false;
                btnNuevo.Enabled = true;
                btnImport.Enabled = false;
                btnExportar.Enabled = true;
                btnDepurar.Enabled = true;
            }
        }
        private void limpiezaTextBoxes()
        {
            idprod = muestraAutoincrementoId("SELECT id_producto FROM PRODUCTOS ORDER BY id_producto desc");
            activarDesactivarEdicion(true, Color.White);
            tbxCodigo.Text = muestraAutoincrementoId("SELECT Codigo FROM PRODUCTOS ORDER BY Codigo desc").ToString();
            tbxPLU.Text = idprod;
            tbxdescripcion.Clear();
            tbxprecio.Clear();
            tbximp.Clear();
            tbxcaducidad.Clear();
            tbxMultiplo.Clear();
            tbxTara.Clear();
            rBxEditable.Checked = false;
            rBxNoeditable.Checked = false;
            rBxPesado.Checked = false;
            rBxNopesado.Checked = false;
            tbxImagenAsignada.Clear();
            this.picImagen.Image = null;
            tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);
            tbxCodigo.Focus();
        }

        #endregion

        #region Registro de Base de Datos y DataSet
        private void Consulta_EnBD(string ncod)
        {
            DataRow dr = baseDeDatosDataSet.Productos.Rows.Find(Convert.ToInt64(ncod));
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
            idprod = dr["id_producto"].ToString();
            tbxCodigo.Text = dr["Codigo"].ToString();
            tbxPLU.Text = dr["NoPlu"].ToString();
            tbxdescripcion.Text = dr["Nombre"].ToString();
            tbxprecio.Text=String.Format(Variable.F_Decimal,Convert.ToDouble(dr["Precio"].ToString()));
            tbximp.Text = String.Format(Variable.F_Decimal,Convert.ToDouble(dr["Impuesto"].ToString()));
            tbxcaducidad.Text=dr["CaducidadDias"].ToString();
            tbxFecha.Text = String.Format(Variable.F_Hora, Convert.ToDateTime(dr["actualizado"].ToString())) + " " + String.Format(Variable.FOR_FECHAS[Variable.ffecha], Convert.ToDateTime(dr["actualizado"].ToString())); ;
            tbxMultiplo.Text = dr["Mutiplo"].ToString();
            tbxImagenAsignada.Text = dr["imagen1"].ToString();
            tbxTara.Text = dr["tara"].ToString();

            if (Convert.ToInt16(dr["PrecioEditable"].ToString()) == 1) rBxEditable.Checked = true;
            else rBxNoeditable.Checked = true;
            if (Convert.ToInt16(dr["TipoId"].ToString()) == 1) rBxPesado.Checked = true;
            else rBxNopesado.Checked = true;

            if (Convert.ToInt32(dr["publicidad1"].ToString()) > 0) cbxPubli1.SelectedValue = Convert.ToInt32(dr["publicidad1"].ToString());
            else cbxPubli1.SelectedValue = 0;
            if (Convert.ToInt32(dr["publicidad2"].ToString()) > 0) cbxPubli2.SelectedValue = Convert.ToInt32(dr["publicidad2"].ToString());
            else cbxPubli2.SelectedValue = 0;
            if (Convert.ToInt32(dr["publicidad3"].ToString()) > 0) cbxPubli3.SelectedValue = Convert.ToInt32(dr["publicidad3"].ToString());
            else cbxPubli3.SelectedValue = 0;
            if (Convert.ToInt32(dr["publicidad4"].ToString()) > 0) cbxPubli4.SelectedValue = Convert.ToInt32(dr["publicidad4"].ToString());
            else cbxPubli4.SelectedValue = 0;
            if (Convert.ToInt32(dr["id_ingrediente"].ToString()) > 0) cbxInfoadicional.SelectedValue = Convert.ToInt32(dr["id_ingrediente"].ToString());
            else cbxInfoadicional.SelectedValue = 0;
            if (Convert.ToInt32(dr["oferta"].ToString()) > 0) cbxOferta.SelectedValue = Convert.ToInt32(dr["oferta"].ToString());
            else cbxOferta.SelectedValue = 0;

            this.picImagen.SizeMode = PictureBoxSizeMode.StretchImage;
            if (tbxImagenAsignada.Text != "")
            {
                if (File.Exists(tbxImagenAsignada.Text))
                {
                    this.picImagen.Image = new Bitmap(tbxImagenAsignada.Text);
                    this.picImagen.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else { this.picImagen.Image = null; }
            }
            else { this.picImagen.Image = null; }

            tbxdescripcion.Focus();
        }
     
        private void listadoId_EnBD(string condicion)
        {
            try
            {
                treListadoP.Nodes.Clear();
                
                productosTableAdapter.Fill(baseDeDatosDataSet.Productos);

                DataRow[] LDR = baseDeDatosDataSet.Productos.Select(condicion, "Codigo");//"Codigo LIKE '*" +elemento+"*'");
                foreach (DataRow dr in LDR)
                {
                    treListadoP.Nodes.Add(dr["id_producto"].ToString(), dr["Codigo"].ToString() + "-" + dr["Nombre"].ToString());
                }
                treListadoP.ExpandAll();
                treListadoP.Refresh();
                ActivarDesactivarMenu();
                /*if (treListadoP.Nodes.Count == 0)
                {
                    btnEditar.Enabled = false;
                    btnBorrar.Enabled = false;
                    btnGuardar.Enabled = false;
                    btnNuevo.Enabled = true;
                    btnImport.Enabled = true;
                    btnExportar.Enabled = false;
                    btnDepurar.Enabled = false;                    
                }
                else
                {
                    btnEditar.Enabled = false;
                    btnBorrar.Enabled = false;
                    btnGuardar.Enabled = false;
                    btnNuevo.Enabled = true;
                    btnImport.Enabled = false;
                    btnExportar.Enabled = true;
                    btnDepurar.Enabled = true;
                }*/
            }

            catch (Exception ex)
            {
                MessageBox.Show(this, "Excepcion " + ex);
            }
        }

        private string muestraAutoincrementoId(string CadeSelect)
        {           
            int cod = 0;
            //"SELECT id_producto FROM PRODUCTOS ORDER BY id_producto desc"
            OleDbDataReader LP = Conec.Obtiene_Dato(CadeSelect, Conec.CadenaConexion);
            if (LP.Read()) cod = Convert.ToInt32(LP.GetValue(0));
            LP.Close();
            return Convert.ToString(cod + 1);           
        }
        
        private void Guardar()
        {
            string fecha_act = "";
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            // Se guarda la imagen en el buffer
            if (this.picImagen.Image != null)
            {
                this.picImagen.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                // Se extraen los bytes del buffer para asignarlos como valor para el 
                // parámetro.
                byte[] dt_imagen = ms.GetBuffer();
            }
                        
            fecha_act = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);
            tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);
            if (tbximp.Text == "")
            {
                tbximp.Text = "0";
            }            
            if (tbxcaducidad.Text == "" )
            {
                tbxcaducidad.Text = "0";
            }            
            if (tbxMultiplo.Text == "")
            {
                tbxMultiplo.Text = "0";
            }           
            if (tbxTara.Text == "")
            {
                tbxTara.Text = "0";
            }
            if (Form1.statRegistro == ESTADO.EstadoRegistro.PKNOTRATADO)
            {
                DataRow dr = baseDeDatosDataSet.Productos.NewRow();

                dr.BeginEdit();
                dr["id_producto"] = idprod;
                dr["Codigo"] = tbxCodigo.Text;
                dr["NoPlu"] = tbxPLU.Text;
                dr["Nombre"] = Variable.validar_salida(tbxdescripcion.Text,2);
                dr["Precio"] = tbxprecio.Text;
                dr["Impuesto"] = tbximp.Text;
                dr["CaducidadDias"] = tbxcaducidad.Text;
                dr["actualizado"] = fecha_act; 
                dr["PrecioEditable"] = tipo_precio;
                dr["TipoId"] = tipo_producto;
                dr["Mutiplo"] = tbxMultiplo.Text;
                dr["tara"] = tbxTara.Text;
                dr["imagen1"] = tbxImagenAsignada.Text;
                if (cbxPubli1.SelectedValue != null) dr["publicidad1"] = cbxPubli1.SelectedValue.ToString();
                else dr["publicidad1"] = 0;
                if (cbxPubli2.SelectedValue != null) dr["publicidad2"] = cbxPubli2.SelectedValue.ToString();
                else dr["publicidad2"] = 0;
                if (cbxPubli3.SelectedValue != null) dr["publicidad3"] = cbxPubli3.SelectedValue.ToString();
                else dr["publicidad3"] = 0;
                if (cbxPubli4.SelectedValue != null) dr["publicidad4"] = cbxPubli4.SelectedValue.ToString();
                else dr["publicidad4"] = 0;
                if (cbxInfoadicional.SelectedValue != null) dr["id_ingrediente"] = cbxInfoadicional.SelectedValue.ToString();
                else dr["id_ingrediente"] = 0;                
                if (cbxOferta.SelectedValue != null) dr["oferta"] = cbxOferta.SelectedValue.ToString();
                else dr["oferta"] = 0;                
                dr["id_info_nutri"] = 0;
                dr["pendiente"] = false;
                dr.EndEdit();

                productosTableAdapter.Update(dr);
                baseDeDatosDataSet.AcceptChanges();

                Conec.CadenaSelect = "INSERT INTO Productos " +
                "(id_producto, id_ingrediente, id_info_nutri,Codigo, NoPlu, Nombre, Precio, PrecioEditable, TipoId, Impuesto, CaducidadDias, Mutiplo, publicidad1,publicidad2,publicidad3,publicidad4,actualizado,imagen1,oferta,pendiente,imagen,tara)" +  
               "VALUES (" + Convert.ToInt64(idprod) + "," + 
               Convert.ToInt32(dr["id_ingrediente"].ToString()) + "," +
               Convert.ToInt32(dr["id_info_nutri"].ToString()) + "," + 
               Convert.ToInt32(tbxCodigo.Text) + "," + Convert.ToInt32(tbxPLU.Text) +",'" + 
               Variable.validar_salida(tbxdescripcion.Text,2) + "'," + 
               Convert.ToDecimal(tbxprecio.Text) + "," + 
               tipo_precio + "," + 
               tipo_producto + "," +
               Convert.ToDecimal(dr["Impuesto"].ToString()) + "," +
               Convert.ToInt16(dr["CaducidadDias"].ToString()) + "," +
               Convert.ToInt16(dr["Mutiplo"].ToString()) + "," + 
               Convert.ToInt32(dr["publicidad1"].ToString()) + "," + 
               Convert.ToInt32(dr["publicidad2"].ToString()) + "," + 
               Convert.ToInt32(dr["publicidad3"].ToString()) + "," + 
               Convert.ToInt32(dr["publicidad4"].ToString()) + ",'" +
               fecha_act + "','" + 
               tbxImagenAsignada.Text + "'," + 
               Convert.ToInt32(dr["oferta"].ToString()) + "," + 
               false + "," + true + ",'" +
               dr["tara"].ToString() + "')";  

                Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Productos.TableName);
            }
            if (Form1.statRegistro == ESTADO.EstadoRegistro.PKPARCIAL) 
            {
                DataRow dr = baseDeDatosDataSet.Productos.Rows.Find(Convert.ToInt32(idprod));

                dr.BeginEdit();                
                //dr["Codigo"] = tbxCodigo.Text;
                dr["NoPlu"] = tbxPLU.Text;
                dr["Nombre"] = Variable.validar_salida(tbxdescripcion.Text,2);
                dr["Precio"] = tbxprecio.Text;                
                dr["Impuesto"] = tbximp.Text;
                dr["CaducidadDias"] = tbxcaducidad.Text;
                dr["actualizado"] = fecha_act;
                dr["PrecioEditable"] = tipo_precio;                
                dr["TipoId"] = tipo_producto;
                dr["Mutiplo"] = tbxMultiplo.Text;
                dr["imagen1"] = tbxImagenAsignada.Text;
                if (cbxPubli1.SelectedValue != null) dr["publicidad1"] = cbxPubli1.SelectedValue.ToString();
                else dr["publicidad1"] = 0;
                if (cbxPubli2.SelectedValue != null) dr["publicidad2"] = cbxPubli2.SelectedValue.ToString();
                else dr["publicidad2"] = 0;
                if (cbxPubli3.SelectedValue != null) dr["publicidad3"] = cbxPubli3.SelectedValue.ToString();
                else dr["publicidad3"] = 0;
                if (cbxPubli4.SelectedValue != null) dr["publicidad4"] = cbxPubli4.SelectedValue.ToString();
                else dr["publicidad4"] = 0;
                if (cbxInfoadicional.SelectedValue != null) dr["id_ingrediente"] = cbxInfoadicional.SelectedValue.ToString();
                else dr["id_ingrediente"] = 0;
                if (cbxOferta.SelectedValue != null) dr["oferta"] = cbxOferta.SelectedValue.ToString();
                else dr["oferta"] = 0;
                dr["id_info_nutri"] = 0;
                dr["tara"] = tbxTara.Text;
                dr["pendiente"] = true;
                dr.EndEdit();

//                productosTableAdapter.Update(dr);
                baseDeDatosDataSet.AcceptChanges();

                Conec.Condicion = "id_producto = " + Convert.ToInt32(idprod);
                Conec.CadenaSelect = "UPDATE Productos " +
                "SET NoPlu = " + Convert.ToInt32(tbxPLU.Text) +", Nombre = '" + Variable.validar_salida(tbxdescripcion.Text,2) + "', Precio = " + Convert.ToDecimal(tbxprecio.Text) +
                ",PrecioEditable = " + tipo_precio + ", TipoId = " + tipo_producto + ", Impuesto = " + Convert.ToDecimal(tbximp.Text) + ",CaducidadDias = " + Convert.ToInt16(tbxcaducidad.Text) +
                ", Mutiplo = " + Convert.ToInt16(tbxMultiplo.Text) + ", actualizado = '" + fecha_act + "', imagen1 = '" + tbxImagenAsignada.Text +           
                "',publicidad1 = " + Convert.ToInt32(dr["publicidad1"].ToString()) + ",publicidad2 = " + Convert.ToInt32(dr["publicidad2"].ToString()) + 
                ",publicidad3 = " + Convert.ToInt32(dr["publicidad3"].ToString()) + ",publicidad4 = " + Convert.ToInt32(dr["publicidad4"].ToString()) +
                ",oferta = " + Convert.ToInt32(dr["oferta"].ToString()) + ",id_ingrediente =" + Convert.ToInt32(dr["id_ingrediente"].ToString()) + 
                ",pendiente = " + true + ",imagen = " + true + ",tara = '" + tbxTara.Text + "', id_info_nutri = " + Convert.ToInt32(dr["id_info_nutri"].ToString()) + " WHERE (" + Conec.Condicion + ")";

                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Productos.TableName);                           
            }
        }

        private void Eliminar()
        {
            bool borrar = false;
            bool asignado = false;
            DataRow dr = baseDeDatosDataSet.Productos.Rows.Find(Convert.ToInt32(idprod));

            if (dr != null && baseDeDatosDataSet.Productos.Rows.Count > 0)
            {
                //"Este producto sera borrado, ¿Esta seguro?"
                if (MessageBox.Show(this, Variable.SYS_MSJ[78, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    borrar = true;
                    asignado = false;
                    if (Conec.Obtiene_Dato("SELECT * FROM Prod_detalle WHERE (id_producto = " + idprod + ")", Conec.CadenaConexion).Read())
                    {
                        if (MessageBox.Show(this, Variable.SYS_MSJ[209, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            borrar = true;
                            asignado = true;
                        }
                    }
                    if (Conec.Obtiene_Dato("SELECT * FROM PLU_detalle WHERE (id_producto = " + idprod + ")", Conec.CadenaConexion).Read())
                    {
                        if (MessageBox.Show(this, Variable.SYS_MSJ[209, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            borrar = true;
                            asignado = true;
                        }
                    }
                    if (Conec.Obtiene_Dato("SELECT * FROM Ventas_Detalle WHERE (codigo = " + Convert.ToInt32(dr["Codigo"].ToString()) + ")", Conec.CadenaConexion).Read())
                    {
                        if (MessageBox.Show(this, Variable.SYS_MSJ[209, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            borrar = true;
                            asignado = true;
                        }
                    }
                    if (borrar && asignado)
                    {
                        dr.Delete();
                        baseDeDatosDataSet.AcceptChanges();

                        Conec.Condicion = "id_producto = " + Convert.ToInt32(idprod);
                        Conec.CadenaSelect = "UPDATE Productos " +
                        "SET borrado = " + true + " WHERE (" + Conec.Condicion + ")";

                        Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Productos.TableName);
                    }
                    else
                    {
                        Conec.Condicion = "id_producto = " + Convert.ToInt32(idprod);
                        Conec.CadenaSelect = "DELETE * FROM Productos WHERE (" + Conec.Condicion + ")";

                        Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Productos.TableName);
                    }
                    /*   Conec.Condicion = "id_producto = " + Convert.ToInt32(idprod);
                       Conec.CadenaSelect = "DELETE * FROM Productos WHERE (" + Conec.Condicion + ")";

                       Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Productos.TableName);

                       Conec.Condicion = "id_producto = " + Convert.ToInt32(idprod);
                       Conec.CadenaSelect = "DELETE * FROM Prod_detalle WHERE (" + Conec.Condicion + ")";

                       Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Prod_detalle.TableName);                   */
                }

            }
            else
            {
                MessageBox.Show(this, Variable.SYS_MSJ[32, Variable.idioma], "", MessageBoxButtons.OK);
            } 
        }
        #endregion

        #region Evento de Captura y Validacion

        #region eventos del codigo
        private void tbxCodigo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)            
            {
                if (tbxCodigo.Text.Length > 0) this.tbxdescripcion.Focus();
                else tbxCodigo.Focus();
            }            
        }
        private void tbxCodigo_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxCodigo.BackColor;
            tbxCodigo.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxCodigo_Leave(object sender, EventArgs e)
        {
            tbxCodigo.BackColor = colorbefore;           
        }
        private void tbxCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
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
        #endregion

        #region Eventos de descripcion
        private void tbxdescripcion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { this.tbxPLU.Focus(); }
        }
        private void tbxdescripcion_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxdescripcion.BackColor;
            tbxdescripcion.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxdescripcion_Leave(object sender, EventArgs e)
        {
            tbxdescripcion.BackColor = colorbefore;
        }
        private void tbxdescripcion_KeyPress(object sender, KeyPressEventArgs e)
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
            else if (Char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = false;
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
        #endregion

        #region Eventos de precio
        private void tbxprecio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)            
            {
                //this.tbxprecio.Text = string.Format(Variable.F_Decimal, Convert.ToDouble(this.tbxprecio.Text));
                this.rBxEditable.Focus(); 
            }
        }
        private void tbxprecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            int iCountDot = tbxprecio.Text.Count(c => c == '.');
            int iPosDot = tbxprecio.Text.IndexOf('.');

            if (tbxprecio.Text != "" && tbxprecio.Text == "0" && e.KeyChar == '0')
            {
                e.Handled = true;
            }
            else if (e.KeyChar == '.')
            {
                if (iCountDot > 0)
                {
                    e.Handled = true;
                }
                else if (tbxprecio.Text.Count() > 0 && Variable.n_decimal > 0)
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
                switch (Variable.n_decimal)
                {
                    case 0:
                        if (tbxprecio.Text.Count() < 5)
                        {
                            e.Handled = false;
                        }
                        else
                        {
                            e.Handled = true;
                        }
                        break;

                    case 1:
                        if (iCountDot == 1)
                        {
                            if (iPosDot == 1)
                            {
                                if (tbxprecio.Text.Count() < 3)
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
                                if (tbxprecio.Text.Count() < 4)
                                {
                                    e.Handled = false;
                                }
                                else
                                {
                                    e.Handled = true;
                                }
                            }
                            else if (iPosDot == 3)
                            {
                                if (tbxprecio.Text.Count() < 5)
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
                        else if (tbxprecio.Text.Count() < 4)
                        {
                            e.Handled = false;
                        }
                        else
                        {
                            e.Handled = true;
                        }
                        break;

                    case 2:

                        if (iCountDot == 1)
                        {
                            if (iPosDot == 1)
                            {
                                if (tbxprecio.Text.Count() < 4)
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
                                if (tbxprecio.Text.Count() < 5)
                                {
                                    e.Handled = false;
                                }
                                else
                                {
                                    e.Handled = true;
                                }
                            }
                            else if (iPosDot == 3)
                            {
                                if (tbxprecio.Text.Count() < 6)
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
                        else if (tbxprecio.Text.Count() < 3)
                        {
                            e.Handled = false;
                        }
                        else
                        {
                            e.Handled = true;
                        }

                        break;
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
        private void tbxprecio_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxprecio.BackColor;
            tbxprecio.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxprecio_Leave(object sender, EventArgs e)
        {
            tbxprecio.BackColor = colorbefore;
        }
        #endregion

        #region evento de impuesto
        private void tbximp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (tbximp.Text != "")
                {
                    tbximp.Text = string.Format(Variable.F_Descuento, Convert.ToDouble(tbximp.Text));
                }
                this.tbxcaducidad.Focus();
            }
        }
        private void tbximp_KeyPress(object sender, KeyPressEventArgs e)
        {    
            int iCountDot = tbximp.Text.Count(c => c == '.');
            int iPosDot = tbximp.Text.IndexOf('.');

            if (tbximp.Text != "" && tbximp.Text == "0" && e.KeyChar == '0')
            {
                e.Handled = true;

            }
            else if (e.KeyChar == '.')
            {
                if (iCountDot > 0)
                {
                    e.Handled = true;
                }
                else if (tbximp.Text.Count() > 0 && tbximp.Text.Count() < 3)
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
                if (tbximp.Text.Count() < 2 && iCountDot == 0)
                {
                    e.Handled = false;
                }else if(iCountDot == 1){
                    if (iPosDot == 1)
                    {
                        if (tbximp.Text.Count() < 4)
                        {
                            e.Handled = false;
                        }else{
                            e.Handled = true;
                        }
                    }
                    else if (iPosDot == 2)
                    {
                        if (tbximp.Text.Count() < 5)
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
        private void tbximp_Validated(object sender, EventArgs e)
        {
            int cad = Variable.iValidarImpuesto(tbximp.Text);
            if (tbximp.Text != "")
            {
                if (cad == 0)
                {
                    tbximp.Text = string.Format(Variable.F_Descuento, Convert.ToDouble(tbximp.Text));
                }
            }
        }
        private void tbximp_Enter(object sender, EventArgs e)
        {
            colorbefore = tbximp.BackColor;
            tbximp.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbximp_Leave(object sender, EventArgs e)
        {
            tbximp.BackColor = colorbefore;
        }
        #endregion 

        #region evento de caducidad
        private void tbxcaducidad_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { this.rBxPesado.Focus(); }
        }
        private void tbxcaducidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
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
        private void tbxcaducidad_Validated(object sender, EventArgs e)
        {
            string cad = Variable.validar_salida(tbxcaducidad.Text, 0);
            if (cad.Length > 0)
            {
                tbxcaducidad.Text = cad;
            }
            else tbxcaducidad.Text = "0";
        }
        private void tbxcaducidad_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxcaducidad.BackColor;
            tbxcaducidad.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxcaducidad_Leave(object sender, EventArgs e)
        {
            tbxcaducidad.BackColor = colorbefore;
        }
        #endregion

        #region evento de multiplo
        private void tbxMultiplo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { this.btnImagen.Focus(); }
        }
        private void tbxMultiplo_KeyPress(object sender, KeyPressEventArgs e)
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
        private void tbxMultiplo_Validated(object sender, EventArgs e)
        {
            string cad = Variable.validar_salida(tbxMultiplo.Text, 0);
            if (cad.Length > 0)
            {
                tbxMultiplo.Text = cad;
                tbxTara.Text = "0";
            }
            else tbxMultiplo.Text = "0";
        }
        private void tbxMultiplo_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxMultiplo.BackColor;
            tbxMultiplo.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxMultiplo_Leave(object sender, EventArgs e)
        {
            tbxMultiplo.BackColor = colorbefore;
        }
        #endregion

        #region evento de Tara
        private void tbxTara_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxTara.BackColor;
            tbxTara.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxTara_Leave(object sender, EventArgs e)
        {
            tbxTara.BackColor = colorbefore;
        }
        private void tbxTara_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) btnImagen.Focus();
        }
        private void tbxTara_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Variable.unidad == 1)
            {
                if (e.KeyChar == '.')
                {
                    int iCountDot = tbxTara.Text.Count(c => c == '.');

                    if (iCountDot > 0)
                    {
                        e.Handled = true;
                    }
                    else if (tbxTara.Text.Count() > 0)
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
                else if (Char.IsNumber(e.KeyChar))
                {
                    e.Handled = false;
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
            else
            {
                if (tbxTara.Text == "0" && e.KeyChar == '0')
                {
                    e.Handled = true;
                }
                else if (Char.IsNumber(e.KeyChar))
                {
                    e.Handled = false;
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
        }
        private void tbxTara_Validated(object sender, EventArgs e)
        {
            if (Variable.unidad == 1)
            {
                int cad = Variable.iValidarImpuesto(tbxTara.Text);
                if (cad == 0)
                {
                    tbxTara.Text = string.Format(Variable.F_Descuento, Convert.ToDouble(tbxTara.Text));
                }
            }
        }
        #endregion    
   
        #region Evento de informacion adicional
        private void cbxInfoadicional_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((ComboBox)sender).Select();
        }
        private void cbxInfoadicional_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.cbxPubli1.Focus();
            }
        }
        private void cbxInfoadicional_Enter(object sender, EventArgs e)
        {
            colorbefore = cbxInfoadicional.BackColor;
            cbxInfoadicional.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void cbxInfoadicional_Leave(object sender, EventArgs e)
        {
            cbxInfoadicional.BackColor = colorbefore;
        }
        #endregion

        #region evento del PLU
        private void tbxPLU_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.tbxprecio.Focus();
        }       
        private void tbxPLU_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (tbxPLU.Text == "" && e.KeyChar == '0')
            {
                e.Handled = true;
            }
            else if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
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
        private void tbxPLU_Leave(object sender, EventArgs e)
        {
            tbxPLU.BackColor = colorbefore;
        }
        private void tbxPLU_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxPLU.BackColor;
            tbxPLU.BackColor = Color.FromArgb(255, 255, 174);
        }
        #endregion

        #region Evento de asignar imagen
        private void tbxImagenAsignada_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.cbxOferta.Focus();
            }
        }
        private void tbxImagenAsignada_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxImagenAsignada.BackColor;
            tbxImagenAsignada.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxImagenAsignada_Leave(object sender, EventArgs e)
        {
            tbxImagenAsignada.BackColor = colorbefore;
        }
        #endregion

        private void btnborrar_Click(object sender, EventArgs e)
        {
            Image image = null;
            this.picImagen.Image = (Image)image;
            this.picImagen.Refresh();
            this.picImagen.SizeMode = PictureBoxSizeMode.StretchImage;
            
            tbxImagenAsignada.Text = "";
        }
        private void btnImagen_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.InitialDirectory = Variable.appPath + "\\images\\";
            Image image = null;
           
            string NOMBRE_IMAGEN;

            try
            {

                this.openFileDialog1.DefaultExt = "JPG";
                this.openFileDialog1.Filter = "Image Files(*.JPG)|*.JPG|All files (*.*)|*.*";
                DialogResult result = this.openFileDialog1.ShowDialog();

                if (result == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(openFileDialog1.FileName.ToString()))
                    {
                        tbxImagenAsignada.Text = this.openFileDialog1.FileName;
                        NOMBRE_IMAGEN = this.openFileDialog1.SafeFileName;

                        this.picImagen.SizeMode = PictureBoxSizeMode.StretchImage;

                        image = Image.FromFile(tbxImagenAsignada.Text);
                        //Redimensionamos la imagen  
                        image = this.Redimensionar(image, 250, 250, Convert.ToInt32(image.HorizontalResolution));
                        Bitmap Imagen_producto = new Bitmap(image); //tbxImagenAsignada.Text); //this.openFileDialog1.InitialDirectory + NOMBRE_IMAGEN);                
                        this.picImagen.Image = Imagen_producto;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error: Could not read file from disk. Original error: " + ex.Message);
            }

            tbxImagenAsignada.Focus();
        }
        private Image Redimensionar(Image Imagen, int Ancho, int Alto, int resolucion)
        {
            //Bitmap sera donde trabajaremos los cambios
            using (Bitmap imagenBitmap = new Bitmap(Ancho, Alto, PixelFormat.Format32bppRgb))
            {
                imagenBitmap.SetResolution(resolucion, resolucion);
                //Hacemos los cambios a ImagenBitmap usando a ImagenGraphics y la Imagen Original(Imagen)
                //ImagenBitmap se comporta como un objeto de referenciado
                using (Graphics imagenGraphics = Graphics.FromImage(imagenBitmap))
                {
                    imagenGraphics.SmoothingMode = SmoothingMode.AntiAlias;
                    imagenGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    imagenGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    imagenGraphics.DrawImage(Imagen, new Rectangle(0, 0, Ancho, Alto), new Rectangle(0, 0, Imagen.Width, Imagen.Height), GraphicsUnit.Pixel);
                    //todos los cambios hechos en imagenBitmap lo llevaremos un Image(Imagen) con nuevos datos a travez de un MemoryStream
                    MemoryStream imagenMemoryStream = new MemoryStream();
                    imagenBitmap.Save(imagenMemoryStream, ImageFormat.Jpeg);
                    Imagen = Image.FromStream(imagenMemoryStream);
                }
            }
            return Imagen;
        }
        #endregion       
                      
        #region Listado de Datos       

        private void listado_oferta()
        {
            //cbxOferta.Items.Clear();
            Conec.CadenaSelect = "SELECT * FROM Oferta ORDER BY id_oferta";

            ofertaTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            ofertaTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            ofertaTableAdapter.Fill(baseDeDatosDataSet.Oferta);

            ArrayList lGrupos = new ArrayList();
            lGrupos.Add(new USState("----------", 0));

            foreach (DataRow dr in baseDeDatosDataSet.Oferta.Rows)
            {
                lGrupos.Add(new USState(dr["nombre"].ToString(), Convert.ToInt32(dr["id_oferta"])));
            }

            this.cbxOferta.DataSource = lGrupos;
            this.cbxOferta.ValueMember = "ShortName";
            this.cbxOferta.DisplayMember = "LongName";
            this.cbxOferta.SelectedIndex = 0;
            this.cbxOferta.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void listado_ingrediente()
        {
            //cbxInfoadicional.Items.Clear();
            Conec.CadenaSelect = "SELECT * FROM Ingredientes ORDER BY id_ingrediente";

            ingredientesTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            ingredientesTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            ingredientesTableAdapter.Fill(baseDeDatosDataSet.Ingredientes);

            ArrayList lGrupos = new ArrayList();
            lGrupos.Add(new USState("----------", 0));

            foreach (DataRow dr in baseDeDatosDataSet.Ingredientes.Rows)
            {
                lGrupos.Add(new USState(dr["Nombre"].ToString(), Convert.ToInt32(dr["id_ingrediente"])));
            }

            this.cbxInfoadicional.DataSource = lGrupos;
            this.cbxInfoadicional.ValueMember = "ShortName";
            this.cbxInfoadicional.DisplayMember = "LongName";
            this.cbxInfoadicional.SelectedIndex = 0;
            this.cbxInfoadicional.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void listado_publicidad()
        {
            //cbxPubli1.Items.Clear();
            Conec.CadenaSelect = "SELECT * FROM Ingredientes ORDER BY id_publicidad";

            publicidadTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            publicidadTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            publicidadTableAdapter.Fill(baseDeDatosDataSet.Publicidad);

            ArrayList lGrupos1 = new ArrayList();
            lGrupos1.Add(new USState("----------", 0));

            foreach (DataRow dr in baseDeDatosDataSet.Publicidad.Rows)
            {
                lGrupos1.Add(new USState(dr["Titulo"].ToString(), Convert.ToInt32(dr["id_publicidad"])));
            }

            this.cbxPubli1.DataSource = lGrupos1;
            this.cbxPubli1.ValueMember = "ShortName";
            this.cbxPubli1.DisplayMember = "LongName";
            this.cbxPubli1.SelectedIndex = 0;
            this.cbxPubli1.DropDownStyle = ComboBoxStyle.DropDownList;

            ArrayList lGrupos2 = new ArrayList();
            lGrupos2.Add(new USState("----------", 0));

            foreach (DataRow dr in baseDeDatosDataSet.Publicidad.Rows)
            {
                lGrupos2.Add(new USState(dr["Titulo"].ToString(), Convert.ToInt32(dr["id_publicidad"])));
            }

            this.cbxPubli2.DataSource = lGrupos2;
            this.cbxPubli2.ValueMember = "ShortName";
            this.cbxPubli2.DisplayMember = "LongName";
            this.cbxPubli2.SelectedIndex = 0;
            this.cbxPubli2.DropDownStyle = ComboBoxStyle.DropDownList;

            ArrayList lGrupos3 = new ArrayList();
            lGrupos3.Add(new USState("----------", 0));

            foreach (DataRow dr in baseDeDatosDataSet.Publicidad.Rows)
            {
                lGrupos3.Add(new USState(dr["Titulo"].ToString(), Convert.ToInt32(dr["id_publicidad"])));
            }

            this.cbxPubli3.DataSource = lGrupos3;
            this.cbxPubli3.ValueMember = "ShortName";
            this.cbxPubli3.DisplayMember = "LongName";
            this.cbxPubli3.SelectedIndex = 0;
            this.cbxPubli3.DropDownStyle = ComboBoxStyle.DropDownList;

            ArrayList lGrupos4 = new ArrayList();
            lGrupos4.Add(new USState("----------", 0));

            foreach (DataRow dr in baseDeDatosDataSet.Publicidad.Rows)
            {
                lGrupos4.Add(new USState(dr["Titulo"].ToString(), Convert.ToInt32(dr["id_publicidad"])));
            }

            this.cbxPubli4.DataSource = lGrupos4;
            this.cbxPubli4.ValueMember = "ShortName";
            this.cbxPubli4.DisplayMember = "LongName";
            this.cbxPubli4.SelectedIndex = 0;
            this.cbxPubli4.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        #endregion
      
        #region Evento de botones
               
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            limpiezaTextBoxes();
            Form1.statRegistro = ESTADO.EstadoRegistro.PKNOTRATADO;

            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Ingredientes' Puede moverla o quitarla según sea necesario.
            this.ingredientesTableAdapter.Fill(this.baseDeDatosDataSet.Ingredientes);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Oferta' Puede moverla o quitarla según sea necesario.
            this.ofertaTableAdapter.Fill(this.baseDeDatosDataSet.Oferta);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Publicidad' Puede moverla o quitarla según sea necesario.
            this.publicidadTableAdapter.Fill(this.baseDeDatosDataSet.Publicidad);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Productos' Puede moverla o quitarla según sea necesario.
            this.productosTableAdapter.Fill(this.baseDeDatosDataSet.Productos);

            listadoId_EnBD("borrado = " + false);  //"SELECT * FROM Productos ORDER BY id_producto");
           
            listado_oferta();
            listado_ingrediente();
            listado_publicidad();
            rBxEditable.Select();
            rBxPesado.Select();

            btnEditar.Enabled = false;
            btnBorrar.Enabled = false;
            btnGuardar.Enabled = true;
            btnNuevo.Enabled = false;
            picImagen.Enabled = true;
            iActionToSave = 0;

            treListadoP.TabStop = false;
            tbxFind.TabStop = false;
            button1.TabStop = false;
            panel1.TabStop = false;

            tbxdescripcion.Focus();

            button1.Enabled = false;
            tbxFind.Enabled = false;
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            iActionToSave = 1;
            activarDesactivarEdicion(true,Color.White);
            tbxCodigo.Enabled = false;
            tbxdescripcion.Focus();
            Form1.statRegistro = ESTADO.EstadoRegistro.PKPARCIAL;
            btnEditar.Enabled = false;
            btnGuardar.Enabled = true;
            btnBorrar.Enabled = false;
            picImagen.Enabled = true;

            treListadoP.TabStop = false;
            tbxFind.TabStop = false;
            button1.TabStop = false;
            panel1.TabStop = false;

            tbxFind.Enabled = false;
            button1.Enabled = false;
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            bool error = false;
            if (tbxCodigo.Text == "" || tbxCodigo.Text == "0")
            {
                MessageBox.Show(this, Variable.SYS_MSJ[83, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbxCodigo.Focus();
                error = true;
            }
            else if (tbxPLU.Text == "" || tbxPLU.Text == "0")
            {
                MessageBox.Show(this, Variable.SYS_MSJ[84, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbxPLU.Focus();
                error = true;
            }
            else if (tbxdescripcion.Text.Length <= 0)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[85, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbxdescripcion.Focus();
                error = true;
            }
            else if (tbxprecio.Text == "" || tbxprecio.Text == "0")
            {
                MessageBox.Show(this, Variable.SYS_MSJ[86, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbxprecio.Focus();
                error = true;
            }

            if (tbximp.Text == "")
            {
                tbximp.Text = "0";
            }


            if (tbxcaducidad.Text == "")
            {
                tbxcaducidad.Text = "0";
            }

            if (tbxTara.Text == "")
            {
                tbxTara.Text = "0";
            }

            else if (error == false)
            {
                if (Variable.moneda == 0 && Convert.ToDouble(tbxprecio.Text) * (1 + (Convert.ToDouble(tbximp.Text) / 100)) > 999.99)
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[378, Variable.idioma], Variable.SYS_MSJ[379, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tbxprecio.Focus();
                    error = true;
                }
                else if (Variable.moneda == 1 && Convert.ToDouble(tbxprecio.Text) * (1 + (Convert.ToDouble(tbximp.Text) / 100)) > 99999)
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[452, Variable.idioma], Variable.SYS_MSJ[379, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tbxprecio.Focus();
                    error = true;
                }
                else if (Variable.moneda == 2 && Convert.ToDouble(tbxprecio.Text) * (1 + (Convert.ToDouble(tbximp.Text) / 100)) > 9999.9) 
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[451, Variable.idioma], Variable.SYS_MSJ[379, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tbxprecio.Focus();
                    error = true;
                }
                else if (Variable.moneda == 3 && Convert.ToDouble(tbxprecio.Text) * (1 + (Convert.ToDouble(tbximp.Text) / 100)) > 999.999) 
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[450, Variable.idioma], Variable.SYS_MSJ[379, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tbxprecio.Focus();
                    error = true;
                }
            }

            if (!error)
            {
                OleDbDataReader olplu;

                if (iActionToSave == 0)
                {
                    olplu = Conec.Obtiene_Dato("Select codigo From Productos Where codigo = " + Convert.ToInt32(tbxCodigo.Text), Conec.CadenaConexion);
                    if (olplu.Read())
                    {
                        olplu.Close();
                        MessageBox.Show(this, Variable.SYS_MSJ[80, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }                   
                }

                olplu = Conec.Obtiene_Dato("Select NoPlu From Productos Where codigo = " + Convert.ToInt32(tbxCodigo.Text), Conec.CadenaConexion);
                if (olplu.Read() && olplu.HasRows)
                {
                    int NoPLU = Convert.ToInt32(olplu[0]); //NumPLU
                    olplu.Close();

                    //Si los PLUS no son iguales, el PLU se modifico.
                    if (NoPLU != Convert.ToInt32(tbxPLU.Text))
                    {
                        olplu = Conec.Obtiene_Dato("Select NoPlu From Productos Where NoPlu = " + Convert.ToInt32(tbxPLU.Text), Conec.CadenaConexion);
                        if (olplu.Read())
                        {
                            MessageBox.Show(this, Variable.SYS_MSJ[82, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            olplu.Close();
                            return;
                        }
                    }

                }   
                olplu.Close();

                Guardar();
                activarDesactivarEdicion(false, Color.WhiteSmoke);
                if (iActionToSave == 0) listadoId_EnBD("borrado = " + false);
                else ActivarDesactivarMenu();
                treListadoP.TabStop = true;
                tbxFind.TabStop = true;
                button1.TabStop = true;
                panel1.TabStop = true;
                picImagen.Enabled = false;

                MessageBox.Show(this, Variable.SYS_MSJ[344, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbxFind.Focus();

                tbxFind.Enabled = true;
                button1.Enabled = true;
            }
        }
        private void btnBorrar_Click_1(object sender, EventArgs e)
        {
            Eliminar();
            listadoId_EnBD("borrado = " + false);  
            
            activarDesactivarEdicion(false, Color.WhiteSmoke);
            tbxCodigo.Clear();
            tbxPLU.Clear();
            tbxdescripcion.Clear();
            tbxprecio.Clear();
            tbximp.Clear();
            tbxcaducidad.Clear();
            tbxMultiplo.Clear();
            rBxEditable.Checked = false;
            rBxNoeditable.Checked = false;
            rBxPesado.Checked = false;
            rBxNopesado.Checked = false;
            tbxImagenAsignada.Clear();
            this.picImagen.Image = null;
            tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);
            this.cbxPubli1.SelectedIndex = 0;
            this.cbxPubli2.SelectedIndex = 0;
            this.cbxPubli3.SelectedIndex = 0;
            this.cbxPubli4.SelectedIndex = 0;
            this.cbxOferta.SelectedIndex = 0;
            this.cbxInfoadicional.SelectedIndex = 0;
            picImagen.Enabled = false;

            btnEditar.Enabled = false;
            btnBorrar.Enabled = false;

            button1.Enabled = true;
            tbxFind.Enabled = true;
        }
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            ToolStripManager.RevertMerge("toolStrip2");
            this.Dispose();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);
            listadoId_EnBD("Nombre Like '*" + tbxFind.Text + "*'");
        }

        #region Evento de ofertas
        private void cbxOferta_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((ComboBox)sender).Select();
        }
        private void cbxOferta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) cbxInfoadicional.Focus();
        }
        private void cbxOferta_Enter(object sender, EventArgs e)
        {
            colorbefore = cbxOferta.BackColor;
            cbxOferta.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void cbxOferta_Leave(object sender, EventArgs e)
        {
            cbxOferta.BackColor = colorbefore;
        }
        #endregion

        #region Evento de publicidad
        private void cbxPubli1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //((ComboBox)sender).Select();   
        }
        private void cbxPubli1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
               this.cbxPubli2.Focus();
            }
        }
        private void cbxPubli1_Enter(object sender, EventArgs e)
        {
            colorbefore = cbxPubli1.BackColor;
            cbxPubli1.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void cbxPubli1_Leave(object sender, EventArgs e)
        {
            cbxPubli1.BackColor = colorbefore;
        }        

        private void cbxPubli2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
               this.cbxPubli3.Focus();
            }
        }
        private void cbxPubli2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //((ComboBox)sender).Select();   
        }
        private void cbxPubli2_Enter(object sender, EventArgs e)
        {
            colorbefore = cbxPubli2.BackColor;
            cbxPubli2.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void cbxPubli2_Leave(object sender, EventArgs e)
        {
            cbxPubli2.BackColor = colorbefore;
        }

        private void cbxPubli4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.tbxdescripcion.Focus();
            }
        }
        private void cbxPubli4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((ComboBox)sender).Select();   
        }
        private void cbxPubli4_Enter(object sender, EventArgs e)
        {
            colorbefore = cbxPubli4.BackColor;
            cbxPubli4.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void cbxPubli4_Leave(object sender, EventArgs e)
        {
            cbxPubli4.BackColor = colorbefore;
        }

        private void cbxPubli3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.cbxPubli4.Focus();
            }
        }
        private void cbxPubli3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //((ComboBox)sender).Select();   
        }
        private void cbxPubli3_Enter(object sender, EventArgs e)
        {
            colorbefore = cbxPubli3.BackColor;
            cbxPubli3.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void cbxPubli3_Leave(object sender, EventArgs e)
        {
            cbxPubli3.BackColor = colorbefore;
        }
        #endregion

        #region Evento de Radio button
        private void rBxEditable_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxEditable.Checked) tipo_precio = 1;
        }
        private void rBxNoeditable_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxNoeditable.Checked) tipo_precio = 0;
        }
        private void rBxPesado_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxPesado.Checked)
            {
                lbTarMul.Text = Variable.SYS_MSJ[311, Variable.idioma] + " :";
                tbxTara.Enabled = true;
                tbxTara.BringToFront();
                tbxMultiplo.Enabled = false;
                tipo_producto = 1;
            }
        }
        private void rBxNopesado_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxNopesado.Checked)
            {
                lbTarMul.Text = Variable.SYS_MSJ[404,Variable.idioma] +" :";
                tbxMultiplo.Enabled = true;
                tbxMultiplo.BringToFront();
                tbxTara.Enabled = false;                
                tipo_producto = 0;
            }
        }
        #endregion

        #endregion
        
        private void UserProductos_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Ingredientes' Puede moverla o quitarla según sea necesario.
            this.ingredientesTableAdapter.Fill(this.baseDeDatosDataSet.Ingredientes);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Oferta' Puede moverla o quitarla según sea necesario.
            this.ofertaTableAdapter.Fill(this.baseDeDatosDataSet.Oferta);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Publicidad' Puede moverla o quitarla según sea necesario.
            this.publicidadTableAdapter.Fill(this.baseDeDatosDataSet.Publicidad);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Productos' Puede moverla o quitarla según sea necesario.
            this.productosTableAdapter.Fill(this.baseDeDatosDataSet.Productos);

            listadoId_EnBD("borrado = " + false);     //"SELECT * FROM Productos ORDER BY id_producto");
            if (Variable.n_decimal == 3) tbxprecio.MaxLength = 7;
            listado_oferta();
            listado_ingrediente();
            listado_publicidad();
            
            activarDesactivarEdicion(false, Color.WhiteSmoke);
            
            Cursor.Current = Cursors.Default;
        }
        private void btnDepurar_Click(object sender, EventArgs e)
        {
            UserDepurar uspurge = new UserDepurar((int)ESTADO.FileSource.fProductos);
            uspurge.ShowDialog(this);
            listadoId_EnBD("borrado = " + false);  //"SELECT * FROM Productos ORDER BY id_producto");
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

                    UsExport.exportar(1, ref MyFileExp);

                    MyFileExp.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnImport_Click(object sender, EventArgs e)
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
                            MyFile = new FileStream(nombre_archivo, FileMode.Open, FileAccess.Read,FileShare.ReadWrite);
                        }
                        ImpExp IMP = new ImpExp();
                        IMP.importar((int)ESTADO.FileSource.fProductos, ref MyFile, nombre_archivo);

                        listadoId_EnBD("borrado = " + false);  //"SELECT * FROM Productos ORDER BY id_producto");
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
        private void pnldetalle_SizeChanged(object sender, EventArgs e)
        {
            int WScreen = this.ClientSize.Width;

            if (WScreen <= 800 && (iTypeResolution == 0 || iTypeResolution == 2))
            {
                iTypeResolution = 1;
                groupBox4.Location = new Point(3, 265);
                groupBox4.Size = new Size(210, 62);
                groupBox3.Location = new Point(3, 333);
                groupBox3.Size = new Size(210, 62);
                groupBox2.Location = new Point(219, 230);
                groupBox5.Size = new Size(210, 256);
                panel3.Location = new Point(219, 10);
                this.AutoScroll = true;
            }
            else if (WScreen > 800 && iTypeResolution == 1)
            {
                iTypeResolution = 2;
                groupBox4.Location = new Point(3, 432);
                groupBox4.Size = new Size(310, 62);
                groupBox3.Location = new Point(3, 364);
                groupBox3.Size = new Size(310, 62);
                groupBox2.Location = new Point(319,248);
                groupBox5.Size = new Size(310, 356);
                panel3.Location = new Point(319, 10);
                this.AutoScroll = false;
            }
        }
        private void pnldetalle_Resize(object sender, EventArgs e)
        {
            int WScreen = this.ClientSize.Width;

            if (WScreen <= 800 && (iTypeResolution == 0 || iTypeResolution == 2))
            {
                iTypeResolution = 1;
                groupBox4.Location = new Point(3, 265);
                groupBox4.Size = new Size(210, 62);
                groupBox3.Location = new Point(3, 333);
                groupBox3.Size = new Size(210, 62);
                groupBox2.Location = new Point(219, 230);
                groupBox5.Size = new Size(210, 256);
                panel3.Location = new Point(219, 10);
                this.AutoScroll = true;
            }
            else if (WScreen > 800 && iTypeResolution == 1)
            {
                iTypeResolution = 2;
                groupBox4.Location = new Point(3, 432);
                groupBox4.Size = new Size(310, 62);
                groupBox3.Location = new Point(3, 364);
                groupBox3.Size = new Size(310, 62);
                groupBox2.Location = new Point(319,248);
                groupBox5.Size = new Size(310, 356);
                panel3.Location = new Point(319, 10);
                this.AutoScroll = false;
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
        private void picImagen_Click(object sender, EventArgs e)
        {
            btnImagen_Click(btnImagen, null);
        }                      
    }
}

