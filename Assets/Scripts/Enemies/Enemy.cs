using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    [Header("Player attributes")]
    [SerializeField] protected Transform player;
    [Header("AI")]
    [SerializeField] protected int state;
    [TextArea(1, 6)]
    [SerializeField] private string stateName;
    [SerializeField] protected WaypointsManager waypointsManager;
    [Range(1, 20)]
    [SerializeField] protected float visionRange = 10f;
    [Range(1, 180)]
    [SerializeField] protected float viewingAngle = 90f;
    [Range(1, 10)]
    [SerializeField] protected float idlingDuration = 5f;
    [SerializeField] protected float idlingElapsedTime = 0f;
    [SerializeField] protected float idlingSpeed = 1f;
    [Range(0, 20)]
    [SerializeField] protected float attackRange = 5f;
    protected NavMeshAgent agent;

    protected virtual void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
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
}
