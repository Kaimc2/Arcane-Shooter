using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ReactionDatabase : MonoBehaviour
{
    public static ReactionDatabase Instance;
    public Dictionary<string, Action<GameObject, StatusEffect, StatusEffect>> Reactions;
    public AudioSource audioSource;

    [Header("Smoke Properties")]
    public GameObject smokeVFX;

    [Header("Overload Properties")]
    public GameObject explosionVFX;
    public Fireball fireballProperties;
    public AudioClip impactClip;
    private readonly Collider[] _hitTargets = new Collider[20];

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        InitializeReaction();
    }

    public void InitializeReaction()
    {
        Reactions = new() {
            { NormalizeKey("Fire", "Water"), ExtinguishFire },
            { NormalizeKey("Fire", "Electric"), Overload },
        };
    }

    public static string NormalizeKey(string key1, string key2)
    {
        return string.Compare(key1, key2) < 0 ? $"{key1}+{key2}" : $"{key2}+{key1}";
    }

    public void ExtinguishFire(GameObject target, StatusEffect fire, StatusEffect water)
    {
        GameObject smoke = Instantiate(smokeVFX, target.transform.position, transform.rotation);
        Destroy(smoke, 3f);

        fire.RemoveEffect();
        water.RemoveEffect();
    }

    public void Overload(GameObject target, StatusEffect fire, StatusEffect electric)
    {
        Explosion(target);

        fire.RemoveEffect();
        electric.RemoveEffect();
    }
    private void Explosion(GameObject target)
    {
        GameObject explosion = Instantiate(explosionVFX, target.transform.position, target.transform.rotation);
        Destroy(explosion, 2f);

        // Apply damage to every objects within the blast radius
        int numTargets = Physics.OverlapSphereNonAlloc(target.transform.position, fireballProperties.blastRadius, _hitTargets);
        for (int i = 0; i < numTargets; i++)
        {
            Collider explosionTarget = _hitTargets[i];
            Rigidbody objectRb = explosionTarget.GetComponent<Rigidbody>();
            if (objectRb != null && !explosionTarget.CompareTag("Projectile"))
            {
                objectRb.AddExplosionForce(fireballProperties.force, target.transform.position, fireballProperties.blastRadius);
            }

            // Apply damage
            if (explosionTarget.CompareTag("Player"))
            {
                PlayerManager playerManager = explosionTarget.GetComponent<PlayerManager>();
                playerManager.TakeDamage(fireballProperties.damage, "exploded", "", KillMesssageType.StatusEffect);
            }
            else if (explosionTarget.CompareTag("Enemy") || explosionTarget.CompareTag("NPC"))
            {
                AIController aiController = explosionTarget.GetComponent<AIController>();
                aiController.TakeDamage(fireballProperties.damage, "exploded", "", KillMesssageType.StatusEffect);
            }
        }

        // Play impact sound effect 
        if (impactClip != null) audioSource.PlayOneShot(impactClip);
    }
}