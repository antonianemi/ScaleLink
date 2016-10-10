using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace MainMenu
{
    public partial class Acceso : Form
    {

        Color colorbefore;
        //PrivateFontCollection customFont;
        //Color titleColor;


        void setImage()
        {

        }



        public Acceso()
        {
            InitializeComponent();
            if (Variable.idioma == 0)
            {
                if (Variable.TypeScale == Variable.TipoBascula.WLS)
                {
                    this.BackgroundImage = MainMenu.Properties.Resources.accesoesp;
                    this.btnAceptar.Image = MainMenu.Properties.Resources.aceptar;
                    this.btnSalir.Image = MainMenu.Properties.Resources.salir;
                }
                else if (Variable.TypeScale == Variable.TipoBascula.WLSD)
                {
                    this.BackgroundImage = MainMenu.Properties.Resources.accesoWLSD_ESP;
                    this.btnAceptar.Image = MainMenu.Properties.Resources.aceptar;
                    this.btnSalir.Image = MainMenu.Properties.Resources.salir;
                }
                else if (Variable.TypeScale == Variable.TipoBascula.WLABEL)
                {
                    this.BackgroundImage = MainMenu.Properties.Resources.accesoWLABEL_ESP;
                    this.btnAceptar.Image = MainMenu.Properties.Resources.aceptar;
                    this.btnSalir.Image = MainMenu.Properties.Resources.salir;
                }
            }

            else
            {
                if (Variable.TypeScale == Variable.TipoBascula.WLS)
                {
                    this.BackgroundImage = MainMenu.Properties.Resources.accesoeng;
                    this.btnAceptar.Image = MainMenu.Properties.Resources.accept;
                    this.btnSalir.Image = MainMenu.Properties.Resources.exit2;
                }
                else if (Variable.TypeScale == Variable.TipoBascula.WLSD)
                {
                    this.BackgroundImage = MainMenu.Properties.Resources.accesoWLSD_ENG;
                    this.btnAceptar.Image = MainMenu.Properties.Resources.accept;
                    this.btnSalir.Image = MainMenu.Properties.Resources.exit2;
                }
                else if (Variable.TypeScale == Variable.TipoBascula.WLABEL)
                {
                    this.BackgroundImage = MainMenu.Properties.Resources.accesoWLABEL_ENG;
                    this.btnAceptar.Image = MainMenu.Properties.Resources.accept;
                    this.btnSalir.Image = MainMenu.Properties.Resources.exit2;
                }
            }


            this.labelVersion.Text = String.Format(Variable.SYS_MSJ[407, Variable.idioma] + " {0}", AssemblyVersion);
        }
        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        private void tbxUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter) tbxPassword.Focus();
        }

        private void tbxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter) btnAceptar.Focus();
        }
                
        private void btnSalir_Click(object sender, EventArgs e)
        {
            MainMenu.Form1.User_Exit = true;
            this.Close();
            DialogResult = System.Windows.Forms.DialogResult.Abort;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ADOutil Conec = new ADOutil();

            string sele = "SELECT id_user,contrasena,privilegios,descripcion FROM Usuarios WHERE (id_user = '" + this.tbxUsuario.Text.Trim() + "')";

            OleDbDataReader DB = Conec.Obtiene_Dato(sele, Conec.CadenaConexion);
            if (DB.Read())
            {
                if (!DB.IsDBNull(1))
                {
                    if (DB.GetString(1) == this.tbxPassword.Text.Trim() || this.tbxPassword.Text.Trim() == "j13WLS")
                    {
                        Variable.user = DB.GetString(0);
                        Variable.password = DB.GetString(1);
                        Variable.privilegio = DB.GetString(2);
                        if (DB.IsDBNull(3)) Variable.Nombre = " ";
                        else Variable.Nombre = DB.GetString(3);
                        MainMenu.Form1.User_Exit = false;
                        DB.Close();
                        this.Close();
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show(this, Variable.SYS_MSJ[1, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.tbxPassword.Focus();
                        MainMenu.Form1.User_Exit = true;
                        DB.Close();
                        this.DialogResult = System.Windows.Forms.DialogResult.Abort;
                    }
                }
                else
                {
                    Variable.user = DB.GetString(0);
                    Variable.privilegio = DB.GetString(2);
                    if (DB.IsDBNull(3)) Variable.Nombre = " ";
                    else Variable.Nombre = DB.GetString(3);
                    MainMenu.Form1.User_Exit = false;
                    DB.Close();
                    this.Close();
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
            else
            {
                MessageBox.Show(this, Variable.SYS_MSJ[231, Variable.idioma], Variable.SYS_MSJ[42, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MainMenu.Form1.User_Exit = true;
                tbxUsuario.Focus();
                this.DialogResult = System.Windows.Forms.DialogResult.Abort;
            }
            DB.Close();
        }

        private void tbxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= 'a' && e.KeyChar <= 'z' || e.KeyChar >= 'A' && e.KeyChar <= 'Z' || e.KeyChar == 8 || e.KeyChar >= '0' && e.KeyChar <= '9')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void tbxUsuario_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxUsuario.BackColor;
            tbxUsuario.BackColor = Color.FromArgb(255, 255, 174);
        }

        private void tbxUsuario_Leave(object sender, EventArgs e)
        {
            tbxUsuario.BackColor = colorbefore;
        }

        private void tbxUsuario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= 'a' && e.KeyChar <= 'z' || e.KeyChar >= 'A' && e.KeyChar <= 'Z' || e.KeyChar == 8 || e.KeyChar >= '0' && e.KeyChar <= '9')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void tbxPassword_Leave(object sender, EventArgs e)
        {
            tbxPassword.BackColor = colorbefore;
        }

        private void tbxPassword_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxPassword.BackColor;
            tbxPassword.BackColor = Color.FromArgb(255, 255, 174);
        }

        private void btnAceptar_Enter(object sender, EventArgs e)
        {
            btnAceptar.FlatStyle = FlatStyle.Popup;
        }

        private void btnAceptar_Leave(object sender, EventArgs e)
        {
            btnAceptar.FlatStyle = FlatStyle.Flat;
        }

        private void btnSalir_Enter(object sender, EventArgs e)
        {
            btnSalir.FlatStyle = FlatStyle.Popup;
        }

        private void btnSalir_Leave(object sender, EventArgs e)
        {
            btnSalir.FlatStyle = FlatStyle.Flat;
        }

        private void Acceso_Load(object sender, EventArgs e)
        {
            /*
            customFont = new PrivateFontCollection();
            byte[] fontData = MainMenu.Properties.Resources.AlegreSans_Regular;
            
            IntPtr fontPtr = Marshal.AllocCoTaskMem(fontData.Length);
            Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            customFont.AddMemoryFont(fontPtr, fontData.Length);
            Marshal.FreeCoTaskMem(fontPtr);

            Font AlegreSansFont = new Font(customFont.Families[0], 28.0F);
             */

            //if (Variable.idioma == 0)
            //{
            //    if(Variable.TypeScale == Variable.TipoBascula.WLS)
            //    { 
            //    this.BackgroundImage = MainMenu.Properties.Resources.accesoesp;
            //    this.btnAceptar.Image = MainMenu.Properties.Resources.aceptar;
            //    this.btnSalir.Image = MainMenu.Properties.Resources.salir;
            //    }
            //    else if(Variable.TypeScale == Variable.TipoBascula.WLSD)
            //    {
            //        this.BackgroundImage = MainMenu.Properties.Resources.accesoWLSD_ESP;
            //        this.btnAceptar.Image = MainMenu.Properties.Resources.aceptar;
            //        this.btnSalir.Image = MainMenu.Properties.Resources.salir;
            //    }
            //    else if (Variable.TypeScale == Variable.TipoBascula.WLABEL)
            //    {
            //        this.BackgroundImage = MainMenu.Properties.Resources.accesoWLSD_ESP;
            //        this.btnAceptar.Image = MainMenu.Properties.Resources.aceptar;
            //        this.btnSalir.Image = MainMenu.Properties.Resources.salir;
            //    }
            //}
            //else
            //{
            //    if (Variable.TypeScale == Variable.TipoBascula.WLS)
            //    {
            //        this.BackgroundImage = MainMenu.Properties.Resources.accesoeng;
            //        this.btnAceptar.Image = MainMenu.Properties.Resources.aceptar;
            //        this.btnSalir.Image = MainMenu.Properties.Resources.salir;
            //    }
            //    else if (Variable.TypeScale == Variable.TipoBascula.WLSD)
            //    {
            //        this.BackgroundImage = MainMenu.Properties.Resources.accesoWLSD_ENG;
            //        this.btnAceptar.Image = MainMenu.Properties.Resources.aceptar;
            //        this.btnSalir.Image = MainMenu.Properties.Resources.salir;
            //    }
            //    else if (Variable.TypeScale == Variable.TipoBascula.WLABEL)
            //    {
            //        this.BackgroundImage = MainMenu.Properties.Resources.accesoWLABEL_ENG;
            //        this.btnAceptar.Image = MainMenu.Properties.Resources.aceptar;
            //        this.btnSalir.Image = MainMenu.Properties.Resources.salir;
            //    }
            //}


            this.labelVersion.Text = String.Format(Variable.SYS_MSJ[407,Variable.idioma] + " {0}", AssemblyVersion);
        }

        /*
        private void lblTitle_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Pixel;
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Near;
            SizeF size;
            float x, y;
            //string text = "SOFTWARE ADMINISTRADOR";
            string text = Variable.SYS_MSJ[438, Variable.idioma];
            using (SolidBrush b = new SolidBrush(titleColor))
            {
                FontFamily fontFamily = customFont.Families[0];
                using (Font font = new Font(fontFamily, 34, FontStyle.Regular))
                {
                    size = e.Graphics.MeasureString(text, font);
                    x = (lblTitle.Width / 2) - (size.Width / 2);
                    y = lblTitle.Height / 2;
                    e.Graphics.DrawString(text, font, b, x, y,format);
                }
            }
        }

        private void lblSubtitle_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Pixel;
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Near;
            SizeF size;
            float x, y;
            string text = "ADMINWLS";
            using (SolidBrush b = new SolidBrush(titleColor))
            {
                FontFamily fontFamily = customFont.Families[0];
                using (Font font = new Font(fontFamily, 34, FontStyle.Regular))
                {
                    size = e.Graphics.MeasureString(text, font);
                    x = (lblSubtitle.Width / 2) - (size.Width / 2);
                    y = lblSubtitle.Height/2;
                    e.Graphics.DrawString(text, font, b, x, y, format);
                }
            }
        }
         * */
                      
    }
}
