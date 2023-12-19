using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCheck : MonoBehaviour
{
     public Room currentRoom;

    void Awake(){
        currentRoom = gameObject.transform.parent.gameObject.GetComponent<Room>();
    }
   
     void OnTriggerEnter(Collider other) {
        
        //If Player enters room, lock room doors
        if(other.tag == "Player"){
            
            currentRoom.isPlayerInRoom = true;
            currentRoom.LockRoom();
        }

    }

      void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player"){
            currentRoom.isPlayerInRoom = false;
        }
    }
}
