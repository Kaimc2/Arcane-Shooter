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
    private bool _alreadyDead;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        uIManager.UpdateHealth(maxHealth);
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        uIManager.UpdateHealth(health);

        if (health <= 0 && !_alreadyDead)
        {
            uIManager.UpdateHealth(0f);
            _alreadyDead = true;
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("You Died");
        // Update the enemy team score
        GameManager.Instance.UpdateScore(gameObject.CompareTag("Enemy"));

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
