using UnityEngine;
using UnityEngine.VFX;

public class PlayerCharacterController : MonoBehaviour
{
    public Animator myAnim;
    public bool isAttacking = false;
    public static PlayerCharacterController instance;
    public VisualEffect swordSlashVFX; // Reference to the VFX component
    public VisualEffect swordSlashVFX2;
    public VisualEffect swordSlashVFX3;

    public Transform AttackPoint;
    public float attackRange = 1f;
    public LayerMask enemyLayers;

    public float attackDamage = 5f;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        myAnim = GetComponent<Animator>();
        myAnim.SetTrigger("Spawn");
    }

    void Update()
    {
        Attack();

        // Movement 
        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
        {
            myAnim.SetBool("isMoving", true);
        }
        else
        {
            myAnim.SetBool("isMoving", false);
        }
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            isAttacking = true;

            Collider[] hitEnemies = Physics.OverlapSphere(AttackPoint.position, attackRange, enemyLayers);

            foreach(Collider enemy in hitEnemies)
            {
                Debug.Log("We hit" + enemy.name);
                enemy.GetComponent<AttributeManager>().TakeDamage(attackDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (AttackPoint == null)
            return;

        Gizmos.DrawWireSphere(AttackPoint.position, attackRange);
    }
}
