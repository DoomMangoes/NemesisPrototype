using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public DungeonGenerationData dungeonGenerationData;

    private List<Vector2> dungeonRooms;

    //private bool largeRoomSpawnRight;

    private void Start(){

        dungeonRooms = DungeonCrawlerController.GenerateDungeon(dungeonGenerationData);

        SpawnRooms(dungeonRooms);
    }


    private void SpawnRooms(IEnumerable<Vector2> rooms){


        RoomController.instance.LoadRoom("Empty", 0f, 0f);

        //Testing Large Room
        //RoomController.instance.LoadRoom("Large", -0.25f, 0.25f);

        foreach(Vector2 roomLocation in rooms){

            /*
            bool largeSpawn = CheckLargeRoomSpawnLocation(roomLocation);


            
            if(largeSpawn){
                SpawnLargeRoom(roomLocation);
                
            }else{
                RoomController.instance.LoadRoom("Empty", roomLocation.x, roomLocation.y);
            }
            */
            RoomController.instance.LoadRoom("Empty", roomLocation.x, roomLocation.y);

        }

        //RoomController.instance.checkDoors = true;
        //RoomController.instance.CheckUnusedDoors();
    }
    /*

    private bool CheckLargeRoomSpawnLocation(Vector2 roomLocation){

        bool check = false;
        int i;
        bool find = false;
        //bool exist = false;


        //Check Top
        if(dungeonRooms.Find( room => room.x == roomLocation.x + 1 && room.y == roomLocation.y) == null){

            //Check Right
            i = 0;
            while(check == false && i < 2){

                find = dungeonRooms.Find( room => room.x == roomLocation.x && room.y == roomLocation.y + i) == null;
                
                if(find){

                check = true; 
                largeRoomSpawnRight = true;
                }
                else{
                    check = false;
                }

            }   

           if(check == false){

                //Check Left
                i = 0;
                while(check == false && i < 2){

                    find = dungeonRooms.Find( room => room.x == roomLocation.x && room.y == roomLocation.y - i) == null;
                    
                    if(find){

                    check = true; 
                    largeRoomSpawnRight = false;
                    }
                    else{
                        check = false;
                    }

                }   

           }

        }

        return check;
        
    }

    private void SpawnLargeRoom(Vector2 roomLocation){

        if(largeRoomSpawnRight){
            RoomController.instance.LoadLargeRoom("Large", roomLocation.x + 0.25f, roomLocation.y + 0.25f);

            dungeonRooms.Remove(new Vector2(roomLocation.x + 1, roomLocation.y));
            dungeonRooms.Remove(new Vector2(roomLocation.x + 1, roomLocation.y + 1));
            dungeonRooms.Remove(new Vector2(roomLocation.x , roomLocation.y + 1));

             

        }else{
            RoomController.instance.LoadLargeRoom("Large", roomLocation.x - 0.25f, roomLocation.y + 0.25f);

            dungeonRooms.Remove(new Vector2(roomLocation.x - 1, roomLocation.y));
            dungeonRooms.Remove(new Vector2(roomLocation.x - 1, roomLocation.y + 1));
            dungeonRooms.Remove(new Vector2(roomLocation.x , roomLocation.y + 1));

        }
        
       

    }

    */
    
}
