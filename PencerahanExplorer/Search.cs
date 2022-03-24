﻿using System;
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
        // belum bikin tree

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
            bool foundHere = false;
            tree.giveColor(Path.GetFileName(path), "Blue");
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
                                foundHere = true;
                                pathList.Add(files[i]);
                                tree.giveColor(Path.GetFileName(files[i]), "Blue");

                                //Console.WriteLine(tree.getRelativePath(start_path, files[i]));
                                //Console.WriteLine(Directory.GetParent(files[i]));
                                //Console.WriteLine(Path.GetPathRoot(files[i]) + ' ' + Path.GetPathRoot(files[i]).Length);
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
                                foundHere = true;
                                pathList.Add(files[i]);
                                tree.graph.FindNode(Path.GetFileName(files[i])).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Blue;

                                //Console.WriteLine(tree.getRelativePath(start_path, files[i]));
                                //Console.WriteLine(Directory.GetParent(files[i]));
                                //Console.WriteLine(Path.GetPathRoot(files[i]) + ' ' + Path.GetPathRoot(files[i]).Length);
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
                        //Console.WriteLine(tree.getRelativePath(start_path, folders[k]));
                        tree.addChild(folders[k], tree.root, true);
                    }
                }
                if (!foundHere)
                {
                    tree.giveColor(Path.GetFileName(path), "Red");
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
        public List<string> pathList; // sementara public
        private Stack<string> stack; // untuk keep track current path
        private List<string> output; // untuk keep track visited path

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

            // Loop DFS utama
            if (findall)
            {
                while (stack.Count > 0)
                {
                    string top = stack.Pop();
                    SearchDFS(top, findall);
                }
            }
            else
            {
                while (!found && stack.Count > 0)
                {
                    string top = stack.Pop();
                    SearchDFS(top, findall);
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
                if (files != null)
                {
                    // Iterate over files in current path
                    int i;
                    if (findall)
                    {
                        for (i = 0; i < files.Length; i++)
                        {
                            if (Path.GetFileName(files[i]) == target_name)
                            {
                                found = true;
                                if (!pathList.Contains(files[i]))
                                {
                                    pathList.Add(files[i]);
                                }
                            }
                        }

                    }
                    else // cari satu file
                    {
                        i = 0;
                        while (!found && i < files.Length)
                        {
                            if (Path.GetFileName(files[i]) == target_name)
                            {
                                found = true;
                                pathList.Add(files[i]); // Pencarian selesai
                            }
                            else
                            {
                                i++;
                            }
                        }
                    }


                    // File not found in current path, lanjut ke path selanjutnya
                    if (!found || folders != null)
                    {
                        
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
                                SearchDFS(folders[k], findall);
                            }
                        }
                    }
                }
            }
        }

        public bool isFound() // Apakah ketemu
        {
            return found;
        }

        public List<string> get_target_path() // Ketemunya di path mana
        {
            return pathList;
        }


    }
}
