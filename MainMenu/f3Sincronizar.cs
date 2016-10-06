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
    public partial class f3Sincronizar : MdiChildForm
    {      
        private bool exist_nodo = false;      
        private ContextMenu MenuBotton1 = new ContextMenu();      
        Variable.lgrupo[] myGrupo;
        Variable.lbasc[] myScale;
        Variable.nodo_actual myCurrent;
        Serial SR = new Serial();
        ADOutil Conec = new ADOutil();
        ArrayList Dato_Nuevo = new ArrayList();
        Conexion Sock_Bascula = new Conexion();
        bool gpoWLS_WLSD = false, gpoWLABEL = false;


        #region Inicializacion

        public f3Sincronizar()
        {
            InitializeComponent();
            
            this.TransparencyKey = Color.Empty;
            if (Variable.TypeScale == Variable.TipoBascula.WLABEL) { funcionWLABEL(); }//Funcion para configurar la aplicacionpara WLABEL
            //else if (Variable.TypeScale == Variable.TipoBascula.WLABEL && Variable.TypeScale == Variable.TipoBascula.WLSD) { funcionWLSD_WLABEL(); }
            //else if (Variable.TypeScale == Variable.TipoBascula.WLABEL && Variable.TypeScale == Variable.TipoBascula.WLS) { funcionWLS_WLABEL(); }
        }
        private void funcionWLSD_WLABEL()
        {
            
        }
        private void funcionWLS_WLABEL()
        {
            
        }
        void funcionWLABEL()
        {
            toolStrip3.Items.Remove(ribAsignar);
        }

        #endregion

        #region BotonesTap-Sincronización

        public  void QuitarBackColor     ()
        {
            this.Activate();
            this.splitContainer2.Panel2.Controls.Clear();
            ToolStripManager.RevertMerge(toolStrip3);  

            ribAsignar.Checked = false;
            ribOrganizar.Checked = false;
            ribOrdenar.Checked = false;
            ribDtosGnral.Checked = false;
            ribCodebar.Checked = false;
            ribTextos.Checked = false;
            ribBascula.Checked = false;
            ribHora.Checked = false;    
        }
        private void ribAsignar_Click    (object sender,EventArgs e)
        {
            QuitarBackColor();
            ribAsignar.Checked = true;

            if (myCurrent.Nserie != null)
            {
                UserCarpetas UsCarp = new UserCarpetas();
                UsCarp.Num_Bascula = myCurrent.idbas;
                UsCarp.Num_Grupo = myCurrent.gpo;
                UsCarp.Nombre_Select = myCurrent.nombre;
                UsCarp.Dock = DockStyle.Fill;
                if (ToolStripManager.FindToolStrip("toolStrip31").Items.Count > 0)
                    ToolStripManager.Merge(UsCarp.toolStrip31, toolStrip3);
                this.splitContainer2.Panel2.Controls.Add(UsCarp);
            }
            else MessageBox.Show(this, Variable.SYS_MSJ[24, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void ribOrganizar_Click  (object sender,EventArgs e)
        {
            QuitarBackColor();
            ribOrganizar.Checked = true;

           if (myCurrent.Nserie != null)
            {
                UserMaestros UsCata = new UserMaestros();
                UsCata.Num_Bascula = myCurrent.idbas;
                UsCata.Num_Grupo = myCurrent.gpo;
                UsCata.Nombre_Select = myCurrent.nombre;
                UsCata.Dock = DockStyle.Fill;
                if (ToolStripManager.FindToolStrip("toolStrip31").Items.Count > 0)
                    ToolStripManager.Merge(UsCata.toolStrip31, toolStrip3);
                this.splitContainer2.Panel2.Controls.Add(UsCata);
            }
           else MessageBox.Show(this, Variable.SYS_MSJ[24, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void ribOrdenar_Click    (object sender,EventArgs e)
        {
            int NumeroDeBaculas = 0;
            bool wls = false;
            bool wlsd = false;
            bool wlabel = false;


            if (myCurrent.gpo != 0)//Validacion por grupo
            {
                for (int pos = 0; pos < myScale.Length; pos++)  //Num de Basculas en el grupo
                    if (myScale[pos].gpo == myCurrent.gpo) NumeroDeBaculas++;

                for (int pos = 0; pos < myScale.Length; pos++)//Recorrer todas las basculas
                {
                    if (myScale[pos].gpo == myCurrent.gpo)//Revisar solo las basculas de ese grupo
                    {
                        if (myScale[pos].modelo == "WLS") { wls = true; }
                        if (myScale[pos].modelo == "WLSD") { wlsd = true; }
                        if (myScale[pos].modelo == "W-LABEL") { wlabel = true; }
                    }
                }
           

            if (wlabel == true && wls==false && wlsd==false){ShowOrderWLABEL();}
            else if (wlabel == true && wls == false && wlsd == true){ShowOrderWLSorWLSD();}
            else if (wlabel == false && wls==true && wlsd==true){ShowOrderWLSorWLSD();}
            else if (wlabel == true && wls == true && wlsd == false){ShowOrderWLSorWLSD();}
            else if (wlabel == false && wls == true && wlsd == false){ShowOrderWLSorWLSD();}
            else if (wlabel == false && wls == false && wlsd == true){ShowOrderWLSorWLSD();}
            else if (wlabel == true && wls == true && wlsd == true) {ShowOrderWLSorWLSD();}
            else if(wlabel==false &&wls==false && wlsd == false){MessageBox.Show(this, "EL GRUPO SELECCIONADO NO TIENE BASCULAS REGISTRADAS", Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);}
            }
            else
            { 
            /*Validaciones para una sola bascula*/
            if (myCurrent.Modelo=="W-LABEL"){ShowOrderWLABEL();}
            else if (myCurrent.Modelo == "WLS"){ShowOrderWLSorWLSD();}
            else if (myCurrent.Modelo == "WLSD"){ShowOrderWLSorWLSD();}
            }
        }
/*********************************************************************************************************************************************************************/

        private void ribDtosGrnal_Click  (object sender,EventArgs e)
        {
            QuitarBackColor();
            ribDtosGnral.Checked = true;

          if (myCurrent.Nserie != null)
            {
                UserGeneral UsGral = new UserGeneral();
                UsGral.Num_Bascula = myCurrent.idbas;
                UsGral.Num_Grupo = myCurrent.gpo;
                UsGral.Nombre_Select = myCurrent.nombre;
                UsGral.Dock = DockStyle.Fill;
                if (ToolStripManager.FindToolStrip("toolStrip31").Items.Count > 0)
                    ToolStripManager.Merge(UsGral.toolStrip31, toolStrip3);
                this.splitContainer2.Panel2.Controls.Add(UsGral);
            }
          else MessageBox.Show(this, Variable.SYS_MSJ[24, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void ribCodeBar_Click    (object sender,EventArgs e)
        {
            QuitarBackColor();
            ribCodebar.Checked = true;

          if (myCurrent.Nserie != null)
            {
                UserFormatos UsForm = new UserFormatos();
                UsForm.Num_Bascula = myCurrent.idbas;
                UsForm.Num_Grupo = myCurrent.gpo;
                UsForm.Nombre_Select = myCurrent.nombre;
                UsForm.Dock = DockStyle.Fill;
                if (ToolStripManager.FindToolStrip("toolStrip31").Items.Count > 0)
                    ToolStripManager.Merge(UsForm.toolStrip31, toolStrip3);
                this.splitContainer2.Panel2.Controls.Add(UsForm);
            }
          else MessageBox.Show(this, Variable.SYS_MSJ[24, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void ribTexto_Click      (object sender,EventArgs e)
        {
            QuitarBackColor();
            ribTextos.Checked = true;

         if (myCurrent.Nserie != null)
            {
                UserTextos UsText = new UserTextos();
                UsText.Num_Bascula = myCurrent.idbas;
                UsText.Num_Grupo = myCurrent.gpo;
                UsText.Nombre_Select = myCurrent.nombre;
                UsText.Dock = DockStyle.Fill;
                if (ToolStripManager.FindToolStrip("toolStrip31").Items.Count > 0) 
                    ToolStripManager.Merge(UsText.toolStrip31, toolStrip3);
                this.splitContainer2.Panel2.Controls.Add(UsText);
            }
         else MessageBox.Show(this, Variable.SYS_MSJ[24, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void ribBascula_Click    (object sender,EventArgs e)
        {
            QuitarBackColor();
            ribBascula.Checked = true;

          if (myCurrent.Nserie != null)
            {
                UserLecturas UsLector = new UserLecturas();
                UsLector.Num_Bascula = myCurrent.idbas;
                UsLector.Num_Grupo = myCurrent.gpo;
                UsLector.Nombre_Select = myCurrent.nombre;
                UsLector.Dock = DockStyle.Fill;
                if (ToolStripManager.FindToolStrip("toolStrip31").Items.Count > 0)
                    ToolStripManager.Merge(UsLector.toolStrip31, toolStrip3);
                this.splitContainer2.Panel2.Controls.Add(UsLector);
            }
          else MessageBox.Show(this, Variable.SYS_MSJ[24, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        #endregion


        private void showOptionsbetweenWLABELandWLS()
        {
            //shows options for the new 
            SelectorOrder item = new SelectorOrder(this);
            item.ShowDialog();

        }
        public void ShowOrderWLABEL()
        {
            //Mostrar Recuadro para asignar productos al teclado con drag and drop.
            QuitarBackColor();
            ribOrdenar.Checked = true;

            if (myCurrent.Nserie != null)
            {
                UserKeys UsCata = new UserKeys();
                UsCata.Num_Bascula = myCurrent.idbas;
                UsCata.Num_Grupo = myCurrent.gpo;
                UsCata.Nombre_Select = myCurrent.nombre;
                UsCata.Dock = DockStyle.Fill;
                if (ToolStripManager.FindToolStrip("toolStrip31").Items.Count > 0)
                    ToolStripManager.Merge(UsCata.toolStrip31, toolStrip3);
                this.splitContainer2.Panel2.Controls.Add(UsCata);
            }
            else MessageBox.Show(this, Variable.SYS_MSJ[24, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
            
        }
        public void ShowOrderWLSorWLSD()
        {
            QuitarBackColor();
            ribOrdenar.Checked = true;

            if (myCurrent.Nserie != null)
            {
                UserOrdenar UsCata = new UserOrdenar();
                UsCata.Num_Bascula = myCurrent.idbas;
                UsCata.Num_Grupo = myCurrent.gpo;
                UsCata.Nombre_Select = myCurrent.nombre;
                UsCata.Dock = DockStyle.Fill;

                if (ToolStripManager.FindToolStrip("toolStrip31").Items.Count > 0)
                    ToolStripManager.Merge(UsCata.toolStrip31, toolStrip3);
                this.splitContainer2.Panel2.Controls.Add(UsCata);
            }
            else MessageBox.Show(this, Variable.SYS_MSJ[24, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
            
        }



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
                nitem++;
            }

        }
                


        private void Crear_Nodos_Basculas()
        {           
            tvwBascula.Nodes.Clear();
            tvwBascula.Refresh();
            myCurrent.ip = null;
            myCurrent.Nserie = null;

            for (int i = 0; i < this.myGrupo.Length; i++)
            {
                if (myGrupo[i].nombre != "" && myGrupo[i].nombre != null)
                {
                    TreeNodeBound trG = new TreeNodeBound(myGrupo[i].nombre);
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


                    if (myScale[j].modelo == "WLABEL")
                    {
                        trB.SelectedImageIndex = 1;
                        trB.ImageIndex = 1;
                    }
                    else if (myScale[j].modelo == "WLS")
                    {
                        trB.SelectedImageIndex = 2;
                        trB.ImageIndex = 2;
                    }
                    else if (myScale[j].modelo == "WLSD")
                    {
                        trB.SelectedImageIndex = 3;
                        trB.ImageIndex = 3;
                    }

                    this.tvwBascula.Nodes.Add(trB);
                }
            }
        }    
        #endregion

        #region ListView y treeView      

        private void tvwBascula_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView nodo = ((TreeView)sender);
            nodo.Select();
            nodo.Focus();            
            Consulta_EnBD(nodo.SelectedNode.Name, nodo.SelectedNode.Tag.ToString());       
            //validar si se muestran las carpetas o no
            int NumeroDeBaculas = 0;
            bool wls = false;
            bool wlsd = false;
            bool wlabel = false;


            if (myCurrent.gpo != 0)//Validacion por grupo
            {
                for (int pos = 0; pos < myScale.Length; pos++)  //Num de Basculas en el grupo
                    if (myScale[pos].gpo == myCurrent.gpo) NumeroDeBaculas++;

                for (int pos = 0; pos < myScale.Length; pos++)//Recorrer todas las basculas
                {
                    if (myScale[pos].gpo == myCurrent.gpo)//Revisar solo las basculas de ese grupo
                    {
                        if (myScale[pos].modelo == "WLS") { wls = true; }
                        if (myScale[pos].modelo == "WLSD") { wlsd = true; }
                        if (myScale[pos].modelo == "W-LABEL") { wlabel = true; }
                    }
                }

                if (wlabel == true && wls == false && wlsd == false) { ribAsignar.Visible = false; }
                else if (wlabel == true && wls == false && wlsd == true) { ribAsignar.Visible = true; }
                else if (wlabel == false && wls == true && wlsd == true) { ribAsignar.Visible = true; }
                else if (wlabel == true && wls == true && wlsd == false) { ribAsignar.Visible = true; }
                else if (wlabel == false && wls == true && wlsd == false) { ribAsignar.Visible = true; }
                else if (wlabel == false && wls == false && wlsd == true) { ribAsignar.Visible = true; }
                else if (wlabel == true && wls == true && wlsd == true) { ribAsignar.Visible = true; }
                else if (wlabel == false && wls == false && wlsd == false) { MessageBox.Show(this, "EL GRUPO SELECCIONADO NO TIENE BASCULAS REGISTRADAS", Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
            else
            {
                /*Validaciones para una sola bascula*/
                if (myCurrent.Modelo == "W-LABEL") { ribAsignar.Visible = false; }
                else if (myCurrent.Modelo == "WLS") { ribAsignar.Visible = true; }
                else if (myCurrent.Modelo == "WLSD") { ribAsignar.Visible = true; }
            }


        }


        private void Consulta_EnBD(string ncod, string tipo)
        {
            QuitarBackColor();
            splitContainer2.Panel2.Controls.Clear();
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
                    myCurrent.Modelo = dr["modelo"].ToString();
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
                    myCurrent.nombre = dr["nombre_gpo"].ToString();  // +" " + dr["descripcion"].ToString();
                    myCurrent.Nserie = dr["nombre_gpo"].ToString();
                }

            }

        }     
        #endregion
        
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
       
        private void f3Sincronizar_Load(object sender, EventArgs e)
        {
            this.grupoTableAdapter.Fill(this.baseDeDatosDataSet.Grupo);
            // TODO: esta línea de código carga datos en la tabla 'baseDeDatosDataSet.Bascula' Puede moverla o quitarla según sea necesario.
            this.basculaTableAdapter.Fill(this.baseDeDatosDataSet.Bascula);
            if (this.tvwBascula.GetNodeCount(false) > 0) this.tvwBascula.SelectedNode = this.tvwBascula.TopNode;
        }
         
        private void f3Sincronizar_Activated(object sender, EventArgs e)
        {
            Asigna_Grupo();
            Asigna_Bascula();
            Crear_Nodos_Basculas();

            if (this.tvwBascula.GetNodeCount(false) > 0) this.tvwBascula.SelectedNode = this.tvwBascula.TopNode;

            for (int i = 0; i < Variable.privilegio.Length; i++)
            {
                if (Variable.privilegio.Substring(i, 1) == "1")
                {
                    if (i == 10)
                    {
                        this.ribAsignar.Enabled = true;
                        this.ribOrdenar.Enabled = true;
                    } //organizacion de carpetas y ordenar los elemento dentro de la carpetas
                    if (i == 11)
                    {
                        this.ribOrganizar.Enabled = true;
                        this.ribBascula.Enabled = true;
                    }  //sincronizacion de catalogos    
                    if (i == 12) { this.ribDtosGnral.Enabled = true; } //configuracion general del las basculas
                    if (i == 13) { this.ribCodebar.Enabled = true; }  //configuracion de codigo de barras y formatos
                    if (i == 14) { this.ribTextos.Enabled = true; } //configuracion de textos y encabezados
                }
                else
                {
                    if (i == 10)
                    {
                        this.ribAsignar.Enabled = false;
                        this.ribOrdenar.Enabled = false;
                    } //organizacion de carpetas
                    if (i == 11)
                    {
                        this.ribOrganizar.Enabled = false;
                        this.ribBascula.Enabled = false;
                    }  //sincronizacion de catalogos    
                    if (i == 12) { this.ribDtosGnral.Enabled = false; } //configuracion general del las basculas
                    if (i == 13) { this.ribCodebar.Enabled = false; }  //configuracion de codigo de barras y formatos
                    if (i == 14) { this.ribTextos.Enabled = false; } //configuracion de textos y encabezados
                }
            } 
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            Asigna_Grupo();
            Asigna_Bascula();

            QuitarBackColor();
            ribHora.Checked = true;

            if (myCurrent.Nserie != null)
            {
                ProgressContinue pro = new ProgressContinue();

                int Num_Bascula = myCurrent.idbas;
                int Num_Grupo = myCurrent.gpo;


                string sHora = String.Format("{0:HH:mm:ss}", DateTime.Now);
                string sFecha = String.Format("{0:dd/MM/yyyy}", DateTime.Now);

                int NumeroDeBaculas = 0;
                int BasculasActualizadas = 0;

                if (Num_Grupo > 0)
                {
                    for (int iIndex = 0; iIndex < myScale.Length; iIndex++)
                    {
                        if (myScale[iIndex].gpo == Num_Grupo)
                        {
                            NumeroDeBaculas++;
                        }
                    }
                }
                else
                {
                    for (int iIndex = 0; iIndex < myScale.Length; iIndex++)
                    {
                        if (myScale[iIndex].idbas == Num_Bascula)
                        {
                            NumeroDeBaculas++;
                        }
                    }
                }

                pro.IniciaProcess(NumeroDeBaculas, Variable.SYS_MSJ[406, Variable.idioma] + "... ");

                if (Num_Grupo > 0)
                {
                    for (int iIndex = 0; iIndex < myScale.Length; iIndex++)
                    {
                        if (myScale[iIndex].gpo == Num_Grupo)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            BasculasActualizadas++;

                            if (myScale[iIndex].tipo == 0)
                            {

                                Socket Cliente_bascula = Sock_Bascula.conectar(myScale[iIndex].ip, 50036);

                                if (Cliente_bascula != null)
                                {
                                    Sock_Bascula.enviar(ref Cliente_bascula, "SetHORA," + sHora + "\n", myScale[iIndex].ip);
                                    Thread.Sleep(50);
                                    Sock_Bascula.enviar(ref Cliente_bascula, "SetFECHA," + sFecha + "\n", myScale[iIndex].ip);
                                    Thread.Sleep(50);
                                    Cliente_bascula.Close();
                                }
                                else
                                {
                                    Cursor.Current = Cursors.Default;
                                    BasculasActualizadas--;
                                    if (MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma] + " "
                                        + myScale[iIndex].nserie + " " + Variable.SYS_MSJ[417, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma],
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No) break;
                                }
                            }
                            else
                            {
                                SerialPort serialPort1 = new SerialPort();

                                if (SR.OpenPort(ref serialPort1, myScale[iIndex].pto, 115200))
                                {
                                    string Variable_frame = "SetHORA," + sHora + "\n\r";
                                    SR.SendData(ref serialPort1, Variable_frame);

                                    Thread.Sleep(50);

                                    Variable_frame = "SetFECHA," + sFecha + "\n\r";
                                    SR.SendData(ref serialPort1, Variable_frame);

                                    SR.ClosePort(ref serialPort1);
                                }
                                else
                                {
                                    Cursor.Current = Cursors.Default;
                                    BasculasActualizadas--;
                                    MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma]
                                        + " " + myScale[iIndex].nserie + ".", Variable.SYS_MSJ[42, Variable.idioma],
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    break;
                                }
                            }

                            pro.UpdateProcess(1, Variable.SYS_MSJ[214, Variable.idioma] + " " + myScale[iIndex].nombre);
                        }
                    }
                }
                else
                {
                    for (int iIndex = 0; iIndex < myScale.Length; iIndex++)
                    {
                        if (myScale[iIndex].idbas == Num_Bascula)
                        {
                            Cursor.Current = Cursors.WaitCursor;

                            if (myScale[iIndex].tipo == 0)
                            {
                                Socket Cliente_bascula = Sock_Bascula.conectar(myScale[iIndex].ip, 50036);
                                BasculasActualizadas++;

                                if (Cliente_bascula != null)
                                {
                                    Sock_Bascula.enviar(ref Cliente_bascula, "SetHORA," + sHora + "\n", myScale[iIndex].ip);
                                    Thread.Sleep(50);
                                    Sock_Bascula.enviar(ref Cliente_bascula, "SetFECHA," + sFecha + "\n", myScale[iIndex].ip);
                                    Thread.Sleep(50);
                                    Cliente_bascula.Close();
                                }
                                else
                                {
                                    Cursor.Current = Cursors.Default;
                                    BasculasActualizadas--;
                                    MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma]
                                        + " " + myScale[iIndex].nserie + ".", Variable.SYS_MSJ[42, Variable.idioma],
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    break;
                                }
                            }
                            else
                            {
                                SerialPort serialPort1 = new SerialPort();

                                if (SR.OpenPort(ref serialPort1, myScale[iIndex].pto, 115200))
                                {
                                    string Variable_frame = "SetHORA," + sHora + "\n\r";
                                    SR.SendData(ref serialPort1, Variable_frame);

                                    Thread.Sleep(50);

                                    Variable_frame = "SetFECHA," + sFecha + "\n\r";
                                    SR.SendData(ref serialPort1, Variable_frame);

                                    SR.ClosePort(ref serialPort1);
                                }
                                else
                                {
                                    Cursor.Current = Cursors.Default;
                                    BasculasActualizadas--;
                                    MessageBox.Show(this, Variable.SYS_MSJ[416, Variable.idioma] + ", " + Variable.SYS_MSJ[214, Variable.idioma]
                                        + " " + myScale[iIndex].nserie + ".", Variable.SYS_MSJ[42, Variable.idioma],
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    break;
                                }
                            }


                            pro.UpdateProcess(1, Variable.SYS_MSJ[214, Variable.idioma] + " " + myScale[iIndex].nombre);
                        }
                    }
                }

                Cursor.Current = Cursors.Default;
                pro.TerminaProcess();
                Thread.Sleep(500);

                MessageBox.Show(this, Variable.SYS_MSJ[418, Variable.idioma] + " " + BasculasActualizadas + " "
                    + Variable.SYS_MSJ[419, Variable.idioma] + " " + NumeroDeBaculas,
                    Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else MessageBox.Show(this, Variable.SYS_MSJ[24, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
       

    }     
}



