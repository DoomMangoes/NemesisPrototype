
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveScript : MonoBehaviour
{
    public bool isWaveOver;
    public RoomBattleSystemScript roomBattleScript;
    void Awake(){
        isWaveOver = false;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        roomBattleScript = gameObject.transform.parent.gameObject.GetComponent<RoomBattleSystemScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isWaveOver)
            CheckWaveOver();
        if(isWaveOver)
            DeactivateWave();
    }

    public void CheckWaveOver(){

        isWaveOver = true;

        for(int c = 0; c < gameObject.transform.childCount; c++){

            if(gameObject.transform.GetChild(c).gameObject.activeInHierarchy){
                isWaveOver = false;
                break;
            }
        }

    }

    public void DeactivateWave(){

        roomBattleScript.CheckWavesLeft();
        gameObject.SetActive(false);
    }
}
