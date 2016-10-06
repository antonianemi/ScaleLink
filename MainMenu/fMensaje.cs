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
    public partial class fMensaje : Form
    {
        //public fMensaje()
        //{
        //    InitializeComponent();
        //}

        public void valoresDeVista(string psNombre, string psmensaje, int botones)
        {
            InitializeComponent();
            this.Name = psNombre;
            this.rtbMensaje.AppendText(psmensaje);
            this.Show();
        }

        void mostrarBotones(object sender, EventArgs e)
        {
            this.Close();
            
        }
    }
}
