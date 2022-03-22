using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Search;

namespace PencerahanExplorer
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FileNameInput f1;
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowserDialog.SelectedPath;
                f1 = new FileNameInput(path);
                Application.Run(f1);
                string target_name = f1.target_name;
                Console.WriteLine("Starting folder : " + path);


                BFS bfs = new BFS(path, target_name, true);
                FileFound f2 = new FileFound(path, bfs.pathList, bfs.isFound());
                Application.Run(f2);
                bfs.tree.display(bfs.tree.root);
                /*
                DFS dfs = new DFS(path, target_name);
                if (dfs.isFound())
                {
                    FileFound f2 = new FileFound(path, dfs.target_path);
                    Application.Run(f2);
                }
                */
                //Console.WriteLine(bfs.tree.getRelativePath("C:\\Users\\ACER\\Documents", "C:\\Users\\ACER\\Documents\\GitHub"));

            }
            else
            {
                Console.WriteLine("Wrong path!");
            }

        }
    }
}