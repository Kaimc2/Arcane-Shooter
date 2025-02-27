using System;
using System.Collections;
using UnityEngine;

public class WindStaff : Staff
{
    public override void Fire()
    {
        Vector3 aimDir = GetAimDirectionGround();
        FiringLogic(aimDir);
    }

    public override void AIFire(Transform target)
    {
        FiringLogic(target.position);
    }

    private void FiringLogic(Vector3 aimDir)
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("Projectile not assigned");
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

            Instantiate(projectilePrefab, aimDir, Quaternion.identity);
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
