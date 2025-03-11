using UnityEngine;

public class MultiplayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed of forward/backward movement
    public float rotationSpeed = 200f;  // Speed of turning

    private Rigidbody2D rb;
    private float moveInput;
    private float rotateInput;

    public AudioClip moveSound; // Assign in Inspector
    private AudioSource audioSource;
    public float moveVolume = 0.2f; //  Lower movement sound volume

    public bool isPlayer1 = true; // Toggle in Unity: Player 1 , Player 2 

    [SerializeField] private float minX = -15f, maxX = 15f; // X boundaries
    [SerializeField] private float minY = -15f, maxY = 15f; // Y boundaries

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        // Setup looped engine sound with lower volume
        if (moveSound != null)
        {
            audioSource.clip = moveSound;
            audioSource.loop = true;
            audioSource.volume = moveVolume; //  Set lower volume
            audioSource.Play();
            audioSource.Pause(); // Start paused, unpause when moving or rotating
        }
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

        //  If moving or rotating, play sound
        if (moveInput != 0 || rotateInput != 0)
        {
            if (!audioSource.isPlaying)
                audioSource.UnPause();
        }
        else
        {
            audioSource.Pause(); // Stop sound when idle
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
