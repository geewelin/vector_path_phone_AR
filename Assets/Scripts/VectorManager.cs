using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;
using static InterfaceManager;

public class VectorManager : MonoBehaviour
{


    public static VectorManager Instance;

    private List<Vector3> allVectors, placedVectors, unplacedVectors;
    private Vector3 previewVector;


    private GameObject referencePoint, vectorPathRenderedLine, previewVectorRenderedLine;
    private Vector3 currentPathPosition;

    private VectorDrawer drawer;

    private void Awake()
    {


        Instance = this;

        OnGameStateChanged += GameManagerOnGameStateChanged;
        OnVectorSelectionDone += InterfaceManagerOnVectorSelectionDone;

        allVectors = new List<Vector3>
        {


            //from 0,0,0 to 1, 0.5, 0.5

            new Vector3(0f,0.5f,0f),
            new Vector3(0.25f, 0f,0.25f),
            new Vector3(0.35f,-0.5f,0.75f),
            new Vector3(0.15f,0.05f,-0.8f),
            new Vector3(0.5f,0.55f,0f),
            new Vector3(-0.25f,-0.1f,0.3f)

        };

        placedVectors = new List<Vector3>();
        unplacedVectors = new List<Vector3>();

        previewVector = new Vector3(0, 0, 0);
        currentPathPosition = new Vector3(0, 0, 0);

        referencePoint = new GameObject();
        vectorPathRenderedLine = new GameObject();
        previewVectorRenderedLine = new GameObject();


        vectorPathRenderedLine = Instantiate(vectorPathRenderedLine, referencePoint.transform);
        previewVectorRenderedLine = Instantiate(previewVectorRenderedLine, referencePoint.transform);

        drawer = gameObject.AddComponent<VectorDrawer>();

    }


    private void OnDestroy()
    {
        OnGameStateChanged += GameManagerOnGameStateChanged;
        OnVectorSelectionDone -= InterfaceManagerOnVectorSelectionDone;
    }

    private void GameManagerOnGameStateChanged(GameState state)
    {
        if (state == GameState.PlaceGrid) {
            ResetVectors();
        }
        if (state == GameState.SelectVector)
        {
            StartVectorSelection();
        }
    }




    //Fängt Event von Interface Manager auf, dass Vector UI Button gedrückt wurde
    private void InterfaceManagerOnVectorSelectionDone(VectorSelection selection)
    {
        
        switch (selection)
        {
            case InterfaceManager.VectorSelection.placeCurrentVector:
                HandlePlaceCurrentVector();
                break;
            case InterfaceManager.VectorSelection.removeVector:
                HandleRemoveVector();
                break;
            case InterfaceManager.VectorSelection.selectOtherVector:
                HandleSelectOtherVector();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(selection), selection, null);
        }
    }





    //'''''' Ab hier in Controller auslagern? Alles Funktionen die Tatsächlich etwas tun ''''''

    //Zeige Vorschau eines anderen Vector in der Liste; dafür muss current Vorschau ausgeblendet und neue Vorschau eingeblendet werden
    private void HandleSelectOtherVector()
    {
        Debug.Log("Change Vektor wird erreicht!");
        unplacedVectors.Add(unplacedVectors[0]);
        unplacedVectors.RemoveAt(0);

        PrintVectorList(unplacedVectors);

        UpdatePreviewVector();
        Debug.Log("Preview Vector: " + previewVector.ToString());
    }

    
    //entferne den zuletzt hinzugefügten Vektor; dafür muss current vorschau ausgeblendet, alter vektor von Placed list zu unplaced list hinzugefügt werden
    private void HandleRemoveVector()
    {
        throw new NotImplementedException();
    }


    //füge aktuellen Vorschauvektor zur place-list hinzu; zeige vorschau von neuem Vektor
    private void HandlePlaceCurrentVector()
    {
        Debug.Log("Place Vektor wird erreicht!");
        placedVectors.Add(unplacedVectors[0]);
        unplacedVectors.RemoveAt(0);
        currentPathPosition += previewVector;

        drawer.DrawVectorPath(vectorPathRenderedLine, vectorPathRenderedLine.transform.localPosition, placedVectors);

        if(unplacedVectors.Count == 0)
        {
            GameManager.Instance.UpdateGameState(GameState.Success);
            return;
        }

        UpdatePreviewVector();

    }


    public void UpdateReferencePoint(Vector3 position, Quaternion rotation)
    {

        Debug.Log("Setze Reference point:");

        if (referencePoint)
        {
            referencePoint.transform.position = position;
            referencePoint.transform.rotation = rotation;
        }
        Debug.Log("Reference point gesetzt!");
    }


    private void ResetVectors()
    {

        Destroy(referencePoint);
        Destroy(previewVectorRenderedLine);
        referencePoint = new GameObject();
        vectorPathRenderedLine = new GameObject();
        previewVectorRenderedLine = new GameObject();
        vectorPathRenderedLine = Instantiate(vectorPathRenderedLine, referencePoint.transform);
        previewVectorRenderedLine = Instantiate(previewVectorRenderedLine, referencePoint.transform);


        placedVectors.Clear();
        unplacedVectors.Clear();

        previewVector = Vector3.zero;
        currentPathPosition = Vector3.zero;
    }

    private void StartVectorSelection()
    {

        Debug.Log("Vector Selection started");
        foreach (Vector3 vec in allVectors)
        {
            unplacedVectors.Add(vec);
        }

        PrintVectorList(unplacedVectors);

        UpdatePreviewVector();
        Debug.Log("Preview Vector: " + previewVector.ToString());
    }

    private void UpdatePreviewVector()
    {
        previewVector = unplacedVectors[0];
        Debug.Log("Preview Vector assigned");
        drawer.DrawPreviewVector(previewVectorRenderedLine, currentPathPosition, previewVector);
        Debug.Log("Preview Vector gezeichnet!");

    }

    private void PrintVectorList(List<Vector3> list)
    {
        Debug.Log("List Start:");
        foreach (Vector3 vec in list)
        {
            Debug.Log(vec.ToString());
        }
        Debug.Log("List End");
    }
}

