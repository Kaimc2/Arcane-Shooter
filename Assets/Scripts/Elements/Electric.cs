using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electric : Projectile
{
    // void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log($"Electric hit {other.gameObject.name}");
    //     Destroy(gameObject);
    // }

    public ParticleSystem electricParticleSystem;
    public float knockbackForce = 10f;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Electric hit {other.gameObject.name}");
        Destroy(gameObject);
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log($"Particle hit {other.name}");
        // Add any additional logic you want to execute when a particle hits something
    }
}
