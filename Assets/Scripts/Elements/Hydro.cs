using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydro : Projectile
{
    public float duration = 5f;

    void OnTriggerEnter(Collider other)
    {
        Wet wetEffect = other.gameObject.AddComponent<Wet>();
        wetEffect.Initialize(other.gameObject, "Water", duration);
        if (other.CompareTag("Player"))
        {
            PlayerManager playerManager = other.GetComponent<PlayerManager>();
            playerManager.TakeDamage(damage, "drowned", shooter.name);

            ReactionManager.ApplyEffect(other.gameObject, wetEffect);
        }
        else if (other.CompareTag("Enemy") || other.CompareTag("NPC"))
        {
            AIController aiController = other.GetComponent<AIController>();
            aiController.TakeDamage(damage, "drowned", shooter.name);

            ReactionManager.ApplyEffect(other.gameObject, wetEffect);
        }

        // Debug.Log($"Water Orb hit {other.gameObject.name}");
        Destroy(gameObject);
    }
}
