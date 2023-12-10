using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextDialogue : MonoBehaviour
{
    int index = 2;

    private void Update()
    {
        if (Input.GetMouseButton(1) && transform.childCount > 1)
        {
            if (PlayerCharacterController.dialogue)
            {
                // Check if the current index is within the bounds of the children
                if (index < transform.childCount)
                {
                    transform.GetChild(index).gameObject.SetActive(true);
                    index += 1;
                }
                else
                {
                    // Reset index and finish dialogue
                    index = 2;
                    PlayerCharacterController.dialogue = false;
                }
            }
            else
            {
                // Disable the entire dialogue object
                gameObject.SetActive(false);
            }
        }
    }
}
