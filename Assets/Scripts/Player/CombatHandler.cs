using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnPoint;

    [Header("Variables")]
    [SerializeField] float projectileSpeed = 1000f;
    [SerializeField] float fireRate = 0.2f;

    private bool isFiring = false;
    private bool isShootingCooldown = false;

    private void Update()
    {
        if (isFiring)
        {
            FireProjectile();
        }
    }

    private void FireProjectile()
    {
        // Check if shooting is on cooldown
        if (isShootingCooldown)
        {
            return;
        }

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(projectileSpawnPoint.forward * projectileSpeed, ForceMode.Impulse);

        // destroy the projectile after 5 seconds
        Destroy(projectile, 5f);
        
        // Start the shooting cooldown
        StartCoroutine(ShootingCooldown());
    }

    IEnumerator ShootingCooldown()
    {
        isShootingCooldown = true;
        yield return new WaitForSeconds(fireRate); // Wait for the specified delay
        isShootingCooldown = false;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
         if (context.performed)
        {
            isFiring = true;
        }
        else if (context.canceled)
        {
            isFiring = false;
        }
    }
}
