using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{

    public Transform defaultWeaponPosition;
    public Transform aDSWeaponPosition;
    public Transform downWeaponPosition;
    public Transform weaponParentSocket;

    WeaponController[] weaponSlots = new WeaponController[9]; // 9 available weapon slots

    void SwitchWeapon()
    {
        // Switch to the next availiable weapon

    }
    public bool AddWeapon(WeaponController weaponPrefab)
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] == null)
            {
                // spawn the weapon prefab as child of the weapon socket
                WeaponController weaponInstance = Instantiate(weaponPrefab, weaponParentSocket);
                weaponInstance.transform.localPosition = Vector3.zero;
                weaponInstance.transform.localRotation = Quaternion.identity;

                return true;
            }
        }

        if(GetActiveWeapon() == null)
        {
            SwitchWeapon();
        }
        return false;
    }

    void RemoveWeapon()
    {

    }
}
