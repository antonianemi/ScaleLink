using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.Common;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using Validaciones;

namespace MainMenu
{
    
    public partial class f1Basculas : MdiChildForm
    {   
        #region Declaracion de Class     
        ADOutil Conec = new ADOutil();
        CheckSum Chk = new CheckSum();
        UserGrupos UsGpo = new UserGrupos();
        UserBasculas UsBasc;
        Variable.lgrupo[] myGrupo;
        Variable.lbasc[] myScale;     
        #endregion

        #region Constantes y variable
        private TreeNode StartNode = null;
        private TreeNode OriginalNode = new TreeNode("", 1, 1);
        private TreeNode RootNode = new TreeNode("", 1, 0);
        private Point Position;
        private int iActionToSave = 0;      //0-> Grupo o bascula nueva; 1-> Grupo o bascula editada.
        private Form1 _form1;
        #endregion

        #region Inicio
        public f1Basculas(Form1 form1)
        {
            InitializeComponent();
            treListadoB.AllowDrop = true;
            treListadoB.TabIndex = 0;
            treListadoB.DragDrop += new System.Windows.Forms.DragEventHandler(treeListado_DragDrop);
            treListadoB.DragEnter += new System.Windows.Forms.DragEventHandler(treeListado_DragEnter);
            treListadoB.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(treeListado_ItemDrag);
            treListadoB.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeListado_AfterSelect);
            toolStripLabel2.Text = String.Format(Variable.F_Hora, DateTime.Now) + " " + string.Format(Variable.FOR_FECHAS[Variable.ffecha], DateTime.Now);
            this._form1 = form1;
            UsBasc = new UserBasculas(this._form1);
        }
        #endregion

        #region Porcesos de pushBotones
        private void treeListado_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView nodo = ((TreeView)sender);
            nodo.Select();
            nodo.Focus();              
            Consulta_EnBD(nodo.SelectedNode.Name,nodo.SelectedNode.Tag.ToString());
        }

        /// <summary>
        /// Captura el evento soltar del arbol de grupos y basculas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListado_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            int iIndexGroup = 0;
            int iIndexScale = 0;
            Position.X = e.X;
            Position.Y = e.Y;
            Position = this.treListadoB.PointToClient(Position);
            TreeNode DropNode = this.treListadoB.GetNodeAt(Position);
            TreeNode DragNode = ((System.Windows.Forms.TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode"));
            DialogResult answer;
            TreeNode nodo_i = DragNode;

            if (treListadoB.SelectedNode == null || treListadoB.SelectedNode.Tag.ToString() != "B") //Solo se pueden mover basculas.
            {
                return;
            }

            iIndexScale = buscar_nodo(DragNode.Text);

            if (DropNode != null)
            {
                iIndexGroup = buscar_nodo_grupo(DropNode.Text);

                if (iIndexScale >= 0 && iIndexGroup >= 0 && myScale[iIndexScale].gpo == myGrupo[iIndexGroup].ngpo)
                {
                    return;
                }
            }

            if (iIndexScale > myScale.Length || iIndexScale < 0)   //Verifica que no se exceda el indice de las basculas
            {
                return;
            }

            if (DropNode == null)
            {
                answer = MessageBox.Show(this, Variable.SYS_MSJ[23, Variable.idioma] + " " + DragNode.Text + (char)13 + Variable.SYS_MSJ[190, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                switch (answer)
                {
                    case DialogResult.Yes:
                        DragNode.Remove();
                        this.treListadoB.Nodes.Add(nodo_i);
                        Asignar_Grupo(0, myScale[iIndexScale].ip, Convert.ToInt32(myScale[iIndexScale].idbas));
                        myScale[iIndexScale].gpo = 0;
                        Asigna_Grupo();
                        Asigna_Bascula();
                        break;

                    case DialogResult.No:
                        break;

                }
            }
            else
            {
                iIndexGroup = buscar_nodo_grupo(DropNode.Text);

                if (iIndexGroup > myGrupo.Length)   //Verifica que no se exceda el indice de los grupos
                {
                    return;
                }

                if (DropNode != null && DropNode.Parent == this.StartNode.Parent && iIndexGroup != -1)
                {
                    if (DropNode.Nodes.Count > 0)
                    {
                        if (this.buscar_capacidad(Convert.ToInt16(myGrupo[iIndexGroup].ngpo), myScale[iIndexScale].um) == false || this.buscar_modelo(Convert.ToInt16(myGrupo[iIndexGroup].ngpo), myScale[iIndexScale].modelo) == false)
                        {
                            answer = MessageBox.Show(this, DragNode.Text + Variable.SYS_MSJ[47, Variable.idioma] + DropNode.Text, Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                           //return;
                        }
                    }

                    answer = MessageBox.Show(this, Variable.SYS_MSJ[23, Variable.idioma] + " " + DragNode.Text + (char)13 + Variable.SYS_MSJ[192, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                    switch (answer)
                    {
                        case DialogResult.Yes:
                            DragNode.Remove();
                            DropNode.Nodes.Add(nodo_i);
                            myScale[iIndexScale].gpo = Convert.ToInt16(myGrupo[iIndexGroup].ngpo);
                            Asignar_Grupo(Convert.ToInt16(myGrupo[iIndexGroup].ngpo), myScale[iIndexScale].ip, Convert.ToInt16(myScale[iIndexScale].idbas));
                            Asigna_Grupo();
                            Asigna_Bascula();
                            break;

                        case DialogResult.No:
                            break;
                    }
                }
                else
                {
                    if (iIndexGroup != -1)
                    {
                        if (DropNode.Nodes.Count > 0)
                        {
                            if (this.buscar_capacidad(Convert.ToInt16(myGrupo[iIndexGroup].ngpo), myScale[iIndexScale].um) == false || this.buscar_modelo(Convert.ToInt16(myGrupo[iIndexGroup].ngpo), myScale[iIndexScale].modelo) == false)
                            {
                                answer = MessageBox.Show(this, DragNode.Text + Variable.SYS_MSJ[47, Variable.idioma] + DropNode.Text, Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                //return;
                            }
                        }

                        answer = MessageBox.Show(this, Variable.SYS_MSJ[23, Variable.idioma] + " " + DragNode.Text + (char)13 + Variable.SYS_MSJ[192, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                        switch (answer)
                        {
                            case DialogResult.Yes:

                                DragNode.Remove();
                                DropNode.Nodes.Add(nodo_i);
                                myScale[iIndexScale].gpo = Convert.ToInt16(myGrupo[iIndexGroup].ngpo);
                                Asignar_Grupo(myScale[iIndexScale].gpo, myScale[iIndexScale].ip, Convert.ToInt16(myScale[iIndexScale].idbas));
                                Asigna_Grupo();
                                Asigna_Bascula();
                                break;

                            case DialogResult.No:
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListado_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
        {
            StartNode = (System.Windows.Forms.TreeNode)e.Item;

            if (e.Button == MouseButtons.Left)
            {
                object strItem = e.Item;
                DoDragDrop(strItem, DragDropEffects.Move);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListado_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        
        private void treeListadoMenu_Click(object sender, System.EventArgs e)
        {
            MenuItem miScale = (MenuItem)sender;
            switch (miScale.Index)
            {
                case 0:
                    {  	// Nuevo Grupo
                        pnldetalle.Controls.Clear();
                        pnldetalle.Controls.Add(UsGpo);
                        UsGpo.comando(0, 0);
                    } break;
                case 1:
                    { // Borrar Grupo
                        int n_sele = this.treListadoB.SelectedNode.Index;
                        UsGpo.idgrup = this.treListadoB.SelectedNode.Name;
                        pnldetalle.Controls.Clear();
                        pnldetalle.Controls.Add(UsGpo);
                        UsGpo.comando(1, 0);
                        bool tiene = false;
                        try
                        {
                            for (int j = 0; j < myScale.Length; j++)
                            {
                                if (myScale[j].gpo == Convert.ToInt32(UsGpo.idgrup))
                                {
                                    tiene = true;
                                    break;
                                }
                            }

                            if (!tiene)
                            {
                                UsGpo.comando(1, 0);
                                this.treListadoB.Nodes.RemoveAt(n_sele);
                            }
                            else MessageBox.Show(this, Variable.SYS_MSJ[22, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);  //No tiene grupo
                        }
                        catch (Exception ex) { MessageBox.Show(this, ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    } break;
            }
        }

        private void Asignar_Grupo(int ngpo, string ip_bas, int n_idbas)
        {
            string sele = "UPDATE Bascula SET id_grupo = " + ngpo + " WHERE (id_bascula = " + n_idbas + ")";
            Conec.ActualizaReader(Conec.CadenaConexion, sele, "Bascula");
                       
            sele = "DELETE * FROM carpeta_detalle WHERE (id_bascula = " + n_idbas + ")";
            Conec.EliminarReader(Conec.CadenaConexion, sele, "carpeta_detalle");
            sele = "DELETE * FROM Oferta_Detalle WHERE (id_bascula = " + n_idbas + ")";
            Conec.EliminarReader(Conec.CadenaConexion, sele, "Oferta_Detalle");
            sele = "DELETE * FROM Prod_detalle WHERE (id_bascula = " + n_idbas + ")";
            Conec.EliminarReader(Conec.CadenaConexion, sele, "Prod_detalle");
            sele = "DELETE * FROM Public_Detalle WHERE (id_bascula = " + n_idbas + ")";
            Conec.EliminarReader(Conec.CadenaConexion, sele, "Public_Detalle");
            sele = "DELETE * FROM Ingre_detalle WHERE (id_bascula = " + n_idbas + ")";
            Conec.EliminarReader(Conec.CadenaConexion, sele, "Ingre_detalle");            
            sele = "DELETE * FROM Impresor WHERE (id_bascula = " + n_idbas + ")";
            Conec.EliminarReader(Conec.CadenaConexion, sele, "Impresor");
            sele = "DELETE * FROM Encabezado WHERE (id_bascula = " + n_idbas + ")";
            Conec.EliminarReader(Conec.CadenaConexion, sele, "Encabezado");
            sele = "DELETE * FROM Configuracion WHERE (id_bascula = " + n_idbas + ")";
            Conec.EliminarReader(Conec.CadenaConexion, sele, "Configuracion");
        }
        private int buscar_nodo(string n_serie)
        {
            int i;
            for (i = 0; i < myScale.Length; i++)
            {
                if (n_serie == myScale[i].nserie) 
                { 
                    return i;
                } 
            }
            return -1;
        }

        private bool buscar_capacidad(int grupo, string capacidad)
        {
            int i;
            for (i = 0; i < myScale.Length; i++)
            {
                if (myScale[i].gpo == grupo)
                {
                    if (myScale[i].um == capacidad) return true;
                    else return false;
                }
            }
            return true;
        }

        private bool buscar_modelo(int grupo, string modelo)
        {
            int i;
            for (i = 0; i < myScale.Length; i++)
            {
                if (myScale[i].gpo == grupo)
                {
                    if (myScale[i].modelo == modelo) return true;
                    else return false;
                }
            }
            return true;
        }

        private int buscar_nodo_grupo(string nombre)
        {
            int i;
            for (i = 0; i < myGrupo.Length; i++)
            {
                if (nombre.ToString() == myGrupo[i].nombre)
                { return i; }
            }
            return -1;
        }
        #endregion
        
        #region Registro de Base de Datos y DataSet
        private void Consulta_EnBD(string ncod,string tipo)
        {
            pnldetalle.Controls.Clear();
            if (tipo == "B")
            {
                DataRow dr = baseDeDatosDataSet1.Bascula.Rows.Find(Convert.ToInt64(ncod));
                if (dr != null)
                {
                    UsBasc.Dock = DockStyle.Fill;
                    UsBasc.idbasc = dr["id_bascula"].ToString();
                    pnldetalle.Controls.Add(UsBasc);
                    UsBasc.comando(6, 0);
                    if (this.basculasToolStripMenuItem.Enabled || this.grupoToolStripMenuItem.Enabled)
                    {
                        btnEditar.Enabled = true;
                        btnBorrar.Enabled = true;
                        btnGuardar.Enabled = false;
                    }
                    if (!this.basculasToolStripMenuItem.Enabled || !this.grupoToolStripMenuItem.Enabled)
                    {
                        btnEditar.Enabled = false;
                        btnBorrar.Enabled = false;
                        btnGuardar.Enabled = false;
                    }       
                }
            }
            else
            {
                DataRow dr = baseDeDatosDataSet1.Grupo.Rows.Find(Convert.ToInt64(ncod));
                if (dr != null)
                {
                    UsGpo.Dock = DockStyle.Fill;
                    UsGpo.idgrup = dr["id_grupo"].ToString();
                    pnldetalle.Controls.Add(UsGpo);
                    UsGpo.comando(6, 0);
                    if (this.basculasToolStripMenuItem.Enabled || this.grupoToolStripMenuItem.Enabled)
                    {
                        btnEditar.Enabled = true;
                        btnBorrar.Enabled = true;
                        btnGuardar.Enabled = false;
                    }
                    if (!this.basculasToolStripMenuItem.Enabled || !this.grupoToolStripMenuItem.Enabled)
                    {
                        btnEditar.Enabled = false;
                        btnBorrar.Enabled = false;
                        btnGuardar.Enabled = false;
                    }                    
                }
            }
        }
      

        void Asigna_Grupo()
        {
            if (this.treListadoB.GetNodeCount(false) > 0) this.treListadoB.SelectedNode = this.treListadoB.TopNode;
            Conec.CadenaSelect = "SELECT * FROM Grupo ORDER BY id_grupo";

            grupoTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            grupoTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            grupoTableAdapter.Fill(baseDeDatosDataSet1.Grupo);

            myGrupo = new Variable.lgrupo[baseDeDatosDataSet1.Grupo.Rows.Count];
            int nitem = 0;

            foreach (DataRow dr in baseDeDatosDataSet1.Grupo.Rows)
            {
                myGrupo[nitem].ngpo = Convert.ToInt32(dr["id_grupo"].ToString());
                myGrupo[nitem].nombre = dr["nombre_gpo"].ToString();
                //myGrupo[nitem].Modelo = dr["modelo"].ToString();
                nitem++;
            }
        }

        void Asigna_Bascula()
        {
            Conec.CadenaSelect = "SELECT * FROM Bascula ORDER BY id_bascula";

            this.basculaTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            this.basculaTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            this.basculaTableAdapter.Fill(baseDeDatosDataSet1.Bascula);
           

            myScale = new Variable.lbasc[baseDeDatosDataSet1.Bascula.Rows.Count];
            int nitem = 0;
            foreach (DataRow dr in baseDeDatosDataSet1.Bascula.Rows)
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

        private void Crear_Nodos()
        {
            treListadoB.Nodes.Clear();
            treListadoB.Refresh();
			btnEditar.Enabled = false;
            btnBorrar.Enabled = false;
            btnGuardar.Enabled = false;
            for (int i = 0; i < this.myGrupo.Length; i++)
            {
                if (myGrupo[i].nombre != "" && myGrupo[i].nombre != null)//Grupo
                {
                    //Verificar el tipo de Bascula para asignarle la imagen que le corresponde.

                    TreeNode trG = new TreeNode();
                    trG.Text = myGrupo[i].nombre;
                    trG.Name = myGrupo[i].ngpo.ToString();
                    trG.Tag = "G";
                    trG.SelectedImageIndex = 0;
                    trG.ImageIndex = 0;

                    this.treListadoB.Nodes.Add(trG);                    
                    this.treListadoB.Select();
                    int indi_nodo = this.treListadoB.Nodes.Count;

                    IEnumerable<Variable.lbasc> query = myScale.OrderBy(pet => pet.gpo);

                    foreach (Variable.lbasc pet in query)
                    {
                        if (pet.gpo == Convert.ToInt16(myGrupo[i].ngpo))//Bascula
                        {
                            TreeNode trB = new TreeNode();
                            trB.Text = pet.nserie;
                            trB.Name = pet.idbas.ToString();
                            trB.Tag = "B";

                            if (pet.modelo == "W-LABEL")
                            {
                                //trB.SelectedImageIndex = 2;
                                trB.ImageIndex = 2;
                            }
                            else if (pet.modelo == "WLS")
                            {
                                //trB.SelectedImageIndex = 1;
                                trB.ImageIndex = 1;
                            }
                            else if (pet.modelo == "WLSD")
                            {
                                //trB.SelectedImageIndex = 3;
                                trB.ImageIndex = 3;
                            }

                            this.treListadoB.Nodes[indi_nodo - 1].Nodes.Add(trB);
                        }
                    }
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


                    if (myScale[j].modelo == "W-LABEL")
                    {
                        trB.SelectedImageIndex = 2;
                        trB.ImageIndex = 2;
                    }
                    else if (myScale[j].modelo == "WLS")
                    {
                        trB.SelectedImageIndex = 1;
                        trB.ImageIndex = 1;
                    }
                    else if (myScale[j].modelo == "WLSD")
                    {
                        trB.SelectedImageIndex = 3;
                        trB.ImageIndex = 3;
                    }

                    
                    this.treListadoB.Nodes.Add(trB);
                }
            }
        }    
        #endregion

        private void f1BasculasLista_Load(object sender, EventArgs e)
        {            
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet1.Grupo' Puede moverla o quitarla según sea necesario.
            this.grupoTableAdapter.Fill(this.baseDeDatosDataSet1.Grupo);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet1.Bascula' Puede moverla o quitarla según sea necesario.
            this.basculaTableAdapter.Fill(this.baseDeDatosDataSet1.Bascula);

            if (this.treListadoB.GetNodeCount(false) > 0) this.treListadoB.SelectedNode = this.treListadoB.TopNode;
           
        }
        private void f1BasculasLista_Activated(object sender, EventArgs e)
        {
            Asigna_Grupo();
            Asigna_Bascula();
            Crear_Nodos();
            btnEditar.Enabled = false;
            btnGuardar.Enabled = false;
            if (this.treListadoB.GetNodeCount(false) > 0)
            {
                this.treListadoB.SelectedNode = this.treListadoB.TopNode;
                btnEditar.Enabled = true;
            }
            for (int i = 0; i < Variable.privilegio.Length; i++)
            {
                if (Variable.privilegio.Substring(i, 1) == "1")
                {
                    if (i == 2) { this.grupoToolStripMenuItem.Enabled = true; } //Alta de grupo
                    if (i == 3) { this.basculasToolStripMenuItem.Enabled = true; }  //Alta de Bascula
                }
                else
                {
                    if (i == 2) { this.grupoToolStripMenuItem.Enabled = false; } //Alta de grupo
                    if (i == 3) { this.basculasToolStripMenuItem.Enabled = false; }  //Alta de Bascula 
                }
            }
            if (this.grupoToolStripMenuItem.Enabled || this.basculasToolStripMenuItem.Enabled)
            {
                btnNuevo.Enabled = true;
            }
            else
            {
                btnNuevo.Enabled = false;
            }
        }
        private void btnNuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem men = ((ToolStripItem)sender);

            switch (this.btnNuevo.DropDown.Items.IndexOf(men))
            {
                case 0:
                    iActionToSave = 0;
                    UsGpo.Dock = DockStyle.Fill;
                    this.pnldetalle.Controls.Clear();
                    this.pnldetalle.Controls.Add(UsGpo);
                    UsGpo.comando(1, 0);
                    break;
                case 1:
                    iActionToSave = 0;
                    UsBasc.Dock = DockStyle.Fill;
                    this.pnldetalle.Controls.Clear();
                    this.pnldetalle.Controls.Add(UsBasc);
                    UsBasc.comando(1, 0);
                    break;
            }

            btnEditar.Enabled = false;
            btnBorrar.Enabled = false;
            btnGuardar.Enabled = true;
        }
        private void Cerrar_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        } 
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (treListadoB.SelectedNode.Tag.ToString() == "B")
            {
                iActionToSave = 1;
                UsBasc.idbasc = treListadoB.SelectedNode.Name;
                UsBasc.Dock = DockStyle.Fill;
                this.pnldetalle.Controls.Clear();
                this.pnldetalle.Controls.Add(UsBasc);
                UsBasc.comando(2, 0);
            }
            if (treListadoB.SelectedNode.Tag.ToString() == "G")
            {
                iActionToSave = 1;
                UsGpo.idgrup = treListadoB.SelectedNode.Name;
                UsGpo.Dock = DockStyle.Fill;
                this.pnldetalle.Controls.Clear();
                this.pnldetalle.Controls.Add(UsGpo);
                UsGpo.comando(2, 0);
            }

            btnEditar.Enabled = false;
            btnBorrar.Enabled = false;
            btnGuardar.Enabled = true;
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (this.pnldetalle.Controls["UserGrupos"] != null)
            {
                if (UsGpo.comando(3, iActionToSave) == true)
                {
                    Asigna_Grupo();
                    Crear_Nodos();
                }
            }
            else
            {
                if (this.pnldetalle.Controls["UserBasculas"] != null)
                {
                    
                    if (UsBasc.comando(3, iActionToSave) == true)
                    {
                        Asigna_Bascula();
                        Crear_Nodos();
                    }
                }
                else MessageBox.Show(this, Variable.SYS_MSJ[24, Variable.idioma], Variable.SYS_MSJ[24, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning); //"No hay bascula o grupo seleccionado"
            }  
        }
        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (treListadoB.SelectedNode.Tag.ToString() == "B")
            {

                UsBasc.idbasc = treListadoB.SelectedNode.Name;
                UsBasc.Dock = DockStyle.Fill;
                this.pnldetalle.Controls.Clear();
                //this.pnldetalle.Controls.Add(UsBasc);
                UsBasc.comando(4, 0);
                Asigna_Bascula();
            }
            if (treListadoB.SelectedNode.Tag.ToString() == "G")
            {
                UsGpo.idgrup = treListadoB.SelectedNode.Name;
                UsGpo.Dock = DockStyle.Fill;
                this.pnldetalle.Controls.Clear();
                //this.pnldetalle.Controls.Add(UsGpo);
                UsGpo.comando(4, 0);
                Asigna_Grupo();
            }
            Crear_Nodos();
        }                                              
    }
}