using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerController _playerController;
    public static float mouseSensitivity = 80f;
    public Transform playerBody;
    public Transform playerHead;

    public Vector3 offsets;
    private float _xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _playerController = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        Vector3 cameraOffsets = playerHead.forward * offsets.z + playerHead.up * offsets.y + playerHead.right * offsets.x;
        transform.position = playerHead.position + cameraOffsets;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Calculate the camera rotation and set the rotation limits
        _xRotation = _playerController.isDead ? 0f : _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -50f, 40f);

        // Rotate the player to follow the mouse position
        if (!_playerController.isDead) playerBody.Rotate(Vector3.up * mouseX);

        // Rotate the camera smoothly 
        transform.rotation = _playerController.isDead 
            ? Quaternion.Euler(_xRotation, 90, 0) 
            : Quaternion.Euler(_xRotation, playerBody.eulerAngles.y, 0);
    }
}
