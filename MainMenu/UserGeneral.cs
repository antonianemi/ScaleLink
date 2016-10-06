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
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using TorreyTransfer;
using TreeViewBound;

namespace MainMenu
{
    public partial class UserGeneral : UserControl
    {
        public Int32 Num_Grupo;
        public Int32 Num_Bascula;
        public String Nombre_Select;

        delegate void SetTextCallback(string text);

        Variable.lgrupo[] myGrupo;
        Variable.lbasc[] myScale;

        private TextBox[] tScroll;
        private TextBox[] tSplash;
        
        ADOutil Conec = new ADOutil();
        Conexion Cte = new Conexion();
        Serial SR = new Serial();
        ArrayList Dato_Nuevo = new ArrayList();
        Socket Cliente_bascula = null;
        Envia_Dato Env = new Envia_Dato();

        #region Inicializacion
        public UserGeneral()
        {
            InitializeComponent();

            this.tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + string.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);
            tScroll = new TextBox[5] { this.tbxScroll1, this.tbxScroll2, this.tbxScroll3, this.tbxScroll4, this.tbxScroll5 };
            tSplash = new TextBox[5] { this.tbxSplash1, this.tbxSplash2, this.tbxSplash3, this.tbxSplash4, this.tbxSplash5 };
            
            asignarEventos();
           
        }

        void funcionWLABEL()
        {
            gbxAdImages.Enabled = false;
            gbxAdImages.Visible = false;

            groupBox1.Enabled = false;
            groupBox1.Visible = false;

            gbxLogoPrint.Enabled = false;
            gbxLogoPrint.Visible = false;

            gbxScreenLogo.Enabled = false;
            gbxScreenLogo.Visible = false;

            gbxScreenSaverTime.Enabled = false;
            gbxScreenSaverTime.Visible = false;

            gbxAdsTime.Enabled = false;
            gbxAdsTime.Visible = false;

            chkActivaProtect.Visible = false;
            chkActivaProtect.Enabled = false;
            chkActivaPubli.Visible = false;
            chkActivaPubli.Enabled = false;
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
        void Asigna_Productos()
        {
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

        private void Consulta_EnBD()
        {
            DataRow[] DA = baseDeDatosDataSet.Configuracion.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);
            if (DA.Length > 0)
            {
                DataRow dr = DA[0];
                Mostrar_Dato(ref dr);
            }
        }
        private void Mostrar_Dato(ref DataRow dr)
        {
            DataRow[] configRow = baseDeDatosDataSet.Tables["Configuracion"].Select("id_bascula = " + dr["id_bascula"].ToString() + " AND id_grupo = " + dr["id_grupo"].ToString());
            this.tbxScroll1.Text = dr["publicidad1"].ToString();
            this.tbxScroll2.Text = dr["publicidad2"].ToString();
            this.tbxScroll3.Text = dr["publicidad3"].ToString();
            this.tbxScroll4.Text = dr["publicidad4"].ToString();
            this.tbxScroll5.Text = dr["publicidad5"].ToString();
            this.tbxSplash1.Text = dr["splash1"].ToString();
            this.tbxSplash2.Text = dr["splash2"].ToString();
            this.tbxSplash3.Text = dr["splash3"].ToString();
            this.tbxSplash4.Text = dr["splash4"].ToString();
            this.tbxSplash5.Text = dr["splash5"].ToString();
            this.tbxPubli1.Text = dr["imgpubli1"].ToString();
            this.tbxPubli2.Text = dr["imgpubli2"].ToString();
            this.tbxPubli3.Text = dr["imgpubli3"].ToString();
            this.tbxPubli4.Text = dr["imgpubli4"].ToString();
            this.tbxPubli5.Text = dr["imgpubli5"].ToString();
            this.tbxLogoScreen.Text = dr["logo"].ToString();
            this.tbxLogoPrint.Text = dr["logoprint"].ToString();

            if (Convert.ToInt32(dr["u_varios"].ToString()) > 0) this.chkvarios.Checked = true;
            else this.chkvarios.Checked = false;
            if (Convert.ToInt32(dr["p_descuentos"].ToString()) > 0) this.chkdescuento.Checked = true;
            else this.chkdescuento.Checked = false;
            if (Convert.ToInt32(dr["p_devoluciones"].ToString()) > 0) this.chkdevolucion.Checked = true;
            else this.chkdevolucion.Checked = false;
            if (Convert.ToInt32(dr["bloq_precio"].ToString()) > 0) this.chkblqprecio.Checked = true;
            else this.chkblqprecio.Checked = false;
            if (Convert.ToInt32(dr["autoprint"].ToString()) > 0) this.chkautoimp.Checked = true;
            else this.chkautoimp.Checked = false;
            if (Convert.ToInt32(dr["reprint"].ToString()) > 0) this.chkreimpre.Checked = true;
            else this.chkreimpre.Checked = false;
            if (Convert.ToInt16(dr["formatofecha"].ToString()) == 0) this.rbtnf1fecha.Checked = true;
            else this.rbtnf2fecha.Checked = true;
            if (Convert.ToInt16(dr["formatohora"].ToString()) == 0) this.rbtnf1hora.Checked = true;
            else this.rbtnf2hora.Checked = true;
            try
            {
                if (Convert.ToInt32(dr["activasplash"].ToString()) > 0) this.chkActivaProtect.Checked = true;
                else this.chkActivaProtect.Checked = false;
            }
            catch
            {
                Variable.user_activaprotector = 0;
                configRow[0]["activasplash"] = 0;
                this.chkActivaProtect.Checked = false;
            }
            try
            {
                if (Convert.ToInt32(dr["activapubli"].ToString()) > 0) this.chkActivaPubli.Checked = true;
                else this.chkActivaPubli.Checked = false;
            }
            catch
            {
                Variable.user_activapublicidad = 0;
                configRow[0]["activapubli"] = 0;
                this.chkActivaPubli.Checked = false;
            }

            this.tbxPswAdmin.Text = dr["pss_supervisor"].ToString();
            this.tbxPswSupervisor.Text = dr["pss_operario"].ToString();

            this.lbxCetiqueta.Text = dr["tiemposplash"].ToString();
            try
            {
            this.trackTiempo.Value = Convert.ToInt32(dr["tiemposplash"].ToString());
             }
            catch
            {
                this.trackTiempo.Value = 1;
                configRow[0]["tiemposplash"] = 1;
            }
            this.lblTetiqueta.Text = dr["tiempopubli"].ToString();
            try
            {
                this.trackTiempoPubli.Value = Convert.ToInt32(dr["tiempopubli"].ToString());
            }
            catch
            {
                this.trackTiempoPubli.Value = 5;
                configRow[0]["tiempopubli"] = 5;
            }
            Guardar(true);
            this.tbxScroll1.Focus();
        }
        void activarDesactivarEdicion(bool pbActivar, Color clActivar)
        {
            this.tbxScroll1.Enabled = pbActivar;
            this.tbxScroll2.Enabled = pbActivar;
            this.tbxScroll3.Enabled = pbActivar;
            this.tbxScroll4.Enabled = pbActivar;
            this.tbxScroll5.Enabled = pbActivar;
            this.tbxSplash1.Enabled = pbActivar;
            this.tbxSplash2.Enabled = pbActivar;
            this.tbxSplash3.Enabled = pbActivar;
            this.tbxSplash4.Enabled = pbActivar;
            this.tbxSplash5.Enabled = pbActivar;
            this.tbxPubli1.Enabled = pbActivar;
            this.tbxPubli2.Enabled = pbActivar;
            this.tbxPubli3.Enabled = pbActivar;
            this.tbxPubli4.Enabled = pbActivar;
            this.tbxPubli5.Enabled = pbActivar;
            this.tbxLogoScreen.Enabled = pbActivar;
            this.tbxLogoPrint.Enabled = pbActivar;
            this.tbxPswAdmin.Enabled = pbActivar;
            this.tbxPswSupervisor.Enabled = pbActivar;
            this.trackTiempo.Enabled = pbActivar;
            this.trackTiempoPubli.Enabled = pbActivar;

            this.tbxScroll1.BackColor = clActivar;
            this.tbxScroll2.BackColor = clActivar;
            this.tbxScroll3.BackColor = clActivar;
            this.tbxScroll4.BackColor = clActivar;
            this.tbxScroll5.BackColor = clActivar;
            this.tbxSplash1.BackColor = clActivar;
            this.tbxSplash2.BackColor = clActivar;
            this.tbxSplash3.BackColor = clActivar;
            this.tbxSplash4.BackColor = clActivar;
            this.tbxSplash5.BackColor = clActivar;
            this.tbxPubli1.BackColor = clActivar;
            this.tbxPubli2.BackColor = clActivar;
            this.tbxPubli3.BackColor = clActivar;
            this.tbxPubli4.BackColor = clActivar;
            this.tbxPubli5.BackColor = clActivar;
            this.tbxLogoScreen.BackColor = clActivar;
            this.tbxLogoPrint.BackColor = clActivar;
            this.tbxPswAdmin.BackColor = clActivar;
            this.tbxPswSupervisor.BackColor = clActivar;
          
            this.chkActivaProtect.Enabled = pbActivar;
            this.chkvarios.Enabled = pbActivar;
            this.chkdescuento.Enabled = pbActivar;
            this.chkdevolucion.Enabled = pbActivar;
            this.chkblqprecio.Enabled = pbActivar;
            this.chkautoimp.Enabled = pbActivar;
            this.chkreimpre.Enabled = pbActivar;
            this.chkActivaPubli.Enabled = pbActivar;;
            this.rbtnf1fecha.Enabled = pbActivar;
            this.rbtnf2fecha.Enabled = pbActivar;
            this.rbtnf1hora.Enabled = pbActivar;
            this.rbtnf2hora.Enabled = pbActivar;

            this.btnExam1.Enabled = pbActivar;
            this.btnExam2.Enabled = pbActivar;
            this.btnExam3.Enabled = pbActivar;
            this.btnExam4.Enabled = pbActivar;
            this.btnExam5.Enabled = pbActivar;
            this.btnExam6.Enabled = pbActivar;
            this.btnExam7.Enabled = pbActivar;
            this.btnExam8.Enabled = pbActivar;
            this.btnExam9.Enabled = pbActivar;
            this.btnExam10.Enabled = pbActivar;
            this.btnExam11.Enabled = pbActivar;
            this.btnExam12.Enabled = pbActivar;
        }
        private void limpiezaTextBoxes()
        {
            activarDesactivarEdicion(true, Color.White);
            this.tbxScroll1.Text = Variable.SYS_MSJ[318,Variable.idioma].Trim();
            this.tbxScroll2.Text = Variable.SYS_MSJ[319,Variable.idioma].Trim();
            this.tbxScroll3.Text = Variable.SYS_MSJ[320,Variable.idioma].Trim();
            this.tbxScroll4.Text = Variable.SYS_MSJ[321,Variable.idioma].Trim();
            this.tbxScroll5.Text = Variable.SYS_MSJ[322,Variable.idioma].Trim();
            this.tbxSplash1.Text = Application.StartupPath + "\\images\\Splash\\splash1.jpg";
            this.tbxSplash2.Text = Application.StartupPath + "\\images\\Splash\\splash2.jpg";
            this.tbxSplash3.Text = Application.StartupPath + "\\images\\Splash\\splash3.jpg";
            this.tbxSplash4.Text = Application.StartupPath + "\\images\\Splash\\splash4.jpg";
            this.tbxSplash5.Text = Application.StartupPath + "\\images\\Splash\\splash5.jpg";
            this.tbxPubli1.Text = Application.StartupPath + "\\images\\Ads\\Add1.jpg";
            this.tbxPubli2.Text = Application.StartupPath + "\\images\\Ads\\Add2.jpg";
            this.tbxPubli3.Text = Application.StartupPath + "\\images\\Ads\\Add3.jpg";
            this.tbxPubli4.Text = Application.StartupPath + "\\images\\Ads\\Add4.jpg";
            this.tbxPubli5.Text = Application.StartupPath + "\\images\\Ads\\Add5.jpg";
            this.tbxLogoScreen.Text = Application.StartupPath + "\\images\\Logos\\LogoTorrey.jpg";
            this.tbxLogoPrint.Text = Application.StartupPath + "\\images\\Logos\\LogoTorrey.bmp";
            this.tbxPswSupervisor.Text = "1234";
            this.tbxPswAdmin.Text = "123456";
            this.trackTiempo.Value = 1;
            this.trackTiempoPubli.Value = 5;
            this.lbxCetiqueta.Text = "1";
            this.lblTetiqueta.Text = "5";
            this.chkActivaProtect.Checked = true;
            this.chkActivaPubli.Checked = true;
            this.chkvarios.Checked = true;
            this.chkdescuento.Checked = true;
            this.chkdevolucion.Checked =true;
            this.chkblqprecio.Checked = false;
            this.chkautoimp.Checked = true;
            this.chkreimpre.Checked = false;
            this.rbtnf1fecha.Checked = true;
            this.rbtnf2fecha.Checked = false;
            this.rbtnf1hora.Checked = false;
            this.rbtnf2hora.Checked = true;

            this.tbxScroll1.Focus();

            Form1.statRegistro = ESTADO.EstadoRegistro.PKNOTRATADO;
        }

        private void Guardar(bool Existe)
        {
            object[] clave = new object[2];
            clave[0] = Num_Bascula;
            clave[1] = Num_Grupo;

            string fecha_act = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);
            tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);

            if (!Existe)
            {
                DataRow dr = baseDeDatosDataSet.Configuracion.NewRow();

                dr.BeginEdit();
                dr["id_bascula"] = Num_Bascula;
                dr["id_grupo"] = Num_Grupo;
                dr["publicidad1"] = tbxScroll1.Text;
                dr["publicidad2"] = tbxScroll2.Text;
                dr["publicidad3"] = tbxScroll3.Text;
                dr["publicidad4"] = tbxScroll4.Text;
                dr["publicidad5"] = tbxScroll5.Text;
                dr["splash1"] = tbxSplash1.Text;
                dr["splash2"] = tbxSplash2.Text;
                dr["splash3"] = tbxSplash3.Text;
                dr["splash4"] = tbxSplash4.Text;
                dr["splash5"] = tbxSplash5.Text;
                dr["imgpubli1"] = tbxPubli1.Text;
                dr["imgpubli2"] = tbxPubli2.Text;
                dr["imgpubli3"] = tbxPubli3.Text;
                dr["imgpubli4"] = tbxPubli4.Text;
                dr["imgpubli5"] = tbxPubli5.Text;
                dr["logo"] = tbxLogoScreen.Text;
                dr["logoprint"] = tbxLogoPrint.Text;
                dr["pss_supervisor"] = tbxPswAdmin.Text;
                dr["pss_operario"] = tbxPswSupervisor.Text;
                dr["u_varios"] = Variable.user_varios;
                dr["p_descuentos"] = Variable.user_descuentos;
                dr["p_devoluciones"] = Variable.user_devoluciones;
                dr["bloq_precio"] = Variable.user_lockprecio;
                dr["autoprint"] = Variable.user_autoprint;
                dr["reprint"] = Variable.user_reprint;
                dr["formatofecha"] = Variable.user_formfecha;
                dr["formatohora"] = Variable.user_formhora;
                dr["activasplash"] = Variable.user_activaprotector;
                dr["activalogo"] = Variable.user_activalogo;
                dr["activapubli"] = Variable.user_activapublicidad;
                dr["tiemposplash"] = trackTiempo.Value;
                dr["tiempopubli"] = trackTiempoPubli.Value;
                dr["actualizado"] = fecha_act;
                dr["pendiente"] = true;
                dr.EndEdit();

                configuracionTableAdapter.Update(dr);
                baseDeDatosDataSet.AcceptChanges();

                Conec.CadenaSelect = "INSERT INTO Configuracion " +
                "(id_bascula, id_grupo, publicidad1, publicidad2, publicidad3, publicidad4, publicidad5, splash1, splash2,splash3,splash4,splash5,imgpubli1,imgpubli2,imgpubli3,imgpubli4,imgpubli5,logo,logoprint,pss_supervisor,pss_operario,u_varios, p_descuentos, p_devoluciones, bloq_precio, autoprint,reprint,formatofecha,formatohora,actualizado,pendiente,activasplash,activalogo,activapubli,tiemposplash,tiempopubli)" +
               "VALUES (" + Num_Bascula + "," +
               Num_Grupo + ",'" +
               tbxScroll1.Text + "','" +
               tbxScroll2.Text + "','" +
               tbxScroll3.Text + "','" +
               tbxScroll4.Text + "','" +
               tbxScroll5.Text + "','" +
               tbxSplash1.Text + "','" +
               tbxSplash2.Text + "','" +
               tbxSplash3.Text + "','" +
               tbxSplash4.Text + "','" +
               tbxSplash5.Text + "','" +
               tbxPubli1.Text + "','" +
               tbxPubli2.Text + "','" +
               tbxPubli3.Text + "','" +
               tbxPubli4.Text + "','" +
               tbxPubli5.Text + "','" +
               tbxLogoScreen.Text + "','" +
               tbxLogoPrint.Text + "','" +
               tbxPswAdmin.Text + "','" +
               tbxPswSupervisor.Text + "'," +
               Variable.user_varios + "," +
               Variable.user_descuentos + "," +
               Variable.user_devoluciones + "," +
               Variable.user_lockprecio + "," +
               Variable.user_autoprint + "," +
               Variable.user_reprint + "," +
               Variable.user_formfecha + "," +
               Variable.user_formhora + ",'" +
               fecha_act + "'," +
               true + "," +
               Variable.user_activaprotector + "," +
               Variable.user_activalogo + "," +
               Variable.user_activapublicidad + "," +
               trackTiempo.Value + "," +
               trackTiempoPubli.Value + ")";

                Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Configuracion.TableName);

            }
            if (Existe)
            {
                DataRow dr = baseDeDatosDataSet.Configuracion.Rows.Find(clave);

                dr.BeginEdit();
                dr["publicidad1"] = tbxScroll1.Text;
                dr["publicidad2"] = tbxScroll2.Text;
                dr["publicidad3"] = tbxScroll3.Text;
                dr["publicidad4"] = tbxScroll4.Text;
                dr["publicidad5"] = tbxScroll5.Text;
                dr["splash1"] = tbxSplash1.Text;
                dr["splash2"] = tbxSplash2.Text;
                dr["splash3"] = tbxSplash3.Text;
                dr["splash4"] = tbxSplash4.Text;
                dr["splash5"] = tbxSplash5.Text;
                dr["imgpubli1"] = tbxPubli1.Text;
                dr["imgpubli2"] = tbxPubli2.Text;
                dr["imgpubli3"] = tbxPubli3.Text;
                dr["imgpubli4"] = tbxPubli4.Text;
                dr["imgpubli5"] = tbxPubli5.Text;
                dr["logo"] = tbxLogoScreen.Text;
                dr["logoprint"] = tbxLogoPrint.Text;
                dr["pss_supervisor"] = tbxPswAdmin.Text;
                dr["pss_operario"] = tbxPswSupervisor.Text;
                dr["u_varios"] = Variable.user_varios;
                dr["p_descuentos"] = Variable.user_descuentos;
                dr["p_devoluciones"] = Variable.user_devoluciones;
                dr["bloq_precio"] = Variable.user_lockprecio;
                dr["autoprint"] = Variable.user_autoprint;
                dr["reprint"] = Variable.user_reprint;
                dr["formatofecha"] = Variable.user_formfecha;
                dr["formatohora"] = Variable.user_formhora;
                dr["activasplash"] = Variable.user_activaprotector;
                dr["activalogo"] = Variable.user_activalogo;
                dr["activapubli"] = Variable.user_activapublicidad;
                dr["tiemposplash"] = trackTiempo.Value;
                dr["tiempopubli"] = trackTiempoPubli.Value;
                dr["actualizado"] = fecha_act;
                dr["pendiente"] = true;
                dr.EndEdit();

                baseDeDatosDataSet.AcceptChanges();

                Conec.Condicion = "id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo;
                Conec.CadenaSelect = "UPDATE Configuracion SET " +
                "publicidad1 = '" + tbxScroll1.Text + "'," +
                "publicidad2 = '" + tbxScroll2.Text + "'," +
                "publicidad3 = '" + tbxScroll3.Text + "'," +
                "publicidad4 = '" + tbxScroll4.Text + "'," +
                "publicidad5 = '" + tbxScroll5.Text + "'," +
                "splash1 = '" + tbxSplash1.Text + "'," +
                "splash2= '" + tbxSplash2.Text + "'," +
                "splash3 = '" + tbxSplash3.Text + "'," +
                "splash4 = '" + tbxSplash4.Text + "'," +
                "splash5 = '" + tbxSplash5.Text + "'," +
                "imgpubli1 = '" + tbxPubli1.Text + "'," +
                "imgpubli2 = '" + tbxPubli2.Text + "'," +
                "imgpubli3 = '" + tbxPubli3.Text + "'," +
                "imgpubli4 = '" + tbxPubli4.Text + "'," +
                "imgpubli5 = '" + tbxPubli5.Text + "'," +
                "logo = '" + tbxLogoScreen.Text + "'," +
                "logoprint = '" + tbxLogoPrint.Text + "'," +
                "pss_supervisor = '" + tbxPswAdmin.Text + "'," +
                "pss_operario = '" + tbxPswSupervisor.Text + "'," +
                "u_varios = " + Variable.user_varios + "," +
                "p_descuentos = " + Variable.user_descuentos + "," +
                "p_devoluciones = " + Variable.user_devoluciones + "," +
                "bloq_precio = " + Variable.user_lockprecio + "," +
                "autoprint = " + Variable.user_autoprint + "," +
                "reprint = " + Variable.user_reprint + "," +
                "formatofecha = " + Variable.user_formfecha + "," +
                "formatohora = " + Variable.user_formhora + "," +
                "pendiente = " + true + "," +
                "actualizado = '" + fecha_act + "'," +
                "activasplash = " + Variable.user_activaprotector + "," +
                "activalogo = " + Variable.user_activalogo + "," +
                "activapubli = " + Variable.user_activapublicidad + "," +
                "tiemposplash = " + trackTiempo.Value + "," +
                "tiempopubli = " + trackTiempoPubli.Value + " " +
                "WHERE (" + Conec.Condicion + ")";

                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Configuracion.TableName);
                configuracionTableAdapter.Fill(baseDeDatosDataSet.Configuracion);
            }

        }



        void SendWLABEL()
        {

        }
        
        
        #endregion

        #region Botones del ToolStripMenu

        private void StripEnviar_Click(object sender, EventArgs e)
        {
            int BasculasActualizadas = 0;
            int NumeroDeBaculas = 0;
            configuracionTableAdapter.Fill(baseDeDatosDataSet.Configuracion);
            if (Num_Grupo != 0)
            {
                for (int pos = 0; pos < myScale.Length; pos++)  //Num de Basculas en el grupo
                    if (myScale[pos].gpo == Num_Grupo) NumeroDeBaculas++;

                for (int pos = 0; pos < myScale.Length; pos++)
                {
                    if (myScale[pos].gpo == Num_Grupo)
                    {
                        Variable.IP_Address = myScale[pos].ip;  //direccion ip de la bascula
                        Variable.nidbas = myScale[pos].idbas;   //numero id de la bascula
                        Variable.Nserie = myScale[pos].nserie;  //numero de serie de la bascula                    
                        Variable.Bascula = myScale[pos].nombre;  //mombre de la bascula 
                        Variable.Nombre = myScale[pos].modelo; // descripcion o modelo de la bascula
                        Variable.nsucursal = myScale[pos].gpo;  //Grupo al que pertenece la bascula
                        Variable.ppeso = myScale[pos].um;
                        Variable.P_COMM = myScale[pos].pto;
                        Variable.Buad = myScale[pos].baud;
                        BasculasActualizadas++;

                        if (myScale[pos].tipo == (int)ESTADO.tipoConexionesEnum.PKWIFI)
                        {
                            if (EnviaDatosA_Bascula(myScale[pos].ip, Variable.Nombre))  //, Num_gpo, Num_basc);
                            {
                                BasculasActualizadas--;
                                if (MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma] + " "
                                    + myScale[pos].nserie + " " + Variable.SYS_MSJ[417, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma],
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No) break;
                            }
                        }
                        else
                        {
                            if (EnviaDatosA_Bascula(ref serialPort1, myScale[pos].pto, myScale[pos].baud, Variable.Nombre))
                            {
                                BasculasActualizadas--;
                                if (MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma] + " "
                                    + myScale[pos].nserie + " " + Variable.SYS_MSJ[417, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma],
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No) break;
                            }
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
                        Variable.IP_Address = myScale[pos].ip;  //direccion ip de la bascula
                        Variable.nidbas = myScale[pos].idbas;   //numero id de la bascula
                        Variable.Nserie = myScale[pos].nserie;  //numero de serie de la bascula                    
                        Variable.Bascula = myScale[pos].nombre;  //mombre de la bascula 
                        Variable.Nombre = myScale[pos].modelo; // descripcion o modelo de la bascula
                        Variable.nsucursal = myScale[pos].gpo;  //Grupo al que pertenece la bascula
                        Variable.ppeso = myScale[pos].um;
                        Variable.P_COMM = myScale[pos].pto;
                        Variable.Buad = myScale[pos].baud;
                        BasculasActualizadas++;

                        if (myScale[pos].tipo == (int)ESTADO.tipoConexionesEnum.PKWIFI)
                        {
                            if (EnviaDatosA_Bascula(myScale[pos].ip, Variable.Nombre))  //, Num_gpo, Num_basc);
                            {
                                BasculasActualizadas--;
                                MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma]
                                    + " " + myScale[pos].nserie + ".", Variable.SYS_MSJ[42, Variable.idioma],
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                break;
                            }
                        }
                        else
                        {
                            if (EnviaDatosA_Bascula(ref serialPort1, myScale[pos].pto, myScale[pos].baud, Variable.Nombre))
                            {
                                BasculasActualizadas--;
                                MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma]
                                    + " " + myScale[pos].nserie + ".", Variable.SYS_MSJ[42, Variable.idioma],
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                break;
                            }
                        }

                        break;
                    }
                }
            }

            MessageBox.Show(this, Variable.SYS_MSJ[418, Variable.idioma] + " " + BasculasActualizadas + " "
                + Variable.SYS_MSJ[419, Variable.idioma] + " " + NumeroDeBaculas,
                Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void StripCerrar_Click(object sender, EventArgs e)
        {
            ToolStrip menugral =  ToolStripManager.FindToolStrip("toolStrip3");
            ToolStripManager.RevertMerge(menugral);
            this.Dispose();
        }
        private void StripGuardar_Click(object sender, EventArgs e)
        {
            bool Existe = false;

            Conec.CadenaSelect = "SELECT * FROM Configuracion";
            configuracionTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            configuracionTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            configuracionTableAdapter.Fill(baseDeDatosDataSet.Configuracion);

            DataRow[] DRA = baseDeDatosDataSet.Configuracion.Select("id_bascula = " + Num_Bascula + "AND id_grupo = " + Num_Grupo);

            if (DRA.Length > 0){Existe = true;}

            Guardar(Existe);
            MessageBox.Show(this, Variable.SYS_MSJ[48, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
            activarDesactivarEdicion(false, Color.WhiteSmoke);
            Consulta_EnBD();

            StripEditar.Enabled = true;
            StripBorrar.Enabled = true;
            StripGuardar.Enabled = false;
            StripEnviar.Enabled = true;
        }
        private void StripBorrar_Click(object sender, EventArgs e)
        {
            bool existe = false;

            /*OleDbDataReader Dconfig = Conec.Obtiene_Dato("Select * From Configuracion Where id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo, Conec.CadenaConexion);
            if (Dconfig.Read()) existe = true;
            else existe = false;
            Dconfig.Close();*/

            Conec.CadenaSelect = "SELECT * FROM Configuracion";
            configuracionTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            configuracionTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            configuracionTableAdapter.Fill(baseDeDatosDataSet.Configuracion);

            DataRow[] DRA = baseDeDatosDataSet.Configuracion.Select("id_bascula = " + Num_Bascula + "AND id_grupo = " + Num_Grupo);

            if (DRA.Length > 0)
            {
                existe = true;
            }

            if (existe)
            {
                Conec.Condicion = "id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo;

                Conec.CadenaSelect = "DELETE * FROM Configuracion WHERE (" + Conec.Condicion + ")";
                Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Configuracion.TableName);
              
                limpiezaTextBoxes();
                StripEditar.Enabled = false;
                StripBorrar.Enabled = false;
                StripGuardar.Enabled = true;
                StripEnviar.Enabled = false;
            }
        }
        private void StripEditar_Click(object sender, EventArgs e)
        {
            activarDesactivarEdicion(true, Color.White);
            Consulta_EnBD();
            StripEditar.Enabled = false;
            StripBorrar.Enabled = true;
            StripGuardar.Enabled = true;
            StripEnviar.Enabled = false;
        }

        private void btnExam1_Click(object sender, EventArgs e)
        {
            string text = Asigna_Nombre_Imagen("jpg", 0);
            if (text.Length > 0)
                tbxSplash1.Text = text;
        }
        private void btnExam2_Click(object sender, EventArgs e)
        {
            string text = Asigna_Nombre_Imagen("jpg", 0);
            if (text.Length > 0)
                tbxSplash2.Text = text;
        }
        private void btnExam3_Click(object sender, EventArgs e)
        {
            string text = Asigna_Nombre_Imagen("jpg", 0);
            if (text.Length > 0)
                tbxSplash3.Text = text;
        }
        private void btnExam4_Click(object sender, EventArgs e)
        {
            string text = Asigna_Nombre_Imagen("jpg", 0);
            if (text.Length > 0)
                tbxSplash4.Text = text;
        }
        private void btnExam5_Click(object sender, EventArgs e)
        {
            string text = Asigna_Nombre_Imagen("jpg", 0);
            if (text.Length > 0)
                tbxSplash5.Text = text;
        }
        private void btnExam6_Click(object sender, EventArgs e)
        {
            string text = Asigna_Nombre_Imagen("jpg", 1);
            if (text.Length > 0)
                tbxPubli1.Text = text;
        }
        private void btnExam7_Click(object sender, EventArgs e)
        {
            string text = Asigna_Nombre_Imagen("jpg", 1);
            if (text.Length > 0)
                tbxPubli2.Text = text;
        }
        private void btnExam8_Click(object sender, EventArgs e)
        {
            string text = Asigna_Nombre_Imagen("jpg", 1);
            if (text.Length > 0)
                tbxPubli3.Text = text;
        }
        private void btnExam9_Click(object sender, EventArgs e)
        {
            string text = Asigna_Nombre_Imagen("jpg", 1);
            if (text.Length > 0)
                tbxPubli4.Text = text;
        }
        private void btnExam10_Click(object sender, EventArgs e)
        {
            string text = Asigna_Nombre_Imagen("jpg", 1);
            if (text.Length > 0)
                tbxPubli5.Text = text;
        }
        private void btnExam11_Click(object sender, EventArgs e)
        {
            string text = Asigna_Nombre_Imagen("jpg", 2);
            if (text.Length > 0)
                tbxLogoScreen.Text = text;
        }
        private void btnExam12_Click(object sender, EventArgs e)
        {
            string text = Asigna_Nombre_Imagen("bmp", 2);
            if (text.Length > 0)
                tbxLogoPrint.Text = text;
        }

        private string Asigna_Nombre_Imagen(string formato, int tipo) //0 - splash, 1 - ads, 2 - logos
        {
            if (tipo == 0)
                this.openFileDialog1.InitialDirectory = Variable.appPath + "\\images\\Splash";
            else if (tipo == 1)
                this.openFileDialog1.InitialDirectory = Variable.appPath + "\\images\\Ads";
            else if (tipo == 2)
                this.openFileDialog1.InitialDirectory = Variable.appPath + "\\images\\Logos";

            string File_temporal = "";
            string fn = "";

            this.openFileDialog1.FileName = "";
            this.openFileDialog1.DefaultExt = formato;
            this.openFileDialog1.Filter = "Image Files(*." + formato + ")|*." + formato + "|All files (*.*)|*.*";
    AbrirDialogo:
            DialogResult result = this.openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                fn = openFileDialog1.FileName;
                if (string.Compare(formato, "bmp") == 0)
                {
                    if (string.Compare(fn.Substring(fn.Length - 3, 3), formato) != 0)
                    {
                        MessageBox.Show(this, "Seleccione un archivo con formato .bmp");
                        goto AbrirDialogo;
                    }
                }
                File_temporal = this.openFileDialog1.FileName;
            }
            return File_temporal;
        }
        #endregion

        #region Envio de informacion a basculas
        private bool EnviaDatosA_Bascula(string direccionIP, string modelo)
        {
            bool ERROR = false;

            ProgressContinue pro = new ProgressContinue();
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                
                Cliente_bascula = Cte.conectar(direccionIP, 50036);
                if (Cliente_bascula != null)
                {
                    string sComando = "XX" + (char)9 + (char)10;
                    string Msj_recibido = Env.Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");              

                    if (Msj_recibido != null)
                    {
                        if (Cliente_bascula.Connected == true)
                        {
                            Enviar_Imagenes_Splash(direccionIP, ref Cliente_bascula, ref pro);
                            if (modelo == "WLSD") Enviar_Imagenes_Publicidad(direccionIP, ref Cliente_bascula, ref pro);
                            Enviar_Imagenes_Logo(direccionIP, ref Cliente_bascula, ref pro);
                            Enviar_Configuracion(direccionIP, ref Cliente_bascula, ref pro);
                            //if (modelo == "WLSD") 
                            Enviar_Configuracion2(direccionIP, ref Cliente_bascula, ref pro);
                        }
                        
                    }
                    
                    Cte.desconectar(ref Cliente_bascula);
                }
                else
                {
                    ERROR =  true;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Variable.SYS_MSJ[381, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                ERROR = true;
            }

            pro.TerminaProcess();
            Cursor.Current = Cursors.Default;
            Thread.Sleep(500);
            return ERROR;
        }

        private bool EnviaDatosA_Bascula(ref SerialPort serialPort1, string puerto, Int32 baud_rate, string modelo)
        {
            
            serialPort1 = new SerialPort();
            bool ERROR = false;

            ProgressContinue pro = new ProgressContinue();
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (SR.OpenPort(ref serialPort1, puerto, baud_rate))
                {

                    SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)9 + (char)10);
                    SR.ReceivedCOMSerial(ref serialPort1);

                    Enviar_Configuracion(ref serialPort1, ref pro);
                    if (modelo == "WLSD") Enviar_Configuracion2(ref serialPort1, ref pro);
                    SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)9 + (char)10);
                    SR.ReceivedCOMSerial(ref serialPort1);

                    SR.ClosePort(ref serialPort1);

                    //Se tienen que enviar las imágenes después de cerrar el puerto serie porque la rutina de envío de imágenes
                    //utiliza su propio puerto serie.
                    Enviar_Imagenes_Splash(ref serialPort1, ref pro);
                    //Bloquear_Báscula(puerto, baud_rate);
                    Enviar_Imagenes_Logo(ref serialPort1, ref pro);
                    //Bloquear_Báscula(puerto, baud_rate);
                    if (modelo == "WLSD") Enviar_Imagenes_Publicidad(ref serialPort1, ref pro);
                    serialPort1 = new SerialPort();
                    if (SR.OpenPort(ref serialPort1, puerto, baud_rate))
                    {
                        SR.SendCOMSerial(ref serialPort1, "DXXX" + (char)9 + (char)10);
                        SR.ReceivedCOMSerial(ref serialPort1);
                        SR.ClosePort(ref serialPort1);
                    }
                }
                else
                {
                    ERROR = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Variable.SYS_MSJ[381, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                ERROR = true;
            }

            pro.TerminaProcess();
            Cursor.Current = Cursors.Default;
            Thread.Sleep(500);
            return ERROR;
        }

        private void Bloquear_Báscula(string puerto, int baud_rate)
        {
            serialPort1 = new SerialPort();
            if (SR.OpenPort(ref serialPort1, puerto, baud_rate))
            {
                SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)9 + (char)10);
                SR.ReceivedCOMSerial(ref serialPort1);
                SR.ClosePort(ref serialPort1);
            }
        }

        private void Enviar_Imagenes_Splash(string direccionIP, ref Socket Cliente_bascula, ref ProgressContinue pro)
        {
            int iRtaFunct = 0;
            int posicion = 0;
            string ImagenAEnviar = "";            
            object[] Datos_Splash;
            Console.WriteLine("{0}(): IN", System.Reflection.MethodBase.GetCurrentMethod().Name);
            CommandTorrey myobj = new CommandTorrey();            
           
            OleDbDataReader OlSplash = Conec.Obtiene_Dato("Select splash1,splash2,splash3,splash4,splash5 From Configuracion Where (id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo + ")", Conec.CadenaConexion);
            if (OlSplash.Read())
            {
                Datos_Splash = new object[OlSplash.FieldCount];
                OlSplash.GetValues(Datos_Splash);
                OlSplash.Close();

                int iNumberImage = 0;

                for (int i = 0; i < Datos_Splash.Length; i++)
                {
                    posicion = Datos_Splash[i].ToString().LastIndexOf('\\');
                    if (posicion > 0)
                    {
                        iNumberImage++;
                    }
                }
                
                pro.IniciaProcess(iNumberImage, Variable.SYS_MSJ[429, Variable.idioma]);

                for (int i = 0; i < Datos_Splash.Length; i++)
                {
                    posicion = Datos_Splash[i].ToString().LastIndexOf('\\');
                    if (posicion > 0)
                    {
                        ImagenAEnviar = Datos_Splash[i].ToString();

                        iRtaFunct = myobj.TORREYSendImagesToScale(direccionIP, ImagenAEnviar, "Splash", Cliente_bascula);
                        
                        /*if (iRtaFunct > 0) pro.UpdateProcess(1, Variable.SYS_MSJ[276, Variable.idioma] + " " + ImagenAEnviar);
                        else pro.UpdateProcess(1, Variable.SYS_MSJ[275, Variable.idioma] + " " + ImagenAEnviar);*/

                        pro.UpdateProcess(1, Variable.SYS_MSJ[429, Variable.idioma]);
                    }
                }
            }
            else OlSplash.Close();            
        }
        private void Enviar_Imagenes_Splash(ref SerialPort Cliente_bascula, ref ProgressContinue pro)
        {
            int iRtaFunct = 0;
            int posicion = 0;
            string ImagenAEnviar = "";           
            object[] Datos_Splash;

            CommandTorrey myobj = new CommandTorrey();
            
            OleDbDataReader OlSplash = Conec.Obtiene_Dato("Select splash1,splash2,splash3,splash4,splash5 From Configuracion Where (id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo + ")", Conec.CadenaConexion);
            if (OlSplash.Read())
            {
                Datos_Splash = new object[OlSplash.FieldCount];
                OlSplash.GetValues(Datos_Splash);
                OlSplash.Close();

                int iNumberImage = 0;

                for (int i = 0; i < Datos_Splash.Length; i++)
                {
                    posicion = Datos_Splash[i].ToString().LastIndexOf('\\');
                    if (posicion > 0)
                    {
                        iNumberImage++;
                    }
                }

                pro.IniciaProcess(iNumberImage, Variable.SYS_MSJ[429, Variable.idioma]);

                for (int i = 0; i < Datos_Splash.Length; i++)
                {
                    posicion = Datos_Splash[i].ToString().LastIndexOf('\\');
                    if (posicion > 0)
                    {
                        ImagenAEnviar = Datos_Splash[i].ToString();

                        iRtaFunct = myobj.TORREYSendImagesToScaleSerial(Variable.P_COMM, ImagenAEnviar, "Splash");

                        /*if (iRtaFunct > 0) pro.UpdateProcess(1, Variable.SYS_MSJ[276, Variable.idioma] + " " + ImagenAEnviar);
                        else pro.UpdateProcess(1, Variable.SYS_MSJ[275, Variable.idioma] + " " + ImagenAEnviar);*/

                        pro.UpdateProcess(1, Variable.SYS_MSJ[429, Variable.idioma]);
                    }
                }
            }
            else OlSplash.Close();            
        }
        private void Enviar_Imagenes_Publicidad(string direccionIP, ref Socket Cliente_bascula, ref ProgressContinue pro)
        {
            int iRtaFunct = 0;
            int posicion = 0;
            string ImagenAEnviar = "";
            object[] Datos_Publi;

            Console.WriteLine("{0}(): IN", System.Reflection.MethodBase.GetCurrentMethod().Name);
            CommandTorrey myobj = new CommandTorrey();

            OleDbDataReader OlPubli = Conec.Obtiene_Dato("Select imgpubli1,imgpubli2,imgpubli3,imgpubli4,imgpubli5 From Configuracion Where (id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo + ")", Conec.CadenaConexion);
            if (OlPubli.Read())
            {
                Datos_Publi = new object[OlPubli.FieldCount];
                OlPubli.GetValues(Datos_Publi);
                OlPubli.Close();

                int iNumberImage = 0;

                for (int i = 0; i < Datos_Publi.Length; i++)
                {
                    posicion = Datos_Publi[i].ToString().LastIndexOf('\\');
                    if (posicion > 0)
                    {
                        iNumberImage++;
                    }
                }

                pro.IniciaProcess(iNumberImage, Variable.SYS_MSJ[431, Variable.idioma]);

                for (int i = 0; i < Datos_Publi.Length; i++)
                {
                    posicion = Datos_Publi[i].ToString().LastIndexOf('\\');
                    if (posicion > 0)
                    {
                        ImagenAEnviar = Datos_Publi[i].ToString();

                        iRtaFunct = myobj.TORREYSendImagesToScale(direccionIP, ImagenAEnviar, "Ad", Cliente_bascula);

                        /*if (iRtaFunct > 0) pro.UpdateProcess(1, Variable.SYS_MSJ[276, Variable.idioma] + " " + ImagenAEnviar);
                        else pro.UpdateProcess(1, Variable.SYS_MSJ[275, Variable.idioma] + " " + ImagenAEnviar);*/

                        pro.UpdateProcess(1, Variable.SYS_MSJ[431, Variable.idioma]);
                    }
                }
            }
            else OlPubli.Close();
        }
        private void Enviar_Imagenes_Publicidad(ref SerialPort Cliente_bascula, ref ProgressContinue pro)
        {
            int iRtaFunct = 0;
            int posicion = 0;
            string ImagenAEnviar = "";
            object[] Datos_Publi;

            Console.WriteLine("{0}(): IN", System.Reflection.MethodBase.GetCurrentMethod().Name);
            CommandTorrey myobj = new CommandTorrey();

            OleDbDataReader OlPubli = Conec.Obtiene_Dato("Select imgpubli1,imgpubli2,imgpubli3,imgpubli4,imgpubli5 From Configuracion Where (id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo + ")", Conec.CadenaConexion);
            if (OlPubli.Read())
            {
                Datos_Publi = new object[OlPubli.FieldCount];
                OlPubli.GetValues(Datos_Publi);
                OlPubli.Close();

                int iNumberImage = 0;

                for (int i = 0; i < Datos_Publi.Length; i++)
                {
                    posicion = Datos_Publi[i].ToString().LastIndexOf('\\');
                    if (posicion > 0)
                    {
                        iNumberImage++;
                    }
                }

                pro.IniciaProcess(iNumberImage, Variable.SYS_MSJ[431, Variable.idioma]);

                for (int i = 0; i < Datos_Publi.Length; i++)
                {
                    posicion = Datos_Publi[i].ToString().LastIndexOf('\\');
                    if (posicion > 0)
                    {
                        ImagenAEnviar = Datos_Publi[i].ToString();
                        iRtaFunct = myobj.TORREYSendImagesToScaleSerial(Variable.P_COMM, ImagenAEnviar, "Ad");

                        /*if (iRtaFunct > 0) pro.UpdateProcess(1, Variable.SYS_MSJ[276, Variable.idioma] + " " + ImagenAEnviar);
                        else pro.UpdateProcess(1, Variable.SYS_MSJ[275, Variable.idioma] + " " + ImagenAEnviar);*/

                        pro.UpdateProcess(1, Variable.SYS_MSJ[431, Variable.idioma]);
                    }
                }
            }
            else OlPubli.Close();
        }
        private void Enviar_Imagenes_Logo(string direccionIP, ref Socket Cliente_bascula, ref ProgressContinue pro)
        {
            int iRtaFunct = 0;
            int posicion = 0;
            string ImagenAEnviar = "";
            object[] Datos_Logo;
            Console.WriteLine("{0}(): IN", System.Reflection.MethodBase.GetCurrentMethod().Name);

            CommandTorrey myobj = new CommandTorrey();

            OleDbDataReader OlLogo = Conec.Obtiene_Dato("Select logo,logoprint From Configuracion Where (id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo + ")", Conec.CadenaConexion);
            if (OlLogo.Read())
            {
                Datos_Logo = new object[OlLogo.FieldCount];
                OlLogo.GetValues(Datos_Logo);
                OlLogo.Close();

                int iNumberImage = 0;

                for (int i = 0; i < Datos_Logo.Length; i++)
                {
                    posicion = Datos_Logo[i].ToString().LastIndexOf('\\');
                    if (posicion > 0)
                    {
                        iNumberImage++;
                    }
                }

                pro.IniciaProcess(iNumberImage, Variable.SYS_MSJ[430, Variable.idioma]);

                for (int i = 0; i < Datos_Logo.Length; i++)
                {
                    posicion = Datos_Logo[i].ToString().LastIndexOf('\\');
                    if (posicion > 0)
                    {
                        ImagenAEnviar = Datos_Logo[i].ToString();

                        iRtaFunct = myobj.TORREYSendImagesToScale(direccionIP, ImagenAEnviar, "Logo", Cliente_bascula);

                        /*if (iRtaFunct > 0) pro.UpdateProcess(1, Variable.SYS_MSJ[276, Variable.idioma] + " " + ImagenAEnviar);
                        else pro.UpdateProcess(1, Variable.SYS_MSJ[275, Variable.idioma] + " " + ImagenAEnviar);*/

                        pro.UpdateProcess(1, Variable.SYS_MSJ[430, Variable.idioma]);
                    }
                }
            }
            else OlLogo.Close();
        }
        private void Enviar_Imagenes_Logo(ref SerialPort Cliente_bascula, ref ProgressContinue pro)
        {
            int iRtaFunct = 0;
            int posicion = 0;
            string ImagenAEnviar = "";
            object[] Datos_Logo;
            Console.WriteLine("{0}(): IN", System.Reflection.MethodBase.GetCurrentMethod().Name);

            CommandTorrey myobj = new CommandTorrey();

            OleDbDataReader OlLogo = Conec.Obtiene_Dato("Select logo,logoprint From Configuracion Where (id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo + ")", Conec.CadenaConexion);
            if (OlLogo.Read())
            {
                Datos_Logo = new object[OlLogo.FieldCount];
                OlLogo.GetValues(Datos_Logo);
                OlLogo.Close();

                int iNumberImage = 0;

                for (int i = 0; i < Datos_Logo.Length; i++)
                {
                    posicion = Datos_Logo[i].ToString().LastIndexOf('\\');
                    if (posicion > 0)
                    {
                        iNumberImage++;
                    }
                }

                pro.IniciaProcess(iNumberImage, Variable.SYS_MSJ[430, Variable.idioma]);

                for (int i = 0; i < Datos_Logo.Length; i++)
                {
                    posicion = Datos_Logo[i].ToString().LastIndexOf('\\');
                    if (posicion > 0)
                    {
                        ImagenAEnviar = Datos_Logo[i].ToString();

                        iRtaFunct = myobj.TORREYSendImagesToScaleSerial(Variable.P_COMM, ImagenAEnviar, "Logo");

                        /*if (iRtaFunct > 0) pro.UpdateProcess(1, Variable.SYS_MSJ[276, Variable.idioma] + " " + ImagenAEnviar);
                        else pro.UpdateProcess(1, Variable.SYS_MSJ[275, Variable.idioma] + " " + ImagenAEnviar);*/

                        pro.UpdateProcess(1, Variable.SYS_MSJ[430, Variable.idioma]);
                    }
                }
            }
            else OlLogo.Close();
        }
        private void Enviar_Configuracion(string direccionIP, ref Socket Cliente_bascula, ref ProgressContinue pro)
        {
            int reg_leido = 1;
            int reg_envio = 0;
            int reg_total = 0;
            string Msg_recibido;
            string Variable_frame = null;
            Console.WriteLine("{0}(): IN", System.Reflection.MethodBase.GetCurrentMethod().Name);

            DataRow[] DR_Config = baseDeDatosDataSet.Configuracion.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);
            reg_total = DR_Config.Length;
            Console.WriteLine("{0}(): Registros leidos: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, reg_total);
            Variable_frame = "";
            pro.IniciaProcess(reg_total*2, Variable.SYS_MSJ[253, Variable.idioma]);   //"Configuracion General", "Iniciando proceso");            

            foreach (DataRow DP in DR_Config)
            {
                Variable_frame = Variable_frame + Env.Genera_Trama_Config_General(DP);
                reg_envio++;
                pro.UpdateProcess(1, Variable.SYS_MSJ[253, Variable.idioma]);
                Msg_recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo, "GG");
                pro.UpdateProcess(1, Variable.SYS_MSJ[253, Variable.idioma]);
                reg_leido = 1;
                Variable_frame = "";
            }
            Console.WriteLine("{0}(): OUT", System.Reflection.MethodBase.GetCurrentMethod().Name);
        }
        private void Enviar_Configuracion(ref SerialPort Cliente_bascula, ref ProgressContinue pro)
        {
            int reg_leido = 1;
            int reg_envio = 0;
            int reg_total = 0;
            string strcomando;
            string Variable_frame = null;
            string[] Dato_recibido = null;

            DataRow[] DR_Config = baseDeDatosDataSet.Configuracion.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);
            reg_total = DR_Config.Length;

            Variable_frame = "";
            pro.IniciaProcess(reg_total, Variable.SYS_MSJ[253, Variable.idioma]);  

            foreach (DataRow DP in DR_Config)
            {
                Variable_frame = Variable_frame + Env.Genera_Trama_Config_General(DP);
                reg_envio++;
                strcomando = "GG" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                SR.SendCOMSerial(ref Cliente_bascula, strcomando, ref Dato_recibido);
                if (Dato_recibido[0].IndexOf("Error") >= 0)
                {
                    Env.Guardar_Trama_pendiente(Num_Bascula, Num_Grupo, strcomando);
                }

                pro.UpdateProcess(1, Variable.SYS_MSJ[253, Variable.idioma]);
                reg_leido = 1;
                Variable_frame = "";
            }
        }
        private void Enviar_Configuracion2(string direccionIP, ref Socket Cliente_bascula, ref ProgressContinue pro)
        {
            int reg_leido = 1;
            int reg_envio = 0;
            int reg_total = 0;
            string Msg_recibido;
            string Variable_frame = null;
            Console.WriteLine("{0}(): IN", System.Reflection.MethodBase.GetCurrentMethod().Name);

            DataRow[] DR_Config = baseDeDatosDataSet.Configuracion.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);
            reg_total = DR_Config.Length;

            Console.WriteLine("{0}(): Registros leidos: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, reg_total);
            Variable_frame = "";
            pro.IniciaProcess(reg_total*2, Variable.SYS_MSJ[253, Variable.idioma]);   //"Configuracion General", "Iniciando proceso");            

            foreach (DataRow DP in DR_Config)
            {
                Variable_frame = Variable_frame + Env.Genera_Trama_Config_General2(DP);
                reg_envio++;
                pro.UpdateProcess(1, Variable.SYS_MSJ[253, Variable.idioma]);
                Msg_recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo, "Gg");
                pro.UpdateProcess(1, Variable.SYS_MSJ[253, Variable.idioma]);
                reg_leido = 1;
                Variable_frame = "";
            }
            Console.WriteLine("{0}(): OUT", System.Reflection.MethodBase.GetCurrentMethod().Name);
        }
        private void Enviar_Configuracion2(ref SerialPort Cliente_bascula, ref ProgressContinue pro)
        {
            int reg_leido = 1;
            int reg_envio = 0;
            int reg_total = 0;
            string strcomando;
            string Variable_frame = null;
            string[] Dato_recibido = null;

            DataRow[] DR_Config = baseDeDatosDataSet.Configuracion.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);
            reg_total = DR_Config.Length;

            Variable_frame = "";
            pro.IniciaProcess(reg_total, Variable.SYS_MSJ[253, Variable.idioma]);

            foreach (DataRow DP in DR_Config)
            {
                Variable_frame = Variable_frame + Env.Genera_Trama_Config_General2(DP);
                reg_envio++;
                strcomando = "Gg" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                SR.SendCOMSerial(ref Cliente_bascula, strcomando, ref Dato_recibido);
                if (Dato_recibido[0].IndexOf("Error") >= 0)
                {
                    Env.Guardar_Trama_pendiente(Num_Bascula, Num_Grupo, strcomando);
                }

                pro.UpdateProcess(1, Variable.SYS_MSJ[253, Variable.idioma]);
                reg_leido = 1;
                Variable_frame = "";
            }
        }
        

        #endregion
        
        #region Eventos de TextBoxs y Check
        private void asignarEventos()
        {           
            foreach (System.Windows.Forms.TextBox tbxScroll in tScroll)
            {
                tbxScroll.LostFocus += new EventHandler(tbxScroll_LostFocus);
                tbxScroll.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxScroll_KeyDown);
                tbxScroll.KeyPress += new KeyPressEventHandler(this.tbxScroll_KeyPress);

            }
            foreach (System.Windows.Forms.TextBox tbxSplash in tSplash)
            {
                tbxSplash.LostFocus += new EventHandler(tbxSplash_LostFocus);
                tbxSplash.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxSplash_KeyDown);
            }
        }

        private void tbxSplash_LostFocus(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox tbx_Splash = ((System.Windows.Forms.TextBox)sender);
            if (tbx_Splash.Text.Length > 0) tbx_Splash.Text = Variable.validar_salida(tbx_Splash.Text, 2);      
        }
        private void tbxScroll_LostFocus(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox tbx_Scroll = ((System.Windows.Forms.TextBox)sender);
            if (tbx_Scroll.Text.Length > 0) tbx_Scroll.Text = Variable.validar_salida(tbx_Scroll.Text, 2);
        }
        private void tbxScroll_KeyDown(object sender, KeyEventArgs e)
        {
            System.Windows.Forms.TextBox tbx_Scroll = ((System.Windows.Forms.TextBox)sender);
            if (e.KeyCode == Keys.Enter) GetNextControl(tbx_Scroll, true).Focus();        
        }
        private void tbxScroll_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) || Char.IsPunctuation(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (e.KeyChar == 'Ñ' || e.KeyChar == 'ñ')
            {
                e.Handled = true;
            }            
            else if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (e.KeyChar != (char)Keys.Enter && e.KeyChar != (char)Keys.Tab && e.KeyChar != 8)
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }
        private void tbxSplash_KeyDown(object sender, KeyEventArgs e)
        {
            System.Windows.Forms.TextBox tbx_Splash = ((System.Windows.Forms.TextBox)sender);
            if (e.KeyCode == Keys.Enter) GetNextControl(tbx_Splash, true).Focus();        
        }

        private void tbxPswAdmin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.tbxPswSupervisor.Focus();
            }
        }
        private void tbxPswSupervisor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.rbtnf1fecha.Focus();
            }
        }

        private void chkvarios_CheckedChanged(object sender, EventArgs e)
        {
            if (chkvarios.Checked) Variable.user_varios = 1;
            else Variable.user_varios = 0;
            this.chkdevolucion.Focus();           
        }
        private void chkdevolucion_CheckedChanged(object sender, EventArgs e)
        {
            if (chkdevolucion.Checked) Variable.user_devoluciones = 1;
            else Variable.user_devoluciones = 0;
            this.chkdescuento.Focus();
        }
        private void chkdescuento_CheckedChanged(object sender, EventArgs e)
        {
            if (chkdescuento.Checked) Variable.user_descuentos = 1;
            else Variable.user_descuentos = 0;
            this.chkblqprecio.Focus();
        }
        private void chkblqprecio_CheckedChanged(object sender, EventArgs e)
        {
            if (chkblqprecio.Checked) Variable.user_lockprecio = 1;
            else Variable.user_lockprecio = 0;
            this.chkautoimp.Focus();
        }
        private void chkautoimp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkautoimp.Checked) Variable.user_autoprint = 1;
            else Variable.user_autoprint = 0;
            this.chkreimpre.Focus();
        }
        private void chkreimpre_CheckedChanged(object sender, EventArgs e)
        {
            if (chkreimpre.Checked) Variable.user_reprint = 1;
            else Variable.user_reprint = 0;
            this.chkActivaProtect.Focus();
        }
        private void chkActivaProtect_CheckedChanged(object sender, EventArgs e)
        {
            if (chkActivaProtect.Checked) Variable.user_activaprotector = 1;
            else
            {
                Variable.user_activaprotector = 0;
                this.trackTiempo.Value = 1;
            }
            this.chkActivaPubli.Focus();
        }
        private void chkActivaPubli_CheckedChanged(object sender, EventArgs e)
        {
            if (chkActivaPubli.Checked) Variable.user_activapublicidad = 1;
            else
            {
                Variable.user_activapublicidad = 0;
                this.trackTiempoPubli.Value = 1;
            }
            this.trackTiempo.Focus();
        }
        private void trackTiempoPubli_Scroll(object sender, EventArgs e)
        {
            lblTetiqueta.Text = trackTiempoPubli.Value.ToString();
        }
        private void trackTiempo_Scroll(object sender, EventArgs e)
        {
            lbxCetiqueta.Text = trackTiempo.Value.ToString();
        }

        private void rbtnf1fecha_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnf1fecha.Checked) Variable.user_formfecha = 0;
           // else Variable.user_formfecha = 0;
        }
        private void rbtnf2fecha_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnf2fecha.Checked) Variable.user_formfecha = 1;
            //else Variable.user_formfecha = 0;
        }
        private void rbtnf1hora_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnf1hora.Checked) Variable.user_formhora = 0;
           // else Variable.user_formhora = 0;
        }
        private void rbtnf2hora_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnf2hora.Checked) Variable.user_formhora = 1;
           // else Variable.user_formhora = 0;
        }

        #endregion

        private void UserGeneral_Load(object sender, EventArgs e)
        {

            if (Variable.TypeScale == Variable.TipoBascula.WLABEL) 
            {
                bool existe = false;
                // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Grupo' Puede moverla o quitarla según sea necesario.
                this.grupoTableAdapter.Fill(this.baseDeDatosDataSet.Grupo);
                // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Bascula' Puede moverla o quitarla según sea necesario.
                this.basculaTableAdapter.Fill(this.baseDeDatosDataSet.Bascula);

                this.configuracionTableAdapter.Fill(this.baseDeDatosDataSet.Configuracion);

                toolStripLabel3.Text = Nombre_Select;

                Asigna_Grupo();
                Asigna_Bascula();

                OleDbDataReader Dconfig = Conec.Obtiene_Dato("Select * From Configuracion Where id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo, Conec.CadenaConexion);
                if (Dconfig.Read()) existe = true;
                else existe = false;
                Dconfig.Close();

                baseDeDatosDataSet.Configuracion.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Configuracion.id_basculaColumn, baseDeDatosDataSet.Configuracion.id_grupoColumn };
                object[] clave = new object[2] { Num_Bascula, Num_Grupo };
                if (existe)
                {
                    activarDesactivarEdicion(false, Color.WhiteSmoke);
                    Consulta_EnBD();

                    StripEditar.Enabled = true;
                    StripBorrar.Enabled = true;
                    StripGuardar.Enabled = false;
                    StripEnviar.Enabled = true;
                }
                else
                {
                    limpiezaTextBoxes();
                    StripEditar.Enabled = false;
                    StripBorrar.Enabled = false;
                    StripGuardar.Enabled = true;
                    StripEnviar.Enabled = false;
                }

                funcionWLABEL();
            }//Funcion para configurar la aplicacionpara WLABEL

            else
            {

                bool existe = false;
                // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Grupo' Puede moverla o quitarla según sea necesario.
                this.grupoTableAdapter.Fill(this.baseDeDatosDataSet.Grupo);
                // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Bascula' Puede moverla o quitarla según sea necesario.
                this.basculaTableAdapter.Fill(this.baseDeDatosDataSet.Bascula);

                this.configuracionTableAdapter.Fill(this.baseDeDatosDataSet.Configuracion);

                toolStripLabel3.Text = Nombre_Select;

                Asigna_Grupo();
                Asigna_Bascula();

                OleDbDataReader Dconfig = Conec.Obtiene_Dato("Select * From Configuracion Where id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo, Conec.CadenaConexion);
                if (Dconfig.Read()) existe = true;
                else existe = false;
                Dconfig.Close();

                baseDeDatosDataSet.Configuracion.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Configuracion.id_basculaColumn, baseDeDatosDataSet.Configuracion.id_grupoColumn };
                object[] clave = new object[2] { Num_Bascula, Num_Grupo };

                if (Variable.Habilitar_PubliWLSD == 0)
                {
                    gbxAdImages.Visible = false;
                    gbxAdsTime.Visible = false;
                    gbxScreenLogo.Visible = false;
                    chkActivaPubli.Visible = false;
                    gbxLogoPrint.Location = new Point(49, 385);
                    gbxScreenSaverTime.Location = new Point(913, 15);
                    gbxOpciones.Location = new Point(668, 15);
                }
                else
                {
                    gbxLogoPrint.Location = new Point(603, 441);
                    gbxOpciones.Location = new Point(603, 15);
                    gbxScreenSaverTime.Location = new Point(848, 15);
                    gbxAdImages.Visible = true;
                    gbxAdsTime.Visible = true;
                    gbxScreenLogo.Visible = true;
                    chkActivaPubli.Visible = true;
                }

                if (existe)
                {
                    activarDesactivarEdicion(false, Color.WhiteSmoke);
                    Consulta_EnBD();

                    StripEditar.Enabled = true;
                    StripBorrar.Enabled = true;
                    StripGuardar.Enabled = false;
                    StripEnviar.Enabled = true;
                }
                else
                {
                    limpiezaTextBoxes();
                    StripEditar.Enabled = false;
                    StripBorrar.Enabled = false;
                    StripGuardar.Enabled = true;
                    StripEnviar.Enabled = false;
                }

            }

        }
    }
}


