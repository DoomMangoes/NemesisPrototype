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
           // Debug.Log(gameObject.name.ToString());
          //  Debug.Log("X: " + gameObject.transform.position.x.ToString());
            //Debug.Log("Z: " + gameObject.transform.position.z.ToString());
           
          //  Debug.Log("Triggered");
           OnPlayerEnterRoomTrigger?.Invoke(this, EventArgs.Empty);
        }
        /*
        
        //If Player enters room and room is not cleared, lock room doors
        if(other.tag == "Player" && !currentRoom.isPlayerInRoom && !currentRoom.isRoomClear){
            
            currentRoom.isPlayerInRoom = true;
            currentRoom.LockRoom();
        }

        //If enemies are in room keep it locked
        if(other.tag == "Enemy" && !currentRoom.isEnemyInRoom){
             currentRoom.isEnemyInRoom = true;
        }

        */
    }

      void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player" && currentRoom.isPlayerInRoom){
            currentRoom.isPlayerInRoom = false;
        }

        /*
        //Temporary Room Open System
        if(other.tag == "Enemy" && currentRoom.enemyCount > 0){
            currentRoom.enemyCount -= 1;

            if(currentRoom.enemyCount == 0){
                currentRoom.isEnemyInRoom = false;
                currentRoom.OpenRoom();
            }
                
        }
        
        */
    }
}
