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
    public partial class UserCarpetas : UserControl
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
        private bool l_plu = false;
        private string[] tipo_prod = new string[] { "No Pesable", "Pesable" };
        private string[] tipo_prec = new string[] { "No Editable", "Editable" };

        public struct sPoint_Mouse
        {
            public int x;
            public int y;
        }

        public sPoint_Mouse _sPositionCurrent = new sPoint_Mouse();


        Variable.lgrupo[] myGrupo;
        Variable.lbasc[] myScale;
        Variable.nodo_actual myCurrent;
        //ESTADO.botonesEnvioDato DatosEnviado = new ESTADO.botonesEnvioDato();
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
        public UserCarpetas()
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

            _sPositionCurrent.x = 0;
            _sPositionCurrent.y = 0;
        }
        #endregion

        #region Consulta y escritura de Base de Datos

        private void Asigna_Grupo()
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
        private void Asigna_Bascula()
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
        private void Asigna_Productos()
        {
            Cursor.Current = Cursors.WaitCursor;

            this.listViewProductos.Visible = true;
            this.listViewProductos.BringToFront();
            this.listViewProductos.View = View.Details;
            this.listViewProductos.FullRowSelect = true;
            this.listViewProductos.GridLines = true;
            this.listViewProductos.LabelEdit = false;
            this.listViewProductos.HideSelection = false;
            this.listViewProductos.ListViewItemSorter = new ListViewIndexComparer();
            this.listViewProductos.Clear();
            this.listViewProductos.Columns.Add(new ColHeader(Variable.SYS_MSJ[145, Variable.idioma], 80, HorizontalAlignment.Left,false));  // Codigo
            this.listViewProductos.Columns.Add(new ColHeader(Variable.SYS_MSJ[162, Variable.idioma], 80, HorizontalAlignment.Left,false)); //No. PLU
            this.listViewProductos.Columns.Add(new ColHeader(Variable.SYS_MSJ[163, Variable.idioma], 230, HorizontalAlignment.Left,false)); //Nombre
            this.listViewProductos.Columns.Add(new ColHeader(Variable.SYS_MSJ[166, Variable.idioma], 90, HorizontalAlignment.Left,false));  //Precio
            this.listViewProductos.Columns.Add(new ColHeader(Variable.SYS_MSJ[148, Variable.idioma], 80, HorizontalAlignment.Left,false));  //Impuesto
            this.listViewProductos.Columns.Add(new ColHeader(Variable.SYS_MSJ[170, Variable.idioma], 100, HorizontalAlignment.Left,false));  //Tipo de producto
            this.listViewProductos.Columns.Add(new ColHeader(Variable.SYS_MSJ[169, Variable.idioma], 100, HorizontalAlignment.Left,false));  //tipo de precio
            this.listViewProductos.Columns.Add(new ColHeader(Variable.SYS_MSJ[143, Variable.idioma], 80, HorizontalAlignment.Left,false)); //Caducidad
            this.listViewProductos.Columns.Add(new ColHeader(Variable.SYS_MSJ[160, Variable.idioma], 80, HorizontalAlignment.Left, false));  //Multiplo

            Conec.CadenaSelect = "SELECT * FROM Productos ORDER BY id_producto";

            productosTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            productosTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            productosTableAdapter.Fill(baseDeDatosDataSet.Productos);
            DataRow[] DR_PR = baseDeDatosDataSet.Productos.Select("borrado = " + false, "id_producto");
            baseDeDatosDataSet.Productos.PrimaryKey = new DataColumn[] { baseDeDatosDataSet.Productos.CodigoColumn };

           // for (int i = 0; i < baseDeDatosDataSet.Productos.Rows.Count; i++)
            foreach(DataRow PROD in DR_PR)
            {
                ListViewItem lwitem = new ListViewItem(PROD["Codigo"].ToString());  //0
                lwitem.SubItems.Add(PROD["NoPlu"].ToString()); //1
                lwitem.SubItems.Add(PROD["Nombre"].ToString());  //2
                lwitem.SubItems.Add(String.Format(Variable.F_Decimal,Convert.ToDouble(PROD["Precio"].ToString())));  //3
                lwitem.SubItems.Add(String.Format(Variable.F_Descuento,Convert.ToDouble(PROD["Impuesto"].ToString())));  //4
                lwitem.SubItems.Add(tipo_prod[Convert.ToInt16(PROD["TipoID"].ToString())]); //5
                lwitem.SubItems.Add(tipo_prec[Convert.ToInt16(PROD["PrecioEditable"].ToString())]); //6
                lwitem.SubItems.Add(PROD["CaducidadDias"].ToString()); //7
                lwitem.SubItems.Add(PROD["Mutiplo"].ToString()); //8
                lwitem.SubItems.Add(PROD["publicidad1"].ToString());  //9
                lwitem.SubItems.Add(PROD["publicidad2"].ToString());  //10
                lwitem.SubItems.Add(PROD["publicidad3"].ToString());  //11
                lwitem.SubItems.Add(PROD["publicidad4"].ToString());  //12
                lwitem.SubItems.Add(PROD["actualizado"].ToString());  //13             
                lwitem.SubItems.Add(PROD["imagen1"].ToString()); //14                
                lwitem.SubItems.Add(PROD["imagen"].ToString()); //15
                lwitem.SubItems.Add(PROD["id_ingrediente"].ToString()); //16
                lwitem.SubItems.Add(PROD["id_info_nutri"].ToString()); //17
                lwitem.SubItems.Add(PROD["id_producto"].ToString()); //18
                lwitem.SubItems.Add(PROD["tara"].ToString()); //19
                this.listViewProductos.Items.Add(lwitem);
            }
           
            Cursor.Current = Cursors.Default;
        }

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
           // dr["posicion"] = DatosNuevos[4];
            dr["pendiente"] = true;
            dr.EndEdit();

            prod_detalleTableAdapter.Update(dr);
            baseDeDatosDataSet.Prod_detalle.AcceptChanges();

            Conec.CadenaSelect = "INSERT INTO Prod_detalle " +
            "(id_bascula,id_grupo,id_carpeta,id_producto, codigo, NoPLU, pendiente)" +
           "VALUES (" + bascula + "," +   //id_bascula
             sucursal + "," +   //id_grupo  
             carpeta + "," +     // id_carpeta
             Convert.ToInt32(DatosNuevos[0]) + "," +     // id_producto
             Convert.ToInt32(DatosNuevos[1]) + "," +  //codigo
             Convert.ToInt32(DatosNuevos[2]) + "," + //NoPLU
             true + ")"; //posicion

            Conec.InsertarReader(Conec.CadenaConexion, Conec.CadenaSelect, baseDeDatosDataSet.Prod_detalle.TableName);
        }

        private void Ver_Productos_en_Carpetas(int n_carpeta)
        {
            StripProductos.Enabled = true;

            this.Cursor = Cursors.WaitCursor;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.listViewProductos.Visible = true;
            this.listViewProductos.BringToFront();
            this.listViewProductos.View = View.Details;
            this.listViewProductos.FullRowSelect = true;
            this.listViewProductos.GridLines = true;
            this.listViewProductos.LabelEdit = false;
            this.listViewProductos.HideSelection = false;
            this.listViewProductos.ListViewItemSorter = new ListViewIndexComparer();
            this.listViewProductos.Clear();
            this.listViewProductos.Columns.Add(new ColHeader(Variable.SYS_MSJ[145, Variable.idioma], 80, HorizontalAlignment.Left,false));
            this.listViewProductos.Columns.Add(new ColHeader(Variable.SYS_MSJ[162, Variable.idioma], 80, HorizontalAlignment.Left,false));
            this.listViewProductos.Columns.Add(new ColHeader(Variable.SYS_MSJ[163, Variable.idioma], 230, HorizontalAlignment.Left,false));
            this.listViewProductos.Columns.Add(new ColHeader(Variable.SYS_MSJ[166, Variable.idioma], 80, HorizontalAlignment.Left,false));
            this.listViewProductos.Columns.Add(new ColHeader(Variable.SYS_MSJ[148, Variable.idioma], 80, HorizontalAlignment.Left,false));
            this.listViewProductos.Columns.Add(new ColHeader(Variable.SYS_MSJ[143, Variable.idioma], 80, HorizontalAlignment.Left,false));
            this.listViewProductos.Columns.Add(new ColHeader(Variable.SYS_MSJ[149, Variable.idioma], 80, HorizontalAlignment.Left,false));
            this.listViewProductos.Columns.Add(new ColHeader(Variable.SYS_MSJ[171, Variable.idioma], 80, HorizontalAlignment.Left,false));
            this.listViewProductos.Columns.Add(new ColHeader(Variable.SYS_MSJ[170, Variable.idioma], 100, HorizontalAlignment.Left, false));

            prod_detalleTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            prod_detalleTableAdapter.Connection.CreateCommand().CommandText = "SELECT * FROM Prod_detalle WHERE (id_carpeta = " + n_carpeta + " AND id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo + ")";
            prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);

            DataRelation Producto_Detalle = baseDeDatosDataSet.Relations["ProductosProd_detalle"];

            foreach (DataRow DA in baseDeDatosDataSet.Productos.Rows)
            {
                foreach (DataRow PR in DA.GetChildRows(Producto_Detalle))
                {                    
                    if (Convert.ToInt32(PR["id_carpeta"].ToString()) == n_carpeta && Convert.ToInt32(PR["id_bascula"].ToString()) == Num_Bascula && Convert.ToInt32(PR["id_grupo"].ToString()) == Num_Grupo)
                    {
                        ListViewItem lwitem = new ListViewItem(PR["codigo"].ToString());  //0
                        lwitem.SubItems.Add(PR["NoPlu"].ToString()); //1
                        lwitem.SubItems.Add(DA["Nombre"].ToString().Trim()); //2
                        lwitem.SubItems.Add(string.Format(Variable.F_Decimal, Convert.ToDouble(DA["precio"].ToString()))); //3
                        lwitem.SubItems.Add(string.Format(Variable.F_Decimal, Convert.ToDouble(DA["Impuesto"].ToString()))); //4
                        lwitem.SubItems.Add(DA["CaducidadDias"].ToString()); //5
                        lwitem.SubItems.Add(DA["id_ingrediente"].ToString()); //6
                        lwitem.SubItems.Add(DA["id_info_nutri"].ToString()); // 7
                        lwitem.SubItems.Add(tipo_prod[Convert.ToInt16(DA["TipoId"].ToString())]); //8
                        lwitem.SubItems.Add(DA["PrecioEditable"].ToString()); //9
                        lwitem.SubItems.Add(DA["Mutiplo"].ToString()); //10
                        lwitem.SubItems.Add(DA["publicidad1"].ToString()); //11
                        lwitem.SubItems.Add(DA["publicidad2"].ToString()); //12
                        lwitem.SubItems.Add(DA["publicidad3"].ToString()); //13
                        lwitem.SubItems.Add(DA["publicidad4"].ToString()); //14
                        lwitem.SubItems.Add(DA["imagen1"].ToString()); //15
                        lwitem.SubItems.Add(DA["imagen"].ToString()); //16
                        lwitem.SubItems.Add(DA["oferta"].ToString()); //17
                        lwitem.SubItems.Add(PR["id_producto"].ToString()); //18
                        lwitem.SubItems.Add(DA["tara"].ToString()); //19
                        this.listViewProductos.Items.Add(lwitem);
                    }
                }
            }
            this.Cursor = Cursors.Default;
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
            Carpetas dat = new Carpetas(new Point(_sPositionCurrent.x, _sPositionCurrent.y));
            Carpetas.nbascula = Num_Bascula;
            Carpetas.ngrupo = Num_Grupo;
            Carpetas.numfold = Convert.ToInt32(dat.muestraAutoincrementoId());
            Carpetas.nomfold = "Nueva Carpeta";            

            if (Carpetas.numfold > 0)
            {
                if (dat.Nuevo_Folder(Carpetas.numfold, Carpetas.nomfold, Convert.ToInt32(subnodo.Value),Convert.ToInt16(subnodo.Index)))
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

        private void Order_Ascending(int Ncolumna)
        {
            ColHeader clickedCol = (ColHeader)this.listViewProductos.Columns[Ncolumna];

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
       
        private class ListViewIndexComparer : System.Collections.IComparer
        {
            public int Compare(object x, object y)
            {
                return ((ListViewItem)x).Index - ((ListViewItem)y).Index;
            }
        }
        #endregion

        #region ListView y treeView
        private void listViewProductos_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            Order_Ascending(e.Column);
        }
        private void listViewProductos_ItemDrag(object sender, ItemDragEventArgs e)
        {
            System.Windows.Forms.ListView lv_item = (System.Windows.Forms.ListView)sender;
            object strItem = lv_item;
            l_plu = true;
            DoDragDrop(strItem, DragDropEffects.All);
        }

        private void listViewProductos_DragDrop(object sender, DragEventArgs e)
        {
            System.Windows.Forms.ListView lv_item = (System.Windows.Forms.ListView)sender;
        }

        private void listViewProductos_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void listViewProductos_KeyDown(object sender, KeyEventArgs e)
        {
            Int32 IdCarpeta = Convert.ToInt32(tvwCarpetas.SelectedValue);
            Int32 IdProducto;

            if (e.KeyCode == Keys.Delete && RegistroEstado == ESTADO.EstadoRegistro.PKTRATADO)
            {
                DialogResult op = MessageBox.Show(this, Variable.SYS_MSJ[232, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (op == DialogResult.Yes)
                {

                    WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[349, Variable.idioma]);
                    Thread t = new Thread(workerObject.vShowMsg);
                    t.Start();

                    foreach (ListViewItem prod_sele in listViewProductos.SelectedItems)
                    {
                        IdProducto = Convert.ToInt32(prod_sele.SubItems[18].Text);
                        Borrar_Productos_en_Carpetas(IdCarpeta, IdProducto);
                    }

                    Thread.Sleep(50);

                    workerObject.vEndShowMsg();
                    t.Join();

                    Ver_Productos_en_Carpetas(IdCarpeta);
                }
            }
        }

        private void ActualizarChildNodes(TreeNodeCollection ListNodes)
        {
            foreach (TreeNode inode in ListNodes)
            {
                TreeNodeBound N_folder = (TreeNodeBound)inode;


                Conec.CadenaSelect = "UPDATE carpeta_detalle SET ruta = '" + inode.FullPath + "', tabla = 'C'" +
                                         " WHERE (id_bascula = " + Num_Bascula + " AND id_grupo = " + Num_Grupo + " AND ID = " + Convert.ToInt32(N_folder.Value) + ")";

                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "carpeta_detalle");

                if (inode.Nodes.Count > 0)
                {
                    this.ActualizarChildNodes(inode.Nodes);
                }

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
                    if (this.tvwCarpetas.SelectedNode.Level < 5)
                    {
                        if (this.tvwCarpetas.GetNodeCount(false) < 501)
                        {
                            Cambiar_carpeta_de_Nodo(-1, Convert.ToInt32(NodeMove.Value));
                            DragNode.Remove();
                            this.tvwCarpetas.Nodes.Add(NodeMove);
                        }
                        else MessageBox.Show(this, Variable.SYS_MSJ[279, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma]);
                    }
                    else MessageBox.Show(this, Variable.SYS_MSJ[385, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma]);
                }
                else
                {
                    if (NodeMove != null)
                    {
                        if (Convert.ToInt32(DropNode.Value) != Convert.ToInt32(NodeMove.Value))
                        {
                            this.exist_nodo = false;
                            if (DropNode.Level < 5)
                            {
                                if (!find_node_tree(NodeMove, DropNode))
                                {
                                    if (DropNode.Nodes.Count < 501)
                                    {
                                        Cambiar_carpeta_de_Nodo(Convert.ToInt32(DropNode.Value), Convert.ToInt32(NodeMove.Value));
                                        DragNode.Remove();
                                        DropNode.Nodes.Add(NodeMove);
                                    }
                                    else MessageBox.Show(this, Variable.SYS_MSJ[279, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma]);
                                }
                            }
                            else MessageBox.Show(this, Variable.SYS_MSJ[385, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma]);
                        }
                    }
                }
            }
            else
            {    // Agrega productos seleccionados a la carpeta seleccionada
                
                OriginalNode = ((TreeViewBound.TreeNodeBound)DropNode);
                if (OriginalNode != null)
                {
                    WorkerFormWait workerObject = new WorkerFormWait(Variable.SYS_MSJ[348, Variable.idioma]);
                    Thread t = new Thread(workerObject.vShowMsg);
                    t.Start();
                    
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
                    if (tot_prod_carpeta < 501)
                    {
                        for (int i = 0; i < this.listViewProductos.SelectedItems.Count; i++)
                        {
                            clave[3] = Convert.ToInt32(this.listViewProductos.SelectedItems[i].SubItems[18].Text);  //numero de codigo de producto

                            DataRow DT = baseDeDatosDataSet.Prod_detalle.Rows.Find(clave);
                            if (DT == null && i < 501)
                            {
                                Nposicion++;
                                datos[0] = this.listViewProductos.SelectedItems[i].SubItems[18].Text;  //id_producto
                                datos[1] = this.listViewProductos.SelectedItems[i].SubItems[0].Text;  //codigo
                                datos[2] = this.listViewProductos.SelectedItems[i].SubItems[1].Text;  //NoPLU
                                datos[3] = this.listViewProductos.SelectedItems[i].SubItems[19].Text;  //Tara
                                datos[4] = Nposicion.ToString();
                                Crear_DetalleProducto(Num_Bascula, Num_Grupo, Convert.ToInt32(OriginalNode.Value), datos);
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

                        Thread.Sleep(50);

                        workerObject.vEndShowMsg();
                        t.Join();
                    }
                    else
                    {
                        Thread.Sleep(50);

                        workerObject.vEndShowMsg();
                        t.Join();
                        MessageBox.Show(this, Variable.SYS_MSJ[278, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma]);
                    }
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
                    Carpetas dat = new Carpetas(new Point(_sPositionCurrent.x,_sPositionCurrent.y));
                    Carpetas.nbascula = Num_Bascula;
                    Carpetas.ngrupo = Num_Grupo;

                    if (Convert.ToInt32(nodo.SelectedValue) > 0)
                    {
                        if (!dat.Buscar_SubFolder(nodo.SelectedValue.ToString()))
                        {
                            if (!dat.Buscar_Articulo(Convert.ToInt32(nodo.SelectedValue.ToString()), Num_Bascula, Num_Grupo))
                            {
                                if (dat.Del_Folder(nodo.SelectedNode.Text, Convert.ToInt32(nodo.SelectedValue)))
                                {
                                    this.tvwCarpetas.SelectedNode.Remove();
                                }
                            }
                            else
                            {
                                DialogResult op = MessageBox.Show(this, Variable.SYS_MSJ[233, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                                if (op == DialogResult.OK)
                                {
                                    Borrar_Productos_en_Carpetas(Convert.ToInt32(nodo.SelectedValue));

                                    if (dat.Del_Folder(nodo.SelectedNode.Text, Convert.ToInt32(nodo.SelectedValue)))
                                    {
                                        this.tvwCarpetas.SelectedNode.Remove();
                                    }
                                }
                            }
                        }
                    }
                    e.Handled = true;
                }
                if (e.KeyCode == Keys.Insert && nodo.SelectedNode != null)
                {
                    if (nodo.SelectedNode.Level < 5)
                    {
                        if (nodo.GetNodeCount(false) < 501)
                        {
                            Crear_Carpetas((TreeNodeBound)nodo.SelectedNode);
                            e.Handled = true;
                        }
                        else
                        {
                            MessageBox.Show(this, Variable.SYS_MSJ[279, Variable.idioma], Variable.SYS_MSJ[39, Variable.idioma]);
                            e.Handled = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, Variable.SYS_MSJ[385, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);  //"No puede agregar mas niveles;
                        e.Handled = true;
                    }
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

            this.Cursor = new System.Windows.Forms.Cursor(Cursor.Current.Handle);
            _sPositionCurrent.x = Cursor.Position.X;
            _sPositionCurrent.y = Cursor.Position.Y;
        }

        private void tvwCarpetasMenu_Click(object sender, System.EventArgs e)
        {
            ToolStripMenuItem MiCarpeta = (ToolStripMenuItem)sender;

            switch (MiCarpeta.MergeIndex)
            {
                case 0:
                    {  	// Nuevo Carpeta}            
                        if (this.tvwCarpetas.SelectedNode.Level < 5)
                        {
                            Carpetas dat = new Carpetas(new Point(_sPositionCurrent.x, _sPositionCurrent.y));
                            Carpetas.opcion = 0;
                            Carpetas.ngrupo = Num_Grupo;
                            Carpetas.nbascula = Num_Bascula;
                            dat.ShowDialog(this);
                            if (Carpetas.nomfold != "" || Carpetas.nomfold != null)
                            {
                                Crear_Nodos_Carpetas();
                            }
                        }
                        else
                        {
                            MessageBox.Show(this, Variable.SYS_MSJ[385, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);  //"No puede agregar mas niveles;
                            tvwCarpetas.Focus();
                        }
                    } break;
                case 2: //Cambiar de carpeta
                    {
                        if (Convert.ToInt32(this.tvwCarpetas.SelectedValue) > 0)
                        {
                            Carpetas dat = new Carpetas(new Point(_sPositionCurrent.x, _sPositionCurrent.y));
                            Carpetas.opcion = 1;
                            Carpetas.ngrupo = Num_Grupo;
                            Carpetas.nbascula = Num_Bascula;
                            Carpetas.nomfold = this.tvwCarpetas.SelectedNode.Text;
                            Carpetas.numfold = Convert.ToInt32(this.tvwCarpetas.SelectedValue);
                            dat.ShowDialog(this);
                            if (Carpetas.nomfold != "" || Carpetas.nomfold != null)
                            {
                                Crear_Nodos_Carpetas();
                            }
                        }
                    } break;
                case 3:
                    { // Borrar Carpeta
                        Carpetas dat = new Carpetas(new Point(_sPositionCurrent.x, _sPositionCurrent.y));
                        Carpetas.opcion = 0;
                        Carpetas.ngrupo = Num_Grupo;
                        Carpetas.nbascula = Num_Bascula;
                        Carpetas.nomfold = this.tvwCarpetas.SelectedNode.Text;
                        Carpetas.numfold = Convert.ToInt32(this.tvwCarpetas.SelectedValue);

                        if (Carpetas.numfold > 0)
                        {
                            if (!dat.Buscar_SubFolder(Carpetas.numfold.ToString()))
                            {
                                if (dat.Buscar_Articulo(Carpetas.numfold, Num_Bascula, Num_Grupo)) //(!dat.Buscar_Articulo(Carpetas.numfold.ToString()))
                                {
                                    if (dat.Del_Folder(Carpetas.nomfold, Carpetas.numfold))
                                    {
                                        this.tvwCarpetas.SelectedNode.Remove();
                                    }
                                }
                                else
                                {
                                    DialogResult op = MessageBox.Show(this, Variable.SYS_MSJ[234, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (op == DialogResult.Yes)
                                    {
                                        Borrar_Productos_en_Carpetas(Carpetas.numfold);

                                        if (dat.Del_Folder(Carpetas.nomfold, Carpetas.numfold))
                                        { this.tvwCarpetas.SelectedNode.Remove(); }
                                    }
                                }
                            }
                        }
                        Crear_Nodos_Carpetas();
                    } break;
            }
        }
        #endregion

        #region Proceso de productos y folder
        private void Cambiar_nombre_carpeta(int numero, string nombre)
        {
            Conec.CadenaSelect = "UPDATE carpeta_detalle SET Nombre = '" + nombre + "' WHERE ( ID = " + numero + " and id_grupo = " + Num_Grupo + " and id_bascula = " + Num_Bascula + ")";
            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "carpeta_detalle");
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

        private bool Cambiar_carpeta_de_Nodo(int padre, int movido)
        {
            int numero_carpeta = baseDeDatosDataSet.carpeta_detalle.Select("ID_padre = " + padre + " and id_grupo = " + Num_Grupo + " and id_bascula = " + Num_Bascula).Length + 1;
            if (numero_carpeta < 501)
            {
                if (padre >= 0) Conec.CadenaSelect = "UPDATE carpeta_detalle SET ID_padre = " + padre + " WHERE ( ID = " + movido + " and id_grupo = " + Num_Grupo + " and id_bascula = " + Num_Bascula + ")";
                else Conec.CadenaSelect = "UPDATE carpeta_detalle SET ID_padre = null WHERE ( ID = " + movido + " and id_grupo = " + Num_Grupo + " and id_bascula = " + Num_Bascula + ")";

                Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "carpeta_detalle");
                return true;
            }
            else return false;
        }
        #endregion

        private void UserCarpetas_Load(object sender, EventArgs e)
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

            contextMenuStrip1.Items[0].Text = Variable.SYS_MSJ[399, Variable.idioma];
            contextMenuStrip1.Items[1].Text = Variable.SYS_MSJ[400, Variable.idioma];
            contextMenuStrip1.Items[2].Text = Variable.SYS_MSJ[401, Variable.idioma];

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

        #region funciones para las carpetas y los nodos de carpetas
        private void Cambiar_Estado_pendiente()
        {
            Conec.CadenaSelect = "UPDATE carpeta_detalle SET pendiente= " + false + " WHERE ( enviado = " + true + ")";
            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "carpeta_detalle");
            Conec.CadenaSelect = "UPDATE Prod_detalle SET pendiente= " + false + " WHERE ( enviado = " + true + ")";
            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "Prod_detalle");
            Conec.CadenaSelect = "UPDATE carpeta_detalle SET pendiente= " + true + " WHERE ( enviado = " + false + ")";
            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "carpeta_detalle");
            Conec.CadenaSelect = "UPDATE Prod_detalle SET pendiente= " + true + " WHERE ( enviado = " + false + ")";
            Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "Prod_detalle");
        }

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

        private void UserCarpetas_MouseMove(object sender, MouseEventArgs e)
        {
            _sPositionCurrent.x = e.X;
            _sPositionCurrent.y = e.Y;
        }

        private void tvwCarpetas_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode subnodo = (TreeNode)e.Node;

            if (e.Button == MouseButtons.Right && subnodo != null)
            {
                this.tvwCarpetas.SelectedNode = subnodo;
                this.tvwCarpetas.ContextMenuStrip = this.contextMenuStrip1;
                tvwCarpetas.ContextMenuStrip.Show(tvwCarpetas, new Point(e.X, e.Y));
            }
        }

        private void crearCarpetasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Nuevo Carpeta  

            if (this.tvwCarpetas.SelectedNode != null)
            {
                if (this.tvwCarpetas.SelectedNode.Level < 5)
                {
                    Carpetas dat = new Carpetas(new Point(_sPositionCurrent.x, _sPositionCurrent.y));
                    Carpetas.opcion = 0;
                    Carpetas.ngrupo = Num_Grupo;
                    Carpetas.nbascula = Num_Bascula;
                    Carpetas.numparent = Convert.ToInt32(this.tvwCarpetas.SelectedValue);
                    Carpetas.iAction = 0;
                    dat.ShowDialog(this);
                    if (Carpetas.nomfold != "" || Carpetas.nomfold != null)
                    {
                        Crear_Nodos_Carpetas();
                    }
                }
                else
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[385, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);  //"No puede agregar mas niveles;
                    tvwCarpetas.Focus();
                }
            }
            else
            {
                MessageBox.Show(this, Variable.SYS_MSJ[47, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);  //"No Hay carpeta seleccionada");
                tvwCarpetas.Focus();
            }

        }

        private void editarCarpetasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Cambiar de carpeta
            TreeNodeBound nod = (TreeNodeBound)this.tvwCarpetas.SelectedNode;
            if (this.tvwCarpetas.SelectedNode != null && nod.ParentValue != null)
            {
                Carpetas dat = new Carpetas(new Point(_sPositionCurrent.x, _sPositionCurrent.y));
                Carpetas.opcion = 1;
                Carpetas.ngrupo = Num_Grupo;
                Carpetas.nbascula = Num_Bascula;
                Carpetas.nomfold = this.tvwCarpetas.SelectedNode.Text;
                Carpetas.numparent = Convert.ToInt32(nod.ParentValue);
                Carpetas.numfold = Convert.ToInt32(this.tvwCarpetas.SelectedValue);
                Carpetas.iAction = 1;
                // Carpetas.numposicion = Convert.ToInt32(nod.Index);
                dat.ShowDialog(this);
                if (Carpetas.nomfold != "" || Carpetas.nomfold != null)
                {
                    Crear_Nodos_Carpetas();
                }
            }
            else
            {
                MessageBox.Show(this, Variable.SYS_MSJ[47, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);  //"No Hay carpeta seleccionada");
                tvwCarpetas.Focus();
            }
        }

        private void borrarCarpetasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Borrar Carpeta
            TreeNodeBound nod = (TreeNodeBound)this.tvwCarpetas.SelectedNode;
            if (this.tvwCarpetas.SelectedNode != null && nod.ParentValue != null)
            {
                Carpetas dat = new Carpetas(new Point(_sPositionCurrent.x, _sPositionCurrent.y));
                Carpetas.opcion = 0;
                Carpetas.ngrupo = Num_Grupo;
                Carpetas.nbascula = Num_Bascula;
                Carpetas.nomfold = this.tvwCarpetas.SelectedNode.Text;
                Carpetas.numfold = Convert.ToInt32(this.tvwCarpetas.SelectedValue);
                if (this.listViewProductos.SelectedItems.Count <= 0)
                {
                    if (Carpetas.numfold > 0)
                    {
                        if (!dat.Buscar_SubFolder(Carpetas.numfold.ToString()))
                        {
                            if (dat.Buscar_Articulo(Carpetas.numfold, Num_Bascula, Num_Grupo)) //(!dat.Buscar_Articulo(Carpetas.numfold.ToString()))
                            {
                                if (dat.Del_Folder(Carpetas.nomfold, Carpetas.numfold)) this.tvwCarpetas.SelectedNode.Remove();
                            }
                            else
                            {
                                DialogResult op = MessageBox.Show(this, Variable.SYS_MSJ[233, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                                if (op == DialogResult.OK)
                                {
                                    Borrar_Productos_en_Carpetas(Carpetas.numfold);

                                    if (dat.Del_Folder(Carpetas.nomfold, Carpetas.numfold)) this.tvwCarpetas.SelectedNode.Remove();
                                }
                            }
                        }
                    }
                }
                else { Borrar_Productos_en_Carpetas(Carpetas.numfold); }
            }
            else
            {
                MessageBox.Show(this, Variable.SYS_MSJ[47, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information); //No hay carpeta seleccionada
                tvwCarpetas.Focus();
            }
        }
        
        #endregion

        #region funciones para enviar informacion por serial y por ethernet
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

            Console.WriteLine("{0}(): IN", System.Reflection.MethodBase.GetCurrentMethod().Name);

            Variable_frame = "";
            Variable_frame = "PAXX" + (char)9 + (char)10;

            SR.SendCOMSerial(ref serialPort1, Variable_frame);
        }

        private bool Envia_Borrar_Carpetas(string direccionIP, ref Socket Cliente_bascula)
        {
            string[] Dato_Recibido = null;
            string reg_enviado;
            string Variable_frame;
            bool Limpiar = false;

            Variable_frame = "";
            Variable_frame = "PCXX" + (char)9 + (char)10;

            Cte.Envio_Dato(ref Cliente_bascula, direccionIP, Variable_frame, ref Dato_Recibido);

            if (Dato_Recibido != null)
            {
                reg_enviado = Variable_frame.Substring(4);
                if (Dato_Recibido[0].IndexOf("Error") >= 0) { Limpiar = false; }
                if (Dato_Recibido[0].IndexOf("Ok") >= 0) { Limpiar = true; }
            }

            return Limpiar;
        }
        private void Envia_Borrar_Carpetas(ref SerialPort Cliente_bascula)
        {
            string Variable_frame;
            Console.WriteLine("{0}(): IN", System.Reflection.MethodBase.GetCurrentMethod().Name);

            Variable_frame = "";
            Variable_frame = "PCXX" + (char)9 + (char)10;

            SR.SendCOMSerial(ref Cliente_bascula, Variable_frame);

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
            string Variable_frame= null;
            string[] Dato_recibido = null;
            string strcomando;

            Console.WriteLine("{0}(): IN", System.Reflection.MethodBase.GetCurrentMethod().Name);

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

            Console.WriteLine("{0}(): IN", System.Reflection.MethodBase.GetCurrentMethod().Name);

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

        private bool Enviar_Producto_Bascula(string direccionIP, ref Socket Cliente_bascula, int SoloUno, ref int TotalEnviado, ref int TotalNoEnviado, ref int TotalBorrado, ref ProgressContinue pro)
        {
            string Msj_recibido;
            string Variable_frame = null;
            int reg_leido = 0;
            int reg_envio = 0;
            bool ERROR = false;
            char[] chr = new char[] { (char)10, (char)13 };

            DataRelation Producto_Detalle = baseDeDatosDataSet.Relations["ProductosProd_detalle"];

            int reg_total = baseDeDatosDataSet.Prod_detalle.Select("id_bascula = " + Num_Bascula + " AND id_grupo = " + Num_Grupo).Length;

            Variable_frame = "";

            pro.IniciaProcess(reg_total, Variable.SYS_MSJ[249, Variable.idioma] + " " + myCurrent.Nserie + "... ");  //"Enviando Productos","Iniciando proeso");

            CommandTorrey myobj = new CommandTorrey();
            foreach (DataRow DA in baseDeDatosDataSet.Productos.Rows)
            {
                foreach (DataRow PR in DA.GetChildRows(Producto_Detalle))
                {
                    if (SoloUno == 0)
                    {
                        if (Convert.ToInt32(PR["id_bascula"].ToString()) == Num_Bascula && Convert.ToInt32(PR["id_grupo"].ToString()) == Num_Grupo)
                        {
                            if (!Convert.ToBoolean(DA["borrado"].ToString()))
                            {
                                int posicion = DA["imagen1"].ToString().LastIndexOf('\\');
                                string ImagenAEnviar = DA["imagen1"].ToString();
                                if (posicion > 0)
                                {
                                    string CarpetaAEnviar = DA["imagen1"].ToString().Substring(0, posicion);
                                    int iRtaFunct = myobj.TORREYSendImagesToScale(direccionIP, ImagenAEnviar, "Product", Cliente_bascula);
                                }

                                Variable_frame = Variable_frame + Env.Genera_Trama_Producto(DA);
                                reg_leido++;
                                if (reg_leido > 4)
                                {
                                    reg_envio = reg_envio + reg_leido;
                                    Msj_recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo, "GP");
                                    if (Msj_recibido != null)
                                    {
                                        Env.Crea_Producto_Detalle(myCurrent.idbas, myCurrent.gpo, Variable_frame.Split(chr), false, true);
                                        TotalEnviado++;
                                    }
                                    else
                                    {
                                        TotalNoEnviado++;
                                        ERROR = true;
                                        break;
                                    }
                                    pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[249, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                                    reg_leido = 0;
                                    Variable_frame = "";
                                }
                            }
                            else
                            {
                                Env.Borrar_Producto_detalle(myCurrent.idbas, myCurrent.gpo, Variable_frame.Split(chr));
                                Borrar_DetalleProducto(Convert.ToInt32(PR["id_producto"].ToString()));
                                TotalBorrado++;
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(PR["id_bascula"].ToString()) == Num_Bascula && Convert.ToInt32(PR["id_grupo"].ToString()) == Num_Grupo && Convert.ToBoolean(DA["pendiente"]) == true)
                        {
                            if (!Convert.ToBoolean(DA["borrado"].ToString()))
                            {
                                int posicion = DA["imagen1"].ToString().LastIndexOf('\\');
                                string ImagenAEnviar = DA["imagen1"].ToString();
                                if (posicion > 0)
                                {
                                    string CarpetaAEnviar = DA["imagen1"].ToString().Substring(0, posicion);
                                    int iRtaFunct = myobj.TORREYSendImagesToScale(direccionIP, ImagenAEnviar, "Product", Cliente_bascula);
                                }
                                Variable_frame = Variable_frame + Env.Genera_Trama_Producto(DA);
                                reg_leido++;
                                if (reg_leido > 4)
                                {
                                    reg_envio = reg_envio + reg_leido;
                                    Msj_recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo, "GP");
                                    if (Msj_recibido != null)
                                    {
                                        Env.Crea_Producto_Detalle(myCurrent.idbas, myCurrent.gpo, Variable_frame.Split(chr), false, true);
                                        TotalEnviado++;
                                    }
                                    else
                                    {
                                        TotalNoEnviado++;
                                        ERROR = true;
                                        break;
                                    }
                                    pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[249, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                                    reg_leido = 0;
                                    Variable_frame = "";
                                }
                            }
                            else
                            {
                                Env.Borrar_Producto_detalle(myCurrent.idbas, myCurrent.gpo, Variable_frame.Split(chr));
                                Borrar_DetalleProducto(Convert.ToInt32(PR["id_producto"].ToString()));
                                TotalBorrado++;
                            }
                        }
                    }
                }
                if (ERROR) break;
            }

            if (Variable_frame.Length > 0 && reg_leido <= 4)
            {
                reg_envio = reg_envio + reg_leido;
                Msj_recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo, "GP");
                if (Msj_recibido != null)
                {
                    Env.Crea_Producto_Detalle(myCurrent.idbas, myCurrent.gpo, Variable_frame.Split(chr), false, true);
                    TotalEnviado++;
                }
                else
                {
                    ERROR = true;
                    TotalNoEnviado++;
                }
                pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[249, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                reg_leido = 0;
                Variable_frame = "";
            }
            if (reg_envio > 0) Env.Command_Limpiar(direccionIP, ref Cliente_bascula, "GPF0");
            return ERROR;
            //if (TotalNoEnviado > 0) MessageBox.Show(this, Variable.SYS_MSJ[194, Variable.idioma] + myCurrent.Nserie + Variable.SYS_MSJ[195, Variable.idioma] + direccionIP + Variable.SYS_MSJ[282, Variable.idioma] + " " +TotalNoEnviado.ToString(), Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
            
        }
        private void Enviar_Producto_Bascula(ref SerialPort Cliente_bascula, int SoloUno, ref int TotalEnviados, ref int TotalNoEnviados, ref int TotalBorrado, ref ProgressContinue pro)  
        {
            char[] chr = new char[] { (char)10, (char)13 };
            string[] Msj_recibido = null;
            string strcomando;
            string Variable_frame = null;
            int reg_leido = 0;
            int reg_envio = 0;
            CommandTorrey myobj = new CommandTorrey();
            Console.WriteLine("{0}(): IN", System.Reflection.MethodBase.GetCurrentMethod().Name);

            DataRelation Producto_Detalle = baseDeDatosDataSet.Relations["ProductosProd_detalle"];

            int reg_total = baseDeDatosDataSet.Prod_detalle.Select("id_bascula = " + Num_Bascula + " AND id_grupo = " + Num_Grupo).Length;

            Variable_frame = "";

            pro.IniciaProcess(reg_total, Variable.SYS_MSJ[249, Variable.idioma] + " " + myCurrent.Nserie + "... ");

            foreach (DataRow DA in baseDeDatosDataSet.Productos.Rows)
            {
                foreach (DataRow PR in DA.GetChildRows(Producto_Detalle))
                {
                    if (SoloUno == 0)
                    {
                        if (Convert.ToInt32(PR["id_bascula"].ToString()) == Num_Bascula && Convert.ToInt32(PR["id_grupo"].ToString()) == Num_Grupo)
                        {
                            if (!Convert.ToBoolean(DA["borrado"].ToString()))
                            {
                                Variable_frame = Variable_frame + Env.Genera_Trama_Producto(DA);
                                reg_leido++;
                                if (reg_leido > 1 && Variable_frame.Length > 0)
                                {
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.WriteLine("{0}: Se enviaron {1} imagenes", System.Reflection.MethodBase.GetCurrentMethod().Name, reg_leido);
                                    reg_envio = reg_envio + reg_leido;

                                    strcomando = "GP" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                                    
                                    SR.SendCOMSerial(ref serialPort1, strcomando, ref Msj_recibido);
                                    if (Msj_recibido != null)
                                    {
                                        Console.WriteLine("{0}: Mensaje recibido", System.Reflection.MethodBase.GetCurrentMethod().Name);
                                        if (Msj_recibido[0].IndexOf("Error") >= 0)
                                        {
                                            Env.Guardar_Trama_pendiente(Num_Bascula, Num_Grupo, strcomando);
                                            TotalNoEnviados++;
                                        }
                                        if (Msj_recibido[0].IndexOf("Ok") >= 0)
                                        {
                                            Env.Crea_Producto_Detalle(myCurrent.idbas, myCurrent.gpo, Variable_frame.Split(chr), false, true);
                                            TotalEnviados++;
                                            //pro.UpdateProcess(1, Variable.SYS_MSJ[249, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                                        }
                                    }
                                    Console.WriteLine("{0}: Update progress bar + {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, reg_leido);
                                    pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[249, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    reg_leido = 0;
                                    Variable_frame = "";
                                }
                            }
                            else
                            {
                                Env.Borrar_Producto_detalle(myCurrent.idbas, myCurrent.gpo, Variable_frame.Split(chr));
                                Borrar_DetalleProducto(Convert.ToInt32(PR["id_producto"].ToString()));
                                TotalBorrado++;
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(PR["id_bascula"].ToString()) == Num_Bascula && Convert.ToInt32(PR["id_grupo"].ToString()) == Num_Grupo && Convert.ToBoolean(DA["pendiente"]) == true)
                        {
                            if (!Convert.ToBoolean(DA["borrado"].ToString()))
                            {
                                Variable_frame = Variable_frame + Env.Genera_Trama_Producto(DA);
                                reg_leido++;
                                if (reg_leido > 1 && Variable_frame.Length > 0)
                                {
                                    reg_envio = reg_envio + reg_leido;
                                    strcomando = "GP" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                                    SR.SendCOMSerial(ref serialPort1, strcomando, ref Msj_recibido);
                                    if (Msj_recibido != null)
                                    {
                                        if (Msj_recibido[0].IndexOf("Error") >= 0)
                                        {
                                            Env.Guardar_Trama_pendiente(Num_Bascula, Num_Grupo, strcomando);                                            
                                            TotalNoEnviados++;
                                        }
                                        if (Msj_recibido[0].IndexOf("Ok") >= 0)
                                        {
                                            Env.Crea_Producto_Detalle(myCurrent.idbas, myCurrent.gpo, Variable_frame.Split(chr), false, true);
                                            TotalEnviados++;
                                        }
                                    }
                                    pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[249, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                                    reg_leido = 0;
                                    Variable_frame = "";
                                }
                            }
                            else
                            {
                                Env.Borrar_Producto_detalle(myCurrent.idbas, myCurrent.gpo, Variable_frame.Split(chr));
                                Borrar_DetalleProducto(Convert.ToInt32(PR["id_producto"].ToString()));
                                TotalBorrado++;
                            }
                        }
                    }
                }
            }
            //Envío de imágenes--------------------------------------------------------------------------------------------------
            SR.ClosePort(ref serialPort1); //Se cierra el puerto porque la rutina de envío de imágenes utiliza su propio puerto serie
            
            Variable_frame = "";
            pro.IniciaProcess(reg_total, Variable.SYS_MSJ[425, Variable.idioma] + " " + myCurrent.Nserie + "... ");

            foreach (DataRow DA in baseDeDatosDataSet.Productos.Rows)
            {
                foreach (DataRow PR in DA.GetChildRows(Producto_Detalle))
                {
                    if (SoloUno == 0)
                    {
                        if (Convert.ToInt32(PR["id_bascula"].ToString()) == Num_Bascula && Convert.ToInt32(PR["id_grupo"].ToString()) == Num_Grupo)
                        {
                            if (!Convert.ToBoolean(DA["borrado"].ToString()))
                            {
                                int posicion = DA["imagen1"].ToString().LastIndexOf('\\');
                                string ImagenAEnviar = DA["imagen1"].ToString();
                                if (posicion > 0)
                                {
                                    string CarpetaAEnviar = DA["imagen1"].ToString().Substring(0, posicion);
                                    int iRtaFunct = myobj.TORREYSendImagesToScaleSerial(myCurrent.COMM, ImagenAEnviar, "Product");
                                    //serialPort1 = new SerialPort();
                                    //int iPortOpenRetries = 0;
                                    //while (!(SR.OpenPort(ref serialPort1, myCurrent.COMM, myCurrent.BAUD) && iPortOpenRetries < 3)) iPortOpenRetries++;
                                    //if (iPortOpenRetries >= 3) break;
                                }                        
                                Console.WriteLine("{0}: Update progress bar + {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, 1);
                                pro.UpdateProcess(1, Variable.SYS_MSJ[425, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(PR["id_bascula"].ToString()) == Num_Bascula && Convert.ToInt32(PR["id_grupo"].ToString()) == Num_Grupo && Convert.ToBoolean(DA["pendiente"]) == true)
                        {
                            if (!Convert.ToBoolean(DA["borrado"].ToString()))
                            {
                                int posicion = DA["imagen1"].ToString().LastIndexOf('\\');
                                string ImagenAEnviar = DA["imagen1"].ToString();
                                if (posicion > 0)
                                {
                                    string CarpetaAEnviar = DA["imagen1"].ToString().Substring(0, posicion);
                                    //SR.ClosePort(ref serialPort1);
                                    int iRtaFunct = myobj.TORREYSendImagesToScaleSerial(myCurrent.COMM, ImagenAEnviar, "Product");
                                    //serialPort1 = new SerialPort();
                                    //int iPortOpenRetries = 0;
                                    //while (!(SR.OpenPort(ref serialPort1, myCurrent.COMM, myCurrent.BAUD) && iPortOpenRetries < 3)) iPortOpenRetries++;
                                    //if (iPortOpenRetries >= 3) break;
                                }
                                pro.UpdateProcess(1, Variable.SYS_MSJ[425, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                            }
                        }
                    }
                }
            }
            serialPort1 = new SerialPort();
            int iPortOpenRetries = 0;
            while (!(SR.OpenPort(ref serialPort1, myCurrent.COMM, myCurrent.BAUD) && iPortOpenRetries < 3)) iPortOpenRetries++;
            if (iPortOpenRetries >= 3) MessageBox.Show(this, Variable.SYS_MSJ[428, Variable.idioma],
                    Variable.SYS_MSJ[39, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

/*      
        private void Enviar_Producto_BasculaORIGINAL(ref SerialPort Cliente_bascula, int SoloUno, ref int TotalEnviados, ref int TotalNoEnviados, ref int TotalBorrado, ref ProgressContinue pro)
        {
            char[] chr = new char[] { (char)10, (char)13 };
            string[] Msj_recibido = null;
            string strcomando;
            string Variable_frame = null;
            int reg_leido = 0;
            int reg_envio = 0;
            CommandTorrey myobj = new CommandTorrey();
            Console.WriteLine("{0}(): IN", System.Reflection.MethodBase.GetCurrentMethod().Name);

            DataRelation Producto_Detalle = baseDeDatosDataSet.Relations["ProductosProd_detalle"];

            int reg_total = baseDeDatosDataSet.Prod_detalle.Select("id_bascula = " + Num_Bascula + " AND id_grupo = " + Num_Grupo).Length;

            Variable_frame = "";

            pro.IniciaProcess(reg_total, Variable.SYS_MSJ[249, Variable.idioma] + " " + myCurrent.Nserie + "... ");

            foreach (DataRow DA in baseDeDatosDataSet.Productos.Rows)
            {
                foreach (DataRow PR in DA.GetChildRows(Producto_Detalle))
                {
                    if (SoloUno == 0)
                    {
                        if (Convert.ToInt32(PR["id_bascula"].ToString()) == Num_Bascula && Convert.ToInt32(PR["id_grupo"].ToString()) == Num_Grupo)
                        {
                            if (!Convert.ToBoolean(DA["borrado"].ToString()))
                            {
                                int posicion = DA["imagen1"].ToString().LastIndexOf('\\');
                                string ImagenAEnviar = DA["imagen1"].ToString();
                                if (posicion > 0)
                                {
                                    string CarpetaAEnviar = DA["imagen1"].ToString().Substring(0, posicion);

                                    SR.ClosePort(ref serialPort1);
                                    int iRtaFunct = myobj.TORREYSendImagesToScaleSerial(myCurrent.COMM, ImagenAEnviar, "Product");
                                    serialPort1 = new SerialPort();
                                    int iPortOpenRetries = 0;
                                    while (!(SR.OpenPort(ref serialPort1, myCurrent.COMM, myCurrent.BAUD) && iPortOpenRetries < 3)) iPortOpenRetries++;
                                    if (iPortOpenRetries >= 3) break;
                                }
                                Variable_frame = Variable_frame + Env.Genera_Trama_Producto(DA);
                                reg_leido++;
                                if (reg_leido > 1 && Variable_frame.Length > 0)
                                {
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.WriteLine("{0}: Se enviaron {1} imagenes", System.Reflection.MethodBase.GetCurrentMethod().Name, reg_leido);
                                    reg_envio = reg_envio + reg_leido;

                                    strcomando = "GP" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;

                                    SR.SendCOMSerial(ref serialPort1, strcomando, ref Msj_recibido);
                                    if (Msj_recibido != null)
                                    {
                                        Console.WriteLine("{0}: Mensaje recibido", System.Reflection.MethodBase.GetCurrentMethod().Name);
                                        if (Msj_recibido[0].IndexOf("Error") >= 0)
                                        {
                                            Env.Guardar_Trama_pendiente(Num_Bascula, Num_Grupo, strcomando);
                                            TotalNoEnviados++;
                                        }
                                        if (Msj_recibido[0].IndexOf("Ok") >= 0)
                                        {
                                            Env.Crea_Producto_Detalle(myCurrent.idbas, myCurrent.gpo, Variable_frame.Split(chr), false, true);
                                            TotalEnviados++;
                                            //pro.UpdateProcess(1, Variable.SYS_MSJ[249, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                                        }
                                    }
                                    Console.WriteLine("{0}: Update progress bar + {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, reg_leido);
                                    pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[249, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    reg_leido = 0;
                                    Variable_frame = "";
                                }
                            }
                            else
                            {
                                Env.Borrar_Producto_detalle(myCurrent.idbas, myCurrent.gpo, Variable_frame.Split(chr));
                                Borrar_DetalleProducto(Convert.ToInt32(PR["id_producto"].ToString()));
                                TotalBorrado++;
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(PR["id_bascula"].ToString()) == Num_Bascula && Convert.ToInt32(PR["id_grupo"].ToString()) == Num_Grupo && Convert.ToBoolean(DA["pendiente"]) == true)
                        {
                            if (!Convert.ToBoolean(DA["borrado"].ToString()))
                            {
                                int posicion = DA["imagen1"].ToString().LastIndexOf('\\');
                                string ImagenAEnviar = DA["imagen1"].ToString();
                                if (posicion > 0)
                                {
                                    string CarpetaAEnviar = DA["imagen1"].ToString().Substring(0, posicion);
                                    SR.ClosePort(ref serialPort1);
                                    int iRtaFunct = myobj.TORREYSendImagesToScaleSerial(myCurrent.COMM, ImagenAEnviar, "Product");
                                    serialPort1 = new SerialPort();
                                    int iPortOpenRetries = 0;
                                    while (!(SR.OpenPort(ref serialPort1, myCurrent.COMM, myCurrent.BAUD) && iPortOpenRetries < 3)) iPortOpenRetries++;
                                    if (iPortOpenRetries >= 3) break;
                                }
                                Variable_frame = Variable_frame + Env.Genera_Trama_Producto(DA);
                                reg_leido++;
                                if (reg_leido > 1 && Variable_frame.Length > 0)
                                {
                                    reg_envio = reg_envio + reg_leido;
                                    strcomando = "GP" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                                    SR.SendCOMSerial(ref serialPort1, strcomando, ref Msj_recibido);
                                    if (Msj_recibido != null)
                                    {
                                        if (Msj_recibido[0].IndexOf("Error") >= 0)
                                        {
                                            Env.Guardar_Trama_pendiente(Num_Bascula, Num_Grupo, strcomando);
                                            TotalNoEnviados++;
                                        }
                                        if (Msj_recibido[0].IndexOf("Ok") >= 0)
                                        {
                                            Env.Crea_Producto_Detalle(myCurrent.idbas, myCurrent.gpo, Variable_frame.Split(chr), false, true);
                                            TotalEnviados++;
                                        }
                                    }
                                    pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[249, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                                    reg_leido = 0;
                                    Variable_frame = "";
                                }
                            }
                            else
                            {
                                Env.Borrar_Producto_detalle(myCurrent.idbas, myCurrent.gpo, Variable_frame.Split(chr));
                                Borrar_DetalleProducto(Convert.ToInt32(PR["id_producto"].ToString()));
                                TotalBorrado++;
                            }
                        }
                    }
                }
            }

            if (Variable_frame.Length > 0 && reg_leido <= 2)
            {
                reg_envio = reg_envio + reg_leido;
                strcomando = "GP" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                SR.SendCOMSerial(ref serialPort1, Variable_frame, ref Msj_recibido);
                if (Msj_recibido != null)
                {
                    if (Msj_recibido[0].IndexOf("Error") >= 0)
                    {
                        Env.Guardar_Trama_pendiente(Num_Bascula, Num_Grupo, strcomando);
                        TotalNoEnviados++;
                    }
                    if (Msj_recibido[0].IndexOf("Ok") >= 0)
                    {
                        Env.Crea_Producto_Detalle(myCurrent.idbas, myCurrent.gpo, Variable_frame.Split(chr), false, true);
                        TotalEnviados++;
                    }
                }
                pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[249, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                reg_leido = 0;
                Variable_frame = "";
            }
            Console.WriteLine("{0}(): Reg_envio = {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, reg_envio);
            if (reg_envio > 0)
            {
                Console.WriteLine("{0}(): Command GPF0 sent", System.Reflection.MethodBase.GetCurrentMethod().Name);
                SR.SendCOMSerial(ref serialPort1, "GPF0" + (char)9 + (char)10);
            }
        }
*/
        private void Enviar_Carpetas_Bascula(string direccionIP, ref Socket Cliente_bascula, ref int TotalEnviado, ref int TotalNoEnviado, ref ProgressContinue pro)
        {
            string Msg_Recibido;
            string Variable_frame = "";
            int reg_leido = 0;
            int reg_envio = 0;
            int reg_total;

            DataRow[] DR;
            DR = baseDeDatosDataSet.carpeta_detalle.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);

            reg_total = DR.Length;
            
            pro.IniciaProcess(reg_total, Variable.SYS_MSJ[250, Variable.idioma] + " " + myCurrent.Nserie + "... ");  //"Enviando Carpetas","Iniciando proceso");

            foreach (DataRow DR_Folder in DR)
            {
                Variable_frame = Variable_frame + Env.Genera_Trama_Carpeta(DR_Folder);

                reg_leido++;
                pro.UpdateProcess(1, Variable.SYS_MSJ[250, Variable.idioma] + " " + myCurrent.Nserie + "... ");

                if (reg_leido > 4)
                {
                    reg_envio = reg_envio + reg_leido;
                    Msg_Recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo, "GC");                    
                    reg_leido = 0;
                    Variable_frame = "";
                }
            }
            if (Variable_frame.Length > 0 && reg_leido <= 4)
            {
                reg_envio = reg_envio + reg_leido;
                Msg_Recibido = Env.Command_Enviado(reg_leido, Variable_frame, direccionIP, ref Cliente_bascula, Num_Bascula, Num_Grupo, "GC");
                reg_leido = 0;
                Variable_frame = "";
            }            
        }
        private void Enviar_Carpetas_Bascula(ref SerialPort serialPort1, int SoloUno, ref int TotalEnviado, ref int TotalNoEnviado, ref ProgressContinue pro) 
        {
            string[] Msg_Recibido = null;
            string strcomando;
            string Variable_frame = null;
            int reg_leido = 0;
            int reg_envio = 0;
            int reg_total;

            Console.WriteLine("{0}(): IN", System.Reflection.MethodBase.GetCurrentMethod().Name);

            DataRow[] DR;
            if (SoloUno == 0)
                DR = baseDeDatosDataSet.carpeta_detalle.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo);
            else
                DR = baseDeDatosDataSet.carpeta_detalle.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo + "and pendiente = " + true);

            Variable_frame = "";
            reg_total = DR.Length;

            pro.IniciaProcess(reg_total, Variable.SYS_MSJ[250, Variable.idioma] + " " + myCurrent.Nserie + "... ");

            foreach (DataRow DR_Folder in DR)
            {
                Variable_frame = Variable_frame + Env.Genera_Trama_Carpeta(DR_Folder);

                reg_leido++;
                if (reg_leido > 4)
                {
                    reg_envio = reg_envio + reg_leido;

                    Variable_frame = "GC" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                    strcomando = Variable_frame;
                    SR.SendCOMSerial(ref serialPort1, Variable_frame, ref Msg_Recibido);
                    if (Msg_Recibido != null)
                    {
                        if (Msg_Recibido[0].IndexOf("Error") >= 0)
                        {
                            Env.Guardar_Trama_pendiente(Num_Bascula, Num_Grupo, strcomando);
                        }
                        pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[250, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                    }
                    reg_leido = 0;
                    Variable_frame = "";
                }
            }
            if (Variable_frame.Length > 0 && reg_leido <= 4)
            {
                reg_envio = reg_envio + reg_leido;

                Variable_frame = "GC" + reg_leido.ToString().PadLeft(2, '0') + Variable_frame;
                strcomando = Variable_frame;
                SR.SendCOMSerial(ref serialPort1, Variable_frame, ref Msg_Recibido);
                if (Msg_Recibido != null)
                {
                    if (Msg_Recibido[0].IndexOf("Error") >= 0) { Env.Guardar_Trama_pendiente(Num_Bascula, Num_Grupo, strcomando); }
                    pro.UpdateProcess(reg_leido, Variable.SYS_MSJ[250, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                }
                reg_leido = 0;
                Variable_frame = "";
            }
            
        }

        private void Enviar_Imagen_Carpeta(string direccionIP, TreeNodeCollection ListNodes, int reg_envio, ref Socket Cliente_bascula, ref ProgressContinue pro)
        {
            int iRtaFunct = 0;
            int posicion = 0;
            string ImagenAEnviar = "";
            int reg_total = tvwCarpetas.GetNodeCount(true);

            CommandTorrey myobj = new CommandTorrey();

            foreach (TreeNodeBound Nod_fold in ListNodes)
            {
                TreeNodeBound Nod_prod = (TreeNodeBound)Nod_fold;
                reg_envio++;

                DataRow[] DR_Folder = baseDeDatosDataSet.carpeta_detalle.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo + " and ID = " + Convert.ToInt32(Nod_prod.Value));
                if (DR_Folder.Length > 0)
                {
                    posicion = DR_Folder[0]["imagen"].ToString().LastIndexOf('\\');

                    pro.UpdateProcess(1, Variable.SYS_MSJ[328, Variable.idioma] + " " + myCurrent.Nserie + "... ");

                    if (posicion > 0)
                    {             
                        ImagenAEnviar = DR_Folder[0]["imagen"].ToString();
                        iRtaFunct = myobj.TORREYSendImagesToScale(direccionIP, ImagenAEnviar, "Product", Cliente_bascula);

                        if (iRtaFunct > 0)
                        {
                            //pro.UpdateProcess(1, "Enviando imagen a báscula IP " + direccionIP);
                        }
                        else
                        {
                            //pro.UpdateProcess(1, reg_envio.ToString() + "/" + reg_total.ToString() + " " + Variable.SYS_MSJ[275, Variable.idioma] + " " + ImagenAEnviar);
                        }
                    }
                    //else pro.UpdateProcess(1, reg_envio.ToString() + "/" + reg_total.ToString() + " " + Variable.SYS_MSJ[280, Variable.idioma]);

                }

                if (Nod_fold.Nodes.Count > 0)
                {
                   // this.Enviar_Imagen_Carpeta(direccionIP, Nod_fold.Nodes, reg_envio, ref Cliente_bascula, ref pro);
                }
            }
        }
        private void Enviar_Imagen_Carpeta(TreeNodeCollection ListNodes, int reg_envio, ref ProgressContinue pro)
        {
            int iRtaFunct = 0;
            int posicion = 0;
            string ImagenAEnviar = "";
            int reg_total = tvwCarpetas.GetNodeCount(true);

            CommandTorrey myobj = new CommandTorrey();

            Console.WriteLine("{0}(): IN", System.Reflection.MethodBase.GetCurrentMethod().Name);

            foreach (TreeNodeBound Nod_fold in ListNodes)
            {
                TreeNodeBound Nod_prod = (TreeNodeBound)Nod_fold;
                reg_envio++;

                DataRow[] DR_Folder = baseDeDatosDataSet.carpeta_detalle.Select("id_bascula = " + Num_Bascula + " and id_grupo = " + Num_Grupo + " and ID = " + Convert.ToInt32(Nod_prod.Value));
                if (DR_Folder.Length > 0)
                {
                    posicion = DR_Folder[0]["imagen"].ToString().LastIndexOf('\\');

                    pro.UpdateProcess(1, Variable.SYS_MSJ[328, Variable.idioma] + " " + myCurrent.Nserie + "... ");

                    if (posicion > 0)
                    {
                        //CarpetaAEnviar = DR_Folder[0]["imagen"].ToString().Substring(0, posicion);
                        ImagenAEnviar = DR_Folder[0]["imagen"].ToString();
                        SR.ClosePort(ref serialPort1);
                        iRtaFunct = myobj.TORREYSendImagesToScaleSerial(myCurrent.COMM, ImagenAEnviar, "Product");
                        serialPort1 = new SerialPort();
                        int iPortOpenRetries = 0;
                        while (!(SR.OpenPort(ref serialPort1, myCurrent.COMM, myCurrent.BAUD) && iPortOpenRetries < 3)) iPortOpenRetries++;
                        if (iPortOpenRetries >= 3) break;


                        /*if (iRtaFunct > 0)
                        {
                            pro.UpdateProcess(1, reg_envio.ToString() + "/" + reg_total.ToString() + " " + Variable.SYS_MSJ[276, Variable.idioma] + " " + ImagenAEnviar);
                        }
                        else pro.UpdateProcess(1, reg_envio.ToString() + "/" + reg_total.ToString() + " " + Variable.SYS_MSJ[275, Variable.idioma] + " " + ImagenAEnviar);*/
                    }
                    /*else pro.UpdateProcess(1, reg_envio.ToString() + "/" + reg_total.ToString() + " " + Variable.SYS_MSJ[280, Variable.idioma]);*/

                }

                if (Nod_fold.Nodes.Count > 0)
                {
                   // this.Enviar_Imagen_Carpeta(Nod_fold.Nodes, reg_envio, ref pro);
                }
            }

        }
        #endregion
        
        #region evento de toolstripmenuitem y para carpetas
        private void soloCarpetasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TotalEnviado;
            int TotalNoEnviado;
            int BasculasActualizadas = 0;
            int NumeroDeBaculas = 0;

            carpeta_detalleTableAdapter.Fill(baseDeDatosDataSet.carpeta_detalle);
            prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);
            try
            {
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
                            TotalEnviado = 0;
                            TotalNoEnviado = 0;
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

                                        pro.IniciaProcess(tvwCarpetas.GetNodeCount(true), Variable.SYS_MSJ[252, Variable.idioma] + " " + myCurrent.Nserie + "... ");

                                        //Enviar_Imagen_Carpeta(myCurrent.ip, tvwCarpetas.Nodes, 0, ref Cliente_bascula, ref pro);

                                        if (Envia_Borrar_Carpetas(myCurrent.ip, ref Cliente_bascula))
                                        {
                                            Enviar_Carpetas_Bascula(myCurrent.ip, ref Cliente_bascula, ref TotalEnviado, ref TotalNoEnviado, ref pro);  //, Num_gpo, Num_basc);
                                        }
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
                                pro.IniciaProcess(tvwCarpetas.GetNodeCount(true), Variable.SYS_MSJ[252, Variable.idioma] + " " + myCurrent.Nserie + "... ");
                                serialPort1 = new SerialPort();
                                if (SR.OpenPort(ref serialPort1, myCurrent.COMM, myCurrent.BAUD))
                                {
                                    SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)10);
                                    //Enviar_Imagen_Carpeta(tvwCarpetas.Nodes, 0, ref pro);
                                    Envia_Borrar_Carpetas(ref serialPort1);
                                    Enviar_Carpetas_Bascula(ref serialPort1, 0, ref TotalEnviado, ref TotalNoEnviado, ref pro);
                                    Envia_Borrar_Asociacion(ref serialPort1);
                                    Enviar_DetalleCarpeta_Bascula(ref serialPort1, ref pro);
                                    Enviar_DetalleProducto_Bascula(ref serialPort1, ref pro);
                                    Cambiar_Estado_pendiente();
                                    SR.SendCOMSerial(ref serialPort1, "dXXX" + (char)10);
                                    SR.ClosePort(ref serialPort1);
                                }
                                else
                                {
                                    Form1.toolLabel.Text = Variable.SYS_MSJ[193, Variable.idioma];  // "Proceso concluido .....";
                                    pro.TerminaProcess();
                                    Thread.Sleep(500);
                                    MessageBox.Show(this, Variable.SYS_MSJ[426, Variable.idioma],
                                        Variable.SYS_MSJ[39, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
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
                            TotalEnviado = 0;
                            TotalNoEnviado = 0;
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

                                        pro.IniciaProcess(tvwCarpetas.GetNodeCount(true), Variable.SYS_MSJ[252, Variable.idioma] + " " + myCurrent.Nserie + "... ");

                                        //Enviar_Imagen_Carpeta(myCurrent.ip, tvwCarpetas.Nodes, 0, ref Cliente_bascula, ref pro);

                                        if (Envia_Borrar_Carpetas(myCurrent.ip, ref Cliente_bascula))
                                        {
                                            Enviar_Carpetas_Bascula(myCurrent.ip, ref Cliente_bascula, ref TotalEnviado, ref TotalNoEnviado, ref pro);  //, Num_gpo, Num_basc);
                                        }

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
                                    //Enviar_Imagen_Carpeta(tvwCarpetas.Nodes, 0, ref pro);
                                    Envia_Borrar_Carpetas(ref serialPort1);
                                    Enviar_Carpetas_Bascula(ref serialPort1, 0, ref TotalEnviado, ref TotalNoEnviado, ref pro);
                                    Envia_Borrar_Asociacion(ref serialPort1);
                                    Enviar_DetalleCarpeta_Bascula(ref serialPort1, ref pro);
                                    Enviar_DetalleProducto_Bascula(ref serialPort1, ref pro);
                                    Cambiar_Estado_pendiente();
                                    SR.SendCOMSerial(ref serialPort1, "dXXX" + (char)10);
                                    SR.ClosePort(ref serialPort1);
                                }
                                else
                                {
                                    Form1.toolLabel.Text = Variable.SYS_MSJ[193, Variable.idioma];  // "Proceso concluido .....";
                                    pro.TerminaProcess();
                                    Thread.Sleep(500);
                                    MessageBox.Show(this, Variable.SYS_MSJ[426, Variable.idioma],
                                        Variable.SYS_MSJ[39, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
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

        public void soloProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow[] pendi_carpeta;
            DataRow[] pendi_producto;
            int TotalEnviado = 0;
            int TotalNoEnviado = 0;
            int TotalBorrado = 0;
            int BasculasActualizadas = 0;
            int NumeroDeBaculas = 0;
            ProgressContinue pro = new ProgressContinue();
            carpeta_detalleTableAdapter.Fill(baseDeDatosDataSet.carpeta_detalle);
            pendi_carpeta = baseDeDatosDataSet.carpeta_detalle.Select("pendiente = " + true);

            prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);
            pendi_producto = baseDeDatosDataSet.Prod_detalle.Select("pendiente = " + true);
            try
            {
                pro.IniciaProcess(Variable.SYS_MSJ[192, Variable.idioma]);

                if (Num_Grupo != 0)
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
                            myCurrent.tipo = myScale[pos].tipo;
                            TotalEnviado = 0;
                            TotalNoEnviado = 0;
                            BasculasActualizadas++;

                            Form1.toolLabel.Text = Variable.SYS_MSJ[263, Variable.idioma];  // "Preparando Bascula.....";
                            if (myScale[pos].tipo == (int)ESTADO.tipoConexionesEnum.PKWIFI)
                            {
                                Cliente_bascula = Cte.conectar(myCurrent.ip, 50036);
                                if (Cliente_bascula != null)
                                {
                                    Cursor.Current = Cursors.WaitCursor;

                                    string sComando = "XX" + (char)9 + (char)10;
                                    string Msj_recibido = Env.Command_Enviado(1, sComando, myCurrent.ip, ref Cliente_bascula, 0, 0, "bX");

                                    if (Msj_recibido != null)
                                    {

                                        Enviar_Producto_Bascula(myCurrent.ip, ref Cliente_bascula, 1, ref TotalEnviado, ref TotalNoEnviado, ref TotalBorrado, ref pro);

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
                            else
                            {
                                serialPort1 = new SerialPort();
                                if (SR.OpenPort(ref serialPort1, myCurrent.COMM, myCurrent.BAUD))
                                {
                                    SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)10);
                                    SR.ReceivedCOMSerial(ref serialPort1);

                                    Enviar_Producto_Bascula(ref serialPort1, 0, ref TotalEnviado, ref TotalNoEnviado, ref TotalBorrado, ref pro);
                                    Envia_Borrar_Asociacion(ref serialPort1);
                                    Enviar_DetalleCarpeta_Bascula(ref serialPort1, ref pro);
                                    Enviar_DetalleProducto_Bascula(ref serialPort1, ref pro);
                                    Cambiar_Estado_pendiente();
                                    
                                    SR.SendCOMSerial(ref serialPort1, "dXXX" + (char)10);
                                    SR.ReceivedCOMSerial(ref serialPort1);
                                    
                                    SR.ClosePort(ref serialPort1);
                                }
                                else
                                {
                                    Form1.toolLabel.Text = Variable.SYS_MSJ[193, Variable.idioma];  // "Proceso concluido .....";
                                    pro.TerminaProcess();
                                    Thread.Sleep(500);
                                    MessageBox.Show(this, Variable.SYS_MSJ[426, Variable.idioma],
                                        Variable.SYS_MSJ[39, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
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
                            myCurrent.ip = myScale[pos].ip;  //direccion ip de la bascula
                            myCurrent.idbas = myScale[pos].idbas;   //numero id de la bascula
                            myCurrent.Nserie = myScale[pos].nserie;  //numero de serie de la bascula                    
                            myCurrent.nombre = myScale[pos].nombre;  //mombre de la bascula 
                            myCurrent.gpo = myScale[pos].gpo;  //Grupo al que pertenece la bascula
                            myCurrent.BAUD = myScale[pos].baud;
                            myCurrent.COMM = myScale[pos].pto;
                            myCurrent.tipo = myScale[pos].tipo;
                            TotalEnviado = 0;
                            TotalNoEnviado = 0;
                            BasculasActualizadas++;

                            Form1.toolLabel.Text = Variable.SYS_MSJ[263, Variable.idioma];  // "Preparando Bascula.....";
                            if (myScale[pos].tipo == (int)ESTADO.tipoConexionesEnum.PKWIFI)
                            {
                                Cliente_bascula = Cte.conectar(myCurrent.ip, 50036);
                                if (Cliente_bascula != null)
                                {
                                    Cursor.Current = Cursors.WaitCursor;

                                    string sComando = "XX" + (char)9 + (char)10;
                                    string Msj_recibido = Env.Command_Enviado(1, sComando, myCurrent.ip, ref Cliente_bascula, 0, 0, "bX");

                                    if (Msj_recibido != null)
                                    {
                                        Enviar_Producto_Bascula(myCurrent.ip, ref Cliente_bascula, 1, ref TotalEnviado, ref TotalNoEnviado, ref TotalBorrado, ref pro);

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
                            else
                            {
                                serialPort1 = new SerialPort();
                                if (SR.OpenPort(ref serialPort1, myCurrent.COMM, myCurrent.BAUD))
                                {
                                    SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)10);
                                    SR.ReceivedCOMSerial(ref serialPort1);

                                    Enviar_Producto_Bascula(ref serialPort1, 0, ref TotalEnviado, ref TotalNoEnviado, ref TotalBorrado, ref pro);
                                    Envia_Borrar_Asociacion(ref serialPort1);
                                    Enviar_DetalleCarpeta_Bascula(ref serialPort1, ref pro);
                                    Enviar_DetalleProducto_Bascula(ref serialPort1, ref pro);
                                    Cambiar_Estado_pendiente();
                                    SR.SendCOMSerial(ref serialPort1, "dXXX" + (char)10);
                                    SR.ReceivedCOMSerial(ref serialPort1);
                                    SR.ClosePort(ref serialPort1);
                                }
                                else
                                {
                                    Form1.toolLabel.Text = Variable.SYS_MSJ[193, Variable.idioma];  // "Proceso concluido .....";
                                    pro.TerminaProcess();
                                    Thread.Sleep(500);
                                    MessageBox.Show(this, Variable.SYS_MSJ[426, Variable.idioma],
                                        Variable.SYS_MSJ[39, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }
                            break;
                        }
                    }
                }
                Form1.toolLabel.Text = Variable.SYS_MSJ[193, Variable.idioma];  // "Proceso concluido .....";  
                pro.TerminaProcess();
                Thread.Sleep(500);

                MessageBox.Show(this, Variable.SYS_MSJ[418, Variable.idioma] + " " + BasculasActualizadas + " "
                    + Variable.SYS_MSJ[419, Variable.idioma] + " " + NumeroDeBaculas,
                    Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Form1.toolLabel.Text = Variable.SYS_MSJ[193, Variable.idioma];  // "Proceso concluido .....";
                pro.TerminaProcess();
                Thread.Sleep(500);
                SR.ClosePort(ref serialPort1);
                MessageBox.Show(this, "Error: " + ex.Message + " " + Variable.SYS_MSJ[427, Variable.idioma],
                    Variable.SYS_MSJ[39, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
        }

        private void TodaEstructuraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow[] pendi_carpeta;
            DataRow[] pendi_producto;
            int TotalEnviado = 0;
            int TotalNoEnviado = 0;
            int TotalBorrado = 0;
            int BasculasActualizadas = 0;
            int NumeroDeBaculas = 0;
            Console.WriteLine("{0}(): IN", System.Reflection.MethodBase.GetCurrentMethod().Name);

            carpeta_detalleTableAdapter.Fill(baseDeDatosDataSet.carpeta_detalle);
            pendi_carpeta = baseDeDatosDataSet.carpeta_detalle.Select("pendiente = " + true);

            prod_detalleTableAdapter.Fill(baseDeDatosDataSet.Prod_detalle);
            pendi_producto = baseDeDatosDataSet.Prod_detalle.Select("pendiente = " + true);

            ProgressContinue pro = new ProgressContinue();

            try
            {
                pro.IniciaProcess(Variable.SYS_MSJ[192, Variable.idioma]);

                if (Num_Grupo != 0)
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
                            myCurrent.tipo = myScale[pos].tipo;
                            myCurrent.BAUD = myScale[pos].baud;
                            myCurrent.COMM = myScale[pos].pto;
                            TotalEnviado = 0;
                            TotalNoEnviado = 0;
                            BasculasActualizadas++;

                            Form1.toolLabel.Text = Variable.SYS_MSJ[263, Variable.idioma]; // "Preparando bascula......";

                            if (myScale[pos].tipo == (int)ESTADO.tipoConexionesEnum.PKWIFI)
                            {
                                Cliente_bascula = Cte.conectar(myCurrent.ip, 50036);
                                if (Cliente_bascula != null)
                                {
                                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                                    string sComando = "XX" + (char)9 + (char)10;
                                    string Msj_recibido = Env.Command_Enviado(1, sComando, myCurrent.ip, ref Cliente_bascula, 0, 0, "bX");

                                    if (Msj_recibido != null)
                                    {
                                        Form1.toolLabel.Text = Variable.SYS_MSJ[252, Variable.idioma];  // "Enviando imagenes.....";

                                        pro.IniciaProcess(tvwCarpetas.GetNodeCount(true), Variable.SYS_MSJ[252, Variable.idioma] + " " + myCurrent.Nserie + "... ");  //"Enviando Imagenes", "Iniciando proceso");                          

                                        //Enviar_Imagen_Carpeta(myCurrent.ip, tvwCarpetas.Nodes, 0, ref Cliente_bascula, ref pro);

                                        if (Envia_Borrar_Carpetas(myCurrent.ip, ref Cliente_bascula))
                                        {
                                            Enviar_Carpetas_Bascula(myCurrent.ip, ref Cliente_bascula, ref TotalEnviado, ref TotalNoEnviado, ref pro);
                                        }
                                        if (Envia_Borrar_Asociacion(myCurrent.ip, ref Cliente_bascula))
                                        {
                                            if (Enviar_Producto_Bascula(myCurrent.ip, ref Cliente_bascula, 0, ref TotalEnviado, ref TotalNoEnviado, ref TotalBorrado, ref pro))
                                            {
                                                BasculasActualizadas--;
                                                if (MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma] + " "
                                                    + myCurrent.Nserie + " " + Variable.SYS_MSJ[417, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma],
                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No) break;
                                            }
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
                                        }

                                    }
                                    Cte.desconectar(ref Cliente_bascula);

                                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                                }
                                else
                                {
                                    BasculasActualizadas--;
                                    if (MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma] + " "
                                        + myCurrent.Nserie + " " + Variable.SYS_MSJ[417, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma],
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No) break;
                                }

                            }
                            else
                            {
                                serialPort1 = new SerialPort();
                                if (SR.OpenPort(ref serialPort1, myCurrent.COMM, myCurrent.BAUD)) //Variable.P_COMM, Variable.Buad))
                                {
                                    SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)10);
                                    SR.ReceivedCOMSerial(ref serialPort1);

                                    pro.IniciaProcess(tvwCarpetas.GetNodeCount(true), Variable.SYS_MSJ[328, Variable.idioma] + " " + myCurrent.Nserie + "... ");  //"Enviando Imagenes", "Iniciando proceso");

                                   // Enviar_Imagen_Carpeta(tvwCarpetas.Nodes, 0, ref pro);

                                    Envia_Borrar_Carpetas(ref serialPort1);
                                    Envia_Borrar_Asociacion(ref serialPort1);

                                    Enviar_Producto_Bascula(ref serialPort1, 0, ref TotalEnviado, ref TotalNoEnviado, ref TotalBorrado, ref pro);
                                    Enviar_Carpetas_Bascula(ref serialPort1, 0, ref TotalEnviado, ref TotalNoEnviado, ref pro);
                                    Enviar_DetalleCarpeta_Bascula(ref serialPort1, ref pro);
                                    Enviar_DetalleProducto_Bascula(ref serialPort1, ref pro);
                                    Cambiar_Estado_pendiente();
                                    SR.SendCOMSerial(ref serialPort1, "dXXX" + (char)10);
                                    SR.ReceivedCOMSerial(ref serialPort1);

                                    SR.ClosePort(ref serialPort1);
                                }
                                else
                                {
                                    Form1.toolLabel.Text = Variable.SYS_MSJ[193, Variable.idioma];  // "Proceso concluido .....";
                                    pro.TerminaProcess();
                                    Thread.Sleep(500);
                                    MessageBox.Show(this, Variable.SYS_MSJ[426, Variable.idioma],
                                        Variable.SYS_MSJ[39, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
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
                            myCurrent.ip = myScale[pos].ip;  //direccion ip de la bascula
                            myCurrent.idbas = myScale[pos].idbas;   //numero id de la bascula
                            myCurrent.Nserie = myScale[pos].nserie;  //numero de serie de la bascula                    
                            myCurrent.nombre = myScale[pos].nombre;  //mombre de la bascula                         
                            myCurrent.gpo = myScale[pos].gpo;  //Grupo al que pertenece la bascula
                            myCurrent.BAUD = myScale[pos].baud;
                            myCurrent.COMM = myScale[pos].pto;
                            myCurrent.tipo = myScale[pos].tipo;
                            TotalEnviado = 0;
                            TotalNoEnviado = 0;
                            BasculasActualizadas++;

                            Form1.toolLabel.Text = Variable.SYS_MSJ[263, Variable.idioma];  // "Preparando bascula.....";
                            if (myScale[pos].tipo == 0)
                            {
                                Cliente_bascula = Cte.conectar(myCurrent.ip, 50036);
                                if (Cliente_bascula != null)
                                {
                                    //Cursor.Current = Cursors.WaitCursor;
                                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                                    string sComando = "XX" + (char)9 + (char)10;
                                    string Msj_recibido = Env.Command_Enviado(1, sComando, myCurrent.ip, ref Cliente_bascula, 0, 0, "bX");

                                    if (Msj_recibido != null)
                                    {

                                        pro.IniciaProcess(tvwCarpetas.GetNodeCount(true), Variable.SYS_MSJ[252, Variable.idioma] + " " + myCurrent.Nserie + "... ");

                                        //Enviar_Imagen_Carpeta(myCurrent.ip, tvwCarpetas.Nodes, 0, ref Cliente_bascula, ref pro);

                                        if (Envia_Borrar_Carpetas(myScale[pos].ip, ref Cliente_bascula))
                                        {
                                            Enviar_Carpetas_Bascula(myCurrent.ip, ref Cliente_bascula, ref TotalEnviado, ref TotalNoEnviado, ref pro);
                                        }
                                        if (Envia_Borrar_Asociacion(myCurrent.ip, ref Cliente_bascula))
                                        {
                                            if (Enviar_Producto_Bascula(myCurrent.ip, ref Cliente_bascula, 0, ref TotalEnviado, ref TotalNoEnviado, ref TotalBorrado, ref pro))
                                            {
                                                BasculasActualizadas--;
                                                MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma]
                                                    + " " + myCurrent.Nserie + ".", Variable.SYS_MSJ[42, Variable.idioma],
                                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation); 
                                                break;
                                            }
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
                                    Form1.toolLabel.Text = Variable.SYS_MSJ[252, Variable.idioma];  // "Enviando imagenes.....";
                                    Cte.desconectar(ref Cliente_bascula);
                                    //Cursor.Current = Cursors.Default;
                                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
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
                            else
                            {
                                serialPort1 = new SerialPort();
                                if (SR.OpenPort(ref serialPort1, myCurrent.COMM, myCurrent.BAUD)) //Variable.P_COMM, Variable.Buad))
                                {
                                    SR.SendCOMSerial(ref serialPort1, "bXXX" + (char)10);
                                    SR.ReceivedCOMSerial(ref serialPort1);

                                    pro.IniciaProcess(tvwCarpetas.GetNodeCount(true), Variable.SYS_MSJ[252, Variable.idioma] + " " + myCurrent.Nserie + "... ");  //"Enviando Imagenes", "Iniciando proceso");

                                    //Enviar_Imagen_Carpeta(tvwCarpetas.Nodes, 0, ref pro);

                                    Envia_Borrar_Carpetas(ref serialPort1);
                                    Envia_Borrar_Asociacion(ref serialPort1);

                                    Enviar_Producto_Bascula(ref serialPort1, 0, ref TotalEnviado, ref TotalNoEnviado, ref TotalBorrado, ref pro);
                                    Enviar_Carpetas_Bascula(ref serialPort1, 0, ref TotalEnviado, ref TotalNoEnviado, ref pro);
                                    Enviar_DetalleCarpeta_Bascula(ref serialPort1, ref pro);
                                    Enviar_DetalleProducto_Bascula(ref serialPort1, ref pro);
                                    Cambiar_Estado_pendiente();
                                    SR.SendCOMSerial(ref serialPort1, "dXXX" + (char)10);
                                    SR.ReceivedCOMSerial(ref serialPort1);

                                    SR.ClosePort(ref serialPort1);
                                }
                                else
                                {
                                    Form1.toolLabel.Text = Variable.SYS_MSJ[193, Variable.idioma];  // "Proceso concluido .....";
                                    pro.TerminaProcess();
                                    Thread.Sleep(500);
                                    MessageBox.Show(this, Variable.SYS_MSJ[426, Variable.idioma],
                                        Variable.SYS_MSJ[39, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }                            
                            break;
                        }
                        
                    }
                }
                Form1.toolLabel.Text = Variable.SYS_MSJ[193, Variable.idioma];  // "Proceso concluido .....";
                pro.TerminaProcess();
                Thread.Sleep(500);
                MessageBox.Show(this, Variable.SYS_MSJ[418, Variable.idioma] + " " + BasculasActualizadas + " "
                        + Variable.SYS_MSJ[419, Variable.idioma] + " " + NumeroDeBaculas,
                        Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Variable.SYS_MSJ[381, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                pro.TerminaProcess();
                Thread.Sleep(500);
            }
        }

        /*
        private void SoloImagenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            carpeta_detalleTableAdapter.Fill(baseDeDatosDataSet.carpeta_detalle);
            try
            {
                ProgressContinue pro = new ProgressContinue();

                if (Num_Grupo != 0)
                {
                    for (int pos = 0; pos < myScale.Length; pos++)
                    {
                        if (myScale[pos].gpo == Num_Grupo)
                        {
                            myCurrent.ip = myScale[pos].ip;  //direccion ip de la bascula
                            myCurrent.idbas = myScale[pos].idbas;   //numero id de la bascula
                            myCurrent.Nserie = myScale[pos].nserie;  //numero de serie de la bascula                    
                            myCurrent.nombre = myScale[pos].nombre;  //mombre de la bascula 
                            myCurrent.gpo = myScale[pos].gpo;  //Grupo al que pertenece la bascula
                            myCurrent.tipo = myScale[pos].tipo;
                            myCurrent.BAUD = myScale[pos].baud;
                            myCurrent.COMM = myScale[pos].pto;

                            Form1.toolLabel.Text = Variable.SYS_MSJ[252, Variable.idioma];  // "Enviando imagenes.....";

                            pro.IniciaProcess(tvwCarpetas.GetNodeCount(true), Variable.SYS_MSJ[252, Variable.idioma] + " " + myCurrent.Nserie + "... ");

                            if (myScale[pos].tipo == (int)ESTADO.tipoConexionesEnum.PKWIFI)
                            {
                                Cliente_bascula = Cte.conectar(myCurrent.ip, 50036);  //, Variable.frame, ref Dato_Recivido);
                                if (Cliente_bascula != null)
                                {
                                    Enviar_Imagen_Carpeta(myScale[pos].ip, tvwCarpetas.Nodes, 0, ref Cliente_bascula, ref pro);

                                    Cte.desconectar(ref Cliente_bascula);
                                }
                            }
                            else Enviar_Imagen_Carpeta(tvwCarpetas.Nodes, 0, ref pro);
                        }
                    }
                }
                else
                {
                    for (int pos = 0; pos < myScale.Length; pos++)
                    {
                        if (myScale[pos].idbas == Num_Bascula)
                        {
                            myCurrent.ip = myScale[pos].ip;  //direccion ip de la bascula
                            myCurrent.idbas = myScale[pos].idbas;   //numero id de la bascula
                            myCurrent.Nserie = myScale[pos].nserie;  //numero de serie de la bascula                    
                            myCurrent.nombre = myScale[pos].nombre;  //mombre de la bascula 
                            myCurrent.gpo = myScale[pos].gpo;  //Grupo al que pertenece la bascula
                            myCurrent.tipo = myScale[pos].tipo;
                            myCurrent.BAUD = myScale[pos].baud;
                            myCurrent.COMM = myScale[pos].pto;

                            Form1.toolLabel.Text = Variable.SYS_MSJ[252, Variable.idioma];  // "Enviando imagenes.....";

                            pro.IniciaProcess(tvwCarpetas.GetNodeCount(true), Variable.SYS_MSJ[252, Variable.idioma] + " " + myCurrent.Nserie + "... ");  //"Enviando Imagenes", "Iniciando proceso");

                            if (myScale[pos].tipo == (int)ESTADO.tipoConexionesEnum.PKWIFI)
                            {
                                Cliente_bascula = Cte.conectar(myCurrent.ip, 50036);  //, Variable.frame, ref Dato_Recivido);
                                if (Cliente_bascula != null)
                                {

                                    string sComando = "XX" + (char)9 + (char)10;
                                    string Msj_recibido = Env.Command_Enviado(1, sComando, myCurrent.ip, ref Cliente_bascula, 0, 0, "bX");

                                    if (Msj_recibido != null)
                                    {
                                        Enviar_Imagen_Carpeta(myScale[pos].ip, tvwCarpetas.Nodes, 0, ref Cliente_bascula, ref pro);
                                    }

                                    Cte.desconectar(ref Cliente_bascula);
                                }
                            }
                            else Enviar_Imagen_Carpeta(tvwCarpetas.Nodes, 0, ref pro);

                            break;
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
                Form1.toolLabel.Text = Variable.SYS_MSJ[193, Variable.idioma];  // "Proceso concluido .....";    
                pro.TerminaProcess();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Variable.SYS_MSJ[381, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
            }  
        }
        */

        private void StripCerrar_Click(object sender, EventArgs e)
        {
            ToolStripManager.RevertMerge("toolStrip3");
            this.Dispose();
        }

        private void StripProductos_Click(object sender, EventArgs e)
        {
            StripProductos.Enabled = false;
            Asigna_Productos();
            RegistroEstado = ESTADO.EstadoRegistro.PKNOTRATADO;
        }
        #endregion

        

       
    }

}



