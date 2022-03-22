using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
