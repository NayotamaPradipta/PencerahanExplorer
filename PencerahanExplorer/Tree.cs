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
        private Node parent;
        public bool isFolder;

        public Node(string path, bool isFolder)
        {
            this.full_path = path;
            string[] pathList = path.Split((char)92);
            this.name = pathList[pathList.Length - 1];
            child = new List<Node>();
            this.isFolder = isFolder;
            this.parent = null;
        }

        public string getName()
        {
            return name;
        }

        public string getPath()
        {
            return full_path;
        }

        public void setParent(Node p)
        {
            parent = p;
        }

        public Node getParent()
        {
            return parent;
        }

        public void addChild(string child_path, bool isFolder)
        {
            Node c = new Node(child_path, isFolder);
            c.setParent(this);
            child.Add(c);
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
        public List<Node> getChildList()
        {
            return child;
        }


    }
    public class Tree
    {
        public Node root;
        private Microsoft.Msagl.GraphViewerGdi.GViewer viewer;
        public Microsoft.Msagl.Drawing.Graph graph;

        // misalkan untuk node yang sudah diperiksa diwarnai merah
        // dan untuk yang sudah masuk ke antrian diwarnai biru

        public Tree(string root)
        {
            this.root = new Node(root, true);
            //create a viewer object
            viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            //create a graph object
            graph = new Microsoft.Msagl.Drawing.Graph("graph");
            graph.AddNode(this.root.getName());
            //create the graph content, along with tree building
        }

        public void addChild(string child_full_path, Node current_path, bool isfolder)
        {
            string relative_path = getRelativePath(current_path.getPath(), child_full_path);
            if (relative_path != null )
            {
                string[] child_path_list = relative_path.Split((char)92);

                if (current_path.getChild(child_path_list[0]) == null)
                {
                    // basis
                    // tidak ada child dengan nama yang sama pada Node, buat child baru
                    current_path.addChild(child_full_path, isfolder);
                    graph.AddEdge(current_path.getName(), child_path_list[0]);
                }
                else
                {
                    // rekurens
                    // terdapat child dengan nama yang sama, pindah ke node tersebut
                    addChild(child_full_path, current_path.getChild(child_path_list[0]), isfolder);
                }
            }
        }

        public Node findNode(string path, Node n)
        {
            Node res = null;
            if (n.getPath() == path)
            {
                return n;
            }
            else
            {
                foreach(Node c in n.getChildList())
                {
                    res = findNode(path, c);
                    if (res != null)
                    {
                        return res;
                    }
                }
                return res;
            }
        }

        public string getRelativePath(string relativeTo, string path)
        {
            if (relativeTo.Length == path.Length)
            {
                return null;
            }
            else
            {
                string relative_path = path.Remove(0, relativeTo.Length + 1);
                return relative_path;
            }
        }

        public void giveColor(string node_name, string color)
        {
            if (color == "Blue")
            {
                graph.FindNode(node_name).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Blue;
            }
            else if (color == "Red")
            {
                graph.FindNode(node_name).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Red;
            }
        }

        public void updateParentColor(string path)
        {
            // mengupdate warna dari semua parent
            Node p = findNode(path, root);
            while (p != null)
            {
                giveColor(p.getName(), "Blue");
                p = p.getParent();
            }
        }

        public void displayTree(List<string> visited, List<string> pathList)
        {
            // color every visited 

            foreach (string s in visited)
            {
                giveColor(Path.GetFileName(s), "Red");
            }

            foreach (string c in pathList)
            {
                updateParentColor(c);
            }

            //create a form
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            viewer.FitGraphBoundingBox();
            //bind the graph to the viewer
            viewer.Graph = graph;
            //associate the viewer with the form
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.ResumeLayout();
            //show the form
            System.Windows.Forms.Application.Run(form);
        }

    }
}
