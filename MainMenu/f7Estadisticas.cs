using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;



namespace MainMenu
{
    public partial class f7Estadisticas : MdiChildForm
    {

        #region Declaracion Class
        ADOutil Conec = new ADOutil();
        ReportParameter[] paramFields;
        BaseDeDatosDataSet basededataset1 = new BaseDeDatosDataSet();

        BaseDeDatosDataSetTableAdapters.ProductosTableAdapter productosTableAdapter = new BaseDeDatosDataSetTableAdapters.ProductosTableAdapter();
        BaseDeDatosDataSetTableAdapters.OfertaTableAdapter ofertasTableAdapter = new BaseDeDatosDataSetTableAdapters.OfertaTableAdapter();
        BaseDeDatosDataSetTableAdapters.PublicidadTableAdapter publicidadTableAdapter = new BaseDeDatosDataSetTableAdapters.PublicidadTableAdapter();
        BaseDeDatosDataSetTableAdapters.BasculaTableAdapter basculasTableAdapter = new BaseDeDatosDataSetTableAdapters.BasculaTableAdapter();
        BaseDeDatosDataSetTableAdapters.VendedorTableAdapter vendedoresTableAdapter = new BaseDeDatosDataSetTableAdapters.VendedorTableAdapter();
        BaseDeDatosDataSetTableAdapters.VentasTableAdapter ventasTableAdapter = new BaseDeDatosDataSetTableAdapters.VentasTableAdapter();
        BaseDeDatosDataSetTableAdapters.Ventas_DetalleTableAdapter ventadetalleTableAdapter = new BaseDeDatosDataSetTableAdapters.Ventas_DetalleTableAdapter();
        BaseDeDatosDataSetTableAdapters.IngredientesTableAdapter ingredienteTableAdapter = new BaseDeDatosDataSetTableAdapters.IngredientesTableAdapter();
        BaseDeDatosDataSetTableAdapters.Cortes_CajaTableAdapter cortescajaTableAdapter = new BaseDeDatosDataSetTableAdapters.Cortes_CajaTableAdapter();

        Color colorbefore;
        #endregion

        #region metodo constructor
        public f7Estadisticas()
        {
            InitializeComponent();
            tbxFeinicio.CustomFormat = Variable.CUST_DATE[Variable.idioma];
            tbxFefinal.CustomFormat = Variable.CUST_DATE[Variable.idioma];
            tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + string.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);
        }
        #endregion

        #region Evento Load
        private void f7Estadisticas_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'BaseDeDatosDataSet.Ventas_Detalle' Puede moverla o quitarla según sea necesario.
            this.Ventas_DetalleTableAdapter.Fill(this.BaseDeDatosDataSet.Ventas_Detalle);

        }
        #endregion

        #region Manejo de Treeview
        private void treeListado_AfterSelect(object sender, TreeViewEventArgs e)
        {
            visiblePanel(true);
            limpiezaTextBoxes();
            TreeView nodo = ((TreeView)sender);
            nodo.Select();
            nodo.Focus();
            activarDesactivarEdicion(nodo.SelectedNode);
        }
        private void listadoId_EnBD()
        {
            try
            {
                treListadoR.Nodes.Clear();
                Cargar_basculas();
                limpiezaTextBoxes();
                visibleGrupoBox1(false);
                visibleGrupoBox2(false);

                switch ((int)Form1.btnEdicion)
                {
                    case (int)ESTADO.botonesEdicionEnum.PKGENERAL:
                        treListadoR.Nodes.Add(Variable.SYS_MSJ[119, Variable.idioma]); //"Catalago de Basculas");
                        treListadoR.Nodes.Add(Variable.SYS_MSJ[123, Variable.idioma]); //"Catalogo de Productos");
                        treListadoR.Nodes.Add(Variable.SYS_MSJ[120, Variable.idioma]); //"Catalogo de Info adicional");
                        if (Variable.TypeScale == Variable.TipoBascula.WLABEL){treListadoR.Nodes.Add(Variable.SYS_MSJ[121, Variable.idioma]);}
                        treListadoR.Nodes.Add(Variable.SYS_MSJ[122, Variable.idioma]); //"Catalogo de Ofertas");
                        treListadoR.Nodes.Add(Variable.SYS_MSJ[124, Variable.idioma]); //"Catalogo de Vendedor");
                        break;
                    case (int)ESTADO.botonesEdicionEnum.PKSTADITICO:
                        treListadoR.Nodes.Add(Variable.SYS_MSJ[126, Variable.idioma]); //"Analisis de venta por Grupo");
                        treListadoR.Nodes.Add(Variable.SYS_MSJ[127, Variable.idioma]); //"Analisis de venta por Bascula");
                        treListadoR.Nodes.Add(Variable.SYS_MSJ[128, Variable.idioma]); //"Analisis de venta por Producto");
                        treListadoR.Nodes.Add(Variable.SYS_MSJ[129, Variable.idioma]); //"Analisis de venta por Vendedor");
                        break;
                    case (int)ESTADO.botonesEdicionEnum.PKVENTAS:
                        treListadoR.Nodes.Add(Variable.SYS_MSJ[130, Variable.idioma]); //"Reporte de Venta por Fecha");
                        treListadoR.Nodes.Add(Variable.SYS_MSJ[131, Variable.idioma]); //"Reporte de Venta por Grupo");
                        treListadoR.Nodes.Add(Variable.SYS_MSJ[132, Variable.idioma]); //"Reporte de venta por Bascula");
                        treListadoR.Nodes.Add(Variable.SYS_MSJ[133, Variable.idioma]); //"Reporte de venta por Corte de Ventas");           
                        treListadoR.Nodes.Add(Variable.SYS_MSJ[134, Variable.idioma]); //"Reporte de venta por Vendedor");                                     
                        treListadoR.Nodes.Add(Variable.SYS_MSJ[135, Variable.idioma]); //"Reporte de venta por Producto");  
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Excepcion " + ex);
            }

        }
        #endregion

        #region Activacion de controles en la vista
        void activarDesactivarEdicion(TreeNode Nodo)
        {
            if (Form1.btnEdicion == ESTADO.botonesEdicionEnum.PKSTADITICO)
                visibleGrupoBox1(true);
            if (Form1.btnEdicion == ESTADO.botonesEdicionEnum.PKVENTAS)
                visibleGrupoBox1(true);
            if (Form1.btnEdicion == ESTADO.botonesEdicionEnum.PKGENERAL)
            {

                switch (Nodo.Index)
                {
                    case 0:
                        {
                            visibleGrupoBox2(true);
                            label9.Text = Variable.SYS_MSJ[224,Variable.idioma] +":";  //N/serie
                        } break;
                    case 1:
                        {
                            visibleGrupoBox2(true);
                            label9.Text = Variable.SYS_MSJ[167, Variable.idioma] + ":";  // "Productos :";
                        } break;
                    case 2:
                        {
                            visibleGrupoBox2(true);
                            label9.Text = Variable.SYS_MSJ[149, Variable.idioma] + ":"; //"Ingredientes :";
                        } break;
                    case 3:
                        {
                            visibleGrupoBox2(true);
                            label9.Text = Variable.SYS_MSJ[168, Variable.idioma] + ":";   //"Mensajes :";
                        } break;
                    case 4:
                        {
                            visibleGrupoBox2(true);
                            label9.Text = Variable.SYS_MSJ[165, Variable.idioma] + ":";   //"Ofertas :";
                        } break;
                    case 5:
                        {
                            visibleGrupoBox2(true);
                            label9.Text = Variable.SYS_MSJ[175, Variable.idioma] + ":"; //"Vendedores :";
                        } break;
                }

                tbxDatoini.Focus();
            }
        }
        void visiblePanel(bool pbVisible)
        {
            pnldetalle.Enabled = pbVisible;
            pnldetalle.Visible = pbVisible;
            pnldetalle.BringToFront();
        }
        void visibleGrupoBox1(bool pbActivar)
        {
            tbxFeinicio.Enabled = pbActivar;
            tbxProdini.Enabled = pbActivar;
            tbxVendini.Enabled = pbActivar;
            tbxFefinal.Enabled = pbActivar;            
            tbxProdfin.Enabled = pbActivar;
            tbxVendfin.Enabled = pbActivar;

            cBxNserie.Enabled = pbActivar;

            groupBox1.Location = new Point(20, 18);
            groupBox1.Visible = pbActivar;
            groupBox1.Enabled = pbActivar;

            btnImprimir.Visible = pbActivar;
            tbxFeinicio.Focus();
        }
        void visibleGrupoBox2(bool pbActivar)
        {
            tbxDatoini.Enabled = pbActivar;
            tbxDatofin.Enabled = pbActivar;
            groupBox2.Location = new Point(20, 18);
            groupBox2.Visible = pbActivar;
            groupBox2.Enabled = pbActivar;
            btnImprimir.Visible = pbActivar;
            tbxDatoini.Focus();
        }
        private void limpiezaTextBoxes()
        {
            tbxProdini.Clear();
            tbxVendini.Clear();
            tbxDatoini.Clear();
            tbxProdfin.Clear();
            tbxVendfin.Clear();
            tbxDatoini.Clear();
            tbxDatofin.Clear();
            cBxNserie.SelectedIndex = 0;
            tbxFeinicio.Value = DateTime.Now.Date;
            tbxFefinal.Value = DateTime.Now.Date;
            tbxFecha.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);
            if (pnldetalle.Controls.Count > 3) pnldetalle.Controls.RemoveAt(3);
            //reportViewer1.Visible = false;
            //reportViewer1.Dock = DockStyle.None;
        }
        private void Cargar_basculas()
        {
            try
            {
                basculasTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                basculasTableAdapter.Connection.CreateCommand().CommandText = "SELECT * FROM Bascula ORDER BY id_bascula";
                basculasTableAdapter.Fill(basededataset1.Bascula);

                ArrayList lBascula = new ArrayList();
                lBascula.Add(new USState("----------", 0));

                if (basededataset1.Bascula.Count > 0)
                {
                    foreach (DataRow dr in basededataset1.Bascula.Rows)
                    {
                        lBascula.Add(new USState(dr["no_serie"].ToString() + " - " + dr["nombre"].ToString(), Convert.ToInt32(dr["id_bascula"])));
                    }
                }
                this.cBxNserie.DataSource = lBascula;
                this.cBxNserie.ValueMember = "ShortName";
                this.cBxNserie.DisplayMember = "LongName";

                this.cBxNserie.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Excepcion " + ex);
            }
        }
        #endregion
        
        #region Eventos del click
        #region toolstrips
        private void ribGeneral_Click(object sender, EventArgs e)
        {
            Form1.btnEdicion = ESTADO.botonesEdicionEnum.PKGENERAL;
            listadoId_EnBD();
        }
        private void ribEstadistica_Click(object sender, EventArgs e)
        {
            Form1.btnEdicion = ESTADO.botonesEdicionEnum.PKSTADITICO;
            listadoId_EnBD();
        }
        private void ribVentas_Click(object sender, EventArgs e)
        {
            Form1.btnEdicion = ESTADO.botonesEdicionEnum.PKVENTAS;
            listadoId_EnBD();
        }        
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
        #endregion
        #endregion
        #region Eventos de KeyDown
        private void tbxDatoini_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.tbxDatofin.Focus();
        }
        private void tbxDatofin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.btnImprimir.Focus();
        }
        private void tbxFeinicio_ValueChanged(object sender, EventArgs e)
        {
            this.tbxFefinal.Focus();
        }
        private void tbxFefinal_ValueChanged(object sender, EventArgs e)
        {
            this.cBxNserie.Focus();
        }
        private void tbxProdini_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.tbxProdfin.Focus();
        }
        private void tbxProdfin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.tbxVendini.Focus();
        }
        private void tbxVendini_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.tbxVendfin.Focus();
        }
        private void tbxVendfin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.btnImprimir.Focus();
        }
        private void tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (e.KeyChar == 8 || e.KeyChar == 32)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        #endregion        
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            string nombre_reporte = "";
            string filtro = "";
            string condi = "";
            bool hay = false;
            bool tipo_orientacion = false;
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            #region VALIDACIONES

            if (Form1.btnEdicion == ESTADO.botonesEdicionEnum.PKGENERAL)//Reporte general
            {
                if (this.tbxDatoini.Text != "")
                {
                    if (this.tbxDatofin.Text == "")
                    {
                        MessageBox.Show(this.label9.Text + " " + this.label3.Text + Variable.SYS_MSJ[203, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation); //" no puede estar vacia");
                        this.tbxVendfin.Focus();
                        return;
                    }
                }
                else
                {
                    if (this.tbxDatofin.Text != "")
                    {
                        MessageBox.Show(this.label9.Text + " " + this.label4.Text + Variable.SYS_MSJ[203, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation); //" no puede estar vacia");
                        this.tbxVendini.Focus();
                        return;
                    }
                }
                if (tbxDatoini.Text != "" && tbxDatofin.Text != "")
                {
                    if (Convert.ToInt32(tbxDatoini.Text) > Convert.ToInt32(tbxDatofin.Text))
                    {
                        MessageBox.Show(this.label3.Text + " " + this.label4.Text + Variable.SYS_MSJ[205, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation); //" no puede ser menor que la inicial");
                        tbxVendfin.Focus();
                        return;
                    }
                }
            }//finaliza validaciones de reporte general
            else
            {
                if (tbxFefinal.Value.Date < tbxFeinicio.Value.Date)
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[76, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //"La fecha final no puede ser menor que la fecha inicial", "Alerta!!!");
                    tbxFefinal.Focus();
                }
                if (this.tbxVendini.Text != "")
                {
                    if (this.tbxVendfin.Text == "")
                    {
                        MessageBox.Show(this.label8.Text + " " + this.Label2.Text + Variable.SYS_MSJ[203, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation); //" no puede estar vacia");
                        this.tbxVendfin.Focus();
                        return;
                    }
                }
                else
                {
                    if (this.tbxVendfin.Text != "")
                    {
                        MessageBox.Show(this.label8.Text + " " + this.Label1.Text + Variable.SYS_MSJ[203, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation); //" no puede estar vacia");
                        this.tbxVendini.Focus();
                        return;
                    }
                }
                if (tbxVendini.Text != "" && tbxVendfin.Text != "")
                {
                    if (Convert.ToInt32(tbxVendini.Text) > Convert.ToInt32(tbxVendfin.Text))
                    {
                        MessageBox.Show(this.label8.Text + " " + this.Label2.Text + Variable.SYS_MSJ[205, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation); //" no puede ser menor que la inicial");
                        tbxVendfin.Focus();
                        return;
                    }
                }
                if (this.tbxProdini.Text != "")
                {
                    if (this.tbxProdfin.Text == "")
                    {
                        MessageBox.Show(this.Label5.Text + " " + this.Label2.Text + Variable.SYS_MSJ[203, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation); //" no puede estar vacia");
                        this.tbxProdfin.Focus();
                        return;
                    }
                }
                else
                {
                    if (this.tbxProdfin.Text != "")
                    {
                        MessageBox.Show(this.Label5.Text + " " + this.Label1.Text + Variable.SYS_MSJ[203, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation); //" no puede estar vacia");
                        this.tbxProdini.Focus();
                        return;
                    }
                }
                if (this.tbxProdini.Text != "" && this.tbxProdfin.Text != "")
                {
                    if (Convert.ToInt32(this.tbxProdini.Text) > Convert.ToInt32(this.tbxProdfin.Text))
                    {
                        MessageBox.Show(this.Label5.Text + " " + this.Label2.Text + Variable.SYS_MSJ[205, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation); //" no puede ser menor que la inicial");
                        tbxVendfin.Focus();
                        return;
                    }
                }
            }

            #endregion
            ReportDataSource myconnect = new ReportDataSource();
            myconnect.Name = "DataSet1";
            //Arbol
            switch (treListadoR.SelectedNode.Index)
            {

                case 0://
                    {
                        if (Form1.btnEdicion == ESTADO.botonesEdicionEnum.PKGENERAL)
                        {
                            paramFields = new ReportParameter[8];
                            nombre_reporte = Imprimir_Reportes_Generales(0, ref filtro, ref hay, ref condi); //imprime el listado de basculas                        }                                                
                            myconnect.Value = basededataset1.Bascula.Select(condi);
                            tipo_orientacion = false;
                        }

                        if (Form1.btnEdicion == ESTADO.botonesEdicionEnum.PKSTADITICO)
                        {
                            #region OPCION CON DESGLOSE
                            if (DesgloseImpuestos.Checked)
                            {                               
                                //Crear nuevo reporte
                                paramFields = new ReportParameter[12];//aumenta el numero de parametros
                                nombre_reporte = Imprimir_Reportes_Estadisticos_Con_Desglose(0, ref filtro, ref hay, ref condi); //Se modifica
                                DateTime Fh_Inicio = Convert.ToDateTime(string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value));
                                DateTime Fh_fin = Convert.ToDateTime(string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value));
                                DataTable tbl = basededataset1.Ventas;
                                GetDataFilteredStadists(ref tbl, Fh_Inicio, Fh_fin);
                                myconnect.Value = tbl;
                                tipo_orientacion = false;// seuq eda igual
                            }
                            #endregion
                            #region OPCION SIN DESGLOSE
                            else if(!DesgloseImpuestos.Checked)
                            {
                                paramFields = new ReportParameter[9];
                                nombre_reporte = Imprimir_Reportes_Estadisticos(0, ref filtro, ref hay, ref condi); //imprime analisis de venta X grupo
                                myconnect.Value = basededataset1.Ventas.Select(condi);
                                tipo_orientacion = false;
                            }
                            #endregion
                        }
                        if (Form1.btnEdicion == ESTADO.botonesEdicionEnum.PKVENTAS)
                        {
                            #region OPCION CON DESGLOSE
                            if (DesgloseImpuestos.Checked)
                            {
                                paramFields = new ReportParameter[18];
                                nombre_reporte = Imprimir_Reportes_Ventas_Con_Desglose(0, ref filtro, ref hay, ref condi);  //imprimir ventas X fecha
                                DateTime Fh_Inicio = Convert.ToDateTime(string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value));
                                DateTime Fh_fin = Convert.ToDateTime(string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value));
                                DataTable tbl = basededataset1.Ventas_Detalle;
                                GetDataFiltered(ref tbl, Fh_Inicio, Fh_fin);
                                myconnect.Value = tbl;
                                tipo_orientacion = true;
                            }
                            #endregion
                            #region OPCION SIN DESGLOSE
                            else if (!DesgloseImpuestos.Checked)
                            {
                                paramFields = new ReportParameter[15];
                                nombre_reporte = Imprimir_Reportes_Ventas(0, ref filtro, ref hay, ref condi);  //imprimir ventas X fecha
                                myconnect.Value = FilterColumns(basededataset1.Ventas_Detalle.Select(condi));
                                tipo_orientacion = true;
                            }
                            #endregion
                        }
                    }
                    break;

                case 1://
                    {
                        if (Form1.btnEdicion == ESTADO.botonesEdicionEnum.PKGENERAL)
                        {
                            paramFields = new ReportParameter[9];
                            nombre_reporte = Imprimir_Reportes_Generales(1, ref filtro, ref hay, ref condi); //Imprime el listado de productos
                            myconnect.Value = basededataset1.Productos.Select(condi);
                            tipo_orientacion = false;
                        }

                        if (Form1.btnEdicion == ESTADO.botonesEdicionEnum.PKSTADITICO)
                        {
                            #region CON DESGLOSE
                            if (DesgloseImpuestos.Checked)
                            {
                                paramFields = new ReportParameter[12];
                                nombre_reporte = Imprimir_Reportes_Estadisticos_Con_Desglose_1(1, ref filtro, ref hay, ref condi); //imprime analisis de venta X bascula
                                DateTime Fh_Inicio = Convert.ToDateTime(string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value));
                                DateTime Fh_fin = Convert.ToDateTime(string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value));
                                DataTable tbl = basededataset1.Ventas;
                                GetDataFilteredStadists(ref tbl, Fh_Inicio, Fh_fin);
                                myconnect.Value = tbl;

                                tipo_orientacion = false;
                            }
                            #endregion
                            #region SIN DESGLOSE
                            else if(!DesgloseImpuestos.Checked)
                            {
                                paramFields = new ReportParameter[9];
                                nombre_reporte = Imprimir_Reportes_Estadisticos(1, ref filtro, ref hay, ref condi); //imprime analisis de venta X bascula
                                myconnect.Value = basededataset1.Ventas.Select(condi);
                                tipo_orientacion = false;
                            }
                            #endregion
                        }

                        if (Form1.btnEdicion == ESTADO.botonesEdicionEnum.PKVENTAS)
                        {
                            #region CON DESGLOSE
                            if (DesgloseImpuestos.Checked)
                            {
                                paramFields = new ReportParameter[18];
                                nombre_reporte = Imprimir_Reportes_Ventas_Con_Desglose(1, ref filtro, ref hay, ref condi); //imprimir ventas X grupo
                             


                                DateTime Fh_Inicio = Convert.ToDateTime(string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value));
                                DateTime Fh_fin = Convert.ToDateTime(string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value));
                                DataTable tbl = basededataset1.Ventas_Detalle;
                                GetDataFiltered(ref tbl, Fh_Inicio, Fh_fin);
                                myconnect.Value = tbl;
                                tipo_orientacion = true;
                            }
                            #endregion
                            #region SIN DESGLOSE
                            else if(!DesgloseImpuestos.Checked)
                            {
                                paramFields = new ReportParameter[15];
                                nombre_reporte = Imprimir_Reportes_Ventas(1, ref filtro, ref hay, ref condi); //imprimir ventas X grupo
                                myconnect.Value = FilterColumns(basededataset1.Ventas_Detalle.Select(condi));
                                tipo_orientacion = true;
                            }
                            #endregion
                        }
                    } 
                    break;
                case 2:
                    {
                        if (Form1.btnEdicion == ESTADO.botonesEdicionEnum.PKGENERAL)
                        {
                            paramFields = new ReportParameter[5];
                            nombre_reporte = Imprimir_Reportes_Generales(2, ref filtro, ref hay, ref condi); //imprime el listado de Info adicional
                            myconnect.Value = basededataset1.Ingredientes.Select(condi);
                            tipo_orientacion = false;
                        }

                        if (Form1.btnEdicion == ESTADO.botonesEdicionEnum.PKSTADITICO)
                        {
                            #region CON DESGLOSE
                            if (DesgloseImpuestos.Checked)
                            {
                                paramFields = new ReportParameter[14];
                                nombre_reporte = Imprimir_Reportes_Estadisticos_Con_Desglose_2(2, ref filtro, ref hay, ref condi); //imprime analisis de venta X producto
                                DateTime Fh_Inicio = Convert.ToDateTime(string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value));
                                DateTime Fh_fin = Convert.ToDateTime(string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value));
                                DataTable tbl = basededataset1.Ventas_Detalle;
                                GetDataFilteredProductos(ref tbl, Fh_Inicio, Fh_fin);
                                myconnect.Value = tbl;
                                tipo_orientacion = true;
                            }
                            #endregion
                            #region SIN DESGLOSE
                            else if (!DesgloseImpuestos.Checked)
                            {
                                paramFields = new ReportParameter[11];
                                nombre_reporte = Imprimir_Reportes_Estadisticos(2, ref filtro, ref hay, ref condi); //imprime analisis de venta X producto
                                myconnect.Value = basededataset1.Ventas_Detalle.Select(condi);
                                tipo_orientacion = true;
                            }
                            #endregion
                        }
                        if (Form1.btnEdicion == ESTADO.botonesEdicionEnum.PKVENTAS)
                        {
                            #region CON DESGLOSE
                            if (DesgloseImpuestos.Checked)
                            {
                                paramFields = new ReportParameter[18];
                                nombre_reporte = Imprimir_Reportes_Ventas_Con_Desglose(2, ref filtro, ref hay, ref condi); //imprimir ventas X bascula
                                DateTime Fh_Inicio = Convert.ToDateTime(string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value));
                                DateTime Fh_fin = Convert.ToDateTime(string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value));
                                DataTable tbl = basededataset1.Ventas_Detalle;
                                GetDataFiltered(ref tbl, Fh_Inicio, Fh_fin);
                                myconnect.Value = tbl;
                                tipo_orientacion = true;
                            }
                            #endregion
                            #region SIN DESGLOSE
                            else if(!DesgloseImpuestos.Checked) 
                            {
                                paramFields = new ReportParameter[15];
                                nombre_reporte = Imprimir_Reportes_Ventas(2, ref filtro, ref hay, ref condi); //imprimir ventas X bascula
                                myconnect.Value = FilterColumns(basededataset1.Ventas_Detalle.Select(condi));
                                tipo_orientacion = true;
                            }
                            #endregion
                        }
                    } 
                    break;
                case 3:
                    {
                        if (Form1.btnEdicion == ESTADO.botonesEdicionEnum.PKGENERAL)
                        {
                            paramFields = new ReportParameter[5];
                            nombre_reporte = Imprimir_Reportes_Generales(3, ref filtro, ref hay, ref condi);  //imprime el listado de mensajes
                            myconnect.Value = basededataset1.Publicidad.Select(condi);
                            tipo_orientacion = false;
                        }
                        if (Form1.btnEdicion == ESTADO.botonesEdicionEnum.PKSTADITICO)
                        {
                            #region CON DESGLOSE
                            if (DesgloseImpuestos.Checked)
                            {
                                paramFields = new ReportParameter[12];
                                nombre_reporte = Imprimir_Reportes_Estadisticos_Con_Desglose_3(3, ref filtro, ref hay, ref condi); //imprime analisis de venta X vendedor
                                DateTime Fh_Inicio = Convert.ToDateTime(string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value));
                                DateTime Fh_fin = Convert.ToDateTime(string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value));
                                DataTable tbl = basededataset1.Ventas;
                                GetDataFilteredStadistsAgent(ref tbl, Fh_Inicio, Fh_fin);
                                myconnect.Value = tbl;
                                tipo_orientacion = false;
                            }
                            #endregion
                            #region SIN DESGLOSE
                            else if(!DesgloseImpuestos.Checked)
                            {
                                paramFields = new ReportParameter[9];
                                nombre_reporte = Imprimir_Reportes_Estadisticos(3, ref filtro, ref hay, ref condi); //imprime analisis de venta X vendedor
                                myconnect.Value = basededataset1.Ventas.Select(condi);
                                tipo_orientacion = false;
                            }
                            #endregion

                        }
                        if (Form1.btnEdicion == ESTADO.botonesEdicionEnum.PKVENTAS)
                        {
                            #region CON DESGLOSE

                            if (DesgloseImpuestos.Checked)
                            {
                                paramFields = new ReportParameter[18];
                                nombre_reporte = Imprimir_Reportes_Ventas_Con_Desglose(3, ref filtro, ref hay, ref condi); //imprimir ventas X corte de venta
                                DateTime Fh_Inicio=Convert.ToDateTime(string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value));
                                DateTime Fh_fin=Convert.ToDateTime(string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value));
                                DataTable tbl = basededataset1.Ventas_Detalle;
                                GetDataFiltered(ref tbl, Fh_Inicio, Fh_fin);
                                myconnect.Value = tbl;
                                tipo_orientacion = true;
                            }
                            #endregion
                            #region SIN DESGLOSE
                            else if(!DesgloseImpuestos.Checked)
                            {
                                paramFields = new ReportParameter[15];
                                nombre_reporte = Imprimir_Reportes_Ventas(3, ref filtro, ref hay, ref condi); //imprimir ventas X corte de venta
                                myconnect.Value = FilterColumns(basededataset1.Ventas_Detalle.Select(condi));
                                tipo_orientacion = true;
                            }
                            #endregion
                        }
                    } 
                    break;
                case 4:
                    {

                        if (Form1.btnEdicion == ESTADO.botonesEdicionEnum.PKGENERAL)
                        {
                            paramFields = new ReportParameter[7];
                            nombre_reporte = Imprimir_Reportes_Generales(4, ref filtro, ref hay, ref condi);  //imprime el listado de ofertas                       
                            myconnect.Value =  basededataset1.Oferta.Select(condi);
                            tipo_orientacion = false;
                        }
                        if (Form1.btnEdicion == ESTADO.botonesEdicionEnum.PKVENTAS)
                        {
                            #region CON DESGLOSE
                            if (DesgloseImpuestos.Checked)
                            {
                                paramFields = new ReportParameter[18];
                                nombre_reporte = Imprimir_Reportes_Ventas_Con_Desglose(4, ref filtro, ref hay, ref condi); //imprimir ventas X vendedor
                                DateTime Fh_Inicio = Convert.ToDateTime(string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value));
                                DateTime Fh_fin = Convert.ToDateTime(string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value));
                                DataTable tbl = basededataset1.Ventas_Detalle;
                                GetDataFiltered(ref tbl, Fh_Inicio, Fh_fin);
                                myconnect.Value = tbl;
                                tipo_orientacion = true;
                            }
                            #endregion
                            #region SIN DESGLOSE
                            else if(!DesgloseImpuestos.Checked)
                            {
                                paramFields = new ReportParameter[15];
                                nombre_reporte = Imprimir_Reportes_Ventas(4, ref filtro, ref hay, ref condi); //imprimir ventas X vendedor
                                myconnect.Value = FilterColumns(basededataset1.Ventas_Detalle.Select(condi));
                                tipo_orientacion = true;
                            }
                            #endregion
                        }
                    } break;


                case 5://
                    {

                        if (Form1.btnEdicion == ESTADO.botonesEdicionEnum.PKGENERAL)
                        {
                            paramFields = new ReportParameter[7];
                            nombre_reporte = Imprimir_Reportes_Generales(5, ref filtro, ref hay, ref condi); //imprime el listado de vendedores                          
                            myconnect.Value = basededataset1.Vendedor.Select(condi);
                            tipo_orientacion = false;
                        }
                        if (Form1.btnEdicion == ESTADO.botonesEdicionEnum.PKVENTAS)
                        {
                            #region CON DESGLOSE
                            if (DesgloseImpuestos.Checked)
                            {
                                paramFields = new ReportParameter[18];
                                nombre_reporte = Imprimir_Reportes_Ventas_Con_Desglose(5, ref filtro, ref hay, ref condi); //imprimir ventas X producto
                                myconnect.Value = basededataset1.Ventas_Detalle.Select(condi);
                                DateTime Fh_Inicio = Convert.ToDateTime(string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value));
                                DateTime Fh_fin = Convert.ToDateTime(string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value));
                                DataTable tbl = basededataset1.Ventas_Detalle;
                                GetDataFiltered(ref tbl, Fh_Inicio, Fh_fin);
                                myconnect.Value = tbl;
                                tipo_orientacion = true;
                            }
                            #endregion
                            #region SIN DESGLOSE
                            else if (!DesgloseImpuestos.Checked) 
                            {
                                paramFields = new ReportParameter[15];
                                nombre_reporte = Imprimir_Reportes_Ventas(5, ref filtro, ref hay, ref condi); //imprimir ventas X producto
                                myconnect.Value = FilterColumns(basededataset1.Ventas_Detalle.Select(condi));
                                tipo_orientacion = true;
                                
                            }
                            #endregion
                        }
                    } 
               break;
            }



            //IMPRIMIR REPORTES
            if(hay)
            {                 
                visibleGrupoBox1(false);
                visibleGrupoBox2(false);

                ReportViewer reportViewer2 = new ReportViewer();   
                reportViewer2.Visible = true;
                reportViewer2.Dock = DockStyle.Fill;
                reportViewer2.ProcessingMode = ProcessingMode.Local;
                reportViewer2.PageCountMode = PageCountMode.Actual;             
                reportViewer2.PrinterSettings.DefaultPageSettings.Landscape = tipo_orientacion;
                reportViewer2.PrinterSettings.DefaultPageSettings.Margins.Top = 200;
                reportViewer2.PrinterSettings.DefaultPageSettings.Margins.Bottom = 200;
                reportViewer2.PrinterSettings.DefaultPageSettings.Margins.Left = 200;
                reportViewer2.PrinterSettings.DefaultPageSettings.Margins.Right = 200;
                try
                {
                    reportViewer2.LocalReport.ReportPath = nombre_reporte;
                    reportViewer2.LocalReport.DataSources.Clear();
                    reportViewer2.LocalReport.DataSources.Add(myconnect);
                    reportViewer2.LocalReport.SetParameters(paramFields);
                    reportViewer2.LocalReport.Refresh();
                    reportViewer2.SetDisplayMode(DisplayMode.PrintLayout);
                    pnldetalle.Controls.Add(reportViewer2);
                }
                catch (Exception ex)
                {
                    string asd = ex.Message;
                }

            }
            else
            {
                MessageBox.Show(this, Variable.SYS_MSJ[366, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  //"No se encontro la información");
            }
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }
        void multiplica(DataTable dato)
        {

            for (int i=0; i<dato.Rows.Count;i++)
            {                
               dato.Rows[i]["Precio"] = ((float.Parse(dato.Rows[i]["Precio"].ToString().Trim()) < 0)) ? (float.Parse(dato.Rows[i]["Precio"].ToString().Trim()) * -1).ToString() : dato.Rows[i]["Precio"].ToString().Trim();
               dato.Rows[i]["Devolucion"] = (float.Parse(dato.Rows[i]["Devolucion"].ToString()) < 0 && float.Parse(dato.Rows[i]["Impuesto"].ToString().Trim()) < 0 && int.Parse(dato.Rows[i]["TipoPlu"].ToString().Trim()) == 0) ? (float.Parse(dato.Rows[i]["Devolucion"].ToString().Trim()) - float.Parse(dato.Rows[i]["Impuesto"].ToString().Trim())) : dato.Rows[i]["Devolucion"]; 
            }
        }
        void GetDataFiltered(ref DataTable tbl, DateTime fechaInicio, DateTime FechaFin)
        {

            DateTime fechaobjetivo;
            DataTable tblAux = tbl.Clone();
            tblAux.Rows.Clear();

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                DataRow FilaEnCuestion = tbl.Rows[i];

                string fechaOriginal = FilaEnCuestion["Fecha"].ToString();
                string date = string.Format(Variable.FOR_FECHAS[Variable.ffecha], FilaEnCuestion["Fecha"].ToString());


                string[] fecha = date.Split('/');

                fechaobjetivo = new DateTime(int.Parse(fecha[2]), int.Parse(fecha[1]), int.Parse(fecha[0]));

                if (fechaobjetivo >= fechaInicio && fechaobjetivo <= FechaFin)
                {
                    DataRow row = tblAux.NewRow();
                    row["id_Venta"] = FilaEnCuestion["id_Venta"];
                    row["id_detalle"] = FilaEnCuestion["id_detalle"];
                    row["Linea"] = FilaEnCuestion["Linea"];
                    row["NoPlu"] = FilaEnCuestion["NoPlu"];
                    row["Nombre"] = FilaEnCuestion["Nombre"];
                    row["TipoPlu"] = FilaEnCuestion["TipoPlu"];
                    row["Peso"] = FilaEnCuestion["Peso"];
                    row["Precio"] = FilaEnCuestion["Precio"];
                    row["Total"] = FilaEnCuestion["Total"];
                    row["Impuesto"] = FilaEnCuestion["Impuesto"];
                    row["id_bascula"] = FilaEnCuestion["id_bascula"];
                    row["id_corte"] = FilaEnCuestion["id_corte"];
                    row["id_grupo"] = FilaEnCuestion["id_grupo"];
                    row["id_vendedor"] = FilaEnCuestion["id_vendedor"];
                    row["TipoItem"] = FilaEnCuestion["TipoItem"];
                    row["subtotal"] = FilaEnCuestion["subtotal"];
                    row["descuento"] = FilaEnCuestion["descuento"];
                    row["Nserie"] = FilaEnCuestion["Nserie"];
                    row["Oferta"] = FilaEnCuestion["Oferta"];
                    row["Fecha"] = FilaEnCuestion["Fecha"];
                    row["Depto"] = FilaEnCuestion["Depto"];
                    row["Devolucion"] = FilaEnCuestion["Devolucion"];
                    row["Codigo"] = FilaEnCuestion["Codigo"];
                    row["registro"] = FilaEnCuestion["registro"];
                    tblAux.Rows.Add(row);
                }
            }
            tbl = null;
            tbl = tblAux;
            multiplica(tbl);
        }
        void GetDataFilteredStadists(ref DataTable tbl, DateTime fechaInicio, DateTime FechaFin)
        {

            DateTime fechaobjetivo;
            DataTable tblAux = tbl.Clone();
            tblAux.Rows.Clear();

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                DataRow FilaEnCuestion = tbl.Rows[i];

                string date = FilaEnCuestion["Fecha"].ToString();

                string[] fecha = date.Split('/');

                fechaobjetivo = new DateTime(int.Parse(fecha[2]), int.Parse(fecha[1]), int.Parse(fecha[0]));


                if (fechaobjetivo >= fechaInicio && fechaobjetivo <= FechaFin)
                {
                    DataRow row = tblAux.NewRow();
                    row["id_Venta"] = FilaEnCuestion["id_Venta"];
                    row["id_bascula"] = FilaEnCuestion["id_bascula"];
                    row["id_corte"] = FilaEnCuestion["id_corte"];
                    row["id_grupo"] = FilaEnCuestion["id_grupo"];
                    row["id_vendedor"] = FilaEnCuestion["id_vendedor"];
                    row["Cantidad"] = FilaEnCuestion["Cantidad"];
                    row["GranTotal"] = FilaEnCuestion["GranTotal"];
                    row["Descuento"] = FilaEnCuestion["Descuento"];
                    row["Efectivo"] = FilaEnCuestion["Efectivo"];
                    row["Cambio"] = FilaEnCuestion["Cambio"];
                    row["Fecha"] = FilaEnCuestion["Fecha"];
                    row["Hora"] = FilaEnCuestion["Hora"];
                    row["Nserie"] = FilaEnCuestion["Nserie"];
                    row["Subtotal"] = FilaEnCuestion["Subtotal"];
                    row["Impuesto"] = FilaEnCuestion["Impuesto"];
                    row["Oferta"] = FilaEnCuestion["Oferta"];
                    row["Vendedor"] = FilaEnCuestion["Vendedor"];
                    row["Depto"] = FilaEnCuestion["Depto"];
                    row["registro"] = FilaEnCuestion["registro"];
                    row["Hora"] = FilaEnCuestion["Hora"];
                    row["Depto"] = FilaEnCuestion["Depto"];
                    row["registro"] = FilaEnCuestion["registro"];
                    tblAux.Rows.Add(row);

                }
            
            }
            tbl = null;
            tbl = tblAux;
        }
        void GetDataFilteredStadistsAgent(ref DataTable tbl, DateTime fechaInicio, DateTime FechaFin)
        {

            DateTime fechaobjetivo;
            DataTable tblAux = tbl.Clone();
            tblAux.Rows.Clear();

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                DataRow FilaEnCuestion = tbl.Rows[i];

                string date = FilaEnCuestion["Fecha"].ToString();

                string[] fecha = date.Split('/');

                fechaobjetivo = new DateTime(int.Parse(fecha[2]), int.Parse(fecha[1]), int.Parse(fecha[0]));


                if (fechaobjetivo >= fechaInicio && fechaobjetivo <= FechaFin)
                {

                    DataRow row = tblAux.NewRow();
                    row["id_Venta"] = FilaEnCuestion["id_Venta"];
                    row["id_bascula"] = FilaEnCuestion["id_bascula"];
                    row["id_corte"] = FilaEnCuestion["id_corte"];
                    row["id_grupo"] = FilaEnCuestion["id_grupo"];
                    row["id_vendedor"] = FilaEnCuestion["id_vendedor"];
                    row["Folio"] = FilaEnCuestion["Folio"];
                    row["Cantidad"] = FilaEnCuestion["Cantidad"];
                    row["GranTotal"] = FilaEnCuestion["GranTotal"];
                    row["Descuento"] = FilaEnCuestion["Descuento"];
                    row["Efectivo"] = FilaEnCuestion["Efectivo"];
                    row["Cambio"] = FilaEnCuestion["Cambio"];
                    row["Fecha"] = FilaEnCuestion["Fecha"];
                    row["Hora"] = FilaEnCuestion["Hora"];
                    row["Nserie"] = FilaEnCuestion["Nserie"];
                    row["Subtotal"] = FilaEnCuestion["Subtotal"];
                    row["Impuesto"] = FilaEnCuestion["Impuesto"];
                    row["Oferta"] = FilaEnCuestion["Oferta"];
                    row["Vendedor"] = FilaEnCuestion["Vendedor"];
                    row["Depto"] = FilaEnCuestion["Depto"];
                    row["registro"] = FilaEnCuestion["registro"];
                    tblAux.Rows.Add(row);

                }

            }
            tbl = null;
            tbl = tblAux;
            //multiplica(tbl);
        }
        void GetDataFilteredProductos(ref DataTable tbl, DateTime fechaInicio, DateTime FechaFin)
        {

            DateTime fechaobjetivo;
            DataTable tblAux = tbl.Clone();
            tblAux.Rows.Clear();

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                DataRow FilaEnCuestion = tbl.Rows[i];

                string date = FilaEnCuestion["Fecha"].ToString();

                string[] fecha = date.Split('/');

                fechaobjetivo = new DateTime(int.Parse(fecha[2]), int.Parse(fecha[1]), int.Parse(fecha[0]));

                if (fechaobjetivo >= fechaInicio && fechaobjetivo <= FechaFin)
                {

                    DataRow row = tblAux.NewRow();
                    row["id_Venta"] = FilaEnCuestion["id_Venta"];
                    row["id_detalle"] = FilaEnCuestion["id_detalle"];
                    row["Linea"] = FilaEnCuestion["Linea"];
                    row["NoPLU"] = FilaEnCuestion["NoPLU"];
                    row["TipoPLU"] = FilaEnCuestion["TipoPLU"];
                    row["Peso"] = FilaEnCuestion["Peso"];
                    row["Precio"] = FilaEnCuestion["Precio"];
                    row["Nombre"] = FilaEnCuestion["Nombre"];
                    row["Total"] = FilaEnCuestion["Total"];
                    row["Impuesto"] = FilaEnCuestion["Impuesto"];
                    row["id_bascula"] = FilaEnCuestion["id_bascula"];
                    row["id_corte"] = FilaEnCuestion["id_corte"];
                    row["id_grupo"] = FilaEnCuestion["id_grupo"];
                    row["id_vendedor"] = FilaEnCuestion["id_vendedor"];
                    row["TipoItem"] = FilaEnCuestion["TipoItem"];
                    row["subtotal"] = FilaEnCuestion["subtotal"];
                    row["descuento"] = FilaEnCuestion["descuento"];
                    row["Nserie"] = FilaEnCuestion["Nserie"];
                    row["Oferta"] = FilaEnCuestion["Oferta"];
                    row["Fecha"] = FilaEnCuestion["Fecha"];
                    row["Hora"] = FilaEnCuestion["Hora"];
                    row["Depto"] = FilaEnCuestion["Depto"];
                    row["Devolucion"] = FilaEnCuestion["Devolucion"];
                    row["Codigo"] = FilaEnCuestion["Codigo"];
                    row["registro"] = FilaEnCuestion["registro"];
                    tblAux.Rows.Add(row);

                }

            }

            tbl = null;
            tbl = tblAux;
            multiplica(tbl);
        }
        DataRow[] FilterColumns( DataRow[] dtr) 
        {
            for (int i = 0; i < dtr.Length; i++)
            {
                dtr[i]["Precio"] = ((float.Parse(dtr[i]["Precio"].ToString().Trim()) < 0)) ? (float.Parse(dtr[i]["Precio"].ToString().Trim()) * -1).ToString() : dtr[i]["Precio"].ToString().Trim();
            }
            return dtr;
        }
        private string Imprimir_Reportes_Ventas(int opcion, ref string filtro, ref bool hay, ref string condi)
        {
            string nreporte = "";
            string filtro1 = "", filtro2 = "", filtro3 = "", filtro4 = "";
            string fecha = string.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now.Date);
            string hora1 = string.Format(Variable.F_Hora, DateTime.Now);

            if (tbxFeinicio.Text.Length > 0 && tbxFefinal.Text.Length > 0)
            {

                filtro1 = "(DateValue({Ventas_Detalle.Fecha}) in DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "') to DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "'))";
                condi = "( Convert(Fecha,'System.DateTime') >= '" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "' and  Convert(Fecha,'System.DateTime') <= '" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "')";
                // condi = "(DateValue(Fecha) >= DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "') and DateValue(Fecha) <= DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "'))";               
            }
            if (cBxNserie.SelectedIndex > 0)
            {
                filtro2 = " and ({Ventas.id_bascula} = " + Convert.ToInt32(cBxNserie.SelectedValue) + ")";
                condi = condi + " and (id_bascula = " + Convert.ToInt32(cBxNserie.SelectedValue) + ")";
            }
            if (tbxProdini.Text.Length > 0 && tbxProdfin.Text.Length > 0)
            {
                filtro3 = " and ({Ventas_Detalle.Codigo} in " + Convert.ToInt32(tbxProdini.Text) + " to " + Convert.ToInt32(tbxProdfin.Text) + ")";
                condi = condi + " and (Codigo >= " + Convert.ToInt32(tbxProdini.Text) + " and Codigo <= " + Convert.ToInt32(tbxProdfin.Text) + ")";
            }
            if (tbxVendini.Text.Length > 0 && tbxVendfin.Text.Length > 0)
            {
                filtro4 = " and ({Ventas_Detalle.id_vendedor} in " + Convert.ToInt32(tbxVendini.Text) + " to " + Convert.ToInt32(tbxVendfin.Text) + ")";
                condi = condi + " and (id_vendedor >= " + Convert.ToInt32(tbxVendini.Text) + " and id_vendedor <= " + Convert.ToInt32(tbxVendfin.Text) + ")";
            }

            filtro = filtro1 + filtro2 + filtro3 + filtro4;

            Parametros pa = new Parametros();

            switch (opcion)
            {
                case 0:  //imprime ventas por fecha
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY Fecha WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY Fecha";

                        ventadetalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventadetalleTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventadetalleTableAdapter.Fill(basededataset1.Ventas_Detalle);

                        if (basededataset1.Ventas_Detalle.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Reportes_Venta(0, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "RVxFecha.rdlc";  // new RVxFecha().ResourceName;
                        }

                    } break;
                case 1: //Imprime ventas por grupo
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY Depto WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY Depto";

                        ventadetalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventadetalleTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventadetalleTableAdapter.Fill(basededataset1.Ventas_Detalle);

                        if (basededataset1.Ventas_Detalle.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Reportes_Venta(1, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "RVxDepto.rdlc";  // new RVxGpo().ResourceName;
                        }

                    } break;
                case 2:   //imprime ventas por basculas
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY id_bascula WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY id_bascula";

                        ventadetalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventadetalleTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventadetalleTableAdapter.Fill(basededataset1.Ventas_Detalle);

                        if (basededataset1.Ventas_Detalle.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Reportes_Venta(2, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "RVxBasc.rdlc";  //new RVxBasc().ResourceName;
                        }
                    } break;
                case 3:   //imprime ventas por cortes
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY id_corte WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY id_corte";

                        ventadetalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventadetalleTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventadetalleTableAdapter.Fill(basededataset1.Ventas_Detalle);

                        if (basededataset1.Ventas_Detalle.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Reportes_Venta(3, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "RVxCorte.rdlc";  // new RVxCorte().ResourceName;
                        }
                    } break;
                case 4:   //imprime ventas por vendedor
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY id_vendedor WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY d_vendedor";

                        ventadetalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventadetalleTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventadetalleTableAdapter.Fill(basededataset1.Ventas_Detalle);

                        if (basededataset1.Ventas_Detalle.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Reportes_Venta(4, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "RVxVendedor.rdlc";  // new RVxVendedor().ResourceName;
                        }
                    } break;
                case 5:  //imprime ventas por producto
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY Codigo WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY Codigo";

                        ventadetalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventadetalleTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventadetalleTableAdapter.Fill(basededataset1.Ventas_Detalle);

                        if (basededataset1.Ventas_Detalle.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Reportes_Venta(5, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "RVxProd.rdlc";  // new RVxProd().ResourceName;
                        }
                    } break;
            }
            return nreporte;
        }
        private string Imprimir_Reportes_Ventas_Con_Desglose(int opcion, ref string filtro, ref bool hay, ref string condi)
        {
            string nreporte = "";
            string filtro1 = "", filtro2 = "", filtro3 = "", filtro4 = "";
            string fecha = string.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now.Date);
            string hora1 = string.Format(Variable.F_Hora, DateTime.Now);

            
            if (tbxFeinicio.Text.Length > 0 && tbxFefinal.Text.Length > 0)
            {

                filtro1 = "(DateValue({Ventas_Detalle.Fecha}) in DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "') to DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "'))";
                
                condi = "(Fecha >= '" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "' and  Fecha <= '" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "')";
                

                
                // condi = "(DateValue(Fecha) >= DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "') and DateValue(Fecha) <= DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "'))";               
            }
            if (cBxNserie.SelectedIndex > 0)
            {
                filtro2 = " and ({Ventas.id_bascula} = " + Convert.ToInt32(cBxNserie.SelectedValue) + ")";
                condi = condi + " and (id_bascula = " + Convert.ToInt32(cBxNserie.SelectedValue) + ")";
            }
            if (tbxProdini.Text.Length > 0 && tbxProdfin.Text.Length > 0)
            {
                filtro3 = " and ({Ventas_Detalle.Codigo} in " + Convert.ToInt32(tbxProdini.Text) + " to " + Convert.ToInt32(tbxProdfin.Text) + ")";
                condi = condi + " and (Codigo >= " + Convert.ToInt32(tbxProdini.Text) + " and Codigo <= " + Convert.ToInt32(tbxProdfin.Text) + ")";
            }
            if (tbxVendini.Text.Length > 0 && tbxVendfin.Text.Length > 0)
            {
                filtro4 = " and ({Ventas_Detalle.id_vendedor} in " + Convert.ToInt32(tbxVendini.Text) + " to " + Convert.ToInt32(tbxVendfin.Text) + ")";
                condi = condi + " and (id_vendedor >= " + Convert.ToInt32(tbxVendini.Text) + " and id_vendedor <= " + Convert.ToInt32(tbxVendfin.Text) + ")";
            }

            filtro = filtro1 + filtro2 + filtro3 + filtro4;

            Parametros pa = new Parametros();

            switch (opcion)
            {
                case 0:  //imprime ventas por fecha
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY Fecha WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY Fecha";

                        ventadetalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventadetalleTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventadetalleTableAdapter.Fill(basededataset1.Ventas_Detalle);

                        if (basededataset1.Ventas_Detalle.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Reportes_Venta_Desglose(0, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "RVxFechaImp.rdlc";  // new RVxFecha().ResourceName;
                        }

                    } break;
                case 1: //Imprime ventas por grupo
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY Depto WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY Depto";

                        ventadetalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventadetalleTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventadetalleTableAdapter.Fill(basededataset1.Ventas_Detalle);

                        if (basededataset1.Ventas_Detalle.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Reportes_Venta_Desglose(1, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "RVxDeptoImp.rdlc";  // new RVxGpo().ResourceName;
                        }

                    } break;
                case 2:   //imprime ventas por basculas
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY id_bascula WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY id_bascula";

                        ventadetalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventadetalleTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventadetalleTableAdapter.Fill(basededataset1.Ventas_Detalle);

                        if (basededataset1.Ventas_Detalle.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Reportes_Venta_Desglose(2, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "RVxBascImp.rdlc";  //new RVxBasc().ResourceName;
                        }
                    } break;
                case 3:   //imprime ventas por cortes
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY id_corte WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY id_corte";

                        ventadetalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventadetalleTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventadetalleTableAdapter.Fill(basededataset1.Ventas_Detalle);

                        if (basededataset1.Ventas_Detalle.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Reportes_Venta_Desglose(3, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "RVxCorteImp.rdlc";  // new RVxCorte().ResourceName;
                        }
                    } break;
                case 4:   //imprime ventas por vendedor
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY id_vendedor WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY d_vendedor";

                        ventadetalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventadetalleTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventadetalleTableAdapter.Fill(basededataset1.Ventas_Detalle);

                        if (basededataset1.Ventas_Detalle.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Reportes_Venta_Desglose(4, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "RVxVendedorImp.rdlc";  // new RVxVendedor().ResourceName;
                        }
                    } break;


                case 5:  //imprime ventas por producto
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY Codigo WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY Codigo";

                        ventadetalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventadetalleTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventadetalleTableAdapter.Fill(basededataset1.Ventas_Detalle);

                        if (basededataset1.Ventas_Detalle.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Reportes_Venta_Desglose(5, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "RVxProdImp.rdlc";  // new RVxProd().ResourceName;
                        }
                    } break;
            }
            return nreporte;
        }
        private string Imprimir_Reportes_Generales(int opcion, ref string filtro, ref bool hay, ref string condi)
        {
            string nreporte = "";

            string fecha = string.Format(Variable.F_Fecha, DateTime.Now);
            string hora1 = string.Format(Variable.F_Hora, DateTime.Now);

            Parametros pa = new Parametros();

            switch (opcion)
            {
                case 0:  //imprime el listado de basculas
                    {
                        if (tbxDatoini.Text.Length > 0 && tbxDatofin.Text.Length > 0)
                        {
                            filtro = "({Bascula.id_bascula} in " + Convert.ToInt32(tbxDatoini.Text) + " to " + Convert.ToInt32(tbxDatofin.Text) + ")";
                            condi = "(id_bascula >= " + Convert.ToInt32(tbxDatoini.Text) + " and id_bascula <= " + Convert.ToInt32(tbxDatofin.Text) + ")";
                        }
                        else { filtro = ""; }

                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Bascula  Order by id_bascula WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Bascula Order by id_bascula";

                        basculasTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        basculasTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        basculasTableAdapter.Fill(basededataset1.Bascula);

                        if (basededataset1.Bascula.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Listados_Maestros(0, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "Rbasc.rdlc"; // new RBASC().ResourceName;
                        }

                    } break;
                case 1: //Imprime el listado de productos
                    {
                        if (tbxDatoini.Text.Length > 0 && tbxDatofin.Text.Length > 0)
                        {
                            filtro = "{Productos.Codigo} in " + Convert.ToInt32(tbxDatoini.Text) + " to " + Convert.ToInt32(tbxDatofin.Text);
                            condi = "(Codigo >= " + Convert.ToInt32(tbxDatoini.Text) + " and Codigo <= " + Convert.ToInt32(tbxDatofin.Text) + ")";
                        }
                        else { filtro = ""; }

                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Productos ORDER BY Codigo WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Productos ORDER BY Codigo";

                        productosTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        productosTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        productosTableAdapter.Fill(basededataset1.Productos);

                        if (basededataset1.Productos.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Listados_Maestros(1, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "Rarti.rdlc";  //new RARTI().ResourceName;
                        }

                    } break;
                case 2:   //imprime el listado de Ingredientes
                    {
                        if (tbxDatoini.Text.Length > 0 && tbxDatofin.Text.Length > 0)
                        {
                            filtro = "({Ingredientes.id_ingrediente} in " + Convert.ToInt32(tbxDatoini.Text) + " to " + Convert.ToInt32(tbxDatofin.Text) + ")";
                            condi = "(id_ingrediente >= " + Convert.ToInt32(tbxDatoini.Text) + " and id_ingrediente <= " + Convert.ToInt32(tbxDatofin.Text) + ")";
                        }
                        else filtro = "";

                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ingredientes Order by id_ingrediente WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ingredientes Order by id_ingrediente";

                        ingredienteTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ingredienteTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ingredienteTableAdapter.Fill(basededataset1.Ingredientes);

                        if (basededataset1.Ingredientes.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Listados_Maestros(2, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "Rinfoadd.rdlc";  //new RINFOADD().ResourceName;                            
                        }
                    } break;
                case 3:   //imprime el listado de publicacion
                    {
                        if (tbxDatoini.Text.Length > 0 && tbxDatofin.Text.Length > 0)
                        {
                            filtro = "{Publicidad.id_publicidad} in " + Convert.ToInt32(tbxDatoini.Text) + " to " + Convert.ToInt32(tbxDatofin.Text);
                            condi = "(id_publicidad >= " + Convert.ToInt32(tbxDatoini.Text) + " and id_publicidad <= " + Convert.ToInt32(tbxDatofin.Text) + ")";
                        }
                        else { filtro = ""; }

                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Publicidad Order by id_publicidad WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Publicidad Order by id_publicidad";

                        publicidadTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        publicidadTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        publicidadTableAdapter.Fill(basededataset1.Publicidad);

                        if (basededataset1.Publicidad.Rows.Count > 0) hay = true;
                        else hay = false;
                        if (hay)
                        {
                            pa.Listados_Maestros(3, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "Rpubl.rdlc";  //new RPUBL().ResourceName;
                        }
                    } break;
                case 4:   //imprime el listado de ofertas
                    {
                        if (tbxDatoini.Text.Length > 0 && tbxDatofin.Text.Length > 0)
                        {
                            filtro = "{Oferta.id_oferta} in " + Convert.ToInt32(tbxDatoini.Text) + " to " + Convert.ToInt32(tbxDatofin.Text);
                            condi = "(id_oferta >= " + Convert.ToInt32(tbxDatoini.Text) + " and id_oferta <= " + Convert.ToInt32(tbxDatofin.Text) + ")";
                        }
                        else { filtro = ""; }

                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Oferta Order by id_oferta WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Oferta Order by id_oferta";

                        ofertasTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ofertasTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ofertasTableAdapter.Fill(basededataset1.Oferta);

                        if (basededataset1.Oferta.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Listados_Maestros(4, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "Roferta.rdlc";  //new ROferta().ResourceName;
                        }
                    } break;
                case 5:   //imprime el listado de vendedore
                    {
                        if (tbxDatoini.Text.Length > 0 && tbxDatofin.Text.Length > 0)
                        {
                            filtro = "({Vendedor.id_vendedor} in " + Convert.ToInt32(tbxDatoini.Text) + " to " + Convert.ToInt32(tbxDatofin.Text) + ")";
                            condi = "(id_vendedor >= " + Convert.ToInt32(tbxDatoini.Text) + " and id_vendedor <= " + Convert.ToInt32(tbxDatofin.Text) + ")";
                        }
                        else filtro = "";

                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Vendedor Order by id_vendedor WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Vendedor Order by id_vendedor";

                        vendedoresTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        vendedoresTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        vendedoresTableAdapter.Fill(basededataset1.Vendedor);

                        if (basededataset1.Vendedor.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Listados_Maestros(5, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "Ragte.rdlc";  //new RAGTE().ResourceName;
                        }
                    } break;
            }
            return nreporte;
        }
        private string Imprimir_Reportes_Estadisticos(int opcion, ref string filtro, ref bool hay, ref string condi)
        {

            string nreporte = "";

            string filtro1 = "", filtro2 = "", filtro3 = "", filtro4 = "";

            string fecha = string.Format(Variable.F_Fecha, DateTime.Now);

            string hora1 = string.Format(Variable.F_Hora, DateTime.Now);

            if (tbxFeinicio.Text.Length > 0 && tbxFefinal.Text.Length > 0)
            {
                if (opcion != 2)
                    filtro1 = "(DateValue({Ventas.Fecha}) in DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "') to DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "'))";
                else
                    filtro1 = "(DateValue({Ventas_Detalle.Fecha}) in DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "') to DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "'))";

                //condi = "(DateValue(Fecha) >= DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "') and DateValue(Fecha) <= DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "'))";               
                //condi = "( Cdate(Fecha) >= Cdate('" + tbxFeinicio.Text + "') and  Cdate(Fecha) <= Cdate('" + tbxFefinal.Text + "'))";
                condi = "( Convert(Fecha,'System.DateTime') >= '" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "' and  Convert(Fecha,'System.DateTime') <= '" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "')";

            }
            if (cBxNserie.SelectedIndex > 0)
            {
                filtro2 = " and ({Ventas.id_bascula} = " + Convert.ToInt32(cBxNserie.SelectedValue) + ")";
                condi = condi + " and (id_bascula = " + Convert.ToInt32(cBxNserie.SelectedValue) + ")";
            }
            if (tbxProdini.Text.Length > 0 && tbxProdfin.Text.Length > 0)
            {
                if (opcion == 2)
                {
                    filtro3 = " and ({Ventas_Detalle.Codigo} in " + Convert.ToInt32(tbxProdini.Text) + " to " + Convert.ToInt32(tbxProdfin.Text) + ")";
                    condi = condi + " and (Codigo >= " + Convert.ToInt32(tbxProdini.Text) + " and Codigo <= " + Convert.ToInt32(tbxProdfin.Text) + ")";
                }
            }
            if (tbxVendini.Text.Length > 0 && tbxVendfin.Text.Length > 0)
            {
                filtro4 = " and ({Ventas.id_vendedor} in " + Convert.ToInt32(tbxVendini.Text) + " to " + Convert.ToInt32(tbxVendfin.Text) + ")";
                condi = condi + " and (id_vendedor >= " + Convert.ToInt32(tbxVendini.Text) + " and id_vendedor <= " + Convert.ToInt32(tbxVendfin.Text) + ")";
            }

            filtro = filtro1 + filtro2 + filtro3 + filtro4;

            Parametros pa = new Parametros();

            switch (opcion)
            {
                case 0:   //imprime el analisis por grupo
                    {

                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas WHERE " + condi;

                        else Conec.CadenaSelect = "SELECT * FROM Ventas";

                        ventasTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventasTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventasTableAdapter.Fill(basededataset1.Ventas);

                        if (basededataset1.Ventas.Rows.Count > 0) hay = true;

                        else hay = false;

                        if (hay)
                        {
                            pa.Analisis_Venta(0, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "AVxDepto.rdlc";  //().ResourceName;
                        }

                    } break;
                case 1:  //imprimir analisis de bascula
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas";
                        ventasTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventasTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventasTableAdapter.Fill(basededataset1.Ventas);

                        if (basededataset1.Ventas.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Analisis_Venta(1, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "AVxBasc.rdlc"; // new AVxBasc().ResourceName;
                        }

                    } break;
                case 2: //Imprime analisis de producto
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY Codigo WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY Codigo";

                        ventadetalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventadetalleTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventadetalleTableAdapter.Fill(basededataset1.Ventas_Detalle);

                        if (basededataset1.Ventas_Detalle.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Analisis_Venta(2, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "AVxProd.rdlc";  // new AVxProd().ResourceName;
                        }

                    } break;
                case 3:   //imprime analisis de vendedor
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas";

                        ventasTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventasTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventasTableAdapter.Fill(basededataset1.Ventas);

                        if (basededataset1.Ventas.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Analisis_Venta(3, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "AVxVendedor.rdlc";  // new AVxVendedor().ResourceName;
                        }
                    } break;
            }
            return nreporte;
        }

        #region metodos para reportes desglosados de estadisticos



        private string Imprimir_Reportes_Estadisticos_Con_Desglose(int opcion, ref string filtro, ref bool hay, ref string condi)
        {
            string nreporte = "";
            string filtro1 = "", filtro2 = "", filtro3 = "", filtro4 = "";

            string fecha = string.Format(Variable.F_Fecha, DateTime.Now);
            string hora1 = string.Format(Variable.F_Hora, DateTime.Now);

            if (tbxFeinicio.Text.Length > 0 && tbxFefinal.Text.Length > 0)
            {
                if (opcion != 2)
                    filtro1 = "(DateValue({Ventas.Fecha}) in DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "') to DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "'))";
                else
                    filtro1 = "(DateValue({Ventas_Detalle.Fecha}) in DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "') to DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "'))";

                //condi = "(DateValue(Fecha) >= DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "') and DateValue(Fecha) <= DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "'))";               
                //condi = "( Cdate(Fecha) >= Cdate('" + tbxFeinicio.Text + "') and  Cdate(Fecha) <= Cdate('" + tbxFefinal.Text + "'))";
                condi = "( Convert(Fecha,'System.DateTime') >= '" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "' and  Convert(Fecha,'System.DateTime') <= '" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "')";

            }
            if (cBxNserie.SelectedIndex > 0)
            {
                filtro2 = " and ({Ventas.id_bascula} = " + Convert.ToInt32(cBxNserie.SelectedValue) + ")";
                condi = condi + " and (id_bascula = " + Convert.ToInt32(cBxNserie.SelectedValue) + ")";
            }
            if (tbxProdini.Text.Length > 0 && tbxProdfin.Text.Length > 0)
            {
                if (opcion == 2)
                {
                    filtro3 = " and ({Ventas_Detalle.Codigo} in " + Convert.ToInt32(tbxProdini.Text) + " to " + Convert.ToInt32(tbxProdfin.Text) + ")";
                    condi = condi + " and (Codigo >= " + Convert.ToInt32(tbxProdini.Text) + " and Codigo <= " + Convert.ToInt32(tbxProdfin.Text) + ")";
                }
            }
            if (tbxVendini.Text.Length > 0 && tbxVendfin.Text.Length > 0)
            {
                filtro4 = " and ({Ventas.id_vendedor} in " + Convert.ToInt32(tbxVendini.Text) + " to " + Convert.ToInt32(tbxVendfin.Text) + ")";
                condi = condi + " and (id_vendedor >= " + Convert.ToInt32(tbxVendini.Text) + " and id_vendedor <= " + Convert.ToInt32(tbxVendfin.Text) + ")";
            }

            filtro = filtro1 + filtro2 + filtro3 + filtro4;

            Parametros pa = new Parametros();

            switch (opcion)
            {
                case 0:   //imprime el analisis por grupo
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas";

                        ventasTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventasTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventasTableAdapter.Fill(basededataset1.Ventas);

                        if (basededataset1.Ventas.Rows.Count > 0) hay = true;
                        else hay = false;
                        if (hay)
                        {
                            pa.Analisis_Venta_Estadisticos_Desglose(0, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "AVxDeptoImp.rdlc";  //().ResourceName;
                        }
                    } break;
                case 1:  //imprimir analisis de bascula
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas";
                        ventasTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventasTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventasTableAdapter.Fill(basededataset1.Ventas);

                        if (basededataset1.Ventas.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Analisis_Venta_Desglose(1, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "AVxBasc.rdlc"; // new AVxBasc().ResourceName;
                        }

                    } break;
                case 2: //Imprime analisis de producto
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY Codigo WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY Codigo";

                        ventadetalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventadetalleTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventadetalleTableAdapter.Fill(basededataset1.Ventas_Detalle);

                        if (basededataset1.Ventas_Detalle.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Analisis_Venta_Desglose(2, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "AVxProd.rdlc";  // new AVxProd().ResourceName;
                        }

                    } break;
                case 3:   //imprime analisis de vendedor
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas";

                        ventasTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventasTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventasTableAdapter.Fill(basededataset1.Ventas);

                        if (basededataset1.Ventas.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Analisis_Venta_Desglose(3, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "AVxVendedor.rdlc";  // new AVxVendedor().ResourceName;
                        }
                    } break;
            }
            return nreporte;
        }



        private string Imprimir_Reportes_Estadisticos_Con_Desglose_1(int opcion, ref string filtro, ref bool hay, ref string condi)
        {
            string nreporte = "";
            string filtro1 = "", filtro2 = "", filtro3 = "", filtro4 = "";

            string fecha = string.Format(Variable.F_Fecha, DateTime.Now);
            string hora1 = string.Format(Variable.F_Hora, DateTime.Now);

            if (tbxFeinicio.Text.Length > 0 && tbxFefinal.Text.Length > 0)
            {
                if (opcion != 2)
                    filtro1 = "(DateValue({Ventas.Fecha}) in DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "') to DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "'))";
                else
                    filtro1 = "(DateValue({Ventas_Detalle.Fecha}) in DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "') to DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "'))";

                //condi = "(DateValue(Fecha) >= DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "') and DateValue(Fecha) <= DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "'))";               
                //condi = "( Cdate(Fecha) >= Cdate('" + tbxFeinicio.Text + "') and  Cdate(Fecha) <= Cdate('" + tbxFefinal.Text + "'))";
                condi = "( Convert(Fecha,'System.DateTime') >= '" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "' and  Convert(Fecha,'System.DateTime') <= '" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "')";

            }
            if (cBxNserie.SelectedIndex > 0)
            {
                filtro2 = " and ({Ventas.id_bascula} = " + Convert.ToInt32(cBxNserie.SelectedValue) + ")";
                condi = condi + " and (id_bascula = " + Convert.ToInt32(cBxNserie.SelectedValue) + ")";
            }
            if (tbxProdini.Text.Length > 0 && tbxProdfin.Text.Length > 0)
            {
                if (opcion == 2)
                {
                    filtro3 = " and ({Ventas_Detalle.Codigo} in " + Convert.ToInt32(tbxProdini.Text) + " to " + Convert.ToInt32(tbxProdfin.Text) + ")";
                    condi = condi + " and (Codigo >= " + Convert.ToInt32(tbxProdini.Text) + " and Codigo <= " + Convert.ToInt32(tbxProdfin.Text) + ")";
                }
            }
            if (tbxVendini.Text.Length > 0 && tbxVendfin.Text.Length > 0)
            {
                filtro4 = " and ({Ventas.id_vendedor} in " + Convert.ToInt32(tbxVendini.Text) + " to " + Convert.ToInt32(tbxVendfin.Text) + ")";
                condi = condi + " and (id_vendedor >= " + Convert.ToInt32(tbxVendini.Text) + " and id_vendedor <= " + Convert.ToInt32(tbxVendfin.Text) + ")";
            }

            filtro = filtro1 + filtro2 + filtro3 + filtro4;

            
            
            Parametros pa = new Parametros();

            switch (opcion)
            {
                case 0:   //imprime el analisis por grupo
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas";

                        ventasTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventasTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventasTableAdapter.Fill(basededataset1.Ventas);

                        if (basededataset1.Ventas.Rows.Count > 0) hay = true;
                        else hay = false;
                        if (hay)
                        {
                            pa.Analisis_Venta_Estadisticos_Desglose(0, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "AVxDeptoImp.rdlc";  //().ResourceName;
                        }
                    } 
                    break;

                case 1:  //imprimir analisis de bascula
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas";
                        ventasTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventasTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventasTableAdapter.Fill(basededataset1.Ventas);

                        if (basededataset1.Ventas.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Analisis_Venta_Desglose(1, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "AVxBascImp.rdlc"; // new AVxBasc().ResourceName;
                        }

                    } 
                    break;

                case 2: //Imprime analisis de producto
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY Codigo WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY Codigo";

                        ventadetalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventadetalleTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventadetalleTableAdapter.Fill(basededataset1.Ventas_Detalle);

                        if (basededataset1.Ventas_Detalle.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Analisis_Venta_Desglose(2, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "AVxProdImp.rdlc";  // new AVxProd().ResourceName;
                        }

                    } break;
                case 3:   //imprime analisis de vendedor
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas";

                        ventasTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventasTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventasTableAdapter.Fill(basededataset1.Ventas);

                        if (basededataset1.Ventas.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Analisis_Venta_Desglose(3, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "AVxVendedorImp.rdlc";  // new AVxVendedor().ResourceName;
                        }
                    } break;
            }
            return nreporte;
        }


        private string Imprimir_Reportes_Estadisticos_Con_Desglose_2(int opcion, ref string filtro, ref bool hay, ref string condi)
        {
            string nreporte = "";
            string filtro1 = "", filtro2 = "", filtro3 = "", filtro4 = "";

            string fecha = string.Format(Variable.F_Fecha, DateTime.Now);
            string hora1 = string.Format(Variable.F_Hora, DateTime.Now);

            if (tbxFeinicio.Text.Length > 0 && tbxFefinal.Text.Length > 0)
            {
                if (opcion != 2)
                    filtro1 = "(DateValue({Ventas.Fecha}) in DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "') to DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "'))";
                else
                    filtro1 = "(DateValue({Ventas_Detalle.Fecha}) in DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "') to DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "'))";

                //condi = "(DateValue(Fecha) >= DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "') and DateValue(Fecha) <= DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "'))";               
                //condi = "( Cdate(Fecha) >= Cdate('" + tbxFeinicio.Text + "') and  Cdate(Fecha) <= Cdate('" + tbxFefinal.Text + "'))";
                condi = "( Convert(Fecha,'System.DateTime') >= '" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "' and  Convert(Fecha,'System.DateTime') <= '" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "')";

            }
            if (cBxNserie.SelectedIndex > 0)
            {
                filtro2 = " and ({Ventas.id_bascula} = " + Convert.ToInt32(cBxNserie.SelectedValue) + ")";
                condi = condi + " and (id_bascula = " + Convert.ToInt32(cBxNserie.SelectedValue) + ")";
            }
            if (tbxProdini.Text.Length > 0 && tbxProdfin.Text.Length > 0)
            {
                if (opcion == 2)
                {
                    filtro3 = " and ({Ventas_Detalle.Codigo} in " + Convert.ToInt32(tbxProdini.Text) + " to " + Convert.ToInt32(tbxProdfin.Text) + ")";
                    condi = condi + " and (Codigo >= " + Convert.ToInt32(tbxProdini.Text) + " and Codigo <= " + Convert.ToInt32(tbxProdfin.Text) + ")";
                }
            }
            if (tbxVendini.Text.Length > 0 && tbxVendfin.Text.Length > 0)
            {
                filtro4 = " and ({Ventas.id_vendedor} in " + Convert.ToInt32(tbxVendini.Text) + " to " + Convert.ToInt32(tbxVendfin.Text) + ")";
                condi = condi + " and (id_vendedor >= " + Convert.ToInt32(tbxVendini.Text) + " and id_vendedor <= " + Convert.ToInt32(tbxVendfin.Text) + ")";
            }

            filtro = filtro1 + filtro2 + filtro3 + filtro4;



            Parametros pa = new Parametros();

            switch (opcion)
            {
                case 0:   //imprime el analisis por grupo
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas";

                        ventasTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventasTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventasTableAdapter.Fill(basededataset1.Ventas);

                        if (basededataset1.Ventas.Rows.Count > 0) hay = true;
                        else hay = false;
                        if (hay)
                        {
                            pa.Analisis_Venta_Estadisticos_Desglose(0, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "AVxDeptoImp.rdlc";  //().ResourceName;
                        }
                    }
                    break;

                case 1:  //imprimir analisis de bascula
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas";
                        ventasTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventasTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventasTableAdapter.Fill(basededataset1.Ventas);

                        if (basededataset1.Ventas.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Analisis_Venta_Desglose(1, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "AVxBascImp.rdlc"; // new AVxBasc().ResourceName;
                        }

                    }
                    break;

                case 2: //Imprime analisis de producto
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY Codigo WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY Codigo";

                        ventadetalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventadetalleTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventadetalleTableAdapter.Fill(basededataset1.Ventas_Detalle);

                        if (basededataset1.Ventas_Detalle.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Analisis_Venta_Desglose(2, fecha, hora1, ref paramFields);

                            nreporte = Variable.appPath + "\\Reportes\\" + "AVxProdImp.rdlc";  // new AVxProd().ResourceName;
                        }

                    }
                    break;

                case 3:   //imprime analisis de vendedor
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas";

                        ventasTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventasTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventasTableAdapter.Fill(basededataset1.Ventas);

                        if (basededataset1.Ventas.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Analisis_Venta_Desglose(3, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "AVxVendedorImp.rdlc";  // new AVxVendedor().ResourceName;
                        }
                    } break;
            }
            return nreporte;
        }




        private string Imprimir_Reportes_Estadisticos_Con_Desglose_3(int opcion, ref string filtro, ref bool hay, ref string condi)
        {
            string nreporte = "";
            string filtro1 = "", filtro2 = "", filtro3 = "", filtro4 = "";

            string fecha = string.Format(Variable.F_Fecha, DateTime.Now);
            string hora1 = string.Format(Variable.F_Hora, DateTime.Now);

            if (tbxFeinicio.Text.Length > 0 && tbxFefinal.Text.Length > 0)
            {
                if (opcion != 2)
                    filtro1 = "(DateValue({Ventas.Fecha}) in DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "') to DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "'))";
                else
                    filtro1 = "(DateValue({Ventas_Detalle.Fecha}) in DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "') to DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "'))";

                //condi = "(DateValue(Fecha) >= DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "') and DateValue(Fecha) <= DateValue('" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "'))";               
                //condi = "( Cdate(Fecha) >= Cdate('" + tbxFeinicio.Text + "') and  Cdate(Fecha) <= Cdate('" + tbxFefinal.Text + "'))";
                condi = "( Convert(Fecha,'System.DateTime') >= '" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFeinicio.Value) + "' and  Convert(Fecha,'System.DateTime') <= '" + string.Format(Variable.FOR_FECHAS[Variable.ffecha], tbxFefinal.Value) + "')";

            }
            if (cBxNserie.SelectedIndex > 0)
            {
                filtro2 = " and ({Ventas.id_bascula} = " + Convert.ToInt32(cBxNserie.SelectedValue) + ")";
                condi = condi + " and (id_bascula = " + Convert.ToInt32(cBxNserie.SelectedValue) + ")";
            }
            if (tbxProdini.Text.Length > 0 && tbxProdfin.Text.Length > 0)
            {
                if (opcion == 2)
                {
                    filtro3 = " and ({Ventas_Detalle.Codigo} in " + Convert.ToInt32(tbxProdini.Text) + " to " + Convert.ToInt32(tbxProdfin.Text) + ")";
                    condi = condi + " and (Codigo >= " + Convert.ToInt32(tbxProdini.Text) + " and Codigo <= " + Convert.ToInt32(tbxProdfin.Text) + ")";
                }
            }
            if (tbxVendini.Text.Length > 0 && tbxVendfin.Text.Length > 0)
            {
                filtro4 = " and ({Ventas.id_vendedor} in " + Convert.ToInt32(tbxVendini.Text) + " to " + Convert.ToInt32(tbxVendfin.Text) + ")";
                condi = condi + " and (id_vendedor >= " + Convert.ToInt32(tbxVendini.Text) + " and id_vendedor <= " + Convert.ToInt32(tbxVendfin.Text) + ")";
            }

            filtro = filtro1 + filtro2 + filtro3 + filtro4;



            Parametros pa = new Parametros();

            switch (opcion)
            {
                case 0:   //imprime el analisis por grupo
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas";

                        ventasTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventasTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventasTableAdapter.Fill(basededataset1.Ventas);

                        if (basededataset1.Ventas.Rows.Count > 0) hay = true;
                        else hay = false;
                        if (hay)
                        {
                            pa.Analisis_Venta_Estadisticos_Desglose(0, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "AVxDeptoImp.rdlc";  //().ResourceName;
                        }
                    }
                    break;

                case 1:  //imprimir analisis de bascula
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas";
                        ventasTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventasTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventasTableAdapter.Fill(basededataset1.Ventas);

                        if (basededataset1.Ventas.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Analisis_Venta_Desglose(1, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "AVxBascImp.rdlc"; // new AVxBasc().ResourceName;
                        }

                    }
                    break;

                case 2: //Imprime analisis de producto
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY Codigo WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas_Detalle ORDER BY Codigo";

                        ventadetalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventadetalleTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventadetalleTableAdapter.Fill(basededataset1.Ventas_Detalle);

                        if (basededataset1.Ventas_Detalle.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Analisis_Venta_Desglose(2, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "AVxProdImp.rdlc";  // new AVxProd().ResourceName;
                        }

                    } break;
                case 3:   //imprime analisis de vendedor
                    {
                        if (filtro != "") Conec.CadenaSelect = "SELECT * FROM Ventas WHERE " + condi;
                        else Conec.CadenaSelect = "SELECT * FROM Ventas";

                        ventasTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                        ventasTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                        ventasTableAdapter.Fill(basededataset1.Ventas);

                        if (basededataset1.Ventas.Rows.Count > 0) hay = true;
                        else hay = false;

                        if (hay)
                        {
                            pa.Analisis_Venta_Desglose(3, fecha, hora1, ref paramFields);
                            nreporte = Variable.appPath + "\\Reportes\\" + "AVxVendedorImp.rdlc";  // new AVxVendedor().ResourceName;
                        }
                    } break;
            }
            return nreporte;
        }
       
        
        
        
        #endregion
        //Metodologia para imprimir un reporte
        //.- Determinar los filtros y el reporte seleccionado por el usuario.
        //1.- obtener la informacion de la base de datos para el reporte.
        //2.- filtrar y validar la informacion obtenida.
        //3.-identificar el reporte al que se le asignara la informacion
        //4.- identifcar los campos en el reporte a los que pertenece cada uno de los campos resultantes dela base de datos.
        //5.- Asignacion(crear el reporte y asignar llos datos desde las variables).
        //6.- Como visualizar el reporte en la pantalla de usuario(Visor de reporte).
        private void tbxProdini_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxProdini.BackColor;
            tbxProdini.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxProdfin_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxProdfin.BackColor;
            tbxProdfin.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxVendini_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxVendini.BackColor;
            tbxVendini.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxVendfin_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxVendfin.BackColor;
            tbxVendfin.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxProdini_Leave(object sender, EventArgs e)
        {
            tbxProdini.BackColor = colorbefore;
        }
        private void tbxProdfin_Leave(object sender, EventArgs e)
        {
            tbxProdfin.BackColor = colorbefore;
        }
        private void tbxVendini_Leave(object sender, EventArgs e)
        {
            tbxVendini.BackColor = colorbefore;
        }
        private void tbxVendfin_Leave(object sender, EventArgs e)
        {
            tbxVendfin.BackColor = colorbefore;
        }
        private void tbxDatoini_Leave(object sender, EventArgs e)
        {
            tbxDatoini.BackColor = colorbefore;
        }
        private void tbxDatofin_Leave(object sender, EventArgs e)
        {
            tbxDatofin.BackColor = colorbefore;
        }
        private void tbxDatoini_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxDatoini.BackColor;
            tbxDatoini.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxDatofin_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxDatofin.BackColor;
            tbxDatofin.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void DesgloseImpuestos_CheckedChanged(object sender, EventArgs e)
        {

        }
       
    }
}