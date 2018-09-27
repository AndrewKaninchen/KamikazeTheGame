using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float rotatingSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private Vector2 bordersX;
    [SerializeField] private Vector2 bordersZ;
    
    [Header("Zoom")]
    [SerializeField, Range(0f, 1f)]
    private float zoomLevel;

    [SerializeField] private Vector3 zoom0, zoom1;
    [SerializeField] private float cameraAngleZoom0, cameraAngleZoom1;
    [SerializeField] private float zoomSpeed;

    private void Update()
    {
        var hor = Input.GetAxis("Horizontal");
        var vert = Input.GetAxis("Vertical");
        ApplyMovement(new Vector2(hor, vert));
        GetMouseRotation();
        ApplyZoom();
    }

    private void GetMouseRotation()
    {
        if (Input.GetMouseButtonDown(1))
            Cursor.lockState = CursorLockMode.Locked;
        
        if (Input.GetMouseButtonUp(1))
            Cursor.lockState = CursorLockMode.None;      
        
        if (Input.GetMouseButton(1))
            ApplyRotation(Input.GetAxisRaw("Mouse X"));
        
        if (Input.GetKey(KeyCode.Q))
            ApplyRotation(5);
        if (Input.GetKey(KeyCode.E))
            ApplyRotation(-5);
    }

    private void ApplyRotation (float hor)
    {
        var eulerAngles = new Vector3(0f, hor * rotatingSpeed * Time.deltaTime, 0f);
        transform.Rotate(eulerAngles);
    }

    private void ApplyMovement (Vector2 input)
    {

        var targetMovement = new Vector3(input.x, 0f, input.y) * movementSpeed * (1/(zoomLevel+1)) * Time.deltaTime;
        transform.Translate(targetMovement, Space.Self);

        transform.position = new Vector3
        (
            Mathf.Clamp(transform.position.x, bordersX.x, bordersX.y),
            0f,
            Mathf.Clamp(transform.position.z, bordersZ.x, bordersZ.y)
        );
    }

    private void ApplyZoom()
    {
        zoomLevel = Mathf.Clamp01(zoomLevel + Input.mouseScrollDelta.y * zoomSpeed);
        transform.GetChild(0).localPosition = Vector3.Lerp(zoom0, zoom1, zoomLevel);
        transform.GetChild(0).localRotation = Quaternion.Lerp(Quaternion.Euler(cameraAngleZoom0, 0f, 0f), Quaternion.Euler(cameraAngleZoom1, 0f, 0f), zoomLevel);
    }
}
