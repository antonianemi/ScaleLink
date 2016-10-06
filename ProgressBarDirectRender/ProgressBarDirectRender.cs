using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgressBarDirectRender
{
    public partial class ProgressBarDirectRender: UserControl
    {
        private int _value;
        private int _iValueMax;

        public ProgressBarDirectRender()
        {
            InitializeComponent();
        }
       
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
                        ProgressBarRenderer.DrawHorizontalBar(g, ClientRectangle);
                    else
                    {
                        var rectangle = new Rectangle(ClientRectangle.X + margin,
                                                      ClientRectangle.Y + margin,
                                                      ClientRectangle.Width * _value / _iValueMax - margin * 2,
                                                      ClientRectangle.Height - margin * 2);
                        ProgressBarRenderer.DrawHorizontalChunks(g, rectangle);
                    }
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ProgressBarRenderer.DrawHorizontalBar(e.Graphics, ClientRectangle);
        }
    }
}
