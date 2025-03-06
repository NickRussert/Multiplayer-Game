using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // The object to follow (tank)
    [SerializeField] private float smoothSpeed = 5f; // Adjust for smoother movement
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10); // Default camera offset

    [SerializeField] private float minX = -15f, maxX = 15f; // X limits
    [SerializeField] private float minY = -15f, maxY = 15f; // Y limits

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        // Clamp the position within the map bounds
        float clampedX = Mathf.Clamp(desiredPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(desiredPosition.y, minY, maxY);

        Vector3 clampedPosition = new Vector3(clampedX, clampedY, desiredPosition.z);

        transform.position = Vector3.Lerp(transform.position, clampedPosition, smoothSpeed * Time.deltaTime);
    }
}
