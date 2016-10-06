using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MainMenu
{
    public partial class UserParametro : UserControl
    {
        #region Declaracion Class
        ADOutil Conec = new ADOutil();
        Color colorbefore;
        #endregion
        public UserParametro()
        {
            InitializeComponent();
            this.tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + string.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);
            tbxHorainicio.Value = Convert.ToDateTime("8:00:00 am");
            tbxHorafin.Value = Convert.ToDateTime("18:00:00 pm");
            this.LUNES.CheckStateChanged += new EventHandler(LUNES_CheckStateChanged);
            this.MARTES.CheckStateChanged += new EventHandler(MARTES_CheckStateChanged);
            this.MIERCOLES.CheckStateChanged += new EventHandler(MIERCOLES_CheckStateChanged);
            this.JUEVES.CheckStateChanged += new EventHandler(JUEVES_CheckStateChanged);
            this.VIERNES.CheckStateChanged += new EventHandler(VIERNES_CheckStateChanged);
            this.SABADO.CheckStateChanged += new EventHandler(SABADO_CheckStateChanged);
            this.DOMINGO.CheckStateChanged += new EventHandler(DOMINGO_CheckStateChanged);
            chkEnvioProgramado.Checked = EnabledPendientes;
            groupBox8.Visible = false;
            groupBox8.Enabled = false;
        }






        private void UpdatePendientes()
        {
            if (chkEnvioProgramado.Checked) Pendientes.HabilitarPendientes(); else Pendientes.DeshabilitarPendientes();
        }

        private bool EnabledPendientes
        {
            get
            {
                return (Pendientes.StatusPendientes() == 1) ? true : false;
            }
        }

        #region Procesos pushBotones
        private void DOMINGO_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.DOMINGO.CheckState == CheckState.Checked) this.DOMINGO.Tag = "D";
            else this.DOMINGO.Tag = " ";
        }
        private void SABADO_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.SABADO.CheckState == CheckState.Checked) this.SABADO.Tag = "S";
            else this.SABADO.Tag = " ";
        }
        private void VIERNES_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.VIERNES.CheckState == CheckState.Checked) this.VIERNES.Tag = "V";
            else this.VIERNES.Tag = " ";
        }
        private void JUEVES_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.JUEVES.CheckState == CheckState.Checked) this.JUEVES.Tag = "J";
            else this.JUEVES.Tag = " ";
        }
        private void MIERCOLES_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.MIERCOLES.CheckState == CheckState.Checked) this.MIERCOLES.Tag = "M";
            else this.MIERCOLES.Tag = " ";
        }
        private void MARTES_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.MARTES.CheckState == CheckState.Checked) this.MARTES.Tag = "M";
            else this.MARTES.Tag = " ";
        }
        private void LUNES_CheckStateChanged(object sender, EventArgs e)
        {
            if (this.LUNES.CheckState == CheckState.Checked) this.LUNES.Tag = "L";
            else this.LUNES.Tag = " ";
        }
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            ToolStripManager.RevertMerge("toolStrip5");
            this.Dispose();
        }
        private void tbxRazon_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.tbxRazon.Text == "" || this.tbxRazon.Text == null)
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[77, Variable.idioma]);  //"Requiere nombre de la empresa");
                    this.tbxRazon.Focus();
                }
                else this.tbxDireccion.Focus();
            }
        }
        private void tbxRazon_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.tbxRazon.Text == "" || this.tbxRazon.Text == null)
            {
                this.tbxDireccion.Focus();
            }
        }
        private void tbxDireccion_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            { this.tbxCiudad.Focus(); }
        }
        private void tbxCiudad_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            { this.LUNES.Focus(); }
        }
        private void tbxRazon_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxRazon.BackColor;
            tbxRazon.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxDireccion_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxDireccion.BackColor;
            tbxDireccion.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxCiudad_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxCiudad.BackColor;
            tbxCiudad.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxRazon_Leave(object sender, EventArgs e)
        {
            tbxRazon.BackColor = colorbefore;
        }
        private void tbxDireccion_Leave(object sender, EventArgs e)
        {
            tbxDireccion.BackColor = colorbefore;
        }
        private void tbxCiudad_Leave(object sender, EventArgs e)
        {
            tbxCiudad.BackColor = colorbefore;
        }
        private void tbxRazon_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= 'a' && e.KeyChar <= 'z' || e.KeyChar >= 'A' && e.KeyChar <= 'Z' || e.KeyChar == 8 || e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 32)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void tbxDireccion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= 'a' && e.KeyChar <= 'z' || e.KeyChar >= 'A' && e.KeyChar <= 'Z' || e.KeyChar == 8 || e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 32)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void tbxCiudad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= 'a' && e.KeyChar <= 'z' || e.KeyChar >= 'A' && e.KeyChar <= 'Z' || e.KeyChar == 8 || e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == 32)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void tbxHorainicio_ValueChanged(object sender, EventArgs e)
        {
            Variable.Hora_Inicial = string.Format(Variable.F_Hora, this.tbxHorainicio.Value);
        }

        private void tbxHorafin_ValueChanged(object sender, EventArgs e)
        {
            Variable.Hora_final = string.Format(Variable.F_Hora, this.tbxHorafin.Value);
        }

        private void chkEnvio_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnvio.Checked) Variable.Activar_Frecuencia = 1;
            else Variable.Activar_Frecuencia = 0;
        }

        private void chkEnableAds_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnableAds.Checked) Variable.Habilitar_PubliWLSD = 1;
            else Variable.Habilitar_PubliWLSD = 0;
        }

        private void rBxFechaForm1_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxFechaForm1.Checked) Variable.ffecha = 0;
        }

        private void rBxFechaForm2_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxFechaForm2.Checked) Variable.ffecha = 1;
        }

        private void rBxMonedaForm1_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxMonedaForm1.Checked) Variable.moneda = 0;
        }

        private void rBxMonedaForm2_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxMonedaForm2.Checked) Variable.moneda = 1;
        }

        private void rBxMonedaForm3_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxMonedaForm3.Checked) Variable.moneda = 2;
        }

        private void rBxMonedaForm4_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxMonedaForm4.Checked) Variable.moneda = 3;
        }

        void asignamoneda()
        {
            if (rBxMonedaForm3.Checked) Variable.moneda = 3;
            if (rBxMonedaForm3.Checked) Variable.moneda = 2;
            if (rBxMonedaForm2.Checked) Variable.moneda = 1;
            if (rBxMonedaForm1.Checked) Variable.moneda = 0;
        }

        private void rBxUMForm1_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxUMForm1.Checked) Variable.unidad = 0;
        }

        private void rBxUMForm2_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxUMForm2.Checked) Variable.unidad = 1;
        }

        private void rBxIdiomaForm1_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxIdiomaForm1.Checked)
            {
                Variable.idioma = 0;
                rBxFechaForm1.Checked = true;
            }
        }

        private void rBxIdiomaForm2_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxIdiomaForm2.Checked)
            {
                Variable.idioma = 1;
                rBxFechaForm2.Checked = true;
            }
        }
        #endregion

        #region Activacion de controles en la vista
        void activarDesactivarEdicion(bool pbActivar)
        {
            tbxRazon.Enabled = pbActivar;
            tbxDireccion.Enabled = pbActivar;
            tbxCiudad.Enabled = pbActivar;
            chkEnvio.Enabled = pbActivar;
            chkEnvioProgramado.Enabled = pbActivar;
            this.LUNES.Enabled = pbActivar;
            this.MARTES.Enabled = pbActivar;
            this.MIERCOLES.Enabled = pbActivar;
            this.JUEVES.Enabled = pbActivar;
            this.VIERNES.Enabled = pbActivar;
            this.SABADO.Enabled = pbActivar;
            this.DOMINGO.Enabled = pbActivar;
            tbxHorainicio.Enabled = pbActivar;
            tbxHorafin.Enabled = pbActivar;
            cbxFrecuencia.Enabled = pbActivar;
            chkEnableAds.Enabled = pbActivar;
        }
        private void limpiezaTextBoxes()
        {
            tbxRazon.Clear();
            tbxDireccion.Clear();
            tbxCiudad.Clear();
            chkEnvio.Checked = true;
            chkEnableAds.Checked = true;
            LUNES.Checked = false;
            MARTES.Checked = false;
            MIERCOLES.Checked = false;
            JUEVES.Checked = false;
            VIERNES.Checked = false;
            SABADO.Checked = false;
            DOMINGO.Checked = false;
            tbxHorainicio.Value = Convert.ToDateTime("8:00:00 AM");
            tbxHorafin.Value = Convert.ToDateTime("18:00:00 PM");
            cbxFrecuencia.SelectedIndex = 5;
            tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);
           
            //Change this sentences for english version.

            //Configuracion para idioma de ingles
            //rBxFechaForm1.Checked = false;
            //rBxFechaForm2.Checked = true;
            //rBxIdiomaForm1.Checked = false;
            //rBxIdiomaForm2.Checked = true;
            //rBxMonedaForm1.Checked = false;
            //rBxMonedaForm2.Checked = false;
            //rBxMonedaForm3.Checked = false;
            //rBxMonedaForm4.Checked = true;
            //rBxUMForm1.Checked = false;
            //rBxUMForm2.Checked = true;


            //configuracion para idioma de español
            rBxFechaForm1.Checked = true;
            rBxFechaForm2.Checked = false;
            rBxIdiomaForm1.Checked = true;
            rBxIdiomaForm2.Checked = false;
            rBxMonedaForm1.Checked = true;
            rBxMonedaForm2.Checked = false;
            rBxMonedaForm3.Checked = false;
            rBxMonedaForm4.Checked = false;
            rBxUMForm1.Checked = true;
            rBxUMForm2.Checked = false;

            activarDesactivarEdicion(true);

        }
        #endregion

        #region Registro de Base de Datos y DataSet

        private void Mostrar_Dato(ref DataRow dr)
        {
            this.tbxRazon.Text = dr["Empresa"].ToString();
            this.tbxDireccion.Text = dr["Dir1"].ToString();
            this.tbxCiudad.Text = dr["Dir2"].ToString();

            if (Convert.ToBoolean(dr["ActivarFrecuencia"].ToString()))
            {
                Variable.Activar_Frecuencia = 1;
                chkEnvio.Checked = true;
            }
            else
            {
                Variable.Activar_Frecuencia = 0;
                chkEnvio.Checked = false;
            }
            if (Convert.ToBoolean(dr["HabilitarPubli"].ToString()))
            {
                Variable.Habilitar_PubliWLSD = 1;
                chkEnableAds.Checked = true;
            }
            else
            {
                Variable.Habilitar_PubliWLSD = 0;
                chkEnableAds.Checked = false;
            }
            Variable.dias_semana = dr["dias"].ToString();
            Variable.intervalo = Convert.ToInt32(dr["Intervalo"].ToString());
            Variable.pos_intervalo = Convert.ToInt16(dr["pos_inter"].ToString());
            Variable.Hora_Inicial = dr["H_inicial"].ToString();
            Variable.Hora_final = dr["H_final"].ToString();

            this.cbxFrecuencia.SelectedIndex = Variable.pos_intervalo;
            this.tbxHorainicio.Value = DateTime.Parse(Variable.Hora_Inicial);
            this.tbxHorafin.Value = DateTime.Parse(Variable.Hora_final);

            this.LUNES.Checked = false;
            this.MARTES.Checked = false;
            this.MIERCOLES.Checked = false;
            this.JUEVES.Checked = false;
            this.VIERNES.Checked = false;
            this.SABADO.Checked = false;
            this.DOMINGO.Checked = false;
            this.LUNES.Tag = " ";
            this.MARTES.Tag = " ";
            this.MIERCOLES.Tag = " ";
            this.JUEVES.Tag = " ";
            this.VIERNES.Tag = " ";
            this.SABADO.Tag = " ";
            this.DOMINGO.Tag = " ";

            if (Variable.dias_semana.Length > 0)
            {
                if (Variable.dias_semana.IndexOf('L') >= 0) this.LUNES.Checked = true;
                if (Variable.dias_semana.IndexOf('M') > 0) this.MARTES.Checked = true;
                if (Variable.dias_semana.IndexOf('M') > 0) this.MIERCOLES.Checked = true;
                if (Variable.dias_semana.IndexOf('J') > 0) this.JUEVES.Checked = true;
                if (Variable.dias_semana.IndexOf('V') > 0) this.VIERNES.Checked = true;
                if (Variable.dias_semana.IndexOf('S') > 0) this.SABADO.Checked = true;
                if (Variable.dias_semana.IndexOf('D') > 0) this.DOMINGO.Checked = true;
            }

            Variable.moneda = Convert.ToInt16(dr["formato_moneda"].ToString());
            Variable.ffecha = Convert.ToInt16(dr["formato_fecha"].ToString());
            Variable.unidad = Convert.ToInt16(dr["UM"].ToString());
            Variable.idioma = Convert.ToInt16(dr["idioma"].ToString());

            if (Variable.moneda == 0) rBxMonedaForm1.Checked = true;
            else if (Variable.moneda == 1) rBxMonedaForm2.Checked = true;
            else if (Variable.moneda == 2) rBxMonedaForm3.Checked = true;
            else if (Variable.moneda == 3) rBxMonedaForm4.Checked = true;
            if (Variable.ffecha == 0) rBxFechaForm1.Checked = true;
            else rBxFechaForm2.Checked = true;
            if (Variable.unidad == 0) rBxUMForm1.Checked = true;
            else rBxUMForm2.Checked = true;
            if (Variable.idioma == 0) rBxIdiomaForm1.Checked = true;
            else rBxIdiomaForm2.Checked = true;
        }



        private void Guardar(bool Existe, string ipLocal)
        {
            try
            {
                //asignamoneda();
                Variable.dias_semana = LUNES.Tag.ToString() + MARTES.Tag.ToString() + MIERCOLES.Tag.ToString() + JUEVES.Tag.ToString() + VIERNES.Tag.ToString() + SABADO.Tag.ToString() + DOMINGO.Tag.ToString();
                Variable.Hora_Inicial = String.Format(Variable.F_Hora, this.tbxHorainicio.Value);
                Variable.Hora_final = String.Format(Variable.F_Hora, this.tbxHorafin.Value);
                
                if (!Existe)
                {
                    Conec.CadenaSelect = "INSERT INTO DatosGral (IPLocal,Empresa,Dir1,Dir2,H_inicial,H_final,dias,Intervalo,pos_inter,ActivarFrecuencia,HabilitarPubli,UM,formato_fecha,idioma,formato_moneda)" +
                    "VALUES ( '" + ipLocal + "','" +
                    this.tbxRazon.Text + "','" +
                    this.tbxDireccion.Text + "','" +
                    this.tbxCiudad.Text + "','" +
                    String.Format(Variable.F_Hora, this.tbxHorainicio.Value) + "','" +
                    String.Format(Variable.F_Hora, this.tbxHorafin.Value) + "','" +
                    Variable.dias_semana + "'," +
                    Variable.list_tiempo[this.cbxFrecuencia.SelectedIndex] + "," +
                    this.cbxFrecuencia.SelectedIndex + "," +
                    chkEnvio.Checked + "," +
                    chkEnableAds.Checked + "," +
                    Variable.unidad + "," +
                    Variable.ffecha + "," +
                    Variable.idioma + "," +
                    Variable.moneda + ")";

                    Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.DatosGral.TableName);
                }
                else
                {
                    //  Conec.Condicion = "( IPLocal = '" + ipLocal + "')";
                    Conec.CadenaSelect = "UPDATE DatosGral SET " +
                    "IPLocal = '" + new GetIP().IPStr + "'," +
                    "Empresa = '" + this.tbxRazon.Text + "'," +
                    "Dir1 = '" + this.tbxDireccion.Text + "'," +
                    "Dir2 = '" + this.tbxCiudad.Text + "'," +
                    "dias='" + Variable.dias_semana + "'," +
                    "H_inicial= '" + String.Format(Variable.F_Hora, this.tbxHorainicio.Value) + "'," +
                    "H_final= '" + String.Format(Variable.F_Hora, this.tbxHorafin.Value) + "', " +
                    "Intervalo= " + Variable.list_tiempo[this.cbxFrecuencia.SelectedIndex] + "," +
                    "pos_inter=" + this.cbxFrecuencia.SelectedIndex + "," +
                    "ActivarFrecuencia = " + chkEnvio.Checked.ToString() + "," +
                    "HabilitarPubli = " + chkEnableAds.Checked.ToString() + "," +
                        //  "UM = " + Variable.unidad + "," +
                        // "formato_fecha = " + Variable.ffecha + "," +
                        // "formato_moneda = " + Variable.moneda + "," +
                     "idioma = " + Variable.idioma;  // +" " +
                    //  "WHERE " + Conec.Condicion;

                    Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.DatosGral.TableName);
                }
                datosGralTableAdapter.Fill(baseDeDatosDataSet.DatosGral);

            }
            catch (System.PlatformNotSupportedException explat)
            {
                MessageBox.Show(this, explat.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Form1.statRegistro = ESTADO.EstadoRegistro.PKTRATADO;

        }

        public void GuardarHabilitarPubli(bool Existe, string ipLocal)
        {
            try
            {
                if (!Existe)
                {
                    Conec.CadenaSelect = "INSERT INTO DatosGral (IPLocal,Empresa,Dir1,Dir2,H_inicial,H_final,dias,Intervalo,pos_inter,ActivarFrecuencia,HabilitarPubli,UM,formato_fecha,idioma,formato_moneda)" +
                    "VALUES (" + chkEnableAds.Checked + ")";

                    Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.DatosGral.TableName);
                }
                else
                {
                    Conec.CadenaSelect = "UPDATE DatosGral SET " +
                    "HabilitarPubli = " + chkEnableAds.Checked.ToString();

                    Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.DatosGral.TableName);
                }
                datosGralTableAdapter.Fill(baseDeDatosDataSet.DatosGral);

            }
            catch (System.PlatformNotSupportedException explat)
            {
                MessageBox.Show(this, explat.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Form1.statRegistro = ESTADO.EstadoRegistro.PKTRATADO;

        }
        #endregion



        private void UserParametro_Load(object sender, EventArgs e)
        {

            string ipLocal = new GetIP().IPStr;
            bool existe = false;
           // VerificaIdioma();



            this.datosGralTableAdapter.Fill(this.baseDeDatosDataSet.DatosGral);
            OleDbDataReader olr = Conec.Obtiene_Dato("Select * from DatosGral", Conec.CadenaConexion);
            if (olr.Read()) existe = true;
            else existe = false;
            olr.Close();

            if (!existe)
            {
                limpiezaTextBoxes();
                rBxFechaForm1.Enabled = true;
                rBxFechaForm2.Enabled = true;
                rBxIdiomaForm1.Enabled = true;
                rBxIdiomaForm2.Enabled = true;
                rBxMonedaForm1.Enabled = true;
                rBxMonedaForm2.Enabled = true;
                rBxMonedaForm3.Enabled = true;
                rBxMonedaForm4.Enabled = true;
                rBxUMForm1.Enabled = true;
                rBxUMForm2.Enabled = true;
                btnEditar.Enabled = false;
                btnGuardar.Enabled = true;
                Form1.statRegistro = ESTADO.EstadoRegistro.PKNOTRATADO;
            }

            else
            {
                Form1.statRegistro = ESTADO.EstadoRegistro.PKPARCIAL;
                DataRow[] DR = baseDeDatosDataSet.DatosGral.Select();
                DataRow drr = DR[0];
                Mostrar_Dato(ref drr);
                activarDesactivarEdicion(false);
                rBxFechaForm1.Enabled  = false;
                rBxFechaForm2.Enabled  = false;
                rBxIdiomaForm1.Enabled = false;
                rBxIdiomaForm2.Enabled = false;
                rBxMonedaForm1.Enabled = false;
                rBxMonedaForm2.Enabled = false;
                rBxMonedaForm3.Enabled = false;
                rBxMonedaForm4.Enabled = false;
                rBxUMForm1.Enabled = false;
                rBxUMForm2.Enabled = false;
                btnEditar.Enabled = true;
                btnGuardar.Enabled = false;
                btnEditar.Enabled = true;
            }
            this.tbxRazon.Focus();
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet1.Usuarios' Puede moverla o quitarla según sea necesario.


            if(Variable.TypeScale==Variable.TipoBascula.WLABEL){ funcionWLABEL();}
        }


        void funcionWLABEL()
        {
            groupBox8.Visible = false;
        }
        #region funcion de botones
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            UpdatePendientes();
            bool Existe;
            datosGralTableAdapter.Fill(baseDeDatosDataSet.DatosGral);
            baseDeDatosDataSet.DatosGral.PrimaryKey = new DataColumn[1] { baseDeDatosDataSet.DatosGral.IPLocalColumn };
            DataRow[] DR = baseDeDatosDataSet.DatosGral.Select();
            if (DR.Length > 0) Existe = true;
            else Existe = false;
            string ipLocal = new GetIP().IPStr;
            Guardar(Existe, ipLocal);
            activarDesactivarEdicion(false);
            btnEditar.Enabled = false;
            btnGuardar.Enabled = false;
            this.Dispose();
        }
        private void btnEditar_Click(object sender, EventArgs e)
        {
            activarDesactivarEdicion(true);
            btnEditar.Enabled = false;
            btnGuardar.Enabled = true;
        }
        #endregion


       
    }
}