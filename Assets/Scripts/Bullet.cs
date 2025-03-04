using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;  // Speed of bullet
    public float lifeTime = 2f; // Destroy after 2 seconds

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Move bullet forward in the Y direction based on tank's rotation
        rb.velocity = transform.right * speed;

        // Destroy the bullet after some time
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the bullet hits a tank (any Player tag)
        if (other.CompareTag("Player") || other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            // Apply damage (or destroy the tank for now)
            TankHealth tankHealth = other.GetComponent<TankHealth>();
            if (tankHealth != null)
            {
                tankHealth.TakeDamage(1); // Reduce health by 1
            }

            Destroy(gameObject); // Destroy bullet on impact
        }
        else if (!other.CompareTag("Bullet")) // Ignore other bullets
        {
            Destroy(gameObject); // Destroy bullet when hitting walls/obstacles
        }
    }
}

