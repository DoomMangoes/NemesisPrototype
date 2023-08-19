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

    private List<Room> roomsToRemove = new List<Room>();

    //TESTING VARIABLES

    private List<Room> tempRoomSpawnList= new List<Room>();

    private List<Room> roomsToAdd = new List<Room>();

    private int index = 1;

    private int tempIndex = 0;

    private bool tempCheck = false; 

    private bool test = false;

    void Awake() {
        instance = this;
    }

    void Start(){
       
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
            
            UpdateExistingRooms();
            updatedRooms = true;

        }


        //Spawn Boss Room And Unique Rooms
        if(loadRoomQueue.Count == 0){

        /*
            if(!spawnedBossRoom && index == loadedRooms.Count - 1){
                StartCoroutine(SpawnBossRoom());
            } else if(spawnedBossRoom && !updatedRooms){

                foreach(Room room in loadedRooms){
                    Debug.Log(room.ToString());
                    room.RemoveUnconnectedDoors();
                    room.ActivateWalls();
                    
                }
                updatedRooms = true;
            }

        */
                    
        if(largeRoomSpawn == false && index < loadedRooms.Count - 1){
          
            if(loadedRooms[index] != null) {
                tempCheck = CheckLargeRoomSpawnLocation(loadedRooms[index]);
            }
         

            if(tempCheck){
                largeRoomSpawn = true;
            }
            else{
                index +=1;
            }
            Debug.Log("loadedRooms Index: " + index.ToString());
        }

        if(tempCheck && largeRoomSpawn){

            tempCheck = false;
            
            StartCoroutine(SpawnLargeRoom(loadedRooms[index]));

                foreach (var room in roomsToRemove)
                    {
                        Debug.Log(room.ToString());
                    }

                Debug.Log(loadRoomQueue.Count.ToString());
                
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
            float tempX = 0;
            float tempZ = 0;


            if(currentLoadRoomData.name == "Large"){
               
                 if(largeRoomSpawnRight){
                        tempX = 0.5f;
                    }else if(largeRoomSpawnRight == false){
                        tempX = -0.5f;
                    }
                
                    if(largeRoomSpawnTop){
                        tempZ = 0.5f;
                    }else if(largeRoomSpawnTop == false){
                        tempZ = -0.5f;
                    }
        

                room.transform.position = new Vector3( 
                (currentLoadRoomData.X * 20) + (tempX * 20),
                0,
                (currentLoadRoomData.Z * 20) + (tempZ * 20)
                );
                


            }
            else{
                room.transform.position = new Vector3( 
                currentLoadRoomData.X * room.Width,
                0,
                currentLoadRoomData.Z * room.Height
            );
            }
            
            if(currentLoadRoomData.name == "Large"){

                room.X = currentLoadRoomData.X + tempX;
                room.Z = currentLoadRoomData.Z + tempZ;
                room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.X + ", " + room.Z;
                room.transform.parent = transform;

            }else{
                room.X = currentLoadRoomData.X;
                room.Z = currentLoadRoomData.Z;
                room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.X + ", " + room.Z;
                room.transform.parent = transform;
            }
           

            isLoadingRoom = false;

            if(loadedRooms.Count == 0){

                RoomCameraController.instance.currRoom = room;
            }


            loadedRooms.Add(room);

            UpdateExistingRooms();
           
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

    

    IEnumerator SpawnLargeRoom(Room currentRoom){

        yield return new WaitForSeconds(0.2f);

        Room largeRoom = currentRoom;
        Room tempRoom = new Room(largeRoom.X, largeRoom.Z);
        
        foreach(Room room in roomsToRemove){

            Destroy(room.gameObject);
            var roomToRemove = loadedRooms.Single( r => r.X == room.X && r.Z == room.Z);
            loadedRooms.Remove(roomToRemove);

        }
           
        LoadRoom("Large", tempRoom.X, tempRoom.Z);
        
          Debug.Log("Before Clear:");
        foreach(Room room in roomsToRemove){
            Debug.Log(room);
        }
        
        roomsToRemove.Clear();

        largeRoomSpawn = false;
        index += 1;
        
       
    }
    
    private bool CheckLargeRoomSpawnLocation(Room currentRoom){

        bool check = false;
        Room find = null;
        bool test = false;
        List<Room> tempRoomList = new List<Room>();

        test = currentRoom.X != 0 && currentRoom.Z != 0 ;

            Debug.Log("Spawn Test:" + test);

        //If not Start Room Check Possible Room Spawn

        if(test == true){
            bool rand = Random.value >= 0.5f;

            for(int c = 0; c < 1; c++){

                if(rand){
                 


                check = CheckLargeRoomSpawnRight(check, currentRoom, tempRoomList);
                    if(check){
                        break;
                    }
                }
                if(rand == false){
                  
                check = CheckLargeRoomSpawnLeft(check, currentRoom, tempRoomList);
                    if(check){
                        break;
                    }
                }

                

            if(check == false && c == 1){
                rand = !rand;
            }

            }
            
        }
        
        Debug.Log("Check:" + check);

        if(check){
            foreach(Room room in tempRoomList){
                roomsToRemove.Add(room);
            }
        }

        return check;
        
    }

    private bool CheckLargeRoomSpawnRight(bool check, Room currentRoom, List<Room> tempRoomList){


          largeRoomSpawnRight = true;
          Room find;

            find = loadedRooms.Find( room => room.X == currentRoom.X && room.Z == currentRoom.Z);
            tempRoomList.Add(find);

            largeRoomSpawnTop = Random.value >= 0.5f;

           for(int c = 0; c < 2; c++){
                        //Check Top or Bottom Free Space
                        if(largeRoomSpawnTop){

                            find = loadedRooms.Find( room => room.X == currentRoom.X && room.Z == currentRoom.Z + 1);
       
                            if(find != null && find.X == 0 && find.Z == 0){
                                find = null;
                            }

                            if(find != null){
                               
                                Debug.Log("Top Room Free");
                                check = true;
                                tempRoomList.Add(find);
                                break;
                            }else if(find == null){
                                Debug.Log("Top Room Does Not Exist");
                            }
                            
                        }
                        if(largeRoomSpawnTop == false){
                            
                            find = loadedRooms.Find( room => room.X == currentRoom.X && room.Z == currentRoom.Z - 1);

                            if(find != null && find.X == 0 && find.Z == 0){
                                find = null;
                            }

                            if(find != null){
                                //nearSpawn = find.X == 0 && find.Z == 0;
                                Debug.Log("Bottom Room Free");
                                check = true;
                                tempRoomList.Add(find);
                                break;
                            }else if(find == null){
                                Debug.Log("Bottom Room Does Not Exist");
                            }

                        }

                    if(find == null && c == 1){
                        largeRoomSpawnTop = !largeRoomSpawnTop;
                    }else{
                        check = false;
                    }
                    
                }

            //Check Right Tiles
            if(check){
              
                find = loadedRooms.Find( room => room.X == currentRoom.X + 1 && room.Z == currentRoom.Z);

                if(find != null && find.X == 0 && find.Z == 0){
                    find = null;
                }

                if(find != null){
                    Debug.Log("Right Room Free");
                    check = true;
                    tempRoomList.Add(find);
                }else if(find == null){
                    Debug.Log("Right Room Not Exist");
                    check = false;
                   
                }

                //Find Top Right or Bottom Right Tiles
                if(largeRoomSpawnTop && check){

                    find = loadedRooms.Find( room => room.X == currentRoom.X + 1 && room.Z == currentRoom.Z + 1);

                    if(find != null && find.X == 0 && find.Z == 0){
                        find = null;
                    }


                    if(find != null){
                        Debug.Log("Top Right Room Free");
                        check = true;
                        tempRoomList.Add(find);
                    }else if(find == null){
                        Debug.Log("Top Right Room Not Exist");
                        check = false;
                       
                    }

                }else if(largeRoomSpawnTop == false && check){
                    
                    find = loadedRooms.Find( room => room.X == currentRoom.X + 1 && room.Z == currentRoom.Z - 1);

                    if(find != null && find.X == 0 && find.Z == 0){
                        find = null;
                    }


                    if(find != null){
                        Debug.Log("Bottom Right Room Free");
                        check = true;
                        tempRoomList.Add(find);
                    }else if(find == null){
                        Debug.Log("Bottom Right Room Not Exist");
                        check = false;
                     
                    }
                }
            }

            return check;
    }

    private bool CheckLargeRoomSpawnLeft(bool check, Room currentRoom, List<Room> tempRoomList){

        //Check Spawn Left
            largeRoomSpawnRight = false;
            Room find;

            find = loadedRooms.Find( room => room.X == currentRoom.X && room.Z == currentRoom.Z);
            tempRoomList.Add(find);

            largeRoomSpawnTop = Random.value >= 0.5f;

           for(int c = 0; c < 2; c++){
                        //Check Top or Bottom Free Space
                        if(largeRoomSpawnTop){

                            find = loadedRooms.Find( room => room.X == currentRoom.X && room.Z == currentRoom.Z + 1);

                            if(find != null && find.X == 0 && find.Z == 0){
                                find = null;
                            }

                            if(find != null){
                               
                                Debug.Log("Top Room Free");
                                check = true;
                                tempRoomList.Add(find);
                                break;
                            }else if(find == null){
                                Debug.Log("Top Room Does Not Exist");
                            }
                            
                        }
                        if(largeRoomSpawnTop == false){
                            
                            find = loadedRooms.Find( room => room.X == currentRoom.X && room.Z == currentRoom.Z - 1);

                            if(find != null && find.X == 0 && find.Z == 0){
                                find = null;
                            }

                            if(find != null){
                                //nearSpawn = find.X == 0 && find.Z == 0;
                                Debug.Log("Bottom Room Free");
                                check = true;
                                tempRoomList.Add(find);
                                break;
                            }else if(find == null){
                                Debug.Log("Bottom Room Does Not Exist");
                            }

                        }

                    if(find == null && c == 1){
                        largeRoomSpawnTop = !largeRoomSpawnTop;
                    }else{
                        check = false;
                    }
                    
                }

            //Check Left Tiles
            if(check){
              
                find = loadedRooms.Find( room => room.X == currentRoom.X - 1 && room.Z == currentRoom.Z);

                if(find != null && find.X == 0 && find.Z == 0){
                    find = null;
                }

                if(find != null){
                    Debug.Log("Left Room Free");
                    check = true;
                    tempRoomList.Add(find);
                }else if(find == null){
                    Debug.Log("Left Room Not Exist");
                    check = false;
                   
                }

                //Find Top Left or Bottom Left Tiles
                if(largeRoomSpawnTop && check){

                    find = loadedRooms.Find( room => room.X == currentRoom.X - 1 && room.Z == currentRoom.Z + 1);

                    if(find != null && find.X == 0 && find.Z == 0){
                        find = null;
                    }

                    if(find != null){
                        Debug.Log("Top Left Room Free");
                        check = true;
                        tempRoomList.Add(find);
                    }else if(find == null){
                        Debug.Log("Top Left Room Not Exist");
                        check = false;
                       
                    }

                }else if(largeRoomSpawnTop == false && check){
                    
                    find = loadedRooms.Find( room => room.X == currentRoom.X - 1 && room.Z == currentRoom.Z - 1);

                    if(find != null && find.X == 0 && find.Z == 0){
                        find = null;
                    }

                    if(find != null){
                        Debug.Log("Bottom Left Room Free");
                        check = true;
                        tempRoomList.Add(find);
                    }else if(find == null){
                        Debug.Log("Bottom Left Room Not Exist");
                        check = false;
                     
                    }
                }
            }

        return check;
    }
    
    void UpdateExistingRooms(){
         
        
            foreach(Room room in loadedRooms){

                    //Debug.Log(room.ToString());
                    room.CheckAdjacentRoom();


            }

            

    }

    
}
