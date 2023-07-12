using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCameraController : MonoBehaviour
{

    public static RoomCameraController instance;
    public Room currRoom;
    public float moveSpeedWhenRoomChange;

    void Awake(){

        instance  = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition(){

        if(currRoom == null){

            return;
        }

        Vector3 targetPos = GetCameraTargetPosition();

        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeedWhenRoomChange);
    }

    Vector3 GetCameraTargetPosition(){

        if(currRoom == null){

            return Vector3.zero;
        }


        Vector3 targetPos = currRoom.GetRoomCenter();

        //Changed Z to Y
        targetPos.y = transform.position.y;

        //Tweaked Camera Position for Isometric View
        targetPos.z =  targetPos.z - 5;
        targetPos.x =  targetPos.x - 5;

        return targetPos;
    }

    public bool isSwitchingScene(){

        return transform.position.Equals(GetCameraTargetPosition()) == false;
    }
}
