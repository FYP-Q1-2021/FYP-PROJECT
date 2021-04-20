using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurabilityPickup : MonoBehaviour
{
    [Header("Parameters")]
    [Tooltip("Amount of durability to restore on pickup")]
    public float restoreAmount = 40f;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerWeaponsManager weaponManager = GameObject.Find("Player").GetComponent<PlayerWeaponsManager>();
            WeaponController weaponInstance = weaponManager.GetActiveWeapon();
            if (weaponInstance.hasDurability && weaponInstance.canRestoreDurability)
            {
                weaponInstance.RestoreDurability(restoreAmount);
                Destroy(gameObject);
            }
        }
    }
}
