using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PencerahanExplorer
{
    public partial class Form2 : Form
    {
        public string target_name;
        public Form2(string path)
        {
            InitializeComponent();
            textBox2.Text = path;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            target_name = textBox1.Text.ToString();
            Close();
        }
    }
}
