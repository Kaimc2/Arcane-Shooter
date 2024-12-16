using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electric : Projectile
{
    public GameObject lightningStrikeVFXPrefab;
    public float duration = 5f;

    void OnTriggerEnter(Collider other)
    {
        GameObject lightningStrike = Instantiate(lightningStrikeVFXPrefab, transform.position, transform.rotation);
        Destroy(lightningStrike, 2f);

        // Play impact sound effect 
        if (impactClip != null) audioSource.PlayOneShot(impactClip);

        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Play impact sound effect 
            if (impactClip != null) audioSource.PlayOneShot(impactClip);

            // Apply damage
            Shock shockEffect = other.gameObject.AddComponent<Shock>();
            shockEffect.Initialize(other.gameObject, "Electric", duration);

            if (other.CompareTag("Player"))
            {
                PlayerManager playerManager = other.GetComponent<PlayerManager>();
                playerManager.TakeDamage(damage, "electrocuted", shooter.name);

                ReactionManager.ApplyEffect(other.gameObject, shockEffect);
            }
            else if (other.CompareTag("Enemy") || other.CompareTag("NPC"))
            {
                AIController aiController = other.GetComponent<AIController>();
                aiController.TakeDamage(damage, "electrocuted", shooter.name);

                ReactionManager.ApplyEffect(other.gameObject, shockEffect);
            }
        }

        Destroy(gameObject);
    }
}
