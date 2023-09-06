using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttackCollider : MonoBehaviour
{
    public BoxCollider attackOneCollider;
    public BoxCollider attackTwoCollider;
    public BoxCollider attackThreeCollider;


    // Update is called once per frame
    void Update()
    {
        
    }

   void AttackOneOn()
    {
        attackOneCollider.enabled = true;
    }
    void AttackTwoOn()
    {
        attackTwoCollider.enabled = true;
    }
    void AttackThreeOn()
    {
        attackThreeCollider.enabled = true;
    }

    void AttackOneOff()
    {
        attackOneCollider.enabled = false;
    }
    void AttackTwoOff()
    {
        attackTwoCollider.enabled = false;
    }
    void AttackThreeOff()
    {
        attackThreeCollider.enabled = false;
    }
}
