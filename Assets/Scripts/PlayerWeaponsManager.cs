﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsManager : MonoBehaviour
{

    [Tooltip("List of weapon the player will start with")]
    public List<WeaponController> startingWeapons = new List<WeaponController>();

    public Transform defaultWeaponPosition;
    public Transform aDSWeaponPosition;
    public Transform downWeaponPosition;
    public Transform weaponParentSocket;
    Vector3 weaponParentOrigin;
    PlayerCharacterController playerCharacterController;
    InputHandler inputHandler;

    [Header("Weapon Bob")]
    [Range(0f,1f)][Tooltip("Intensity of weapon bob when player is still")]
    public float idleWeaponBobIntensity;
    [Range(0f, 15f)]
    [Tooltip("Smoothness of weapon bob")]
    public float idleWeaponBobSmoothness;

    [Range(0f, 1f)]
    [Tooltip("Intensity of weapon bob when player is still")]
    public float movingWeaponBobIntensity;
    [Range(0f, 15f)]
    [Tooltip("Smoothness of weapon bob")]
    public float movingWeaponBobSmoothness;

    Vector3 targetWeaponBobPosition;
    float idleCounter;
    float movementCounter;
    WeaponController[] weaponSlots = new WeaponController[9]; // 9 available weapon slots

    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        playerCharacterController = GetComponent<PlayerCharacterController>();
        weaponParentOrigin = weaponParentSocket.localPosition;

        foreach (var weapon in startingWeapons)
        {
            AddWeapon(weapon);
        }
        SwitchWeapon(true);
    }

    void Update()
    {
        
    }

    void LateUpdate()
    {
        Vector3 moveInput = inputHandler.GetMoveInput();
        if (moveInput.x == 0 && moveInput.z == 0)
        {
            UpdateWeaponBob(idleCounter, idleWeaponBobIntensity, idleWeaponBobIntensity);
            idleCounter += Time.deltaTime;
            weaponParentSocket.localPosition = Vector3.Lerp(weaponParentSocket.localPosition, targetWeaponBobPosition, Time.deltaTime * idleWeaponBobSmoothness);
        }
        else
        {
            UpdateWeaponBob(movementCounter, movingWeaponBobIntensity, movingWeaponBobIntensity);
            movementCounter += Time.deltaTime * 2f;
            weaponParentSocket.localPosition = Vector3.Lerp(weaponParentSocket.localPosition, targetWeaponBobPosition, Time.deltaTime * movingWeaponBobSmoothness);
        }
    }

    void UpdateWeaponBob(float x, float xIntensity, float yIntensity)
    {
        targetWeaponBobPosition = weaponParentOrigin + new Vector3(Mathf.Cos(x) * xIntensity, Mathf.Sin(x * 2) * yIntensity , 0);
    }

    void SwitchWeapon(bool ascendingOrder)
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
            SwitchWeapon(true);
        }
        return false;
    }

    WeaponController GetActiveWeapon()
    {
        return GetWeaponAtSlotIndex(0);
    }

    WeaponController GetWeaponAtSlotIndex(int index)
    {

        return null;
    }


    void RemoveWeapon()
    {

    }
}
