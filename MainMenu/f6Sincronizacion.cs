using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
using System.Diagnostics;
using System.Globalization;
using MainMenu.BaseDeDatosDataSetTableAdapters;
using TorreyTransfer;
using Validaciones;


namespace MainMenu
{

    public partial class f6Sincronizacion : MdiChildForm
    {
        public delegate void UpdateRichEditCallback(string text);
        public delegate void UpdateRichCallback(string text);
        public delegate void UpdateClientListCallback(int iNumberClientCurrent);
        char[] CharSeparador = new char[] { (char)9, (char)10 };
        private Socket m_mainSocket;
        // es un arreglo de socket que se asignan para trabajar con cada cliente
        private ArrayList m_workerSocketList = ArrayList.Synchronized(new ArrayList());
        private ArrayList myAL = new ArrayList();
        private ArrayList myDatos = new ArrayList();
        Conexion Cte = new Conexion(); //socket cliente para comunicarse con las basculas
        Variable.lgrupo[] myGrupo;
        Variable.lbasc[] myScale;
        private int iNumberClient = 0;
        ADOutil Conec = new ADOutil();
        CheckSum Chk = new CheckSum();
        //definicion del DAtaSet y los TableAdapter.
        BaseDeDatosDataSet baseDeDatosDataSet = new BaseDeDatosDataSet();
        GrupoTableAdapter grupoTableAdapter = new GrupoTableAdapter();
        BasculaTableAdapter basculaTableAdapter = new BasculaTableAdapter();
        PublicidadTableAdapter publicidadTableAdapter = new PublicidadTableAdapter();
        Public_DetalleTableAdapter public_DetalleTableAdapter = new Public_DetalleTableAdapter();
        ProductosTableAdapter productosTableAdapter = new ProductosTableAdapter();
        Prod_detalleTableAdapter prod_detalleTableAdapter = new Prod_detalleTableAdapter();
        carpeta_detalleTableAdapter carpeta_detalleTableAdapter = new carpeta_detalleTableAdapter();
      //  carpetaTableAdapter carpetaTableAdapter = new carpetaTableAdapter();
        OfertaTableAdapter ofertaTableAdapter = new OfertaTableAdapter();
        Oferta_DetalleTableAdapter oferta_DetalleTableAdapter = new Oferta_DetalleTableAdapter();       
        VendedorTableAdapter vendedorTableAdapter = new VendedorTableAdapter();
        IngredientesTableAdapter ingredientesTableAdapter = new IngredientesTableAdapter();
        PLU_detalleTableAdapter pludetalleTableAdapter = new PLU_detalleTableAdapter();

        // State object for reading client data asynchronously
        public struct DataProductFromIpad
        {
            public Int32 iId;
            public string sCodigo;
            public Int32 iNoPlu;
            public string sNombre;
            public double dPrecio;
            public string sImagen;
            public int iTipoId;
            public int iPrecioEditable;
            public int iCaducidadDias;
            public double dImpuesto;
            public Int32 iInfoNutriId;
            public Int32 infoAddId;
            public string sFechaUit;
            public double dTara;
            public int iMultiplo;
            public Int32 iPublicidad1Id;
            public Int32 iPublicidad2Id;
            public Int32 iPublicidad3Id;
            public Int32 iPublicidad4Id;
            public Int32 iOferta;
            public Int32 iGrupo;
            public char cTypeGrupo;
            public Int32 ibImagen;
            public Int32 iCarpetaId;
            public char cChecksum;
        }
        public struct DataPublicidadFromIpad
        {
            public Int32 iId;
            public string sTitulo;            
            public string sMensaje;
            public char cChecksum;
        }
        public struct DataOfertaFromIpad
        {
            public Int32 iId;
            public string sNombre;
            public string sFeIncio;           
            public string sFeFinal;
            public int iTipoDesc;
            public double dDescuento;
            public int iVentas;
            public char cChecksum;
        }
        public struct DataIngredienteFromIpad
        {
            public Int32 iId;
            public string sNombre;
            public string sInformacion;            
            public char cChecksum;
        }
        public struct DataPrecioFromIpad
        {
            public Int32 iIdSuc;
            public Int32 iIdProd;
            public double dPrecio;
            public char cChecksum;
        }
        public struct DataVendedorFromIpad
        {
            public Int32 iId;
            public string sNombre;
            public int iMsjEnable;
            public int iVtaEnable;
            public double dMetaVenta;
            public Int32 iPublicidad1Id;
            public Int32 iPublicidad2Id;        
            public char cChecksum;
        }
        public struct DataLeerProductoFromIpad
        {
            public Int32 idproduct;           
            public int idTipoId;
            public char cTipoId;
        }
        public struct DataLeerCarpetaFromIpad
        {
            public Int32 idGrupo;
            public char cTipo;
            public Int32 iIdCarpeta;
        }
        // State object for reading client data asynchronously
        public class StateObject
        {
            // Client  socket.
            public Socket workSocket = null;
            public string sIpWorkSocket;
            // Size of receive buffer.
            public const int BufferSize = 1024;
            // Receive buffer.
            public byte[] buffer = new byte[BufferSize];
            // Received data string.
            public StringBuilder sb = new StringBuilder();
            public int iNumberClientActive = 0;
        }

        #region Clase de Socket
        public class SocketPacket
        {
            // Constructor which takes a Socket and a client number
            public SocketPacket(Socket socket, int clientNumber, int comando, string ip)
            {
                m_currentSocket = socket;
                m_clientNumber = clientNumber;
                m_comando = comando;
                m_ip = ip;
            }

            public System.Net.Sockets.Socket m_currentSocket;
            public int m_clientNumber;
            public int m_comando;
            public string m_ip;
            // Buffer to store the data sent by the client
            public byte[] dataBuffer = new byte[1024];
        }
        #endregion

        #region Inicializacion
        public f6Sincronizacion()
        {
            InitializeComponent();
            FileStream fw = new FileStream(Variable.appPath + "\\recibe.ttt", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            fw.Close();
            textBoxIP.Text = new GetIP().IPStr;
            this.textBoxPort.Text = "50001";
            Asigna_Grupo();
            Asigna_Bascula();
            ButtonStartListenClick(this.buttonStartListen, null);
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

            Conec.CadenaSelect = "SELECT * FROM Bascula ORDER BY id_grupo";

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
                nitem++;
            }
        }

        private string muestraAutoincrementoId(int op)
        {
            int cod = 0;
            
            switch (op)
            {
                case 0: //productos
                    Conec.CadenaSelect = "SELECT id_producto FROM PRODUCTOS ORDER BY id_producto desc";                   
                    break;
                case 1: //mensajes
                    Conec.CadenaSelect = "SELECT id_publicidad FROM PUBLICIDAD ORDER BY id_publicidad desc";
                    break;
                case 2: // ofertas
                    Conec.CadenaSelect = "SELECT id_oferta FROM OFERTA ORDER BY id_oferta desc";  
                    break;
                case 3: // Ingredientes
                    Conec.CadenaSelect = "SELECT id_ingrediente FROM INGREDIENTES ORDER BY id_ingrediente desc";
                    break;
                case 4: // Vendedores
                    Conec.CadenaSelect = "SELECT id_vendedor FROM VENDEDOR ORDER BY id_vendedor desc";                   
                    break;
                case 5: // Asignacion de carpeta
                    Conec.CadenaSelect = "SELECT  FROM VENDEDOR ORDER BY id_vendedor desc";
                    break;
            }

            OleDbDataReader LP = Conec.Obtiene_Dato(Conec.CadenaSelect, Conec.CadenaConexion);
            if (LP.Read()) cod = Convert.ToInt32(LP.GetValue(0));
            LP.Close();
            return Convert.ToString(cod + 1);
        }

        /// <summary>
        /// Registrar producto en la base de datos.
        /// </summary>
        /// <param name="mystruct"></param>
        private void Crear_Producto(ref f6Sincronizacion.DataProductFromIpad mystruct)
        {
            string idprod = muestraAutoincrementoId(0);

            mystruct.iId = Convert.ToInt32(idprod);

            DataRow dr = baseDeDatosDataSet.Productos.NewRow();

            dr.BeginEdit();
            dr["id_producto"] = mystruct.iId.ToString();
            dr["Codigo"] = mystruct.sCodigo;
            dr["NoPlu"] = mystruct.iNoPlu.ToString();
            dr["Nombre"] = mystruct.sNombre;
            dr["Precio"] = mystruct.dPrecio;
            if (mystruct.ibImagen == 1)
            {
                dr["imagen1"] = Variable.appPath + "\\images\\" + mystruct.sImagen + ".jpg ',";
                mystruct.sImagen = Variable.appPath + "\\images\\" + mystruct.sImagen + ".jpg ',";
                dr["imagen"] = true;
            }
            else
            {
                dr["imagen1"] = "',";
                mystruct.sImagen = "',";
                dr["imagen"] = false;
            }

            dr["TipoId"] = mystruct.iTipoId;
            dr["PrecioEditable"] = mystruct.iPrecioEditable.ToString();
            dr["CaducidadDias"] = mystruct.iCaducidadDias.ToString();
            dr["Impuesto"] = mystruct.dImpuesto.ToString();
            dr["Mutiplo"] = mystruct.iMultiplo.ToString();
            dr["id_ingrediente"] = mystruct.infoAddId.ToString();
            dr["publicidad1"] = mystruct.iPublicidad1Id.ToString();
            dr["publicidad2"] = mystruct.iPublicidad2Id.ToString();
            dr["publicidad3"] = mystruct.iPublicidad3Id.ToString();
            dr["publicidad4"] = mystruct.iPublicidad4Id.ToString();
            dr["actualizado"] = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);
            dr["oferta"] = mystruct.iOferta.ToString();
            dr["tara"] = mystruct.dTara.ToString();
            dr["pendiente"] = true;
            dr["id_info_nutri"] = 0;
            dr.EndEdit();

            productosTableAdapter.Update(dr);
            baseDeDatosDataSet.Productos.AcceptChanges();

            Conec.CadenaSelect = "INSERT INTO Productos " +
            "(id_producto, Codigo, NoPlu, Nombre, Precio, imagen1, PrecioEditable, TipoId, Impuesto, CaducidadDias, Mutiplo, id_ingrediente, publicidad1,publicidad2,publicidad3,publicidad4,actualizado,oferta,tara,pendiente,imagen,id_info_nutri)" +  
           "VALUES ( " + mystruct.iId + "," +                  //id_producto
                         Convert.ToInt32(mystruct.sCodigo) + "," +              //codigo
                         mystruct.iNoPlu + ",'" +                  //No_Plu
                         mystruct.sNombre + "'," +              // Nombre
                         mystruct.dPrecio + ",'" +              //Precio
                         mystruct.sImagen +                     //path de la imagen
                         mystruct.iPrecioEditable + "," +       //PrecioEditable
                         mystruct.iTipoId + "," +               //TipoId pesable o no pesable
                         mystruct.dImpuesto + "," +             //Impuesto
                         mystruct.iCaducidadDias + "," +        //CaducidadDias
                         mystruct.iMultiplo + "," +             //Multiplo
                         mystruct.infoAddId + "," +             //Informacion
                         mystruct.iPublicidad1Id + "," +        //publicidad1
                         mystruct.iPublicidad2Id + "," +        //publicidad2
                         mystruct.iPublicidad3Id + "," +        //publicidad3
                         mystruct.iPublicidad4Id + ",'" +       //publicidad4
                         String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now) + "'," +   //fecha de actualizacion por la pc;
                         mystruct.dTara + "," +                 //Tara
                         mystruct.iOferta + "," +
                         true + "," +
                         dr["imagen"] + "," +
                         0 + ")";

            Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Productos.TableName);
            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);

            Int32 id_bascula = 0;
            Int32 id_grupo = 0;

            if (mystruct.cTypeGrupo == 'D')
            {
                id_grupo = mystruct.iGrupo;
            }
            else
            {
                id_bascula = mystruct.iGrupo;
            }

            Conec.CadenaSelect = "INSERT INTO Prod_detalle " +
               "(id_bascula, id_grupo, id_carpeta, id_producto, codigo, precio, NoPLU, pendiente)" + "VALUES ( " +
                id_bascula + "," +                  //id_bascula             
                id_grupo + "," +                    //Departamento             
                mystruct.iCarpetaId + "," +         //Carptea Id
                mystruct.iId + "," +               //id_producto
                Convert.ToInt32(mystruct.sCodigo) + "," +           //codigo
                mystruct.dPrecio.ToString() + "," + //Precio
                mystruct.iNoPlu + "," +             //NoPLU
                true + ")";                         //Pendiente

            Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Prod_detalle.TableName);
            prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);


            if (id_grupo > 0)
            {
                basculaTableAdapter.Fill(baseDeDatosDataSet.Bascula);

                DataRow[] DR_Selec = baseDeDatosDataSet.Bascula.Select("id_grupo = " + id_grupo);

                foreach (DataRow DA in DR_Selec)
                {
                    id_bascula = Convert.ToInt32(DA["id_bascula"].ToString());

                    Conec.CadenaSelect = "INSERT INTO PLU_detalle " +
                       "(id_bascula, id_grupo, id_producto, pendiente)" + "VALUES ( " +
                        id_bascula + "," +                  //id_bascula             
                        id_grupo + "," +                    //Departamento             
                        mystruct.iId + "," +                //id_producto
                        false + ")";                        //Pendiente

                    Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.PLU_detalle.TableName);

                    pludetalleTableAdapter.Fill(baseDeDatosDataSet.PLU_detalle);
                }
            }
        }

        /// <summary>
        /// Registrar mensaje en la base de datos.
        /// </summary>
        /// <param name="mystruct"></param>
        private void Crear_Mensaje(ref f6Sincronizacion.DataPublicidadFromIpad mystruct)
        {
            string idpubl = muestraAutoincrementoId(1);

            mystruct.iId = Convert.ToInt32(idpubl);

            DataRow dr = baseDeDatosDataSet.Publicidad.NewRow();

            dr.BeginEdit();
            dr["id_publicidad"] = mystruct.iId.ToString();
            dr["Titulo"] = mystruct.sTitulo;
            dr["Mensaje"] = mystruct.sMensaje;
            dr["actualizado"] = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);
            dr["pendiente"] = true;
            dr.EndEdit();

            publicidadTableAdapter.Update(dr);
            baseDeDatosDataSet.Publicidad.AcceptChanges();

            Conec.CadenaSelect = "INSERT INTO Publicidad " +
            "(id_publicidad, Titulo, Mensaje, actualizado, pendiente)" +
           "VALUES ( " + mystruct.iId + ",'" +                  //id_publicidad
                         mystruct.sTitulo + "','" +              //Titulo
                         mystruct.sMensaje + "','" +               //Mensaje
                         String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now) + "'," + //Actualizacion
                         true + ")";

            Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Publicidad.TableName);

            publicidadTableAdapter.Fill(baseDeDatosDataSet.Publicidad);
        }

        /// <summary>
        /// Registrar Ingredientes en la base de datos.
        /// </summary>
        /// <param name="mystruct"></param>
        private void Crear_Ingrediente(ref f6Sincronizacion.DataIngredienteFromIpad mystruct)
        {
            string idingr = muestraAutoincrementoId(3);

            mystruct.iId = Convert.ToInt32(idingr);

            DataRow dr = baseDeDatosDataSet.Ingredientes.NewRow();

            dr.BeginEdit();
            dr["id_ingrediente"] = mystruct.iId.ToString();
            dr["Nombre"] = mystruct.sNombre;
            dr["Informacion"] = mystruct.sInformacion;
            dr["actualizado"] = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);
            dr["pendiente"] = true;
            dr.EndEdit();

            ingredientesTableAdapter.Update(dr);
            baseDeDatosDataSet.Ingredientes.AcceptChanges();

            Conec.CadenaSelect = "INSERT INTO Ingredientes " +
            "(id_ingrediente, Nombre, Informacion, actualizado, pendiente)" +
           "VALUES ( " + mystruct.iId + ",'" +                  //id_ingrediente
                         mystruct.sNombre + "','" +              //Nombre
                         mystruct.sInformacion + "','" +        //Informacion
                        String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now) + "'," + //Actualizacion
                         true + ")";  

            Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Ingredientes.TableName);

            ingredientesTableAdapter.Fill(baseDeDatosDataSet.Ingredientes);
        }

        /// <summary>
        /// Registrar Ofertas en la base de datos.
        /// </summary>
        /// <param name="mystruct"></param>
        private void Crear_Oferta(ref f6Sincronizacion.DataOfertaFromIpad mystruct)
        {
            string fecha_act = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);
            string idofer = muestraAutoincrementoId(2);

            mystruct.iId = Convert.ToInt32(idofer);

            DataRow dr = baseDeDatosDataSet.Oferta.NewRow();

            dr.BeginEdit();
            dr["id_oferta"] = mystruct.iId.ToString();
            dr["nombre"] = mystruct.sNombre;
            dr["fecha_inicio"] = mystruct.sFeIncio;
            dr["fecha_fin"] = mystruct.sFeFinal;
            dr["tipo_desc"] = mystruct.iTipoDesc;
            dr["Descuento"] = mystruct.dDescuento;
            dr["nVentas"] = mystruct.iVentas;
            dr["actualizado"] = fecha_act;
            dr["pendiente"] = true;
            dr.EndEdit();

            ofertaTableAdapter.Update(dr);
            baseDeDatosDataSet.Oferta.AcceptChanges();

            Conec.CadenaSelect = "INSERT INTO Oferta " +
            "(id_oferta, nombre, fecha_inicio, fecha_fin, tipo_desc, Descuento, nVentas, actualizado, pendiente)" +
           "VALUES ( " + mystruct.iId + ",'" +                  //id_oferta
                         mystruct.sNombre + "','" +              //nombre
                         mystruct.sFeIncio + "','" +               //fecha de inicio
                         mystruct.sFeFinal + "'," +              // fecha de termino
                         mystruct.iTipoDesc + "," +              //tipo de descuento
                         mystruct.dDescuento + "," +       //descuento
                         mystruct.iVentas + ",'" +
                         fecha_act + "'," +
                         true + ")";    //numero de ventas autorizadas

            Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Oferta.TableName);

            ofertaTableAdapter.Fill(baseDeDatosDataSet.Oferta);
        }
        /// <summary>
        /// Registrar Vendedores en la base de datos.
        /// </summary>
        /// <param name="mystruct"></param>
        private void Crear_Vendedor(ref f6Sincronizacion.DataVendedorFromIpad mystruct)
        {
            string idvend = muestraAutoincrementoId(4);

            mystruct.iId = Convert.ToInt32(idvend);

            DataRow dr = baseDeDatosDataSet.Vendedor.NewRow();

            dr.BeginEdit();
            dr["id_vendedor"] = mystruct.iId.ToString();
            dr["Nombre"] = mystruct.sNombre;
            dr["Msj_Enable"] = mystruct.iMsjEnable;
            dr["Meta_Enable"] = mystruct.iVtaEnable;
            dr["Meta_Ventas"] = mystruct.dMetaVenta;   
            dr["publicidad1"] = mystruct.iPublicidad1Id.ToString();
            dr["publicidad2"] = mystruct.iPublicidad2Id.ToString();
            dr["actualizado"] = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);
            dr["pendiente"] = true;
            dr.EndEdit();

            vendedorTableAdapter.Update(dr);
            baseDeDatosDataSet.Vendedor.AcceptChanges();

            Conec.CadenaSelect = "INSERT INTO Vendedor " +
            "(id_vendedor,Nombre, Msj_Enable, Meta_Enable, Meta_Ventas, publicidad1,publicidad2,actualizado,pendiente)" +
           "VALUES ( " + mystruct.iId + ",'" +                  //id_producto                        
                         mystruct.sNombre + "'," +              // Nombre                        
                         mystruct.iMsjEnable + "," +            //Mensaje habilitado
                         mystruct.iVtaEnable + "," +            //Ventas habilitada
                         mystruct.dMetaVenta + "," +            //Monto de venta                       
                         mystruct.iPublicidad1Id + "," +        //publicidad1
                         mystruct.iPublicidad2Id + ",'" +        //publicidad2                         
                        String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now) + "'," +    //fecha de actualizacion por la pc;
                         true + ")";


            Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Vendedor.TableName);

            vendedorTableAdapter.Fill(baseDeDatosDataSet.Vendedor);
        }

        /// <summary>
        /// Obtener los parametros de un producto
        /// </summary>
        /// <param name="Codigo_buscado"></param>
        /// <returns></returns>
        private string Lista_Producto(int Codigo_buscado, char cTipoId, int idTipoId)
        {            
            string DatosLeido="";

            DataRow[] drd;
            
            if (cTipoId == 'D')
            {
                drd = baseDeDatosDataSet.Prod_detalle.Select("id_grupo = " + idTipoId + "AND id_producto = " + Codigo_buscado);                
            }
            else
            {
                drd = baseDeDatosDataSet.Prod_detalle.Select("id_bascula = " + idTipoId + "AND id_producto = " + Codigo_buscado);
            }

            baseDeDatosDataSet.Productos.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Productos.id_productoColumn };                            

            DataRow dr = baseDeDatosDataSet.Productos.Rows.Find(Codigo_buscado);

            if (dr != null)
            {
                /*0*/DatosLeido = dr["id_producto"].ToString() + "\t";
                /*1*/DatosLeido += dr["Codigo"].ToString() + "\t";
                /*2*/DatosLeido += dr["NoPLU"].ToString() + "\t";
                /*3*/DatosLeido += Variable.validar_salida(dr["Nombre"].ToString(),2) + "\t";
                /*4*/DatosLeido += dr["Precio"].ToString() + "\t";

                string sNameImage = dr["imagen1"].ToString();
                int index1 = sNameImage.LastIndexOf('\\');
                int index2 = sNameImage.LastIndexOf('.');

                if (index1 > 0 && index2 > 0)
                {
                    sNameImage = sNameImage.Substring(index1 + 1, (index2 - index1) - 1);
                }
                else
                {
                    sNameImage = " ";
                }

                /*5*/DatosLeido  += sNameImage + "\t";
                /*6*/DatosLeido  += dr["TipoId"].ToString() + "\t";
                /*7*/DatosLeido  += dr["PrecioEditable"].ToString() + "\t";
                /*8*/DatosLeido  += dr["CaducidadDias"].ToString() + "\t";
                /*9*/DatosLeido  += string.Format("{0:#0.#0}",Convert.ToDouble(dr["Impuesto"])) + "\t";
                /*10*/DatosLeido += "0" + "\t";                                       /*Informacion Nutricional */

                /*11*/DatosLeido += dr["id_ingrediente"].ToString() + "\t";
                DataRow ing = baseDeDatosDataSet.Ingredientes.Rows.Find(dr["id_ingrediente"]);

                if (ing != null)
                {
                    DatosLeido += ing["Nombre"].ToString() + "\t";
                }
                else
                {
                    DatosLeido += "\t";
                }

                /*13*/DatosLeido += dr["actualizado"].ToString() + "\t";

                /*14*/DatosLeido += dr["tara"].ToString() + "\t";                                       /*Tara */
                
                /*15*/DatosLeido += dr["Mutiplo"].ToString() + "\t";

                /*16*/DatosLeido += dr["publicidad1"].ToString() + "\t";
                DataRow pb = baseDeDatosDataSet.Publicidad.Rows.Find(dr["publicidad1"]);

                if (pb != null)
                {
                    DatosLeido += pb["Titulo"].ToString() + "\t";
                }
                else
                {
                    DatosLeido += "\t";
                }

                /*18*/DatosLeido += dr["publicidad2"].ToString() + "\t";
                pb = baseDeDatosDataSet.Publicidad.Rows.Find(dr["publicidad2"]);

                if (pb != null)
                {
                    DatosLeido += pb["Titulo"].ToString() + "\t";
                }
                else
                {
                    DatosLeido += "\t";
                }
                
                /*20*/DatosLeido += dr["publicidad3"].ToString() + "\t";
                pb = baseDeDatosDataSet.Publicidad.Rows.Find(dr["publicidad3"]);

                if (pb != null)
                {
                    DatosLeido += pb["Titulo"].ToString() + "\t";
                }
                else
                {
                    DatosLeido += "\t";
                }
                
                /*22*/DatosLeido += dr["publicidad4"].ToString() + "\t";
                pb = baseDeDatosDataSet.Publicidad.Rows.Find(dr["publicidad4"]);

                if (pb != null)
                {
                    DatosLeido += pb["Titulo"].ToString() + "\t";
                }
                else
                {
                    DatosLeido += "\t";
                }
                
                /*24*/DatosLeido += dr["oferta"].ToString() + "\t";

                DataRow ofer = baseDeDatosDataSet.Oferta.Rows.Find(dr["oferta"]);

                if (ofer != null)
                {
                    DatosLeido += ofer["nombre"].ToString() + "\t";
                }
                else
                {
                    DatosLeido += "\t";
                }
                
                /*26*/DatosLeido += "1" + "\t";                                       /* Grupo */
                /*27*/DatosLeido += "0" + "\t";                                       /* bansimagen */
                /*28*/DatosLeido += "0" + "\t";                                       /* idcarpeta */

            }
            return DatosLeido;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mystruct"></param>
        private void Modifica_Producto(ref f6Sincronizacion.DataProductFromIpad mystruct)
        {

            baseDeDatosDataSet.Productos.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Productos.id_productoColumn };

            DataRow dr = baseDeDatosDataSet.Productos.Rows.Find(mystruct.iId);

            dr.BeginEdit();
            dr["id_producto"] = mystruct.iId.ToString();
            dr["Codigo"] = mystruct.sCodigo;
            dr["NoPlu"] = mystruct.iNoPlu.ToString();
            dr["Nombre"] = mystruct.sNombre;
            dr["Precio"] = mystruct.dPrecio;
            if (mystruct.ibImagen == 1)
            {
                dr["imagen1"] = Variable.appPath + "\\images\\" + mystruct.sImagen + ".jpg ',";
                mystruct.sImagen = Variable.appPath + "\\images\\" + mystruct.sImagen + ".jpg";
                dr["imagen"] = true;
            }
            else
            {
                mystruct.sImagen = dr["imagen1"].ToString();
                dr["imagen"] = false;
            }

            dr["TipoId"] = mystruct.iTipoId;
            dr["PrecioEditable"] = mystruct.iPrecioEditable.ToString();
            dr["CaducidadDias"] = mystruct.iCaducidadDias.ToString();
            dr["Impuesto"] = mystruct.dImpuesto.ToString();
            dr["Mutiplo"] = mystruct.iMultiplo.ToString();
            dr["id_ingrediente"] = mystruct.infoAddId.ToString();
            dr["publicidad1"] = mystruct.iPublicidad1Id.ToString();
            dr["publicidad2"] = mystruct.iPublicidad2Id.ToString();
            dr["publicidad3"] = mystruct.iPublicidad3Id.ToString();
            dr["publicidad4"] = mystruct.iPublicidad4Id.ToString();
            dr["actualizado"] = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);
            dr["oferta"] = mystruct.iOferta.ToString();
            dr["tara"] = mystruct.dTara.ToString();
            dr["pendiente"] = true;
            dr["id_info_nutri"] = 0;
            dr.EndEdit();

            productosTableAdapter.Update(dr);
            baseDeDatosDataSet.Productos.AcceptChanges();

            Conec.Condicion = "id_producto = " + mystruct.iId;
            Conec.CadenaSelect = "UPDATE Productos " +
            "SET NoPlu = " + mystruct.iNoPlu +
            ", Nombre = '" + mystruct.sNombre +
            "', Precio = " + mystruct.dPrecio +
            ", imagen1 = '" + mystruct.sImagen +
            "', PrecioEditable = " + mystruct.iPrecioEditable +
            ", TipoId = " + mystruct.iTipoId +
            ", Impuesto = " + mystruct.dImpuesto +
            ", CaducidadDias = " + mystruct.iCaducidadDias +
            ", Mutiplo = " + mystruct.iMultiplo +
            ", id_ingrediente = " + mystruct.infoAddId +
            ", publicidad1 = " + mystruct.iPublicidad1Id +
            ", publicidad2 = " + mystruct.iPublicidad2Id +
            ", publicidad3 = " + mystruct.iPublicidad3Id +
            ", publicidad4 = " + mystruct.iPublicidad4Id +
            ", actualizado = '" + String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now) +
            "', oferta = " + mystruct.iOferta +
            ", tara = " + mystruct.dTara +
            ", pendiente = " + true +
            ", imagen = " + dr["imagen"] +
            ", id_info_nutri = " + 0 +
            " WHERE (" + Conec.Condicion + ")";

            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Productos.TableName);
            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);
        }

        private void Modifica_Precio(ref f6Sincronizacion.DataPrecioFromIpad mystruct)
        {
            object[] clave = new object[] { mystruct.iIdSuc, mystruct.iIdProd }; 

            DataRow dr = baseDeDatosDataSet.Prod_detalle.Rows.Find(clave);

            dr.BeginEdit();           
            dr["Precio"] = mystruct.dPrecio;
            dr["pendiente"] = true;
            dr.EndEdit();

            prod_detalleTableAdapter.Update(dr);
            baseDeDatosDataSet.Prod_detalle.AcceptChanges();

            Conec.Condicion = "id_Grupo = " + mystruct.iIdSuc + " AND id_producto = " + mystruct.iIdProd;
            Conec.CadenaSelect = "UPDATE Prod_detalle " +
            "SET Precio = " + mystruct.dPrecio +
            ", pendiente = " + true +
            " WHERE (" + Conec.Condicion + ")";

            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Prod_detalle.TableName);

            prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mystruct"></param>
        private void Modifica_Mensaje(ref f6Sincronizacion.DataPublicidadFromIpad mystruct)
        {
            string fecha_act = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);
            DataRow dr = baseDeDatosDataSet.Publicidad.Rows.Find(mystruct.iId);

            dr.BeginEdit();
            dr["Titulo"] = mystruct.sTitulo;            
            dr["Mensaje"] = mystruct.sMensaje;
            dr["actualizado"] = fecha_act;
            dr["pendiente"] = true;
            dr.EndEdit();

            publicidadTableAdapter.Update(dr);
            baseDeDatosDataSet.Publicidad.AcceptChanges();

            Conec.Condicion = "id_publicidad = " + mystruct.iId;
            Conec.CadenaSelect = "UPDATE Publicidad " +
            "SET Titulo = '" + mystruct.sTitulo +
            "', Mensaje = '" + mystruct.sMensaje +
            "', actualizado = '" + fecha_act +
            "', pendiente = " + true +
            " WHERE (" + Conec.Condicion + ")";

            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Publicidad.TableName);

            publicidadTableAdapter.Fill(baseDeDatosDataSet.Publicidad);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mystruct"></param>
        private void Modifica_Ingrediente(ref f6Sincronizacion.DataIngredienteFromIpad mystruct)
        {
            DataRow dr = baseDeDatosDataSet.Ingredientes.Rows.Find(mystruct.iId);

            dr.BeginEdit();
            dr["Nombre"] = mystruct.sNombre;
            dr["Informacion"] = mystruct.sInformacion;
            dr["actualizado"] = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);
            dr["pendiente"] = true;
            dr.EndEdit();

            ingredientesTableAdapter.Update(dr);
            baseDeDatosDataSet.Ingredientes.AcceptChanges();

            Conec.Condicion = "id_ingrediente = " + mystruct.iId;
            Conec.CadenaSelect = "UPDATE Ingredientes " +
            "SET Nombre = '" + mystruct.sNombre +
            "', Informacion = '" + mystruct.sInformacion +
            "', actualizado = '" + String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now) +
            "', pendiente = " + true +
            " WHERE (" + Conec.Condicion + ")";

            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Ingredientes.TableName);

            ingredientesTableAdapter.Fill(baseDeDatosDataSet.Ingredientes);
        }

        private void Modifica_Oferta(ref f6Sincronizacion.DataOfertaFromIpad mystruct)
        {
            string fecha_act = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);
            DataRow dr = baseDeDatosDataSet.Oferta.Rows.Find(mystruct.iId);

            dr.BeginEdit();
            dr["nombre"] = mystruct.sNombre;
            dr["fecha_inicio"] = mystruct.sFeIncio;
            dr["fecha_fin"] = mystruct.sFeFinal;
            dr["tipo_desc"] = mystruct.iTipoDesc.ToString();
            dr["Descuento"] = mystruct.dDescuento.ToString();
            dr["nVentas"] = mystruct.iVentas.ToString();
            dr["actualizado"] = fecha_act;
            dr["pendiente"] = true;
            dr.EndEdit();

            ofertaTableAdapter.Update(dr);
            baseDeDatosDataSet.Oferta.AcceptChanges();

            Conec.Condicion = "id_oferta = " + mystruct.iId;
            Conec.CadenaSelect = "UPDATE Oferta " +
            "SET nombre = '" + mystruct.sNombre +
            "', fecha_inicio = '" + mystruct.sFeIncio +
            "', fecha_fin = '" + mystruct.sFeFinal +
            "', tipo_desc = " + mystruct.iTipoDesc +
            ", Descuento = " + mystruct.dDescuento +
            ", nVentas = " + mystruct.iVentas +
            ", actualizado = '" + fecha_act +
            "', pendiente = " + true +
            " WHERE (" + Conec.Condicion + ")";

            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Oferta.TableName);

            ofertaTableAdapter.Fill(baseDeDatosDataSet.Oferta);
        }

        private void Modifica_Vendedor(ref f6Sincronizacion.DataVendedorFromIpad mystruct)
        {
            DataRow dr = baseDeDatosDataSet.Vendedor.Rows.Find(mystruct.iId);

            dr.BeginEdit();
            dr["Nombre"] = mystruct.sNombre;
            dr["Msj_Enable"] = mystruct.iMsjEnable;
            dr["Meta_Enable"] = mystruct.iVtaEnable;
            dr["Meta_Ventas"] = mystruct.dMetaVenta;
            dr["publicidad1"] = mystruct.iPublicidad1Id.ToString();
            dr["publicidad2"] = mystruct.iPublicidad2Id.ToString();
            dr["actualizado"] = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);
            dr["pendiente"] = true;
            dr.EndEdit();

            vendedorTableAdapter.Update(dr);
            baseDeDatosDataSet.Vendedor.AcceptChanges();

            Conec.Condicion = "id_vendedor = " + mystruct.iId;
            Conec.CadenaSelect = "UPDATE Vendedor " +
            "SET Nombre = '" + mystruct.sNombre +
            "', Msj_Enable = " + mystruct.iMsjEnable +
            ", Meta_Enable = " + mystruct.iVtaEnable +
            ", Meta_Ventas = " + mystruct.dMetaVenta +
            ", actualizado = '" + String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now) +
            "', publicidad1 = " + mystruct.iPublicidad1Id +
            ", publicidad2 = " + mystruct.iPublicidad2Id +
            ", pendiente = " + true +
            " WHERE (" + Conec.Condicion + ")";

            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Vendedor.TableName);

            vendedorTableAdapter.Fill(baseDeDatosDataSet.Vendedor);
        }
        #endregion

        #region  Funciones Asyncrona de Socket
        
        /// <summary>
        /// Escuchar conexiones en el socket 50001
        /// </summary>
        void inicia_listen()
        {
            try
            {
                string portStr = "50001";
                if (textBoxPort.Text == "") textBoxPort.Text = portStr;  // valida el puerto

                int port = System.Convert.ToInt32(portStr);
                
                m_mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, port);
                
                m_mainSocket.Bind(ipLocal);
                m_mainSocket.Listen(100);

                Console.WriteLine("Waiting for a connection...");
               // AppendToRichEditControl("Waiting for a connection...\n");
                

                m_mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), m_mainSocket);
                UpdateControls(true);
            }
            catch (SocketException se)
            {
                MessageBox.Show(this, se.Message);
            }
            catch (ObjectDisposedException oe)
            {
                MessageBox.Show(this, oe.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void UpdateControls(bool listening)
        {
            //buttonStartListen.Enabled = !listening;
            //buttonStopListen.Enabled = listening;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="asyn"></param>
        public void OnClientConnect(IAsyncResult asyn)
        {
            try
            {
                // Get the socket that handles the client request.
                Socket listener = (Socket)asyn.AsyncState;
                Socket handler = listener.EndAccept(asyn);

                iNumberClient += 1;
                m_workerSocketList.Add(handler);
                UpdateClientListControl(iNumberClient);

                // Create the state object.
                StateObject state = new StateObject();
                state.workSocket = handler;
                state.iNumberClientActive = iNumberClient;

                IPEndPoint clientep = (IPEndPoint)state.workSocket.RemoteEndPoint;

                Console.WriteLine(" ");
                Console.WriteLine("---------------------------------------------------------------------------");
                Console.WriteLine("Se conecto el cliente: {0}, Port: {1}", state.iNumberClientActive, clientep);

                try
                {
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                }
                catch (SocketException e)
                {
                    Console.WriteLine("Error en beginReceive: {0}", e.Message);
                }

                state.sIpWorkSocket = IPAddress.Parse(((IPEndPoint)handler.RemoteEndPoint).Address.ToString()).ToString();

            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\n OnClientConnection: Socket has been closed\n");
            }
            catch (SocketException se)
            {
                MessageBox.Show(this, se.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        /// <summary>
        /// Lectura asincrona del socket
        /// </summary>
        /// <param name="ar"></param>
        public void ReadCallback(IAsyncResult ar)
        {
            int iBanderaError = 0;
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            int bytesRead = handler.EndReceive(ar);             // Read data from the client socket. 

            if (bytesRead > 0)
            {
                try
                {
                    // There  might be more data, so store the data received so far.
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    content = state.sb.ToString();
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}", content.Length, content);

                    state.sb.Clear();

                    //---------------------------------

                    string direccionip = state.sIpWorkSocket;
                    string msg = "";
                    
                    string replyMsg = content;
                    int sizereplymsg = replyMsg.Length;
                    
                    string sCommand = "XX";

                    byte[] byData = null;
					char chk = new CheckSum().ChkSum(replyMsg);

                    if (replyMsg.Length > 1 && chk == replyMsg[sizereplymsg - 2])
                    {
                        replyMsg = replyMsg.Substring(1, sizereplymsg - 1);
                        sCommand = replyMsg.Substring(0, 2);        //Obtiene el comando a ejecutar
                        msg = "Cliente " + direccionip + " Comando: " + sCommand + "\n";
                        Console.WriteLine(msg);

                        switch (sCommand)
                        {

                            case "Lg":
                                byData = pCommand_Lg(replyMsg);
                                break;

                            case "Lb":
                                byData = pCommand_Lb(replyMsg);
                                break;

                            case "LM":
                                byData = pCommand_LM(replyMsg);
                                break;

                            case "LP":
                                byData = pCommand_LP(replyMsg, handler);
                                break;
                            case "LI":
                                byData = pCommand_LI(replyMsg);
                                break;

                            case "LO":
                                byData = pCommand_LO(replyMsg);
                                break;

                            case "LV":
                                byData = pCommand_LV(replyMsg);
                                break;

                            case "NP":
                                byData = pCommand_NP(replyMsg, handler);
                                break;

                            case "NO":
                                byData = pCommand_NO(replyMsg, handler);
                                break;

                            case "NI":
                                byData = pCommand_NI(replyMsg, handler);
                                break;

                            case "NM":
                                byData = pCommand_NM(replyMsg, handler);
                                break;

                            case "NV":
                                byData = pCommand_NV(replyMsg);
                                break;

                            case "GP":
                                byData = pCommand_GP(replyMsg, handler);
                                break;

                            case "GI":
                                byData = pCommand_GI(replyMsg);
                                break;

                            case "GM":
                                byData = pCommand_GM(replyMsg);
                                break;

                            case "GO":
                                byData = pCommand_GO(replyMsg);
                                break;

                            case "Gp":
                                byData = pCommand_Gp(replyMsg);
                                break;

                            case "GV":
                                byData = pCommand_GV(replyMsg);
                                break;

                            case "LC":
                                byData = pCommand_LC(replyMsg);
                                break;

                            case "CX":
                                byData = pCommand_CX(replyMsg);
                                break;

                            case "VP":
                                byData = pCommand_VP(replyMsg);
                                break;

                            default:
                                Console.WriteLine("Comando no reconocido");
                                msg = sCommand + "0" + "\t" + "2" + "\t" + "\n";
                                byData = Encoding.GetEncoding(437).GetBytes(msg);
                                break;
                        }

                        iEnviarCadenaIpad(sCommand, byData, handler);
                    }
                    else
                    {
                        Console.WriteLine("Error de Checksum");
                        AppendToRichEditControl("Se desconecto el cliente: " + state.iNumberClientActive + "\n");
                        Console.WriteLine("OB Se desconecto el cliente: {0}", state.iNumberClientActive);
                        handler.Close();
                        iBanderaError = 1;
                    }
                }
                catch (ObjectDisposedException)
                {
                    AppendToRichEditControl("OD Se desconecto el cliente: " + state.iNumberClientActive + "\n");
                    Console.WriteLine("OD Se desconecto el cliente: {0}", state.iNumberClientActive);
                    handler.Close();
                }
                catch (SocketException se)
                {
                    AppendToRichEditControl("OD Se desconecto el cliente: " + state.iNumberClientActive + "\n");
                    Console.WriteLine("SE Se desconecto el cliente: {0}", state.iNumberClientActive);
                    System.Diagnostics.Debugger.Log(0, "1", se.Message);                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message);
                    iBanderaError = 1;
                }

                if (iBanderaError == 0)
                {
                    try
                    {
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            else
            {
                AppendToRichEditControl("Se desconecto el cliente: " + state.iNumberClientActive + "\n");
                Console.WriteLine("OB Se desconecto el cliente: {0}", state.iNumberClientActive);
            }

            m_mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), m_mainSocket);
        }

        #region Loggin Activities 
        // This method could be called by either the main thread or any of the
        // worker threads
        private void AppendToRichEditControl(string msg)
        {
            // Check to see if this method is called from a thread 
            // other than the one created the control
            if (InvokeRequired)
            {
                // We cannot update the GUI on this thread.
                // All GUI controls are to be updated by the main (GUI) thread.
                // Hence we will use the invoke method on the control which will
                // be called when the Main thread is free
                // Do UI update on UI thread
                object[] pList = { msg };
                richTextBoxReceivedMsg.BeginInvoke(new UpdateRichEditCallback(OnUpdateRichEdit), pList);
            }
            else
            {
                // This is the main thread which created this control, hence update it
                // directly 
                OnUpdateRichEdit(msg);
            
            }
        }

        // This UpdateRichEdit will be run back on the UI thread
        // (using System.EventHandler signature
        // so we don't need to define a new
        // delegate type here)
        public void OnUpdateRichEdit(string msg)
        {
            //if (richTextBoxReceivedMsg.TextLength >= richTextBoxReceivedMsg.MaxLength) richTextBoxReceivedMsg.Clear();
            //richTextBoxReceivedMsg.AppendText(msg);
        }

      /*  public void AppendToRichUpdateControl(string msg)
        {
            if (InvokeRequired)
            {
                object[] pList = { msg };
                richTextBoxSendMsg.BeginInvoke(new UpdateRichCallback(OnUpdateRich), pList);
            }
            else
            {
                OnUpdateRich(msg);
            }
        }

        private void OnUpdateRich(string msg)
        {
            if (richTextBoxSendMsg.TextLength >= richTextBoxSendMsg.MaxLength) richTextBoxSendMsg.Clear();
            richTextBoxSendMsg.AppendText(msg);
        }*/

        private void UpdateClientListControl(int iNumber_Client)
        {
            if (InvokeRequired)
            {
                object[] pList = { iNumber_Client };
                Listado_bascula.BeginInvoke(new UpdateClientListCallback(UpdateClientBascula), pList);  
            }
            else
            {
                UpdateClientBascula(iNumber_Client);
            }
        }
	    #endregion

		#region Enviar cadena ipad

        private int iEnviarCadenaIpad(string sCommand, byte[] byData, Socket client)
        {
            string sDataOut = Encoding.Default.GetString(byData);

            //Console.WriteLine("DataOut: {0}", sDataOut);

            sDataOut = EliminaAcentos(sDataOut);
            char chk = new CheckSum().ChkSum(sDataOut);
            sDataOut += chk + "\r";

            string sSendSize = sCommand;
            sSendSize += "\t" + sDataOut.Length.ToString("D8") + "\t" + "\n";

            chk = new CheckSum().ChkSum(sSendSize);
            sSendSize += chk + "\r";

            Console.WriteLine("{0}: Cadena de tamaño: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, sSendSize);

            try
            {
                int iBytesEnviados = client.Send(Encoding.GetEncoding(437).GetBytes(sSendSize));
                Console.WriteLine("Bytes enviados: {0}", iBytesEnviados);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return 1;
            }

            byte[] bDataRecibido = new byte[10];

            for (int i = 0; i < 10; i++)
            {
                bDataRecibido[i] = 0;
            }

            int iBytesRecibidos = client.Receive(bDataRecibido);
            string sDataRecibido = Encoding.Default.GetString(bDataRecibido);

            sDataRecibido = sDataRecibido.Substring(1, sDataRecibido.Length - 1);

            Console.WriteLine("{0}(): Dato recibido: {1}, \nBytes: {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, sDataRecibido, iBytesRecibidos);

            if (iBytesRecibidos > 4)
            {
                if (sDataOut.Substring(0, 2) == sDataRecibido.Substring(0, 2) && sDataRecibido.Substring(2, 2) == "00")
                {

                    int iBytesSends = client.Send(Encoding.GetEncoding(437).GetBytes(sDataOut));
                    Console.WriteLine("{0}(): Bytes enviados: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, iBytesSends);

                    if (sDataOut.Substring(0, 1) == "N" || sDataOut.Substring(0, 1) == "G")
                    {
                        string ipLocal = new GetIP().IPStr;
                        OleDbDataReader olr = Conec.Obtiene_Dato("Select IPLocal, dias, Intervalo, pos_inter, H_inicial,H_final,ActivarFrecuencia from DatosGral Where IPLocal = '" + ipLocal + "'", Conec.CadenaConexion);
                        if (olr.Read())
                        {
                            Variable.Activar_Frecuencia = Convert.ToSByte(olr.GetValue(6));
                        }
                        olr.Close();

                        if (Variable.Activar_Frecuencia != 0)
                        {
                            Envia_Dato E = new Envia_Dato();
                            Variable.Comando_Ipad = true;
                            E.actualizar_basculas();
                        }
                    }
                }
                else
                {
                    Console.WriteLine("IPAD no recibio el tamaño de la cadena");
                    return 1;
                }
            }
            else
            {
                Console.WriteLine("{0}(): El numero de bytes es menor al esprado (4)", System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

            return 0;
        }
				
        #endregion

				
        #region Procesamienvo Comando desde Ipad

        /// <summary>
        /// Lg   Leer grupos
        /// </summary>
        /// <returns></returns>
        private byte[] pCommand_Lg(string CadenaComando)
        {
            string SubMensaje = CadenaComando.Substring(2);                      //quita el comando para analizar toda la trama
            string[] DatoRecibido = SubMensaje.Split(CharSeparador);
            ValidateData validar = new ValidateData();
            string ReturnDataString = "";
            int posicion = 0;

						Asigna_Grupo();
            Asigna_Bascula();
						
            if (validar.iValidateInt(DatoRecibido[0]) == 0)
            {
                int idLg = Convert.ToInt32(DatoRecibido[0]);

                if (idLg == 0)
                {                    

                    for (int iIndex = 0; iIndex < myGrupo.Length; iIndex++)
                    {
                        ReturnDataString += myGrupo[iIndex].ngpo + "\t" + myGrupo[iIndex].nombre + "\t" + "D" + "\t\n";
                        posicion++;
                    }

                    for (int iIndex = 0; iIndex < myScale.Length; iIndex++)
                    {
                        if (myScale[iIndex].gpo == 0)
                        {
                            ReturnDataString += myScale[iIndex].idbas.ToString() + "\t" + myScale[iIndex].nombre + "\t" + "B" + "\t\n";
                            posicion++;
                        }
                    }
                }
                else
                {                                  
                    for (int iIndex = 0; iIndex < myScale.Length; iIndex++)
                    {
                        if (myScale[iIndex].gpo == idLg)
                        {
                            ReturnDataString += myScale[iIndex].idbas.ToString() + "\t" + myScale[iIndex].nserie + "\t" + myScale[iIndex].ip + "\t" + myScale[iIndex].nombre + "\t" + "\n";
                            posicion++;
                        }
                    }
                }

                if (posicion == 0)
                {
                    ReturnDataString = "Lg" + posicion + "\t\n";
                }
                else
                {
                    ReturnDataString = "Lg" + posicion + "\t" + ReturnDataString;
                }
            }            
						
            return Encoding.GetEncoding(437).GetBytes(ReturnDataString);
        }

        /// <summary>
        /// Lb -> Leer bascula
        /// </summary>
        /// <param name="CadenaComando"></param>
        /// <returns></returns>
        private byte[] pCommand_Lb(string CadenaComando)
        {                      
            string SubMensaje = CadenaComando.Substring(2);                      
            string[] DatoRecibido = SubMensaje.Split(CharSeparador);
            string DatoElemento = "";
            int iIdBascula = 0;
            ValidateData validar = new ValidateData();
            int posicion = 0;

            if (validar.iValidateInt(DatoRecibido[0]) == 0)
            {
                iIdBascula = Convert.ToInt32(DatoRecibido[0]);

                for (int iIndex = 0; iIndex < myScale.Length; iIndex++)
                {
                    if (myScale[iIndex].idbas == iIdBascula)
                    {
                        DatoElemento += myScale[iIndex].idbas.ToString() + "\t" + myScale[iIndex].nserie + "\t" + myScale[iIndex].ip + "\t" + myScale[iIndex].nombre + "\t" + "\n";
                        posicion++;
                    }
                }
            }

            string ReturnDataString = "Lb" + posicion + "\t";

            if (posicion == 0)
            {
                ReturnDataString += "\n";

            }else
            {
                ReturnDataString += DatoElemento;
            }

            return Encoding.GetEncoding(437).GetBytes(ReturnDataString);
        }

        /// <summary>
        /// LM -> Leer los mensajes almacenados
        /// </summary>
        /// <param name="CadenaComando"></param>
        /// <returns></returns>
        private byte[] pCommand_LM(string CadenaComando)
        {
            string SubMensaje = CadenaComando.Substring(2);                      
            string[] DatoRecibido = SubMensaje.Split(CharSeparador);   
            string ReturnDataString = "";

            ValidateData validar = new ValidateData();

            if (validar.iValidateInt(DatoRecibido[0]) == 0)                         //TODO Verificr esta validacion
            {
                int idMensaje = Convert.ToInt32(DatoRecibido[0]);

                if (idMensaje == 0)
                {
                    publicidadTableAdapter.Fill(baseDeDatosDataSet.Publicidad);

                    DataRow[] DT = baseDeDatosDataSet.Publicidad.Select("borrado = " + false, "Titulo ASC");

                    if (DT.Length > 0)
                    {
                        foreach (DataRow DR in DT)
                        {
                            ReturnDataString += DR["id_publicidad"].ToString() + "\t" + DR["Titulo"].ToString() + "\t" + DR["Mensaje"].ToString() + "\t" + "\n";
                        }
                        ReturnDataString = "LM" + baseDeDatosDataSet.Publicidad.Rows.Count.ToString() + "\t" + ReturnDataString;
                    }
                    else
                    {
                        ReturnDataString = "LM0" + "\t" + "\n";
                    }
                }
                else
                {                    
                    publicidadTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                    publicidadTableAdapter.Connection.CreateCommand().CommandText = "SELECT * FROM Publicidad Order by id_publicidad";
                    publicidadTableAdapter.Fill(baseDeDatosDataSet.Publicidad);
                    baseDeDatosDataSet.Publicidad.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Publicidad.id_publicidadColumn };
                    
                    DataRow DR = baseDeDatosDataSet.Publicidad.Rows.Find(idMensaje);

                    if (DR != null)
                    {
                        ReturnDataString = "LM1" + "\t" + DR["id_publicidad"].ToString() + "\t" + DR["Titulo"].ToString() + "\t" + DR["Mensaje"].ToString() + "\t" + "\n";
                    }
                    else
                    {
                        ReturnDataString = "LM0" + "\t" + "\n";
                    }
                }
            }

            return Encoding.GetEncoding(437).GetBytes(ReturnDataString);
        }

        
        /// <summary>
        /// LI -> Leer los ingredientes almacenados
        /// </summary>
        /// <param name="CadenaComando"></param>
        /// <returns></returns>
        private byte[] pCommand_LI(string CadenaComando)
        {
            string SubMensaje = CadenaComando.Substring(2);
            string[] DatoRecibido = SubMensaje.Split(CharSeparador);
            string ReturnDataString = "";

            ValidateData validar = new ValidateData();

            if (validar.iValidateInt(DatoRecibido[0]) == 0)                         //TODO Verificr esta validacion
            {
                int idInformacion = Convert.ToInt32(DatoRecibido[0]);

                if (idInformacion == 0)
                {
                    ingredientesTableAdapter.Fill(baseDeDatosDataSet.Ingredientes);

                    DataRow[] DT = baseDeDatosDataSet.Ingredientes.Select("borrado = " + false, "Nombre ASC");

                    if (DT.Length > 0)
                    {
                        foreach (DataRow DR in DT)
                        {
                            ReturnDataString += DR["id_ingrediente"].ToString() + "\t" + DR["Nombre"].ToString() + "\t" + DR["Informacion"].ToString() + "\t" + "\n";
                        }
                        ReturnDataString = "LI" + baseDeDatosDataSet.Ingredientes.Rows.Count.ToString() + "\t" + ReturnDataString;
                    }
                    else
                    {
                        ReturnDataString = "LI0" + "\t" + "\n";
                    }
                }
                else
                {
                    ingredientesTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                    ingredientesTableAdapter.Connection.CreateCommand().CommandText = "SELECT * FROM Ingredientes Order by id_ingrediente";
                    ingredientesTableAdapter.Fill(baseDeDatosDataSet.Ingredientes);
                    baseDeDatosDataSet.Ingredientes.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Ingredientes.id_ingredienteColumn };

                    DataRow DR = baseDeDatosDataSet.Ingredientes.Rows.Find(idInformacion);

                    if (DR != null)
                    {
                        ReturnDataString = "LI1" + "\t" + DR["id_ingrediente"].ToString() + "\t" + DR["Nombre"].ToString() + "\t" + DR["Informacion"].ToString() + "\t" + "\n";
                    }
                    else
                    {
                        ReturnDataString = "LI0" + "\t" + "\n";
                    }
                }
            }

            return Encoding.GetEncoding(437).GetBytes(ReturnDataString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CadenaComando"></param>
        /// <returns></returns>
        private byte[] pCommand_LV(string CadenaComando)
        {
            string SubMensaje = CadenaComando.Substring(2);
            string[] DatoRecibido = SubMensaje.Split(CharSeparador);            
            string ReturnDataString = "";

            ValidateData validar = new ValidateData();

            if (validar.iValidateInt(DatoRecibido[0]) == 0)                         //TODO Verificr esta validacion
            {
                int idVendedor = Convert.ToInt32(DatoRecibido[0]);

                if (idVendedor == 0)
                {                                     
                    vendedorTableAdapter.Fill(baseDeDatosDataSet.Vendedor);

                    DataRow[] DT = baseDeDatosDataSet.Vendedor.Select("borrado = " + false, "nombre ASC");

                    if(DT.Length > 0)
                    {
                        foreach (DataRow DR in DT)
                        {
                            ReturnDataString += DR["id_vendedor"].ToString() + "\t" + DR["nombre"] + "\t" + Convert.ToInt32(DR["Meta_Enable"]) + "\t" + Convert.ToInt32(DR["Msj_Enable"]) + "\t" + DR["Meta_VEntas"].ToString() + "\t";

                            ReturnDataString += DR["publicidad1"].ToString() + "\t";

                            DataRow vd = baseDeDatosDataSet.Publicidad.Rows.Find(DR["publicidad1"]);

                            if (vd != null)
                            {
                                ReturnDataString += vd["Titulo"].ToString() + "\t";
                            }
                            else
                            {
                                ReturnDataString += "\t";
                            }


                            ReturnDataString += DR["publicidad2"].ToString() + "\t";

                            vd = baseDeDatosDataSet.Publicidad.Rows.Find(DR["publicidad2"]);

                            if (vd != null)
                            {
                               ReturnDataString += vd["Titulo"].ToString();
                            }

                            ReturnDataString += "\t" + "\n";
                        }

                        ReturnDataString = "LV" + baseDeDatosDataSet.Vendedor.Rows.Count.ToString() + "\t" + ReturnDataString;
                    }
                    else
                    {
                        ReturnDataString = "LV0" + "\t" + "\n";
                    }
                }
                else
                {
                    vendedorTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                    vendedorTableAdapter.Connection.CreateCommand().CommandText = "SELECT * FROM Vendedor Order by id_vendedor";
                    vendedorTableAdapter.Fill(baseDeDatosDataSet.Vendedor);
                    baseDeDatosDataSet.Vendedor.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Vendedor.id_vendedorColumn };

                    DataRow DR = baseDeDatosDataSet.Vendedor.Rows.Find(idVendedor);

                    if (DR != null)
                    {
                        ReturnDataString = "LV1" + "\t" + DR["id_vendedor"].ToString() + "\t" + DR["nombre"] + "\t" + Convert.ToInt32(DR["Meta_Enable"]) + "\t" + Convert.ToInt32(DR["Msj_Enable"]) + "\t" + DR["Meta_VEntas"].ToString() + "\t";
                        
                        ReturnDataString += DR["publicidad1"].ToString() + "\t";

                        DataRow vd = baseDeDatosDataSet.Publicidad.Rows.Find(DR["publicidad1"]);

                        if (vd != null)
                        {
                            ReturnDataString += vd["Titulo"].ToString() + "\t";
                        }
                        else
                        {
                            ReturnDataString += "\t";
                        }


                        ReturnDataString += DR["publicidad2"].ToString() + "\t";

                        vd = baseDeDatosDataSet.Publicidad.Rows.Find(DR["publicidad2"]);

                        if (vd != null)
                        {
                            ReturnDataString += vd["Titulo"].ToString();
                        }                      
                        
                        ReturnDataString += "\t" + "\n";
                    }
                    else
                    {
                        ReturnDataString = "LV0" + "\t" + "\n";
                    }
                }
            }

            return Encoding.GetEncoding(437).GetBytes(ReturnDataString);
        }

        /// <summary>
        /// GM -> Grabar mensajes 
        /// </summary>
        /// <param name="CadenaComando"></param>
        /// <returns></returns>
        private byte[] pCommand_GM(string CadenaComando)
        {
            string SubMensaje = CadenaComando.Substring(2);
            string[] DatoRecibido = SubMensaje.Split(CharSeparador);
            string ReturnDataString = null;

            ValidateData validar = new ValidateData();
            DataPublicidadFromIpad StructData = new DataPublicidadFromIpad();
            int iError_Funct = 0;

            iError_Funct = validar.iValidateGM(DatoRecibido, ref StructData);

            if (iError_Funct == 0)
            {
                publicidadTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                publicidadTableAdapter.Connection.CreateCommand().CommandText = "SELECT * FROM Publicidad";
                publicidadTableAdapter.Fill(baseDeDatosDataSet.Publicidad);
                baseDeDatosDataSet.Publicidad.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Publicidad.id_publicidadColumn };

                DataRow DR_Mensaje = baseDeDatosDataSet.Publicidad.Rows.Find(StructData.iId);
                if (DR_Mensaje != null)
                {
                    Modifica_Mensaje(ref StructData);
                    ReturnDataString = "GM" + StructData.iId + "\t" + "\n";
                }
                else
                {
                    ReturnDataString = "GM" + "0" + "\t" + "4" + "\t" + "\n";           //Mensaje no existe
                }
            }
            else
            {
                ReturnDataString = "GM" + "0" + "\t" + "2" + "\t" + "\n";           //Error formato datos
            }


            return Encoding.GetEncoding(437).GetBytes(ReturnDataString);
        }
        /// <summary>
        /// GI -> Grabar Ingredientes 
        /// </summary>
        /// <param name="CadenaComando"></param>
        /// <returns></returns>
        private byte[] pCommand_GI(string CadenaComando)
        {
            string SubMensaje = CadenaComando.Substring(2);
            string[] DatoRecibido = SubMensaje.Split(CharSeparador);
            string ReturnDataString = null;

            ValidateData validar = new ValidateData();
            DataIngredienteFromIpad StructData = new DataIngredienteFromIpad();
            int iError_Funct = 0;

            iError_Funct = validar.iValidateGI(DatoRecibido, ref StructData);

            if (iError_Funct == 0)
            {
                ingredientesTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                ingredientesTableAdapter.Connection.CreateCommand().CommandText = "SELECT * FROM ingredientes";
                ingredientesTableAdapter.Fill(baseDeDatosDataSet.Ingredientes);
                baseDeDatosDataSet.Ingredientes.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Ingredientes.id_ingredienteColumn };

                DataRow DR_Mensaje = baseDeDatosDataSet.Publicidad.Rows.Find(StructData.iId);

                if (DR_Mensaje != null)
                {
                    Modifica_Ingrediente(ref StructData);
                    ReturnDataString = "GI" + StructData.iId + "\t" + "\n";
                }
                else
                {
                    ReturnDataString = "GI" + "0" + "\t" + "5" + "\t" + "\n";           //Mensaje no existe
                }
            }
            else
            {
                ReturnDataString = "GI" + "0" + "\t" + "2" + "\t" + "\n";           //Error formato datos
            }

            return Encoding.GetEncoding(437).GetBytes(ReturnDataString);
        }
        /// <summary>
        /// Gp -> Actualiza precio 
        /// </summary>
        /// <param name="CadenaComando"></param>
        /// <returns></returns>
        private byte[] pCommand_Gp(string CadenaComando)
        {
            string SubMensaje = CadenaComando.Substring(2);
            string[] DatoRecibido = SubMensaje.Split(CharSeparador);
            string ReturnDataString = null;
            object[] clave = new object[2];

            ValidateData validar = new ValidateData();
            DataPrecioFromIpad StructData = new DataPrecioFromIpad();

            if (validar.iValidateInt(DatoRecibido[0]) == 0)                         
            {
                prod_detalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                prod_detalleTableAdapter.Connection.CreateCommand().CommandText = "SELECT * FROM ingredientes";
                prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);
                baseDeDatosDataSet.Prod_detalle.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Prod_detalle.id_grupoColumn, baseDeDatosDataSet.Prod_detalle.id_productoColumn };

                clave[0] = StructData.iIdSuc;
                clave[1] = StructData.iIdProd;

                DataRow DR_Mensaje = baseDeDatosDataSet.Prod_detalle.Rows.Find(clave);
                if (DR_Mensaje != null)
                {
                    Modifica_Precio(ref StructData);
                    ReturnDataString = "Gp01" + Crear_Trama_Precio(clave);
                }
                else
                {
                    ReturnDataString = "Gp0" + "\t" + "6" + "\n";           //Codigo no existente
                }
            }
            else
            {
                ReturnDataString = "Gp" + "0" + "\t" + "2" + "\t" + "\n";   //Error formato datos
            }
            
            return Encoding.GetEncoding(437).GetBytes(ReturnDataString);
        }
        /// <summary>
        /// GO -> Grabar Ofertas
        /// </summary>
        /// <param name="CadenaComando"></param>
        /// <returns></returns>
        private byte[] pCommand_GO(string CadenaComando)
        {
            string SubMensaje = CadenaComando.Substring(2);
            string[] DatoRecibido = SubMensaje.Split(CharSeparador);
            string ReturnDataString = null;

            ValidateData CheckData = new ValidateData();
            DataOfertaFromIpad StructData = new DataOfertaFromIpad();

            int iError_Funct = 0;

            iError_Funct = CheckData.iValidateGO(DatoRecibido, ref StructData);

            if (iError_Funct == 0)
            {               
                ofertaTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                ofertaTableAdapter.Connection.CreateCommand().CommandText = "Select * from Oferta Order by id_oferta";
                ofertaTableAdapter.Fill(baseDeDatosDataSet.Oferta);
                baseDeDatosDataSet.Oferta.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Oferta.id_ofertaColumn };

                DataRow dr = baseDeDatosDataSet.Oferta.Rows.Find(StructData.iId);

                if (dr != null)
                {
                    Modifica_Oferta(ref StructData);
                    ReturnDataString = "GO" + StructData.iId.ToString() + "\t" + "\n";
                }
                else
                {
                    ReturnDataString = "GO" + "0" + "\t" + "7" + "\t" + "\n";       //Oferta no existe
                }
            }
            else
            {
                ReturnDataString = "GO" + "0" + "\t" + "2" + "\t" + "\n";           //Error formato datos
            }

            return Encoding.GetEncoding(437).GetBytes(ReturnDataString);
        }

        /// <summary>
        /// Obtener datos de producto(s)
        /// </summary>
        /// <param name="CadenaComando"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        private byte[] pCommand_LP(string CadenaComando, Socket client)
        {
            string SubMensaje = CadenaComando.Substring(2);                      //quita el comando para analizar toda la trama
            string[] DatoRecibido = SubMensaje.Split(CharSeparador);

            productosTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            productosTableAdapter.Connection.CreateCommand().CommandText = "Select * from Productos Order by Codigo";
            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);

            string ReturnDataString = null;
            ValidateData validar = new ValidateData();
            DataLeerProductoFromIpad StructData = new DataLeerProductoFromIpad();

            int iError_Funct = 0;

            iError_Funct = validar.iValidateLP(DatoRecibido, ref StructData);

            if (iError_Funct == 0)
            {
                if (StructData.idproduct > 0)
                {
                    ReturnDataString = "LP01" + "\t";
                    ReturnDataString += Lista_Producto(StructData.idproduct, StructData.cTipoId, StructData.idTipoId) + "\n";

                    CommandTorrey myobj = new CommandTorrey();

                    string PathImage = ReadPatchImage(StructData.idproduct);

                    if (string.Compare(PathImage, "") == 0)
                    {
                        string sNoImage = "00000001" + "\t" + "\n";

                        char chk = new CheckSum().ChkSum(sNoImage);
                        sNoImage += chk + "\r";

                        client.Send(Encoding.GetEncoding(437).GetBytes(sNoImage));

                        Console.WriteLine("Dato enviado: {0}", sNoImage);

                    }
                    else
                    {
                        int iRtaSendImage = myobj.TORREYSendImageToIpad(client, PathImage);

                        if (iRtaSendImage > 0)
                        {
                            string sNoImage = "00000001" + "\t" + "\n";

                            char chk = new CheckSum().ChkSum(sNoImage);
                            sNoImage += chk + "\r";

                            client.Send(Encoding.GetEncoding(437).GetBytes(sNoImage));

                            Console.WriteLine("Dato enviado: {0}", sNoImage);
                            //ReturnDataString = "LP" + "0" + "\t" + iRtaSendImage + "\t" + "\n";
                        }
                    }
                }
                else
                {
                    productosTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                    productosTableAdapter.Connection.CreateCommand().CommandText = "Select * from Productos Order by Codigo";
                    productosTableAdapter.Fill(baseDeDatosDataSet.Productos);
                    baseDeDatosDataSet.Productos.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Productos.CodigoColumn };

                    ReturnDataString = "";

                    foreach (DataRow dr in baseDeDatosDataSet.Productos.Rows)
                    {
                        ReturnDataString += dr["id_producto"].ToString() + "\t" + Variable.validar_salida(dr["Nombre"].ToString(), 2) + "\t" + "P" + "\t" + dr["Codigo"].ToString() + "\t" + "\n";
                    }
                    ReturnDataString = "LP" + baseDeDatosDataSet.Productos.Rows.Count + "\t" + ReturnDataString;
                }
            }
            else
            {
                ReturnDataString = "LP" + "0" + "\t" + "2" + "\t" + "\n";           //Error formato datos
            }

            return Encoding.GetEncoding(437).GetBytes(ReturnDataString);
        }

        /// <summary>
        /// Obtener datos de Oferta
        /// </summary>
        /// <param name="CadenaComando"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        private byte[] pCommand_LO(string CadenaComando)
        {
            string SubMensaje = CadenaComando.Substring(2);
            string[] DatoRecibido = SubMensaje.Split(CharSeparador);
            string ReturnDataString = null;

            ValidateData validar = new ValidateData();

            if (validar.iValidateInt(DatoRecibido[0]) == 0)                         //TODO Verificr esta validacion
            {
                int idOferta = Convert.ToInt32(DatoRecibido[0]);

                if (idOferta == 0)
                {
                    ofertaTableAdapter.Fill(baseDeDatosDataSet.Oferta);

                    DataRow[] DT = baseDeDatosDataSet.Oferta.Select("borrado = " + false, "nombre ASC");

                    if (DT.Length > 0)
                    {
                        foreach (DataRow DR in DT)
                        {
                            ReturnDataString = ReturnDataString + DR["id_oferta"].ToString() + "\t" + DR["nombre"].ToString() + "\t" + DR["fecha_inicio"].ToString() +
                                "\t" + DR["fecha_fin"].ToString() + "\t" + DR["tipo_desc"].ToString() + "\t" + DR["Descuento"].ToString() + "\t" + DR["nVentas"].ToString() +
                                    "\t" + "\n";
                        }
                        ReturnDataString = "LO" + baseDeDatosDataSet.Oferta.Rows.Count.ToString() + "\t" + ReturnDataString;
                    }
                    else
                    {
                        ReturnDataString = "LO0" + "\t" + "\n";
                    }
                }
                else
                {
                    ofertaTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                    ofertaTableAdapter.Connection.CreateCommand().CommandText = "SELECT * FROM Oferta Order by id_oferta";
                    ofertaTableAdapter.Fill(baseDeDatosDataSet.Oferta);
                    baseDeDatosDataSet.Oferta.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Oferta.id_ofertaColumn };

                    DataRow DR = baseDeDatosDataSet.Oferta.Rows.Find(idOferta);

                    if (DR != null)
                    {
                        ReturnDataString = "LO" + "1" + "\t" + DR["id_oferta"].ToString() + "\t" + DR["nombre"].ToString() + "\t" + DR["fecha_inicio"].ToString() +
                                "\t" + DR["fecha_fin"].ToString() + "\t" + DR["tipo_desc"].ToString() + "\t" + DR["Descuento"].ToString() + "\t" + DR["nVentas"].ToString() +
                                    "\t" + "\n";
                    }
                    else
                    {
                        ReturnDataString = "LO0" + "\t" + "\n";
                    }
                }
            }
            else
            {
                ReturnDataString = "LO0" + "\t" + "\n";
            }

            return Encoding.GetEncoding(437).GetBytes(ReturnDataString);
        }

        /// <summary>
        /// Crear producto
        /// </summary>
        /// <param name="CadenaComando"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        private byte[] pCommand_NP(string CadenaComando, Socket client){

            string SubMensaje = CadenaComando.Substring(2);                      //quita el comando para analizar toda la trama
            string[] DatoRecibido = SubMensaje.Split(CharSeparador);
            string ReturnDataString = "";
                                        
            ValidateData CheckData = new ValidateData();
            DataProductFromIpad StructData = new DataProductFromIpad();
            int iError_Funct = 0;

            iError_Funct= CheckData.iValidateCreateNewProductFromIpad(DatoRecibido, ref StructData);

            if (iError_Funct == 0)
            {
                if (StructData.ibImagen == 1)
                {
                    CommandTorrey myobj = new CommandTorrey();

                    iError_Funct = myobj.TORREYGetImageFromIpad(client, Variable.appPath + "\\images\\");

                    if (iError_Funct > 0)
                    {
                        AppendToRichEditControl("Error en la recepcion de imagen");
                        ReturnDataString = "NP0" + "\t" + iError_Funct + "\t" + "\n";
                        return Encoding.GetEncoding(437).GetBytes(ReturnDataString);
                    }
                }

                productosTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                productosTableAdapter.Connection.CreateCommand().CommandText = "Select * from Productos Order by Codigo";
                productosTableAdapter.Fill(baseDeDatosDataSet.Productos);
                baseDeDatosDataSet.Productos.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Productos.CodigoColumn };

                DataRow dr = baseDeDatosDataSet.Productos.Rows.Find(StructData.sCodigo);
                
                if (dr == null)
                {

                    DataRow[] drd;

                    if (StructData.cTypeGrupo == 'D')
                    {
                        drd = baseDeDatosDataSet.Prod_detalle.Select("id_grupo = " + StructData.iGrupo + "AND NoPLU = " + StructData.iNoPlu);
                    }
                    else
                    {
                        drd = baseDeDatosDataSet.Prod_detalle.Select("id_bascula = " + StructData.iGrupo + "AND NoPLU = " + StructData.iNoPlu);
                    }

                    if (drd.Length == 0)
                    {
                        Crear_Producto(ref StructData);
                        ReturnDataString = "NP" + StructData.iId.ToString() + "\t" + "\n";// Crear_Trama_Producto(StructData.sCodigo);
                    }
                    else
                    {
                        ReturnDataString = "NP0" + "\t" +  "1" + "\t" + "\n";           //PLU Existente
                    }
                }
                else
                {
                    ReturnDataString = "NP0" + "\t" +  "0" + "\t" + "\n";           //Codigo existente
                }
            }
            else
            {
                ReturnDataString = "NP0" + "\t" + "2" + "\t" + "\n";      //Error formato de datos recibidos
            }

            return Encoding.GetEncoding(437).GetBytes(ReturnDataString);
        }

        /// <summary>
        /// Crear Oferta
        /// </summary>
        /// <param name="CadenaComando"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        private byte[] pCommand_NO(string CadenaComando, Socket client)
        {          
            string SubMensaje = CadenaComando.Substring(2);                      //quita el comando para analizar toda la trama
            string[] DatoRecibido = SubMensaje.Split(CharSeparador);
            string ReturnDataString = null;

            ValidateData CheckData = new ValidateData();
            DataOfertaFromIpad StructData = new DataOfertaFromIpad();

            int iError_Funct = 0;

            iError_Funct = CheckData.iValidateGO(DatoRecibido, ref StructData);

            if (iError_Funct == 0)
            {
                ofertaTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                ofertaTableAdapter.Connection.CreateCommand().CommandText = "Select * from Oferta Order by id_oferta";
                ofertaTableAdapter.Fill(baseDeDatosDataSet.Oferta);
                baseDeDatosDataSet.Oferta.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Oferta.nombreColumn };

                DataRow dr = baseDeDatosDataSet.Oferta.Rows.Find(StructData.sNombre);

                if (dr == null)
                {
                    Crear_Oferta(ref StructData);
                    ReturnDataString = "NO" + StructData.iId.ToString() + "\t" + "\n";
                }
                else
                {
                    ReturnDataString = "NO0" + "\t" + "8" + "\t" + "\n";           //Oferta existente
                }
            }
            else
            {
                ReturnDataString = "NO0" + "\t" + "2" + "\t" + "\n";      //Error formato de datos recibidos
            }

            return Encoding.GetEncoding(437).GetBytes(ReturnDataString);           
        }

        /// <summary>
        /// Crear Publicidad
        /// </summary>
        /// <param name="CadenaComando"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        private byte[] pCommand_NM(string CadenaComando, Socket client)
        {
            string SubMensaje = CadenaComando.Substring(2);                      //quita el comando para analizar toda la trama
            string[] DatoRecibido = SubMensaje.Split(CharSeparador);
            string ReturnDataString = null;

            ValidateData CheckData = new ValidateData();
            DataPublicidadFromIpad StructData = new DataPublicidadFromIpad();

            int iError_Funct = 0;
            iError_Funct = CheckData.iValidateGM(DatoRecibido, ref StructData);

            if (iError_Funct == 0)
            {
                publicidadTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                publicidadTableAdapter.Connection.CreateCommand().CommandText = "Select * from Publicidad Order by id_publicidad";
                publicidadTableAdapter.Fill(baseDeDatosDataSet.Publicidad);
                baseDeDatosDataSet.Publicidad.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Publicidad.TituloColumn };

                DataRow dr = baseDeDatosDataSet.Publicidad.Rows.Find(StructData.sTitulo);

                if (dr == null)
                {
                    Crear_Mensaje(ref StructData);
                    ReturnDataString = "NM" + StructData.iId.ToString() + "\t" + "\n";
                }
                else
                {
                    ReturnDataString = "NM0" + "\t" + "9" + "\t" + "\n";           //Codigo existente
                }
            }
            else
            {
                ReturnDataString = "NM0" + "\t" + "2" + "\t" + "\n";      //Error formato de datos recibidos
            }

            return Encoding.GetEncoding(437).GetBytes(ReturnDataString);
        }

        /// <summary>
        /// Crear Ingredientes
        /// </summary>
        /// <param name="CadenaComando"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        private byte[] pCommand_NI(string CadenaComando, Socket client)
        {
            string SubMensaje = CadenaComando.Substring(2);                      //quita el comando para analizar toda la trama
            string[] DatoRecibido = SubMensaje.Split(CharSeparador);
            string ReturnDataString = null;

            ValidateData CheckData = new ValidateData();
            DataIngredienteFromIpad StructData = new DataIngredienteFromIpad();

            int iError_Funct = 0;
            iError_Funct = CheckData.iValidateGI(DatoRecibido, ref StructData);

            if (iError_Funct == 0)
            {
                ingredientesTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                ingredientesTableAdapter.Connection.CreateCommand().CommandText = "Select * from Publicidad Order by id_publicidad";
                ingredientesTableAdapter.Fill(baseDeDatosDataSet.Ingredientes);
                baseDeDatosDataSet.Ingredientes.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Ingredientes.NombreColumn };

                DataRow dr = baseDeDatosDataSet.Ingredientes.Rows.Find(StructData.sNombre);

                if (dr == null)
                {
                    Crear_Ingrediente(ref StructData);
                    ReturnDataString = "NI" + StructData.iId.ToString() + "\t" + "\n";
                }
                else
                {
                    ReturnDataString = "NI0" + "\t" + "\n";           //Codigo existente
                }
            }

            return Encoding.GetEncoding(437).GetBytes(ReturnDataString);
        }

        /// <summary>
        /// Crear vendedor
        /// </summary>
        /// <param name="CadenaComando"></param>
        /// <returns></returns>
        private byte[] pCommand_NV(string CadenaComando)
        {

            string SubMensaje = CadenaComando.Substring(2);                      //quita el comando para analizar toda la trama
            string[] DatoRecibido = SubMensaje.Split(CharSeparador);
            string ReturnDataString = "";

            ValidateData CheckData = new ValidateData();
            DataVendedorFromIpad StructData = new DataVendedorFromIpad();
            
            int iError_Funct = 0;
            iError_Funct = CheckData.iValidateGV(DatoRecibido, ref StructData);

            if (iError_Funct == 0)
            {
                vendedorTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                vendedorTableAdapter.Connection.CreateCommand().CommandText = "Select * from Vendedor Order by Nombre";
                vendedorTableAdapter.Fill(baseDeDatosDataSet.Vendedor);
                baseDeDatosDataSet.Vendedor.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Vendedor.NombreColumn };

                DataRow dr = baseDeDatosDataSet.Vendedor.Rows.Find(StructData.sNombre);

                if (dr == null)
                {
                    Crear_Vendedor(ref StructData);
                    ReturnDataString = "NV" + StructData.iId.ToString() + "\t" + "\n";
                }
                else
                {
                    ReturnDataString = "NV0" + "\t" + "10" + "\n";           //Vendedor existente
                }
            }
            else
            {
                ReturnDataString = "NV0" + "\t" + "2" + "\t" + "\n";      //Error formato de datos recibidos
            }

            return Encoding.GetEncoding(437).GetBytes(ReturnDataString);
        }

        /// <summary>
        /// Actualizar producto
        /// </summary>
        /// <param name="CadenaComando"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        private byte[] pCommand_GP(string CadenaComando, Socket client)
        {
            string SubMensaje = CadenaComando.Substring(2);                      //quita el comando para analizar toda la trama
            string[] DatoRecibido = SubMensaje.Split(CharSeparador);
            string ReturnDataString = "";

            ValidateData CheckData = new ValidateData();
            DataProductFromIpad StructData = new DataProductFromIpad();
            int iError_Funct = 0;

            iError_Funct = CheckData.iValidateCreateNewProductFromIpad(DatoRecibido, ref StructData);

            if (iError_Funct == 0)
            {
                if (StructData.ibImagen == 1)
                {
                    CommandTorrey myobj = new CommandTorrey();

                    iError_Funct = myobj.TORREYGetImageFromIpad(client, Variable.appPath + "\\images\\");

                    if (iError_Funct > 0)
                    {
                        AppendToRichEditControl("Error en la recepcion de imagen");
                        ReturnDataString = "GP0" + "\t" + iError_Funct + "\t" + "\n";
                        return Encoding.GetEncoding(437).GetBytes(ReturnDataString);
                    }
                }

                productosTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                productosTableAdapter.Connection.CreateCommand().CommandText = "Select * from Productos Order by Codigo";
                productosTableAdapter.Fill(baseDeDatosDataSet.Productos);
                baseDeDatosDataSet.Productos.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Productos.CodigoColumn };

                DataRow dr = baseDeDatosDataSet.Productos.Rows.Find(StructData.sCodigo);

                if (dr != null)
                {

                    DataRow[] drd;

                    //if (StructData.cTypeGrupo == 'D')                    
                    drd = baseDeDatosDataSet.Prod_detalle.Select("NoPLU = " + StructData.iNoPlu);

                    foreach (DataRow ds in drd)
                    {
                        if (StructData.cTypeGrupo == 'D')
                        {
                            if (Convert.ToInt32(ds["id_grupo"].ToString()) == StructData.iGrupo && Convert.ToInt32(ds["id_producto"].ToString()) != StructData.iId)
                            {
                                ReturnDataString = "GP0" + "\t" + "1" + "\t" + "\n";           //PLU Existente
                                return Encoding.GetEncoding(437).GetBytes(ReturnDataString);
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(ds["id_bascula"].ToString()) == StructData.iGrupo && Convert.ToInt32(ds["id_producto"].ToString()) != StructData.iId)
                            {
                                ReturnDataString = "GP0" + "\t" + "1" + "\t" + "\n";           //PLU Existente
                                return Encoding.GetEncoding(437).GetBytes(ReturnDataString);
                            }
                        }
                    }

                    Modifica_Producto(ref StructData);
                    ReturnDataString = "GP" + +StructData.iId + "\t" + "\n";
                }
            }
            else
            {
                ReturnDataString = "GP0" + "\t" + "2" + "\t" + "\n";      //Error formato de datos recibidos
            }

            return Encoding.GetEncoding(437).GetBytes(ReturnDataString);
        }

        /// <summary>
        /// Modificar vendedor
        /// </summary>
        /// <param name="CadenaComando"></param>
        /// <returns></returns>
        private byte[] pCommand_GV(string CadenaComando)
        {

            string SubMensaje = CadenaComando.Substring(2);                      //quita el comando para analizar toda la trama
            string[] DatoRecibido = SubMensaje.Split(CharSeparador);
            string ReturnDataString = "";

            ValidateData CheckData = new ValidateData();
            DataVendedorFromIpad StructData = new DataVendedorFromIpad();

            int iError_Funct = 0;
            iError_Funct = CheckData.iValidateGV(DatoRecibido, ref StructData);

            if (iError_Funct == 0)
            {
                vendedorTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                vendedorTableAdapter.Connection.CreateCommand().CommandText = "Select * from Vendedor Order by id_vendedor";
                vendedorTableAdapter.Fill(baseDeDatosDataSet.Vendedor);
                baseDeDatosDataSet.Vendedor.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Vendedor.id_vendedorColumn };

                DataRow dr = baseDeDatosDataSet.Vendedor.Rows.Find(StructData.iId);

                if (dr != null)
                {
                    Modifica_Vendedor(ref StructData);
                    ReturnDataString = "GV" + StructData.iId.ToString() + "\t" + "\n";
                }
                else
                {
                    ReturnDataString = "GV0" + "\t" + "11" + "\t" +"\n";           //Vendedor No existente
                }
            }
            else
            {
                ReturnDataString = "GV0" + "\t" + "2" + "\t" + "\n";      //Error formato de datos recibidos
            }

            return Encoding.GetEncoding(437).GetBytes(ReturnDataString);
        }

        /// <summary>
        /// Leer carpeta
        /// </summary>
        /// <param name="CadenaComando"></param>
        /// <returns></returns>
        private byte[] pCommand_LC(string CadenaComando)
        {
            string SubMensaje = CadenaComando.Substring(2); 
            string[] DatoRecibido = SubMensaje.Split(CharSeparador);
            string ReturnDataString = null;
            int size_reg = 0;

            ValidateData validar = new ValidateData();
            DataLeerCarpetaFromIpad StructData = new DataLeerCarpetaFromIpad();

            int iError_Funct = 0;
            iError_Funct = validar.iValidateLC(DatoRecibido, ref StructData);

            if (iError_Funct == 0)
            {
                productosTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                productosTableAdapter.Connection.CreateCommand().CommandText = "SELECT * FROM Producto Order by id_producto";
                productosTableAdapter.Fill(baseDeDatosDataSet.Productos);
                baseDeDatosDataSet.Productos.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Productos.id_productoColumn };

                prod_detalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                prod_detalleTableAdapter.Connection.CreateCommand().CommandText = "SELECT * FROM Prod_detalle WHERE id_grupo = 0";
                prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);

                Conec.CadenaSelect = "SELECT * FROM carpeta_detalle WHILE (id_grupo = " + StructData.idGrupo.ToString() + ")";

                carpeta_detalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                carpeta_detalleTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                carpeta_detalleTableAdapter.Fill(baseDeDatosDataSet.carpeta_detalle);

                carpeta_detalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                carpeta_detalleTableAdapter.Connection.CreateCommand().CommandText = "SELECT * FROM carpeta Where id_padre = " + StructData.iIdCarpeta.ToString();
                carpeta_detalleTableAdapter.Fill(baseDeDatosDataSet.carpeta_detalle);
                
                DataRelation Producto_Detalle = baseDeDatosDataSet.Relations["ProductosProd_detalle"];
                DataRelation Carpetas_Detalle = baseDeDatosDataSet.Relations["carpetacarpeta_detalle"];
                               
                DataRow[] DR_Selec;

                if (StructData.cTipo == 'D')
                {
                    DR_Selec = baseDeDatosDataSet.carpeta_detalle.Select("id_grupo = " + StructData.idGrupo + "AND id_padre = " + StructData.iIdCarpeta);
                }
                else
                {
                    DR_Selec = baseDeDatosDataSet.carpeta_detalle.Select("id_bascula = " + StructData.idGrupo + "AND id_padre = " + StructData.iIdCarpeta);
                }

                if (DR_Selec != null)
                {
                    ReturnDataString = "";

                    foreach (DataRow dr in DR_Selec)
                    {
                        ReturnDataString += dr["ID"].ToString() + "\t" + dr["Nombre"].ToString() + "\t" + "C" + "\t" + "\n";
                        size_reg++;
                    }
                    foreach (DataRow DA in baseDeDatosDataSet.Productos.Rows)
                    {
                        foreach (DataRow PR in DA.GetChildRows(Producto_Detalle))
                        {
                            if (StructData.cTipo == 'D')
                            {
                                if (Convert.ToInt32(PR["id_grupo"].ToString()) == StructData.idGrupo && Convert.ToInt32(PR["id_carpeta"].ToString()) == StructData.iIdCarpeta)
                                {
                                    ReturnDataString += DA["id_producto"].ToString() + "\t" + DA["Nombre"].ToString() + "\t" + "P" + "\t" + PR["codigo"].ToString() + "\t" + "\n";
                                    size_reg++;
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(PR["id_bascula"].ToString()) == StructData.idGrupo && Convert.ToInt32(PR["id_carpeta"].ToString()) == StructData.iIdCarpeta)
                                {
                                    ReturnDataString += DA["id_producto"].ToString() + "\t" + DA["Nombre"].ToString() + "\t" + "P" + "\t" + PR["codigo"].ToString() + "\t" + "\n";
                                    size_reg++;
                                }
                            }
                        }
                    }

                    if (size_reg > 0)
                    {
                        ReturnDataString = "LC" + size_reg + "\t" + ReturnDataString;
                    }
                    else
                    {
                        ReturnDataString = "LC" + "0" + "\t" + "\n";
                    }
                }
                else
                {
                    ReturnDataString = "LC" + "0" + "\t" + "\n";
                }
            }
            else
            {
                ReturnDataString = "LC0" + "\t" + "2" + "\t" + "\n";      //Error formato de datos recibidos
            }

            return Encoding.GetEncoding(437).GetBytes(ReturnDataString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CadenaComando"></param>
        /// <returns></returns>
        private byte[] pCommand_VP(string CadenaComando)
        {
            string ReturnDataString = "VP";
            string SubMensaje = CadenaComando.Substring(2);
            string[] DatoRecibido = SubMensaje.Split(CharSeparador);

            if (string.Compare(DatoRecibido[0], Variable.password) == 0)
            {
                ReturnDataString += "OK\t";
            }
            else
            {
                ReturnDataString += "0\t";
            }

            ReturnDataString += "0\t"; //Mred = 1 esta en red
            ReturnDataString += Variable.idioma.ToString() + "\t";
            if (Variable.unidad == 0)
            {
                ReturnDataString += "gr\t\n";
            }
            else
            {
                ReturnDataString += "lb\t\n";
            }

            return Encoding.GetEncoding(437).GetBytes(ReturnDataString);
        }

        private byte[] pCommand_CX(string CadenaComando)
        {
            string ReturnDataString = "CX";
            ReturnDataString += "00" + "\t" + "\n";
            return Encoding.GetEncoding(437).GetBytes(ReturnDataString);
        }

        #endregion

        public static string EliminaAcentos(string texto)
        {
            byte[] tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(texto);
            return System.Text.Encoding.UTF8.GetString(tempBytes);
		}
			 
        /// <summary>
        /// Obtener la ruta de la imagen de un prodcuto en especifica
        /// </summary>
        /// <param name="Codigo_buscado"></param>
        /// <returns></returns>
        public string ReadPatchImage(int iId_buscado)
        {
            string DatosLeido = "";

            productosTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            productosTableAdapter.Connection.CreateCommand().CommandText = "Select * from Productos Order by id_producto";
            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);
            baseDeDatosDataSet.Productos.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Productos.id_productoColumn };

            DataRow dr = baseDeDatosDataSet.Productos.Rows.Find(iId_buscado);
            if (dr != null)
            {
                DatosLeido = dr["imagen1"].ToString();
            }
            return DatosLeido;
        }                      

        /// <summary>
        /// Crear trama a enviar a bascula para grabar producto
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string Crear_Trama_Producto(string codigo)
        {
            int pos_imagen;
            string Nombre_imagen = "";
            string Variable_frame = null;

            NumberFormatInfo frt = new NumberFormatInfo();
            frt.NumberDecimalDigits = Variable.n_decimal;
            frt.NumberDecimalSeparator = Variable.c_decimal.ToString();
            
            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);
            baseDeDatosDataSet.Productos.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Productos.CodigoColumn };

            DataRow dr = baseDeDatosDataSet.Productos.Rows.Find(codigo);
            if (dr != null)
            {
                string[] fecha_bascula = dr["actualizado"].ToString().Split(' ');
                pos_imagen = dr["imagen1"].ToString().LastIndexOf('\\');
                
                if (pos_imagen > 0){ 
                    Nombre_imagen = Variable.Ruta_SDCard + dr["imagen1"].ToString().Substring(pos_imagen + 1); 
                }

                decimal f_precio = Convert.ToDecimal(dr["Precio"].ToString(), frt);

                Variable_frame = "";
                Variable_frame += dr["id_producto"].ToString() + "\t";                           //id_producto
                Variable_frame += dr["Codigo"].ToString() + "\t";                                // codigo
                Variable_frame += dr["NoPlu"].ToString() + "\t";                                 // numero de PLUs
                Variable_frame += Variable.validar_salida(dr["Nombre"].ToString(), 2) + "\t";    //Nombre
                Variable_frame += string.Format(CultureInfo.CreateSpecificCulture(Variable.idioma2), Variable.F_Decimal, f_precio) + "\t";  //precio
                Variable_frame += Nombre_imagen + "\t";                                          // imagen
                Variable_frame += dr["TipoId"].ToString() + "\t";                                // tipo_id
                Variable_frame += Convert.ToString(dr["PrecioEditable"].ToString()) + "\t";      // precioeditable
                Variable_frame += dr["CaducidadDias"].ToString() + "\t";                         // caducidad
                Variable_frame += dr["Impuesto"].ToString() + "\t";                              // impuesto                       
                Variable_frame += "0" + "\t";                                                    // info nutricional
                Variable_frame += "0" + "\t";                                                    //info adicional
                Variable_frame += string.Format(Variable.F_Fecha, Convert.ToDateTime(fecha_bascula[2])) + "\t"; // actualizacion
                Variable_frame += "0" + "\t";                                                    // tara
                Variable_frame += dr["Mutiplo"].ToString() + "\t";                               // multiplo
                Variable_frame += dr["publicidad1"].ToString() + "\t";   // publicidad1
                Variable_frame += dr["publicidad2"].ToString() + "\t";   // publicidad2
                Variable_frame += dr["publicidad3"].ToString() + "\t";   // publicidad1
                Variable_frame += dr["publicidad4"].ToString() + "\t";   // publicidad2 
                Variable_frame += dr["oferta"].ToString() + "\t";   //Oferta
                Variable_frame += "0" + "\t";   //Grupo
                Variable_frame += "0" + "\t";   //bandimagen
                Variable_frame += "0" + "\t";   //IdCarpeta
            }
            return Variable_frame + "\n";
        }

        /// <summary>
        /// Crear trama a enviar a bascula para grabar precio
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string Crear_Trama_Precio(object clave)
        {
            string Variable_frame = null;
            NumberFormatInfo frt = new NumberFormatInfo();
            frt.NumberDecimalDigits = Variable.n_decimal;
            frt.NumberDecimalSeparator = Variable.c_decimal.ToString();

            prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);
            baseDeDatosDataSet.Prod_detalle.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Prod_detalle.id_grupoColumn, baseDeDatosDataSet.Prod_detalle.id_productoColumn };

            DataRow dr = baseDeDatosDataSet.Prod_detalle.Rows.Find(clave);
            if (dr != null)
            {
                decimal f_precio = Convert.ToDecimal(dr["Precio"].ToString(), frt);

                Variable_frame = "";
                Variable_frame = Variable_frame + dr["id_grupo"].ToString() + "\t";                           //id_grupo o Grupo
                Variable_frame = Variable_frame + dr["id_producto"].ToString() + "\t";                                // id_producto
                Variable_frame = Variable_frame + string.Format(CultureInfo.CreateSpecificCulture(Variable.idioma2), Variable.F_Decimal, f_precio) + "\t";  //precio
            }
            return Variable_frame + "\n";
        }

        /// <summary>
        /// Crear trama a enviar a bascula para grabar publicidad
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string Crear_Trama_Publicidad(int codigo)
        {
            string Variable_frame = null;
            publicidadTableAdapter.Fill(baseDeDatosDataSet.Publicidad);
            baseDeDatosDataSet.Publicidad.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Publicidad.id_publicidadColumn };

            DataRow dr = baseDeDatosDataSet.Productos.Rows.Find(codigo);
            if (dr != null)
            {

                Variable_frame = "";
                Variable_frame = Variable_frame + dr["id_publicidad"].ToString() + "\t"; // id_publicidad
                Variable_frame = Variable_frame + Variable.validar_salida(dr["Titulo"].ToString(), 2) + "\t"; //Titulo
                Variable_frame = Variable_frame + Variable.validar_salida(dr["Mensaje"].ToString(), 2) + "\t"; //Mensaje
            }
            return Variable_frame + "\n";
        }

        /// <summary>
        /// Crear trama a enviar a bascula para grabar oferta
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="ngrupo"></param>
        /// <returns></returns>
        private string Crear_Trama_Oferta(int codigo, string ngrupo)
        {
            string Variable_frame = null;
            ofertaTableAdapter.Fill(baseDeDatosDataSet.Oferta);
            baseDeDatosDataSet.Oferta.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Oferta.id_ofertaColumn };

            DataRow dr = baseDeDatosDataSet.Productos.Rows.Find(codigo);
            if (dr != null)
            {

                Variable_frame = "";
                Variable_frame = Variable_frame + ngrupo + "\t";  // Grupo
                Variable_frame = Variable_frame + dr["id_oferta"].ToString() + "\t"; // codigo
                Variable_frame = Variable_frame + Variable.validar_salida(dr["nombre"].ToString(), 2) + "\t"; //Nombre
                Variable_frame = Variable_frame + "0" + "\t";  // producto
                Variable_frame = Variable_frame + dr["fecha_inicio"].ToString() + "\t";  // Fecha de inicio
                Variable_frame = Variable_frame + dr["fecha_fin"].ToString() + "\t";   // fecha termino
                Variable_frame = Variable_frame + dr["tipo_desc"].ToString() + "\t"; // tipo de descuento
                Variable_frame = Variable_frame + dr["Descuento"].ToString() + "\t";    // descuento                     
                Variable_frame = Variable_frame + "0" + "\t";
            }
            return Variable_frame + "\n";
        }

        /// <summary>
        /// Crear trama a enviar a bascula para grabar Ingrediente
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string Crear_Trama_Ingrediente(int codigo)
        {
            string Variable_frame = null;
            ingredientesTableAdapter.Fill(baseDeDatosDataSet.Ingredientes);
            baseDeDatosDataSet.Ingredientes.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Ingredientes.id_ingredienteColumn };

            DataRow dr = baseDeDatosDataSet.Ingredientes.Rows.Find(codigo);
            if (dr != null)
            {

                Variable_frame = "";
               // Variable_frame = Variable_frame + ngrupo + "\t";  // Grupo
                Variable_frame = Variable_frame + dr["id_ingrediente"].ToString() + "\t"; // No.ingrediente
                Variable_frame = Variable_frame + Variable.validar_salida(dr["Nombre"].ToString(), 2) + "\t"; //Nombre
                Variable_frame = Variable_frame + Variable.validar_salida(dr["Informacion"].ToString(), 2) + "\t";  //informacion               
            }
            return Variable_frame + "\n";
        }

        /// <summary>
        /// Crear trama a enviar a bascula para grabar vendedor
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        private string Crear_Trama_Vendedor(int codigo)
        {
            string Variable_frame = null;

            NumberFormatInfo frt = new NumberFormatInfo();
            frt.NumberDecimalDigits = Variable.n_decimal;
            frt.NumberDecimalSeparator = Variable.c_decimal.ToString();

            vendedorTableAdapter.Fill(baseDeDatosDataSet.Vendedor);
            baseDeDatosDataSet.Vendedor.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Vendedor.id_vendedorColumn };

            DataRow dr = baseDeDatosDataSet.Vendedor.Rows.Find(codigo);
            if (dr != null)
            {
                decimal f_precio = Convert.ToDecimal(dr["Meta_Ventas"].ToString(), frt);

                Variable_frame = "";
                Variable_frame = Variable_frame + dr["id_vendedor"].ToString() + "\t";                           //No. Vendedor
                Variable_frame = Variable_frame + Variable.validar_salida(dr["Nombre"].ToString(), 2) + "\t";    //Nombre                
                Variable_frame = Variable_frame + dr["Meta_Enable"].ToString() + "\t"; //Habilitar Ventas
                Variable_frame = Variable_frame + dr["Msj_Enable"].ToString() + "\t";  //Habilitar Mensaje
                Variable_frame = Variable_frame + string.Format(CultureInfo.CreateSpecificCulture(Variable.idioma2), Variable.F_Decimal, f_precio) + "\t";  //Monto de meta de venta
                Variable_frame = Variable_frame + dr["publicidad1"].ToString() + "\t";   // publicidad1
                Variable_frame = Variable_frame + dr["publicidad2"].ToString() + "\t";   // publicidad2
            }
            return Variable_frame + "\n";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Reg_Enviado"></param>
        /// <param name="bascula"></param>
        /// <param name="Grupo"></param>
        private void Crear_DetalleProducto(string[] Reg_Enviado, long bascula, long Grupo)
        {
            string[] DatosNuevos;

            //for (int ii = 0; ii < Reg_Enviado.Length; ii++)
            //{
                DatosNuevos = Reg_Enviado;//Reg_Enviado[ii].Split("\t");

                DataRow dr = baseDeDatosDataSet.Prod_detalle.NewRow();

                dr.BeginEdit();
                dr["id_bascula"] = bascula;
                dr["id_grupo"] = Grupo;
                dr["id_producto"] = DatosNuevos[0]; ;
                dr["codigo"] = DatosNuevos[1]; ;
                dr["precio"] = Convert.ToDecimal(DatosNuevos[4]);
                dr.EndEdit();

                prod_detalleTableAdapter.Update(dr);
                baseDeDatosDataSet.AcceptChanges();

                Conec.CadenaSelect = "INSERT INTO Prod_detalle " +
                "(id_bascula,id_grupo,id_producto, codigo, Precio)" +
               "VALUES (" + bascula + "," +   //id_bascula
                             Grupo + "," +   //id_grupo
                             Convert.ToInt32(DatosNuevos[0]) + "," +     // id_producto
                             Convert.ToInt32(DatosNuevos[1]) + "," +  //codigo
                             Convert.ToDecimal(DatosNuevos[4]) + ")"; //Precio

                Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Prod_detalle.TableName);
            //}
        }

        #endregion
                
        #region Envio dato bascula
        /// <summary>
        /// Enviar datos hacia cada bascula
        /// </summary>
        /// <param name="mensaje"></param>
        /// <param name="Dato"></param>
        private void Enviando_Datos_Bascula(string mensaje, byte[] Dato)
        {
            string[] DatoEnviado = mensaje.Split((char)9);

            string Dato_Trama;

            Dato_Trama = Crear_Trama_Producto(DatoEnviado[1]);

            string[] DatoEnviado2 = Dato_Trama.Split((char)9);

            char chk = new CheckSum().ChkSum(Dato_Trama);
            Dato_Trama = "GP01" + Dato_Trama + chk + "\r";

            CommandTorrey myobj = new CommandTorrey();

            for (int pos = 0; pos < myScale.Length; pos++)
            {
                if (myScale[pos].gpo == Convert.ToInt32(DatoEnviado[19]))
                {
                    Socket Cliente_bascula = Cte.conectar(myScale[pos].ip, 50036);  //, Variable_frame, ref Dato_Recivido);

                    if (Cliente_bascula != null)
                    {
                        Envio_Trama(Cliente_bascula, DatoEnviado2, myScale[pos].idbas, myScale[pos].gpo, Dato_Trama, myScale[pos].ip);
                        Cte.desconectar(ref Cliente_bascula);
                        productosTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        productosTableAdapter.Connection.CreateCommand().CommandText = "Select * from Productos Order by Codigo";
                        productosTableAdapter.Fill(baseDeDatosDataSet.Productos);
                        baseDeDatosDataSet.Productos.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Productos.CodigoColumn };

                        DataRow dr = baseDeDatosDataSet.Productos.Rows.Find(DatoEnviado[1]);
                        if (dr != null)
                        {
                            myobj.TORREYSendImagesToScale(myScale[pos].ip, dr["imagen1"].ToString(),"Product",Cliente_bascula);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No se pudo accesar bascula: {0}", myScale[pos].ip);
                    }
                }
            }
        }

        /// <summary>
        /// Enviar a trama hacia la bascula socket TCP
        /// </summary>
        /// <param name="SkBascula"></param>
        /// <param name="DatoEnviado"></param>
        /// <param name="nBascula"></param>
        /// <param name="nGrupo"></param>
        /// <param name="Trama_Enviada"></param>
        /// <param name="ipbascula"></param>
        private void Envio_Trama(Socket SkBascula, string[] DatoEnviado, long nBascula, long nGrupo, string Trama_Enviada, string ipbascula)
        {
            string[] Dato_Recibido = null;

            Cte.Envio_Dato(ref SkBascula, ipbascula, Trama_Enviada, ref Dato_Recibido);
            if (Dato_Recibido != null)
            {
                if (Dato_Recibido[0].IndexOf("Error") >= 0) Envio_Trama(SkBascula, DatoEnviado, nBascula, nGrupo, Trama_Enviada, ipbascula);
                if (Dato_Recibido[0].IndexOf("Ok") >= 0) Crear_DetalleProducto(DatoEnviado, nBascula, nGrupo);
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        void CloseSockets()
        {
            if (m_mainSocket != null)
            {
                m_mainSocket.Close();
            }
            Socket workerSocket = null;
            for (int i = 0; i < m_workerSocketList.Count; i++)
            {
                workerSocket = (Socket)m_workerSocketList[i];
                if (workerSocket != null)
                {
                    workerSocket.Close();
                    workerSocket = null;
                }
            }
            m_workerSocketList.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iNumberClientCurrent"></param>
        void UpdateClientBascula(int iNumberClientCurrent)
        {
            Listado_bascula.Nodes.Clear();
            int i = iNumberClientCurrent - 1;

            Socket workerSocket = (Socket)m_workerSocketList[i];

            if (workerSocket != null)
            {
                if (workerSocket.Connected)
                {
                    string direccionip = IPAddress.Parse(((IPEndPoint)workerSocket.RemoteEndPoint).Address.ToString()).ToString();
                    TreeNode ne = new TreeNode(direccionip, 0, 0);
                    Listado_bascula.Nodes.Add(ne);
                    string msg = "Cliente No: " + iNumberClientCurrent + " , " + direccionip + " Conectado...\n";
                    AppendToRichEditControl(msg);
                }
            }
        }

        #region Botones
        void ButtonStartListenClick(object sender, System.EventArgs e)
        {
            inicia_listen();
        }
        void ButtonStopListenClick(object sender, System.EventArgs e)
        {
            CloseSockets();
            UpdateControls(false);
            richTextBoxReceivedMsg.Clear();
           // richTextBoxSendMsg.Clear();
        }

        void ButtonCloseClick(object sender, System.EventArgs e)
        {
            this.Hide();
            this.Visible = false;
        }

        private void btnClear_Click(object sender, System.EventArgs e)
        {
            richTextBoxReceivedMsg.Clear();
           // richTextBoxSendMsg.Clear();
        }

     /*   private void button1_Click(object sender, System.EventArgs e)
        {
            CloseSockets();
            UpdateControls(false);
            this.Close();
            this.Dispose();
        }*/
        #endregion

        private void f6Sincronizacion_Load(object sender, EventArgs e)
        {
          
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Oferta_Detalle' Puede moverla o quitarla según sea necesario.
            this.oferta_DetalleTableAdapter.Fill(this.baseDeDatosDataSet.Oferta_Detalle);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Vendedor' Puede moverla o quitarla según sea necesario.
            this.vendedorTableAdapter.Fill(this.baseDeDatosDataSet.Vendedor);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Ingredientes' Puede moverla o quitarla según sea necesario.
            this.ingredientesTableAdapter.Fill(this.baseDeDatosDataSet.Ingredientes);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Public_Detalle' Puede moverla o quitarla según sea necesario.
            this.public_DetalleTableAdapter.Fill(this.baseDeDatosDataSet.Public_Detalle);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.carpeta_detalle' Puede moverla o quitarla según sea necesario.
            this.carpeta_detalleTableAdapter.Fill(this.baseDeDatosDataSet.carpeta_detalle);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.carpeta' Puede moverla o quitarla según sea necesario.
           // this.carpetaTableAdapter.Fill(this.baseDeDatosDataSet.carpeta);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Oferta' Puede moverla o quitarla según sea necesario.
            this.ofertaTableAdapter.Fill(this.baseDeDatosDataSet.Oferta);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Prod_detalle' Puede moverla o quitarla según sea necesario.
            this.prod_detalleTableAdapter.Fill(this.baseDeDatosDataSet.Prod_detalle);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Publicidad' Puede moverla o quitarla según sea necesario.
            this.publicidadTableAdapter.Fill(this.baseDeDatosDataSet.Publicidad);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Productos' Puede moverla o quitarla según sea necesario.
            this.productosTableAdapter.Fill(this.baseDeDatosDataSet.Productos);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Grupo' Puede moverla o quitarla según sea necesario.
            this.grupoTableAdapter.Fill(this.baseDeDatosDataSet.Grupo);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Bascula' Puede moverla o quitarla según sea necesario.
            this.basculaTableAdapter.Fill(this.baseDeDatosDataSet.Bascula);

            ButtonStartListenClick(this.buttonStartListen,null);
            this.Hide();
            this.Visible = false;
        }
    }
}
