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
using TreeViewBound;
using TorreyTransfer;

namespace MainMenu
{
    public partial class f5Cortes : MdiChildForm
    {      
        private bool exist_nodo = false;      
        private ContextMenu MenuBotton1 = new ContextMenu();

        Variable.lgrupo[] myGrupo;
        Variable.lbasc[] myScale;
        Variable.nodo_actual myCurrent;
       
        ADOutil Conec = new ADOutil();
        ArrayList Dato_Nuevo = new ArrayList();
        Conexion Cte = new Conexion();
        Serial SR = new Serial();
        Socket Cliente_bascula = null;
        Envia_Dato Env = new Envia_Dato();
       
        #region Inicializacion
        public f5Cortes()
        {
            InitializeComponent();
            this.TransparencyKey = Color.Empty;            
        }
        
        #endregion

        #region BotonesTap-Sincronización

        public void QuitarBackColor()
        {
            ribConsulta.Checked = false;
            ribCorte.Checked = false;
            ribCerrar.Checked = false;          
            this.Activate();
            //this.splitContainer2.Panel2.Controls.Clear();
            ToolStripManager.RevertMerge("toolStrip3");
        }
        private void ribConsulta_Click(object sender, EventArgs e)
        {
            QuitarBackColor();
            ribConsulta.Checked = true;
            lvwVentas.Items.Clear();

            int BasculasActualizadas = 0;
            int NumeroDeBaculas = 0;

            if (myCurrent.Nserie != null)
            {
                if (myCurrent.gpo > 0)
                {
                    for (int pos = 0; pos < myScale.Length; pos++)  //Num de Basculas en el grupo
                        if (myScale[pos].gpo == myCurrent.gpo) NumeroDeBaculas++;

                    for (int pos = 0; pos < myScale.Length; pos++)
                    {
                        if (myScale[pos].gpo == myCurrent.gpo)
                        {
                            BasculasActualizadas++;

                            if (myScale[pos].tipo == (int)ESTADO.tipoConexionesEnum.PKWIFI)
                            {
                                if (RecibirConsulta_Bascula(myScale[pos].ip, myScale[pos].gpo, myScale[pos].idbas, myScale[pos].nserie))
                                {
                                    BasculasActualizadas--;
                                    if (MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma] + " "
                                        + myCurrent.Nserie + " " + Variable.SYS_MSJ[417, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma],
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No) break;
                                }
                            }
                            else
                            {
                                if (RecibirConsulta_Bascula(myScale[pos].pto, myScale[pos].baud, myScale[pos].gpo, myScale[pos].idbas, myScale[pos].nserie))
                                {
                                    BasculasActualizadas--;
                                    if (MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma] + " "
                                        + myCurrent.Nserie + " " + Variable.SYS_MSJ[417, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma],
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No) break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int pos = 0; pos < myScale.Length; pos++)  //Num de Basculas con el mismo Num_Bascula
                        if (myScale[pos].idbas == myCurrent.idbas) NumeroDeBaculas++;

                    for (int pos = 0; pos < myScale.Length; pos++)
                    {
                        if (myScale[pos].idbas == myCurrent.idbas)
                        {
                            BasculasActualizadas++;

                            if (myScale[pos].tipo == (int)ESTADO.tipoConexionesEnum.PKWIFI)
                            {
                                if (RecibirConsulta_Bascula(myScale[pos].ip, myScale[pos].gpo, myScale[pos].idbas, myScale[pos].nserie))
                                {
                                    BasculasActualizadas--;
                                    MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma]
                                        + " " + myCurrent.Nserie + ".", Variable.SYS_MSJ[42, Variable.idioma],
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    break;
                                }
                            }
                            else
                            {
                                if (RecibirConsulta_Bascula(myScale[pos].pto, myScale[pos].baud, myScale[pos].gpo, myScale[pos].idbas, myScale[pos].nserie))
                                {
                                    BasculasActualizadas--;
                                    MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma]
                                        + " " + myCurrent.Nserie + ".", Variable.SYS_MSJ[42, Variable.idioma],
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                Thread.Sleep(500);
                MessageBox.Show(this, Variable.SYS_MSJ[421, Variable.idioma] + " " + BasculasActualizadas + " "
                    + Variable.SYS_MSJ[419, Variable.idioma] + " " + NumeroDeBaculas,
                    Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this, Variable.SYS_MSJ[24, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma]);
                Thread.Sleep(500);
            }
        }
        private void ribCorte_Click(object sender, EventArgs e)
        {
            QuitarBackColor();
            ribCorte.Checked = true;
            lvwVentas.Items.Clear();

            int BasculasActualizadas = 0;
            int NumeroDeBaculas = 0;

            if (myCurrent.Nserie != null)
            {
                if (myCurrent.gpo > 0)
                {
                    for (int pos = 0; pos < myScale.Length; pos++)  //Num de Basculas en el grupo
                        if (myScale[pos].gpo == myCurrent.gpo) NumeroDeBaculas++;

                    for (int pos = 0; pos < myScale.Length; pos++)
                    {
                        if (myScale[pos].gpo == myCurrent.gpo)
                        {
                            BasculasActualizadas++;

                            if (myScale[pos].tipo == (int)ESTADO.tipoConexionesEnum.PKWIFI)
                            {
                                if (RecibirVentasD_Bascula(myScale[pos].ip, myScale[pos].gpo, myScale[pos].idbas, myScale[pos].nserie))
                                {
                                    BasculasActualizadas--;
                                    if (MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma] + " "
                                        + myCurrent.Nserie + " " + Variable.SYS_MSJ[417, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma],
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No) break;
                                }
                            }
                            else
                            {
                                if (RecibirVentasD_Bascula(myScale[pos].pto, myScale[pos].baud, myScale[pos].gpo, myScale[pos].idbas, myScale[pos].nserie))
                                {
                                    BasculasActualizadas--;
                                    if (MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma] + " "
                                        + myCurrent.Nserie + " " + Variable.SYS_MSJ[417, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma],
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No) break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int pos = 0; pos < myScale.Length; pos++)  //Num de Basculas con el mismo Num_Bascula
                        if (myScale[pos].idbas == myCurrent.idbas) NumeroDeBaculas++;

                    for (int pos = 0; pos < myScale.Length; pos++)
                    {
                        if (myScale[pos].idbas == myCurrent.idbas)
                        {
                            if (myScale[pos].tipo == (int)ESTADO.tipoConexionesEnum.PKWIFI)
                            {
                                BasculasActualizadas++;

                                if (RecibirVentasD_Bascula(myScale[pos].ip, myScale[pos].gpo, myScale[pos].idbas, myScale[pos].nserie))
                                {
                                    BasculasActualizadas--;
                                    MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma]
                                        + " " + myCurrent.Nserie + ".", Variable.SYS_MSJ[42, Variable.idioma],
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    break;
                                }
                            }
                            else
                            {
                                if (RecibirVentasD_Bascula(myScale[pos].pto, myScale[pos].baud, myScale[pos].gpo, myScale[pos].idbas, myScale[pos].nserie))
                                {
                                    BasculasActualizadas--;
                                    MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma]
                                        + " " + myCurrent.Nserie + ".", Variable.SYS_MSJ[42, Variable.idioma],
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                Thread.Sleep(500);
                MessageBox.Show(this, Variable.SYS_MSJ[418, Variable.idioma] + " " + BasculasActualizadas + " "
                    + Variable.SYS_MSJ[419, Variable.idioma] + " " + NumeroDeBaculas,
                    Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this, Variable.SYS_MSJ[24, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma]);
                Thread.Sleep(500);
            }
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
                
        private void Crear_Nodos_Basculas()
        {           
            tvwBascula.Nodes.Clear();
            tvwBascula.Refresh();

            for (int i = 0; i < this.myGrupo.Length; i++)
            {
                if (myGrupo[i].nombre != "" && myGrupo[i].nombre != null)
                {
                    TreeNode trG = new TreeNode();
                    trG.Text = myGrupo[i].nombre;
                    trG.Name = myGrupo[i].ngpo.ToString();
                    trG.Tag = "G";
                    trG.SelectedImageIndex = 0;
                    trG.ImageIndex = 0;

                    this.tvwBascula.Nodes.Add(trG);
                    this.tvwBascula.Select();
                    int indi_nodo = this.tvwBascula.Nodes.Count;
                                    
                }
            }

            for (int j = 0; j < myScale.Length; j++)
            {
                if (myScale[j].gpo == 0)
                {
                    TreeNode trB = new TreeNode();
                    trB.Text = myScale[j].nserie;
                    trB.Name = myScale[j].idbas.ToString();
                    trB.Tag = "B";
                    trB.SelectedImageIndex = 1;
                    trB.ImageIndex = 1;
                    this.tvwBascula.Nodes.Add(trB);
                }
            }
        }
        private string Guardar_Cortes_Recibidos(long bascula, long sucursal, string Nserie, string[] Trama, int nregistro, ref int Total_registro)
        {
            string[] Trama_Recibida;
            bool Existe;
            string fecha_reg;
            string fecha_final;
            string Ncorte = "";

            for (int i = 0; i < nregistro; i++)
            {
                Trama_Recibida = Trama[i].Split((char)9);
                Ncorte = Trama_Recibida[0];
                OleDbDataReader OLpen = Conec.Obtiene_Dato("SELECT id_Corte FROM Cortes_Caja WHERE id_Corte = " + Convert.ToInt32(Trama_Recibida[0]) + " and id_bascula = " + bascula, Conec.CadenaConexion);
                if (OLpen.Read()) Existe = true;
                else Existe = false;
                OLpen.Close();

                Trama_Recibida[1] = Trama_Recibida[1].Replace('-', '/');
                fecha_reg = string.Format(Variable.FOR_FECHAS[Variable.ffecha], Convert.ToDateTime(Trama_Recibida[1]));
                fecha_final = string.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now.Date);
                if (!Existe)
                {
                    Conec.CadenaSelect = "INSERT INTO Cortes_Caja " +
                    "(id_bascula,id_grupo, id_Corte,FechaIni,HoraIni,FechaFin,Horafin,Estatus,Transacciones,Totalfolios," +
                    "SubtotalTotal,ImpuestoTotal,DescuentoTotal,OfertaTotal,VentaTotal,DevolucionTotal,nserie)" +
                   "VALUES (" + bascula + "," +   //id_bascula
                                 sucursal + "," +   //id_grupo 
                                 Trama_Recibida[0] + ",'" + //id_corte
                                 fecha_reg + "','" + //Fecha Inicial
                                 Trama_Recibida[2] + "','" + //Hora Inicial
                                  fecha_final + "','" + //Fecha Final
                                 DateTime.Now.ToShortTimeString() + "'," + //Hora final
                                 Trama_Recibida[5] + "," + //Status
                                 Trama_Recibida[6] + "," + //Transacciones
                                 Trama_Recibida[7] + "," + //Total de folio
                                 Trama_Recibida[8] + "," + //Subtotal
                                 Trama_Recibida[9] + "," + //Total de impuesto
                                 Trama_Recibida[10] + "," + //Descuento Total
                                 Trama_Recibida[11] + "," + //Oferta Total
                                 Trama_Recibida[12] + "," + //Venta Total
                                 Trama_Recibida[13] + ",'" + //Devolucion total                                                             
                                 Nserie + "')"; //Numero de serie

                    Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, "Cortes_Caja");
                }
                else
                {
                    Conec.CadenaSelect = "UPDATE Cortes_Caja SET " +
                    "FechaIni = '" + fecha_reg + "', " + //Fecha inicial
                    "HoraIni = '" + Trama_Recibida[2] + "', " + //Hora inicial
                    "FechaFin = '" + fecha_final + "', " + //fech final
                    "Horafin = '" + DateTime.Now.ToShortTimeString() + "', " + //hora final
                    "Estatus = " + Trama_Recibida[5] + ", " + //estatus
                    "Transacciones = " + Trama_Recibida[6] + ", " + //transacciones
                    "Totalfolios = " + Trama_Recibida[7] + ", " + //toal de folios
                    "SubtotalTotal = " + Trama_Recibida[8] + ", " + //Subtotal
                    "ImpuestoTotal = " + Trama_Recibida[9] + ", " + //impuesto total
                    "DescuentoTotal = " + Trama_Recibida[10] + ", " + //descuento total
                    "OfertaTotal = " + Trama_Recibida[11] + ", " + //oferta total
                    "VentaTotal = " + Trama_Recibida[12] + ", " + //venta total
                    "DevolucionTotal = " + Trama_Recibida[13] + ", " + //devolucion total
                    "nserie = '" + Nserie + "'" + //numero de serie
                    " WHERE (id_grupo = " + sucursal + ") AND (id_bascula = " + bascula + ") AND (id_Corte = " + Convert.ToInt32(Trama_Recibida[0]) + ")";

                    Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "Cortes_Caja");
                }               
            }
            Total_registro += nregistro;
            return Ncorte;
        }
        private void Guardar_Ventas_Recibidas(long bascula, long sucursal, string Nserie, string[] Trama, int nregistro, ref int Total_registro)
        {
            string[] Trama_Recibida;
            bool Existe;
            string fecha_reg;
            string fecha_final;
         
            for (int i = 0; i < nregistro; i++)
            {
                Trama_Recibida = Trama[i].Split((char)9);

                //  fecha_reg = Trama_Recibida[13].Replace('-', '/');
                Trama_Recibida[13] = Trama_Recibida[13].Replace('-', '/');
                fecha_reg = string.Format(Variable.FOR_FECHAS[Variable.ffecha], Convert.ToDateTime(Trama_Recibida[13]));
                fecha_final = string.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now.Date);

                OleDbDataReader OLpen = Conec.Obtiene_Dato("SELECT * FROM Ventas WHERE " +
                    "(id_grupo = " + sucursal + ") AND (id_Corte = " + Convert.ToInt32(Trama_Recibida[2].ToString()) + ") AND " +
                    "(id_bascula = " + bascula + ") AND (id_Venta = " + Convert.ToInt32(Trama_Recibida[0].ToString()) + ") and " +
                    "( fecha = '" + fecha_reg +"')", Conec.CadenaConexion);
                if (OLpen.Read()) Existe = true;
                else Existe = false;
                OLpen.Close();
                           
            /*    vendedorTableAdapter.Fill(baseDeDatosDataSet.Vendedor);
                baseDeDatosDataSet.Vendedor.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Vendedor.NombreColumn };
                DataRow DR_temp = baseDeDatosDataSet.Vendedor.Rows.Find(Trama_Recibida[4]);

                int id_vendedor = 0;

                if (DR_temp != null)
                {
                    id_vendedor = Convert.ToInt32(DR_temp["id_vendedor"].ToString());
                }*/

                if (!Existe)
                {
                    Conec.CadenaSelect = "INSERT INTO Ventas " +
                    "(id_bascula,id_grupo, id_Venta, id_Corte,id_Vendedor,Folio,Depto, Cantidad,GranTotal,Subtotal,Descuento,Efectivo,Cambio,Fecha,Hora,Oferta, Impuesto, Nserie,vendedor,registro)" +
                   "VALUES (" + bascula + "," +   //id_bascula
                                 sucursal + "," +   //id_grupo 
                                 Trama_Recibida[0] + "," + //id_Venta
                                 Trama_Recibida[2] + "," + //id_corte
                                 Trama_Recibida[3] + "," + //id_vendedor.ToString() + "," + //id_vendedor
                                 Trama_Recibida[5] + "," + //Folio
                                 Trama_Recibida[6] + "," + //Departamento
                                 Trama_Recibida[7] + "," + //Cantidad
                                 Trama_Recibida[8] + "," + //GranTotal
                                 Trama_Recibida[9] + "," + //Subtotal
                                 Trama_Recibida[10] + "," + //Descuento
                                 Trama_Recibida[11] + "," + //Efectivo
                                 Trama_Recibida[12] + ",'" + //Cambio
                                 fecha_reg + "','" + //Fecha
                                 Trama_Recibida[14] + "'," + //Hora
                                 Trama_Recibida[15] + "," + //Oferta
                                 Trama_Recibida[16] + ",'" + //Impuesto
                                 Trama_Recibida[1] + "','" + //Numero de serie
                                 Trama_Recibida[4] + "','" + //Nombre de vendedor
                                 fecha_final + "')";//fecha del corte

                    Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, "Ventas");
                }
                else
                {
                    Conec.CadenaSelect = "UPDATE Ventas SET " +
                    "Folio = " + Trama_Recibida[5] + "," + //Folio+
                    "Depto = " + Trama_Recibida[6] + "," + //Departamento
                    "Cantidad = " + Trama_Recibida[7] + "," + //Cantidad
                    "GranTotal= " + Trama_Recibida[8] + "," + //GranTotal
                    "Subtotal=" + Trama_Recibida[9] + "," + //Subtotal
                    "Descuento = " + Trama_Recibida[10] + "," + //Descuento
                    "Efectivo = " + Trama_Recibida[11] + "," + //Efectivo
                    "Cambio = " + Trama_Recibida[12] + "," + //Cambio
                    "Fecha = '" + fecha_reg + "'," + //Fecha
                    "Hora= '" + Trama_Recibida[14] + "'," + //Hora
                    "Oferta = " + Trama_Recibida[15] + "," + //Oferta
                    "Impuesto = " + Trama_Recibida[16] +
                    " WHERE (id_grupo = " + sucursal + ") AND (id_Corte = " + Convert.ToInt32(Trama_Recibida[2].ToString()) + ") AND (id_bascula = " + bascula + ") AND (id_Venta = " + Convert.ToInt32(Trama_Recibida[0].ToString()) + ") AND ( Fecha = " + fecha_reg +")";

                    Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "Ventas");
                }
            }
            Total_registro += nregistro;
        }
        private void Guardar_VentasDetalle_Recibidas(long bascula, long sucursal, string Nserie, long Ncorte, string[] Trama, int nregistro, ref int Total_registro)
        {
            bool Existe;
            object[] datos_venta = new object[6];
            object[] clave = new object[5];
            string[] Trama_Recibida;
            string fecha_fin = string.Format(Variable.FOR_FECHAS[Variable.idioma], DateTime.Now.Date);

           

            ventasTableAdapter.Fill(baseDeDatosDataSet.Ventas);
            baseDeDatosDataSet.Ventas.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Ventas.id_basculaColumn, baseDeDatosDataSet.Ventas.id_grupoColumn, baseDeDatosDataSet.Ventas.id_CorteColumn, baseDeDatosDataSet.Ventas.id_VentaColumn, baseDeDatosDataSet.Ventas.registroColumn };

         //   System.IO.StreamWriter fil = new StreamWriter("C:\\PROYECTOS\\recibido.txt",true);
            for (int i = 0; i < nregistro; i++)
            {
               // fil.WriteLine(Trama[i]);

                Trama_Recibida = Trama[i].Split((char)9);

                clave[0] = bascula;
                clave[1] = sucursal;
                clave[2] = Ncorte;  //Trama_Recibida[16]; // Ncorte; //Trama_Recibida[1];//numero de corte
                clave[3] = Trama_Recibida[1];  //id de venta
                clave[4] = fecha_fin; //fecha_fin del corte

                Double Descuento = 0;
                Double Oferta = 0;
                Double Devolucion = 0;
                Double Subtotal = Convert.ToDouble(Trama_Recibida[13]);
                String Nombre_Producto = Trama_Recibida[5];
                                
                if (Convert.ToInt16(Trama_Recibida[12]) == 1)  
                {
                    Descuento = Convert.ToDouble(Trama_Recibida[10]);
                    Subtotal = 0;
                }
                if (Convert.ToInt16(Trama_Recibida[12]) == 2)
                {
                    Oferta = Convert.ToDouble(Trama_Recibida[10]);
                    Subtotal = 0;
                }
                if (Convert.ToInt16(Trama_Recibida[6]) == 1)
                {
                    Devolucion = Convert.ToDouble(Trama_Recibida[10]);
                    Subtotal = 0;
                }                
                if (Convert.ToInt32(Trama_Recibida[3]) == 0)
                {
                    Nombre_Producto = Variable.SYS_MSJ[218, Variable.idioma];
                }
                DataRow DR = baseDeDatosDataSet.Ventas.Rows.Find(clave);

                if (DR != null)
                {
                    datos_venta[0] = DR["id_corte"]; //id_corte                    
                    datos_venta[1] = DR["id_vendedor"]; //id_Vendedor
                    datos_venta[2] = DR["Folio"]; //FOLIO
                    datos_venta[3] = DR["Fecha"]; //FECHA de la venta
                    datos_venta[4] = DR["Hora"]; //HORA
                    datos_venta[5] = DR["Depto"]; //departamento

                  Conec.CadenaSelect = "id_bascula = " + bascula + " and id_grupo = " + sucursal + 
                                       " and id_detalle = " + Convert.ToInt32(Trama_Recibida[0]) + " and id_Venta = " + Convert.ToInt32(Trama_Recibida[1])  + 
                                       " and id_Corte = " +  Convert.ToInt32(datos_venta[0])+ " and id_Vendedor = " +  Convert.ToInt32(datos_venta[1]) +
                                        //" and id_Corte = " + Convert.ToInt32(Trama_Recibida[16]) + " and id_Vendedor = " + Convert.ToInt32(Trama_Recibida[17]) +
                                       " and Fecha = '" + datos_venta[3] +"'";
                    OleDbDataReader OLpen = Conec.Obtiene_Dato("SELECT * FROM Ventas_Detalle WHERE (" + Conec.CadenaSelect + ")", Conec.CadenaConexion);
                    if (OLpen.Read()) Existe = true;
                    else Existe = false;
                    OLpen.Close();

                    if (!Existe)
                    {                       
                        Conec.CadenaSelect = "INSERT INTO Ventas_Detalle " +
                        "(id_bascula,id_grupo, id_detalle,id_Venta,id_Corte,id_Vendedor,Linea,Codigo,NoPLU,Nombre,Devolucion,TipoPLU,Peso,Precio,Total,Impuesto,TipoItem,subtotal,descuento,Oferta,Fecha, Hora, Depto,Nserie,registro)" +
                       "VALUES (" + bascula + "," +   //id_bascula
                                     sucursal + "," +   //id_grupo 
                                     Trama_Recibida[0] + "," + //id_detalle
                                     Trama_Recibida[1] + "," + //id_venta
                                     datos_venta[0] + "," + // Trama_Recibida[16] + "," + //id_Corte
                                     datos_venta[1] + "," + //Trama_Recibida[17] + "," + //id_vendedor
                                     Trama_Recibida[2] + "," + //Linea
                                     Convert.ToInt32(Trama_Recibida[3]) + "," + //Codigo
                                     Trama_Recibida[4] + ",'" + //NoPLU
                                     Nombre_Producto +"'," + //Nombre Trama_Recibida[5]
                                     Devolucion + ",'" + //Trama_Recibida[6] + ",'" + //Devolucion
                                     Trama_Recibida[7] + "','" + //TipoId  editado, no editado
                                     Trama_Recibida[8] + "'," + //Peso
                                     Trama_Recibida[9] + "," + //Precio
                                     Trama_Recibida[10] + "," + //Total
                                     Trama_Recibida[11] + "," + //Impuesto
                                     Trama_Recibida[12] + "," + //TipoItem  pesado, no pesado
                                     Subtotal + "," +  //Trama_Recibida[13] + "," + //Subtotal
                                     Descuento + "," + //Trama_Recibida[14] + "," + //Descuento
                                     Oferta + ",'" + //Trama_Recibida[15] + ",'" + //Oferta
                                     datos_venta[3] + "','" + //Fecha de la venta
                                     datos_venta[4] + "'," + //Hora
                                     datos_venta[5] + ",'" + //departamento
                                     Nserie + "','" + //Nserie
                                     fecha_fin + "')"; //FECHA DE REGISTRO

                        Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, "Ventas_Detalle");
                    }
                    else
                    {
                        Conec.CadenaSelect = "UPDATE Ventas_Detalle SET " +
                        "Linea = " + Trama_Recibida[2] + "," + //Linea
                        "Codigo = " + Convert.ToInt32(Trama_Recibida[3]) + "," + //Codigo
                        "NoPLU = " + Trama_Recibida[4] + "," + //NoPLU
                        "Nombre = '" + Nombre_Producto +"'," + //Nombre Trama_Recibida[5]
                        "Devolucion = " + Devolucion +"," + //Devolucion Trama_Recibida[6]
                        "TipoPLU = '" + Trama_Recibida[7] + "'," + //TipoID
                        "Peso = '" + Trama_Recibida[8] + "'," + //Peso
                        "Precio = " + Trama_Recibida[9] + "," + //Precio
                        "Total = " + Trama_Recibida[10] + "," + //Total
                        "Impuesto = " + Trama_Recibida[11] + "," + //Impuesto
                        "TipoItem = " + Trama_Recibida[12] + "," + //TipoItem
                        "subtotal = " + Subtotal +"," + //Subtotal Trama_Recibida[13]
                        "descuento = " + Descuento +"," +//Descuento Trama_Recibida[14]
                        "Oferta = " + Oferta +"," +//Oferta Trama_Recibida[15]
                     //   "Fecha = '" + datos_venta[3] + "'," + //Fecha
                        "Hora = '" + datos_venta[4] + "'," + //Hora
                        "Depto = " + datos_venta[5] + //Departamento
                        " WHERE (id_bascula = " + bascula + " and id_grupo = " + sucursal + 
                               " and id_Venta = " + Convert.ToInt32(Trama_Recibida[1]) + 
                               " and id_detalle = " + Convert.ToInt32(Trama_Recibida[0]) +
                               " and id_Corte = " + Convert.ToInt32(datos_venta[0]) +  // Convert.ToInt32(Trama_Recibida[16]) + //Convert.ToInt32(datos_venta[0]) +  
                               " and id_vendedor = " + Convert.ToInt32(datos_venta[1]) + // Convert.ToInt32(Trama_Recibida[17]) + //Convert.ToInt32(datos_venta[1]) +
                               " and Fecha = '" + datos_venta[3] + "')"; //Hora

                        Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "Ventas_Detalle");
                    }
                }
            }
            //fil.Close();
            Total_registro += nregistro;
        }
        #endregion

        #region ListView y treeView      
        private void tvwBascula_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView nodo = ((TreeView)sender);
            nodo.Select();
            nodo.Focus();            
            Consulta_EnBD(nodo.SelectedNode.Name, nodo.SelectedNode.Tag.ToString());           
        }
        private void Consulta_EnBD(string ncod, string tipo)
        {
            QuitarBackColor();

            if (tipo == "B")
            {
                DataRow dr = baseDeDatosDataSet.Bascula.Rows.Find(Convert.ToInt32(ncod));
                if (dr != null)
                {
                    myCurrent.idbas = Convert.ToInt32(dr["id_bascula"].ToString());
                    myCurrent.gpo = 0;
                    myCurrent.ip = dr["dir_ip"].ToString();
                    myCurrent.nombre = dr["no_serie"].ToString() + "-" + dr["nombre"].ToString();
                    myCurrent.Nserie = dr["no_serie"].ToString();
                    myCurrent.tipo = Convert.ToInt32(dr["tipo_conec"]);                   
                }                
            }
            else
            {
                DataRow dr = baseDeDatosDataSet.Grupo.Rows.Find(Convert.ToInt32(ncod));
                if (dr != null)
                {
                    myCurrent.gpo = Convert.ToInt32(dr["id_grupo"].ToString());
                    myCurrent.idbas = 0;
                    myCurrent.ip = "0.0.0.0";
                    myCurrent.nombre = dr["nombre_gpo"].ToString() + " " + dr["descripcion"].ToString();
                    myCurrent.Nserie = dr["nombre_gpo"].ToString();
                   // myCurrent.tipo = Convert.ToInt32(dr["tipo_conec"]);   
                }

            }

        }
        private void Mostra_Cortes_recibidos(long bascula, long sucursal, string Nserie, string[] Trama_recibida, int nregistroo)
        {
            string[] Trama = null;
            string fecha_reg, fecha_final;


            if (Trama_recibida[0].Length > 1)
            {
                Trama = Trama_recibida[0].Split((char)9);
                fecha_reg = Trama[1].Replace('-', '/');
                fecha_final = string.Format("{0:yyyy/MM/dd}", DateTime.Now.Date);

                ListViewItem lcorte = new ListViewItem(Nserie);
                lcorte.SubItems.Add(fecha_reg);  //Fecha inicial
                lcorte.SubItems.Add(fecha_final);  //Fecha Final
                lcorte.SubItems.Add(Trama[6]); //transacciones
                lcorte.SubItems.Add(Trama[13]); //devolucion total
                lcorte.SubItems.Add(Trama[10]); //descuentoTotal
              //  lcorte.SubItems.Add(Trama[8]); //Subtotal
                lcorte.SubItems.Add(Trama[11]); //Ofertas
                lcorte.SubItems.Add(Trama[12]); //ventaTotal                                 
                lvwVentas.Items.Add(lcorte);
                //MessageBox.Show(this, Variable.SYS_MSJ[28, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information); //"Los Datos han sido enviado"
            }
          
        }
        #endregion

        public bool RecibirVentasD_Bascula(string direccionIP, long nSucursal, long nbascula,string nserie)
        {
            Cursor.Current = Cursors.WaitCursor;
            char[] chr = new char[2] { (char)10, (char)13 };   
            string recibe_venta=null, recibe_detalle=null;
            string recibe_corte=null;
            int total_registro = 0;
            string strcomando;
            long Ncorte=0;
            bool ERROR = false;

            Cliente_bascula = Cte.conectar(direccionIP, 50036);  //, Variable.frame, ref Dato_Recivido);
            if (Cliente_bascula != null)
            {
                string sComando = "XX" + (char)9 + (char)10;

                string Msg_Recibido = Env.Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");
                if (Msg_Recibido != null)
                {
                    WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[200, Variable.idioma] + " " + nserie + "...");
                    Thread t = new Thread(workerObject.vShowMsg);
                    t.Start();

                    strcomando = "LvXX" + (char)9 + (char)10;
                    recibe_venta = Command_Recibido(nserie, Ncorte, strcomando, direccionIP, ref Cliente_bascula, nbascula, nSucursal, ref total_registro, true);
                    strcomando = "LcXX" + (char)9 + (char)10;
                    recibe_corte = Command_Recibido(nserie, Ncorte, strcomando, direccionIP, ref Cliente_bascula, nbascula, nSucursal, ref total_registro, true);
                    if (recibe_corte.Length > 0)
                    {
                        Ncorte = Convert.ToInt32(recibe_corte);
                        strcomando = "LdXX" + (char)9 + (char)10;
                        recibe_detalle = Command_Recibido(nserie, Ncorte, strcomando, direccionIP, ref Cliente_bascula, nbascula, nSucursal, ref total_registro, true);
                    }
                    Env.Command_Limpiar(direccionIP, ref Cliente_bascula, "SXXX" + (char)9 + (char)10);

                    workerObject.vEndShowMsg();
                }
                else
                {
                    ERROR = true;
                }

                Cte.desconectar(ref Cliente_bascula);
                Cursor.Current = Cursors.Default;                
            }
            else
            {
                Cursor.Current = Cursors.Default;
                ERROR = true;
            }

            return ERROR;
        }
        public bool RecibirConsulta_Bascula(string direccionIP, long nSucursal, long nbascula, string nserie)
        {
            Cursor.Current = Cursors.WaitCursor;
            string recibe_corte=null;
            int total_registro = 0;
            string strcomando;
            bool ERROR = false;

            Cliente_bascula = Cte.conectar(direccionIP, 50036);  //, Variable.frame, ref Dato_Recivido);
            if (Cliente_bascula != null)
            {
                string sComando = "XX" + (char)9 + (char)10;
                string Msg_Recibido = Env.Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");
                if (Msg_Recibido != null)
                {
                    WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[200, Variable.idioma] + " " + nserie + "...");
                    Thread t = new Thread(workerObject.vShowMsg);
                    t.Start();

                    strcomando = "LcXX" + (char)9 + (char)10;
                    recibe_corte = Command_Recibido(nserie, 0, strcomando, direccionIP, ref Cliente_bascula, nbascula, nSucursal, ref total_registro, false);

                    workerObject.vEndShowMsg();
                }
                else
                {
                    ERROR = true;
                }
                Cte.desconectar(ref Cliente_bascula);
                Cursor.Current = Cursors.Default;                
            }
            else
            {
                Cursor.Current = Cursors.Default;
                ERROR = true;
            }

            return ERROR;
        }
        public bool RecibirVentasD_Bascula(string puerto, Int32 Baud_rate, long nSucursal, long nbascula, string nserie)
        {
            Cursor.Current = Cursors.WaitCursor;
            bool recibe_venta = true, recibe_detalle = true, recibe_corte = true; 
            string strcomando;
            int total_registro = 0;
            bool ERROR = false;

            serialPort1 = new SerialPort();
            if (SR.OpenPort(ref serialPort1, puerto, Baud_rate))
            {
                SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)9 + (char)10);

                WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[24, Variable.idioma] + " " + nserie + "...");
                Thread t = new Thread(workerObject.vShowMsg);
                t.Start();
              
                strcomando = "LvXX" + (char)9 + (char)10;
                recibe_venta = Command_Recibido(nserie, strcomando, ref serialPort1, nbascula, nSucursal, ref total_registro,true);              
                                      
                strcomando = "LcXX" + (char)9 + (char)10;
                recibe_corte = Command_Recibido(nserie, strcomando, ref serialPort1, nbascula, nSucursal, ref total_registro,true);
                
                strcomando = "LdXX" + (char)9 + (char)10;
                recibe_detalle = Command_Recibido(nserie, strcomando, ref serialPort1, nbascula, nSucursal, ref total_registro, true);
     
                SR.SendCOMSerial(ref serialPort1, "SXXX" + (char)9 + (char)10);  //ELIMINAR VENTA
                      
                SR.SendCOMSerial(ref serialPort1, "DXXX" + (char)9 + (char)10);

                SR.ClosePort(ref serialPort1);

                workerObject.vEndShowMsg();

                Cursor.Current = Cursors.Default;              
            }
            else
            {
                Cursor.Current = Cursors.Default;
                ERROR = true;
            }

            return ERROR;
        }
        public bool RecibirConsulta_Bascula(string puerto, Int32 Baud_rate, long nSucursal, long nbascula, string nserie)
        {
            Cursor.Current = Cursors.WaitCursor;
            bool recibe_corte = true;
            string strcomando;
            int total_registro = 0;
            bool ERROR = false;

            serialPort1 = new SerialPort();
            if (SR.OpenPort(ref serialPort1, puerto, Baud_rate))
            {
                SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)9 + (char)10);

                WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[200, Variable.idioma] + " " + nserie + "...");
                Thread t = new Thread(workerObject.vShowMsg);
                t.Start();
                
                strcomando = "LcXX" + (char)9 + (char)10;
                recibe_corte = Command_Recibido(nserie, strcomando, ref serialPort1, nbascula, nSucursal, ref total_registro,false);

                SR.SendCOMSerial(ref serialPort1, "DXXX" + (char)9 + (char)10);

                SR.ClosePort(ref serialPort1);

                workerObject.vEndShowMsg();
                
                Cursor.Current = Cursors.Default;
            }
            else
            {
                Cursor.Current = Cursors.Default;
                ERROR = true;
            }

            return ERROR;
        }
        
        /// <summary>
        /// COMANDO DE RECIBIDO DE LA BASCULA
        /// </summary>
        /// <param name="reg_leido"></param>
        /// <param name="Trama_Enviada"></param>
        /// <param name="direccionIP"></param>
        /// <param name="Cliente_bascula"></param>
        /// <param name="bascula"></param>
        /// <param name="grupo"></param>
        public string Command_Recibido(string Nserie, long Ncorte, string comando, string direccionIP, ref Socket Cliente_bascula, long bascula, long grupo, ref int Total_registro, bool CORTE)
        {
            char[] chr = new char[2] { (char)10, (char)13 };       
            string reg_enviado, strcomando;
            string reg_recibido="";
            int nregistro;
            bool continuar = true;
            string Ocupado = "";
            strcomando = comando;

            System.IO.StreamWriter fil = new StreamWriter(Variable.appPath + "\\recibido.txt", true);

            reg_enviado = Cte.Recibir_Respuesta(ref Cliente_bascula, direccionIP, strcomando);
           
            while (continuar)
            {
                fil.WriteLine(reg_enviado);

                if (reg_enviado.IndexOf("Error") >= 0)
                {
                    Env.Guardar_Trama_pendiente(bascula, grupo, strcomando);
                    continuar = false;
                }
                else
                {
                    if (reg_enviado.Length > 0)
                    {
                        comando = reg_enviado.Substring(0, 2);
                        nregistro = Convert.ToInt16(reg_enviado.Substring(2, 2));

                        reg_recibido = reg_enviado.Substring(4);
                      
                        if (comando == "Lv" && nregistro > 0) Guardar_Ventas_Recibidas(bascula, grupo, Nserie, reg_recibido.Split(chr), nregistro, ref Total_registro);
                        if (comando == "Ld" && nregistro > 0) Guardar_VentasDetalle_Recibidas(bascula, grupo, Nserie, Ncorte, reg_recibido.Split(chr), nregistro, ref Total_registro);
                        if (comando == "Lc" && nregistro > 0)
                        {
                            if (CORTE)
                            {
                                Ocupado = Guardar_Cortes_Recibidos(bascula, grupo, Nserie, reg_recibido.Split(chr), nregistro, ref Total_registro);
                            }
                           
                            Mostra_Cortes_recibidos(bascula, grupo, Nserie, reg_recibido.Split(chr), nregistro);
                        }                        
                    }
                    if (reg_enviado.IndexOf("End") > 0)
                    {
                        continuar = false;
                    }
                    else
                    {
                        continuar = true;
                        reg_enviado = Cte.Recibir_Respuesta(ref Cliente_bascula, direccionIP, comando + "XXOk" + (char)9 + (char)10);
                        if (reg_enviado.Length == 0) continuar = false;
                    }
                }
            }
            fil.Close();
           // Ocupado = false;
            return Ocupado;
        }
        public bool Command_Recibido(string Nserie, string strcomando, ref SerialPort Cliente_bascula, long bascula, long grupo, ref int Total_registro, bool CORTE)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Dato_Recibido;
            string reg_recibido;
            string comando="";
            int nregistro;
            long Ncorte = 0;
            bool continuar = true;
            bool Ocupado = false;
                        
            SR.SendCOMSerial(ref Cliente_bascula, strcomando); 

            while (continuar)
            {
                Dato_Recibido = SR.ReceivedCOMSerial(ref Cliente_bascula);

                if (Dato_Recibido.IndexOf("Error") >= 0)
                {
                    Env.Guardar_Trama_pendiente(bascula, grupo, strcomando);
                    continuar = false;
                }
                else
                {
                    if (Dato_Recibido.Length > 0)
                    {
                        comando = Dato_Recibido.Substring(0, 2);
                        nregistro = Convert.ToInt16(Dato_Recibido.Substring(2, 2));
                        
                        reg_recibido = Dato_Recibido.Substring(2);
                        if (comando == "Lv" && nregistro > 0) Guardar_Ventas_Recibidas(bascula, grupo, Nserie, reg_recibido.Split(chr), nregistro, ref Total_registro);
                       
                        if (comando == "Lc" && nregistro > 0)
                        {
                            if (CORTE) Guardar_Cortes_Recibidos(bascula, grupo, Nserie, reg_recibido.Split(chr), nregistro, ref Total_registro);
                            Mostra_Cortes_recibidos(bascula, grupo, Nserie, reg_recibido.Split(chr), nregistro);
                        }
                        if (comando == "Ld" && nregistro > 0) Guardar_VentasDetalle_Recibidas(bascula, grupo, Nserie, Ncorte, reg_recibido.Split(chr), nregistro, ref Total_registro);
                    }
                    if (Dato_Recibido.IndexOf("End") > 0) continuar = false;
                    else
                    {
                        continuar = true;
                        SR.SendCOMSerial(ref Cliente_bascula, comando + "XXOk" + (char)9 + (char)10);
                    }

                }
            }
            Ocupado = false;

            return Ocupado;
        }

        private bool find_node_tree(TreeViewBound.TreeNodeBound sub_tree, TreeViewBound.TreeNodeBound DropNode)
        {
            int n_nodo = sub_tree.Nodes.Count;
            if (n_nodo > 0)
            {
                int i = 0;
                while (i < n_nodo)
                {
                    TreeViewBound.TreeNodeBound tn = (TreeViewBound.TreeNodeBound)sub_tree.Nodes[i];
                    if (Convert.ToInt32(DropNode.Value) == Convert.ToInt32(tn.Value))
                    {
                        exist_nodo = true;
                        i = n_nodo;
                        break;
                    }
                    else
                    {
                        if (tn.Nodes.Count > 0) return find_node_tree(tn, DropNode);
                        if (!exist_nodo) i++;
                    }
                }
            }
            return exist_nodo;
        }
       
        private void f5Cortes_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Vendedor' Puede moverla o quitarla según sea necesario.
            this.vendedorTableAdapter.Fill(this.baseDeDatosDataSet.Vendedor);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Ventas_Detalle' Puede moverla o quitarla según sea necesario.
            this.ventas_DetalleTableAdapter.Fill(this.baseDeDatosDataSet.Ventas_Detalle);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Ventas' Puede moverla o quitarla según sea necesario.
            this.ventasTableAdapter.Fill(this.baseDeDatosDataSet.Ventas);
            this.grupoTableAdapter.Fill(this.baseDeDatosDataSet.Grupo);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Bascula' Puede moverla o quitarla según sea necesario.
            this.basculaTableAdapter.Fill(this.baseDeDatosDataSet.Bascula);
            this.vendedorTableAdapter.Fill(this.baseDeDatosDataSet.Vendedor);
            if (this.tvwBascula.GetNodeCount(false) > 0) this.tvwBascula.SelectedNode = this.tvwBascula.TopNode;
        }
                                
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void f5Cortes_Activated(object sender, EventArgs e)
        {
            Asigna_Grupo();
            Asigna_Bascula();
            Crear_Nodos_Basculas();
            if (this.tvwBascula.GetNodeCount(false) > 0) this.tvwBascula.SelectedNode = this.tvwBascula.TopNode;
        }

        private void ribCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }        
    }
}



