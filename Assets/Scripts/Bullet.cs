using UnityEngine;
using Unity.Netcode;
using UnityEngine.Tilemaps;

public class Bullet : NetworkBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;

    private Rigidbody2D rb;

    public AudioClip obstacleHitSound;
    public float hitVolume = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (IsServer) // Server controls bullet movement
        {
            rb.linearVelocity = transform.right * speed;
            Invoke(nameof(DestroyBullet), lifeTime); // Destroy bullet after some time
        }
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer) // Ensure Clients apply velocity when they receive the bullet
        {
            rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = transform.right * speed;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return; // Only the server handles collisions

        if (other.CompareTag("Player") || other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            TankHealth tankHealth = other.GetComponent<TankHealth>();
            if (tankHealth != null)
            {
                tankHealth.TakeDamage(1);
            }
            DestroyBullet();
        }
        else if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Bullet hit an obstacle!");

            TilemapManager tilemapManager = FindObjectOfType<TilemapManager>();
            if (tilemapManager != null)
            {
                // Get the **exact impact position**
                Vector3 hitPosition = GetImpactPoint(other);
                Debug.Log($"Bullet impact at world position: {hitPosition}");

                //  Call TilemapManager to destroy the correct tile
                tilemapManager.DestroyTileServerRpc(hitPosition);
            }
            else
            {
                Debug.LogError("TilemapManager not found in the scene!");
            }

            PlayHitSoundClientRpc();
            DestroyBullet();
        }
    }

    //  Helper function to get the **exact impact point** on the obstacle
    private Vector3 GetImpactPoint(Collider2D collider)
    {
        if (collider.TryGetComponent<Tilemap>(out Tilemap tilemap))
        {
            //  Convert bullet position to the **nearest tile center**
            Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
            return tilemap.GetCellCenterWorld(cellPosition);
        }

        return collider.ClosestPoint(transform.position); // Fallback for non-tilemap objects
    }

    [ClientRpc]
    private void PlayHitSoundClientRpc()
    {
        if (obstacleHitSound != null)
        {
            AudioSource.PlayClipAtPoint(obstacleHitSound, transform.position, hitVolume);
        }
    }

    private void DestroyBullet()
    {
        if (IsServer)
        {
            NetworkObject networkObject = GetComponent<NetworkObject>();
            if (networkObject != null)
            {
                networkObject.Despawn(true); // Remove bullet from all Clients
            }
        }
    }
}

