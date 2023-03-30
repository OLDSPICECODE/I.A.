using Assets.script;
using System;
using System.Collections;

using System.Collections.Generic;
using UnityEngine;
//class Node
//{
//    public int val = 0;
//    public int[] coords = new int[2];
//    public int color = 0;
//    public GameObject cilinder = null;
//    public List<Node> edges = new List<Node>();
//    public Node(int v, int i, int j)
//    {
//        color = 0;
//        val = v;
//        coords[0] = i;
//        coords[1] = j;
//    }
//    public Node(int v, int i, int j, GameObject c)
//    {
//        cilinder = c;
//        color = 0;
//        val = v;
//        coords[0] = i;
//        coords[1] = j;
//    }
//    public bool deleteEdges(int f)
//    {
//        int i = 0;
//        foreach (Node n in edges)
//        {
//            if (f == n.val)
//            {
//                edges.RemoveAt(i);
//                return true;
//            }
//            i++;
//        }
//        return false;
//    }








//}
//class Graph
//{
//    public List<Node> nodes;

//    public void insertNode(int val, int x, int y)
//    {
//        nodes.Add(new Node(val, x, y));
//    }
//    public void insertNode(int val, int x, int y, GameObject cilindro)
//    {
//        nodes.Add(new Node(val, x, y, cilindro));
//    }
//    public static void insertEdge(Node a, Node b)
//    {
//        a.edges.Add(b);
//    }

//    public Node findNode(int pos)
//    {
//        //        for (auto i: nodes) {
//        //            if (i.val == val) {
//        //                return i;
//        //            }
//        //        }
//        //        return nullptr;
//        return nodes[pos];
//    }

//    //Métodos para el insertado masivo y borrar elementos aleatorios en porcentaje
//    public void insertNodeMassive(int n)
//    {
//        nodes = new List<Node>(n * n);
//        for (int i = 0; i < n; i++)
//        {
//            for (int j = 0; j < n; j++)
//            {
//                insertNode(i * n + j, i, j);
//            }
//        }
//    }
//    public void insertNodeMassive(int n, GameObject cilindro)
//    {
//        nodes = new List<Node>(n * n);
//        for (int i = 0; i < n; i++)
//        {
//            for (int j = 0; j < n; j++)
//            {
//                insertNode(i * n + j, i, j, cilindro);
//            }
//        }
//    }
//    public void insertEdgeMassive(int n)
//    {
//        for (int i = 0; i < n * n; i++)
//        {
//            if (nodes[i].coords[0] != 0 && nodes[i].coords[1] != 0)
//            {
//                insertEdge(nodes[i], nodes[i - n - 1]);
//            }

//            if (nodes[i].coords[0] != 0)
//            {
//                insertEdge(nodes[i], nodes[i - n]);
//            }

//            if (nodes[i].coords[0] != 0 && nodes[i].coords[1] != n - 1)
//            {
//                insertEdge(nodes[i], nodes[i - n + 1]);
//            }

//            if (nodes[i].coords[1] != n - 1)
//            {
//                insertEdge(nodes[i], nodes[i + 1]);
//            }

//            if (nodes[i].coords[0] != n - 1 && nodes[i].coords[1] != n - 1)
//            {
//                insertEdge(nodes[i], nodes[i + n + 1]);
//            }

//            if (nodes[i].coords[0] != n - 1)
//            {
//                insertEdge(nodes[i], nodes[i + n]);
//            }

//            if (nodes[i].coords[0] != n - 1 && nodes[i].coords[1] != 0)
//            {
//                insertEdge(nodes[i], nodes[i + n - 1]);

//            }

//            if (nodes[i].coords[1] != 0)
//            {
//                insertEdge(nodes[i], nodes[i - 1]);
//            }
//        }
//    }

//    public void randomDelete(int percentage)
//    {
//        System.Random random = new System.Random();
//        int n_delete = (int)nodes.Count * percentage / 100;
//        for (int i = 0; i < n_delete; i++)
//        {
//            int rand = random.Next(0, nodes.Count - 1);
//            //            cout << nodes[rand].val << endl;
//            foreach (Node edge in nodes[rand].edges)
//            {
//                edge.deleteEdges(nodes[rand].val);
//            }
//            nodes.RemoveAt(rand);
//        }
//    }

//    void resetColors()
//    {
//        foreach (Node i in nodes)
//        {
//            i.color = 0;
//        }
//    }

//    //Búsqueda ciega
//    public bool BFS(int ini, int fin, ref List<int> res)
//    {
//        Node n_ini = findNode(ini);
//        Node n_fin = findNode(fin);
//        Queue<Node> L = new Queue<Node>();
//        Stack<(int, int)> camino = new Stack<(int, int)>();
//        camino.Push((n_ini.val, n_ini.val));
//        L.Enqueue(n_ini);
//        n_ini.color = 1;


//        while (L.Count != 0)
//        {
//            Node n = L.Peek();
//            if (n == n_fin)
//            {
//                int pivot = n.val;
//                while (camino.Count != 0)
//                {
//                    if (camino.Peek().Item1 == pivot)
//                    {
//                        res.Add(camino.Peek().Item1);
//                        pivot = camino.Peek().Item2;
//                    }
//                    camino.Pop();
//                }
//                res.Reverse();
//                return true;
//            }
//            List<Node> nei = n.edges;
//            int padre = n.val;
//            L.Dequeue();
//            foreach (Node edge in nei)
//            {
//                if (edge.color == 0)
//                {
//                    L.Enqueue(edge);
//                    camino.Push((edge.val, padre));
//                    edge.color = 1;
//                }
//            }
//        }
//        return false;
//    }
//}




/*bool DFS(int ini, int fin, List<int> &res)
{
    Node n_ini = findNode(ini);
    Node n_fin = findNode(fin);
    stack<Node> L;
    stack<pair<int, int>> camino;
    camino.Add(make_pair(n_ini.val, n_ini.val));
    L.Add(n_ini);
    n_ini.color = 1;
    while (!L.empty())
    {
        Node n = L.Peek();
        if (n == n_fin)
        {
            int pivot = n.val;
            while (!camino.empty())
            {
                if (camino.Peek().Item1 == pivot)
                {
                    res.Add(camino.Peek().Item1);
                    pivot = camino.Peek().Item2;
                }
                camino.pop();
            }
            reverse(res.begin(), res.end());
            return true;
        }
        List<Node> nei = n.edges;
        int padre = n.val;
        L.pop();
        for (auto & edge: nei) {
    if (edge.color == 0)
    {
        L.Add(edge);
        camino.Add(make_pair(edge.val, padre));
        edge.color = 1;
    }
}
        }
        return false;
    }

    //Búsquedas Heuristicas
    bool MejorPrimero(int ini, int fin, List<int> &res)
{
    Node n_ini = findNode(ini);
    Node n_fin = findNode(fin);
    priority_queue<pair<Node, double>, List<pair<Node, double>>, CustomCompare> L;//Par nodo - distancia con el objetivo
    stack<pair<int, int>> camino;
    camino.Add(make_pair(n_ini.val, n_ini.val));
    L.Add(make_pair(n_ini, distanceNodes(n_ini, n_fin)));
    n_ini.color = 1;
    while (!L.empty())
    {
        Node n = L.Peek().Item1;
        if (n == n_fin)
        {
            int pivot = n.val;
            while (!camino.empty())
            {
                if (camino.Peek().Item1 == pivot)
                {
                    res.Add(camino.Peek().Item1);
                    pivot = camino.Peek().Item2;
                }
                camino.pop();
            }
            reverse(res.begin(), res.end());
            return true;
        }
        List<Node> edges_saves = n.edges;
        int padre = n.val;
        L.pop();
        for (auto & edge: edges_saves) {
    if (edge.color == 0)
    {
        L.Add(make_pair(edge, distanceNodes(edge, n_fin)));
        camino.Add(make_pair(edge.val, padre));
        edge.color = 1;
    }
}
        }
        return false;
    }

};

*/
public class PFScreept : MonoBehaviour
{
    
    // Start is called before the Item1 frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
    