using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject player;
    private Transform target;
    [SerializeField] private Transform gunTransform;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    private AudioSource audioSource;
    [SerializeField] private AudioClip []shootSound;

    [Header("Varibles")]
    [SerializeField] private float visionRange = 15f;
    [SerializeField] private float turretRotationSpeed = 5f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float burstRate = 0.5f;
    private float fireCountdown = 0f;
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private float projectileSpread = 0.1f;
    [SerializeField] private float projectileLifeTime = 5f;
    [SerializeField] private float fovAngle = 45f;

    [SerializeField] bool burstFireMode = true;
    private bool isPlayerInFOV = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        // Check if the player GameObject is active
        if (!player.activeSelf)
        {
            target = null;
            return;
        }

        // Check if the player is within range
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= visionRange)
        {
            target = player.transform;
        }
        else
        {
            target = null;
        }

        //check if player is in field of view of fire point and sight is not blocked
        Vector3 directionToPlayer = player.transform.position - firePoint.position;
        float angle = Vector3.Angle(firePoint.forward, directionToPlayer);
        if (angle < fovAngle)
        {
            RaycastHit hit;
            if (Physics.Raycast(firePoint.position, directionToPlayer, out hit, visionRange))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    isPlayerInFOV = true;
                }
                else
                {
                    isPlayerInFOV = false;
                }
            }
        }
        else
        {
            isPlayerInFOV = false;
        }
        
    }

    void Update()
    {
        if (target == null)
        {
            return;
        }

        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(gunTransform.rotation, lookRotation, Time.deltaTime * turretRotationSpeed).eulerAngles;
        gunTransform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        // fires a burst of 3 bullets before entering a cooldown
        if (fireCountdown <= 0f && isPlayerInFOV)
        {
            if (burstFireMode)
            {
                StartCoroutine(FireBurst());
            }
            else
            {
                Shoot();
            }
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    IEnumerator FireBurst()
    {
        for (int i = 0; i < 3; i++)
        {
            Shoot();
            yield return new WaitForSeconds(burstRate);
        }
    }

    void Shoot()
    {
        audioSource.PlayOneShot(shootSound[UnityEngine.Random.Range(0, shootSound.Length)]);

        GameObject projectile = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // Apply spread to the projectile
        Vector3 spread = new Vector3(0, Random.Range(-projectileSpread, projectileSpread), 0);
        Quaternion spreadRotation = Quaternion.Euler(spread);
        Vector3 spreadDirection = spreadRotation * firePoint.forward;

        // Apply force to the projectile
        rb.AddForce(spreadDirection * projectileSpeed, ForceMode.Impulse);

        // destroy the projectile after a set amount of time
        Destroy(projectile, projectileLifeTime);
    }

    void Dead()
    {
        Debug.Log("Turret is dead");
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRange);   
    }

}
