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
            //DontDestroyOnLoad(this.gameObject);
        }
        else if (Singleton != this)
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(StartGameRoutine());
    }

    IEnumerator StartGameRoutine()
    {
        yield return new WaitForEndOfFrame();
        StartGame();
    }

    public void StartGame()
    {
        SetLevelState(LevelStage.gameMode);
    }

    public void SetLevelState(LevelStage state)
    {
        if (state == LevelStage.inbetween)
        {
            foreach (var character in FindObjectsOfType<CharacterStateMachine>())
            {
                Rigidbody rigidbody = character.GetComponent<Rigidbody>();
                if (rigidbody != null)
                    rigidbody.isKinematic = true;
            }
        }
        if (state == LevelStage.gameMode)
        {
            foreach (var character in FindObjectsOfType<CharacterStateMachine>())
            {
                Rigidbody rigidbody = character.GetComponent<Rigidbody>();
                if (rigidbody != null)
                    rigidbody.isKinematic = false;
            }
        }
        if (state == LevelStage.loose)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        if (state == LevelStage.victory)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        LevelStage = state;
        OnGameStateChanged?.Invoke(LevelStage);
    }
}

public enum LevelStage
{
    setup = 0,
    inbetween = 1,
    gameMode = 2,
    victory = 3,
    loose = 4
}