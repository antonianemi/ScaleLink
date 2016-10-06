using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Net.Sockets;
using System.Collections;
using System.Threading;


namespace MainMenu
{    
    public partial class f4Precio : MdiChildForm
    {
        ADOutil Conec = new ADOutil();
        Int32 idprod = 0;
        Conexion Cte = new Conexion();
        Serial SR = new Serial();
        ArrayList Dato_Nuevo = new ArrayList(); 
        Envia_Dato Env = new Envia_Dato();
 
        private bool BUENO = true;
        private CurrencyManager cmRegister;

        public f4Precio()
        {
            InitializeComponent();
        }                     

        #region Manejo del Grid

        private void dtG_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                baseDeDatosDataSet.Productos.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Productos.CodigoColumn };

                System.Windows.Forms.DataGridView DGart = ((System.Windows.Forms.DataGridView)sender);

                int pos = e.RowIndex; //DGart.CurrentCell.RowIndex;
                if (BUENO && DGart[3, pos].Value.ToString() != "" && DGart[3, pos].Value.ToString() != "0")
                {
                    switch (Variable.n_decimal)
                    {
                        case 0:
                            {
                                if (Convert.ToDecimal(DGart[3, pos].Value) > 99999m)
                                {
                                    MessageBox.Show(this, Variable.SYS_MSJ[25, Variable.idioma]);  //"El precio no puede ser mayor que 99999");
                                    DGart[3, pos].Value = 99999;
                                }
                            } break;
                        case 1:
                            {
                                if (Convert.ToDecimal(DGart[3, pos].Value) > Convert.ToDecimal(string.Format(Variable.F_Decimal, 9999.9m)))
                                {
                                    MessageBox.Show(this, Variable.SYS_MSJ[26, Variable.idioma]);   //"El precio no puede ser mayor que 9999.9");
                                    DGart[3, pos].Value = 9999.9;
                                }
                            } break;
                        case 2:
                            {
                                if (Convert.ToDecimal(DGart[3, pos].Value) > Convert.ToDecimal(string.Format(Variable.F_Decimal, 999.99m)))
                                {
                                    MessageBox.Show(this, Variable.SYS_MSJ[27, Variable.idioma]);   //"El precio no puede ser mayor que 999.99");
                                    DGart[3, pos].Value = 999.99;
                                }
                            } break;
                        case 3:
                            {
                                if (Convert.ToDecimal(DGart[3, pos].Value) > Convert.ToDecimal(string.Format(Variable.F_Decimal, 999.999m)))
                                {
                                    MessageBox.Show(this, Variable.SYS_MSJ[449, Variable.idioma]);   //"El precio no puede ser mayor que 999.999");
                                    DGart[3, pos].Value = 999.999;
                                }
                            } break;
                    }
                    DataRow DrGrp = baseDeDatosDataSet.Productos.Rows.Find(DGart[0, pos].Value);
                    string ultfecha = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);
                   // actualiza_precio(Convert.ToDecimal(DGart[3, pos].Value), Convert.ToInt32(DrGrp["id_producto"].ToString()), ultfecha, true);

                    DrGrp.BeginEdit();
                    DrGrp["precio"] = string.Format(CultureInfo.CreateSpecificCulture(Variable.idioma2), Variable.F_Decimal, Convert.ToDecimal(DGart[3, pos].Value)); 
                    DrGrp["actualizado"] = ultfecha;
                    DrGrp.EndEdit();

                    actualiza_precio(Convert.ToDecimal(DrGrp["precio"]), Convert.ToInt32(DrGrp["id_producto"].ToString()), ultfecha, true);

                    DataSet DSChanges = baseDeDatosDataSet.GetChanges(DataRowState.Modified);
                    if (DSChanges != null || DrGrp.RowState == DataRowState.Added)
                    {
                        DrGrp.AcceptChanges();
                        cmRegister = (CurrencyManager)this.BindingContext[baseDeDatosDataSet, baseDeDatosDataSet.Productos.TableName];
                    }
                }               
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void dtG_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (!e.ThrowException)
                MessageBox.Show(this, Variable.SYS_MSJ[86, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma]); 
        }

        private void dtG_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                System.Windows.Forms.DataGridView DGart = ((System.Windows.Forms.DataGridView)sender);
                int pos = e.RowIndex;
                if (DGart[3, pos].Value.ToString() != "" && DGart[3, pos].Value.ToString() != "0")
                {
                    switch (Variable.n_decimal)
                    {
                        case 0:
                            {
                                if (Convert.ToDecimal(DGart[3, pos].Value) > 99999m)
                                {
                                    MessageBox.Show(this, Variable.SYS_MSJ[25, Variable.idioma]);  //"El precio no puede ser mayor que 99999");
                                    DGart[3, pos].Value = 99999;
                                    BUENO = false;
                                }
                            } break;
                        case 1:
                            {
                                if (Convert.ToDecimal(DGart[3, pos].Value) > Convert.ToDecimal(string.Format(Variable.F_Decimal, 9999.9m)))
                                {
                                    MessageBox.Show(this, Variable.SYS_MSJ[26, Variable.idioma]);   //"El precio no puede ser mayor que 999.99");
                                    DGart[3, pos].Value = 9999.9;
                                    BUENO = false;
                                }
                            } break;
                        case 2:
                            {
                                if (Convert.ToDecimal(DGart[3, pos].Value) > Convert.ToDecimal(string.Format(Variable.F_Decimal, 999.99m)))
                                {
                                    MessageBox.Show(this, Variable.SYS_MSJ[27, Variable.idioma]);   //"El precio no puede ser mayor que 999.99");
                                    DGart[3, pos].Value = 999.99; 
                                    BUENO = false;
                                }
                            } break;
                        case 3:
                            {
                                if (Convert.ToDecimal(DGart[3, pos].Value) > Convert.ToDecimal(string.Format(Variable.F_Decimal, 999.999m)))
                                {
                                    MessageBox.Show(this, Variable.SYS_MSJ[449, Variable.idioma]);   //"El precio no puede ser mayor que 999.999");
                                    DGart[3, pos].Value = 999.999;
                                    BUENO = false;
                                }
                            } break;
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void dtG_ReadOnlyChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.DataGridView DGart = ((System.Windows.Forms.DataGridView)sender);
            
            if (!DGart.ReadOnly)
            {
                DGart.BeginEdit(true);
                BUENO = true;
            }
        }

        private void dtG_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            cmRegister.Position = e.RowIndex;
            try
            {
                System.Windows.Forms.DataGridView DGart = ((System.Windows.Forms.DataGridView)sender);

                DGart.ReadOnly = true;

                if (e.RowIndex >= 0 && e.RowIndex < cmRegister.Count)
                {
                    if (e.ColumnIndex == 3) DGart.ReadOnly = false;

                    Int32 clave = (Int32)DGart.CurrentRow.Cells[0].Value;
                    DataRow dr = baseDeDatosDataSet.Productos.Rows.Find(clave);
                    Mostrar_Dato(ref dr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void dtG_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            System.Windows.Forms.DataGridView reg = ((System.Windows.Forms.DataGridView)sender);
            this.bindingNavigatorPositionItem.Text = Convert.ToString(cmRegister.Position + 1);
            try
            {
                if (cmRegister.Position >= 0 && cmRegister.Position < cmRegister.Count)
                {
                    this.bindingNavigatorMoveFirstItem.Enabled = true;
                    this.bindingNavigatorMovePreviousItem.Enabled = true;
                    this.bindingNavigatorMoveNextItem.Enabled = true;
                    this.bindingNavigatorMoveLastItem.Enabled = true;
                    cmRegister.Position = e.RowIndex;
                }
                if (cmRegister.Position < 0)
                {
                    this.bindingNavigatorMoveNextItem.Enabled = true;
                    this.bindingNavigatorMoveLastItem.Enabled = true;
                    this.bindingNavigatorMoveFirstItem.Enabled = false;
                    this.bindingNavigatorMovePreviousItem.Enabled = false;
                    cmRegister.Position = 0;
                }
                if (cmRegister.Position >= cmRegister.Count)
                {
                    this.bindingNavigatorMoveNextItem.Enabled = false;
                    this.bindingNavigatorMoveLastItem.Enabled = false;
                    this.bindingNavigatorMoveFirstItem.Enabled = true;
                    this.bindingNavigatorMovePreviousItem.Enabled = true;
                }

                reg.Select();

                if (e.RowIndex >= 0 && e.RowIndex < cmRegister.Count)
                {
                    // string clave = (string)reg.CurrentRow.Cells[0].Value;
                    Int32 clave = (Int32)reg.CurrentRow.Cells[0].Value;

                    DataRow dr = baseDeDatosDataSet.Productos.Rows.Find(clave);
                    Mostrar_Dato(ref dr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void bindingNavigator1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            this.dtG.ClearSelection();
            try{
            switch (this.bindingNavigator1.Items.IndexOf(e.ClickedItem))
            {
                case 0:
                    cmRegister.Position = 0;
                    break;
                case 1:
                    cmRegister.Position = Convert.ToInt32(this.bindingNavigatorPositionItem.Text) - 1;
                    Conec.Previous(ref cmRegister);
                    break;
                case 6:
                    cmRegister.Position = Convert.ToInt32(this.bindingNavigatorPositionItem.Text) - 1;
                    Conec.Next(ref cmRegister);
                    break;
                case 7:
                    cmRegister.Position = cmRegister.Count - 1;
                    break;
            }
            if (cmRegister.Position > 0 && cmRegister.Position < cmRegister.Count)
            {
                //string clave = (string)dtG[0, cmRegister.Position].Value;
                Int32 clave = (Int32)dtG[0, cmRegister.Position].Value;
                DataRow dr = baseDeDatosDataSet.Productos.Rows.Find(clave);
                Mostrar_Dato(ref dr);
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        public void Llenar_Grid()
        {
            Conec.CadenaSelect = "SELECT FROM Producto ORDER BY codigo WHERE borrado = false";
            productosTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            productosTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);
            baseDeDatosDataSet.Productos.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Productos.CodigoColumn };

            this.bindingSource1.DataSource = baseDeDatosDataSet;
            this.bindingSource1.DataMember = baseDeDatosDataSet.Productos.TableName;
            this.bindingSource1.Filter = "borrado = " + false;
            this.bindingNavigator1.BindingSource = this.bindingSource1;

            cmRegister = (CurrencyManager)this.BindingContext[baseDeDatosDataSet, baseDeDatosDataSet.Productos.TableName];
        }
        private void Grid_articulo()
        {
            try
            {
                DataGridViewTextBoxColumn colStyle1 = new DataGridViewTextBoxColumn();
                colStyle1.HeaderText = Variable.SYS_MSJ[145, Variable.idioma];  //codigo
                colStyle1.Width = 100;
                colStyle1.DefaultCellStyle.NullValue = "0";
                colStyle1.DefaultCellStyle.Format = "######";
                colStyle1.DataPropertyName = "Codigo";
                colStyle1.MaxInputLength = 6;
                DataGridViewTextBoxColumn colStyle2 = new DataGridViewTextBoxColumn();
                colStyle2.HeaderText = Variable.SYS_MSJ[146, Variable.idioma]; //"Descripcion";
                colStyle2.Width = 300;
                colStyle2.DefaultCellStyle.NullValue = "";
                colStyle2.DataPropertyName = "Nombre";
                colStyle2.MaxInputLength = 50;
                DataGridViewTextBoxColumn colStyle3 = new DataGridViewTextBoxColumn();
                colStyle3.HeaderText = Variable.SYS_MSJ[162, Variable.idioma]; //"Num. PLU";
                colStyle3.Width = 100;
                colStyle3.DefaultCellStyle.NullValue = "0";
                colStyle3.DataPropertyName = "NoPlu";
                colStyle3.MaxInputLength = 5;
                DataGridViewTextBoxColumn colStyle4 = new DataGridViewTextBoxColumn();
                colStyle4.HeaderText = Variable.SYS_MSJ[166, Variable.idioma]; //"Precio";
                colStyle4.Width = 100;
                colStyle4.DefaultCellStyle.NullValue = "0";
                colStyle4.DefaultCellStyle.Format = Variable.FOR_FORMAT[Variable.moneda];
                colStyle4.DataPropertyName = "Precio";
                colStyle4.DefaultCellStyle.ForeColor = Color.Red;
                colStyle4.DefaultCellStyle.Font = new Font(dtG.Font, FontStyle.Bold);
                if (Variable.n_decimal == 3) colStyle4.MaxInputLength = 7;
                else colStyle4.MaxInputLength = 6;
                DataGridViewTextBoxColumn colStyle17 = new DataGridViewTextBoxColumn();
                colStyle17.HeaderText = Variable.SYS_MSJ[159, Variable.idioma]; //"Actualizado";
                colStyle17.Width = 130;
                colStyle17.DefaultCellStyle.NullValue = "";
                colStyle17.DefaultCellStyle.Format = Variable.F_Hora + " " + Variable.FOR_FECHAS[Variable.ffecha];
                colStyle17.DataPropertyName = "actualizado";
                colStyle17.MaxInputLength = 20;


                this.dtG.Columns.Add(colStyle1);
                this.dtG.Columns.Add(colStyle2);
                this.dtG.Columns.Add(colStyle3);
                this.dtG.Columns.Add(colStyle4);
                this.dtG.Columns.Add(colStyle17);

                cmRegister = (CurrencyManager)this.BindingContext[baseDeDatosDataSet, baseDeDatosDataSet.Productos.TableName];
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
            
        }

        #endregion

        #region Consulta de base de datos

        private void Mostrar_Dato(ref DataRow dr)
        {
            idprod = Convert.ToInt32(dr["id_producto"].ToString());
            tbxCodigo.Text = dr["Codigo"].ToString();
            tbxPLU.Text = dr["NoPlu"].ToString();
            tbxdescripcion.Text = dr["Nombre"].ToString();
            tbxprecio.Text = string.Format(CultureInfo.CreateSpecificCulture(Variable.idioma2), Variable.F_Decimal, Convert.ToDecimal(dr["Precio"].ToString()));           
            tbxcaducidad.Text = dr["CaducidadDias"].ToString();           
        }

        private void actualiza_precio(decimal precio, int codigo, string feact,bool estado)
        {         
            string sele = "UPDATE Productos SET " +
                "Precio= " + precio + "," +
                "pendiente = " + estado + "," +
                "actualizado =  '" + feact + "' " +
                "WHERE ( id_producto = " + codigo + ")";
            
            try
            {
                Conec.ActualizaReader(Conec.CadenaConexion, sele, baseDeDatosDataSet.Productos.TableName);
                cmRegister = (CurrencyManager)this.BindingContext[baseDeDatosDataSet, baseDeDatosDataSet.Productos.TableName];
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }
        private void actualiza_status_pendiente(int codigo, string feact, bool estado)
        {
            string sele = "UPDATE Productos SET " +
                "pendiente = " + estado + "," +
                "actualizado =  '" + feact + "' " +
                "WHERE ( id_producto = " + codigo + ")";

            try
            {
                Conec.ActualizaReader(Conec.CadenaConexion, sele, baseDeDatosDataSet.Productos.TableName);
                cmRegister = (CurrencyManager)this.BindingContext[baseDeDatosDataSet, baseDeDatosDataSet.Productos.TableName];
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }
        private void Actualiza_PrecioxRango(string incremento, string plu1, string plu2)
        {
            try
            {
                int nt_precio = 0;
                DataRow[] DR = null;

                if (plu2 == "")
                {
                    DR = baseDeDatosDataSet.Productos.Select("Codigo = " + Convert.ToInt32(plu1));
                }
                else
                {
                    DR = baseDeDatosDataSet.Productos.Select("Codigo >= " + Convert.ToInt32(plu1) + " AND Codigo <= " + Convert.ToInt32(plu2) + " AND borrado = false");
                }

                if (DR.Length > 0)
                {
                    ProgressContinue pro = new ProgressContinue();
                    pro.Show();
                    pro.IniciaProcess(DR.Length, Variable.SYS_MSJ[197, Variable.idioma]);

                    foreach (DataRow AP in DR)
                    {
                        AP.BeginEdit();
                        AP["Precio"] = string.Format(CultureInfo.CreateSpecificCulture(Variable.idioma2), Variable.F_Decimal, Convert.ToDecimal(AP["Precio"].ToString()) + (Convert.ToDecimal(AP["Precio"].ToString()) * (Convert.ToDecimal(incremento) / 100)));
                        AP["pendiente"] = true;
                        AP["actualizado"] = String.Format(Variable.F_Hora, DateTime.Now) + " " + String.Format(Variable.F_Fecha, DateTime.Now);
                        AP.EndEdit();

                        actualiza_precio(Convert.ToDecimal(AP["Precio"].ToString()), Convert.ToInt32(AP["id_producto"].ToString()), AP["actualizado"].ToString(), true);

                        nt_precio++;
                        pro.UpdateProcess(1, Variable.SYS_MSJ[197, Variable.idioma]);
                    }

                    MessageBox.Show(this, Variable.SYS_MSJ[384, Variable.idioma] + " " + DR.Length + Variable.SYS_MSJ[167, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);

                    pro.TerminaProcess();
                    Thread.Sleep(500);
                }
                else
                {
                    if (plu2 == "")
                    {
                        MessageBox.Show(this, Variable.SYS_MSJ[382, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(this, Variable.SYS_MSJ[383, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Variable.SYS_MSJ[381, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }      
        #endregion

        #region envio de informacion y toolstrip
        /*public bool EnviaDatosA_Bascula(string direccionIP, long nSucursal, long nbascula)
        {
            bool STATUS = false;
            Cursor.Current = Cursors.WaitCursor;

            Cliente_bascula = Cte.conectar(direccionIP, 50036);  //, Variable.frame, ref Dato_Recivido);
            if (Cliente_bascula != null)
            {
                string sComando = "XX" + (char)9 + (char)10;

                string Msg_Recibido = Env.Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");
                if (Msg_Recibido != null)
                {
                    STATUS = Enviar_Precio_Bascula(direccionIP, ref Cliente_bascula, nbascula, nSucursal);
                }
                Cte.desconectar(ref Cliente_bascula);
                Cursor.Current = Cursors.Default;
                return STATUS;
            }
            else
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(this, Variable.SYS_MSJ[194, Variable.idioma] + myCurrent.Nserie + Variable.SYS_MSJ[195, Variable.idioma] + direccionIP + Variable.SYS_MSJ[196, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }        
        public bool EnviaDatosA_Bascula(int puerto, Int32 Baud_rate,long nbascula,long nsucursal)
        {
            bool STATUS;
            Cursor.Current = Cursors.WaitCursor;

            serialPort1 = new SerialPort();

            if (SR.OpenPort(ref serialPort1, puerto, Baud_rate))
            {
                SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)9 + (char)10);

                STATUS = Enviar_Precio_Bascula(ref serialPort1, nbascula, nsucursal);

                SR.SendCOMSerial(ref serialPort1, "DXXX" + (char)9 + (char)10);
                SR.ClosePort(ref serialPort1);
                Cursor.Current = Cursors.Default;
                return STATUS;
            }
            else
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(this, Variable.SYS_MSJ[194, Variable.idioma] + Variable.Bascula.ToString() + Variable.SYS_MSJ[195, Variable.idioma] + Variable.Nserie + Variable.SYS_MSJ[196, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
        
        private void Actualizar_Status_Precio(int nbascula, int nsucursal)
        {
            string fecha_ult = DateTime.Now.ToShortTimeString() + " " + DateTime.Now.ToShortDateString();
            int reg_leido = 0;
            int reg_envio = 0;

            pLU_detalleTableAdapter.Fill(baseDeDatosDataSet.PLU_detalle);
            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);

            DataRelation Producto_Detalle = baseDeDatosDataSet.Relations["Productos_PLU_detalle"];//"ProductosProd_detalle"];
            int reg_pendiente = baseDeDatosDataSet.Productos.Select("pendiente = " + true).Length;
            int reg_total = baseDeDatosDataSet.PLU_detalle.Select("id_bascula = " + nbascula + " AND id_grupo = " + nsucursal).Length;

            if (reg_pendiente > 0)
            {
                Process pro = new Process();
                pro.Show();
                pro.IniciaProcess(reg_total, Variable.SYS_MSJ[198, Variable.idioma] + " " + myCurrent.Nserie, Variable.SYS_MSJ[192, Variable.idioma], 1);  //"Enviando Cambio de Precio","Iniciando Proceso"

                foreach (DataRow DA in baseDeDatosDataSet.Productos.Rows)
                {
                    foreach (DataRow PR in DA.GetChildRows(Producto_Detalle))
                    {
                        if (Convert.ToInt32(PR["id_bascula"].ToString()) == nbascula && Convert.ToInt32(PR["id_grupo"].ToString()) == nsucursal && Convert.ToBoolean(DA["pendiente"]) == true)
                        {
                            actualiza_status_pendiente(Convert.ToInt32(PR["id_producto"].ToString()), fecha_ult, false);
                            reg_leido++;
                            pro.UpdateProcess(1, reg_envio.ToString() + "/" + reg_total.ToString());
                        }
                    }
                }
                pro.TerminaProcess(Variable.SYS_MSJ[193, Variable.idioma]); //"Proceso terminado");               
            }
        }
        private bool Enviar_Precio_Bascula(string direccionIP, ref Socket Cliente_bascula, long nbascula, long nsucursal)
        {
            string Msj_recibido;
            string Variable_frame;
            int reg_leido = 0;
            int reg_envio = 0;
            bool envia_dato = false;

            //prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);
            pLU_detalleTableAdapter.Fill(baseDeDatosDataSet.PLU_detalle);
            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);

            DataRelation Producto_Detalle = baseDeDatosDataSet.Relations["Productos_PLU_detalle"]; //"ProductosProd_detalle"];

            int reg_pendiente = baseDeDatosDataSet.Productos.Select("pendiente = " + true).Length;
            int reg_total = baseDeDatosDataSet.PLU_detalle.Select("id_bascula = " + nbascula + " AND id_grupo = " + nsucursal).Length;

            Variable_frame = "";
            if (reg_pendiente > 0)
            {
                Process pro = new Process();
                pro.Show();
                pro.IniciaProcess(reg_total, Variable.SYS_MSJ[198, Variable.idioma] + " " + myCurrent.Nserie + " " + direccionIP, Variable.SYS_MSJ[192, Variable.idioma], 1);  //"Enviando Cambio de Precio","Iniciando Proceso"

                foreach (DataRow DA in baseDeDatosDataSet.Productos.Rows)
                {
                    foreach (DataRow PR in DA.GetChildRows(Producto_Detalle))
                    {
                        if (Convert.ToInt32(PR["id_bascula"].ToString()) == nbascula && Convert.ToInt32(PR["id_grupo"].ToString()) == nsucursal && Convert.ToBoolean(DA["pendiente"]) == true)
                        {
                            Variable_frame = Variable_frame + Env.Genera_Trama_Precio(DA);
                            reg_leido++;
                            if (reg_leido > 5)
                            {
                                reg_envio = reg_envio + reg_leido;
                                Msj_recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, nbascula, nsucursal, "Gp");
                                if (Msj_recibido != null)
                                {
                                    envia_dato = true;
                                    //Actualizar_Status_Precio(Variable.frame.Split((char)10));
                                    pro.UpdateProcess(reg_leido, reg_envio.ToString() + "/" + reg_total.ToString());
                                }else envia_dato = false;
                                reg_leido = 0;
                                Variable_frame = "";
                            }
                        }
                    }
                }

                if (Variable_frame.Length > 0 && reg_leido <= 5)
                {
                    reg_envio = reg_envio + reg_leido;
                    Msj_recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, nbascula, nsucursal, "Gp");
                    if (Msj_recibido != null)
                    {
                        envia_dato = true;
                       // Actualizar_Status_Precio(Variable.frame.Split((char)10));
                    }
                    else envia_dato = true;
                    pro.UpdateProcess(reg_leido, reg_envio.ToString() + "/" + reg_total.ToString());
                    reg_leido = 0;
                    Variable_frame = "";
                }
                pro.TerminaProcess(Variable.SYS_MSJ[193, Variable.idioma]); //"Proceso terminado");
                return envia_dato;
            }
            else return false;
        }
        private bool Enviar_Precio_Bascula(ref SerialPort Cliente_bascula, long nbascula, long nsucursal)
        {
            string strcomando;
            string Variable_frame;
            string[] Dato_recibido = null;
            bool envia_dato = false;
            int reg_leido = 0;
            int reg_envio = 0;

           // prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);
            pLU_detalleTableAdapter.Fill(baseDeDatosDataSet.PLU_detalle);
            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);

            DataRelation Producto_Detalle = baseDeDatosDataSet.Relations["Productos_PLU_detalle"];
            int reg_pendiente = baseDeDatosDataSet.Productos.Select("pendiente = " + true).Length;
            int reg_total = baseDeDatosDataSet.PLU_detalle.Select("id_bascula = " + nbascula + " AND id_grupo = " + nsucursal).Length;

            Variable_frame = "";
            if (reg_pendiente > 0)
            {
                Process pro = new Process();
                pro.Show();
                pro.IniciaProcess(reg_total, Variable.SYS_MSJ[198, Variable.idioma] + " " + myCurrent.Nserie, Variable.SYS_MSJ[192, Variable.idioma], 1);  //"Enviando Cambio de Precio","Iniciando Proceso"

                foreach (DataRow DA in baseDeDatosDataSet.Productos.Rows)
                {
                    foreach (DataRow PR in DA.GetChildRows(Producto_Detalle))
                    {
                        if (Convert.ToInt32(PR["id_bascula"].ToString()) == nbascula && Convert.ToInt32(PR["id_grupo"].ToString()) == nsucursal && Convert.ToBoolean(DA["pendiente"]) == true)
                        {
                            Variable_frame = Env.Genera_Trama_Precio(DA);
                            reg_leido++;
                            if (reg_leido > 5)
                            {
                                reg_envio = reg_envio + reg_leido;
                                strcomando = "Gp" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;

                                SR.SendCOMSerial(ref Cliente_bascula, strcomando, ref Dato_recibido);
                                if (Dato_recibido[0].IndexOf("Error") >= 0)
                                {
                                    envia_dato = false;
                                    Env.Guardar_Trama_pendiente(nbascula, nsucursal, strcomando);
                                }
                                if (Dato_recibido[0].IndexOf("Ok") >= 0)
                                {
                                    envia_dato = true;
                                   // Actualizar_Status_Precio(Variable.frame.Split((char)10));
                                }
                                pro.UpdateProcess(reg_leido, reg_envio.ToString() + "/" + reg_total.ToString());
                                reg_leido = 0;
                                Variable_frame = "";
                            }
                        }
                    }
                }

                if (Variable_frame.Length > 0 && reg_leido <= 5)
                {
                    reg_envio = reg_envio + reg_leido;
                    strcomando = "Gp" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                    SR.SendCOMSerial(ref Cliente_bascula, strcomando, ref Dato_recibido);
                    if (Dato_recibido[0].IndexOf("Error") >= 0)
                    {
                        envia_dato = false;
                        Env.Guardar_Trama_pendiente(nbascula, nsucursal, strcomando);
                    }
                    if (Dato_recibido[0].IndexOf("Ok") >= 0)
                    {
                        envia_dato = true;
                      //  Actualizar_Status_Precio(Variable.frame.Split((char)10));
                    }
                    pro.UpdateProcess(reg_leido, reg_envio.ToString() + "/" + reg_total.ToString());
                    reg_leido = 0;
                    Variable_frame = "";
                }
                pro.TerminaProcess(Variable.SYS_MSJ[193, Variable.idioma]); //"Proceso terminado");
                return envia_dato;
            }
            else return false;
        }
        */
        #endregion

        private void toolStripCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
        private void toolStripRango_Click(object sender, EventArgs e)
        {
            RangoPrecios Prec = new RangoPrecios();
            this.AddOwnedForm(Prec);
            Prec.Owner = this;
            Prec.ShowDialog();
            this.Focus();
            if (RangoPrecios.cambiar_precio)
            {
                Actualiza_PrecioxRango(RangoPrecios.incremento, RangoPrecios.codigo_inicio, RangoPrecios.codigo_final);
                Llenar_Grid();
            }
        }
        private void toolStripLoad_Click(object sender, EventArgs e)
        {
            FileStream MyFile;
            string strpath;
            try
            {
                dlgOpenFile.ShowReadOnly = true;
                dlgOpenFile.InitialDirectory = Variable.appPath;

                dlgOpenFile.DefaultExt = "*.txt";
                dlgOpenFile.Filter = "Data files (*.txt)|*.txt|All files(*.*)|*.*";

                if (dlgOpenFile.ShowDialog() == DialogResult.OK)
                {
                    strpath = dlgOpenFile.FileName;
                    if (dlgOpenFile.FileName != "")
                    {
                        if (dlgOpenFile.ReadOnlyChecked == true)
                        {
                            MyFile = (FileStream)dlgOpenFile.OpenFile();
                        }
                        else
                        {
                            MyFile = new FileStream(strpath, FileMode.Open, FileAccess.Read);
                        }
                        ImpExp entrada = new ImpExp();
                        entrada.importar((int)ESTADO.FileSource.fPrecios, ref MyFile, strpath);

                        this.productosTableAdapter.Fill(this.baseDeDatosDataSet.Productos);           
                        Llenar_Grid();
                    }
                    else MessageBox.Show(this, Variable.SYS_MSJ[29, Variable.idioma]); //"No hay archivo seleccionado");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
           
        }
        private void toolStripEnviar_Click(object sender, EventArgs e)
        {
            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);
            DataRow[] DT = baseDeDatosDataSet.Productos.Select("pendiente = " + true + " and borrado = " + false);

            if (DT.Length > 0)
            {
                Env.vActualizar_Bascula_Precio();
            }
            else
            {
                MessageBox.Show(this, Variable.SYS_MSJ[295, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }       

        private void f4Precio_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.PLU_detalle' Puede moverla o quitarla según sea necesario.
            this.pLU_detalleTableAdapter.Fill(this.baseDeDatosDataSet.PLU_detalle);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Bascula' Puede moverla o quitarla según sea necesario.
            this.basculaTableAdapter.Fill(this.baseDeDatosDataSet.Bascula);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Productos' Puede moverla o quitarla según sea necesario.
            this.productosTableAdapter.Fill(this.baseDeDatosDataSet.Productos);

            Llenar_Grid();
            Grid_articulo();
        }

    }
}
