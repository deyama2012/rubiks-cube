using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [Header("Zoom")]
    public float zoom = 45;
    public float minZoom = 30;
    public float maxZoom = 60;
    public float zoomSpeed = 5000;

    [Header("Rotation")]
    public float xSpeed = 500;
    public float ySpeed = 500;

    float x, y;
    Camera mainCam;

    Vector3 defaultPos;
    Quaternion defaultRot;

    [Header("Turn table")]
    public bool turnTable;
    public float turnSpeed = 100;

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