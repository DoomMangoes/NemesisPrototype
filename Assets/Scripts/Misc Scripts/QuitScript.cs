using UnityEngine;

public class QuitScript : MonoBehaviour
{
    // Call this method to quit the application
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
