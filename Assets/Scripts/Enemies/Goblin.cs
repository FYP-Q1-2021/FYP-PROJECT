using UnityEngine;

public class Goblin : Enemy
{
    protected override void Start()
    {
        base.Start();
        SetState(State.PATROL);
    }

    protected override void Update()
    {
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
                        idlingElapsedTime += simulationSpeed * Time.deltaTime;
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
                    float distanceFromPlayer = Vector3.Distance(transform.position, player.position);

                    if (IsPlayerOutOfRange(distanceFromPlayer, visionRange))
                    {
                        SetState(State.PATROL);
                        return;
                    }

                    // Player is within attacking range
                    if (distanceFromPlayer < attackRange)
                    {
                        SetState(State.ATTACK);
                        return;
                    }

                    agent.destination = player.position;
                }
                break;
            case State.ATTACK:
                {
                    float distanceFromPlayer = Vector3.Distance(transform.position, player.position);

                    if (IsPlayerOutOfRange(distanceFromPlayer, attackRange))
                    {
                        SetState(State.CHASE);
                        return;
                    }

                    if(canAttack)
                    {
                        playerHP.Damage(attackDamage);
                        canAttack = false;
                    }
                    else
                    {
                        attackElapsedTime += simulationSpeed * Time.deltaTime;
                        if(attackElapsedTime > attackSpeed)
                        {
                            canAttack = true;
                            attackElapsedTime = 0f;
                        }
                    }

                    agent.destination = player.position;
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
                animator.SetInteger("State", State.IDLE);
                break;
            case State.PATROL:
                agent.isStopped = false;
                waypointsManager.endPointReached = false;
                waypointsManager.enabled = true;
                animator.SetInteger("State", State.PATROL);
                break;
            case State.ATTACK:
                canAttack = true;
                stateChangeBufferElapsedTime = 0f;
                waypointsManager.enabled = false;
                animator.SetInteger("State", State.ATTACK);
                break;
            case State.DEAD:
                agent.isStopped = true;
                waypointsManager.enabled = false;
                animator.SetInteger("State", State.DEAD);
                break;
        }
    }

    private bool IsPlayerOutOfRange(float distance, float range)
    {
        stateChangeBufferElapsedTime += simulationSpeed * Time.deltaTime;

        if (distance > range && stateChangeBufferElapsedTime > stateChangeBufferDuration)
            return true;

        return false;
    }
}
