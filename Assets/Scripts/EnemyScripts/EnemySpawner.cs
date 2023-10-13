using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public ObjectPoolManager objectPoolManager; // Reference to the ObjectPoolManager script
    public float spawnRadius = 10f;           // Radius within which enemies can spawn
    public LayerMask groundLayer;             // Layer mask for the ground or terrain
    public string monsterObjectName;          // Name of the monster object in the ObjectPoolManager

    private void Start()
    {
        // Start spawning enemies periodically
        StartCoroutine(SpawnEnemiesPeriodically());
    }

    private IEnumerator SpawnEnemiesPeriodically()
    {
        while (true) // Spawn enemies continuously
        {
            SpawnEnemy();
            yield return new WaitForSeconds(2f); // Adjust the spawn interval as needed
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition(transform.position, spawnRadius);

        // Get an enemy from the object pool based on its name
        GameObject enemy = objectPoolManager.GetPooledObject(monsterObjectName);

        if (enemy != null)
        {
            enemy.transform.position = spawnPosition;
            enemy.SetActive(true);
        }
    }

    private Vector3 GetRandomSpawnPosition(Vector3 center, float radius)
    {
        float randomAngle = Random.Range(0f, 360f);
        float randomDistance = Random.Range(0f, radius);

        // Calculate the random spawn position
        Vector3 spawnPosition = center + Quaternion.Euler(0, randomAngle, 0) * Vector3.forward * randomDistance;

        // Perform raycasting to ensure the spawn position is valid
        RaycastHit hit;
        if (Physics.Raycast(spawnPosition, Vector3.down, out hit, 100f, groundLayer))
        {
            spawnPosition = hit.point;
        }

        return spawnPosition;
    }
}
