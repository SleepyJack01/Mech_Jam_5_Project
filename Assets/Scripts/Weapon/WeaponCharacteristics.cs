using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCharacteristics : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnPoint;
    private AudioSource audioSource;

    [Header("Projectile Variables")]
    [SerializeField] float projectileSpeed = 30f;
    [SerializeField] float fireRate = 0.2f;
    [SerializeField] float projectileLifeTime = 5f;
    [SerializeField] float projectileAmount = 1;
    [SerializeField] float projectileSpread = 0;
    private bool isShootingCooldown= false;

    [Header("Ammo Variables")]
    [SerializeField] float maxAmmoCount = 10;
    private float currentAmmoCount;

    [Header("Reload Variables")]
    [SerializeField] float reloadTime = 2f;
    private bool isReloading = false;

    [Header("Effects Variables")]
    [SerializeField] AudioClip []fireSound;
    [SerializeField] AudioClip reloadStartSound;
    [SerializeField] AudioClip reloadEndSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentAmmoCount = maxAmmoCount;
    }

    public void FireProjectile()
    {
        // Check if reloading
        if (isReloading)
        {
            return;
        }

        // Check if out of ammo
        if (currentAmmoCount <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        
        // Check if shooting is on cooldown
        if (isShootingCooldown)
        {
            return;
        }

        Debug.Log("ammo " + currentAmmoCount + " / " + maxAmmoCount + " bullets");

        for (int i = 0; i < projectileAmount; i++)
        {
            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            // Apply spread to the projectile
            Vector3 spread = new Vector3(0, Random.Range(-projectileSpread, projectileSpread), 0);
            Quaternion spreadRotation = Quaternion.Euler(spread);
            Vector3 spreadDirection = spreadRotation * projectileSpawnPoint.forward;

            // Apply force to the projectile
            rb.AddForce(spreadDirection * projectileSpeed, ForceMode.Impulse);

            // destroy the projectile after 5 seconds
            Destroy(projectile, projectileLifeTime);
        }

        // Decrease the ammo count
        currentAmmoCount--;

        // Play the fire sound
        audioSource.PlayOneShot(fireSound[Random.Range(0, fireSound.Length)]);

        // Start the shooting cooldown
        StartCoroutine(ShootingCooldown());
    }

    public void ResetShootingCooldown()
    {
        isShootingCooldown = false;
    }

    public void ResetAmmo()
    {
        //currentAmmoCount = maxAmmoCount;
        isReloading = false;
    }

    public void ReloadWeapon()
    {
        if (!isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator ShootingCooldown()
    {
        isShootingCooldown = true;
        yield return new WaitForSeconds(fireRate);
        isShootingCooldown = false;
    }

    public IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(0.4f);
        audioSource.PlayOneShot(reloadStartSound);
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmoCount = maxAmmoCount;
        audioSource.PlayOneShot(reloadEndSound);
        yield return new WaitForSeconds(0.8f);
        isReloading = false;
    }
}
