using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBattleSystemScript : MonoBehaviour
{

    private enum State {
        Idle,
        Active,
        Clear
    }
    [SerializeField]
    public Room currentRoom;
    [SerializeField]
    private RoomCheck roomCheck;
    [SerializeField]
    public EnemyWaveData enemyWaveData;
    [SerializeField]
    private State state;

    int waveSpawn;
    public int waveCount;
    public int currentWaveCount;
    Transform parentTransform;
    GameObject currentWave;

    void Awake(){
        //currentRoom = gameObject.transform.parent.gameObject.GetComponent<Room>();
        parentTransform = currentRoom.transform;

        state = State.Idle;
        waveCount = Random.Range(enemyWaveData.minWaves, enemyWaveData.maxWaves + 1);
        currentWaveCount = 0;
        
    }

    void Start(){
        roomCheck.OnPlayerEnterRoomTrigger += RoomCheck_OnPlayerEnterRoomTrigger;
    }
   
    private void RoomCheck_OnPlayerEnterRoomTrigger(object sender, System.EventArgs e){
        if(state == State.Idle){
            StartBattle();

            Debug.Log(
                "-X: " + currentRoom.X.ToString() +
                " -Z: " + currentRoom.Z.ToString()
                );
            roomCheck.OnPlayerEnterRoomTrigger -= RoomCheck_OnPlayerEnterRoomTrigger;
    
        }
            
    }

    private void StartBattle(){
        state = State.Active;
        currentRoom.LockRoom();
        
        SpawnWave();
    }

    void SpawnWave(){
        waveSpawn = Random.Range(0, enemyWaveData.enemyWaveList.Count);

        currentWave = Instantiate(enemyWaveData.enemyWaveList[waveSpawn], 
                        new Vector3(parentTransform.position.x, 
                                    parentTransform.position.y + 0.1f, 
                                    parentTransform.position.z), Quaternion.identity);

        currentWave.transform.parent = gameObject.transform;
        
        currentWaveCount++;

    }

    public void CheckWavesLeft(){

        if(currentWaveCount < waveCount && state == State.Active){
            SpawnWave();
        }
        else{
            EndBattle();
        }
        
    }

    private void EndBattle(){

        Debug.Log("Room Clear");
        state = State.Clear;
        currentRoom.OpenRoom();

    }
    
}
