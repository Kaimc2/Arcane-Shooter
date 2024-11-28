using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [Header("Target References")]
    public NavMeshAgent agent;
    public Transform target;
    public Animator animator;
    public LayerMask targetLayer, groundLayer;
    private AIWeaponController _weaponController;

    [Header("Patrolling Properties")]
    public Vector3 walkPoint;
    public float walkPointRange;
    private bool _isWalkPointSet;

    [Header("AI Properties")]
    public float maxHealth = 200f;
    public float health = 200f;
    public float timeBetweenAttack;
    private bool _alreadyAttacked;

    [Header("AI States")]
    public float sightRange;
    public float attackRange;
    public bool isInSight;
    public bool isInAttackRange;
    private bool _isDead = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        _weaponController = GetComponent<AIWeaponController>();
    }

    void Update()
    {
        if (_isDead) return;

        FindTarget();

        // Check for sight and attack range
        isInSight = Physics.CheckSphere(transform.position, sightRange, targetLayer);
        isInAttackRange = Physics.CheckSphere(transform.position, attackRange, targetLayer);

        if (!isInSight && !isInAttackRange) Wander();
        if (isInSight && !isInAttackRange) Chase();
        if (isInSight && isInAttackRange) Attack();

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    private void FindTarget()
    {
        target = null;

        // Look for potential targets within sight
        Collider[] targetsInRange = Physics.OverlapSphere(transform.position, sightRange, targetLayer);

        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider potentialTarget in targetsInRange)
        {
            // Skip if the potential target is this object
            if (potentialTarget.transform == transform) continue;

            // Calculate the distance to potential target 
            float distance = Vector3.Distance(transform.position, potentialTarget.transform.position);

            // Update target to the closest one
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = potentialTarget.transform;
            }
        }

        if (closestTarget != null)
        {
            target = closestTarget;
        }
    }

    private void Wander()
    {
        if (!_isWalkPointSet)
        {
            SearchWalkPoint();
        }

        if (_isWalkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        // Check if the agent has reached its destination
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Debug.Log($"{gameObject.name} reached walkpoint");
            _isWalkPointSet = false;
        }
    }
    private void SearchWalkPoint()
    {
        // Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // Check if the random point is in bound
        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer)) _isWalkPointSet = true;
    }

    private void Chase()
    {
        agent.SetDestination(target.transform.position);
    }

    private void Attack()
    {
        // Stop moving
        agent.SetDestination(transform.position);

        if (!_alreadyAttacked)
        {
            // Attack code
            if (_weaponController != null)
            {
                Staff currentWeapon = _weaponController.GetCurrentWeapon();
                currentWeapon.AIFire(target.transform);
            }

            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttack);
        }
    }
    private void ResetAttack()
    {
        _alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void TakeDamage(float damage)
    {
        if (_isDead) return;

        health -= damage;
        // Debug.Log($"{gameObject.name} has remaining {health} health");

        if (health <= 0) Die();
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died");

        if (agent != null && !_isDead)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        if (animator != null && !_isDead) animator.SetBool("isDead", true);

        _isDead = true;

        gameObject.layer = 0;
    }
}