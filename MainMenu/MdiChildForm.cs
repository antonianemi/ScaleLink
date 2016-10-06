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
    public partial class MdiChildForm : Form
    {
        public MdiChildForm()
        {            
            InitializeComponent();
            this.Load += new System.EventHandler(this.MdiChildForm_Load);
            this.Activated +=new EventHandler(MdiChildForm_Activated);
        }

        void MdiChildForm_Activated(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.BringToFront();
           // this.Activate();
            //this.ActiveMdiChild.Activate();
            //this.Focus();
        }

        private void MdiChildForm_Load(object sender, EventArgs e)
        {          
            this.WindowState = FormWindowState.Maximized;
            this.BringToFront();
            //this.Focus();
        }
    }
}
