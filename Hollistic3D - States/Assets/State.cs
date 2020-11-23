using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE
    {
        IDLE, PATROL, PURSUE, ATTACK, FLEE, SLEEP
    };
    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };
    public STATE name;
    protected EVENT stage;
    protected GameObject npc;
    protected NavMeshAgent agent;
    protected Animator animator;
    protected Transform player;
    protected State nextState;

    float vistDist = 10.0f;
    float visAngle = 30.0f;
    float shootDist = 7.0f;

    public State(GameObject _npc, NavMeshAgent _agent, Animator _animator, Transform _player)
    {
        stage = EVENT.ENTER;
        npc = _npc;
        agent = _agent;
        animator = _animator;
        player = _player;
    }
    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    public State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }

    public bool CanSensePlayer()
    {
        Vector3 direction = npc.transform.position - player.position;
        float angle = Vector3.Angle(npc.transform.forward, direction);
        return (direction.magnitude < 2 && angle < 30);
    }
    public bool CanSeePlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        return (direction.magnitude < vistDist && angle < visAngle);
    }
    public bool CanAttackPlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        return (direction.magnitude < shootDist);
    }
}

public class Idle : State
{
    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _animator, Transform _player)
                : base(_npc, _agent, _animator, _player)
    {
        name = STATE.IDLE;
    }
    public override void Enter()
    {
        animator.SetTrigger("isIdle");
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, animator, player);
            stage = EVENT.EXIT;
        }
        else if (Random.Range(0, 100) < 10)
        {
            nextState = new Patrol(npc, agent, animator, player);
            stage = EVENT.EXIT;
        }

    }
    public override void Exit()
    {
        animator.ResetTrigger("isIdle");
        base.Exit();
    }
}

public class Patrol : State
{
    int currentIndex = -1;
    public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _animator, Transform _player)
                : base(_npc, _agent, _animator, _player)
    {
        name = STATE.PATROL;
        agent.speed = 2;
        agent.isStopped = false;
    }
    public override void Enter()
    {
        float lastDist = Mathf.Infinity;
        for (int i = 0; i < GameEnvironment.Singleton.Checkpoints.Count; i++)
        {
            GameObject thisWP = GameEnvironment.Singleton.Checkpoints[i];
            float distance = Vector3.Distance(npc.transform.position, thisWP.transform.position);
            if (distance < lastDist)
            {
                currentIndex = i - 1;
                lastDist = distance;
            }
        }
        animator.SetTrigger("isWalking");
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        if (agent.remainingDistance < 1)
        {
            if (currentIndex >= GameEnvironment.Singleton.Checkpoints.Count - 1)
                currentIndex = 0;
            else
                currentIndex++;

            Vector3 destination = GameEnvironment.Singleton.Checkpoints[currentIndex].transform.position;
            agent.SetDestination(destination);
        }
        if(CanSensePlayer()){
            nextState = new Flee(npc, agent, animator, player);
            stage = EVENT.EXIT;
        }
        else if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, animator, player);
            stage = EVENT.EXIT;
        }

    }
    public override void Exit()
    {
        animator.ResetTrigger("isWalking");
        base.Exit();
    }
}

public class Pursue : State
{
    public Pursue(GameObject _npc, NavMeshAgent _agent, Animator _animator, Transform _player)
                : base(_npc, _agent, _animator, _player)
    {
        name = STATE.PURSUE;
        agent.speed = 5;
        agent.isStopped = false;
    }
    public override void Enter()
    {
        animator.SetTrigger("isRunning");
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        agent.SetDestination(player.position);
        if (agent.hasPath)
        {
            if (CanAttackPlayer())
            {
                nextState = new Attack(npc, agent, animator, player);
                stage = EVENT.EXIT;
            }
            else if (!CanSeePlayer())
            {
                nextState = new Patrol(npc, agent, animator, player);
                stage = EVENT.EXIT;
            }
        }
    }
    public override void Exit()
    {
        animator.ResetTrigger("isRunning");
        base.Exit();
    }
}

public class Attack : State
{
    float rotationSpeed = 2.0f;
    AudioSource shoot;

    public Attack(GameObject _npc, NavMeshAgent _agent, Animator _animator, Transform _player)
                : base(_npc, _agent, _animator, _player)
    {
        name = STATE.ATTACK;
        shoot = _npc.GetComponent<AudioSource>();
    }
    public override void Enter()
    {
        animator.SetTrigger("isShooting");
        agent.isStopped = true;
        shoot.Play();
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        direction.y = 0;
        npc.transform.rotation =
            Quaternion.Slerp(
                npc.transform.rotation,
                Quaternion.LookRotation(direction),
                Time.deltaTime * rotationSpeed
            );
        if (!CanAttackPlayer())
        {
            nextState = new Idle(npc, agent, animator, player);
            stage = EVENT.EXIT;
        }
    }
    public override void Exit()
    {
        animator.ResetTrigger("isShooting");
        shoot.Stop();
        base.Exit();
    }
}

public class Flee : State
{
    int currentIndex = -1;
    Vector3 destination = new Vector3(0, 0, 0);
    public Flee(GameObject _npc, NavMeshAgent _agent, Animator _animator, Transform _player)
                : base(_npc, _agent, _animator, _player)
    {
        name = STATE.FLEE;
        agent.speed = 5;
        agent.isStopped = false;
    }
    public override void Enter()
    {
        animator.SetTrigger("isRunning");
        float lastDist = Mathf.Infinity;
        for (int i = 0; i < GameEnvironment.Singleton.Checkpoints.Count; i++)
        {
            GameObject thisWP = GameEnvironment.Singleton.Checkpoints[i];
            float distance = Vector3.Distance(npc.transform.position, thisWP.transform.position);
            if (distance < lastDist)
            {
                currentIndex = i - 1;
                lastDist = distance;
            }
        }
        if (currentIndex >= GameEnvironment.Singleton.SafeZones.Count - 1)
            currentIndex = 0;
        else
            currentIndex++;

        destination = GameEnvironment.Singleton.SafeZones[currentIndex].transform.position;
        agent.SetDestination(destination);
        base.Enter();
    }
    public override void Update()
    {
        base.Update();

        if (agent.hasPath && agent.remainingDistance < 1)
        {
            nextState = new Patrol(npc, agent, animator, player);
            stage = EVENT.EXIT;
        }
    }
    public override void Exit()
    {
        animator.ResetTrigger("isRunning");
        base.Exit();
    }
}
