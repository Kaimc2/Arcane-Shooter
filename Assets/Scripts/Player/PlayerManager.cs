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

        if (health <= 0) Die();
    }

    private void Die()
    {
        Debug.Log("You Died");

        if (animator != null) animator.SetBool("isDead", true);

        // Disable further inputs 
        // gameObject.GetComponent<PlayerController>().enabled = false;
        // gameObject.GetComponentInParent<WeaponController>().enabled = false;
        
        gameObject.layer = 0;
    }
}
