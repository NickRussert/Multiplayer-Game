using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructibleObstacle : MonoBehaviour
{
    private Tilemap tilemap;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();

        if (tilemap == null)
        {
            Debug.LogError("Tilemap reference is NULL! Make sure this script is attached to the Obstacles tilemap.");
        }
        else
        {
            Debug.Log("Tilemap successfully assigned: " + tilemap.gameObject.name);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet")) // If bullet hits an obstacle
        {
            Vector3 hitPosition = other.transform.position; // Get bullet's impact world position
            Vector3Int tilePosition = tilemap.WorldToCell(hitPosition); // Convert to tile position

            Debug.Log("Bullet hit at world position: " + hitPosition);
            Debug.Log("Converted tile position: " + tilePosition);

            // Create a buffer range around the tile
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Vector3Int bufferedTilePos = new Vector3Int(tilePosition.x + x, tilePosition.y + y, tilePosition.z);

                    if (tilemap.HasTile(bufferedTilePos)) // Check if a tile exists in the buffered range
                    {
                        Debug.Log(" Tile found at: " + bufferedTilePos + " - Removing!");
                        tilemap.SetTile(bufferedTilePos, null); // Remove the tile
                        tilemap.RefreshAllTiles();
                        break; // Exit loop once we remove a tile
                    }
                }
            }

            Destroy(other.gameObject); // Destroy bullet on impact
        }
    }

}