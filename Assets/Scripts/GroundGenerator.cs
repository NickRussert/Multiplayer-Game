using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;

public class GroundGenerator : NetworkBehaviour
{
    [SerializeField] private Tilemap groundTilemap; // Assign Ground Tilemap
    [SerializeField] private TileBase groundTile; // Assign a tile
    [SerializeField] private int minX = -15, maxX = 15;
    [SerializeField] private int minY = -15, maxY = 15;

    private NetworkVariable<bool> isGenerated = new NetworkVariable<bool>(false);

    public override void OnNetworkSpawn()
    {
        if (IsServer && !isGenerated.Value) // Only Host generates
        {
            GenerateGround();
            isGenerated.Value = true;
        }
    }

    private void GenerateGround()
    {
        groundTilemap.ClearAllTiles();

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                groundTilemap.SetTile(tilePosition, groundTile);
            }
        }

        SyncGroundClientRpc();
    }

    [ClientRpc]
    private void SyncGroundClientRpc()
    {
        groundTilemap.RefreshAllTiles();
    }
}

