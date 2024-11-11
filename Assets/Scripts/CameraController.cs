using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 80f;
    public Transform playerBody;
    public Transform playerHead;

    public Vector3 offsets;
    private float _xRotation = 0f;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        transform.position = playerHead.position + offsets;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Calculate the camera rotation and set the rotation limits
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -50f, 40f);

        // Rotate the player to follow the mouse position
        playerBody.Rotate(Vector3.up * mouseX);

        // Rotate the camera to smoothly rotate around the player
        transform.rotation = Quaternion.Euler(_xRotation, playerBody.eulerAngles.y, 0);
    }
}
