using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    //Changed Y to Z

    public Room(float x, float z){
        X = x;
        Z = z;
    }

    public float Width, Height, X, Z;


    public Door leftDoor, rightDoor, topDoor, bottomDoor;

   

    public List<Door> doors = new List<Door>();

    public Wall leftWall, rightWall, topWall, bottomWall;
    public List<Wall> walls = new List<Wall>();

    private bool updatedDoors = false;

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
        Wall[] ws = GetComponentsInChildren<Wall>();

        foreach(Wall w in ws){

            walls.Add(w);
            switch(w.wallType){

                case Wall.WallType.right:
                rightWall = w;
                break;
                case Wall.WallType.left:
                leftWall = w;
                break;
                case Wall.WallType.top:
                topWall = w;
                break;
                case Wall.WallType.bottom:
                bottomWall = w;
                break;
            }
        }

        RoomController.instance.RegisterRoom(this);
    }

    void Update(){


        if(name.Contains("End") && !updatedDoors){
            RemoveUnconnectedDoors();
            ActivateWalls();
            updatedDoors = true;
        }


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
    
    public void ActivateWalls(){

        foreach(Wall wall in walls){

            switch(wall.wallType){

                case Wall.WallType.right:
                    if(GetRight() == null){
                        DeactivatePreviousWall(wall);
                        wall.gameObject.SetActive(true);
                       
                    }
                    else{
                        wall.gameObject.SetActive(false);
                    }
                break;
                case Wall.WallType.left:
                if(GetLeft() == null){
                        DeactivatePreviousWall(wall);
                         wall.gameObject.SetActive(true);
                       
                    } else{
                        wall.gameObject.SetActive(false);
                    }
                break;
                case Wall.WallType.top:
                if(GetTop() == null){
                    DeactivatePreviousWall(wall);
                  wall.gameObject.SetActive(true);
                    } else{
                        wall.gameObject.SetActive(false);
                    }
                break;
                case Wall.WallType.bottom:
               if(GetBottom() == null){
                DeactivatePreviousWall(wall);
                wall.gameObject.SetActive(true);
                    } else{
                        wall.gameObject.SetActive(false);
                    }
                break;
            }
        }
    }
    

    
    void DeactivatePreviousWall(Wall wall){

        Room tempRoom;
        switch(wall.wallType){

                case Wall.WallType.right:
                  
                   if(GetLeft() != null){
                     tempRoom = GetLeft();
                     tempRoom.DeactivateWalls();
                   }
                   
                break;
                case Wall.WallType.left:
                   
                   if(GetRight() != null){
                     tempRoom = GetRight();
                     tempRoom.DeactivateWalls();
                   }
                break;
                case Wall.WallType.top:
               
                   if(GetBottom() != null){
                      tempRoom = GetBottom();
                     tempRoom.DeactivateWalls();
                   }
                break;
                case Wall.WallType.bottom:
                    
                   if(GetTop() != null){
                    tempRoom = GetTop();
                   tempRoom.DeactivateWalls();
                   }
                break;

        }

    }

    public void DeactivateWalls(){

        foreach(Wall wall in walls){

            switch(wall.wallType){

                case Wall.WallType.right:
                    if(GetRight() != null){
                        wall.gameObject.SetActive(false);
                       
                    }
                break;
                case Wall.WallType.left:
                if(GetLeft() != null){
                         
                        wall.gameObject.SetActive(false);
                       
                    }
                break;
                case Wall.WallType.top:
                if(GetTop() != null){
                    
                        wall.gameObject.SetActive(false);
                         
                    }
                break;
                case Wall.WallType.bottom:
               if(GetBottom() != null){
                 
                        wall.gameObject.SetActive(false);
                    }
                break;
            }
        }
    }
}
