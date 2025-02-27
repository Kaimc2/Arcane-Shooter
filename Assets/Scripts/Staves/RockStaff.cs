using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockStaff : Staff
{
    public override void Fire()
    {
        Vector3 aimDir = GetAimDirectionGround();
        FiringLogic(aimDir);
    }

    public override void AIFire(Transform target)
    {
        Vector3 midpoint = (transform.position + target.position) / 2;
        midpoint.y = 0;

        FiringLogic(midpoint);
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

            // Spawn the rock wall 
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
