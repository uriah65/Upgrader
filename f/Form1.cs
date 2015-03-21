using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace f
{
    public partial class Form1 : Form
    {
        public Form1(string[] args)
        {
            InitializeComponent();

            string wrap = "Args: ";
            foreach (string arg in args)
            {
                wrap += arg + "; ";
            }

            this.textBox1.Text = wrap;

        }
    }
}
