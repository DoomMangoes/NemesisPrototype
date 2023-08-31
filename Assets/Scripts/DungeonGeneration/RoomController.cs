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

    private List<Room> roomCandidates= new List<Room>();

    private int index = 1;

    private bool tempCheck = false; 

    private Room tempBossRoom = new Room(0,0);

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
        
            
            if(!spawnedBossRoom && uniqueRoomSpawn){
                
            CheckBossRoomSpawn();
    
            StartCoroutine(SpawnBossRoom());
            Debug.Log("BOSS ROOM SPAWNED!");

            } else if(spawnedBossRoom && !updatedRooms){

            foreach(Room room in loadedRooms){
               
                room.CheckAdjacentRoom();
                    
            }
            updatedRooms = true;

            
            }
        
            
            if(index == loadedRooms.Count){
                uniqueRoomSpawn = true;
            }


            if(largeRoomSpawn == false && index < loadedRooms.Count){
          
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


            if(currentLoadRoomData.name == "Large" ||  currentLoadRoomData.name == "Boss"){
               
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
                (currentLoadRoomData.X * (room.Width / 2)) + (tempX * (room.Width / 2)),
                0,
                (currentLoadRoomData.Z * (room.Height / 2)) + (tempZ * (room.Height / 2))
                );
                


            }
            else{
                room.transform.position = new Vector3( 
                currentLoadRoomData.X * room.Width,
                0,
                currentLoadRoomData.Z * room.Height
            );
            }
            
            if(currentLoadRoomData.name == "Large" || currentLoadRoomData.name == "Boss"){

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
            /*
            if(loadedRooms.Count == 0){

                RoomCameraController.instance.currRoom = room;
            }
            */

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

    void CheckBossRoomSpawn(){

        
        float XCoordinate = 0;
        float ZCoordinate = 0;
        bool XCondition = false;
        bool ZCondition = false;

        
        //Check furthest coordinate and if Boss Room Spawn is atleast 2 tiles away from spawn
       
        XCoordinate = CheckFurthestRoomXCoordinate();
            
        ZCoordinate = CheckFurthestRoomZCoordinate();


            
        
        foreach(Room room in loadedRooms){

            if(XCoordinate > 0){
            XCondition = room.X >= XCoordinate;
            }else if(XCoordinate < 0){
            XCondition = room.X <= XCoordinate;
            }

            if(ZCoordinate > 0){
                ZCondition = room.Z >= ZCoordinate;
            }else if(ZCoordinate < 0){
                ZCondition = room.Z <= ZCoordinate;
            }


            if( XCondition || ZCondition){
                roomCandidates.Add(room);
            }
        }

            tempBossRoom = roomCandidates[Random.Range(0,roomCandidates.Count)];
            //Debug.Log("Furthest X:" + XCoordinate + " Z:" + ZCoordinate);
            //Debug.Log("Boss Room Spawn Location:" + tempBossRoom.ToString());
            roomCandidates.Clear();
    }

    float CheckFurthestRoomXCoordinate(){

        float furthestX = 0;
        bool isPositive = Random.value >= 0.5f;


        for(int c = 0; c < 2; c++){

            if(isPositive){
                foreach(Room room in loadedRooms){

                if(room.X > furthestX){
                    furthestX = room.X;
                }

                    
                }
                if(furthestX > 2){
                     break;
                }else{
                    isPositive = false;
                }
            }

               

            if(isPositive == false){

                foreach(Room room in loadedRooms){

                if(room.X < furthestX){
                    furthestX = room.X;
                }

                

            }
                if(furthestX > -2){
                    break;
                }else{
                    isPositive = true;
                   
                }
                
            }

        }
       
        

        return furthestX;
    }

    float CheckFurthestRoomZCoordinate(){

        float furthestZ = 0;
        bool isPositive = Random.value >= 0.5f;


        for(int c = 0; c < 2; c++){

            if(isPositive){
                foreach(Room room in loadedRooms){

                if(room.Z > furthestZ){
                    furthestZ = room.Z;
                }

                }

                if(furthestZ > 2){
                    break;
                }else{
                    isPositive = false;
                   
                }
            }

                

            if(isPositive == false){

            foreach(Room room in loadedRooms){

                if(room.Z < furthestZ){
                    furthestZ = room.Z;
                }

               
                }

                if(furthestZ < -2){
                    break;
                }else{

                    isPositive = true;
                }

            }

                

        }
       
        return furthestZ;
    }



    IEnumerator SpawnBossRoom(){
        
        spawnedBossRoom = true;
        yield return new WaitForSeconds(0.5f);



        if(loadRoomQueue.Count == 0){

            float XIncrement = 0;
            float ZIncrement = 0;


            Room bossRoom = tempBossRoom;
            Room tempRoom = new Room(bossRoom.X, bossRoom.Z);

            bool rand = Random.value >= 0.5f;

            for(int c = 0; c < 2; c++){

                if(rand){

                    if(tempRoom.X > 0 && !DoesRoomExist(tempRoom.X + 1, tempRoom.Z)){
                        
                        XIncrement = 1;
                        break;
                    }else if(tempRoom.X < 0 && !DoesRoomExist(tempRoom.X - 1, tempRoom.Z)){
                        
                        XIncrement = -1;
                         break;
                    }

                }
                if(rand == false){

                    if(tempRoom.Z > 0 && !DoesRoomExist(tempRoom.X , tempRoom.Z + 1)){
                        
                        ZIncrement = 1;
                        break;
                    }else if(tempRoom.Z < 0 && !DoesRoomExist(tempRoom.X, tempRoom.Z - 1)){
                        
                        ZIncrement = -1;
                        break;
                    }

                }

                rand = !rand;

            }

                if(XIncrement != 0){
                    if(XIncrement > 0){
                        largeRoomSpawnRight = true;
                    }
                    if(XIncrement < 0){
                        largeRoomSpawnRight = false;
                    }

                    largeRoomSpawnTop = Random.value >= 0.5f;
                }
                if(ZIncrement != 0){
                    if(ZIncrement > 0){
                        largeRoomSpawnTop = true;
                    }
                    if(ZIncrement < 0){
                        largeRoomSpawnTop = false;
                    }

                    largeRoomSpawnRight = Random.value >= 0.5f;
                }

            

            LoadRoom("End", tempRoom.X + XIncrement, tempRoom.Z + ZIncrement);

            Debug.Log("Boss Room: " + tempRoom.ToString());
            LoadRoom("Boss", tempRoom.X + (XIncrement * 2), tempRoom.Z + (ZIncrement * 2));

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
