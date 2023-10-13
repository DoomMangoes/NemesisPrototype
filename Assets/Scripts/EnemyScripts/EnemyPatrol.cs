using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyPatrol : MonoBehaviour
{
    public float patrolSpeed = 1.5f;
    public float detectionRadius = 5f;
    public LayerMask playerLayer;
    public List<Transform> waypoints;

    private Transform player;
    private NavMeshAgent navMeshAgent;
    private int currentWaypointIndex = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Ensure there's at least one waypoint
        if (waypoints.Count > 0)
        {
            SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void Update()
    {
        DetectPlayer();

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.1f)
        {
            // Reached the current waypoint, move to the next one
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void DetectPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        if (colliders.Length > 0)
        {
            // Player detected, start pursuing.
            FollowPlayer();
        }
    }

    void FollowPlayer()
    {
        navMeshAgent.SetDestination(player.position);
    }

    void SetDestination(Vector3 destination)
    {
        navMeshAgent.SetDestination(destination);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.green;
        foreach (Transform waypoint in waypoints)
        {
            Gizmos.DrawSphere(waypoint.position, 0.1f);
        }
    }
}
