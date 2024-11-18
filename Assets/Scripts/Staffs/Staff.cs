using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Staff : MonoBehaviour
{
  public Transform projectilePrefab;  // Reference to the projectile object
  public Transform projectileSpawnPosition; // Reference to the projectile spawn location
  public float cooldown;

  public abstract void Fire(Vector3 worldMousePosition);
}
