using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int numberOfEnemies = 3;

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 randomPosition = GetRandomPositionInRoom();
            Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomPositionInRoom()
    {
        BoxCollider collider = GetComponent<BoxCollider>();

        float randomX = Random.Range(transform.position.x - collider.size.x / 2, transform.position.x + collider.size.x / 2);
        float randomZ = Random.Range(transform.position.z - collider.size.z / 2, transform.position.z + collider.size.z / 2);

        return new Vector3(randomX, transform.position.y, randomZ);
    }
}
