using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    [Header("Player attributes")]
    [SerializeField] protected Transform player;
    [SerializeField] protected Health playerHP;

    [Header("AI")]
    [SerializeField] protected int state;
    protected int previousState;
    [TextArea(1, 6)]
    [SerializeField] private string stateNames;
    [SerializeField] protected WaypointsManager waypointsManager;

    [Range(1, 20)]
    [SerializeField] protected float visionRange = 10f;
    [Range(1, 180)]
    [SerializeField] protected float viewingAngle = 90f;

    [Range(1, 10)]
    [SerializeField] protected float idlingDuration = 5f;
    [SerializeField] protected float idlingElapsedTime = 0f;

    [SerializeField] protected int attackDamage = 10;
    [Range(0, 20)]
    [SerializeField] protected float attackRange = 5f;
    [Range(0, 20)]
    [SerializeField] protected float attackSpeed = 1f;
    [SerializeField] protected float attackElapsedTime = 0f;
    [SerializeField] protected bool canAttack;

    [Range(1, 10)]
    [SerializeField] protected float stateChangeBufferDuration = 1f; // Buffer time to prevent immediate change of state when player is in-range / out-of-range eg. Chase <-> Attack
    [SerializeField] protected float stateChangeBufferElapsedTime = 0f;

    protected float simulationSpeed = 1f;
    protected NavMeshAgent agent;

    protected virtual void Start()
    {
        GameObject p = GameObject.Find("Player");
        player = p.GetComponent<Transform>();
        playerHP = p.GetComponent<Health>();

        agent = GetComponent<NavMeshAgent>();
        waypointsManager = GetComponent<WaypointsManager>();
    }

    protected abstract void Update();

    protected void OnDestroy()
    {
        EnemyManager.Instance.RemoveFromList(gameObject);
    }

    public abstract void SetState(int nextState);

    public int GetCurrentState()
    {
        return state;
    }

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
