using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWaveData.asset", menuName = "EnemyWaveData/ Enemy Wave Data")]

public class EnemyWaveData : ScriptableObject
{
    [SerializeField]
    public List<GameObject> enemyWaveList;

    public int minWaves;

    public int maxWaves;
}
