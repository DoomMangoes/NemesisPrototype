using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCInteractable : MonoBehaviour
{
    public GameObject d_template;
    public GameObject canva;
    [SerializeField] private string interactText;
   public void Interact()
    {
        /*ChatBubble3D.Create(transform.transform, new Vector3(-.3f, 1.7f, 0f), ChatBubble3D.IconType.Happy, "Hello There!");
        Debug.Log("E is pressed and interacted");*/
        canva.SetActive(true);
        PlayerCharacterController.dialogue = true;
        NewDialogue("Welcome Adventurer!, I am Finn. I'll be you guide your journey. Feel free to explore our world!");
        canva.transform.GetChild(1).gameObject.SetActive(true);
    }

    void NewDialogue(string text)
    {
        GameObject template_clone = Instantiate(d_template);
        template_clone.transform.SetParent(canva.transform, false);
        template_clone.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = text;
    }

    public string GetInteractText()
    {
        return interactText;
    }
}
