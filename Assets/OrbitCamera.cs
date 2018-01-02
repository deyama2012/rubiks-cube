using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [Header("Zoom")]
    public float zoom = 4;
    public float minZoom = 2;
    public float maxZoom = 5;
    public float zoomSpeed = 5;

    [Header("Rotation")]
    public float xSpeed = 120;
    public float ySpeed = 120;
    public float yMinLimit = 20;
    public float yMaxLimit = 70;

    float x, y;

    Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
    Vector3 lastPosition;

    Camera mainCam, childCam;

    /// /////////////////////////////////////////////////////////

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        mainCam = GetComponent<Camera>();
    }


    void LateUpdate()
    {
        // Orbiting (rotation)
        if (Input.GetMouseButton(1))
            Rotation();

        // Panning (movement)
        if (Input.GetMouseButtonDown(2))
            lastPosition = GetMouseGroundAlignedWorldPos();

        else if (Input.GetMouseButton(2))
            Panning();

        // Zoom
        else if (!Mathf.Approximately(Input.GetAxis("Mouse ScrollWheel"), 0f))
            Zooming();

        if (childCam != null)
            childCam.orthographicSize = mainCam.orthographicSize;
    }


    void Rotation()
    {
        Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
        float enter;

        if (groundPlane.Raycast(ray, out enter))
        {
            float distance = 20;

            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            Vector3 negativeDistance = new Vector3(0, 0, -distance);
            Vector3 position = rotation * negativeDistance + ray.GetPoint(enter);

            transform.rotation = rotation;
            transform.position = position;
        }
    }


    void Panning()
    {
        Vector3 delta = GetMouseGroundAlignedWorldPos() - lastPosition;

        Vector3 position = new Vector3(transform.position.x - delta.x, transform.position.y, transform.position.z - delta.z);
        transform.position = position;

        lastPosition = GetMouseGroundAlignedWorldPos();
    }


    void Zooming()
    {
        zoom = Mathf.Clamp(zoom - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, minZoom, maxZoom);
        mainCam.orthographicSize = zoom;
    }


    Vector3 GetMouseGroundAlignedWorldPos()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        float enter;

        if (groundPlane.Raycast(ray, out enter))
            return ray.GetPoint(enter);

        return Vector3.zero;
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