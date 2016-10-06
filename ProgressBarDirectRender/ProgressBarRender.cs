using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgressBarDirectRender
{
    public partial class ProgressBarRender : Component
    {
        public ProgressBarRender()
        {
            InitializeComponent();
        }

        public ProgressBarRender(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
