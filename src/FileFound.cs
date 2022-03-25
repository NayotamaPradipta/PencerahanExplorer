using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PencerahanExplorer
{
    public partial class FileFound : Form
    {
        public FileFound(string start_path, List<string> final_path, bool isFound, long time)
        {
            InitializeComponent();
            label1.Text = "Starting Path : " + start_path;
            if (isFound)
            {
                LinkLabel[] links = new LinkLabel[final_path.Count];
                for (int i = 0; i < links.Length; i++)
                {
                    links[i] = new LinkLabel();
                    links[i].Text = final_path[i];
                    links[i].Links[0].LinkData = Path.GetDirectoryName(final_path[i]);
                    links[i].AutoSize = true;
                    links[i].Location = new Point(50, 100 + (i * 20));
                    Controls.Add(links[i]);
                    links[i].LinkClicked += new LinkLabelLinkClickedEventHandler(link_LinkClicked);
                }
            }
            else
            {
                Label labelNotFound = new Label();
                labelNotFound.Text = "Tidak ada path yang sesuai";
                labelNotFound.AutoSize = true;
                labelNotFound.Location = new Point(50, 100);
                Controls.Add(labelNotFound);
            }
            label3.Text = "Time Elapsed: " + time + " ms";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { 
            System.Diagnostics.Process.Start(e.Link.LinkData as string);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
