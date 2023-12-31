using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public DungeonGenerationData dungeonGenerationData;

    private List<Vector2> dungeonRooms;

    public GameObject playerPrefab;

    private GameObject player;

    private void Start(){

        dungeonRooms = DungeonCrawlerController.GenerateDungeon(dungeonGenerationData);

        SpawnRooms(dungeonRooms);

        player = Instantiate(playerPrefab, new Vector3(0, 1.5f, 0), Quaternion.identity);
        CamFollow.instance.target = player.transform;
           
    }


    private void SpawnRooms(IEnumerable<Vector2> rooms){


        RoomController.instance.LoadRoom("Start", 0f, 0f);

        foreach(Vector2 roomLocation in rooms){

         
            RoomController.instance.LoadRoom("Empty", roomLocation.x, roomLocation.y);

        }

        
    }
  
    
}
