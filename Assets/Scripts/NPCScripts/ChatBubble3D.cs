using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class ChatBubble3D : MonoBehaviour
{
    public static void Create(Transform parent, Vector3 localPosition, IconType iconType, string text)
    {
        // Load the prefab directly from the "Prefabs" folder
        GameObject ChatBubble3D = Resources.Load<GameObject>("Prefabs/ChatBubble3D");

        if (ChatBubble3D == null)
        {
            Debug.LogError("ChatBubble3D prefab not found in Resources/Prefabs folder.");
            return;
        }

        Transform chatBubbleTransform = Instantiate(ChatBubble3D, parent).transform;
        chatBubbleTransform.localPosition = localPosition;

        ChatBubble3D chatBubble = chatBubbleTransform.GetComponent<ChatBubble3D>();
        if (chatBubble != null)
        {
            chatBubble.Setup(iconType, text);
            Destroy(chatBubbleTransform.gameObject, 6f);
        }
    }

    private void Setup(IconType iconType, string text)
    {
        throw new NotImplementedException();
    }

    public enum IconType {
        Happy,
        Neutral,
        Angry,
    }
}
