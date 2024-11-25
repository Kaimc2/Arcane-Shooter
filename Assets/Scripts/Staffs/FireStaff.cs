using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStaff : Staff
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

      // Spawn the fireball
      // TODO: if performance is needed consider pooling
      Instantiate(projectilePrefab, projectileSpawnPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
      mana -= manaCost;
      if (gameObject.CompareTag("Player")) weaponController.uiManager.UpdateMana(mana);

      // Play sound effect
      if (fireClip) audioSource.PlayOneShot(fireClip);
    }
    else
    {
      RechargeStaff();
    }
  }
}