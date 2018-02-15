using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using PMML;

namespace DecisionTree
{
    
    class TreeModel
    {   
        //this dictionary contains ID of each node and its Node object
        Dictionary<int, TreeNode> nodesDic = new Dictionary<int, TreeNode>();
        Fields fields;
        

        public TreeModel(string dir)
        {
            Pmml doc = new Pmml(dir);
            NodeList nodes = doc.getList("Node");
            fields = new Fields(doc.getList("DataField"));

            for (int i = 0; i < nodes.Size(); i++)
            {
                TreeNode n = new TreeNode(nodes.getNode(i));
                int id = Convert.ToInt16(nodes.getNode(i).Attribute("id"));
                n.id = id;
                n.cID = getChildren(nodes.getNode(i));
                n.type = fields.type;
                nodesDic.Add(id, n);
           }

}
        //gets the id of each node. its an integer array. with this array cID of Node object will be initialized
        int[] getChildren(Node xNode)
        {
            List<int> tmp = new List<int>();
            int[] children = null;

            for (int i = 0; i < xNode.ChildCount("Node"); i++)
            { 
                    tmp.Add(Convert.ToInt16(xNode.Child(i, "Node").Attribute("id")));
            }

            children = tmp.ToArray();
            return children;
        }

        public string Predict(Dictionary<string, object> inputDic)
        {
            string result = null;
            Stack<TreeNode> nStack= new Stack<TreeNode>();
            TreeNode tmpNode = null;

            int min;
            var keys = nodesDic.Keys;
            min = keys.Min();
            
            nStack.Push(nodesDic[min]); //get the root
            
            while (true)
            {
                if (nStack.Peek().Predicate(inputDic))
                {
                    if (nStack.Peek().cID.GetLength(0) == 0) //So it is Leaf
                    {
                        result = nStack.Peek().score;

                        break;
                    }
                    else //if this is not leaf 
                    {
                        tmpNode = nStack.Pop();
                        for (int i = (tmpNode.cID.GetLength(0)) - 1; i >= 0; i--)
                        {
                            nStack.Push(nodesDic[tmpNode.cID[i]]);
                        }
                    }
                }
                else
                    nStack.Pop();

            } //end of while(true)

            return result;
        }  // end of the method

    }
}
