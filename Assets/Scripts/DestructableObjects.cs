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
        if (other.CompareTag("Bullet")) // If hit by a bullet
        {
            Vector3 hitPosition = other.ClosestPoint(transform.position);
            Vector3Int tilePosition = tilemap.WorldToCell(hitPosition + new Vector3(0.5f, 0.5f, 0)); // Adjusted slightly


            Debug.Log("Bullet hit at world position: " + hitPosition);
            Debug.Log("Converted tile position: " + tilePosition);

            if (tilemap.HasTile(tilePosition)) // Check if a tile exists at the hit position
            {
                Debug.Log("Tile exists at: " + tilePosition + " and will be removed");

                tilemap.SetTile(tilePosition, null);
                tilemap.RefreshAllTiles();  // Force a refresh
            }
            else
            {
                Debug.LogWarning("No tile found at: " + tilePosition);
            }

            Destroy(other.gameObject); // Destroy bullet on impact
        }
    }

}
