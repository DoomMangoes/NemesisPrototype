using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class SoulEssenceCounter : MonoBehaviour
{
    public int soulCount;
    public Text count;

    [ContextMenu("IncreaseCount")]
    public void addScore()
    {
        soulCount = soulCount + 1;
        count.text = soulCount.ToString();

    }
}
