using System;
using System.Collections;
using UnityEngine;

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
    public float weight;  // This is used for AI weapon selection
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

    public void RechargeStaff()
    {
        if (isRecharging) return;

        isRecharging = true;
        // Debug.Log("Reloading");
        StartCoroutine(RechargeCoroutine());
    }

    private IEnumerator RechargeCoroutine()
    {
        float rechargeDuration = rechargeSpeed;
        float elapsed = 0f;

        while (elapsed < rechargeDuration)
        {
            mana = Mathf.Lerp(mana, maxMana, elapsed / rechargeDuration * Time.deltaTime);
            if (gameObject.CompareTag("Player")) UIManager.Instance.UpdateMana(mana, maxMana);
            elapsed += Time.deltaTime;
            yield return null;
        }

        mana = maxMana;
        if (gameObject.CompareTag("Player")) UIManager.Instance.UpdateMana(mana, maxMana);
        // Debug.Log("Finish reloading");

        isRecharging = false;
    }

    public void StartCooldown(float cooldownDuration)
    {
        StartCoroutine(CooldownRoutine(cooldownDuration));
    }

    private IEnumerator CooldownRoutine(float cooldownDuration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < cooldownDuration)
        {
            elapsedTime += Time.deltaTime;
            float remainingTime = cooldownDuration - elapsedTime;
            UIManager.Instance.UpdateCooldownOverlay(remainingTime, cooldownDuration);
            yield return null;
        }

        // Reset the overlay when cooldown end
        UIManager.Instance.UpdateCooldownOverlay(0, cooldownDuration);
    }

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

    protected Vector3 GetAimDirectionGround()
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

        return worldMousePosition;
    }
}
