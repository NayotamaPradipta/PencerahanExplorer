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
                        var watch = new System.Diagnostics.Stopwatch();
                        watch.Start();
                        BFS bfs = new BFS(path, target_name, true);
                        watch.Stop();
                        FileFound f2 = new FileFound(path, bfs.pathList, bfs.isFound(), watch.ElapsedMilliseconds);

                        Application.Run(f2);
                        bfs.tree.displayTree(bfs.visited, bfs.pathList);
                    } 
                    else
                    {
                        var watch = new System.Diagnostics.Stopwatch();
                        watch.Start();
                        BFS bfs = new BFS(path, target_name, false);
                        watch.Stop();
                        FileFound f2 = new FileFound(path, bfs.pathList, bfs.isFound(), watch.ElapsedMilliseconds);
                        Application.Run(f2);
                        bfs.tree.displayTree(bfs.visited, bfs.pathList);
                    }
                    
                }
                else
                {
                    if (f1.scope_choice == 1)
                    {
                        var watch = new System.Diagnostics.Stopwatch();
                        watch.Start();
                        DFS dfs = new DFS(path, target_name, true);
                        watch.Stop();
                        FileFound f2 = new FileFound(path, dfs.pathList, dfs.isFound(), watch.ElapsedMilliseconds);
                        Application.Run(f2);
                        dfs.tree.displayTree(dfs.output, dfs.pathList);
                    }
                    else
                    {
                        var watch = new System.Diagnostics.Stopwatch();
                        watch.Start();
                        DFS dfs = new DFS(path, target_name, false);
                        watch.Stop();
                        FileFound f2 = new FileFound(path, dfs.pathList, dfs.isFound(), watch.ElapsedMilliseconds);
                        Application.Run(f2);
                        dfs.tree.displayTree(dfs.output, dfs.pathList);
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