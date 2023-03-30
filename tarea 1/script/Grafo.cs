using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.script
{
    
    public class Graph
    {
        
        public List<Node> nodes=null;
        Node helper= new Node();
        public void insertNode(int val, int x, int y)
        {
            nodes.Add(new Node(val, x, y));
        }
        public void insertNode(int val, int x, int y, GameObject cilindro)
        {
            nodes.Add(new Node(val, x, y, cilindro));
        }
        public static void insertEdge(Node a, Node b)
        {
            a.edges.Add(b);
        }

        public Node findNode(int pos)
        {
            //foreach (Node i in nodes)
            //{
            //    if (i.val == pos)
            //    {
            //        return i;
            //    }
            //}
            //return null;
            if (nodes.Count!=0 && nodes[pos].ignorado) return null;
            return nodes[pos];
        }

        //Métodos para el insertado masivo y borrar elementos aleatorios en porcentaje
        public void insertNodeMassive(int n)
        {
            nodes = new List<Node>(n * n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    insertNode(i * n + j, i, j);
                }
            }
        }
        public void insertNodeMassive(int n, GameObject cilindro)
        {
            nodes = new List<Node>(n * n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    insertNode(i * n + j, i, j, cilindro);
                }
            }
        }
        public void insertEdgeMassive(int n)
        {
            for (int i = 0; i < n * n; i++)
            {
                if (nodes[i].coords[0] != 0 && nodes[i].coords[1] != 0)
                {
                    insertEdge(nodes[i], nodes[i - n - 1]);
                }

                if (nodes[i].coords[0] != 0)
                {
                    insertEdge(nodes[i], nodes[i - n]);
                }

                if (nodes[i].coords[0] != 0 && nodes[i].coords[1] != n - 1)
                {
                    insertEdge(nodes[i], nodes[i - n + 1]);
                }

                if (nodes[i].coords[1] != n - 1)
                {
                    insertEdge(nodes[i], nodes[i + 1]);
                }

                if (nodes[i].coords[0] != n - 1 && nodes[i].coords[1] != n - 1)
                {
                    insertEdge(nodes[i], nodes[i + n + 1]);
                }

                if (nodes[i].coords[0] != n - 1)
                {
                    insertEdge(nodes[i], nodes[i + n]);
                }

                if (nodes[i].coords[0] != n - 1 && nodes[i].coords[1] != 0)
                {
                    insertEdge(nodes[i], nodes[i + n - 1]);

                }

                if (nodes[i].coords[1] != 0)
                {
                    insertEdge(nodes[i], nodes[i - 1]);
                }
            }
        }

        public void randomDelete(int percentage)
        {
            System.Random random = new System.Random();
            int n_delete = (int)nodes.Count * percentage / 100;
            for (int i = 0; i < n_delete; i++)
            {
                int rand = random.Next(0, nodes.Count - 1);
                //            cout << nodes[rand].val << endl;
                //foreach (Node edge in nodes[rand].edges)
                //{
                    
                //    edge.deleteEdges(nodes[rand].val);
                //}
                nodes[rand].cilinder.SetActive(false);
                nodes[rand].ignorado=true;
            }
        }

        public void resetColors(Material blanco)
        {
            foreach ( Node i in nodes)
            {
                i.color = 0;
                i.cilinder.GetComponent<MeshRenderer>().material = blanco;
            }
        }

        //Búsqueda ciega
        public bool BFS(int ini, int fin, ref List<int> res,ref List<int> reco, ref Text E1, ref Text E2)
        {
            Node n_ini = findNode(ini);
            Node n_fin = findNode(fin);
            if (n_ini == null)
            {
                E1.text = "no hay inicio";
                return false;
            }
            if (n_fin == null)
            {
                E2.text = "no hay fin";
                return false;
            }


            Queue<Node> L = new Queue<Node>();
            Stack<(int, int)> camino = new Stack<(int, int)>();
            camino.Push((n_ini.val, n_ini.val));
            L.Enqueue(n_ini);
            n_ini.color = 1;
            //n_ini.cilinder.GetComponent<MeshRenderer>().material.color = new Color(255, 255, 0);


            while (L.Count != 0)
            {
                Node n = L.Peek();
                reco.Add(n.val);
                if (n == n_fin)
                {
                    int pivot = n.val;
                    while (camino.Count != 0)
                    {
                        if (camino.Peek().Item1 == pivot)
                        {
                            
                            res.Add(camino.Peek().Item1);
                            pivot = camino.Peek().Item2;
                        }
                        camino.Pop();
                    }
                    res.Reverse();
                    return true;
                }
                
                List<Node> nei = n.edges;
                int padre = n.val;

                L.Dequeue();

                foreach (Node edge in nei)
                {
                    if (!edge.ignorado &&  edge.color == 0)
                    {
                        L.Enqueue(edge);
                        camino.Push((edge.val, padre));
                        edge.color = 1;
                    }
                }
            }
            return false;
        }

        public bool DFS(int ini, int fin, ref List<int>  res,ref List<int> reco, ref Text E1, ref Text E2)
        {
            Node n_ini = findNode(ini);
            Node n_fin = findNode(fin);

            if (n_ini == null)
            {
                E1.text = "no hay inicio";
                return false;
            }
            if (n_fin == null)
            {
                E2.text = "no hay fin";
                return false;
            }

            Stack<Node> L= new Stack<Node>();
            Stack<(int, int)> camino=new Stack<(int, int)>();
            camino.Push((n_ini.val, n_ini.val));
            L.Push(n_ini);
            n_ini.color = 1;
            while (L.Count!=0)
            {
                Node n = L.Peek();
                reco.Add(n.val);
                if (n == n_fin)
                {
                    int pivot = n.val;
                    while (camino.Count!=0)
                    {
                        if (camino.Peek().Item1 == pivot)
                        {
                            
                            res.Add(camino.Peek().Item1);
                            pivot = camino.Peek().Item2;
                        }
                        camino.Pop();
                    }
                    res.Reverse();
                    return true;
                }
                List<Node> nei = n.edges;
                int padre = n.val;
                L.Pop();
                foreach (Node edge in nei)
                {
                    if (!edge.ignorado && edge.color == 0)
                    {
                        L.Push(edge);
                        camino.Push((edge.val, padre));
                        edge.color = 1;
                    }
                }
            }
            return false;
        }

        //Búsquedas Heuristicas

        public bool hillClimbing(int ini, int fin,ref List<int> res,ref List<int> reco,ref Text E1,ref Text E2 )
        {
            Node n_ini = findNode(ini);
            Node n_fin = findNode(fin);
            if (n_ini == null)
            {
                E1.text="no hay inicio";
                return false;
            }
            if (n_fin == null)
            {
                E2.text="no hay fin";
                return false;
            }

            Stack<Node> L = new Stack<Node>();
            Stack<(int, int)> camino = new Stack<(int, int)>();
            camino.Push((n_ini.val, n_ini.val));
            L.Push(n_ini);
            n_ini.color = 1;
            while (L.Count != 0)
            {
                Node n = L.Peek();
                reco.Add(n.val);
                if (n == n_fin)
                {
                    int pivot = n.val;
                    while (camino.Count != 0)
                    {
                        if (camino.Peek().Item1 == pivot)
                        {

                            res.Add(camino.Peek().Item1);
                            pivot = camino.Peek().Item2;
                        }
                        camino.Pop();
                    }
                    res.Reverse();
                    return true;
                }
                List<Node> nei= new List<Node>( n.edges);
                for (int i = 0; i < nei.Count; i++)
                {
                    for (int j = i + 1; j < nei.Count; j++)
                    {
                        if (helper.distanceNodes(nei[i], n_fin) < helper.distanceNodes(nei[j], n_fin))
                        {
                            Node c = nei[j];
                            nei[j] = nei[i];
                            nei[i] = c;
                        }
                    }
                }
                int padre = n.val;
                L.Pop();
                foreach (Node edge in nei)
                {
                    if (!edge.ignorado && edge.color == 0)
                    {
                        L.Push(edge);
                        camino.Push((edge.val, padre));
                        edge.color = 1;
                    }
                }
            }
            return false;
        }
        
        public bool MejorPrimero(int ini, int fin,ref List<int> res, ref List<int> reco, ref Text E1, ref Text E2)
        {
            Node n_ini = findNode(ini);
            Node n_fin = findNode(fin);
            if (n_ini == null)
            {
                E1.text = "no hay inicio";
                return false;
            }
            if (n_fin == null)
            {
                E2.text = "no hay fin";
                return false;
            }
            PriorityQueue L = new PriorityQueue(1);
            Stack<(int, int)> camino= new Stack<(int, int)>();
            camino.Push((n_ini.val, n_ini.val));
            L.Enqueue((n_ini, helper.distanceNodes(n_ini, n_fin)));
            n_ini.color = 1;
            while (L.Count!=0)
            {
                Node n = L.Peek().Item1;
                reco.Add(n.val);
                if (n == n_fin)
                {
                    int pivot = n.val;
                    while (camino.Count!=0)
                    {
                        if (camino.Peek().Item1 == pivot)
                        {
                            res.Add(camino.Peek().Item1);
                            pivot = camino.Peek().Item2;
                        }
                        camino.Pop();
                    }
                    res.Reverse();
                    return true;
                }
                List<Node> edges_saves = n.edges;
                int padre = n.val;
                L.Dequeue();
                foreach (Node  edge in edges_saves)
                {
                    if (!edge.ignorado && edge.color == 0)
                    {
                        L.Enqueue((edge, helper.distanceNodes(edge, n_fin)));
                        camino.Push((edge.val, padre));
                        edge.color = 1;
                    }
                }
            }
            return false;
        }
        

    }
}
