using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorType{

        left,
        right,
        top,
        bottom,
        leftB,
        rightB,
        topB,
        bottomB,

    }

    public enum State{
        Open,
        Locked,
    }

    public DoorType doorType;
   
    MeshRenderer _renderer;
    public Room currentRoom;

    [SerializeField]
    Material openDoorMaterial;
    [SerializeField]
    Material lockedDoorMaterial;

    public State state;

    void Awake(){
        _renderer = GetComponent<MeshRenderer>();
        currentRoom = gameObject.transform.parent.gameObject.GetComponent<Room>();
        state = State.Open;
    }
    
     void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player"){

            if(state == State.Open)
                gameObject.SetActive(false);
            
        }
    }

    public void SetLocked(){

        state = State.Locked;
        if(_renderer){
            _renderer.material = lockedDoorMaterial;
        }
    }

    public void SetOpen(){

        state = State.Open;

        if(_renderer){
            _renderer.material = openDoorMaterial;
        }
    }
}
