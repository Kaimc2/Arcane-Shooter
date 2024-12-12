using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Projectile
{
    public float duration = 5f;
    private float _startTime = 0f;

    void Start()
    {
        _startTime = Time.time;

        // Play impact sound effect 
        if (impactClip != null) audioSource.PlayOneShot(impactClip);
    }

    void Update()
    {
        if (Time.time > duration + _startTime)
        {
            Destroy(gameObject);
        }
    }
}
