using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrelHandler : MonoBehaviour
{
    [SerializeField] private Transform explosionPoint;
    [SerializeField] private GameObject explosionPrefab;
    private bool hasBlownUp = false;

    void Dead()
    {
        BlowUp();
    }

    void BlowUp()
    {
        if (hasBlownUp)
        {
            return;
        }
        GameObject explosion = Instantiate(explosionPrefab, explosionPoint.position, explosionPoint.rotation);
        Destroy(gameObject);
        hasBlownUp = true;
    }
}
