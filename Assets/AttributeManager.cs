using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeManager : MonoBehaviour
{
    public float maxHealth;
    private float currenthealth;
    private healthBarScript healthBar;

    void Start()
    {
        currenthealth = maxHealth;

        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<healthBarScript>();
    }

    public void TakeDamage(float damage)
    {
        currenthealth -= damage;

        healthBar.takeDamage(damage);

        //Play hurt animation

        if (currenthealth < 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        //Die animation

        //disable enemy
    }
}
