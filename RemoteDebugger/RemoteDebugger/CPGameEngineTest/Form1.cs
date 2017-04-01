using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CPGameEngineTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private DateTime _lastTime = DateTime.Now;

        private void _frameTimer_Tick(object sender, EventArgs e)
        {
            var ct = DateTime.Now;
            var delta = ct - _lastTime;
            HLRemoting.GameEngine ge = HLRemoting.GameEngine.Instance();
            ge.Progress((float)(delta.Seconds + delta.Milliseconds / 1000.0));

            label1.Text = string.Format("{0} {1}/{2}/{3}  M:{4} / T:{5}", ge.GameContext.Time, ge.GameYear(), ge.GameMonth(), ge.GameDay(), ge.MailCount(), ge.ThreadCount());

            _lastTime = ct;
        }
    }
}
