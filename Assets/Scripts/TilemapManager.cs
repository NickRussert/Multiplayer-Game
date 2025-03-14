using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;

public class TilemapManager : NetworkBehaviour
{
    public Tilemap obstacleTilemap; // Assign in Inspector

    private void Start()
    {
        if (obstacleTilemap == null)
        {
            Debug.LogError("Obstacle Tilemap not assigned in TilemapManager!");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyTileServerRpc(Vector3 worldPosition)
    {
        if (obstacleTilemap == null) return;

        //  Convert world position to tile position
        Vector3Int cellPos = obstacleTilemap.WorldToCell(worldPosition);
        Debug.Log($"Bullet hit at world position: {worldPosition}, Tile position: {cellPos}");

        //  Apply explosion radius (3x3)
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int bufferedTilePos = new Vector3Int(cellPos.x + x, cellPos.y + y, cellPos.z);

                if (obstacleTilemap.HasTile(bufferedTilePos))
                {
                    Debug.Log($"Removing tile at {bufferedTilePos}");
                    obstacleTilemap.SetTile(bufferedTilePos, null);
                }
            }
        }

        DestroyTileClientRpc(cellPos);
    }

    [ClientRpc]
    private void DestroyTileClientRpc(Vector3Int cellPos)
    {
        if (IsServer) return;
        if (obstacleTilemap == null) return;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int bufferedTilePos = new Vector3Int(cellPos.x + x, cellPos.y + y, cellPos.z);
                obstacleTilemap.SetTile(bufferedTilePos, null);
            }
        }
    }
}

