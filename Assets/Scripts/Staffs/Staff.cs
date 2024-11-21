using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Staff : MonoBehaviour
{
  [Header("References")]
  public Transform projectilePrefab;  // Reference to the projectile object
  public Transform projectileSpawnPosition; // Reference to the projectile spawn location
  public LayerMask projectileColliderLayerMask;
  public WeaponController weaponController;

  [Header("Staff Settings")]
  public float maxMana = 100f;
  public float mana;
  public float manaCost;
  public float rechargeSpeed;
  public float cooldown;
  public bool isOnCooldown;
  public bool isRecharging;
  protected float lastFireTime;

  [Header("SFX Properties")]
  public AudioSource audioSource;
  public AudioClip fireClip;

  void Awake()
  {
    audioSource = GetComponent<AudioSource>();
    mana = maxMana;
  }

  public abstract void Fire();
  public virtual void AIFire(Transform target) { }
  public virtual void StopFire() { }

  /// <summary>
  /// Calculates the aim direction based on mouse position and spawn position.
  /// </summary>
  /// <returns>A normalized direction vector.</returns>
  protected Vector3 GetAimDirection()
  {
    Vector3 worldMousePosition;

    Vector2 screenCenterPoint = new(Screen.width / 2f, Screen.height / 2f);
    Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
    if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, projectileColliderLayerMask))
    {
      worldMousePosition = raycastHit.point;
    }
    else
    {
      worldMousePosition = ray.GetPoint(10);
    }

    return (worldMousePosition - projectileSpawnPosition.position).normalized;
  }
}
