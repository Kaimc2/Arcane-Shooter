using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : Projectile 
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Wind hit {other.gameObject.name}");
        Destroy(gameObject);
    }
}
