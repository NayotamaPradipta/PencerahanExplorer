using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

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
                bfs.tree.display(bfs.tree.root);
                /*
                Search.DFS dfs = new Search.DFS(path, target_name);
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
    
    namespace Search
    { 
        public class BFS
        { 
            // belum bikin tree

            private bool found;
            private string target_name;
            private Queue<string> queue;
            private string start_path;
            public List<string> pathList;
            public Tree.Tree tree;

            public BFS(string start_path, string target_name, bool findall)
            {
                // inisialisasi nilai-nilai atribut
                this.start_path = start_path;
                found = false;
                this.target_name = target_name;
                queue = new Queue<string>();
                pathList = new List<string>();
                tree = new Tree.Tree(start_path);

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
                                    tree.addChild(files[i], tree.root, false);
                                    //Console.WriteLine(tree.getRelativePath(start_path, files[i]));
                                    //Console.WriteLine(Directory.GetParent(files[i]));
                                    //Console.WriteLine(Path.GetPathRoot(files[i]) + ' ' + Path.GetPathRoot(files[i]).Length);
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
                                    tree.addChild(files[i], tree.root, false);
                                    //Console.WriteLine(tree.getRelativePath(start_path, files[i]));
                                    //Console.WriteLine(Directory.GetParent(files[i]));
                                    //Console.WriteLine(Path.GetPathRoot(files[i]) + ' ' + Path.GetPathRoot(files[i]).Length);
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
                            //Console.WriteLine(tree.getRelativePath(start_path, folders[k]));
                            //tree.addChild(folders[k], tree.root, true);
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
        private string full_path;
        private List<Node> child;
        public bool isFolder;

        public Node(string path, bool isFolder)
        {
            this.full_path = path;
            this.name = Path.GetDirectoryName(name);
            child = new List<Node>();
            this.isFolder = isFolder;
        }

        public string getName()
        {
            return name;
        }

        public string getPath()
        {
            return full_path;
        }

        public void addChild(string child_path, bool isFolder)
        {
            child.Add(new Node(child_path, isFolder));
        }

        public bool isChild(string child_name)
        {
            bool isChild = false;
            foreach (Node c in child)
            {
                if (c.getName() == child_name)
                {
                    isChild = true;
                }
            }
            return isChild;
        }

        public Node getChild(string child_name)
        {
            bool found = false;
            int i = 0;
            while (i < child.Count && !found)
            {
                if (child[i].getName() == child_name)
                {
                    found = true;
                }
                else
                {
                    i++;
                }
            }
            if (found)
            {
                return child[i];
            }
            else
            {
                return null;
            }
        }


        public int sumChild()
        {
            return child.Count;
        }

        public List<Node> getChildList()
        {
            return child;
        }

    }
    public class Tree
    {
        public Node root;

        public Tree(string root)
        {
            this.root = new Node(root, true);
        }

        public void addChild(string child_full_path, Node current_path, bool isfolder)
        {
            string relative_path = getRelativePath(current_path.getPath(), child_full_path);
            string root_name = Path.GetPathRoot(relative_path).TrimEnd((char)92);
            if (current_path.getChild(root_name) == null)
            {
                // basis
                // tidak ada child dengan nama yang sama pada Node, buat child baru
                current_path.addChild(child_full_path, isfolder);
            }
            else
            {
                // rekurens
                // terdapat child dengan nama yang sama, pindah ke node tersebut
                addChild(child_full_path, current_path.getChild(root_name), isfolder);
            }
        }

        public void display(Node n)
        {
            Console.WriteLine(n.getName());
            if (n.sumChild() == 0)
            {
                // do nothing
                // basis
            }
            else
            { 
                for (int i = 0; i < n.sumChild(); i++)
                {
                    display(n.getChildList()[i]);
                }
            }
        }

        public string getRelativePath(string relativeTo, string path)
        {
            string relative_path = path.Remove(0, relativeTo.Length + 1);
            return relative_path;
        }

        
    }
}