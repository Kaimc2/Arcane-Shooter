using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimator : MonoBehaviour
{
    private PlayerControls playerControls; // Generated new Input action C# class
    public WeaponController weaponController;
    private Animator _animator;
    private PlayerController _playerController;

    private InputAction _moveAction;
    private InputAction _runAction;
    private InputAction _jumpAction;
    private InputAction _fireAction;

    public static bool active = true;

    public bool IsSprinting { get { return _runAction.IsPressed(); } }
    public bool IsJumping { get { return _jumpAction.WasPerformedThisFrame(); } }

    private void Awake()
    {
        playerControls = new PlayerControls();

        // Grab input actions
        _moveAction = playerControls.FindAction("Move");
        _runAction = playerControls.FindAction("Sprint");
        _jumpAction = playerControls.FindAction("Jump");
        _fireAction = playerControls.FindAction("Fire");
    }

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        // Enable player controls
        playerControls?.Enable();
    }

    private void OnDisable()
    {
        // Disable player controls
        playerControls?.Disable();
    }

    private void Update()
    {
        if (_playerController.isDead) return;
        if (!active) return;

        // Play animation
        WalkAnimation();
        RunAnimation();
        JumpAnimation();
        FireAnimation();
    }

    private void WalkAnimation()
    {
        if (_moveAction.WasPressedThisFrame())
        {
            _animator.SetBool("isWalking", true);
        }
        else if (_moveAction.WasReleasedThisFrame())
        {
            _animator.SetBool("isWalking", false);
        }
    }

    private void RunAnimation()
    {
        if (_runAction.WasPressedThisFrame())
        {
            _animator.SetBool("isRunning", true);
        }
        else if (_runAction.WasReleasedThisFrame())
        {
            _animator.SetBool("isRunning", false);
        }
    }

    private void JumpAnimation()
    {
        if (_jumpAction.WasPressedThisFrame())
        {
            _animator.SetBool("isJumping", true);
        }
        else if (_jumpAction.WasReleasedThisFrame())
        {
            _animator.SetBool("isJumping", false);
        }
    }

    private void FireAnimation()
    {
        Staff currentStaff = weaponController.GetCurrentWeapon();

        if (_fireAction.WasPressedThisFrame() && !currentStaff.isOnCooldown && !currentStaff.isRecharging)
        {
            _animator.SetBool("isFiring", true);
        }
        else if (_fireAction.WasReleasedThisFrame())
        {
            _animator.SetBool("isFiring", false);
        }
    }
}
