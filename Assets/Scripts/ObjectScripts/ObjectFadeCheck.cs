using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFadeCheck : MonoBehaviour
{
    bool fadeCheck = false;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player"){
            fadeCheck = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player"){
            fadeCheck = false;
        }
    }
}
