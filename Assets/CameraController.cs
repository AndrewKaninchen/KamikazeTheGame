using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float rotatingSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private Vector2 bordersX;
    [SerializeField] private Vector2 bordersZ;
    [SerializeField, Range(1f, 4f)]
    private float zoomLevel;

    private void Update()
    {
        var hor = Input.GetAxis("Horizontal");
        var vert = Input.GetAxis("Vertical");
        ApplyMovement(new Vector2(hor, vert));
        ApplyMouseRotation();
    }

    private Vector3 originalMousePos;
    private void ApplyMouseRotation()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            originalMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(1))
        {
            Cursor.lockState = CursorLockMode.None;           
        }
        if (Input.GetMouseButton(1))
        {
            ApplyRotation(Input.GetAxisRaw("Mouse X"));
        }
    }

    private void ApplyRotation (float hor)
    {
        var eulerAngles = new Vector3(0f, hor * rotatingSpeed * Time.deltaTime, 0f);
        transform.Rotate(eulerAngles);
    }

    private void ApplyMovement (Vector2 input)
    {

        var targetMovement = new Vector3(input.x, 0f, input.y) * movementSpeed * Time.deltaTime;
        transform.Translate(targetMovement, Space.Self);

        transform.position = new Vector3
        (
            Mathf.Clamp(transform.position.x, bordersX.x, bordersX.y),
            0f,
            Mathf.Clamp(transform.position.z, bordersZ.x, bordersZ.y)
        );
    }
}
