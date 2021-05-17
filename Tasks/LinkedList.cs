using System;
using System.Collections;
using System.Collections.Generic;

namespace Tasks
{
    public class ListNode<T>
    {
        public ListNode(T data)
        {
            Data = data;
            Next = null;
        }
        public T Data { get; }
        public ListNode<T> Next { get; set; }
    }

    public class LinkedList<T> : IEnumerable<T>
    {
        private ListNode<T> First { get; set; }
        private ListNode<T> Last { get; set; }

        public LinkedList(IEnumerable<T> array)
        {
            foreach (var el in array)
            {
                AddLast(el);
            }
        }

        public void AddLast(T data)
        {
            var node = new ListNode<T>(data);

            if (First == null)
            {
                First = node;
                Last = node;
            }
            else
            {
                Last.Next = node;
                Last = node;
            }
        }
        
        public void AddFirst(T data)
        {
            var node = new ListNode<T>(data);
            
            if (First == null)
            {
                First = node;
                Last = node;
            }
            else
            {
                node.Next = First;
                First = node;
            }
        }

        public void RemoveFirst()
        {
            if (First == null) return;
            
            First = First.Next;
            if (First == null)
                Last = null;
        }

        public void RemoveLast()
        {
            if (First == null) return;

            if (First.Next == null)
            {
                Clear();
                return;
            }
            
            var cur = First;
            ListNode<T> prev = null;

            while (cur.Next != null)
            {
                prev = cur;
                cur = cur.Next;
            }

            Last = prev;
        }
        
        public bool Remove(T data)
        {
            var cur = First;
            ListNode<T> prev = null;
            
            while (cur != null)
            {
                if (cur.Data.Equals(data))
                {
                    if (prev == null)
                    {
                        RemoveFirst();
                    }
                    else
                    {
                        prev.Next = cur.Next;
                        if (cur.Next == null)
                            Last = prev;
                    }
                    return true;
                }
                prev = cur;
                cur = cur.Next;
            }
            return false;
        }
        
        public void Reverse()
        {
            var cur = First;
            ListNode<T> prev = null;

            while (cur != null)
            {
                var next = cur.Next;
                cur.Next = prev;
                prev = cur;
                cur = next;
            }

            Last = First;
            First = prev;
        }

        public void Clear()
        {
            First = null;
            Last = null;
        }

        public void Print()
        {
            foreach (var el in this)
                Console.Write(el + " ");
            Console.WriteLine();
        }

        public IEnumerator<T> GetEnumerator()
        {
            var cur = First;
            while (cur != null)
            {
                yield return cur.Data;
                cur = cur.Next;
            }
        }

        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this as IEnumerable).GetEnumerator();
        }
    }
}