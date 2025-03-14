using UnityEngine;
using Unity.Netcode;

public class TankShooting : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireCooldown = 1f;
    private float lastFireTime;

    public AudioClip shootSound;
    public float shootVolume = 0.5f;

    void Update()
    {
        if (!IsOwner) return; // Ensure only the local player can shoot

        if (CanShoot() && Input.GetKeyDown(KeyCode.Space) && gameObject.CompareTag("Player1"))
        {
            RequestShootServerRpc();
        }
        else if (CanShoot() && Input.GetKeyDown(KeyCode.Return) && gameObject.CompareTag("Player2"))
        {
            RequestShootServerRpc();
        }
    }

    bool CanShoot()
    {
        return Time.time >= lastFireTime + fireCooldown;
    }

    [ServerRpc]
    private void RequestShootServerRpc(ServerRpcParams rpcParams = default)
    {
        lastFireTime = Time.time;

        GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        NetworkObject bulletNetworkObject = bulletInstance.GetComponent<NetworkObject>();

        if (bulletNetworkObject != null)
        {
            bulletNetworkObject.Spawn(true); // Spawn bullet across the network
        }

        PlayShootSoundClientRpc();
    }

    [ClientRpc]
    private void PlayShootSoundClientRpc()
    {
        if (shootSound != null)
        {
            AudioSource.PlayClipAtPoint(shootSound, firePoint.position, shootVolume);
        }
    }
}

