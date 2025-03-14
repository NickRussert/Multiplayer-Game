using UnityEngine;
using Unity.Netcode;
using UnityEngine.Tilemaps;

public class GridSpawner : NetworkBehaviour
{
    public GameObject gridPrefab; // Assign Grid Prefab in Inspector
    private GameObject spawnedGrid;

    public override void OnNetworkSpawn()
    {
        if (IsServer) //  This ensures the host spawns the grid AFTER the network is initialized
        {
            SpawnGridServerRpc();
        }
        else
        {
            Debug.Log("GridSpawner: Not the host, skipping grid spawn.");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnGridServerRpc()
    {
        if (gridPrefab == null)
        {
            Debug.LogError("GridSpawner: Grid Prefab is not assigned!");
            return;
        }

        if (spawnedGrid != null)
        {
            Debug.LogWarning("GridSpawner: Grid already spawned!");
            return;
        }

        Debug.Log("GridSpawner: Spawning Grid...");
        spawnedGrid = Instantiate(gridPrefab, Vector3.zero, Quaternion.identity);
        NetworkObject gridNetworkObject = spawnedGrid.GetComponent<NetworkObject>();

        if (gridNetworkObject != null)
        {
            gridNetworkObject.Spawn(true); //  Ensures all clients see it
            Debug.Log("GridSpawner: Grid spawned successfully.");
            SyncTilemapClientRpc();
        }
        else
        {
            Debug.LogError("GridSpawner: Grid Prefab does NOT have a NetworkObject!");
        }
    }

    [ClientRpc]
    private void SyncTilemapClientRpc()
    {
        Tilemap tilemap = spawnedGrid.GetComponentInChildren<Tilemap>();
        if (tilemap != null)
        {
            tilemap.RefreshAllTiles(); //  Force tilemap refresh on all clients
            Debug.Log("GridSpawner: Tilemap refreshed on clients.");
        }
    }
}

