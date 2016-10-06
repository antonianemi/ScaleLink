#define _DEBUGMODE                 // Leave this defined to view debug output to the console

using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Drawing.Drawing2D;
using Validaciones;
using SocketConexion;

namespace SGSDK
{

    /// <summary>
    /// SGSDKCOM - The Steele Group communications Class Library
    /// This CL is used for sending and receiving images over either serial or TCP to a linux machine.
    /// </summary>
    public class SGSDKCOM
    {
        #region _SGSDKCOM_VARS
        internal const int SOH = 0x01;
        internal const int STX = 0x02;
        internal const int EOT = 0x04;
        internal const int ACK = 0x06;
        internal const int NAK = 0x15;
        internal const int CAN = 0x18;
        internal const int CTRLZ = 0x1A;

        internal static SerialPort _serialPort;

        public bool bComStart = false;
        private bool bTcpStart = false;

        public static myCircBuff<byte> ctb = new myCircBuff<byte>(2048);
        public enum xtypes { USING_COM = 1, USING_TCP, USING_IPAD };
        public enum xtypesAnswer { ANS_PC = 1, ANS_IPAD };
        public static byte[] InArray = new byte[131];

        public string sFileName;
        public string sPath;
        string sSize;

        public int nFileSize;

        public static int TCP_BLOCKSIZE = 128;

        private static XModem myXmodem;
        IPEndPoint ServIP;
        string _sSendIP;
        int _sPort;

        public static ManualResetEvent reciveDoneSerial = new ManualResetEvent(false);

        #endregion

        /// <summary>
        ///  SGSDKCOM class - 
        ///  
        /// This class library is used to send and receive images to a linux connection. Both Serial and TCP 
        ///  Network support are available but, only active when the class is constructed with a valid COM port and/or IP.
        ///  For Installations where only serial is available, provide only a COM port. If serial is not available, an IP
        ///  number will skip the com port and use only the network connection.
        ///  
        /// Anytime a SGSDKCOM class is instantiated, use must call the StopSDK() member when you are finished with it. This
        /// method will free all the resources used by the object. Please review the sample application for proper usage.
        /// 
        /// </summary>
        /// <param name="sComPort"> The Name of the COM port used for communications</param>
        /// <param name="sSendIP"> The IP of the linux computer</param>
        /*public SGSDKCOM(string sComPort, string sSendIP, int sPort)
        {
            if (sSendIP.Length > 0)          // Only works when an IP is provided
            {
                ServIP = new IPEndPoint(IPAddress.Parse(sSendIP), sPort);
                _sSendIP = sSendIP;
                _sPort = sPort;
            }
        }*/

        /// <summary>
        ///  Communication errors will be copied into this string and made public for examination
        /// </summary>
        public string ComErrString;

        /// <summary>
        ///  SGSDKCOM class - 
        ///  
        /// This class library is used to send and receive images to a linux connection. Both Serial and TCP 
        ///  Network support are available but, only active when the class is constructed with a valid COM port and/or IP.
        ///  For Installations where only serial is available, provide only a COM port. If serial is not available, an IP
        ///  number will skip the com port and use only the network connection.
        ///  
        /// Anytime a SGSDKCOM class is instantiated, use must call the StopSDK() member when you are finished with it. This
        /// method will free all the resources used by the object. Please review the sample application for proper usage.
        /// 
        /// </summary>
        /// <param name="sComPort"> The Name of the COM port used for communications</param>
        /// <param name="sSendIP"> The IP of the linux computer</param>
        public SGSDKCOM(string sComPort, string sSendIP, int sPort)
        {
            #region _Serial_port_init
            if (sComPort.Length > 0)
            {
                Console.WriteLine("Iniciando puerto serie SGSDKCOM");
                _serialPort = new SerialPort();
                // set the appropriate properties to talk to linux connection
                _serialPort.PortName = sComPort;
                _serialPort.BaudRate = 115200;
                _serialPort.Parity = Parity.None;
                _serialPort.DataBits = 8;
                _serialPort.StopBits = StopBits.One;
                _serialPort.Handshake = Handshake.None;

                // Set the read/write timeouts
                _serialPort.ReadTimeout = 1000;
                _serialPort.WriteTimeout = 1000;
                _serialPort.Encoding = Encoding.GetEncoding(1252);  // use this to receive all 255 values from char to byte * per MSDN blog

                _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

                try
                {
                    _serialPort.Open();
                    bComStart = true;
                }
                catch (Exception ex)
                {
                    // Nothing we can really do about it here at the moment
                    ComErrString = ex.Message;    // Save the message in case someone wants to read it
                    #if _DEBUGMODE
                        Console.WriteLine(ComErrString);
                    #endif
                        bComStart = false;
                }               
            }
            #endregion
            
            #region TCP_port_init
            if (sSendIP.Length > 0)          // Only works when an IP is provided
            {
                ServIP = new IPEndPoint(IPAddress.Parse(sSendIP), sPort);
                _sSendIP = sSendIP;
                _sPort = sPort;
                bTcpStart = true;
            }
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string inStr;
            int iCnt = 0;
            int nMax = 0;

            try
            {
                if (sp.BytesToRead > 0)
                {
                    inStr = sp.ReadExisting();
                    InArray = Encoding.GetEncoding(437).GetBytes(inStr);
                    nMax = InArray.Length;
                    for (iCnt = 0; iCnt < nMax; iCnt++)
                    {
                        SGSDKCOM.ctb.Put(InArray[iCnt]);
                        reciveDoneSerial.Set();                        
                    }
                }
            }
            catch (TimeoutException) {
                Console.WriteLine("Timeout Serial Input");
            }

            iCnt = InArray.Length;
            Array.Clear(InArray, 0, iCnt); // clear the byte array
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int iReceSerialTimeOut()
        {
            reciveDoneSerial.Reset();

            if (ctb.Size <= 0)
            {
                if (reciveDoneSerial.WaitOne(2000, false))
                {
                    if (ctb.Size <= 0)
                    {
                        reciveDoneSerial.Reset();
                        return -1;
                    }
                }
                else
                {
                    if (ctb.Size > 0)
                    {

                    }
                    else
                    {
                        reciveDoneSerial.Reset();
                        return -1;
                    }
                }
            }

            reciveDoneSerial.Reset();
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void sendNAK()
        {
            byte[] btOutemp = new byte[3];
            btOutemp[0] = NAK;
            btOutemp[1] = 0x09;
            btOutemp[2] = 0x0A;

            char chk = new CheckSum().ChkSumBytes(btOutemp);

            byte[] btOut = new byte[btOutemp.Length + 3];

            btOut[0] = NAK;
            btOut[1] = 0x09;
            btOut[2] = 0x0A;
            btOut[3] = (byte)chk;
            btOut[4] = 0x0D;
            btOut[5] = EOT;

            _serialPort.Write(btOut, 0, btOut.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void sendACK()
        {
            byte[] btOutemp = new byte[3];
            btOutemp[0] = ACK;
            btOutemp[1] = 0x09;
            btOutemp[2] = 0x0A;
                        
            char chk = new CheckSum().ChkSumBytes(btOutemp);

            byte[] btOut = new byte[btOutemp.Length + 3];
            
            btOut[0] = ACK;
            btOut[1] = 0x09;
            btOut[2] = 0x0A;
            btOut[3] = (byte)chk;
            btOut[4] = 0x0D;
            btOut[5] = EOT;

            _serialPort.Write(btOut, 0, btOut.Length);
        }

        /// <summary>
        ///  StopSDK
        ///  This method should be called when the application is finished with the object.
        /// </summary>
        public void StopSDK()
        {
            if (bComStart)
            {
                if (_serialPort.IsOpen)
                    _serialPort.Close();
            }
            else if (bTcpStart)
            {
                bTcpStart = false;
            }
        }

        /// <summary>
        ///  ListenLoopCom
        ///  This method is for testing only. It demonstrates one way of setting up a listen on the Com port to answer files transfers
        ///  from the linux computer.
        /// </summary>
        /// <returns>Nonzero return on error.</returns>
        public int ListenLoopCom(string sPathImageDestine)
        {
            myXmodem = new XModem();

            // Read a byte from the serial port and check to see if linux computer is ready to send.

            Console.WriteLine("");
            Console.WriteLine("Sending Ping to Host...");

            if (iPingHost() > 0)
            {   // Alert Server
                Console.WriteLine("No se recibio PING de respuesta correcto");
                StopSDK();
                return 1;
            }

            if (iReceSerialTimeOut() != 0)
            {
                Console.WriteLine("Timeout Exit without seeing ping.");
                StopSDK();
                return 2;
            }

            byte bTes = ctb.Get();

            if (bTes != 'P')
            {
                Console.WriteLine("It is not the value of ping.");
                StopSDK();
                return 3;
            }

            Console.WriteLine("Received a ping request.");

            sendACK();

            sFileName = GetField();

            if (sFileName == null)
            {
                Console.WriteLine("Error get sFilename");
                StopSDK();
                return 4;
            }

            #if _DEBUGMODE
                Console.WriteLine("Getting filename..");
                Console.Write(sFileName);
                Console.WriteLine(" ");
            #endif
            sendACK();

            /*sPath = GetField();
            #if _DEBUGMODE
                Console.WriteLine("Getting Path");
                Console.Write(sPath);
                Console.WriteLine(" ");
            #endif
            sendACK();*/

            sSize = GetField();

            if (sSize == null)
            {
                Console.WriteLine("Error get sSize");
                StopSDK();
                return 5;
            }

            #if _DEBUGMODE
                Console.WriteLine("Getting filesize");
                Console.WriteLine(" ");
            #endif
            sendACK();
            nFileSize = Int32.Parse(sSize, System.Globalization.NumberStyles.HexNumber);
            #if _DEBUGMODE
                Console.Write(nFileSize);
                Console.WriteLine(" ");
            #endif
            // Switch to serial file transfer receive
            byte[] baDest = new byte[nFileSize];
            int tret = SGSDK.XModem.xmodemReceive(baDest);

            if (tret > 0)
            {
                SGSDKCOM.sendNAK();
                Console.WriteLine("Error transferencia de imagen");
                StopSDK();
                return 6;
            }

            SGSDKCOM.sendACK();
            StopSDK();

            string sFName;
            sFName = sPathImageDestine;
            sFName += "//";
            sFName += sFileName;
           
            byte[] baDestNew = new byte[nFileSize-4];

            for (int i = 0; i < nFileSize - 4; i++)
            {
                baDestNew[i] = baDest[i];
            }

            // Write file to volume
            BinaryWriter writer = new BinaryWriter(File.Open(sFName, FileMode.Create));
            try
            {
                writer.Write(baDestNew);
            }
            catch (IOException e)
            {
               ComErrString = e.Message;    // Save the message in case someone wants to read it               
               Console.WriteLine(ComErrString);
               Console.WriteLine("Error al escribir la imagen: {0}", e.Message);
               writer.Close();
               return 8;
            }

            writer.Close();
            Console.WriteLine("Recepcion de imagen OK");

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static int iPingHost()
        {
            
            byte btSing;

            string str = "P\t\n";
            char chk = new CheckSum().ChkSum(str);
            str += chk + "\r";

            byte[] btOut = new byte[str.Length + 1];

            for (int cnt = 0; cnt < str.Length; cnt++)
            {
                btOut[cnt] = System.Convert.ToByte(str[cnt]);
            }

            btOut[str.Length] = EOT;


            _serialPort.Write(btOut, 0, btOut.Length);

            if (iReceSerialTimeOut() != 0)
            {
                Console.WriteLine("TimeOut al recibir rta Ping");
                return 1;
            }
            else
            {
                btSing = ctb.Get();
                if (btSing == ACK)
                {
                    Console.WriteLine("Rta de Ping OK");
                }
                else
                {
                    Console.WriteLine("Rta de Ping BAD {0}", btSing);
                    return 2;
                }
            }

            return 0;
        }

        public static byte XmodemResp()
        {
            byte btSing;

            if (iReceSerialTimeOut() != 0)
            {
                btSing = 0;
            }
            else
            {
                btSing = ctb.Get();
            }

            return btSing;
        }

        public static int SetBlock(byte[] bSrc)
        {
            int nLen = bSrc.Length;
            _serialPort.Write(bSrc, 0, nLen);
            
            return 0;
        }

        private static int SetField(string str)
        {
            int nLen = str.Length + 1;
            int cnt = 0;
            byte btSing;
            byte[] btemp = new byte[2];

            str += "\t\n";
            char chk = new CheckSum().ChkSum(str);
            str += chk + "\r";

            Console.WriteLine("Dato enviado: {0}", str);

            Thread.Sleep(10);
            byte[] bOut = new byte[str.Length + 1];

            for (cnt = 0; cnt < str.Length; cnt++)
            {
                bOut[cnt] = System.Convert.ToByte(str[cnt]);
            }
            bOut[str.Length] = EOT;

            for (int i = 0; i < bOut.Length; i++)
            {
                btemp[0] = bOut[i];
                _serialPort.Write(btemp, 0, 1);
            }


            if (iReceSerialTimeOut() != 0)
            {
                Console.WriteLine("Timeout al recibir ping");
                return 1;
            }
            else
            {
                btSing = ctb.Get();
                if (btSing == ACK)
                {
                    Console.WriteLine("Ping recivido OK");
                    return 0;
                }

                Console.WriteLine("Error al recibir ACK {0}",btSing);
                return 2;
            }
        }

        private static string GetField()
        {
            if (iReceSerialTimeOut() != 0)
            {
                Console.WriteLine("Timeout GetField");
                return null;
            }

            var ar = new ArrayList();
            byte btSing;
            bool bconti = true;
            string strRet = "";

            while (bconti)
            {
                
                //if (ctb.Size > 0)
                if (iReceSerialTimeOut() == 0)
                {
                    btSing = ctb.Get();
                    ar.Add(btSing);

                    if (btSing == EOT)
                    {
                        bconti = false;
                    }
                }
                else
                {
                    return null;
                }
            }

            for (int cnt = 0; cnt < ar.Count - 1; cnt++)
            {
                strRet += System.Convert.ToChar(ar[cnt]);
            }

            return strRet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bDest"></param>
        /// <returns></returns>
        public static int ReadSerialPacket(byte[] bDest)
        {

            if (iReceSerialTimeOut() != 0)
            {
                Console.WriteLine("Timeout ReadSerialPacket");
                return 1;
            }

            byte btSing;
            bool bconti = true;
            int nIndex = 0;
            int DestLen = bDest.Length;

            int halflen = 0;

            if (DestLen > 0)
                halflen = DestLen / 10;

            while (bconti)
            {
                //if (ctb.Size > 0)
                if(iReceSerialTimeOut() == 0)
                {
                    btSing = ctb.Get();
                    if (nIndex < DestLen - 1)
                    {
                        bDest[nIndex] = btSing;
                        nIndex++;
                        if (nIndex < halflen)
                            Thread.Sleep(1);
                    }
                    else
                    {
                        bDest[nIndex] = btSing;
                        return 0;
                    }
                }
                else
                {
                    return 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// ListenToScale
        /// this method is for testing only. It demonstrates one way of setting up a listen event on the network to receive file transfers
        /// from the linux computer.
        /// </summary>
        /// <returns>Nonzero return on error.</returns>
        /// Return  0 - OK
        ///         1 - Error en bind
        ///         2 - Error listen
        ///         3 - Error en Accept
        ///         4 - Error al obtener nombre de la imagen
        ///         5 - Error al obtener tamaño de la imagen
        ///         6 - Error al obtener la imagen
        ///         7 - Error al guardar la imagen en DD
        ///         8 - Error en CRC
        public int ListenToScale(string sPathDestino, string sPathOrigen, Socket iSock)
        {
            /*Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, _sPort);

            Socket client = null;

            try
            {
                socket.Bind(ip);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Socket bind: {0}", e);
                return 1;
            }

            try
            {
                socket.Listen(10);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Error socket listen: {0}", e.SocketErrorCode);
                return 2;
            } */

            #region Envia_Comando

            Socket socket = iSock;

            string sCommand = "LJ" + "\t" + sPathOrigen + "\t\n";
            char chk = new CheckSum().ChkSum(sCommand);
            sCommand += chk + "\r";

            byte[] bOut = new byte[10];
            bOut = Encoding.GetEncoding(437).GetBytes(sCommand);
            int iSizeStr = bOut.Length;
            int iByteSen = 0;

            try
            {
                iByteSen = socket.Send(bOut);
            }
            catch (SocketException e)
            {
                if (iByteSen == 0)
                {
                    Console.WriteLine("Conexion cerrada");
                }

                Console.WriteLine("SetFieldTCP Codigo de Error: {0}", e.ErrorCode);
                return 1;
            }
            #endregion

            /*
            #if _DEBUGMODE
                Console.WriteLine("Waiting for a client...");
            #endif

            SocketOperation tcpConnection = new SocketOperation();

            socket = tcpConnection.iAcceptClient(socket);

            if (socket == null)
            {
                Console.WriteLine("TimeOut Exception connection");
                return 3;
            }

            IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;
            
            
            #if _DEBUGMODE
                Console.WriteLine("Connected with {0} at port {1}", clientep.Address, clientep.Port);
            #endif
            */

            string sFileName;
            sFileName = GetFieldTCP(socket, xtypes.USING_TCP);        //Obtiene el nombre de la imagen a guardar

            if (sFileName == null)
            {
                Console.WriteLine("Error al obtener nombre del archivo");
                socket.Close();
                return 4;
            }

            #if _DEBUGMODE
                Console.WriteLine("Nombre del archivo: {0}", sFileName);
            #endif

            string sSize;
            sSize = GetFieldTCP(socket, xtypes.USING_TCP);            //Obtiene el tamanio de la imagen a guardar

            if (sSize == null)
            {
                Console.WriteLine("Error al obtener tamanio del archivo");
                socket.Close();
                return 5;
            }

            #if _DEBUGMODE
                Console.WriteLine("Tamanio del archivo: {0}", sSize);
            #endif
            
            int fileSize = 0;
            fileSize = Int32.Parse(sSize, System.Globalization.NumberStyles.HexNumber);

            byte[] baDest = new byte[fileSize];

            if (GetBlockTCP(socket, baDest, fileSize) > 0)          //Obtiene la imagen a guardar
            {
                Console.WriteLine("Error al obtener la imagen");
                socket.Close();
                return 7;
            }

            socket.Close();

            string sFName;
            sFName = sPathDestino;
            sFName += sFileName;
 
            // Write file to volume
            byte[] baDestNew = new byte[fileSize-4];

            for (int i = 0; i < fileSize - 4; i++)
            {
                baDestNew[i] = baDest[i];
            }

            BinaryWriter writer = new BinaryWriter(File.Open(sFName, FileMode.Create));

            try
            {
                writer.Write(baDestNew);
            }
            catch (IOException e)
            {
                Console.WriteLine("Error al escribir la imagen: {0}", e.Message);
                writer.Close();
                return 8;
            }

            writer.Close();
            return 0;
        }

        /// <summary>
        /// ListenToIpad
        /// this method is for testing only. It demonstrates one way of setting up a listen event on the network to receive file transfers
        /// from the linux computer.
        /// </summary>
        /// <returns>Nonzero return on error.</returns>
        /// Return  0 - OK
        ///         1 - Error en bind
        ///         2 - Error listen
        ///         3 - Error en Accept
        ///         4 - Error al obtener nombre de la imagen
        ///         5 - Error al obtener el path de la imagen
        ///         6 - Error al obtener tamaño de la imagen
        ///         7 - Error al obtener la imagen
        ///         8 - Error al guardar la imagen en DD
        public int ListenToIpad(Socket clientsocket, string sPathDestine)
        {

            Int32 fileSize = 0;
            string sFileName;
            
            string sdoka = "OK";

            sdoka += "\t\n";
            char chk = new CheckSum().ChkSum(sdoka);
            sdoka += chk + "\r";
            int iByteSen = 0;

            try
            {
                iByteSen = clientsocket.Send(Encoding.GetEncoding(437).GetBytes(sdoka));
            }
            catch (SocketException e)
            {
                if (iByteSen == 0)
                {
                    Console.WriteLine("Conexion cerrada");
                }

                Console.WriteLine("Error al enviar Inicio de recepcion", e.ErrorCode);
                return 4;
            }

            // Read Filename, path and size
            sFileName = GetFieldTCP(clientsocket, xtypes.USING_IPAD);              //Obtiene el nombre del archivo

            if (sFileName == null)
            {
                Console.WriteLine("Error al obtener nombre del archivo");
                //clientsocket.Close();
                return 4;
            }

            #if _DEBUGMODE
                Console.WriteLine("Nombre del archivo: {0}", sFileName);
            #endif

            string sSize;
            sSize = GetFieldTCP(clientsocket, xtypes.USING_IPAD);                  //Obtiene el tamanio del archivo

            if (sSize == null)
            {
                Console.WriteLine("Error al obtener tamanio del archivo");
                //clientsocket.Close();
                return 6;
            }

            #if _DEBUGMODE
                Console.WriteLine("Tamanio del archivo: {0}", sSize);
            #endif

            fileSize = Int32.Parse(sSize, System.Globalization.NumberStyles.HexNumber);

            byte[] baDest = new byte[fileSize];

            if (GetBlockTCP(clientsocket, baDest, fileSize) > 0)        //Obtiene el archivo
            {
                Console.WriteLine("Error al obtener la imagen");
                //clientsocket.Close();
                return 7;
            }

            #if _DEBUGMODE
                Console.WriteLine("Disconnected");
            #endif

            //clientsocket.Close();

            string sFName;
            sFName = sPathDestine;
            sFName += sFileName;
            sFName += ".jpg";
           
            //Nuevo vector que contiene la imagen sin CRC16
            byte[] baDestNew = new byte[fileSize - 4];

            for (UInt32 i = 0; i < fileSize - 4; i++)
            {
                baDestNew[i] = baDest[i];
            }
            
            BinaryWriter writer = new BinaryWriter(File.Open(sFName, FileMode.Create));
            
            try
            {
                writer.Write(baDestNew);
            }
            catch (IOException e)
            {
                Console.WriteLine("Error al escribir la imagen: {0}", e.Message);
                writer.Close();
                return 8;
            }

            sdoka = "OK";
            sdoka += "\t\n";
            chk = new CheckSum().ChkSum(sdoka);
            sdoka += chk + "\r";
            iByteSen = 0;

            try
            {
                iByteSen = clientsocket.Send(Encoding.GetEncoding(437).GetBytes(sdoka));
            }
            catch (SocketException e)
            {
                if (iByteSen == 0)
                {
                    Console.WriteLine("Conexion cerrada");
                }

                Console.WriteLine("Error al enviar Inicio de recepcion", e.ErrorCode);
                return 9;
            }

            #if _DEBUGMODE
                Console.WriteLine("Imagen Guardada");
            #endif

            writer.Close();
            return 0;
        }

        /// <summary>
        /// GetBlockTCP
        /// Recibe la imagen.
        /// </summary>
        /// <param name="client"> Socket abierto para la recepcion de datos</param>
        /// <param name="bDest">Array que alamacena la imagen recibida</param>
        /// <param name="nFileSize">Tamanio de bytes a recibir</param>
        /// Return  0 - OK
        ///         1 - Error al recibir la imagen
        ///         2 - Error en el numero de bytes recibidos
        ///         3 - Error en el CRC del bloque
        ///         4 - Error al enviar respuesta
        ///         5 - Error en el numero de baytes enviados de respuesta
        /// <returns></returns>
        private static int GetBlockTCP(Socket client, byte[] bDest, int nFileSize)
        {
            int iByteRec = 0;
            int iByteSen = 0;
            int iTotalRecv = 0;
            byte[] baDest = new byte[nFileSize];

            try
            {
                do
                {
                    iByteRec = client.Receive(baDest, nFileSize-iTotalRecv, SocketFlags.None);

                    for (int i = 0; i < iByteRec; i++)
                    {
                        bDest[iTotalRecv + i] = baDest[i];
                    }

                    iTotalRecv += iByteRec;

                } while (iByteRec > 0 && iTotalRecv < nFileSize);
            }
            catch (SocketException e)
            {
                if (iByteRec == 0)
                {
                    Console.WriteLine("Conexion cerrada");
                }

                Console.WriteLine("Error al obtener la imagen: {0}", e.SocketErrorCode);
                return 1;
            }

            if (iByteRec == 0 || iTotalRecv != nFileSize)
            {
                if (iByteRec != 0)
                {
                    Console.WriteLine("Error en el tamanio de bytes recibidos");
                }

                Console.WriteLine("Conexion cerrada");
                return 2;
            }

            //--Verifica el CRC16 de la imagen que recive --
            string sdoka;
            byte[] bte = new byte[2];
            char chk;

            Crc16 myobj = new Crc16();

            if(myobj.iCompareCrc16(bDest) > 0){
                sdoka = "ER";

                sdoka += "\t\n";
                chk = new CheckSum().ChkSum(sdoka);
                sdoka += chk + "\r";

                bte = Encoding.GetEncoding(437).GetBytes(sdoka);
                iByteSen = client.Send(bte);
                return 3;
            }

            sdoka = "OK";
            sdoka += "\t\n";
            chk = new CheckSum().ChkSum(sdoka);
            sdoka += chk + "\r";

            bte = Encoding.GetEncoding(437).GetBytes(sdoka);

            try
            {
                iByteSen = client.Send(bte);
            }
            catch (SocketException e)
            {
                if (iByteSen == 0)
                {
                    Console.WriteLine("Conexion cerrada");
                }

                Console.WriteLine("Error al enviar respuesta: {0}", e.SocketErrorCode);
                return 4;
            }

            if (iByteSen != sdoka.Length)
            {
                Console.WriteLine("Error en la transmision de bytes de respuesta");
                return 5;
            }

            return 0;
        }

        /// <summary>
        /// SetBlockTCP
        /// Enviar la imagen en byte
        /// </summary>
        /// <param name="client">Socket abierto para la transmision de imagen</param>
        /// <param name="bSrc"> Array con la imegan guardada tipo byte</param>
        /// <returns>retorna numero mayor que cero en caso de error</returns>
        /// Return  0 - OK
        ///         1 - Error al enviar imagen
        ///         2 - Error en el numero de bytes transmitidos
        ///         3 - Error al recivir respuesta
        ///         4 - Error respuesta incorrecta
        private static short SetBlockTCP(Socket client, byte[] bSrc)
        {
            int iByteSen = 0;
            int iByteRec = 0;

            Crc16 myobj2 = new Crc16();

            ushort crc16result = myobj2.iComputeCrc16(bSrc, bSrc.Length);
            string sCrc16 = crc16result.ToString("X4");
            byte[] bCrc16 = new byte[4];
            bCrc16 = Encoding.GetEncoding(437).GetBytes(sCrc16);
            
            byte[] bFileDataNew = new byte[bSrc.Length + 4];      

            bSrc.CopyTo(bFileDataNew, 0);
            bCrc16.CopyTo(bFileDataNew, bSrc.Length);

            #if _DEBUGMODE
            Console.WriteLine("{0}: CRC de la imagen a transmitir: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, sCrc16);
            #endif

            int iSizeData = bFileDataNew.Length;

            try
            {
                Console.WriteLine("{0}: Bytes a transmitir: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, iSizeData);
                iByteSen = client.Send(bFileDataNew);
            }
            catch (SocketException e)
            {
                if (iByteSen == 0)
                {
                    Console.WriteLine("{0}: Conexion cerrada", System.Reflection.MethodBase.GetCurrentMethod().Name);
                }

                Console.WriteLine("{0}: Error al enviar la imagen: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.ErrorCode);
                return 1;
            }

            if (iByteSen != iSizeData)
            {
                Console.WriteLine("{0}: Error en datos transmitidos: OUT: {1}, REAL: {2}",System.Reflection.MethodBase.GetCurrentMethod().Name, iSizeData, iByteSen);
                return 2;
            }

            Console.WriteLine("{0}: Esperando respuesta de la transmision", System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine("{0}: Bytes transmitidos: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, iByteSen);

            byte[] bRecv = new byte[7];

            try
            {
                iByteRec = client.Receive(bRecv, 7, SocketFlags.None);
            }
            catch (SocketException e)
            {
                if (iByteRec == 0)
                {
                    Console.WriteLine("{0}: Conexion cerrada", System.Reflection.MethodBase.GetCurrentMethod().Name);
                }

                Console.WriteLine("{0}: Error al recibir respuesta: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.ErrorCode);
                return 3;
            }

            string returndata = Encoding.Default.GetString(bRecv);

            if (bRecv[0] == 'W')
            {
                if (returndata.Length > 3 && string.Compare(returndata.Substring(1, 2), "OK") != 0)
                {
                    Console.WriteLine("{0}: Error respuesta no valida: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, returndata);
                    return 4;
                }
            }
            else
            {
                if (returndata.Length > 2 && string.Compare(returndata.Substring(0, 2), "OK") != 0)
                {
                    Console.WriteLine("{0}: Error respuesta no valida: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, returndata);
                    return 4;
                }
            }
            return 0;
        }

        /// <summary>
        /// GetFieldTPC
        /// Espera dato a solicitado
        /// </summary>
        /// <param name="client"> Socket abierto para recepcion de datos</param>
        /// <returns>Retorna string null si hay error</returns>
        private static string GetFieldTCP(Socket client, xtypes xtyp)
        {
            int iByteRec = 0;
            int iByteSen = 0;
            string strTT = null;
            byte[] bBlock = new byte[TCP_BLOCKSIZE];

            try
            {
                client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 2000);
                iByteRec = client.Receive(bBlock, TCP_BLOCKSIZE, SocketFlags.None);
            }
            catch (SocketException e)
            {
                if (iByteRec == 0)
                {
                    Console.WriteLine("Conexion cerrada");
                }

                Console.WriteLine("Error al recivir dato: {0}, {1}", e.ErrorCode, e.SocketErrorCode);
                return strTT;
            }
            
            for (int cnt = 0; cnt < iByteRec; cnt++)
            {
                strTT += System.Convert.ToChar(bBlock[cnt]);
            }

            if (strTT == null)
            {
                return strTT;
            }

            char cChksumIn;
            int iLength = 0;

            char chk = new CheckSum().ChkSum(strTT);
            iLength = strTT.Length;

            cChksumIn = strTT[iLength - 2];
            
            if (cChksumIn != chk)
            {
                Console.WriteLine("Error CheckSum  In: {0}, Calc: {1}", cChksumIn, chk);
                return null;
            }

            strTT = strTT.Substring(0,strTT.LastIndexOf('\t'));

            byte[] bte = new byte[2];
            string sdoka = "OK";

            sdoka += "\t\n";
            chk = new CheckSum().ChkSum(sdoka);
            sdoka += chk + "\r";

            try
            {
                iByteSen = client.Send(Encoding.GetEncoding(437).GetBytes(sdoka));
            }
            catch (SocketException e)
            {
                if (iByteSen == 0)
                {
                    Console.WriteLine("Conexion cerrada");
                }
                    
                Console.WriteLine("Error al enviar respuesta: {0}", e.ErrorCode);
                strTT = null;                
                return strTT;
            }

            if (iByteSen != sdoka.Length)
            {
                #if _DEBUGMODE
                    Console.WriteLine("Error en bytes enviado de respuesta");
                #endif

                strTT = null;
                return strTT;
            }

            if (xtyp == xtypes.USING_IPAD)
            {
                strTT = strTT.Substring(1, strTT.Length - 1);
            }            

            return strTT;
        }
        
        /// <summary>
        /// SetFieldTCP
        /// Enviar un dato al servidor
        /// </summary>
        /// <param name="server">Socket abierto para transmision de datos</param>
        /// <param name="str">Array que contiene el dato a transmitir</param>
        /// <param name="iDEstine">Selecciona destino del dato</param>
        /// <returns>Retorna mayor que cero si hay algun error</returns>
        /// Return  0 - OK
        ///         1 - Error al enviar el dato
        ///         2 - Error en el numero de bytes enviados
        ///         3 - Error al recivir respuesta del servidor
        ///         4 - Error respuesta del servidor incorrecta 
        private static int SetFieldTCP(Socket server, string str)
        {
            byte[] bOut = new byte[str.Length];
            byte[] bret = new byte[7];
            int iByteSen = 0;
            int iByteRec = 0;
            int iSizeStr;
            char chk;

            str += "\t\n";
            chk = new CheckSum().ChkSum(str);
            str += chk + "\r";

            bOut = Encoding.GetEncoding(437).GetBytes(str); 
            iSizeStr = bOut.Length;

            try
            {
                iByteSen  = server.Send(bOut);
            }
            catch (SocketException e)
            {
                if (iByteSen == 0)
                {
                    Console.WriteLine("Conexion cerrada");
                }

                Console.WriteLine("SetFieldTCP Codigo de Error: {0}", e.ErrorCode);               
                return 1;
            }

            if (iByteSen != iSizeStr)
            {
                Console.WriteLine("Error en el numero de bytes transmitidos, OUT: {0}, REAL: {1}", iSizeStr, iByteSen);
                return 2;
            }

            try
            {
                iByteRec = server.Receive(bret, 7, SocketFlags.None);
            }
            catch (SocketException e)
            {
                if (iByteRec == 0)
                {
                    Console.WriteLine("Conexion cerrada");
                }

                Console.WriteLine("SetFieldTCP Codigo de Error: {0}", e.ErrorCode);
                return 3;
            }
            

            string returndata = Encoding.Default.GetString(bret);

            if(bret[0] == 'W')
            {
                if (returndata.Length > 3 && string.Compare(returndata.Substring(1, 2), "OK") != 0)
                {
                    Console.WriteLine("Error respuesta incorrecta: {0}", returndata);
                    return 4;
                }
            }
            else
            {
                if (returndata.Length > 2 && string.Compare(returndata.Substring(0, 2), "OK") != 0)
                {
                    Console.WriteLine("Error respuesta incorrecta: {0}", returndata);
                    return 4;
                }
            }

            return 0;
        }

        /// <summary>
        /// TransferToLinux
        /// This method will transfer an image from the local windows computer to the linux computer.
        /// </summary>
        /// <param name="sSource"> This is the source image file including path. (Ex: c:\images\image1.jpg) </param>
        /// <param name="DestPath"> This is the destination path on the linux computer. (Ex: /images/new) </param>
        /// <param name="xtyp">This is the type of transfer: USING_COM or USING_TCP</param>
        /// <returns> a non-zero return indicates problem with the transfer.</returns>
        /// Retorna 0 - La transferencia fue exitosa.
        /// Retorna 1 - Error al verificar el tamaño de la imagen
        /// Retorna 2 - Error al cargar la imagen
        /// Retorna 3 - Error al conectar con el servidor
        /// Retorna 4 - Error al enviar el nombre del archivo
        /// Retorna 5 - Error al enviar la ruta de destino 
        /// Retorna 6 - Error al enviar el tamaño de la imagen
        /// Retorna 7 - Error al enviar la imagen
        /// Retorne 8 - No recivio confirmacion finalizacion de guardado imagen
        public int TransferToLinux(string sSource, string DestPath, xtypes xtyp, Socket iSock, string sFileName)
        {            
            long fileSize;
            string returndata;
            int iRtaBascula = 0;

            Console.WriteLine("{3}:>>>>> sSource: {0}, DestPath: {1}, sFileName: {2}", sSource, DestPath, sFileName, System.Reflection.MethodBase.GetCurrentMethod().Name);

            DateTime DateTimeObject = DateTime.Now;

            Console.WriteLine(" ");
            Console.WriteLine("{0}: Inicio: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, DateTimeObject.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            if (!System.IO.File.Exists(sSource))
            {
                Console.WriteLine("{0}: No existe archivo para transferir a la bascula", System.Reflection.MethodBase.GetCurrentMethod().Name);
                return 9;
            }

            FileInfo fi = new FileInfo(sSource);

            #if _DEBUGMODE
            Console.WriteLine("{0}: Obteniendo tamanio de la imagen", System.Reflection.MethodBase.GetCurrentMethod().Name);
            #endif            
            
            try
            {
                fileSize = fi.Length;           //Obtiene el tamanio de la imagen
            }
            catch (FileNotFoundException ee)
            {
                Console.WriteLine("Archivo no encontrado: {0}", ee.Message);
                return 1;
            }

            byte[] bFileData = new byte[fileSize];

            #if _DEBUGMODE
            Console.WriteLine("{0}: Cargando la imagen en la Ram", System.Reflection.MethodBase.GetCurrentMethod().Name);
            #endif

            try
            {
                bFileData = File.ReadAllBytes(sSource);     //Carga la imagen en la Ram
            }
            catch (IOException e)
            {
                Console.WriteLine("Error al cargar la imagen: {0}", e.Message);
                return 2;
            }

            #if _DEBUGMODE
            Console.WriteLine("{0}: Codificando en hexadecimal tamaño de la imagen", System.Reflection.MethodBase.GetCurrentMethod().Name);
            #endif

            fileSize += 4;  //Agrega el tamanio del CRC16

            string stmp;
            stmp = fileSize.ToString("X8");

            byte[] bfSize = new byte[8];
            bfSize = Encoding.GetEncoding(437).GetBytes(stmp);               //Pasa tamanio de la imagen de decimal a hexadecimal
            
            if (System.IO.File.Exists(sSource))
            {
                System.IO.File.Delete(sSource);
            }

            if (xtyp == xtypes.USING_COM)
            {
                #region Com

                Console.WriteLine("");
                Console.WriteLine("Sending Ping to Host...");

                if (iPingHost() > 0)
                {   // Alert Server
                    Console.WriteLine("No se recibio PING de respuesta correcto");
                    StopSDK();
                    return 3;
                }

                Console.WriteLine("Sending transfer information");

                if(SetField(sFileName) > 0){
                    Console.WriteLine("Error al enviar el Nombre del archivo");
                    StopSDK();
                    return 4;
                }


                if(SetField(DestPath) != 0){
                    Console.WriteLine("Error al enviar el Path del archivo");
                    StopSDK();
                    return 5;
                }

                if(SetField(stmp) != 0){
                    Console.WriteLine("Error al enviar el Tamaño del archivo");
                    StopSDK();
                    return 6;
                }


                Console.WriteLine("Sending File to Server");

                // Send file Xmodem
                Console.WriteLine("Tamanio de la imagen: {0}", bFileData.Length);

                if (XModem.xmodemSend(bFileData) > 0)
                {
                    Console.WriteLine("Error en la transferencia de datos de la imagen");
                    StopSDK();
                    return 7;
                }
            
                Console.WriteLine("File Transfer Complete.");

                Console.WriteLine("Waiting for ACK save file complete.");

                if (iReceSerialTimeOut() != 0)
                {
                    Console.WriteLine("Timeout en el ping de confirmacion de grabado");
                    StopSDK();
                    return 8;
                }
                else
                {
                    Console.WriteLine("-------------------> Got ACK from host");

                    byte btSing = ctb.Get();
                    if (btSing == ACK)
                    {
                        Console.WriteLine("Save OK.");
                    }
                }

                StopSDK();

                return 0;
                #endregion
            }
            else if (xtyp == xtypes.USING_TCP)
            {

                #region Envia_Comando

                Socket socket = iSock;

                string sCommand = "GJ00\t\n";
                char chk = new CheckSum().ChkSum(sCommand);
                sCommand += chk + "\r";

                byte[] bOut = new byte[10];
                bOut = Encoding.GetEncoding(437).GetBytes(sCommand);
                int iSizeStr = bOut.Length;
                int iByteSen = 0;

                try
                {
                    iByteSen = socket.Send(bOut);
                }
                catch (SocketException e)
                {
                    if (iByteSen == 0)
                    {
                        Console.WriteLine("Conexion cerrada");
                    }

                    Console.WriteLine("SetFieldTCP Codigo de Error: {0}", e.ErrorCode);
                    return 1;
                }
                #endregion

                #region Recibe_Inico_Transmision

                byte[] bret = new byte[7];
                int iByteRec = 0;

                try
                {
                    iByteRec = socket.Receive(bret, 2, SocketFlags.None);
                }
                catch (SocketException e)
                {
                    if (iByteRec == 0)
                    {
                        Console.WriteLine("Conexion cerrada");
                    }

                    Console.WriteLine("Recepcion start transmision: {0}", e.ErrorCode);
                    vWaitAnswerCommand(socket);
                    return 1;
                }
                #endregion


                if (SetFieldTCP(socket, sFileName) > 0)     //Envia el nombre de la imagen
                {
                    Console.WriteLine("{0}: Error Transmitiendo nombre de la imagen: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, sFileName);
                    vWaitAnswerCommand(socket);
                    return 4;
                }

                Console.WriteLine("{0}: Transmitiendo DestPath imagen: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, DestPath);

                if (SetFieldTCP(socket, DestPath) > 0)      //Envia el destino de la imagen en linux
                {
                    Console.WriteLine("{0}: Error Transmitiendo destino de la imagen: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, DestPath);
                    vWaitAnswerCommand(socket);
                    return 5;
                }

                Console.WriteLine("{0}: Transmitiendo tamanio de la imagen: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, fileSize);

                if (SetFieldTCP(socket, Encoding.Default.GetString(bfSize)) > 0)      //Envia el tamajnio de la imagen
                {
                    Console.WriteLine("{0}: Error Transmitiendo tamanio de la imagen: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, Encoding.Default.GetString(bfSize));
                    vWaitAnswerCommand(socket);
                    return 6;
                }

                Console.WriteLine("{0}: Transmitiendo la imagen", System.Reflection.MethodBase.GetCurrentMethod().Name);

                if (SetBlockTCP(socket, bFileData) > 0)     //Envia la imagen a linux
                {
                    vWaitAnswerCommand(socket);
                    return 7;
                }

                Console.WriteLine("{0}: Esperando grabado de la imagen en linux", System.Reflection.MethodBase.GetCurrentMethod().Name);

                byte[] bRecv = new byte[2];

                try
                {
                    iRtaBascula = socket.Receive(bRecv, 2, SocketFlags.None);       //Espera confirmacion de grabado de la imagen
                }
                catch (SocketException e)
                {
                    if (iRtaBascula == 0)
                    {
                        Console.WriteLine("{0}: Conexion cerrada", System.Reflection.MethodBase.GetCurrentMethod().Name);
                    }

                    Console.WriteLine("{0}: Error en la confirmacion de guardado de la imagen: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message);
                    vWaitAnswerCommand(socket);
                    return 10;
                }

                if (iRtaBascula == 0)
                {
                    Console.WriteLine("{0}: La bascula a cerrado la conexion", System.Reflection.MethodBase.GetCurrentMethod().Name);
                    vWaitAnswerCommand(socket);
                    return 8;
                }

                returndata = Encoding.Default.GetString(bRecv);

                if (string.Compare(returndata, "OK") != 0)      //Espera respuesta de termino de escriturta de la imagen
                {
                    if (string.Compare(returndata, "ER") == 0)
                    {
                        Console.WriteLine("{0}: Error CRC de la imagen", System.Reflection.MethodBase.GetCurrentMethod().Name);
                    }
                    vWaitAnswerCommand(socket);
                    return 8;
                }

                Console.WriteLine("{0}: Cerrando Socket", System.Reflection.MethodBase.GetCurrentMethod().Name);

                DateTimeObject = DateTime.Now;

                Console.WriteLine("{0}: Fin: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, DateTimeObject.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                vWaitAnswerCommand(socket);
            }

            return 0;
        }

        public void vWaitAnswerCommand(Socket socket)
        {
            int iRtaBascula = 0;
            byte[] bRecv = new byte[10];

            try
            {
                iRtaBascula = socket.Receive(bRecv, 10, SocketFlags.None);       //Espera confirmacion de grabado de la imagen
            }
            catch (SocketException e)
            {
                if (iRtaBascula == 0)
                {
                    Console.WriteLine("{0}: Conexion cerrada", System.Reflection.MethodBase.GetCurrentMethod().Name);
                }

                Console.WriteLine("{0}: Error en la confirmacion de guardado de la imagen: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message);
                return;
            }

            Console.WriteLine("{0}: RtaCommand: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, Encoding.Default.GetString(bRecv));
        }

        /// <summary>
        /// TransferToIpda
        /// This method will transfer an image from the local windows computer to Ipad.
        /// </summary>
        /// <param name="socket"> This is the socket that containing the connection with the IPAD </param>
        /// <param name="sSource"> This is the source image file including path. (Ex: c:\images\image1.jpg) </param>
        /// <returns> a non-zero return indicates problem with the transfer.</returns>
        /// Retorna 0 - La transferencia fue exitosa.
        /// Retorna 1 - Error al verificar el tamaño de la imagen
        /// Retorna 2 - Error al cargar la imagen
        /// Retorna 3 - Error al enviar el tamaño de la imagen
        /// Retorna 4 - Error al enviar la imagen
        public int TransferToIpad(Socket socket, string sSource)
        {
            long fileSize;

            FileInfo fi = new FileInfo(sSource);

            try
            {
                fileSize = fi.Length;           //Obtiene tamanio de la imagen
            }
            catch (FileNotFoundException ee)
            {
                Console.WriteLine("Error al obtener la longitud del archivo: {0}", ee.Message);
                return 1;
            }

            byte[] bFileData = new byte[fileSize];

            try
            {
                bFileData = File.ReadAllBytes(sSource); //Carga la imagen en la ram
            }
            catch (IOException e)
            {                
                Console.WriteLine("Error al cargar la imagen: {0}", e.Message);
                return 2;
            }

            string sFileName;
            sFileName = Path.GetFileName(sSource);

            fileSize += 4; //Suma bytes de crc16

            string sfSize = fileSize.ToString("D8");
            
            Console.WriteLine("Transmitiendo tamanio de la imagen: {0}", sfSize);

            if (SetFieldTCP(socket, sfSize) > 0)      //Envia el tamanio de la imagen
            {
                Console.WriteLine("Error Transmitiendo tamanio de la imagen: {0}");
                //socket.Close();
                return 3;
            }

            Console.WriteLine("Enviando imagen imagen al IPAD");

            if (SetBlockTCP(socket, bFileData) > 0)         //Envia la imagen
            {
                Console.WriteLine("Error al enviar imagen");
                //socket.Close();
                return 4;
            }

            //socket.Close();
            return 0;
        }
    }

    // internal classes
    public class myCircBuff<T>
    {
        private int capacity;
        private volatile int size;
        private volatile int head;
        private volatile int tail;
        private volatile T[] buffer;

        public myCircBuff(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentException("capacity must be greater than or equal to zero.", "capacity");

            this.capacity = capacity;
            size = 0;
            head = 0;
            tail = 0;
            buffer = new T[capacity + 1];
        }

        public int Size
        {
            get { return size; }
        }

        public bool Contains(T item)
        {
            int bufferIndex = head;
            var comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < size; i++, bufferIndex++)
            {
                if (bufferIndex == capacity)
                    bufferIndex = 0;

                if (item == null && buffer[bufferIndex] == null)
                    return true;
                else if ((buffer[bufferIndex] != null) &&
                    comparer.Equals(buffer[bufferIndex], item))
                    return true;
            }

            return false;
        }

        public void Clear()
        {
            size = 0;
            head = 0;
            tail = 0;
        }

        public void Put(T item)
        {
            if (size == capacity)
                throw new InternalBufferOverflowException("Buffer is full.");

            buffer[tail] = item;
            if (tail++ == capacity)
                tail = 0;
            size++;
        }

        public T Get()
        {
            //            if (size == 0)
            //                throw new InvalidOperationException("Buffer is empty.");

            var item = buffer[head];

            if (head++ == capacity)
                head = 0;
            size--;

            return item;
        }
    }

    public class XModem
    {
        const short MAXRETRANS = 3;
        public static string strXModemError;
        const short _PAYLOADSIZE = 2048;

        public static int xmodemSend(byte[] bSrc)
        {
            byte[] ackBuf = new byte[2];        // used to store response after frame

            byte PacketNo = 0;
            byte bResp;
            byte[] outBuf;

            short count = 0;

            int bContinue = 1;
            int nIndex = 0;
            int reSend = 0;
            int srcsz = bSrc.Length;


            Crc16 myobj2 = new Crc16();

            ushort crc16result = myobj2.iComputeCrc16(bSrc, bSrc.Length);
            string sCrc16 = crc16result.ToString("X4");
            byte[] bCrc16 = new byte[4];
            bCrc16 = Encoding.Default.GetBytes(sCrc16);

            byte[] bFileDataNew = new byte[bSrc.Length + 4];

            bSrc.CopyTo(bFileDataNew, 0);
            bCrc16.CopyTo(bFileDataNew, bSrc.Length);

            Console.WriteLine("{0}: CRC de la imagen a transmitir: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, sCrc16);

            srcsz = bFileDataNew.Length;

            while (bContinue == 1)
            {

                outBuf = new byte[_PAYLOADSIZE + 2];           // new for each packet
                outBuf[0] = PacketNo;

                for (count = 1; count < _PAYLOADSIZE + 1; count++)
                {
                    if (nIndex == srcsz)
                    {                            // If we are about to exceed data buffer size
                        bContinue = 0;                               // stop the process
                        count = _PAYLOADSIZE + 1; //stop the process
                        break;
                    }
                    else
                    {
                        outBuf[count] = bFileDataNew[nIndex];               // Build out buffer based on index in data buffer
                        nIndex++;                                     // Increment the total bytes sent index
                    }
                } // end of a single packet

                DateTime DateTimeObject = DateTime.Now;
                Console.WriteLine("{0}: Time: {1} PacketNo: {2}     retries: {3}     nIndex: {4}", System.Reflection.MethodBase.GetCurrentMethod().Name, DateTimeObject.ToString("yyyy-MM-dd HH:mm:ss.fff"), PacketNo, reSend, nIndex);


                outBuf[_PAYLOADSIZE + 1] = SGSDKCOM.EOT;                    // set the end of packet byte

                SGSDKCOM.SetBlock(outBuf);
                bResp = SGSDKCOM.XmodemResp();                       // Wait for Response from receiver ***¨¨¨¨****

                if (bResp == SGSDKCOM.ACK)
                {
                    Console.WriteLine("Llego ACK");

                    if (PacketNo == 255) PacketNo = 0;  // Handle the packet increment
                    else PacketNo++;

                    reSend = 0;
                }
                else if (bResp == SGSDKCOM.NAK)
                {
                    Console.WriteLine("{0}: Respuesta de NAK para reenviar paquete", System.Reflection.MethodBase.GetCurrentMethod().Name);
                    // Re-send the packet
                    nIndex -= _PAYLOADSIZE;               // reset the last packet, roll back the Total index
                    reSend++;
                    if (reSend > MAXRETRANS)
                    {             // Too many resends, we need to bail
                        return 3;
                    }
                }
                else if (bResp == 0)
                {
                    Console.WriteLine("{0}: Timeout en la recepcion de bytes", System.Reflection.MethodBase.GetCurrentMethod().Name);
                    nIndex -= _PAYLOADSIZE;               // reset the last packet, roll back the Total index
                    reSend++;
                    if (reSend > MAXRETRANS) return 1;  // Too many resends, we need to bail

                }
                else
                {
                    Console.WriteLine("{0}: Rta desconocida", System.Reflection.MethodBase.GetCurrentMethod().Name);
                    nIndex -= _PAYLOADSIZE;               // reset the last packet, roll back the Total index
                    reSend++;
                    if (reSend > MAXRETRANS) return 2; // Too many resends, we need to bail

                }
            } // end of the true loop

            return 0;            // all is well
        }

        //public static int xmodemSendORIGINAL(byte[] bSrc)
        //{
        //    byte[] ackBuf = new byte[2];        // used to store response after frame

        //    byte PacketNo = 0;
        //    byte bResp;
        //    byte[] outBuf;

        //    short count = 0;

        //    int bContinue = 1;
        //    int nIndex = 0;
        //    int reSend = 0;
        //    int srcsz = bSrc.Length;             


        //   Crc16 myobj2 = new Crc16();

        //    ushort crc16result = myobj2.iComputeCrc16(bSrc, bSrc.Length);
        //    string sCrc16 = crc16result.ToString("X4");
        //    byte[] bCrc16 = new byte[4];
        //    bCrc16 = Encoding.GetEncoding(437).GetBytes(sCrc16);
            
        //    byte[] bFileDataNew = new byte[bSrc.Length + 4];      

        //    bSrc.CopyTo(bFileDataNew, 0);
        //    bCrc16.CopyTo(bFileDataNew, bSrc.Length);

        //    Console.WriteLine("{0}: CRC de la imagen a transmitir: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, sCrc16);

        //    srcsz = bFileDataNew.Length;            

        //    while (bContinue == 1)
        //    {
        //        DateTime DateTimeObject = DateTime.Now;
        //        Console.WriteLine("{0}:\t\t\tTime: {1}\n\t\t\t\tPacketNo: {2}\n\t\t\t\t Retries: {3}", System.Reflection.MethodBase.GetCurrentMethod().Name, DateTimeObject.ToString("yyyy-MM-dd HH:mm:ss.fff"), PacketNo, reSend);

        //        outBuf = new byte[_PAYLOADSIZE + 2];           // new for each packet
        //        outBuf[0] = PacketNo;

        //        for (count = 1; count < _PAYLOADSIZE + 1; count++)
        //        {
        //            if (nIndex == srcsz)
        //            {                            // If we are about to exceed data buffer size
        //                bContinue = 0;                               // stop the process
        //                break;
        //            }
        //            else
        //            {
        //                outBuf[count] = bFileDataNew[nIndex];               // Build out buffer based on index in data buffer
        //                nIndex++;                                     // Increment the total bytes sent index
        //            }
        //        } // end of a single packet

        //        outBuf[_PAYLOADSIZE + 1] = SGSDKCOM.EOT;                    // set the end of packet byte

        //        SGSDKCOM.SetBlock(outBuf);
        //        bResp = SGSDKCOM.XmodemResp();                       // Wait for Response from receiver

        //        if (bResp == SGSDKCOM.ACK)
        //        {
        //            Console.WriteLine("{0}: ACK received", System.Reflection.MethodBase.GetCurrentMethod().Name);
        //            if (PacketNo == 255)
        //            {					// Handle the packet increment
        //                PacketNo = 0;
        //            }
        //            else
        //            {
        //                PacketNo++;
        //            }
        //            Console.WriteLine("{0}: Packet No: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, PacketNo);
        //        }
        //        else if (bResp == SGSDKCOM.NAK)
        //        {
        //            Console.WriteLine("{0}: Respuesta de NAK para reenviar paquete", System.Reflection.MethodBase.GetCurrentMethod().Name);
        //            // Re-send the packet
        //            nIndex -= _PAYLOADSIZE;               // reset the last packet, roll back the Total index
        //            reSend++;
        //            if (reSend > MAXRETRANS)
        //            {             // Too many resends, we need to bail
        //                return 3;
        //            }
        //        }
        //        else if (bResp == 0)
        //        {
        //            Console.WriteLine("{0}: Timeout en la recepcion de bytes", System.Reflection.MethodBase.GetCurrentMethod().Name);
        //            nIndex -= _PAYLOADSIZE;               // reset the last packet, roll back the Total index
        //            reSend++;
        //            if (reSend > MAXRETRANS)
        //            {             // Too many resends, we need to bail
        //                return 1;
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("{0}: Rta desconocida", System.Reflection.MethodBase.GetCurrentMethod().Name);
        //            nIndex -= _PAYLOADSIZE;               // reset the last packet, roll back the Total index
        //            return 2;
        //        }
        //    } // end of the true loop

        //    return 0;            // all is well
        //}

        public static int xmodemReceive(byte[] bDest)
        {
            byte[] inBuf;
            byte PacketNo = 0;
            short count = 0;
            int bContinue = 1;
            long nIndex = 0;
            int reSend = 0;
            int DestSize = bDest.Length;                        // Total Out file buffer
            int nTotalResend = 0;

            while (bContinue == 1)
            {
                Console.WriteLine("Reciving packet {0}.. retries {1}", PacketNo, nTotalResend);

                inBuf = new byte[_PAYLOADSIZE + 2];
                // Read the incoming packet
                if (SGSDKCOM.ReadSerialPacket(inBuf) == 0)
                {
                    if (inBuf[0] == PacketNo)
                    {
                        reSend = 0;
                        nTotalResend = 0;
                    }
                    else
                    {
                        reSend = 1;                                // Packets to not match, ask for re-send at end
                    }
                }
                else
                {
                    reSend = 1;
                }

                for (count = 1; count < _PAYLOADSIZE + 1; count++)
                {
                    if (nIndex == DestSize)
                    {                            // If we are about to exceed data buffer size
                        bContinue = 0;                               // stop the process
                        break;
                    }
                    else
                    {
                        bDest[nIndex] = inBuf[count];                  // Add payload to file dest buffer                        
                        nIndex++;                                     // Increment the total bytes received index
                    }
                } // end of a single packet

                Console.WriteLine("Processed packet... {0} expected Retry count {1}", PacketNo, nTotalResend);

                if (reSend == 0)
                {
                    // bump the current packet
                    if (PacketNo == 255)
                    {
                        PacketNo = 0;
                    }
                    else
                    {
                        PacketNo++;
                    }

                    Console.WriteLine("Enviando ACK");
                    SGSDKCOM.sendACK();
                }
                else
                {
                    nIndex -= _PAYLOADSIZE;               // reset the last packet, roll back the Total index
                    #if _DEBUGMODE
                        Console.WriteLine("Asking for resend");
                    #endif
                    nTotalResend++;
                    SGSDKCOM.ctb.Clear();
                    Console.WriteLine("Enviando NAK");
                    SGSDKCOM.sendNAK();
                }
            } // end of the true loop


            //--Verifica el CRC16 de la imagen que recive --
            Crc16 myobj = new Crc16();

            if (myobj.iCompareCrc16(bDest) > 0)
            {
                return 1;
            }

            return 0;
        }
    }

}
