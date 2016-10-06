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
    public partial class ProgressContinue : Form    
    {
        private delegate void ProgressDelegateShowDialog(int numero,string texto);
        private ProgressDelegateShowDialog delShowDialog;

        private delegate void ProgressDelegate(string Message);
        public delegate void UpdateBarProgres(int iNumber);
        private ProgressDelegate del;
        private string sMsgPrincipal = "";

        public Label lblStatus = new Label();

        public int iValueMax = 0;
        private int iValueCurrent = 0;

        private int iTipoProgress = 0; //1 -> Progres propio
                                       //3 -> Progres Windows
        public ProgressContinue()
        {
            InitializeComponent();
            try
            {
                del = UpdateProcessInternal;
                delShowDialog = UpdateProcessShowDialog;

                this.StartPosition = FormStartPosition.CenterScreen;
                this.Show();

                Application.DoEvents();

                if (ProgressBarRenderer.IsSupported == true)
                {
                    progressBar1.Enabled = false;
                    progressBar1.Visible = false;
                    iTipoProgress = 1;
                }
                else
                {
                    progressBar1.Enabled = true;
                    progressBar1.Visible = true;
                    iTipoProgress = 3;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        public ProgressContinue(int iPosX, int iPosY)
        {
            InitializeComponent();
            try
            {
                del = UpdateProcessInternal;
                delShowDialog = UpdateProcessShowDialog;

                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(iPosX, iPosY);
                this.Show();

                if (ProgressBarRenderer.IsSupported == true)
                {
                    progressBar1.Enabled = false;
                    progressBar1.Visible = false;
                    iTipoProgress = 1;
                }
                else
                {
                    progressBar1.Enabled = true;
                    progressBar1.Visible = true;
                    iTipoProgress = 3;
                }

                Application.DoEvents();
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message.ToString());
                MessageBox.Show(this, ex.Message);
            }
        }

        public void IniciaProcess(int maximo, string msg1)
        {
            try
            {
                sMsgPrincipal = msg1;
                lbxMsj.Text = msg1;
                lbxMsj.Refresh();

                iValueMax = maximo;
                iValueCurrent = 0;

                if (ProgressBarRenderer.IsSupported == true)
                {
                    if (iTipoProgress == 1)
                    {
                        progressBarDirectRender1.iValueMax = iValueMax;
                        progressBarDirectRender1.Value = 0;
                    }
                    else
                    {
                        progressBar1.Enabled = true;
                        progressBar1.Visible = true;
                        progressBar1.Maximum = iValueMax;
                        progressBar1.Value = 0;
                        iTipoProgress = 3;
                    }
                }
                else
                {
                    if (iTipoProgress == 3)
                    {
                        progressBar1.Maximum = iValueMax;
                        progressBar1.Value = 0;
                    }
                    else
                    {
                        progressBarDirectRender1.iValueMax = iValueMax;
                        progressBarDirectRender1.Value = 0;
                        iTipoProgress = 1;
                    }
                }

                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        public void IniciaProcess(string msg1)
        {
            try
            {
                lbxMsj.Text = msg1;
                lbxMsj.Refresh();

                iValueMax = 0;
                iValueCurrent = 0;

                if (ProgressBarRenderer.IsSupported == true)
                {
                    if (iTipoProgress == 1)
                    {
                        progressBarDirectRender1.iValueMax = iValueMax;
                        progressBarDirectRender1.Value = 0;
                    }
                    else
                    {
                        progressBar1.Enabled = true;
                        progressBar1.Visible = true;
                        progressBar1.Maximum = iValueMax;
                        progressBar1.Value = 0;
                        iTipoProgress = 3;
                    }
                }
                else
                {
                    if (iTipoProgress == 3)
                    {
                        progressBar1.Maximum = iValueMax;
                        progressBar1.Value = 0;
                    }
                    else
                    {
                        progressBarDirectRender1.iValueMax = iValueMax;
                        progressBarDirectRender1.Value = 0;
                        iTipoProgress = 1;
                    }
                }

                Application.DoEvents();
            }
            catch (Exception ex)
            {
               // Console.WriteLine(ex.Message.ToString());
                MessageBox.Show(this, ex.Message);
            }
        }


        public void UpdateProcess(int incremen, string msg)
        {
            try
            {

                iValueCurrent += incremen;


                if (ProgressBarRenderer.IsSupported == true)
                {
                    if (iTipoProgress == 1)
                    {
                        progressBarDirectRender1.Value = iValueCurrent;
                    }
                    else
                    {
                        progressBar1.Enabled = false;
                        progressBar1.Visible = false;
                        progressBarDirectRender1.iValueMax = iValueMax;
                        progressBarDirectRender1.Value = iValueCurrent;
                        iTipoProgress = 1;
                    }
                }
                else
                {
                    if (iTipoProgress == 3)
                    {
                        progressBar1.Value = iValueCurrent;
                    }
                    else
                    {
                        progressBar1.Enabled = true;
                        progressBar1.Visible = true;                        
                        progressBar1.Maximum = iValueMax;
                        progressBar1.Value = iValueCurrent;
                        iTipoProgress = 3;
                    }
                }
                
                lbxMsj.Text = sMsgPrincipal + " " + iValueCurrent.ToString() + "\\" + iValueMax.ToString();
                lbxMsj.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                //MessageBox.Show(this, ex.Message);
            }
        }

        #region Use ShowDialog
        public ProgressContinue(string texto)
        {
            InitializeComponent();
            del = UpdateProcessInternal;
            delShowDialog = UpdateProcessShowDialog;

            this.StartPosition = FormStartPosition.CenterScreen;
            Application.DoEvents();
        }

        public void UpdateProcessShowDialog(int incremen, string msg)
        {
            try
            {
                iValueCurrent += incremen;

                if (ProgressBarRenderer.IsSupported == true)
                {
                    if (iTipoProgress == 1)
                    {
                        progressBarDirectRender1.Value = iValueCurrent;
                    }
                    else
                    {
                        progressBar1.Enabled = false;
                        progressBar1.Visible = false;
                        progressBarDirectRender1.iValueMax = iValueMax;
                        progressBarDirectRender1.Value = iValueCurrent;
                        iTipoProgress = 1;
                    }
                }
                else
                {
                    if (iTipoProgress == 3)
                    {
                        progressBar1.Value = iValueCurrent;
                    }
                    else
                    {
                        progressBar1.Enabled = true;
                        progressBar1.Visible = true;
                        progressBar1.Maximum = iValueMax;
                        progressBar1.Value = iValueCurrent;
                        iTipoProgress = 3;
                    }
                }

                lbxMsj.Text = sMsgPrincipal + " " + iValueCurrent.ToString() + "\\" + iValueMax.ToString();
                lbxMsj.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        public void UpdateProcessShowDialogDelegate(int numero, string texto)
        {
            object[] parametro = {numero, texto};
            if (delShowDialog != null)
            {
                this.Invoke(delShowDialog, parametro);
            }
        }
        #endregion

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

        public void TerminaProcess(string texto)
        {
            iValueCurrent = 0;
            this.Close();
            this.Dispose();
        }

        public void TerminaProcess()
        {
            iValueCurrent = 0;
            this.Close();
            this.Dispose();
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

    public class ProgressBarDirectRender : UserControl
    {
        private int _value;
        private int _iValueMax;

        public int iValueMax
        {
            get { return _iValueMax; }
            set
            {
                _iValueMax = value;
            }
        }
        public int Value
        {
            get { return _value; }
            set
            {
                if (value < 0 || value > _iValueMax)
                    throw new ArgumentOutOfRangeException("value");
                _value = value;
                const int margin = 1;
                using (var g = CreateGraphics())
                {
                    if (_value == 0)
                    {
                        if (ProgressBarRenderer.IsSupported == true)
                        {
                            ProgressBarRenderer.DrawHorizontalBar(g, ClientRectangle);
                        }
                    }
                    else
                    {
                        var rectangle = new Rectangle(ClientRectangle.X + margin,
                                                      ClientRectangle.Y + margin,
                                                      ClientRectangle.Width * _value / _iValueMax - margin * 2,
                                                      ClientRectangle.Height - margin * 2);
                        if (ProgressBarRenderer.IsSupported == true)
                        {
                            ProgressBarRenderer.DrawHorizontalChunks(g, rectangle);
                        }
                    }
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (ProgressBarRenderer.IsSupported == true)
            {
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, ClientRectangle);
            }
        }
    }
}
