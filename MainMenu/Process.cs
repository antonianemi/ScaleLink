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
    public partial class Process : Form
    {
        private delegate void ProgressDelegate(string Message);
        public delegate void UpdateBarProgres(int iNumber);
        private ProgressDelegate del;

        public Label lblStatus = new Label();
        public ProgressBar pb1 = new ProgressBar();

        private int iValueMax;
        private int iValueCurrent;

        public Process()
        {
            InitializeComponent();
            del = UpdateProcessInternal;
        }
        public Process(int iPosX, int iPosY)
        {
            InitializeComponent();
            del = UpdateProcessInternal;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(iPosX, iPosY);
            this.Show();

            pictureBox1.Size = new Size(0, 23);
            iValueCurrent = 0;
        }

        public void IniciaProcess(int maximo, string msg1, string msg2,int op)
        {
            lbxTit.Text = msg1;
            lbxTit.Refresh();

            lbxMsj.Text = msg2;
            lbxMsj.Refresh();

            pictureBox1.Size = new Size(0, 23);

            iValueMax = maximo;
            iValueCurrent = 0;
            Application.DoEvents();
        }

        public void IniciaProcess(string msg1, string msg2)
        {
            lbxTit.Text = msg1;
            lbxTit.Refresh();

            lbxMsj.Text = msg2;
            lbxMsj.Refresh();
            iValueCurrent = 0;
            iValueMax = 100;
        }

        public void UpdateProcess(int incremen, string msg)
        {
            try
            {
                double numerator, denominator, completed;
                numerator = iValueCurrent;
                denominator = iValueMax;
                completed = (numerator / denominator) * 100.0;

                iValueCurrent += incremen;

                float fValue = 368 / (float)(iValueMax);
                double dValueW = fValue * (double)(iValueCurrent);                
                int iValueW = (int)(dValueW);
                pictureBox1.Size = new Size(iValueW, 23);

                lbxTit.Text = msg;
                lbxTit.Refresh();

                lbxMsj.Text = iValueCurrent.ToString() + "\\" + iValueMax.ToString();
                lbxMsj.Refresh();
                Application.DoEvents();                
                Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        public void UpdateProcessInternal(string msg)
        {
            if (this.Handle == null)
            {
                return;
            }
            lbxMsj.Text = msg;
            lbxMsj.Refresh();
        }

        public void UpdateProcess(string progress)
        {
            this.Invoke(del, progress);
        }

        public void TerminaProcess(string msg)
        {
            lbxMsj.Text = msg;
            lbxMsj.Refresh();
            iValueCurrent = 0;
            this.Close();
            this.Dispose();
        }

        public void TerminaProcess()
        {
            iValueCurrent = 0;
        }
        private void Process_Load(object sender, EventArgs e)
        {
            TransparencyKey = Color.Empty;
        }
    }
}
