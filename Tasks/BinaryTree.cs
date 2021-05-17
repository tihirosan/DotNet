using System;
using System.Collections.Generic;

namespace Tasks
{
    public class TreeNode
    {
        public int Data { get; set; }
        public TreeNode Left { get; set; }
        public TreeNode Right { get; set; }
        public TreeNode Parent { get; set; }
    }

    public class BinaryTree
    {
        private TreeNode Root { get; set; }

        public BinaryTree()
        {
            Root = null;
        }
        
        public BinaryTree(IEnumerable<int> array)
        {
            foreach (var el in array)
            {
                Add(el);
            }
        }

        public void Add(int data)
        {
            if (Root == null)
                Root = new TreeNode {Data = data, Left = null, Right = null, Parent = null};
            else
            {
                var treeNode = Root;
                while (true)
                {
                    if (data < treeNode.Data)
                    {
                        if (treeNode.Left != null)
                        {
                            treeNode = treeNode.Left;
                            continue;
                        }
                    
                        treeNode.Left = new TreeNode {Data = data, Left = null, Right = null, Parent = treeNode};
                    }
                    else
                    {
                        if (treeNode.Right != null)
                        {
                            treeNode = treeNode.Right;
                            continue;
                        }
                    
                        treeNode.Right = new TreeNode {Data = data, Right = null, Left = null, Parent = treeNode};
                    }

                    break;
                }
            }
        }

        public TreeNode Find(int data)
        {
            var treeNode = Root;
            while (true)
            {
                if (treeNode != null)
                {
                    if (data == treeNode.Data)
                        return treeNode;

                    treeNode = data < treeNode.Data ? treeNode.Left : treeNode.Right;
                }
                else
                {
                    return null;
                }
            }
        }

        public bool Remove(int data)
        {
            var treeNode = Find(data);

            if (treeNode == null) return false;

            if (treeNode.Left == null && treeNode.Right == null)
            {
                if (treeNode.Parent.Left == treeNode)
                    treeNode.Parent.Left = null;
                else
                    treeNode.Parent.Right = null;
                return true;
            }

            if (treeNode.Left == null)
            {
                if (treeNode.Parent.Left == treeNode)
                    treeNode.Parent.Left = treeNode.Right;
                else
                    treeNode.Parent.Right = treeNode.Right;
                return true;
            }
            
            if (treeNode.Right == null)
            {
                if (treeNode.Parent.Left == treeNode)
                    treeNode.Parent.Left = treeNode.Left;
                else
                    treeNode.Parent.Right = treeNode.Left;
                return true;
            }
            
            var minDataNode = treeNode.Right;
            while (minDataNode.Left != null)
                minDataNode = minDataNode.Left;

            treeNode.Data = minDataNode.Data;
            if (minDataNode.Right != null)
            {
                if (minDataNode.Parent.Left.Data == minDataNode.Data)
                    minDataNode.Parent.Left = minDataNode;
                else
                    minDataNode.Parent.Right = minDataNode;
            }
            else if (minDataNode.Parent.Left.Data == minDataNode.Data)
                minDataNode.Parent.Left = null;
            else
                minDataNode.Parent.Right = null;

            return true;
        }

        public void InorderPrint()
        {
            _inorderPrint(Root);
            Console.WriteLine();
        }

        private static void _inorderPrint(TreeNode treeNode)
        {
            if (treeNode == null) return;
            _inorderPrint(treeNode.Left);
            Console.Write(treeNode.Data.ToString() + ' ');
            _inorderPrint(treeNode.Right);
        }
    }
}