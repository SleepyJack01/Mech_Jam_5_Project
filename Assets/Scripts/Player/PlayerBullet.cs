using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] int damage = 10;

    void OnCollisionEnter(Collision collision)
    {
        // First, try to get the component directly on the collided object.
        EnemyHealthHandler enemyHealth = collision.gameObject.GetComponent<EnemyHealthHandler>();
    
        // If not found on the parent, try to find it on any of the children.
        if (enemyHealth == null)
        {
            enemyHealth = collision.gameObject.GetComponentInParent<EnemyHealthHandler>();
        }

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
