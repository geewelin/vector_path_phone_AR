using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameManager;

public class InterfaceManager : MonoBehaviour
{

    public static InterfaceManager Instance;

    [SerializeField] private GameObject _placeGridPanel, _memorizeObstaclesPanel, _selectVectorPanel, _successPanel;
    [SerializeField] private GameObject _selectOtherVectorButton, _removeVectorButton, _placeCurrentVectorButton, _reloadExperienceButton;

    public VectorSelection vectorSelection;

    public static event Action<VectorSelection> OnVectorSelectionDone;

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameState state)
    {
        _placeGridPanel.SetActive(state == GameState.PlaceGrid);
        _memorizeObstaclesPanel.SetActive(state == GameState.MemorizeGrid);
        _selectVectorPanel.SetActive(state == GameState.SelectVector);
        _successPanel.SetActive(state == GameState.Success);
    }

    public void ReloadExperience()
    {
        GameManager.Instance.UpdateGameState(GameState.PlaceGrid);
    }


    public enum VectorSelection
    {
        selectOtherVector,
        removeVector,
        placeCurrentVector
    }

    private void ThrowVectorSelectionEvent(VectorSelection sel)
    {
        vectorSelection = sel;
        OnVectorSelectionDone?.Invoke(vectorSelection);
    }

    //wird ausgeführt von Select button
    public void ChangeVector()
    {
        ThrowVectorSelectionEvent(VectorSelection.selectOtherVector);
    }


    //wird ausgeführt von Place button
    public void PlaceVector()
    {
        ThrowVectorSelectionEvent(VectorSelection.placeCurrentVector);
    }


    //wird ausgeführt von start placing button
    public void HideObstacles()
    {
        GameManager.Instance.UpdateGameState(GameState.SelectVector);
    }

}
