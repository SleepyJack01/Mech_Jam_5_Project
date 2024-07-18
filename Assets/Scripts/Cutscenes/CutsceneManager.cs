using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager instance;
    
    [SerializeField] Animator gameOverAnim;
    [SerializeField] AudioSource audioSource;
    [SerializeField] float fadeRate = 0.02f;

    private void Awake()
    {
        instance = this;
    } 

    public void Start()
    {
        audioSource.volume = 0;
        StartCoroutine(FadeInVolume());
    }

    public void NextLevel()
    {
        StartCoroutine(NextLevelSequence());
    }

    IEnumerator NextLevelSequence()
    {
        StartCoroutine(FadeOutVolume());
        gameOverAnim.SetTrigger("FadeOut");
        yield return new WaitForSeconds(2f);
        if (SceneManager.GetActiveScene().buildIndex == 8)
        {
            SceneManager.LoadScene(4);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 7)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator FadeInVolume()
    {
        while (audioSource.volume < 0.5f)
        {
            audioSource.volume += fadeRate;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator FadeOutVolume()
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= fadeRate;
            yield return new WaitForSeconds(0.1f);
        }
    }

    
}
