using System.Collections;
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

    [Header("Collision Settings")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool _isGrounded;

    private Vector2 _movement;
    private Vector3 _velocity;
    private Vector3 _worldMousePosition = Vector3.zero;

    [Header("Elements Effects")]
    public GameObject lightningVFXPrefab;

    private GameObject activeLightningVFX;
    private Coroutine fireCoroutine; // To manage rapid fire

    void Start()
    {
        _playerController = GetComponent<CharacterController>();
        _playerMovement = GetComponent<PlayerAnimator>();
    }

    void OnEnable()
    {
        fireInput.action.started += StartFiring;
        fireInput.action.canceled += StopFiring;
    }

    void OnDisable()
    {
        fireInput.action.started -= StartFiring;
        fireInput.action.canceled -= StopFiring;
    }

    private void Fire(InputAction.CallbackContext context)
    {
        Vector3 aimDir = (_worldMousePosition - projectileSpawnPosition.position).normalized;
        Instantiate(projectilePrefab, projectileSpawnPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
    }

    private void StartFiring(InputAction.CallbackContext context)
    {
        if (fireCoroutine == null)
        {
            fireCoroutine = StartCoroutine(FireContinuously());
        }

        // Spawn VFX
        if (activeLightningVFX == null)
        {
            activeLightningVFX = Instantiate(lightningVFXPrefab, projectileSpawnPosition.position, Quaternion.identity);
            activeLightningVFX.transform.parent = projectileSpawnPosition;
        }
    }

    private void StopFiring(InputAction.CallbackContext context)
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }

        // Destroy VFX
        if (activeLightningVFX != null)
        {
            Destroy(activeLightningVFX);
            activeLightningVFX = null;
        }
    }

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            FireProjectile();
            yield return new WaitForSeconds(0.2f); // Adjust the fire rate as needed
        }
    }

    private void FireProjectile()
    {
        // Calculate direction
        Vector3 aimDir = (_worldMousePosition - projectileSpawnPosition.position).normalized;

        // Instantiate projectile
        Transform projectile = Instantiate(projectilePrefab, projectileSpawnPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));

        // Add Rigidbody force
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(aimDir * 20f, ForceMode.Impulse); // Adjust speed as needed
        }
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

        // Update aiming
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, projectileColliderLayerMask))
        {
            _worldMousePosition = raycastHit.point;
        }

        // Update VFX position
        if (activeLightningVFX != null)
        {
            activeLightningVFX.transform.position = projectileSpawnPosition.position;
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

        // Handle jumping
        if (_playerMovement.IsJumping && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * _gravity);
        }
        _velocity.y += _gravity * Time.deltaTime;
        _playerController.Move(_velocity * Time.deltaTime);
    }
}
