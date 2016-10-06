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
using Validaciones;

namespace MainMenu
{
    public partial class UserOrdenar : UserControl
    {
        #region Declaracion de variables y costantes

        public Int32 Num_Grupo;
        public Int32 Num_Bascula;
        public String Nombre_Select;

        private TreeNodeBound StartNode = null;
        private Point Position;
        private TreeNodeBound OriginalNode = null;
        private ContextMenu MenuBotton1 = new ContextMenu();
        private bool exist_nodo = false;
        private Int32 Carpeta_Activa = 0;
        private bool l_plu = false;
        private string[] tipo_prod = new string[] { "No Pesable", "Pesable" };

        Variable.lgrupo[] myGrupo;
        Variable.lbasc[] myScale;
        Variable.nodo_actual myCurrent;
        ESTADO.EstadoRegistro RegistroEstado = new ESTADO.EstadoRegistro();
        ADOutil Conec = new ADOutil();
        Conexion Cte = new Conexion();
        ArrayList Dato_Nuevo = new ArrayList();
        Socket Cliente_bascula = null;
        CheckSum CkSum = new CheckSum();
        Envia_Dato Env = new Envia_Dato();
        Serial SR = new Serial();

        #endregion
        #region Inicializacion
        public UserOrdenar()
        {
            InitializeComponent();
            this.toolStripLabel2.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + string.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);
            this.tvwCarpetas.AllowDrop = true;
            this.tvwCarpetas.TabIndex = 0;
            this.tvwCarpetas.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwCarpetas_AfterSelect);
            this.tvwCarpetas.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvwCarpetas_DragDrop);
            this.tvwCarpetas.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvwCarpetas_DragEnter);
            this.tvwCarpetas.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvwCarpetas_ItemDrag);
            this.tvwCarpetas.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvwCarpetas_KeyDown);
            this.tvwCarpetas.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvwCarpetas_AfterLabelEdit);
            this.listViewProductos.AllowDrop = true;
            this.listViewProductos.ItemDrag += new ItemDragEventHandler(listViewProductos_ItemDrag);
            this.listViewProductos.DragEnter += new DragEventHandler(listViewProductos_DragEnter);
            this.listViewProductos.DragOver += new DragEventHandler(listViewProductos_DragOver);
            this.listViewProductos.DragLeave += new EventHandler(listViewProductos_DragLeave);
            this.listViewProductos.DragDrop += new DragEventHandler(listViewProductos_DragDrop);
            this.listViewProductos.KeyDown += new KeyEventHandler(listViewProductos_KeyDown);
            this.listViewProductos.LostFocus += listViewProductos_LostFocus;
            StripGuardar.Enabled = false;
        }
        void listViewProductos_LostFocus(object sender, EventArgs e)
        {
            /*if (StripGuardar.Enabled)
            {
                DialogResult df = MessageBox.Show(this, Variable.SYS_MSJ[342, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (df != DialogResult.Yes)
                {
                    this.listViewProductos.Focus();
                }
            }*/
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
        /// <summary>
        /// Este metodo es llamado para crear Un producto de detalle,que indica el orden quen que los productos seran enviados a la bascula.
        /// Este metodo agrega un producto a la vez a la base de datos.
        /// </summary>
        /// <param name="bascula">Bascula a la que se enviarar la informacion.</param>
        /// <param name="sucursal">Sucursal en la que esta la bascula</param>
        /// <param name="carpeta">Carpeta donde estan los productos</param>
        /// <param name="DatosNuevos">Datos del producto que se va a agregar.</param>
        private void Crear_DetalleProducto(long bascula, long sucursal, long carpeta, string[] DatosNuevos)
        {
            DataRow dr = baseDeDatosDataSet.Prod_detalle.NewRow();

            dr.BeginEdit();
            dr["id_bascula"] = bascula;
            dr["id_grupo"] = sucursal;
            dr["id_carpeta"] = carpeta;
            dr["id_producto"] = DatosNuevos[0];
            dr["codigo"] = DatosNuevos[1];
            dr["NoPLU"] = DatosNuevos[2];
            dr["posicion"] = DatosNuevos[3];
            dr["pendiente"] = true;
            dr.EndEdit();

            prod_detalleTableAdapter.Update(dr);
            baseDeDatosDataSet.Prod_detalle.AcceptChanges();

            Conec.CadenaSelect = "INSERT INTO Prod_detalle " +
            "(id_bascula,id_grupo,id_carpeta,id_producto, codigo, NoPLU, pendiente,posicion)" +
           "VALUES (" + bascula + "," +   //id_bascula
             sucursal + "," +   //id_grupo  
             carpeta + "," +     // id_carpeta
             Convert.ToInt32(DatosNuevos[0]) + "," +     // id_producto
             Convert.ToInt32(DatosNuevos[1]) + "," +  //codigo
             Convert.ToInt32(DatosNuevos[2]) + "," + //NoPLU
             true + "," + Convert.ToInt32(DatosNuevos[3]) + ")"; //posicion

            Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Prod_detalle.TableName);
        }
        /// <summary>
        /// Este metodo permite actualizar el estatus y la pocision de un producto en especifico. 
        /// Tambien funciona como un producto a la vez.
        /// </summary>
        /// <param name="bascula">Bascula a la que pertenece ese producto.</param>
        /// <param name="sucursal">Sucursal en la que esta la bascula.</param>
        /// <param name="carpeta">Carpeta donde se encuentra el producto.</param>
        /// <param name="producto">Producto que se va a modificar</param>
        /// <param name="posicion">Procision que se dara al producto.</param>
        private void Modifica_DetalleProducto(long bascula, long sucursal, long carpeta, long producto, int posicion)
        {
            Conec.CadenaSelect = "id_bascula = " + bascula + " and id_grupo = " + sucursal + " and id_carpeta = " + carpeta + " and id_producto = " + producto;

            Conec.CadenaSelect = "UPDATE Prod_detalle SET" +
            " pendiente = " + true +  // pendiente
            ", posicion = " + posicion + // posicion
            " WHERE (" + Conec.CadenaSelect + ")"; 
             
            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Prod_detalle.TableName);
        }
        private void Ver_Productos_en_Carpetas(long n_carpeta)
        {
            this.Cursor = Cursors.WaitCursor;

            this.listViewProductos.Visible = true;
            this.listViewProductos.BringToFront();
            this.listViewProductos.View = View.Details;
            this.listViewProductos.FullRowSelect = true;
            this.listViewProductos.GridLines = true;
            this.listViewProductos.LabelEdit = false;
            this.listViewProductos.HideSelection = false;
            this.listViewProductos.InsertionMark.Color = Color.Green;
            this.listViewProductos.ListViewItemSorter = new ListViewIndexComparer();
            this.listViewProductos.LabelEdit = true;

            this.listViewProductos.Clear();
            this.listViewProductos.Columns.Add(new ColHeader(Variable.SYS_MSJ[389,Variable.idioma], 80, HorizontalAlignment.Left, false));  //posicion                       
            this.listViewProductos.Columns.Add(Variable.SYS_MSJ[386, Variable.idioma], 100, HorizontalAlignment.Left);  //codigo / ID
            this.listViewProductos.Columns.Add(Variable.SYS_MSJ[387, Variable.idioma], 100, HorizontalAlignment.Left);  //PLU /ID_parent
            this.listViewProductos.Columns.Add(Variable.SYS_MSJ[163, Variable.idioma], 400, HorizontalAlignment.Left); //nombre           
            this.listViewProductos.Columns.Add(Variable.SYS_MSJ[388,Variable.idioma], 130, HorizontalAlignment.Left);  //Tipo de producto
                   
            carpeta_detalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            carpeta_detalleTableAdapter.Connection.CreateCommand().CommandText = "SELECT * FROM carpeta_detalle WHERE (ID_padre = " + n_carpeta + " AND id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo + ")";
            carpeta_detalleTableAdapter.Fill(baseDeDatosDataSet.carpeta_detalle);

            DataRow[] DR_Folder = baseDeDatosDataSet.carpeta_detalle.Select("ID_padre = " + n_carpeta + " and id_bascula = " + Num_Bascula + "and id_grupo = " + Num_Grupo, "posicion");
           
            foreach (DataRow DA in DR_Folder)
            {
                ListViewItem lwitem = new ListViewItem(DA["posicion"].ToString().ToString(), 0);  //0
                lwitem.SubItems.Add(DA["ID"].ToString());
                lwitem.SubItems.Add(DA["ID_padre"].ToString()); //1
                lwitem.SubItems.Add(DA["Nombre"].ToString().Trim()); //2
                lwitem.SubItems.Add(DA["ruta"].ToString()); //3
                lwitem.SubItems.Add(DA["ID"].ToString()); //11   
                lwitem.SubItems.Add(DA["tabla"].ToString()); //11  
                this.listViewProductos.Items.Add(lwitem);
            }

            prod_detalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            prod_detalleTableAdapter.Connection.CreateCommand().CommandText = "SELECT * FROM Prod_detalle WHERE (id_carpeta = " + n_carpeta + " AND id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo + ") Order by posicion";
            prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);

            DataRelation Producto_Detalle = baseDeDatosDataSet.Relations["ProductosProd_detalle"];

            foreach (DataRow DA in baseDeDatosDataSet.Productos.Rows)
            {
                foreach (DataRow PR in DA.GetChildRows(Producto_Detalle))
                {
                    if (Convert.ToInt32(PR["id_carpeta"].ToString()) == n_carpeta && Convert.ToInt32(PR["id_bascula"].ToString()) == Num_Bascula && Convert.ToInt32(PR["id_grupo"].ToString()) == Num_Grupo)
                    {
                        ListViewItem lwitem = new ListViewItem(PR["posicion"].ToString(), 1);  //0
                        lwitem.SubItems.Add(PR["codigo"].ToString()); //0
                        lwitem.SubItems.Add(PR["NoPlu"].ToString()); //1
                        lwitem.SubItems.Add(DA["Nombre"].ToString().Trim()); //2
                        lwitem.SubItems.Add(tipo_prod[Convert.ToInt16(DA["TipoId"].ToString())]); //6
                        lwitem.SubItems.Add(PR["id_producto"].ToString()); //11     
                        lwitem.SubItems.Add("P"); //11     
                        this.listViewProductos.Items.Add(lwitem);                        
                    }
                }
            }

            if (listViewProductos.Items.Count > 0)
            {
                Order_Ascending();
                StripEnviar.Enabled = true;
                StripGuardar.Enabled = false;
            }
            this.Cursor = Cursors.Default;
        }
        private void Order_Ascending()
        {
            ColHeader clickedCol = (ColHeader)this.listViewProductos.Columns[0];

            clickedCol.ascending = !clickedCol.ascending;

            int numItems = this.listViewProductos.Items.Count;

            this.listViewProductos.BeginUpdate();

            ArrayList SortArray = new ArrayList();
            for (int i = 0; i < numItems; i++)
            {
                SortArray.Add(new SortWrapper(this.listViewProductos.Items[i], 0));
            }

            SortArray.Sort(0, SortArray.Count, new SortWrapper.SortComparer(clickedCol.ascending));

            this.listViewProductos.Items.Clear();
            for (int i = 0; i < numItems; i++)
            {
                this.listViewProductos.Items.Add(((SortWrapper)SortArray[i]).sortItem);
            }
            this.listViewProductos.EndUpdate();
        }
        private void Reordenar_Datos(int posicion, int elemento, int IdCarpeta)
        {
            for (int k = elemento; k < listViewProductos.Items.Count; k++)
            {                
                listViewProductos.Items[k].Text = posicion.ToString();
                posicion++;                
            }
        }
        private class ListViewIndexComparer : System.Collections.IComparer
        {
            public int Compare(object x, object y)
            {
                return ((ListViewItem)x).Index - ((ListViewItem)y).Index;
            }
        }
        private void Borrar_Productos_en_Carpetas(int n_carpeta)
        {
            DataRow[] Borrar_Prod = baseDeDatosDataSet.Prod_detalle.Select("id_carpeta =" + n_carpeta + " and id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);

            if (Borrar_Prod.Length > 0)
            {
                foreach (DataRow PrB in Borrar_Prod)
                {
                    PrB.Delete();
                    baseDeDatosDataSet.Prod_detalle.AcceptChanges();
                }
            }
            Conec.Condicion = "id_carpeta = " + n_carpeta + " and id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo;
            Conec.CadenaSelect = "DELETE * FROM Prod_detalle WHERE (" + Conec.Condicion + ")";
            Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Prod_detalle.TableName);

            prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);
        }
        private void Borrar_Productos_en_Carpetas(int n_carpeta, int n_producto)
        {
            DataRow[] Borrar_Prod = baseDeDatosDataSet.Prod_detalle.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo + " and id_carpeta =" + n_carpeta + " and id_producto = " + n_producto);

            if (Borrar_Prod.Length > 0)
            {
                foreach (DataRow PrB in Borrar_Prod)
                {
                    PrB.Delete();
                    baseDeDatosDataSet.Prod_detalle.AcceptChanges();
                }

                Conec.Condicion = "id_bascula =" + Num_Bascula + "and id_grupo = " + Num_Grupo + " and id_carpeta = " + n_carpeta + " and id_producto = " + n_producto;
                Conec.CadenaSelect = "DELETE * FROM Prod_detalle WHERE (" + Conec.Condicion + ")";
                Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Prod_detalle.TableName);
            }
            prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);
        }
        /// <summary>
        /// Elimina un producto de la formacion de productos.
        /// </summary>
        /// <param name="n_producto"></param>
        private void Borrar_DetalleProducto(int n_producto)
        {
            DataRow[] Borrar_Prod = baseDeDatosDataSet.Prod_detalle.Select("id_producto =" + n_producto + " and id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);
            if (Borrar_Prod.Length > 0)
            {
                foreach (DataRow PrB in Borrar_Prod)
                {
                    PrB.Delete();
                    baseDeDatosDataSet.Prod_detalle.AcceptChanges();
                }
            }
            Conec.Condicion = "id_producto = " + n_producto + " and id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo;
            Conec.CadenaSelect = "DELETE * FROM Prod_detalle WHERE (" + Conec.Condicion + ")";
            Conec.EliminarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Prod_detalle.TableName);
            prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);
        }             
        private void Crear_Carpetas(TreeNodeBound subnodo)
        {
            Carpetas dat = new Carpetas();
            Carpetas.numfold = Convert.ToInt32(dat.muestraAutoincrementoId());
            Carpetas.nomfold = "Nueva Carpeta";

            if (Carpetas.numfold > 0)
            {
                if (dat.Nuevo_Folder(Carpetas.numfold, Carpetas.nomfold, Convert.ToInt32(subnodo.Value), Convert.ToInt16(subnodo.Index)))
                {
                    DataRow dr = baseDeDatosDataSet.carpeta_detalle.NewRow();
                    dr.BeginEdit();
                    dr["id_bascula"] = Num_Bascula;
                    dr["id_grupo"] = Num_Grupo;
                    dr["ID"] = Carpetas.numfold;
                    dr["ID_padre"] = Convert.ToInt32(subnodo.Value);
                    dr["Nombre"] = Carpetas.nomfold;
                    dr["ruta"] = subnodo.FullPath;
                    dr.EndEdit();
                    carpeta_detalleTableAdapter.Update(dr);
                    baseDeDatosDataSet.carpeta_detalle.AcceptChanges();
                    Crear_Nodos_Carpetas();
                }
            }
        }
        #endregion
        #region ListView y treeView
        private void listViewProductos_DoubleClick(object sender, EventArgs e)
        {
            System.Windows.Forms.ListView lv_item = (System.Windows.Forms.ListView)sender;
            
            if (lv_item.FocusedItem.SubItems[6].Text == "C")
            {
                Carpeta_Activa = Convert.ToInt32(lv_item.FocusedItem.SubItems[5].Text);
                if (Convert.ToInt32(lv_item.FocusedItem.SubItems[5].Text) > 0)
                {
                    this.Ver_Productos_en_Carpetas(Carpeta_Activa);
                    RegistroEstado = ESTADO.EstadoRegistro.PKTRATADO;
                }
            }
        }
        private void listViewProductos_DragLeave(object sender, EventArgs e)
        {
            listViewProductos.InsertionMark.Index = -1;
        }
        private void listViewProductos_DragOver(object sender, DragEventArgs e)
        {
            Point targetPoint = listViewProductos.PointToClient(new Point(e.X, e.Y));

             int targetIndex = listViewProductos.InsertionMark.NearestIndex(targetPoint);

             if (targetIndex > -1)
            {
                Rectangle itemBounds = listViewProductos.GetItemRect(targetIndex);
                if (targetPoint.X > itemBounds.Left + (itemBounds.Width / 2))
                {
                    listViewProductos.InsertionMark.AppearsAfterItem = true;
                }
                else
                {
                    listViewProductos.InsertionMark.AppearsAfterItem = false;
                }
            }

            listViewProductos.InsertionMark.Index = targetIndex;
        }
        private void listViewProductos_ItemDrag(object sender, ItemDragEventArgs e)
        {
            System.Windows.Forms.ListView lv_item = (System.Windows.Forms.ListView)sender;
            object strItem = lv_item;
            l_plu = true;
            listViewProductos.DoDragDrop(e.Item, DragDropEffects.Move);
            listViewProductos.LabelEdit = true;
        }
        private void listViewProductos_DragDrop(object sender, DragEventArgs e)
        {
            int ini_pos = 0;
            int ini_ele = 0;

            Int32 IdCarpeta = Convert.ToInt32(tvwCarpetas.SelectedValue);
            
            System.Windows.Forms.ListView lv_item = (System.Windows.Forms.ListView)sender;

            int sourceIndex = lv_item.FocusedItem.Index;

            int targetIndex = listViewProductos.InsertionMark.Index;

            if (targetIndex == -1) { return; }

            if (listViewProductos.InsertionMark.AppearsAfterItem) { targetIndex++; }
            
            ListViewItem draggedItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));

            if (targetIndex > sourceIndex)
            {
                ini_pos = Convert.ToInt32(draggedItem.Text);
                ini_ele = sourceIndex;
            }
            else
            {
                ini_pos = Convert.ToInt32(targetIndex) + 1;
                ini_ele = targetIndex + 1;
            }
            draggedItem.Text = Convert.ToString(targetIndex);

            listViewProductos.Items.Insert(targetIndex, (ListViewItem)draggedItem.Clone());

            listViewProductos.Items.Remove(draggedItem);
            
            Reordenar_Datos(ini_pos, ini_ele, IdCarpeta);
            
            listViewProductos.InsertionMark.Index = -1;
            StripEnviar.Enabled = false;
            StripGuardar.Enabled = true;
            listViewProductos.LabelEdit = false;
        }
        private void listViewProductos_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            int ini_pos = 0;
            int ini_ele = 0;

            Int32 IdCarpeta = Convert.ToInt32(tvwCarpetas.SelectedValue);

            System.Windows.Forms.ListView lv_item = (System.Windows.Forms.ListView)sender;

            int sourceIndex = lv_item.FocusedItem.Index;

            int targetIndex = Convert.ToInt32(e.Label); 

            if (targetIndex == -1) { return; }

            lv_item.FocusedItem.Text = e.Label;

            ListViewItem draggedItem = (ListViewItem)lv_item.FocusedItem; //listViewProductos.Items[e.Item].Clone();

            if (targetIndex > sourceIndex)
            {
                ini_pos = e.Item;
                ini_ele = sourceIndex;
            }
            else
            {
                ini_pos = Convert.ToInt32(targetIndex) + 1;
                ini_ele = targetIndex + 1;
            }

           // draggedItem.Text = Convert.ToString(targetIndex);
            e.CancelEdit = true;

            listViewProductos.Items.Insert(targetIndex, (ListViewItem)draggedItem.Clone());

            listViewProductos.Items.Remove(draggedItem);
                        
            Reordenar_Datos(ini_pos, ini_ele, IdCarpeta);

            StripEnviar.Enabled = false;
            StripGuardar.Enabled = true;
            listViewProductos.LabelEdit = false;
        }
        private void listViewProductos_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }
        private void listViewProductos_KeyDown(object sender, KeyEventArgs e)
        {           

            if (e.KeyCode == Keys.Delete && RegistroEstado == ESTADO.EstadoRegistro.PKTRATADO)
            {
                Int32 IdCarpeta = Convert.ToInt32(tvwCarpetas.SelectedValue);
                Int32 IdProducto;

                DialogResult op = MessageBox.Show(this, Variable.SYS_MSJ[232, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (op == DialogResult.Yes)
                {
                    foreach (ListViewItem prod_sele in listViewProductos.SelectedItems)
                    {
                        if (prod_sele.SubItems[6].Text == "P")
                        {
                            IdProducto = Convert.ToInt32(prod_sele.SubItems[6].Text);
                            Borrar_Productos_en_Carpetas(IdCarpeta, IdProducto);
                        }
                    }
                    Ver_Productos_en_Carpetas(IdCarpeta);
                }
            }
            if (e.KeyCode == Keys.Escape)
            {
                object nodo = find_node_padre(Carpeta_Activa);

                if (nodo != null && nodo.ToString() != "")
                {
                    Carpeta_Activa = Convert.ToInt32(nodo);
                    Ver_Productos_en_Carpetas(Carpeta_Activa);
                }
            }
            if (e.KeyCode == Keys.F2)
            {
                //Editar la posicion.
                listViewProductos.LabelEdit = true;
            }
        }                            
      
        
        private void tvwCarpetas_DragDrop(object sender, DragEventArgs e)
        {
            object[] clave = new object[4];
            string[] datos = new string[5];
            int Nposicion = 0;

            Position.X = e.X;
            Position.Y = e.Y;
            Position = this.tvwCarpetas.PointToClient(Position);

            TreeViewBound.TreeNodeBound DropNode = (TreeViewBound.TreeNodeBound)this.tvwCarpetas.GetNodeAt(Position);
            TreeViewBound.TreeNodeBound DragNode = ((TreeViewBound.TreeNodeBound)e.Data.GetData("TreeViewBound.TreeNodeBound"));
            TreeViewBound.TreeNodeBound NodeMove = ((TreeViewBound.TreeNodeBound)e.Data.GetData("TreeViewBound.TreeNodeBound"));
            if (!l_plu)
            {
                if (DropNode == null)
                {
                    if (this.tvwCarpetas.GetNodeCount(false) < 500)
                    {
                        Cambiar_carpeta_de_Nodo(-1, Convert.ToInt32(NodeMove.Value));
                        DragNode.Remove();
                        this.tvwCarpetas.Nodes.Add(NodeMove);
                    }
                    else MessageBox.Show(this, Variable.SYS_MSJ[279, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma]);
                }
                else
                {
                    if (NodeMove != null)
                    {
                        if (Convert.ToInt32(DropNode.Value) != Convert.ToInt32(NodeMove.Value))
                        {
                            this.exist_nodo = false;
                            if (!find_node_tree(NodeMove, DropNode))
                            {
                                if (DropNode.Nodes.Count < 500)
                                {
                                    Cambiar_carpeta_de_Nodo(Convert.ToInt32(DropNode.Value), Convert.ToInt32(NodeMove.Value));
                                    DragNode.Remove();
                                    DropNode.Nodes.Add(NodeMove);
                                }
                                else MessageBox.Show(this, Variable.SYS_MSJ[279, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma]);

                            }
                        }
                    }
                }
            }
            else
            {    // Agrega productos seleccionados a la carpeta seleccionada
                OriginalNode = ((TreeViewBound.TreeNodeBound)DropNode);
                if (OriginalNode != null)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Conec.CadenaSelect = "SELECT * FROM Prod_detalle WHERE (id_carpeta = " + Convert.ToInt32(OriginalNode.Value) + " and id_grupo = " + Num_Grupo + " and id_bascula = " + Num_Bascula + ") ORDER BY id_producto";

                    prod_detalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
                    prod_detalleTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
                    prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);
                    baseDeDatosDataSet.Prod_detalle.PrimaryKey = new DataColumn[] 

                    {   baseDeDatosDataSet.Prod_detalle.id_basculaColumn,
                        baseDeDatosDataSet.Prod_detalle.id_grupoColumn,
                        baseDeDatosDataSet.Prod_detalle.id_carpetaColumn,
                        baseDeDatosDataSet.Prod_detalle.id_productoColumn
                    };

                    clave[0] = Num_Bascula;
                    clave[1] = Num_Grupo;
                    clave[2] = Convert.ToInt32(OriginalNode.Value);  //numero de carpeta seleccionada
                    int tot_prod_carpeta = baseDeDatosDataSet.Prod_detalle.Select("id_carpeta = " + Convert.ToInt32(OriginalNode.Value) + " and id_grupo = " + Num_Grupo + " and id_bascula = " + Num_Bascula).Length + this.listViewProductos.SelectedItems.Count;
                    if (tot_prod_carpeta < 500)
                    {
                        for (int i = 0; i < this.listViewProductos.SelectedItems.Count; i++)
                        {
                            if (this.listViewProductos.SelectedItems[i].SubItems[7].Text == "P")
                            {
                                clave[3] = Convert.ToInt32(this.listViewProductos.SelectedItems[i].SubItems[6].Text);  //numero de codigo de producto

                                DataRow DT = baseDeDatosDataSet.Prod_detalle.Rows.Find(clave);
                                if (DT == null && i < 500)
                                {
                                    Nposicion++;
                                    datos[0] = this.listViewProductos.SelectedItems[i].SubItems[6].Text;  //id_producto
                                    datos[1] = this.listViewProductos.SelectedItems[i].SubItems[1].Text;  //codigo
                                    datos[2] = this.listViewProductos.SelectedItems[i].SubItems[2].Text;  //NoPLU
                                    //datos[3] = this.listViewProductos.SelectedItems[i].SubItems[5].Text;  //Posicion
                                    datos[3] = Nposicion.ToString();
                                    Crear_DetalleProducto(Num_Bascula, Num_Grupo, Convert.ToInt32(OriginalNode.Value), datos);
                                }
                            }
                        }
                        l_plu = false;
                        while (this.listViewProductos.SelectedItems.Count > 0)
                        {

                            this.listViewProductos.SelectedItems[0].Remove();

                        }
                        Cursor.Current = Cursors.Default;
                        prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);
                        this.listViewProductos.Refresh();
                    }
                    else MessageBox.Show(this, Variable.SYS_MSJ[278, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma]);
                }
            }
        }
        private void tvwCarpetas_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }
        private void tvwCarpetas_KeyDown(object sender, KeyEventArgs e)
        {
            TreeViewBound.TreeViewBound nodo = ((TreeViewBound.TreeViewBound)sender);
            if (!e.Handled)
            {
                if (e.KeyCode == Keys.Delete && Convert.ToInt32(nodo.SelectedValue) > 0)
                {
                    Carpetas dat = new Carpetas();
                    Carpetas.nbascula = Num_Bascula;
                    Carpetas.ngrupo = Num_Grupo;

                    if (Convert.ToInt32(nodo.SelectedValue) > 0)
                    {
                        if (!dat.Buscar_SubFolder(nodo.SelectedValue.ToString()))
                        {
                            if (dat.Buscar_Articulo(Convert.ToInt32(nodo.SelectedValue.ToString()), Num_Bascula, Num_Grupo))//(!dat.Buscar_Articulo(nodo.SelectedValue.ToString()))
                            {
                                if (dat.Del_Folder(nodo.SelectedNode.Text, Convert.ToInt32(nodo.SelectedValue)))
                                { this.tvwCarpetas.SelectedNode.Remove(); }
                            }
                            else
                            {
                                DialogResult op = MessageBox.Show(this, Variable.SYS_MSJ[233, Variable.idioma], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (op == DialogResult.Yes)
                                {
                                    Borrar_Productos_en_Carpetas(Carpetas.numfold);

                                    if (dat.Del_Folder(Carpetas.nomfold, Carpetas.numfold))
                                    {
                                        this.tvwCarpetas.SelectedNode.Remove();
                                    }
                                }
                            }
                        }
                    }
                    e.Handled = true;
                }
                if (e.KeyCode == Keys.Insert && Convert.ToInt32(nodo.SelectedValue) > 0)
                {
                    if (nodo.GetNodeCount(false) < 500)
                    {
                        Crear_Carpetas((TreeNodeBound)nodo.SelectedNode);
                        e.Handled = true;
                    }
                    else MessageBox.Show(this, Variable.SYS_MSJ[279, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma]);
                }
                if (e.KeyCode == Keys.F2 && Convert.ToInt32(nodo.SelectedValue) > 0)
                {
                    if (Convert.ToInt32(nodo.SelectedValue) > 0) tvwCarpetas.LabelEdit = true;
                }
            }
        }
        private void tvwCarpetas_ItemDrag(object sender, ItemDragEventArgs e)
        {
            StartNode = (TreeViewBound.TreeNodeBound)e.Item;

            if (e.Button == MouseButtons.Left)
            {
                object strItem = e.Item;
                DoDragDrop(strItem, DragDropEffects.Move);
            }
        }
        private void tvwCarpetas_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            TreeViewBound.TreeViewBound nodo = ((TreeViewBound.TreeViewBound)sender);
            TreeNodeBound subnodo = (TreeNodeBound)e.Node; // nodo.SelectedNode;
            this.Cambiar_nombre_carpeta(Convert.ToInt32(subnodo.Value), e.Label);
            tvwCarpetas.LabelEdit = false;
        }
        private void tvwCarpetas_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNodeBound subnodo = (TreeNodeBound)e.Node;
            if (Convert.ToInt32(subnodo.Value) >= 0)
            {
                this.Ver_Productos_en_Carpetas(Convert.ToInt32(subnodo.Value));
                RegistroEstado = ESTADO.EstadoRegistro.PKTRATADO;
            }
        }
        private void tvwCarpetas_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(tvwCarpetas.SelectedValue) > 0)
            {
                this.Ver_Productos_en_Carpetas(Convert.ToInt32(tvwCarpetas.SelectedValue));
                RegistroEstado = ESTADO.EstadoRegistro.PKTRATADO;
            }
        }       
        #endregion
        #region Proceso de productos y folder
        private void Cambiar_nombre_carpeta(int numero, string nombre)
        {
            Conec.CadenaSelect = "UPDATE carpeta_detalle SET Nombre = '" + nombre + "' WHERE ( ID = " + numero + " and id_grupo = " + Num_Grupo + " and id_bascula = " + Num_Bascula + ")";
            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "carpeta_detalle");
        }
        private void Cambiar_posicion_carpeta(int numero, int posicion)
        {
            Conec.CadenaSelect = "UPDATE carpeta_detalle SET posicion = " + posicion + " WHERE ( ID = " + numero + " and id_grupo = " + Num_Grupo + " and id_bascula = " + Num_Bascula + ")";
            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "carpeta_detalle");
        }
        private bool Cambiar_carpeta_de_Nodo(int padre, int movido)
        {
            int numero_carpeta = baseDeDatosDataSet.carpeta_detalle.Select("ID_padre = " + padre + " and id_grupo = " + Num_Grupo + " and id_bascula = " + Num_Bascula).Length + 1;
            if (numero_carpeta < 500)
            {
                if (padre >= 0) Conec.CadenaSelect = "UPDATE carpeta_detalle SET ID_padre = " + padre + " WHERE ( ID = " + movido + " and id_grupo = " + Num_Grupo + " and id_bascula = " + Num_Bascula + ")";
                else Conec.CadenaSelect = "UPDATE carpeta_detalle SET ID_padre = null WHERE ( ID = " + movido + " and id_grupo = " + Num_Grupo + " and id_bascula = " + Num_Bascula + ")";

                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "carpeta_detalle");
                return true;
            }
            else return false;
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
        private object find_node_padre(Int32 IDCarpeta)
        {
            object Num_padre = null;

            DataRow[] DR_nodo = baseDeDatosDataSet.carpeta_detalle.Select("id_bascula = " + Num_Bascula + "and id_grupo = " + Num_Grupo + " and ID = " + IDCarpeta);
            if (DR_nodo.Length > 0)
            {
                Num_padre = DR_nodo[0]["ID_padre"].ToString();
            }
            return Num_padre;
        }       
        private void ActualizarChildNodes(TreeNodeCollection ListNodes)
        {
            foreach (TreeNode inode in ListNodes)
            {
                TreeNodeBound N_folder = (TreeNodeBound)inode;

                Conec.CadenaSelect = "UPDATE carpeta_detalle SET ruta = '" + inode.FullPath + "', tabla = 'C' " +
                                         " WHERE (id_bascula = " + Num_Bascula + " AND id_grupo = " + Num_Grupo + " AND ID = " + Convert.ToInt32(N_folder.Value) + ")";

                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "carpeta_detalle");

                if (inode.Nodes.Count > 0)
                {
                    this.ActualizarChildNodes(inode.Nodes);
                }

            }
        }
        #endregion


        private void UserOrdenar_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Prod_detalle' Puede moverla o quitarla según sea necesario.
            this.prod_detalleTableAdapter.Fill(this.baseDeDatosDataSet.Prod_detalle);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Publicidad' Puede moverla o quitarla según sea necesario.
            this.productosTableAdapter.Fill(this.baseDeDatosDataSet.Productos);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Grupo' Puede moverla o quitarla según sea necesario.
            this.grupoTableAdapter.Fill(this.baseDeDatosDataSet.Grupo);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Bascula' Puede moverla o quitarla según sea necesario.
            this.basculaTableAdapter.Fill(this.baseDeDatosDataSet.Bascula);
            this.carpeta_detalleTableAdapter.Fill(this.baseDeDatosDataSet.carpeta_detalle);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.carpeta' Puede moverla o quitarla según sea necesario.

            toolStripLabel3.Text = Nombre_Select;

            Conec.CadenaSelect = "SELECT id_bascula, id_grupo, ID " +
                                 "FROM carpeta_detalle " +
                                 "WHERE (id_bascula = " + Num_Bascula + " AND id_grupo = " + Num_Grupo + ")";

            OleDbDataReader ODR = Conec.Obtiene_Dato(Conec.CadenaSelect, Conec.CadenaConexion);
            if (!ODR.Read())
            {
                ODR.Close();
                Conec.CadenaSelect = "INSERT INTO carpeta_detalle (id_grupo, id_bascula, ID, ID_padre,Nombre)" +
                                     " VALUES ( " + Num_Grupo + "," + Num_Bascula + ",0,null,'/')";
                Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, "carpeta_detalle");
            }
            ODR.Close();
            Crear_Nodos_Carpetas();
            Asigna_Grupo();
            Asigna_Bascula();
        }
        #region contruccion de nodo y opcion de menu y sub menu
        private void Crear_Nodos_Carpetas()
        {
            Cursor.Current = Cursors.WaitCursor;

            Conec.CadenaSelect2 = "SELECT ID, ID_padre, Nombre FROM carpeta_detalle " +
                                "WHERE (id_bascula = " + Num_Bascula + " AND id_grupo = " + Num_Grupo + ") ORDER BY ID";

            DataSet ds_carpeta = new DataSet();
            DataTable dt_carpeta = new DataTable();
            OleDbDataAdapter myDataAdapter4 = new OleDbDataAdapter(Conec.CadenaSelect2, Conec.CadenaConexion);
            myDataAdapter4.Fill(ds_carpeta, "carpeta_detalle");
            dt_carpeta = ds_carpeta.Tables["carpeta_detalle"];
            this.tvwCarpetas.Clear();

            TreeNodeBound raiz = new TreeNodeBound("/");
            raiz.Text = "/";
            raiz.Name = "0";
            raiz.Value = 0;
            tvwCarpetas.Nodes.Add(raiz);

            buildtree(ds_carpeta, raiz, 0);

            ActualizarChildNodes(tvwCarpetas.Nodes);

            tvwCarpetas.ExpandAll();
            Cursor.Current = Cursors.Default;
        }
        private void buildtree(DataSet ds, TreeNode raiz, int p)
        {
            string filter = "ID_padre = " + p;
            DataRow[] rows = ds.Tables["carpeta_detalle"].Select(filter);
            foreach (DataRow row in rows)
            {
                TreeNodeBound nodo = new TreeNodeBound(row["Nombre"].ToString());
                nodo.Text = row["Nombre"].ToString();
                nodo.Name = row["ID"].ToString();
                nodo.Value = Convert.ToInt32(row["ID"]);
                nodo.ParentValue = p;
                raiz.Nodes.Add(nodo);
                buildtree(ds, nodo, Convert.ToInt32(row["ID"]));
            }

        }
        private void soloCarpetasToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int BasculasActualizadas = 0;
            int NumeroDeBaculas = 0;

            try
            {
                carpeta_detalleTableAdapter.Fill(baseDeDatosDataSet.carpeta_detalle);
                prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);

                ProgressContinue pro = new ProgressContinue();
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
                                            Cambiar_Estado_pendiente();
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
                                serialPort1 = new SerialPort();
                                if (SR.OpenPort(ref serialPort1, myCurrent.COMM, myCurrent.BAUD))
                                {
                                    SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)10);
                                    Envia_Borrar_Asociacion(ref serialPort1);
                                    Enviar_DetalleCarpeta_Bascula(ref serialPort1, ref pro);
                                    Enviar_DetalleProducto_Bascula(ref serialPort1, ref pro);
                                    Cambiar_Estado_pendiente();
                                    SR.SendCOMSerial(ref serialPort1, "dXXX" + (char)10);
                                    SR.ClosePort(ref serialPort1);
                                }
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
                                            if (Enviar_DetalleCarpeta_Bascula(myCurrent.ip, ref Cliente_bascula, ref pro))
                                            {
                                                BasculasActualizadas--;
                                                MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma]
                                                    + " " + myCurrent.Nserie + ".", Variable.SYS_MSJ[42, Variable.idioma],
                                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                                break;
                                            }
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
                            else
                            #region Serial
                            {
                                serialPort1 = new SerialPort();

                                if (SR.OpenPort(ref serialPort1, myCurrent.COMM, myCurrent.BAUD))
                                {
                                    SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)10);
                                    Envia_Borrar_Asociacion(ref serialPort1);
                                    Enviar_DetalleCarpeta_Bascula(ref serialPort1, ref pro);
                                    Enviar_DetalleProducto_Bascula(ref serialPort1, ref pro);
                                    Cambiar_Estado_pendiente();
                                    SR.SendCOMSerial(ref serialPort1, "dXXX" + (char)10);
                                    SR.ClosePort(ref serialPort1);
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
        private void StripCerrar_Click(object sender, EventArgs e)
        {
            ToolStripManager.RevertMerge("toolStrip3");
            this.Dispose();
        }
        private void StripGuardar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            Int32 IdCarpeta = Convert.ToInt32(tvwCarpetas.SelectedValue);
            int posicion = 0;

            WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[327, Variable.idioma]);

            Thread t = new Thread(workerObject.vShowMsg);

            t.Start();

            for (int k = 0; k < listViewProductos.Items.Count; k++)
            {
                posicion = Convert.ToInt32(listViewProductos.Items[k].Text);

                if (listViewProductos.Items[k].SubItems[6].Text == "P")
                {
                    Modifica_DetalleProducto(Num_Bascula, Num_Grupo, IdCarpeta, Convert.ToInt32(listViewProductos.Items[k].SubItems[5].Text), posicion);                    
                }
                else
                {
                    Cambiar_posicion_carpeta(Convert.ToInt32(listViewProductos.Items[k].SubItems[5].Text), posicion);
                }
            }

            workerObject.vEndShowMsg();

            Cursor.Current = Cursors.Default;
            StripEnviar.Enabled = true;
            StripGuardar.Enabled = false;
        }
        #endregion
        #region Porcesos de envio a las basculas
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
        private void Envia_Borrar_Asociacion(ref SerialPort serialPort1)
        {
            string Variable_frame;

            Variable_frame = "";
            Variable_frame = "PAXX" + (char)9 + (char)10;

            SR.SendCOMSerial(ref serialPort1, Variable_frame);
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
        private void Enviar_DetalleCarpeta_Bascula(ref SerialPort serialPort1, ref ProgressContinue pro)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            int reg_leido = 0;
            int reg_envio = 0;
            int reg_total;
            string Msj_recibido;
            string Variable_frame = null;
            string[] Dato_recibido = null;
            string strcomando;

            DataRow[] DR = baseDeDatosDataSet.carpeta_detalle.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);

            Variable_frame = "";
            reg_total = DR.Length;

            pro.IniciaProcess(reg_total, Variable.SYS_MSJ[247, Variable.idioma] + " " + myCurrent.Nserie + "... ");

            foreach (DataRow DR_Detail in DR)
            {
                Variable_frame = Variable_frame + Env.Genera_Trama_Carpeta_Detalle(DR_Detail);

                reg_leido++;
                if (reg_leido > 4)
                {
                    reg_envio = reg_envio + reg_leido;
                    Msj_recibido = Variable_frame;
                    Variable_frame = "GA" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                    strcomando = Variable_frame;
                    SR.SendCOMSerial(ref serialPort1, Variable_frame, ref Dato_recibido);
                    if (Dato_recibido != null)
                    {
                        if (Dato_recibido[0].IndexOf("Ok") >= 0)
                        {
                            Msj_recibido = Variable_frame.Substring(4);
                        }
                        if (Dato_recibido[0].IndexOf("Error") >= 0)
                        {
                            Env.Guardar_Trama_pendiente(Num_Bascula, Num_Grupo, strcomando);
                        }
                        pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[247, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                    }
                    reg_leido = 0;
                    Variable_frame = "";
                }
            }
            if (Variable_frame.Length > 0 && reg_leido <= 4)
            {
                reg_envio = reg_envio + reg_leido;
                Msj_recibido = Variable_frame;
                Variable_frame = "GA" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                strcomando = Variable_frame;
                SR.SendCOMSerial(ref serialPort1, Variable_frame, ref Dato_recibido);
                if (Dato_recibido != null)
                {
                    if (Dato_recibido[0].IndexOf("Ok") >= 0)
                    {
                        Msj_recibido = Variable_frame.Substring(4);
                    }
                    if (Dato_recibido[0].IndexOf("Error") >= 0)
                    {
                        Env.Guardar_Trama_pendiente(Num_Bascula, Num_Grupo, strcomando);
                    }
                    pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[247, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                }
                reg_leido = 0;
                Variable_frame = "";
            }
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

            pro.IniciaProcess(reg_total, Variable.SYS_MSJ[248, Variable.idioma] + " " + myCurrent.Nserie + "... ");  //"Enviando Detalle Producto","Iniciando proceso");

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
                        pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[248, Variable.idioma] + " " + myCurrent.Nserie + "... ");
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
                    pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[248, Variable.idioma] + " " + myCurrent.Nserie + "... ");
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
        private void Enviar_DetalleProducto_Bascula(ref SerialPort serialPort1, ref ProgressContinue pro)
        {
            char[] chr = new char[2] { (char)10, (char)13 };
            string Msj_recibido;
            string Variable_frame = null;
            string[] Dato_Recibido = null;
            string strcomando;
            int reg_leido = 0;
            int reg_envio = 0;
            int reg_total;

            DataRow[] DR = baseDeDatosDataSet.Prod_detalle.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);

            Variable_frame = "";
            reg_total = DR.Length;

            pro.IniciaProcess(reg_total, Variable.SYS_MSJ[248, Variable.idioma] + " " + myCurrent.Nserie + "... ");  //"Enviando Detalle Producto","Iniciando proceso");

            foreach (DataRow DR_Detail in DR)
            {
                Variable_frame = Variable_frame + Env.Genera_Trama_Producto_Detalle(DR_Detail);

                reg_leido++;
                if (reg_leido > 4)
                {
                    reg_envio = reg_envio + reg_leido;
                    Msj_recibido = Variable_frame;
                    Variable_frame = "GA" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                    strcomando = Variable_frame;
                    SR.SendCOMSerial(ref serialPort1, Variable_frame, ref Dato_Recibido);
                    if (Dato_Recibido != null)
                    {
                        if (Dato_Recibido[0].IndexOf("Ok") >= 0)
                        {
                            Msj_recibido = Variable_frame.Substring(4);
                        }
                        if (Dato_Recibido[0].IndexOf("Error") >= 0)
                        {
                            Env.Guardar_Trama_pendiente(Num_Bascula, Num_Grupo, strcomando);
                        }
                        pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[248, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                    }
                    reg_leido = 0;
                    Variable_frame = "";
                }
            }
            if (Variable_frame.Length > 0 && reg_leido <= 4)
            {
                reg_envio = reg_envio + reg_leido;
                Msj_recibido = Variable_frame;
                Variable_frame = "GA" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                strcomando = Variable_frame;
                SR.SendCOMSerial(ref serialPort1, Variable_frame, ref Dato_Recibido);
                if (Dato_Recibido != null)
                {
                    if (Dato_Recibido[0].IndexOf("Ok") >= 0)
                    {
                        Msj_recibido = Variable_frame.Substring(4);
                    }
                    if (Dato_Recibido[0].IndexOf("Error") >= 0)
                    {
                        Env.Guardar_Trama_pendiente(Num_Bascula, Num_Grupo, strcomando);
                    }
                    pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[248, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                }
                reg_leido = 0;
                Variable_frame = "";

            }
            Variable_frame = "GAF0" + +(char)9 + (char)10;
            SR.SendCOMSerial(ref serialPort1, Variable_frame);
        }       
        private void Cambiar_Estado_pendiente()
        {
            Conec.CadenaSelect = "UPDATE carpeta_detalle SET pendiente= " + false + " WHERE ( enviado = " + true + ")";
            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "carpeta_detalle");
            Conec.CadenaSelect = "UPDATE Prod_detalle SET pendiente= " + false + " WHERE ( enviado = " + true + ")";
            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "Prod_detalle");
        }
        #endregion

        private void listViewProductos_MouseLeave(object sender, EventArgs e)
        {
            /*if (StripGuardar.Enabled)
            {
                DialogResult df = MessageBox.Show(this, Variable.SYS_MSJ[342, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (df != DialogResult.Yes)
                {
                    this.listViewProductos.Focus();
                }
            }*/
        }
        private void listViewProductos_Leave(object sender, EventArgs e)
        {
            /* if (StripGuardar.Enabled)
              {
                  DialogResult df = MessageBox.Show(this, Variable.SYS_MSJ[342, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                  if (df != DialogResult.Yes)
                  {
                      this.listViewProductos.Focus();
                  }
              }*/
        }            
    }
    public class SortWrapper
    {
        internal ListViewItem sortItem;
        internal int sortColumn;

        public SortWrapper(ListViewItem Item, int iColumn)
        {
            sortItem = Item;
            sortColumn = iColumn;
        }

        public Int16 Text
        {
            get
            {
                return Convert.ToInt16(sortItem.SubItems[sortColumn].Text);
            }
        }

        public class SortComparer : IComparer
        {
            bool ascending;

            public SortComparer(bool asc)
            {
                this.ascending = asc;
            }

            public int Compare(object x, object y)
            {
                SortWrapper xItem = (SortWrapper)x;
                SortWrapper yItem = (SortWrapper)y;

                Int32 xText = Convert.ToInt32(xItem.sortItem.SubItems[xItem.sortColumn].Text);
                Int32 yText = Convert.ToInt32(yItem.sortItem.SubItems[yItem.sortColumn].Text);
                return xText.CompareTo(yText) * (this.ascending ? 1 : -1);
            }
        }
    }
    public class ColHeader : ColumnHeader
    {
        public bool ascending;
        public ColHeader(string text, int width, HorizontalAlignment align, bool asc)
        {
            this.Text = text;
            this.Width = width;
            this.TextAlign = align;
            this.ascending = asc;
        }
    }

}



