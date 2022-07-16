using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    public static GameStateMachine Singleton;
    public event Action<LevelStage> OnGameStateChanged;

    public LevelStage LevelStage = LevelStage.setup;

    [Header("GameContext")]
    public Transform UI;
    public PlayerStateMachine Player;
    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Singleton != this)
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        SetLevelState(LevelStage.gameMode);
    }

    private void SetLevelState(LevelStage state)
    {
        LevelStage = state;
        OnGameStateChanged?.Invoke(LevelStage);
    }
}

public enum LevelStage
{
    setup = 0,
    inbetween = 1,
    gameMode = 2,
    victory = 3
}