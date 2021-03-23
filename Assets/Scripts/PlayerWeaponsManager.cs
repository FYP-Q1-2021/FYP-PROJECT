using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{

    public Transform DefaultWeaponPosition;
    public Transform ADSWeaponPosition;
    public Transform DownWeaponPosition;
    public Transform WeaponParentSocket;

    WeaponController[] weaponSlots = new WeaponController[9]; // 9 available weapon slots

    void AddWeapon()
    {
        for(int i = 0;i < weaponSlots.Length; i++)
        {

        }
    }

    void RemoveWeapon()
    {

    }
}
