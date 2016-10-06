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
    public partial class SelectorOrder : Form
    {
        private f3Sincronizar f3Sincronizar;

        


        public SelectorOrder()
        {
            InitializeComponent();
        }

        public SelectorOrder(Form1 form1)
        {
            //this.form1 = form1;
        }

        public SelectorOrder(f3Sincronizar f3Sincronizar)
        {
            // TODO: Complete member initialization
            this.f3Sincronizar = f3Sincronizar;
        }

    





        private void btn_wlabel_Click(object sender, EventArgs e)
        {
            this.Hide();
            f3Sincronizar.ShowOrderWLABEL();
            this.Close();
        }

        private void btn_wls_Click(object sender, EventArgs e)
        {
            this.Hide();
            f3Sincronizar.ShowOrderWLSorWLSD();
            this.Close();
        }



    }
}
