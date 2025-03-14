using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;

public class DestructibleObstacle : NetworkBehaviour
{
    private Tilemap obstacleTilemap;

    private void Start()
    {
        // Get reference from TilemapManager
        TilemapManager tilemapManager = FindObjectOfType<TilemapManager>();

        if (tilemapManager != null)
        {
            obstacleTilemap = tilemapManager.obstacleTilemap;
        }

        if (obstacleTilemap == null)
        {
            Debug.LogError("Obstacle Tilemap not found! Ensure TilemapManager is in the scene and assigned.");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyTileServerRpc(Vector3 worldPosition)
    {
        if (obstacleTilemap == null) return;

        Vector3Int cellPos = obstacleTilemap.WorldToCell(worldPosition);

        Debug.Log("Bullet hit at world position: " + worldPosition);
        Debug.Log("Converted tile position: " + cellPos);

        // Apply buffer around the impact position (1 tile radius)
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int bufferedTilePos = new Vector3Int(cellPos.x + x, cellPos.y + y, cellPos.z);

                if (obstacleTilemap.HasTile(bufferedTilePos))
                {
                    Debug.Log("Tile found at: " + bufferedTilePos + " - Removing!");
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
