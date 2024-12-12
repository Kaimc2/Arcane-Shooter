using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fireball : Projectile
{
    public GameObject explosionVFX;
    public GameObject burningVFX;
    public float damageOverTime = 10f;
    public float duration = 5f;
    public float blastRadius = 5f;
    public float force = 700f;

    private readonly Collider[] _hitTargets = new Collider[20];
    private bool _hasCollision = false;

    void OnTriggerEnter(Collider other)
    {
        if (_hasCollision) return;  // Prevent multiple triggers
        _hasCollision = true;

        GameObject explosion = Instantiate(explosionVFX, transform.position, transform.rotation);
        Destroy(explosion, 2f);

        // Apply damage to every objects within the blast radius
        int numTargets = Physics.OverlapSphereNonAlloc(transform.position, blastRadius, _hitTargets);
        for (int i = 0; i < numTargets; i++)
        {
            Collider explosionTarget = _hitTargets[i];
            Rigidbody objectRb = explosionTarget.GetComponent<Rigidbody>();
            if (objectRb != null && !explosionTarget.CompareTag("Projectile"))
            {
                objectRb.AddExplosionForce(force, transform.position, blastRadius);
            }

            // Apply damage
            Burn burnEffect = explosionTarget.AddComponent<Burn>();
            burnEffect.Initialize(explosionTarget.gameObject, burningVFX, "Fire", damageOverTime, duration);
            if (explosionTarget.CompareTag("Player"))
            {
                PlayerManager playerManager = explosionTarget.GetComponent<PlayerManager>();
                playerManager.TakeDamage(damage);

                ReactionManager.ApplyEffect(explosionTarget.gameObject, burnEffect);
            }
            else if (explosionTarget.CompareTag("Enemy") || explosionTarget.CompareTag("NPC"))
            {
                AIController aiController = explosionTarget.GetComponent<AIController>();
                aiController.TakeDamage(damage);

                ReactionManager.ApplyEffect(explosionTarget.gameObject, burnEffect);
            }
        }

        // Debug.Log($"Fireball hit {other.gameObject.name}");

        // Play impact sound effect 
        if (impactClip != null) audioSource.PlayOneShot(impactClip);

        Cleanup();

        Destroy(gameObject, impactClip.length);
    }

    private void Cleanup()
    {
        // Disable properties after collision 
        Collider collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particles)
        {
            particle.Stop();
            particle.Clear();
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.velocity = Vector3.zero;
    }
}
