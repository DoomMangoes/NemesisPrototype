using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulCounterCollider : MonoBehaviour
{
    public SoulEssenceCounter logic;

    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<SoulEssenceCounter>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SoulEssence"))
        {
            logic.addScore();
        }
    }
}
