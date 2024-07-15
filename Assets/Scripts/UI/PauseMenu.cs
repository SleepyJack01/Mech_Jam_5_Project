using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    [SerializeField] private GameObject PauseCanvas;
    
    void Start()
    {
        Time.timeScale = 1f;
    }

    void Pause()
    {
        PauseCanvas.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        PauseCanvas.SetActive(false);
        isPaused = false;
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); // this needs to be changed to the main menu scene
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
