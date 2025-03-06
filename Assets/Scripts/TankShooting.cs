using UnityEngine;

public class TankShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Assign Bullet Prefab in Inspector
    public Transform firePoint; // Empty GameObject placed at tank's cannon
    public float fireCooldown = 1f; // Cooldown time between shots

    private float lastFireTime; // Tracks when the last shot was fired

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
        return Time.time >= lastFireTime + fireCooldown; // Ensures cooldown is over
    }

    void Shoot()
    {
        lastFireTime = Time.time; // Update last shot time

        // Instantiate the bullet at the fire point
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Ensure bullet rotates correctly to face the right direction
        bullet.transform.rotation = transform.rotation;

        Debug.Log(gameObject.name + " fired a bullet!");
    }
}

