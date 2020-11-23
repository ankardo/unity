using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    public Transform player;
    private State currentState;

    private void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        currentState = new Idle(this.gameObject, agent, animator, player);
    }
    private void Update()
    {
        currentState = currentState.Process();
    }
}
