using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody _projectileRigidbody;
    public AudioSource audioSource;
    public AudioClip impactClip;

    [Header("Projectile Properties")]
    public int damage = 0;
    public float speed = 0f;

    void Awake()
    {
        _projectileRigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _projectileRigidbody.velocity = transform.forward * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
