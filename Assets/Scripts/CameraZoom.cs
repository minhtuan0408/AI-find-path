using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 10f;         // Tốc độ zoom
    public float minZoom = 5f;            // Giới hạn gần nhất
    public float maxZoom = 60f;           // Giới hạn xa nhất

    private Camera cam;

    public float dragSpeed = 10f;
    private Vector3 lastMousePosition;
    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("CameraZoom script phải gắn vào một Camera!");
        }
    }

    void Update()
    {
        HandleDrag();
        HandleZoom();
    }
    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Với Camera Orthographic (2D)
        if (cam.orthographic)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
        // Với Camera Perspective (3D)
        else
        {
            cam.fieldOfView -= scroll * zoomSpeed;
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minZoom, maxZoom);
        }
    }
    void HandleDrag()
    {
        if (Input.GetMouseButtonDown(1)) // Chuột phải được nhấn
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(1)) // Giữ chuột phải
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 move = new Vector3(-delta.x, -delta.y, 0) * dragSpeed * Time.deltaTime;

            // Di chuyển camera theo hướng ngang (X, Y), không thay đổi Z
            transform.Translate(move, Space.Self);

            lastMousePosition = Input.mousePosition;
        }
    }
}
