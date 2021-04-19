using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Parameters")]
    [Tooltip("Amount of health to heal on pickup")]
    public float HealAmount = 40f;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Health playerHealth = GameObject.Find("Player").GetComponent<Health>();
            if (playerHealth && playerHealth.CanPickup)
            {
                playerHealth.Heal(HealAmount);
                Destroy(gameObject);
            }
        }

    }
}
