using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // Assign the tank to follow
    [SerializeField] private float smoothSpeed = 5f; // Adjust for smoother movement
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10); // Adjust camera position

    void LateUpdate()
    {
        if (target == null) return; // Prevent errors if no target is assigned

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget; // Change target dynamically if needed
    }
}
