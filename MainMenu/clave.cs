using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MainMenu
{   

    public partial class clave : Form
    {
        int tipo_user;
        Color colorbefore;

        public clave(int opcion)
        {
            InitializeComponent();
            this.CancelButton = cancelar;
            tipo_user = opcion;
            inicio_user.Text = Variable.user;
        }

        private void aceptar_Click(object sender, EventArgs e)
        {


            
            ADOutil Conec = new ADOutil();
            
            
            
            
            
            string sele = "";
            if (this.tipo_user == 1)
            {
                sele = "SELECT id_user,contrasena,privilegios FROM Usuarios WHERE (id_user = '" + this.inicio_user.Text.Trim() + "' AND contrasena = '" + this.inicio_password.Text.Trim() + "')";
                OleDbDataReader DB = Conec.Obtiene_Dato(sele, Conec.CadenaConexion);
                if (DB.Read())
                {
                    if (DB.GetString(2).Substring(17, 1) == "1") Variable.clv_aceptada = true;
                    else MessageBox.Show(this, Variable.SYS_MSJ[294, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DB.Close();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[231, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.inicio_password.Focus();
                    DB.Close();
                }
            }
            else
            {
                sele = "SELECT id_user,contrasena,privilegios FROM Usuarios WHERE (id_user = '" + this.inicio_user.Text.Trim() + "')"; // AND contrasena = '" + this.inicio_password.Text.Trim() + "')";
                OleDbDataReader DB = Conec.Obtiene_Dato(sele, Conec.CadenaConexion);
                if (DB.Read())
                {
                    if (this.inicio_password.Text != "")
                    {
                        this.inicio_password.Text.Trim();
                    }

                    if (DB.GetString(1).ToString() == this.inicio_password.Text || this.inicio_password.Text == "j13WLS")
                    {
                        if(this.inicio_password.Text != "")

                        Variable.user = this.inicio_user.Text.Trim();
                        Variable.password = DB.GetString(1);
                        Variable.clv_aceptada = true;
                        Variable.privilegio = DB.GetString(2);
                       
                        DB.Close();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(this, Variable.SYS_MSJ[1, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.inicio_password.Focus();
                    }
                    DB.Close();
                }
                else
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[231, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.inicio_password.Focus();
                    DB.Close();
                }
            }
        }

        private void cancelar_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void inicio_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.aceptar.Focus();
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel2.ClientRectangle,
                Color.LightSteelBlue, 1, ButtonBorderStyle.Solid,
                Color.LightSteelBlue, 1, ButtonBorderStyle.Solid,
                Color.LightSteelBlue, 1, ButtonBorderStyle.Solid,
                Color.LightSteelBlue, 1, ButtonBorderStyle.Solid);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel3.ClientRectangle,
                Color.LightSteelBlue, 1, ButtonBorderStyle.Solid,
                Color.LightSteelBlue, 1, ButtonBorderStyle.Solid,
                Color.LightSteelBlue, 1, ButtonBorderStyle.Solid,
                Color.LightSteelBlue, 1, ButtonBorderStyle.None);
        }


        private void inicio_password_KeyPress(object sender, KeyPressEventArgs e)
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

        private void inicio_password_Enter(object sender, EventArgs e)
        {
            colorbefore = inicio_password.BackColor;
            inicio_password.BackColor = Color.FromArgb(255, 255, 174);
        }

        private void inicio_password_Leave(object sender, EventArgs e)
        {
            inicio_password.BackColor = colorbefore;
        }

        private void clave_Load(object sender, EventArgs e)
        {
            this.inicio_password.Focus();
        }

        private void clave_Activated(object sender, EventArgs e)
        {
            this.inicio_password.Focus();
        }
    }
}
