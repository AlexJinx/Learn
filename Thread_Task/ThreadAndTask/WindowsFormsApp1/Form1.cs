using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button1.Click += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Console.WriteLine("ssss");
            Debug.WriteLine("sssss");
            Debug.Assert(1 == 1);
            Debug.Assert(1 == 2);
        }
    }
}
