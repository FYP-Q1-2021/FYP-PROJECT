using UnityEngine;
using UnityEngine.AI;
using System;

public class Goblin : BasicEnemy
{
    private NavMeshAgent agent;
    private WaypointsManager waypointsManager;
    private GoblinAnimationEvents goblinAnimationEvents;

    private Health health;

    public event Action OnDeath;

    void Awake()
    {
        health = GetComponent<Health>();
    }

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        waypointsManager = GetComponent<WaypointsManager>();
        SetState(State.PATROL);

        health = GetComponent<Health>();
        health.OnDamaged += OnDamagedEvent;

        goblinAnimationEvents = GetComponentInChildren<GoblinAnimationEvents>();
        if (goblinAnimationEvents) // Might be null due to Pirate Goblin currently not having any attack animation
            goblinAnimationEvents.OnLastAttackFrame += OnLastAttackAnimationFrameEvent;
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

                    if (canAttack)
                    {
                        animator.SetInteger("State", (int)State.ATTACK);

                        if (!goblinAnimationEvents) // Quick hack since pirate doesn't have attack animation yet
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

    public override void SetState(State nextState)
    {
        state = nextState;

        switch (state)
        {
            case State.IDLE:
                agent.isStopped = true;
                waypointsManager.enabled = false;
                animator.SetInteger("State", (int)State.IDLE);
                break;
            case State.PATROL:
                agent.isStopped = false;
                waypointsManager.endPointReached = false;
                waypointsManager.enabled = true;
                animator.SetInteger("State", (int)State.PATROL);
                break;
            case State.ATTACK:
                canAttack = true;
                stateChangeBufferElapsedTime = 0f;
                waypointsManager.enabled = false;
                break;
            case State.DEAD:
                agent.isStopped = true;
                waypointsManager.enabled = false;
                animator.SetInteger("State", (int)State.DEAD);
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

    protected override void OnDrawGizmosSelected()
    {
        Gizmos.color = detectionRangeColor;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        Gizmos.color = attackRangeColor;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void OnEnable()
    {
        health.OnDamaged += OnDamagedEvent;
    }

    void OnDisable()
    {
        health.OnDamaged -= OnDamagedEvent;

        if (goblinAnimationEvents)
            goblinAnimationEvents.OnLastAttackFrame -= OnLastAttackAnimationFrameEvent;
    }

    #region Events
    private void OnDamagedEvent()
    {
        if(health.GetCurrentHealth() < 1f)
        {
            SetState(State.DEAD);
            OnDeath?.Invoke();
        }
    }

    private void OnLastAttackAnimationFrameEvent()
    {
        playerHP.Damage(attackDamage);
    }
    #endregion
}
