using UnityEngine;

public class MultiplayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed of forward/backward movement
    public float rotationSpeed = 200f;  // Speed of turning

    private Rigidbody2D rb;
    private float moveInput;
    private float rotateInput;

    public bool isPlayer1 = true; // Toggle in Unity: Player 1 , Player 2 

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
        //  Move the tank forward/backward along the X-axis
        rb.velocity = transform.right * moveInput * moveSpeed;

        //  Rotate the tank left/right
        rb.angularVelocity = -rotateInput * rotationSpeed;
    }
}
