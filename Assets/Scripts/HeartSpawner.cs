using UnityEngine;

public class HeartSpawner : MonoBehaviour
{
    public GameObject heartPrefab; // Assign the heart prefab in Inspector
    public int numberOfHearts = 5; // How many hearts to spawn
    public float minX = -14f, maxX = 14f; // X boundaries
    public float minY = -14f, maxY = 14f; // Y boundaries

    void Start()
    {
        SpawnHearts();
    }

    void SpawnHearts()
    {
        for (int i = 0; i < numberOfHearts; i++)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            Instantiate(heartPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
