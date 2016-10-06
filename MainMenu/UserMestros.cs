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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using TorreyTransfer;
using TreeViewBound;

namespace MainMenu
{


    /// <summary>
    /// remove these controls.
    /// StripImagen
    /// StripPublicidad
    /// </summary>
    public partial class UserMaestros : UserControl
    {
        public Int32 Num_Grupo;
        public Int32 Num_Bascula;
        public String Nombre_Select;
        
        Variable.lgrupo[] myGrupo;
        Variable.lbasc[] myScale;
        Variable.nodo_actual myCurrent;

        ESTADO.botonesEnvioDato DatosEnviado = new ESTADO.botonesEnvioDato();
        ADOutil Conec = new ADOutil();
        Conexion Cte = new Conexion();
        ArrayList Dato_Nuevo = new ArrayList();
        Socket Cliente_bascula = null;
        Envia_Dato Env = new Envia_Dato();
        Serial SR = new Serial();

        List<Int32> LidBorr;
        List<Int32> LidPend;
        List<Int32> LidInfo;
        List<Int32> LidMens;
        List<Int32> LidOfer;
        List<Int32> LidVend;

        BaseDeDatosDataSetTableAdapters.Prod_detalleTableAdapter prod_detalleTableAdapter = new BaseDeDatosDataSetTableAdapters.Prod_detalleTableAdapter();
        BaseDeDatosDataSetTableAdapters.Ingre_detalleTableAdapter ingredetalleTableAdapter = new BaseDeDatosDataSetTableAdapters.Ingre_detalleTableAdapter();
        BaseDeDatosDataSetTableAdapters.carpeta_detalleTableAdapter carpetadetalleTableAdapter = new BaseDeDatosDataSetTableAdapters.carpeta_detalleTableAdapter();
        BaseDeDatosDataSetTableAdapters.BufferSalidaTableAdapter BufferSalidaTableAdapter = new BaseDeDatosDataSetTableAdapters.BufferSalidaTableAdapter();
        BaseDeDatosDataSetTableAdapters.PLU_detalleTableAdapter pludetalleTableAdapter = new BaseDeDatosDataSetTableAdapters.PLU_detalleTableAdapter();
        BaseDeDatosDataSetTableAdapters.Vendedor_detalleTableAdapter vendedordetalleTableAdapter = new BaseDeDatosDataSetTableAdapters.Vendedor_detalleTableAdapter();

        #region Inicializacion
        public UserMaestros()
        {
            InitializeComponent();
            
            if (Variable.TypeScale == Variable.TipoBascula.WLABEL) { funcionWLABEL(); }//Funcion para configurar la aplicacionpara WLABEL
        }


        void funcionWLABEL()
        {
            toolStrip31.Items.Remove(StripPublicidad);
            toolStrip31.Items.Remove(StripImagen);
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

        #endregion

        private void UserMaestros_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Vendedor' Puede moverla o quitarla según sea necesario.
            this.vendedorTableAdapter.Fill(this.baseDeDatosDataSet.Vendedor);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Oferta' Puede moverla o quitarla según sea necesario.
            this.ofertaTableAdapter.Fill(this.baseDeDatosDataSet.Oferta);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Publicidad' Puede moverla o quitarla según sea necesario.
            this.publicidadTableAdapter.Fill(this.baseDeDatosDataSet.Publicidad);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Productos' Puede moverla o quitarla según sea necesario.
            this.productosTableAdapter.Fill(this.baseDeDatosDataSet.Productos);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Grupo' Puede moverla o quitarla según sea necesario.
            this.grupoTableAdapter.Fill(this.baseDeDatosDataSet.Grupo);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Bascula' Puede moverla o quitarla según sea necesario.
            this.basculaTableAdapter.Fill(this.baseDeDatosDataSet.Bascula);

            toolStripLabel3.Text = Nombre_Select;

            Asigna_Grupo();
            Asigna_Bascula();
            loadProductos();
        }


        void loadProductos()
        {
            StripProductos.Enabled = false;
            StripVendedores.Enabled = true;
            StripOferta.Enabled = true;
            StripPublicidad.Enabled = true;
            StripIngrediente.Enabled = true;
            StripImagen.Enabled = true;

            DatosEnviado = ESTADO.botonesEnvioDato.SDPRODUCTO;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.listViewDatos.Visible = true;
            this.listViewDatos.BringToFront();
            this.listViewDatos.View = View.Details;
            this.listViewDatos.FullRowSelect = true;
            this.listViewDatos.GridLines = true;
            this.listViewDatos.LabelEdit = false;
            this.listViewDatos.HideSelection = false;
            this.listViewDatos.Clear();
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[145, Variable.idioma], 80, HorizontalAlignment.Left);  //Codigeo
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[162, Variable.idioma], 80, HorizontalAlignment.Left);  //No. PLU
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[163, Variable.idioma], 230, HorizontalAlignment.Left);  //Nombre
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[166, Variable.idioma], 100, HorizontalAlignment.Left);  //Precio
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[168, Variable.idioma] + "1", 80, HorizontalAlignment.Left);  //Mensaje1
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[168, Variable.idioma] + "2", 80, HorizontalAlignment.Left);  //Mensaje2
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[168, Variable.idioma] + "3", 80, HorizontalAlignment.Left);  //Mensaje3
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[168, Variable.idioma] + "4", 80, HorizontalAlignment.Left);  // Mensaje4
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[159, Variable.idioma], 150, HorizontalAlignment.Left);  // Modificado

            Conec.CadenaSelect = "SELECT * FROM Productos ORDER BY Codigo ASC";

            productosTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            productosTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);
            DataRow[] DR = baseDeDatosDataSet.Productos.Select("", "Codigo ASC");

            foreach (DataRow DA in DR)
            {
                if (!Convert.ToBoolean(DA["borrado"].ToString()))
                {
                    ListViewItem lwitem = new ListViewItem(DA["Codigo"].ToString());  //0
                    lwitem.SubItems.Add(DA["NoPlu"].ToString()); //1
                    lwitem.SubItems.Add(DA["Nombre"].ToString());  //2
                    //lwitem.SubItems.Add(DA["Precio"].ToString());  //3
                    lwitem.SubItems.Add(string.Format(CultureInfo.CreateSpecificCulture(Variable.idioma2), Variable.F_Decimal, Convert.ToDecimal(DA["Precio"].ToString())));
                    lwitem.SubItems.Add(DA["publicidad1"].ToString());  //4
                    lwitem.SubItems.Add(DA["publicidad2"].ToString());  //5
                    lwitem.SubItems.Add(DA["publicidad3"].ToString());  //6
                    lwitem.SubItems.Add(DA["publicidad4"].ToString());  //7
                    lwitem.SubItems.Add(DA["actualizado"].ToString());  //8                    
                    lwitem.SubItems.Add(DA["Impuesto"].ToString());  //9
                    lwitem.SubItems.Add(DA["TipoID"].ToString()); //10
                    lwitem.SubItems.Add(DA["PrecioEditable"].ToString()); //11
                    lwitem.SubItems.Add(DA["CaducidadDias"].ToString()); //12
                    lwitem.SubItems.Add(DA["Mutiplo"].ToString()); //13
                    lwitem.SubItems.Add(DA["imagen1"].ToString()); //14
                    lwitem.SubItems.Add(DA["id_producto"].ToString()); //15
                    lwitem.SubItems.Add(DA["imagen"].ToString()); //16
                    lwitem.SubItems.Add(DA["pendiente"].ToString()); // 17
                    lwitem.SubItems.Add(DA["id_ingrediente"].ToString()); // 18
                    lwitem.SubItems.Add(DA["oferta"].ToString()); // 19
                    lwitem.SubItems.Add(DA["borrado"].ToString()); // 20
                    this.listViewDatos.Items.Add(lwitem);
                }
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }


        #region Botones del ToolStripMenu

        private void StripProductos_Click(object sender, EventArgs e)
        {
            loadProductos();
        }
        private void StripPublicidad_Click(object sender, EventArgs e)
        {
            StripPublicidad.Enabled = false;
            StripOferta.Enabled = true;
            StripImagen.Enabled = true;
            StripProductos.Enabled = true;
            StripVendedores.Enabled = true;
            StripIngrediente.Enabled = true;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            DatosEnviado = ESTADO.botonesEnvioDato.SDPUBLICIDAD;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.listViewDatos.Visible = true;
            this.listViewDatos.BringToFront();
            this.listViewDatos.View = View.Details;
            this.listViewDatos.FullRowSelect = true;
            this.listViewDatos.GridLines = true;
            this.listViewDatos.LabelEdit = false;
            this.listViewDatos.HideSelection = false;
            this.listViewDatos.Clear();
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[145, Variable.idioma], 80, HorizontalAlignment.Left);  //Codigo
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[174, Variable.idioma], 200, HorizontalAlignment.Left);  //Titulo
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[146, Variable.idioma], 800, HorizontalAlignment.Left);  //Descripcion


            Conec.CadenaSelect = "SELECT * FROM Publicidad ORDER BY id_publicidad";

            publicidadTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            publicidadTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            publicidadTableAdapter.Fill(baseDeDatosDataSet.Publicidad);

            for (int i = 0; i < baseDeDatosDataSet.Publicidad.Rows.Count; i++)
            {
                if (!Convert.ToBoolean(baseDeDatosDataSet.Publicidad.Rows[i]["borrado"].ToString()))
                {
                    ListViewItem lwitem = new ListViewItem(baseDeDatosDataSet.Publicidad.Rows[i]["id_publicidad"].ToString()); //0
                    lwitem.SubItems.Add(baseDeDatosDataSet.Publicidad.Rows[i]["Titulo"].ToString());//1
                    lwitem.SubItems.Add(baseDeDatosDataSet.Publicidad.Rows[i]["Mensaje"].ToString());//2
                    lwitem.SubItems.Add(baseDeDatosDataSet.Publicidad.Rows[i]["pendiente"].ToString());//3
                    lwitem.SubItems.Add(baseDeDatosDataSet.Publicidad.Rows[i]["borrado"].ToString());//4
                    this.listViewDatos.Items.Add(lwitem);
                }
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        private void StripImagen_Click(object sender, EventArgs e)
        {
            StripImagen.Enabled = false;
            StripProductos.Enabled = true;
            StripVendedores.Enabled = true;
            StripOferta.Enabled = true;
            StripPublicidad.Enabled = true;
            StripIngrediente.Enabled = true;

            DatosEnviado = ESTADO.botonesEnvioDato.SDIMAGEN;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.listViewDatos.Visible = true;
            this.listViewDatos.BringToFront();
            this.listViewDatos.View = View.Details;
            this.listViewDatos.FullRowSelect = true;
            this.listViewDatos.GridLines = true;
            this.listViewDatos.LabelEdit = false;
            this.listViewDatos.HideSelection = false;
            this.listViewDatos.Clear();
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[145, Variable.idioma], 80, HorizontalAlignment.Left);  //Codigo
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[163, Variable.idioma], 200, HorizontalAlignment.Left);  //Nombre
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[144, Variable.idioma], 800, HorizontalAlignment.Left);  //Carpeta del archivo
            // this.listViewDatos.Columns.Add(Variable.SYS_MSJ[176, Variable.idioma], 100, HorizontalAlignment.Left);  //Pendiente

            Conec.CadenaSelect = "SELECT * FROM Productos WHERE imagen = " + true + " ORDER BY id_producto";

            productosTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            productosTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);

            for (int i = 0; i < baseDeDatosDataSet.Productos.Rows.Count; i++)
            {
                //if (Convert.ToBoolean(baseDeDatosDataSet.Productos.Rows[i]["imagen"]) == true && baseDeDatosDataSet.Productos.Rows[i]["imagen1"].ToString().Length > 0)
                if (baseDeDatosDataSet.Productos.Rows[i]["imagen1"].ToString().Length > 0)
                {
                    ListViewItem lwitem = new ListViewItem(baseDeDatosDataSet.Productos.Rows[i]["Codigo"].ToString()); //0
                    lwitem.SubItems.Add(baseDeDatosDataSet.Productos.Rows[i]["Nombre"].ToString());  //1 
                    lwitem.SubItems.Add(baseDeDatosDataSet.Productos.Rows[i]["imagen1"].ToString()); //2
                    lwitem.SubItems.Add(baseDeDatosDataSet.Productos.Rows[i]["imagen"].ToString()); //3
                    lwitem.SubItems.Add(baseDeDatosDataSet.Productos.Rows[i]["id_producto"].ToString()); //4
                    this.listViewDatos.Items.Add(lwitem);
                }
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        private void StripOferta_Click(object sender, EventArgs e)
        {
            StripOferta.Enabled = false;
            StripImagen.Enabled = true;
            StripProductos.Enabled = true;
            StripVendedores.Enabled = true;
            StripPublicidad.Enabled = true;
            StripIngrediente.Enabled = true;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            DatosEnviado = ESTADO.botonesEnvioDato.SDOFERTA;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.listViewDatos.Visible = true;
            this.listViewDatos.BringToFront();
            this.listViewDatos.View = View.Details;
            this.listViewDatos.FullRowSelect = true;
            this.listViewDatos.GridLines = true;
            this.listViewDatos.LabelEdit = false;
            this.listViewDatos.HideSelection = false;
            this.listViewDatos.Clear();
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[145, Variable.idioma], 80, HorizontalAlignment.Left);  //Codigo
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[163, Variable.idioma], 230, HorizontalAlignment.Left);  //Nombre
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[151, Variable.idioma], 200, HorizontalAlignment.Left);  //Inicio
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[172, Variable.idioma], 200, HorizontalAlignment.Left);  //Termino
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[173, Variable.idioma], 50, HorizontalAlignment.Left);  //Tipo
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[147, Variable.idioma], 100, HorizontalAlignment.Left);  //Descuento

            Conec.CadenaSelect = "SELECT * FROM Oferta WHERE pendiente = " + true + " ORDER BY id_oferta";

            ofertaTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            ofertaTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            ofertaTableAdapter.Fill(baseDeDatosDataSet.Oferta);

            for (int i = 0; i < baseDeDatosDataSet.Oferta.Rows.Count; i++)
            {
                if (!Convert.ToBoolean(baseDeDatosDataSet.Oferta.Rows[i]["borrado"].ToString()))
                {
                    ListViewItem lwitem = new ListViewItem(baseDeDatosDataSet.Oferta.Rows[i]["id_oferta"].ToString()); //0
                    lwitem.SubItems.Add(baseDeDatosDataSet.Oferta.Rows[i]["Nombre"].ToString());  //1
                    lwitem.SubItems.Add(baseDeDatosDataSet.Oferta.Rows[i]["fecha_inicio"].ToString()); //2
                    lwitem.SubItems.Add(baseDeDatosDataSet.Oferta.Rows[i]["fecha_fin"].ToString()); //3
                    lwitem.SubItems.Add(baseDeDatosDataSet.Oferta.Rows[i]["tipo_desc"].ToString()); //4
                    lwitem.SubItems.Add(baseDeDatosDataSet.Oferta.Rows[i]["Descuento"].ToString()); //5
                    lwitem.SubItems.Add(baseDeDatosDataSet.Oferta.Rows[i]["nVentas"].ToString()); //6
                    lwitem.SubItems.Add(baseDeDatosDataSet.Oferta.Rows[i]["pendiente"].ToString()); //7
                    lwitem.SubItems.Add(baseDeDatosDataSet.Oferta.Rows[i]["borrado"].ToString()); // 8
                    this.listViewDatos.Items.Add(lwitem);
                }
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        private void StripVendedores_Click(object sender, EventArgs e)
        {
            StripVendedores.Enabled = false;
            StripProductos.Enabled = true;
            StripImagen.Enabled = true;
            StripPublicidad.Enabled = true;
            StripOferta.Enabled = true;
            StripIngrediente.Enabled = true;

            DatosEnviado = ESTADO.botonesEnvioDato.SDVENDEDOR;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            this.listViewDatos.Visible = true;
            this.listViewDatos.BringToFront();
            this.listViewDatos.View = View.Details;
            this.listViewDatos.FullRowSelect = true;
            this.listViewDatos.GridLines = true;
            this.listViewDatos.LabelEdit = false;
            this.listViewDatos.HideSelection = false;
            this.listViewDatos.Clear();
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[145, Variable.idioma], 80, HorizontalAlignment.Left);  //Codigo
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[163, Variable.idioma], 230, HorizontalAlignment.Left);  //Nombre

            Conec.CadenaSelect = "SELECT * FROM Vendedor WHERE pendeinte = " + true + " ORDER BY id_vendedor";

            vendedorTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            vendedorTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            vendedorTableAdapter.Fill(baseDeDatosDataSet.Vendedor);

            for (int i = 0; i < baseDeDatosDataSet.Vendedor.Rows.Count; i++)
            {
                if (!Convert.ToBoolean(baseDeDatosDataSet.Vendedor.Rows[i]["borrado"].ToString()))
                {
                    ListViewItem lwitem = new ListViewItem(baseDeDatosDataSet.Vendedor.Rows[i]["id_vendedor"].ToString());  //0
                    lwitem.SubItems.Add(baseDeDatosDataSet.Vendedor.Rows[i]["Nombre"].ToString()); //1
                    lwitem.SubItems.Add(baseDeDatosDataSet.Vendedor.Rows[i]["Msj_Enable"].ToString());  //2
                    lwitem.SubItems.Add(baseDeDatosDataSet.Vendedor.Rows[i]["Meta_Enable"].ToString()); //3
                    lwitem.SubItems.Add(baseDeDatosDataSet.Vendedor.Rows[i]["Meta_Ventas"].ToString()); //4
                    lwitem.SubItems.Add(baseDeDatosDataSet.Vendedor.Rows[i]["publicidad1"].ToString()); //5
                    lwitem.SubItems.Add(baseDeDatosDataSet.Vendedor.Rows[i]["publicidad2"].ToString()); //6
                    lwitem.SubItems.Add(baseDeDatosDataSet.Vendedor.Rows[i]["pendiente"].ToString()); //7
                    lwitem.SubItems.Add(baseDeDatosDataSet.Vendedor.Rows[i]["borrado"].ToString()); //8
                    this.listViewDatos.Items.Add(lwitem);
                }
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        private void StripIngrediente_Click(object sender, EventArgs e)
        {
            StripIngrediente.Enabled = false;
            StripProductos.Enabled = true;
            StripVendedores.Enabled = true;
            StripOferta.Enabled = true;
            StripPublicidad.Enabled = true;
            StripImagen.Enabled = true;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            DatosEnviado = ESTADO.botonesEnvioDato.SDINGREDIENTE;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.listViewDatos.Visible = true;
            this.listViewDatos.BringToFront();
            this.listViewDatos.View = View.Details;
            this.listViewDatos.FullRowSelect = true;
            this.listViewDatos.GridLines = true;
            this.listViewDatos.LabelEdit = false;
            this.listViewDatos.HideSelection = false;
            this.listViewDatos.Clear();
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[145, Variable.idioma], 80, HorizontalAlignment.Left);  //Codigo
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[163, Variable.idioma], 200, HorizontalAlignment.Left); //Nombre
            this.listViewDatos.Columns.Add(Variable.SYS_MSJ[150, Variable.idioma], 800, HorizontalAlignment.Left);  //Informacion
            //  this.listViewDatos.Columns.Add(Variable.SYS_MSJ[176, Variable.idioma], 100, HorizontalAlignment.Left);  //Pendiente

            Conec.CadenaSelect = "SELECT * FROM Ingredientes ORDER BY id_ingrediente";

            ingredientesTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            ingredientesTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            ingredientesTableAdapter.Fill(baseDeDatosDataSet.Ingredientes);

            for (int i = 0; i < baseDeDatosDataSet.Ingredientes.Rows.Count; i++)
            {
                if (!Convert.ToBoolean(baseDeDatosDataSet.Ingredientes.Rows[i]["borrado"].ToString()))
                {
                    ListViewItem lwitem = new ListViewItem(baseDeDatosDataSet.Ingredientes.Rows[i]["id_ingrediente"].ToString());  //0
                    lwitem.SubItems.Add(baseDeDatosDataSet.Ingredientes.Rows[i]["Nombre"].ToString());  //1
                    lwitem.SubItems.Add(baseDeDatosDataSet.Ingredientes.Rows[i]["Informacion"].ToString()); //2
                    lwitem.SubItems.Add(baseDeDatosDataSet.Ingredientes.Rows[i]["pendiente"].ToString()); // 3
                    lwitem.SubItems.Add(baseDeDatosDataSet.Ingredientes.Rows[i]["borrado"].ToString()); //4
                    this.listViewDatos.Items.Add(lwitem);
                }
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }


        private void StripEnviar_Click(object sender, EventArgs e)
        {
            LidBorr = new List<Int32>();
            LidPend = new List<Int32>();
            LidInfo = new List<Int32>();
            LidMens = new List<Int32>();
            LidOfer = new List<Int32>();
            LidVend = new List<Int32>();

            int BasculasActualizadas = 0;
            int NumeroDeBaculas = 0;

            try
            {
                if (listViewDatos.SelectedItems.Count > 0)
                {
                    ProgressContinue pro = new ProgressContinue();
                    pro.IniciaProcess(Variable.SYS_MSJ[192, Variable.idioma] + "... ");
                    
                    //validar si es un grupo
                    if (Num_Grupo != 0)//Quiere decir que hay grupos
                    {
                        /********************************************************************************/
                        //Obtener las basculas del grupo en cuestion
                        for (int pos = 0; pos < myScale.Length; pos++)  //Num de Basculas en el grupo
                            if (myScale[pos].gpo == Num_Grupo) NumeroDeBaculas++;
                        /********************************************************************************/

/*********************************************************************************************************************/
                        
                        //Enviar los productos a cada bascula del grupo

                        for (int pos = 0; pos < myScale.Length; pos++)
                        {
                            if (myScale[pos].gpo == Num_Grupo)//proceso para armar la bascula de este grupo
                            {
                                myCurrent.ip = myScale[pos].ip;  //direccion ip de la bascula
                                myCurrent.idbas = myScale[pos].idbas;   //numero id de la bascula
                                myCurrent.Nserie = myScale[pos].nserie;  //numero de serie de la bascula                    
                                myCurrent.nombre = myScale[pos].nombre;  //mombre de la bascula
                                myCurrent.gpo = myScale[pos].gpo;  //Grupo al que pertenece la bascula
                                myCurrent.COMM = myScale[pos].pto;
                                myCurrent.BAUD = myScale[pos].baud;
                                myCurrent.tipo = myScale[pos].tipo;
                                BasculasActualizadas++;//contador de basculas procesadas.
                                
                                //envio de la informacion a la bascula.
                                if (EnviaDatosA_Bascula((int)DatosEnviado, myCurrent.ip, myCurrent.tipo, myScale[pos].idbas, myScale[pos].gpo, ref pro)) //;Variable.IP_Address);  //, Num_gpo, Num_basc);                            
                                {
                                    BasculasActualizadas--;
                                    if (MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma] + " "
                                        + myCurrent.Nserie + " " + Variable.SYS_MSJ[417, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma],
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No) break;
                                }
                            }
                        }
/*****************************************************************************************************************************************/

                    }
                    else//quiere decir que hay basculas sin agrupar
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
                                myCurrent.COMM = myScale[pos].pto;
                                myCurrent.BAUD = myScale[pos].baud;
                                myCurrent.tipo = myScale[pos].tipo;
                                BasculasActualizadas++;

                                if (EnviaDatosA_Bascula((int)DatosEnviado, myCurrent.ip, myCurrent.tipo, myScale[pos].idbas, myScale[pos].gpo, ref pro)) //Variable.IP_Address);  //, Num_gpo, Num_basc);
                                {
                                    BasculasActualizadas--;
                                    MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma]
                                        + " " + myCurrent.Nserie + ".", Variable.SYS_MSJ[42, Variable.idioma],
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    break;
                                }
                            }
                        }
                    }

                    pro.TerminaProcess();
                    Actualizacion_Borrado_Datos((int)DatosEnviado);
                    Thread.Sleep(500);
                    MessageBox.Show(this, Variable.SYS_MSJ[418, Variable.idioma] + " " + BasculasActualizadas + " "
                        + Variable.SYS_MSJ[419, Variable.idioma] + " " + NumeroDeBaculas,
                        Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[324, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (this.listViewDatos.Items.Count > 0) this.listViewDatos.Items[0].Selected = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Variable.SYS_MSJ[381, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
            }  
        }
        private void StripCerrar_Click(object sender, EventArgs e)
        {
            ToolStripManager.RevertMerge("toolStrip3");
            this.Dispose();
        }

        #endregion

        #region Envio de informacion a basculas

        private void Actualizacion_Borrado_Datos(int Info_A_Enviar)
        {
            WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[327, Variable.idioma]);
            Thread t = new Thread(workerObject.vShowMsg);
            t.Start();

            switch (Info_A_Enviar)
            {
                case (int)ESTADO.botonesEnvioDato.SDPRODUCTO:
                    {
                        for (int o = 0; o < LidPend.Count; o++)
                        {
                            Actualizas_pendientes_productos(LidPend[o]);
                        }
                        for (int o = 0; o < LidInfo.Count; o++)
                        {
                            Actualizas_pendientes_ingredientes(LidInfo[o]);
                        }
                        for (int o = 0; o < LidOfer.Count; o++)
                        {
                            Actualizas_pendientes_ofertas(LidOfer[o]);
                        }
                        for (int o = 0; o < LidMens.Count; o++)
                        {
                            Actualizas_pendientes_publicidad(LidMens[o]);
                        }
                        for (int o = 0; o < LidBorr.Count; o++)
                        {
                            Eliminando_Productos_Borrados(LidBorr[o]);
                        }
                    }
                    break;

                case (int)ESTADO.botonesEnvioDato.SDVENDEDOR:
                    {
                        for (int o = 0; o < LidPend.Count; o++)
                        {
                            Actualizas_pendientes_vendedores(LidPend[o]);
                        }
                        for (int o = 0; o < LidBorr.Count; o++)
                        {
                            Eliminando_Vendedores_Borrados(LidBorr[o]);
                        }
                    }
                    break;
                case (int)ESTADO.botonesEnvioDato.SDPUBLICIDAD:
                    {
                        for (int o = 0; o < LidPend.Count; o++)
                        {
                            Actualizas_pendientes_publicidad(LidPend[o]);
                        }
                        for (int o = 0; o < LidBorr.Count; o++)
                        {
                            Eliminando_Publicidad_Borrados(LidBorr[o]);
                        }
                    }
                    break;
                case (int)ESTADO.botonesEnvioDato.SDINGREDIENTE:
                    {
                        for (int o = 0; o < LidPend.Count; o++)
                        {
                            Actualizas_pendientes_ingredientes(LidPend[o]);
                        }
                        for (int o = 0; o < LidBorr.Count; o++)
                        {
                            Eliminando_Ingredientes_Borrados(LidBorr[o]);
                        }
                    }
                    break;
                case (int)ESTADO.botonesEnvioDato.SDOFERTA:
                    {
                        for (int o = 0; o < LidPend.Count; o++)
                        {
                            Actualizas_pendientes_ofertas(LidPend[o]);
                        }
                        for (int o = 0; o < LidBorr.Count; o++)
                        {
                            Eliminando_Ofertas_Borrados(LidBorr[o]);
                        }
                    }
                    break;
            }

            workerObject.vEndShowMsg();
        }



        private bool EnviaDatosA_Bascula(int Info_A_Enviar, string direccionIP, int tipo_scale, int iIdBascula, int iIdGrupo, ref ProgressContinue pro)
        {
            int reg_total = listViewDatos.SelectedItems.Count;
            bool err = false;

            ingredientesTableAdapter.Fill(baseDeDatosDataSet.Ingredientes);
            ofertaTableAdapter.Fill(baseDeDatosDataSet.Oferta);
            publicidadTableAdapter.Fill(baseDeDatosDataSet.Publicidad);
            prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);

            Cursor.Current = Cursors.WaitCursor;

            switch (Info_A_Enviar)
            {
                case (int)ESTADO.botonesEnvioDato.SDPRODUCTO:
                    {
                        if (tipo_scale == (int)ESTADO.tipoConexionesEnum.PKWIFI) { err = Enviar_productos(direccionIP, reg_total, iIdBascula, iIdGrupo, ref pro); }
                        else { err = Enviar_productos(reg_total, ref pro); }
                    }
                    break;

                case (int)ESTADO.botonesEnvioDato.SDVENDEDOR:
                    {
                        if (tipo_scale == (int)ESTADO.tipoConexionesEnum.PKWIFI) { err = Enviar_vendedor(direccionIP, reg_total, ref pro); }
                        else { err = Enviar_vendedor(reg_total, ref pro); }
                    }
                    break;

                case (int)ESTADO.botonesEnvioDato.SDIMAGEN:
                    {
                        if (tipo_scale == (int)ESTADO.tipoConexionesEnum.PKWIFI)
                        {
                            err = vEnvia_Imagenes(iIdBascula, iIdGrupo, myCurrent.ip, ref pro);
                        }
                        else
                        {
                            err = Envia_Imagenes(reg_total, ref pro);
                        }
                    }
                    break;

                case (int)ESTADO.botonesEnvioDato.SDPUBLICIDAD:
                    {
                        if (tipo_scale == (int)ESTADO.tipoConexionesEnum.PKWIFI) { err = Enviar_publicidad(direccionIP, reg_total, ref pro); }
                        else { err = Enviar_publicidad(reg_total, ref pro); }
                    }
                    break;
                case (int)ESTADO.botonesEnvioDato.SDINGREDIENTE:
                    {
                        if (tipo_scale == (int)ESTADO.tipoConexionesEnum.PKWIFI) { err = Enviar_InfoAdicional(direccionIP, reg_total, ref pro); }
                        else { err = Enviar_InfoAdicional(reg_total, ref pro); }
                    }
                    break;
                case (int)ESTADO.botonesEnvioDato.SDOFERTA:
                    {
                        if (tipo_scale == (int)ESTADO.tipoConexionesEnum.PKWIFI) { err = Envia_Ofertas(direccionIP, reg_total, ref pro); }
                        else { err = Envia_Ofertas(reg_total, ref pro); }
                    }
                    break;
            }

            Cursor.Current = Cursors.Default;

            return err;
        }
        private void Enviar_Borrado_producto(string direccionIP, ref Socket Cliente_bascula, ref int TotalBorrado, int iIdBascula, int iIdGrupo)
        {
            string Msg_Recibido;
            string strborrado;
            char[] chr = new char[] { (char)10, (char)13 };

            DataRow[] DRE = baseDeDatosDataSet.Productos.Select("borrado = " + true, "id_producto");

            foreach (DataRow DR in DRE)
            {
                strborrado = DR["id_producto"].ToString() + (char)9 + (char)10;
                Msg_Recibido = Env.Command_Enviado(1, strborrado, direccionIP, ref Cliente_bascula, myCurrent.idbas, myCurrent.gpo, "EP");

                if (Msg_Recibido != null)
                {
                    if (LidBorr.BinarySearch(Convert.ToInt32(DR["id_producto"].ToString())) < 0)
                    {
                        LidBorr.Add(Convert.ToInt32(DR["id_producto"].ToString()));
                    }

                    Env.Borrar_Producto_detalle(myCurrent.idbas, myCurrent.gpo, strborrado.Split(chr));
                    TotalBorrado++;
                    strborrado = "";
                }
                DataRow[] DPD = baseDeDatosDataSet.Prod_detalle.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo + "and id_producto = " + Convert.ToInt32(DR["id_producto"].ToString()));
                if (DPD.Length > 0)
                {
                    foreach (DataRow PD in DPD)
                    {
                        strborrado = Env.Genera_Trama_Producto_Detalle_Eliminado(PD);
                        Msg_Recibido = Env.Command_Enviado(1, strborrado, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo, "EA");

                        if (Msg_Recibido != null)
                        {
                            if (LidBorr.BinarySearch(Convert.ToInt32(DR["id_producto"].ToString())) < 0)
                            {
                                LidBorr.Add(Convert.ToInt32(DR["id_producto"].ToString()));
                            }
                            strborrado = "";
                        }
                    }
                }
            }
        }
        private void Enviar_Borrado_publicidad(string direccionIP, ref Socket Cliente_bascula, ref int TotalBorrado, int iIdBascula, int iIdGrupo)
        {
            string Msg_Recibido;
            string strborrado;
            char[] chr = new char[] { (char)10, (char)13 };

            DataRow[] DRE = baseDeDatosDataSet.Publicidad.Select("borrado = " + true, "id_publicidad");

            foreach (DataRow DR in DRE)
            {
                strborrado = DR["id_publicidad"].ToString() + (char)9 + (char)10;
                Msg_Recibido = Env.Command_Enviado(1, strborrado, direccionIP, ref Cliente_bascula, myCurrent.idbas, myCurrent.gpo, "EM");

                if (Msg_Recibido != null)
                {
                    if (LidBorr.BinarySearch(Convert.ToInt32(DR["id_publicidad"].ToString())) < 0)
                    {
                        LidBorr.Add(Convert.ToInt32(DR["id_publicidad"].ToString()));
                    }
                    Env.Borrar_Publicidad_detalle(myCurrent.idbas, myCurrent.gpo, strborrado.Split(chr));
                    TotalBorrado++;
                    strborrado = "";
                }
            }
        }
        private void Enviar_Borrado_Ingredientes(string direccionIP, ref Socket Cliente_bascula, ref int TotalBorrado, int iIdBascula, int iIdGrupo)
        {
            string Msg_Recibido;
            string strborrado;
            char[] chr = new char[] { (char)10, (char)13 };

            DataRow[] DRE = baseDeDatosDataSet.Ingredientes.Select("borrado = " + true, "id_ingrediente");

            foreach (DataRow DR in DRE)
            {
                strborrado = DR["id_ingrediente"].ToString() + (char)9 + (char)10;
                Msg_Recibido = Env.Command_Enviado(1, strborrado, direccionIP, ref Cliente_bascula, myCurrent.idbas, myCurrent.gpo, "EI");

                if (Msg_Recibido != null)
                {
                    if (LidBorr.BinarySearch(Convert.ToInt32(DR["id_ingrediente"].ToString())) < 0)
                    {
                        LidBorr.Add(Convert.ToInt32(DR["id_ingrediente"].ToString()));
                    }
                    Env.Borrar_InfoAdd_detalle(myCurrent.idbas, myCurrent.gpo, strborrado.Split(chr));
                    TotalBorrado++;
                    strborrado = "";
                }
            }
        }
        private void Enviar_Borrado_Ofertas(string direccionIP, ref Socket Cliente_bascula, ref int TotalBorrado, int iIdBascula, int iIdGrupo)
        {
            string Msg_Recibido;
            string strborrado;
            char[] chr = new char[] { (char)10, (char)13 };

            DataRow[] DRE = baseDeDatosDataSet.Oferta.Select("borrado = " + true, "id_oferta");

            foreach (DataRow DR in DRE)
            {
                strborrado = DR["id_oferta"].ToString() + (char)9 + (char)10;
                Msg_Recibido = Env.Command_Enviado(1, strborrado, direccionIP, ref Cliente_bascula, myCurrent.idbas, myCurrent.gpo, "EO");

                if (Msg_Recibido != null)
                {
                    if (LidBorr.BinarySearch(Convert.ToInt32(DR["id_oferta"].ToString())) < 0)
                    {
                        LidBorr.Add(Convert.ToInt32(DR["id_oferta"].ToString()));
                    }
                    Env.Borrar_Oferta_detalle(myCurrent.idbas, myCurrent.gpo, strborrado.Split(chr));
                    TotalBorrado++;
                    strborrado = "";
                }
            }
        }
        private void Enviar_Borrado_Vendedores(string direccionIP, ref Socket Cliente_bascula, ref int TotalBorrado, int iIdBascula, int iIdGrupo)
        {
            string Msg_Recibido;
            string strborrado;
            char[] chr = new char[] { (char)10, (char)13 };

            DataRow[] DRE = baseDeDatosDataSet.Vendedor.Select("borrado = " + true, "id_vendedor");

            foreach (DataRow DR in DRE)
            {
                strborrado = DR["id_vendedor"].ToString() + (char)9 + (char)10;
                Msg_Recibido = Env.Command_Enviado(1, strborrado, direccionIP, ref Cliente_bascula, myCurrent.idbas, myCurrent.gpo, "EV");

                if (Msg_Recibido != null)
                {
                    Env.Borrar_Vendedor_detalle(myCurrent.idbas, myCurrent.gpo, strborrado.Split(chr));
                    TotalBorrado++;
                    strborrado = "";
                    if (LidBorr.BinarySearch(Convert.ToInt32(DR["id_vendedor"].ToString())) < 0)
                    {
                        LidBorr.Add(Convert.ToInt32(DR["id_vendedor"].ToString()));
                    }
                }
            }
        }

        private void Enviar_Borrado_producto(ref SerialPort Cliente_bascula, ref int TotalBorrado, int iIdBascula, int iIdGrupo)
        {
            string[] Msg_Recibido = null;
            string strborrado;
            char[] chr = new char[] { (char)10, (char)13 };

            DataRow[] DRE = baseDeDatosDataSet.Productos.Select("borrado = " + true, "id_producto");

            foreach (DataRow DR in DRE)
            {
                strborrado = DR["id_producto"].ToString() + (char)9 + (char)10;

                strborrado = "EP01" + DR["id_producto"].ToString() + (char)9 + (char)10;
                SR.SendCOMSerial(ref Cliente_bascula, strborrado, ref Msg_Recibido);

                if (Msg_Recibido != null)
                {
                    if (Msg_Recibido[0].IndexOf("Ok") >= 0)
                    {
                        if (LidBorr.BinarySearch(Convert.ToInt32(DR["id_producto"].ToString())) < 0)
                        {
                            LidBorr.Add(Convert.ToInt32(DR["id_producto"].ToString()));
                        }
                        Env.Borrar_Producto_detalle(myCurrent.idbas, myCurrent.gpo, strborrado.Split(chr));
                        TotalBorrado++;
                    }
                    if (Msg_Recibido[0].IndexOf("Error") >= 0)
                    {
                        Env.Guardar_Trama_pendiente(myCurrent.idbas, myCurrent.gpo, strborrado);
                    }
                    strborrado = "";
                }

                DataRow[] DPD = baseDeDatosDataSet.Prod_detalle.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo + "and id_producto = " + Convert.ToInt32(DR["id_producto"].ToString()));
                if (DPD.Length > 0)
                {
                    foreach (DataRow PD in DPD)
                    {
                        strborrado = "EA01" + Env.Genera_Trama_Producto_Detalle_Eliminado(PD);

                        SR.SendCOMSerial(ref Cliente_bascula, strborrado, ref Msg_Recibido);
                        if (Msg_Recibido != null)
                        {
                            if (Msg_Recibido[0].IndexOf("Ok") >= 0)
                            {
                                if (LidBorr.BinarySearch(Convert.ToInt32(DR["id_producto"].ToString())) < 0)
                                {
                                    LidBorr.Add(Convert.ToInt32(DR["id_producto"].ToString()));
                                }
                            }
                            if (Msg_Recibido[0].IndexOf("Error") >= 0)
                            {
                                Env.Guardar_Trama_pendiente(myCurrent.idbas, myCurrent.gpo, strborrado);
                            }
                            strborrado = "";
                        }
                    }
                }
            }
        }
        private void Enviar_Borrado_publicidad(ref SerialPort Cliente_bascula, ref int TotalBorrado, int iIdBascula, int iIdGrupo)
        {
            string[] Msg_Recibido = null;
            string strborrado;
            char[] chr = new char[] { (char)10, (char)13 };

            DataRow[] DRE = baseDeDatosDataSet.Publicidad.Select("borrado = " + true, "id_publicidad");

            foreach (DataRow DR in DRE)
            {                
                strborrado = "EM01" + DR["id_publicidad"].ToString() + (char)9 + (char)10;
                SR.SendCOMSerial(ref Cliente_bascula, strborrado, ref Msg_Recibido);
                if (Msg_Recibido != null)
                {
                    if (Msg_Recibido[0].IndexOf("Ok") >= 0)
                    {
                        if (LidBorr.BinarySearch(Convert.ToInt32(DR["id_publicidad"].ToString())) < 0)
                        {
                            LidBorr.Add(Convert.ToInt32(DR["id_publicidad"].ToString()));
                        }
                        Env.Borrar_Publicidad_detalle(myCurrent.idbas, myCurrent.gpo, strborrado.Split(chr));
                        TotalBorrado++;
                    }
                    if (Msg_Recibido[0].IndexOf("Error") >= 0)
                    {
                        Env.Guardar_Trama_pendiente(myCurrent.idbas, myCurrent.gpo, strborrado);
                    }
                }
            }
        }
        private void Enviar_Borrado_Ingredientes(ref SerialPort Cliente_bascula, ref int TotalBorrado, int iIdBascula, int iIdGrupo)
        {
            string[] Msg_Recibido = null;
            string strborrado;
            char[] chr = new char[] { (char)10, (char)13 };

            DataRow[] DRE = baseDeDatosDataSet.Ingredientes.Select("borrado = " + true, "id_ingrediente");

            foreach (DataRow DR in DRE)
            {  
                strborrado = "EI01" + DR["id_ingrediente"].ToString() + (char)9 + (char)10;
                SR.SendCOMSerial(ref Cliente_bascula, strborrado, ref Msg_Recibido);
                if (Msg_Recibido != null)
                {
                    if (Msg_Recibido[0].IndexOf("Ok") >= 0)
                    {
                        if (LidBorr.BinarySearch(Convert.ToInt32(DR["id_ingrediente"].ToString())) < 0)
                        {
                            LidBorr.Add(Convert.ToInt32(DR["id_ingrediente"].ToString()));
                        }
                        Env.Borrar_InfoAdd_detalle(myCurrent.idbas, myCurrent.gpo, strborrado.Split(chr));
                        TotalBorrado++;                       
                    }
                    if (Msg_Recibido[0].IndexOf("Error") >= 0)
                    {
                        Env.Guardar_Trama_pendiente(myCurrent.idbas, myCurrent.gpo, strborrado);
                    }
                    strborrado = "";
                }
            }
        }
        private void Enviar_Borrado_Ofertas(ref SerialPort Cliente_bascula, ref int TotalBorrado, int iIdBascula, int iIdGrupo)
        {
            string[] Msg_Recibido=null;
            string strborrado;
            char[] chr = new char[] { (char)10, (char)13 };

            DataRow[] DRE = baseDeDatosDataSet.Oferta.Select("borrado = " + true, "id_oferta");

            foreach (DataRow DR in DRE)
            {                
                strborrado = "EO01" + DR["id_oferta"].ToString() + (char)9 + (char)10;
                SR.SendCOMSerial(ref Cliente_bascula, strborrado, ref Msg_Recibido);

                if (Msg_Recibido != null)
                {
                    if (Msg_Recibido[0].IndexOf("Ok") >= 0)
                    {
                        if (LidBorr.BinarySearch(Convert.ToInt32(DR["id_oferta"].ToString())) < 0)
                        {
                            LidBorr.Add(Convert.ToInt32(DR["id_oferta"].ToString()));
                        }
                        Env.Borrar_Oferta_detalle(myCurrent.idbas, myCurrent.gpo, strborrado.Split(chr));
                        TotalBorrado++;
                    }
                    if (Msg_Recibido[0].IndexOf("Error") >= 0)
                    {
                        Env.Guardar_Trama_pendiente(myCurrent.idbas, myCurrent.gpo, strborrado);
                    }
                    strborrado = "";
                }
            }
        }
        private void Enviar_Borrado_Vendedores(ref SerialPort Cliente_bascula, ref int TotalBorrado, int iIdBascula, int iIdGrupo)
        {
            string[] Msg_Recibido = null;
            string strborrado;
            char[] chr = new char[] { (char)10, (char)13 };

            DataRow[] DRE = baseDeDatosDataSet.Vendedor.Select("borrado = " + true, "id_vendedor");

            foreach (DataRow DR in DRE)
            {                
                strborrado = "EV01" + DR["id_vendedor"].ToString() + (char)9 + (char)10;
                SR.SendCOMSerial(ref Cliente_bascula, strborrado, ref Msg_Recibido);

                if (Msg_Recibido != null)
                {
                    if (Msg_Recibido[0].IndexOf("Ok") >= 0)
                    {
                        if (LidBorr.BinarySearch(Convert.ToInt32(DR["id_vendedor"].ToString())) < 0)
                        {
                            LidBorr.Add(Convert.ToInt32(DR["id_vendedor"].ToString()));
                        }
                        Env.Borrar_Vendedor_detalle(myCurrent.idbas, myCurrent.gpo, strborrado.Split(chr));
                        TotalBorrado++;
                    }
                    if (Msg_Recibido[0].IndexOf("Error") >= 0)
                    {
                        Env.Guardar_Trama_pendiente(myCurrent.idbas, myCurrent.gpo, strborrado);
                    }
                }
            }
        }

        private bool Enviar_productos(string direccionIP, int reg_total, int iIdBascula, int iIdGrupo, ref ProgressContinue pro)
        {
            int reg_leido = 0;
            int reg_envio = 0;
            string Msg_Recibido;
            char[] chr = new char[] { (char)10, (char)13 };
            int TotalNoEnviados = 0;
            int TotalEnviados = 0;
            int TotalBorrado = 0;
            string Variable_frame = null;
            bool ERROR = false;

            CommandTorrey myobj = new CommandTorrey();

            if (listViewDatos.SelectedItems.Count > 0)
            {
                Cliente_bascula = Cte.conectar(direccionIP, 50036);  //, Variable.frame, ref Dato_Recivido);
                if (Cliente_bascula != null)
                {
                    reg_leido = 0;
                    reg_envio = 0;
                    Variable_frame = "";

                    string sComando = "XX" + (char)9 + (char)10;

                    Msg_Recibido = Env.Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");
                    if (Msg_Recibido != null)
                    {
                        if (Variable.TypeScale == Variable.TipoBascula.WLS)//Solo se envian en las wls
                        {
                            if (Variable.EnviarImagenesProductos)
                            {
                                vEnvia_Imagenes(iIdBascula, iIdGrupo, direccionIP, Cliente_bascula, ref TotalEnviados, ref TotalNoEnviados, ref pro);
                            }
                        }
                        pro.IniciaProcess(reg_total, Variable.SYS_MSJ[249, Variable.idioma] + " " + myCurrent.Nserie + "... ");

                        for (int nreg = 0; nreg < listViewDatos.Items.Count; nreg++)
                        {
                            if (listViewDatos.Items[nreg].Selected)
                            {
                                DataRow DR = baseDeDatosDataSet.Productos.Rows.Find(Convert.ToInt32(listViewDatos.Items[nreg].SubItems[15].Text));
                                if (!Convert.ToBoolean(DR["borrado"]))
                                {
                                    if (LidPend.BinarySearch(Convert.ToInt32(DR["id_producto"].ToString())) < 0)
                                    {
                                        LidPend.Add(Convert.ToInt32(DR["id_producto"].ToString()));
                                    }
                                    Variable_frame = Variable_frame + Env.Genera_Trama_Producto(DR);

                                    reg_leido++;
                                    if (reg_leido > 4)
                                    {
                                        reg_envio = reg_envio + reg_leido;

                                        Msg_Recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, myCurrent.idbas, myCurrent.gpo, "GP");

                                        if (Msg_Recibido != null)
                                        {
                                            Env.Crea_Producto_Detalle(myCurrent.idbas, myCurrent.gpo, Variable_frame.Split(chr), false, true);
                                            TotalEnviados = TotalEnviados + reg_leido;                                           
                                        }
                                        else
                                        {
                                            TotalNoEnviados = TotalNoEnviados + reg_leido;
                                            ERROR = true;
                                        }

                                        pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[249, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                                        reg_leido = 0;
                                        Variable_frame = "";
                                    }
                                }
                            }
                        }
                        if (Variable_frame.Length > 0 && reg_leido <= 4)
                        {
                            reg_envio = reg_envio + reg_leido;

                            Msg_Recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, myCurrent.idbas, myCurrent.gpo, "GP");

                            if (Msg_Recibido != null)
                            {
                                Env.Crea_Producto_Detalle(myCurrent.idbas, myCurrent.gpo, Variable_frame.Split(chr), false, true);
                                TotalEnviados = TotalEnviados + reg_leido;
                            }
                            else
                            {
                                TotalNoEnviados = TotalNoEnviados + reg_leido;
                                ERROR = true;
                            }

                            pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[249, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                            reg_leido = 0;
                            Variable_frame = "";
                        }

                        if (Env.Command_Limpiar(direccionIP, ref Cliente_bascula, "GPF0"))
                        {
                            Enviar_InfoAdicional(direccionIP, ref Cliente_bascula, ref TotalEnviados, ref TotalNoEnviados, ref pro);
                            Envia_Ofertas(direccionIP, ref Cliente_bascula, ref TotalEnviados, ref TotalNoEnviados, ref pro);
                            Enviar_publicidad(direccionIP, ref Cliente_bascula, ref TotalEnviados, ref TotalNoEnviados, ref pro);
                        }

                        Enviar_Borrado_producto(direccionIP, ref Cliente_bascula, ref TotalBorrado, iIdBascula, iIdGrupo);
                        Cte.desconectar(ref Cliente_bascula);

                        //MessageBox.Show(this, TotalNoEnviados.ToString() + " " + Variable.SYS_MSJ[282, Variable.idioma] + (char)13 + TotalEnviados.ToString() + " " + Variable.SYS_MSJ[281, Variable.idioma] + (char)13 + TotalBorrado.ToString() + " " + Variable.SYS_MSJ[283, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    ERROR = true;
                }
            }

            return ERROR;
        }
        private bool Enviar_productos(int reg_total, ref ProgressContinue pro)
        {
            int reg_leido = 0;
            int reg_envio = 0;
            string[] Msg_Recibido = null;
            string strcomando;
            string Variable_frame = null;
            char[] chr = new char[] { (char)10, (char)13 };
            int TotalNoEnviados = 0;
            int TotalEnviados = 0;
            int TotalBorrado = 0;
            bool ERROR = false;

            if (listViewDatos.SelectedItems.Count > 0)
            {
                serialPort1 = new SerialPort();
                if (SR.OpenPort(ref serialPort1, myCurrent.COMM, myCurrent.BAUD)) //Variable.P_COMM, Variable.Buad))
                {
                    SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)9 + (char)10);
                    
                    pro.IniciaProcess(reg_total, Variable.SYS_MSJ[249, Variable.idioma] + " " + myCurrent.Nserie + "... ");  //"Enviando Productos", "Iniciando proceso");

                    reg_leido = 0;
                    reg_envio = 0;
                    Variable_frame = "";

                    for (int nreg = 0; nreg < listViewDatos.Items.Count; nreg++)
                    {
                        if (listViewDatos.Items[nreg].Selected)
                        {
                            DataRow DR = baseDeDatosDataSet.Productos.Rows.Find(Convert.ToInt32(listViewDatos.Items[nreg].SubItems[15].Text));
                           
                            if (!Convert.ToBoolean(DR["borrado"]))
                            {
                                Variable_frame = Variable_frame + Env.Genera_Trama_Producto(DR);

                                reg_leido++;

                                if (reg_leido > 4)
                                {
                                    reg_envio = reg_envio + reg_leido;
                                    
                                    strcomando = "GP" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                                    
                                    SR.SendCOMSerial(ref serialPort1, strcomando, ref Msg_Recibido);
                                    if (Msg_Recibido != null)
                                    {
                                        if (Msg_Recibido[0].IndexOf("Error") >= 0)
                                        {
                                            Env.Guardar_Trama_pendiente(myCurrent.idbas, myCurrent.gpo, strcomando);
                                            ERROR = true;
                                        }
                                        if (Msg_Recibido[0].IndexOf("Ok") >= 0)
                                        {
                                            Env.Crea_Producto_Detalle(myCurrent.idbas, myCurrent.gpo, Variable_frame.Split(chr), false, true);                                            
                                        }
                                        pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[249, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                                    }
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
                        
                        SR.SendCOMSerial(ref serialPort1, strcomando, ref Msg_Recibido);
                        if (Msg_Recibido != null)
                        {
                            if (Msg_Recibido[0].IndexOf("Error") >= 0)
                            {
                                Env.Guardar_Trama_pendiente(myCurrent.idbas, myCurrent.gpo, strcomando);
                                ERROR = true;
                            }
                            if (Msg_Recibido[0].IndexOf("Ok") >= 0)
                            {
                                Env.Crea_Producto_Detalle(myCurrent.idbas, myCurrent.gpo, Variable_frame.Split(chr), false, true);
                            }
                            pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[249, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                        }

                        reg_leido = 0;
                        Variable_frame = "";
                    }

                    Variable_frame = "GPF0" + (char)9 + (char)10;
                    SR.SendCOMSerial(ref serialPort1, Variable_frame, ref Msg_Recibido);
                    if (Msg_Recibido[0].IndexOf("Ok") > 0)
                    {
                        Enviar_InfoAdicional(ref serialPort1, ref TotalEnviados, ref TotalNoEnviados, ref pro);
                        Envia_Ofertas(ref serialPort1, ref TotalEnviados, ref TotalNoEnviados, ref pro);
                        Enviar_publicidad(ref serialPort1, ref TotalEnviados, ref TotalNoEnviados, ref pro);
                        Envia_Imagenes(listViewDatos.SelectedItems.Count, ref pro);
                    }
                    
                    Enviar_Borrado_producto(ref serialPort1, ref TotalBorrado, myCurrent.idbas, myCurrent.gpo);
                    SR.SendCOMSerial(ref serialPort1, "dXXX" + (char)9 + (char)10);
                    SR.ClosePort(ref serialPort1);
                }
                else
                {
                    ERROR = true;
                }
            }

            return ERROR;
        }

        private bool Enviar_vendedor(string direccionIP, int reg_total, ref ProgressContinue pro)
        {
            int reg_leido = 0;
            int reg_envio = 0;
            string Msg_Recibido;
            string Variable_frame;
            char[] chr = new char[] { (char)10, (char)13 };
            int TotalNoEnviados = 0;
            int TotalEnviados = 0;
            int TotalBorrado = 0;
            bool ERROR = false;

            if (listViewDatos.SelectedItems.Count > 0)
            {
                Cliente_bascula = Cte.conectar(direccionIP, 50036);
                if (Cliente_bascula != null)
                {
                    pro.IniciaProcess(reg_total, Variable.SYS_MSJ[254, Variable.idioma] + " " + myCurrent.Nserie + "... ");  //"Enviando Vendedores", "Iniciando proceso");

                    string sComando = "XX" + (char)9 + (char)10;

                    Msg_Recibido = Env.Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");
                    if (Msg_Recibido != null)
                    {
                        Variable_frame = "";
                        for (int nreg = 0; nreg < listViewDatos.Items.Count; nreg++)
                        {
                            if (listViewDatos.Items[nreg].Selected)
                            {
                                DataRow DR = baseDeDatosDataSet.Vendedor.Rows.Find(Convert.ToInt32(listViewDatos.Items[nreg].SubItems[0].Text));
                                if (!Convert.ToBoolean(DR["borrado"]))
                                {
                                    if (LidPend.BinarySearch(Convert.ToInt32(DR["id_vendedor"].ToString())) < 0)
                                    {
                                        LidPend.Add(Convert.ToInt32(DR["id_vendedor"].ToString()));
                                    }
                                    Variable_frame = Variable_frame + Env.Genera_Trama_Vendedor(DR);
                                    reg_leido++;

                                    pro.UpdateProcess(1, Variable.SYS_MSJ[254, Variable.idioma] + " " + myCurrent.Nserie + "... ");

                                    if (reg_leido > 4)
                                    {
                                        reg_envio = reg_envio + reg_leido;
                                        Msg_Recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, myCurrent.idbas, myCurrent.gpo, "GV");

                                        if (Msg_Recibido != null)
                                        {
                                            Env.Crear_Vendedor_detalle(myCurrent.idbas, myCurrent.gpo, Variable_frame.Split(chr), false, true);
                                            TotalEnviados = TotalEnviados + reg_leido;
                                        }
                                        else
                                        {
                                            TotalNoEnviados = TotalNoEnviados + reg_leido;
                                            ERROR = true;
                                        }
                                        reg_leido = 0;
                                        Variable_frame = "";
                                    }
                                }
                            }
                        }
                        if (Variable_frame.Length > 0 && reg_leido <= 4)
                        {
                            reg_envio = reg_envio + reg_leido;

                            Msg_Recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, myCurrent.idbas, myCurrent.gpo, "GV");

                            if (Msg_Recibido != null) //Env.Modifica_Estado_Vendedores(Variable.frame.Split(chr), true);
                            {
                                Env.Crear_Vendedor_detalle(myCurrent.idbas, myCurrent.gpo, Variable_frame.Split(chr), false, true);
                                TotalEnviados = TotalEnviados + reg_leido;
                            }
                            else
                            {
                                TotalNoEnviados = TotalNoEnviados + reg_leido;
                                ERROR = true;
                            }
                            reg_leido = 0;
                            Variable_frame = "";
                        }

                        Enviar_Borrado_Vendedores(direccionIP, ref Cliente_bascula, ref TotalBorrado, myCurrent.idbas, myCurrent.gpo);

                    }
                    Cte.desconectar(ref Cliente_bascula);

                    //MessageBox.Show(this, TotalNoEnviados.ToString() + " " + Variable.SYS_MSJ[282, Variable.idioma] + (char)13 + TotalEnviados.ToString() + " " + Variable.SYS_MSJ[281, Variable.idioma] + (char)13 + TotalBorrado.ToString() + " " + Variable.SYS_MSJ[283, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ERROR = true;
                }
            }

            return ERROR;
        }
        private bool Enviar_vendedor(int reg_total, ref ProgressContinue pro)
        {
            int reg_leido = 0;
            int reg_envio = 0;
            string[] Msg_Recibido = null;
            string strcomando;
            string Variable_frame = null;
            char[] chr = new char[] { (char)10, (char)13 };
            int TotalNoEnviados = 0;
            int TotalEnviados = 0;
            int TotalBorrado = 0;
            bool ERROR = false;

            serialPort1 = new SerialPort();

            if (SR.OpenPort(ref serialPort1, myCurrent.COMM, myCurrent.BAUD))  //Variable.P_COMM, Variable.Buad))
            {
                SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)9 + (char)10);

                pro.IniciaProcess(reg_total, Variable.SYS_MSJ[254, Variable.idioma] + " " + myCurrent.Nserie + "... ");  //"Enviando Vendedores", "Iniciando proceso");

                Variable_frame = "";
                for (int nreg = 0; nreg < listViewDatos.Items.Count; nreg++)
                {
                    if (listViewDatos.Items[nreg].Selected)
                    {
                        DataRow DR = baseDeDatosDataSet.Vendedor.Rows.Find(Convert.ToInt32(listViewDatos.Items[nreg].SubItems[0].Text));
                        if (!Convert.ToBoolean(DR["borrado"]))
                        {
                            if (LidPend.BinarySearch(Convert.ToInt32(DR["id_vendedor"].ToString())) < 0)
                            {
                                LidPend.Add(Convert.ToInt32(DR["id_vendedor"].ToString()));
                            }
                            Variable_frame = Variable_frame + Env.Genera_Trama_Vendedor(DR);
                            reg_leido++;
                            if (reg_leido > 4)
                            {
                                reg_envio = reg_envio + reg_leido;
                                strcomando = "GV" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                                
                                SR.SendCOMSerial(ref serialPort1, strcomando, ref Msg_Recibido);
                                if (Msg_Recibido[0].IndexOf("Error") >= 0)
                                {
                                    Env.Guardar_Trama_pendiente(myCurrent.idbas, myCurrent.gpo, strcomando);
                                    TotalNoEnviados = TotalNoEnviados + reg_leido;
                                    ERROR = true;
                                }
                                if (Msg_Recibido[0].IndexOf("Ok") >= 0)
                                {
                                    Env.Crear_Vendedor_detalle(myCurrent.idbas, myCurrent.gpo, Variable_frame.Split(chr), false, true);
                                    TotalEnviados = TotalEnviados + reg_leido;                                   
                                }

                                pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[254, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                                
                                reg_leido = 0;
                                Variable_frame = "";
                            }
                        }                      
                    }
                }
                if (Variable_frame.Length > 0 && reg_leido <= 4)
                {
                    reg_envio = reg_envio + reg_leido;

                    strcomando = "GV" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;                    
                    SR.SendCOMSerial(ref serialPort1, strcomando, ref Msg_Recibido);

                    if (Msg_Recibido[0].IndexOf("Error") >= 0)
                    {
                        Env.Guardar_Trama_pendiente(myCurrent.idbas, myCurrent.gpo, strcomando);
                        TotalNoEnviados = TotalNoEnviados + reg_leido;
                        ERROR = true;
                    }
                    if (Msg_Recibido[0].IndexOf("Ok") >= 0)
                    {
                        TotalEnviados = TotalEnviados + reg_leido;
                        Env.Crear_Vendedor_detalle(myCurrent.idbas, myCurrent.gpo, Variable_frame.Split(chr), false, true);
                    }

                    pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[254, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                    reg_leido = 0;
                    Variable_frame = "";
                }

                Enviar_Borrado_Vendedores(ref serialPort1, ref TotalBorrado, myCurrent.idbas, myCurrent.gpo);
                SR.SendCOMSerial(ref serialPort1, "dXXX" + (char)9 + (char)10);
                SR.ClosePort(ref serialPort1);
                
            }
            else
            {
                ERROR = true;
            }

            return ERROR;
        }

        private bool Enviar_publicidad(string direccionIP, ref Socket Cliente_bascula, ref int Enviado, ref int NoEnviado, ref ProgressContinue pro)
        {
            publicidadTableAdapter.Fill(baseDeDatosDataSet.Publicidad);
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msg_Recibido;        
            string Variable_frame = null;
            int reg_leido = 0;
            int reg_envio = 0;
            List<int> idPubl = new List<int>();
            bool ERROR = false;

            for (int i = 0; i < listViewDatos.SelectedItems.Count; i++)
            {
                int pub1 = Convert.ToInt32(listViewDatos.SelectedItems[i].SubItems[4].Text);
                int pub2 = Convert.ToInt32(listViewDatos.SelectedItems[i].SubItems[5].Text);
                int pub3 = Convert.ToInt32(listViewDatos.SelectedItems[i].SubItems[6].Text);
                int pub4 = Convert.ToInt32(listViewDatos.SelectedItems[i].SubItems[7].Text);
                if (!Convert.ToBoolean(listViewDatos.SelectedItems[i].SubItems[20].Text))
                {
                    if (idPubl.BinarySearch(pub1) < 0 && pub1 > 0)
                    {
                        idPubl.Add(pub1);
                    }
                    if (idPubl.BinarySearch(pub2) < 0 && pub2 > 0)
                    {
                        idPubl.Add(pub2);
                    }
                    if (idPubl.BinarySearch(pub3) < 0 && pub3 > 0)
                    {
                        idPubl.Add(pub3);
                    }
                    if (idPubl.BinarySearch(pub4) < 0 && pub4 > 0)
                    {
                        idPubl.Add(pub4);
                    }
                }
            }
            
            Variable_frame = "";
            
            if (idPubl.Count > 0)
            {

                pro.IniciaProcess(idPubl.Count, Variable.SYS_MSJ[255, Variable.idioma] + " " + myCurrent.Nserie + "... ");

                for (int nreg = 0; nreg < idPubl.Count; nreg++)   
                {
                    DataRow DP = baseDeDatosDataSet.Publicidad.Rows.Find(idPubl[nreg]);
                    if (DP != null)
                    {
                        if (LidMens.BinarySearch(Convert.ToInt32(DP["id_publicidad"].ToString())) < 0)
                        {
                            LidMens.Add(Convert.ToInt32(DP["id_publicidad"].ToString()));
                        }

                        Variable_frame = Variable_frame + Env.Genera_Trama_Publicidad(DP);
                        reg_leido++;

                        if (reg_leido > 4)
                        {
                            reg_envio = reg_envio + reg_leido;

                            Msg_Recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo, "GM");
                            if (Msg_Recibido != null)
                            {
                                Env.Crea_Publicidad_Detalle(myCurrent.idbas, myCurrent.gpo, Msg_Recibido.Split(chr), false, true);
                                Enviado = Enviado + reg_leido;
                            }
                            else
                            {
                                NoEnviado = NoEnviado + reg_leido;
                                ERROR = true;
                            }
                            pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[255, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                            reg_leido = 0;
                            Variable_frame = "";
                        }
                    }
                }
                if (Variable_frame.Length > 0 && reg_leido <= 4)
                {
                    reg_envio = reg_envio + reg_leido;

                    Msg_Recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo, "GM");
                    if (Msg_Recibido != null)
                    {
                        Env.Crea_Publicidad_Detalle(myCurrent.idbas, myCurrent.gpo, Msg_Recibido.Split(chr), false, true);
                        Enviado = Enviado + reg_leido;
                    }
                    else
                    {
                        NoEnviado = NoEnviado + reg_leido;
                        ERROR = true;
                    }
                    pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[255, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                    reg_leido = 0;
                    Variable_frame = "";
                }               
            }

            return ERROR;
        }
        private bool Enviar_publicidad(string direccionIP, int reg_total, ref ProgressContinue pro)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msg_Recibido;
            string Variable_frame;
            int reg_leido = 0;
            int reg_envio = 0;
            int TotalEnviados = 0;
            int TotalNoEnviados = 0;
            int TotalBorrado = 0;
            bool ERROR = false;

            if (listViewDatos.SelectedItems.Count > 0)
            {
                Cliente_bascula = Cte.conectar(direccionIP, 50036);
                if (Cliente_bascula != null)
                {
                    pro.IniciaProcess(reg_total, Variable.SYS_MSJ[255, Variable.idioma] + " " + myCurrent.Nserie + "... ");  // "Enviando Mensajes", "Iniciando proceso");

                    string sComando = "XX" + (char)9 + (char)10;

                    Msg_Recibido = Env.Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");
                    if (Msg_Recibido != null)
                    {
                        Variable_frame = "";
                        for (int nreg = 0; nreg < listViewDatos.Items.Count; nreg++)
                        {
                            if (listViewDatos.Items[nreg].Selected)
                            {
                                DataRow DR = baseDeDatosDataSet.Publicidad.Rows.Find(Convert.ToInt32(listViewDatos.Items[nreg].SubItems[0].Text));
                                if (!Convert.ToBoolean(DR["borrado"]))
                                {
                                    if (LidPend.BinarySearch(Convert.ToInt32(DR["id_publicidad"].ToString())) < 0)
                                    {
                                        LidPend.Add(Convert.ToInt32(DR["id_publicidad"].ToString()));
                                    }
                                    Variable_frame = Variable_frame + Env.Genera_Trama_Publicidad(DR);
                                    reg_leido++;
                                    if (reg_leido > 4)
                                    {
                                        reg_envio = reg_envio + reg_leido;

                                        Msg_Recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, myCurrent.idbas, myCurrent.gpo, "GM");
                                        if (Msg_Recibido != null)
                                        {
                                            Env.Crea_Publicidad_Detalle(myCurrent.idbas, myCurrent.gpo, Msg_Recibido.Split(chr), false, true);
                                            TotalEnviados = TotalEnviados + reg_leido;
                                        }
                                        else
                                        {
                                            TotalNoEnviados = TotalNoEnviados + reg_leido;
                                            ERROR = true;
                                        }
                                        pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[255, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                                        reg_leido = 0;
                                        Variable_frame = "";
                                    }
                                }

                            }
                        }
                        if (Variable_frame.Length > 0 && reg_leido <= 4)
                        {
                            reg_envio = reg_envio + reg_leido;

                            Msg_Recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo, "GM");
                            if (Msg_Recibido != null)
                            {
                                Env.Crea_Publicidad_Detalle(myCurrent.idbas, myCurrent.gpo, Msg_Recibido.Split(chr), false, true);
                                TotalEnviados = TotalEnviados + reg_leido;
                            }
                            else
                            {
                                TotalNoEnviados = TotalNoEnviados + reg_leido;
                                ERROR = true;
                            }
                            pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[255, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                            reg_leido = 0;
                            Variable_frame = "";
                        }

                    }

                    Enviar_Borrado_publicidad(direccionIP, ref Cliente_bascula, ref TotalBorrado, myCurrent.idbas, myCurrent.gpo);
                    Cte.desconectar(ref Cliente_bascula);

                    //MessageBox.Show(this, TotalNoEnviados.ToString() + " " + Variable.SYS_MSJ[282, Variable.idioma] + (char)13 + TotalEnviados.ToString() + " " + Variable.SYS_MSJ[281, Variable.idioma] + (char)13 + TotalBorrado.ToString() + " " + Variable.SYS_MSJ[283, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ERROR = true;
                }
            }

            return ERROR;
        }
        private bool Enviar_publicidad(ref SerialPort Cliente_bascula, ref int Enviado, ref int NoEnviado, ref ProgressContinue pro)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msg_Recibido;
            string[] Dato_Recibido = null;
            string strcomando;
            string Variable_frame = null;
            bool ERROR = false;

            int reg_leido = 0;
            int reg_envio = 0;

            List<int> idPubl = new List<int>();

            for (int i = 0; i < listViewDatos.SelectedItems.Count; i++)
            {
                int pub1 = Convert.ToInt32(listViewDatos.SelectedItems[i].SubItems[4].Text);
                int pub2 = Convert.ToInt32(listViewDatos.SelectedItems[i].SubItems[5].Text);
                int pub3 = Convert.ToInt32(listViewDatos.SelectedItems[i].SubItems[6].Text);
                int pub4 = Convert.ToInt32(listViewDatos.SelectedItems[i].SubItems[7].Text);

                if (idPubl.BinarySearch(pub1) < 0 && pub1 > 0)
                {
                    idPubl.Add(pub1);
                }
                if (idPubl.BinarySearch(pub2) < 0 && pub2 > 0)
                {
                    idPubl.Add(pub2);
                }
                if (idPubl.BinarySearch(pub3) < 0 && pub3 > 0)
                {
                    idPubl.Add(pub3);
                }
                if (idPubl.BinarySearch(pub4) < 0 && pub4 > 0)
                {
                    idPubl.Add(pub4);
                }
            }
            
            Variable_frame = "";

            if (idPubl.Count > 0)
            {

                pro.IniciaProcess(idPubl.Count, Variable.SYS_MSJ[255, Variable.idioma] + " " + myCurrent.Nserie + "... ");

                for (int nreg = 0; nreg < idPubl.Count; nreg++)
                {
                    DataRow DP = baseDeDatosDataSet.Publicidad.Rows.Find(idPubl[nreg]);
                    if (DP != null)
                    {
                        if (LidMens.BinarySearch(Convert.ToInt32(DP["id_publicidad"].ToString())) < 0)
                        {
                            LidMens.Add(Convert.ToInt32(DP["id_publicidad"].ToString()));
                        }
                        Variable_frame = Variable_frame + Env.Genera_Trama_Publicidad(DP);
                        reg_leido++;
                        if (reg_leido > 4)
                        {
                            reg_envio = reg_envio + reg_leido;
                            strcomando = "GM" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;

                            SR.SendCOMSerial(ref serialPort1, strcomando, ref Dato_Recibido);
                            Msg_Recibido = Variable_frame;
                            if (Dato_Recibido[0].IndexOf("Ok") >= 0)
                            {
                                Env.Crea_Publicidad_Detalle(myCurrent.idbas, myCurrent.gpo, Msg_Recibido.Split(chr), false, true);
                                Enviado = Enviado + reg_leido;
                            }
                            if (Dato_Recibido[0].IndexOf("Error") >= 0)
                            {
                                Env.Guardar_Trama_pendiente(myCurrent.idbas, myCurrent.gpo, strcomando);
                                NoEnviado = NoEnviado + reg_leido;
                                ERROR = true;
                            }
                            pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[255, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                            reg_leido = 0;
                            Variable_frame = "";
                        }
                    }
                }
                if (Variable_frame.Length > 0 && reg_leido <= 4)
                {
                    reg_envio = reg_envio + reg_leido;
                    strcomando = "GM" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;

                    SR.SendCOMSerial(ref serialPort1, strcomando, ref Dato_Recibido);
                    Msg_Recibido = Variable_frame;
                    if (Dato_Recibido[0].IndexOf("Ok") >= 0)
                    {
                        Env.Crea_Publicidad_Detalle(myCurrent.idbas, myCurrent.gpo, Msg_Recibido.Split(chr), false, true);
                        Enviado = Enviado + reg_leido;
                    }
                    if (Dato_Recibido[0].IndexOf("Error") >= 0)
                    {
                        Env.Guardar_Trama_pendiente(myCurrent.idbas, myCurrent.gpo, strcomando);
                        NoEnviado = NoEnviado + reg_leido;
                        ERROR = true;
                    }
                    pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[255, Variable.idioma] + " " + myCurrent.Nserie + "... ");

                    reg_leido = 0;
                    Variable_frame = "";
                }
            }

            return ERROR;
        }
        private bool Enviar_publicidad(int reg_total, ref ProgressContinue pro)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msg_Recibido;
            string[] Dato_Recibido = null;
            string strcomando;
            string Variable_frame = null;
            int TotalNoEnviados = 0;
            int TotalEnviados = 0;
            int TotalBorrado = 0;
            int reg_leido = 0;
            int reg_envio = 0;
            bool ERROR = false;

            serialPort1 = new SerialPort();

            if (listViewDatos.Items.Count > 0)
            {
                if (SR.OpenPort(ref serialPort1, myCurrent.COMM, myCurrent.BAUD))  //Variable.P_COMM, Variable.Buad))            
                {
                    SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)9 + (char)10);

                    pro.IniciaProcess(reg_total, Variable.SYS_MSJ[255, Variable.idioma] + " " + myCurrent.Nserie + "... ");

                    Variable_frame = "";
                    for (int nreg = 0; nreg < listViewDatos.Items.Count; nreg++)
                    {
                        if (listViewDatos.Items[nreg].Selected)
                        {
                            DataRow DR = baseDeDatosDataSet.Publicidad.Rows.Find(Convert.ToInt32(listViewDatos.Items[nreg].SubItems[0].Text));
                            if (!Convert.ToBoolean(DR["borrado"]))
                            {
                                if (LidPend.BinarySearch(Convert.ToInt32(DR["id_publicidad"].ToString())) < 0)
                                {
                                    LidPend.Add(Convert.ToInt32(DR["id_publicidad"].ToString()));
                                }
                                Variable_frame = Variable_frame + Env.Genera_Trama_Publicidad(DR);
                                reg_leido++;
                                if (reg_leido > 4)
                                {
                                    reg_envio = reg_envio + reg_leido;
                                    strcomando = "GM" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;

                                    SR.SendCOMSerial(ref serialPort1, strcomando, ref Dato_Recibido);
                                    if (Dato_Recibido != null)
                                    {
                                        Msg_Recibido = Variable_frame;
                                        if (Dato_Recibido[0].IndexOf("Ok") >= 0)
                                        {
                                            Env.Crea_Publicidad_Detalle(myCurrent.idbas, myCurrent.gpo, Msg_Recibido.Split(chr), false, true);
                                            TotalEnviados = TotalEnviados + reg_leido;
                                        }
                                        if (Dato_Recibido[0].IndexOf("Error") >= 0)
                                        {
                                            Env.Guardar_Trama_pendiente(myCurrent.idbas, myCurrent.gpo, strcomando);
                                            TotalNoEnviados = TotalNoEnviados + reg_leido;
                                            ERROR = true;
                                        }
                                        pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[255, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                                    }
                                    reg_leido = 0;
                                    Variable_frame = "";
                                }
                            }
                        }
                    }
                    if (Variable_frame.Length > 0 && reg_leido <= 4)
                    {
                        reg_envio = reg_envio + reg_leido;
                        strcomando = "GM" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;

                        SR.SendCOMSerial(ref serialPort1, strcomando, ref Dato_Recibido);
                        if (Dato_Recibido != null)
                        {
                            Msg_Recibido = Variable_frame;
                            if (Dato_Recibido[0].IndexOf("Ok") >= 0)
                            {
                                Env.Crea_Publicidad_Detalle(myCurrent.idbas, myCurrent.gpo, Msg_Recibido.Split(chr), false, true);
                                TotalEnviados = TotalEnviados + reg_leido;
                            }
                            if (Dato_Recibido[0].IndexOf("Error") >= 0)
                            {
                                Env.Guardar_Trama_pendiente(myCurrent.idbas, myCurrent.gpo, strcomando);
                                TotalNoEnviados = TotalNoEnviados + reg_leido;
                                ERROR = true;
                            }
                            pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[255, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                        }
                        reg_leido = 0;
                        Variable_frame = "";
                    }

                    Enviar_Borrado_publicidad(ref serialPort1, ref TotalBorrado, myCurrent.idbas, myCurrent.gpo);
                    SR.SendCOMSerial(ref serialPort1, "dXXX" + (char)9 + (char)10);
                    SR.ClosePort(ref serialPort1);

                }
                else
                {
                    ERROR = true;
                }
            }

            return ERROR;
        }

        private bool Enviar_InfoAdicional(string direccionIP, ref Socket Cliente_bascula, ref int Enviado, ref int NoEnviado, ref ProgressContinue pro)
        {
            ingredientesTableAdapter.Fill(baseDeDatosDataSet.Ingredientes);

            char[] chr = new char[2] { (char)10, (char)13 };
            string Msg_Recibido;
            string Variable_frame = null;
            int reg_leido = 0;
            int reg_envio = 0;
            List<Int32> idPubl = new List<Int32>();
            bool ERROR = false;

            for (int i = 0; i < listViewDatos.SelectedItems.Count; i++)
            {
                Int32 ing1 = Convert.ToInt32(listViewDatos.SelectedItems[i].SubItems[18].Text);

                if (!Convert.ToBoolean(listViewDatos.SelectedItems[i].SubItems[20].Text))
                {
                    if (idPubl.BinarySearch(ing1) < 0 && ing1 > 0)
                    {
                        idPubl.Add(ing1);
                    }
                }
            }

            Variable_frame = "";

            if (idPubl.Count > 0)
            {
                pro.IniciaProcess(idPubl.Count, Variable.SYS_MSJ[256, Variable.idioma] + " " + myCurrent.Nserie + "... ");  //"Enviando Info. Adicional","Iniciando proceso");

                for (int nreg = 0; nreg < idPubl.Count; nreg++)
                {
                    DataRow DI = baseDeDatosDataSet.Ingredientes.Rows.Find(idPubl[nreg]);  //DPI["id_ingrediente"]);
                    if (DI != null)
                    {
                        if (LidInfo.BinarySearch(Convert.ToInt32(DI["id_ingrediente"].ToString())) < 0)
                        {
                            LidInfo.Add(Convert.ToInt32(DI["id_ingrediente"].ToString()));
                        }
                        Variable_frame = Variable_frame + Env.Genera_Trama_InfoAdicional(DI);
                        reg_leido++;
                        if (reg_leido > 4)
                        {
                            reg_envio = reg_envio + reg_leido;

                            Msg_Recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, myCurrent.idbas, myCurrent.gpo, "GI");
                            if (Msg_Recibido != null)
                            {
                                Env.Crear_InfoAdd_detalle(myCurrent.idbas, myCurrent.gpo, Msg_Recibido.Split(chr), false, true);
                                Enviado = Enviado + reg_leido;
                            }
                            else
                            {
                                NoEnviado = NoEnviado + reg_leido;
                                ERROR = true;
                            }

                            pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[256, Variable.idioma] + " " + myCurrent.Nserie + "... ");

                            reg_leido = 0;
                            Variable_frame = "";
                        }
                    }
                }
                if (Variable_frame.Length > 0 && reg_leido <= 4)
                {
                    reg_envio = reg_envio + reg_leido;

                    Msg_Recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, myCurrent.idbas, myCurrent.gpo, "GI");
                    if (Msg_Recibido != null)
                    {
                        Env.Crear_InfoAdd_detalle(myCurrent.idbas, myCurrent.gpo, Msg_Recibido.Split(chr), false, true);
                        Enviado = Enviado + reg_leido;
                    }
                    else
                    {
                        NoEnviado = NoEnviado + reg_leido;
                        ERROR = true;
                    }
                    pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[256, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                    reg_leido = 0;
                    Variable_frame = "";
                }
            }

            return ERROR;
        }
        private bool Enviar_InfoAdicional(string direccionIP, int reg_total, ref ProgressContinue pro)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msg_Recibido;
            string Variable_frame = null;
            int reg_leido = 0;
            int reg_envio = 0;
            int TotalEnviados = 0;
            int TotalNoEnviados = 0;
            int TotalBorrado = 0;
            bool ERROR = false;

            if (listViewDatos.SelectedItems.Count > 0)
            {
                Cliente_bascula = Cte.conectar(direccionIP, 50036);
                if (Cliente_bascula != null)
                {
                    pro.IniciaProcess(reg_total, Variable.SYS_MSJ[256, Variable.idioma] + " " + myCurrent.Nserie + "... ");  //"Enviando Info. Adicional", "Iniciando proceso");

                    string sComando = "XX" + (char)9 + (char)10;

                    Msg_Recibido = Env.Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");
                    if (Msg_Recibido != null)
                    {
                        Variable_frame = "";
                        for (int nreg = 0; nreg < listViewDatos.Items.Count; nreg++)
                        {
                            if (listViewDatos.Items[nreg].Selected)
                            {
                                DataRow DR = baseDeDatosDataSet.Ingredientes.Rows.Find(Convert.ToInt32(listViewDatos.Items[nreg].SubItems[0].Text));
                                if (!Convert.ToBoolean(DR["borrado"]))
                                {
                                    if (LidPend.BinarySearch(Convert.ToInt32(DR["id_ingrediente"].ToString())) < 0)
                                    {
                                        LidPend.Add(Convert.ToInt32(DR["id_ingrediente"].ToString()));
                                    }
                                    Variable_frame = Variable_frame + Env.Genera_Trama_InfoAdicional(DR);
                                    reg_leido++;
                                    if (reg_leido > 4)
                                    {
                                        reg_envio = reg_envio + reg_leido;

                                        Msg_Recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, myCurrent.idbas, myCurrent.gpo, "GI");
                                        if (Msg_Recibido != null)
                                        {
                                            Env.Crear_InfoAdd_detalle(myCurrent.idbas, myCurrent.gpo, Msg_Recibido.Split(chr), false, true);
                                            TotalEnviados = TotalEnviados + reg_leido;
                                        }
                                        else
                                        {
                                            TotalNoEnviados = TotalNoEnviados + reg_leido;
                                            ERROR = true;
                                        }
                                        pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[256, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                                        reg_leido = 0;
                                        Variable_frame = "";
                                    }
                                }
                            }
                        }
                        if (Variable_frame.Length > 0 && reg_leido <= 4)
                        {
                            reg_envio = reg_envio + reg_leido;

                            Msg_Recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, myCurrent.idbas, myCurrent.gpo, "GI");
                            if (Msg_Recibido != null)
                            {
                                Env.Crear_InfoAdd_detalle(myCurrent.idbas, myCurrent.gpo, Msg_Recibido.Split(chr), false, true);
                                TotalEnviados = TotalEnviados + reg_leido;
                            }
                            else
                            {
                                TotalNoEnviados = TotalNoEnviados + reg_leido;
                                ERROR = true;
                            }
                            pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[256, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                            reg_leido = 0;
                            Variable_frame = "";
                        }

                    }
                    Enviar_Borrado_Ingredientes(direccionIP, ref Cliente_bascula, ref TotalBorrado, myCurrent.idbas, myCurrent.gpo);
                    Cte.desconectar(ref Cliente_bascula);
                   
                }
                else
                {
                    ERROR = true;
                }
            }

            return ERROR;
        }
        private bool Enviar_InfoAdicional(ref SerialPort Cliente_bascula, ref int Enviado, ref int NoEnviado, ref ProgressContinue pro)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msg_Recibido;
            string Variable_frame = null;
            string[] Dato_Recibido = null;
            string strcomando;
            bool ERROR = false;

            int reg_leido = 0;
            int reg_envio = 0;
            List<Int32> idPubl = new List<Int32>();

            for (int i = 0; i < listViewDatos.SelectedItems.Count; i++)
            {
                Int32 ing1 = Convert.ToInt32(listViewDatos.SelectedItems[i].SubItems[18].Text);

                if (idPubl.BinarySearch(ing1) < 0 && ing1 > 0)
                {
                    idPubl.Add(ing1);
                }
            }

            ingredientesTableAdapter.Fill(baseDeDatosDataSet.Ingredientes);

            Variable_frame = "";

            if (idPubl.Count > 0)
            {

                pro.IniciaProcess(idPubl.Count, Variable.SYS_MSJ[256, Variable.idioma] + " " + myCurrent.Nserie + "... ");  //"Enviando Info. Adicional","Iniciando proceso");

                for (int nreg = 0; nreg < idPubl.Count; nreg++)
                {
                    DataRow DI = baseDeDatosDataSet.Ingredientes.Rows.Find(idPubl[nreg]);
                    if (DI != null)
                    {
                        if (LidInfo.BinarySearch(Convert.ToInt32(DI["id_ingrediente"].ToString())) < 0)
                        {
                            LidInfo.Add(Convert.ToInt32(DI["id_ingrediente"].ToString()));
                        }
                        Variable_frame = Variable_frame + Env.Genera_Trama_InfoAdicional(DI);
                        reg_leido++;
                        if (reg_leido > 4)
                        {
                            reg_envio = reg_envio + reg_leido;
                            strcomando = "GI" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;

                            SR.SendCOMSerial(ref serialPort1, strcomando, ref Dato_Recibido);
                            Msg_Recibido = Variable_frame;  //.Substring(4);
                            if (Dato_Recibido[0].IndexOf("Ok") >= 0)
                            {
                                Env.Crear_InfoAdd_detalle(myCurrent.idbas, myCurrent.gpo, Msg_Recibido.Split(chr), false, true);
                                Enviado = Enviado + reg_leido;
                            }
                            if (Dato_Recibido[0].IndexOf("Error") >= 0)
                            {
                                Env.Guardar_Trama_pendiente(myCurrent.idbas, myCurrent.gpo, strcomando);
                                NoEnviado = NoEnviado + reg_leido;
                                ERROR = true;
                            }

                            pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[256, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                            reg_leido = 0;
                            Variable_frame = "";
                        }
                    }
                }
                if (Variable_frame.Length > 0 && reg_leido <= 4)
                {
                    reg_envio = reg_envio + reg_leido;
                    strcomando = "GI" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;

                    SR.SendCOMSerial(ref serialPort1, strcomando, ref Dato_Recibido);
                    Msg_Recibido = Variable_frame;  //.Substring(4);
                    if (Dato_Recibido[0].IndexOf("Ok") >= 0)
                    {
                        Env.Crear_InfoAdd_detalle(myCurrent.idbas, myCurrent.gpo, Msg_Recibido.Split(chr), false, true);
                        Enviado = Enviado + reg_leido;
                    }
                    if (Dato_Recibido[0].IndexOf("Error") >= 0)
                    {
                        Env.Guardar_Trama_pendiente(myCurrent.idbas, myCurrent.gpo, strcomando);
                        NoEnviado = NoEnviado + reg_leido;
                        ERROR = true;
                    }
                    pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[256, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                    reg_leido = 0;
                    Variable_frame = "";
                }
            }

            return ERROR;
        }
        private bool Enviar_InfoAdicional(int reg_total, ref ProgressContinue pro)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msg_Recibido;
            string Variable_frame = null;
            string[] Dato_Recibido = null;
            string strcomando;
            int TotalEnviados = 0;
            int TotalNoEnviados = 0;
            int TotalBorrado = 0;
            int reg_leido = 0;
            int reg_envio = 0;
            bool ERROR = false;

            serialPort1 = new SerialPort();

            if (listViewDatos.Items.Count > 0)
            {
                if (SR.OpenPort(ref serialPort1, myCurrent.COMM, myCurrent.BAUD))  // Variable.P_COMM, Variable.Buad))            
                {
                    SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)9 + (char)10);

                    pro.IniciaProcess(reg_total, Variable.SYS_MSJ[256, Variable.idioma] + " " + myCurrent.Nserie + "... ");  //"Enviando Info. Adicional", "Iniciando proceso");

                    Variable_frame = "";
                    for (int nreg = 0; nreg < listViewDatos.Items.Count; nreg++)
                    {
                        if (listViewDatos.Items[nreg].Selected)
                        {
                            DataRow DR = baseDeDatosDataSet.Ingredientes.Rows.Find(Convert.ToInt32(listViewDatos.Items[nreg].SubItems[0].Text));
                            if (!Convert.ToBoolean(DR["borrado"]))
                            {
                                if (LidPend.BinarySearch(Convert.ToInt32(DR["id_ingrediente"].ToString())) < 0)
                                {
                                    LidPend.Add(Convert.ToInt32(DR["id_ingrediente"].ToString()));
                                }
                                Variable_frame = Variable_frame + Env.Genera_Trama_InfoAdicional(DR);
                                reg_leido++;
                                if (reg_leido > 4)
                                {
                                    reg_envio = reg_envio + reg_leido;
                                    strcomando = "GI" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;

                                    SR.SendCOMSerial(ref serialPort1, strcomando, ref Dato_Recibido);
                                    if (Dato_Recibido != null)
                                    {
                                        Msg_Recibido = Variable_frame;
                                        if (Dato_Recibido[0].IndexOf("Ok") >= 0)
                                        {
                                            Env.Crear_InfoAdd_detalle(myCurrent.idbas, myCurrent.gpo, Msg_Recibido.Split(chr), false, true);
                                            TotalEnviados = TotalEnviados + reg_leido;
                                        }
                                        if (Dato_Recibido[0].IndexOf("Error") >= 0)
                                        {
                                            Env.Guardar_Trama_pendiente(myCurrent.idbas, myCurrent.gpo, strcomando);
                                            TotalNoEnviados = TotalNoEnviados + reg_leido;
                                            ERROR = true;
                                        }
                                        pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[256, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                                    }
                                    reg_leido = 0;
                                    Variable_frame = "";
                                }
                            }
                        }
                    }
                    if (Variable_frame.Length > 0 && reg_leido <= 4)
                    {
                        reg_envio = reg_envio + reg_leido;
                        strcomando = "GI" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;

                        SR.SendCOMSerial(ref serialPort1, strcomando, ref Dato_Recibido);
                        if (Dato_Recibido != null)
                        {
                            Msg_Recibido = Variable_frame;
                            if (Dato_Recibido[0].IndexOf("Ok") >= 0)
                            {
                                Env.Crear_InfoAdd_detalle(myCurrent.idbas, myCurrent.gpo, Msg_Recibido.Split(chr), false, true);
                                TotalEnviados = TotalEnviados + reg_leido;
                            }
                            if (Dato_Recibido[0].IndexOf("Error") >= 0)
                            {
                                Env.Guardar_Trama_pendiente(Num_Bascula, Num_Grupo, strcomando);
                                TotalNoEnviados = TotalNoEnviados + reg_leido;
                                ERROR = true;
                            }
                            pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[256, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                        }
                        reg_leido = 0;
                        Variable_frame = "";
                    }

                    Enviar_Borrado_Ingredientes(ref serialPort1, ref TotalBorrado, myCurrent.idbas, myCurrent.gpo);
                    SR.SendCOMSerial(ref serialPort1, "dXXX" + (char)9 + (char)10);
                    SR.ClosePort(ref serialPort1);

                    //MessageBox.Show(this, TotalNoEnviados.ToString() + " " + Variable.SYS_MSJ[282, Variable.idioma] + (char)13 + TotalEnviados.ToString() + " " + Variable.SYS_MSJ[281, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ERROR = true;
                }
            }

            return ERROR;
        }

        private bool Envia_Ofertas(string direccionIP, ref Socket Cliente_bascula, ref int Enviado, ref int NoEnviado, ref ProgressContinue pro)
        {
            ofertaTableAdapter.Fill(baseDeDatosDataSet.Oferta);
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msg_Recibido;
            string Variable_frame = null;
            int reg_leido = 0;
            int reg_envio = 0;
            List<Int32> idPubl = new List<Int32>();
            bool ERROR = false;

            for (int i = 0; i < listViewDatos.SelectedItems.Count; i++)
            {
                Int32 ing1 = Convert.ToInt32(listViewDatos.SelectedItems[i].SubItems[19].Text);
                if (!Convert.ToBoolean(listViewDatos.SelectedItems[i].SubItems[20].Text))
                {
                    if (idPubl.BinarySearch(ing1) < 0 && ing1 > 0)
                    {
                        idPubl.Add(ing1);
                    }
                }
            }
            
            Variable_frame = "";

            if (idPubl.Count > 0)
            {

                pro.IniciaProcess(idPubl.Count, Variable.SYS_MSJ[257, Variable.idioma] + " " + myCurrent.Nserie + "... ");  //"Enviando Ofertas","Inciando proceso");

                for (int nreg = 0; nreg < idPubl.Count; nreg++)
                {
                    DataRow DO = baseDeDatosDataSet.Oferta.Rows.Find(idPubl[nreg]);
                    if (DO != null)
                    {
                        if (LidOfer.BinarySearch(Convert.ToInt32(DO["id_oferta"].ToString())) < 0)
                        {
                            LidOfer.Add(Convert.ToInt32(DO["id_oferta"].ToString()));
                        }
                        Variable_frame = Variable_frame + Env.Genera_Trama_Oferta(DO, Variable.nsucursal.ToString());
                        reg_leido++;
                        if (reg_leido > 4)
                        {
                            reg_envio = reg_envio + reg_leido;
                            Msg_Recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, myCurrent.idbas, myCurrent.gpo, "GO");
                            if (Msg_Recibido != null)
                            {
                                Env.Crear_Oferta_Detalle(myCurrent.idbas, myCurrent.gpo, Msg_Recibido.Split(chr), false, true);
                                Enviado = Enviado + reg_leido;
                            }
                            else
                            {
                                NoEnviado = NoEnviado + reg_leido;
                                ERROR = true;
                            }
                            pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[257, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                            reg_leido = 0;
                            Variable_frame = "";
                        }
                    }
                }
                if (Variable_frame.Length > 0 && reg_leido <= 4)
                {
                    reg_envio = reg_envio + reg_leido;

                    Msg_Recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, myCurrent.idbas, myCurrent.gpo, "GO");
                    if (Msg_Recibido != null)
                    {
                        Env.Crear_Oferta_Detalle(myCurrent.idbas, myCurrent.gpo, Msg_Recibido.Split(chr), false, true);
                        Enviado = Enviado + reg_leido;
                    }
                    else
                    {
                        NoEnviado = NoEnviado + reg_leido;
                        ERROR = true;
                    }
                    pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[257, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                    reg_leido = 0;
                    Variable_frame = "";
                }
            }

            return ERROR;
        }
        private bool Envia_Ofertas(string direccionIP, int reg_total, ref ProgressContinue pro)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msg_Recibido;
            string Variable_frame = null;
            int reg_leido = 0;
            int reg_envio = 0;
            int TotalEnviados = 0;
            int TotalNoEnviados = 0;
            int TotalBorrado = 0;
            bool ERROR = false;

            if (listViewDatos.SelectedItems.Count > 0)
            {
                Cliente_bascula = Cte.conectar(direccionIP, 50036);
                if (Cliente_bascula != null)
                {
                    pro.IniciaProcess(reg_total, Variable.SYS_MSJ[257, Variable.idioma] + " " + myCurrent.Nserie + "... ");  //"Enviando Ofertas", "Iniciando proceso");

                    string sComando = "XX" + (char)9 + (char)10;

                    Msg_Recibido = Env.Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");
                    if (Msg_Recibido != null)
                    {
                        Variable_frame = "";
                        for (int nreg = 0; nreg < listViewDatos.Items.Count; nreg++)
                        {
                            if (listViewDatos.Items[nreg].Selected)
                            {
                                DataRow DR = baseDeDatosDataSet.Oferta.Rows.Find(Convert.ToInt32(listViewDatos.Items[nreg].SubItems[0].Text));
                                if (!Convert.ToBoolean(DR["borrado"]))
                                {
                                    if (LidPend.BinarySearch(Convert.ToInt32(DR["id_oferta"].ToString())) < 0)
                                    {
                                        LidPend.Add(Convert.ToInt32(DR["id_oferta"].ToString()));
                                    }
                                    Variable_frame = Variable_frame + Env.Genera_Trama_Oferta(DR, myCurrent.gpo.ToString());
                                    reg_leido++;
                                    if (reg_leido > 4)
                                    {
                                        reg_envio = reg_envio + reg_leido;
                                        Msg_Recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, myCurrent.idbas, myCurrent.gpo, "GO");
                                        if (Msg_Recibido != null)
                                        {
                                            Env.Crear_Oferta_Detalle(myCurrent.idbas, myCurrent.gpo, Msg_Recibido.Split(chr), false, true);
                                            TotalEnviados = TotalEnviados + reg_leido;
                                        }
                                        else
                                        {
                                            TotalNoEnviados = TotalNoEnviados + reg_leido;
                                            ERROR = true;
                                            
                                        }
                                        pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[257, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                                        reg_leido = 0;
                                        Variable_frame = "";
                                    }
                                }
                            }
                        }
                        if (Variable_frame.Length > 0 && reg_leido <= 4)
                        {
                            reg_envio = reg_envio + reg_leido;

                            Msg_Recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, myCurrent.idbas, myCurrent.gpo, "GO");
                            if (Msg_Recibido != null)
                            {
                                Env.Crear_Oferta_Detalle(myCurrent.idbas, myCurrent.gpo, Msg_Recibido.Split(chr), false, true);
                                TotalEnviados = TotalEnviados + reg_leido;
                            }
                            else
                            {
                                TotalNoEnviados = TotalNoEnviados + reg_leido;
                                ERROR = true;
                            }
                            pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[257, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                            reg_leido = 0;
                            Variable_frame = "";
                        }

                    }

                    Enviar_Borrado_Ofertas(direccionIP, ref Cliente_bascula, ref TotalBorrado, myCurrent.idbas, myCurrent.gpo);
                    Cte.desconectar(ref Cliente_bascula);

                    //MessageBox.Show(this, TotalNoEnviados.ToString() + " " + Variable.SYS_MSJ[282, Variable.idioma] + (char)13 + TotalEnviados.ToString() + " " + Variable.SYS_MSJ[281, Variable.idioma] + (char)13 + TotalBorrado.ToString() + " " + Variable.SYS_MSJ[283, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ERROR = true;
                }
            }

            return ERROR;
        }
        private bool Envia_Ofertas(ref SerialPort Cliente_bascula, ref int Enviado, ref int NoEnviado, ref ProgressContinue pro)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msg_Recibido;
            string Variable_frame = null;
            string[] Dato_Recibido = null;
            string strcomando;
            int reg_leido = 0;
            int reg_envio = 0;
            List<Int32> idPubl = new List<Int32>();
            bool ERROR = false;

            for (int i = 0; i < listViewDatos.SelectedItems.Count; i++)
            {
                Int32 ing1 = Convert.ToInt32(listViewDatos.SelectedItems[i].SubItems[19].Text);

                if (idPubl.BinarySearch(ing1) < 0 && ing1 > 0)
                {
                    idPubl.Add(ing1);
                }
            }
            
            Variable_frame = "";

            if (idPubl.Count > 0)
            {
                pro.IniciaProcess(idPubl.Count, Variable.SYS_MSJ[257, Variable.idioma] + " " + myCurrent.Nserie + "... ");  //"Enviando Ofertas","Inciando proceso");

                for (int nreg = 0; nreg < idPubl.Count; nreg++)
                {
                    DataRow DO = baseDeDatosDataSet.Oferta.Rows.Find(idPubl[nreg]);
                    if (DO != null)
                    {
                        if (LidOfer.BinarySearch(Convert.ToInt32(DO["id_oferta"].ToString())) < 0)
                        {
                            LidOfer.Add(Convert.ToInt32(DO["id_oferta"].ToString()));
                        }
                        Variable_frame = Variable_frame + Env.Genera_Trama_Oferta(DO, myCurrent.gpo.ToString());
                        reg_leido++;
                        if (reg_leido > 4)
                        {
                            reg_envio = reg_envio + reg_leido;
                            strcomando = "GO" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;

                            SR.SendCOMSerial(ref serialPort1, strcomando, ref Dato_Recibido);
                            if (Dato_Recibido != null)
                            {
                                Msg_Recibido = Variable_frame;
                                if (Dato_Recibido[0].IndexOf("Ok") >= 0)
                                {
                                    Env.Crear_Oferta_Detalle(myCurrent.idbas, myCurrent.gpo, Msg_Recibido.Split(chr), false, true);
                                    Enviado = Enviado + reg_leido;
                                }
                                if (Dato_Recibido[0].IndexOf("Error") >= 0)
                                {
                                    Env.Guardar_Trama_pendiente(Num_Bascula, Num_Grupo, strcomando);
                                    NoEnviado = NoEnviado + reg_leido;
                                    ERROR = true;
                                }
                                pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[257, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                            }
                            reg_leido = 0;
                            Variable_frame = "";
                        }
                    }
                }
                if (Variable_frame.Length > 0 && reg_leido <= 4)
                {
                    reg_envio = reg_envio + reg_leido;

                    strcomando = "GO" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;

                    SR.SendCOMSerial(ref serialPort1, strcomando, ref Dato_Recibido);
                    if (Dato_Recibido != null)
                    {
                        Msg_Recibido = Variable_frame;
                        if (Dato_Recibido[0].IndexOf("Ok") >= 0)
                        {
                            Env.Crear_Oferta_Detalle(myCurrent.idbas, myCurrent.gpo, Msg_Recibido.Split(chr), false, true);
                            Enviado = Enviado + reg_leido;
                        }
                        if (Dato_Recibido[0].IndexOf("Error") >= 0)
                        {
                            Env.Guardar_Trama_pendiente(Num_Bascula, Num_Grupo, strcomando);
                            NoEnviado = NoEnviado + reg_leido;
                            ERROR = true;
                        }
                        pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[257, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                    }
                    reg_leido = 0;
                    Variable_frame = "";
                }
            }

            return ERROR;
        }
        private bool Envia_Ofertas(int reg_total, ref ProgressContinue pro)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msg_Recibido;
            string[] Dato_Recibido = null;
            string strcomando;
            string Variable_frame = null;
            int TotalNoEnviados = 0;
            int TotalEnviados = 0;
            int TotalBorrado = 0;
            int reg_leido = 0;
            int reg_envio = 0;
            serialPort1 = new SerialPort();
            bool ERROR = false;

            if (listViewDatos.SelectedItems.Count > 0)
            {
                if (SR.OpenPort(ref serialPort1, myCurrent.COMM, myCurrent.BAUD))
                {
                    SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)9 + (char)10);
                                        
                    pro.IniciaProcess(reg_total, Variable.SYS_MSJ[257, Variable.idioma] + " " + myCurrent.Nserie + "... ");  //"Enviando Ofertas", "Iniciando proceso");

                    Variable_frame = "";
                    for (int nreg = 0; nreg < listViewDatos.Items.Count; nreg++)
                    {
                        if (listViewDatos.Items[nreg].Selected)
                        {
                            DataRow DR = baseDeDatosDataSet.Oferta.Rows.Find(Convert.ToInt32(listViewDatos.Items[nreg].SubItems[0].Text));
                            if (!Convert.ToBoolean(DR["borrado"]))
                            {
                                if (LidPend.BinarySearch(Convert.ToInt32(DR["id_oferta"].ToString())) < 0)
                                {
                                    LidPend.Add(Convert.ToInt32(DR["id_oferta"].ToString()));
                                }
                                Variable_frame = Variable_frame + Env.Genera_Trama_Oferta(DR, myCurrent.gpo.ToString());
                                reg_leido++;
                                if (reg_leido > 4)
                                {
                                    reg_envio = reg_envio + reg_leido;
                                    strcomando = "GO" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                                    
                                    SR.SendCOMSerial(ref serialPort1, strcomando, ref Dato_Recibido);
                                    Msg_Recibido = Variable_frame;  
                                    if (Dato_Recibido[0].IndexOf("Ok") >= 0)
                                    {
                                        Env.Crear_Oferta_Detalle(myCurrent.idbas, myCurrent.gpo, Msg_Recibido.Split(chr), false, true);
                                        TotalEnviados = TotalEnviados + reg_leido;
                                    }
                                    if (Dato_Recibido[0].IndexOf("Error") >= 0)
                                    {
                                        Env.Guardar_Trama_pendiente(myCurrent.idbas, myCurrent.gpo, strcomando);
                                        TotalNoEnviados = TotalNoEnviados + reg_leido;
                                        ERROR = true;
                                    }
                                    pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[257, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                                    reg_leido = 0;
                                    Variable_frame = "";
                                }
                            }                          
                        }
                    }
                    if (Variable_frame.Length > 0 && reg_leido <= 4)
                    {
                        reg_envio = reg_envio + reg_leido;
                        strcomando = "GO" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                        
                        SR.SendCOMSerial(ref serialPort1, strcomando, ref Dato_Recibido);
                        Msg_Recibido = Variable_frame;  
                        if (Dato_Recibido[0].IndexOf("Ok") >= 0)
                        {
                            Env.Crear_Oferta_Detalle(myCurrent.idbas, myCurrent.gpo, Msg_Recibido.Split(chr), false, true);
                            TotalEnviados = TotalEnviados + reg_leido;
                        }
                        if (Dato_Recibido[0].IndexOf("Error") >= 0)
                        {
                            Env.Guardar_Trama_pendiente(myCurrent.idbas, myCurrent.gpo, strcomando);
                            TotalNoEnviados = TotalNoEnviados + reg_leido;
                            ERROR = true;
                        }
                        pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[257, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                        reg_leido = 0;
                        Variable_frame = "";
                    }
                    
                    Enviar_Borrado_Ofertas(ref serialPort1, ref TotalBorrado, myCurrent.idbas, myCurrent.gpo);
                    SR.SendCOMSerial(ref serialPort1, "dXXX" + (char)9 + (char)10);
                    SR.ClosePort(ref serialPort1);

                    //MessageBox.Show(this, TotalNoEnviados.ToString() + " " + Variable.SYS_MSJ[282, Variable.idioma] + (char)13 + TotalEnviados.ToString() + " " + Variable.SYS_MSJ[281, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ERROR = true;
                }
            }

            return ERROR;
        }

        private bool Envia_Imagenes(int reg_total, ref ProgressContinue pro)
        {
            int iRtaFunct = 0;
            int posicion = 0;
            string ImagenAEnviar = "";
            string[] dato_imagen = new string[1];           
            int TotalEnviados = 0;
            int TotalNoEnviados = 0;
            CommandTorrey myobj = new CommandTorrey();
            bool ERROR = false;

            if (listViewDatos.SelectedItems.Count > 0)
            {                                               
                pro.IniciaProcess(reg_total, Variable.SYS_MSJ[252, Variable.idioma] + " " + myCurrent.Nserie + "... ");  //"Enviando Imagenes", "Iniciando proceso");

                 SerialPort serialPort1 = new SerialPort();

                 if (SR.OpenPort(ref serialPort1, myCurrent.COMM, 115200))
                {
                    SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)9 + (char)10);
                    SR.ReceivedCOMSerial(ref serialPort1);
                    SR.ClosePort(ref serialPort1);
                }

                for (int nreg = 0; nreg < listViewDatos.Items.Count; nreg++)
                {
                    if (listViewDatos.Items[nreg].Selected)
                    {
                        if (listViewDatos.Items[nreg].SubItems[2].Text.Length > 0)
                        {
                            dato_imagen[0] = listViewDatos.Items[nreg].SubItems[4].Text;
                            posicion = listViewDatos.Items[nreg].SubItems[2].Text.LastIndexOf('\\');
                            if (posicion > 0)
                            {
                                ImagenAEnviar = listViewDatos.Items[nreg].SubItems[2].Text;

                                iRtaFunct = myobj.TORREYSendImagesToScaleSerial(myCurrent.COMM, ImagenAEnviar, "Product");

                                if (iRtaFunct > 0 && iRtaFunct != 4)
                                {
                                    pro.UpdateProcess(1, Variable.SYS_MSJ[276, Variable.idioma] + " " + ImagenAEnviar + "... ");
                                    Env.Guardar_Trama_pendiente(myCurrent.idbas, myCurrent.gpo, ImagenAEnviar);
                                    TotalNoEnviados++;
                                    ERROR = true;

                                }
                                else
                                {
                                    pro.UpdateProcess(1, Variable.SYS_MSJ[275, Variable.idioma] + " " + ImagenAEnviar + "... ");
                                    TotalEnviados++;
                                }
                            }
                        }
                    }
                }

                serialPort1 = new SerialPort();

                if (SR.OpenPort(ref serialPort1, myCurrent.COMM, 115200))
                {
                    SR.SendCOMSerial(ref serialPort1, "DXXX" + (char)9 + (char)10);
                    SR.ReceivedCOMSerial(ref serialPort1);
                    SR.ClosePort(ref serialPort1);
                }

                //MessageBox.Show(this, TotalNoEnviados.ToString() + " " + Variable.SYS_MSJ[282, Variable.idioma] + (char)13 + TotalEnviados.ToString() + " " + Variable.SYS_MSJ[281, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return ERROR;
        }

        private void Actualizas_pendientes_productos(Int32 idprod)
        {
            Conec.CadenaSelect = "UPDATE Productos SET pendiente= " + false + " WHERE ( id_producto = " + idprod + ")";
            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "Productos");
        }

        private void Actualizas_pendientes_ingredientes(Int32 idprod)
        {
            Conec.CadenaSelect = "UPDATE Ingredientes SET pendiente= " + false + " WHERE ( id_ingrediente = " + idprod + ")";
            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "Ingredientes");
        }
        private void Actualizas_pendientes_ofertas(Int32 idprod)
        {
            Conec.CadenaSelect = "UPDATE Oferta SET pendiente= " + false + " WHERE ( id_oferta = " + idprod + ")";
            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "Oferta");
        }
        private void Actualizas_pendientes_publicidad(Int32 idprod)
        {
            Conec.CadenaSelect = "UPDATE Publicidad SET pendiente= " + false + " WHERE ( id_publicidad = " + idprod + ")";
            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "Publicidad");
        }
        private void Actualizas_pendientes_vendedores(Int32 idprod)
        {
            Conec.CadenaSelect = "UPDATE Vendedor SET pendiente= " + false + " WHERE ( id_vendedor = " + idprod + ")";
            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "Vendedor");
        }

        private void Eliminando_Productos_Borrados(Int32 idprod)
        {
            bool existe;

            Conec.Condicion = "id_producto = " + idprod +" and id_bascula = " + Num_Bascula + "and id_grupo = " + Num_Grupo;

            System.Data.OleDb.OleDbDataReader DbRead = Conec.Obtiene_Dato("Select * From Prod_detalle Where (" + Conec.Condicion + ")", Conec.CadenaConexion);
            if (DbRead.Read()) existe = true;
            else existe = false;
            DbRead.Close();

            if (existe)
            {
                Conec.CadenaSelect = "DELETE * FROM Prod_detalle WHERE (" + Conec.Condicion + ")";
                Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Prod_detalle.TableName);
            }
            try
            {
                System.Data.OleDb.OleDbDataReader DbRead2 = Conec.Obtiene_Dato("Select * From Prod_detalle Where (id_producto = " + idprod + ")", Conec.CadenaConexion);
                if (DbRead2.Read()) existe = true;
                else existe = false;
                DbRead.Close();

                if (!existe)
                {

                    Conec.Condicion = "id_producto = " + idprod;

                    Conec.CadenaSelect = "DELETE * FROM Productos WHERE (" + Conec.Condicion + ")";
                    Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Productos.TableName);
                }
            }
            catch (System.Data.OleDb.OleDbException myException)
            {
                Console.WriteLine(myException.Message);
            }
        }
        private void Eliminando_Ingredientes_Borrados(Int32 idingre)
        {
            try
            {
                Conec.Condicion = "id_ingrediente = " + idingre;

                Conec.CadenaSelect = "DELETE * FROM Ingredientes WHERE (" + Conec.Condicion + ")";
                Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Ingredientes.TableName);
            }
            catch (System.Data.OleDb.OleDbException myException)
            {
                Console.WriteLine(myException.Message);
            }
        }
        private void Eliminando_Ofertas_Borrados(Int32 idofer)
        {
            try
            {
                Conec.Condicion = "id_oferta = " + idofer;

                Conec.CadenaSelect = "DELETE * FROM Ofertas WHERE (" + Conec.Condicion + ")";
                Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Oferta.TableName);
            }
            catch (System.Data.OleDb.OleDbException myException)
            {
                Console.WriteLine(myException.Message);
            }
        }
        private void Eliminando_Publicidad_Borrados(Int32 idpubl)
        {
            try
            {
                Conec.Condicion = "id_publicidad = " + idpubl;

                Conec.CadenaSelect = "DELETE * FROM Publicidad WHERE (" + Conec.Condicion + ")";
                Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Publicidad.TableName);
            }
            catch (System.Data.OleDb.OleDbException myException)
            {
                Console.WriteLine(myException.Message);
            }
        }
        private void Eliminando_Vendedores_Borrados(Int32 idvend)
        {
            try
            {
                Conec.Condicion = "id_vendedor = " + idvend;

                Conec.CadenaSelect = "DELETE * FROM Vendedor WHERE (" + Conec.Condicion + ")";
                Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Vendedor.TableName);
            }
            catch (System.Data.OleDb.OleDbException myException)
            {
                Console.WriteLine(myException.Message);
            }
        }

        #endregion

        private bool vEnvia_Imagenes(int iIdBascula, int iIdGrupo, string direccionIP, ref ProgressContinue pro)
        {
            List<string> mycollection = new List<string>();
            string ImagenAEnviar = "";
            bool ERROR = false;

            if (listViewDatos.SelectedItems.Count > 0)
            {
                for (int nreg = 0; nreg < listViewDatos.Items.Count; nreg++)
                {
                    if (listViewDatos.Items[nreg].Selected)
                    {
                        if (listViewDatos.Items[nreg].SubItems[2].Text.Length > 0)
                        {
                            int posicion = listViewDatos.Items[nreg].SubItems[2].Text.LastIndexOf('\\');
                            if (posicion > 0)
                            {
                                ImagenAEnviar = listViewDatos.Items[nreg].SubItems[2].Text;
                                mycollection.Add(ImagenAEnviar);
                            }
                        }
                    }
                }
            }

            // Create the thread object. This does not start the thread.
            if (mycollection.Count > 0)
            {
                Worker workerObject = new Worker();

                //Thread t = new Thread(() =>
                //{
                    ERROR = workerObject.vSendImageThread(iIdBascula, iIdGrupo, direccionIP, mycollection, null, ref pro);
                //});

                //t.Start();

                //while (!t.IsAlive) ;

                //t.Join();

                Console.WriteLine("main thread: Worker thread has terminated.");
            }

            return ERROR;
        }
        private bool vEnvia_Imagenes(int iIdBascula, int iIdGrupo, string direccionIP, Socket Cliente_bascula, ref int Enviados, ref int NoEnviado, ref ProgressContinue pro)
        {
            List<string> mycollection = new List<string>();
            string ImagenAEnviar = "";
            bool ERROR = false;

            /*-------------------------------------------------------------------------*/
            int reg_total = listViewDatos.SelectedItems.Count;

            for (int nreg = 0; nreg < listViewDatos.Items.Count; nreg++)
            {
                if (listViewDatos.Items[nreg].Selected)
                {

                    DataRow DR = baseDeDatosDataSet.Productos.Rows.Find(Convert.ToInt32(listViewDatos.Items[nreg].SubItems[15].Text));

                    ImagenAEnviar = DR["imagen1"].ToString();

                    if (ImagenAEnviar.LastIndexOf('\\') > 0)
                    {
                        mycollection.Add(ImagenAEnviar);
                    }
                }
            }

            /*-------------------------------------------------------------------------*/

            // Create the thread object. This does not start the thread.
            if (mycollection.Count > 0)
            {
                Worker workerObject = new Worker();

                //Thread t = new Thread(() =>
                //{
                ERROR  = workerObject.vSendImageThread(iIdBascula, iIdGrupo, direccionIP, mycollection, Cliente_bascula, ref pro);
                //});

                //t.Start();

                //while (!t.IsAlive) ;

                //t.Join();

                Console.WriteLine("main thread: Worker thread has terminated.");
            }

            return ERROR;
        }   
    }

    public class Worker
    {
        Conexion Cte = new Conexion();
        Envia_Dato Env = new Envia_Dato();

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

        // This method will be called when the thread is started. 
        public bool vSendImageThread(int iIdBascula, int iIdGrupo, string direccionIP, List<string> myCollection, Socket Cliente_bascula, ref ProgressContinue pro)
        {
            int iRtaFunct = 0;
            string ImagenAEnviar = "";
            string Msg_Recibido;
            string[] dato_imagen = new string[1];
            int TotalEnviados = 0;
            int TotalNoEnviados = 0;
            int iStartConection = 0;
            bool ERROR = false;

            CommandTorrey myobj = new CommandTorrey();

            if (myCollection.Count > 0)
            {                
                //ProgressContinue pro = new ProgressContinue();
                pro.IniciaProcess(myCollection.Count, Variable.SYS_MSJ[252, Variable.idioma] + " " +  direccionIP + "... ");

                if (Cliente_bascula == null)
                {
                    Cliente_bascula = Cte.conectar(direccionIP, 50036);
                    iStartConection = 1;
                }

                if (Cliente_bascula != null)
                {

                    string sComando = "XX" + (char)9 + (char)10;
                    Msg_Recibido = Env.Command_Enviado(1, sComando, direccionIP, ref Cliente_bascula, 0, 0, "bX");
                    if (Msg_Recibido != null)
                    {

                        for (int nreg = 0; nreg < myCollection.Count; nreg++)
                        {
                            ImagenAEnviar = myCollection[nreg];

                            iRtaFunct = myobj.TORREYSendImagesToScale(direccionIP, ImagenAEnviar, "Product", Cliente_bascula);

                            pro.UpdateProcess(1, Variable.SYS_MSJ[252, Variable.idioma] + " " + direccionIP + "... ");

                            if (iRtaFunct > 0)
                            {
                                if (iRtaFunct != 4) Env.Guardar_Trama_pendiente(iIdBascula, iIdGrupo, ImagenAEnviar);
                                TotalNoEnviados++;
                                ERROR = true;
                            }
                            else
                            {
                                TotalEnviados++;
                            }
                        }
                    }

                    if (iStartConection == 1)
                    {
                        Cte.desconectar(ref Cliente_bascula);
                        //MessageBox.Show(this, TotalNoEnviados.ToString() + " " + Variable.SYS_MSJ[282, Variable.idioma] + (char)13 + TotalEnviados.ToString() + " " + Variable.SYS_MSJ[281, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                        TotalNoEnviados = 0;
                    }

                }
                else
                {
                    ERROR =  true;
                }                
            }

            return ERROR;
        }

        public string Command_Enviado(int reg_leido, string Trama_Enviada, string direccionIP, ref Socket Cliente_bascula, long bascula, long grupo, string comando)
        {
            string[] Dato_Recibido = null;
            string reg_enviado = "", strcomando = Trama_Enviada;
            string Msj_Recibido = null;

            strcomando = comando + reg_leido.ToString().PadLeft(2, '0') + strcomando;

            Cte.Envio_Dato(ref Cliente_bascula, direccionIP, strcomando, ref Dato_Recibido);
            if (Dato_Recibido != null)
            {
                reg_enviado = strcomando.Substring(4);
                if (Dato_Recibido[0].IndexOf("Error") >= 0)
                {
                    Msj_Recibido = null;
                }
                if (Dato_Recibido[0].IndexOf("Ok") >= 0)
                {
                    Msj_Recibido = reg_enviado;
                }
            }
            else Msj_Recibido = null;

            return Msj_Recibido;
        }
    }
}


