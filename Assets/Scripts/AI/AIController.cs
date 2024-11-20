using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;
    public Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distance = Vector3.Distance(agent.destination, target.position);

        if (distance > agent.stoppingDistance)
        {
            agent.SetDestination(target.transform.position);
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}
