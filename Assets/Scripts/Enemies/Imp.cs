using UnityEngine;

public class Imp : Enemy
{
    public float movementSpeed = 3.5f;
    public float rotationSpeed = 4f;

    private FlyingWaypointsManager waypointsManager;

    protected override void Start()
    {
        base.Start();
        waypointsManager = GetComponent<FlyingWaypointsManager>();
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
                        SetState(State.ATTACK);
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
            case State.ATTACK:
                {
                    float distanceFromPlayer = Vector3.Distance(transform.position, player.position);

                    if (IsPlayerOutOfRange(distanceFromPlayer, attackRange))
                    {
                        SetState(State.PATROL);
                        return;
                    }

                    if (canAttack)
                    {
                        playerHP.Damage(attackDamage);
                        canAttack = false;
                    }
                    else
                    {
                        attackElapsedTime += simulationSpeed * Time.deltaTime;
                        if (attackElapsedTime > attackSpeed)
                        {
                            canAttack = true;
                            attackElapsedTime = 0f;
                        }
                    }

                    OrientTowards();
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
                waypointsManager.enabled = false;
                animator.SetInteger("State", State.IDLE);
                break;
            case State.PATROL:
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

    private void OrientTowards()
    {
        float distanceToTurn = rotationSpeed * Time.deltaTime;
        Vector3 targetDir = player.position - transform.position;
        Quaternion rotationToTarget = Quaternion.LookRotation(targetDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, distanceToTurn);
    }

    private void MoveTowards()
    {
        float step = movementSpeed * Time.deltaTime; // distance to move
        transform.position = Vector3.MoveTowards(transform.position, player.position, step);
    }

    private void Shoot()
    {

    }
}
