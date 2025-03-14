using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;

public class DestructibleObstacle : NetworkBehaviour
{
    private Tilemap tilemap;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyTileServerRpc(Vector3 hitPosition)
    {
        Vector3Int tilePosition = tilemap.WorldToCell(hitPosition);
        if (tilemap.HasTile(tilePosition))
        {
            tilemap.SetTile(tilePosition, null); // Remove the tile
            tilemap.RefreshAllTiles();
            SyncTileDestructionClientRpc(tilePosition);
        }
    }

    [ClientRpc]
    private void SyncTileDestructionClientRpc(Vector3Int tilePosition)
    {
        tilemap.SetTile(tilePosition, null); // Remove the tile on Clients
        tilemap.RefreshAllTiles();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet")) // If bullet hits an obstacle
        {
            DestroyTileServerRpc(other.transform.position);
            Destroy(other.gameObject);
        }
    }
}
