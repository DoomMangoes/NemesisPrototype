using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributesManager : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int attack;

    public HealthBar healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth); 
    }

    public void TakeDamage(int damageValue)
    {
        currentHealth -= damageValue;

        healthBar.SetHealth(currentHealth);


    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            TakeDamage(20);
        }
    }

    public void DealDamageOne(GameObject target)
    {
        var atm = target.GetComponent<AttributesManager>();
        if(atm != null)
        {
            atm.TakeDamage(attack);
        }
    }
}
