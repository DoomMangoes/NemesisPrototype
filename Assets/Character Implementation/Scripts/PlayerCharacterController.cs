using UnityEngine;
using UnityEngine.VFX;

public class PlayerCharacterController : MonoBehaviour
{
    public Animator myAnim;
    public bool isAttacking = false;
    public static PlayerCharacterController instance;
    public VisualEffect swordSlashVFX; // Reference to the VFX component
    public VisualEffect swordSlashVFX2;
    public VisualEffect swordSlashVFX3;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    void Update()
    {
        Attack();

        // Movement 
        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
        {
            myAnim.SetBool("isMoving", true);
        }
        else
        {
            myAnim.SetBool("isMoving", false);
        }
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            isAttacking = true;

            // Play the SwordSlashOne VFX
            //if (swordSlashVFX != null)
           // {
            //    swordSlashVFX.Play();
           // }
        }
    }
}
