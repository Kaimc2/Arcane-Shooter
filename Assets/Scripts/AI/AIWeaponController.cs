using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class AIWeaponController : MonoBehaviour
{
    public Transform[] weapons;
    private int _currentWeaponIndex = 0;
    private Staff[] _weaponScripts;
    public float switchInterval;
    private float _nextSwitchTime;

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

    void Update()
    {
        // Change the AI weapon in an interval
        if (Time.time > _nextSwitchTime)
        {
            SwitchWeapon();
            _nextSwitchTime = Time.time + Random.Range(switchInterval - 3f, switchInterval + 3f);
        }
    }

    private void SwitchWeapon()
    {
        float totalWeight = 0f;
        // Calculate total weapon weight
        foreach (Staff weapon in _weaponScripts)
        {
            totalWeight += weapon.weight;
        }

        float randomValue = Random.Range(0, totalWeight);
        float cumalativeWeight = 0f;

        for (int i = 0; i < weapons.Length; i++)
        {
            cumalativeWeight += _weaponScripts[i].weight;
            if (randomValue <= cumalativeWeight)
            {
                _currentWeaponIndex = i;
                // Set the selected weapon active and the rest inactive
                for (int j = 0; j < weapons.Length; j++)
                {
                    weapons[j].gameObject.SetActive(_currentWeaponIndex == j);
                }
                _weaponScripts[_currentWeaponIndex].isRecharging = false;

                Debug.Log($"{gameObject.name} switch weapon to {_weaponScripts[_currentWeaponIndex]}");
                break;
            }
        }
    }

    public Staff GetCurrentWeapon()
    {
        return _weaponScripts[_currentWeaponIndex];
    }
}
