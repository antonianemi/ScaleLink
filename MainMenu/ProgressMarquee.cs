using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace MainMenu
{
    public partial class ProgressMarquee : Form
    {
        public int iValueMax;

        public ProgressMarquee()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            progressBar1.Style = ProgressBarStyle.Marquee;            

            Application.DoEvents();
        }

        public void IniciaProcess(string msg1)
        {
            lbxMsj.Text = msg1;
            lbxMsj.Refresh();

            Application.DoEvents();
        }

        public void UpdateProcessInternal(string msg)
        {
            if (this.Handle == null)
            {
                return;
            }
            lbxMsj.Text = msg;
            lbxMsj.Refresh();

            Application.DoEvents();
        }

        public void UpdateProgressBar()
        {
            Application.DoEvents();
        }
        public void TerminaProcess()
        {
            this.Close();
            this.Dispose();
            Application.DoEvents();
        }
        private void Process_Load(object sender, EventArgs e)
        {
            TransparencyKey = Color.Empty;
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
    }

}
