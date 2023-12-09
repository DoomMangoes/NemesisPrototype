using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 200;
    public float currenthealth;
    void Start()
    {
        currenthealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currenthealth -= damage;

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
