using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCrawler : MonoBehaviour
{
    public Vector2 Position { get; set;}

    public DungeonCrawler(Vector2 startPos){
        Position = startPos;
    }

    public Vector2 Move(Dictionary<Direction, Vector2> directionMovementMap){

        Direction toMove = (Direction) Random.Range(0, directionMovementMap.Count);

        Position += directionMovementMap[toMove];

        return Position;
    }
}
