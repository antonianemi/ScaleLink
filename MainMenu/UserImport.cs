using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MainMenu
{
    public partial class UserImport : UserControl
    {
        #region Declaracion Class
       // Form1 fMenu = new Form1();
        ADOutil Conec = new ADOutil();
        FileStream MyFile;
        #endregion

        public UserImport()
        {
            InitializeComponent();           
        }
         #region Procesos pushBotones
        private void treeListado_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView nodo = ((TreeView)sender);
            nodo.Select();
            nodo.Focus();
            switch ((int)Form1.btnEdicion)
            {
                case (int)ESTADO.botonesEdicionEnum.PKIMPORT:                    
                    gbxPath.Enabled = true;                  
                    break;
                case (int)ESTADO.botonesEdicionEnum.PKEXPORT:
                    gbxPath.Enabled = true;
                    break;
                case (int)ESTADO.botonesEdicionEnum.PKPURGAR:
                    gbxPath.Enabled = false;
                    break;
            }
            tbxFile.Text = "";
        }
                

        public void presionoBotonBarra(int poBB)
        {
            Form1.btnEdicion = (ESTADO.botonesEdicionEnum)poBB;
            switch(poBB)
            {
                case (int)ESTADO.botonesEdicionEnum.PKCOMPACTAR:
                    //Campacta_DB();
                    break;
                case (int)ESTADO.botonesEdicionEnum.PKBACKUP:
                    //Respaldar_DB();
                    break;
                default:
                    listadoId_EnBD();
                    break;
            }
        }
        #endregion

        
        private void  Consulta_EnBD(int opc)
        {
            if (MyFile != null)
            {
                switch ((int)Form1.btnEdicion)
                {
                    case (int)ESTADO.botonesEdicionEnum.PKIMPORT:
                        ImpExp IMP = new ImpExp();
                        IMP.importar(opc, ref MyFile, tbxFile.Text);
                        break;
                    case (int)ESTADO.botonesEdicionEnum.PKEXPORT:
                        ImpExp EXP = new ImpExp();
                        EXP.exportar(opc, ref MyFile);
                        break;
                    
                    default:
                        listadoId_EnBD();
                        break;
                }
            }
            else
            {
                if ((int)Form1.btnEdicion == (int)ESTADO.botonesEdicionEnum.PKPURGAR)
                {
                    ImpExp DEP = new ImpExp();
                    //DEP.depurar(opc, ref MyFile);
                }
                else
                {
                    MessageBox.Show(Variable.SYS_MSJ[29, Variable.idioma]);
                    this.tbxFile.Focus();
                }
            }
        }
        
        private void listadoId_EnBD()
        {
            try
            {
                treListadoH.Nodes.Clear();
                switch ((int)Form1.btnEdicion)
                {
                    case (int)ESTADO.botonesEdicionEnum.PKIMPORT:
                        treListadoH.Nodes.Add("1", Variable.SYS_MSJ[167, Variable.idioma]);  //"Productos");
                        treListadoH.Nodes.Add("2", Variable.SYS_MSJ[149, Variable.idioma]);  //"Info. Adicional");
                        treListadoH.Nodes.Add("3", Variable.SYS_MSJ[168, Variable.idioma]);  //"Publicidad");
                        treListadoH.Nodes.Add("4", Variable.SYS_MSJ[165, Variable.idioma]);  // "Ofertas");
                        treListadoH.Nodes.Add("5", Variable.SYS_MSJ[175, Variable.idioma]);  //"Vendedores");
                        break;
                    case (int)ESTADO.botonesEdicionEnum.PKEXPORT:
                        treListadoH.Nodes.Add("1", Variable.SYS_MSJ[123, Variable.idioma]);  //"Catalogo de Productos");
                        treListadoH.Nodes.Add("2", Variable.SYS_MSJ[120, Variable.idioma]);  //"Catalogo de Info Adicional");
                        treListadoH.Nodes.Add("3", Variable.SYS_MSJ[121, Variable.idioma]);  //"Catalogo de Publicidad");
                        treListadoH.Nodes.Add("4", Variable.SYS_MSJ[122, Variable.idioma]);  //"Catalogo de Ofertas");
                        treListadoH.Nodes.Add("5", Variable.SYS_MSJ[124, Variable.idioma]);  //"Catalogo de Vendedores");
                        treListadoH.Nodes.Add("6", Variable.SYS_MSJ[125, Variable.idioma]);  //"Registro de Ventas");
                        break;                   
                }
                this.tbxFile.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Excepcion " + ex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // OpenFileDialog dlgOpenFile = new OpenFileDialog();
            try
            {
                dlgOpenFile.ShowReadOnly = true;
                dlgOpenFile.InitialDirectory = Variable.appPath;

                dlgOpenFile.DefaultExt = "*.txt";
                dlgOpenFile.Filter = "Data files (*.txt)|*.txt|All files(*.*)|*.*";

                if (dlgOpenFile.ShowDialog() == DialogResult.OK)
                {
                    tbxFile.Text = dlgOpenFile.FileName;

                    if (tbxFile.Text != "")
                    {
                        if (dlgOpenFile.ReadOnlyChecked == true)
                        {
                            MyFile = (FileStream)dlgOpenFile.OpenFile();
                        }
                        else
                        {
                            MyFile = new FileStream(tbxFile.Text, FileMode.Open, FileAccess.Read);
                        }
                        if (this.treListadoH.SelectedNode != null)
                        {
                            Consulta_EnBD(Convert.ToInt32(this.treListadoH.SelectedNode.Name));
                        }
                        else
                        {
                            MessageBox.Show(Variable.SYS_MSJ[29, Variable.idioma]);  //"No hay opcion seleccionada");
                        }
                    }
                    else MessageBox.Show(Variable.SYS_MSJ[29, Variable.idioma]);  //"No hay archivo seleccionado");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UserImport_Load(object sender, EventArgs e)
        {
            Form1.btnEdicion = ESTADO.botonesEdicionEnum.PKIMPORT;
            listadoId_EnBD();
        }
        
    }
}
