using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;  // Speed of bullet
    public float lifeTime = 2f; // Destroy bullet after 2 seconds

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed; // Move in tank's facing direction
        Destroy(gameObject, lifeTime); // Auto-destroy bullet after some time
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            TankHealth tankHealth = other.GetComponent<TankHealth>();
            if (tankHealth != null)
            {
                tankHealth.TakeDamage(1); // Reduce health by 1
            }
            Destroy(gameObject); // Destroy bullet
        }
        else if (other.CompareTag("Obstacle")) // If bullet hits an obstacle
        {
            Debug.Log("Bullet hit an obstacle!"); // Debugging message
            Destroy(gameObject); // Destroy bullet
        }
    }
}
