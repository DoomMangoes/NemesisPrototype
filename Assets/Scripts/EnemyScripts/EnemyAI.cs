using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public string playerTag = "Player";
    public float health;
    private Transform playerTransform;

    // Chasing
    public float chaseSpeed = 6f;

    // Attacking
    public float timeBetweenAttacks = 2f;
    public GameObject projectilePrefab; // Projectile prefab
    public float projectileSpeed = 10f;
    private float attackTimer;

    // States
    public float sightRange = 10f;
    public float attackRange = 2f;
    public bool playerInSightRange, playerInAttackRange;

    private Animator animator;

    private void Start()
    {
        playerTransform = GameObject.FindWithTag(playerTag).transform;
    }

    private void Awake()
    {
        
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Check if playerTransform is not null before using it
        if (playerTransform != null)
        {
            // Check for sight and attack range
            playerInSightRange = Vector3.Distance(transform.position, playerTransform.position) < sightRange;
            playerInAttackRange = Vector3.Distance(transform.position, playerTransform.position) < attackRange;

            if (playerInSightRange && !playerInAttackRange)
            {
                ChasePlayer();
            }
            else if (playerInAttackRange && playerInSightRange)
            {
                AttackPlayer();
            }
            else
            {
                // Add any additional logic for other states (e.g., idle)
            }
        }
        else
        {
            // Handle the case when playerTransform is null (optional)
        }
    }


    private void AttackPlayer()
    {
        // Stop moving when attacking
        transform.Translate(Vector3.zero);

        attackTimer += Time.deltaTime;
        if (attackTimer >= timeBetweenAttacks)
        {
            // Trigger the Attack animation
            animator.SetTrigger("Attack");
            transform.LookAt(playerTransform);

            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            // Get the rigidbody of the projectile
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

            // Check if the projectile has a rigidbody
            if (projectileRb != null)
            {
                // Calculate the direction towards the player
                Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

                // Set the velocity of the projectile
                projectileRb.velocity = directionToPlayer * projectileSpeed;
            }

            attackTimer = 0f;
        }
    }

    private void ChasePlayer()
    {
        // Move towards the player
        Vector3 moveDirection = (playerTransform.position - transform.position).normalized;
        transform.Translate(moveDirection * chaseSpeed * Time.deltaTime);

        // Make the enemy face the player
        transform.LookAt(playerTransform);
    }

    // ... (rest of the code remains unchanged)
}
