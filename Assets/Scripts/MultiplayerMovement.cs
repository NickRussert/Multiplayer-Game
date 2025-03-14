using UnityEngine;
using Unity.Netcode;

public class MultiplayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 200f;

    private Rigidbody2D rb;
    private float moveInput;
    private float rotateInput;

    private Camera playerCamera;

    [Header("Player Movement Boundaries")]
    public float playerMinX = -14f, playerMaxX = 15f;
    public float playerMinY = -14f, playerMaxY = 15f;

    [Header("Camera Boundaries")]
    public float cameraMinX = -4.5f, cameraMaxX = 5.5f;
    public float cameraMinY = -9f, cameraMaxY = 10f;

    // List of spawn positions for players
    private static Vector2[] spawnPositions = new Vector2[]
    {
        new Vector2(-10f, 0f),  // Host spawn
        new Vector2(10f, 0f),   // First client
        new Vector2(-10f, 5f),  // Second client
        new Vector2(10f, 5f),   // Third client
    };

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (IsOwner)
        {
            RequestSpawnPositionServerRpc();
        }
    }

    void Update()
    {
        if (!IsOwner) return;

        moveInput = Input.GetAxisRaw("Vertical");
        rotateInput = Input.GetAxisRaw("Horizontal");

        MoveServerRpc(moveInput, rotateInput);

        // Make the camera follow the player but stay within camera bounds
        if (playerCamera != null)
        {
            float clampedX = Mathf.Clamp(transform.position.x, cameraMinX, cameraMaxX);
            float clampedY = Mathf.Clamp(transform.position.y, cameraMinY, cameraMaxY);
            playerCamera.transform.position = new Vector3(clampedX, clampedY, -10);
        }
    }

    [ServerRpc]
    private void RequestSpawnPositionServerRpc(ServerRpcParams rpcParams = default)
    {
        int clientIndex = (int)rpcParams.Receive.SenderClientId;

        if (clientIndex >= spawnPositions.Length)
        {
            clientIndex = spawnPositions.Length - 1;
        }

        Vector2 assignedPosition = spawnPositions[clientIndex];

        transform.position = assignedPosition;

        UpdateSpawnPositionClientRpc(assignedPosition);
    }

    [ClientRpc]
    private void UpdateSpawnPositionClientRpc(Vector2 newPosition)
    {
        if (!IsOwner)
        {
            transform.position = newPosition;
        }

        AssignCamera();
    }

    private void AssignCamera()
    {
        if (!IsOwner) return; // Ensure only the local player assigns the camera

        playerCamera = Camera.main;

        if (playerCamera == null)
        {
            playerCamera = FindObjectOfType<Camera>();
        }

        if (playerCamera != null)
        {
            Debug.Log("MultiplayerMovement: Camera assigned successfully!");
        }
        else
        {
            Debug.LogError("MultiplayerMovement: No Camera found in the scene! Make sure the Camera is tagged 'MainCamera'.");
        }
    }

    [ServerRpc]
    private void MoveServerRpc(float move, float rotate, ServerRpcParams rpcParams = default)
    {
        if (!IsServer) return;

        Vector2 movement = transform.right * move * moveSpeed * Time.fixedDeltaTime;
        Vector2 newPosition = rb.position + movement;

        newPosition.x = Mathf.Clamp(newPosition.x, playerMinX, playerMaxX);
        newPosition.y = Mathf.Clamp(newPosition.y, playerMinY, playerMaxY);

        float newRotation = rb.rotation - (rotate * rotationSpeed * Time.fixedDeltaTime);

        rb.MovePosition(newPosition);
        rb.MoveRotation(newRotation);

        UpdatePositionClientRpc(newPosition, newRotation);
    }

    [ClientRpc]
    private void UpdatePositionClientRpc(Vector2 newPosition, float newRotation)
    {
        if (IsOwner) return;

        rb.MovePosition(newPosition);
        rb.MoveRotation(newRotation);
    }
}



