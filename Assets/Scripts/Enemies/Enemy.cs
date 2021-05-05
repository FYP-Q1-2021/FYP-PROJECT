using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Player attributes")]
    [SerializeField] protected Transform player;
    [SerializeField] protected Health playerHP;

    [Header("Animation")]
    public Animator animator;

    [Header("State")]
    [SerializeField] protected State state;
    protected State previousState;

    protected float simulationSpeed = 1f;

    #region Unity Callbacks
    protected virtual void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        player = p.GetComponent<Transform>();
        playerHP = p.GetComponent<Health>();
    }

    protected virtual void Update()
    {

    }

    protected void OnDestroy()
    {
        if(EnemyManager.Instance)
            EnemyManager.Instance.RemoveFromList(gameObject);
    }

    protected virtual void OnDrawGizmosSelected()
    {

    }
    #endregion

    public virtual void SetState(State nextState)
    {

    }

    public State GetCurrentState()
    {
        return state;
    }
}
