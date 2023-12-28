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

    public GameObject deathPrefab; // Reference to the prefab to instantiate upon death

    public delegate void BossDefeatedEvent();
    public static event BossDefeatedEvent OnBossDefeated;

    //Wave Checking Mechanics

    public EnemyWaveScript enemyWaveScript;

    void Awake(){
        //Wave Checking Mechanics
        if(gameObject.tag == "Enemy"){
            enemyWaveScript = gameObject.transform.parent.gameObject.GetComponent<EnemyWaveScript>();
        }
        
    }

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
            OnBossDefeated?.Invoke();
        }

        // Instantiate the prefab
        if (deathPrefab != null)
        {
            Instantiate(deathPrefab, transform.position, Quaternion.identity);
        }

       
        // Destroy the object
        //Destroy(gameObject);

        //Wave Checking Mechanics

        gameObject.SetActive(false);
        /*
        if(gameObject.tag == "Enemy"){
            enemyWaveScript.CheckWaveOver();
        }
        */
    }
}
