using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target; // Reference to the player object
    public float minModifier = 7; // Minimum modifier for SmoothDamp
    public float maxModifier = 11; // Maximum modifier for SmoothDamp
    private Vector3 _velocity = Vector3.zero; // Velocity reference for SmoothDamp

    bool follow = false;

    public void StartFollowing()
    {
        follow = true;
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            if (follow)
            {
                target = player.transform;

                // Adjust Y position to make it travel lower
                Vector3 targetPosition = target.position;
                targetPosition.y = 0; // Set your desired lower Y position

                // Smoothly follow the modified position using SmoothDamp
                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    targetPosition,
                    ref _velocity,
                    Time.deltaTime * Random.Range(minModifier, maxModifier)
                );
            }
        }
        else
        {
            Debug.LogWarning("Player not found!");
        }
    }
}
