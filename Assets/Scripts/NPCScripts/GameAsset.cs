using UnityEngine;

public class GameAsset : MonoBehaviour
{
    // Singleton instance for easy access
    public static GameAsset Instance { get; private set; }

    // Reference to the prefab
    public GameObject pfChatBubble;

    private void Awake()
    {
        // Ensure there is only one instance of this class
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
