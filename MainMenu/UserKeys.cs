using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MainMenu
{

    public partial class UserKeys : UserControl
    {
        bool isSaved;
        ADOutil Conec = new ADOutil();
        Envia_Dato Env = new Envia_Dato();
        Conexion Cte = new Conexion();
        ContextMenu menu=new ContextMenu();
        List<ProductoDTO> TableRelationship = new List<ProductoDTO>();
        ToolTip tool = new ToolTip();
        Variable.lbasc[] myScale;
        Socket Cliente_bascula = null;
        int index;
        bool Removing;
        MainMenu.BaseDeDatosDataSetTableAdapters.carpeta_detalleTableAdapter carpeta_detalleTableAdapter = new BaseDeDatosDataSetTableAdapters.carpeta_detalleTableAdapter();
        MainMenu.BaseDeDatosDataSetTableAdapters.Prod_detalleTableAdapter prod_detalleTableAdapter = new BaseDeDatosDataSetTableAdapters.Prod_detalleTableAdapter();
        MainMenu.BaseDeDatosDataSetTableAdapters.BasculaTableAdapter basculaTableAdapter= new BaseDeDatosDataSetTableAdapters.BasculaTableAdapter();
        MainMenu.BaseDeDatosDataSetTableAdapters.Prod_detalleTableAdapter prod_detalleTableAdapterAux = new BaseDeDatosDataSetTableAdapters.Prod_detalleTableAdapter();
        MainMenu.BaseDeDatosDataSetTableAdapters.ProductosTableAdapter _productosTableAdapter = new BaseDeDatosDataSetTableAdapters.ProductosTableAdapter();
        Variable.nodo_actual myCurrent;
        public int Num_Bascula { get; set; }
        public int Num_Grupo { get; set; }
        public string Nombre_Select { get; set; }
        private bool Envia_Borrar_Asociacion(string direccionIP, ref Socket Cliente_bascula)
        {
            string[] Dato_Recibido = null;
            string reg_enviado;
            string Variable_frame;
            bool Limpiar = false;

            Variable_frame = "";
            Variable_frame = "PAXX" + (char)9 + (char)10;

            Cte.Envio_Dato(ref Cliente_bascula, direccionIP, Variable_frame, ref Dato_Recibido);

            if (Dato_Recibido != null)
            {
                reg_enviado = Variable_frame.Substring(4);
                if (Dato_Recibido[0].IndexOf("Error") >= 0) { Limpiar = false; }
                if (Dato_Recibido[0].IndexOf("Ok") >= 0) { Limpiar = true; }
            }

            return Limpiar;
        }
        private bool Enviar_DetalleProducto_Bascula(string direccionIP, ref Socket Cliente_bascula, ref ProgressContinue pro)  //long bascula, long sucursal,string direccionIP,ref Socket Cliente_bascula)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msj_recibido;
            string Variable_frame;
            int reg_leido = 0;
            int reg_envio = 0;
            bool ERROR = false;
            int reg_total;

            DataRow[] DR = baseDeDatosDataSet.Prod_detalle.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);

            Variable_frame = "";
            reg_total = DR.Length;

            pro.IniciaProcess(reg_total, Variable.SYS_MSJ[459, Variable.idioma] + " " + myCurrent.Nserie + "... ");  //"Enviando Detalle Producto","Iniciando proceso");

            foreach (DataRow DR_Detail in DR)
            {
                Variable_frame = Variable_frame + Env.Genera_Trama_Producto_Detalle(DR_Detail);

                reg_leido++;
                if (reg_leido > 4)
                {
                    reg_envio = reg_envio + reg_leido;
                    Msj_recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo, "GA");
                    if (Msj_recibido != null)
                    {
                        pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[459, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                    }
                    else
                    {
                        ERROR = true;
                        break;
                    }
                    reg_leido = 0;
                    Variable_frame = "";
                }
            }


            if (Variable_frame.Length > 0 && reg_leido <= 4)
            {
                reg_envio = reg_envio + reg_leido;

                Msj_recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo, "GA");
                if (Msj_recibido != null)
                {
                    pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[459, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                }
                else
                {
                    ERROR = true;
                }
                reg_leido = 0;
                Variable_frame = "";

            }

            Env.Command_Limpiar(direccionIP, ref Cliente_bascula, "GAF0");
            return ERROR;
        }
        private void Cambiar_Estado_pendiente()
        {
            //Conec.CadenaSelect = "UPDATE carpeta_detalle SET pendiente= " + false + " WHERE ( enviado = " + true + ")";
            //Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "carpeta_detalle");
            Conec.CadenaSelect = "UPDATE Prod_detalle SET pendiente= " + false + " WHERE ( enviado = " + true + ")";
            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "Prod_detalle");
        }
        /// <summary>
        /// Este metodo es llamado para crear Un producto de detalle,que indica el orden quen que los productos seran enviados a la bascula.
        /// Este metodo agrega un producto a la vez a la base de datos.
        /// </summary>
        /// <param name="bascula">Bascula a la que se enviarar la informacion.</param>
        /// <param name="sucursal">Sucursal en la que esta la bascula</param>
        /// <param name="carpeta">Carpeta donde estan los productos</param>
        /// <param name="DatosNuevos">Datos del producto que se va a agregar.</param>
        private void Crear_DetalleProducto(long bascula, long sucursal, long carpeta, ProductoDTO _Producto)
        {
            DataRow dr = baseDeDatosDataSet.Prod_detalle.NewRow();

            dr.BeginEdit();
            dr["id_bascula"] = bascula;
            dr["id_grupo"] = sucursal;
            dr["id_carpeta"] = carpeta;
            dr["id_producto"] = _Producto.idProducto;
            dr["codigo"] = _Producto.idProducto;
            dr["NoPLU"] = _Producto.NoPLU;
            dr["posicion"] = _Producto.Position;
            dr["pendiente"] = true;
            dr.EndEdit();

            prod_detalleTableAdapter.Update(dr);

            baseDeDatosDataSet.Prod_detalle.AcceptChanges();
           
            Conec.CadenaSelect = "INSERT INTO Prod_detalle " +
            "(id_bascula,id_grupo,id_carpeta,id_producto, codigo, NoPLU, pendiente,posicion)" +
           "VALUES (" + bascula + "," +   //id_bascula
             sucursal + "," +   //id_grupo  
             carpeta + "," +     // id_carpeta
             Convert.ToInt32(_Producto.idProducto) + "," +     // id_producto
             Convert.ToInt32(_Producto.idProducto) + "," +  //codigo
             Convert.ToInt32(_Producto.NoPLU) + "," + //NoPLU
             true + "," + Convert.ToInt32(_Producto.Position) + ")"; //posicion
            Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Prod_detalle.TableName);
        }
        private void LoadLastKeys()
        {
            TableRelationship.Clear();
            DataTable dt;
            Conec.CadenaSelect = "SELECT * FROM Prod_detalle ORDER BY id_bascula";
            prod_detalleTableAdapterAux.Connection.ConnectionString = Conec.CadenaConexion;
            prod_detalleTableAdapterAux.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            prod_detalleTableAdapterAux.Fill(baseDeDatosDataSet.Prod_detalle);

            foreach(DataRow item in baseDeDatosDataSet.Prod_detalle.Rows)
            {

                Conec.CadenaSelect = "SELECT * FROM Productos WHERE id_producto = " + Convert.ToInt32(item["id_producto"].ToString());
                IDataReader reader=Conec.Obtiene_Dato(Conec.CadenaSelect, Conec.CadenaConexion);
                if (reader != null )
                { 
                while (reader.Read())
                {
                    ProductoDTO p = new ProductoDTO();
                    p.idProducto = Convert.ToInt32(reader["id_producto"].ToString());
                    p.Nombre = reader["Nombre"].ToString();
                    p.NoPLU = item["NoPLU"].ToString();
                    p.precio = reader["precio"].ToString();
                    p.Actualizado = reader["Actualizado"].ToString();
                    p.ControlAsignado = getControl(Convert.ToInt32(item["posicion"].ToString()));
                    TableRelationship.Add(p);
                }
                }

            }
            DrawlingStatusButtons();
        }
        private Label getControl(int p)
        {

            switch (p)
            {
                case 0:return ProductShortcut_1;
                case 1:return ProductShortcut_2;
                case 2:return ProductShortcut_3;
                case 3:return ProductShortcut_4;
                case 4:return ProductShortcut_5;
                case 5:return ProductShortcut_6;
                case 6:return ProductShortcut_7;
                case 7:return ProductShortcut_8;
                case 8:return ProductShortcut_9;
                case 9:return ProductShortcut_10;
                case 10:return ProductShortcut_11;
                case 11:return ProductShortcut_12;
                case 12:return ProductShortcut_13;
                case 13:return ProductShortcut_14;
                case 14:return ProductShortcut_15;
                case 15:return ProductShortcut_16;
                case 16:return ProductShortcut_17;
                case 17:return ProductShortcut_18;
                case 18:return ProductShortcut_19;
                case 19:return ProductShortcut_20;
                case 20:return ProductShortcut_21;
                case 21:return ProductShortcut_22;
                case 22:return ProductShortcut_23;
                case 23:return ProductShortcut_24;
                case 24:return ProductShortcut_25;
                case 25:return ProductShortcut_26;
                case 26:return ProductShortcut_27;
                case 27:return ProductShortcut_28;
                case 28:return ProductShortcut_29;
                case 29:return ProductShortcut_30;
                case 30:return ProductShortcut_31;
                case 31:return ProductShortcut_32;
                case 32:return ProductShortcut_33;
                case 33:return ProductShortcut_34;
                case 34:return ProductShortcut_35;
                default:return ProductShortcut_1;
            }
        }        
        private void LoadBascula()
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
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserKeys));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ProductShortcut_1 = new System.Windows.Forms.Label();
            this.ProductShortcut_2 = new System.Windows.Forms.Label();
            this.ProductShortcut_3 = new System.Windows.Forms.Label();
            this.baseDeDatosDataSet = new MainMenu.BaseDeDatosDataSet();
            this.productosTableAdapter = new MainMenu.BaseDeDatosDataSetTableAdapters.ProductosTableAdapter();
            this.prod_detalleTableAdapter1 = new MainMenu.BaseDeDatosDataSetTableAdapters.Prod_detalleTableAdapter();
            this.listView1 = new System.Windows.Forms.ListView();
            this.ProductShortcut_4 = new System.Windows.Forms.Label();
            this.ProductShortcut_5 = new System.Windows.Forms.Label();
            this.ProductShortcut_6 = new System.Windows.Forms.Label();
            this.ProductShortcut_7 = new System.Windows.Forms.Label();
            this.ProductShortcut_8 = new System.Windows.Forms.Label();
            this.ProductShortcut_9 = new System.Windows.Forms.Label();
            this.ProductShortcut_10 = new System.Windows.Forms.Label();
            this.ProductShortcut_11 = new System.Windows.Forms.Label();
            this.ProductShortcut_12 = new System.Windows.Forms.Label();
            this.ProductShortcut_13 = new System.Windows.Forms.Label();
            this.ProductShortcut_14 = new System.Windows.Forms.Label();
            this.ProductShortcut_15 = new System.Windows.Forms.Label();
            this.ProductShortcut_16 = new System.Windows.Forms.Label();
            this.ProductShortcut_17 = new System.Windows.Forms.Label();
            this.ProductShortcut_18 = new System.Windows.Forms.Label();
            this.ProductShortcut_19 = new System.Windows.Forms.Label();
            this.ProductShortcut_20 = new System.Windows.Forms.Label();
            this.ProductShortcut_21 = new System.Windows.Forms.Label();
            this.ProductShortcut_22 = new System.Windows.Forms.Label();
            this.ProductShortcut_23 = new System.Windows.Forms.Label();
            this.ProductShortcut_24 = new System.Windows.Forms.Label();
            this.ProductShortcut_25 = new System.Windows.Forms.Label();
            this.ProductShortcut_26 = new System.Windows.Forms.Label();
            this.ProductShortcut_27 = new System.Windows.Forms.Label();
            this.ProductShortcut_28 = new System.Windows.Forms.Label();
            this.ProductShortcut_29 = new System.Windows.Forms.Label();
            this.ProductShortcut_30 = new System.Windows.Forms.Label();
            this.ProductShortcut_31 = new System.Windows.Forms.Label();
            this.ProductShortcut_32 = new System.Windows.Forms.Label();
            this.ProductShortcut_33 = new System.Windows.Forms.Label();
            this.ProductShortcut_34 = new System.Windows.Forms.Label();
            this.ProductShortcut_35 = new System.Windows.Forms.Label();
            this.btn_ClearAll = new System.Windows.Forms.Button();
            this.toolStrip31 = new System.Windows.Forms.ToolStrip();
            this.StripGuardar = new System.Windows.Forms.ToolStripButton();
            this.StripEnviar = new System.Windows.Forms.ToolStripButton();
            this.StripCerrar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.lbl_indicator_visual_1 = new System.Windows.Forms.Label();
            this.lbl_indicator_visual_2 = new System.Windows.Forms.Label();
            this.lbl_indicator_1 = new System.Windows.Forms.Label();
            this.lbl_indicator_2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseDeDatosDataSet)).BeginInit();
            this.toolStrip31.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MainMenu.Properties.Resources.Teclado3;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // ProductShortcut_1
            // 
            this.ProductShortcut_1.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_1, "ProductShortcut_1");
            this.ProductShortcut_1.Name = "ProductShortcut_1";
            // 
            // ProductShortcut_2
            // 
            this.ProductShortcut_2.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_2, "ProductShortcut_2");
            this.ProductShortcut_2.Name = "ProductShortcut_2";
            // 
            // ProductShortcut_3
            // 
            this.ProductShortcut_3.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_3, "ProductShortcut_3");
            this.ProductShortcut_3.Name = "ProductShortcut_3";
            // 
            // baseDeDatosDataSet
            // 
            this.baseDeDatosDataSet.DataSetName = "BaseDeDatosDataSet";
            this.baseDeDatosDataSet.EnforceConstraints = false;
            this.baseDeDatosDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // productosTableAdapter
            // 
            this.productosTableAdapter.ClearBeforeFill = true;
            // 
            // prod_detalleTableAdapter1
            // 
            this.prod_detalleTableAdapter1.ClearBeforeFill = true;
            // 
            // listView1
            // 
            this.listView1.AllowDrop = true;
            resources.ApplyResources(this.listView1, "listView1");
            this.listView1.Name = "listView1";
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // ProductShortcut_4
            // 
            this.ProductShortcut_4.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_4, "ProductShortcut_4");
            this.ProductShortcut_4.Name = "ProductShortcut_4";
            // 
            // ProductShortcut_5
            // 
            this.ProductShortcut_5.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_5, "ProductShortcut_5");
            this.ProductShortcut_5.Name = "ProductShortcut_5";
            // 
            // ProductShortcut_6
            // 
            this.ProductShortcut_6.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_6, "ProductShortcut_6");
            this.ProductShortcut_6.Name = "ProductShortcut_6";
            // 
            // ProductShortcut_7
            // 
            this.ProductShortcut_7.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_7, "ProductShortcut_7");
            this.ProductShortcut_7.Name = "ProductShortcut_7";
            // 
            // ProductShortcut_8
            // 
            this.ProductShortcut_8.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_8, "ProductShortcut_8");
            this.ProductShortcut_8.Name = "ProductShortcut_8";
            // 
            // ProductShortcut_9
            // 
            this.ProductShortcut_9.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_9, "ProductShortcut_9");
            this.ProductShortcut_9.Name = "ProductShortcut_9";
            // 
            // ProductShortcut_10
            // 
            this.ProductShortcut_10.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_10, "ProductShortcut_10");
            this.ProductShortcut_10.Name = "ProductShortcut_10";
            // 
            // ProductShortcut_11
            // 
            this.ProductShortcut_11.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_11, "ProductShortcut_11");
            this.ProductShortcut_11.Name = "ProductShortcut_11";
            // 
            // ProductShortcut_12
            // 
            this.ProductShortcut_12.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_12, "ProductShortcut_12");
            this.ProductShortcut_12.Name = "ProductShortcut_12";
            // 
            // ProductShortcut_13
            // 
            this.ProductShortcut_13.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_13, "ProductShortcut_13");
            this.ProductShortcut_13.Name = "ProductShortcut_13";
            // 
            // ProductShortcut_14
            // 
            this.ProductShortcut_14.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_14, "ProductShortcut_14");
            this.ProductShortcut_14.Name = "ProductShortcut_14";
            // 
            // ProductShortcut_15
            // 
            this.ProductShortcut_15.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_15, "ProductShortcut_15");
            this.ProductShortcut_15.Name = "ProductShortcut_15";
            // 
            // ProductShortcut_16
            // 
            this.ProductShortcut_16.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_16, "ProductShortcut_16");
            this.ProductShortcut_16.Name = "ProductShortcut_16";
            // 
            // ProductShortcut_17
            // 
            this.ProductShortcut_17.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_17, "ProductShortcut_17");
            this.ProductShortcut_17.Name = "ProductShortcut_17";
            // 
            // ProductShortcut_18
            // 
            this.ProductShortcut_18.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_18, "ProductShortcut_18");
            this.ProductShortcut_18.Name = "ProductShortcut_18";
            // 
            // ProductShortcut_19
            // 
            this.ProductShortcut_19.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_19, "ProductShortcut_19");
            this.ProductShortcut_19.Name = "ProductShortcut_19";
            // 
            // ProductShortcut_20
            // 
            this.ProductShortcut_20.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_20, "ProductShortcut_20");
            this.ProductShortcut_20.Name = "ProductShortcut_20";
            // 
            // ProductShortcut_21
            // 
            this.ProductShortcut_21.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_21, "ProductShortcut_21");
            this.ProductShortcut_21.Name = "ProductShortcut_21";
            // 
            // ProductShortcut_22
            // 
            this.ProductShortcut_22.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_22, "ProductShortcut_22");
            this.ProductShortcut_22.Name = "ProductShortcut_22";
            // 
            // ProductShortcut_23
            // 
            this.ProductShortcut_23.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_23, "ProductShortcut_23");
            this.ProductShortcut_23.Name = "ProductShortcut_23";
            // 
            // ProductShortcut_24
            // 
            this.ProductShortcut_24.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_24, "ProductShortcut_24");
            this.ProductShortcut_24.Name = "ProductShortcut_24";
            // 
            // ProductShortcut_25
            // 
            this.ProductShortcut_25.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_25, "ProductShortcut_25");
            this.ProductShortcut_25.Name = "ProductShortcut_25";
            // 
            // ProductShortcut_26
            // 
            this.ProductShortcut_26.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_26, "ProductShortcut_26");
            this.ProductShortcut_26.Name = "ProductShortcut_26";
            // 
            // ProductShortcut_27
            // 
            this.ProductShortcut_27.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_27, "ProductShortcut_27");
            this.ProductShortcut_27.Name = "ProductShortcut_27";
            // 
            // ProductShortcut_28
            // 
            this.ProductShortcut_28.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_28, "ProductShortcut_28");
            this.ProductShortcut_28.Name = "ProductShortcut_28";
            // 
            // ProductShortcut_29
            // 
            this.ProductShortcut_29.AllowDrop = true;
            this.ProductShortcut_29.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            resources.ApplyResources(this.ProductShortcut_29, "ProductShortcut_29");
            this.ProductShortcut_29.Name = "ProductShortcut_29";
            // 
            // ProductShortcut_30
            // 
            this.ProductShortcut_30.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_30, "ProductShortcut_30");
            this.ProductShortcut_30.Name = "ProductShortcut_30";
            // 
            // ProductShortcut_31
            // 
            this.ProductShortcut_31.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_31, "ProductShortcut_31");
            this.ProductShortcut_31.Name = "ProductShortcut_31";
            // 
            // ProductShortcut_32
            // 
            this.ProductShortcut_32.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_32, "ProductShortcut_32");
            this.ProductShortcut_32.Name = "ProductShortcut_32";
            // 
            // ProductShortcut_33
            // 
            this.ProductShortcut_33.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_33, "ProductShortcut_33");
            this.ProductShortcut_33.Name = "ProductShortcut_33";
            // 
            // ProductShortcut_34
            // 
            this.ProductShortcut_34.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_34, "ProductShortcut_34");
            this.ProductShortcut_34.Name = "ProductShortcut_34";
            // 
            // ProductShortcut_35
            // 
            this.ProductShortcut_35.AllowDrop = true;
            resources.ApplyResources(this.ProductShortcut_35, "ProductShortcut_35");
            this.ProductShortcut_35.Name = "ProductShortcut_35";
            // 
            // btn_ClearAll
            // 
            resources.ApplyResources(this.btn_ClearAll, "btn_ClearAll");
            this.btn_ClearAll.Name = "btn_ClearAll";
            this.btn_ClearAll.UseVisualStyleBackColor = true;
            this.btn_ClearAll.Click += new System.EventHandler(this.btn_ClearAll_Click);
            // 
            // toolStrip31
            // 
            this.toolStrip31.BackColor = System.Drawing.Color.LightSteelBlue;
            this.toolStrip31.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip31.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip31.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StripGuardar,
            this.StripEnviar,
            this.StripCerrar,
            this.toolStripSeparator3,
            this.toolStripLabel1,
            this.toolStripLabel2,
            this.toolStripSeparator1,
            this.toolStripLabel3});
            resources.ApplyResources(this.toolStrip31, "toolStrip31");
            this.toolStrip31.Name = "toolStrip31";
            // 
            // StripGuardar
            // 
            this.StripGuardar.ForeColor = System.Drawing.Color.MidnightBlue;
            this.StripGuardar.Image = global::MainMenu.Properties.Resources.save3;
            this.StripGuardar.Name = "StripGuardar";
            resources.ApplyResources(this.StripGuardar, "StripGuardar");
            this.StripGuardar.Click += new System.EventHandler(this.StripGuardar_Click);
            // 
            // StripEnviar
            // 
            this.StripEnviar.ForeColor = System.Drawing.Color.MidnightBlue;
            this.StripEnviar.Image = global::MainMenu.Properties.Resources.connect;
            this.StripEnviar.Name = "StripEnviar";
            resources.ApplyResources(this.StripEnviar, "StripEnviar");
            this.StripEnviar.Click += new System.EventHandler(this.EnviarDatos_Click);
            // 
            // StripCerrar
            // 
            this.StripCerrar.ForeColor = System.Drawing.Color.MidnightBlue;
            this.StripCerrar.Image = global::MainMenu.Properties.Resources.cancelar;
            this.StripCerrar.Name = "StripCerrar";
            resources.ApplyResources(this.StripCerrar, "StripCerrar");
            this.StripCerrar.Click += new System.EventHandler(this.StripCerrar_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripLabel1.Name = "toolStripLabel1";
            resources.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.ForeColor = System.Drawing.Color.Blue;
            this.toolStripLabel2.Name = "toolStripLabel2";
            resources.ApplyResources(this.toolStripLabel2, "toolStripLabel2");
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.ForeColor = System.Drawing.Color.Blue;
            this.toolStripLabel3.Name = "toolStripLabel3";
            resources.ApplyResources(this.toolStripLabel3, "toolStripLabel3");
            // 
            // lbl_indicator_visual_1
            // 
            this.lbl_indicator_visual_1.AllowDrop = true;
            this.lbl_indicator_visual_1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            resources.ApplyResources(this.lbl_indicator_visual_1, "lbl_indicator_visual_1");
            this.lbl_indicator_visual_1.Name = "lbl_indicator_visual_1";
            // 
            // lbl_indicator_visual_2
            // 
            this.lbl_indicator_visual_2.AllowDrop = true;
            this.lbl_indicator_visual_2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            resources.ApplyResources(this.lbl_indicator_visual_2, "lbl_indicator_visual_2");
            this.lbl_indicator_visual_2.Name = "lbl_indicator_visual_2";
            // 
            // lbl_indicator_1
            // 
            resources.ApplyResources(this.lbl_indicator_1, "lbl_indicator_1");
            this.lbl_indicator_1.Name = "lbl_indicator_1";
            // 
            // lbl_indicator_2
            // 
            resources.ApplyResources(this.lbl_indicator_2, "lbl_indicator_2");
            this.lbl_indicator_2.Name = "lbl_indicator_2";
            // 
            // UserKeys
            // 
            this.Controls.Add(this.lbl_indicator_2);
            this.Controls.Add(this.lbl_indicator_1);
            this.Controls.Add(this.lbl_indicator_visual_2);
            this.Controls.Add(this.lbl_indicator_visual_1);
            this.Controls.Add(this.btn_ClearAll);
            this.Controls.Add(this.ProductShortcut_35);
            this.Controls.Add(this.ProductShortcut_34);
            this.Controls.Add(this.ProductShortcut_33);
            this.Controls.Add(this.ProductShortcut_32);
            this.Controls.Add(this.ProductShortcut_31);
            this.Controls.Add(this.ProductShortcut_30);
            this.Controls.Add(this.ProductShortcut_29);
            this.Controls.Add(this.ProductShortcut_28);
            this.Controls.Add(this.ProductShortcut_27);
            this.Controls.Add(this.ProductShortcut_26);
            this.Controls.Add(this.ProductShortcut_25);
            this.Controls.Add(this.ProductShortcut_24);
            this.Controls.Add(this.ProductShortcut_23);
            this.Controls.Add(this.ProductShortcut_22);
            this.Controls.Add(this.ProductShortcut_21);
            this.Controls.Add(this.ProductShortcut_20);
            this.Controls.Add(this.ProductShortcut_19);
            this.Controls.Add(this.ProductShortcut_18);
            this.Controls.Add(this.ProductShortcut_17);
            this.Controls.Add(this.ProductShortcut_16);
            this.Controls.Add(this.ProductShortcut_15);
            this.Controls.Add(this.ProductShortcut_14);
            this.Controls.Add(this.ProductShortcut_13);
            this.Controls.Add(this.ProductShortcut_12);
            this.Controls.Add(this.ProductShortcut_11);
            this.Controls.Add(this.ProductShortcut_10);
            this.Controls.Add(this.ProductShortcut_9);
            this.Controls.Add(this.ProductShortcut_8);
            this.Controls.Add(this.ProductShortcut_7);
            this.Controls.Add(this.ProductShortcut_6);
            this.Controls.Add(this.ProductShortcut_5);
            this.Controls.Add(this.ProductShortcut_4);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.ProductShortcut_3);
            this.Controls.Add(this.ProductShortcut_2);
            this.Controls.Add(this.ProductShortcut_1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "UserKeys";
            resources.ApplyResources(this, "$this");
            this.Load += new System.EventHandler(this.UserKeys_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.baseDeDatosDataSet)).EndInit();
            this.toolStrip31.ResumeLayout(false);
            this.toolStrip31.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private void StripCerrar_Click(object sender, EventArgs e)
        {
            ToolStripManager.RevertMerge("toolStrip3");
            this.Dispose();
        }
        private void Crear_Carpetas()
        {
            Carpetas dat = new Carpetas();
            Carpetas.numfold = Convert.ToInt32(dat.muestraAutoincrementoId());
            Carpetas.nomfold = "Nueva Carpeta";

            if (Carpetas.numfold > 0)
            {
                if (dat.Nuevo_Folder(Carpetas.numfold, Carpetas.nomfold, Convert.ToInt32(1), Convert.ToInt16(0)))
                {
                    DataRow dr = baseDeDatosDataSet.carpeta_detalle.NewRow();
                    dr.BeginEdit();
                    dr["id_bascula"] = Num_Bascula;
                    dr["id_grupo"] = Num_Grupo;
                    dr["ID"] = Carpetas.numfold;
                    dr["ID_padre"] = Convert.ToInt32(1);
                    dr["Nombre"] = Carpetas.nomfold;
                    dr["ruta"] = "";
                    dr.EndEdit();
                    carpeta_detalleTableAdapter.Update(dr);
                    baseDeDatosDataSet.carpeta_detalle.AcceptChanges();
                }
            }
        }
        private bool Enviar_DetalleCarpeta_Bascula(string direccionIP, ref Socket Cliente_bascula, ref ProgressContinue pro)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            int reg_leido = 0;
            int reg_envio = 0;
            int reg_total;
            string Msj_recibido;
            string Variable_frame;
            bool ERROR = false;

            DataRow[] DR = baseDeDatosDataSet.carpeta_detalle.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);

            Variable_frame = "";
            reg_total = DR.Length;

            pro.IniciaProcess(reg_total, Variable.SYS_MSJ[247, Variable.idioma] + " " + myCurrent.Nserie + "... ");  //"Enviando Detalle de Carpetas","Iniciando proceso");

            foreach (DataRow DR_Detail in DR)
            {
                Variable_frame = Variable_frame + Env.Genera_Trama_Carpeta_Detalle(DR_Detail);

                reg_leido++;
                if (reg_leido > 4)
                {
                    reg_envio = reg_envio + reg_leido;
                    Msj_recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo, "GA");
                    if (Msj_recibido != null)
                    {
                        pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[247, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                    }
                    else
                    {
                        ERROR = true;
                        break;
                    }
                    reg_leido = 0;
                    Variable_frame = "";

                }
            }
            if (Variable_frame.Length > 0 && reg_leido <= 4)
            {
                reg_envio = reg_envio + reg_leido;

                Msj_recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo, "GA");
                if (Msj_recibido != null)
                {
                    pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[247, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                }
                else
                {
                    ERROR = true;
                }
                reg_leido = 0;
                Variable_frame = "";
            }
            return ERROR;
        }
        private void EnviarDatos_Click(object sender, EventArgs e)
        {
            if (isSaved)
            {
                int BasculasActualizadas = 0;
                int NumeroDeBaculas = 0;

                try
                {
                    carpeta_detalleTableAdapter.Fill(baseDeDatosDataSet.carpeta_detalle);
                    prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);
                    ProgressContinue pro = new ProgressContinue();// se inicia el Progress
                    pro.IniciaProcess(Variable.SYS_MSJ[192, Variable.idioma]);
                    if (Num_Grupo != 0)

                    #region Envia a Grupo
                    {
                        for (int pos = 0; pos < myScale.Length; pos++)  //Num de Basculas en el grupo
                            if (myScale[pos].gpo == Num_Grupo) NumeroDeBaculas++;

                        for (int pos = 0; pos < myScale.Length; pos++)
                        {
                            if (myScale[pos].gpo == Num_Grupo)
                            {
                                myCurrent.ip = myScale[pos].ip;  //direccion ip de la bascula
                                myCurrent.idbas = myScale[pos].idbas;   //numero id de la bascula
                                myCurrent.Nserie = myScale[pos].nserie;  //numero de serie de la bascula                    
                                myCurrent.nombre = myScale[pos].nombre;  //mombre de la bascula 
                                myCurrent.gpo = myScale[pos].gpo;  //Grupo al que pertenece la bascula
                                myCurrent.BAUD = myScale[pos].baud;
                                myCurrent.COMM = myScale[pos].pto;
                                BasculasActualizadas++;

                                Form1.toolLabel.Text = Variable.SYS_MSJ[263, Variable.idioma]; // "Preparando Bascula.....";

                                if (myScale[pos].tipo == (int)ESTADO.tipoConexionesEnum.PKWIFI)
                                #region TCP
                                {
                                    Cliente_bascula = Cte.conectar(myCurrent.ip, 50036);
                                    if (Cliente_bascula != null)
                                    {

                                        string sComando = "XX" + (char)9 + (char)10;
                                        string Msj_recibido = Env.Command_Enviado(1, sComando, myCurrent.ip, ref Cliente_bascula, 0, 0, "bX");

                                        if (Msj_recibido != null)
                                        {
                                            Cursor.Current = Cursors.WaitCursor;

                                            if (Envia_Borrar_Asociacion(myCurrent.ip, ref Cliente_bascula))
                                            {
                                                if (Enviar_DetalleCarpeta_Bascula(myCurrent.ip, ref Cliente_bascula, ref pro))
                                                {
                                                    BasculasActualizadas--;
                                                    if (MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma] + " "
                                                        + myCurrent.Nserie + " " + Variable.SYS_MSJ[417, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma],
                                                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No) break;
                                                }
                                                if (Enviar_DetalleProducto_Bascula(myCurrent.ip, ref Cliente_bascula, ref pro))
                                                {
                                                    BasculasActualizadas--;
                                                    if (MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma] + " "
                                                        + myCurrent.Nserie + " " + Variable.SYS_MSJ[417, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma],
                                                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No) break;
                                                }
                                                //Cambiar_Estado_pendiente();
                                            }
                                        }
                                        Cte.desconectar(ref Cliente_bascula);
                                        Cursor.Current = Cursors.Default;
                                    }
                                    else
                                    {
                                        BasculasActualizadas--;
                                        if (MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma] + " "
                                            + myCurrent.Nserie + " " + Variable.SYS_MSJ[417, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma],
                                            MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No) break;
                                    }
                                }
                                #endregion
                                else
                                #region Serial
                                {
                                    //serialPort1 = new SerialPort();
                                    //if (SR.OpenPort(ref serialPort1, myCurrent.COMM, myCurrent.BAUD))
                                    //{
                                    //    SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)10);
                                    //    Envia_Borrar_Asociacion(ref serialPort1);
                                    //    Enviar_DetalleCarpeta_Bascula(ref serialPort1, ref pro);
                                    //    Enviar_DetalleProducto_Bascula(ref serialPort1, ref pro);
                                    //    Cambiar_Estado_pendiente();
                                    //    SR.SendCOMSerial(ref serialPort1, "dXXX" + (char)10);
                                    //    SR.ClosePort(ref serialPort1);
                                    //}
                                }
                                #endregion
                            }
                        }
                    }
                    #endregion
                    else
                    #region Envia a Bascula
                    {
                        for (int pos = 0; pos < myScale.Length; pos++)  //Num de Basculas con el mismo Num_Bascula
                            if (myScale[pos].idbas == Num_Bascula) NumeroDeBaculas++;

                        for (int pos = 0; pos < myScale.Length; pos++)
                        {
                            if (myScale[pos].idbas == Num_Bascula)
                            {
                                myCurrent.ip = myScale[pos].ip;  //direccion ip de la bascula
                                myCurrent.idbas = myScale[pos].idbas;   //numero id de la bascula
                                myCurrent.Nserie = myScale[pos].nserie;  //numero de serie de la bascula                    
                                myCurrent.nombre = myScale[pos].nombre;  //mombre de la bascula 
                                myCurrent.gpo = myScale[pos].gpo;  //Grupo al que pertenece la bascula
                                myCurrent.BAUD = myScale[pos].baud;
                                myCurrent.COMM = myScale[pos].pto;
                                BasculasActualizadas++;

                                Form1.toolLabel.Text = Variable.SYS_MSJ[263, Variable.idioma];  // "Preparando Bascula.....";

                                if (myScale[pos].tipo == (int)ESTADO.tipoConexionesEnum.PKWIFI)
                                #region TCP
                                {
                                    Cliente_bascula = Cte.conectar(myCurrent.ip, 50036);
                                    if (Cliente_bascula != null)
                                    {

                                        string sComando = "XX" + (char)9 + (char)10;
                                        string Msj_recibido = Env.Command_Enviado(1, sComando, myCurrent.ip, ref Cliente_bascula, 0, 0, "bX");

                                        if (Msj_recibido != null)
                                        {

                                            Cursor.Current = Cursors.WaitCursor;

                                            if (Envia_Borrar_Asociacion(myCurrent.ip, ref Cliente_bascula))
                                            {
                                                if (Enviar_DetalleProducto_Bascula(myCurrent.ip, ref Cliente_bascula, ref pro))
                                                {
                                                    BasculasActualizadas--;
                                                    MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma]
                                                        + " " + myCurrent.Nserie + ".", Variable.SYS_MSJ[42, Variable.idioma],
                                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                                    break;
                                                }
                                                Cambiar_Estado_pendiente();
                                            }

                                        }
                                        Cte.desconectar(ref Cliente_bascula);

                                        Cursor.Current = Cursors.Default;
                                    }
                                    else
                                    {
                                        BasculasActualizadas--;
                                        MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma]
                                            + " " + myCurrent.Nserie + ".", Variable.SYS_MSJ[42, Variable.idioma],
                                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        break;
                                    }
                                }
                                #endregion
                             break;
                            }
                        }
                    }
                    #endregion

                    pro.TerminaProcess();
                    Thread.Sleep(500);
                    Form1.toolLabel.Text = Variable.SYS_MSJ[193, Variable.idioma];

                    MessageBox.Show(this, Variable.SYS_MSJ[418, Variable.idioma] + " " + BasculasActualizadas + " "
                        + Variable.SYS_MSJ[419, Variable.idioma] + " " + NumeroDeBaculas,
                        Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, Variable.SYS_MSJ[381, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
            //MessageBox.Show(this, Variable.SYS_MSJ[418, Variable.idioma] , Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show(this, "Aun no has guardado los cambios, por favor guarda los cambios", "ALERTA", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void StripGuardar_Click(object sender, EventArgs e)
        {
            Conec.CadenaSelect = "DELETE from Prod_detalle ";
            Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, "Prod_detalle");
            
            foreach (ProductoDTO item in TableRelationship)
                 Crear_DetalleProducto(Num_Bascula, Num_Grupo, 0, item);

            isSaved = true;//se indica que los datos han sido guardados con exito y estan listos para ser enviados.
            StripEnviar.Enabled = true;
            StripGuardar.Enabled = false;
            MessageBox.Show(this, "Los cambios se han guardado con exito..!!!", "ALERTA", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void UserKeys_Load(object sender, EventArgs e)
        {

            lbl_indicator_visual_1.BackColor = Color.Green;
            lbl_indicator_visual_2.BackColor = Color.OrangeRed;
            lbl_indicator_1.Text = Variable.SYS_MSJ[461, Variable.idioma];
            lbl_indicator_2.Text = Variable.SYS_MSJ[460, Variable.idioma];
            StripEnviar.Enabled = false;
            tool.AutoPopDelay = 7000;
            tool.InitialDelay = 300;
            tool.ReshowDelay = 500;
            tool.ShowAlways = true;
            fillProductos();
            ProductShortcut_1.AllowDrop = true;
            ProductShortcut_2.AllowDrop = true;
            ProductShortcut_3.AllowDrop = true;
            ProductShortcut_4.AllowDrop = true;
            ProductShortcut_5.AllowDrop = true;
            ProductShortcut_6.AllowDrop = true;
            ProductShortcut_7.AllowDrop = true;
            ProductShortcut_8.AllowDrop = true;
            ProductShortcut_9.AllowDrop = true;
            ProductShortcut_10.AllowDrop = true;
            ProductShortcut_11.AllowDrop = true;
            ProductShortcut_12.AllowDrop = true;
            ProductShortcut_13.AllowDrop = true;
            ProductShortcut_14.AllowDrop = true;
            ProductShortcut_15.AllowDrop = true;
            ProductShortcut_16.AllowDrop = true;
            ProductShortcut_17.AllowDrop = true;
            ProductShortcut_18.AllowDrop = true;
            ProductShortcut_19.AllowDrop = true;
            ProductShortcut_20.AllowDrop = true;
            ProductShortcut_21.AllowDrop = true;
            ProductShortcut_22.AllowDrop = true;
            ProductShortcut_23.AllowDrop = true;
            ProductShortcut_24.AllowDrop = true;
            ProductShortcut_25.AllowDrop = true;
            ProductShortcut_26.AllowDrop = true;
            ProductShortcut_27.AllowDrop = true;
            ProductShortcut_28.AllowDrop = true;
            ProductShortcut_29.AllowDrop = true;
            ProductShortcut_30.AllowDrop = true;
            ProductShortcut_31.AllowDrop = true;
            ProductShortcut_32.AllowDrop = true;
            ProductShortcut_33.AllowDrop = true;
            ProductShortcut_34.AllowDrop = true;
            ProductShortcut_35.AllowDrop = true;
            listView1.AllowDrop = true;
            listView1.MouseDown += listView1_MouseDown;
            listView1.DragOver += DragOver;
            ProductShortcut_1.DragEnter += DragEnter;
            ProductShortcut_1.DragDrop += DragDrop;
            ProductShortcut_2.DragEnter += DragEnter;
            ProductShortcut_2.DragDrop += DragDrop;
            ProductShortcut_3.DragEnter += DragEnter;
            ProductShortcut_3.DragDrop += DragDrop;
            ProductShortcut_4.DragEnter += DragEnter;
            ProductShortcut_4.DragDrop += DragDrop;
            ProductShortcut_5.DragEnter += DragEnter;
            ProductShortcut_5.DragDrop += DragDrop;
            ProductShortcut_6.DragEnter += DragEnter;
            ProductShortcut_6.DragDrop += DragDrop;
            ProductShortcut_7.DragEnter += DragEnter;
            ProductShortcut_7.DragDrop += DragDrop;
            ProductShortcut_8.DragEnter += DragEnter;
            ProductShortcut_8.DragDrop += DragDrop;
            ProductShortcut_9.DragEnter += DragEnter;
            ProductShortcut_9.DragDrop += DragDrop;
            ProductShortcut_10.DragEnter += DragEnter;
            ProductShortcut_10.DragDrop += DragDrop;
            ProductShortcut_11.DragEnter += DragEnter;
            ProductShortcut_11.DragDrop += DragDrop;
            ProductShortcut_12.DragEnter += DragEnter;
            ProductShortcut_12.DragDrop += DragDrop;
            ProductShortcut_13.DragEnter += DragEnter;
            ProductShortcut_13.DragDrop += DragDrop;
            ProductShortcut_14.DragEnter += DragEnter;
            ProductShortcut_14.DragDrop += DragDrop;
            ProductShortcut_15.DragEnter += DragEnter;
            ProductShortcut_15.DragDrop += DragDrop;
            ProductShortcut_16.DragEnter += DragEnter;
            ProductShortcut_16.DragDrop += DragDrop;
            ProductShortcut_17.DragEnter += DragEnter;
            ProductShortcut_17.DragDrop += DragDrop;
            ProductShortcut_18.DragEnter += DragEnter;
            ProductShortcut_18.DragDrop += DragDrop;
            ProductShortcut_19.DragEnter += DragEnter;
            ProductShortcut_19.DragDrop += DragDrop;
            ProductShortcut_20.DragEnter += DragEnter;
            ProductShortcut_20.DragDrop += DragDrop;
            ProductShortcut_21.DragEnter += DragEnter;
            ProductShortcut_21.DragDrop += DragDrop;
            ProductShortcut_22.DragEnter += DragEnter;
            ProductShortcut_22.DragDrop += DragDrop;
            ProductShortcut_23.DragEnter += DragEnter;
            ProductShortcut_23.DragDrop += DragDrop;
            ProductShortcut_24.DragEnter += DragEnter;
            ProductShortcut_24.DragDrop += DragDrop;
            ProductShortcut_25.DragEnter += DragEnter;
            ProductShortcut_25.DragDrop += DragDrop;
            ProductShortcut_26.DragEnter += DragEnter;
            ProductShortcut_26.DragDrop += DragDrop;
            ProductShortcut_27.DragEnter += DragEnter;
            ProductShortcut_27.DragDrop += DragDrop;
            ProductShortcut_28.DragEnter += DragEnter;
            ProductShortcut_28.DragDrop += DragDrop;
            ProductShortcut_29.DragEnter += DragEnter;
            ProductShortcut_29.DragDrop += DragDrop;
            ProductShortcut_30.DragEnter += DragEnter;
            ProductShortcut_30.DragDrop += DragDrop;
            ProductShortcut_31.DragEnter += DragEnter;
            ProductShortcut_31.DragDrop += DragDrop;
            ProductShortcut_32.DragEnter += DragEnter;
            ProductShortcut_32.DragDrop += DragDrop;
            ProductShortcut_33.DragEnter += DragEnter;
            ProductShortcut_33.DragDrop += DragDrop;
            ProductShortcut_34.DragEnter += DragEnter;
            ProductShortcut_34.DragDrop += DragDrop;
            ProductShortcut_35.DragEnter += DragEnter;
            ProductShortcut_35.DragDrop += DragDrop;
            LoadBascula();
            RefrescarMenus();
            DrawlingStatusButtons();
            configureEventsDrag();
            Crear_Carpetas();
            LoadLastKeys();
        }
        private bool isBotonAvailable(Label ctrl)
        {
            bool bandera = false;

            if (TableRelationship.Find(x => x.ControlAsignado == ctrl) != null)
            {
                bandera = false;
            }
            else
            {
                bandera=true;
            }

            return bandera;
        }
        private void DrawlingStatusButtons()
        {
            ProductShortcut_1.BackColor = (isBotonAvailable(ProductShortcut_1)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_2.BackColor = (isBotonAvailable(ProductShortcut_2)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_3.BackColor = (isBotonAvailable(ProductShortcut_3)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_4.BackColor = (isBotonAvailable(ProductShortcut_4)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_5.BackColor = (isBotonAvailable(ProductShortcut_5)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_6.BackColor = (isBotonAvailable(ProductShortcut_6)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_7.BackColor = (isBotonAvailable(ProductShortcut_7)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_8.BackColor = (isBotonAvailable(ProductShortcut_8)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_9.BackColor = (isBotonAvailable(ProductShortcut_9)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_10.BackColor = (isBotonAvailable(ProductShortcut_10)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_11.BackColor = (isBotonAvailable(ProductShortcut_11)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_12.BackColor = (isBotonAvailable(ProductShortcut_12)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_13.BackColor = (isBotonAvailable(ProductShortcut_13)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_14.BackColor = (isBotonAvailable(ProductShortcut_14)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_15.BackColor = (isBotonAvailable(ProductShortcut_15)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_16.BackColor = (isBotonAvailable(ProductShortcut_16)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_17.BackColor = (isBotonAvailable(ProductShortcut_17)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_18.BackColor = (isBotonAvailable(ProductShortcut_18)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_19.BackColor = (isBotonAvailable(ProductShortcut_19)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_20.BackColor = (isBotonAvailable(ProductShortcut_20)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_21.BackColor = (isBotonAvailable(ProductShortcut_21)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_22.BackColor = (isBotonAvailable(ProductShortcut_22)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_23.BackColor = (isBotonAvailable(ProductShortcut_23)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_24.BackColor = (isBotonAvailable(ProductShortcut_24)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_25.BackColor = (isBotonAvailable(ProductShortcut_25)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_26.BackColor = (isBotonAvailable(ProductShortcut_26)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_27.BackColor = (isBotonAvailable(ProductShortcut_27)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_28.BackColor = (isBotonAvailable(ProductShortcut_28)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_29.BackColor = (isBotonAvailable(ProductShortcut_29)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_30.BackColor = (isBotonAvailable(ProductShortcut_30)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_31.BackColor = (isBotonAvailable(ProductShortcut_31)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_32.BackColor = (isBotonAvailable(ProductShortcut_32)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_33.BackColor = (isBotonAvailable(ProductShortcut_33)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_34.BackColor = (isBotonAvailable(ProductShortcut_34)) ? Color.Green : Color.OrangeRed;
            ProductShortcut_35.BackColor = (isBotonAvailable(ProductShortcut_35)) ? Color.Green : Color.OrangeRed;
            setToolTip();
            RefrescarMenus();
        }
        private void RefrescarMenus()
        {
            if (Variable.idioma == 1){ProductShortcut_1.ContextMenu = (!isBotonAvailable(ProductShortcut_1)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event1)) }) : null;}
            else if (Variable.idioma == 0){ProductShortcut_1.ContextMenu = (!isBotonAvailable(ProductShortcut_1)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event1)) }) : null;}
            if (Variable.idioma == 1) { ProductShortcut_2.ContextMenu = (!isBotonAvailable(ProductShortcut_2)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event2)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_2.ContextMenu = (!isBotonAvailable(ProductShortcut_2)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event2)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_3.ContextMenu = (!isBotonAvailable(ProductShortcut_3)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event3)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_3.ContextMenu = (!isBotonAvailable(ProductShortcut_3)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event3)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_4.ContextMenu = (!isBotonAvailable(ProductShortcut_4)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event4)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_4.ContextMenu = (!isBotonAvailable(ProductShortcut_4)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event4)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_5.ContextMenu = (!isBotonAvailable(ProductShortcut_5)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event5)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_5.ContextMenu = (!isBotonAvailable(ProductShortcut_5)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event5)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_6.ContextMenu = (!isBotonAvailable(ProductShortcut_6)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event6)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_6.ContextMenu = (!isBotonAvailable(ProductShortcut_6)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event6)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_7.ContextMenu = (!isBotonAvailable(ProductShortcut_7)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event7)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_7.ContextMenu = (!isBotonAvailable(ProductShortcut_7)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event7)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_8.ContextMenu = (!isBotonAvailable(ProductShortcut_8)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event8)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_8.ContextMenu = (!isBotonAvailable(ProductShortcut_8)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event8)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_9.ContextMenu = (!isBotonAvailable(ProductShortcut_9)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event9)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_9.ContextMenu = (!isBotonAvailable(ProductShortcut_9)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event9)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_10.ContextMenu = (!isBotonAvailable(ProductShortcut_10)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event10)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_10.ContextMenu = (!isBotonAvailable(ProductShortcut_10)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event10)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_11.ContextMenu = (!isBotonAvailable(ProductShortcut_11)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event11)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_11.ContextMenu = (!isBotonAvailable(ProductShortcut_11)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event11)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_12.ContextMenu = (!isBotonAvailable(ProductShortcut_12)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event12)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_12.ContextMenu = (!isBotonAvailable(ProductShortcut_12)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event12)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_13.ContextMenu = (!isBotonAvailable(ProductShortcut_13)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event13)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_13.ContextMenu = (!isBotonAvailable(ProductShortcut_13)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event13)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_14.ContextMenu = (!isBotonAvailable(ProductShortcut_14)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event14)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_14.ContextMenu = (!isBotonAvailable(ProductShortcut_14)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event14)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_15.ContextMenu = (!isBotonAvailable(ProductShortcut_15)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event15)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_15.ContextMenu = (!isBotonAvailable(ProductShortcut_15)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event15)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_16.ContextMenu = (!isBotonAvailable(ProductShortcut_16)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event16)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_16.ContextMenu = (!isBotonAvailable(ProductShortcut_16)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event16)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_17.ContextMenu = (!isBotonAvailable(ProductShortcut_17)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event17)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_17.ContextMenu = (!isBotonAvailable(ProductShortcut_17)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event17)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_18.ContextMenu = (!isBotonAvailable(ProductShortcut_18)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event18)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_18.ContextMenu = (!isBotonAvailable(ProductShortcut_18)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event18)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_19.ContextMenu = (!isBotonAvailable(ProductShortcut_19)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event19)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_19.ContextMenu = (!isBotonAvailable(ProductShortcut_19)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event19)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_20.ContextMenu = (!isBotonAvailable(ProductShortcut_20)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event20)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_20.ContextMenu = (!isBotonAvailable(ProductShortcut_20)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event20)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_21.ContextMenu = (!isBotonAvailable(ProductShortcut_21)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event21)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_21.ContextMenu = (!isBotonAvailable(ProductShortcut_21)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event21)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_22.ContextMenu = (!isBotonAvailable(ProductShortcut_22)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event22)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_22.ContextMenu = (!isBotonAvailable(ProductShortcut_22)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event22)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_23.ContextMenu = (!isBotonAvailable(ProductShortcut_23)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event23)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_23.ContextMenu = (!isBotonAvailable(ProductShortcut_23)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event23)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_24.ContextMenu = (!isBotonAvailable(ProductShortcut_24)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event24)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_24.ContextMenu = (!isBotonAvailable(ProductShortcut_24)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event24)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_25.ContextMenu = (!isBotonAvailable(ProductShortcut_25)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event25)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_25.ContextMenu = (!isBotonAvailable(ProductShortcut_25)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event25)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_26.ContextMenu = (!isBotonAvailable(ProductShortcut_26)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event26)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_26.ContextMenu = (!isBotonAvailable(ProductShortcut_26)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event26)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_27.ContextMenu = (!isBotonAvailable(ProductShortcut_27)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event27)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_27.ContextMenu = (!isBotonAvailable(ProductShortcut_27)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event27)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_28.ContextMenu = (!isBotonAvailable(ProductShortcut_28)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event28)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_28.ContextMenu = (!isBotonAvailable(ProductShortcut_28)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event28)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_29.ContextMenu = (!isBotonAvailable(ProductShortcut_29)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event29)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_29.ContextMenu = (!isBotonAvailable(ProductShortcut_29)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event29)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_30.ContextMenu = (!isBotonAvailable(ProductShortcut_30)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event30)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_30.ContextMenu = (!isBotonAvailable(ProductShortcut_30)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event30)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_31.ContextMenu = (!isBotonAvailable(ProductShortcut_31)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event31)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_31.ContextMenu = (!isBotonAvailable(ProductShortcut_31)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event31)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_32.ContextMenu = (!isBotonAvailable(ProductShortcut_32)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event32)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_32.ContextMenu = (!isBotonAvailable(ProductShortcut_32)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event32)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_33.ContextMenu = (!isBotonAvailable(ProductShortcut_33)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event33)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_33.ContextMenu = (!isBotonAvailable(ProductShortcut_33)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event33)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_34.ContextMenu = (!isBotonAvailable(ProductShortcut_34)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event34)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_34.ContextMenu = (!isBotonAvailable(ProductShortcut_34)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event34)) }) : null; }
            if (Variable.idioma == 1) { ProductShortcut_35.ContextMenu = (!isBotonAvailable(ProductShortcut_35)) ? new ContextMenu(new MenuItem[] { new MenuItem("Remove", new EventHandler(Remove_Event35)) }) : null; }
            else if (Variable.idioma == 0) { ProductShortcut_35.ContextMenu = (!isBotonAvailable(ProductShortcut_35)) ? new ContextMenu(new MenuItem[] { new MenuItem("Eliminar", new EventHandler(Remove_Event35)) }) : null; }
      

            
        }
        public UserKeys()
        {
            InitializeComponent();
            
        }
        private void DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            this.listView1.DoDragDrop(this.listView1.SelectedIndices, DragDropEffects.Move);
        }
        private ProductoDTO getProducto(int index)
        {
            ListView.ListViewItemCollection item = listView1.Items;
                ProductoDTO producto = new ProductoDTO();
                producto.idProducto = Convert.ToInt32(item[index].SubItems[0].Text);
                producto.Nombre = Convert.ToString(item[index].SubItems[1].Text);
                producto.NoPLU = Convert.ToString(item[index].SubItems[2].Text);
                producto.precio = Convert.ToString(item[index].SubItems[3].Text);
                producto.Actualizado = Convert.ToString(item[index].SubItems[4].Text);
                return producto;                                     
        }
        private void RefillList()
        {

        }
        private void DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        private void fillProductos()
        {
            Conec.CadenaSelect = "SELECT * FROM Productos ORDER BY Codigo ASC";
            productosTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            productosTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);

            this.listView1.View = View.Details;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.LabelEdit = false;
            this.listView1.HideSelection = false;

            this.listView1.Columns.Add("Codigo", 80, HorizontalAlignment.Left);  //Codigo
            this.listView1.Columns.Add("Nombre", 230, HorizontalAlignment.Left);  //Nombre
            this.listView1.Columns.Add("NpPLU", 230, HorizontalAlignment.Left);  //Nombre
            this.listView1.Columns.Add("Precio", 80, HorizontalAlignment.Left);  //Nombre
            this.listView1.Columns.Add("Actualizado", 230, HorizontalAlignment.Left);  //Nombre

            foreach (var item in baseDeDatosDataSet.Productos.Select("", "Codigo ASC"))
            {
                if (!Convert.ToBoolean(item["borrado"].ToString()))
                {
                    ListViewItem lwitem = new ListViewItem(item["id_producto"].ToString());  //0
                    lwitem.SubItems.Add(item["Nombre"].ToString()); //1
                    lwitem.SubItems.Add(item["NoPlu"].ToString()); //3
                    lwitem.SubItems.Add(item["Precio"].ToString()); //4
                    lwitem.SubItems.Add(item["Actualizado"].ToString()); //5
                    this.listView1.Items.Add(lwitem);
                }
            }
        }
        private void AsignarTodos(object sender, EventArgs e)
        {
            TableRelationship.Clear();

            ProductoDTO p=getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p.ControlAsignado = ProductShortcut_1;
            TableRelationship.Add(p);
            ProductoDTO p2 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p2.ControlAsignado = ProductShortcut_2;
            TableRelationship.Add(p2);
            ProductoDTO p3 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p3.ControlAsignado = ProductShortcut_3;
            TableRelationship.Add(p3);
            ProductoDTO p4 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p4.ControlAsignado = ProductShortcut_4;
            TableRelationship.Add(p4);
            ProductoDTO p5 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p5.ControlAsignado = ProductShortcut_5;
            TableRelationship.Add(p5);
            ProductoDTO p6 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p6.ControlAsignado = ProductShortcut_6;
            TableRelationship.Add(p6);
            ProductoDTO p7 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p7.ControlAsignado = ProductShortcut_7;
            TableRelationship.Add(p7);
            ProductoDTO p8 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p8.ControlAsignado = ProductShortcut_8;
            TableRelationship.Add(p8);
            ProductoDTO p9 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p9.ControlAsignado = ProductShortcut_9;
            TableRelationship.Add(p9);
            ProductoDTO p10 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p10.ControlAsignado = ProductShortcut_10;
            TableRelationship.Add(p10);
            ProductoDTO p11 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p11.ControlAsignado = ProductShortcut_11;
            TableRelationship.Add(p11);
            ProductoDTO p12 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p12.ControlAsignado = ProductShortcut_12;
            TableRelationship.Add(p12);
            ProductoDTO p13 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p13.ControlAsignado = ProductShortcut_13;
            TableRelationship.Add(p13);
            ProductoDTO p14 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p14.ControlAsignado = ProductShortcut_14;
            TableRelationship.Add(p14);
            ProductoDTO p15 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p15.ControlAsignado = ProductShortcut_15;
            TableRelationship.Add(p15);
            ProductoDTO p16 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p16.ControlAsignado = ProductShortcut_16;
            TableRelationship.Add(p16);
            ProductoDTO p17 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p17.ControlAsignado = ProductShortcut_17;
            TableRelationship.Add(p17);
            ProductoDTO p18 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p18.ControlAsignado = ProductShortcut_18;
            TableRelationship.Add(p18);
            ProductoDTO p19 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p19.ControlAsignado = ProductShortcut_19;
            TableRelationship.Add(p19);
            ProductoDTO p20 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p20.ControlAsignado = ProductShortcut_20;
            TableRelationship.Add(p20);
            ProductoDTO p21 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p21.ControlAsignado = ProductShortcut_21;
            TableRelationship.Add(p21);
            ProductoDTO p22 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p22.ControlAsignado = ProductShortcut_22;
            TableRelationship.Add(p22);
            ProductoDTO p23 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p23.ControlAsignado = ProductShortcut_23;
            TableRelationship.Add(p23);
            ProductoDTO p24 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p24.ControlAsignado = ProductShortcut_24;
            TableRelationship.Add(p24);
            ProductoDTO p25 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p25.ControlAsignado = ProductShortcut_25;
            TableRelationship.Add(p25);
            ProductoDTO p26 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p26.ControlAsignado = ProductShortcut_26;
            TableRelationship.Add(p26);
            ProductoDTO p27 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p27.ControlAsignado = ProductShortcut_27;
            TableRelationship.Add(p27);
            ProductoDTO p28 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p28.ControlAsignado = ProductShortcut_28;
            TableRelationship.Add(p28);
            ProductoDTO p29 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p29.ControlAsignado = ProductShortcut_29;
            TableRelationship.Add(p29);
            ProductoDTO p30 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p30.ControlAsignado = ProductShortcut_30;
            TableRelationship.Add(p30);
            ProductoDTO p31 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p31.ControlAsignado = ProductShortcut_31;
            TableRelationship.Add(p31);
            ProductoDTO p32 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p32.ControlAsignado = ProductShortcut_32;
            TableRelationship.Add(p32);
            ProductoDTO p33 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p33.ControlAsignado = ProductShortcut_33;
            TableRelationship.Add(p33);
            ProductoDTO p34 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p34.ControlAsignado = ProductShortcut_34;
            TableRelationship.Add(p34);
            ProductoDTO p35 = getProducto(Convert.ToInt32(((ListView.SelectedIndexCollection)listView1.SelectedIndices)[0]));
            p35.ControlAsignado = ProductShortcut_35;
            TableRelationship.Add(p35);

            
            DrawlingStatusButtons();
        }
        private void DragDrop(object sender, DragEventArgs e)
        {
            ListView.SelectedIndexCollection producto = (ListView.SelectedIndexCollection)e.Data.GetData(typeof(ListView.SelectedIndexCollection));

            if (producto != null)
            {
                try
                {
                    index = Convert.ToInt32(producto[0]);
                    ProductoDTO _producto = getProducto(index);
                    _producto.ControlAsignado = (Label)sender;

                    if (isBotonAvailable((Label)sender))
                    {
                        TableRelationship.Add(_producto);
                    }
                    else
                    {
                        TableRelationship.Remove(TableRelationship.Find(x => x.ControlAsignado == (Label)sender));
                        TableRelationship.Add(_producto);
                    }
                }
                catch (Exception ex)
                {

                }         
            }
            else 
            {
                ProductoDTO x_producto = (ProductoDTO)e.Data.GetData(typeof(ProductoDTO));
                if (x_producto != null)
                {
                 TableRelationship.Remove(x_producto);
                 x_producto.ControlAsignado = (Label)sender;
                 TableRelationship.Add(x_producto);
                }
            }           
            DrawlingStatusButtons();
        }
        private void removeFromKey(ProductoDTO ProductoToRemove)
        {
            if (ProductoToRemove != null)
            {
                ListViewItem lwitem = new ListViewItem(ProductoToRemove.idProducto.ToString());  //0
                lwitem.SubItems.Add(ProductoToRemove.Nombre.ToString()); //1
                lwitem.SubItems.Add(ProductoToRemove.NoPLU.ToString()); //3
                lwitem.SubItems.Add(ProductoToRemove.precio.ToString()); //4
                lwitem.SubItems.Add(ProductoToRemove.Actualizado.ToString()); //5
                //this.listView1.Items.Add(lwitem);//Agregar de nuevo a la lista
                TableRelationship.Remove(ProductoToRemove);//Quitar de la tabla el producto que ya estaba en el boton designado
            }
        }
        private void configureEventsDrag()
        {
            ProductShortcut_1.MouseDown += ProductShortcut_1_MouseDown;
            ProductShortcut_1.DragOver += DragOver;
            ProductShortcut_2.MouseDown +=ProductShortcut_2_MouseDown;
            ProductShortcut_2.DragOver += DragOver;
            ProductShortcut_3.MouseDown +=ProductShortcut_3_MouseDown;
            ProductShortcut_3.DragOver += DragOver;
            ProductShortcut_4.MouseDown +=ProductShortcut_4_MouseDown;
            ProductShortcut_4.DragOver += DragOver;
            ProductShortcut_5.MouseDown +=ProductShortcut_5_MouseDown;
            ProductShortcut_5.DragOver += DragOver;
            ProductShortcut_6.MouseDown +=ProductShortcut_6_MouseDown;
            ProductShortcut_6.DragOver += DragOver;
            ProductShortcut_7.MouseDown +=ProductShortcut_7_MouseDown;
            ProductShortcut_7.DragOver += DragOver;
            ProductShortcut_8.MouseDown +=ProductShortcut_8_MouseDown;
            ProductShortcut_8.DragOver += DragOver;
            ProductShortcut_9.MouseDown +=ProductShortcut_9_MouseDown;
            ProductShortcut_9.DragOver += DragOver;
            ProductShortcut_10.MouseDown +=ProductShortcut_10_MouseDown;
            ProductShortcut_10.DragOver += DragOver;
            ProductShortcut_11.MouseDown +=ProductShortcut_11_MouseDown;
            ProductShortcut_11.DragOver += DragOver;
            ProductShortcut_12.MouseDown +=ProductShortcut_12_MouseDown;
            ProductShortcut_12.DragOver += DragOver;
            ProductShortcut_13.MouseDown +=ProductShortcut_13_MouseDown;
            ProductShortcut_13.DragOver += DragOver;
            ProductShortcut_14.MouseDown +=ProductShortcut_14_MouseDown;
            ProductShortcut_14.DragOver += DragOver;
            ProductShortcut_15.MouseDown +=ProductShortcut_15_MouseDown;
            ProductShortcut_15.DragOver += DragOver;
            ProductShortcut_16.MouseDown +=ProductShortcut_16_MouseDown;
            ProductShortcut_16.DragOver += DragOver;
            ProductShortcut_17.MouseDown +=ProductShortcut_17_MouseDown;
            ProductShortcut_17.DragOver += DragOver;
            ProductShortcut_18.MouseDown +=ProductShortcut_18_MouseDown;
            ProductShortcut_18.DragOver += DragOver;
            ProductShortcut_19.MouseDown +=ProductShortcut_19_MouseDown;
            ProductShortcut_19.DragOver += DragOver;
            ProductShortcut_20.MouseDown +=ProductShortcut_20_MouseDown;
            ProductShortcut_21.DragOver += DragOver;
            ProductShortcut_21.MouseDown +=ProductShortcut_21_MouseDown;
            ProductShortcut_22.DragOver += DragOver;
            ProductShortcut_22.MouseDown +=ProductShortcut_22_MouseDown;
            ProductShortcut_23.DragOver += DragOver;
            ProductShortcut_23.MouseDown +=ProductShortcut_23_MouseDown;
            ProductShortcut_24.DragOver += DragOver;
            ProductShortcut_24.MouseDown +=ProductShortcut_24_MouseDown;
            ProductShortcut_25.DragOver += DragOver;
            ProductShortcut_25.MouseDown +=ProductShortcut_25_MouseDown;
            ProductShortcut_26.DragOver += DragOver;
            ProductShortcut_26.MouseDown +=ProductShortcut_26_MouseDown;
            ProductShortcut_27.DragOver += DragOver;
            ProductShortcut_27.MouseDown +=ProductShortcut_27_MouseDown;
            ProductShortcut_28.DragOver += DragOver;
            ProductShortcut_28.MouseDown +=ProductShortcut_28_MouseDown;
            ProductShortcut_29.DragOver += DragOver;
            ProductShortcut_29.MouseDown +=ProductShortcut_29_MouseDown;
            ProductShortcut_30.DragOver += DragOver;
            ProductShortcut_30.MouseDown +=ProductShortcut_30_MouseDown;
            ProductShortcut_31.DragOver += DragOver;
            ProductShortcut_31.MouseDown +=ProductShortcut_31_MouseDown;
            ProductShortcut_32.DragOver += DragOver;
            ProductShortcut_32.MouseDown +=ProductShortcut_32_MouseDown;
            ProductShortcut_33.DragOver += DragOver;
            ProductShortcut_33.MouseDown +=ProductShortcut_33_MouseDown;
            ProductShortcut_34.DragOver += DragOver;
            ProductShortcut_34.MouseDown +=ProductShortcut_34_MouseDown;
            ProductShortcut_35.DragOver += DragOver;
            ProductShortcut_35.MouseDown += ProductShortcut_35_MouseDown;
            
        }
        private string getDescriptionToolTip(Label ctrl)
        {
            StringBuilder sr = new StringBuilder();
            var item = TableRelationship.Find(x => x.ControlAsignado == ctrl); 
            if (item != null)
            {
                if (Variable.idioma == 1)
                {
                    //sr.AppendLine("Code: [" + item.idProducto + "]");
                    sr.AppendLine("NoPLU: [" + item.NoPLU + "]");
                    sr.AppendLine("Name: [" + item.Nombre + "]");
                    sr.AppendLine("Price: [" + item.precio + "]");
                    sr.AppendLine("Updated: [" + item.Actualizado + "]");
                }
                else if (Variable.idioma == 0)
                {
                    //sr.AppendLine("Codigo: [" + item.idProducto + "]");
                    sr.AppendLine("NoPLU: [" + item.NoPLU + "]");
                    sr.AppendLine("Nombre: [" + item.Nombre + "]");
                    sr.AppendLine("Precio: [" + item.precio + "]");
                    sr.AppendLine("Actualizado: [" + item.Actualizado + "]");
                }
                
            }
            return sr.ToString();
        }
        private void setToolTip()
        {
            tool.RemoveAll();
            if (!isBotonAvailable(ProductShortcut_1)) tool.SetToolTip(ProductShortcut_1, getDescriptionToolTip(ProductShortcut_1));
            if (!isBotonAvailable(ProductShortcut_2)) tool.SetToolTip(ProductShortcut_2, getDescriptionToolTip(ProductShortcut_2));
            if (!isBotonAvailable(ProductShortcut_3)) tool.SetToolTip(ProductShortcut_3, getDescriptionToolTip(ProductShortcut_3));
            if (!isBotonAvailable(ProductShortcut_4)) tool.SetToolTip(ProductShortcut_4, getDescriptionToolTip(ProductShortcut_4));
            if (!isBotonAvailable(ProductShortcut_5)) tool.SetToolTip(ProductShortcut_5, getDescriptionToolTip(ProductShortcut_5));
            if (!isBotonAvailable(ProductShortcut_6)) tool.SetToolTip(ProductShortcut_6, getDescriptionToolTip(ProductShortcut_6));
            if (!isBotonAvailable(ProductShortcut_7)) tool.SetToolTip(ProductShortcut_7, getDescriptionToolTip(ProductShortcut_7));
            if (!isBotonAvailable(ProductShortcut_8)) tool.SetToolTip(ProductShortcut_8, getDescriptionToolTip(ProductShortcut_8));
            if (!isBotonAvailable(ProductShortcut_9)) tool.SetToolTip(ProductShortcut_9, getDescriptionToolTip(ProductShortcut_9));
            if (!isBotonAvailable(ProductShortcut_10)) tool.SetToolTip(ProductShortcut_10, getDescriptionToolTip(ProductShortcut_10));
            if (!isBotonAvailable(ProductShortcut_11)) tool.SetToolTip(ProductShortcut_11, getDescriptionToolTip(ProductShortcut_11));
            if (!isBotonAvailable(ProductShortcut_12)) tool.SetToolTip(ProductShortcut_12, getDescriptionToolTip(ProductShortcut_12));
            if (!isBotonAvailable(ProductShortcut_13)) tool.SetToolTip(ProductShortcut_13, getDescriptionToolTip(ProductShortcut_13));
            if (!isBotonAvailable(ProductShortcut_14)) tool.SetToolTip(ProductShortcut_14, getDescriptionToolTip(ProductShortcut_14));
            if (!isBotonAvailable(ProductShortcut_15)) tool.SetToolTip(ProductShortcut_15, getDescriptionToolTip(ProductShortcut_15));
            if (!isBotonAvailable(ProductShortcut_16)) tool.SetToolTip(ProductShortcut_16, getDescriptionToolTip(ProductShortcut_16));
            if (!isBotonAvailable(ProductShortcut_17)) tool.SetToolTip(ProductShortcut_17, getDescriptionToolTip(ProductShortcut_17));
            if (!isBotonAvailable(ProductShortcut_18)) tool.SetToolTip(ProductShortcut_18, getDescriptionToolTip(ProductShortcut_18));
            if (!isBotonAvailable(ProductShortcut_19)) tool.SetToolTip(ProductShortcut_19, getDescriptionToolTip(ProductShortcut_19));
            if (!isBotonAvailable(ProductShortcut_20)) tool.SetToolTip(ProductShortcut_20, getDescriptionToolTip(ProductShortcut_20));
            if (!isBotonAvailable(ProductShortcut_21)) tool.SetToolTip(ProductShortcut_21, getDescriptionToolTip(ProductShortcut_21));
            if (!isBotonAvailable(ProductShortcut_22)) tool.SetToolTip(ProductShortcut_22, getDescriptionToolTip(ProductShortcut_22));
            if (!isBotonAvailable(ProductShortcut_23)) tool.SetToolTip(ProductShortcut_23, getDescriptionToolTip(ProductShortcut_23));
            if (!isBotonAvailable(ProductShortcut_24)) tool.SetToolTip(ProductShortcut_24, getDescriptionToolTip(ProductShortcut_24));
            if (!isBotonAvailable(ProductShortcut_25)) tool.SetToolTip(ProductShortcut_25, getDescriptionToolTip(ProductShortcut_25));
            if (!isBotonAvailable(ProductShortcut_26)) tool.SetToolTip(ProductShortcut_26, getDescriptionToolTip(ProductShortcut_26));
            if (!isBotonAvailable(ProductShortcut_27)) tool.SetToolTip(ProductShortcut_27, getDescriptionToolTip(ProductShortcut_27));
            if (!isBotonAvailable(ProductShortcut_28)) tool.SetToolTip(ProductShortcut_28, getDescriptionToolTip(ProductShortcut_28));
            if (!isBotonAvailable(ProductShortcut_29)) tool.SetToolTip(ProductShortcut_29, getDescriptionToolTip(ProductShortcut_29));
            if (!isBotonAvailable(ProductShortcut_30)) tool.SetToolTip(ProductShortcut_30, getDescriptionToolTip(ProductShortcut_30));
            if (!isBotonAvailable(ProductShortcut_31)) tool.SetToolTip(ProductShortcut_31, getDescriptionToolTip(ProductShortcut_31));
            if (!isBotonAvailable(ProductShortcut_32)) tool.SetToolTip(ProductShortcut_32, getDescriptionToolTip(ProductShortcut_32));
            if (!isBotonAvailable(ProductShortcut_33)) tool.SetToolTip(ProductShortcut_33, getDescriptionToolTip(ProductShortcut_33));
            if (!isBotonAvailable(ProductShortcut_34)) tool.SetToolTip(ProductShortcut_34, getDescriptionToolTip(ProductShortcut_34));
            if (!isBotonAvailable(ProductShortcut_35)) tool.SetToolTip(ProductShortcut_35, getDescriptionToolTip(ProductShortcut_35));
        }
        #region eventos de mouse
        void ProductShortcut_35_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_35;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_34_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_34;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_33_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_33;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_32_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_32;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_31_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_31;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_30_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_30;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_29_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_29;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_28_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_28;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_27_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_27;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_26_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_26;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_25_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_25;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_24_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_24;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_23_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_23;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_22_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_22;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_21_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_21;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_20_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_20;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_19_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_19;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_18_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_18;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_17_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_17;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_16_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_16;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_15_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_15;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_14_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_14;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_13_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_13;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_12_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_12;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_11_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_11;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_10_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_10;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_9_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_9;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_8_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_8;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_7_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_7;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_6_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_6;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_5_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_5;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_4_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_4;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_3;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                var boton = ProductShortcut_2;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        void ProductShortcut_1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var boton = ProductShortcut_1;
                if (!isBotonAvailable(boton))
                {
                    boton.DoDragDrop(TableRelationship.Find(x => x.ControlAsignado == boton), DragDropEffects.Move);
                }
            }
        }
        #endregion
        #region Eventos para RemoverDatos
        void Remove_Event1(object sender, EventArgs e)  { if (!isBotonAvailable(ProductShortcut_1))  removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_1)); DrawlingStatusButtons(); }
        void Remove_Event2(object sender, EventArgs e)  { if (!isBotonAvailable(ProductShortcut_2))  removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_2)); DrawlingStatusButtons(); }
        void Remove_Event3(object sender, EventArgs e)  { if (!isBotonAvailable(ProductShortcut_3))  removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_3)); DrawlingStatusButtons(); }
        void Remove_Event4(object sender, EventArgs e)  { if (!isBotonAvailable(ProductShortcut_4))  removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_4)); DrawlingStatusButtons(); }
        void Remove_Event5(object sender, EventArgs e)  { if (!isBotonAvailable(ProductShortcut_5))  removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_5)); DrawlingStatusButtons(); }
        void Remove_Event6(object sender, EventArgs e)  { if (!isBotonAvailable(ProductShortcut_6))  removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_6)); DrawlingStatusButtons(); }
        void Remove_Event7(object sender, EventArgs e)  { if (!isBotonAvailable(ProductShortcut_7))  removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_7)); DrawlingStatusButtons(); }
        void Remove_Event8(object sender, EventArgs e)  { if (!isBotonAvailable(ProductShortcut_8))  removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_8)); DrawlingStatusButtons(); }
        void Remove_Event9(object sender, EventArgs e)  { if (!isBotonAvailable(ProductShortcut_9))  removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_9)); DrawlingStatusButtons(); }
        void Remove_Event10(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_10))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_10)); DrawlingStatusButtons(); }
        void Remove_Event11(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_11))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_11)); DrawlingStatusButtons(); }
        void Remove_Event12(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_12))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_12)); DrawlingStatusButtons(); }
        void Remove_Event13(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_13))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_13)); DrawlingStatusButtons(); }
        void Remove_Event14(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_14))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_14)); DrawlingStatusButtons(); }
        void Remove_Event15(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_15))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_15)); DrawlingStatusButtons(); }
        void Remove_Event16(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_16))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_16)); DrawlingStatusButtons(); }
        void Remove_Event17(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_17))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_17)); DrawlingStatusButtons(); }
        void Remove_Event18(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_18))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_18)); DrawlingStatusButtons(); }
        void Remove_Event19(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_19))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_19)); DrawlingStatusButtons(); }
        void Remove_Event20(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_20))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_20)); DrawlingStatusButtons(); }
        void Remove_Event21(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_21))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_21)); DrawlingStatusButtons(); }
        void Remove_Event22(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_22))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_22)); DrawlingStatusButtons(); }
        void Remove_Event23(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_23))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_23)); DrawlingStatusButtons(); }
        void Remove_Event24(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_24))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_24)); DrawlingStatusButtons(); }
        void Remove_Event25(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_25))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_25)); DrawlingStatusButtons(); }
        void Remove_Event26(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_26))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_26)); DrawlingStatusButtons(); }
        void Remove_Event27(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_27))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_27)); DrawlingStatusButtons(); }
        void Remove_Event28(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_28))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_28)); DrawlingStatusButtons(); }
        void Remove_Event29(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_29))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_29)); DrawlingStatusButtons(); }
        void Remove_Event30(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_30))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_30)); DrawlingStatusButtons(); }
        void Remove_Event31(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_31))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_31)); DrawlingStatusButtons(); }
        void Remove_Event32(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_32))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_32)); DrawlingStatusButtons(); }
        void Remove_Event33(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_33))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_33)); DrawlingStatusButtons(); }
        void Remove_Event34(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_34))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_34)); DrawlingStatusButtons(); }
        void Remove_Event35(object sender, EventArgs e) { if (!isBotonAvailable(ProductShortcut_35))removeFromKey(TableRelationship.Find(x => x.ControlAsignado == ProductShortcut_35)); DrawlingStatusButtons(); }
        #endregion
        private void btn_ClearAll_Click(object sender, EventArgs e)
        {
            TableRelationship.Clear();
            DrawlingStatusButtons(); 
        }
    }


    public class ProductoDTO
    {
        int pos;
        public int idProducto { set; get; }
        public string Nombre { set; get; }
        public string NoPLU { set; get; }
        public string precio { set; get; }
        public string Actualizado { set; get; }
        public Label ControlAsignado { set; get; }
        public int Position { set { pos = value; } get { return getPosition(this); } }
        private int getPosition(ProductoDTO producto)
        {
            int res = -1;

            if      (producto.ControlAsignado.Name.Equals("ProductShortcut_1")) { res = 0; }
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_2")) { res = 1; }
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_3")) { res = 2; }
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_4")) { res = 3; }
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_5")) { res = 4; }
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_6")) { res = 5; }
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_7")) { res = 6; }
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_8")) { res = 7; }
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_9")) { res = 8; }
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_10")){ res = 9; }
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_11")){ res = 10;}
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_12")){ res = 11;}
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_13")){ res = 12;}
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_14")){ res = 13;}
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_15")){ res = 14;}
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_16")){ res = 15;}
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_17")){ res = 16;}
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_18")){ res = 17;}
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_19")){ res = 18;}
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_20")){ res = 19;}
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_21")){ res = 20;}
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_22")){ res = 21;}
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_23")){ res = 22;}
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_24")){ res = 23;}
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_25")){ res = 24;}
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_26")){ res = 25;}
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_27")){ res = 26;}
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_28")){ res = 27;}
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_29")){ res = 28;}
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_30")){ res = 29;}
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_31")){ res = 30;}
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_32")){ res = 31;}
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_33")){ res = 32;}
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_34")){ res = 33;}
            else if (producto.ControlAsignado.Name.Equals("ProductShortcut_35")){ res = 34;}
            return res;
        }
    }



}


