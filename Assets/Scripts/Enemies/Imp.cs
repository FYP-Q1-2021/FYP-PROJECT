using UnityEngine;

public class Imp : Enemy
{
    protected override void Start()
    {
        base.Start();
        SetState(State.PATROL);
    }

    protected override void Update()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.red);

        switch (state)
        {
            case State.IDLE:
                {
                    if (IsPlayerVisible())
                    {
                        SetState(State.CHASE);
                        return;
                    }

                    if (idlingElapsedTime < idlingDuration)
                    {
                        idlingElapsedTime += idlingSpeed * Time.deltaTime;
                    }
                    // Change state to PATROL after some time passes
                    else
                    {
                        SetState(State.PATROL);
                        idlingElapsedTime = 0f;
                        return;
                    }
                }
                break;
            case State.PATROL:
                {
                    if (IsPlayerVisible())
                    {
                        SetState(State.CHASE);
                        return;
                    }

                    // Change state to IDLE when patrol end point is reached
                    if (waypointsManager.endPointReached)
                    {
                        SetState(State.IDLE);
                        return;
                    }
                }
                break;
            case State.CHASE:
                {
                    agent.destination = player.position;

                    if (Vector3.Distance(transform.position, player.position) < attackRange)
                    {
                        SetState(State.ATTACK);
                        return;
                    }
                }
                break;
            case State.ATTACK:
                {

                }
                break;
            case State.DEAD:
                {
                    Destroy(gameObject);
                }
                break;
        }
    }

    public override void SetState(int nextState)
    {
        state = nextState;

        switch (state)
        {
            case State.IDLE:
                agent.isStopped = true;
                waypointsManager.enabled = false;
                break;
            case State.PATROL:
                agent.isStopped = false;
                waypointsManager.enabled = true;
                waypointsManager.endPointReached = false;
                // Find nearest waypoint or reset target waypoint
                //waypointsManager.FindNearestWaypoint();
                break;
            case State.CHASE:
                //agent.isStopped = true;
                waypointsManager.enabled = false;
                break;
            case State.ATTACK:
                agent.isStopped = true;
                waypointsManager.enabled = false;
                break;
            case State.DEAD:
                agent.isStopped = true;
                waypointsManager.enabled = false;
                break;
        }
    }

    private bool IsPlayerVisible()
    {
        if (Vector3.Distance(transform.position, player.position) < visionRange)
        {
            Vector3 targetDir = player.position - transform.position;
            float angle = Vector3.Angle(targetDir, transform.forward);
            if (angle < viewingAngle)
                return true;
            else
                return false;
        }
        return false;
    }
}
