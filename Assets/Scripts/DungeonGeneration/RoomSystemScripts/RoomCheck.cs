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
        
        //If Player enters room and room is not cleared, lock room doors
        if(other.tag == "Player" && !currentRoom.isPlayerInRoom && !currentRoom.isRoomClear){
            
            currentRoom.isPlayerInRoom = true;
            currentRoom.LockRoom();
        }

        //If enemies are in room keep it locked
        if(other.tag == "Enemy" && !currentRoom.isEnemyInRoom){
             currentRoom.isEnemyInRoom = true;
        }

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
