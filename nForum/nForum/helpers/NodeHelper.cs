using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.NodeFactory;

namespace nForum.helpers
{
    public class NodeHelper
    {
        public static List<Node> GetAllNodesByType(int nodeId, string typeName)
        {
            List<Node> result = new List<Node>();

            GetNodeList(nodeId, typeName, ref result);

            return result;
        }

        private static void GetNodeList(int nodeId, string typeName, ref List<Node> result)
        {
            var node = new Node(nodeId);
            foreach (Node childNode in node.Children)
            {
                var child = childNode;
                if (child.NodeTypeAlias == typeName)
                {
                    result.Add(child);
                }

                if (child.Children.Count > 0)
                {
                    GetNodeList(child.Id, typeName, ref result);
                }
            }   
        }

        public static Node GetParentNodeByType(int nodeId, string typeName)
        {
            if (nodeId <= 0)
            {
                return null;
            }
            else
            {
                return GetParentNode(nodeId, typeName);
            }
        }

        private static Node GetParentNode(int nodeId, string typeName)
        {
            var node = new Node(nodeId);
            Node parentNode = (Node)node.Parent;

            if (parentNode.NodeTypeAlias == typeName)
            {
                return parentNode;
            }
            else
            {
                return GetParentNode(parentNode.Id, typeName);
            }
        }
           
    }
}