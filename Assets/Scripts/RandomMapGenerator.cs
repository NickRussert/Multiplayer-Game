using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomMapGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;   // Assign the Tilemap in Inspector
    [SerializeField] private TileBase[] tiles;  // Assign different obstacle tile types
    [SerializeField] private int minX = 2, maxX = 18; // X boundary for spawning
    [SerializeField] private int minY = 2, maxY = 10; // Y boundary for spawning
    [SerializeField] private int obstacleCount = 20; // Number of obstacles to spawn

    void Start()
    {
        GenerateRandomObstacles();
    }

    void GenerateRandomObstacles()
    {
        tilemap.ClearAllTiles(); // Clear previous obstacles

        int spawned = 0;
        while (spawned < obstacleCount)
        {
            // Generate random position within defined boundaries
            int x = Random.Range(minX, maxX);
            int y = Random.Range(minY, maxY);
            Vector3Int tilePosition = new Vector3Int(x, y, 0);

            // Check if there's already a tile to prevent overlap
            if (!tilemap.HasTile(tilePosition))
            {
                TileBase randomTile = tiles[Random.Range(0, tiles.Length)];
                tilemap.SetTile(tilePosition, randomTile);
                spawned++; // Increase obstacle count
            }
        }

        tilemap.RefreshAllTiles(); // Refresh the Tilemap to update visuals
    }
}
