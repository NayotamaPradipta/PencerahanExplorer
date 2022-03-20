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
        private LinkLabel[] links;
        public FileFound(string start_path, List<string> final_path)
        {
            InitializeComponent();
            label1.Text = "Starting Path : " + start_path;
            links = new LinkLabel[final_path.Count];
            for (int i = 0; i < links.Length; i++)
            {
                links[i] = new LinkLabel();
                links[i].Text = final_path[i];
                links[i].Links[0].LinkData = final_path[i];
                links[i].AutoSize = true;
                links[i].Location = new Point(50, 100 + (i*20));
                Controls.Add(links[i]);
                links[i].LinkClicked += new LinkLabelLinkClickedEventHandler(link_LinkClicked);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Console.WriteLine(e.Link.LinkData as string);
            System.Diagnostics.Process.Start(e.Link.LinkData as string);
        }

    }
}
