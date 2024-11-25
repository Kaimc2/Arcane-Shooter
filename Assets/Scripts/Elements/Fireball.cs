using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fireball : Projectile
{
    public GameObject explosionVFX;
    public float blastRadius = 5f;
    public float force = 700f;
    private readonly Collider[] _hitTargets = new Collider[10];
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
            if (explosionTarget.CompareTag("Player"))
            {
                PlayerManager playerManager = explosionTarget.GetComponent<PlayerManager>();
                playerManager.TakeDamage(damage);
            }
            else if (explosionTarget.CompareTag("Enemy") || explosionTarget.CompareTag("Ally"))
            {
                AIController aiController = explosionTarget.GetComponent<AIController>();
                aiController.TakeDamage(damage);
            }

        }

        Debug.Log($"Fireball hit {other.gameObject.name}");

        // Play impact sound effect 
        if (impactClip != null) audioSource.PlayOneShot(impactClip);

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

        Destroy(gameObject, impactClip.length);
    }
}
