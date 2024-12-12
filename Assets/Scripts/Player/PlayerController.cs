using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController _playerController;
    private PlayerAnimator _playerMovement;
    public InputActionReference movementInput;
    public UIManager uiManager;

    [Header("Player Properties")]
    public float maxStamina = 10f;
    public float stamina;
    public float speed = 4f;
    public float jumpHeight = 3f;
    public bool isDead;
    private bool _canSprint = true;
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
        stamina = maxStamina;
    }

    void Update()
    {
        if (!isDead) _movement = movementInput.action.ReadValue<Vector2>();
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -6f;
        }

        Move();
        Sprint();
        Jump();
    }

    private void Move()
    {
        Vector3 move = transform.right * _movement.x + transform.forward * _movement.y;

        if (!isDead) _playerController.Move(speed * Time.deltaTime * move);
    }

    private void Sprint()
    {
        // Handle sprinting
        if (_isGrounded && _canSprint && _playerMovement.IsSprinting && stamina > 0)
        {
            stamina -= 1 * Time.deltaTime;
            stamina = Mathf.Max(stamina, 0);
            uiManager.UpdateStamina(stamina, maxStamina);

            if (stamina <= 0)
            {
                _canSprint = false;
            }

            speed = 8f;
        }
        else
        {
            if (stamina < maxStamina)
            {
                stamina += 1 * Time.deltaTime;
                stamina = Mathf.Min(stamina, maxStamina);
                uiManager.UpdateStamina(stamina, maxStamina);
            }

            if (stamina > 1)
            {
                _canSprint = true;
            }

            speed = 4f;
        }
    }


    private void Jump()
    {
        // Handle jumping
        if (_playerMovement.IsJumping && _isGrounded && !isDead)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * _gravity);
        }

        _velocity.y += _gravity * Time.deltaTime;
        _playerController.Move(_velocity * Time.deltaTime);
    }

    public void ApplyJumpForce(float force)
    {
        _velocity.y = force;
    }
}