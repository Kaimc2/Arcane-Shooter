using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController _playerController;
    private PlayerAnimator _playerMovement;
    public InputActionReference movementInput;

    [Header("Player Properties")]
    public float speed = 4f;
    public float jumpHeight = 3f;
    private readonly float _gravity = -9.81f;

    [Header("Collision Settings")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool _isGrounded;

    private Vector2 _movement;
    private Vector3 _velocity;

    void Start()
    {
        _playerController = GetComponent<CharacterController>();
        _playerMovement = GetComponent<PlayerAnimator>();
    }

    void Update()
    {
        _movement = movementInput.action.ReadValue<Vector2>();
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -6f;
        }

        Move();
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

        // Handle jumping
        if (_playerMovement.IsJumping && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * _gravity);
        }

        _velocity.y += _gravity * Time.deltaTime;
        _playerController.Move(_velocity * Time.deltaTime);
    }
}