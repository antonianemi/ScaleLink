using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using TorreyTransfer;
using TreeViewBound;

namespace MainMenu
{
    public partial class UserImpresor : UserControl
    {
        public Int32 Num_Grupo;
        public Int32 Num_Bascula;
        public String Nombre_Select;
        
        //delegate void SetTextCallback(string text);

       // Variable.lgrupo[] myGrupo;
        Variable.lbasc[] myScale;
        Variable.formato[] myImpresion;
        Variable.formato_size[] myFormato;
        private Label[] Etiquetas;

        private string[] Aaccion = new string[] { "Tamaño", "Eliminar", "Cancelar" };
        private string[] Atamaño = new string[] { "Ninguno", "1-8x16", "2-16x16", "3-16x24", "4-24x24", "5-24x32", "6-16x32" };
        private string[] Acampos = new string[] { "!-Espacio", "C-Codigo de Barras","c-Consecutivo","E-Encabezados","F-Fecha",
                                                    "f-Texto Fecha","G-Caducidad","g-Texto Caducidad","H-Hora","I-Ingredientes",
                                                    "m-Mensaje Adicional","N-Descripcion de PLU","P-Precio","p-Texto Precio",
                                                    "r-Texto Vendedor","S-Gran Total","T-Total","t-Texto Total","V-Vendedor",
                                                    "v-Numero Vendedor","W-Peso","w-Texto Peso" };
        bool formato_locked;

        ESTADO.botonesEnvioDato DatosEnviado = new ESTADO.botonesEnvioDato();
        ADOutil Conec = new ADOutil();
        Conexion Cte = new Conexion();
        ArrayList Dato_Nuevo = new ArrayList();
        Socket Cliente_bascula = null;
        Envia_Dato Env = new Envia_Dato();

        #region Inicializacion
        public UserImpresor()
        {
            InitializeComponent();                      

            Etiquetas = new System.Windows.Forms.Label[]{this.for1,this.for2,this.for3,this.for4,this.for5,this.for6,this.for7,this.for8,this.for9,this.for10,
														 this.for11,this.for12,this.for13,this.for14,this.for15,this.for16,this.for17,this.for18,this.for19,this.for20,
														 this.for21,this.for22,this.for23,this.for24,this.for25,this.for26,this.for27,this.for28,this.for29,this.for30,
														 this.for31,this.for32,this.for33,this.for34,this.for35,this.for36,this.for37,this.for38,this.for39,this.for40,
														 this.for41,this.for42,this.for43,this.for44,this.for45,this.for46,this.for47,this.for48,this.for49,this.for50,
														 this.for51,this.for52,this.for53,this.for54,this.for55,this.for56,this.for57,this.for58,this.for59,this.for60};
            asignarEventos();

            myImpresion = new Variable.formato[2];
            myFormato = new Variable.formato_size[3];
            Variable.user_codigoxticket = "pdddaatttttt";         //formato codigo de barras ticket, 14 caracteres
            Variable.user_codigoxprod = "pwwwwwtttttt"; 
            
        }
        #endregion
        
        #region Consulta y escritura de Base de Datos

       /* void Asigna_Grupo()
        {
            Conec.CadenaSelect = "SELECT * FROM Grupo ORDER BY id_grupo";

            grupoTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            grupoTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            grupoTableAdapter.Fill(baseDeDatosDataSet.Grupo);

            myGrupo = new Variable.lgrupo[baseDeDatosDataSet.Grupo.Rows.Count];
            int nitem = 0;

            foreach (DataRow dr in baseDeDatosDataSet.Grupo.Rows)
            {
                myGrupo[nitem].ngpo = dr["id_grupo"].ToString();
                myGrupo[nitem].nombre = dr["nombre_gpo"].ToString();
                nitem++;
            }
        }*/

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
                nitem++;
            }

        }
                
        private void Mostrar_Dato(DataRow[] DF)
        {
            int nposicion = 0;

            foreach (DataRow dr in DF)
            {
                myFormato[nposicion].posdef = dr["posdef"].ToString();
                myFormato[nposicion].possize = dr["possize"].ToString();
                myFormato[nposicion].largo_medio = dr["largo"].ToString();
                myFormato[nposicion].ancho_medio = dr["ancho"].ToString();
                myFormato[nposicion].separacion_medio = dr["separacion"].ToString();
                myFormato[nposicion].nencabezado = dr["Nencabezados"].ToString();
                myFormato[nposicion].ningrediente = dr["Ningredientes"].ToString();
            }            
        }
        private void Mostrar_Dato(DataRow dr)
        {
            Variable.user_EAN_UPCxProd = Convert.ToSByte(dr["barcode_prod"]);
            Variable.user_EAN_UPCxTicket = Convert.ToSByte(dr["barcode_ticket"]);
            Variable.user_nutri = Convert.ToSByte(dr["nutrientes"]);
                        
            if (Variable.user_EAN_UPCxProd == 0) rBxEanPrd.Checked = true;
            else rBxUpcPrd.Checked = true;
            if (Variable.user_EAN_UPCxTicket == 0) rBxEanTicket.Checked = true;
            else rBxUpcTicket.Checked = true;
            this.texto_codigoxprod.Text = dr["bascode_personal_prod"].ToString();
            this.texto_codigoxticket.Text = dr["bascode_personal_ticket"].ToString();
            this.texto_depto.Text = dr["departamento"].ToString();
            this.texto_prefijo.Text = dr["prefijo"].ToString();
            if (Variable.user_nutri == 1) chkNutriente.Checked = true;
            else chkNutriente.Checked = false;
            this.ScrollBarContraEtiqueta.Value = Convert.ToInt16(dr["c_etiqueta"].ToString());
            this.ScrollBarContraPapel.Value = Convert.ToInt16(dr["c_papel"].ToString());
            this.ScrollBarCorrimiento.Value = Convert.ToInt16(dr["cero_pieza"].ToString());
            this.ScrollBarRetardo.Value = Convert.ToInt16(dr["retardo"].ToString());
            if (Convert.ToSByte(dr["tipoimp"]) == 0) rBxTipopapel.Checked = true;
            else rBxTipoetiqueta.Checked = true;

            if (Convert.ToSByte(dr["formato_papel"].ToString()) == 0 || Convert.ToSByte(dr["formato_ticket"].ToString()) == 0) rBxformato1_Standar.Checked = true;
            
            if (Convert.ToSByte(dr["formato_papel"].ToString()) == 1 || Convert.ToSByte(dr["formato_ticket"].ToString()) == 1) rBxformato2_Standar.Checked = true;
 
            if (Convert.ToSByte(dr["formato_papel"].ToString()) == 2 || Convert.ToSByte(dr["formato_ticket"].ToString()) == 2) rBxformato_Personalizado.Checked = true;
 
            this.tbxFecha.Text = dr["actualizado"].ToString();

        }

        void activarDesactivarEdicion(bool pbActivar, Color clActivar)
        {
            for (int i = 0; i < Etiquetas.Length; i++)
            {
                Etiquetas[i].Enabled = pbActivar;
                Etiquetas[i].BackColor = clActivar;
            }

            this.texto_codigoxprod.Enabled = pbActivar;
            this.texto_codigoxticket.Enabled = pbActivar;
            this.texto_depto.Enabled = pbActivar;
            this.texto_prefijo.Enabled = pbActivar;
            this.texto_numenca.Enabled = pbActivar;
            this.texto_numing.Enabled = pbActivar;
            this.tbxAncho.Enabled = pbActivar;
            this.tbxLargo.Enabled = pbActivar;
            this.tbxSeparacion.Enabled = pbActivar;
            this.ScrollBarContraEtiqueta.Enabled = pbActivar;
            this.ScrollBarContraPapel.Enabled = pbActivar;
            this.ScrollBarCorrimiento.Enabled = pbActivar;
            this.ScrollBarRetardo.Enabled = pbActivar;
            this.rBxEanPrd.Enabled = pbActivar;
            this.rBxEanTicket.Enabled = pbActivar;
            this.rBxUpcPrd.Enabled = pbActivar;
            this.rBxUpcTicket.Enabled = pbActivar;
            this.List4.Enabled = pbActivar;

            this.texto_codigoxprod.BackColor = clActivar;
            this.texto_codigoxticket.BackColor = clActivar;
            this.texto_depto.BackColor = clActivar;
            this.texto_prefijo.BackColor = clActivar;
            this.texto_numenca.BackColor = clActivar;
            this.texto_numing.BackColor = clActivar;
            this.tbxAncho.BackColor = clActivar;
            this.tbxLargo.BackColor = clActivar;
            this.tbxSeparacion.BackColor = clActivar;
        }

        private void limpiezaTextBoxes()
        {           
           /* Variable.user_codigoxticket = "pdddaatttttt";         //formato codigo de barras ticket, 14 caracteres
            Variable.user_codigoxprod = "pwwwwwtttttt";         //formato codigo de barras productos pesados, 14 caracteres
            //formato personalizado 1
            Variable.user_formato1_posdef = " ..E..N..I..f.gF.GwptWPT....C...............................";         //pos_def
            Variable.user_formato1_possize = "...1..4..1..1.12.2111336....8...............................";
            //formato personalizado 2
            Variable.user_formato2_posdef = "...E..............wptN..WPT.s..C..F.........................";         //pos_def
            Variable.user_formato2_possize = "...1..............2223..336.5..8..2.........................";
            //formato personalizado 3
            Variable.user_formato3_posdef = " ..E..N..I..f.gF.GwptWPT....C...............................";         //pos_def
            Variable.user_formato3_possize = "...1..4..1..1.12.2111336....8...............................";
            //size_def
            //etiqueta continua
            Variable.user_formato2.ancho_medio[0] = "56";         //ancho para personalizado para medio
            Variable.user_formato2.largo_medio[0] = "44";         //largo para personalizado para medio
            Variable.user_formato2.separacion_medio[0] = "3";         //separacion para personalizado para medio
            //etiqueta c/separador
            Variable.user_formato2.ancho_medio[1] = "56";         //ancho para personalizado para medio
            Variable.user_formato2.largo_medio[1] = "0";         //largo para personalizado para medio
            Variable.user_formato2.separacion_medio[1] = "0";         //separacion para personalizado para medio*/

            activarDesactivarEdicion(true, Color.White);
                       
            for (int i = 0; i < Etiquetas.Length; i++)
            {
                Etiquetas[i].Text = "";
            }
            this.texto_codigoxprod.Text="";     //codigo de barra por producto personalizado
            this.texto_codigoxticket.Text="";   //codigo de barra por ticket personalizado
            this.texto_depto.Text = "0";        //numero de departamento
            this.texto_prefijo.Text = "0";      //numero de prefijo
            this.texto_numenca.Text = "0";      //numero de encabezado
            this.texto_numing.Text = "0";       //numero de ingrediente
            this.tbxAncho.Clear();              //tamaño de anchura
            this.tbxLargo.Clear();
            this.tbxSeparacion.Clear();
            this.ScrollBarContraEtiqueta.Value = 1;
            this.ScrollBarContraPapel.Value = 1;
            this.ScrollBarCorrimiento.Value = 0;
            this.ScrollBarRetardo.Value = 0;
            Form1.statRegistro = ESTADO.EstadoRegistro.PKNOTRATADO;
        }
        
        #endregion        

        #region Botones del ToolStripMenu
        private void StripCerrar_Click(object sender, EventArgs e)
        {
            ToolStripManager.RevertMerge("toolStrip3");
            this.Dispose();
        }

        private void StripEnviar_Click(object sender, EventArgs e)
        {

            if (Num_Grupo != 0)
            {
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

                        EnviaDatosA_Bascula((int)DatosEnviado, Variable.IP_Address);  //, Num_gpo, Num_basc);
                    }
                }
            }
            else
            {
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

                        EnviaDatosA_Bascula((int)DatosEnviado, Variable.IP_Address);  //, Num_gpo, Num_basc);

                        break;
                    }
                }
            }
        }
        #endregion

        #region Envio de informacion a basculas
        private void EnviaDatosA_Bascula(int Info_A_Enviar, string direccionIP)
        {
            Cursor.Current = Cursors.WaitCursor;

            Console.WriteLine("Iniciando Conexion.....");

            Cliente_bascula = Cte.conectar(direccionIP, 50036);  //, Variable.frame, ref Dato_Recivido);
            if (Cliente_bascula != null)
            {
                Enviar_Formato(direccionIP, ref Cliente_bascula);
            }
            Cursor.Current = Cursors.Default;
        }
        private void Enviar_Formato(string direccionIP, ref Socket Cliente_bascula)
        {
            int reg_leido = 0;
            int reg_envio = 0;
            int reg_total = baseDeDatosDataSet.Publicidad.Rows.Count;

            Variable.frame = "";
            Process pro = new Process();
            pro.IniciaProcess(reg_total);
            foreach (DataRow DP in baseDeDatosDataSet.Publicidad.Rows)
            {
                Variable.frame = Env.Genera_Trama_Config_Formato(DP);
                reg_leido++;
                if (reg_leido > 4)
                {
                    reg_envio = reg_envio + reg_leido;

                    Env.Command_GF(reg_leido, Variable.frame, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo);
                    pro.UpdateProcess(reg_leido, reg_envio.ToString() + " de " + reg_total.ToString());
                    reg_leido = 0;
                    Variable.frame = "";
                }
            }
            if (Variable.frame.Length > 0 && reg_leido < 4)
            {
                reg_envio = reg_envio + reg_leido;

                Env.Command_GF(reg_leido, Variable.frame, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo);
                pro.UpdateProcess(reg_leido, reg_envio.ToString() + " de " + reg_total.ToString());
                reg_leido = 0;
                Variable.frame = "";
            }
            pro.TerminaProcess();
        }

        #endregion

        #region Creacion de Comando y envio
        private bool Command_PM(string direccionIP, ref Socket Cliente_bascula)
        {
            string[] Dato_Recibido = null;
            string reg_enviado;
            bool Limpiar = false;
            char CkSum;

            Variable.frame = "";
            Variable.frame = "PMXX" + (char)9;
            CkSum = new CheckSum().ChkSum(Variable.frame);
            Variable.frame = Variable.frame + CkSum + (char)13;

            Cte.Envio_Dato(ref Cliente_bascula, direccionIP, Variable.frame, ref Dato_Recibido);

            if (Dato_Recibido != null)
            {
                reg_enviado = Variable.frame.Substring(4);
                if (Dato_Recibido[0].IndexOf("Error") >= 0) { Limpiar = false; }
                if (Dato_Recibido[0].IndexOf("Ok") >= 0) { Limpiar = true; }
            }

            return Limpiar;
        }
        private bool Command_PI(string direccionIP, ref Socket Cliente_bascula)
        {
            string[] Dato_Recibido = null;
            string reg_enviado;
            bool Limpiar = false;
            char CkSum;

            Variable.frame = "";
            Variable.frame = "PIXX" + (char)9;
            CkSum = new CheckSum().ChkSum(Variable.frame);
            Variable.frame = Variable.frame + CkSum + (char)13;

            Cte.Envio_Dato(ref Cliente_bascula, direccionIP, Variable.frame, ref Dato_Recibido);

            if (Dato_Recibido != null)
            {
                reg_enviado = Variable.frame.Substring(4);
                if (Dato_Recibido[0].IndexOf("Error") >= 0) { Limpiar = false; }
                if (Dato_Recibido[0].IndexOf("Ok") >= 0) { Limpiar = true; }
            }

            return Limpiar;
        }
        private bool Command_PV(string direccionIP, ref Socket Cliente_bascula)
        {
            string[] Dato_Recibido = null;
            string reg_enviado;
            bool Limpiar = false;
            char CkSum;

            Variable.frame = "";
            Variable.frame = "PVXX" + (char)9;
            CkSum = new CheckSum().ChkSum(Variable.frame);
            Variable.frame = Variable.frame + CkSum + (char)13;

            Cte.Envio_Dato(ref Cliente_bascula, direccionIP, Variable.frame, ref Dato_Recibido);

            if (Dato_Recibido != null)
            {
                reg_enviado = Variable.frame.Substring(4);
                if (Dato_Recibido[0].IndexOf("Error") >= 0) { Limpiar = false; }
                if (Dato_Recibido[0].IndexOf("Ok") >= 0) { Limpiar = true; }
            }

            return Limpiar;
        }
        private bool Command_PO(string direccionIP, ref Socket Cliente_bascula)
        {
            string[] Dato_Recibido = null;
            string reg_enviado;
            bool Limpiar = false;
            char CkSum;

            Variable.frame = "";
            Variable.frame = "POXX" + (char)9;
            CkSum = new CheckSum().ChkSum(Variable.frame);
            Variable.frame = Variable.frame + CkSum + (char)13;

            Cte.Envio_Dato(ref Cliente_bascula, direccionIP, Variable.frame, ref Dato_Recibido);

            if (Dato_Recibido != null)
            {
                reg_enviado = Variable.frame.Substring(4);
                if (Dato_Recibido[0].IndexOf("Error") >= 0) { Limpiar = false; }
                if (Dato_Recibido[0].IndexOf("Ok") >= 0) { Limpiar = true; }
            }

            return Limpiar;
        }
        #endregion

        #region funciones para los campos
        private void Llenar_etiqueta(string a, string b)
        {
            int i = 0;
            if (a.Length > 0 && b.Length > 0)
            {
                for (int c = 0; c < 20; c++)
                {
                    for (int s = 0; s < 3; s++)
                    {
                        Etiquetas[i].Text = campo(a.Substring(i, 1).ToString());

                        if (b.Substring(i, 1).ToString() != ".") { Etiquetas[i].Tag = b.Substring(i, 1).ToString(); }
                        else { Etiquetas[i].Tag = "0"; }

                        i = i + 1;
                    }
                }
            }
        }
        private bool buscar_campo(string aa)
        {
            bool exist = false;
            int i = 0;
            for (int c = 0; c < 20; c++)
            {
                for (int s = 0; s < 3; s++)
                {
                    if (campo(aa) != "" && aa != "!" && Etiquetas[i].Text != "")
                    {
                        if (Etiquetas[i].Text.Substring(0, 1) == aa)
                        {
                            exist = true;
                            break;
                        }
                    }
                    i = i + 1;
                }
            }
            return exist;
        }

        private string campo(string car)
        {
            string cad_result = "";
            const char nulo = '"';
            string letra = nulo.ToString();
            if (letra == car) { cad_result = Variable.list_campo[0, 1]; }

            for (int i = 0; i < Variable.list_campo.GetLength(0); i++)
            {
                if (Variable.list_campo[i,0] == car)
                {
                    cad_result = Variable.list_campo[i,1];
                    break;
                }
            }
            return cad_result;
        }

        private int indice(string Name)
        {
            for (int i = 0; i < this.Etiquetas.Length; i++)
            {
                if (this.Etiquetas[i].Name == Name) { return i; }
            }
            return -1;
        }
        private void asignarEventos()
        {
            foreach (Label Lab in Etiquetas)
            {
                Lab.MouseDown += new MouseEventHandler(this.Etiquetas_MouseDown);
                Lab.DragDrop += new DragEventHandler(this.Etiquetas_DragDrop);
                Lab.DragEnter += new DragEventHandler(this.Etiquetas_DragEnter);
                Lab.DragOver += new DragEventHandler(this.Etiquetas_DragOver);
                Lab.AllowDrop = true;
            }
        }
        private void colocar_label(Label etique, int op, int Index, int Index2)
        {
            string cc = "";
            char[] Ncomparte= new char[]{'"','C','m','E','I','N','!','s'}; // "","E","C","I","N","m","!","s"
            int a = Index + 1;
            int b = 0;
            cc = etique.Text.Substring(0, 1);
            if (cc == "" || cc == "C" || cc == "m" || cc == "E" || cc == "I" || cc == "N" || cc == "!" || cc == "s")
            {
                if ((a % 3) == 0)
                {
                    a = a - 3;
                    b = 2;
                }
                else
                {
                    if ((a % 3) == 2)
                    {
                        a = a - 2;
                        b = 1;
                    }
                    else { a = a - 1; }
                }

                if (Etiquetas[a].Text.Length > 0)
                    if (cc == Etiquetas[a].Text.Substring(0, 1)) return;

                Etiquetas[a].Text = "";
                Etiquetas[a].Tag = "";
                Etiquetas[a + 1].Text = "";
                Etiquetas[a + 1].Tag = "";
                Etiquetas[a + 2].Text = "";
                Etiquetas[a + 2].Tag = "";
                a = Index + 1;

                if (cc == "E" || cc == "I" || cc == "N" || cc == "!" || cc == "s") { a = a - b; }
            }
            else
            {

                if ((a % 3) == 0)
                {
                    if ((Etiquetas[a - 3].Text == "") || (Etiquetas[a - 3].Text.Substring(0, 1) == "E") || (Etiquetas[a - 3].Text.Substring(0, 1) == "C") || (Etiquetas[a - 3].Text.Substring(0, 1) == "I") || (Etiquetas[a - 3].Text.Substring(0, 1) == "N") || (Etiquetas[a - 3].Text.Substring(0, 1) == "m") || (Etiquetas[a - 3].Text.Substring(0, 1) == "!") || (Etiquetas[a - 3].Text.Substring(0, 1) == "s"))
                    {
                        Etiquetas[a - 3].Text = "";
                        Etiquetas[a - 3].Tag = "";
                    }
                }
                else
                {
                    if ((a % 3) == 2)
                    {
                        if ((Etiquetas[a - 2].Text == "") || (Etiquetas[a - 2].Text.Substring(0, 1) == "E") || (Etiquetas[a - 2].Text.Substring(0, 1) == "C") || (Etiquetas[a - 2].Text.Substring(0, 1) == "I") || (Etiquetas[a - 2].Text.Substring(0, 1) == "N") || (Etiquetas[a - 2].Text.Substring(0, 1) == "m") || (Etiquetas[a - 2].Text.Substring(0, 1) == "!") || (Etiquetas[a - 2].Text.Substring(0, 1) == "s"))
                        {
                            Etiquetas[a - 2].Text = "";
                            Etiquetas[a - 2].Tag = "";
                        }
                    }
                    else
                    {
                        if ((Etiquetas[a - 1].Text == "") || (Etiquetas[a - 1].Text.Substring(0, 1) == "E") || (Etiquetas[a - 1].Text.Substring(0, 1) == "C") || (Etiquetas[a - 1].Text.Substring(0, 1) == "I") || (Etiquetas[a - 2].Text.Substring(0, 1) == "N") || (Etiquetas[a - 2].Text.Substring(0, 1) == "m") || (Etiquetas[a - 2].Text.Substring(0, 1) == "!") || (Etiquetas[a - 1].Text.Substring(0, 1) == "s"))
                        {
                            Etiquetas[a - 1].Text = "";
                            Etiquetas[a - 1].Tag = "";
                        }

                    }
                }
            }
            if (op == 1)
            {
                Etiquetas[a - 1].Text = etique.Text;
                Etiquetas[a - 1].Tag = "1";
                Etiquetas[a - 1].BorderStyle = System.Windows.Forms.BorderStyle.None;
                Etiquetas[Index2].Text = "";
                Etiquetas[Index2].Tag = "";
            }
            else
            {
                Etiquetas[a - 1].Text = etique.Text;
                Etiquetas[a - 1].Tag = "1";
                Etiquetas[a - 1].BorderStyle = System.Windows.Forms.BorderStyle.None;
            }
        }
        #endregion

        #region Eventos de los controles 
        private void Etiquetas_DragOver(object sender, DragEventArgs e)
        {
            Label Source = ((Label)sender);

            for (int i = 0; i < this.Etiquetas.Length; i++)
            {
                if (Source.Name != this.Etiquetas[i].Name) this.Etiquetas[i].BorderStyle = BorderStyle.None;
            }
            Source.BorderStyle = BorderStyle.FixedSingle;
        }
        private void Etiquetas_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.Label")) { e.Effect = DragDropEffects.Copy; }
        }
        private void Etiquetas_DragDrop(object sender, DragEventArgs e)
        {
            Label Source = ((Label)sender);
            Label etique = ((Label)e.Data.GetData("System.Windows.Forms.Label"));

            int Index = indice(Source.Name);

            if (etique.Tag.ToString().Substring(0, 1) == "L")
            {
                string cc = etique.Text.Substring(0, 1);
                if (!buscar_campo(cc))
                { colocar_label(etique, 0, Index, 0); }
                else
                {
                    Source.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    MessageBox.Show("El campo ya esta asignado");
                }
            }
            else
            {
                int Index2 = Convert.ToInt16(etique.Tag);
                if (Index != Index2)
                {
                    if (etique.Text != "") colocar_label(etique, 1, Index, Index2);
                }
            }
        }
        private void Etiquetas_MouseDown(object sender, MouseEventArgs e)
        {
            Label I = ((Label)sender);
            I.Select();
            int Index = 0;
            for (int i = 0; i <= Etiquetas.Length - 1; i++)
            {
                if (Etiquetas[i].Name == I.Name)
                {
                    Index = i;
                    break;
                }
            }
            //List1.Visible = false;
            //List3.Visible = false;

            if ((e.Button == MouseButtons.Right) && (formato_locked == false))
            {
                this.Frame1.Tag = Index;
                contextMenuStrip1.Top = Etiquetas[Index].Top;
                contextMenuStrip1.Left = Etiquetas[Index].Left;
                contextMenuStrip1.Show();
                //List1.Top 
                //List1.Left = Etiquetas[Index].Left;
                //List1.Visible = true;
                //List1.Focus();
            }
            else
            {
                if (I.Text != "")
                {
                    Point pt = new Point(e.X, e.Y);
                    if (e.Button == MouseButtons.Left && (formato_locked == false))
                    {
                        pasar.Left = e.X + I.Left;
                        pasar.Top = e.Y + I.Top;
                        pasar.Text = I.Text;
                        pasar.Tag = Index;
                        pasar.DoDragDrop(pasar, DragDropEffects.Copy);
                    }
                }
            }
        }
        private void List4_MouseDown(object sender, MouseEventArgs e)
        {
            ListBox campo = ((ListBox)sender);

            int index = campo.SelectedIndex;
            Point pt = new Point(e.X, e.Y);
            if (e.Button == System.Windows.Forms.MouseButtons.Left && (formato_locked == false))
            {
                pasar.Left = e.X + campo.Left;
                pasar.Top = e.Y + campo.Top;
                pasar.Text = campo.Items[index].ToString();
                pasar.Tag = "L";
                pasar.DoDragDrop(pasar, DragDropEffects.Copy);
            }
        }
        private void rBxEanPrd_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxEanPrd.Checked)
            {
                Variable.user_EAN_UPCxProd = 0;
                cBxCodigoPrd.Items.Clear();
                cBxCodigoPrd.Text = "";               
                cBxCodigoPrd.Items.Add("EAN13(pccccccwwwwwv) Peso");
                cBxCodigoPrd.Items.Add("EAN13(pcccccctttttv) Total");
                cBxCodigoPrd.Items.Add("Personalizado");
                this.cBxCodigoPrd.Focus();
            }
        }
        private void rBxUpcPrd_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxUpcPrd.Checked)
            {
                Variable.user_EAN_UPCxProd = 1;
                cBxCodigoPrd.Items.Clear();
                cBxCodigoPrd.Text = "";               
                cBxCodigoPrd.Items.Add("UPC12(xpcccccwwwwwv) Peso");
                cBxCodigoPrd.Items.Add("UPC12(xpccccctttttv) Total");
                cBxCodigoPrd.Items.Add("Personalizado");
                this.cBxCodigoPrd.Focus();
            }
        }
        private void cBxCodigoPrd_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cBxCodigoPrd.SelectedIndex == 2)
            {
                this.texto_codigoxprod.Text = Variable.user_codigoxprod;
                this.texto_codigoxprod.Focus();
            }
            else this.rBxEanTicket.Focus();
        }
        private void rBxEanTicket_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxEanTicket.Checked)
            {
                Variable.user_EAN_UPCxTicket = 0;
                cBxCodigoTicket.Items.Clear();
                cBxCodigoTicket.Text = "";                
                cBxCodigoTicket.Items.Add("EAN13(pxaannttttttv) Vendedor");
                cBxCodigoTicket.Items.Add("EAN13(pxddnnttttttv) Depto.");
                cBxCodigoTicket.Items.Add("Personalizado");
                this.cBxCodigoTicket.Focus();
            }
        }
        private void rBxUpcTicket_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxUpcTicket.Checked)
            {
                Variable.user_EAN_UPCxTicket = 1;
                cBxCodigoTicket.Items.Clear();
                cBxCodigoTicket.Text = "";                
                cBxCodigoTicket.Items.Add("UPC12 (xpaannttttttv) Vendedor");
                cBxCodigoTicket.Items.Add("UPC12 (xpddnnttttttv) Depto.");
                cBxCodigoTicket.Items.Add("Personalizado");
                this.cBxCodigoTicket.Focus();
            }
        }
        private void cBxCodigoTicket_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cBxCodigoTicket.SelectedIndex == 2)
            {
                this.texto_codigoxticket.Text = Variable.user_codigoxticket;
                this.texto_codigoxticket.Focus();
            }
            else this.texto_prefijo.Focus();
        }
        private void texto_codigoxticket_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.texto_codigoxticket.Text = "";
            if (e.KeyCode == Keys.Enter) this.texto_prefijo.Focus();

        }
        private void texto_codigoxprod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.texto_codigoxprod.Text = "";
            if (e.KeyCode == Keys.Enter) this.rBxEanTicket.Focus();

        }
        private void texto_prefijo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.texto_depto.Focus();
        }
        private void texto_depto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.chkNutriente.Focus();
        }
        private void chkNutriente_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNutriente.Checked) Variable.user_nutri = 1;
            else Variable.user_nutri = 0;
            this.ScrollBarContraEtiqueta.Focus();
        }

        private void texto_codigoxticket_Validating(object sender, CancelEventArgs e)
        {
            int c;
            texto_codigoxticket.Text = texto_codigoxticket.Text.ToLower();
            texto_codigoxticket.Text = Variable.validar_salida(texto_codigoxticket.Text, 4);
            Variable.user_codigoxticket = this.texto_codigoxticket.Text;

            if (texto_codigoxticket.Text != "")
            {
                if (texto_codigoxticket.Text.Length < 12)
                {
                    Help.ShowPopup(texto_codigoxticket,"Longitud no es válida.", new Point(this.texto_codigoxticket.Location.X, this.texto_codigoxticket.Location.Y));
                    this.texto_codigoxticket.Focus();
                    return;
                }
                bool ya = false;
                for (c = 0; c < texto_codigoxticket.Text.Length; c++)
                {
                    if (texto_codigoxticket.Text.Substring(c, 1) != "x")
                    {
                        ya = false;
                        for (int s = (c + 1); s < texto_codigoxticket.Text.Length; s++)
                        {
                            if (texto_codigoxticket.Text.Substring(c, 1) == texto_codigoxticket.Text.Substring(s, 1))
                            {
                                if (ya)
                                {
                                    Help.ShowPopup(texto_codigoxticket, "La secuencia de caracteres no es valida( " + texto_codigoxticket.Text.Substring(s, 1) + " )", new Point(this.texto_codigoxticket.Location.X, this.texto_codigoxticket.Location.Y));
                                    c = texto_codigoxticket.Text.Length;
                                    s = texto_codigoxticket.Text.Length;
                                    this.texto_codigoxticket.Focus();
                                    return;
                                }
                            }
                            else
                            { ya = true; }
                        }
                    }
                }
                int cont = 0;
                for (c = 0; c < texto_codigoxticket.Text.Length; c++)
                {
                    if (texto_codigoxticket.Text.Substring(c, 1) == "d")
                    { cont++; }
                }

                if (cont > 2)
                {
                    Help.ShowPopup(texto_codigoxticket, "El Departamento no puede ser mayor de 2 caracteres (d)", new Point(this.texto_codigoxticket.Location.X, this.texto_codigoxticket.Location.Y));
                    texto_codigoxticket.Focus();
                    return;
                }
                cont = 0;
                for (c = 0; c < texto_codigoxticket.Text.Length; c++)
                {
                    if (texto_codigoxticket.Text.Substring(c, 1) == "p")
                    { cont++; }
                }
                if (cont > 2)
                {
                    Help.ShowPopup(texto_codigoxticket, "El Prefijo no puede ser mayor de 2 caracteres (p)", new Point(this.texto_codigoxticket.Location.X, this.texto_codigoxticket.Location.Y));
                    texto_codigoxticket.Focus();
                    return;
                }
                cont = 0;
                for (c = 0; c < texto_codigoxticket.Text.Length; c++)
                {
                    if (texto_codigoxticket.Text.Substring(c, 1) == "a")
                    { cont++; }
                }
                if (cont > 2)
                {
                    Help.ShowPopup(texto_codigoxticket,"El Vendedor no puede ser mayor de 2 caracteres (a)", new Point(this.texto_codigoxticket.Location.X, this.texto_codigoxticket.Location.Y));
                    texto_codigoxticket.Focus();
                    return;
                }
            }
        }

        private void texto_codigoxprod_Validating(object sender, CancelEventArgs e)
        {
            texto_codigoxprod.Text = texto_codigoxprod.Text.ToLower();
            texto_codigoxprod.Text = Variable.validar_salida(texto_codigoxprod.Text, 3);
            Variable.user_codigoxprod = this.texto_codigoxprod.Text;
            if (texto_codigoxprod.Text != "")
            {
                if (texto_codigoxprod.Text.Length < 12)
                {
                    Help.ShowPopup(texto_codigoxprod, "Longitud no es válida.", new  Point(this.texto_codigoxprod.Location.X,this.texto_codigoxprod.Location.Y));
                    texto_codigoxprod.Focus();
                    return;
                }
                bool ya = false;
                for (int c = 0; c < texto_codigoxprod.Text.Length; c++)
                {
                    if (texto_codigoxprod.Text.Substring(c, 1) != "x")
                    {
                        ya = false;
                        for (int s = (c + 1); s < texto_codigoxprod.Text.Length; s++)
                        {
                            if (texto_codigoxprod.Text.Substring(c, 1) == texto_codigoxprod.Text.Substring(s, 1))
                            {
                                if (ya)
                                {
                                    Help.ShowPopup(texto_codigoxprod, "La secuencia de caracter no es valida (" + texto_codigoxprod.Text.Substring(s, 1) + ")", new Point(this.texto_codigoxprod.Location.X, this.texto_codigoxprod.Location.Y));
                                    c = texto_codigoxprod.Text.Length;
                                    s = texto_codigoxprod.Text.Length;
                                    this.texto_codigoxprod.Focus();
                                    return;
                                }
                            }
                            else
                            {
                                ya = true;
                            }
                        }
                    }
                }

                int cont = 0;
                for (int c = 0; c < texto_codigoxprod.Text.Length; c++)
                {
                    if (texto_codigoxprod.Text.Substring(c, 1) == "d")
                    { cont++; }
                }
                if (cont > 2)
                {
                    Help.ShowPopup(texto_codigoxprod, "El Departamento no puede ser mayor de 2 caracteres (d)", new Point(this.texto_codigoxprod.Location.X, this.texto_codigoxprod.Location.Y));
                    texto_codigoxprod.Focus();
                    return;
                }
                cont = 0;
                for (int c = 0; c < texto_codigoxprod.Text.Length; c++)
                {
                    if (texto_codigoxprod.Text.Substring(c, 1) == "p")
                    { cont++; }
                }
                if (cont > 2)
                {
                    Help.ShowPopup(texto_codigoxprod, "El Prefijo no puede ser mayor de 2 caracteres (p)", new Point(this.texto_codigoxprod.Location.X, this.texto_codigoxprod.Location.Y));
                    texto_codigoxprod.Focus();
                    return;
                }
                cont = 0;
                for (int c = 0; c < texto_codigoxprod.Text.Length; c++)
                {
                    if (texto_codigoxprod.Text.Substring(c, 1) == "c")
                    { cont++; }
                }
                if (cont > 6)
                {
                    Help.ShowPopup(texto_codigoxprod, "El Código no puede ser mayor de 6 caracteres (c)", new Point(this.texto_codigoxprod.Location.X, this.texto_codigoxprod.Location.Y));
                    texto_codigoxprod.Focus();
                    return;
                }
                cont = 0;
                for (int c = 0; c < texto_codigoxprod.Text.Length; c++)
                {
                    if (texto_codigoxprod.Text.Substring(c, 1) == "a")
                    { cont++; }
                }
                if (cont > 2)
                {
                    Help.ShowPopup(texto_codigoxprod, "El Vendedor no puede ser mayor de 2 caracteres (a)", new Point(this.texto_codigoxprod.Location.X, this.texto_codigoxprod.Location.Y));
                    texto_codigoxprod.Focus();
                    return;
                }
                cont = 0;
                for (int c = 0; c < texto_codigoxprod.Text.Length; c++)
                {
                    if (texto_codigoxprod.Text.Substring(c, 1) == "w")
                    { cont++; }
                }
                if (cont > 5)
                {
                    //MessageBox.Show("El Peso no puede ser mayor de 5 caracteres (w)");
                    Help.ShowPopup(texto_codigoxprod, "El Peso no puede ser mayor de 5 caracteres (w)", new Point(this.texto_codigoxprod.Location.X, this.texto_codigoxprod.Location.Y));
                    texto_codigoxprod.Focus();
                    return;
                }
            }
        }

        private void ScrollBarContraEtiqueta_Scroll(object sender, ScrollEventArgs e)
        {
            Variable.user_contrasteetiqueta = e.NewValue;           
        }
        private void ScrollBarContraPapel_Scroll(object sender, ScrollEventArgs e)
        {
            Variable.user_contrastepapel = e.NewValue;          
        }
        private void ScrollBarRetardo_Scroll(object sender, ScrollEventArgs e)
        {
            Variable.user_retardoimpresion = e.NewValue;            
        }
        private void ScrollBarCorrimiento_Scroll(object sender, ScrollEventArgs e)
        {
            Variable.user_corrimientopieza = e.NewValue;
        }

        private void tipo_papel_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxTipopapel.Checked) Variable.user_formato.medio_imp = 0;
            this.rBxformato1_Standar.Focus();
        }

        private void tipo_etiqueta_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxTipoetiqueta.Checked) Variable.user_formato.medio_imp = 1;
            this.rBxformato1_Standar.Focus();
        }

        private void formato1_Standar_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxTipopapel.Checked && rBxformato1_Standar.Checked)
            {
                Variable.user_formato.for_papel_tipoimpre = 0;
                //'formato default 1 ticket
                Variable.user_formato1_posdef = "...E..............wptN..WPTs...C..m.........................";
                Variable.user_formato1_possize = "...1..............2223..3365...8..2.........................";
                tbxAncho.Text = "56";
                tbxLargo.Text = "0";
                tbxSeparacion.Text = "0";
                myImpresion[0].medio_imp = 0;
                myImpresion[0].tipoCodigo = Variable.user_EAN_UPCxProd;
            }
            if (rBxTipoetiqueta.Checked && rBxformato1_Standar.Checked)
            {
                Variable.user_formato.for_ecsep_tipoimpre = 0;
                //'formato default 1 etiqueta c/separador
                Variable.user_formato1_posdef = "...E..N..I..f.gF.GwptWPT....C..m............................";
                Variable.user_formato1_possize = "...1..4..1..1.12.2111336....8..2............................";
                tbxAncho.Text = "56";
                tbxLargo.Text = "44";
                tbxSeparacion.Text = "3";
                myImpresion[0].medio_imp = 1;
                myImpresion[0].tipoCodigo = Variable.user_EAN_UPCxTicket;
            }
            myImpresion[0].codigo_xprod = cBxCodigoPrd.SelectedIndex;
            myImpresion[0].codigo_xticket = cBxCodigoTicket.SelectedIndex;
            myImpresion[0].for_papel_tipoimpre = Variable.user_formato.for_papel_tipoimpre;
            myImpresion[0].for_ecsep_tipoimpre = Variable.user_formato.for_ecsep_tipoimpre;
                        
            tbxAncho.Enabled = false;
            tbxLargo.Enabled = false;
            tbxSeparacion.Enabled = false;
            this.List4.Enabled = false;
            formato_locked = true;
            Llenar_etiqueta(Variable.user_formato1_posdef, Variable.user_formato1_possize);            
            this.panel3.Focus();
        }

        private void formato2_Standar_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxTipopapel.Checked && rBxformato2_Standar.Checked)
            {
                Variable.user_formato.for_papel_tipoimpre = 1;
                //'formato default 2 ticket
                Variable.user_formato2_posdef = "...FHcrV.E........wptN..WPTs...C..m.........................";
                Variable.user_formato2_possize = "...22223.1........2223..3365...8..2.........................";
                tbxAncho.Text = "56";
                tbxLargo.Text = "0";
                tbxSeparacion.Text = "0";
                myImpresion[1].medio_imp = 0;
                myImpresion[1].tipoCodigo = Variable.user_EAN_UPCxProd;
            }
            if (rBxTipoetiqueta.Checked && rBxformato2_Standar.Checked)
            {
                Variable.user_formato.for_ecsep_tipoimpre = 1;
                //'formato default 2 etiqueta c/separador
                Variable.user_formato2_posdef = "...C..!..N..I..f.gFHG...wptWPT.m............................";
                Variable.user_formato2_possize = "...8..1..4..1..1.1333...111336.2............................";
                tbxAncho.Text = "56";
                tbxLargo.Text = "44";
                tbxSeparacion.Text = "3";
                myImpresion[1].medio_imp = 1;
                myImpresion[1].tipoCodigo = Variable.user_EAN_UPCxTicket;
            }
            myImpresion[1].codigo_xprod = cBxCodigoPrd.SelectedIndex;
            myImpresion[1].codigo_xticket = cBxCodigoTicket.SelectedIndex;
            myImpresion[1].for_papel_tipoimpre = Variable.user_formato.for_papel_tipoimpre;
            myImpresion[1].for_ecsep_tipoimpre = Variable.user_formato.for_ecsep_tipoimpre;
            tbxAncho.Enabled = false;
            tbxLargo.Enabled = false;
            tbxSeparacion.Enabled = false;
            this.List4.Enabled = false;
            formato_locked = true;
            Llenar_etiqueta(Variable.user_formato2_posdef, Variable.user_formato2_possize);
            this.panel3.Focus();
        }

        private void formato_Personalizado_CheckedChanged(object sender, EventArgs e)
        {
            if (rBxTipopapel.Checked && rBxformato_Personalizado.Checked)
            {
                Variable.user_formato.for_papel_tipoimpre = 2;
                //papel ticket
                Variable.user_formato3_posdef = " ..E..N..I..f.gF.GwptWPT....C...............................";         //pos_def
                Variable.user_formato3_possize = "...1..4..1..1.12.2111336....8...............................";
                tbxAncho.Text = "56";
                tbxLargo.Text = "44";
                tbxSeparacion.Text = "3"; 
                tbxLargo.Enabled = false;
                tbxSeparacion.Enabled = false;

                myFormato[0].largo_medio = tbxLargo.Text;
                myFormato[0].separacion_medio = tbxSeparacion.Text;
                myFormato[0].ancho_medio = tbxAncho.Text;
                myFormato[0].posdef = Variable.user_formato3_posdef;
                myFormato[0].possize = Variable.user_formato3_possize;

                myImpresion[2].medio_imp = 1;
                myImpresion[2].tipoCodigo = Variable.user_EAN_UPCxTicket;
            }
            if (rBxTipoetiqueta.Checked && rBxformato_Personalizado.Checked)
            {
                Variable.user_formato.for_ecsep_tipoimpre = 2;
                //'etiqueta con separador               
                Variable.user_formato3_posdef = "...E..............wptN..WPT.s..C..F.........................";         //pos_def
                Variable.user_formato3_possize = "...1..............2223..336.5..8..2.........................";
                tbxAncho.Text = "56";
                tbxLargo.Text = "0";
                tbxSeparacion.Text = "0";              
                tbxLargo.Enabled = true;
                tbxSeparacion.Enabled = true;

                myFormato[1].largo_medio = tbxLargo.Text;
                myFormato[1].separacion_medio = tbxSeparacion.Text;
                myFormato[1].ancho_medio = tbxAncho.Text;
                myFormato[1].posdef = Variable.user_formato3_posdef;
                myFormato[1].possize = Variable.user_formato3_possize;

                myImpresion[2].medio_imp = 1;
                myImpresion[2].tipoCodigo = Variable.user_EAN_UPCxTicket;
            }
            myImpresion[2].codigo_xprod = cBxCodigoPrd.SelectedIndex;
            myImpresion[2].codigo_xticket = cBxCodigoTicket.SelectedIndex;
            myImpresion[2].for_papel_tipoimpre = Variable.user_formato.for_papel_tipoimpre;
            myImpresion[2].for_ecsep_tipoimpre = Variable.user_formato.for_ecsep_tipoimpre;

           

            formato_locked = false;
            tbxAncho.Enabled = true;
            this.List4.Enabled = true;
            Llenar_etiqueta(Variable.user_formato3_posdef, Variable.user_formato3_possize);
            this.panel3.Focus();
        }

        private void texto_numing_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.texto_numenca.Focus();
        }

        private void texto_numenca_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.rBxTipopapel.Focus();
        }
        #endregion

        private void UserImpresor_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Grupo' Puede moverla o quitarla según sea necesario.
            this.grupoTableAdapter.Fill(this.baseDeDatosDataSet.Grupo);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Bascula' Puede moverla o quitarla según sea necesario.
            this.basculaTableAdapter.Fill(this.baseDeDatosDataSet.Bascula);

            toolStripLabel3.Text = Nombre_Select;

            Asigna_Grupo();
            Asigna_Bascula();

            List1.Items.AddRange(Aaccion);
            List3.Items.AddRange(Atamaño);
            List4.Items.AddRange(Acampos);

            ///codigo por producto
            ///
            ///EAN13 (pccccccwwwwwv) codigo peso
            ///EAN13 (pcccccctttttv) codigo total
            ///UPC13 (xpcccccwwwwwv) codigo peso
            ///UPC13 (xpccccctttttv) codigo total
            ///
            ///codigo por tikect
            ///EAN13 (pxaannttttttv) Vendedor Total
            ///EAN13 (pxddnnttttttv) Departamento Total
            ///UPC12 (xpaannttttttv) Vendedor Total
            ///UPC12 (xpddnnttttttv) Departamento Total
            ///

        }

        
    }
}


