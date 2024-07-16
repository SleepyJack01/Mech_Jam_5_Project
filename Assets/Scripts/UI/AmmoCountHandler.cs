using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AmmoCountHandler : MonoBehaviour
{
    private int maxAmmoCount;
    private int currentAmmoCount;
    [SerializeField] public TextMeshProUGUI ammoCountText;

    public void SetMaxAmmoCount(int maxAmmoCount)
    {
        this.maxAmmoCount = maxAmmoCount;
        ammoCountText.text = maxAmmoCount.ToString("00") + "/" + maxAmmoCount.ToString("00");
    }
    public void SetAmmoCount(int ammoCount)
    {
        currentAmmoCount = ammoCount;
        ammoCountText.text = ammoCount.ToString("00") + "/" + maxAmmoCount.ToString("00");
    }

    
}
