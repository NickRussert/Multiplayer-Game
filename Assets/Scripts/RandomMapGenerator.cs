using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;

public class RandomMapGenerator : NetworkBehaviour
{
    [SerializeField] private Tilemap tilemap; // Assign in Inspector
    [SerializeField] private TileBase[] tiles; // Assign obstacle tiles
    [SerializeField] private int minX = 2, maxX = 18;
    [SerializeField] private int minY = 2, maxY = 10;
    [SerializeField] private int obstacleCount = 20;

    private NetworkVariable<bool> isGenerated = new NetworkVariable<bool>(false);

    public override void OnNetworkSpawn()
    {
        if (IsServer && !isGenerated.Value)
        {
            GenerateRandomObstacles();
            isGenerated.Value = true; // Ensure map is only generated once
        }
    }

    private void GenerateRandomObstacles()
    {
        tilemap.ClearAllTiles(); // Clear previous obstacles

        int spawned = 0;
        while (spawned < obstacleCount)
        {
            int x = Random.Range(minX, maxX);
            int y = Random.Range(minY, maxY);
            Vector3Int tilePosition = new Vector3Int(x, y, 0);

            if (!tilemap.HasTile(tilePosition)) // Only place tile if there's no existing one
            {
                TileBase randomTile = tiles[Random.Range(0, tiles.Length)];
                tilemap.SetTile(tilePosition, randomTile);
                spawned++;
            }
        }

        tilemap.RefreshAllTiles(); // Update the visuals for everyone
        SyncMapClientRpc();
    }

    [ClientRpc]
    private void SyncMapClientRpc()
    {
        tilemap.RefreshAllTiles(); // Ensure Clients update visuals
    }
}


