using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class RoomInfo{

    public string name;

    public float X;


    //Changed to Z;
    public float Z;

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

    bool spawnedBossRoom = false;

    bool updatedRooms = false;

    private bool largeRoomSpawnRight;
    private bool largeRoomSpawnTop;

    private bool uniqueRoomSpawn = false;

    private bool largeRoomSpawn = false;

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
    }

    void UpdateRoomQueue(){

        if(isLoadingRoom){
            return;

        }

        //Update Rooms
        if(loadRoomQueue.Count != 0){
            
            foreach(Room room in loadedRooms){

                    room.RemoveUnconnectedDoors();
                    room.ActivateWalls();
                    
            }
            updatedRooms = true;

        }


        //Spawn Boss Room And Unique Rooms
        if(loadRoomQueue.Count == 0){

            /*
            if(uniqueRoomSpawn == false){
                 foreach(Room room in loadedRooms){

                
                bool largeSpawn = CheckLargeRoomSpawnLocation(room);
                Debug.Log(largeSpawn);


                if(largeSpawn && !largeRoomSpawn){
                    largeRoomSpawn = true;
                    StartCoroutine(SpawnLargeRoom(room));
                    largeRoomSpawn = false;
                }

                    room.RemoveUnconnectedDoors();
                    room.ActivateWalls();

            }
                uniqueRoomSpawn = true;
                Debug.Log("Room Spawn Done");
            }

            
        */
           
            if(!spawnedBossRoom /*&& uniqueRoomSpawn */){
                StartCoroutine(SpawnBossRoom());
            } else if(spawnedBossRoom && !updatedRooms){

                foreach(Room room in loadedRooms){
                    room.RemoveUnconnectedDoors();
                    room.ActivateWalls();
                    
                }
                updatedRooms = true;
            }

          
                    

            return;
        }

       

        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;
        updatedRooms = false;

        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }   

    public void LoadRoom(string name, float x, float z){

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
            if(currentLoadRoomData.name == "Large"){
                float width = 0;
                float height = 0;

                 if(largeRoomSpawnRight){
                        width = -10;
                    }else if(largeRoomSpawnRight == false){
                         width = 10;
                    }
                
                    if(largeRoomSpawnTop){
                        height = -10;
                    }else if(largeRoomSpawnTop == false){
                        height = 10;
                    }
        

                room.transform.position = new Vector3( 
                (currentLoadRoomData.X * 20) + width,
                0,
                (currentLoadRoomData.Z * 20) + height
            );
            }
            else{
                room.transform.position = new Vector3( 
                currentLoadRoomData.X * room.Width,
                0,
                currentLoadRoomData.Z * room.Height
            );
            }
            

            room.X = currentLoadRoomData.X;
            room.Z = currentLoadRoomData.Z;
            room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.X + ", " + room.Z;
            room.transform.parent = transform;

            isLoadingRoom = false;

            if(loadedRooms.Count == 0){

                RoomCameraController.instance.currRoom = room;
            }


            loadedRooms.Add(room);
            //room.RemoveUnconnectedDoors();
            //room.ActivateWalls();
        }
        else
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }
    }

    public bool DoesRoomExist(float x, float z){

        //Changed to Z;

        return loadedRooms.Find( item => item.X == x && item.Z == z) != null;
    }

     public Room FindRoom(float x, float z){

        //Changed to Z;

        return loadedRooms.Find( item => item.X == x && item.Z == z);
    }

    public void OnPlayerEnterRoom(Room room){

        RoomCameraController.instance.currRoom = room;
        currRoom = room;
    }

    IEnumerator SpawnBossRoom(){
        
        spawnedBossRoom = true;
        yield return new WaitForSeconds(0.5f);

        if(loadRoomQueue.Count == 0){
            Room bossRoom = loadedRooms[loadedRooms.Count - 1];
            Room tempRoom = new Room(bossRoom.X, bossRoom.Z);
            Destroy(bossRoom.gameObject);
            var roomToRemove = loadedRooms.Single( r => r.X == tempRoom.X && r.Z == tempRoom.Z);
            loadedRooms.Remove(roomToRemove);

            LoadRoom("Large", tempRoom.X, tempRoom.Z);
            //LoadRoom("End", tempRoom.X, tempRoom.Z);

        }
    }

    private bool CheckLargeRoomSpawnLocation(Room currentRoom){

        bool check = false;
        int i;
        Room find = null;
        bool nearSpawn = false;

        //Check If Start Room
            Debug.Log("Check Spawn Potential");
            Debug.Log("CurrentRoom X:" + currentRoom.X);
            Debug.Log("CurrentRoom X:" + currentRoom.Z);
        bool test = currentRoom.X != 0 && currRoom.Z != 0 ;
            Debug.Log("Test:" + test);
        if(currentRoom.X != 0 && currentRoom.Z != 0 ){
            Debug.Log("Spawn Check Good");
            //Check Top
            find = loadedRooms.Find( room => room.X == currentRoom.X && room.Z == currentRoom.Z + 1);

            if(find){
                nearSpawn = find.X == 0 && find.Z == 0;
                Debug.Log("Top Room Free");
            }else if(find == null){
                Debug.Log("Top Room Does Not Exist");
            }
            
            
            if(nearSpawn){
                find = null;
            }else{
                largeRoomSpawnTop = true;
            }

            //Check Bottom
            if(find == null){
                find = loadedRooms.Find( room => room.X == currentRoom.X && room.Z == currentRoom.Z - 1);

                if(find){
                nearSpawn = find.X == 0 && find.Z == 0;
                Debug.Log("Bottom Room Free");
                }
                if(nearSpawn){
                    find = null;
                }else{
                largeRoomSpawnTop = false;
                }
            }

        }

        if(find != null){

            //Check Right
            i = 0;
            while(check == false && i < 2){

                find = loadedRooms.Find( room => room.X == currentRoom.X + i&& room.Z == currentRoom.Z);
                nearSpawn = find.X == 0 && find.Z == 0;
                
                if(find != null && nearSpawn == false){

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

                    find = loadedRooms.Find( room => room.X == currentRoom.X - i && room.Z == currentRoom.Z );
                    nearSpawn = find.X == 0 && find.Z == 0;
                    
                    if(find != null && nearSpawn == false){

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

    IEnumerator SpawnLargeRoom(Room currentRoom){

        yield return new WaitForSeconds(0.5f);

        Room largeRoom = currentRoom;
        Room tempRoom = new Room(largeRoom.X, largeRoom.Z);
        Destroy(largeRoom.gameObject);

        var roomToRemove = loadedRooms.Single( r => r.X == tempRoom.X && r.Z == tempRoom.Z);
        loadedRooms.Remove(roomToRemove);
        

           
        if(largeRoomSpawnTop){
            
        roomToRemove = loadedRooms.Single( r => r.X == tempRoom.X && r.Z == tempRoom.Z + 1);
        Destroy(roomToRemove.gameObject);
        loadedRooms.Remove(roomToRemove);
        
        }else if(largeRoomSpawnTop == false){
            
        roomToRemove = loadedRooms.Single( r => r.X == tempRoom.X && r.Z == tempRoom.Z - 1);
         Destroy(roomToRemove.gameObject);
        loadedRooms.Remove(roomToRemove);
    
        }

        if(largeRoomSpawnRight){
            roomToRemove = loadedRooms.Single( r => r.X == tempRoom.X + 1 && r.Z == tempRoom.Z);
             Destroy(roomToRemove.gameObject);
        loadedRooms.Remove(roomToRemove);
        }else if(largeRoomSpawnRight == false){
            
        roomToRemove = loadedRooms.Single( r => r.X == tempRoom.X - 1 && r.Z == tempRoom.Z);
        Destroy(roomToRemove.gameObject);
        loadedRooms.Remove(roomToRemove);
    
        }

        LoadLargeRoom("Large", tempRoom.X, tempRoom.Z);
        
    }
    
    public void LoadLargeRoom(string name, float x, float z){

        if(DoesRoomExist(x,z)){
            return;
        }

        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        
        if(largeRoomSpawnRight){
            newRoomData.X = x + 1;
        }else if(largeRoomSpawnRight == false){
            newRoomData.X = x - 1;
        }
       
        if(largeRoomSpawnTop){
            newRoomData.Z = z + 1;
        }else if(largeRoomSpawnTop == false){
            newRoomData.Z = z - 1;
        }
       

        loadRoomQueue.Enqueue(newRoomData);

    }
}
