using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;

public class RandomMapGenerator : NetworkBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase[] tiles;
    [SerializeField] private int minX = 2, maxX = 18;
    [SerializeField] private int minY = 2, maxY = 10;
    [SerializeField] private int obstacleCount = 20;

    public override void OnNetworkSpawn()
    {
        if (IsServer) // Only the host generates obstacles
        {
            GenerateRandomObstacles();
        }
    }

    void GenerateRandomObstacles()
    {
        tilemap.ClearAllTiles();

        int spawned = 0;
        while (spawned < obstacleCount)
        {
            int x = Random.Range(minX, maxX);
            int y = Random.Range(minY, maxY);
            Vector3Int tilePosition = new Vector3Int(x, y, 0);

            if (!tilemap.HasTile(tilePosition))
            {
                TileBase randomTile = tiles[Random.Range(0, tiles.Length)];
                tilemap.SetTile(tilePosition, randomTile);
                spawned++;
            }
        }

        tilemap.RefreshAllTiles();
        SyncObstaclesClientRpc();
    }

    [ClientRpc]
    private void SyncObstaclesClientRpc()
    {
        tilemap.RefreshAllTiles();
    }
}

