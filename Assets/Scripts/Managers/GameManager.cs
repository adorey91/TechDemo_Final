using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Gameplay,
        Gameplay_Arena,
        Pause,
        Controls,
        Gameover,
    }
    public GameState state;
    internal GameState currentState;
    GameState stateBeforePause;

    bool isGamePaused = false;

    public Player player;

    [Header("Other Managers")]
    [SerializeField] BuildingManager buildingManager;
    [SerializeField] CollectableManager collectableManager;
    [SerializeField] UIManager uiManager;
    [SerializeField] SoundManager soundManager;

    [Header("Boundaries")]
    public GameObject levelBoundaries;

    public void Start()
    {
        GameObject.Instantiate(levelBoundaries);
        state = GameState.Gameplay;
    }

    public void Update()
    {
        if (state != currentState)
            StateSwitch();
    }

    public void StateSwitch()
    {
        switch (state)
        {
            case GameState.Gameplay: Gameplay(); break;
            case GameState.Gameplay_Arena: Arena(); break;
            case GameState.Pause: Pause(); break;
            case GameState.Controls: Controls(); break;
            case GameState.Gameover: Gameover(); break;
        }

        currentState = state;
    }

    public void EscapeState()
    {
        player.escapeInteracted = false;

        if (state == GameState.Gameplay || state == GameState.Gameplay_Arena)
        {
            stateBeforePause = currentState;
            LoadState("Pause");
        }
        else if (state == GameState.Controls)
            LoadState("Pause");
        else if(state == GameState.Pause)
            LoadState(stateBeforePause.ToString());
    }

    void Gameplay()
    {
        uiManager.UI_Gameplay();
        isGamePaused = false;
    }

    void Arena()
    {
        uiManager.UI_Arena();
        isGamePaused = false;
    }

    void Pause()
    {
        uiManager.UI_Pause();
        isGamePaused = true;
    }

    void Controls()
    {
        uiManager.UI_Controls();
        isGamePaused = true;
    }

    void Gameover()
    {
        uiManager.UI_Gameover();
        isGamePaused = true;
    }

    public void ResetGame()
    {
        LoadState("Gameplay");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EndGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
    }

    public void LoadState(string gameState)
    {
        if (gameState == "Controls")
            state = GameState.Controls;
        else if (gameState == "Pause")
            state = GameState.Pause;
        else if (gameState == "Gameplay")
            state = GameState.Gameplay;
        else if (gameState == "Arena")
            state = GameState.Gameplay_Arena;
        else if (gameState == "Gameover")
            state = GameState.Gameover;
        else
            Debug.Log("State doesnt exist");
    }

    public bool IsGamePaused() {  return isGamePaused; }

}