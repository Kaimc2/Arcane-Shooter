using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireStaff : Staff
{
  public override void Fire()
  {
    if (projectilePrefab == null || projectileSpawnPosition == null)
    {
      Debug.LogWarning("Projectile or spawn location not assigned");
      return;
    }

    Vector3 aimDir = GetAimDirection();

    // Check if enough time passed since last shot 
    isOnCooldown = Time.time < lastFireTime + cooldown;
    if (isOnCooldown)
    {
      return;
    }
    lastFireTime = Time.time;

    // Spawn the fireball
    Instantiate(projectilePrefab, projectileSpawnPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));

    // Play sound effect
    if (fireClip) audioSource.PlayOneShot(fireClip);
  }
}