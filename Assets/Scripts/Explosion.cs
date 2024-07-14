using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] int damage = 100;
    [SerializeField] float explosionlifetime = 0.5f;
    

    [SerializeField] float explosionRadius = 5f;

    void Start()
    {
        var surroundingBots = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (var obj in surroundingBots)
        {
            Debug.Log(obj.name);
            if (obj.CompareTag("Enemy"))
            {
                obj.GetComponent<EnemyHealthHandler>().TakeDamage(damage);
            }
            else if (obj.CompareTag("Player"))
            {
                obj.GetComponent<PlayerHealthHandler>().TakeDamage(damage);
            }
        }

        Destroy(gameObject, explosionlifetime);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
    
}
