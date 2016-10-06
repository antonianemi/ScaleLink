using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.IO.Ports;
using System.Web;
using System.Threading;
using MainMenu.Properties;


namespace MainMenu
{
    public partial class f10Mantenimiento : MdiChildForm
    {
        #region Declaracion Class
        Conexion Cte = new Conexion();
        Envia_Dato Env = new Envia_Dato();
        Serial SR = new Serial();
        #endregion

        public f10Mantenimiento()
        {
            InitializeComponent();           
        }
        #region BotonesTap-Mantenimiento
        private void IniciarControles()
        {
            this.Activate();
            this.panel1.Controls.Clear();
            ToolStripManager.RevertMerge(toolStrip5);
        }

        private void ribCompactar_Click(object sender, EventArgs e)
        {            
            Settings str_db = new Settings();
            string mdwfilename = str_db.dbName;
            string connectionString = str_db.bdConexionString;
           
            Form1.btnEdicion = ESTADO.botonesEdicionEnum.PKCOMPACTAR;
            IniciarControles();

            Variable.clv_aceptada = false;
            clave clv1 = new clave(2);
            clv1.ShowDialog(this);
            if (Variable.clv_aceptada && Variable.privilegio.Substring(20, 1) == "1")
            {
                if (System.IO.File.Exists(Variable.appPath + str_db.dbName))
                {
                    string src = "Provider= Microsoft.ACE.OLEDB.12.0; Data Source=" + Variable.appPath + mdwfilename + ";Jet OLEDB:Engine Type=5";
                    //string dest =  "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + Variable.appPath + "\\tempdb.mdb";
                    string dest =  "Provider= Microsoft.ACE.OLEDB.12.0; Data Source=" + Variable.appPath + "\\tempdb.mdb;Jet OLEDB:Engine Type=5";
                                      
                    JRO.JetEngine jro = new JRO.JetEngine();
                    jro.CompactDatabase(src,dest);

                    File.Delete(Variable.appPath + mdwfilename);
                    File.Move(Variable.appPath + "\\tempdb.mdb", Variable.appPath + mdwfilename);
                }
            }
        }

        private void ribRespaldar_Click(object sender, EventArgs e)
        {
            Settings str_db = new Settings();
            string mdwfilename = str_db.dbName;

            Form1.btnEdicion = ESTADO.botonesEdicionEnum.PKBACKUP;
            IniciarControles();
            Variable.clv_aceptada = false;
            clave clv1 = new clave(2);
            clv1.inicio_user.Text = Variable.user;
            clv1.ShowDialog(this);
            if (Variable.clv_aceptada && Variable.privilegio.Substring(20, 1) == "1")
            {
                mdwfilename = Variable.appPath + @"\" + str_db.dbName;                
                string nombre_backup = System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString();

                if (System.IO.File.Exists(mdwfilename))
                {
                    if (!System.IO.File.Exists(Variable.appPath + "\\" + nombre_backup + ".mdb"))
                    {
                        System.IO.File.Copy(mdwfilename, Variable.appPath + "\\" + nombre_backup + ".mdb", true);
                    }
                    MessageBox.Show(this, Variable.SYS_MSJ[377, Variable.idioma], "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ribUsuario_Click(object sender, EventArgs e)
        {
            Form1.btnEdicion = ESTADO.botonesEdicionEnum.PKUSER;
            IniciarControles();
            UserUsuarios UsUser = new UserUsuarios();
            UsUser.Dock = DockStyle.Fill;
            if (ToolStripManager.FindToolStrip("toolStrip51").Items.Count > 0)
                ToolStripManager.Merge(UsUser.toolStrip51, toolStrip5);
            this.panel1.Controls.Add(UsUser);
        }

        private void ribConfig_Click(object sender, EventArgs e)
        {
            Form1.btnEdicion = ESTADO.botonesEdicionEnum.PKCONFIG;
            IniciarControles();
            UserParametro UsConfig = new UserParametro();
            UsConfig.Dock = DockStyle.Fill;
            if (ToolStripManager.FindToolStrip("toolStrip51").Items.Count > 0)
                ToolStripManager.Merge(UsConfig.toolStrip51, toolStrip5);
            this.panel1.Controls.Add(UsConfig);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void ribPurgeScale_Click(object sender, EventArgs e)
        {
            string Variable_frame;
            List<int> lConexionType = new List<int>();
            List<string> lConexion = new List<string>();
            List<string> lNumSerie = new List<string>();
            string[] Dato_Recibido = null;

            basculaTableAdapter.Fill(baseDeDatosDataSet1.Bascula);

            DataRow[] DR_Basc = baseDeDatosDataSet1.Bascula.Select();

            IniciarControles();

            Variable.clv_aceptada = false;
            clave clv1 = new clave(2);
            clv1.ShowDialog(this);
            if (Variable.clv_aceptada && Variable.privilegio.Substring(20, 1) == "1")
            {
                Cursor.Current = Cursors.WaitCursor;

                foreach (DataRow sOutput in DR_Basc)
                {
                    lConexionType.Add(Convert.ToInt16(sOutput["tipo_conec"].ToString()));
                    if (Convert.ToInt16(sOutput["tipo_conec"].ToString()) == 0)
                    {
                        lConexion.Add(sOutput["dir_ip"].ToString());
                    }
                    else
                    {
                        lConexion.Add(sOutput["puerto"].ToString());
                    }

                    lNumSerie.Add(sOutput["no_serie"].ToString());
                }

                for (int i = 0; i < lConexion.Count; i++)
                {
                    if (lConexionType[i] == 0)
                    {
                        Socket Cliente_bascula = null;
                        Cliente_bascula = Cte.CheckConnectivityForProxyHost(lConexion[i], 50036);
                        if (Cliente_bascula.Connected == true)
                        {
                            WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[423, Variable.idioma] + " " + lNumSerie[i]);
                            Thread t = new Thread(workerObject.vShowMsg);
                            t.Start();

                            Variable_frame = "PCXX" + (char)9 + (char)10;
                            Cte.Envio_Dato(ref Cliente_bascula, lConexion[i], Variable_frame, ref Dato_Recibido);

                            Variable_frame = "GC010" + (char)9 + "/" + (char)9 + "" + (char)9 + "/" + (char)9 + (char)10;
                            Cte.Envio_Dato(ref Cliente_bascula, lConexion[i], Variable_frame, ref Dato_Recibido);

                            Variable_frame = "GA010" + (char)9 + "" + (char)9 + "0" + (char)9 + "C" + (char)9 + (char)10;
                            Cte.Envio_Dato(ref Cliente_bascula, lConexion[i], Variable_frame, ref Dato_Recibido);

                            Env.Command_Limpiar(lConexion[i], ref Cliente_bascula, "GAF0");

                            Variable_frame = "PAXX" + (char)9 + (char)10;
                            Cte.Envio_Dato(ref Cliente_bascula, lConexion[i], Variable_frame, ref Dato_Recibido);

                            Variable_frame = "PVXX" + (char)9 + (char)10;
                            Cte.Envio_Dato(ref Cliente_bascula, lConexion[i], Variable_frame, ref Dato_Recibido);

                            Variable_frame = "PIXX" + (char)9 + (char)10;
                            Cte.Envio_Dato(ref Cliente_bascula, lConexion[i], Variable_frame, ref Dato_Recibido);
                            Variable_frame = "PMXX" + (char)9 + (char)10;
                            Cte.Envio_Dato(ref Cliente_bascula, lConexion[i], Variable_frame, ref Dato_Recibido);
                            Variable_frame = "POXX" + (char)9 + (char)10;
                            Cte.Envio_Dato(ref Cliente_bascula, lConexion[i], Variable_frame, ref Dato_Recibido);
                            Variable_frame = "PPXX" + (char)9 + (char)10;
                            Cte.Envio_Dato(ref Cliente_bascula, lConexion[i], Variable_frame, ref Dato_Recibido);
                            Variable_frame = "XABXX" + (char)9 + (char)10;
                            Cte.Envio_Dato(ref Cliente_bascula, lConexion[i], Variable_frame, ref Dato_Recibido);
                            Cte.desconectar(ref Cliente_bascula);

                            workerObject.vEndShowMsg();
                        }
                        else
                        {
                            if (MessageBox.Show(this, Variable.SYS_MSJ[34, Variable.idioma] + " " + lNumSerie[i] + " " + Variable.SYS_MSJ[190, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        SerialPort serialPort1 = new SerialPort();

                        if (SR.OpenPort(ref serialPort1, lConexion[i], 115200))
                        {
                            WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[423, Variable.idioma] + " " + lNumSerie[i]);
                            Thread t = new Thread(workerObject.vShowMsg);
                            t.Start();

                            SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)9 + (char)10);
                            SR.ReceivedCOMSerial(ref serialPort1);

                            string Msg_Recibido = "";

                            Variable_frame = "PCXX" + (char)9 + (char)10;
                            SR.SendCOMSerial(ref serialPort1, Variable_frame);
                            Msg_Recibido = SR.ReceivedCOMSerial(ref serialPort1);

                            Variable_frame = "GC010" + (char)9 + "/" + (char)9 + "" + (char)9 + "/" + (char)9 + (char)10;
                            SR.SendCOMSerial(ref serialPort1, Variable_frame);
                            Msg_Recibido = SR.ReceivedCOMSerial(ref serialPort1);

                            Variable_frame = "GA010" + (char)9 + "" + (char)9 + "0" + (char)9 + "C" + (char)9 + (char)10;
                            SR.SendCOMSerial(ref serialPort1, Variable_frame);
                            Msg_Recibido = SR.ReceivedCOMSerial(ref serialPort1);

                            Variable_frame = "GAF0" + (char)9 + (char)10;
                            SR.SendCOMSerial(ref serialPort1, Variable_frame);
                            Msg_Recibido = SR.ReceivedCOMSerial(ref serialPort1);

                            Variable_frame = "PAXX" + (char)9 + (char)10;
                            SR.SendCOMSerial(ref serialPort1, Variable_frame);
                            Msg_Recibido = SR.ReceivedCOMSerial(ref serialPort1);

                            Variable_frame = "PVXX" + (char)9 + (char)10;
                            SR.SendCOMSerial(ref serialPort1, Variable_frame);
                            Msg_Recibido = SR.ReceivedCOMSerial(ref serialPort1);


                            Variable_frame = "PIXX" + (char)9 + (char)10;
                            SR.SendCOMSerial(ref serialPort1, Variable_frame);
                            Msg_Recibido = SR.ReceivedCOMSerial(ref serialPort1);

                            Variable_frame = "PMXX" + (char)9 + (char)10;
                            SR.SendCOMSerial(ref serialPort1, Variable_frame);
                            Msg_Recibido = SR.ReceivedCOMSerial(ref serialPort1);

                            Variable_frame = "POXX" + (char)9 + (char)10;
                            SR.SendCOMSerial(ref serialPort1, Variable_frame);
                            Msg_Recibido = SR.ReceivedCOMSerial(ref serialPort1);

                            Variable_frame = "PPXX" + (char)9 + (char)10;
                            SR.SendCOMSerial(ref serialPort1, Variable_frame);
                            Msg_Recibido = SR.ReceivedCOMSerial(ref serialPort1);

                            Variable_frame = "XABXX" + (char)9 + (char)10;
                            SR.SendCOMSerial(ref serialPort1, Variable_frame);
                            Msg_Recibido = SR.ReceivedCOMSerial(ref serialPort1);

                            SR.SendCOMSerial(ref serialPort1, "DXXX" + (char)9 + (char)10);
                            SR.ReceivedCOMSerial(ref serialPort1);

                            SR.ClosePort(ref serialPort1);

                            workerObject.vEndShowMsg();
                        }
                    }
                }
            }

            Cursor.Current = Cursors.Default;
        }

        #endregion
        private void f10Mantenimiento_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet1.Bascula' Puede moverla o quitarla según sea necesario.
            this.basculaTableAdapter.Fill(this.baseDeDatosDataSet1.Bascula);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet1.Usuarios' Puede moverla o quitarla según sea necesario.
            this.usuariosTableAdapter.Fill(this.baseDeDatosDataSet1.Usuarios);

           
        }

        private void f10Mantenimiento_Activated(object sender, EventArgs e)
        {
            for (int i = 0; i < Variable.privilegio.Length; i++)
            {
                if (Variable.privilegio.Substring(i, 1) == "1")
                {
                    if (i == 1) { this.ribUsuario.Enabled = true; } //administracion de usuario
                    if (i == 3) { this.ribPurgeScale.Enabled = true; } // Administrador de Basculas
                    if (i == 17) { this.ribConfig.Enabled = true; }  //configuracion del sistema   
                    if (i == 20)
                    {
                        this.ribCompactar.Enabled = true;
                        this.ribRespaldar.Enabled = true;
                    } //proceso de respaldo y compactar 
                }
                else
                {
                    if (i == 1) { this.ribUsuario.Enabled = false; } //administracion de usuario
                    if (i == 3) { this.ribPurgeScale.Enabled = false; } // Administrador de Basculas
                    if (i == 17) { this.ribConfig.Enabled = false; }  //configuracion del sistema   
                    if (i == 20)
                    {
                        this.ribCompactar.Enabled = false;
                        this.ribRespaldar.Enabled = false;
                    } //proceso de respaldo y compactar            
                }
            } 
        }

    }
}
