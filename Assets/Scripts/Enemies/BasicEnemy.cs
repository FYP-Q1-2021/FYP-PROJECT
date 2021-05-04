using UnityEngine;

public class BasicEnemy : Enemy
{
    [Header("AI")]
    [Range(1, 50)]
    [SerializeField] protected float visionRange = 10f;
    [Range(1, 180)]
    [SerializeField] protected float viewingAngle = 90f;

    [Range(0, 10)]
    [SerializeField] protected float idlingDuration = 5f;
    [SerializeField] protected float idlingElapsedTime = 0f;

    [SerializeField] protected int attackDamage = 10;
    [Range(0, 50)]
    [SerializeField] protected float attackRange = 5f;
    [Range(0, 20)]
    [SerializeField] protected float attackSpeed = 1f;
    [SerializeField] protected float attackElapsedTime = 0f;
    [SerializeField] protected bool canAttack;

    [Range(1, 10)]
    [SerializeField] protected float stateChangeBufferDuration = 1f; // Buffer time to prevent immediate change of state when player is in-range / out-of-range eg. Chase <-> Attack
    [SerializeField] protected float stateChangeBufferElapsedTime = 0f;

    [Header("Debug Display")]
    public Color attackRangeColor = Color.red;
    public Color detectionRangeColor = Color.blue;

    protected bool IsPlayerVisible()
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
