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
    public partial class IpadInfo : Form
    {
        public IpadInfo()
        {
            InitializeComponent();
        }

        private void IpadInfo_Load(object sender, EventArgs e)
        {
            tbxIp.Text = new GetIP().IPStr;
            this.tbxPort.Text = "50001";
        }

        private void aceptar_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel1.ClientRectangle,
                Color.LightSteelBlue, 1, ButtonBorderStyle.Solid,
                Color.LightSteelBlue, 1, ButtonBorderStyle.Solid,
                Color.LightSteelBlue, 1, ButtonBorderStyle.Solid,
                Color.LightSteelBlue, 1, ButtonBorderStyle.Solid);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.panel2.ClientRectangle,
                Color.LightSteelBlue, 1, ButtonBorderStyle.Solid,
                Color.LightSteelBlue, 1, ButtonBorderStyle.Solid,
                Color.LightSteelBlue, 1, ButtonBorderStyle.Solid,
                Color.LightSteelBlue, 1, ButtonBorderStyle.None);
        }
    }
}
