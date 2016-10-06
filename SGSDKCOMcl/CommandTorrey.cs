#define _DEBUGMODE                 // Leave this defined to view debug output to the console

using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGSDK;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Drawing.Drawing2D;
using SocketConexion;
using Validaciones;
using System.Threading;


namespace TorreyTransfer
{
    public class CommandTorrey
    {
        public string ComErrString;
        public static int iImageIpad = 200;
        public static int iImageScale = 235;
        public static int iSimageScale = 77;
        public static int iSsimageScale = 48;
        public static int iScreenSaverWidth = 800;
        public static int iScreenSaverHigh  = 600;
        public static int iTestImageUsb = 105;
        public static int iTestImageSdcard = 125;
        public static int iLogoHeight = 110;
        public static int iLogoWidth = 245;
        public static int iAdHeight = 382;
        public static int iAdWidth = 373;

        internal static SerialPort _serialPort;
        public enum xtypes { USING_COM = 1, USING_TCP };
        public static myCircBuff<byte> ctb = new myCircBuff<byte>(2048);
        public static byte[] InArray = new byte[131];

        public static ManualResetEvent reciveDoneSerial1 = new ManualResetEvent(false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sComPort"></param>
        /// <returns></returns>
        public int iConfigCom(string sComPort)
        {

            _serialPort = new SerialPort();
            // set the appropriate properties to talk to linux connection
            _serialPort.PortName = sComPort;
            _serialPort.BaudRate = 115200;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;

            // Set the read/write timeouts
            _serialPort.ReadTimeout = 2;
            _serialPort.WriteTimeout = 1;
            _serialPort.Encoding = Encoding.GetEncoding(1252);  // use this to receive all 255 values from char to byte * per MSDN blog

            _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler1);

            try
            {
                if (!_serialPort.IsOpen)
                    _serialPort.Open();
                Console.WriteLine("{0}: COM port {1} opened!", System.Reflection.MethodBase.GetCurrentMethod().Name, sComPort);
            }
            catch (Exception ex)
            {
                // Nothing we can really do about it here at the moment
                ComErrString = ex.Message;    // Save the message in case someone wants to read it
                Console.WriteLine("Error al abrir el puerto COM {0}", ComErrString);
                return 1;
            }

            return 0;
        }

        private static void DataReceivedHandler1(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string inStr;
            int iCnt = 0;
            int nMax = 0;

            if (sp.BytesToRead > 0)
            {
                inStr = sp.ReadExisting();
                InArray = Encoding.GetEncoding(437).GetBytes(inStr);
                nMax = InArray.Length;
                for (iCnt = 0; iCnt < nMax; iCnt++)
                {
                    CommandTorrey.ctb.Put(InArray[iCnt]);
                    reciveDoneSerial1.Set();
                }
            }
            iCnt = InArray.Length;
            Array.Clear(InArray, 0, iCnt); // clear the byte array
        }

        public int iReceSerialTimeOut1()
        {
            reciveDoneSerial1.Reset();

            if (ctb.Size <= 0)
            {
                if (reciveDoneSerial1.WaitOne(3500, false))
                {
                    if (ctb.Size <= 0)
                    {
                        reciveDoneSerial1.Reset();
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
                        reciveDoneSerial1.Reset();
                        return -1;
                    }
                }
            }

            reciveDoneSerial1.Reset();
            return 0;
        }

        /// <summary>
        /// TORREYCommandBascula
        /// envia un comando hacia la bascula
        /// </summary>
        /// <param name="sCommand"></param>
        /// <param name="sIpScale"></param>
        /// <returns>Retorna mayor que cero si hay algun error</returns>
        /// Return  0 - OK
        ///         1 - Error al abrir el puerto COM
        ///         2 - Error al enviar el comando
        ///         3 - Error al recibir la respuesta de la bascula
        public int TORREYCommandBasculaSerial(string sCommand, string sComPort)
        {

            if (iConfigCom(sComPort) != 0)
            {
                Console.WriteLine();
                return 1;
            }

            #if _DEBUGMODE
                Console.WriteLine("{0}: Connecting with scale {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, sComPort);
            #endif


            #if _DEBUGMODE
                //Console.WriteLine("{0}: Connected with {1} at port {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, sComPort);
            #endif

            sCommand += "\t\n";
            char chk = new CheckSum().ChkSum(sCommand);
            sCommand += chk + "\r";
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Command to send: {0}", sCommand);
            Console.WriteLine("Command checksum: {0}", chk);
            byte[] bOut = new byte[sCommand.Length];
            bOut = Encoding.GetEncoding(437).GetBytes(sCommand);
            string sOut = "";
            foreach(byte b in bOut) {
                sOut = sOut + "[" + b.ToString() + "]";
            }
            Console.WriteLine("{0}: Send command", System.Reflection.MethodBase.GetCurrentMethod().Name);
            Console.WriteLine("{0}: Command = {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, sCommand);
            Console.WriteLine("{0}: CommandBytes = {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, sOut);
            Console.ForegroundColor = ConsoleColor.White;

            int nLen = bOut.Length;

            try
            {
                _serialPort.Write(bOut, 0, nLen);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error {0}", e);
                return 4;
            }

            byte btSing1;
            byte btSing2;


            if (iReceSerialTimeOut1() != 0)
            {
                _serialPort.Close();
                Console.WriteLine("{0}: TimeOut 1 receiving answer of command {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, sCommand);
                return 2;    
            }
            
            btSing1 = ctb.Get();

            if (iReceSerialTimeOut1() != 0)
            {
                _serialPort.Close();
                Console.WriteLine("{0}: TimeOut 2 receiving answer of command {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, sCommand);
                return 2;
            }

            btSing2 = ctb.Get();

            string returndata = Convert.ToChar(btSing1).ToString() + Convert.ToChar(btSing2).ToString();

            if (string.Compare(returndata, "OK") != 0)
            {
                Console.WriteLine("{0}: Error receiving answer of command {1}: {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, sCommand, returndata);
                _serialPort.Close();
                return 3;
            }

            _serialPort.Close();

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iDestImage"></param>
        /// <param name="sComPort"></param>
        /// <param name="sSourceFile"></param>
        /// <returns>Retorna mayor que cero si hay algun error</returns>
        /// Return  0 - OK
        ///         1 - Error en comando FILESERVER_PutFile
        ///         2 - Error al cambiar tamnio de la imagen
        ///         3 - Error al enviar la imagen
        ///         4 - Error al abrir el puerto COM
        public int TORREYSendImageToScaleSerial(int iDestImage, string sComPort, string sSourceFile)
        {
            string sSourcefile = null;
            string sPathDestino;
            int rta_transfer = 0;

            #if _DEBUGMODE
                Console.WriteLine("{0}: Enviando comando para transmitir imagen", System.Reflection.MethodBase.GetCurrentMethod().Name);
            #endif

            if (TORREYCommandBasculaSerial("FILESERVER_PutFile\n", sComPort) == 0)
            {
                #if _DEBUGMODE
                    Console.WriteLine("{0}: Enviando comando.. OK\n", System.Reflection.MethodBase.GetCurrentMethod().Name);
                #endif
            }
            else
            {
                Console.WriteLine("{0}: Enviando comando.. BAD\n", System.Reflection.MethodBase.GetCurrentMethod().Name);
                return 1;
            }

            Console.WriteLine("{0}: Creando objeto para la conexion", System.Reflection.MethodBase.GetCurrentMethod().Name);

            SGSDKCOM myobj = new SGSDKCOM(sComPort, "", 0);  // For listen loop any IP address will be fine

            if (myobj.bComStart == false)
            {
                Console.WriteLine("{0}: Error al abrir el puerto {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, sComPort);
                return 4;
            }

            sSourcefile = resizeImage(iDestImage, sSourceFile, sComPort);

            string sFileName = Path.GetFileName(sSourceFile);

            if (sSourcefile != null)
            {
                #if _DEBUGMODE
                    Console.WriteLine("{0}: Iniciando transmision de imagen1 hacia la bascula", System.Reflection.MethodBase.GetCurrentMethod().Name);
                #endif

                switch (iDestImage)
                {
                    case 235:
                        sPathDestino = "/sdcard/images/user/pcimages/";
                        break;

                    case 77:
                        sPathDestino = "/sdcard/s_images/user/pcimages/";
                        break;

                    case 48:
                        sPathDestino = "/sdcard/ss_images/user/pcimages/";
                        break;
                    case 110:
                        sPathDestino = "/sdcard/logos/";
                        break;
                    case 382:
                        sPathDestino = "/sdcard/adds/";
                        break;
                    default:
                        sPathDestino = "/sdcard/splash/";
                        break;
                }

                rta_transfer = myobj.TransferToLinux(sSourcefile, sPathDestino, SGSDK.SGSDKCOM.xtypes.USING_COM, null, sFileName);
            }
            else
            {
                Console.WriteLine("{0}: Error en el patch o nombre de  la imagen", System.Reflection.MethodBase.GetCurrentMethod().Name);
                return 2;
            }

            if (rta_transfer == 0)
            {
                #if _DEBUGMODE
                    Console.WriteLine("{0}: Transferencia de la imagen.. OK\n", System.Reflection.MethodBase.GetCurrentMethod().Name);
                #endif
            }
            else
            {
                Console.WriteLine("{0}: Transferencia de la imagen.. BAD {0}", System.Reflection.MethodBase.GetCurrentMethod().Name, rta_transfer);
                return 3;
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sComPort"></param>
        /// <param name="sPathImage"></param>
        /// <param name="sTypeData"></param>
        /// <returns></returns>
        public int TORREYSendImagesToScaleSerial(string sComPort, string sPathImage, string sTypeData)
        {

            int iRtaFunct = 0;

            if (System.IO.File.Exists(sPathImage))
            {

                if (string.Compare(sTypeData, "Product") == 0)
                {
                    iRtaFunct = TORREYSendImageToScaleSerial(iImageScale, sComPort, sPathImage);

                    if (iRtaFunct > 0)
                    {
                        Console.WriteLine("Error Send Image - TORREYSendImageToScale (Image)");
                        return 1;
                    }

                    iRtaFunct = TORREYSendImageToScaleSerial(iSimageScale, sComPort, sPathImage);

                    if (iRtaFunct > 0)
                    {
                        Console.WriteLine("Error Send Image - TORREYSendImageToScale (s_Image)");
                        return 2;
                    }

                    /*iRtaFunct = TORREYSendImageToScale(iSsimageScale, sIpScale, sPathImage);

                    if (iRtaFunct > 0)
                    {
                        Console.WriteLine("Error Send Image - TORREYSendImageToScale (ss_Image)");
                        return 3;
                    }*/

                }
                else if (string.Compare(sTypeData, "Splash") == 0)
                {

                    iRtaFunct = TORREYSendImageToScaleSerial(iScreenSaverHigh, sComPort, sPathImage);

                    if (iRtaFunct > 0)
                    {
                        Console.WriteLine("Error Send Image - TORREYSendImageToScale (splash)");
                        return 4;
                    }
                }
                else if (string.Compare(sTypeData, "Ad") == 0)
                {

                    iRtaFunct = TORREYSendImageToScaleSerial(iAdHeight, sComPort, sPathImage);

                    if (iRtaFunct > 0)
                    {
                        Console.WriteLine("Error Send Image - TORREYSendImageToScale (ad)");
                        return 5;
                    }
                }
                else if (string.Compare(sTypeData, "Logo") == 0)
                {

                    iRtaFunct = TORREYSendImageToScaleSerial(iLogoHeight, sComPort, sPathImage);

                    if (iRtaFunct > 0)
                    {
                        Console.WriteLine("Error Send Image - TORREYSendImageToScale (logo)");
                        return 6;
                    }
                }
            }
            else
            {
                Console.WriteLine("No existe archivo");
                return 4;
            }

            return 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sComPort"></param>
        /// <param name="sNameImage"></param>
        /// <param name="sPathImageDestine"></param>
        /// <returns></returns>
        public int TORREYGetImageFromScaleSerial(string sComPort, string sNameImage, string sPathImageDestine)
        {
            int iRatCommand = 0;
            int iRtaGetImg = 0;

            iRatCommand = TORREYCommandBasculaSerial("FILESERVER_GetFile," + sNameImage + "\n", sComPort);

            if (iRatCommand > 0)
            {
                Console.WriteLine("Error al enviar el comando FILESERVER_SetRemoteDIR, error {0}", iRatCommand);
                return 2;
            }

            // Server to receive file form linux
            SGSDKCOM myobj = new SGSDKCOM(sComPort, "", 0);  // For listen loop any IP address will be fine

            iRtaGetImg = myobj.ListenLoopCom(sPathImageDestine);

            if (iRtaGetImg > 0)
            {
                Console.WriteLine("Error en la recepcion de imagen: {0}", iRtaGetImg);
                return 3;
            }

            return 0;
        }

        //----------------------------------------------------------------------------------------

        /// <summary>
        /// TORREYCommandBascula
        /// envia un comando hacia la bascula
        /// </summary>
        /// <param name="sCommand"></param>
        /// <param name="sIpScale"></param>
        /// <returns>Retorna mayor que cero si hay algun error</returns>
        /// Return  0 - OK
        ///         1 - Error al intentar la conexion
        ///         2 - Error al enviar el comando
        ///         3 - Error al recibir la respuesta del servidor
        ///         4 - Error en la respuesta del servidor
        public int TORREYCommandBascula(string sCommand, string sIpScale)
        {

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(sIpScale), 50036);

            SocketOperation tcpConnection = new SocketOperation();

            #if _DEBUGMODE
                Console.WriteLine("{0}: Connecting with scale {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ip.Address);
            #endif

            socket = tcpConnection.iConnectServer(socket, ip);          //Conexion con timeout

            if (socket == null)
            {
                Console.WriteLine("{0}: Unable to connect to server", System.Reflection.MethodBase.GetCurrentMethod().Name);
                return 1;
            }

            IPEndPoint clientep = (IPEndPoint)socket.RemoteEndPoint;

            #if _DEBUGMODE  
                Console.WriteLine("{0}: Connected with {1} at port {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, clientep.Address, clientep.Port);
            #endif

            byte[] bOut = new byte[sCommand.Length];
            bOut = Encoding.GetEncoding(437).GetBytes(sCommand);

            #if _DEBUGMODE
                Console.WriteLine("{0}: Send command", System.Reflection.MethodBase.GetCurrentMethod().Name, clientep.Address, clientep.Port);
            #endif

            try
            {
                socket.Send(bOut);
            }
            catch (SocketException e)
            {
                Console.WriteLine("{0}: Error send command {1}: {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, sCommand, e.Message);
                socket.Close();
                return 2;
            }
            catch (ObjectDisposedException e)
            {
                Console.WriteLine("{0}: Error send command {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message);
                socket.Close();
                return 2;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("{0}: Error send command {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message);
                socket.Close();
                return 2;
            }

            byte[] bret = new byte[2];

            try
            {
                socket.Receive(bret, 2, SocketFlags.None);  // we throw it away.
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("{0}: Error send command {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message);
                socket.Close();
                return 3;
            }
            catch (SocketException e)
            {
                Console.WriteLine("{0}: Error socket receiving answer: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message);
                socket.Close();
                return 3;
            }
            catch (ObjectDisposedException e)
            {
                Console.WriteLine("{0}: Error socket receiving answer: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message);
                socket.Close();
                return 3;
            }

            string returndata = Encoding.Default.GetString(bret);

            if (string.Compare(returndata, "OK") != 0)
            {
                Console.WriteLine("{0}: Error receiving answer of command {1}: {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, sCommand, returndata);
                socket.Close();
                return 4;
            }

            socket.Close();
            return 0;
        }

        /// <summary>
        /// TORREYConfigConenection
        /// Configura la IP del servidor en la Bascula
        /// </summary>
        /// <param name="sIpScale"></param>
        /// <param name="sIpPc"></param>
        /// <returns>Retorna mayor que cero si hay algun error</returns>
        /// Return  0 - OK
        ///         1 - Error en el comando
        public int TORREYConfigConnection(string sIpScale, string sIpPc)
        {
            int iRatCommand = 0;

            iRatCommand = TORREYCommandBascula("FILESERVER_SetIp," + sIpPc + "\n", sIpScale);

            if (iRatCommand > 0)
            {
                Console.WriteLine("Error al enviar el comando FILESERVER_SetIp, error {0}", iRatCommand);
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// TORREYGetImageFromScale
        /// Obtiene una imagen desde la bascula
        /// </summary>
        /// <param name="sIpScale"></param>
        /// <param name="sPathImageSource"></param>
        /// <param name="sPathImageDestine"></param>
        /// <returns>Retorna mayor que cero si hay algun error</returns>
        /// Return  0 - OK 
        ///         1 - Error en el comando FILESERVER_SetRemoteDIR
        ///         2 - Error en el comando FILESERVER_GetFile
        ///         3 - Error en la recepcion de la imagen
        public int TORREYGetImageFromScale(string sIpScale, string sPathImageOrigen, string sPathImageDestine, Socket iSock)
        {
            int iRtaGetImg = 0;

            /*iRatCommand = TORREYConfigConnection(sIpScale, sIpPc);

            if (iRatCommand > 0)
            {
                Console.WriteLine("Error al enviar el comando ConfigIP, error {0}", iRatCommand);
                return 1;
            }

            iRatCommand = TORREYCommandBascula("FILESERVER_GetFile," + sNameImage + "\n", sIpScale);

            if (iRatCommand > 0)
            {
                Console.WriteLine("Error al enviar el comando FILESERVER_SetRemoteDIR, error {0}", iRatCommand);
                return 2;
            }
            */
            // Server to receive file form linux
            SGSDKCOM myobj = new SGSDKCOM("", sIpScale, 1233);  // For listen loop any IP address will be fine

            iRtaGetImg = myobj.ListenToScale(sPathImageDestine, sPathImageOrigen, iSock);

            if (iRtaGetImg > 0)
            {
                Console.WriteLine("Error en la recepcion de imagen: {0}", iRtaGetImg);
                return 3;
            }

            //myobj.StopSDK();
            return 0;
        }
        
        /// <summary>
        /// TORREYGetImageFromIpad
        /// Obtiene una imagen desde la bascula
        /// </summary>
        /// <param name="sIpScale"></param>
        /// <param name="sPathImageSource"></param>
        /// <param name="sPathImageDestine"></param>
        /// <returns>Retorna mayor que cero si hay algun error</returns>
        /// Return  0 - OK 
        public int TORREYGetImageFromIpad(Socket clientsocket, string sPathDestine)
        {
            SGSDKCOM myobj = new SGSDKCOM("", "", 50001);

            if (myobj.ListenToIpad(clientsocket, sPathDestine) > 0)
            {
                Console.WriteLine("Error Rec Image - TORREYGetImageToIpad");
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// reszeImage
        /// Ajusta el tamanio de la imagen para su envio
        /// </summary>
        /// <param name="iDestImage"></param>
        /// <param name="sSourceFile"></param>
        /// <param name="sNameFile"></param>
        /// <returns>Retorna string null si hay algun error</returns>
        private static string resizeImage(int iDestImage, string sSourceFile, string sIp)
        {
            string sPathImage = null;
            Bitmap imgToResize;

            if (iDestImage == iTestImageUsb || iDestImage == iTestImageSdcard || iDestImage == iLogoHeight)
            {
                string sourceFileName = System.IO.Path.GetFileName(sSourceFile);
                string targetPath = @"C:\windows\temp";
                string destFile = System.IO.Path.Combine(targetPath, sourceFileName);
                System.IO.File.Copy(sSourceFile, destFile);
                return destFile;
            }

            if (iDestImage <= 0 || sSourceFile == "")
            {
                Console.WriteLine("Error en algun argumento");
                return sPathImage;
            }

            try
            {
                imgToResize = new Bitmap(sSourceFile, true);
            }
            catch (ArgumentException)
            {

                Console.WriteLine("No se pudo cargar la imagen");
                return sPathImage;
            }

            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            int destHeight = 0;
            int destWidth = 0;

            float nPercent = 0;

            string sFileName = Path.GetFileName(sSourceFile);

            sPathImage = "C:/windows/temp/";
            if (iDestImage == iSimageScale || iDestImage == iImageScale || iDestImage == iSsimageScale)
            {
                sPathImage += sIp + sFileName;
            }
            else
            {
                sPathImage += sFileName;
            }

            nPercent = ((float)iDestImage / (float)sourceHeight);
            destHeight = (int)(sourceHeight * nPercent);
            destWidth =  (int)(sourceWidth * nPercent);

            if (iDestImage == iSsimageScale)
            {
                nPercent = ((float)iDestImage / (float)sourceWidth);
                destWidth = (int)(sourceWidth * nPercent);
            }
            else if (iDestImage == iScreenSaverHigh)
            {
                nPercent = ((float)iScreenSaverHigh / (float)sourceHeight);
                destHeight = (int)(sourceHeight * nPercent);
                nPercent = ((float)iScreenSaverWidth / (float)sourceWidth);
                destWidth = (int)(sourceWidth * nPercent);
            }
            else if (iDestImage == iAdHeight)
            {
                nPercent = ((float)iAdHeight / (float)sourceHeight);
                destHeight = (int)(sourceHeight * nPercent);
                nPercent = ((float)iAdWidth / (float)sourceWidth);
                destWidth = (int)(sourceWidth * nPercent);
            }

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);

            b.Save(sPathImage, System.Drawing.Imaging.ImageFormat.Jpeg);

            g.Dispose();
            imgToResize.Dispose();

            return sPathImage;
        }

        /// <summary>
        /// TORREYSendImagesToScale
        /// Envia tres imagenes hacia la bascula
        /// </summary>
        /// <param name="sIpScale"></param>
        /// <param name="sPathImage"></param>
        /// <param name="sTypeData"></param>
        /// <returns></returns>
        /// Return  0 - OK
        ///     1 - Error al enviar hacia la carpeta image
        ///     2 - Error al enviar hacia la carpeta s_image
        ///     3 - Error al enviar hacia la carpeta ss_image
        ///     4 - Error al enviar hacia la carpeta splash
        ///     5 - Error al enviar hacia la carpeta ads
        ///     6 - Error al enviar hacia la carpeta logo
        public int TORREYSendImagesToScale(string sIpScale, string sPathImage ,string sTypeData, Socket iSock)
        {

            int iRtaFunct = 0;

            if (System.IO.File.Exists(sPathImage))
            {

                   if(string.Compare(sTypeData, "Product") == 0){
                    iRtaFunct = TORREYSendImageToScale(iImageScale, sIpScale, sPathImage, iSock);

                    if (iRtaFunct > 0)
                    {
                        Console.WriteLine("Error Send Image - TORREYSendImageToScale (Image)");
                        return 1;
                    }

                    iRtaFunct = TORREYSendImageToScale(iSimageScale, sIpScale, sPathImage, iSock);

                    if (iRtaFunct > 0)
                    {
                        Console.WriteLine("Error Send Image - TORREYSendImageToScale (s_Image)");
                        return 2;
                    }
                    
                    /*iRtaFunct = TORREYSendImageToScale(iSsimageScale, sIpScale, sPathImage);

                    if (iRtaFunct > 0)
                    {
                        Console.WriteLine("Error Send Image - TORREYSendImageToScale (ss_Image)");
                        return 3;
                    }*/
                   }
                   else if (string.Compare(sTypeData, "Splash") == 0)
                   {

                       iRtaFunct = TORREYSendImageToScale(iScreenSaverHigh, sIpScale, sPathImage, iSock);

                       if (iRtaFunct > 0)
                       {
                           Console.WriteLine("Error Send Image - TORREYSendImageToScale (splash)");
                           return 3;
                       }
                   }else if (string.Compare(sTypeData, "Ad") == 0)
                   {

                       iRtaFunct = TORREYSendImageToScale(iAdHeight, sIpScale, sPathImage, iSock);

                       if (iRtaFunct > 0)
                       {
                           Console.WriteLine("Error Send Image - TORREYSendImageToScale (ad)");
                           return 5;
                       }
                   }else if (string.Compare(sTypeData, "Logo") == 0)
                   {
                    
                    iRtaFunct = TORREYSendImageToScale(iLogoHeight, sIpScale, sPathImage, iSock);

                    if (iRtaFunct > 0)
                    {
                        Console.WriteLine("Error Send Image - TORREYSendImageToScale (logo)");
                        return 6;
                    }
                } else if (string.Compare(sTypeData, "TestUsb") == 0) {

                       iRtaFunct = TORREYSendImageToScale(iTestImageUsb, sIpScale, sPathImage, iSock);

                       if (iRtaFunct > 0)
                       {
                           Console.WriteLine("Error Send Image - TORREYSendImageToScale (TestUsb)");
                           return 4;
                       }
                } else if (string.Compare(sTypeData, "TestSdcard") == 0) {

                       iRtaFunct = TORREYSendImageToScale(iTestImageSdcard, sIpScale, sPathImage, iSock);

                       if (iRtaFunct > 0)
                       {
                           Console.WriteLine("Error Send Image - TORREYSendImageToScale (TestUsb)");
                           return 4;
                       }
               }
            }
            else
            {
                Console.WriteLine("No existe archivo");
                return 4;
            }

            return 0;
        }

        /// <summary>
        /// TORREYSendImageToScale
        /// Envia la imagen hacia la bascula
        /// </summary>
        /// <param name="scaleIP"></param>
        /// <param name="sSourceFile"></param>
        /// <param name="iSizeImage"></param>
        /// <returns>Retorna mayor que cero si hay algun error</returns>
        /// Return  0 - OK
        ///         1 - Error en comando FILESERVER_PutFile
        ///         2 - Error al cambiar tamnio de la imagen
        ///         3 - Error al enviar la imagen
        private int TORREYSendImageToScale(int iDestImage, string sIpScale, string sSourceFile, Socket iSock)
        {
            short sClientPort;
            string sSourcefile = null;
            string sPathDestino;

            int rta_transfer = 0;

            sClientPort = 1232; //Fijo en la Basc.

            #if _DEBUGMODE
            Console.WriteLine("{0}: Enviando comando para transmitir imagen", System.Reflection.MethodBase.GetCurrentMethod().Name);
            #endif

            Console.WriteLine("{0}: Creando objeto para la conexion", System.Reflection.MethodBase.GetCurrentMethod().Name);

            SGSDKCOM myobj = new SGSDKCOM("", sIpScale, sClientPort);  // For listen loop any IP address will be fine

            sSourcefile = resizeImage(iDestImage, sSourceFile, sIpScale);

            string sFileName = Path.GetFileName(sSourceFile);

            if (sSourcefile != null)
            {
                #if _DEBUGMODE
                    Console.WriteLine("{0}: Iniciando transmision de imagen1 hacia la bascula", System.Reflection.MethodBase.GetCurrentMethod().Name);
                #endif 

                switch (iDestImage)
                {
                    case 235:
                        sPathDestino = "/sdcard/images/user/pcimages/";
                        break;

                    case 77:
                        sPathDestino = "/sdcard/s_images/user/pcimages/";
                        break;

                    case 48:
                        sPathDestino = "/sdcard/ss_images/user/pcimages/";
                        break;

                    case 105:
                        sPathDestino = "/usb/";
                        break;

                    case 125:
                        sPathDestino = "/sdcard/";
                        break;
                    case 110:
                        sPathDestino = "/sdcard/logos/";
                        break;
                    case 382:
                        sPathDestino = "/sdcard/adds/";
                        break;
                    default:
                        sPathDestino = "/sdcard/splash/";
                        break;
                }

                rta_transfer = myobj.TransferToLinux(sSourcefile, sPathDestino, SGSDK.SGSDKCOM.xtypes.USING_TCP, iSock,sFileName);
            }
            else
            {
                Console.WriteLine("{0}: Error en el patch o nombre de  la imagen", System.Reflection.MethodBase.GetCurrentMethod().Name);
                return 2;
            }

            if (rta_transfer == 0)
            {
                #if _DEBUGMODE
                Console.WriteLine("{0}: Transferencia de la imagen.. OK\n", System.Reflection.MethodBase.GetCurrentMethod().Name);
                #endif
            }
            else
            {
                Console.WriteLine("{0}: Transferencia de la imagen.. BAD {0}", System.Reflection.MethodBase.GetCurrentMethod().Name, rta_transfer);
                return 3;
            }

            //myobj.StopSDK();
            return 0;
        }

        /// <summary>
        /// TORREYSendImageToIpad
        /// enviar la imagen del PC hacia el IPAD
        /// </summary>
        /// <param name="socketIpad"></param>
        /// <param name="sSourceFile"></param>
        /// <returns>Retorna mayor que cero si hay algun error</returns>
        /// Return  0 - OK
        ///         1 - Error en reasignacion de tamanio de la imagen
        ///         2 - Error en la transmision de la imagen
        ///         3 - Error el archivo no existe
        public int TORREYSendImageToIpad(Socket socketIpad, string sSourceFile)
        {
            int rta_transfer = 0;
            string sPathItemp = null;

            SGSDKCOM myobj = new SGSDKCOM("", "", 0);  // For listen loop any IP address will be fine

            if (System.IO.File.Exists(sSourceFile))
            {
                sPathItemp = resizeImage(iImageIpad, sSourceFile, "Ipad");

                if (sPathItemp != null)
                {
                        #if _DEBUGMODE
                        Console.WriteLine("Iniciando transmision de imagen hacia la Ipad");
                    #endif

                    rta_transfer = myobj.TransferToIpad(socketIpad, sPathItemp);
                }
                else
                {
                    Console.WriteLine("Error en la ruta ({0}) de  la imagen", sPathItemp);
                    return 1;
                }

                if (rta_transfer == 0)
                {
                    #if _DEBUGMODE
                        Console.WriteLine("Transferencia de la imagen.. OK\n");
                    #endif
                }
                else
                {
                    Console.WriteLine("Transferencia de la imagen.. BAD: {0}", rta_transfer);
                    return 2;
                }
            }
            else
            {
                Console.WriteLine("El archivo no existe");
                return 3;
            }

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
}
