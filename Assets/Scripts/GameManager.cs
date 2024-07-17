using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    [Header("Player References")]
    [SerializeField] GameObject player;
    [SerializeField] Animator gameOverAnim;
    private int enemeiesLeftInScene;
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        player = GameObject.FindGameObjectWithTag("Player");

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        enemeiesLeftInScene = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemeiesLeftInScene == 0 && EnemySpawner.enemiesRemaining == 0)
        {
            NextLevel();
        }
        Debug.Log(EnemySpawner.enemiesRemaining);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverSequence());
    }

    IEnumerator GameOverSequence()
    {
        player.SetActive(false);
        yield return new WaitForSeconds(2f);
        gameOverAnim.SetTrigger("FadeOut");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameOver");
    }

    public void NextLevel()
    {
        StartCoroutine(NextLevelSequence());
    }

    IEnumerator NextLevelSequence()
    {
        gameOverAnim.SetTrigger("FadeOut");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
