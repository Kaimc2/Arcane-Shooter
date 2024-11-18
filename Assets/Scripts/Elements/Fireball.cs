using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Projectile
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Fireball hit {other.gameObject.name}");
        Destroy(gameObject);
    }
}
