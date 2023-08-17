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

    private List<Room> tempRoomList= new List<Room>();

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

        tempRoomList.Add(new Room (-1,0));
        tempRoomList.Add(new Room (1,0));
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
        
        if(largeRoomSpawn == false && index < tempRoomList.Count){
            /*
            foreach (var item in loadedRooms)
                    {
                        Debug.Log(item.ToString());
                    }
            */
          
            //Debug.Log("Temp Room: " + tempRoom.ToString());
            tempCheck = CheckLargeRoomSpawnLocation(tempRoomList[index]);
            largeRoomSpawn = true;
              //test = true;
        }

        if(tempCheck && largeRoomSpawn){

            tempCheck = false;
            
            StartCoroutine(SpawnLargeRoom(tempRoomList[index]));

                foreach (var room in roomsToRemove)
                    {
                        Debug.Log(room.ToString());
                    }

                Debug.Log(loadRoomQueue.Count.ToString());
            }

        //END TESTING
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
                float tempX = 0;
                float tempZ = 0;

                 if(largeRoomSpawnRight){
                        tempX = +10;
                    }else if(largeRoomSpawnRight == false){
                        tempX = -10;
                    }
                
                    if(largeRoomSpawnTop){
                        tempZ = +10;
                    }else if(largeRoomSpawnTop == false){
                        tempZ = -10;
                    }
        

                room.transform.position = new Vector3( 
                (currentLoadRoomData.X * 20) + tempX,
                0,
                (currentLoadRoomData.Z * 20) + tempZ
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

        if(test == false && currentRoom.X > 0){

            largeRoomSpawnRight = true;

            //Debug.Log(currentRoom.ToString());
            find = loadedRooms.Find( room => room.X == currentRoom.X && room.Z == currentRoom.Z);
            tempRoomList.Add(find);

            largeRoomSpawnTop = Random.value >= 0.5f;

           for(int c = 0; c < 2; c++){
                        //Check Top or Bottom Free Space
                        if(largeRoomSpawnTop){

                            find = loadedRooms.Find( room => room.X == currentRoom.X && room.Z == currentRoom.Z + 1);

                            if(find){
                               
                                Debug.Log("Top Room Free");
                                check = true;
                                tempRoomList.Add(find);
                                break;
                            }else if(find == null){
                                Debug.Log("Top Room Does Not Exist");
                            }
                            
                        }else if(largeRoomSpawnTop == false){
                            
                            find = loadedRooms.Find( room => room.X == currentRoom.X && room.Z == currentRoom.Z - 1);

                            if(find){
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

                if(find){
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
            
        }
        if(test == false && currentRoom.X < 0){
            //Check Spawn Left
            largeRoomSpawnRight = false;

            find = loadedRooms.Find( room => room.X == currentRoom.X && room.Z == currentRoom.Z);
            tempRoomList.Add(find);

            largeRoomSpawnTop = Random.value >= 0.5f;

           for(int c = 0; c < 2; c++){
                        //Check Top or Bottom Free Space
                        if(largeRoomSpawnTop){

                            find = loadedRooms.Find( room => room.X == currentRoom.X && room.Z == currentRoom.Z + 1);

                            if(find){
                               
                                Debug.Log("Top Room Free");
                                check = true;
                                tempRoomList.Add(find);
                                break;
                            }else if(find == null){
                                Debug.Log("Top Room Does Not Exist");
                            }
                            
                        }else if(largeRoomSpawnTop == false){
                            
                            find = loadedRooms.Find( room => room.X == currentRoom.X && room.Z == currentRoom.Z - 1);

                            if(find){
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

                if(find){
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


        }


        /*
        if(test){
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

        */

        Debug.Log("Check:" + check);

        if(check){
            foreach(Room room in tempRoomList){
                roomsToRemove.Add(room);
            }
        }

        return check;
        
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
        }

        roomsToRemove.Clear();

          Debug.Log("After Clear:");
        foreach(Room room in roomsToRemove){
            Debug.Log(room);
        }

        largeRoomSpawn = false;
        index += 1;
        /*
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

        LoadRoom("Large", tempRoom.X, tempRoom.Z);
        */
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
}
