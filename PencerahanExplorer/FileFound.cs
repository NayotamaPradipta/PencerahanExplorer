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
    public partial class FileFound : Form
    {
        // hyperlink belum berupa list
        string path_link;
        public FileFound(string start_path, string final_path)
        {
            InitializeComponent();
            label1.Text = "Starting Path : " + start_path;
            linkLabel1.Text = final_path;
            path_link = final_path;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(path_link);
        }
    }
}
