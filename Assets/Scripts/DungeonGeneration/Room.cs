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

    //Large Room Variables

    public Door leftDoorB, rightDoorB, topDoorB, bottomDoorB;
    public Wall leftWallB, rightWallB, topWallB, bottomWallB;


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

                //Large Room Doors
                case Door.DoorType.rightB:
                rightDoorB = d;
                break;
                case Door.DoorType.leftB:
                leftDoorB = d;
                break;
                case Door.DoorType.topB:
                topDoorB = d;
                break;
                case Door.DoorType.bottomB:
                bottomDoorB = d;
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

                //Large Room Walls
                case Wall.WallType.rightB:
                rightWallB = w;
                break;
                case Wall.WallType.leftB:
                leftWallB = w;
                break;
                case Wall.WallType.topB:
                topWallB = w;
                break;
                case Wall.WallType.bottomB:
                bottomWallB = w;
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

                // Large Room Doors

                case Door.DoorType.rightB:
                    if(GetRight() == null){
                        ActivatePreviousDoor(door);
                        door.gameObject.SetActive(false);
                        
                    }
                break;
                case Door.DoorType.leftB:
                if(GetLeft() == null){
                         ActivatePreviousDoor(door);
                        door.gameObject.SetActive(false);
                       
                    }
                break;
                case Door.DoorType.topB:
                if(GetTop() == null){
                    ActivatePreviousDoor(door);
                        door.gameObject.SetActive(false);
                         
                    }
                break;
                case Door.DoorType.bottomB:
               if(GetBottom() == null){
                 ActivatePreviousDoor(door);
                        door.gameObject.SetActive(false);
                        
                    }
                break;
            }
        }
    }

    public Room GetRight(){

        Room find = null;
        //Normal find Normal
        if(RoomController.instance.DoesRoomExist(X + 1 , Z)){
           find = RoomController.instance.FindRoom(X + 1, Z);
        
        }
        //Normal find Large Door B or Large Door A Find Normal
        if(find == null && RoomController.instance.DoesRoomExist(X + 1.5f , Z + 0.5f)){
            find = RoomController.instance.FindRoom(X + 1.5f, Z + 0.5f);
        }
        //Normal find Large Door A or Large Door B Find Normal
        if(find == null && RoomController.instance.DoesRoomExist(X + 1.5f , Z - 0.5f)){
            find = RoomController.instance.FindRoom(X + 1.5f, Z - 0.5f);
        }


        //Large Find Large
        if(find == null && RoomController.instance.DoesRoomExist(X + 2f , Z)){
            find = RoomController.instance.FindRoom(X + 2f, Z);
        }
        //Large A Find Large A
        if(find == null && RoomController.instance.DoesRoomExist(X + 2f , Z + 0.5f)){
            find = RoomController.instance.FindRoom(X + 2f, Z + 0.5f);
        }
        //Large B Find Large B
        if(find == null && RoomController.instance.DoesRoomExist(X + 2f , Z - 0.5f)){
            find = RoomController.instance.FindRoom(X + 2f, Z - 0.5f);
        }
      
        if(find == null && X == 0.5f && Z == 1.5f){
            Debug.Log("----------No Right Room Found!");
        }
      
        return find;
    }

    public Room GetLeft(){

        Room find = null;
        if(RoomController.instance.DoesRoomExist(X - 1 , Z)){
           find = RoomController.instance.FindRoom(X - 1, Z);
        }
        if(find == null && RoomController.instance.DoesRoomExist(X - 1.5f , Z + 0.5f)){
            find = RoomController.instance.FindRoom(X - 1.5f, Z + 0.5f);
        }
        //Normal Left Door Find Large Door Right A
        if(find == null && RoomController.instance.DoesRoomExist(X - 1.5f , Z - 0.5f)){
            find = RoomController.instance.FindRoom(X - 1.5f, Z - 0.5f);
        }

        //Large Find Large
        if(find == null && RoomController.instance.DoesRoomExist(X - 2f , Z)){
            find = RoomController.instance.FindRoom(X - 2f, Z);
        }
        //Large A Find Large A
        if(find == null && RoomController.instance.DoesRoomExist(X - 2f , Z + 0.5f)){
            find = RoomController.instance.FindRoom(X - 2f, Z + 0.5f);
        }
        //Large B Find Large B
        if(find == null && RoomController.instance.DoesRoomExist(X - 2f , Z - 0.5f)){
            find = RoomController.instance.FindRoom(X - 2f, Z - 0.5f);
        }

        if(find == null && X == 2 && Z == 2){
            Debug.Log("----------No Left Room Found!");
            Debug.Log("X:" + X + " Z:" + Z);
            Debug.Log("X:" + (X-1.5f) + " Z:" + (Z-0.5f));
        }

        return find;
        
    }

    public Room GetTop(){

        Room find = null;
        if(RoomController.instance.DoesRoomExist(X , Z + 1)){
           find = RoomController.instance.FindRoom(X, Z + 1);
        
        }
        if(find == null && RoomController.instance.DoesRoomExist(X + 0.5f , Z + 1.5f)){
            find = RoomController.instance.FindRoom(X + 0.5f, Z + 1.5f);
        }
        if(find == null && RoomController.instance.DoesRoomExist(X - 0.5f , Z + 1.5f)){
            find = RoomController.instance.FindRoom(X - 0.5f, Z + 1.5f);
        }

        //Large Find Large
        if(find == null && RoomController.instance.DoesRoomExist(X , Z + 2f)){
            find = RoomController.instance.FindRoom(X, Z + 2f);
        }
        //Large A Find Large A
        if(find == null && RoomController.instance.DoesRoomExist(X - 0.5f , Z + 2f)){
            find = RoomController.instance.FindRoom(X - 0.5f, Z + 2f);
        }

        //Large B Find Large B
        if(find == null && RoomController.instance.DoesRoomExist(X + 0.5f , Z + 2f)){
            find = RoomController.instance.FindRoom(X + 0.5f, Z + 2f);
        }

        return find;
     
    }

    public Room GetBottom(){
        
        Room find = null;
        if(RoomController.instance.DoesRoomExist(X , Z - 1)){
           find = RoomController.instance.FindRoom(X, Z - 1);
        
        }
        if(find == null && RoomController.instance.DoesRoomExist(X + 0.5f , Z - 1.5f)){
            find = RoomController.instance.FindRoom(X + 0.5f, Z - 1.5f);
        }
        if(find == null && RoomController.instance.DoesRoomExist(X - 0.5f , Z - 1.5f)){
            find = RoomController.instance.FindRoom(X - 0.5f, Z - 1.5f);
        }

        //Large Find Large
        if(find == null && RoomController.instance.DoesRoomExist(X , Z - 2f)){
            find = RoomController.instance.FindRoom(X, Z - 2f);
        }
        //Large A Find Large A
        if(find == null && RoomController.instance.DoesRoomExist(X - 0.5f , Z - 2f)){
            find = RoomController.instance.FindRoom(X - 0.5f, Z - 2f);
        }
        //Large B Find Large B
        if(find == null && RoomController.instance.DoesRoomExist(X + 0.5f , Z - 2f)){
            find = RoomController.instance.FindRoom(X + 0.5f, Z - 2f);
        }

        return find;
        
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

                //Large Room Doors
                case Door.DoorType.rightB:
                  
                   if(GetLeft() != null){
                     tempRoom = GetLeft();
                     tempRoom.ReactivateDoors();
                   }
                   
                break;
                case Door.DoorType.leftB:
                   
                   if(GetRight() != null){
                     tempRoom = GetRight();
                     tempRoom.ReactivateDoors();
                   }
                break;
                case Door.DoorType.topB:
               
                   if(GetBottom() != null){
                      tempRoom = GetBottom();
                     tempRoom.ReactivateDoors();
                   }
                break;
                case Door.DoorType.bottomB:
                    
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

                //Large Room Doors
                case Door.DoorType.rightB:
                    if(GetRight() != null){
                        door.gameObject.SetActive(true);
                       
                    }
                break;
                case Door.DoorType.leftB:
                if(GetLeft() != null){
                         
                        door.gameObject.SetActive(true);
                       
                    }
                break;
                case Door.DoorType.topB:
                if(GetTop() != null){
                    
                        door.gameObject.SetActive(true);
                         
                    }
                break;
                case Door.DoorType.bottomB:
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

                // Large Room Walls

                case Wall.WallType.rightB:
                    if(GetRight() == null){
                        DeactivatePreviousWall(wall);
                        wall.gameObject.SetActive(true);
                       
                    }
                    else{
                        wall.gameObject.SetActive(false);
                    }
                    break;
                case Wall.WallType.leftB:
                    if(GetLeft() == null){
                        DeactivatePreviousWall(wall);
                        wall.gameObject.SetActive(true);
                       
                    } else{
                        wall.gameObject.SetActive(false);
                    }
                    break;
                case Wall.WallType.topB:
                    if(GetTop() == null){
                        DeactivatePreviousWall(wall);
                        wall.gameObject.SetActive(true);
                    } else{
                        wall.gameObject.SetActive(false);
                    }
                    break;
                case Wall.WallType.bottomB:
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

                // Large Room Walls

                case Wall.WallType.rightB:
                  
                   if(GetLeft() != null){
                     tempRoom = GetLeft();
                     tempRoom.DeactivateWalls();
                   }
                   
                break;
                case Wall.WallType.leftB:
                   
                   if(GetRight() != null){
                     tempRoom = GetRight();
                     tempRoom.DeactivateWalls();
                   }
                break;
                case Wall.WallType.topB:
               
                   if(GetBottom() != null){
                      tempRoom = GetBottom();
                     tempRoom.DeactivateWalls();
                   }
                break;
                case Wall.WallType.bottomB:
                    
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

                // Large Room Walls

                case Wall.WallType.rightB:
                    if(GetRight() != null){
                        wall.gameObject.SetActive(false);
                       
                    }
                break;
                case Wall.WallType.leftB:
                if(GetLeft() != null){
                         
                        wall.gameObject.SetActive(false);
                       
                    }
                break;
                case Wall.WallType.topB:
                if(GetTop() != null){
                    
                        wall.gameObject.SetActive(false);
                         
                    }
                break;
                case Wall.WallType.bottomB:
               if(GetBottom() != null){
                 
                        wall.gameObject.SetActive(false);
                    }
                break;
            }
        }
    }
}
