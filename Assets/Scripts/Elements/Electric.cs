using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electric : Projectile
{
    public GameObject lightningStrikeVFXPrefab;
    public float knockbackForce = 5f; // Knockback force to apply
    public float duration = 5f;

    private bool _hasCollision = false;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Electric hit {other.gameObject.name}");

        GameObject lightningStrike = Instantiate(lightningStrikeVFXPrefab, transform.position, transform.rotation);
        Destroy(lightningStrike, 2f);

        // Play impact sound effect 
        if (impactClip != null) audioSource.PlayOneShot(impactClip);

        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Debug.Log("Electric Hit object: " + other.name);

            // Play impact sound effect 
            if (impactClip != null) audioSource.PlayOneShot(impactClip);

            // Apply damage
            Shock shockEffect = other.gameObject.AddComponent<Shock>();
            shockEffect.Initialize(other.gameObject, "Electric", duration);

            if (other.CompareTag("Player"))
            {
                PlayerManager playerManager = other.GetComponent<PlayerManager>();
                playerManager.TakeDamage(damage);

                ReactionManager.ApplyEffect(other.gameObject, shockEffect);
            }
            else if (other.CompareTag("Enemy") || other.CompareTag("NPC"))
            {
                AIController aiController = other.GetComponent<AIController>();
                aiController.TakeDamage(damage);

                ReactionManager.ApplyEffect(other.gameObject, shockEffect);
            }

            // Calculate knockback direction
            Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;

            // Apply force
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

            // Optionally set velocity for dramatic movement
            rb.velocity = knockbackDirection * knockbackForce;
        }

        Destroy(gameObject);
    }
}
