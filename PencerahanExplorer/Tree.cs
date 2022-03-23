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
            string[] pathList = path.Split((char)92);
            this.name = pathList[pathList.Length - 1];
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
            //create the graph content, along with tree building
        }

        public void addChild(string child_full_path, Node current_path, bool isfolder)
        {
            string relative_path = getRelativePath(current_path.getPath(), child_full_path);
            string[] child_path_list = relative_path.Split((char)92);
            /*
            Console.WriteLine("Child path : " + child_full_path);
            Console.WriteLine("Current path : " + current_path.getPath());
            Console.WriteLine("Relative path : " + relative_path);
            Console.WriteLine("Root Name : " + child_path_list[0]);
            */
            
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
            graph.FindNode(current_path.getName()).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Blue;
            /*
            if (child_path_list.Length == 1)
            {
                // basis
                // sampai pada leaf node
                current_path.addChild(child_full_path, isfolder);
            }
            else
            {
                // rekurens
                if (current_path.getChild(child_path_list[0]) == null)
                {
                    // tidak ada child dengan nama yang sama pada Node, buat child baru
                    current_path.addChild(child_full_path, isfolder);
                    // lanjutkan ke node berikutnya
                }
                addChild(child_full_path, current_path.getChild(child_path_list[0]), isfolder);
            } */
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
                foreach(Node c in n.getChildList())
                {
                    display(c);
                }
            }
        }

        public string getRelativePath(string relativeTo, string path)
        {
            string relative_path = path.Remove(0, relativeTo.Length + 1);
            return relative_path;
        }

        public void displayTree()
        {
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
