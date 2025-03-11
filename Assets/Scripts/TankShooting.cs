using UnityEngine;

public class TankShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireCooldown = 1f;
    private float lastFireTime;

    public AudioClip shootSound; // Assign in Inspector
    public float shootVolume = 0.5f; // Adjust shooting volume

    void Update()
    {
        if (CanShoot() && Input.GetKeyDown(KeyCode.Space) && gameObject.CompareTag("Player1"))
        {
            Shoot();
        }
        else if (CanShoot() && Input.GetKeyDown(KeyCode.Return) && gameObject.CompareTag("Player2"))
        {
            Shoot();
        }
    }

    bool CanShoot()
    {
        return Time.time >= lastFireTime + fireCooldown;
    }

    void Shoot()
    {
        lastFireTime = Time.time;
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Play shooting sound at the fire point (instead of tank)
        if (shootSound != null)
        {
            AudioSource.PlayClipAtPoint(shootSound, firePoint.position, shootVolume);
        }

        Debug.Log(gameObject.name + " fired a bullet!");
    }
}
