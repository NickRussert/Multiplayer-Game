using UnityEngine;

public class TankShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Assign Bullet Prefab in Inspector
    public Transform firePoint; // Empty GameObject placed at tank's cannon

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && gameObject.CompareTag("Player1"))
        {
            Shoot();
        }
        else if (Input.GetKeyDown(KeyCode.Return) && gameObject.CompareTag("Player2"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Instantiate the bullet at the fire point
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Ensure bullet rotates correctly to face up/down (Y-axis movement)
        bullet.transform.rotation = transform.rotation;
    }
}
