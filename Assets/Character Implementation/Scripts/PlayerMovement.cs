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

    private bool canMove = false; // Flag to control movement

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (canMove)
        {
            GatherInput();
            Look();
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
    }

    void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    void Look()
    {
        if (_input != Vector3.zero)
        {
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
            var skewedInput = matrix.MultiplyPoint3x4(_input);
            var relative = (transform.position + skewedInput) - transform.position;
            var rot = Quaternion.LookRotation(relative, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _turnSpeed * Time.deltaTime);
        }
    }

    void Move()
    {
        _instance.MovePosition(transform.position + (transform.forward * _input.magnitude) * _speed * Time.deltaTime);
    }

    void repositionAfterAttack()
    {
        Vector3 forwardDirection = transform.forward;
        Vector3 newPosition = transform.position + (forwardDirection * reposition);
        transform.position = newPosition;
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void DisableMovement()
    {
        canMove = false;
    }
}
