using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class ChatBubble3D : MonoBehaviour
{
    public static void Create(Transform parent, Vector3 localPosition, IconType iconType, string text)
    {
        if (GameAssets.Instance.ChatBubble3D == null)
        {
            Debug.LogError("ChatBubble3D prefab is not assigned in GameAssets.");
            return;
        }

        Transform chatBubbleTransform = Instantiate(GameAssets.Instance.ChatBubble3D, parent);
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
