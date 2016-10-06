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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using TorreyTransfer;
using TreeViewBound;

namespace MainMenu
{
    public partial class UserLecturas : UserControl
    { 
        public Int32 Num_Grupo;
        public Int32 Num_Bascula;
        public String Nombre_Select;
        
        Variable.lgrupo[] myGrupo;
        Variable.lbasc[] myScale;
        Variable.nodo_actual myCurrent;

        ESTADO.botonesEnvioDato DatosEnviado = new ESTADO.botonesEnvioDato();
        ADOutil Conec = new ADOutil();
        Conexion Cte = new Conexion();
        ArrayList Dato_Nuevo = new ArrayList();
        Socket Cliente_bascula = null;
        Envia_Dato Env = new Envia_Dato();
        Serial SR = new Serial();

        #region Inicializacion
        public UserLecturas()
        {
            InitializeComponent();
            
            FileStream fw = new FileStream(Variable.appPath + "\\recibe.ttt", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            fw.Close();
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
        #endregion


        void funcionWLABEL()
        {
            toolStrip31.Items.Remove(this.StripCarpetas);
//            toolStrip31.Items.Remove(this.StripCarpetas);
        }


        private void UserLecturas_Load(object sender, EventArgs e)
        {
            if (Variable.TypeScale == Variable.TipoBascula.WLABEL)
            {
                funcionWLABEL();
            }
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Vendedor' Puede moverla o quitarla según sea necesario.
            this.vendedorTableAdapter.Fill(this.baseDeDatosDataSet.Vendedor);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.carpeta_detalle' Puede moverla o quitarla según sea necesario.
            this.public_DetalleTableAdapter.Fill(this.baseDeDatosDataSet.Public_Detalle);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Oferta_Detalle' Puede moverla o quitarla según sea necesario.
            this.oferta_DetalleTableAdapter.Fill(this.baseDeDatosDataSet.Oferta_Detalle);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Oferta' Puede moverla o quitarla según sea necesario.
            this.ofertaTableAdapter.Fill(this.baseDeDatosDataSet.Oferta);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Prod_detalle' Puede moverla o quitarla según sea necesario.
            //this.prod_detalleTableAdapter.Fill(this.baseDeDatosDataSet.Prod_detalle);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Publicidad' Puede moverla o quitarla según sea necesario.
            this.publicidadTableAdapter.Fill(this.baseDeDatosDataSet.Publicidad);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Productos' Puede moverla o quitarla según sea necesario.
            this.productosTableAdapter.Fill(this.baseDeDatosDataSet.Productos);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Grupo' Puede moverla o quitarla según sea necesario.
            this.grupoTableAdapter.Fill(this.baseDeDatosDataSet.Grupo);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Bascula' Puede moverla o quitarla según sea necesario.
            this.basculaTableAdapter.Fill(this.baseDeDatosDataSet.Bascula);

            toolStripLabel3.Text = Nombre_Select;

            Asigna_Grupo();
            Asigna_Bascula();           
        }
             
        #region Botones del ToolStripMenu
        private void StripProductos_Click(object sender, EventArgs e)
        {
            DatosEnviado = ESTADO.botonesEnvioDato.SDPRODUCTO;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.listViewDatos.Visible = true;
            this.listViewDatos.BringToFront();
            this.listViewDatos.View = View.Details;
            this.listViewDatos.FullRowSelect = true;
            this.listViewDatos.GridLines = true;
            this.listViewDatos.LabelEdit = false;
            this.listViewDatos.HideSelection = false;
            this.listViewDatos.Clear();
            this.listViewDatos.Columns.Add(new ColHeader(Variable.SYS_MSJ[145, Variable.idioma], 80, HorizontalAlignment.Left,false));  //Codigeo            
            this.listViewDatos.Columns.Add(new ColHeader(Variable.SYS_MSJ[163, Variable.idioma], 200, HorizontalAlignment.Left,false));  //Nombre
            this.listViewDatos.Columns.Add(new ColHeader(Variable.SYS_MSJ[166, Variable.idioma], 80, HorizontalAlignment.Left,false));  //Precio
            this.listViewDatos.Columns.Add(new ColHeader(Variable.SYS_MSJ[170, Variable.idioma], 50, HorizontalAlignment.Left,false));   //TipoID
            this.listViewDatos.Columns.Add(new ColHeader(Variable.SYS_MSJ[162, Variable.idioma], 80, HorizontalAlignment.Left,false));  //No. PLU
            this.listViewDatos.Columns.Add(new ColHeader(Variable.SYS_MSJ[169, Variable.idioma], 50, HorizontalAlignment.Left,false));  // PrecioEditable
            this.listViewDatos.Columns.Add(new ColHeader(Variable.SYS_MSJ[143, Variable.idioma], 70, HorizontalAlignment.Left,false));  //caducidad
            this.listViewDatos.Columns.Add(new ColHeader(Variable.SYS_MSJ[148, Variable.idioma], 70, HorizontalAlignment.Left,false));   //Impuesto
            this.listViewDatos.Columns.Add(new ColHeader(Variable.SYS_MSJ[160, Variable.idioma], 70, HorizontalAlignment.Left,false));  //Multiplo
            this.listViewDatos.Columns.Add(new ColHeader(Variable.SYS_MSJ[149, Variable.idioma], 70, HorizontalAlignment.Left,false));  // Ingrediente
            this.listViewDatos.Columns.Add(new ColHeader(Variable.SYS_MSJ[168, Variable.idioma] +"1", 70, HorizontalAlignment.Left,false));  //Mensaje1
            this.listViewDatos.Columns.Add(new ColHeader(Variable.SYS_MSJ[168, Variable.idioma] + "2", 70, HorizontalAlignment.Left,false));  //Mensaje2
            this.listViewDatos.Columns.Add(new ColHeader(Variable.SYS_MSJ[168, Variable.idioma] + "3", 70, HorizontalAlignment.Left,false));  //Mensaje3
            this.listViewDatos.Columns.Add(new ColHeader(Variable.SYS_MSJ[168, Variable.idioma] + "4", 70, HorizontalAlignment.Left, false));  // Mensaje4   
            this.listViewDatos.Columns.Add(new ColHeader("Imagen", 200, HorizontalAlignment.Left, false));  // Imagen
            //Codigo,Nombre,Precio,TipoID,NoPlu,PrecioEditable,CaducidadDias,Impuesto,Mutiplo,id_ingrediente,publicidad1,publicidad2,publicidad3,publicidad4,imagen1
            StripEnviar();
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        private void StripPublicidad_Click(object sender, EventArgs e)
        {           
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            DatosEnviado = ESTADO.botonesEnvioDato.SDPUBLICIDAD;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.listViewDatos.Visible = true;
            this.listViewDatos.BringToFront();
            this.listViewDatos.View = View.Details;
            this.listViewDatos.FullRowSelect = true;
            this.listViewDatos.GridLines = true;
            this.listViewDatos.LabelEdit = false;
            this.listViewDatos.HideSelection = false;
            this.listViewDatos.Clear();
            this.listViewDatos.Columns.Add(new ColHeader( Variable.SYS_MSJ[145, Variable.idioma], 80, HorizontalAlignment.Left,false));  //Codigo
            this.listViewDatos.Columns.Add(new ColHeader( Variable.SYS_MSJ[174, Variable.idioma], 100, HorizontalAlignment.Left,false));  //Titulo
            this.listViewDatos.Columns.Add(new ColHeader( Variable.SYS_MSJ[146, Variable.idioma], 300, HorizontalAlignment.Left,false));  //Descripcion
            //id_publicidad,Titulo,Mensaje
            StripEnviar();

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        private void StripOferta_Click(object sender, EventArgs e)
        {           
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            DatosEnviado = ESTADO.botonesEnvioDato.SDOFERTA;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.listViewDatos.Visible = true;
            this.listViewDatos.BringToFront();
            this.listViewDatos.View = View.Details;
            this.listViewDatos.FullRowSelect = true;
            this.listViewDatos.GridLines = true;
            this.listViewDatos.LabelEdit = false;
            this.listViewDatos.HideSelection = false;
            this.listViewDatos.Clear();
            this.listViewDatos.Columns.Add(new ColHeader(Variable.SYS_MSJ[145, Variable.idioma], 80, HorizontalAlignment.Left,false));  //Codigo
            this.listViewDatos.Columns.Add(new ColHeader(Variable.SYS_MSJ[163, Variable.idioma], 200, HorizontalAlignment.Left,false));  //Nombre
            this.listViewDatos.Columns.Add(new ColHeader(Variable.SYS_MSJ[151, Variable.idioma], 100, HorizontalAlignment.Left,false));  //Inicio
            this.listViewDatos.Columns.Add(new ColHeader(Variable.SYS_MSJ[172, Variable.idioma], 100, HorizontalAlignment.Left,false));  //Termino
            this.listViewDatos.Columns.Add(new ColHeader(Variable.SYS_MSJ[173, Variable.idioma], 50, HorizontalAlignment.Left,false));  //Tipo
            this.listViewDatos.Columns.Add(new ColHeader(Variable.SYS_MSJ[147, Variable.idioma], 100, HorizontalAlignment.Left, false));  //Descuento
            //id_oferta,nombre,fecha_inicio,fecha_fin,tipo_desc,Descuento
            StripEnviar();

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        private void StripVendedores_Click(object sender, EventArgs e)
        {           
            DatosEnviado = ESTADO.botonesEnvioDato.SDVENDEDOR;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            this.listViewDatos.Visible = true;
            this.listViewDatos.BringToFront();
            this.listViewDatos.View = View.Details;
            this.listViewDatos.FullRowSelect = true;
            this.listViewDatos.GridLines = true;
            this.listViewDatos.LabelEdit = false;
            this.listViewDatos.HideSelection = false;
            this.listViewDatos.Clear();
            this.listViewDatos.Columns.Add(new ColHeader( Variable.SYS_MSJ[145, Variable.idioma], 80, HorizontalAlignment.Left,false));  //Codigo
            this.listViewDatos.Columns.Add(new ColHeader( Variable.SYS_MSJ[163, Variable.idioma], 200, HorizontalAlignment.Left,false));  //Nombre
            //id_vendedor,Nombre
            StripEnviar();

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        private void StripIngrediente_Click(object sender, EventArgs e)
        {           
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            DatosEnviado = ESTADO.botonesEnvioDato.SDINGREDIENTE;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.listViewDatos.Visible = true;
            this.listViewDatos.BringToFront();
            this.listViewDatos.View = View.Details;
            this.listViewDatos.FullRowSelect = true;
            this.listViewDatos.GridLines = true;
            this.listViewDatos.LabelEdit = false;
            this.listViewDatos.HideSelection = false;
            this.listViewDatos.Clear();
            this.listViewDatos.Columns.Add(new ColHeader( Variable.SYS_MSJ[145, Variable.idioma], 80, HorizontalAlignment.Left,false));  //Codigo
            this.listViewDatos.Columns.Add(new ColHeader( Variable.SYS_MSJ[163, Variable.idioma], 200, HorizontalAlignment.Left,false)); //Nombre
            this.listViewDatos.Columns.Add(new ColHeader( Variable.SYS_MSJ[150, Variable.idioma], 300, HorizontalAlignment.Left,false));  //Informacion
            //id_ingrediente,Nombre,Informacion   
            StripEnviar();

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        private void StripCarpetas_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            DatosEnviado = ESTADO.botonesEnvioDato.SDCARPETA;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.listViewDatos.Visible = true;
            this.listViewDatos.BringToFront();
            this.listViewDatos.View = View.Details;
            this.listViewDatos.FullRowSelect = true;
            this.listViewDatos.GridLines = true;
            this.listViewDatos.LabelEdit = false;
            this.listViewDatos.HideSelection = false;
            this.listViewDatos.Clear();
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[145, Variable.idioma], 80, HorizontalAlignment.Left);  //Codigo  id
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[163, Variable.idioma], 150, HorizontalAlignment.Left); //Nombre id_padre
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[150, Variable.idioma], 300, HorizontalAlignment.Left);  //Informacion decripcion
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[150, Variable.idioma], 250, HorizontalAlignment.Left);  //Informacion imagen

            StripEnviar();

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void StripEnviar()
        {
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
                        myCurrent.ip = myScale[pos].ip;
                        myCurrent.idbas = myScale[pos].idbas;
                        myCurrent.gpo = myScale[pos].gpo;
                        myCurrent.nombre = myScale[pos].nombre;
                        myCurrent.Nserie = myScale[pos].nserie;
                        myCurrent.tipo = myScale[pos].tipo;
                        myCurrent.BAUD = myScale[pos].baud;
                        myCurrent.COMM = myScale[pos].pto;
                        BasculasActualizadas++;

                        if (LeerDatos_Bascula((int)DatosEnviado, myCurrent.ip, myCurrent.tipo, myCurrent.Nserie)) //;Variable.IP_Address);  //, Num_gpo, Num_basc);
                        {
                            BasculasActualizadas--;
                            if (MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma] + " "
                                + myScale[pos].nserie + " " + Variable.SYS_MSJ[417, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma],
                                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No) break;
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
                        myCurrent.ip = myScale[pos].ip;
                        myCurrent.idbas = myScale[pos].idbas;
                        myCurrent.gpo = myScale[pos].gpo;
                        myCurrent.nombre = myScale[pos].nombre;
                        myCurrent.Nserie = myScale[pos].nserie;
                        myCurrent.tipo = myScale[pos].tipo;
                        myCurrent.BAUD = myScale[pos].baud;
                        myCurrent.COMM = myScale[pos].pto;
                        BasculasActualizadas++;

                        if (LeerDatos_Bascula((int)DatosEnviado, myCurrent.ip, myCurrent.tipo, myCurrent.Nserie)) //Variable.IP_Address);  //, Num_gpo, Num_basc);
                        {
                            BasculasActualizadas--;
                            MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma]
                                + " " + myScale[pos].nserie + ".", Variable.SYS_MSJ[42, Variable.idioma],
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            break;
                        }
                        
                        break;
                    }
                }
            }

            MessageBox.Show(this, Variable.SYS_MSJ[420, Variable.idioma] + " " + BasculasActualizadas + " "
                + Variable.SYS_MSJ[419, Variable.idioma] + " " + NumeroDeBaculas,
                Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region Lectura de informacion de basculas

        private bool LeerDatos_Bascula(int Info_A_Enviar, string direccionIP,int tipo_scale,string NumSerie)
        {
            Cursor.Current = Cursors.WaitCursor;
            listViewDatos.Items.Clear();
            bool err = false;

            switch (Info_A_Enviar)
            {
                case (int)ESTADO.botonesEnvioDato.SDPRODUCTO:
                    {
                        if (tipo_scale == (int)ESTADO.tipoConexionesEnum.PKWIFI) { err = bLeer_productos(direccionIP, NumSerie); }
                        else { err = bLeer_productos(NumSerie); }
                    }
                    break;

                case (int)ESTADO.botonesEnvioDato.SDVENDEDOR:
                    {
                        if (tipo_scale == (int)ESTADO.tipoConexionesEnum.PKWIFI) { err = bLeer_vendedor(direccionIP, NumSerie); }
                        else { err = bLeer_vendedor(NumSerie); }
                    }
                    break;               
                case (int)ESTADO.botonesEnvioDato.SDPUBLICIDAD:
                    {
                        if (tipo_scale == (int)ESTADO.tipoConexionesEnum.PKWIFI) { err = bLeer_publicidad(direccionIP, NumSerie); }
                        else { err = bLeer_publicidad(NumSerie); }
                    }
                    break;
                case (int)ESTADO.botonesEnvioDato.SDINGREDIENTE:
                    {
                        if (tipo_scale == (int)ESTADO.tipoConexionesEnum.PKWIFI) { err = bLeer_infoAdicional(direccionIP, NumSerie); }
                        else { err = bLeer_inforAdicional(NumSerie); }
                    }
                    break;
                case (int)ESTADO.botonesEnvioDato.SDOFERTA:
                    {
                        if (tipo_scale == (int)ESTADO.tipoConexionesEnum.PKWIFI) { err = bLeer_ofertas(direccionIP, NumSerie); }
                        else { err = bLeer_ofertas(NumSerie); }
                    }
                    break;
                case (int)ESTADO.botonesEnvioDato.SDCARPETA:
                    {
                        if (tipo_scale == (int)ESTADO.tipoConexionesEnum.PKWIFI) { err = bLeer_carpetas(direccionIP, NumSerie); }
                        else { err = bLeer_carpetas(NumSerie); }
                    }
                    break;
            }            
            Cursor.Current = Cursors.Default;

            return err;
        }        

        private bool bLeer_productos(string direccionIP,string NumSerie)
        {
            bool Msg_Recibido;           
            string strcomando;
            bool ERROR = false;
            
            string Archivo = Variable.appPath + "\\PLU_" + NumSerie + ".txt";

            FileStream fi = new FileStream(Archivo, FileMode.Create, FileAccess.Write);
            StreamWriter sr = new StreamWriter(fi, System.Text.UnicodeEncoding.Unicode);

            Cliente_bascula = Cte.conectar(direccionIP, 50036);  //, Variable.frame, ref Dato_Recivido);
            if (Cliente_bascula != null)
            {
                string sComando = "XX" + (char)9 + (char)10;

                string msg_respuesta = Env.Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");
                if (msg_respuesta != null)
                {
                    WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[357, Variable.idioma] + myCurrent.Nserie + "...");
                    Thread t = new Thread(workerObject.vShowMsg);
                    t.Start();

                    strcomando = "LPXX" + (char)9 + (char)10;
                    Msg_Recibido = Command_Recibido(Archivo, strcomando, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo, ref sr);

                    workerObject.vEndShowMsg();
                }
                else
                {
                    ERROR = true;
                }
                Cte.desconectar(ref Cliente_bascula);
                Cursor.Current = Cursors.Default;

                sr.Close();
                fi.Close();                
               
            }
            else
            {
                ERROR = true;
                sr.Close();
                fi.Close(); 
            }

            return ERROR;
        }

        private bool bLeer_productos(string NumSerie)
        {
            bool Msg_Recibido;
            string strcomando;         
            string Archivo = Variable.appPath + "\\PLU_" + NumSerie + ".txt";
            bool ERROR = false;

            FileStream fi = new FileStream(Archivo, FileMode.Create, FileAccess.Write);
            StreamWriter sr = new StreamWriter(fi, System.Text.UnicodeEncoding.Unicode);
            
            serialPort1 = new SerialPort();

            if (SR.OpenPort(ref serialPort1, myCurrent.COMM,myCurrent.BAUD))  // Variable.P_COMM, Variable.Buad))
            {
                SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)9 + (char)10);
                SR.ReceivedCOMSerial(ref serialPort1);

                WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[357, Variable.idioma] + myCurrent.Nserie + "...");
                Thread t = new Thread(workerObject.vShowMsg);
                t.Start();

                strcomando = "LPXX" + (char)9 + (char)10;
                Msg_Recibido = Command_Recibido(Archivo, strcomando, ref serialPort1, Num_Bascula, Num_Grupo, ref sr);

                SR.SendCOMSerial(ref serialPort1, "DXXX" + (char)9 + (char)10);
                SR.ReceivedCOMSerial(ref serialPort1);

                workerObject.vEndShowMsg();

                SR.ClosePort(ref serialPort1);
          
                Cursor.Current = Cursors.Default;
                sr.Close();
                fi.Close();
            }
            else
            {
                ERROR = true;
                sr.Close();
                fi.Close();
            }

            return ERROR;
        }

        private bool bLeer_vendedor(string direccionIP, string NumSerie)
        {            
            bool Msg_Recibido;
            string strcomando;
            string Archivo = Variable.appPath + "\\VEND_" + NumSerie + ".txt";
            bool ERROR = false;

            FileStream fi = new FileStream(Archivo, FileMode.Create, FileAccess.Write);
            StreamWriter sr = new StreamWriter(fi, System.Text.UnicodeEncoding.Unicode);

            Cliente_bascula = Cte.conectar(direccionIP, 50036);
            if (Cliente_bascula != null)
            {
                string sComando = "XX" + (char)9 + (char)10;

                string msg_respuesta = Env.Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");
                if (msg_respuesta != null)
                {
                    WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[358, Variable.idioma] + myCurrent.Nserie + "...");
                    Thread t = new Thread(workerObject.vShowMsg);
                    t.Start();

                    strcomando = "LVXX" + (char)9 + (char)10;
                    Msg_Recibido = Command_Recibido(Archivo, strcomando, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo, ref sr);

                    workerObject.vEndShowMsg();
                }
                else
                {
                    ERROR = true;
                }

                Cte.desconectar(ref Cliente_bascula);
                Cursor.Current = Cursors.Default;

                sr.Close();
                fi.Close();
                
            }
            else
            {
                ERROR = true;
                sr.Close();
                fi.Close();
            }

            return ERROR;
        }

        private bool bLeer_vendedor(string NumSerie)
        {
            bool Msg_Recibido;
            string strcomando;           
            string Archivo = Variable.appPath + "\\VEND_" + NumSerie + ".txt";
            bool ERROR = false;

            FileStream fi = new FileStream(Archivo, FileMode.Create, FileAccess.Write);
            StreamWriter sr = new StreamWriter(fi, System.Text.UnicodeEncoding.Unicode);

            serialPort1 = new SerialPort();

            if (SR.OpenPort(ref serialPort1, myCurrent.COMM,myCurrent.BAUD))  //Variable.P_COMM, Variable.Buad))
            {                
                SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)9 + (char)10);
                SR.ReceivedCOMSerial(ref serialPort1);

                WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[358, Variable.idioma] + myCurrent.Nserie + "...");
                Thread t = new Thread(workerObject.vShowMsg);
                t.Start();

                strcomando = "LVXX" + (char)9 + (char)10;
                Msg_Recibido = Command_Recibido(Archivo, strcomando, ref serialPort1, Num_Bascula, Num_Grupo, ref sr);

                SR.SendCOMSerial(ref serialPort1, "DXXX" + (char)9 + (char)10);
                SR.ReceivedCOMSerial(ref serialPort1);

                workerObject.vEndShowMsg();
                
                Cursor.Current = Cursors.Default;

                SR.ClosePort(ref serialPort1);                
                sr.Close();
                fi.Close();
        
            }
            else
            {
                ERROR = true;
                sr.Close();
                fi.Close();
            }

            return ERROR;
        }       

        private bool bLeer_publicidad(string direccionIP, string NumSerie)
        {
            bool Msg_Recibido;
            string strcomando;            
            string Archivo = Variable.appPath + "\\MSG_" + NumSerie + ".txt";
            bool ERROR = false;

            FileStream fi = new FileStream(Archivo, FileMode.Create, FileAccess.Write);
            StreamWriter sr = new StreamWriter(fi, System.Text.UnicodeEncoding.Unicode);

            Cliente_bascula = Cte.conectar(direccionIP, 50036);
            if (Cliente_bascula != null)
            {
                string sComando = "XX" + (char)9 + (char)10;

                string msg_respuesta = Env.Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");
                if (msg_respuesta != null)
                {
                    WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[359, Variable.idioma] + myCurrent.Nserie + "...");
                    Thread t = new Thread(workerObject.vShowMsg);
                    t.Start();

                    strcomando = "LMXX" + (char)9 + (char)10;
                    Msg_Recibido = Command_Recibido(Archivo, strcomando, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo, ref sr);

                    workerObject.vEndShowMsg();
                }
                else
                {
                    ERROR = true;
                }

                Cte.desconectar(ref Cliente_bascula);
                Cursor.Current = Cursors.Default;
                
                sr.Close();
                fi.Close();
 
            }
            else
            {
                ERROR = true;
                sr.Close();
                fi.Close();
            }

            return ERROR;
        }

        private bool bLeer_publicidad(string NumSerie)
        {
            bool Msg_Recibido;            
            string strcomando;                      
            string Archivo = Variable.appPath + "\\MSG_" + NumSerie + ".txt";
            bool ERROR = false;

            FileStream fi = new FileStream(Archivo, FileMode.Create, FileAccess.Write);
            StreamWriter sr = new StreamWriter(fi, System.Text.UnicodeEncoding.Unicode);
            serialPort1 = new SerialPort();
            if (SR.OpenPort(ref serialPort1, myCurrent.COMM,myCurrent.BAUD))  //Variable.P_COMM, Variable.Buad))
            {
                SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)9 + (char)10);
                SR.ReceivedCOMSerial(ref serialPort1);

                WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[359, Variable.idioma] + myCurrent.Nserie + "...");
                Thread t = new Thread(workerObject.vShowMsg);
                t.Start();

                strcomando = "LMXX" + (char)9 + (char)10;
                Msg_Recibido = Command_Recibido(Archivo, strcomando, ref serialPort1, Num_Bascula, Num_Grupo,ref sr);

                SR.SendCOMSerial(ref serialPort1, "DXXX" + (char)9 + (char)10);
                SR.ReceivedCOMSerial(ref serialPort1);

                workerObject.vEndShowMsg();

                SR.ClosePort(ref serialPort1);
                
                Cursor.Current = Cursors.Default;
                sr.Close();
                fi.Close();
            }
            else
            {
                ERROR = true;
                sr.Close();
                fi.Close();
            }

            return ERROR;
        }
        
        private bool bLeer_infoAdicional(string direccionIP,string NumSerie)
        {
            bool Msg_Recibido;
            string msg_respuesta;
            string strcomando;          
            string Archivo = Variable.appPath + "\\INFO_" + NumSerie + ".txt";
            bool ERROR = false;

            FileStream fi = new FileStream(Archivo, FileMode.Create, FileAccess.Write);
            StreamWriter sr = new StreamWriter(fi, System.Text.UnicodeEncoding.Unicode);
            
            Cliente_bascula = Cte.conectar(direccionIP, 50036);
            if (Cliente_bascula != null)
            {
                string sComando = "XX" + (char)9 + (char)10;

                msg_respuesta = Env.Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");

                if (msg_respuesta != null)
                {
                    WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[360, Variable.idioma] + myCurrent.Nserie + "...");
                    Thread t = new Thread(workerObject.vShowMsg);
                    t.Start();

                    strcomando = "LIXX" + (char)9 + (char)10;
                    Msg_Recibido = Command_Recibido(Archivo, strcomando, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo, ref sr);

                    workerObject.vEndShowMsg();
                }
                else
                {
                    ERROR = true;
                }
                Cte.desconectar(ref Cliente_bascula);
                Cursor.Current = Cursors.Default;

                sr.Close();
                fi.Close();
               
            }
            else
            {
                ERROR = true;
                sr.Close();
                fi.Close();
            }

            return ERROR;
            
        }

        private bool bLeer_inforAdicional(string NumSerie)
        {
            bool Msg_Recibido;            
            string strcomando;          
            string Archivo = Variable.appPath + "\\INFO_" + NumSerie + ".txt";
            bool ERROR = false;

            FileStream fi = new FileStream(Archivo, FileMode.Create, FileAccess.Write);
            StreamWriter sr = new StreamWriter(fi, System.Text.UnicodeEncoding.Unicode);
            serialPort1 = new SerialPort();
            if (SR.OpenPort(ref serialPort1, myCurrent.COMM,myCurrent.BAUD)) //Variable.P_COMM, Variable.Buad))
            {
                SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)9 + (char)10);
                SR.ReceivedCOMSerial(ref serialPort1);

                WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[360, Variable.idioma] + myCurrent.Nserie + "...");
                Thread t = new Thread(workerObject.vShowMsg);
                t.Start();

                strcomando = "LIXX" + (char)9 + (char)10;
                Msg_Recibido = Command_Recibido(Archivo, strcomando, ref serialPort1, Num_Bascula, Num_Grupo,ref sr);

                SR.SendCOMSerial(ref serialPort1, "DXXX" + (char)9 + (char)10);
                SR.ReceivedCOMSerial(ref serialPort1);

                SR.ClosePort(ref serialPort1);

                workerObject.vEndShowMsg();
                
                Cursor.Current = Cursors.Default;
                sr.Close();
                fi.Close();
            }
            else
            {
                ERROR = true;
                sr.Close();
                fi.Close();
            }

            return ERROR;
        }
      
        private bool bLeer_ofertas(string direccionIP, string NumSerie)
        {
            bool Msg_Recibido;
            string strcomando;           
            string Archivo = Variable.appPath + "\\OFER_" + NumSerie + ".txt";
            bool ERROR = false;

            FileStream fi = new FileStream(Archivo, FileMode.Create, FileAccess.Write);
            StreamWriter sr = new StreamWriter(fi, System.Text.UnicodeEncoding.Unicode);

            Cliente_bascula = Cte.conectar(direccionIP, 50036);
            if (Cliente_bascula != null)
            {
                string sComando = "XX" + (char)9 + (char)10;

                string msg_respuesta = Env.Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");
                if (msg_respuesta != null)
                {
                    WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[361, Variable.idioma] + myCurrent.Nserie + "...");
                    Thread t = new Thread(workerObject.vShowMsg);
                    t.Start();

                    strcomando = "LOXX" + (char)9 + (char)10;
                    Msg_Recibido = Command_Recibido(Archivo, strcomando, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo, ref sr);

                    workerObject.vEndShowMsg();
                }
                else
                {
                    ERROR = true;
                }

                Cte.desconectar(ref Cliente_bascula);
                Cursor.Current = Cursors.Default;

                sr.Close();
                fi.Close();

            }
            else
            {
                ERROR = true;
                sr.Close();
                fi.Close();
            }

            return ERROR;
        }

        private bool bLeer_ofertas(string NumSerie)
        {
            bool Msg_Recibido;            
            string strcomando;          
            string Archivo = Variable.appPath + "\\OFER_" + NumSerie + ".txt";
            bool ERROR = false;

            FileStream fi = new FileStream(Archivo, FileMode.Create, FileAccess.Write);
            StreamWriter sr = new StreamWriter(fi, System.Text.UnicodeEncoding.Unicode);
            serialPort1 = new SerialPort();
            if (SR.OpenPort(ref serialPort1, myCurrent.COMM,myCurrent.BAUD))  //Variable.P_COMM, Variable.Buad))
            {
                SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)9 + (char)10);
                SR.ReceivedCOMSerial(ref serialPort1);

                WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[361, Variable.idioma] + myCurrent.Nserie + "...");
                Thread t = new Thread(workerObject.vShowMsg);
                t.Start();

                strcomando = "LOXX" + (char)9 + (char)10;
                Msg_Recibido = Command_Recibido(Archivo, strcomando, ref serialPort1, Num_Bascula, Num_Grupo,ref sr);

                SR.SendCOMSerial(ref serialPort1, "DXXX" + (char)9 + (char)10);
                SR.ReceivedCOMSerial(ref serialPort1);

                SR.ClosePort(ref serialPort1);

                workerObject.vEndShowMsg();

                Cursor.Current = Cursors.Default;
                sr.Close();
                fi.Close();
                
            }
            else
            {
                ERROR = true;
                sr.Close();
                fi.Close();
            }

            return ERROR;
        }

        private bool bLeer_carpetas(string direccionIP, string NumSerie)
        {
            bool Msg_Recibido;
            string strcomando;          
            string Archivo = Variable.appPath + "\\CARP_" + NumSerie + ".txt";
            bool ERROR = false;

            FileStream fi = new FileStream(Archivo, FileMode.Create, FileAccess.Write);
            StreamWriter sr = new StreamWriter(fi, System.Text.UnicodeEncoding.Unicode);

            Cliente_bascula = Cte.conectar(direccionIP, 50036);
            if (Cliente_bascula != null)
            {
                string sComando = "XX" + (char)9 + (char)10;

                string msg_respuesta = Env.Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");
                if (msg_respuesta != null)
                {

                    WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[361, Variable.idioma] + myCurrent.Nserie + "...");
                    Thread t = new Thread(workerObject.vShowMsg);
                    t.Start();

                    strcomando = "LCXX" + (char)9 + (char)10;
                    Msg_Recibido = Command_Recibido(Archivo, strcomando, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo, ref sr);

                    workerObject.vEndShowMsg();
                }
                else
                {
                    ERROR = true;
                }
                Cte.desconectar(ref Cliente_bascula);
                Cursor.Current = Cursors.Default;
                
                sr.Close();
                fi.Close();
            }
            else
            {
                ERROR = true;
                sr.Close();
                fi.Close();
            }

            return ERROR;
        }

        private bool bLeer_carpetas(string NumSerie)
        {
            bool Msg_Recibido;
            string strcomando;          
            string Archivo = Variable.appPath + "\\CARP_" + NumSerie + ".txt";
            bool ERROR = false;

            FileStream fi = new FileStream(Archivo, FileMode.Create, FileAccess.Write);
            StreamWriter sr = new StreamWriter(fi, System.Text.UnicodeEncoding.Unicode);
            serialPort1 = new SerialPort();

            if (SR.OpenPort(ref serialPort1, myCurrent.COMM,myCurrent.BAUD))  //Variable.P_COMM, Variable.Buad))
            {
                SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)9 + (char)10);
                SR.ReceivedCOMSerial(ref serialPort1);

                WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[361, Variable.idioma] + myCurrent.Nserie + "...");
                Thread t = new Thread(workerObject.vShowMsg);
                t.Start();

                strcomando = "LCXX" + (char)9 + (char)10;
                Msg_Recibido = Command_Recibido(Archivo, strcomando, ref serialPort1, Num_Bascula, Num_Grupo, ref sr);

                SR.SendCOMSerial(ref serialPort1, "DXXX" + (char)9 + (char)10);
                SR.ReceivedCOMSerial(ref serialPort1);

                SR.ClosePort(ref serialPort1);

                workerObject.vEndShowMsg();
                
                Cursor.Current = Cursors.Default;

                sr.Close();
                fi.Close();
            }
            else
            {
                ERROR = true;
                sr.Close();
                fi.Close();
            }

            return ERROR;
        }
        
        public bool Command_Recibido(string Archivo, string comando, string direccionIP, ref Socket Cliente_bascula, long bascula, long grupo, ref StreamWriter sr)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string reg_enviado, strcomando;
            string reg_recibido;
            int nregistro;
            bool continuar = true;
            bool Ocupado = false;
            strcomando = comando;
            
            reg_enviado = Cte.Recibir_Respuesta(ref Cliente_bascula, direccionIP, strcomando);

            while (continuar && reg_enviado != "Null")
            {
                if (reg_enviado.IndexOf("Error") >= 0)
                {
                    Env.Guardar_Trama_pendiente(bascula, grupo, strcomando);
                    continuar = false;
                }
                else
                {
                    if (reg_enviado.Length > 0)
                    {
                        comando = reg_enviado.Substring(0, 2);
                        nregistro = Convert.ToInt16(reg_enviado.Substring(2, 2));

                        reg_recibido = reg_enviado.Substring(4);
                        if (comando == "LP" && nregistro > 0) Mostrar_Productos_Recibidos(reg_recibido.Split(chr), ref sr);
                        if (comando == "LI" && nregistro > 0) Mostrar_InfoAdd_Recibidos(reg_recibido.Split(chr), ref sr);
                        if (comando == "LM" && nregistro > 0) Mostrar_Publicidad_Recibidos(reg_recibido.Split(chr), ref sr);
                        if (comando == "LO" && nregistro > 0) Mostrar_Ofertas_Recibidos(reg_recibido.Split(chr), ref sr);
                        if (comando == "LV" && nregistro > 0) Mostrar_Vendedores_Recibidos(reg_recibido.Split(chr), ref sr);
                        if (comando == "LC" && nregistro > 0) Mostrar_Carpetas_Recibidos(reg_recibido.Split(chr), ref sr);
                       // if (comando == "LA" && nregistro > 0) Mostrar_Asociaciones_Recibidos(reg_recibido.Split(chr), ref pro, ref sr); 
                    }
                    if (reg_enviado.IndexOf("End") > 0) continuar = false;
                    else
                    {
                        continuar = true;
                        reg_enviado = Cte.Recibir_Respuesta(ref Cliente_bascula, direccionIP, comando + "XXOk" + (char)9 + (char)10);
                    }
                }
            }
            if (reg_enviado == "Null") MessageBox.Show(this, Variable.SYS_MSJ[194, Variable.idioma] + " " + Nombre_Select + " " + Variable.SYS_MSJ[195, Variable.idioma] + " " + direccionIP + " " + Variable.SYS_MSJ[196, Variable.idioma] + ". " + Variable.SYS_MSJ[236, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Ocupado = false;
            return Ocupado;
        }
        public bool Command_Recibido(string Archivo, string strcomando, ref SerialPort Cliente_bascula, long bascula, long grupo, ref StreamWriter sr)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Dato_Recibido;
            string reg_recibido;
            string comando = "";
            int nregistro;
            bool continuar = true;
            bool Ocupado = false;

            SR.SendCOMSerial(ref Cliente_bascula, strcomando);

            while (continuar)
            {
                Dato_Recibido = SR.ReceivedCOMSerial(ref Cliente_bascula);

                if (Dato_Recibido.IndexOf("Error") >= 0)
                {
                    Env.Guardar_Trama_pendiente(bascula, grupo, strcomando);
                    continuar = false;
                }
                else
                {
                    if (Dato_Recibido.Length > 0)
                    {
                        comando = Dato_Recibido.Substring(0, 2);
                        nregistro = Convert.ToInt16(Dato_Recibido.Substring(2, 2));
                        reg_recibido = Dato_Recibido.Substring(4);

                        if (comando == "LP" && nregistro > 0) Mostrar_Productos_Recibidos(reg_recibido.Split(chr), ref sr);
                        if (comando == "LI" && nregistro > 0) Mostrar_InfoAdd_Recibidos(reg_recibido.Split(chr), ref sr);
                        if (comando == "LM" && nregistro > 0) Mostrar_Publicidad_Recibidos(reg_recibido.Split(chr), ref sr);
                        if (comando == "LO" && nregistro > 0) Mostrar_Ofertas_Recibidos(reg_recibido.Split(chr), ref sr);
                        if (comando == "LV" && nregistro > 0) Mostrar_Vendedores_Recibidos(reg_recibido.Split(chr), ref sr);
                        if (comando == "LC" && nregistro > 0) Mostrar_Carpetas_Recibidos(reg_recibido.Split(chr), ref sr);
                        //if (comando == "LA" && nregistro > 0) Mostrar_Asociaciones_Recibidos(reg_recibido.Split(chr), ref pro, ref sr); 
                    }
                    if (Dato_Recibido.IndexOf("End") > 0) continuar = false;
                    else
                    {
                        continuar = true;
                        SR.SendCOMSerial(ref Cliente_bascula, comando + "XXOk" + (char)9 + (char)10);
                    }
                }
            }
            
            Ocupado = false;

            return Ocupado;
        }
        #endregion

        #region funciones para mostrar los datos leidos
        private void Mostrar_Productos_Recibidos(string[] Trama,  ref StreamWriter sr)
        {
            string[] Trama_Recibida = null;
            Double impuesto = 0;
            for (int i = 0; i < Trama.Length-3; i++)
            {
                Trama_Recibida = Trama[i].Split((char)9);
                impuesto = Convert.ToDouble(Trama_Recibida[9]) * 100;
                //0   1       2         3               4                   5                       6   7   8       9   10  11          12              13  14  15  16  17  18                 
                //1	4403	4403	AGUJA NORTEA K.	  8.43	images/user/CarneRes/AgujaNortena.jpg	1	0	8	  0.00	0	2	2013-08-06 16:39:00	  0.00	0	5	6	7	8	
                ListViewItem lwitem = new ListViewItem(Trama_Recibida[1]);  //0   codigo                
                lwitem.SubItems.Add(Trama_Recibida[3]);  //1 Nombre
                lwitem.SubItems.Add(Trama_Recibida[4]);  //2 Precio
                lwitem.SubItems.Add(Trama_Recibida[6]); //3  tipo ID
                lwitem.SubItems.Add(Trama_Recibida[2]); //1  Num. PLU               
                lwitem.SubItems.Add(Trama_Recibida[7]); //11 PRECIO EDITABLE
                lwitem.SubItems.Add(Trama_Recibida[8]); //12 caducidad
                lwitem.SubItems.Add(impuesto.ToString());  //9 impuesto           
                lwitem.SubItems.Add(Trama_Recibida[10]);  //12 MULTIPLO
                lwitem.SubItems.Add(Trama_Recibida[14]); //13  ingredientes
                lwitem.SubItems.Add(Trama_Recibida[15]);  //4  Mensaje1
                lwitem.SubItems.Add(Trama_Recibida[16]);  //5  Mensaje 2
                lwitem.SubItems.Add(Trama_Recibida[17]);  //6  Mensaje 3
                lwitem.SubItems.Add(Trama_Recibida[18]);  //7 Mensaje 4
                lwitem.SubItems.Add(Trama_Recibida[5]); //14  imagen                     
                this.listViewDatos.Items.Add(lwitem);

                sr.WriteLine(Trama_Recibida[1] + (char)9 + Trama_Recibida[3] + (char)9 + Trama_Recibida[4] + (char)9 + Trama_Recibida[6] + (char)9 +
                Trama_Recibida[2] + (char)9 + Trama_Recibida[7] + (char)9 + Trama_Recibida[8] + (char)9 + impuesto.ToString() + (char)9 +
                Trama_Recibida[10] + (char)9 + Trama_Recibida[14] + (char)9 + Trama_Recibida[15] + (char)9 + Trama_Recibida[16] + (char)9 +
                Trama_Recibida[17] + (char)9 + Trama_Recibida[18] + (char)9 + Trama_Recibida[5]);
            }
        }
        private void Mostrar_InfoAdd_Recibidos(string[] Trama, ref StreamWriter sr)
        {
            string[] Trama_Recibida = null;
            
            for (int i = 0; i < Trama.Length-3; i++)
            {
                Trama_Recibida = Trama[i].Split((char)9);                

                ListViewItem lwitem = new ListViewItem(Trama_Recibida[0]);  //0  id_ingrediente
                lwitem.SubItems.Add(Trama_Recibida[1]); //1 nombre
                lwitem.SubItems.Add(Trama_Recibida[2]);  //2 informacion
                this.listViewDatos.Items.Add(lwitem);

                sr.WriteLine(Trama_Recibida[0] + (char)9 + Trama_Recibida[1] + (char)9 + Trama_Recibida[2]);
            }
        }
        private void Mostrar_Publicidad_Recibidos(string[] Trama, ref StreamWriter sr)
        {
            string[] Trama_Recibida = null;
            
            for (int i = 0; i < Trama.Length-3; i++)
            {
                Trama_Recibida = Trama[i].Split((char)9);
               
                ListViewItem lwitem = new ListViewItem(Trama_Recibida[0]);  //0 id_publicidad
                lwitem.SubItems.Add(Trama_Recibida[1]); //1 titulo
                lwitem.SubItems.Add(Trama_Recibida[2]);  //2 mensaje
                this.listViewDatos.Items.Add(lwitem);

                sr.WriteLine(Trama_Recibida[0] + (char)9 + Trama_Recibida[1] + (char)9 + Trama_Recibida[2]);
            }
        }
        private void Mostrar_Ofertas_Recibidos(string[] Trama, ref StreamWriter sr)
        {
            string[] Trama_Recibida = null;
            
            for (int i = 0; i < Trama.Length-3; i++)
            {
                Trama_Recibida = Trama[i].Split((char)9);
              
                ListViewItem lwitem = new ListViewItem(Trama_Recibida[0]);  //0  id_oferta
                lwitem.SubItems.Add(Trama_Recibida[1]); //1 Nombre
                lwitem.SubItems.Add(Trama_Recibida[2]);  //2 fecha de inicio
                lwitem.SubItems.Add(Trama_Recibida[3]);  //3 fecha de termino
                lwitem.SubItems.Add(Trama_Recibida[4]);  //4 tipo descuanto
                lwitem.SubItems.Add(Trama_Recibida[5]);  //5 descuento                
                this.listViewDatos.Items.Add(lwitem);

                sr.WriteLine(Trama_Recibida[0] + (char)9 + Trama_Recibida[1] + (char)9 + Trama_Recibida[2] + (char)9 +
                             Trama_Recibida[3] + (char)9 + Trama_Recibida[5]);
            }
        }
        private void Mostrar_Vendedores_Recibidos(string[] Trama, ref StreamWriter sr)
        {
            string[] Trama_Recibida = null;
            
            for (int i = 0; i < Trama.Length-3; i++)
            {
                Trama_Recibida = Trama[i].Split((char)9);
             
                ListViewItem lwitem = new ListViewItem(Trama_Recibida[0]);  //0
                lwitem.SubItems.Add(Trama_Recibida[1]); //1 
               /* lwitem.SubItems.Add(Trama_Recibida[2]);  //2
                lwitem.SubItems.Add(Trama_Recibida[3]);  //3
                lwitem.SubItems.Add(Trama_Recibida[4]);  //4
                lwitem.SubItems.Add(Trama_Recibida[5]);  //5
                lwitem.SubItems.Add(Trama_Recibida[6]);  //6*/
                this.listViewDatos.Items.Add(lwitem);

                sr.WriteLine(Trama_Recibida[0] + (char)9 + Trama_Recibida[1]);
            }
        }
        private void Mostrar_Carpetas_Recibidos(string[] Trama, ref StreamWriter sr)
        {
            string[] Trama_Recibida = null;
            
            for (int i = 0; i < Trama.Length-3; i++)
            {
                Trama_Recibida = Trama[i].Split((char)9);
                sr.WriteLine(Trama[i]);

                ListViewItem lwitem = new ListViewItem(Trama_Recibida[0]);  //0 ID
                lwitem.SubItems.Add(Trama_Recibida[1]); //1 nombre
                lwitem.SubItems.Add(Trama_Recibida[2]);  //2  imagen
                lwitem.SubItems.Add(Trama_Recibida[3]);  //3  ruta
        
                this.listViewDatos.Items.Add(lwitem);
            }
        }
        private void Mostrar_Asociaciones_Recibidos(string[] Trama, ref StreamWriter sr)
        {
            string[] Trama_Recibida = null;
            
            for (int i = 0; i < Trama.Length-3; i++)
            {
                Trama_Recibida = Trama[i].Split((char)9);
                sr.WriteLine(Trama[i]);

                ListViewItem lwitem = new ListViewItem(Trama_Recibida[0]);  //0
                lwitem.SubItems.Add(Trama_Recibida[1]); //1 
                lwitem.SubItems.Add(Trama_Recibida[2]);  //2
                lwitem.SubItems.Add(Trama_Recibida[3]);  //3
                lwitem.SubItems.Add(Trama_Recibida[4]);  //4
                lwitem.SubItems.Add(Trama_Recibida[5]);  //5
                lwitem.SubItems.Add(Trama_Recibida[6]);  //6
                this.listViewDatos.Items.Add(lwitem);
            }
        }
        
        private void Order_Ascending(int Ncolumna)
        {
            ColHeader clickedCol = (ColHeader)this.listViewDatos.Columns[Ncolumna];

            clickedCol.ascending = !clickedCol.ascending;

            int numItems = this.listViewDatos.Items.Count;

            this.listViewDatos.BeginUpdate();

            ArrayList SortArray = new ArrayList();
            for (int i = 0; i < numItems; i++)
            {
                SortArray.Add(new SortWrapper(this.listViewDatos.Items[i], 0));
            }

            SortArray.Sort(0, SortArray.Count, new SortWrapper.SortComparer(clickedCol.ascending));

            this.listViewDatos.Items.Clear();
            for (int i = 0; i < numItems; i++)
            {
                this.listViewDatos.Items.Add(((SortWrapper)SortArray[i]).sortItem);
            }
            this.listViewDatos.EndUpdate();
        }
        private class ListViewIndexComparer : System.Collections.IComparer
        {
            public int Compare(object x, object y)
            {
                return ((ListViewItem)x).Index - ((ListViewItem)y).Index;
            }
        }
        private void listViewDatos_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            Order_Ascending(e.Column);
        }

        #endregion

        private void StripCerrar_Click(object sender, EventArgs e)
        {
            ToolStripManager.RevertMerge("toolStrip3");
            this.Dispose();
        }
        
    }

    public class WorkerFormWait
    {
        private volatile bool bCloseThread = false;
        string _sMensaje = "";

        public ProgressMarquee pro = new ProgressMarquee();

        public WorkerFormWait(string sMensaje)
        {
            _sMensaje = sMensaje;
        }
        public void vShowMsg()
        {
            pro.Show();
            pro.IniciaProcess(_sMensaje);  //Variable.SYS_MSJ[265, Variable.idioma] + " " + Nserie1 + " " + sIp1
            while (!bCloseThread)
            {
                pro.UpdateProcessInternal(_sMensaje);
            }
            //pro.Close();
            pro.TerminaProcess();
        }
        //public void vShowMsg(IWin32Window owner)
        //{                       
        //    pro.Show(owner);
        //    pro.IniciaProcess(_sMensaje);  //Variable.SYS_MSJ[265, Variable.idioma] + " " + Nserie1 + " " + sIp1
        //    while (!bCloseThread)
        //    {
        //        pro.UpdateProcessInternal(_sMensaje);
        //    }
        //    //pro.Close();
        //    pro.TerminaProcess();            
        //}

        public void vEndShowMsg()
        {            
            bCloseThread = true;            
        }

        public void vUpdateMsg(string sMensaje)
        {
            _sMensaje = sMensaje;   
        }
    }
}


