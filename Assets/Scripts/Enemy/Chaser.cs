using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chaser : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject meleePrefab;
    private Transform target;

    [Header("Varibles")]
    [SerializeField] private float detectionRange = 15f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackRate = 1f;
    private float attackCountdown = 0f;
    [SerializeField] private float attackSpeed = 20f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    

    void Update()
    {
        if (target == null)
        {
            return;
        }

        Chase();

        if (target != null)
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer <= attackRange)
            {
                Attack();
            }
        }
    }

    void UpdateTarget()
    {

        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            target = null;
            return;
        }
        
        // Check if the player GameObject is active
        if (!player.activeSelf)
        {
            target = null;
            return;
        }

        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= detectionRange)
        {
            target = player.transform;
        }
        else
        {
            target = null;
        }
    }

    void Chase()
    {
        // If there's a target, set the NavMeshAgent's destination to the target's current position
        if (target != null)
        {
            agent.SetDestination(target.position);

            // Rotate towards the player
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            // Ensure the rotation is only on the y-axis
            directionToPlayer.y = 0;
            if (directionToPlayer != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }
    }

    void Attack()
    {
        if (attackCountdown <= 0f)
        {
            GameObject melee = Instantiate(meleePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = melee.GetComponent<Rigidbody>();
            rb.AddForce(firePoint.forward * attackSpeed, ForceMode.Impulse);
            attackCountdown = 1f / attackRate;
        }

        attackCountdown -= Time.deltaTime;
    }
    
}
