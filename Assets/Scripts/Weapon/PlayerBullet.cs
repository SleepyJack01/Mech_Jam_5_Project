using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] int damage = 10;
    [SerializeField] bool destroyedOnImpact = true;

    private void OnTriggerEnter(Collider other) 
    {
        // First, try to get the component directly on the collided object.
        EnemyHealthHandler enemyHealth = other.gameObject.GetComponent<EnemyHealthHandler>();
    
        // If not found on the parent, try to find it on any of the children.
        if (enemyHealth == null)
        {
            enemyHealth = other.gameObject.GetComponentInParent<EnemyHealthHandler>();
        }

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }

        if (destroyedOnImpact)
        {
            Destroy(gameObject);
        }
    }
}
