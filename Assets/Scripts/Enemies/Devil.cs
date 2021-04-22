using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil : MonoBehaviour
{
    enum States
    {
        IDLE,
        MELEE_MODE,
        RANGED_MODE,
        ENRAGED
    }

    [Header("Player attributes")]
    [SerializeField] private Transform player;
    [SerializeField] protected Health playerHP;

    [Header("Spells")]
    [SerializeField] private Ripple ripple;
    [SerializeField] private GameObject geyserPrefab;

    [Header("Spawnables")]
    [SerializeField] private GameObject crystal;

    [Header("Skills Cooldown")]
    [SerializeField] private float rippleCooldown = 4f;
    private float rippleCooldownTimer = 0f;
    private bool canUseRipple = true;

    [SerializeField] private float eruptionCooldown = 1f;
    private float eruptionCooldownTimer = 0f;
    private bool canUseEruption = true;

    [SerializeField] private float staffCooldown = 3f;
    private float staffCooldownTimer = 0f;
    private bool canUseStaff = true;

    private int timesStaffIsUsed = 0;

    private Staff staff;

    [Header("Debug Display")]
    [SerializeField] private Color rangedAttackColor = Color.blue;
    [SerializeField] private Color meleeRangeColor = Color.red;
    [SerializeField] private float rangedAttackRange = 50f;
    [SerializeField] private float meleeAttackRange = 10f;

    private States state;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        player = p.GetComponent<Transform>();
        playerHP = p.GetComponent<Health>();

        staff = GetComponentInChildren<Staff>();

        state = States.IDLE;
    }

    void Update()
    {
        CheckPlayerPosition();

        switch (state)
        {
            case States.IDLE:

                break;
            case States.MELEE_MODE:
                {
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
                }
                break;
            case States.RANGED_MODE:
                {
                    if (canUseStaff)
                    {
                        staff.Attack();
                        canUseStaff = false;

                        ++timesStaffIsUsed;
                        if (timesStaffIsUsed % 2 == 0)
                        {
                            StartCoroutine("GeyserAttack");
                        }
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
                }
                break;
            case States.ENRAGED:

                break;
        }

    }

    IEnumerator GeyserAttack()
    {
        while (eruptionCooldownTimer < eruptionCooldown)
        {
            eruptionCooldownTimer += Time.deltaTime;
            yield return null;
        }

        Instantiate(geyserPrefab, player.position, geyserPrefab.transform.rotation);
    }

    public void SetState(int nextState)
    {

    }

    private void CheckPlayerPosition()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance > meleeAttackRange)
        {
            state = States.RANGED_MODE;
        }
        else
        {
            state = States.MELEE_MODE;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = rangedAttackColor;
        Gizmos.DrawWireSphere(transform.position, rangedAttackRange);

        Gizmos.color = meleeRangeColor;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);
    }
}
