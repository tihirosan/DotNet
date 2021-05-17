using System;
using System.Collections.Generic;

namespace Tasks
{
    static class Program
    {
        private static void InsertionSort(IList<int> arr)
        {
            for (var i = 1; i < arr.Count; i++)
            {
                var current = arr[i];
                var j = i;
                
                while (j > 0 && arr[j - 1] > current)
                {
                    arr[j] = arr[j - 1];
                    j--;
                }

                arr[j] = current;
            }
        }
        
        static void Main(string[] args)
        {
            Console.WriteLine("LinkedList:");
            
            int[] arrList = {4, 5, 6};
            var linkedList = new LinkedList<int>(arrList);
            linkedList.Print();
            
            linkedList.AddLast(7);
            linkedList.AddLast(8);
            linkedList.AddLast(9);
            linkedList.Print();

            linkedList.AddFirst(3);
            linkedList.AddFirst(2);
            linkedList.AddFirst(1);
            linkedList.Print();
            
            linkedList.Remove(2);
            linkedList.Remove(5);
            linkedList.Remove(8);
            linkedList.Print();
            
            linkedList.Reverse();
            linkedList.Print();
            
            linkedList.RemoveFirst();
            linkedList.Print();

            linkedList.Clear();
            
            Console.WriteLine("BinaryTree:");
            
            int[] arrTree = {10, 5, 3, 2, 4, 7, 6, 8, 15, 13, 12, 14, 17, 18};
            var binaryTree = new BinaryTree(arrTree);
            binaryTree.InorderPrint();
            
            binaryTree.Add(16);
            binaryTree.InorderPrint();

            var treeNode = binaryTree.Find(17);
            Console.WriteLine(treeNode.Data);

            binaryTree.Remove(15);
            binaryTree.Remove(5);
            binaryTree.InorderPrint();
            
            Console.WriteLine("InsertionSort:");

            int[] arrSort = {7, 8, 9, 1, 2, 3, 4, 5, 6};
            InsertionSort(arrSort);
            foreach (var el in arrSort)
                Console.Write(el.ToString() + ' ');
            Console.WriteLine();
        }
    }
}