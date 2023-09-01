using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Direction {

    up = 0,

    left = 1,

    down = 2,

    right = 3


};

public class DungeonCrawlerController : MonoBehaviour
{
    public static List<Vector2> positionsVisited = new List<Vector2>();

    private static readonly Dictionary<Direction, Vector2> directionMovementMap = new Dictionary<Direction, Vector2>{
        
        {Direction.up, Vector2.up},
        {Direction.left, Vector2.left},
        {Direction.down, Vector2.down},
        {Direction.right, Vector2.right}
    };

    public static List<Vector2> GenerateDungeon(DungeonGenerationData dungeonData){

        List<DungeonCrawler> dungeonCrawlers = new List<DungeonCrawler>();

        for(int i = 0; i< dungeonData.numberOfCrawlers; i++){

            dungeonCrawlers.Add(new DungeonCrawler(Vector2.zero));
        }

        int iterations = Random.Range(dungeonData.iterationMin, dungeonData.iterationMax);

        for(int i = 0; i < iterations; i++){

            foreach(DungeonCrawler dungeonCrawler in dungeonCrawlers){
                Vector2 newPos = dungeonCrawler.Move(directionMovementMap);
                positionsVisited.Add(newPos);
            }
        }

        return positionsVisited;
    }
}
