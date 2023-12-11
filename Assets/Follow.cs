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
            if (follow == true)
            {
                target = player.transform;

                // Smoothly follow the player using SmoothDamp
                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    target.position,
                    ref _velocity,
                    Time.deltaTime * Random.Range(minModifier, maxModifier)
                );
            }
           
        }
    }

    
}

