using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;  // Array to store enemy prefabs
    public int poolSize = 10;          // Initial pool size for each enemy type

    // Dictionary to hold object pools for each enemy type
    private Dictionary<string, Queue<GameObject>> objectPools = new Dictionary<string, Queue<GameObject>>();

    void Start()
    {
        // Initialize object pools for each enemy type
        foreach (GameObject enemyPrefab in enemyPrefabs)
        {
            string enemyType = enemyPrefab.name;
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject enemy = Instantiate(enemyPrefab);
                enemy.SetActive(false);
                objectPool.Enqueue(enemy);
            }

            objectPools.Add(enemyType, objectPool);
        }
    }

    // Method to retrieve an object from the pool based on enemy type
    public GameObject GetPooledObject(string enemyType)
    {
        if (objectPools.ContainsKey(enemyType))
        {
            Queue<GameObject> objectPool = objectPools[enemyType];

            if (objectPool.Count > 0)
            {
                GameObject obj = objectPool.Dequeue();
                obj.SetActive(true);
                return obj;
            }
        }

        return null; // Object pool for the specified enemy type is empty
    }

    // Method to return an object to the pool
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        objectPools[obj.name].Enqueue(obj);
    }
}