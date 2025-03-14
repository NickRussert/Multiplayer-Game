using UnityEngine;
using Unity.Netcode;

public class MapSpawner : NetworkBehaviour
{
    public GameObject gameGridPrefab; // Assign the GameGrid prefab in Inspector
    private GameObject spawnedGrid;

    public override void OnNetworkSpawn()
    {
        if (IsServer) // Only the Host spawns the map
        {
            SpawnMapServerRpc();
        }
    }

    [ServerRpc]
    private void SpawnMapServerRpc()
    {
        spawnedGrid = Instantiate(gameGridPrefab, Vector3.zero, Quaternion.identity);
        spawnedGrid.GetComponent<NetworkObject>().Spawn(true); // Syncs with Clients
    }
}
