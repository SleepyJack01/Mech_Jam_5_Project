using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player References")]
    [SerializeField] Transform playerRespawnPoint;
    [SerializeField] GameObject player;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void RespawnPlayer()
    {
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        player.SetActive(false);
        Debug.Log("Player died.");

        yield return new WaitForSeconds(3f);

        Debug.Log("Player respawned.");
        player.transform.position = playerRespawnPoint.position;
        player.SetActive(true);

        player.GetComponent<PlayerHealthHandler>().ResetHealthAndColor();
        player.GetComponent<CombatHandler>().ResetShootingCooldown();
    }
}
