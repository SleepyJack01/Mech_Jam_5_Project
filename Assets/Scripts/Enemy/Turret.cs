using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject player;
    private Transform target;
    [SerializeField] private Transform gunTransform;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Varibles")]
    [SerializeField] private float range = 15f;
    [SerializeField] private float turretRotationSpeed = 5f;
    [SerializeField] private float fireRate = 1f;
    private float fireCountdown = 0f;
    [SerializeField] private float projectileSpeed = 20f;

    void Start()
    {
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

        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= range)
        {
            target = player.transform;
        }
        else
        {
            target = null;
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

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * projectileSpeed, ForceMode.Impulse);

        // destroy the projectile after 5 seconds
        Destroy(projectile, 5f);
    }

    void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);   
    }

}
