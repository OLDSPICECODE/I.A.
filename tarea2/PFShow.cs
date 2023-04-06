using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Assets.script;
using UnityEngine.UI;

public class PFShow : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera camara;

    
    public GameObject losa;
    public List<Material> materiales;
    public Material done;
    public Material originMat;

    private Graph grafo= new Graph();
    private Vector3 ini,fin, cam1, cam2;
    List<int> res = new List<int>();

    public int ni=0, nf=0;

    public Text E1, E2, E3;

    List<int> reco = new List<int>();

    Color colorr = new Color(0,0,40);
    int tam = 0;
    public InputField t1, t2, t3;
    public Text t4;
    public void createGrafo()
    {
        if (grafo.nodes != null)
        {
            foreach(Node g in grafo.nodes)
            {
                Destroy(g.cilinder);
            }
        }
        grafo = new Graph();
        tam =int.Parse(t3.text);
        grafo.insertNodeMassive(tam);
        grafo.insertEdgeMassive(tam);

        int cont = 0;
        
        for (int i = 0; i < grafo.nodes.Count; i++)
        {
            grafo.nodes[i].cilinder = Instantiate(losa, new Vector3(grafo.nodes[i].coords[0], 0f, grafo.nodes[i].coords[1]), new Quaternion(0, 0, 0, 0));
            grafo.nodes[i].cilinder.name = i.ToString();
            cont++;
        }


        Vector3 pos=grafo.nodes[grafo.nodes.Count - 1].cilinder.transform.position - grafo.nodes[0].cilinder.transform.position;
        camara.orthographicSize = (tam / 2) + 3;
        camara.transform.position = new Vector3(tam / 2, 10,tam / 2);
    }
    void resetMensajes()
    {
        E1.text = "";
        E2.text = "";
        E3.text = "";
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (grafo.nodes != null)
        {
            grafo.nodes[ni].cilinder.GetComponent<MeshRenderer>().material = materiales[3];
            grafo.nodes[nf].cilinder.GetComponent<MeshRenderer>().material = materiales[2];
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //Select stage    
                grafo.nodes[ni].cilinder.GetComponent<MeshRenderer>().material = originMat;
                hit.collider.gameObject.GetComponent<MeshRenderer>().material = materiales[3];
                ni = Int32.Parse(hit.transform.name);
                Debug.Log(hit.transform.name);
                // hit.GetComponent<MeshRenderer>().material = materiales[1];
            }
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //Select stage    
                grafo.nodes[nf].cilinder.GetComponent<MeshRenderer>().material = originMat;
                hit.collider.gameObject.GetComponent<MeshRenderer>().material = materiales[2];
                nf = Int32.Parse(hit.transform.name);
                Debug.Log(hit.transform.name);
                // hit.GetComponent<MeshRenderer>().material = materiales[1];
            }
        }
        //control de camara
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            camara.orthographicSize /= 2;
            if (Input.GetKey(KeyCode.A))
            {
                camara.orthographicSize = tam / 2 + 3;
                camara.transform.position = new Vector3(tam * 2, 10, 0);
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            camara.orthographicSize *= 2;
            if (Input.GetKey(KeyCode.A))
            {
                camara.orthographicSize = tam / 2 + 3;
                camara.transform.position = new Vector3(tam / 2, 10, 0);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            cam1 = camara.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            var position = camara.transform.position;
            fin = camara.ScreenToWorldPoint(Input.mousePosition);
            position += -(fin - cam1);
            camara.transform.position = position;
        }



        //ejecución algoritmos
        if (Input.GetKeyUp(KeyCode.A))
        {
            E1.text = "";
            E2.text = "";
            E3.text = "";
            t4.text = "BFS";
            grafo.resetColors(originMat);
            res.Clear();
            reco.Clear();
            
            if(!grafo.BFS(ni, nf, ref res,ref reco, ref E1, ref E2)) E3.text = "No se encontró camino";
            else StartCoroutine(printreco());
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            E1.text = "";
            E2.text = "";
            E3.text = "";
            t4.text = "A*";
            grafo.resetColors(originMat);
            res.Clear();
            reco.Clear();

            if (!grafo.Astar(ni, nf, ref res, ref reco, ref E1, ref E2)) E3.text = "No se encontró camino";
            else StartCoroutine(printreco());
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            E1.text = "";
            E2.text = "";
            E3.text = "";
            t4.text = "DFS";
            grafo.resetColors(originMat);
            res.Clear();
            reco.Clear();
            if (!grafo.DFS(ni, nf, ref res, ref reco,ref E1,ref E2)) E3.text = "No se encontró camino";
            else StartCoroutine(printreco());
        }
        if (Input.GetKeyUp(KeyCode.H))
        {
            E1.text = "";
            E2.text = "";
            E3.text = "";
            t4.text = "Hill Climbing";
            grafo.resetColors(originMat);
            res.Clear();
            reco.Clear();
            if (!grafo.hillClimbing(ni, nf, ref res, ref reco, ref E1, ref E2)) E3.text = "No se encontró camino";
            else StartCoroutine(printreco());
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            E1.text = "";
            E2.text = "";
            E3.text = "";
            t4.text = "Mejor Primero";
            grafo.resetColors(originMat);
            res.Clear();
            reco.Clear();
            if (!grafo.MejorPrimero(ni, nf, ref res, ref reco, ref E1, ref E2)) E3.text="No se encontró camino";
            else StartCoroutine(printreco());
        }


        if (Input.GetKeyUp(KeyCode.D))
        {
            grafo.randomDelete(30);
            Debug.Log(grafo.nodes[0].edges.Count);
           
        }


        if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.A))
        {
                camara.orthographicSize = (tam / 2) + 3;
                camara.transform.position = new Vector3(tam / 2, 10, tam / 3);
        }
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (Input.GetKey(KeyCode.Mouse0))
        //    {
        //        // Called on the first update where the user has pressed the mouse button.
        //        if (Input.GetKeyDown(KeyCode.Mouse0))
        //            ini = camara.ScreenToWorldPoint(Input.mousePosition);
                    
        //    }
        //}
        //if (Input.GetMouseButtonUp(0))
        //{
        //    fin = camara.ScreenToWorldPoint(Input.mousePosition);
        //    if (ini - fin != Vector3.zero)
        //    {
        //        camara.transform.position = (ini + fin) / 2;
        //        float a = (Math.Abs(ini.x - fin.x )> Math.Abs(ini.z-fin.z))? Math.Abs(ini.x - fin.x) : Math.Abs(ini.z - fin.z);
        //        camara.orthographicSize = a / 2;
        //    }
            
        //}
    }
    IEnumerator printreco()
    {
        Debug.Log("coloring..");
        int j = 0;
        foreach (int val in reco)
        {
            
            grafo.nodes[val].cilinder.GetComponent<MeshRenderer>().material = materiales[1];
            
            yield return new WaitForSecondsRealtime(0.01f);

        }

        if (res.Count > 0)
        {
            foreach (int val in res)
            {
                Node aux = grafo.findNode(val);
                if (aux != null) aux.cilinder.GetComponent<MeshRenderer>().material = done;
                yield return new WaitForSecondsRealtime(0.001f);
            }
        }
    }
    public void setvalues()
    {
        ni = int.Parse(t1.text);
        nf = int.Parse(t2.text);
    }
}
