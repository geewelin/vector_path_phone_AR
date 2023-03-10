using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.PlaceGrid);
    }


    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.PlaceGrid:
                break;
            case GameState.MemorizeGrid:
                break;
            case GameState.SelectVector:
                break;
            case GameState.Success:
                Debug.Log("Gamestate: Success!");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);

    }


    public enum GameState
    {
        PlaceGrid,
        MemorizeGrid,
        SelectVector,
        Success
    }



}
