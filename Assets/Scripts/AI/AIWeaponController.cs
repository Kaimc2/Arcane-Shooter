using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeaponController : MonoBehaviour
{
    public Transform[] weapons;
    private int _currentWeaponIndex = 0;
    private Staff[] _weaponScripts;

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

    public Staff GetCurrentWeapon()
    {
        return _weaponScripts[_currentWeaponIndex];
    }
}
