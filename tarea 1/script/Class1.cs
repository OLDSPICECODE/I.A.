using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.script
{
    public class Node
    {
        public bool ignorado = false;
        public int val = 0;
        public int[] coords = new int[2];
        public int color = 0;
        public GameObject cilinder = null;
        public List<Node> edges = new List<Node>();
        public Node()
        {

        }
        public Node(int v, int i, int j)
        {
            color = 0;
            val = v;
            coords[0] = i;
            coords[1] = j;
        }
        public Node(int v, int i, int j, GameObject c)
        {
            cilinder = c;
            color = 0;
            val = v;
            coords[0] = i;
            coords[1] = j;
        }
        public bool deleteEdges(int f)
        {
            int i = 0;
            foreach (Node n in edges)
            {
                if (f == n.val)
                {
                    edges.RemoveAt(i);
                    return true;
                }
                i++;
            }
            return false;
        }
        public double distanceNodes(Node a, Node b)
        {
            double distancia = Math.Sqrt((Math.Pow(a.coords[0] - b.coords[0], 2) + Math.Pow(a.coords[1] - b.coords[1], 2)));
            return distancia;
        }
        
    }


    public class PriorityQueue
    {
        private List<(Node,double)> list = new List<(Node, double)>();
        public int Count { get { return list.Count; } }
        public readonly bool IsDescending;
        int comparar((Node, double) x, (Node, double) y)
        {
            return (x.Item2 > y.Item2)?1:-1;
        }
        public PriorityQueue()
        {
            list = new List <(Node,double )>();
        }

        public PriorityQueue(bool isdesc)
            : this()
        {
            IsDescending = isdesc;
        }

        public PriorityQueue(int capacity)
            : this(capacity, false)
        { }

        //public PriorityQueue(IEnumerable <(Node,double )> collection)
        //    : this(collection, false)
        //{ }

        public PriorityQueue(int capacity, bool isdesc)
        {
            list = new List <(Node,double )>(capacity);
            IsDescending = isdesc;
        }

        //public PriorityQueue(IEnumerable <(Node,double )> collection, bool isdesc)
        //    : this()
        //{
        //    IsDescending = isdesc;
        //    foreach (var item in collection)
        //        Enqueue(item);
        //}


        public void Enqueue((Node, double) x)
        {
            list.Add(x);
            int i = Count - 1;

            while (i > 0)
            {
                int p = (i - 1) / 2;
                if ((IsDescending ? -1 : 1) * comparar(list[p],(x)) <= 0) break;

                list[i] = list[p];
                i = p;
            }

            if (Count > 0) list[i] = x;
        }

        public (Node,double )Dequeue()
        {
            (Node,double )target = Peek();
            (Node,double )root = list[Count - 1];
            list.RemoveAt(Count - 1);

            int i = 0;
            while (i * 2 + 1 < Count)
            {
                int a = i * 2 + 1;
                int b = i * 2 + 2;
                int c = b < Count && (IsDescending ? -1 : 1) *comparar( list[b],list[a]) < 0 ? b : a;

                if ((IsDescending ? -1 : 1) * comparar(list[c],root) >= 0) break;
                list[i] = list[c];
                i = c;
            }

            if (Count > 0) list[i] = root;
            return target;
        }

        public (Node,double )Peek()
        {
            if (Count == 0) throw new InvalidOperationException("Queue is empty.");
            return list[0];
        }

        public void Clear()
        {
            list.Clear();
        }
    }

};
