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
    public partial class UserTablaNutricional : UserControl
    {
        public UserTablaNutricional()
        {
            InitializeComponent();
           
        }
        
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void UserTablaNutricional_Load(object sender, EventArgs e)
        {

        }
    }
}
