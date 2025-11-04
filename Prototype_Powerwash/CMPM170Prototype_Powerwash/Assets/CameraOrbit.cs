using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;      // assign your CarBody here
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 80.0f;
    public float distanceMin = 2f;
    public float distanceMax = 10f;

    private float x = 0.0f;
    private float y = 0.0f;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraOrbit: No target assigned!");
            enabled = false;
            return;
        }

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        UpdateCameraPosition();
    }

    void LateUpdate()
    {
        if (!target) return;

        if (Input.GetMouseButton(1))
        {
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
        }

        // Allow full rotation around — no clamping
        y = (y + 360f) % 360f;

        // Zoom
        distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(y, x, 0);

        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + target.position;

        transform.rotation = rotation;
        transform.position = position;
    }
}
