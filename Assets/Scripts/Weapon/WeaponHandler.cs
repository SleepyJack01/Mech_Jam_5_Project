using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHandler : MonoBehaviour
{
    [Header("References")]
    private GameObject []weaponArray;
    private GameObject currentWeapon;
    private int currentWeaponIndex = 0;
    private bool isFiring = false;

    private void Start()
    {
        weaponArray = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            weaponArray[i] = transform.GetChild(i).gameObject;
            weaponArray[i].SetActive(false);
        }

        currentWeapon = weaponArray[currentWeaponIndex];
        currentWeapon.SetActive(true);
    }

    private void Update()
    {
        if (isFiring)
        {
            currentWeapon.GetComponent<WeaponCharacteristics>().FireProjectile();
        }
    }

    private void SwitchToNextWeapon()
    {
        currentWeapon.SetActive(false);
        currentWeaponIndex = (currentWeaponIndex + 1) % weaponArray.Length;
        currentWeapon = weaponArray[currentWeaponIndex];
        currentWeapon.SetActive(true);

        // reset the shooting cooldown
        currentWeapon.GetComponent<WeaponCharacteristics>().ResetShootingCooldown();
        // reset the ammo
        currentWeapon.GetComponent<WeaponCharacteristics>().ResetAmmo();
    }

    private void SwitchToPreviousWeapon()
    {
        currentWeapon.SetActive(false);
        currentWeaponIndex = (currentWeaponIndex - 1 + weaponArray.Length) % weaponArray.Length;
        currentWeapon = weaponArray[currentWeaponIndex];
        currentWeapon.SetActive(true);

        // reset the shooting cooldown
        currentWeapon.GetComponent<WeaponCharacteristics>().ResetShootingCooldown();
        // reset the ammo
        currentWeapon.GetComponent<WeaponCharacteristics>().ResetAmmo();
    }
    
    public void onWeaponSwitch(InputAction.CallbackContext context)
    {
        float weaponSwitch = context.ReadValue<float>();
        //Debug.Log("Weapon Switch: " + weaponSwitch);

        if (weaponSwitch > 0)
        {
            SwitchToNextWeapon();
        }
        else if (weaponSwitch < 0)
        {
            SwitchToPreviousWeapon();
        }
    }

    public void onFire(InputAction.CallbackContext context)
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

    public void onReload(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            currentWeapon.GetComponent<WeaponCharacteristics>().ReloadWeapon();
        }
    }

}
