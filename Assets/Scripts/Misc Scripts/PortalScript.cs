using UnityEngine;

public class PortalScript : MonoBehaviour
{
    public string secondSceneName = "Scene2"; // Replace with the name of your second scene
    public GameObject Portal; // Reference to the portal GameObject
    private bool bossDefeated = false; // Flag to track whether the boss is defeated

    void Awake()
    {
        // Initially, hide the portal
        if (Portal != null)
        {
            Portal.SetActive(false);
        }

        // Subscribe to the BossDefeated event
        AttributeManager.OnBossDefeated += BossDefeated;
    }

    void BossDefeated()
    {
        // Call this method when the boss is defeated
        bossDefeated = true;

        // Check if the Portal variable is assigned
        if (Portal != null)
        {
            // Show the portal
            Portal.SetActive(true);
        }
        else
        {
            // Log a warning if the Portal variable is not assigned
            Debug.LogWarning("Portal variable in PortalScript is not assigned.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && bossDefeated)
        {
            // Load the second scene when the player enters the portal and the boss is defeated
            UnityEngine.SceneManagement.SceneManager.LoadScene(secondSceneName);
        }
    }

    // Unsubscribe from the event when this script is destroyed
    private void OnDestroy()
    {
        AttributeManager.OnBossDefeated -= BossDefeated;
    }
}
