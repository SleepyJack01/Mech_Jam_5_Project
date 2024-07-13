using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] int damage = 10;

    void OnCollisionEnter(Collision collision)
    {
        // First, try to get the component directly on the collided object.
        PlayerHealthHandler playerHealth = collision.gameObject.GetComponent<PlayerHealthHandler>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
