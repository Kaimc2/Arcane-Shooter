using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electric : Projectile
{
    public float knockbackForce = 5f; // Knockback force to apply

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Electric hit {other.gameObject.name}");

        Debug.Log($"Playing sound {impactClip}");
        // Play impact sound
        if (impactClip != null) audioSource.PlayOneShot(impactClip);

        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Debug.Log("Electric Hit object: " + other.name);

            // // Calculate knockback direction
            // Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;

            // // Apply force
            // rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

            // // Optionally set velocity for dramatic movement
            // rb.velocity = knockbackDirection * knockbackForce;
        }

        Destroy(gameObject);
    }
}
