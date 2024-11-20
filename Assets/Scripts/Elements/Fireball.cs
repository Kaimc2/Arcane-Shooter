using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Projectile
{
    private bool _hasCollision = false;

    void OnTriggerEnter(Collider other)
    {
        if (_hasCollision) return;  // Prevent multiple triggers
        _hasCollision = true;

        Debug.Log($"Fireball hit {other.gameObject.name}");

        // Play impact sound effect 
        if (impactClip) audioSource.PlayOneShot(impactClip);

        // Disable collider to avoid further collision 
        Collider collider = GetComponent<Collider>();
        if (collider) collider.enabled = false;

        // Stop and Clear any particles
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particles)
        {
            particle.Stop();
            particle.Clear();
        }

        // Stop projectile movement
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb) rb.velocity = Vector3.zero;

        Destroy(gameObject, impactClip.length);
    }
}
