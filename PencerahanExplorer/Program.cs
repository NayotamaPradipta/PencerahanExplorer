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

                if (f1.method_choice == 0)
                {
                    // BFS
                    if (f1.scope_choice == 1)
                    {
                        BFS bfs = new BFS(path, target_name, true);
                        FileFound f2 = new FileFound(path, bfs.pathList, bfs.isFound());
                        Application.Run(f2);
                        bfs.tree.display(bfs.tree.root);
                    }
                    else
                    {
                        BFS bfs = new BFS(path, target_name, false);
                        FileFound f2 = new FileFound(path, bfs.pathList, bfs.isFound());
                        Application.Run(f2);
                        bfs.tree.display(bfs.tree.root);
                    }
                    
                }
                else
                {
                    if (f1.scope_choice == 1)
                    {
                        DFS dfs = new DFS(path, target_name, true);
                        FileFound f2 = new FileFound(path, dfs.pathList, dfs.isFound());
                        Application.Run(f2);
                    }
                    else
                    {
                        DFS dfs = new DFS(path, target_name, false);
                        FileFound f2 = new FileFound(path, dfs.pathList, dfs.isFound());
                        Application.Run(f2);
                    }
                }
            }
            else
            {
                Console.WriteLine("Wrong path!");
            }

        }
    }
}