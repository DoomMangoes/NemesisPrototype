using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChatBubble3D : MonoBehaviour
{
    public static void Create(Transform parent, Vector3 localPosition, IconType iconType, string text)
    {
        Transform chatBubbleTransform = Instantiate(parent);
        chatBubbleTransform.localPosition = localPosition;

        chatBubbleTransform.GetComponent<ChatBubble3D>().Setup(iconType, text);

        Destroy(chatBubbleTransform.gameObject, 6f);
    }

    private void Setup(IconType iconType, string text)
    {
        throw new NotImplementedException();
    }

    public enum IconType{
        Happy,
        Neutral,
        Angry,
        }
}
