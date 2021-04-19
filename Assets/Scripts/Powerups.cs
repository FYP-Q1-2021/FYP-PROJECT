using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Powerup
{
    DAMAGE,
    MOVEMENT_SPEED,
};


public class Powerups : MonoBehaviour
{
    public Powerup powerupType;
    public float powerUpStrength;
    public float powerUpDuration;
    //Renderer renderer;
    Collider collider;

    void Start()
    {
        //renderer = GetComponent<Renderer>();
        collider = GetComponent<Collider>();
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Yeap");
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
            //renderer.enabled = false;
            this.enabled = false;

            yield return new WaitForSeconds(powerUpDuration);
            playerCharacterController.Buff(powerupType, false);

            Destroy(gameObject);

        }
    }
}
