using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Rigidbody _instance;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _turnSpeed = 360;

    private Vector3 _input;
    public Animator anim;

    [SerializeField] private AudioSource SwordSlashAudio;

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
        anim.SetFloat("Speed", speed); // Update the "Speed" animation parameter
    }

    private void Attack()
    {
        //attack animation
        SwordSlashAudio.Play();
        anim.SetTrigger("Attack 1");
        

    }

}
