using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.IO.Ports;
using System.Runtime.Remoting;
using Validaciones;

namespace MainMenu
{
    /// <summary>
    /// 
    /// </summary>
    /// 	
    #region Clase de Socket
    public class SocketPacket
    {
        public SocketPacket(System.Net.Sockets.Socket socket, int clientNumber)
        {
            m_currentSocket = socket;
            m_clientNumber = clientNumber;
        }

        public System.Net.Sockets.Socket m_currentSocket;
        public int m_clientNumber;
        public byte[] dataBuffer = new byte[32767];
    }
    #endregion

    public class StateObject
    {
        // Socket de cliente.
        public Socket workSocket = null;
        //Tamaño del buffer.
        public const int BufferSize = 256;
        // Buffer de recibo.
        public byte[] buffer = new byte[BufferSize];
        // Dato recibido
        public StringBuilder sb = new StringBuilder();
    }

    public class GetIP
    {
        public String IPStr;
        public String IPBrou;

        public GetIP()
        {
            String strHostName = Dns.GetHostName();
            // Find host by name
            IPHostEntry iphostentry = Dns.GetHostEntry(strHostName);
            // Grab the first IP addresses
            IPStr = "";
            foreach (IPAddress ipaddress in iphostentry.AddressList)
            {
                if (ipaddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    IPStr = ipaddress.ToString();

                    break;
                }
            }
        }
    }

    #region Conexion de socket con bascula y con ipad
    public class Conexion
    {       
        public StreamWriter Err = new StreamWriter(new FileStream(Variable.appPath + "\\Error.ttt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite));

        #region variable conectar asincrono
        public static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static bool IsConnectServer = false;
        public static Socket SocketConnectServer = null;
        private static Exception socketexception;

        #endregion

        public Conexion() { }

        public Socket conectar(string ipremota, int port) //Comunicacion con la bascula TLS
        {
            Socket Winsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint myscale = new IPEndPoint(IPAddress.Parse(ipremota), port);

            connectDone.Reset();
            socketexception = null;
            IsConnectServer = false;

            DateTime DateTimeObject = DateTime.Now;

            Err.WriteLine(" ");
            Err.WriteLine("{0}: Ip: {1}, Port: {2}, Time: {3}", System.Reflection.MethodBase.GetCurrentMethod().Name, ipremota, port, DateTimeObject.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            try
            {
                Winsock.BeginConnect(myscale, new AsyncCallback(ConnectCallback), Winsock); //Llamado a la funcion asyncrona de conexion

                try
                {
                    if (connectDone.WaitOne(10000, false))
                    {
                        if (IsConnectServer == true)
                        {
                            connectDone.Reset();
                            Err.WriteLine("{0}: Conectado", System.Reflection.MethodBase.GetCurrentMethod().Name);
                            return SocketConnectServer;
                        }
                        else
                        {
                            Winsock.Close();
                            Err.WriteLine("{0}: Error Socket Conexion", System.Reflection.MethodBase.GetCurrentMethod().Name);
                            throw socketexception;
                        }
                    }
                    else
                    {
                        Winsock.Close();
                        Err.WriteLine("{0}: TimeOut Exception connection", System.Reflection.MethodBase.GetCurrentMethod().Name);
                        throw new TimeoutException("TimeOut Exception");
                    }
                }
                catch (ObjectDisposedException e)
                {
                    Err.WriteLine("{0}: Error: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message);

                }
                catch (ArgumentOutOfRangeException e)
                {
                    Err.WriteLine("{0}: Error: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message);

                }
                catch (AbandonedMutexException e)
                {
                    Err.WriteLine("{0}: Error: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message);

                }
                catch (InvalidOperationException e)
                {
                    Err.WriteLine("{0}: Error: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message);

                }
                catch (TimeoutException ex)
                {
                    Err.WriteLine(ex.Source + " " + ex.Message);
                    Err.WriteLine("{0}: Error: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                }

                return null;
            }
            catch (SocketException ex)
            {
                //Err.WriteLine(ex.ErrorCode.ToString() + ": -> " + ex.Source.ToString(), ex.Message);
                switch (ex.ErrorCode)
                {
                    case 10048: Err.WriteLine("Alerta!!! " + ex.ErrorCode.ToString() + ": La direccion esta en uso");
                        break;
                    case 10049: Err.WriteLine("Alerta!!! " + ex.ErrorCode.ToString() + ": No puede asignar direccion solicitada");
                        break;
                    case 10050: Err.WriteLine("Alerta!!! " + ex.ErrorCode.ToString() + ": La red no funciona");
                        break;
                    case 10053: Err.WriteLine("Alerta!!! " + ex.ErrorCode.ToString() + ": El software causo la anulación de la conexión");
                        break;
                    case 10054: Err.WriteLine("Alerta!!! " + ex.ErrorCode.ToString() + ": La conexion es reinicada por el host");
                        break;
                    case 10057: Err.WriteLine("Alerta!!! " + ex.ErrorCode.ToString() + ": El socket no esta conectado");
                        break;
                    case 10060: Err.WriteLine("Alerta!!! " + ex.ErrorCode.ToString() + ": Tiempo de conexion agotado");
                        break;
                    case 10061: Err.WriteLine("Alerta!!! " + ex.ErrorCode.ToString() + ": Conexion Rechazada");
                        break;
                }
                if (ex.ErrorCode == 10048 || ex.ErrorCode == 10049 || ex.ErrorCode == 10050 ||
                    ex.ErrorCode == 10053 || ex.ErrorCode == 10054 || ex.ErrorCode == 10057 ||
                    ex.ErrorCode == 10060 || ex.ErrorCode == 10061)
                {
                    desconectar(ref Winsock);
                }
                return null;
            }
            catch (Exception ex)
            {
                Err.WriteLine(ex.Source + " " + ex.Message);
                Err.WriteLine("{0}: Error: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                return null;
            }

        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            Conexion C = new Conexion();
            C.Err.WriteLine("{0}: In ConnectCallBack", System.Reflection.MethodBase.GetCurrentMethod().Name);
            DateTime DateTimeObject = DateTime.Now;
            C.Err.WriteLine("{0}: Time: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, DateTimeObject.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            try
            {
                Socket client = (Socket)ar.AsyncState;

                if (client.Connected == true)
                {
                    Console.WriteLine("Cliente conectado");
                    client.EndConnect(ar);
                    IsConnectServer = true;
                    SocketConnectServer = client;
                    connectDone.Set();
                }

            }
            catch (SocketException e)
            {
                C.Err.WriteLine("{0}: Error conexion: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message);
            }
            catch (ObjectDisposedException e)
            {
                C.Err.WriteLine("{0}: Error conexion: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message);
            }
            catch (ArgumentException e)
            {
                C.Err.WriteLine("{0}: Error conexion: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message);
            }
            catch (InvalidOperationException e)
            {
                C.Err.WriteLine("{0}: Error conexion: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message);
            }
            catch (Exception e)
            {
                C.Err.WriteLine("{0}: Error conexion: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message);
                socketexception = e;
            }
        }

        #region test_socket_conection

        public Socket CheckConnectivityForProxyHost(string hostName, int port)
        {
            Socket testSocket = null;

            if (string.IsNullOrEmpty(hostName))
                return testSocket;

            try
            {
                testSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = null;
                if (testSocket != null && IPAddress.TryParse(hostName, out ip)) // Pass a Correct IP
                {
                    IPEndPoint ipEndPoint = new IPEndPoint(ip, port);
                    CallWithTimeout(ConnectToProxyServers, 5000, testSocket, ipEndPoint);

                    if (testSocket != null && testSocket.Connected == true)
                    {
                        return testSocket;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
            }

            return testSocket;
        }


        private void CallWithTimeout(Action<Socket, IPEndPoint> action,
                int timeoutMilliseconds, Socket socket, IPEndPoint ipendPoint)
        {
            try
            {
                Action wrappedAction = () =>
                {
                    action(socket, ipendPoint);
                };

                IAsyncResult result = wrappedAction.BeginInvoke(null, null);

                if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
                {
                    wrappedAction.EndInvoke(result);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
            }
        }

        private void ConnectToProxyServers(Socket testSocket, IPEndPoint ipEndPoint)
        {
            try
            {
                if (testSocket == null || ipEndPoint == null)
                    return;

                testSocket.Connect(ipEndPoint);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
            }
        }
        #endregion

        public void Envio_Dato(ref Socket sock, string ipremota, string Envio_Trama, ref string[] Dato_Recivido)
        {
            string clientmessage = "";
            bool Codigo_Error = false;
            bool Recibo_dato = false;
            char[] chr = new char[2] { (char)9, (char)10 };
            char CkSum, chk = (char)0, caracter = (char)0;
            int n_pos;

            CkSum = new CheckSum().ChkSum(Envio_Trama);
            Envio_Trama = Envio_Trama + CkSum + (char)13;            

            try
            {
                if (sock.Connected)
                {
                    while (!Codigo_Error && !Recibo_dato)
                    {
                        if (!sock.Poll(3000, SelectMode.SelectError))
                        {
                            if (enviar(ref sock, Envio_Trama, ipremota))
                            {
                                Variable.envio_dato = true;

                                byte[] recs = new byte[32767];
                                recs.Initialize();
                                int rcount = 0;
                                EndPoint RemoteIP = sock.RemoteEndPoint;

                                sock.ReceiveTimeout = 10000;
                                rcount = sock.ReceiveFrom(recs, 0, recs.Length, SocketFlags.None, ref RemoteIP);
                                if (rcount > 0)
                                {
                                    clientmessage = System.Text.Encoding.ASCII.GetString(recs, 0, rcount);
                                    clientmessage = clientmessage.Substring(0, rcount);
                                    Recibo_dato = true;
                                }
                                else
                                {
                                    Codigo_Error = true;
                                    Recibo_dato = false;
                                }
                            }
                            else
                            {
                                Codigo_Error = true;
                                Recibo_dato = false;
                            }
                            n_pos = clientmessage.Length - 2;
                            chk = new CheckSum().ChkSum(clientmessage);
                            caracter = Convert.ToChar(clientmessage.Substring(n_pos, 1));
                            if (chk == caracter) Dato_Recivido = clientmessage.Split(chr);
                            else Codigo_Error = true;
                        }
                    }
                    if (!Variable.envio_dato)
                    {
                        DialogResult msg = MessageBox.Show(Variable.SYS_MSJ[206, Variable.idioma]);  //"No se envio dato");
                        Codigo_Error = false;
                    }
                    else
                    {
                        Codigo_Error = false;
                    }
                }
            }
            catch (System.Net.Sockets.SocketException ex)
            {                
                switch (ex.ErrorCode)
                {
                    case 10048: MessageBox.Show(ex.ErrorCode.ToString() + ": " + Variable.SYS_MSJ[441, Variable.idioma] + " IP " + ipremota, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case 10049: MessageBox.Show(ex.ErrorCode.ToString() + ": " + Variable.SYS_MSJ[442, Variable.idioma] + " IP " + ipremota, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case 10050: MessageBox.Show(ex.ErrorCode.ToString() + ": " + Variable.SYS_MSJ[443, Variable.idioma] + " IP " + ipremota, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case 10053: MessageBox.Show(ex.ErrorCode.ToString() + ": " + Variable.SYS_MSJ[444, Variable.idioma] + " IP " + ipremota, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case 10054: MessageBox.Show(ex.ErrorCode.ToString() + ": " + Variable.SYS_MSJ[445, Variable.idioma] + " IP " + ipremota, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case 10057: MessageBox.Show(ex.ErrorCode.ToString() + ": " + Variable.SYS_MSJ[446, Variable.idioma] + " IP " + ipremota, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case 10060: MessageBox.Show(ex.ErrorCode.ToString() + ": " + Variable.SYS_MSJ[447, Variable.idioma] + " IP " + ipremota, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case 10061: MessageBox.Show(ex.ErrorCode.ToString() + ": " + Variable.SYS_MSJ[448, Variable.idioma] + " IP " + ipremota, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                }
                if (ex.ErrorCode == 10048 || ex.ErrorCode == 10049 || ex.ErrorCode == 10050 ||
                     ex.ErrorCode == 10053 || ex.ErrorCode == 10054 || ex.ErrorCode == 10057 ||
                     ex.ErrorCode == 10060 || ex.ErrorCode == 10061)
                {
                    Dato_Recivido = null;
                    desconectar(ref sock);
                }

            }
            catch (Exception ex)
            {
                Err.WriteLine(ex.Source + " " + ex.Message);
            }
        }
        public void Envio_Dato(ref Socket sock, string ipremota, string Envio_Trama)
        {
            char CkSum;

            CkSum = new CheckSum().ChkSum(Envio_Trama);
            Envio_Trama = Envio_Trama + CkSum + (char)13;

            try
            {
                if (sock.Connected)
                {
                    if (!sock.Poll(3000, SelectMode.SelectError))
                    {
                        if (enviar(ref sock, Envio_Trama, ipremota))
                        {
                            Variable.envio_dato = true;
                        }
                    }
                    if (!Variable.envio_dato)
                    {
                        DialogResult msg = MessageBox.Show(Variable.SYS_MSJ[206, Variable.idioma]);  //"No se envio dato");                       
                    }
                }
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                switch (ex.ErrorCode)
                {
                    case 10048: MessageBox.Show(ex.ErrorCode.ToString() + ": " + Variable.SYS_MSJ[441, Variable.idioma], "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case 10049: MessageBox.Show(ex.ErrorCode.ToString() + ": " + Variable.SYS_MSJ[442, Variable.idioma], "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case 10050: MessageBox.Show(ex.ErrorCode.ToString() + ": " + Variable.SYS_MSJ[443, Variable.idioma], "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case 10053: MessageBox.Show(ex.ErrorCode.ToString() + ": " + Variable.SYS_MSJ[444, Variable.idioma], "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case 10054: MessageBox.Show(ex.ErrorCode.ToString() + ": " + Variable.SYS_MSJ[445, Variable.idioma], "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case 10057: MessageBox.Show(ex.ErrorCode.ToString() + ": " + Variable.SYS_MSJ[446, Variable.idioma], "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case 10060: MessageBox.Show(ex.ErrorCode.ToString() + ": " + Variable.SYS_MSJ[447, Variable.idioma], "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case 10061: MessageBox.Show(ex.ErrorCode.ToString() + ": " + Variable.SYS_MSJ[448, Variable.idioma], "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                }
                if (ex.ErrorCode == 10048 || ex.ErrorCode == 10049 || ex.ErrorCode == 10050 ||
                     ex.ErrorCode == 10053 || ex.ErrorCode == 10054 || ex.ErrorCode == 10057 ||
                     ex.ErrorCode == 10060 || ex.ErrorCode == 10061)
                {
                    desconectar(ref sock);
                }
            }
            catch (Exception ex)
            {
                Err.WriteLine(ex.Source + " " + ex.Message);
            }
        }
        public void desconectar(ref Socket Winsock)
        {
            char CkSum;
            string strcomando = "DXXX" + (char)9 + (char)10;

            try
            {
                if (Winsock != null)
                {
                    if (Winsock.Connected == true)
                    {
                        CkSum = new CheckSum().ChkSum(strcomando);
                        byte[] resp = new Byte[25];
                        resp.Initialize();
                        resp = System.Text.Encoding.ASCII.GetBytes(strcomando + CkSum + (char)13);
                        EndPoint RemoteIP = Winsock.RemoteEndPoint;                        
                        Winsock.Shutdown(SocketShutdown.Both);
                    }
                    Winsock.Close();
                    Winsock = null;
                }
            }
            catch (SocketException rsult)
            {
                Err.WriteLine(rsult.Message.ToString() + "   " + rsult.Source.ToString() + "  " + rsult.StackTrace, rsult.Message);
            }
        }

        //TODO: Agregar timeout 
        public string Recibir_Respuesta(ref Socket sock, string ipremota, string Envio_Trama)
        {
            string clientmessage = "";
            int n_pos;
            bool Recibo_dato = false;
            bool Codigo_Error = false;
            char CkSum, chk = (char)0, caracter = (char)0;

            CkSum = new CheckSum().ChkSum(Envio_Trama);
            Envio_Trama = Envio_Trama + CkSum + (char)13;

            try
            {
                if (sock.Connected)
                {
                    while (!Recibo_dato && !Codigo_Error)
                    {
                        if (!sock.Poll(3000, SelectMode.SelectError))
                        {
                            enviar(ref sock, Envio_Trama, ipremota);
                            byte[] recs = new byte[32767];
                            recs.Initialize();
                            int rcount;
                            EndPoint RemoteIP = sock.RemoteEndPoint;

                            sock.ReceiveTimeout = 15000;
                            
                            rcount = sock.ReceiveFrom(recs, 0, recs.Length, SocketFlags.None, ref RemoteIP);
                            if (rcount > 0)
                            {
                                clientmessage = System.Text.Encoding.ASCII.GetString(recs, 0, rcount);
                                clientmessage = clientmessage.Substring(0, rcount);
                                Recibo_dato = true;
                                if (clientmessage.Length > 0)
                                {
                                    n_pos = clientmessage.Length - 2;
                                    chk = new CheckSum().ChkSum(clientmessage);
                                    caracter = Convert.ToChar(clientmessage.Substring(n_pos, 1));
                                    if (chk != caracter) Codigo_Error = true;
                                }
                            }
                            else
                            {
                                Codigo_Error = true;
                                Recibo_dato = false;
                            }                            
                        }
                    }
                }
                return clientmessage;
            }
            catch (SocketException ex)
            {
                Err.WriteLine(ex.ErrorCode.ToString() + " " + ex.Source.ToString(), ex.Message);

                if (ex.ErrorCode == 10054 || ex.ErrorCode == 10060 || ex.ErrorCode == 10045 || ex.ErrorCode == 10048 || ex.ErrorCode == 10061)
                {
                    Codigo_Error = true;                    
                    clientmessage = "Null";
                   // desconectar(ref sock);
                }
                return clientmessage;
            }
        }

        public bool enviar(ref Socket sock, string Envio_Trama, string ipremota)
        {
            if (Envio_Trama.Length > 0)
            {
                byte[] sende = new byte[32767];
                sende.Initialize();
                sende = Encoding.GetEncoding(437).GetBytes(Envio_Trama);
                IPEndPoint Remote = new IPEndPoint(IPAddress.Parse(ipremota), 50036);
                EndPoint RemoteIP = sock.RemoteEndPoint;  // (EndPoint)Remote;
                int envio = 0, rcount_total = 0;
                do
                {
                    envio = sock.SendTo(sende, sende.Length, SocketFlags.None, RemoteIP);
                    rcount_total += envio;                    
                } while (rcount_total < Envio_Trama.Length && envio > 0);

                return true;
            }
            else
            {
                return false;
            }
        }
    }
    #endregion

    public class Serial
    {
        public bool OpenPort(ref SerialPort S_RS232, string puerto, Int32 baud)
        {
            try
            {
                if (S_RS232 != null)
                {
                    if (S_RS232.IsOpen) S_RS232.Close();
                }
                S_RS232.BaudRate = baud; //Convert.ToInt32(Variable.velocidad[baud]);
                S_RS232.PortName = puerto;  // "COM" + puerto.ToString();
                S_RS232.Parity = System.IO.Ports.Parity.None;
                S_RS232.StopBits = System.IO.Ports.StopBits.One;
                S_RS232.DataBits = 8;
                S_RS232.Handshake = System.IO.Ports.Handshake.None;
                S_RS232.ReadTimeout = 2000;
                S_RS232.WriteTimeout = 2000;
                S_RS232.Open();

                return true;
            }
            catch (System.UnauthorizedAccessException ex)
            {
                Console.Write(ex.Message);
                return false;
            }
            catch (System.IO.IOException e)
            {
                Console.Write(e.Message);
                return false;
            }
            catch (System.ArgumentOutOfRangeException ea)
            {
                Console.Write(ea.Message);
                return false;
            }
        }

        public void SendData(ref SerialPort S_RS232, string dato)
        {
            try
            {

                S_RS232.Write(dato);
            }
            catch (System.NullReferenceException en)
            {
                ClosePort(ref S_RS232);
                System.Console.Write(en.Message);
            }
            catch (System.IO.IOException e)
            {
                ClosePort(ref S_RS232);
                System.Console.Write(e.Message);
            }
            catch (System.InvalidOperationException eo)
            {
                ClosePort(ref S_RS232);
                System.Console.Write(eo.Message);
            }
        }


        public void ClosePort(ref SerialPort S_RS232)
        {
            try
            {
                if (S_RS232.IsOpen)
                {
                    S_RS232.Close();
                }
                S_RS232.Dispose();
                S_RS232 = null;
            }
            catch (System.IO.IOException eO)
            {
                Console.Write(eO.Message);
            }
            catch (System.InvalidOperationException ei)
            {
                Console.Write(ei.Message);
            }
            catch (System.ArgumentNullException ea)
            {
                Console.Write(ea.Message);
            }
            catch (System.NullReferenceException e)
            {
                Console.Write(e.Message);
            }
        }

        public void Connect(ref SerialPort Comm1, string puerto, int baud, string comando)  //Conectar a otras opciones
        {
            try
            {
                OpenPort(ref Comm1, puerto, baud);
                SendData(ref Comm1, comando);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        public void SendCOMSerial(ref SerialPort Comm1, string Envio_Trama, ref string[] Dato_Recibido)
        {
            char[] car = new char[Envio_Trama.Length];
            char CkSum;
            string ComBuffer = "";
            int nrec = 0;

            CkSum = new CheckSum().ChkSum(Envio_Trama);
            Envio_Trama = Envio_Trama + CkSum + (char)13;

            car = Envio_Trama.ToCharArray();

            int T_car = 0;
            try
            {
                while (T_car < car.Length)
                {
                    if (Comm1.IsOpen)
                    {
                        SendData(ref Comm1, Convert.ToString(car[T_car]));
                        //System.Threading.Thread.SpinWait(500);
                        T_car++;
                    }
                    else
                    {
                        //MessageBox.Show(Variable.M_Error[26, Variable.idioma]);
                        break;
                    }
                }

                do
                {
                    if (Comm1.BytesToRead > 0)
                    {
                        ComBuffer += Comm1.ReadExisting();
                        nrec = 0;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(10);
                        nrec++;
                    }
                }
                while (ComBuffer.IndexOf((char)13) < 0 && nrec < 1000);

                if (ComBuffer.Length > 0 && ComBuffer.IndexOf((char)13) > 0)
                {
                    Recibir_COM(ComBuffer, ref Dato_Recibido);
                }
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        public void SendCOMSerial(ref SerialPort Comm1, string Envio_Trama)
        {
            char[] car = new char[Envio_Trama.Length];
            char CkSum;

            CkSum = new CheckSum().ChkSum(Envio_Trama);
            Envio_Trama = Envio_Trama + CkSum + (char)13;

            car = Envio_Trama.ToCharArray();

            int T_car = 0;
            try
            {
                while (T_car < car.Length)
                {
                    if (Comm1.IsOpen)
                    {
                        SendData(ref Comm1, Convert.ToString(car[T_car]));
                        System.Threading.Thread.SpinWait(500);
                        T_car++;
                    }
                    else
                    {
                        break;
                    }
                }
                System.Threading.Thread.SpinWait(800);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        public string ReceivedCOMSerial(ref SerialPort Comm1)
        {
            string ComBuffer = "";
            string Mensaje_recibido = "";
            int nrec = 0;

            try
            {
                do
                {
                    if (Comm1.BytesToRead > 0)
                    {
                        ComBuffer += Comm1.ReadExisting();
                        nrec = 0;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(10);
                        nrec++;
                    }
                }
                while (ComBuffer.IndexOf((char)13) < 0 && nrec < 1000);

                if (ComBuffer.Length > 0 && ComBuffer.IndexOf((char)13) > 0)
                {
                    Recibir_COM(ComBuffer, ref Mensaje_recibido);
                }
                return Mensaje_recibido;
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return Mensaje_recibido;
            }
        }

        public bool Recibir_COM(string ComBuffer, ref string[] Dato_Recibido)
        {
            char[] chr = new char[2] { (char)9, (char)10 };
            string Msg_Recibido = "";
            char chk = (char)0;
            char caracter = (char)0;
            int n_pos = 0;
            bool continuar = true;
            string comando = ComBuffer.Substring(0, 2);
            try
            {
                Msg_Recibido = ComBuffer;

                n_pos = Msg_Recibido.Length - 2;
                chk = new CheckSum().ChkSum(Msg_Recibido);
                caracter = Convert.ToChar(Msg_Recibido.Substring(n_pos, 1));

                if (Msg_Recibido.IndexOf("Error") >= 0 || chk != caracter) continuar = false;
                if (Msg_Recibido.IndexOf("Ok") >= 0) continuar = true;

                ComBuffer = "";
                Dato_Recibido = Msg_Recibido.Split(chr);
                return continuar;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return false;
            }
        }

        public bool Recibir_COM(string ComBuffer, ref string Dato_Recibido)
        {
            string Msg_Recibido = "";
            char chk = (char)0;
            char caracter = (char)0;
            int n_pos = 0;
            bool continuar = true;
            string comando = ComBuffer.Substring(0, 2);
            try
            {
                Msg_Recibido = ComBuffer;
                
                n_pos = Msg_Recibido.Length - 2;
                chk = new CheckSum().ChkSum(Msg_Recibido);
                caracter = Convert.ToChar(Msg_Recibido.Substring(n_pos, 1));

                if (Msg_Recibido.IndexOf("Error") >= 0 || chk != caracter) continuar = false;
                if (Msg_Recibido.IndexOf("Ok") >= 0) continuar = true;

                ComBuffer = "";
                Dato_Recibido = Msg_Recibido;
                return continuar;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return false;
            }
        }
    }
}
