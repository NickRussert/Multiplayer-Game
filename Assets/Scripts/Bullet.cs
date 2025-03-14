using UnityEngine;
using Unity.Netcode;

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

            // Try to get the obstacle script and destroy the tile
            DestructibleObstacle obstacle = other.GetComponent<DestructibleObstacle>();
            if (obstacle != null)
            {
                obstacle.DestroyTileServerRpc(transform.position);
            }

            PlayHitSoundClientRpc();
            DestroyBullet();
        }
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


