using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeManager : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;
    private healthBarScript healthBar;
    public Animator animator;

    public delegate void BossDefeatedEvent();
    public static event BossDefeatedEvent OnBossDefeated;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<healthBarScript>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        healthBar.takeDamage(damage);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        // Set the "Die" trigger in the animator
        animator.SetTrigger("Die");

        // Delay the destruction by 5 seconds
        Invoke("DestroyAfterDelay", 10f);
    }

    void DestroyAfterDelay()
    {
        // Trigger the BossDefeated event
        if (OnBossDefeated != null)
        {
            OnBossDefeated();
        }

        // Destroy the object
        Destroy(gameObject);
    }
}
