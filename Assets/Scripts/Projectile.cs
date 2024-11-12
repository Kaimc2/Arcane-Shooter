using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody _projectileRigidbody;
    private Transform _projectileTransform;
    public float speed = 5f;
    public Color color = Color.red;

    void Awake()
    {
        _projectileRigidbody = GetComponent<Rigidbody>();
        _projectileTransform = GetComponent<Transform>();
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
