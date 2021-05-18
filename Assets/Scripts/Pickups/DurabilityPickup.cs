using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurabilityPickup : MonoBehaviour
{
    [Header("Parameters")]
    public float ammoToReplenish = 50;
    [Tooltip("Amount of durability to restore on pickup")]
    public float restoreAmount = 40f;
    [Tooltip("Effect played when picked up")]
    public GameObject pickupVFX;
    [Tooltip("Sound played when picked up")]
    public AudioClip pickupSFX;
    public float pickupVFXDuration = 1.5f;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerWeaponsManager weaponManager = GameObject.Find("Player").GetComponent<PlayerWeaponsManager>();
            WeaponController weaponInstance = weaponManager.GetActiveWeapon();
            if ((weaponInstance.hasDurability && weaponInstance.canRestoreDurability))
            {
                weaponInstance.RestoreDurability(restoreAmount);
                Destroy(gameObject);
            }

            if(weaponInstance.weaponType == WeaponType.RANGED)
            {
                weaponInstance.MaxAmmo += ammoToReplenish;
            }

            Debug.Log("Passed");
            // play shoot SFX
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
        }
    }
}
