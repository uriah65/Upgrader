using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Upgrader
{
    public partial class UpdatePopup : Form
    {
        private int _timerCount = 1;
        private int _timerLimit = 7 * 3;

        public UpdatePopup()
        {
            InitializeComponent();
        }

        private void UpdatePopup_Load(object sender, EventArgs e)
        {
            this.timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            this._timerCount++;
            if (this._timerCount > this._timerLimit)
            {
                this.Close();
            }

            Bitmap image = null;
            switch (this._timerCount % 7)
            {
                case 1:

                    image = Properties.Resources.upgrade2;
                    break;

                case 2:
                    image = Properties.Resources.upgrade3;
                    break;

                case 3:
                    image = Properties.Resources.upgrade4;
                    break;

                case 4:
                    image = Properties.Resources.upgrade5;
                    break;

                case 5:
                    image = Properties.Resources.upgrade6;
                    break;

                case 6:
                    image = Properties.Resources.upgrade7;
                    break;

                default:
                    image = Properties.Resources.upgrade1;
                    break;
            }

            this.pictureBox1.Image = image;
        }
    }
}
