using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator transition;
    private AudioSource audioSource;
    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        audioSource.volume = 0;
        
        StartCoroutine(FadeInVolume());

    }

    public void PlayGame()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        StartCoroutine(FadeOutVolume());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("FadeOut");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator FadeInVolume()
    {
        while (audioSource.volume < 0.5f)
        {
            audioSource.volume += 0.02f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator FadeOutVolume()
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= 0.02f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
