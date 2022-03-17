using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using PencerahanExplorer;

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
            Form2 f1;
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowserDialog.SelectedPath;
                f1 = new Form2(path);
                Application.Run(f1);
                string target_name = f1.target_name;
                Console.WriteLine("Starting folder : " + path);
                Search.BFS bfs = new Search.BFS(path, target_name);
                if (bfs.isFound())
                {
                    Console.WriteLine("File found in path : " + bfs.target_path);
                    Form3 f2 = new Form3(path, bfs.target_path);
                    Application.Run(f2);
                }
                else
                {
                    Console.WriteLine("File not found ");
                }
            }
            else
            {
                Console.WriteLine("Wrong path!");
            }
            
        }
    }
    /*
    namespace WindowsFormsApp1
    {
        class ViewerSample // Contoh Visualisasi Graf
        {
            public static void Main()
            {
                //create a form
                System.Windows.Forms.Form form = new System.Windows.Forms.Form();
                //create a viewer object
                Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new
                Microsoft.Msagl.GraphViewerGdi.GViewer();
                //create a graph object
                Microsoft.Msagl.Drawing.Graph graph = new
                Microsoft.Msagl.Drawing.Graph("graph");
                //create the graph content
                graph.AddEdge("A", "B");
                graph.AddEdge("B", "C");
                graph.AddEdge("A", "C").Attr.Color = Microsoft.Msagl.Drawing.Color.Green;
                graph.FindNode("A").Attr.FillColor =
                Microsoft.Msagl.Drawing.Color.Magenta;
                graph.FindNode("B").Attr.FillColor =
                Microsoft.Msagl.Drawing.Color.MistyRose;
                Microsoft.Msagl.Drawing.Node c = graph.FindNode("C");
                c.Attr.FillColor = Microsoft.Msagl.Drawing.Color.PaleGreen;
                c.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Diamond;
                //bind the graph to the viewer
                viewer.Graph = graph;
                //associate the viewer with the form
                form.SuspendLayout();
                viewer.Dock = System.Windows.Forms.DockStyle.Fill;
                form.Controls.Add(viewer);
                form.ResumeLayout();
                //show the form
                form.ShowDialog();
            }
        }
    }
    */
    namespace Search
    { 
        public class BFS
        {
            // belum handle untuk banyak instance dengan nama sama di folder yang berbeda
            // belum bikin tree

            private bool found;
            private string target_name;
            public string target_path; // sementara public
            private Queue<string> queue;

            public BFS(string start_path, string target_name)
            {
                // inisialisasi nilai-nilai atribut
                found = false;
                this.target_name = target_name;
                target_path = "NULL";
                queue = new Queue<string>();
                queue.Enqueue(start_path);
                while (!found && queue.Count > 0)
                {
                    string head = queue.Dequeue();
                    SearchBFS(head);
                }

            }

            private void SearchBFS(string path)
            {
                string[] files = null;
                try
                {
                    // pake exception handling karena kadang nemu restricted folder
                    files = Directory.GetFiles(path);
                
                }
                catch { }
                finally
                {
                    if (files != null)
                    {
                        int i = 0;
                        // iterate over files
                        while (!found && i < files.Length)
                        {
                            if (Path.GetFileName(files[i]) == target_name)
                            {
                                found = true;
                                target_path = files[i];
                            }
                            else
                            {
                                i++;
                            }
                        }
                        // file not found in current path, enqueue subdirectories
                        if (!found)
                        {
                            string[] folders = Directory.GetDirectories(path);
                            for (int k = 0; k < folders.Length; k++)
                            {
                                queue.Enqueue(folders[k]);
                            }
                        }
                    }
                }
            }

            public bool isFound()
            {
                return found;
            }

            public string get_target_path()
            {
                return target_path;
            }
        }

        public class DFS
        {
            // Kurang lebih modifikasi dari BFS yang di atas
            // Untuk mencari file pertama dengan nama input

            // Inisialisasi atribut
            private bool found;
            private string target_name;
            public string target_path; // sementara public
            private Stack<string> stack; // untuk keep track current path
            private List<string> output; // untuk keep track visited path

            public DFS(string start_path, string target_name)
            {
                // Inisialisasi atribut
                found = false;
                this.target_name = target_name;
                target_path = "NULL";
                stack = new Stack<string>();
                stack.Push(start_path);
                output = new List<string>();
                output.Add(start_path);

                // Loop DFS utama
                while (!found && stack.Count > 0)
                {
                    string top = stack.Pop();
                    SearchDFS(top);
                }
            }

            private void SearchDFS(string path)
            {
                string[] files = null;

                try
                {
                    // Exception handling untuk restricted folder
                    files = Directory.GetFiles(path);
                }
                catch { }

                finally
                {
                    if (files != null)
                    {
                        // Iterate over files in current path
                        int i = 0;
                        while (!found && i < files.Length)
                        {
                            if (Path.GetFileName(files[i]) == target_name)
                            {
                                found = true; 
                                target_path = files[i]; // Pencarian selesai
                            }
                            else
                            {
                                i++;
                            }
                        }

                        // File not found in current path, lanjut ke path selanjutnya
                        if (!found)
                        {
                            string[] folders = Directory.GetDirectories(path);
                            for (int k = 0; k < folders.Length; k++)
                            {
                                if (output.Contains(folders[k])) // Cek apakah sudah visited
                                {
                                    // skip folder
                                }
                                else
                                {
                                    stack.Push(folders[k]);
                                    output.Add(folders[k]);
                                    SearchDFS(folders[k]);
                                }
                            }
                            stack.Pop();
                        }
                    }
                }
            }

            public bool isFound() // Apakah ketemu
            {
                return found;
            }

            public string get_target_path() // Ketemunya di path mana
            {
                return target_path;
            }


        }
    }

}
