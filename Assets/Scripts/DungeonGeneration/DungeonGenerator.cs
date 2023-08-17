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

         
            RoomController.instance.LoadRoom("Empty", roomLocation.x, roomLocation.y);

        }

        //RoomController.instance.checkDoors = true;
        //RoomController.instance.CheckUnusedDoors();
    }
  
    
}
