using System.Collections;
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

    void Start()
    {
        currentHealth = maxHealth;

        // Checks whether this game object is a player or an enemy
        if(gameObject.GetComponent<PlayerCharacterController>())
        {
            isAPlayer = true;
            healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        }
        else
        {
            isAPlayer = false;
        }
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
        currentHealth -= damage;

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
}
