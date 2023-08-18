using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorType{

        left,
        right,
        top,
        bottom,
        leftB,
        rightB,
        topB,
        bottomB,

    }

    public DoorType doorType;
    
     void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player"){
            gameObject.SetActive(false);
        }
    }
}
