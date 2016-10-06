using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.Common;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;
using Validaciones;

namespace MainMenu
{
    public partial class UserBasculas : UserControl
    {
        Form1 _form;
        bool DBwlsd   = false;
        bool DBwls    = false;
        bool DBwlabel = false;
        Variable.TipoBascula getScale(string type)
        {
            Variable.TipoBascula tipo = Variable.TipoBascula.WLABEL;
            if ((type.ToUpper().Equals("WLS"))){tipo = Variable.TipoBascula.WLS;}
            if ((type.ToUpper().Equals("WLSD"))){tipo = Variable.TipoBascula.WLSD;}
            if ((type.ToUpper().Equals("W-LABEL"))){tipo = Variable.TipoBascula.WLABEL;}
            return tipo;
        }
        void loadScalesTypesfromDB()
        {
            DBwlsd = false;
            DBwls = false;
            DBwlabel = false;
            Variable.scales.Clear();
            Conec.CadenaSelect = "SELECT * FROM Bascula";
            BaseDeDatosDataSetTableAdapters.BasculaTableAdapter basculaTableAdapter = new BaseDeDatosDataSetTableAdapters.BasculaTableAdapter();
            BaseDeDatosDataSet baseDeDatosDataSet1 = new BaseDeDatosDataSet();
            basculaTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            basculaTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            basculaTableAdapter.Fill(baseDeDatosDataSet1.Bascula);

            foreach (DataRow dr in baseDeDatosDataSet1.Bascula.Rows)
            {
                Scale s = new Scale();
                s.tipo = getScale(dr["modelo"].ToString().ToUpper());
                Variable.scales.Add(s);
            }



            //BUSCAR LOS TIPOS DE BASCULA QUE TIENE EL SISTEMA**************************************
            foreach (Scale item in Variable.scales)
            {
                if (item.tipo == Variable.TipoBascula.WLSD)
                {
                    DBwlsd = true;
                }
            }

            foreach (Scale item in Variable.scales)
            {
                if (item.tipo == Variable.TipoBascula.WLS)
                {
                    DBwls = true;
                }
            }
            foreach (Scale item in Variable.scales)
            {
                if (item.tipo == Variable.TipoBascula.WLABEL)
                {
                    DBwlabel = true;
                }
            }
            ///********************************************************************************************

        }
        void ValidarTipoSistema()
        {
                loadScalesTypesfromDB();//cargar las basculas dadas de alta en el sistema.

                if (DBwlabel == true  && DBwls == false && DBwlsd == false) 
                {                    
                    if (Variable.TypeScale != Variable.TipoBascula.WLABEL)
                    {
                        MessageBox.Show(this, "EL SISTEMA SE ADAPTO PARA TRABAJAR SOLO W-LABEL, NO SE DETECTO NINGUN OTRO TIPO DE BASCULA", Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Variable.TypeScale = Variable.TipoBascula.WLABEL;
                    }
                }
                if (DBwlabel == true  && DBwls == true  && DBwlsd == false) 
                {
                    if (Variable.TypeScale != Variable.TipoBascula.WLS)
                    {
                        MessageBox.Show(this, "EL SISTEMA SE ADAPTO PARA TRABAJAR SOLO WLS, TIENE REGISTRADAS BASCULAS W-LABEL Y WLS", Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Variable.TypeScale = Variable.TipoBascula.WLS;
                    }
                }

                if (DBwlabel == true  && DBwls == false && DBwlsd == true)
                {
                    if (Variable.TypeScale != Variable.TipoBascula.WLSD)
                    {
                        MessageBox.Show(this, "EL SISTEMA SE ADAPTO PARA TRABAJAR SOLO WLSD, TIENE REGISTRADAS BASCULAS WLS Y WLSD", Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Variable.TypeScale = Variable.TipoBascula.WLSD;
                    }
                }

                if (DBwlabel == true  && DBwls == true  && DBwlsd == true)  
                {
                    if (Variable.TypeScale != Variable.TipoBascula.WLSD)
                    {
                        MessageBox.Show(this, "EL SISTEMA SE ADAPTO PARA TRABAJAR SOLO WLSD, TIENE REGISTRADAS BASCULAS WLS Y WLSD Y W-LABEL", Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Variable.TypeScale = Variable.TipoBascula.WLSD;
                    }
                }


                if (DBwlabel == false && DBwls == true  && DBwlsd == false) 
                {
                    if (Variable.TypeScale != Variable.TipoBascula.WLS)
                    {
                        MessageBox.Show(this, "EL SISTEMA SE ADAPTO PARA TRABAJAR SOLO WLS, TIENE REGISTRADAS BASCULAS WLS", Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Variable.TypeScale = Variable.TipoBascula.WLS;
                    }
                }

                if (DBwlabel == false && DBwls == false && DBwlsd == true)  
                {
                    if (Variable.TypeScale != Variable.TipoBascula.WLSD)
                    {
                        MessageBox.Show(this, "EL SISTEMA SE ADAPTO PARA TRABAJAR SOLO WLSD, TIENE REGISTRADAS BASCULAS WLSD", Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Variable.TypeScale = Variable.TipoBascula.WLSD;
                    }
                    }
                if (DBwlabel == false && DBwls == true  && DBwlsd == true)  
                {
                    if (Variable.TypeScale != Variable.TipoBascula.WLSD)
                    {
                        MessageBox.Show(this, "EL SISTEMA SE ADAPTO PARA TRABAJAR SOLO WLSD, TIENE REGISTRADAS BASCULAS WLS Y WLSD", Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Variable.TypeScale = Variable.TipoBascula.WLSD;
                    }
                }
                this._form.RefrescaName();

         }
        #region Declaracion de Class
        ADOutil Conec = new ADOutil();
        Conexion Sock_Bascula = new Conexion();
        Serial SR = new Serial();
        CheckSum Chk = new CheckSum();
        int iTypeResolution = 0;
        #endregion
        #region Constantes y variable
        public string idbasc;
        Color colorbefore;
        private Form1 form1;
        #endregion
        #region Inicio
        public UserBasculas(Form1 form1)
        {
            // TODO: Complete member initialization
            this._form = form1;
            InitializeComponent();
            
        }
        #endregion               
        #region  Comunicacion
        private bool validarAddress(string psAddress)
        {
            IPAddress ip;
            bool valid = false;
            if (string.IsNullOrEmpty(psAddress))
            {
                valid = false;
            }
            else
            {
                valid = IPAddress.TryParse(psAddress, out ip);
                if (valid)
                {
                    ip = IPAddress.Parse(psAddress);
                }
            }
            return valid;
        }

        private int cargarCOMs()
        {
            cbxUSB.Items.Clear();

            if (Variable.port.Count > 0)
            {
                foreach (string puerto in Variable.port)
                {
                    cbxUSB.Items.Add(puerto);
                }

                cbxUSB.SelectedIndex = 0;
            }
            else
            {
                cbxUSB.Items.Add("----------");
            }

            return Variable.port.Count;
        }
        #endregion
        #region Registro de Base de Datos y DataSet
        private void Consulta_EnBD(string nbasc)
        {
            grupoTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            grupoTableAdapter.Connection.CreateCommand().CommandText = "SELECT * FROM Grupo ORDER BY id_grupo";
            grupoTableAdapter.Fill(baseDeDatosDataSet.Grupo);

            ArrayList lGrupos = new ArrayList();
            lGrupos.Add(new USState("----------", 0));

            if (baseDeDatosDataSet.Grupo.Count > 0)
            {
                foreach (DataRow dr in baseDeDatosDataSet.Grupo.Rows)
                {
                    lGrupos.Add(new USState(dr["nombre_gpo"].ToString(), Convert.ToInt32(dr["id_grupo"])));
                }
            }
            this.cbxGrupo.DataSource = lGrupos;
            this.cbxGrupo.ValueMember = "ShortName";
            this.cbxGrupo.DisplayMember = "LongName";

            this.cbxGrupo.SelectedIndex = 0;

            Conec.CadenaSelect = "SELECT * FROM Bascula ORDER BY id_bascula";

            basculaTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            basculaTableAdapter.Connection.CreateCommand().CommandText = "SELECT * FROM Bascula ORDER BY id_bascula";            
            basculaTableAdapter.Fill(baseDeDatosDataSet.Bascula);

            DataRow dre = baseDeDatosDataSet.Bascula.Rows.Find(Convert.ToInt32(nbasc));
            
            if (dre != null)
            {
                Mostrar_Dato(ref dre);
            }

        }
        private void Mostrar_Dato(ref DataRow dr)
        {
            idbasc = dr["id_bascula"].ToString();
            this.tbxNombre.Text = dr["nombre"].ToString();
            this.tbxDescrip.Text = dr["descripcion"].ToString();

            if (Convert.ToInt16(dr["tipo_conec"].ToString()) == (int)ESTADO.tipoConexionesEnum.PKWIFI)
            {
                this.cbxUSB.SelectedItem = 0;
                this.tbxDirIp.Text = dr["dir_ip"].ToString();
            }
            else
            {
                this.tbxDirIp.Text = "";
                this.cbxUSB.SelectedItem = dr["puerto"].ToString();
            }

            this.tbxmodelo.Text = dr["modelo"].ToString();
            this.tbxmedida.Text = dr["capacidad"].ToString();
            this.tbxnserie.Text = dr["no_serie"].ToString();
            this.tbxum.Text = dr["uni_med"].ToString();
            this.tbxdivminima.Text = dr["div_minima"].ToString();
            this.tbxIdioma.Text = dr["idioma"].ToString();
            this.tbxDecimales.Text = dr["decimales"].ToString();
            this.cbxConexion.SelectedIndex = Convert.ToInt16(dr["tipo_conec"].ToString());            
            if (dr["id_grupo"].ToString() != "")
            {
                this.cbxGrupo.SelectedValue = dr["id_grupo"];
                Console.WriteLine("Grupo: {0}", dr["id_grupo"]);
            }
            else
            {
                this.cbxGrupo.SelectedIndex = 0;
            }

            this.tbxNombre.Focus();
        }
        private void listadoId_EnBD()
        {
            try
            {
                grupoTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                grupoTableAdapter.Connection.CreateCommand().CommandText = "SELECT * FROM Grupo ORDER BY id_grupo";
                grupoTableAdapter.Fill(baseDeDatosDataSet.Grupo);

                ArrayList lGrupos = new ArrayList();
                lGrupos.Add(new USState("----------", 0));

                if (baseDeDatosDataSet.Grupo.Count > 0)
                {
                    foreach (DataRow dr in baseDeDatosDataSet.Grupo.Rows)
                    {
                        lGrupos.Add(new USState(dr["nombre_gpo"].ToString(), Convert.ToInt32(dr["id_grupo"])));
                    }
                }
                this.cbxGrupo.DataSource = lGrupos;
                this.cbxGrupo.ValueMember = "ShortName";
                this.cbxGrupo.DisplayMember = "LongName";

                this.cbxGrupo.SelectedIndex = 0;

                Conec.CadenaSelect = "SELECT * FROM Bascula ORDER BY id_bascula";

                basculaTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                basculaTableAdapter.Connection.CreateCommand().CommandText = "SELECT * FROM Bascula ORDER BY id_bascula";
                basculaTableAdapter.Fill(baseDeDatosDataSet.Bascula);
            }

            catch (Exception ex)
            {
                MessageBox.Show(this,"Excepcion " + ex);
            }        
        }
        private bool buscar_capacidad(int grupo, string capacidad)
        {
            if (grupo != 0)
            {
                DataRow[] drr = baseDeDatosDataSet.Bascula.Select("id_grupo = " + grupo);

                foreach (DataRow drbas in drr)
                {
                    if (drbas["uni_med"].ToString().ToLower() == capacidad.ToLower()) return true;
                    else return false;
                }
                return true;
            }
            else return true;
        }
        private string muestraAutoincrementoId()
        {            
            int cod = 0;

            OleDbDataReader LP = Conec.Obtiene_Dato("SELECT id_bascula FROM BASCULA ORDER BY id_bascula desc", Conec.CadenaSelect); //Variable.Conexion);
            if (LP.Read()) cod = Convert.ToInt32(LP.GetValue(0));
            LP.Close();
            return Convert.ToString(cod + 1);

        }



        private void Guardar(bool Existe, int iActionToSave)
        {
            basculaTableAdapter.Fill(baseDeDatosDataSet.Bascula);

            string Fecha_ult = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);

            if (!Existe)
            {
                if (cbxConexion.SelectedIndex == (int)ESTADO.tipoConexionesEnum.PKWIFI)
                {
                   Conec.CadenaSelect = "INSERT INTO Bascula " +                    
                   "(id_bascula,descripcion, id_grupo, dir_ip, no_serie, modelo, capacidad,div_minima,uni_med, nombre, tipo_conec,puerto,baud,actualizacion,idioma,decimales)" +
                   "VALUES (" + idbasc + ",'" +
                   tbxDescrip.Text + "'," +
                   Convert.ToInt32(cbxGrupo.SelectedValue) + ",'" +                   
                   tbxDirIp.Text + "','" +                   
                   tbxnserie.Text + "','" +
                   tbxmodelo.Text + "','" +
                   tbxmedida.Text + "','" +
                   tbxdivminima.Text + "','" +
                   tbxum.Text + "','" +
                   tbxNombre.Text + "','" +
                   cbxConexion.SelectedIndex.ToString() + "'," +
                   cbxUSB.SelectedIndex + ", 115200 ,'" +
                   Fecha_ult + "','" +
                   tbxIdioma.Text + "'," +
                   Convert.ToByte(tbxDecimales.Text) + ")";

                   Conec.InsertarReader(Conec.CadenaSelect, Conec.CadenaSelect, baseDeDatosDataSet.Bascula.TableName);

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
                   "VALUES (" + idbasc + "," +
                       "0," +
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
                }

                else
                {
                    tbxDirIp.Text = "";

                   Conec.CadenaSelect = "INSERT INTO Bascula " +
                   "(id_bascula,descripcion, id_grupo,  dir_ip, no_serie, modelo, capacidad,div_minima,uni_med, nombre, tipo_conec,puerto,baud,actualizacion,idioma,decimales)" +
                   "VALUES (" + idbasc + ",'" +
                   tbxDescrip.Text + "'," +
                   Convert.ToInt32(cbxGrupo.SelectedValue) + ",'" +
                   tbxDirIp.Text + "','" +
                   tbxnserie.Text + "','" +
                   tbxmodelo.Text + "','" +
                   tbxmedida.Text + "','" +
                   tbxdivminima.Text + "','" +
                   tbxum.Text + "','" +
                   tbxNombre.Text + "','" +
                   cbxConexion.SelectedIndex.ToString() + "','" +
                   cbxUSB.SelectedItem.ToString() + "', 115200 ,'" +
                   Fecha_ult + "','" +
                   tbxIdioma.Text + "'," +
                   Convert.ToByte(tbxDecimales.Text) + ")";

                   Conec.InsertarReader(Conec.CadenaSelect, Conec.CadenaSelect, baseDeDatosDataSet.Bascula.TableName);
                }                
            }
            else
            {

                if (cbxConexion.SelectedIndex == (int)ESTADO.tipoConexionesEnum.PKWIFI)
                {
                    tbxnserie.Text = tbxnserie.Text;
                    Conec.Condicion = "id_bascula = " + Convert.ToInt32(idbasc);
                    Conec.CadenaSelect = "UPDATE Bascula SET " +
                    "descripcion = '" + tbxDescrip.Text + "'," +
                    "id_grupo = " + Convert.ToInt32(cbxGrupo.SelectedValue) + "," +
                    "dir_ip = '" + tbxDirIp.Text + "'," +
                    "no_serie = '" + tbxnserie.Text + "'," +
                    "modelo = '" + tbxmodelo.Text + "'," +
                    "capacidad = '" + tbxmedida.Text + "'," +
                    "div_minima = '" + tbxdivminima.Text + "'," +
                    "uni_med = '" + tbxum.Text + "'," +
                    "nombre = '" + tbxNombre.Text + "'," +
                    "tipo_conec = '" + cbxConexion.SelectedIndex.ToString() + "'," +
                    "puerto = " + cbxUSB.SelectedIndex + "," +
                    "baud = 115200 ," +
                    "actualizacion = '" + Fecha_ult + "'," +
                    "idioma = '" + tbxIdioma.Text + "'," +
                    "decimales = " + tbxDecimales.Text + "" +
                    " WHERE ( " + Conec.Condicion + " )";
                }
                else
                {

                    tbxDirIp.Text = "";                    
                    Conec.Condicion = "id_bascula = " + Convert.ToInt32(idbasc);
                    Conec.CadenaSelect = "UPDATE Bascula SET " +
                    "descripcion = '" + tbxDescrip.Text + "'," +
                    "id_grupo = " + Convert.ToInt32(cbxGrupo.SelectedValue) + "," +
                    "dir_ip = '" + tbxDirIp.Text + "'," +
                    "no_serie = '" + tbxnserie.Text + "'," +
                    "modelo = '" + tbxmodelo.Text + "'," +
                    "capacidad = '" + tbxmedida.Text + "'," +
                    "div_minima = '" + tbxdivminima.Text + "'," +
                    "uni_med = '" + tbxum.Text + "'," +
                    "nombre = '" + tbxNombre.Text + "'," +
                    "tipo_conec = '" + cbxConexion.SelectedIndex.ToString() + "'," +
                    "puerto = '" + cbxUSB.SelectedItem.ToString() + "'," +
                    "baud = 115200 ," +
                    "actualizacion = '" + Fecha_ult + "'," +
                    "idioma = '" + tbxIdioma.Text + "'," +
                    "decimales = " + tbxDecimales.Text +
                    " WHERE ( " + Conec.Condicion + " )";
                }

                Conec.ActualizaReader(Conec.CadenaSelect, Conec.CadenaSelect, baseDeDatosDataSet.Bascula.TableName);
            }
        }       
        private void Eliminar()
        {
            bool borrar = false;

            basculaTableAdapter.Fill(baseDeDatosDataSet.Bascula);

            DataRow dr = baseDeDatosDataSet.Bascula.Rows.Find(Convert.ToInt32(idbasc));

            if (dr != null && baseDeDatosDataSet.Bascula.Rows.Count > 0)
            {
                //Esta bascula sera borrada
                if (MessageBox.Show(this,Variable.SYS_MSJ[30, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    borrar = true;
                    if (Conec.Obtiene_Dato("SELECT * FROM Prod_detalle WHERE (id_bascula = " + idbasc + ")", Conec.CadenaConexion).Read())
                    {
                        MessageBox.Show(this, Variable.SYS_MSJ[31, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);  //la bascula tiene productos asignado no es posible borrarla
                        borrar = false;
                    }

                    if (borrar)
                    {
                        dr.Delete();
                        baseDeDatosDataSet.AcceptChanges();
                        
                        //Conec.Condicion = "id_bascula = " + Convert.ToInt32(idbasc);
                        //Conec.CadenaSelect = "DELETE * FROM Bascula WHERE (" + Conec.Condicion + ")";

                        //Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Bascula.TableName);

                        string sele = "DELETE * FROM Bascula WHERE ( id_bascula = " + Convert.ToInt32(idbasc) +  ")";
                        Conec.EliminarReader(Conec.CadenaConexion, sele, "Bascula");

                        sele = "DELETE * FROM carpeta_detalle WHERE ( id_bascula = " + Convert.ToInt32(idbasc) + ")";
                        Conec.EliminarReader(Conec.CadenaConexion, sele, "carpeta_detalle");

                        Conec.Condicion = "id_bascula = " + Convert.ToInt32(idbasc);
                        Conec.CadenaSelect = "DELETE * FROM Impresor WHERE (" + Conec.Condicion + ")";

                        Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Impresor.TableName);

                        Conec.Condicion = "id_bascula = " + Convert.ToInt32(idbasc);
                        Conec.CadenaSelect = "DELETE * FROM BufferSalida WHERE (" + Conec.Condicion + ")";

                        Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.BufferSalida.TableName);

                        Conec.Condicion = "id_bascula = " + Convert.ToInt32(idbasc);
                        Conec.CadenaSelect = "DELETE * FROM Public_detalle WHERE (" + Conec.Condicion + ")";

                        Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Public_Detalle.TableName);

                        Conec.Condicion = "id_bascula = " + Convert.ToInt32(idbasc);
                        Conec.CadenaSelect = "DELETE * FROM Oferta_Detalle WHERE (" + Conec.Condicion + ")";

                        Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Oferta_Detalle.TableName);

                        Conec.Condicion = "id_bascula = " + Convert.ToInt32(idbasc);
                        Conec.CadenaSelect = "DELETE * FROM Ingre_detalle WHERE (" + Conec.Condicion + ")";

                        Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Ingre_detalle.TableName);

                        Conec.Condicion = "id_bascula = " + Convert.ToInt32(idbasc);
                        Conec.CadenaSelect = "DELETE * FROM Vendedor_detalle WHERE (" + Conec.Condicion + ")";

                        Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Vendedor_detalle.TableName);

                        Conec.Condicion = "id_bascula = " + Convert.ToInt32(idbasc);
                        Conec.CadenaSelect = "DELETE * FROM PLU_detalle WHERE (" + Conec.Condicion + ")";

                        Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.PLU_detalle.TableName);

                        bool existe;
                        OleDbDataReader OlHead = Conec.Obtiene_Dato("Select * From Encabezado Where id_bascula = " + Convert.ToInt32(idbasc) + " and id_grupo = 0", Conec.CadenaConexion);
                        if (OlHead.Read()) existe = true;
                        else existe = false;
                        OlHead.Close();

                        if (existe)
                        {
                            Conec.Condicion = "id_bascula = " + Convert.ToInt32(idbasc) + " and id_grupo = 0";

                            Conec.CadenaSelect = "DELETE * FROM Encabezado WHERE (" + Conec.Condicion + ")";
                            Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Configuracion.TableName);
                        }

                        OleDbDataReader OlTex = Conec.Obtiene_Dato("Select * From Textos Where id_bascula = " + Convert.ToInt32(idbasc) + " and id_grupo = 0", Conec.CadenaConexion);
                        if (OlTex.Read()) existe = true;
                        else existe = false;
                        OlTex.Close();

                        if (existe)
                        {
                            Conec.Condicion = "id_bascula = " + Convert.ToInt32(idbasc) + " and id_grupo = 0";

                            Conec.CadenaSelect = "DELETE * FROM Textos WHERE (" + Conec.Condicion + ")";
                            Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Textos.TableName);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(this, Variable.SYS_MSJ[32, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information); //"No hay productos dados de alta"
            }
        }
        #endregion
        #region Activacion de controles
        void activarDesactivarEdicion(bool pbActivar,Color pbColor)
        {          
            tbxDescrip.Enabled = pbActivar;
            tbxDescrip.BackColor = pbColor;
            tbxNombre.Enabled = pbActivar;
            tbxNombre.BackColor = pbColor;
            cbxConexion.Enabled = pbActivar;
            cbxConexion.BackColor = pbColor;
            cbxGrupo.Enabled = pbActivar;
            cbxGrupo.BackColor = pbColor;
            tbxDirIp.Enabled = pbActivar;
            tbxDirIp.BackColor = pbColor;
            cbxUSB.Enabled = pbActivar;
            cbxUSB.BackColor = pbColor;
            btnConectarBas.Enabled = pbActivar;
        }
        
        private void limpiezaTextBoxes()
        {
            idbasc = muestraAutoincrementoId();
            tbxDirIp.Text =  new GetIP().IPStr;
            cbxGrupo.SelectedIndex = 0;
            cbxConexion.SelectedIndex = 0;
            cbxUSB.SelectedIndex = -1;
            tbxDescrip.Clear();
            tbxNombre.Clear();            
            tbxnserie.Clear();
            tbxmodelo.Clear();
            tbxmedida.Clear();
            tbxIdioma.Clear();
            tbxdivminima.Clear();
            tbxum.Clear();
            tbxDecimales.Clear();
            activarDesactivarEdicion(true,Color.White);
            tbxNombre.Focus();
        }

        private void cbxConexion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxConexion.SelectedIndex == (int)ESTADO.tipoConexionesEnum.PKUSBCOM)
            {                
                this.lblUsb.Visible = true;
                this.cbxUSB.Visible = true;
                this.tbxDirIp.Visible = false;
                this.lblDirIp.Visible = false;
                Variable.Cargar_puertos();
                cargarCOMs();
            }
            if ((cbxConexion.SelectedIndex == (int)ESTADO.tipoConexionesEnum.PKWIFI)) //(cbxConexion.SelectedIndex == (int)ESTADO.tipoConexionesEnum.PKETHERNET) || 
            {
                this.lblUsb.Visible = false;
                this.cbxUSB.Visible = false;
                this.tbxDirIp.Visible = true;
                this.lblDirIp.Visible = true;
            }
        }

        private void btnConectarBas_Click(object sender, EventArgs e)
        {
            int DirIp;
            int CountError = 0;
            string[] Dato_Recivido = null;

            if (cbxConexion.SelectedIndex == 0)
            {
                DirIp = iValidarDireccionIp(tbxDirIp.Text, "IP");

                if (DirIp != 0)
                {
                    string MsgAtencion = "";
                    MsgAtencion += Variable.SYS_MSJ[182, Variable.idioma] + " '";

                    if (DirIp == 1)
                    {
                        MsgAtencion += " " + Variable.SYS_MSJ[177, Variable.idioma];
                        CountError++;
                    }

                    if (CountError > 1)
                    {
                        MsgAtencion += "' " + Variable.SYS_MSJ[180, Variable.idioma];
                    }
                    else
                    {
                        MsgAtencion += "' " + Variable.SYS_MSJ[180, Variable.idioma];
                    }

                    MessageBox.Show(this, MsgAtencion, Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                Cursor.Current = Cursors.WaitCursor;


                WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[390, Variable.idioma]);
                Thread t = new Thread(workerObject.vShowMsg);
                t.Start();

                string ck = Convert.ToString(Chk.ChkSum("CX" + (char)9 + (char)10));

                Socket Cliente_bascula = Sock_Bascula.conectar(this.tbxDirIp.Text, 50036);

                if (Cliente_bascula != null)
                {
                    Sock_Bascula.Envio_Dato(ref Cliente_bascula, tbxDirIp.Text, "CX" + (char)9 + (char)10, ref Dato_Recivido);
                    if (Dato_Recivido != null && Dato_Recivido[1] != "")
                    {
                        if (Dato_Recivido[0].IndexOf("CX") >= 0 && Dato_Recivido.Length > 8)
                        {
                            if (Dato_Recivido[6].ToLower() == Variable.FOR_UM[Variable.unidad].ToLower())
                            {
                                this.tbxnserie.Text = Dato_Recivido[1];
                                this.tbxmedida.Text = Dato_Recivido[2];
                                this.tbxdivminima.Text = Dato_Recivido[3];
                                this.tbxIdioma.Text = Dato_Recivido[4];
                                this.tbxum.Text = Dato_Recivido[6];
                                this.tbxmodelo.Text = Dato_Recivido[7];
                                this.tbxDecimales.Text = Dato_Recivido[8];

                                if (this.tbxDecimales.TextLength == 0) this.tbxDecimales.Text = "2"; //Si la bascula no manda los decimales, es 2.

                                string sHora = String.Format("{0:HH:mm:ss}", DateTime.Now);
                                string sFecha = String.Format("{0:dd/MM/yyyy}", DateTime.Now);

                                Sock_Bascula.enviar(ref Cliente_bascula, "SetHORA," + sHora + "\n", tbxDirIp.Text);

                                Thread.Sleep(50);

                                Sock_Bascula.enviar(ref Cliente_bascula, "SetFECHA," + sFecha + "\n", tbxDirIp.Text);

                                Thread.Sleep(50);

                                workerObject.vEndShowMsg();
                                t.Join();
                                MessageBox.Show(this, Variable.SYS_MSJ[299, Variable.idioma] + " IP " + tbxDirIp.Text + " " + Variable.SYS_MSJ[300, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);

                             }
                            else
                            {
                                this.tbxnserie.Clear();
                                this.tbxmedida.Clear();
                                this.tbxdivminima.Clear();
                                this.tbxIdioma.Clear();
                                this.tbxum.Clear();
                                this.tbxmodelo.Clear();
                                this.tbxDecimales.Clear();
                                workerObject.vEndShowMsg();
                                t.Join();
                                MessageBox.Show(this,Variable.SYS_MSJ[46, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma]);

                            }
                        }
                        else
                        {
                            workerObject.vEndShowMsg();
                            t.Join();
                            MessageBox.Show(this,Variable.SYS_MSJ[33, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma]); //"No Tiene Numero de Serie","Alerta!!!");

                        }
                    }

                    if (Cliente_bascula != null)
                        Cliente_bascula.Close();
                }
                else
                {
                    workerObject.vEndShowMsg();
                    t.Join();
                    this.tbxnserie.Clear();
                    this.tbxmedida.Clear();
                    this.tbxdivminima.Clear();
                    this.tbxIdioma.Clear();
                    this.tbxum.Clear();
                    this.tbxmodelo.Clear();
                    this.tbxDecimales.Clear();
                    MessageBox.Show(this,Variable.SYS_MSJ[34, Variable.idioma] + " " + tbxnserie.Text, Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
            else
            {

                WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[390, Variable.idioma]);
                Thread t = new Thread(workerObject.vShowMsg);
                t.Start();
                while (!t.IsAlive) ;
                Thread.Sleep(1);
                Variable.P_COMM = cbxUSB.SelectedItem.ToString();
                serialPort1 = new SerialPort();
                if (SR.OpenPort(ref serialPort1, Variable.P_COMM, 115200))
                {
                    SR.SendCOMSerial(ref serialPort1, "CX" + (char)9 + (char)10, ref Dato_Recivido);
                    if (Dato_Recivido != null && Dato_Recivido[1] != "")
                    {
                        if (Dato_Recivido[0].IndexOf("CX") >= 0 && Dato_Recivido.Length > 8)
                        {
                            if (Dato_Recivido[6].ToLower() == Variable.FOR_UM[Variable.unidad].ToLower())
                            {
                                this.tbxnserie.Text = Dato_Recivido[1];
                                this.tbxmedida.Text = Dato_Recivido[2];
                                this.tbxdivminima.Text = Dato_Recivido[3];
                                this.tbxIdioma.Text = Dato_Recivido[4];
                                this.tbxum.Text = Dato_Recivido[6];
                                this.tbxmodelo.Text = Dato_Recivido[7];
                                this.tbxDecimales.Text = Dato_Recivido[8];
                                workerObject.vEndShowMsg();
                                t.Join();
                                
                                MessageBox.Show(this,Variable.SYS_MSJ[299, Variable.idioma] + " " + Variable.P_COMM + " " + Variable.SYS_MSJ[300, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                workerObject.vEndShowMsg();
                                t.Join();
                                MessageBox.Show(this, Variable.SYS_MSJ[46, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma]);
                            }
                        }
                        else
                        {
                            workerObject.vEndShowMsg();
                            t.Join();
                            MessageBox.Show(this, Variable.SYS_MSJ[433, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma]); //"No Tiene Numero de Serie","Alerta!!!");
                        }
                    }
                    else
                    {
                        workerObject.vEndShowMsg();
                        t.Join();
                        MessageBox.Show(this, Variable.SYS_MSJ[34, Variable.idioma] + " " + tbxnserie.Text, Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                    SR.ClosePort(ref serialPort1);
                }
                else
                {
                    workerObject.vEndShowMsg();
                    t.Join();
                    MessageBox.Show(this, Variable.SYS_MSJ[34, Variable.idioma] + " " + tbxnserie.Text, Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void cbxUSB_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnConectarBas.Enabled = true;
            Variable.Cargar_puertos();
            cargarCOMs();
        }

        private void btnCancelarBas_Click(object sender, EventArgs e)
        {
            limpiezaTextBoxes();
        }
        #endregion
        #region Captura y validacion
        
        private void cbxConexion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (cbxConexion.SelectedIndex == (int)ESTADO.tipoConexionesEnum.PKWIFI) { this.tbxDirIp.Focus(); } //cbxConexion.SelectedIndex == (int)ESTADO.tipoConexionesEnum.PKETHERNET || 
                else
                {
                    this.cbxUSB.Focus();
                    
                    if (cargarCOMs() <= 0)
                    {
                        Console.Write(Variable.SYS_MSJ[96, Variable.idioma] + (char)10 + Variable.SYS_MSJ[97, Variable.idioma]);
                    }
                    //else { this.cbxUSB.Focus(); }
                }
            }
        }

        private void cbxGrupo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                 this.cbxConexion.Focus(); 
            }
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
            colorbefore = tbxDescrip.BackColor;
            tbxDescrip.BackColor = Color.FromArgb(255, 255, 174);
        }

        private void tbxDescrip_Leave(object sender, EventArgs e) //KeyPressEventArgs e)
        {
            tbxDescrip.BackColor = colorbefore;
        }

        private void cbxGrupo_Enter(object sender, EventArgs e) //KeyPressEventArgs e)
        {
            colorbefore = cbxGrupo.BackColor;
            cbxGrupo.BackColor = Color.FromArgb(255, 255, 174);
        }

        private void cbxGrupo_Leave(object sender, EventArgs e) //KeyPressEventArgs e)
        {
            cbxGrupo.BackColor = colorbefore;
        }

        private void cbxGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbxUSB_DropDown(object sender, EventArgs e)
        {
            Variable.Cargar_puertos();
            cargarCOMs();

            if (Variable.port.Count > 0)
            {
                cbxUSB.SelectedIndex = 0;
            }
        }

        private void tbxNombre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.tbxDescrip.Focus();
        }

        private void tbxDescrip_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.cbxGrupo.Focus();
        }

        private void tbxDirIp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.btnConectarBas.Focus();
        }

        private void cbxUSB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.btnConectarBas.Focus();
        }

        private void cbxConexion_Enter(object sender, EventArgs e)
        {
            colorbefore = cbxConexion.BackColor;
            cbxConexion.BackColor = Color.FromArgb(255, 255, 174);
        }

        private void cbxConexion_Leave(object sender, EventArgs e)
        {
            cbxConexion.BackColor = colorbefore;
        }

        private void cbxUSB_Enter(object sender, EventArgs e)
        {
            colorbefore = cbxUSB.BackColor;
            cbxUSB.BackColor = Color.FromArgb(255, 255, 174);
        }

        private void cbxUSB_Leave(object sender, EventArgs e)
        {
            cbxUSB.BackColor = colorbefore;
        }

        private void lblGrupoComb_Click(object sender, EventArgs e)
        {

        }

        #endregion
        #region Direcciones Ip

        private void tbxDirIp_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) || e.KeyChar == 8 || e.KeyChar == '.')
            {
                switch (e.KeyChar)
                {
                    case '.':

                        if (tbxDirIp.Text.Length > 0)
                        {
                            int iCountDot = tbxDirIp.Text.Count(c => c == '.');

                            if (iCountDot > 2)
                            {
                                e.Handled = true;
                                return;
                            }
                            else
                            {
                                if (Char.IsNumber(tbxDirIp.Text[tbxDirIp.Text.Length - 1]) == false)
                                {
                                    e.Handled = true;
                                    return;
                                }
                            }
                            break;
                        }
                        else
                        {
                            e.Handled = true;
                            return;
                        }
                }

                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void tbxDirIp_Enter(object sender, EventArgs e) //KeyPressEventArgs e)
        {
            colorbefore = tbxDirIp.BackColor;
            tbxDirIp.BackColor = Color.FromArgb(255, 255, 174);
        }

        private void tbxDirIp_Leave(object sender, EventArgs e) //KeyPressEventArgs e)
        {
            tbxDirIp.BackColor = colorbefore;
        }

        public static int iValidarDireccionIp(string sIp, string tipo_ip)
        {
            int iRtaFunct = 0;
            string[] splitIp = sIp.Split('.');

            try
            {
                if (splitIp.Length < 4 || sIp[0] == '.')
                {
                    iRtaFunct = 1;
                }
                else if (splitIp[0] == "" || splitIp[1] == "" || splitIp[2] == "" || splitIp[3] == "")
                {
                    iRtaFunct = 1;
                }
                else if (string.Compare(tipo_ip, "Mascara") == 0)
                {
                    if (Convert.ToInt32(splitIp[0]) > 255 || Convert.ToInt32(splitIp[1]) > 255 || Convert.ToInt32(splitIp[2]) > 255 || Convert.ToInt32(splitIp[3]) > 255)
                    {
                        iRtaFunct = 1;
                    }
                }
                else
                {
                    if (Convert.ToInt32(splitIp[0]) == 0 || Convert.ToInt32(splitIp[0]) > 223 || Convert.ToInt32(splitIp[1]) > 255 || Convert.ToInt32(splitIp[2]) > 255 || Convert.ToInt32(splitIp[3]) > 255)
                    {
                        iRtaFunct = 1;
                    }
                }

                return iRtaFunct;
            }
            catch
            {
                return 1;
            }
        }
        #endregion 
        #region Eventos de botones
        public bool comando(int opcion, int iActionToSave)
        {
            //bool Existe = false;
            switch (opcion)
            {
                case 1://nuevo
                    listadoId_EnBD();
                    limpiezaTextBoxes();
                    Form1.statRegistro = ESTADO.EstadoRegistro.PKNOTRATADO;
                    break;
                case 2://editar
                    activarDesactivarEdicion(true, Color.White);
                    Consulta_EnBD(idbasc);
                    Form1.statRegistro = ESTADO.EstadoRegistro.PKPARCIAL;
                    break;

                case 3://guardar

                    if (buscar_capacidad(Convert.ToInt32(cbxGrupo.SelectedValue), tbxum.Text))
                    {
                        bool bDirIp = false;
                        bool bNombre = false;
                        bool bserie = false;

                        basculaTableAdapter.Fill(baseDeDatosDataSet.Bascula);

                        DataRow[] drr = baseDeDatosDataSet.Bascula.Select("nombre = '" + tbxNombre.Text + "'");

                        foreach (DataRow drbas in drr)
                        {
                            if (drbas["id_bascula"].ToString() != idbasc && drbas["nombre"].ToString() == tbxNombre.Text)
                            {
                                bNombre = true;
                                break;
                            }
                        }

                        drr = baseDeDatosDataSet.Bascula.Select("no_serie = '" + tbxnserie.Text + "'");

                        foreach (DataRow drbas in drr)
                        {
                            if (drbas["id_bascula"].ToString() != idbasc && drbas["no_serie"].ToString() == tbxnserie.Text)
                            {
                                bserie = true;
                                break;
                            }
                        }

                        if (cbxConexion.SelectedIndex == (int)ESTADO.tipoConexionesEnum.PKWIFI)
                        {

                            drr = baseDeDatosDataSet.Bascula.Select("dir_ip = '" + tbxDirIp.Text + "'");

                            foreach (DataRow drbas in drr)
                            {
                                if (drbas["id_bascula"].ToString() != idbasc && drbas["dir_ip"].ToString() == tbxDirIp.Text)
                                {
                                    bDirIp = true;
                                    break;
                                }
                            }
                        }



                        if (bDirIp == true || bNombre == true || bserie == true || tbxnserie.Text == "")
                        {
                            string sMsgAdvertencia = Variable.SYS_MSJ[183, Variable.idioma];  // "No se puede guardar la bascula:";

                            if (bNombre == true)
                            {
                                sMsgAdvertencia = sMsgAdvertencia + " '" + Variable.SYS_MSJ[184, Variable.idioma] + "'";
                            }

                            if (bDirIp == true)
                            {
                                sMsgAdvertencia = sMsgAdvertencia + " '" + Variable.SYS_MSJ[185, Variable.idioma] + "'";
                            }

                            if (bserie == true)
                            {
                                sMsgAdvertencia = sMsgAdvertencia + " '" + Variable.SYS_MSJ[186, Variable.idioma] + "'";
                            }

                            if (tbxnserie.Text == "")
                            {
                                if (bDirIp == true || bNombre == true || bserie == true)
                                {
                                    sMsgAdvertencia = sMsgAdvertencia + ", " + Variable.SYS_MSJ[187, Variable.idioma];
                                }

                                sMsgAdvertencia = sMsgAdvertencia + " '" + Variable.SYS_MSJ[188, Variable.idioma] + "'";
                            }
                            else
                            {
                                sMsgAdvertencia = sMsgAdvertencia + ", " + Variable.SYS_MSJ[187,Variable.idioma]; //  ya existe.";
                            }

                            MessageBox.Show(this, sMsgAdvertencia, Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }

                        if (tbxNombre.Text == "")
                        {
                            MessageBox.Show(this, Variable.SYS_MSJ[343, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            tbxNombre.Focus();
                            return false;
                        }
                        if (Convert.ToInt32(tbxDecimales.Text) != Variable.n_decimal)
                        {
                            MessageBox.Show(this, Variable.SYS_MSJ[422, Variable.idioma]); //"La báscula no pudo ser guardada: Tiene diferente numero de decimales"
                            return false;
                        }

                        if (iActionToSave == 0)     //Nuevo
                        {
                            if (cbxConexion.SelectedIndex == (int)ESTADO.tipoConexionesEnum.PKWIFI)
                            {
                                if (iValidarDireccionIp(tbxDirIp.Text, "IP") == 0)
                                {

                                    //string ck = Convert.ToString(Chk.ChkSum("CX" + (char)9 + (char)10));
                                    Socket Cliente_bascula = Sock_Bascula.conectar(this.tbxDirIp.Text, 50036);
                                    string[] Dato_Recibido = null;

                                    if (Cliente_bascula != null)
                                    {
                                        string Variable_frame = "FXX" + (char)9 + tbxnserie.Text + (char)9 + (char)10;
                                        Sock_Bascula.Envio_Dato(ref Cliente_bascula, tbxDirIp.Text, Variable_frame, ref Dato_Recibido);

                                        if (Dato_Recibido != null && Dato_Recibido.Length == 3 && string.Compare(Dato_Recibido[0], "F0") == 0)
                                        {
                                            Guardar(false, iActionToSave);
                                        }
                                        else
                                        {
                                            MessageBox.Show(this, Variable.SYS_MSJ[44, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show(this, Variable.SYS_MSJ[44, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        return false;
                                    }

                                    MessageBox.Show(this, Variable.SYS_MSJ[43, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    if (tbxmodelo.Text == "WLSD")
                                    {
                                        if (Variable.Habilitar_PubliWLSD == 0)
                                            MessageBox.Show(this, Variable.SYS_MSJ[432, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(this, Variable.SYS_MSJ[44, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    return false;
                                }
                            }
                            else
                            {
                                Variable.P_COMM = cbxUSB.SelectedItem.ToString();
                                serialPort1 = new SerialPort();
                                string[] Dato_Recivido = null;

                                if (SR.OpenPort(ref serialPort1, Variable.P_COMM, 115200))
                                {
                                    string Variable_frame = "FXX" + (char)9 + tbxnserie.Text + (char)9 + (char)10;
                                    SR.SendCOMSerial(ref serialPort1, Variable_frame, ref Dato_Recivido);

                                    if (Dato_Recivido != null && Dato_Recivido.Length == 3 && string.Compare(Dato_Recivido[0], "F0") == 0)
                                    {
                                        Guardar(false, iActionToSave);
                                    }
                                    else
                                    {
                                        MessageBox.Show(this, Variable.SYS_MSJ[302, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        SR.ClosePort(ref serialPort1);
                                        return false;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(this, Variable.SYS_MSJ[302, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    SR.ClosePort(ref serialPort1);
                                    return false;
                                }

                                MessageBox.Show(this, Variable.SYS_MSJ[43, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                                if (tbxmodelo.Text == "WLSD")
                                {
                                    if (Variable.Habilitar_PubliWLSD == 0)
                                        MessageBox.Show(this, Variable.SYS_MSJ[432, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                SR.ClosePort(ref serialPort1);
                            }
                        }
                        else                 //Edicion
                        {
                            if (cbxConexion.SelectedIndex == (int)ESTADO.tipoConexionesEnum.PKWIFI)
                            {
                                if (iValidarDireccionIp(tbxDirIp.Text, "IP") == 0)
                                {

                                    //string ck = Convert.ToString(Chk.ChkSum("CX" + (char)9 + (char)10));
                                    Socket Cliente_bascula = Sock_Bascula.conectar(this.tbxDirIp.Text, 50036);
                                    string[] Dato_Recibido = null;

                                    if (Cliente_bascula != null)
                                    {
                                        string Variable_frame = "FXX" + (char)9 + tbxnserie.Text + (char)9 + (char)10;
                                        Sock_Bascula.Envio_Dato(ref Cliente_bascula, tbxDirIp.Text, Variable_frame, ref Dato_Recibido);

                                        if (Dato_Recibido != null && Dato_Recibido.Length == 3 && string.Compare(Dato_Recibido[0], "F0") == 0)
                                        {
                                            Guardar(true, iActionToSave);
                                        }
                                        else
                                        {
                                            MessageBox.Show(this, Variable.SYS_MSJ[44, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show(this, Variable.SYS_MSJ[44, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        return false;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(this, Variable.SYS_MSJ[44, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    return false;
                                }

                                MessageBox.Show(this, Variable.SYS_MSJ[45, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                Variable.P_COMM = cbxUSB.SelectedItem.ToString();
                                serialPort1 = new SerialPort();
                                string[] Dato_Recivido = null;

                                if (SR.OpenPort(ref serialPort1, Variable.P_COMM, 115200))
                                {
                                    string Variable_frame = "FXX" + (char)9 + tbxnserie.Text + (char)9 + (char)10;
                                    SR.SendCOMSerial(ref serialPort1, Variable_frame, ref Dato_Recivido);

                                    if (Dato_Recivido != null && Dato_Recivido.Length == 3 && string.Compare(Dato_Recivido[0], "F0") == 0)
                                    {
                                        Guardar(true, iActionToSave);
                                    }
                                    else
                                    {
                                        MessageBox.Show(this, Variable.SYS_MSJ[302, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        SR.ClosePort(ref serialPort1);
                                        return false;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(this, Variable.SYS_MSJ[302, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    SR.ClosePort(ref serialPort1);
                                    return false;
                                }

                                MessageBox.Show(this, Variable.SYS_MSJ[45, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                                SR.ClosePort(ref serialPort1);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, Variable.SYS_MSJ[46, Variable.idioma]); //"La báscula No tiene la misma capacidad que el grupo");
                        return false;
                    }
                    
                    //if (Form1.statRegistro == ESTADO.EstadoRegistro.PKNOTRATADO) limpiezaTextBoxes();
                    //if (Form1.statRegistro == ESTADO.EstadoRegistro.PKPARCIAL) activarDesactivarEdicion(false, Color.WhiteSmoke);
                    activarDesactivarEdicion(false, Color.WhiteSmoke);
                    listadoId_EnBD();
                    break;
                case 4://borrar
                    Eliminar();
                   // Form1.statRegistro = ESTADO.EstadoRegistro.PKTRATADO;
                    listadoId_EnBD();
                    limpiezaTextBoxes();
                    break;
                case 5://salir
                    this.Dispose();
                    break;
                case 6://seleccionar
                    activarDesactivarEdicion(false, Color.WhiteSmoke);
                    Consulta_EnBD(idbasc);
                    break;
            }
            ValidarTipoSistema();//este cambiara el sistema en base a lo que se tenga en la base de datos.
            return true;
        }
        #endregion
        private void UserBasculas_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet1.Grupo' Puede moverla o quitarla según sea necesario.
            this.grupoTableAdapter.Fill(this.baseDeDatosDataSet.Grupo);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet1.Bascula' Puede moverla o quitarla según sea necesario.
            this.basculaTableAdapter.Fill(this.baseDeDatosDataSet.Bascula);
            
            listadoId_EnBD();
            cargarCOMs();
        }        
        private void UserBasculas_Resize(object sender, EventArgs e)
        {
            int WScreen = this.ClientSize.Width;

            if (WScreen <= 800 && (iTypeResolution == 0 || iTypeResolution == 2))
            {
                iTypeResolution = 1;
                lblNombre.Location = new Point(7, 17);
                tbxNombre.Location = new Point(86, 13);
                lblDescrip.Location = new Point(7, 55);
                tbxDescrip.Location = new Point(86, 52);
                tbxDescrip.Size = new Size(435, 43);

                lblConexionComb.Location = new Point(7, 167);
                cbxConexion.Location = new Point(86, 171);

                lblGrupoComb.Location = new Point(7, 112);
                cbxGrupo.Location = new Point(86, 115);

                groupConexion.Location = new Point(10, 234);
                groupCaracteristicas.Location = new Point(264, 115);
                this.AutoScroll = true;
             }
            else if (WScreen > 800 && iTypeResolution == 1)
            {
                iTypeResolution = 0;
                lblNombre.Location = new Point(24, 36);
                tbxNombre.Location = new Point(124, 33);
                lblDescrip.Location = new Point(24, 80);
                tbxDescrip.Location = new Point(124, 77);
                tbxDescrip.Size = new Size(669, 43);

                lblConexionComb.Location = new Point(24, 185);
                cbxConexion.Location = new Point(124, 191);

                lblGrupoComb.Location = new Point(24, 137);
                cbxGrupo.Location = new Point(124, 143);

                groupConexion.Location = new Point(27, 247);
                groupCaracteristicas.Location = new Point(511, 143);
                this.AutoScroll = false;
            }
        }       
        private void UserBasculas_SizeChanged(object sender, EventArgs e)
        {

        }
        private void UserBasculas_Leave(object sender, EventArgs e)
        {
            if (!this.Focused) this.Focus();
        }
    }

    public class USState
    {
        private int myId;
        private string myName;

        public USState(string strName, int iId)
        {

            this.myId = iId;
            this.myName = strName;
        }

        public int ShortName
        {
            get
            {
                return myId;
            }
        }

        public string LongName
        {

            get
            {
                return myName;
            }
        }

    }
}