using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private TextMeshProUGUI interactTextProUGUI;

    private void Update()
    {
        if (playerInteract.GetInteractableObject() != null)
        {
            Show(playerInteract.GetInteractableObject());
        }
        else
        {
            Hide();
        }
    }

    private void Show(NPCInteractable npcInteractable)
    {
        containerGameObject.SetActive(true);
        interactTextProUGUI.text = npcInteractable.GetInteractText();
    }

    private void Hide()
    {
        containerGameObject.SetActive(false);
    }

}
