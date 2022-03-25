using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Tree;

namespace Search
{
    public class BFS
    {

        private bool found;
        private string target_name;
        private Queue<string> queue;
        private string start_path;
        public List<string> pathList;
        public Tree.Tree tree;
        public List<string> visited;

        public BFS(string start_path, string target_name, bool findall)
        {
            // inisialisasi nilai-nilai atribut
            this.start_path = start_path;
            found = false;
            this.target_name = target_name;
            queue = new Queue<string>();
            pathList = new List<string>();
            tree = new Tree.Tree(start_path);
            visited = new List<string>();

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
                            tree.addChild(files[i], tree.root, false);
                            if (Path.GetFileName(files[i]) == target_name)
                            {
                                found = true;
                                pathList.Add(files[i]);
                            }
                            else
                            {
                                visited.Add(files[i]);
                            }
                        }
                    }
                    else
                    {
                        i = 0;
                        while (!found && i < files.Length)
                        {
                            tree.addChild(files[i], tree.root, false);
                            if (Path.GetFileName(files[i]) == target_name)
                            {
                                found = true;
                                pathList.Add(files[i]);
                            }
                            else
                            {
                                visited.Add(files[i]);
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
                        tree.addChild(folders[k], tree.root, true);
                    }
                }
                visited.Add(path);
            }
        }

        
        public bool isFound()
        {
            return found;
        }
    }

    public class DFS
    {
        // Kurang lebih modifikasi dari BFS yang di atas
        // Untuk mencari file pertama dengan nama input

        // Inisialisasi atribut
        private bool found;
        private string target_name;
        public List<string> pathList; // sementara public
        private Stack<string> stack; // untuk keep track current path
        public List<string> output; // untuk keep track visited path
        public Tree.Tree tree;
        public DFS(string start_path, string target_name, bool findall)
        {
            // Inisialisasi atribut
            found = false;
            this.target_name = target_name;
            pathList = new List<string>();
            stack = new Stack<string>();
            stack.Push(start_path);
            output = new List<string>();
            output.Add(start_path);
            tree = new Tree.Tree(start_path);
            BuildTreeDFS(start_path);
            // Loop DFS utama
            SearchDFS(stack.Peek(), findall);

        }
        private void CheckFileInDirectory(string[] files, bool findall)
        {
            if (files != null)
            {
                Array.Sort(files);
                int j = 0;
                if (!findall)
                {
                    while (!found && j < files.Length)
                    {
                        // tree.addChild(files[j], tree.root, false);
                        if (Path.GetFileName(files[j]) == target_name)
                        {
                            found = true;
                            pathList.Add(files[j]);
                        }
                        else
                        {
                            output.Add(files[j]);
                            j++;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        // tree.addChild(files[i], tree.root, false);
                        if (Path.GetFileName(files[i]) == target_name)
                        {
                            pathList.Add(files[i]);
                            found = true;
                        } else
                        {
                            output.Add(files[i]);
                        }
                    }
                }
                if (findall && stack.Count > 1)
                {
                    stack.Pop();
                    SearchDFS(stack.Peek(), findall);
                }
                
            }
        }
        private void SearchDFS(string path, bool findall)
        {
            string[] files = null;
            string[] folders = null;
            try
            {
                // Exception handling untuk restricted folder
                files = Directory.GetFiles(path);
                folders = Directory.GetDirectories(path);
            }
            catch { }

            finally
            {

                if (folders.Length != 0)
                {
                    Array.Sort(folders);
                    int i = 0;
                    while (i < folders.Length && output.Contains(folders[i]))
                    {
                        i++;
                    }
                    
                    if (i != folders.Length) // Masih ada folder yang belum di visit
                    {
                        // tree.addChild(folders[i], tree.root, true);
                        stack.Push(folders[i]);
                        output.Add(folders[i]);
                        SearchDFS(stack.Peek(), findall);
                    }
                    else // Semua folder sudah visited
                    {
                        CheckFileInDirectory(files, findall);
                    }
                }
                else
                {
                    CheckFileInDirectory(files, findall);
                    if (!found)
                    {
                        if (stack.Count > 1)
                        {
                            stack.Pop();
                            SearchDFS(stack.Peek(), findall);
                        }
                    }
                }   
            }
        }  
        private void BuildTreeDFS(string start_path)
        {
            string[] files = null;
            string[] folders = null;
            try
            {
                // Exception handling untuk restricted folder
                files = Directory.GetFiles(start_path);
                folders = Directory.GetDirectories(start_path);
            }
            catch { }
            finally
            {
                if (folders.Length != 0)
                {
                    for (int i = 0; i < folders.Length; i++)
                    {
                        tree.addChild(folders[i], tree.root, true);
                        BuildTreeDFS(folders[i]);
                    }
                }
                if (files.Length != 0)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        tree.addChild(files[i], tree.root, true);
                    }
                }
            }
        }
        public bool isFound() // Apakah ketemu
        {
            return found;
        }
    }
}
