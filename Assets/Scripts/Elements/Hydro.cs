using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydro : Projectile
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Water Orb hit {other.gameObject.name}");
        Destroy(gameObject);
    }
}
