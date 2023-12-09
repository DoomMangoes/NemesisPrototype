using UnityEngine;

public class GameAssets : MonoBehaviour
{

    private static GameAssets Prefabs;

    public static GameAssets Instance
    {
        get
        {
            if (Prefabs == null) Prefabs = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return Prefabs;
        }
    }

    private void Awake()
    {
        Prefabs = this;
    }

    public Transform ChatBubble3D;
}
