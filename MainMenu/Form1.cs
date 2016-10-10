using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TorreyTransfer;

namespace MainMenu
{
    public partial class Form1 : Form
    {

        Variable.TipoBascula TipoBasculaAnterior;

        #region Variablees
        public static ESTADO.botonesVistasEnum vistaActiva = new ESTADO.botonesVistasEnum();
        public static ESTADO.EstadoRegistro statRegistro = new ESTADO.EstadoRegistro();
        public static ESTADO.botonesEdicionEnum btnEdicion = new ESTADO.botonesEdicionEnum();
        public static ToolStripStatusLabel toolLabel = new ToolStripStatusLabel();
        public static ToolStripProgressBar toolStripProgressBar1 = new ToolStripProgressBar();
        public static bool User_Exit = false;
        ADOutil Conec = new ADOutil();
        Variable Glob = new Variable();
        public int iNoConnect = 0;
        #endregion

        #region Inicializacion

        public Form1()
        {
            InitializeComponent();
            Glob.Cargar_Mensajes();
            Variable.Cargar_puertos();
            toolLabel.AutoSize = false;
            toolLabel.Width = 600;
            toolLabel.TextAlign = ContentAlignment.MiddleCenter;
            statusStrip1.Items.Add(toolLabel);                        
            this.Inter_Timer.Tick += new System.EventHandler(this.Inter_Timer_Tick);
            LoadScalesFromDB();
        }

      
        #endregion

        #region BotononesMenuOrb y Tap
      
        private void rioSalir_Click(object sender, EventArgs e)
        {
            if (MsjOkCancel(Variable.SYS_MSJ[189, Variable.idioma]) == DialogResult.OK)
            {              
                this.Dispose();
                Application.Exit();
            }
        }

        private void ritInicio_Click(object sender, EventArgs e)
        {
            CambiarCheck();            
             int nMDI = Find_FORM_LOAD("f1Basculas");
            f1Basculas f1;
            if (nMDI < 0)
            {
                f1 = new f1Basculas(this);
                f1.MdiParent = this;
                f1.Show();
            }
            else
            {
                f1 = (f1Basculas)this.MdiChildren[nMDI];
                f1.Activate();
            }
            ritInicio.BackColor = Color.FromArgb(246, 205, 217, 254);  // .BackgroundImage = global::MainMenu.Properties.Resources.fondo2;
        }

        private void ritCatalogo_Click(object sender, EventArgs e)
        {
            CambiarCheck();
           
            int nMDI = Find_FORM_LOAD("f2Catalogos");
            f2Catalogos f2;
            if (nMDI < 0)
            {
                f2 = new f2Catalogos();
                f2.MdiParent = this;
                f2.Show();
            }
            else
            {
                f2 = (f2Catalogos)this.MdiChildren[nMDI];
                f2.Activate();
            }
            ritCatalogo.BackColor = Color.FromArgb(246, 205, 217, 254);  //BackgroundImage = global::MainMenu.Properties.Resources.fondo2; 
        }

        private void ritCarpetas_Click(object sender, EventArgs e)
        {
            CambiarCheck();
           
            int nMDI = Find_FORM_LOAD("f3Sincronizar");
            f3Sincronizar f3;
            if (nMDI < 0)
            {
                f3 = new f3Sincronizar();
                f3.MdiParent = this;
                f3.Show();
            }
            else
            {
                f3 = (f3Sincronizar)this.MdiChildren[nMDI];
                f3.Activate();
            }
            ritCarpetas.BackColor = Color.FromArgb(246, 205, 217, 254); //BackgroundImage = global::MainMenu.Properties.Resources.fondo2;
        }

        private void ritPrecio_Click(object sender, EventArgs e)
        {
            CambiarCheck();
            
            int nMDI = Find_FORM_LOAD("f4Precio");
            f4Precio f4;
            if (nMDI < 0)
            {
                f4 = new f4Precio();
                f4.MdiParent = this;
                f4.Show();
            }
            else
            {
                f4 = (f4Precio)this.MdiChildren[nMDI];
                f4.Llenar_Grid();
                f4.Activate();
            }
            ritPrecio.BackColor = Color.FromArgb(246, 205, 217, 254); //BackgroundImage = global::MainMenu.Properties.Resources.fondo2;
        }

        private void ritCorte_Click(object sender, EventArgs e)
        {
            CambiarCheck();

            int nMDI = Find_FORM_LOAD("f5Cortes");
            f5Cortes f5;
            if (nMDI < 0)
            {
                f5 = new f5Cortes();
                f5.MdiParent = this;
                f5.Show();
            }
            else
            {
                f5 = (f5Cortes)this.MdiChildren[nMDI];
                f5.Activate();
            }
            ritCorte.BackColor = Color.FromArgb(246, 205, 217, 254); //BackgroundImage = global::MainMenu.Properties.Resources.fondo2;

        }  
     
        private void ritSincronizacion_Click(object sender, EventArgs e)
        {
            CambiarCheck();

            IpadInfo Infoipad = new IpadInfo();            
            Infoipad.ShowDialog(this);

            /*int nMDI = Find_FORM_LOAD("f6Sincronizacion");
            f6Sincronizacion f6;
            if (nMDI < 0)
            {
                f6 = new f6Sincronizacion();
                f6.MdiParent = this;
                f6.Show();
            }
            else
            {
                f6 = (f6Sincronizacion)this.MdiChildren[nMDI];
            }
            f6.Activate();
            ritCorte.BackColor = Color.FromArgb(246, 205, 217, 254);   //BackgroundImage = global::MainMenu.Properties.Resources.fondo2; */
        }

        private void ritEstadistica_Click(object sender, EventArgs e)
        {
            CambiarCheck();
            
            int nMDI = Find_FORM_LOAD("f7Estadisticas");
            f7Estadisticas f7;
            if (nMDI < 0)
            {
                f7 = new f7Estadisticas();
                f7.MdiParent = this;
                f7.Show();
            }
            else
            {
                f7 = (f7Estadisticas)this.MdiChildren[nMDI];
                f7.Activate();
            }
            ritEstadistica.BackColor = Color.FromArgb(246, 205, 217, 254);   //BackgroundImage = global::MainMenu.Properties.Resources.fondo2;
        }

        private void ritMentenimiento_Click(object sender, EventArgs e)
        {
            CambiarCheck();
            
            int nMDI = Find_FORM_LOAD("f10Mantenimiento");
            f10Mantenimiento f10;
            if (nMDI < 0)
            {
                f10 = new f10Mantenimiento();
                f10.MdiParent = this;
                f10.Show();
            }
            else
            {
                f10 = (f10Mantenimiento)this.MdiChildren[nMDI];
                f10.Activate();
            }
            ritMentenimiento.BackColor = Color.FromArgb(246, 205, 217, 254);  //BackgroundImage = global::MainMenu.Properties.Resources.fondo2;
        }

        private void ritAyuda_Click(object sender, EventArgs e)
        {
            //CambiarCheck();
            
            int nMDI = Find_FORM_LOAD("f9Acerca");
            f9Acerca f9 = new f9Acerca();
            f9.ShowDialog(this);

            //ritAyuda.BackColor = Color.FromArgb(246, 205, 217, 254);  //BackgroundImage = global::MainMenu.Properties.Resources.fondo2;
            
        }           

        private void CambiarCheck()
        {            
            ritInicio.BackColor = Color.CornflowerBlue;
            ritCatalogo.BackColor = Color.CornflowerBlue;
            ritCarpetas.BackColor = Color.CornflowerBlue;
            ritPrecio.BackColor = Color.CornflowerBlue;
            ritCorte.BackColor = Color.CornflowerBlue;
            ritEstadistica.BackColor = Color.CornflowerBlue;
            ritMentenimiento.BackColor = Color.CornflowerBlue;
            ritAyuda.BackColor = Color.CornflowerBlue;
        }
        #endregion
                              
        #region Mensajes

        public DialogResult MsjGuardarAntes(string poMsj)
        {
            DialogResult respuesta = DialogResult.None;
            if (statRegistro != ESTADO.EstadoRegistro.PKTRATADO)
            {
                respuesta = MessageBox.Show(this, poMsj + vistaActiva + (char)13, Variable.SYS_MSJ[42, Variable.idioma],
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                // if (respuesta == DialogResult.Yes)
                // {
                //   edicion(btnEdicion);
                // }
            }
            return respuesta;
        }

        public DialogResult MsjOkCancel(string poMsj)
        {
            DialogResult result = MessageBox.Show(this, poMsj + (char)13, Variable.SYS_MSJ[42, Variable.idioma],
                MessageBoxButtons.OKCancel, MessageBoxIcon.Hand);

            if (result == DialogResult.OK)
            {
            }
            else if (result == DialogResult.Cancel)
            {
            }
            return result;
        }

        #endregion

        #region Funciones Especiales
        private int Find_FORM_LOAD(string formulario)
        {
            Application.EnableVisualStyles();

            for (int j = 0; j < this.MdiChildren.Length; j++)
            {
                if (this.MdiChildren[j].Name == formulario)
                {
                    return j;
                }
            }
            return -1;
        }
        #endregion

        #region Procesos de Timer

        private void Inter_Timer_Tick(object sender, EventArgs e)
        {


            if (Pendientes.StatusPendientes() == 1)
            {
                string fecha = string.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);
                string hora1 = string.Format(Variable.F_Hora, DateTime.Now);
                if (DateTime.Parse(hora1) >= DateTime.Parse(Variable.Hora_Inicial) && DateTime.Parse(hora1) <= DateTime.Parse(Variable.Hora_final))
                {
                    if (this.Inter_Timer.Enabled)// si el timer esta Habilitado 
                    {
                        if (DateTime.Parse(hora1) >= DateTime.Parse(Variable.Hora_Intervalo))//Valida que la hora si este avanzando?
                        {
                            Envia_Dato E = new Envia_Dato();
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("Timer: Enter");
                            Console.ForegroundColor = ConsoleColor.White;
                            E.actualizar_basculas();//Se lleva acabo la actualizacion de la bascula....
                            Variable.Hora_Intervalo = DateTime.Now.AddMilliseconds(Variable.list_tiempo[Variable.pos_intervalo]).ToShortTimeString();
                            Form1.toolLabel.Text = Variable.SYS_MSJ[261, Variable.idioma] + " " + Variable.Hora_Intervalo;
                        }
                    }
                    else
                    {
                        //Se reinicia el Timer.
                        this.Inter_Timer.Interval = Variable.list_tiempo[Variable.pos_intervalo];
                        this.Inter_Timer.Enabled = true;
                        this.Inter_Timer.Start();
                    }
                }
                else
                {
                    //Se detiene el Timer.
                    //  esto sucedera cuando las fechas no esten correctas.
                    this.Inter_Timer.Tick -= new System.EventHandler(this.Inter_Timer_Tick);
                    this.Inter_Timer.Stop();
                    this.Inter_Timer.Enabled = false;
                }
            }


        }
                       
        #endregion

        bool IsFirst()
        {
            if (Variable.TypeScale == Variable.TipoBascula.WLABEL){return false;}
            else if (Variable.TypeScale == Variable.TipoBascula.WLS){return false;}
            else if (Variable.TypeScale == Variable.TipoBascula.WLSD){return false;}
            else{return true;}
        }

        bool isThereDB()
        {
            bool bandera = false;
            //BUSCAR LOS TIPOS DE BASCULA QUE TIENE EL SISTEMA********************************************

            foreach (Scale item in Variable.scales)
            {
                if (item.tipo == Variable.TipoBascula.WLABEL)
                {
                    bandera = true;
                    Variable.TypeScale = Variable.TipoBascula.WLABEL;
                }
            }

            foreach (Scale item in Variable.scales)
            {
                if (item.tipo == Variable.TipoBascula.WLS)
                {
                    bandera = true;
                    Variable.TypeScale = Variable.TipoBascula.WLS;
                }
            }

            foreach (Scale item in Variable.scales)
            {
                if (item.tipo == Variable.TipoBascula.WLSD)
                {
                    bandera = true;
                    Variable.TypeScale = Variable.TipoBascula.WLSD;
                }
            }

           
           
            ///********************************************************************************************
            return bandera;
        }

        Variable.TipoBascula getScale(string type)
        {
            Variable.TipoBascula tipo = Variable.TipoBascula.WLABEL;

            if ((type.ToUpper().Equals("WLS")))
            {
                tipo = Variable.TipoBascula.WLS;
            }
            if ((type.ToUpper().Equals("WLSD")))
            {
                tipo = Variable.TipoBascula.WLSD;
            }
            if ((type.ToUpper().Equals("W-LABEL")))
            {
                tipo = Variable.TipoBascula.WLABEL;
            }

            return tipo;
        }

        private void LoadScalesFromDB()
        {
            Variable.scales.Clear();
            Conec.CadenaSelect = "SELECT * FROM Bascula";
            BaseDeDatosDataSetTableAdapters.BasculaTableAdapter basculaTableAdapter = new BaseDeDatosDataSetTableAdapters.BasculaTableAdapter();
            BaseDeDatosDataSet baseDeDatosDataSet1 = new BaseDeDatosDataSet();
            basculaTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            basculaTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            basculaTableAdapter.Fill(baseDeDatosDataSet1.Bascula);

            foreach (DataRow dr in baseDeDatosDataSet1.Bascula.Rows)
            {
                Scale s = new Scale();
                s.tipo = getScale(dr["modelo"].ToString().ToUpper());
                Variable.scales.Add(s);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!isThereDB())
            {
                ChooseMode form = new ChooseMode(this);
                form.ShowDialog(this);
            }
            Acceso INIC = new Acceso();
            INIC.ShowDialog(this);

            if (!User_Exit)
            {                
                for (int i = 0; i < Variable.privilegio.Length; i++)
                {
                    if (Variable.privilegio.Substring(i, 1) == "1")
                    {
                        if (i == 15) { this.ritPrecio.Enabled = true; } //cambio de precio
                        if (i == 16) { this.ritCorte.Enabled = true; }  //registro de corte de caja                        
                    }
                    else
                    {
                        if (i == 15) { this.ritPrecio.Enabled = false; } //cambio de precio
                        if (i == 16) { this.ritCorte.Enabled = false; }  //registro de corte de caja   
                    }
                }
                
                RefrescaName();

                int nMDI = Find_FORM_LOAD("f6Sincronizacion");
                f6Sincronizacion f6;
                if (nMDI >= 0) f6 = (f6Sincronizacion)this.MdiChildren[nMDI];               
                else f6 = new f6Sincronizacion();        
                Iniciar_Timer_EnvioAutomatico();
            }
            else
            {
                this.Close();
                this.Dispose();                
            }            
        } 

        public void AsignarImagenIcono()
        {
            if (Variable.TypeScale == Variable.TipoBascula.WLABEL){toolStripMenuItem1.Image = MainMenu.Properties.Resources.ico_wlabel2;}
            else if (Variable.TypeScale == Variable.TipoBascula.WLS) { toolStripMenuItem1.Image = MainMenu.Properties.Resources.ico_wlsd2; }
            else if (Variable.TypeScale == Variable.TipoBascula.WLSD) { toolStripMenuItem1.Image = MainMenu.Properties.Resources.BO_1_bc; }
        }


        public void RefrescaName()
        {
            this.Text = String.Empty;
            this.Text = getName() + Variable.Empresa + " [" + Variable.user + "]";
            AsignarImagenIcono();
        }

        string getName()
        {
            string data = "";
            if (Variable.TypeScale== Variable.TipoBascula.WLSD)  {data = "Admin WLSD";   }
            if (Variable.TypeScale == Variable.TipoBascula.WLS) { data = "Admin WLS"; }
            if (Variable.TypeScale == Variable.TipoBascula.WLABEL) { data = "Admin W-LABEL"; }
            return data;
        }
        
        public void ConfigureLikeWLabel()
        {
            Variable.TypeScale = Variable.TipoBascula.WLABEL;
            //Configuracion para wlabel
        }

        public void ConfigureLikeWLS()
        {
            Variable.TypeScale = Variable.TipoBascula.WLS;
            //Configuracion para wls
        }

        public void ConfigureLikeWLSD()
        {
            Variable.TypeScale = Variable.TipoBascula.WLSD;
            //configuracion para wlsd
            Variable.Habilitar_PubliWLSD = 1;
        }

        public void Iniciar_Timer_EnvioAutomatico()
        {
            string ipLocal = new GetIP().IPStr;
            string fecha = string.Format(Variable.F_Fecha, DateTime.Now.ToShortDateString());
            string hora1 = string.Format(Variable.F_Hora, DateTime.Now.ToShortTimeString());
            bool bandera = false;
            OleDbDataReader olr = Conec.Obtiene_Dato("Select IPLocal, dias, Intervalo, pos_inter, H_inicial,H_final,ActivarFrecuencia from DatosGral Where IPLocal = '" + ipLocal + "'", Conec.CadenaConexion);

            if (olr.Read())
            {
                Variable.dias_semana = olr.GetString(1);
                Variable.pos_intervalo = olr.GetInt16(3);
                Variable.Hora_Inicial = olr.GetString(4);
                Variable.Hora_final = olr.GetString(5);
                Variable.Activar_Frecuencia = Convert.ToSByte(olr.GetValue(6));
                bandera = true;
            }
            olr.Close();


            if (bandera)
            {
                if (Variable.Activar_Frecuencia != 0 && DateTime.Parse(hora1) >= DateTime.Parse(Variable.Hora_Inicial) && DateTime.Parse(hora1) <= DateTime.Parse(Variable.Hora_final))
                {
                    Variable.Hora_Intervalo = DateTime.Now.AddMilliseconds(Variable.list_tiempo[Variable.pos_intervalo]).ToShortTimeString();

                    Form1.toolLabel.Text = Variable.SYS_MSJ[261, Variable.idioma] + " " + Variable.Hora_Intervalo;

                    this.Inter_Timer.Interval = Variable.list_tiempo[Variable.pos_intervalo];
                    if (!this.Inter_Timer.Enabled)
                    {
                        this.Inter_Timer.Tick += new System.EventHandler(this.Inter_Timer_Tick);
                        this.Inter_Timer.Enabled = true;
                        this.Inter_Timer.Start();
                    }
                }
                else if (Variable.Activar_Frecuencia != 0 && !(DateTime.Parse(hora1) >= DateTime.Parse(Variable.Hora_Inicial) && DateTime.Parse(hora1) <= DateTime.Parse(Variable.Hora_final)))
                {
                    this.Inter_Timer.Stop();
                    Form1.toolLabel.Text = Variable.SYS_MSJ[439, Variable.idioma];  // "Envio automatico fuera de horario";
                }
                else
                {
                    this.Inter_Timer.Stop();
                    Form1.toolLabel.Text = Variable.SYS_MSJ[262, Variable.idioma];  // "Envio Automatico desactivado";
                }
            }

        }

        private void Form1_MdiChildActivate(object sender, EventArgs e)
        {
            string nombr = "";
            CambiarCheck();
            Iniciar_Timer_EnvioAutomatico();
            Form form_activo = (Form)sender;         

            if (form_activo.ActiveMdiChild != null)
            {
                nombr = form_activo.ActiveMdiChild.Name;

                switch (nombr)
                {
                    case "f2Catalogos":
                        ritCatalogo.BackColor = Color.FromArgb(246, 205, 217, 254);
                        break;
                    case "f1Basculas":
                        ritInicio.BackColor = Color.FromArgb(246, 205, 217, 254);
                        break;
                    case "f3Sincronizar":
                        ritCarpetas.BackColor = Color.FromArgb(246, 205, 217, 254);
                        break;
                    case "f4Precio":
                        ritPrecio.BackColor = Color.FromArgb(246, 205, 217, 254);
                        break;
                    case "f5Cortes":
                        ritCorte.BackColor = Color.FromArgb(246, 205, 217, 254);
                        break;
                    case "f10Mantenimiento":
                        ritMentenimiento.BackColor = Color.FromArgb(246, 205, 217, 254);
                        break;
                    case "f7Estadisticas":
                        ritEstadistica.BackColor = Color.FromArgb(246, 205, 217, 254);
                        break;
                    case "f9Ayuda":
                        ritAyuda.BackColor = Color.FromArgb(246, 205, 217, 254);
                        break;
                }
            }          
        }

        private void CambiopasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
           cambiopsw pass = new cambiopsw();
           pass.ShowDialog(this);
        }

        private void CambioUsuarioToolStrip_Click(object sender, EventArgs e)
        {
            Acceso INIC = new Acceso();
            INIC.ShowDialog(this);
            if (!User_Exit)
            //if (INIC.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                for (int i = 0; i < Variable.privilegio.Length; i++)
                {
                    if (Variable.privilegio.Substring(i, 1) == "1")
                    {
                        if (i == 15) { this.ritPrecio.Enabled = true; } //cambio de precio
                        if (i == 16) { this.ritCorte.Enabled = true; }  //registro de corte de caja                        
                    }
                    else
                    {
                        if (i == 15) { this.ritPrecio.Enabled = false; } //cambio de precio
                        if (i == 16) { this.ritCorte.Enabled = false; }  //registro de corte de caja   
                    }
                }
                RefrescaName();
                //this.Text = Variable.Empresa + " [" + Variable.user + "]";               
                for (int j = 0; j < this.MdiChildren.Length; j++)
                {  
                    this.MdiChildren[j].Close();
                }  
            }
            else
            {
                this.Close();
                this.Dispose();
            }         
        }

        private void wizardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TipoBasculaAnterior = Variable.TypeScale;
            clearEventos();
            ChooseMode ch = new ChooseMode(this);
            ch.ShowDialog();

            if (TipoBasculaAnterior != Variable.TypeScale)//significa que cambio
            {
                if (MdiChildren.Count() > 0)
                { 
                  for (int i = 0; i < MdiChildren.Count(); i++)
                    {
                        MdiChildren[i].Close();
                    }
                }
                TipoBasculaAnterior = Variable.TypeScale;
            }


        }

        void clearEventos()
        {
            //limpiar las variables
            Variable.wlabel = false;
            Variable.wls = false;
            Variable.wlsd = false;
        }

    }

    public class Envia_Dato
    {
        ADOutil Conec = new ADOutil();
        Conexion Cte = new Conexion();
        Serial SR = new Serial();
        BackgroundWorker worker = new BackgroundWorker();
        //proceso pro = new proceso();
        public FileStream fi3;
        public StreamWriter sr3;

        BaseDeDatosDataSet baseDeDatosDataSet = new BaseDeDatosDataSet();
        BaseDeDatosDataSetTableAdapters.BasculaTableAdapter basculasTableAdapter = new BaseDeDatosDataSetTableAdapters.BasculaTableAdapter();
        BaseDeDatosDataSetTableAdapters.ProductosTableAdapter productosTableAdapter = new BaseDeDatosDataSetTableAdapters.ProductosTableAdapter();
        BaseDeDatosDataSetTableAdapters.Prod_detalleTableAdapter prod_detalleTableAdapter = new BaseDeDatosDataSetTableAdapters.Prod_detalleTableAdapter();
        BaseDeDatosDataSetTableAdapters.PublicidadTableAdapter publicidadTableAdapter = new BaseDeDatosDataSetTableAdapters.PublicidadTableAdapter();
        BaseDeDatosDataSetTableAdapters.Public_DetalleTableAdapter public_DetalleTableAdapter = new BaseDeDatosDataSetTableAdapters.Public_DetalleTableAdapter();
        BaseDeDatosDataSetTableAdapters.OfertaTableAdapter ofertaTableAdapter = new BaseDeDatosDataSetTableAdapters.OfertaTableAdapter();
        BaseDeDatosDataSetTableAdapters.Oferta_DetalleTableAdapter oferta_DetalleTableAdapter = new BaseDeDatosDataSetTableAdapters.Oferta_DetalleTableAdapter();
        BaseDeDatosDataSetTableAdapters.VendedorTableAdapter vendedorTableAdapter = new BaseDeDatosDataSetTableAdapters.VendedorTableAdapter();
        BaseDeDatosDataSetTableAdapters.IngredientesTableAdapter ingredientesTableAdapter = new BaseDeDatosDataSetTableAdapters.IngredientesTableAdapter();
        BaseDeDatosDataSetTableAdapters.Ingre_detalleTableAdapter ingredetalleTableAdapter = new BaseDeDatosDataSetTableAdapters.Ingre_detalleTableAdapter();
        BaseDeDatosDataSetTableAdapters.carpeta_detalleTableAdapter carpetadetalleTableAdapter = new BaseDeDatosDataSetTableAdapters.carpeta_detalleTableAdapter();
        BaseDeDatosDataSetTableAdapters.BufferSalidaTableAdapter BufferSalidaTableAdapter = new BaseDeDatosDataSetTableAdapters.BufferSalidaTableAdapter();
        BaseDeDatosDataSetTableAdapters.VentasTableAdapter ventasTableAdapter = new BaseDeDatosDataSetTableAdapters.VentasTableAdapter();
        BaseDeDatosDataSetTableAdapters.Ventas_DetalleTableAdapter ventadetalleTableAdapter = new BaseDeDatosDataSetTableAdapters.Ventas_DetalleTableAdapter();
        BaseDeDatosDataSetTableAdapters.PLU_detalleTableAdapter pludetalleTableAdapter = new BaseDeDatosDataSetTableAdapters.PLU_detalleTableAdapter();
        BaseDeDatosDataSetTableAdapters.Vendedor_detalleTableAdapter vendedordetalleTableAdapter = new BaseDeDatosDataSetTableAdapters.Vendedor_detalleTableAdapter();

        public void actualizar_basculas()   
        {
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

            worker.RunWorkerAsync();
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            worker.CancelAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<Thread> myThreadCollection = new List<Thread>();

            fi3 = new FileStream(Variable.appPath + "\\bitacora.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            sr3 = new StreamWriter(fi3, System.Text.UnicodeEncoding.Unicode);

            sr3.WriteLine("Iniciando el proceso de envio de pendientes a las basculas..");

            basculasTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            basculasTableAdapter.Connection.CreateCommand().CommandText = "SELECT * FROM Bascula ORDER BY id_bascula";
            basculasTableAdapter.Fill(baseDeDatosDataSet.Bascula);
            
            int iCountThread = 0;       

            foreach (DataRow Dbas in baseDeDatosDataSet.Bascula.Rows)
            {
                sr3.WriteLine("Enviando informacion a la bascula " + Dbas["no_serie"].ToString() + " Fecha -->" + DateTime.Now.ToLongTimeString());
                Form1.toolLabel.Text = "Procesando la bascula " + Dbas["no_serie"].ToString();

                if (Convert.ToInt16(Dbas["tipo_conec"].ToString()) != (int)ESTADO.tipoConexionesEnum.PKUSBCOM)
                {
                    #region Conexion TCP

                    Console.WriteLine("Se Inicia el proceso en paralelo de actualizacion");

                    WorkerForm1 workerObject = new WorkerForm1(iCountThread, Dbas["dir_IP"].ToString(), Convert.ToInt32(Dbas["id_bascula"].ToString()), Convert.ToInt32(Dbas["id_grupo"].ToString()), 0);

                    Thread t = new Thread(() =>
                    {
                        workerObject.vSendDatosBascula();
                    });

                    t.Start();
                    myThreadCollection.Add(t);

                    iCountThread++;

                    Console.WriteLine("Se finaliza el proceso en paralelo de actualizacion");

                    #endregion
                }
                else
                {
                    #region Conexion Serial
                    /*
                    if (Convert.ToInt32(Dbas["id_grupo"].ToString()) == 0)
                    {
                        if (EnviaDatosA_Bascula(Convert.ToInt16(Dbas["puerto"].ToString()), Convert.ToInt16(Dbas["baud"].ToString()),Convert.ToInt32(Dbas["id_grupo"].ToString()), Convert.ToInt32(Dbas["id_bascula"].ToString()), Dbas["nserie"].ToString()))
                            //actualiza = true;
                        else
                            //actualiza = false;
                    }
                    else
                    {
                        if (EnviaDatosA_Bascula(Convert.ToInt16(Dbas["puerto"].ToString()), Convert.ToInt16(Dbas["baud"].ToString()), Convert.ToInt32(Dbas["id_grupo"].ToString()), 0, Dbas["nserie"].ToString()))
                            actualiza = true;
                        else
                            actualiza = false;
                    }*/
                    #endregion
                }

                Form1.toolLabel.Text = Variable.SYS_MSJ[440, Variable.idioma] + Dbas["no_serie"].ToString();
            }

            foreach (var thread in myThreadCollection)
            {
                thread.Join();
            }

            WorkerFormWait workerObject1 = new WorkerFormWait("Actualizando base de datos…");
            Thread t1 = new Thread(workerObject1.vShowMsg);
            t1.Start();

            sr3.WriteLine("Modificando estado de pendiente pendientes ....");

            Conec.CadenaSelect = "UPDATE Ingredientes SET pendiente= " + false + ", enviado= " + false + " WHERE ( pendiente = " + true + ")";
            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "Ingredientes");

            Conec.CadenaSelect = "UPDATE Oferta SET pendiente= " + false + ", enviado= " + false + " WHERE ( pendiente = " + true + ")";
            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "Oferta");

            Conec.CadenaSelect = "UPDATE Publicidad SET pendiente= " + false + ", enviado= " + false + " WHERE ( pendiente = " + true + ")";
            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "Publicidad");

            Conec.CadenaSelect = "UPDATE Vendedor SET pendiente= " + false + ", enviado= " + false + " WHERE ( pendiente = " + true + ")";
            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "Vendedor");

            Conec.CadenaSelect = "UPDATE Prod_detalle SET pendiente= " + false + ", enviado= " + false + " WHERE ( pendiente = " + true + ")";
            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "Prod_detalle");

            Conec.CadenaSelect = "UPDATE PLU_detalle SET pendiente= " + false + ", enviado= " + false + " WHERE ( pendiente = " + true + ")";
            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "PLU_detalle");

            Conec.CadenaSelect = "UPDATE Productos SET pendiente= " + false + ", enviado= " + false + " WHERE ( pendiente = " + true + ")";
            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "Productos");

            sr3.WriteLine("Termino de proceso de actualizacion a la bascula -->" + System.DateTime.Now.ToLongTimeString());
            
            sr3.Close();
            fi3.Close();

            workerObject1.vEndShowMsg();
            t1.Join();
        }

        public void vActualizar_Bascula_PV(List<string> sIps, List<int> lCodigosProducto, List<int> lCodigosVendedores)
        {
            List<Thread> myThreadCollection = new List<Thread>();

            basculasTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            basculasTableAdapter.Connection.CreateCommand().CommandText = "SELECT * FROM Bascula ORDER BY id_bascula";
            basculasTableAdapter.Fill(baseDeDatosDataSet.Bascula);

            int iCountThread = 0;
            int i=0;

            Console.WriteLine("Se Inicia el proceso en paralelo de actualizacion");

            do{
                string sIp = sIps[i];

                if (sIp.Length > 0)
                {
                    WorkerForm1 workerObject = new WorkerForm1(iCountThread, sIp, ref lCodigosProducto, ref lCodigosVendedores, 1);

                    Thread t = new Thread(() =>
                    {
                        workerObject.vSendDatosBascula();
                    });

                    t.Start();
                    myThreadCollection.Add(t);

                    iCountThread++;
                    i++;
                }
            }while(i < sIps.Count);

            foreach (var thread in myThreadCollection)
            {
                thread.Join();
            }

            Console.WriteLine("Se finaliza el proceso en paralelo de actualizacion");

            WorkerFormWait workerObject1 = new WorkerFormWait("Actualizando base de datos…");
            Thread t1 = new Thread(workerObject1.vShowMsg);
            t1.Start();

            for (i = 0; i < lCodigosProducto.Count; i++)
            {
                Conec.CadenaSelect = "UPDATE Productos SET pendiente= " + false + ", enviado= " + false + " WHERE ( Codigo = " + lCodigosProducto[i] + ")";
                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "Productos");
            }

            for (i = 0; i < lCodigosVendedores.Count; i++)
            {
                Conec.CadenaSelect = "UPDATE Vendedor SET pendiente= " + false + ", enviado= " + false + " WHERE ( id_vendedor = " + lCodigosVendedores[i] + ")";
                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "Vendedor");
            }

            workerObject1.vEndShowMsg();
            t1.Join();
        }

        public void vActualizar_Bascula_Precio()
        {
            List<Thread> myThreadCollection = new List<Thread>();

            basculasTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            basculasTableAdapter.Connection.CreateCommand().CommandText = "SELECT * FROM Bascula ORDER BY id_bascula";
            basculasTableAdapter.Fill(baseDeDatosDataSet.Bascula);

            int iCountThread = 0;

            int iNoConectSend = 0;

            WorkerForm1 workerObject = null;

            foreach (DataRow Dbas in baseDeDatosDataSet.Bascula.Rows)
            {
                if (Convert.ToInt16(Dbas["tipo_conec"].ToString()) != (int)ESTADO.tipoConexionesEnum.PKUSBCOM)
                {
                    #region Conexion TCP

                    Console.WriteLine("Se Inicia el proceso en paralelo de actualizacion");

                    workerObject = new WorkerForm1(iCountThread, Dbas["dir_IP"].ToString(), Convert.ToInt32(Dbas["id_bascula"].ToString()), Convert.ToInt32(Dbas["id_grupo"].ToString()), 2);

                    Thread t = new Thread(delegate()
                    {
                        iNoConectSend += workerObject.vSendDatosBascula();
                    });

                    t.Start();
                    myThreadCollection.Add(t);

                    iCountThread++;

                    Console.WriteLine("Se finaliza el proceso en paralelo de actualizacion");

                    #endregion
                }
                else
                {
                    #region Conexion Serial
                    ProgressContinue pro = new ProgressContinue();
                    pro.IniciaProcess(Variable.SYS_MSJ[192, Variable.idioma]);
                    SerialPort serialPort1 = new SerialPort();
                    if (SR.OpenPort(ref serialPort1, Dbas["puerto"].ToString(), Convert.ToInt32(Dbas["baud"].ToString())))
                    {
                        SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)10);
                        SR.ReceivedCOMSerial(ref serialPort1);
                        Enviar_Precios(ref serialPort1, Convert.ToInt32(Dbas["id_bascula"].ToString()), Convert.ToInt32(Dbas["id_grupo"].ToString()), Dbas["puerto"].ToString(), pro);
                        SR.SendCOMSerial(ref serialPort1, "dXXX" + (char)10);
                        SR.ReceivedCOMSerial(ref serialPort1);

                        SR.ClosePort(ref serialPort1);
                        Form1.toolLabel.Text = Variable.SYS_MSJ[193, Variable.idioma];  // "Proceso concluido .....";  
                        pro.TerminaProcess();
                    }
                    else 
                    {
                        Form1.toolLabel.Text = Variable.SYS_MSJ[193, Variable.idioma];  // "Proceso concluido .....";
                        pro.TerminaProcess();
                        Thread.Sleep(500);
                        MessageBox.Show(Variable.SYS_MSJ[426, Variable.idioma],
                            Variable.SYS_MSJ[39, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }


                    /*
                    if (Convert.ToInt32(Dbas["id_grupo"].ToString()) == 0)
                    {
                        if (EnviaDatosA_Bascula(Convert.ToInt16(Dbas["puerto"].ToString()), Convert.ToInt16(Dbas["baud"].ToString()),Convert.ToInt32(Dbas["id_grupo"].ToString()), Convert.ToInt32(Dbas["id_bascula"].ToString()), Dbas["nserie"].ToString()))
                            actualiza = true;
                        else
                            actualiza = false;
                    }
                    else
                    {
                        if (EnviaDatosA_Bascula(Convert.ToInt16(Dbas["puerto"].ToString()), Convert.ToInt16(Dbas["baud"].ToString()), Convert.ToInt32(Dbas["id_grupo"].ToString()), 0, Dbas["nserie"].ToString()))
                            actualiza = true;
                        else
                            actualiza = false;
                    }*/
                    #endregion
                }

                Form1.toolLabel.Text = "Finalizando la bascula " + Dbas["no_serie"].ToString();
            }

            foreach (var thread in myThreadCollection)
            {
                thread.Join();
            }

            if (iNoConectSend == 0)
            {
                WorkerFormWait workerObject1 = new WorkerFormWait(Variable.SYS_MSJ[327, Variable.idioma]);
                Thread t1 = new Thread(workerObject1.vShowMsg);
                t1.Start();

                Conec.CadenaSelect = "UPDATE Productos SET pendiente= " + false + ", enviado= " + false + " WHERE ( pendiente = " + true + ")";
                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "Productos");

                Thread.Sleep(3000);
                workerObject1.vEndShowMsg();
                t1.Join();
            }
        }
        

        #region FUNCIONES PARA ENVIO DE INFORMACION A LAS BASCULAS

        public bool EnviaDatosA_Bascula(string P_COMM,  int P_BAUD, long nSucursal, long nbascula,string nserie)
        {
            SerialPort Cliente_bascula = new SerialPort();
            if (SR.OpenPort(ref Cliente_bascula, P_COMM, P_BAUD)) //Variable.P_COMM, Variable.Buad))
            {
                SR.SendCOMSerial(ref Cliente_bascula, "bXXX" + (char)9 + (char)10);

                Form1.toolLabel.Text = "Envio de Tramas pendientes...";
                Envia_Dato_Pendientes(ref Cliente_bascula, nbascula, nSucursal,P_COMM);
                Form1.toolLabel.Text = "Envio de Imagenes...";
                Envia_Imagenes(ref Cliente_bascula, nbascula, nSucursal,P_COMM);

                Form1.toolLabel.Text = "Envio de productos...";
                if (Enviar_Productos(ref Cliente_bascula, nbascula, nSucursal,P_COMM) > 0)
                {
                    Form1.toolLabel.Text = "Envio de carpetas...";
                    Enviar_Carpetas_Bascula(ref Cliente_bascula, nbascula, nSucursal,P_COMM);
                    Form1.toolLabel.Text = "Envio de asociaciones...";
                    Enviar_DetalleCarpeta_Bascula(ref Cliente_bascula, nbascula, nSucursal,P_COMM);
                    Enviar_DetalleProducto_Bascula(ref Cliente_bascula, nbascula, nSucursal,P_COMM);
                }
                Form1.toolLabel.Text = "Envio de Info adicional...";
                Enviar_InfoAdicional(ref Cliente_bascula, nbascula, nSucursal,P_COMM);
                Form1.toolLabel.Text = "Envio de Ofertas...";
                Envia_Ofertas(ref Cliente_bascula, nbascula, nSucursal,P_COMM);
                Form1.toolLabel.Text = "Envio de Mensajes...";
                Enviar_Publicidad(ref Cliente_bascula, nbascula, nSucursal,P_COMM);
                Form1.toolLabel.Text = "Envio de Vendedores...";
                Enviar_Vendedores(ref Cliente_bascula, nbascula, nSucursal,P_COMM);

                sr3.WriteLine("Proceso finalizado para la bascula " + nserie + "--> " + System.DateTime.Now.ToLongTimeString());
                SR.ClosePort(ref Cliente_bascula);
                return true;
            }
            else
            {
                sr3.WriteLine("La bascula " + nbascula.ToString() + "con IP " + nserie + " no responde --> " + System.DateTime.Now.ToLongTimeString());
                return false;
            }
        }
        public void Envia_Dato_Pendientes(string direccionIP, ref Socket Cliente_bascula, long nbascula, long ngrupo)
        {
            int reg_leido = 0;
            int reg_envio = 0;
            int reg_total;
            string Variable_frame = null;

            BufferSalidaTableAdapter.Fill(baseDeDatosDataSet.BufferSalida);

            Variable_frame = "";
            DataRow[] DR_Buffer = baseDeDatosDataSet.BufferSalida.Select("id_bascula = " + nbascula + " and id_grupo = " + ngrupo);
            reg_total = DR_Buffer.Length;

            if (reg_total > 0)
            {
                foreach (DataRow DA in DR_Buffer)
                {
                    Variable_frame = DA["comando"].ToString();
                    reg_leido++;
                    reg_envio = Convert.ToInt32(DA["id_error"].ToString());
                    Command_Pendiente(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, nbascula, ngrupo, reg_envio);
                    Variable_frame = "";
                }
            }
        }
        public void Envia_Dato_Pendientes(ref SerialPort Cliente_bascula, long nbascula, long ngrupo,string puerto)
        {
            int reg_leido = 0;
            int reg_envio = 0;
            int reg_total;
            string[] Dato_recibido = null;
            string Variable_frame = null;
            BufferSalidaTableAdapter.Fill(baseDeDatosDataSet.BufferSalida);

            Variable_frame = "";
            DataRow[] DR_Buffer = baseDeDatosDataSet.BufferSalida.Select("id_bascula = " + nbascula + " and id_grupo = " + ngrupo);
            reg_total = DR_Buffer.Length;

            sr3.WriteLine("Envio de tramas pendientes --> " + System.DateTime.Now.ToLongTimeString());
           
            foreach (DataRow DA in DR_Buffer)
            {
                Variable_frame = DA["comando"].ToString();
                reg_leido++;
                reg_envio = Convert.ToInt32(DA["id_error"].ToString());

                SR.SendCOMSerial(ref Cliente_bascula, Variable_frame, ref Dato_recibido);

                if (Dato_recibido[0].IndexOf("Ok") > 0) Borrar_Trama_Pendiente(nbascula, ngrupo, reg_envio);
                if (Dato_recibido[0].IndexOf("Error") > 0) Guardar_Trama_pendiente(nbascula, ngrupo, Variable_frame);

                sr3.WriteLine("Enviando => " + Variable_frame + "--> " + System.DateTime.Now.ToLongTimeString());

                Variable_frame = "";
            }

            sr3.WriteLine("Finalizado envio de tramas pendiente --> " + System.DateTime.Now.ToLongTimeString());
        }

        public bool Envia_Borrar_Asociacion(string direccionIP, ref Socket Cliente_bascula)
        {
            string[] Dato_Recibido = null;
            string reg_enviado;
            string Variable_frame = null;
            bool Limpiar = false;

            Variable_frame = "";
            Variable_frame = "PAXX" + (char)9 + (char)10;

            Cte.Envio_Dato(ref Cliente_bascula, direccionIP, Variable_frame, ref Dato_Recibido);
            sr3.WriteLine("Enviando -> " + Variable_frame);
            if (Dato_Recibido != null)
            {
                reg_enviado = Variable_frame.Substring(4);
                if (Dato_Recibido[0].IndexOf("Error") >= 0) { Limpiar = false; }
                if (Dato_Recibido[0].IndexOf("Ok") >= 0) { Limpiar = true; }
            }
            return Limpiar;
        }

        public void Enviar_DetalleCarpeta_Bascula(string direccionIP, ref Socket Cliente_bascula, long nbascula, long ngrupo)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msj_recibido;
            string Variable_frame = null;
            int reg_leido = 0;
            int reg_envio = 0;
            int reg_total;
            carpetadetalleTableAdapter.Fill(baseDeDatosDataSet.carpeta_detalle);
            DataRow[] DR = baseDeDatosDataSet.carpeta_detalle.Select("id_bascula = " + nbascula + " and id_grupo = " + ngrupo);

            Variable_frame = "";
            reg_total = DR.Length;

            sr3.WriteLine("Envio Asociacion de carpetas --> " + System.DateTime.Now.ToLongTimeString());
            if (reg_total > 0)
            {
                foreach (DataRow DR_Detail in DR)
                {
                    Variable_frame = Variable_frame + Genera_Trama_Carpeta_Detalle(DR_Detail);

                    reg_leido++;
                    if (reg_leido > 4)
                    {
                        reg_envio = reg_envio + reg_leido;
                        Msj_recibido = Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, nbascula, ngrupo, "GA");

                        if (Msj_recibido != null) Modifica_DetalleCarpeta(nbascula, ngrupo, Variable_frame.Split(chr), true);
                        else Modifica_DetalleCarpeta(nbascula, ngrupo, Variable_frame.Split(chr), false);

                        sr3.WriteLine("Enviando => " + Variable_frame + "--> " + System.DateTime.Now.ToLongTimeString());
                        reg_leido = 0;
                        Variable_frame = "";
                    }
                }
                if (Variable_frame.Length > 0 && reg_leido <= 4)
                {
                    reg_envio = reg_envio + reg_leido;
                    Msj_recibido = Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, nbascula, ngrupo, "GA");

                    if (Msj_recibido != null) Modifica_DetalleCarpeta(nbascula, ngrupo, Variable_frame.Split(chr), true);
                    else Modifica_DetalleCarpeta(nbascula, ngrupo, Variable_frame.Split(chr), false);

                    sr3.WriteLine("Enviando => " + Variable_frame + "--> " + System.DateTime.Now.ToLongTimeString());
                    reg_leido = 0;
                    Variable_frame = "";
                }
                Command_Limpiar(direccionIP, ref Cliente_bascula, "GAF0");
            }
            sr3.WriteLine("Finalizando Asociacion de carpetas --> " + System.DateTime.Now.ToLongTimeString());

        }
        public void Enviar_DetalleCarpeta_Bascula(ref SerialPort Cliente_bascula, long nbascula, long ngrupo, string puerto)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msj_recibido;
            string strcomando;
            string Variable_frame = null;
            string[] Dato_Recibido = null;

            int reg_leido = 0;
            int reg_envio = 0;
            int reg_total;
            carpetadetalleTableAdapter.Fill(baseDeDatosDataSet.carpeta_detalle);
            DataRow[] DR = baseDeDatosDataSet.carpeta_detalle.Select("id_bascula = " + nbascula + " and id_grupo = " + ngrupo);

            Variable_frame = "";
            reg_total = DR.Length;

            sr3.WriteLine("Envio Asociacion de carpetas --> " + System.DateTime.Now.ToLongTimeString());

            if (reg_total > 0)
            {
                foreach (DataRow DR_Detail in DR)
                {
                    Variable_frame = Variable_frame + Genera_Trama_Carpeta_Detalle(DR_Detail);

                    reg_leido++;
                    if (reg_leido > 4)
                    {
                        reg_envio = reg_envio + reg_leido;
                        Msj_recibido = Variable_frame;
                        strcomando = "GA" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;

                        SR.SendCOMSerial(ref Cliente_bascula, strcomando, ref Dato_Recibido);

                        if (Dato_Recibido[0].IndexOf("Ok") > 0) Modifica_DetalleCarpeta(nbascula, ngrupo, Msj_recibido.Split(chr), true);
                        if (Dato_Recibido[0].IndexOf("Error") > 0) Modifica_DetalleCarpeta(nbascula, ngrupo, Msj_recibido.Split(chr), false);

                        sr3.WriteLine("Enviando => " + Variable_frame + "--> " + System.DateTime.Now.ToLongTimeString());
                        reg_leido = 0;
                        Variable_frame = "";
                    }
                }
                if (Variable_frame.Length > 0 && reg_leido <= 4)
                {
                    reg_envio = reg_envio + reg_leido;
                    Msj_recibido = Variable_frame;
                    strcomando = "GA" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;

                    SR.SendCOMSerial(ref Cliente_bascula, strcomando, ref Dato_Recibido);

                    if (Dato_Recibido[0].IndexOf("Ok") > 0) Modifica_DetalleCarpeta(nbascula, ngrupo, Msj_recibido.Split(chr), true);
                    if (Dato_Recibido[0].IndexOf("Error") > 0) Modifica_DetalleCarpeta(nbascula, ngrupo, Msj_recibido.Split(chr), false);

                    sr3.WriteLine("Enviando => " + Variable_frame + "--> " + System.DateTime.Now.ToLongTimeString());
                    reg_leido = 0;
                    Variable_frame = "";
                }

                Variable_frame = "GAF0" + +(char)9 + (char)10;
                SR.SendCOMSerial(ref Cliente_bascula, Variable_frame);
            }
            sr3.WriteLine("Finalizando Asociacion de carpetas --> " + System.DateTime.Now.ToLongTimeString());

        }

        public void Enviar_DetalleProducto_Bascula(string direccionIP, ref Socket Cliente_bascula, long nbascula, long ngrupo)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msj_recibido;
            string Variable_frame = null;
            int reg_leido = 0;
            int reg_envio = 0;

            prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);

            DataRow[] DR = baseDeDatosDataSet.Prod_detalle.Select("pendiente = " + true);

            Variable_frame = "";

            if (DR.Length > 0)
            {

                basculasTableAdapter.Fill(baseDeDatosDataSet.Bascula);
                baseDeDatosDataSet.Bascula.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Bascula.id_basculaColumn, baseDeDatosDataSet.Bascula.id_grupoColumn };
                object[] clave = new object[2];

                foreach (DataRow DA in DR)
                {
                    clave[0] = nbascula;
                    clave[1] = Convert.ToInt32(DA["id_grupo"].ToString());

                    DataRow DF = baseDeDatosDataSet.Bascula.Rows.Find(clave);

                    if (DF != null)
                    {

                        Variable_frame = Variable_frame + Genera_Trama_Producto_Detalle(DA);
                        reg_leido++;
                        if (reg_leido > 4)
                        {
                            reg_envio = reg_envio + reg_leido;
                            Msj_recibido = Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, nbascula, ngrupo, "GA");
                            reg_leido = 0;
                            Variable_frame = "";
                        }
                    }
                }
                if (Variable_frame.Length > 0 && reg_leido <= 4)
                {
                    reg_envio = reg_envio + reg_leido;
                    Msj_recibido = Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, nbascula, ngrupo, "GA");
                    reg_leido = 0;
                    Variable_frame = "";
                }
                Command_Limpiar(direccionIP, ref Cliente_bascula, "GAF0");
            }
        }
        public void Enviar_DetalleProducto_Bascula(ref SerialPort Cliente_bascula, long nbascula, long ngrupo, string puerto)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msj_recibido;
            string strcomando;
            string Variable_frame = null;
            string[] Dato_Recibido=null;
            int reg_leido = 0;
            int reg_envio = 0;
            int reg_total;

            DataRow[] DR = baseDeDatosDataSet.Prod_detalle.Select("id_bascula = " + nbascula + " and id_grupo = " + ngrupo);

            Variable_frame = "";
            reg_total = DR.Length;

            sr3.WriteLine("Envio Asociacion de productos --> " + System.DateTime.Now.ToLongTimeString());
            foreach (DataRow DR_Detail in DR)
            {
                Variable_frame = Variable_frame + Genera_Trama_Producto_Detalle(DR_Detail);

                reg_leido++;
                if (reg_leido > 4)
                {
                    reg_envio = reg_envio + reg_leido;
                    Msj_recibido = Variable_frame;
                    strcomando = "GA" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;

                    SR.SendCOMSerial(ref Cliente_bascula, strcomando, ref Dato_Recibido);

                    if (Dato_Recibido[0].IndexOf("Ok") > 0) Modifica_DetalleProducto(nbascula, ngrupo, Msj_recibido.Split(chr), true);
                    if (Dato_Recibido[0].IndexOf("Error") > 0) Modifica_DetalleProducto(nbascula, ngrupo, Msj_recibido.Split(chr), false);

                    sr3.WriteLine("Enviando => " + Variable_frame + "--> " + System.DateTime.Now.ToLongTimeString());
                    reg_leido = 0;
                    Variable_frame = "";
                }
            }
            if (Variable_frame.Length > 0 && reg_leido <= 4)
            {
                reg_envio = reg_envio + reg_leido;
                Msj_recibido = Variable_frame;
                strcomando = "GA" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;

                SR.SendCOMSerial(ref Cliente_bascula, strcomando, ref Dato_Recibido);

                if (Dato_Recibido[0].IndexOf("Ok") > 0) Modifica_DetalleProducto(nbascula, ngrupo, Msj_recibido.Split(chr), true);
                if (Dato_Recibido[0].IndexOf("Error") > 0) Modifica_DetalleProducto(nbascula, ngrupo, Msj_recibido.Split(chr), false);

                sr3.WriteLine("Enviando => " + Variable_frame + "--> " + System.DateTime.Now.ToLongTimeString());
                reg_leido = 0;
                Variable_frame = "";
            }
            if (reg_envio > 0)
            {
                Variable_frame = "GAF0" + +(char)9 + (char)10;
                SR.SendCOMSerial(ref Cliente_bascula, Variable_frame);
            }
            sr3.WriteLine("Finalizando Asociacion de carpetas --> " + System.DateTime.Now.ToLongTimeString());

        }

        private void Enviar_Carpetas_Bascula(string direccionIP, ref Socket Cliente_bascula, long nbascula, long ngrupo)
        {
            string Msj_recibido;
            string Variable_frame = null;
            int reg_leido = 0;
            int reg_envio = 0;
            int reg_total;

            carpetadetalleTableAdapter.Fill(baseDeDatosDataSet.carpeta_detalle);
            DataRow[] DR = baseDeDatosDataSet.carpeta_detalle.Select("id_bascula = " + nbascula + " and id_grupo = " + ngrupo + "and pendiente = " + true);

            Variable_frame = "";
            reg_total = DR.Length;
            sr3.WriteLine("Envio las Carpetas --> " + System.DateTime.Now.ToLongTimeString());

            if (reg_total > 0)
            {
                foreach (DataRow DR_Folder in DR)
                {
                    Variable_frame = Variable_frame + Genera_Trama_Carpeta(DR_Folder);

                    reg_leido++;
                    if (reg_leido > 4)
                    {
                        reg_envio = reg_envio + reg_leido;
                        Msj_recibido = Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, nbascula, ngrupo, "GC");

                        sr3.WriteLine("Enviando => " + Variable_frame + "--> " + System.DateTime.Now.ToLongTimeString());
                        reg_leido = 0;
                        Variable_frame = "";
                    }
                }
                if (Variable_frame.Length > 0 && reg_leido <= 4)
                {
                    reg_envio = reg_envio + reg_leido;
                    Msj_recibido = Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, nbascula, ngrupo, "GC");

                    sr3.WriteLine("Enviando => " + Variable_frame + "--> " + System.DateTime.Now.ToLongTimeString());
                    reg_leido = 0;
                    Variable_frame = "";
                }
            }

            sr3.WriteLine("Finalizando las carpetas --> " + System.DateTime.Now.ToLongTimeString());
        }
        private void Enviar_Carpetas_Bascula(ref SerialPort Cliente_bascula, long nbascula, long ngrupo, string puerto)
        {
            string Msj_recibido;
            string Variable_frame = null;
            string strcomando;
            string[] Dato_Recibido = null;

            int reg_leido = 0;
            int reg_envio = 0;
            int reg_total;

            carpetadetalleTableAdapter.Fill(baseDeDatosDataSet.carpeta_detalle);
            DataRow[] DR = baseDeDatosDataSet.carpeta_detalle.Select("id_bascula = " + nbascula + " and id_grupo = " + ngrupo + "and pendiente = " + true);

            Variable_frame = "";
            reg_total = DR.Length;
            sr3.WriteLine("Envio las Carpetas --> " + System.DateTime.Now.ToLongTimeString());

            foreach (DataRow DR_Folder in DR)
            {
                Variable_frame = Variable_frame + Genera_Trama_Carpeta(DR_Folder);

                reg_leido++;
                if (reg_leido > 4)
                {
                    reg_envio = reg_envio + reg_leido;
                    Msj_recibido = Variable_frame;
                    strcomando = "GC" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;

                    SR.SendCOMSerial(ref Cliente_bascula, strcomando, ref Dato_Recibido);
                    if (Dato_Recibido != null)
                    {
                        if (Dato_Recibido[0].IndexOf("Error") >= 0) Guardar_Trama_pendiente(nbascula,ngrupo, strcomando);                            
                    }

                    sr3.WriteLine("Enviando => " + Variable_frame + "--> " + System.DateTime.Now.ToLongTimeString());
                    reg_leido = 0;
                    Variable_frame = "";
                }
            }
            if (Variable_frame.Length > 0 && reg_leido <= 4)
            {
                reg_envio = reg_envio + reg_leido;
                Msj_recibido = Variable_frame;
                strcomando = "GC" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;

                SR.SendCOMSerial(ref Cliente_bascula, strcomando, ref Dato_Recibido);
                if (Dato_Recibido != null)
                {
                    if (Dato_Recibido[0].IndexOf("Error") >= 0) Guardar_Trama_pendiente(nbascula, ngrupo, strcomando);
                }
                sr3.WriteLine("Enviando => " + Variable_frame + "--> " + System.DateTime.Now.ToLongTimeString());
                reg_leido = 0;
                Variable_frame = "";
            }

            sr3.WriteLine("Finalizando las carpetas --> " + System.DateTime.Now.ToLongTimeString());
        }
        
        public int Enviar_Productos(ref SerialPort Cliente_bascula, long nbascula, long ngrupo, string puerto)
        {
            char[] chr = new char[2] { (char)10, (char)13 };          
            string Msj_recibido;
            string strcomando;
            string Variable_frame = null;
            string[] Dato_enviado= null;

            int reg_leido = 0;
            int reg_envio = 0;
            int reg_detalle = 0;
            int reg_total;

            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);
            prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);

            DataRelation Producto_Detalle = baseDeDatosDataSet.Relations["ProductosProd_detalle"];

            Variable_frame = "";
            reg_total = baseDeDatosDataSet.Productos.Select("pendiente = " + true).Length;

            sr3.WriteLine("Enviando los productos --> " + System.DateTime.Now.ToLongTimeString());

            if (reg_total > 0)
            {
                foreach (DataRow DA in baseDeDatosDataSet.Productos.Rows)
                {
                    foreach (DataRow PR in DA.GetChildRows(Producto_Detalle))
                    {
                        if (Convert.ToInt32(PR["id_bascula"].ToString()) == nbascula && Convert.ToInt32(PR["id_grupo"].ToString()) == ngrupo && Convert.ToBoolean(DA["pendiente"]) == true)
                        {
                            Variable_frame = Variable_frame + Genera_Trama_Producto(DA);
                            reg_leido++;
                            reg_detalle++;
                            if (reg_leido > 4)
                            {
                                reg_envio = reg_envio + reg_leido;
                                strcomando = "GP" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                                Msj_recibido = Variable_frame;
                                SR.SendCOMSerial(ref Cliente_bascula, strcomando,ref Dato_enviado);
                                if (Dato_enviado != null)
                                {
                                    if (Dato_enviado[0].IndexOf("Error") >= 0) 
                                    {
                                       // Guardar_Trama_pendiente(nbascula,ngrupo,puerto.ToString(), strcomando);
                                       // Modifica_Estado_Productos(Msj_recibido.Split(chr), false);
                                        Crea_Producto_Detalle(nbascula, ngrupo, Msj_recibido.Split(chr), true, false);
                                    }
                                    if (Dato_enviado[0].IndexOf("Ok") >= 0)
                                    {
                                        Crea_Producto_Detalle(nbascula, ngrupo, Msj_recibido.Split(chr), false, true);
                                    }
                                    //Modifica_Estado_Productos(Msj_recibido.Split(chr), true);                                    
                                }
                                sr3.WriteLine("Enviando -> " + Variable_frame + "->" + System.DateTime.Now.ToLongTimeString());
                                reg_leido = 0;
                                Variable_frame = "";
                            }
                        }
                    }
                }
                if (Variable_frame.Length > 0 && reg_leido <= 4)
                {
                    reg_envio = reg_envio + reg_leido;
                    strcomando = "GP" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                    Msj_recibido = Variable_frame;
                    SR.SendCOMSerial(ref Cliente_bascula, strcomando, ref Dato_enviado);
                    if (Dato_enviado[0].IndexOf("Error") >= 0)
                    {
                       // Guardar_Trama_pendiente(nbascula, ngrupo, puerto.ToString(), strcomando);
                       // Modifica_Estado_Productos(Msj_recibido.Split(chr), false);
                        Crea_Producto_Detalle(nbascula, ngrupo, Msj_recibido.Split(chr), true, false);
                    }
                    if (Dato_enviado[0].IndexOf("Ok") >= 0)
                    {
                        Crea_Producto_Detalle(nbascula, ngrupo, Msj_recibido.Split(chr), false, true);
                    }
                    //Modifica_Estado_Productos(Msj_recibido.Split(chr), true);  

                    sr3.WriteLine("Enviando -> " + Variable_frame + "->" + System.DateTime.Now.ToLongTimeString());
                    reg_leido = 0;
                    Variable_frame = "";
                }
                if (reg_envio > 0)
                {
                    Variable_frame = "GAF0" + +(char)9 + (char)10;
                    SR.SendCOMSerial(ref Cliente_bascula, Variable_frame);
                }

            }
            sr3.WriteLine("Finalizando los productos --> " + System.DateTime.Now.ToLongTimeString());
            return reg_detalle;
        }

        public int Enviar_Precios(ref SerialPort Cliente_bascula, long nbascula, long ngrupo, string puerto, ProgressContinue pMsg)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msj_recibido;
            int reg_leido = 0;
            int reg_envio = 0;
            int reg_detalle = 0;
            int reg_total_pendientes;
            string strcomando;
            string[] Dato_enviado = null;

            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);
            prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);
            pludetalleTableAdapter.Fill(baseDeDatosDataSet.PLU_detalle);

            DataRelation Producto_Detalle = baseDeDatosDataSet.Relations["ProductosProd_detalle"];
            DataRelation Plus_Detalle = baseDeDatosDataSet.Relations["Productos_PLU_detalle"];

            reg_total_pendientes = baseDeDatosDataSet.Productos.Select("pendiente = " + true + " and borrado = " + false).Length;
            DataRow[] DT = baseDeDatosDataSet.Productos.Select("pendiente = " + true + " and borrado = " + false);

            List<int> myCollectionProdProd_Det = new List<int>();
            List<int> myCollectionProdPlu_Det = new List<int>();
            List<int> myCollectionSend = new List<int>();

            if (DT.Length > 0)
            {
                foreach (DataRow DA in DT)
                {
                    foreach (DataRow PR in DA.GetChildRows(Producto_Detalle))
                    {
                        long iIdBascula = Convert.ToInt32(PR["id_bascula"].ToString());
                        //Console.WriteLine();
                        if (iIdBascula == nbascula)
                        {
                            myCollectionProdProd_Det.Add(Convert.ToInt32(PR["id_producto"].ToString()));
                        }
                    }
                }

                foreach (DataRow DA in DT)
                {
                    foreach (DataRow PR in DA.GetChildRows(Plus_Detalle))
                    {
                        long iIdBascula = Convert.ToInt32(PR["id_bascula"].ToString());

                        if (iIdBascula == nbascula)
                        {
                            myCollectionProdPlu_Det.Add(Convert.ToInt32(PR["id_producto"].ToString()));
                        }
                    }
                }
            

                if (myCollectionProdProd_Det.Count > 0)
                {
                    for (int i = 0; i < myCollectionProdProd_Det.Count; i++)
                    {
                        myCollectionSend.Add(myCollectionProdProd_Det[i]);
                    }

                    if (myCollectionProdPlu_Det.Count > 0)
                    {
                        for (int i = 0; i < myCollectionProdPlu_Det.Count; i++)
                        {
                            if (myCollectionSend.BinarySearch(myCollectionProdPlu_Det[i]) < 0)
                            {
                                myCollectionSend.Add(myCollectionProdPlu_Det[i]);
                            }
                        }
                    }
                }
                else
                {
                    if (myCollectionProdPlu_Det.Count > 0)
                    {
                        for (int i = 0; i < myCollectionProdPlu_Det.Count; i++)
                        {
                            myCollectionSend.Add(myCollectionProdPlu_Det[i]);
                        }
                    }
                }

                Console.WriteLine("Enviando {0} productos", myCollectionSend.Count);

                if (myCollectionSend.Count > 0)
                {

                    pMsg.IniciaProcess(myCollectionSend.Count, Variable.SYS_MSJ[198, Variable.idioma] + " " + puerto + "... ");

                    productosTableAdapter.Fill(baseDeDatosDataSet.Productos);

                        string Variable_frame = "";

                        for (int i = 0; i < myCollectionSend.Count; i++)
                        {
                            DataRow DR = baseDeDatosDataSet.Productos.Rows.Find(myCollectionSend[i]);

                            if (DR != null)
                            {
                                Variable_frame = Variable_frame + Genera_Trama_Precio(DR);
                                reg_leido++;
                                reg_detalle++;

                                if (reg_leido > 4)
                                {
                                    reg_envio = reg_envio + reg_leido;

                                    pMsg.UpdateProcess(reg_leido, Variable.SYS_MSJ[198, Variable.idioma] + " " + puerto + "... ");

                                    //Msj_recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, nbascula, ngrupo, "Gp");
                                    strcomando = "Gp" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                                    Msj_recibido = Variable_frame;
                                    SR.SendCOMSerial(ref Cliente_bascula, strcomando, ref Dato_enviado);
                                    reg_leido = 0;
                                    Variable_frame = "";
                                }
                            }
                        }

                        if (Variable_frame.Length > 0 && reg_leido <= 4)
                        {
                            reg_envio = reg_envio + reg_leido;

                            pMsg.UpdateProcess(reg_leido, Variable.SYS_MSJ[198, Variable.idioma] + " " + puerto + "... ");

                            //Msj_recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, nbascula, ngrupo, "Gp");
                            strcomando = "Gp" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                            Msj_recibido = Variable_frame;
                            SR.SendCOMSerial(ref Cliente_bascula, strcomando, ref Dato_enviado);
                            Variable_frame = "";
                            reg_leido = 0;
                        }

                        if (reg_envio > 0)
                        {                         
                            //Env.Command_Limpiar(direccionIP, ref Cliente_bascula, "GpF0");
                            Variable_frame = "GpF0" + +(char)9 + (char)10;
                            SR.SendCOMSerial(ref Cliente_bascula, Variable_frame);
                        }
                }
            }

            return reg_detalle;
        }
        
        private void Enviar_Publicidad(ref SerialPort Cliente_bascula, long nbascula, long ngrupo, string puerto)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msj_recibido;
            string strcomando;
            string Variable_frame = null;
            string[] Dato_enviado = null;
            int reg_leido = 0;
            int reg_envio = 0;
            int reg_total;

            publicidadTableAdapter.Fill(baseDeDatosDataSet.Publicidad);
            public_DetalleTableAdapter.Fill(baseDeDatosDataSet.Public_Detalle);

            DataRelation Publicidad_Detalle = baseDeDatosDataSet.Relations["PublicidadPublic_Detalle"];

            Variable_frame = "";
            reg_total = baseDeDatosDataSet.Publicidad.Select("pendiente = " + true).Length;
            sr3.WriteLine("Enviando los mensajes --> " + System.DateTime.Now.ToLongTimeString());

            foreach (DataRow DA in baseDeDatosDataSet.Publicidad.Rows)
            {
                if (Convert.ToBoolean(DA["pendiente"]) == true)
                {
                    Variable_frame = Variable_frame + Genera_Trama_Publicidad(DA);
                    reg_leido++;
                    if (reg_leido > 4)
                    {
                        reg_envio = reg_envio + reg_leido;
                        strcomando = "GM" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                        Msj_recibido = Variable_frame;
                        SR.SendCOMSerial(ref Cliente_bascula, strcomando, ref Dato_enviado);
                        if (Dato_enviado != null)
                        {
                            if (Dato_enviado[0].IndexOf("Ok") >= 0)
                            {
                                Crea_Publicidad_Detalle(nbascula, ngrupo, Msj_recibido.Split(chr),false,true);
                                
                            }
                            if (Dato_enviado[0].IndexOf("Error") >= 0)
                            {
                               // Crea_Publicidad_Detalle(nbascula, ngrupo, Msj_recibido.Split(chr), true,false);
                               
                                Guardar_Trama_pendiente(nbascula, ngrupo, strcomando);
                            }
                        }
                        sr3.WriteLine("Enviando -> " + Variable_frame + "-->" + System.DateTime.Now.ToLongTimeString());
                        reg_leido = 0;
                        Variable_frame = "";
                    }
                }
            }
            if (Variable_frame.Length > 0 && reg_leido < 4)
            {
                reg_envio = reg_envio + reg_leido;
                strcomando = "GM" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                Msj_recibido = Variable_frame;
                SR.SendCOMSerial(ref Cliente_bascula, strcomando, ref Dato_enviado);
                if (Dato_enviado != null)
                {
                    if (Dato_enviado[0].IndexOf("Ok") >= 0)
                    {
                        Crea_Publicidad_Detalle(nbascula, ngrupo, Msj_recibido.Split(chr), false, true);
                        
                    }
                    if (Dato_enviado[0].IndexOf("Error") >= 0)
                    {
                       // Crea_Publicidad_Detalle(nbascula, ngrupo, Msj_recibido.Split(chr), true, false);
                        Guardar_Trama_pendiente(nbascula, ngrupo, strcomando);
                       
                    }
                }
                sr3.WriteLine("Enviando -> " + Variable_frame + "-->" + System.DateTime.Now.ToLongTimeString());
                reg_leido = 0;
                Variable_frame = "";
            }

            sr3.WriteLine("Finalizando los mensajes --> " + System.DateTime.Now.ToLongTimeString());
        }

        public void Enviar_Vendedores(string direccionIP, ref Socket Cliente_bascula, long nbascula, long ngrupo)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msj_recibido;
            string Variable_frame = null;
            int reg_leido = 0;
            int reg_envio = 0;

            Variable_frame = "";

            vendedorTableAdapter.Fill(baseDeDatosDataSet.Vendedor);
            vendedordetalleTableAdapter.Fill(baseDeDatosDataSet.Vendedor_detalle);

            DataRelation VendedorAdd_Detalle = baseDeDatosDataSet.Relations["Vendedor_detalle_Vendedor"];

            List<int> mycollectionVendedor = new List<int>();

            Variable_frame = "";
            DataRow[] DR_Selec = baseDeDatosDataSet.Vendedor.Select("pendiente = " + true);

            if (DR_Selec.Length > 0)
            {
                foreach (DataRow DA in DR_Selec)
                {
                    //foreach (DataRow PR in DA.GetChildRows(VendedorAdd_Detalle))
                    //{
                        //if (Convert.ToInt32(PR["id_bascula"].ToString()) == nbascula)
                        //{
                            mycollectionVendedor.Add(Convert.ToInt32(DA["id_vendedor"].ToString()));
                        //}
                    //}
                }

                if (mycollectionVendedor.Count > 0)
                {
                    string sComando = "XX" + (char)9 + (char)10;
                    Msj_recibido = Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");

                    if (Msj_recibido != null)
                    {
                        for (int i = 0; i < mycollectionVendedor.Count; i++)
                        {
                            DataRow DA = baseDeDatosDataSet.Vendedor.Rows.Find(mycollectionVendedor[i]);

                            Variable_frame = Variable_frame + Genera_Trama_Vendedor(DA);
                            reg_leido++;

                            if (reg_leido > 4)
                            {
                                reg_envio = reg_envio + reg_leido;

                                Msj_recibido = Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, nbascula, ngrupo, "GV");
                                if (Msj_recibido != null)
                                {
                                    Crear_Vendedor_detalle(nbascula, ngrupo, Msj_recibido.Split(chr), false, false);
                                }

                                reg_leido = 0;
                                Variable_frame = "";
                            }
                        }

                        if (Variable_frame.Length > 0 && reg_leido < 4)
                        {
                            reg_envio = reg_envio + reg_leido;
                            Msj_recibido = Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, nbascula, ngrupo, "GV");
                            if (Msj_recibido != null)
                            {
                                Crear_Vendedor_detalle(nbascula, ngrupo, Msj_recibido.Split(chr), false, false);
                            }

                            reg_leido = 0;
                            Variable_frame = "";
                        }
                    }
                }
            }
        }
        private void Enviar_Vendedores(ref SerialPort Cliente_bascula, long nbascula, long ngrupo, string puerto)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msj_recibido;
            string Variable_frame = null;
            string strcomando;
            string[] Dato_enviado = null;
            int reg_leido = 0;
            int reg_envio = 0;
            int reg_total;

            Variable_frame = "";

            vendedorTableAdapter.Fill(baseDeDatosDataSet.Vendedor);
            reg_total = baseDeDatosDataSet.Vendedor.Select("pendiente =" + true).Length;
            sr3.WriteLine("Finalizando los vendedores --> " + System.DateTime.Now.ToLongTimeString());

            foreach (DataRow DP in baseDeDatosDataSet.Vendedor.Rows)
            {
                if (Convert.ToBoolean(DP["pendiente"].ToString()))
                {
                    Variable_frame = Variable_frame + Genera_Trama_Vendedor(DP);
                    reg_leido++;
                    if (reg_leido > 4)
                    {
                        reg_envio = reg_envio + reg_leido;
                        strcomando = "GV" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                        Msj_recibido = Variable_frame;
                        SR.SendCOMSerial(ref Cliente_bascula, strcomando, ref Dato_enviado);
                        if (Dato_enviado != null)
                        {
                            if (Dato_enviado[0].IndexOf("Error") >= 0)
                            {
                                Guardar_Trama_pendiente(nbascula, ngrupo, strcomando);
                                Modifica_Estado_Vendedores(Msj_recibido.Split(chr), false);
                            }
                            if (Dato_enviado[0].IndexOf("Ok") >= 0)
                            {
                                Modifica_Estado_Vendedores(Msj_recibido.Split(chr), true);
                            }
                        }
                        sr3.WriteLine("Enviando => " + Variable_frame + "-->" + System.DateTime.Now.ToLongTimeString());
                        reg_leido = 0;
                        Variable_frame = "";
                    }
                }
            }
            if (Variable_frame.Length > 0 && reg_leido < 4)
            {
                reg_envio = reg_envio + reg_leido;
                strcomando = "GV" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                Msj_recibido = Variable_frame;
                SR.SendCOMSerial(ref Cliente_bascula, strcomando, ref Dato_enviado);
                if (Dato_enviado != null)
                {
                    if (Dato_enviado[0].IndexOf("Error") >= 0)
                    {
                        Guardar_Trama_pendiente(nbascula, ngrupo, strcomando);
                        Modifica_Estado_Vendedores(Msj_recibido.Split(chr), false);
                    }
                    if (Dato_enviado[0].IndexOf("Ok") >= 0)
                    {
                        Modifica_Estado_Vendedores(Msj_recibido.Split(chr), true);
                    }
                }
                sr3.WriteLine("Enviando => " + Variable_frame + "-->" + System.DateTime.Now.ToLongTimeString());
                reg_leido = 0;
                Variable_frame = "";
            }
            sr3.WriteLine("Finalizando los vendedores --> " + System.DateTime.Now.ToLongTimeString());
        }

        public void Enviar_InfoAdicional(string direccionIP, ref Socket Cliente_bascula, long nbascula, long ngrupo)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msj_recibido;
            string Variable_frame = null;
            int reg_leido = 0;
            int reg_envio = 0;

            ingredientesTableAdapter.Fill(baseDeDatosDataSet.Ingredientes);
            ingredetalleTableAdapter.Fill(baseDeDatosDataSet.Ingre_detalle);

            DataRelation InfoAdd_Detalle = baseDeDatosDataSet.Relations["Ingredetalle_Ingredientes"];

            List<int> mycollectionIngredientes = new List<int>();

            Variable_frame = "";
            DataRow[] DR_Selec = baseDeDatosDataSet.Ingredientes.Select("pendiente = " + true);

            if (DR_Selec.Length > 0)
            {
                foreach (DataRow DA in DR_Selec)
                {
                    //foreach (DataRow PR in DA.GetChildRows(InfoAdd_Detalle))
                    //{
                        //if (Convert.ToInt32(PR["id_bascula"].ToString()) == nbascula)
                        //{
                            mycollectionIngredientes.Add(Convert.ToInt32(DA["id_ingrediente"].ToString()));
                        //}
                    //}
                }

                if (mycollectionIngredientes.Count > 0)
                {
                    string sComando = "XX" + (char)9 + (char)10;
                    Msj_recibido = Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");

                    if (Msj_recibido != null)
                    {
                        for (int i = 0; i < mycollectionIngredientes.Count; i++)
                        {
                            DataRow DA = baseDeDatosDataSet.Ingredientes.Rows.Find(mycollectionIngredientes[i]);

                            Variable_frame = Variable_frame + Genera_Trama_InfoAdicional(DA);
                            reg_leido++;

                            if (reg_leido > 4)
                            {
                                reg_envio = reg_envio + reg_leido;

                                Msj_recibido = Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, nbascula, ngrupo, "GI");
                                if (Msj_recibido != null)
                                {
                                    Crear_InfoAdd_detalle(nbascula, ngrupo, Msj_recibido.Split(chr), false, false);
                                }

                                reg_leido = 0;
                                Variable_frame = "";
                            }
                        }

                        if (Variable_frame.Length > 0 && reg_leido < 4)
                        {
                            reg_envio = reg_envio + reg_leido;
                            Msj_recibido = Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, nbascula, ngrupo, "GI");
                            if (Msj_recibido != null)
                            {
                                Crear_InfoAdd_detalle(nbascula, ngrupo, Msj_recibido.Split(chr), false, false);
                            }

                            reg_leido = 0;
                            Variable_frame = "";
                        }
                    }
                }
            }                                                     
        }
        private void Enviar_InfoAdicional(ref SerialPort Cliente_bascula, long nbascula, long ngrupo, string puerto)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msj_recibido;
            string Variable_frame;
            string strcomando;
            string[] Dato_enviado = null;
            int reg_leido = 0;
            int reg_envio = 0;
            int reg_total;

            ingredientesTableAdapter.Fill(baseDeDatosDataSet.Ingredientes);
            ingredetalleTableAdapter.Fill(baseDeDatosDataSet.Ingre_detalle);

            DataRelation InfoAdd_Detalle = baseDeDatosDataSet.Relations["Ingredetalle_Ingredientes"];

            Variable_frame = "";
            reg_total = baseDeDatosDataSet.Ingredientes.Select("pendiente = " + true).Length;
            sr3.WriteLine("Enviando las Info. Adicional --> " + System.DateTime.Now.ToLongTimeString());

            foreach (DataRow DA in baseDeDatosDataSet.Ingredientes.Rows)
            {
                if (Convert.ToBoolean(DA["pendiente"]) == true)
                {
                    Variable_frame = Variable_frame + Genera_Trama_InfoAdicional(DA);
                    reg_leido++;
                    if (reg_leido > 4)
                    {
                        reg_envio = reg_envio + reg_leido;
                        strcomando = "GI" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                        Msj_recibido = Variable_frame;
                        SR.SendCOMSerial(ref Cliente_bascula, strcomando, ref Dato_enviado);
                        if (Dato_enviado != null)
                        {
                            if (Dato_enviado[0].IndexOf("Error") >= 0)
                            {
                               // Crear_InfoAdd_detalle(nbascula, ngrupo, Msj_recibido.Split(chr),true,false);
                                
                                Guardar_Trama_pendiente(nbascula, ngrupo, strcomando);
                            }
                            if (Dato_enviado[0].IndexOf("Ok") >= 0)
                            {
                                Crear_InfoAdd_detalle(nbascula, ngrupo, Msj_recibido.Split(chr), false, true);
                               
                            }
                        }

                        sr3.WriteLine("Enviando => " + Variable_frame + "--> " + System.DateTime.Now.ToLongTimeString());
                        reg_leido = 0;
                        Variable_frame = "";
                    }
                }
            }
            if (Variable_frame.Length > 0 && reg_leido < 4)
            {
                reg_envio = reg_envio + reg_leido;
                strcomando = "GI" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                Msj_recibido = Variable_frame;
                SR.SendCOMSerial(ref Cliente_bascula, strcomando, ref Dato_enviado);
                if (Dato_enviado != null)
                {
                    if (Dato_enviado[0].IndexOf("Error") >= 0)
                    {
                                              
                        Guardar_Trama_pendiente(nbascula, ngrupo, strcomando);
                    }
                    if (Dato_enviado[0].IndexOf("Ok") >= 0)
                    {
                        Crear_InfoAdd_detalle(nbascula, ngrupo, Msj_recibido.Split(chr), false, true);
                        Modifica_Estado_Ingredientes(Variable_frame.Split(chr), true);
                    }
                }

                sr3.WriteLine("Enviando => " + Variable_frame + "-->" + System.DateTime.Now.ToLongTimeString());
                reg_leido = 0;
                Variable_frame = "";
            }

            sr3.WriteLine("Finalizando las Info. Adicional --> " + System.DateTime.Now.ToLongTimeString());
        }

        public void Envia_Ofertas(string direccionIP, ref Socket Cliente_bascula, long nbascula, long ngrupo)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msj_recibido;
            string Variable_frame = null;
            int reg_leido = 0;
            int reg_envio = 0;

            ofertaTableAdapter.Fill(baseDeDatosDataSet.Oferta);
            oferta_DetalleTableAdapter.Fill(baseDeDatosDataSet.Oferta_Detalle);

            DataRelation OfertaAdd_Detalle = baseDeDatosDataSet.Relations["OfertaOferta_Detalle"];

            List<int> mycollectionOferta = new List<int>();

            Variable_frame = "";
            DataRow[] DR_Selec = baseDeDatosDataSet.Oferta.Select("pendiente = " + true);

            if (DR_Selec.Length > 0)
            {
                foreach (DataRow DA in DR_Selec)
                {
                    //foreach (DataRow PR in DA.GetChildRows(OfertaAdd_Detalle))
                    //{
                        //if (Convert.ToInt32(PR["id_bascula"].ToString()) == nbascula)
                        //{
                            mycollectionOferta.Add(Convert.ToInt32(DA["id_oferta"].ToString()));
                        //}
                    //}
                }

                if (mycollectionOferta.Count > 0)
                {
                    string sComando = "XX" + (char)9 + (char)10;
                    Msj_recibido = Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");

                    if (Msj_recibido != null)
                    {
                        for (int i = 0; i < mycollectionOferta.Count; i++)
                        {
                            DataRow DA = baseDeDatosDataSet.Oferta.Rows.Find(mycollectionOferta[i]);

                            Variable_frame = Variable_frame + Genera_Trama_Oferta(DA, "");
                            reg_leido++;

                            if (reg_leido > 4)
                            {
                                reg_envio = reg_envio + reg_leido;

                                Msj_recibido = Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, nbascula, ngrupo, "GO");
                                if (Msj_recibido != null)
                                {
                                    Crear_Oferta_Detalle(nbascula, ngrupo, Msj_recibido.Split(chr), false, false);
                                }

                                reg_leido = 0;
                                Variable_frame = "";
                            }
                        }

                        if (Variable_frame.Length > 0 && reg_leido < 4)
                        {
                            reg_envio = reg_envio + reg_leido;
                            Msj_recibido = Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, nbascula, ngrupo, "GO");
                            if (Msj_recibido != null)
                            {
                                Crear_Oferta_Detalle(nbascula, ngrupo, Msj_recibido.Split(chr), false, false);
                            }

                            reg_leido = 0;
                            Variable_frame = "";
                        }
                    }
                }
            }
        }
        private void Envia_Ofertas(ref SerialPort Cliente_bascula, long nbascula, long ngrupo, string puerto)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msj_recibido;
            string Variable_frame = null;
            string strcomando;
            string[] Dato_enviado = null;

            int reg_leido = 0;
            int reg_envio = 0;
            int reg_total;
            ofertaTableAdapter.Fill(baseDeDatosDataSet.Oferta);
            oferta_DetalleTableAdapter.Fill(baseDeDatosDataSet.Oferta_Detalle);

            DataRelation Ofer_Detalle = baseDeDatosDataSet.Relations["OfertaOferta_Detalle"];

            Variable_frame = "";
            reg_total = baseDeDatosDataSet.Oferta.Select("pendiente = " + true).Length;
            sr3.WriteLine("Finalizando los vendedores --> " + System.DateTime.Now.ToLongTimeString());

            foreach (DataRow DA in baseDeDatosDataSet.Oferta.Rows)
            {
                if (Convert.ToBoolean(DA["pendiente"]) == true)
                {
                    Variable_frame = Variable_frame + Genera_Trama_Oferta(DA, ngrupo.ToString());
                    reg_leido++;
                    if (reg_leido > 4)
                    {
                        reg_envio = reg_envio + reg_leido;
                        strcomando = "GO" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                        Msj_recibido = Variable_frame;
                        SR.SendCOMSerial(ref Cliente_bascula, strcomando, ref Dato_enviado);
                        if (Dato_enviado != null)
                        {
                            if (Dato_enviado[0].IndexOf("Ok") >= 0)
                            {
                                Crear_Oferta_Detalle(nbascula, ngrupo, Msj_recibido.Split(chr),false,true);                               
                            }
                            if (Dato_enviado[0].IndexOf("Error") >= 0)
                            {            
                                Guardar_Trama_pendiente(nbascula, ngrupo, strcomando);
                            }
                        }
                        sr3.WriteLine("Enviando -> " + Variable_frame + "-->" + System.DateTime.Now.ToLongTimeString());
                        reg_leido = 0;
                        Variable_frame = "";
                    }
                }
            }
            if (Variable_frame.Length > 0 && reg_leido < 4)
            {
                reg_envio = reg_envio + reg_leido;
                strcomando = "GO" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                Msj_recibido = Variable_frame;
                SR.SendCOMSerial(ref Cliente_bascula, strcomando, ref Dato_enviado);
                if (Dato_enviado != null)
                {
                    if (Dato_enviado[0].IndexOf("Ok") >= 0)
                    {
                        Crear_Oferta_Detalle(nbascula, ngrupo, Msj_recibido.Split(chr),false,true);

                    }
                    if (Dato_enviado[0].IndexOf("Error") >= 0)
                    {


                        Guardar_Trama_pendiente(nbascula, ngrupo, strcomando);
                    }
                }

                sr3.WriteLine("Enviando => " + Variable_frame + "-->" + System.DateTime.Now.ToLongTimeString());
                reg_leido = 0;
                Variable_frame = "";
            }

            sr3.WriteLine("Finalizando las Ofertas --> " + System.DateTime.Now.ToLongTimeString());
        }

        private void Envia_Imagenes(ref SerialPort Cliente_bascula, long nbascula, long ngrupo, string puerto)
        {
            int iRtaFunct = 0;
            string ImagenAEnviar = "";
            
            int reg_total;
            CommandTorrey myobj = new CommandTorrey();

            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);
            prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);

            DataRelation Producto_Detalle = baseDeDatosDataSet.Relations["ProductosProd_detalle"];

            reg_total = baseDeDatosDataSet.Productos.Select("imagen = " + true).Length;
            sr3.WriteLine("Enviando Imagenes --> " + System.DateTime.Now.ToLongTimeString());

            if (reg_total > 0)
            {
                foreach (DataRow DA in baseDeDatosDataSet.Productos.Rows)
                {
                    foreach (DataRow PR in DA.GetChildRows(Producto_Detalle))
                    {
                        if (Convert.ToInt32(PR["id_bascula"].ToString()) == nbascula && Convert.ToInt32(PR["id_grupo"].ToString()) == ngrupo && Convert.ToBoolean(DA["pendiente"]) == true)
                        {
                            int posicion = DA["imagen1"].ToString().LastIndexOf('\\');
                            if (posicion > 0)
                            {

                                if (Convert.ToBoolean(DA["imagen"]) == true)
                                {
                                    ImagenAEnviar = DA["imagen1"].ToString();
                                    iRtaFunct = myobj.TORREYSendImagesToScaleSerial(puerto, ImagenAEnviar, "Product");
                                    if (iRtaFunct > 0)
                                        sr3.WriteLine("Error en la imagen " + ImagenAEnviar + "--> " + System.DateTime.Now.ToLongTimeString());
                                    else
                                        sr3.WriteLine("Imagen Enviada " + ImagenAEnviar + "--> " + System.DateTime.Now.ToLongTimeString());
                                }
                            }
                        }
                    }
                }
            }
            sr3.WriteLine("Finalizando las Imagenes --> " + System.DateTime.Now.ToLongTimeString());
        }

        #endregion

        #region GENERACION DE TRAMAS PARA ENVIAR
        /// <summary>
        /// GENERA TRAMA DE PRECIO.
        /// </summary>
        /// <param name="DA"></param>
        /// <returns></returns>
        public string Genera_Trama_Precio(DataRow DA)
        {
            string sVarieble_Frame = "";
            System.Globalization.NumberFormatInfo frt = new System.Globalization.NumberFormatInfo();
            frt.NumberDecimalDigits = Variable.n_decimal;
            frt.NumberDecimalSeparator = Variable.c_decimal.ToString();

            decimal f_precio = Convert.ToDecimal(DA["precio"].ToString(), frt);

            sVarieble_Frame = sVarieble_Frame + "0" + (char)9;  // DA["id_producto"].ToString() + (char)9;   //id_producto
            sVarieble_Frame = sVarieble_Frame + DA["id_producto"].ToString() + (char)9; // id_producto
            sVarieble_Frame = sVarieble_Frame + string.Format(System.Globalization.CultureInfo.CreateSpecificCulture(Variable.idioma2), Variable.F_Decimal, f_precio) + (char)9;  //precio
            return sVarieble_Frame + (char)10;
        }
        /// <summary>
        /// GENERA TRAMA DE PRODUCTOS
        /// </summary>
        /// <param name="PR"></param>
        /// <param name="DA"></param>
        /// <returns></returns>
        public string Genera_Trama_Producto(DataRow DA)
        {
            int pos_imagen;
            string Nombre_imagen = "";
            string fecha_basc;
            string hora_basc;
            string Variable_frame = null;

            System.Globalization.NumberFormatInfo frt = new System.Globalization.NumberFormatInfo();
            frt.NumberDecimalDigits = Variable.n_decimal;
            frt.NumberDecimalSeparator = Variable.c_decimal.ToString();

            string[] fecha_bascula = DA["actualizado"].ToString().Split(' ');
            if (fecha_bascula.Length > 2)
            {
                fecha_basc = string.Format(Variable.F_Fecha, Convert.ToDateTime(fecha_bascula[2]));
                hora_basc = string.Format(Variable.F_Hora, Convert.ToDateTime(fecha_bascula[0] + fecha_bascula[1]));
            }
            else
            {
                fecha_basc = string.Format(Variable.F_Fecha, Convert.ToDateTime(fecha_bascula[1]));
                hora_basc = string.Format(Variable.F_Hora, Convert.ToDateTime(fecha_bascula[0]));
            }
            pos_imagen = DA["imagen1"].ToString().LastIndexOf('\\');
            if (pos_imagen > 0)
            { Nombre_imagen = Variable.Ruta_SDCard + DA["imagen1"].ToString().Substring(pos_imagen + 1); }
            else Nombre_imagen = DA["imagen1"].ToString();

            decimal f_precio = Convert.ToDecimal(DA["precio"].ToString(), frt);
            Double impuesto_100 = Convert.ToDouble(DA["Impuesto"].ToString()) / 100;

            Variable_frame = Variable_frame + DA["id_producto"].ToString() + (char)9;   //0 id_producto
            Variable_frame = Variable_frame + DA["codigo"].ToString() + (char)9; //1 codigo
            Variable_frame = Variable_frame + DA["NoPlu"].ToString() + (char)9; //2 numero de PLUs
            Variable_frame = Variable_frame + Variable.validar_salida(DA["Nombre"].ToString().Trim(), 2) + (char)9; //3 Nombre
            Variable_frame = Variable_frame + string.Format(System.Globalization.CultureInfo.CreateSpecificCulture(Variable.idioma2), Variable.F_Decimal, f_precio) + (char)9;  //4 precio
            Variable_frame = Variable_frame + Nombre_imagen + (char)9;  //5 imagen
            Variable_frame = Variable_frame + DA["TipoId"].ToString() + (char)9;  //6 tipo_id
            Variable_frame = Variable_frame + DA["PrecioEditable"].ToString() + (char)9;   //7 precioeditable
            Variable_frame = Variable_frame + DA["CaducidadDias"].ToString() + (char)9; //8 caducidad
            Variable_frame = Variable_frame + impuesto_100.ToString() + (char)9;  //// DA["Impuesto"].ToString() + (char)9;    //9 impuesto                       
            Variable_frame = Variable_frame + DA["id_info_nutri"].ToString() + (char)9;   //10 info nutricional
            Variable_frame = Variable_frame + DA["id_ingrediente"].ToString() + (char)9;   //11 info adicional
            Variable_frame = Variable_frame + fecha_basc + " " + hora_basc + (char)9; //12 actualizacion
            Variable_frame = Variable_frame + DA["tara"] + (char)9;   //13 tara
            Variable_frame = Variable_frame + DA["Mutiplo"].ToString() + (char)9;   //14 multiplo
            Variable_frame = Variable_frame + DA["publicidad1"].ToString() + (char)9;   //15 publicidad1
            Variable_frame = Variable_frame + DA["publicidad2"].ToString() + (char)9;   //16 publicidad2
            Variable_frame = Variable_frame + DA["publicidad3"].ToString() + (char)9;   //17 publicidad3
            Variable_frame = Variable_frame + DA["publicidad4"].ToString() + (char)9;   //18 publicidad4
            Variable_frame = Variable_frame + DA["oferta"].ToString() + (char)9;   //19 Oferta      
            return (Variable_frame + (char)10);
        }

        public string Genera_Trama_RImagen(DataRow DA)
        {
            string Nombre_imagen = "";
            string Variable_frame = "";

            Nombre_imagen = Variable.Ruta_SDCard + Path.GetFileName(DA["imagen1"].ToString());

            Variable_frame = Variable_frame + DA["id_producto"].ToString() + (char)9;   
            Variable_frame = Variable_frame + Nombre_imagen + (char)9;  
            return (Variable_frame + (char)10);
        }
        /// <summary>
        /// GENERA TRAMA DE PUBLICIDAD
        /// </summary>
        /// <param name="DPubli"></param>
        /// <returns></returns>
        public string Genera_Trama_Publicidad(DataRow DPubli)
        {
            string sVarieble_Frame = null;
            sVarieble_Frame = sVarieble_Frame + DPubli["id_publicidad"].ToString() + (char)9; // id_publicidad
            sVarieble_Frame = sVarieble_Frame + Variable.validar_salida(DPubli["Titulo"].ToString(), 2) + (char)9; //Titulo
            sVarieble_Frame = sVarieble_Frame + Variable.validar_salida(DPubli["Mensaje"].ToString(), 2) + (char)9; //Mensaje
            return sVarieble_Frame + (char)10;
        }
        /// <summary>
        /// GENERA TRAMA DE OFERTA
        /// </summary>
        /// <param name="PR"></param>
        /// <param name="DA"></param>
        /// <param name="ngrupo"></param>
        /// <returns></returns>
        public string Genera_Trama_Oferta(DataRow DA, string ngrupo)  //DataRow PR, DataRow DA, string ngrupo)
        {
            string sVarieble_Frame = null;
            string fecha_inicio = "";
            string fecha_final = "";
            string hora_inicio = "";
            string hora_final = "";

            string[] fecha_inicio_bascula = DA["fecha_inicio"].ToString().Split(' ');
            if (fecha_inicio_bascula.Length > 2)
            {
                fecha_inicio= string.Format(Variable.F_Fecha, Convert.ToDateTime(fecha_inicio_bascula[0]));
                hora_inicio = string.Format(Variable.F_Hora, Convert.ToDateTime(fecha_inicio_bascula[1] + fecha_inicio_bascula[2]));
            }
            else
            {
                fecha_inicio= string.Format(Variable.F_Fecha, Convert.ToDateTime(fecha_inicio_bascula[0]));
                hora_inicio = string.Format(Variable.F_Hora, Convert.ToDateTime(fecha_inicio_bascula[1]));
            }
            string[] fecha_final_bascula = DA["fecha_fin"].ToString().Split(' ');
            if (fecha_final_bascula.Length > 2)
            {
                fecha_final = string.Format(Variable.F_Fecha, Convert.ToDateTime(fecha_final_bascula[0]));
                hora_final = string.Format(Variable.F_Hora, Convert.ToDateTime(fecha_final_bascula[1] + fecha_final_bascula[2]));
            }
            else
            {
                fecha_final = string.Format(Variable.F_Fecha, Convert.ToDateTime(fecha_final_bascula[0]));
                hora_final = string.Format(Variable.F_Hora, Convert.ToDateTime(fecha_final_bascula[1]));
            }
            sVarieble_Frame = sVarieble_Frame + DA["id_oferta"].ToString() + (char)9; // codigo
            sVarieble_Frame = sVarieble_Frame + Variable.validar_salida(DA["nombre"].ToString(), 2) + (char)9; //Nombre
            sVarieble_Frame = sVarieble_Frame + fecha_inicio +  " " + hora_inicio + (char)9;  // Fecha de inicio
            sVarieble_Frame = sVarieble_Frame + fecha_final +  " " + hora_final + (char)9;   // fecha termino
            sVarieble_Frame = sVarieble_Frame + DA["tipo_desc"].ToString() + (char)9; // tipo de descuento
            sVarieble_Frame = sVarieble_Frame + DA["Descuento"].ToString() + (char)9;    // descuento                     
            sVarieble_Frame = sVarieble_Frame + DA["nVentas"].ToString() + (char)9; // numero de ventas
            return sVarieble_Frame + (char)10;
        }
        /// <summary>
        /// GENERA TRAMA DE VENDEDOR
        /// </summary>
        /// <param name="DA"></param>
        /// <returns></returns>
        public string Genera_Trama_Vendedor(DataRow DA)
        {
            int msj, meta;
            string sVarieble_Frame = null;

            if (Convert.ToBoolean(DA["Msj_Enable"].ToString())) msj = 1;
            else msj = 0;

            if (Convert.ToBoolean(DA["Meta_Enable"].ToString())) meta = 1;
            else meta = 0;

            sVarieble_Frame = sVarieble_Frame + DA["id_vendedor"].ToString() + (char)9; // codigo
            sVarieble_Frame = sVarieble_Frame + Variable.validar_salida(DA["Nombre"].ToString(), 2) + (char)9; //Nombre
            sVarieble_Frame = sVarieble_Frame + msj.ToString() + (char)9;   //Msj_Enable
            sVarieble_Frame = sVarieble_Frame + meta.ToString() + (char)9;  // Meta_Enable
            sVarieble_Frame = sVarieble_Frame + DA["Meta_Ventas"].ToString() + (char)9;   // Meta_Ventas
            sVarieble_Frame = sVarieble_Frame + DA["publicidad1"].ToString() + (char)9; // publicidad
            sVarieble_Frame = sVarieble_Frame + DA["publicidad2"].ToString() + (char)9;    // publicidad
            return sVarieble_Frame + (char)10;
        }
        /// <summary>
        /// GENERA TRAMA DE INFORMACION ADICIONAL
        /// </summary>
        /// <param name="DA"></param>
        /// <returns></returns>
        public string Genera_Trama_InfoAdicional(DataRow DA)
        {
            string sVarieble_Frame = null;
            sVarieble_Frame = sVarieble_Frame + DA["id_ingrediente"].ToString() + (char)9; // id_ingredinte
            sVarieble_Frame = sVarieble_Frame + Variable.validar_salida(DA["Nombre"].ToString(), 2) + (char)9; //Nombre
            sVarieble_Frame = sVarieble_Frame + Variable.validar_salida(DA["Informacion"].ToString(), 2) + (char)9;  //informacion               
            return sVarieble_Frame + (char)10;
        }
        /// <summary>
        /// GENERA TRAMA DE CARPETAS
        /// </summary>
        /// <param name="DR_Folder"></param>
        /// <returns></returns>
        public string Genera_Trama_Carpeta(DataRow DR_Folder)
        {
            string RutaCarpeta, Nombre_imagen;
            int pos_imagen;
            string sVarieble_Frame = null;

            if (DR_Folder["ruta"].ToString().StartsWith("//"))
                RutaCarpeta = DR_Folder["ruta"].ToString().Substring(1) + "/";
            else
                RutaCarpeta = DR_Folder["ruta"].ToString();

            pos_imagen = DR_Folder["imagen"].ToString().LastIndexOf('\\');
            if (pos_imagen > 0)
            { Nombre_imagen = Variable.Ruta_SDCard + DR_Folder["imagen"].ToString().Substring(pos_imagen + 1); }
            else Nombre_imagen = DR_Folder["imagen"].ToString();

            sVarieble_Frame = sVarieble_Frame + DR_Folder["ID"].ToString() + (char)9; // ID
            sVarieble_Frame = sVarieble_Frame + Variable.validar_salida(DR_Folder["Nombre"].ToString(), 2) + (char)9; //Nombre
            sVarieble_Frame = sVarieble_Frame + Nombre_imagen + (char)9; //Imagen
            sVarieble_Frame = sVarieble_Frame + RutaCarpeta + (char)9; //Ruta
            return sVarieble_Frame + (char)10;
        }
        /// <summary>
        /// GENER TRAMA DE LAS ASOCIACIONES DE LAS CARPETAS
        /// </summary>
        /// <param name="DR_Detail"></param>
        /// <returns></returns>
        public string Genera_Trama_Carpeta_Detalle(DataRow DR_Detail)
        {
            string sVarieble_Frame = null;
            sVarieble_Frame = sVarieble_Frame + DR_Detail["ID"].ToString() + (char)9; // ID
            sVarieble_Frame = sVarieble_Frame + DR_Detail["ID_padre"].ToString() + (char)9; //ID_padre
            sVarieble_Frame = sVarieble_Frame + DR_Detail["posicion"] + (char)9;  //posicion
            sVarieble_Frame = sVarieble_Frame + DR_Detail["tabla"] + (char)9;  //table

            return sVarieble_Frame + (char)10;
        }
        /// <summary>
        /// GENERA TRAMA DE LAS ASOCIACIONES DE PRODUCTOS
        /// </summary>
        /// <param name="DR_Detail"></param>
        /// <returns></returns>
        public string Genera_Trama_Producto_Detalle(DataRow DR_Detail)
        {
            string sVarieble_Frame = null;
            sVarieble_Frame = sVarieble_Frame + DR_Detail["id_producto"].ToString() + (char)9; // ID
            sVarieble_Frame = sVarieble_Frame + DR_Detail["id_carpeta"].ToString() + (char)9; //ID_padre
            sVarieble_Frame = sVarieble_Frame + DR_Detail["posicion"].ToString() + (char)9;  //posicion
            sVarieble_Frame = sVarieble_Frame + "P" + (char)9;  //table

            return sVarieble_Frame + (char)10;
        }
        public string Genera_Trama_Producto_Detalle(long iIdProducto, long iIdCarpeta, int iPosicion, string sTable)
        {
            string sVarieble_Frame = null;
            sVarieble_Frame = sVarieble_Frame + iIdProducto + (char)9; // ID
            sVarieble_Frame = sVarieble_Frame + iIdCarpeta + (char)9; //ID_padre
            sVarieble_Frame = sVarieble_Frame + iPosicion + (char)9;  //posicion
            sVarieble_Frame = sVarieble_Frame + sTable + (char)9;  //table

            return sVarieble_Frame + (char)10;
        }

        public string Genera_Trama_Producto_Detalle_Eliminado(DataRow DR_Detail)
        {
            string sVarieble_Frame = null;
            sVarieble_Frame = sVarieble_Frame + (char)9;
            sVarieble_Frame = sVarieble_Frame + DR_Detail["id_producto"].ToString() + (char)9; // ID
            sVarieble_Frame = sVarieble_Frame + DR_Detail["id_carpeta"].ToString() + (char)9; //ID_padre            
            sVarieble_Frame = sVarieble_Frame + "P" + (char)9;  //table

            return sVarieble_Frame + (char)10;
        }
        /// <summary>
        /// GENERA TRAMA DE CONFIGURACION GENERAL PARA LA BASCULA
        /// </summary>
        /// <param name="DA"></param>
        /// <returns></returns>
        public string Genera_Trama_Config_General(DataRow DA)
        {
            string sVarieble_Frame = null;

            string NomSplash1 = "", NomSplash2 = "", NomSplash3 = "", NomSplash4 = "", NomSplash5 = "";
            int pos_imagen;
            pos_imagen = DA["splash1"].ToString().LastIndexOf('\\');
            if (pos_imagen > 0)
            { NomSplash1 = Variable.Ruta_SPlash + DA["splash1"].ToString().Substring(pos_imagen + 1); }
            else NomSplash1 = DA["splash1"].ToString();

            pos_imagen = DA["splash2"].ToString().LastIndexOf('\\');
            if (pos_imagen > 0)
            { NomSplash2 = Variable.Ruta_SPlash + DA["splash2"].ToString().Substring(pos_imagen + 1); }
            else NomSplash2 = DA["splash2"].ToString();

            pos_imagen = DA["splash3"].ToString().LastIndexOf('\\');
            if (pos_imagen > 0)
            { NomSplash3 = Variable.Ruta_SPlash + DA["splash3"].ToString().Substring(pos_imagen + 1); }
            else NomSplash3 = DA["splash3"].ToString();

            pos_imagen = DA["splash4"].ToString().LastIndexOf('\\');
            if (pos_imagen > 0)
            { NomSplash4 = Variable.Ruta_SPlash + DA["splash4"].ToString().Substring(pos_imagen + 1); }
            else NomSplash4 = DA["splash4"].ToString();

            pos_imagen = DA["splash5"].ToString().LastIndexOf('\\');
            if (pos_imagen > 0)
            { NomSplash5 = Variable.Ruta_SPlash + DA["splash5"].ToString().Substring(pos_imagen + 1); }
            else NomSplash5 = DA["splash5"].ToString();

            sVarieble_Frame = sVarieble_Frame + DA["publicidad1"].ToString().Trim() + (char)9; // publicidad1
            sVarieble_Frame = sVarieble_Frame + DA["publicidad2"].ToString().Trim() + (char)9; // publicidad2
            sVarieble_Frame = sVarieble_Frame + DA["publicidad3"].ToString().Trim() + (char)9; // publicidad3
            sVarieble_Frame = sVarieble_Frame + DA["publicidad4"].ToString().Trim() + (char)9; // publicidad4
            sVarieble_Frame = sVarieble_Frame + DA["publicidad5"].ToString().Trim() + (char)9; // publicidad5
            sVarieble_Frame = sVarieble_Frame + NomSplash1 + (char)9;  //ruta de imagen splash1
            sVarieble_Frame = sVarieble_Frame + NomSplash2 + (char)9;  //ruta de imagen splash2
            sVarieble_Frame = sVarieble_Frame + NomSplash3 + (char)9;  //ruta de imagen splash3
            sVarieble_Frame = sVarieble_Frame + NomSplash4 + (char)9;  //ruta de imagen splash4
            sVarieble_Frame = sVarieble_Frame + NomSplash5 + (char)9;  //ruta de imagen splash5
            sVarieble_Frame = sVarieble_Frame + DA["pss_supervisor"].ToString() + (char)9;  //password administrador
            sVarieble_Frame = sVarieble_Frame + DA["pss_operario"].ToString() + (char)9;  //password supervisor
            sVarieble_Frame = sVarieble_Frame + DA["u_varios"].ToString() + (char)9; // varios
            sVarieble_Frame = sVarieble_Frame + DA["p_descuentos"].ToString() + (char)9;  //descuento
            sVarieble_Frame = sVarieble_Frame + DA["p_devoluciones"].ToString() + (char)9;  //devolucion
            sVarieble_Frame = sVarieble_Frame + DA["bloq_precio"].ToString() + (char)9;  //bloqueo de precio
            sVarieble_Frame = sVarieble_Frame + DA["autoprint"].ToString() + (char)9;  //auto impresion
            sVarieble_Frame = sVarieble_Frame + DA["reprint"].ToString() + (char)9;  //re impresion
            sVarieble_Frame = sVarieble_Frame + DA["formatofecha"].ToString() + (char)9;  //formato fecha
            sVarieble_Frame = sVarieble_Frame + DA["formatohora"].ToString() + (char)9;  //formato hora           
            sVarieble_Frame = sVarieble_Frame + DA["activasplash"].ToString() + (char)9;  //activa las splash
            sVarieble_Frame = sVarieble_Frame + DA["tiemposplash"].ToString() + (char)9;  //tiempo de las splash
            return sVarieble_Frame + (char)10;
        }
        /// <summary>
        /// GENERA TRAMA DE CONFIGURACION GENERAL 2 PARA LA BASCULA
        /// </summary>
        /// <param name="DA"></param>
        /// <returns></returns>
        public string Genera_Trama_Config_General2(DataRow DA)
        {
            string sVarieble_Frame = null;

            string NomPubli1 = "", NomPubli2 = "", NomPubli3 = "", NomPubli4 = "", NomPubli5 = "", NomLogo = "", NomLogoPrint = "";
            int pos_imagen;
            pos_imagen = DA["imgpubli1"].ToString().LastIndexOf('\\');
            if (pos_imagen > 0)
            { NomPubli1 = Variable.Ruta_Ads + DA["imgpubli1"].ToString().Substring(pos_imagen + 1); }
            else NomPubli1 = DA["imgpubli1"].ToString();

            pos_imagen = DA["imgpubli2"].ToString().LastIndexOf('\\');
            if (pos_imagen > 0)
            { NomPubli2 = Variable.Ruta_Ads + DA["imgpubli2"].ToString().Substring(pos_imagen + 1); }
            else NomPubli2 = DA["imgpubli2"].ToString();

            pos_imagen = DA["imgpubli3"].ToString().LastIndexOf('\\');
            if (pos_imagen > 0)
            { NomPubli3 = Variable.Ruta_Ads + DA["imgpubli3"].ToString().Substring(pos_imagen + 1); }
            else NomPubli3 = DA["imgpubli3"].ToString();

            pos_imagen = DA["imgpubli4"].ToString().LastIndexOf('\\');
            if (pos_imagen > 0)
            { NomPubli4 = Variable.Ruta_Ads + DA["imgpubli4"].ToString().Substring(pos_imagen + 1); }
            else NomPubli4 = DA["imgpubli4"].ToString();

            pos_imagen = DA["imgpubli5"].ToString().LastIndexOf('\\');
            if (pos_imagen > 0)
            { NomPubli5 = Variable.Ruta_Ads + DA["imgpubli5"].ToString().Substring(pos_imagen + 1); }
            else NomPubli5 = DA["imgpubli5"].ToString();

            pos_imagen = DA["logo"].ToString().LastIndexOf('\\');
            if (pos_imagen > 0)
            { NomLogo = Variable.Ruta_Logos + DA["logo"].ToString().Substring(pos_imagen + 1); }
            else NomLogo = DA["logo"].ToString();

            pos_imagen = DA["logoprint"].ToString().LastIndexOf('\\');
            if (pos_imagen > 0)
            { NomLogoPrint = Variable.Ruta_Logos + DA["logoprint"].ToString().Substring(pos_imagen + 1); }
            else NomLogoPrint = DA["logoprint"].ToString();

            sVarieble_Frame = sVarieble_Frame + NomPubli1 + (char)9;  //ruta de imagen ad1
            sVarieble_Frame = sVarieble_Frame + NomPubli2 + (char)9;  //ruta de imagen ad2
            sVarieble_Frame = sVarieble_Frame + NomPubli3 + (char)9;  //ruta de imagen ad3
            sVarieble_Frame = sVarieble_Frame + NomPubli4 + (char)9;  //ruta de imagen ad4
            sVarieble_Frame = sVarieble_Frame + NomPubli5 + (char)9;  //ruta de imagen ad5
            sVarieble_Frame = sVarieble_Frame + NomLogo + (char)9;  //ruta de imagen logo
            sVarieble_Frame = sVarieble_Frame + NomLogoPrint + (char)9;  //ruta de imagen logo print
            sVarieble_Frame = sVarieble_Frame + DA["tiempopubli"].ToString() + (char)9; // activa el logo
            sVarieble_Frame = sVarieble_Frame + DA["activapubli"].ToString() + (char)9;  //activa publicidad
            sVarieble_Frame = sVarieble_Frame + DA["activalogo"].ToString() + (char)9;  //tiempo de publicidad
            return sVarieble_Frame + (char)10;
        }
        /// <summary>
        /// GENERA TRAMA DE CONFIGURACION PARA EL IMPRESOR DE LA BASCULA
        /// </summary>
        /// <param name="DA"></param>
        /// <returns></returns>
        public string Genera_Trama_Config_Impresor(DataRow DA)
        {
            string sVarieble_Frame = null;

            sVarieble_Frame = sVarieble_Frame + DA["tipoimp"].ToString() + (char)9; // medio de impresion           
            sVarieble_Frame = sVarieble_Frame + DA["formato_etiq"].ToString() + (char)9; // formato de etiqueta
            sVarieble_Frame = sVarieble_Frame + DA["formato_papel"].ToString() + (char)9; // formato de papel   
            sVarieble_Frame = sVarieble_Frame + DA["f_personalizado_etiq"] + (char)9;  //numero de formato para etiqueta 
            sVarieble_Frame = sVarieble_Frame + DA["f_personalizado_papel"] + (char)9;  //numero de formato para papel o ticket
            sVarieble_Frame = sVarieble_Frame + DA["c_etiqueta"].ToString() + (char)9;  //contraste etiqueta
            sVarieble_Frame = sVarieble_Frame + DA["c_papel"].ToString() + (char)9;  //contraste papel
            sVarieble_Frame = sVarieble_Frame + DA["EAN_etiq"].ToString() + (char)9; // formato codigo barra por ticket o papel  ean13_etiq
            sVarieble_Frame = sVarieble_Frame + DA["barcode_prod"].ToString() + (char)9; // formato codigo barra por ticket o papel  ean13_etiq
            sVarieble_Frame = sVarieble_Frame + DA["barcode_personal_prod"].ToString() + (char)9;  //odiga de barra personalizado por producto
            sVarieble_Frame = sVarieble_Frame + DA["EAN_papel"].ToString() + (char)9; // formato codigo barra por ticket o papel  ean13_etiq
            sVarieble_Frame = sVarieble_Frame + DA["barcode_ticket"].ToString() + (char)9; //formato codigo barra por producto
            sVarieble_Frame = sVarieble_Frame + DA["barcode_personal_ticket"].ToString() + (char)9; // codiga de barra personalizado por ticket o papel            
            sVarieble_Frame = sVarieble_Frame + DA["prefijo"].ToString() + (char)9;  //prefijo
            sVarieble_Frame = sVarieble_Frame + DA["departamento"].ToString() + (char)9;  //departamento
            sVarieble_Frame = sVarieble_Frame + DA["retardo"] + (char)9;  //retardo de impresion
            sVarieble_Frame = sVarieble_Frame + DA["nutrientes"] + (char)9;  //activar impresion de nutrientes
            sVarieble_Frame = sVarieble_Frame + DA["cero_pieza"].ToString() + (char)9;  //cero piezas o corrimiento

            return sVarieble_Frame + (char)10;
        }
        /// <summary>
        /// GENERA TRAMA DE LA CONFIGURACION DEL ENCABEZADO DE LAS BASCULAS
        /// </summary>
        /// <param name="DA"></param>
        /// <returns></returns>
        public string Genera_Trama_Config_Encabezado(DataRow DA)
        {
            string sVarieble_Frame = null;

            sVarieble_Frame = sVarieble_Frame + DA["id_encabezado"].ToString() + (char)9; // id encabezado 
            sVarieble_Frame = sVarieble_Frame + DA["encabezado"].ToString() + (char)9; // descripcion de encabezado
            sVarieble_Frame = sVarieble_Frame + DA["align"].ToString() + (char)9; // alineacion
            sVarieble_Frame = sVarieble_Frame + DA["letra"].ToString() + (char)9; // font o tamaño de letra

            return sVarieble_Frame + (char)10;
        }
        /// <summary>
        /// GENERA TRAMA DE LA CONFIGURACION DE LOS FORMATOS
        /// </summary>
        /// <param name="DA"></param>
        /// <returns></returns>
        public string Genera_Trama_Config_Formato(DataRow DA)
        {
            string sVarieble_Frame = null;

            sVarieble_Frame = sVarieble_Frame + DA["id_formato"].ToString() + (char)9; // id formato 
            sVarieble_Frame = sVarieble_Frame + DA["posdef"].ToString() + (char)9; // descripcion del campo
            sVarieble_Frame = sVarieble_Frame + DA["possize"].ToString() + (char)9; // tamaño del campo
            sVarieble_Frame = sVarieble_Frame + DA["largo"].ToString() + (char)9; // largo del ticket o papel
            sVarieble_Frame = sVarieble_Frame + DA["ancho"].ToString() + (char)9; // ancho del ticket o papel
            sVarieble_Frame = sVarieble_Frame + DA["separacion"].ToString() + (char)9; // separacion del papel
            sVarieble_Frame = sVarieble_Frame + DA["Nencabezados"].ToString() + (char)9; // lineas para encabezado
            sVarieble_Frame = sVarieble_Frame + DA["Ningredientes"].ToString() + (char)9; // lineas para ingredientes

            return sVarieble_Frame + (char)10;
        }
        /// <summary>
        /// GENERA TRAMA DE CONFIGURACION DE TEXTO PARA LA BASCULA
        /// </summary>
        /// <param name="DA"></param>
        /// <returns></returns>
        public string Genera_Trama_Config_Texto(DataRow DA)
        {
            string sVarieble_Frame = null;

            sVarieble_Frame = sVarieble_Frame + DA["tx_peso"].ToString() + (char)9; // texto peso
            sVarieble_Frame = sVarieble_Frame + DA["tx_precio"].ToString() + (char)9; // texto precio
            sVarieble_Frame = sVarieble_Frame + DA["tx_total"].ToString() + (char)9; // texto total
            sVarieble_Frame = sVarieble_Frame + DA["tx_vendedor"].ToString() + (char)9; // texto vendedor
            sVarieble_Frame = sVarieble_Frame + DA["tx_mensaje"].ToString() + (char)9; // texto mensaje
            sVarieble_Frame = sVarieble_Frame + DA["tx_talon"].ToString() + (char)9; // texto talon
            sVarieble_Frame = sVarieble_Frame + DA["tx_devolucion"].ToString() + (char)9;  //texto devolucion
            sVarieble_Frame = sVarieble_Frame + DA["tx_descuento"].ToString() + (char)9;  //texto descuento
            sVarieble_Frame = sVarieble_Frame + DA["tx_efectivo"].ToString() + (char)9;  //texto efectivo
            sVarieble_Frame = sVarieble_Frame + DA["tx_cambio"].ToString() + (char)9;  //texto de cambio
            sVarieble_Frame = sVarieble_Frame + DA["tx_fecha"].ToString() + (char)9;  //texto de fecha
            sVarieble_Frame = sVarieble_Frame + DA["tx_textofechacad"].ToString() + (char)9;  //texto de fecha de caducidad
            sVarieble_Frame = sVarieble_Frame + DA["tx_tara"].ToString() + (char)9;  //texto de tara
            sVarieble_Frame = sVarieble_Frame + DA["tx_pesopieza"].ToString() + (char)9;  //texto de peso pieza
            sVarieble_Frame = sVarieble_Frame + DA["tx_preciopieza"].ToString() + (char)9;  //texto de precio pieza
            sVarieble_Frame = sVarieble_Frame + DA["tx_pesoticket"].ToString() + (char)9;  //texto de peso ticket
            sVarieble_Frame = sVarieble_Frame + DA["tx_precioticket"].ToString() + (char)9;  //texto de precio ticket
            
            return sVarieble_Frame + (char)10;
        }
        #endregion

        #region FUNCIONES PARA ENVIO DE COMANDO

        public string Command_Enviado(int reg_leido, string Trama_Enviada, string direccionIP, ref Socket Cliente_bascula, long bascula, long grupo, string comando)
        {
            string[] Dato_Recibido = null;
            string reg_enviado = "", strcomando = " ";
            string Msj_Recibido = "";

            strcomando = comando + reg_leido.ToString().PadLeft(2, '0') + Trama_Enviada;

            Cte.Envio_Dato(ref Cliente_bascula, direccionIP, strcomando, ref Dato_Recibido);
            if (Dato_Recibido != null)
            {
                reg_enviado = strcomando.Substring(4);
                if (Dato_Recibido[0].IndexOf("Error") >= 0)
                {
                    if (bascula > 0 || grupo > 0)
                    {
                        Console.WriteLine("{0}(): Error!", System.Reflection.MethodBase.GetCurrentMethod().Name);
                        Guardar_Trama_pendiente(bascula, grupo, strcomando);
                    }
                    Msj_Recibido = null;
                }
                if (Dato_Recibido[0].IndexOf("Ok") >= 0)
                {
                    Console.WriteLine("{0}(): Ok!", System.Reflection.MethodBase.GetCurrentMethod().Name);
                    Msj_Recibido = reg_enviado;
                }
            }
            else
            {
                Msj_Recibido = null;
                if (bascula > 0 || grupo > 0)
                {
                    Guardar_Trama_pendiente(bascula, grupo, strcomando);
                }
                Console.WriteLine("No respondio la bascula {0}", direccionIP);
            }

            return Msj_Recibido;
        }
        public void Command_Pendiente(int reg_leido, string Trama_Enviada, string direccionIP, ref Socket Cliente_bascula, long bascula, long grupo, Int32 nregistro)
        {
            string[] Dato_Recibido = null;
            string reg_enviado, strcomando = Trama_Enviada;

            Cte.Envio_Dato(ref Cliente_bascula, direccionIP, strcomando, ref Dato_Recibido);
            if (Dato_Recibido != null)
            {
                reg_enviado = strcomando.Substring(4);
                //if (Dato_Recibido[0].IndexOf("Error") >= 0) Guardar_Trama_pendiente(bascula, grupo, strcomando);
                if (Dato_Recibido[0].IndexOf("Ok") >= 0) Borrar_Trama_Pendiente(bascula, grupo, nregistro);
            }
        }
        public bool Command_Limpiar(string direccionIP, ref Socket Cliente_bascula, string comando)
        {
            string[] Dato_Recibido = null;
            string reg_enviado;
            bool Limpiar = false;
            string strcomando = "";

            strcomando = comando + (char)9 + (char)10;

            Cte.Envio_Dato(ref Cliente_bascula, direccionIP, strcomando, ref Dato_Recibido);

            if (Dato_Recibido != null)
            {
                reg_enviado = strcomando.Substring(4);
                if (Dato_Recibido[0].IndexOf("Error") >= 0) { Limpiar = false; }
                if (Dato_Recibido[0].IndexOf("Ok") >= 0) { Limpiar = true; }
            }

            return Limpiar;
        }
       
        #endregion

        #region FUNCIONES PARA CREAR, MODIFICAR Y ELIMINAR EN LAS BASE DE DATOS

        public void Crear_InfoAdd_detalle(long bascula, long sucursal, string[] DatosNuevos, bool StPendiente, bool StEnviado)
        {
            string[] DatoNuevo2;
            // OleDbDataReader DbRead;
            bool existe = false;
            DataRow[] DR_ingre;

			ingredetalleTableAdapter.Fill(baseDeDatosDataSet.Ingre_detalle);

            for (int i = 0; i < DatosNuevos.Length - 1; i++)
            {
                DatoNuevo2 = DatosNuevos[i].Split((char)9);

                Conec.Condicion = "id_ingrediente = " + Convert.ToInt32(DatoNuevo2[0]) + " and id_bascula = " + bascula + "and id_grupo = " + sucursal;

                //  DbRead = Conec.Obtiene_Dato("Select * From Ingre_detalle Where (" + Conec.Condicion + ")", Conec.CadenaConexion);
                DR_ingre = baseDeDatosDataSet.Ingre_detalle.Select(Conec.Condicion);
                if (DR_ingre.Length > 0) existe = true;
                else existe = false;
                // if (DbRead.Read()) existe = true;
                //else existe = false;
                //DbRead.Close();

                if (existe)
                {
                    Conec.CadenaSelect = "UPDATE Ingre_detalle " +
                    "SET pendiente = " + StPendiente + ", enviado = " + StEnviado + " WHERE (" + Conec.Condicion + ")";
                    Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Ingre_detalle.TableName);
                }
                else
                {
                    Conec.CadenaSelect = "INSERT INTO Ingre_Detalle " +
                       "(id_bascula,id_grupo,id_ingrediente, pendiente,enviado)" +
                       "VALUES (" + bascula + "," +
                       sucursal + "," +
                       Convert.ToInt32(DatoNuevo2[0]) + "," +
                       StPendiente + "," + StEnviado + ")";
                    Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Ingre_detalle.TableName);
                }
            }
            ingredetalleTableAdapter.Fill(baseDeDatosDataSet.Ingre_detalle);
        }
        public void Borrar_InfoAdd_detalle(long bascula, long grupo, string[] DatosNuevos)
        {
            string[] DatoNuevo2;
            // OleDbDataReader DbRead;
            DataRow[] DR_ingre;
            bool existe = false;

			ingredetalleTableAdapter.Fill(baseDeDatosDataSet.Ingre_detalle);

            for (int i = 0; i < DatosNuevos.Length - 2; i++)
            {
                DatoNuevo2 = DatosNuevos[i].Split((char)9);

                Conec.Condicion = "id_ingrediente = " + Convert.ToInt32(DatoNuevo2[0]) + " and id_bascula = " + bascula + "and id_grupo = " + grupo;

                // DbRead = Conec.Obtiene_Dato("Select * From Ingre_detalle Where (" + Conec.Condicion + ")", Conec.CadenaConexion);
                //if (DbRead.Read()) existe = true;
                // else existe = false;
                //DbRead.Close();
                DR_ingre = baseDeDatosDataSet.Ingre_detalle.Select(Conec.Condicion);
                if (DR_ingre.Length > 0) existe = true;
                else existe = false;

                if (existe)
                {
                    Conec.CadenaSelect = "DELETE FROM Ingre_detalle WHERE ( " + Conec.Condicion + ")"; //comando pendiente

                    Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, "Ingre_detalle");
                }
            }
            ingredetalleTableAdapter.Fill(baseDeDatosDataSet.Ingre_detalle);
        }
        public void Crear_Oferta_Detalle(long bascula, long sucursal, string[] DatosNuevos, bool StPendiente, bool StEnviado)
        {
            string[] DatoNuevo2;
            // OleDbDataReader DbRead;
            DataRow[] DR_oferta;
            bool existe = false;
			oferta_DetalleTableAdapter.Fill(baseDeDatosDataSet.Oferta_Detalle);

            for (int i = 0; i < DatosNuevos.Length - 1; i++)
            {

                DatoNuevo2 = DatosNuevos[i].Split((char)9);

                Conec.Condicion = "id_oferta = " + Convert.ToInt32(DatoNuevo2[0]) + " and id_bascula = " + bascula + "and id_grupo = " + sucursal;

                // DbRead = Conec.Obtiene_Dato("Select * From Oferta_Detalle Where (" + Conec.Condicion + ")", Conec.CadenaConexion);
                // if (DbRead.Read()) existe = true;
                // else existe = false;
                //DbRead.Close();
                DR_oferta = baseDeDatosDataSet.Oferta_Detalle.Select(Conec.Condicion);
                if (DR_oferta.Length > 0) existe = true;
                else existe = false;

                if (existe)
                {
                    Conec.CadenaSelect = "UPDATE Oferta_detalle " +
                    "SET pendiente = " + StPendiente + ",enviado = " + StEnviado + " WHERE (" + Conec.Condicion + ")";
                    Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Oferta_Detalle.TableName);
                }
                else
                {
                    Conec.CadenaSelect = "INSERT INTO Oferta_Detalle " +
                       "(id_bascula,id_grupo,id_oferta, pendiente,enviado)" +
                       "VALUES (" + bascula + "," +
                       sucursal + "," +
                       Convert.ToInt32(DatoNuevo2[0]) + "," +
                       StPendiente + "," + StEnviado + ")";
                    Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Oferta_Detalle.TableName);
                }
            }
            oferta_DetalleTableAdapter.Fill(baseDeDatosDataSet.Oferta_Detalle);
        }
        public void Borrar_Oferta_detalle(long bascula, long grupo, string[] DatosNuevos)
        {
            string[] DatoNuevo2;
            //OleDbDataReader DbRead;
            DataRow[] DR_oferta;
            bool existe = false;
			oferta_DetalleTableAdapter.Fill(baseDeDatosDataSet.Oferta_Detalle);

            for (int i = 0; i < DatosNuevos.Length - 2; i++)
            {
                DatoNuevo2 = DatosNuevos[i].Split((char)9);

                Conec.Condicion = "id_oferta = " + Convert.ToInt32(DatoNuevo2[0]) + " and id_bascula = " + bascula + "and id_grupo = " + grupo;

               // DbRead = Conec.Obtiene_Dato("Select * From Oferta_detalle Where (" + Conec.Condicion + ")", Conec.CadenaConexion);
               // if (DbRead.Read()) existe = true;
               // else existe = false;
               // DbRead.Close();
                DR_oferta = baseDeDatosDataSet.Oferta_Detalle.Select(Conec.Condicion);
                if (DR_oferta.Length > 0) existe = true;
                else existe = false;

                if (existe)
                {
                    Conec.CadenaSelect = "DELETE FROM Oferta_detalle WHERE ( " + Conec.Condicion + ")"; //comando pendiente

                    Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, "Oferta_detalle");
                }               
            }
            oferta_DetalleTableAdapter.Fill(baseDeDatosDataSet.Oferta_Detalle);
        }
        public void Crea_Publicidad_Detalle(long bascula, long sucursal, string[] DatosNuevos, bool StPendiente, bool StEnviado)
        {
            string[] DatoNuevo2;
           // OleDbDataReader DbRead;
            bool existe = false;
            DataRow[] DR_publicidad;
			public_DetalleTableAdapter.Fill(baseDeDatosDataSet.Public_Detalle);

            for (int i = 0; i < DatosNuevos.Length - 1; i++)
            {
                DatoNuevo2 = DatosNuevos[i].Split((char)9);

                Conec.Condicion = "id_publicidad = " + Convert.ToInt32(DatoNuevo2[0]) + " and id_bascula = " + bascula + "and id_grupo = " + sucursal;

               // DbRead = Conec.Obtiene_Dato("Select * From Public_Detalle Where (" + Conec.Condicion + ")", Conec.CadenaConexion);
                //if (DbRead.Read()) existe = true;
               // else existe = false;
               // DbRead.Close();
                DR_publicidad = baseDeDatosDataSet.Public_Detalle.Select(Conec.Condicion);
                if (DR_publicidad.Length > 0) existe = true;
                else existe = false;

                if (existe)
                {
                    Conec.CadenaSelect = "UPDATE Public_detalle " +
                    "SET pendiente = " + StPendiente + ", enviado = " + true + " WHERE (" + Conec.Condicion + ")";
                    Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Public_Detalle.TableName);
                }
                else
                {
                    Conec.CadenaSelect = "INSERT INTO Public_Detalle " +
                       "(id_bascula,id_grupo,id_publicidad, pendiente,enviado)" +
                       "VALUES (" + bascula + "," + 
                       sucursal + "," + 
                       Convert.ToInt32(DatoNuevo2[0]) + "," +
                       StPendiente + "," + StEnviado + ")";
                    Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Public_Detalle.TableName);
                }                
            }
            public_DetalleTableAdapter.Fill(baseDeDatosDataSet.Public_Detalle);
        }
        public void Borrar_Publicidad_detalle(long bascula, long grupo, string[] DatosNuevos)
        {
            string[] DatoNuevo2;
            //OleDbDataReader DbRead;
            DataRow[] DR_publicidad;
            bool existe = false;
			public_DetalleTableAdapter.Fill(baseDeDatosDataSet.Public_Detalle);

            for (int i = 0; i < DatosNuevos.Length - 2; i++)
            {
                DatoNuevo2 = DatosNuevos[i].Split((char)9);

                Conec.Condicion = "id_publicidad = " + Convert.ToInt32(DatoNuevo2[0]) + " and id_bascula = " + bascula + "and id_grupo = " + grupo;

              //  DbRead = Conec.Obtiene_Dato("Select * From Public_detalle Where (" + Conec.Condicion + ")", Conec.CadenaConexion);
                //if (DbRead.Read()) existe = true;
                //else existe = false;
                //DbRead.Close();
                DR_publicidad = baseDeDatosDataSet.Public_Detalle.Select(Conec.Condicion);
                if (DR_publicidad.Length > 0) existe = true;
                else existe = false;

                if (existe)
                {
                    Conec.CadenaSelect = "DELETE FROM Public_detalle WHERE ( " + Conec.Condicion + ")"; //comando pendiente

                    Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, "Public_detalle");
                }
            }
            public_DetalleTableAdapter.Fill(baseDeDatosDataSet.Public_Detalle);
        }
        public void Crea_Producto_Detalle(long bascula, long sucursal, string[] DatosNuevos, bool StPendiente, bool StEnviado)
        {
            string[] DatoNuevo2;
           // OleDbDataReader DbRead;
            DataRow[] DR_producto;
            bool existe = false;
		 	pludetalleTableAdapter.Fill(baseDeDatosDataSet.PLU_detalle);

            if (bascula != 0 || sucursal != 0)
            {
                for (int i = 0; i < DatosNuevos.Length - 1; i++)
                {
                    DatoNuevo2 = DatosNuevos[i].Split((char)9);

                    Conec.Condicion = "id_producto = " + Convert.ToInt32(DatoNuevo2[0]) + " and id_bascula = " + bascula + " and id_grupo = " + sucursal;

                   // DbRead = Conec.Obtiene_Dato("Select * From PLU_detalle Where (" + Conec.Condicion + ")", Conec.CadenaConexion);
                    //if (DbRead.Read()) existe = true;
                    //else existe = false;
                    //DbRead.Close();
                    DR_producto = baseDeDatosDataSet.PLU_detalle.Select(Conec.Condicion);
                    if (DR_producto.Length > 0) existe = true;
                    else existe = false;

                    if (existe)
                    {
                        Conec.CadenaSelect = "UPDATE PLU_detalle " +
                        "SET pendiente = " + StPendiente + ", enviado = " + true + " WHERE (" + Conec.Condicion + ")";
                        Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.PLU_detalle.TableName);
                    }
                    else
                    {
                        Conec.CadenaSelect = "INSERT INTO PLU_detalle " +
                           "(id_bascula,id_grupo,id_producto, pendiente,enviado)" +
                           "VALUES (" + bascula + "," +
                           sucursal + "," +
                           Convert.ToInt32(DatoNuevo2[0]) + "," +
                           StPendiente + "," + StEnviado + ")";
                        Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.PLU_detalle.TableName);
                    }
                }
                pludetalleTableAdapter.Fill(baseDeDatosDataSet.PLU_detalle);
            }
        }
        public void Borrar_Producto_detalle(long bascula, long grupo, string[] DatosNuevos)
        {
            string[] DatoNuevo2;
            DataRow[] DR_producto;
           // OleDbDataReader DbRead;
            bool existe = false;
			pludetalleTableAdapter.Fill(baseDeDatosDataSet.PLU_detalle);

            for (int i = 0; i < DatosNuevos.Length - 2; i++)
            {
                DatoNuevo2 = DatosNuevos[i].Split((char)9);

                Conec.Condicion = "id_producto = " + Convert.ToInt32(DatoNuevo2[0]) + " and id_bascula = " + bascula + "and id_grupo = " + grupo;
               
                DR_producto = baseDeDatosDataSet.PLU_detalle.Select(Conec.Condicion);
                if (DR_producto.Length > 0) existe = true;
                else existe = false;
              //  DbRead = Conec.Obtiene_Dato("Select * From PLU_detalle Where (" + Conec.Condicion + ")", Conec.CadenaConexion);
                //if (DbRead.Read()) existe = true;
                //else existe = false;
                //DbRead.Close();

                if (existe)
                {
                    Conec.CadenaSelect = "DELETE FROM PLU_detalle WHERE ( " + Conec.Condicion + ")"; //comando pendiente

                    Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, "PLU_detalle");
                }
            }
            pludetalleTableAdapter.Fill(baseDeDatosDataSet.PLU_detalle);
        }
        public void Borrar_Producto_Carpeta_detalle(long bascula, long grupo, string[] DatosNuevos)
        {
            string[] DatoNuevo2;
            //OleDbDataReader DbRead;
            DataRow[] DR_producto;
            bool existe = false;
			pludetalleTableAdapter.Fill(baseDeDatosDataSet.PLU_detalle);

            for (int i = 0; i < DatosNuevos.Length - 2; i++)
            {
                DatoNuevo2 = DatosNuevos[i].Split((char)9);

                Conec.Condicion = "id_producto = " + Convert.ToInt32(DatoNuevo2[0]) + " and id_bascula = " + bascula + "and id_grupo = " + grupo;

                //DbRead = Conec.Obtiene_Dato("Select * From PLU_detalle Where (" + Conec.Condicion + ")", Conec.CadenaConexion);
                //if (DbRead.Read()) existe = true;
                //else existe = false;
                //DbRead.Close();
                DR_producto = baseDeDatosDataSet.PLU_detalle.Select(Conec.Condicion);
                if (DR_producto.Length > 0) existe = true;
                else existe = false;

                if (existe)
                {
                    Conec.CadenaSelect = "DELETE FROM PLU_detalle WHERE ( " + Conec.Condicion + ")"; //comando pendiente

                    Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, "PLU_detalle");
                }
            }
            pludetalleTableAdapter.Fill(baseDeDatosDataSet.PLU_detalle);
        }
        public void Crear_Vendedor_detalle(long bascula, long sucursal, string[] DatosNuevos, bool StPendiente, bool StEnviado)
        {
            string[] DatoNuevo2;
            //OleDbDataReader DbRead;
            DataRow[] DR_vendedor;
            bool existe = false;
			vendedordetalleTableAdapter.Fill(baseDeDatosDataSet.Vendedor_detalle);

            for (int i = 0; i < DatosNuevos.Length - 1; i++)
            {
                DatoNuevo2 = DatosNuevos[i].Split((char)9);

                Conec.Condicion = "id_vendedor = " + Convert.ToInt32(DatoNuevo2[0]) + " and id_bascula = " + bascula + "and id_grupo = " + sucursal;

                //DbRead = Conec.Obtiene_Dato("Select * From Vendedor_detalle Where (" + Conec.Condicion + ")", Conec.CadenaConexion);
                //if (DbRead.Read()) existe = true;
                //else existe = false;
                //DbRead.Close();
                DR_vendedor = baseDeDatosDataSet.Vendedor_detalle.Select(Conec.Condicion);
                if (DR_vendedor.Length > 0) existe = true;
                else existe = false;

                if (existe)
                {
                    Conec.CadenaSelect = "UPDATE Vendedor_detalle " +
                    "SET pendiente = " + StPendiente + ", enviado = " + StEnviado + " WHERE (" + Conec.Condicion + ")";
                    Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Ingre_detalle.TableName);
                }
                else
                {
                    Conec.CadenaSelect = "INSERT INTO Vendedor_Detalle " +
                       "(id_bascula,id_grupo,id_vendedor, pendiente,enviado)" +
                       "VALUES (" + bascula + "," +
                       sucursal + "," +
                       Convert.ToInt32(DatoNuevo2[0]) + "," +
                       StPendiente + "," + StEnviado + ")";
                    Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Vendedor_detalle.TableName);
                }
            }
            vendedordetalleTableAdapter.Fill(baseDeDatosDataSet.Vendedor_detalle);
        }
        public void Borrar_Vendedor_detalle(long bascula, long grupo, string[] DatosNuevos)
        {
            string[] DatoNuevo2;
           // OleDbDataReader DbRead;
            DataRow[] DR_vendedor;
            bool existe = false;
			vendedordetalleTableAdapter.Fill(baseDeDatosDataSet.Vendedor_detalle);

            for (int i = 0; i < DatosNuevos.Length - 2; i++)
            {
                DatoNuevo2 = DatosNuevos[i].Split((char)9);

                Conec.Condicion = "id_vendedor = " + Convert.ToInt32(DatoNuevo2[0]) + " and id_bascula = " + bascula + "and id_grupo = " + grupo;

                //DbRead = Conec.Obtiene_Dato("Select * From Vendedor_detalle Where (" + Conec.Condicion + ")", Conec.CadenaConexion);
                //if (DbRead.Read()) existe = true;
                //else existe = false;
                //DbRead.Close();
                DR_vendedor = baseDeDatosDataSet.Vendedor_detalle.Select(Conec.Condicion);
                if (DR_vendedor.Length > 0) existe = true;
                else existe = false;

                if (existe)
                {
                    Conec.CadenaSelect = "DELETE FROM Vendedor_detalle WHERE ( " + Conec.Condicion + ")"; //comando pendiente

                    Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, "Vendedor_detalle");
                }
            }
            vendedordetalleTableAdapter.Fill(baseDeDatosDataSet.Vendedor_detalle);
        }

        public void Modifica_DetalleCarpeta(long bascula, long sucursal, string[] DatosNuevos,bool estado)
        {
            string[] DatoNuevo2;

            for (int i = 0; i < DatosNuevos.Length - 2; i++)
            {
                DatoNuevo2 = DatosNuevos[i].Split((char)9);

                Conec.CadenaSelect = "UPDATE carpeta_detalle " +
                "SET enviado = " + estado +
                " WHERE (id_bascula = " + bascula + " AND id_grupo = " + sucursal + "AND ID = " + Convert.ToInt32(DatoNuevo2[0]) + ")";

                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.carpeta_detalle.TableName);

            }
        }
        public void Modifica_DetalleProducto(long bascula, long sucursal, string[] DatosNuevos,bool estado)
        {
            string[] DatoNuevo2;

            for (int i = 0; i < DatosNuevos.Length - 2; i++)
            {
                DatoNuevo2 = DatosNuevos[i].Split((char)9);

                Conec.CadenaSelect = "UPDATE Prod_detalle " +
                "SET enviado = " + estado +
                " WHERE (id_bascula = " + bascula + " AND id_grupo = " + sucursal + "AND id_producto = " + Convert.ToInt32(DatoNuevo2[0]) + " AND id_carpeta = " + Convert.ToInt32(DatoNuevo2[1]) + ")";

                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Prod_detalle.TableName);

            }
        }
        public void Modifica_Estado_Productos(string[] DatosNuevos, bool estado)
        {
            string[] DatoNuevo2;

            for (int i = 0; i < DatosNuevos.Length - 2; i++)
            {
                DatoNuevo2 = DatosNuevos[i].Split((char)9);

                Conec.CadenaSelect = "UPDATE Productos " +
                "SET enviado = " + estado +
                " WHERE (id_producto = " + Convert.ToInt32(DatoNuevo2[0]) + ")";

                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Productos.TableName);
            }
        }
        public void Modifica_Estado_Ingredientes(string[] DatosNuevos, bool estado)
        {
            string[] DatoNuevo2;

            for (int i = 0; i < DatosNuevos.Length - 2; i++)
            {
                DatoNuevo2 = DatosNuevos[i].Split((char)9);

                Conec.CadenaSelect = "UPDATE Ingredientes " +
                "SET enviado = " + estado +
                " WHERE (id_ingrediente = " + Convert.ToInt32(DatoNuevo2[0]) + ")";

                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Ingredientes.TableName);
            }
        }
        public void Modifica_Estado_IngredientesDetalle(long bascula, long sucursal, string[] DatosNuevos, bool estado)
        {
            string[] DatoNuevo2;

            for (int i = 0; i < DatosNuevos.Length - 2; i++)
            {
                DatoNuevo2 = DatosNuevos[i].Split((char)9);

                Conec.CadenaSelect = "UPDATE Ingre_detalle " +
                "SET enviado = " + estado +
                " WHERE (id_bascula = " + bascula + " AND id_grupo = " + sucursal + "AND id_ingrediente = " + Convert.ToInt32(DatoNuevo2[0]) + ")";

                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Ingre_detalle.TableName);
            }
        }
        public void Modifica_Estado_Ofertas(string[] DatosNuevos, bool estado)
        {
            string[] DatoNuevo2;

            for (int i = 0; i < DatosNuevos.Length - 2; i++)
            {
                DatoNuevo2 = DatosNuevos[i].Split((char)9);

                Conec.CadenaSelect = "UPDATE Oferta " +
                "SET enviado = " + estado +
                " WHERE (id_oferta = " + Convert.ToInt32(DatoNuevo2[0]) + ")";

                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Oferta.TableName);
            }
        }
        public void Modifica_Estado_OfertasDetalle(long bascula, long sucursal, string[] DatosNuevos, bool estado)
        {
            string[] DatoNuevo2;

            for (int i = 0; i < DatosNuevos.Length - 2; i++)
            {
                DatoNuevo2 = DatosNuevos[i].Split((char)9);

                Conec.CadenaSelect = "UPDATE Oferta_Detalle " +
                "SET enviado = " + estado +
                " WHERE (id_bascula = " + bascula + " AND id_grupo = " + sucursal + "AND id_ofertas = " + Convert.ToInt32(DatoNuevo2[0]) + ")";

                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Oferta_Detalle.TableName);
            }
        }
        public void Modifica_Estado_Publicidad(string[] DatosNuevos, bool estado)
        {
            string[] DatoNuevo2;

            for (int i = 0; i < DatosNuevos.Length - 2; i++)
            {
                DatoNuevo2 = DatosNuevos[i].Split((char)9);

                Conec.CadenaSelect = "UPDATE Publicidad " +
                "SET enviado = " + estado +
                " WHERE (id_publicidad = " + Convert.ToInt32(DatoNuevo2[0]) + ")";

                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Publicidad.TableName);
            }
        }
        public void Modifica_Estado_PublicidadDetalle(long bascula, long sucursal, string[] DatosNuevos, bool estado)
        {
            string[] DatoNuevo2;

            for (int i = 0; i < DatosNuevos.Length - 2; i++)
            {
                DatoNuevo2 = DatosNuevos[i].Split((char)9);

                Conec.CadenaSelect = "UPDATE Public_Detalle " +
                "SET enviado = " + estado +
                " WHERE (id_bascula = " + bascula + " AND id_grupo = " + sucursal + "AND id_publicidad = " + Convert.ToInt32(DatoNuevo2[0]) + ")";

                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Public_Detalle.TableName);
            }
        }
        public void Modifica_Estado_Vendedores(string[] DatosNuevos, bool estado)
        {
            string[] DatoNuevo2;

            for (int i = 0; i < DatosNuevos.Length - 2; i++)
            {
                DatoNuevo2 = DatosNuevos[i].Split((char)9);

                Conec.CadenaSelect = "UPDATE Vendedores " +
                "SET enviado = " + estado +
                " WHERE (id_vendedor = " + Convert.ToInt32(DatoNuevo2[0]) + ")";

                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Vendedor.TableName);
            }
        }

        private void Borrar_Trama_Pendiente(long bascula, long grupo, Int32 nregistro)
        {
            Conec.CadenaSelect = "DELETE FROM BufferSalida " +
            "WHERE ( id_bascula = " + bascula + " and id_grupo = " + grupo + " and id_error = " + nregistro + ")"; //comando pendiente

            Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, "BufferSalida");
        }
        public void Guardar_Trama_pendiente(long bascula, long sucursal, string Trama_Pendiente)
        {
            Int32 nreg = 1;

            if (bascula != 0 || sucursal != 0)
            {
                OleDbDataReader OLpen = Conec.Obtiene_Dato("select id_error from BufferSalida Order by id_error Desc", Conec.CadenaConexion);
                if (OLpen.Read()) nreg = Convert.ToInt32(OLpen.GetValue(0)) + 1;
                OLpen.Close();

                Conec.CadenaSelect = "INSERT INTO BufferSalida " +
                "(id_bascula,id_grupo, id_error, comando)" +
               "VALUES (" + bascula + "," +   //id_bascula
                             sucursal + "," +   //id_grupo 
                             nreg + ",'" + //id_error registro de error                         
                             Trama_Pendiente + "')"; //comando pendiente

                Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, "BufferSalida");
            }
        }

        #endregion
    }

    public class WorkerForm1
    {
        Conexion Cte = new Conexion();
        Envia_Dato Env = new Envia_Dato();
        ADOutil Conec = new ADOutil();

        #region Definicion Dataset
        BaseDeDatosDataSet baseDeDatosDataSet = new BaseDeDatosDataSet();
        BaseDeDatosDataSetTableAdapters.BasculaTableAdapter basculasTableAdapter = new BaseDeDatosDataSetTableAdapters.BasculaTableAdapter();
        BaseDeDatosDataSetTableAdapters.ProductosTableAdapter productosTableAdapter = new BaseDeDatosDataSetTableAdapters.ProductosTableAdapter();
        BaseDeDatosDataSetTableAdapters.Prod_detalleTableAdapter prod_detalleTableAdapter = new BaseDeDatosDataSetTableAdapters.Prod_detalleTableAdapter();
        BaseDeDatosDataSetTableAdapters.PublicidadTableAdapter publicidadTableAdapter = new BaseDeDatosDataSetTableAdapters.PublicidadTableAdapter();
        BaseDeDatosDataSetTableAdapters.Public_DetalleTableAdapter public_DetalleTableAdapter = new BaseDeDatosDataSetTableAdapters.Public_DetalleTableAdapter();
        BaseDeDatosDataSetTableAdapters.OfertaTableAdapter ofertaTableAdapter = new BaseDeDatosDataSetTableAdapters.OfertaTableAdapter();
        BaseDeDatosDataSetTableAdapters.Oferta_DetalleTableAdapter oferta_DetalleTableAdapter = new BaseDeDatosDataSetTableAdapters.Oferta_DetalleTableAdapter();
        BaseDeDatosDataSetTableAdapters.VendedorTableAdapter vendedorTableAdapter = new BaseDeDatosDataSetTableAdapters.VendedorTableAdapter();
        BaseDeDatosDataSetTableAdapters.IngredientesTableAdapter ingredientesTableAdapter = new BaseDeDatosDataSetTableAdapters.IngredientesTableAdapter();
        BaseDeDatosDataSetTableAdapters.Ingre_detalleTableAdapter ingredetalleTableAdapter = new BaseDeDatosDataSetTableAdapters.Ingre_detalleTableAdapter();
        BaseDeDatosDataSetTableAdapters.carpeta_detalleTableAdapter carpetadetalleTableAdapter = new BaseDeDatosDataSetTableAdapters.carpeta_detalleTableAdapter();
        BaseDeDatosDataSetTableAdapters.BufferSalidaTableAdapter BufferSalidaTableAdapter = new BaseDeDatosDataSetTableAdapters.BufferSalidaTableAdapter();
        BaseDeDatosDataSetTableAdapters.VentasTableAdapter ventasTableAdapter = new BaseDeDatosDataSetTableAdapters.VentasTableAdapter();
        BaseDeDatosDataSetTableAdapters.Ventas_DetalleTableAdapter ventadetalleTableAdapter = new BaseDeDatosDataSetTableAdapters.Ventas_DetalleTableAdapter();
        BaseDeDatosDataSetTableAdapters.PLU_detalleTableAdapter pludetalleTableAdapter = new BaseDeDatosDataSetTableAdapters.PLU_detalleTableAdapter();
        BaseDeDatosDataSetTableAdapters.Vendedor_detalleTableAdapter vendedordetalleTableAdapter = new BaseDeDatosDataSetTableAdapters.Vendedor_detalleTableAdapter();
        #endregion

        #region Declaracion de Variables
        private   int iCountThread_t;
        private   string sIp_t;
        private   List<int> lCodigosProductos_t;
        private   List<int> lCodigosVendedores_t;
        private   long iIdBascula_t;
        private   long iIdGrupo_t;
        private   int iAction_t;
        #endregion
        
        public WorkerForm1(int iCountThread,string sIp,ref List<int> lCodigosProductos, ref List<int> lCodigosVendedores, int iAction)
        {
            // TODO: Complete member initialization
            this.iCountThread_t = iCountThread;
            this.sIp_t = sIp;
            this.lCodigosProductos_t = lCodigosProductos;
            this.lCodigosVendedores_t = lCodigosVendedores;
            this.iAction_t = iAction;
        }

        public WorkerForm1(int iCountThread, string sIp, long iIdBascula, long iIdGrupo, int iAction)
        {
            this.iCountThread_t = iCountThread;
            this.sIp_t = sIp;
            this.iIdBascula_t = iIdBascula;
            this.iIdGrupo_t = iIdGrupo;
            this.iAction_t = iAction;
        }

        // This method will be called when the thread is started. 
        public int vSendDatosBascula()
        {
            Socket Cliente_bascula = null;
            int iResultConect = 0;

            int iXpos = 0;
            int iYpos = 0;

            if (iCountThread_t > 10)
            {
                iXpos = 510 * 2;
                iYpos = 130 * (iCountThread_t - 11);
            }
            else if (iCountThread_t > 5)
            {
                iXpos = 510;
                iYpos = 130 * (iCountThread_t - 6);
            }
            else
            {
                iXpos = 0;
                iYpos = 130 * iCountThread_t;
            }
            try
            {
                ProgressContinue pro = null;

                switch (iAction_t)
                {
                    case 0:
                        // pro.IniciaProcess("Bascula " + iIdBascula_t, " Ip: " + sIp_t + " en Grupo: " + iIdGrupo_t);
                        Console.WriteLine("Thread para bascula {0}, grupo {1}, ip {2}", iIdBascula_t, iIdGrupo_t, sIp_t);

                        break;
                    case 1:
                        pro = new ProgressContinue(iXpos, iYpos);
                        pro.IniciaProcess(Variable.SYS_MSJ[351, Variable.idioma] + sIp_t + "... ");
                        Console.WriteLine("Conectando con IP {0}", sIp_t);
                        break;
                    case 2:
                        pro = new ProgressContinue(iXpos, iYpos);
                        pro.UpdateProcessInternal(Variable.SYS_MSJ[374, Variable.idioma] + " " + sIp_t);
                        Console.WriteLine("Conectando con IP {0}", sIp_t);
                        break;
                    default:
                        pro = new ProgressContinue(iXpos, iYpos);
                        break;
                }

                Cliente_bascula = Cte.CheckConnectivityForProxyHost(sIp_t, 50036);
                switch (iAction_t)
                {
                    case 0:     //Envio de datos en Background
                        if (Cliente_bascula.Connected == true)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("Envio de datos en background");
                            Console.ForegroundColor = ConsoleColor.White;
                            Env.Envia_Dato_Pendientes(sIp_t, ref Cliente_bascula, iIdBascula_t, iIdGrupo_t);
                            Enviar_Productos(sIp_t, ref Cliente_bascula, iIdBascula_t, iIdGrupo_t);
                            Env.Enviar_DetalleProducto_Bascula(sIp_t, ref Cliente_bascula, iIdBascula_t, iIdGrupo_t);
                            Env.Enviar_InfoAdicional(sIp_t, ref Cliente_bascula, iIdBascula_t, iIdGrupo_t);
                            Env.Envia_Ofertas(sIp_t, ref Cliente_bascula, iIdBascula_t, iIdGrupo_t);
                            Enviar_Publicidad(sIp_t, ref Cliente_bascula, iIdBascula_t, iIdGrupo_t);
                            Env.Enviar_Vendedores(sIp_t, ref Cliente_bascula, iIdBascula_t, iIdGrupo_t);
                            vActualizar_Screen_Bascula(sIp_t, ref Cliente_bascula);
                        }
                        else
                        {
                            iResultConect = 1;
                            Thread.Sleep(2000);
                            if (Variable.Comando_Ipad)
                            {
                                MessageBox.Show(Variable.SYS_MSJ[34, Variable.idioma] + " " + sIp_t, Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                Variable.Comando_Ipad = false;
                            }
                        }
                        break;

                    case 1:     //Uso para punto de venta
                        if (Cliente_bascula.Connected == true)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("Conectada con IP {0}", sIp_t);
                            Console.ForegroundColor = ConsoleColor.White;
                            Enviar_Borrado_productos(sIp_t, ref Cliente_bascula, pro);
                            Enviar_Borrado_DetalleProducto(sIp_t, ref Cliente_bascula, pro);
                            Enviar_DetalleProducto_Bascula(sIp_t, ref Cliente_bascula, lCodigosProductos_t, pro);
                            Enviar_Productos(sIp_t, ref Cliente_bascula, lCodigosProductos_t, pro);
                            Enviar_Vendedores(sIp_t, ref Cliente_bascula, lCodigosVendedores_t, pro);
                            Enviar_Borrado_vendedores(sIp_t, ref Cliente_bascula, pro);
                            vActualizar_Screen_Bascula(sIp_t, ref Cliente_bascula);
                            pro.TerminaProcess();
                            Thread.Sleep(500);
                        }
                        else
                        {
                            iResultConect = 1;
                            Thread.Sleep(2000);
                            MessageBox.Show(Variable.SYS_MSJ[34, Variable.idioma] + " " + sIp_t, Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        break;

                    case 2:
                        if (Cliente_bascula.Connected == true)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("Caso 2 de envio");
                            Console.ForegroundColor = ConsoleColor.White;
                            int iCountRegistro = Enviar_Precios(sIp_t, ref Cliente_bascula, iIdBascula_t, iIdGrupo_t, pro);
                            if (iCountRegistro > 0)
                            {
                                pro.UpdateProcessInternal(Variable.SYS_MSJ[368, Variable.idioma] + " " + iCountRegistro + " " + Variable.SYS_MSJ[369, Variable.idioma] + " " + sIp_t);
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                pro.UpdateProcessInternal(Variable.SYS_MSJ[367, Variable.idioma] + " " + sIp_t);
                                Thread.Sleep(3000);
                            }
                            pro.TerminaProcess();
                            Thread.Sleep(500);
                        }
                        else
                        {
                            iResultConect = 1;
                            Thread.Sleep(2000);
                            MessageBox.Show(Variable.SYS_MSJ[34, Variable.idioma] + " " + sIp_t, Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        break;
                }
                Cte.desconectar(ref Cliente_bascula);
                return iResultConect;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Variable.SYS_MSJ[381, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                return 0;
            }  
        }

        #region Transmision datos a punto de venta
        public int Enviar_Productos(string direccionIP, ref Socket Cliente_bascula, List<int> lCodigosProductos, ProgressContinue pMsg)
        {            
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msj_recibido;
            int reg_leido = 0;
            int reg_envio = 0;
            int reg_detalle = 0;
            string sTrama_Enviar = "";
            List<string[]> LRut_Imagen = new List<string[]>();

            baseDeDatosDataSet.Productos.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Productos.CodigoColumn };
            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);

            int iNumberProducts = 0;

            if (lCodigosProductos.Count > 0)
            {
                for (int i = 0; i < lCodigosProductos.Count; i++)
                {
                    DataRow DR = baseDeDatosDataSet.Productos.Rows.Find(lCodigosProductos[i]);

                    if (DR != null && Convert.ToBoolean(DR["borrado"]) == false)
                    {
                        iNumberProducts++;
                    }
                }
            }

            if (iNumberProducts > 0)
            {

                pMsg.IniciaProcess(iNumberProducts, Variable.SYS_MSJ[249, Variable.idioma] + " " + direccionIP + "... ");

                string sComando = "XX" + (char)9 + (char)10;
                Msj_recibido = Env.Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");

                if (Msj_recibido != null)
                {
                    if (lCodigosProductos.Count > 0)
                    {
                        for (int i = 0; i < lCodigosProductos.Count; i++)
                        {
                            DataRow DR = baseDeDatosDataSet.Productos.Rows.Find(lCodigosProductos[i]);

                            if (DR != null && Convert.ToBoolean(DR["borrado"]) == false)
                            {
                                sTrama_Enviar = sTrama_Enviar + Env.Genera_Trama_Producto(DR);
                                reg_leido++;
                                reg_detalle++;

                                if (reg_leido > 4)
                                {
                                    reg_envio = reg_envio + reg_leido;
                                    pMsg.UpdateProcess(reg_leido, Variable.SYS_MSJ[249, Variable.idioma] + " " + direccionIP + "... ");

                                    Msj_recibido = Env.Command_Enviado(reg_leido, sTrama_Enviar, direccionIP, ref Cliente_bascula, 0, 0, "XGP");

                                    reg_leido = 0;
                                    sTrama_Enviar = "";
                                }
                            }
                        }

                        if (sTrama_Enviar.Length > 0 && reg_leido <= 4)
                        {
                            reg_envio = reg_envio + reg_leido;

                            pMsg.UpdateProcess(reg_leido, Variable.SYS_MSJ[249, Variable.idioma] + " " + direccionIP + "... ");

                            Msj_recibido = Env.Command_Enviado(reg_leido, sTrama_Enviar, direccionIP, ref Cliente_bascula, 0, 0, "XGP");

                            sTrama_Enviar = "";
                            reg_leido = 0;
                        }

                        if (reg_envio > 0)
                        {
                            Env.Command_Limpiar(direccionIP, ref Cliente_bascula, "XGPF0");
                        }

                        int iREgImagen = 0;

                        for (int i = 0; i < lCodigosProductos.Count; i++)
                        {
                            DataRow DR = baseDeDatosDataSet.Productos.Rows.Find(lCodigosProductos[i]);

                            int posicion = DR["imagen1"].ToString().LastIndexOf('\\');

                            if (posicion > 0 && Convert.ToBoolean(DR["pendiente"]) == true && Convert.ToBoolean(DR["imagen"]) == true && Convert.ToBoolean(DR["borrado"]) == false)
                            {
                                iREgImagen++;
                            }
                        }

                        if (iREgImagen > 0)
                        {
                            pMsg.IniciaProcess(iREgImagen, Variable.SYS_MSJ[252, Variable.idioma] + " " + direccionIP + "... ");
                        }

                        int iCountSendBadImage = 0;

                        for (int i = 0; i < lCodigosProductos.Count; i++)
                        {
                            DataRow DR = baseDeDatosDataSet.Productos.Rows.Find(lCodigosProductos[i]);

                            int posicion = DR["imagen1"].ToString().LastIndexOf('\\');

                            if (posicion > 0 && Convert.ToBoolean(DR["pendiente"]) == true && Convert.ToBoolean(DR["imagen"]) == true && Convert.ToBoolean(DR["borrado"]) == false)
                            {
                                string ImagenAEnviar = DR["imagen1"].ToString();

                                pMsg.UpdateProcess(1, Variable.SYS_MSJ[252, Variable.idioma] + " " + direccionIP + "... ");

                                CommandTorrey myobj = new CommandTorrey();
                                int iRtaFunct = myobj.TORREYSendImagesToScale(direccionIP, ImagenAEnviar, "Product", Cliente_bascula);

                                if (iRtaFunct == 0)
                                {
                                    string[] sDato = { lCodigosProductos[i].ToString(), ImagenAEnviar };
                                    LRut_Imagen.Add(sDato);
                                    Console.WriteLine("Si se transmitio: {0}", ImagenAEnviar);
                                }
                                else
                                {
                                    iCountSendBadImage++;
                                    Console.WriteLine("No se transmitio: {0}, {1}", ImagenAEnviar, iRtaFunct);
                                }
                            }
                        }

                        if (iCountSendBadImage > 0)
                        {
                            MessageBox.Show("Ip " + direccionIP + " " + Variable.SYS_MSJ[296, Variable.idioma] + ": " + iCountSendBadImage, "IMAGEN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        if (LRut_Imagen.Count > 0)
                        {
                            sTrama_Enviar = "";
                            reg_envio = 0;
                            reg_leido = 0;

                            pMsg.IniciaProcess(LRut_Imagen.Count, Variable.SYS_MSJ[352, Variable.idioma] + " " + direccionIP + "... ");

                            for (int i = 0; i < LRut_Imagen.Count; i++)
                            {
                                string[] sDato = LRut_Imagen[i];

                                DataRow DR = baseDeDatosDataSet.Productos.Rows.Find(Convert.ToInt32(sDato[0]));

                                sTrama_Enviar = sTrama_Enviar + Env.Genera_Trama_RImagen(DR);
                                reg_leido++;
                                reg_detalle++;

                                pMsg.UpdateProcess(1, Variable.SYS_MSJ[352, Variable.idioma] + " " + direccionIP + "... ");

                                if (reg_leido > 4)
                                {
                                    reg_envio = reg_envio + reg_leido;                                    
                                    Msj_recibido = Env.Command_Enviado(reg_leido, sTrama_Enviar, direccionIP, ref Cliente_bascula, 0, 0, "XGR");

                                    reg_leido = 0;
                                    sTrama_Enviar = "";
                                }
                            }

                            if (sTrama_Enviar.Length > 0 && reg_leido <= 4)
                            {
                                reg_envio = reg_envio + reg_leido;                                
                                Msj_recibido = Env.Command_Enviado(reg_leido, sTrama_Enviar, direccionIP, ref Cliente_bascula, 0, 0, "XGR");

                                sTrama_Enviar = "";
                                reg_leido = 0;
                            }

                            if (reg_envio > 0)
                            {
                                Env.Command_Limpiar(direccionIP, ref Cliente_bascula, "XGRF0");
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No se envio comando de inicio");
                }
            }

            return reg_detalle;
        }
        public void Enviar_DetalleProducto_Bascula(string direccionIP, ref Socket Cliente_bascula, List<int> lCodigosProductos, ProgressContinue pMsg)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msj_recibido;
            string Variable_frame = "";
            int reg_leido = 0;
            int reg_envio = 0;


            baseDeDatosDataSet.Productos.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Productos.CodigoColumn };
            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);
            Variable_frame = "";

            if (lCodigosProductos.Count > 0)
            {

                int iNumberErase = 0;
                for (int i = 0; i < lCodigosProductos.Count; i++)
                {
                    DataRow DR = baseDeDatosDataSet.Productos.Rows.Find(lCodigosProductos[i]);

                    if (DR != null && Convert.ToBoolean(DR["borrado"]) == false)
                    {
                        iNumberErase++;                       
                    }
                }

                if (iNumberErase > 0)
                {

                    pMsg.IniciaProcess(iNumberErase, Variable.SYS_MSJ[348, Variable.idioma] + " " + direccionIP + "... ");

                    for (int i = 0; i < lCodigosProductos.Count; i++)
                    {
                        DataRow DR = baseDeDatosDataSet.Productos.Rows.Find(lCodigosProductos[i]);

                        if (DR != null && Convert.ToBoolean(DR["borrado"]) == false)
                        {
                            Variable_frame = Variable_frame + Env.Genera_Trama_Producto_Detalle(Convert.ToInt32(DR["id_producto"].ToString()), 0, 0, "P");
                            reg_leido++;

                            pMsg.UpdateProcess(1, Variable.SYS_MSJ[348, Variable.idioma] + " " + direccionIP + "... ");

                            if (reg_leido > 4)
                            {
                                reg_envio = reg_envio + reg_leido;                                
                                Msj_recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, 0, 0, "XGA");
                                reg_leido = 0;
                                Variable_frame = "";
                            }
                        }
                    }
                    if (Variable_frame.Length > 0 && reg_leido <= 4)
                    {
                        reg_envio = reg_envio + reg_leido;                        
                        Msj_recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, 0, 0, "XGA");
                        reg_leido = 0;
                        Variable_frame = "";
                    }

                    if (reg_envio > 0)
                    {
                        Env.Command_Limpiar(direccionIP, ref Cliente_bascula, "XGAF0");
                    }
                }
            }

        }
        public void Enviar_Borrado_productos(string direccionIP, ref Socket Cliente_bascula, ProgressContinue pMsg)
        {
            string Msg_Recibido;
            string strborrado;
            int TotalBorrado = 0;
            char[] chr = new char[] { (char)10, (char)13 };

            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);
            DataRow[] DRE = baseDeDatosDataSet.Productos.Select("borrado = " + true + " and pendiente = " + true, "id_producto");

            if (DRE.Length > 0)
            {
                pMsg.IniciaProcess(DRE.Length, Variable.SYS_MSJ[348, Variable.idioma] + " " + direccionIP + "... ");

                foreach (DataRow DR in DRE)
                {
                    pMsg.UpdateProcess(1, Variable.SYS_MSJ[348, Variable.idioma] + " " + direccionIP + "... ");

                    strborrado = DR["id_producto"].ToString() + (char)9 + (char)10;
                    Msg_Recibido = Env.Command_Enviado(1, strborrado, direccionIP, ref Cliente_bascula, 0, 0, "XEP");

                    if (Msg_Recibido != null)
                    {
                        TotalBorrado++;
                        strborrado = "";                        
                    }
                }
            }
        }
        public void Enviar_Borrado_DetalleProducto(string direccionIP, ref Socket Cliente_bascula, ProgressContinue pMsg)
        {
            string Msg_Recibido;
            string strborrado;
            int TotalBorrado = 0;
            char[] chr = new char[] { (char)10, (char)13 };

            pMsg.UpdateProcessInternal(Variable.SYS_MSJ[354, Variable.idioma]);

            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);
            DataRow[] DRE = baseDeDatosDataSet.Productos.Select("borrado = " + true + " and pendiente = " + true, "id_producto");

            if (DRE.Length > 0)
            {

                pMsg.IniciaProcess(DRE.Length, Variable.SYS_MSJ[354, Variable.idioma] + " " + direccionIP + "... ");

                foreach (DataRow DR in DRE)
                {
                    pMsg.UpdateProcess(1, Variable.SYS_MSJ[354, Variable.idioma] + " " + direccionIP + "... ");

                    strborrado = (char)9 + DR["id_producto"].ToString() + (char)9 + (char)10;
                    Msg_Recibido = Env.Command_Enviado(1, strborrado, direccionIP, ref Cliente_bascula, 0, 0, "XEA");

                    if (Msg_Recibido != null)
                    {
                        TotalBorrado++;
                        strborrado = "";
                    }
                }
            }
        }
        public int Enviar_Vendedores(string direccionIP, ref Socket Cliente_bascula, List<int> lCodigosVendedores, ProgressContinue pMsg)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msj_recibido;
            int reg_leido = 0;
            int reg_envio = 0;
            int reg_detalle = 0;
            string sTrama_Enviar = "";

            baseDeDatosDataSet.Vendedor.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Vendedor.id_vendedorColumn};
            vendedorTableAdapter.Fill(baseDeDatosDataSet.Vendedor);

            if (lCodigosVendedores.Count > 0)
            {

                pMsg.IniciaProcess(lCodigosVendedores.Count, Variable.SYS_MSJ[254, Variable.idioma] + " " + direccionIP + "... ");

                string sComando = "XX" + (char)9 + (char)10;
                Msj_recibido = Env.Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");

                if (Msj_recibido != null)
                {
                    if (lCodigosVendedores.Count > 0)
                    {
                        for (int i = 0; i < lCodigosVendedores.Count; i++)
                        {
                            DataRow DR = baseDeDatosDataSet.Vendedor.Rows.Find(lCodigosVendedores[i]);

                            if (DR != null && Convert.ToBoolean(DR["borrado"]) == false)
                            {
                                sTrama_Enviar = sTrama_Enviar + Env.Genera_Trama_Vendedor(DR);
                                reg_leido++;
                                reg_detalle++;

                                if (reg_leido > 4)
                                {
                                    reg_envio = reg_envio + reg_leido;
                                    pMsg.UpdateProcess(reg_leido, Variable.SYS_MSJ[254, Variable.idioma] + " " + direccionIP + "... ");

                                    Msj_recibido = Env.Command_Enviado(reg_leido, sTrama_Enviar, direccionIP, ref Cliente_bascula, 0, 0, "GV");

                                    reg_leido = 0;
                                    sTrama_Enviar = "";
                                }
                            }
                            else
                            {
                                pMsg.UpdateProcess(1, Variable.SYS_MSJ[254, Variable.idioma] + " " + direccionIP);
                            }
                        }

                        if (sTrama_Enviar.Length > 0 && reg_leido <= 4)
                        {
                            reg_envio = reg_envio + reg_leido;

                            pMsg.UpdateProcess(reg_leido, Variable.SYS_MSJ[254, Variable.idioma] + " " + direccionIP + "... ");

                            Msj_recibido = Env.Command_Enviado(reg_leido, sTrama_Enviar, direccionIP, ref Cliente_bascula, 0, 0, "GV");

                            sTrama_Enviar = "";
                            reg_leido = 0;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No se envio comando de inicio");
                }
            }

            return reg_detalle;
        }
        public void Enviar_Borrado_vendedores(string direccionIP, ref Socket Cliente_bascula, ProgressContinue pMsg){
            string Msg_Recibido;
            string strborrado;
            int TotalBorrado = 0;
            char[] chr = new char[] { (char)10, (char)13 };

            vendedorTableAdapter.Fill(baseDeDatosDataSet.Vendedor);
            DataRow[] DRE = baseDeDatosDataSet.Vendedor.Select("borrado = " + true + " and pendiente = " + true, "id_vendedor");

            if (DRE.Length > 0)
            {
                pMsg.IniciaProcess(DRE.Length, Variable.SYS_MSJ[354, Variable.idioma] + " " + direccionIP + "... ");

                foreach (DataRow DR in DRE)
                {
                    strborrado = DR["id_vendedor"].ToString() + (char)9 + (char)10;
                    Msg_Recibido = Env.Command_Enviado(1, strborrado, direccionIP, ref Cliente_bascula, 0, 0, "EV");

                    if (Msg_Recibido != null)
                    {
                        TotalBorrado++;
                        strborrado = "";
                        pMsg.UpdateProcess(1, Variable.SYS_MSJ[354, Variable.idioma] + " " + direccionIP + "... ");
                    }
                }
            }
        }

        public void vActualizar_Screen_Bascula(string direccionIP, ref Socket Cliente_bascula)
        {
            string Msg_Recibido;
            string strborrado = (char)9 + "1" + (char)9 + (char)10;
            Msg_Recibido = Env.Command_Enviado(1, strborrado, direccionIP, ref Cliente_bascula, 0, 0, "XAB");
        }
        #endregion

        #region Actualizacion de Precios

        public int Enviar_Precios(string direccionIP, ref Socket Cliente_bascula, long nbascula, long ngrupo, ProgressContinue pMsg)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msj_recibido;
            int reg_leido = 0;
            int reg_envio = 0;
            int reg_detalle = 0;
            int reg_total_pendientes;

            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);
            prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);
            pludetalleTableAdapter.Fill(baseDeDatosDataSet.PLU_detalle);

            DataRelation Producto_Detalle = baseDeDatosDataSet.Relations["ProductosProd_detalle"];
            DataRelation Plus_Detalle = baseDeDatosDataSet.Relations["Productos_PLU_detalle"];

            reg_total_pendientes = baseDeDatosDataSet.Productos.Select("pendiente = " + true + " and borrado = " + false).Length;
            DataRow[] DT = baseDeDatosDataSet.Productos.Select("pendiente = " + true + " and borrado = " + false);

            List<int> myCollectionProdProd_Det = new List<int>();
            List<int> myCollectionProdPlu_Det = new List<int>();
            List<int> myCollectionSend = new List<int>();

            if (DT.Length > 0)
            {
                foreach (DataRow DA in DT)
                {
                    foreach (DataRow PR in DA.GetChildRows(Producto_Detalle))
                    {
                        long iIdBascula = Convert.ToInt32(PR["id_bascula"].ToString());
                        //Console.WriteLine();
                        if (iIdBascula == nbascula)
                        {
                            myCollectionProdProd_Det.Add(Convert.ToInt32(PR["id_producto"].ToString()));
                        }
                    }
                }

                foreach (DataRow DA in DT)
                {
                    foreach (DataRow PR in DA.GetChildRows(Plus_Detalle))
                    {
                        long iIdBascula = Convert.ToInt32(PR["id_bascula"].ToString());

                        if (iIdBascula == nbascula)
                        {
                            myCollectionProdPlu_Det.Add(Convert.ToInt32(PR["id_producto"].ToString()));
                        }
                    }
                }
            

                if (myCollectionProdProd_Det.Count > 0)
                {
                    for (int i = 0; i < myCollectionProdProd_Det.Count; i++)
                    {
                        myCollectionSend.Add(myCollectionProdProd_Det[i]);
                    }

                    if (myCollectionProdPlu_Det.Count > 0)
                    {
                        for (int i = 0; i < myCollectionProdPlu_Det.Count; i++)
                        {
                            if (myCollectionSend.BinarySearch(myCollectionProdPlu_Det[i]) < 0)
                            {
                                myCollectionSend.Add(myCollectionProdPlu_Det[i]);
                            }
                        }
                    }
                }
                else
                {
                    if (myCollectionProdPlu_Det.Count > 0)
                    {
                        for (int i = 0; i < myCollectionProdPlu_Det.Count; i++)
                        {
                            myCollectionSend.Add(myCollectionProdPlu_Det[i]);
                        }
                    }
                }

                Console.WriteLine("Enviando {0} productos", myCollectionSend.Count);

                if (myCollectionSend.Count > 0)
                {

                    pMsg.IniciaProcess(myCollectionSend.Count, Variable.SYS_MSJ[198, Variable.idioma] + " " + direccionIP + "... ");

                    productosTableAdapter.Fill(baseDeDatosDataSet.Productos);

                    string sComando = "XX" + (char)9 + (char)10;
                    Msj_recibido = Env.Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");

                    if (Msj_recibido != null)
                    {
                        string Variable_frame = "";

                        for (int i = 0; i < myCollectionSend.Count; i++)
                        {
                            DataRow DR = baseDeDatosDataSet.Productos.Rows.Find(myCollectionSend[i]);

                            if (DR != null)
                            {
                                Variable_frame = Variable_frame + Env.Genera_Trama_Precio(DR);
                                reg_leido++;
                                reg_detalle++;

                                if (reg_leido > 4)
                                {
                                    reg_envio = reg_envio + reg_leido;

                                    pMsg.UpdateProcess(reg_leido, Variable.SYS_MSJ[198, Variable.idioma] + " " + direccionIP + "... ");

                                    Msj_recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, nbascula, ngrupo, "Gp");

                                    reg_leido = 0;
                                    Variable_frame = "";
                                }
                            }
                        }

                        if (Variable_frame.Length > 0 && reg_leido <= 4)
                        {
                            reg_envio = reg_envio + reg_leido;

                            pMsg.UpdateProcess(reg_leido, Variable.SYS_MSJ[198, Variable.idioma] + " " + direccionIP + "... ");

                            Msj_recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, nbascula, ngrupo, "Gp");

                            Variable_frame = "";
                            reg_leido = 0;
                        }

                        if (reg_envio > 0)
                        {                         
                            Env.Command_Limpiar(direccionIP, ref Cliente_bascula, "GpF0");
                        }
                    }
                }
            }

            return reg_detalle;
        }

        #endregion

        #region Envio informacion Background
        public int Enviar_Productos(string direccionIP, ref Socket Cliente_bascula, long nbascula, long ngrupo)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msj_recibido;
            int reg_leido = 0;
            int reg_envio = 0;
            int reg_detalle = 0;

            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);
            prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);
            pludetalleTableAdapter.Fill(baseDeDatosDataSet.PLU_detalle);

            DataRelation Producto_Detalle = baseDeDatosDataSet.Relations["ProductosProd_detalle"];
            DataRelation Plus_Detalle = baseDeDatosDataSet.Relations["Productos_PLU_detalle"];

            DataRow[] DT = baseDeDatosDataSet.Productos.Select("pendiente = " + true + " and borrado = " + false);

            List<int> myCollectionProdProd_Det = new List<int>();
            List<int> myCollectionProdPlu_Det = new List<int>();
            List<int> myCollectionSend = new List<int>();

            if (DT.Length > 0)
            {
                foreach (DataRow DA in DT)
                {
                    foreach (DataRow PR in DA.GetChildRows(Producto_Detalle))
                    {
                        long iIdBascula = Convert.ToInt32(PR["id_bascula"].ToString());

                        if (iIdBascula == nbascula)
                        {
                            myCollectionProdProd_Det.Add(Convert.ToInt32(PR["id_producto"].ToString()));
                        }
                    }
                }
               
                foreach (DataRow DA in DT)
                {
                    foreach (DataRow PR in DA.GetChildRows(Plus_Detalle))
                    {
                        long iIdBascula = Convert.ToInt32(PR["id_bascula"].ToString());

                        if (iIdBascula == nbascula)
                        {
                            myCollectionProdPlu_Det.Add(Convert.ToInt32(PR["id_producto"].ToString()));
                        }
                    }
                }

                if (myCollectionProdProd_Det.Count > 0)
                {
                    for (int i = 0; i < myCollectionProdProd_Det.Count; i++)
                    {
                        myCollectionSend.Add(myCollectionProdProd_Det[i]);
                    }

                    if (myCollectionProdPlu_Det.Count > 0)
                    {
                        for (int i = 0; i < myCollectionProdPlu_Det.Count; i++)
                        {
                            if (myCollectionSend.BinarySearch(myCollectionProdPlu_Det[i]) < 0)
                            {
                                myCollectionSend.Add(myCollectionProdPlu_Det[i]);
                            }
                        }
                    }
                }
                else
                {
                    if (myCollectionProdPlu_Det.Count > 0)
                    {
                        for (int i = 0; i < myCollectionProdPlu_Det.Count; i++)
                        {
                            myCollectionSend.Add(myCollectionProdPlu_Det[i]);
                        }
                    }
                }

                Console.WriteLine("Enviando {0} productos", myCollectionSend.Count);

                if (myCollectionSend.Count > 0)
                {
                    productosTableAdapter.Fill(baseDeDatosDataSet.Productos);

                    string sComando = "XX" + (char)9 + (char)10;
                    Msj_recibido = Env.Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");

                    if (Msj_recibido != null)
                    {
                        string Variable_frame = "";

                        for (int i = 0; i < myCollectionSend.Count; i++)
                        {
                            DataRow DR = baseDeDatosDataSet.Productos.Rows.Find(myCollectionSend[i]);

                            if (DR != null)
                            {
                                Variable_frame = Variable_frame + Env.Genera_Trama_Producto(DR);
                                reg_leido++;
                                reg_detalle++;


                                if (reg_leido > 4)
                                {
                                    reg_envio = reg_envio + reg_leido;
                                    Msj_recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, nbascula, ngrupo, "GP");

                                    if (Msj_recibido != null)
                                    {
                                        Env.Crea_Producto_Detalle(nbascula, ngrupo, Msj_recibido.Split(chr), false, false);
                                    }

                                    reg_leido = 0;
                                    Variable_frame = "";
                                }
                            }
                        }

                        if (Variable_frame.Length > 0 && reg_leido <= 4)
                        {
                            reg_envio = reg_envio + reg_leido;
                            Msj_recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, nbascula, ngrupo, "GP");

                            if (Msj_recibido != null)
                            {
                                Env.Crea_Producto_Detalle(nbascula, ngrupo, Msj_recibido.Split(chr), false, false);
                            }

                            Variable_frame = "";
                            reg_leido = 0;
                        }

                        if (reg_envio > 0) Env.Command_Limpiar(direccionIP, ref Cliente_bascula, "GPF0");

                        for (int i = 0; i < myCollectionSend.Count; i++)
                        {
                            DataRow DR = baseDeDatosDataSet.Productos.Rows.Find(myCollectionSend[i]);

                            int posicion = DR["imagen1"].ToString().LastIndexOf('\\');

                            if (posicion > 0)
                            {
                                string ImagenAEnviar = DR["imagen1"].ToString();

                                CommandTorrey myobj = new CommandTorrey();
                                int iRtaFunct = myobj.TORREYSendImagesToScale(direccionIP, ImagenAEnviar, "Product", Cliente_bascula);
                            }
                        }
                    }
                }
            }

            return reg_detalle;
        }
        
        public void Enviar_Publicidad(string direccionIP, ref Socket Cliente_bascula, long nbascula, long ngrupo)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msj_recibido;
            string Variable_frame = null;
            int reg_leido = 0;
            int reg_envio = 0;

            publicidadTableAdapter.Fill(baseDeDatosDataSet.Publicidad);
            public_DetalleTableAdapter.Fill(baseDeDatosDataSet.Public_Detalle);

            DataRelation PublicidadAdd_Detalle = baseDeDatosDataSet.Relations["PublicidadPublic_Detalle"];

            List<int> mycollectionPublicidad = new List<int>();

            Variable_frame = "";
            DataRow[] DR_Selec = baseDeDatosDataSet.Publicidad.Select("pendiente = " + true);

            if (DR_Selec.Length > 0)
            {
                foreach (DataRow DA in DR_Selec)
                {
                    //foreach (DataRow PR in DA.GetChildRows(PublicidadAdd_Detalle))
                    //{
                        //if (Convert.ToInt32(PR["id_bascula"].ToString()) == nbascula)
                        //{
                            mycollectionPublicidad.Add(Convert.ToInt32(DA["id_publicidad"].ToString()));
                        //}
                    //}
                }

                Console.WriteLine("Enviando {0} publicidad", mycollectionPublicidad.Count);

                if (mycollectionPublicidad.Count > 0)
                {
                    string sComando = "XX" + (char)9 + (char)10;
                    Msj_recibido = Env.Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");

                    if (Msj_recibido != null)
                    {
                        for (int i = 0; i < mycollectionPublicidad.Count; i++)
                        {
                            DataRow DA = baseDeDatosDataSet.Publicidad.Rows.Find(mycollectionPublicidad[i]);

                            Variable_frame = Variable_frame + Env.Genera_Trama_Publicidad(DA);
                            reg_leido++;

                            if (reg_leido > 4)
                            {
                                reg_envio = reg_envio + reg_leido;

                                Msj_recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, nbascula, ngrupo, "GM");
                                if (Msj_recibido != null)
                                {
                                    Env.Crea_Publicidad_Detalle(nbascula, ngrupo, Msj_recibido.Split(chr), false, false);
                                }

                                reg_leido = 0;
                                Variable_frame = "";
                            }
                        }

                        if (Variable_frame.Length > 0 && reg_leido < 4)
                        {
                            reg_envio = reg_envio + reg_leido;
                            Msj_recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, nbascula, ngrupo, "GM");
                            if (Msj_recibido != null)
                            {
                                Env.Crea_Publicidad_Detalle(nbascula, ngrupo, Msj_recibido.Split(chr), false, false);
                            }

                            reg_leido = 0;
                            Variable_frame = "";
                        }
                    }
                }
            }
        }
        #endregion

    }

    public class StatusChecker
    {
        private int invokeCount;
        private int maxCount;

        public StatusChecker(int count)
        {
            invokeCount = 0;
            maxCount = count;
        }

        // This method is called by the timer delegate.
        public void CheckStatus(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            Console.WriteLine("{0} Checking status {1,2}.",
                DateTime.Now.ToString("h:mm:ss.fff"),
                (++invokeCount).ToString());

            if (invokeCount == maxCount)
            {
                // Reset the counter and signal Main.
                invokeCount = 0;
                autoEvent.Set();
            }
        }
    }

}
 
