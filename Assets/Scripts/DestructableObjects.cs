using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;

public class DestructibleObstacle : NetworkBehaviour
{
    private Tilemap tilemap;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();

        if (tilemap == null)
        {
            Debug.LogError("Tilemap reference is NULL! Make sure this script is attached to the Obstacles tilemap.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Vector3 hitPosition = other.transform.position;
            Vector3Int tilePosition = tilemap.WorldToCell(hitPosition);

            if (IsServer)
            {
                RemoveTileServerRpc(tilePosition);
            }

            Destroy(other.gameObject);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void RemoveTileServerRpc(Vector3Int tilePosition)
    {
        RemoveTileClientRpc(tilePosition);
    }

    [ClientRpc]
    private void RemoveTileClientRpc(Vector3Int tilePosition)
    {
        tilemap.SetTile(tilePosition, null);
        tilemap.RefreshAllTiles();
    }
}
