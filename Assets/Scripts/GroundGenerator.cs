using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap groundTilemap;   // Assign Ground Tilemap in Inspector
    [SerializeField] private TileBase[] groundTiles;  // Assign multiple ground tiles for variation
    [SerializeField] private int minX = 2, maxX = 18; // X bounds for ground
    [SerializeField] private int minY = 2, maxY = 10; // Y bounds for ground

    void Start()
    {
        GenerateGround();
    }

    void GenerateGround()
    {
        groundTilemap.ClearAllTiles(); // Clear any previous tiles

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                // Pick a random ground tile (if multiple available)
                TileBase randomTile = groundTiles[Random.Range(0, groundTiles.Length)];

                // Place the tile
                groundTilemap.SetTile(tilePosition, randomTile);
            }
        }

        groundTilemap.RefreshAllTiles(); // Refresh to update visuals
    }
}
