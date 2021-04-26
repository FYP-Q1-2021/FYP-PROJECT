﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth = 0;

    [SerializeField] private bool isAPlayer;
    private HealthBar healthBar;

    public event Action OnDamaged;
    public bool CanPickup;
    PlayerCharacterController playerCharacterController;

    void Start()
    {
        currentHealth = maxHealth;

        // Checks whether this game object is a player or an enemy
        if (gameObject.GetComponent<PlayerCharacterController>())
        {
            isAPlayer = true;
            healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
            playerCharacterController = GetComponent<PlayerCharacterController>();

        }
        else
        {
            isAPlayer = false;
        }
    }

    void Update()
    {
        CanPickup = currentHealth < maxHealth ? true : false;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void Damage(float damage)
    {
        DealDamage(damage);

        OnDamaged?.Invoke();

        if(isAPlayer)
        {
            healthBar.UpdateHealthBar();
        }

        if(currentHealth < 1f)
        {
            // End game
            if(isAPlayer)
            {

            }
            // Delete enemy
            else
            {
                DeadEnemyManager.Instance.AddToDeadEnemiesList(gameObject);
                GetComponent<Enemy>().SetState(State.DEAD);
            }
        }
    }

    public void Heal(float heal)
    {
        currentHealth += heal;
    }

    void DealDamage(float damage)
    {
        if (isAPlayer && !playerCharacterController.isInvincible)
            currentHealth -= damage;
        else if (!isAPlayer)
            currentHealth -= damage;
    }

    public void DamageOverTime(float damageAmount, float duration, float delayBetweenDamage)
    {
        StartCoroutine(DamageOverTimeCoroutine(damageAmount, duration, delayBetweenDamage));
    }

    IEnumerator DamageOverTimeCoroutine(float damageAmount, float duration, float delayBetweenDamage)
    {
        while(duration > 0)
        {
            DealDamage(damageAmount);
            duration -= delayBetweenDamage;
            yield return new WaitForSeconds(delayBetweenDamage);

        }
    }
}
