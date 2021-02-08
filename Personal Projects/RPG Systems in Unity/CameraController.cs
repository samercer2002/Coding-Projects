using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Sets the variable for what the camera is following
    public Transform target;

    // variable for the offset of the camera
    public Vector3 offset;

    // variables used for the camera's max, min, and speed of zooming
    public float zoomSpeed = 4f;
    public float minZoom = 5f;
    public float maxZoom = 15f;

    // Pitch of the camera
    public float pitch = 2f;

    // Rotation speed for horizontal camera rotation
    public float yawSpeed = 100f;

    // Current zoom of the camera
    private float currentZoom = 10f;

    // variable for the current horizontal position of the camera
    private float currentYaw = 0f;

    // Updates the variables used for camera position
    void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        currentYaw -= Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;
    }

    // Transforms the camera's position based upon the variables
    void LateUpdate()
    {
        transform.position = target.position - offset * currentZoom;
        transform.LookAt(target.position + Vector3.up * pitch);

        transform.RotateAround(target.position, Vector3.up, currentYaw);
    }
}
