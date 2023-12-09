using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBossAttackScript : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayers;
    public float attackDamage = 20f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Attack()
    {
            Collider[] hitPlayers = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayers);

            foreach (Collider player in hitPlayers)
            {
                Debug.Log("We hit" + player.name);
               player.GetComponent<PlayerAttributeManager>().TakeDamage(attackDamage);
            }
        
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
