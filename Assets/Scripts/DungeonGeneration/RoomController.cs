using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomInfo{

    public string name;

    public int X;


    //Changed to Z;
    public int Z;

}

public class RoomController : MonoBehaviour
{

    public static RoomController instance;

    string currentWorldName = "LevelOne";

    RoomInfo currentLoadRoomData;

    Room currRoom;

    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();

    public List<Room> loadedRooms = new List<Room>();

    bool isLoadingRoom = false;

    //public bool checkDoors = false;

    void Awake() {
        instance = this;
    }

    void Start(){
        /*
        LoadRoom("Start",0,0);
        LoadRoom("Empty",1,0);
        LoadRoom("Empty",-1,0);
        LoadRoom("Empty",0,1);
        LoadRoom("Empty",0,-1);
        */
    }

    void Update() {
        UpdateRoomQueue();    

        /*
        if(checkDoors){
            CheckUnusedDoors();
            checkDoors = false;
        };
        */
    }

    void UpdateRoomQueue(){

        if(isLoadingRoom){
            return;

        }

        if(loadRoomQueue.Count == 0){
            return;
        }

        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;

        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }   

    public void LoadRoom(string name, int x, int z){

        if(DoesRoomExist(x,z)){
            return;
        }

        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.X = x;
        newRoomData.Z = z;

        loadRoomQueue.Enqueue(newRoomData);

    }

    IEnumerator LoadRoomRoutine(RoomInfo info){

        string roomName = currentWorldName + info.name;

        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while(loadRoom.isDone == false){

            yield return null;
        }
    }

    public void RegisterRoom(Room room){

        if(!DoesRoomExist(currentLoadRoomData.X, currentLoadRoomData.Z)){
            room.transform.position = new Vector3( 
                currentLoadRoomData.X * room.Width,
                0,
                currentLoadRoomData.Z * room.Height
            );

            room.X = currentLoadRoomData.X;
            room.Z = currentLoadRoomData.Z;
            room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.X + ", " + room.Z;
            room.transform.parent = transform;

            isLoadingRoom = false;

            if(loadedRooms.Count == 0){

                RoomCameraController.instance.currRoom = room;
            }


            loadedRooms.Add(room);
            room.RemoveUnconnectedDoors();
            room.ActivateWalls();
        }
        else
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }
    }

    public bool DoesRoomExist(int x, int z){

        //Changed to Z;

        return loadedRooms.Find( item => item.X == x && item.Z == z) != null;
    }

     public Room FindRoom(int x, int z){

        //Changed to Z;

        return loadedRooms.Find( item => item.X == x && item.Z == z);
    }

    public void OnPlayerEnterRoom(Room room){

        RoomCameraController.instance.currRoom = room;
        currRoom = room;
    }
    /*
    public void CheckUnusedDoors(){

        foreach(Room room in loadedRooms){
            room.RemoveUnconnectedDoors();
        }

    }
    */
    
}
