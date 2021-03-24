using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE
    {
        IDLE,
        PATROL,
        ATTACK,
        DEAD
    }

    public enum EVENT
    { 
        ENTER,
        UPDATE,
        EXIT
    }

    public STATE name;
    protected EVENT stage;
    protected GameObject npc;
    protected NavMeshAgent agent;
    protected Animator animator;
    protected State nextState;

    float visibilityRange = 10f;
    float visibilityAngle = 30f;

    public State(GameObject _npc, NavMeshAgent _agent, Animator _animator)
    {
        stage = EVENT.ENTER;
        npc = _npc;
        agent = _agent;
        animator = _animator;
    }

    public virtual void Enter()
    {
        stage = EVENT.UPDATE;
    }

    public virtual void Update()
    {
        stage = EVENT.UPDATE;
    }

    public virtual void Exit()
    {
        stage = EVENT.EXIT;
    }

    public State Process()
    {
        if (stage == EVENT.ENTER)
            Enter();

        if (stage == EVENT.UPDATE)
            Update();

        if(stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }

        return this;
    }
}
