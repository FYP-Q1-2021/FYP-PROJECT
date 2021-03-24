using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Idle : State
{
    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _animator) : base(_npc, _agent, _animator)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        //anim.SetTrigger("isIdle");
        base.Enter();
    }

    public override void Update()
    {
        //stage = EVENT.EXIT;
        base.Update();
    }

    public override void Exit()
    {
        //animator.ResetTrigger("isIdle");
        base.Exit();
    }
}
