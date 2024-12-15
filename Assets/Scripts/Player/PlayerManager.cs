using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public UIManager uIManager;
    public Animator animator;

    [Header("Player Stats")]
    public float maxHealth = 200f;
    public float health;

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
        uIManager.UpdateHealth(maxHealth, maxHealth);
        animator = GetComponent<Animator>();

        Debug.Log($"Initial Fall Damage Threshold: {fallDamageThreshold:F3}");
    }

    void Update()
    {
        CheckFalling();
    }

    private void CheckFalling()
    {
        RaycastHit hit;
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f);

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
                TakeDamage(damage);
            }
            else
            {
                Debug.Log("Fatal Fall!");
                health = 0;
                Die();
            }

            Debug.Log($"Fall Distance: {fallDistance}, Damage: {damage}");
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        uIManager.UpdateHealth(health, maxHealth);
        if (health <= 0) Die();
    }

    private void Die()
    {
        Debug.Log("You Died");
        GameManager.Instance.UpdateScore(gameObject.CompareTag("Enemy"));
        uIManager.GameHub.SetActive(false);

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
