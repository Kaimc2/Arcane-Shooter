using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    public Transform[] weapons;
    public LayerMask projectileColliderLayerMask;

    public InputActionReference numberInput;
    public InputActionReference scrollInput;
    public InputActionReference fireInput;

    private int _currentWeaponIndex = 0;
    private Staff[] _weaponScripts;
    private Action<InputAction.CallbackContext> _currentFireAction;

    private Vector2 _scrollAxis;
    private Vector3 _worldMousePosition = Vector3.zero;

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
    }

    void OnEnable()
    {
        // Initialize the shooting function
        SetCurrentWeaponFiringAction();
        fireInput.action.started += _currentFireAction;
    }

    void OnDisable()
    {
        fireInput.action.started -= _currentFireAction;
    }

    // Update is called once per frame
    void Update()
    {
        _scrollAxis = scrollInput.action.ReadValue<Vector2>();

        // Create a raycast in the center of the screen for aiming 
        Vector2 screenCenterPoint = new(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, projectileColliderLayerMask))
        {
            _worldMousePosition = raycastHit.point;
        }
        else
        {
            _worldMousePosition = ray.GetPoint(10);
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
        fireInput.action.started -= _currentFireAction;

        // Activate the selected weapon
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(weaponIndex == i);
        }

        _currentWeaponIndex = weaponIndex;

        // Subscribe to the selected weapon's Fire function 
        SetCurrentWeaponFiringAction();
        fireInput.action.started += _currentFireAction;

        Debug.Log($"Current Weapon: {weapons[_currentWeaponIndex].name}");
    }

    private void SetCurrentWeaponFiringAction()
    {
        Staff weapon = _weaponScripts[_currentWeaponIndex];
        if (weapon != null)
        {
            _currentFireAction = ctx => weapon.Fire(_worldMousePosition); 
        }
        else
        {
            Debug.LogWarning("No Staff script attached to the selected staff");
            _currentFireAction = null;
        }
    }
}
