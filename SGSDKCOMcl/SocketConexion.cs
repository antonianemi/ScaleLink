using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SocketConexion
{
    public class SocketOperation
    {

        public static ManualResetEvent acceptDone = new ManualResetEvent(false);
        public static ManualResetEvent connectDone = new ManualResetEvent(false);
        public static ManualResetEvent sendDone = new ManualResetEvent(false);
        public static ManualResetEvent reciveDone = new ManualResetEvent(false);

        private static bool IsConnectServer = false;
        public static Socket SocketConnectServer = null;
        private static Exception socketexception;

        private static bool IsAcceptClient = false;
        public static Socket SocketConnectClient = null;


        // State object for receiving data from remote device.
        public class StateObject
        {
            // Client socket.
            public Socket workSocket = null;
            // Size of receive buffer.
            public const int BufferSize = 256;
            // Receive buffer.
            public byte[] buffer = new byte[BufferSize];
            // Received data string.
            public StringBuilder sb = new StringBuilder();
        }

        #region iConnectServer
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SocketHandler"></param>
        /// <param name="IpServer"></param>
        /// <returns></returns>
        public Socket iConnectServer(Socket SocketHandler, IPEndPoint IpServer)
        {
            connectDone.Reset();
            socketexception = null;
            IsConnectServer = false;

            DateTime DateTimeObject = DateTime.Now;

            Console.WriteLine(" ");
            Console.WriteLine("{0}: Time: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, DateTimeObject.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            try
            {
                SocketHandler.BeginConnect(IpServer, new AsyncCallback(ConnectCallback), SocketHandler); //Llamado a la funcion asyncrona de conexion

                if (connectDone.WaitOne(5000, false))
                {
                    if (IsConnectServer == true)
                    {
                        connectDone.Reset();
                        Console.WriteLine("{0}: Conectado", System.Reflection.MethodBase.GetCurrentMethod().Name);
                        return SocketConnectServer;
                    }
                    else
                    {
                        SocketHandler.Close();
                        Console.WriteLine("{0}: Error Socket", System.Reflection.MethodBase.GetCurrentMethod().Name);
                        throw socketexception;
                    }

                }
                else
                {
                    SocketHandler.Close();
                    Console.WriteLine("{0}: TimeOut Exception connection", System.Reflection.MethodBase.GetCurrentMethod().Name);
                    throw new TimeoutException("TimeOut Exception");
                }

            }
            catch (SocketException ex)
            {
                Console.WriteLine("{0}: Error: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: Error: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        private static void ConnectCallback(IAsyncResult ar)
        {
            Console.WriteLine("{0}: In ConnectCallBack", System.Reflection.MethodBase.GetCurrentMethod().Name);
            DateTime DateTimeObject = DateTime.Now;
            Console.WriteLine("{0}: Time: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, DateTimeObject.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            try
            {
                Socket client = (Socket)ar.AsyncState;

                if (client.Connected == true)
                {
                    client.EndConnect(ar);
                    IsConnectServer = true;
                    SocketConnectServer = client;
                    IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;
                    Console.WriteLine("{0}: Connect Ip: {1}, Port: {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, clientep.Address, clientep.Port);
                    connectDone.Set();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("{0}: Error conexion: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message);
            }
            catch (ObjectDisposedException e)
            {
                Console.WriteLine("{0}: Error conexion: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}: Error conexion: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("{0}: Error conexion: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}: Error conexion: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, e.Message);
            }
            finally
            {
                connectDone.Set();
            }
        }
        #endregion 

        #region iAcceptClient
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SocketHandler"></param>
        /// <returns></returns>
        public Socket iAcceptClient(Socket SocketHandler)
        {
            acceptDone.Reset();
            socketexception = null;
            IsAcceptClient = false;

            DateTime DateTimeObject = DateTime.Now;

            Console.WriteLine(" ");
            Console.WriteLine("{0}: Time: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, DateTimeObject.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            try
            {
                SocketHandler.BeginAccept(new AsyncCallback(AcceptCallback), SocketHandler); 

                if (acceptDone.WaitOne(5000, false))
                {
                    if (IsAcceptClient == true)
                    {
                        acceptDone.Reset();
                        Console.WriteLine("{0}: Conectado", System.Reflection.MethodBase.GetCurrentMethod().Name);
                        return SocketConnectClient;
                    }
                    else
                    {
                        SocketHandler.Close();
                        Console.WriteLine("{0}: Error Socket", System.Reflection.MethodBase.GetCurrentMethod().Name);
                        throw socketexception;
                    }

                }
                else
                {
                    SocketHandler.Close();
                    Console.WriteLine("{0}: TimeOut Exception connection", System.Reflection.MethodBase.GetCurrentMethod().Name);
                    throw new TimeoutException("TimeOut Exception");
                }

            }
            catch (SocketException ex)
            {
                Console.WriteLine("{0}: Error: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: Error: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        private static void AcceptCallback(IAsyncResult ar)
        {

            Console.WriteLine("{0}: In ConnectCallBack", System.Reflection.MethodBase.GetCurrentMethod().Name);
            DateTime DateTimeObject = DateTime.Now;
            Console.WriteLine("{0}: Time: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, DateTimeObject.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            try
            {
                Socket client = (Socket)ar.AsyncState;

                if (client.Connected == true)
                {
                    client.EndConnect(ar);
                    IsAcceptClient = true;
                    SocketConnectClient = client;
                    IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;
                    Console.WriteLine("{0}: Connect Ip: {1}, Port: {2}", System.Reflection.MethodBase.GetCurrentMethod().Name, clientep.Address, clientep.Port);
                    acceptDone.Set();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("Error conexion con servidor: {0}", e.Message);
            }
            catch (ObjectDisposedException e)
            {
                Console.WriteLine("Error conexion con servidor: {0}", e.Message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Error conexion con servidor: {0}", e.Message);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("Error conexion con servidor: {0}", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error conexion con servidor: {0}", e.Message);
            }
            finally
            {
                connectDone.Set();
            }
        }
        #endregion
    }
}
