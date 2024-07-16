using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    [SerializeField] private GameObject PauseCanvas;
    [SerializeField] private GameObject HealthBar;
    [SerializeField] private GameObject AmmoBar;
    
    void Start()
    {
        Time.timeScale = 1f;
    }

    void Pause()
    {
        PauseCanvas.SetActive(true);
        HealthBar.SetActive(false);
        AmmoBar.SetActive(false);
        isPaused = true;
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        PauseCanvas.SetActive(false);
        HealthBar.SetActive(true);
        AmmoBar.SetActive(true);
        isPaused = false;
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void OnStartButton(InputAction.CallbackContext context)
    {
        if (!isPaused)
        {
            Pause();
        }
        else
        {
            Resume();
        } 
    }
}
