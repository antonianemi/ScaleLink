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

    public partial class cambiopsw : Form
    {
        Color colorbefore;

        public cambiopsw()       
        {
            InitializeComponent();
            this.CancelButton = cancelar;
            this.tbxUsuario.Text = Variable.user;
            this.tbxContActual.Focus();
        }

        private void aceptar_Click(object sender, EventArgs e)
        {
            ADOutil Conec = new ADOutil();
            if (Variable.password == this.tbxContActual.Text.Trim())
            {
                if (this.tbxContNueva.Text.Trim() == this.tbxContConfirm.Text.Trim())
                {
                    Conec.Condicion = "( id_user = '" + Variable.user + "')";
                    Conec.CadenaSelect = "UPDATE Usuarios " +
                        "SET contrasena = '" + this.tbxContNueva.Text.Trim() + "' WHERE " + Conec.Condicion;

                    Conec.ActualizaReader(Conec.CadenaConexion, Conec.CadenaSelect, "Usuarios");

                    Variable.password = this.tbxContNueva.Text.Trim();

                    MessageBox.Show(this, Variable.SYS_MSJ[391, Variable.idioma], Variable.SYS_MSJ[41, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[301, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(this, Variable.SYS_MSJ[1, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }            
        }

        private void cancelar_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
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


        #region tbxContActual
        private void tbxContActual_KeyPress(object sender, KeyPressEventArgs e)
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
        private void tbxContActual_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                /*if (Variable.password != this.tbxContActual.Text.Trim())
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[1, Variable.idioma].ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.tbxContActual.Focus();
                }
                else*/
                this.tbxContNueva.Focus();
            }
        }
        private void tbxContActual_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxContActual.BackColor;
            tbxContActual.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxContActual_Leave(object sender, EventArgs e)
        {
            tbxContActual.BackColor = colorbefore;
        }
        #endregion

        #region tbxContNueva
        private void tbxContNueva_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.tbxContConfirm.Focus();
            }
        } 
        private void tbxContNueva_KeyPress(object sender, KeyPressEventArgs e)
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
        private void tbxContNueva_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxContNueva.BackColor;
            tbxContNueva.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxContNueva_Leave(object sender, EventArgs e)
        {
            tbxContNueva.BackColor = colorbefore;
        }
        #endregion

        #region tbxContConfirm
        private void tbxContConfirm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                /*if (this.tbxContConfirm.Text.Trim() != this.tbxContNueva.Text.Trim())
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[301, Variable.idioma].ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.tbxContNueva.Focus();
                }
                else*/
                this.aceptar.Focus();
            }
        }

        private void tbxContConfirm_KeyPress(object sender, KeyPressEventArgs e)
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
        private void tbxContConfirm_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxContConfirm.BackColor;
            tbxContConfirm.BackColor = Color.FromArgb(255, 255, 174);
        }
        private void tbxContConfirm_Leave(object sender, EventArgs e)
        {
            tbxContConfirm.BackColor = colorbefore;
        }
        #endregion      


        /*public IButtonControl CancelButton
        { //get; set; }
            get { return cancelar; }
        }*/
    }
}
