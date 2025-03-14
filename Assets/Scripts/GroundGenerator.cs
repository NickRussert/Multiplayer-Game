using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;

public class GroundGenerator : NetworkBehaviour
{
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private TileBase[] groundTiles;
    [SerializeField] private int minX = 2, maxX = 18;
    [SerializeField] private int minY = 2, maxY = 10;

    public override void OnNetworkSpawn()
    {
        if (IsServer) // Only the host generates the ground
        {
            GenerateGround();
        }
    }

    void GenerateGround()
    {
        groundTilemap.ClearAllTiles();

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                TileBase randomTile = groundTiles[Random.Range(0, groundTiles.Length)];
                groundTilemap.SetTile(tilePosition, randomTile);
            }
        }

        groundTilemap.RefreshAllTiles();
        SyncGroundClientRpc();
    }

    [ClientRpc]
    private void SyncGroundClientRpc()
    {
        groundTilemap.RefreshAllTiles();
    }
}

