using System.Collections;
using UnityEngine;
using System;

public class Devil : Enemy
{
    enum Phase
    { 
        PHASE_1,
        PHASE_2,
        PHASE_3
    }

    [Header("Attack ranges")]
    [SerializeField] private float rangedAttackRange = 50f;
    [SerializeField] private float meleeAttackRange = 10f;

    [Header("Starting Phase")]
    [SerializeField] private float invincibilityDuration = 5f;
    private bool isInvincibleCoroutineRunning;
    [SerializeField] private GameObject canvas;

    [Header("Phase 1")]
    [SerializeField] private float rippleCooldown = 4f;
    private float rippleCooldownTimer = 0f;
    private bool canUseRipple = true;

    [SerializeField] private float eruptionCooldown = 1f;

    [SerializeField] private float staffCooldown = 3f;
    private float staffCooldownTimer = 0f;
    private bool canUseStaff = false;
    private int timesStaffIsUsed = 0;

    [Header("Phase 2")]
    [SerializeField] private float hPToPhase2 = 750f;

    [Header("Phase 3")]
    [SerializeField] private float timeBetweenClusterEruptions = 5f;
    [SerializeField] private int eruptionCount = 10;
    [SerializeField] private float timeBetweenEachEruption = 0.2f;
    private float timeBetweenEachEruptionTimer = 0f;
    private readonly int tryRandomPositionLimit = 10;
    private int currentTries = 0;
    private bool isMultipleEruptionsCoroutineRunning;
    private LayerMask geyserLayerMask;

    [Header("Debug Display")]
    [SerializeField] private Color rangedAttackColor = Color.blue;
    [SerializeField] private Color meleeRangeColor = Color.red;

    [Header("Spells positioning")]
    [SerializeField] private Transform eruptionCoord;

    private Staff staff;
    private Ripple ripple;

    private Health health;
    private DevilTransitionManager transitionManager;

    private Phase currentPhase;

    private float eruptionRange;

    public event Action OnDeath;

    void Awake()
    {
        staff = GetComponentInChildren<Staff>();
        ripple = GetComponentInChildren<Ripple>();
        health = GetComponent<Health>();
        transitionManager = GameObject.Find("DevilPhaseManager").GetComponent<DevilTransitionManager>();
        geyserLayerMask = LayerMask.GetMask("Spell");
    }

    #region Inherited functions
    protected override void Start()
    {
        base.Start();

        gameObject.SetActive(false);

        ripple.gameObject.SetActive(false);

        health.OnDamaged += OnDamagedEvent;
        health.isInvincible = true;

        canvas.SetActive(false);

        transitionManager.OnTransitionToPhase3 += OnTransitionToPhase3Event;

        // To be removed when events is added to eruption
        timeBetweenClusterEruptions += 6f;

        currentPhase = Phase.PHASE_1;

        eruptionRange = rangedAttackRange * 0.75f;

        state = State.IDLE;
    }

    protected override void Update()
    {
        switch (state)
        {
            case State.IDLE:
                if (health.isInvincible)
                {
                    if(!isInvincibleCoroutineRunning)
                        StartCoroutine("InvulnerableState");
                    return;
                }
                else
                    CheckPlayerPosition();
                break;
            case State.MELEE_MODE:
                CheckPlayerPosition();

                if (canUseRipple)
                {
                    ripple.Attack();
                    canUseRipple = false;
                }
                else
                {
                    if (ripple.expanding)
                        break;

                    rippleCooldownTimer += Time.deltaTime;
                    if (rippleCooldownTimer > rippleCooldown)
                    {
                        canUseRipple = true;
                        rippleCooldownTimer = 0f;
                    }
                }
                break;
            case State.RANGED_MODE:
                CheckPlayerPosition();

                if (canUseStaff)
                {
                    staff.Attack();
                    canUseStaff = false;

                    ++timesStaffIsUsed;
                    if (timesStaffIsUsed % 2 == 0)
                        StartCoroutine("GeyserAttack");
                }
                else
                {
                    staffCooldownTimer += Time.deltaTime;
                    if (staffCooldownTimer > staffCooldown)
                    {
                        canUseStaff = true;
                        staffCooldownTimer = 0f;
                    }
                }
                break;
            case State.ENRAGED:
                {
                    if (!isMultipleEruptionsCoroutineRunning)
                        StartCoroutine("MultipleRandomGeyserAttack");
                }
                break;
            case State.DEAD:
                {
                    Destroy(gameObject);
                }
                break;
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        Gizmos.color = rangedAttackColor;
        Gizmos.DrawWireSphere(transform.position, rangedAttackRange);

        Gizmos.color = meleeRangeColor;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);
    }

    public override void SetState(State nextState)
    {
        state = nextState;
        timesStaffIsUsed = 0;
    }
    #endregion

    #region Coroutines
    IEnumerator InvulnerableState()
    {
        isInvincibleCoroutineRunning = true;
        yield return new WaitForSeconds(invincibilityDuration);
        health.isInvincible = false;
        canvas.SetActive(true);
        isInvincibleCoroutineRunning = false;
    }

    IEnumerator GeyserAttack()
    {
        yield return new WaitForSeconds(eruptionCooldown);

        GameObject geyser = GeyserPool.Instance.GetPooledObject();
        geyser.transform.position = new Vector3(player.position.x, eruptionCoord.position.y, player.position.z);
    }

    IEnumerator MultipleRandomGeyserAttack()
    {
        isMultipleEruptionsCoroutineRunning = true;

        for (int i = 0; i < eruptionCount; ++i)
        {
            // Spawn a geyser once in a while
            while (timeBetweenEachEruptionTimer < timeBetweenEachEruption)
            {
                timeBetweenEachEruptionTimer += simulationSpeed * Time.deltaTime;
                yield return null;
            }

            float x = eruptionCoord.position.x + UnityEngine.Random.Range(-eruptionRange, eruptionRange);
            float z = eruptionCoord.position.z + UnityEngine.Random.Range(-eruptionRange, eruptionRange);

            // Check the location of where the spell would be cast
            // Find another position if there is already a spell near it
            Collider[] hitColliders = Physics.OverlapSphere(new Vector3(x, eruptionCoord.position.y, z), 10f, geyserLayerMask);

            if (hitColliders.Length > 0)
            {
                // Prevent infinite loop
                ++currentTries;
                if (currentTries > tryRandomPositionLimit)
                {
                    currentTries = 0;
                    continue;
                }
                --i;
                continue;
            }

            GameObject geyser = GeyserPool.Instance.GetPooledObject();
            if (geyser)
                geyser.transform.position = new Vector3(x, eruptionCoord.position.y, z);

            // Reset timer only if a geyser is spawned successfully
            timeBetweenEachEruptionTimer = 0f;
        }

        yield return new WaitForSeconds(timeBetweenClusterEruptions);

        isMultipleEruptionsCoroutineRunning = false;
    }
    #endregion

    #region Unity Callbacks
    void OnDisable()
    {
        if (currentPhase == Phase.PHASE_3)
        {
            health.OnDamaged -= OnDamagedEvent;
            transitionManager.OnTransitionToPhase3 -= OnTransitionToPhase3Event;
        }
    }
    #endregion

    #region Private functions
    private void CheckPlayerPosition()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance > meleeAttackRange)
            state = State.RANGED_MODE;
        else
            state = State.MELEE_MODE;
    }

    private void SpawnImps()
    {
        int impsCount = ImpPool.Instance.amountToPool;
        float angle = 360 / impsCount;
        for (int i = 0; i < impsCount; ++i)
        {
            GameObject imp = ImpPool.Instance.GetPooledObject();
            Vector3 impDirection = Quaternion.AngleAxis(i * angle, transform.up) * transform.forward;
            imp.GetComponent<Imp>().SetSpawnDirection(impDirection);
            imp.transform.position = transform.position;
        }
    }
    #endregion

    #region Events
    private void OnDamagedEvent()
    {
        if (currentPhase == Phase.PHASE_1)
        {
            // Summon imps
            if (health.GetCurrentHealth() < hPToPhase2 + 1)
            {
                currentPhase = Phase.PHASE_2;
                SpawnImps();
                Destroy(ripple.gameObject);
                gameObject.SetActive(false);
            }
        }

        if (health.GetCurrentHealth() < 1)
        {
            SetState(State.DEAD);
            OnDeath?.Invoke();
        }
    }

    private void OnTransitionToPhase3Event()
    {
        gameObject.SetActive(true);
        currentPhase = Phase.PHASE_3;
        SetState(State.ENRAGED);
    }
    #endregion
}