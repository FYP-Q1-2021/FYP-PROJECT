using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil : MonoBehaviour
{
    enum States
    {
        IDLE,
        THREE_WAY_ATTACK,
        EXPANDING_RING_ATTACK,
        FIRE_GEYSER_ATTACK,
        ENRAGED
    }

    [Header("Player attributes")]
    [SerializeField] protected Transform player;
    [SerializeField] protected Health playerHP;

    [Header("Spawnables")]
    [SerializeField] private ExpandingRing expandingRing;
    [SerializeField] private GameObject geyserCircle;
    [SerializeField] private GameObject bullet;

    [Header("Skills Cooldown")]
    [SerializeField] private float ringCooldown = 4f;
    private float ringCooldownTimer = 0f;
    [SerializeField] private float geyserCooldown = 1f;
    [SerializeField] private float shootCooldown = 3f;


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

        state = States.IDLE;
    }

    void Update()
    {
        CheckPlayerPosition();

        switch (state)
        {
            case States.IDLE:

                break;
            case States.THREE_WAY_ATTACK:

                break;
            case States.EXPANDING_RING_ATTACK:
                expandingRing.ExpandingRingAttack();
                state = States.IDLE;
                break;
            case States.FIRE_GEYSER_ATTACK:

                break;
            case States.ENRAGED:

                break;
        }

    }

    public void SetState(int nextState)
    {

    }

    private void CheckPlayerPosition()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if(distance > meleeAttackRange)
        {
            //state = States.FIRE_GEYSER_ATTACK;
            state = States.EXPANDING_RING_ATTACK;
        }
        else
        {
            state = States.THREE_WAY_ATTACK;
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
