using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

// public class ElectricStaff : Staff
// {
//   private float _lastFireTime = 0f;

//   [Header("Elements Effects")]
//   public GameObject lightningVFXPrefab;
//   private GameObject activeLightningVFX;
//   private Coroutine fireCoroutine; // To manage rapid fire

//   void Update()
//   {
//     // Update VFX position
//     if (activeLightningVFX != null)
//     {
//       activeLightningVFX.transform.position = projectileSpawnPosition.position;
//     }
//   }

//   private void StartFiring(InputAction.CallbackContext context)
//   {
//     if (projectilePrefab == null || projectileSpawnPosition == null)
//     {
//       Debug.LogWarning("Projectile or spawn location not assigned");
//       return;
//     }

//     if (fireCoroutine == null)
//     {
//       fireCoroutine = StartCoroutine(FireContinuously());
//     }

//     // Spawn VFX
//     if (activeLightningVFX == null)
//     {
//       activeLightningVFX = Instantiate(lightningVFXPrefab, projectileSpawnPosition.position, Quaternion.identity);
//       activeLightningVFX.transform.parent = projectileSpawnPosition;
//     }
//   }

//   private void StopFiring(InputAction.CallbackContext context)
//   {
//     if (fireCoroutine != null)
//     {
//       StopCoroutine(fireCoroutine);
//       fireCoroutine = null;
//     }

//     // Destroy VFX
//     if (activeLightningVFX != null)
//     {
//       Destroy(activeLightningVFX);
//       activeLightningVFX = null;
//     }
//   }

//   private IEnumerator FireContinuously()
//   {
//     while (true)
//     {
//       FireProjectile();
//       yield return new WaitForSeconds(cooldown);
//     }
//   }

//   public void FireProjectile()
//   {
//     // Check if enough time passed since last shot 
//     if (Time.time < _lastFireTime + cooldown)
//     {
//       return;
//     }

//     _lastFireTime = Time.time;

//     Vector3 aimDir = GetAimDirection();
//     Transform projectile = Instantiate(projectilePrefab, projectileSpawnPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));

//     // Add Rigidbody force
//     Rigidbody rb = projectile.GetComponent<Rigidbody>();
//     if (rb != null)
//     {
//       rb.AddForce(aimDir * 20f, ForceMode.Impulse);
//     }
//   }

//   public override void Fire()
//   {
//     StartFiring(default);
//   }

//   public override void StopFire()
//   {
//     StopFiring(default);
//   }
// }

public class ElectricStaff : Staff
{
    public override void Fire()
    {
        Vector3 aimDir = GetAimDirection();
        FiringLogic(aimDir);
    }

    public override void AIFire(Transform target)
    {
        Vector3 aiAimDir = (target.position - projectileSpawnPosition.position).normalized;
        FiringLogic(aiAimDir);
    }

    private void FiringLogic(Vector3 aimDir)
    {
        if (projectilePrefab == null || projectileSpawnPosition == null)
        {
            Debug.LogWarning("Projectile or spawn location not assigned");
            return;
        }

        if (isRecharging) return;

        if (mana >= manaCost)
        {
            // Check if enough time passed since last shot 
            isOnCooldown = Time.time < lastFireTime + cooldown;
            if (isOnCooldown) return;
            lastFireTime = Time.time;
            if (gameObject.CompareTag("Player")) StartCooldown(cooldown);

            // Spawn the electric 
            // TODO: if performance is needed consider pooling
            Instantiate(projectilePrefab, projectileSpawnPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            mana -= manaCost;
            if (gameObject.CompareTag("Player")) weaponController.uiManager.UpdateMana(mana, maxMana);

            // Play sound effect
            if (fireClip) audioSource.PlayOneShot(fireClip);
        }
        else
        {
            RechargeStaff();
        }
    }
}
