using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCheck : MonoBehaviour
{
     public Room currentRoom;
     public event EventHandler OnPlayerEnterRoomTrigger;

    void Awake(){
        currentRoom = gameObject.transform.parent.gameObject.GetComponent<Room>();
    }
   
     void OnTriggerEnter(Collider other) {

        if(other.tag == "Player"){
           OnPlayerEnterRoomTrigger?.Invoke(this, EventArgs.Empty);
        }
     
    }

      void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player" && currentRoom.isPlayerInRoom){
            currentRoom.isPlayerInRoom = false;
        }

    }
}
