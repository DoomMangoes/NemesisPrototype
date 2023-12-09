using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public float reposition = 2f;
    private bool canMove = false; // Flag to control movement

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Adjusting input for isometric movement
            float diagonalMultiplier = 0.7f; // Tweak this value for desired diagonal speed
            Vector3 movementDirection = new Vector3(
                horizontalInput + verticalInput,
                0,
                verticalInput - horizontalInput  // Adjusted the calculation here
            ).normalized;


            transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);

            if (movementDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    void EnableMovement()
    {
        canMove = true;
    }

    void DisableMovement()
    {
        canMove = false;
    }

    void repositionAfterAttack()
    {
        Vector3 forwardDirection = transform.forward;
        Vector3 newPosition = transform.position + (forwardDirection * reposition);
        transform.position = newPosition;
    }
}
