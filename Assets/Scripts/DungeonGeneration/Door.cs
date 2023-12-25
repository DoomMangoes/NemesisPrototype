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

    public DoorType doorType;
   
    MeshRenderer _renderer;
    public Room currentRoom;

    [SerializeField]
    Material openDoorMaterial;
    [SerializeField]
    Material lockedDoorMaterial;

    void Awake(){
        _renderer = GetComponent<MeshRenderer>();
        currentRoom = gameObject.transform.parent.gameObject.GetComponent<Room>();
    }
    
     void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player"){

            if(!currentRoom.isPlayerInRoom && !currentRoom.isRoomClear)
                gameObject.SetActive(false);

            if(currentRoom.isRoomClear)
                gameObject.SetActive(false);
        }
    }

    public void SetLocked(){

        if(_renderer){
            _renderer.material = lockedDoorMaterial;
        }
    }

    public void SetOpen(){

        if(_renderer){
            _renderer.material = openDoorMaterial;
        }
    }
}
