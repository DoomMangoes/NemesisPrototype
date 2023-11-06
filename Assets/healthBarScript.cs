using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBarScript : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float maxhealth = 100f;
    public float health;
    public float lerpSpeed = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        health = maxhealth;
    }

    // Update is called once per frame
    void Update()
    {

        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }

        if (easeHealthSlider.value != health)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed);
        }



        if (Input.GetKeyDown(KeyCode.Space))
        {
            takeDamage(15);
        }
    }

    void takeDamage(float damage)
    {
        health -= damage;
    }
}
