using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electric : Projectile
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Electric hit {other.gameObject.name}");
        Destroy(gameObject);
    }
}
