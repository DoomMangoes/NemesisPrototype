using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashScript : MonoBehaviour
{
    PlayerMovement moveScript;
    Animator animator; // Reference to the Animator component

    public float dashSpeed;
    public float dashTime;

    // Start is called before the first frame update
    void Start()
    {
        moveScript = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        // Disable player movement while dashing
        moveScript.enabled = false;

        // Trigger the dashing animation
        animator.SetBool("IsDashing", true);

        Vector3 startPosition = transform.position;
        Vector3 dashDirection = transform.forward;
        Vector3 targetPosition = startPosition + dashDirection * dashSpeed * dashTime;

        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            float normalizedTime = (Time.time - startTime) / dashTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, normalizedTime);
            yield return null;
        }

        animator.SetBool("IsDashing", false); // Disable the dashing animation
        moveScript.enabled = true;
    }
}
