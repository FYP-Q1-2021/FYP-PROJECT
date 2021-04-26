using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Powerup
{
    DAMAGE,
    MOVEMENT_SPEED,
    INVINCIBLE,
};


public class Powerups : MonoBehaviour
{
    public Powerup powerupType;
    public float powerUpDuration;
    Renderer renderer;
    Collider collider;

    [Tooltip("Effect played when picked up")]
    public GameObject pickupVFX;
    [Tooltip("Eound played when picked up")]
    public AudioClip pickupSFX;

    void Start()    
    {
        renderer = GetComponent<Renderer>();
        collider = GetComponent<Collider>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(PickUp(other));
        }
    }


    IEnumerator PickUp(Collider other)
    {
        PlayerCharacterController playerCharacterController = GameObject.Find("Player").GetComponent<PlayerCharacterController>();
        if (playerCharacterController != null)
        {
            playerCharacterController.Buff(powerupType, true);

            collider.enabled = false;
            renderer.enabled = false;
            this.enabled = false;

            // play shoot SFX
            if (pickupSFX)
            {
                AudioUtility.CreateSFX(pickupSFX, transform.position, AudioUtility.AudioGroups.Pickup, 0f);
            }

            yield return new WaitForSeconds(powerUpDuration);
            playerCharacterController.Buff(powerupType, false);

            Destroy(gameObject);

        }
    }
}
