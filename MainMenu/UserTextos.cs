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
    public partial class UserTextos : UserControl
    {
        public Int32 Num_Grupo;
        public Int32 Num_Bascula;
        public String Nombre_Select;

        delegate void SetTextCallback(string text);

        Variable.lgrupo[] myGrupo;
        Variable.lbasc[] myScale;
        Variable.headers[] myHeaders = new Variable.headers[15];

        private TextBox[] texto_encabezado;
        private ComboBox[] tamano_encabezado;
        private ComboBox[] alineacion_encabezado;
        private Label[] Nreg;
        private string[] it2 = new string[] {Variable.SYS_MSJ[243,Variable.idioma], "1-8x16", "2-16x16", "3-16x24", "4-24x24", "5-24x32", "6-16x32" };
        private string[] it = new string[] { Variable.SYS_MSJ[244, Variable.idioma], Variable.SYS_MSJ[245, Variable.idioma], Variable.SYS_MSJ[246, Variable.idioma] };   //"I Izquierdo", "C Centro", "D Derecha" };

        ESTADO.botonesEnvioDato DatosEnviado = new ESTADO.botonesEnvioDato();
        ADOutil Conec = new ADOutil();
        Conexion Cte = new Conexion();
        Serial SR = new Serial();
        ArrayList Dato_Nuevo = new ArrayList();
        Socket Cliente_bascula = null;
        Envia_Dato Env = new Envia_Dato();
        
        #region Inicializacion
        public UserTextos()
        {
            InitializeComponent();

            this.tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + string.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);

            texto_encabezado = new TextBox[15]{this.texto_encabezado1,
												 this.texto_encabezado2,
												 this.texto_encabezado3,
												 this.texto_encabezado4,
												 this.texto_encabezado5,
												 this.texto_encabezado6,
												 this.texto_encabezado7,
												 this.texto_encabezado8,
												 this.texto_encabezado9,
												 this.texto_encabezado10, 
                                                 this.texto_encabezado11,
                                                 this.texto_encabezado12,
                                                 this.texto_encabezado13,
                                                 this.texto_encabezado14,
                                                 this.texto_encabezado15};

            this.tamano_encabezado1.Items.AddRange(it2);
            this.tamano_encabezado2.Items.AddRange(it2);
            this.tamano_encabezado3.Items.AddRange(it2);
            this.tamano_encabezado4.Items.AddRange(it2);
            this.tamano_encabezado5.Items.AddRange(it2);
            this.tamano_encabezado6.Items.AddRange(it2);
            this.tamano_encabezado7.Items.AddRange(it2);
            this.tamano_encabezado8.Items.AddRange(it2);
            this.tamano_encabezado9.Items.AddRange(it2);
            this.tamano_encabezado10.Items.AddRange(it2);
            this.tamano_encabezado11.Items.AddRange(it2);
            this.tamano_encabezado12.Items.AddRange(it2);
            this.tamano_encabezado13.Items.AddRange(it2);
            this.tamano_encabezado14.Items.AddRange(it2);
            this.tamano_encabezado15.Items.AddRange(it2);

            tamano_encabezado = new ComboBox[15]{this.tamano_encabezado1,
												   this.tamano_encabezado2,
												   this.tamano_encabezado3,
												   this.tamano_encabezado4,
												   this.tamano_encabezado5,
												   this.tamano_encabezado6,
												   this.tamano_encabezado7,
												   this.tamano_encabezado8,
												   this.tamano_encabezado9,
												   this.tamano_encabezado10,
                                                   this.tamano_encabezado11,
												   this.tamano_encabezado12,
												   this.tamano_encabezado13,
												   this.tamano_encabezado14,
												   this.tamano_encabezado15};

            this.alineacion_encabezado1.Items.AddRange(it);
            this.alineacion_encabezado2.Items.AddRange(it);
            this.alineacion_encabezado3.Items.AddRange(it);
            this.alineacion_encabezado4.Items.AddRange(it);
            this.alineacion_encabezado5.Items.AddRange(it);
            this.alineacion_encabezado6.Items.AddRange(it);
            this.alineacion_encabezado7.Items.AddRange(it);
            this.alineacion_encabezado8.Items.AddRange(it);
            this.alineacion_encabezado9.Items.AddRange(it);
            this.alineacion_encabezado10.Items.AddRange(it);
            this.alineacion_encabezado11.Items.AddRange(it);
            this.alineacion_encabezado12.Items.AddRange(it);
            this.alineacion_encabezado13.Items.AddRange(it);
            this.alineacion_encabezado14.Items.AddRange(it);
            this.alineacion_encabezado15.Items.AddRange(it);

            alineacion_encabezado = new ComboBox[15]{this.alineacion_encabezado1,
														this.alineacion_encabezado2,
														this.alineacion_encabezado3,
														this.alineacion_encabezado4,
														this.alineacion_encabezado5,
														this.alineacion_encabezado6,
														this.alineacion_encabezado7,
														this.alineacion_encabezado8,
														this.alineacion_encabezado9,
														this.alineacion_encabezado10,
                                                        this.alineacion_encabezado11,
														this.alineacion_encabezado12,
														this.alineacion_encabezado13,
														this.alineacion_encabezado14,
														this.alineacion_encabezado15};

            Nreg = new Label[15]{this.NRen0,this.NRen1,this.NRen2,this.NRen3,this.NRen4,
                                 this.NRen5,this.NRen6,this.NRen7,this.NRen8,this.NRen9,
                                 this.NRen10,this.NRen11,this.NRen12,this.NRen13,this.NRen14};

            asignarEventos();
        }
        #endregion

        #region Consulta y escritura de Base de Datos
        void Asigna_Grupo()
        {
            Conec.CadenaSelect = "SELECT * FROM Grupo ORDER BY id_grupo";

            grupoTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            grupoTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            grupoTableAdapter.Fill(baseDeDatosDataSet.Grupo);

            myGrupo = new Variable.lgrupo[baseDeDatosDataSet.Grupo.Rows.Count];
            int nitem = 0;

            foreach (DataRow dr in baseDeDatosDataSet.Grupo.Rows)
            {
                myGrupo[nitem].ngpo = Convert.ToInt32(dr["id_grupo"].ToString());
                myGrupo[nitem].nombre = dr["nombre_gpo"].ToString();
                nitem++;
            }
        }
        void Asigna_Bascula()
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
        void Asigna_Encabezado()
        {
            for (int i = 0; i < texto_encabezado.Length; i++)
            {
                if (texto_encabezado[i].Text.Length > 0)
                {
                    myHeaders[i].texto = texto_encabezado[i].Text;
                    myHeaders[i].tam = tamano_encabezado[i].SelectedIndex;
                    switch (alineacion_encabezado[i].SelectedIndex)
                    {
                        case 0: myHeaders[i].centrado = 'I'; break;
                        case 1: myHeaders[i].centrado = 'C'; break;
                        case 2: myHeaders[i].centrado = 'D'; break;
                    }                   
                }
            }            
        }
        private void Consulta_EnBD()
        {
            Form1.statRegistro = ESTADO.EstadoRegistro.PKPARCIAL;

            textosTableAdapter.Fill(baseDeDatosDataSet.Textos);
            DataRow[] DB = baseDeDatosDataSet.Textos.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);
            if (DB.Length > 0)
            {
                Mostrar_Dato(DB[0]);
            }
            encabezadoTableAdapter.Fill(baseDeDatosDataSet.Encabezado);
            DataRow[] DA = baseDeDatosDataSet.Encabezado.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);
            if (DA.Length > 0)
            {
                Mostrar_Dato(DA);
            }
        }

        private void Mostrar_Dato(DataRow[] DA)
        {
            int nposicion = 0;

            foreach (DataRow dr in DA)
            {
                nposicion = Convert.ToInt16(dr["posicion"].ToString());
                myHeaders[nposicion].texto = dr["encabezado"].ToString();
                myHeaders[nposicion].tam = Convert.ToInt16(dr["letra"].ToString());
                myHeaders[nposicion].centrado = Convert.ToChar(dr["align"].ToString());

                texto_encabezado[nposicion].Text = dr["encabezado"].ToString();
                tamano_encabezado[nposicion].SelectedIndex = Convert.ToInt16(dr["letra"].ToString());
                switch (myHeaders[nposicion].centrado)
                {
                    case 'I': alineacion_encabezado[nposicion].SelectedIndex = 0; break;
                    case 'C': alineacion_encabezado[nposicion].SelectedIndex = 1; break;
                    case 'D': alineacion_encabezado[nposicion].SelectedIndex = 2; break;
                }
            }
            this.tamano_encabezado1.Focus();
        }
        private void Mostrar_Dato(DataRow dr)
        {
            this.texto_prepieza.Text = dr["tx_preciopieza"].ToString();
            this.texto_pesopieza.Text = dr["tx_pesopieza"].ToString();
            this.texto_preciopapel.Text = dr["tx_precioticket"].ToString();
            this.texto_Pesopapel.Text = dr["tx_pesoticket"].ToString();
            this.texto_ttotal.Text = dr["tx_total"].ToString();
            this.texto_tprecio.Text = dr["tx_precio"].ToString();
            this.texto_tpeso.Text = dr["tx_peso"].ToString();
            this.texto_tfechacad.Text = dr["tx_textofechacad"].ToString();
            this.texto_tfecha.Text = dr["tx_fecha"].ToString();
            this.texto_tvendedor.Text = dr["tx_vendedor"].ToString();
            this.texto_tadicional.Text = dr["tx_mensaje"].ToString();
            this.texto_ttalon.Text = dr["tx_talon"].ToString();
            this.texto_tdevolucion.Text = dr["tx_devolucion"].ToString();
            this.texto_tdescuento.Text = dr["tx_descuento"].ToString();
            this.texto_tefectivo.Text = dr["tx_efectivo"].ToString();
            this.texto_tcambio.Text = dr["tx_cambio"].ToString();
            this.texto_ttara.Text = dr["tx_tara"].ToString();
            this.tbxFecha.Text = String.Format(Variable.F_Hora, Convert.ToDateTime(dr["actualizado"].ToString())) + " " + String.Format(Variable.FOR_FECHAS[Variable.ffecha], Convert.ToDateTime(dr["actualizado"].ToString())); ;
            this.tamano_encabezado1.Focus();
        }
        void activarDesactivarEdicion(bool pbActivar, Color clActivar)
        {
            for (int i = 0; i < texto_encabezado.Length; i++)
            {
                texto_encabezado[i].Enabled = pbActivar;
                texto_encabezado[i].BackColor = clActivar;
            }
            for (int i = 0; i < alineacion_encabezado.Length; i++)
            {
                alineacion_encabezado[i].Enabled = pbActivar;
            }
            for (int i = 0; i < tamano_encabezado.Length; i++)
            {
                tamano_encabezado[i].Enabled = pbActivar;
            }
            this.Enabled = pbActivar;
            this.texto_preciopapel.Enabled = pbActivar;
            this.texto_Pesopapel.Enabled = pbActivar;
            this.texto_pesopieza.Enabled = pbActivar;
            this.texto_prepieza.Enabled = pbActivar;
            this.texto_tprecio.Enabled = pbActivar;           
            this.texto_ttotal.Enabled = pbActivar;
            this.texto_tprecio.Enabled = pbActivar;
            this.texto_tpeso.Enabled = pbActivar;
            this.texto_tfechacad.Enabled = pbActivar;
            this.texto_tfecha.Enabled = pbActivar;
            this.texto_tvendedor.Enabled = pbActivar;
            this.texto_tadicional.Enabled = pbActivar;
            this.texto_ttalon.Enabled = pbActivar;
            this.texto_tdevolucion.Enabled = pbActivar;
            this.texto_tdescuento.Enabled = pbActivar;
            this.texto_tefectivo.Enabled = pbActivar;
            this.texto_tcambio.Enabled = pbActivar;
            this.texto_ttara.Enabled = pbActivar;

            this.texto_preciopapel.BackColor = clActivar;
            this.texto_Pesopapel.BackColor = clActivar;
            this.texto_pesopieza.BackColor = clActivar;
            this.texto_prepieza.BackColor = clActivar;
            this.texto_ttotal.BackColor = clActivar;
            this.texto_tprecio.BackColor = clActivar;
            this.texto_tpeso.BackColor = clActivar;
            this.texto_tfechacad.BackColor = clActivar;
            this.texto_tfecha.BackColor = clActivar;
            this.texto_tvendedor.BackColor = clActivar;
            this.texto_tadicional.BackColor = clActivar;
            this.texto_ttalon.BackColor = clActivar;
            this.texto_tdevolucion.BackColor = clActivar;
            this.texto_tdescuento.BackColor = clActivar;
            this.texto_tefectivo.BackColor = clActivar;
            this.texto_tcambio.BackColor = clActivar;
            this.texto_ttara.BackColor = clActivar;
        }
        private void limpiezaTextBoxes()
        {
            activarDesactivarEdicion(true, Color.White);
            texto_encabezado[0].Text=Variable.SYS_MSJ[316,Variable.idioma];            
            tamano_encabezado[0].SelectedIndex = 4;
            alineacion_encabezado[0].SelectedIndex = 1;
            texto_encabezado[1].Text=Variable.SYS_MSJ[317,Variable.idioma];  
            tamano_encabezado[1].SelectedIndex = 4;
            alineacion_encabezado[1].SelectedIndex = 1;
            for (int i = 2; i < texto_encabezado.Length; i++)
            {
                texto_encabezado[i].Clear();
            }
            for (int i = 2; i < alineacion_encabezado.Length; i++)
            {
                alineacion_encabezado[i].SelectedIndex = 0;
            }
            for (int i = 2; i < tamano_encabezado.Length; i++)
            {
                tamano_encabezado[i].SelectedIndex = 0;
            }
            this.texto_Pesopapel.Text = Variable.SYS_MSJ[314, Variable.idioma]; //texto peso papel
            this.texto_preciopapel.Text = Variable.SYS_MSJ[315, Variable.idioma]; //texto precio papel
            this.texto_prepieza.Text = Variable.SYS_MSJ[313, Variable.idioma]; // texto precio pieza
            this.texto_pesopieza.Text = Variable.SYS_MSJ[312, Variable.idioma]; //texto precio pieza
            this.texto_ttotal.Text = Variable.SYS_MSJ[292, Variable.idioma];      //TextoTotal
            this.texto_tprecio.Text = Variable.SYS_MSJ[291, Variable.idioma];     //TextoPrecio
            this.texto_tpeso.Text = Variable.SYS_MSJ[290, Variable.idioma];       //TextoPeso
            this.texto_tfechacad.Text = Variable.SYS_MSJ[310, Variable.idioma];   //TextoFechaCad
            this.texto_tfecha.Text = Variable.SYS_MSJ[309, Variable.idioma];      //TextoFecha
            this.texto_tvendedor.Text = Variable.SYS_MSJ[302, Variable.idioma];   //TextoVendedor
            this.texto_tadicional.Text = Variable.SYS_MSJ[303, Variable.idioma];  //TextoMensaje
            this.texto_ttalon.Text = Variable.SYS_MSJ[304, Variable.idioma];
            this.texto_tdevolucion.Text = Variable.SYS_MSJ[305, Variable.idioma];
            this.texto_tdescuento.Text = Variable.SYS_MSJ[306, Variable.idioma];
            this.texto_tefectivo.Text = Variable.SYS_MSJ[307, Variable.idioma];
            this.texto_tcambio.Text = Variable.SYS_MSJ[308, Variable.idioma];
            this.texto_ttara.Text = Variable.SYS_MSJ[311, Variable.idioma];

            this.texto_encabezado1.Focus();

            Form1.statRegistro = ESTADO.EstadoRegistro.PKNOTRATADO;
        }
        private void Guardar(bool Existe)
        {
            object[] clave = new object[2];
            clave[0] = Num_Bascula;
            clave[1] = Num_Grupo;

            textosTableAdapter.Fill(baseDeDatosDataSet.Textos);
            baseDeDatosDataSet.Textos.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Textos.id_basculaColumn, baseDeDatosDataSet.Textos.id_grupoColumn };

            string fecha_act = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);
            tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);

            if (!Existe)
            {
                Conec.CadenaSelect = "INSERT INTO Textos " +
                "(id_bascula, id_grupo, tx_preciopieza, tx_pesopieza,tx_precioticket,tx_pesoticket, tx_precio,tx_peso,tx_total,tx_fecha,tx_textofechacad,tx_vendedor,tx_mensaje,tx_talon,tx_devolucion,tx_descuento,tx_cambio,tx_efectivo,tx_tara,actualizado,pendiente)" +
               "VALUES (" + Num_Bascula + "," +
                   Num_Grupo + ",'" +
                   texto_prepieza.Text + "','" +
                   texto_pesopieza.Text + "','" +
                   texto_preciopapel.Text + "','" +
                   texto_Pesopapel.Text + "','" +
                   texto_tprecio.Text + "','" +
                   texto_tpeso.Text + "','" +
                   texto_ttotal.Text + "','" +
                   texto_tfecha.Text + "','" +
                   texto_tfechacad.Text + "','" +
                   texto_tvendedor.Text + "','" +
                   texto_tadicional.Text + "','" +
                   texto_ttalon.Text + "','" +
                   texto_tdevolucion.Text + "','" +
                   texto_tdescuento.Text + "','" +
                   texto_tcambio.Text + "','" +
                   texto_tefectivo.Text + "','" +
                   texto_ttara.Text + "','" +
                   fecha_act + "'," +
                   true + ")";

                Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Textos.TableName);
            }
            else
            {
                Conec.Condicion = "id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo;
                Conec.CadenaSelect = "UPDATE Textos SET " +
                    "tx_preciopieza = '" + texto_prepieza.Text + "'," +
                    "tx_pesopieza = '" + texto_pesopieza.Text + "'," +
                    "tx_precioticket = '" + texto_preciopapel.Text + "'," +
                    "tx_pesoticket = '" + texto_Pesopapel.Text + "'," +
                    "tx_precio = '" + texto_tprecio.Text + "'," +
                    "tx_peso = '" + texto_tpeso.Text + "'," +
                    "tx_total = '" + texto_ttotal.Text + "'," +
                    "tx_fecha = '" + texto_tfecha.Text + "'," +
                    "tx_textofechacad = '" + texto_tfechacad.Text + "'," +
                    "tx_vendedor = '" + texto_tvendedor.Text + "'," +
                    "tx_mensaje = '" + texto_tadicional.Text + "'," +
                    "tx_talon = '" + texto_ttalon.Text + "'," +
                    "tx_devolucion = '" + texto_tdevolucion.Text + "'," +
                    "tx_descuento = '" + texto_tdescuento.Text + "'," +
                    "tx_cambio = '" + texto_tcambio.Text + "'," +
                    "tx_efectivo = '" + texto_tefectivo.Text + "'," +
                    "tx_tara = '" + texto_ttara.Text + "'," +
                    "pendiente = " + true + "," +
                    "actualizado = '" + fecha_act + "' " +
                    "WHERE (" + Conec.Condicion + ")";

                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Textos.TableName);
            }

            object[] clave2 = new object[3];
            clave2[0] = Num_Bascula;
            clave2[1] = Num_Grupo;

            encabezadoTableAdapter.Fill(baseDeDatosDataSet.Encabezado);
            baseDeDatosDataSet.Encabezado.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Encabezado.id_basculaColumn, baseDeDatosDataSet.Encabezado.id_grupoColumn, baseDeDatosDataSet.Encabezado.id_encabezadoColumn };
            
            for (int i = 0; i < texto_encabezado.Length; i++)
            {
                if (texto_encabezado[i].Text.Length > 0)
                {
                    clave2[2] = Nreg[i].Text;

                    DataRow de = baseDeDatosDataSet.Encabezado.Rows.Find(clave2);

                    if (de != null)
                    {
                        Conec.Condicion = "id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo + " and id_encabezado = " + Convert.ToInt16(Nreg[i].Text);
                        Conec.CadenaSelect = "UPDATE Encabezado SET " +
                        "posicion = " + i + "," +
                        "encabezado = '" + myHeaders[i].texto + "'," +
                        "letra = " + myHeaders[i].tam + "," +
                        "align = '" + myHeaders[i].centrado + "' " +
                        "WHERE (" + Conec.Condicion + ")";  //"',pendiente = " + true +

                        Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Encabezado.TableName);
                    }
                    else
                    {
                        DataRow dh = baseDeDatosDataSet.Encabezado.NewRow();

                        Conec.CadenaSelect = "INSERT INTO Encabezado " +
                            "(id_bascula, id_grupo, id_encabezado,posicion,encabezado,letra,align)" +  //,pendiente
                            "VALUES (" + Num_Bascula + "," +
                            Num_Grupo + "," +
                            Convert.ToInt16(Nreg[i].Text) + "," +
                            i + ",'" +
                            myHeaders[i].texto + "'," +
                            myHeaders[i].tam + ",'" +
                            myHeaders[i].centrado + "')";  //," + true + ")";

                        Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Encabezado.TableName);
                    }
                }
                else
                {
                    Conec.Condicion = "id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo + " and id_encabezado = " + Convert.ToInt16(Nreg[i].Text);
                    Conec.CadenaSelect = "DELETE FROM Encabezado WHERE (" + Conec.Condicion + ")"; 
                           
                    Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Encabezado.TableName);
                }
            }
        }    
        #endregion

        #region Botones del ToolStripMenu
        private void StripEnviar_Click(object sender, EventArgs e)
        {
            encabezadoTableAdapter.Fill(baseDeDatosDataSet.Encabezado);
            textosTableAdapter.Fill(baseDeDatosDataSet.Textos);
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
                        /*Variable.IP_Address = myScale[pos].ip;  //direccion ip de la bascula
                        Variable.nidbas = myScale[pos].idbas;   //numero id de la bascula
                        Variable.Nserie = myScale[pos].nserie;  //numero de serie de la bascula                    
                        Variable.Bascula = myScale[pos].nombre;  //mombre de la bascula 
                        Variable.Nombre = myScale[pos].modelo; // descripcion o modelo de la bascula
                        Variable.nsucursal = myScale[pos].gpo;  //Grupo al que pertenece la bascula
                        Variable.ppeso = myScale[pos].um;*/
                        Variable.P_COMM = myScale[pos].pto;
                        Variable.Buad = myScale[pos].baud;
                        BasculasActualizadas++;

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
                            if (EnviaDatosA_Bascula(ref serialPort1, myScale[pos].pto, myScale[pos].baud))
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
                        /*Variable.IP_Address = myScale[pos].ip;  //direccion ip de la bascula
                        Variable.nidbas = myScale[pos].idbas;   //numero id de la bascula
                        Variable.Nserie = myScale[pos].nserie;  //numero de serie de la bascula                    
                        Variable.Bascula = myScale[pos].nombre;  //mombre de la bascula 
                        Variable.Nombre = myScale[pos].modelo; // descripcion o modelo de la bascula
                        Variable.nsucursal = myScale[pos].gpo;  //Grupo al que pertenece la bascula
                        Variable.ppeso = myScale[pos].um;*/
                        Variable.P_COMM = myScale[pos].pto;
                        Variable.Buad = myScale[pos].baud;
                        BasculasActualizadas++;

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
                            if (EnviaDatosA_Bascula(ref serialPort1, myScale[pos].pto, myScale[pos].baud))
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

            MessageBox.Show(this, Variable.SYS_MSJ[418, Variable.idioma] + " " + BasculasActualizadas + " "
                + Variable.SYS_MSJ[419, Variable.idioma] + " " + NumeroDeBaculas,
                Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void StripCerrar_Click(object sender, EventArgs e)
        {
            ToolStripManager.RevertMerge("toolStrip3");
            this.Dispose();
        }
        private void StripGuardar_Click(object sender, EventArgs e)
        {
            bool Existe = false;

            Asigna_Encabezado();

            OleDbDataReader OLConfig = Conec.Obtiene_Dato("Select * From Textos Where id_bascula = " + Num_Bascula + "AND id_grupo = " + Num_Grupo, Conec.CadenaConexion);
            if (OLConfig.Read()) Existe = true;
            else Existe = false;
            OLConfig.Close();

            Guardar(Existe);

            MessageBox.Show(this, Variable.SYS_MSJ[48, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);  //"Configuracion Guardada");
         
            activarDesactivarEdicion(false, Color.WhiteSmoke);
            Consulta_EnBD();

            StripEditar.Enabled = true;
            StripBorrar.Enabled = true;
            StripGuardar.Enabled = false;
            StripEnviar.Enabled = true;
        }
        private void StripBorrar_Click(object sender, EventArgs e)
        {
            bool existe;
            OleDbDataReader OlHead = Conec.Obtiene_Dato("Select * From Encabezado Where id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo, Conec.CadenaConexion);
            if (OlHead.Read()) existe = true;
            else existe = false;
            OlHead.Close();

            if (existe)
            {
                Conec.Condicion = "id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo;

                Conec.CadenaSelect = "DELETE * FROM Encabezado WHERE (" + Conec.Condicion + ")";
                Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Configuracion.TableName);                                
            }

             OleDbDataReader OlTex = Conec.Obtiene_Dato("Select * From Textos Where id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo, Conec.CadenaConexion);
             if (OlTex.Read()) existe = true;
            else existe = false;
             OlTex.Close();

            if (existe)
            {
                Conec.Condicion = "id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo;

                Conec.CadenaSelect = "DELETE * FROM Textos WHERE (" + Conec.Condicion + ")";
                Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Textos.TableName);
            }

            limpiezaTextBoxes();
            StripEditar.Enabled = false;
            StripBorrar.Enabled = false;
            StripGuardar.Enabled = true;
            StripEnviar.Enabled = false;
        }
        private void StripEditar_Click(object sender, EventArgs e)
        {
            object[] clave = new object[2] { Num_Bascula, Num_Grupo };
            Form1.statRegistro = ESTADO.EstadoRegistro.PKPARCIAL;
            
            DataRow[] DA = baseDeDatosDataSet.Encabezado.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);
            Mostrar_Dato(DA);

            DataRow[] DB = baseDeDatosDataSet.Textos.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);
            if (DB.Length > 0) Mostrar_Dato(DB[0]);
            
            activarDesactivarEdicion(true, Color.White);

            StripEditar.Enabled = false;
            StripBorrar.Enabled = true;
            StripGuardar.Enabled = true;
            StripEnviar.Enabled = false;
        }
        #endregion

        #region Envio de informacion a basculas
        private bool EnviaDatosA_Bascula(int Info_A_Enviar, string direccionIP)
        {        
            int reg_total = 0;
            bool ERROR = false;

            ProgressContinue pro = new ProgressContinue();
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                
                Cliente_bascula = Cte.conectar(direccionIP, 50036);  //, Variable.frame, ref Dato_Recivido);
                if (Cliente_bascula != null)
                {
                    reg_total = baseDeDatosDataSet.Textos.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo).Length;
                    reg_total += baseDeDatosDataSet.Encabezado.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo).Length;

                    pro.IniciaProcess(reg_total, Variable.SYS_MSJ[258, Variable.idioma]);  //"Textos y Encabezado","Iniciando Proceso");

                    string sComando = "XX" + (char)9 + (char)10;
                    string Msj_recibido = Env.Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");

                    if (Msj_recibido != null)
                    {
                        Enviar_Texto(direccionIP, ref Cliente_bascula, ref pro);
                        Enviar_Encabezado(direccionIP, ref Cliente_bascula, ref pro);
                    }
                    else
                    {
                        ERROR = true;
                    }
           
                    Cte.desconectar(ref Cliente_bascula);
                }
                else
                {
                    ERROR = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Variable.SYS_MSJ[381, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                ERROR = true;
            }

            Cursor.Current = Cursors.Default;
            pro.TerminaProcess();
            Thread.Sleep(500);

            return ERROR;
        }
        private bool EnviaDatosA_Bascula(ref SerialPort Cliente_bascula, string puerto, Int32 Baud_Rate)
        {          
            int reg_total = 0;
            bool ERROR = false;

            ProgressContinue pro = new ProgressContinue();
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                serialPort1 = new SerialPort();

                if (SR.OpenPort(ref Cliente_bascula, puerto, Baud_Rate))
                {
                    SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)9 + (char)10);
                    SR.ReceivedCOMSerial(ref serialPort1);

                    reg_total = baseDeDatosDataSet.Textos.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo).Length;
                    reg_total += baseDeDatosDataSet.Encabezado.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo).Length;

                    
                    pro.IniciaProcess(reg_total, Variable.SYS_MSJ[258, Variable.idioma]);  //"Textos y Encabezado","Iniciando Proceso");

                    Enviar_Texto(ref Cliente_bascula, ref pro);
                    Enviar_Encabezado(ref Cliente_bascula, ref pro);

                    Cursor.Current = Cursors.Default;
                    SR.SendCOMSerial(ref serialPort1, "DXXX" + (char)9 + (char)10);
                    SR.ReceivedCOMSerial(ref serialPort1);

                    SR.ClosePort(ref Cliente_bascula);
                    
                }
                else
                {             
                    ERROR = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Variable.SYS_MSJ[381, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                ERROR = true;
            }

            pro.TerminaProcess();   //"Proceso Terminado");
            Thread.Sleep(500);
            Cursor.Current = Cursors.Default;
            return ERROR;
        }
        private void Enviar_Texto(string direccionIP, ref Socket Cliente_bascula, ref ProgressContinue pro)
        {
            int reg_leido = 1;
            int reg_envio = 0;
            int reg_total = 0;
            string Msg_recibido;
            string Variable_frame = null;

            Variable_frame = "";
            DataRow[] DR_Texto = baseDeDatosDataSet.Textos.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);
            reg_total = DR_Texto.Length;

            foreach (DataRow DP in DR_Texto)
            {
                Variable_frame = Variable_frame + Env.Genera_Trama_Config_Texto(DP);
                reg_envio++;

                Msg_recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo, "GT");

                if (Msg_recibido != null)
                {
                    pro.UpdateProcess(1, Variable.SYS_MSJ[258, Variable.idioma]);
                }

                reg_leido = 1;
                Variable_frame = "";
            }
        }
        private void Enviar_Texto(ref SerialPort Cliente_bascula, ref ProgressContinue pro)
        {
            int reg_leido = 1;
            int reg_envio = 0;
            int reg_total = 0;
            string strcomando;
            string[] Dato_recibido = null;
            string Variable_frame = null;

            Variable_frame = "";
            DataRow[] DR_Texto = baseDeDatosDataSet.Textos.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);
            reg_total = DR_Texto.Length;

            foreach (DataRow DP in DR_Texto)
            {
                Variable_frame = Variable_frame + Env.Genera_Trama_Config_Texto(DP);
                reg_envio++;
                strcomando = "GT" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;

                SR.SendCOMSerial(ref Cliente_bascula,strcomando,ref Dato_recibido);
                if (Dato_recibido[0].IndexOf("Error") >= 0)
                {
                    Env.Guardar_Trama_pendiente(Num_Bascula, Num_Grupo, strcomando);
                }
                pro.UpdateProcess(1, reg_envio.ToString() + "/" + reg_total.ToString());
                reg_leido = 1;
                Variable_frame = "";
            }
        }
        private void Enviar_Encabezado(string direccionIP, ref Socket Cliente_bascula, ref ProgressContinue pro)
        {
            int reg_leido = 1;
            int reg_envio = 0;
            int reg_total = 0;
            string Msg_recibido;
            string Variable_frame = null;

            DataRow[] DR_Head = baseDeDatosDataSet.Encabezado.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);
            reg_total = DR_Head.Length;

            Variable_frame = "";

            foreach (DataRow DP in DR_Head)
            {
                Variable_frame = Variable_frame + Env.Genera_Trama_Config_Encabezado(DP);
                reg_envio++;

                Msg_recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo, "GH");
                pro.UpdateProcess(1, Variable.SYS_MSJ[258, Variable.idioma]);
                reg_leido = 1;
                Variable_frame = "";
            }
           
        }
        private void Enviar_Encabezado(ref SerialPort Cliente_bascula, ref ProgressContinue pro)
        {
            int reg_leido = 1;
            int reg_envio = 0;
            int reg_total = 0;
            string strcomando;
            string Variable_frame = null;
            string[] Dato_recibido = null;

            DataRow[] DR_Head = baseDeDatosDataSet.Encabezado.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);
            reg_total = DR_Head.Length;

            Variable_frame = "";

            foreach (DataRow DP in DR_Head)
            {
                Variable_frame = Variable_frame + Env.Genera_Trama_Config_Encabezado(DP);
                reg_envio++;
                strcomando = "GH" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;

                SR.SendCOMSerial(ref Cliente_bascula, strcomando, ref Dato_recibido);
                if (Dato_recibido[0].IndexOf("Error") >= 0)
                {
                    Env.Guardar_Trama_pendiente(Num_Bascula, Num_Grupo, strcomando);
                }
                pro.UpdateProcess(1, reg_envio.ToString() + "/" + reg_total.ToString());
                reg_leido = 1;
                Variable_frame = "";
            }
        }
        #endregion
           
        #region Eventos de control
        private void asignarEventos()
        {
            foreach (System.Windows.Forms.ComboBox tamano in tamano_encabezado)
            {
                tamano.SelectedIndexChanged += new EventHandler(tamano_encabezado_SelectedIndexChanged);
                tamano.KeyDown += new KeyEventHandler(tamano_encabezado_KeyDown);
            }
            foreach (System.Windows.Forms.ComboBox alineacion in alineacion_encabezado)
            {
                alineacion.SelectedIndexChanged += new EventHandler(alineacion_encabezado_SelectedIndexChanged);
                alineacion.KeyDown += new KeyEventHandler(alineacion_encabezado_KeyDown);
            }
            foreach (System.Windows.Forms.TextBox encabezado in texto_encabezado)
            {
                encabezado.LostFocus += new System.EventHandler(this.encabezado_LostFocus);
                encabezado.KeyDown += new System.Windows.Forms.KeyEventHandler(this.encabezado_KeyDown);
                encabezado.KeyPress += new KeyPressEventHandler(encabezado_KeyPress);
            }
            this.texto_Pesopapel.LostFocus += texto_Pesopapel_LostFocus;
            this.texto_pesopieza.LostFocus += texto_pesopieza_LostFocus;
            this.texto_preciopapel.LostFocus += texto_preciopapel_LostFocus;
            this.texto_prepieza.LostFocus += texto_prepieza_LostFocus;
            this.texto_tadicional.LostFocus += texto_tadicional_LostFocus;
            this.texto_tcambio.LostFocus += texto_tcambio_LostFocus;
            this.texto_tdescuento.LostFocus += texto_tdescuento_LostFocus;
            this.texto_tdevolucion.LostFocus += texto_tdevolucion_LostFocus;
            this.texto_tefectivo.LostFocus += texto_tefectivo_LostFocus;
            this.texto_tfecha.LostFocus += texto_tfecha_LostFocus;
            this.texto_tfechacad.LostFocus += texto_tfechacad_LostFocus;
            this.texto_tpeso.LostFocus += texto_tpeso_LostFocus;
            this.texto_tprecio.LostFocus += texto_tprecio_LostFocus;
            this.texto_ttalon.LostFocus += texto_ttalon_LostFocus;
            this.texto_ttara.LostFocus += texto_ttara_LostFocus;
            this.texto_ttotal.LostFocus += texto_ttotal_LostFocus;
            this.texto_tvendedor.LostFocus += texto_tvendedor_LostFocus;
        }

        void texto_tvendedor_LostFocus(object sender, EventArgs e)
        {
            texto_tvendedor.Text = Variable.validar_salida(texto_tvendedor.Text, 2);
        }
        void texto_ttotal_LostFocus(object sender, EventArgs e)
        {
            texto_ttotal.Text = Variable.validar_salida(texto_ttotal.Text, 2);
        }
        void texto_ttara_LostFocus(object sender, EventArgs e)
        {
            texto_ttara.Text = Variable.validar_salida(texto_ttara.Text, 2);
        }
        void texto_ttalon_LostFocus(object sender, EventArgs e)
        {
            texto_ttalon.Text = Variable.validar_salida(texto_ttalon.Text, 2);
        }
        void texto_tprecio_LostFocus(object sender, EventArgs e)
        {
            texto_tprecio.Text = Variable.validar_salida(texto_tprecio.Text, 2);
        }
        void texto_tpeso_LostFocus(object sender, EventArgs e)
        {
            texto_tpeso.Text = Variable.validar_salida(texto_tpeso.Text, 2);
        }

        void texto_tfechacad_LostFocus(object sender, EventArgs e)
        {
            texto_tfechacad.Text = Variable.validar_salida(texto_tfechacad.Text, 2);
        }
        void texto_tfecha_LostFocus(object sender, EventArgs e)
        {
            texto_tfecha.Text = Variable.validar_salida(texto_tfecha.Text, 2);
        }
        void texto_tefectivo_LostFocus(object sender, EventArgs e)
        {
            texto_tefectivo.Text = Variable.validar_salida(texto_tefectivo.Text, 2);
        }
        void texto_tdevolucion_LostFocus(object sender, EventArgs e)
        {
            texto_tdevolucion.Text = Variable.validar_salida(texto_tdevolucion.Text, 2);
        }
        void texto_tdescuento_LostFocus(object sender, EventArgs e)
        {
            texto_tdescuento.Text = Variable.validar_salida(texto_tdescuento.Text, 2);
        }

        void texto_tcambio_LostFocus(object sender, EventArgs e)
        {
            texto_tcambio.Text = Variable.validar_salida(texto_tcambio.Text, 2);
        }
        void texto_tadicional_LostFocus(object sender, EventArgs e)
        {
            texto_tadicional.Text = Variable.validar_salida(texto_tadicional.Text, 2);
        }
        void texto_prepieza_LostFocus(object sender, EventArgs e)
        {
            texto_prepieza.Text = Variable.validar_salida(texto_prepieza.Text, 2);
        }
        void texto_preciopapel_LostFocus(object sender, EventArgs e)
        {
            texto_preciopapel.Text = Variable.validar_salida(texto_preciopapel.Text, 2);
        }
        void texto_pesopieza_LostFocus(object sender, EventArgs e)
        {
            texto_pesopieza.Text = Variable.validar_salida(texto_pesopieza.Text, 2);
        }
        void texto_Pesopapel_LostFocus(object sender, EventArgs e)
        {
            texto_Pesopapel.Text = Variable.validar_salida(texto_Pesopapel.Text, 2);
        }
               
        private int buscar_posicion(int opc, string nom_control)
        {
            int posicion = -1;
            switch (opc)
            {
                case 0:
                    for (int i = 0; i < texto_encabezado.Length; i++)
                    {
                        if (texto_encabezado[i].Name == nom_control)
                        {
                            posicion = i;
                            break;
                        }
                    }
                    break;
                case 1:
                    for (int i = 0; i < alineacion_encabezado.Length; i++)
                    {
                        if (alineacion_encabezado[i].Name == nom_control)
                        {
                            posicion = i;
                            break;
                        }
                    }
                    break;
                case 2:
                    for (int i = 0; i < tamano_encabezado.Length; i++)
                    {
                        if (tamano_encabezado[i].Name == nom_control)
                        {
                            posicion = i;
                            break;
                        }
                    }
                    break;
            }
            return posicion;
        }
        private void encabezado_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            System.Windows.Forms.TextBox encabe = ((System.Windows.Forms.TextBox)sender);
            if (e.KeyCode == Keys.Enter)
            {                
                GetNextControl(encabe, true).Focus();
            }
        }
        private void encabezado_LostFocus(object sender, System.EventArgs e)
        {
            System.Windows.Forms.TextBox encabe = ((System.Windows.Forms.TextBox)sender);
            if (encabe.Text.Length > 0)
            {
                encabe.Text = Variable.validar_salida(encabe.Text, 2);
                int POS = buscar_posicion(0, encabe.Name);
                myHeaders[POS].texto = encabe.Text;
            }
        }
        private void encabezado_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) || Char.IsPunctuation(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (e.KeyChar == 'Ñ' || e.KeyChar == 'ñ')
            {
                e.Handled = true;
            }
            else if (e.KeyChar == '$' || e.KeyChar == '£' || e.KeyChar == '€')
            {
                e.Handled = false;
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
        private void tamano_encabezado_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox tama_encabe = ((System.Windows.Forms.ComboBox)sender);
            int POS = buscar_posicion(2, tama_encabe.Name);
            myHeaders[POS].tam = tama_encabe.SelectedIndex;
            GetNextControl(tama_encabe, true).Focus();
        }
        private void alineacion_encabezado_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox alinea_encabe = ((System.Windows.Forms.ComboBox)sender);
            int POS = buscar_posicion(1, alinea_encabe.Name);
            myHeaders[POS].centrado = Convert.ToChar(alinea_encabe.SelectedItem.ToString().Substring(0, 1));
            GetNextControl(alinea_encabe, true).Focus();
        }
        private void alineacion_encabezado_KeyDown(object sender, KeyEventArgs e)
        {
            System.Windows.Forms.ComboBox alinea_encabe = ((System.Windows.Forms.ComboBox)sender);
            if (e.KeyCode == Keys.Enter) GetNextControl(alinea_encabe, true).Focus();
        }
        private void tamano_encabezado_KeyDown(object sender, KeyEventArgs e)
        {
            System.Windows.Forms.ComboBox tama_encabe = ((System.Windows.Forms.ComboBox)sender);
            if (e.KeyCode == Keys.Enter) GetNextControl(tama_encabe, true).Focus();
        }
        private void texto_pesopieza_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.texto_prepieza.Focus();
        }
        private void texto_prepieza_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.texto_Pesopapel.Focus();
        }
        private void texto_Pesopapel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.texto_preciopapel.Focus();
        }
        private void texto_preciopapel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.texto_tpeso.Focus();
        }
        private void texto_tpeso_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.texto_tprecio.Focus();
        }
        private void texto_tprecio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.texto_ttotal.Focus();
        }
        private void texto_ttotal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.texto_tfecha.Focus();
        }
        private void texto_tfecha_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.texto_tfechacad.Focus();
        }
        private void texto_tfechacad_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.texto_tvendedor.Focus();
        }
        private void texto_tvendedor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.texto_tadicional.Focus();
        }
        private void texto_tadicional_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.texto_ttalon.Focus();
        }
        private void texto_ttalon_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.texto_tefectivo.Focus();
        }
        private void texto_tefectivo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.texto_tcambio.Focus();
        }
        private void texto_tcambio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.texto_ttara.Focus();
        }
        private void texto_ttara_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.texto_tdevolucion.Focus();
        }
        private void texto_tdevolucion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.texto_tdescuento.Focus();
        }
        private void texto_tdescuento_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.toolStrip31.Focus();
        }
        #endregion

        private void UserTextos_Load(object sender, EventArgs e)
        {
            bool existe;
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Grupo' Puede moverla o quitarla según sea necesario.
            this.grupoTableAdapter.Fill(this.baseDeDatosDataSet.Grupo);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Bascula' Puede moverla o quitarla según sea necesario.
            this.basculaTableAdapter.Fill(this.baseDeDatosDataSet.Bascula);

            this.encabezadoTableAdapter.Fill(this.baseDeDatosDataSet.Encabezado);

            this.textosTableAdapter.Fill(this.baseDeDatosDataSet.Textos);
            
            toolStripLabel3.Text = Nombre_Select;

            Asigna_Grupo();
            Asigna_Bascula();

            OleDbDataReader OlHead = Conec.Obtiene_Dato("Select * From Encabezado Where id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo, Conec.CadenaConexion);
            if (OlHead.Read()) existe = true;
            else existe = false;
            OlHead.Close();

            if (existe)
            {
                activarDesactivarEdicion(false, Color.WhiteSmoke);
                Consulta_EnBD();

                StripEditar.Enabled = true;
                StripBorrar.Enabled = true;
                StripGuardar.Enabled = false;
                StripEnviar.Enabled = true;
            }
            else
            {
                limpiezaTextBoxes();
                StripEditar.Enabled = false;
                StripBorrar.Enabled = false;
                StripGuardar.Enabled = true;
                StripEnviar.Enabled = false;
            }
        }        
    }
}


