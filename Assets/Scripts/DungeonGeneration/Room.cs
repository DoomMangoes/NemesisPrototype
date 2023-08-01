using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //Changed Y to Z

    public int Width, Height, X, Z;


    public Door leftDoor, rightDoor, topDoor, bottomDoor;

    public List<Door> doors = new List<Door>();

    // Start is called before the first frame update
    void Start()
    {
        if(RoomController.instance == null){
            Debug.Log("Wrong Scene");
            return;
        }

        Door[] ds = GetComponentsInChildren<Door>();

        foreach(Door d in ds){

            doors.Add(d);
            switch(d.doorType){

                case Door.DoorType.right:
                rightDoor = d;
                break;
                case Door.DoorType.left:
                leftDoor = d;
                break;
                case Door.DoorType.top:
                topDoor = d;
                break;
                case Door.DoorType.bottom:
                bottomDoor = d;
                break;
            }
        }

        RoomController.instance.RegisterRoom(this);
    }

    public void RemoveUnconnectedDoors(){

        foreach(Door door in doors){

            switch(door.doorType){

                case Door.DoorType.right:
                    if(GetRight() == null){
                         ActivatePreviousDoor(door);
                        door.gameObject.SetActive(false);
                       
                    }
                break;
                case Door.DoorType.left:
                if(GetLeft() == null){
                         ActivatePreviousDoor(door);
                        door.gameObject.SetActive(false);
                       
                    }
                break;
                case Door.DoorType.top:
                if(GetTop() == null){
                    ActivatePreviousDoor(door);
                        door.gameObject.SetActive(false);
                         
                    }
                break;
                case Door.DoorType.bottom:
               if(GetBottom() == null){
                 ActivatePreviousDoor(door);
                        door.gameObject.SetActive(false);
                        
                    }
                break;
            }
        }
    }

    public Room GetRight(){

        if(RoomController.instance.DoesRoomExist(X + 1 , Z)){
            return RoomController.instance.FindRoom(X + 1, Z);
           

        } else {
            return null;
        }
    }

    public Room GetLeft(){

        if(RoomController.instance.DoesRoomExist(X - 1 , Z)){
            return RoomController.instance.FindRoom(X - 1, Z);
           

        } else {
            return null;
        }
    }

    public Room GetTop(){

        if(RoomController.instance.DoesRoomExist(X , Z + 1)){
            return RoomController.instance.FindRoom(X, Z + 1);
           

        } else {
            return null;
        }
    }

    public Room GetBottom(){

        if(RoomController.instance.DoesRoomExist(X , Z - 1)){
            return RoomController.instance.FindRoom(X, Z - 1);
           

        } else {
            return null;
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, 0, Height));
    }

    public Vector3 GetRoomCenter(){

        return new Vector3(X * Width, 0,Z * Height);
    }

    void OnTriggerEnter(Collider other) {
        
        if(other.tag == "Player"){
            RoomController.instance.OnPlayerEnterRoom(this);
        }
    }

    void ActivatePreviousDoor(Door door){


        

        Room tempRoom;
        switch(door.doorType){

                case Door.DoorType.right:
                  
                   if(GetLeft() != null){
                     tempRoom = GetLeft();
                     tempRoom.ReactivateDoors();
                   }
                   
                break;
                case Door.DoorType.left:
                   
                   if(GetRight() != null){
                     tempRoom = GetRight();
                     tempRoom.ReactivateDoors();
                   }
                break;
                case Door.DoorType.top:
               
                   if(GetBottom() != null){
                      tempRoom = GetBottom();
                     tempRoom.ReactivateDoors();
                   }
                break;
                case Door.DoorType.bottom:
                    
                   if(GetTop() != null){
                    tempRoom = GetTop();
                    tempRoom.ReactivateDoors();
                   }
                break;

        }





        /*

        Room tempRoom;
        switch(door.doorType){

                case Door.DoorType.right:
                  
                   if(GetLeft() != null){
                     tempRoom = GetLeft();
                     Door tempDoor = tempRoom.doors.Find( roomDoor => roomDoor.doorType.ToString() == "left");
                     tempDoor.gameObject.SetActive(true);
                   }
                   
                break;
                case Door.DoorType.left:
                   
                   if(GetRight() != null){
                     tempRoom = GetRight();
                     Door tempDoor = tempRoom.doors.Find( roomDoor => roomDoor.doorType.ToString() == "right");
                     tempDoor.gameObject.SetActive(true);
                   }
                break;
                case Door.DoorType.top:
               
                   if(GetBottom() != null){
                      tempRoom = GetBottom();
                     Door tempDoor = tempRoom.doors.Find( roomDoor => roomDoor.doorType.ToString() == "bottom");
                     tempDoor.gameObject.SetActive(true);
                   }
                break;
                case Door.DoorType.bottom:
                    
                   if(GetTop() != null){
                    tempRoom = GetTop();
                     Door tempDoor = tempRoom.doors.Find( roomDoor => roomDoor.doorType.ToString() == "top");
                     tempDoor.gameObject.SetActive(true);
                   }
                break;

        }
        */

    }

    public void ReactivateDoors(){

        foreach(Door door in doors){

            switch(door.doorType){

                case Door.DoorType.right:
                    if(GetRight() != null){
                        door.gameObject.SetActive(true);
                       
                    }
                break;
                case Door.DoorType.left:
                if(GetLeft() != null){
                         
                        door.gameObject.SetActive(true);
                       
                    }
                break;
                case Door.DoorType.top:
                if(GetTop() != null){
                    
                        door.gameObject.SetActive(true);
                         
                    }
                break;
                case Door.DoorType.bottom:
               if(GetBottom() != null){
                 
                        door.gameObject.SetActive(true);
                    }
                break;
            }
        }
    }
}
