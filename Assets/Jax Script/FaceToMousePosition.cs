using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToMousePosition : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButton(0))
        {
            // Get the mouse position in viewport coordinates
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToViewportPoint(mousePosition);

            // Calculate the direction from the object to the mouse position
            Vector3 direction = new Vector3(mousePosition.x - transform.position.x, 0, mousePosition.y - transform.position.y);

            // Set the rotation to face the mouse position only in the Y-axis
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }
}