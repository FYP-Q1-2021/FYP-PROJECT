using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth = 0;

    [SerializeField] private bool isAPlayer;
    private HealthBar healthBar;

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

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;

        if(isAPlayer)
        {
            healthBar.UpdateHealthBar();
        }

        if(currentHealth < 1)
        {
            // End game
            if(isAPlayer)
            {

            }
            // Delete enemy
            else
            {
                GetComponent<Enemy>().SetState(State.DEAD);
            }
        }
    }

    public void Heal(int heal)
    {
        currentHealth += heal;
    }
}
