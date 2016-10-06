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
    public partial class UserDepurar : Form
    {
        private int nopcion;
        Color colorbefore;
        #region Declaracion Class
        ADOutil Conec = new ADOutil();
        #endregion

        public UserDepurar(int opc)
        {
            InitializeComponent();
            this.CancelButton = cancelar;
            nopcion = opc;
            tbxProdini.Focus();
        }
         #region Procesos pushBotones
       

        private void btnCerarEdit_Click(object sender, EventArgs e)
        {
            activarDesactivarEdicion(false);    
        }       
                
        #endregion

        #region Activacion de controles en la vista
        void activarDesactivarEdicion(bool pbActivar)
        {           
            tbxProdini.Enabled = pbActivar;                     
            tbxProdfin.Enabled = pbActivar;      
        }
               
        private void limpiezaTextBoxes()
        {
            tbxProdini.Clear();
            tbxProdfin.Clear();
            activarDesactivarEdicion(true);
            tbxProdini.Focus();
        }
        #endregion
              
        private void UserDepurar_Load(object sender, EventArgs e)
        {
            switch (nopcion)
            {
                case (int)ESTADO.FileSource.fProductos: this.Label5.Text = Variable.SYS_MSJ[167, Variable.idioma]; break;  //producto
                case (int)ESTADO.FileSource.fInfoAdicional: this.Label5.Text = Variable.SYS_MSJ[149, Variable.idioma]; break;  //info adicional
                case (int)ESTADO.FileSource.fOfertas: this.Label5.Text = Variable.SYS_MSJ[165, Variable.idioma]; break;  //oferta
                case (int)ESTADO.FileSource.fMensajes: this.Label5.Text = Variable.SYS_MSJ[168, Variable.idioma]; break;  //mensaje
                case (int)ESTADO.FileSource.fVendedores: this.Label5.Text = Variable.SYS_MSJ[175, Variable.idioma]; break;  //vendedor
            }
        }

        private void btnProcesar_Click(object sender, EventArgs e)
        {

            if (this.tbxProdini.Text != "" && this.tbxProdfin.Text == "")
            {
                

            }
            else if (this.tbxProdini.Text == "" && this.tbxProdfin.Text != "")
            {
                MessageBox.Show(this, Variable.SYS_MSJ[395, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.tbxProdini.Focus();
                return;
            }
            else if (this.tbxProdini.Text == "" && this.tbxProdfin.Text == "")
            {



            }
            else if (this.tbxProdini.Text != "" && this.tbxProdfin.Text != "")
            {
                if (Convert.ToInt32(this.tbxProdini.Text) > Convert.ToInt32(this.tbxProdfin.Text))
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[392, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.tbxProdfin.Focus();
                    return;
                }
            }

            ImpExp DEP = new ImpExp();
            DEP.depurar(nopcion, tbxProdini.Text, tbxProdfin.Text);
            this.Dispose();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {           
            this.Dispose();
        }

        private void tbxProdini_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (tbxProdini.Text == "" && e.KeyChar == '0')
            {
                e.Handled = true;
            }
            else if (Char.IsDigit(e.KeyChar) || e.KeyChar == 8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void tbxProdfin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (tbxProdfin.Text == "" && e.KeyChar == '0')
            {
                e.Handled = true;
            }
            else if (Char.IsDigit(e.KeyChar) || e.KeyChar == 8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
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

        private void tbxProdini_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxProdini.BackColor;
            tbxProdini.BackColor = Color.FromArgb(255, 255, 174);
        }

        private void tbxProdfin_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxProdfin.BackColor;
            tbxProdfin.BackColor = Color.FromArgb(255, 255, 174);
        }

        private void tbxProdini_Leave(object sender, EventArgs e)
        {
            tbxProdini.BackColor = colorbefore;
        }

        private void tbxProdfin_Leave(object sender, EventArgs e)
        {
            tbxProdfin.BackColor = colorbefore;
        }

        private void tbxProdini_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.tbxProdfin.Focus();
            }
        }

        private void tbxProdfin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.aceptar.Focus();
            }
        }

        private void UserDepurar_Activated(object sender, EventArgs e)
        {
            this.tbxProdini.Focus();
        }
    }
}
