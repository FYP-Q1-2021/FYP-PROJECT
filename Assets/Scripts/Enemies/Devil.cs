using System.Collections;
using UnityEngine;

public class Devil : Enemy
{
    [Header("Attack ranges")]
    [SerializeField] private float rangedAttackRange = 50f;
    [SerializeField] private float meleeAttackRange = 10f;

    [Header("Eruption spell")]
    [SerializeField] private float rippleCooldown = 4f;
    private float rippleCooldownTimer = 0f;
    private bool canUseRipple = true;

    [Header("Eruption spell")]
    [SerializeField] private float eruptionCooldown = 1f;

    [Header("Basic attack spell")]
    [SerializeField] private float staffCooldown = 3f;
    private float staffCooldownTimer = 0f;
    private bool canUseStaff = false;
    private int timesStaffIsUsed = 0;

    [Header("Enraged state")]
    [SerializeField] private int hitsToEnterEnragedMode = 3;
    [SerializeField] private float enragedModeBufferTime = 10f; // Time taken before entering enraged mode
    private float enragedModeBufferTimer = 0f;
    private bool isEnragedTimerCoroutineRunning;
    private bool isMultipleEruptionsCoroutineRunning;
    private int numOfTimesDamaged;

    [Header("Cluster eruption spell")]
    [SerializeField] private int multipleEruptionCount = 10;
    [SerializeField] private float multipleEruptionCooldown = 0.2f;
    private float multipleEruptionCooldownTimer = 0f;
    private readonly int tryRandomPositionLimit = 10;
    private int currentTries = 0;
    private LayerMask geyserLayerMask;

    [Header("Spells positioning")]
    [SerializeField] Transform eruptionYCoord;

    [Header("Phase 2")]
    [SerializeField] private float hPToPhase2 = 750f;

    [Header("Debug Display")]
    [SerializeField] private Color rangedAttackColor = Color.blue;
    [SerializeField] private Color meleeRangeColor = Color.red;

    private Staff staff;
    private Ripple ripple;

    private Health health;

    #region Inherited functions
    protected override void Start()
    {
        base.Start();

        staff = GetComponentInChildren<Staff>();

        ripple = GetComponentInChildren<Ripple>();
        ripple.gameObject.SetActive(false);

        health = GetComponent<Health>();
        health.OnDamaged += OnDamagedEvent;

        geyserLayerMask = LayerMask.GetMask("Spell");

        state = State.IDLE;
    }

    protected override void Update()
    {
        switch (state)
        {
            case State.IDLE:
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
                if (!isMultipleEruptionsCoroutineRunning)
                    StartCoroutine("MultipleRandomGeyserAttack");
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
    IEnumerator EnragedTimer()
    {
        isEnragedTimerCoroutineRunning = true;

        while (enragedModeBufferTimer < enragedModeBufferTime)
        {
            enragedModeBufferTimer += simulationSpeed * Time.deltaTime;

            if (numOfTimesDamaged == hitsToEnterEnragedMode)
            {
                SetState(State.ENRAGED);
                enragedModeBufferTimer = 0f;
                break;
            }

            yield return null;
        }

        numOfTimesDamaged = 0;
        isEnragedTimerCoroutineRunning = false;
    }

    IEnumerator GeyserAttack()
    {
        yield return new WaitForSeconds(eruptionCooldown);

        GameObject geyser = GeyserPool.Instance.GetPooledObject();
        geyser.transform.position = new Vector3(player.position.x, eruptionYCoord.position.y, player.position.z);
    }

    IEnumerator MultipleRandomGeyserAttack()
    {
        isMultipleEruptionsCoroutineRunning = true;

        for (int i = 0; i < multipleEruptionCount; ++i)
        {
            // Spawn a geyser once in a while
            while (multipleEruptionCooldownTimer < multipleEruptionCooldown)
            {
                multipleEruptionCooldownTimer += simulationSpeed * Time.deltaTime;
                yield return null;
            }

            float x = Random.Range(-rangedAttackRange, rangedAttackRange);
            float z = Random.Range(-rangedAttackRange, rangedAttackRange);

            // Check the location of where the spell would be cast
            // Find another position if there is already a spell near it
            Collider[] hitColliders = Physics.OverlapSphere(new Vector3(x, eruptionYCoord.position.y, z), 20f, geyserLayerMask);

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
                geyser.transform.position = new Vector3(x, geyser.transform.position.y, z);

            // Reset timer only if a geyser is spawned successfully
            multipleEruptionCooldownTimer = 0f;
        }

        SetState(State.IDLE);
        isMultipleEruptionsCoroutineRunning = false;
    }
    #endregion

    #region Unity Callbacks
    void OnDisable()
    {
        health.OnDamaged -= OnDamagedEvent;
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
        for (int i = 0; i < ImpPool.Instance.amountToPool; ++i)
        {
            GameObject imp = ImpPool.Instance.GetPooledObject();
            
        }
    }

    #endregion

    #region Events
    private void OnDamagedEvent()
    {
        if (health.GetCurrentHealth() < hPToPhase2 + 1)
        {

        }

        ++numOfTimesDamaged;
        if (!isEnragedTimerCoroutineRunning)
            StartCoroutine("EnragedTimer");
    }
    #endregion
}