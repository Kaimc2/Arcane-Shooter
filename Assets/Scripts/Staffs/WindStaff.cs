using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class WindStaff : Staff
{
  private float _lastFireTime = 0f;

  public override void Fire()
  {
    if (projectilePrefab == null || projectileSpawnPosition == null)
    {
      Debug.LogWarning("Projectile or spawn location not assigned");
      return;
    }

    Vector3 aimDir = GetAimDirection();

    // Check if enough time passed since last shot 
    if (Time.time < _lastFireTime + cooldown)
    {
      return;
    }

    _lastFireTime = Time.time;

    Instantiate(projectilePrefab, projectileSpawnPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
  }
}
