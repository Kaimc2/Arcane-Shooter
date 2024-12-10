using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class ElectricStaff : Staff
{
 private float _lastFireTime = 5f;

  public override void Fire()
  {
    if (projectilePrefab == null || projectileSpawnPosition == null)
    {
      Debug.LogWarning("Projectile or spawn location not assigned");
      return;
    }

    Vector3 aimDir = GetAimDirectionGround();

    // Check if enough time passed since last shot 
    if (Time.time < _lastFireTime + cooldown)
    {
      return;
    }

    _lastFireTime = Time.time;

    Instantiate(projectilePrefab, aimDir, Quaternion.LookRotation(aimDir, Vector3.up));

    // Play sound effect
    if (fireClip) audioSource.PlayOneShot(fireClip);
  }
}
