using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [Header("UI Objects")]
    public GameObject gameUI;
    public GameObject arenaUI;
    public GameObject pauseUI;
    public GameObject controlsUI;
    public GameObject gameOverUI;
    public GameObject arenaCamera;
    public GameObject arenaScreen;


    public void UI_Gameplay()
    {
        PlayerMovement(CursorLockMode.Locked, false, 1f);
        SetUIActive(gameUI);
    }

    public void UI_Arena()
    {
        PlayerMovement(CursorLockMode.Locked, false, 1f);
        SetUIActive(arenaUI);
    }

    public void UI_Pause()
    {
        PlayerMovement(CursorLockMode.None, true, 0f);
        SetUIActive(pauseUI);
    }

    public void UI_Gameover()
    {
        PlayerMovement(CursorLockMode.None, true, 0f);
        SetUIActive(gameOverUI);
    }

    public void UI_Controls()
    {
        PlayerMovement(CursorLockMode.None, true, 0f);
        SetUIActive(controlsUI);
    }


    void SetUIActive(GameObject activeUI)
    {
        gameUI.SetActive(false);
        arenaUI.SetActive(false);
        pauseUI.SetActive(false);
        gameOverUI.SetActive(false);
        controlsUI.SetActive(false);
        arenaCamera.SetActive(false);
        arenaScreen.SetActive(false);

        activeUI.SetActive(true);

        if(activeUI == arenaUI)
        {
            arenaCamera.SetActive(true);
            arenaScreen.SetActive(true);
        }
    }

    void PlayerMovement(CursorLockMode lockMode, bool visible, float time)
    {
        Cursor.lockState = lockMode;
        Cursor.visible = visible;
        Time.timeScale = time;
    }
}
