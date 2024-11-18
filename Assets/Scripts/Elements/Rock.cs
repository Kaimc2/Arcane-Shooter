using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Projectile
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Rock hit {other.gameObject.name}");
        Destroy(gameObject);
    }
}
