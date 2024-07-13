using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SuicideBotBehaviour : MonoBehaviour
{
    [Header("References")]
    private NavMeshAgent agent;
    private GameObject player;
    [SerializeField] private Transform modelTransform;
    [SerializeField] private Transform explosionPoint;
    [SerializeField] private GameObject explosionPrefab;
    private AudioSource audioSource;
    private Transform target;

    [Header("Varibles")]
    [SerializeField] private float detectionRange = 50f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float modelBobAmount = 0.1f;
    [SerializeField] private float modelBobSpeed = 1f;

    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
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

    void Update()
    {
        ModelBob();

        if (target == null)
        {
            return;
        }

        IncreaseBeepingSpeed();
        Chase();
    }

    void ModelBob()
    {
        // Bobs the model up and down by a small amount
        modelTransform.localPosition = new Vector3(modelTransform.localPosition.x, Mathf.Sin(Time.time * modelBobSpeed) * modelBobAmount, modelTransform.localPosition.z);
        
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

        if (target != null)
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer <= attackRange)
            {
                BlowUp();
            }
        }
    }

    void IncreaseBeepingSpeed()
    {
        // Increase the pitch of the audio source as the player gets closer
        if (target != null)
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            audioSource.pitch = 1 + (1 - distanceToPlayer / detectionRange);
        }
    }

    void BlowUp()
    {
        GameObject explosion = Instantiate(explosionPrefab, explosionPoint.position, explosionPoint.rotation);
        Destroy(gameObject);
    }

    void Dead()
    {
        BlowUp();
    }
}