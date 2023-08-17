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

    //private bool test = false;

    private List<Room> tempRoomSpawnList= new List<Room>();

    private List<Room> roomsToAdd = new List<Room>();

    private int index = 0;

    private bool tempCheck = false; 
    void Awake() {
        instance = this;
    }

    void Start(){
        
        LoadRoom("Start",0,0);
        LoadRoom("Empty",-1,0);
        LoadRoom("Empty",0,1);
        LoadRoom("Empty",0,-1);

        LoadRoom("Empty",1,0);
        LoadRoom("Empty",2,0);
        LoadRoom("Empty",1,1);
        LoadRoom("Empty",2,1);
        LoadRoom("Empty",1,-1);
        LoadRoom("Empty",2,-1);

       
        LoadRoom("Empty",-2,0);
        LoadRoom("Empty",-1,1);
        LoadRoom("Empty",-2,1);
        LoadRoom("Empty",-1,-1);
        LoadRoom("Empty",-2,-1);

        LoadRoom("Empty",-2,2);
        LoadRoom("Empty",-1,2);
        LoadRoom("Empty",0,2);
        LoadRoom("Empty",1,2);
        LoadRoom("Empty",2,2);

        LoadRoom("Empty",-2,-2);
        LoadRoom("Empty",-1,-2);
        LoadRoom("Empty",0,-2);
        LoadRoom("Empty",1,-2);
        LoadRoom("Empty",2,-2);

        tempRoomSpawnList.Add(new Room (0,1));
        tempRoomSpawnList.Add(new Room (0,-1));
        
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
                    int largeSpawnChance = Random.Range(0,3);
                    Debug.Log(largeSpawn);

                    if(largeSpawnChance == 1){

                        if(largeSpawn && !largeRoomSpawn){
                            largeRoomSpawn = true;
                            StartCoroutine(SpawnLargeRoom(room));
                            largeRoomSpawn = false;
                         }

                        room.RemoveUnconnectedDoors();
                        room.ActivateWalls();

                    }


                    
                }
                uniqueRoomSpawn = true;
                Debug.Log("Room Spawn Done");
            }

        */
        
        /*
            if(!spawnedBossRoom){
                StartCoroutine(SpawnBossRoom());
            } else if(spawnedBossRoom && !updatedRooms){

                foreach(Room room in loadedRooms){
                    room.RemoveUnconnectedDoors();
                    room.ActivateWalls();
                    
                }
                updatedRooms = true;
            }

        */
                    
        //TESTING
        
        if(largeRoomSpawn == false && index < tempRoomSpawnList.Count){
            /*
            foreach (var item in loadedRooms)
                    {
                        Debug.Log(item.ToString());
                    }
            */
          
            //Debug.Log("Temp Room: " + tempRoom.ToString());
            tempCheck = CheckLargeRoomSpawnLocation(tempRoomSpawnList[index]);

            if(tempCheck){
                largeRoomSpawn = true;
            }
            else{
                index +=1;
            }
            
        }

        if(tempCheck && largeRoomSpawn){

            tempCheck = false;
            
            StartCoroutine(SpawnLargeRoom(tempRoomSpawnList[index]));

                foreach (var room in roomsToRemove)
                    {
                        Debug.Log(room.ToString());
                    }

                Debug.Log(loadRoomQueue.Count.ToString());

           updatedRooms = false;
        }



        //END TESTING
            return;
        }

       if(updatedRooms == false && largeRoomSpawn == false){
         Debug.Log("Rooms in loadedRooms:");
            //Update Rooms
            foreach(Room room in loadedRooms){

                    Debug.Log(room.ToString());
                    room.RemoveUnconnectedDoors();
                    room.ActivateWalls();

            }

            updatedRooms = true;
       }
        
        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;
        //updatedRooms = false;

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

        //List<Room> roomArray = new List<Room>;

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

                //int count = 1;
                room.X = currentLoadRoomData.X + tempX;
                room.Z = currentLoadRoomData.Z + tempZ;
                room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.X + ", " + room.Z;
                room.transform.parent = transform;

                /*
                foreach(Room roomToAdd in roomsToAdd){

                    count +=1;
                    roomToAdd.name = currentWorldName + "-" + currentLoadRoomData.name + "-" + count + " " + room.X + ", " + room.Z;
                    roomToAdd.transform.parent = transform;

                    loadedRooms.Add(roomToAdd);
                }

                roomsToAdd.Clear();

                */

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

    

    IEnumerator SpawnLargeRoom(Room currentRoom){

        yield return new WaitForSeconds(0.5f);

        Room largeRoom = currentRoom;
        Room tempRoom = new Room(largeRoom.X, largeRoom.Z);
        //Destroy(largeRoom.gameObject);

        //var roomToRemove = loadedRooms.Single( r => r.X == tempRoom.X && r.Z == tempRoom.Z);
        //loadedRooms.Remove(roomToRemove);
        
        foreach(Room room in roomsToRemove){

            Destroy(room.gameObject);
            var roomToRemove = loadedRooms.Single( r => r.X == room.X && r.Z == room.Z);
            loadedRooms.Remove(roomToRemove);

        }
           
        LoadRoom("Large", tempRoom.X, tempRoom.Z);

          Debug.Log("Before Clear:");
        foreach(Room room in roomsToRemove){
            Debug.Log(room);
            roomsToAdd.Add(room);
        }

        roomsToRemove.Clear();

          Debug.Log("After Clear:");
        foreach(Room room in roomsToRemove){
            Debug.Log(room);
        }

        largeRoomSpawn = false;
        index += 1;
       
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


        /*
          if(DoesRoomExist(x,z)){
            return;
        }

        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.X = x;
        newRoomData.Z = z;

        loadRoomQueue.Enqueue(newRoomData);
        */
    }

    private bool CheckLargeRoomSpawnLocation(Room currentRoom){

        bool check = false;
        Room find = null;
        bool test = false;
        List<Room> tempRoomList = new List<Room>();

            
        //Check If Start Room
            Debug.Log("Check Spawn Potential");
            //Debug.Log(currentRoom.ToString());
            Debug.Log("CurrentRoom X:" + currentRoom.X);
            Debug.Log("CurrentRoom Z:" + currentRoom.Z);

        test = currentRoom.X != 0 && currentRoom.Z != 0 ;

            Debug.Log("Spawn Test:" + test);

        //If not Start Room Check Possible Room Spawn

        //Check Spawn Right

        if(test == false){
            bool rand = Random.value >= 0.5f;

            for(int c = 0; c < 1; c++){

                if(rand){
                check = CheckLargeRoomSpawnRight(check, currentRoom, tempRoomList);
                    if(check){
                        break;
                    }
                }else{
                check = CheckLargeRoomSpawnLeft(check, currentRoom, tempRoomList);
                    if(check){
                        break;
                    }
                }

                rand = !rand;

            }
            
        }
        /*
        if(test == false && currentRoom.X > 0){

            
            check = CheckLargeRoomSpawnRight(check, currentRoom, tempRoomList);
            
        }
        if(test == false && currentRoom.X < 0){
            
            check = CheckLargeRoomSpawnLeft(check, currentRoom, tempRoomList);
        }
        */
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

            //Debug.Log(currentRoom.ToString());
            find = loadedRooms.Find( room => room.X == currentRoom.X && room.Z == currentRoom.Z);
            tempRoomList.Add(find);

            largeRoomSpawnTop = Random.value >= 0.5f;

           for(int c = 0; c < 2; c++){
                        //Check Top or Bottom Free Space
                        if(largeRoomSpawnTop){

                            find = loadedRooms.Find( room => room.X == currentRoom.X && room.Z == currentRoom.Z + 1);
       
                            if(find.X == 0 && find.Z == 0){
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
                            
                        }else if(largeRoomSpawnTop == false){
                            
                            find = loadedRooms.Find( room => room.X == currentRoom.X && room.Z == currentRoom.Z - 1);

                            if(find.X == 0 && find.Z == 0){
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

                if(find.X == 0 && find.Z == 0){
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

                    if(find){
                        Debug.Log("Top Right Room Free");
                        check = true;
                        tempRoomList.Add(find);
                    }else if(find == null){
                        Debug.Log("Top Right Room Not Exist");
                        check = false;
                       
                    }

                }else if(largeRoomSpawnTop == false && check){
                    
                    find = loadedRooms.Find( room => room.X == currentRoom.X + 1 && room.Z == currentRoom.Z - 1);

                    if(find){
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

                            if(find.X == 0 && find.Z == 0){
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
                            
                        }else if(largeRoomSpawnTop == false){
                            
                            find = loadedRooms.Find( room => room.X == currentRoom.X && room.Z == currentRoom.Z - 1);

                            if(find.X == 0 && find.Z == 0){
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

                if(find.X == 0 && find.Z == 0){
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

                    if(find){
                        Debug.Log("Top Left Room Free");
                        check = true;
                        tempRoomList.Add(find);
                    }else if(find == null){
                        Debug.Log("Top Left Room Not Exist");
                        check = false;
                       
                    }

                }else if(largeRoomSpawnTop == false && check){
                    
                    find = loadedRooms.Find( room => room.X == currentRoom.X - 1 && room.Z == currentRoom.Z - 1);

                    if(find){
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
    
}
