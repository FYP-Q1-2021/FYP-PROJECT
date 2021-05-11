using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Parameters")]
    [Tooltip("Amount of health to heal on pickup")]
    public float HealAmount = 40f;
    [Tooltip("Effect played when picked up")]
    public GameObject pickupVFX;
    [Tooltip("Eound played when picked up")]
    public AudioClip pickupSFX;
    public float pickupVFXDuration = 5f;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Health playerHealth = GameObject.Find("Player").GetComponent<Health>();
            if (playerHealth && playerHealth.CanPickup)
            {
                if (pickupSFX)
                {
                    AudioUtility.CreateSFX(pickupSFX, transform.position, AudioUtility.AudioGroups.Pickup, 0f);
                }

                Debug.Log("Collide");

                if(pickupVFX)
                {
                    GameObject pickupVFXInstance = Instantiate(pickupVFX, gameObject.transform.position, pickupVFX.transform.rotation);
                    pickupVFXInstance.transform.SetParent(null);
                    pickupVFXInstance.SetActive(true);
                    Destroy(pickupVFXInstance, pickupVFXDuration);
                }

                playerHealth.Heal(HealAmount);
                Destroy(gameObject);
            }
        }

    }
}
