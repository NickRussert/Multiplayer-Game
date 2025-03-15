using UnityEngine;
using Unity.Netcode;

public class HeartPickup : NetworkBehaviour
{
    public float respawnTime = 10f; // Time before the heart respawns
    public AudioClip pickupSound; // Heart pickup sound
    public float soundVolume = 0.7f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return; // Only the server handles pickups

        if (other.CompareTag("Player") || other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            TankHealth playerHealth = other.GetComponent<TankHealth>();

            if (playerHealth != null)
            {
                playerHealth.RestoreHealth(); // Restore one heart of health
                Debug.Log($"Player {other.GetComponent<NetworkObject>().OwnerClientId} picked up a heart!");

                //  Play sound for all players
                PlayPickupSoundClientRpc();

                //  Despawn the heart for all clients
                DespawnHeartServerRpc();
            }
        }
    }

    // Server RPC to despawn the heart across all players
    [ServerRpc(RequireOwnership = false)]
    private void DespawnHeartServerRpc()
    {
        NetworkObject networkObject = GetComponent<NetworkObject>();

        if (networkObject != null)
        {
            networkObject.Despawn(true); // Despawn heart for all clients
            StartCoroutine(RespawnHeart()); // Start respawn timer
        }
    }

    //  Respawn the heart after a delay
    private System.Collections.IEnumerator RespawnHeart()
    {
        yield return new WaitForSeconds(respawnTime);

        if (IsServer) // Only the server respawns the heart
        {
            NetworkObject networkObject = GetComponent<NetworkObject>();

            if (networkObject != null)
            {
                networkObject.Spawn(true); // Respawn heart for all clients
            }
        }
    }

    //  Play the heart pickup sound for all players
    [ClientRpc]
    private void PlayPickupSoundClientRpc()
    {
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, soundVolume);
        }
    }
}
