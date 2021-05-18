using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [Header("Parameters")]
    [Tooltip("Effect played when picked up")]
    public GameObject pickupVFX;
    [Tooltip("Eound played when picked up")]
    public AudioClip pickupSFX;
    public float pickupVFXDuration = 5f;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerCharacterController>().hasKey = true;
        
            if (pickupSFX)
            {
                AudioUtility.CreateSFX(pickupSFX, transform.position, AudioUtility.AudioGroups.Pickup, 0f);
            }

               

            if (pickupVFX)
            {
                GameObject pickupVFXInstance = Instantiate(pickupVFX, gameObject.transform.position, pickupVFX.transform.rotation);
                pickupVFXInstance.transform.SetParent(null);
                pickupVFXInstance.SetActive(true);
                Destroy(pickupVFXInstance, pickupVFXDuration);
            }

            Destroy(gameObject);
        }

    }
}
