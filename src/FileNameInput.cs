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
    public partial class FileNameInput : Form
    {
        public string target_name;
        public int method_choice;
        // 0 -  untuk BFS
        // 1 - untuk DFS
        public int scope_choice;
        // 0 - untuk one file
        // 1 - untuk all files

        public FileNameInput(string path)
        {
            InitializeComponent();
            textBox2.Text = path;
            method_choice = -1;
            scope_choice = -1;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (method_choice != -1 && scope_choice != -1 && !String.IsNullOrEmpty(textBox1.Text))
            {
                target_name = textBox1.Text.ToString();
                Close();
            }
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            method_choice = 0;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            method_choice = 1;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            scope_choice = 1;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            scope_choice = 0;
        }

    }
}
