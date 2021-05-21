using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowText : MonoBehaviour
{

    public Text ammo;
    float playerAmmo;
    float playerMaxAmmo;

    public PlayerWeaponsManager playerWeaponsManager;
    // Start is called before the first frame update
    void Start()
    {
        playerWeaponsManager = GameObject.Find("Player").GetComponent<PlayerWeaponsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        WeaponController weaponinstance = playerWeaponsManager.GetActiveWeapon();

        playerAmmo = weaponinstance.m_CurrentAmmo;
        playerMaxAmmo = weaponinstance.MaxAmmo;

        ammo.text = "Current Ammo: " + playerAmmo.ToString();
    }
}
