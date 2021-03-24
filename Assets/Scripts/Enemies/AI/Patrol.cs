using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : State
{
    int currentWaypointIndex = -1;

    public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _animator) : base(_npc, _agent, _animator)
    {
        name = STATE.PATROL;
        agent.speed = 2;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        currentWaypointIndex = 0;
        //anim.SetTrigger("isIdle");
        base.Enter();
    }

    public override void Update()
    {
        if(agent.remainingDistance < 1f)
        {
            if (currentWaypointIndex >= WaypointsManager.Singleton.GetGoblinWaypoints().Count - 1)
                currentWaypointIndex = 0;
            else
                ++currentWaypointIndex;

            agent.SetDestination(WaypointsManager.Singleton.GetGoblinWaypoints()[currentWaypointIndex].transform.position);
        }
        base.Update();
    }

    public override void Exit()
    {
        //animator.ResetTrigger("isIdle");
        base.Exit();
    }
}
