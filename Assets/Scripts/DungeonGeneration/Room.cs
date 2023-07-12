using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //Changed Y to Z

    public int Width, Height, X, Z;
    // Start is called before the first frame update
    void Start()
    {
        if(RoomController.instance == null){
            Debug.Log("Wrong Scene");
            return;
        }

        RoomController.instance.RegisterRoom(this);
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
}
