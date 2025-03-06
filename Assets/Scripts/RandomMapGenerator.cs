using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomMapGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;   // Assign the obstacle Tilemap in Inspector
    [SerializeField] private TileBase[] tiles;  // Assign obstacle tiles in Inspector
    [SerializeField] private int minX = 2, maxX = 18; // X boundaries for spawning
    [SerializeField] private int minY = 2, maxY = 10; // Y boundaries for spawning
    [SerializeField] private int obstacleCount = 20; // Total number of obstacles to spawn

    [SerializeField] private Transform blackTank; // Assign Player 1 (Black Tank) Transform
    [SerializeField] private Transform redTank;   // Assign Player 2 (Red Tank) Transform
    [SerializeField] private float safeZoneRadius = 3f; // No obstacles will spawn within this radius of a tank

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
            Vector3 worldPos = tilemap.CellToWorld(tilePosition); // Convert to world space for distance check

            // Check if tile is too close to either tank
            if (Vector3.Distance(worldPos, blackTank.position) < safeZoneRadius ||
                Vector3.Distance(worldPos, redTank.position) < safeZoneRadius)
            {
                continue; // Skip this tile and try another position
            }

            // Check if there's already an obstacle there
            if (!tilemap.HasTile(tilePosition))
            {
                TileBase randomTile = tiles[Random.Range(0, tiles.Length)];
                tilemap.SetTile(tilePosition, randomTile);
                spawned++; // Increase obstacle count
            }
        }

        tilemap.RefreshAllTiles(); // Refresh Tilemap to apply changes
    }
}
