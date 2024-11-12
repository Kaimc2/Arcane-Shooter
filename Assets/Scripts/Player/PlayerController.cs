using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController _playerController;
    private PlayerAnimator _playerMovement;
    public InputActionReference movementInput;
    public InputActionReference fireInput;

    public Transform projectilePrefab;
    public Transform projectileSpawnPosition;
    public LayerMask projectileColliderLayerMask;

    [Header("Player Properties")]
    public float speed = 4f;
    public float jumpHeight = 3f;
    private readonly float _gravity = -9.81f;

    [Header("Collission Settings")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool _isGrounded;

    private Vector2 _movement;
    private Vector3 _velocity;
    private Vector3 _worldMousePosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = GetComponent<CharacterController>();
        _playerMovement = GetComponent<PlayerAnimator>();
    }

    void OnEnable()
    {
        fireInput.action.started += Fire;
    }

    void OnDisable()
    {
        fireInput.action.started -= Fire;
    }

    private void Fire(InputAction.CallbackContext context)
    {
        Vector3 aimDir = (_worldMousePosition - projectileSpawnPosition.position).normalized;
        Instantiate(projectilePrefab, projectileSpawnPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
    }

    // Update is called once per frame
    void Update()
    {
        _movement = movementInput.action.ReadValue<Vector2>();
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Apply force to pull the player back down
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -6f;
        }

        Move();

        // Aiming
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, projectileColliderLayerMask))
        {
            _worldMousePosition = raycastHit.point;
        }
    }

    private void Move()
    {
        Vector3 move = transform.right * _movement.x + transform.forward * _movement.y;
        _playerController.Move(speed * Time.deltaTime * move);

        // Handle sprinting
        if (_playerMovement.IsSprinting && _isGrounded)
        {
            speed = 8f;
        }
        else
        {
            speed = 4f;
        }

        // Handle Jumping
        if (_playerMovement.IsJumping && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * _gravity);
        }
        _velocity.y += _gravity * Time.deltaTime;
        _playerController.Move(_velocity * Time.deltaTime);
    }
}
