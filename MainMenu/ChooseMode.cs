using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MainMenu
{
    public partial class ChooseMode : Form
    {
        bool huboMensaje;
        ADOutil Conec = new ADOutil();
        Form1 form1;
        public ChooseMode(Form1 _form1)
        {
            form1 = _form1;
            InitializeComponent();
            LoadScalesFromDB();
            LoadTypeScales();
            PritlnScalesSelected();
        }
        private void LoadScalesFromDB()
        {
            Variable.scales.Clear();
            Conec.CadenaSelect = "SELECT * FROM Bascula";
            BaseDeDatosDataSetTableAdapters.BasculaTableAdapter basculaTableAdapter = new BaseDeDatosDataSetTableAdapters.BasculaTableAdapter();
            BaseDeDatosDataSet baseDeDatosDataSet1 = new BaseDeDatosDataSet();
            basculaTableAdapter.Connection.ConnectionString = Conec.CadenaConexion;
            basculaTableAdapter.Connection.CreateCommand().CommandText = Conec.CadenaSelect;
            basculaTableAdapter.Fill(baseDeDatosDataSet1.Bascula);

            foreach (DataRow dr in baseDeDatosDataSet1.Bascula.Rows)
            {
                Scale s = new Scale();
                s.tipo = getScale(dr["modelo"].ToString().ToUpper());
                Variable.scales.Add(s);
            }
            

        }
        bool DBwls = false;
        bool DBwlsd = false;
        bool DBwlabel = false;
        Variable.TipoBascula getScale(string type)
        {
            Variable.TipoBascula tipo= Variable.TipoBascula.WLABEL;

            if((type.ToUpper().Equals("WLS")))
            {
                tipo = Variable.TipoBascula.WLS;
            }
            if ((type.ToUpper().Equals("WLSD")))
            {
                tipo = Variable.TipoBascula.WLSD;
            }
            if ((type.ToUpper().Equals("W-LABEL")))
            {
                tipo = Variable.TipoBascula.WLABEL;
            }

            return tipo;
        }


        void LoadTypeScales()
        {
            //BUSCAR LOS TIPOS DE BASCULA QUE TIENE EL SISTEMA********************************************
            foreach (Scale item in Variable.scales)
            {
                if (item.tipo == Variable.TipoBascula.WLSD)
                {
                    DBwlsd = true;
                }
            }

            foreach (Scale item in Variable.scales)
            {
                if (item.tipo == Variable.TipoBascula.WLS)
                {
                    DBwls = true;
                }
            }
            foreach (Scale item in Variable.scales)
            {
                if (item.tipo == Variable.TipoBascula.WLABEL)
                {
                    DBwlabel = true;
                }
            }
            ///********************************************************************************************
        }
        void PritlnScalesSelected()
        {
            CHK_WLABEL.Checked = (DBwlabel) ? true : false;
            CHK_WLS.Checked = (DBwls) ? true : false;
            CHK_WLSD.Checked = (DBwlsd) ? true : false;
        }
        private void BTN_ENTRAR_Click(object sender, EventArgs e)
        {

            DBwls = false;
            DBwlsd = false;
            DBwlabel = false;

            huboMensaje = false;//reiniciar el mensaje por caa evento de entrar para repetir correctamente la evaluzacion.

            //asignacion de banderas
            if (CHK_WLABEL.Checked) {Variable.wlabel = true;}else{Variable.wlabel = false;}
            if (CHK_WLS.Checked)    {Variable.wls    = true;}else{Variable.wls    = false;}
            if (CHK_WLSD.Checked)   {Variable.wlsd   = true;}else{Variable.wlsd   = false;}


            if(!Variable.wlabel && !Variable.wls && !Variable.wlsd)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[453, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }
              
               
            //caso de validacion para cuando la base de datos esta vacia
            if (Variable.scales.Count <= 0){validacionesBaseDatosEmpty();}
            
            //caso de validacion para cuando la base de datos ya tiene basculas registradas
            else if (Variable.scales.Count > 0)
            {

                LoadTypeScales();

                /***********************************************************************************************/
                //validaciones en base a lo que hay en la base de datos
                if (DBwlsd == true  && DBwls == false && DBwlabel == false){ validaciones_WLSD_DB();           }
                if (DBwlsd == false && DBwls == true  && DBwlabel == false){ validaciones_WLS_DB();            }
                if (DBwlsd == false && DBwls == false && DBwlabel == true) { validaciones_WLABEL_DB();         }
                if (DBwlsd == false && DBwls == true  && DBwlabel == true ){ validaciones_WLABEL_WLS_DB();     }
                if (DBwlsd == true  && DBwls == true  && DBwlabel == false){ validaciones_WLS_WLSD_DB();       }
                if (DBwlsd == true  && DBwls == false && DBwlabel == true ){ validaciones_WLABEL_WLSD_DB();    }
                if (DBwlsd == true  && DBwls == true  && DBwlabel == true ){ validaciones_WLSD_WLS_WLABEL_DB();}
                /***********************************************************************************************/
               
            }

            ///se determina si no hubo mensaje de error quiere decir que el aplicativo esta preparado para funcionar 
            if (!huboMensaje)
            {
                this.Hide();
                this.Close();
            }

            this.form1.RefrescaName();

            Debug.Print("Status : ");
            Debug.Print("Tipo de Software : "+ Variable.TypeScale);
            Debug.Print("W-LABEL: " + Variable.wlabel.ToString());
            Debug.Print("WLS    : " + Variable.wls.ToString());
            Debug.Print("WLSD   : " + Variable.wlsd.ToString()); 
        }
        
        #region VALIDACIONES

        /// <summary>
        /// Esta funcion realiza las validaciones y comparaciones cuano la base de datos esta totalmente vacia.
        /// </summary>
        void validacionesBaseDatosEmpty()
        {
            if (Variable.wlabel == true  && Variable.wls == false && Variable.wlsd == false){Variable.TypeScale = Variable.TipoBascula.WLABEL;}
            if (Variable.wlabel == true  && Variable.wls == true  && Variable.wlsd == false){Variable.TypeScale = Variable.TipoBascula.WLS;   }
            if (Variable.wlabel == true  && Variable.wls == false && Variable.wlsd == true ){Variable.TypeScale = Variable.TipoBascula.WLSD;  }
            if (Variable.wlabel == true  && Variable.wls == true  && Variable.wlsd == true ){Variable.TypeScale = Variable.TipoBascula.WLSD;  }
            if (Variable.wlabel == false && Variable.wls == true  && Variable.wlsd == false){Variable.TypeScale = Variable.TipoBascula.WLS;   }
            if (Variable.wlabel == false && Variable.wls == false && Variable.wlsd == true ){Variable.TypeScale = Variable.TipoBascula.WLSD;  }
            if (Variable.wlabel == false && Variable.wls == true  && Variable.wlsd == true ){ Variable.TypeScale = Variable.TipoBascula.WLSD; }
        }
        /// <summary>
        /// Esta funcion realiza las validaciones y comparaciones cuando existe una wlabel en la base de datos.
        /// </summary>
        void validaciones_WLABEL_DB()
        {
            if (Variable.wlabel == true  && Variable.wls == false && Variable.wlsd == false){Variable.TypeScale = Variable.TipoBascula.WLABEL;}
            if (Variable.wlabel == true  && Variable.wls == true  && Variable.wlsd == false){Variable.TypeScale = Variable.TipoBascula.WLS;   }
            if (Variable.wlabel == true  && Variable.wls == false && Variable.wlsd == true ){Variable.TypeScale = Variable.TipoBascula.WLSD;  }
            if (Variable.wlabel == true  && Variable.wls == true  && Variable.wlsd == true ){Variable.TypeScale = Variable.TipoBascula.WLSD;  }
            if (Variable.wlabel == false && Variable.wls == true  && Variable.wlsd == false){Variable.TypeScale = Variable.TipoBascula.WLS;  }
            if (Variable.wlabel == false && Variable.wls == false && Variable.wlsd == true ){Variable.TypeScale = Variable.TipoBascula.WLSD; }
            if (Variable.wlabel == false && Variable.wls == true  && Variable.wlsd == true ){Variable.TypeScale = Variable.TipoBascula.WLSD;  }  
         }
        /// <summary>
        /// Esta funcion realiza las validaciones y comparaciones cuando existe una wlsd en la base de datos.
        /// </summary>
        void validaciones_WLSD_DB()
        {
            if (Variable.wlabel == true && Variable.wls == false && Variable.wlsd == false){
                //Variable.SYS_MSJ[23, Variable.idioma]
                MessageBox.Show(this, Variable.SYS_MSJ[453, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                huboMensaje = true;
            }
            if (Variable.wlabel == true && Variable.wls == true && Variable.wlsd == false)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[454, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                huboMensaje = true;
            }
            if (Variable.wlabel == true && Variable.wls == false && Variable.wlsd == true){Variable.TypeScale = Variable.TipoBascula.WLSD;}
            if (Variable.wlabel == true && Variable.wls == true && Variable.wlsd == true) {Variable.TypeScale = Variable.TipoBascula.WLSD;}
            if (Variable.wlabel == false && Variable.wls == true && Variable.wlsd == false)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[455, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                huboMensaje = true;
            }
            if (Variable.wlabel == false && Variable.wls == false && Variable.wlsd == true) {Variable.TypeScale = Variable.TipoBascula.WLSD;}
            if (Variable.wlabel == false && Variable.wls == true && Variable.wlsd == true ) {Variable.TypeScale = Variable.TipoBascula.WLSD;}
        }
        /// <summary>
        /// Esta funcion realiza las validaciones y comparaciones cuando existe una wls en la base de datos.
        /// </summary>
        void validaciones_WLS_DB()
        {
            if (Variable.wlabel == true && Variable.wls == false && Variable.wlsd == false)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[456, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                huboMensaje = true;
            }
            if (Variable.wlabel == true  && Variable.wls == true  && Variable.wlsd == false){Variable.TypeScale = Variable.TipoBascula.WLS; }
            if (Variable.wlabel == true  && Variable.wls == false && Variable.wlsd == true ){Variable.TypeScale = Variable.TipoBascula.WLSD;}
            if (Variable.wlabel == true  && Variable.wls == true  && Variable.wlsd == true ){Variable.TypeScale = Variable.TipoBascula.WLSD;}
            if (Variable.wlabel == false && Variable.wls == true  && Variable.wlsd == false){Variable.TypeScale = Variable.TipoBascula.WLS;}
            if (Variable.wlabel == false && Variable.wls == false && Variable.wlsd == true ){Variable.TypeScale = Variable.TipoBascula.WLSD; }
            if (Variable.wlabel == false && Variable.wls == true  && Variable.wlsd == true ){Variable.TypeScale = Variable.TipoBascula.WLSD; }
        }
        /// <summary>
        /// Esta funcion realiza las validaciones y comparaciones cuando existe una wlabel en la base de datos.
        /// </summary>
        void validaciones_WLABEL_WLS_DB()
        {
            if (Variable.wlabel == true && Variable.wls == false && Variable.wlsd == false)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[457, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                huboMensaje = true;
            }
            if (Variable.wlabel == true  && Variable.wls == true  && Variable.wlsd == false){Variable.TypeScale = Variable.TipoBascula.WLS;  }
            if (Variable.wlabel == true  && Variable.wls == false && Variable.wlsd == true ){Variable.TypeScale = Variable.TipoBascula.WLSD; }
            if (Variable.wlabel == true  && Variable.wls == true  && Variable.wlsd == true ){Variable.TypeScale = Variable.TipoBascula.WLSD; }
            if (Variable.wlabel == false && Variable.wls == true  && Variable.wlsd == false){Variable.TypeScale = Variable.TipoBascula.WLS;  }
            if (Variable.wlabel == false && Variable.wls == false && Variable.wlsd == true ){Variable.TypeScale = Variable.TipoBascula.WLSD; }
            if (Variable.wlabel == false && Variable.wls == true && Variable.wlsd == true  ){Variable.TypeScale = Variable.TipoBascula.WLSD; }
        }
        /// <summary>
        /// Esta funcion realiza las validaciones y comparaciones cuando existe una wlabel en la base de datos.
        /// </summary>
        void validaciones_WLS_WLSD_DB()
        {
            
            if (Variable.wlabel == true && Variable.wls == false && Variable.wlsd == false)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[457, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                huboMensaje = true;
            }
            
            if (Variable.wlabel == true && Variable.wls == true && Variable.wlsd == false)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[458, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                huboMensaje = true;
            }
            
            if (Variable.wlabel == true  && Variable.wls == false && Variable.wlsd == true){Variable.TypeScale = Variable.TipoBascula.WLSD;}
            if (Variable.wlabel == true  && Variable.wls == true && Variable.wlsd == true ){Variable.TypeScale = Variable.TipoBascula.WLSD;}
            if (Variable.wlabel == false && Variable.wls == true && Variable.wlsd == false)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[457, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                huboMensaje = true;
            }
            if (Variable.wlabel == false && Variable.wls == false && Variable.wlsd == true){Variable.TypeScale = Variable.TipoBascula.WLSD;}
            if (Variable.wlabel == false && Variable.wls == true && Variable.wlsd  == true){Variable.TypeScale = Variable.TipoBascula.WLSD;}
            

        }
        /// <summary>
        /// Esta funcion realiza las validaciones y comparaciones cuando existe una wlabel en la base de datos.
        /// </summary>
        void validaciones_WLABEL_WLSD_DB()
        {
            if (Variable.wlabel == true && Variable.wls == false && Variable.wlsd == false)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[454, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                huboMensaje = true;
            }

            if (Variable.wlabel == true && Variable.wls == true && Variable.wlsd == false)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[455, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                huboMensaje = true;
            }

            if (Variable.wlabel == true && Variable.wls == false && Variable.wlsd == true) { Variable.TypeScale = Variable.TipoBascula.WLSD; }
            if (Variable.wlabel == true && Variable.wls == true && Variable.wlsd == true) { Variable.TypeScale = Variable.TipoBascula.WLSD; }
            if (Variable.wlabel == false && Variable.wls == true && Variable.wlsd == false)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[455, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                huboMensaje = true;
            }
            if (Variable.wlabel == false && Variable.wls == false && Variable.wlsd == true) { Variable.TypeScale = Variable.TipoBascula.WLSD; }
            if (Variable.wlabel == false && Variable.wls == true && Variable.wlsd == true) { Variable.TypeScale = Variable.TipoBascula.WLSD; }
            

        }
        /// <summary>
        /// Esta funcion realiza las validaciones y comparaciones cuando existe una wlabel en la base de datos.
        /// </summary>
        void validaciones_WLSD_WLS_WLABEL_DB()
        {
            if (Variable.wlabel == true && Variable.wls == false && Variable.wlsd == false)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[454, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                huboMensaje = true;
            }

            if (Variable.wlabel == true && Variable.wls == true && Variable.wlsd == false)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[455, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                huboMensaje = true;
            }

            if (Variable.wlabel == true && Variable.wls == false && Variable.wlsd == true) { Variable.TypeScale = Variable.TipoBascula.WLSD; }
            if (Variable.wlabel == true && Variable.wls == true && Variable.wlsd == true) { Variable.TypeScale = Variable.TipoBascula.WLSD; }
            if (Variable.wlabel == false && Variable.wls == true && Variable.wlsd == false)
            {
                MessageBox.Show(this, Variable.SYS_MSJ[455, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                huboMensaje = true;
            }
            if (Variable.wlabel == false && Variable.wls == false && Variable.wlsd == true) { Variable.TypeScale = Variable.TipoBascula.WLSD; }
            if (Variable.wlabel == false && Variable.wls == true && Variable.wlsd == true) { Variable.TypeScale = Variable.TipoBascula.WLSD; }
            

        }
        
        #endregion
        
        private void ChooseMode_Load(object sender, EventArgs e)
        {

        }




        private void BTN_CANCELAR_Click(object sender, EventArgs e)
        {
            MainMenu.Form1.User_Exit = true;
            this.Close();
            DialogResult = System.Windows.Forms.DialogResult.Abort;
            
            if (Variable.TypeScale == Variable.TipoBascula.WLABEL)
            {

            }
            else if (Variable.TypeScale == Variable.TipoBascula.WLS)
            {

            }
            else if (Variable.TypeScale == Variable.TipoBascula.WLSD)
            {

            }
            else
            {
                Environment.Exit(0);
            }


        }
        private void CHK_WLABEL_CheckedChanged(object sender, EventArgs e)
        {

        }
    }

    public class Scale
    {
        public Variable.TipoBascula tipo;
    }
    
}
