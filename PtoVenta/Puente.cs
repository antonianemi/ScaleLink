using System;
using System.IO;
using System.Data.OleDb;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MainMenu;
using MainMenu.BaseDeDatosDataSetTableAdapters;
using System.Net.Sockets;
using System.Net;


namespace PtoVenta
{
    public partial class Puente
    {
        ADOutil Conec = new ADOutil();
        Conexion Cte = new Conexion();
        Envia_Dato Env = new Envia_Dato();

        BaseDeDatosDataSet baseDeDatosDataSet = new BaseDeDatosDataSet();
        ProductosTableAdapter productosTableAdapter = new ProductosTableAdapter();
        VendedorTableAdapter vendedoresTableAdapter = new VendedorTableAdapter();
       
        public Puente()
        {
            readTextFile();
        }

        private void readTextFile()
        {
            string sPatchDir = @"C:\WLS"; 
            if (!System.IO.Directory.Exists(sPatchDir))
                System.IO.Directory.CreateDirectory(sPatchDir);
            
            string sPatchLogs = @"C:\WLS\WLS-Logs.txt";
            FileStream objLogs = new FileStream (sPatchLogs, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter strLogs = new StreamWriter (objLogs, System.Text.ASCIIEncoding.ASCII);

            string sPatch = @"C:\WLS\Productos.txt";

            string sFile = leerArchivoTXT(sPatch);
            
            char[] CharSeparador = new char[] { (char)10 };
            
            ConsoleSpiner spin = new ConsoleSpiner();

            List<string> ListIps = new List<string>();
            List<int> ListCodProduc = new List<int>();
            List<int> ListCodVendor = new List<int>();

            string today = DateTime.Now.ToShortTimeString() + " " +
                           DateTime.Now.ToShortDateString();

            if(sFile.Length > 0)
            {                   
                string[] DatoRecibido = sFile.Split(CharSeparador);

                strLogs.WriteLine("\n" + today + "Leyendo archivo \n" + sPatch + "\n\r");
                Console.Write("Leyendo productos.... ");

                int iNum = 0;
                int iTotal = 0;

                if (DatoRecibido[DatoRecibido.Length - 1] == "")
                    iTotal = DatoRecibido.Length - 1;
                else
                    iTotal = DatoRecibido.Length;

                foreach (string sOutput in DatoRecibido)
                {
                    string nsOutput = sOutput.Replace("\r","");
                    
                        if (validarIP(nsOutput, ref ListIps) == 2)
                        {
                            if (nsOutput.Length >= 64)
                            {
                                int iSave = guardarProductosEnBD(nsOutput, ref ListCodProduc);
                                strLogs.WriteLine(today + mensajeGuardarEnBD(iSave) + nsOutput);
                                iNum++;
                                spin.Turn(iNum, iTotal);
                            }
                            else
                                strLogs.WriteLine(today + "Error en lectura de producto, cadena incompleta " + nsOutput);
                        }
                        else
                            iTotal--;
                    
                }
                Console.SetCursorPosition(Console.CursorLeft + 5, Console.CursorTop);
                Console.WriteLine("");
                Console.WriteLine("Termino de leer " + iTotal + " productos");
            }
            else
                strLogs.WriteLine(today + "No existe archivo de productos " + sPatch);


            sPatch = @"C:\WLS\Vendedores.txt";
            sFile = leerArchivoTXT(sPatch);

            if (sFile.Length > 0)
            {
                string[] DatoRecibido = sFile.Split(CharSeparador);
                strLogs.WriteLine("     ..... ..... ..... ..... ..... ..... ..... ..... ..... .....     ");
                strLogs.WriteLine("\n" + today + "Leyendo vendedores: " + sPatch + "\n\n");
                Console.Write("\nLeyendo Archivo Vendedores.... ");

                int iNum = 0;
                int iTotal = 0;

                if (DatoRecibido[DatoRecibido.Length - 1] == "")
                    iTotal = DatoRecibido.Length - 1;
                else
                    iTotal = DatoRecibido.Length;

                foreach (string sOutput in DatoRecibido)
                {
                    string nsOutput = sOutput.Replace("\r", "");
                    if (nsOutput.Length >= 4)
                    {
                        int iSave = guardarVendorEnDB (nsOutput, ref ListCodVendor);
                        strLogs.WriteLine(today + mensajeGuardarEnBD(iSave) + nsOutput);
                        iNum++;
                        spin.Turn(iNum, iTotal);
                    }
                    else
                        strLogs.WriteLine(today + "Error en lectura de vendedor, cadena incompleta" + nsOutput);
                }
                
                Console.SetCursorPosition(Console.CursorLeft + 5, Console.CursorTop);
                Console.WriteLine("");
                Console.WriteLine("Termino de leer " + iTotal + " vendedores");
            }
            else
                strLogs.WriteLine(today + "No existe archivo  de vendedores  en " + sPatch );



            Console.WriteLine("\nInicia envio a las básculas\r");

            if (ListIps.Count > 0)
            {
                strLogs.WriteLine(today + "Se guardo con exito.\r");
                Env.vActualizar_Bascula_PV(ListIps, ListCodProduc, ListCodVendor);         
                strLogs.WriteLine(today + "Envio con exito a las basculas\r");
                Console.WriteLine("\nFin envio a las básculas\r");
                //Console.ReadLine();
            }
            else
                strLogs.WriteLine(today + "No hay datos para enviar.");

            strLogs.WriteLine("-------------------- -------------------- -------------------- -------------------- --------------------");
            strLogs.Close();
            objLogs.Close();
        }


        private string leerArchivoTXT(string sPatch)
        {
            if (System.IO.File.Exists(sPatch))
            {
                StreamReader objReader = new StreamReader(sPatch, System.Text.UTF8Encoding.UTF8);

                string sLine = "";
                sLine = objReader.ReadToEnd();
                objReader.Close();
                return sLine;

            }else
                return "";
        }

        private int validarIP(string sIpRead, ref List<string> lIps)
        {
            IPAddress ip = null;
            if (MainMenu.UserBasculas.iValidarDireccionIp(sIpRead, "IP") == 0 && IPAddress.TryParse(sIpRead, out ip)) // Pass a Correct IP
            {
                // IPEndPoint ipEndPoint = new IPEndPoint(ip, 50036);
                lIps.Add(sIpRead);
                return 1;
            }
            else
            {
                return 2;
            }
        }

        private string mensajeGuardarEnBD(int iSave)
         {
            switch (iSave)
            {
                case 0: 
                    return "OK en lectura de productos: ";
                case 1:
                    return "Error cadena incompleta: ";
                case 2:
                    return "Error en codigo de producto: ";
                default:
                    return "VACIO";
            }
         }

        private string muestraAutoincrementoId(string CadeSelect)
         {
             int cod = 0;
             OleDbDataReader LPp = Conec.Obtiene_Dato(CadeSelect, Conec.CadenaConexion);
             if (LPp.Read()) cod = Convert.ToInt32(LPp.GetValue(0));
             LPp.Close();
             return Convert.ToString(cod + 1);
         }

        public int guardarProductosEnBD(string cadena, ref List<int> lCodigo)
        {
            Int32 iCode;                               //4 dgt
            string sName;                               //50 dgt
            int iPesable = 1;                           //1 dgt 1=Pesable - -  0=Pieza
            string sPrecio;                             //6 dgt
            string sCaducidad;                          //3 dgt
            int iBaja = 0;                              //1 dgt 1=Baja --  0=Alta

            string sImgn = "";
            bool bExisImg = false;
            
            string sNow = string.Format(Variable.F_Hora, DateTime.Now) + " " + string.Format(Variable.F_Fecha, DateTime.Now);
           
            if (cadena[3] >= '0' && cadena[3] <= '9' || cadena[3] == ' ')
            {
                long i = 0;
                bool resul = long.TryParse(cadena.Substring(54, 11), out i);
                if (resul)
                {
                    iCode = Convert.ToInt32(cadena.Substring(0, 4));
                    sName = cadena.Substring(4, 50);
                    iPesable = Convert.ToInt32(cadena.Substring(54, 1).ToString());
                    sPrecio = (cadena.Substring(55, 4) + "." + cadena.Substring(59, 2));
                    iBaja = Convert.ToInt32(cadena.Substring(61, 1).ToString());
                    sCaducidad = cadena.Substring(62, 3);
                    if (cadena.Length > 65)
                    {
                        sImgn = cadena.Substring(65, (cadena.Length - 65));
                        sImgn = sImgn.Replace("\r", "");
                    }
                }
                else
                    return 1;
            }
            else
            {
                long i = 0;
                bool resul = long.TryParse(cadena.Substring(53, 11), out i);
                if (resul)
                {
                    iCode = Convert.ToInt32(cadena.Substring(0, 3));
                    sName = cadena.Substring(3, 50);
                    iPesable = Convert.ToInt32(cadena.Substring(53, 1).ToString());
                    sPrecio = (cadena.Substring(54, 4) + "." + cadena.Substring(58, 2));
                    iBaja = Convert.ToInt32(cadena.Substring(60, 1).ToString());
                    sCaducidad = cadena.Substring(61, 3);

                    if (cadena.Length > 64)
                    {
                        sImgn = cadena.Substring(64, (cadena.Length - 64));
                        sImgn = sImgn.Replace("\r", "");
                    }
                }
                else
                    return 1;
            }

            if (iCode <= 0)
                return 2;

            lCodigo.Add(iCode);

            if (sImgn.IndexOf(".jpg") > 0) 
                bExisImg = true;
            else
                bExisImg = false;


            if (iPesable == 0)  //SE VOLTEA PQ EL PUNTO DE VENTA 0=PESABLE 1=PIEZA
                iPesable = 1;
            else
                iPesable = 0;

            sPrecio = string.Format(Variable.F_Decimal, Convert.ToDouble(sPrecio));

            try
            {                
                productosTableAdapter.Fill(baseDeDatosDataSet.Productos);
                baseDeDatosDataSet.Productos.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Productos.CodigoColumn };
            }
            catch(Exception varEx)
            {
                Console.WriteLine(varEx.Message);
                return 2;
            }

            DataRow dr = baseDeDatosDataSet.Productos.Rows.Find(iCode);

            if(dr != null)
            {
                Conec.Condicion = "codigo = " + iCode;
                if (iBaja == 1)
                {
                    Conec.CadenaSelect = "UPDATE Productos " +
                    "SET borrado = " + true + 
                     ", pendiente = " + true +
                     " WHERE (" + Conec.Condicion + ")";
                }
                else
                {
                    if (bExisImg)
                    {
                        Conec.CadenaSelect = "UPDATE Productos " +
                            "SET Nombre = '" + sName + "', " +
                             "Precio = " + Convert.ToDouble(sPrecio) + ", " +
                             "TipoId = " + iPesable + ", " +
                             "CaducidadDias = " + sCaducidad + ", " +
                             "actualizado = '" + sNow + "', " +
                             "imagen1 = '" + sImgn + "', " +
                             "imagen = " + bExisImg + ", " +
                             "pendiente = " + true + ", " +
                              "borrado = " + false +
                             " WHERE (" + Conec.Condicion + ")";
                    }
                    else
                    {
                        Conec.CadenaSelect = "UPDATE Productos " +
                        "SET Nombre = '" + sName + "', " +
                        "Precio = " + Convert.ToDouble(sPrecio) + ", " +
                        "TipoId = " + iPesable + ", " +
                        "CaducidadDias = " + sCaducidad + ", " +
                        "actualizado = '" + sNow + "', " +
                        "pendiente = " + true + ", " +
                        "borrado = " + false +
                        " WHERE (" + Conec.Condicion + ")";
                    }
                }
                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Productos.TableName);
                return 0;
            }
            else
            {
                string iIdprod = muestraAutoincrementoId("SELECT id_producto FROM PRODUCTOS ORDER BY id_producto desc");

                Conec.CadenaSelect = "INSERT INTO Productos " +
                     "(id_producto, Codigo, NoPlu, Nombre, Precio, TipoId, CaducidadDias, actualizado, imagen1, imagen, pendiente, borrado ) " +
                     "VALUES ( " +
                         Convert.ToInt32(iIdprod) + "," +                         
                         //sCode + "'," +      
                         iCode + "," + 
                         iCode + ",'" +         
                         sName + "'," +                          
                         Convert.ToDouble(sPrecio) + "," +
                         iPesable + "," + 
                         Convert.ToInt32(sCaducidad) + ",'" +
                         sNow + "','" + 
                         sImgn + "'," +
                         bExisImg + "," +
                         true + "," +
                         false + ")";                                               

                Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect,
                    baseDeDatosDataSet.Productos.TableName);
                return 0;
            }
        }

        public int guardarVendorEnDB(string sVendor, ref List<int> lCodigoV)
        {
            int iBaja = Convert.ToInt32(sVendor.Substring(0,1)); //FALTA PEDIR ESTE CAMPO AL PV
            string sCode = sVendor.Substring(1, 2);                               //2 dgt
            string sName = sVendor.Substring(3,sVendor.Length-3);                               //50 dgt

            string sNowV = string.Format(Variable.F_Hora, DateTime.Now) + " " +
                           string.Format(Variable.F_Fecha, DateTime.Now);

            if(sName.Length > 22)
                sName = sName.Substring(0,19) + "...";
           
            sCode = sCode.Trim();

            if (Convert.ToInt32(sCode) <= 0)
                return 2;

            lCodigoV.Add(Convert.ToInt32(sCode));

            try
            {
                vendedoresTableAdapter.Fill(baseDeDatosDataSet.Vendedor);
                baseDeDatosDataSet.Vendedor.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Vendedor.id_vendedorColumn };
            }
            catch (Exception varEx)
            {
                Console.WriteLine(varEx.Message);
                return 2;
            }


            DataRow dr = baseDeDatosDataSet.Vendedor.Rows.Find(Convert.ToInt32(sCode));

            if (dr != null)
            {
                Conec.Condicion = "id_vendedor = " + Convert.ToInt32(sCode);
                if (iBaja == 1)
                {                
                    Conec.CadenaSelect = "UPDATE Vendedor " +
                    "SET borrado = " + true +
                    ", pendiente = " + true +
                    " WHERE ( " + Conec.Condicion + ")";
                }
                else
                {
                    Conec.CadenaSelect = "UPDATE Vendedor " +
                    "SET Nombre = '" + sName + "'" +
                    ", actualizado = '" + sNowV +
                    "', pendiente = " + true +
                    ", borrado = " + false +
                    " WHERE ( " + Conec.Condicion + ")";
                }

                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Vendedor.TableName);
                return 0;
            }
            else
            {
               
                Conec.CadenaSelect = "INSERT INTO Vendedor " +
                     "(id_vendedor, Nombre,actualizado,pendiente, borrado) " +
                    "VALUES (" + 
                    Convert.ToInt32(sCode) + ",'" +
                    sName + "','" +
                    sNowV + "'," +
                    true + "," +
                    false +  ")";

                Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Vendedor.TableName);

                return 0;
            }
        }
    }

    public class ConsoleSpiner
    {
        int counter;
        public ConsoleSpiner()
        {
            counter = 0;
        }
        public void Turn(int iValueCurrent, int iValueMax)
        {
            counter++;
            switch (counter % 4)
            {
                case 0: Console.Write("/"); break;
                case 1: Console.Write("-"); break;
                case 2: Console.Write("\\"); break;
                case 3: Console.Write("-"); break;
            }

            float fPorcentaje = ((float)(iValueCurrent) / (float)(iValueMax)) * 100;
            int iProcentaje = (int)fPorcentaje;
            string sPorcentaje = string.Format("{0:###}", iProcentaje);
            Console.Write(" {0}%", sPorcentaje);
            Console.SetCursorPosition(Console.CursorLeft - 1 - sPorcentaje.Length - 2, Console.CursorTop);
        }
    } 
}
