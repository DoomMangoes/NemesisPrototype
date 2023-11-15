using UnityEngine;

public class BossIdle : StateMachineBehaviour
{
    private string playerTag = "Player";
    private Transform playerTransform;
    public float distanceToWakeUp; // Assign a default value, or set it in the Inspector
    GameObject player;
    GameObject golemCanvas;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindWithTag(playerTag);
        golemCanvas = GameObject.FindWithTag("GolemCanvas");

        if (golemCanvas == null)
        {
            Debug.LogError("GolemCanvas not found. Make sure it has the correct tag.");
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player != null)
        {
            playerTransform = player.transform;
            float distanceToPlayer = Vector3.Distance(animator.transform.position, playerTransform.position);

            Debug.Log("Distance to Player: " + distanceToPlayer);

            if (distanceToPlayer <= distanceToWakeUp)
            {
                if (golemCanvas != null)
                {
                    // Disable the Canvas component of the GolemCanvas
                    Canvas canvasComponent = golemCanvas.GetComponent<Canvas>();
                    if (canvasComponent != null)
                    {
                        canvasComponent.enabled = true;
                        animator.SetTrigger("WakeUp");
                    }
                    else
                    {
                        Debug.LogError("Canvas component not found on GolemCanvas.");
                    }
                }
                else
                {
                    Debug.LogError("GolemCanvas not found. Make sure it has the correct tag.");
                }
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Optionally, you can reset or perform any other actions when exiting the state
    }
}
