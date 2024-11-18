using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireStaff : MonoBehaviour
{
  public Transform projectilePrefab;  // Reference to the projectile object

  public void Fire(Vector3 worldMousePosition, Transform projectileSpawnPosition)
  {
    Vector3 aimDir = (worldMousePosition - projectileSpawnPosition.position).normalized;
    Instantiate(projectilePrefab, projectileSpawnPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
  }
}
