using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Animator animator;

    [Header("Player Stats")]
    public float maxHealth = 200f;
    public float health;
    private bool _alreadyDead = false;

    [Header("Falling System")]
    [SerializeField] 
    private int fallDamageThreshold = 2; // Minimum fall distance to take damage

    [SerializeField]
    private float fallDamageMultiplier = 20f; // Damage multiplier based on fall distance

    [SerializeField]
    private int maxFallDistance = 8; // Maximum fall distance without taking fatal damage

    private Vector3 lastGroundedPosition;
    private bool isFalling = false;
    private float fallStartHeight;

    void Start()
    {
        health = maxHealth;
        UIManager.Instance.UpdateHealth(maxHealth, maxHealth);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        CheckFalling();
    }

    private void CheckFalling()
    {
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.1f);

        if (!isGrounded && !isFalling)
        {
            isFalling = true;
            fallStartHeight = transform.position.y;
        }
        else if (isGrounded && isFalling)
        {
            float fallDistance = fallStartHeight - transform.position.y;
            HandleFallDamage(fallDistance);
            isFalling = false;
        }
    }

    private void HandleFallDamage(float fallDistance)
    {
        if (fallDistance > fallDamageThreshold)
        {
            float damage = (fallDistance - fallDamageThreshold) * fallDamageMultiplier;

            if (fallDistance < maxFallDistance)
            {
                TakeDamage(damage, "fell to their death", "", KillMesssageType.StatusEffect);
            }
            else
            {
                Debug.Log("Fatal Fall!");
                health = 0;
                Die("fell to their death", "", KillMesssageType.StatusEffect);
            }

            Debug.Log($"Fall Distance: {fallDistance}, Damage: {damage}");
        }
    }

    public void TakeDamage(float damage, string damageType, string killer, KillMesssageType type = KillMesssageType.Default)
    {
        health -= damage;
        UIManager.Instance.UpdateHealth(health, maxHealth);

        if (health <= 0 && !_alreadyDead)
        {
            _alreadyDead = true;
            Die(damageType, killer, type);
        }
    }

    private void Die(string damageType, string killer, KillMesssageType type)
    {
        if (type == KillMesssageType.Default)
        {
            KillFeedManager.Instance.AddNewMessage(killer, damageType, gameObject.name);
        }
        else
        {
            KillFeedManager.Instance.AddNewMessage(gameObject.name, damageType, "");
        }
        // Update the enemy team score
        GameManager.Instance.UpdateScore(gameObject.CompareTag("Enemy"));
        UIManager.Instance.GameHub.SetActive(false);

        if (animator != null)
        {
            animator.Rebind();
            animator.SetBool("isDead", true);
        }

        // Disable further inputs 
        gameObject.GetComponent<PlayerController>().isDead = true;
        gameObject.GetComponentInParent<WeaponController>().enabled = false;

        gameObject.layer = 0;
        Destroy(gameObject.transform.root.gameObject, 5f);
    }
}
