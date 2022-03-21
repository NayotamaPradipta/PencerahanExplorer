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
            FileNameInput f1;
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowserDialog.SelectedPath;
                f1 = new FileNameInput(path);
                Application.Run(f1);
                string target_name = f1.target_name;
                Console.WriteLine("Starting folder : " + path);
                
                Search.BFS bfs = new Search.BFS(path, target_name, true);
                FileFound f2 = new FileFound(path, bfs.pathList, bfs.isFound());
                Application.Run(f2);
                /*
                Search.DFS dfs = new Search.DFS(path, target_name);
                if (dfs.isFound())
                {
                    FileFound f2 = new FileFound(path, dfs.target_path);
                    Application.Run(f2);
                }
                */

            }
            else
            {
                Console.WriteLine("Wrong path!");
            }
            
        }
    }
    
    namespace Search
    { 
        public class BFS
        { 
            // belum bikin tree

            private bool found;
            private string target_name;
            private Queue<string> queue;
            public List<string> pathList;

            public BFS(string start_path, string target_name, bool findall)
            {
                // inisialisasi nilai-nilai atribut
                found = false;
                this.target_name = target_name;
                queue = new Queue<string>();
                pathList = new List<string>();

                queue.Enqueue(start_path);
                if (findall)
                {
                    while (queue.Count > 0)
                    {
                        string head = queue.Dequeue();
                        SearchBFS(head, findall);
                    }
                }
                else
                {
                    while (!found && queue.Count > 0)
                    {
                        string head = queue.Dequeue();
                        SearchBFS(head, findall);
                    }
                }

            }

            private void SearchBFS(string path, bool findall)
            {
                string[] files = null;
                string[] folders = null;
                try
                {
                    // pake exception handling karena kadang nemu restricted folder
                    files = Directory.GetFiles(path);
                    folders = Directory.GetDirectories(path);

                }
                catch { }
                finally
                {
                    if (files != null)
                    {
                        int i;
                        // iterate over files
                        if (findall)
                        {
                            for (i = 0; i < files.Length; i++)
                            {
                                if (Path.GetFileName(files[i]) == target_name)
                                {
                                    found = true;
                                    pathList.Add(files[i]);
                                }
                            }
                        }
                        else
                        {
                            i = 0;
                            while (!found && i < files.Length)
                            {
                                if (Path.GetFileName(files[i]) == target_name)
                                {
                                    found = true;
                                    pathList.Add(files[i]);
                                }
                                else
                                {
                                    i++;
                                }
                            }
                        }
                    }
                    // file not found in current path, enqueue subdirectories
                    if ((findall || (!findall && !found)) && folders != null)
                    {
                        for (int k = 0; k < folders.Length; k++)
                        {
                            queue.Enqueue(folders[k]);
                        }
                    }
                }
            }

            public bool isFound()
            {
                return found;
            }

            public List<string> get_target_path()
            {
                return pathList;
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

namespace Tree
{
    public class Node
    {
        private string name;
        private string path;
        private List<Node> child;
        private bool isFolder;

        public Node(string root, bool isFolder)
        {
            if (isFolder)
            {
                name = Path.GetDirectoryName(root);
            }
            else
            {
                name = Path.GetFileName(root);
            }
            path = root;
            this.isFolder = isFolder;
            child = new List<Node>();

        }

        public void AddChild()
        {
            string[] files = null;
            string[] folders = null;
            try
            {
                files = Directory.GetFiles(path);
                folders = Directory.GetDirectories(path);
            }
            catch { }
            finally
            {
                for (int i = 0; i < files.Length; i++)
                {
                    child.Add(new Node(files[i], false));
                }

                for (int i = 0; i < folders.Length; i++)
                {
                    child.Add(new Node(folders[i], true));
                }
            }
        }
    }

    public class Tree
    {
        private Node root;

        public Tree(string root)
        {
            this.root = new Node(root, true);
        }

    }
}