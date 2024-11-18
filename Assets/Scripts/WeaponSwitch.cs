using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitch : MonoBehaviour
{
    public Transform[] weapons;
    private int _currentWeapon = 0;
    public InputActionReference numberInput;
    public InputActionReference scrollInput;

    private Vector2 scrollAxis;

    // Update is called once per frame
    void Update()
    {
        scrollAxis = scrollInput.action.ReadValue<Vector2>();
        _currentWeapon = (int)numberInput.action.ReadValue<float>();

        Debug.Log("Current Weapon: " + weapons[_currentWeapon].name);

        if (numberInput.action.IsPressed())
        {
            ChangeWeapon(_currentWeapon);
        }

        if (scrollInput.action.IsInProgress())
        {
            SwitchWeapon();
        }
    }

    private void SwitchWeapon()
    {
        if (scrollAxis.y >= 120 && _currentWeapon < 4)
        {
            _currentWeapon += 1;
        }
        else
        {
            _currentWeapon = 0;
        }

        if (scrollAxis.y <= -120 && _currentWeapon > 0)
        {
            _currentWeapon -= 1;
        }
        else
        {
            _currentWeapon = 4;
        }

        ChangeWeapon(_currentWeapon);
    }

    private void ChangeWeapon(int weaponIndex)
    {
        weapons[weaponIndex].gameObject.SetActive(true);

        for (int i = 0; i < weapons.Length; i++)
        {
            if (i == weaponIndex)
            {
                continue;
            }

            weapons[i].gameObject.SetActive(false);
        }
    }
}
