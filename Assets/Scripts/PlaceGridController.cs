using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using static GameManager;


[RequireComponent(typeof(ARRaycastManager), typeof(ARPlaneManager))]
public class PlaceGridController : MonoBehaviour
{

    [SerializeField] private GameObject prefab;


    private ARRaycastManager aRRaycastManager;
    private ARPlaneManager aRPlaneManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private GameObject _grid;

    bool gridPlaced = false;


    private void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        aRPlaneManager = GetComponent<ARPlaneManager>();

        OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameState newState)
    {
        GameState state = newState;

        switch (state)
        {
            case GameState.PlaceGrid:
                TogglePlaneDetection(true);
                if (gridPlaced)
                {
                    Destroy(_grid);
                    gridPlaced = false;
                }
                break;

            case GameState.MemorizeGrid:
                TogglePlaneDetection(false);
                break;

            case GameState.SelectVector:
                _grid.transform.Find("Obstacles").gameObject.SetActive(false);
                break;
            case GameState.Success:
                _grid.transform.Find("Obstacles").gameObject.SetActive(true);
                break;

        }

    }

    private void OnDestroy()
    {
        OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void Start()
    {
        TogglePlaneDetection(false);
    }

    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void FingerDown(EnhancedTouch.Finger finger)
    {
        if (finger.index != 0) return;

        

        //auslagern
        if (!gridPlaced && aRRaycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;
            //_grid = new GameObject();
            _grid = Instantiate(prefab, pose.position, pose.rotation);
            Debug.Log("STELLE 1");
            gridPlaced = true;
            Debug.Log("STELLE 2");

            VectorManager.Instance.UpdateReferencePoint(pose.position, pose.rotation);
            Debug.Log("STELLE 4");
            GameManager.Instance.UpdateGameState(GameState.MemorizeGrid);
            Debug.Log("STELLE 3");



        }
    }

    private void TogglePlaneDetection(bool newState)
    {
        //aRRaycastManager.enabled = newState;
        aRPlaneManager.enabled = newState;

        if (newState)
        {
            foreach (ARPlane plane in aRPlaneManager.trackables)
            {
                plane.gameObject.SetActive(false);
            }
        }
        


    }


}
