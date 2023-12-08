using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Rigidbody _instance;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _turnSpeed = 1000f;

    private Vector3 _input;

    public Animator myAnim;
    public bool isAttacking = false;
    public static PlayerController instance;
    public VisualEffect swordSlashVFX; // Reference to the VFX component
    public VisualEffect swordSlashVFX2;
    public VisualEffect swordSlashVFX3;

    [SerializeField] private AudioSource SwordSlashAudio;

    private void Awake()
    {
        instance = this;
    }
    void Update(){

        GatherInput();
        Look();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }

    }

    void FixedUpdate() {
        Move();
    }


    void GatherInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Adjust the input to match the isometric movement
        float diagonalMultiplier = 0.75f; // Adjust this value to fit your isometric movement speed
        _input = new Vector3(
            horizontalInput + verticalInput * diagonalMultiplier,
            0f,
            -verticalInput + horizontalInput * diagonalMultiplier
        ).normalized;
    }

    void Look(){

        if(_input != Vector3.zero){

            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0,45,0));

            var skewedInput = matrix.MultiplyPoint3x4(_input);


            var relative = (transform.position + skewedInput) - transform.position;

            var rot = Quaternion.LookRotation(relative, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _turnSpeed * Time.deltaTime);

        }

       
    }


    void Move(){
        _instance.MovePosition(transform.position + (transform.forward * _input.magnitude) * _speed * Time.deltaTime);

        float speed = _input.magnitude; // Calculate the magnitude of the movement input
        //anim.SetFloat("Speed", speed); // Update the "Speed" animation parameter
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

    // private void Attack()
    //{
    //attack animation
    //  SwordSlashAudio.Play();
    //anim.SetTrigger("Attack 1");


    //}



    /*
    void OnCollisionEnter(Collision other)
    {
        _input = Vector3.zero;
        _instance.velocity = Vector3.zero;
    }
    */
}
