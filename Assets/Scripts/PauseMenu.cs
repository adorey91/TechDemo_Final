using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject controlsPanel;


    public void pauseGame()
    {
        pausePanel.SetActive(true);
        controlsPanel.SetActive(false); 
        rb.isKinematic = true;
        Cursor.visible = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }

    public void resumeGame()
    {
        pausePanel.SetActive(false);
        controlsPanel.SetActive(false);
        rb.isKinematic = false;
        Cursor.visible = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void resetGame()
    {
        resumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void ControlsPage()
    {
        pausePanel.SetActive(false);
        controlsPanel.SetActive(true);
    }
}