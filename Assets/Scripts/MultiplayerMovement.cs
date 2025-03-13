using UnityEngine;
using Unity.Netcode;

public class MultiplayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;  // Speed of forward/backward movement
    public float rotationSpeed = 200f;  // Speed of turning

    private Rigidbody2D rb;
    private float moveInput;
    private float rotateInput;

    public AudioClip moveSound; // Assign in Inspector
    private AudioSource audioSource;
    public float moveVolume = 0.2f; // Lower movement sound volume

    [SerializeField] private float minX = -15f, maxX = 15f; // X boundaries
    [SerializeField] private float minY = -15f, maxY = 15f; // Y boundaries

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        //  Disable movement for non-owners (networked)
        if (!IsOwner)
        {
            enabled = false;
            return;
        }

        // Setup looped engine sound with lower volume
        if (moveSound != null)
        {
            audioSource.clip = moveSound;
            audioSource.loop = true;
            audioSource.volume = moveVolume; // Set lower volume
            audioSource.Play();
            audioSource.Pause(); // Start paused, unpause when moving or rotating
        }
    }

    void Update()
    {
        if (!IsOwner) return; //  Prevents input from non-owners

        //  Get movement input from the local player
        moveInput = Input.GetAxisRaw("Vertical");
        rotateInput = Input.GetAxisRaw("Horizontal");

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
        if (!IsOwner) return; //  Ensure only local player controls movement

        //  Move the tank forward/backward
        Vector2 movement = transform.right * moveInput * moveSpeed * Time.fixedDeltaTime;
        Vector2 newPosition = rb.position + movement;

        //  Clamp position to keep within the map bounds
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        // Apply the movement (stays within boundaries)
        rb.MovePosition(newPosition);

        //  Rotate the tank
        float rotation = -rotateInput * rotationSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation + rotation);
    }
}
