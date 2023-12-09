using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Rigidbody _instance;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _turnSpeed = 360f;

    private Vector3 _input;

    public static PlayerMovement instance;

    public float reposition = 2f;
   

    private void Awake()
    {
        instance = this;
    }
    void Update(){

        GatherInput();
        Look();

    }

    void FixedUpdate() {
        Move();
    }


    void GatherInput(){
        _input = new Vector3(Input.GetAxisRaw("Horizontal"),0, Input.GetAxisRaw("Vertical"));

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

     void repositionAfterAttack()
    {
        Vector3 forwardDirection = transform.forward;
        Vector3 newPosition = transform.position + (forwardDirection * reposition);
        transform.position = newPosition;
    }

    /*
    public float speed;
    public float rotationSpeed;
    public float reposition = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // Update is called once per frame
    void Update()
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





    void repositionAfterAttack()
    {
        Vector3 forwardDirection = transform.forward;
        Vector3 newPosition = transform.position + (forwardDirection * reposition);
        transform.position = newPosition;
    }

    */
}
