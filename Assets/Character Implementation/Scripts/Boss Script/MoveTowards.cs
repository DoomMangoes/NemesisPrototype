using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    public string playerTag = "Player"; // Tag of the object to follow
    public float moveSpeed = 5f; // Speed of movement
    public float rotationSpeed = 5f; // Speed of rotation

    private Transform playerTransform; // Transform of the player

    private void Update()
    {
        // Find the player object with the specified tag
        GameObject player = GameObject.FindWithTag(playerTag);

        if (player != null)
        {
            // Get the Transform of the player
            playerTransform = player.transform;

            // Calculate the direction to move towards the player
            Vector3 moveDirection = (playerTransform.position - transform.position).normalized;

            // Calculate the rotation towards the player
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            // Rotate the object towards the player
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move the object towards the player
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }
}
