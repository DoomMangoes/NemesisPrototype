using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeManager : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;
    public Animator animator;

    // Reference to the specific health bar for this enemy
    public healthBarScript healthBar;

    public delegate void BossDefeatedEvent();
    public static event BossDefeatedEvent OnBossDefeated;

    void Start()
    {
        currentHealth = maxHealth;

        // Assign the health bar reference via Inspector or script
        // healthBar = ...; // Assign the health bar specific to this enemy
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // Ensure the associated health bar exists and update its display
        if (healthBar != null)
        {
            healthBar.takeDamage(damage);
        }

        // Play hurt animation

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        if (animator != null)
        {
            OnBossDefeated();
        }

        
    }

    public void SelfDestruct()
    {
        // Destroy the object
        Destroy(gameObject);
    }
}
