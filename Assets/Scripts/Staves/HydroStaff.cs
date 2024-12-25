using System;
using System.Collections;
using UnityEngine;

public class HydroStaff : Staff
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

            // Spawn the water 
            // TODO: if performance is needed consider pooling
            Transform projectile = Instantiate(projectilePrefab, projectileSpawnPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.shooter = gameObject.transform.root.gameObject;

            mana -= manaCost;
            if (gameObject.CompareTag("Player")) UIManager.Instance.UpdateMana(mana, maxMana);

            // Play sound effect
            if (fireClip) audioSource.PlayOneShot(fireClip);
        }
        else
        {
            RechargeStaff();
        }
    }
}
