using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    public UIManager uiManager;
    public Transform[] weapons;

    public InputActionReference numberInput;
    public InputActionReference scrollInput;
    public InputActionReference fireInput;
    public InputActionReference reloadInput;

    private int _currentWeaponIndex = 0;
    private Staff[] _weaponScripts;
    private Action<InputAction.CallbackContext> _currentFireAction;
    private Action<InputAction.CallbackContext> _currentStopFireAction;
    private Vector2 _scrollAxis;

    void Awake()
    {
        // Cache all staff scripts for performance
        _weaponScripts = new Staff[weapons.Length];

        for (int i = 0; i < weapons.Length; i++)
        {
            _weaponScripts[i] = weapons[i].GetComponent<Staff>();

            if (weapons[i] == null)
            {
                Debug.LogWarning($"No staff script found on {weapons[i].name}");
            }
        }

        // Initialize the weapon UI
        uiManager.UpdateWeaponUI(_weaponScripts[_currentWeaponIndex].gameObject.name);
        uiManager.UpdateMana(1, 1);
    }

    void OnEnable()
    {
        // Initialize the shooting function
        SetCurrentWeaponFiringAction();
        fireInput.action.performed += _currentFireAction;
    }

    void OnDisable()
    {
        fireInput.action.performed -= _currentFireAction;
    }

    // Update is called once per frame
    void Update()
    {
        _scrollAxis = scrollInput.action.ReadValue<Vector2>();

        // Reload weapon
        if (reloadInput.action.IsPressed())
        {
            Staff currentStaff = GetCurrentWeapon();
            if (currentStaff.mana < currentStaff.maxMana) currentStaff.RechargeStaff();
        }

        // Switch weapon by numbers
        if (numberInput.action.IsPressed())
        {
            _currentWeaponIndex = (int)numberInput.action.ReadValue<float>();
            SwitchWeapon(_currentWeaponIndex);
        }

        // Switch weapon by scroll wheel
        if (scrollInput.action.IsInProgress())
        {
            SwitchWeaponByScroll();
        }
    }

    private void SwitchWeaponByScroll()
    {
        // Increment or decrement the weapon index when scrolling
        if (_scrollAxis.y >= 120 && _currentWeaponIndex < weapons.Length - 1)
        {
            _currentWeaponIndex += 1;
        }
        else if (_scrollAxis.y <= -120 && _currentWeaponIndex > 0)
        {
            _currentWeaponIndex -= 1;
        }

        SwitchWeapon(_currentWeaponIndex);
    }

    private void SwitchWeapon(int weaponIndex)
    {
        // Unsubscribe from previous weapon's Fire function 
        fireInput.action.performed -= _currentFireAction;
        fireInput.action.canceled -= _currentStopFireAction;

        // Activate the selected weapon
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(weaponIndex == i);
        }

        _currentWeaponIndex = weaponIndex;
        uiManager.UpdateWeaponUI(_weaponScripts[weaponIndex].gameObject.name);
        uiManager.UpdateCooldownOverlay(0, _weaponScripts[_currentWeaponIndex].cooldown);

        // Subscribe to the selected weapon's Fire function 
        SetCurrentWeaponFiringAction();
        fireInput.action.performed += _currentFireAction;
        fireInput.action.canceled += _currentStopFireAction;

        // Debug.Log($"Current Weapon: {weapons[_currentWeaponIndex].name}");
    }

    private void SetCurrentWeaponFiringAction()
    {
        Staff weapon = _weaponScripts[_currentWeaponIndex];
        weapon.isRecharging = false;
        uiManager.UpdateMana(weapon.mana, weapon.maxMana);

        if (weapon != null)
        {
            _currentFireAction = ctx => weapon.Fire();
            _currentStopFireAction = ctx => weapon.StopFire();
        }
        else
        {
            Debug.LogWarning("No Staff script attached to the selected staff");
            _currentFireAction = null;
            _currentStopFireAction = null;
        }
    }

    public Staff GetCurrentWeapon()
    {
        return _weaponScripts[_currentWeaponIndex];
    }
}
