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
    public partial class f8Parametro : Form
    {
        #region Declaracion Class
        ADOutil Conec = new ADOutil();
        #endregion

        public f8Parametro()
        {
            InitializeComponent();
            this.tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + string.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);
            this.tbxRazon.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxRazon_KeyDown);
            this.tbxDireccion.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxDireccion_KeyDown);           
            this.LUNES.CheckStateChanged += new EventHandler(LUNES_CheckStateChanged);
            this.MARTES.CheckStateChanged += new EventHandler(MARTES_CheckStateChanged);
            this.MIERCOLES.CheckStateChanged += new EventHandler(MIERCOLES_CheckStateChanged);
            this.JUEVES.CheckStateChanged += new EventHandler(JUEVES_CheckStateChanged);
            this.VIERNES.CheckStateChanged += new EventHandler(VIERNES_CheckStateChanged);
            this.SABADO.CheckStateChanged += new EventHandler(SABADO_CheckStateChanged);
            this.DOMINGO.CheckStateChanged += new EventHandler(DOMINGO_CheckStateChanged);            
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

        private void tbxHorainicio_ValueChanged(object sender, EventArgs e)
        {
           Variable.Hora_Inicial = this.tbxHorainicio.Value.ToShortTimeString();
        }

        private void tbxHorafin_ValueChanged(object sender, EventArgs e)
        {
            Variable.Hora_final = this.tbxHorafin.Value.ToShortTimeString();
        }
        private void chkEnvio_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnvio.Checked) Variable.Activar_Frecuencia = 1;
            else Variable.Activar_Frecuencia = 0;
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
            if (rBxIdiomaForm1.Checked) Variable.idioma = 0;
        }

        private void rBxIdiomaForm2_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxIdiomaForm2.Checked) Variable.idioma = 1;
        }
        #endregion

        #region Activacion de controles en la vista
        void activarDesactivarEdicion(bool pbActivar)
        { 
            tbxRazon.Enabled = pbActivar;
            tbxDireccion.Enabled = pbActivar;
            tbxCiudad.Enabled = pbActivar;
                        
            tbxHorainicio.Enabled = pbActivar;
            tbxHorafin.Enabled = pbActivar;
            cbxFrecuencia.Enabled = pbActivar;
           
        }

       
        private void limpiezaTextBoxes()
        { 
            tbxRazon.Clear();
            tbxDireccion.Clear();
            tbxCiudad.Clear();
            chkEnvio.Checked = false;
            LUNES.Checked = false;
            MARTES.Checked = false;
            MIERCOLES.Checked = false;
            JUEVES.Checked = false;
            VIERNES.Checked = false;
            SABADO.Checked = false;
            DOMINGO.Checked = false;
            tbxHorainicio.Value = Convert.ToDateTime("8:00");
            tbxHorafin.Value = Convert.ToDateTime("18:00");
            cbxFrecuencia.SelectedIndex = 0;

            tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);
            activarDesactivarEdicion(true);
            
        }
        #endregion

        #region Registro de Base de Datos y DataSet

        private void Mostrar_Dato(ref DataRow dr)
        {

            this.tbxRazon.Text = dr["Empresa"].ToString();
            this.tbxDireccion.Text = dr["Dir1"].ToString();
            this.tbxCiudad.Text = dr["Dir2"].ToString();

            Variable.dias_semana = dr["dias"].ToString();
            Variable.Hora_Inicial = dr["H_inicial"].ToString();
            Variable.Hora_final = dr["H_final"].ToString();
            Variable.intervalo = Convert.ToInt32(dr["Intervalo"].ToString());
            Variable.pos_intervalo = Convert.ToInt16(dr["pos_inter"].ToString());

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
                if (Variable.dias_semana.IndexOf('L') >= 0)this.LUNES.Checked = true;
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
            else rBxMonedaForm2.Checked = true;
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
                Variable.dias_semana = LUNES.Tag.ToString() + MARTES.Tag.ToString() + MIERCOLES.Tag.ToString() + JUEVES.Tag.ToString() + VIERNES.Tag.ToString() + SABADO.Tag.ToString() + DOMINGO.Tag.ToString();

                if (!Existe)
                {
                    Conec.CadenaSelect = "INSERT INTO DatosGral (IPLocal,Empresa,Dir1,Dir2,H_inicial,H_final,dias,Intervalo,pos_inter,ActivarFrecuencia,UM,formato_fecha,idioma,formato_moneda)" +
                    "VALUES ( '" + ipLocal +"','" + 
                    this.tbxRazon.Text + "','" + 
                    this.tbxDireccion.Text + "','" + 
                    this.tbxCiudad.Text + "','" +
                    Variable.Hora_Inicial + "','" + 
                    Variable.Hora_final + "','" + 
                    Variable.dias_semana + "'," + 
                    Variable.list_tiempo[this.cbxFrecuencia.SelectedIndex] + "," + 
                    this.cbxFrecuencia.SelectedIndex + "," +
                   chkEnvio.Checked + "," +
                    Variable.unidad + "," +
                    Variable.ffecha + "," +
                    Variable.idioma + "," +
                    Variable.moneda + ")";

                    Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.DatosGral.TableName);
                }
                else
                {
                    Conec.Condicion = "( IPLocal = '" + ipLocal + "')";
                    Conec.CadenaSelect = "UPDATE DatosGral SET " +
                    "Empresa = '" + this.tbxRazon.Text + "'," +
                    "Dir1 = '" + this.tbxDireccion.Text + "'," +
                    "Dir2 = '" + this.tbxCiudad.Text + "'," +
                    "dias='" + Variable.dias_semana + "'," +
                    "H_inicial= '" + Variable.Hora_Inicial + "'," +
                    "H_final= '" + Variable.Hora_final + "', " +
                    "Intervalo= " + Variable.list_tiempo[this.cbxFrecuencia.SelectedIndex] + "," +
                    "pos_inter=" + this.cbxFrecuencia.SelectedIndex + "," +
                    "ActivarFrecuencia = " + chkEnvio.Checked + "," +
                    "UM = " + Variable.unidad + "," +
                    "formato_fecha = " + Variable.ffecha + "," +
                    "formato_moneda = " + Variable.moneda + "," +
                    "idioma = " + Variable.idioma + " " +
                    "WHERE " + Conec.Condicion;

                    Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.DatosGral.TableName);                  
                }
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
               
        private void f8Parametro_Load(object sender, EventArgs e)
        {
            string ipLocal = new GetIP().IPStr;
            bool existe = false;

            this.datosGralTableAdapter.Fill(this.baseDeDatosDataSet.DatosGral);
            
            OleDbDataReader olr = Conec.Obtiene_Dato("Select IPLocal from DatosGral Where IPLocal = '" + ipLocal + "'",Conec.CadenaConexion);
            if (olr.Read()) existe = true; 
            else existe=false; 
            olr.Close();

            if (!existe)
            {
                limpiezaTextBoxes();
                Form1.statRegistro = ESTADO.EstadoRegistro.PKNOTRATADO;
            }
            else
            {
                Form1.statRegistro = ESTADO.EstadoRegistro.PKPARCIAL;
                DataRow[] DR = baseDeDatosDataSet.DatosGral.Select("IPLocal = '" + ipLocal +"'");
                DataRow drr = DR[0];
                Mostrar_Dato(ref drr);
            }

            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet1.Usuarios' Puede moverla o quitarla según sea necesario.
        }
        #region funcion de botones
        
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            bool Existe;

            datosGralTableAdapter.Fill(baseDeDatosDataSet.DatosGral);
            baseDeDatosDataSet.DatosGral.PrimaryKey = new DataColumn[1] { baseDeDatosDataSet.DatosGral.IPLocalColumn };

            string ipLocal = new GetIP().IPStr;

            DataRow DR = baseDeDatosDataSet.DatosGral.Rows.Find(ipLocal);

            if (DR != null) Existe = true;
            else Existe = false;

            Guardar(Existe, ipLocal);

            this.Dispose();
            this.Close();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            activarDesactivarEdicion(true);
        }
        #endregion

        private void f8Parametro_Activated(object sender, EventArgs e)
        {
            this.Show();
        }
        
    }
}
