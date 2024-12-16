using System.Collections;
using System.Collections.Generic;
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
    private Rigidbody _agentRb;

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
    public float jumpHeight = 1.5f;
    private bool _isDead = false;
    public float jumpInterval = 30f;
    private float _nextJumpTime;
    private bool _isJumping;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        _weaponController = GetComponent<AIWeaponController>();
        _agentRb = GetComponent<Rigidbody>();

        health = maxHealth;
        agent.stoppingDistance = attackRange;
        _nextJumpTime = Time.time + Random.Range(jumpInterval - 3f, jumpInterval + 3f);
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

        // Handle Jumping
        // if (!_isJumping && Time.time > _nextJumpTime)
        // {
        //     Jump(jumpHeight);
        // }

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
            // Debug.Log($"{gameObject.name} reached walkpoint");
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
            animator.SetTrigger("Attack");
            Invoke(nameof(ResetAttack), timeBetweenAttack);
        }
    }
    private void ResetAttack()
    {
        _alreadyAttacked = false;
    }

    public void Jump(float force)
    {
        if (_isJumping) return;

        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.isStopped = true;

        _isJumping = true;

        // Apply upward force
        _agentRb.AddRelativeForce(Vector3.up * force, ForceMode.Impulse);
        _agentRb.velocity = Vector3.up * 4;

        _nextJumpTime = Time.time + Random.Range(jumpInterval - 3f, jumpInterval + 3f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (_isJumping)
            {
                _isJumping = false;
                _agentRb.velocity = Vector3.zero;

                agent.updateRotation = true;
                agent.updatePosition = true;
                agent.isStopped = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void TakeDamage(float damage, string damageType, string killer)
    {
        if (_isDead) return;

        health -= damage;
        // Debug.Log($"{gameObject.name} has remaining {health} health");

        if (health <= 0)
        {
            Die(damageType, killer);
        }
    }

    private void Die(string damageType, string killer)
    {
        // Debug.Log($"{gameObject.name} died");
        KillFeedManager.Instance.AddNewMessage(killer, damageType, gameObject.name);
        GameManager.Instance.UpdateScore(gameObject.CompareTag("Enemy"));

        if (agent != null && !_isDead)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        if (animator != null && !_isDead) animator.SetBool("isDead", true);

        _isDead = true;
        gameObject.layer = 0;

        Destroy(gameObject, 5f);
    }
}