using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public interface IState
{
    void Enter();
    void Execute();
    void Exit();
}

public class StateMachine
{
    IState currentState;

    public void ChangeState(IState newState)
    {
        if (currentState != null)
            currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
    public void Update()
    {
        if (currentState != null) currentState.Execute();
    }
}

public class State_Patrol : IState
{
    TankController owner;
    NavMeshAgent agent;
    public State_Patrol(TankController owner) { this.owner = owner; }
    public void Enter()
    {
        //Patrol state moves towards player until in range
        Debug.Log("entering patrol state");
        agent = owner.GetComponent<NavMeshAgent>();
        agent.destination = owner.target.gameObject.transform.position;
        // start moving, in case we were previously stopped
        agent.isStopped = false;
    }
    public void Execute()
    {
        Debug.Log("updating patrol state");

        if (owner.seenTarget)
        {
            owner.stateMachine.ChangeState(new State_Attack(owner));
        }
    }
    public void Exit()
    {
        Debug.Log("exiting patrol state");
        // stop moving
        agent.isStopped = true;
    }
}

public class State_Attack : IState
{
    TankController owner;
    NavMeshAgent agent;
    public State_Attack(TankController owner) { this.owner = owner; }
    public void Enter()
    {
        //Attack state will stop tank from moving and will start firing towards player
        Debug.Log("entering attack state");
        agent = owner.GetComponent<NavMeshAgent>();
        if (owner.seenTarget)
        {
            agent.destination = owner.transform.position;
        }
    }
    public void Execute()
    {
        Debug.Log("updating attack state");
        if (owner.seenTarget != true)
        {
            Debug.Log("lost sight");
            owner.stateMachine.ChangeState(new State_Patrol(owner));
        }
    }
    public void Exit()
    {
        Debug.Log("exiting attack state");
        agent.isStopped = false;
    }
}