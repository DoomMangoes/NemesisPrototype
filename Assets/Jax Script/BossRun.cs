using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRun : StateMachineBehaviour
{
    public string playerTag = "Player"; // Tag of the object to follow
    public float moveSpeed ; // Speed of movement
    public float rotationSpeed ; // Speed of rotation
    public float stopDistance; // Distance to stop following

    private Transform playerTransform; // Transform of the player

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Find the player object with the specified tag
        GameObject player = GameObject.FindWithTag(playerTag);

        if (player != null)
        {
            // Get the Transform of the player
            playerTransform = player.transform;

            // Calculate the direction to move towards the player
            Vector3 moveDirection = (playerTransform.position - animator.transform.position).normalized;

            // Calculate the rotation towards the player
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            // Rotate the object towards the player
            animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Calculate the distance to the player
            float distanceToPlayer = Vector3.Distance(animator.transform.position, playerTransform.position);

            if (distanceToPlayer > stopDistance)
            {
                // Move the object towards the player
                animator.transform.position += moveDirection * moveSpeed * Time.deltaTime;
                
            }
            else
            {
                animator.SetTrigger("Attack");
            }
            
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}
