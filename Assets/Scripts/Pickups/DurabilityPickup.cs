using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurabilityPickup : MonoBehaviour
{
    [Header("Parameters")]
    [Tooltip("Amount of durability to restore on pickup")]
    public float restoreAmount = 40f;
    [Tooltip("Effect played when picked up")]
    public GameObject pickupVFX;
    [Tooltip("Eound played when picked up")]
    public AudioClip pickupSFX;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerWeaponsManager weaponManager = GameObject.Find("Player").GetComponent<PlayerWeaponsManager>();
            WeaponController weaponInstance = weaponManager.GetActiveWeapon();
            if (weaponInstance.hasDurability && weaponInstance.canRestoreDurability)
            {
                // play shoot SFX
                if (pickupSFX)
                {
                    AudioUtility.CreateSFX(pickupSFX, transform.position, AudioUtility.AudioGroups.Pickup, 0f);
                }

                weaponInstance.RestoreDurability(restoreAmount);
                Destroy(gameObject);
            }
        }
    }
}
