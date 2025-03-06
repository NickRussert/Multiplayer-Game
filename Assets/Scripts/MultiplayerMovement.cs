using UnityEngine;

public class MultiplayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed of forward/backward movement
    public float rotationSpeed = 200f;  // Speed of turning

    private Rigidbody2D rb;
    private float moveInput;
    private float rotateInput;

    public bool isPlayer1 = true; // Toggle in Unity: Player 1 , Player 2 

    [SerializeField] private float minX = -15f, maxX = 15f; // X boundaries
    [SerializeField] private float minY = -15f, maxY = 15f; // Y boundaries

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isPlayer1)
        {
            // Player 1 (WASD)
            moveInput = Input.GetAxisRaw("Vertical");  // W/S = Forward/Backward
            rotateInput = Input.GetAxisRaw("Horizontal");  // A/D = Rotate Left/Right
        }
        else
        {
            // Player 2 (Arrow Keys)
            moveInput = Input.GetAxisRaw("Vertical2");  // Up/Down = Forward/Backward
            rotateInput = Input.GetAxisRaw("Horizontal2");  // Left/Right = Rotate
        }
    }

    void FixedUpdate()
    {
        // Move the tank forward/backward
        Vector2 movement = transform.right * moveInput * moveSpeed * Time.fixedDeltaTime;
        Vector2 newPosition = rb.position + movement;

        // Clamp position to keep within the map bounds
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        // Apply the movement (stays within boundaries)
        rb.MovePosition(newPosition);

        // Rotate the tank
        float rotation = -rotateInput * rotationSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation + rotation);
    }
}

