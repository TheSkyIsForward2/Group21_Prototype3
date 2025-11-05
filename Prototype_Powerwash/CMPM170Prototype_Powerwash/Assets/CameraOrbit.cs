using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;
    
    [Header("Rotation Speeds")]
    public float keyRotationSpeed = 100.0f;
    public float mouseXSpeed = 250.0f;
    public float mouseYSpeed = 120.0f;

    [Header("Zoom Settings")]
    public float distance = 5.0f;
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

        if (Input.GetKey(KeyCode.A)) x += keyRotationSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) x -= keyRotationSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.W)) y += keyRotationSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S)) y -= keyRotationSpeed * Time.deltaTime;

        if (Input.GetMouseButton(1))
        {
            x += Input.GetAxis("Mouse X") * mouseXSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * mouseYSpeed * 0.02f;
        }

        y = Mathf.Clamp(y, -80f, 80f);

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