using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorDrawer : MonoBehaviour
{

    private GameObject PfeilspitzenPath, PreviewPfeilspitze, PrefabPfeilspitze;
    private Material VectorLine;

    private void Awake()
    {
        PrefabPfeilspitze = (GameObject)Instantiate(Resources.Load("Prefabs/Pfeilspitze", typeof(GameObject)));
        PrefabPfeilspitze.SetActive(false);
        VectorLine = (Material)Resources.Load("Materials/Line");

        PfeilspitzenPath = new();
    }


    public void DrawVectorPath(GameObject obj, Vector3 pathStartingPoint, List<Vector3> path)
    {

        LineRenderer renderer = ResetLineRenderer(obj);



        
        path.Insert(0, pathStartingPoint);
        
        renderer.positionCount = path.Count;
        renderer.SetPosition(0,pathStartingPoint);
        SetupLineRenderer(renderer, Color.green);

        Vector3 pathSum = pathStartingPoint;
        Destroy(PfeilspitzenPath);
        PfeilspitzenPath = new();
        PfeilspitzenPath.transform.SetParent(obj.transform, false);
        PfeilspitzenPath.transform.localPosition = pathStartingPoint;


        for (int i = 1; i < path.Count; i++)
        {
            pathSum += path[i];
            renderer.SetPosition(i, pathSum);
            Debug.Log("Instantiate Single Pfeilspitze:");
            GameObject SinglePathPfeilspitze = Instantiate(PrefabPfeilspitze, PfeilspitzenPath.transform);
            SinglePathPfeilspitze.transform.localPosition = pathSum;
            SinglePathPfeilspitze.transform.localRotation = Quaternion.LookRotation(path[i]);
            SinglePathPfeilspitze.SetActive(true);
        }



    }


    public void DrawPreviewVector(GameObject obj, Vector3 startingPoint, Vector3 vector)
    {
        //LineRenderer
        LineRenderer renderer = ResetLineRenderer(obj);
        GameObject.Destroy(PreviewPfeilspitze);

        renderer.positionCount = 2;
        renderer.SetPosition(0, startingPoint);
        renderer.SetPosition(1, startingPoint + vector);
        
        SetupLineRenderer(renderer, Color.cyan);


        //Pfeilspitze
        PreviewPfeilspitze = Instantiate(PrefabPfeilspitze, obj.transform);
        PreviewPfeilspitze.transform.localPosition = startingPoint + vector;
        PreviewPfeilspitze.transform.localRotation = Quaternion.LookRotation(vector);
        PreviewPfeilspitze.SetActive(true);


    }


    private void SetupLineRenderer(LineRenderer renderer, Color color)
    {
        renderer.startWidth = 0.01f;
        renderer.endWidth = 0.01f;

        renderer.material = VectorLine;

        renderer.startColor = color;
        renderer.endColor = color;

        renderer.useWorldSpace = false;
    }

    private LineRenderer ResetLineRenderer(GameObject obj)
    {
        if (obj.GetComponent<LineRenderer>() == null)
        {
            obj.AddComponent<LineRenderer>();
        }

        LineRenderer renderer = obj.GetComponent<LineRenderer>();
        renderer.positionCount = 0;


        return renderer;
    }





}
