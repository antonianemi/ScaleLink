using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using TorreyTransfer;
using TreeViewBound;

namespace MainMenu
{
    public partial class UserFormatos : UserControl
    {
        public Int32 Num_Grupo;
        public Int32 Num_Bascula;
        public String Nombre_Select;

        Variable.lbasc[] myScale;
        Variable.formato[] myImpresion;
        Variable.formato_size[] myFormato;

        private Label[] Etiquetas;
       // private string[] Atamaño = new string[] { "1-8x16", "2-16x16", "3-16x24", "4-24x24", "5-24x32", "6-16x32" };
        private string[,] list_campo = new string[,]  { {"!",Variable.SYS_MSJ[97,  Variable.idioma]},{"b",Variable.SYS_MSJ[424, Variable.idioma]}, {"C",Variable.SYS_MSJ[98, Variable.idioma]},
                                                        {"c",Variable.SYS_MSJ[99,  Variable.idioma]},{"E",Variable.SYS_MSJ[100, Variable.idioma]},
                                                        {"F",Variable.SYS_MSJ[101, Variable.idioma]},{"f",Variable.SYS_MSJ[102, Variable.idioma]},
                                                        {"G",Variable.SYS_MSJ[103, Variable.idioma]},{"g",Variable.SYS_MSJ[104, Variable.idioma]},
                                                        {"H",Variable.SYS_MSJ[105, Variable.idioma]},{"I",Variable.SYS_MSJ[106, Variable.idioma]},
                                                        {"m",Variable.SYS_MSJ[107, Variable.idioma]},{"N",Variable.SYS_MSJ[108, Variable.idioma]},
                                                        {"P",Variable.SYS_MSJ[109, Variable.idioma]},{"p",Variable.SYS_MSJ[110, Variable.idioma]},
                                                        {"r",Variable.SYS_MSJ[111, Variable.idioma]},{"s",Variable.SYS_MSJ[112, Variable.idioma]},
                                                        {"T",Variable.SYS_MSJ[113, Variable.idioma]},{"t",Variable.SYS_MSJ[114, Variable.idioma]},
                                                        {"V",Variable.SYS_MSJ[115, Variable.idioma]},{"v",Variable.SYS_MSJ[116, Variable.idioma]},
                                                        {"W",Variable.SYS_MSJ[117, Variable.idioma]},{"w",Variable.SYS_MSJ[118, Variable.idioma]} };

        private string[] Acampos = new string[] {Variable.SYS_MSJ[97,  Variable.idioma], Variable.SYS_MSJ[424, Variable.idioma], Variable.SYS_MSJ[98, Variable.idioma], Variable.SYS_MSJ[99, Variable.idioma],
                                                 Variable.SYS_MSJ[100, Variable.idioma], Variable.SYS_MSJ[101, Variable.idioma], Variable.SYS_MSJ[102, Variable.idioma],
                                                 Variable.SYS_MSJ[103, Variable.idioma], Variable.SYS_MSJ[104, Variable.idioma], Variable.SYS_MSJ[105, Variable.idioma],
                                                 Variable.SYS_MSJ[106, Variable.idioma], Variable.SYS_MSJ[107, Variable.idioma], Variable.SYS_MSJ[108, Variable.idioma],
                                                 Variable.SYS_MSJ[109, Variable.idioma], Variable.SYS_MSJ[110, Variable.idioma], Variable.SYS_MSJ[111, Variable.idioma],
                                                 Variable.SYS_MSJ[112, Variable.idioma], Variable.SYS_MSJ[113, Variable.idioma], Variable.SYS_MSJ[114, Variable.idioma],
                                                 Variable.SYS_MSJ[115, Variable.idioma], Variable.SYS_MSJ[116, Variable.idioma], Variable.SYS_MSJ[117, Variable.idioma],Variable.SYS_MSJ[118, Variable.idioma]};
        ///
        /// "!-Espacio","b-Logo", "C-Codigo de Barras","c-Consecutivo","E-Encabezados","F-Fecha",
        /// "f-Texto Fecha","G-Caducidad","g-Texto Caducidad","H-Hora","I-Info. Adicional",
        /// "m-Mensaje Adicional","N-Descripcion de PLU","P-Precio","p-Texto Precio",
        /// "r-Texto Vendedor","s-Gran Total","T-Total","t-Texto Total","V-Vendedor",
        /// "v-Numero Vendedor","W-Peso","w-Texto Peso" };
        /// 
        private bool formato_locked;
        private int Num_Formato;
        ESTADO.botonesEnvioDato DatosEnviado = new ESTADO.botonesEnvioDato();
        ADOutil Conec = new ADOutil();
        Conexion Cte = new Conexion();
        Serial SR = new Serial();
        ArrayList Dato_Nuevo = new ArrayList();
        Socket Cliente_bascula = null;
        Envia_Dato Env = new Envia_Dato();
        private bool cargandoForma = false;
        Color colorbefore;

        #region Inicializacion
        public UserFormatos()
        {
            InitializeComponent();

            this.tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + string.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);

            Etiquetas = new System.Windows.Forms.Label[]{this.for1,this.for2,this.for3,this.for4,this.for5,this.for6,this.for7,this.for8,this.for9,this.for10,
														 this.for11,this.for12,this.for13,this.for14,this.for15,this.for16,this.for17,this.for18,this.for19,this.for20,
														 this.for21,this.for22,this.for23,this.for24,this.for25,this.for26,this.for27,this.for28,this.for29,this.for30,
														 this.for31,this.for32,this.for33,this.for34,this.for35,this.for36,this.for37,this.for38,this.for39,this.for40,
														 this.for41,this.for42,this.for43,this.for44,this.for45,this.for46,this.for47,this.for48,this.for49,this.for50,
														 this.for51,this.for52,this.for53,this.for54,this.for55,this.for56,this.for57,this.for58,this.for59,this.for60};
            asignarEventos();

            myImpresion = new Variable.formato[3];
            myFormato = new Variable.formato_size[3];

            Variable.user_codigoxticket = "pddaattttttt";         //formato codigo de barras ticket, 14 caracteres
            Variable.user_codigoxprod = "pwwwwwtttttt";

        }
        #endregion



        void funcionWLABEL()
        {
            rBxformato3_Standar.Enabled = false;
            rBxformato3_Standar.Visible = false;

            ScrollBarContraEtiqueta.Enabled = false;
            ScrollBarContraEtiqueta.Visible = false;
            lbxCetiqueta.Enabled = false;
            lbxCetiqueta.Visible = false;

            label1.Visible = false;
        }



        #region Consulta y escritura de Base de Datos
                
        private void Asigna_Bascula()
        {

            Conec.CadenaSelect = "SELECT * FROM Bascula ORDER BY id_bascula";

            basculaTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            basculaTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            basculaTableAdapter.Fill(baseDeDatosDataSet.Bascula);

            myScale = new Variable.lbasc[baseDeDatosDataSet.Bascula.Rows.Count];
            int nitem = 0;
            foreach (DataRow dr in baseDeDatosDataSet.Bascula.Rows)
            {
                myScale[nitem].idbas = Convert.ToInt32(dr["id_bascula"].ToString());
                myScale[nitem].gpo = Convert.ToInt32(dr["id_grupo"].ToString());
                myScale[nitem].ip = dr["dir_ip"].ToString();
                myScale[nitem].nserie = dr["no_serie"].ToString();
                myScale[nitem].nombre = dr["nombre"].ToString();
                myScale[nitem].modelo = dr["modelo"].ToString();
                myScale[nitem].cap = dr["capacidad"].ToString();
                myScale[nitem].div = dr["div_minima"].ToString();
                myScale[nitem].tipo = Convert.ToInt16(dr["tipo_conec"].ToString());
                myScale[nitem].pto = dr["puerto"].ToString();
                myScale[nitem].baud = Convert.ToInt32(dr["baud"].ToString());
                nitem++;
            }

        }
        private void Listado_Campos()
        {
            List4.Items.Clear();
            for (int i = 0; i < Acampos.Length; i++)
            {
                List4.Items.Add(Acampos[i]);
            }
        }
        private void Llenar_Formato()
        {
            int nposicion = 0;

            formatoTableAdapter.Fill(baseDeDatosDataSet.Formato);
            
            myFormato = new Variable.formato_size[baseDeDatosDataSet.Formato.Count];
            foreach (DataRow dr in baseDeDatosDataSet.Formato)
            {
                myFormato[nposicion].nformato = Convert.ToInt16(dr["id_formato"].ToString());
                myFormato[nposicion].posdef = dr["posdef"].ToString();
                myFormato[nposicion].possize = dr["possize"].ToString();
                myFormato[nposicion].largo_medio = dr["largo"].ToString();
                myFormato[nposicion].ancho_medio = dr["ancho"].ToString();
                myFormato[nposicion].separacion_medio = dr["separacion"].ToString();
                myFormato[nposicion].nencabezado = dr["Nencabezados"].ToString();
                myFormato[nposicion].ningrediente = dr["Ningredientes"].ToString();
                nposicion++;
            }
        }
        private void Listado_formatos(int nformato)
        {
            formatoTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            formatoTableAdapter.Connection.CreateCommand().CommandText = "SELECT * FROM Formato ORDER BY id_formato";
            formatoTableAdapter.Fill(baseDeDatosDataSet.Formato);
            baseDeDatosDataSet.Formato.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Formato.id_formatoColumn };

            ArrayList lFormatos = new ArrayList();
            lFormatos.Add(new USState("----------", 0));

            if (baseDeDatosDataSet.Formato.Count > 0)
            {
                foreach (DataRow dr in baseDeDatosDataSet.Formato.Rows)
                {
                    lFormatos.Add(new USState(dr["Fields"].ToString(), Convert.ToInt32(dr["id_formato"])));
                }
            }
            cBxFormatoPersonalizados.DataSource = lFormatos;
            cBxFormatoPersonalizados.ValueMember = "ShortName";
            cBxFormatoPersonalizados.DisplayMember = "LongName";

            if (cBxFormatoPersonalizados.Items.Count > 0) cBxFormatoPersonalizados.SelectedValue = nformato;
            else cBxFormatoPersonalizados.SelectedValue = 0;

           // cBxFormatoPersonalizados.DataSource = baseDeDatosDataSet.Formato;
            //cBxFormatoPersonalizados.DisplayMember = baseDeDatosDataSet.Formato.FieldsColumn.ColumnName.ToString();
            //cBxFormatoPersonalizados.ValueMember = baseDeDatosDataSet.Formato.id_formatoColumn.ColumnName.ToString();
            //if (cBxFormatoPersonalizados.Items.Count > 0) cBxFormatoPersonalizados.SelectedValue = nformato;                        
        }

        private int buscar_formato(int nformato)
        {
            int pos = -1;

            for (int i = 0; i < myFormato.Length; i++)
            {
                if (myFormato[i].nformato == nformato)
                {
                    pos = i;
                    break;
                }
            }
            return pos;
        }
        private void Consulta_EnBD()
        {
            cargandoForma = true;
            formatoTableAdapter.Fill(baseDeDatosDataSet.Formato);
            impresorTableAdapter.Fill(baseDeDatosDataSet.Impresor);

            DataRow[] DA = baseDeDatosDataSet.Impresor.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);
            if (DA.Length > 0)
            {
                DataRow dr = DA[0];
                Mostrar_Dato(dr);
            }
            cargandoForma = false;
            activarDesactivarEdicion(false, Color.WhiteSmoke);
        }

        private void Mostrar_Dato(DataRow dr)
        {
            Variable.user_EAN_UPCxProd = Convert.ToSByte(dr["EAN_etiq"]);
            Variable.user_EAN_UPCxTicket = Convert.ToSByte(dr["EAN_papel"]);
            Variable.user_formato.ncodigobar_xprod = Convert.ToSByte(dr["barcode_prod"]);
            Variable.user_formato.ncodigobar_xticket = Convert.ToSByte(dr["barcode_ticket"]);
            Variable.user_nutri = Convert.ToSByte(dr["nutrientes"]);
            if (Convert.ToInt16(dr["f_personalizado_papel"].ToString()) > 0)
                Variable.user_Nformato_ticket = Convert.ToInt16(dr["f_personalizado_papel"].ToString());
            if (Convert.ToInt16(dr["f_personalizado_etiq"].ToString()) > 0)
               Variable.user_Nformato_producto  = Convert.ToInt16(dr["f_personalizado_etiq"].ToString());

            Variable.user_depto = dr["departamento"].ToString();
            Variable.user_prefijo = dr["prefijo"].ToString();
            Variable.user_corrimientopieza = Convert.ToSByte(dr["cero_pieza"]);
            Variable.user_contrastepapel = Convert.ToInt16(dr["c_papel"].ToString());
            Variable.user_contrasteetiqueta = Convert.ToInt16(dr["c_etiqueta"].ToString());
            Variable.user_retardoimpresion = Convert.ToInt16(dr["retardo"].ToString());

            this.texto_codigoxprod.Text = dr["barcode_personal_prod"].ToString();
            this.texto_codigoxticket.Text = dr["barcode_personal_ticket"].ToString();
            this.texto_depto.Text = dr["departamento"].ToString();
            this.texto_prefijo.Text = dr["prefijo"].ToString();
            this.texto_corrimiento.Text = dr["cero_pieza"].ToString();
            if (Convert.ToInt16(dr["c_etiqueta"].ToString()) > 0)
                this.ScrollBarContraEtiqueta.Value = Convert.ToInt16(dr["c_etiqueta"].ToString());
            else this.ScrollBarContraEtiqueta.Value = 1;
            if (Convert.ToInt16(dr["c_papel"].ToString()) > 0)
                this.ScrollBarContraPapel.Value = Convert.ToInt16(dr["c_papel"].ToString());
            else this.ScrollBarContraPapel.Value = 1;
            this.ScrollBarRetardo.Value = Convert.ToInt16(dr["retardo"].ToString());
            this.lbxCetiqueta.Text = ScrollBarContraEtiqueta.Value.ToString();
            this.lbxCpapel.Text = ScrollBarContraPapel.Value.ToString();
            this.lbxRetardo.Text = dr["retardo"].ToString();
            this.tbxFecha.Text = String.Format(Variable.F_Hora,Convert.ToDateTime(dr["actualizado"].ToString())) + " " + String.Format(Variable.FOR_FECHAS[Variable.ffecha],Convert.ToDateTime(dr["actualizado"].ToString()));

            /*if (Variable.user_EAN_UPCxProd == 0) rBxEanPrd.Checked = true;
            else rBxUpcPrd.Checked = true;
            if (Variable.user_EAN_UPCxTicket == 0) rBxEanTicket.Checked = true;
            else rBxUpcTicket.Checked = true;*/

            #region CodigoBarraxProducto

            int Auxncodigobar_xprod = Variable.user_formato.ncodigobar_xprod;

            if (Variable.user_EAN_UPCxProd == 0)
            {
                rBxEanPrd.Checked = true;
                cBxCodigoPrd.Items.Clear();
                cBxCodigoPrd.Text = "";
                cBxCodigoPrd.Items.Add(Variable.SYS_MSJ[136, Variable.idioma]); //"EAN13(pccccccwwwwwv) Peso");
                cBxCodigoPrd.Items.Add("EAN13(pcccccctttttv) Total");
                cBxCodigoPrd.Items.Add(Variable.SYS_MSJ[142, Variable.idioma]);  //"Personalizado");
                cBxCodigoPrd.Enabled = true;
            }
            else if (Variable.user_EAN_UPCxProd == 1)
            {
                rBxUpcPrd.Checked = true;
                cBxCodigoPrd.Items.Clear();
                cBxCodigoPrd.Text = "";
                cBxCodigoPrd.Items.Add(Variable.SYS_MSJ[137, Variable.idioma]); //"UPC12(xpcccccwwwwwv) Peso");
                cBxCodigoPrd.Items.Add("UPC12(xpccccctttttv) Total");
                cBxCodigoPrd.Items.Add(Variable.SYS_MSJ[142, Variable.idioma]);  //"Personalizado");
                cBxCodigoPrd.Enabled = true;
            }
            else
            {
                rBxNoCodePrd.Checked = true;
                cBxCodigoPrd.Items.Clear();
                cBxCodigoPrd.Text = "";
                cBxCodigoPrd.Enabled = false;
            }

            this.cBxCodigoPrd.SelectedIndex = Auxncodigobar_xprod;
            Variable.user_formato.ncodigobar_xprod = Auxncodigobar_xprod;

            if (Variable.user_formato.ncodigobar_xprod != 2)
            {
                texto_codigoxprod.BackColor = Color.WhiteSmoke;
                texto_codigoxprod.Enabled = false;
                texto_codigoxprod.Text = "";
            }
            else
            {
                texto_codigoxprod.Text = dr["barcode_personal_prod"].ToString();
            }
            #endregion

            #region CodigoBarraxTicket

            int Auxncodigobar_xticket = Variable.user_formato.ncodigobar_xticket;

            if (Variable.user_EAN_UPCxTicket == 0)
            {
                rBxEanTicket.Checked = true;
                cBxCodigoTicket.Items.Clear();
                cBxCodigoTicket.Text = "";
                cBxCodigoTicket.Items.Add(Variable.SYS_MSJ[138, Variable.idioma]);  //"EAN13(pxaannttttttv) Vendedor");
                cBxCodigoTicket.Items.Add(Variable.SYS_MSJ[139, Variable.idioma]);  //"EAN13(pxddnnttttttv) Depto.");
                cBxCodigoTicket.Items.Add(Variable.SYS_MSJ[142, Variable.idioma]);  //"Personalizado");
                cBxCodigoTicket.Enabled = true;
            }
            else if (Variable.user_EAN_UPCxTicket == 1)
            {
                rBxUpcTicket.Checked = true;
                cBxCodigoTicket.Items.Clear();
                cBxCodigoTicket.Text = "";
                cBxCodigoTicket.Items.Add(Variable.SYS_MSJ[140, Variable.idioma]);  //"UPC12 (xpaannttttttv) Vendedor");
                cBxCodigoTicket.Items.Add(Variable.SYS_MSJ[141, Variable.idioma]);  //"UPC12 (xpddnnttttttv) Depto.");
                cBxCodigoTicket.Items.Add(Variable.SYS_MSJ[142, Variable.idioma]);  //"Personalizado");
                cBxCodigoTicket.Enabled = true;
            }
            else
            {
                rBxNoCodeTicket.Checked = true;
                cBxCodigoTicket.Items.Clear();
                cBxCodigoTicket.Text = "";
                cBxCodigoTicket.Enabled = false;
            }

            this.cBxCodigoTicket.SelectedIndex = Auxncodigobar_xticket;
            Variable.user_formato.ncodigobar_xticket = Auxncodigobar_xticket;

            if (Variable.user_formato.ncodigobar_xticket != 2)
            {
                texto_codigoxticket.BackColor = Color.WhiteSmoke;
                texto_codigoxticket.Enabled = false;
                texto_codigoxticket.Text = "";
            }
            else
            {
                texto_codigoxticket.Text = dr["barcode_personal_ticket"].ToString();
            }
            #endregion

            //this.cBxCodigoTicket.SelectedIndex = Variable.user_formato.ncodigobar_xticket;

            if (Variable.user_nutri == 1) chkNutriente.Checked = true;
            else chkNutriente.Checked = false;

            if (Convert.ToSByte(dr["tipoimp"]) == 0)
            {
                rBxTipopapel.Checked = true;
                if (Convert.ToSByte(dr["formato_papel"].ToString()) == 0)
                {
                    Variable.user_formato.for_papel_tipoimpre = 0;
                    rBxformato1_Standar.Checked = true;
                    cBxFormatoPersonalizados.Enabled = false;
                    ActivaDesactivaFormato(false, Color.WhiteSmoke);
                    formato_locked = true;
                }
                
                if (Convert.ToSByte(dr["formato_papel"].ToString()) == 1)
                {
                    Variable.user_formato.for_papel_tipoimpre = 1;
                    rBxformato2_Standar.Checked = true;
                    cBxFormatoPersonalizados.Enabled = false;
                    ActivaDesactivaFormato(false, Color.WhiteSmoke);
                    formato_locked = true;
                }
                
                if (Convert.ToSByte(dr["formato_papel"].ToString()) == 2)
                {
                    cBxFormatoPersonalizados.Enabled = true;
                    rBxformato_Personalizado.Checked = true;
                    Variable.user_formato.for_papel_tipoimpre = 2;
                    formato_locked = false;
                    DataRow[] DF = baseDeDatosDataSet.Formato.Select("id_formato = " + Variable.user_Nformato_ticket);
                    if (DF.Length > 0)
                    {
                        Mostrar_Dato(DF[0], Variable.user_Nformato_ticket);
                    }
                }
                if (Convert.ToSByte(dr["formato_papel"].ToString()) == 3)
                {
                    Variable.user_formato.for_papel_tipoimpre = 3;
                    rBxformato3_Standar.Checked = true;
                    cBxFormatoPersonalizados.Enabled = false;
                    ActivaDesactivaFormato(false, Color.WhiteSmoke);
                    formato_locked = true;
                }
                rBxTipopapel.Checked = true;
            }
            else
            {             
                if (Convert.ToSByte(dr["formato_etiq"].ToString()) == 0)
                {
                    Variable.user_formato.for_ecsep_tipoimpre = 0;
                    rBxformato1_Standar.Checked = true;
                    cBxFormatoPersonalizados.Enabled = false;
                    ActivaDesactivaFormato(false, Color.WhiteSmoke);
                    formato_locked = true;
                }                
                if (Convert.ToSByte(dr["formato_etiq"].ToString()) == 1)
                {
                    Variable.user_formato.for_ecsep_tipoimpre = 1;
                    rBxformato2_Standar.Checked = true;
                    cBxFormatoPersonalizados.Enabled = false;
                    ActivaDesactivaFormato(false, Color.WhiteSmoke);
                    formato_locked = true;
                }                
                if (Convert.ToSByte(dr["formato_etiq"].ToString()) == 2)
                {
                    cBxFormatoPersonalizados.Enabled = true;
                    rBxformato_Personalizado.Checked = true;
                    Variable.user_formato.for_ecsep_tipoimpre = 2;
                    formato_locked = false;
                    DataRow[] DF = baseDeDatosDataSet.Formato.Select("id_formato = " + Variable.user_Nformato_producto);
                    if (DF.Length > 0)
                    {
                        Mostrar_Dato(DF[0], Variable.user_Nformato_producto);
                    }
                    else
                    {
                        btnAddFormato.Enabled = false;
                        btnDelFormato.Enabled = false;
                        btnSaveFormato.Enabled = false;
                    }
                }
                if (Convert.ToSByte(dr["formato_etiq"].ToString()) == 3)
                {
                    Variable.user_formato.for_ecsep_tipoimpre = 3;
                    rBxformato3_Standar.Checked = true;
                    cBxFormatoPersonalizados.Enabled = false;
                    ActivaDesactivaFormato(false, Color.WhiteSmoke);
                    formato_locked = true;
                }
                rBxTipoetiqueta.Checked = true;  
            }           
        }
        private void Mostrar_Dato(DataRow dr, int nformato)
        {
            int elemento;
            
            this.texto_numenca.Text = dr["Nencabezados"].ToString();
            this.texto_numing.Text = dr["Ningredientes"].ToString();
            this.tbxAncho.Text = dr["ancho"].ToString();
            this.tbxLargo.Text = dr["largo"].ToString();
            this.tbxSeparacion.Text = dr["separacion"].ToString();
            if (nformato > 0)
            {                
                cBxFormatoPersonalizados.SelectedValue = nformato;
                elemento = cBxFormatoPersonalizados.SelectedIndex;
                Variable.user_formatosize.nformato = nformato;
                Num_Formato = nformato;
                Variable.user_formato3_posdef = dr["posdef"].ToString();
                Variable.user_formato3_possize = dr["possize"].ToString();
                Llenar_etiqueta(Variable.user_formato3_posdef, Variable.user_formato3_possize);
            }
            btnAddFormato.Enabled = true;
            btnDelFormato.Enabled = true;
            btnSaveFormato.Enabled = true;
        }

        private void Mostrar_Formato(DataRow dr, bool papel)
        {
            if (papel == true)
            {
                if (Convert.ToSByte(dr["formato_papel"].ToString()) == 0)
                {
                    Variable.user_formato.for_papel_tipoimpre = 0;
                    rBxformato1_Standar.Checked = true;
                    cBxFormatoPersonalizados.Enabled = false;
                    ActivaDesactivaFormato(false, Color.WhiteSmoke);
                    formato_locked = true;
                }

                if (Convert.ToSByte(dr["formato_papel"].ToString()) == 1)
                {
                    Variable.user_formato.for_papel_tipoimpre = 1;
                    rBxformato2_Standar.Checked = true;
                    cBxFormatoPersonalizados.Enabled = false;
                    ActivaDesactivaFormato(false, Color.WhiteSmoke);
                    formato_locked = true;
                }

                if (Convert.ToSByte(dr["formato_papel"].ToString()) == 2)
                {
                    cBxFormatoPersonalizados.Enabled = true;
                    rBxformato_Personalizado.Checked = true;
                    Variable.user_formato.for_papel_tipoimpre = 2;
                    formato_locked = false;
                    DataRow[] DF = baseDeDatosDataSet.Formato.Select("id_formato = " + Variable.user_Nformato_ticket);
                    if (DF.Length > 0)
                    {
                        Mostrar_Dato(DF[0], Variable.user_Nformato_ticket);
                    }
                }
                if (Convert.ToSByte(dr["formato_papel"].ToString()) == 3)
                {
                    Variable.user_formato.for_papel_tipoimpre = 3;
                    rBxformato3_Standar.Checked = true;
                    cBxFormatoPersonalizados.Enabled = false;
                    ActivaDesactivaFormato(false, Color.WhiteSmoke);
                    formato_locked = true;
                }
            }
            else
            {
                if (Convert.ToSByte(dr["formato_etiq"].ToString()) == 0)
                {
                    Variable.user_formato.for_ecsep_tipoimpre = 0;
                    rBxformato1_Standar.Checked = true;
                    cBxFormatoPersonalizados.Enabled = false;
                    ActivaDesactivaFormato(false, Color.WhiteSmoke);
                    formato_locked = true;
                }
                if (Convert.ToSByte(dr["formato_etiq"].ToString()) == 1)
                {
                    Variable.user_formato.for_ecsep_tipoimpre = 1;
                    rBxformato2_Standar.Checked = true;
                    cBxFormatoPersonalizados.Enabled = false;
                    ActivaDesactivaFormato(false, Color.WhiteSmoke);
                    formato_locked = true;
                }
                if (Convert.ToSByte(dr["formato_etiq"].ToString()) == 2)
                {
                    cBxFormatoPersonalizados.Enabled = true;
                    rBxformato_Personalizado.Checked = true;
                    Variable.user_formato.for_ecsep_tipoimpre = 2;
                    formato_locked = false;
                    DataRow[] DF = baseDeDatosDataSet.Formato.Select("id_formato = " + Variable.user_Nformato_producto);
                    if (DF.Length > 0)
                    {
                        Mostrar_Dato(DF[0], Variable.user_Nformato_producto);
                    }
                    else
                    {
                        btnAddFormato.Enabled = false;
                        btnDelFormato.Enabled = false;
                        btnSaveFormato.Enabled = false;
                    }
                }
                if (Convert.ToSByte(dr["formato_etiq"].ToString()) == 3)
                {
                    Variable.user_formato.for_ecsep_tipoimpre = 3;
                    rBxformato3_Standar.Checked = true;
                    cBxFormatoPersonalizados.Enabled = false;
                    ActivaDesactivaFormato(false, Color.WhiteSmoke);
                    formato_locked = true;
                }
            }      

        }

        private void activarDesactivarEdicion(bool pbActivar, Color clActivar)
        {
            this.texto_codigoxprod.Enabled = pbActivar;
            this.texto_codigoxticket.Enabled = pbActivar;
            this.texto_depto.Enabled = pbActivar;
            this.texto_prefijo.Enabled = pbActivar;
            this.texto_corrimiento.Enabled = pbActivar;           
            this.ScrollBarContraEtiqueta.Enabled = pbActivar;
            this.ScrollBarContraPapel.Enabled = pbActivar;            
            this.ScrollBarRetardo.Enabled = pbActivar;
            this.rBxEanPrd.Enabled = pbActivar;
            this.rBxEanTicket.Enabled = pbActivar;
            this.rBxUpcPrd.Enabled = pbActivar;
            this.rBxUpcTicket.Enabled = pbActivar;
            this.rBxNoCodePrd.Enabled = pbActivar;
            this.rBxNoCodeTicket.Enabled = pbActivar;
            this.rBxTipoetiqueta.Enabled = pbActivar;
            this.rBxTipopapel.Enabled = pbActivar;
            this.rBxformato_Personalizado.Enabled = pbActivar;
            this.rBxformato1_Standar.Enabled = pbActivar;
            this.rBxformato2_Standar.Enabled = pbActivar;
            this.rBxformato3_Standar.Enabled = pbActivar;
            this.chkNutriente.Enabled = pbActivar;           
            this.cBxCodigoPrd.Enabled = pbActivar;
            this.cBxCodigoTicket.Enabled = pbActivar;
            this.cBxFormatoPersonalizados.Enabled = pbActivar;
            
            this.texto_codigoxprod.BackColor = clActivar;
            this.texto_codigoxticket.BackColor = clActivar;
            this.texto_depto.BackColor = clActivar;
            this.texto_prefijo.BackColor = clActivar;
            this.texto_corrimiento.BackColor = clActivar;            
        }
        private void ActivaDesactivaFormato(bool pbActivar, Color clActivar)
        {
            for (int i = 0; i < Etiquetas.Length; i++)
            {
                Etiquetas[i].Enabled = pbActivar;
                Etiquetas[i].BackColor = clActivar;
            }
            this.List4.Enabled = pbActivar;
            this.List4.BackColor = clActivar;

            this.texto_numenca.BackColor = clActivar;
            this.texto_numing.BackColor = clActivar;
            this.tbxAncho.BackColor = clActivar;
            this.tbxLargo.BackColor = clActivar;
            this.tbxSeparacion.BackColor = clActivar;

            this.texto_numenca.Enabled = pbActivar;
            this.texto_numing.Enabled = pbActivar;
            this.tbxAncho.Enabled = pbActivar;
            this.tbxLargo.Enabled = pbActivar;
            this.tbxSeparacion.Enabled = pbActivar;
            
            this.btnAddFormato.Enabled = pbActivar;
            this.btnDelFormato.Enabled = pbActivar;
            this.btnSaveFormato.Enabled = pbActivar;
        }
        private void limpiezaTextBoxes()
        {
            activarDesactivarEdicion(true, Color.White);

            for (int i = 0; i < Etiquetas.Length; i++)
            {
                Etiquetas[i].Text = "";
            }

            this.texto_codigoxprod.Text = "";     //codigo de barra por producto personalizado
            this.texto_codigoxticket.Text = "";   //codigo de barra por ticket personalizado
            this.texto_depto.Text = "0";        //numero de departamento
            this.texto_prefijo.Text = "0";      //numero de prefijo        
            this.texto_corrimiento.Text = "0";
            this.cBxFormatoPersonalizados.SelectedIndex = 0;
            this.ScrollBarContraEtiqueta.Value = 6;
            this.ScrollBarContraPapel.Value = 6;           
            this.ScrollBarRetardo.Value = 0;
            Form1.statRegistro = ESTADO.EstadoRegistro.PKNOTRATADO;
            lbxCetiqueta.Text = this.ScrollBarContraEtiqueta.Value.ToString();
            lbxCpapel.Text = this.ScrollBarContraPapel.Value.ToString();
        }
        private void limpiezaLabelText()
        {           
            Num_Formato = Convert.ToInt32(muestraAutoincrementoId("SELECT id_formato FROM Formato ORDER BY id_formato desc"));
            ActivaDesactivaFormato(true, Color.White);
            for (int i = 0; i < Etiquetas.Length; i++)
            {
                Etiquetas[i].Text = "";
            }
            this.texto_numenca.Text = "0";      //numero de encabezado
            this.texto_numing.Text = "0";       //numero de ingrediente
            this.tbxAncho.Text = "0";              //tamaño de anchura
            this.tbxLargo.Text = "0";
            this.tbxSeparacion.Text = "0";
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
        private void GuardarImpresion(bool Existe)
        {
            object[] clave = new object[2];
            clave[0] = Num_Bascula;
            clave[1] = Num_Grupo;

            string fecha_act = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);
            tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);
               
            if (!Existe)
            { 
                Conec.CadenaSelect = "INSERT INTO Impresor " +
                "(id_bascula, id_grupo, c_papel, c_etiqueta, tipoimp, formato_papel, formato_etiq, EAN_etiq,EAN_papel,barcode_prod,barcode_ticket, barcode_personal_ticket," +
                "barcode_personal_prod,cero_pieza,prefijo,departamento,retardo,nutrientes,f_personalizado_papel, f_personalizado_etiq,actualizado,pendiente)" +
               "VALUES (" + Num_Bascula + "," + 
               Num_Grupo + "," + 
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
               true + ")";

                Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Impresor.TableName);

            }
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
                ", barcode_prod = " +  Variable.user_formato.ncodigobar_xprod + 
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
            }
        }
        private void GuardarFormato(int nformato,bool existe)
        {
            string fecha_act = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);
            tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);
            
            if (rBxformato_Personalizado.Checked)
            {
                Variable.user_formato3_posdef = Leer_etiqueta();
                Variable.user_formato3_possize = Leer_posicion();
            }
            if (!existe)
            {
                Conec.CadenaSelect = "INSERT INTO Formato " +
                "(id_formato, Fields, posdef, possize, largo, ancho,separacion,Nencabezados,Ningredientes,actualizado,pendiente)" +
               "VALUES (" + nformato + ",'" + 
               Variable.SYS_MSJ[405,Variable.idioma] + " " + nformato.ToString() + "','" + 
               Variable.user_formato3_posdef + "','" + 
               Variable.user_formato3_possize + "'," + 
               Convert.ToInt16(tbxLargo.Text) + "," + 
               Convert.ToInt16(tbxAncho.Text) + "," + 
               Convert.ToInt16(tbxSeparacion.Text) + "," + 
               Convert.ToInt16(texto_numenca.Text) + "," + 
               Convert.ToInt16(texto_numing.Text) + ",'" + 
               fecha_act + "'," + 
               true + ")";

                Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Formato.TableName);
            }
            else
            {               

               // Conec.Condicion = "id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo + " and id_formato = " + nformato; 
                Conec.Condicion = "id_formato = " + nformato; 
                Conec.CadenaSelect = "UPDATE Formato SET " +
                    "Fields = '" + Variable.SYS_MSJ[405, Variable.idioma] + " " +nformato.ToString() + "'," +
                    "posdef = '" + Variable.user_formato3_posdef + "'," +
                    "possize = '" + Variable.user_formato3_possize + "'," +
                    "largo = " + Convert.ToInt16(tbxLargo.Text) + "," +
                    "ancho = " + Convert.ToInt16(tbxAncho.Text) + "," +
                    "separacion = " + Convert.ToInt16(tbxSeparacion.Text) + "," +
                    "Nencabezados = " + Convert.ToInt16(texto_numenca.Text) + "," +
                    "Ningredientes = " + Convert.ToInt16(texto_numing.Text) + "," +
                    "pendiente = " + true + ","+
                    "actualizado = '" + fecha_act + "' "+
                    "WHERE (" + Conec.Condicion + ")";

                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Formato.TableName);
                formatoTableAdapter.Fill(baseDeDatosDataSet.Formato);
            }
        }
        #endregion

        #region Botones del ToolStripMenu y de buttom
        private void StripCerrar_Click(object sender, EventArgs e)
        {
            ToolStripManager.RevertMerge("toolStrip3");
            this.Dispose();
        }
        private void StripEnviar_Click(object sender, EventArgs e)
        {
            impresorTableAdapter.Fill(baseDeDatosDataSet.Impresor);
            formatoTableAdapter.Fill(baseDeDatosDataSet.Formato);

            WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[251, Variable.idioma]);
            Thread t = new Thread(workerObject.vShowMsg);
            t.Start();

            int BasculasActualizadas = 0;
            int NumeroDeBaculas = 0;

            if (Num_Grupo != 0)
            {
                for (int pos = 0; pos < myScale.Length; pos++)  //Num de Basculas en el grupo
                    if (myScale[pos].gpo == Num_Grupo) NumeroDeBaculas++;

                for (int pos = 0; pos < myScale.Length; pos++)
                {
                    if (myScale[pos].gpo == Num_Grupo)
                    {
                        Variable.P_COMM = myScale[pos].pto;
                        BasculasActualizadas++;

                        workerObject.vUpdateMsg(Variable.SYS_MSJ[251, Variable.idioma] + " " + myScale[pos].nserie.ToString());

                        if (myScale[pos].tipo == (int)ESTADO.tipoConexionesEnum.PKWIFI)
                        {
                            if (EnviaDatosA_Bascula((int)DatosEnviado, myScale[pos].ip))  //, Num_gpo, Num_basc);
                            {
                                BasculasActualizadas--;
                                if (MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma] + " "
                                    + myScale[pos].nserie + " " + Variable.SYS_MSJ[417, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma],
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No) break;
                            }
                        }
                        else
                        {
                            if (EnviaDatosA_Bascula((int)DatosEnviado, myScale[pos].pto, myScale[pos].baud))
                            {
                                BasculasActualizadas--;
                                if (MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma] + " "
                                    + myScale[pos].nserie + " " + Variable.SYS_MSJ[417, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma],
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No) break;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int pos = 0; pos < myScale.Length; pos++)  //Num de Basculas con el mismo Num_Bascula
                    if (myScale[pos].idbas == Num_Bascula) NumeroDeBaculas++;

                for (int pos = 0; pos < myScale.Length; pos++)
                {
                    if (myScale[pos].idbas == Num_Bascula)
                    {

                        workerObject.vUpdateMsg(Variable.SYS_MSJ[251, Variable.idioma] + " " + myScale[pos].nombre);
                        BasculasActualizadas++;

                        Variable.P_COMM = myScale[pos].pto;
                        if (myScale[pos].tipo == (int)ESTADO.tipoConexionesEnum.PKWIFI)
                        {
                            if (EnviaDatosA_Bascula((int)DatosEnviado, myScale[pos].ip))  //, Num_gpo, Num_basc);
                            {
                                BasculasActualizadas--;
                                MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma]
                                    + " " + myScale[pos].nserie + ".", Variable.SYS_MSJ[42, Variable.idioma],
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                break;
                            }
                        }
                        else
                        {
                            if (EnviaDatosA_Bascula((int)DatosEnviado, myScale[pos].pto, myScale[pos].baud))
                            {
                                BasculasActualizadas--;
                                MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma]
                                    + " " + myScale[pos].nserie + ".", Variable.SYS_MSJ[42, Variable.idioma],
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                break;
                            }
                        }
                        break;
                    }
                }
            }

            Thread.Sleep(50);

            workerObject.vEndShowMsg();
            t.Join();

            MessageBox.Show(this, Variable.SYS_MSJ[418, Variable.idioma] + " " + BasculasActualizadas + " "
                + Variable.SYS_MSJ[419, Variable.idioma] + " " + NumeroDeBaculas,
                Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void StripGuardar_Click(object sender, EventArgs e)
        {
            bool Existe = false;
            Form1.statRegistro = ESTADO.EstadoRegistro.PKTRATADO;


            if (!rBxEanPrd.Checked && !rBxUpcPrd.Checked && !rBxNoCodePrd.Checked)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[411, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!rBxEanTicket.Checked && !rBxUpcTicket.Checked && !rBxNoCodeTicket.Checked)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[412, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!rBxTipopapel.Checked && !rBxTipoetiqueta.Checked)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[413, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int barcodeFormat = 0;
            if (rBxEanPrd.Checked == true) barcodeFormat = 0;
            else if (rBxUpcPrd.Checked == true) barcodeFormat = 1;
            else if (rBxNoCodePrd.Checked == true) barcodeFormat = 2;
            if (cBxCodigoPrd.SelectedIndex == 2 && !Validando_codigobar(texto_codigoxprod.Text, barcodeFormat))
            {

                MessageBox.Show(this, Variable.SYS_MSJ[414, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error);
                texto_codigoxprod.Focus();
                return;
            }
            if (rBxEanTicket.Checked == true) barcodeFormat = 0;
            else if (rBxUpcTicket.Checked == true) barcodeFormat = 1;
            else if (rBxNoCodeTicket.Checked == true) barcodeFormat = 2;
            if (cBxCodigoTicket.SelectedIndex == 2 && !Validando_codigobar(texto_codigoxticket.Text,barcodeFormat))
            {
                MessageBox.Show(this, Variable.SYS_MSJ[415, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error);
                texto_codigoxticket.Focus();
                return;
            }

            if (texto_corrimiento.Text == "")
            {
                MessageBox.Show(this, Variable.SYS_MSJ[35, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error);
                texto_corrimiento.Focus();
                return;
            }
            if (texto_depto.Text == "")
            {
                MessageBox.Show(this, Variable.SYS_MSJ[35, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error);
                texto_depto.Focus();
                return;
            }
            if (texto_prefijo.Text == "")
            {
                MessageBox.Show(this, Variable.SYS_MSJ[35, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Error);
                texto_prefijo.Focus();
                return;
            }
            
            Variable.user_contrastepapel = Convert.ToInt16(lbxCpapel.Text);
            Variable.user_contrasteetiqueta = Convert.ToInt16(lbxCetiqueta.Text);
            Variable.user_retardoimpresion = Convert.ToInt16(lbxRetardo.Text);

            if (rBxTipopapel.Checked) Variable.user_formato.medio_imp = 0;
            if (rBxTipoetiqueta.Checked) Variable.user_formato.medio_imp = 1;
            if (rBxTipopapel.Checked && rBxformato1_Standar.Checked) Variable.user_formato.for_papel_tipoimpre = 0;
            if (rBxTipopapel.Checked && rBxformato2_Standar.Checked) Variable.user_formato.for_papel_tipoimpre = 1;
            if (rBxTipopapel.Checked && rBxformato_Personalizado.Checked) Variable.user_formato.for_papel_tipoimpre = 2;
            if (rBxTipopapel.Checked && rBxformato3_Standar.Checked) Variable.user_formato.for_papel_tipoimpre = 3;

            if (rBxTipoetiqueta.Checked && rBxformato1_Standar.Checked) Variable.user_formato.for_ecsep_tipoimpre = 0;
            if (rBxTipoetiqueta.Checked && rBxformato2_Standar.Checked) Variable.user_formato.for_ecsep_tipoimpre = 1;
            if (rBxTipoetiqueta.Checked && rBxformato_Personalizado.Checked) Variable.user_formato.for_ecsep_tipoimpre = 2;
            if (rBxTipoetiqueta.Checked && rBxformato3_Standar.Checked) Variable.user_formato.for_ecsep_tipoimpre = 3;

            if (rBxEanPrd.Checked) Variable.user_EAN_UPCxProd = 0;
            if (rBxUpcPrd.Checked) Variable.user_EAN_UPCxProd = 1;
            if (rBxEanTicket.Checked) Variable.user_EAN_UPCxTicket = 0;
            if (rBxUpcTicket.Checked) Variable.user_EAN_UPCxTicket = 1;

            Variable.user_formato.ncodigobar_xprod = cBxCodigoPrd.SelectedIndex;
            Variable.user_formato.ncodigobar_xticket = cBxCodigoTicket.SelectedIndex;
            Variable.user_codigoxticket = this.texto_codigoxticket.Text;
            Variable.user_codigoxprod = this.texto_codigoxprod.Text;

            Variable.user_corrimientopieza = Convert.ToSByte(texto_corrimiento.Text);
            Variable.user_prefijo = texto_prefijo.Text;
            Variable.user_depto = texto_depto.Text;

            if (chkNutriente.Checked) Variable.user_nutri = 1;
            else Variable.user_nutri = 0;

            OleDbDataReader OLConfig = Conec.Obtiene_Dato("Select * From Impresor Where id_bascula = " + Num_Bascula + "AND id_grupo = " + Num_Grupo, Conec.CadenaConexion);
            if (OLConfig.Read()) Existe = true;
            else Existe = false;
            OLConfig.Close();

            GuardarImpresion(Existe);

            MessageBox.Show(this, Variable.SYS_MSJ[48, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);  //"Configuracion Guardada");

            activarDesactivarEdicion(false, Color.WhiteSmoke);
            ActivaDesactivaFormato(false, Color.WhiteSmoke);
            Consulta_EnBD();

            StripEditar.Enabled = true;
            StripBorrar.Enabled = false;
            StripGuardar.Enabled = false;
            StripEnviar.Enabled = true;
        }
        private void StripEditar_Click(object sender, EventArgs e)
        {           
            object[] clave = new object[2] { Num_Bascula, Num_Grupo };
            Form1.statRegistro = ESTADO.EstadoRegistro.PKPARCIAL;
            baseDeDatosDataSet.Impresor.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Impresor.id_basculaColumn, baseDeDatosDataSet.Impresor.id_grupoColumn };
            DataRow dr = baseDeDatosDataSet.Impresor.Rows.Find(clave);
            activarDesactivarEdicion(true, Color.White);
            Mostrar_Dato(dr);            
            //if (Num_Formato > 1) ActivaDesactivaFormato(true, Color.WhiteSmoke);

            StripEditar.Enabled = false;
            StripBorrar.Enabled = false;
            StripGuardar.Enabled = true;
            StripEnviar.Enabled = false;
        }
        private void StripBorrar_Click(object sender, EventArgs e)
        {
            bool existe;

            OleDbDataReader OlPrint = Conec.Obtiene_Dato("Select * From Impresor Where id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo, Conec.CadenaConexion);
            if (OlPrint.Read()) existe = true;
            else existe = false;
            OlPrint.Close();

            if (existe)
            {
                Conec.Condicion = "id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo;

                Conec.CadenaSelect = "DELETE * FROM Impresor WHERE (" + Conec.Condicion + ")";
                Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Impresor.TableName);
            }

            limpiezaTextBoxes();
            limpiezaLabelText();
            StripEditar.Enabled = false;
            StripBorrar.Enabled = false;
            StripGuardar.Enabled = true;
            StripEnviar.Enabled = false;
        }        

        private void btnAddFormato_Click(object sender, EventArgs e)
        {
            if (myFormato.Length < 10)
            {
                limpiezaLabelText();
                Listado_Campos();
                tbxAncho.Text = "56";
                tbxLargo.Text = "44";
                texto_numenca.Text = "2";
            }
            else
            {
                MessageBox.Show(this, Variable.SYS_MSJ[284, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btnDelFormato_Click(object sender, EventArgs e)
        {
            bool existe;
            Int32 nformato = Convert.ToInt32(cBxFormatoPersonalizados.SelectedValue);

            OleDbDataReader OlPrint = Conec.Obtiene_Dato("Select * From Formato Where id_formato = " + nformato, Conec.CadenaConexion);
            if (OlPrint.Read()) existe = true;
            else existe = false;
            OlPrint.Close();

            if (existe)
            {
                Conec.Condicion = "id_formato = " + nformato;

                Conec.CadenaSelect = "DELETE * FROM Formato WHERE (" + Conec.Condicion + ")";
                Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Formato.TableName);
                Llenar_Formato();
                Listado_formatos(0); 
            }
        }
        private void btnSaveFormat_Click(object sender, EventArgs e)
        {
            bool existe;

            if (texto_numenca.Text == "")
            {
                MessageBox.Show(this, Variable.SYS_MSJ[237, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.texto_numenca.Focus();
                return;
            }

            if (texto_numing.Text == "")
            {
                MessageBox.Show(this, Variable.SYS_MSJ[238, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                this.texto_numing.Focus();
                return;
            }
            if (tbxAncho.Text == "")
            {
                MessageBox.Show(this, Variable.SYS_MSJ[362, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.tbxAncho.Focus();
                return;
            }
            else if (Convert.ToInt32(tbxAncho.Text) < 1)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[409, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.tbxAncho.Focus();
                return;
            }


            if (tbxLargo.Text == "")
            {
                MessageBox.Show(this, Variable.SYS_MSJ[363, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.tbxLargo.Focus();
                return;
            }
            else if (Convert.ToInt32(tbxLargo.Text) < 1)
            {
                tbxLargo.Text = "44";
            }

            if (tbxSeparacion.Text == "")
            {
                MessageBox.Show(this, Variable.SYS_MSJ[364, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                this.tbxSeparacion.Focus();
                return;
            }
            string Contenido_etiquetas = Leer_etiqueta().Replace(".","");
           
            if (Contenido_etiquetas.Length < 0)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[365, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (Num_Formato <= 0)
            {
                Num_Formato = Convert.ToInt32(muestraAutoincrementoId("SELECT id_formato FROM Formato ORDER BY id_formato desc"));
            }

            formatoTableAdapter.Fill(baseDeDatosDataSet.Formato);
            baseDeDatosDataSet.Formato.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Formato.id_formatoColumn };

            DataRow olfor = baseDeDatosDataSet.Formato.Rows.Find(Num_Formato);

            if (olfor != null) existe = true;
            else existe = false;

            if (Num_Formato > 0 && Contenido_etiquetas.Length > 0)
            {
                GuardarFormato(Num_Formato, existe);
                MessageBox.Show(this, Variable.SYS_MSJ[48, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);  //"Configuracion Guardada");           
                Llenar_Formato();
                Listado_formatos(Num_Formato);
            }

        }        

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

            Console.WriteLine("toolStripMenuItem1_Click"); 
            if (Etiquetas[Convert.ToInt16(Frame1.Tag)].Text == "")
            {
                contextMenuStrip1.Close();
                MessageBox.Show(contextMenuStrip1, Variable.SYS_MSJ[49, Variable.idioma]);  //"No existe dato");
            }
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Etiquetas[Convert.ToInt16(Frame1.Tag)].Text = "";
            Etiquetas[Convert.ToInt16(Frame1.Tag)].Tag = "";
        }
        private void toolStripMenuItem1_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Console.WriteLine("toolStripMenuItem1_DropDownItemClicked"); 
            ToolStripItem myItem = (ToolStripItem)e.ClickedItem;
            if (Etiquetas[Convert.ToInt16(Frame1.Tag)].Text != "")
            {
                Etiquetas[Convert.ToInt16(Frame1.Tag)].Tag = myItem.MergeIndex;
            }
        }
        private void toolStripMenuItem1_DropDownOpened(object sender, EventArgs e)
        {
            Console.WriteLine("toolStripMenuItem1_DropDownOpened"); 
            ToolStripDropDownItem menu = (ToolStripDropDownItem)sender;
            
            if (Etiquetas[Convert.ToInt16(Frame1.Tag)].Text != "")
            {

                string index = Etiquetas[Convert.ToInt16(Frame1.Tag)].Tag.ToString();
                Console.WriteLine("Index of this label = "+ index); 

                if (Etiquetas[Convert.ToInt16(Frame1.Tag)].Text.Substring(0, 1) == "T")
                {
                    menu.DropDown.Items[4].Enabled = false;  // toolStripMenuItem6.Enabled = false;  
                    menu.DropDown.Items[5].Enabled = false;  // toolStripMenuItem7.Enabled = false;   
                }
                else
                {
                    menu.DropDownItems[4].Enabled = true; // toolStripMenuItem6.Enabled = true;  
                    menu.DropDownItems[5].Enabled = true; // toolStripMenuItem7.Enabled = true;  
                }
                if ((Etiquetas[Convert.ToInt16(Frame1.Tag)].Text.Substring(0, 1) != "E") && (Etiquetas[Convert.ToInt16(Frame1.Tag)].Text.Substring(0, 1) != "C"))
                {
                    toolStripMenuItem1.DropDown.Items[Convert.ToInt16(Etiquetas[Convert.ToInt16(Frame1.Tag)].Tag)].Select();
                }
            }
        }
        #endregion

        #region Envio de informacion a basculas
        private bool EnviaDatosA_Bascula(int Info_A_Enviar, string direccionIP)
        {
            bool ERROR = false;
            Cursor.Current = Cursors.WaitCursor;

            int reg_total = 0;
            try
            {
                impresorTableAdapter.Fill(baseDeDatosDataSet.Impresor);
                formatoTableAdapter.Fill(baseDeDatosDataSet.Formato);

                Cliente_bascula = Cte.conectar(direccionIP, 50036);  
                if (Cliente_bascula != null)
                {
                    reg_total = baseDeDatosDataSet.Impresor.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo).Length;
                    reg_total += baseDeDatosDataSet.Formato.Rows.Count;
                    
                    string sComando = "XX" + (char)9 + (char)10;
                    string Msj_recibido = Env.Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");

                    if (Msj_recibido != null)
                    {
                        Enviar_Impresor(direccionIP, ref Cliente_bascula);
                    }
                    
                    Cte.desconectar(ref Cliente_bascula);
                    
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    ERROR = true;
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
                ERROR = true;
            }

            return ERROR;
        }
        private bool EnviaDatosA_Bascula(int Info_A_Enviar, string puerto, Int32 BaudRate)
        {
            bool ERROR = false;
            Cursor.Current = Cursors.WaitCursor;
            int reg_total = 0;

            try
            {
                impresorTableAdapter.Fill(baseDeDatosDataSet.Impresor);
                formatoTableAdapter.Fill(baseDeDatosDataSet.Formato);

                serialPort1 = new SerialPort();

                if (SR.OpenPort(ref serialPort1, puerto, BaudRate))
                {
                    SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)9 + (char)10);
                    SR.ReceivedCOMSerial(ref serialPort1);

                    reg_total = baseDeDatosDataSet.Impresor.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo).Length;
                    reg_total += baseDeDatosDataSet.Formato.Rows.Count;

                    WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[251, Variable.idioma]);
                    Thread t = new Thread(workerObject.vShowMsg);
                    t.Start();

                    Enviar_Impresor(ref serialPort1);

                    workerObject.vEndShowMsg();

                    SR.SendCOMSerial(ref serialPort1, "DXXX" + (char)9 + (char)10);
                    SR.ReceivedCOMSerial(ref serialPort1);

                    SR.ClosePort(ref serialPort1);
                    
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    ERROR = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
                ERROR = true;
            }

            return ERROR;
        }
        private void Enviar_Impresor(string direccionIP, ref Socket Cliente_bascul)
        {
            int reg_leido = 1;
            int reg_envio = 0;
            int reg_total = 0;
            string Msj_Recibido;
            string Variable_frame = null;

            Variable_frame = "";
            DataRow[] DR_Impre = baseDeDatosDataSet.Impresor.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);
            reg_total = DR_Impre.Length;

            foreach (DataRow DP in DR_Impre)
            {
                Variable_frame = Variable_frame + Env.Genera_Trama_Config_Impresor(DP);
                reg_envio++;

                Msj_Recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo, "Gi");

                //pro.UpdateProcess(1, Variable.SYS_MSJ[251, Variable.idioma]);

                reg_leido = 1;
                Variable_frame = "";
                if (Convert.ToInt32(DP["f_personalizado_papel"].ToString()) > 0)
                    Enviar_Formato(direccionIP, ref Cliente_bascula, Convert.ToInt32(DP["f_personalizado_papel"].ToString()));
                if (Convert.ToInt32(DP["f_personalizado_etiq"].ToString()) > 0)
                    Enviar_Formato(direccionIP, ref Cliente_bascula, Convert.ToInt32(DP["f_personalizado_etiq"].ToString()));
            }
        }
        private void Enviar_Impresor(ref SerialPort Cliente_bascula)
        {
            int reg_leido = 1;
            int reg_envio = 0;
            int reg_total = 0;            
            string strcomando;
            string Variable_frame = null;
            string[] Dato_Recibido = null;

            Variable_frame = "";
            DataRow[] DR_Impre = baseDeDatosDataSet.Impresor.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);
            reg_total = DR_Impre.Length;

            foreach (DataRow DP in DR_Impre)
            {
                Variable_frame = Variable_frame + Env.Genera_Trama_Config_Impresor(DP);
                reg_envio++;
                strcomando = "Gi" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;

                SR.SendCOMSerial(ref Cliente_bascula, strcomando, ref Dato_Recibido);
                if (Dato_Recibido[0].IndexOf("Error") >= 0)
                {
                    Env.Guardar_Trama_pendiente(Num_Bascula, Num_Grupo, strcomando);
                }
                if (Dato_Recibido[0].IndexOf("Ok") >= 0)
                {
                    if (Convert.ToInt32(DP["f_personalizado_papel"].ToString()) > 0)
                        Enviar_Formato(ref Cliente_bascula, Convert.ToInt32(DP["f_personalizado_papel"].ToString()));
                    if (Convert.ToInt32(DP["f_personalizado_etiq"].ToString()) > 0)
                        Enviar_Formato(ref Cliente_bascula, Convert.ToInt32(DP["f_personalizado_etiq"].ToString()));
                }

                //pro.UpdateProcess(1, Variable.SYS_MSJ[251, Variable.idioma]);
                reg_leido = 1;
                Variable_frame = "";
            }
        }
        private void Enviar_Formato(string direccionIP, ref Socket Cliente_bascula, int nformato)
        {
            int reg_leido = 1;
            int reg_envio = 0;
            string Msj_Recibido;
            string Variable_frame;

            Variable_frame = "";
       
            DataRow DP = baseDeDatosDataSet.Formato.Rows.Find(nformato);

            if (DP != null)
            {
                Variable_frame = Variable_frame + Env.Genera_Trama_Config_Formato(DP);
                reg_envio++;
                Msj_Recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo, "GF");

                reg_leido = 1;
                Variable_frame = "";
            }
           
        }
        private void Enviar_Formato(ref SerialPort Cliente_bascula, int nformato)
        {
            int reg_leido = 1;
            int reg_envio = 0;
            string Variable_frame;
            string strcomando;
            string[] Dato_Recibido = null;

            Variable_frame = "";
            //DataRow[] DR_Format = baseDeDatosDataSet.Formato.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);
            // reg_total = baseDeDatosDataSet.Formato.Rows.Count;
            DataRow DP = baseDeDatosDataSet.Formato.Rows.Find(nformato);
            // foreach (DataRow DP in baseDeDatosDataSet.Formato.Rows)
            //{
            Variable_frame = Variable_frame + Env.Genera_Trama_Config_Formato(DP);
            reg_envio++;
            strcomando = "GF" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;

            SR.SendCOMSerial(ref Cliente_bascula, strcomando, ref Dato_Recibido);
            if (Dato_Recibido[0].IndexOf("Error") >= 0)
            {
                Env.Guardar_Trama_pendiente(Num_Bascula, Num_Grupo, strcomando);
            }
            // pro.UpdateProcess(1, reg_envio.ToString() + "/" + reg_total.ToString());
            reg_leido = 1;
            Variable_frame = "";
            //  }
        }
        #endregion
        
        #region funciones para los campos
        private void Llenar_etiqueta(string posdef, string possize)
        {
            int i = 0;

            if (posdef != null && possize != null)
            {
                for (int c = 0; c < 20; c++)
                {
                    for (int s = 0; s < 3; s++)
                    {
                        Etiquetas[i].Text = campo(posdef.Substring(i, 1).ToString());

                        if (possize.Substring(i, 1).ToString() != ".") { Etiquetas[i].Tag = possize.Substring(i, 1).ToString(); }
                        else { Etiquetas[i].Tag = "."; }

                        i = i + 1;
                    }
                }
            }
        }
        private bool buscar_campo(string aa)
        {
            bool exist = false;
            int i = 0;
            for (int c = 0; c < 20; c++)
            {
                for (int s = 0; s < 3; s++)
                {
                    if (campo(aa) != "" && aa != "!" && Etiquetas[i].Text != "")
                    {
                        if (Etiquetas[i].Text.Substring(0, 1) == aa)
                        {
                            exist = true;
                            break;
                        }
                    }
                    i = i + 1;
                }
            }
            return exist;
        }
        private string campo(string car)
        {
            string cad_result = "";
            const char nulo = '"';
            string letra = nulo.ToString();
            if (letra == car) { cad_result = list_campo[0, 1]; }

            for (int i = 0; i < list_campo.GetLength(0); i++)
            {
                if (list_campo[i, 0] == car)
                {
                    cad_result = list_campo[i, 1];
                    break;
                }
            }
            return cad_result;
        }
        private int indice(string Name)
        {
            for (int i = 0; i < this.Etiquetas.Length; i++)
            {
                if (this.Etiquetas[i].Name == Name) { return i; }
            }
            return -1;
        }
        private void asignarEventos()
        {
            foreach (Label Lab in Etiquetas)
            {
                Lab.MouseDown += new MouseEventHandler(this.Etiquetas_MouseDown);
                Lab.DragDrop += new DragEventHandler(this.Etiquetas_DragDrop);
                Lab.DragEnter += new DragEventHandler(this.Etiquetas_DragEnter);
                Lab.DragOver += new DragEventHandler(this.Etiquetas_DragOver);
                Lab.AllowDrop = true;
            }         
        }
        private void colocar_label(Label etique, int op, int Index, int Index2)
        {
            string cc = "";
            char[] Ncomparte = new char[] { '"', 'C', 'm', 'E', 'I', 'N', '!', 's', 'b' }; // "","E","C","I","N","m","!","s"
            int a = Index + 1;
            int b = 0;
            cc = etique.Text.Substring(0, 1);

            Console.WriteLine("colocar_label");

            if (cc == "" || cc == "C" || cc == "m" || cc == "E" || cc == "I" || cc == "N" || cc == "!" || cc == "s" || cc == "b")
            {
                if ((a % 3) == 0)
                {
                    a = a - 3;
                    b = 2;
                }
                else
                {
                    if ((a % 3) == 2)
                    {
                        a = a - 2;
                        b = 1;
                    }
                    else { a = a - 1; }
                }

                if (Etiquetas[a].Text.Length > 0)
                    if (cc == Etiquetas[a].Text.Substring(0, 1)) return;

                Etiquetas[a].Text = "";
                Etiquetas[a].Tag = "";
                Etiquetas[a + 1].Text = "";
                Etiquetas[a + 1].Tag = "";
                Etiquetas[a + 2].Text = "";
                Etiquetas[a + 2].Tag = "";
                a = Index + 1;

                if (cc == "E" || cc == "I" || cc == "N" || cc == "!" || cc == "s" || cc == "b") { a = a - b; }
            }
            else
            {
                if ((a % 3) == 0)
                {
                    if ((Etiquetas[a - 3].Text == "") || (Etiquetas[a - 3].Text.Substring(0, 1) == "E") || (Etiquetas[a - 3].Text.Substring(0, 1) == "C") || (Etiquetas[a - 3].Text.Substring(0, 1) == "I") || (Etiquetas[a - 3].Text.Substring(0, 1) == "N") || (Etiquetas[a - 3].Text.Substring(0, 1) == "m") || (Etiquetas[a - 3].Text.Substring(0, 1) == "!") || (Etiquetas[a - 3].Text.Substring(0, 1) == "s"))
                    {
                        Etiquetas[a - 3].Text = "";
                        Etiquetas[a - 3].Tag = "";
                    }
                }
                else
                {
                    if ((a % 3) == 2)
                    {
                        if ((Etiquetas[a - 2].Text == "") || (Etiquetas[a - 2].Text.Substring(0, 1) == "E") || (Etiquetas[a - 2].Text.Substring(0, 1) == "C") || (Etiquetas[a - 2].Text.Substring(0, 1) == "I") || (Etiquetas[a - 2].Text.Substring(0, 1) == "N") || (Etiquetas[a - 2].Text.Substring(0, 1) == "m") || (Etiquetas[a - 2].Text.Substring(0, 1) == "!") || (Etiquetas[a - 2].Text.Substring(0, 1) == "s"))
                        {
                            Etiquetas[a - 2].Text = "";
                            Etiquetas[a - 2].Tag = "";
                        }
                    }
                    else
                    {
                        if ((Etiquetas[a - 1].Text == "") || (Etiquetas[a - 1].Text.Substring(0, 1) == "E") || (Etiquetas[a - 1].Text.Substring(0, 1) == "C") || (Etiquetas[a - 1].Text.Substring(0, 1) == "I") || (Etiquetas[a - 1].Text.Substring(0, 1) == "N") || (Etiquetas[a - 1].Text.Substring(0, 1) == "m") || (Etiquetas[a - 1].Text.Substring(0, 1) == "!") || (Etiquetas[a - 1].Text.Substring(0, 1) == "s"))
                        {
                            Etiquetas[a - 1].Text = "";
                            Etiquetas[a - 1].Tag = "";
                        }
                    }
                }
            }
            if (op == 1)
            {
                Etiquetas[a - 1].Text = etique.Text;
                Etiquetas[a - 1].Tag = "1";
                Etiquetas[a - 1].BorderStyle = System.Windows.Forms.BorderStyle.None;
                Etiquetas[Index2].Text = "";
                Etiquetas[Index2].Tag = "";
            }
            else
            {
                Etiquetas[a - 1].Text = etique.Text;
                Etiquetas[a - 1].Tag = "1";
                Etiquetas[a - 1].BorderStyle = System.Windows.Forms.BorderStyle.None;
            }
        }
        private string Leer_etiqueta()        
        {
            string cadena = "";
            int i = 0;

            for (int c = 0; c < 20; c++)
            {
                for (int s = 0; s < 3; s++)
                {
                    if (Etiquetas[i].Text.Length > 0)
                        cadena = cadena + Etiquetas[i].Text.Substring(0, 1);
                    else
                        cadena = cadena + ".";
                    i = i + 1;
                }
            }
            return cadena;
        }
        private string Leer_posicion()
        {
            string cadena = "";
            int i = 0;

            for (int c = 0; c < 20; c++)
            {
                for (int s = 0; s < 3; s++)
                {
                    if (Etiquetas[i].Tag.ToString().Length > 0)
                        cadena = cadena + Etiquetas[i].Tag.ToString();
                    else
                        cadena = cadena + "0";
                    i = i + 1;
                }
            }
            return cadena;
        }
        #endregion

        #region Eventos de los controles

        #region procedimiento para la manipulacion de etiquetas
        private void Etiquetas_DragOver(object sender, DragEventArgs e)
        {
            Label Source = ((Label)sender);

            for (int i = 0; i < this.Etiquetas.Length; i++)
            {
                if (Source.Name != this.Etiquetas[i].Name) this.Etiquetas[i].BorderStyle = BorderStyle.None;
            }
            Source.BorderStyle = BorderStyle.FixedSingle;
        }
        private void Etiquetas_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.Label")) { e.Effect = DragDropEffects.Copy; }
        }
        private void Etiquetas_DragDrop(object sender, DragEventArgs e)
        {
            Label Source = ((Label)sender);
            Label etique = ((Label)e.Data.GetData("System.Windows.Forms.Label"));

            int Index = indice(Source.Name);

            if (etique.Tag.ToString().Substring(0, 1) == "L")
            {
                string cc = etique.Text.Substring(0, 1);
                if (!buscar_campo(cc))
                { colocar_label(etique, 0, Index, 0); }
                else
                {
                    Source.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    MessageBox.Show(this, Variable.SYS_MSJ[50, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation); //"El campo ya esta asignado");
                }
            }
            else
            {
                int Index2 = Convert.ToInt16(etique.Tag);
                if (Index != Index2)
                {
                    if (etique.Text != "") colocar_label(etique, 1, Index, Index2);
                }
            }
        }
        private void Etiquetas_MouseDown(object sender, MouseEventArgs e)
        {
            Label I = ((Label)sender);
            I.Select();
            int Index = 0;
            for (int i = 0; i <= Etiquetas.Length - 1; i++)
            {
                if (Etiquetas[i].Name == I.Name)
                {
                    Index = i;
                    break;
                }
            }

            if ((e.Button == MouseButtons.Right) && (formato_locked == false))
            {
                this.Frame1.Tag = Index;
                panel3.ContextMenuStrip = contextMenuStrip1;
                contextMenuStrip1.Top = Etiquetas[Index].Top;
                contextMenuStrip1.Left = Etiquetas[Index].Left;
                contextMenuStrip1.Show();
            }
            else
            {
                if (I.Text != "")
                {
                    Point pt = new Point(e.X, e.Y);
                    if (e.Button == MouseButtons.Left && (formato_locked == false))
                    {
                        pasar.Left = e.X + I.Left;
                        pasar.Top = e.Y + I.Top;
                        pasar.Text = I.Text;
                        pasar.Tag = Index;
                        pasar.DoDragDrop(pasar, DragDropEffects.Copy);
                    }
                }
            }
        }
        private void List4_MouseDown(object sender, MouseEventArgs e)
        {
            ListBox campo = ((ListBox)sender);

            int index = campo.SelectedIndex;
            Point pt = new Point(e.X, e.Y);
            if (e.Button == System.Windows.Forms.MouseButtons.Left && (formato_locked == false))
            {
                pasar.Left = e.X + campo.Left;
                pasar.Top = e.Y + campo.Top;
                pasar.Text = campo.Items[index].ToString();
                pasar.Tag = "L";
                pasar.DoDragDrop(pasar, DragDropEffects.Copy);
            }
        }
        #endregion

        #region Eventos de radiobuttom y checkbox
        private void rBxEanPrd_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxEanPrd.Checked)
            {
                Variable.user_EAN_UPCxProd = 0;
                cBxCodigoPrd.Enabled = true;
                cBxCodigoPrd.Items.Clear();
                cBxCodigoPrd.Text = "";
                cBxCodigoPrd.Items.Add(Variable.SYS_MSJ[136,Variable.idioma]); //"EAN13(pccccccwwwwwv) Peso");
                cBxCodigoPrd.Items.Add("EAN13(pcccccctttttv) Total");
                cBxCodigoPrd.Items.Add(Variable.SYS_MSJ[142,Variable.idioma]);  //"Personalizado");
                cBxCodigoPrd.SelectedIndex = 0;
                texto_codigoxprod.MaxLength = 12;
                //this.cBxCodigoPrd.Focus();
            }
        }
        private void rBxUpcPrd_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxUpcPrd.Checked)
            {
                Variable.user_EAN_UPCxProd = 1;
                cBxCodigoPrd.Enabled = true;
                cBxCodigoPrd.Items.Clear();
                cBxCodigoPrd.Text = "";
                cBxCodigoPrd.Items.Add(Variable.SYS_MSJ[137,Variable.idioma]); //"UPC12(xpcccccwwwwwv) Peso");
                cBxCodigoPrd.Items.Add("UPC12(xpccccctttttv) Total");
                cBxCodigoPrd.Items.Add(Variable.SYS_MSJ[142,Variable.idioma]);  //"Personalizado");
                cBxCodigoPrd.SelectedIndex = 0;
                texto_codigoxprod.MaxLength = 11;
                //this.cBxCodigoPrd.Focus();
            }
        }
        private void rBxNoCodePrd_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxNoCodePrd.Checked)
            {
                Variable.user_EAN_UPCxProd = 2;
                cBxCodigoPrd.Enabled = false;
                cBxCodigoPrd.Items.Clear();
                cBxCodigoPrd.Text = "";
                texto_codigoxprod.Clear();
            }
        }

        private void rBxEanTicket_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxEanTicket.Checked)
            {
                Variable.user_EAN_UPCxTicket = 0;
                cBxCodigoTicket.Enabled = true;
                cBxCodigoTicket.Items.Clear();
                cBxCodigoTicket.Text = "";
                cBxCodigoTicket.Items.Add(Variable.SYS_MSJ[138,Variable.idioma]);  //"EAN13(pxaannttttttv) Vendedor");
                cBxCodigoTicket.Items.Add(Variable.SYS_MSJ[139,Variable.idioma]);  //"EAN13(pxddnnttttttv) Depto.");
                cBxCodigoTicket.Items.Add(Variable.SYS_MSJ[142,Variable.idioma]);  //"Personalizado");
                cBxCodigoTicket.SelectedIndex = 0;
                texto_codigoxticket.MaxLength = 12;
                //this.cBxCodigoTicket.Focus();
            }
        }
        private void rBxUpcTicket_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxUpcTicket.Checked)
            {
                Variable.user_EAN_UPCxTicket = 1;
                cBxCodigoTicket.Enabled = true;
                cBxCodigoTicket.Items.Clear();
                cBxCodigoTicket.Text = "";
                cBxCodigoTicket.Items.Add(Variable.SYS_MSJ[140,Variable.idioma]);  //"UPC12 (xpaannttttttv) Vendedor");
                cBxCodigoTicket.Items.Add(Variable.SYS_MSJ[141,Variable.idioma]);  //"UPC12 (xpddnnttttttv) Depto.");
                cBxCodigoTicket.Items.Add(Variable.SYS_MSJ[142,Variable.idioma]);  //"Personalizado");
                cBxCodigoTicket.SelectedIndex = 0;
                texto_codigoxticket.MaxLength = 11;
                //this.cBxCodigoTicket.Focus();
            }
        }

        private void rBxNoCodeTicket_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxNoCodeTicket.Checked)
            {
                Variable.user_EAN_UPCxTicket = 2;
                cBxCodigoTicket.Enabled = false;
                cBxCodigoTicket.Items.Clear();
                cBxCodigoTicket.Text = "";
                texto_codigoxticket.Clear();
            }
        }

        private void chkNutriente_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNutriente.Checked) Variable.user_nutri = 1;
            else Variable.user_nutri = 0;
            this.ScrollBarContraEtiqueta.Focus();
        }
        private void tipo_papel_CheckedChanged(object sender, EventArgs e)
        {
            rBxformato1_Standar.Enabled = true;
            rBxformato2_Standar.Enabled = true;
            rBxformato3_Standar.Enabled = true;
            rBxformato_Personalizado.Enabled = true;

            if (rBxTipopapel.Checked)
            {
                Variable.user_formato.medio_imp = 0;
                if (!cargandoForma)
                {
                    formatoTableAdapter.Fill(baseDeDatosDataSet.Formato);
                    impresorTableAdapter.Fill(baseDeDatosDataSet.Impresor);

                    DataRow[] DA = baseDeDatosDataSet.Impresor.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);
                    if (DA.Length > 0)
                    {
                        DataRow dr = DA[0];
                        Mostrar_Formato(dr, true);
                    }
                }
            }
        }
        private void tipo_etiqueta_CheckedChanged(object sender, EventArgs e)
        {
            rBxformato1_Standar.Enabled = true;
            rBxformato2_Standar.Enabled = true;
            rBxformato3_Standar.Enabled = true;
            rBxformato_Personalizado.Enabled = true;

            if (rBxTipoetiqueta.Checked)
            {
                Variable.user_formato.medio_imp = 1;
                if (!cargandoForma)
                {
                    formatoTableAdapter.Fill(baseDeDatosDataSet.Formato);
                    impresorTableAdapter.Fill(baseDeDatosDataSet.Impresor);

                    DataRow[] DA = baseDeDatosDataSet.Impresor.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);
                    if (DA.Length > 0)
                    {
                        DataRow dr = DA[0];
                        Mostrar_Formato(dr, false);
                    }
                }
            }
        }
        private void formato1_Standar_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxTipopapel.Checked && rBxformato1_Standar.Checked)
            {
                Variable.user_formato.for_papel_tipoimpre = 0;
                //'formato default 1 ticket
                Variable.user_formato1_posdef = "...E..............wptN..WPTs...C..m.........................";
                Variable.user_formato1_possize = "...1..............2223..3365...8..2.........................";
                tbxAncho.Text = "56";
                tbxLargo.Text = "44";
                tbxSeparacion.Text = "0";
                texto_numenca.Text = "2";
                myImpresion[0].medio_imp = 0;
            }
            else if (rBxTipoetiqueta.Checked && rBxformato1_Standar.Checked)
            {
                Variable.user_formato.for_ecsep_tipoimpre = 0;
                //'formato default 1 etiqueta c/separador
                Variable.user_formato1_posdef = "...E..N..I..f.gF.GwptWPT....C..m............................";
                Variable.user_formato1_possize = "...1..4..1..1.12.2111336....8..2............................";
                tbxAncho.Text = "56";
                tbxLargo.Text = "44";
                tbxSeparacion.Text = "3";
                texto_numenca.Text = "2";
                myImpresion[0].medio_imp = 1;
            }
            
            if (rBxTipoetiqueta.Checked || rBxTipopapel.Checked)
            {
                myImpresion[0].ncodigobar_xprod = cBxCodigoPrd.SelectedIndex;
                myImpresion[0].ncodigobar_xticket = cBxCodigoTicket.SelectedIndex;
                myImpresion[0].for_papel_tipoimpre = Variable.user_formato.for_papel_tipoimpre;
                myImpresion[0].for_ecsep_tipoimpre = Variable.user_formato.for_ecsep_tipoimpre;

                ActivaDesactivaFormato(false, Color.WhiteSmoke);

                cBxFormatoPersonalizados.Enabled = false;
                formato_locked = true;
                Llenar_etiqueta(Variable.user_formato1_posdef, Variable.user_formato1_possize);
                this.panel3.Focus();
            }
            else
            {
                rBxTipopapel.Focus();
            }
        }
        private void formato2_Standar_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxTipopapel.Checked && rBxformato2_Standar.Checked)
            {
                Variable.user_formato.for_papel_tipoimpre = 1;
                //'formato default 2 ticket
                Variable.user_formato2_posdef = "...FHcrV.E........wptN..WPTs...C..m.........................";
                Variable.user_formato2_possize = "...22223.1........2223..3365...8..2.........................";
                tbxAncho.Text = "56";
                tbxLargo.Text = "44";
                tbxSeparacion.Text = "0";
                texto_numenca.Text = "2";
                myImpresion[1].medio_imp = 0;
            }
            else if (rBxTipoetiqueta.Checked && rBxformato2_Standar.Checked)
            {
                Variable.user_formato.for_ecsep_tipoimpre = 1;
                //'formato default 2 etiqueta c/separador
                Variable.user_formato2_posdef = "...C..!..N..I..f.gFHG...wptWPT.m............................";
                Variable.user_formato2_possize = "...8..1..4..1..1.1333...111336.2............................";
                tbxAncho.Text = "56";
                tbxLargo.Text = "44";
                tbxSeparacion.Text = "3";
                texto_numenca.Text = "2";
                myImpresion[1].medio_imp = 1;
            }
                        
            if (rBxTipoetiqueta.Checked || rBxTipopapel.Checked)
            {
                myImpresion[1].ncodigobar_xprod = cBxCodigoPrd.SelectedIndex;
                myImpresion[1].ncodigobar_xticket = cBxCodigoTicket.SelectedIndex;
                myImpresion[1].for_papel_tipoimpre = Variable.user_formato.for_papel_tipoimpre;
                myImpresion[1].for_ecsep_tipoimpre = Variable.user_formato.for_ecsep_tipoimpre;
                ActivaDesactivaFormato(false, Color.WhiteSmoke);

                cBxFormatoPersonalizados.Enabled = false;
                formato_locked = true;
                Llenar_etiqueta(Variable.user_formato2_posdef, Variable.user_formato2_possize);
                this.panel3.Focus();
            }
            else rBxTipopapel.Focus();
        }
        private void rBxformato3_Standar_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxTipopapel.Checked && rBxformato3_Standar.Checked)
            {
                Variable.user_formato.for_papel_tipoimpre = 3;
                //'formato default 3 ticket
                Variable.user_formato4_posdef = "b..FHcrV.E..I..wptN..WPTs..C..m.............................";
                Variable.user_formato4_possize = "10022223.100100111300336500100200.2.........................";
                tbxAncho.Text = "56";
                tbxLargo.Text = "44";
                tbxSeparacion.Text = "0";
                texto_numenca.Text = "2";
                myImpresion[1].medio_imp = 0;
            }
            else if (rBxTipoetiqueta.Checked && rBxformato3_Standar.Checked)
            {
                Variable.user_formato.for_ecsep_tipoimpre = 3;
                //'formato default 3 etiqueta c/separador
                Variable.user_formato4_posdef = "b........N.....wptWPT......C................................";
                Variable.user_formato4_possize = "1000000003000001113360..006600.8..2.........................";
                tbxAncho.Text = "56";
                tbxLargo.Text = "44";
                tbxSeparacion.Text = "3";
                texto_numenca.Text = "2";
                myImpresion[1].medio_imp = 1;
            }

            if (rBxTipoetiqueta.Checked || rBxTipopapel.Checked)
            {
                myImpresion[1].ncodigobar_xprod = cBxCodigoPrd.SelectedIndex;
                myImpresion[1].ncodigobar_xticket = cBxCodigoTicket.SelectedIndex;
                myImpresion[1].for_papel_tipoimpre = Variable.user_formato.for_papel_tipoimpre;
                myImpresion[1].for_ecsep_tipoimpre = Variable.user_formato.for_ecsep_tipoimpre;
                ActivaDesactivaFormato(false, Color.WhiteSmoke);

                cBxFormatoPersonalizados.Enabled = false;
                formato_locked = true;
                Llenar_etiqueta(Variable.user_formato4_posdef, Variable.user_formato4_possize);
                this.panel3.Focus();
            }
            else rBxTipopapel.Focus();
        }
        private void formato_Personalizado_CheckedChanged(object sender, EventArgs e)
        {
            if (Form1.statRegistro == ESTADO.EstadoRegistro.PKNOTRATADO)
            {
                if (rBxTipopapel.Checked && rBxformato_Personalizado.Checked)
                {
                    Variable.user_formato.for_papel_tipoimpre = 2;                   
                    //papel ticket
                    Variable.user_formato3_posdef = "";  //...E..............wptN..WPT.s..C..F.........................";         //pos_def
                    Variable.user_formato3_possize = ""; //...1..............2223..336.5..8..2.........................";                    
                    Variable.user_formatosize.nformato = 0;
                    myImpresion[2].medio_imp = 0;
                }
                if (rBxTipoetiqueta.Checked && rBxformato_Personalizado.Checked)
                {
                    Variable.user_formato.for_ecsep_tipoimpre = 2;
                    //'etiqueta con separador       
                    Variable.user_formato3_posdef = "";// ..E..N..I..f.gF.GwptWPT....C...............................";         //pos_def
                    Variable.user_formato3_possize = ""; //...1..4..1..1.12.2111336....8...............................";                    
                    Variable.user_formatosize.nformato = 0;
                    myImpresion[2].medio_imp = 1;
                }
                if (rBxTipoetiqueta.Checked || rBxTipopapel.Checked)
                {
                    myImpresion[2].ncodigobar_xprod = cBxCodigoPrd.SelectedIndex;
                    myImpresion[2].ncodigobar_xticket = cBxCodigoTicket.SelectedIndex;
                    myImpresion[2].for_papel_tipoimpre = Variable.user_formato.for_papel_tipoimpre;
                    myImpresion[2].for_ecsep_tipoimpre = Variable.user_formato.for_ecsep_tipoimpre;

                    ActivaDesactivaFormato(true, Color.White);
                    limpiezaLabelText();
                    if (rBxTipopapel.Checked && rBxformato_Personalizado.Checked)
                    {
                        tbxAncho.Text = "56";
                        tbxLargo.Text = "44";
                        tbxSeparacion.Text = "2";
                        tbxLargo.Enabled = false;
                        tbxSeparacion.Enabled = false;
                    }
                    else
                    {
                        tbxAncho.Text = "56";
                        tbxLargo.Text = "44";
                        tbxSeparacion.Text = "2";
                    }
                    formato_locked = false;
                    cBxFormatoPersonalizados.Enabled = true;
                    cBxFormatoPersonalizados.Focus();
                    Llenar_Formato();
                    ActivaDesactivaFormato(false, Color.WhiteSmoke);
                    btnAddFormato.Enabled = true;
                }
                else rBxTipopapel.Focus();
            }
            if (Form1.statRegistro == ESTADO.EstadoRegistro.PKPARCIAL)            
            {
                if (rBxTipopapel.Checked && rBxformato_Personalizado.Checked)
                {
                    Variable.user_formato.for_papel_tipoimpre = 2;                  
                    //Variable.user_formatosize.nformato = 0;
                    myImpresion[2].medio_imp = 0;
                }
                if (rBxTipoetiqueta.Checked && rBxformato_Personalizado.Checked)
                {
                    Variable.user_formato.for_ecsep_tipoimpre = 2;
                   // Variable.user_formatosize.nformato = 0;
                    myImpresion[2].medio_imp = 1;
                }
                if (rBxTipoetiqueta.Checked || rBxTipopapel.Checked)
                {
                    myImpresion[2].ncodigobar_xprod = cBxCodigoPrd.SelectedIndex;
                    myImpresion[2].ncodigobar_xticket = cBxCodigoTicket.SelectedIndex;
                    myImpresion[2].for_papel_tipoimpre = Variable.user_formato.for_papel_tipoimpre;
                    myImpresion[2].for_ecsep_tipoimpre = Variable.user_formato.for_ecsep_tipoimpre;
                   
                    formato_locked = false;

                    if (rBxformato_Personalizado.Checked == true)
                    {
                        cBxFormatoPersonalizados.Enabled = true;
                        cBxFormatoPersonalizados.Focus();
                        Llenar_Formato();

                        int posicion = buscar_formato(Convert.ToInt32(cBxFormatoPersonalizados.SelectedIndex));
                        if (posicion >= 0)
                        {
                            Variable.user_formato3_posdef = myFormato[posicion].posdef;
                            Variable.user_formato3_possize = myFormato[posicion].possize;
                            tbxAncho.Text = myFormato[posicion].ancho_medio.ToString();
                            tbxLargo.Text = myFormato[posicion].largo_medio.ToString();
                            tbxSeparacion.Text = myFormato[posicion].separacion_medio.ToString();
                            Llenar_etiqueta(Variable.user_formato3_posdef, Variable.user_formato3_possize);
                            Num_Formato = Variable.user_formatosize.nformato;
                            //ActivaDesactivaFormato(true, Color.White);
                            if (rBxTipopapel.Checked && rBxformato_Personalizado.Checked)
                            {
                                tbxLargo.Enabled = false;
                                tbxSeparacion.Enabled = false;
                            }
                        }
                        else
                        {
                            limpiezaLabelText();
                            ActivaDesactivaFormato(false, Color.WhiteSmoke);
                            btnAddFormato.Enabled = true;

                            if (rBxTipopapel.Checked && rBxformato_Personalizado.Checked)
                            {
                                tbxAncho.Text = "56";
                                tbxLargo.Text = "44";
                                tbxSeparacion.Text = "2";
                                tbxLargo.Enabled = false;
                                tbxSeparacion.Enabled = false;
                            }
                            else
                            {
                                tbxAncho.Text = "56";
                                tbxLargo.Text = "44";
                                tbxSeparacion.Text = "2";
                            }
                        }
                    }
                   
                }
                else rBxTipopapel.Focus();
            }
            
        }        
        #endregion

        #region Eventos de los combo_box
        private void cBxCodigoPrd_SelectedIndexChanged(object sender, EventArgs e)
        {
            Variable.user_formato.ncodigobar_xprod = cBxCodigoPrd.SelectedIndex;
            if (cBxCodigoPrd.SelectedIndex == 2)
            {
                if (rBxEanPrd.Checked == true)
                {
                    this.texto_codigoxprod.Text = "pwwwwwtttttt";  //codigo de barra para etiqueta personalizado ean13
                    this.texto_codigoxprod.MaxLength = 12;
                }
                else if (rBxUpcPrd.Checked == true)
                {
                    this.texto_codigoxprod.Text = "pwwwwtttttt";  //codigo de barra para etiqueta personalizado upc12
                    this.texto_codigoxprod.MaxLength = 11;
                }
                this.texto_codigoxprod.BackColor = Color.White;
                this.texto_codigoxprod.Enabled = true;
                this.texto_codigoxprod.Focus();
            }else{
                this.texto_codigoxprod.BackColor = Color.WhiteSmoke;                
                this.texto_codigoxprod.Text = "";
                this.texto_codigoxprod.Enabled = false;
                //this.rBxEanTicket.Focus();
            }
        }       
        private void cBxCodigoTicket_SelectedIndexChanged(object sender, EventArgs e)
        {
            Variable.user_formato.ncodigobar_xticket = cBxCodigoTicket.SelectedIndex;
            if (cBxCodigoTicket.SelectedIndex == 2)
            {
                if (rBxEanTicket.Checked == true)
                {
                    this.texto_codigoxticket.Text = "paannttttttt";  //codigo de barra para papel personalizado
                    this.texto_codigoxticket.MaxLength = 12;
                }
                else if (rBxUpcTicket.Checked == true)
                {
                    this.texto_codigoxticket.Text = "paanntttttt";  //codigo de barra para papel personalizado
                    this.texto_codigoxticket.MaxLength = 11;
                }
                this.texto_codigoxticket.BackColor = Color.White;
                this.texto_codigoxticket.Enabled = true;
                this.texto_codigoxticket.Focus();
            }
            else
            {
                this.texto_codigoxticket.BackColor = Color.WhiteSmoke;
                this.texto_codigoxticket.Text = "";
                this.texto_codigoxticket.Enabled = false;                
                this.texto_prefijo.Focus();
            }
        }
        private void cBxFormatoPersonalizados_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Form1.statRegistro != ESTADO.EstadoRegistro.PKTRATADO)
            {
                if (cBxFormatoPersonalizados.SelectedIndex > 0)
                {
                    ActivaDesactivaFormato(true, Color.White);                   
                }
                else
                {
                    limpiezaLabelText();
                    ActivaDesactivaFormato(false, Color.WhiteSmoke);
                    this.btnAddFormato.Enabled = true;
                }
            }
        }
        private void cBxFormatoPersonalizados_DataSourceChanged(object sender, EventArgs e)
        {
            cBxFormatoPersonalizados.ValueMember = "ShortName";
            cBxFormatoPersonalizados.DisplayMember = "LongName";
        }
        private void cBxFormatoPersonalizados_SelectedValueChanged(object sender, EventArgs e)
        {
            int posicion = -1;

            if (Form1.statRegistro != ESTADO.EstadoRegistro.PKTRATADO)
            {
                if (cBxFormatoPersonalizados.SelectedValue  != null && (int)cBxFormatoPersonalizados.SelectedValue > 0)
                {
                    if (rBxTipopapel.Checked && rBxformato_Personalizado.Checked)
                    {
                        Variable.user_Nformato_ticket = Convert.ToInt32(cBxFormatoPersonalizados.SelectedValue);
                        Variable.user_formatosize.nformato = Variable.user_Nformato_ticket;
                        posicion = buscar_formato(Variable.user_Nformato_ticket);
                        tbxLargo.Enabled = false;
                        tbxSeparacion.Enabled = false;
                    }
                    if (rBxTipoetiqueta.Checked && rBxformato_Personalizado.Checked)
                    {
                        Variable.user_Nformato_producto = Convert.ToInt32(cBxFormatoPersonalizados.SelectedValue);
                        Variable.user_formatosize.nformato = Variable.user_Nformato_producto;
                        posicion = buscar_formato(Variable.user_Nformato_producto);
                        tbxLargo.Enabled = true;
                        tbxSeparacion.Enabled = true;
                    }
                    if (posicion > -1)
                    {
                        Variable.user_formato3_posdef = myFormato[posicion].posdef;
                        Variable.user_formato3_possize = myFormato[posicion].possize;
                        tbxAncho.Text = myFormato[posicion].ancho_medio.ToString();
                        tbxLargo.Text = myFormato[posicion].largo_medio.ToString();
                        tbxSeparacion.Text = myFormato[posicion].separacion_medio.ToString();
                        texto_numenca.Text = myFormato[posicion].nencabezado.ToString();
                        texto_numing.Text = myFormato[posicion].ningrediente.ToString();
                        Llenar_etiqueta(Variable.user_formato3_posdef, Variable.user_formato3_possize);
                        Num_Formato = Variable.user_formatosize.nformato;
                    }
                }
            }
        }
        #endregion

        #region Eventos de text_box
        private void texto_codigoxticket_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.texto_codigoxticket.Text = "";
            if (e.KeyCode == Keys.Enter) this.texto_prefijo.Focus();

        }
        private void texto_codigoxticket_Enter(object sender, EventArgs e)
        {
            colorbefore = texto_codigoxticket.BackColor;
            texto_codigoxticket.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void texto_codigoxticket_Leave(object sender, EventArgs e)
        {
            texto_codigoxticket.BackColor = colorbefore;
        }
        private void texto_codigoxticket_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
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

            /*            if (e.KeyChar == 'a' || e.KeyChar == 'p' || e.KeyChar == 'n' || e.KeyChar == 'x' || e.KeyChar == 't' || e.KeyChar == 'd')
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
             */
        }        

        private void texto_codigoxprod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.texto_codigoxprod.Text = "";
            if (e.KeyCode == Keys.Enter) this.rBxEanTicket.Focus();

        }        
        private void texto_codigoxprod_Enter(object sender, EventArgs e)
        {
            colorbefore = texto_codigoxprod.BackColor;
            texto_codigoxprod.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void texto_codigoxprod_Leave(object sender, EventArgs e)
        {
            texto_codigoxprod.BackColor = colorbefore;
        }
        private void texto_codigoxprod_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
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

            /*if (e.KeyChar == 'a' || e.KeyChar == 'p' || e.KeyChar == 'c' || e.KeyChar == 'w' || e.KeyChar == 't' || e.KeyChar == 'd' || e.KeyChar == 'y' || e.KeyChar == 'r')
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
            }*/
        }       

        private bool Validando_codigobar(string codigoBar_Personal, int format) //format 0: EAN13, 1: UPC12
        {
            if (codigoBar_Personal != "")
            {
                if (format == 0)
                {
                    if (codigoBar_Personal.Length < 12)
                    {
                        MessageBox.Show(this, Variable.SYS_MSJ[52, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //"Longitud no es válida.");                   
                        return false;
                    }
                }
                else if (format == 1)
                {
                    if (codigoBar_Personal.Length < 11)
                    {
                        MessageBox.Show(this, Variable.SYS_MSJ[52, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //"Longitud no es válida.");                   
                        return false;
                    }
                }
                bool ya = false;
                for (int c = 0; c < codigoBar_Personal.Length; c++)
                {
                    if (codigoBar_Personal.Substring(c, 1) != "x")
                    {
                        ya = false;
                        for (int s = (c + 1); s < codigoBar_Personal.Length; s++)
                        {
                            if (codigoBar_Personal.Substring(c, 1) == codigoBar_Personal.Substring(s, 1))
                            {
                                if (ya)
                                {
                                    MessageBox.Show(this, Variable.SYS_MSJ[53, Variable.idioma] + " ( " + codigoBar_Personal.Substring(s, 1) + " )");
                                    c = codigoBar_Personal.Length;
                                    s = codigoBar_Personal.Length;
                                    return false;
                                }
                            }
                            else { ya = true; }
                        }
                    }
                }
                int cont1 = 0, cont2 = 0, cont3 = 0, cont4 = 0, cont5 = 0;
                for (int c = 0; c < codigoBar_Personal.Length; c++)
                {
                    switch (codigoBar_Personal.Substring(c, 1))
                    {
                        case "d": cont1++; break;
                        case "p": cont2++; break;
                        case "a": cont3++; break;
                        case "c": cont4++; break;
                        case "w": cont5++; break;
                    }
                }

                if (cont1 > 2)
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[54, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation); //"El Departamento no puede ser mayor de 2 caracteres (d)");
                    return false;
                }
                if (cont2 > 2)
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[55, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //"El Prefijo no puede ser mayor de 2 caracteres (p)");
                    return false;
                }
                if (cont3 > 3)
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[57, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);   //"El Vendedor no puede ser mayor de 2 caracteres (a)");
                    return false;
                }
                if (cont4 > 6)
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[56, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //"El Código no puede ser mayor de 6 caracteres (c)");
                    return false;
                }
                if (cont5 > 5)
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[58, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //"El Peso no puede ser mayor de 5 caracteres (w)");
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }
        
        private void texto_prefijo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.texto_depto.Focus();
        }
        private void texto_prefijo_Validating(object sender, CancelEventArgs e)
        {
            if (texto_prefijo.Text != "")
            {
                texto_prefijo.Text = Variable.validar_salida(texto_prefijo.Text, 0);
                Variable.user_prefijo = texto_prefijo.Text;
            }            
        }
        private void texto_prefijo_KeyPress(object sender, KeyPressEventArgs e)
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
        private void texto_prefijo_Enter(object sender, EventArgs e)
        {
            colorbefore = texto_prefijo.BackColor;
            texto_prefijo.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void texto_prefijo_Leave(object sender, EventArgs e)
        {
            texto_prefijo.BackColor = colorbefore;
        }

        private void texto_depto_Enter(object sender, EventArgs e)
        {
            colorbefore = texto_depto.BackColor;
            texto_depto.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void texto_depto_Leave(object sender, EventArgs e)
        {
            texto_depto.BackColor = colorbefore;
        }
        private void texto_depto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.texto_corrimiento.Focus();
        }
        private void texto_depto_Validating(object sender, CancelEventArgs e)
        {
            if (texto_depto.Text != "")
            {
                texto_depto.Text = Variable.validar_salida(texto_depto.Text, 0);
                Variable.user_depto = texto_depto.Text;
            }
            else texto_depto.Text = "0";
        }
        private void texto_depto_KeyPress(object sender, KeyPressEventArgs e)
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

        private void texto_corrimiento_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.chkNutriente.Focus();
        }
        private void texto_corrimiento_Validating(object sender, CancelEventArgs e)
        {
            if (texto_corrimiento.Text == "") texto_corrimiento.Text = "1";
            else Variable.user_corrimientopieza = Convert.ToSByte(texto_corrimiento.Text);
        }
        private void texto_corrimiento_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '0' || e.KeyChar == '1' || e.KeyChar == '2' || e.KeyChar == '3')
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
        private void texto_corrimiento_Enter(object sender, EventArgs e)
        {
            colorbefore = texto_corrimiento.BackColor;
            texto_corrimiento.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void texto_corrimiento_Leave(object sender, EventArgs e)
        {
            texto_corrimiento.BackColor = colorbefore;
        }

        private void texto_numing_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.texto_numenca.Focus();
        }       
        private void texto_numing_Enter(object sender, EventArgs e)
        {
            colorbefore = texto_numing.BackColor;
            texto_numing.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void texto_numing_Leave(object sender, EventArgs e)
        {
            texto_numing.BackColor = colorbefore;
        }
        private void texto_numing_Validating(object sender, CancelEventArgs e)
        {
            if (texto_numing.Text != "")
            {
                texto_numing.Text = Variable.validar_salida(texto_numing.Text, 0);
                Variable.user_formatosize.ningrediente = texto_numing.Text;
            }
            else texto_numing.Text = "0";
        }
        private void texto_numing_KeyPress(object sender, KeyPressEventArgs e)
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

        private void texto_numenca_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.rBxTipopapel.Focus();
        }
        private void texto_numenca_Validating(object sender, CancelEventArgs e)
        {
            if (texto_numenca.Text != "")
            {
                texto_numenca.Text = Variable.validar_salida(texto_numenca.Text, 0);
                Variable.user_formatosize.nencabezado = texto_numenca.Text;
            }
            else texto_numenca.Text = "0";
        }
        private void texto_numenca_KeyPress(object sender, KeyPressEventArgs e)
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
        private void texto_numenca_Enter(object sender, EventArgs e)
        {
            colorbefore = texto_numenca.BackColor;
            texto_numenca.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void texto_numenca_Leave(object sender, EventArgs e)
        {
            texto_numenca.BackColor = colorbefore;
        }

        private void tbxLargo_Validating(object sender, CancelEventArgs e)
        {            
            if (tbxLargo.Text != "")
            {                
                tbxLargo.Text = Variable.validar_salida(tbxLargo.Text, 0);
                if (Convert.ToInt16(tbxLargo.Text) > 99) { tbxLargo.Text = "99"; }
                Variable.user_formatosize.largo_medio = tbxLargo.Text;
            }
            else tbxLargo.Text = "0";
        }
        private void tbxLargo_KeyPress(object sender, KeyPressEventArgs e)
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
        private void tbxLargo_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxLargo.BackColor;
            tbxLargo.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxLargo_Leave(object sender, EventArgs e)
        {
            tbxLargo.BackColor = colorbefore;
        }

        private void tbxAncho_Validating(object sender, CancelEventArgs e)
        {
            if (tbxAncho.Text != "")
            {
                tbxAncho.Text = Variable.validar_salida(tbxAncho.Text, 0);
                if (Convert.ToInt16(tbxAncho.Text) > 56) { tbxAncho.Text = "56"; }
                Variable.user_formatosize.ancho_medio = tbxAncho.Text;
            }
            else tbxAncho.Text = "0";
        }
        private void tbxAncho_KeyPress(object sender, KeyPressEventArgs e)
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
        private void tbxAncho_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxAncho.BackColor;
            tbxAncho.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxAncho_Leave(object sender, EventArgs e)
        {
            tbxAncho.BackColor = colorbefore;
        }

        private void tbxSeparacion_Validating(object sender, CancelEventArgs e)
        {
            if (tbxSeparacion.Text != "")
            {
                tbxSeparacion.Text = Variable.validar_salida(tbxSeparacion.Text, 0);
                if (Convert.ToInt16(tbxSeparacion.Text) > 10) { tbxSeparacion.Text = "10"; }
                Variable.user_formatosize.separacion_medio = tbxSeparacion.Text;
            }
            else tbxSeparacion.Text = "0";
        }
        private void tbxSeparacion_KeyPress(object sender, KeyPressEventArgs e)
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
        private void tbxSeparacion_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxSeparacion.BackColor;
            tbxSeparacion.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxSeparacion_Leave(object sender, EventArgs e)
        {
            tbxSeparacion.BackColor = colorbefore;
        }
        #endregion

        #region Eventos de SrollBar
        private void ScrollBarContraEtiqueta_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue > 0)
            {
                Variable.user_contrasteetiqueta = e.NewValue;
            }
            else Variable.user_contrasteetiqueta = 1;
            lbxCetiqueta.Text = Variable.user_contrasteetiqueta.ToString();
        }
        private void ScrollBarContraPapel_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue > 0) Variable.user_contrastepapel = e.NewValue;
            else Variable.user_contrastepapel = 1;
            lbxCpapel.Text = Variable.user_contrastepapel.ToString();
        }
        private void ScrollBarRetardo_Scroll(object sender, ScrollEventArgs e)
        {
           Variable.user_retardoimpresion = e.NewValue;
           lbxRetardo.Text = Variable.user_retardoimpresion.ToString();
        }        
        #endregion
               
        #endregion

        private void UserFormatos_Load(object sender, EventArgs e)
        {
            bool existe;
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Grupo' Puede moverla o quitarla según sea necesario.
            this.grupoTableAdapter.Fill(this.baseDeDatosDataSet.Grupo);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Bascula' Puede moverla o quitarla según sea necesario.
            this.basculaTableAdapter.Fill(this.baseDeDatosDataSet.Bascula);

            this.impresorTableAdapter.Fill(this.baseDeDatosDataSet.Impresor);

            this.formatoTableAdapter.Fill(this.baseDeDatosDataSet.Formato);

            toolStripLabel3.Text = Nombre_Select;
            contextMenuStrip1.Items[0].Text = Variable.SYS_MSJ[402, Variable.idioma];
            contextMenuStrip1.Items[1].Text = Variable.SYS_MSJ[403, Variable.idioma];
            toolStripMenuItem9.Text = Variable.SYS_MSJ[243, Variable.idioma];
            Asigna_Bascula();
            Listado_Campos();
            Llenar_Formato();
            Listado_formatos(0);            
            
            ///codigo por producto
            ///
            ///EAN13 (pccccccwwwwwv) codigo peso
            ///EAN13 (pcccccctttttv) codigo total
            ///UPC13 (xpcccccwwwwwv) codigo peso
            ///UPC13 (xpccccctttttv) codigo total
            ///
            ///codigo por tikect
            ///EAN13 (pxaannttttttv) Vendedor Total
            ///EAN13 (pxddnnttttttv) Departamento Total
            ///UPC12 (xpaannttttttv) Vendedor Total
            ///UPC12 (xpddnnttttttv) Departamento Total
            ///
            
            OleDbDataReader OlPrint = Conec.Obtiene_Dato("Select * From Impresor Where id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo, Conec.CadenaConexion);
            if (OlPrint.Read()) existe = true;
            else existe = false;
            OlPrint.Close();

            if (existe)
            {
                Form1.statRegistro = ESTADO.EstadoRegistro.PKTRATADO;
                activarDesactivarEdicion(false, Color.WhiteSmoke);
                ActivaDesactivaFormato(false, Color.WhiteSmoke);
                Consulta_EnBD();
                StripEditar.Enabled = true;
                StripBorrar.Enabled = false;
                StripGuardar.Enabled = false;
                StripEnviar.Enabled = true;
            }
            else
            {
                Form1.statRegistro = ESTADO.EstadoRegistro.PKNOTRATADO;
                limpiezaTextBoxes();
                limpiezaLabelText();
                ActivaDesactivaFormato(false, Color.WhiteSmoke);
                cBxFormatoPersonalizados.Enabled = false;
                cBxCodigoPrd.Enabled = false;
                cBxCodigoTicket.Enabled = false;
                texto_codigoxprod.Enabled = false;
                texto_codigoxprod.BackColor = Color.WhiteSmoke;
                texto_codigoxticket.BackColor = Color.WhiteSmoke;
                rBxformato1_Standar.Enabled = false;
                rBxformato2_Standar.Enabled = false;
                rBxformato3_Standar.Enabled = false;
                rBxformato_Personalizado.Enabled = false;
                texto_codigoxticket.Enabled = false;
                StripEditar.Enabled = false;
                StripBorrar.Enabled = false;
                StripGuardar.Enabled = true;
                StripEnviar.Enabled = false;
            }

            if (Variable.TypeScale == Variable.TipoBascula.WLABEL)
            {
                funcionWLABEL();
            }

        }
        #region Evento de los controles de texto
       
        #endregion

      
    }
}


