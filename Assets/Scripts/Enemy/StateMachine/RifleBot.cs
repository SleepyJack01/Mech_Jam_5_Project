using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RifleBot : MonoBehaviour
{
    [Header("References")]
    private EnemyState currentState;
    public NavMeshAgent agent;
    public NavMeshSurface surface;
    public GameObject player;
    public Transform target;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public Transform modelTransform;

    [Header("Sound References")]
    public AudioSource audioSource;
    public AudioClip[] shootSound;

    [Header("Attack Varibles")]
    public float atttackRange = 5f;
    public float projectileSpeed = 20f;
    public float projectileSpread = 0.1f;
    public float projectileLifeTime = 5f;
    public float attackRate = 2f;
    public float attackTimer = 0f;
    public float burstRate = 0.1f;
    public float fovAngle = 20f;

    [Header("player detection")]
    public float distanceToPlayer;
    public Vector3 directionToPlayer;

    [Header("bools")]
    public bool hasReposistioned = false;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);

        ChangeState(new IdleState(this));
    }

    void Update()
    {
        Debug.Log(currentState);

        if (currentState != null)
        {
            currentState.OnStateExecute();
        }
    }

    public void ChangeState(EnemyState newState)
    {
        if (currentState != null)
        {
            currentState.OnStateExit();
        }

        currentState = newState;
        currentState.OnStateEnter();
    }

    public void UpdateTarget()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            target = null;
            return;
        }

        if (!player.activeSelf)
        {
            target = null;
            return;
        }
        
        target = player.transform;
    }

    public bool PlayerIsActive()
    {
        if (player == null)
        {
            return false;
        }

        return player.activeSelf;
    }

    public bool IsInRange()
    {
        // if the player is within attack range return true
        distanceToPlayer = Vector3.Distance(target.position, transform.position);
        return distanceToPlayer <= atttackRange;
    }

    public bool IsPlayerInFov()
    {
        if (target == null)
        {
            return false;
        }

        directionToPlayer = target.position - transform.position;
        float angle = Vector3.Angle(modelTransform.forward, directionToPlayer);

        return angle < fovAngle;
    }

    public bool IsPlayerVisible()
    {
        RaycastHit hit;
        if (Physics.Raycast(modelTransform.position, directionToPlayer, out hit, atttackRange))
        {
            return hit.transform.CompareTag("Player");
        }

        return false;
    }

    public void Shoot()
    {
        if (attackTimer <= 0)
        {
            StartCoroutine(BurstFire());
            attackTimer = 1f / attackRate;
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }

    public IEnumerator BurstFire()
    {
        Debug.Log("Burst Fire");

        //fire 3 bullets in quick succession and then set repostion to false
        for (int i = 0; i < 3; i++)
        {
            FireProjectile();
            yield return new WaitForSeconds(burstRate);
        }

        hasReposistioned = false;

    }

    public void FireProjectile()
    {
        audioSource.PlayOneShot(shootSound[Random.Range(0, shootSound.Length)]);

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        Vector3 spread = new Vector3(Random.Range(-projectileSpread, projectileSpread), 0, Random.Range(-projectileSpread, projectileSpread));
        Quaternion spreadRotation = Quaternion.Euler(spread);
        Vector3 spreadDirection = spreadRotation * bulletSpawnPoint.forward;

        rb.velocity = spreadDirection * projectileSpeed;

        Destroy(bullet, projectileLifeTime);
    }

    private void Dead()
    {
        Destroy(gameObject);
    }
}
