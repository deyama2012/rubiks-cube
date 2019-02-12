using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [Header("Zoom")]
    [SerializeField] private float zoom = 45;
    [SerializeField] private float minZoom = 30;
    [SerializeField] private float maxZoom = 60;
    [SerializeField] private float zoomSpeed = 5000;

    [Header("Rotation")]
    [SerializeField] private float xSpeed = 500;
    [SerializeField] private float ySpeed = 500;

    private float x, y;
    private Camera mainCam;

    private Vector3 defaultPos;
    private Quaternion defaultRot;

    [Header("Turn table")]
    [SerializeField] private bool turnTable;
    [SerializeField] private float turnSpeed = 100;

    /// /////////////////////////////////////////////////////////

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        mainCam = GetComponent<Camera>();

        defaultPos = mainCam.transform.position;
        defaultRot = mainCam.transform.rotation;
    }


    void LateUpdate()
    {
        // Orbiting (rotation)
        if (Input.GetMouseButton(1) || turnTable)
            Rotation();

        // Zoom
        else if (!Mathf.Approximately(Input.GetAxis("Mouse ScrollWheel"), 0f))
            Zooming();

        else if (Input.GetMouseButtonDown(2))
            ResetCamera();
    }


    void Rotation()
    {
        if (turnTable)
            x += turnSpeed * Time.deltaTime;

        else
        {
            x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
            y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
        }

        Quaternion rotation = Quaternion.Euler(y, x, 0);

        Vector3 negativeDistance = new Vector3(0, 0, -10);
        Vector3 position = rotation * negativeDistance;

        transform.rotation = rotation;
        transform.position = position;
    }


    void Zooming()
    {
        zoom = Mathf.Clamp(zoom - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime, minZoom, maxZoom);
        mainCam.fieldOfView = zoom;
    }


    void ResetCamera()
    {
        transform.position = defaultPos;
        transform.rotation = defaultRot;

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        zoom = (minZoom + maxZoom) / 2;
        mainCam.fieldOfView = zoom;
    }


    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360f;
        if (angle > 360)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}