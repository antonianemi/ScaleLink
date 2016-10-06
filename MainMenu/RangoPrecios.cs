using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MainMenu
{
    public partial class RangoPrecios : Form
    {        
        public static string incremento;
        public static string codigo_inicio;
        public static string codigo_final;
        public static bool cambiar_precio = false;
        Color colorbefore;
            

        public RangoPrecios()
        {
            InitializeComponent();
            this.CancelButton = btnCan;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (tbxCodigo1.Text == "")
            {
                MessageBox.Show(this, Variable.SYS_MSJ[395, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.tbxCodigo1.Focus();
                return;
            }
            else if (tbxCodigo1.Text != "" && tbxCodigo2.Text != "")
            {
                if (Convert.ToInt32(tbxCodigo1.Text) > Convert.ToInt32(tbxCodigo2.Text))
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[392, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.tbxCodigo2.Focus();
                    return;
                }
            }
            
            if (tbxDescuento.Text == "")
            {
                MessageBox.Show(this, Variable.SYS_MSJ[393, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.tbxDescuento.Focus();
                return;
            }
            else if (tbxDescuento.Text != null)
            {
                if (Convert.ToDouble(tbxDescuento.Text) == 0)
                {
                    MessageBox.Show(this, Variable.SYS_MSJ[394, Variable.idioma], Variable.SYS_MSJ[40, Variable.idioma], MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.tbxDescuento.Focus();
                    return;
                }
            }

            codigo_inicio = this.tbxCodigo1.Text;
            codigo_final = this.tbxCodigo2.Text;
            incremento = this.tbxDescuento.Text;
            cambiar_precio = true;

            this.Close();
        }

        private void btnCan_Click(object sender, EventArgs e)
        {
            cambiar_precio = false;
            this.Close();
            this.Dispose();
        }                            


        private void tbxDescuento_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.btnOk.Focus();
        }

        private void tbxCodigo2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.tbxDescuento.Focus();
        }

        private void tbxCodigo2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (tbxCodigo2.Text == "" && e.KeyChar == '0')
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

        private void tbxCodigo1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.tbxCodigo2.Focus();
        }

        private void tbxCodigo1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (tbxCodigo1.Text == "" && e.KeyChar == '0')
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

        private void tbxDescuento_Validated(object sender, EventArgs e)
        {
            string dat;

            dat = Variable.validar_salida(tbxDescuento.Text, 10);

            if (dat != "")
            {
                tbxDescuento.Text = dat;
            }
            else tbxDescuento.Text = "0";
        }

        private void tbxDescuento_KeyPress(object sender, KeyPressEventArgs e)
        {
            int iCountDot = tbxDescuento.Text.Count(c => c == '.');
            int iPosDot =   tbxDescuento.Text.IndexOf('.');
            int iCountMenos = tbxDescuento.Text.Count(c => c == '-');

            if (iCountMenos == 0 && tbxDescuento.Text == "" && e.KeyChar == '-')
            {
                e.Handled = false;
            }
            else if (e.KeyChar == '-')
            {
                e.Handled = true;

            }
            else if (e.KeyChar == '.')
            {
                if (iCountDot > 0)
                {
                    e.Handled = true;
                }
                else if (tbxDescuento.Text.Count() > 0 && tbxDescuento.Text.Count() < 3 && iCountMenos == 0)
                {
                    e.Handled = false;
                }
                else if (tbxDescuento.Text.Count() > 1 && tbxDescuento.Text.Count() < 4 && iCountMenos == 1)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            else if (Char.IsDigit(e.KeyChar))
            {
                if (iCountMenos == 0)
                {
                    if (tbxDescuento.Text.Count() < 2 && iCountDot == 0)
                    {
                        e.Handled = false;
                    }
                    else if (iCountDot == 1)
                    {
                        if (iPosDot == 1)
                        {
                            if (tbxDescuento.Text.Count() < 4)
                            {
                                e.Handled = false;
                            }
                            else
                            {
                                e.Handled = true;
                            }
                        }
                        else if (iPosDot == 2)
                        {
                            if (tbxDescuento.Text.Count() < 5)
                            {
                                e.Handled = false;
                            }
                            else
                            {
                                e.Handled = true;
                            }
                        }
                        else
                        {
                            e.Handled = true;
                        }
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
                else
                {
                    if (tbxDescuento.Text.Count() < 3 && iCountDot == 0)
                    {
                        e.Handled = false;
                    }
                    else if (iCountDot == 1)
                    {
                        if (iPosDot == 2)
                        {
                            if (tbxDescuento.Text.Count() < 5)
                            {
                                e.Handled = false;
                            }
                            else
                            {
                                e.Handled = true;
                            }
                        }
                        else if (iPosDot == 3)
                        {
                            if (tbxDescuento.Text.Count() < 6)
                            {
                                e.Handled = false;
                            }
                            else
                            {
                                e.Handled = true;
                            }
                        }
                        else
                        {
                            e.Handled = true;
                        }
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
            else if (e.KeyChar != (char)Keys.Enter && e.KeyChar != (char)Keys.Tab && e.KeyChar != 8)
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
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

        private void tbxCodigo1_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxCodigo1.BackColor;
            tbxCodigo1.BackColor = Color.FromArgb(255, 255, 174);
        }

        private void tbxCodigo2_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxCodigo2.BackColor;
            tbxCodigo2.BackColor = Color.FromArgb(255, 255, 174);
        }

        private void tbxDescuento_Enter(object sender, EventArgs e)
        {
            colorbefore = tbxDescuento.BackColor;
            tbxDescuento.BackColor = Color.FromArgb(255, 255, 174);
        }

        private void tbxCodigo1_Leave(object sender, EventArgs e)
        {
            tbxCodigo1.BackColor = colorbefore;
        }

        private void tbxCodigo2_Leave(object sender, EventArgs e)
        {
            tbxCodigo2.BackColor = colorbefore;
        }

        private void tbxDescuento_Leave(object sender, EventArgs e)
        {
            tbxDescuento.BackColor = colorbefore;
        }

        private void RangoPrecios_Activated(object sender, EventArgs e)
        {
            this.tbxCodigo1.Focus();
        }
    }
}
