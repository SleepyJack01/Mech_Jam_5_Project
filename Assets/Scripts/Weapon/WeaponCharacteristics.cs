using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCharacteristics : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnPoint;

    [Header("Variables")]
    [SerializeField] float projectileSpeed = 1000f;
    [SerializeField] float fireRate = 0.2f;
    [SerializeField] float projectileAmount = 1;
    [SerializeField] float projectileSpread = 0;

    private bool isFiring = false;

    public void FireProjectile()
    {
        for (int i = 0; i < projectileAmount; i++)
        {
            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            // Apply spread to the projectile
            Vector3 spread = new Vector3(Random.Range(-projectileSpread, projectileSpread), Random.Range(-projectileSpread, projectileSpread), 0);
            Quaternion spreadRotation = Quaternion.Euler(spread);
            Vector3 spreadDirection = spreadRotation * projectileSpawnPoint.forward;

            rb.AddForce(spreadDirection * projectileSpeed, ForceMode.Impulse);

            // destroy the projectile after 5 seconds
            Destroy(projectile, 5f);
        }
    }


}
