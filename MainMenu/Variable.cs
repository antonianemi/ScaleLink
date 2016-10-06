using System;
using System.IO;
using System.Configuration;
using System.Collections.Generic;

namespace MainMenu
{
	/// <summary>
	/// Descripción breve de Global.
	/// </summary>
	/// 
    public class Variable
    {

        public enum TipoBascula
        {
            WLS=1,
            WLABEL=2,
            WLSD=3
        }

        public static bool wls;
        public static bool wlsd;
        public static bool wlabel;

        public struct lplus
        {
            public string nombre;
            public string indice;
            public string codigo;
            public string numplu;
            public string precio;
            public string impu;
            public string depto;
            public string fcad;
            public string ingre;
            public string tararef;
            public string accesodirecto;
            public string idacceso;
            public string tipo;
            public string pub1;
            public string pub2;
            public string pub3;
            public string pub4;
            
        }
       
        public struct lbasc
        {
            public Int32 idbas;
            public Int32 gpo;
            public string nombre;
            public string modelo;
            public string ip;
            public string getway;
            public string nserie;
            public string cap;
            public string div;
            public string um;
            public int indice;
            public int tipo;
            public int baud;
            public string pto;
            public bool selec;            
        }

        public struct lgrupo
        {
            public Int32 ngpo;
            public string nombre;
            public string selec;
            public string Modelo;
        }
        public struct nodo_actual
        {
            public Int32 idbas;
            public Int32 gpo;
            public string nombre;            
            public string ip;
            public string Nserie;
            public int BAUD;
            public string COMM;
            public int tipo;
            public string Modelo;
        }
        public struct headers
        {
            public char centrado;               //C=centrado,I=izquierda,D=derecha
            public int tam;                     //1-6
            public string texto;                //texto maximo 18 caracteres
        }
        public struct formato
        {
            public int medio_imp;             //medio de impresion,0=papel,1=eti c/sepa
           // public int tipoCodigo;           //tipo de impresion por producto o por ticket ,0=por EAN,1=por UPC
            public int ncodigobar_xprod;          //codigo de barra seleccionado para imp x prod, 2= personalizado.
            public int ncodigobar_xticket;        //codigo de barra seleccionado para imp ticket, 2= personalizado.    
            public int for_papel_tipoimpre;   //formato para papel, 0 = formato1, 1= formato2, 2= formato personalizado
            public int for_ecsep_tipoimpre;   //formato para etiqueta con separador,0 = foramto1, 1= formato2, 2= formato personalizado
        }
        public struct formato_size
        {
            public int nformato;
            public string posdef;
            public string possize;
            public string ancho_medio;       //ancho para personalizado para medio
            public string largo_medio;       //largo para personalizado para medio
            public string separacion_medio;  //separacion para personalizado para medio
            public string nencabezado;
            public string ningrediente;                        
        }

        public static List<Scale> scales = new List<Scale>();//Representa el tipo de basculas que estan cargadas en ese momento.
       

        public static string appPath = System.IO.Directory.GetCurrentDirectory();   //"C:\\Prog-pls6\\PLSLIGHT\\SCALNET-CLIENTE-SERVIDOR\\SCALENET";					//directorio de trabajo
        
        // public static string appPath = System.IO.Deployment.Application.ApplicationDeployment.CurrentDeployment.DataDirectory;
        //public const string Motor = "Provider=Microsoft.Jet.OLEDB.4.0;Password='';User ID=Admin; Mode =ReadWrite;Data Source= "; 
               
        public static int year = System.DateTime.Now.Year;
        public static int mes = System.DateTime.Now.Month;
        public static int dia = System.DateTime.Now.Day;
        public static int hora = System.DateTime.Now.Hour;
        public static int minutos = System.DateTime.Now.Minute;
        public static int segundo = System.DateTime.Now.Second;

        /// <summary>
        /// VARIABLES PARA DATOS GENERALES DEL SISTEMA
        /// se usan en todo el sistema como parametros globales.
        /// </summary>
        //public static DateTime MyDateShort = new DateTime(year, mes, dia, hora, minutos, segundo);
        public static string user;		//nombre del usuario
        public static string password;	//password del usuario
        public static string privilegio;  //privilegio del usuario        
        public static int idioma = 0; // idioma del sistema para seleccion de mensaje

        public static TipoBascula TypeScale;


        public static int unidad = 0; // unidad de medida para el sistema
        public static int n_decimal = 2; // numero de decimal
        public static char c_decimal = '.'; // caracter del decimal
        public static int moneda = 0; // formeto seleccionado de moneda para el sistema 0: 2 dec, 1: 0 dec, 2: 1 decimal, 3: 3 decimales
        public static int ffecha = 0;
        public static string[] FOR_IDIOMA = new string[2] { "es-MX", "en-US" };
        public static string[] FOR_MONEDA = new string[4] { "{0:##0.#0}", "{0:####0}", "{0:###0.0}", "{0:##0.##0}" };
        public static string[] FOR_FORMAT = new string[4] { "##0.#0", "####0", "###0.0", "##0.##0" };
        public static string[] FOR_UM = new string[2] { "Kg", "Lb" };
        public static string idioma2 = FOR_IDIOMA[0]; //lenguaje ingles o español en-US o es-MX
        public static string umedida = FOR_UM[0]; // unidad de medida      
        public static string F_Decimal = FOR_MONEDA[0]; //Formato de decimal
        public static string F_Descuento = "{0:#0.#0}"; //Formato de descuento para oferta
        public static string F_Total = "{0:#,###,###,##0}"; // Formato de decimal de los totales
        public static string F_Fecha = "{0:yyyy-MM-dd}"; // formato de fecha para envio a bascula
        public static string F_Hora = "{0:HH:mm:ss}"; //formato de 24 hora para envio de bascula 
        public static bool envio_dato = false;		//bandera que indica si se envio algun dato
        public static bool clv_aceptada = false;		//Bandera que indica si se la clave es aceptada
        public static bool EnviarImagenesProductos;		//Bandera que indica si se la clave es aceptada
        public static string dias_semana = "";  //frecuencia en dias de la semana
        public static int pos_intervalo = 0;
        public static Int32 intervalo = 300000; //intervalo de frecuencia      
        public static string[] FOR_FECHAS = new string[2]{"{0:dd/MM/yyyy}","{0:MM/dd/yyyy}"};
        public static string[] CUST_DATE = new string[2] { "dd/MM/yyyy", "MM/dd/yyyy" };
        public static string Fecha_inicial = string.Format(FOR_FECHAS[0], DateTime.Now);
        public static string Fecha_final = string.Format(FOR_FECHAS[0], DateTime.MaxValue);
        public static string Hora_Inicial = string.Format("{0:HH:mm:ss tt}", DateTime.MinValue);
        public static string Hora_final = string.Format("{0:HH:mm:ss tt}", DateTime.MaxValue);
        public static string Hora_Intervalo = string.Format("{0:HH:mm:ss}", DateTime.Now);
        public static sbyte Activar_Frecuencia = 1;
        public static sbyte Habilitar_PubliWLSD = 1;
        public static bool Comando_Ipad = false;
        // para pasar las imagenes a la bascula o recibirla.
        public static int iImageIpad = 200;     /*Se debe declarar como constantes en el programa principal */
        public static int iImageScale = 235;
        public static int iSimageScale = 77;
        public static int iSsimageScale = 48;
        public static string Ruta_SDCard = "images/user/pcimages/";
        public static string Ruta_SPlash = "/sdcard/splash/";
        public static string Ruta_Logos = "/sdcard/logos/";
        public static string Ruta_Ads = "/sdcard/adds/";
        /// <summary>
        /// Declaraciones de Arreglos Globales
        /// para almacenar informacion de forma temporal que usa todo el sistema.
        /// </summary>
        /// 
        public static string[] velocidad = new string[] { "115200"};
        public static System.Collections.ArrayList port;  //Lista de Puertos Seriales
        public static string[,] SYS_MSJ = new string[500, 2];
        public static Int32[] list_tiempo = new Int32[] { 300000,600000, 900000, 1200000, 1800000, 3600000, 7200000, 10800000, 14400000, 60000, 30000 };
                                                         //5min.   10m      15m     20m     30m     1hr      2hr        3hr      4hr       1m     30s
       
        /// <summary>
        /// VARIABLES PARA DATOS GENERALES DE LAS BASCULSA.
        /// es exclusivo para los parametros correspondiente a las basculas
        /// </summary>
        public static int nind;			            //numero de ingrediente
        public static int ncod;			            //numero de codigo
        public static int nplu;			            //numero de plu
        public static long nidbas;		            //numero de ID de la bascula  
        public static long nsucursal = 0;           //Numero de la empresa
        public static string ppeso;					//capacidad Kg o Lb
        public static string Bascula;				//Nombre de la bascula
        public static string Nombre;				//Descripcion de la bascula 
        public static string Nserie;                //Numero de serie
        public static int Div_Min = 5;              // Division minima
        public static string Cap_Max = "20.000";    //Capacidad de la bascula
        public static string IP_Address;            //Direccion IP de la bascula
        public static string N_Grupo;               //Numero de Grupo
        public static string P_COMM;				//Numero de puerto serial
        public static int Buad;					//Velocidad del Baud	       
        public static string Empresa;				//Nombre de la empresa       

       // public static string frame;					//frame de mensaje
               
        /// <summary>
        /// VARIABLE PARA DATOS DE LA CONFIGURACION GENERAL DE LAS BASCULA
        /// </summary>
        public static string[] user_scroll = new string[5];		//scroll, 60 caracteres
        public static string[] user_splash = new string[5];     //splash, 60 caracteres
        public static string user_passadmin;			//password de administrador, 6 caracteres
        public static string user_passsuper;			//password supervisor, 4 caracters
        public static sbyte user_varios;				//permite productos varios, 1 caracter
        public static sbyte user_lockprecio;			//bloquea precio, 1 caracter
        public static sbyte user_devoluciones;		    //permite devoluciones con password, 1 caracter
        public static sbyte user_descuentos;			//permite descuentos con password, 1 caracter
        public static sbyte user_autoprint;            //permite autoimpresion, 1 caracter
        public static sbyte user_reprint;              //permite reimpresion, 1 caracter
        public static sbyte user_formfecha;            //formato de fecha, 1 caracter 0 = ddmmyyyy  1= mmddyyyy
        public static sbyte user_formhora;             //formato de hora, 1 caracter 0 = 12 horas 1= 24 horas
        public static sbyte user_activaprotector;       // activa las imagenes de splash
        public static sbyte user_activapublicidad;       // activa las imagenes de publicidad
        public static sbyte user_activalogo;       // activa las imagenes de logo
        /// <summary>
        /// VARIABLE PARA DATOS DE LA CONFIGURACION DEL IMPRESOR Y FORMATOS
        /// </summary>
        public static int user_contrastepapel;		//contraste papel,  1 caracter
        public static int user_contrasteetiqueta;	//contraste etiqueta, 1 caracter
        public static int user_retardoimpresion;    //retardo impresion, 1 caracter
        public static sbyte user_corrimientopieza;    //corrimiento pieza, 1 caracter
        public static string user_prefijo;				//numero de prefijo, 2 caracter
        public static string user_depto;				//numero de depto, 2 caracter
        public static sbyte user_nutri;                 //Imprimir nutriente 0=no habilitado 1=habilitado
        //public static sbyte user_numing;				//numero de renglones de ingredientes, 1 caracter
        //public static sbyte user_numenca;				//numero de encabezado, 1 caracter
        public static int user_Nformato_producto;  //numero de la plantilla asignada por producto
        public static int user_Nformato_ticket;    //numero de la plantilla asignada por ticket
        public static int user_EAN_UPCxProd;              // 'tipo de codigo EAN o UPC para producto, 14 caracteres
        public static int user_EAN_UPCxTicket;            // 'tipo de codigo EAN o UPC para ticket, 14 caracteres       
        public static string user_codigoxticket;        // 'formato codigo de barras ticket, 14 caracteres
        public static string user_codigoxprod;          // 'formato codigo de barras productos pesados, 14 caracteres
        public static string user_formato1_posdef;      //formato personalizado 1 pos_def
        public static string user_formato1_possize;     //size_def
        public static string user_formato2_posdef;      //formato personalizado 2 pos_def
        public static string user_formato2_possize;     //size_def
        public static string user_formato3_posdef;      //formato personalizado 3 pos_def
        public static string user_formato3_possize;     //size_def
        public static string user_formato4_posdef;      //formato personalizado 3 pos_def
        public static string user_formato4_possize;     //size_def
        public static formato user_formato;			    //tipo de formato standar o personalizado
        public static formato_size user_formatosize;    // definicion de tamaño de etiqueta y papael
        //public static formato_size user_formato2;
        /// <summary>
        /// VARIABLE DE DATOS PARA ENCABEZADOS Y TEXTOS
        /// </summary>
        public headers[] user_headers = new headers[10];        //encabezados
        public static string user_tpeso;				//texto peso, 16 caracteres
        public static string user_tprecio;			    //texto precio, 16 caracteres
        public static string user_ttotal;				//texto total, 16 caracteres
        public static string user_tfecha;				//texto fecha, 16 caracteres
        public static string user_tfechacad;            //texto fechacad, 16 caracteres
        public static string user_tvendedor;            //texto vendedor, 16 caracteres
        public static string user_tadicional;           //texto adicional, 28 caracteres
        public static string[] ingred = new string[501];    //contenido del ingrediente

        public Variable()
        {


            //switch (ConfigurationManager.AppSettings["TypeScale"].ToString())
            //{
            //    case "0":
            //        TypeScale = TipoBascula.WLABEL;
            //        break;
            //    case "1":
            //        TypeScale = TipoBascula.WLS;
            //        break;
            //    case "2":

            //        break;
            //}

            EnviarImagenesProductos = (ConfigurationManager.AppSettings["EnviaImagenes"].ToString() == "0") ? false : true;
            
        }




        /// <summary>
        /// Se encarga de cargar los tipos de basculas que estan registrados en el sistema.
        /// </summary>
        public static void cargarTiposBasculas()
        {

        }



        public static void Cargar_puertos()
        {

            port = new System.Collections.ArrayList();
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames()) port.Add(s);
            port.Sort(); 
            if (port.Count <= 0)
            {
                Console.Write(Variable.SYS_MSJ[95, Variable.idioma] + (char)10 + Variable.SYS_MSJ[96, Variable.idioma]);
            }
        }

        public void Cargar_Mensajes()
        {           
            FileStream fi = new FileStream(Variable.appPath + "\\SysTXT.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fi, System.Text.Encoding.Unicode);
            sr.BaseStream.Position = 0;

            char[] cc = new char[] { (char)9 };
            string renglon;

            string[] Msj_TXT = new string[3];
            int posicion;

            while ((renglon = sr.ReadLine()) != null)
            {
                Msj_TXT = renglon.Split(cc);
                posicion = Convert.ToInt32(Msj_TXT[0]);
                SYS_MSJ[posicion, 0] = Msj_TXT[1];
                SYS_MSJ[posicion, 1] = Msj_TXT[2];
            }
        }

        public static int iValidarImpuesto(string sImpuesto)
        {
            int iRtaFunct = 0;

            if (sImpuesto != "")
            {
                if (Convert.ToDouble(sImpuesto) > 99.99)
                {
                    iRtaFunct = 1;
                }
            }
            return iRtaFunct;
        }

        public static string validar_salida(string dato, int tipo)
        {
            string cadena = "";
            string validacion = "", cc;
            int c, n;

            n = dato.Length;
            if (n > 0)
            {
                switch (tipo)
                {
                    case 0: { validacion = "0123456789"; } break;   //solo numeros
                    case 1: { validacion = ".0123456789"; } break;   //solo numeros y punto decimal
                    case 2: { validacion = (char)32 + @"'£€!#$%&/()=@?*+\;,:._-.0123456789 abcdefghijklmnñopqrsutvuwxyzáéíóúïüABCDEFGHIJKLMNÑOPQRSTUVWXYZÁÉÍÓÚÜÏ"; } break; //texto libre
                    case 3: { validacion = "apcwxtdyr"; } break;      //código de barras xproducto
                    case 4: { validacion = "apnxtd"; } break;       //código de barras xticket
                    case 5: { validacion = "ABCDEFGHIJKL"; } break;
                    case 6: { validacion = "KkGgLlBb"; } break;
                    case 7: { validacion = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-abcdefghijklmnñopqrstuvwxyz"; } break;
                    case 8: { validacion = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-_ Ñabcdefghijklmnñopqrstuvwxyz"; } break;
                    case 9: { validacion = "-0123456789"; } break;   //solo numeros
                    case 10: { validacion = "-.0123456789"; } break;   //solo numeros y punto decimal
                
                }
                for (c = 0; c < n; c++)
                {
                    cc = dato.Substring(c, 1);
                    if (validacion.IndexOf(cc) >= 0)
                    {
                        switch (cc)
                        {
                            case "á": { cc = "a"; } break;
                            case "Á": { cc = "A"; } break;
                            case "é": { cc = "e"; } break;
                            case "É": { cc = "E"; } break;
                            case "í": { cc = "i"; } break;
                            case "Í": { cc = "I"; } break;
                            case "ó": { cc = "o"; } break;
                            case "Ó": { cc = "O"; } break;
                            case "ú": { cc = "u"; } break;
                            case "Ú": { cc = "U"; } break;
                            case "ï": { cc = "i"; } break;
                            case "Ï": { cc = "I"; } break;
                            case "ü": { cc = "u"; } break;
                            case "Ü": { cc = "U"; } break;
                            case "ñ": { cc = "n"; } break;
                            case "Ñ": { cc = "N"; } break;
                            case "€": { cc = "}"; } break;
                            case "£": { cc = "{"; } break;                            
                        }
                        cadena = cadena + cc;
                    }
                }
                if (tipo < 2)
                {   //solo numeros
                    cadena = cadena.ToString().Trim();
                }
                if (tipo == 2)
                {
                    cadena = cadena.Replace("''", Convert.ToChar(34).ToString()); //Para reemplazar doble apóstrofe con comilla

                    string texto_comparador = "";
                    for (int i = 0; i < 100; i++)
                    {
                        texto_comparador = texto_comparador + Convert.ToChar(34).ToString();
                        cadena = cadena.Replace(texto_comparador, Convert.ToChar(34).ToString());
                    }                    
                }
            }
            return (cadena);
        }
    }
}
